
using System;
using System.Windows.Forms;
using System.Collections.Generic;
using DCSoft.Printing;
// // 
using System.Text;
using DCSoft.WinForms;
using System.ComponentModel;
using DCSoft.Common;
using DCSoft.Drawing;
using DCSoft.Writer.Dom;
using System.Runtime.InteropServices;

namespace DCSoft.Writer.Controls
{
    partial class WriterViewControl
    {
        public override bool Focus()
        {
            this.UpdateTextCaretWithoutScroll();
            return base.Focus();
        }
        
        public void CheckForLoadDefaultDocument()
        {
                if (this._Document == null || this._Document.PageRefreshed == false)
                {
                    if (this._Document != null)
                    {
                        this.Document.FixDomState();
                    }
                    this.RefreshDocument();
                }
        }

        public float WASMZoomRate
        {
            get
            {
                return this.OwnerWriterControl.WASMZoomRate;
            }
        }

        public override void OnCurrentPageChanged()
        {
            base.OnCurrentPageChanged();
            if( this.OwnerWriterControl.ctlHRule != null )
            {
                this.OwnerWriterControl.ctlHRule.ViewControl_CurrentPageChanged(null, null);
            }
            if( this.OwnerWriterControl.ctlVRule != null )
            {
                this.OwnerWriterControl.ctlVRule.ViewControl_CurrentPageChanged(null, null);
            }
        }
        public override void ReversibleViewDrawLine(int x1, int y1, int x2, int y2)
        {
            this.InnerReversibleDraw(x1, y1, x2, y2, DCSoft.WinForms.Native.ReversibleShapeStyle.Line);
        }
        internal void InnerReversibleDraw(
            int x1,
            int y1,
            int x2,
            int y2,
            DCSoft.WinForms.Native.ReversibleShapeStyle type)
        {
            this.RefreshScaleTransform();
            DCSystem_Drawing.Point p1 = this.MouseCaptureTransform.UnTransformPoint(x1, y1);
            DCSystem_Drawing.Point p2 = this.MouseCaptureTransform.UnTransformPoint(x2, y2);
            var rect = RectangleCommon.GetRectangle(p1, p2);
            rect.Width += 1;
            rect.Height += 1;
            for (var iCount = 0; iCount < this.Pages.Count; iCount++)
            {
                var page = this.Pages[iCount];

                if (page.ClientBounds.IntersectsWith( rect ))
                {
                    this.WASMParent.ReversibleDraw(
                        iCount,
                        p1.X - page.ClientBounds.Left,
                        p1.Y - page.ClientBounds.Top,
                        p2.X - page.ClientBounds.Left,
                        p2.Y - page.ClientBounds.Top,
                        (int)type);
                    break;
                }
            }
        }
        public DCSoft.WASM.WriterControlForWASM WASMParent
        {
            get
            {
                return this.OwnerWriterControl.WASMParent;
            }
        }

        private DCSystem_Drawing.Rectangle GetPageContentBounds(PrintPage page, bool onlyFixHeight = false)
        {
            var info = new PageLayoutInfo(page);
            info.ConvertUnit(GraphicsUnit.Pixel);
            var left = info.LeftMargin;
            var top = info.HeaderDistance;
            var right = info.PageWidth - info.RightMargin;
            right = Math.Max(right, left + this._Document.Body.PixelWidth);
            var bottom = info.PageHeight - info.FooterDistance;
            if (onlyFixHeight)
            {
                return new DCSystem_Drawing.Rectangle(0, 0, (int)info.PageWidth, (int)bottom);
            }
            else
            {
                return new DCSystem_Drawing.Rectangle(
                    (int)(left - 2),
                    (int)(top - 2),
                    (int)((right - left) + 4),
                    (int)((bottom - top) + 4));
            }
        }

        private bool _ReadyForWASM = false;
        internal void StartForWASM()
        {
            this._WASMInvalidateAll = false;
            this._ReadyForWASM = true;
            this.myTransform = new DocumentViewTransform();
        }
         
        /// <summary>
        /// 打印时的放大比率。
        /// </summary>
        public const float ZoomRateForPrint = 1;
        internal PrintPageCollection GetRuntimePrintPages(
            PrintPageCollection allPages,
            DCSoft.WASM.MyPrintTaskOptions options,
            DomDocumentList documents, 
            bool forPrintPreview)
        {
            PrintPageCollection resultList = allPages;
            //List<PrintPage> pages = this.VisiblePages;
            if (allPages == null || allPages.Count == 0)
            {
                return null;
            }
            if (options != null)
            {
                // 处理自定义打印页码设置
                var indexs = options.GetRuntimePageIndexs(
                    allPages.Count, 
                    this.CurrentPageIndex,
                    documents);
                resultList = new PrintPageCollection();
                foreach (var index in indexs)
                {
                    if (index >= 0 && index < allPages.Count)
                    {
                        resultList.Add(allPages[index]);
                    }
                }
            }
            return resultList;
        }

        private void WritePageBorderBackgroundStyle(DomDocument document, StringBuilder strCode)
        {
            strCode.Append("\"border-radius:5px;margin:5px 5px 5px 5px;vertical-align:top;background-color:white;border:1px solid black\",");
        }

        internal string GetPageSizeCodes(PrintPageCollection pages, bool forPrintPreview = true)
        {
            if (this._ReadyForWASM == false)
            {
                return null;
            }
            var strCode = new StringBuilder();
            strCode.Append('[');
            WritePageBorderBackgroundStyle(this.Document, strCode);
            if (pages == null)
            {
                pages = this.Pages;
            }

            //if (this.WASMParent != null && this._WASMPrintMode == false)
            {
                strCode.Append(this.WASMZoomRate);
            }
            if (pages != null && pages.Count > 0)
            {
                    int intDefaultWidth = pages[0].ClientBounds.Width;
                    int intDefaultHeight = pages[0].ClientBounds.Height;
                    string strDefaultWidth = intDefaultWidth.ToString();
                    string strDefaultHeight = intDefaultHeight.ToString();
                    foreach (var page in pages)
                    {
                        //var layInfo = new PageLayoutInfo(page);
                        strCode.Append(",{\"PageIndex\":").Append(pages.IndexOf(page));
                        if (page.ClientBounds.Width == intDefaultWidth)
                        {
                            strCode.Append(",\"Width\":").Append(strDefaultWidth);
                        }
                        else
                        {
                            strCode.Append(",\"Width\":").Append(page.ClientBounds.Width);
                        }
                        if (page.ClientBounds.Height == intDefaultHeight)
                        {
                            strCode.Append(",\"Height\":").Append(strDefaultHeight);
                        }
                        else
                        {
                            strCode.Append(",\"Height\":").Append(page.ClientBounds.Height);
                        }
                        var doc = (DomDocument)page.Document;
                        {
                            strCode.Append(",\"Background\":false");
                        }
                        strCode.Append('}');
                    }
            }
            strCode.Append(']');
            return strCode.ToString();
        }
        public override bool Focused
        {
            get
            {
                return true;
                //return Convert.ToBoolean( WASMEnvironment.JSProvider.JSRunMethod(
                //    WASMEnvironment.JSNAME_WriterControl_UI_IsFocused,
                //    this.Name));
            }
        }
        
        internal class WASMClientPosInfo
        {
            public int PageIndex = 0;
            public PrintPage Page = null;
            public int dx = 0;
            public int dy = 0;
            public int Width = 0;
            public int Height = 0;
        }
        internal WASMClientPosInfo GetClientPosInfo(int viewX, int viewY, int width = 0, int height = 0, bool useGlobalPos = false)
        {
                DCSystem_Drawing.Point p;
                if (useGlobalPos)
                {
                    p = ((MultiPageTransform)this.Transform).UnTransformPointBodyDirect(viewX, viewY);
                }
                else
                {
                    p = this.ViewPointToClient(viewX, viewY);
                }
                var pages = this.Pages;
                foreach (var page in pages)
                {
                    if (page.ClientBounds.Contains(p))
                    {
                        var result = new WASMClientPosInfo();
                        result.Page = page;
                        result.PageIndex = pages.IndexOf(page);
                        result.dx = (int)((p.X - page.ClientBounds.Left) * this.WASMZoomRate);
                        result.dy = (int)((p.Y - page.ClientBounds.Top) * this.WASMZoomRate);
                        if (width > 0 && height > 0)
                        {
                            var size4 = this.Transform.UnTransformSize(width, height);
                            result.Width = (int)(size4.Width * this.WASMZoomRate);
                            result.Height = (int)(size4.Height * this.WASMZoomRate);
                        }
                        return result;
                    }
                }
            return null;
        }
        internal WASMClientPosInfo GetClientPosInfo(DomElement element)
        {
            if(element == null )
            {
                return null;
            }
                var pages = this.Pages;
                var absLeft = element.GetAbsLeft();
                var absTop = element.GetAbsTop();
                if (element.DocumentContentElement is DomDocumentBodyElement)
                {
                    var p = this.ViewPointToClient((int)absLeft, (int)absTop);
                    foreach (var page in pages)
                    {
                        if (page.ClientBounds.Contains(p))
                        {
                            var result = new WASMClientPosInfo();
                            result.Page = page;
                            result.PageIndex = pages.IndexOf(page);
                            result.dx = (int)((p.X - page.ClientBounds.Left) * this.WASMZoomRate);
                            result.dy = (int)((p.Y - page.ClientBounds.Top) * this.WASMZoomRate);
                            result.Width = (int)(element.PixelWidth * this.WASMZoomRate);
                            result.Height = (int)(element.PixelHeight * this.WASMZoomRate);
                            return result;
                        }
                    }
                    return null;
                }
                else
                {
                    var result = new WASMClientPosInfo();
                    var page = this.CurrentPage;
                    if (page == null)
                    {
                        page = pages[0];
                    }
                    result.PageIndex = pages.IndexOf(page);
                    result.Page = page;
                    var cs = element.OwnerPagePartyStyle;
                    foreach (SimpleRectangleTransform item in this.PagesTransform)
                    {
                        if (item.PageObject == page && item.ContentStyle == cs)
                        {
                            var p2 = item.UnTransformPointF(absLeft, absTop);
                            result.dx = (int)((p2.X - page.ClientBounds.Left) * this.WASMZoomRate);
                            result.dy = (int)((p2.Y - page.ClientBounds.Top) * this.WASMZoomRate);
                        }
                    }
                    return result;
                }
            return null;
        }

        public string GetPageClientSelectionBounds( int pageIndex )
        {
            var pages = this.Pages;
            if (pageIndex >= 0 && pageIndex < pages.Count)
            {
                var page = pages[pageIndex];
                var rect = page.ClientSelectionBounds;
                if (rect.IsEmpty == false)
                {
                    var zoomRate = this.WASMZoomRate;
                    return StringCommon.ToInt32String((rect.Left - page.ClientBounds.Left) * zoomRate)
                        + "," + StringCommon.ToInt32String((rect.Top - page.ClientBounds.Top) * zoomRate)
                        + "," + StringCommon.ToInt32String(rect.Width * zoomRate)
                        + "," + StringCommon.ToInt32String(rect.Height * zoomRate);
                }
            }
            return null;
        }
        public byte[] WASMPaintPage(int pageIndex)
        {
            var pages = this.Pages;
            if (pageIndex < 0 || pageIndex >= pages.Count)
            {
                return null;
            }
                this._Document.States.Printing = false;
                this._Document.States.PrintPreviewing = false;
            var page = pages[pageIndex];
            //lock (_InvalidateLock)
            {
                // 删除该页相关的无效视图状态信息
                if (this._InvalidateInfos != null)
                {
                    for (var iCount = this._InvalidateInfos.Count - 1; iCount >= 0; iCount--)
                    {
                        if (this._InvalidateInfos[iCount].Page == page)
                        {
                            this._InvalidateInfos.RemoveAt(iCount);
                        }
                    }
                }
            }
            //lock (_PaintPageLock)
            {
                this._WASMPrintMode = false;
                var g = new  Graphics();
                g.ZoomRate = this.WASMZoomRate * this.OwnerWriterControl.WASMBaseZoomRate;
                g.SetAbsoluteOffset(-page.ClientBounds.Left, -page.ClientBounds.Top);
                var args = new System.Windows.Forms.PaintEventArgs(g, page.ClientBounds);
                this._EnabledViewInvalidate = false;
                try
                {
                    this._LastClientSelectionBounds = DCSystem_Drawing.Rectangle.Empty;
                    this.OnPaint(args);
                    page.ClientSelectionBounds = this._LastClientSelectionBounds;
                }
                finally
                {
                    this._EnabledViewInvalidate = true;
                }
                var strFlagString = this.OwnerWriterControl.InnerFlagString();
                if( strFlagString != null && strFlagString.Length > 0 )
                {
                    g.PageUnit = GraphicsUnit.Pixel;
                    var format = new StringFormat();
                    format.Alignment = StringAlignment.Far;
                    format.LineAlignment = StringAlignment.Near;
                    g.DrawString(
                        strFlagString,
                        _InnerVersionFont,
                        Brushes.Blue,
                        new DCSystem_Drawing.RectangleF(page.ClientBounds.Left, page.ClientBounds.Top + 5, page.ClientBounds.Width - 50 , 100),
                        format);
                    format.Dispose();
                }
                g.PageUnit = GraphicsUnit.Document;
                var strCode = g.ToByteArray();
                g.Dispose();
                return strCode;
            }
        }
        private static readonly Font _InnerVersionFont = new Font(XFontValue.DefaultFontName, 13, FontStyle.Bold);
        internal int[] _OldPageSizeList = null;
        private void WASMCheckPageSizesChanged()
        {
            var pages = this.Pages;
            bool changed = false;
            bool raiseEvent = false;
            if (_OldPageSizeList == null)
            {
                changed = true;
                raiseEvent = true;
            }
            else if( _OldPageSizeList.Length != pages.Count * 2 )
            {
                changed = true;
                raiseEvent = true;
            }
            else
            {
                for(int iCount = 0;iCount < pages.Count;iCount ++)
                {
                    if(_OldPageSizeList[iCount *2 ] != pages[iCount].ClientBounds.Width 
                        || _OldPageSizeList[iCount* 2+1] != pages[iCount].ClientBounds.Height )
                    {
                        changed = true;
                        raiseEvent = true;
                        break;
                    }
                }
            }
            if( changed)
            { 
                _OldPageSizeList = new int[pages.Count * 2];
                for (int iCount = 0; iCount < pages.Count; iCount++)
                {
                    _OldPageSizeList[iCount * 2] = pages[iCount].ClientBounds.Width;
                    _OldPageSizeList[iCount * 2 + 1] = pages[iCount].ClientBounds.Height;
                }
            }
            if(raiseEvent &&  this.WASMParent != null )
            {
                this.OwnerWriterControl.JS_UpdatePages(this.GetPageSizeCodes(null,false));
            }
        }
        private void WASMInvalidate( PrintPage page , DCSystem_Drawing.Rectangle bounds )
        {
            if( this._WASMInvalidateAll )
            {
                return;
            }
            if( page != null && bounds.Width >0 && bounds.Height > 0 )
            {
                {
                    if( this._InvalidateInfos == null )
                    {
                        this._InvalidateInfos = new List<WASMInvalidateInfo>();
                    }
                    bool needAddItem = true;
                    for( var iCount = 0;iCount < this._InvalidateInfos.Count;iCount ++ )
                    {
                        var item = this._InvalidateInfos[iCount];
                        if( item.Page == page )
                        {
                            if( item.Bounds.Contains( bounds ))
                            {
                                // 被合并了，无需后续处理
                                needAddItem = false;
                                break;
                            }
                            if( bounds.Contains( item.Bounds ))
                            {
                                // 合并了已有项目
                                item.Bounds = bounds;
                                needAddItem = false;
                                break;
                            }
                        }
                    }//for
                    if( needAddItem  )
                    {
                        for( var iCount = this._InvalidateInfos.Count -1;iCount >= 0;iCount --)
                        {
                            var item = this._InvalidateInfos[iCount];
                            if( item.Page == page )
                            {
                                var rect = DCSystem_Drawing.Rectangle.Union(item.Bounds, bounds);
                                var size1 = item.Bounds.Width * item.Bounds.Height
                                    + bounds.Width * bounds.Height;
                                var size2 = rect.Width * rect.Height;
                                if( size2 < size1 * 1.5f)
                                {
                                    // 尝试进行合并
                                    item.Bounds = rect;
                                    needAddItem = false;
                                    break;
                                }
                            }
                        }
                    }
                    if( needAddItem )
                    {
                        var info = new WASMInvalidateInfo();
                        info.Page = page;
                        info.PageIndex = this.Pages.IndexOf(page);
                        info.Bounds = bounds;
                        this._InvalidateInfos.Add(info);
                    }
                }//lock
                if( this.WASMParent != null )
                {
                    this.WASMParent.NeedUpdateView();
                }
            }
        }

        private List<WASMInvalidateInfo> _InvalidateInfos = new List<WASMInvalidateInfo>();
        private class WASMInvalidateInfo
        {
            public WASMInvalidateInfo()
            {

            }
            public PrintPage Page = null;
            public int PageIndex = -1;
            public DCSystem_Drawing.Rectangle Bounds = DCSystem_Drawing.Rectangle.Empty;
        }
        private bool _WASMInvalidateAll = false;
        public override void Invalidate()
        {
            if (this._ReadyForWASM == false)
            {
                return;
            }
            this._WASMInvalidateAll = true;
            {
                if (this._InvalidateInfos != null)
                {
                    this._InvalidateInfos.Clear();
                }
            }
            if (this.WASMParent != null)
            {
                this.WASMParent.NeedUpdateView();
            }
        }
        /// <summary>
        /// 判断指定页是否存在需要重新绘制的内容
        /// </summary>
        /// <param name="intPageIndex">页码</param>
        /// <returns>是否需要重新绘制部分内容</returns>
        public bool HasInvalidateView( )
        {
            if( this._WASMInvalidateAll )
            {
                return true;
            }
            return this._InvalidateInfos != null && this._InvalidateInfos.Count > 0;
        }
        public byte[] GetInvalidateViewData(string strPageIndexs)
        {
            if (this._WASMInvalidateAll)
            {
                this._WASMInvalidateAll = false;
                return _AllArray;
            }
            if (string.IsNullOrEmpty(strPageIndexs))
            {
                return null;
            }
            WASMInvalidateInfo currentInfo = null;
            {
                if( this._InvalidateInfos == null || this._InvalidateInfos.Count == 0 )
                {
                    return null;
                }
                this._WASMPrintMode = false;
                for (var iCount = 0; iCount < this._InvalidateInfos.Count; iCount++)
                {
                    var item = this._InvalidateInfos[iCount];
                    if (item.Page == this.CurrentPage)
                    {
                        // 首先优先获得当前页的绘图信息
                        currentInfo = item;
                        this._InvalidateInfos.RemoveAt(iCount);
                        break;
                    }
                }
                if (currentInfo == null)
                {
                    // 获得所有可见页面的绘图信息
                    var pages = this.Pages;
                    foreach (var strItem in strPageIndexs.Split(','))
                    {
                        var pi = 0;
                        if (int.TryParse(strItem, out pi) && pi >= 0 && pi < pages.Count)
                        {
                            for (var iCount = 0; iCount < this._InvalidateInfos.Count; iCount++)
                            {
                                var item = this._InvalidateInfos[iCount];
                                if (item.PageIndex == pi )
                                {
                                    currentInfo = item;
                                    this._InvalidateInfos.RemoveAt(iCount);
                                    break;
                                }
                            }
                            if (currentInfo != null)
                            {
                                break;
                            }
                        }
                    }
                }
            }
            if (currentInfo == null)
            {
                // 未找到需要的区域信息
                return null;
            }
            var strCode = new  MyBinaryBuilder();
            // 还需要绘制后续内容
            strCode.AppendBoolean(this._InvalidateInfos != null && this._InvalidateInfos.Count > 0);
            // 页码
            var pi34 = (short)this.Pages.IndexOf(currentInfo.Page);
            strCode.AppendInt16(pi34);
            var zoomRate = this.WASMZoomRate * this.OwnerWriterControl.WASMBaseZoomRate ;// this.WASMParent.ZoomRate;
            var g = new Graphics();
            g.ZoomRate = zoomRate;
            strCode.AppendBoolean(this.OwnerWriterControl.WASMDoubleBufferForPaint);
            g.SetAbsoluteOffset(-currentInfo.Page.ClientBounds.Left, -currentInfo.Page.ClientBounds.Top);
            var args = new System.Windows.Forms.PaintEventArgs(g, currentInfo.Bounds);
            strCode.AppendInt32((int)((currentInfo.Bounds.Left - currentInfo.Page.ClientBounds.Left) * zoomRate));
            strCode.AppendInt32((int)((currentInfo.Bounds.Top - currentInfo.Page.ClientBounds.Top) * zoomRate));
            strCode.AppendInt32((int)(currentInfo.Bounds.Width * zoomRate));
            strCode.AppendInt32((int)(currentInfo.Bounds.Height * zoomRate));
            this._LastClientSelectionBounds = DCSystem_Drawing.Rectangle.Empty;
            this.OnPaint(args);
            currentInfo.Page.AddClientSelectionBounds(this._LastClientSelectionBounds);
            g.PageUnit = GraphicsUnit.Document;
            g.WriteTo(strCode);
            g.Dispose();
            var strResult = strCode.ToByteArray();
            strCode.Close();
            return strResult;
        }

        private static readonly byte[] _AllArray = new byte[] { 0xfe };

        /// <summary>
        /// 当前使用的鼠标拖拽处理对象
        /// </summary>
        internal DCSoft.WinForms.Native.MouseCapturer _CurrentMouseCapturer = null;
        
        public void MyRaiseMouseDown(MouseEventArgs args)
        {
            _StaticMousePosition = new DCSystem_Drawing.Point(args.X, args.Y);
            _StaticMouseButtons = args.Button;
            this.OnMouseDown(args);
            if(this._CurrentMouseCapturer != null )
            {
                this._CurrentMouseCapturer.HandleMouseDown(args);
            }
        }
        public void MyRaiseMouseMove(MouseEventArgs args)
        {
            _StaticMousePosition = new DCSystem_Drawing.Point(args.X, args.Y);
            _StaticMouseButtons = args.Button;
            if (this._CurrentMouseCapturer != null)
            {
                if (this._CurrentMouseCapturer.HandleMouseMove(args))
                {
                    return;
                }
            }
            else
            {
                this.OnMouseMove(args);
            }
        }
        public void MyRaiseMouseUp(MouseEventArgs args)
        {
            _StaticMousePosition = new DCSystem_Drawing.Point(args.X, args.Y);
            _StaticMouseButtons = args.Button;
            if (this._CurrentMouseCapturer != null)
            {
                this._CurrentMouseCapturer.HandleMouseUp(args);
                this._CurrentMouseCapturer = null;
                this.WASMParent.CloseReversibleShape();
            }
            else
            {
                this.OnMouseUp(args);
            }
        }
        public void MyRaiseMouseClick(MouseEventArgs args)
        {
            _StaticMousePosition = new DCSystem_Drawing.Point(args.X, args.Y);
            _StaticMouseButtons = args.Button;
            this.OnMouseClick(args);
        }
        public void MyRaiseMouseDoubleClick(MouseEventArgs args)
        {
            _StaticMousePosition = new DCSystem_Drawing.Point(args.X, args.Y);
            _StaticMouseButtons = args.Button;
            this.OnMouseDoubleClick(args);
        }
        public void MyRaiseKeyDown(KeyEventArgs args)
        {
            this.OnKeyDown(args);
        }
        public void MyRaiseKeyPress(KeyPressEventArgs args)
        {
            this.OnKeyPress(args);
        }
        public void MyRaiseKeyUp(KeyEventArgs args)
        {
            this.OnKeyUp(args);
        }
        public void MyRaiseDragDrop(DragEventArgs drgevent)
        {
            this.OnDragDrop(drgevent);
        }
        public void MyRaiseDragEnter(DragEventArgs drgevent)
        {
            this.OnDragEnter(drgevent);
        }
        public void MyRaiseDragLeave(EventArgs e)
        {
            this.OnDragLeave(e);
        }
        public void MyRaiseDragOver(System.Windows.Forms.DragEventArgs drgevent)
        {
            this.OnDragOver(drgevent);
        }
    }
}

