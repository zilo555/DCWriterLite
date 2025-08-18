using System;
using System.Collections.Generic;
using System.Text;

namespace DCSoft.Common
{
    public static class DateTimeCommon
    {
        public static DateTime GetNow()
        {
            return DateTime.Now;
        }

        public const string FORMAT_yyyy_MM_dd_HH_mm_ss = "yyyy-MM-dd HH:mm:ss";

        private static char[] _Chars = "2020-05-04 15:28:11".ToCharArray();
        /// <summary>
        /// 快速将日期时间转换为字符串
        /// </summary>
        /// <param name="dtm"></param>
        /// <returns></returns>
        public static string FastToYYYY_MM_DD_HH_MM_SS(DateTime dtm)
        {
            if (_Chars == null)
            {
                // -------0123456789012345678
                _Chars = "2020-05-04 15:28:11".ToCharArray();
            }
            int dec = 0;
            int num = dtm.Year;
            dec = num % 10; _Chars[3] = (char)(dec + '0'); num = (num - dec) / 10;
            dec = num % 10; _Chars[2] = (char)(dec + '0'); num = (num - dec) / 10;
            dec = num % 10; _Chars[1] = (char)(dec + '0'); num = (num - dec) / 10;
            dec = num % 10; _Chars[0] = (char)(dec + '0');

            num = dtm.Month;
            dec = num % 10; _Chars[6] = (char)(dec + '0'); num = (num - dec) / 10;
            dec = num % 10; _Chars[5] = (char)(dec + '0');

            num = dtm.Day;
            dec = num % 10; _Chars[9] = (char)(dec + '0'); num = (num - dec) / 10;
            dec = num % 10; _Chars[8] = (char)(dec + '0');

            num = dtm.Hour;
            dec = num % 10; _Chars[12] = (char)(dec + '0'); num = (num - dec) / 10;
            dec = num % 10; _Chars[11] = (char)(dec + '0');

            num = dtm.Minute;
            dec = num % 10; _Chars[15] = (char)(dec + '0'); num = (num - dec) / 10;
            dec = num % 10; _Chars[14] = (char)(dec + '0');

            num = dtm.Second;
            dec = num % 10; _Chars[18] = (char)(dec + '0'); num = (num - dec) / 10;
            dec = num % 10; _Chars[17] = (char)(dec + '0');

            return new string(_Chars);
        }
    }
}
