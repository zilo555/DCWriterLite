using System;
using DCSoft.Drawing;
using DCSoft.Printing;
//using System.Drawing ;
using DCSoft.Writer.Controls;
using System.Collections.Generic;

namespace DCSoft.Writer.Dom
{
    internal class DocumentViewTransform : MultiPageTransform
    {
        public DocumentViewTransform()
        {
        }

        public static void UpdatePageHeaderRowsBounds(PrintPage page)
        {
            if (page.IsEmpty)
            {
                return;
            }
            if (page.HasHeaderRows)
            {
                var rows = page.HeaderRows;
                DomTableRowElement firstRow = (DomTableRowElement)rows[0];
                float headerRowsViewHeight = 0;
                foreach (DomTableRowElement row in rows)
                {
                    headerRowsViewHeight = headerRowsViewHeight + row.Height;
                }
                DomDocumentContentElement dce = firstRow.DocumentContentElement;
                page.HeaderRowsBounds = new RectangleF(
                    dce.GetAbsLeft(),
                    firstRow.GetAbsTop(),
                    dce.Width,
                    headerRowsViewHeight);
            }
            else
            {
                page.HeaderRowsBounds = RectangleF.Empty;
            }
        } 
        
        /// <summary>
        /// 是否扩展到页边距所占据的地方
        /// </summary>
        private const bool ExpentToMarginArea = true;

        //private static readonly List<SimpleRectangleTransform> _Page_OwneredTransformItems = new List<SimpleRectangleTransform>();
        /// <summary>
        /// 添加页面
        /// </summary>
        /// <param name="page">页面对象</param>
        /// <param name="pageTop">页面内容顶端位置</param>
        /// <param name="zoomRate">缩放比率</param>
        public override void AddPage(PrintPage page, float pageTop, float zoomRate)
        {
            if (page.IsEmpty)
            {
                return;
            }
            float expentWidthFix = 30;
            XPageSettings ps = page.PageSettings;
            DomDocument document = (DomDocument)page.Document;
            PageLayoutInfo layoutInfo = new PageLayoutInfo(page, true);
            float top = (int)pageTop + layoutInfo.TopMargin * zoomRate;
            expentWidthFix = Math.Min(expentWidthFix, layoutInfo.LeftMargin);
            expentWidthFix = Math.Min(expentWidthFix, layoutInfo.RightMargin);
            SimpleRectangleTransform headerItem = null;
            DomDocumentContentElement headerElement = document.Header;
            DomDocumentContentElement footerElement = document.Footer;
            float headerElementHeight = headerElement.Height ;
            float footerElementHeight = footerElement.Height ;
            float pageViewWidth = page.Width;
            float pageViewLeft = 0;
            WriterControl writerControl = document.EditorControl;
            {
                if (ExpentToMarginArea)
                {
                    pageViewLeft = -layoutInfo.LeftMargin + expentWidthFix;
                    pageViewWidth = page.Width + layoutInfo.LeftMargin + layoutInfo.RightMargin - expentWidthFix * 2;
                }
            }
            var psViewHeaderHeight = ps.ViewHeaderHeight;
            // 添加文档页眉视图映射
            headerItem = new SimpleRectangleTransform();
            var currentContentPartyStyle = document.CurrentContentPartyStyle;
            headerItem.Enable = (currentContentPartyStyle == headerElement.ContentPartyStyle);
            headerItem.PageIndex = page.PageIndex;
            headerItem.ContentStyle = headerElement.ContentPartyStyle;
            headerItem.PageObject = page;
            headerItem.DocumentObject = page.Document;
            // 映射到文档视图
            headerItem._DescRectF = new  RectangleF(
                pageViewLeft,
                0,
                pageViewWidth,
                // 如果当前编辑区域是页眉则设置页眉可视区域的高度为页眉内容高度和页眉标准高度的较大者
                Math.Max(psViewHeaderHeight - 1, headerElementHeight));
            if (headerElementHeight > psViewHeaderHeight)
            {
                top = top + (int)((headerElementHeight - psViewHeaderHeight) * zoomRate);
            }
            float viewHeaderDistance = layoutInfo.HeaderDistance * zoomRate;
            if (ExpentToMarginArea)
            {
                SetSourceRect(
                    headerItem,
                    zoomRate,
                    (PrintPage.ClientLeftFix + expentWidthFix * zoomRate),
                    (pageTop + viewHeaderDistance));
            }
            else
            {
                SetSourceRect(
                    headerItem,
                    zoomRate,
                    layoutInfo.LeftMargin * zoomRate + PrintPage.ClientLeftFix,
                    (pageTop + viewHeaderDistance));
            }

            headerItem.PartialAreaSourceBounds = headerItem.SourceRect;
            int bodyTopFix = 0;
            float headerRowsViewHeight = 0;
            SimpleRectangleTransform headerRowItem = null;
            if (page.HasHeaderRows)
            {
                // 添加标题行文档映射
                var rows = page.HeaderRows;
                headerRowItem = new SimpleRectangleTransform();
                headerRowItem.Enable = false;
                headerRowItem.DrawMaskWhenDisable = false;
                if (DrawerUtil.IsHeaderFooter(currentContentPartyStyle))
                {
                    headerRowItem.DrawMaskWhenDisable = true;
                }
                var firstRow = (DomTableRowElement)rows[0];
                foreach (DomTableRowElement row in rows)
                {
                    headerRowsViewHeight = headerRowsViewHeight + row.Height;
                }
                headerRowItem.PageIndex = page.PageIndex;
                headerRowItem.ContentStyle = PageContentPartyStyle.HeaderRows;
                headerRowItem.PageObject = page;
                headerRowItem.DocumentObject = page.Document;
                // 映射到文档视图
                headerRowItem.DescRectF = new RectangleF(
                    pageViewLeft,
                    firstRow.GetAbsTop(),
                    pageViewWidth,
                    headerRowsViewHeight);
                page.HeaderRowsBounds = headerRowItem.DescRectF;

                if (headerElementHeight > psViewHeaderHeight - 10 && bodyTopFix == 0) //if (document.Header.Height > ps.ViewHeaderHeight)
                {
                    bodyTopFix = 5; ;// 当页眉实际高度大于标准页眉高度，页眉内容突出了标准区域，此时为了美观，页眉和页身之间留点空隙
                }
                if (ExpentToMarginArea)
                {
                    top = SetSourceRect(
                        headerRowItem,
                        zoomRate,
                        (PrintPage.ClientLeftFix + expentWidthFix * zoomRate),
                        headerItem.PartialAreaSourceBounds.Bottom + bodyTopFix);
                }
                else
                {
                    top = SetSourceRect(
                        headerRowItem,
                        zoomRate,
                        layoutInfo.LeftMargin * zoomRate + PrintPage.ClientLeftFix,
                        headerItem.PartialAreaSourceBounds.Bottom + bodyTopFix);
                }
                headerRowItem.PartialAreaSourceBounds = new Rectangle(
                    (int)headerRowItem._SourceRectF.Left,
                    (int)headerRowItem._SourceRectF.Top,
                    (int)headerRowItem._SourceRectF.Width,
                    (int)(headerRowsViewHeight * zoomRate));
                //totalPartialAreaHeight += headerRowItem.PartialAreaSourceBounds.Height;
            }
            bool lastPage = page == document.Pages.LastPage;
            float bodyContentHeight = page.Height;
            var pageStandartPapeBodyHeight = page.StandartPapeBodyHeight;
            if (lastPage)
            {
                bodyContentHeight = pageStandartPapeBodyHeight - 5;// document.GetStandartPapeViewHeight(ps) - 5;
            }
            // 添加正文文档映射
            SimpleRectangleTransform bodyItem = new SimpleRectangleTransform();
            bodyItem.Enable = (currentContentPartyStyle == PageContentPartyStyle.Body);
            bodyItem.PageIndex = page.PageIndex;
            bodyItem.ContentStyle = PageContentPartyStyle.Body;
            bodyItem.PageObject = page;
            bodyItem.DocumentObject = page.Document;

            // 映射到文档视图
            bodyItem._DescRectF = new  RectangleF(
                pageViewLeft,
                page.Top,
                pageViewWidth,
                bodyContentHeight);
            //int spacing = 0;
            if (headerElementHeight > psViewHeaderHeight - 10 && bodyTopFix == 0)
            {
                bodyTopFix = 3;// 当页眉实际高度大于标准页眉高度，页眉内容突出了标准区域，此时为了美观，页眉和页身之间留点空隙
            }
            if (headerRowsViewHeight > 0)
            {
                //遇到表格标题行，body块稍微往上移动一点。
                bodyTopFix -= 1;
            }
            if (ExpentToMarginArea)
            {
                top = SetSourceRect(
                    bodyItem,
                    zoomRate,
                    (float)(PrintPage.ClientLeftFix + expentWidthFix * zoomRate),
                    (float)(headerItem.PartialAreaSourceBounds.Bottom + bodyTopFix + headerRowsViewHeight * zoomRate));
            }
            else
            {
                top = SetSourceRect(
                     bodyItem,
                     zoomRate,
                     layoutInfo.LeftMargin * zoomRate + PrintPage.ClientLeftFix,
                     (float)Math.Floor(headerItem.PartialAreaSourceBounds.Bottom + bodyTopFix + headerRowsViewHeight * zoomRate));
            }
            bodyItem.PartialAreaSourceBounds = new Rectangle(
                (int)bodyItem._SourceRectF.Left,
                (int)bodyItem._SourceRectF.Top,
                (int)bodyItem._SourceRectF.Width,
                (int)(pageStandartPapeBodyHeight * zoomRate - headerRowsViewHeight * zoomRate));
            //totalPartialAreaHeight += bodyItem.PartialAreaSourceBounds.Height;
            {
                bodyItem.PageClipBounds = new RectangleF(
                    bodyItem._DescRectF.Left,
                    bodyItem._DescRectF.Top,
                    bodyItem._DescRectF.Width,
                    page.StandartPapeBodyHeight - headerRowsViewHeight);
            }
            // 添加页脚文档视图映射
            SimpleRectangleTransform footerItem = new SimpleRectangleTransform();
            footerItem.Enable = (currentContentPartyStyle == footerElement.ContentPartyStyle);
            footerItem.PageIndex = page.PageIndex;
            footerItem.ContentStyle = footerElement.ContentPartyStyle;
            footerItem.PageObject = page;
            footerItem.DocumentObject = page.Document;
            int headerTop = 0;
            // 映射到文档视图
            int pagePixelHeight = (int)(layoutInfo.PageHeight * zoomRate);// (int)( page.ViewPaperHeight * zoomRate);
            {
                footerItem._DescRectF = new RectangleF(
                    pageViewLeft,
                    0,
                    pageViewWidth,
                    // 如果当前编辑区域是页脚则设置页脚可视区域的高度为页脚内容高度和页脚标准高度的较大者
                    footerElementHeight);
                {
                    float viewFooterDistance = layoutInfo.FooterDistance * zoomRate;
                    headerTop = (int)(pageTop + pagePixelHeight - viewFooterDistance - footerElementHeight * zoomRate);
                    if (headerTop < bodyItem.PartialAreaSourceBounds.Bottom)
                    {
                        headerTop = bodyItem.PartialAreaSourceBounds.Bottom;
                    }
                }
            }
            if (ExpentToMarginArea)
            {
                SetSourceRect(
                     footerItem,
                     zoomRate,
                     (int)(PrintPage.ClientLeftFix + expentWidthFix * zoomRate),
                     headerTop);
            }
            else
            {
                SetSourceRect(
                    footerItem,
                    zoomRate,
                    layoutInfo.LeftMargin * zoomRate + PrintPage.ClientLeftFix,
                    headerTop);
            }
            Rectangle footerPSB = footerItem.SourceRect;
            if (footerPSB.Height < 25)
            {
                footerPSB.Height = 25;
            }
            Rectangle fpa = new Rectangle(
                (int)footerItem._SourceRectF.Left,
                (int)(pageTop + pagePixelHeight - layoutInfo.BottomMargin * zoomRate),
                (int)footerItem._SourceRectF.Width,
                (int)footerItem._SourceRectF.Height);
            fpa = Rectangle.Union(fpa, footerPSB);
            if (fpa.Bottom > footerItem.SourceRectF.Bottom)
            {
                fpa.Height = (int)(footerItem.SourceRectF.Bottom - fpa.Top);
            }
            footerItem.PartialAreaSourceBounds = fpa;
            switch (currentContentPartyStyle)
            {
                case PageContentPartyStyle.Header:
                    {
                        if (headerItem != null)
                        {
                            this.Add(headerItem);
                        }
                        if (headerRowItem != null)
                        {
                            this.Add(headerRowItem);
                        }
                    }
                    this.Add(bodyItem);
                    if (footerItem != null)
                    {
                        this.Add(footerItem);
                    }
                    break;
                case PageContentPartyStyle.Body:
                    this.Add(bodyItem);
                    if (headerRowItem != null)
                    {
                        this.Add(headerRowItem);
                        //_Page_OwneredTransformItems.Add(headerRowItem);
                    }
                    {
                        if (headerItem != null )
                        {
                            this.Add(headerItem);
                        }
                        if (footerItem != null)
                        {
                            this.Add(footerItem);
                        }
                    }
                    break;
                case PageContentPartyStyle.Footer:
                    {
                        if (footerItem != null )
                        {
                            this.Add(footerItem);
                        }
                        if (headerItem != null )
                        {
                            this.Add(headerItem);
                        }
                    }
                    if (headerRowItem != null)
                    {
                        this.Add(headerRowItem);
                    }
                    this.Add(bodyItem);
                    break;
            }//switch
            //page.OwneredTransformItems = _Page_OwneredTransformItems.ToArray();
            //_Page_OwneredTransformItems.Clear();
        }

        private float SetSourceRect(
            SimpleRectangleTransform item,
            float zoomRate,
            float left,
            float top)
        {
             RectangleF rect = new RectangleF();
            rect.X = (int)left;
            rect.Y = top;
            rect.Width = (item._DescRectF.Width * zoomRate);
            rect.Height = (item._DescRectF.Height * zoomRate);
            // 映射到控件客户区
            item._SourceRectF = rect;
            return rect.Bottom;// ( int ) Math.Ceiling( top + rect.Height );
        }
    }
}