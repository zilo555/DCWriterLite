using DCSoft.Drawing;
using DCSoft.Writer;
using System.Collections.Generic;
using System.Text.Json;

namespace DCSoft.WASM
{
    /// <summary>
    /// TrueType字体文件相关代码
    /// </summary>
    [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = false)]
    public class TTFontFileHelper
    {
        /// <summary>
        /// 字体信息快照缓存区
        /// </summary>
        private static readonly Dictionary<string, DCSoft.TrueTypeFontSnapshort> _FontSnapshorts
            = new Dictionary<string, TrueTypeFontSnapshort>();
        /// <summary>
        /// 错误的字体名称列表
        /// </summary>
        private static List<string> _BadFontNames = null;
        /// <summary> 
        /// 启动字体处理模块
        /// </summary>
        public static void Start()
        {
            // 获得字体快照
            DCSoft.TrueTypeFontSnapshort.EventGetInstance = delegate (string strFontName, FontStyle style)
            {
                if (strFontName == null || strFontName.Length == 0)
                {
                    strFontName = XFontValue.DefaultFontName;
                }
                var bolBold = (style & FontStyle.Bold) == FontStyle.Bold;
                var bolItalic = (style & FontStyle.Italic) == FontStyle.Italic;
                if (_BadFontNames != null && _BadFontNames.Contains(strFontName))
                {
                    return null;
                }
                var strKey = JavaScriptMethods.GetFontInfoKeyName(strFontName, bolBold, bolItalic);
                DCSoft.TrueTypeFontSnapshort info = null;
                if (_FontSnapshorts.TryGetValue(strKey, out info))
                {
                    return info;
                }
                byte[] bsData = null;
                var strData = JavaScriptMethods.GetFontSnapshort(strKey);
                if (strData != null && strData.Length > 0)
                {
                    float vBoldZoomRate = 1;
                    var index99 = strData.LastIndexOf(',');
                    if (index99 > 0)
                    {
                        vBoldZoomRate = float.Parse(strData.Substring(index99 + 1));
                        strData = strData.Substring(0, index99);
                    }
                    bsData = Convert.FromBase64String(strData);
                    info = TrueTypeFontSnapshort.Create(strFontName, bsData);
                    info.BoldZoomRate = vBoldZoomRate;
                }
                else
                {
                    if (_BadFontNames == null)
                    {
                        _BadFontNames = new List<string>();
                    }
                    _BadFontNames.Add(strFontName);
                }
                _FontSnapshorts[strKey] = info;
                //Console.WriteLine(info.FontName + " lll :" + info.LineSpacing);
                if (info != null && info.FontName == "华文宋体")
                {
                    //针对问题DUWRITER5_0-4067，宋体LineSpacing=1.140625,华文宋体LineSpacing=1.301
                    info.LineSpacing = 1.140625f;
                }
                return info;
            };
        }
        

        public static MyFontInfo2 GetRuntimeFont(string strFontName, bool bolBold, bool bolItalic)
        {
            if (strFontName == null || strFontName.Length == 0)
            {
                strFontName = XFontValue.DefaultFontName;
            }
            var strKey = JavaScriptMethods.GetFontInfoKeyName(strFontName, bolBold, bolItalic);
            var strFileName = JavaScriptMethods.GetFontFileName(strKey);
            return new MyFontInfo2(strKey, strFontName, bolBold, bolItalic, strFileName);
        }

        
        public class MyFontInfo2
        {
            internal MyFontInfo2(string strKey, string strFontName, bool bolBold, bool bolItalic, string strFileName )
            {
                this.Key = strKey;
                this.FontName = strFontName;
                this.Bold = bolBold;
                this.Italic = bolItalic;
                this.FontFileName = strFileName;
                if (this.FontFileName == null)
                {
                    this.FontFileName = string.Empty;
                }
            }
            public readonly string Key;
            public readonly string FontName;
            public readonly bool Bold;
            public readonly bool Italic;
            public readonly string FontFileName ;
            public readonly bool Cached;
        }
    }
}
