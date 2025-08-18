#if NET9_0


using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Microsoft.JSInterop.Infrastructure;
using static Microsoft.AspNetCore.Internal.LinkerFlags;

namespace Microsoft.JSInterop;

[System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
public sealed class DotNetObjectReference<[DynamicallyAccessedMembers(JSInvokable)] TValue> :
    IDotNetObjectReference, IDisposable where TValue : class
{
    private readonly TValue _value;
    private long _objectId;
    private JSRuntime? _jsRuntime;

    internal DotNetObjectReference(TValue value)
    {
        _value = value;
    }

    public TValue Value
    {
        get
        {
            ThrowIfDisposed();
            return _value;
        }
    }

    internal long ObjectId
    {
        get
        {
            ThrowIfDisposed();
            Debug.Assert(_objectId != 0, "Accessing ObjectId without tracking is always incorrect.");

            return _objectId;
        }
        set
        {
            ThrowIfDisposed();
            _objectId = value;
        }
    }

    internal JSRuntime? JSRuntime
    {
        get
        {
            ThrowIfDisposed();
            return _jsRuntime;
        }
        set
        {
            ThrowIfDisposed();
            _jsRuntime = value;
        }
    }

    object IDotNetObjectReference.Value => Value;

    internal bool Disposed { get; private set; }

    public void Dispose()
    {
        if (!Disposed)
        {
            Disposed = true;

            _jsRuntime?.ReleaseObjectReference(_objectId);
        }
    }

    internal void ThrowIfDisposed()
    {
        ObjectDisposedException.ThrowIf(Disposed, this);
    }
}
#endif