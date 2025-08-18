using System;
using System.Collections.Generic;
using System.Text;
using DCSoft.Common;

namespace DCSoft.Drawing
{
    public static class ParagraphListHelper
    {
        static ParagraphListHelper()
        {
            var list = new Dictionary<ParagraphListStyle, string>();
            list[ParagraphListStyle.ListNumberStyle] = "\0.";
            list[ParagraphListStyle.ListNumberStyleArabic1] = "\0,";
            list[ParagraphListStyle.ListNumberStyleArabic2] = "\0)";
            list[ParagraphListStyle.ListNumberStyleArabic3] = "\0、";
            list[ParagraphListStyle.ListNumberStyleLowercaseLetter] = "\0)";
            list[ParagraphListStyle.ListNumberStyleLowercaseRoman] = "\0)";
            list[ParagraphListStyle.ListNumberStyleNumberInCircle] = "\0";
            list[ParagraphListStyle.ListNumberStyleUppercaseLetter] = "\0)";
            list[ParagraphListStyle.ListNumberStyleUppercaseRoman] = "\0)";
            list[ParagraphListStyle.ListNumberStyleZodiac1] = "\0,";
            list[ParagraphListStyle.ListNumberStyleZodiac2] = "\0,";
            list[ParagraphListStyle.None] = null;
            list[ParagraphListStyle.BulletedList] = new string((char)0XF06C, 1);
            list[ParagraphListStyle.BulletedListBlock] = new string((char)0XF06E, 1);
            list[ParagraphListStyle.BulletedListCheck] = new string((char)0XF0FC, 1);
            list[ParagraphListStyle.BulletedListDiamond] = new string((char)0XF075, 1);
            list[ParagraphListStyle.BulletedListRightArrow] = new string((char)0XF0D8, 1);
            list[ParagraphListStyle.BulletedListHollowStar] = new string((char)0XF0B2, 1);
            _LevelText = list;
        }

        public static readonly Dictionary<ParagraphListStyle , string > _LevelText  ;
        
        public static string GetLevelText(ParagraphListStyle style)
        {
            if( style == ParagraphListStyle.None )
            {
                return null;
            }
            string v = null;
            if( _LevelText.TryGetValue( style , out v ))
            {
                return v;
            }
            return null;

            //if (_LevelText.ContainsKey(style))
            //{
            //    return _LevelText[style];
            //}
            //else
            //{
            //    return null ;
            //}
        }
        private static readonly string _Default_BulletedListString = new string((char)0xF06C, 1);
        public static string GetBulletedListString(ParagraphListStyle style)
        {
            if(style == ParagraphListStyle.None )
            {
                return _Default_BulletedListString;
            }
            string v = null;
            if( _LevelText.TryGetValue( style , out v ))
            {
                return v;
            }
            return _Default_BulletedListString;
        }
        public static ParagraphListStyle GetBulletedListStyle(char chrValue)
        {
            foreach (ParagraphListStyle ps in _LevelText.Keys)
            {
                string v = _LevelText[ps];
                if (string.IsNullOrEmpty(v) == false )
                {
                    if ( ( v[0] & 0xff ) == ( chrValue & 0xff ))
                    {
                        return ps;
                    }
                }
            }
            return ParagraphListStyle.BulletedList;
        }
        public static string GetSimpleGetParagraphListText(int number, ParagraphListStyle style)
        {
            string strValue = number.ToString();
            switch (style)
            {
                case ParagraphListStyle.ListNumberStyle:
                    strValue = number.ToString();
                    break;
                case ParagraphListStyle.ListNumberStyleArabic1:
                    strValue = number.ToString();
                    break;
                case ParagraphListStyle.ListNumberStyleArabic2:
                    strValue = number.ToString();
                    break;
                case ParagraphListStyle.ListNumberStyleLowercaseLetter:
                    strValue = StringConvertHelper.GetLetterNumber(number - 1, "abcdefghijklmnopqrstuvwxyz");
                    break;
                case ParagraphListStyle.ListNumberStyleLowercaseRoman:
                    strValue = StringConvertHelper.GetRomanNumber(number).ToLower();
                    break;
                case ParagraphListStyle.ListNumberStyleNumberInCircle:
                    {
                        string txt = "_①②③④⑤⑥⑦⑧⑨⑩";
                        strValue = txt[number % txt.Length].ToString();
                        break;
                    }
                case ParagraphListStyle.ListNumberStyleUppercaseLetter:
                    strValue = StringConvertHelper.GetLetterNumber(number - 1, "ABCDEFGHIJKLMNOPQRSTUVWXYZ");
                    break;
                case ParagraphListStyle.ListNumberStyleUppercaseRoman:
                    strValue = StringConvertHelper.GetRomanNumber(number).ToUpper();
                    break;
                case ParagraphListStyle.ListNumberStyleZodiac1:
                    {
                        string txt = "_甲乙丙丁戊己庚辛壬癸";
                        strValue = txt[number % txt.Length].ToString();
                        break;
                    }
                case ParagraphListStyle.ListNumberStyleZodiac2:
                    {
                        string txt = "_子丑寅卯辰巳午未申酉戌亥";
                        strValue = txt[number % txt.Length].ToString();
                        break;
                    }
            }
            return strValue;
        }

        public static string GetParagraphListText(int number, ParagraphListStyle style )
        {
            string currentLevelText = "\0.";
            if (_LevelText.ContainsKey(style))
            {
                currentLevelText = _LevelText[style];
            }
            if (string.IsNullOrEmpty(currentLevelText))
            {
                return null;
            }
            string strValue = GetSimpleGetParagraphListText(number, style); 
            string result = currentLevelText.Replace("\0", strValue);
            return result;
        }

        /// <summary>
        /// 判断是否是圆点列表方式
        /// </summary>
        public static bool IsBulletedList(ParagraphListStyle ps)
        {

            return ps == ParagraphListStyle.BulletedList
                || ps == ParagraphListStyle.BulletedListBlock
                || ps == ParagraphListStyle.BulletedListCheck
                || ps == ParagraphListStyle.BulletedListDiamond
                || ps == ParagraphListStyle.BulletedListHollowStar
                || ps == ParagraphListStyle.BulletedListRightArrow;

        }

        /// <summary>
        /// 判断是否是数字列表方式
        /// </summary>
        public static bool IsListNumberStyle(ParagraphListStyle ps)
        {
            return ps == ParagraphListStyle.ListNumberStyle
                || ps == ParagraphListStyle.ListNumberStyleArabic1
                || ps == ParagraphListStyle.ListNumberStyleArabic2
                || ps == ParagraphListStyle.ListNumberStyleLowercaseLetter
                || ps == ParagraphListStyle.ListNumberStyleLowercaseRoman
                || ps == ParagraphListStyle.ListNumberStyleNumberInCircle
                || ps == ParagraphListStyle.ListNumberStyleUppercaseLetter
                || ps == ParagraphListStyle.ListNumberStyleUppercaseRoman
                || ps == ParagraphListStyle.ListNumberStyleZodiac1
                || ps == ParagraphListStyle.ListNumberStyleZodiac2;
        }

    }
}
