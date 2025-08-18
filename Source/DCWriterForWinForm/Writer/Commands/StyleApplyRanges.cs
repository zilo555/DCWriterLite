using System;
using System.Collections.Generic;
using System.Text;

namespace DCSoft.Writer.Commands
{
    /// <summary>
    /// 样式应用范围
    /// </summary>
     
    [Flags]
    //[System.Runtime.InteropServices.ComVisible(true)]
    [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
    //[DCSoft.Common.DCPublishAPI]
    //[System.Runtime.InteropServices.Guid("ABC522FF-9F85-4862-AD7E-10F370F4E191")]
    public enum StyleApplyRanges
    {
        /// <summary>
        /// 没有应用范围
        /// </summary>
        None= 0 ,
        /// <summary>
        /// 应用于文本
        /// </summary>
        Text = 1,
        /// <summary>
        /// 应用到字段
        /// </summary>
        Field = 2 ,
        /// <summary>
        /// 应用到段落
        /// </summary>
        Paragraph = 4 ,
        /// <summary>
        /// 应用到单元格
        /// </summary>
        Cell = 8 ,
        /// <summary>
        /// 应用到表格行
        /// </summary>
        Row = 16 ,
        /// <summary>
        /// 应用到表格
        /// </summary>
        Table = 32
    }
}
