using System;
using System.Collections.Generic;
using System.Text;
using DCSoft.Writer.Dom;
using DCSoft.Writer.Controls;
using System.Windows.Forms;
using DCSoft.Drawing;
using DCSoft.Writer.Undo;
//  
using DCSoft.Common;
using DCSoft.WinForms.Design;
using DCSoft.Writer.Data;
using System.ComponentModel;
//using DCSoft.Writer.Script;
using DCSoft.WASM;

namespace DCSoft.Writer.Commands
{
    /// <summary>
    /// 编辑文档内容的功能模块
    /// </summary>
    /// <remarks>编制 袁永福</remarks>
    internal sealed class WriterCommandModuleEdit : WriterCommandModule
    {
        /// <summary>
        /// 初始化对象
        /// </summary>
        public WriterCommandModuleEdit()
        {
        }

        // DCSoft.Writer.Commands.WriterCommandModuleEdit
        protected override WriterCommandList CreateCommands()
        {
            var list = new WriterCommandList();
            AddCommandToList(list, StandardCommandNames.Backspace, this.Backspace, System.Windows.Forms.Keys.Back);
            AddCommandToList(list, StandardCommandNames.ClearAllFieldValue, this.ClearAllFieldValue);
            AddCommandToList(list, StandardCommandNames.ClearBodyContent, this.ClearBodyContent);
            AddCommandToList(list, StandardCommandNames.ClearCurrentFieldValue, this.ClearCurrentFieldValue);
            AddCommandToList(list, StandardCommandNames.ClearUndo, this.ClearUndo);
            //AddCommandToList(list, StandardCommandNames.Copy, this.Copy);
            //AddCommandToList(list, StandardCommandNames.CopyAsText, this.CopyAsText);
            //AddCommandToList(list, StandardCommandNames.CopyWithClearFieldValue, this.CopyWithClearFieldValue);
            //AddCommandToList(list, StandardCommandNames.Cut, this.Cut);
            AddCommandToList(list, StandardCommandNames.Delete, this.Delete, System.Windows.Forms.Keys.Delete);
            AddCommandToList(list, StandardCommandNames.DeleteField, this.DeleteField);
            AddCommandToList(list, StandardCommandNames.DeleteLine, this.DeleteLine);
            AddCommandToList(list, StandardCommandNames.ExecuteElementDefaultMethod, this.ExecuteElementDefaultMethod, System.Windows.Forms.Keys.F2);
            AddCommandToList(list, StandardCommandNames.HeaderBottomLineVisible, this.HeaderBottomLineVisible);
            AddCommandToList(list, StandardCommandNames.InsertMode, this.InsertMode, System.Windows.Forms.Keys.Insert);
            //AddCommandToList(list, StandardCommandNames.Paste, this.Paste);
            //AddCommandToList(list, StandardCommandNames.PasteAsText, this.PasteAsText);
            AddCommandToList(list, StandardCommandNames.Redo, this.Redo, System.Windows.Forms.Keys.Y | System.Windows.Forms.Keys.Control);
            AddCommandToList(list, StandardCommandNames.Search, this.Search);
            AddCommandToList(list, StandardCommandNames.SearchReplace, this.SearchReplace, System.Windows.Forms.Keys.F | System.Windows.Forms.Keys.Control);
            AddCommandToList(list, StandardCommandNames.SetDefaultStyle, this.SetDefaultStyle);
            //AddCommandToList(list, StandardCommandNames.SpecifyPaste, this.SpecifyPaste);
            //AddCommandToList(list, StandardCommandNames.SwapTransparentEncrypt, this.SwapTransparentEncrypt);
            AddCommandToList(list, StandardCommandNames.Undo, this.Undo, System.Windows.Forms.Keys.Z | System.Windows.Forms.Keys.Control);
            return list;
        }


        /// <summary>
        /// 设置是否显示页眉下边缘的横线
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        [WriterCommandDescription(
            StandardCommandNames.HeaderBottomLineVisible,
            ReturnValueType = typeof(bool),
            DefaultResultValue = true)]
        private void HeaderBottomLineVisible(object sender, WriterCommandEventArgs args)
        {
            if (args.Mode == WriterCommandEventMode.QueryState)
            {
                args.Enabled = args.Document != null;
                if (args.Document != null)
                {
                    args.Checked = args.Document.Info.ShowHeaderBottomLine != DCBooleanValue.False;
                }
            }
            else if (args.Mode == WriterCommandEventMode.Invoke)
            {
                DCBooleanValue v = DCBooleanValue.True;
                if (args.Document.Info.ShowHeaderBottomLine == DCBooleanValue.True)
                {
                    v = DCBooleanValue.False;
                }
                else
                {
                    v = DCBooleanValue.True;
                }
                if (args.Parameter is bool)
                {
                    if ((bool)args.Parameter)
                    {
                        v = DCBooleanValue.True;
                    }
                    else
                    {
                        v = DCBooleanValue.False;
                    }
                }
                else if (args.Parameter is DCBooleanValue)
                {
                    v = (DCBooleanValue)args.Parameter;
                }
                if (args.Document.Info.ShowHeaderBottomLine != v)
                {
                    if (args.Document.BeginLogUndo())
                    {
                        args.Document.UndoList.AddProperty("ShowHeaderBottomLine", args.Document.Info.ShowHeaderBottomLine, v, args.Document.Info);
                        args.Document.EndLogUndo();
                    }
                    args.Document.Info.ShowHeaderBottomLine = v;
                    args.Document.Modified = true;
                    args.Document.UpdateContentVersion();
                    args.Document.OnDocumentContentChanged();
                    args.RefreshLevel = UIStateRefreshLevel.Current;
                    if (args.EditorControl != null)
                    {
                        args.EditorControl.GetInnerViewControl().Invalidate();
                    }
                }
            }
        }
        /// <summary>
        /// 清空文档正文内容
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        [WriterCommandDescription( StandardCommandNames.ClearBodyContent )]
        private void ClearBodyContent(object sender, WriterCommandEventArgs args)
        {
            if (args.Mode == WriterCommandEventMode.QueryState)
            {
                args.Enabled = args.EditorControl != null && args.EditorControl.Readonly == false;
            }
            else if (args.Mode == WriterCommandEventMode.Invoke)
            {
                args.Result = false;
                if (args.EditorControl == null || args.EditorControl.Readonly)
                {
                    return;
                }
                args.Document.UndoList.Clear();
                args.Document.Body.Elements.Clear();
                args.EditorControl.RefreshDocument();
                args.Document.Modified = true;
                args.Document.OnDocumentContentChanged();
                args.Document.OnSelectionChanged();
                args.RefreshLevel = UIStateRefreshLevel.All;
                args.Result = true;
            }
        }
        /// <summary>
        /// 执行退格删除操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        [WriterCommandDescription(
            StandardCommandNames.Backspace,
            ShortcutKey = Keys.Back,
            ReturnValueType = typeof(bool))]
        private void Backspace(object sender, WriterCommandEventArgs args)
        {
            if (args.Mode == WriterCommandEventMode.QueryState)
            {
                // 获得动作状态
                args.Enabled = args.EditorControl != null;
            }
            else if (args.Mode == WriterCommandEventMode.Invoke)
            {
                args.Result = false;
                // 执行动作
                if (args.Document.Selection.Mode == ContentRangeMode.Cell)
                {
                    // 处于选择单元格的状态
                    List<DomTableRowElement> handledRows = new List<DomTableRowElement>();
                    List<DomTableColumnElement> handledCols = new List<DomTableColumnElement>();
                    DomElementList cells = args.Document.Selection.Cells;
                    bool fullRowSelect = true;
                    bool fullColSelect = true;
                    DomTableElement table = ( ( DomTableCellElement ) cells[0]).OwnerTable ;
                    for (int iCount = 0; iCount < cells.Count; iCount++)
                    {
                        DomTableCellElement cell = (DomTableCellElement)cells[iCount];
                        if( cell.OwnerTable != table )
                        {
                            // 出现跨表格选择单元格，一切都是浮云。
                            fullRowSelect = false ;
                            fullColSelect = false ;
                            break;
                        }
                        if (cell.OwnerColumn != null && handledCols.Contains(cell.OwnerColumn) == false)
                        {
                            handledCols.Add(cell.OwnerColumn);
                            DomTableColumnElement col = cell.OwnerColumn;
                            int cellCounts = 0;
                            for (int iCount2 = iCount; iCount2 < cells.Count; iCount2++)
                            {
                                DomTableCellElement cell2 = (DomTableCellElement)cells[iCount2];
                                if (cell2.OwnerColumn == col)
                                {
                                    cellCounts++;
                                    if (cellCounts == table.Rows.Count)
                                    {
                                        break;
                                    }
                                }
                            }
                            if (cellCounts != table.Rows.Count)
                            {
                                // 出现某个表格列没有被全部选中的现象
                                fullColSelect = false;
                            }
                        }
                        if (cell.OwnerRow != null && handledRows.Contains(cell.OwnerRow) == false)
                        {
                            handledRows.Add(cell.OwnerRow);
                            DomTableRowElement row = cell.OwnerRow;
                            int cellCounts = 0;
                            for (int iCount2 = iCount; iCount2 < cells.Count; iCount2++)
                            {
                                if (cells[iCount2].Parent == row)
                                {
                                    cellCounts++;
                                    if (cellCounts == row.Cells.Count)
                                    {
                                        break;
                                    }
                                }
                            }
                            if (cellCounts != row.Cells.Count)
                            {
                                //  出现某个表格行的单元格没有被全部选中的现象
                                fullRowSelect = false;
                            }
                        }
                        if (fullRowSelect == false && fullColSelect == false)
                        {
                            // 确定没有出现整体的选中表格行及表格列的现象，无需判断，退出循环
                            break;
                        }
                    }//foreach
                    if (fullRowSelect)
                    {
                        // 当全选表格行时执行删除表格行操作
                        args.Result = args.EditorControl.ExecuteCommandSpecifyArgs(
                            StandardCommandNames.Table_DeleteRow,
                            args );
                        args.RefreshLevel = UIStateRefreshLevel.All;
                    }
                    else if (fullColSelect)
                    {
                        // 当全选表格列时执行删除表格列操作
                        args.Result = args.EditorControl.ExecuteCommandSpecifyArgs(
                            StandardCommandNames.Table_DeleteColumn,
                            args );
                        args.RefreshLevel = UIStateRefreshLevel.All;
                    }
                    else
                    {
                        if ( args.ShowUI)
                        {
                            // 触发JS函数执行可选择的删除表格行或者表格列的操作。
                            JavaScriptMethods.UI_QueryDeleteRowOfColumns(args.EditorControl.WASMClientID);
                        }
                        else
                        {
                            // 默认删除表格行
                            args.Result = args.EditorControl.ExecuteCommandSpecifyArgs(
                                            StandardCommandNames.Table_DeleteRow,
                                            args );
                            args.RefreshLevel = UIStateRefreshLevel.All;
                        }
                    }
                }
                else
                {
                    
                    if(args.EditorControl.DocumentControler.Backspace(args.ShowUI))
                    {
                        args.Result = true;
                        args.RefreshLevel = UIStateRefreshLevel.All;
                    }
                }
            }
        }

        ///// <summary>
        ///// 以文本方式复制内容，忽略其他所有设置
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="args"></param>
        //[WriterCommandDescription(
        //    StandardCommandNames.CopyWithClearFieldValue,
        //    // ImageResource = "DCSoft.Writer.Commands.Images.CommandCopy.bmp",
        //    ReturnValueType = typeof(bool))]
        //private void CopyWithClearFieldValue(object sender, WriterCommandEventArgs args)
        //{
        //    if (args.Mode == WriterCommandEventMode.QueryState)
        //    {
        //        args.Enabled = (args.DocumentControler != null
        //            && args.DocumentControler.CanCopy);
        //    }
        //    else if (args.Mode == WriterCommandEventMode.Invoke)
        //    {
        //        args.Result = args.DocumentControler.CopyWithClearFieldValue();
        //        args.RefreshLevel = UIStateRefreshLevel.All;
        //    }
        //}

        ///// <summary>
        ///// 以文本方式复制内容，忽略其他所有设置
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="args"></param>
        //[WriterCommandDescription(
        //    StandardCommandNames.CopyAsText,
        //    // ImageResource = "DCSoft.Writer.Commands.Images.CommandCopy.bmp",
        //    ReturnValueType = typeof(bool))]
        //private void CopyAsText(object sender, WriterCommandEventArgs args)
        //{
        //    if (args.Mode == WriterCommandEventMode.QueryState)
        //    {
        //        args.Enabled = (args.DocumentControler != null
        //            && args.DocumentControler.CanCopy);
        //    }
        //    else if (args.Mode == WriterCommandEventMode.Invoke)
        //    {
        //        args.Result = args.DocumentControler.CopyAsText();
        //        args.RefreshLevel = UIStateRefreshLevel.All;
        //    }
        //}

        ///// <summary>
        ///// 复制内容
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="args"></param>
        //[WriterCommandDescription(
        //    StandardCommandNames.Copy,
        //    // ImageResource = "DCSoft.Writer.Commands.Images.CommandCopy.bmp",
        //    ReturnValueType=typeof( bool ))]
        //private void Copy(object sender, WriterCommandEventArgs args)
        //{
        //    if (args.Mode == WriterCommandEventMode.QueryState)
        //    {
        //        args.Enabled = (args.DocumentControler != null
        //            && args.DocumentControler.CanCopy);
        //    }
        //    else if (args.Mode == WriterCommandEventMode.Invoke)
        //    {
        //        if (args.ShowUI)
        //        {
        //            try
        //            {
        //                args.Result = args.DocumentControler.Copy();
        //                args.RefreshLevel = UIStateRefreshLevel.All;
        //            }
        //            catch (System.Exception ext)
        //            {
        //                DCSoft.WinForms.DCUIHelper.ShowErrorMessageBox( null , ext.ToString());
        //            }
        //        }
        //        else
        //        {
        //            args.Result = args.DocumentControler.Copy();
        //            args.RefreshLevel = UIStateRefreshLevel.All;
        //        }
        //    }
        //}

        ///// <summary>
        ///// 粘贴操作
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="args"></param>
        //[WriterCommandDescription(
        //    StandardCommandNames.Cut,
        //    // ImageResource = "DCSoft.Writer.Commands.Images.CommandCut.bmp",
        //    ReturnValueType = typeof( bool ))]
        //private void Cut(object sender, WriterCommandEventArgs args)
        //{
        //    if (args.Mode == WriterCommandEventMode.QueryState)
        //    {
        //        args.Enabled = (args.DocumentControler != null
        //            && args.DocumentControler.CanCut());
        //    }
        //    else if (args.Mode == WriterCommandEventMode.Invoke)
        //    {
        //        args.Result = false;
        //        args.Result = args.DocumentControler.Cut( args.ShowUI );
        //        args.RefreshLevel = UIStateRefreshLevel.All;
        //    }
        //}

        ///// <summary>
        ///// 执行删除操作
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="args"></param>
        //[WriterCommandDescription(
        //    StandardCommandNames.DeleteSelectionOld,
        //    ShortcutKey = Keys.Delete,
        //    // ImageResource = "DCSoft.Writer.Commands.Images.CommandDelete.bmp")]
        //private void DeleteSelectionOld(object sender, WriterCommandEventArgs args)
        //{
        //    if (args.Mode == WriterCommandEventMode.QueryState)
        //    {
        //        args.Enabled = (args.DocumentControler != null
        //            && args.DocumentControler.CanDeleteSelection);
        //    }
        //    else if (args.Mode == WriterCommandEventMode.Invoke)
        //    {
        //        args.DocumentControler.DeleteSelectionOld();
        //        args.RefreshLevel = UIStateRefreshLevel.All;
        //    }
        //}

        /// <summary>
        /// 执行删除整个文本行的操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        [WriterCommandDescription(
            StandardCommandNames.DeleteLine,
            // ImageResource = "DCSoft.Writer.Commands.Images.CommandDelete.bmp",
            ReturnValueType = typeof(bool))]
        private void DeleteLine(object sender, WriterCommandEventArgs args)
        {
            if (args.Mode == WriterCommandEventMode.QueryState)
            {
                args.Enabled = (args.EditorControl != null && args.EditorControl.Readonly == false);
            }
            else if (args.Mode == WriterCommandEventMode.Invoke)
            {
                args.Result = false;
                DomLine line = null;
                DCContent content = args.Document.Content;
                if (args.Parameter is DomElement)
                {
                    DomElement element = (DomElement)args.Parameter;
                    line = element.OwnerLine;
                    if (element.FirstContentElement != null)
                    {
                        line = element.FirstContentElement.OwnerLine;
                    }
                }
                else
                {
                    line = content.CurrentLine;
                }
                if (line == null)
                {
                    return;
                }
                DomContentElement ce = line[0].ContentElement;
                DCSelection selection  = new DCSelection( line[0].DocumentContentElement );
                int index = content.IndexOf( line[0] );
                DomElement lastElement = line.LastElement;
                int endIndex = content.IndexOf(lastElement);
                if (lastElement is DomParagraphFlagElement && ce.Elements.LastElement == lastElement)
                {
                    endIndex = endIndex - 1;
                }
                if (endIndex >= index)
                {
                    index = content.InnerFixIndex(index, FixIndexDirection.Back, false);
                    endIndex = content.InnerFixIndex(endIndex, FixIndexDirection.Forward, false);
                    while ( index <= endIndex )
                    {
                        if (args.DocumentControler.CanDeleteExt(content[index], DomAccessFlags.Normal))
                        {
                            break;
                        }
                        else
                        {
                            index++;
                        }
                    }//while
                    while (endIndex >= index)
                    {
                        if (args.DocumentControler.CanDeleteExt(content[endIndex] , DomAccessFlags.Normal))
                        {
                            break;
                        }
                        else
                        {
                            endIndex--;
                        }
                    }
                    if (endIndex >= index)
                    {
                        selection.Refresh(index, endIndex - index + 1 );
                        args.Document.BeginLogUndo();
                        int result = content.DeleteSelection(true, false, false, selection);
                        args.Document.EndLogUndo();
                        if (result > 0)
                        {
                            content.MoveToPosition(index);
                        }
                        args.Result = result > 0;
                        args.RefreshLevel = UIStateRefreshLevel.All;
                    }
                }
            }
        }

        /// <summary>
        /// 执行删除操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        [WriterCommandDescription(
            StandardCommandNames.Delete,
            ShortcutKey =Keys.Delete ,
            // ImageResource = "DCSoft.Writer.Commands.Images.CommandDelete.bmp",
            ReturnValueType=typeof( bool ))]
        private void Delete(object sender, WriterCommandEventArgs args)
        {
            if (args.Mode == WriterCommandEventMode.QueryState)
            {
                args.Enabled = (args.DocumentControler != null
                    && args.DocumentControler.Snapshot.CanDeleteSelection);
            }
            else if (args.Mode == WriterCommandEventMode.Invoke)
            {
                args.Result = false;
                bool result = false;
                result = args.DocumentControler.Delete(args.ShowUI);
                args.Result = result;
                args.RefreshLevel = UIStateRefreshLevel.All;
            }
        }


        /// <summary>
        /// 设置修改、插入模式
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        [WriterCommandDescription(
            StandardCommandNames.InsertMode,
            ShortcutKey = Keys.Insert,
            ReturnValueType=typeof( bool ))]
        private void InsertMode(object sender, WriterCommandEventArgs args)
        {
            if (args.Mode == WriterCommandEventMode.QueryState)
            {
                args.Enabled = args.EditorControl != null;
                args.Checked = args.EditorControl != null && args.EditorControl.InsertMode;
            }
            else if (args.Mode == WriterCommandEventMode.Invoke)
            {
                bool v = WriterUtilsInner.GetArgumentBooleanValue(
                    args.Parameter,
                    !args.EditorControl.InsertMode);
                if (v != args.EditorControl.InsertMode)
                {
                    args.EditorControl.InsertMode = v;
                    args.EditorControl.UpdateTextCaret();
                    args.EditorControl.OnSelectionChanged(EventArgs.Empty);
                }
                args.Result = args.EditorControl.InsertMode;
            }
        }

        ///// <summary>
        ///// 已纯文本格式粘贴操作
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="args"></param>
        //[WriterCommandDescription(
        //    StandardCommandNames.PasteAsText,
        //    // ImageResource = "DCSoft.Writer.Commands.Images.CommandPaste.bmp",
        //    ReturnValueType = typeof(bool))]
        //private void PasteAsText(object sender, WriterCommandEventArgs args)
        //{
        //    if (args.Mode == WriterCommandEventMode.QueryState)
        //    {
        //        args.Enabled = false;
        //        if (args.EditorControl != null && args.DocumentControler.CanInsertAtCurrentPosition)
        //        {
        //            //Clipboard.Clear();
        //            args.Enabled = true;
        //        }
        //        //args.Enabled = (args.EditorControl != null && args.EditorControl.InnerCanPaste);
        //    }
        //    else if (args.Mode == WriterCommandEventMode.Invoke)
        //    {
        //        args.Result = false;
        //        args.Result = args.EditorControl.InnerPaste(DataFormats.Text, args.ShowUI);
        //        args.RefreshLevel = UIStateRefreshLevel.All;
        //    }
        //}

        ///// <summary>
        ///// 粘贴操作
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="args"></param>
        //[WriterCommandDescription(
        //    StandardCommandNames.Paste,
        //    // ImageResource = "DCSoft.Writer.Commands.Images.CommandPaste.bmp",
        //    ReturnValueType = typeof( bool ))]
        //private void Paste(object sender, WriterCommandEventArgs args)
        //{
        //    if (args.Mode == WriterCommandEventMode.QueryState)
        //    {
        //        args.Enabled = false;
        //        if (args.EditorControl != null && args.DocumentControler.CanInsertAtCurrentPosition)
        //        {
        //            //Clipboard.Clear();
        //            args.Enabled = true;
        //        }
        //        //args.Enabled = (args.EditorControl != null && args.EditorControl.InnerCanPaste);
        //    }
        //    else if (args.Mode == WriterCommandEventMode.Invoke)
        //    {
        //        args.Result = false;
        //        args.Result = args.EditorControl.InnerPaste(null , args.ShowUI);
        //        args.RefreshLevel = UIStateRefreshLevel.All;
        //    }
        //}

        ///// <summary>
        ///// 选择性粘贴操作
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="args"></param>
        //[WriterCommandDescription(
        //    StandardCommandNames.SpecifyPaste,
        //    // ImageResource = "DCSoft.Writer.Commands.Images.CommandPaste.bmp",
        //    ReturnValueType=typeof( bool ))]
        //private void SpecifyPaste(object sender, WriterCommandEventArgs args)
        //{
        //    if (args.Mode == WriterCommandEventMode.QueryState)
        //    {
        //        args.Enabled = false;
        //        if (args.EditorControl != null && args.DocumentControler.CanInsertAtCurrentPosition)
        //        {
        //            //Clipboard.Clear();
        //            args.Enabled = true;
        //        }

        //        //args.Enabled = (args.EditorControl != null && args.EditorControl.InnerCanPaste );
        //    }
        //    else if (args.Mode == WriterCommandEventMode.Invoke)
        //    {
        //        args.Result = false;
        //        string specifyFormat = args.Parameter as string;
        //        if (string.IsNullOrEmpty(specifyFormat) == false)
        //        {
        //            args.Result = args.EditorControl.InnerPaste(specifyFormat ,args.ShowUI);
        //            args.RefreshLevel = UIStateRefreshLevel.All;
        //        }
        //    }
        //}

        /// <summary>
        /// 重复执行操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        [WriterCommandDescription(
            StandardCommandNames.Redo,
            ShortcutKey = Keys.Control | Keys.Y,
            // ImageResource = "DCSoft.Writer.Commands.Images.CommandRedo.bmp",
            ReturnValueType= typeof( bool ))]
        private void Redo(object sender, WriterCommandEventArgs args)
        {
            if (args.Mode == WriterCommandEventMode.QueryState)
            {
                args.Enabled = false;
                if (args.Document != null)
                {
                    args.Enabled = args.DocumentControler.EditorControlReadonly == false
                        && args.Document.UndoList != null
                        && args.Document.UndoList.CanRedo()
                        && args.Document.GetDocumentBehaviorOptions().EnableLogUndo ;
                }
            }
            else if (args.Mode == WriterCommandEventMode.Invoke)
            {
                args.Result = false;
                XUndoEventArgs e = new XUndoEventArgs();
                args.EditorControl.CancelEditElementValue();
                args.EditorControl.CancelFormatBrush();
                args.Document.States.ExecutingRedo = true ;
                try
                {
                    args.Result = args.Document.UndoList.Redo(e);
                }
                finally
                {
                    args.Document.States.ExecutingRedo = false;
                }
                args.RefreshLevel = UIStateRefreshLevel.All;
            }
        }

        /// <summary>
        /// 重复执行操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        [WriterCommandDescription(
            StandardCommandNames.Undo,
            ShortcutKey = Keys.Control | Keys.Z,
            // ImageResource = "DCSoft.Writer.Commands.Images.CommandUndo.bmp",
            ReturnValueType=typeof( bool ))]
        private void Undo(object sender, WriterCommandEventArgs args)
        {
            if (args.Mode == WriterCommandEventMode.QueryState)
            {
                args.Enabled = false;
                if (args.Document != null)
                {
                    args.Enabled = args.DocumentControler.EditorControlReadonly == false
                        && args.Document.UndoList != null
                        && args.Document.UndoList.CanUndo()
                        && args.Document.GetDocumentBehaviorOptions().EnableLogUndo ;
                }
            }
            else if (args.Mode == WriterCommandEventMode.Invoke)
            {
                args.Result = false;
                XUndoEventArgs e = new XUndoEventArgs();
                args.EditorControl.CancelEditElementValue();
                args.EditorControl.CancelFormatBrush();
                args.Document.States.ExecutingUndo = true;
                try
                {
                    args.Result = args.Document.UndoList.Undo(e);
                }
                finally
                {
                    args.Document.States.ExecutingUndo = false;
                }
                args.RefreshLevel = UIStateRefreshLevel.All;
            }
        }

        /// <summary>
        /// 清空撤销操作信息列表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        [WriterCommandDescription( StandardCommandNames.ClearUndo)]
        private void ClearUndo(object sender, WriterCommandEventArgs args)
        {
            if (args.Mode == WriterCommandEventMode.QueryState)
            {
                args.Enabled = args.Document != null ;
            }
            else if (args.Mode == WriterCommandEventMode.Invoke)
            {
                args.Result = false;
                if (args.Document != null && args.Document.UndoList != null)
                {
                    args.Document.CancelLogUndo();
                    args.Document.UndoList.Clear();
                    args.RefreshLevel = UIStateRefreshLevel.All;
                    args.Result = true;
                }
            }
        }

        /// <summary>
        /// 清空正文中所有输入域的内容
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        [WriterCommandDescription(
            StandardCommandNames.ClearAllFieldValue,
            ReturnValueType = typeof(bool))]
        private void ClearAllFieldValue(object sender, WriterCommandEventArgs args)
        {
            if (args.Mode == WriterCommandEventMode.QueryState)
            {
                args.Enabled = args.EditorControl != null && args.EditorControl.Readonly == false;
            }
            else if (args.Mode == WriterCommandEventMode.Invoke)
            {
                args.Result = false;
                bool result = false;
                DomElementList fields = args.Document.Body.GetElementsByType<DomInputFieldElement>();
                if (fields != null && fields.Count > 0)
                {
                    args.Document.BeginLogUndo();
                    foreach (DomInputFieldElement field in fields)
                    {
                        if (args.DocumentControler.CanModify(field, DomAccessFlags.Normal))
                        {
                            bool flag = false;
                            //args.Document.Content.MoveToPosition(field.FirstContentElement.ViewIndex + 1);
                            if (field.SetEditorTextExt(null, DomAccessFlags.Normal, true))
                            {
                                flag = true;
                            }
                            string v = field.InnerValue;
                            if (string.IsNullOrEmpty(v) == false)
                            {
                                if (args.Document.CanLogUndo)
                                {
                                    args.Document.UndoList.AddProperty("InnerValue", v, null, field);
                                }
                                field.InnerValue = null;
                                flag = true;
                            }
                            if (flag)
                            {
                                result = true;
                            }
                        }
                    }
                    args.Document.EndLogUndo();
                    args.Document.Modified = true;
                    args.Result = result;
                }
            }
        }

        /// <summary>
        /// 清空输入域的内容
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        [WriterCommandDescription(
            StandardCommandNames.ClearCurrentFieldValue,
            ReturnValueType= typeof( bool ))]
        private void ClearCurrentFieldValue(object sender, WriterCommandEventArgs args)
        {
            if (args.Mode == WriterCommandEventMode.QueryState)
            {
                args.Enabled = false;
                if (args.EditorControl == null)
                {
                    return;
                }
                DomInputFieldElement field = (DomInputFieldElement)args.Document.GetCurrentElement(typeof(DomInputFieldElement));
                if (field != null)
                {
                    // 这个判断有点消耗时间，不判断了。反正大多数情况下也没意义。
                    //if (args.DocumentControler.CanModify(field, DomAccessFlags.Normal)) 
                    {
                        args.Enabled = true;
                    }
                }
            }
            else if (args.Mode == WriterCommandEventMode.Invoke)
            {
                args.Result = false;
                DomInputFieldElement field = (DomInputFieldElement)args.Document.GetCurrentElement(
                    typeof(DomInputFieldElement));
                if (field != null)
                {
                    if (args.DocumentControler.CanModify(field, DomAccessFlags.Normal))
                    {
                        args.Document.BeginLogUndo();
                        bool result = false;
                        args.Document.Content.MoveToPosition(field.FirstContentElementInPublicContent.ContentIndex + 1);
                        if (field.SetEditorTextExt(null, DomAccessFlags.Normal , true ))
                        {
                            result = true;
                        }
                        string v = field.InnerValue;
                        if (string.IsNullOrEmpty(v) == false)
                        {
                            if (args.Document.CanLogUndo)
                            {
                                args.Document.UndoList.AddProperty("InnerValue", v, null, field);
                            }
                            field.InnerValue = null;
                            result = true;
                        }
                        args.Document.EndLogUndo();
                        args.Result = result;
                    }
                }
            }
        }

        /// <summary>
        /// 删除字段域元素
        /// </summary>
        /// <param name="sender">参数</param>
        /// <param name="args">参数</param>
        [WriterCommandDescription(
            StandardCommandNames.DeleteField,
            ReturnValueType=typeof( bool ))]
        private void DeleteField(object sender, WriterCommandEventArgs args)
        {
            if (args.Document == null)
            {
                args.Enabled = false;
                return;
            }
            DomElement element = args.Document.CurrentElement;
            if ( args.Parameter is DomElement)
            {
                element = ( DomElement ) args.Parameter;
            }
            while (element != null)
            {
                if (element is DomFieldElementBase)
                {
                    break;
                }
                element = element.Parent;
            }
            if (args.Mode == WriterCommandEventMode.QueryState)
            {
                if (element == null)
                {
                    args.Enabled = false;
                }
                else
                {
                    args.Enabled = args.DocumentControler != null
                        && args.DocumentControler.CanDelete(element);
                }
            }
            else if (args.Mode == WriterCommandEventMode.Invoke)
            {
                args.Result = false;
                DomFieldElementBase field = (DomFieldElementBase)element;
                int pos = field.StartElement.ContentIndex;
                if (field.EditorDelete(true))
                {
                    // 设置新插入点位置
                    args.Document.Content.SetSelection(pos, 0);
                    args.RefreshLevel = UIStateRefreshLevel.All;
                    args.Result = true;
                }
            }
        }

        /// <summary>
        /// 编辑元素值，标准快捷键是 F2 。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        [WriterCommandDescription(
            StandardCommandNames.ExecuteElementDefaultMethod,
            ShortcutKey = Keys.F2 )]
        private void ExecuteElementDefaultMethod(object sender, WriterCommandEventArgs args)
        {
            if (args.Mode == WriterCommandEventMode.QueryState)
            {
                args.Enabled = false;
                if (args.EditorControl != null && args.EditorControl.Readonly == false)
                {
                    args.Enabled = true;
                    return;
                    //if (args.EditorControl != null)
                    //{
                    //    args.Enabled = args.EditorControl.Readonly == false
                    //        && args.EditorControl.BeginEditElementValue(
                    //            args.EditorControl.CurrentElement, true);
                    //}
                }
            }
            else if (args.Mode == WriterCommandEventMode.Invoke)
            {
                args.EditorControl.ExecuteElementDefaultMethod(args.EditorControl.CurrentElement);

                //bool result = args.EditorControl.BeginEditElementValue(
                //    args.EditorControl.CurrentElement,
                //    false,
                //    ValueEditorActiveMode.F2);
                //if (result == false)
                //{
                //    result = args.EditorControl.BeginInsertKB();
                //}
                //args.Result = result;
            }
        }


        /// <summary>
        /// 查找
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        [WriterCommandDescription(StandardCommandNames.Search)]
        // ImageResource = "DCSoft.Writer.Commands.Images.CommandSearch.bmp" )]
        private void Search(object sender, WriterCommandEventArgs args)
        {
            if (args.Mode == WriterCommandEventMode.QueryState)
            {
                args.Enabled = args.EditorControl != null && args.Document != null;
            }
            else if (args.Mode == WriterCommandEventMode.Invoke)
            {
                InnerSearchReplace( args , false );
            }
        }

        /// <summary>
        /// 查找和替换
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        [WriterCommandDescription(StandardCommandNames.SearchReplace ,
            // ImageResource = "DCSoft.Writer.Commands.Images.CommandSearch.bmp",
            ShortcutKey=Keys.Control | Keys.F )]
        private void SearchReplace(object sender, WriterCommandEventArgs args)
        {
            if (args.Mode == WriterCommandEventMode.QueryState)
            {
                args.Enabled = args.EditorControl != null && args.Document != null;
            }
            else if (args.Mode == WriterCommandEventMode.Invoke)
            {
                InnerSearchReplace( args , true );
            }
        }

        /// <summary>
        /// 执行查找替换功能
        /// </summary>
        /// <param name="args"></param>
        /// <param name="enableReplace"></param>
        private void InnerSearchReplace(WriterCommandEventArgs args, bool enableReplace)
        {
            args.Result = -1;
            SearchReplaceCommandArgs cmdArgs = args.Parameter as SearchReplaceCommandArgs;
            {
                args.Result = -1;
                if (cmdArgs != null)
                {
                    // 不显示查找替换对话框，执行进行查找替换操作
                    var csr = new DCSoft.Writer.Commands.ContentSearchReplacer(args.Document);// WriterToolsBase.Instance.CreateContentSearchReplacer();
                    if (cmdArgs.EnableReplaceString)
                    {
                        int index = csr.Replace(cmdArgs);
                        args.Result = index;
                        args.RefreshLevel = UIStateRefreshLevel.None;
                    }
                    else
                    {
                        int index = csr.Search(cmdArgs, true, -1);
                        args.Result = index;
                        args.RefreshLevel = UIStateRefreshLevel.None;
                    }
                }
            }
        }


        /// <summary>
        /// 设置文档默认样式
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        [WriterCommandDescription( 
            StandardCommandNames.SetDefaultStyle ,
            ReturnValueType= typeof( bool ) )]
        private void SetDefaultStyle(object sender, WriterCommandEventArgs args)
        {
            if (args.Mode == WriterCommandEventMode.QueryState)
            {
                args.Enabled = false;
                if (args.Document != null && args.EditorControl.Readonly == false)
                {
                    args.Enabled = true;
                }
            }
            else if (args.Mode == WriterCommandEventMode.Invoke)
            {
                args.Result = false;
                DocumentContentStyle style = null;
                if (args.Parameter is string)
                {
                    style = new DocumentContentStyle();
                    style.DisableDefaultValue = true;
                    string values = (string)args.Parameter;
                    AttributeString attrs = new AttributeString(values);
                    bool setFlag = false;
                    foreach (AttributeStringItem item in attrs)
                    {
                        XDependencyProperty p = XDependencyProperty.GetProperty(
                            typeof(DocumentContentStyle),
                            item.Name);
                        if (p != null)
                        {
                            Type pt = p.PropertyType;
                            try
                            {
                                if (pt.Equals(typeof(string)))
                                {
                                    style.SetValue(p, item.Value);
                                }
                                else
                                {
                                    //TypeConverter tc = TypeDescriptor.GetConverter(pt);
                                    //if (tc != null && tc.CanConvertFrom(typeof(string)))
                                    //{
                                    //    object v = tc.ConvertFrom(item.Value);
                                    //    style.SetValue(p, v);
                                    //}
                                    //else
                                    {
                                        object v = Convert.ChangeType(item.Value, pt);
                                        style.SetValue(p, v);
                                    }
                                }
                                setFlag = true;
                            }
                            catch { }
                        }//p
                    }//foreach
                    if (setFlag == false )
                    {
                        // 输入的数据不对，退出处理
                        style = null;
                    }
                }//if
                else if (args.Parameter is DocumentContentStyle)
                {
                    style = (DocumentContentStyle)args.Parameter;
                }
                if (style != null)
                {
                    args.Document.EditorSetDefaultStyle( style , true );
                    args.RefreshLevel = UIStateRefreshLevel.All;
                    args.Result = true;
                }
            }//else
        }
    }
}