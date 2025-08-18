using System;
using DCSoft.Drawing;
using System.Text;

namespace DCSoft.Writer.Dom
{
    /// <summary>
    /// 单个字符对象
    /// </summary>
    /// <remarks>编写 袁永福</remarks>
#if !RELEASE
    [System.Diagnostics.DebuggerDisplay("Char:{CharValue}")]
#endif
    public partial class DomCharElement : DomElement
    {
        public static readonly string TypeName_XTextCharElement = "XTextCharElement";
        public override string TypeName => TypeName_XTextCharElement;

        /// <summary>
        ///  &nbsp;
        /// </summary>
        /// <remarks>
        /// 不换行空格，全称No-Break Space，它是最常见和我们使用最多的空格，
        /// 大多数的人可能只接触了&nbsp;，它是按下space键产生的空格。在HTML中，
        /// 如果你用空格键产生此空格，空格是不会累加的（只算1个）。要使用html
        /// 实体表示才可累加，该空格占据宽度受字体影响明显而强烈。
        /// </remarks>
        public const char CHAR_NBSP = (char)160;
        /// <summary>
        /// @ensp
        /// </summary>
        /// <remarks>
        /// “半角空格”，全称是En Space，en是字体排印学的计量单位，为em宽度的一半。
        /// 根据定义，它等同于字体度的一半（如16px字体中就是8px）。名义上是小写字母n
        /// 的宽度。此空格传承空格家族一贯的特性：透明的，此空格有个相当稳健的特性，
        /// 就是其占据的宽度正好是1/2个中文宽度，而且基本上不受字体影响。
        /// </remarks>
        public const char CHAR_ENSP = (char)8194;
        /// <summary>
        /// &emsp
        /// </summary>
        /// <remarks>
        /// “全角空格”，全称是Em Space，em是字体排印学的计量单位，相当于当前指定的点数。
        /// 例如，1 em在16px的字体中就是16px。此空格也传承空格家族一贯的特性：透明的，
        /// 此空格也有个相当稳健的特性，就是其占据的宽度正好是1个中文宽度，而且基本上
        /// 不受字体影响。
        /// </remarks>
        public const char CHAR_EMSP = (char)8195;
        /// <summary>
        /// thinsp
        /// </summary>
        /// <remarks>
        /// 窄空格，全称是Thin Space。我们不妨称之为“瘦弱空格”，就是该空格长得比较瘦弱，
        /// 身体单薄，占据的宽度比较小。它是em之六分之一宽。
        /// </remarks>
        public const char CHAR_THINSP = (char)8201;
        /// <summary>
        /// zwnj
        /// </summary>
        /// <remarks>
        /// 零宽不连字，全称是Zero Width Non Joiner，简称“ZWNJ”，是一个不打印字符，放在
        /// 电子文本的两个字符之间，抑制本来会发生的连字，而是以这两个字符原本的字形来绘制。
        /// Unicode中的零宽不连字字符映射为“”（zero width non-joiner，U+200C），
        /// HTML字符值引用为： &#8204;
        /// </remarks>
        public const char CHAR_ZWNJ = (char)8204;
        /// <summary>
        /// zwj
        /// </summary>
        /// <remarks>
        /// 零宽连字，全称是Zero Width Joiner，简称“ZWJ”，是一个不打印字符，放在某些需要复杂
        /// 排版语言（如阿拉伯语、印地语）的两个字符之间，使得这两个本不会发生连字的字符产生了
        /// 连字效果。零宽连字符的Unicode码位是U+200D (HTML: &#8205; &zwj;）。
        /// </remarks>
        public const char CHAR_ZWJ = (char)8205;
        /// <summary>
        /// 标准空格
        /// </summary>
        public const char CHAR_WHITESPACE = (char)32;
        /// <summary>
        /// 制表符
        /// </summary>
        public const char CHAR_TAB = (char)8;

        /// <summary>
        /// 初始化对象
        /// </summary>
        public DomCharElement()
        {
        }
        /// <summary>
        /// 初始化对象
        /// </summary>
        /// <param name="c">字符值</param>
        /// <param name="styleIndex">样式编号</param>
        public DomCharElement(char c, int styleIndex)
        {
            this._CharValue = c;
            this._StyleIndex = styleIndex;
        }
        /// <summary>
        /// 初始化对象
        /// </summary>
        /// <param name="c">字符值</param>
        /// <param name="parent">父节点</param>
        /// <param name="document">文档对象</param>
        /// <param name="styleIndex">样式编号</param>
        internal DomCharElement(char c, DomContainerElement parent, DomDocument document, int styleIndex)
        {
            this._CharValue = c;
            this._Parent = parent;
            this._OwnerDocument = document;
            this._StyleIndex = styleIndex;
        }

        /// <summary>
        /// 是否为HTML中的特殊的空白字符
        /// </summary>
        /// <returns></returns>
        public static bool IsHtmlWhitespace(char c)
        {
            if (c == 32 || c == CHAR_TAB)
            {
                return true;
            }
            if (c > 8000)
            {
                return c == CHAR_EMSP
                    || c == CHAR_ENSP
                    || c == CHAR_THINSP
                    || c == CHAR_ZWJ
                    || c == CHAR_ZWNJ;
            }
            else
            {
                return c == CHAR_NBSP;
            }
        }

        internal char _CharValue;
        /// <summary>
        /// 字符数据
        /// </summary>
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public char CharValue
        {
            get
            {
                return this._CharValue;
            }
            set
            {
                this._CharValue = value;
            }
        }

        /// <summary>
        /// 获得字符值
        /// </summary>
        /// <returns>字符值</returns>
        public char GetCharValue()
        {
            return this._CharValue;
        }

        /// <summary>
        /// 字符整数数值
        /// </summary>
        public int CharInt32Value
        {
            get
            {
                return (int)_CharValue;
            }
        }

        /// <summary>
        /// 表示对象的文本
        /// </summary>
        public override string Text
        {
            get
            {
                    return DCSoft.Common.StringCommon.CharToString(this._CharValue);// this._CharValue.ToString();
            }
            set
            {
                if (value != null && value.Length > 0)
                {
                    this._CharValue = value[0];
                }
            }
        }
        public override void InnerGetText(GetTextArgs args)
        {
                args.Append(this._CharValue);
        }
        public override void WriteText(WriteTextArgs args)
        {
                args.Append(this._CharValue);
        }


        public override void Draw(InnerDocumentPaintEventArgs args)
        {
            //args.Document.AddRenderedElement(this);
            args.LastHighlightInfo = null;
            var vb = args.ViewBounds;
            vb.Width = this._Width + this.WidthFix;
            args.Render.DrawBackground(this, args, vb);
            if (args.SVG != null)
            {
                DrawCharElementContent(args);
            }
            else
            {
                if (this._CharValue == CHAR_EMSP
                    || this._CharValue == CHAR_ENSP
                    || this._CharValue == CHAR_NBSP
                    || this._CharValue == CHAR_THINSP
                    || this._CharValue == CHAR_ZWJ
                    || this._CharValue == CHAR_ZWNJ
                    || this._CharValue == CHAR_WHITESPACE
                    || this._CharValue == CHAR_TAB)
                {
                    var rs = this.RuntimeStyle;
                    if (rs.Underline || rs.Strikeout )
                    {
                        //args.Render.DrawCharElementContent(this, args);
                        DrawCharElementContent(args);
                    }
                    return;
                }
                DrawCharElementContent(args);
            }
            //if( this._CharValue < 127 )
            //{
            //    args.Graphics.DrawRectangle(Pens.Red, vb.Left, vb.Top, vb.Width, vb.Height);
            //}
        }


        /// <summary>
        /// 绘制对象
        /// </summary>
        /// <param name="args">参数对象</param>
        private void DrawCharElementContent(InnerDocumentPaintEventArgs args)
        {
            var charValue = this._CharValue;
            //if( charValue =='2')
            //{
            //    var s = "ff";
            //}
            if (charValue == '\u0001')
            {
                // 空白的字符
                var jiejie = "JIEJIE.NET.SWITCH:-controlfow";
                return;
            }
            //var bolDebugFlag = false;
            //if (args.RenderMode == InnerDocumentRenderMode.Print)
            //{
            //    if (charValue == 9)
            //    {
            //        var ss = "fff";
            //        bolDebugFlag = true;
            //    }
            //    Console.WriteLine(charValue.ToString());
            //}

            var isWhitespaceChar = args.SVG == null && IsHtmlWhitespace(this._CharValue);
            RuntimeDocumentContentStyle rs = args.Style;
            //if( isWhitespaceChar )
            //{
            //    if( rs.Underline || rs.Strikeout )
            //    {
            //        isWhitespaceChar = false;
            //    }
            //}
            float topFix = 0;
            var absBounds = args.ViewBounds;// chr.AbsBounds;
            var rect = absBounds;
            rect.Y += args.CharMeasurer_CharTopFix;
            rect.Height *= 1.5f;
            rect.Width *= 1.5f;
            var cc = rs.Color;
            if (this.IsBackgroundText)
            {
                // 为背景文字
                if (args.DrawFieldBackgroundText)
                {
                    // 允许绘制输入域背景文字
                    cc = DocumentViewOptions._BackgroundTextColor;
                }
                else
                {
                    return;// cc = Color.Transparent;
                }
            }
            if (args.RenderMode == InnerDocumentRenderMode.Paint)
            {
                HighlightInfo info = null;
                if (args._LastCharElement == this)
                {
                    info = args._LastHighlightInfoForCharElement;
                }
                else
                {
                    info = args.LastHighlightInfo;
                    if (info == null && args.HighlightManager != null)
                    {
                        info = args.HighlightManager[this];
                    }
                }
                if (info != null && info.Color.A != 0)
                {
                    // 设置高亮度文本值
                    cc = info.Color;
                }
            }
            var bolDrawSingleChar = true;
            string text = null;// DCSoft.Common.StringCommon.CharToString(charValue);// charValue.ToString();
            var charColor = cc;// args.GetRuntimeForeColor(cc);// this._OwnerDocument.GetRuntimeForeColor(cc);
            // 首先判断运行的字体是否需要改变
            bool needChangeFont = false;
            if (this.StateFlag10)
            {
                needChangeFont = true;
            }
            else if (rs.Superscript || rs.Subscript)
            {
                needChangeFont = true;
            }
            else if (rs.Underline || rs.Strikeout)
            {
                needChangeFont = true;
            }
            XFontValue runtimeFont = rs.Font;
            if (needChangeFont)
            {
                runtimeFont = runtimeFont.Clone();
                runtimeFont.Underline = false;
                runtimeFont.Strikeout = false;
                if (rs.Subscript || rs.Superscript)
                {
                    runtimeFont.Size = runtimeFont.Size * 0.5f;
                }
                if (this.StateFlag10) // DUWRITER5_0-4086
                {
                    // 临时修改了字体名称
                    var strFontName5 = CharacterMeasurer.GetDefaultFontName(this._CharValue);
                    if (strFontName5 != null)
                    {
                        runtimeFont.Name = strFontName5;
                    }
                }
            }
            var format = args.DrawCharFormat;// .Render.DrawCharFormat();
            //if (bolDebugFlag)
            //{
            //    Console.WriteLine("4444444444444444");
            //}
            //format.Color = args.Document.GetRuntimeForeColor(cc);
            if (rs.Subscript || rs.Superscript)
            {
                // 存在上下标样式
                //var isStrikeout = runtimeFont.Strikeout;
                runtimeFont.Strikeout = false;
                var runtimeFontHeight = args.GraphicsGetFontHeight(runtimeFont);
                bolDrawSingleChar = false;
                text = DCSoft.Common.StringCommon.CharToString(charValue);
                if (rs.Superscript)
                {
                    if (args.RenderMode == InnerDocumentRenderMode.Print)
                    {
                        if (isWhitespaceChar == false)
                        {
                            args.Graphics.DrawString(
                                text,
                                runtimeFont,
                                charColor,
                                rect.Left,
                                rect.Top - runtimeFontHeight * 0.1f,
                                format);
                        }
                        if (rs.Strikeout)
                        {
                            // 绘制删除线效果
                            var top2 = rect.Top + runtimeFontHeight * 0.38f;
                            args.Graphics.DrawLine(
                                charColor,
                                rect.Left,
                                top2,
                                rect.Left + this.Width + this.WidthFix,
                                top2);
                        }
                    }
                    else
                    {
                        if (isWhitespaceChar == false)
                        {
                            args.Graphics.DrawString(text,
                                runtimeFont,
                                charColor,
                                rect.Left,
                                rect.Top + runtimeFontHeight * 0.4F,
                                format);
                        }
                        if (rs.Strikeout)
                        {
                            // 绘制删除线效果
                            var top2 = rect.Top + runtimeFontHeight * 0.38f;
                            args.Graphics.DrawLine(
                                charColor,
                                rect.Left,
                                top2,
                                rect.Left + this.Width + this.WidthFix,
                                top2);
                        }
                    }
                }
                else
                {
                    //float fh;
                    //if (runtimeFont.Size == rs.FontSize)
                    //{
                    //    fh = args.GraphicsGetFontHeight(rs);
                    //}
                    //else
                    //{
                    //    fh = args.GraphicsGetFontHeight(runtimeFont);// font.GetHeight(args.Graphics.PageUnit);
                    //}

                    if (args.RenderMode == InnerDocumentRenderMode.Print)
                    {
                        args.Graphics.DrawString(
                           text,
                           runtimeFont,
                           charColor,
                           rect.Left,
                           (float)Math.Floor(rect.Top + runtimeFontHeight * 1f),
                           //new RectangleF(
                           //    rect.Left,
                           //    (float)Math.Floor( absBounds.Bottom - runtimeFontHeight *1.5f), //(float)Math.Floor(rect.Top + (rect.Height * 0.4)),
                           //    rect.Width,
                           //    rect.Height),
                           format);
                        if (rs.Strikeout)
                        {
                            // 绘制删除线效果
                            var top2 = rect.Top + runtimeFontHeight * 1.5f;// rect.Top + runtimeFontHeight * 0.2f;// rect.Top + runtimeFontHeight * 0.2f;
                            args.Graphics.DrawLine(
                                charColor,
                                rect.Left,
                                top2,
                                rect.Left + this.Width + this.WidthFix,
                                top2);
                        }
                    }
                    else
                    {
                        var charTop = (float)Math.Floor(rect.Bottom - runtimeFontHeight * 1.5f);
                        {
                            args.Graphics.DrawString(
                                text,
                                runtimeFont,
                                charColor,
                                rect.Left,
                                charTop,
                                //(float)Math.Floor(rect.Top + (rect.Height * 0.4))  ,
                                format);
                        }
                        if (rs.Strikeout)
                        {
                            // 绘制删除线效果
                            var top2 = charTop - runtimeFontHeight * 0.03f;// rect.Top + runtimeFontHeight * 0.2f;// rect.Top + runtimeFontHeight * 0.2f;
                            args.Graphics.DrawLine(
                                charColor,
                                rect.Left,
                                top2,
                                rect.Left + this.Width + this.WidthFix,
                                top2);
                        }
                    }
                }
            }
            else
            {
                if (isWhitespaceChar == false)
                {
                    if (bolDrawSingleChar == false)
                    {
                        if (args.RenderMode == InnerDocumentRenderMode.Print)
                        {
                            args.Graphics.DrawString(
                               text,
                               runtimeFont,
                               charColor,
                               new RectangleF(rect.Left, rect.Top + topFix, rect.Width, rect.Height),
                               args.DrawCharFormat);
                        }
                        else
                        {
                            //args.NativeGraphics?.SetTextBaseline(GraphicsTextBaseline.hanging); 
                            args.Graphics.DrawString(
                              text,
                              runtimeFont,
                              charColor,
                              new RectangleF(rect.Left, rect.Top + topFix, rect.Width, rect.Height),
                              args.DrawCharFormat);
                            //args.NativeGraphics?.SetTextBaseline(GraphicsTextBaseline.Top);
                        }
                    }
                    else
                    {
                        // 在最有可能执行的地方特殊处理。
                        if (args.NativeGraphics != null)
                        {
                            if (needChangeFont)
                            {
                                args.NativeGraphics.FastDrawChar(
                                   charValue,
                                   runtimeFont.CreateFont(),
                                   charColor,
                                   rect.Left,
                                   rect.Top + topFix,
                                   0);
                            }
                            else
                            {
                                args.NativeGraphics.FastDrawChar(
                                    charValue,
                                    rs.FontValue,
                                    charColor,
                                    rect.Left,
                                    rect.Top + topFix,
                                    rs.FontHeight);
                            }
                        }
                        else if (args.SVG != null)
                        {
                            if (needChangeFont)
                            {
                                args.SVG.FastDrawChar(
                                   charValue,
                                   this._StyleIndex,
                                   runtimeFont.Name,
                                   runtimeFont.Size,
                                   runtimeFont.Style,
                                   charColor,
                                   rect.Left,
                                   rect.Top + topFix,
                                   0);
                            }
                            else
                            {
                                args.SVG.FastDrawChar(
                                    charValue,
                                    this._StyleIndex,
                                    rs.FontName,
                                    rs.FontSize,
                                    rs.FontStyle,
                                    charColor,
                                    rect.Left,
                                    rect.Top + topFix,
                                    rs.FontHeight);
                            }
                        }
                        else
                        {
                            args.Graphics.DrawString(
                                DCSoft.Common.StringCommon.CharToString(charValue),//    text,
                               runtimeFont,
                               charColor,
                               new RectangleF(rect.Left, rect.Top + topFix, rect.Width, rect.Height),
                               args.DrawCharFormat);
                            //}
                        }
                        if (rs.Strikeout)
                        {
                            // 绘制删除线效果
                            var top2 = rect.Top + args.GraphicsGetFontHeight(runtimeFont) / 2;
                            args.Graphics.DrawLine(
                                charColor,
                                rect.Left,
                                top2,
                                rect.Left + this.Width + this.WidthFix,
                                top2);
                        }
                    }
                }
                if (rs.Strikeout)
                {
                    // 绘制删除线效果
                    var top2 = rect.Top + args.GraphicsGetFontHeight(runtimeFont) / 2;
                    args.Graphics.DrawLine(
                        charColor,
                        rect.Left,
                        top2,
                        rect.Left + this.Width + this.WidthFix,
                        top2);
                }
            }
            if (rs.Underline)
            {
                // 绘制下划线
                args.Render.DrawUnderLine(this, args, cc, absBounds);
            }
        }



        public override void RefreshSize(InnerDocumentPaintEventArgs args)
        {
            //this.OwnerDocument.ElementsRendered.AddCheckContains(this);
            if (this._CharValue == CHAR_ZWJ || this._CharValue == CHAR_ZWNJ)
            {
                this.Width = 0;
                this.Height = this.RuntimeStyle.FontValue.GetHeight(args.NativeGraphics);
            }
            else
            {
                var chrValue = this._CharValue;
                if (chrValue == DomCharElement.CHAR_EMSP)
                {
                    chrValue = (char)34945;// '袁';
                }
                else if (chrValue == DomCharElement.CHAR_ENSP)
                {
                    chrValue = 'n';
                }
                RuntimeDocumentContentStyle rs = this.RuntimeStyle;
                if (rs == null)
                {
                    return;
                }
                this.StateFlag10 = false;
                var size = args.CharMeasurer.Measure(
                    chrValue,
                    args.NativeGraphics,
                    args.GraphicsPageUnit,
                    rs,
                    1);
                if( CharacterMeasurer.ChangeToNewFontName != null )
                {
                    this.StateFlag10 = true;
                }
                if( size.Width == 0 && chrValue > 9794)
                {
                    // 遇到不支持的大编码的字符，当做汉字来处理。
                    size = args.CharMeasurer.Measure(
                        '袁',
                        args.NativeGraphics, 
                        args.GraphicsPageUnit, 
                        rs, 
                        1);
                }
                //if (args.SpecifyDebugMode)// _DocumentOptions.BehaviorOptions.SpecifyDebugMode)
                //{
                //    XFontValue font = rs.GetFastFont(this._FontSizeZoomRate);
                //    SizeF size5 = CharacterMeasurer.MeasureString(
                //        args.Graphics,
                //        this._CharValue.ToString(),
                //        font,
                //        10000,
                //        DCStringFormat.GenericTypographic);
                //    this._NativeHeight = size5.Height;
                //    this._NativeWidth = size5.Width;
                //}
                this._Width = size.Width;
                //this._FontHeight = size.Height;// - cmr.CharTopFix;// rs.Font.GetHeight(g);
                this._Height = size.Height;// this._FontHeight;// size.Height;
                if (rs.Superscript || rs.Subscript)
                {
                    this._Width = this._Width * 0.6f;
                }
                this.SizeInvalid = false;
            }
        }

            /// <summary>
            /// 处理文档用户界面事件
            /// </summary>
            /// <param name="args">事件参数</param>

        public override void HandleDocumentEvent(DocumentEventArgs args)
        {
            if (this.Parent is DomFieldElementBase)
            {
                DomFieldElementBase field = (DomFieldElementBase)this.Parent;
                if (field.IsBackgroundTextElement(this))
                {
                    // 背景元素没有事件
                    return;
                }
            }
            if (args.Style == DocumentEventStyles.LostFocus
                || args.Style == DocumentEventStyles.GotFocus
                || args.Style == DocumentEventStyles.LostFocusExt)
            {
                // 字符元素不触发焦点事件
            }
            else
            {
                base.HandleDocumentEvent(args);
            }
        }

        internal void SetWidthForTab()
        {
            if (_CharValue == '\t')
            {
                this.Width = WriterUtilsInner.GetTabWidth(
                    this.Left - this.OwnerDocument.Left,
                    this.OwnerDocument.DefaultStyle.TabWidth);
            }
        }


        /// <summary>
        /// 是否为输入域背景文本字符
        /// </summary>
        public bool IsBackgroundText
        {
            get { return this.StateFlag04; }
            set { this.StateFlag04 = value; }
        }
        /// <summary>
        /// 获得纯文本内容
        /// </summary>
        /// <returns>纯文本内容</returns>

        public override string ToPlaintString()
        {
            return _CharValue.ToString();
        }

        /// <summary>
        /// 获得表示对象的文本
        /// </summary>
        /// <returns>文本</returns>

        public override string ToString()
        {
                return _CharValue.ToString();
        }
#if !RELEASE
        public override string ToDebugString()
        {
                return "[" + this._ContentIndex + "]" + _CharValue.ToString() + " [" + this.CharInt32Value.ToString() + "]";
        }
#endif
        /// <summary>
        /// 将对象文本添加到一个字符串创建器中，本函数考虑到了UNICODE代理的情况
        /// </summary>
        /// <param name="str"></param>
        internal void AppendContent(System.Text.StringBuilder str)
        {
            if( this._CharValue >= 8)
            {
                str.Append(this._CharValue);
            }
        }
    }//public class XTextChar : XTextElement , IFontable

}