using System;
using System.Collections.Generic;
using System.Text;
using DCSoft.Writer.Dom;
using DCSoft.Writer.Controls;
// // 
using DCSoft.Common;
using DCSoft.Drawing;
using DCSoft.Printing;
using System.Runtime.CompilerServices;

namespace DCSoft.Writer.Commands
{
    /// <summary>
    /// 功能模块对象
    /// </summary>
    public abstract class WriterCommandModule : IDisposable 
    {
        /// <summary>
        /// 初始化对象
        /// </summary>
        protected WriterCommandModule()
        {
        }

        private bool _Enabled = true;
        /// <summary>
        /// 模块是否可用
        /// </summary>
        public bool Enabled
        {
            get
            {
                return _Enabled; 
            }
            set
            {
                _Enabled = value; 
            }
        }

        ///// <summary>
        ///// 启动模块
        ///// </summary>
        ///// <param name="args">事件参数</param>
        ///// <returns>操作是否成功</returns>
        private WriterCommandList _Commands = null;
        /// <summary>
        /// 本模块包含的动作对象列表
        /// </summary>
        public virtual WriterCommandList Commands()
        {
            if (_Commands == null)
            {
                _Commands = CreateCommands();
            }
            return _Commands;
        }
        protected static void AddCommandToList( WriterCommandList list , string name, WriterCommandEventHandler handler, System.Windows.Forms.Keys hotKey = System.Windows.Forms.Keys.None)
        {
            //Console.WriteLine("public const string "+ name +" = \"" + name +"\";");
            var cmd = new WriterCommandDelegate();
            cmd.Name = name;
            cmd.ShortcutKey = hotKey;
            cmd.Handler = handler;
            list.Add(cmd);
        }

        /// <summary>
        /// 销毁对象
        /// </summary>
        //[System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        ////[DCPublishAPI]
        public virtual void Dispose()
        {
        }
        /// <summary>
        /// 创建编辑器命令列表
        /// </summary>
        /// <returns>创建的列表</returns>
        protected virtual WriterCommandList CreateCommands()
        {
            throw new NotSupportedException();
            
        }

    }

    /// <summary>
    /// 功能模块列表
    /// </summary>
    public class WriterActionModuleList : System.Collections.IEnumerable
    {
        private static readonly WriterActionModuleList _Default = new WriterActionModuleList();
        /// <summary>
        /// 默认列表对象
        /// </summary>
        public static WriterActionModuleList Default
        {
            get
            {
                return _Default;
            }
        }

        private readonly List<WriterCommandModule> _Modules
            = new List<WriterCommandModule>();
        /// <summary>
        /// 添加编辑器命令模块
        /// </summary>
        /// <param name="mdl">模块对象</param>
        public void AddModule(WriterCommandModule mdl)
        {
            if (mdl == null)
            {
                throw new ArgumentNullException("mdl");
            }
            foreach (WriterCommandModule item in this)
            {
                if( item.GetType().Equals( mdl.GetType()))
                {
                    return ;
                }
            }
            this._Modules.Add( mdl );
            this._Commands = null;
        }

        private Dictionary<string, WriterCommand> _Commands = null;
        /// <summary>
        /// 不区分大小写的获得指定名称的命令对象
        /// </summary>
        /// <param name="commandName">命令名称</param>
        /// <returns>获得的命令对象</returns>
        internal WriterCommand GetCommandIgnorecase(string commandName)
        {
            if (_Commands == null)
            {
                _Commands = new Dictionary<string, WriterCommand>();
                foreach (WriterCommandModule module in this)
                {
                    foreach (WriterCommand cmd in module.Commands())
                    {
                        string name = cmd.Name;
                        if (string.IsNullOrEmpty(name) == false)
                        {
                            _Commands[name.ToLower()] = cmd;
                        }
                    }
                }
            }
            if (string.IsNullOrEmpty(commandName) == false)
            {
                WriterCommand cmd = null;
                if (_Commands.TryGetValue(commandName.ToLower(), out cmd))
                {
                    return cmd;
                }
            }
            return null;
        }

        public System.Collections.IEnumerator GetEnumerator()
        {
            return this._Modules.GetEnumerator();
        }
    }
}
