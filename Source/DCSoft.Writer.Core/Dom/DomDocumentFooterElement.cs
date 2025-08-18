using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using DCSoft.Printing;
using DCSoft.Drawing;
// // 
using System.Runtime.InteropServices;
using DCSoft.Common;

namespace DCSoft.Writer.Dom
{
    /// <summary>
    ///文档页脚对象
    /// </summary>
#if !RELEASE
    [System.Diagnostics.DebuggerDisplay("Footer :{ PreviewString }")]
#endif
    public sealed partial class DomDocumentFooterElement : DomDocumentContentElement
    {
        /// <summary>
        /// 初始化对象
        /// </summary>

        public DomDocumentFooterElement()
        {
        }

        public override DCGridLineInfo InnerRuntimeGridLine()
        {
              return null;
            
        }

        //[DCSoft.Common.DCPublishAPI]
        public override PageContentPartyStyle ContentPartyStyle
        {
            get
            {
                return PageContentPartyStyle.Footer ;
            }
        }
#if !RELEASE
        /// <summary>
        /// 返回预览用的文本
        /// </summary>

        public override string PreviewString
        {
            get
            {
                return "Footer:" + base.PreviewString;
            }
        }
#endif
        /// <summary>
        /// 返回Footer
        /// </summary>
        //[Browsable(false)]
        //[DCSoft.Common.DCPublishAPI]
        public override PageContentPartyStyle PagePartyStyle
        {
            get
            {
                return PageContentPartyStyle.Footer;
            }
        }
#if !RELEASE
        /// <summary>
        /// 返回调试时显示的文本
        /// </summary>
        /// <returns>文本</returns>

        public override string ToDebugString()
        {
            string txt = "Footer";
            if (this.OwnerDocument != null)
            {
                txt = txt + ":" + this.OwnerDocument.RuntimeTitle;
            }
            return txt;
        }
#endif
    }
}
