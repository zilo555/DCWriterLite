using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace DCSoft.Common
{
    /// <summary>
    /// 表示对一个列表的子集合的引用
    /// </summary>
    /// <typeparam name="ElementType">元素类型</typeparam>
    public class SubList<ElementType> : IList<ElementType>
    {
        public SubList()
        {
        }
        public SubList( IList<ElementType> baseList , int startIndex , int count )
        {
            Init(baseList, startIndex, count);
        }

        public void Init(IList<ElementType> baseList, int startIndex, int count)
        {
            if (baseList == null)
            {
                throw new ArgumentNullException("baseList");
            }
            if (startIndex < 0 || startIndex >= baseList.Count)
            {
                throw new ArgumentOutOfRangeException("startIndex=" + startIndex);
            }
            if (count < 0 || count + startIndex > baseList.Count)
            {
                throw new ArgumentOutOfRangeException("count=" + count);
            }
            this._BaseList = baseList;
            this._StartIndex = startIndex;
            this._Count = count;
        }
        /// <summary>
        /// 快速初始化，不做参数有效性判断
        /// </summary>
        /// <param name="baseList"></param>
        /// <param name="startIndex"></param>
        /// <param name="count"></param>
        public void FastInit(IList<ElementType> baseList, int startIndex, int count)
        {
            this._BaseList = baseList;
            this._StartIndex = startIndex;
            this._Count = count;
        }
        public void CleanStates()
        {
            this._BaseList = null;
        }

        private int _StartIndex = 0;
        internal int StartIndex
        {
            get
            {
                return this._StartIndex;
            }
        }
        private int _Count = 0;
        private IList<ElementType> _BaseList = null;
        internal IList<ElementType> BaseList
        {
            get
            {
                return this._BaseList;
            }
        }
        public ElementType this[int index]
        {
            get => this._BaseList[ this._StartIndex +index ];
            set => this._BaseList[this._StartIndex + index] = value;
        }

        public int Count => this._Count;

        public bool IsReadOnly => this._BaseList.IsReadOnly;

        public void Add(ElementType item)
        {
            this._BaseList.Insert(this._StartIndex + this._Count, item);
            this._Count++;
        }

        public void Clear()
        {
            if (this._BaseList is List<ElementType>)
            {
                var list2 = (List<ElementType>)this._BaseList;
                list2.RemoveRange(this._StartIndex, this._Count);
            }
            else
            {
                for (int iCount = this._StartIndex + this._Count; iCount >= this._StartIndex; iCount--)
                {
                    this._BaseList.RemoveAt(iCount);
                }
            }
            this._Count = 0;
        }

        public bool Contains(ElementType item)
        {
            int endIndex = this._StartIndex + this._Count;
            if (item == null)
            {
                for (int iCount = this._StartIndex; iCount <= endIndex; iCount++)
                {
                    if (this._BaseList[iCount] == null)
                    {
                        return true;
                    }
                }
                return false;
            }
            EqualityComparer<ElementType> @default = EqualityComparer<ElementType>.Default;
            for (int iCount = this._StartIndex; iCount <= endIndex; iCount++)
            {
                if( @default.Equals( this._BaseList[ iCount] , item ))
                {
                    return true;
                }
            }
            return false;
        }

        public void CopyTo(ElementType[] array, int arrayIndex)
        {
            if (this._BaseList is List<ElementType>)
            {
                ((List<ElementType>)this._BaseList).CopyTo(this._StartIndex, array, arrayIndex, this._Count);
            }
            else
            {
                for (int iCount = 0; iCount < this._Count; iCount++)
                {
                    array[iCount + arrayIndex ] = this._BaseList[iCount + this._StartIndex];
                }
            }
        }
        private class MyEnumer<ElementType2> : IEnumerator<ElementType2>
        {
            public MyEnumer( SubList<ElementType2> parent )
            {
                this._ParentList = parent._BaseList;
                this._Position = parent._StartIndex;
                this._StartPosition = parent._StartIndex;
                this._EndPostion = this._Position + parent._Count;
            }

            private IList<ElementType2> _ParentList = null;
            private readonly int _StartPosition = 0;
            private readonly int _EndPostion = 0;
            private int _Position = 0;

            public ElementType2 Current => this._ParentList[this._Position];

            object IEnumerator.Current => this._ParentList[this._Position];

            public void Dispose()
            {
                this._ParentList = null;
            }

            public bool MoveNext()
            {
                this._Position++;
                return this._Position > this._EndPostion;
            }

            public void Reset()
            {
                this._Position = this._StartPosition;
            }
        }

        public IEnumerator<ElementType> GetEnumerator()
        {
            return new MyEnumer<ElementType>(this);
        }

        public int IndexOf(ElementType item)
        {
            int endIndex = this._StartIndex + this._Count;
            if (item == null)
            {
                for (int iCount = this._StartIndex; iCount <= endIndex; iCount++)
                {
                    if (this._BaseList[iCount] == null )
                    {
                        return iCount - this._StartIndex;
                    }
                }
            }
            else
            {
                EqualityComparer<ElementType> @default = EqualityComparer<ElementType>.Default;
                for (int iCount = this._StartIndex; iCount <= endIndex; iCount++)
                {
                    if (@default.Equals(this._BaseList[iCount], item))
                    {
                        return iCount - this._StartIndex;
                    }
                }
            }
            return -1;
        }

        public void Insert(int index, ElementType item)
        {
            this._BaseList.Insert(this._StartIndex + index, item);
            this._Count++;
        }

        public bool Remove(ElementType item)
        {
            int index = this.IndexOf(item);
            if(index >= 0 )
            {
                this._BaseList.RemoveAt(index + this._StartIndex);
                this._Count--;
                return true;
            }
            return false;
        }

        public void RemoveAt(int index)
        {
            this._BaseList.RemoveAt(this._StartIndex + index);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new MyEnumer<ElementType>(this);
        }
    }
}
