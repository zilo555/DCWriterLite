using System;
using System.Collections.Generic;
using System.Text;

namespace DCSoft.Writer.Dom
{
    /// <summary>
    /// 排版帮助类型
    /// </summary>
    internal static class LayoutHelper
    {

        /// <summary>
        /// 判断元素是否是强制分行
        /// </summary>
        /// <param name="element">元素对象</param>
        /// <returns>元素是否是强制分行</returns>
        public static bool IsNewLine(DomElement element)
        {
            if (element is DomCharElement)
            {
                return false;
            }
            if (element is DomParagraphFlagElement)
            {
                return true;
            }
            if (element is DomLineBreakElement)
            {
                return true;
            }
            if (element is DomPageBreakElement)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 判断元素是否单独占据一行
        /// </summary>
        /// <param name="element">元素对象</param>
        /// <returns>元素是否单独占据一行</returns>
        public static bool OwnerHoleLine(DomElement element)
        {
            if (element is DomCharElement || element is DomParagraphFlagElement)
            {
                return false;
            }
            if (element is DomTableElement)
            {
                return true;
            }
            if (element is DomPageBreakElement)
            {
                return true;
            }
            return false;
        }

        private static readonly bool[] _IsEnglishLetterOrDigit = Create_IsEnglishLetterOrDigit();
        private static bool[] Create_IsEnglishLetterOrDigit()
        {
            var result = new bool[127];
            for (int iCount = 0; iCount < result.Length; iCount++)
            {
                result[iCount] = false;
                if (iCount >= 'a' && iCount <= 'z')
                {
                    result[iCount] = true;
                }
                else if (iCount >= 'A' && iCount <= 'Z')
                {
                    result[iCount] = true;
                }
                else if (iCount >= '0' && iCount <= '9')
                {
                    result[iCount] = true;
                }
                else if (iCount == '.')
                {
                    result[iCount] = true;
                }
            }
            return result;
        }
        /// <summary>
        /// 判断指定的字符是否是应为字母或者数字
        /// </summary>
        /// <param name="c">字符值</param>
        /// <returns>判断结果</returns>
        public static bool IsEnglishLetterOrDigit(char c)
        {
            if (c < 127)
            {
                return _IsEnglishLetterOrDigit[c];
            }
            if( 19968 <= c && c <=40869)
            {
                // 汉字字符
                return false;
            }
            return false;
        }



        /// <summary>
        /// 判断元素是否可以出现在行首
        /// </summary>
        /// <param name="element">元素对象</param>
        /// <returns>元素是否可以出现在行首</returns>
        public static bool CanBeLineHead(DomElement element)
        {
            if (element is DomCharElement)
            {
                return CanBeLineHead(((DomCharElement)element).GetCharValue());
            }
            else if (element is DomFieldBorderElement)
            {
                DomFieldBorderElement border = (DomFieldBorderElement)element;
                DomFieldElementBase field = (DomFieldElementBase)border.OwnerField;
                if (field.StartElement == border)
                {
                    // 字段的起始边界元素可以放在行首。
                    return true;
                }
                return false;
            }
            else if (element is DomLineBreakElement)
            {
                // 换行符不能在行首。
                return false;
            }
            else
            {
                return true;
            }
        }
         
        /// <summary>
        /// 判断元素是否可以出现在行尾
        /// </summary>
        /// <param name="element">元素对象</param>
        /// <returns>元素是否可以出现在行尾</returns>
        public static bool CanBeLineEnd(DomElement element)
        {
            if (element is DomCharElement)
            {
                return CanBeLineEnd(((DomCharElement)element).GetCharValue());
            }
            else if (element is DomFieldBorderElement)
            {
                DomFieldBorderElement border = (DomFieldBorderElement)element;
                DomFieldElementBase field = (DomFieldElementBase)border.OwnerField;
                if (field.EndElement == border)
                {
                    // 字段的末端边界元素可以放在行尾。
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// 判断指定字符能否放在行首
        /// </summary>
        /// <param name="c">字符值</param>
        /// <returns>判断结果</returns>
        public static bool CanBeLineHead(char c)
        {
            if (_TailSymbols == null)
            {
                return true;
            }
            return _TailSymbols.IndexOf(c) < 0;
        }

        /// <summary>
        /// 判断指定字符能否放置在行尾
        /// </summary>
        /// <param name="c">字符值</param>
        /// <returns>判断结果</returns>
        public static bool CanBeLineEnd(char c)
        {
            if (_HeadSymbols == null)
            {
                return true;
            }
            return _HeadSymbols.IndexOf(c) < 0;
        }

        private static string _TailSymbols = "!),.:;?]}¨·ˇˉ―‖’”…∶、。〃々〉》」』】〕〗！＂＇），．：；？］｀｜｝～￠";

        private static string _HeadSymbols = "([{·‘“〈《「『【〔〖（．［｛￡￥";
    }
}
