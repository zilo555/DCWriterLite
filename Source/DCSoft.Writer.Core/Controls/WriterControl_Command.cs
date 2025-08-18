using System;
using DCSoft.WinForms;
using DCSoft.Printing;
using DCSoft.Writer.Dom;
using DCSoft.Writer.Commands;
using DCSoft.Writer.Dom.Undo;
using DCSoft.Drawing;
using System.Windows.Forms;
// // 
using DCSoft.WinForms.Native;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Collections;
using System.Collections.Generic;
using DCSoft.Common;
using DCSoft.Writer.Undo;
using System.Text;
using DCSoft.Writer.Data;

namespace DCSoft.Writer.Controls
{
    /// <summary>
    /// 编辑器命令相关功能模块
    /// </summary>
    ///<remarks>袁永福到此一游</remarks>
    public partial class WriterControl
    {
        /// <summary>
        /// 获得所有支持的命令名称组成的字符串，各个名称之间用逗号分开
        /// </summary>
        /// <returns>字符串列表</returns>
         
        public string GetCommandNameList()
        {
            return this.GetInnerViewControl().GetCommandNameList();
        }

        /// <summary>
        /// 执行命令
        /// </summary>
        /// <param name="commandName">命令文本</param>
        /// <param name="showUI">是否允许显示用户界面</param>
        /// <param name="parameter">用户参数</param>
        /// <returns>执行命令后的结果</returns>
        public object ExecuteCommand(
            string commandName,
            bool showUI,
            object parameter)
        {
            return this.GetInnerViewControl().ExecuteCommand(commandName, showUI, parameter);
        }

        /// <summary>
        /// 执行命令
        /// </summary>
        /// <param name="commandName">命令文本</param>
        /// <param name="args">参数</param>
        /// <returns>执行命令后的结果</returns>
         
        public object ExecuteCommandSpecifyArgs(
            string commandName,
            WriterCommandEventArgs args )
        {
            return this.GetInnerViewControl().ExecuteCommandSpecifyArgs(commandName, args);
        }

        /// <summary>
        /// 执行命令
        /// </summary>
        /// <param name="commandName">命令文本</param>
        /// <param name="showUI">是否允许显示用户界面</param>
        /// <param name="parameter">用户参数</param>
        /// <param name="raiseFromUI">用户界面操作而触发的命令</param>
        /// <returns>执行命令后的结果</returns>
        public object ExecuteCommandSpecifyRaiseFromUI(
            string commandName,
            bool showUI,
            object parameter,
            bool raiseFromUI )
        {
            return this.GetInnerViewControl().ExecuteCommandSpecifyRaiseFromUI(
                commandName, showUI, parameter, raiseFromUI);
        }
         
        /// <summary>
        /// 命令控制器对象
        /// </summary>
        public WriterCommandControler CommandControler
        {
            get
            {
                return this.myViewControl?.CommandControler;
            }
            set
            {
                this.myViewControl.CommandControler = value;
                if (value != null)
                {
                    value.EditControl = this;
                }
            }
        }
    }
}
