using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using DCSoft.Writer.Dom;
using DCSoft.Printing;
//using DCSoft.Writer.Printing;
using DCSoft.Writer.Data;
using DCSoft.Drawing;
using System.Web;
using System.IO;
using DCSoft.Writer.Controls;
using DCSoft.Common;

namespace DCSoft.Writer.Commands
{
    /// <summary>
    /// 数据相关的编辑器命令模块
    /// </summary>
    /// <remarks>编制 袁永福</remarks>
    internal sealed class WriterCommandModuleData : WriterCommandModule
    {
        /// <summary>
        /// 初始化对象
        /// </summary>
        public WriterCommandModuleData()
        { }

        // DCSoft.Writer.Commands.WriterCommandModuleData
        protected override WriterCommandList CreateCommands()
        {
            var list = new WriterCommandList();
            AddCommandToList(list, StandardCommandNames.ResetFormValue, this.ResetFormValue);
            //AddCommandToList(list, StandardCommandNames.UpdateDataSourceForView, this.UpdateDataSourceForView);
            //AddCommandToList(list, StandardCommandNames.UpdateViewForDataSource, this.UpdateViewForDataSource);
            return list;
        }

        /// <summary>
        /// 重置表单数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        [WriterCommandDescription( StandardCommandNames.ResetFormValue )]
        protected void ResetFormValue(object sender, WriterCommandEventArgs args)
        {
            if (args.Mode == WriterCommandEventMode.QueryState)
            {
                args.Enabled = args.Document != null;
            }
            else if (args.Mode == WriterCommandEventMode.Invoke)
            {
                if (args.Document.ResetFormValue())
                {
                    args.Document.Modified = true;
                    args.Document.UndoList.Clear();
                    args.EditorControl.RefreshDocumentExt(false, true);
                    args.Document.OnDocumentContentChanged();
                    args.Document.OnSelectionChanged();
                    args.RefreshLevel = UIStateRefreshLevel.All;
                }
                //bool modified = false;
                //XTextElementList elements = args.Document.GetElementsByType(typeof(XTextInputFieldElement));
                //if (elements != null && elements.Count > 0)
                //{
                //    foreach (XTextInputFieldElement field in elements)
                //    {
                //        field.InnerValue = null;
                //        if (field.Elements.Count > 0)
                //        {
                //            bool match = false;
                //            foreach (XTextElement e2 in field.Elements)
                //            {
                //                if (e2 is XTextInputFieldElement || e2 is XTextCheckBoxElementBase)
                //                {
                //                    match = true;
                //                    break;
                //                }
                //            }
                //            if (match == false)
                //            {
                //                field.Elements.Clear();
                //                modified = true;
                //            }
                //        }
                //    }
                //}
                //elements = args.Document.GetElementsByType(typeof(XTextCheckBoxElementBase));
                //foreach (XTextCheckBoxElementBase chk in elements)
                //{
                //    if (chk.Checked)
                //    {
                //        chk.Checked = false;
                //        modified = true;
                //    }
                //}
                //if (modified)
                //{
                //    args.Document.Modified = true;
                //    args.Document.UndoList.Clear();
                //    args.EditorControl.RefreshDocumentExt(false, true);
                //    args.Document.OnDocumentContentChanged();
                //    args.Document.OnSelectionChanged();
                //    args.RefreshLevel = UIStateRefreshLevel.All;
                //}
            }
        }
    }
}
