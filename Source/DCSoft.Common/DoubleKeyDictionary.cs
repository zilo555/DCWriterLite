using System;
using System.Collections.Generic;
using System.Text;

namespace DCSoft.Common
{
    /// <summary>
    /// 双关键字的字典对象
    /// </summary>
    public class DoubleKeyDictionary<TKey1, TKey2, TValue>
    {
        public DoubleKeyDictionary()
        {
            this._Items = new Dictionary<KeyItem, TValue>(new MyComparer(this));
        }

        public bool ContainsKey(TKey1 key1, TKey2 key2)
        {
            return this._Items.ContainsKey(new KeyItem(key1, key2));
        }
        public void Add( TKey1 key1 , TKey2 key2 , TValue value )
        {
            this._Items.Add(new KeyItem(key1, key2), value);
        }
        public void Clear()
        {
            this._Items.Clear();
            this._Items = new Dictionary<KeyItem, TValue>(new MyComparer(this));
        }
        private Dictionary<KeyItem, TValue> _Items = null;
        private struct KeyItem
        {
            public KeyItem(TKey1 k1, TKey2 k2)
            {
                this.Key1 = k1;
                this.Key2 = k2;
            }
            public readonly TKey1 Key1;
            public readonly TKey2 Key2;
        }

        protected virtual bool EqualsKey(TKey1 x1, TKey2 y1, TKey1 x2, TKey2 y2)
        {
            return System.Collections.Generic.Comparer<TKey1>.Default.Compare(x1, x2) == 0
                && System.Collections.Generic.Comparer<TKey2>.Default.Compare(y1, y2) == 0;
        }

        protected virtual int GetKeyHashCode(TKey1 key1, TKey2 key2)
        {
            return key1.GetHashCode() + key2.GetHashCode();
        }
        private class MyComparer : IEqualityComparer<KeyItem>
        {
            public MyComparer(DoubleKeyDictionary<TKey1, TKey2, TValue> p)
            {
                this._Parent = p;
            }
            private DoubleKeyDictionary<TKey1, TKey2, TValue> _Parent = null;
            public bool Equals(KeyItem x, KeyItem y)
            {
                return this._Parent.EqualsKey(x.Key1, x.Key2, y.Key1, y.Key2);
            }
            public int GetHashCode(KeyItem obj)
            {
                return this._Parent.GetKeyHashCode(obj.Key1, obj.Key2);
            }
        }
    }
}