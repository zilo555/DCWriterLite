#if NET9_0


using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using Microsoft.JSInterop.Infrastructure;
using WebAssembly.JSInterop;

namespace Microsoft.JSInterop.WebAssembly;

[System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
public abstract class WebAssemblyJSRuntime : JSInProcessRuntime
{
    protected WebAssemblyJSRuntime()
    {
        //JsonSerializerOptions.Converters.Insert(0, new WebAssemblyJSObjectReferenceJsonConverter(this));
    }

    protected override string InvokeJS(string identifier, [StringSyntax(StringSyntaxAttribute.Json)] string? argsJson, JSCallResultType resultType, long targetInstanceId)
    {
        try
        {
            return InternalCalls.InvokeJSJson(identifier, targetInstanceId, (int)resultType, argsJson ?? "[]", 0);
        }
        catch (Exception ex)
        {
            throw new JSException(ex.Message, ex);
        }
    }

    protected override void BeginInvokeJS(long asyncHandle, string identifier, [StringSyntax(StringSyntaxAttribute.Json)] string? argsJson, JSCallResultType resultType, long targetInstanceId)
    {
        InternalCalls.InvokeJSJson(identifier, targetInstanceId, (int)resultType, argsJson ?? "[]", asyncHandle);
    }

    //[UnconditionalSuppressMessage("ReflectionAnalysis", "IL2026:RequiresUnreferencedCode", Justification = "TODO: This should be in the xml suppressions file, but can't be because https://github.com/mono/linker/issues/2006")]
    protected internal override void EndInvokeDotNet(DotNetInvocationInfo callInfo, in DotNetInvocationResult dispatchResult)
    {
        var resultJsonOrErrorMessage = dispatchResult.Success
            ? dispatchResult.ResultJson!
            : dispatchResult.Exception!.ToString();
        InternalCalls.EndInvokeDotNetFromJS(callInfo.CallId, dispatchResult.Success, resultJsonOrErrorMessage);
    }

    protected internal override void SendByteArray(int id, byte[] data)
    {
        InternalCalls.ReceiveByteArray(id, data);
    }
}
#endif