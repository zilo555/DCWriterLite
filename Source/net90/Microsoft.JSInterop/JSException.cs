#if NET9_0
namespace Microsoft.JSInterop;
public class JSException : Exception
{
    public JSException(string message) : base(message)
    {
    }
    public JSException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
#endif