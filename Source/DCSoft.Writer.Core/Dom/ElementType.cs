using System;
using System.Collections.Generic;
using System.Text;
using DCSoft.Common;

namespace DCSoft.Writer.Dom
{
    /// <summary>
    /// 内容元素类型
    /// </summary>
    [Flags]
    [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true )]
    public enum ElementType
    {
        /// <summary>
        /// 无效类型
        /// </summary>
        //[DCDescription(typeof(ElementType), "None")]
        None = 0 ,
        /// <summary>
        /// 文本元素
        /// </summary>
        //[DCDescription(typeof(ElementType), "Text")]
        Text = 1,
        /// <summary>
        /// 字段元素
        /// </summary>
        //[DCDescription(typeof(ElementType), "Field")]
        Field = 2,
        /// <summary>
        /// 输入框元素
        /// </summary>
        //[DCDescription(typeof(ElementType), "InputField")]
        InputField = 4,
        /// <summary>
        /// 表格元素
        /// </summary>
        //[DCDescription(typeof(ElementType), "Table")]
        Table = 8,
        /// <summary>
        /// 表格行
        /// </summary>
        //[DCDescription(typeof(ElementType), "TableRow")]
        TableRow = 16,
        /// <summary>
        /// 表格列
        /// </summary>
        //[DCDescription(typeof(ElementType), "TableColumn")]
        TableColumn = 32,
        /// <summary>
        /// 表格单元格
        /// </summary>
        //[DCDescription(typeof(ElementType), "TableCell")]
        TableCell = 64,
        /// <summary>
        /// 嵌入的对象
        /// </summary>
        //[DCDescription(typeof(ElementType), "Object")]
        Object = 128,
        /// <summary>
        /// 换行
        /// </summary>
        //[DCDescription(typeof(ElementType), "LineBreak")]
        LineBreak = 256,
        /// <summary>
        /// 分页符号
        /// </summary>
        //[DCDescription(typeof(ElementType), "PageBreak")]
        PageBreak = 512,
        /// <summary>
        /// 段落标记
        /// </summary>
        //[DCDescription(typeof(ElementType), "ParagraphFlag")]
        ParagraphFlag = 1024,
        /// <summary>
        /// 单复选框
        /// </summary>
        //[DCDescription(typeof(ElementType), "CheckRadioBox")]
        CheckRadioBox = 2048,
        /// <summary>
        /// 复选框，仅供兼容使用。
        /// </summary>
        CheckBox = 2048,
        /// <summary>
        /// 图片元素
        /// </summary>
        //[DCDescription(typeof(ElementType), "Image")]
        Image = 4096,
        /// <summary>
        /// 文档对象
        /// </summary>
        //[DCDescription(typeof(ElementType), "Document")]
        Document = 8192,
        /// <summary>
        /// 按钮
        /// </summary>
        Button = 8192 * 2 ,
        /// <summary>
        /// 容器元素
        /// </summary>
        Container = 8192 * 4,
        /// <summary>
        /// 所有类型
        /// </summary>
        //[DCDescription(typeof(ElementType), "All")]
        All = 0xffffff
    }
}
