using System;
using System.Collections.Generic;
using System.Text;
// // 
using DCSoft.Drawing;
using DCSoft.Writer.Dom;

namespace DCSoft.Printing
{
    /// <summary>
    /// 分页文档绘制内容事件参数
    /// </summary>
    public class InnerPageDocumentPaintEventArgs 
    {
        public InnerPageDocumentPaintEventArgs(
            DCGraphics graphics,
            DCSystem_Drawing.Rectangle clipRectangle,
            DCSoft.Writer.Dom.DomDocument document,
            PrintPage page,
            PageContentPartyStyle contentStyle,
            SimpleRectangleTransform transformItem)
        {
            _Graphics = graphics;
            _ClipRectangle = clipRectangle;
            _Document = document;
            _Page = page;
            _ContentStyle = contentStyle;
            if (page != null)
            {
                _PageIndex = page.GlobalIndex;
            }
            _TransformItem = transformItem;
        }

        private DCGraphics _Graphics = null;
        public DCGraphics Graphics
        {
            get
            {
                return this._Graphics;
            }
        }
        private readonly SimpleRectangleTransform _TransformItem = null;

        public SimpleRectangleTransform TransformItem
        {
            get { return _TransformItem; }
        }

        private DCPrintDocumentOptions _Options = null;
        /// <summary>
        /// 文档选项
        /// </summary>
        public DCPrintDocumentOptions Options
        {
            get { return _Options; }
            set { _Options = value; }
        }


        private int _PageIndex = 0;
        /// <summary>
        /// 从0开始计算的页码号
        /// </summary>
        public int PageIndex
        {
            get
            {
                return _PageIndex;
            }
            set
            {
                _PageIndex = value;
            }
        }

        private int _NumberOfPages = 0;
        /// <summary>
        /// 总页数
        /// </summary>
        [System.ComponentModel.DefaultValue(0)]
        public int NumberOfPages
        {
            get
            {
                return _NumberOfPages;
            }
            set
            {
                _NumberOfPages = value;
            }
        }

        //private Graphics _Graphics = null;
        ///// <summary>
        ///// 图形绘制对象
        ///// </summary>
        //public Graphics Graphics
        //{
        //    get
        //    {
        //        return _Graphics;
        //    }
        //}

        private DCSystem_Drawing.Rectangle _ClipRectangle = DCSystem_Drawing.Rectangle.Empty;
        /// <summary>
        /// 剪切矩形
        /// </summary>
        public DCSystem_Drawing.Rectangle ClipRectangle
        {
            get
            {
                return _ClipRectangle;
            }
            set
            {
                _ClipRectangle = value;
            }
        }

        private DCSystem_Drawing.Rectangle _ContentBounds = DCSystem_Drawing.Rectangle.Empty;
        /// <summary>
        /// 当绘制页眉页脚时，页眉页脚内容边界
        /// </summary>
        public DCSystem_Drawing.Rectangle ContentBounds
        {
            get
            {
                return _ContentBounds;
            }
            set
            {
                _ContentBounds = value;
            }
        }

        private DCSystem_Drawing.RectangleF _PageClipRectangle
            = DCSystem_Drawing.RectangleF.Empty;
        /// <summary>
        /// 页面剪切矩形
        /// </summary>
        public DCSystem_Drawing.RectangleF PageClipRectangle
        {
            get
            {
                return _PageClipRectangle;
            }
            set
            {
                _PageClipRectangle = value;
            }
        }

        private readonly DCSoft.Writer.Dom.DomDocument _Document = null;
        /// <summary>
        /// 文档对象
        /// </summary>
        public DomDocument Document
        {
            get
            {
                return _Document;
            }
        }

        private readonly PrintPage _Page = null;
        /// <summary>
        /// 页面对象
        /// </summary>
        public PrintPage Page
        {
            get
            {
                return _Page;
            }
        }

        private readonly PageContentPartyStyle _ContentStyle = PageContentPartyStyle.Body;
        /// <summary>
        /// 内容样式
        /// </summary>
        public PageContentPartyStyle ContentStyle
        {
            get
            {
                return _ContentStyle;
            }
            //set
            //{
            //    _ContentStyle = value; 
            //}
        }


        private ContentRenderMode _RenderMode = ContentRenderMode.UIPaint;
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

        //private bool _EditMode = false;
        ///// <summary>
        ///// 处于编辑模式
        ///// </summary>
        //public bool EditMode
        //{
        //    get
        //    {
        //        return _EditMode;
        //    }
        //    set
        //    {
        //        _EditMode = value;
        //    }
        //}

        private bool _Cancel = false;
        /// <summary>
        /// 取消操作
        /// </summary>
        public bool Cancel
        {
            get
            {
                return _Cancel;
            }
            set
            {
                _Cancel = value;
            }
        }
    }
}
