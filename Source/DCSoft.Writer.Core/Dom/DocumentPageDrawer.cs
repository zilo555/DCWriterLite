using System;
using System.Collections.Generic;
using System.Text;
using DCSoft.Printing;
using DCSoft.Drawing;

namespace DCSoft.Writer.Dom
{
    /// <summary>
    /// 文档页面绘制器
    /// </summary>
    /// <remarks>编制 袁永福</remarks>
    public class DocumentPageDrawer : PageContentDrawer
    {
        public DocumentPageDrawer()
        {

        }

        /// <summary>
        /// 打印指定页面
        /// </summary>
        /// <param name="myPage">页面对象</param>
        /// <param name="g">绘图操作对象</param>
        /// <param name="MainClipRect">主剪切矩形</param>
        /// <param name="UseMargin">是否启用页边距</param>
        public override void DrawPage(
            PrintPage myPage,
            DCGraphics g,
             Rectangle MainClipRect,
            bool UseMargin)
        {
            DomDocument document = this.Document;
            XPageSettings ps = myPage.PageSettings;
            if (ps == null)
            {
                ps = document.PageSettings;
            }
            PageLayoutInfo layoutInfo = new PageLayoutInfo(myPage, true);
            if (UseMargin)
            {
                MainClipRect.X = MainClipRect.X - (int)layoutInfo.LeftMargin;
                MainClipRect.Width = MainClipRect.Width + (int)layoutInfo.LeftMargin + (int)layoutInfo.RightMargin;
            }
            g.PageUnit = DCSystem_Drawing.GraphicsUnit.Document;
            Rectangle ClipRect = Rectangle.Empty;
            if (this.PageHeadText != null)
            {
                // 绘制标题文本
                g.DrawString(
                    this.PageHeadText,
                    XFontValue.DefaultFont,
                    Brushes.Red,
                    20,
                    20,
                    StringFormat.GenericDefault);
            }

            float viewHeaderDistance = layoutInfo.HeaderDistance;

            float headerHeight = Math.Max(ps.ViewHeaderHeight, myPage.HeaderContentHeight);
            int headerHeightFix = 0;
            myPage.PageHeaderLineFixOffset = 0;
            if (myPage.HeaderContentHeight > ps.ViewHeaderHeight - 10)
            {
                headerHeightFix = (int)(myPage.HeaderContentHeight - (ps.ViewHeaderHeight - 10));
                myPage.PageHeaderLineFixOffset = 13;
            }
            {
                // 绘制页眉
                g.ResetTransform();
                g.ResetClip();
                g.PageUnit = DCSystem_Drawing.GraphicsUnit.Document;
                ClipRect = new Rectangle(
                    -(int)layoutInfo.LeftMargin,
                    0,
                    (int)Math.Ceiling(myPage.Width + (int)(layoutInfo.LeftMargin + layoutInfo.RightMargin)),
                    (int)Math.Ceiling(headerHeight));
                g.ScaleTransform(this.RuntimeXZoomRate, this.RuntimeYZoomRate);
                g.TranslateTransform(
                    (int)layoutInfo.LeftMargin,
                    viewHeaderDistance);

                g.SetClip(this.ZoomClipRect(new RectangleF(
                    ClipRect.Left,
                    ClipRect.Top,
                    ClipRect.Width + 1,
                    ClipRect.Height + 2)));

                var args = new InnerPageDocumentPaintEventArgs(
                    g,
                    ClipRect,
                    document,
                    myPage,
                    PageContentPartyStyle.Header,
                    null);
                //args.SetRecorder(base._Recorder);
                args.RenderMode = this.RenderMode;
                args.ContentBounds = ClipRect;
                args.PageClipRectangle = ClipRect.ToFloat();
                args.Options = this.Options;
                args.PageIndex = myPage.PageIndex;
                document.DrawPageContent(args);
                g.ResetClip();
                g.ResetTransform();

                if (myPage.HeaderRowsBounds.IsEmpty == false)
                {
                    // 绘制标题行
                    g.ResetTransform();
                    g.ResetClip();
                    g.PageUnit = DCSystem_Drawing.GraphicsUnit.Document;
                    ClipRect = new Rectangle(
                        (int)myPage.HeaderRowsBounds.Left - (int)layoutInfo.LeftMargin,
                        (int)myPage.HeaderRowsBounds.Top - 2,
                        (int)myPage.HeaderRowsBounds.Width + (int)(layoutInfo.LeftMargin + layoutInfo.RightMargin),
                        (int)myPage.HeaderRowsBounds.Height + 4);
                    g.ScaleTransform(this.RuntimeXZoomRate, this.RuntimeYZoomRate);
                    g.TranslateTransform(
                        layoutInfo.LeftMargin,
                        layoutInfo.TopMargin - myPage.HeaderRowsBounds.Top + headerHeightFix);

                    g.SetClip(this.ZoomClipRect(new RectangleF(
                        ClipRect.Left,
                        ClipRect.Top - 1,
                        ClipRect.Width + 1,
                        ClipRect.Height + 2)));

                    var args2 = new InnerPageDocumentPaintEventArgs(
                        g,
                        ClipRect,
                        document,
                        myPage,
                        PageContentPartyStyle.HeaderRows,
                        null);
                    //args2.SetRecorder(base._Recorder);
                    args2.ContentBounds = ClipRect;
                    args2.PageIndex = myPage.GlobalIndex;
                    args2.NumberOfPages = this.Pages.Count;
                    args2.ContentBounds = ClipRect;
                    args2.PageClipRectangle = ClipRect.ToFloat();
                    args2.RenderMode = this.RenderMode;
                    args2.Options = this.Options;
                    document.DrawPageContent(args2);
                    g.ResetClip();
                    g.ResetTransform();

                }

            }

            // 绘制页面正文
            ClipRect = new Rectangle(
                -(int)layoutInfo.LeftMargin,
                (int)Math.Floor(myPage.Top),
                (int)Math.Ceiling(myPage.Width + (int)(layoutInfo.LeftMargin + layoutInfo.RightMargin)),
                (int)Math.Ceiling(myPage.StandartPapeBodyHeight));
            if (myPage.HeaderRowsBounds.IsEmpty == false)
            {
                ClipRect.Height = (int)(ClipRect.Height - myPage.HeaderRowsBounds.Height);
            }
            if (MainClipRect.IsEmpty == false)
            {
                ClipRect = Rectangle.Intersect(ClipRect, MainClipRect);
            }
            if (ClipRect.IsEmpty == false)
            {
                g.ScaleTransform(this.RuntimeXZoomRate, this.RuntimeYZoomRate);
                g.TranslateTransform(
                    layoutInfo.LeftMargin,
                    layoutInfo.TopMargin
                    - myPage.Top
                    + headerHeightFix
                    + myPage.HeaderRowsBounds.Height);



                PointF[] pts = new PointF[] {
                    new PointF(myPage.Left, myPage.Top),
                    new PointF(
                        myPage.Left + myPage.Width,
                        myPage.Top + myPage.Height) ,
                    new PointF(
                        myPage.Left + myPage.Width ,
                        myPage.Top + myPage.StandartPapeBodyHeight)};
                g.TransformPoints(pts);
                this._LastBodyContentTransfrom = new SimpleRectangleTransform();
                this._LastBodyContentTransfrom.SourceRectF =
                    new RectangleF(
                        pts[0].X,
                        pts[0].Y,
                        pts[1].X - pts[0].X,
                        pts[1].Y - pts[0].Y);
                this._LastBodyContentTransfrom.DescRectF =
                    new RectangleF(
                        myPage.Left,
                        myPage.Top,
                        myPage.Width,
                        myPage.Height);
                this._LastBodyContentTransfrom.PartialAreaSourceBounds = new Rectangle(
                    (int)pts[0].X,
                    (int)pts[0].Y,
                    (int)(pts[2].X - pts[0].X),
                    (int)(pts[2].Y - pts[0].Y));
                RectangleF rect = DrawerUtil.FixClipBounds(
                   g,
                   ClipRect.Left,
                   ClipRect.Top,
                   ClipRect.Width,
                   ClipRect.Height);

                rect.Offset(-4, -4);
                rect.Width = rect.Width + 8;
                rect.Height = rect.Height + 8;
                g.SetClip(this.ZoomClipRect(rect));
                var args = new InnerPageDocumentPaintEventArgs(
                    g,
                    ClipRect,
                    document,
                    myPage,
                    PageContentPartyStyle.Body,
                    null);
                //args.SetRecorder(base._Recorder);
                args.RenderMode = this.RenderMode;
                args.ContentBounds = ClipRect;
                args.PageClipRectangle = ClipRect.ToFloat();
                float pcHeight = myPage.StandartPapeBodyHeight;
                if (myPage.HeaderRowsBounds.IsEmpty == false)
                {
                    pcHeight = pcHeight - myPage.HeaderRowsBounds.Height;
                }
                args.PageClipRectangle = new RectangleF(
                    ClipRect.Left,
                    myPage.Top,
                    Math.Max(ClipRect.Width, myPage.Width),
                    pcHeight);// myPage.StandartPapeBodyHeight );// myPage.Height );// myPage.ViewStandardHeight - myPage.HeaderHeight);
                args.Options = this.Options;
                args.PageIndex = myPage.PageIndex;
                document.DrawPageContent(args);
            }

            {
                // 绘制页脚
                g.ResetClip();
                g.ResetTransform();
                g.PageUnit = DCSystem_Drawing.GraphicsUnit.Document;
                float footerHeight = Math.Max(myPage.FooterContentHeight, ps.ViewFooterHeight);
                ClipRect = new Rectangle(
                    -(int)layoutInfo.LeftMargin,
                    0,
                    (int)Math.Ceiling(myPage.Width + (int)(layoutInfo.LeftMargin + layoutInfo.RightMargin)),
                    (int)Math.Ceiling(footerHeight));

                int dy = 0;

                dy = (int)(myPage.ViewPaperHeight
                    - layoutInfo.FooterDistance - myPage.FooterContentHeight);

                g.ScaleTransform(this.RuntimeXZoomRate, this.RuntimeYZoomRate);
                g.TranslateTransform(
                    layoutInfo.LeftMargin,
                    dy);

                g.SetClip(this.ZoomClipRect(new RectangleF(
                    ClipRect.Left,
                    ClipRect.Top,
                    ClipRect.Width + 1,
                    ClipRect.Height + 1)));

                var args = new InnerPageDocumentPaintEventArgs(
                    g,
                    ClipRect,
                    document,
                    myPage,
                    PageContentPartyStyle.Footer,
                    null);
                args.RenderMode = this.RenderMode;
                args.ContentBounds = ClipRect;
                args.PageClipRectangle = ClipRect.ToFloat();
                args.Options = this.Options;
                document.DrawPageContent(args);
            }//if( this.bolDrawFooter )
        }//public void DrawPage()
    }
}