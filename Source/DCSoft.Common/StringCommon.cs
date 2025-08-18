using System;
using System.Collections.Generic;
using System.Text;

namespace DCSoft.Common
{

    /// <summary>
    /// 通用的字符串处理静态对象
    /// </summary>
    /// <remarks>编制 袁永福</remarks>
    public class StringCommon
    {
        static StringCommon()
        {
            _CharStrings = new string[129];
            for (int iCount = 0; iCount < _CharStrings.Length; iCount++)
            {
                _CharStrings[iCount] = new string((char)iCount, 1);
            }
        }
        private static readonly string[] _CharStrings = null;
        public static string CharToString(char c)
        {
            if (c >= 0 && c < 129)
            {
                return _CharStrings[(int)c];
            }
            return new string(c, 1);// c.ToString();
        }
        public static string ToInt32String(float v)
        {
            return Int32ToString((int)v);
        }
        private static readonly string[] _Int32Strings = BuildInt32Strings();
        private static string[] BuildInt32Strings()
        {
            var result = new string[200];
            for (int iCount = 0; iCount < result.Length; iCount++)
            {
                result[iCount] = iCount.ToString();
            }
            return result;
        }
        public static string Int32ToString(int v)
        {
            if (v == 0)
            {
                return "0";
            }
            if (v == 1)
            {
                return "1";
            }
            if (v >= 0 && v < 200)
            {
                return _Int32Strings[v];
            }
            else
            {
                return v.ToString();
            }
        }

        private static readonly int[] _Table_IndexOfHexChar = Build_Table_IndexOfHexChar();
        private static int[] Build_Table_IndexOfHexChar()
        {
            var result = new int[103];
            for (int iCount = 0; iCount < result.Length; iCount++)
            {
                int index = -1;
                if (iCount >= '0' && iCount <= '9')
                {
                    index = iCount - '0';
                }
                else if (iCount >= 'A' && iCount <= 'F')
                {
                    index = iCount - 'A' + 10;
                }
                else if (iCount >= 'a' && iCount <= 'f')
                {
                    index = iCount - 'a' + 10;
                }
                result[iCount] = index;
            }
            return result;
        }
        /// <summary>
        /// 将16进制字符转换为数字，如果不是16进制字符则返回-1
        /// </summary>
        /// <param name="c">字符</param>
        /// <returns>数值</returns>
        public static int IndexOfHexChar(char c)
        {
            if (c < 103)
            {
                return _Table_IndexOfHexChar[c];
            }
            else
            {
                return -1;
            }
        }

        private static readonly char[] _Hexs = "0123456789ABCDEF".ToCharArray();

        public static bool MyIsWhiteSpace(char c)
        {
            return c == ' ' || c == '\r' || c == '\n' || c == '\t';
        }


        /// <summary>
        /// 压缩空白字符。将连续的空白字符压缩成一个空格。
        /// </summary>
        /// <param name="txt">要处理的字符串</param>
        /// <returns>处理后的字符串</returns>
        public static string CompressWhiteSpace(string txt)
        {
            if (string.IsNullOrEmpty(txt))
            {
                return txt;
            }
            System.Text.StringBuilder str = new System.Text.StringBuilder();
            bool flag = false;
            foreach (char c in txt)
            {
                if (MyIsWhiteSpace(c) || c == '\u3000')
                {
                    if (flag == false)
                    {
                        flag = true;
                        str.Append(' ');
                    }
                    continue;
                }
                else
                {
                    flag = false;
                }
                str.Append(c);
            }
            return str.ToString();
        }


        private static readonly int[] _Base64Indexs = BuildBase64Indexs();
        private static int[] BuildBase64Indexs()
        {
            var chrs = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/";
            var result = new int[127];
            for (int iCount = 0; iCount < 127; iCount++)
            {
                result[iCount] = chrs.IndexOf((char)iCount);
            }
            return result;
        }

        //#endregion
        public static int IndexOfBase64Char(char c)
        {
            if (c < 127)
            {
                return _Base64Indexs[c];
            }
            else
            {
                return -1;
            }
        }

        public static unsafe byte[] TryConvertFromBase64String(string strData)
        {
            if (string.IsNullOrEmpty(strData))
            {
                return null;
            }
            BitCounter c = new BitCounter();
            int len = strData.Length;
            for (int iCount = 0; iCount < len; iCount++)
            {
                int index = IndexOfBase64Char(strData[iCount]);
                if (index >= 0)
                {
                    c.AddBit(6, index);
                }
            }
            return c.ToArray();
        }

        private class BitCounter
        {
            private readonly List<byte> _Values = new List<byte>();
            private long _CurrentValue = 0;
            private int _BitCount = 0;
            public void AddBit(int bites, int Value)
            {
                if (bites > 0)
                {
                    _CurrentValue = (_CurrentValue << bites) + Value;
                    _BitCount += bites;

                    if (_BitCount >= 32)
                    {
                        Fill(false);
                    }
                }
            }
            private void Fill(bool fillAllBits)
            {
                long cv = _CurrentValue;
                int bitLeft = _BitCount % 8;
                if (bitLeft > 0)
                {
                    long mask = (1 << (bitLeft)) - 1;
                    _CurrentValue = _CurrentValue & mask;
                    cv = cv >> bitLeft;
                    _BitCount -= bitLeft;
                }
                if (_BitCount == 32)
                {
                    byte[] bs = BitConverter.GetBytes((int)cv);
                    _Values.Add(bs[3]);
                    _Values.Add(bs[2]);
                    _Values.Add(bs[1]);
                    _Values.Add(bs[0]);
                    _BitCount = 0;
                }
                else
                {
                    while (_BitCount > 0)
                    {
                        _BitCount -= 8;
                        byte v = (byte)((cv >> _BitCount) & 0xff);
                        _Values.Add(v);
                    }
                }
                _BitCount = bitLeft;
            }
            public byte[] ToArray()
            {
                if (_BitCount > 0)
                {
                    Fill(true);
                }
                return _Values.ToArray();
            }
        }
    }// class StringCommon
}
