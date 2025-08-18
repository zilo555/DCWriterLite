using System;
using System.Collections.Generic;
using System.Text;

namespace DCSoft.Writer
{
    /// <summary>
    /// 在文档中移动的单位
    /// </summary>
    /// <remarks>编制 袁永福</remarks>
    [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true )]
    [System.Text.Json.Serialization.JsonConverter(typeof(System.Text.Json.Serialization.JsonStringEnumConverter))]
    public enum MoveUnit
    {
        /// <summary>
        /// 字符
        /// </summary>
        Character ,
        /// <summary>
        /// 单词
        /// </summary>
        Word ,
        /// <summary>
        /// 文本行
        /// </summary>
        Line ,
        /// <summary>
        /// 段落
        /// </summary>
        Paragraph ,
        /// <summary>
        /// 单元格
        /// </summary>
        Cell ,

    }

    /// <summary>
    /// 移动目标
    /// </summary>
    /// <remarks>编制 袁永福</remarks>
    [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
    public enum MoveTarget
    {
        /// <summary>
        /// 无意义
        /// </summary>
        None ,
        /// <summary>
        /// 文档开头
        /// </summary>
        DocumentHome ,
         /// <summary>
        /// 文档尾
        /// </summary>
        DocumentEnd,
        /// <summary>
        /// 单元格的开头
        /// </summary>
        CellHome,
        /// <summary>
        /// 单元格结尾
        /// </summary>
        CellEnd ,
        /// <summary>
        /// 上一个单元格
        /// </summary>
        PreCell ,
        /// <summary>
        /// 下一个单元格
        /// </summary>
        NextCell,
        /// <summary>
        /// 表格前面
        /// </summary>
        BeforeTable ,
        /// <summary>
        /// 表格后面
        /// </summary>
        AfterTable,
        /// <summary>
        /// 文档域前面
        /// </summary>
        BeforeField,
        /// <summary>
        /// 文档域后面
        /// </summary>
        AfterField,
        /// <summary>
        /// 段落开头
        /// </summary>
        ParagraphHome ,
        /// <summary>
        /// 段落尾
        /// </summary>
        ParagraphEnd ,
        /// <summary>
        /// 上一行
        /// </summary>
        PreLine ,
        /// <summary>
        /// 下一行
        /// </summary>
        NextLine,
        /// <summary>
        /// 输入域开头
        /// </summary>
        FieldHome ,
        /// <summary>
        /// 输入域结尾
        /// </summary>
        FieldEnd ,
        /// <summary>
        /// 行首
        /// </summary>
        Home ,
        /// <summary>
        /// 行尾
        /// </summary>
        End ,
        /// <summary>
        /// 当前页首
        /// </summary>
        PageHome ,
        /// <summary>
        /// 当前页尾
        /// </summary>
        PageEnd 
    }
}
