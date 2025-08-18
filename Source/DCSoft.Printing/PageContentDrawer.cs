using System;
//using DCSoft.WinForms;
using DCSoft.Drawing;

namespace DCSoft.Printing
{
    /// <summary>
    /// 文档页面绘制对象
    /// </summary>
    /// <remarks>
    /// 本对象用于使用页面的方式绘制文档的内容。
    /// </remarks>
    public class PageContentDrawer
    {
        /// <summary>
        /// 初始化对象
        /// </summary>
        public PageContentDrawer()
        {
        }

        private ContentRenderMode _RenderMode = ContentRenderMode.Print;
        /// <summary>
        /// 内容呈现模式
        /// </summary>
        public ContentRenderMode RenderMode
        {
            get
            {
                return _RenderMode;
            }
            set
            {
                _RenderMode = value;
            }
        }

        private DCSoft.Writer.Dom.DomDocument myDocument = null;
        /// <summary>
        /// 文档对象
        /// </summary>
        public DCSoft.Writer.Dom.DomDocument Document
        {
            get
            {
                return myDocument;
            }
            set
            {
                myDocument = value;
                if (value != null)
                {
                    myPages = value.Pages;
                }
            }
        }

        private float fXZoomRate = 1.0f;
        /// <summary>
        /// X轴缩放比例
        /// </summary>
        public float XZoomRate
        {
            get
            {
                return fXZoomRate;
            }
            set
            {
                fXZoomRate = value;
            }
        }

        /// <summary>
        /// 运行时的X轴缩放比率
        /// </summary>
        protected float RuntimeXZoomRate
        {
            get
            {
                float v = this.XZoomRate;
                if (v <= 0.01)
                {
                    return 1;
                }
                if (v < 0.2f)
                {
                    v = 0.2f;
                }
                if (v > 5)
                {
                    v = 5;
                }
                return v;
            }
        }

        private float fYZoomRate = 1.0f;
        /// <summary>
        /// X轴缩放比例
        /// </summary>
        public float YZoomRate
        {
            get
            {
                return fYZoomRate;
            }
            set
            {
                fYZoomRate = value;
            }
        }

        /// <summary>
        /// 运行时的Y轴缩放比率
        /// </summary>
        protected float RuntimeYZoomRate
        {
            get
            {
                float v = this.YZoomRate;
                if (v <= 0.01)
                {
                    return 1;
                }
                if (v < 0.2f)
                {
                    v = 0.2f;
                }
                if (v > 5)
                {
                    v = 5;
                }
                return v;
            }
        }

        /// <summary>
        /// 缩放剪切矩形
        /// </summary>
        /// <param name="rect">原始剪切矩形</param>
        /// <returns>处理后的矩形</returns>
        protected DCSystem_Drawing.RectangleF ZoomClipRect(DCSystem_Drawing.RectangleF rect)
        {
            float x = this.RuntimeXZoomRate;
            float y = this.RuntimeYZoomRate;
            if (x != 1)
            {
                rect.X = rect.X * x;
                rect.Width = rect.Width * x;
            }
            if (y != 1)
            {
                rect.Y = rect.Y * y;
                rect.Height = rect.Height * y;
            }
            return rect;
        }

        private string strPageHeadText = null;
        /// <summary>
        /// 页面头额外显示的文字
        /// </summary>
        public string PageHeadText
        {
            get
            {
                return strPageHeadText;
            }
            set
            {
                strPageHeadText = value;
            }
        }

        private PrintPageCollection myPages = null;
        /// <summary>
        /// 分页集合对象
        /// </summary>
        public PrintPageCollection Pages
        {
            get
            {
                return myPages;
            }
            set
            {
                myPages = value;
            }
        }
        private int _PageMarginLineLength = 60;
        /// <summary>
        /// 表示页面边界的线条长度,
        /// </summary>
        public int PageMarginLineLength
        {
            get
            {
                return _PageMarginLineLength;
            }
            set
            {
                _PageMarginLineLength = value;
            }
        }


        /// <summary>
        /// 创建页面框架绘制器
        /// </summary>
        /// <returns>绘制器</returns>
        protected virtual PageFrameDrawer CreatePageFrameDawer()
        {
            return new PageFrameDrawer();
        }

        /// <summary>
        /// 创建指定页的BMP图片对象
        /// </summary>
        /// <param name="page">页面对象</param>
        /// <param name="DrawMargin">是否绘制页边距线</param>
        /// <returns>创建的BMP图片对象</returns>
        public void InnerDrawPage2(PrintPage page, DCGraphics g, bool DrawMargin)
        {
            g.PageUnit = DCSystem_Drawing.GraphicsUnit.Document;
            PageLayoutInfo layoutInfo = new PageLayoutInfo(page, true);
            PageFrameDrawer drawer = this.CreatePageFrameDawer();
            drawer.DrawMargin = DrawMargin;
            drawer.MarginLineLength = this.PageMarginLineLength;
            drawer.LeftMargin = (int)(layoutInfo.LeftMargin * this.XZoomRate);
            drawer.TopMargin = (int)(layoutInfo.TopMargin * this.YZoomRate);
            drawer.RightMargin = (int)(layoutInfo.RightMargin * this.XZoomRate);
            drawer.BottomMargin = (int)(layoutInfo.BottomMargin * this.YZoomRate);
            drawer.Bounds = new DCSystem_Drawing.Rectangle(
                0,
                0,
                (int)(layoutInfo.PageWidth * this.XZoomRate),
                (int)(layoutInfo.PageHeight * this.YZoomRate));

            drawer.DrawPageFrame(g, DCSystem_Drawing.Rectangle.Empty);

            DrawPage(page, g, DCSystem_Drawing.Rectangle.Ceiling(page.Bounds), true);
        }

        private DCPrintDocumentOptions _Options = null;
        /// <summary>
        /// 文档选项信息
        /// </summary>
        public DCPrintDocumentOptions Options
        {
            get
            {
                return _Options;
            }
            set
            {
                _Options = value;
            }
        }

        /// <summary>
        /// 最后一次调用的正文内容坐标转换对象
        /// </summary>
        protected SimpleRectangleTransform _LastBodyContentTransfrom = null;
        /// <summary>
        /// 打印指定页面
        /// </summary>
        /// <param name="myPage">页面对象</param>
        /// <param name="g">绘图操作对象</param>
        /// <param name="MainClipRect">主剪切矩形</param>
        /// <param name="UseMargin">是否启用页边距</param>
        public virtual void DrawPage(
            PrintPage myPage,
            DCGraphics g,
             DCSystem_Drawing.Rectangle MainClipRect,
            bool UseMargin)
        {
            int LeftMargin = 0;
            int TopMargin = 0;
            //int RightMargin = 0;
            //int BottomMargin = 0;
            if (UseMargin)
            {
                LeftMargin = (int)myPage.ViewLeftMargin;
                TopMargin = (int)myPage.ViewTopMargin;
                //RightMargin = (int)myPage.ViewRightMargin;
                //BottomMargin = (int)myPage.ViewBottomMargin;
            }
            var localXZoomRate = this.RuntimeXZoomRate;
            var localYZoomRate = this.RuntimeYZoomRate;
            g.PageUnit = DCSystem_Drawing.GraphicsUnit.Document;
            DCSystem_Drawing.Rectangle ClipRect = DCSystem_Drawing.Rectangle.Empty;
            if (this.strPageHeadText != null)
            {
                // 绘制标题文本
                g.DrawString(
                    strPageHeadText,
                    XFontValue.DefaultFont,
                    Brushes.Red,
                    20,
                    20,
                    StringFormat.GenericDefault);
            }

            {
                // 绘制页眉
                if (myPage.HeaderContentHeight > 0)
                {
                    g.ResetTransform();
                    g.ResetClip();

                    ClipRect = new DCSystem_Drawing.Rectangle(
                        0,
                        0,
                        (int)Math.Ceiling(myPage.Width),
                        (int)Math.Ceiling(myPage.HeaderContentHeight));

                    g.ScaleTransform(localXZoomRate, localYZoomRate);
                    g.TranslateTransform(
                        LeftMargin,
                        TopMargin);

                    g.SetClip(this.ZoomClipRect(new RectangleF(
                        ClipRect.Left,
                        ClipRect.Top,
                        ClipRect.Width + 1,
                        ClipRect.Height + 1)));

                    var args = new InnerPageDocumentPaintEventArgs(
                        g,
                        ClipRect,
                        myDocument,
                        myPage,
                        PageContentPartyStyle.Header,
                        null);
                    args.Options = this.Options;
                    args.ContentBounds = ClipRect;
                    args.PageClipRectangle = ClipRect.ToFloat();
                    args.PageIndex = myPage.GlobalIndex;
                    args.NumberOfPages = this.Pages.Count;
                    args.ContentBounds = ClipRect;
                    // 绘制页眉
                    myDocument.DrawPageContent(args);
                }
                g.ResetClip();
                g.ResetTransform();

                if (myPage.HeaderRowsBounds.IsEmpty == false)
                {
                    // 绘制标题行
                    g.ResetTransform();
                    g.ResetClip();
                    ClipRect = new DCSystem_Drawing.Rectangle(
                        (int)myPage.HeaderRowsBounds.Left - 1,
                        (int)myPage.HeaderRowsBounds.Top - 1,
                        (int)myPage.HeaderRowsBounds.Width + 1,
                        (int)myPage.HeaderRowsBounds.Height + 1);

                    g.ScaleTransform(localXZoomRate, localYZoomRate);
                    g.TranslateTransform(
                        LeftMargin,
                        TopMargin - myPage.HeaderRowsBounds.Top);

                    g.SetClip(this.ZoomClipRect(new DCSystem_Drawing.RectangleF(
                        ClipRect.Left,
                        ClipRect.Top,
                        ClipRect.Width + 1,
                        ClipRect.Height + 1)));

                    var args = new InnerPageDocumentPaintEventArgs(
                        g,
                        ClipRect,
                        myDocument,
                        myPage,
                        PageContentPartyStyle.Header,
                        null);
                    //args.SetRecorder(this._Recorder);
                    args.ContentBounds = ClipRect;
                    args.PageClipRectangle = ClipRect.ToFloat();
                    args.PageIndex = myPage.GlobalIndex;
                    args.NumberOfPages = this.Pages.Count;
                    args.ContentBounds = ClipRect;
                    myDocument.DrawPageContent(args);
                }
            }

            // 绘制页面正文
            ClipRect = new DCSystem_Drawing.Rectangle(
                0,
                (int)Math.Floor(myPage.Top),
                (int)Math.Ceiling(myPage.Width),
                (int)Math.Ceiling(myPage.Height));

            if (!MainClipRect.IsEmpty)
            {
                ClipRect = DCSystem_Drawing.Rectangle.Intersect(ClipRect, MainClipRect);
            }
            if (!ClipRect.IsEmpty)
            {
                g.ScaleTransform(localXZoomRate, localYZoomRate);
                g.TranslateTransform(
                    LeftMargin,
                    TopMargin - myPage.Top + myPage.HeaderContentHeight + myPage.HeaderRowsBounds.Height);

                // 获得文档正文内容视图区域到打印区域的坐标转换关系
                DCSystem_Drawing.PointF[] pts = new DCSystem_Drawing.PointF[] {
                    new  DCSystem_Drawing.PointF(myPage.Left, myPage.Top),
                    new  DCSystem_Drawing.PointF(myPage.Left + myPage.Width, myPage.Top + myPage.Height) };
                g.Transform.TransformPoints(pts);
                this._LastBodyContentTransfrom = new SimpleRectangleTransform();
                this._LastBodyContentTransfrom.SourceRectF =
                    new DCSystem_Drawing.RectangleF(pts[0].X, pts[0].Y, pts[1].X - pts[0].X, pts[1].Y - pts[0].Y);
                this._LastBodyContentTransfrom.DescRectF =
                    new DCSystem_Drawing.RectangleF(myPage.Left, myPage.Top, myPage.Width, myPage.Height);

                DCSystem_Drawing.RectangleF rect = DrawerUtil.FixClipBounds(
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
                    myDocument,
                    myPage,
                    PageContentPartyStyle.Body,
                    null);
                //args.SetRecorder(this._Recorder);
                args.PageIndex = myPage.GlobalIndex;
                args.NumberOfPages = this.Pages.Count;
                args.ContentBounds = ClipRect;
                args.PageClipRectangle = ClipRect.ToFloat();
                myDocument.DrawPageContent(args);
            }

            {
                // 绘制页脚
                if (myPage.FooterContentHeight > 0)
                {
                    g.ResetClip();
                    g.ResetTransform();
                    float documentHeight = myPage.DocumentHeight;

                    ClipRect = new DCSystem_Drawing.Rectangle(
                        0,
                        (int)Math.Floor(documentHeight - myPage.FooterContentHeight),
                        (int)Math.Ceiling(myPage.Width),
                        (int)Math.Ceiling(myPage.FooterContentHeight));

                    int dy = 0;

                    if (UseMargin)
                    {
                        dy = (int)(myPage.ViewPaperHeight
                            - myPage.ViewBottomMargin);
                    }
                    else
                    {
                        dy = (int)(myPage.ViewPaperHeight
                            - myPage.ViewBottomMargin
                            - myPage.ViewTopMargin);
                    }


                    g.ScaleTransform(localXZoomRate, localYZoomRate);
                    g.TranslateTransform(
                        LeftMargin,
                        dy);

                    g.SetClip(this.ZoomClipRect(new RectangleF(
                        ClipRect.Left,
                        ClipRect.Top,
                        ClipRect.Width + 1,
                        ClipRect.Height + 1)));

                    var args = new InnerPageDocumentPaintEventArgs(
                        g,
                        ClipRect,
                        myDocument,
                        myPage,
                        PageContentPartyStyle.Footer,
                        null);
                    //args.SetRecorder(this._Recorder);
                    args.ContentBounds = ClipRect;
                    args.PageClipRectangle = ClipRect.ToFloat();
                    args.PageIndex = myPage.GlobalIndex;
                    args.NumberOfPages = this.Pages.Count;
                    myDocument.DrawPageContent(args);
                }
            }//if( this.bolDrawFooter )
        }//public void DrawPage()
    }//public class DocumentPageDrawer
}