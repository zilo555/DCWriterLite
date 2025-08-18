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

namespace DCSoft.Writer.Commands
{
    /// <summary>
    /// 命令控制对象
    /// </summary>
    /// <remarks>本控件用于将用户界面控件的事件转换为对系统命令的调用，并提供设计时的支持。
    /// 本对象支持的用户界面控件有Button、TextBox、ComboBox、Menu、ToolStripItem、ToolStripButton、
    /// ToolStripTextBox、ToolStripComboBox、ToolSTripMenuItem等等
    /// 编写 袁永福</remarks>
    public abstract class WriterCommandExecuterBase
    {
        /// <summary>
        /// 初始化对象
        /// </summary>
        protected WriterCommandExecuterBase()
        {

        }

        private WriterCommandControler _Controler = null;
        /// <summary>
        /// 命令控制器对象
        /// </summary>
        public WriterCommandControler Controler
        {
            get
            {
                return _Controler; 
            }
            set
            {
                _Controler = value; 
            }
        }
        /// <summary>
        /// 判断是否支持指定名称的功能命令
        /// </summary>
        /// <param name="commandName">功能命令名称</param>
        /// <returns>是否支持功能命令</returns>
        public abstract bool IsCommandSupported(string commandName);

        /// <summary>
        /// 判断指定名称的命令是否可用
        /// </summary>
        /// <param name="commandName">命令名称</param>
        /// <returns>该命令是否可用</returns>
        public abstract bool IsCommandEnabled(string commandName);

        /// <summary>
        /// 判断指定名称的命令的状态是否处于选中状态
        /// </summary>
        /// <param name="commandName">命令名称</param>
        /// <returns>该命令是否处于选中状态</returns>
        public abstract bool IsCommandChecked(string commandName);

        /// <summary>
        /// DCWriter内部使用。执行命令
        /// </summary>
        /// <param name="commandName">命令文本</param>
        /// <param name="showUI">是否允许显示用户界面</param>
        /// <param name="parameter">用户参数</param>
        /// <returns>执行操作后的返回值</returns>
        public abstract object InnerExecuteCommand(string commandName, bool showUI, object parameter);

        /// <summary>
        /// 执行命令
        /// </summary>
        /// <param name="commandName">命令文本</param>
        /// <param name="showUI">是否允许显示用户界面</param>
        /// <param name="parameter">用户参数</param>
        /// <param name="raiseFromUI">命令是普通用户界面操作而触发的</param>
        /// <returns>执行操作后的返回值</returns>
        public abstract object ExecuteCommandSpecifyRaiseFromUI(
            string commandName,
            bool showUI,
            object parameter,
            bool raiseFromUI);


        /// <summary>
        /// 启动对象
        /// </summary>
        public abstract void Start();



    }
}