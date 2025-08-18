using System;
using System.Collections.Generic;
using System.Text;

namespace DCSoft.Printing
{
    /// <summary>
    /// 打印模式
    /// </summary>
     
    [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true )]
    [System.Text.Json.Serialization.JsonConverter(typeof(System.Text.Json.Serialization.JsonStringEnumConverter))]
    public enum DCPrintMode
    {
        /// <summary>
        /// 正常打印模式
        /// </summary>
        Normal = 0 ,
        /// <summary>
        /// 打印奇数页
        /// </summary>
        OddPage = 1 ,
        /// <summary>
        /// 打印偶数页
        /// </summary>
        EvenPage = 2 ,
        /// <summary>
        /// 手动双面打印
        /// </summary>
        ManualDuplex = 3,
        /// <summary>
        /// 打印当前页
        /// </summary>
        CurrentPage = 4

    }
}
