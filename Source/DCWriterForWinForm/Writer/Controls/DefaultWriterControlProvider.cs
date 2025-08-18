using System;
using System.Collections.Generic;
using System.Text;
using DCSoft.Writer.Dom;
using DCSoft.Writer.Data;
using DCSoft.Writer.Commands;
using System.Windows.Forms;
using DCSoft.WinForms;
using DCSoft.Common;
using DCSoft.Drawing;
using System.Reflection;
using System.ComponentModel;

namespace DCSoft.Writer.Controls
{
    /// <summary>
    /// 默认的编辑器控件提供者
    /// </summary>
    internal partial class DefaultWriterControlProvider 
    {
        /// <summary>
        /// 初始化对象
        /// </summary>
        /// <param name="ctl"></param>
        public DefaultWriterControlProvider()
        {
        }
        /// <summary>
        /// 销毁对象
        /// </summary>
        public virtual void Dispose()
        {
            this._Control = null;
        }

        public WriterControl Control
        {
            get
            {
                return this._Control;
            }
            set
            {
                this._Control = value;
            }
        }
        /// <summary>
        /// 编辑器控件对象
        /// </summary>
        protected WriterControl _Control = null;

        /// <summary>
        /// 文档对象
        /// </summary>
        protected DomDocument Document
        {
            get
            {
                return this._Control.Document;
            }
        }

        private void WASMRaiseEvent(string eventName, object p1)
        {
            this._Control.WASMRaiseEvent(eventName, p1);
        }



        /// <summary>
        /// 触发文档内容发生改变事件
        /// </summary>
        /// <param name="args">事件参数</param>
        public  void OnDocumentContentChanged(EventArgs args)
        {
            this.WASMRaiseEvent(WriterControl.EVENTNAME_DocumentContentChanged, args);
        }



        /// <summary>
        /// DCWriter内部使用。触发自定义的错误事件
        /// </summary>
        /// <param name="cmd">编辑器命令对象</param>
        /// <param name="cmdArgs">参数</param>
        /// <param name="exp">异常对象</param>
        public  void OnCommandError(
            WriterCommand cmd,
            WriterCommandEventArgs cmdArgs,
            Exception exp)
        {
            if( DomDocument._DebugMode)
            {
                DCConsole.Default.WriteLineError( cmd.Name + "$$"  + exp.ToString());
            }
            {
                CommandErrorEventArgs args = new CommandErrorEventArgs();
                args.WriterControl = this._Control;
                args.CommandName = cmd.Name;
                args.CommmandParameter = cmdArgs.Parameter;
                args.Document = args.Document;
                args.Exception = exp;
                this.WASMRaiseEvent(WriterControl.EVENTNAME_CommandError, args);
                if (args.Handled)
                {
                    // 用户处理了事件，无需后续处理。
                    return;
                }
            }
            string msg = cmd.Name;
            if (exp != null)
            {
                msg = msg + ":" + exp.Message;
            }
            DCConsole.Default.WriteLineError(exp.ToString());
        }
    }
}