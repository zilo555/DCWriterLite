using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.Design;
//  
using System.Windows.Forms;
using System.Collections;
//using DCSoft.Writer.Commands.Design;
using DCSoft.Writer.Controls;
using DCSoft.Writer.Dom ;
using System.Reflection;
using DCSoft.WinForms;

namespace DCSoft.Writer.Commands
{
    /// <summary>
    /// 编辑器命令功能执行对象
    /// </summary>
    /// <remarks>本控件用于将用户界面控件的事件转换为对系统命令的调用，并提供设计时的支持。
    /// 本对象支持的用户界面控件有Button、TextBox、ComboBox、Menu、ToolStripItem、ToolStripButton、ToolStripTextBox、ToolStripComboBox、ToolSTripMenuItem等等
    /// 编写 袁永福</remarks>
    internal class WriterCommandExecuter : WriterCommandExecuterBase
    {
        public WriterCommandExecuter()
        {
        }
        /// <summary>
        /// 文本编辑器控件
        /// </summary>
        private WriterControl EditControl
        {
            get
            {
                return this.Controler.EditControl;
            }
        }

        /// <summary>
        /// 正在处理的文档对象
        /// </summary>
        private DomDocument Document
        {
            get
            {
                return this.Controler.Document;
            }
        }

        /// <summary>
        /// 命令容器对象
        /// </summary>
        private WriterCommandContainer CommandContainer
        {
            get
            {
                return this.Controler.CommandContainer;
            }
        }



        private object InnerExecuteCommand(
            WriterCommand cmd,
            bool showUI,
            object parameter,
            bool raiseFromUI)
        {
            var document = this.Document;
            var editorControl = this.EditControl;
            WriterCommandEventArgs cmdArgs = new WriterCommandEventArgs(
                editorControl,
                document,
                WriterCommandEventMode.QueryState,
                this.Controler);
            if (editorControl != null)
            {
                cmdArgs.Host = editorControl.AppHost;
            }
            else
            {
                cmdArgs.Host = WriterAppHost.Default;
            }
            cmdArgs.RaiseFromUI = raiseFromUI;
            cmdArgs.Name = cmd.Name;
            cmdArgs.ShowUI = showUI;
            cmdArgs.Parameter = parameter;
            cmdArgs.RaiseFromUI = raiseFromUI;
            cmdArgs.Mode = WriterCommandEventMode.QueryState;
            if (document != null)
            {
                document.CacheOptions();
            }
            cmd.Execute(cmdArgs);
            if (document != null)
            {
                document.ClearCachedOptions();
            }
            if (cmdArgs.Enabled)
            {
                cmdArgs.Mode = WriterCommandEventMode.Invoke;
                cmdArgs.Cancel = false;
                if (editorControl == null)
                {
                    try
                    {
                        if (document != null)
                        {
                            document.CacheOptions();
                        }
                        if (cmdArgs.Cancel == false)
                        {
                            cmd.Execute(cmdArgs);
                        }
                    }
                    finally
                    {
                        if (document != null)
                        {
                            document.ClearCachedOptions();
                        }
                        //_IsExecutingCommand = false;
                    }
                }
                else
                {
                    if (editorControl.DocumentOptions.BehaviorOptions.HandleCommandException)
                    {
                        // 处理命令过程中抛出的异常
                        try
                        {
                            if (cmdArgs.Cancel)
                            {
                                // 命令被取消了
                                return cmdArgs.Result;
                            }
                            if (cmdArgs.Cancel == false)
                            {
                                cmd.Execute(cmdArgs);
                            }
                        }
                        catch (Exception ext)
                        {
                            editorControl.OnCommandError(cmd, cmdArgs, ext);
                        }
                        finally
                        {
                            //_IsExecutingCommand = false;
                        }
                    }
                    else
                    {
                        // 不处理命令执行过程中抛出的异常
                        try
                        {
                            if (cmdArgs.Cancel)
                            {
                                // 命令被取消了
                                return cmdArgs.Result;
                            }
                            if (cmdArgs.Cancel == false)
                            {
                                cmd.Execute(cmdArgs);
                            }
                        }
                        finally
                        {
                            //_IsExecutingCommand = false;
                        }
                    }
                }
                if( editorControl != null )
                {
                    editorControl.RaiseEventUpdateToolarState();
                }
                return cmdArgs.Result;
            }//if
            else
            {
                    return cmdArgs.Result;
            }
        }

        /// <summary>
        /// 判断是否支持指定名称的功能命令
        /// </summary>
        /// <param name="commandName">功能命令名称</param>
        /// <returns>是否支持功能命令</returns>
        public override bool IsCommandSupported(string commandName)
        {
            WriterCommand cmd = this.CommandContainer.GetCommandRaw(commandName);
            return cmd != null;
        }

        /// <summary>
        /// 判断指定名称的命令是否可用
        /// </summary>
        /// <param name="commandName">命令名称</param>
        /// <returns>该命令是否可用</returns>
        public override bool IsCommandEnabled(string commandName)
        {
            WriterCommand cmd = this.CommandContainer.GetCommand(commandName);
            if (cmd != null)
            {
                WriterCommandEventArgs args = new WriterCommandEventArgs(
                    this.EditControl,
                    this.Document,
                    WriterCommandEventMode.QueryState,
                    this.Controler);
                args.ShowUI = true;
                cmd.Execute(args);
                return args.Enabled;
            }
            return false;
        }

        /// <summary>
        /// 判断指定名称的命令的状态是否处于选中状态
        /// </summary>
        /// <param name="commandName">命令名称</param>
        /// <returns>该命令是否处于选中状态</returns>
        public override bool IsCommandChecked(string commandName)
        {
            WriterCommand cmd = this.CommandContainer.GetCommand(commandName);
            if (cmd != null)
            {
                WriterCommandEventArgs args = new WriterCommandEventArgs(
                    this.EditControl,
                    this.Document,
                    WriterCommandEventMode.QueryState,
                    this.Controler);
                args.ShowUI = true;
                cmd.Execute(args);
                return args.Checked;
            }
            return false;
        }

        /// <summary>
        /// DCWriter内部使用。执行命令
        /// </summary>
        /// <param name="commandName">命令文本</param>
        /// <param name="showUI">是否允许显示用户界面</param>
        /// <param name="parameter">用户参数</param>
        /// <returns>执行操作后的返回值</returns>
        public override object InnerExecuteCommand(string commandName, bool showUI, object parameter)
        {
            WriterCommand cmd = this.CommandContainer.GetCommand(commandName);
            if( cmd != null && cmd.Enable)
            {
                object result = InnerExecuteCommand(cmd, showUI, parameter, false);
                return result;
            }
            return null;
        }

        /// <summary>
        /// 执行命令
        /// </summary>
        /// <param name="commandName">命令文本</param>
        /// <param name="showUI">是否允许显示用户界面</param>
        /// <param name="parameter">用户参数</param>
        /// <param name="raiseFromUI">命令是普通用户界面操作而触发的</param>
        /// <returns>执行操作后的返回值</returns>
        public override object ExecuteCommandSpecifyRaiseFromUI(
            string commandName,
            bool showUI,
            object parameter,
            bool raiseFromUI)
        {
            WriterCommand cmd = this.CommandContainer.GetCommand(commandName);
            if (cmd != null)
            {
                return InnerExecuteCommand(cmd, showUI, parameter, raiseFromUI);
            }
            return null;
        }

         
        /// <summary>
        /// 启动对象
        /// </summary>
        public override void Start()
        {
            var doc = this.Document;
            doc?.FixDomStateWithCheckInvalidateFlag();// .FixDomState();
           
        }
    }
}