#if NET9_0
using System.Diagnostics.CodeAnalysis;
using static Microsoft.AspNetCore.Internal.LinkerFlags;
namespace Microsoft.JSInterop;
[System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
public interface IJSRuntime
{
    ValueTask<TValue> InvokeAsync<[DynamicallyAccessedMembers(JsonSerialized)] TValue>(string identifier, object?[]? args);
    ValueTask<TValue> InvokeAsync<[DynamicallyAccessedMembers(JsonSerialized)] TValue>(string identifier, CancellationToken cancellationToken, object?[]? args);
}
#endif