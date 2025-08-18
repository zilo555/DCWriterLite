#if NET9_0
namespace Microsoft.JSInterop;
public enum JSCallResultType : int
{
    Default = 0,
    JSObjectReference = 1,
    JSStreamReference = 2,
    JSVoidResult = 3,
}
#endif