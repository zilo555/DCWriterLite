using System;
using System.Collections.Generic;
using System.Text;

// 袁永福到此一游

namespace DCSoft.WinForms
{
    /// <summary>
    /// 鼠标事件类型
    /// </summary>
    public enum MouseEventType
    {
        /// <summary>
        /// 鼠标按键按下
        /// </summary>
        MouseDown ,
        /// <summary>
        /// 鼠标移动
        /// </summary>
        MouseMove,
        /// <summary>
        /// 鼠标按键松开
        /// </summary>
        MouseUp ,
        /// <summary>
        /// 鼠标单击
        /// </summary>
        MouseClick ,
        /// <summary>
        /// 鼠标双击
        /// </summary>
        MouseDblClick ,
        /// <summary>
        /// 鼠标离开
        /// </summary>
        MouseLeave,
        /// <summary>
        /// 鼠标进入
        /// </summary>
        MouseEnter
    }
}
