using System;
using System.Collections.Generic;
using System.Text;
//using DCSoft.Writer.Dom.Undo;
using DCSystemXml;
using System.ComponentModel;
using System.Runtime.InteropServices;
using DCSoft.Writer.Data;

namespace DCSoft.Writer.Dom
{
    /// <summary>
    /// 表格列元素
    /// </summary>
    /// <remarks>编写 袁永福</remarks>
#if !RELEASE
    [System.Diagnostics.DebuggerDisplay("Column {Index}:{Width}")]
#endif
    public sealed partial class DomTableColumnElement : DomElement
    {
        public static readonly string TypeName_XTextTableColumnElement = "XTextTableColumnElement";
        public override string TypeName => TypeName_XTextTableColumnElement;
        /// <summary>
        /// 初始化对象
        /// </summary>
        //[DCSoft.Common.DCPublishAPI]
        public DomTableColumnElement()
        {
        }
        /// <summary>
        /// 文档元素编号前缀
        /// </summary>
        public override string ElementIDPrefix()
        {
            return "col";
        }

        

        /// <summary>
        /// 属性无效
        /// </summary>
        new private string FormulaValue
        {
            get
            {
                return base.FormulaValue;
            }
            set
            {
                base.FormulaValue = value;
            }
        }
        public override DomElement Clone(bool Deeply)
        {
            DomTableColumnElement col = (DomTableColumnElement)base.Clone(Deeply);
            col.InnerCells = null;
            return col;
        }

        /// <summary>
        /// 判断元素是否挂在指定文档的DOM结构中
        /// </summary>
        /// <param name="document">文档对象</param>
        /// <returns>是否挂在DOM结构中</returns>
        public override bool BelongToDocumentDom(DomDocument document)
        {
            if (this.Parent == null)
            {
                return false;
            }
            else
            {
                return this.Parent.BelongToDocumentDom(document);
            }
        }


        /// <summary>
        /// 宽度是否为零
        /// </summary>
        internal bool ZeroWidth()
        {
            return Math.Abs(this.Width) < 0.01;
        }

        /// <summary>
        /// 选择整个表格列
        /// </summary>
        /// <returns>操作是否成功</returns>
        public override bool Select()
        {
            DomTableElement table = this.OwnerTable;
            if (table == null || table.Rows.Count == 0)
            {
                return false;
            }
            int colIndex = table.Columns.IndexOf(this);
            if (colIndex >= 0)
            {
                DomDocumentContentElement dce = this.DocumentContentElement;
                if (dce == null)
                {
                    return false;
                }
                if (this.OwnerDocument.CurrentContentElement != dce)
                {
                    dce.Focus();
                }
                DomElementList cells = this.Cells;
                dce.SetSelectionSpecifyCells(
                    (DomTableCellElement)cells.FirstElement,
                    (DomTableCellElement)cells.LastElement);
                return true;
            }
            return false;
        }

        /// <summary>
        /// 获得输入焦点
        /// </summary>
        public override void Focus()
        {
            DomTableElement table = this.OwnerTable;
            if (table == null || table.Rows.Count == 0)
            {
                return;
            }
            int colIndex = table.Columns.IndexOf(this);
            if (colIndex >= 0)
            {
                DomTableCellElement cell = table.GetCell(0, colIndex, false);
                if (cell != null)
                {
                    if (cell.OverrideCell != null)
                    {
                        cell = cell.OverrideCell;
                    }
                    cell.Focus();
                }
            }
        }

        /// <summary>
        /// 创建新的文档对象，使其包含本文档元素的复制品
        /// </summary>
        /// <param name="includeThis">是否包含本文档原始对象,对表格列该参数无作用</param>
        /// <returns>创建的文档对象</returns>
        public override DomDocument CreateContentDocument(bool includeThis)
        {
            DomTableElement table = this.OwnerTable;
            if (table == null)
            {
                return null;
            }
            int colIndex = table.Columns.IndexOf(this);
            DomElementList sourceCells = new DomElementList();
            foreach (DomTableRowElement row in table.Rows)
            {
                DomTableCellElement cell = (DomTableCellElement)row.Cells[colIndex];
                if (cell.IsOverrided == false)
                {
                    sourceCells.Add(cell);
                }
            }
            if (sourceCells.Count > 0)
            {
                DomTableElement tempTable = (DomTableElement)table.Clone(false);
                tempTable.Rows = new DomElementList();
                tempTable.Columns = new DomElementList();
                tempTable.Columns.Add(this.Clone(true));
                foreach (DomTableCellElement cell in sourceCells)
                {
                    DomTableRowElement tempRow = (DomTableRowElement)cell.OwnerRow.Clone(false);
                    tempRow.Cells = new DomElementList();
                    DomTableCellElement tempCell = (DomTableCellElement)cell.Clone(true);
                    tempCell.InternalSetColSpan(1);
                    tempCell.InternalSetRowSpan(1);
                    tempRow.Cells.Add(tempCell);
                    tempTable.Rows.Add(tempRow);
                }
                DomDocument document = tempTable.CreateContentDocument(true);
                tempTable.Dispose();
                return document;
            }
            return null;
        }

        /// <summary>
        /// 在编辑器中设置表格列的宽度
        /// </summary>
        /// <param name="newWidth">新表格列宽度</param>
        /// <param name="logUndo">是否记录撤销操作</param>
        /// <param name="setNextColumnWidth">设置右边一个表格列的宽度</param>
        public void EditorSetWidth(float newWidth, bool logUndo, bool setNextColumnWidth)
        {
            // 获得最小的表格列宽度
            float minTableColumnWidth = GetDocumentViewOptions().MinTableColumnWidth;
            if (newWidth < minTableColumnWidth)
            {
                // 新表格列宽度小于最小值，在此进行修正
                newWidth = minTableColumnWidth;
            }
            DomTableElement table = this.OwnerTable;
            DomTableColumnElement nextCol = (DomTableColumnElement)
                table.Columns.GetNextElement(this);
            float totalWidth = this.Width;
            if (setNextColumnWidth)
            {
                if (nextCol != null)
                {
                    totalWidth = this.Width + nextCol.Width;
                    if (totalWidth - newWidth < minTableColumnWidth)
                    {
                        // 修改表格列的宽度导致右边的表格列宽度小于最小值，在此进行修正
                        newWidth = totalWidth - minTableColumnWidth;
                    }
                }
            }
            if (this.Width != newWidth)
            {
                float tableHeight = table.Height;
                table.InvalidateView();
                float oldWidth = this.Width;
                this.Width = newWidth;
                this.Modified = true;
                if (setNextColumnWidth)
                {
                    if (nextCol != null)
                    {
                        // 设置右边一列的宽度
                        nextCol.Width = totalWidth - newWidth;
                    }
                }
                if (logUndo && this.OwnerDocument.BeginLogUndo())
                {
                    var undo = new DCSoft.Writer.Dom.Undo.XTextUndoTableColumnWidth(
                        this,
                        oldWidth,
                        this.Width,
                        setNextColumnWidth);
                    this.OwnerDocument.AddUndo(undo);
                    this.OwnerDocument.EndLogUndo();
                }
                this.OwnerDocument.Modified = true;
                table.InnerExecuteLayout();
                table.InvalidateView();
                if (table.Height != tableHeight)
                {
                    // 表格高度发生改变，需要重新设置行状态和重新分页
                    table.UpdatePagesForTable(true);
                }
                if (this.OwnerDocument.EditorControl != null)
                {
                    // 更新光标位置
                    this.OwnerDocument.EditorControl.UpdateTextCaretWithoutScroll();
                }
            }
        }

        /// <summary>
        /// 作废
        /// </summary>
        public new float Height
        {
            get
            {
                if (this.OwnerTable == null)
                {
                    return 0;
                }
                else
                {
                    return this.OwnerTable.Height;
                }
            }
            set
            {
            }
        }

        /// <summary>
        /// 列号
        /// </summary>
        public int Index
        {
            get
            {
                DomTableElement table = (DomTableElement)this.Parent;
                if (table == null)
                {
                    return -1;
                }
                else
                {
                    return table.Columns.IndexOf(this);
                }
            }
        }
        /// <summary>
        /// 本表格列所属的表格对象
        /// </summary>
        public override DomTableElement OwnerTable
        {
            get
            {
                return (DomTableElement)this.Parent;
            }
        }

        /// <summary>
        /// 表格列所属的单元格对象列表
        /// </summary>
        public DomElementList Cells
        {
            get
            {
                DomElementList result = new DomElementList();
                DomTableElement table = this.OwnerTable;
                if (table != null)
                {
                    int index = table.Columns.IndexOf(this);
                    if (index >= 0)
                    {
                        foreach (DomTableRowElement row in table.Rows)
                        {
                            result.FastAdd2(row.Cells[index]);
                        }
                    }
                }
                return result;
            }
        }

        /// <summary>
        /// 内置的单元格元素列表,程序内部临时使用
        /// </summary>
        internal DomElementList InnerCells = null;

        public override void Dispose()
        {
            base.Dispose();
        }
    }
}
