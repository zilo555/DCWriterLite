#if NET9_0
using System.Diagnostics.CodeAnalysis;
using static Microsoft.AspNetCore.Internal.LinkerFlags;

namespace Microsoft.JSInterop;
[System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
public interface IJSInProcessRuntime : IJSRuntime
{
    //[RequiresUnreferencedCode("JSON serialization and deserialization might require types that cannot be statically analyzed.")]
    TResult Invoke<[DynamicallyAccessedMembers(JsonSerialized)] TResult>(string identifier, params object?[]? args);
}
#endif