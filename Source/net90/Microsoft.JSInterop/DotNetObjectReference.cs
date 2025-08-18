#if NET9_0
global using Microsoft.JSInterop;
using System.Diagnostics.CodeAnalysis;
using static Microsoft.AspNetCore.Internal.LinkerFlags;

namespace Microsoft.JSInterop;

[System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
public static class DotNetObjectReference
{
    public static DotNetObjectReference<TValue> Create<[DynamicallyAccessedMembers(JSInvokable)] TValue>(TValue value) where TValue : class
    {
        ArgumentNullException.ThrowIfNull(value);

        return new DotNetObjectReference<TValue>(value);
    }
}
#endif