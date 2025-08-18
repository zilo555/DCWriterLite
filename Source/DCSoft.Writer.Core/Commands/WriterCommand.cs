using System;
using DCSoft.Writer.Controls ;

namespace DCSoft.Writer.Commands
{
    /// <summary>
    /// 编辑器动作基础类型
    /// </summary>
    public abstract class WriterCommand
    {
        /// <summary>
        /// 初始化对象
        /// </summary>
        protected WriterCommand()
        {
        }

        /// <summary>
        /// 动作名称
        /// </summary>
        public string Name = null;

        /// <summary>
        /// 是否启用快键键
        /// </summary>
        public bool EnableHotKey = true;


        /// <summary>
        /// 动作对象是否可用
        /// </summary>
        public bool Enable = true;

        /// <summary>
        /// 优先级，值越低则越优先
        /// </summary>
        public int Priority = 10;

        /// <summary>
        /// 快捷键
        /// </summary>
        public System.Windows.Forms.Keys ShortcutKey = System.Windows.Forms.Keys.None;


        /// <summary>
        /// 动作说明
        /// </summary>
        public string Description = null;

        /// <summary>
        /// 命令所属的模块对象
        /// </summary>
        public WriterCommandModule Module = null;

        /// <summary>
        /// 执行命令
        /// </summary>
        /// <param name="args">参数</param>
        public virtual void Execute(WriterCommandEventArgs args)
        {

        }
        /// <summary>
        /// 返回表示对象的字符串
        /// </summary>
        /// <returns>字符串</returns>
        public override string ToString()
        {
            return this.Name;
        }
    }
}