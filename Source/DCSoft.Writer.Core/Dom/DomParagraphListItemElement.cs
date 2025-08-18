using System.Text;
using DCSoft.Drawing;
using System.ComponentModel;

namespace DCSoft.Writer.Dom
{

    /// <summary>
    /// 段落列表元素
    /// </summary>
    public class DomParagraphListItemElement : DomElement
    {
        /// <summary>
        /// 初始化对象
        /// </summary>
        internal DomParagraphListItemElement()
        {
        }
        /// <summary>
        /// 本对象所对应的段落标记元素
        /// </summary>
        internal DomParagraphFlagElement ParagraphFlagElement
        {
            get
            {
                return this.OwnerLine?.ParagraphFlagElement;
            }
        }

        /// <summary>
        /// 重新计算对象大小
        /// </summary>
        /// <param name="args"></param>
        public override void RefreshSize(InnerDocumentPaintEventArgs args)
        {
            InnerRefreshSize(args.Graphics);
        }

        internal void InnerRefreshSize(DCGraphics g)
        {
            DisplayInfo info = new DisplayInfo(this, true);
            info.Measure(g);
            this.Width = info.Size.Width;
            this.Height = info.Size.Height;
        }

        /// <summary>
        /// 显示信息对象
        /// </summary>
        public class DisplayInfo
        {
            public DisplayInfo(DocumentContentStyle style, int index)
            {
                this.TextColor = style.Color;
                if (style.IsBulletedList)
                {
                    this.Text = style.BulletedString;
                    this.Font = new XFontValue("Wingdings", style.FontSize);
                }
                else
                {
                    string txt = style.GetParagraphListText(index);
                    this.Text = txt;
                    this.Font = new XFontValue(XFontValue.DefaultFontName, style.FontSize);
                }
            }

            /// <summary>
            /// 创建对象
            /// </summary>
            /// <param name="element">文档元素对象</param>
            /// <param name="forMaxtListIndex">为最大列表文本而计算</param>
            /// <param name="forPDF">为输出PDF而创建</param>
            /// <returns>创建的对象</returns>
            public DisplayInfo(DomParagraphListItemElement element, bool forMaxtListIndex)
            {
                RuntimeDocumentContentStyle style = element.ParagraphFlagElement?.RuntimeStyle;
                if (style == null)
                {
                    this.Text = "?";
                    this.Font = XFontValue.Default;
                    return;
                }
                this.TextColor = style.Color;
                if (style.IsBulletedList)
                {
                    this.Text = style.BulletedString;
                    this.Font = new XFontValue("Wingdings", style.FontSize);
                }
                else
                {
                    string txt = null;
                    if (style.ParagraphMultiLevel)
                    {
                        StringBuilder str = new StringBuilder();
                        DomParagraphFlagElement pf = element.ParagraphFlagElement;
                        while (pf != null && pf.IsRootLevelElement == false)
                        {
                            if (ParagraphListHelper.IsListNumberStyle(pf.ListStyle()))
                            {
                                if (forMaxtListIndex)
                                {
                                    str.Insert(0, style.GetParagraphListText(pf.MaxListIndex));
                                }
                                else
                                {
                                    str.Insert(0, style.GetParagraphListText(pf.ListIndex));
                                }
                            }
                            pf = pf.ParentParagraph;
                        }
                        txt = str.ToString();
                    }
                    else
                    {
                        if (forMaxtListIndex)
                        {
                            txt = style.GetParagraphListText(element.ParagraphFlagElement.MaxListIndex);
                        }
                        else
                        {
                            txt = style.GetParagraphListText(element.ParagraphFlagElement.ListIndex);
                        }
                    }
                    this.Text = txt;
                    this.Font = new XFontValue(XFontValue.DefaultFontName, style.FontSize);
                }
            }

            /// <summary>
            /// 文本
            /// </summary>
            public string Text = null;
            /// <summary>
            /// 字体
            /// </summary>
            public XFontValue Font = null;
            /// <summary>
            /// 大小
            /// </summary>
            public DCSystem_Drawing.SizeF Size = new DCSystem_Drawing.SizeF(0, 0);
            /// <summary>
            /// 文本颜色
            /// </summary>
            public DCSystem_Drawing.Color TextColor = DCSystem_Drawing.Color.Black;
            /// <summary>
            /// 计算大小
            /// </summary>
            /// <param name="g">画布对象</param>
            public void Measure(DCGraphics g)
            {
                if (string.IsNullOrEmpty(this.Text))
                {
                    this.Size = new DCSystem_Drawing.SizeF(100, 0);
                }
                else
                {
                    //if( this.Text.Length == 6 )
                    //{
                    //    var s = 1;
                    //}
                    this.Size = CharacterMeasurer.MeasureString(g.GraphisForMeasure, this.Text, this.Font, 100000, StringFormat.GenericTypographic);//  g.MeasureString(this.Text, this.Font, 100000, DrawStringFormatExt.GenericTypographic);
                    //int v = (int)Math.Ceiling(this.Size.Width / 100.0);
                    //if (v <= 0)
                    //{
                    //    v = 1;
                    //}
                    //this.Size.Width = 100 * v;
                    if (this.Size.Width < 100)
                    {
                        this.Size.Width = 100;
                    }
                    else
                    {
                        this.Size.Width = this.Size.Width + 20;
                    }
                }
                this.Size.Height = CharacterMeasurer.GetFontHeight(g.GraphisForMeasure, this.Font);// + 3.125f;// g.GetFontHeight(this.Font) + 3.125f;// XTextLine.StdContentTopFix;
            }
        }


        /// <summary>
        /// 计算段落列表项目元素的大小，仅供DCWriter内部使用。
        /// </summary>
        /// <param name="style"></param>
        /// <param name="g"></param>
        /// <param name="maxListIndex"></param>
        /// <returns></returns>
        public static DCSystem_Drawing.SizeF MeasureSize(DocumentContentStyle style, DCGraphics g, int maxListIndex)
        {
            DisplayInfo info = new DisplayInfo(style, maxListIndex);
            info.Measure(g);
            return info.Size;
        }

        private static StringFormat _CenterFormat = null;
        /// <summary>
        /// 绘制对象
        /// </summary>
        /// <param name="args"></param>
        public override void Draw(InnerDocumentPaintEventArgs args)
        {
            if (_CenterFormat == null)
            {
                _CenterFormat = new StringFormat();
                _CenterFormat.Alignment = StringAlignment.Near;
                _CenterFormat.LineAlignment = StringAlignment.Far;
                _CenterFormat.FormatFlags = StringFormatFlags.NoWrap;
            }
            DisplayInfo info = new DisplayInfo(this, false);

            if (string.IsNullOrEmpty(info.Text) == false)
            {
                DCSystem_Drawing.RectangleF txtRect = args.ClientViewBounds;
                txtRect.Y = txtRect.Y + args.Render.CharMeasurer.CharTopFix;
                txtRect.X += 3f;

                if (args.Graphics.IsPDFMode)//.RenderMode == InnerDocumentRenderMode.PDF )
                {
                    args.Graphics.DrawString(
                       info.Text,
                       info.Font,
                       info.TextColor,
                       txtRect,
                       _CenterFormat);
                }
                else
                {
                    args.Graphics.DrawString(
                        info.Text,
                        info.Font,
                        info.TextColor,
                        args.ViewBounds.X,
                        args.ViewBounds.Y + this._Height * 0.5f + 3.125f);
                }
            }
            
        }

        /// <summary>
        /// 返回表示对象的文本
        /// </summary>
        public override string Text
        {
            get
            {
                RuntimeDocumentContentStyle rs = this.ParagraphFlagElement.RuntimeStyle;
                if (rs.IsBulletedList)
                {
                    switch (rs.ParagraphListStyle)
                    {
                        case ParagraphListStyle.BulletedList:
                            return "●";
                        case ParagraphListStyle.BulletedListBlock:
                            return "■";
                        case ParagraphListStyle.BulletedListCheck:
                            return "√";
                        case ParagraphListStyle.BulletedListDiamond:
                            return "◆";
                        case ParagraphListStyle.BulletedListHollowStar:
                            return "◇";
                        case ParagraphListStyle.BulletedListRightArrow:
                            return "＞";
                        default:
                            return "●";
                    }
                }
                else if (rs.IsListNumberStyle)
                {
                    DisplayInfo info = new DisplayInfo(this, false);
                    return info.Text;
                }
                return string.Empty;
            }
            set
            {

            }
        }

    }
}
