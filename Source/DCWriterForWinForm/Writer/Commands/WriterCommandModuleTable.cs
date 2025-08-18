using System;
using System.Collections.Generic;
using System.Text;
using DCSoft.Writer;
using DCSoft.Writer.Dom;
using DCSoft.Writer.Controls;
using DCSoft.Drawing;
using DCSoft.Printing;
using System.Windows.Forms;
using System.Text.Json;
using DCSoft.WASM;
using System.Diagnostics;

namespace DCSoft.Writer.Commands
{
    /// <summary>
    /// 操作表格的命令功能模块对象
    /// </summary>
    /// <remarks>编制 袁永福</remarks>
    internal sealed class WriterCommandModuleTable : WriterCommandModule
    {
        protected override WriterCommandList CreateCommands()
        {
            var list = new WriterCommandList();
            AddCommandToList(list, StandardCommandNames.AlignBottomCenter, this.AlignBottomCenter);
            AddCommandToList(list, StandardCommandNames.AlignBottomLeft, this.AlignBottomLeft);
            AddCommandToList(list, StandardCommandNames.AlignBottomRight, this.AlignBottomRight);
            AddCommandToList(list, StandardCommandNames.AlignMiddleCenter, this.AlignMiddleCenter);
            AddCommandToList(list, StandardCommandNames.AlignMiddleLeft, this.AlignMiddleLeft);
            AddCommandToList(list, StandardCommandNames.AlignMiddleRight, this.AlignMiddleRight);
            AddCommandToList(list, StandardCommandNames.AlignTopCenter, this.AlignTopCenter);
            AddCommandToList(list, StandardCommandNames.AlignTopLeft, this.AlignTopLeft);
            AddCommandToList(list, StandardCommandNames.AlignTopRight, this.AlignTopRight);
            AddCommandToList(list, StandardCommandNames.Table_DeleteColumn, this.Table_DeleteColumn);
            AddCommandToList(list, StandardCommandNames.Table_DeleteRow, this.Table_DeleteRow);
            AddCommandToList(list, StandardCommandNames.Table_DeleteTable, this.Table_DeleteTable);
            AddCommandToList(list, StandardCommandNames.Table_HeaderRow, this.Table_HeaderRow);
            AddCommandToList(list, StandardCommandNames.Table_InsertColumnLeft, this.Table_InsertColumnLeft);
            AddCommandToList(list, StandardCommandNames.Table_InsertColumnRight, this.Table_InsertColumnRight);
            AddCommandToList(list, StandardCommandNames.Table_InsertRowDown, this.Table_InsertRowDown);
            AddCommandToList(list, StandardCommandNames.Table_InsertRowUp, this.Table_InsertRowUp);
            AddCommandToList(list, StandardCommandNames.Table_InsertTable, this.Table_InsertTable);
            AddCommandToList(list, StandardCommandNames.Table_MergeCell, this.Table_MegeCell);
            AddCommandToList(list, StandardCommandNames.Table_ResetTableStyle, this.Table_ResetTableStyle);
            AddCommandToList(list, StandardCommandNames.Table_SplitCell, this.Table_SplitCell);
            AddCommandToList(list, StandardCommandNames.Table_SplitCellExt, this.Table_SplitCellExt);
            return list;
        }

        /// <summary>
        /// 重置表格样式
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        [WriterCommandDescription(StandardCommandNames.Table_ResetTableStyle)]
        private void Table_ResetTableStyle(object sender, WriterCommandEventArgs args)
        {
            if (args.Mode == WriterCommandEventMode.QueryState)
            {
                args.Enabled = args.Document != null;
            }
            else if (args.Mode == WriterCommandEventMode.Invoke)
            {
                DomTableElement table = GetHandledTable(args, false);
                if (table == null)
                {
                    return;
                }
                DocumentContentStyle style = new DocumentContentStyle();
                style.BorderColor = Color.Black;
                style.BorderStyle = DashStyle.Solid;
                style.BorderWidth = 1;
                style.BorderLeft = true;
                style.BorderTop = true;
                style.BorderRight = true;
                style.BorderBottom = true;
                int si = args.Document.ContentStyles.GetStyleIndex(style);
                table.StyleIndex = -1;
                foreach (DomTableCellElement cell in table.Cells)
                {
                    cell.EditorSetStyleDeeply(args.Document.DefaultStyle);
                    cell.StyleIndex = si;
                }
                if (table.Columns.Count > 0)
                {
                    DomContentElement ce = table.ContentElement;
                    float colWidth = (ce.ClientWidth - 3) / table.Columns.Count;
                    colWidth = Math.Max(50, colWidth);
                    foreach (DomTableColumnElement col in table.Columns)
                    {
                        col.Width = colWidth;
                    }
                }
                foreach (DomTableRowElement row in table.Rows)
                {
                    row.SpecifyHeight = 0;
                }
                args.Document.UndoList.Clear();
                table.EditorRefreshView();
                args.RefreshLevel = UIStateRefreshLevel.All;
                args.Document.Modified = true;
                args.Document.OnSelectionChanged();
                args.Document.OnDocumentContentChanged();
            }
        }

        /// <summary>
        /// 插入表格
        /// </summary>
        /// <param name="sender">参数</param>
        /// <param name="args">参数</param>
        [WriterCommandDescription(StandardCommandNames.Table_InsertTable)]
        private void Table_InsertTable(object sender, WriterCommandEventArgs args)
        {
            if (args.Mode == WriterCommandEventMode.QueryState)
            {
                args.Enabled = false;
                if (args.DocumentControler != null)
                {
                    args.Enabled = args.DocumentControler.CanInsertElementAtCurrentPosition(
                        typeof(DomTableElement));
                }
            }
            else if (args.Mode == WriterCommandEventMode.Invoke)
            {
                DomTableElement table = null;
                if (args.Parameter is DomTableElement)
                {
                    table = (DomTableElement)args.Parameter;
                }
                else
                {
                    XTextTableElementProperties info = args.Parameter as XTextTableElementProperties;
                    if (info == null)
                    {
                        info = new XTextTableElementProperties();
                    }
                    if (info.ColumnsCount <= 0 || info.RowsCount <= 0)
                    {
                        // 新增的表格的行数和列数不能为0
                        return;
                    }
                    if (info.ColumnWidth == 0)
                    {
                        // 根据当前位置设置表格列的宽度
                        if (args.Document.CurrentElement != null)
                        {
                            DomContentElement ce = args.Document.CurrentElement.ContentElement;
                            info.ColumnWidth = (ce.ClientWidth - args.Document.PixelToDocumentUnit(2)) / info.ColumnsCount;
                        }
                    }
                    table = (DomTableElement)info.CreateElement(
                        args.Document);
                }
                if (table != null)
                {
                    args.Document.AllocElementID("table", table);
                    foreach (DomTableRowElement row in table.Rows)
                    {
                        foreach (DomTableCellElement cell in row.Cells)
                        {
                            cell.FixElements();
                        }
                    }
                    //foreach (XTextTableCellElement cell in table.Cells)
                    //{
                    //    cell.FixElements();
                    //}
                    table.OwnerDocument = args.Document;
                    table.FixDomState();
                    table.SizeInvalid = true;
                    //using ( Graphics g = args.Document.CreateGraphics())
                    //{
                    //    DocumentPaintEventArgs args2 = new DocumentPaintEventArgs(g, Rectangle.Empty);
                    //    args2.Document = args.Document;
                    //    args2.Element = table;
                    //    args2.Render = args.Document.Render;
                    //    table.RefreshSize(args2);

                    //    //args.Document.Render.RefreshSize(table, g);
                    //}
                    args.DocumentControler.MeasureElementSize(new DomElementList(table));
                    args.Document.ValidateElementIDRepeat(table);
                    args.Document.BeginLogUndo();
                    args.Document.InsertElement(table);
                    args.Document.EndLogUndo();
                    args.RefreshLevel = UIStateRefreshLevel.All;
                    args.Result = table;
                }
            }
        }

        /// <summary>
        /// 删除整个表格
        /// </summary>
        /// <param name="sender">参数</param>
        /// <param name="args">参数</param>
        [WriterCommandDescription(
            StandardCommandNames.Table_DeleteTable)]
        private void Table_DeleteTable(object sender, WriterCommandEventArgs args)
        {
            if (args.Mode == WriterCommandEventMode.QueryState)
            {
                args.Enabled = false;
                if (args.Parameter is DomTableElement)
                {
                    args.Enabled = args.DocumentControler.CanDelete((DomTableElement)args.Parameter);
                }
                else
                {
                    DomTableCellElement cell = GetCurrentCell(args.Document);
                    if (cell != null)
                    {
                        args.Enabled = args.DocumentControler.CanDelete(cell);
                    }
                }
            }
            else if (args.Mode == WriterCommandEventMode.Invoke)
            {
                args.Result = null;
                DomTableElement table = null;
                if (args.Parameter is DomTableElement)
                {
                    // 直接从参数中获得表格对象
                    table = (DomTableElement)args.Parameter;
                }
                else
                {
                    // 获得插入点所在的表格对象
                    DomTableCellElement cell = GetCurrentCell(args.Document);
                    table = cell.OwnerTable;
                }
                ContentProtectedInfoList protectedInfos = new ContentProtectedInfoList();
                args.DocumentControler.BeginCacheValue();
                try
                {
                    table.CollecProtectedElements(protectedInfos, 100);
                }
                finally
                {
                    args.DocumentControler.EndCacheValue();
                }
                if (protectedInfos != null && protectedInfos.Count > 0)
                {
                    // 表格中包含不可删除的内容，不执行操作
                    if (args.ShowUI)
                    {
                        args.Document.PromptProtectedContent(protectedInfos);
                    }
                }
                else
                {
                    // 删除整个表格
                    if (table.EditorDelete(true))
                    {
                        args.Result = table;
                        args.RefreshLevel = UIStateRefreshLevel.All;
                    }
                }
            }
        }

        /// <summary>
        /// 合并单元格
        /// </summary>
        /// <param name="sender">参数</param>
        /// <param name="args">参数</param>
        [WriterCommandDescription(StandardCommandNames.Table_MergeCell,
            // ImageResource = "DCSoft.Writer.Commands.Images.CommandMergeCell.bmp",
            ReturnValueType = typeof(DomTableCellElement))]
        private void Table_MegeCell(object sender, WriterCommandEventArgs args)
        {
            if (args.Mode == WriterCommandEventMode.QueryState
                || args.Mode == WriterCommandEventMode.Invoke)
            {
                if (args.EditorControl == null)
                {
                    args.Enabled = false;
                    return;
                }
                var cp = args.Parameter as MegeCellCommandParameter;
                if (cp == null)
                {
                    // 没有指定用户参数
                    DomDocumentContentElement dce = args.Document.CurrentContentElement;
                    if (dce.Selection.Mode == ContentRangeMode.Cell)
                    {
                        // 仅仅处于纯粹的选择单元格的模式下本动作才有效
                        DomTableCellElement cell = (DomTableCellElement)dce.Selection.Cells.FirstElement;
                        DomTableElement table = cell.OwnerTable;
                        if (cell.RowIndex < table.Rows.Count - 1
                            || cell.ColIndex < table.Columns.Count - 1)
                        {
                            args.Enabled = args.DocumentControler.CanModify(cell);
                            if (args.Enabled && args.Mode == WriterCommandEventMode.Invoke)
                            {
                                // 获得选择区域的第一个单元格作为要处理的单元格
                                DomTableCellElement firstCell = (DomTableCellElement)dce.Selection.Cells.FirstElement;
                                // 获得选择区域中最后一个单元格作为合并的截止单元格
                                DomTableCellElement lastCell = (DomTableCellElement)dce.Selection.Cells.LastElement;
                                if (lastCell.IsOverrided)
                                {
                                    lastCell = lastCell.OverrideCell;
                                }
                                // 计算新跨行数和跨列数
                                cp = new MegeCellCommandParameter();
                                cp.Cell = firstCell;
                                cp.NewRowSpan = lastCell.RowIndex + lastCell.RowSpan - firstCell.RowIndex;
                                cp.NewColSpan = lastCell.ColIndex + lastCell.ColSpan - firstCell.ColIndex;

                                int rowSpanFix = 0;
                                for (int startcolindex = firstCell.OwnerRow.RowIndex;
                                    startcolindex < lastCell.OwnerRow.RowIndex;
                                    startcolindex++)
                                {
                                    DomTableRowElement row = firstCell.OwnerTable.Rows[startcolindex] as DomTableRowElement;
                                    if (row.Visible == false)
                                    {
                                        rowSpanFix++;
                                    }
                                }
                                cp.NewRowSpanFix = rowSpanFix;
                            }
                        }
                    }
                    else
                    {
                        args.Enabled = false;
                    }
                }
                else
                {
                    // 指定了用户参数
                    if (cp.Cell == null && string.IsNullOrEmpty(cp.CellID) == false)
                    {
                        cp.Cell = args.Document.GetElementById(cp.CellID) as DomTableCellElement;
                    }
                    if (cp.Cell != null && cp.Cell.IsOverrided == false && args.DocumentControler.CanModify(cp.Cell))
                    {
                        if (cp.NewRowSpan >= cp.Cell.RowSpan && cp.NewColSpan >= cp.Cell.ColSpan)
                        {
                            args.Enabled = true;
                        }
                    }
                }
                if (args.Mode == WriterCommandEventMode.Invoke)
                {
                    if (args.Enabled)
                    {
                        // 执行合并单元格的操作
                        cp.Cell.EditorSetCellSpan(cp.NewRowSpan, cp.NewColSpan, true, null);
                        args.RefreshLevel = UIStateRefreshLevel.All;
                        args.Result = cp;
                    }
                }
            }
            //if (args.Mode == WriterCommandEventMode.QueryState)
            //{
            //    args.Enabled = false;
            //    if (args.Document != null)
            //    {
            //        XTextDocumentContentElement dce = args.Document.CurrentContentElement;
            //        if (dce.Selection.Mode == ContentRangeMode.Cell)
            //        {
            //            // 仅仅处于纯粹的选择单元格的模式下本动作才有效
            //            XTextTableCellElement cell = (XTextTableCellElement)dce.Selection.Cells.FirstElement;
            //            XTextTableElement table = cell.OwnerTable;
            //            if (cell.RowIndex < table.Rows.Count - 1
            //                || cell.ColIndex < table.Columns.Count - 1)
            //            {
            //                args.Enabled = args.DocumentControler.CanModify(cell);
            //            }
            //        }
            //    }
            //}
            //else if (args.Mode == WriterCommandEventMode.Invoke)
            //{
            //    XTextDocumentContentElement dce = args.Document.CurrentContentElement;
            //    if (dce.Selection.Mode == ContentRangeMode.Cell)
            //    {

            //        // 获得选择区域的第一个单元格作为要处理的单元格
            //        XTextTableCellElement firstCell = (XTextTableCellElement)dce.Selection.Cells.FirstElement;
            //        // 获得选择区域中最后一个单元格作为合并的截止单元格
            //        XTextTableCellElement lastCell = (XTextTableCellElement)dce.Selection.Cells.LastElement;
            //        // 计算新跨行数和跨列数
            //        int rowSpan = lastCell.RowIndex + lastCell.RowSpan - firstCell.RowIndex;
            //        int colSpan = lastCell.ColIndex + lastCell.ColSpan - firstCell.ColIndex;
            //        // 执行合并单元格的操作
            //        firstCell.EditorSetCellSpan(rowSpan, colSpan, true, null);
            //        args.RefreshLevel = UIStateRefreshLevel.All;
            //        args.Result = firstCell;
            //    }
            //}
        }

        /// <summary>
        /// 增强型的拆分单元格
        /// </summary>
        /// <param name="sender">参数</param>
        /// <param name="args">参数</param>
        [WriterCommandDescription(StandardCommandNames.Table_SplitCellExt,
            // ImageResource = "DCSoft.Writer.Commands.Images.CommandSplitCell.bmp",
            ReturnValueType = typeof(DomTableCellElement))]
        private void Table_SplitCellExt(object sender, WriterCommandEventArgs args)
        {
            SplitCellExtCommandParameter parameter = null;
            if (args.Mode == WriterCommandEventMode.QueryState
                || args.Mode == WriterCommandEventMode.Invoke)
            {
                if (args.EditorControl == null)
                {
                    args.Enabled = false;
                    return;
                }
                // 获得命令参数
                if (args.Parameter is SplitCellExtCommandParameter)
                {
                    parameter = (SplitCellExtCommandParameter)args.Parameter;
                    if (parameter.CellElement == null)
                    {
                        parameter.CellElement = args.Document.GetElementById(parameter.CellID) as DomTableCellElement;
                    }
                }
                else
                {
                    parameter = new SplitCellExtCommandParameter();
                    if (args.Document.Selection.Cells != null && args.Document.Selection.Cells.Count > 1)
                    {
                        parameter.CellElement = null;
                    }
                    else
                    {
                        parameter.CellElement = GetCurrentCell(args.Document);
                    }
                }
                if (parameter.CellElement != null)
                {
                    if (args.DocumentControler.CanModify(parameter.CellElement) == false)
                    {
                        parameter.CellElement = null;
                    }
                }
            }//if
            if (args.Mode == WriterCommandEventMode.QueryState)
            {
                // 判断命令是否可用
                args.Enabled = parameter != null && parameter.CellElement != null;
            }
            else if (args.Mode == WriterCommandEventMode.Invoke)
            {
                // 执行命令
                args.Result = null;
                if (parameter == null || parameter.CellElement == null)
                {
                    return;
                }

                if (args.Parameter is string)
                {
                    string strParamter = (string)args.Parameter;
                    int index = strParamter.IndexOf(',');
                    if (index >= 0)
                    {
                        try
                        {
                            parameter.NewRowSpan = int.Parse(strParamter.Substring(0, index));
                            parameter.NewColSpan = int.Parse(strParamter.Substring(index + 1));
                        }
                        catch (Exception ext)
                        {
                            DCConsole.Default.WriteLine("Table_SplitCellExt:" + ext.Message);
                            parameter.NewRowSpan = parameter.CellElement.RowSpan;
                            parameter.NewColSpan = parameter.CellElement.ColSpan;
                        }
                    }
                }
                if (parameter.NewRowSpan < 1)
                {
                    parameter.NewRowSpan = parameter.CellElement.RowSpan;
                }
                if (parameter.NewColSpan < 1)
                {
                    parameter.NewColSpan = parameter.CellElement.ColSpan;
                }
                if (parameter.CellElement.EditorSplitCellExt(
                    parameter.NewRowSpan,
                    parameter.NewColSpan,
                    true))
                {
                    args.Result = parameter.CellElement;
                    args.RefreshLevel = UIStateRefreshLevel.All;
                }
            }
        }


        /// <summary>
        /// 拆分单元格
        /// </summary>
        /// <param name="sender">参数</param>
        /// <param name="args">参数</param>
        [WriterCommandDescription(StandardCommandNames.Table_SplitCell,
            // ImageResource = "DCSoft.Writer.Commands.Images.CommandSplitCell.bmp",
            ReturnValueType = typeof(DomTableCellElement))]
        private void Table_SplitCell(object sender, WriterCommandEventArgs args)
        {
            if (args.Mode == WriterCommandEventMode.QueryState)
            {
                if (args.EditorControl == null)
                {
                    args.Enabled = false;
                    return;
                }
                args.Enabled = false;
                if (args.Document != null)
                {
                    if (args.Document.Selection.Cells != null
                        && args.Document.Selection.Cells.Count > 1)
                    {
                        //当多个单元格被选中时本命令无效
                        args.Enabled = false;
                        return;
                    }
                    DomTableCellElement cell = GetCurrentCell(args.Document);
                    if (cell != null)
                    {
                        // 只有当前表格的跨行数或者跨列数大于1则动作才有效
                        if (cell.RowSpan > 1 || cell.ColSpan > 1)
                        {
                            args.Enabled = args.DocumentControler.CanModify(cell);
                        }
                    }
                }
            }
            else if (args.Mode == WriterCommandEventMode.Invoke)
            {
                args.Result = null;

                args.RefreshLevel = UIStateRefreshLevel.None;
                DomTableCellElement cell = GetCurrentCell(args.Document);
                if (cell != null)
                {
                    if (cell.RowSpan > 1 || cell.ColSpan > 1)
                    {
                        if (cell.EditorSetCellSpan(1, 1, true, null))
                        {
                            args.Result = cell;
                            args.RefreshLevel = UIStateRefreshLevel.All;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 在当前行上面插入表格行
        /// </summary>
        /// <param name="sender">事件参数</param>
        /// <param name="args">事件参数</param>
        [WriterCommandDescription(StandardCommandNames.Table_InsertRowUp,
            // ImageResource = "DCSoft.Writer.Commands.Images.CommandInsertRowUp.bmp",
            ReturnValueType = typeof(DomTableRowElement))]
        private void Table_InsertRowUp(object sender, WriterCommandEventArgs args)
        {
            if (args.Mode == WriterCommandEventMode.QueryState)
            {
                args.Enabled = InsertRows(args, true, true);
            }
            else if (args.Mode == WriterCommandEventMode.Invoke)
            {
                InsertRows(args, true, false);
            }
        }


        /// <summary>
        /// 获得命令操作要处理的表格对象
        /// </summary>
        /// <param name="args">命令参数</param>
        /// <param name="useLastTable">是否允许使用文档中最后一个表格</param>
        /// <returns>操作是否成功</returns>
        private DomTableElement GetHandledTable(WriterCommandEventArgs args, bool useLastTable)
        {
            if (args.Parameter is DomTableElement)
            {
                return (DomTableElement)args.Parameter;
            }
            if (args.Parameter is string)
            {
                // 参数为一个字符串，认为是一个表格元素编号
                string id = (string)args.Parameter;
                if (string.IsNullOrEmpty(id) == false)
                {
                    DomTableElement table = args.Document.GetElementById(id) as DomTableElement;
                    return table;
                }
            }
            if (args.Parameter is TableCommandArgs)
            {
                // 参数指定了表格对象
                TableCommandArgs tca = (TableCommandArgs)args.Parameter;
                if (tca.TableElement != null)
                {
                    return tca.TableElement;
                }
                if (string.IsNullOrEmpty(tca.TableID) == false)
                {
                    DomTableElement table = args.Document.GetElementById(tca.TableID) as DomTableElement;
                    if (table != null)
                    {
                        return table;
                    }
                }
            }
            // 获得当前插入点所在的表格对象
            DomTableElement table2 = (DomTableElement)args.Document.GetCurrentElement(typeof(DomTableElement));
            if (table2 != null)
            {
                return table2;
            }
            if (useLastTable)
            {
                // 获得文档正文中最后一个表格对象
                DomTableElement lastTable = (DomTableElement)args.Document.Body.GetLastElementByType(typeof(DomTableElement));
                return lastTable;
            }
            return null;
        }



        /// <summary>
        /// 在当前行上面插入表格行
        /// </summary>
        /// <param name="sender">事件参数</param>
        /// <param name="args">事件参数</param>
        [WriterCommandDescription(StandardCommandNames.Table_InsertRowDown,
            // ImageResource = "DCSoft.Writer.Commands.Images.CommandInsertRowDown.bmp",
            ReturnValueType = typeof(DomTableRowElement))]
        private void Table_InsertRowDown(object sender, WriterCommandEventArgs args)
        {
            if (args.Mode == WriterCommandEventMode.QueryState)
            {
                args.Enabled = InsertRows(args, false, true);
            }
            else if (args.Mode == WriterCommandEventMode.Invoke)
            {
                InsertRows(args, false, false);
            }
        }
        /// <summary>
        /// 删除当前表格行
        /// </summary>
        /// <param name="sender">事件参数</param>
        /// <param name="args">事件参数</param>
        [WriterCommandDescription(StandardCommandNames.Table_DeleteRow,
            // ImageResource = "DCSoft.Writer.Commands.Images.CommandDeleteRow.bmp",
            ReturnValueType = typeof(bool),
            DefaultResultValue = false)]
        private void Table_DeleteRow(object sender, WriterCommandEventArgs args)
        {
            if (args.Mode == WriterCommandEventMode.Invoke || args.Mode == WriterCommandEventMode.QueryState)
            {
                if (args.Mode == WriterCommandEventMode.QueryState)
                {
                    args.Enabled = false;
                }
                if (args.EditorControl == null)
                {
                    return;
                }
                args.Result = false;
                DomElementList rows = new DomElementList();
                if (args.Parameter is DomTableRowElement)
                {
                    // 直接指定了要删除的表格行对象
                    rows.Add((DomTableRowElement)args.Parameter);
                }
                else if (args.Parameter is DomElementList)
                {
                    DomTableElement table2 = null;
                    DomElementList list2 = (DomElementList)args.Parameter;
                    foreach (DomElement e in list2)
                    {
                        if (e is DomTableRowElement)
                        {
                            DomTableRowElement row = (DomTableRowElement)e;
                            if (table2 == null)
                            {
                                table2 = row.OwnerTable;
                            }
                            else if (row.OwnerTable != table2)
                            {
                                throw new ArgumentOutOfRangeException("不是同一个表格的表格行");
                            }
                            rows.Add(row);
                        }
                    }
                }
                else if (args.Parameter is TableCommandArgs)
                {
                    // 指定了用户参数
                    TableCommandArgs cmdArgs = (TableCommandArgs)args.Parameter;
                    DomTableElement table = cmdArgs.TableElement;
                    if (table == null)
                    {
                        table = args.Document.GetElementById(cmdArgs.TableID) as DomTableElement;
                    }
                    if (table != null && table.Rows.Count > 0)
                    {
                        int index = cmdArgs.RowIndex;
                        if (index < 0)
                        {
                            index = 0;
                        }
                        if (index > table.Rows.Count)
                        {
                            index = table.Rows.Count - 1;
                        }
                        for (int iCount = 0; iCount < cmdArgs.RowsCount; iCount++)
                        {
                            int ri = index + iCount;
                            if (ri >= 0 && ri < table.Rows.Count)
                            {
                                rows.Add(table.Rows[ri]);
                            }
                        }//for
                    }
                    if (rows.Count == 0)
                    {
                        // 没有要处理的表格行
                        return;
                    }
                    foreach (DomTableRowElement row in rows)
                    {
                        if (args.DocumentControler.CanDelete(row) == false)
                        {
                            // 出现不可删除的表格行
                            return;
                        }
                    }
                }
                else
                {
                    // 获得被用户选择的表格行对象列表
                    rows = GetRowsOrColumns(args.Document, true);
                }
                if (rows != null && rows.Count > 0)
                {
                    // 删除表格行
                    DomTableRowElement row = (DomTableRowElement)rows[0];
                    if (args.DocumentControler.CanDelete(row) == false)
                    {
                        if (args.Mode == WriterCommandEventMode.QueryState)
                        {
                            args.Enabled = false;
                        }
                        return;
                    }
                    DomTableElement table = row.OwnerTable;
                    if (rows.Count == table.Rows.Count)
                    {
                        // 删除整个表格
                        Table_DeleteTable(sender, args);
                        if (args.Result != null)
                        {
                            args.Result = true;
                        }
                    }
                    else
                    {
                        // 删除部分表格行
                        ContentProtectedInfoList protectedInfos = new ContentProtectedInfoList();
                        args.DocumentControler.BeginCacheValue();
                        try
                        {
                            if (args.Mode == WriterCommandEventMode.QueryState)
                            {
                                foreach (DomTableRowElement row2 in rows)
                                {
                                    row2.CollecProtectedElements(protectedInfos, 1);
                                    if (protectedInfos.Count > 0)
                                    {
                                        break;
                                    }

                                }
                            }
                            else
                            {
                                foreach (DomTableRowElement row2 in rows)
                                {
                                    //if (row2.TemporaryHeaderRow == false)
                                    {
                                        if (row2.CollecProtectedElements(protectedInfos, 10))
                                        {
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                        finally
                        {
                            args.DocumentControler.EndCacheValue();
                        }
                        if (protectedInfos.Count > 0)
                        {
                            // 包含不可删除的内容，无法进行删除
                            if (args.Mode == WriterCommandEventMode.QueryState)
                            {
                                args.Enabled = false;
                            }
                            else
                            {
                                if (args.ShowUI)
                                {
                                    args.Document.PromptProtectedContent(protectedInfos);
                                }
                            }
                            args.Result = false;
                        }
                        else
                        {
                            // 不包含不可删除的内容，删除表格行
                            if (args.Mode == WriterCommandEventMode.QueryState)
                            {
                                args.Enabled = true;
                                return;
                            }
                            else
                            {
                                args.Result = table.EditorDeleteRows2(table.Rows.IndexOf(row), rows.Count, true, null);
                            }
                        }
                    }
                    if (args.Parameter is TableCommandArgs)
                    {
                        TableCommandArgs cmdArgs = (TableCommandArgs)args.Parameter;
                        cmdArgs.Result = (bool)args.Result;
                    }
                    if (args.Result == null)
                    {
                        args.Result = false;
                    }
                    if ((bool)args.Result)
                    {
                        args.RefreshLevel = UIStateRefreshLevel.All;
                    }
                }
            }
        }

        /// <summary>
        /// 在左边插入表格列
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        [WriterCommandDescription(StandardCommandNames.Table_InsertColumnLeft,
            // ImageResource = "DCSoft.Writer.Commands.Images.CommandInsertColumnLeft.bmp",
            ReturnValueType = typeof(DomTableColumnElement))]
        private void Table_InsertColumnLeft(object sender, WriterCommandEventArgs args)
        {
            if (args.Mode == WriterCommandEventMode.QueryState)
            {
                args.Enabled = false;
                InsertCols(args, args.Document, true);

                //XTextTableElement table = GetCurrentTable(args.Document);
                //if (table != null)
                //{
                //    args.Enabled = args.DocumentControler.CanInsert(
                //        table, 
                //        0,
                //        typeof(XTextTableColumnElement));
                //}
            }
            else if (args.Mode == WriterCommandEventMode.Invoke)
            {
                args.Result = InsertCols(args, args.Document, true);
                if (args.Result == null)
                {
                    args.RefreshLevel = UIStateRefreshLevel.None;
                }
                else
                {
                    args.RefreshLevel = UIStateRefreshLevel.All;
                }
            }
        }

        /// <summary>
        /// 在右边插入表格列
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        [WriterCommandDescription(StandardCommandNames.Table_InsertColumnRight,
            // ImageResource = "DCSoft.Writer.Commands.Images.CommandInsertColumnRight.bmp",
            ReturnValueType = typeof(DomTableColumnElement))]
        private void Table_InsertColumnRight(object sender, WriterCommandEventArgs args)
        {
            if (args.Mode == WriterCommandEventMode.QueryState)
            {
                args.Enabled = false;
                InsertCols(args, args.Document, false);
                //XTextTableElement table = GetCurrentTable(args.Document);
                //if (table != null)
                //{
                //    args.Enabled = args.DocumentControler.CanInsert(table, 0, typeof(XTextTableColumnElement));
                //}
            }
            else if (args.Mode == WriterCommandEventMode.Invoke)
            {
                args.Result = InsertCols(args, args.Document, false);
                if (args.Result == null)
                {
                    args.RefreshLevel = UIStateRefreshLevel.None;
                }
                else
                {
                    args.RefreshLevel = UIStateRefreshLevel.All;
                }
            }
        }

        /// <summary>
        /// 删除当前表格行
        /// </summary>
        /// <param name="sender">事件参数</param>
        /// <param name="args">事件参数</param>
        [WriterCommandDescription(StandardCommandNames.Table_DeleteColumn,
            // ImageResource = "DCSoft.Writer.Commands.Images.CommandDeleteColumn.bmp",
            ReturnValueType = typeof(bool),
            DefaultResultValue = false)]
        private void Table_DeleteColumn(object sender, WriterCommandEventArgs args)
        {
            //if (args.Mode == WriterCommandEventMode.QueryState)
            //{
            //    args.Enabled = false;
            //    if (args.Document != null)
            //    {
            //        XTextElementList list = GetRowsOrColumns(args.Document, false);
            //        if (list != null && list.Count > 0)
            //        {
            //            args.Enabled = args.DocumentControler.CanDelete(list[0]);
            //        }
            //    }
            //}
            if (args.Mode == WriterCommandEventMode.Invoke
                || args.Mode == WriterCommandEventMode.QueryState)
            {
                if (args.Mode == WriterCommandEventMode.QueryState)
                {
                    //args.Enabled = false;
                }
                if (args.EditorControl == null)
                {
                    return;
                }
                args.Result = false;
                DomElementList list = new DomElementList();
                if (args.Parameter is DomTableColumnElement)
                {
                    // 用户指定表格列对象
                    list.Add((DomTableColumnElement)args.Parameter);
                }
                else if (args.Parameter is DomElementList)
                {
                    // 用户指定多个表格列对象
                    DomElementList list2 = (DomElementList)args.Parameter;
                    DomTableElement table2 = null;
                    foreach (DomElement e in list2)
                    {
                        if (e is DomTableColumnElement)
                        {
                            DomTableColumnElement col = (DomTableColumnElement)e;
                            if (table2 == null)
                            {
                                table2 = col.OwnerTable;
                            }
                            else if (table2 != col.OwnerTable)
                            {
                                throw new ArgumentOutOfRangeException("不是同表格的表格列");
                            }
                            list.Add(col);
                        }
                    }
                }
                else if (args.Parameter is TableCommandArgs)
                {
                    // 用户指定参数
                    TableCommandArgs cmdArgs = (TableCommandArgs)args.Parameter;
                    DomTableElement table = cmdArgs.TableElement;
                    if (table == null)
                    {
                        table = args.Document.GetElementById(cmdArgs.TableID) as DomTableElement;
                    }
                    if (table != null && table.Columns.Count > 0)
                    {
                        int index = cmdArgs.ColIndex;
                        if (index < 0)
                        {
                            index = 0;
                        }
                        if (index > table.Columns.Count)
                        {
                            index = table.Columns.Count - 1;
                        }
                        // 获得所有参与操作的表格列对象
                        for (int i = 0; i < cmdArgs.ColsCount; i++)
                        {
                            int ci = i + index;
                            if (ci >= 0 && ci < table.Columns.Count)
                            {
                                list.Add(table.Columns[ci]);
                            }
                        }
                    }
                    if (list.Count == 0)
                    {
                        // 没有任何处理的表格列对象
                        return;
                    }
                    foreach (DomTableColumnElement col in list)
                    {
                        if (args.DocumentControler.CanDelete(col) == false)
                        {
                            // 表格列不能删除 
                            return;
                        }
                    }
                }
                else
                {
                    list = GetRowsOrColumns(args.Document, false);
                }
                if (list != null && list.Count > 0)
                {
                    // 删除多个表格列
                    foreach (DomTableColumnElement col2 in list)
                    {
                        if (args.DocumentControler.CanDelete(col2) == false)
                        {
                            if (args.Mode == WriterCommandEventMode.QueryState)
                            {
                                args.Enabled = false;
                            }
                            return;
                        }
                        else
                        {
                            ContentProtectedInfoList protectedInfos = new ContentProtectedInfoList();
                            bool flag = true;
                            if (args.Mode == WriterCommandEventMode.QueryState)
                            {
                                if (col2.OwnerTable.Rows.Count > 100)
                                {
                                    // 表格行过多，不判断,这个命令用得不多，不影响大局。
                                    flag = false;
                                }
                            }
                            if (flag)
                            {
                                DomElementList cells = col2.Cells;
                                args.DocumentControler.BeginCacheValue();
                                try
                                {
                                    foreach (DomTableCellElement cell in cells)
                                    {
                                        if (args.Mode == WriterCommandEventMode.QueryState)
                                        {
                                            if (cell.CollecProtectedElements(protectedInfos, 1))
                                            {
                                                break;
                                            }
                                        }
                                        else
                                        {
                                            if (cell.CollecProtectedElements(protectedInfos, 10))
                                            {
                                                break;
                                            }
                                        }
                                    }
                                }
                                finally
                                {
                                    args.DocumentControler.EndCacheValue();
                                }

                                if (protectedInfos.Count > 0)
                                {
                                    // 包含不可删除的内容，无法进行删除
                                    if (args.Mode == WriterCommandEventMode.QueryState)
                                    {
                                        //args.Enabled = false;
                                    }
                                    else
                                    {
                                        if (args.ShowUI)
                                        {
                                            args.Document.PromptProtectedContent(protectedInfos);
                                        }
                                    }
                                    //args.Result = false;
                                    return;
                                }
                            }
                        }
                    }

                    DomTableColumnElement col = (DomTableColumnElement)list[0];
                    DomTableElement table = col.OwnerTable;
                    if (list.Count == table.Columns.Count)
                    {
                        if (args.Mode == WriterCommandEventMode.QueryState)
                        {
                            args.Enabled = true;
                            return;
                        }
                        args.Parameter = table;
                        Table_DeleteTable(sender, args);
                    }
                    else
                    {
                        // 删除部分表格行
                        ContentProtectedInfoList protectedInfos = new ContentProtectedInfoList();
                        //if (args.EditorControl != null && args.RaiseFromUI)
                        //{
                        //    CollectProtectedElementsEventArgs args2 = new CollectProtectedElementsEventArgs(
                        //       args.EditorControl,
                        //       args.Document,
                        //       list,
                        //       protectedInfos);
                        //    args.EditorControl.OnEventCollectProtectedElements(args2);
                        //}
                        if (protectedInfos.Count > 0)
                        {
                            args.DocumentControler.BeginCacheValue();
                            try
                            {
                                foreach (DomTableRowElement row2 in table.Rows)
                                {
                                    //if (row2.TemporaryHeaderRow == false)
                                    {
                                        for (int iCount = 0; iCount < list.Count; iCount++)
                                        {
                                            DomTableCellElement cell = (DomTableCellElement)row2.Cells[col.Index + iCount];
                                            if (cell.CollecProtectedElements(protectedInfos, 10))
                                            {
                                                break;
                                            }
                                        }
                                    }
                                }
                            }
                            finally
                            {
                                args.DocumentControler.EndCacheValue();
                            }
                        }
                        if (protectedInfos != null
                            && protectedInfos.Count > 0)
                        {
                            if (args.Mode == WriterCommandEventMode.QueryState)
                            {
                                args.Enabled = false;
                                return;
                            }
                            if (args.ShowUI)
                            {
                                args.Document.PromptProtectedContent(protectedInfos);
                            }
                            args.Result = false;
                        }
                        else
                        {
                            if (args.Mode == WriterCommandEventMode.QueryState)
                            {
                                args.Enabled = true;
                                return;
                            }
                            else
                            {
                                args.Result = table.EditorDeleteColumns2(
                                    col.Index,
                                    list.Count,
                                    true,
                                    null,
                                    args.Document.GetDocumentEditOptions().KeepTableWidthWhenInsertDeleteColumn,
                                    null);
                            }
                        }
                    }
                    if (args.Parameter is TableCommandArgs)
                    {
                        ((TableCommandArgs)args.Parameter).Result = (bool)args.Result;
                    }
                    if (args.Parameter is DomTableElement)
                    {
                        if (!((object)args.Result == null))
                        {
                            args.Result = true;
                        }
                        else
                        {
                            args.Result = false;
                        }
                    }
                    if ((bool)args.Result)
                    {
                        args.RefreshLevel = UIStateRefreshLevel.All;
                    }
                }
            }
        }


        public static DomElementList InnerInsertRow(
            DomTableElement table,
            DomTableRowElement currentRow,
            bool insertUp,
            bool autoSetRowSpan,
            int newRowsNum)
        {
            if (table == null)
            {
                throw new ArgumentNullException("table");
            }
            if (currentRow == null)
            {
                throw new ArgumentNullException("currentRow");
            }
            if (currentRow.OwnerTable != table)
            {
                throw new ArgumentOutOfRangeException("currentRow");
            }
            DomElementList newRows = new DomElementList();
            DomDocument document = table.OwnerDocument;
            try
            {
                DCGraphics g = null;
                InnerDocumentPaintEventArgs args = null;
                g = document.InnerCreateDCGraphics();
                args = document.CreateInnerPaintEventArgs(g);
                DomContainerElement.GlobalEnabledResetChildElementStats = false;
                for (int iCount = 0; iCount < newRowsNum; iCount++)
                {
                    var newRow = currentRow.EditorClone();
                    ////////////////////////////////////////
                    newRow.FixDomState();
                    if (args != null)
                    {
                        args.Element = newRow;
                        newRow.RefreshSize(args);
                    }
                    newRows.Add(newRow);
                }//for
                if (g != null)
                {
                    g.Dispose();
                }
            }
            finally
            {
                DomContainerElement.GlobalEnabledResetChildElementStats = true;
            }
            if (insertUp)
            {
                // 在原地插入表格行，将原先的表格行向下挤压
                table.EditorInsertRows2(table.Rows.IndexOf(currentRow), newRows, true, null, autoSetRowSpan);
            }
            else
            {
                // 在下面插入表格行
                table.EditorInsertRows2(table.Rows.IndexOf(currentRow) + 1, newRows, true, null, autoSetRowSpan);
            }
            return newRows;
        }

        /// <summary>
        /// 在当前单元格下面插入表格行
        /// </summary>
        /// <param name="args">参数</param>
        /// <param name="detect">是否仅仅是检测操作能否成功，而不真正的执行操作</param>
        /// <param name="insertUp">是否是在上面插入表格行</param>
        /// <returns>操作是否成功</returns>
        private bool InsertRows(WriterCommandEventArgs args, bool insertUp, bool detect)
        {
            if (args.EditorControl == null)
            {
                return false;
            }
            DomTableRowElement myRow = null;
            int newRowsNum = 1;
            if (args.Parameter is DomTableRowElement)
            {
                // 直接指定要操作的表格行对象
                myRow = (DomTableRowElement)args.Parameter;
            }
            else if (args.Parameter is TableCommandArgs)
            {
                // 用户指定参数
                TableCommandArgs cmdArgs = (TableCommandArgs)args.Parameter;
                newRowsNum = cmdArgs.RowsCount;
                DomTableElement table = cmdArgs.TableElement;
                if (table == null)
                {
                    table = args.Document.GetElementById(cmdArgs.TableID) as DomTableElement;
                }
                if (table != null && table.Rows.Count > 0)
                {
                    int index = cmdArgs.RowIndex;
                    if (index < 0)
                    {
                        myRow = (DomTableRowElement)table.Rows[0];
                    }
                    else if (index >= table.Rows.Count)
                    {
                        myRow = (DomTableRowElement)table.Rows.LastElement;
                    }
                    else
                    {
                        myRow = (DomTableRowElement)table.Rows[index];
                    }
                }
            }
            if (myRow == null)
            {
                DomElementList list = GetRowsOrColumns(args.Document, true);
                if (list == null || list.Count == 0)
                {
                    return false;
                }
                if (insertUp)
                {
                    myRow = (DomTableRowElement)list.FirstElement;
                }
                else
                {
                    myRow = (DomTableRowElement)list.LastElement;
                }
            }
            if (myRow != null)
            {
                if (args.DocumentControler.CanInsert(
                    myRow.OwnerTable,
                    0,
                    typeof(DomTableRowElement)) == false)
                {
                    return false;
                }
                if (myRow.HeaderStyle)
                {
                    if (insertUp)
                    {
                        // 标题行上面不能插入表格行
                        return false;
                    }
                }
            }
            if (detect)
            {
                // 仅仅进行检测，到此为止
                if (myRow != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            DomTableElement myTable = myRow.OwnerTable;
            // 插入表格行
            args.Result = InnerInsertRow(myTable, myRow, insertUp, false, newRowsNum);
            if (args.Result == null)
            {
                args.RefreshLevel = UIStateRefreshLevel.None;
                return false;
            }
            else
            {
                if (args.Parameter is TableCommandArgs)
                {
                    ((TableCommandArgs)args.Parameter).Result = true;
                }
                args.RefreshLevel = UIStateRefreshLevel.All;
                return true;
            }
        }



        /// <summary>
        /// 在当前单元格旁边插入表格列
        /// </summary>
        private DomElementList InsertCols(
            WriterCommandEventArgs args,
            DomDocument doc,
            bool insertLeft)
        {
            if (args.EditorControl == null)
            {
                return null;
            }
            if (args.Mode == WriterCommandEventMode.Invoke)
            {
            }
            DomTableColumnElement col = null;
            int newColsNum = 1;
            if (args.Parameter is DomTableColumnElement)
            {
                // 直接指定表格列对象
                col = (DomTableColumnElement)args.Parameter;
            }
            else if (args.Parameter is TableCommandArgs)
            {
                // 通过参数指定表格列
                TableCommandArgs cmdArgs = (TableCommandArgs)args.Parameter;
                newColsNum = cmdArgs.ColsCount;
                DomTableElement table = cmdArgs.TableElement;
                if (table == null)
                {
                    table = args.Document.GetElementById(cmdArgs.TableID) as DomTableElement;
                }
                if (table != null && table.Columns.Count > 0)
                {
                    int index = cmdArgs.ColIndex;
                    if (index < 0)
                    {
                        col = (DomTableColumnElement)table.Columns[0];
                    }
                    else if (index >= table.Columns.Count)
                    {
                        col = (DomTableColumnElement)table.Columns.LastElement;
                    }
                    else
                    {
                        col = (DomTableColumnElement)table.Columns[index];
                    }
                }
            }
            if (col == null)
            {
                DomElementList list = GetRowsOrColumns(doc, false);
                if (list == null || list.Count == 0)
                {
                    // 没有选择任何表格列，无法进行操作
                    if (args.Mode == WriterCommandEventMode.QueryState)
                    {
                        args.Enabled = false;
                    }
                    return null;
                }
                if (insertLeft)
                {
                    col = (DomTableColumnElement)list.FirstElement;
                }
                else
                {
                    col = (DomTableColumnElement)list.LastElement;
                }
            }
            DomTableElement myTable = col.OwnerTable;
            if (args.DocumentControler.CanInsert(
                myTable,
                0,
                typeof(DomTableColumnElement)) == false)
            {
                if (args.Mode == WriterCommandEventMode.QueryState)
                {
                    args.Enabled = false;
                }
                return null;
            }

            if (args.Mode == WriterCommandEventMode.QueryState)
            {
                // 只是查询命令状态，到此为止
                args.Enabled = true;
                return null;
            }

            DomElementList newCols = new DomElementList();
            for (int iCount = 0; iCount < newColsNum; iCount++)
            {
                DomTableColumnElement newCol = myTable.CreateColumnInstance();
                newCol.Width = col.Width;
                newCols.Add(newCol);
            }

            if (insertLeft)
            {
                // 在左侧插入表格列
                myTable.EditorInsertColumns2(
                    col.Index,
                    newCols,
                    null,
                    true,
                    null,
                    doc.GetDocumentEditOptions().KeepTableWidthWhenInsertDeleteColumn,
                    null);
            }
            else
            {
                // 在右侧插入表格列
                myTable.EditorInsertColumns2(
                    col.Index + 1,
                    newCols,
                    null,
                    true,
                    null,
                    doc.GetDocumentEditOptions().KeepTableWidthWhenInsertDeleteColumn,
                    null);
            }
            if (newCols != null && newCols.Count > 0)
            {
                if (args.Parameter is TableCommandArgs)
                {
                    ((TableCommandArgs)args.Parameter).Result = true;
                }
            }
            return newCols;
        }

        public static DomElementList GetHandledRows(DomDocument document, bool firstRowOnly)
        {
            DomElementList cells = WriterCommandModuleTable.GetHandledCells(document, firstRowOnly);
            DomElementList result = new DomElementList();
            if (cells != null && cells.Count > 0)
            {
                foreach (DomTableCellElement cell in cells)
                {
                    if (result.Contains(cell.OwnerRow) == false)
                    {
                        result.Add(cell.OwnerRow);
                    }
                }
            }
            return result;
        }



        /// <summary>
        /// 单元格内容底端居中对齐
        /// </summary>
        /// <param name="sender">参数</param>
        /// <param name="args">参数</param>
        [WriterCommandDescription(StandardCommandNames.AlignBottomCenter)]
        // ImageResource = "DCSoft.Writer.Commands.Images.CommandAlignBottomCenter.bmp")]
        private void AlignBottomCenter(object sender, WriterCommandEventArgs args)
        {
            SetCellContentAlign(ContentAlignment.BottomCenter, args);
        }
        /// <summary>
        /// 单元格内容底端左对齐
        /// </summary>
        /// <param name="sender">参数</param>
        /// <param name="args">参数</param>
        [WriterCommandDescription(StandardCommandNames.AlignBottomLeft)]
        // ImageResource = "DCSoft.Writer.Commands.Images.CommandAlignBottomLeft.bmp")]
        private void AlignBottomLeft(object sender, WriterCommandEventArgs args)
        {
            SetCellContentAlign(ContentAlignment.BottomLeft, args);
        }

        /// <summary>
        /// 单元格内容底端居中对齐
        /// </summary>
        /// <param name="sender">参数</param>
        /// <param name="args">参数</param>
        [WriterCommandDescription(StandardCommandNames.AlignBottomRight)]
        // ImageResource = "DCSoft.Writer.Commands.Images.CommandAlignBottomRight.bmp")]
        private void AlignBottomRight(object sender, WriterCommandEventArgs args)
        {
            SetCellContentAlign(ContentAlignment.BottomRight, args);
        }

        /// <summary>
        /// 单元格内容垂直居中水平居中对齐
        /// </summary>
        /// <param name="sender">参数</param>
        /// <param name="args">参数</param>
        [WriterCommandDescription(StandardCommandNames.AlignMiddleCenter)]
        // ImageResource = "DCSoft.Writer.Commands.Images.CommandAlignMiddleCenter.bmp")]
        private void AlignMiddleCenter(object sender, WriterCommandEventArgs args)
        {
            SetCellContentAlign(ContentAlignment.MiddleCenter, args);
        }

        /// <summary>
        /// 单元格内容垂直居中水平左对齐
        /// </summary>
        /// <param name="sender">参数</param>
        /// <param name="args">参数</param>
        [WriterCommandDescription(StandardCommandNames.AlignMiddleLeft)]
        // ImageResource = "DCSoft.Writer.Commands.Images.CommandAlignMiddleLeft.bmp")]
        private void AlignMiddleLeft(object sender, WriterCommandEventArgs args)
        {
            SetCellContentAlign(ContentAlignment.MiddleLeft, args);
        }
        /// <summary>
        /// 单元格内容垂直居中右对齐
        /// </summary>
        /// <param name="sender">参数</param>
        /// <param name="args">参数</param>
        [WriterCommandDescription(StandardCommandNames.AlignMiddleRight)]
        // ImageResource = "DCSoft.Writer.Commands.Images.CommandAlignMiddleRight.bmp")]
        private void AlignMiddleRight(object sender, WriterCommandEventArgs args)
        {
            SetCellContentAlign(ContentAlignment.MiddleRight, args);
        }
        /// <summary>
        /// 单元格内容靠上居中对齐
        /// </summary>
        /// <param name="sender">参数</param>
        /// <param name="args">参数</param>
        [WriterCommandDescription(StandardCommandNames.AlignTopCenter)]
        // ImageResource = "DCSoft.Writer.Commands.Images.CommandAlignTopCenter.bmp")]
        private void AlignTopCenter(object sender, WriterCommandEventArgs args)
        {
            SetCellContentAlign(ContentAlignment.TopCenter, args);
        }
        /// <summary>
        /// 单元格内容靠上居中对齐
        /// </summary>
        /// <param name="sender">参数</param>
        /// <param name="args">参数</param>
        [WriterCommandDescription(StandardCommandNames.AlignTopLeft)]
        // ImageResource = "DCSoft.Writer.Commands.Images.CommandAlignTopLeft.bmp")]
        private void AlignTopLeft(object sender, WriterCommandEventArgs args)
        {
            SetCellContentAlign(ContentAlignment.TopLeft, args);
        }
        /// <summary>
        /// 单元格内容靠上居中对齐
        /// </summary>
        /// <param name="sender">参数</param>
        /// <param name="args">参数</param>
        [WriterCommandDescription(StandardCommandNames.AlignTopRight)]
        // ImageResource = "DCSoft.Writer.Commands.Images.CommandAlignTopRight.bmp")]
        private void AlignTopRight(object sender, WriterCommandEventArgs args)
        {
            SetCellContentAlign(ContentAlignment.TopRight, args);
        }

        /// <summary>
        /// 设置单元格的内容对齐方式
        /// </summary>
        /// <param name="align">新对齐方式</param>
        /// <param name="args">参数</param>
        private static void SetCellContentAlign(
             ContentAlignment align,
            WriterCommandEventArgs args)
        {
            if (args.Mode == WriterCommandEventMode.QueryState)
            {
                args.Enabled = false;
                args.Checked = false;
                if (args.EditorControl == null)
                {
                    return;
                }
                DomElementList cells = WriterCommandModuleTable.GetHandledCells(args.Document, true);
                if (cells != null && cells.Count > 0)
                {
                    DomTableCellElement cell = (DomTableCellElement)cells[0];
                    foreach (DomParagraphFlagElement flag in cell.ParagraphsFlags)
                    {
                        if (args.DocumentControler.CanModify(flag))
                        {
                            args.Enabled = true;
                            switch (cell.ContentVertialAlign)
                            {
                                case VerticalAlignStyle.Top:
                                    switch (flag.RuntimeStyle.Align)
                                    {
                                        case DocumentContentAlignment.Left:
                                            args.Checked = align == ContentAlignment.TopLeft;
                                            break;
                                        case DocumentContentAlignment.Center:
                                            args.Checked = align == ContentAlignment.TopCenter;
                                            break;
                                        case DocumentContentAlignment.Right:
                                            args.Checked = align == ContentAlignment.TopRight;
                                            break;
                                        case DocumentContentAlignment.Justify:
                                            args.Checked = align == ContentAlignment.TopCenter;
                                            break;
                                    }
                                    break;
                                //case VerticalAlignStyle.Justify:
                                case VerticalAlignStyle.Middle:
                                    switch (flag.RuntimeStyle.Align)
                                    {
                                        case DocumentContentAlignment.Left:
                                            args.Checked = align == ContentAlignment.MiddleLeft;
                                            break;
                                        case DocumentContentAlignment.Center:
                                            args.Checked = align == ContentAlignment.MiddleCenter;
                                            break;
                                        case DocumentContentAlignment.Right:
                                            args.Checked = align == ContentAlignment.MiddleRight;
                                            break;
                                        case DocumentContentAlignment.Justify:
                                            args.Checked = align == ContentAlignment.MiddleCenter;
                                            break;
                                    }
                                    break;
                                case VerticalAlignStyle.Bottom:
                                    switch (flag.RuntimeStyle.Align)
                                    {
                                        case DocumentContentAlignment.Left:
                                            args.Checked = align == ContentAlignment.BottomLeft;
                                            break;
                                        case DocumentContentAlignment.Center:
                                            args.Checked = align == ContentAlignment.BottomCenter;
                                            break;
                                        case DocumentContentAlignment.Right:
                                            args.Checked = align == ContentAlignment.BottomRight;
                                            break;
                                        case DocumentContentAlignment.Justify:
                                            args.Checked = align == ContentAlignment.BottomCenter;
                                            break;
                                    }
                                    break;
                            }//switch
                            return;

                        }//if
                    }//foreach
                }
            }
            else if (args.Mode == WriterCommandEventMode.Invoke)
            {
                args.Result = false;
                args.RefreshLevel = UIStateRefreshLevel.All;
                DomElementList cells = WriterCommandModuleTable.GetHandledCells(args.Document, false);
                if (cells != null && cells.Count > 0)
                {
                    Dictionary<DomElement, int> newStyleIndexs = new Dictionary<DomElement, int>();
                    VerticalAlignStyle newVAlign = VerticalAlignStyle.Middle;
                    DocumentContentAlignment newHAlign = DocumentContentAlignment.Left;
                    switch (align)
                    {
                        case ContentAlignment.BottomCenter:
                            newVAlign = VerticalAlignStyle.Bottom;
                            newHAlign = DocumentContentAlignment.Center;
                            break;
                        case ContentAlignment.BottomLeft:
                            newVAlign = VerticalAlignStyle.Bottom;
                            newHAlign = DocumentContentAlignment.Left;
                            break;
                        case ContentAlignment.BottomRight:
                            newVAlign = VerticalAlignStyle.Bottom;
                            newHAlign = DocumentContentAlignment.Right;
                            break;
                        case ContentAlignment.MiddleCenter:
                            newVAlign = VerticalAlignStyle.Middle;
                            newHAlign = DocumentContentAlignment.Center;
                            break;
                        case ContentAlignment.MiddleLeft:
                            newVAlign = VerticalAlignStyle.Middle;
                            newHAlign = DocumentContentAlignment.Left;
                            break;
                        case ContentAlignment.MiddleRight:
                            newVAlign = VerticalAlignStyle.Middle;
                            newHAlign = DocumentContentAlignment.Right;
                            break;
                        case ContentAlignment.TopCenter:
                            newVAlign = VerticalAlignStyle.Top;
                            newHAlign = DocumentContentAlignment.Center;
                            break;
                        case ContentAlignment.TopLeft:
                            newVAlign = VerticalAlignStyle.Top;
                            newHAlign = DocumentContentAlignment.Left;
                            break;
                        case ContentAlignment.TopRight:
                            newVAlign = VerticalAlignStyle.Top;
                            newHAlign = DocumentContentAlignment.Right;
                            break;
                    }

                    foreach (DomTableCellElement cell in cells)
                    {
                        if (cell.IsOverrided)
                        {
                            continue;
                        }
                        if (args.DocumentControler.CanModify(cell))
                        {
                            if (cell.ContentVertialAlign != newVAlign)
                            {
                                DocumentContentStyle rs = (DocumentContentStyle)cell.RuntimeStyle.CloneParent();
                                rs.DisableDefaultValue = true;
                                rs.VerticalAlign = newVAlign;
                                newStyleIndexs[cell] = args.Document.ContentStyles.GetStyleIndex(rs);
                            }
                            foreach (DomParagraphFlagElement flag in cell.ParagraphsFlags)// .ParagraphsEOFs)
                            {
                                if (flag.OwnerLine == null)
                                {
                                }
                                if (args.DocumentControler.CanModify(flag))
                                {
                                    if (flag.RuntimeStyle.Align != newHAlign)
                                    {
                                        DocumentContentStyle rs = (DocumentContentStyle)flag.RuntimeStyle.CloneParent();
                                        rs.DisableDefaultValue = true;
                                        rs.Align = newHAlign;
                                        newStyleIndexs[flag] = args.Document.ContentStyles.GetStyleIndex(rs);
                                    }
                                }//if
                            }//foreach
                        }//if
                    }//foreach

                    if (newStyleIndexs.Count > 0)
                    {
                        args.Result = true;
                        args.Document.EditorSetElementStyle(newStyleIndexs, true, true, args.Name);
                    }
                }
            }
        }


        /// <summary>
        /// 标题行
        /// </summary>
        /// <param name="sender">参数</param>
        /// <param name="args">参数</param>
        [WriterCommandDescription(StandardCommandNames.Table_HeaderRow)]
        private void Table_HeaderRow(object sender, WriterCommandEventArgs args)
        {
            if (args.Mode == WriterCommandEventMode.QueryState)
            {
                if (args.EditorControl == null)
                {
                    args.Enabled = false;
                    return;
                }
                args.Checked = false;
                DomElementList rows = GetHandledRows(args.Document, true);
                if (rows != null && rows.Count > 0)
                {
                    args.Enabled = true;
                    foreach (DomTableRowElement row in rows)
                    {
                        if (row.HeaderStyle)
                        {
                            args.Checked = true;
                            break;
                        }
                    }
                }
                else
                {
                    args.Enabled = false;
                }
            }
            else if (args.Mode == WriterCommandEventMode.Invoke)
            {
                DomElementList rows = GetHandledRows(args.Document, false);
                if (rows != null && rows.Count > 0)
                {
                    DomTableRowElement lastRow = (DomTableRowElement)rows.LastElement;
                    DomTableElement table = lastRow.OwnerTable;
                    rows = new DomElementList();
                    for (int iCount = 0; iCount <= table.Rows.IndexOf(lastRow); iCount++)
                    {
                        rows.Add(table.Rows[iCount]);
                    }
                    bool newHeaderStyle = false;
                    if (args.Parameter is bool)
                    {
                        newHeaderStyle = (bool)args.Parameter;
                    }
                    else
                    {
                        newHeaderStyle = true;
                        foreach (DomTableRowElement row in rows)
                        {
                            if (row.HeaderStyle)
                            {
                                newHeaderStyle = false;
                                break;
                            }
                        }//foreach
                    }
                    //bool modified = false;
                    Dictionary<DomTableRowElement, bool> newHeaderStyles = new Dictionary<DomTableRowElement, bool>();
                    foreach (DomTableRowElement row in rows)
                    {
                        if (row.HeaderStyle != newHeaderStyle)
                        {
                            newHeaderStyles[row] = newHeaderStyle;

                            //if (args.Document.CanLogUndo)
                            //{
                            //    args.Document.UndoList.AddProperty(
                            //        "HeaderStyle",
                            //        row.HeaderStyle, 
                            //        newHeaderStyle,
                            //        row);
                            //}
                            //row.HeaderStyle = newHeaderStyle;
                            //modified = true;
                            //row.OwnerTable.RuntimeRows = null;
                        }
                    }
                    args.Document.BeginLogUndo();
                    if (newHeaderStyles.Count > 0)
                    {
                        DomTableElement table2 = ((DomTableRowElement)rows[0]).OwnerTable;
                        table2.EditorSetHeaderRow(newHeaderStyles, true);
                    }
                    args.Document.EndLogUndo();
                    args.RefreshLevel = UIStateRefreshLevel.All;
                }
            }
        }


        /// <summary>
        /// 获得当前位置所在的表格行或者所有在同一个表格中的表格行或者表格列的列表
        /// </summary>
        /// <param name="document">文档对象</param>
        /// <param name="getTableRow">true:获得表格行;false:获得表格列</param>
        /// <returns>获得的要操作的对象列表</returns>
        public static DomElementList GetRowsOrColumns(DomDocument document, bool getTableRow)
        {
            DomElementList result = new DomElementList();
            DCSelection selection = document.Selection;
            if (selection.Cells != null && selection.Cells.Count > 0)
            {
                DomTableElement table = ((DomTableCellElement)selection.Cells[0]).OwnerTable;
                if (getTableRow)
                {
                    DomElement lastElement = null;
                    foreach (DomTableCellElement cell in selection.Cells)
                    {
                        DomTableRowElement row = cell.OwnerRow;
                        if (row != lastElement)
                        {
                            lastElement = row;
                            //if( result.Contains( row ))
                            //{
                            //    continue;
                            //}
                            if (row.OwnerTable == table)
                            {
                                result.Add(row);
                            }
                        }
                    }
                    lastElement = null;
                }
                else
                {
                    foreach (DomTableCellElement cell in selection.Cells)
                    {
                        DomTableColumnElement col = cell.OwnerColumn;
                        if (col.OwnerTable == table && result.Contains(col) == false)
                        {
                            result.Add(col);
                        }
                    }
                }
            }
            else
            {
                DomTableCellElement cell = GetCurrentCell(document);
                if (cell != null && cell.RowIndex >= 0)
                {
                    DomTableElement table = cell.OwnerTable;
                    if (getTableRow)
                    {
                        for (int iCount = 0; iCount < cell.RowSpan; iCount++)
                        {
                            result.Add(table.Rows[cell.RowIndex + iCount]);
                        }
                    }
                    else
                    {
                        for (int iCount = 0; iCount < cell.ColSpan; iCount++)
                        {
                            result.Add(table.Columns[cell.ColIndex + iCount]);
                        }
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 获得文档中需要处理的单元格对象列表
        /// </summary>
        /// <param name="document">文档对象</param>
        /// <param name="firstOnly">只返回第一个要处理的单元格对象</param>
        /// <returns>获得的单元格对象列表</returns>
        public static DomElementList GetHandledCells(DomDocument document, bool firstOnly)
        {
            if (document == null)
            {
                throw new ArgumentNullException("document");
            }
            DomElementList result = new DomElementList();
            if (document.Selection.Length == 0)
            {
                if (document.CurrentElement != null)
                {
                    DomTableCellElement cell = document.CurrentElement.OwnerCell;
                    if (cell != null)
                    {
                        result.Add(cell);
                    }
                }
            }
            else
            {
                if (document.Selection.Mode == ContentRangeMode.Cell
                    || document.Selection.Mode == ContentRangeMode.Mixed)
                {
                    result = document.Selection.Cells.Clone();
                }
                else
                {
                    DomTableCellElement cell = document.Selection.ContentElements[0].OwnerCell;
                    if (cell != null)
                    {
                        result.Add(cell);
                    }
                }
            }
            var ctl = document.DocumentControler;
            ctl.BeginCacheValue();
            try
            {
                if (firstOnly)
                {
                    foreach (DomTableCellElement cell in result)
                    {
                        if (cell.IsOverrided == false && ctl.CanModify(cell))
                        {
                            DomElementList list2 = new DomElementList(cell);
                            return list2;
                        }
                    }
                    return new DomElementList();
                }
                else
                {
                    for (int iCount = result.Count - 1; iCount >= 0; iCount--)
                    {
                        DomTableCellElement cell = (DomTableCellElement)result[iCount];
                        if (cell.IsOverrided || ctl.CanModify(cell) == false)
                        {
                            // 删除不能修改的单元格对象
                            result.RemoveAt(iCount);
                        }
                    }
                    return result;
                }
            }
            finally
            {
                ctl.EndCacheValue();
            }
        }

        /// <summary>
        /// 获得当前单元格对象
        /// </summary>
        /// <param name="doc">设计文档对象</param>
        /// <returns>当前单元格对象,若没有则返回空引用</returns>
        public static DomTableCellElement GetCurrentCell(DomDocument doc)
        {
            if (doc == null)
            {
                return null;
            }
            DCContent content = doc.Content;
            if (content.Selection.Cells != null
                && content.Selection.Cells.Count > 0)
            {
                return (DomTableCellElement)content.Selection.Cells[0];
            }
            else
            {
                DomElement element = doc.CurrentElement;
                while (element != null)
                {
                    if (element is DomTableCellElement)
                    {
                        return (DomTableCellElement)element;
                    }
                    element = element.Parent;
                }
            }
            return null;
        }
    }
}