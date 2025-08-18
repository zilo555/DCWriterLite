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
//using DCSoft.Writer.Script;
using System.Runtime.InteropServices;
//using System.Collections.Specialized;

namespace DCSoft.Writer.Controls
{
    /// <summary>
    /// 编辑器命令相关功能模块
    /// </summary>
    ///<remarks>袁永福到此一游</remarks>
    public partial class WriterViewControl
    {
        /// <summary>
        /// 获得所有支持的命令名称组成的字符串，各个名称之间用逗号分开
        /// </summary>
        /// <returns>字符串列表</returns>
        public string GetCommandNameList()
        {
            StringBuilder str = new StringBuilder();
            foreach (WriterCommand cmd in this.CommandControler.CommandContainer.AllCommands())
            {
                if (str.Length > 0)
                {
                    str.Append(',');
                }
                str.Append(cmd.Name);
            }//foreach
            return str.ToString();
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
            object result =  this.CommandControler.ExecuteCommand(
                    commandName,
                    showUI,
                    parameter);
            if (this.DocumentControler != null)
            {
                this.DocumentControler.ClearSnapshot();
            }
            return result;
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
            object result = this.CommandControler.ExecuteCommandSpecifyRaiseFromUI(
                commandName,
                args.ShowUI ,
                args.Parameter ,
                args.RaiseFromUI );
            if (this.DocumentControler != null)
            {
                this.DocumentControler.ClearSnapshot();
            }
            return result;
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
            object result = this.CommandControler.ExecuteCommandSpecifyRaiseFromUI(
                commandName,
                showUI,
                parameter,
                raiseFromUI);
            this.DocumentControler.ClearSnapshot();
            return result;
        }
        internal void InvalidateCommandState(string commandName )
        {
            this.OwnerWriterControl.RaiseEventUpdateToolarState();
        }
        internal void InvalidateCommandState()
        {
            this.OwnerWriterControl.RaiseEventUpdateToolarState();
        }

        private List<WriterCommandControler> _CommandControllers = null;

        private WriterCommandControler _CommandControler = null;
        /// <summary>
        /// 命令控制器对象
        /// </summary>
        public WriterCommandControler CommandControler
        {
            get
            {
                if (this._CommandControler == null)
                {
                    this._CommandControler = new WriterCommandControler();
                }
                this._CommandControler.CommandContainer = this.AppHost.CommandContainer;
                this._CommandControler.EditControl = this.OwnerWriterControl;
                //_CommandControler.Document = this.Document;
                return _CommandControler;
            }
            set
            {
                if (this._CommandControler != value
                    && this._CommandControler != null
                    && value != null)
                {
                    if (this._CommandControllers == null)
                    {
                        this._CommandControllers = new List<WriterCommandControler>();
                    }
                    this._CommandControllers.Add(this._CommandControler);
                    this._CommandControllers.Add(value);
                }
                else if (this._CommandControler != null
                    && value == null
                    && this._CommandControllers != null
                    && this._CommandControllers.Contains(this._CommandControler))
                {
                    this._CommandControllers.Remove(this._CommandControler);
                }
                this._CommandControler = value;
                if (this._CommandControler != null)
                {
                    if (this._CommandControler.EditControl != null
                        && this._CommandControler.EditControl.GetInnerViewControl() != null)
                    {
                        this._CommandControler.EditControl.GetInnerViewControl()._CommandControler = null;
                    }
                    this._CommandControler = value;
                    this._CommandControler.CommandContainer = this.AppHost.CommandContainer;
                    this._CommandControler.EditControl = this.OwnerWriterControl;
                }
            }
        }
    }
}
