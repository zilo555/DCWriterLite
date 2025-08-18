using System;
using System.Collections.Generic;
using System.Text;
using DCSoft.Drawing;
using DCSoft.Common;
using System.IO;

namespace DCSoft.Writer.Dom
{

    /// <summary>
    /// 字符大小测量对象
    /// </summary>
    public class CharacterMeasurer //: IDisposable
    {
        private static readonly Dictionary<char, float> _SpecifyCharWidths = new Dictionary<char, float>();
        /// <summary>
        /// 设置特殊字符的宽度
        /// </summary>
        /// <param name="c"></param>
        /// <param name="width"></param>
        public static void SetSpecifyCharWidth(char c, float width)
        {
            _SpecifyCharWidths[c] = width;
        }

        ///// <summary>
        ///// 使用TrueTypeFont文件内容进行字符测量
        ///// </summary>
        //public const bool Options_MeasureUseTrueTypeFont = true;// DebugHelper.IsWindowsPlatform == false;

        public const int ChineseStartCode = 19968;
        public const int ChineseEndCode = 40869;
        public const int EnglishStartCode = 32;
        public const int EnglishEndCode = 127;
        public const float BoldFontWidthFixRate = 1.05f;


        static CharacterMeasurer()
        {
            myMeasureFormat = new StringFormat(
                     StringFormat.GenericTypographic);
        }

        public static DCSystem_Drawing.RectangleF[] LayoutString(
            DCSystem_Drawing.GraphicsUnit pageUnit,
            string text,
            string fontName,
            float fontSize,
            DCSystem_Drawing.FontStyle vStyle,
            DCSystem_Drawing.RectangleF bounds,
            DCSystem_Drawing.StringFormat format,
            out string[] lines)
        {
            if (string.IsNullOrEmpty(text))// == null || text.Length == 0)
            {
                lines = null;
                return null;
            }
            DCSystem_Drawing.SizeF outSize = DCSystem_Drawing.SizeF.Empty;
            var lineWidths = new List<float>();
            lines = StaticSplitToLines(
                pageUnit,
                text,
                fontName,
                fontSize,
                vStyle,
                bounds.Width,
                format,
                out outSize,
                lineWidths);
            var topCount = bounds.Top;
            var align = format == null ? StringAlignment.Near : format.Alignment;
            var vAlign = format == null ? StringAlignment.Near : format.LineAlignment;
            if (vAlign == StringAlignment.Center)
            {
                topCount = bounds.Top + (bounds.Height - outSize.Height) / 2;
            }
            else if (vAlign == StringAlignment.Far)
            {
                topCount = bounds.Bottom - outSize.Height;
            }
            var lineHeight = outSize.Height / lines.Length;
            var result = new DCSystem_Drawing.RectangleF[lines.Length];
            for (var iCount = 0; iCount < lines.Length; iCount++)
            {
                if (align == StringAlignment.Far)
                {
                    result[iCount] = new DCSystem_Drawing.RectangleF(
                        bounds.Right - lineWidths[iCount],
                        topCount,
                        lineWidths[iCount],
                        lineHeight);
                }
                else if (align == StringAlignment.Center)
                {
                    result[iCount] = new DCSystem_Drawing.RectangleF(
                        bounds.Left + (bounds.Width - lineWidths[iCount]) / 2,
                        topCount,
                        lineWidths[iCount],
                        lineHeight);
                }
                else
                {
                    result[iCount] = new DCSystem_Drawing.RectangleF(
                        bounds.Left,
                        topCount,
                        lineWidths[iCount],
                        lineHeight);
                }
                topCount += lineHeight;
            }
            return result;
        }
        /// <summary>
        /// 将一个文本拆成按行处理的字符串数组
        /// </summary>
        /// <param name="g">画布对象</param>
        /// <param name="txt">文本</param>
        /// <param name="font">字体</param>
        /// <param name="layoutWidth">布局宽度</param>
        /// <param name="format">格式化字符串</param>
        /// <returns>字符串数组</returns>
        public static string[] StaticSplitToLines(
            GraphicsUnit intPageUnit,
            string txt,
            string fontName,
            float fontSize,
            FontStyle vStyle,
            float layoutWidth,
            StringFormat format,
            out SizeF layoutSize,
            List<float> lineWidths)
        {
            var formatFlags = format == null ? (StringFormatFlags)0 : format.FormatFlags;
            layoutSize = DCSystem_Drawing.SizeF.Empty;
            if (string.IsNullOrEmpty(txt))
            {
                return null;
            }
            DCSoft.TrueTypeFontSnapshort fontInfo = null;
            if (formatFlags != StringFormatFlags.NoWrap || lineWidths != null)
            {
                fontInfo = DCSoft.Writer.Dom.CharacterMeasurer.GetFontInfo(fontName, vStyle);
            }
            if (layoutWidth > 0
                && (formatFlags & StringFormatFlags.NoWrap) != StringFormatFlags.NoWrap)
            {
                // 允许自动换行
                var bolHasWarp = txt.Contains('\n');
                if (bolHasWarp == false && layoutWidth > 0)
                {
                    var totalWidth2 = DCSoft.Writer.Dom.CharacterMeasurer.MeasureSingleLineStringUseTrueTypeFont(
                        intPageUnit,
                        txt,
                        fontInfo,
                        fontSize,
                        vStyle).Width;
                    bolHasWarp = totalWidth2 > layoutWidth && txt.Length > 1;
                }
                if (bolHasWarp)
                {
                    // 如果文本总宽度超过布局宽度，或者有换行符，则出现多行文本
                    // 首先拆成多个单词
                    var strWords = new List<string>();
                    var strCurrentWord = new StringBuilder();
                    for (int iCount = 0; iCount < txt.Length; iCount++)
                    {
                        var c = txt[iCount];
                        if (c == '\n')
                        {
                            // 出现换行符
                            if (strCurrentWord.Length > 0)
                            {
                                strWords.Add(strCurrentWord.ToString());
                                strCurrentWord = new StringBuilder();
                            }
                            strWords.Add("\n");
                        }
                        else if (c == '\r')
                        {

                        }
                        else if (char.IsLetterOrDigit(c) && c < 128)
                        {
                            // 英文或者数字
                            strCurrentWord.Append(c);
                        }
                        else
                        {
                            if (strCurrentWord.Length > 0)
                            {
                                strWords.Add(strCurrentWord.ToString());
                                strCurrentWord = new StringBuilder();
                            }
                            strWords.Add(c.ToString());
                        }
                    }//for
                    if (strCurrentWord.Length > 0)
                    {
                        strWords.Add(strCurrentWord.ToString());
                    }
                    // 不能出现行首的字符
                    string _TailSymbols = "!),.:;?]}¨·ˇˉ―‖’”…∶、。〃々〉》」』】〕〗！＂＇），．：；？］｀｜｝～￠";
                    // 不能出现行尾的字符
                    string _HeadSymbols = "([{·‘“〈《「『【〔〖（．［｛￡￥";
                    // 尽量按照单词来进行排版
                    var lines = new List<StringBuilder>();
                    StringBuilder strCurrentLine = new StringBuilder();
                    lines.Add(strCurrentLine);
                    float totalWidth = 0;
                    float maxWidth = 0;
                    for (int wordIndex = 0; wordIndex < strWords.Count; wordIndex++)
                    {
                        if (lines.Count > 10000)
                        {
                            // 超出处理能力了。
                            break;
                        }
                        var strCurWord = strWords[wordIndex];
                        if (strCurWord == "\n")
                        {
                            // 强制换行
                            strCurrentLine = new StringBuilder();
                            lines.Add(strCurrentLine);
                            if (lineWidths != null)
                            {
                                lineWidths.Add(totalWidth);
                            }
                            totalWidth = 0;
                            continue;
                        }
                        if (strCurrentLine.Length == 0
                            && _TailSymbols.Contains(strCurWord[0])
                            && lines.Count > 0)
                        {
                            // 行首出现不应该出现的字符，则将上一行最后一个字符给借过来
                            var preLine = lines[lines.Count - 1];
                            if (preLine.Length > 3)
                            {
                                // 上一行足够长才借，不长不借。
                                strCurWord = string.Concat(preLine[preLine.Length - 1].ToString(), strCurWord);
                                strWords[wordIndex] = strCurWord;
                                preLine.Remove(preLine.Length - 1, 1);
                            }
                        }
                        var cw = DCSoft.Writer.Dom.CharacterMeasurer.MeasureSingleLineStringUseTrueTypeFont(
                            intPageUnit,
                            strCurWord,
                            fontInfo,
                            fontSize,
                            vStyle).Width;
                        if (cw == 0 && strCurWord == " ")
                        {
                            // 测量空格字符的宽度
                            cw = fontInfo.GetCharWidth('i', fontSize, intPageUnit);
                        }
                        if (totalWidth + cw > layoutWidth)
                        {
                            // 当前行无法完整的容纳当前单词，则换行
                            if (totalWidth < layoutWidth * 0.3)
                            {
                                // 当前行还有大段空白区域，则认为出现超长单词，则单词内断开换行
                                float TextWidth2 = 0;
                                for (var charIndex = 0; charIndex < strCurWord.Length; charIndex++)
                                {
                                    float cw2;
                                    cw2 = fontInfo.GetCharWidth(strCurWord[charIndex], fontSize, intPageUnit);
                                    TextWidth2 += cw2;
                                    if (totalWidth + TextWidth2 > layoutWidth)
                                    {
                                        if (charIndex == 0)
                                        {
                                            // 连一个字符都放不下，则强制放一个字符
                                            charIndex = 1;
                                        }
                                        strCurrentLine.Append(strCurWord, 0, charIndex);
                                        strWords[wordIndex] = strCurWord.Substring(charIndex);
                                        break;
                                    }
                                }//while
                            }
                            else
                            {
                                if (strCurrentLine.Length > 3
                                    && _HeadSymbols.Contains(strCurrentLine[strCurrentLine.Length - 1]))
                                {
                                    // 行尾出现不能出现的字符，则移动这个字符到下一行
                                    strCurrentLine.Remove(strCurrentLine.Length - 1, 1);
                                    wordIndex--;
                                }
                            }
                            // 完成当前行，开始添加新的行
                            wordIndex--;
                            strCurrentLine = new StringBuilder();
                            lines.Add(strCurrentLine);
                            if (lineWidths != null)
                            {
                                lineWidths.Add(totalWidth);
                            }
                            totalWidth = 0;
                        }
                        else
                        {
                            // 将当前单词添加到当前行中
                            strCurrentLine.Append(strCurWord);
                            totalWidth += cw;
                            if (maxWidth < totalWidth)
                            {
                                maxWidth = totalWidth;
                            }
                        }
                    }//for
                    if (lines[lines.Count - 1].Length == 0)
                    {
                        lines.RemoveAt(lines.Count - 1);
                    }
                    if (lineWidths != null && lineWidths.Count < lines.Count)
                    {
                        lineWidths.Add(totalWidth);
                    }
                    layoutSize = new SizeF(
                        maxWidth,
                        lines.Count * fontInfo.GetFontHeight(fontSize, intPageUnit));
                    var result = new string[lines.Count];
                    for (var iCount = 0; iCount < lines.Count; iCount++)
                    {
                        if (lines[iCount].Length == 0)
                        {
                            result[iCount] = string.Empty;
                        }
                        else
                        {
                            result[iCount] = lines[iCount].ToString();
                        }
                    }
                    lines.Clear();
                    return result;
                }
                else
                {
                    layoutSize = DCSoft.Writer.Dom.CharacterMeasurer.MeasureSingleLineStringUseTrueTypeFont(
                        intPageUnit,
                        txt,
                        fontInfo,
                        fontSize,
                        vStyle);
                    if (lineWidths != null)
                    {
                        lineWidths.Add(layoutSize.Width);
                    }
                }
            }
            else
            {
                if (lineWidths != null)
                {
                    layoutSize = DCSoft.Writer.Dom.CharacterMeasurer.MeasureSingleLineStringUseTrueTypeFont(
                                intPageUnit,
                                txt,
                                fontInfo,
                                fontSize,
                                vStyle);
                    lineWidths.Add(layoutSize.Width);
                }
            }
            // 单行文本
            _OneStringArrar[0] = txt;
            return _OneStringArrar;
            //return new string[] { txt };
        }
        private static readonly string[] _OneStringArrar = new string[1];

        /// <summary>
        /// 判断是否是中文
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        private static bool IsChinese(char c)
        {
            //if (c > 8216)
            //{
            //    return true;
            //}
            if (c >= ChineseStartCode && c <= ChineseEndCode)// 19968 <= c && c < 40869)
            {
                return true;
            }

            if (c < 127)
            {
                return false;
            }
            if (_ChineseStr.Contains(c))
            {
                return true;
            }
            return false;
        }

        private static readonly string _ChineseStr = "‘’“”；、，。？：＋（）　－";
        //public CharacterMeasurer()
        //{


        //}

        //internal static void NothingMethod()
        //{

        //}

        public static DCSystem_Drawing.SizeF MeasureString(Graphics g, string text, XFontValue f, int width, StringFormat format)
        {
            //if (Options_MeasureUseTrueTypeFont)
            //{
            return MeasureStringUseTrueTypeFont(g.PageUnit, text, f.Name, f.Size, f.Style, width, format);
            //}
            //else
            //{
            //    return g.MeasureString(text, f.Value, width, format?.Value());
            //}
        }

        public static DCSystem_Drawing.SizeF MeasureString(DCGraphics g, string text, XFontValue f, int width, StringFormat format)
        {
            //if (Options_MeasureUseTrueTypeFont)
            //{
            return MeasureStringUseTrueTypeFont(g.PageUnit, text, f.Name, f.Size, f.Style, width, format);
            //}
            //else
            //{
            //    return g.MeasureString(text, f, width, format);
            //}
        }


        public static float GetFontHeight(DCGraphics g, RuntimeDocumentContentStyle rs)
        {
            if (g == null)
            {
                throw new ArgumentNullException("g");
            }
            if (rs == null)
            {
                throw new ArgumentNullException("rs");
            }
            //if (Options_MeasureUseTrueTypeFont)
            //{
            var info = TrueTypeFontSnapshort.GetInstance(FixFontName(rs.FontName), rs.FontStyle);
            return info.GetFontHeight(rs.FontSize, g.PageUnit);
            //}
            //else
            //{
            //    return rs.FontValue.GetHeight(g.NativeGraphics);
            //}
        }
        public static float GetFontHeight(Graphics g, XFontValue f)
        {
            if (g == null)
            {
                throw new ArgumentNullException("g");
            }
            if (f == null)
            {
                throw new ArgumentNullException("f");
            }
            //if (Options_MeasureUseTrueTypeFont)
            //{
            var info = TrueTypeFontSnapshort.GetInstance(FixFontName(f.Name), f.Style);
            return info.GetFontHeight(f.Size, g.PageUnit);
            //}
            //else
            //{
            //    return f.Value.GetHeight(g);
            //}
        }
        public static float GetFontHeightUseTrueTypeFont(GraphicsUnit pageUnit, XFontValue f)
        {
            if (f == null)
            {
                throw new ArgumentNullException("f");
            }
            var info = TrueTypeFontSnapshort.GetInstance(FixFontName(f.Name), f.Style);
            return info.GetFontHeight(f.Size, pageUnit);
        }
        public static float GetFontHeightUseTrueTypeFont(GraphicsUnit pageUnit, DCSystem_Drawing.Font f)
        {
            if (f == null)
            {
                throw new ArgumentNullException("f");
            }
            var info = TrueTypeFontSnapshort.GetInstance(FixFontName(f.Name), f.Style);
            return info.GetFontHeight(f.Size, pageUnit);
        }
        public static DCSystem_Drawing.SizeF MeasureString(
            Graphics g,
            string text,
            RuntimeDocumentContentStyle style,
            float layoutWidth,
            StringFormat format)
        {
            if (style == null)
            {
                throw new ArgumentNullException("style");
            }
            //if (Options_MeasureUseTrueTypeFont)
            //{
            return MeasureStringUseTrueTypeFont(g.PageUnit, text, style.FontName, style.FontSize, style.FontStyle, layoutWidth, format);
            //}
            //else
            //{
            //    return g.MeasureString(text, style.FontValue, (int)layoutWidth, format?.Value());
            //}
        }
        private static string _GlobalDefaultFontName = XFontValue.DefaultFontName;
        //public static void SetGlobalDefaultFont(string fontName)
        //{
        //    _GlobalDefaultFontName = fontName;
        //}
        private static string FixFontName(string fn)
        {
            return DCSoft.TrueTypeFontSnapshort.FixFontName(fn);

            //if (fn == null || fn.Length == 0)
            //{
            //    return _GlobalDefaultFontName;
            //}
            //return fn;
        }
        /// <summary>
        /// 计算单个字符的大小
        /// </summary>
        /// <param name="unit"></param>
        /// <param name="c"></param>
        /// <param name="font"></param>
        /// <returns></returns>
        public static DCSystem_Drawing.SizeF MeasureCharUseTrueTypeFont(GraphicsUnit unit, char c, Font font)
        {
            var info = GetFontInfo(font);
            if (info != null)
            {
                var w = info.GetCharWidth(c, font.Size, unit);
                if (w == 0 && info.NotSupportCharForLastGetCharWidth)
                {
                    var strDfn = FontFileManagerBase.Instance.GetDefaultFontName(c);
                    if (string.IsNullOrEmpty(strDfn))
                    {
                        strDfn = XFontValue.DefaultFontName;
                    }
                    var dinfo = TrueTypeFontSnapshort.GetInstance(strDfn, info.Style);
                    if (dinfo == null)
                    {
                        dinfo = TrueTypeFontSnapshort.GetInstance(_GlobalDefaultFontName, info.Style);
                    }
                    if (dinfo != null)
                    {
                        w = dinfo.GetCharWidth(c, font.Size, unit);
                    }
                }
                return new DCSystem_Drawing.SizeF(w, info.GetFontHeight(font.Size, unit));
            }
            return DCSystem_Drawing.SizeF.Empty;
        }

        public static DCSystem_Drawing.SizeF MeasureStringUseTrueTypeFont(
            GraphicsUnit graphicsPageUnit,
            string text,
             Font font,
            float layoutWidth,
            StringFormat format)
        {
            bool allowMultiline = format == null || (format.FormatFlags & StringFormatFlags.NoWrap) != StringFormatFlags.NoWrap;
            var info = TrueTypeFontSnapshort.GetInstance(FixFontName(font.Name), font.Style);
            if (info == null)
            {
                info = TrueTypeFontSnapshort.GetInstance(_GlobalDefaultFontName, font.Style);
            }
            return MeasureStringUseTrueTypeFont(graphicsPageUnit, text, info, font.Size, font.Style, layoutWidth, allowMultiline);

        }
        public static DCSoft.TrueTypeFontSnapshort GetFontInfo(string fontName, FontStyle vStyle)
        {
            var info = DCSoft.TrueTypeFontSnapshort.GetInstance(FixFontName(fontName), vStyle);
            if (info == null)
            {
                info = DCSoft.TrueTypeFontSnapshort.GetInstance(_GlobalDefaultFontName, vStyle);
            }
            return info;
        }

        public static TrueTypeFontSnapshort GetFontInfo(Font f)
        {
            var info = TrueTypeFontSnapshort.GetInstance(FixFontName(f.Name), f.Style);
            if (info == null)
            {
                info = TrueTypeFontSnapshort.GetInstance(_GlobalDefaultFontName, f.Style);
            }
            return info;
        }
        //public static SizeF MeasureSingleLineStringUseTrueTypeFont(
        //   GraphicsUnit graphicsPageUnit,
        //   string text,
        //    Font font)
        //{
        //    var info = TrueTypeFontInfo.GetInstance(FixFontName(font.Name), font.Style);
        //    if (info == null)
        //    {
        //        info = TrueTypeFontInfo.GetInstance(_GlobalDefaultFontName, font.Style);
        //    }
        //    return MeasureSingleLineStringUseTrueTypeFont(graphicsPageUnit, text, info, font.Size); 
        //}

        public static DCSystem_Drawing.SizeF MeasureStringUseTrueTypeFont(
            GraphicsUnit graphicsPageUnit,
            string text,
            string fontName,
            float fontSize,
            FontStyle vFontStyle,
            float layoutWidth,
            StringFormat format)
        {
            fontName = FixFontName(fontName);
            bool allowMultiline = format == null || (format.FormatFlags & StringFormatFlags.NoWrap) != StringFormatFlags.NoWrap;
            var info = TrueTypeFontSnapshort.GetInstance(fontName, vFontStyle);
            if (info == null)
            {
                info = TrueTypeFontSnapshort.GetInstance(_GlobalDefaultFontName, vFontStyle);
            }
            if (info == null)
            {
                return DCSystem_Drawing.SizeF.Empty;
            }
            return MeasureStringUseTrueTypeFont(graphicsPageUnit, text, info, fontSize, vFontStyle, layoutWidth, allowMultiline);
        }
        /// <summary>
        /// 测量单行文本的大小
        /// </summary>
        /// <param name="graphicsPageUnit"></param>
        /// <param name="text"></param>
        /// <param name="info"></param>
        /// <param name="fontSize"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static DCSystem_Drawing.SizeF MeasureSingleLineStringUseTrueTypeFont(
            GraphicsUnit graphicsPageUnit,
            string text,
             Font font)
        {
            return MeasureSingleLineStringUseTrueTypeFont(graphicsPageUnit, text, GetFontInfo(font), font);
        }
        /// <summary>
        /// 测量单行文本的大小
        /// </summary>
        /// <param name="graphicsPageUnit"></param>
        /// <param name="text"></param>
        /// <param name="info"></param>
        /// <param name="fontSize"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static SizeF MeasureSingleLineStringUseTrueTypeFont(
            GraphicsUnit graphicsPageUnit,
            string text,
            TrueTypeFontSnapshort info,
            float fontSize,
            FontStyle vStyle)
        {
            if (info == null)
            {
                throw new ArgumentNullException("info");
            }
            var bolFixForBold = info.NeedFixWidthForBold(vStyle);

            //bool allowMultiline = format == null || (format.FormatFlags & StringFormatFlags.NoWrap) != StringFormatFlags.NoWrap;
            float lineHeight = info.GetFontHeight(fontSize, graphicsPageUnit);
            float totalWidth = 0;
            if (text != null && text.Length > 0)
            {
                int len = text.Length;
                for (int iCount = 0; iCount < len; iCount++)
                {
                    var cv = text[iCount];
                    float w = 0;
                    w = info.GetCharWidth(cv, fontSize, graphicsPageUnit);
                    if (w > 0 && bolFixForBold)
                    {
                        if (cv < 127)
                        {
                            w *= BoldFontWidthFixRate;
                        }
                    }
                    else if (w == 0 && info.NotSupportCharForLastGetCharWidth)
                    {
                        var strDfn = FontFileManagerBase.Instance.GetDefaultFontName(cv);
                        if (string.IsNullOrEmpty(strDfn))
                        {
                            strDfn = XFontValue.DefaultFontName;
                        }
                        var dinfo = TrueTypeFontSnapshort.GetInstance(strDfn, info.Style);
                        if (dinfo == null)
                        {
                            dinfo = TrueTypeFontSnapshort.GetInstance(_GlobalDefaultFontName, info.Style);
                        }
                        if (dinfo != null)
                        {
                            w = dinfo.GetCharWidth(cv, fontSize, graphicsPageUnit);
                            if (w > 0 && dinfo.NeedFixWidthForBold(vStyle) && cv < 127)
                            {
                                w *= BoldFontWidthFixRate;
                            }
                        }
                        //if (defaultInfo == null)
                        //{
                        //    defaultInfo = TrueTypeFontInfo.GetInstance(_DefaultFontName, vFontStyle);
                        //    if (defaultInfo == null)
                        //    {
                        //        defaultInfo = TrueTypeFontInfo.Empty;
                        //    }
                        //}
                        //w = defaultInfo.GetCharWidth(text[iCount], fontSize, graphicsPageUnit);
                    }
                    totalWidth += w;
                }
            }
            return new SizeF(totalWidth, lineHeight);
        }

        /// <summary>
        /// 测量单行文本的大小
        /// </summary>
        /// <param name="graphicsPageUnit"></param>
        /// <param name="text"></param>
        /// <param name="info"></param>
        /// <param name="fontSize"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static DCSystem_Drawing.SizeF MeasureSingleLineStringUseTrueTypeFont(
            GraphicsUnit graphicsPageUnit,
            string text,
            TrueTypeFontSnapshort info,
             Font font)
        {
            if (info == null)
            {
                throw new ArgumentNullException("info");
            }
            var fontSize = font.Size;
            var bolFixForBold = info.NeedFixWidthForBold(font.Style);

            //bool allowMultiline = format == null || (format.FormatFlags & StringFormatFlags.NoWrap) != StringFormatFlags.NoWrap;
            float lineHeight = info.GetFontHeight(fontSize, graphicsPageUnit);
            float totalWidth = 0;
            if (text != null && text.Length > 0)
            {
                int len = text.Length;
                for (int iCount = 0; iCount < len; iCount++)
                {
                    var cv = text[iCount];
                    float w;
                    w = info.GetCharWidth(cv, fontSize, graphicsPageUnit);
                    if (w > 0 && bolFixForBold)
                    {
                        if (cv < 127)
                        {
                            w *= BoldFontWidthFixRate;
                        }
                    }
                    else if (w == 0 && info.NotSupportCharForLastGetCharWidth)
                    {
                        var strDfn = FontFileManagerBase.Instance.GetDefaultFontName(cv);
                        if (string.IsNullOrEmpty(strDfn))
                        {
                            strDfn = XFontValue.DefaultFontName;
                        }
                        var dinfo = TrueTypeFontSnapshort.GetInstance(strDfn, info.Style);
                        if (dinfo == null)
                        {
                            dinfo = TrueTypeFontSnapshort.GetInstance(_GlobalDefaultFontName, info.Style);
                        }
                        if (dinfo != null)
                        {
                            w = dinfo.GetCharWidth(cv, fontSize, graphicsPageUnit);
                            if (w > 0 && dinfo.NeedFixWidthForBold(font.Style) && cv < 127)
                            {
                                w *= BoldFontWidthFixRate;
                            }
                        }
                    }
                    totalWidth += w;
                }
            }
            return new SizeF(totalWidth, lineHeight);
        }

        private static readonly List<CharLayoutInfo> _Cached_LayoutInfos = new List<CharLayoutInfo>();
        public static SizeF MeasureStringUseTrueTypeFont(
            GraphicsUnit graphicsPageUnit,
            string text,
            TrueTypeFontSnapshort info,
            float fontSize,
            FontStyle vFontStyle,
            float layoutWidth,
            bool allowMultiline)
        {
            if (info == null)
            {
                throw new ArgumentNullException("info");
            }
            var bolFixForBold = info.NeedFixWidthForBold(vFontStyle);
            //bool allowMultiline = format == null || (format.FormatFlags & StringFormatFlags.NoWrap) != StringFormatFlags.NoWrap;
            float lineHeight = info.GetFontHeight(fontSize, graphicsPageUnit);
            float totalWidth = 0;
            //float totalHeight = lineHeight;
            int lineCount = 1;
            float maxWidth = 0;
            //TrueTypeFontInfo defaultInfo = null;
            if (text != null && text.Length > 0)
            {
                int len = text.Length;
                var line = _Cached_LayoutInfos;// new List<CharLayoutInfo>();
                line.Clear();
                for (int iCount = 0; iCount < len; iCount++)
                {
                    float w;
                    w = info.GetCharWidth(text[iCount], fontSize, graphicsPageUnit);
                    if (w > 0 && bolFixForBold)
                    {
                        if (text[iCount] < 127)
                        {
                            w *= BoldFontWidthFixRate;
                        }
                    }
                    else if (w == 0 && info.NotSupportCharForLastGetCharWidth)
                    {
                        var strDfn = FontFileManagerBase.Instance.GetDefaultFontName(text[iCount]);
                        if (string.IsNullOrEmpty(strDfn))
                        {
                            strDfn = XFontValue.DefaultFontName;
                        }
                        var dinfo = TrueTypeFontSnapshort.GetInstance(strDfn, info.Style);
                        if (dinfo == null)
                        {
                            dinfo = TrueTypeFontSnapshort.GetInstance(_GlobalDefaultFontName, info.Style);
                        }
                        if (dinfo != null)
                        {
                            w = dinfo.GetCharWidth(text[iCount], fontSize, graphicsPageUnit);
                            if (w > 0 && bolFixForBold && text[iCount] < 127)
                            {
                                w *= BoldFontWidthFixRate;
                            }
                        }
                        //if (defaultInfo == null)
                        //{
                        //    defaultInfo = TrueTypeFontInfo.GetInstance(_DefaultFontName, vFontStyle);
                        //    if (defaultInfo == null)
                        //    {
                        //        defaultInfo = TrueTypeFontInfo.Empty;
                        //    }
                        //}
                        //w = defaultInfo.GetCharWidth(text[iCount], fontSize, graphicsPageUnit);
                    }
                    var cinfo = new CharLayoutInfo(text[iCount], w);
                    if (totalWidth + w > layoutWidth && allowMultiline)
                    {
                        // 换行
                        if (IsEnglishLetterOrDigit(cinfo.Value))
                        {
                            int breakIndex = 0;
                            for (int cindex = line.Count - 1; cindex >= 0; cindex--)
                            {
                                if (IsEnglishLetterOrDigit(line[cindex].Value))
                                {
                                    breakIndex = cindex;
                                }
                                else
                                {
                                    break;
                                }
                            }
                            if (breakIndex > 0)
                            {
                                // 进行排版回滚
                                for (int cindex = line.Count - 1; cindex >= breakIndex; cindex--)
                                {
                                    totalWidth -= line[cindex].Width;

                                }
                                if (maxWidth < totalWidth)
                                {
                                    maxWidth = totalWidth;
                                }
                                iCount -= line.Count - breakIndex;
                                iCount--;
                                line.Clear();
                                totalWidth = 0;
                                lineCount++;
                                continue;
                            }
                        }
                        line.Clear();
                        if (maxWidth < totalWidth)
                        {
                            maxWidth = totalWidth;
                        }

                        totalWidth = w;
                        line.Add(cinfo);
                        lineCount++;

                    }
                    else
                    {
                        totalWidth += w;
                        line.Add(cinfo);
                    }
                }
                line.Clear();
            }
            if (lineCount == 1)
            {
                return new DCSystem_Drawing.SizeF(totalWidth, lineHeight);
            }
            else
            {
                return new DCSystem_Drawing.SizeF(maxWidth, lineHeight * lineCount);
            }
        }

        private static bool IsEnglishLetterOrDigit(char c)
        {
            if (c >= 'a' && c <= 'z')
            {
                return true;
            }
            if (c >= 'A' && c <= 'Z')
            {
                return true;
            }
            return c >= '0' && c <= '9';
        }

        private class CharLayoutInfo
        {
            public CharLayoutInfo(char c, float w)
            {
                this.Value = c;
                this.Width = w;
            }
            public readonly char Value;
            public readonly float Width;
#if !(RELEASE || LightWeight)
            public override string ToString()
            {
                return this.Value.ToString() + ' ' + this.Width;
            }
#endif

        }

        private readonly float _CharTopFix = 5;
        /// <summary>
        /// 字符顶端位置修正量
        /// </summary>
        public float CharTopFix
        {
            get { return _CharTopFix; }
        }

        private GraphicsUnit _GraphicsUnit = GraphicsUnit.Document;

        public GraphicsUnit GraphicsUnit
        {
            set
            {
                _GraphicsUnit = value;
            }
        }

        public static bool _EnableCacheFile = true;

        private static readonly StringFormat myMeasureFormat = null;


        /// <summary>
        /// 设置默认字体
        /// </summary>
        /// <param name="startChar">字符开始编号</param>
        /// <param name="endChar">字符结束编号</param>
        /// <param name="fontName">字体名称</param>
        public static void SetDefaultFont(char startChar, char endChar, string fontName)
        {
            var info = new DefaultFontInfo();
            info.StartChar = startChar;
            info.EndChar = endChar;
            info.FontName = fontName;
            _DefaultFonts.Add(info);
        }

        private class DefaultFontInfo
        {
            public char StartChar = char.MinValue;
            public char EndChar = char.MinValue;
            public bool Contains(char c)
            {
                return c >= this.StartChar && c <= this.EndChar;
            }
            public string FontName = null;
            //public readonly  List<string> NotSupportFontNames = new List<string>();
        }

        private static readonly List<DefaultFontInfo> _DefaultFonts = new List<DefaultFontInfo>();
        public static string GetDefaultFontName(char c)
        {
            foreach (var item in _DefaultFonts)
            {
                if (item.Contains(c))
                {
                    return item.FontName;
                }
            }
            return null;
        }
        internal static List<string> _ChangedToNewFontNames = null;

        internal static string ChangeToNewFontName = null;
        //internal static bool ChangeFontFlag = false;
        public DCSystem_Drawing.SizeF Measure(char c, Graphics g, GraphicsUnit pageUnit, RuntimeDocumentContentStyle rs, float fontSizeZoomRate = 1)
        {
            //long tick = DCSoft.Common.CountDown.GetTickCountExt();
            if (rs == null)
            {
                throw new ArgumentNullException("rs");
            }
            ChangeToNewFontName = null;
            float sizeWidth = 0;
            //float sizeHeight = 0;
            //SizeF size = SizeF.Empty;
            this._GraphicsUnit = pageUnit;
            XFontInfo fontInfo = rs.MyFontInfo;
            if (fontInfo == null || fontSizeZoomRate != 1)
            {
                fontInfo = new XFontInfo(
                    rs.FontName,
                    rs.FontSize * fontSizeZoomRate,
                    rs.FontStyle,
                    GraphicsUnit.Point);
            }

            var info = rs.StdSize10FontInfoForMeasure ;
            if (info == null)
            {
                info = StdSize10FontInfo.GetInstance(fontInfo, g);
                rs.StdSize10FontInfoForMeasure = info;
            }

            sizeWidth = info.GetCharWidth(c);
            if (sizeWidth < 0)
            {
                // 遇到不支持的字符，检查默认字体
                var strDefaultFontName = GetDefaultFontName(c);
                if (strDefaultFontName == info.FontName)
                {
                    if (_SpecifyCharWidths.TryGetValue(c, out sizeWidth) == false)
                    {
                        // 就是遇到默认字体也是测量不出来，特殊字符表也没用，则当做汉字来处理。
                        sizeWidth = info.ChineseWidth;
                        if (sizeWidth < 0)
                        {
                            // 汉字也不行，则使用英文
                            sizeWidth = info.GetCharWidth('H');
                        }
                    }
                }
                else
                {
                    if (strDefaultFontName != null)
                    {
                        fontInfo = new XFontInfo(strDefaultFontName, rs.FontSize, rs.FontStyle, GraphicsUnit.Point);
                        var info2 = StdSize10FontInfo.GetInstance(fontInfo, g);
                        if (info2 != null)
                        {
                            info = info2;
                            ChangeToNewFontName = info2.FontName;
                            if (_ChangedToNewFontNames == null)
                            {
                                _ChangedToNewFontNames = new List<string>();
                                _ChangedToNewFontNames.Add(info2.FontName);
                            }
                            else if (_ChangedToNewFontNames.Contains(info2.FontName) == false)
                            {
                                _ChangedToNewFontNames.Add(info2.FontName);
                            }
                            //ChangeFontFlag = true;
                        }
                    }
                    sizeWidth = info.GetCharWidth(c);
                    if (sizeWidth < 0 && _SpecifyCharWidths.TryGetValue(c, out sizeWidth) == false)
                    {
                        sizeWidth = info.GetCharWidth('H');
                    }
                    if (sizeWidth > 0 && info.NeedFixWidthForBold(rs.FontStyle))
                    {
                        sizeWidth *= BoldFontWidthFixRate;
                    }
                }
            }
            float resultZoomRate = rs.FontSize * fontSizeZoomRate / StdSize10FontInfo.StdSize10;

            sizeWidth = sizeWidth * resultZoomRate;
            float sizeHeight = info.FontHeight * resultZoomRate;// + this._CharTopFix;
            //tick = DCSoft.Common.CountDown.GetTickCountExt() - tick;
            return new SizeF(sizeWidth, sizeHeight);

        }

        internal class StdSize10FontInfo
        {
            public static void VoidMethod()
            {

            }

            internal const float StdSize10 = 10f;


            private static readonly Dictionary<StdSize10FontInfoKey, StdSize10FontInfo> _Instances
                = new Dictionary<StdSize10FontInfoKey, StdSize10FontInfo>();
            [Serializable]
            private class StdSize10FontInfoKey
            {
                public StdSize10FontInfoKey(
                    string fontName,
                    FontStyle vStyle,
                    GraphicsUnit unit,
                    GraphicsUnit pageUnit)
                {
                    this._FontName = fontName;
                    this._Style = vStyle;
                    this._Unit = unit;
                    this._PageUnit = pageUnit;
                    this._HashCode = fontName.GetHashCode() + 1000 * (int)this._Style + 100 * (int)this._Unit + (int)this._PageUnit;
                }
                public readonly string _FontName;
                public readonly FontStyle _Style;
                public readonly GraphicsUnit _Unit;
                public readonly GraphicsUnit _PageUnit;
                private readonly int _HashCode;
#if !(RELEASE || LightWeight)
                public override string ToString()
                {
                    return this._FontName + " " + this._Style;
                }
#endif
                public override int GetHashCode()
                {
                    return this._HashCode;
                }
                public override bool Equals(object obj)
                {
                    if (this == obj)
                    {
                        return true;
                    }
                    var info = (StdSize10FontInfoKey)obj;
                    return this._FontName == info._FontName
                        && this._Style == info._Style
                        && this._Unit == info._Unit
                        && this._PageUnit == info._PageUnit;
                }
            }
            internal static StdSize10FontInfo GetInstance(XFontInfo font, Graphics g)
            {
                StdSize10FontInfo result = null;
                var key = new StdSize10FontInfoKey(
                    font.Name,
                    font.Style,
                    font.Unit,
                    g.PageUnit);
                //lock (_Instances)
                {
                    if (_Instances.TryGetValue(key, out result) == false)
                    {
                        result = new StdSize10FontInfo(key, g);
                        _Instances[key] = result;
                    }
                }
                return result;
            }
            [NonSerialized]
            private Font _RuntimeFontValue = null;
            [NonSerialized]
            private TrueTypeFontSnapshort _TrueTypeFont = null;
            [NonSerialized]
            private GraphicsUnit _Graphics_PageUnit = GraphicsUnit.Document;
            private float InnerMeasureCharWidth(Graphics g, char c, StringFormat format)
            {
                //if (Options_MeasureUseTrueTypeFont)
                {
                    return _TrueTypeFont.GetCharWidth(c, StdSize10FontInfo.StdSize10, _Graphics_PageUnit) * this._RateForBoldInWebApplication;
                }
                //else
                //{
                //    return g.MeasureString(c.ToString(), this._RuntimeFontValue, 10000, format).Width * this._RateForBoldInWebApplication;
                //}
            }
            private readonly float _RateForBoldInWebApplication = 1f;
            /// <summary>
            /// 初始化对象
            /// </summary>
            /// <param name="key"></param>
            /// <param name="g"></param>
            private StdSize10FontInfo(StdSize10FontInfoKey key, Graphics g)
            {
                //System.Diagnostics.Debug.WriteLine("StdSize10FontInfo:" + key.ToString());
                this.FontName = XFontValue.FixFontName(key._FontName, false);


                this._FontStyle = key._Style;
                this._WidthStyle = FontWidthStyle.Proportional;
                StringFormat format = myMeasureFormat;
                this._TrueTypeFont = TrueTypeFontSnapshort.GetInstance(this.FontName, key._Style, key._Unit);
                if (this._TrueTypeFont == null)
                {
                    var msg = "系统不支持字体:" + this.FontName + " " + key._Style;
                    DCConsole.Default.WriteLine(msg);
                    throw new NotSupportedException(msg);
                }
                if ((this._FontStyle & FontStyle.Bold) == FontStyle.Bold)
                {
                    this.ExtZoomRate = this._TrueTypeFont.BoldZoomRate;
                }
                this._Graphics_PageUnit = g.PageUnit;
                var size1 = InnerMeasureCharWidth(g, 'W', format);// g.MeasureString("W", runtimeFontValue, 10000, format).Width;
                var size2 = InnerMeasureCharWidth(g, 'i', format);// g.MeasureString("i", runtimeFontValue, 10000, format).Width;
                if (size1 == size2)
                {
                    // 等宽字体
                    if (size1 == 0)
                    {
                        // 不支持的字符
                        this.SymbolWidth = -1;
                    }
                    else
                    {
                        this.SymbolWidth = size1;
                    }
                    this._WidthStyle = FontWidthStyle.Monospaced;
                }
                else
                {
                    // 不等宽字体
                    this._WidthStyle = FontWidthStyle.Proportional;
                    // 计算编码在127以下的字符宽度
                    this.StdCharWidths = new float[127];
                    for (int iCount = 0; iCount < this.StdCharWidths.Length; iCount++)
                    {
                        this.StdCharWidths[iCount] = InnerMeasureCharWidth(g, (char)iCount, format);
                    }
                }
                // 计算汉字的宽度
                this.ChineseWidth = InnerMeasureCharWidth(g, '袁', format);// g.MeasureString("袁", runtimeFontValue, 10000, format).Width;
                if (this.ChineseWidth == 0)
                {
                    this.ChineseWidth = -1;
                }
                // 计算空格的宽度
                this.WhitespaceWidth = InnerMeasureCharWidth(g, ' ', format);// g.MeasureString(" ", runtimeFontValue, 1000, format).Width;
                if (this.WhitespaceWidth < 0.1)
                {
                    this.WhitespaceWidth = InnerMeasureCharWidth(g, 'i', format);// g.MeasureString("i", runtimeFontValue, 1000, format).Width;
                }
                size1 = InnerMeasureCharWidth(g, ' ', format);// g.MeasureString(" ", runtimeFontValue, 10000, format).Width;
                this.WhitespaceWidth = size1;// *0.57f;
                this.FontHeight = this._TrueTypeFont.GetFontHeight(StdSize10FontInfo.StdSize10, g.PageUnit);
                this._CharWidthRanges = this._TrueTypeFont.GetCharWidthRanges();
                if (this._CharWidthRanges != null && this._CharWidthRanges.Length > 0)
                {
                    this.CharWidths = new float[this._CharWidthRanges.Length / 2];
                    for (int iCount = 0; iCount < this._CharWidthRanges.Length; iCount += 2)
                    {
                        char c = (char)this._CharWidthRanges[iCount];
                        this.CharWidths[iCount / 2] = InnerMeasureCharWidth(g, c, format);
                    }
                }
                if (this._RuntimeFontValue != null)
                {
                    this._RuntimeFontValue.Dispose();
                    this._RuntimeFontValue = null;
                }
                //this._TrueTypeFont = null;
                if (this.StdCharWidths == null && this._WidthStyle == FontWidthStyle.Proportional)
                {

                }
            }

            public static int IndexOfRanges(int[] ranges, int value)
            {
                if (ranges != null && ranges.Length > 0 && (ranges.Length % 2 == 0))
                {
                    int startIndex = 0;
                    int endIndex = ranges.Length >> 1;
                    while (endIndex > startIndex)
                    {
                        int middleIndex = (startIndex + endIndex) >> 1;
                        if (middleIndex == startIndex)
                        {
                            break;
                        }
                        int pos = middleIndex << 1;
                        if (value < ranges[pos])
                        {
                            endIndex = middleIndex;
                        }
                        else if (value <= ranges[pos + 1])
                        {
                            // 命中范围
                            return middleIndex;
                        }
                        else
                        {
                            startIndex = middleIndex;
                        }
                    }
                }
                return -1;
            }

            private readonly int[] _CharWidthRanges = null;
            private float GetCharWidthUseRanges(char c)
            {
                if (this._CharWidthRanges != null)
                {
                    int index = IndexOfRanges(this._CharWidthRanges, c);
                    if (index >= 0)
                    {
                        return this.CharWidths[index];
                    }
                }
                return -1;
            }
#if !(RELEASE || LightWeight)
            public override string ToString()
            {
                return FontName + " " + this._WidthStyle;
            }
#endif
            public void Dispose()
            {
            }
            /// <summary>
            /// 字体名称
            /// </summary>
            internal readonly string FontName = null;
            internal readonly FontStyle _FontStyle = FontStyle.Regular;
            internal readonly float ExtZoomRate = 1;
            private readonly Dictionary<char, float> _SpecifyCharWidth = new Dictionary<char, float>();

            public bool NeedFixWidthForBold(FontStyle style)
            {
                if (this._TrueTypeFont != null)
                {
                    return this._TrueTypeFont.NeedFixWidthForBold(style);
                }
                else
                {
                    return false;
                }
            }
            internal float GetCharWidth(char c)
            {
                if (IsChinese(c))
                {
                    return this.ChineseWidth;
                }
                if (c == ' ' || c == '\t')
                {
                    return this.WhitespaceWidth;
                }
                if (c >= 32 && c < 127)
                {
                    if (this._WidthStyle == FontWidthStyle.Monospaced)
                    {
                        // 等宽字体
                        return this.SymbolWidth * this.ExtZoomRate;
                    }
                    else
                    {
                        // 不等宽字体
                        return this.StdCharWidths[c] * this.ExtZoomRate;
                    }
                }
                if (this._CharWidthRanges != null)
                {
                    float w = GetCharWidthUseRanges(c);
                    if (w >= 0)
                    {
                        return w;
                    }
                }

                if (_SpecifyCharWidth.Count > 0)
                {
                    float w = 0;
                    if (_SpecifyCharWidth.TryGetValue(c, out w))
                    {
                        return w;
                    }
                }
                return -1;
            }

            private readonly float[] CharWidths = null;
            //public readonly int CharWidthsLength = 0;

            /// <summary>
            /// 英文字符宽度
            /// </summary>
            private readonly float SymbolWidth = 0f;
            /// <summary>
            /// 中文字符宽度
            /// </summary>
            internal readonly float ChineseWidth = 0f;
            /// <summary>
            /// 空格宽度
            /// </summary>
            private readonly float WhitespaceWidth = 0f;
            /// <summary>
            /// 字体高度
            /// </summary>
            internal readonly float FontHeight = 0f;

            private readonly float[] StdCharWidths = null;

            /// <summary>
            /// 字体宽度样式
            /// </summary>
            private readonly FontWidthStyle _WidthStyle = FontWidthStyle.Proportional;
        }
    }
}