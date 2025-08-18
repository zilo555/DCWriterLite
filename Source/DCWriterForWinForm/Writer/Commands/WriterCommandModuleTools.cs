using System;
using System.Collections.Generic;
using System.Text;
using DCSoft.Writer.Dom;
using DCSoft.Writer.Controls;
using System.Windows.Forms;
using DCSoft.Drawing;
using DCSoft.Writer.Undo;
 
using DCSoft.Common;
using DCSoft.WinForms.Design;
using DCSoft.Writer.Data;
using System.ComponentModel;
using DCSoft.WinForms;
using DCSoft.WinForms.Native;
//using DCSoft.Writer.Script;
using System.Reflection;
using System.IO;
using DCSystemXml;
using System.Collections;
namespace DCSoft.Writer.Commands
{
    /// <summary>
    /// 工具类型的编辑器命令容器
    /// </summary>
    /// <remarks>编制 袁永福</remarks>
    internal sealed class WriterCommandModuleTools : WriterCommandModule
    {
        /// <summary>
        /// 初始化对象
        /// </summary>
        public WriterCommandModuleTools()
        {
        }
        // DCSoft.Writer.Commands.WriterCommandModuleTools
        protected override WriterCommandList CreateCommands()
        {
            var list = new WriterCommandList();
            AddCommandToList(list, StandardCommandNames.ClearValueValidateResult, this.ClearValueValidateResult);
            AddCommandToList(list, StandardCommandNames.DocumentValueValidate, this.DocumentValueValidate);
            return list;
        }

        /// <summary>
        /// 清空数据校验结果
        /// </summary>
        /// <param name="sender">参数</param>
        /// <param name="args">参数</param>
        [WriterCommandDescription(StandardCommandNames.ClearValueValidateResult)]
        private void ClearValueValidateResult(object sender, WriterCommandEventArgs args)
        {
            if (args.Mode == WriterCommandEventMode.QueryState)
            {
                args.Enabled = args.Document != null
                    && args.Document.GetDocumentEditOptions().ValueValidateMode != DocumentValueValidateMode.None;
            }
            else if (args.Mode == WriterCommandEventMode.Invoke)
            {
                args.Result = args.Document.ClearValueValidateResult();
            }
        }

        /// <summary>
        /// 对文档数据进行校验
        /// </summary>
        /// <param name="sender">参数</param>
        /// <param name="args">参数</param>
        [WriterCommandDescription(StandardCommandNames.DocumentValueValidate)]
        private void DocumentValueValidate(object sender, WriterCommandEventArgs args)
        {
            if (args.Mode == WriterCommandEventMode.QueryState)
            {
                args.Enabled = args.Document != null
                    && args.Document.GetDocumentEditOptions().ValueValidateMode != DocumentValueValidateMode.None;
            }
            else if (args.Mode == WriterCommandEventMode.Invoke)
            {
                ValueValidateResultList result = args.Document.ValueValidate();
                args.Result = result;
                if (args.ShowUI)
                {
                    if (result != null && result.Count > 0)
                    {
                        if (result[0].Element != null)
                        {
                            result[0].Element.Focus();
                        }
                        DCSoft.WinForms.DCUIHelper.ShowWarringMessageBox(
                            args.EditorControl,
                            DCSR.ValueValidateFail
                            + Environment.NewLine
                            + result.ToString());
                        //MessageBox.Show(
                        //    WriterStrings.ValueValidateFail
                        //    + Environment.NewLine
                        //    + result.ToString());
                    }
                    else
                    {
                        DCSoft.WinForms.DCUIHelper.ShowMessageBox(
                            args.EditorControl,
                            DCSR.ValueValidateOK);
                        //MessageBox.Show(WriterStrings.ValueValidateOK);
                    }
                }
                // 定位到第一个出现错误的地方
                if (result != null && result.Count > 0)
                {
                    DomElement element = result[0].Element;
                    if (element != null)
                    {
                        element.Focus();
                        if (args.ShowUI)
                        {
                            args.EditorControl.Focus();
                        }
                    }
                }
            }
        }
    }
}
