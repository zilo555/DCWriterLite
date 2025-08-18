using System;
using DCSoft.Printing;
using System.Reflection;


//using DCSoft.Writer.Dom.Undo;
using DCSoft.Writer.Serialization;


using System.ComponentModel;
using DCSoft.Drawing;
using DCSoft.Common;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using DCSoft.Writer.Data;
using System.Text;
using System.Runtime.InteropServices;

using DCSoft.Writer.Controls;

namespace DCSoft.Writer.Dom
{
    partial class DomDocument
    {
        /// <summary>
        /// 由于打印或者打印预览而锁定了文档内容排版
        /// </summary>
        internal bool FixLayoutForPrint = false;

        [NonSerialized]
        internal DCGridLineInfo _GlobalGridInfo = null;

        /// <summary>
        /// 获得全局性的文档网格线排版设置
        /// </summary>
        /// <returns></returns>
        internal DCGridLineInfo InnerGlobalGridInfo()
        {
             return _GlobalGridInfo;
        }

        /// <summary>
        /// 获得全局性的文档网格线设置
        /// </summary>
        /// <returns>设置信息对象</returns>
        private DCGridLineInfo GetGlobalGridLineInfo()
        {
            XPageSettings ps = this.PageSettings;
            if( ps != null
                && ps.PowerDocumentGridLine
                && ps.DocumentGridLine != null 
                && ps.DocumentGridLine.Visible 
                && ps.DocumentGridLine.GridNumInOnePage > 0)
            {
                {
                    return ps.DocumentGridLine;
                }
            }
            return null;
        }

         
        public override void EditorRefreshView()
        {
            if (this.EditorControl != null)
            {
                this.EditorControl.RefreshDocumentExt(true, true);
            }
            else
            {
                this.PageRefreshed = false;
                this.CheckPageRefreshed();
            }
        }

        /// <summary>
        /// 检查文档内容排版和布局
        /// </summary>
       //[System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public void CheckPageRefreshed()
        {
            if (this.PageRefreshed == false )
            {
                this.RefreshContent(false , true );
                this.RefreshSizeWithoutParamter();
                this.InnerExecuteLayout();
                this.RefreshPages();
                this.SetReadyState( DomReadyStates.Complete );   
            }
        }

        /// <summary>
        /// 刷新文档内部排版和分页。不更新用户界面。
        /// </summary>
        /// <param name="bolFastMode">是否为快速模式，若为快速模式，则不刷新文档视图和用户界面</param>
        public void RefreshInnerView(bool bolFastMode)
        {
            this._InvalidateLayoutFast = false;
            CheckDisposed();
            WriterControl ctlBack = this.EditorControl;
            DocumentStates stateBack = null;
            this.EditorControl = null;
            try
            {
                if (bolFastMode)
                {
                    this.RefreshContent(true, false);
                }
                else
                {
                    this.RefreshContent(false, true);
                }
                using (DCGraphics g = this.InnerCreateDCGraphics())
                {
                    this.InnerRefreshSize(g, bolFastMode);
                    this.InnerExecuteLayout();
                    this.InnerRefreshPages(bolFastMode);
                    this.EditorControl = ctlBack;
                }
                this.SetReadyState(DomReadyStates.Complete);
            }
            finally
            {
                if( stateBack != null )
                {
                    this._States = stateBack;
                }
                this.EditorControl = ctlBack;
            }
        }

        /// <summary>
        /// 无参数的重新计算文件元素大小
        /// </summary>
        public void RefreshSizeWithoutParamter()
        {
            using (DCGraphics g = this.InnerCreateDCGraphics())
            {
                InnerRefreshSize(g);
            }
        }

        /// <summary>
        /// 段落符号元素的大小
        /// </summary>
        internal SizeF _ParagraphFlagSize = SizeF.Empty;
        /// <summary>
        /// 段落符号元素的显示宽度
        /// </summary>
        [NonSerialized]
        internal static float _ParagraphFlagViewWidth = 0f;
        /// <summary>
        /// 计算段落符号元素的大小
        /// </summary>
        /// <param name="eof">元素对象</param>
        /// <param name="g">参数</param>
        private void RefreshParagraphFlagSize( DCGraphics g)
        {
            //int h = (int)Math.Ceiling((double)this.DefaultStyle.DefaultLineHeight);
            DocumentContentStyle rs = this.DefaultStyle;
            //SizeF size = g.MeasureString("#", rs.Font);
            this._ParagraphFlagSize.Height = g.GetFontHeight(rs.Font);
            this._ParagraphFlagSize.Width = this.PixelToDocumentUnit(2);
            _ParagraphFlagViewWidth = this.PixelToDocumentUnit(9);
        }
        /// <summary>
        /// 内部的计算文档元素大小
        /// </summary>
        /// <param name="g">画布对象</param>
        /// <param name="fastMode">是否为快速模式</param>
        public void InnerRefreshSize(DCGraphics g, bool fastMode = false)
        {
            CheckDisposed();
            //GraphicsUnitConvert.SetDpi(g.GraphisForMeasure);
            this.CacheOptions();
            //this.ContentStyles.ResetFastValue();
            RefreshParagraphFlagSize(g);
            this.Width = this.PageSettings.ViewPaperWidth;
            g.PageUnit = DCSystem_Drawing.GraphicsUnit.Document;
            //this.ContentStyles.UpdateState(g);
            //XTextContentRender view = this.Render;
            var vRenderMode = InnerDocumentRenderMode.Paint;
            if (this.States.Printing)
            {
                vRenderMode = InnerDocumentRenderMode.Print;
            }
            InnerDocumentPaintEventArgs args = CreateInnerPaintEventArgs(g, vRenderMode);
            this.ContentStyles.UpdateState(args.Graphics);
            if (fastMode)
            {
                args.CheckSizeInvalidateWhenRefreshSize = true;
            }

            this.Render.ResetStateForDocument(g);
            float pw = this.PageSettings.ViewClientWidth;
            foreach (DomElement dce in this.Elements.FastForEach())
            {
                dce.Width = pw;
            }
            base.RefreshSize(args);
            this.PageRefreshed = false;
            this.ClearCachedOptions();
        }


        /// <summary>
        /// 采用视图单位的标准的页面高度,DCWriter内部使用。
        /// </summary>
        public float GetStandartPapeViewHeight(XPageSettings ps)
        {
            float height = ps.ViewClientHeight;
            float hh = this.Header.Height;
            float vhh = ps.ViewHeaderHeight;
            if ( hh > vhh ) //this.Header.Height > ps.ViewHeaderHeight)
            {
                height = height - (hh - vhh);// (this.Header.Height - ps.ViewHeaderHeight);
            }
            float fh = this.Footer.Height;
            float vfh = ps.ViewFooterHeight;
            if ( fh > vfh )// this.Footer.Height > ps.ViewFooterHeight)
            {
                height = height - (fh - vfh);// (this.Footer.Height - ps.ViewFooterHeight);
            }
            return height;
        }

        /// <summary>
        /// 重新进行分页
        /// </summary>
         
        public void RefreshPages( )
        {
            InnerRefreshPages(false);
        }

        /// <summary>
        /// 文档分页状态锁定标记
        /// </summary>
        [NonSerialized]
        private bool _LockPageState = false;

        private void InnerRefreshPages_HandledNewPage(
            DomContainerElement rootElement, 
            List<DomPageBreakElement> pbElements , 
            List<DomContentElement> contentElements ,
            List<DomPageInfoElement> pageInfoElements )
        {
            var arr1 = rootElement.GetCompressedElements();
            if( arr1 == null )
            {
                return;
            }
            var len1 = arr1.Count;
            //foreach (XTextElement element in rootElement.Elements.FastForEach())
            for(var iCount1 = 0; iCount1 < len1;iCount1 ++)
            {
                var element = arr1[iCount1];
                if (element is DomTableElement)
                {
                    var table = (DomTableElement)element;
                    table.SetContainsUnHandledPageBreak(false);
                    var rowArr = table.Rows.InnerGetArrayRaw();
                    var rowCount = table.Rows.Count;
                    var nc = contentElements.Count + table.Rows.Count * table.Columns.Count + 10;
                    if(contentElements.Capacity < nc )
                    {
                        contentElements.Capacity = nc;
                    }
                    //foreach (XTextTableRowElement row in table.Rows.FastForEach())
                    for(var rowIndex = 0; rowIndex < rowCount; rowIndex ++)
                    {
                        var row = (DomTableRowElement)rowArr[rowIndex];
                        if (row._RuntimeVisible)
                        {
                            row.HandledNewPage = false;
                            row.SetContainsUnHandledPageBreak(false);
                            var cellArr = row.Cells.InnerGetArrayRaw();
                            var cellCount = row.Cells.Count;
                            //foreach (XTextTableCellElement cell in row.Cells.FastForEach())
                            for(var cellIndex = 0; cellIndex < cellCount; cellIndex ++)
                            {
                                var cell = (DomTableCellElement)cellArr[cellIndex];
                                if (cell.RuntimeVisible)
                                {
                                    cell.SetContainsUnHandledPageBreak(false);
                                    contentElements.Add(cell);
                                    InnerRefreshPages_HandledNewPage(
                                        cell, 
                                        pbElements ,
                                        contentElements,
                                        pageInfoElements);
                                }
                            }
                        }
                    }
                }
                else if (element is DomContainerElement)
                {
                    if( element is DomContentElement)
                    {
                        contentElements.Add((DomContentElement)element);
                    }
                    // 重置所有容器元素对象的ContainsUnHandledPageBreak属性值
                    DomContainerElement c = (DomContainerElement)element;
                    c.SetContainsUnHandledPageBreak(false);
                    //if (c is XTextTableRowElement)
                    //{
                    //    ((XTextTableRowElement)c).HandledNewPage = false;
                    //}
                    if (c.RuntimeVisible && c.HasElements())
                    {
                        InnerRefreshPages_HandledNewPage(
                            c, 
                            pbElements , 
                            contentElements,
                            pageInfoElements);
                    }
                }
                else if (element is DomPageBreakElement)
                {
                    //处理所有的分页符元素
                    DomPageBreakElement pb = (DomPageBreakElement)element;
                    pb.Handled = false;
                    if (pb.RuntimeVisible)
                    {
                        pbElements.Add(pb);
                    }
                }
                else if( element is DomPageInfoElement )
                {
                    pageInfoElements.Add((DomPageInfoElement)element);
                }
            }//foreach
        }
         
        private void GetXTextPageInfoElementList(DomContainerElement rootElement, List<DomPageInfoElement> list)
        {
            var arr = rootElement.Elements.InnerGetArrayRaw();
            var len = rootElement.Elements.Count;
            //foreach (var element in rootElement.Elements.FastForEach())
            for (var iCount = 0; iCount < len; iCount++)
            {
                var element = arr[iCount];
                if (element is DomPageInfoElement)
                {
                    if (element._RuntimeVisible)
                    {
                        list.Add((DomPageInfoElement)element);
                    }
                }
                else if (element is DomContainerElement && element.RuntimeVisible)
                {
                    GetXTextPageInfoElementList((DomContainerElement)element, list);
                }
            }
        }

        /// <summary>
        /// 正在执行分页操作
        /// </summary>
        [ThreadStatic]
        private static bool _Processing_InnerRefreshPages = false;
        internal static bool Processing_InnerRefreshPages
        {
            get
            {
                return _Processing_InnerRefreshPages;
            }
        }
        

        [NonSerialized]
        private List<DomContentElement> _Elements_FixLinePositionForPageLine = null;

        private void InnerRefreshPages(bool fastMode)
        {
            if (this._LockPageState)
            {
                DCConsole.Default.WriteLine("JIEJIE.NET.SWITCH:-controlfow");
                throw new System.Exception(DCSR.PageStateLocked);
            }
            //this._PagesForNormalViewMode = null;
            CheckDisposed();
            if (fastMode == false)
            {
                if (_Processing_InnerRefreshPages)
                {
                    // 若检测到递归则退出处理
                    return;
                }
            }
            _Processing_InnerRefreshPages = true;
            try
            {
                //float tick = CountDown.GetTickCountFloat();
                if(this._Elements_FixLinePositionForPageLine != null && this._Elements_FixLinePositionForPageLine.Count > 0 )
                {
                    foreach( var element in this._Elements_FixLinePositionForPageLine)
                    {
                        element.ClearFixLinePositionForPageLine();
                    }
                }
                DomDocumentBodyElement body = this.Body;
                List<DomPageBreakElement> pbElements = new List<DomPageBreakElement>();
                var bodyContentElements = new List<DomContentElement>();
                bodyContentElements.Add(body);
                var pageInfoElements = new List<DomPageInfoElement>();
                InnerRefreshPages_HandledNewPage(body, pbElements, bodyContentElements, pageInfoElements);
                // 获得文档中的页码元素
                ////var pageInfos = new List<XTextPageInfoElement>();// this.GetElementsByType(typeof(XTextPageInfoElement));
                //foreach (var element in this.Elements.FastForEach())
                //{
                //    if (element != body)
                //    {
                //        GetXTextPageInfoElementList((XTextContainerElement)element, pageInfoElements);
                //    }
                //}
                foreach (DomDocumentContentElement ce in this.Elements.FastForEach())
                {
                    if( ce != body )
                    {
                        GetXTextPageInfoElementList(ce, pageInfoElements);
                    }
                    ce.Top = 0;
                    //float topCount = ce.Top;
                    var lineArr = ce.Lines.InnerGetArrayRaw();
                    //foreach (XTextLine line in ce.Lines.FastForEach())
                    for(var iCount = ce.Lines.Count -1; iCount >=0;iCount --)
                    {
                        lineArr[iCount]._OwnerPage = null;
                        //line._OwnerPage = null;
                        //if (line[0] is XTextPageBreakElement)
                        //{
                        //    ((XTextPageBreakElement)line[0]).Handled = false;
                        //}
                    }
                    lineArr = ce.PrivateLines.InnerGetArrayRaw();
                    for(var iCount = ce.PrivateLines.Count -1;iCount >=0;iCount --)
                    {
                        lineArr[iCount]._OwnerPage = null;
                    }
                    //foreach (XTextLine line in ce.PrivateLines.FastForEach())
                    //{
                    //    line._OwnerPage = null;
                    //}
                    //ce.Height = topCount;
                }//foreach
                
                PrintPageCollection documentPages = this.Pages;
                
                XPageSettings documentPageSettings = this.PageSettings;
                documentPages.Clear();
                this._CurrentPage = null;
                if (this.InnerViewControl != null)
                {
                    this.InnerViewControl.SetCurrentPageRaw(null);
                }
                //myPages.Reset( g );

                documentPages.Top = 0;
                //float bodyHeight = body.Height;
                float lastPos = _Pages.Top;
                //this.Pages.MinPageHeight = 15 ;
                //_RawPageIndex = 0;
                this._CurrentPage = null;
                PageLineInfo info = new PageLineInfo();
                info.PageSettings = documentPageSettings;
                info.StdPageContentHeight = documentPageSettings.ViewClientHeight;
                info.MinPageContentHeight = 50;
                info.PageBreakElements = pbElements;
                if (info.StdPageContentHeight < info.MinPageContentHeight * 2)
                {
                    throw new Exception(DCSR.InvalidatePageSettings);
                }
                //XTextElementList lastHeaderRows = null;
                //float v = body.Elements[0].AbsTop;
                // 开始循环计算分页线的位置
                int absBodyBottom = (int)(body.GetAbsTop() + body.Height);
                bool lastForNewPageFlag = false;
                body.RecalculateLineAbsPosition();
                float standartPapeBodyHeight = -1;
                var headerElement = this.Header;
                var footerElement = this.Footer;

               
                while (documentPages.Height < absBodyBottom)
                {
                    if (pbElements.Count > 0)
                    {
                        // 更新分页符所在容器元素的ContainsUnHandledPageBreak属性值
                        foreach (DomPageBreakElement pb in pbElements)
                        {
                            //if (pb.Handled == true)
                            {
                                DomContainerElement c = pb.Parent;
                                while (c != null)
                                {
                                    c.SetContainsUnHandledPageBreak(false);
                                    c = c.Parent;
                                }
                            }
                        }
                        foreach (DomPageBreakElement pb in pbElements)
                        {
                            if (pb.Handled == false)
                            {
                                DomContainerElement c = pb.Parent;
                                while (c != null)
                                {
                                    c.SetContainsUnHandledPageBreak(true);
                                    c = c.Parent;
                                }
                            }
                        }//foreach
                    }
                    PrintPage page = new PrintPage(
                        this,
                        documentPageSettings,//.Clone(),
                        documentPages,
                        0,
                        0);
                    page.OddPageIndex = (documentPages.Count % 2) == 1;
                    page.Margins = documentPageSettings.Margins;
                    page.ForNewPage = lastForNewPageFlag;
                            page.HeaderContentHeight = headerElement.Height;
                            page.FooterContentHeight = footerElement.Height;
                    // 设置文档页的标准页高
                    if( standartPapeBodyHeight < 0 )
                    {
                        standartPapeBodyHeight = page.StandartPapeBodyHeight;
                    }
                    page.SetHeightRaw(standartPapeBodyHeight);
                    //page.SetHeightRaw(page.StandartPapeBodyHeight);// GetStandartPapeViewHeight(this.PageSettings);
                    if (info.HeaderRows.Count > 0 )
                    {
                        page.HeaderRows = info.HeaderRows.ToArray();
                        DocumentViewTransform.UpdatePageHeaderRowsBounds(page);
                        float rh = 0;
                        foreach (DomTableRowElement row in info.HeaderRows)
                        {
                            rh += row.Height;
                        }//foreach
                        page.Height = page.Height - rh;
                        info.HeaderRows.Clear();
                    }
                    //if (bolNormalViewMode)
                    //{
                    //    // 普通视图模式
                    //    page.SetHeightRaw(absBodyBottom);
                    //    documentPages.Add(page);
                    //    break;
                    //}
                    if (page.Height < 50)
                    {
                        // 若标准页高小于50则页面设置可能错误，退出处理
                        break;
                    }
                    page.DocumentHeight = this.Height;
                    info.BodyLines = this.Body.Lines;
                    info.CurrentPageIndex = documentPages.Count;
                    info.LastPosition = page.Top;
                    info._CurrentPoistion = page.Bottom;
                    info.ForNewPage = false;
                    info.CutLineCells.Clear();
                    info.HeaderRows.Clear();
                    body.FixPageLine(info);
                    if (info.CutLineCells != null && info.CutLineCells.Count > 0)
                    {
                        // 单元格被分页线分割了，可能需要移动一些文档行来避免被分页线分割
                        foreach (var cell in info.CutLineCells)
                        {
                            if(cell.FixLinePositionForPageLine(info.CurrentPoistion))
                            {
                                if( this._Elements_FixLinePositionForPageLine == null )
                                {
                                    this._Elements_FixLinePositionForPageLine = new List<DomContentElement>();
                                }
                                this._Elements_FixLinePositionForPageLine.Add(cell);
                            }
                        }
                    }
                    if (info.SourceElements.Count > 1)
                    {
                        // 修正分页线的文档元素有多个，说明出现了有内容容器元素曾经
                        // 进行过分页但后来被其他元素修改了，此时这些内容容器元素需要
                        // 针对分页线进行文档行位置的微调
                        //List<XTextElement> handledElements = new List<XTextElement>();
                        for (int iCount = 0; iCount < info.SourceElements.Count - 1; iCount++)
                        {
                            DomContentElement ce = null;
                            DomElement element = info.SourceElements[iCount];
                            if (element is DomContentElement)
                            {
                                ce = (DomContentElement)element;
                            }
                            else
                            {
                                ce = element.ContentElement;
                            }
                            if (ce == null)
                            {
                            }
                            else
                            {
                                //if (handledElements.Contains(ce) == false)
                                //{
                                //    handledElements.Add(ce);
                                //    //if ((ce is XTextDocumentContentElement) == false)
                                //    //{
                                //    //    if( this._FixContentForPageLineElements == null )
                                //    //    {
                                //    //        this._FixContentForPageLineElements = new Dictionary<XTextContentElement, object>();
                                //    //        this._FixContentForPageLineElements[ce] = null;
                                //    //    }
                                //    //    else if( this._FixContentForPageLineElements.ContainsKey( ce ) == false )
                                //    //    {
                                //    //        this._FixContentForPageLineElements[ce] = null;
                                //    //    }
                                //    //    //if ( this._FixContentForPageLineElements == null 
                                //    //    //    || this._FixContentForPageLineElements.ContainsKey(ce) == false)
                                //    //    //{
                                //    //    //    if( this._FixContentForPageLineElements == null )
                                //    //    //    {
                                //    //    //        this._FixContentForPageLineElements = new Dictionary<XTextContentElement, object>();
                                //    //    //    }
                                //    //    //    try
                                //    //    //    {
                                //    //    //        this._FixContentForPageLineElements[ce] = null;
                                //    //    //    }
                                //    //    //    catch( System.Exception ext )
                                //    //    //    {

                                //    //    //    }
                                //    //    //    //this.FixContentForPageLineElements().Add(ce);
                                //    //    //}
                                //    //}
                                //}
                            }
                        }
                    }
                    if( info.SourceElement is DomPageBreakElement)
                    {
                        // 被分页符强制分页的
                        page.EndByPageBreak = true;
                    }
                    info.SourceElement = null;
                    info.SourceElements.Clear();
                    page.Height = info.CurrentPoistion - page.Top;
                    lastForNewPageFlag = info.ForNewPage;
                    if ((info.SourceElement is DomPageBreakElement) == false)
                    {
                        // 导致分页的元素不是分页符元素，则进行页面高度最小值判断
                        if (page.Height < info.MinPageContentHeight)
                        {
                            page.Height = page.ViewStandardHeight;
                        }
                    }
                    lastPos = page.Bottom;
                    documentPages.Add(page);
                }//while
                info.Clear();
                info.Dispose();
                pbElements.Clear();
                pbElements = null;

                if (documentPages.Count > 0)
                {
                    body.GlobalUpdateLineIndex();
                    documentPages.LastPage.Height =
                        (documentPages.LastPage.Height - (documentPages.Height - body.GetAbsTop() - body.Height));
                }

                //foreach (PrintPage page in documentPages)
                //{
                //    //System.Diagnostics.Debug.WriteLine(page.Height.ToString());
                //}

                this.PageRefreshed = true;
                if (this.Info != null)
                {
                    this.Info.NumOfPage = documentPages.Count;
                }
                //// 为分页线而细微的调整各个文档块内部的文档行的位置
                //if ( this._FixContentForPageLineElements != null
                //    && this._FixContentForPageLineElements.Count > 0 )// this.FixContentForPageLineElements().Count > 0)
                //{
                //    foreach (var item in this._FixContentForPageLineElements)
                //    {
                //        if (item.Key.AlignLineHeightToGridLine == false)
                //        {
                //            item.Key.FixLinePositionForPageLine();
                //        }
                //    }
                //    //// 缓存文档正文中文档行的顶端位置
                //    //foreach (XTextLine line in allLines)
                //    //{
                //    //    //line.UpdateAbsLocation();
                //    //    //line.AbsTopBuffered = -1f;
                //    //    //line.AbsTopBuffered = line.AbsTop;
                //    //}
                //}
                foreach (DomContentElement ce in bodyContentElements)// this.Body.ContentElements)
                {
                    ce.RefreshPrivateLinesOwnerPage();
                }

                bodyContentElements.Clear();
                bodyContentElements = null;

                body.GlobalUpdateLineIndex();

                //// 取消文档正文中文档行的顶端位置
                //foreach (XTextLine line in allLines)
                //{
                //    //line.UpdateAbsLocation();
                //    //line.AbsTopBuffered = -1f;
                //}

                // 更新文档中的页码元素
                //GetXTextPageInfoElementList(this, pageInfos);
                if (pageInfoElements != null && pageInfoElements.Count > 0)
                {
                    using (DCGraphics g = this.InnerCreateDCGraphics())
                    {
                        InnerDocumentPaintEventArgs args = CreateInnerPaintEventArgs(g);
                        foreach (DomPageInfoElement field in pageInfoElements)
                        {
                            args.Element = field;
                            float oldWidth = field.Width;
                            //float oldHeight = field.Height;
                            field.RefreshSize(args);
                            float widthFix = field.Width - oldWidth;
                            //float heightFix = field.Height - oldHeight;
                            if (widthFix != 0)
                            {
                                // 元素宽度发生改变,则修正同行后面元素的位置
                                DomLine line = field.OwnerLine;
                                for (int iCount = line.IndexOf(field) + 1; iCount < line.Count; iCount++)
                                {
                                    line[iCount].Left += widthFix;
                                }
                            }//if
                        }//foreach
                    }//using
                }
                this.States.Layouted = true;
            }
            finally
            {
                _Processing_InnerRefreshPages = false;
            }
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
            InnerRefreshDocumentExt(refreshSize, executeLayout, null);
        }

        private void InnerResetRuntimeStyle( DomContainerElement rootElement )
        {
            foreach( var element in rootElement.Elements)
            {
                element._RuntimeStyle = null;
                if( element is DomContainerElement )
                {
                    var c = (DomContainerElement)element;
                    if( c.HasElements())
                    {
                        InnerResetRuntimeStyle(c);
                    }
                }
            }
        }

        /// <summary>
        /// 刷新文档视图
        /// </summary>
        /// <param name="refreshSize">是否重新计算元素大小</param>
        /// <param name="executeLayout">是否进行文档内容排版</param>
        /// <param name="ctl">编辑器控件对象</param>
        internal void InnerRefreshDocumentExt(bool refreshSize, bool executeLayout , WriterViewControl ctl )
        {
            this.InvalidateLayoutFast = false;
            this.CheckBoxGroupInfo.InvalidateAll();
            this.ContentStyles.Styles.SetValueLocked(false);
            var _debugMode = this.GetDocumentBehaviorOptions().DebugMode;
            if (_debugMode)
            {
                DCConsole.Default.WriteLine("RefreshDocument RefreshSize="
                    + refreshSize + " ExecuteLayout="
                    + executeLayout);
            }
            object selectionState = this.Selection.SaveState();
            try
            {
                WriterControl.RefreshingDocumentView = true;
                InnerResetRuntimeStyle(this);
                //DomTreeNodeEnumerable enumer2 = new DomTreeNodeEnumerable(this);
                //enumer2.ExcludeParagraphFlag = false;
                //enumer2.ExcludeCharElement = false;
                //foreach (XTextElement element in enumer2)
                //{
                //    element.ResetRuntimeStyle();
                //}
                this.EnabledFixDomState = true;
                //this.FixDomStateWithCheckInvalidateFlag();
                if (this.IsLoadingDocument)
                {
                    this.FixDomStateWithCheckInvalidateFlag();
                }
                else
                {
                    this.FixDomState();
                }
                //this._CompressDom = new DCCompressDom(this);
                DomElementList tables = this.GetElementsByType<DomTableElement>();
                if (tables.Count > 0)
                {
                    foreach (DomTableElement table in tables)
                    {
                        if (table.FixCells() > 0)
                        {
                            table.FixDomState();
                        }
                    }
                    tables.Clear();
                }
                tables = null;
                //if (this.ScriptEngine != null)
                //{
                //    this.ScriptEngine.CheckExecuteMain();
                //}

                this.EnabledFixDomState = false;
                int bigTick = System.Environment.TickCount;// CountDown.GetTickCountFloat();
                //float tick = CountDown.GetTickCountFloat();
                DomElement selectionStartElement = null;
                DomElement selectionEndElement = null;
                this.CurrentContentElement.GetSelectionElement(
                    ref selectionStartElement,
                    ref selectionEndElement);

                this.RefreshContent(executeLayout, true);
                if (ctl != null)
                {
                    ctl.GraphicsUnit = DCSystem_Drawing.GraphicsUnit.Document;
                    if (ctl.AutoSetDocumentDefaultFont)
                    {
                        bool mod = this.Modified;
                        ctl.UpdateDefaultFont(false);
                        this.Modified = mod;
                    }
                }
                //tick = CountDown.GetTickCountFloat() - tick;
                DCGraphics g = null;
                if ( ctl == null )
                {
                    g = this.InnerCreateDCGraphics();
                }
                else
                {
                    g = ctl.CreateDCViewGraphics();
                }
                using (g)
                {
                    if (refreshSize)
                    {
                        ElementLoadEventArgs args = new ElementLoadEventArgs(this, null);
                            this.AfterLoad(args);
                        this.InnerRefreshSize(g); // 此处消耗时间
                    }
                    if (executeLayout)
                    {
                        if (ctl != null)
                        {
                            this.Pages.Clear();
                            var flag2 = false;
                            if( this.FixLayoutForPrint)
                            {
                                this.FixLayoutForPrint = false;
                                flag2 = true;
                            }
                            this.InnerExecuteLayout();
                            if( flag2 )
                            {
                                this.FixLayoutForPrint = true;
                            }
                        }
                        else
                        {
                            this.Pages.Clear();
                            this.InnerExecuteLayout();
                        }
                    }
                    this.RefreshPages();
                    ctl?.UpdatePages();
                    this.Selection.RestoreState(selectionState);
                    this.SetReadyState(DomReadyStates.Complete);
                    if (this.PrintingViewMode() == false)
                    {
                        this.OnSelectionChanged();
                    }
                }//using
                if (ctl != null)
                {
                    ctl.Invalidate();
                    if (this.PrintingViewMode() == false)
                    {
                                if (this.CurrentContentElement.Selection.AllBelongToContent() == false)
                                {
                                    this.CurrentContentElement.SetSelectionElement(selectionStartElement, selectionEndElement);
                                }
                                ctl.UpdateTextCaret();
                    }
                }
                bigTick = Math.Abs(System.Environment.TickCount - bigTick);
                if (_debugMode )
                {
                    DCConsole.Default.WriteLine("RefrehDocument TimeSpan:" + bigTick);
                }
            }
            finally
            {
                WriterControl.RefreshingDocumentView = false;
                this.EnabledFixDomState = true;
            } 
        }
        

        public void RefreshContent(bool invalidateLayout , bool refreshExpression )
        {
            CheckDisposed();
            if (this.IsLoadingDocument)
            {
                this.FixDomStateWithCheckInvalidateFlag();
            }
            else
            {
                this.FixDomState();
            }
            var args9 = DomContainerElement.UpdateElementsRuntimeVisibleArgs.Create(this, true);
            foreach (DomDocumentContentElement dce in this.Elements.FastForEach())
            {
                dce.ParagraphTreeInvalidte = true;
                dce.InnerUpdateElementsRuntimeVisible(args9);
            }
            DomElement lastParent = null;
            //XTextTableElement lastTable = null;
            DomElementList ces = this.GetElementsByType<DomContentElement>();
            foreach (DomContentElement ce in ces.FastForEach() )
            {
                //if (ce.PrivateLines != null)
                //{
                //    ce.PrivateLines.Clear();
                //}
                DomContentElement.UpdateContentElementsArgs args = new DomContentElement.UpdateContentElementsArgs();
                args.UpdateParentContentElement = false;
                args.UpdateElementsVisible = false;
                if (this.IsLoadingDocument == false)
                {
                    args.UpdateChildContentElement = true;
                }
                ce.UpdateContentElements(args);
                //ce.UpdateContentElements(false);
                if (invalidateLayout)
                {
                    if (ce is DomTableCellElement)
                    {
                        DomTableCellElement cell = (DomTableCellElement)ce;
                        cell.Width = 0;
                        cell.Height = 0;
                        DomElement p = cell.Parent;
                        if ( p != null && p != lastParent)
                        {
                            lastParent = p;
                            DomTableElement table = (DomTableElement)p.Parent;
                            if (table != null)
                            {
                                table.LayoutInvalidate = true;
                            }
                        }

                        //XTextTableElement table = cell.OwnerTable;
                        //if (table != null)
                        //{
                        //    table.LayoutInvalidate = true;
                        //}
                    }
                }
            }//foreach
            if (this.HighlightManager != null)
            {
                this.HighlightManager.Clear();
            }
            //foreach (XTextDocumentContentElement dce in this.Elements)
            //{
            //    dce.UpdateContentElements(false);
            //}
        }
        /// <summary>
        /// 增加内容排版版本号
        /// </summary>
        public void IncreaseLayoutVersion()
        {
        }


        /// <summary>
        /// 正在执行文档全局排版
        /// </summary>
        internal static bool IsExecuteingLayout = false;
        /// <summary>
        /// 对整个文档执行重新排版操作
        /// </summary>
        public override void InnerExecuteLayout()
        {
            CheckDisposed();
            this.CacheOptions();
            this.IncreaseLayoutVersion();
            this._GlobalGridInfo = null;
            if (this.IsLoadingDocument)
            {
                this.FixDomStateWithCheckInvalidateFlag();
            }
            else
            {
                this.FixDomState();
            }

            if( this._Pages != null )
            {
                this._Pages.Clear();
            }
            //this.ContentStyles.ResetFastValue();
            this._EnableInvalidateViewFunction = false;
            var ps = this.PageSettings;
            try
            {
                DomContentElement._Cached_PrivateContent = new DomElementList();
                IsExecuteingLayout = true;
                DomContainerElement.AppendViewContentElementArgs.SetTemplate(this);
                //this.ContentStyles.AllowResetFastValue = true;
                //this.ContentStyles.ResetFastValue();
                //this.ContentStyles.AllowResetFastValue = false;
                var list = this.Elements.ToArray();
                DomDocumentBodyElement body = null;
                // 首页页眉页脚是否不同
                foreach (DomDocumentContentElement ce in list)
                {
                    if (ce is DomDocumentBodyElement)
                    {
                        // 正文等会排版
                        body = (DomDocumentBodyElement)ce;
                    }
                    else
                    {
                        ce.InnerForceExecuteLayout();
                        //ce.InnerExecuteLayout();
                    }
                }//foreach
                if (body != null)
                {
                    // 当正文内容元素具有网格线时，由于页眉页脚的高度影响到网格线的步长。因此
                    // 需要计算完页眉页脚后再计算正文内容。
                    // 袁永福 2017-11-22
                    this._GlobalGridInfo = this.GetGlobalGridLineInfo();
                    var dgl = ps == null ? null : ps.DocumentGridLine;
                    if (dgl != null && dgl.Visible)
                    {
                        dgl.UpdateRuntimeGridSpan(
                                this.GetStandartPapeViewHeight(ps),
                                DCSystem_Drawing.GraphicsUnit.Document,
                                1);
                    }
                    body.InnerForceExecuteLayout();
                }
            }
            finally
            {
                IsExecuteingLayout = false;
                DomContentElement._Cached_PrivateContent.ClearAndEmpty();
                DomContentElement._Cached_PrivateContent = null;
                LoaderListBuffer<DomElement[]>.Instance.Clear(
                    delegate (DomElement[] item9s)
                    {
                        Array.Clear(item9s, 0, item9s.Length); 
                    });
                //this.ContentStyles.AllowResetFastValue = true;
                LoaderListBuffer<DomLine[]>.Instance.Clear(null);
                DomContainerElement.AppendViewContentElementArgs.SetTemplate(null);
                this._EnableInvalidateViewFunction = true;
            }
            this.ClearCachedOptions();
            var headerElement = this.Header;
            var footerElement = this.Footer;
            float h = headerElement.Height + footerElement.Height;
            float fixH = h - ps.ViewClientHeight * 0.9f;
            if (fixH > 0)
            {
                // 页眉页脚区域太高了
                if (headerElement.Height > fixH)
                {
                    headerElement.Height -= fixH;
                }
                else
                {
                    footerElement.Height -= fixH;
                }
            }
            //tick = CountDown.GetTickCountFloat() - tick;
        }
         
        /// <summary>
        /// 检查文档是否已经被销毁掉了
        /// </summary>
        public void CheckDisposed()
        {
            if (this._Disposed)
            {
                throw new ObjectDisposedException("dcwriter.document");
            }
        }
        [NonSerialized]
        private bool _Disposed = false;
        /// <summary>
        /// 销毁对象
        /// </summary>
        public override void Dispose()
        {
            try
            {
                this._Disposed = true;
                base.Dispose();
                if (this._PageSettings != null)
                {
                    this._PageSettings.Dispose();
                    this._PageSettings = null;
                }
                if (this._TypedElements != null)
                {
                    this._TypedElements.InnerDispose();
                    this._TypedElements = null;
                }
                if(this._Pages != null )
                {
                    foreach( var page in this._Pages)
                    {
                        page.Dispose();
                    }
                    this._Pages.Clear();
                    this._Pages = null;
                }
                if(this._ContentStyles != null )
                {
                    this._ContentStyles.Dispose();
                    this._ContentStyles = null;
                }
                if( this._Elements != null )
                {
                    foreach( var item in this._Elements)
                    {
                        item.Dispose();
                    }
                    this._Elements.Clear();
                    this._Elements = null;
                }
                if( this._HighlightManager != null )
                {
                    this._HighlightManager.Dispose();
                    this._HighlightManager = null;
                }
                if( this._TabIndexManager != null )
                {
                    this._TabIndexManager.Dispose();
                    this._TabIndexManager = null;
                }
                if(this._UndoList != null )
                {
                    this._UndoList.Dispose();
                    this._UndoList = null;
                }
                this._Cached_BehaviorOptions = null;
                this._Cached_EditOptions = null;
                this._Cached_Options = null;
                this._Cached_ViewOptions = null;
                if(this._CheckBoxGroupInfo != null )
                {
                    this._CheckBoxGroupInfo.Dispose();
                    this._CheckBoxGroupInfo = null;
                }
                this._CurrentContentElement = null;
                this._CurrentPage = null;
                if(this._CurrentStyleInfo != null )
                {
                    this._CurrentStyleInfo.Dispose();
                    this._CurrentStyleInfo = null;
                }
                this._ReplaceElements_CurrentContainer = null;
                this._SerializeOptionsOnce = null;
                //this._ServerObject = null;
                this._SrcElementForEventReadFileContent = null;

                this.ClearContent();
                this._States = null;


                //this.Elements.Clear();
                if (this._Render != null)
                {
                    this._Render.Dispose();
                    this._Render = null;
                }
                this._Options = null;
                //this._BinaryBuffer = null;
                this._CheckBoxGroupInfo = null;
                this._ContentProtectedInfos = null;
                this._ContentStyles = null;
                this._EditorControl = null;
                //this._ContentSnapshot = null;
                //this._ControlOptionsForDebug = null;
                this._EditorControl = null;
                this._MouseCapture = null;
                this._HighlightManager = null;
                this._CurrentContentElement = null;
                this._CurrentPage = null;
                this._CurrentStyleInfo = null;
                this._DocumentControler = null;
                this._FileName = null;
                this._GlobalPages = null;
                this._HoverElement = null;
                this._Info = null;
                //this._OutlineNodes = null;
                this._Options = null;
                this._Pages = null;
                this._PageSettings = null;
                //this._RuntimeChars = null;
                this._SerializeOptionsOnce = null;
                //this._ServerObject = null;
                this._SrcElementForEventReadFileContent = null;
                this._States = null;
                this._TabIndexManager = null;
                this._UndoList = null;
            }
            catch( System.Exception ext )
            {
                DCConsole.Default.WriteLineError("Document.Dispose:" + ext.ToString());
            }
        }
    }
}
