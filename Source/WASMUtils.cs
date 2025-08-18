using DCSoft.Writer.Dom;
using DCSoft.Writer;
using System.Collections.Generic;
using System.Text.Json;
using DCSoft.Drawing;
using DCSoft.Printing;
using DCSoft.Writer.Data;
using System.Collections;
using DCSoft.Writer.Controls;
using DCSoft.Common;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using DCSystemXml;
using System.Globalization;
using DCSoft.Data;

namespace DCSoft.WASM
{
    internal static partial class WASMUtils
    {
        /// <summary>
        /// 比较两个字符串是否相等，空引用和空字符串都视为相等
        /// </summary>
        /// <param name="s1"></param>
        /// <param name="s2"></param>
        /// <returns>是否相等</returns>
        public static bool EqualsUIString(string s1, string s2)
        {
            if (s1 == s2)
            {
                return true;
            }
            // 如果两个字符串都为 null 或空字符串，则视为相等
            if (string.IsNullOrEmpty(s1) && string.IsNullOrEmpty(s2))
            {
                return true;
            }
            // 使用字符串的 Equals 方法进行比较
            return string.Equals(s1, s2, StringComparison.Ordinal);
        }
         
        private static System.Globalization.DateTimeFormatInfo _dtmFormat = null;

        private static void processDateTimeFormatStringUpperLower(System.Globalization.DateTimeFormatInfo _dtmFormat, DCBooleanValue value)
        {
            if (value == DCBooleanValue.Inherit || _dtmFormat == null)
            {
                return;
            }
            List<string> listAbbreviatedDayNames = new List<string>();
            List<string> listAbbreviatedMonthGenitiveNames = new List<string>();
            List<string> listAbbreviatedMonthNames = new List<string>();
            List<string> listDayNames = new List<string>();
            List<string> listMonthGenitiveNames = new List<string>();
            List<string> listMonthNames = new List<string>();
            List<string> listShortestDayNames = new List<string>();
            for (int i = 0; i < _dtmFormat.AbbreviatedDayNames.Length; i++)
            {
                if (value == DCBooleanValue.True)
                {
                    listAbbreviatedDayNames.Add(_dtmFormat.AbbreviatedDayNames[i].ToUpper());
                }
                else
                {
                    listAbbreviatedDayNames.Add(_dtmFormat.AbbreviatedDayNames[i].ToLower());
                }
            }
            for (int i = 0; i < _dtmFormat.AbbreviatedMonthGenitiveNames.Length; i++)
            {
                if (value == DCBooleanValue.True)
                {
                    listAbbreviatedMonthGenitiveNames.Add(_dtmFormat.AbbreviatedMonthGenitiveNames[i].ToUpper());
                }
                else
                {
                    listAbbreviatedMonthGenitiveNames.Add(_dtmFormat.AbbreviatedMonthGenitiveNames[i].ToLower());
                }
            }
            for (int i = 0; i < _dtmFormat.AbbreviatedMonthNames.Length; i++)
            {
                if (value == DCBooleanValue.True)
                {
                    listAbbreviatedMonthNames.Add(_dtmFormat.AbbreviatedMonthNames[i].ToUpper());
                }
                else
                {
                    listAbbreviatedMonthNames.Add(_dtmFormat.AbbreviatedMonthNames[i].ToLower());
                }
            }
            for (int i = 0; i < _dtmFormat.DayNames.Length; i++)
            {
                if (value == DCBooleanValue.True)
                {
                    listDayNames.Add(_dtmFormat.DayNames[i].ToUpper());
                }
                else
                {
                    listDayNames.Add(_dtmFormat.DayNames[i].ToLower());
                }
            }
            for (int i = 0; i < _dtmFormat.MonthGenitiveNames.Length; i++)
            {
                if (value == DCBooleanValue.True)
                {
                    listMonthGenitiveNames.Add(_dtmFormat.MonthGenitiveNames[i].ToUpper());
                }
                else
                {
                    listMonthGenitiveNames.Add(_dtmFormat.MonthGenitiveNames[i].ToLower());
                }
            }
            for (int i = 0; i < _dtmFormat.MonthNames.Length; i++)
            {
                if (value == DCBooleanValue.True)
                {
                    listMonthNames.Add(_dtmFormat.MonthNames[i].ToUpper());
                }
                else
                {
                    listMonthNames.Add(_dtmFormat.MonthNames[i].ToLower());
                }
            }
            for (int i = 0; i < _dtmFormat.ShortestDayNames.Length; i++)
            {
                if (value == DCBooleanValue.True)
                {
                    listShortestDayNames.Add(_dtmFormat.ShortestDayNames[i].ToUpper());
                }
                else
                {
                    listShortestDayNames.Add(_dtmFormat.ShortestDayNames[i].ToLower());
                }
            }
            _dtmFormat.AbbreviatedDayNames = listAbbreviatedDayNames.ToArray(); ;
            _dtmFormat.AbbreviatedMonthGenitiveNames = listAbbreviatedMonthGenitiveNames.ToArray(); ;
            _dtmFormat.AbbreviatedMonthNames = listAbbreviatedMonthNames.ToArray(); ;
            _dtmFormat.DayNames = listDayNames.ToArray(); ;
            _dtmFormat.MonthGenitiveNames = listMonthGenitiveNames.ToArray(); ;
            _dtmFormat.MonthNames = listMonthNames.ToArray(); ;
            _dtmFormat.ShortestDayNames = listShortestDayNames.ToArray();
        }


        /// <summary>
        /// 执行数据格式化输出为文本的处理
        /// </summary>
        /// <param name="style">样式</param>
        /// <param name="format">格式</param>
        /// <param name="noneText">为空数据时的文本</param>
        /// <param name="Value">原始数据</param>
        /// <returns>输出的文本</returns>
        public static string ExecuteValueFormat(
            ValueFormatStyle style,
            string format,
            string noneText,
            object Value)
        {
            static bool HasContent(string txt)
            {
                return txt != null && txt.Trim().Length > 0;
            }
            static bool IsNumberTypeFast(object value)
            {
                return (value is byte
                    || value is sbyte
                    || value is short
                    || value is ushort
                    || value is int
                    || value is uint
                    || value is long
                    || value is ulong
                    || value is float
                    || value is double
                    || value is decimal);
            }

            if (Value == null || DBNull.Value.Equals(Value))
            {
                // 数据为空
                return noneText;
            }
            if (format == null)
            {
                format = string.Empty;
            }
            switch (style)
            {
                case ValueFormatStyle.None:
                    {
                        // 无任何格式转换
                        return Convert.ToString(Value);
                    }
                case ValueFormatStyle.Currency:
                    {
                        // 货币类型
                        decimal dec = 0;
                        bool convertFlag = false;
                        if (Value is string)
                        {
                            string txt = (string)Value;
                            if (HasContent(txt))
                            {
                                try
                                {
                                    convertFlag = decimal.TryParse(txt, out dec);
                                }
                                catch
                                {
                                }
                            }
                        }
                        else if (IsNumberTypeFast(Value) || Value is bool)
                        {
                            dec = Convert.ToDecimal(Value);
                            convertFlag = true;
                        }
                        if (convertFlag)
                        {
                            if (HasContent(format))
                            {
                                return dec.ToString(format);
                            }
                            else
                            {
                                return dec.ToString();
                            }
                        }
                        else
                        {
                            return noneText;
                        }
                        //break;
                    }
                case ValueFormatStyle.DateTime:
                    {
                        DateTime dtm = DateTime.MinValue;
                        bool convertFlag = false;
                        if (Value is string)
                        {
                            string txt = (string)Value;
                            if (txt != null && txt.Length > 0)
                            {
                                convertFlag = DateTime.TryParse(txt, out dtm);
                                if (convertFlag == false)
                                {
                                    convertFlag = DateTime.TryParseExact(
                                        txt,
                                        format,
                                        null,
                                        System.Globalization.DateTimeStyles.AllowWhiteSpaces,
                                        out dtm);
                                    if (convertFlag == false)
                                    {
                                        // 开始解析纯数字式的时间日期字符串
                                        txt = txt.Trim();
                                        if (StringConvertHelper.IsIntegerString(txt))
                                        {
                                            if (format.Contains("y"))
                                            {
                                                dtm = StringConvertHelper.ToDBDateTime(txt, DateTime.MinValue);
                                            }
                                            else
                                            {
                                                dtm = StringConvertHelper.ToDBTime(txt, DateTime.MinValue);
                                            }
                                            convertFlag = true;
                                        }
                                    }
                                }
                            }
                        }
                        else if (Value is DateTime)
                        {
                            dtm = (DateTime)Value;
                            convertFlag = true;
                        }
                        else if (IsNumberTypeFast(Value))
                        {
                            dtm = new DateTime(Convert.ToInt64(Value));
                            convertFlag = true;
                        }
                        else
                        {
                            dtm = Convert.ToDateTime(Value);
                            convertFlag = true;
                        }
                        if (convertFlag)
                        {
                            if (format != null && format.Length > 0)
                            {
                                if(_dtmFormat == null )
                                {
                                    _dtmFormat = new DateTimeFormatInfo();
                                    _dtmFormat.DateSeparator = "/";
                                    processDateTimeFormatStringUpperLower(
                                        _dtmFormat,
                                        WriterControlForWASM.FormatDateTimeUsingUpperString);
                                }
                                string txt = dtm.ToString(format, _dtmFormat);

                                if (txt.Contains("星期", StringComparison.Ordinal))
                                {
                                    string txt2 = string.Empty;
                                    switch (dtm.DayOfWeek)
                                    {
                                        case DayOfWeek.Sunday: txt2 = "星期天"; break;
                                        case DayOfWeek.Monday: txt2 = "星期一"; break;
                                        case DayOfWeek.Tuesday: txt2 = "星期二"; break;
                                        case DayOfWeek.Wednesday: txt2 = "星期三"; break;
                                        case DayOfWeek.Thursday: txt2 = "星期四"; break;
                                        case DayOfWeek.Friday: txt2 = "星期五"; break;
                                        case DayOfWeek.Saturday: txt2 = "星期六"; break;
                                    }
                                    txt = txt.Replace("星期", txt2);
                                }
                                return txt;
                            }
                            else
                            {
                                return dtm.ToLongDateString();
                            }
                        }
                        else
                        {
                            return noneText;
                        }
                    }
                case ValueFormatStyle.Numeric:
                    {
                        double dbl = 0;
                        bool flag = false;
                        if (Value is string)
                        {
                            string txt = (string)Value;
                            if (HasContent(txt))
                            {
                                if (format == "c")
                                {
                                    string temptxt = txt.Replace(",", string.Empty)
                                        .Replace("￥", string.Empty)
                                        .Replace("€", string.Empty)
                                        .Replace("$", string.Empty)
                                        .Replace("¥", string.Empty)
                                        .Replace("¤", string.Empty);
                                    flag = double.TryParse(temptxt, out dbl);
                                }
                                else
                                {
                                    flag = double.TryParse(txt, out dbl);
                                }
                            }
                        }
                        else if (IsNumberTypeFast(Value))
                        {
                            dbl = Convert.ToDouble(Value);
                            flag = true;
                        }
                        else
                        {
                            try
                            {
                                dbl = Convert.ToDouble(Value);
                                flag = true;
                            }
                            catch
                            {
                            }
                        }
                        if (flag && DoubleNaN.IsNaN(dbl) == false)
                        {
                            if (format.Length > 0)
                            {
                                if (format == "c")
                                {
                                    return "¥" + dbl.ToString("0.00");
                                }
                                else
                                {
                                    return dbl.ToString(format);
                                }
                            }
                            else
                            {
                                return dbl.ToString();
                            }
                        }
                        else
                        {
                            return noneText;
                        }
                        //break;
                    }
                case ValueFormatStyle.Percent:
                    {
                        double dbl = 0;
                        int dig = 0;
                        int rate = 100;
                        if (Value is string)
                        {
                            string txt = (string)Value;
                            if (txt != null && txt.Length > 0)
                            {
                                if (txt.IndexOf('%') > 0)
                                {
                                    rate = 1;
                                    txt = txt.Replace("%", string.Empty);
                                }
                                if (double.TryParse(txt, out dbl) == false)
                                {
                                    dbl = DoubleNaN.NaN;// double.NaN;
                                }
                            }
                            else
                            {
                                return noneText;
                            }
                        }
                        else if (IsNumberTypeFast(Value))
                        {
                            dbl = Convert.ToDouble(Value);
                        }
                        else
                        {
                            try
                            {
                                dbl = Convert.ToDouble(Value);
                            }
                            catch
                            {
                                return noneText;
                            }
                        }
                        if (Int32.TryParse(format, out dig) == false)
                        {
                            dig = 0;
                        }
                        if (dig < 0)
                        {
                            dig = 0;
                        }
                        if (DoubleNaN.IsNaN(dbl) == false)
                        {
                            dbl = Math.Round(dbl * rate, dig);
                            if (dig == 0)
                            {
                                return dbl.ToString() + '%';
                            }
                            else
                            {
                                return dbl.ToString("0." + new string('0', dig)) + "%";
                            }
                        }
                        else
                        {
                            return noneText;
                        }
                    }
                case ValueFormatStyle.SpecifyLength:
                    {
                        int specifyLength = 0;
                        string txt = Convert.ToString(Value);
                        if (Int32.TryParse(format, out specifyLength))
                        {
                            if (specifyLength > 0)
                            {
                                int len = 0;
                                foreach (char c in txt)
                                {
                                    if (c > 255)
                                        len += 2;
                                    else
                                        len++;
                                }
                                if (len < specifyLength)
                                {
                                    return Value + new string(' ', specifyLength - len);
                                }
                            }
                        }
                        return txt;
                    }
                case ValueFormatStyle.String:
                    {
                        string txt = Convert.ToString(Value);
                        if (HasContent(txt) == false)
                        {
                            return txt;
                        }
                        if (HasContent(format) == false)
                        {
                            return txt;
                        }
                        format = format.Trim();
                        if (string.Compare(format, "trim", true) == 0)
                        {
                            return txt.Trim();
                        }
                        else if (string.Compare(format, "lower", StringComparison.OrdinalIgnoreCase) == 0)
                        {
                            return txt.ToLower();
                        }
                        else if (string.Compare(format, "upper", StringComparison.OrdinalIgnoreCase) == 0)
                        {
                            return txt.ToUpper();
                        }

                        format = format.ToLower();
                        if (format.StartsWith("left"))
                        {
                            int index = format.IndexOf(',');
                            if (index > 0)
                            {
                                string left = format.Substring(index + 1);
                                int len = 0;
                                if (Int32.TryParse(left, out len))
                                {
                                    if (len > 0 && txt.Length > len)
                                    {
                                        return txt.Substring(0, len);
                                    }
                                }
                            }
                            return txt;
                        }
                        if (format.StartsWith("right"))
                        {
                            int index = format.IndexOf(',');
                            if (index > 0)
                            {
                                string right = format.Substring(index + 1);
                                int len = 0;
                                if (Int32.TryParse(right, out len))
                                {
                                    if (len > 0 && txt.Length > len)
                                    {
                                        return txt.Substring(txt.Length - len, len);
                                    }
                                }
                            }
                            return txt;
                        }
                        return txt;
                    }
                case ValueFormatStyle.Boolean:
                    {
                        if (format == null)
                        {
                            return Convert.ToString(Value);
                        }
                        int index = format.IndexOf(',');
                        string trueStr = format;
                        string falseStr = string.Empty;
                        if (index >= 0)
                        {
                            trueStr = format.Substring(0, index);
                            falseStr = format.Substring(index + 1);
                        }
                        bool bol = false;
                        bool flag = false;
                        if (Value is string)
                        {
                            flag = Boolean.TryParse((string)Value, out bol);
                        }
                        else if (Value is bool)
                        {
                            bol = (bool)Value;
                            flag = true;
                        }
                        else
                        {
                            try
                            {
                                bol = Convert.ToBoolean(Value);
                                flag = true;
                            }
                            catch
                            {
                                return noneText;
                            }
                        }
                        if (bol)
                        {
                            return trueStr;
                        }
                        else
                        {
                            return falseStr;
                        }
                        //return Value ;
                    }
            }
            return Convert.ToString(Value);
        }

        private static readonly string[] s_allDateTimeFormats = new string[] {
            "yyyy-MM-ddTHH:mm:ss+zzz",//1979-12-31T15:00:00+-01:00
            "yyyy-MM-ddTHH:mm:sszzz",//1979-12-31T15:00:00+01:00
            "yyyy-MM-ddTHH:mm:sszz",
            "yyyy-MM-ddTHH:mm:ssz",
                    "yyyy-MM-ddTHH:mm:ss.FFFFFFFzzzzzz", //dateTime
                    "yyyy-MM-ddTHH:mm:ss.FFFFFFF",
                    "yyyy-MM-ddTHH:mm:ss.FFFFFFFZ",
                    "HH:mm:ss.FFFFFFF",                  //time
                    "HH:mm:ss.FFFFFFFZ",
                    "HH:mm:ss.FFFFFFFzzzzzz",
                    "yyyy-MM-dd",                   // date
                    "yyyy-MM-ddZ",
                    "yyyy-MM-ddzzzzzz",
                    "yyyy-MM",                      // yearMonth
                    "yyyy-MMZ",
                    "yyyy-MMzzzzzz",
                    "yyyy",                         // year
                    "yyyyZ",
                    "yyyyzzzzzz",
                    "--MM-dd",                      // monthDay
                    "--MM-ddZ",
                    "--MM-ddzzzzzz",
                    "---dd",                        // day
                    "---ddZ",
                    "---ddzzzzzz",
                    "--MM--",                       // month
                    "--MM--Z",
                    "--MM--zzzzzz",
                };
        /// <summary>
        /// 将XML中的时间日期字符串解析为时间日期值
        /// </summary>
        /// <param name="strValue">字符串</param>
        /// <returns>解析结果</returns>
        public static DateTime ParseXmlDateTime( string strValue )
        {
            if( strValue == null || strValue.Length == 0 )
            {
                return WriterConst.NullDateTime;
            }
            return DateTime.ParseExact(
                strValue, 
                s_allDateTimeFormats,
                DateTimeFormatInfo.InvariantInfo,
                DateTimeStyles.AllowLeadingWhite | DateTimeStyles.AllowTrailingWhite);
        }


        static WASMUtils()
        {

        }


        public static string ToHtmlString(DragDropEffects effects)
        {
            if (effects == DragDropEffects.None) return "none";
            if (effects == DragDropEffects.Copy) return "copy";
            if (effects == DragDropEffects.Move) return "move";
            if (effects == (DragDropEffects.Copy | DragDropEffects.Move)) return "copyMove";
            if (effects == DragDropEffects.Link) return "link";
            if (effects == DragDropEffects.All) return "all";
            return "none";
        }


        private static readonly string _DCWriterXmlPrefix = "dcwriterxml20230629:";

        public static Dictionary<string, object> CreateDataDictionary(string[] strDatas)
        {
            if (strDatas != null && strDatas.Length > 0)
            {
                
                var vs = new Dictionary<string, object>();
                for (var iCount = 0; iCount < strDatas.Length; iCount += 2)
                {
                    var strType = strDatas[iCount];
                    var strData = strDatas[iCount + 1];
                    if (strData == null || strData.Length == 0)
                    {
                        continue;
                    }
                    if (strType == null || strType.Length == 0)
                    {
                        // 默认为纯文本格式
                        //vs[System.Windows.Forms.DataFormats.UnicodeText] = strData;
                        vs[System.Windows.Forms.DataFormats.Text] = strData;
                    }
                    else
                    {
                        strType = strType.Trim().ToLower();
                        if (strType == "text/plain")
                        {
                            if (strData.StartsWith(_DCWriterXmlPrefix, StringComparison.Ordinal))
                            {
                                vs[DCSoft.Writer.Dom.DocumentControler.XMLDataFormatName] = strData.Substring(_DCWriterXmlPrefix.Length);
                            }
                            else
                            {
                                vs[System.Windows.Forms.DataFormats.UnicodeText] = strData;
                                vs[System.Windows.Forms.DataFormats.Text] = strData;
                            }
                        }
                        else if (strType == "text/html")
                        {
                            vs[System.Windows.Forms.DataFormats.Html] = strData;
                        }
                        else if (strType == "image/png"
                            || strType == "image/jpg"
                            || strType == "image/jpeg"
                            || strType == "image/bmp"
                            || strType == "image/gif")
                        {
                            var bs = XImageValue.ParseEmitImageSource(strData);
                            if (bs != null)
                            {
                                vs[System.Windows.Forms.DataFormats.Bitmap] = new Bitmap(bs);
                            }
                        }
                        else if (strType == "html format")
                        {
                            vs[System.Windows.Forms.DataFormats.Html] = strData;
                        }
                        else
                        {
                            vs[strType] = strData;
                        }
                    }
                }
                return vs;
            }
            else
            {
                return null;
            }
        }

        public static T ConvertToEnum<T>(this JsonProperty p,T defaultValue) where T : struct
        {
            if (p.Value.ValueKind == JsonValueKind.String)
            {
                var str = p.Value.GetString();
                if (str == null || str.Length == 0)
                {
                    return defaultValue;
                }
                T result = default(T);
                if (Enum.TryParse<T>(str, true, out result))
                {
                    return result;
                }
                else
                {
                    return defaultValue;
                }
            }
            else if (p.Value.ValueKind == JsonValueKind.Number)
            {
                var v = p.Value.GetInt32();
                return (T)Enum.ToObject(typeof(T), v);
            }
            else
            {
                return defaultValue;
            }
        }

        public static bool ConvertToBoolean(this ref Utf8JsonReader reader, bool defaultValue)
        {
            if (reader.TokenType == JsonTokenType.True) return true;
            else if (reader.TokenType == JsonTokenType.False) return false;
            else if (reader.TokenType == JsonTokenType.Null) return defaultValue;
            else if (reader.TokenType == JsonTokenType.String)
            {
                var str = reader.GetString();
                str = str.Trim().ToLower();
                if (str == "true") return true;
                else if (str == "false") return false;
            }
            return defaultValue;
        }

        public static DCSystem_Drawing.Color ConvertToColor(this ref Utf8JsonReader reader , DCSystem_Drawing.Color defaultValue )
        {
            if(reader.TokenType == JsonTokenType.String )
            {
                var str = reader.GetString().Trim();
                return DCSoft.Common.XMLSerializeHelper.StringToColor(str, defaultValue);
            }
            else if(reader .TokenType == JsonTokenType.Number)
            {
                var v = reader.GetInt32();
                return DCSystem_Drawing.Color.FromArgb(v);
            }
            return defaultValue;
        }

        public static DateTime ConvertToDateTime( this ref Utf8JsonReader reader , long defaultValue )
        {
            if( reader.TokenType == JsonTokenType.String )
            {
                var str = reader.GetString();
                DateTime dtm = DateTime.MinValue;
                if(DateTime.TryParse( str , out dtm ))
                {
                    //if (dtm.Year == 0001)
                    //{

                    //}
                    //else
                    //{
                    //    System.Globalization.DateTimeFormatInfo dtFormat = new System.Globalization.DateTimeFormatInfo();
                    //    dtFormat.ShortDatePattern = "yyyy/MM/dd HH:mm:ss";
                    //    dtm = Convert.ToDateTime(str, dtFormat);

                    //    return dtm;
                    //}
                    return dtm;
                }
            }
            return new DateTime(defaultValue);
        }

        public static int ConvertToInt32( this ref Utf8JsonReader reader , int defaultValue )
        {
            if(reader.TokenType == JsonTokenType.Number)
            {
                return reader.GetInt32();
            }
            else if(reader.TokenType == JsonTokenType.String )
            {
                var str = reader.GetString();
                int v = 0;
                if( int.TryParse(str , out v ))
                {
                    return v;
                }
            }
            return defaultValue;
        }
        public static float ConvertToSingle(this ref Utf8JsonReader reader, float defaultValue)
        {
            if (reader.TokenType == JsonTokenType.Number)
            {
                return reader.GetSingle();
            }
            else if (reader.TokenType == JsonTokenType.String)
            {
                var str = reader.GetString();
                float v = 0;
                if (float.TryParse(str, out v))
                {
                    return v;
                }
            }
            return defaultValue;
        }
        public static double ConvertToDouble(this ref Utf8JsonReader reader, double defaultValue)
        {
            if (reader.TokenType == JsonTokenType.Number)
            {
                return reader.GetDouble();
            }
            else if (reader.TokenType == JsonTokenType.String)
            {
                var str = reader.GetString();
                double v = 0;
                if (double.TryParse(str, out v))
                {
                    return v;
                }
            }
            return defaultValue;
        }

        public static void WriteColorString( this Utf8JsonWriter writer ,string strName , DCSystem_Drawing.Color c )
        {
            writer.WriteString(strName, DCSoft.Common.XMLSerializeHelper.ColorToString(c));
        }
        public static bool ConvertToBoolean(this JsonProperty property, bool defaultValue)
        {
            return JsonElementToBoolean(property.Value, defaultValue);
        }

        /// <summary>
        /// 将一个JSON元素转换为布尔值
        /// </summary>
        /// <param name="element">JSON元素对象</param>
        /// <param name="defaultValue">转换失败后的默认值</param>
        /// <returns>转换结果</returns>
        public static bool JsonElementToBoolean(this JsonElement element, bool defaultValue)
        {
            if (element.ValueKind == JsonValueKind.True)
            {
                return true;
            }
            else if (element.ValueKind == JsonValueKind.False)
            {
                return false;
            }
            else if (element.ValueKind == JsonValueKind.String)
            {
                string v = element.GetString();
                if (v != null)
                {
                    v = v.Trim();
                    if (string.Equals(v, "true", StringComparison.OrdinalIgnoreCase))
                    {
                        return true;
                    }
                    else if (string.Equals(v, "false", StringComparison.OrdinalIgnoreCase))
                    {
                        return false;
                    }
                }
            }
            return defaultValue;
        }

        public static int ConvertToInt32(this JsonProperty property, int defalutValue)
        {
            return JsonElementToInt32(property.Value, defalutValue);
        }
        /// <summary>
        /// 将一个JSON对象转换为一个整数
        /// </summary>
        /// <param name="element">JSON元素对象</param>
        /// <param name="defalutValue">转换失败后的默认值</param>
        /// <returns>转换结果</returns>
        public static int JsonElementToInt32(this JsonElement element, int defalutValue)
        {
            int result = 0;
            if (element.ValueKind == JsonValueKind.Number)
            {
                if (element.TryGetInt32(out result))
                {
                    return result;
                }
            }
            else if (element.ValueKind == JsonValueKind.String)
            {
                var str = element.GetString();
                if (int.TryParse(str, out result))
                {
                    return result;
                }
            }
            return defalutValue;
        }
        public static float ConvertToSingle(this JsonProperty property, float defalutValue)
        {
            return JsonElementToSingle(property.Value, defalutValue);
        }
        /// <summary>
        /// 将一个JSON对象转换为一个整数
        /// </summary>
        /// <param name="element">JSON元素对象</param>
        /// <param name="defalutValue">转换失败后的默认值</param>
        /// <returns>转换结果</returns>
        public static float JsonElementToSingle(this JsonElement element, float defalutValue)
        {
            float result = 0;
            if (element.ValueKind == JsonValueKind.Number)
            {
                if (element.TryGetSingle(out result))
                {
                    return result;
                }
            }
            else if (element.ValueKind == JsonValueKind.String)
            {
                var str = element.GetString();
                if (float.TryParse(str, out result))
                {
                    return result;
                }
            }
            return defalutValue;
        }

        //wyc20240930:DUWRITER5_0-3661
        /// <summary>
        /// 设置容器元素是否改变，若设为未改变，会遍历元素内所有子元素的modified
        /// </summary>
        /// <param name="modified"></param>
        internal static void SetContainerElementModified(DomElement element, bool modified)
        {
            element.Modified = modified;
            if (modified == false && element is DomContainerElement)
            {
                DomContainerElement container = element as DomContainerElement;
                foreach(var childelement in container.Elements)
                {
                    childelement.Modified = false;
                    if (childelement is DomContainerElement)
                    {
                        SetContainerElementModified(childelement, false);
                    }
                }
            }
        }

        public static string ConvertToString( this JsonProperty p)
        {
            return JsonElementToString(p.Value);
        }
        public static string ConvertToStringEmptyAsNull(this JsonProperty p)
        {
            var s = JsonElementToString(p.Value);
            if (s == null || s.Length == 0)
            {
                return null;
            }
            else
            {
                return s;
            }
        }
        public static string JsonElementToString(this JsonElement element)
        {
            switch (element.ValueKind)
            {
                case JsonValueKind.String: return element.GetString();
                case JsonValueKind.Null: return null;
                case JsonValueKind.False: return "false";
                case JsonValueKind.True: return "true";
                case JsonValueKind.Number: return element.GetSingle().ToString();
                case JsonValueKind.Undefined: return null;
                case JsonValueKind.Object: return element.GetString();
                case JsonValueKind.Array: return null;
            }
            return null;
        }
    }
}
