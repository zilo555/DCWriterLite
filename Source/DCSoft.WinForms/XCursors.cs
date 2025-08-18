using System;
using System.Windows.Forms ;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.IO;

namespace DCSoft.WinForms
{
    /// <summary>
    /// 扩展鼠标光标对象,本对象提供了一些非标准的鼠标光标对象
    /// </summary>
    public static partial class XCursors
    {
        /// <summary>
        /// 选择单元格光标对象
        /// </summary>
        public static readonly System.Windows.Forms.Cursor SelectCell = new Cursor("cell");
        /// <summary>
        /// 格式刷光标2对象
        /// </summary>
        public static readonly System.Windows.Forms.Cursor FormatBrush2 = System.Windows.Forms.Cursors.Hand;
        /// <summary>
        /// 手型拖拽松开光标对象
        /// </summary>
        public static readonly System.Windows.Forms.Cursor HandDragUp = new Cursor("grab");
        /// <summary>
        /// 手型拖拽按下光标对象
        /// </summary>
        public static readonly System.Windows.Forms.Cursor HandDragDown = new Cursor("grabbing");
        /// <summary>
        /// 向右的鼠标光标对象
        /// </summary>
        public static readonly System.Windows.Forms.Cursor RightArrow = System.Windows.Forms.Cursors.PanNE;
        public static readonly System.Windows.Forms.Cursor Hand = new Cursor("pointer");
    }//public sealed class XCursors
}