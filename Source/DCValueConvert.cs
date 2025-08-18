using DCSoft.Common;
using DCSoft.Writer.Dom;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace DCSoft.WASM
{
   
    /// <summary>
    /// 定义一些供JS中调用的静态函数
    /// </summary>
    public static class DCValueConvert
    {
        
        internal static string ObjectToString(object v)
        {
            if (v == null)
            {
                return null;
            }
            if( v is JsonElement e2)
            {
                if(e2.ValueKind == JsonValueKind.String )
                {
                    return e2.GetString();
                }
                else if (e2.ValueKind == JsonValueKind.Null || e2.ValueKind == JsonValueKind.Undefined)
                {
                    return null;
                }
                else if(e2.ValueKind == JsonValueKind.True )
                {
                    return "true";
                }
                else if( e2.ValueKind == JsonValueKind.False)
                {
                    return "false";
                }
                //wyc20241106:DUWRITER5_0-3805
                else if (e2.ValueKind == JsonValueKind.Number)
                {
                    double vv = e2.GetDouble();
                    return vv.ToString();
                }
                else if (e2.ValueKind == JsonValueKind.Null)
                {
                    return null;
                }
                ////////////////////////////////
                else
                {
                    return e2.GetString();
                }
            }
            return Convert.ToString(v);
        }
        internal static bool ObjectToBoolean(object v, bool defaultValue = false)
        {
            if (v == null)
            {
                return defaultValue;
            }
            string str = null;
            if (v is JsonElement e2)
            {
                if (e2.ValueKind == JsonValueKind.True)
                {
                    return true;
                }
                else if (e2.ValueKind == JsonValueKind.False)
                {
                    return false;
                }
                else if (e2.ValueKind == JsonValueKind.Null || e2.ValueKind == JsonValueKind.Undefined)
                {
                    return defaultValue;
                }
                else
                {
                    str = e2.GetString();
                }
            }
            else
            {
                str = Convert.ToString(v).ToLower();
            }
            if (str == "true" || str == "yes" || str == "on")
            {
                return true;
            }
            if (str == "false" || str == "no" || str == "off")
            {
                return false;
            }
            return defaultValue;
        }
        

        internal static int ObjectToInt32(object v, int defaultValue = 0)
        {
            if (v == null)
            {
                return defaultValue;
            }
            int result = 0;
            if (v is System.Text.Json.JsonElement)
            {
                var je = (System.Text.Json.JsonElement)v;
                if (je.TryGetInt32(out result))
                {
                    return result;
                }
            }
            var str = v.ToString();
            if (int.TryParse(str, out result))
            {
                return result;
            }
            return defaultValue;
        }

        /// <summary>
        /// 将HTML格式的颜色值字符串转化为颜色值,支持#fff,#ffffff,rgb(),rgba()模式
        /// </summary>
        /// <param name="str"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        internal static Color HtmlToColor( string str , Color defaultValue )
        {
            Color result;
            if( ColorTranslator.TryFromHtml( str , out result ) )
            {
                return result;
            }
            else
            {
                return defaultValue;
            }
            //return XMLSerializeHelper.StringToColor(str, defaultValue);
        }
        /// <summary>
        /// 试图将一个HTML颜色字符串转换为一个颜色值
        /// </summary>
        /// <param name="htmlColor">HTML颜色字符串</param>
        /// <param name="result">转换成功后的颜色值</param>
        /// <returns>操作是否成功</returns>
        internal static bool TryHtmlToColor(string htmlColor, out Color result)
        {
            return ColorTranslator.TryFromHtml(htmlColor, out result);
        }
    }
}
