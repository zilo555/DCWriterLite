using System.Diagnostics;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Collections;

namespace DCSoft.Common
{
#if ! RELEASE
    internal sealed class DCCollectionDebugView<T>
    {
        private ICollection<T> collection;

        [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
        public T[] Items
        {
            get
            {
                T[] array = new T[collection.Count];
                collection.CopyTo(array, 0);
                return array;
            }
        }

        public DCCollectionDebugView(ICollection<T> collection)
        {
            if (collection == null)
            {
                throw new ArgumentNullException("collection");
            }
            this.collection = collection;
        }
    }
    [DebuggerTypeProxy(typeof(DCCollectionDebugView<>))]
    [DebuggerDisplay("Count = {Count}")]
#endif
    public class DCList<T> : IList<T>, System.Collections.IList
    {
        private const int _defaultCapacity = 4;

        protected T[] _items;
        protected int _size;
        //[NonSerialized]
        //private int _version;// 为了省内存，把这个属性去掉了。
        //[NonSerialized]
        //private Object _syncRoot;

        protected static readonly T[] _emptyArray = Array.Empty<T>();

        // Constructs a List. The list is initially empty and has a capacity
        // of zero. Upon adding the first element to the list the capacity is
        // increased to 16, and then increased in multiples of two as required.
        public DCList()
        {
            //if( typeof(T) == typeof(DCSoft.Writer.Dom.XTextContentElement))
            //{
            //    var s = 1;
            //}
            _items = _emptyArray;
        }

        // Constructs a List with a given initial capacity. The list is
        // initially empty, but will have room for the given number of elements
        // before any reallocations are required.
        // 
        public DCList(int capacity)
        {
            //if (typeof(T) == typeof(DCSoft.Writer.Dom.XTextContentElement))
            //{
            //    var s = 1;
            //}
            if (capacity < 0) throw new ArgumentOutOfRangeException("capacity,NeedNonNegNum");
            if (capacity == 0)
                this._items = _emptyArray;
            else
                this._items = new T[capacity];
            CheckLengthDebug();
        }

        public DCList(T[] items , bool useRaw )
        {
            //if (typeof(T) == typeof(DCSoft.Writer.Dom.XTextContentElement))
            //{
            //    var s = 1;
            //}
            if (items != null && items.Length > 0)
            {
                this._size = items.Length;
                if (useRaw)
                {
                    this._items = items;
                }
                else
                {
                    this._items = (T[])items.Clone();
                }
            }
            CheckLengthDebug();
        }
        //public bool CheckIndex(int index)
        //{
        //    return index >= 0 && index < this._size;
        //}

        public T[] InnerGetArrayRaw()
        {
            return this._items;
        }
        public void InnerSetArrayRaw(T[] arr , int len  )
        {
            this._items = arr;
            this._size = len;
        }
        

        public void SetData(T[] items )
        {
            if( items == null )
            {
                throw new ArgumentNullException("items");
            }
            if( this._size > 0 )
            {
                Array.Clear(this._items,0, this._size);
            }
            this._items = items;
            this._size = items.Length;
            CheckLengthDebug();
        }
        /// <summary>
        /// 根据哈希值获得元素对象
        /// </summary>
        /// <param name="hashCode">哈希值</param>
        /// <returns>元素对象</returns>
        public T GetElementByHashCode(int hashCode)
        {
            for (var iCount = this._size - 1; iCount >= 0; iCount--)
            {
                if (this._items[iCount].GetHashCode() == hashCode)
                {
                    return this._items[iCount];
                }
            }
            return default(T);
        }

        /// <summary>
        /// 获得第一个指定类型的元素
        /// </summary>
        /// <typeparam name="T2">指定的元素类型</typeparam>
        /// <returns>获得的元素对象</returns>
        public T2 GetFirstElement<T2>() where T2 :T
        {
            int len = this._size;
            for( int iCount = 0; iCount < len ;iCount ++)
            {
                if(this._items[iCount] is T2 )
                {
                    return (T2)this._items[iCount];
                }
            }
            return default(T2);
        }

        /// <summary>
        /// 获得第一个指定类型的元素
        /// </summary>
        /// <typeparam name="T2">指定的元素类型</typeparam>
        /// <returns>获得的元素对象</returns>
        protected T2 InnerGetFirstElement<T2>( int startIndex ) where T2 : T
        {
            if( startIndex < 0 )
            {
                throw new ArgumentOutOfRangeException("startIndex=" + startIndex);
            }
            int len = this._size;
            for (int iCount = startIndex ; iCount < len; iCount++)
            {
                if (this._items[iCount] is T2)
                {
                    return (T2)this._items[iCount];
                }
            }
            return default(T2);
        }

        /// <summary>
        /// 获得最后一个元素
        /// </summary>
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        //[DCSoft.Common.DCPublishAPI]
        public T LastElement
        {
            get
            {
                if(this._size > 0 )
                {
                    return this._items[this._size - 1];
                }
                else
                {
                    return default(T);
                }
            }
        }
        ///// <summary>
        ///// 尽量收缩占用的内存，不浪费内容。
        ///// </summary>
        //public void CollapseCapacity()
        //{
        //    if( this._items.Length > this._size + 10)
        //    {
        //        if (this._size == 0)
        //        {
        //            this._items = _emptyArray;
        //        }
        //        else
        //        {
        //            var arr = new T[this._size];
        //            Array.Copy(this._items, arr, this._size);
        //            Array.Clear(this._items, 0, this._size);
        //            this._items = arr;
        //        }
        //    }
        //}
        // Gets and sets the capacity of this list.  The capacity is the size of
        // the internal array used to hold items.  When set, the internal 
        // array of the list is reallocated to the given capacity.
        // 
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public int Capacity
        {
            get
            {
                return _items.Length;
            }
            set
            {
                if (value < _size)
                {
                    throw new ArgumentOutOfRangeException("value SmallCapacity");
                }
                if (value != _items.Length)
                {
                    if (value > 0)
                    {
                        T[] newItems = new T[value];
                        if (_size > 0)
                        {
                            Array.Copy(_items, 0, newItems, 0, _size);
                        }
                        _items = newItems;
                        CheckLengthDebug();
                    }
                    else
                    {
                        _items = _emptyArray;
                    }
                }
            }
        }

        public void CheckLengthDebug( )
        {
            //var len = this._items.Length;
            //if (len == 2048)//|| len 4484 || len == 3512 || len == 4000 || len == 4096 || len == 1121 || len == 2048)
            //{
            //    var s = 1;
            //}
        }
        // Read-only property describing how many elements are in the List.
        [System.Reflection.Obfuscation(Exclude = true , ApplyToMembers = true )]
        public int Count
        {
            get
            {
                return _size;
            }
        }
        //public void EnsureCount(int minCount )
        //{
        //    if( minCount < 0 )
        //    {
        //        throw new ArgumentOutOfRangeException("minCount=" + minCount);
        //    }
        //    if (this._size <= minCount)
        //    {
        //        this.EnsureCapacity(minCount);
        //        this._size = minCount;
        //    }
        //}
        bool System.Collections.IList.IsFixedSize
        {
            get { return false; }
        }


        // Is this List read-only?
        bool ICollection<T>.IsReadOnly
        {
            get { return false; }
        }

        bool System.Collections.IList.IsReadOnly
        {
            get { return false; }
        }

        // Is this List synchronized (thread-safe)?
        bool System.Collections.ICollection.IsSynchronized
        {
            get { return false; }
        }

        // Synchronization root for this object.
        Object System.Collections.ICollection.SyncRoot
        {
            get
            {
                return null;
            }
        }

        /// <summary>
		/// 安全的获得指定序号的元素,若序号超出范围则返回空引用
		/// </summary>
		/// <param name="index">序号</param>
		/// <returns>获得的元素对象</returns>
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public T SafeGet(int index)
        {
            if (index >= 0 && index < this._size)
                return this._items[index];
            else
                return default(T);
        }

        
        // Sets or Gets the element at the given index.
        // 
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public T this[int index]
        {
            get
            {
                // Following trick can reduce the range check by one
                if (index >= this._size)
                {
                    throw new ArgumentOutOfRangeException();
                }
                return _items[index];
            }
            set
            {
                if (index >= this._size)
                {
                    throw new ArgumentOutOfRangeException();
                }
                _items[index] = value;
                //_version++;
            }
        }


        /// <summary>
        /// 快速获得对象
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public T GetItemFast(int index)
        {
            return this._items[index];
        }

        /// <summary>
        /// 内部的快速设置数值
        /// </summary>
        /// <param name="index">序号</param>
        /// <param name="v">数值</param>
        public void InnerSetValueFast( int index , T v )
        {
            this._items[index] = v;
        }
        private static bool IsCompatibleObject(object value)
        {
            // Non-null values are fine.  Only accept nulls if T is a class or Nullable<U>.
            // Note that default(T) is not equal to null for value types except when T is Nullable<U>. 
            return ((value is T) || (value == null && default(T) == null));
        }

        Object System.Collections.IList.this[int index]
        {
            get
            {
                return this[index];
            }
            set
            {
                this[index] = (T)value;
            }
        }

        // Adds the given object to the end of this list. The size of the list is
        // increased by one. If required, the capacity of the list is doubled
        // before adding the new element.
        //
        public void Add(T item)
        {
            if (this._size == this._items.Length) this.EnsureCapacity(this._size + 1);
            this._items[this._size++] = item;
            //_version++;
        }
        public void FastAdd2(T item )
        {
            if (this._size == this._items.Length) this.EnsureCapacity(this._size + 1);
            this._items[this._size++] = item;
        }
        /// <summary>
        /// 超快速的添加项目，调用本函数前确保Capacity值足够用
        /// </summary>
        /// <param name="item"></param>
        public void SuperFastAdd(T item)
        {
            this._items[this._size++] = item;
        }
        ///// <summary>
        ///// 外界确认列表为空时的添加唯一的一个元素
        ///// </summary>
        ///// <param name="item">元素对象</param>
        //public void SetSingleElement(T item)
        //{
        //    this._items = new T[] { item };
        //    this._size = 1;
        //}

        int System.Collections.IList.Add(Object item)
        {
            Add((T)item);
            return Count - 1;
        }

        /// <summary>
        /// 移动数据到另外一个列表中，来源数据列表清空。
        /// </summary>
        /// <param name="targetList">目标列表对象</param>
        /// <param name="setCapacity">是否设置内部缓存区长度</param>
        public void MoveDataTo( DCList<T> targetList, bool setCapacity = false )
        {
            if(targetList != null && targetList != this )
            {
                if( targetList._items != null && targetList._size > 0 )
                {
                    Array.Clear(targetList._items, 0, targetList._size);
                }
                if(this._size == 0 )
                {
                    targetList._items = _emptyArray;
                    targetList._size = 0;
                }
                else
                {
                    if (setCapacity && this._size != this._items.Length)
                    {
                        targetList._items = new T[this._size];
                        Array.Copy(this._items, targetList._items, this._size);
                        targetList._size = this._size;
                    }
                    else
                    {
                        targetList._items = this._items;
                        targetList._size = this._size;
                    }
                    //targetList._version++;
                    this._items = _emptyArray;
                    this._size = 0;
                }
            }
        }
        //public void AddArray(T[] array, int startIndex, int length)
        //{
        //    if (array == null)
        //    {
        //        throw new ArgumentNullException("array");
        //    }
        //    if (startIndex < 0 || startIndex > array.Length)
        //    {
        //        throw new ArgumentOutOfRangeException("startIndex=" + startIndex);
        //    }
        //    if (length < 0)
        //    {
        //        throw new ArgumentOutOfRangeException("length=" + length);
        //    }
        //    if (length == 0)
        //    {
        //        return;
        //    }
        //    if (this._items == _emptyArray)
        //    {
        //        this._items = new T[length];
        //        Array.Copy(array, startIndex, this._items, 0, length);
        //        this._size = length;
        //    }
        //    else
        //    {
        //        this.EnsureCapacity(this._size + length);
        //        Array.Copy(array, startIndex, this._items, this._size, length);
        //        this._size += length;
        //    }
        //}
        public void AddArray(T[] array)
        {
            if (array == null)
            {
                throw new ArgumentNullException("array");
            }
            if (this._items == _emptyArray)
            {
                this._items = (T[])array.Clone();
                this._size = array.Length;
            }
            else
            {
                var len = array.Length;
                this.EnsureCapacity(this._size + len);
                Array.Copy(array, 0, this._items, this._size, len);
                this._size += len;
            }
        }

        public void AddRange4(DCList<T> list, int intStartIndex, int intLength)
        {
            if (list != null
                && list != this
                && intStartIndex >= 0
                && intLength > 0
                && intStartIndex + intLength <= list._size)
            {
                this.EnsureCapacity(this._size + intLength);
                Array.Copy(list._items, intStartIndex, this._items, this._size, intLength);
                this._size += intLength;
            }
        }
        public void AddRangeByDCList(DCList<T> list2)
        {
            if (list2 != null && list2._size > 0)
            {
                if (this._size == 0)// ._items == _emptyArray)
                {
                    if (this._items.Length < list2._size)
                    {
                        this._items = new T[list2._size];
                    }
                    Array.Copy(list2._items, 0, this._items, 0, list2._size);
                    this._size = list2._size;
                }
                else
                {
                    this.EnsureCapacity(this._size + list2._size);
                    Array.Copy(list2._items, 0, this._items, this._size, list2._size);
                    this._size += list2._size;
                }
            }
        }

        // Adds the elements of the given collection to the end of this list. If
        // required, the capacity of the list is increased to twice the previous
        // capacity or the new size, whichever is larger.
        //
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = false)]
        public void AddRange(IEnumerable<T> collection)
        {
            if (collection is DCList<T>)
            {
                var list2 = (DCList<T>)collection;
                if (list2._size > 0)
                {
                    AddRangeByDCList(list2);
                    //if (this._size == 0)// ._items == _emptyArray)
                    //{
                    //    if (this._items.Length < list2._size)
                    //    {
                    //        this._items = new T[list2._size];
                    //    }
                    //    Array.Copy(list2._items, 0, this._items, 0, list2._size);
                    //    this._size = list2._size;
                    //}
                    //else
                    //{
                    //    this.EnsureCapacity(this._size + list2._size);
                    //    Array.Copy(list2._items, 0, this._items, this._size, list2._size);
                    //    this._size += list2._size;
                    //}
                }
            }
            else if (collection is SubList<T>)
            {
                var list3 = (SubList<T>)collection;
                if (list3.BaseList is DCList<T>)
                {
                    var list4 = (DCList<T>)list3.BaseList;
                    if (this._items == _emptyArray)
                    {
                        this._items = new T[list3.Count];
                        Array.Copy(list4._items, list3.StartIndex, this._items, 0, list3.Count);
                        this._size = list3.Count;
                    }
                    else
                    {
                        var list3Count = list3.Count;
                        this.EnsureCapacity(this._size + list3Count);
                        Array.Copy(list4._items, list3.StartIndex, this._items, this._size, list3Count);
                        this._size += list3Count;
                    }
                }
                else
                {
                    var list9 = list3.BaseList;
                    this.EnsureCapacity(this._size + list3.Count);
                    int endIndex = list3.StartIndex + list3.Count;
                    for (int iCount = list3.StartIndex; iCount < endIndex; iCount++)
                    {
                        this._items[this._size++] = list9[iCount];
                    }
                }
            }
            else if (collection is T[])
            {
                var arr = (T[])collection;
                if (arr.Length > 0)
                {
                    if (this._size == 0)
                    {
                        if (this._items.Length < arr.Length)
                        {
                            this._items = (T[])arr.Clone();
                        }
                        else
                        {
                            Array.Copy(arr, 0, this._items, 0, arr.Length);
                        }
                        this._size = arr.Length;// this._items.Length;
                    }
                    else
                    {
                        this.EnsureCapacity(this._size + arr.Length);
                        Array.Copy(arr, 0, this._items, this._size, arr.Length);
                        this._size += arr.Length;
                    }
                }
            }
            else
            {
                InsertRange(_size, collection);
            }
            CheckLengthDebug();
        }
        public int BinarySearch(int index, int count, T item, IComparer<T> comparer)
        {
            if (index < 0)
                throw new ArgumentOutOfRangeException("index NeedNonNegNum");
            if (count < 0)
                throw new ArgumentOutOfRangeException("count NeedNonNegNum");
            if (_size - index < count)
            {
                throw new ArgumentException("InvalidOffLen");
            }
            return Array.BinarySearch<T>(_items, index, count, item, comparer);
        }
        public int BinarySearch(T item, IComparer<T> comparer)
        {
            return BinarySearch(0, Count, item, comparer);
        }

        protected T GetTheSingleElement()
        {
            if( this._size == 1 )
            {
                return this._items[0];
            }
            else
            {
                return default(T);
            }
        }

        /// <summary>
        /// 本列表的数组本来是公用的，现在归还出去
        /// </summary>
        internal void ReturnPublicArray()
        {
            if (this._size == 0)
            {
                this._items = _emptyArray;
            }
            else
            {
                //AllNum++;
                //AllCount += this._size;
                var arr = new T[this._size];
                Array.Copy(this._items, 0, arr, 0, this._size);
                this._items = arr;
                CheckLengthDebug();
            }
        }

        internal void ReturnToLoaderListBuffer(bool clearData )
        {
            if( this._items.Length > 0 )
            {
                LoaderListBuffer<T[]>.Instance.Return(this._items);
                if(clearData || this._size == 0 )
                {
                    this._size = 0;
                    this._items = _emptyArray;
                }
                else
                {
                    var arr = new T[this._size];
                    Array.Copy( this._items , 0 , arr , 0, this._size );
                    this._items = arr;
                }
            }
        }

        public virtual void DisposeItemsAndClear()
        {
            if (this._size > 0)
            {
                var items = this._items;
                for (int iCount = this._size - 1; iCount >= 0; iCount--)
                {
                    ((IDisposable)items[iCount]).Dispose();
                }
                Array.Clear(items, 0, this._size);
                this._size = 0;
                //this._version++;
                this._items = _emptyArray;
            }
        }

        // Clears the contents of List.
        public void Clear()
        {
            if (this._size > 0)
            {
                Array.Clear(this._items, 0, this._size); // Don't need to doc this but we clear the elements so that the gc can reclaim the references.
                this._size = 0;
                //this._version++;
            }
        }
        /// <summary>
        /// 快速清空内容
        /// </summary>
        public void FastClear()
        {
            this._size = 0;
            //this._version++;
        }

        public void ClearAndEmpty()
        {
            if( this._size > 0 )
            {
                Array.Clear(this._items, 0, this._size);
                this._items = _emptyArray;
                this._size = 0;
                //this._version++;
            }
        }

        public void ClearAndEmptyAll()
        {
            if (this._items.Length > 0)
            {
                Array.Clear(this._items, 0, this._items.Length);
                this._items = _emptyArray;
                this._size = 0;
                //this._version++;
            }
        }

        // Contains returns true if the specified element is in the List.
        // It does a linear, O(n) search.  Equality is determined by calling
        // item.Equals().
        //
        public virtual bool Contains(T item)
        {
            if ((Object)item == null)
            {
                for (int i = 0; i < _size; i++)
                {
                    if ((Object)_items[i] == null)
                    {
                        return true;
                    }
                }
                return false;
            }
            else
            {
                EqualityComparer<T> c = EqualityComparer<T>.Default;
                for (int i = 0; i < _size; i++)
                {
                    if (c.Equals(_items[i], item)) return true;
                }
                return false;
            }
        }

        bool System.Collections.IList.Contains(Object item)
        {
            if (IsCompatibleObject(item))
            {
                return Contains((T)item);
            }
            return false;
        }
        public void CopyTo(T[] array, int arrayIndex)
        {
            // Delegate rest of error checking to Array.Copy.
            Array.Copy(_items, 0, array, arrayIndex, _size);
        }

        // Copies this List into array, which must be of a 
        // compatible array type.  
        //
        void System.Collections.ICollection.CopyTo(Array array, int arrayIndex)
        {
            if ((array != null) && (array.Rank != 1))
            {
                throw new ArgumentException("Arg_RankMultiDimNotSupported");
            }
            Array.Copy(this._items, 0, array, arrayIndex, this._size);
        }

        // Ensures that the capacity of this list is at least the given minimum
        // value. If the currect capacity of the list is less than min, the
        // capacity is increased to twice the current capacity or to min,
        // whichever is larger.
        public void EnsureCapacity(int min)
        {
            if (this._items.Length < min)
            {
                int newCapacity = this._items.Length == 0 ? _defaultCapacity : this._items.Length * 2;
                // Allow the list to grow to maximum possible capacity (~2G elements) before encountering overflow.
                // Note that this check works even when _items.Length overflowed thanks to the (uint) cast
                //if ((uint)newCapacity > int.MaxValue) newCapacity = int.MaxValue;
                if (newCapacity < min) newCapacity = min;
                this.Capacity = newCapacity;
            }
        }
        public DCEnumerator GetEnumerator()
        {
            return new DCEnumerator(this);
        }

        /// <internalonly/>
        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return new DCEnumerator(this);
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return new DCEnumerator(this);
        }

        public DCList<T> GetRange(int index, int count)
        {
            if (index < 0)
            {
                throw new ArgumentOutOfRangeException("index");
            }

            if (count < 0)
            {
                throw new ArgumentOutOfRangeException("count");
            }

            if (_size - index < count)
            {
                
                throw new ArgumentException("InvalidOffLen");
            }
            DCList<T> list = (DCList<T>)this.MemberwiseClone();
            //list._syncRoot = null;
            //list._version = 0;
            if (count == 0)
            {
                list._items = _emptyArray;
            }
            else
            {
                list._items = new T[count];
                Array.Copy(_items, index, list._items, 0, count);
            }
            list._size = count;
            return list;
        }


        // Returns the index of the first occurrence of a given value in a range of
        // this list. The list is searched forwards from beginning to end.
        // The elements of the list are compared to the given value using the
        // Object.Equals method.
        // 
        // This method uses the Array.IndexOf method to perform the
        // search.
        // 
        public int IndexOf(T item)
        {
            return Array.IndexOf(_items, item, 0, _size);
        }

        public int IndexOfByReference(T item)
        {
            if (this._size > 0)
            {
                var items = this._items;
                for (int iCount = this._size - 1; iCount >= 0; iCount--)
                {
                    if ((object)items[iCount] == (object)item)
                    {
                        return iCount;
                    }
                }
            }
            return -1;
        }

        int System.Collections.IList.IndexOf(Object item)
        {
            if (IsCompatibleObject(item))
            {
                return IndexOf((T)item);
            }
            return -1;
        }

        // Returns the index of the first occurrence of a given value in a range of
        // this list. The list is searched forwards, starting at index
        // index and ending at count number of elements. The
        // elements of the list are compared to the given value using the
        // Object.Equals method.
        // 
        // This method uses the Array.IndexOf method to perform the
        // search.
        // 
        public int IndexOf(T item, int index)
        {
            if (index > _size)
                throw new ArgumentOutOfRangeException("index");
            return Array.IndexOf(_items, item, index, _size - index);
        }

        // Returns the index of the first occurrence of a given value in a range of
        // this list. The list is searched forwards, starting at index
        // index and upto count number of elements. The
        // elements of the list are compared to the given value using the
        // Object.Equals method.
        // 
        // This method uses the Array.IndexOf method to perform the
        // search.
        // 
        public int IndexOf(T item, int index, int count)
        {
            if (index > _size)
                throw new ArgumentOutOfRangeException("index");

            if (count < 0 || index > _size - count) throw new ArgumentOutOfRangeException("count");
            return Array.IndexOf(_items, item, index, count);
        }

        // Inserts an element into this list at a given index. The size of the list
        // is increased by one. If required, the capacity of the list is doubled
        // before inserting the new element.
        // 
        public void Insert(int index, T item)
        {
            // Note that insertions at the end are legal.
            if ((uint)index > (uint)_size)
            {
                throw new ArgumentOutOfRangeException("index");
            }
            if (_size == _items.Length) EnsureCapacity(_size + 1);
            if (index < _size)
            {
                Array.Copy(_items, index, _items, index + 1, _size - index);
            }
            _items[index] = item;
            _size++;
            //_version++;
        }


        void System.Collections.IList.Insert(int index, Object item)
        {
            Insert(index, (T)item);
        }
        /// <summary>
        /// 复制对象
        /// </summary>
        /// <returns>复制品</returns>
        public DCList<T> CloneList()
        {
            var list = (DCList<T>)this.MemberwiseClone();
            if (this._size > 0)
            {
                list._items = new T[this._size];
                Array.Copy(this._items, list._items, this._size);
            }
            //list._syncRoot = null;
            //list._version = 0;
            return list;
        }

        /// <summary>
        /// 复制对象数据到另外的一个列表
        /// </summary>
        /// <param name="list">要复制的目标列表</param>
        public void CopyTo( DCList<T> list )
        {
            if( list == null )
            {
                throw new ArgumentNullException("list");
            }
            if( list != this )
            {
                list._items = new T[this._size];
                Array.Copy(this._items, list._items, this._size);
                list._size = this._size;
            }
        }
        

        // Inserts the elements of the given collection at a given index. If
        // required, the capacity of the list is increased to twice the previous
        // capacity or the new size, whichever is larger.  Ranges may be added
        // to the end of the list by setting index to the List's size.
        //
        public void InsertRange(int index, IEnumerable<T> collection)
        {
            if (collection == null)
            {
                throw new ArgumentNullException("collection");
            }
            if ((uint)index > (uint)_size)
            {
                throw new ArgumentOutOfRangeException("index");
            }
            ICollection<T> c = collection as ICollection<T>;
            if (c != null)
            {    // if collection is ICollection<T>
                int count = c.Count;
                if (count > 0)
                {
                    EnsureCapacity(_size + count);
                    if (index < _size)
                    {
                        Array.Copy(_items, index, _items, index + count, _size - index);
                    }

                    // If we're inserting a List into itself, we want to be able to deal with that.
                    if (this == c)
                    {
                        // Copy first part of _items to insert location
                        Array.Copy(_items, 0, _items, index, index);
                        // Copy last part of _items back to inserted location
                        Array.Copy(_items, index + count, _items, index * 2, _size - index);
                    }
                    else if (c is DCList<T>)
                    {
                        Array.Copy(((DCList<T>)c)._items, 0, this._items, index, count);
                    }
                    else if ( c is T[])
                    {
                        Array.Copy((T[])c, 0, this._items, index, count);
                    }
                    else
                    {
                        T[] itemsToInsert = new T[count];
                        c.CopyTo(itemsToInsert, 0);
                        itemsToInsert.CopyTo(_items, index);
                    }
                    _size += count;
                }
            }
            else
            {
                using (IEnumerator<T> en = collection.GetEnumerator())
                {
                    while (en.MoveNext())
                    {
                        Insert(index++, en.Current);
                    }
                }
            }
            //_version++;
        }
        // Returns the index of the last occurrence of a given value in a range of
        // this list. The list is searched backwards, starting at the end 
        // and ending at the first element in the list. The elements of the list 
        // are compared to the given value using the Object.Equals method.
        // 
        // This method uses the Array.LastIndexOf method to perform the
        // search.
        // 
        public int LastIndexOf(T item)
        {
            if (_size == 0)
            {  // Special case for empty list
                return -1;
            }
            else
            {
                return LastIndexOf(item, _size - 1, _size);
            }
        }
        public int LastIndexOf(T item, int index, int count)
        {
            if ((Count != 0) && (index < 0))
            {
                throw new ArgumentOutOfRangeException("index,NeedNonNegNum");
            }

            if ((Count != 0) && (count < 0))
            {
                throw new ArgumentOutOfRangeException("count,NeedNonNegNum");
            }
            if (_size == 0)
            {  // Special case for empty list
                return -1;
            }

            if (index >= _size)
            {
                throw new ArgumentOutOfRangeException("index,BiggerThanCollection");
            }

            if (count > index + 1)
            {
                throw new ArgumentOutOfRangeException("count,BiggerThanCollection");
            }

            return Array.LastIndexOf<T>(_items, item, index, count);
        }

        // Removes the element at the given index. The size of the list is
        // decreased by one.
        // 
        public bool Remove(T item)
        {
            int index = IndexOf(item);
            if (index >= 0)
            {
                RemoveAt(index);
                return true;
            }

            return false;
        }

        void System.Collections.IList.Remove(Object item)
        {
            if (IsCompatibleObject(item))
            {
                Remove((T)item);
            }
        }
        public void RemoveAt(int index)
        {
            if ((uint)index >= (uint)_size)
            {
                throw new ArgumentOutOfRangeException();
            }
            _size--;
            if (index < _size)
            {
                Array.Copy(_items, index + 1, _items, index, _size - index);
            }
            _items[_size] = default(T);
            //_version++;
        }

        // Removes a range of elements from this list.
        // 
        public void RemoveRange(int index, int count)
        {
            if (index < 0)
            {
                throw new ArgumentOutOfRangeException("index,NeedNonNegNum");
            }

            if (count < 0)
            {
                throw new ArgumentOutOfRangeException("count,NeedNonNegNum");
            }

            if (_size - index < count)
                throw new ArgumentException("InvalidOffLen");

            if (count > 0)
            {
                int i = _size;
                _size -= count;
                if (index < _size)
                {
                    Array.Copy(_items, index + count, _items, index, _size - index);
                }
                Array.Clear(_items, _size, count);
                //_version++;
            }
        }

    

        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = false)]
        public T[] ToArray()
        {
            if (this._size == 0)
            {
                return _emptyArray;
            }
            else
            {
                T[] array = new T[_size];
                Array.Copy(_items, 0, array, 0, _size);
                return array;
            }
        }
        public T[] ToArrayRange(int startIndex , int length )
        {
            if( startIndex >=0 &&length > 0 && startIndex +length <= this._size )
            {
                var arr = new T[length];
                Array.Copy(this._items, startIndex, arr, 0, length);
                return arr;
            }
            return null;
        }



        /// <summary>
        /// 进行快速遍历
        /// </summary>
        /// <returns></returns>
        public IEnumerable<T> FastForEach()
        {
            return new FastForeachEnumerable(this._items, this._size);
        }

        /// <summary>
        /// 快速进行枚举遍历的类型，内部不进行一些状态的判断
        /// </summary>
        internal class FastForeachEnumerable : IEnumerable<T> , IEnumerator<T>
        {
            private T[] _Items ;
            private readonly int _Size ;
            private int _Index ;
            public FastForeachEnumerable( T[] items , int size )
            {
                this._Items = items;
                this._Size = size;
                this._Index = -1;
            }
            public T Current
            {
                get
                {
                    return this._Items[this._Index];
                }
            }
            object IEnumerator.Current
            {
                get
                {
                    return this._Items[this._Index];
                }
            }

            public void Dispose()
            {
                this._Items = null;
            }

            public IEnumerator<T> GetEnumerator()
            {
                return this;
            }

            public bool MoveNext()
            {
                return (++this._Index) < this._Size;
                //if( (++ this._Index ) < this._Size )
                //{
                //    return true;
                //}
                //else
                //{
                //    this._Items = null;
                //    return false;
                //}
            }

            public void Reset()
            {
                this._Index = -1;
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return this;
            }
        }

        public struct DCEnumerator : IEnumerator<T>, System.Collections.IEnumerator
        {
            private DCList<T> list;
            private int _index;
            //private int _version;
            private T _current;

            internal DCEnumerator(DCList<T> list)
            {
                this.list = list;
                _index = 0;
                //_version = list._version;
                _current = default(T);
            }

            public void Dispose()
            {
            }

            public bool MoveNext()
            {
                //if( this._version != this.list._version )
                //{
                //    throw new InvalidOperationException("list changed");
                //}
                if (this._index < this.list._size)
                {
                    this._current = this.list._items[_index];
                    _index++;
                    return true;
                }
                else
                {
                    return false;
                }

                //DCList<T> localList = list;

                //if (version == localList._version && ((uint)index < (uint)localList._size))
                //{
                //    current = localList._items[index];
                //    index++;
                //    return true;
                //}
                //return MoveNextRare();
            }

            //private bool MoveNextRare()
            //{
            //    if (_version != list._version)
            //    {
            //        throw new InvalidOperationException("EnumFailedVersion");
            //    }

            //    _index = list._size + 1;
            //    _current = default(T);
            //    return false;
            //}

            public T Current
            {
                get
                {
                    return _current;
                }
            }

            Object System.Collections.IEnumerator.Current
            {
                get
                {
                    return this._current;
                    //if (index == 0 || index == list._size + 1)
                    //{
                    //    throw new InvalidOperationException("EnumOpCantHappen");
                    //}
                    //return Current;
                }
            }

            void System.Collections.IEnumerator.Reset()
            {
                //if (_version != list._version)
                //{
                //    throw new InvalidOperationException("EnumFailedVersion");
                //}
                _index = 0;
                _current = default(T);
            }

        }
    }
}
