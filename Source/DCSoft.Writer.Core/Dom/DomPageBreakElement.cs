using System;
using System.Collections.Generic;
using System.Text;
// // 
//using System.Drawing.Drawing2D;
using DCSoft.Drawing;
using DCSoft.Writer.Serialization;

namespace DCSoft.Writer.Dom
{
    /// <summary>
    /// 分页元素
    /// </summary>
    /// <remarks>编制 袁永福</remarks>
    public sealed partial class DomPageBreakElement : DomElement
    {
        /// <summary>
        /// 初始化对象
        /// </summary>
        //[DCSoft.Common.DCPublishAPI]
        public DomPageBreakElement()
        {
            this.Height = 50;
        }

        /// <summary>
        /// 绘制分页符内容
        /// </summary>
        /// <param name="args">参数</param>
        public override void Draw(InnerDocumentPaintEventArgs args)
        {
            if (this.GetDocumentViewOptions().ShowPageBreak
                && args.RenderMode == InnerDocumentRenderMode.Paint
                && args.Graphics != null)
            {
                RectangleF bounds = args.ViewBounds;
                using (var format = new StringFormat())
                {
                    var font = XFontValue.Default;
                    var txt = DCSR.PageBreak;
                    if (string.IsNullOrEmpty(txt))
                    {
                        // 没有文本
                        using (var p = new Pen(Color.Black, 1, DashStyle.Dot))
                        {
                            args.Graphics.DrawLine(
                                 p,
                                 bounds.Left,
                                 bounds.Top + bounds.Height / 2,
                                 bounds.Right,
                                 bounds.Top + bounds.Height / 2);
                        }
                    }
                    else
                    {
                        SizeF size = args.GraphicsMeasureString(
                            txt,
                            font,
                            10000,
                            format);
                        using (var p = new Pen(Color.Black, 1, DashStyle.Dot))
                        {
                            args.Graphics.DrawLine(
                                 p,
                                 bounds.Left,
                                 bounds.Top + bounds.Height / 2,
                                 bounds.Left + bounds.Width / 2 - size.Width / 2 - 6,
                                 bounds.Top + bounds.Height / 2);
                            args.Graphics.DrawLine(
                                p,
                                bounds.Left + bounds.Width / 2 + size.Width / 2 + 6,
                                bounds.Top + bounds.Height / 2,
                                bounds.Right,
                                bounds.Top + bounds.Height / 2);
                        }
                        format.Alignment = StringAlignment.Center;
                        format.LineAlignment = StringAlignment.Center;
                        format.FormatFlags = StringFormatFlags.NoWrap;
                        args.Graphics.DrawString(
                            txt,
                            font,
                            Color.Black,
                            bounds.Left + bounds.Width / 2 - size.Width / 2,
                            bounds.Top + 3 + size.Height * 0.5f);
                    }
                }
            }
        }
        /// <summary>
        /// 是否处理过了分页
        /// </summary>
        [NonSerialized]
        internal bool Handled = false;
    }
}
