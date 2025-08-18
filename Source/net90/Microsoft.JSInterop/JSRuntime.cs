#if NET9_0

// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Concurrent;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using DCSoft.WASM;
using Microsoft.JSInterop.Infrastructure;
using static Microsoft.AspNetCore.Internal.LinkerFlags;

namespace Microsoft.JSInterop;

[System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
public abstract partial class JSRuntime : IJSRuntime, IDisposable
{
    private long _nextObjectReferenceId; // Initial value of 0 signals no object, but we increment prior to assignment. The first tracked object should have id 1
    private long _nextPendingTaskId = 1; // Start at 1 because zero signals "no response needed"
    private readonly ConcurrentDictionary<long, object> _pendingTasks = new();
    private readonly ConcurrentDictionary<long, IDotNetObjectReference> _trackedRefsById = new();
    private readonly ConcurrentDictionary<long, CancellationTokenRegistration> _cancellationRegistrations = new();

    internal readonly ArrayBuilder<byte[]> ByteArraysToBeRevived = new();

    /// <summary>
    /// Initializes a new instance of <see cref="JSRuntime"/>.
    /// </summary>
    protected JSRuntime()
    {
        JsonSerializerOptions = WASMJsonConvert.CreateJsonSerializerOptions(this);
        //JsonSerializerOptions = new JsonSerializerOptions
        //{
        //    MaxDepth = 32,
        //    PropertyNamingPolicy = null,
        //    PropertyNameCaseInsensitive = true,
        //    Converters =
        //        {
        //            new DotNetObjectReferenceJsonConverterFactory(this),
        //            //new JSObjectReferenceJsonConverter(this),
        //            //new JSStreamReferenceJsonConverter(this),
        //            //new DotNetStreamReferenceJsonConverter(this),
        //            new ByteArrayJsonConverter(this),
        //        }
        //};
    }

    internal JsonSerializerOptions JsonSerializerOptions { get; set; }

    protected TimeSpan? DefaultAsyncTimeout { get; set; }

    public ValueTask<TValue> InvokeAsync<[DynamicallyAccessedMembers(JsonSerialized)] TValue>(string identifier, object?[]? args)
        => InvokeAsync<TValue>(0, identifier, args);

    public ValueTask<TValue> InvokeAsync<[DynamicallyAccessedMembers(JsonSerialized)] TValue>(string identifier, CancellationToken cancellationToken, object?[]? args)
        => InvokeAsync<TValue>(0, identifier, cancellationToken, args);

    internal async ValueTask<TValue> InvokeAsync<[DynamicallyAccessedMembers(JsonSerialized)] TValue>(long targetInstanceId, string identifier, object?[]? args)
    {
        if (DefaultAsyncTimeout.HasValue)
        {
            using var cts = new CancellationTokenSource(DefaultAsyncTimeout.Value);
            // We need to await here due to the using
            return await InvokeAsync<TValue>(targetInstanceId, identifier, cts.Token, args);
        }

        return await InvokeAsync<TValue>(targetInstanceId, identifier, CancellationToken.None, args);
    }

    //[UnconditionalSuppressMessage("Trimming", "IL2026", Justification = "We expect application code is configured to ensure JS interop arguments are linker friendly.")]
    internal ValueTask<TValue> InvokeAsync<[DynamicallyAccessedMembers(JsonSerialized)] TValue>(
        long targetInstanceId,
        string identifier,
        CancellationToken cancellationToken,
        object?[]? args)
    {
        var taskId = Interlocked.Increment(ref _nextPendingTaskId);
        var tcs = new TaskCompletionSource<TValue>();
        if (cancellationToken.CanBeCanceled)
        {
            _cancellationRegistrations[taskId] = cancellationToken.Register(() =>
            {
                tcs.TrySetCanceled(cancellationToken);
                CleanupTasksAndRegistrations(taskId);
            });
        }
        _pendingTasks[taskId] = tcs;

        try
        {
            if (cancellationToken.IsCancellationRequested)
            {
                tcs.TrySetCanceled(cancellationToken);
                CleanupTasksAndRegistrations(taskId);

                return new ValueTask<TValue>(tcs.Task);
            }

            var argsJson = args is not null && args.Length != 0 ?
                JsonSerializer.Serialize(args, JsonSerializerOptions) :
                null;
            var resultType = JSCallResultTypeHelper.FromGeneric<TValue>();

            BeginInvokeJS(taskId, identifier, argsJson, resultType, targetInstanceId);

            return new ValueTask<TValue>(tcs.Task);
        }
        catch
        {
            CleanupTasksAndRegistrations(taskId);
            throw;
        }
    }

    private void CleanupTasksAndRegistrations(long taskId)
    {
        _pendingTasks.TryRemove(taskId, out _);
        if (_cancellationRegistrations.TryRemove(taskId, out var registration))
        {
            registration.Dispose();
        }
    }

    protected abstract void BeginInvokeJS(long taskId, string identifier, [StringSyntax(StringSyntaxAttribute.Json)] string? argsJson, JSCallResultType resultType, long targetInstanceId);

    protected internal abstract void EndInvokeDotNet(
        DotNetInvocationInfo invocationInfo,
        in DotNetInvocationResult invocationResult);

    protected internal virtual void SendByteArray(int id, byte[] data)
    {
        throw new NotSupportedException("JSRuntime subclasses are responsible for implementing byte array transfer to JS.");
    }

    protected internal virtual void ReceiveByteArray(int id, byte[] data)
    {
        if (id == 0)
        {
            ByteArraysToBeRevived.Clear();
        }

        if (id != ByteArraysToBeRevived.Count)
        {
            throw new ArgumentOutOfRangeException($"Element id '{id}' cannot be added to the byte arrays to be revived with length '{ByteArraysToBeRevived.Count}'.", innerException: null);
        }

        ByteArraysToBeRevived.Append(data);
    }

    //[UnconditionalSuppressMessage("ReflectionAnalysis", "IL2072:RequiresUnreferencedCode", Justification = "We enforce trimmer attributes for JSON deserialized types on InvokeAsync.")]
    //[UnconditionalSuppressMessage("Trimming", "IL2026", Justification = "We enforce trimmer attributes for JSON deserialized types on InvokeAsync.")]
    internal bool EndInvokeJS(long taskId, bool succeeded, ref Utf8JsonReader jsonReader)
    {
        if (!_pendingTasks.TryRemove(taskId, out var tcs))
        {
            return false;
        }

        CleanupTasksAndRegistrations(taskId);

        try
        {
            if (succeeded)
            {
                var resultType = TaskGenericsUtil.GetTaskCompletionSourceResultType(tcs);

                var result = JsonSerializer.Deserialize(ref jsonReader, resultType, JsonSerializerOptions);
                ByteArraysToBeRevived.Clear();
                TaskGenericsUtil.SetTaskCompletionSourceResult(tcs, result);
            }
            else
            {
                var exceptionText = jsonReader.GetString() ?? string.Empty;
                TaskGenericsUtil.SetTaskCompletionSourceException(tcs, new JSException(exceptionText));
            }

            return true;
        }
        catch (Exception exception)
        {
            var message = $"An exception occurred executing JS interop: {exception.Message}. See InnerException for more details.";
            TaskGenericsUtil.SetTaskCompletionSourceException(tcs, new JSException(message, exception));
            return false;
        }
    }


    internal long TrackObjectReference<[DynamicallyAccessedMembers(JSInvokable)] TValue>(DotNetObjectReference<TValue> dotNetObjectReference) where TValue : class
    {
        ArgumentNullException.ThrowIfNull(dotNetObjectReference);

        dotNetObjectReference.ThrowIfDisposed();

        var jsRuntime = dotNetObjectReference.JSRuntime;
        if (jsRuntime is null)
        {
            var dotNetObjectId = Interlocked.Increment(ref _nextObjectReferenceId);

            dotNetObjectReference.JSRuntime = this;
            dotNetObjectReference.ObjectId = dotNetObjectId;

            _trackedRefsById[dotNetObjectId] = dotNetObjectReference;
        }
        else if (!ReferenceEquals(this, jsRuntime))
        {
            throw new InvalidOperationException($"{dotNetObjectReference.GetType().Name} is already being tracked by a different instance of {nameof(JSRuntime)}." +
                $" A common cause is caching an instance of {nameof(DotNetObjectReference<TValue>)} globally. Consider creating instances of {nameof(DotNetObjectReference<TValue>)} at the JSInterop callsite.");
        }

        Debug.Assert(dotNetObjectReference.ObjectId != 0);
        return dotNetObjectReference.ObjectId;
    }

    internal IDotNetObjectReference GetObjectReference(long dotNetObjectId)
    {
        return _trackedRefsById.TryGetValue(dotNetObjectId, out var dotNetObjectRef)
            ? dotNetObjectRef
            : throw new ArgumentException($"There is no tracked object with id '{dotNetObjectId}'. Perhaps the DotNetObjectReference instance was already disposed.", nameof(dotNetObjectId));
    }

    internal void ReleaseObjectReference(long dotNetObjectId) => _trackedRefsById.TryRemove(dotNetObjectId, out _);

    public void Dispose() => ByteArraysToBeRevived.Dispose();
}
#endif