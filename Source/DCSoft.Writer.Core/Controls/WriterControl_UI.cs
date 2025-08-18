using System;
using System.Collections.Generic;
using System.Text;
using DCSoft.Common;
using DCSoft.Drawing;
using DCSoft.Writer.Dom;
using System.Windows.Forms;
using System.ComponentModel;
using System.Runtime.InteropServices;
using DCSoft.WinForms;
namespace DCSoft.Writer.Controls
{
    partial class WriterControl
    {
        /// <summary>
        /// 报告错误事件委托类型
        /// </summary>
        /// <param name="parentWindow"></param>
        /// <param name="message"></param>
        /// <param name="details"></param>
        public delegate void ReportErrorHandler(
            System.Windows.Forms.IWin32Window parentWindow,
            string message,
            string details);

        /// <summary>
        /// 内部的报告错误事件
        /// </summary>
        public static ReportErrorHandler InnerGlobalEventReportError = null;


        private static Cursor _CursorSizeWE = Cursors.SizeWE;
        /// <summary>
        /// 水平方向调整位置或大小的鼠标光标对象
        /// </summary>
        public static Cursor InnerCursorSizeWE
        {
            get
            {
                if (_CursorSizeWE == null)
                {
                    _CursorSizeWE = Cursors.SizeWE;
                }
                return _CursorSizeWE;
            }
            set
            {
                _CursorSizeWE = value;
            }
        }

        private static Cursor _CursorSizeNS = Cursors.SizeNS;
        /// <summary>
        /// 垂直方向调整位置或大小的鼠标光标对象
        /// </summary>
        public static Cursor InnerCursorSizeNS
        {
            get
            {
                if (_CursorSizeNS == null)
                {
                    _CursorSizeNS = Cursors.SizeNS;
                }
                return _CursorSizeNS;
            }
            set
            {
                _CursorSizeNS = value;
            }
        }

        private static Cursor _CursorSelectCell = null;
        /// <summary>
        /// 表示选择单元格的鼠标光标对象
        /// </summary>
        public static Cursor InnerCursorSelectCell
        {
            get
            {
                if (_CursorSelectCell == null)
                {
                    _CursorSelectCell = XCursors.SelectCell;
                }
                return _CursorSelectCell;
            }
            set
            {
                _CursorSelectCell = value;
            }
        }

        private static Cursor _CursorArrow = Cursors.Arrow;
        /// <summary>
        /// 表示指针的鼠标光标对象
        /// </summary>
        public static Cursor InnerCursorArrow
        {
            get
            {
                if (_CursorArrow == null)
                {
                    _CursorArrow = Cursors.Arrow;
                }
                return _CursorArrow;
            }
            set
            {
                _CursorArrow = value;
            }
        }

        private bool _CommandStateNeedRefreshFlag = false;
        /// <summary>
        /// 命令状态需要刷新标识
        /// </summary>
        /// <remarks>
        /// 当不能启用控件事件或者无法响应控件事件时，可以使用定时器来判断本属性值，如果本属性值为true，
        /// 则需要刷新应用程序界面按钮的状态，然后设置本属性值为false。
        /// 比如
        /// void Timer_Intervel()
        /// {
        ///     if( myWriterControl.CommandStateNeedRefreshFlag == true )
        ///     {
        ///         myWriterControl.CommandStateNeedRefreshFlag = false ;
        ///         ---- 此处编写刷新菜单按钮状态的代码 ----
        ///     }
        /// }
        /// 在DCWriter内部会根据实时情况来自动设置本属性值为true，以标记应用程序需要刷新按钮状态了。
        /// </remarks>
        public bool CommandStateNeedRefreshFlag
        {
            get
            {
                return _CommandStateNeedRefreshFlag;
            }
            set
            {
                this._CommandStateNeedRefreshFlag = value;
            }
        }
        /// <summary>
        /// 编辑器控件获得输入焦点
        /// </summary>
        /// <returns></returns>
        new public bool Focus()
        {
            if (this.GetInnerViewControl() == null)
            {
                return false;
            }
            else
            {
                return this.GetInnerViewControl().Focus();
            }
        }

        /// <summary>
        /// 销毁控件的时候是否自动销毁文档对象
        /// </summary>
        public bool AutoDisposeDocument
        {
            get
            {
                return this.GetInnerViewControl() == null ? true : this.GetInnerViewControl().AutoDisposeDocument;
            }
            set
            {
                if (this.GetInnerViewControl() != null)
                {
                    this.GetInnerViewControl().AutoDisposeDocument = value;
                }
            }
        }
    }
}
