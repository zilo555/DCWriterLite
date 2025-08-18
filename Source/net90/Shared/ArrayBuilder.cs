#if NET9_0

using System.Buffers;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Microsoft.JSInterop.Infrastructure;

internal class ArrayBuilder<T> : IDisposable
{
    protected T[] _items;
    protected int _itemsInUse;

    private static readonly T[] Empty = Array.Empty<T>();
    private readonly ArrayPool<T> _arrayPool;
    private readonly int _minCapacity;
    private bool _disposed;

    public ArrayBuilder(int minCapacity = 32, ArrayPool<T> arrayPool = null)
    {
        _arrayPool = arrayPool ?? ArrayPool<T>.Shared;
        _minCapacity = minCapacity;
        _items = Empty;
    }

    public int Count => _itemsInUse;

    public T[] Buffer => _items;

    [MethodImpl(MethodImplOptions.AggressiveInlining)] // Just like System.Collections.Generic.List<T>
    public int Append(in T item)
    {
        if (_itemsInUse == _items.Length)
        {
            GrowBuffer(_items.Length * 2);
        }

        var indexOfAppendedItem = _itemsInUse++;
        _items[indexOfAppendedItem] = item;
        return indexOfAppendedItem;
    }
    public void Clear()
    {
        ReturnBuffer();
        _items = Empty;
        _itemsInUse = 0;
    }

    protected void GrowBuffer(int desiredCapacity)
    {
        ObjectDisposedException.ThrowIf(_disposed, null);

        var newCapacity = Math.Max(desiredCapacity, _minCapacity);
        Debug.Assert(newCapacity > _items.Length);

        var newItems = _arrayPool.Rent(newCapacity);
        Array.Copy(_items, newItems, _itemsInUse);

        // Return the old buffer and start using the new buffer
        ReturnBuffer();
        _items = newItems;
    }

    private void ReturnBuffer()
    {
        if (!ReferenceEquals(_items, Empty))
        {
            Array.Clear(_items, 0, _itemsInUse);
            _arrayPool.Return(_items);
        }
    }

    public void Dispose()
    {
        if (!_disposed)
        {
            _disposed = true;
            ReturnBuffer();
            _items = Empty;
            _itemsInUse = 0;
        }
    }
}
#endif