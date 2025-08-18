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
    /// 文档页眉对象
    /// </summary>
#if !RELEASE
    [System.Diagnostics.DebuggerDisplay("Header :{ PreviewString }")]
#endif
    public sealed partial class DomDocumentHeaderElement : DomDocumentContentElement
    {
        /// <summary>
        /// 初始化对象
        /// </summary>

        public DomDocumentHeaderElement()
        {
        }


        public override PageContentPartyStyle ContentPartyStyle
        {
            get
            {
                return PageContentPartyStyle.Header ;
            }
        }

         
        /// <summary>
        /// 修正元素内容
        /// </summary>
        public override void FixElements()
        {
            if (this.OwnerDocument == null)
            {
                return;
            }
            if (this.Elements.Count == 0
                || (this.Elements.LastElement is DomParagraphFlagElement) == false)
            {
                DomParagraphFlagElement flag = new DomParagraphFlagElement();
                flag.AutoCreate = true;
                DocumentContentStyle style = new DocumentContentStyle();
                style.Align = DocumentContentAlignment.Center;
                flag.StyleIndex = this.OwnerDocument.ContentStyles.GetStyleIndex(style);
                this.AppendChildElement(flag);
            }
        }
#if !RELEASE
        /// <summary>
        /// 返回预览文本
        /// </summary>
        public override string PreviewString
        {
            get
            {
                return "Header:" + base.PreviewString;
            }
        }
#endif
        /// <summary>
        /// 返回Header
        /// </summary>
        public override PageContentPartyStyle PagePartyStyle
        {
            get
            {
                return PageContentPartyStyle.Header;
            }
        }
#if !RELEASE
        /// <summary>
        /// 返回调试时显示的文本
        /// </summary>
        /// <returns>文本</returns>
        public override string ToDebugString()
        {
            string txt = "Header";
            if (this.OwnerDocument != null)
            {
                txt = txt + ":" + this.OwnerDocument.RuntimeTitle;
            }
            return txt;
        }
#endif
    }
}
