using System;
//using DCSoft.Writer.Dom.Undo ;
using DCSoft.Drawing;
// // 
using DCSoft.Common;
using System.Collections;
using System.Collections.Generic;

namespace DCSoft.Writer.Dom
{
    /// <summary>
    /// 文档区域对象
    /// </summary>
    /// <remarks>编写 袁永福</remarks>
#if !RELEASE
    [System.Diagnostics.DebuggerDisplay("Index={StartIndex},Length={Length}")]
    [System.Diagnostics.DebuggerTypeProxy(typeof(DCSoft.Common.ListDebugView))]
#endif     
    public class DCRange : System.Collections.IEnumerable
    {
        /// <summary>
        /// 初始化对象
        /// </summary>
        /// <param name="element">文档元素</param>
        /// <param name="startIndex">开始位置</param>
        /// <param name="length">长度</param>
        public DCRange(DomDocumentContentElement element, int startIndex, int length)
        {
            Refresh(element, startIndex, length);
        }

        /// <summary>
        /// 初始化对象
        /// </summary>
        /// <param name="element">文档元素</param>
        /// <param name="startElement">开始元素</param>
        /// <param name="endElement">结束元素</param>
        public DCRange(DomDocumentContentElement element, DomElement startElement , DomElement endElement )
        {
            Refresh(element, startElement , endElement );
        }

        /// <summary>
        /// 刷新状态
        /// </summary>
        /// <param name="element">文档元素</param>
        /// <param name="startElement">开始元素</param>
        /// <param name="endElement">结束元素</param>
        public void Refresh(DomDocumentContentElement element, DomElement startElement, DomElement endElement)
        {
            _Document = element.OwnerDocument;
            _Elements = element.Content;
            _StartElement = startElement;
            _EndElement = endElement;
            _StateInvalidate = true;
            _ContentVersion++;
            //if (_Document != null)
            //{
            //    // 清空用户指定的样式
            //    //_Document._UserSpecifyStyle = null;
            //}
        }

        /// <summary>
        /// 刷新状态
        /// </summary>
        /// <param name="element">文档元素</param>
        /// <param name="startIndex">开始序号</param>
        /// <param name="length">长度</param>
        public void Refresh(DomDocumentContentElement element, int startIndex, int length)
        {
            _Document = element.OwnerDocument;
            _Elements = element.Content;
            _StartIndex = startIndex;
            _Length = length;
            _StateInvalidate = true;
            _ContentVersion++;
            if (_Document != null)
            {
                // 清空用户指定的样式
                //_Document._UserSpecifyStyle = null;
            }
        }

        internal void Clear()
        {
            this._Document = null;
            this._Elements = null;
            this._EndElement = null;
            this._StartElement = null;
        }
        private int _ContentVersion = 0;
        private DomDocument _Document = null;
        private DomElementList _Elements = null;
        /// <summary>
        /// 参与的元素列表
        /// </summary>
        public DomElementList Elements
        {
            get
            {
                return _Elements; 
            }
            set
            {
                _Elements = value;
                _StateInvalidate = true;
                _ContentVersion++; 
            }
        }

        private int _StartIndex = 0;
        /// <summary>
        /// 区域开始序号
        /// </summary>
        public int StartIndex
        {
            get
            {
                return _StartIndex; 
            }
            set
            {
                _StartIndex = value;
                _StateInvalidate = true;
                _ContentVersion++; 
            }
        }

        private int _Length = 0;
        /// <summary>
        /// 区域长度
        /// </summary>
        public int Length
        {
            get
            {
                return _Length; 
            }
            set
            {
                _Length = value;
                _StateInvalidate = true; 
                _ContentVersion++; 
            }
        }

        private DomElement _StartElement = null;
        private DomElement _EndElement = null;
        private bool _StateInvalidate = true;
         
        public void CheckState()
        {
            if (_StateInvalidate)
            {
                CheckState(true);
            }
        }
        public static int _CheckStateCount = 0;
        public bool CheckState(bool throwException)
        {
            _CheckStateCount++;

            if (this._Elements == null)
            {
                if (throwException)
                {
                    throw new ArgumentNullException("Elements");
                }
                else
                {
                    return false;
                }
            }
            
            if (this._StartElement != null && this._EndElement != null)
            {
                int startElementIndex = WriterUtilsInner.SmartIndexOf(this._Elements, this._StartElement);
                int endElementIndex = WriterUtilsInner.SmartIndexOf(this._Elements, this._EndElement);
                if( startElementIndex < 0 || endElementIndex < 0 )
                {
                    if (throwException)
                    {
                        throw new IndexOutOfRangeException(this._StartIndex.ToString());
                    }
                    else
                    {
                        return false;
                    }
                }
                if( startElementIndex < endElementIndex )
                {
                    this._StartIndex = startElementIndex;
                    this._Length = endElementIndex - startElementIndex + 1;
                }
                else
                {
                    this._StartIndex = endElementIndex;
                    this._Length = startElementIndex - endElementIndex + 1;
                }
                //this._StartIndex = Math.Min(
                //    startElementIndex,
                //    endElementIndex);
                //if (this._StartIndex < 0)
                //{
                //    if (throwException)
                //    {
                //        throw new IndexOutOfRangeException(this._StartIndex.ToString());
                //    }
                //    else
                //    {
                //        return false;
                //    }
                //}
                //this._Length = Math.Abs(startElementIndex - endElementIndex) + 1;
            }
            else if ((this._StartElement == null) != (this._EndElement == null))
            {
                if (throwException)
                {
                    throw new ArgumentException("StartElement vs EndElement");
                }
                else
                {
                    return false;
                }
            }
            else
            {
                if (_Length < 0)
                {
                    // 修正区域长度
                    this._StartIndex = this._StartIndex + this._Length;
                    this._Length = -this._Length;
                }
                if (this._StartIndex < 0 || this._StartIndex >= this._Elements.Count)
                {
                    if (throwException)
                    {
                        throw new ArgumentOutOfRangeException("StartIndex");
                    }
                    else
                    {
                        return false;
                    }
                }
                if (this._Length < 0 || this._StartIndex + this._Length > this._Elements.Count)
                {
                    if (throwException)
                    {
                        throw new ArgumentOutOfRangeException("Length");
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            this._StateInvalidate = false;
            return true;
        }
        /// <summary>
        /// 判断指定的元素是否在区域中
        /// </summary>
        /// <param name="element">元素对象</param>
        /// <returns>是否在区域中</returns>
        public bool Contains(DomElement element)
        {
            CheckState();
            if (this._Document != null && this._Document.GetDocumentBehaviorOptions().WeakMode)
            {
                // 处于脆弱模式，抛出异常
                int index2 = -1;
                if (_Elements.SafeGet(element.ContentIndex) == element)
                {
                    index2 = element.ContentIndex;
                }
                else if (_Elements.SafeGet(element.PrivateContentIndex()) == element)
                {
                    index2 = element.PrivateContentIndex();
                }
                if (index2 < 0)
                {
                    index2 = _Elements.IndexOf(element, _StartIndex, _Length);
                }
                if (index2 >= 0)
                {
                    return true;
                }
            }
            else
            {
                try
                {
                    int index2 = -1;
                    if (_Elements.SafeGet(element.ContentIndex) == element)
                    {
                        index2 = element.ContentIndex;
                    }
                    else if (_Elements.SafeGet(element.PrivateContentIndex()) == element)
                    {
                        index2 = element.PrivateContentIndex();
                    }
                    if (index2 < 0)
                    {
                        index2 = _Elements.IndexOf(element, _StartIndex, _Length);
                    }
                    if (index2 >= 0)
                    {
                        return true;
                    }
                }
                catch
                {
                    return false;
                }
            }
            return false;
            //for (int iCount = 0; iCount < _Length; iCount++)
            //{
            //    int index = iCount + _StartIndex;
            //    if (index < 0 || index >= _Elements.Count)
            //    {
            //        if (this._Document != null && this._Document.Options.BehaviorOptions.WeakMode)
            //        {
            //            // 处于脆弱模式，抛出异常
            //            throw new ArgumentOutOfRangeException(index + " vs " + _Elements.Count);
            //        }
            //        return false;
            //    }
            //    if (_Elements[index] == element)
            //    {
            //        return true;
            //    }
            //}
            //return false;
        }

        /// <summary>
        /// 获得元素的枚举器
        /// </summary>
        /// <returns>枚举器</returns>
        public System.Collections.IEnumerator GetEnumerator()
        {
            CheckState();
            return new RangeEnumerator(this);
        }

        private class RangeEnumerator : System.Collections.IEnumerator
        {
            public RangeEnumerator(DCRange r)
            {
                this.range = r;
                this.Reset();

                //this.position = range.StartIndex - 1 ;
                //this.contentVersion = range._ContentVersion;
            }

            private int contentVersion = 0;
            private readonly DCRange range = null;

            private int position = -1;
             
            public object Current
            {
                get
                {
                    if (this.contentVersion != range._ContentVersion)
                    {
                        throw new InvalidOperationException(" 列表内容已被修改 ");
                    }
                    if (this.position >= 0 && this.position <= this._MaxPosition )// this.position < range.Elements.Count)
                    {
                        DomElement e = range._Elements[this.position];
                        //if (e == null)
                        //{ 
                        //}
                        return e;
                    }
                    else
                    {
                        return null;
                    }
                }
            }

            public bool MoveNext()
            {
                if (this.contentVersion != this.range._ContentVersion)
                {
                    throw new InvalidOperationException("列表内容已被修改");
                }
                //if (this.position < this.range.StartIndex + this.range.Length
                //    && this.position < this.range.Elements.Count - 1 )
                if( this.position < this._MaxPosition)
                {
                    this.position++;
                    return true;
                }
                else
                {
                    return false;
                }
            }

            public void Reset()
            {
                this.position = this.range.StartIndex - 1;
                this.contentVersion = this.range._ContentVersion;
                this._MaxPosition = Math.Min(this.range.StartIndex + this.range.Length, this.range.Elements.Count - 1);
            }
            private int _MaxPosition = 0;
        }
#if !RELEASE
        /// <summary>
        /// 返回表示对象数据的字符串
        /// </summary>
        /// <returns>字符串</returns>
        public override string ToString()
        {
            return "Index=" + this.StartIndex + ",Length=" + this.Length;
        }
#endif
    }
}