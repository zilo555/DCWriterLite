using System;
using System.Collections.Generic;
using System.Text;
// // 
using DCSoft.Drawing;
using DCSoft.WinForms;
using DCSoft.Printing;
using DCSoft.Writer.Dom;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using DCSoft.Common;
using DCSoft.WinForms.Native;
//using  Drawing2D;

namespace DCSoft.Writer.Controls
{
    /// <summary>
    /// 编辑器中绘制用户界面的操作
    /// </summary>
    public partial class WriterViewControl
    {
        /// <summary>
        /// 处理绘制用户界面事件
        /// </summary>
        /// <param name="e">事件参数</param>
        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        {
            WriterViewControl_InnerOnPaint(new InnerPaintEventArgs(e.Graphics, e.ClipRectangle));
        }
        private void WriterViewControl_InnerOnPaint( InnerPaintEventArgs e )
        {
            try
            {
                {
                    this._PageMarginLineLength = this.DocumentOptions.ViewOptions.PageMarginLineLength;
                    if (this._Document == null)
                    {
                        // 绘制无文档字样
                        DCSystem_Drawing.RectangleF rect = new DCSystem_Drawing.RectangleF(
                            0,
                            0,
                            this.ClientSizeWidth(),
                            this.ClientSizeHeight());
                        using (var f = new StringFormat())
                        {
                            f.Alignment = StringAlignment.Center;
                            f.LineAlignment = StringAlignment.Center;
                            e.Graphics.DrawString(
                                DCSR.NoDocument,
                                this.Font,
                                DCSystem_Drawing.Brushes.Red,
                                rect,
                                f);
                        }//using
                    }
                    else
                    {
                        this._Document.CacheOptions();
                        this._Document.States.Printing = false;
                        this._Document.States.PrintPreviewing = false;
                        {
                            this._Document.States.RenderMode = DocumentRenderMode.Paint;
                        }
                        if (this._WASMPrintMode)
                        {
                            this._Document.States.Printing = true;
                            this._Document.States.PrintPreviewing = true;
                            this._Document.States.RenderMode = DocumentRenderMode.Print;
                        }
                        _SelectionAreaRectangles = new List<DCSystem_Drawing.Rectangle>();
                        bool checkBodyPartialSourceRect =
                            this._Document.Body.IsDrawGridLine(InnerDocumentRenderMode.Paint);
                        foreach (var page in this.Pages)
                        {
                            page.GlobalIndex = page.PageIndex;
                        }
                        base.InnerOnPaint(
                            e,
                            checkBodyPartialSourceRect);
                        DocumentViewOptions vop = this.DocumentOptions.ViewOptions;
                        if (_SelectionAreaRectangles != null
                            && _SelectionAreaRectangles.Count > 0)
                        {
                            {
                                // 使用遮盖色高亮度显示被选择的区域
                                using (var sb6 = new SolidBrush(vop.SelectionHightlightMaskColor))
                                {
                                    foreach (SimpleRectangleTransform item in this.PagesTransform)
                                    {
                                        if (item.Enable
                                            && item.ContentStyle == this._Document.CurrentContentPartyStyle)
                                        {
                                            foreach (var rect in _SelectionAreaRectangles)
                                            {
                                                var rect2 = DCSystem_Drawing.Rectangle.Intersect(rect, item.DescRect);
                                                if (rect2.IsEmpty == false)
                                                {
                                                    rect2 = item.UnTransformRectangle(rect2);
                                                    DCSystem_Drawing.Rectangle rect3 = DCSystem_Drawing.Rectangle.Intersect(rect2, e.ClipRectangle);
                                                    if (rect3.IsEmpty == false)
                                                    {
                                                        e.Graphics.FillRectangle(sb6, rect3);
                                                        if( this._LastClientSelectionBounds.Width == 0 )
                                                        {
                                                            this._LastClientSelectionBounds = rect3;
                                                        }
                                                        else
                                                        {
                                                            this._LastClientSelectionBounds = DCSystem_Drawing.Rectangle.Union(this._LastClientSelectionBounds, rect3);
                                                        }
                                                    }//if
                                                }//if
                                            }//foreach
                                        }//if
                                    }//foreach
                                }
                            }//if
                        }
                        _SelectionAreaRectangles = null;
                        // 绘制拖拽句柄
                        if (this.DragableElement != null
                            && this.DragableElement.DocumentContentElement == this._Document.CurrentContentElement)
                        {
                            DCSystem_Drawing.Rectangle ab = this.DragableHandleBounds;
                            if (ab.IsEmpty == false)
                            {
                                e.Graphics.DrawImage(
                                    DCStdImageList.DragHandle ,
                                    ab.Left,
                                    ab.Top);
                            }
                        }
                    }//else
                }//else
                e.Graphics.ResetTransform();
            }
            catch (Exception ext)
            {
                var strErrorMessage = ext.ToString();
                DCConsole.Default.WriteLine("DCWriter内部错误:" + strErrorMessage);
                var opts = this.DocumentOptions.ViewOptions;
                if (opts == null || opts.ShowRedErrorMessageForPaint)
                {
                    e.Graphics.ResetClip();
                    e.Graphics.ResetTransform();
                    e.Graphics.PageUnit =  GraphicsUnit.Pixel;
                    using (var format = new StringFormat())
                    {
                        format.Alignment = StringAlignment.Near;
                        format.LineAlignment = StringAlignment.Near;
                        e.Graphics.DrawString(
                            strErrorMessage,
                            this.Font,
                            DCSystem_Drawing.Brushes.Red,
                            new DCSystem_Drawing.RectangleF(
                                0,
                                0,
                                this.ClientSizeWidth(),
                                this.ClientSizeHeight()),
                            format);
                    }
                }
                if( DomDocument._DebugMode)
                {
                    DCConsole.Default.WriteLineError(ext.ToString());
                }
            }
            finally
            {
                this._Document?.ClearCachedOptions();
            }
        }

        internal DCSystem_Drawing.Rectangle _LastClientSelectionBounds = DCSystem_Drawing.Rectangle.Empty;

        /// <summary>
        /// 绘制页面框架
        /// </summary>
        /// <param name="v_PrintPage"></param>
        /// <param name="g"></param>
        /// <param name="ClipRectangle"></param>
        /// <param name="FillBackGround"></param>
        protected override void DrawPageFrame(
            object v_PrintPage, 
            DCGraphics g,
            DCSystem_Drawing.Rectangle ClipRectangle, 
            bool FillBackGround)
        {
            if (v_PrintPage == null || g == null )
            {
                return;
            }
            base.DrawPageFrame( 
                v_PrintPage, 
                g, 
                ClipRectangle, 
                FillBackGround);
            var myPage = (PrintPage)v_PrintPage;
        }
        /// <summary>
        /// 绘制文档内容
        /// </summary>
        /// <param name="document"></param>
        /// <param name="args"></param>
        /// <param name="nativeArgs"></param>
        protected override void DrawDocumentContent(
            DomDocument document,
            InnerPageDocumentPaintEventArgs args,
            InnerPaintEventArgs nativeArgs)
        {
            if (document == null)
            {
                return;
            }
            base.DrawDocumentContent(document, args, nativeArgs);
        }

        /// <summary>
        /// 完成绘制一个视图转换单元后执行的操作
        /// </summary>
        /// <param name="args">绘图事件参数</param>
        /// <param name="item">视图转换单元</param>
        protected override void OnAfterPaintItem(InnerPaintEventArgs args, SimpleRectangleTransform item)
        {
            {
                if (item.Enable == false && item.DrawMaskWhenDisable)
                {
                        DomDocumentContentElement dce = null;
                        if (item.ContentStyle == PageContentPartyStyle.Header)
                        {
                            dce = ((DomDocument)item.DocumentObject).Header;
                        }
                        else if (item.ContentStyle == PageContentPartyStyle.Footer)
                        {
                            dce = ((DomDocument)item.DocumentObject).Footer;
                        }
                        if (dce != null && dce.HasContentElement == false)
                        {
                            // 内容为空，则无需白色半透明覆盖
                            return;
                        }
                        DCSystem_Drawing.Rectangle rect = args.IntersectWithClipRectangle(item.SourceRect);
                        if (rect.IsEmpty == false)
                        {
                            // 若区域无效则用白色半透明进行覆盖，以作标记
                            using ( SolidBrush maskBrush
                                = new SolidBrush(DCSystem_Drawing.Color.FromArgb(140, Color.White)))
                            {
                                args.Graphics.FillRectangle(
                                    maskBrush,
                                    rect.Left,
                                    rect.Top,
                                    rect.Width + 2,
                                    rect.Height + 2);
                            }
                        }
                }
            }
        }
     
        /// <summary>
        /// 可逆矩形列表
        /// </summary>
        private List<DCSystem_Drawing.Rectangle> _SelectionAreaRectangles = null;

        /// <summary>
        /// 添加被选择区域矩形
        /// </summary>
        /// <param name="rect">矩形</param>
        public void AddSelectionAreaRectangle(DCSystem_Drawing.Rectangle rect)
        {
            if (_SelectionAreaRectangles != null)
            {
                for (int iCount = _SelectionAreaRectangles.Count - 1; iCount >= 0; iCount--)
                {
                    DCSystem_Drawing.Rectangle item = _SelectionAreaRectangles[iCount];
                    if (item.Contains(rect))
                    {
                        return;
                    }

                    DCSystem_Drawing.Rectangle iRect = DCSystem_Drawing.Rectangle.Intersect(item, rect);
                    if (iRect.IsEmpty == false)
                    {
                        if (iRect.Equals(item))
                        {
                            _SelectionAreaRectangles.RemoveAt(iCount);
                            //break;
                        }
                        else if (iRect.Equals(rect))
                        {
                            return;
                        }
                        else if (iRect.Top == item.Top && iRect.Height == item.Height)
                        {
                            if (iRect.Left == item.Left)
                            {
                                item.Width = item.Right - iRect.Right;
                                item.X = iRect.Right;
                                _SelectionAreaRectangles[iCount] = item;
                            }
                            else if (iRect.Right == item.Right)
                            {
                                item.Width = iRect.Left - item.Left;
                                _SelectionAreaRectangles[iCount] = item;
                            }
                        }
                        else if (iRect.Left == item.Left && iRect.Width == item.Width)
                        {
                            if (iRect.Top == item.Top)
                            {
                                item.Height = item.Bottom - iRect.Bottom;
                                item.Y = iRect.Bottom;
                                _SelectionAreaRectangles[iCount] = item;
                            }
                            else if (iRect.Bottom == item.Bottom)
                            {
                                item.Height = iRect.Top - item.Top;
                                _SelectionAreaRectangles[iCount] = item;
                            }
                        }
                        else if (iRect.Top == rect.Top && iRect.Height == rect.Height)
                        {
                            if (iRect.Left == rect.Left)
                            {
                                rect.Width = rect.Right - iRect.Right;
                                rect.X = iRect.Right;
                            }
                            else if (iRect.Right == rect.Right)
                            {
                                rect.Width = iRect.Left - rect.Left;
                            }
                        }

                        else if (iRect.Left == rect.Left && iRect.Width == rect.Width)
                        {
                            if (iRect.Top == rect.Top)
                            {
                                rect.Height = rect.Bottom - iRect.Bottom;
                                rect.Y = iRect.Bottom;
                            }
                            else if (iRect.Bottom == rect.Bottom)
                            {
                                rect.Height = iRect.Top - rect.Top;
                            }
                        }
                    }
                }//foreach
                if (rect.IsEmpty == false)
                {
                    _SelectionAreaRectangles.Add(rect);
                }
            }
        }

        /// <summary>
        /// 声明指定文档行区域无效，需要重新绘制。
        /// </summary>
        /// <param name="line"></param>
        /// <param name="party"></param>
        public void ViewInvalidate(DomLine line, PageContentPartyStyle party)
        {
            if (line == null)
            {
                throw new ArgumentNullException("line");
            }
            DCSystem_Drawing.RectangleF rect = line.AbsBounds;
            rect.Width = line.ViewWidth;
            this.ViewInvalidate(rect, party);
        }

        internal bool _EnabledViewInvalidate = true;
        /// <summary>
        /// 声明指定区域无效，需要重新绘制。无效矩形采用视图坐标。
        /// </summary>
        /// <param name="rect">无效矩形</param>
        /// <param name="party">文档区域</param>
        public void ViewInvalidate(RectangleF rect, PageContentPartyStyle party)
        {
            if(this._EnabledViewInvalidate == false )
            {
                return;
            }
            DomDocument doc = this._Document;
            if (doc != null)
            {
                //SimpleRectangleTransform maxItem = null;
                //float maxArea = 0;
                //if (rect.Top <= 0 && ( party == PageContentPartyStyle.Body || party == PageContentPartyStyle.HeaderRows))
                //{
                //    float b = rect.Bottom ;
                //    rect.Y = 1;
                //    rect.Height = b - rect.Y;
                //}
                var items = this.PagesTransform;
                int len = items.Count;
                //foreach (SimpleRectangleTransform item in this.PagesTransform)
                for (var iCount = 0; iCount < len; iCount++)
                {
                    var item = items.GetNativeItem(iCount);
                    if (item.ContentStyle == party)
                    {
                        if (party == PageContentPartyStyle.Body || party == PageContentPartyStyle.HeaderRows)
                        {
                            if (rect.Top == item.DescRectFTop)
                            {
                                // 将区域顶端位置向下移动一个单位，避免对页眉下边框线的影响
                                //float b = rect.Bottom;
                                rect.Y++;
                                rect.Height--;
                            }
                        }
                        DCSystem_Drawing.RectangleF irect = DCSystem_Drawing.RectangleF.Intersect(rect, item.DescRectF);
                        if (irect.Width > 2 && irect.Height > 2)// item.DescRectF.Contains(rect.Left, rect.Top))
                        {
                            var p = item.UnTransformPointF(irect.Left, irect.Top);// .Location);
                            // 可能存在微小的误差使得点不能精确的在目标矩形中
                            // 在此将目标矩形稍微放大一点，提高命中率。
                            var srect = item.SourceRectF;
                            srect.Inflate(3, 3);
                            if (srect.Contains(p))// p.IsEmpty == false)
                            {
                                var rect2 = new DCSystem_Drawing.RectangleF(
                                    p,
                                    item.UnTransformSizeF(rect.Width , rect.Height));
                                if (rect2.Width > 0 && rect2.Height > 0)
                                {
                                    //rect2.Offset(-2, -2);
                                    //rect2.Width += 4;
                                    //rect2.Height += 4;
                                    rect2.X -= 2;
                                    rect2.Width += 4;
                                    var rect3 = rect2.ToOutInt32();// DCSystem_Drawing.Rectangle.Ceiling(rect2);
                                    this.WASMInvalidate(item.PageObject, rect3);
                                   
                                }
                            }
                        }//if
                    }//if
                }//foreach
            }
        }

        /// <summary>
        /// 设置指定的区域视图无效
        /// </summary>
        /// <param name="range">文档区域</param>
        public void Invalidate(DCRange range)
        {
            InvalidateElements(range); 
        }
        /// <summary>
        /// 声明一群文档元素视图无效，需要重新绘制
        /// </summary>
        /// <param name="elements">元素对象集合</param>
        public void InvalidateElements(System.Collections.IEnumerable elements)
        {
            if (elements == null)
            {
                return;
            }
            if (this._Document != null && this._Document.AllowInvalidateForUILayoutOrView())
            {
                DomLine lastLine = null;
                float lastStartPos = 0;
                float lastEndPos = 0;
                PageContentPartyStyle pcps = PageContentPartyStyle.Body;
                bool isFirstElement = true;
                foreach (DomElement element in elements)
                {
                    if( element.DocumentContentElement == null )
                    {
                        continue;
                    }
                    if (isFirstElement)
                    {
                        pcps = element.DocumentContentElement.ContentPartyStyle;
                        isFirstElement = false;
                    }
                    if (element._OwnerLine == null)
                    {
                        var bounds = this._Document.GetViewBoundsForInvalidateElementView(element);
                        this.ViewInvalidate(bounds, pcps);
                    }
                    else
                    {
                        if (lastLine == null || element._OwnerLine != lastLine)
                        {
                            if (lastLine != null)
                            {
                                var bounds = new DCSystem_Drawing.RectangleF(
                                    lastLine.AbsLeft + lastStartPos,
                                    lastLine.AbsTop,
                                    lastEndPos - lastStartPos,
                                    lastLine.Height);
                                this.ViewInvalidate(bounds, pcps);
                            }
                            lastLine = element._OwnerLine;
                            lastStartPos = element.Left;
                        }
                        lastEndPos = element.Left + element.ViewWidth;
                    }
                }
                if (lastLine != null)
                {
                    var bounds2 = new DCSystem_Drawing.RectangleF(
                        lastLine.AbsLeft + lastStartPos,
                        lastLine.AbsTop,
                        lastEndPos - lastStartPos,
                        lastLine.Height);
                    this.ViewInvalidate(bounds2, pcps);
                }
            }
        }

    }
}
