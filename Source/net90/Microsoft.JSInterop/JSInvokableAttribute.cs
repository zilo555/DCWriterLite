#if NET9_0
global using Microsoft.JSInterop;

namespace Microsoft.JSInterop;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public sealed class JSInvokableAttribute : Attribute
{
    public JSInvokableAttribute()
    {
    }
}
#endif