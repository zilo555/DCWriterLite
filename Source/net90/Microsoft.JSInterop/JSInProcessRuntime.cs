#if NET9_0
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using static Microsoft.AspNetCore.Internal.LinkerFlags;
namespace Microsoft.JSInterop;
[System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
public abstract class JSInProcessRuntime : JSRuntime, IJSInProcessRuntime
{
    //[RequiresUnreferencedCode("JSON serialization and deserialization might require types that cannot be statically analyzed.")]
    internal TValue Invoke<[DynamicallyAccessedMembers(JsonSerialized)] TValue>(string identifier, long targetInstanceId, params object?[]? args)
    {
        var resultJson = InvokeJS(
            identifier,
            JsonSerializer.Serialize(args, JsonSerializerOptions),
            JSCallResultTypeHelper.FromGeneric<TValue>(),
            targetInstanceId);
        if (resultJson is null)
        {
            return default!;
        }

        var result = JsonSerializer.Deserialize<TValue>(resultJson, JsonSerializerOptions)!;
        ByteArraysToBeRevived.Clear();
        return result;
    }

    //[RequiresUnreferencedCode("JSON serialization and deserialization might require types that cannot be statically analyzed.")]
    public TValue Invoke<[DynamicallyAccessedMembers(JsonSerialized)] TValue>(string identifier, params object?[]? args)
        => Invoke<TValue>(identifier, 0, args);
    protected abstract string? InvokeJS(string identifier, [StringSyntax(StringSyntaxAttribute.Json)] string? argsJson, JSCallResultType resultType, long targetInstanceId);
}
#endif