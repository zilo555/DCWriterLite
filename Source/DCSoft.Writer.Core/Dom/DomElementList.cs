using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Reflection;
using DCSoft.Common;

namespace DCSoft.Writer.Dom
{
    /// <summary>
    /// 元素对象列表
    /// </summary>
    /// <remarks>
    /// 本列表专门用于放置若干个文本文档元素对象
    /// 编制 袁永福 2006-11-13
    /// </remarks>
#if !RELEASE
    [System.Diagnostics.DebuggerDisplay("Count={Count}")]
    [System.Diagnostics.DebuggerTypeProxy(typeof(DCSoft.Common.ListDebugView))]
#endif
    public partial class DomElementList : DCList<DomElement>, ICloneable
    {

        /// <summary>
        /// 表示空的列表对象
        /// </summary>
        internal static readonly DomElementList NullList = new DomElementList();
        public bool EqualItem(int index, DomElement item)
        {
            return index >= 0 && index < this._size && this._items[index] == item;
        }

        /// <summary>
        /// 重新计算所有元素的大小
        /// </summary>
        /// <param name="args"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public void ElementsRefreshSize(InnerDocumentPaintEventArgs args)
        {
            if (args == null)
            {
                throw new ArgumentNullException("args");
            }
            var len = this.Count;
            var arr = this.InnerGetArrayRaw();
            for (var iCount = 0; iCount < len; iCount++)
            {
                arr[iCount].RefreshSize(args);
            }
        }
        /// <summary>
        /// 初始化对象 
        /// </summary>
        public DomElementList()
        {
        }

        /// <summary>
        /// 初始化对象
        /// </summary>
        /// <param name="capacity">指定的元素个数</param>
        public DomElementList(int capacity) : base(capacity)
        {
        }
        /// <summary>
        /// 判断列表最后一个元素是段落符号元素
        /// </summary>
        /// <returns></returns>
        internal bool LastElementIsXTextParagraphFlagElement()
        {
            return base._size > 0 && base._items[base._size - 1] is DomParagraphFlagElement;
        }
        /// <summary>
        /// 列表中只包含一个段落符号元素
        /// </summary>
        /// <returns>是否只包含一个段落符号元素</returns>
        internal bool OnlyHasSingleParagraphFlagElement()
        {
            return base._size == 1 && base._items[0] is DomParagraphFlagElement;
        }
        /// <summary>
        /// 深入遍历所有的子孙元素
        /// </summary>
        /// <typeparam name="ElementType">元素类型</typeparam>
        /// <param name="handler">处理委托对象</param>
        /// <exception cref="ArgumentNullException">参数为空错误</exception>
        public void EnumerateElementDeeply<ElementType>(System.Action<ElementType> handler) where ElementType : DomElement
        {
            if (handler == null)
            {
                throw new ArgumentNullException("handler");
            }
            var arr = this.InnerGetArrayRaw();
            var len = this.Count;
            for (var iCount = 0; iCount < len; iCount++)
            {
                if (arr[iCount] is ElementType)
                {
                    handler((ElementType)arr[iCount]);
                }
                if (arr[iCount] is DomContainerElement c)
                {
                    if (c.HasElements())
                    {
                        c.Elements.EnumerateElementDeeply<ElementType>(handler);
                    }
                }
            }
        }

        internal void FastCloneDeeplyTo(
           DomElementList newList,
           DomContainerElement newParent,
           DomDocument newOwnerDocument)
        {
            if (newList == null)
            {
                throw new ArgumentNullException("newList");
            }
            var len = this.Count;
            if (len == 0)
            {
                newList.Clear();
            }
            else
            {
                var oldArr = this.InnerGetArrayRaw();
                var newArr = new DomElement[len];
                for (var iCount = len - 1; iCount >= 0; iCount--)
                {
                    var ne = oldArr[iCount].Clone(true);
                    ne.InnerSetOwnerDocumentParentRaw(newOwnerDocument, newParent);
                    newArr[iCount] = ne;
                }
                newList.SetData(newArr);
            }
        }
        public void ClearOwnerLine()
        {
            var arr = this.InnerGetArrayRaw();
            for (var iCount = this.Count - 1; iCount >= 0; iCount--)
            {
                arr[iCount]._OwnerLine = null;
            }
        }
        /// <summary>
        /// 对象状态位
        /// </summary>
        [NonSerialized]
        protected short _InstanceStates = 2;

        protected bool GetStateFlag(int mask)
        {
            return (this._InstanceStates & mask) != 0;
        }
        protected void SetStateFlag(int mask, bool vValue)
        {
            if (vValue)
            {
                this._InstanceStates = (short)(this._InstanceStates | mask);
            }
            else
            {
                this._InstanceStates = (short)(this._InstanceStates & ~mask);
            }
        }


        internal bool IsNone
        {
            get { return GetStateFlag(1); }
            set { SetStateFlag(1, value); }
        }

        /// <summary>
        /// 列表已经被拆分标记
        /// </summary>
        public bool IsElementsSplited
        {
            get { return GetStateFlag(2); }
            set { SetStateFlag(2, value); }
        }

        internal void SetToNone()
        {
            base.FastClear();
            this.IsNone = true;
        }
        /// <summary>
        /// 当元素只有一个段落符号，则获得这个唯一的段落符号元素，否则返回空。
        /// </summary>
        /// <returns></returns>
        internal DomParagraphFlagElement GetTheSingleParagraphFlagElement()
        {
            return base.GetTheSingleElement() as DomParagraphFlagElement;
        }

        /// <summary>
        /// 初始化对象
        /// </summary>
        /// <param name="element">默认就包含的元素</param>
        public DomElementList(DomElement element) : base(new DomElement[] { element }, true)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }
        }
        /// <summary>
        /// 获得指定类型的文档元素对象
        /// </summary>
        /// <param name="t">文档元素类型</param>
        /// <returns>获得的元素对象</returns>
        public DomElement GetElement(Type t)
        {
            if (t == null)
            {
                throw new ArgumentNullException("t");
            }
            foreach (DomElement element in this)
            {
                if (t.IsInstanceOfType(element))
                {
                    return element;
                }
            }
            return null;
        }

        /// <summary>
        /// 本文本行行尾是不是行结束类型的元素
        /// </summary>
        public bool HasLineEndElement
        {
            get
            {
                if (this.Count > 0)
                {
                    DomElement element = this[this.Count - 1];
                    if (element is DomEOFElement)
                    {
                        return true;
                    }
                    if (LayoutHelper.IsNewLine(element))
                    {
                        return true;
                    }
                    //if (XTextLine.__NewLayoutMode)
                    //{
                    //    element = this[0];
                    //    if (element is XTextEOFElement)
                    //    {
                    //        return true;
                    //    }
                    //    if (element.OwnerDocument.DocumentControler.IsNewLine(element))
                    //    {
                    //        return true;
                    //    }
                    //}
                }
                return false;
            }
        }

        internal bool InnerHasLineEndElement()
        {
            if (this.Count > 0)
            {
                DomElement element = this.LastElement;// [ this.Count -1 ] ;
                if (element is DomEOFElement)
                {
                    return true;
                }
                if (LayoutHelper.IsNewLine(element))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 移动元素位置
        /// </summary>
        /// <param name="oldIndex">旧位置</param>
        /// <param name="newIndex">新位置</param>
        /// <returns>操作是否成功</returns>
        public bool MoveElement(int oldIndex, int newIndex)
        {
            if (oldIndex == newIndex)
            {
                return false;
            }
            DomElement e = this[oldIndex];
            this.RemoveAt(oldIndex);
            this.Insert(newIndex, e);
            return true;
        }
        /// <summary>
        /// 修正元素序号以保证需要在元素列表的范围内
        /// </summary>
        /// <param name="index">原始的序号</param>
        /// <returns>修正后的序号</returns>
        public int FixElementIndex(int index)
        {
            if (index <= 0)
            {
                return 0;
            }
            if (index >= this.Count)
            {
                return this.Count - 1;
            }
            return index;
        }


        private void OnChanged()
        {
            //if (this.myOwnerElement != null && this.myOwnerElement.Elements == this )
            //{
            //    this.myOwnerElement.OnElementsChanged(this);
            //}
        }

        /// <summary>
        /// 列表中的第一个元素
        /// </summary>
        public DomElement FirstElement
        {
            get
            {
                if (this.Count > 0)
                {
                    return this[0];
                }
                else
                {
                    return null;
                }
            }
        }
        ///// <summary>
        ///// 列表中的最后一个元素
        ///// </summary>
        //      //[System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        //      //[DCSoft.Common.DCPublishAPI]
        //      public XTextElement LastElement
        //{
        //	get
        //	{
        //              int ic = this.Count;
        //              if (ic > 0)
        //              {
        //                  return this[ic - 1];
        //              }
        //              else
        //              {
        //                  return null;
        //              }
        //	}
        //}

        /// <summary>
        /// DCWriter内部使用。元素列表中第一个显示在文档视图中的元素
        /// </summary>
        public DomElement FirstContentElement
        {
            get
            {
                foreach (DomElement element in this)
                {
                    if (element is DomTableElement)
                    {
                        DomTableElement table = (DomTableElement)element;
                        if (table.FirstCell != null)
                        {
                            return table.FirstCell.FirstContentElement;
                        }
                    }
                    else if (element is DomParagraphFlagElement
                        || element is DomCharElement)
                    {
                        return element;
                    }
                    DomElement e = element.FirstContentElement;
                    if (e != null)
                    {
                        return e;
                    }
                }
                return null;
            }
        }
        /// <summary>
        /// DCWriter内部使用。元素列表中最后一个显示在文档视图中的元素
        /// </summary>
        public DomElement LastContentElement
        {
            get
            {
                for (int iCount = this.Count - 1; iCount >= 0; iCount--)
                {
                    DomElement e = this[iCount].LastContentElement;
                    if (e != null)
                    {
                        return e;
                    }
                }
                return null;
            }
        }

        /// <summary>
        /// 使用元素的ConentIndex属性值加速的查找元素序号。
        /// </summary>
        /// <param name="element">元素对象</param>
        /// <returns>元素序号</returns>
        internal int IndexOfUseContentIndex(DomElement element)
        {
            if (base.SafeGet(element._ContentIndex) == element)
            {
                return element._ContentIndex;
            }
            return this.IndexOf(element);
        }

        /// <summary>
        /// 使用元素的PrivateConentIndex属性值加速的查找元素序号。
        /// </summary>
        /// <param name="element">元素对象</param>
        /// <returns>元素序号</returns>
        internal int IndexOfUsePrivateContentIndex(DomElement element)
        {
            if (base.SafeGet(element._PrivateContentIndex) == element)
            {
                return element._PrivateContentIndex;
            }
            return this.IndexOf(element);
        }

        internal int IndexOfFast(DomElement element, int specifyIndex)
        {
            if (specifyIndex >= 0 && specifyIndex < this._size && this._items[specifyIndex] == element)
            {
                return specifyIndex;
            }
            return this.IndexOf(element);
        }

        /// <summary>
        /// 从指定位置开始获得第一个段落符号元素
        /// </summary>
        /// <param name="startIndex">开始的序号</param>
        /// <returns>获得的段落符号元素</returns>
        internal DomParagraphFlagElement GetFirstParagraphFlag(int startIndex)
        {
            return base.InnerGetFirstElement<DomParagraphFlagElement>(startIndex);
        }
        /// <summary>
        /// 快速的获得对象在列表中的序号
        /// </summary>
        /// <param name="element">对象</param>
        /// <returns>序号</returns>
        internal int FastIndexOfForPrivateContent(DomElement element)
        {
            if (SafeGet(element._PrivateContentIndex) == element)
            {
                return element._PrivateContentIndex;
            }
            if (SafeGet(element._ContentIndex) == element)
            {
                return element._ContentIndex;
            }
            if (SafeGet(element._ElementIndex) == element)
            {
                return element._ElementIndex;
            }

            //if (this is XTextContent)
            //{
            //    int vi = element._ContentIndex;
            //    if (vi >= 0 && vi < this.Count && this[vi] == element)
            //    {
            //        return vi;
            //    }
            //}
            //int vi2 = element._ElementIndex;
            //if (vi2 >= 0 && vi2 < this.Count && this[vi2] == element)
            //{
            //    return vi2;
            //}
            //vi2 = element._PrivateContentIndex;
            //if( vi2 >= 0 && vi2 < this.Count && this[vi2 ] == element )
            //{
            //    return vi2;
            //}
            return this.IndexOfByReference(element);
        }

        /// <summary>
        /// 快速的获得对象在列表中的序号
        /// </summary>
        /// <param name="element">对象</param>
        /// <returns>序号</returns>
        public int FastIndexOf(DomElement element)
        {
            if (element == null)
            {
                return -1;
            }
            if (SafeGet(element._ContentIndex) == element)
            {
                return element._ContentIndex;
            }
            if (SafeGet(element._ElementIndex) == element)
            {
                return element._ElementIndex;
            }
            if (SafeGet(element._PrivateContentIndex) == element)
            {
                return element._PrivateContentIndex;
            }
            //if (this is XTextContent)
            //{
            //    int vi = element._ContentIndex;
            //    if (vi >= 0 && vi < this.Count && this[vi] == element)
            //    {
            //        return vi;
            //    }
            //}
            //int vi2 = element._ElementIndex;
            //if (vi2 >= 0 && vi2 < this.Count && this[vi2] == element)
            //{
            //    return vi2;
            //}
            //vi2 = element._PrivateContentIndex;
            //if( vi2 >= 0 && vi2 < this.Count && this[vi2 ] == element )
            //{
            //    return vi2;
            //}
            return this.IndexOfByReference(element);
        }


        /// <summary>
        /// 获得子列表
        /// </summary>
        /// <param name="startIndex">开始区域位置</param>
        /// <param name="length">长度</param>
        /// <returns>子列表</returns>
        new public DomElementList GetRange(int startIndex, int length)
        {
            if (length == 0)
            {
                return new DomElementList();
            }
            var list = base.GetRange(startIndex, length);
            DomElementList result = new DomElementList(list.Count);
            result.AddRangeByDCList(list);
            return result;
        }

        /// <summary>
        /// 插入多个元素
        /// </summary>
        /// <param name="index"></param>
        /// <param name="collection"></param>
        new public void InsertRange(int index, IEnumerable<DomElement> collection)
        {
            //foreach (XTextElement e in collection)
            //{
            //    if (e == null)
            //    {
            //    }
            //}
            base.InsertRange(index, collection);
            this.IsElementsSplited = false;
            OnChanged();
        }

        /// <summary>
        /// 原生态的插入多个元素
        /// </summary>
        /// <param name="index"></param>
        /// <param name="elements"></param>
        public void InsertRangeRaw(int index, IEnumerable<DomElement> elements)
        {
            base.InsertRange(index, elements);
        }

        /// <summary>
        /// 插入元素
        /// </summary>
        /// <param name="index"></param>
        /// <param name="element"></param>
        new public void Insert(int index, DomElement element)
        {
            //if (this._OwnerElement != null)
            //{
            //    element.Parent = _OwnerElement;
            //}
            base.Insert(index, element);
            if (element is DomStringElement)
            {
                this.IsElementsSplited = false;
            }
            OnChanged();
        }

        internal void InsertRaw(int index, DomElement element)
        {
            base.Insert(index, element);
        }

        internal static bool _InnerDeserializing = false;
        /// <summary>
        /// 正在反序列化标记
        /// </summary>
        internal static bool InnerDeserializing
        {
            get
            {
                return _InnerDeserializing;
            }
            set
            {
                _InnerDeserializing = value;
            }
        }
        /// <summary>
        /// 向列表添加对象
        /// </summary>
        /// <param name="element">对象</param>
        new public void Add(DomElement element)
        {
            if (_InnerDeserializing)
            {
                // 正在反序列化操作，快速添加。
                base.Add(element);
                return;
            }
            //if (element == null)
            //{
            //}
            //if (this._OwnerElement != null)
            //{
            //    element.SetParentRaw(_OwnerElement);
            //}
            base.Add(element);
            if (element is DomStringElement || element is DomTextElement)
            {
                this.IsElementsSplited = false;
            }
            OnChanged();
        }

        /// <summary>
        /// 删除元素
        /// </summary>
        /// <param name="element"></param>
        new public void Remove(DomElement element)
        {
            base.Remove(element);
            OnChanged();
        }
        /// <summary>
        /// 直接添加元素
        /// </summary>
        /// <param name="element">元素对象</param>
        public void AddRaw(DomElement element)
        {
            //if (element == null)
            //{
            //}
            base.Add(element);
        }

        //public void AddRangeRaw(XTextElementList elements)
        //{
        //    base.AddRangeByDCList(elements);
        //}

        internal void AddRangeRaw2(IEnumerable<DomElement> elements)
        {
            base.AddRange(elements);
        }
        public void AddRange(DomElement[] elements)
        {
            if (elements != null && elements.Length > 0)
            {
                base.AddRange(elements);
                OnChanged();
            }
        }

        /// <summary>
        /// 若元素个数超过指定值,则删除元素直到元素个数等于指定值
        /// </summary>
        /// <param name="length">指定的元素个数</param>
        internal virtual void RemoveToFixLenght(int length)
        {
            if (length < this.Count)
            {
                base.RemoveRange(length, this.Count - length);
            }
            //bool flag = false;
            //for (int iCount = this.Count - 1; iCount >= length; iCount--)
            //{
            //    this.RemoveAt(iCount);
            //    flag = true;
            //}
            //if (flag)
            //{
            //    //OnItemChange();
            //}
        }

        /// <summary>
        /// 删除多个元素
        /// </summary>
        /// <param name="elements"></param>
        public void RemoveRange(DomElementList elements)
        {
            if (elements != null && elements.Count > 0)
            {
                bool match = false;
                int startIndex = this.IndexOf(elements[0]);
                int endIndex = this.IndexOf(elements.LastElement);
                if (startIndex < 0 || endIndex < 0)
                {
                    // 不属于该列表的元素，退出操作
                    return;
                }
                if (endIndex - startIndex + 1 == elements.Count)
                {
                    match = true;
                    int index = startIndex;
                    foreach (DomElement element in elements)
                    {
                        if (element != this[index])
                        {
                            match = false;
                            break;
                        }
                        index++;
                    }
                }
                if (match)
                {
                    // 可以批量快速删除
                    this.RemoveRange(startIndex, elements.Count);
                }
                else
                {
                    // 无法批量快速删除，只能一个个删除了
                    for (int iCount = elements.Count - 1; iCount >= 0; iCount--)
                    {
                        this.Remove(elements[iCount]);
                    }
                }
                OnChanged();
            }
        }
        internal void RemoveAtRaw(int index)
        {
            base.RemoveAt(index);
        }


        /// <summary>
        /// 在指定的元素前面插入新的元素
        /// </summary>
        /// <param name="OldElement">指定的元素</param>
        /// <param name="NewElement">要插入的新的元素</param>
        /// <returns>操作是否成功</returns>
        public bool InsertBefore(DomElement OldElement, DomElement NewElement)
        {
            if (OldElement == null)
            {
                throw new System.ArgumentNullException("未指定元素");
            }
            if (NewElement == null)
            {
                throw new System.ArgumentNullException("未指定新元素");
            }
            int index = this.IndexOf(OldElement);
            if (index >= 0)
            {
                this.Insert(index, NewElement);
                return true;
            }
            return false;
        }

        /// <summary>
        /// 在指定的元素后面插入新的元素
        /// </summary>
        /// <param name="OldElement">指定的元素</param>
        /// <param name="NewElement">要插入的新的元素</param>
        /// <returns>操作是否成功</returns>
        public bool InsertAfter(DomElement OldElement, DomElement NewElement)
        {
            if (OldElement == null)
                throw new System.ArgumentNullException("未指定元素");
            if (NewElement == null)
                throw new System.ArgumentNullException("未指定新元素");
            int index = this.IndexOf(OldElement);
            if (index >= 0)
            {
                this.Insert(index + 1, NewElement);
                return true;
            }
            return false;
        }
        /// <summary>
        /// 在指定元素前面插入多个元素
        /// </summary>
        /// <param name="OldElement">指定的元素</param>
        /// <param name="list">要插入的新元素列表</param>
        public void InsertRangeBefore(DomElement OldElement, DomElementList list)
        {
            int index = this.IndexOf(OldElement);
            if (index >= 0)
            {
                foreach (DomElement e in list)
                {
                    if (e == null)
                    {
                    }
                }
                this.InsertRange(index, list);
            }
        }

        /// <summary>
        /// 获得子列表
        /// </summary>
        /// <param name="startIndex">开始元素的序号</param>
        /// <param name="length">元素的个数</param>
        /// <returns>获得的子列表</returns>
        public DomElementList GetElements(int startIndex, int length)
        {
            if (startIndex < 0)
            {
                throw new ArgumentException("startIndex=" + startIndex);
            }
            DomElementList list = new DomElementList();
            int endIndex = Math.Min(this.Count - 1, startIndex + length - 1);
            for (int iCount = startIndex; iCount <= endIndex; iCount++)
            {
                list.Add(this[iCount]);
            }
            return list;
        }

        /// <summary>
        /// 获得指定元素前面的一个元素
        /// </summary>
        /// <param name="element">元素对象</param>
        /// <returns>该元素的前一个元素对象</returns>
        public DomElement GetPreElement(DomElement element)
        {
            int index = this.FastIndexOf(element);
            if (index > 0)
            {
                return this[index - 1];
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// 获得指定元素后面的一个元素
        /// </summary>
        /// <param name="element">元素对象</param>
        /// <returns>该元素后面的一个元素</returns>
        public DomElement GetNextElement(DomElement element)
        {
            var ei = element._ElementIndex;
            if (ei >= 0 && ei < this.Count && this[ei] == element)
            {
                if (ei < this.Count - 1)
                {
                    return this[ei + 1];
                }
                else
                {
                    return null;
                }
            }
            int index = this.FastIndexOf(element);
            if (index >= 0 && index < this.Count - 1)
            {
                return this[index + 1];
            }
            else
            {
                return null;
            }
        }

        public DomElement SafeGetNextElement(DomElement element, int offset)
        {
            int index = this.FastIndexOf(element);
            if (index < 0)
            {
                return null;
            }
            index = index + offset;
            if (index >= 0 && index < this.Count)
            {
                return this[index];
            }
            return null;
        }
#if !RELEASE
        /// <summary>
        /// 返回表示对象的字符串
        /// </summary>
        /// <returns>字符串</returns>
        public override string ToString()
        {
            System.Text.StringBuilder myStr = new System.Text.StringBuilder();
            foreach (DomElement element in this)
            {
                myStr.Append(element.ToString());
            }
            return myStr.ToString();
        }
#endif
        /// <summary>
        /// 获得内置的文本
        /// </summary>
        /// <returns>获得的文本</returns>
        public string GetInnerText()
        {
            if (this.Count == 0)
            {
                return string.Empty;
            }
            var args = new DomElement.WriteTextArgs(this[0].OwnerDocument);
            foreach (DomElement element in this)
            {
                element.WriteText(args);
                //myStr.Append(element.Text);
            }
            return args.ToString();
        }

        /// <summary>
        /// 进行深度复制
        /// </summary>
        /// <returns>复制品</returns>
        public DomElementList CloneDeeply()
        {
            DomElementList list = new DomElementList();
            foreach (DomElement element in this)
            {
                list.Add(element.Clone(true));
            }
            return list;
        }
        ///// <summary>
        ///// 元素反转
        ///// </summary>
        //public void Reverse()
        //{
        //    base.rev
        //    base.InnerList.Reverse();
        //}
        internal bool ContainsUsePrivateIndex(DomElement item)
        {
            if (this.SafeGet(item._PrivateContentIndex) == item)
            {
                return true;
            }
            for (var iCount = base._size - 1; iCount >= 0; iCount--)
            {
                if ((object)base._items[iCount] == (object)item)
                {
                    return true;
                }
            }
            return false;
        }

        public bool EqualsElement(DomElement item, int index)
        {
            return index >= 0 && index < base._size && base._items[index] == item;
        }

        public override bool Contains(DomElement item)
        {
            //return System.Array.IndexOf<XTextElement>(base._items, item, 0, base._size) >= 0;
            for (var iCount = base._size - 1; iCount >= 0; iCount--)
            {
                if ((object)base._items[iCount] == (object)item)
                {
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// 复制对象
        /// </summary>
        /// <returns>复制品</returns>
        object ICloneable.Clone()
        {
            DomElementList list = null;
            if (this is DomLine)
            {
                list = new DomLine();
            }
            else if (this is DCContent)
            {
                list = new DCContent(null);
            }
            else
            {
                list = new DomElementList();
            }
            list.AddRangeByDCList(this);
            return list;
        }

        /// <summary>
        /// 复制对象
        /// </summary>
        /// <returns>复制品</returns>
        public DomElementList Clone()
        {
            DomElementList list = null;
            if (this is DomLine)
            {
                list = new DomLine();
            }
            else if (this is DCContent)
            {
                list = new DCContent(null);
            }
            else if (this is DomElementList)
            {
                list = new DomElementList();
            }
            else
            {
                list = (DomElementList)System.Activator.CreateInstance(this.GetType());
            }
            this.CopyTo(list);
            return list;
        }
    }
}