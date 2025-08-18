using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using DCSoft.Drawing;
using System.Runtime.InteropServices ;
using DCSoft.Common;
namespace DCSoft.Writer.Dom
{
    /// <summary>
    /// 字段元素
    /// </summary>
    /// <remarks>编制 袁永福</remarks>
    [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = false)]
    public abstract partial class DomFieldElementBase : DomContainerElement
    {
        /// <summary>
        /// 初始化对象
        /// </summary>
        protected DomFieldElementBase()
        {

        }

        /// <summary>
        /// 修复内部元素的样式编号
        /// </summary>
        internal void FixInnerElementStyleIndex()
        {
            int si = this._StyleIndex;
            if (this._StartElement != null)
            {
                this._StartElement.StyleIndex = si;
            }
            if (this._EndElement != null)
            {
                this._EndElement.StyleIndex = si;
            }
            if (this._InnerBackgroundTextElements != null)
            {
                foreach (DomElement e in this._InnerBackgroundTextElements)
                {
                    e.StyleIndex = si;
                }
            }
        }
        /// <summary>
        /// DCWriter内部使用。获得运行时的子元素列表
        /// </summary>
        /// <returns></returns>

        public DomElementList GetRuntimeElements()
        {
            DomElementList list = new DomElementList();
            if (this.StartElement != null)
            {
                list.FastAdd2(this.StartElement);
            }
            if (this.Elements.Count == 0)
            {
                if (this._InnerBackgroundTextElements != null && this._InnerBackgroundTextElements.Length > 0)
                {
                    list.AddArray(this._InnerBackgroundTextElements);
                }
            }
            else
            {
                list.AddRangeByDCList(this.Elements);
            }
            if (this.EndElement != null)
            {
                list.FastAdd2(this.EndElement);
            }
            return list;
        }

        /// <summary>
        /// 运行时使用的背景文字
        /// </summary>
        public virtual string RuntimeBackgroundText
        {
            get
            {
                return null;
            }
        }

        /// <summary>
        /// 元素大小无效标记
        /// </summary>
        /// <remarks>若设置该属性,则元素的大小发生改变,系统需要从该元素
        /// 开始重新进行文档排版和分页</remarks>
        public override bool SizeInvalid
        {
            get
            {
                return base.SizeInvalid;
            }
            set
            {
                base.SizeInvalid = value;
                if (this._StartElement != null)
                {
                    this._StartElement.SizeInvalid = value;
                }
                if (this._EndElement != null)
                {
                    this._EndElement.SizeInvalid = value;
                }
            }
        }

        /// <summary>
        /// 判断指定元素是否是本元素的父节点或者更高层次的父节点。
        /// </summary>
        /// <param name="element">要判断的元素</param>
        /// <returns>是否是父节点或者更高级的父节点</returns>
        public override bool IsParentOrSupParent(DCSoft.Writer.Dom.DomElement element)
        {
            if (element == this._StartElement || element == this._EndElement)
            {
                return true;
            }
            return base.IsParentOrSupParent(element);
        }

        /// <summary>
        /// 初始化起始元素和结束元素
        /// </summary>
        protected virtual void CheckStartEndElement()
        {
            if (this._StartElement == null)
            {
                this._StartElement = new DomFieldBorderElement(this);
                //if (this.Elements.Count > 0)
                //{
                //    _StartElement.StyleIndex = this.Elements[0].StyleIndex;
                //}
            }
            this._StartElement._StyleIndex = this._StyleIndex;
            if (this._EndElement == null)
            {
                this._EndElement = new DomFieldBorderElement(this);
                //if (this.Elements.Count > 0)
                //{
                //    _EndElement.StyleIndex = this.Elements[this.Elements.Count - 1].StyleIndex;
                //}
            }

            this._EndElement._StyleIndex = this._StyleIndex;
        }

        /// <summary>
        /// 文档元素所属的文档对象
        /// </summary>
        public override DomDocument OwnerDocument
        {
            get
            {
                return this._OwnerDocument;// base.ElementOwnerDocument() ;
            }
            set
            {
                if (base._OwnerDocument != value)
                {
                    base.OwnerDocument = value;
                    if (this._StartElement != null)
                    {
                        this._StartElement.OwnerDocument = value;
                    }
                    if (this._EndElement != null)
                    {
                        this._EndElement.OwnerDocument = value;
                    }
                }
            }
        }
        /// <summary>
        /// 起始元素在文档中的绝对坐标值
        /// </summary>
        public float StartElementAbsLeft
        {
            get
            {
                var e = this.StartElement;
                if (e == null)
                {
                    return 0;
                }
                else
                {
                    return e.AbsLeft;
                }
            }
        }
        /// <summary>
        /// 起始元素在文档中的绝对坐标值
        /// </summary>
        public float StartElementAbsTop
        {
            get
            {
                var e = this.StartElement;
                if (e == null)
                {
                    return 0;
                }
                else
                {
                    return e.AbsTop;
                }
            }
        }

        /// <summary>
        /// 对象在文档中的绝对坐标位置
        /// </summary>
        public override float AbsLeft
        {
            get
            {
                if (this.StartElement == null)
                {
                    return 0;
                }
                else
                {
                    return this.StartElement.AbsLeft;
                }
            }
        }

        /// <summary>
        /// 对象在文档中的绝对坐标位置
        /// </summary>
        public override float AbsTop
        {
            get
            {
                if (this.StartElement == null)
                {
                    return 0;
                }
                else
                {
                    return this.StartElement.AbsTop;
                }
            }
        }

        /// <summary>
        /// 获得一个从0开始计算的当前元素所在的页码
        /// </summary>
        public override int OwnerPageIndex
        {
            get
            {
                if (this.StartElement == null)
                {
                    return -1;
                }
                else
                {
                    return this.StartElement.OwnerPageIndex;
                }
            }
        }

        [NonSerialized]
        internal protected DomFieldBorderElement _StartElement;
        /// <summary>
        /// 起始元素
        /// </summary>
        public DomFieldBorderElement StartElement
        {
            get
            {
                CheckStartEndElement();
                if (_StartElement != null)
                {
                    _StartElement.Parent = this;
                }
                //_StartElement.FixWidthForEmptyField();
                return _StartElement;
            }
            set
            {
                _StartElement = value;
            }
        }
        [NonSerialized]
        internal protected DomFieldBorderElement _EndElement;
        /// <summary>
        /// 结束元素
        /// </summary>
        public DomFieldBorderElement EndElement
        {
            get
            {
                CheckStartEndElement();
                if (_EndElement != null)
                {
                    _EndElement.Parent = this;
                }
                //_EndElement.FixWidthForEmptyField();
                return _EndElement;
            }
            set
            {
                _EndElement = value;
            }
        }

        /// <summary>
        /// 判断所有的内容是否都处于被选择区域
        /// </summary>
        public virtual bool IsFullSelect
        {
            get
            {
                DomDocumentContentElement dce = this.DocumentContentElement;
                if (dce != null
                    && dce.Content.FastIndexOf(this.StartElement) >= dce.Selection.AbsStartIndex
                    && dce.Content.FastIndexOf(this.EndElement) <= dce.Selection.AbsEndIndex)
                {
                    return true;
                }
                return false;
            }
        }

        /// <summary>
        /// 选择文档元素
        /// </summary>
        /// <returns></returns>
        public override bool Select()
        {
            DomDocumentContentElement dce = this.DocumentContentElement;
            if (dce == null)
            {
                // DOM状态不对
                return false;
            }
            CheckDocumentContentElementFocused();
            int startIndex = dce.Content.FastIndexOf(this.StartElement);
            int endIndex = dce.Content.FastIndexOf(this.EndElement);
            if (startIndex >= 0 && endIndex > 0)
            {
                this.OwnerDocument.ClearPagesClientSelectionBounds();
                dce.SetSelection(startIndex, endIndex - startIndex + 1);
                if (dce.Content.FastIndexOf(this.StartElement) != startIndex
                    && endIndex != dce.Content.FastIndexOf(this.EndElement))
                {
                    startIndex = dce.Content.FastIndexOf(this.StartElement);
                    endIndex = dce.Content.FastIndexOf(this.EndElement);
                    if (startIndex >= 0 && endIndex > 0)
                    {
                        dce.SetSelection(startIndex, endIndex - startIndex + 1);
                    }
                }
                return true;
            }
            else
            {
                return base.Select();
            }
        }


        /// <summary>
        /// 背景文档元素
        /// </summary>
        protected DomElement[] _InnerBackgroundTextElements;
        /// <summary>
        /// 背景文档元素
        /// </summary>
        protected DomElement[] InnerBackgroundTextElements
        {
            get
            {
                return this._InnerBackgroundTextElements;
            }
            set
            {
                this._InnerBackgroundTextElements = value;
            }
        }

        private static readonly DomElementList _List_CheckBackgroundTextElements = new DomElementList();
        internal void CheckBackgroundTextElements()
        {
            base.CommitStyle(false);
            if (this._InnerBackgroundTextElements == null)
            {
                // 创建背景元素
                string bgText = this.RuntimeBackgroundText;
                if (string.IsNullOrEmpty(bgText) == false)// bgText != null && bgText.Length > 0 )
                {
                    DomDocument document = this._OwnerDocument;
                    //int styleIndex = document.ContentStyles.GetStyleIndex(this.RuntimeStyle.CloneWithoutBorder());
                    var list = _List_CheckBackgroundTextElements;
                    list.Clear();
                    list.EnsureCapacity(bgText.Length);
                    foreach (var c in bgText)
                    {
                        if (c != '\r' || c != '\n')
                        {
                            var ce = new DomCharElement(c, this, document, this._StyleIndex);
                            //ce.SetParentAndDocumentRaw(this);
                            //ce._StyleIndex = this._StyleIndex;
                            ce.IsBackgroundText = true;
                            list.SuperFastAdd(ce);
                        }
                    }//foreach
                    this._InnerBackgroundTextElements = list.ToArray();
                    list.Clear();
                }
                else
                {
                    this._InnerBackgroundTextElements = DomElement.EmptyArray;
                }
            }
        }

        /// <summary>
        /// 运行时的是否打印背景文本
        /// </summary>
        public virtual bool RuntimePrintBackgroundText()
        {
            if (this._OwnerDocument == null)
            {
                return true;
            }
            else
            {
                return GetDocumentViewOptions().PrintBackgroundText;
            }
        }


        /// <summary>
        /// 快速判断是否是背景文本元素,仅在特定情况下是可靠的。
        /// </summary>
        /// <param name="element">文档元素对象</param>
        /// <returns>是否是背景文本元素</returns>
        public bool FastIsBackgroundTextElement(DomElement element)
        {
            if (this._Elements == null || this._Elements.Count == 0)
            {
                return true;
            }
            return false;
            //return IsBackgroundTextElement(element);
        }


        public virtual bool IsBackgroundTextElementDom(DomElement element)
        {
            var arr = this._InnerBackgroundTextElements;
            return arr != null
                && arr.Length > 0
                && arr.ArrayContains<DomElement>(element);
        }
        /// <summary>
        /// 判断是否是背景文本元素
        /// </summary>
        /// <param name="element">文档元素对象</param>
        /// <returns>是否是背景文本元素</returns>
        public virtual bool IsBackgroundTextElement(DomElement element)
        {
            if (element is DomCharElement)
            {
                return ((DomCharElement)element).IsBackgroundText;
            }
            return false;
        }

        /// <summary>
        /// 重新计算文档元素内容大小
        /// </summary>
        /// <param name="args">参数</param>
        public override void RefreshSize(InnerDocumentPaintEventArgs args)
        {
            base.RefreshSize(args);
            CheckStartEndElement();
            if (this._StartElement != null)
            {
                this._StartElement.RefreshSize(args);
            }
            if (this._EndElement != null)
            {
                this._EndElement.RefreshSize(args);
            }
            CheckBackgroundTextElements();
            var arr = this._InnerBackgroundTextElements;
            if (arr != null && arr.Length > 0)
            {
                for (var iCount = arr.Length - 1; iCount >= 0; iCount--)
                {
                    arr[iCount].RefreshSize(args);
                }
            }
        }

        internal void UpdateChildStyleIndex()
        {
            this._StartElement?.SetStyleIndexForce(this._StyleIndex);
            this._EndElement?.SetStyleIndexForce(this._StyleIndex);
            var arr = this._InnerBackgroundTextElements;
            if (arr != null && arr.Length > 0)
            {
                for (var iCount = arr.Length - 1; iCount >= 0; iCount--)
                {
                    arr[iCount].SetStyleIndexForce(this._StyleIndex);
                }
            }
        }
        /// <summary>
        /// 处理元素样式发生改变事件
        /// </summary>
        public override void OnStyleChanged()
        {
            base.OnStyleChanged();
            UpdateChildStyleIndex();
        }

        /// <summary>
        /// 修复文档DOM结构数据
        /// </summary>
        public override void FixDomState()
        {
            base.FixDomState();
            var doc = this._OwnerDocument;
            if (this._StartElement != null)
            {
                this._StartElement.InnerSetOwnerDocumentParentRaw(doc, this);
            }
            if (this._EndElement != null)
            {
                this._EndElement.InnerSetOwnerDocumentParentRaw(doc, this);
            }
            if (this._InnerBackgroundTextElements != null)
            {
                var arr = this._InnerBackgroundTextElements;
                for (var iCount = arr.Length - 1; iCount >= 0; iCount--)
                {
                    var e = arr[iCount];
                    e.InnerSetOwnerDocumentParentRaw(doc, this);
                    e._StyleIndex = this._StyleIndex;
                }
            }
        }

        /// <summary>
        /// 文档中第一个内容元素
        /// </summary>
       //////[Browsable( false )]
        public override DomElement FirstContentElement
        {
            get
            {
                DomElement e = this._StartElement;
                if (e == null)
                {
                    return base.FirstContentElement;
                }
                else
                {
                    return e;
                }
            }
        }


        /// <summary>
        /// 文档中最后一个内容元素
        /// </summary>
        //////[Browsable( false )]
        public override DomElement LastContentElement
        {
            get
            {
                DomElement e = this._EndElement;
                if (e == null)
                {
                    return base.LastContentElement;
                }
                else
                {
                    return e;
                }
            }
        }

        /// <summary>
        /// 获得输入焦点
        /// </summary>
        //[DCSoft.Common.DCPublishAPI]
        public override void Focus()
        {
            if (this.BelongToDocumentDom(this.OwnerDocument) == false)
            {
                // 元素不在文档中
                return;
            }
            if (this.StartElement != null)
            {
                DomDocumentContentElement dce = this.DocumentContentElement;
                if (this.OwnerDocument.CurrentContentElement != dce)
                {
                    dce.Focus();
                }
                int index = this.StartElement.ContentIndex + 1;
                dce.Content.MoveToPosition(index);
                if (index != this.StartElement.ContentIndex + 1)
                {
                    index = this.StartElement.ContentIndex + 1;
                    dce.Content.MoveToPosition(index);
                }
            }
            else
            {
                base.Focus();
            }
        }



        /// <summary>
        /// 添加文档内容
        /// </summary>
        /// <param name="content"></param>
        /// <param name="privateMode"></param>
        public override int AppendViewContentElement(AppendViewContentElementArgs args)
        {
            //if (this.OwnerDocument.Printing)
            //{
            //    // 若处于打印模式则只添加可见元素
            //    return base.AppendViewContentElement(content, privateMode);
            //}
            //else
            var content = args.Content;
            var document = this.OwnerDocument;
            {
                int result = 0;
                var se = this.StartElement;
                var ee = this.EndElement;
                if (se != null)
                {
                    //this._StartElement.Parent = this;
                    content.Add(se);
                    result++;
                }
                int result2 = base.AppendViewContentElement(args);
                this.HasContentElement = result2 > 0;
                result = result + result2;
                if (ee != null)
                {
                    //this.EndElement.Parent = this;
                    content.Add(ee);
                    result++;
                }
                return result;
            }
        }

        /// <summary>
        /// 底层的添加视图内容元素
        /// </summary>
        /// <param name="content"></param>
        /// <param name="privateMode"></param>
        /// <returns></returns>
        internal protected int XTextContainerElement_AppendViewContentElement(AppendViewContentElementArgs args)
        {
            return base.AppendViewContentElement(args);
        }

        /// <summary>
        /// 声明用户界面无效，需要重新绘制
        /// </summary>
        //[DCSoft.Common.DCPublishAPI]
        public override void InvalidateView()
        {
            if (this.Parent == null)
            {
                // 对象的父节点还没有,说明它还没插入到文档树状结构中
                // 此时对象时不可能显示,因此无需声明用户界面无效.
                return;
            }
            if (this.RuntimeVisible == false)
            {
                return;
            }
            var document = this.OwnerDocument;
            if (document != null
                && document.EditorControl != null
                && this.DocumentContentElement != null)
            {
                RectangleF rect = RectangleF.Empty;
                var se = this._StartElement;
                var ee = this._EndElement;
                if (se != null
                    && ee != null
                    && se._OwnerLine != null
                    && ee._OwnerLine != null
                    && se._OwnerLine.Count > 0
                    && ee._OwnerLine.Count > 0)
                {
                    if (se._OwnerLine != ee._OwnerLine)
                    {
                        // 输入域出现跨行
                        rect = RectangleF.Union(se.OwnerLine.AbsBounds, ee.OwnerLine.AbsBounds);
                        document.EditorControl.ViewInvalidate(
                            Rectangle.CeilingFloat(rect),
                            this.DocumentContentElement.ContentPartyStyle);
                        return;
                    }
                    rect = RectangleF.Union(se.GetAbsBounds(), ee.GetAbsBounds());
                }

                DomElementList list = new DomElementList();
                var args6 = new AppendViewContentElementArgs(document, list, false);
                DomContainerElement.GlobalEnabledResetChildElementStats = false;
                this.AppendViewContentElement(args6);
                DomContainerElement.GlobalEnabledResetChildElementStats = true;
                DomLine lastLine = null;
                foreach (DomElement element in list.FastForEach())
                {
                    if (element.OwnerLine != null && element.OwnerLine.Count > 0)
                    {
                        RectangleF ea = element.GetAbsBounds();
                        if (element.OwnerLine != lastLine)
                        {
                            lastLine = element.OwnerLine;
                            if (lastLine != null)
                            {
                                ea.Y = lastLine.AbsTop;
                                ea.Height = lastLine.Height;
                            }
                        }
                        if (rect.IsEmpty)
                        {
                            rect = ea;
                        }
                        else
                        {
                            rect = RectangleF.Union(rect, ea);
                        }
                    }
                }
                document.EditorControl?.ViewInvalidate(
                    Rectangle.CeilingFloat(rect),
                    this.DocumentContentElement.ContentPartyStyle);
            }
        }

        /// <summary>
        /// 复制对象
        /// </summary>
        /// <param name="Deeply">复制品</param>
        /// <returns>复制品</returns>
        public override DomElement Clone(bool Deeply)
        {
            //XTextDocument doc = this.OwnerDocument;
            DomFieldElementBase field = (DomFieldElementBase)base.Clone(Deeply);
            field._StartElement = null;
            field._EndElement = null;
            field._InnerBackgroundTextElements = null;
            return field;
        }

        public override void Dispose()
        {
            base.Dispose();
            this.DisposeBackgroundTextElements();
            this._StartElement = null;
            this._EndElement = null;
        }
        private void DisposeBackgroundTextElements()
        {
            if (this._InnerBackgroundTextElements != null)
            {
                var arr = this._InnerBackgroundTextElements;
                this._InnerBackgroundTextElements = null;
                for (var iCount = arr.Length - 1; iCount >= 0; iCount--)
                {
                    arr[iCount].Dispose();
                }
                Array.Clear(arr);
            }
        }
    }
}