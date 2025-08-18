using System;

namespace DCSoft.Common
{
    /// <summary>
    /// 堆栈数据结构
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DCStack<T>
    {
        public DCStack()
        {

        }
        private static readonly T[] _emptyArray = Array.Empty<T>();
        private T[] _Items = _emptyArray;
        private int _Count = 0;
        /// <summary>
        /// 元素个数
        /// </summary>
        public int Count
        {
            get
            {
                return _Count;
            }
        }
        /// <summary>
        /// 压入堆栈
        /// </summary>
        /// <param name="item">新元素</param>
        public void Push( T item )
        {
            if( this._Count == this._Items.Length )
            {
                var temp = new T[(int)(this._Count * 1.5) + 4];
                if (this._Items.Length > 0)
                {
                    Array.Copy(this._Items, temp, this._Count);
                    Array.Clear(this._Items, 0, this._Count);
                }
                this._Items = temp;
            }
            this._Items[this._Count++] = item;
        }
        /// <summary>
        /// 弹出堆栈
        /// </summary>
        /// <returns>弹出的内容</returns>
        /// <exception cref="Exception">没有可弹出的内容了</exception>
        public T Pop()
        {
            if(this._Count == 0 )
            {
                throw new Exception("Pop()没有数据");
            }
            var index =  -- this._Count;
            var result = this._Items[index];
            this._Items[index] = default(T);
            return result;
        }
        /// <summary>
        /// 获得顶部的数据
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public T Peek()
        {
            if (this._Count == 0)
            {
                throw new Exception("Peek()没有数据");
            }
            return this._Items[this._Count - 1];
        }
        /// <summary>
        /// 清空列表
        /// </summary>
        public void Clear()
        {
            if( this._Count > 0 )
            {
                Array.Clear(this._Items, 0, this._Count);
                this._Items = _emptyArray;
                this._Count = 0;
            }
        }
    }
}
