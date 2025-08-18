using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Reflection;
using System.IO;
using DCSystemXml;
// // 

namespace DCSoft.Common
{
    public static class XMLSerializeHelper
    {

        private static readonly string _X2 = "X2";
        private static readonly int _ARGB_BLACK = DCSystem_Drawing.Color.Black.ToArgb();
        private static readonly string _Black_String = "#000000";
        private static readonly int _ARGB_WHITE = DCSystem_Drawing.Color.White.ToArgb();
        private static readonly string _White_String = "#FFFFFF";
        private static readonly int _ARGB_Gray = DCSystem_Drawing.Color.Gray.ToArgb();
        private static readonly string _Gray_String = "#" + DCSystem_Drawing.Color.Gray.R.ToString(_X2) 
            + DCSystem_Drawing.Color.Gray.G.ToString(_X2) 
            + DCSystem_Drawing.Color.Gray.B.ToString(_X2);
        public static string ColorToString(DCSystem_Drawing.Color c)
        {
            if (c.A == 255)
            {
                var argb = c.ToArgb();
                if( argb == _ARGB_BLACK )
                {
                    return _Black_String;
                }
                if( argb == _ARGB_WHITE)
                {
                    return _White_String;
                }
                if(argb == _ARGB_Gray)
                {
                    return _Gray_String;
                }
                string v = '#' + c.R.ToString(_X2) + c.G.ToString(_X2) + c.B.ToString(_X2);
                return v;
            }
            else
            {
                string v = '#' + c.A.ToString(_X2) + c.R.ToString(_X2) + c.G.ToString(_X2) + c.B.ToString(_X2);
                return v;
            }
        }

        //[System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public static string ColorToString(DCSystem_Drawing.Color c, DCSystem_Drawing.Color defaultValue)
        {
            if (c.ToArgb() == defaultValue.ToArgb())
            {
                return null;
            }
            else
            {
                if (c.A == 255)
                {
                    var argb = c.ToArgb();
                    if (argb == _ARGB_BLACK)
                    {
                        return _Black_String;
                    }
                    if (argb == _ARGB_WHITE)
                    {
                        return _White_String;
                    }
                    if (argb == _ARGB_Gray)
                    {
                        return _Gray_String;
                    }
                    string v = '#' + c.R.ToString(_X2) + c.G.ToString(_X2) + c.B.ToString(_X2);
                    return v;
                }
                else
                {
                    string v = '#' +  c.A.ToString(_X2) + c.R.ToString(_X2) + c.G.ToString(_X2) + c.B.ToString(_X2);
                    return v;
                }
            }
        }

        private static int Parse2HexValue(string txt, int startIndex)
        {
            int result = 0;
            int index = StringCommon.IndexOfHexChar(txt[startIndex]);
            if (index > 0)
            {
                result = index << 4;
            }
            index = StringCommon.IndexOfHexChar(txt[startIndex + 1]);
            if (index > 0)
            {
                result += index;
            }
            return result;
        }

        private static readonly string _Color_00000000 = "#00000000";
        private static readonly string _Color_00ffffff = "#00ffffff";
        private static readonly string _Color_000000 = "#000000";
        private static readonly string _Color_ffffff = "#ffffff";
        private static readonly string _Empty = "Empty";

        public static DCSystem_Drawing.Color StringToColor(string v, DCSystem_Drawing.Color defaultValue)
        {
            if (string.IsNullOrEmpty(v ))
            {
                return defaultValue;
            }
            else
            {
                v = v.Trim();
                try
                {
                    if (v[0] == '#')
                    {
                        if (v.Length == 9)
                        {
                            if (v == _Color_00000000)
                            {
                                return DCSystem_Drawing.Color.Empty;
                            }
                            if (string.Equals(v, _Color_00ffffff, StringComparison.OrdinalIgnoreCase))
                            {
                                return DCSystem_Drawing.Color.Transparent;
                            }
                            int a = Parse2HexValue(v, 1);
                            int r = Parse2HexValue(v, 3);
                            int g = Parse2HexValue(v, 5);// int.Parse(v.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
                            int b = Parse2HexValue(v, 7);// int.Parse(v.Substring(6, 2), System.Globalization.NumberStyles.HexNumber);
                            var c = DCSystem_Drawing.Color.FromArgb(a, r, g, b);
                            return c;
                        }
                        else if (v.Length == 7)
                        {
                            if (v == _Color_000000)
                            {
                                return DCSystem_Drawing.Color.Black;
                            }
                            if (string.Equals(v, _Color_ffffff, StringComparison.OrdinalIgnoreCase))
                            {
                                return DCSystem_Drawing.Color.White;
                            }
                            int r = Parse2HexValue(v, 1);// int.Parse(v.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
                            int g = Parse2HexValue(v, 3);// int.Parse(v.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
                            int b = Parse2HexValue(v, 5);// int.Parse(v.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
                            var c = DCSystem_Drawing.Color.FromArgb(r, g, b);
                            return c;
                        }
                    }
                    v = v.Trim();
                    if( v.Length > 10 && v.StartsWith("rgb(", StringComparison.OrdinalIgnoreCase) )
                    {
                        v = v.Substring(4, v.Length - 5);
                    }
                    if (v.IndexOf(',') > 0)
                    {
                        string[] items = v.Split(',');
                        var vs = new int[4];
                        var vsCount = 0;
                        foreach (string item in items)
                        {
                            int iv = 0;
                            if (int.TryParse(item.Trim(), out iv))
                            {
                                vs[vsCount++] = iv;
                                if(vsCount == 4 )
                                {
                                    break;
                                }
                            }
                        }
                        if (vsCount == 1)
                        {
                            return DCSystem_Drawing.Color.FromArgb(vs[0], 255, 255);
                        }
                        else if (vsCount == 2)
                        {
                            return DCSystem_Drawing.Color.FromArgb(vs[0], vs[1], 255);
                        }
                        else if (vsCount == 3)
                        {
                            return DCSystem_Drawing.Color.FromArgb(vs[0], vs[1], vs[2]);
                        }
                        else if (vsCount == 4)
                        {
                            return DCSystem_Drawing.Color.FromArgb(vs[0], vs[1], vs[2], vs[3]);
                        }
                    }
                    if (v == _Empty || string.Equals(v, _Empty, StringComparison.OrdinalIgnoreCase))
                    {
                        return DCSystem_Drawing.Color.Empty;
                    }
                    var c2 = DCSystem_Drawing.ColorTranslator.FromHtml(v);
                    return c2;
                }
                catch
                {
                    return defaultValue;
                }
            }
        }


    }
}
