using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
// // 
using DCSoft.Writer.Dom;
using System.ComponentModel;
using System.Runtime.InteropServices;
using DCSoft.Printing;
using DCSoft.Drawing;
using DCSoft.WinForms;
using DCSoft.WinForms.Native;
using DCSoft.Common;
using System.Runtime.InteropServices.JavaScript;

namespace DCSoft.Writer.Controls
{
    /// <summary>
    /// 编辑器鼠标事件处理
    /// </summary>
    public partial class WriterViewControl
    {
        /// <summary>
        /// 触发鼠标悬停的文档元素改变事件。
        /// </summary>
        /// <param name="oldHoverElement">旧的鼠标悬停元素</param>
        /// <param name="newHoverElement">新的鼠标悬停元素</param>
        public void SetHoverElement(
            DomElement oldHoverElement,
            DomElement newHoverElement)
        {
            var document = this.Document;
            if (document.HighlightManager != null)
            {
                HighlightInfo info = document.HighlightManager[oldHoverElement];
                if (info != null && info.ActiveStyle == HighlightActiveStyle.Hover)
                {
                    this.Invalidate(info.Range);
                }
                info = document.HighlightManager[newHoverElement];
                if (info != null && info.ActiveStyle == HighlightActiveStyle.Hover)
                {
                    this.Invalidate(info.Range);
                }
            }
        }


        #region 处理OLE拖拽事件 ***********************************************

        /// <summary>
        /// 能否直接拖拽文档内容
        /// </summary>
        public bool AllowDragContent
        {
            get
            {
                return this.DocumentOptions.BehaviorOptions.AllowDragContent;
            }
            set
            {
                this.DocumentOptions.BehaviorOptions.AllowDragContent = value;
            }
        }

        /// <summary>
        /// 编辑器控件能接受的数据格式，包括粘贴操作和OLE拖拽操作获得的数据
        /// </summary>
        public WriterDataFormats AcceptDataFormats
        {
            get
            {
                return this.DocumentOptions.BehaviorOptions.AcceptDataFormats;
            }
            set
            {
                this.DocumentOptions.BehaviorOptions.AcceptDataFormats = value;
            }
        }

        /// <summary>
        /// 编辑器控件能创建的数据格式，这些数据将用于复制操作和OLE拖拽操作
        /// </summary>
        internal WriterDataFormats CreationDataFormats
        {
            get
            {
                return this.DocumentOptions.BehaviorOptions.CreationDataFormats;
            }
            set
            {
                this.DocumentOptions.BehaviorOptions.CreationDataFormats = value;
            }
        }

        /// <summary>
        /// 处理OLE拖拽进入事件
        /// </summary>
        /// <param name="drgevent">事件参数</param>
        protected override void OnDragEnter(DragEventArgs drgevent)
        {
            //this._CurrentEventHandled = false;
            drgevent.Effect = DragDropEffects.None;
            //this._LastUIEventTime = DateTime.Now;
            if (this._Document == null)
            {
                // 无文档，不处理事件
                return;
            }
            drgevent.Effect = DragDropEffects.None;
            MyHandleDragEvent(drgevent, 0);
        }

        protected override void OnDragLeave(EventArgs e)
        {
        }

        /// <summary>
        /// 处理OLE拖拽经过事件
        /// </summary>
        /// <param name="drgevent">事件参数</param>
        protected override void OnDragOver(System.Windows.Forms.DragEventArgs drgevent)
        {
            //this._LastUIEventTime = DateTime.Now;
            if (this._DragState == DragOperationState.None)
            {
                this._DragState = DragOperationState.Drag;
            }
            if (this._Document == null)
            {
                // 无文档，不处理事件
                return;
            }
            drgevent.Effect = System.Windows.Forms.DragDropEffects.None;
            MyHandleDragEvent(drgevent, 0);
        }
        /// <summary>
        /// 处理OLE拖拽完成事件
        /// </summary>
        /// <param name="drgevent">事件参数</param>
        protected override void OnDragDrop(DragEventArgs drgevent)
        {
            //this._CurrentEventHandled = false;

            //this._LastUIEventTime = DateTime.Now;
            if (this._DragState == DragOperationState.None)
            {
                this._DragState = DragOperationState.Drag;
            }
            if (this._Document == null)
            {
                // 无文档，不处理事件
                return;
            }
            this.ForceShowCaret = false;
            MyHandleDragEvent(drgevent, 1);
            if (this._DragState != DragOperationState.DragDropInOwnerWriterControl)
            {
                this._DragState = DragOperationState.None;
            }
        }

        private DomElement _LastCurrentElementForDragEvent = null;
        private DragDropEffects _LastDragEventEffect = DragDropEffects.None;

        /// <summary>
        /// 内部的处理OLE拖拽事件
        /// </summary>
        /// <param name="drgevent"></param>
        /// <param name="style">事件类型 0:OnDragOver , 1:OnDragDrop</param>
        private void MyHandleDragEvent(DragEventArgs drgevent, int style)
        {
            if (this.PagesTransform != null)
            {
                SimpleRectangleTransform item = this.PagesTransform.GetItemByPoint(
                    drgevent.X,
                    drgevent.Y,
                    true,
                    true,
                    true);
                if (item != null)
                {
                    //Point p2 = new Point(drgevent.X, drgevent.Y);
                    //p2 = this.PointToClient(p2);
                    PointF p = new PointF(drgevent.X, drgevent.Y);// p2.X, p2.Y);
                    p = RectangleCommon.MoveInto(p, item.SourceRectF);
                    p = item.TransformPointF(p.X, p.Y);
                    DomDocument document = (DomDocument)item.DocumentObject;
                    DomElement element = document.Content.GetElementAt(p.X, p.Y, false);
                    if (element != null)
                    {
                        int index = document.Content.InnerFixIndex(
                            document.Content.FastIndexOf(element),
                            FixIndexDirection.Both,
                            false);
                        if (index >= 0)
                        {
                            element = document.Content[index];
                        }
                        else
                        {
                            drgevent.Effect = DragDropEffects.None;
                            return;
                        }
                        this.ForceShowCaret = true;
                        this.UseAbsTransformPoint = true;
                        if (this._DragState == DragOperationState.DragingSelection)
                        {
                            bool back = this.HideCaretWhenHasSelection;
                            this.HideCaretWhenHasSelection = false;
                            this.UpdateTextCaretByElement(element);
                            this.HideCaretWhenHasSelection = back;
                            if (document.Selection.Contains(element))
                            {
                                drgevent.Effect = DragDropEffects.None;
                                this.SetStatusText(null);
                                return;
                            }
                        }
                        else
                        {
                            this._DragState = DragOperationState.Drag;
                            document.Content.AutoClearSelection = true;
                            document.Content.MoveToPoint(p.X, p.Y);
                            element = document.CurrentElement;
                        }
                        this.UseAbsTransformPoint = false;
                        if (this._LastCurrentElementForDragEvent == element && style != 1)
                        {
                            drgevent.Effect = this._LastDragEventEffect;
                            return;
                        }
                        this._LastCurrentElementForDragEvent = element;
                        if (OnCanDragDrop(element, drgevent, p.X, p.Y))
                        {
                            if ((drgevent.KeyState & 4) == 4
                                    && (drgevent.AllowedEffect & DragDropEffects.Move) == DragDropEffects.Move)
                            {
                                SetStatusText(DCSR.WhereToMove);
                                drgevent.Effect = DragDropEffects.Move;
                            }
                            else if ((drgevent.KeyState & 8) == 8
                                && (drgevent.AllowedEffect & DragDropEffects.Copy) == DragDropEffects.Copy)
                            {
                                SetStatusText(DCSR.WhereToCopy);
                                drgevent.Effect = DragDropEffects.Copy;
                            }
                            else if ((drgevent.AllowedEffect & DragDropEffects.Move) == DragDropEffects.Move)
                            {
                                SetStatusText(DCSR.WhereToMove);
                                drgevent.Effect = DragDropEffects.Move;
                            }
                            else if ((drgevent.AllowedEffect & DragDropEffects.Copy) == DragDropEffects.Copy)
                            {
                                SetStatusText(DCSR.WhereToCopy);
                                drgevent.Effect = DragDropEffects.Copy;
                            }
                            else
                            {
                                SetStatusText(null);
                                drgevent.Effect = DragDropEffects.None;
                            }
                            if (style == 1)
                            {
                                InsertObjectEventArgs ioargs = new InsertObjectEventArgs(this.Document);
                                ioargs.AllowedEffect = drgevent.AllowedEffect;
                                ioargs.DragEffect = drgevent.Effect;
                                ioargs.DataObject = drgevent.Data;
                                ioargs.ShowUI = true;
                                ioargs.AutoSelectContent = true;
                                ioargs.InputSource = InputValueSource.DragDrop;
                                ioargs.Result = false;
                                //ioargs.SpecifyFormat = DocumentControler.XMLDataFormat;
                                ioargs.CurrentElement = element;
                                var whandle = ioargs.GetData(WriterConst.Name_WriterHandle);
                                if (whandle != null && whandle.ToString() == this.OwnerWriterControl.Handle.ToInt32().ToString())
                                {
                                    // 同一个控件内部的内容拖拽
                                    this._DragState = DragOperationState.DragDropInOwnerWriterControl;
                                }
                                if (this._DragState == DragOperationState.DragDropInOwnerWriterControl
                                    && drgevent.Effect == DragDropEffects.Move)
                                {
                                    // 进行操作可行性检测
                                    ioargs.DetectForDragContent = true;
                                    this.DocumentControler.InsertObject(ioargs);
                                    ioargs.DetectForDragContent = false;
                                    if (ioargs.Result == false)
                                    {
                                        // 检测不通过
                                        drgevent.Effect = DragDropEffects.None;
                                        this._LastDragEventEffect = drgevent.Effect;
                                        return;
                                    }
                                    ioargs.CurrentElement = null;
                                    this.DeleteSelection(true);
                                    drgevent.Effect = DragDropEffects.Copy;
                                }
                                document.Content.AutoClearSelection = true;
                                this.ForceShowCaret = false;

                                this.UseAbsTransformPoint = true;
                                document.Content.SetSelection(element.ContentIndex, 0);
                                //document.Content.MoveTo(ViewX, ViewY);
                                this.UseAbsTransformPoint = false;
                                //SetStatusText(null);
                                if (this._DragState == DragOperationState.DragDropInOwnerWriterControl)
                                {
                                    // 当前状态是同一个控件内部的内容拖拽
                                    this.DocumentControler.InsertObject(ioargs);
                                }
                                else
                                {
                                    this.OwnerWriterControl.OnEventInsertObject(ioargs);
                                }
                                drgevent.Effect = ioargs.DragEffect;
                                if (ioargs.Result == false)
                                {
                                    drgevent.Effect = DragDropEffects.None;
                                }
                                SetStatusText(null);
                                //document.DocumentControler.DragDrop(element, drgevent, p.X, p.Y);
                                this.Update();
                                this.Focus();
                                this._LastCurrentElementForDragEvent = null;
                            }
                        }
                        else
                        {
                            SetStatusText(null);
                            drgevent.Effect = DragDropEffects.None;
                            //if (style == 1)
                            //{
                            //    document.Content.MoveTo(p.X, p.Y);
                            //}
                        }
                        this._LastDragEventEffect = drgevent.Effect;
                    }
                }
            }
        }



        /// <summary>
        /// 判断是否可以接受处理OLE拖拽事件
        /// </summary>
        /// <param name="element">处理的元素</param>
        /// <param name="e">拖拽事件参数</param>
        /// <param name="ViewX">使用视图坐标的拖拽位置的X坐标</param>
        /// <param name="ViewY">使用视图坐标的拖拽位置的Y坐标</param>
        /// <returns>是否可以接受拖拽来的数据</returns>
        public bool OnCanDragDrop(
            DomElement element,
            System.Windows.Forms.DragEventArgs e,
            float ViewX,
            float ViewY)
        {
            CanInsertObjectEventArgs args = new CanInsertObjectEventArgs(this.Document);
            args.SpecifyPosition = this.Document.Content.FastIndexOf(element);
            args.DataObject = e.Data;
            args.SpecifyFormat = null;
            args.AccessFlags = DomAccessFlags.Normal;
            this.DocumentControler.CanInsertObject(args);
            return args.Result;
        }

        /// <summary>
        /// 执行粘贴操作
        /// </summary>
        /// <param name="specifyFormat">指定的数据格式</param>
        /// <param name="showUI">是否显示用户界面</param>
        /// <returns>操作是否成功</returns>
        public bool DoPaste( System.Windows.Forms.IDataObject dObj , string specifyFormat, bool showUI)
        {
            var args = new InsertObjectEventArgs(this.Document);
            args.DataObject = dObj;// this.OwnerWriterControl._DataObjectFromPaste;
            if (args.DataObject == null)
            {
                // 没有任何数据
                return false;
            }
            args.SpecifyFormat = specifyFormat;
            args.ShowUI = showUI;
            WriterDataFormats df = this.AcceptDataFormats;
            args.AllowDataFormats = df;
            args.WriterControl = this.OwnerWriterControl;
            args.InputSource = InputValueSource.Clipboard;
            this.OwnerWriterControl.OnEventInsertObject(args);
            if (args.Result == false)
            {
                if (args.RejectFormats.Count > 0 && showUI)
                {
                    if (this.DocumentOptions.BehaviorOptions.PromptForRejectFormat)
                    {
                        string msg = string.Format(
                            DCSR.PromptRejectFormat_Format,
                            WriterUtilsInner.ConcatString(args.RejectFormats, ','));
                        DCSoft.WinForms.DCUIHelper.ShowMessageBox(this, msg);
                    }
                }
            }
            return args.Result;
        }

        #endregion

        private SimpleRectangleTransform _MouseCaptureTransform = null;
        public void InnerSetMouseCaptureTransformByElement(DomElement element)
        {
            if (element == null)
            {
                this._MouseCaptureTransform = null;
            }
            else
            {
                this._MouseCaptureTransform = GetOwnerTransform(element);
            }
        }
        /// <summary>
        /// 鼠标拖拽使用的转换对象
        /// </summary>
        protected override TransformBase MouseCaptureTransform
        {
            get
            {
                if (_MouseCaptureTransform == null)
                {
                    return this.Transform;
                }
                else
                {
                    return _MouseCaptureTransform;
                }
            }
        }

        private long _DisableMouseEventTick = 0;

        /// <summary>
        /// 判断是否允许鼠标事件
        /// </summary>
        /// <returns>是否允许鼠标事件</returns>
        internal bool IsEnabledMouseEvent()
        {
            if (_DisableMouseEventTick > 0)
            {
                if (DateTime.Now.Ticks > _DisableMouseEventTick)
                {
                    _DisableMouseEventTick = 0;
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return true;
        }

        ///// <summary>
        ///// 禁止鼠标事件标记
        ///// </summary>
        //internal bool DisableMouseEvent
        //{
        //    get
        //    {
        //        return _DisableMouseEvent; 
        //    }
        //    set
        //    {
        //        _DisableMouseEvent = value; 
        //    }
        //}
        //public class LastMouseEventInfo
        //{
        //    public void SetValue( string strType , MouseEventArgs args )
        //    {
        //        this.Type = strType;
        //        this.X = args.X;
        //        this.Y = args.Y;
        //        this.Element = null;
        //    }
        //    public string Type = null;
        //    public XTextElement Element = null;
        //    public float X = 0;
        //    public float Y = 0;
        //    public bool LeftButton = false;
        //    public bool RightButton = false;
        //    public bool MiddleButton = false;
        //    public int Button = 0;
        //    public int Detail = 0;
        //}
        //internal LastMouseEventInfo _LastMouseEventInfo = new LastMouseEventInfo();
        /// <summary>
        /// 处理鼠标单击事件
        /// </summary>
        /// <param name="e">事件参数</param>
        protected override void OnMouseClick(MouseEventArgs e)
        {
            // 记录最后一次UI事件时刻
            //this._LastUIEventTime = DateTime.Now;
            if (this.IsEnabledMouseEvent() == false)
            {
                // 禁止鼠标事件
                return;
            }
            base.OnMouseClick(e);
            if (this._Document == null)
            {
                // 无文档，不处理消息
                return;
            }
            this._Document.MouseCapture = null;
            MyHandleMouseEvent(e, DocumentEventStyles.MouseClick);
        }


        /// <summary>
        /// 处理鼠标双击事件
        /// </summary>
        /// <param name="e">事件参数</param>
        protected override void OnMouseDoubleClick(MouseEventArgs e)
        {
            // 记录最后一次UI事件时刻
            //this._LastUIEventTime = DateTime.Now;
            if (this.IsEnabledMouseEvent() == false)
            {
                // 禁止鼠标事件
                return;
            }
            base.OnMouseDoubleClick(e);
            if (this._Document == null)
            {
                // 无文档，不处理消息
                return;
            }
            bool handled = false;
            int x = e.X;// -this.AutoScrollPosition.X;
            int y = e.Y;// -this.AutoScrollPosition.Y;
            this.CurrentTransformItem = null;
            foreach (SimpleRectangleTransform item in this.PagesTransform)
            {
                DomDocument document = (DomDocument)item.DocumentObject;
                bool contains = item.PartialAreaSourceBounds.Contains(x, y);
                if (contains == false)
                {
                    continue;
                }
                if (item.ContentStyle == PageContentPartyStyle.HeaderRows)
                {
                    // 标题行区域不进行任何处理
                    continue;
                }
                DomDocumentContentElement newContentElement = null;
                switch (item.ContentStyle)
                {
                    case PageContentPartyStyle.Body:
                        newContentElement = document.Body;
                        break;
                    case PageContentPartyStyle.Header:
                        newContentElement = document.Header;
                        this.CurrentTransformItem = item;
                        break;
                    case PageContentPartyStyle.Footer:
                        newContentElement = document.Footer;
                        this.CurrentTransformItem = item;
                        break;
                    case PageContentPartyStyle.HeaderRows:
                        break;
                }
                if (newContentElement != null && newContentElement.Visible == false)
                {
                    // 元素不可见
                    return;
                }
                bool raiseFocus = false;
                DomDocumentContentElement oldContentElement = document.CurrentContentElement;
                if (document.CurrentContentElement != newContentElement)
                {
                    document.CurrentContentElement = newContentElement;
                    raiseFocus = true;
                }
                if (item.Enable == false)
                {
                    // 若命中的区域是不可用的则更新文档状态
                    document.CurrentContentElement.FixElements();
                    this.UpdatePages();////////////////////
                    //this.RefreshScaleTransform();
                    this.Invalidate();
                    var p3 = item.TransformPoint(e.X, e.Y);
                    var e9 = document.Content.GetElementAt(p3.X, p3.Y, false);
                    if (e9 != null && e9.ContentIndex >= 0)
                    {
                        document.Content.SetSelection(e9.ContentIndex, 0);
                    }
                    this.UpdateTextCaret();
                    handled = true;
                }
                if (raiseFocus)
                {
                    document.CurrentContentElement.RaiseFocusEvent(
                        oldContentElement.CurrentElement,
                        newContentElement.CurrentElement);
                }
                break;
            }//foreach
            if (handled == false)
            {
                MyHandleMouseEvent(e, DocumentEventStyles.MouseDblClick);
            }
        }

        /// <summary>
        /// 处理鼠标移动事件
        /// </summary>
        /// <param name="e">事件参数</param>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            // 记录最后一次UI事件时刻
            //this._LastUIEventTime = DateTime.Now;
            if (this.IsEnabledMouseEvent() == false)
            {
                // 禁止鼠标事件
                return;
            }
            base.OnMouseMove(e);
            if (this._Document == null)
            {
                // 没有文档，不处理事件
                return;
            }
            Rectangle dhb = this.DragableHandleBounds;
            if (dhb.IsEmpty == false && dhb.Contains(e.X, e.Y))
            {
                // 表示可以鼠标整体拖拽
                this.Cursor = System.Windows.Forms.Cursors.Arrow;
            }
            else
            {
                //var infos = (InnerPageTitleDrawInfoList)this.GetPageTitleDrawInfos();
                //if (infos.OnMouseMove(this, e.X, e.Y))
                //{
                //    if (this.TooltipControl != null)
                //    {
                //        if (infos.HoverInfo != null && this.__ShowPowerByDCSoftTooltip)
                //        {
                //            //this.TooltipControl.SetToolTip(this, WriterStringsCore.PowerByDCSoft);
                //        }
                //        else
                //        {
                //            //this.TooltipControl.SetToolTip(this, null);
                //        }
                //    }
                //}
                //else
                {
                    MyHandleMouseEvent(e, DocumentEventStyles.MouseMove);
                    bool hasPage = false;
                    foreach (var page in this.Pages)
                    {
                        if (page.ClientBounds.Contains(e.X, e.Y))
                        {
                            hasPage = true;
                            break;
                        }
                    }
                    if (hasPage == false)
                    {
                    }
                }
            }
        }

        /// <summary>
        /// 处理鼠标按键按下事件
        /// </summary>
        /// <param name="e">事件参数</param>
        protected override void OnMouseDown(System.Windows.Forms.MouseEventArgs e)
        {
            if (this.IsEnabledMouseEvent() == false)
            {
                // 禁止鼠标事件
                return;
            }
            this._StartTickForDelayGotFocus = Environment.TickCount;
            base.OnMouseDown(e);
            if (this.IsEditingElementValue)
            {
            }
            if (this._Document != null)
            {
                {
                    bool handled = false;
                    if (handled == false)
                    {
                        Rectangle dhb = this.DragableHandleBounds;
                        if (dhb.IsEmpty == false && dhb.Contains(e.X, e.Y))
                        {
                            DomElement element = this.DragableElement;
                            element.Select();
                            this._Document.InvalidateElementView(element);
                            this.Update();
                        }
                        else
                        {
                            MyHandleMouseEvent(e, DocumentEventStyles.MouseDown);
                        }
                    }
                }
            }//if
            if (this.Focused == false)
            {
                this.Focus();
            }
        }


        /// <summary>
        /// 正在拖拽控件文档内容标记
        /// </summary>
        private DragOperationState _DragState = DragOperationState.None;
        /// <summary>
        /// 编辑器控件正在执行OLE拖拽事件
        /// </summary>
        public DragOperationState DragState
        {
            get
            {
                return _DragState;
            }
        }

        /// <summary>
        /// 处理鼠标事件
        /// </summary>
        /// <param name="e">事件参数</param>
        /// <param name="style">事件类型</param>
        private void MyHandleMouseEvent(MouseEventArgs e, DocumentEventStyles style)
        {
            this._LastCurrentElementForDragEvent = null;
            if (this._WaittingForDragStart && style == DocumentEventStyles.MouseUp)
            {
                // 鼠标拖拽失败，则根据当前鼠标光标来重新设置插入点位置
                this._WaittingForDragStart = false;
                this.RefreshScaleTransform();
                var item3 = this.PagesTransform.GetItemByPoint(
                            e.X,
                            e.Y,
                            true,
                            true,
                            true);
                var p = new Point(e.X, e.Y);
                p = RectangleCommon.MoveInto(p, item3.SourceRect);

                if (p.Y == item3.SourceRect.Bottom)
                {
                    p.Y = item3.SourceRect.Bottom - 2;
                }
                p = item3.TransformPoint(p.X, p.Y);
                DomDocument document = (DomDocument)item3.DocumentObject;
                document.Content.MoveToPoint(p.X, p.Y);
                return;
            }
            if (style == DocumentEventStyles.MouseMove)
            {
                if (e.Button == System.Windows.Forms.MouseButtons.Left)
                {
                    //System.Diagnostics.Debug.WriteLine(e.X + " " + e.Y);
                }
            }
            this._ShowDialogTick = 0;
            this.RefreshScaleTransform();
            if (this.PagesTransform == null)
            {
                return;
            }
                //foreach (SimpleRectangleTransform item2 in this.PagesTransform)
                var innerItems = this.PagesTransform.myItems;
                var len44 = innerItems.Count;
                for (var iCount = 0; iCount < len44; iCount++)
                {
                    var item2 = innerItems[iCount];
                    if (item2.PartialAreaSourceBounds.Contains(e.X, e.Y)
                        && item2.Enable == false)
                    {
                        //if ( DrawerUtil.IsHeaderFooter(  item2.ContentStyle))
                        {
                            // 命中未激活页眉页脚区域，可能用户打算双击激活页眉页脚区域
                            // 因此退出处理。
                            if (style == DocumentEventStyles.MouseMove)
                            {
                                this.HideToolTip();
                            }
                            if (e.Clicks > 0)
                            {
                                return;
                            }
                        }
                    }//if
                }//foreach
            SimpleRectangleTransform item = null;
            if (e.Button == System.Windows.Forms.MouseButtons.None)
            {
                item = this.PagesTransform.GetItemByPoint(
                        e.X,
                        e.Y,
                        true,
                        false,
                        true);
                if (item == null)
                {
                    this.Cursor = Cursors.Default;
                    if (style == DocumentEventStyles.MouseMove)
                    {
                        this.HideToolTip();
                    }
                    return;
                }
            }
            item = this.PagesTransform.GetItemByPoint(
                        e.X,
                        e.Y,
                        true,
                        true,
                        true);
            if (style == DocumentEventStyles.MouseMove && e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                if (this.ClientRectangle.Contains(e.X, e.Y) == false)
                {
                    // 正在鼠标拖拽选择操作
                    if (this.Selection.Length == 0)
                    {
                    }
                    else if (e.Y < this.ClientRectangle.Top)
                    {
                        item = this.PagesTransform.GetItemByPoint(e.X, e.Y, true, GetRectangleItemCompatibilityMode.Up, true);
                    }
                    else if (e.Y > this.ClientRectangle.Bottom)
                    {
                        item = this.PagesTransform.GetItemByPoint(e.X, e.Y, true, GetRectangleItemCompatibilityMode.Down, true);
                    }
                    else if (this.Selection.Length < 0)
                    {
                        // 向上拖拽选择

                        item = this.PagesTransform.GetItemByPoint(e.X, e.Y, true, GetRectangleItemCompatibilityMode.Up, true);
                    }
                    else if (this.Selection.Length > 0)
                    {
                        // 向下拖拽选择
                        item = this.PagesTransform.GetItemByPoint(e.X, e.Y, true, GetRectangleItemCompatibilityMode.Down, true);
                    }
                }
            }
            if (item != null)
            {
                DomDocument document = (DomDocument)item.DocumentObject;
                Point p = new Point(e.X, e.Y);
                p = RectangleCommon.MoveInto(p, item.SourceRect);
                if (p.Y > e.Y
                    && style == DocumentEventStyles.MouseMove
                    && e.Button == System.Windows.Forms.MouseButtons.Left)
                {
                    // 可能正在拖拽选择，则做一个小小的偏移量
                    p.Y++;
                }
                if (p.Y == item.SourceRect.Bottom)
                {
                    p.Y = item.SourceRect.Bottom - 2;
                }
                p = item.TransformPoint(p.X, p.Y);
                DocumentEventArgs args = DocumentEventArgs.CreateMouseDown(
                    document,
                    new MouseEventArgs(e.Button, e.Clicks, p.X, p.Y, e.Delta));
                args.Tooltip = null;
                args.Cursor = null;
                if (this.StyleInfoForFormatBrush != null)
                {
                    args.Cursor = DCSoft.WinForms.XCursors.FormatBrush2;
                }
                args._StrictMatch = item.SourceRect.Contains(e.X, e.Y);
                args.intStyle = style;
                // 调用文档对象的事件处理过程
                if (style == DocumentEventStyles.MouseClick)
                {
                }
                document.HandleDocumentEvent(args);
                this.UpdateToolTip(true, args);
                this.Cursor = args.RuntimeCursor();
                if (args.Handled == false)
                {
                    var canHandle = false;
                    if (style == DocumentEventStyles.MouseDown)
                    {
                        if (e.Button == MouseButtons.Left)
                        {
                            canHandle = true;
                        }
                        else if (e.Button == MouseButtons.Right)
                        {
                            if (this.Document.Selection.Length == 0)
                            {
                                canHandle = true;
                            }
                        }
                    }
                    if (canHandle)
                    {
                        if (e.Clicks == 1)
                        {
                            int selectionVersionBack = document.Selection.StateVersion;
                            bool handled = false;
                            if (this.AllowDragContent
                                && this.Selection.Length != 0)
                            //&& this.RuntimeReadonly() == false)
                            {
                                bool enableDragFlag = true;
                                foreach (DomElement element in this.Selection)
                                {
                                    if (element is DomFieldBorderElement)
                                    {
                                        // 域边界元素不能拖拽
                                        if (((DomFieldElementBase)element.Parent).IsFullSelect == false)
                                        {
                                            //enableDragFlag = false;
                                            break;
                                        }
                                    }
                                    if (element.Parent is DomFieldElementBase)
                                    {
                                        // 背景文字元素不能拖拽
                                        DomFieldElementBase field = (DomFieldElementBase)element.Parent;
                                        if (field.HasElements() == false)
                                        {
                                            if (field.IsBackgroundTextElement(element))
                                            {
                                                if (field.IsFullSelect == false)
                                                {
                                                    //enableDragFlag = false;
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                }//foreach
                                if (enableDragFlag)
                                {
                                    bool bolInSelection = false;
                                    var sele = this.Selection;
                                    if (sele.Cells != null && sele.Cells.Count > 0)
                                    {
                                        foreach (DomTableCellElement cell in sele.Cells)
                                        {
                                            DomTableCellElement cell2 = cell;
                                            if (cell2.IsOverrided)
                                            {
                                                cell2 = cell2.OverrideCell;
                                            }
                                            if (cell2.RuntimeVisible)
                                            {
                                                if (cell2.GetAbsBounds().Contains(p.X, p.Y))
                                                {
                                                    bolInSelection = true;
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                    if (bolInSelection == false && sele.ContentElements != null && sele.ContentElements.Count > 0)
                                    {
                                        foreach (var element in sele.ContentElements)
                                        {
                                            var line = element.OwnerLine;
                                            if (line == null)
                                            {
                                                continue;
                                            }
                                            var rect = new RectangleF(
                                                line.AbsLeft + element.Left,
                                                line.AbsTop,
                                                element.Width + element.WidthFix,
                                                line.Height);
                                            if (rect.Contains(p.X, p.Y))
                                            {
                                                bolInSelection = true;
                                                break;
                                            }

                                        }
                                    }
                                    if (bolInSelection)
                                    {
                                        // 开始拖拽文档内容
                                        this._WaittingForDragStart = true;
                                        return;
                                        //if (DCSoft.WinForms.Native.MouseCapturer.DragDetect(this.Handle))
                                        //{
                                        //    DragSelectionContent();
                                        //    handled = true;
                                        //}
                                        //else
                                        //{
                                        //    if (Control.MouseButtons
                                        //        == System.Windows.Forms.MouseButtons.None)
                                        //    {
                                        //        // 补充触发鼠标点击事件
                                        //        Point p2 = Control.MousePosition;
                                        //        p2 = this.PointToClient(p2);
                                        //        this.OnMouseUp(
                                        //            new MouseEventArgs(
                                        //                MouseButtons.Left,
                                        //                1,
                                        //                p2.X,
                                        //                p2.Y,
                                        //                0));
                                        //        this.OnMouseClick(
                                        //            new MouseEventArgs(
                                        //                MouseButtons.Left,
                                        //                1,
                                        //                p2.X,
                                        //                p2.Y,
                                        //                0));
                                        //    }
                                        //}
                                    }//if
                                }
                            }
                            if (handled == false && args.AlreadSetSelection == false)
                            {
                                if (document.Selection.StateVersion == selectionVersionBack)
                                {
                                    // 期间没有修改选择区域，则设置当前插入点
                                    document.MouseCapture = new MouseCaptureInfo(args);
                                    document.Content.AutoClearSelection = !args.ShiftKey;
                                    //float tick2 = CountDown.GetTickCountFloat();
                                    document.Content.MoveToPoint(args.X, args.Y);
                                    //tick2 = CountDown.GetTickCountFloat() - tick2;
                                    //tick2 = CountDown.GetTickCountFloat();
                                    //this.UpdateTextCaret();
                                    //tick2 = CountDown.GetTickCountFloat() - tick2;
                                }
                                this.UseAbsTransformPoint = true;
                            }
                        }
                    }
                    else if (style == DocumentEventStyles.MouseDblClick)
                    {
                        DomElement element = document.Content.GetElementAt(p.X, p.Y, true);
                        if (element != null)
                        {
                            if (item.SourceRect.Contains(e.X, e.Y))
                            {
                                if (this.Capture)
                                {
                                    this.Capture = false;
                                }
                                if (this._ShowDialogTick <= 0)
                                {
                                    // 若鼠标按键按下则试图编辑鼠标所在的元素值
                                    if (this.BeginEditElementValue(
                                        element,
                                        false,
                                        ValueEditorActiveMode.MouseDblClick,
                                        true,
                                        true))
                                    {
                                        //handled = true;
                                    }
                                    else if (this.DocumentOptions.BehaviorOptions.DoubleClickToSelectWord)
                                    {
                                        DomElement e2 = GetElementByPosition(e.X, e.Y);
                                        if (e2 != null && e2.RuntimeSelectable() == false)
                                        {
                                            return;
                                        }
                                        if (e2 != null && e2.Parent is DomInputFieldElementBase)
                                        {
                                            // 双击背景文本，选择整个输入域，包括边界元素s
                                            DomInputFieldElementBase field = (DomInputFieldElementBase)e2.Parent;
                                            if (field.IsBackgroundTextElement(e2))
                                            {
                                                return;
                                            }
                                            else
                                            {
                                                // 双击普通文本，试图选择整个输入域的内容，不包含边界元素。
                                                var vContent = this._Document.Content;
                                                var startIndex9 = vContent.FastIndexOf(field.StartElement);
                                                var endIndex9 = vContent.FastIndexOf(field.EndElement);
                                                if (startIndex9 >= 0 && endIndex9 > startIndex9 && endIndex9 < startIndex9 + 300)
                                                {
                                                    var bolSelectFieldContent = true;
                                                    for (var iCount = startIndex9 + 1; iCount < endIndex9; iCount++)
                                                    {
                                                        var e88 = vContent[iCount];
                                                        if (e88 is DomCharElement
                                                            || e88 is DomParagraphFlagElement
                                                            || e88 is DomObjectElement)
                                                        {
                                                        }
                                                        else
                                                        {
                                                            bolSelectFieldContent = false;
                                                            break;
                                                        }
                                                    }
                                                    if (bolSelectFieldContent)
                                                    {
                                                        // 双击选择纯文本内容的较小的输入域内容
                                                        vContent.DocumentContentElement.SetSelection(startIndex9 + 1, endIndex9 - startIndex9 - 1);
                                                        return;
                                                    }
                                                }
                                            }
                                        }
                                        // 鼠标双击事件
                                        this._Document.Content.SelectWord();
                                    }
                                }
                            }
                        }
                    }
                    else if (style == DocumentEventStyles.MouseClick)
                    {
                        DomElement element = document.Content.GetElementAt(p.X, p.Y, true);
                        if (element != null)
                        {
                            if (item.SourceRect.Contains(e.X, e.Y))
                            {
                                if (this.Capture)
                                {
                                    this.Capture = false;
                                }
                                // 若鼠标按键按下则试图编辑鼠标所在的元素值
                                ValueEditorActiveMode veam = ValueEditorActiveMode.None;
                                if (e.Button == System.Windows.Forms.MouseButtons.Left)
                                {
                                    veam = ValueEditorActiveMode.MouseClick;
                                }
                                else if (e.Button == System.Windows.Forms.MouseButtons.Right)
                                {
                                    veam = ValueEditorActiveMode.MouseRightClick;
                                }
                                else
                                {
                                    veam = ValueEditorActiveMode.MouseClick;
                                }
                                if (this._ShowDialogTick <= 0)
                                {
                                    if (element.DocumentContentElement.Selection.Length == 0)
                                    {
                                        if (this.BeginEditElementValue(
                                            element,
                                            false,
                                            veam,
                                            true,
                                            true))
                                        {
                                            //handled = true;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else if (style == DocumentEventStyles.MouseMove)
                    {
                        //_Tooltip.SetToolTip(this, args.Tooltip);
                        this.Cursor = args.RuntimeCursor();
                    }
                    else if (style == DocumentEventStyles.MouseUp)
                    {

                    }
                }
            }
        }
        /// <summary>
        /// 判断客户区中指定的点是否在被选择区域之中
        /// </summary>
        /// <param name="clientX"></param>
        /// <param name="clientY"></param>
        /// <returns></returns>
        internal bool IsInSelectionArea(int clientX, int clientY)
        {
            var sele = this.Selection;
            if (sele == null || sele.Length == 0)
            {
                // 没有内容被选择
                return false;
            }
            this.RefreshScaleTransform();
            var item = this.PagesTransform.GetItemByPoint(
                        clientX,
                        clientY,
                        true,
                        true,
                        true);
            if (item == null)
            {
                return false;
            }
            var p = new Point(clientX, clientY);
            p = RectangleCommon.MoveInto(p, item.SourceRect);
            if (p.Y > clientY)
            {
                p.Y++;
            }
            if (p.Y == item.SourceRect.Bottom)
            {
                p.Y = item.SourceRect.Bottom - 2;
            }
            p = item.TransformPoint(p.X, p.Y);
            if (sele.Cells != null && sele.Cells.Count > 0)
            {
                foreach (DomTableCellElement cell in sele.Cells)
                {
                    DomTableCellElement cell2 = cell;
                    if (cell2.IsOverrided)
                    {
                        cell2 = cell2.OverrideCell;
                    }
                    if (cell2.RuntimeVisible)
                    {
                        if (cell2.GetAbsBounds().Contains(p.X, p.Y))
                        {
                            return true;
                        }
                    }
                }
            }
            if (sele.ContentElements != null && sele.ContentElements.Count > 0)
            {
                foreach (var element in sele.ContentElements)
                {
                    var line = element.OwnerLine;
                    var rect = new RectangleF(
                        line.AbsLeft + element.Left,
                        line.AbsTop,
                        element.Width + element.WidthFix,
                        line.Height);
                    if (rect.Contains(p.X, p.Y))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        /// <summary>
        /// 正在等待开始内容拖拽操作
        /// </summary>
        internal bool _WaittingForDragStart = false;

        /// <summary>
        /// 拖拽被选择的文档内容
        /// </summary>
        internal IDataObject DragSelectionContent(ref DragDropEffects allowEff)
        {
            this._WaittingForDragStart = false;
            this.Cursor = Cursors.Default;
            var bopts = this.DocumentOptions.BehaviorOptions;
            if (bopts.MoveFieldWhenDragWholeContent)
            {
                DomInputFieldElementBase field = (DomInputFieldElementBase)this.GetCurrentElement(
                    typeof(DomInputFieldElementBase));
                if (field != null)
                {
                    DomDocumentContentElement dce = field.DocumentContentElement;
                    int startIndex = dce.Content.IndexOf(field.StartElement);
                    int endIndex = dce.Content.IndexOf(field.EndElement);
                    int selStartIndex = dce.Selection.AbsStartIndex;
                    int selEndIndex = dce.Selection.AbsEndIndex;
                    if (startIndex == selStartIndex || startIndex == selStartIndex - 1)
                    {
                        if (endIndex == selEndIndex || selEndIndex == endIndex + 1)// endIndex == selEndIndex + 1)
                        {
                            dce.SetSelection(startIndex, endIndex - startIndex + 1);
                        }
                    }
                }
            }
            System.Windows.Forms.IDataObject obj = this.DocumentControler.CreateSelectionDataObject(
                this.CreationDataFormats,
                false,
                this.DocumentOptions.EditOptions.ClearFieldValueWhenCopy,
                this.DocumentOptions.EditOptions.CopyWithoutInputFieldStructure);
            if (obj == null)
            {
                // 没有内容可拖拽
                return null;
            }
            obj.SetData(WriterConst.Name_DragContentFlag, "1");
            allowEff = DragDropEffects.Copy;
            if (this.Readonly == false && this.DocumentControler.CanDeleteSelection == true)
            {
                allowEff = allowEff | DragDropEffects.Move;
            }
            //DragDropEffects effect = DragDropEffects.None;
            this._DragState = DragOperationState.DragingSelection;
            return obj;
        }


        /// <summary>
        /// 处理鼠标按键松开事件
        /// </summary>
        /// <param name="e">参数</param>
        protected override void OnMouseUp(System.Windows.Forms.MouseEventArgs e)
        {
            // 记录最后一次UI事件时刻
            //this._LastUIEventTime = DateTime.Now;
            if (this.IsEnabledMouseEvent() == false)
            {
                // 禁止鼠标事件
                return;
            }
            base.OnMouseUp(e);
            if (this._Document != null)
            {
                this._Document.MouseCapture = null;
            }
            if (this.Capture)
            {
                this.Capture = false;
            }
            if (this._Document == null)
            {
                // 无文档，不处理事件
                return;
            }
            if (this.IsEditingElementValue == false)
            {
                if (e.Button == System.Windows.Forms.MouseButtons.Left)
                {
                    int click = MouseClickCounter.Instance.Count(e.X, e.Y);
                    if (click == 1)
                    {
                        // 鼠标单击事件
                        if (this.StyleInfoForFormatBrush != null)
                        {
                            // 执行格式刷的后期执行操作
                            this.ApplyFormatBrush();
                            return;
                        }
                        if (this._WaittingForDragStart)
                        {
                            // 鼠标拖拽事件失败，则重新移动插入点
                            this.MyHandleMouseEvent(e, DocumentEventStyles.MouseUp);
                            this._WaittingForDragStart = false;
                        }
                    }
                    if (click == 2)
                    {

                    }
                    else if (click == 3)
                    {
                        DomElement e2 = GetElementByPosition(e.X, e.Y);
                        if (e2 != null && e2.RuntimeSelectable() == false)
                        {
                            return;
                        }
                        if (this.DocumentOptions.BehaviorOptions.ThreeClickToSelectParagraph)
                        {
                            // 检测鼠标三击事件
                            this._Document.Content.SelectParagraph();
                        }
                    }
                }
                else
                {
                    this.MyHandleMouseEvent(e, DocumentEventStyles.MouseUp);
                }

            }
        }
        protected override void OnMouseEnter(EventArgs e)
        {
            if (this.IsEnabledMouseEvent() == false)
            {
                return;
            }
            base.OnMouseEnter(e);
        }
        /// <summary>
        /// 处理鼠标光标离开事件
        /// </summary>
        /// <param name="e">参数</param>
        protected override void OnMouseLeave(EventArgs e)
        {
            // 记录最后一次UI事件时刻
            //this._LastUIEventTime = DateTime.Now;
            if (this.IsEnabledMouseEvent() == false)
            {
                // 禁止鼠标事件
                return;
            }
            base.OnMouseLeave(e);
            {
                if (this._Document != null)
                {
                    DocumentEventArgs args = new DocumentEventArgs(
                        this._Document,
                        this._Document,
                        DocumentEventStyles.MouseLeave);
                    args.Cursor = null;
                    this._Document.HandleDocumentEvent(args);
                }
            }
            this.Cursor = Cursors.Default;
            this.HideToolTip();
        }

    }
}
