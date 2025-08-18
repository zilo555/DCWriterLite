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
// // 
//using DCSoft.WinForms.Controls;
using System.Reflection;

// 袁永福到此一游
namespace DCSoft.Writer.Controls
{
    partial class WriterViewControl
    {
        /// <summary>
        /// 编辑器正在显示某种用户界面
        /// </summary>
        public bool ShowingUI = false;


        #region 状态栏文本



        /// <summary>
        /// 状态文本
        /// </summary>
        public string StatusText = null;

        /// <summary>
        /// 设置状态栏文本，并触发事件
        /// </summary>
        /// <param name="text">新状态栏文本</param>
        public void SetStatusText(string text)
        {
            if (this.StatusText != text)
            {
                this.StatusText = text;
                StatusTextChangedEventArgs args = new StatusTextChangedEventArgs(
                    this.OwnerWriterControl,
                    this.StatusText);
                this.OwnerWriterControl.OnStatusTextChanged(args);
            }
        }

        #endregion
    }
}
