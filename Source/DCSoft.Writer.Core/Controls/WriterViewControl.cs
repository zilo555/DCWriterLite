using DCSoft.WinForms;
using DCSoft.Printing;
using DCSoft.Writer.Dom;
using DCSoft.Drawing;
using System.Windows.Forms;
// // 
using System.ComponentModel;
//using DCSoft.Writer.Printing;
using DCSoft.Writer.Data;
using System.Runtime.InteropServices;
namespace DCSoft.Writer.Controls
{
    /// <summary>
    /// 文本文档编辑控件
    /// </summary>
    /// <remarks>编制 袁永福</remarks>
    public sealed partial class WriterViewControl : TextPageViewControl
    {
        private static int _InstanceCount = 0;
        /// <summary>
        /// 初始化对象
        /// </summary>
        public WriterViewControl(WriterControl ctl)
        {
            if (ctl == null)
            {
                throw new ArgumentNullException("ctl");
            }
            this._OwnerWriterControl = ctl;

            _InstanceCount++;

            if (this._DocumentOptions != null)
            {
                this._DocumentOptions.SetWriterControl(this);
            }
        }

        private WriterControl _OwnerWriterControl = null;
        /// <summary>
        /// 控件所在的编辑器控件对象
        /// </summary>
        internal WriterControl OwnerWriterControl
        {
            get
            {
                return this._OwnerWriterControl;
            }
        }

        /// <summary>
        /// 自动设置文档的默认字体
        /// </summary>
        /// <remarks>若该属性值为true，则编辑器会自动将控件的字体和前景色设置
        /// 到文档的默认字体和文本颜色。修改本属性值不会立即更新文档视图，
        /// 此时需要调用“UpdateDefaultFont( true )”来更新文档视图。</remarks>
        internal bool AutoSetDocumentDefaultFont = false;

        /// <summary>
        /// 更新文档的默认字体
        /// </summary>
        /// <param name="updateUI">是否更新用户界面</param>
        public ContentEffects UpdateDefaultFont(bool updateUI)
        {
            if (this.AutoSetDocumentDefaultFont)
            {
                return EditorSetDefaultFont(
                    new XFontValue(this.Font),
                    this.ForeColor,
                    updateUI);
            }
            return ContentEffects.None;
        }

        /// <summary>
        /// 编辑器调用的设置文档的默认字体和颜色
        /// </summary>
        /// <param name="font">默认字体</param>
        /// <param name="color">默认文本颜色</param>
        /// <param name="updateUI">是否更新用户界面</param>
        /// <returns>本次操作对文档的影响</returns>
        public ContentEffects EditorSetDefaultFont(XFontValue font, Color color, bool updateUI)
        {
            if (this._Document != null)
            {
                // 设置文档的默认字体
                ContentEffects effects = this._Document.SetDefaultFont(
                    font,
                    color,
                    true);
                if (updateUI && this.IsHandleCreated)
                {
                    if (effects == ContentEffects.Display)
                    {
                        this.Invalidate();
                    }
                    else if (effects == ContentEffects.Layout)
                    {
                        this.RefreshDocument();
                    }
                    return effects;
                }
            }
            return ContentEffects.None;
        }

        /// <summary>
        /// 移动焦点使用的快捷键
        /// </summary>
        public MoveFocusHotKeys MoveFocusHotKey
        {
            get
            {
                return this.DocumentOptions.BehaviorOptions.MoveFocusHotKey;
            }
            set
            {
                this.DocumentOptions.BehaviorOptions.MoveFocusHotKey = value;
            }
        }

        private bool _Readonly = false;
        /// <summary>
        /// 文档内容只读标记
        /// </summary>
        public bool Readonly
        {
            get
            {
                return _Readonly;
            }
            set
            {
                _Readonly = value;
            }
        }

        private DocumentOptions _DocumentOptions = null;
        /// <summary>
        /// 文档设置
        /// </summary>
        public DocumentOptions DocumentOptions
        {
            get
            {
                if (this._DisposingControl)
                {
                    return this._DocumentOptions;
                }
                //if (_DocumentOptions == null && this.Document != null)
                //{
                //    return this.Document.Options;
                //}
                if (_DocumentOptions == null)
                {
                    _DocumentOptions = new DocumentOptions();
                    _DocumentOptions.SetWriterControl(this);
                }
                return _DocumentOptions;
            }
            set
            {
                _DocumentOptions = value;
                if (_Document != null)
                {
                    _Document.Options = _DocumentOptions;
                    _DocumentOptions.SetWriterControl(this);
                }
            }
        }


        private DocumentControler _DocumentControler = null;// WriterToolsBase.Instance.CreateDocumentControler();// new DocumentControler();
        /// <summary>
        /// 文档控制器
        /// </summary>
        public DocumentControler DocumentControler
        {
            get
            {
                if (this._DisposingControl)
                {
                    return this._DocumentControler;
                }
                if (_DocumentControler == null)
                {
                    this._DocumentControler = new DocumentControler();
                }
                if (_DocumentControler == null)
                {
                    return null;
                }
                if (_DocumentControler.EditorControl != this.OwnerWriterControl)
                {
                    _DocumentControler.EditorControl = this.OwnerWriterControl;
                }
                if (_DocumentControler.Document != this._Document)
                {
                    _DocumentControler.Document = this._Document;
                }
                return _DocumentControler;
            }
            set
            {
                _DocumentControler = value;
                if (_DocumentControler != null)
                {
                    _DocumentControler.Document = this._Document;
                    _DocumentControler.EditorControl = this.OwnerWriterControl;
                }
            }
        }

        public WriterAppHost AppHost
        {
            get
            {
                if (this.OwnerWriterControl == null)
                {
                    return null;
                }
                return this.OwnerWriterControl.AppHost;
            }
        }


        /// <summary>
        /// 将插入点移动到指定位置
        /// </summary>
        /// <param name="target">移动的目标</param>
        public void MoveTo(MoveTarget target)
        {
            if (this._Document != null)
            {
                this._Document.Content.AutoClearSelection = true;
                this._Document.Content.MoveToTarget(target);
            }
        }

        /// <summary>
        /// 移动光标到指定位置处
        /// </summary>
        /// <param name="index">位置序号</param>
        public void MoveToPosition(int index)
        {
            if (this._Document != null)
            {
                this._Document.Content.AutoClearSelection = true;
                this._Document.Content.MoveToPosition(index);
            }
        }


        #region 格式刷相关代码

        /// <summary>
        /// 为格式刷而准备的文档格式信息对象,DCWriter内部使用。
        /// </summary>
        public CurrentContentStyleInfo StyleInfoForFormatBrush = null;

        /// <summary>
        /// 取消格式刷操作,DCWriter内部使用。
        /// </summary>
        public void CancelFormatBrush()
        {
            this.StyleInfoForFormatBrush = null;
            this.Cursor = Cursors.IBeam;
            this.InvalidateCommandState(StandardCommandNames.FormatBrush);
        }

        /// <summary>
        /// 执行格式刷完成功能
        /// </summary>
        internal void ApplyFormatBrush()
        {
            CurrentContentStyleInfo info = this.StyleInfoForFormatBrush;
            this.StyleInfoForFormatBrush = null;
            var document = this.Document;
            DomElementList list = ContentFormatUtils.GetFormatHandleElements(
                document,
                true,
                true,
                true,
                document.DocumentControler);
            if (list == null || list.Count == 0)
            {
                return;
            }
            document.BeginLogUndo();
            bool result = DCSelection.SetElementStyle(
                info.Content,
                info.Paragraph,
                info.Cell,
                document,
                list,
                true,
                StandardCommandNames.FormatBrush,
                true);
            document.EndLogUndo();
            if (result)
            {
                document.OnSelectionChanged();
                document.OnDocumentContentChanged();
                this.InvalidateCommandState();
            }
        }

        #endregion



        /// <summary>
        /// 创建视图用的画布对象
        /// </summary>
        /// <returns>创建的画布对象</returns>
        public DCGraphics CreateDCViewGraphics()
        {
            Graphics g = base.CreateViewGraphics();
            DCGraphics g2 = new DCGraphics(g);
            g2.AutoDisposeNativeGraphics = true;
            return g2;
        }

        /// <summary>
        /// 刷新文档
        /// </summary>
        public void RefreshDocument()
        {
            RefreshDocumentExt(true, true);
        }


        /// <summary>
        /// 刷新文档
        /// </summary>
        /// <param name="refreshSize">是否重新计算元素大小</param>
        /// <param name="executeLayout">是否进行文档内容重新排版</param>
        public void RefreshDocumentExt(bool refreshSize, bool executeLayout)
        {
            if (this._EditorHost != null)
            {
                this._EditorHost.ResetState();
            }
            this._DragableElement = null;
            this.StyleInfoForFormatBrush = null;
            var document = this.Document;
            document.InvalidateFixDomState();
            document.States.RenderMode = DocumentRenderMode.Paint;
            document.InnerRefreshDocumentExt(refreshSize, executeLayout, this);

        }

        /// <summary>
        /// 更新文档页状态
        /// </summary>
        public override void UpdatePages(int preservePixcelWidth = 0)
        {
            if (this._Document != null)
            {
                //float tick = CountDown.GetTickCountFloat();
                //this.Pages.Clear();
                //this.Pages.AddRange(this.Document.Pages);
                this.Pages = this._Document.Pages;
                base.UpdatePages(preservePixcelWidth);

                PageContentPartyStyle style = PageContentPartyStyle.Body;
                if (this._Document.CurrentContentElement != null)
                {
                    style = this._Document.CurrentContentElement.PagePartyStyle;
                }
                foreach (SimpleRectangleTransform item in this.PagesTransform)
                {
                    item.Enable = (item.ContentStyle == style);
                }//foreach
                if (this.CurrentTransformItem != null)
                {
                    // 更新当前转换信息对象
                    bool match = false;
                    foreach (SimpleRectangleTransform item in this.PagesTransform)
                    {
                        if (item.DocumentObject == this.CurrentTransformItem.DocumentObject
                            && item.PageIndex == this.CurrentTransformItem.PageIndex
                            && item.ContentStyle == item.ContentStyle)
                        {
                            this.CurrentTransformItem = item;
                            match = true;
                            break;
                        }
                    }//foreach
                    if (match == false)
                    {
                        this.CurrentTransformItem = null;
                    }
                }
                foreach (SimpleRectangleTransform item in this.PagesTransform)
                {
                    if (DrawerUtil.IsHeaderFooter(item.ContentStyle))
                    {
                        item.Enable = item == this.CurrentTransformItem;
                    }
                }
                this.WASMCheckPageSizesChanged();
                //tick = CountDown.GetTickCountFloat() - tick;
            }
        }
        /// <summary>
        /// 视图坐标转换为控件客户区中的坐标
        /// </summary>
        /// <param name="x">X值</param>
        /// <param name="y">Y值</param>
        /// <returns>新坐标</returns>
        public override Point ViewPointToClient(int x, int y)
        {
            if (this.CurrentTransformItem == null)
            {
                return base.ViewPointToClient(x, y);
            }
            else
            {
                return this.CurrentTransformItem.UnTransformPoint(x, y);
            }
        }

        /// <summary>
        /// 将控件客户区中的坐标转换为视图坐标
        /// </summary>
        /// <param name="x">X值</param>
        /// <param name="y">Y值</param>
        /// <returns>新坐标</returns>
        public override Point ClientPointToView(int x, int y)
        {
            if (this.CurrentTransformItem == null)
            {
                return base.ClientPointToView(x, y);
            }
            else
            {
                return this.CurrentTransformItem.TransformPoint(x, y);
            }
        }


        private bool _HideCaretWhenHasSelection = true;
        /// <summary>
        /// 当选择了文档内容时隐藏光标
        /// </summary>
        public bool HideCaretWhenHasSelection
        {
            get
            {
                return _HideCaretWhenHasSelection;
            }
            set
            {
                _HideCaretWhenHasSelection = value;
            }
        }

        /// <summary>
        /// 根据指定的文档元素对象更新光标
        /// </summary>
        /// <param name="element">指定的文档元素对象</param>
        public void UpdateTextCaretByElement(DomElement element)
        {
            if (this._Document == null || element == null)
            {
                // 文档内容为空，无法更新光标。
                return;
            }
            bool bolReadonlyCaret = false;
            if (this.Readonly)
            {
                bolReadonlyCaret = true;
            }
            else
            {
                try
                {
                    bolReadonlyCaret = !this.DocumentControler.CanModify(element);
                }
                finally
                {
                }
            }
            if (element.OwnerLine != null)
            {
                float topFix = 3.125f;// element.OwnerLine.ContentTopFix;// XTextLine.StdContentTopFix;
                float lineBottom = element.OwnerLine.AbsTop;
                //if (element.Height > element.OwnerLine.Height )
                //{
                //    lineBottom = lineTop + element.OwnerLine.Height;
                //}
                DomDocumentContentElement ce = this._Document.CurrentContentElement;
                if (ce.Content.LineEndFlag && ce.Selection.Length == 0)
                {
                    DomElement e2 = ce.Content.GetPreLayoutElement(element);// ce.Content.GetPreElement(element);
                    if (e2 == null)
                    {
                        e2 = element;
                    }
                    //float curHeight = e2.Height;
                    float curTop = e2.GetAbsTop();
                    float curBottom = e2.GetAbsTop() + e2.Height + topFix;

                    if (e2.OwnerLine != null)
                    {
                        curTop = Math.Max(curTop, e2.OwnerLine.AbsTop);
                        curBottom = Math.Min(e2.OwnerLine.AbsTop + e2.OwnerLine.Height, curBottom);

                        //curHeight = Math.Min(curHeight, e2.OwnerLine.Height);
                    }
                    if (this.HideCaretWhenHasSelection && this.Selection.Length != 0)
                    {
                        base.HideOwnerCaret();
                    }
                    else
                    {
                        if (this.Focused || this.ForceShowCaret)
                        {
                            base.ShowCaret();
                        }
                        //float runtimeTop = Math.Max(lineTop, curTop + topFix);
                        base.MoveTextCaretTo(
                            (int)(e2.GetAbsLeft() + e2.Width - 1),
                            (int)(curBottom),
                            (int)(curBottom - curTop),
                            (int)e2.Width,
                            bolReadonlyCaret);
                    }
                }
                else
                {
                    int curLeftFix = 0;
                    DomElement e2 = element;
                    DomLine line = element.OwnerLine;
                    if (line != null)
                    {
                        e2 = line.LayoutElements.GetPreElement(element);
                        if (e2 == null)
                        {
                            e2 = element;
                        }
                        else if (e2 is DomTableElement)
                        {
                            e2 = element;
                            curLeftFix = 10;
                        }
                    }

                    if (this.HideCaretWhenHasSelection && this.Selection.Length != 0)
                    {
                    }
                    else
                    {
                        if (this.Focused || this.ForceShowCaret)
                        {
                            // 若控件获得焦点或者强制显示光标则显示光标
                            base.ShowCaret();
                        }
                        float curHeight = e2.Height;
                        float curTop = e2.GetAbsTop();
                        if (e2.OwnerLine != null)
                        {
                            curTop = Math.Max(curTop, e2.OwnerLine.AbsTop);
                            curHeight = Math.Min(curHeight, e2.OwnerLine.Height);
                        }
                        float cleft = element.GetAbsLeft();
                        base.MoveTextCaretTo(
                            (int)cleft + curLeftFix,
                            (int)Math.Max(lineBottom, curTop + curHeight + topFix),
                            (int)curHeight,
                            (int)element.Width,
                            bolReadonlyCaret);
                    }
                }
            }
        }

        /// <summary>
        /// 正在更新光标操作的标记，防止递归。
        /// </summary>
        private bool _UpdatingTextCaret = false;

        /// <summary>
        /// 根据当前元素更新光标
        /// </summary>
        public void UpdateTextCaret()
        {
            if (this._Document == null
                || this._Document.PageRefreshed == false
                || this._Document.Content == null
                || this._DocumentControler == null
                || this._Document.DocumentControler == null
                || this._UpdatingTextCaret == true)
            {
                // 状态不对
                return;
            }
            if (this._Document.States.Layouted == true)
            {
                try
                {
                    this._UpdatingTextCaret = true;
                }
                finally
                {
                    this._UpdatingTextCaret = false;
                }
                UpdateTextCaretByElement(this._Document.CurrentElement);
            }
        }

        /// <summary>
        /// 根据当前元素更新光标，而且不会造成用户视图区域的滚动
        /// </summary>
        public void UpdateTextCaretWithoutScroll()
        {
            if (this._Document == null)
            {
                return;
            }
            bool back = this.MoveCaretWithScroll;
            this.MoveCaretWithScroll = false;
            UpdateTextCaretByElement(this._Document.CurrentElement);
            this.MoveCaretWithScroll = back;
        }

        /// <summary>
        /// 文档中当前元素或被选择区域的开始位置在编辑器控件客户区中的坐标
        /// </summary>
        public Point SelectionStartPosition()
        {
            DomElement element = this.CurrentElement();
            if (element != null)
            {
                Rectangle bounds = this.GetElementClientBounds(element);
                return bounds.Location;
            }
            else
            {
                return Point.Empty;
            }
        }

        /// <summary>
        /// 选中文档所有内容
        /// </summary>
        public void SelectAll()
        {
            if (this._Document != null)
            {
                this._Document.Content.SelectAll();
                UpdateTextCaret();
            }
        }

        /// <summary>
        /// 删除选择区域
        /// </summary>
        public void DeleteSelection(bool showUI)
        {
            this.DocumentControler.Delete(showUI);
        }

        /// <summary>
        ///  表示当前插入点位置信息的字符串
        /// </summary>
        public string PositionInfoText()
        {

            DomLine line = this._Document?.CurrentContentElement?.CurrentLine;
            if (line != null && line.OwnerPage != null)
            {
                string txt =
                    string.Format(DCSR.LineInfo_PageIndex_LineIndex_ColumnIndex,
                    Convert.ToString(line.OwnerPage.PageIndex + 1),
                    Convert.ToString(this.CurrentLineIndexInPage()),
                    Convert.ToString(this.CurrentColumnIndex()));
                return txt;
            }
            return null;

        }

        /// <summary>
        /// 更新当前页
        /// </summary>
        /// <returns>操作是否修改了当前页对象</returns>
        public override bool UpdateCurrentPage()
        {
            if (this._Document == null)
            {
                return false;
            }
            DomLine line = this._Document.CurrentContentElement.CurrentLine;
            if (line != null)
            {
                PrintPage page = line.OwnerPage;
                //if (page != null)
                {
                    return this.SetCurrentPageRaw(page);
                }
            }
            return base.UpdateCurrentPage();
        }

        /// <summary>
        /// 执行文档元素默认方法
        /// </summary>
        /// <param name="element">文档元素对象</param>
        public void ExecuteElementDefaultMethod(DomElement element)
        {
            if (element == null)
            {
                element = this.CurrentElement();
            }
            DocumentEventArgs args = new DocumentEventArgs(
                this.Document,
                element,
                DocumentEventStyles.DefaultEditMethod);
            while (element != null)
            {
                args.Element = element;

                element.HandleDocumentEvent(args);
                if (args.Handled)
                {
                    break;
                }
                element = element.Parent;
            }
            //#if ! LightWeight
            //            if (args.Handled == false)
            //            {
            //                this.BeginInsertKB();
            //            }
            //#endif
        }

        /// <summary>
        /// 正在编辑文档元素数值
        /// </summary>
        public bool IsEditingElementValue
        {
            get
            {
                if (this.WASMParent == null)
                {
                    return false;
                }
                else
                {
                    return this.WASMParent.IsDropdownControlVisible();
                }
            }
        }

        /// <summary>
        /// 取消当前的编辑元素内容的操作
        /// </summary>
        /// <returns>操作是否成功</returns>
        public bool CancelEditElementValue()
        {
            this.WASMParent?.CloseDropdownControl();
            return false;
        }


        /// <summary>
        /// 开始执行编辑元素内容值的操作
        /// </summary>
        /// <param name="element">文档元素对象</param>
        /// <returns>操作是否成功</returns>
        public bool BeginEditElementValue(DomElement element)
        {
            return BeginEditElementValue(
                element,
                false,
                ValueEditorActiveMode.Program,
                false,
                true);
        }

        private bool _InnerEnableEditElementValue = true;
        /// <summary>
        /// 内部的是否运行启用文档元素数据编辑器
        /// </summary>
        internal bool InnerEnableEditElementValue
        {
            get
            {
                return _InnerEnableEditElementValue;
            }
            set
            {
                _InnerEnableEditElementValue = value;
            }
        }

        /// <summary>
        /// 开始执行编辑元素内容值的操作
        /// </summary>
        /// <param name="element">元素对象</param>
        /// <param name="detectOnly">本次调用只是检测当前元素的值能否编辑，但不执行编辑操作</param>
        /// <param name="sourceMode">发起编辑操作的来源</param>
        /// <param name="checkActiveMode">是否检测激活模式</param>
        /// <param name="checkRecursion">检查递归</param>
        /// <returns>操作是否成功</returns>
        internal bool BeginEditElementValue(
            DomElement element,
            bool detectOnly,
            ValueEditorActiveMode sourceMode,
            bool checkActiveMode,
            bool checkRecursion)
        {
            if (this.InnerEnableEditElementValue == false)
            {
                return false;
            }
            if (this.Readonly)
            {
                // 控件只读
                return false;
            }
            var bopts = this.DocumentOptions.BehaviorOptions;
            if (bopts.EnableEditElementValue == false)
            {
                if (bopts.SpecifyDebugMode)
                {
                    MessageBox.Show(this, "EnableEditElementValue=false");
                }
                return false;
            }
            if (this.EditorHost.EditingValue)
            {
                if (bopts.SpecifyDebugMode)
                {
                    MessageBox.Show(this, "EditorHost.EditingValue=true");
                }
                return false;
            }
            if (element == null)
            {
                return false;
                //throw new ArgumentNullException("element");
            }
            if (this.Capture)
            {
                if (bopts.SpecifyDebugMode)
                {
                    MessageBox.Show(this, "Capture=true");
                }
                this.Capture = false;
            }
            var document = this.Document;
            document.MouseCapture = null;
            ElementValueEditor editor = null;
            var srcElement = element;
            while (element != null)
            {
                {
                    editor = WriterToolsBase.Instance.CreateElementValueEditor(element);
                    //if ( editor != null
                    //    && editor.GetType().Name =="XTextInputFieldElementNumericValueEditor"
                    //    && this.DocumentOptions.BehaviorOptions.EnableCalculateControl == false)
                    //{
                    //    // 不允许使用计算器控件
                    //    editor = null;
                    //}
                    if (editor != null)
                    {
                        if (element is DomInputFieldElement)
                        {
                            DomInputFieldElement field = (DomInputFieldElement)element;
                            if (field.StartElement == srcElement)
                            {
                                var curElement = element.DocumentContentElement?.CurrentElement;
                                if (curElement == srcElement)
                                {
                                    element = element.Parent;
                                    continue;
                                }
                            }
                            if (checkActiveMode)
                            {
                                // 检查激活模式
                                if ((sourceMode & field.RuntimeEditorActiveMode()) == sourceMode)
                                {
                                    break;
                                }
                            }
                            else
                            {
                                break;
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                }
                element = element.Parent;
            }
            if (element == null)
            {
                return false;
            }
            DomAccessFlags flags2 = DomAccessFlags.Normal;
            flags2 = flags2 & ~DomAccessFlags.CheckUserEditable;

            if (this.DocumentControler.CanModifyContent(
                element, flags2) == false)
            {
                return false;
            }
            if (detectOnly)
            {
                // 仅仅是检测
                return editor != null
                    && this.DocumentControler.CanModify(
                        element,
                        DomAccessFlags.None);
            }
            if (editor != null)
            {
                if (this.DocumentControler.CanModify(
                    element,
                    DomAccessFlags.None) == false)
                {
                    // 元素只读
                    return false;
                }
                bool setSelection = true;
                if (this.CurrentElement() != null)
                {
                    DomElementList ps = WriterUtilsInner.GetParentList(this.CurrentElement());
                    if (ps.Contains(element))
                    {
                        setSelection = false;
                    }
                }
                if (setSelection)
                {
                    document.Content.AutoClearSelection = true;
                    document.Content.SetSelection(
                        element.FirstContentElementInPublicContent.ContentIndex,
                        0);
                }
                if (this.EditorHost.CurrentEditContext != null)
                {
                    this.EditorHost.CancelEditValue();
                }
                this.EditorHost.KeepWriterControlFocused = false;
                ElementValueEditResult result = this.EditorHost.EditValue(
                    element,
                    editor);
                return true;
            }
            return false;
        }

        internal SimpleRectangleTransform GetTransformItemByDescPoint(float x, float y)
        {
            if (this.CurrentTransformItem != null
                && this.CurrentTransformItem.DescRectF.Contains(x, y))
            {
                return this.CurrentTransformItem;
            }
            return this.PagesTransform.GetByDescPoint(x, y);
        }


        private DomElement _DragableElement = null;
        /// <summary>
        /// 可以被拖拽的文档元素
        /// </summary>
        public DomElement DragableElement
        {
            get
            {
                return _DragableElement;
            }
            set
            {
                if (_DragableElement != value)
                {
                    if (_DragableElement != null && this.IsHandleCreated)
                    {
                        Rectangle deb = this.DragableHandleBounds;
                        if (deb.IsEmpty == false)
                        {
                            this.Invalidate(deb);
                        }
                    }
                    _DragableElement = value;
                    if (_DragableElement != null && this.IsHandleCreated)
                    {
                        Rectangle deb = this.DragableHandleBounds;
                        if (deb.IsEmpty == false)
                        {
                            this.Invalidate(deb);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 拖拽句柄边界
        /// </summary>
        public Rectangle DragableHandleBounds
        {
            get
            {
                return Rectangle.Empty;

                //if (_DragableElement != null
                //    && _DragableElement.DocumentContentElement == this.Document.CurrentContentElement )
                //{
                //    Rectangle rect = this.GetElementClientBounds(_DragableElement);
                //    Image img = this.DomImageList.GetBitmap(DCStdImageKey.DragHandle);
                //    Rectangle rect2 = new Rectangle(
                //        rect.Left - img.Width,
                //        rect.Top - img.Height,
                //        img.Width,
                //        img.Height);
                //    if (rect2.Y + this.AutoScrollPosition.Y < 0)
                //    {
                //        rect2.Y = this.AutoScrollPosition.Y;
                //    }
                //    return rect2;
                //}
                //else
                //{
                //    return Rectangle.Empty;
                //}
            }
        }
        /// <summary>
        /// 在控件的获得/失去焦点事件时是否触发文档的获得/失去焦点事件
        /// </summary>
        internal bool RaiseDocumentFoucsEventWhenControlFocusEvent = true;
        /// <summary>
        /// 延时启用GotFocus事件的时刻值
        /// </summary>
        private long _StartTickForDelayGotFocus = 0;

        /// <summary>
        /// 处理控件获得焦点事件,刷新光标
        /// </summary>
        /// <param name="e">事件参数</param>
        protected override void OnGotFocus(EventArgs e)
        {
            base.OnGotFocus(e);
            if (this._StartTickForDelayGotFocus > 0)
            {
                // Environment.TickCount有可能为负数，在此进行绝对值的判断
                if (Math.Abs(Environment.TickCount - _StartTickForDelayGotFocus) > 100)
                {
                    // 通过了禁用期
                    _StartTickForDelayGotFocus = 0;
                }
                else
                {
                    // 还处于禁用期，不执行后续操作
                    return;
                }
            }

            if (this._Document == null)
            {
                // 无文档，不处理事件
                return;
            }
            if (this._ShowDialogTick > 0)
            {
                this._ShowDialogTick = 0;
            }
            if (this.IsEditingElementValue == false)
            {
                // 正在编辑文档元素内容时可能会弹出窗体导致控件失去或获得焦点
                // 对这种情况不需处理
                if (this._Document != null && this.RaiseDocumentFoucsEventWhenControlFocusEvent)
                {
                    this._Document.OnControlGotFocus();
                }
            }
        }

        /// <summary>
        /// 处理控件失去焦点事件
        /// </summary>
        /// <param name="e">事件参数</param>
        protected override void OnLostFocus(EventArgs e)
        {
            //DCSoft.WinForms.Native.WindowInformation info = new WindowInformation(this);
            base.OnLostFocus(e);
            {
                if (this._Document != null)
                {
                    if (this.IsEditingElementValue == false)
                    {
                        // 正在编辑文档元素内容时可能会弹出窗体导致控件失去或获得焦点
                        // 对这种情况不需处理
                        if (this.RaiseDocumentFoucsEventWhenControlFocusEvent)
                        {
                            this._Document.OnControlLostFocus();
                        }
                        this._Document.OnControlLostFocusExt();
                    }
                }
            }
            try
            {
            }
            catch (Exception ext)
            {
                DCConsole.Default.WriteLineError("Get ShowDialogTick:" + ext.Message);
                this._ShowDialogTick = -1;
            }
        }

        private long _ShowDialogTick = 0;

        /// <summary>
        /// 插入从UI输入的字符串
        /// </summary>
        /// <param name="text"></param>
        internal void InsertStringFromUI(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return;
            }
            if (text.Length == 1 && (text[0] == 13 || text[0] == 10) && EndTickForIgnoreEnterChar > 0)
            {
                var tick = WriterUtilsInner.TickCount - EndTickForIgnoreEnterChar;
                if (tick < 80)
                {
                    return;
                }
                EndTickForIgnoreEnterChar = 0;
            }
            if (string.IsNullOrEmpty(text))
            {
                // 取消事件或者转换成空字符串了则退出处理
                return;
            }
            if (this.Readonly == false
                && this.DocumentControler.CanInsertElementAtCurrentPosition(
                    typeof(DomCharElement),
                    DomAccessFlags.Normal))
            {
                DocumentEventArgs args2 = DocumentEventArgs.CreateKeyPress(this.Document, text[0]);
                this.Document.HandleDocumentEvent(args2);
                if (args2.Handled)
                {
                    // 已经被文档对象处理了，不再进行后续操作
                    return;
                }
                // 将输入法输入的文本整体插入到文档中
                var list = this.DocumentControler.InsertString(
                       text,
                       true,
                       InputValueSource.UI,
                       null,
                       null);
            }
        }


        /// <summary>
        /// 正在销毁控件
        /// </summary>
        internal bool _DisposingControl = false;

        /// <summary>
        /// 销毁对象
        /// </summary>
        /// <param name="disposing"></param>
        public override void Dispose()
        {
            base.Dispose();
            this._LastCurrentElementForDragEvent = null;
            if (this._CurrentMouseCapturer != null)
            {
                this._CurrentMouseCapturer.Dispose();
                this._CurrentMouseCapturer = null;
            }
            if (this._CurrentPrintTaskOptions != null)
            {
                this._CurrentPrintTaskOptions.Dispose();
                this._CurrentPrintTaskOptions = null;
            }
            if (this._Document != null)
            {
                this._Document.Dispose();
                this._Document = null;
            }
            this._CurrentPrintTaskOptions = null;

            this._DisposingControl = true;
            if (this._DocumentOptions != null)
            {
                this._DocumentOptions.InnerDispose();
                this._DocumentOptions = null;
            }
            this._OwnerWriterControl = null;
            if (this._CommandControler != null)
            {
                if (this._CommandControler.EditControl == this.OwnerWriterControl)
                {
                    this._CommandControler.EditControl = null;
                }
                this._CommandControler = null;
            }
            if (this._Document != null
                && this._Document.EditorControl == this.OwnerWriterControl)
            {
                this._Document.EditorControl = null;
            }
            if (this.AutoDisposeDocument)
            {
                if (this._Document != null)
                {
                    this._Document.Dispose();
                    this._Document = null;
                }
            }
            if (this._DocumentControler != null)
            {
                this._DocumentControler.Clear();
                this._DocumentControler.ClearBuffer();
                this._DocumentControler = null;
            }
            if (this._InvalidateInfos != null)
            {
                this._InvalidateInfos.Clear();
                this._InvalidateInfos = null;
            }
            this._MouseCaptureTransform = null;
            this._OwnerWriterControl = null;
            this._DocumentOptions = null;
            this._DragableElement = null;
            if (_EditorHost != null)
            {
                _EditorHost.Dispose();
                _EditorHost = null;
            }
            this._OwnerWriterControl = null;
            this._CommandControler = null;
            this._DocumentControler = null;
            this._DocumentOptions = null;
            this._DragableElement = null;
            this._EditorHost = null;
            this._MouseCaptureTransform = null;
            this._OwnerWriterControl = null;
            this._SelectionAreaRectangles = null;
            this.StyleInfoForFormatBrush = null;
            this._ToolTips = null;
        }

        private TextWindowsFormsEditorHostClass _EditorHost = null;

        /// <summary>
        /// 编辑器宿主对象
        /// </summary>
        internal TextWindowsFormsEditorHostClass EditorHost
        {
            get
            {
                if (this._DisposingControl)
                {
                    return this._EditorHost;
                }
                if (this.IsDisposed)
                {
                    return null;
                }
                if (_EditorHost == null)
                {
                    _EditorHost = new TextWindowsFormsEditorHostClass();
                }
                _EditorHost.SetOwner(this.OwnerWriterControl, this.Document);
                return _EditorHost;
            }
        }

        internal SimpleRectangleTransform GetOwnerTransform(DomElement element)
        {
            float x = element.GetAbsLeft();
            float y = element.GetAbsTop();
            foreach (SimpleRectangleTransform item in this.PagesTransform)
            {
                if (item.DescRectF.Contains(x, y))
                {
                    return item;
                }
            }
            return null;
        }
    }
}