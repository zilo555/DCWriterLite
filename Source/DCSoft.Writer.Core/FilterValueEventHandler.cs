using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace DCSoft.Writer
{
    /// <summary>
    /// 输入来源
    /// </summary>
    public enum InputValueSource
    {
        /// <summary>
        /// 数据来自系统剪切板
        /// </summary>
        Clipboard,
        /// <summary>
        /// 数据来用户界面的用户输入
        /// </summary>
        UI,
        /// <summary>
        /// OLE拖拽
        /// </summary>
        DragDrop,
        /// <summary>
        /// 未知
        /// </summary>
        Unknow
    }

}