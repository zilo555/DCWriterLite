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
using DCSoft.Writer.Serialization;
using DCSoft.Common;
using System.Reflection;
using DCSoft.Writer.Controls;
//  
 
using System.ComponentModel;
using DCSoft.WinForms;
//using DCSoft.Writer.Html;

namespace DCSoft.Writer.Commands
{
    /// <summary>
    /// 文件功能模块
    /// </summary>
    /// <remarks>编制 袁永福</remarks>
    internal sealed class WriterCommandModuleFile : WriterCommandModule
    {
        /// <summary>
        /// 初始化对象
        /// </summary>
        public WriterCommandModuleFile()
        {
        }

        protected override WriterCommandList CreateCommands()
        {
            var list = new WriterCommandList();
            AddCommandToList(list, StandardCommandNames.DocumentDefaultFont, this.DocumentDefaultFont);
            AddCommandToList(list, StandardCommandNames.FileNew, this.FileNew);
            AddCommandToList(list, StandardCommandNames.FilePageSettings, this.FilePageSettings);
            return list;
        }

        [WriterCommandDescription(
            StandardCommandNames.FileNew,
            ReturnValueType = typeof(bool))]
        private void FileNew(object sender, WriterCommandEventArgs args)
        {
            if (args.Mode == WriterCommandEventMode.QueryState)
            {
                args.Enabled = args.EditorControl != null; 
            }
            else if (args.Mode == WriterCommandEventMode.Invoke)
            {
                args.Result = false;
                {
                    args.EditorControl.ClearContent();
                    args.EditorControl.OnDocumentLoad(new WriterEventArgs(args.EditorControl, args.Document, null));
                    //args.EditorControl.RefreshDocument();
                    args.Document.FileName = null;
                    args.Document.ContentSourceType = DocumentContentSourceTypes.NewFile;
                    args.Result = true;
                }
            }
        }


        /// <summary>
        /// 显示页面设置对话框
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        [WriterCommandDescription(StandardCommandNames.FilePageSettings,
            // ImageResource = "DCSoft.Writer.Commands.Images.CommandPageSettings.bmp",
            ReturnValueType = typeof(DCSoft.Printing.XPageSettings))]
        private void FilePageSettings(object sender, WriterCommandEventArgs args)
        {
            if (args.Mode == WriterCommandEventMode.QueryState)
            {
                args.Enabled = false;
                if (args.Document != null)
                {
                    args.Enabled = args.DocumentControler.EditorControlReadonly == false;
                }
            }
            else if (args.Mode == WriterCommandEventMode.Invoke)
            {
                XPageSettings ps = null;
                if (args.Parameter is XPageSettings)
                {
                    ps = (XPageSettings)args.Parameter;
                }
                else if (args.Parameter is string)
                {
                    //伍贻超修改：当参数为空时直接显示文档选项
                    if (String.IsNullOrEmpty((string)args.Parameter))
                    {
                        ps = args.Document.PageSettings.Clone();
                    }
                    //****************************************
                    else
                    {
                        ps = new XPageSettings();
                        AttributeString str = new AttributeString((string)args.Parameter);
                        str.ApplyToInstance(ps, false);
                    }
                }
                if (ps != null)
                {
                    if (args.Document.BeginLogUndo())
                    {
                        args.Document.UndoList.AddProperty("PageSettings",
                            args.Document.PageSettings,
                            ps,
                            args.Document);
                        args.Document.EndLogUndo();
                    }
                    args.Document.PageSettings = ps;
                    args.Document.UpdateContentVersion();
                    if (args.EditorControl != null)
                    {
                        args.EditorControl.RefreshDocument();
                        args.EditorControl.GetInnerViewControl().Invalidate();
                    }
                    args.Result = ps;
                }
            }
        }

        /// <summary>
        /// 设置文档的默认字体
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        [WriterCommandDescription(StandardCommandNames.DocumentDefaultFont)]
        private void DocumentDefaultFont(object sender, WriterCommandEventArgs args)
        {
            if (args.Mode == WriterCommandEventMode.QueryState)
            {
                args.Enabled = false;
                if (args.Document != null)
                {
                    if (args.EditorControl != null
                        && args.EditorControl.Readonly == false
                        && args.EditorControl.AutoSetDocumentDefaultFont == false)
                    {
                        args.Enabled = true;
                    }
                }
            }
            else if (args.Mode == WriterCommandEventMode.Invoke)
            {
                XFontValue font = new XFontValue();
                if (args.Parameter is XFontValue)
                {
                    font = (XFontValue)args.Parameter;
                }
                else if (args.Parameter is  Font)
                {
                    font = new XFontValue(( Font)args.Parameter);
                }
                else if (args.Parameter is string)
                {
                    font.Parse((string)args.Parameter);
                }
                else
                {
                    font = args.Document.DefaultStyle.Font;
                }
                Color c = args.Document.DefaultStyle.Color;
                if (args.Document.BeginLogUndo())
                {
                    DCSoft.Writer.Dom.Undo.XTextUndoSetDefaultFont undo =
                        new Dom.Undo.XTextUndoSetDefaultFont(
                            args.EditorControl,
                            args.Document.DefaultStyle.Font,
                            args.Document.DefaultStyle.Color,
                            font,
                            c);
                    args.Document.AddUndo(undo);
                    args.Document.EndLogUndo();
                }
                args.EditorControl.EditorSetDefaultFont(font, c, true);
            }
        }

    }
}
