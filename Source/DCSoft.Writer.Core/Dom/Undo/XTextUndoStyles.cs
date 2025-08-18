using System;
using System.Collections.Generic;
using System.Text;

namespace DCSoft.Writer.Dom.Undo
{
    /// <summary>
    /// 撤销操作类型
    /// </summary>
    public enum XTextUndoStyles
    {
        /// <summary>
        /// 无样式
        /// </summary>
        None,
        /// <summary>
        /// 元素大小
        /// </summary>
        Size,
        /// <summary>
        /// 设计器中的元素大小
        /// </summary>
        EditorSize,
        /// <summary>
        /// 表格行高度
        /// </summary>
        TableRowSpecifyHeight,
        InnerValue
    }
}
