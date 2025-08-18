using System;
using System.Collections.Generic;
using System.Text;

namespace DCSoft.Writer.Commands
{
    /// <summary>
    /// 基于委托的动态命令对象
    /// </summary>
    public class WriterCommandDelegate : WriterCommand
    {
        /// <summary>
        /// 初始化对象
        /// </summary>
        public WriterCommandDelegate()
        {
        }

        /// <summary>
        /// 初始化对象
        /// </summary>
        /// <param name="name">动作名称</param>
        /// <param name="handler">执行动作的委托对象</param>
        public WriterCommandDelegate(string name, WriterCommandEventHandler handler)
        {
            base.Name = name;
            this.Handler = handler;
        }

        /// <summary>
        /// 编辑器命令执行委托对象
        /// </summary>
        public WriterCommandEventHandler Handler = null;
        /// <summary>
        /// 执行编辑器命令
        /// </summary>
        /// <param name="args">参数</param>
        public override void Execute(DCSoft.Writer.Commands.WriterCommandEventArgs args)
        {
            if (Handler != null)
            {
                Handler( this , args);
            }
        }
    }
}
