using System;
using System.Collections.Generic;
using System.Text;
using DCSoft.Printing;
using DCSoft.Drawing;
using System.Windows.Forms;
using System.ComponentModel ;
using DCSoft.WinForms;
using DCSoft.WinForms.Native;
using DCSoft.Writer.Dom;

// 袁永福到此一游

namespace DCSoft.Writer.Controls
{
    /// <summary>
    /// 标尺对象
    /// </summary>
    public class DCDocumentRuleControl : System.Windows.Forms.Control
    {
        internal static Bitmap ImageRuleUpButton = null;
        internal static Bitmap ImageRuleDownButton = null;
        public static void CheckResourceImage()
        {
            if (ImageRuleDownButton == null)
            {
                ImageRuleDownButton = WriterResourcesCore.RuleDownButton;
                ImageRuleUpButton = WriterResourcesCore.RuleUpButton;
            }
        }

        /// <summary>
        /// 初始化对象
        /// </summary>
        public DCDocumentRuleControl()
        {
            DCSoft.WinForms.WinFormUtils.SetFormDefaultFont(this);

        }

        public int PositionOffsetX = 0;
        public int PositionOffsetY = 0;

        private DomParagraphFlagElement _CurrentParagraphFlag = null;
        /// <summary>
        /// 当前段落符号元素对象
        /// </summary>
       //////[Browsable(false)]
        public DomParagraphFlagElement CurrentParagraphFlag
        {
            get
            {
                return _CurrentParagraphFlag;
            }
            set
            {
                if (_CurrentParagraphFlag != value)
                {
                    _CurrentParagraphFlag = value;
                    {
                        this.Invalidate();
                    }
                }
            }
        }

        public string _ToolTip = null;
        /// <summary>
        /// 设置提示文本
        /// </summary>
        /// <param name="txt"></param>
        private void SetToolTip(string txt)
        {
            this._ToolTip = txt;
        }
        private WriterControl _Control = null;
        /// <summary>
        /// 绑定编辑器控件
        /// </summary>
        /// <param name="ctl"></param>
        internal void BindControl(WriterControl ctl)
        {
            if (ctl == null)
            {
                throw new ArgumentNullException("ctl");
            }
            this._Control = ctl;
        }



        /// <summary>
        /// 清除数据，取消绑定控件
        /// </summary>
        internal void ClearData()
        {
            this._Control = null;
            this._CurrentMouseCapturer = null;
            this._CurrentPage = null;
            this._CurrentParagraphFlag = null;
            this._ToolTip = null;
        }


        public override void Dispose()
        {
            base.Dispose();
            this.ClearData();
        }
        /// <summary>
        /// 设置是否是只读的
        /// </summary>
        private bool PageSettingsReadonly
        {
            get
            {
                if (this._Control == null)
                {
                    return true;
                }
                return this._Control.Readonly;
            }
        }
        /// <summary>
        /// 段落设置只读
        /// </summary>
        private bool ParagraphSettingsReadonly
        {
            get
            {
                if (this.CurrentParagraphFlag == null)
                {
                    return true;
                }
                return !this._Control.Document.DocumentControler.CanModifyParagraphs;
            }
        }
        public override void Invalidate()
        {
            this._Control.WASMParent?.RuleInvalidateView(this.Name);
        }
        public override void Invalidate(bool invalidateChildren)
        {
            this.Invalidate();
        }
        public void ViewControl_CurrentPageChanged(object sender, EventArgs e)
        {
            this.UpdateState(false);
        }
        /// <summary>
        /// 水平缩放比率
        /// </summary>
        private float XZoomRate
        {
            get
            {
                if (this._Control != null && this._Control.WASMParent != null)
                {
                    return this._Control.WASMZoomRate;
                }
                else
                {
                    return 1f;
                }
            }
        }

        /// <summary>
        /// 垂直缩放比率
        /// </summary>
        private float YZoomRate
        {
            get
            {
                if (this._Control != null && this._Control.WASMParent != null)
                {
                    return this._Control.WASMZoomRate;
                }
                else
                {
                    return 1f;
                }
            }
        }

        /// <summary>
        /// 页面设置对象
        /// </summary>
        internal XPageSettings PageSettings = null;

        /// <summary>
        /// 标尺方向
        /// </summary>
        internal DCDocumentRuleDirection Direction = DCDocumentRuleDirection.Horizontal;

        public const int StdPixelSize = 24;
        internal int GetRuntimeStdPixelSize()
        {
            return StdPixelSize;
        }
        public static readonly Color StdBackColor = Color.FromArgb(155, 187, 227);
        public static  SolidBrush StdClientBackColorBrush = new SolidBrush( Color.FromArgb(177, 202, 235));
        public static  Pen StdClientBorderColorPen = new Pen( Color.FromArgb(213, 226, 243));
        public static  SolidBrush StdTextColorBrush = new SolidBrush( Color.FromArgb(90, 97, 108) );
        public static Pen StdRuleLineColorPen = new Pen(Color.FromArgb(128, 128, 128));

        /// <summary>
        /// 工作区域边界
        /// </summary>
        private Rectangle _WorkingAreaBounds = Rectangle.Empty;
        private RectangleF _ContentAreaBounds = RectangleF.Empty;
        private RectangleF _ClientBounds = RectangleF.Empty;
        /// <summary>
        /// 当前页面对象
        /// </summary>
        private PrintPage _CurrentPage = null;
        public void ClearDocumentState()
        {
            _CurrentPage = null;
            _CurrentParagraphFlag = null;
            this.PageSettings = null;
        }
        private float _LastZoomRate = 1f;
        /// <summary>
        /// 更新状态
        /// </summary>
        internal void UpdateState(bool forScrollEvent)
        {
            var viewControl = this._Control.GetInnerViewControl();
            if (viewControl == null)
            {
                return;
            }
            this.PageSettings = null;
            this._CurrentPage = this._Control.CurrentPage;
            if (this._CurrentPage == null)
            {
                this._CurrentPage = this._Control.Pages.FirstPage;
            }
            if (this._CurrentPage == null)
            {
                return;
            }
            this.PageSettings = this._CurrentPage.PageSettings;
            var runtimePixelSize = this.GetRuntimeStdPixelSize();
            if (this.Direction == DCDocumentRuleDirection.Horizontal)
            {
                this._LastZoomRate = this.XZoomRate;
                Rectangle rect = this._CurrentPage.ClientBounds;
                rect.X = 0;
                rect.Width = (int)(GraphicsUnitConvert.Convert(
                    this.PageSettings.ViewPaperWidth,
                    GraphicsUnit.Document,
                    GraphicsUnit.Pixel) * this.XZoomRate);
                Rectangle rect2 = new Rectangle(
                    rect.Left,
                    (this.Height - runtimePixelSize) / 2,
                    rect.Width,
                    runtimePixelSize);
                    this._WorkingAreaBounds = rect2;
                    this.Invalidate();
            }
            else if (this.Direction == DCDocumentRuleDirection.Vertical)
            {
                this._LastZoomRate = this.YZoomRate;
                // 垂直标尺
                Rectangle rect = this._CurrentPage.ClientBounds;
                rect.Y = 0;
                Rectangle rect2 = new Rectangle(
                    (this.Width - runtimePixelSize) / 2,
                    rect.Top,
                    runtimePixelSize,
                    rect.Height);
                    this._WorkingAreaBounds = rect2;
                    if (this.ClientRectangle.IntersectsWith(this._WorkingAreaBounds)
                        || this._LastDrawContentFlag)
                    {
                        this.Invalidate();
                    }
            }
        }

        /// <summary>
        /// 控件可见性发生改变事件
        /// </summary>
        /// <param name="e"></param>
        protected override void OnVisibleChanged(EventArgs e)
        {
            base.OnVisibleChanged(e);
            this.UpdateState(false);
        }

        /// <summary>
        /// 当前使用的鼠标拖拽处理对象
        /// </summary>
        internal DCSoft.WinForms.Native.MouseCapturer _CurrentMouseCapturer = null;

        /// <summary>
        /// 处理鼠标事件
        /// </summary>
        /// <param name="e">参数</param>
        /// <param name="type">事件类型</param>
        internal void HandleMouseEvent(MouseEventArgs e, MouseEventType type)
        {
            if (type == MouseEventType.MouseMove)
            {
                if (this._CurrentMouseCapturer != null)
                {
                    if (this._CurrentMouseCapturer.HandleMouseMove(e))
                    {
                        return;
                    }
                }
            }
            else if( type == MouseEventType.MouseUp)
            {
                if (this._CurrentMouseCapturer != null)
                {
                    this._CurrentMouseCapturer.HandleMouseUp(e);
                    this._CurrentMouseCapturer = null;
                    this._Control.WASMParent.CloseReversibleShape();
                    return;
                }
            }


            Cursor cur = WriterControl.InnerCursorArrow;// Cursors.Arrow;
            string tipText = null;

            if (this.PageSettings == null)
            {
                // 没有数据，不处理
                return;
            }

            DomDocument document = this._Control.Document;

            if (this.Direction == DCDocumentRuleDirection.Horizontal)
            {
                int minLeftForIndent = (int)GraphicsUnitConvert.Convert(200.0, GraphicsUnit.Document, GraphicsUnit.Pixel);
                // 水平标尺
                if (this._BoundsFirstLineIndent.IsEmpty == false
                    && this._BoundsFirstLineIndent.Contains(e.X, e.Y))
                {
                    // 首行缩进
                    if (this.ParagraphSettingsReadonly)
                    {
                        // 段落设置是只读的
                        SetToolTip(null);
                        return;
                    }
                    tipText = DCSR.ParagraphFirstLineIndent;
                    if (type == MouseEventType.MouseDown && e.Button == System.Windows.Forms.MouseButtons.Left)
                    {
                        // 鼠标左键按下事件，拖拽设置段落首行缩进
                        this._ClipBoundsForDragPoint = new Rectangle(
                            this._BoundsContentElement.Left,
                            1,
                            this._BoundsContentElement.Width - minLeftForIndent,
                            1);
                        this.DragPosition(delegate (Point[] ps)
                        {
                            DocumentContentStyle style = (DocumentContentStyle)this.CurrentParagraphFlag.RuntimeStyle.CloneParent();
                            int firstLineIndent = (int)(GraphicsUnitConvert.Convert(
                                (ps[1].X - _BoundsContentElement.Left) / this.XZoomRate,
                                GraphicsUnit.Pixel,
                                DCSystem_Drawing.GraphicsUnit.Document) - style.LeftIndent);
                            style.DisableDefaultValue = true;
                            if (style.FirstLineIndent != firstLineIndent)
                            {
                                // 设置段落首行缩进
                                DomContentElement ce = this.CurrentParagraphFlag.ContentElement;
                                RectangleF cecBounds = ce.AbsClientBounds;
                                if (firstLineIndent >= -style.LeftIndent && firstLineIndent < cecBounds.Width * 0.8)
                                {
                                    style.FirstLineIndent = firstLineIndent;
                                    document.BeginLogUndo();
                                    DomElementList list = document.Selection.SetParagraphStyle(style);
                                    if (list != null && list.Count > 0)
                                    {
                                        document.EndLogUndo();
                                        document.Modified = true;
                                        document.OnSelectionChanged();
                                        document.OnDocumentContentChanged();
                                        this.Invalidate();
                                    }
                                    else
                                    {
                                        document.EndLogUndo();
                                    }
                                }//if
                            }//if
                        });
                    }//if
                }
                else if (this._BoundsLeftIndent.IsEmpty == false && this._BoundsLeftIndent.Contains(e.X, e.Y))
                {
                    // 悬挂缩进
                    if (this.ParagraphSettingsReadonly)
                    {
                        // 段落设置是只读的
                        SetToolTip(null);
                        return;
                    }
                    tipText = DCSR.ParagraphLeftIndent;
                    if (type == MouseEventType.MouseDown && e.Button == System.Windows.Forms.MouseButtons.Left)
                    {
                        // 鼠标左键按下事件，拖拽设置段落首行缩进
                        this._ClipBoundsForDragPoint = new Rectangle(
                              this._BoundsContentElement.Left,
                              1,
                              this._BoundsContentElement.Width - minLeftForIndent,
                              1);
                        this.DragPosition(delegate (Point[] ps)
                        {
                            int leftIndent = (int)GraphicsUnitConvert.Convert(
                                (ps[1].X - _BoundsContentElement.Left) / this.XZoomRate,
                                GraphicsUnit.Pixel,
                                DCSystem_Drawing.GraphicsUnit.Document);
                            if (leftIndent < 0)
                            {
                                leftIndent = 0;
                            }
                            DocumentContentStyle style = (DocumentContentStyle)this.CurrentParagraphFlag.RuntimeStyle.CloneParent();
                            style.DisableDefaultValue = true;
                            if (style.LeftIndent != leftIndent)
                            {
                                // 设置段落首行缩进
                                DomContentElement ce = this.CurrentParagraphFlag.ContentElement;
                                RectangleF cecBounds = ce.AbsClientBounds;
                                if (leftIndent >= 0 && leftIndent < cecBounds.Width * 0.8)
                                {
                                    style.LeftIndent = leftIndent;
                                    style.FirstLineIndent = Math.Max(style.FirstLineIndent, -style.LeftIndent);
                                    document.BeginLogUndo();
                                    DomElementList list = document.Selection.SetParagraphStyle(style);
                                    if (list != null && list.Count > 0)
                                    {
                                        document.EndLogUndo();
                                        document.Modified = true;
                                        document.OnSelectionChanged();
                                        document.OnDocumentContentChanged();
                                        this.Invalidate();
                                    }
                                    else
                                    {
                                        document.EndLogUndo();
                                    }
                                }//if
                            }//if
                        });
                    }//if
                }
                else if (e.Y >= this._ContentAreaBounds.Top && e.Y <= this._ContentAreaBounds.Bottom)
                {
                    if (this.PageSettingsReadonly)
                    {
                        // 页面设置是只读的，无法处理
                        SetToolTip(null);
                        return;
                    }
                    int pw = this.PageSettings.Landscape ? this.PageSettings.PaperHeight : this.PageSettings.PaperWidth;
                    if (Math.Abs(e.X - this._ContentAreaBounds.Left) < 2)
                    {
                        cur = WriterControl.InnerCursorSizeWE;// Cursors.VSplit;
                        if (type == MouseEventType.MouseMove)
                        {
                            // 鼠标移动事件
                            tipText = DCSR.PageLeftMargin;
                        }
                        else if (type == MouseEventType.MouseDown
                            && e.Button == System.Windows.Forms.MouseButtons.Left)
                        {
                            // 鼠标左键按下事件,拖拽设置左边距
                            int fix = (int)GraphicsUnitConvert.Convert(
                                this.PageSettings.RightMargin * 3.0,
                                GraphicsUnit.Document,
                                GraphicsUnit.Pixel);
                            this._ClipBoundsForDragPoint = new Rectangle(
                                _WorkingAreaBounds.Left,
                                1,
                                _WorkingAreaBounds.Width - fix,
                                1);
                            this.DragPosition(delegate (Point[] ps)
                            {
                                int leftMargin = (int)GraphicsUnitConvert.PixelToPrintUnit(
                                    (ps[1].X - this._ClientBounds.X) / this.XZoomRate);
                                bool setValue = false;
                                if (leftMargin > 0 && this.PageSettings.LeftMargin != leftMargin)
                                {
                                    if (leftMargin + this.PageSettings.RightMargin < pw * 0.8)
                                    {
                                        setValue = true;
                                    }
                                }
                                if (setValue)
                                {
                                    if (document.BeginLogUndo())
                                    {
                                        document.UndoList.AddProperty("LeftMargin",
                                            this.PageSettings.LeftMargin,
                                            leftMargin,
                                            this.PageSettings);
                                        document.UndoList.AddMethod(Dom.Undo.UndoMethodTypes.RefreshLayout);
                                        document.EndLogUndo();
                                    }
                                    this.PageSettings.LeftMargin = leftMargin;
                                    this._Control.RefreshDocumentExt(false, true);
                                    document.Modified = true;
                                    document.OnDocumentContentChanged();
                                    this.Invalidate();
                                }
                            });
                        }
                    }
                    else if (Math.Abs(e.X - this._ContentAreaBounds.Right) < 2)
                    {
                        cur = WriterControl.InnerCursorSizeWE;// Cursors.VSplit;
                        if (type == MouseEventType.MouseMove)
                        {
                            // 鼠标移动事件
                            tipText = DCSR.PageRightMargin;
                        }
                        else if (type == MouseEventType.MouseDown && e.Button == System.Windows.Forms.MouseButtons.Left)
                        {
                            // 鼠标左键按下事件,拖拽设置右边距
                            int fix = (int)GraphicsUnitConvert.Convert(
                                this.PageSettings.LeftMargin * 3.0,
                                GraphicsUnit.Document,
                                GraphicsUnit.Pixel);
                            this._ClipBoundsForDragPoint = new Rectangle(
                                _WorkingAreaBounds.Left + fix,
                                1,
                                (int)(_WorkingAreaBounds.Width - fix),
                                1);

                            this.DragPosition(delegate (Point[] ps)
                            {
                                int rightMargin = (int)GraphicsUnitConvert.PixelToPrintUnit(
                                    (this._ClientBounds.Right - ps[1].X) / this.XZoomRate);
                                bool setValue = false;
                                if (rightMargin > 0 && this.PageSettings.RightMargin != rightMargin)
                                {
                                    if (rightMargin + this.PageSettings.LeftMargin < pw * 0.8)
                                    {
                                        setValue = true;
                                    }
                                }
                                if (setValue)
                                {
                                    if (document.BeginLogUndo())
                                    {
                                        document.UndoList.AddProperty("RightMargin",
                                            this.PageSettings.RightMargin,
                                            rightMargin,
                                            this.PageSettings);
                                        document.UndoList.AddMethod(Dom.Undo.UndoMethodTypes.RefreshLayout);
                                        document.EndLogUndo();
                                    }
                                    this.PageSettings.RightMargin = rightMargin;
                                    this._Control.RefreshDocumentExt(false, true);
                                    document.Modified = true;
                                    document.OnDocumentContentChanged();
                                    this.Invalidate();
                                }
                            });
                        }
                    }
                }
            }
            else if (this.Direction == DCDocumentRuleDirection.Vertical)
            {
                // 垂直标尺
                if (this.PageSettingsReadonly)
                {
                    // 页面设置是只读的，无法处理
                    SetToolTip(null);
                    return;
                }
                float ph = this.PageSettings.Landscape ? this.PageSettings.PaperWidth : this.PageSettings.PaperHeight;
                if (e.X >= this._ContentAreaBounds.Left && e.X <= this._ContentAreaBounds.Right)
                {
                    if (Math.Abs(e.Y - this._ContentAreaBounds.Top) < 2)
                    {
                        cur = WriterControl.InnerCursorSizeNS;// Cursors.HSplit;
                        if (type == MouseEventType.MouseMove)
                        {
                            // 鼠标移动事件
                            tipText = DCSR.PageTopMargin;
                        }
                        else if (type == MouseEventType.MouseDown && e.Button == System.Windows.Forms.MouseButtons.Left)
                        {
                            // 鼠标左键按下事件,拖拽设置上边距
                            int fix = (int)GraphicsUnitConvert.Convert(
                                this.PageSettings.BottomMargin * 3.0,
                                GraphicsUnit.Document,
                                GraphicsUnit.Pixel);
                            this._ClipBoundsForDragPoint = new Rectangle(
                                1,
                                _WorkingAreaBounds.Top,
                                1,
                                _WorkingAreaBounds.Height - fix);
                            this.DragPosition(delegate (Point[] ps)
                            {
                                int topMargin = (int)GraphicsUnitConvert.PixelToPrintUnit(
                                    (ps[1].Y - this._ClientBounds.Y) / this.YZoomRate);
                                bool setValue = false;
                                if (topMargin > 0 && this.PageSettings.TopMargin != topMargin)
                                {
                                    if (topMargin + this.PageSettings.BottomMargin < ph * 0.8)
                                    {
                                        setValue = true;
                                    }
                                }
                                if (setValue)
                                {
                                    if (document.BeginLogUndo())
                                    {
                                        document.UndoList.AddProperty("TopMargin",
                                            this.PageSettings.TopMargin,
                                            topMargin,
                                            this.PageSettings);
                                        document.UndoList.AddMethod(Dom.Undo.UndoMethodTypes.RefreshLayout);
                                        document.EndLogUndo();
                                    }
                                    this.PageSettings.TopMargin = topMargin;
                                    this._Control.RefreshDocumentExt(false, true);
                                    document.Modified = true;
                                    document.OnDocumentContentChanged();
                                    this.Invalidate();
                                }
                            });
                        }
                    }
                    else if (Math.Abs(e.Y - this._ContentAreaBounds.Bottom) < 2)
                    {
                        cur = WriterControl.InnerCursorSizeNS;// Cursors.HSplit;
                        if (type == MouseEventType.MouseMove)
                        {
                            // 鼠标移动事件
                            tipText = DCSR.PageBottomMargin;
                        }
                        else if (type == MouseEventType.MouseDown && e.Button == System.Windows.Forms.MouseButtons.Left)
                        {
                            // 鼠标左键按下事件,拖拽设置下边距
                            int fix = (int)GraphicsUnitConvert.Convert(
                                this.PageSettings.TopMargin * 3.0,
                                GraphicsUnit.Document,
                                GraphicsUnit.Pixel);
                            this._ClipBoundsForDragPoint = new Rectangle(
                                1,
                                _WorkingAreaBounds.Top + fix,
                                1,
                                _WorkingAreaBounds.Height - fix);
                            this.DragPosition(delegate (Point[] ps)
                            {
                                int bottomMargin = (int)GraphicsUnitConvert.PixelToPrintUnit(
                                    (this._ClientBounds.Bottom - ps[1].Y) / this.YZoomRate);
                                bool setValue = false;
                                if (bottomMargin > 0 && this.PageSettings.BottomMargin != bottomMargin)
                                {
                                    if (bottomMargin + this.PageSettings.TopMargin < ph * 0.8)
                                    {
                                        setValue = true;
                                    }
                                }
                                if (setValue)
                                {
                                    if (document.BeginLogUndo())
                                    {
                                        document.UndoList.AddProperty("BottomMargin",
                                            this.PageSettings.BottomMargin,
                                            bottomMargin,
                                            this.PageSettings);
                                        document.UndoList.AddMethod(Dom.Undo.UndoMethodTypes.RefreshLayout);
                                        document.EndLogUndo();
                                    }
                                    this.PageSettings.BottomMargin = bottomMargin;
                                    this._Control.RefreshDocumentExt(false, true);
                                    document.Modified = true;
                                    document.OnDocumentContentChanged();
                                    this.Invalidate();
                                }
                            });
                        }
                    }
                }
            }

            if (this.Cursor != cur)
            {
                this.Cursor = cur;
            }
            if (type == MouseEventType.MouseMove)
            {
                SetToolTip(tipText);
            }
        }
        internal delegate void ApplyDragPointsHandler(Point[] ps);

        //private Rectangle _RuntimeClipBoundsForDragPoint = Rectangle.Empty;

        private void DragPosition(ApplyDragPointsHandler applyCallback )
        {
            MouseCapturer mc = new MouseCapturer(this);
            mc.ReversibleShape = ReversibleShapeStyle.Custom;
            mc.Draw = new CaptureMouseMoveEventHandler(mc2_ReversibleDrawCallback);
            mc.EventOnFinished = delegate (object sender99, EventArgs args99) {
                    // 成功启动编辑内容，则返回拖拽结果
                    var ps2 = new Point[] { mc.StartPosition, mc.EndPosition };
                    applyCallback(ps2);
            };
            //this._ClipBoundsForDragPoint = Rectangle.Empty;
            this._CurrentMouseCapturer = mc;
        }

        private void mc2_ReversibleDrawCallback(
           object eventSender,
           CaptureMouseMoveEventArgs e)
        {
            var ctl = this._Control.GetInnerViewControl();
            //var pages = ctl.VisiblePages;
            var pageIndex = ctl.Pages.IndexOf(this._CurrentPage);
            if( pageIndex < 0 )
            {
                pageIndex = 0;
            }
            if (this.Direction == DCDocumentRuleDirection.Horizontal)
            {
                var x = e.CurrentPosition.X;
                if( this._ClipBoundsForDragPoint.IsEmpty == false )
                {
                    if( x < this._ClipBoundsForDragPoint.X )
                    {
                        x = this._ClipBoundsForDragPoint.X;
                    }
                    else if( x > this._ClipBoundsForDragPoint.Right )
                    {
                        x = this._ClipBoundsForDragPoint.Right;
                    }
                }
                x = (int)(x / this.XZoomRate);
                this._Control.WASMParent.ReversibleDraw(
                    pageIndex, 
                    x,
                    0,
                    x,
                    this._CurrentPage.ClientBounds.Height,
                    (int)DCSoft.WinForms.Native.ReversibleShapeStyle.Line);
            }
            else if( this.Direction == DCDocumentRuleDirection.Vertical )
            {
                var y = e.CurrentPosition.Y;
                if (this._ClipBoundsForDragPoint.IsEmpty == false)
                {
                    if (y < this._ClipBoundsForDragPoint.Y)
                    {
                        y = this._ClipBoundsForDragPoint.Y;
                    }
                    else if (y > this._ClipBoundsForDragPoint.Bottom)
                    {
                        y = this._ClipBoundsForDragPoint.Bottom;
                    }
                }
                y = (int)(y / this.YZoomRate);
                this._Control.WASMParent.ReversibleDraw(
                    pageIndex,
                    0,
                    y,
                    this._CurrentPage.ClientBounds.Width,
                    y,
                    (int)DCSoft.WinForms.Native.ReversibleShapeStyle.Line);
            }
        }

        /// <summary>
        /// 绘制控件视图
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaint(PaintEventArgs e)
        {
            this._LastDrawContentFlag = false;
            CheckResourceImage();
            if (this.Direction == DCDocumentRuleDirection.Horizontal)
            {
                this.DrawHorizontal(e.Graphics, e.ClipRectangle);
            }
            else if (this.Direction == DCDocumentRuleDirection.Vertical)
            {
                this.DrawVertical(e.Graphics, e.ClipRectangle);
            }
        }

        private bool _LastDrawContentFlag = false;
        /// <summary>
        /// 绘制水平标尺
        /// </summary>
        /// <param name="g"></param>
        /// <param name="clipRectangle"></param>
        private void DrawHorizontal(Graphics g, Rectangle clipRectangle)
        {
            if (this.PageSettings == null)
            {
                return;
            }
            float zoomRate = this.XZoomRate;
            if( this._LastZoomRate != zoomRate )
            {
                this.UpdateState(false);
            }
            this._LastDrawContentFlag = true;
            // 计算坐标
            float rate = (float)(GraphicsUnitConvert.GetRate(GraphicsUnit.Pixel, GraphicsUnit.Inch) / 100.0) * zoomRate;
            float step = (float)GraphicsUnitConvert.Convert(10, GraphicsUnit.Millimeter, GraphicsUnit.Pixel);
            step = step * zoomRate;
            // 计算客户区边界
            this._ClientBounds = new RectangleF(
                this._WorkingAreaBounds.Left,
                this._WorkingAreaBounds.Top + 6,
                this._WorkingAreaBounds.Width,
                this._WorkingAreaBounds.Height - 12);
            // 计算文档正文区域边界
            float pw = this.PageSettings.Landscape ? this.PageSettings.PaperHeight : this.PageSettings.PaperWidth;
            //g.GraphicsSetClip(this._ClipRectangle);
            this._ContentAreaBounds.X = this._ClientBounds.Left + this.PageSettings.LeftMargin * rate;
            this._ContentAreaBounds.Y = this._ClientBounds.Top + 1;
            this._ContentAreaBounds.Width = (pw - this.PageSettings.LeftMargin - this.PageSettings.RightMargin) * rate;
            this._ContentAreaBounds.Height = this._ClientBounds.Height - 2;
            // 绘制背景
            //g.FillRectangle(GraphicsObjectBuffer.GetSolidBrush(StdBackColor), this.Left, this.Top, this.Width, this.Height);
            Rectangle runtimeClientBounds = new Rectangle(
                (int)this._ClientBounds.Left,
                (int)this._ClientBounds.Top,
                (int)this._ClientBounds.Width,
                (int)this._ClientBounds.Height);
            g.FillRectangle(
                StdClientBackColorBrush,// GraphicsObjectBuffer.GetSolidBrush(StdClientBackColor),
                runtimeClientBounds);
            g.FillRectangle(Brushes.White, this._ContentAreaBounds);
            // 绘制边框
            g.DrawRectangle(
                StdClientBorderColorPen,// GraphicsObjectBuffer.GetPen(StdClientBorderColor),
                runtimeClientBounds);

            // 绘制标尺
            using (var format = new StringFormat(StringFormat.GenericTypographic))
            {
                format.Alignment = StringAlignment.Near;
                format.LineAlignment = StringAlignment.Center;
                format.FormatFlags = StringFormatFlags.NoWrap;
                for (int iCount = 0; iCount < 10000; iCount++)
                {
                    float pos = this._WorkingAreaBounds.Left + step * iCount;
                    if (pos >= this._ClientBounds.Right )
                    {
                        break;
                    }
                    var strText = DCSoft.Common.StringCommon.Int32ToString(iCount);
                    SizeF txtSize = g.MeasureString(strText, this.Font, 1000, format);
                    //RectangleF txtBounds = new RectangleF(
                    //    pos - txtSize.Width / 2,
                    //    this._ContentAreaBounds.Top,
                    //    txtSize.Width,
                    //    this._ContentAreaBounds.Height);
                    float txtPos = pos - txtSize.Width / 2;
                    if (iCount == 0)
                    {
                        txtPos = pos + 1;
                    }
                    //if (txtPos < _ClientBounds.Left)
                    //{
                    //    txtPos = _ClientBounds.Left;
                    //}
                    //if (txtPos + txtSize.Width > _ClientBounds.Right)
                    //{
                    //    txtPos = _ClientBounds.Right - txtSize.Width;
                    //}
                    g.DrawString(
                        strText,
                        this.Font,
                        StdTextColorBrush,
                        txtPos,
                        this._ContentAreaBounds.Top + (int)Math.Ceiling( txtSize.Height / 2 ) - 1,// ( _ContentAreaBounds.Height - txtSize.Height ) / 2,
                        format);
                    g.DrawLine(
                        StdRuleLineColorPen,
                        pos,
                        this._ContentAreaBounds.Bottom + 2,
                        pos,
                        this.Height - 2);
                    //_ClientBounds.Bottom - 1);
                }//for
            }//using
            if (this._CurrentParagraphFlag != null)
            {
                // 绘制段落刻度按钮
                GraphicsUnit unit = DCSystem_Drawing.GraphicsUnit.Document;
                int imgWidth = ImageRuleDownButton.Width;
                int imgHeight = ImageRuleDownButton.Height;
                DomContentElement contentElement = this._CurrentParagraphFlag.ContentElement;
                RuntimeDocumentContentStyle style = this._CurrentParagraphFlag.RuntimeStyle;
                RectangleF absBounds = contentElement.AbsClientBounds;
                this._BoundsContentElement.X = (int)(this._ContentAreaBounds.Left + GraphicsUnitConvert.Convert(absBounds.Left, unit, GraphicsUnit.Pixel) * zoomRate);
                this._BoundsContentElement.Width = (int)(GraphicsUnitConvert.Convert(absBounds.Width, unit, GraphicsUnit.Pixel) * zoomRate);
                // 计算行首缩进位置
                float pos = this._ContentAreaBounds.Left + GraphicsUnitConvert.Convert(absBounds.Left + style.FirstLineIndent + style.LeftIndent, unit, GraphicsUnit.Pixel) * zoomRate;
                this._BoundsFirstLineIndent = new Rectangle(
                    (int)(pos - imgWidth / 2),
                    (int)(this._ContentAreaBounds.Top - imgHeight / 2 - 1),
                    imgWidth,
                    imgHeight);
                if (clipRectangle.IntersectsWith(this._BoundsFirstLineIndent))
                {
                    g.DrawImageUnscaled(ImageRuleDownButton, this._BoundsFirstLineIndent);
                }
                // 计算段落左缩进位置
                pos = this._ContentAreaBounds.Left + GraphicsUnitConvert.Convert(
                    absBounds.Left + style.LeftIndent,
                    unit,
                    GraphicsUnit.Pixel) * zoomRate;
                this._BoundsLeftIndent = new Rectangle(
                    (int)(pos - imgWidth / 2),
                    (int)(this._ContentAreaBounds.Bottom - imgHeight / 2 + 1),
                    imgWidth,
                    imgHeight);
                if (clipRectangle.IntersectsWith(this._BoundsLeftIndent))
                {
                    g.DrawImageUnscaled(ImageRuleUpButton, this._BoundsLeftIndent);
                }
                //// 计算段落右缩进位置
                //pos = _ContentAreaBounds.Left + GraphicsUnitConvert.Convert(absBounds.Right + style.RightToLeft , unit, GraphicsUnit.Pixel);
                //_BoundsLeftIndent = new Rectangle(
                //    (int)(pos - imgWidth / 2),
                //    (int)(_ContentAreaBounds.Bottom - imgHeight / 2),
                //    imgWidth,
                //    imgHeight);
                //if (clipRectangle.IntersectsWith(_BoundsLeftIndent))
                //{
                //    g.DrawImageUnscaled(ImageRuleDownButton, _BoundsLeftIndent);
                //}
            }
        }

        private Rectangle _BoundsContentElement = Rectangle.Empty;
        private Rectangle _BoundsFirstLineIndent = Rectangle.Empty;
        private Rectangle _BoundsLeftIndent = Rectangle.Empty;
        //private Rectangle _BoundsRightIndent = Rectangle.Empty;
        /// <summary>
        /// 拖拽操作的限制范围
        /// </summary>
        private Rectangle _ClipBoundsForDragPoint = Rectangle.Empty;

        /// <summary>
        /// 绘制垂直标尺
        /// </summary>
        /// <param name="g"></param>
        /// <param name="clipRectangle"></param>
        private void DrawVertical(Graphics g, Rectangle clipRectangle)
        {
            if (this.PageSettings == null)
            {
                return;
            }
            //if (this.ClientRectangle.IntersectsWith(this._WorkingAreaBounds) == false)
            //{
            //    return;
            //}
            float zoomRate = this.YZoomRate;
            if( this._LastZoomRate != zoomRate )
            {
                this.UpdateState(false);
            }
            this._LastDrawContentFlag = true;
            // 计算坐标
            float rate = (float)(GraphicsUnitConvert.GetRate(GraphicsUnit.Pixel, GraphicsUnit.Inch) / 100.0) * zoomRate;
            float step = (float)GraphicsUnitConvert.Convert(10.0, GraphicsUnit.Millimeter, GraphicsUnit.Pixel);
            step = step * zoomRate;
            // 计算客户区边界
            this._ClientBounds = new RectangleF(
                this._WorkingAreaBounds.Left + 6,
                this._WorkingAreaBounds.Top,
                this._WorkingAreaBounds.Width - 12,
                this._WorkingAreaBounds.Height);
            // 计算文档正文区域边界
            float ph = this.PageSettings.Landscape ? this.PageSettings.PaperWidth : this.PageSettings.PaperHeight;
            this._ContentAreaBounds.X = this._ClientBounds.Left + 1;
            this._ContentAreaBounds.Y = this._ClientBounds.Top + this.PageSettings.TopMargin * rate;
            this._ContentAreaBounds.Width = this._ClientBounds.Width - 2;// (ph - this.PageSettings.LeftMargin - this.PageSettings.RightMargin) * rate;
            this._ContentAreaBounds.Height = (ph - this.PageSettings.TopMargin - this.PageSettings.BottomMargin) * rate;
            // 绘制背景
            g.FillRectangle(
                StdClientBackColorBrush,// GraphicsObjectBuffer.GetSolidBrush(StdClientBackColor),
                _ClientBounds);
            g.FillRectangle(Brushes.White, _ContentAreaBounds);
            // 绘制边框
            g.DrawRectangle(
                StdClientBorderColorPen,// GraphicsObjectBuffer.GetPen(StdClientBorderColor),
                this._ClientBounds.Left,
                this._ClientBounds.Top,
                this._ClientBounds.Width,
                this._ClientBounds.Height);

            // 绘制标尺
            using (var format = new StringFormat(StringFormat.GenericTypographic))
            {
                format.Alignment = StringAlignment.Near;
                format.LineAlignment = StringAlignment.Center;
                //format.FormatFlags = StringFormatFlags.NoWrap | StringFormatFlags.DirectionVertical;
                for (int iCount = 0; iCount < 10000; iCount++)
                {
                    float pos = this._WorkingAreaBounds.Top + step * iCount;
                    if (pos >= this._ClientBounds.Bottom * zoomRate)
                    {
                        break;
                    }
                    var strText = DCSoft.Common.StringCommon.Int32ToString(iCount);
                    SizeF txtSize = g.MeasureString(strText, this.Font, 1000, format);
                    float txtPos = pos - txtSize.Height / 2;
                    if (iCount == 0)
                    {
                        txtPos = pos + 1;
                    }
                    g.DrawString(
                        strText,
                        this.Font,
                        StdTextColorBrush,
                        this._ContentAreaBounds.Left + (this._ContentAreaBounds.Width - txtSize.Width) / 2 ,
                         txtPos,
                         format);
                        g.DrawLine(
                            StdRuleLineColorPen,
                            this._ContentAreaBounds.Right + 2,
                            pos,
                            this.Width - 2,
                            pos);
                }//for
            }//using
        }
    }

    /// <summary>
    /// 标尺方向
    /// </summary>
    internal enum DCDocumentRuleDirection
    {
        /// <summary>
        /// 垂直方向
        /// </summary>
        Vertical,
        /// <summary>
        /// 水平方向
        /// </summary>
        Horizontal
    }
}
