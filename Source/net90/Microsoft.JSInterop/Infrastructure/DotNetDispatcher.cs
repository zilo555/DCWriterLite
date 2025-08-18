#if NET9_0
using System.Buffers;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Reflection.Metadata;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Internal;
using static Microsoft.AspNetCore.Internal.LinkerFlags;

[assembly: MetadataUpdateHandler(typeof(Microsoft.JSInterop.Infrastructure.DotNetDispatcher.MetadataUpdateHandler))]

namespace Microsoft.JSInterop.Infrastructure;
[System.Reflection.Obfuscation(Exclude = true , ApplyToMembers = true )]
public static class DotNetDispatcher
{
    private const string DisposeDotNetObjectReferenceMethodName = "__Dispose";

    internal static readonly JsonEncodedText DotNetObjectRefKey = JsonEncodedText.Encode("__dotNetObject");

    private static readonly ConcurrentDictionary<AssemblyKey, IReadOnlyDictionary<string, (MethodInfo, Type[])>> _cachedMethodsByAssembly = new();

    private static readonly ConcurrentDictionary<Type, IReadOnlyDictionary<string, (MethodInfo, Type[])>> _cachedMethodsByType = new();

    private static readonly ConcurrentDictionary<Type, Func<object, Task>> _cachedConvertToTaskByType = new();

    private static readonly MethodInfo _taskConverterMethodInfo = typeof(DotNetDispatcher).GetMethod(nameof(CreateValueTaskConverter), BindingFlags.NonPublic | BindingFlags.Static)!;

    //[UnconditionalSuppressMessage("Trimming", "IL2026", Justification = "We expect application code is configured to ensure return types of JSInvokable methods are retained.")]
    public static string? Invoke(JSRuntime jsRuntime, in DotNetInvocationInfo invocationInfo, [StringSyntax(StringSyntaxAttribute.Json)] string argsJson)
    {
        IDotNetObjectReference? targetInstance = default;
        if (invocationInfo.DotNetObjectId != default)
        {
            targetInstance = jsRuntime.GetObjectReference(invocationInfo.DotNetObjectId);
        }

        var syncResult = InvokeSynchronously(jsRuntime, invocationInfo, targetInstance, argsJson);
        if (syncResult == null)
        {
            return null;
        }

        return JsonSerializer.Serialize(syncResult, jsRuntime.JsonSerializerOptions);
    }

    //[UnconditionalSuppressMessage("Trimming", "IL2026", Justification = "We expect application code is configured to ensure return types of JSInvokable methods are retained.")]
    public static void BeginInvokeDotNet(JSRuntime jsRuntime, DotNetInvocationInfo invocationInfo, [StringSyntax(StringSyntaxAttribute.Json)] string argsJson)
    {
        var callId = invocationInfo.CallId;

        object? syncResult = null;
        ExceptionDispatchInfo? syncException = null;
        IDotNetObjectReference? targetInstance = null;
        try
        {
            if (invocationInfo.DotNetObjectId != default)
            {
                targetInstance = jsRuntime.GetObjectReference(invocationInfo.DotNetObjectId);
            }

            syncResult = InvokeSynchronously(jsRuntime, invocationInfo, targetInstance, argsJson);
        }
        catch (Exception ex)
        {
            syncException = ExceptionDispatchInfo.Capture(ex);
        }

        if (callId == null)
        {
            return;
        }
        else if (syncException != null)
        {
            jsRuntime.EndInvokeDotNet(invocationInfo, new DotNetInvocationResult(syncException.SourceException, "InvocationFailure"));
        }
        else if (syncResult is Task task)
        {
            task.ContinueWith(t => EndInvokeDotNetAfterTask(t, jsRuntime, invocationInfo), TaskScheduler.Current);

        }
        else if (syncResult is ValueTask valueTaskResult)
        {
            valueTaskResult.AsTask().ContinueWith(t => EndInvokeDotNetAfterTask(t, jsRuntime, invocationInfo), TaskScheduler.Current);
        }
        else if (syncResult?.GetType() is { IsGenericType: true } syncResultType
            && syncResultType.GetGenericTypeDefinition() == typeof(ValueTask<>))
        {
            var innerTask = GetTaskByType(syncResultType.GenericTypeArguments[0], syncResult);

            innerTask!.ContinueWith(t => EndInvokeDotNetAfterTask(t, jsRuntime, invocationInfo), TaskScheduler.Current);
        }
        else
        {
            var syncResultJson = JsonSerializer.Serialize(syncResult, jsRuntime.JsonSerializerOptions);
            var dispatchResult = new DotNetInvocationResult(syncResultJson);
            jsRuntime.EndInvokeDotNet(invocationInfo, dispatchResult);
        }
    }

    //[UnconditionalSuppressMessage("Trimming", "IL2026", Justification = "We expect application code is configured to ensure return types of JSInvokable methods are retained.")]
    private static void EndInvokeDotNetAfterTask(Task task, JSRuntime jsRuntime, in DotNetInvocationInfo invocationInfo)
    {
        if (task.Exception != null)
        {
            var exceptionDispatchInfo = ExceptionDispatchInfo.Capture(task.Exception.GetBaseException());
            var dispatchResult = new DotNetInvocationResult(exceptionDispatchInfo.SourceException, "InvocationFailure");
            jsRuntime.EndInvokeDotNet(invocationInfo, dispatchResult);
        }

        var result = TaskGenericsUtil.GetTaskResult(task);
        var resultJson = JsonSerializer.Serialize(result, jsRuntime.JsonSerializerOptions);
        jsRuntime.EndInvokeDotNet(invocationInfo, new DotNetInvocationResult(resultJson));
    }

    private static object? InvokeSynchronously(JSRuntime jsRuntime, in DotNetInvocationInfo callInfo, IDotNetObjectReference? objectReference, string argsJson)
    {
        var assemblyName = callInfo.AssemblyName;
        var methodIdentifier = callInfo.MethodIdentifier;

        AssemblyKey assemblyKey;
        MethodInfo methodInfo;
        Type[] parameterTypes;
        if (objectReference is null)
        {
            assemblyKey = new AssemblyKey(assemblyName!);
            (methodInfo, parameterTypes) = GetCachedMethodInfo(assemblyKey, methodIdentifier);
        }
        else
        {
            if (assemblyName != null)
            {
                throw new ArgumentException($"For instance method calls, '{nameof(assemblyName)}' should be null. Value received: '{assemblyName}'.");
            }

            if (string.Equals(DisposeDotNetObjectReferenceMethodName, methodIdentifier, StringComparison.Ordinal))
            {
                // The client executed dotNetObjectReference.dispose(). Dispose the reference and exit.
                objectReference.Dispose();
                return default;
            }

            (methodInfo, parameterTypes) = GetCachedMethodInfo(objectReference, methodIdentifier);
        }

        var suppliedArgs = ParseArguments(jsRuntime, methodIdentifier, argsJson, parameterTypes);

        try
        {
            // objectReference will be null if this call invokes a static JSInvokable method.
            return methodInfo.Invoke(objectReference?.Value, suppliedArgs);
        }
        catch (TargetInvocationException tie) // Avoid using exception filters for AOT runtime support
        {
            if (tie.InnerException != null)
            {
                ExceptionDispatchInfo.Capture(tie.InnerException).Throw();
                throw tie.InnerException; // Unreachable
            }

            throw;
        }
        finally
        {
            jsRuntime.ByteArraysToBeRevived.Clear();
        }
    }

    //[UnconditionalSuppressMessage("Trimming", "IL2026", Justification = "We expect application code is configured to ensure return types of JSInvokable methods are retained.")]
    internal static object?[] ParseArguments(JSRuntime jsRuntime, string methodIdentifier, string arguments, Type[] parameterTypes)
    {
        if (parameterTypes.Length == 0)
        {
            return Array.Empty<object>();
        }

        var count = Encoding.UTF8.GetByteCount(arguments);
        var buffer = ArrayPool<byte>.Shared.Rent(count);
        try
        {
            var receivedBytes = Encoding.UTF8.GetBytes(arguments, buffer);
            Debug.Assert(count == receivedBytes);

            var reader = new Utf8JsonReader(buffer.AsSpan(0, count));
            if (!reader.Read() || reader.TokenType != JsonTokenType.StartArray)
            {
                throw new JsonException("Invalid JSON");
            }

            var suppliedArgs = new object?[parameterTypes.Length];

            var index = 0;
            while (index < parameterTypes.Length && reader.Read() && reader.TokenType != JsonTokenType.EndArray)
            {
                var parameterType = parameterTypes[index];
                if (reader.TokenType == JsonTokenType.StartObject && IsIncorrectDotNetObjectRefUse(parameterType, reader))
                {
                    throw new InvalidOperationException($"In call to '{methodIdentifier}', parameter of type '{parameterType.Name}' at index {(index + 1)} must be declared as type 'DotNetObjectRef<{parameterType.Name}>' to receive the incoming value.");
                }

                suppliedArgs[index] = JsonSerializer.Deserialize(ref reader, parameterType, jsRuntime.JsonSerializerOptions);
                index++;
            }

            if (index < parameterTypes.Length)
            {
                // If we parsed fewer parameters, we can always make a definitive claim about how many parameters were received.
                throw new ArgumentException($"The call to '{methodIdentifier}' expects '{parameterTypes.Length}' parameters, but received '{index}'.");
            }

            if (!reader.Read() || reader.TokenType != JsonTokenType.EndArray)
            {
                // Either we received more parameters than we expected or the JSON is malformed.
                throw new JsonException($"Unexpected JSON token {reader.TokenType}. Ensure that the call to `{methodIdentifier}' is supplied with exactly '{parameterTypes.Length}' parameters.");
            }

            return suppliedArgs;

            // Note that the JsonReader instance is intentionally not passed by ref (or an in parameter) since we want a copy of the original reader.
            static bool IsIncorrectDotNetObjectRefUse(Type parameterType, Utf8JsonReader jsonReader)
            {
                // Check for incorrect use of DotNetObjectRef<T> at the top level. We know it's
                // an incorrect use if there's a object that looks like { '__dotNetObject': <some number> },
                // but we aren't assigning to DotNetObjectRef{T}.
                if (jsonReader.Read() &&
                    jsonReader.TokenType == JsonTokenType.PropertyName &&
                    jsonReader.ValueTextEquals(DotNetObjectRefKey.EncodedUtf8Bytes))
                {
                    // The JSON payload has the shape we expect from a DotNetObjectRef instance.
                    return !parameterType.IsGenericType || parameterType.GetGenericTypeDefinition() != typeof(DotNetObjectReference<>);
                }

                return false;
            }
        }
        finally
        {
            ArrayPool<byte>.Shared.Return(buffer);
        }
    }

    public static void EndInvokeJS(JSRuntime jsRuntime, string arguments)
    {
        var utf8JsonBytes = Encoding.UTF8.GetBytes(arguments);

        var reader = new Utf8JsonReader(utf8JsonBytes);

        if (!reader.Read() || reader.TokenType != JsonTokenType.StartArray)
        {
            throw new JsonException("Invalid JSON");
        }

        reader.Read();
        var taskId = reader.GetInt64();

        reader.Read();
        var success = reader.GetBoolean();

        reader.Read();
        if (!jsRuntime.EndInvokeJS(taskId, success, ref reader))
        {
            return;
        }

        if (!reader.Read() || reader.TokenType != JsonTokenType.EndArray)
        {
            throw new JsonException("Invalid JSON");
        }
    }

    public static void ReceiveByteArray(JSRuntime jsRuntime, int id, byte[] data)
    {
        jsRuntime.ReceiveByteArray(id, data);
    }

    private static (MethodInfo, Type[]) GetCachedMethodInfo(AssemblyKey assemblyKey, string methodIdentifier)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(assemblyKey.AssemblyName);
        ArgumentException.ThrowIfNullOrWhiteSpace(methodIdentifier);

        var assemblyMethods = _cachedMethodsByAssembly.GetOrAdd(assemblyKey, ScanAssemblyForCallableMethods);
        if (assemblyMethods.TryGetValue(methodIdentifier, out var result))
        {
            return result;
        }
        else
        {
            throw new ArgumentException($"The assembly '{assemblyKey.AssemblyName}' does not contain a public invokable method with [{nameof(JSInvokableAttribute)}(\"{methodIdentifier}\")].");
        }
    }

    //[UnconditionalSuppressMessage(
    //    "ReflectionAnalysis",
    //    "IL2060:MakeGenericMethod",
    //    Justification = "https://github.com/mono/linker/issues/1727")]
    private static Task GetTaskByType(Type type, object obj)
    {
        var converterDelegate = _cachedConvertToTaskByType.GetOrAdd(type, (Type t, MethodInfo taskConverterMethodInfo) =>
            taskConverterMethodInfo.MakeGenericMethod(t).CreateDelegate<Func<object, Task>>(), _taskConverterMethodInfo);

        return converterDelegate.Invoke(obj);
    }

    private static Task CreateValueTaskConverter<[DynamicallyAccessedMembers(LinkerFlags.JsonSerialized)] T>(object result) => ((ValueTask<T>)result).AsTask();

    private static bool CheckMethod(MethodInfo m)
    {
        if (m == null)
        {
            throw new ArgumentNullException("m");
        }
        if (m.ContainsGenericParameters)
        {
            return false;
        }
        if (m.IsDefined(typeof(JSInvokableAttribute), false))
        {
            return true;
        }
        return false;
    }


    private static (MethodInfo methodInfo, Type[] parameterTypes) GetCachedMethodInfo(IDotNetObjectReference objectReference, string methodIdentifier)
    {
        var type = objectReference.Value.GetType();

        var assemblyMethods = _cachedMethodsByType.GetOrAdd(type, ScanTypeForCallableMethods);
        if (assemblyMethods.TryGetValue(methodIdentifier, out var result))
        {
            return result;
        }
        else
        {
            throw new ArgumentException($"The type '{type.Name}' does not contain a public invokable method with [{nameof(JSInvokableAttribute)}(\"{methodIdentifier}\")].");
        }

        static Dictionary<string, (MethodInfo, Type[])> ScanTypeForCallableMethods([DynamicallyAccessedMembers(JSInvokable)] Type type)
        {
            var result = new Dictionary<string, (MethodInfo, Type[])>(StringComparer.Ordinal);

            foreach (var method in type.GetMethods(BindingFlags.Instance | BindingFlags.Public))
            {
                if (CheckMethod(method) == false) continue;
                //if (method.ContainsGenericParameters || !method.IsDefined(typeof(JSInvokableAttribute), inherit: false))
                //{
                //    continue;
                //}

                var identifier = method.Name;// method.GetCustomAttribute<JSInvokableAttribute>(false)!.Identifier ?? method.Name!;
                var parameterTypes = GetParameterTypes(method);

                if (result.ContainsKey(identifier))
                {
                    throw new InvalidOperationException($"The type {type.Name} contains more than one " +
                        $"[JSInvokable] method with identifier '{identifier}'. All [JSInvokable] methods within the same " +
                        "type must have different identifiers. You can pass a custom identifier as a parameter to " +
                        $"the [JSInvokable] attribute.");
                }

                result.Add(identifier, (method, parameterTypes));
            }

            return result;
        }
    }

    //[UnconditionalSuppressMessage("ReflectionAnalysis", "IL2026", Justification = "We expect application code is configured to ensure JSInvokable methods are retained. https://github.com/dotnet/aspnetcore/issues/29946")]
    //[UnconditionalSuppressMessage("ReflectionAnalysis", "IL2072", Justification = "We expect application code is configured to ensure JSInvokable methods are retained. https://github.com/dotnet/aspnetcore/issues/29946")]
    //[UnconditionalSuppressMessage("Trimming", "IL2075", Justification = "We expect application code is configured to ensure JSInvokable methods are retained. https://github.com/dotnet/aspnetcore/issues/29946")]
    private static Dictionary<string, (MethodInfo, Type[])> ScanAssemblyForCallableMethods(AssemblyKey assemblyKey)
    {
        var result = new Dictionary<string, (MethodInfo, Type[])>(StringComparer.Ordinal);
        var exportedTypes = GetRequiredLoadedAssembly(assemblyKey).GetExportedTypes();
        foreach (var type in exportedTypes)
        {
            foreach (var method in type.GetMethods(BindingFlags.Public | BindingFlags.Static))
            {
                if( CheckMethod(method) == false) continue;
                //if (method.ContainsGenericParameters || !method.IsDefined(typeof(JSInvokableAttribute), inherit: false))
                //{
                //    continue;
                //}

                var identifier = method.Name;// method.GetCustomAttribute<JSInvokableAttribute>(false)!.Identifier ?? method.Name;
                var parameterTypes = GetParameterTypes(method);

                if (result.ContainsKey(identifier))
                {
                    throw new InvalidOperationException($"The assembly '{assemblyKey.AssemblyName}' contains more than one " +
                        $"[JSInvokable] method with identifier '{identifier}'. All [JSInvokable] methods within the same " +
                        $"assembly must have different identifiers. You can pass a custom identifier as a parameter to " +
                        $"the [JSInvokable] attribute.");
                }

                result.Add(identifier, (method, parameterTypes));
            }
        }

        return result;
    }

    private static Type[] GetParameterTypes(MethodInfo method)
    {
        var parameters = method.GetParameters();
        if (parameters.Length == 0)
        {
            return Type.EmptyTypes;
        }

        var parameterTypes = new Type[parameters.Length];
        for (var i = 0; i < parameters.Length; i++)
        {
            parameterTypes[i] = parameters[i].ParameterType;
        }

        return parameterTypes;
    }

    private static Assembly GetRequiredLoadedAssembly(AssemblyKey assemblyKey)
    {
        Assembly? assembly = null;
        foreach (Assembly a in AppDomain.CurrentDomain.GetAssemblies())
        {
            if (new AssemblyKey(a).Equals(assemblyKey))
            {
                assembly = a;
            }
        }

        return assembly
            ?? throw new ArgumentException($"There is no loaded assembly with the name '{assemblyKey.AssemblyName}'.");
    }

    // DynamicallyAccessedMemberTypes.All. This causes unnecessary trim warnings on the non-MetadataUpdateHandler methods.
    [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
    internal static class MetadataUpdateHandler
    {
        public static void ClearCache(Type[]? _)
        {
            _cachedMethodsByAssembly.Clear();
            _cachedMethodsByType.Clear();
            _cachedConvertToTaskByType.Clear();
        }
    }

    private readonly struct AssemblyKey : IEquatable<AssemblyKey>
    {
        public AssemblyKey(Assembly assembly)
        {
            Assembly = assembly;
            AssemblyName = assembly.GetName().Name!;
        }

        public AssemblyKey(string assemblyName)
        {
            Assembly = null;
            AssemblyName = assemblyName;
        }

        public Assembly? Assembly { get; }

        public string AssemblyName { get; }

        public bool Equals(AssemblyKey other)
        {
            if (Assembly != null && other.Assembly != null)
            {
                return Assembly == other.Assembly;
            }

            return AssemblyName.Equals(other.AssemblyName, StringComparison.Ordinal);
        }

        public override int GetHashCode() => StringComparer.Ordinal.GetHashCode(AssemblyName);
    }
}
#endif