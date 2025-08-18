using System;
using System.Collections.Generic;
using System.Text;

namespace DCSoft.Writer
{
    /// <summary>
    /// 元素数值编辑器激活模式
    /// </summary>
     
    [Flags]
    [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
    public enum ValueEditorActiveMode
    {
        /// <summary>
        /// 无效状态
        /// </summary>
        None = 0 ,
        /// <summary>
        /// 采用默认模式
        /// </summary>
        Default = 1,
        /// <summary>
        /// 编程激活
        /// </summary>
        Program = 2 ,
        /// <summary>
        /// 按下F2激活
        /// </summary>
        F2 = 4 ,
        /// <summary>
        /// 对象获得输入焦点就激活
        /// </summary>
        GotFocus = 8,
        /// <summary>
        /// 鼠标双击时就激活
        /// </summary>
        MouseDblClick = 16 ,
        /// <summary>
        /// 鼠标左击就激活
        /// </summary>
        MouseClick = 32 ,
        /// <summary>
        /// 鼠标右击就激活，当控件设置了快捷菜单，则该选项无效。
        /// </summary>
        MouseRightClick = 64 ,
        /// <summary>
        /// Enter键激活。
        /// </summary>
        Enter = 128
    }
}