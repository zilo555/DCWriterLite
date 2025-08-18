using System;
using DCSoft.WinForms;
using DCSoft.Drawing;
using DCSoft.Common;
// // 
using System.Windows.Forms;
using System.ComponentModel;
using DCSoft.Writer;
using DCSoft.Writer.Dom;

namespace DCSoft.Printing
{
    /// <summary>
    /// 带分页功能的视图控件
    /// </summary>
    public class PageViewControl : DocumentViewControl
    {
        /// <summary>
        /// 无作为的初始化对象
        /// </summary>
        public PageViewControl()
        {
            base.myTransform = new MultiPageTransform();
            //myUpdateLock.BindControl = this ;
        }

        /// <summary>
        /// WASM打印模式
        /// </summary>
        internal bool _WASMPrintMode = false;


        /// <summary>
        /// 当前页对象
        /// </summary>
        private PrintPage _CurrentPage = null;
        /// <summary>
        /// 当前页对象
        /// </summary>
        public virtual PrintPage CurrentPage
        {
            get
            {
                return _CurrentPage;
            }
            set
            {
                _CurrentPage = value;
                if (_CurrentPage != null)
                {
                    // 当前已分页模式显示,则需要滚动视图
                    int y = _CurrentPage.ClientBounds.Top;
                }
            }
        }

        /// <summary>
        /// 直接设置当前页对象
        /// </summary>
        /// <param name="page"></param>
        public bool SetCurrentPageRaw(PrintPage page)
        {
            if (this._CurrentPage != page)
            {
                this._CurrentPage = page;
                return true;
            }
            return false;
        }

        /// <summary>
        /// 从1开始计算的当前页号
        /// </summary>
        public int CurrentPageIndex
        {
            get
            {
                if (this.CurrentPage == null)
                {
                    return 0;
                }
                else
                {
                    return this.CurrentPage.PageIndex;
                }
            }
            set
            {
                value--;
                if (this.Pages != null && value >= 0 && value < this.Pages.Count)
                {
                    this.CurrentPage = this.Pages[value];
                }
            }
        }

        /// <summary>
        /// 更新当前页面对象
        /// </summary>
        /// <returns>操作是否改变了当前页面对象</returns>
        public virtual bool UpdateCurrentPage()
        {
            if (_Pages != null)
            {
                PrintPage cpage = null;
                DCSystem_Drawing.Rectangle rect = new DCSystem_Drawing.Rectangle(
                    0,
                    0,
                    this.ClientSizeWidth(),
                    this.ClientSizeHeight());

                int MaxHeight = 0;
                foreach (PrintPage page in _Pages)
                {
                    DCSystem_Drawing.Rectangle rect2 = DCSystem_Drawing.Rectangle.Intersect(
                        page.ClientBounds, rect);
                    if (!rect2.IsEmpty)
                    {
                        if (MaxHeight < rect2.Height)
                        {
                            cpage = page;
                            MaxHeight = rect2.Height;
                        }
                    }
                }
                if (cpage != _CurrentPage)
                {
                    _CurrentPage = cpage;
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 当前页改变事件
        /// </summary>
        public System.EventHandler CurrentPageChanged = null;
        /// <summary>
        /// 当前页改变事件处理
        /// </summary>
        public virtual void OnCurrentPageChanged()
        {
            if (CurrentPageChanged != null)
            {
                CurrentPageChanged(this, null);
            }
        }

        /// <summary>
        /// 页面集合
        /// </summary>
        protected PrintPageCollection _Pages = new PrintPageCollection();
        /// <summary>
        /// 页面集合
        /// </summary>
        public PrintPageCollection Pages
        {
            get
            {
                return _Pages;
            }
            set
            {
                _Pages = value;
            }
        }

        private SimpleRectangleTransform _CurrentTransformItem = null;
        /// <summary>
        /// 当前转换信息对象
        /// </summary>
        protected SimpleRectangleTransform CurrentTransformItem
        {
            get
            {
                return _CurrentTransformItem;
            }
            set
            {
                _CurrentTransformItem = value;
            }
        }

        /// <summary>
        /// 刷新坐标转换信息
        /// </summary>
        public override void RefreshScaleTransform(float dpi = 96)
        {
            MultiPageTransform trans = (MultiPageTransform)this.myTransform;
            if (trans == null)
            {
                return;
            }
            this.GraphicsUnit = this._Pages.GraphicsUnit;
            trans.Rate = GraphicsUnitConvert.GetRate(
                this.GraphicsUnit,
                 GraphicsUnit.Pixel) * 96.0 / dpi;
        }

        /// <summary>
        /// 分页坐标转换对象
        /// </summary>
        public MultiPageTransform PagesTransform
        {
            get
            {
                return (MultiPageTransform)this.Transform;
            }
        }
        private bool _Doing_UpdatePages = false;
        /// <summary>
		/// 根据分页信息更新页面排布
		/// </summary>
		public virtual void UpdatePages(int preservePixcelWidth = 0)
        {
            if (this._Pages == null || this._Pages.Count == 0)
            {
                return;
                //throw new Exception("没有有效的分页信息");
            }
            if (_Doing_UpdatePages)
            {
                return;
            }
            _Doing_UpdatePages = true;
            try
            {
                float rate = (float)(1.0 / this.ClientToViewXRate);
                // 分页视图模式
                PrintPageCollection visiblePages = this.Pages;
                var lastPageClientSize = DCSystem_Drawing.Size.Empty;
                XPageSettings lastPageSettings = null;
                var bolUseFirstInfo = false;
                PageLayoutInfo firstLayoutInfo = null;
                DCSystem_Drawing.Printing.Margins firstClientMargins = null;

                foreach (PrintPage page in visiblePages)
                {
                    DCSystem_Drawing.Rectangle clientRect = DCSystem_Drawing.Rectangle.Empty;
                    {
                        if (page.PageSettings != lastPageSettings)
                        {
                            bolUseFirstInfo = true;
                            firstLayoutInfo = new PageLayoutInfo(page, true);
                            firstLayoutInfo.Zoom(rate);
                            firstClientMargins = new Margins(
                                (int)firstLayoutInfo.LeftMargin,
                                (int)firstLayoutInfo.RightMargin,
                                (int)firstLayoutInfo.TopMargin,
                                (int)firstLayoutInfo.BottomMargin);
                            lastPageSettings = page.PageSettings;
                            var s9 = lastPageSettings.GetViewPaperSize();
                            lastPageClientSize = new DCSystem_Drawing.Size(
                                (int)Math.Round(s9.Width * rate),
                                (int)Math.Round(s9.Height * rate));
                        }
                    }
                    clientRect.Size = lastPageClientSize;
                    page.SetClientBounds(clientRect);
                    if (bolUseFirstInfo)
                    {
                        page.ClientLayoutInfo = firstLayoutInfo;
                        page.ClientMargins = firstClientMargins;
                    }
                    else
                    {
                        PageLayoutInfo layoutInfo = new PageLayoutInfo(page, true);
                        layoutInfo.Zoom(rate);
                        page.ClientLayoutInfo = layoutInfo;
                        page.ClientMargins = new Margins(
                            (int)(layoutInfo.LeftMargin),
                            (int)(layoutInfo.RightMargin),
                            (int)(layoutInfo.TopMargin),
                            (int)(layoutInfo.BottomMargin));
                    }
                }//foreach

                float totalSizeWidth = 0;
                float totalSizeHeight = 0;

                foreach (PrintPage page in visiblePages)
                {
                    totalSizeHeight = (totalSizeHeight
                        + PageSpacing
                        + page._ClientBounds.Height);
                    if (totalSizeWidth < page._ClientBounds.Width)
                    {
                        totalSizeWidth = page._ClientBounds.Width;
                    }
                    //page.OwnerPages = this.Pages;
                }
                {
                    totalSizeWidth += PageSpacing * 2;
                    totalSizeHeight += PageSpacing;
                }
                MultiPageTransform trans = (MultiPageTransform)this.Transform;
                base.GraphicsUnit = this.Pages.GraphicsUnit;

                trans.Pages = visiblePages;

                trans.Refresh(
                    rate,
                    PageSpacing,
                    PageSpacing,
                    this.PreserveSpacingInPageView());
                totalSizeWidth = 0;
                totalSizeHeight = 0;
                //totalSize = new SizeF(0, 0);
                foreach (PrintPage page in visiblePages)
                {
                    totalSizeHeight = (totalSizeHeight
                        + PageSpacing
                        + page.ClientBounds.Height);
                    if (totalSizeWidth < page.ClientBounds.Width)
                    {
                        totalSizeWidth = page.ClientBounds.Width;
                    }
                    //page.OwnerPages = this.Pages;
                }
                totalSizeWidth += PageSpacing * 2;
                totalSizeHeight += PageSpacing;
                int clientWidth = this.ClientSizeWidth();
                int x = 0;
                if (clientWidth <= totalSizeWidth + preservePixcelWidth)
                {
                    x = 0;
                }
                else
                {
                    x = (int)((clientWidth - totalSizeWidth - preservePixcelWidth) / 2);
                }
                trans.OffsetSource(x, 0, false);

                this.RefreshScaleTransform();
                int topCount = 0;
                {
                    topCount = PageSpacing + this.PreserveSpacingInPageView();
                }
                foreach (PrintPage page in visiblePages)
                {
                    if (page.ClientBounds.Top > 0)
                    {
                        //if (topCount != page.ClientBounds.Top)
                        {
                            topCount = page.ClientBounds.Top;
                        }
                    }
                    page.SetClientBounds(
                        x + PrintPage.ClientLeftFix,
                        topCount,
                        page.ClientBounds.Width,
                        page.ClientBounds.Height);
                    topCount = topCount + page.ClientBounds.Height + PageSpacing;
                }//foreach
                base._ClientSizeWidth = (int)totalSizeWidth + 1;
                base._ClientSizeHeight = topCount + 1;
                if (this.UpdateCurrentPage())
                {
                    this.OnCurrentPageChanged();
                }
            }
            finally
            {
                _Doing_UpdatePages = false;
            }
        }

        /// <summary>
        /// 分页模式下的控件顶端预留空白高度
        /// </summary>
        protected virtual int PreserveSpacingInPageView()
        {
            return 0;
        }

        /// <summary>
        /// 处于页面视图模式时各个页面间的距离，像素为单位
        /// </summary>
        public const int PageSpacing = 20;

        /// <summary>
        /// 总页数
        /// </summary>
        public int PageCount
        {
            get
            {
                if (_Pages != null)
                {
                    return _Pages.Count;
                }
                else
                {
                    return 0;
                }
            }
        }

        #region 无效矩形区域控制代码群 ****************************************

        /// <summary>
        /// 使用视图坐标来声明无效区域,程序准备重新绘制无效文档
        /// </summary>
        /// <param name="ViewBounds">无效区域</param>
        public override void ViewInvalidate(DCSystem_Drawing.Rectangle ViewBounds)
        {
            ViewBounds = this.FixViewInvalidateRect(ViewBounds);
            if (!ViewBounds.IsEmpty)
            {
                MultiPageTransform trans = this.myTransform as MultiPageTransform;
                if (trans == null)
                {
                    base.ViewInvalidate(ViewBounds);
                }
                else
                {
                    foreach (SimpleRectangleTransform item in trans)
                    {
                        DCSystem_Drawing.Rectangle rect = DCSystem_Drawing.Rectangle.Intersect(item.DescRect, ViewBounds);
                        if (!rect.IsEmpty)
                        {
                            rect = item.UnTransformRectangle(rect);
                            this.Invalidate(rect);
                        }
                    }
                }
            }
        }

        #endregion

        /// <summary>
        /// 是否采用绝对的坐标转换
        /// </summary>
        public bool UseAbsTransformPoint
        {
            get
            {
                return this.PagesTransform.UseAbsTransformPoint;
            }
            set
            {
                this.PagesTransform.UseAbsTransformPoint = value;
            }
        }

        /// <summary>
        /// 设置或返回从1开始的当前页号
        /// </summary>
        public int PageIndex
        {
            get
            {
                if (_CurrentPage == null)
                {
                    return 0;
                }
                else
                {
                    return _CurrentPage.GlobalIndex;
                }
            }
            set
            {
                if (this._Pages != null && this._Pages.Count > 0)
                {
                    foreach (var page in this._Pages)
                    {
                        if (page.GlobalIndex == value)
                        {
                            this._CurrentPage = page;
                            break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 跳到指定页,页号从0开始计算。
        /// </summary>
        /// <param name="targetPageIndex">从0开始的页号</param>
        /// <returns>操作是否成功</returns>
        public bool MoveToPage(int targetPageIndex)
        {
            if (this.Pages != null && targetPageIndex >= 0 && targetPageIndex < this.Pages.Count)
            {
                PrintPage page = this.Pages[targetPageIndex];
                this._CurrentPage = page;
                this.Invalidate();
                return true;
            }
            return false;
        }

        protected int _PageMarginLineLength = 60;

        /// <summary>
        /// 创建页面框架绘制器
        /// </summary>
        /// <returns>绘制器</returns>
        protected virtual PageFrameDrawer CreatePageFrameDrawer()
        {
            return new PageFrameDrawer();
        }

        /// <summary>
        /// 绘制页面框架
        /// </summary>
        /// <param name="v_PrintPage">页面对象</param>
        /// <param name="g">图形绘制对象</param>
        /// <param name="ClipRectangle">剪切矩形</param>
        /// <param name="FillBackGround">是否填充背景</param>
        protected virtual void DrawPageFrame(
            object v_PrintPage,
            DCGraphics g,
            DCSystem_Drawing.Rectangle ClipRectangle,
            bool FillBackGround)
        {
            var myPage = (PrintPage)v_PrintPage;
            if (myPage == null || _Pages.Contains(myPage) == false)
            {
                return;
            }
            var drawer = CreatePageFrameDrawerForDrawPageFrame(myPage, g, ClipRectangle, FillBackGround);
            if (drawer != null)
            {
                drawer.DrawPageFrame(g, ClipRectangle);
            }
        }

        /// <summary>
		/// 绘制页面框架
		/// </summary>
		/// <param name="myPage">页面对象</param>
		/// <param name="g">图形绘制对象</param>
        /// <param name="ClipRectangle">剪切矩形</param>
		/// <param name="FillBackGround">是否填充背景</param>
		protected virtual PageFrameDrawer CreatePageFrameDrawerForDrawPageFrame(
            PrintPage myPage,
            DCGraphics g,
            DCSystem_Drawing.Rectangle ClipRectangle,
            bool FillBackGround)
        {
            if (myPage == null || _Pages.Contains(myPage) == false)
            {
                return null;
            }
            DCSystem_Drawing.Rectangle bounds = myPage.ClientBounds;
            var drawer = this.CreatePageFrameDrawer();
            drawer.Bounds = bounds;
            DCSystem_Drawing.Rectangle bodyBounds = DCSystem_Drawing.Rectangle.Empty;
            foreach (SimpleRectangleTransform item in this.PagesTransform)
            {
                if (item.PageObject == myPage)
                {
                    if (item.ContentStyle == PageContentPartyStyle.HeaderRows || item.ContentStyle == PageContentPartyStyle.Body)
                    {
                        if (bodyBounds.IsEmpty)
                        {
                            bodyBounds = item.PartialAreaSourceBounds;
                        }
                        else
                        {
                            bodyBounds = DCSystem_Drawing.Rectangle.Union(item.PartialAreaSourceBounds, bodyBounds);
                        }
                    }//if
                }//if
            }//foreach
            drawer.BodyBounds = bodyBounds;
            drawer.Margins = myPage.ClientMargins;
            drawer.FillBackGround = FillBackGround;
            if (myPage.IsEmpty)
            {
                drawer.MarginLineLength = 0;
            }
            else
            {
                drawer.MarginLineLength = this._PageMarginLineLength;
            }
            drawer.ZoomRate = this.XZoomRate;
            return drawer;
        }

        /// <summary>
        /// 已重载:绘制文档内容
        /// </summary>
        /// <param name="e">绘制事件参数</param>
        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        {
            base.OnPaint(e);
            InnerOnPaint(new InnerPaintEventArgs(e.Graphics, e.ClipRectangle), false);
        }

        protected void InnerOnPaint(InnerPaintEventArgs e, bool checkBodyPartialSourceRect = false)
        {
            DCSystem_Drawing.Rectangle clipRect = e.ClipRectangle;
            clipRect.Height += 1;
            this.RefreshScaleTransform();
            if (this.PagesTransform == null || this.PagesTransform.Count == 0)
            {
                // 没有任何内容可以绘制
                return;
            }
            MultiPageTransform trans = (MultiPageTransform)this.Transform;
            foreach (PrintPage myPage in this.Pages)
            {
                DCSystem_Drawing.Rectangle clientBounds = myPage.ClientBounds;
                if (clipRect.IntersectsWith(
                    new DCSystem_Drawing.Rectangle(
                        clientBounds.Left,
                        clientBounds.Top,
                        clientBounds.Width + 5,
                        clientBounds.Height + 5)))
                {
                    DrawPageFrame(
                        myPage,
                        e.Graphics,
                        clipRect,
                        true);
                    SimpleRectangleTransform bodyItem = null;
                    for (int iCount = trans.Count - 1; iCount >= 0; iCount--)
                    {
                        SimpleRectangleTransform item = trans[iCount];
                        if (item.Visible == false || item.PageObject != myPage)
                        {
                            continue;
                        }
                        if (item.ContentStyle == PageContentPartyStyle.Body)
                        {
                            bodyItem = item;
                        }
                        // 显示页眉页脚标记文本
                        if (item.ContentStyle == PageContentPartyStyle.Header && item.Enable)
                        {
                            // 绘制页眉标记
                            DrawHeaderFooterFlag(
                                DCSR.Header,
                                item.PartialAreaSourceBounds,
                                e.Graphics,
                                false);
                        }
                        if (item.ContentStyle == PageContentPartyStyle.Footer && item.Enable)
                        {
                            // 绘制页脚标记
                            DrawHeaderFooterFlag(
                                DCSR.Footer,
                                item.PartialAreaSourceBounds,
                                e.Graphics,
                                true);
                        }
                        DCSystem_Drawing.Rectangle rect = new DCSystem_Drawing.Rectangle(
                            (int)item.SourceRectF.Left,
                            (int)Math.Floor(item.SourceRectF.Top),
                            (int)item.SourceRectF.Width,
                            0);
                        rect.Height = (int)Math.Ceiling(item.SourceRectF.Bottom - rect.Top);
                        //DCSystem_Drawing.Rectangle rect2 = DCSystem_Drawing.Rectangle.Ceiling(item.SourceRectF);
                        if (checkBodyPartialSourceRect
                            && item.ContentStyle == PageContentPartyStyle.Body
                            && item.PartialAreaSourceBounds.IsEmpty == false)
                        {
                            rect = item.PartialAreaSourceBounds;
                        }
                        rect = DCSystem_Drawing.Rectangle.Intersect(
                            clipRect,
                            rect);
                        if (rect.IsEmpty == false)
                        {

                            var state2 = e.Graphics.Save();

                            var e2 = this.CreatePaintEventArgs(e, item, true);
                            if (e2 != null)
                            {
                                var e3 = new InnerPageDocumentPaintEventArgs(
                                    e2.Graphics,
                                    e2.ClipRectangle,
                                    myPage.Document,
                                    myPage,
                                    item.ContentStyle,
                                    item);
                                e3.ContentBounds = item.DescRect;
                                e3.PageClipRectangle = item.DescRectF;
                                if (item.PageClipBounds.IsEmpty == false)
                                {
                                    e3.PageClipRectangle = item.PageClipBounds;
                                }
                                e3.RenderMode = ContentRenderMode.UIPaint;
                                if (this._WASMPrintMode)
                                {
                                    e3.RenderMode = ContentRenderMode.Print;
                                }
                                e3.PageIndex = myPage.PageIndex;
                                e3.NumberOfPages = this.Pages.Count;
                                //e3.EditMode = this.EditMode;
                                if (myPage.Document != null)
                                {
                                    DrawDocumentContent(myPage.Document, e3, e);
                                    //myPage.Document.DrawContent(e3);
                                }//if
                            }

                            e.Graphics.Restore(state2);
                            OnAfterPaintItem(e, item);
                        }//if
                        // ClipRect.Height -= 1;
                    }//for (int iCount = trans.Count - 1; iCount >= 0; iCount--)
                }//if
            }//foreach (PrintPage myPage in this.Pages)
        }


        /// <summary>
        /// 完成绘制一个视图转换单元后执行的操作
        /// </summary>
        /// <param name="args">绘图事件参数</param>
        /// <param name="item">视图转换单元</param>
        protected virtual void OnAfterPaintItem(InnerPaintEventArgs args, SimpleRectangleTransform item)
        {
            if (item.Enable == false && item.DrawMaskWhenDisable)
            {
                // 若区域无效则用白色半透明进行覆盖，以作标记
                DCSystem_Drawing.Rectangle rect = item.SourceRect;
                {
                    using (var b = new SolidBrush(DCSystem_Drawing.Color.FromArgb(140, Color.White)))
                    {
                        args.Graphics.FillRectangle(
                            b,
                            rect.Left,
                            rect.Top,
                            rect.Width + 2,
                            rect.Height + 2);
                    }
                }

            }
        }

        protected virtual void DrawDocumentContent(
            DomDocument document,
            InnerPageDocumentPaintEventArgs args,
            InnerPaintEventArgs nativeArgs)
        {
            if (document != null)
            {
                document.DrawPageContent(args);
            }
        }

        /// <summary>
        /// 绘制页眉页脚使用的字体对象
        /// </summary>
        private static XFontValue _HeaderFooterFlagFont = null;
        /// <summary>
        /// 绘制页眉页脚标记
        /// </summary>
        /// <param name="flagText">标记文本</param>
        /// <param name="flagRect">边界区域</param>
        /// <param name="g">图形绘制对象</param>
        private void DrawHeaderFooterFlag(
            string flagText,
            DCSystem_Drawing.Rectangle flagRect,
            DCGraphics g,
            bool topMode)
        {
            if (_HeaderFooterFlagFont == null)
            {
                _HeaderFooterFlagFont = new XFontValue(
                    XFontValue.DefaultFontName,
                    10);
            }
            // 绘制页眉页脚边界
            using (Pen pen
                = new Pen(DCSystem_Drawing.Color.FromArgb(155, 187, 227)))
            {
                pen.DashStyle = DashStyle.Dash;
                //flagRect.Width = flagRect.Width;
                //flagRect.Height = flagRect.Height;
                g.DrawRectangle(pen, flagRect);
            }
            if (string.IsNullOrEmpty(flagText))
            {
                // 文本都不显示了，则边框也不显示
                return;
            }
            DCSystem_Drawing.SizeF size = g.MeasureString(
                flagText,
                _HeaderFooterFlagFont);
            DCSystem_Drawing.Size _HeaderFooterFlagSize = new DCSystem_Drawing.Size(
                (int)size.Width + 10,
                (int)size.Height + 10);
            DCSystem_Drawing.Rectangle labelRect = DCSystem_Drawing.Rectangle.Empty;
            // 绘制页眉页脚文本
            if (topMode)
            {
                // 在区域上面绘制
                labelRect = new DCSystem_Drawing.Rectangle(
                    flagRect.Left + 10,
                    flagRect.Top - _HeaderFooterFlagSize.Height,
                    _HeaderFooterFlagSize.Width,
                    _HeaderFooterFlagSize.Height);
            }
            else
            {
                // 在区域下面绘制
                labelRect = new DCSystem_Drawing.Rectangle(
                 flagRect.Left + 10,
                 flagRect.Bottom,
                 _HeaderFooterFlagSize.Width,
                 _HeaderFooterFlagSize.Height);
            }
            DrawLabel(
                g,
                _HeaderFooterFlagFont,
                flagText,
                DCSystem_Drawing.Color.FromArgb(21, 66, 139),
               DCSystem_Drawing.Color.FromArgb(216, 232, 245),
                DCSystem_Drawing.Color.FromArgb(155, 187, 227),
                labelRect);
        }

        private void DrawLabel(
            DCGraphics graphics,
            XFontValue font,
            string text,
            DCSystem_Drawing.Color textColor,
            DCSystem_Drawing.Color backColor,
            DCSystem_Drawing.Color borderColor,
           DCSystem_Drawing.Rectangle bounds)
        {
            using (var b4 = new SolidBrush(backColor))
            {
                graphics.FillRectangle(b4, bounds);
            }
            if (text != null && text.Length > 0)
            {
                using (var format = new StringFormat())
                {
                    format.Alignment = StringAlignment.Center;
                    format.LineAlignment = StringAlignment.Center;
                    format.FormatFlags = StringFormatFlags.NoWrap;
                    graphics.DrawString(
                            text,
                            font,
                            textColor,
                            new DCSystem_Drawing.RectangleF(
                                bounds.Left,
                                bounds.Top,
                                bounds.Width,
                                bounds.Height),
                            format);
                }//using
            }
            using (var p3 = new Pen(borderColor))
            {
                graphics.DrawRectangle(p3, bounds);
            }
        }
        public override void Dispose()
        {
            base.Dispose();
            this._Pages = null;
            this._CurrentPage = null;
            this._CurrentTransformItem = null;
            this.CurrentPageChanged = null;
        }
    }
}