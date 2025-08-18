using System;
using System.Runtime.InteropServices;
using System.Text;
using System.IO;
using System.IO.Compression;
using System.Reflection;
using System.Collections.Generic;

namespace DCSoft.Common
{
    /// <summary>
    /// 文件处理类,DCWriter内部使用。
    /// </summary>
    /// <remarks>编制 袁永福</remarks>
    public static class FileHelper
    {
        /// <summary>
        /// 修整文件名
        /// </summary>
        /// <param name="strFileName">原始文件名</param>
        /// <param name="InvalidReplaceChar">替换掉错误字符的字符</param>
        /// <returns>修整后的文件名</returns>
        public static string FixFileName(string strFileName, char InvalidReplaceChar)
        {
            if (string.IsNullOrEmpty(strFileName))
            {
                return strFileName;
            }
            const string InValidChars = "\\/:*?\"<>|#";
            if (InValidChars.Contains(InvalidReplaceChar))
            { 
                throw new ArgumentNullException("InvalidReplaceChar");
            }
            System.Text.StringBuilder myStr = new StringBuilder();
            foreach (char c in strFileName)
            {
                if (InValidChars.Contains(c))
                {
                    if (InvalidReplaceChar > 0)
                    {
                        myStr.Append(InvalidReplaceChar);
                    }
                }
                else
                {
                    myStr.Append(c);
                }
            }
            return myStr.ToString();
        }

        /// <summary>
        /// 格式化输出字节大小数据
        /// </summary>
        /// <param name="ByteSize">字节数</param>
        /// <returns>输出的字符串</returns>
        public static string FormatByteSize(long byteSize)
        {
            const long _PBSIZE = (long)1024 * 1024 * 1024 * 1024;
            const long _GBSIZE = 1024 * 1024 * 1024;
            const long _MBSIZE = 1024 * 1024;
            const long _KBSIZE = 1024;

            long v = byteSize;
            string unit = null;
            if (byteSize > _PBSIZE)
            {
                v = v * 100 / _PBSIZE;
                unit = "PB";
            }
            else if (byteSize > _GBSIZE)
            {
                v = v * 100 / _GBSIZE;
                unit = "GB";
            }
            else if (byteSize > _MBSIZE)
            {
                v = v * 100 / _MBSIZE;
                unit = "MB";
            }
            else if (byteSize > _KBSIZE)
            {
                v = v * 100 / _KBSIZE;
                unit = "KB";
            }
            else
            {
                return byteSize.ToString() + "B";
            }
            int mod = (int)(v % 100);
            v = v / 100;
            if (v > 10)
            {
                mod = mod / 10;
            }
            if (mod == 0)
            {
                return v.ToString() + unit;
            }
            else
            {
                return v.ToString() + '.' + mod.ToString() + unit;
            }
        }

    }//public sealed class FileNameHelper
}