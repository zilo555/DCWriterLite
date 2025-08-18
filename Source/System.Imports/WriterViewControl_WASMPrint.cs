
using System.Collections.Generic;
using DCSoft.Printing;
// // 
using System.Text;
using DCSoft.Drawing;
using DCSoft.Writer.Dom;

namespace DCSoft.Writer.Controls
{
    partial class WriterViewControl
    {
        internal DCSoft.WASM.MyPrintTaskOptions _CurrentPrintTaskOptions = null;
       
        /// <summary>
        /// 打印预览输出的页面对象列表
        /// </summary>
        internal PrintPageCollection _OutputPagesForPrintPreview = null;
        /// <summary>
        /// 处理打印预览下的鼠标点击事件
        /// </summary>
        /// <param name="vPageIndex">页码</param>
        /// <param name="x">鼠标X坐标</param>
        /// <param name="y">鼠标Y坐标</param>
        /// <returns>导致需要重新绘制的页码列表</returns>
        public string WASMPrintPreviewHandleMouseClick(int vPageIndex, int x, int y, bool controlKey)
        {
            return null;
        }

        /// <summary>
        /// 打印页面内容
        /// </summary>
        /// <param name="vPageIndex">页码</param>
        /// <param name="forPrintPreview">是否为了打印预览</param>
        /// <returns>生成的打印指令字符串</returns>
        public byte[] WASMPaintPageForPrint(PrintPage page, bool forPrintPreview)
        {
            if( page == null )
            {
                throw new ArgumentNullException("page");
            }
            this._WASMPrintMode = true;
            var document = (DomDocument)page.Document;
            var vPageIndex = document.Pages.IndexOf(page);
            if(vPageIndex < 0 )
            {
                vPageIndex = page.PageIndex;
            }
            document.States.Printing = true;
            document.States.PrintPreviewing = true;
            var g = new Graphics();
            var layoutInfo = new PageLayoutInfo(document.PageSettings, false, false);
            layoutInfo.ConvertUnit(GraphicsUnit.Pixel);
            if (forPrintPreview)
            {
                // 打印预览模式
                    g.ZoomRate = this.WASMZoomRate * this.OwnerWriterControl.WASMBaseZoomRate;
                document.DrawPageContentByGraphics(
                    g,
                    page,
                    false,
                    1,
                    DocumentRenderMode.Print,
                    this._CurrentPrintTaskOptions);
            }
            else
            {
                // 打印模式
                var crect = GetPageContentBounds(page);
                g.ZoomRate = ZoomRateForPrint;
                document.DrawPageContentByGraphics(
                    g,
                    page,
                    false,
                    1,
                    DocumentRenderMode.Print,
                    this._CurrentPrintTaskOptions);
            }
            var strCode = g.ToByteArray();
            g.Dispose();
            return strCode;
        }

        /// <summary>
        /// 打印页面内容
        /// </summary>
        /// <param name="vPageIndex">页码</param>
        /// <param name="forPrintPreview">是否为了打印预览</param>
        /// <returns>生成的打印指令字符串</returns>
        public void WASMPaintPageForPrint(DCGraphics g, PrintPage page , bool forPrintPreview)
        {
            if( page == null)
            {
                throw new ArgumentNullException("page");
            }
            //var drawBackGround = true;
            this._WASMPrintMode = true;
            var document = (DomDocument)page.Document;
            var vPageIndex = document.Pages.IndexOf(page);
            if( vPageIndex < 0 )
            {
                vPageIndex = page.PageIndex;
            }
            document.States.Printing = true;
            document.States.PrintPreviewing = true;
            var layoutInfo = new PageLayoutInfo(document.PageSettings, false, false);
            layoutInfo.ConvertUnit(GraphicsUnit.Pixel);
            if (forPrintPreview)
            {
                // 打印预览模式
                document.DrawPageContentByGraphics(
                    g,
                    page,
                    false,
                    1,
                    DocumentRenderMode.Print,
                    this._CurrentPrintTaskOptions);
            }
            else
            {
                // 打印模式
                var crect = GetPageContentBounds(page);
                document.DrawPageContentByGraphics(
                    g,
                    page,
                    false,
                    1,
                    DocumentRenderMode.Print,
                    this._CurrentPrintTaskOptions);
            }
        }
    }
}
