using System;
using System.Collections.Generic;
using System.Text;

namespace DCSoft.Writer.Controls
{
    /// <summary>
    /// 编辑器编辑模式
    /// </summary>
    [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
    public enum ElementValueEditorEditStyle
    {
        /// <summary>
        /// 无编辑器
        /// </summary>
        None,
        /// <summary>
        /// 下拉列表模式
        /// </summary>
        DropDown,
        /// <summary>
        /// 对话框模式
        /// </summary>
        Modal
    }
}
