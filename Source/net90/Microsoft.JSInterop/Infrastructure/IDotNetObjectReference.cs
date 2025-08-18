#if NET9_0
namespace Microsoft.JSInterop.Infrastructure;
[System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
internal interface IDotNetObjectReference : IDisposable
{
    object Value { get; }
}
#endif