#if NET9_0
namespace Microsoft.JSInterop.Infrastructure;
[System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
public readonly struct DotNetInvocationInfo
{
    public DotNetInvocationInfo(string? assemblyName, string methodIdentifier, long dotNetObjectId, string? callId)
    {
        CallId = callId;
        AssemblyName = assemblyName;
        MethodIdentifier = methodIdentifier;
        DotNetObjectId = dotNetObjectId;
    }
    public string? AssemblyName { get; }
    public string MethodIdentifier { get; }
    public long DotNetObjectId { get; }
    public string? CallId { get; }
}
#endif