using System;
using System.Collections.Generic;

namespace DCSoft.Common
{
    public class LoaderListBuffer<T>
    {
        public static LoaderListBuffer<T> Instance = new LoaderListBuffer<T>();

        private T[] _Items = Array.Empty<T>();
        private int _Length = 0;
        public delegate T CreateHandler();
        private CreateHandler _CreateHandler;
        public void SetCreator(CreateHandler creator)
        {
            this._CreateHandler = creator;
        }
        public T Alloc()
        {
            if (this._Length > 0)
            {
                var item = this._Items[this._Length - 1];
                this._Length--;
                return item;
            }
            return this._CreateHandler();
        }

        public void Return(T item)
        {
            if (this._Length >= this._Items.Length)
            {
                var newItems = new T[this._Items.Length + 4];
                if (this._Length > 0)
                {
                    Array.Copy(this._Items, 0, newItems, 0, this._Length);
                    Array.Clear(this._Items, 0, this._Length);
                }
                this._Items = newItems;
            }
            this._Items[this._Length++] = item;
        }

        public void Clear(System.Action<T> handler)
        {
            if (this._Items.Length > 0)
            {
                if (handler != null)
                {
                    foreach (var item in this._Items)
                    {
                        if (item != null)
                        {
                            handler(item);
                        }
                    }
                }
                else if (this._Items[0] is System.Array)
                {
                    foreach (var item in this._Items)
                    {
                        if (item is System.Array arr2)
                        {
                            if (arr2.Length > 0)
                            {
                                Array.Clear(arr2, 0, arr2.Length);
                            }
                        }
                    }
                }
                Array.Clear(this._Items, 0, this._Length);
                this._Items = Array.Empty<T>();
                this._Length = 0;
            }
        }
    }
}
