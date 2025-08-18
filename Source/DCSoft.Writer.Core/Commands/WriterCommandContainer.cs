using System;
using System.Collections.Generic;
using System.Text;
using DCSoft.WinForms;
using System.Windows.Forms;
using DCSoft.Writer.Dom;
using DCSoft.Writer.Controls;

namespace DCSoft.Writer.Commands
{
    /// <summary>
    /// 编辑器命令容器对象
    /// </summary>
    public class WriterCommandContainer
    {

        /// <summary>
        /// 命令容器对象
        /// </summary>
        public WriterCommandContainer()
        {
        }

        /// <summary>
        /// 模块列表
        /// </summary>
        public WriterActionModuleList Modules
        {
            get
            {
                return WriterActionModuleList.Default;
            }
        }
        private readonly WriterCommandList _Commands = new WriterCommandList();
        /// <summary>
        /// 动作列表
        /// </summary>
        public WriterCommandList Commands()
        {

            return _Commands;

        }

        /// <summary>
        /// 容器中所有的命令组成的列表，该列表是动态生成的对象实例.
        /// </summary>
        /// <remarks>
        /// 该列表是动态生成的对象实例，也就是说即使是调用两次本属性获得的值其引用是不一样的。
        /// </remarks>
        public WriterCommandList AllCommands()
        {
            WriterCommandList list = new WriterCommandList();
            if (this.Modules != null)
            {
                foreach (WriterCommandModule mdl in this.Modules)
                {
                    foreach (WriterCommand cmd in mdl.Commands())
                    {
                        list.AddCommand(cmd);
                    }
                }
            }
            if (this._Commands != null && this._Commands.Count > 0)
            {
                foreach (WriterCommand cmd in this._Commands)
                {
                    list.AddCommand(cmd);
                }
            }
            list.Sort(new CommandNameComparer());
            return list;
        }

        private class CommandNameComparer : IComparer<WriterCommand>
        {
            public int Compare(WriterCommand x, WriterCommand y)
            {
                return string.Compare(x.Name, y.Name, true);
            }
        }

        /// <summary>
        /// 获得指定名称的动作对象
        /// </summary>
        /// <param name="commandName"></param>
        /// <returns></returns>
        public WriterCommand GetCommandRaw(string commandName)
        {
            if (string.IsNullOrEmpty(commandName))
            {
                return null;
            }
            foreach (WriterCommand act in this.Commands())
            {
                if (string.Compare(act.Name, commandName, true) == 0)
                {
                    return act;
                }
            }
            foreach (WriterCommandModule module in this.Modules)
            {
                foreach (WriterCommand act in module.Commands())
                {
                    if (string.Compare(act.Name, commandName, true) == 0)
                    {
                        return act;
                    }
                }
            }
            return null;
        }


        /// <summary>
        /// 获得指定名称的动作对象
        /// </summary>
        /// <param name="commandName"></param>
        /// <returns></returns>
        public WriterCommand GetCommand(string commandName)
        {
            if (string.IsNullOrEmpty(commandName))
            {
                return null;
            }
            WriterCommand result = null;
            foreach (WriterCommand cmd in this.Commands())
            {
                if (string.Compare(cmd.Name, commandName, true) == 0)
                {
                    //if (cmd.Enable)
                    {
                        if (result == null || result.Priority > cmd.Priority)
                        {
                            result = cmd;
                        }
                    }
                }
            }
            WriterCommand cmd2 = this.Modules.GetCommandIgnorecase(commandName);
            if (result == null || result.Priority > cmd2.Priority)
            {
                result = cmd2;
            }
            return result;
        }

        /// <summary>
        /// 根据键盘事件来获得被激活的动作对象
        /// </summary>
        /// <param name="editControl"></param>
        /// <param name="document"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public WriterCommand Active(
            WriterControl editControl,
            DomDocument document,
            System.Windows.Forms.KeyEventArgs args)
        {
            WriterCommandEventArgs e = new WriterCommandEventArgs(
                editControl,
                document,
                WriterCommandEventMode.QueryActive,
                null);
            e.AltKey = args.Alt;
            e.ShiftKey = args.Shift;
            e.CtlKey = args.Control;
            e.KeyCode = args.KeyCode;
            var cmds2 = this.Commands();
            foreach (WriterCommand cmd in cmds2)
            {
                if (cmd.EnableHotKey == false)
                {
                    // 命令禁止快键键
                    continue;
                }
                e.Parameter = null;
                if (cmd.ShortcutKey != System.Windows.Forms.Keys.None)
                {
                    // 检查快捷键
                    KeyState sk = new KeyState(cmd.ShortcutKey);
                    if (sk.Alt == args.Alt
                        && sk.Shift == args.Shift
                        && sk.Control == args.Control
                        && sk.Key == args.KeyCode)
                    {
                        e.Enabled = true;
                        e.Actived = true;
                        cmd.Execute(e);
                        if (e.Actived && e.Enabled)
                        {
                            return cmd;
                        }
                    }
                }
                e.Actived = false;
                cmd.Execute(e);
                if (e.Enabled && e.Actived)
                {
                    return cmd;
                }
            }
            foreach (WriterCommandModule module in this.Modules)
            {
                var cmds = module.Commands();
                for (int iCount = 0; iCount < cmds.Count; iCount++)
                {
                    var act = cmds[iCount];
                    if (act.EnableHotKey == false)
                    {
                        // 命令禁止快键键
                        continue;
                    }
                    e.Parameter = null;
                    if (act.ShortcutKey != System.Windows.Forms.Keys.None)
                    {
                        // 检查快捷键
                        KeyState sk = new KeyState(act.ShortcutKey);
                        if (sk.Alt == args.Alt
                            && sk.Shift == args.Shift
                            && sk.Control == args.Control
                            && sk.Key == args.KeyCode)
                        {
                            e.Enabled = true;
                            e.Actived = true;
                            act.Execute(e);
                            if (e.Enabled && e.Actived)
                            {
                                return act;
                            }
                        }
                    }
                    e.Actived = false;
                    e.Enabled = true;
                    act.Execute(e);
                    if (e.Enabled && e.Actived)
                    {
                        return act;
                    }
                }
            }

            return null;
        }
    }
}
