using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.Design;
// // 
//using System.Drawing.Design;
using System.Windows.Forms;
using System.Collections;
//using DCSoft.Writer.Commands.Design;
using DCSoft.Writer.Controls;
using DCSoft.Writer.Dom ;
using System.Reflection;
using DCSoft.WinForms;
using DCSoft.Common;

namespace DCSoft.Writer.Commands
{
    /// <summary>
    /// 命令控制对象
    /// </summary>
    /// <remarks>本控件用于将用户界面控件的事件转换为对系统命令的调用，并提供设计时的支持。
    /// 本对象支持的用户界面控件有Button、TextBox、ComboBox、Menu、ToolStripItem、ToolStripButton、ToolStripTextBox、ToolStripComboBox、ToolSTripMenuItem等等
    /// 编写 袁永福</remarks>
    public class WriterCommandControler
    {
        /// <summary>
        /// 初始化对象
        /// </summary>
        public WriterCommandControler()
        {
        }
        private WriterCommandExecuter _Executer = null;

        private WriterCommandExecuter Executer
        {
            get
            {
                if (_Executer == null)
                {
                    _Executer =  new DCSoft.Writer.Commands.WriterCommandExecuter();
                }
                _Executer.Controler = this;
                return _Executer;
            }
        }



        private WriterControl _EditControl = null;
        /// <summary>
        /// 文本编辑器控件
        /// </summary>
        public WriterControl EditControl
        {
            get
            {
                return _EditControl;
            }
            set
            {
                _EditControl = value;
            }
        }

        /// <summary>
        /// 正在处理的文档对象
        /// </summary>
        public DomDocument Document
        {
            get
            {
                return this._EditControl?.Document;
            }
        }


        private WriterCommandContainer _CommandContainer = null;
        /// <summary>
        /// 命令容器对象
        /// </summary>
        public WriterCommandContainer CommandContainer
        {
            get
            {
                if (_CommandContainer == null)
                {
                    _CommandContainer = new WriterCommandContainer();
                }
                return _CommandContainer;
            }
            set
            {
                _CommandContainer = value;
            }
        }

        /// <summary>
        /// 获得指定名称的命令对象
        /// </summary>
        /// <param name="name">命令名称</param>
        /// <returns>获得的对象</returns>
        public WriterCommand GetCommand(string name)
        {
            return this.CommandContainer.GetCommandRaw(name);
        }

        /// <summary>
        /// 判断指定名称的命令是否可用
        /// </summary>
        /// <param name="commandName">命令名称</param>
        /// <returns>该命令是否可用</returns>
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public bool IsCommandEnabled(string commandName)
        {
            if (this.Executer != null)
            {
                return this.Executer.IsCommandEnabled(commandName);
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
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public object ExecuteCommand(string commandName, bool showUI, object parameter)
        {
            if (this.Executer != null)
            {
                return this.Executer.InnerExecuteCommand(commandName, showUI, parameter);
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
        public object ExecuteCommandSpecifyRaiseFromUI(
            string commandName,
            bool showUI,
            object parameter,
            bool raiseFromUI)
        {
            if (this.Executer != null)
            {
                return this.Executer.ExecuteCommandSpecifyRaiseFromUI(commandName, showUI, parameter, raiseFromUI);
            }
            return null;
        }


        /// <summary>
        /// 启动对象
        /// </summary>
        public void Start()
        {
            if (this._CommandContainer == null)
            {
                this._CommandContainer = WriterAppHost.Default.CommandContainer;
            }
            if (this.Executer != null)
            {
                this.Executer.Start();
            }
        }

    }
}