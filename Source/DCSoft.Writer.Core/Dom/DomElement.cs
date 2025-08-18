using System;
using System.ComponentModel;
using DCSoft.Drawing;
using DCSoft.Common;
using DCSoft.Writer.Controls;
using System.Runtime.InteropServices;
using System.Text;
using DCSoft.Writer.Data;
using DCSoft.Printing;
using System.Collections;
namespace DCSoft.Writer.Dom
{
    /// <summary>
    /// 文档元素基础类型
    /// </summary>
    /// <remarks>
    /// 本类型是文本文档对象模型的最基础的类型,任何其他的文本文档对象类型都是从该类型
    /// 派生的,本类型定义了所有文本文档对象所需要的通用程序,并定义了一些常用例程.
    /// 编制 袁永福 2012-1-12
    /// </remarks>
    public abstract partial class DomElement :
        IDisposable
    {
        internal static readonly DomElement[] EmptyArray = Array.Empty<DomElement>();

        internal static bool DebugModeToString = false;

        internal static int _ElementInstanceIndexCount = 0;
        /// <summary>
        /// 初始化对象
        /// </summary>
        protected DomElement()
        {
        }

        /// <summary>
        /// 获得运行时的鼠标光标对象
        /// </summary>
        internal virtual System.Windows.Forms.Cursor RuntimeDefaultCursor()
        {
            return this.ElementDefaultCursor();
        }


        /// <summary>
        /// 文档元素默认鼠标光标对象
        /// </summary>
        internal virtual System.Windows.Forms.Cursor ElementDefaultCursor()
        {
            return null;
        }
        /// <summary>
        /// 初始化DOM状态
        /// </summary>
        /// <param name="parent">父元素对象</param>
        /// <param name="doc">文档对象</param>
        /// <param name="styleIndex">样式编号</param>
        internal void InitDomState(DomContainerElement parent, DomDocument doc, int styleIndex)
        {
            this._Parent = parent;
            this._OwnerDocument = doc;
            this._StyleIndex = styleIndex;
        }

        /// <summary>
        /// 获得提示文本信息对象
        /// </summary>
        /// <returns></returns>
        public virtual ElementToolTip GetToolTipInfo()
        {
            return null;
        }


        /// <summary>
        /// 能否被选中
        /// </summary>
        public virtual bool RuntimeSelectable()
        {

            return true;

        }

        /// <summary>
        /// 类型名称
        /// </summary>
        public virtual string TypeName
        {
            get
            {
                return this.GetType().Name;
            }
        }

        /// <summary>
        /// 获得供显示的类型名称
        /// </summary>
        public string DispalyTypeName()
        {

            return WriterUtilsInner.GetTypeDisplayName(this.GetType());

        }
        /// <summary>
        /// 内容是否修改标记
        /// </summary>
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public virtual bool Modified
        {
            get { return false; }
            set { }
        }
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public virtual string ID { get { return null; } set { } }

        /// <summary>
        /// 文档元素编号前缀
        /// </summary>
        public virtual string ElementIDPrefix()
        {
            return "element";
        }

        protected DomDocument _OwnerDocument;
        /// <summary>
        /// 对象所属的文档对象
        /// </summary>
        public virtual DomDocument OwnerDocument
        {
            get
            {
                return this._OwnerDocument;
            }
            set
            {
                this._OwnerDocument = value;
                this._RuntimeStyle = null;
            }
        }

        /// <summary>
        /// 创建画布对象
        /// </summary>
        /// <returns>画布对象</returns>
        public virtual DCGraphics InnerCreateDCGraphics()
        {
            return this._OwnerDocument.InnerCreateDCGraphics();
        }

        /// <summary>
        /// 快速的获得对象所属文档对象
        /// </summary>
        internal protected DomDocument ElementOwnerDocument()
        {
            return this._OwnerDocument;
        }

        /// <summary>
        /// 获得视图文档选项
        /// </summary>
        /// <returns>视图文档选项</returns>
        public virtual DocumentViewOptions GetDocumentViewOptions()
        {
            return this._OwnerDocument?.GetDocumentViewOptions();
        }

        /// <summary>
        /// 获得文档行为配置选项
        /// </summary>
        /// <returns></returns>
        public virtual DocumentBehaviorOptions GetDocumentBehaviorOptions()
        {
            return this._OwnerDocument?.GetDocumentBehaviorOptions();
        }

        /// <summary>
        /// 获得编辑配置选项
        /// </summary>
        /// <returns></returns>
        public virtual DocumentEditOptions GetDocumentEditOptions()
        {
            return this._OwnerDocument?.GetDocumentEditOptions();
        }


        /// <summary>
        /// 获得文档选项
        /// </summary>
        /// <returns></returns>
        public virtual DocumentOptions GetDocumentOptions()
        {
            return this._OwnerDocument?.GetDocumentOptions();
        }

        internal void IncreaseDocumentLayoutVersion()
        {
            this._OwnerDocument?.IncreaseLayoutVersion();
        }

        internal void SetOwnerDocumentRaw(DomDocument doc)
        {
            if (this._OwnerDocument != doc)
            {
                this._OwnerDocument = doc;
                this._RuntimeStyle = null;
            }
        }
        /// <summary>
        /// 内部的直接设置文档和父对象
        /// </summary>
        /// <param name="doc">文档对象</param>
        /// <param name="parent">父对象</param>
        public void InnerSetOwnerDocumentParentRaw(DomDocument doc, DomContainerElement parent)
        {
            this._OwnerDocument = doc;
            this._Parent = parent;
            this._RuntimeStyle = null;
        }

        /// <summary>
        /// 内部的直接设置文档和父对象
        /// </summary>
        /// <param name="srcElement">来源文档元素</param>
        public void InnerSetOwnerDocumentParentRaw2(DomElement srcElement)
        {
            if (srcElement != null)
            {
                this._OwnerDocument = srcElement._OwnerDocument;
                this._Parent = srcElement._Parent;
                this._RuntimeStyle = null;
            }
        }

        internal void FixDomStateForMove(DomContainerElement newParent)
        {
            DomDocument oldDoc = this.OwnerDocument;
            if (this.Parent != null)
            {
                this.Parent.RemoveChild(this);
            }
            if (oldDoc != null && oldDoc != newParent.OwnerDocument && newParent.OwnerDocument != null)
            {
                newParent.OwnerDocument.ImportElement(this);
            }
            this.Parent = newParent;
            this.OwnerDocument = newParent.OwnerDocument;
        }

        /// <summary>
        /// 文档元素所在的编辑器控件对象
        /// </summary>
        public virtual WriterControl WriterControl
        {
            get
            {
                return this._OwnerDocument?.WriterControl;
            }
        }

        /// <summary>
        /// 获得内部文档视图控件
        /// </summary>
        /// <returns>视图控件对象</returns>
        public WriterViewControl InnerViewControl
        {
            get
            {
                return this._OwnerDocument?.WriterControl?.GetInnerViewControl();
            }
        }
        /// <summary>
        /// 元素内容的文本内容
        /// </summary>
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public virtual string InnerText
        {
            get
            {
                return this.Text;
            }
            set
            {
                this.Text = value;
            }
        }

        /// <summary>
        /// 元素内容和元素本身的文本内容
        /// </summary>
        public virtual string OuterText
        {
            get
            {
                return this.Text;
            }
            set
            {
                this.Text = value;
            }
        }

        /// <summary>
        /// 创建新的文档对象，使其包含本文档元素的复制品，这个函数非常慢，耗内存，慎用。
        /// </summary>
        /// <param name="includeThis">是否包含本文档原始对象</param>
        /// <returns>创建的文档对象</returns>
        public virtual DomDocument CreateContentDocument(bool includeThis)
        {
            if (this.OwnerDocument == null)
            {
                return null;
            }
            if (includeThis)
            {
                DomElementList elements = new DomElementList();
                elements.Add(this.Clone(true));
                return WriterUtilsInner.CreateDocument(this.OwnerDocument, elements, true);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 判断元素是否挂在指定文档的DOM结构中
        /// </summary>
        /// <param name="document">文档对象</param>
        /// <returns>是否挂在DOM结构中</returns>
        public virtual bool BelongToDocumentDom(DomDocument document)
        {
            if (document == null)
            {
                return false;
            }
            if (this is DomCharElement && this.Parent is DomInputFieldElementBase)
            {
                var field = (DomInputFieldElementBase)this.Parent;
                if (field.IsBackgroundTextElementDom(this))
                {
                    return field.BelongToDocumentDom(document);
                }
            }
            DomElement current = this;
            while (current != null)
            {
                if (current == document)
                {
                    return true;
                }
                var p = current.Parent;
                if (p != null && p.ContainsChild(current))
                {
                    current = p;
                }
                else
                {
                    return false;
                }
            }
            return false;
        }

        /// <summary>
        /// 元素所属的文档级内容元素对象
        /// </summary>
        public virtual DomDocumentContentElement DocumentContentElement
        {
            get
            {
                return GetParentByType<DomDocumentContentElement>();
            }
        }

        protected void CheckDocumentContentElementFocused()
        {
            DomDocumentContentElement dce = this.DocumentContentElement;
            if (dce != null)
            {
                if (dce != this._OwnerDocument.CurrentContentElement)
                {
                    dce.Focus();
                }
            }
        }
        /// <summary>
        /// 元素所属的页面区域类型
        /// </summary>
        public PageContentPartyStyle OwnerPagePartyStyle
        {
            get
            {
                DomDocumentContentElement dce = this.DocumentContentElement;
                if (dce == null)
                {
                    return PageContentPartyStyle.Body;
                }
                else
                {
                    return dce.PagePartyStyle;
                }
            }
        }

        /// <summary>
        /// 元素所属的内容元素对象
        /// </summary>
        public virtual DomContentElement ContentElement
        {
            get
            {
                return GetParentByType<DomContentElement>();
            }
        }

        /// <summary>
        /// 修正DOM结构信息
        /// </summary>
        public virtual void FixDomState()
        {
        }

        /// <summary>
        /// 获得该元素所在的表格单元格对象
        /// </summary>
        public virtual DomTableCellElement OwnerCell
        {
            get
            {
                return GetParentByType<DomTableCellElement>();
            }
        }

        /// <summary>
        /// 获得该元素所在的表格对象
        /// </summary>
        public virtual DomTableElement OwnerTable
        {
            get
            {
                return GetParentByType<DomTableElement>();
            }
        }


        internal ElementType GetParentByType<ElementType>() where ElementType : DomElement
        {
            DomElement element = this;
            while (element != null)
            {
                if (element is ElementType)
                {
                    return (ElementType)element;
                }
                else if (element is DomDocument)
                {
                    // 文档元素没有上级节点
                    return default(ElementType);
                }
                element = element.Parent;
            }
            return default(ElementType);
        }

        /// <summary>
        /// 元素在Header/Body/Footer.Content列表中的序号。
        /// </summary>
        /// <remarks>
        /// 当文档包含大量的元素时,XTextContent中包含了大量的元素,此时它的 IndexOf 函数执行
        /// 缓慢,此处用该属性来预先设置到元素在 XTextContent中的序号,以此来代替或加速 IndexOf 函数
        /// </remarks>
        internal int _ContentIndex = -1;
        internal void SetContentIndex(int v)
        {
            this._ContentIndex = v;
        }

        /// <summary>
        /// 元素在Header/Body/Footer.Content列表中的序号。
        /// </summary>
        /// <remarks>
        /// 当文档包含大量的元素时,XTextContent中包含了大量的元素,此时它的 IndexOf 函数执行
        /// 缓慢,此处用该属性来预先设置到元素在 XTextContent中的序号,以此来代替或加速 IndexOf 函数
        /// </remarks>
        public int ContentIndex
        {
            get
            {
                return this._ContentIndex;
            }
        }
        internal int LayoutIndex
        {
            get
            {
                return this._ContentIndex;
            }
        }
        /// <summary>
        /// 元素在所属的XTextContentElement.PrivateContent列表中的序号。
        /// </summary>
        /// <remarks>
        /// 当文档包含大量的元素时,XTextContent中包含了大量的元素,此时它的 IndexOf 函数执行
        /// 缓慢,此处用该属性来预先设置到元素在 PrivateContent中的序号,以此来代替或加速 IndexOf 函数
        /// </remarks>
        [NonSerialized]
        internal int _PrivateContentIndex = -1;
        /// <summary>
        /// 元素在所属的XTextContentElement.PrivateContent列表中的序号。
        /// </summary>
        /// <remarks>
        /// 当文档包含大量的元素时,XTextContent中包含了大量的元素,此时它的 IndexOf 函数执行
        /// 缓慢,此处用该属性来预先设置到元素在 PrivateContent中的序号,以此来代替或加速 IndexOf 函数
        /// </remarks>
        internal int PrivateContentIndex()
        {
            return this._PrivateContentIndex;
        }

        /// <summary>
        /// 运行时的元素是否显示
        /// </summary>
        protected internal bool _RuntimeVisible = true;
        /// <summary>
        /// 运行时元素是否显示
        /// </summary>
        public virtual bool RuntimeVisible
        {
            get
            {
                return _RuntimeVisible;
            }
            set
            {
                if (this._RuntimeVisible != value)
                {

                }
                this._RuntimeVisible = value;
            }
        }

        /// <summary>
        /// 设置元素为选中状态
        /// </summary>
        public virtual bool Select()
        {
            CheckDocumentContentElementFocused();
            DomDocumentContentElement dce = this.DocumentContentElement;
            if (dce == null)
            {
                return false;
            }
            if (this.OwnerDocument.CurrentContentElement != dce)
            {
                dce.Focus();
            }
            int index = dce.Content.IndexOf(this);
            if (index >= 0)
            {
                this.OwnerDocument.ClearPagesClientSelectionBounds();
                dce.Content.SetSelection(index, 1);
                return true;
            }
            else
            {
                return false;
            }
        }


        /// <summary>
        ///  设计时元素是否可见
        /// </summary>
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public virtual bool Visible
        {
            get
            {
                return true;
            }
            set
            {
            }
        }
        /// <summary>
        /// 设置容器元素的可见性
        /// </summary>
        /// <param name="visible">新的可见性</param>
        /// <param name="logUndo">是否记录撤销操作信息</param>
        /// <param name="fastMode">是否启用快速模式，当使用快速模式时，
        /// 只更新DOM结构，不更新用户界面视图</param>
        /// <returns>操作是否成功</returns>
        public virtual bool EditorSetVisibleExt(bool visible, bool logUndo, bool fastMode)
        {
            bool result = false;
            bool oldVisible = this.Visible;
            if (oldVisible != visible)
            {
                this.Visible = visible;
                this.RuntimeVisible = this.Visible && this.OwnerDocument.IsVisible(this);
                if (this.OwnerDocument == null || this.OwnerDocument.ReadyState != DomReadyStates.Complete)
                {
                    return false;
                }
                bool visible2 = this.Visible;
                if (visible2 == visible)
                {
                    if (logUndo)
                    {
                        if (this.OwnerDocument.BeginLogUndo())
                        {
                            this.OwnerDocument.UndoList.AddVisible(this, oldVisible, this.Visible);
                            this.OwnerDocument.EndLogUndo();
                        }
                    }
                    this.InvalidateView();
                    // 成功的修改了元素的可见性
                    if (this.OwnerDocument.HighlightManager != null)
                    {
                        if (visible)
                        {
                            this.OwnerDocument.HighlightManager.InvalidateHighlightInfo(this);
                        }
                        else
                        {
                            this.OwnerDocument.HighlightManager.Remove(this);
                        }
                    }
                    result = true;
                    DomElement fc = this.FirstContentElement;
                    //XTextElement lc = this.LastContentElement;
                    DomContentElement content = this.ContentElement;
                    int startIndex = 0;
                    DomElement preElement = content.PrivateContent.GetPreElement(fc);
                    if (oldVisible)
                    {
                        startIndex = content.PrivateContent.IndexOf(fc);
                    }
                    this.UpdateContentVersion();
                    DomDocumentContentElement dce = this.DocumentContentElement;
                    DomElement currentElementBack = dce.CurrentElement;
                    content.UpdateContentElements(true);
                    if (oldVisible == false)
                    {
                        startIndex = content.PrivateContent.IndexOf(fc);
                    }
                    else
                    {
                        if (preElement != null && content.PrivateContent.Contains(preElement))
                        {
                            startIndex = content.PrivateContent.IndexOf(preElement);
                        }
                    }
                    content.RefreshPrivateContent(startIndex, -1, fastMode);
                    if (fastMode == false)
                    {
                        if (currentElementBack != null)
                        {
                            int index = dce.Content.IndexOf(currentElementBack);
                            if (index >= 0)
                            {

                            }
                        }
                    }
                    this.OwnerDocument.Modified = true;
                    this.OwnerDocument.OnSelectionChanged();
                    if (this.OwnerDocument.EditorControl != null)
                    {
                        this.OwnerDocument.EditorControl.UpdateTextCaret();
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 父对象
        /// </summary>
        internal DomContainerElement _Parent;
        internal bool EqualsParent(DomElement e)
        {
            return this._Parent == e._Parent;
        }
        /// <summary>
        /// 父对象
        /// </summary>
        public virtual DomContainerElement Parent
        {
            get
            {
                return this._Parent;
            }
            set
            {
                if (this._Parent != value)
                {
                    this._Parent = value;
                    if (this._Parent != null)
                    {
                        this.OwnerDocument = this._Parent.OwnerDocument;
                    }
                }
            }
        }
        /// <summary>
        /// 文档元素是否具有相同的父元素
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public bool HasSameParent(DomElement e)
        {
            if (e != null && e._Parent == this._Parent)
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// 内部的直接设置元素父节点对象
        /// </summary>
        /// <param name="c"></param>
        public void SetParentRaw(DomContainerElement c)
        {
            this._Parent = c;
        }

        /// <summary>
        /// 内部的直接设置元素父节点及所属文档元素
        /// </summary>
        /// <param name="c">父元素</param>
        public void SetParentAndDocumentRaw(DomContainerElement c)
        {
            this._Parent = c;
            this._OwnerDocument = c._OwnerDocument;
        }
        /// <summary>
        /// 判断指定元素是否是本元素的父节点或者更高层次的父节点。
        /// </summary>
        /// <param name="parentElement">要判断的元素</param>
        /// <returns>是否是父节点或者更高级的父节点</returns>
        public virtual bool IsParentOrSupParent(DomElement parentElement)
        {
            if (this._Parent == parentElement._Parent && this._Parent != null)
            {
                // 具有相同的父节点，之间肯定没有父子关系。
                return false;
            }
            DomElement parent = this._Parent;
            while (parent != null)
            {
                if (parent == parentElement)
                {
                    return true;
                }
                if (parent is DomDocument)
                {
                    // 已经到顶了
                    break;
                }
                parent = parent._Parent;
            }
            return false;
        }
        internal int _ElementIndex = -1;
        /// <summary>
        /// 元素在父节点的子节点列表中的序号
        /// </summary>
        public virtual int ElementIndex
        {
            get
            {
                return _ElementIndex;
            }
            set
            {
                _ElementIndex = value;
            }
        }

        /// <summary>
        /// 对象所在的文本行对象
        /// </summary>
        internal DomLine _OwnerLine;
        /// <summary>
        /// 对象所在的文本行对象
        /// </summary>
        public DomLine OwnerLine
        {
            get
            {
                return _OwnerLine;
            }
        }
        /// <summary>
        /// 设置元素所在的文档行对象
        /// </summary>
        /// <param name="line"></param>
        internal void SetOwnerLine(DomLine line)
        {
            this._OwnerLine = line;
        }


        /// <summary>
        /// 获得排版的右边的元素
        /// </summary>
        internal DomElement RightLayoutElement()
        {

            DomLine line = this.OwnerLine;
            if (line != null)
            {
                return line.LayoutElements.GetNextElement(this);
            }
            return null;

        }

        /// <summary>
        /// 对象在文本行中的从1开始的列号
        /// </summary>
        public int ColumnIndex
        {
            //[JSInvokable]
            get
            {
                if (_OwnerLine != null)
                {
                    return _OwnerLine.IndexOf(this) + 1;
                }
                else
                {
                    return -1;
                }
            }
        }

        /// <summary>
        /// 获得一个从0开始计算的当前元素所在的页码
        /// </summary>
        public virtual int OwnerPageIndex
        {
            //[JSInvokable]
            get
            {
                var doc = this.OwnerDocument;
                if (doc == null)
                {
                    return -1;
                }
                DomElement e = this.FirstContentElement;
                if (e != null)
                {
                    DomLine line = e.OwnerLine;
                    if (line != null && line.OwnerPage != null)
                    {
                        if (doc == null)
                        {
                            return line.OwnerPage.PageIndex;
                        }
                        else
                        {
                            return doc.Pages.IndexOf(line.OwnerPage);
                        }
                    }
                    else
                    {
                    }
                }
                return -1;
            }
        }

        /// <summary>
        /// 获得一个从0开始计算的元素下边缘所能到达的页码
        /// </summary>
        public virtual int OwnerLastPageIndex
        {
            //[JSInvokable]
            get
            {
                if (this.OwnerDocument == null)
                {
                    return -1;
                }
                float bottom = this.GetAbsTop() + this.Height;
                int index = this.OwnerDocument.Pages.IndexOfByPosition(bottom - 0.5f, -1);
                return index;
            }
        }

        /// <summary>
		/// 判断是否本元素或者子孙元素处于选择状态
		/// </summary>
        public virtual bool HasSelection
        {
            //[JSInvokable]
            get
            {
                if (this.DocumentContentElement == null)
                {
                    return false;
                }
                else
                {
                    return this.DocumentContentElement.IsSelected(this);
                }
            }
        }

        /// <summary>
        /// 设置文档元素样式
        /// </summary>
        /// <param name="style">文档样式</param>
        /// <returns>操作是否成功</returns>
        public virtual bool EditorSetStyle(DocumentContentStyle style)
        {
            if (this._OwnerDocument == null)
            {
                throw new NullReferenceException(DCSR.NeedSetOwnerDocument);
            }
            if (style == null)
            {
                throw new ArgumentNullException("style");
            }
            this.StyleIndex = this.OwnerDocument.ContentStyles.GetStyleIndex(style);
            return true;
        }

        /// <summary>
        /// DCWriter内部使用。内置的提交样式信息
        /// </summary>
        public virtual void InnerCommitStyleDeeply()
        {
            this.CommitStyle(false);
        }

        /// <summary>
        /// 提交当前编辑的样式信息
        /// </summary>
        /// <param name="logUndo">是否记录撤销操作信息</param>
        /// <returns>操作是否改变了元素状态</returns>
        internal protected bool CommitStyle(bool logUndo)
        {
            if (this._Style != null && this._Style.ValueModified)
            {
                // 文档元素样式发生改变
                DomDocument document = this._OwnerDocument;
                if (document != null)
                {
                    int si = this._StyleIndex;
                    this._StyleIndex = document.ContentStyles.GetStyleIndex(this._Style);
                    if (logUndo)
                    {
                        // 记录撤销操作信息
                        if (document.CanLogUndo)
                        {
                            document.UndoList.AddStyleIndex(this, si, this._StyleIndex);
                        }
                    }
                    this._RuntimeStyle = null;
                    this._Style = null;
                    return true;
                }
            }
            return false;
        }

        internal void CommitStyleFast()
        {
            if (this._Style != null && this._Style.ValueModified)
            {
                // 文档元素样式发生改变
                DomDocument document = this._OwnerDocument;
                if (document != null)
                {
                    //int si = this._StyleIndex;
                    this._StyleIndex = document.ContentStyles.GetStyleIndex(this._Style);
                    this._RuntimeStyle = null;
                    this._Style = null;
                }
            }
        }
        /// <summary>
        /// 运行时使用的样式对象
        /// </summary>
        internal RuntimeDocumentContentStyle _RuntimeStyle;

        /// <summary>
        /// 重置RuntimeStyle属性值
        /// </summary>
        internal void ResetRuntimeStyle()
        {
            this._RuntimeStyle = null;
        }

        internal void CheckRuntimeStyle()
        {
            if (this._Style != null)
            {
                CommitStyle(false);
            }
            if (_RuntimeStyle == null)
            {
                this._RuntimeStyle = this._OwnerDocument?._ContentStyles?.GetDCRuntimeStyle(this._StyleIndex);
            }
        }
        /// <summary>
        /// 绘制文档内容使用的样式
        /// </summary>
        public virtual RuntimeDocumentContentStyle RuntimeStyle
        {
            get
            {
                if (this._Style != null)
                {
                    CommitStyle(false);
                }
                if (this._RuntimeStyle == null)
                {
                    this._RuntimeStyle = this._OwnerDocument?._ContentStyles?.GetDCRuntimeStyle(this._StyleIndex);
                }
                return this._RuntimeStyle;
            }
        }
        /// <summary>
        /// 当前样式
        /// </summary>
        protected DocumentContentStyle _Style;
        /// <summary>
        /// 文档显示样式
        /// </summary>
        public virtual DocumentContentStyle Style
        {
            get
            {
                if (this._Style == null)
                {
                    if (this._RuntimeStyle != null)
                    {
                        this._Style = (DocumentContentStyle)this._RuntimeStyle.Parent.Clone();
                        this._Style.ValueModified = false;
                    }
                    else
                    {
                        var style = this.OwnerDocument?.ContentStyles?.GetStyle(this._StyleIndex);
                        if (style != null)
                        {
                            this._Style = (DocumentContentStyle)style.Clone();
                        }
                        else
                        {
                            this._Style = new DocumentContentStyle();
                        }
                    }
                    this._Style.ValueModified = false;

                }
                return this._Style;

            }
            set
            {
                this._Style = value;
                if (this._Style != null)
                {
                    this._Style.ValueModified = true;
                }
            }
        }


        protected internal int _StyleIndex = -1;
        /// <summary>
        /// 使用的样式编号
        /// </summary>
        public virtual int StyleIndex
        {
            get
            {
                return _StyleIndex;
            }
            set
            {
                if (_StyleIndex != value)
                {
                    this._StyleIndex = value;
                    this.SizeInvalid = true;
                    this._RuntimeStyle = null;
                    this._Style = null;
                }
            }
        }
        internal void SetStyleIndexForce(int newStyleIndex)
        {
            this._StyleIndex = newStyleIndex;
            this.SizeInvalid = true;
            this._RuntimeStyle = null;
            this._Style = null;
        }


        /// <summary>
        /// 元素样式发生改变事件
        /// </summary>
        public virtual void OnStyleChanged()
        {
        }

        /// <summary>
        /// 子对象列表
        /// </summary>
        public virtual DomElementList Elements
        {
            get
            {
                return null;
            }
            set
            {
            }
        }

        /// <summary>
        /// 元素中第一个显示在文档正文内容中的元素 
        /// </summary>
        public virtual DomElement FirstContentElementInPublicContent
        {
            get
            {
                return this.FirstContentElement;
            }
        }


        /// <summary>
        /// 元素中最后一个显示在文档正文内容中的元素 
        /// </summary>
        public virtual DomElement LastContentElementInPublicContent
        {
            get
            {
                return this.LastContentElement;
            }
        }


        /// <summary>
        /// 元素中第一个显示在文档内容中的元素 
        /// </summary>
        public virtual DomElement FirstContentElement
        {
            get
            {
                return this;
            }
        }

        /// <summary>
        /// 元素中最后一个显示在文档内容中的元素 
        /// </summary>
        public virtual DomElement LastContentElement
        {
            get
            {
                return this;
            }
        }

        /// <summary>
        /// 声明元素的视图无效,需要重新绘制
        /// </summary>
        public virtual void InvalidateView()
        {
            if (this._RuntimeVisible)
            {
                this._OwnerDocument?.InvalidateElementView(this);
            }
        }
        /// <summary>
        /// 是否为巨大的表格
        /// </summary>
        /// <returns></returns>
        public bool IsBigTable()
        {
            return this.Height > 100000;
        }
        /// <summary>
        /// 快速的声明文档排版状态无效。
        /// </summary>
        internal void InvalidateDocumentLayoutFast()
        {
            if (this._OwnerDocument != null)
            {
                this._OwnerDocument.InvalidateLayoutFast = true;
            }
        }
        /// <summary>
        /// 声明元素的高亮度显示信息无效
        /// </summary>
        public virtual void InvalidateHighlightInfo()
        {
            var doc = this.OwnerDocument;
            if (doc != null && doc.IsLoadingDocument == false)
            {
                var man = doc.HighlightManager;
                if (man != null)
                {
                    man.InvalidateHighlightInfo(this);
                }
            }
        }

        private const int StateMask01 = 1;
        private const int StateMask02 = 1 << 1;
        private const int StateMask03 = 1 << 2;
        private const int StateMask04 = 1 << 3;
        private const int StateMask05 = 1 << 4;
        private const int StateMask06 = 1 << 5;
        private const int StateMask07 = 1 << 6;
        private const int StateMask08 = 1 << 7;
        private const int StateMask09 = 1 << 8;
        private const int StateMask10 = 1 << 9;
        private const int StateMask11 = 1 << 10;
        private const int StateMask12 = 1 << 11;
        private const int StateMask13 = 1 << 12;
        private const int StateMask14 = 1 << 13;
        private const int StateMask15 = 1 << 14;
        private const int StateMask16 = 1 << 15;
        private const int StateMask17 = 1 << 16;
        private const int StateMask18 = 1 << 17;
        private const int StateMask19 = 1 << 18;
        private const int StateMask20 = 1 << 19;
        private const int StateMask21 = 1 << 20;
        private const int StateMask22 = 1 << 21;
        private const int StateMask23 = 1 << 22;
        private const int StateMask24 = 1 << 23;
        private const int StateMask25 = 1 << 24;
        private const int StateMask26 = 1 << 25;
        private const int StateMask27 = 1 << 26;
        private const int StateMask28 = 1 << 27;
        private const int StateMask29 = 1 << 28;
        private const int StateMask30 = 1 << 29;
        private const int StateMask31 = 1 << 30;
        private const int StateMask32 = 1 << 31;

        private int _EleStates = 3;

        protected bool StateFlag01
        {
            get { return (this._EleStates & StateMask01) != 0; }
            set { this._EleStates = value ? (this._EleStates | StateMask01) : (this._EleStates & ~StateMask01); }
        }
        protected bool StateFlag02
        {
            get { return (this._EleStates & StateMask02) != 0; }
            set { this._EleStates = value ? (this._EleStates | StateMask02) : (this._EleStates & ~StateMask02); }
        }
        protected bool StateFlag03
        {
            get { return (this._EleStates & StateMask03) != 0; }
            set { this._EleStates = value ? (this._EleStates | StateMask03) : (this._EleStates & ~StateMask03); }
        }
        protected bool StateFlag04
        {
            get { return (this._EleStates & StateMask04) != 0; }
            set { this._EleStates = value ? (this._EleStates | StateMask04) : (this._EleStates & ~StateMask04); }
        }
        protected bool StateFlag05
        {
            get { return (this._EleStates & StateMask05) != 0; }
            set { this._EleStates = value ? (this._EleStates | StateMask05) : (this._EleStates & ~StateMask05); }
        }
        protected bool StateFlag06
        {
            get { return (this._EleStates & StateMask06) != 0; }
            set { this._EleStates = value ? (this._EleStates | StateMask06) : (this._EleStates & ~StateMask06); }
        }
        protected bool StateFlag07
        {
            get { return (this._EleStates & StateMask07) != 0; }
            set { this._EleStates = value ? (this._EleStates | StateMask07) : (this._EleStates & ~StateMask07); }
        }
        protected bool StateFlag08
        {
            get { return (this._EleStates & StateMask08) != 0; }
            set { this._EleStates = value ? (this._EleStates | StateMask08) : (this._EleStates & ~StateMask08); }
        }
        protected bool StateFlag09
        {
            get { return (this._EleStates & StateMask09) != 0; }
            set { this._EleStates = value ? (this._EleStates | StateMask09) : (this._EleStates & ~StateMask09); }
        }
        protected bool StateFlag10
        {
            get { return (this._EleStates & StateMask10) != 0; }
            set { this._EleStates = value ? (this._EleStates | StateMask10) : (this._EleStates & ~StateMask10); }
        }
        protected bool StateFlag11
        {
            get { return (this._EleStates & StateMask11) != 0; }
            set { this._EleStates = value ? (this._EleStates | StateMask11) : (this._EleStates & ~StateMask11); }
        }
        protected bool StateFlag12
        {
            get { return (this._EleStates & StateMask12) != 0; }
            set { this._EleStates = value ? (this._EleStates | StateMask12) : (this._EleStates & ~StateMask12); }
        }
        protected bool StateFlag13
        {
            get { return (this._EleStates & StateMask13) != 0; }
            set { this._EleStates = value ? (this._EleStates | StateMask13) : (this._EleStates & ~StateMask13); }
        }
        protected bool StateFlag14
        {
            get { return (this._EleStates & StateMask14) != 0; }
            set { this._EleStates = value ? (this._EleStates | StateMask14) : (this._EleStates & ~StateMask14); }
        }
        protected bool StateFlag15
        {
            get { return (this._EleStates & StateMask15) != 0; }
            set { this._EleStates = value ? (this._EleStates | StateMask15) : (this._EleStates & ~StateMask15); }
        }
        protected bool StateFlag16
        {
            get { return (this._EleStates & StateMask16) != 0; }
            set { this._EleStates = value ? (this._EleStates | StateMask16) : (this._EleStates & ~StateMask16); }
        }
        protected bool StateFlag17
        {
            get { return (this._EleStates & StateMask17) != 0; }
            set { this._EleStates = value ? (this._EleStates | StateMask17) : (this._EleStates & ~StateMask17); }
        }

        protected bool StateFlag18
        {
            get { return (this._EleStates & StateMask18) != 0; }
            set { this._EleStates = value ? (this._EleStates | StateMask18) : (this._EleStates & ~StateMask18); }
        }
        protected bool StateFlag19
        {
            get { return (this._EleStates & StateMask19) != 0; }
            set { this._EleStates = value ? (this._EleStates | StateMask19) : (this._EleStates & ~StateMask19); }
        }
        protected bool StateFlag20
        {
            get { return (this._EleStates & StateMask20) != 0; }
            set { this._EleStates = value ? (this._EleStates | StateMask20) : (this._EleStates & ~StateMask20); }
        }
        protected bool StateFlag21
        {
            get { return (this._EleStates & StateMask21) != 0; }
            set { this._EleStates = value ? (this._EleStates | StateMask21) : (this._EleStates & ~StateMask21); }
        }
        protected bool StateFlag22
        {
            get { return (this._EleStates & StateMask22) != 0; }
            set { this._EleStates = value ? (this._EleStates | StateMask22) : (this._EleStates & ~StateMask22); }
        }
        protected bool StateFlag23
        {
            get { return (this._EleStates & StateMask23) != 0; }
            set { this._EleStates = value ? (this._EleStates | StateMask23) : (this._EleStates & ~StateMask23); }
        }
        protected bool StateFlag24
        {
            get { return (this._EleStates & StateMask24) != 0; }
            set { this._EleStates = value ? (this._EleStates | StateMask24) : (this._EleStates & ~StateMask24); }
        }
        protected bool StateFlag25
        {
            get { return (this._EleStates & StateMask25) != 0; }
            set { this._EleStates = value ? (this._EleStates | StateMask25) : (this._EleStates & ~StateMask25); }
        }
        protected bool StateFlag26
        {
            get { return (this._EleStates & StateMask26) != 0; }
            set { this._EleStates = value ? (this._EleStates | StateMask26) : (this._EleStates & ~StateMask26); }
        }
        protected bool StateFlag27
        {
            get { return (this._EleStates & StateMask27) != 0; }
            set { this._EleStates = value ? (this._EleStates | StateMask27) : (this._EleStates & ~StateMask27); }
        }
        protected bool StateFlag28
        {
            get { return (this._EleStates & StateMask28) != 0; }
            set { this._EleStates = value ? (this._EleStates | StateMask28) : (this._EleStates & ~StateMask28); }
        }
        public bool StateFlag29
        {
            get { return (this._EleStates & StateMask29) != 0; }
            set { this._EleStates = value ? (this._EleStates | StateMask29) : (this._EleStates & ~StateMask29); }
        }
        public bool StateFlag30
        {
            get { return (this._EleStates & StateMask30) != 0; }
            set { this._EleStates = value ? (this._EleStates | StateMask30) : (this._EleStates & ~StateMask30); }
        }
        public bool StateFlag31
        {
            get { return (this._EleStates & StateMask31) != 0; }
            set { this._EleStates = value ? (this._EleStates | StateMask31) : (this._EleStates & ~StateMask31); }
        }
        public bool StateFlag32
        {
            get { return (this._EleStates & StateMask32) != 0; }
            set { this._EleStates = value ? (this._EleStates | StateMask32) : (this._EleStates & ~StateMask32); }
        }
        public virtual bool SizeInvalid
        {
            get { return this.StateFlag01; }
            set { this.StateFlag01 = value; }
        }

        public bool ViewInvalid
        {
            get { return this.StateFlag02; }
            set { this.StateFlag02 = value; }
        }

        /// <summary>
        /// 为文档行排版而设置元素位置
        /// </summary>
        /// <param name="newLeft">新的左端位置</param>
        /// <param name="newTop">新的顶端位置</param>
        /// <returns>是否修改了位置</returns>
        internal bool SetPositionForLineLayout(float newLeft, float newTop)
        {
            if (this._Left != newLeft || this._Top != newTop)
            {
                this._Left = newLeft;
                this._Top = newTop;
                return true;
            }
            return false;
        }
        /// <summary>
        /// 创建绝对坐标的对象边界
        /// </summary>
        /// <param name="parentAbsLeft"></param>
        /// <param name="parentAbsTop"></param>
        /// <returns></returns>
        internal RectangleF CreateAbsBounds(float parentAbsLeft, float parentAbsTop)
        {
            return new RectangleF(
                parentAbsLeft + this._Left,
                parentAbsTop + this._Top,
                this._Width,
                this._Height);
        }
        /// <summary>
        /// 对象左端位置
        /// </summary>
        internal float _Left;
        /// <summary>
        /// 对象左端位置
        /// </summary>
        public virtual float Left
        {
            //[JSInvokable]
            get
            {
                return _Left;
            }
            set
            {
                //if (_Left != value)
                {
                    //if (this.GetHashCode() == 6744269)
                    //{
                    //}
                    _Left = value;
                }
            }
        }
        /// <summary>
        /// 对象顶端位置
        /// </summary>
        internal float _Top;
        /// <summary>
        /// 对象顶端位置
        /// </summary>
        public virtual float Top
        {
            //[JSInvokable]
            get
            {
                return _Top;
            }
            set
            {
                _Top = value;
            }
        }
        /// <summary>
        /// 对象宽度
        /// </summary>
        internal float _Width;
        /// <summary>
        /// 对象宽度
        /// </summary>
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public virtual float Width
        {
            //[JSInvokable]
            get
            {
                return _Width;
            }
            set
            {
                //if( this._Width > 0 && value == 0 && this is XTextCharElement )
                //{
                //    var ss = 5;
                //}
                //if (_Width != value)
                //{
                this._Width = value;
                //}
                //if (value == 0)
                //{
                //}
                //_Width = value;
            }
        }

        /// <summary>
        /// 像素单位的对象宽度
        /// </summary>
        public float PixelWidth
        {
            get
            {
                return GraphicsUnitConvert.Convert(this.Width, GraphicsUnit.Document, GraphicsUnit.Pixel);
            }
            set
            {
                this.Width = GraphicsUnitConvert.Convert(value, GraphicsUnit.Pixel, GraphicsUnit.Document);
            }
        }

        /// <summary>
        /// 像素单位的对象宽度
        /// </summary>
        public float PixelHeight
        {
            get
            {
                return GraphicsUnitConvert.Convert(this.Height, GraphicsUnit.Document, GraphicsUnit.Pixel);
            }
            set
            {
                this.Height = GraphicsUnitConvert.Convert(value, GraphicsUnit.Pixel, GraphicsUnit.Document);
            }
        }

        /// <summary>
        /// 在视图中的显示宽度
        /// </summary>
        public virtual float ViewWidth
        {
            get
            {
                return this._Width;
            }
        }

        /// <summary>
        /// 对象客户区的宽度
        /// </summary>
        public virtual float ClientWidth
        {
            //[JSInvokable]
            get
            {
                if (this._StyleIndex < 0)
                {
                    return this.Width;
                }
                RuntimeDocumentContentStyle rs = this.RuntimeStyle;
                if (rs == null)
                {
                    return this.Width;
                }
                else
                {
                    return this.Width - rs.PaddingLeft - rs.PaddingRight;
                }
            }
            set
            {
                RuntimeDocumentContentStyle rs = this.RuntimeStyle;
                if (rs == null)
                {
                    this.Width = value;
                }
                else
                {
                    this.Width = value + rs.PaddingLeft + rs.PaddingRight;
                }
            }
        }

        /// <summary>
        /// 对象客户区的高度
        /// </summary>
        public virtual float ClientHeight
        {
            //[JSInvokable]
            get
            {
                RuntimeDocumentContentStyle rs = this.RuntimeStyle;
                if (rs == null)
                {
                    return this.Height;
                }
                else
                {
                    return this.Height - rs.PaddingTop - rs.PaddingBottom;
                }
            }
            set
            {
                RuntimeDocumentContentStyle rs = this.RuntimeStyle;
                if (rs == null)
                {
                    this.Height = value;
                }
                else
                {
                    this.Height = value + rs.PaddingTop + rs.PaddingBottom;
                }
            }
        }

        /// <summary>
        /// 对象高度
        /// </summary>
        internal protected float _Height;
        /// <summary>
        /// 对象高度
        /// </summary>
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public virtual float Height
        {
            //[JSInvokable]
            get
            {
                return _Height;
            }
            set
            {
                //if (_Height != value)
                {
                    _Height = value;
                }
            }
        }

        /// <summary>
        /// 对象大小
        /// </summary>
        public SizeF Size
        {
            get
            {
                return new SizeF(this.Width, this.Height);
            }
        }

        /// <summary>
        /// 专为编辑器提供的对象大小属性
        /// </summary>
        public virtual SizeF EditorSize
        {
            get
            {
                return new SizeF(this.Width, this.Height);
            }
            set
            {
                this.Width = value.Width;
                this.Height = value.Height;
            }
        }

        /// <summary>
		/// 对象右边缘位置
		/// </summary>
        public float Right
        {
            //[JSInvokable]
            get
            {
                return this._Left + this._Width;
            }
        }
        /// <summary>
        /// 对象底端位置
        /// </summary>
        public float Bottom
        {
            //[JSInvokable]
            get
            {
                return this._Top + this._Height;
            }
        }

        /// <summary>
        /// 对象宽度修正值
        /// </summary>
        internal float _WidthFix;
        /// <summary>
        /// 对象宽度修正值
        /// </summary>
        public float WidthFix
        {
            get
            {
                return this._WidthFix;
            }
            set
            {
                //if (_WidthFix != value)
                //{
                this._WidthFix = value;
                //}
            }
        }
        /// <summary>
        /// 文档元素在文档页中的距离页面纸张左上角的水平距离。
        /// </summary>
        public virtual float LeftInOwnerPage
        {
            //[JSInvokable]
            get
            {
                var doc = this.OwnerDocument;
                if (doc == null)
                {
                    return 0;
                }
                float result = this.AbsLeft;
                var page = doc.Pages.SafeGet(this.OwnerPageIndex);
                if (page == null)
                {
                    var info = new PageLayoutInfo(doc.PageSettings, true, false);
                    result = info.LeftMargin + result;
                }
                else
                {
                    var info = new PageLayoutInfo(page, true);
                    result = info.LeftMargin + result;
                }
                return result;
            }
        }

        /// <summary>
        /// 文档元素在文档页中的距离页面纸张左上角的垂直距离。
        /// </summary>
        public virtual float TopInOwnerPage
        {
            //[JSInvokable]
            get
            {
                var doc = this.OwnerDocument;
                if (doc == null)
                {
                    return 0;
                }
                float result = this.AbsTop;
                var dce = this.DocumentContentElement;
                if (dce == null)
                {
                    return 0;
                }
                var pps = dce.ContentPartyStyle;

                var ps = doc.PageSettings;
                if (pps == PageContentPartyStyle.Header)
                {
                    var info = new PageLayoutInfo(ps, true, false);
                    result = info.HeaderDistance + result;
                }
                else if (pps == PageContentPartyStyle.Footer)
                {
                    var info = new PageLayoutInfo(ps, true, false);
                    var top = info.PageHeight - info.FooterDistance - dce.Height;
                    result = result + top;
                }
                else //if(pps == PageContentPartyStyle.Body || pps == PageContentPartyStyle.HeaderRows)
                {
                    var pg = doc.Pages.SafeGet(this.OwnerPageIndex);// this.OwnerLine?.OwnerPage;
                    if (pg != null)
                    {
                        var info = new DCSoft.Printing.PageLayoutInfo(pg, true);
                        result = Math.Max(info.TopMargin, info.HeaderDistance + pg.HeaderContentHeight) + result - pg.Top;
                    }
                }
                return result;
            }
        }

        /// <summary>
        /// 对象边界
        /// </summary>
        public RectangleF Bounds
        {
            get
            {
                return new RectangleF(
                    this._Left,
                    this._Top,
                    this._Width,
                    this._Height);
            }
            set
            {
                if (value.Width == 0)
                {

                }
                this._Left = value.Left;
                this._Top = value.Top;
                this._Width = value.Width;
                this._Height = value.Height;
            }
        }

        /// <summary>
        /// 对象在文档中的绝对坐标位置
        /// </summary>
        public virtual float AbsLeft
        {
            //[JSInvokable]
            get
            {
                if (this._OwnerLine == null)
                {
                    if (this.Parent == null)
                    {
                        return this._Left;
                    }
                    else
                    {
                        return this._Left + this.Parent.Left;
                    }
                }
                else
                {
                    return this._Left + this._OwnerLine.AbsLeft;
                }
            }
        }

        /// <summary>
        /// 对象在文档中的绝对坐标位置
        /// </summary>
        public virtual float AbsTop
        {
            //[JSInvokable]
            get
            {
                if (this._OwnerLine == null)
                {
                    if (this.Parent == null)
                    {
                        return this._Top;
                    }
                    else
                    {
                        return this._Top + this.Parent.AbsTop;
                    }
                }
                else
                {
                    return this._Top + this._OwnerLine.AbsTop;
                }
            }
        }
        /// <summary>
        /// 对象在文档中的绝对坐标位置，是AbsLeft的一个为了混淆的封装。
        /// </summary>
        public float GetAbsLeft()
        {
            return this.AbsLeft;
        }
        /// <summary>
        /// 对象在文档中的绝对坐标位置，是AbsTop的一个为了混淆的封装。
        /// </summary>
        public float GetAbsTop()
        {
            return this.AbsTop;
        }
        /// <summary>
        /// 元素在文档视图中的绝对坐标值
        /// </summary>
        public virtual PointF AbsPosition
        {
            get
            {
                float x = 0;
                float y = 0;
                if (this._OwnerLine == null)
                {
                    if (this.Parent == null)
                    {
                        x = this._Left;
                        y = this._Top;
                    }
                    else
                    {
                        var p = this.Parent.AbsPosition;
                        x = this._Left + p.X;
                        y = this._Top + p.Y;
                    }
                }
                else
                {
                    var p = this._OwnerLine.AbsPosition;
                    x = this._Left + p.X;
                    y = this._Top + p.Y;
                }
                return new PointF(x, y);
            }
        }

        /// <summary>
        /// 绝对边界
        /// </summary>
        public virtual RectangleF AbsBounds
        {
            get
            {
                var p = this.AbsPosition;
                return new RectangleF(
                    p.X,
                    p.Y,
                    this.Width,
                    this.Height);
            }
        }
        /// <summary>
        /// 获得绝对边界，是AbsBounds的一个为了混淆的封装。
        /// </summary>
        /// <returns>数值</returns>
        public RectangleF GetAbsBounds()
        {
            return this.AbsBounds;
        }

        /// <summary>
        /// 客户区的绝对边界
        /// </summary>
        public virtual RectangleF AbsClientBounds
        {
            get
            {
                RectangleF rect = this.AbsBounds;
                RuntimeDocumentContentStyle style = this.RuntimeStyle;
                if (style != null)
                {
                    rect = style.GetClientRectangleF(rect);
                }
                return rect;
            }
        }

        /// <summary>
        /// 对象所属段落符号元素
        /// </summary>
        public virtual DomParagraphFlagElement OwnerParagraphEOF
        {
            get
            {
                return this.OwnerDocument?.GetParagraphEOFElement(this);
            }
        }

        /// <summary>
		/// 沿着DOM树逐级向上查找指定类型的父容器
		/// </summary>
		/// <param name="ParentType">父容器类型</param>
        /// <param name="includeThis">查找时是否包含元素本身</param>
		/// <returns>找到的父容器对象</returns>
        public virtual DomElement GetOwnerParent(Type ParentType, bool includeThis)
        {
            DomElement c = includeThis ? this : this.Parent;
            while (c != null)
            {
                Type ct = c.GetType();
                if (ct.Equals(ParentType) || ct.IsSubclassOf(ParentType))
                {
                    return c;
                }
                c = c.Parent;
            }
            return null;
        }

        /// <summary>
        /// 获得同一层次中的上一个元素
        /// </summary>
        public virtual DomElement PreviousElement
        {
            get
            {
                DomContainerElement c = this.Parent;
                if (c != null)
                {
                    int index = c.Elements.FastIndexOf(this);
                    if (index > 0)
                    {
                        return c.Elements[index - 1];
                    }
                }
                return null;
            }
        }
        /// <summary>
        /// 获得元素的同一层次的下一个元素
        /// </summary>
        public virtual DomElement NextElement
        {
            get
            {
                DomContainerElement c = this.Parent;
                if (c != null)
                {
                    int index = c.Elements.FastIndexOf(this);
                    if (index < c.Elements.Count - 1)
                    {
                        return c.Elements[index + 1];
                    }
                }
                return null;
            }
        }
        /// <summary>
        /// 元素内容版本号，当元素内容发生任何改变时，该属性值都会改变
        /// </summary>
        public virtual int ContentVersion
        {
            //[JSInvokable]
            get
            {
                return 0;
            }
        }

        /// <summary>
        /// 修改元素内容版本号，这会导致所有父级元素的ContentVersion的值的改变。
        /// </summary>
        public virtual void UpdateContentVersion()
        {
            DomElement parent = this;
            while (parent != null)
            {
                if (parent is DomContainerElement)
                {
                    ((DomContainerElement)parent)._ContentVersion++;
                }
                else if (parent is DomObjectElement)
                {
                    ((DomObjectElement)parent)._ContentVersion++;
                }
                parent = parent.Parent;
            }
        }

        /// <summary>
        /// 单独的更新元素状态
        /// </summary>
        /// <returns></returns>
        public virtual void EditorRefreshView()
        {
            EditorRefreshViewExt(false);
        }

        /// <summary>
        /// 单独的更新元素状态
        /// </summary>
        /// <param name="fastMode">快速模式</param>
        /// <returns></returns>
        public virtual void EditorRefreshViewExt(bool fastMode)
        {
            var document = this.OwnerDocument;
            if (document == null)
            {
                throw new Exception(DCSR.OwnerDocumentNUll);
            }
            this.SizeInvalid = true;
            this.FixDomState();
            if (this is DomContainerElement)
            {
                ((DomContainerElement)this).UpdateElementsRuntimeVisible(true);
            }
            else
            {
                this._RuntimeVisible = this.Visible && document.IsVisible(this);
            }
            if (document.ReadyState != DomReadyStates.Complete)
            {
                // 文档尚未完全加载，不执行本功能。
                return;
            }
            if (this.ContentElement == null)
            {
                return;
            }

            if (fastMode == false)
            {
                ElementLoadEventArgs args3 = new ElementLoadEventArgs(this, null);
                this.AfterLoad(args3);
            }
            var back5 = document.FixLayoutForPrint;
            document.FixLayoutForPrint = fastMode;
            var dce = this.DocumentContentElement;
            object selectionState = dce?.Selection.SaveState();

            this.CommitStyle(false);
            //this.OwnerDocument.ContentStyles.ResetFastValue();
            this.ContentElement.UpdateContentElements(true);
            this.InvalidateView();
            document.HighlightManager?.InvalidateHighlightInfo(this);
            // 重新计算大小
            using (DCGraphics g = this.InnerCreateDCGraphics())
            {
                InnerDocumentPaintEventArgs args = document.CreateInnerPaintEventArgs(g);
                args.ActiveMode = false;
                args.Element = this;
                if (fastMode)
                {
                    args.CheckSizeInvalidateWhenRefreshSize = true;
                }
                this.RefreshSize(args);
            }
            var back44 = WriterControl.RefreshingDocumentView;
            WriterControl.RefreshingDocumentView = true;
            try
            {
                DomContentElement ce = this.ContentElement;
                if (this is DomContentElement
                    && (this is DomDocumentContentElement) == false
                    && (ce is DomDocumentContentElement) == false)
                {
                    ce = ce.Parent.ContentElement;
                }
                ce.UpdateContentElements(true);
                //XTextElementList elements = new XTextElementList();
                if (this is DomTableCellElement)
                {
                    EditorRefreshView_Enum(((DomTableCellElement)this).OwnerTable, fastMode == false);
                }
                else if (this is DomTableRowElement)
                {
                    EditorRefreshView_Enum(this.Parent, fastMode == false);
                }
                else
                {
                    EditorRefreshView_Enum(this, fastMode == false);
                }
                dce.UpdateElementBorderInfos(false);
                if (selectionState == null)
                {
                    dce.Selection.UpdateState();
                }
                else
                {
                    dce.Selection.RestoreState(selectionState);
                }
                this.UpdateView(false);
                document.FixLayoutForPrint = back5;
            }
            finally
            {
                WriterControl.RefreshingDocumentView = back44;
            }
        }



        private static DomContentElement.UpdateContentElementsArgs _CachedArgs_EditorRefreshView_Enum = null;
        /// <summary>
        /// 为刷新视图而进行遍历
        /// </summary>
        /// <param name="eventSender"></param>
        /// <param name="args3"></param>
        protected static void EditorRefreshView_Enum(DomElement newElement, bool resetSizeInvalidate)
        {
            if (newElement.RuntimeVisible == false)
            {
                return;
            }
            if (resetSizeInvalidate)
            {
                newElement.SizeInvalid = true;
            }
            if (newElement is DomTableElement table)
            {
                // 表格内容进行排版
                DomContainerElement.UpdateElementsRuntimeVisibleArgs._Template = new DomContainerElement.UpdateElementsRuntimeVisibleArgs(newElement.OwnerDocument, true);
                _CachedArgs_EditorRefreshView_Enum = new DomContentElement.UpdateContentElementsArgs();
                _CachedArgs_EditorRefreshView_Enum.UpdateParentContentElement = false;
                _CachedArgs_EditorRefreshView_Enum.FastMode = false;
                _CachedArgs_EditorRefreshView_Enum.UpdateElementsVisible = true;

                table.EnumerateCells(delegate (DomTableCellElement cell2)
                {
                    cell2.FixElements();
                    cell2.Width = 0;
                    cell2.Height = 0;
                    cell2.UpdateContentElements(_CachedArgs_EditorRefreshView_Enum);
                });
                _CachedArgs_EditorRefreshView_Enum = null;
                DomContainerElement.UpdateElementsRuntimeVisibleArgs._Template = null;
                table.ExecuteLayout(false);
                table.EnumerateCells(delegate (DomTableCellElement cell2)
                {

                    if (cell2.RuntimeVisible)
                    {
                        var es2 = cell2.GetCompressedElements();
                        if (es2 != null)
                        {
                            foreach (var item in es2)
                            {
                                EditorRefreshView_Enum(item, resetSizeInvalidate);
                            }
                        }
                    }
                });
                //table.InnerExecuteLayout();
            }
            else if (newElement is DomDocumentContentElement dce)
            {
                // 文档节内容进行排版
                dce.CommitStyle(false);
                dce.UpdateContentElements(false);
                if (resetSizeInvalidate)
                {
                    dce.SizeInvalid = true;
                }
                dce.InnerExecuteLayout();
                var es = dce.GetCompressedElements();
                if (es != null)
                {
                    foreach (var item2 in es)
                    {
                        EditorRefreshView_Enum(item2, resetSizeInvalidate);
                    }
                }
                //args3.CancelChild = true;
            }
        }

        /// <summary>
        /// 设置元素大小
        /// </summary>
        /// <param name="width">新的元素宽度</param>
        /// <param name="height">新的元素高度</param>
        /// <param name="updateView">操作是否更新视图</param>
        /// <param name="logUndo">是否记录撤销操作信息</param>
        /// <returns>操作是否成功</returns>
        public virtual bool EditorSetSize(
            float width,
            float height,
            bool updateView,
            bool logUndo)
        {
            if (width <= 0)
            {
                throw new ArgumentOutOfRangeException("width:" + width);
            }
            if (height <= 0)
            {
                throw new ArgumentOutOfRangeException("height:" + height);
            }
            if (this.Width == width && this.Height == height)
            {
                return false;
            }
            SizeF oldSize = new SizeF(this.Width, this.Height);
            if (updateView)
            {
                this.InvalidateView();
            }
            this.Width = width;
            this.Height = height;
            this.SizeInvalid = false;

            if (logUndo)
            {
                if (this.OwnerDocument.BeginLogUndo())
                {
                    var undo = new DCSoft.Writer.Dom.Undo.XTextUndoProperty();
                    undo.Style = DCSoft.Writer.Dom.Undo.XTextUndoStyles.Size;
                    undo.Element = this;
                    undo.OldValue = oldSize;
                    undo.NewValue = new SizeF(this.Width, this.Height);
                    //undo.Document = this.OwnerDocument;
                    this.OwnerDocument.AddUndo(undo);
                    this.OwnerDocument.EndLogUndo();
                }
            }
            if (updateView)
            {

                this.UpdateView(true);
            }
            return true;
        }

        /// <summary>
        /// 更新视图
        /// </summary>
        /// <param name="changeDocument">是否修改了文档内容</param>
        protected void UpdateView(bool changeDocument)
        {
            // 更新视图
            if (this.OwnerLine != null)
            {
                this.OwnerLine.InvalidateState = true;
            }
            var back55 = DomContentElement._UseLineInfo_UpdateLinePosition;
            DomContentElement._UseLineInfo_UpdateLinePosition = false;
            try
            {
                if (this is DomTableCellElement)
                {
                    ((DomTableCellElement)this).LayoutInvalidate = true;
                    DomContentElement ce = this.Parent.ContentElement;
                    this.InvalidateView();
                    int index = ce.PrivateContent.IndexOf(this.Parent?.Parent);
                    ce.RefreshPrivateContent(index);
                }
                else
                {
                    if (this is DomTableElement)
                    {
                        ((DomTableElement)this).LayoutInvalidate = true;
                    }
                    DomContentElement ce = this.ContentElement;
                    this.InvalidateView();
                    int index = ce.PrivateContent.IndexOf(this);
                    if (index < 0)
                    {
                        index = ce.PrivateContent.IndexOf(this.FirstContentElement);
                    }
                    if (index < 0)
                    {
                        index = DomContentElement.FlagNotCheckStopLine;
                    }
                    if (this is DomContainerElement)
                    {
                        var sl = this.FirstContentElement?.OwnerLine;
                        var el = this.LastContentElement?.OwnerLine;
                        if (sl != null && el != null)
                        {
                            var privateLines = ce.PrivateLines;
                            var sli = privateLines.FastIndexOf(sl);
                            var eli = privateLines.FastIndexOf(el);
                            if (sli >= 0 && eli >= sli)
                            {
                                for (var iCount = sli; iCount <= eli; iCount++)
                                {
                                    privateLines[iCount].InvalidateState = true;
                                }
                            }
                            else
                            {
                                if (sli >= 0)
                                {
                                    privateLines[sli].InvalidateState = true;
                                }
                                if (eli >= 0)
                                {
                                    privateLines[eli].InvalidateState = true;
                                }
                            }
                        }
                    }
                    ce.RefreshPrivateContent(index);
                }
            }
            finally
            {
                DomContentElement._UseLineInfo_UpdateLinePosition = back55;
            }
            this.OwnerDocument.States.Layouted = true;
            //this.ContentElement.RefreshPrivateContent( index );
            this.InvalidateView();
            if (changeDocument)
            {
                this.OwnerDocument.Modified = true;
                // 触发文档内容修改事件
                ContentChangedEventArgs args2 = new ContentChangedEventArgs();
                args2.EventSource = ContentChangedEventSource.UndoRedo;
                args2.Document = this.OwnerDocument;
                DomContainerElement c = this as DomContainerElement;
                if (c == null)
                {
                    c = this.Parent;
                }
                args2.Element = c;
                c.RaiseBubbleOnContentChanged(args2);
                this.OwnerDocument.OnDocumentContentChanged();
            }
        }
        /// <summary>
        /// 处理文档用户界面事件
        /// </summary>
        /// <param name="args"></param>
        public virtual void HandleDocumentEvent(DocumentEventArgs args)
        {
            DCSoft.Writer.Dom.HandleDocumentEventHelper.HandleDocumentEvent(this, args);
        }

        /// <summary>
        /// 触发获得输入焦点事件
        /// </summary>
        /// <param name="args">参数</param>

        public virtual void OnGotFocus(ElementEventArgs args)
        {
            this.WriterControl?.OnEventElementGotFocus(args);
        }

        /// <summary>
        /// 触发失去输入焦点事件
        /// </summary>
        /// <param name="args">参数</param>

        public virtual void OnLostFocus(ElementEventArgs args)
        {
            this.WriterControl?.OnEventElementLostFocus(args);
        }

        /// <summary>
        /// 判断元素是否获得输入焦点
        /// </summary>
        public virtual bool Focused
        {
            get
            {
                if (this._Parent == null)
                {
                    return false;
                }
                DomDocumentContentElement dce = this.DocumentContentElement;
                if (dce == null)
                {
                    return false;
                }
                return dce.CurrentElement == this;
            }
        }

        /// <summary>
        /// 获得输入焦点
        /// </summary>
        public virtual void Focus()
        {
            if (this.BelongToDocumentDom(this.OwnerDocument) == false)
            {
                return;
            }
            CheckDocumentContentElementFocused();
            this.WriterControl?.Focus();
            DomElement firstElement = this.FirstContentElement;
            if (firstElement != null)
            {
                DomDocumentContentElement dce = this.DocumentContentElement;
                if (dce == null)
                {
                    return;
                }
                if (this.OwnerDocument.CurrentContentElement != dce)
                {
                    dce.Focus();
                }
                int index = -1;
                DomElement element = firstElement;
                while (element != null)
                {
                    index = dce.Content.FastIndexOf(element);
                    if (index >= 0)
                    {
                        break;
                    }
                    element = element.Parent;
                }
                if (index >= 0)
                {
                    dce.SetSelection(index, 0);
                    if (dce.Content.FastIndexOf(element) != index)
                    {
                        // 元素序号发生改变，再次设置焦点
                        dce.SetSelection(dce.Content.FastIndexOf(element), 0);
                    }
                }
            }
        }

        internal bool EnsureInPrivateContent(bool updateContent, bool updateView)
        {
            if (this.WriterControl != null)
            {
                // 用户界面被锁定了，不执行操作。
                return false;
            }
            DomDocumentContentElement dce = this.DocumentContentElement;
            DomContentElement ce = this.ContentElement;
            if (dce == null || ce == null)
            {
                return false;
            }
            int index1 = dce.Content.IndexOfUseContentIndex(this);
            int index2 = ce.PrivateContent.IndexOfUsePrivateContentIndex(this);
            if (index1 >= 0 && index2 < 0)// dce.Content.Contains(this) && ce.PrivateContent.Contains(this) == false)
            {
                // 在全局内容列表中存在但在小容器私有列表中不存在
                DomElement parent = this.Parent;
                while (parent != null)
                {
                    if (parent is DomInputFieldElementBase)
                    {
                        DomInputFieldElementBase field = (DomInputFieldElementBase)parent;
                    }
                    parent = parent.Parent;
                }//while
            }
            return false;
        }

        /// <summary>
        /// 处理鼠标进入事件
        /// </summary>
        /// <param name="args">事件参数</param>
        public virtual void OnMouseEnter(ElementEventArgs args)
        {
        }

        /// <summary>
        /// 处理鼠标离开事件
        /// </summary>
        /// <param name="args">事件参数</param>

        public virtual void OnMouseLeave(ElementEventArgs args)
        {
        }


        /// <summary>
        /// 处理鼠标按键按下事件
        /// </summary>
        /// <param name="args">参数</param>
        public virtual void OnMouseDown(ElementMouseEventArgs args)
        {
            this.WriterControl?.OnEventElementMouseDown(args);
        }

        /// <summary>
        /// 处理鼠标移动事件
        /// </summary>
        /// <param name="args">参数</param>
        public virtual void OnMouseMove(ElementMouseEventArgs args)
        {
            this.WriterControl?.OnEventElementMouseMove(args);
        }

        /// <summary>
        /// 处理鼠标按键松开事件
        /// </summary>
        /// <param name="args">参数</param>
        public virtual void OnMouseUp(ElementMouseEventArgs args)
        {
            this.WriterControl?.OnEventElementMouseUp(args);
        }

        /// <summary>
        /// 处理鼠标单击事件
        /// </summary>
        /// <param name="args">参数</param>
        public virtual void OnMouseClick(ElementMouseEventArgs args)
        {
            this.WriterControl?.OnEventElementMouseClick(args);
        }

        /// <summary>
        /// 处理鼠标双击事件
        /// </summary>
        /// <param name="args">参数</param>
        public virtual void OnMouseDblClick(ElementMouseEventArgs args)
        {
            if (args == null)
            {
                return;
            }
            if (this.OwnerDocument == null)
            {
                args.CancelBubble = true;
                return;
            }
            this.WriterControl?.OnEventElementMouseDblClick(args);
        }

        /// <summary>
        /// 处理键盘按键按下事件
        /// </summary>
        /// <param name="args">参数</param>
        public virtual void OnKeyDown(ElementKeyEventArgs args)
        {
        }

        /// <summary>
        /// 处理键盘按键按下事件
        /// </summary>
        /// <param name="args">参数</param>
        public virtual void OnKeyPress(ElementKeyEventArgs args)
        {
        }
        /// <summary>
        /// 处理键盘按键松开事件
        /// </summary>
        /// <param name="args">参数</param>

        public virtual void OnKeyUp(ElementKeyEventArgs args)
        {
        }

        /// <summary>
        /// 复制对象
        /// </summary>
        /// <param name="Deeply">是否深入复制子元素</param>
        /// <returns>复制品</returns>
        public virtual DomElement Clone(bool Deeply)
        {
            DomElement element = (DomElement)this.MemberwiseClone();
            //element._ElementInstanceIndex = _ElementInstanceIndexCount++;
            element._Style = null;
            //element._OwnerDocument = null;
            element._OwnerLine = null;
            element._Parent = null;
            element.SizeInvalid = true;
            element._ContentIndex = -1;
            element.ViewInvalid = true;
            return element;
        }

        /// <summary>
        /// 输出元素包含的文本数据
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Empty;
        }

        /// <summary>
        /// 输出纯文本数据
        /// </summary>
        /// <returns>纯文本数据</returns>
        public virtual string ToPlaintString()
        {
            return this.ToString();
        }
#if !RELEASE
        /// <summary>
        /// 输出调试用的文本数据
        /// </summary>
        /// <returns>文本数据</returns>
        public virtual string ToDebugString()
        {
            return "[" + this._ContentIndex + "]" + this.GetType().Name + "(" + this.ID + ")";
        }
#endif

        /// <summary>
        /// 绘制文档元素对象
        /// </summary>
        /// <param name="args">事件参数</param>
        public virtual void Draw(InnerDocumentPaintEventArgs args)
        {
            DrawContent(args);
        }


        /// <summary>
        /// 绘制文档元素内容
        /// </summary>
        /// <param name="args">事件参数</param>
        public virtual void DrawContent(InnerDocumentPaintEventArgs args)
        {
        }

        /// <summary>
        /// 计算元素大小
        /// </summary>
        /// <param name="args">参数</param>
        public virtual void RefreshSize(InnerDocumentPaintEventArgs args)
        {
        }

        /// <summary>
        /// 高亮度信息对象列表
        /// </summary>
        /// <returns>获得的列表</returns>

        public virtual HighlightInfoList GetHighlightInfos()
        {
            return null;
        }

        /// <summary>
        /// 准备执行序列化操作
        /// </summary>
        public virtual void PrepareSerialize(string format)
        {
        }


        /// <summary>
        /// 文档加载后的处理
        /// </summary>
        /// <param name="args">事件参数</param>
        public virtual void AfterLoad(ElementLoadEventArgs args)
        {
            this.CommitStyleFast();
        }
        /// <summary>
        /// 销毁对象
        /// </summary>
        public virtual void Dispose()
        {
            this._OwnerDocument = null;
            this._OwnerLine = null;
            this._Parent = null;
            this._RuntimeStyle = null;
            this._Style = null;
        }

        /// <summary>
        /// 表示对象内容的纯文本数据
        /// </summary>
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public virtual string Text
        {
            //[JSInvokable]
            get
            {
                return ToString();
            }
            //[JSInvokable]
            set
            {
            }
        }
        /// <summary>
        /// 输出文本的方法参数
        /// </summary>
        public sealed class WriteTextArgs
        {
            /// <summary>
            /// 初始化对象
            /// </summary>
            /// <param name="doc">文档对象</param>
            /// <param name="vIncludeOuterText">是否输出周边的标记文本</param>
            /// <param name="vIncludeHiddenText">是否输出隐藏的文本</param>
            public WriteTextArgs(DomDocument doc, bool vIncludeOuterText = false, bool vIncludeHiddenText = false, int strLen = 0)
            {
                this.IncludeOuterText = vIncludeOuterText;
                this.IncludeHiddenText = vIncludeHiddenText;
                if (strLen > 0)
                {
                    this._Str = new StringBuilder(strLen);
                }
                else
                {
                    this._Str = new StringBuilder();
                }
            }

            private StringBuilder _Str;
            /// <summary>
            /// 是否输出输入域的背景文本
            /// </summary>
            public bool IncludeBackgroundText = true;
            /// <summary>
            /// 是否输出周边的标记文本
            /// </summary>
            public bool IncludeOuterText = false;
            /// <summary>
            /// 是否输出隐藏的文本
            /// </summary>
            public bool IncludeHiddenText = false;
            /// <summary>
            /// 文本长度
            /// </summary>
            public int Length
            {
                get
                {
                    return this._Str.Length;
                }

            }
            /// <summary>
            /// 添加文本内容
            /// </summary>
            /// <param name="txt">文本内容</param>
            public void Append(string txt)
            {
                if (txt != null && txt.Length > 0)
                {
                    this._Str.Append(txt);
                }
            }
            /// <summary>
            /// 添加文本内容
            /// </summary>
            /// <param name="txt">文本内容</param>
            public void Append(char txt)
            {
                this._Str.Append(txt);
            }
            /// <summary>
            /// 添加新行
            /// </summary>
            public void AppendLine()
            {
                this._Str.AppendLine();
            }
            /// <summary>
            /// 获得输出的文本内容
            /// </summary>
            /// <returns>文本内容</returns>
            public override string ToString()
            {
                return this._Str.ToString();
            }
        }

        /// <summary>
        /// 输出纯文本内容
        /// </summary>
        /// <param name="args">参数</param>
        public virtual void WriteText(WriteTextArgs args)
        {
            args.Append(this.Text);
        }
        /// <summary>
        /// 获得文档元素的文本内容
        /// </summary>
        /// <param name="args">参数</param>
        /// <returns>文本值</returns>
        public string GetText(GetTextArgs args)
        {
            if (args == null)
            {
                throw new ArgumentNullException("args");
            }
            args.Init(this.OwnerDocument);
            InnerGetText(args);
            return args.ToString();
        }
        /// <summary>
        /// 获得文档元素的文本内容
        /// </summary>
        /// <param name="args">参数</param>
        public virtual void InnerGetText(GetTextArgs args)
        {
            args.Append(this.Text);
        }
        /// <summary>
        /// 表达式数值内容
        /// </summary>
        public virtual string FormulaValue
        {
            get
            {
                return this.Text;
            }
            set
            {
                this.Text = this.FixInputFormulaValue(value);
            }
        }

        /// <summary>
        /// 修复输入的表达式的值
        /// </summary>
        /// <param name="txt"></param>
        /// <returns></returns>
        protected string FixInputFormulaValue(string txt)
        {
            if (txt != null)
            {
                txt = txt.Replace("\\r\\n", Environment.NewLine);
            }
            return txt;
        }
    }//public abstract class XTextElement
}