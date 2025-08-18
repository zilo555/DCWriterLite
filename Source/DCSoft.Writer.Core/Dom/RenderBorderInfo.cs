using System;
using System.Collections.Generic;
using System.Text;

namespace DCSoft.Writer.Dom
{
    /// <summary>
    /// 呈现文档元素边框线的信息对象
    /// </summary>
    internal class RenderBorderInfo
    {
        private readonly DomElementList _Elements = new DomElementList();
        /// <summary>
        /// 文档元素对象
        /// </summary>
        public DomElementList Elements
        {
            get { return _Elements; }
            //set { _Elements = value; }
        }

        //private XTextDocumentContentElement _DocumentContentElement = null;

        //public XTextDocumentContentElement DocumentContentElement
        //{
        //    get { return _DocumentContentElement; }
        //    set { _DocumentContentElement = value; }
        //}

        private RectangleF _Bounds = RectangleF.Empty;
        /// <summary>
        /// 边界
        /// </summary>
        public RectangleF Bounds
        {
            get { return _Bounds; }
            set { _Bounds = value; }
        }

        private RuntimeDocumentContentStyle _Style = null;
        /// <summary>
        /// 使用的样式信息对象
        /// </summary>
        public RuntimeDocumentContentStyle Style
        {
            get { return _Style; }
            set { _Style = value; }
        }

        private DomLine _Line = null;
        /// <summary>
        /// 文档行对象
        /// </summary>
        public DomLine Line
        {
            get { return _Line; }
            set { _Line = value; }
        }
    }

    /// <summary>
    /// 边框呈现信息列表
    /// </summary>
    internal class RenderBorderInfoList : List<RenderBorderInfo>
    {
        /// <summary>
        /// 初始化对象
        /// </summary>
        public RenderBorderInfoList()
        {
        }

        //public void AddInfo(XTextElement element , DocumentContentStyle style )
        //{
        //    RenderBorderInfo info = new RenderBorderInfo();
        //    info.Element = element;
        //    info.Line = element.OwnerLine;
        //    info.Style = style;
        //    this.Add(info);
        //}
    }
}
