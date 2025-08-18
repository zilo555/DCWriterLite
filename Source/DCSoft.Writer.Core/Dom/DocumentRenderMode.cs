using System;
using System.Collections.Generic;
using System.Text;

namespace DCSoft.Writer.Dom
{
    /// <summary>
    /// 正在呈现的的文档模式
    /// </summary>
    [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
    public enum DocumentRenderMode
    {
        /// <summary>
        /// 正在WinForm用户界面上绘制图形
        /// </summary>
        Paint,
        /// <summary>
        /// 正在打印
        /// </summary>
        Print
    }
}
