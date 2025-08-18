using DCSoft.Writer.Dom;
using DCSoft.Writer.Controls;
using DCSoft.Common;
using System.Collections.Generic;
using DCSoft.Writer;
using DCSoft.Printing;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text;
using DCSoft.Drawing;
using System.Threading.Tasks;
using System.Resources;
using System.Runtime.CompilerServices;
using DCSoft.Writer.Data;

namespace DCSoft.WASM
{
    // 打印相关代码
    partial class WriterControlForWASM
    {
        //private void CheckNotPrintOrPrintPreview(string name)
        //{
        //    if (this.IsPrintOrPrintPreview)
        //    {
        //        throw new Exception(DCSR.FixLayoutForPrint + ":" + name);
        //    }
        //}

        /// <summary>
        /// 当前控件是否处于打印或者打印预览中
        /// </summary>
        public bool IsPrintOrPrintPreview
        {
            get
            {
                if( this._Control.Document.FixLayoutForPrint)
                {
                    return true;
                }
                return false;
            }
        }
        [JSInvokable]
        public string PaintPageForPrintUseSVG(int vPageIndex, bool forPrintPreview )
        {
            var pages = this._Control.Pages;
            if(pages == null ||  pages.Count == 0)
            {
                return null;
            }
            PrintPage page = pages.SafeGet( vPageIndex );
            if(page == null )
            {
                return null;
            }
            return InnerPaintPageForPrintUseSVG(page, forPrintPreview);
        }

        internal string InnerPaintPageForPrintUseSVG(PrintPage page, bool forPrintPreview)
        {
            if (page == null)
            {
                return null;
            }
            var document = (DomDocument)page.Document;
            var vPageIndex = document.Pages.IndexOf(page);
            if (vPageIndex < 0)
            {
                vPageIndex = page.PageIndex;
            }
            var ctlBack = document.EditorControl;
            document.EditorControl = this._Control;
            var myStr = new System.IO.StringWriter();
            var xWriter = new DCSystemXml.XmlTextWriter(myStr);
            xWriter.WriteStartDocument();
            var ofd = DCSoft.SVG.SVGGraphics.CreateForSVG(xWriter, document);
            var ps = document.PageSettings;
            var pageInfo = new PageLayoutInfo(page, true);
            ofd.PageUnit = DCSystem_Drawing.GraphicsUnit.Document;
            ofd.StartSVGPageContent(pageInfo.PageWidth, pageInfo.PageHeight);
            //ofd.AddPage(pageInfo.PageWidth, pageInfo.PageHeight);
            var pdfg = new DCSoft.Drawing.NewPDFGraphics(ofd);
            var g2 = DCGraphics.FromPDFGraphics(pdfg);
            try
            {
                this._Control.GetInnerViewControl().WASMPaintPageForPrint(g2, page, forPrintPreview);
            }
            finally
            {
            }
            ofd.Dispose();
            xWriter.WriteEndDocument();
            xWriter.Close();
            document.EditorControl = ctlBack;
            var strSB = myStr.GetStringBuilder();
            if (strSB.Length > 220)
            {
                var strHeader = strSB.ToString(0, 100);
                var index1 = strHeader.IndexOf("<g>", StringComparison.OrdinalIgnoreCase);
                if (index1 >= 0)
                {
                    var strEnd = strSB.ToString(strSB.Length - 100, 100);
                    var index2 = strEnd.LastIndexOf("</g>", StringComparison.OrdinalIgnoreCase);
                    if (index2 >= 0)
                    {
                        var strXml = strSB.ToString(index1 + 3, strSB.Length - (100 - index2) - index1 - 3);
                        return strXml;
                    }
                }
                return strSB.ToString();
            }
            else
            {
                var strXml = myStr.ToString();
                // 精简XML
                var index1 = strXml.IndexOf("<g>", 0, 100);
                var index2 = strXml.LastIndexOf("</g>", strXml.Length - 1, 100);
                if (index1 >= 0 && index2 > index1)
                {
                    strXml = strXml.Substring(index1 + 3, index2 - index1 - 3);
                }
                return strXml;
            }
        }


        /// <summary>
        /// 打印页面内容
        /// </summary>
        /// <param name="vPageIndex">页码</param>
        /// <param name="forPrintPreview">是否为了打印预览</param>
        /// <returns>生成的打印指令字符串</returns>
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        [JSInvokable]
        public byte[] PaintPageForPrint(int vPageIndex, bool forPrintPreview)
        {
            try
            {
                return this._Control.WASMPaintPageForPrint(vPageIndex, forPrintPreview);
            }
            catch (Exception ex)
            {
                JavaScriptMethods.Tools_ReportException("PaintPageForPrint", ex.Message, ex.ToString(), 0);
                return null;
            }
        }

        [JSInvokable]
        public string WASMPrintPreviewHandleMouseClick(int vPageIndex, int x, int y, bool controlKey)
        {
            return this._Control.GetInnerViewControl().WASMPrintPreviewHandleMouseClick(vPageIndex, x, y, controlKey);
        }

        private DomDocument _DocumentBackupForPrint = null;
        /// <summary>
        /// 为打印而备份当前文档对象，在关闭打印预览或者打印结束后还原文档
        /// </summary>
        [JSInvokable]
        public void BackupDocumentBeforePrint()
        {
            try
            {
                if (this._DocumentBackupForPrint == null)
                {
                    this._DocumentBackupForPrint = (DomDocument)this._Control.Document.Clone(true);
                }
            }
            catch (Exception ex)
            {
                JavaScriptMethods.Tools_ReportException("BackupDocumentBeforePrint", ex.Message, ex.ToString(), 0);
                return;
            }
        }

        [JSInvokable]
        public void RefreshViewAfterPrint()
        {
            try
            {
                if (this._DocumentStatsBackForPrint != null)
                {
                    foreach (var item in this._DocumentStatsBackForPrint)
                    {
                        item.Key.FixLayoutForPrint = false;
                        item.Value.Restore(item.Key);
                        item.Value.Dispose();
                    }
                    this._DocumentStatsBackForPrint.Clear();
                    this._DocumentStatsBackForPrint = null;
                }
                if (this._DocumentBackupForPrint != null)
                {
                    var doc = this._Control.Document;
                    this._Control.Document = this._DocumentBackupForPrint;
                    this._DocumentBackupForPrint = null;
                    if (doc != null)
                    {
                        doc.Dispose();
                    }
                }
                if (this._OptionsBackForPrint != null)
                {
                    this._Control.DocumentOptions = this._OptionsBackForPrint;
                    this._OptionsBackForPrint = null;
                }
                this._Control.Document.States.PrintPreviewing = false;
                this._Control.Document.States.Printing = false;
                this._Control.Document.FixLayoutForPrint = false;
                //this._Control.Document.BodyLayoutOffset = 0;
                this._Control.RefreshInnerView(false);
            }
            catch (Exception ex)
            {
                JavaScriptMethods.Tools_ReportException("RefreshViewAfterPrint", ex.Message, ex.ToString(), 0);
                return;
            }
        }

        /// <summary>
        /// 从打印预览模式触发为打印获得页码信息列表
        /// </summary>
        /// <param name="jsonOptions">打印选项</param>
        /// <returns></returns>
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        [JSInvokable]
        public string GetPageIndexWidthHeightForPrintFromPrintPreview(JsonElement jsonOptions)
        {
            try
            {
                this._OptionsBackForPrint = this._Control.Document.Options.Clone();
                var printOptions = new MyPrintTaskOptions(jsonOptions, this);
                var viewCtl = this._Control.GetInnerViewControl();
                var doc = viewCtl.Document;
                doc.States.PrintPreviewing = true;
                doc.States.Printing = true;
                var pages = GetRuntimePrintPages(viewCtl, viewCtl.Pages, printOptions, new DomDocumentList(doc), false);
                if (pages == null || pages.Count == 0)
                {
                    viewCtl.SetCurrentPageRaw(null);
                    doc.States.Printing = false;
                    doc.States.PrintPreviewing = false;
                    doc.RefreshInnerView(true);
                    viewCtl.UpdatePages();
                    return null;
                }
                viewCtl._CurrentPrintTaskOptions = printOptions;
                var result = InnerGetPageIndexWidthHeightForPrint2(viewCtl, viewCtl.Pages, pages, false);
                return result;
                //return this._Control.GetInnerViewControl().GetPageIndexWidthHeightForPrintFromPrintPreview(printOptions);
            }
            catch (Exception ex)
            {
                JavaScriptMethods.Tools_ReportException("GetPageIndexWidthHeightForPrintFromPrintPreview", ex.Message, ex.ToString(), 0);
                return "";
            }
        }
        
        private class DocumentStateBack :IDisposable
        {
            public DocumentStateBack(DomDocument doc )
            {
                this.Options = doc.Options.Clone();
            }
            public void Restore( DomDocument doc )
            {
                doc.Options = this.Options;
                this.Options = null;
            }
            public DocumentOptions Options = null;
            public void Dispose()
            {
                this.Options = null;
            }
        }
        private Dictionary<DomDocument, DocumentStateBack> _DocumentStatsBackForPrint = null;

        /// <summary>
        /// 为打印获得页码信息列表
        /// </summary>
        /// <param name="forPrintPreview"></param>
        /// <param name="strCleanMode"></param>
        /// <returns></returns>
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        [JSInvokable]
        public string GetPageIndexWidthHeightForPrint(bool forPrintPreview, JsonElement jsonOptions, bool forcePrintPreview = false)
        {
            try
            {
                if(this._DocumentStatsBackForPrint != null )
                {
                    this._DocumentStatsBackForPrint.Clear();
                }
                else
                {
                    this._DocumentStatsBackForPrint = new Dictionary<DomDocument, DocumentStateBack>();
                }
                
                this._DocumentStatsBackForPrint[this._Control.Document] = new DocumentStateBack(this._Control.Document);
                this._OptionsBackForPrint = this._Control.Document.Options.Clone();
                var printOptions = new MyPrintTaskOptions(jsonOptions, this);

                var viewCtl = this._Control.GetInnerViewControl();

                DomDocumentList docs = new DomDocumentList(viewCtl.Document);
                var mainDoc = docs.FirstDocument;
                foreach (var doc in docs)
                {
                    if( this._DocumentStatsBackForPrint.ContainsKey( doc )== false )
                    {
                        this._DocumentStatsBackForPrint[doc] = new DocumentStateBack(doc);
                    }
                    doc.States.PrintPreviewing = true;
                    doc.States.Printing = true;
                    if (doc.FixLayoutForPrint == false)
                    {
                        doc.RefreshContent(true, false);
                        doc.RefreshSizeWithoutParamter();
                        doc.InnerExecuteLayout();
                        doc.RefreshPages();
                    }
                }
                if (forcePrintPreview == false)
                {
                    var cpi = viewCtl.FocusedPageIndexBase0; //viewCtl.CurrentPageIndex;
                    viewCtl.Pages = mainDoc.Pages;
                    if (cpi >= 0 && cpi < viewCtl.Pages.Count)
                    {
                        viewCtl.SetCurrentPageRaw(viewCtl.Pages[cpi]);
                    }
                }
                else if(docs.Count == 1 )
                {
                    viewCtl.Pages = mainDoc.Pages;
                }
                else
                {
                    viewCtl.Pages = docs.AllPages;
                }
                PrintPageCollection pages = null;
                if (forcePrintPreview == true)
                {
                    pages = GetRuntimePrintPages(
                        viewCtl,
                        viewCtl.Pages,
                        printOptions,
                        docs,
                        forPrintPreview);
                }
                else
                {
                    pages = GetRuntimePrintPages(
                        viewCtl,
                        viewCtl.Pages,
                        printOptions,
                        docs,
                        forPrintPreview);
                }
                if (pages == null || pages.Count == 0)
                {
                        viewCtl.SetCurrentPageRaw(null);
                        mainDoc.States.Printing = false;
                        mainDoc.States.PrintPreviewing = false;
                        mainDoc.RefreshInnerView(true);
                        viewCtl.UpdatePages();
                    return null;
                }

                //wyc20241014:修复多文档合并时页码PageIndex样式错误显示成LocalPageIndex的问题
                if (docs.Count > 1)
                {
                    foreach (DomDocument doc in docs)
                    {
                        doc.GlobalPages = pages;
                    }
                }
                /////////////////////////////////////////////////////////////////////////////
                
                viewCtl._CurrentPrintTaskOptions = printOptions;
                var result = InnerGetPageIndexWidthHeightForPrint2(viewCtl, viewCtl.Pages, pages, forPrintPreview);
                return result;
            }
            catch (Exception ex)
            {
                JavaScriptMethods.Tools_ReportException("GetPageIndexWidthHeightForPrint", ex.Message, ex.ToString(), 0);
                return "";
            }
        }
        private static PrintPageCollection GetRuntimePrintPages(
            WriterViewControl ctl,
            PrintPageCollection allPages,
            MyPrintTaskOptions options,
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
                //var indexs = options.GetRuntimePageIndexs(
                //    allPages.Count,
                //    ctl.CurrentPageIndex,
                //    documents);
                var indexs = options.GetRuntimePageIndexs(
                   allPages.Count,
                   ctl.FocusedPageIndexBase0,
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

        internal static string InnerGetPageIndexWidthHeightForPrint2(
            WriterViewControl ctl,
            List<PrintPage> allPages,
            PrintPageCollection outputPages,
            bool forPrintPreview)
        {
            if (forPrintPreview)
            {
                ctl._OutputPagesForPrintPreview = outputPages;
            }
            //var jointPrintNum = 1;
            //if (ctl.Document != null)
            //{
            //    jointPrintNum = ctl.Document.PageSettings.JointPrintNumber;
            //}
            var strCode = new StringBuilder();
            if (forPrintPreview)
            {
                // 打印预览
                strCode.Append('[');
                bool isFirst = true;
                for (var iCount = 0; iCount < outputPages.Count; iCount ++)
                {
                    var page = outputPages[iCount];
                    var curPageIndex = allPages.IndexOf(page);
                    if (isFirst)
                    {
                        isFirst = false;
                    }
                    else
                    {
                        strCode.Append(',');
                    }
                    var doc = (DomDocument)page.Document;
                    if( doc != null )
                    {
                        doc.FixLayoutForPrint = true;
                    }
                    strCode.Append("{\"PageIndex\":" + curPageIndex);
                    var info = new PageLayoutInfo(page);
                    info.ConvertUnit(GraphicsUnit.Pixel);
                    strCode.Append(",\"Width\":" + ((int)info.PageWidth).ToString());
                    strCode.Append(",\"Height\":" + ((int)info.PageHeight));
                    strCode.Append('}');
                }
                strCode.Append(']');
            }
            else
            {
                // 打印输出
                //var crect = GetPageContentBounds(ctl.Document, outputPages[0]);
                strCode.Append("[\"@page{margin-left:0px;margin-top:0px;margin-right:0px;margin-bottom:0px;");
                //strCode.Append("[\"@page{margin-left:"
                //    + Math.Max(crect.Left - 10, 5)
                //    + "px;margin-top:"
                //    + Math.Max(crect.Top - 10, 5)
                //    + "px;margin-right:0px;margin-bottom:0px;");
                if (ctl.Document.PageSettings.Landscape)
                {
                    strCode.Append("size:landscape;");
                }
                // 输出缩放比率
                strCode.Append("\",1");
                for (var iCount = 0; iCount < outputPages.Count; iCount ++)
                {
                    var page = outputPages[iCount];
                    var curPageIndex = allPages.IndexOf(page);
                    page.GlobalIndex = curPageIndex;
                    var doc = (DomDocument)page.Document;
                    if (doc != null)
                    {
                        doc.FixLayoutForPrint = true;
                    }
                    strCode.Append(",{\"PageIndex\":" + curPageIndex);
                        var rect = GetPageContentBounds(ctl.Document, page, true);
                        strCode.Append(",\"Width\":" + (int)rect.Width);
                        strCode.Append(",\"Height\":" + (int)rect.Height + "}");
                }
                strCode.Append(']');
            }
            return strCode.ToString();
        }

        private static DCSystem_Drawing.Rectangle GetPageContentBounds( DomDocument doc, PrintPage page, bool onlyFixHeight = false)
        {
            var info = new PageLayoutInfo(page);
            info.ConvertUnit(GraphicsUnit.Pixel);
            var left = info.LeftMargin;
            var top = info.HeaderDistance;
            var right = info.PageWidth - info.RightMargin;
            right = Math.Max(right, left + doc.Body.PixelWidth);
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
    }
}
