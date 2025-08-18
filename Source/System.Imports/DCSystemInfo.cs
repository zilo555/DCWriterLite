
// 定义程序集发布日期，本代码是由 DCSoft.CompilerCodeGen 自动生成的
using System;
using System.Collections.Generic;
using System.Text;

namespace DCSoft
{
    public static partial class DCSystemInfo
    {
        public static DateTime PublishDateTime
            = new DateTime(2023, 8,2, 20, 1, 1);

        public static string VersionString 
        { get 
            {
                return PublishDateTime.ToString("yyyyMMddHHmmss");
            } }

        public static long Version { get { return Convert.ToInt64(VersionString); } }
        //internal static readonly int PublishYear = PublishDateTime.Year;
        //internal const int PublishMonth = 7;
        //internal const int PublishDay = 20;
        public static string PublishDateString { get { return PublishDateTime.ToString("yyyy-MM-dd HH:mm:ss"); } }
        public static long PublishDateTick { get { return PublishDateTime.Ticks; } }

    }
}

