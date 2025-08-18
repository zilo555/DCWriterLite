using System;
using System.Collections.Generic;
using System.Text;

namespace DCSoft.Common
{
    /// <summary>
    /// 内容格式化输出的函数集
    /// </summary>
    public static class FormatUtils
    {
        public static string ToYYYY_MM_DD(DateTime dtm)
        {
            return dtm.ToString("yyyy-MM-dd");
        }
    }
}
