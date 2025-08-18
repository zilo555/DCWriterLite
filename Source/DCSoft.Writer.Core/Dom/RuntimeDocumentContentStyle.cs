using System;
using System.Collections.Generic;
using System.Text;
using DCSoft.Drawing;
using DCSoft.Common;

namespace DCSoft.Writer.Dom
{
    /// <summary>
    /// 运行时的样式值
    /// </summary>
    public partial class RuntimeDocumentContentStyle : IDisposable
    {
        internal RuntimeDocumentContentStyle( RuntimeDocumentContentStyle rs , string newFontName , float newFontSize , Color newColor )
        {
            if( rs == null )
            {
                throw new ArgumentNullException("rs");
            }
            this.Align = rs.Align;
            this.BackgroundColor = rs.BackgroundColor;
            this.Bold = rs.Bold;
            this.BorderBottom = rs.BorderBottom;
            this.BorderBottomColor = rs.BorderBottomColor;
            this.BorderColor = rs.BorderColor;
            this.BorderLeft = rs.BorderLeft;
            this.BorderLeftColor = rs.BorderLeftColor;
            this.BorderRight = rs.BorderRight;
            this.BorderRightColor = rs.BorderRightColor;
            this.BorderStyle = rs.BorderStyle;
            this.BorderTop = rs.BorderTop;
            this.BorderTopColor = rs.BorderTopColor;
            this.BorderWidth = rs.BorderWidth;
            this.BulletedString = rs.BulletedString;
            this.Color = rs.Color;
            this.ColorForRTF = rs.ColorForRTF;
            this.FirstLineIndent = rs.FirstLineIndent;
            this.Font = rs.Font;
            this.MyFontInfo = rs.MyFontInfo;
            this.SetFontHeight( rs.FontHeight );
            this.FontName = rs.FontName;
            this.FixedFontName = rs.FixedFontName;
            this.FontSize = rs.FontSize;
            this.FontStyle = rs.FontStyle;
            this.FontValue = new Font(this.FixedFontName, this.FontSize, this.FontStyle, GraphicsUnit.Point);
            this.HasFullBorder = rs.HasFullBorder;
            this.HasVisibleBackground = rs.HasVisibleBackground;
            this.HasVisibleBorder = rs.HasVisibleBorder;
            this.IsBulletedList = rs.IsBulletedList;
            this.IsListNumberStyle = rs.IsListNumberStyle;
            this.Italic = rs.Italic;
            this.LeftIndent = rs.LeftIndent;
            this.LineSpacing = rs.LineSpacing;
            this.LineSpacingStyle = rs.LineSpacingStyle;
            this.PaddingBottom = rs.PaddingBottom;
            this.PaddingLeft = rs.PaddingLeft;
            this.PaddingRight = rs.PaddingRight;
            this.PaddingTop = rs.PaddingTop;
            this.ParagraphListStyle = rs.ParagraphListStyle;
            this.ParagraphMultiLevel = rs.ParagraphMultiLevel;
            this.ParagraphOutlineLevel = rs.ParagraphOutlineLevel;
            this.SpacingAfterParagraph = rs.SpacingAfterParagraph;
            this.SpacingBeforeParagraph = rs.SpacingBeforeParagraph;
            this.Strikeout = rs.Strikeout;
            this.Subscript = rs.Subscript;
            this.Superscript = rs.Superscript;
            this.Underline = rs.Underline;
            this.VerticalAlign = rs.VerticalAlign;
            this.Color = newColor;
            this.FontName = newFontName;
            this.FontSize = newFontSize;
            this.Font = new XFontValue(newFontName, newFontSize, rs.FontStyle);
        }
        /// <summary>
        /// 初始化对象
        /// </summary>
        /// <param name="style">样式值</param>
        internal RuntimeDocumentContentStyle(DocumentContentStyle style)
        {
            if (style == null)
            {
                throw new ArgumentNullException("style");
            }
            this.Parent = style;
            if (style._MyRuntimeStyle is RuntimeDocumentContentStyle)
            {
                RuntimeDocumentContentStyle rs = (RuntimeDocumentContentStyle)style._MyRuntimeStyle;

                this.Align = rs.Align;
                this.BackgroundColor = rs.BackgroundColor;
                this.Bold = rs.Bold;
                this.BorderBottom = rs.BorderBottom;
                this.BorderBottomColor = rs.BorderBottomColor;
                this.BorderColor = rs.BorderColor;
                this.BorderLeft = rs.BorderLeft;
                this.BorderLeftColor = rs.BorderLeftColor;
                this.BorderRight = rs.BorderRight;
                this.BorderRightColor = rs.BorderRightColor;
                this.BorderStyle = rs.BorderStyle;
                this.BorderTop = rs.BorderTop;
                this.BorderTopColor = rs.BorderTopColor;
                this.BorderWidth = rs.BorderWidth;
                this.BulletedString = rs.BulletedString;
                this.Color = rs.Color;
                this.ColorForRTF = rs.ColorForRTF;
                this.FirstLineIndent = rs.FirstLineIndent;
                this.Font = rs.Font;
                this.MyFontInfo = rs.MyFontInfo;
                this.SetFontHeight(rs.FontHeight);
                this.FontName = XFontValue.FixFontName( rs.FontName ,false);
                this.FixedFontName = rs.FixedFontName;
                this.FontSize = rs.FontSize;
                this.FontStyle = rs.FontStyle;
                this.FontValue = rs.FontValue;
                 this.HasFullBorder = rs.HasFullBorder;
                this.HasVisibleBackground = rs.HasVisibleBackground;
                this.HasVisibleBorder = rs.HasVisibleBorder;
                this.IsBulletedList = rs.IsBulletedList;
                this.IsListNumberStyle = rs.IsListNumberStyle;
                this.Italic = rs.Italic;
                this.LeftIndent = rs.LeftIndent;
                this.LineSpacing = rs.LineSpacing;
                this.LineSpacingStyle = rs.LineSpacingStyle;
                this.PaddingBottom = rs.PaddingBottom;
                this.PaddingLeft = rs.PaddingLeft;
                this.PaddingRight = rs.PaddingRight;
                this.PaddingTop = rs.PaddingTop;
                this.ParagraphListStyle = rs.ParagraphListStyle;
                this.ParagraphMultiLevel = rs.ParagraphMultiLevel;
                this.ParagraphOutlineLevel = rs.ParagraphOutlineLevel;
                this.SpacingAfterParagraph = rs.SpacingAfterParagraph;
                this.SpacingBeforeParagraph = rs.SpacingBeforeParagraph;
                this.Strikeout = rs.Strikeout;
                this.Subscript = rs.Subscript;
                this.Superscript = rs.Superscript;
                this.Underline = rs.Underline;
                this.VerticalAlign = rs.VerticalAlign;
                return;
            }

            foreach (var item in style.GetInnerValues())
            {
                switch (item.Key.ID)
                {
                    case ContentStylePropertyIDConsts.ID_Align: this.Align = (DCSoft.Drawing.DocumentContentAlignment)item.Value; break;
                    case ContentStylePropertyIDConsts.ID_BackgroundColor:
                        {
                            this.BackgroundColor = ( Color)item.Value;
                        }
                        break;
                    case ContentStylePropertyIDConsts.ID_Bold:
                        {
                            this.Bold = (bool)item.Value;
                            if (this.Bold)
                            {
                                this.FontStyle = this.FontStyle | FontStyle.Bold;
                            }
                        }
                        break;
                    case ContentStylePropertyIDConsts.ID_BorderBottom: this.BorderBottom = (bool)item.Value; break;
                    case ContentStylePropertyIDConsts.ID_BorderBottomColor: this.BorderBottomColor = ( Color)item.Value; break;
                    case ContentStylePropertyIDConsts.ID_BorderLeft: this.BorderLeft = (bool)item.Value; break;
                    case ContentStylePropertyIDConsts.ID_BorderLeftColor:
                        this.BorderLeftColor = ( Color)item.Value;
                        this.BorderColor = this.BorderLeftColor;
                        break;
                    case ContentStylePropertyIDConsts.ID_BorderRight: this.BorderRight = (bool)item.Value; break;
                    case ContentStylePropertyIDConsts.ID_BorderRightColor: this.BorderRightColor = ( Color)item.Value; break;
                    case ContentStylePropertyIDConsts.ID_BorderStyle: this.BorderStyle = ( DashStyle)item.Value; break;
                    case ContentStylePropertyIDConsts.ID_BorderTop: this.BorderTop = (bool)item.Value; break;
                    case ContentStylePropertyIDConsts.ID_BorderTopColor: this.BorderTopColor = ( Color)item.Value; break;
                    case ContentStylePropertyIDConsts.ID_BorderWidth: this.BorderWidth = (float)item.Value; break;
                    case ContentStylePropertyIDConsts.ID_Color:
                        {
                            this.Color = ( Color)item.Value;
                        }
                        break;
                    case ContentStylePropertyIDConsts.ID_FirstLineIndent:
                        {
                            this.FirstLineIndent = (float)item.Value;
                        }
                        break;
                    case ContentStylePropertyIDConsts.ID_FontName:
                        {
                            this.FontName = XFontValue.FixFontName((string)item.Value, false);
                            this.FixedFontName = XFontValue.FixFontName(this.FontName, false);
                        }
                        break;
                    case ContentStylePropertyIDConsts.ID_FontSize:
                        {
                            this.FontSize = (float)item.Value;
                            if (this.FontSize <= 0)
                            {
                                this.FontSize = 1;
                            }
                        }
                        break;
                    case ContentStylePropertyIDConsts.ID_Italic:
                        {
                            this.Italic = (bool)item.Value;
                            if (this.Italic)
                            {
                                this.FontStyle = this.FontStyle | FontStyle.Italic;
                            }
                        }
                        break;
                    case ContentStylePropertyIDConsts.ID_LeftIndent: this.LeftIndent = (float)item.Value; break;
                    case ContentStylePropertyIDConsts.ID_LineSpacing:
                        {
                            this.LineSpacing = (float)item.Value;
                        }
                        break;
                    case ContentStylePropertyIDConsts.ID_LineSpacingStyle:
                        {
                            this.LineSpacingStyle = (DCSoft.Drawing.LineSpacingStyle)item.Value;
                        }
                        break;
                    case ContentStylePropertyIDConsts.ID_PaddingBottom: this.PaddingBottom = (float)item.Value; break;
                    case ContentStylePropertyIDConsts.ID_PaddingLeft: this.PaddingLeft = (float)item.Value; break;
                    case ContentStylePropertyIDConsts.ID_PaddingRight: this.PaddingRight = (float)item.Value; break;
                    case ContentStylePropertyIDConsts.ID_PaddingTop: this.PaddingTop = (float)item.Value; break;
                    case ContentStylePropertyIDConsts.ID_ParagraphListStyle:
                        {
                            var pls2 = (ParagraphListStyle)item.Value;
                            if (ParagraphListHelper.IsBulletedList(pls2))
                            {
                                {
                                    // 支持圆点列表
                                    this.IsBulletedList = true;
                                }
                            }
                            else if (ParagraphListHelper.IsListNumberStyle(pls2))
                            {
                                {
                                    // 支持数字列表
                                    this.IsListNumberStyle = style.IsListNumberStyle;
                                }
                            }
                            this.ParagraphListStyle = pls2;
                            this.BulletedString = ParagraphListHelper.GetBulletedListString(pls2);
                        }
                        break;
                    case ContentStylePropertyIDConsts.ID_ParagraphMultiLevel: this.ParagraphMultiLevel = (bool)item.Value; break;
                    case ContentStylePropertyIDConsts.ID_ParagraphOutlineLevel: this.ParagraphOutlineLevel = (int)item.Value; break;
                    case ContentStylePropertyIDConsts.ID_SpacingAfterParagraph:
                        {
                            this.SpacingAfterParagraph = (float)item.Value;
                        }
                        break;
                    case ContentStylePropertyIDConsts.ID_SpacingBeforeParagraph:
                        {
                            this.SpacingBeforeParagraph = (float)item.Value;
                        }
                        break;
                    case ContentStylePropertyIDConsts.ID_Strikeout:
                        {
                            this.Strikeout = (bool)item.Value;
                            if (this.Strikeout)
                            {
                                this.FontStyle = this.FontStyle | FontStyle.Strikeout;
                            }
                        }
                        break;
                    case ContentStylePropertyIDConsts.ID_Subscript:
                        {
                            this.Subscript = (bool)item.Value;
                        }
                        break;
                    case ContentStylePropertyIDConsts.ID_Superscript:
                        {
                            this.Superscript = (bool)item.Value;
                        }
                        break;
                    case ContentStylePropertyIDConsts.ID_Underline:
                        {
                            this.Underline = (bool)item.Value;
                            if (this.Underline)
                            {
                                this.FontStyle = this.FontStyle | FontStyle.Underline;
                            }
                        }
                        break;
                    case ContentStylePropertyIDConsts.ID_VerticalAlign: this.VerticalAlign = (DCSoft.Drawing.VerticalAlignStyle)item.Value; break;
                }
                this.FontValue = new Font(this.FixedFontName, this.FontSize, this.FontStyle, GraphicsUnit.Point);
            }
            this.SetFontHeight(style.FontHeight);
            if (this.BorderWidth > 0)
            {
                if (this.BorderLeft || this.BorderTop || this.BorderRight || this.BorderBottom)
                {
                    if (this.BorderTopColor.A != 0
                        || this.BorderLeftColor.A != 0
                        || this.BorderRightColor.A != 0
                        || this.BorderBottomColor.A != 0)
                    {
                        this.HasVisibleBorder = true;
                    }
                }
            }
            this.HasVisibleBackground = this.BackgroundColor.A != 0;
            this.MyFontInfo = new XFontInfo(this.FontName , this.FontSize, this.FontStyle, GraphicsUnit.Point);
            this.Font = new XFontValue(this.FontName, this.FontSize, this.FontStyle, GraphicsUnit.Point);
            this.HasFullBorder = this.BorderLeft && this.BorderTop && this.BorderRight && this.BorderBottom;
        }
        internal DCSoft.Writer.Dom.CharacterMeasurer.StdSize10FontInfo StdSize10FontInfoForMeasure = null;
        public readonly bool HasFullBorder = false;
        public readonly bool HasVisibleBackground = false;
       public string GetParagraphListText(int number)
        {
            return ParagraphListHelper.GetParagraphListText(number, this.ParagraphListStyle);
        }

        /// <summary>
        /// 比较两个样式的边框设置是否一样
        /// </summary>
        /// <param name="style">另外一个样式对象</param>
        /// <returns>边框设置是否一样</returns>
        public bool EqualsBorderStyle(RuntimeDocumentContentStyle style)
        {
            if (style == null)
            {
                return false;
            }
            if (style == this)
            {
                return true;
            }
            if (style.BorderLeft == this.BorderLeft
                && style.BorderTop == this.BorderTop
                && style.BorderRight == this.BorderRight
                && style.BorderBottom == this.BorderBottom
                && style.BorderLeftColor == this.BorderLeftColor
                && style.BorderTopColor == this.BorderTopColor
                && style.BorderRightColor == this.BorderRightColor
                && style.BorderBottomColor == this.BorderBottomColor
                && style.BorderWidth == this.BorderWidth
                && style.BorderStyle == this.BorderStyle)
            {
                return true;
            }
            return false;
        }


        /// <summary>
        /// 获得实际使用的行间距
        /// </summary>
        /// <param name="contentHeight">文本行内容高度</param>
        /// <param name="maxFontHeight">文本行中最大的字体高度</param>
        /// <param name="documentUnit">文档采用的度量单位</param>
        /// <returns>使用的行间距</returns>
        public float GetLineSpacing(float contentHeight, float maxFontHeight, GraphicsUnit documentUnit)
        {
            //float oneRate = 0.1f;
            DCSoft.Drawing.LineSpacingStyle lss = this.LineSpacingStyle;
            if (lss == Drawing.LineSpacingStyle.SpaceSpecify)
            {
                //返回固定行距
                return this.LineSpacing;
            }
            else if (lss == Drawing.LineSpacingStyle.SpaceSpecify)
            {
                return contentHeight + maxFontHeight * 0.1f;
            }
            float result = 0;
            switch (lss)
            {
                case LineSpacingStyle.SpaceSingle:
                    result = contentHeight + maxFontHeight * 0.1f;
                    break;
                case LineSpacingStyle.Space1pt5:
                    result = contentHeight + maxFontHeight * 0.6f;
                    break;
                case LineSpacingStyle.SpaceDouble:
                    result = contentHeight + maxFontHeight * 1.1f;
                    break;
                case LineSpacingStyle.SpaceMultiple:
                    result = contentHeight + maxFontHeight * (this.LineSpacing - 1 + 0.1f);
                    break;
                case Drawing.LineSpacingStyle.SpaceExactly:
                    result = Math.Max(contentHeight, maxFontHeight);
                    break;
            }
            return result;
        }

        public DocumentContentStyle CloneParent()
        {
            return (DocumentContentStyle)this.Parent.Clone();
        }
        public DocumentContentStyle CloneWithoutBorder()
        {
            return (DocumentContentStyle)this.Parent.CloneWithoutBorder();
        }
        /// <summary>
        /// 根据对象设置绘制边框
        /// </summary>
        /// <param name="g"></param>
        /// <param name="rectangle"></param>
        public void DrawBorder(DCGraphics g, RectangleF rectangle)
        {
            float bw = this.BorderWidth;
            DashStyle ds = this.BorderStyle;
            DrawBorder(
                g,
                rectangle,
                this.BorderLeft,
                this.BorderLeftColor,
                bw,
                ds,
                this.BorderTop,
                this.BorderTopColor,
                bw,
                ds,
                this.BorderRight,
                this.BorderRightColor,
                bw,
                ds,
                this.BorderBottom,
                this.BorderBottomColor,
                bw,
                ds
                );
        }
        public static void DrawBorder(
            DCGraphics g,
            RectangleF bounds,
            bool leftBorder,
            Color leftColor,
            float leftWidth,
            DashStyle leftStyle,
            bool topBorder,
            Color topColor,
            float topWidth,
            DashStyle topStyle,
            bool rightBorder,
            Color rightColor,
            float rightWidth,
            DashStyle rightStyle,
            bool bottomBorder,
            Color bottomColor,
            float bottomWidth,
            DashStyle bottomStyle)
        {
            if (leftColor == topColor
                && topColor == rightColor
                && rightColor == bottomColor
                && leftWidth == topWidth
                && topWidth == rightWidth
                && rightWidth == bottomWidth
                && leftStyle == topStyle
                && topStyle == rightStyle
                && rightStyle == bottomStyle
                && leftBorder
                && topBorder
                && rightBorder
                && bottomBorder)
            {
                if (leftColor.A != 0 && leftWidth > 0)
                {
                    using (var p = DrawerUtil.CreatePen(leftColor, leftWidth , leftStyle ))
                    {
                        g.DrawRectangle(
                            p,
                            bounds.Left,
                            bounds.Top,
                            bounds.Width,
                            bounds.Height);
                    }

                }
            }
            else
            {
                if (leftBorder && leftColor.A != 0 && leftWidth > 0)
                {
                    using (var p = new  Pen(leftColor, leftWidth))
                    {
                        p.DashStyle = leftStyle;
                        g.DrawLine(
                            p,
                            bounds.Left,
                            bounds.Top,
                            bounds.Left,
                            bounds.Bottom);
                    }
                }
                if (topBorder && topColor.A != 0 && topWidth > 0)
                {
                    var p = new  Pen(topColor, topWidth);
                    p.DashStyle = topStyle;
                    g.DrawLine(
                        p,
                        bounds.Left,
                        bounds.Top,
                        bounds.Right,
                        bounds.Top);
                    p.Dispose();
                }
                if (rightBorder && rightColor.A != 0 && rightWidth > 0)
                {
                    var p = new  Pen(rightColor, rightWidth);
                    p.DashStyle = rightStyle;
                    g.DrawLine(
                        p,
                        bounds.Right,
                        bounds.Top,
                        bounds.Right,
                        bounds.Bottom);
                    p.Dispose();
                }
                if (bottomBorder && bottomColor.A != 0 && bottomWidth > 0)
                {
                    var p = new  Pen(bottomColor, bottomWidth);
                    p.DashStyle = bottomStyle;
                    g.DrawLine(
                        p,
                        bounds.Left,
                        bounds.Bottom,
                        bounds.Right,
                        bounds.Bottom);
                    p.Dispose();
                }
            }
        }

        /// <summary>
        /// 获得客户区边界
        /// </summary>
        /// <param name="bounds">原始边界</param>
        /// <returns>获得的客户区边界</returns>
        public RectangleF GetClientRectangleF(RectangleF bounds)
        {
            return new RectangleF(
                bounds.Left + this.PaddingLeft,
                bounds.Top + this.PaddingTop,
                bounds.Width - this.PaddingLeft - this.PaddingRight,
                bounds.Height - this.PaddingTop - this.PaddingBottom);
        }
        /// <summary>
        /// 是否存在可见的边框
        /// </summary>
        public readonly bool HasVisibleBorder = false;
        public readonly DocumentContentStyle Parent = null;
        public readonly Color Color = Color.Black;
        public Color ColorForRTF = Color.Empty;

        public readonly DCSoft.Drawing.DocumentContentAlignment Align = DocumentContentAlignment.Left;
        public readonly Color BackgroundColor = Color.Transparent;
        public readonly bool Bold = false;
        public readonly bool BorderBottom = false;
        public readonly Color BorderBottomColor = Color.Black;
        public readonly Color BorderColor = Color.Black;
        public readonly bool BorderLeft = false;
        public readonly Color BorderLeftColor = Color.Black;
        public readonly bool BorderTop = false;
        public readonly Color BorderTopColor = Color.Black;
        public readonly bool BorderRight = false;
        public readonly Color BorderRightColor = Color.Black;
        public readonly DashStyle BorderStyle = DashStyle.Solid;
        public readonly float BorderWidth = 0;
        public readonly string BulletedString = null;
        public readonly float FirstLineIndent = 0;
        public readonly XFontValue Font = null;
        public readonly Font FontValue = null;
        public readonly XFontInfo MyFontInfo = null;

        public float GetFontHeight(DCGraphics g)
        {
            if (this.FontHeight == 0)
            {
                //if (CharacterMeasurer.Options_MeasureUseTrueTypeFont)
                //{
                this.SetFontHeight(CharacterMeasurer.GetFontHeight(g, this));
                //}
                //else
                //{
                //    this.SetFontHeight(g.GetFontHeight(this.Font));// this.FontValue.GetHeight(g));
                //}
            }
            return this.FontHeight;
        }

        /// <summary>
        /// 字体高度
        /// </summary>
        public float FontHeight = 0;
        internal void SetFontHeight(float v )
        {
            if( v== 0 )
            {

            }
            this.FontHeight = v;
        }

        /// <summary>
        /// 原始的字体名称
        /// </summary>
        public readonly string FontName = XFontValue.DefaultFontName;
        /// <summary>
        /// 修正后的字体名称
        /// </summary>
        public readonly string FixedFontName = XFontValue.DefaultFontName;

        public readonly float FontSize = XFontValue.DefaultFontSize;
        public readonly FontStyle FontStyle = FontStyle.Regular;
        public readonly bool IsBulletedList = false;
        public readonly bool IsListNumberStyle = false;
        public readonly bool Italic = false;
        public readonly float LeftIndent = 0;
        public readonly float LineSpacing = 0;
        public readonly LineSpacingStyle LineSpacingStyle = LineSpacingStyle.SpaceSingle;
        public readonly float PaddingLeft = 0;
        public readonly float PaddingTop = 0;
        public readonly float PaddingRight = 0;
        public readonly float PaddingBottom = 0;
        public readonly ParagraphListStyle ParagraphListStyle = ParagraphListStyle.None;
        public readonly bool ParagraphMultiLevel = false;
        public readonly int ParagraphOutlineLevel = -1;
        public readonly float SpacingAfterParagraph = 0;
        public readonly float SpacingBeforeParagraph = 0;
        public readonly bool Strikeout = false;
        public readonly bool Subscript = false;
        public readonly bool Superscript = false;
        public readonly bool Underline = false;
        public readonly VerticalAlignStyle VerticalAlign = VerticalAlignStyle.Top;
        
        public void Dispose()
        {
        }
    }
}