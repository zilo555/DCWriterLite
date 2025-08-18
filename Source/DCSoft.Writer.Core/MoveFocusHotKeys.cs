using System;
using System.Collections.Generic;
using System.Text;
using DCSoft.Common;
using System.ComponentModel ;

namespace DCSoft.Writer
{
    /// <summary>
    /// 移动输入域焦点的快捷键
    /// </summary>
    /// <remarks>编制 袁永福</remarks>
    [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true )]
    [Flags]
    public enum MoveFocusHotKeys
    {
        /// <summary>
        /// 无快捷键
        /// </summary>
        None = 0,
        /// <summary>
        /// 默认方式
        /// </summary>
        Default = 1,
        /// <summary>
        /// Tab键
        /// </summary>
        Tab = 2,
        /// <summary>
        /// Enter键
        /// </summary>
        Enter = 4
    }
}
