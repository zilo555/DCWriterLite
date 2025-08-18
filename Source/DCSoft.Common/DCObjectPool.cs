

using Microsoft.VisualBasic.FileIO;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace DCSoft.Common
{
    /// <summary>
    /// 对象缓存池
    /// </summary>
    internal static class DCObjectPool<T>
    {
        private static T[] _Items = Array.Empty<T>();
        private static int _Count = 0;
        public static System.Action<T> ResetHandler = null;

        public static void Return(T item)
        {
            if (item == null)
            {
                ArgumentNullException.ThrowIfNull( item , "item");
            }
            if (_Items.Length < _Count + 1)
            {
                var items2 = new T[_Count + 5];
                Array.Copy(_Items, items2, _Count);
                Array.Clear(_Items);
                _Items = items2;
            }
            _Items[_Count++] = item;
            if(ResetHandler != null )
            {
                ResetHandler(item);
            }
        }
        public static void ReturnRange(IList<T> items)
        {
            if (items == null)
            {
                ArgumentNullException.ThrowIfNull(items, "items");
            }
            if (_Items.Length < _Count + items.Count)
            {
                var items2 = new T[_Count + items.Count + 10];
                Array.Copy(_Items, items2, _Count);
                Array.Clear(_Items);
                _Items = items2;
            }
            for (var i = 0; i < items.Count; i++)
            {
                _Items[_Count + i] = items[i];
                if (ResetHandler != null)
                {
                    ResetHandler(items[i]);
                }
            }
            _Count += items.Count;
        }

        public static void Clear( int maxCount = 0)
        {
            if( _Count < maxCount )
            {
                return;
            }
            for (var iCount = 0; iCount < _Count; iCount++)
            {
                var item = _Items[iCount];
                if (item is IDisposable)
                {
                    ((IDisposable)item).Dispose();
                }
            }
            Array.Clear(_Items);
            _Items = Array.Empty<T>();
            _Count = 0;
        }
    }
}
