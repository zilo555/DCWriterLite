using System;
using System.Collections.Generic;
using System.Text;
using DCSoft.Writer.Commands;
using DCSoft.Writer.Dom;
using DCSoft.Writer.Data;
using DCSoft.Writer.Controls;
using DCSoft.Printing;
using System.Windows.Forms;

namespace DCSoft.Writer.Extension
{
    /// <summary>
    /// 扩展的编辑器命令模块对象
    /// </summary>
    /// <remarks>编制 袁永福</remarks>
    internal sealed class WriterCommandModuleExtension
        : DCSoft.Writer.Commands.WriterCommandModule
    {
        /// <summary>
        /// 初始化对象
        /// </summary>
        public WriterCommandModuleExtension()
        {
        }
        protected override WriterCommandList CreateCommands()
        {
            var list = new WriterCommandList();
            AddCommandToList(list, StandardCommandNames.InsertDateTimeField, this.InsertDateTimeField);
            AddCommandToList(list, StandardCommandNames.InsertDateTimeString, this.InsertDateTimeString);
            AddCommandToList(list, StandardCommandNames.InsertListField, this.InsertListField);
            return list;
        }

        /// <summary>
        /// 插入日期时间输入域
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        [WriterCommandDescription(
            "InsertDateTimeField")]
        private void InsertDateTimeField(object sender, DCSoft.Writer.Commands.WriterCommandEventArgs args)
        {
            if (args.Mode == WriterCommandEventMode.QueryState)
            {
                args.Enabled = args.CommandControler != null && args.CommandControler.IsCommandEnabled("InsertInputField");
            }
            else if (args.Mode == WriterCommandEventMode.Invoke)
            {
                DomInputFieldElement field = null;

                if (args.Parameter is DomInputFieldElement)
                {
                    field = args.Parameter as DomInputFieldElement;
                }
                else
                {
                    field = new Dom.DomInputFieldElement();
                    DCSoft.Writer.Dom.InputFieldSettings inputfield = new InputFieldSettings();
                    inputfield.EditStyle = InputFieldEditStyle.DateTime;
                    field.FieldSettings = inputfield;
                }
                args.Document.AllocElementID("datetime", field);
                if (field != null)
                {
                    args.EditorControl.ExecuteCommand(
                        StandardCommandNames.InsertInputField,
                        false,
                        field);
                }
            }
        }

        /// <summary>
        /// 插入列表输入域
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        [WriterCommandDescription("InsertListField")]
        private void InsertListField(object sender, DCSoft.Writer.Commands.WriterCommandEventArgs args)
        {
            if (args.Mode == WriterCommandEventMode.QueryState)
            {
                args.Enabled = args.CommandControler != null && args.CommandControler.IsCommandEnabled("InsertInputField");
            }
            else if (args.Mode == WriterCommandEventMode.Invoke)
            {
                DCSoft.Writer.Dom.DomInputFieldElement field = null;


                if (args.Parameter is DCSoft.Writer.Dom.DomInputFieldElement)
                {
                    field = args.Parameter as DCSoft.Writer.Dom.DomInputFieldElement;
                }
                else
                {
                    field = new Dom.DomInputFieldElement();
                    DCSoft.Writer.Dom.InputFieldSettings inputFieldSetting = new DCSoft.Writer.Dom.InputFieldSettings();
                    inputFieldSetting.EditStyle = DCSoft.Writer.Dom.InputFieldEditStyle.DropdownList;
                    field.FieldSettings = inputFieldSetting;
                }
                args.Document.AllocElementID("lst", field);
                if (field != null)
                {
                    args.EditorControl.ExecuteCommand(
                        StandardCommandNames.InsertInputField,
                        false,
                        field);
                }
            }
        }

        /// <summary>
        /// 插入时间日期字符串
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        [WriterCommandDescription(
            // 设置编辑器命令的名称
            "InsertDateTimeString")]
        public void InsertDateTimeString(
            object sender,
            WriterCommandEventArgs args)
        {
            if (args.Mode == WriterCommandEventMode.QueryState)
            {
                // 判断动作是否可用
                args.Enabled = args.CommandControler != null && args.CommandControler.IsCommandEnabled("InsertString");
            }
            else if (args.Mode == WriterCommandEventMode.Invoke)
            {
                // 默认插入当前日期的字符串
                string text = args.Document.GetNowDateTime().ToString();
                if (string.IsNullOrEmpty(text) == false)
                {
                    // 调用标准命令插入当前日期
                    args.EditorControl.ExecuteCommand(StandardCommandNames.InsertString, false, text);
                }
            }
        }

    }
}