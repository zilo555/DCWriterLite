
using System;
using System.Collections.Generic;
using System.Text;
using DCSoft.Common;
using DCSoft.Printing;
// // 
using DCSoft.Drawing;
using System.ComponentModel;
using DCSoft.Data;
using DCSoft.Writer.Data;
using System.Collections;
using DCSoft.Writer.Controls;

namespace DCSoft.Writer.Dom
{
    /// <summary>
    /// 表格元素
    /// </summary>
    /// <remarks>
    /// 本表格支持多行多列，支持横向和纵向合并单元格
    /// 编写  袁永福 2012-7-12</remarks>
#if !RELEASE
    [System.Diagnostics.DebuggerDisplay("表格{ID},{RowsCount}行,{ColumnsCount}列,宽{Width},高{Height}")]
#endif
    public sealed partial class DomTableElement : DomContainerElement
    {
        public static readonly string TypeName_XTextTableElement = "XTextTableElement";
        public override string TypeName => TypeName_XTextTableElement;

        /// <summary>
        /// 初始化对象
        /// </summary>
        public DomTableElement()
        {
        }

        internal override IList<DomElement> GetCompressedElements()
        {
            return this._Elements;
        }
        internal override void ResetChildElementStats()
        {
            base._ChildElementsNotCharOrParagraphFlag = null;
            base.ChildElementTypeStateReady = false;
        }
        internal override void CheckChildElementStatsReady()
        {
            base.ChildElementTypeStateReady = true;
            base.HasCharElement = false;
            base.HasParagraphFlagElement = false;
            base.HasChildObjectElement = false;
            if (this._Elements != null && this._Elements.Count > 0)
            {
                base._ChildElementsNotCharOrParagraphFlag = this._Elements;
                base.HasChildContainerElement = true;
            }
            else
            {
                base._ChildElementsNotCharOrParagraphFlag = null;
                base.HasChildContainerElement = false;
            }
        }
        public override ElementType RuntimeAcceptChildElementTypes()
        {
            return ElementType.TableRow | ElementType.TableColumn;
        }
#if !RELEASE
        public override string ToString()
        {
            return "表格" + this.ID;
        }
#endif
        /// <summary>
        /// 属性无效
        /// </summary>
        public override bool AcceptTab
        {
            get
            {
                return false;
            }
            set
            {

            }
        }

        internal override void ClearRuntimeStyle()
        {
            base.ClearRuntimeStyle();
            if (this._Columns != null)
            {
                foreach (var item in this._Columns.FastForEach())
                {
                    item._RuntimeStyle = null;
                }
            }
        }
        internal override void InnerFixDomStateFast()
        {
            var doc = this._OwnerDocument;
            if (this._Columns != null)
            {
                for (int iCount = this._Columns.Count - 1; iCount >= 0; iCount--)
                {
                    var col = (DomTableColumnElement)this._Columns[iCount];
                    col._ElementIndex = iCount;
                    col.InnerSetOwnerDocumentParentRaw(doc, this);
                }
            }
            if (this._Elements != null)
            {
                var rowArr = this._Elements.InnerGetArrayRaw();
                for (int iCount = this._Elements.Count - 1; iCount >= 0; iCount--)
                {
                    var row = (DomTableRowElement)rowArr[iCount];// this._Elements[iCount];
                    row.InnerSetOwnerDocumentParentRaw(doc, this);
                    row._ElementIndex = iCount;
                    if (row._Elements != null)
                    {
                        var cellArr = row._Elements.InnerGetArrayRaw();
                        for (int iCount2 = row._Elements.Count - 1; iCount2 >= 0; iCount2--)
                        {
                            var cell = (DomTableCellElement)cellArr[iCount2];// row._Elements[iCount2];
                            cell._RowIndex = iCount;
                            cell._ElementIndex = iCount2;
                            cell.InnerSetOwnerDocumentParentRaw(doc, row);
                            cell.InnerFixDomStateFast();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 属性无效
        /// </summary>
        public override string FormulaValue
        {
            get
            {
                return this.Text;
            }
            set
            {
            }
        }

        /// <summary>
        /// 是否包含未处理的分页标记
        /// </summary>
        internal override bool GetContainsUnHandledPageBreak()
        {
            var rowArr = this.RuntimeRows.InnerGetArrayRaw();
            for (var iCount = this.RuntimeRows.Count - 1; iCount >= 0; iCount--)
            {
                if (((DomTableRowElement)rowArr[iCount]).GetContainsUnHandledPageBreak())
                {
                    return true;
                }
            }
            //foreach (XTextTableRowElement row in this.RuntimeRows.FastForEach())
            //{
            //    if (row.GetContainsUnHandledPageBreak())
            //    {
            //        return true;
            //    }
            //}
            return base.GetContainsUnHandledPageBreak();

        }

        /// <summary>
        /// 文档元素编号前缀
        /// </summary>
        public override string ElementIDPrefix()
        {

            return "table";

        }

        /// <summary>
        /// 标题行集合
        /// </summary>
        public DomElement[] HeaderRows
        {
            get
            {
                var rows = this.RuntimeRows;
                int rowsCount = rows.Count;
                for (var iCount = 0; iCount < rowsCount; iCount++)
                {
                    if (((DomTableRowElement)rows[iCount]).HeaderStyle == false)
                    {
                        if (iCount > 0)
                        {
                            return rows.ToArrayRange(0, iCount);
                        }
                        return null;
                    }
                }
                return null;
            }
        }

        private DomElementList _RuntimeRows = null;
        /// <summary>
        /// 运行时的表格行对象列表
        /// </summary>
        public DomElementList RuntimeRows
        {
            get
            {
                if (this._RuntimeRows == null)
                {
                    this._RuntimeRows = this.Rows;
                    var rowsCount = this.Rows.Count;
                    var rowsArr = this.Rows.InnerGetArrayRaw();
                    for (var iCount = rowsCount - 1; iCount >= 0; iCount--)
                    {
                        var row = (DomTableRowElement)rowsArr[iCount];
                        if (row.RuntimeVisible == false)
                        {
                            this._RuntimeRows = new DomElementList(rowsCount);
                            for (var iCount2 = 0; iCount2 < rowsCount; iCount2++)
                            {
                                var row2 = (DomTableRowElement)rowsArr[iCount2];
                                if (row2.RuntimeVisible)
                                {
                                    this._RuntimeRows.SuperFastAdd(row2);
                                }
                            }
                            break;
                        }
                    }
                }
                return this._RuntimeRows;
            }
        }
        /// <summary>
        /// 对象所属文档对象
        /// </summary>
        public override DomDocument OwnerDocument
        {
            get
            {
                return base.ElementOwnerDocument();
            }
            set
            {
                base.OwnerDocument = value;
                if (this._Columns != null && this._Columns.Count > 0)
                {
                    var arr = this._Columns.InnerGetArrayRaw();
                    for (var iCount = this._Columns.Count - 1; iCount >= 0; iCount--)
                    {
                        arr[iCount].OwnerDocument = value;
                    }
                    //foreach (XTextTableColumnElement col in this.Columns.FastForEach())
                    //{
                    //    col.OwnerDocument = this.OwnerDocument;
                    //}
                }
            }
        }

        private DomElementList _Columns = new DomElementList();
        /// <summary>
        /// 表格列对象
        /// </summary>
        public DomElementList Columns
        {
            get
            {
                return _Columns;
            }
            set
            {
                _Columns = value;
            }
        }
        /// <summary>
        /// 表格行对象
        /// </summary>
        public DomElementList Rows
        {
            get
            {
                return this._Elements;
            }
            set
            {
                this._Elements = value;
            }
        }

        /// <summary>
        /// 表格行数
        /// </summary>
        public int RowsCount
        {
            get
            {
                if (this._Elements == null)
                {
                    return 0;
                }
                else
                {
                    return this._Elements.Count;
                }
            }
        }

        /// <summary>
        /// 子元素列表
        /// </summary>
        public override DomElementList Elements
        {
            get
            {
                return base.Elements;
            }
            set
            {
                base.Elements = value;
            }
        }

        /// <summary>
        /// 表格列数
        /// </summary>
        public int ColumnsCount
        {
            get
            {
                if (this._Columns == null)
                {
                    return 0;
                }
                else
                {
                    return this._Columns.Count;
                }
            }
        }

        /// <summary>
        /// 获得所有的文档元素对象，包括自己
        /// </summary>
        /// <returns>获得的元素列表</returns>
        public override DomElementList GetAllElements()
        {
            DomElementList list = base.GetAllElements();
            list.AddRangeByDCList(this.Columns);
            return list;
        }

        /// <summary>
        /// 获得所有的单元格对象
        /// </summary>
        public DomElementList Cells
        {
            get
            {
                var myCells = new DomElementList(this.RowsCount * this.ColumnsCount);
                var rows = this.RuntimeRows;
                int len = rows.Count;
                for (int iCount = 0; iCount < len; iCount++)
                {
                    var row = (DomTableRowElement)rows[iCount];
                    if (row._Elements != null)
                    {
                        myCells.AddRangeByDCList(row._Elements);
                    }
                }
                return myCells;

                //foreach (XTextTableRowElement myRow in this.RuntimeRows)
                //{
                //    if (myRow._Elements != null)
                //    {
                //        myCells.AddRangeRaw(myRow._Elements);
                //    }
                //    //foreach (XTextTableCellElement cell in myRow.Cells)
                //    //{
                //    //    myCells.AddRaw(cell);
                //    //}
                //}
                //return myCells;
            }
        }

        /// <summary>
        /// 获得所有可见的单元格对象
        /// </summary>
        public DomElementList VisibleCells
        {
            get
            {
                DomElementList result = new DomElementList();
                foreach (DomTableRowElement row in this.RuntimeRows.FastForEach())
                {
                    if (row.RuntimeVisible)
                    {
                        foreach (DomTableCellElement cell in row.Cells.FastForEach())
                        {
                            if (cell.RuntimeVisible)
                            {
                                result.FastAdd2(cell);
                            }
                        }
                    }
                }
                return result;
            }
        }

        /// <summary>
        /// 表格中第一个单元格
        /// </summary>
        public DomTableCellElement FirstCell
        {
            get
            {
                if (this.Rows.Count > 0)
                {
                    DomTableRowElement row = (DomTableRowElement)this.Rows[0];
                    if (row.Cells.Count > 0)
                    {
                        return (DomTableCellElement)row.Cells[0];
                    }
                }
                return null;
            }
        }

        /// <summary>
        /// 表格中第一个单元格
        /// </summary>
        public DomTableCellElement FirstVisibleCell
        {
            get
            {
                if (this.Rows.Count > 0)
                {
                    foreach (DomTableRowElement row in this.Rows.FastForEach())
                    {
                        if (row.RuntimeVisible)
                        {
                            foreach (DomTableCellElement cell in row.Cells.FastForEach())
                            {
                                if (cell.RuntimeVisible)
                                {
                                    return cell;
                                }
                            }
                        }
                    }
                }
                return null;
            }
        }

        /// <summary>
        /// 获得最后一个可见的单元格对象
        /// </summary>
        public DomTableCellElement LastVisibleCell
        {
            get
            {
                for (int iCount = this.Rows.Count - 1; iCount >= 0; iCount--)
                {
                    DomTableRowElement row = (DomTableRowElement)this.Rows[iCount];
                    if (row.RuntimeVisible)
                    {
                        for (int iCount2 = row.Cells.Count - 1; iCount2 >= 0; iCount2--)
                        {
                            DomTableCellElement cell = (DomTableCellElement)row.Cells[iCount2];
                            if (cell.RuntimeVisible)
                            {
                                return cell;
                            }
                        }
                    }
                }
                return null;
            }
        }

        /// <summary>
        /// 第一个在正文中的内容元素
        /// </summary>
        public override DomElement FirstContentElementInPublicContent
        {
            get
            {
                DomTableCellElement cell = this.FirstCell;
                if (cell != null)
                {
                    return cell.FirstContentElementInPublicContent;
                }
                return null;
            }
        }

        /// <summary>
        /// 最后一个在文档正文中的内容元素
        /// </summary>
        public override DomElement LastContentElementInPublicContent
        {
            get
            {
                DomTableCellElement cell = this.LastVisibleCell;
                if (cell != null)
                {
                    return cell.LastContentElementInPublicContent;
                }
                return null;
            }
        }

        /// <summary>
        /// 第一个内容元素
        /// </summary>
        public override DomElement FirstContentElement
        {
            get
            {
                return this;
            }
        }

        /// <summary>
        /// 最后一个内容元素
        /// </summary>
        public override DomElement LastContentElement
        {
            get
            {
                return this;
            }
        }


        /// <summary>
        /// 获得输入焦点
        /// </summary>
        public override void Focus()
        {
            DomTableCellElement cell = this.FirstCell;
            if (cell != null)
            {
                if (cell.OverrideCell != null)
                {
                    cell = cell.OverrideCell;
                }
                DomElement firstElement = cell.PrivateContent.FirstElement;
                if (firstElement != null)
                {
                    DomDocumentContentElement dce = this.DocumentContentElement;
                    if (this.OwnerDocument.CurrentContentElement != dce)
                    {
                        dce.Focus();
                    }
                    dce.SetSelection(firstElement.ContentIndex, 0);
                }
            }
        }

        /// <summary>
        /// 表格中是否有内容被选择
        /// </summary>
        public override bool HasSelection
        {
            get
            {

                DCSelection selection = this.DocumentContentElement?.Selection;
                if (selection == null)
                {
                    return false;
                }
                if (selection.Cells != null && selection.Cells.Count > 0)
                {
                    foreach (DomTableCellElement cell in selection.Cells.FastForEach())
                    {
                        if (cell.OwnerTable == this)
                        {
                            return true;
                        }
                    }
                }
                DomTableCellElement fc = this.FirstVisibleCell;
                DomTableCellElement lc = this.LastVisibleCell;
                DomElement first = fc == null ? null : fc.FirstContentElement;
                DomElement last = lc == null ? null : lc.LastContentElement;
                if (first == null || last == null)
                {
                    return false;
                }
                DomDocumentContentElement ce = this.DocumentContentElement;

                if (ce != null && ce.Selection != null)
                {
                    int start = ce.Selection.AbsStartIndex;
                    int end = ce.Selection.AbsEndIndex;
                    if (first.ContentIndex <= end && last.ContentIndex >= start)
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        /// <summary>
        /// 表格套嵌层次，文档中第一层表格的层次为1，子表格为2,再内部的子表格为3，以此类推.
        /// </summary>
        public int NeastLevel
        {
            get
            {
                int level = 0;
                DomElement element = this;
                while (element != null)
                {
                    if (element is DomTableElement)
                    {
                        level++;
                    }
                    element = element.Parent;
                }
                return level;
            }
        }


        /// <summary>
        /// 设置文档元素为选中
        /// </summary>
        public override bool Select()
        {
            DomTableCellElement fc = null;
            foreach (DomTableRowElement row in this.RuntimeRows)
            {
                if (row.RuntimeVisible)
                {
                    fc = (DomTableCellElement)row.FirstChild;
                    if (fc.IsOverrided)
                    {
                        fc = fc.OverrideCell;
                    }
                    break;
                }
            }
            DomTableCellElement lc = null;
            for (var iCount = this.RuntimeRows.Count - 1; iCount >= 0; iCount--)
            {
                var row = (DomTableRowElement)this.RuntimeRows[iCount];
                if (row.RuntimeVisible)
                {
                    lc = (DomTableCellElement)row.LastChild;
                    if (lc.IsOverrided)
                    {
                        lc = lc.OverrideCell;
                    }
                    break;
                }
            }
            if (fc != null && lc != null)
            {
                DomElement fe = fc.FirstContentElement;
                DomElement le = lc.LastContentElement;
                if (fe != null && le != null)
                {
                    this.OwnerDocument.ClearPagesClientSelectionBounds();
                    return this.DocumentContentElement.SetSelectionRange(fe.ContentIndex, le.ContentIndex);
                }
            }
            return false;
        }

        /// <summary>
        /// 创建新的文档对象，使其包含本文档元素的复制品
        /// </summary>
        /// <param name="includeThis">是否包含本文档原始对象,对表格对象该参数无意义</param>
        /// <returns>创建的文档对象</returns>
        public override DomDocument CreateContentDocument(bool includeThis)
        {
            return base.CreateContentDocument(true);
        }

        /// <summary>
        /// 修正分页线位置
        /// </summary>
        /// <param name="info">分页线位置信息</param>
        internal void FixPageLine(PageLineInfo info)
        {
            float tableAbsTop = this.GetAbsTop();
            DomTableRowElement currentRow = null;
            var runtimeRowsArr = this.RuntimeRows.InnerGetArrayRaw();
            var runtimeRowCount = this.RuntimeRows.Count;
            for (var rowIndex = 0; rowIndex < runtimeRowCount; rowIndex++)
            {
                var row = (DomTableRowElement)runtimeRowsArr[rowIndex];
                if (info._CurrentPoistion >= tableAbsTop + row.Top)
                {
                    if (row.NewPage
                        && row.HandledNewPage == false
                        && row.HeaderStyle == false)
                    {
                        // 表格行强制分页
                        if (tableAbsTop + row.Top < info.LastPosition + 100)
                        {
                            // 在这个极端情况下不适合强制分页
                            continue;
                        }
                        row.HandledNewPage = true;
                        info.SourceElement = row;
                        info.CurrentPoistion = row.GetAbsTop();
                        info.ForNewPage = true;
                        if (rowIndex > 0)
                        {
                            AddHeaderRows(info);
                        }
                        return;
                    }
                    if (row.GetContainsUnHandledPageBreak())
                    {
                        // 表格行中存在未处理的分页符
                        foreach (DomTableCellElement cell in row.Cells)
                        {
                            if (cell.RuntimeVisible)
                            {
                                if (cell.GetContainsUnHandledPageBreak())
                                {
                                    // 单元格中存在未处理的分页符，立即处理
                                    cell.FixPageLine(info);
                                    AddHeaderRows(info);
                                    return;
                                }
                            }
                        }
                    }
                    currentRow = row;
                    if (row.CanSplitByPageLine == false)
                    {
                        if (info.CurrentPoistion < tableAbsTop + row.Top + row.MaxCellHeight())
                        {
                            break;
                        }
                    }
                }
                else
                {
                    break;
                }
            }//foreach
            if (currentRow == null)
            {
                // 没有命中任何表格行，返回原值
                return;
            }
            if (info.CurrentPoistion >= tableAbsTop + this.Height - 1)
            {
                // 分页线超过整个表格的下边缘，则退出。
                return;
            }
            var currentRowAbsTop = tableAbsTop + currentRow.Top;
            if (currentRow.CanSplitByPageLine == false)
            {
                // 表格行限制为不能跨页
                float dis = currentRowAbsTop - info.LastPosition;
                if ((dis / info.StdPageContentHeight) > 0.2)
                {
                    // 表格行顶端位置距离上一个分页先线的距离不算近。
                    info.CurrentPoistion = currentRowAbsTop;
                }
                if (this.OwnerDocument.InnerGlobalGridInfo() != null)
                {
                    // 出现全局性的文档网格线，则进行特别处理
                    var info3 = this.OwnerDocument.InnerGlobalGridInfo();
                    if (info3.RuntimeGridSpan > 0)
                    {
                        info.CurrentPoistion = (float)Math.Round(info.CurrentPoistion / info3.RuntimeGridSpan) * info3.RuntimeGridSpan;
                    }
                }
                info.AddCutLineCell(currentRow);
                info.SourceElements.Add(currentRow);
                info.SourceElement = currentRow;
                AddHeaderRows(info);
                return;
            }
            // 当前分页线的位置距离表格行顶端的距离
            float distance = info.CurrentPoistion - currentRowAbsTop;
            // 最小允许的距离
            float minDistance = this.OwnerDocument.PixelToDocumentUnit(10);
            if (info.GetHeaderRows(this) != null)// this.HasHeaderRow())
            {
                // 本表格有标题行，minDistance需要适当的放大
                minDistance = minDistance * 2;
            }
            if (distance > minDistance)
            {
                // List<XTextTableCellElement> handledCells = new List<XTextTableCellElement>();
                // 获得要处理的单元格
                var currentCells = new DCList<DomElement>(currentRow.Cells.Count);
                foreach (DomTableCellElement cell in currentRow.Cells.FastForEach())
                {
                    if (cell.RuntimeVisible)
                    {
                        if (cell.HasPrivateLines() == false)//.PrivateLines == null || cell.PrivateLines.Count == 0)
                        {
                            continue;
                        }
                        var firstLine = cell.PrivateLines[0];
                        if (firstLine.Top + currentRowAbsTop > info.CurrentPoistion)
                        {
                            continue;
                        }
                        currentCells.SuperFastAdd(cell);
                    }
                    else if (cell.IsOverrided)
                    {
                        if (currentCells.Contains(cell.OverrideCell) == false && cell.OverrideCell.HasPrivateLines())
                        {
                            currentCells.SuperFastAdd(cell.OverrideCell);
                        }
                    }
                }//foreach
                if (currentCells.Count == 0)
                {
                    // 没有要处理的单元格对象
                    return;
                }
                // 获得剩余空白高度最小的表格行对象
                DomTableCellElement minBlankCell = null;
                float minBlankHeight = float.NaN;
                foreach (DomTableCellElement cell in currentCells.FastForEach())
                {
                    if (cell.HasPrivateLines())//.PrivateLines == null || cell.PrivateLines.Count == 0)
                    {
                        DomLine line = cell.PrivateLines.LastLine;
                        float crp = 0;
                        if (cell.ZeroRuntimePaddingTopBottom == false)
                        {
                            crp = cell.RuntimeStyle.PaddingTop;
                        }
                        float h = Math.Max(0, crp + cell.ClientHeight - line.Bottom);
                        if (minBlankCell == null || h < minBlankHeight)
                        {
                            minBlankCell = cell;
                            minBlankHeight = h;
                        }
                    }
                }
                info.UpContentTop = false;
                minBlankCell.FixPageLine(info);
                if (info.UpContentTop)
                {
                    // 分页线在所有内容的上方
                    {
                        foreach (DomTableCellElement cell in currentCells.FastForEach())
                        {
                            if (cell == minBlankCell || cell.HasPrivateLines() == false)
                            {
                                continue;
                            }
                            //对其他单元格进行一次分页处理，减少错误概率。
                            if (cell.OwnerRow == currentRow)
                            {
                                float ct = currentRowAbsTop + cell.PrivateLines.FirstLine.Top;
                                float cb = currentRowAbsTop + cell.PrivateLines.LastLine.Bottom;
                                if (ct <= info.CurrentPoistion && info.CurrentPoistion <= cb)
                                {
                                    cell.FixPageLine(info);
                                    break;
                                }
                            }
                        }
                    }
                    AddHeaderRows(info);
                    info.AddCutLineCell(currentRow);
                    return;
                }
                bool handedFlag = false;
                if (info.UnderContentBottom)
                {
                    // 在单元格所有正文的下面，考虑其他换页
                    if (currentCells.Count == 1)
                    {
                        // 只有一个单元格参与分页
                        if (minBlankCell.PrivateLines.Count > 1)
                        {
                            var line = minBlankCell.PrivateLines.LastLine;
                            var pos9 = currentRowAbsTop + line.Top;
                            if (info.CurrentPoistion < pos9 + 200)
                            {
                                info.CurrentPoistion = pos9;
                            }
                            info.SourceElement = currentRow;
                            handedFlag = true;
                        }
                    }
                    if (handedFlag == false)
                    {
                        if (currentRow.Height < info.StdPageContentHeight * 0.08)
                        {
                            // 当前表格行高度不算高，直接提升分页线位置到表格行的位置
                            info.CurrentPoistion = currentRowAbsTop;
                            info.SourceElement = currentRow;
                            handedFlag = true;
                        }
                    }
                }
                if (handedFlag == false)
                {
                    // 对于特别突出的单独列做处理
                    DomTableCellElement singleCell = null;
                    foreach (DomTableCellElement cell in currentCells)
                    {
                        if (cell != minBlankCell && cell.Parent == currentRow)
                        {
                            var firstLine = cell.PrivateLines[0];
                            if (firstLine.Top + currentRowAbsTop < info.CurrentPoistion)
                            {
                                if (singleCell == null)
                                {
                                    singleCell = cell;
                                }
                                else
                                {
                                    // 不是单独列，不处理
                                    singleCell = null;
                                    break;
                                }
                            }
                        }
                    }
                    if (singleCell != null)
                    {
                        var info2 = info.Clone();
                        info.SourceElement = null;
                        info.SourceElements = new DomElementList();
                        singleCell.FixPageLine(info2);
                        if (info2.CurrentPoistion < info.CurrentPoistion
                            && info2.CurrentPoistion > info.CurrentPoistion - 300)
                        {
                            // 发生比较小的差距
                            if (singleCell.PrivateLines[0].Top + currentRowAbsTop >= info2.CurrentPoistion - 1)
                            {
                                if (singleCell != minBlankCell && minBlankCell != null)
                                {
                                    minBlankCell.FixPageLine(info2);
                                    info.CurrentPoistion = info2.CurrentPoistion;
                                    info.SourceElement = minBlankCell;
                                }
                            }
                            else
                            {
                                var dis = info.CurrentPoistion - info2.CurrentPoistion;
                                var lastLine = singleCell.PrivateLines.LastElement;
                                if (singleCell.ClientHeight - lastLine.Bottom > dis)
                                {
                                    // 可以修补,则整体移动
                                    foreach (var line in singleCell.PrivateLines)
                                    {
                                        line.OffsetPosition(0, dis);
                                    }
                                }
                                else
                                {
                                    info.CurrentPoistion = info2.CurrentPoistion;
                                    info.SourceElement = singleCell;
                                }
                            }
                            handedFlag = true;
                        }
                    }
                }
                if (handedFlag == false && info.UnderContentBottom)
                {
                    var dis99 = currentRowAbsTop + currentRow.Height - info.CurrentPoistion;
                    if (dis99 >= 2 && dis99 < 20)
                    {
                        // 分页线和表格行的下边缘存在不可忽视的距离，
                        // 分页线又不能向下移动到表格行下边缘，只能提前换行来改善分页显示效果。
                        info.CurrentPoistion = minBlankCell.PrivateLines.LastElement.AbsBottom - 2;
                        minBlankCell.FixPageLine(info);
                    }
                }
                if (handedFlag == false)
                {
                    if (info.SourceElement == null)
                    {
                        info.SourceElement = currentRow;
                    }
                }
                AddHeaderRows(info);
                info.AddCutLineCell(currentRow);
                return;
            }//if
            else
            {
                //var bolHasOverried = false;
                // 追加被跨行的单元格对象
                for (int iCount = 0; iCount < currentRow.Cells.Count; iCount++)
                {
                    DomTableCellElement cell = (DomTableCellElement)currentRow.Cells[iCount];
                    if (cell.IsOverrided)
                    {
                        cell = cell.OverrideCell;
                    }
                    if (cell.OwnerRow != currentRow)
                    {
                        if (info.SourceElements.Contains(cell) == false)
                        {
                            info.SourceElements.Add(cell);
                        }
                        //bolHasOverried = true;
                    }
                }//foreach
                if (info.CurrentPoistion > tableAbsTop + currentRow.Top + currentRow.Height)
                {
                    info.CurrentPoistion = (tableAbsTop + currentRow.Top + currentRow.Height);
                    info.SourceElement = this.RuntimeRows.GetNextElement(currentRow);
                    foreach (DomTableCellElement cell in currentRow.Cells)
                    {
                        if (cell.IsOverrided)
                        {
                            var cell2 = cell.OverrideCell;
                            if (cell2.RuntimeVisible && cell.RowIndex + cell.RowSpan < cell2.RowIndex + cell2.RowSpan)
                            {
                                info.AddCutLineCell(cell2);
                            }
                        }
                    }
                }
                else
                {
                    info.CurrentPoistion = (tableAbsTop + currentRow.Top);// currentRow.AbsTop;
                    info.SourceElement = currentRow;
                    foreach (DomTableCellElement cell in currentRow.Cells)
                    {
                        if (cell.IsOverrided)
                        {
                            var cell2 = cell.OverrideCell;
                            if (cell2.RuntimeVisible && cell.RowIndex > cell2.RowIndex)
                            {
                                info.AddCutLineCell(cell2);
                            }
                        }
                    }
                }
            }
            if (info.SourceElement == null)
            {
                info.SourceElement = currentRow;
            }
            AddHeaderRows(info);
        }

        private void AddHeaderRows(PageLineInfo info)
        {
            var vheaderRows = info.GetHeaderRows(this);
            if (vheaderRows != null)
            {
                DomElement element2 = info.SourceElement;
                while (element2 != null)
                {
                    if (element2.Parent == this)
                    {
                        break;
                    }
                    element2 = element2.Parent;
                }
                var rows = this.RuntimeRows;
                if (rows.Contains(element2))
                {
                    // 插入临时的标题行
                    DomTableRowElement row = (DomTableRowElement)element2;
                    if (rows.IndexOf(row)
                        > rows.IndexOf(vheaderRows[vheaderRows.Length - 1]))
                    {
                        // 添加标题行
                        info.AddHeaderRowRange(vheaderRows);
                    }//if
                }//if
            }//if
        }

        /// <summary>
        /// 绘制对象内容
        /// </summary>
        /// <param name="args">参数</param>
        public override void Draw(InnerDocumentPaintEventArgs args)
        {
            if (args.RenderMode == InnerDocumentRenderMode.Print
                && args.PrintTaskOptions != null
                && args.PrintTaskOptions.PrintTableCellBorder == false)
            {
                // 打印选项中指明不打印表格边框线
                InnerDrawTable(args, false, false, true);
                return;
            }
            InnerDrawTable(args, true, true, true);
        }


        /// <summary>
        /// 内部的绘制对象内容
        /// </summary>
        /// <param name="args_InnerDocumentPaintEventArgs">参数</param>
        /// <param name="argDrawTableBorder">是否绘制表格边框</param>
        /// <param name="argDrawCellBorder">是否绘制单元格边框</param>
        /// <param name="argDrawCellContent">是否绘制单元格内容</param>
        private void InnerDrawTable(
            object args_InnerDocumentPaintEventArgs,
            bool argDrawTableBorder,
            bool argDrawCellBorder,
            bool argDrawCellContent)
        {
            var document = this.OwnerDocument;
            var vopts = document.GetDocumentViewOptions();
            var args = (InnerDocumentPaintEventArgs)args_InnerDocumentPaintEventArgs;
            if (args.ClipRectangle.IsEmpty)
            {
                // 参数状态不对，退出
                return;
            }
            var thisDocumentContentElement = this.DocumentContentElement;
            if (thisDocumentContentElement == null)
            {
                // 状态不对，不处理了。
                return;
            }
            RuntimeDocumentContentStyle tableStyle = this.RuntimeStyle;
            float leftFix = this.GetAbsLeft() + tableStyle.PaddingLeft;
            float topFix = this.GetAbsTop() + tableStyle.PaddingTop;
            //bool localDrawCellBorder = true;
            //bool localDrawCellBackground = true;
            RectangleF clipRectangle = args.ClipRectangle;
            LineShapeCounter lineShapes = new LineShapeCounter();
            PageContentPartyStyle partyStyle = thisDocumentContentElement.PagePartyStyle;
            // 是否只绘制表格内容，不绘制边框线和单元格背景
            bool drawContentOnly = false;
            var docOpts = document.GetDocumentOptions();
            bool hasVisibleGlobalGridline = document._GlobalGridInfo != null
                && document._GlobalGridInfo.IsTransparent() == false;
            if (hasVisibleGlobalGridline && document.GetDocumentViewOptions().ShowGlobalGridLineInTableAndSection == false)
            {
                hasVisibleGlobalGridline = false;
            }
            var pageClipRectangle = args.PageClipRectangle;
            var pageClipRectangleIsEmpty = args.PageClipRectangle.IsEmpty;
            //var clipRectangleIsEmpty = args.ClipRectangle.IsEmpty;

            if (tableStyle.HasVisibleBorder && drawContentOnly == false && argDrawTableBorder)
            {
                // 绘制表格边框
                RectangleF bb = this.GetAbsBounds();
                if (pageClipRectangleIsEmpty == false)
                {
                    bb = RectangleF.Intersect(bb, pageClipRectangle);
                }
                if (bb.IsEmpty == false)
                {
                    if (hasVisibleGlobalGridline
                        && tableStyle.BorderColor == Color.Black
                        && tableStyle.BorderWidth == 1
                        && tableStyle.BorderStyle == DashStyle.Solid)
                    {
                        lineShapes.AddBorder(
                            //lineShapes.AddBorderForDebug("table:" + this.ID,
                            bb,
                            tableStyle.BorderLeft,
                            false,
                            tableStyle.BorderRight,
                            false,
                            tableStyle.BorderColor,
                            tableStyle.BorderWidth,
                            tableStyle.BorderStyle,
                            -1,
                            false);
                    }
                    else
                    {
                        lineShapes.AddBorder(
                            //lineShapes.AddBorderForDebug("table:" + this.ID,
                            bb,
                            tableStyle.BorderLeft,
                            tableStyle.BorderTop,
                            tableStyle.BorderRight,
                            tableStyle.BorderBottom,
                            tableStyle.BorderColor,
                            tableStyle.BorderWidth,
                            tableStyle.BorderStyle,
                            -1,
                            false);
                    }
                    //args.Render.RenderBorder(this, args, bb);
                }
            }
            var noneBorderColor = vopts.NoneBorderColor;
            var selection = thisDocumentContentElement.Selection;
            var runtimeShowCellNoneBorder = args.ViewOptions.ShowCellNoneBorder;
            // 完成绘制的单元格
            List<DomTableCellElement> drawedCell = new List<DomTableCellElement>();
            var tableAbsPosition = this.AbsPosition;
            var runtimeRows = this.RuntimeRows;
            var runtimeRowsCount = runtimeRows.Count;
            var runtimeRowArray = runtimeRows.InnerGetArrayRaw();
            //foreach (XTextTableRowElement row in this.RuntimeRows.FastForEach())
            for (var rowIndex = 0; rowIndex < runtimeRowsCount; rowIndex++)
            {
                var row = (DomTableRowElement)runtimeRowArray[rowIndex];
                if (row.RuntimeVisible == false)
                {
                    // 表格行不可见
                    continue;
                }
                if (row.Height <= 0)
                {
                    // 高度为0
                    continue;
                }
                RectangleF rowAbsBounds = row.CreateAbsBounds(leftFix, topFix);
                bool firstRowInPage = false;
                if (pageClipRectangleIsEmpty == false)
                {
                    if (rowAbsBounds.Top <= pageClipRectangle.Top - 5)
                    {
                        firstRowInPage = true;
                    }
                }
                //if (clipRectangleIsEmpty == false)
                //{
                // 判断表格行是否在剪切矩形中
                if (rowAbsBounds.Top > clipRectangle.Bottom)
                {
                    // 表格行的顶端位置低于剪切矩形，则剩下的表格行必然不会
                    // 显示，因此退出循环
                    break;
                }
                //// 获得表格行中所有的单元格的最大高度
                //float maxCellHeight = 0;
                //foreach (XTextTableCellElement cell in row.Cells)
                //{
                //    if (cell.IsOverrided == false)
                //    {
                //        maxCellHeight = Math.Max(maxCellHeight, cell.Height);
                //    }
                //}
                float vh = rowAbsBounds.Top + row.MaxCellHeight() - clipRectangle.Top;
                if (vh < 1.5)
                {
                    if (args.RenderMode == InnerDocumentRenderMode.Print)
                    {
                        // 处于打印模式下，可显示的高度不够，则不处理本表格行
                        continue;
                    }
                }
                if (vh < -2)
                {
                    // 表格行的低端尚未达到剪切矩形，进行下一个表格行的判断
                    continue;
                }
                //if (rowBounds.Top + row.MaxCellHeight < clipRectangle.Top - 2)
                //{
                //    continue;
                //}
                //}
                if (args.RenderMode == InnerDocumentRenderMode.Print)
                {
                    if (clipRectangle.Bottom - rowAbsBounds.Top < 2)
                    {
                        continue;
                    }
                    //this.InnerPrintedFlag = true;
                    //row.InnerPrintedFlag = true;
                }
                args.Cancel = false;
                args.ViewBounds = rowAbsBounds;// row.GetAbsBounds();
                args.ClientViewBounds = row.RuntimeStyle.GetClientRectangleF(rowAbsBounds);//row.AbsClientBounds;
                args.Element = row;
                var rowAbsTop = tableAbsPosition.Y + row.Top;
                // 绘制单元格
                var rowCells = row.Cells;
                var rowCellsCount = rowCells.Count;
                var rowCellArray = rowCells.InnerGetArrayRaw();
                //foreach (XTextTableCellElement cell in row.Cells.FastForEach())
                for (var vcellIndex = 0; vcellIndex < rowCellsCount; vcellIndex++)
                {
                    var cell = (DomTableCellElement)rowCellArray[vcellIndex];// rowCells.GetItemFast(cellIndex);
                    if (cell._Width == 0)
                    {
                        continue;
                    }
                    if (cell.RuntimeVisible == false)
                    {
                        continue;
                    }
                    var cellRuntimeStyle = cell.RuntimeStyle;
                    args.Element = cell;
                    args.Style = cellRuntimeStyle;
                    var nativeCellAbsBounds = cell.CreateAbsBounds(tableAbsPosition.X, rowAbsTop);
                    //new RectangleF(
                    //tableAbsPosition.X + cell.Left,
                    //rowAbsTop + cell.Top,
                    //cell.Width,
                    //cell.Height);
                    RectangleF cellBounds = nativeCellAbsBounds;
                    RectangleF cellBorderBounds = cellBounds;
                    //RectangleF backBounds = cellBounds;
                    //if (clipRectangleIsEmpty == false)
                    //{
                    var backBounds = RectangleF.Intersect(cellBounds, clipRectangle);
                    //}
                    if (backBounds.Width <= 0 || backBounds.Height <= 0)
                    {
                        continue;
                    }
                    // 绘制表格背景
                    {
                        if ((partyStyle == PageContentPartyStyle.Body || partyStyle == PageContentPartyStyle.HeaderRows)
                            && pageClipRectangleIsEmpty == false)
                        {
                            // 根据页面边框来修正单元格的边框
                            RectangleF prect = pageClipRectangle;
                            float top2 = Math.Min(prect.Bottom, args.PageBottom);
                            prect.Height = top2 - prect.Top;
                            if (cellBounds.Bottom < prect.Top + 1
                                || cellBounds.Top > prect.Bottom - 1)
                            {
                                // 单元格不在页面范围中，不绘制。
                                continue;
                            }
                            //if (args.RenderMode == DocumentRenderMode.Print)
                            //{
                            //}

                            if (cellBounds.Y < prect.Y + 1)
                            {
                                cellBounds.Height = cellBounds.Bottom - prect.Y - 1;
                                cellBounds.Y = prect.Y + 1;
                            }
                            if (cellBounds.Bottom > prect.Bottom - 1)
                            {
                                cellBounds.Height = prect.Bottom - 1 - cellBounds.Y;
                            }
                            cellBorderBounds = cellBounds;
                            //cellBounds = RectangleF.Intersect(cellBounds, args.PageClipRectangle);
                        }
                    }
                    //PointF[] ps = new PointF[]
                    //    {
                    //        cellBounds.Location , 
                    //        new PointF( cellBounds.Right , cellBounds.Bottom ) 
                    //    };

                    //    args.GraphicsTransformPoints(ps);

                    //if (args.RenderMode == InnerDocumentRenderMode.Print)
                    //{
                    //    cell.InnerPrintedFlag = true;
                    //}
                    args.ViewBounds = nativeCellAbsBounds;
                    args.ClientViewBounds = cellRuntimeStyle.GetClientRectangleF(nativeCellAbsBounds);// cell.AbsClientBounds;
                    if (args.IsPrintRenderMode)
                    {
                        document.AddDrawContentBounds(cellBounds);
                    }
                    bool drawRowCellBackground = true;
                    bool hasVisibleGlobalGridlineForCell = hasVisibleGlobalGridline;
                    //if(cell.CellID == "E1")
                    //{
                    //    var s = 1;
                    //}
                    if (drawRowCellBackground && drawContentOnly == false)
                    {
                        args.Graphics.ResetClip();
                        // 绘制单元格背景
                        if (args.Render.DrawCellBackground(
                            cell,
                            args,
                            cellBounds))
                        {
                            hasVisibleGlobalGridlineForCell = false;
                        }
                    }
                    // 绘制图形文档
                    //Region clipBack = args.GraphicsClip;
                    //if (clipBack != null)
                    //{
                    //    //clipBack = clipBack.Clone();
                    //}
                    args.Graphics.SetClip(
                        new RectangleF(
                            cellBounds.Left - 4,
                            cellBounds.Top - 4,
                            cellBounds.Width + 8,
                            cellBounds.Height + 8));
                    //if(clipBack == args.Graphics.Clip )
                    //{

                    //}
                    {
                        bool back = args.EnabledDrawGridLine;
                        args.EnabledDrawGridLine = args.EnabledDrawGridLine && drawContentOnly == false;
                        try
                        {
                            if (args.RenderMode == InnerDocumentRenderMode.Paint)
                            {
                                if (cellBounds.Height > 3)
                                {
                                    // 当在用户界面上绘制单元格内容时，当绘制高度大于3才绘制单元格内容
                                    cell.InnerDrawContent(args, drawContentOnly);
                                    //cell.DrawContent(args);
                                }
                            }
                            else
                            {
                                cell.InnerDrawContent(args, drawContentOnly);
                                //cell.DrawContent(args);
                            }
                        }
                        finally
                        {
                            args.EnabledDrawGridLine = back;
                        }
                    }
                    args.Graphics.ResetClip();
                    bool drawRowCellBorder = true;
                    if (argDrawCellBorder)
                    {
                    }
                    else
                    {
                        drawRowCellBorder = false;
                    }
                    if (drawRowCellBorder && drawContentOnly == false)
                    {
                        float borderWidth = cellRuntimeStyle.BorderWidth;
                        // 运行模式下的代码 ***********************************
                        if (borderWidth > 0)
                        {
                            int lineLevel = 0;
                            if (cellRuntimeStyle.BorderLeft)
                            {
                                lineShapes.AddLine(
                                    cellBounds.Left,
                                    cellBounds.Top,
                                    cellBounds.Left,
                                    cellBounds.Bottom,
                                    cellRuntimeStyle.BorderLeftColor,
                                    cellRuntimeStyle.BorderWidth,
                                    cellRuntimeStyle.BorderStyle,
                                    lineLevel,
                                    false);
                            }
                            if (cellRuntimeStyle.BorderTop
                                && (hasVisibleGlobalGridlineForCell == false || firstRowInPage || row.HeaderStyle == true))
                            {
                                lineShapes.AddLine(
                                    cellBounds.Left,
                                    cellBounds.Top,
                                    cellBounds.Right,
                                    cellBounds.Top,
                                    cellRuntimeStyle.BorderTopColor,
                                    cellRuntimeStyle.BorderWidth,
                                    cellRuntimeStyle.BorderStyle,
                                    lineLevel,
                                    false);
                            }
                            if (cellRuntimeStyle.BorderRight)
                            {
                                lineShapes.AddLine(
                                    cellBounds.Right,
                                    cellBounds.Top,
                                    cellBounds.Right,
                                    cellBounds.Bottom,
                                    cellRuntimeStyle.BorderRightColor,
                                    cellRuntimeStyle.BorderWidth,
                                    cellRuntimeStyle.BorderStyle,
                                    lineLevel,
                                    false);
                            }
                            if (cellRuntimeStyle.BorderBottom
                                && (hasVisibleGlobalGridlineForCell == false || row.HeaderStyle == true))
                            {
                                lineShapes.AddLine(
                                    cellBounds.Left,
                                    cellBounds.Bottom,
                                    cellBounds.Right,
                                    cellBounds.Bottom,
                                    cellRuntimeStyle.BorderBottomColor,
                                    cellRuntimeStyle.BorderWidth,
                                    cellRuntimeStyle.BorderStyle,
                                    lineLevel,
                                    false);
                            }
                        }

                    }
                    if (args.RenderMode == InnerDocumentRenderMode.Paint)
                    {
                        if (runtimeShowCellNoneBorder && drawContentOnly == false)// this.OwnerDocument.Options.ViewOptions.ShowCellNoneBorder)
                        {
                            RuntimeDocumentContentStyle cr = cellRuntimeStyle;
                            if (cr.BorderLeft == false
                                || cr.BorderTop == false
                                || cr.BorderRight == false
                                || cr.BorderBottom == false
                                || cr.BorderWidth == 0)
                            {
                                bool drawLeft = true;
                                bool drawTop = true;
                                bool drawRight = true;
                                bool drawBottom = true;
                                DomTableCellElement leftCell = (DomTableCellElement)row.Cells.GetPreElement(cell);
                                if (leftCell != null && leftCell.OverrideCell != null)
                                {
                                    leftCell = leftCell.OverrideCell;
                                }
                                if (leftCell != null && drawedCell.Contains(leftCell))
                                {
                                    RuntimeDocumentContentStyle crLeft = leftCell.RuntimeStyle;
                                    if (crLeft.BorderRight
                                        && crLeft.BorderWidth > 0
                                        && crLeft.BorderLeftColor.A != 0)
                                    {
                                        drawLeft = false;
                                    }
                                }
                                DomTableCellElement topCell = this.GetCell(
                                    cell.RowIndex - 1,
                                    cell.ColIndex,
                                    false);
                                if (topCell != null)
                                {
                                    if (topCell.OverrideCell != null)
                                    {
                                        topCell = topCell.OverrideCell;
                                    }
                                }
                                if (topCell != null && drawedCell.Contains(topCell))
                                {
                                    RuntimeDocumentContentStyle crTop = topCell.RuntimeStyle;
                                    if (crTop.BorderBottom
                                        && crTop.BorderWidth > 0
                                        && crTop.BorderTopColor.A != 0)
                                    {
                                        drawTop = false;
                                    }
                                }
                                //if (drawLeft && drawTop && drawRight && drawBottom)
                                //{
                                //    args.Graphics.DrawRectangle(
                                //        pen,
                                //        cellBounds.Left,
                                //        cellBounds.Top,
                                //        cellBounds.Width,
                                //        cellBounds.Height);
                                //}
                                //else
                                if (hasVisibleGlobalGridlineForCell && row.HeaderStyle == false)
                                {
                                    // 存在全局网格线时不绘制上下边框线
                                    if (firstRowInPage == false)
                                    {
                                        drawTop = false;
                                    }
                                    drawBottom = false;
                                }
                                lineShapes.AddBorder(
                                   cellBorderBounds,
                                   drawLeft,
                                   drawTop,
                                   drawRight,
                                   drawBottom,
                                   noneBorderColor,
                                   1,
                                    DashStyle.Solid,
                                   0,
                                   true);

                            }
                        }
                    }

                    if (args.ActiveMode
                        && document.EditorControl != null
                        && args.IsSelectedCell(cell))// cell.IsSelected)
                    {
                        //若单元格是被选中，则逆转绘制突出显示
                        document.EditorControl.AddSelectionAreaRectangle(
                            Rectangle.Ceiling(cell.GetAbsBounds()));
                    }
                    drawedCell.Add(cell);
                }//foreach

                args.ViewBounds = rowAbsBounds;
                args.ClientViewBounds = row.RuntimeStyle.GetClientRectangleF(rowAbsBounds);// row.AbsClientBounds;
            }// foreach (XTextTableRowElement row in this.RuntimeRows)

            drawedCell.Clear();
            drawedCell = null;

            int itemsCount = 0;
            var lineItems = lineShapes.GetRuntimeItems(ref itemsCount, args.ClipRectangle);
            if (itemsCount > 0 && drawContentOnly == false)
            {
                {
                    args.Graphics.ResetClip();
                    RectangleF cp = args.ClipRectangle;
                    if (args.RenderMode == InnerDocumentRenderMode.Print)
                    {
                        cp.Height += 4;
                    }
                    else
                    {
                        cp.Height++;
                    }
                    args.Graphics.SetClip(cp);
                    for (int iCount = 0; iCount < itemsCount; iCount++)
                    {
                        var item = lineItems[iCount];
                        if (args.Graphics != null)
                        {
                            if (item.Color == Color.Black && item.Width == 1 && item.Style == DashStyle.Solid)
                            {
                                args.Graphics.DrawLine(Pens.Black, item.X1, item.Y1, item.X2, item.Y2);
                            }
                            else
                            {
                                using (var p = DrawerUtil.CreatePen(item.Color, item.Width, item.Style))
                                {
                                    args.Graphics.DrawLine(
                                        p,
                                        item.X1,
                                        item.Y1,
                                        item.X2,
                                        item.Y2);
                                }
                            }
                            if (item.NullLine == false && item.Y1 == item.Y2)
                            {
                                document.LogHorizLinePosition(item.Y1);
                            }
                        }
                    }//foreach
                }
            }//if
            lineShapes.Dispose();
            lineShapes = null;
            //totalTick = CountDown.GetTickCountFloat() - totalTick;
        }


        /// <summary>
        /// 获得指定区域内的单元格对象
        /// </summary>
        /// <param name="rowIndex">开始行号</param>
        /// <param name="colIndex">开始列号</param>
        /// <param name="rowSpan">行数</param>
        /// <param name="colSpan">列数</param>
        /// <param name="includeOverriedCell">是否包含被合并的单元格</param>
        /// <returns>单元格对象集合</returns>
        public DomTableCellElement[] GetRangeArray(
            int rowIndex,
            int colIndex,
            int rowSpan,
            int colSpan,
            bool includeOverriedCell)
        {
            // 检查参数
            if (rowSpan < 0 || colSpan < 0)
            {
                return null;
            }
            int RowIndex2 = rowIndex + rowSpan - 1;
            int ColIndex2 = colIndex + colSpan - 1;

            if (rowIndex < 0)
            {
                rowIndex = 0;
            }
            if (colIndex < 0)
            {
                colIndex = 0;
            }
            DomElementList rows = this.Rows;
            if (rowIndex >= rows.Count || colIndex >= this.Columns.Count)
            {
                return null;
            }


            if (RowIndex2 >= rows.Count)
            {
                RowIndex2 = rows.Count - 1;
            }
            if (ColIndex2 >= this.Columns.Count)
            {
                ColIndex2 = this.Columns.Count - 1;
            }
            var resultCells = new DomTableCellElement[rowSpan * colSpan];
            int pos = 0;
            for (int row = rowIndex; row <= RowIndex2; row++)
            {
                DomTableRowElement myRow = (DomTableRowElement)rows[row];
                var cells = myRow.Cells;
                var len = cells.Count;
                for (int col = colIndex; col <= ColIndex2 && col < len; col++)
                {
                    DomTableCellElement cell = (DomTableCellElement)cells[col];
                    if (includeOverriedCell == false && cell.IsOverrided)
                    {
                        continue;
                    }
                    resultCells[pos++] = cell;
                    //myList.AddRaw(cell);
                }//for
            }//for
            if (pos < resultCells.Length)
            {
                var temp = new DomTableCellElement[pos];
                Array.Copy(resultCells, 0, temp, 0, temp.Length);
                resultCells = temp;
            }
            return resultCells;
        }

        /// <summary>
        /// 获得指定区域内的单元格对象
        /// </summary>
        /// <param name="rowIndex">开始行号</param>
        /// <param name="colIndex">开始列号</param>
        /// <param name="rowSpan">行数</param>
        /// <param name="colSpan">列数</param>
        /// <param name="includeOverriedCell">是否包含被合并的单元格</param>
        /// <returns>单元格对象集合</returns>
        private void GetRangeArray(
            int rowIndex,
            int colIndex,
            int rowSpan,
            int colSpan,
            bool includeOverriedCell,
            DCList<DomTableCellElement> resultCells)
        {
            // 检查参数
            if (rowSpan < 0 || colSpan < 0)
            {
                return;
            }
            int RowIndex2 = rowIndex + rowSpan - 1;
            int ColIndex2 = colIndex + colSpan - 1;

            if (rowIndex < 0)
            {
                rowIndex = 0;
            }
            if (colIndex < 0)
            {
                colIndex = 0;
            }
            DomElementList rows = this.Rows;
            if (rowIndex >= rows.Count || colIndex >= this.Columns.Count)
            {
                return;
            }

            if (RowIndex2 >= rows.Count)
            {
                RowIndex2 = rows.Count - 1;
            }
            if (ColIndex2 >= this.Columns.Count)
            {
                ColIndex2 = this.Columns.Count - 1;
            }
            for (int row = rowIndex; row <= RowIndex2; row++)
            {
                DomTableRowElement myRow = (DomTableRowElement)rows[row];
                var cells = myRow.Cells;
                var len = cells.Count;
                for (int col = colIndex; col <= ColIndex2 && col < len; col++)
                {
                    DomTableCellElement cell = (DomTableCellElement)cells[col];
                    if (includeOverriedCell == false && cell.IsOverrided)
                    {
                        continue;
                    }
                    resultCells.Add(cell);
                    //myList.AddRaw(cell);
                }//for
            }//for
        }
        /// <summary>
        /// 插入子元素
        /// </summary>
        /// <param name="index">指定的序号</param>
        /// <param name="element">新添加的元素</param>
        /// <returns>操作是否成功</returns>
        public override bool InsertChildElement(int index, DomElement element)
        {
            if (element is DomTableColumnElement)
            {
                this.Columns.Insert(index, element);
                element.Parent = this;
                element.OwnerDocument = this.OwnerDocument;
                return true;
            }
            else
            {
                return base.InsertChildElement(index, element);
            }
        }

        /// <summary>
        /// 将对象内容添加到文档视图元素内容中
        /// </summary>
        /// <param name="content">文档内容对象</param>
        /// <param name="privateMode">私有模式</param>
        /// <returns>添加的文档元素个数</returns>
        public override int AppendViewContentElement(AppendViewContentElementArgs args)
        {
            int result = 0;
            foreach (DomTableRowElement row in this.RuntimeRows.FastForEach())
            {
                foreach (DomTableCellElement cell in row.Cells.FastForEach())
                {
                    if (cell.RuntimeVisible)
                    {
                        result += cell.AppendViewContentElement(args);
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 添加子元素
        /// </summary>
        /// <param name="element">子元素</param>
        /// <returns>操作是否成功</returns>
        public override bool AppendChildElement(DomElement element)
        {
            if (element is DomTableColumnElement)
            {
                this.Columns.Add(element);
                element.Parent = this;
                element.OwnerDocument = this.OwnerDocument;
                return true;
            }
            else
            {
                return base.AppendChildElement(element);
            }
        }

        /// <summary>
        /// 刷新视图
        /// </summary>
        public override void EditorRefreshView()
        {
            this.CommitStyle(false);
            this.FixCells();
            this.FixDomState();
            this.EnumerateCells(delegate (DomTableCellElement cell)
            {
                cell.FixElements();
                cell.CommitStyle(false);
            });
            //foreach (XTextTableCellElement cell in this.Cells.FastForEach())
            //{
            //    cell.FixElements();
            //    cell.CommitStyle(false);
            //}
            this.UpdateElementsRuntimeVisible(true);
            DomDocument document = this.OwnerDocument;
            var back55 = document._EnableInvalidateViewFunction;
            document._EnableInvalidateViewFunction = false;
            try
            {
                base.EditorRefreshView();
            }
            finally
            {
                document._EnableInvalidateViewFunction = back55;
            }
            this.InvalidateView();
            if (document != null)
            {
                document.RefreshPages();
                if (document.EditorControl != null)
                {
                    document.EditorControl.UpdatePages();
                    document.EditorControl.UpdateTextCaret();
                    document.EditorControl.GetInnerViewControl().Invalidate();
                }
            }
        }
        /// <summary>
        /// 修复DOM状态
        /// </summary>
        public override void FixDomState()
        {
            InnerFixDomState(true, true);
        }


        /// <summary>
        /// 修复DOM状态
        /// </summary>
        internal void InnerFixDomState(bool resetOverrideCell, bool deeply)
        {
            DomDocument document = this._OwnerDocument;
            int columnsCount = 0;
            if (this._Columns != null)
            {
                columnsCount = this._Columns.Count;
                int colIndex = this.Rows.Count;
                foreach (DomTableColumnElement col in this._Columns.FastForEach())
                {
                    col._ElementIndex = colIndex++;
                    col.InnerSetOwnerDocumentParentRaw(document, this);
                    col.InnerCells = null;
                }
            }
            if (this._RuntimeRows != null && this._RuntimeRows != this.Rows)
            {
                this._RuntimeRows.Clear();
                this._RuntimeRows = null;
            }
            var rowsLen = this.Rows.Count;
            var rowsArray = this.Rows.InnerGetArrayRaw();
            for (var rowIndex = 0; rowIndex < rowsLen; rowIndex++)
            {
                var row = (DomTableRowElement)rowsArray[rowIndex];
                row._ElementIndex = rowIndex;
                row.InnerSetOwnerDocumentParentRaw(document, this);
                //MyFixElementsForSerialize(row);
                row.ResetMaxCellHeight();
                int colIndex = 0;
                //int cellIndex = 0;
                var cellsArray = row.Cells.InnerGetArrayRaw();
                var cellLen = row.Cells.Count;
                //row._ElementsCountDeeply = cellLen;
                for (var cellIndex = 0; cellIndex < cellLen; cellIndex++)
                {
                    var cell = (DomTableCellElement)cellsArray[cellIndex];
                    cell._RowIndex = rowIndex;
                    cell._ElementIndex = cellIndex;
                    if (resetOverrideCell)
                    {
                        cell._OverrideCell = null;
                    }
                    cell.InnerSetOwnerDocumentParentRaw(document, row);
                    if (deeply)
                    {
                        cell.FixDomState();
                        //row._ElementsCountDeeply += cell._ElementsCountDeeply;
                    }
                    if (colIndex < columnsCount)
                    {
                        cell.OwnerColumn = (DomTableColumnElement)this._Columns.GetItemFast(colIndex);
                    }
                    colIndex++;
                }
                //this._ElementsCountDeeply += row._ElementsCountDeeply;
            }
            //this._ElementsCountDeeply += this.ColumnsCount;
            //this.UpdateRowIndexColIndex();
            if (resetOverrideCell)
            {
                this.UpdateCellOverrideState();
            }
            //this.UpdateElementsRuntimeVisible(true);
        }

        /// <summary>
        /// 内部的文档加载后的处理
        /// </summary>
        /// <param name="objArgs">事件参数</param>
        public override void AfterLoad(ElementLoadEventArgs args)
        {
            foreach (DomTableColumnElement col in this.Columns.FastForEach())
            {
                col.SetParentAndDocumentRaw(this);
                //col.SetParentRaw(this);
                //col.SetOwnerDocumentRaw(this.OwnerDocument);
            }
            //MyFixElementsForSerialize(this);
            if (this.OwnerDocument.IsLoadingDocument == false)
            {
                InnerFixDomState(true, false);
                if (this.FixCells(false) > 0)
                {
                    this.UpdateCellOverrideState();
                }
            }
        }


        /// <summary>
        /// 更新文档元素的可见性
        /// </summary>
        /// <param name="deeply">是否深入设置子孙元素</param>
        public override void InnerUpdateElementsRuntimeVisible(UpdateElementsRuntimeVisibleArgs args)
        {
            DomDocument doc = this.ElementOwnerDocument();
            var deeply = args.Deeply;
            this._RuntimeVisible = this.GetRuntimeVisibleValue(args);
            if (this._Columns == null)
            {
                this._Columns = new DomElementList();
            }
            if (this._RuntimeVisible)
            {
                foreach (DomTableRowElement row in this.Rows.FastForEach())
                {
                    // 设置表格行的可见性
                    if (doc == null)
                    {
                        row._RuntimeVisible = row.Visible;
                    }
                    else
                    {
                        row._RuntimeVisible = row.GetRuntimeVisibleValue(args);
                    }
                    if (row._RuntimeVisible)
                    {
                        DomElementList cells = row.Cells;
                        var cellsCount = cells.Count;
                        for (int iCount = 0; iCount < cellsCount; iCount++)
                        {
                            // 设置单元格的可见性
                            DomTableCellElement cell = (DomTableCellElement)cells[iCount];
                            cell._RuntimeVisible = true;
                            if (cell.IsOverrided)
                            {
                                cell._RuntimeVisible = false;
                            }
                            if (deeply)
                            {
                                cell.InnerUpdateElementsRuntimeVisible(args);
                            }
                        }
                    }
                    else
                    {
                        row.InnerUpdateElementsRuntimeVisible(args);
                    }
                }
            }
            else
            {
                foreach (DomTableColumnElement col in this.Columns.FastForEach())
                {
                    col.RuntimeVisible = false;
                }
                if (deeply)
                {
                    base.HiddenElementsDeeply();
                }
                else
                {
                    base.InnerUpdateElementsRuntimeVisible(args);
                }
            }
        }
        /// <summary>
        /// 表格内容排版无效标志
        /// </summary>
        internal bool LayoutInvalidate = true;
        /// <summary>
        /// 刷新表格中部分大小状态无效的单元格的内容大小
        /// </summary>
        /// <param name="g"></param>
        internal void RefreshInvalidateCellSize(DCGraphics g)
        {
            InnerDocumentPaintEventArgs args = this.OwnerDocument.CreateInnerPaintEventArgs(g);
            foreach (DomTableCellElement cell in this.Cells)
            {
                if (cell.IsOverrided == false && cell.SizeInvalid)
                {
                    args.Element = cell;
                    cell.FixElements();
                    cell.RefreshSize(args);
                }
            }
        }

        /// <summary>
        /// 重新计算对象大小
        /// </summary>
        /// <param name="args">参数</param>
        public override void RefreshSize(InnerDocumentPaintEventArgs args)
        {
            this.LayoutInvalidate = true;
            // 特别处理表格
            var rowsCount = this.Elements.Count;
            for (var rowIndex = 0; rowIndex < rowsCount; rowIndex++)
            {
                this._Elements[rowIndex].Elements.ElementsRefreshSize(args);
                //var cells = this._Elements.GetItemFast(rowIndex).Elements;
                //var cellsCount = cells.Count;
                //for (var cellIndex = 0; cellIndex < cellsCount; cellIndex++)
                //{
                //    var cell = cells.GetItemFast(cellIndex);
                //    cell.RefreshSize(args);
                //}
            }
        }

        /// <summary>
        /// 执行表格排版
        /// </summary>
        public override void InnerExecuteLayout()
        {
            ExecuteLayout(true);
        }


        internal void ExecuteLayout(bool refreshOwnerLine)
        {
            if (this.OwnerDocument == null)
            {
                return;
            }
            //float tick = CountDown.GetTickCountFloat();
            float totalColWidth = 0;
            float zeroWidthCols = 0;
            foreach (DomTableColumnElement col in this.Columns)
            {
                if (col.Width < 5)
                {
                    zeroWidthCols++;
                }
                else
                {
                    totalColWidth += col.Width;
                }
            }//foreach
            if (zeroWidthCols > 0)
            {
                // 存在宽度为0的表格列，则设置
                float colWidth = 50;
                float sizeLeft = this.Parent.ClientWidth - totalColWidth;
                if (sizeLeft > 0)
                {
                    colWidth = Math.Max(colWidth, sizeLeft / zeroWidthCols);
                }
                foreach (DomTableColumnElement col in this.Columns)
                {
                    if (col.Width < 5)
                    {
                        col.Width = colWidth;
                    }
                }
            }
            this._RuntimeRows = null;
            this.LayoutInvalidate = false;
            if (this.OwnerDocument.IsLoadingDocument == false)
            {
                this.FixCells();
                this.UpdateCellOverrideState();
            }
            var back44 = DomContentElement._UseLineInfo_UpdateLinePosition;
            DomContentElement._UseLineInfo_UpdateLinePosition = false;
            try
            {
                this.UpdateCellsState(true);
            }
            finally
            {
                DomContentElement._UseLineInfo_UpdateLinePosition = back44;
                DomContentElement._Cached_PrivateContent?.ClearAndEmptyAll();
            }
            if (this._OwnerLine != null && refreshOwnerLine)
            {
                this._OwnerLine.ResetAbsLocation();//.DesignWidth = 0;
                this._OwnerLine.RefreshStateNew();
            }
            //tick = CountDown.GetTickCountFloat() - tick;
        }

        internal void UpdateRowIndexColIndex()
        {
            DomElementList rows = this.Rows;
            int rowsCount = rows.Count;
            var rowArray = rows.InnerGetArrayRaw();
            for (int rowIndex = 0; rowIndex < rowsCount; rowIndex++)
            {
                var row = (DomTableRowElement)rowArray[rowIndex];
                row._ElementIndex = rowIndex;
                var cells = row.Cells;
                int len = cells.Count;
                var cellArray = cells.InnerGetArrayRaw();
                for (int colIndex = 0; colIndex < len; colIndex++)
                {
                    var cell = (DomTableCellElement)cellArray[colIndex];
                    cell._RowIndex = rowIndex;
                    cell._ElementIndex = colIndex;
                }
            }
        }
        /// <summary>
        /// 枚举所有的单元格对象
        /// </summary>
        /// <param name="handler">处理方法</param>
        /// <exception cref="ArgumentNullException">参数handler空引用错误</exception>
        public void EnumerateCells(System.Action<DomTableCellElement> handler)
        {
            if (handler == null)
            {
                throw new ArgumentNullException("handler");
            }
            if (this._Elements == null || this._Elements.Count == 0)
            {
                return;
            }
            var rowArr = this._Elements.InnerGetArrayRaw();
            var rowCount = this._Elements.Count;
            for (var rowIndex = 0; rowIndex < rowCount; rowIndex++)
            {
                var rowElements = rowArr[rowIndex].Elements;
                if (rowElements != null && rowElements.Count > 0)
                {
                    var cellArr = rowElements.InnerGetArrayRaw();
                    var cellCount = rowElements.Count;
                    for (var cellIndex = 0; cellIndex < cellCount; cellIndex++)
                    {
                        handler((DomTableCellElement)cellArr[cellIndex]);
                    }
                }
            }
        }
        /// <summary>
        /// 更新单元格的合并状态
        /// </summary>

        public void UpdateCellOverrideState()
        {
            bool hasCellOverrided = false;
            DCList<DomElement> overriedCells = null;
            overriedCells = new DCList<DomElement>();
            this.EnumerateCells(delegate (DomTableCellElement cell)
            {
                if (cell._OverrideCell != null)
                {
                    overriedCells.FastAdd2(cell);
                    cell.SetOverrideCell(null);
                }
                if (cell.RowSpan > 1 || cell.ColSpan > 1)
                {
                    // 存在单元格被合并的现象
                    hasCellOverrided = true;
                }
            });
            if (hasCellOverrided == false)
            {
                // 没有任何单元格被合并
                if (overriedCells.Count > 0)
                {
                    foreach (DomTableCellElement cell in overriedCells)
                    {
                        cell.FixElements();
                        cell.SizeInvalid = true;
                    }
                }
                return;
            }
            this._RuntimeRows = null;
            var rrows = this.RuntimeRows;
            var tableRowsCount = this.RowsCount;
            var tableColumnsCount = this.ColumnsCount;
            var rrowsCount = rrows.Count;
            var cellRangs = new DCList<DomTableCellElement>();
            this.EnumerateCells(delegate (DomTableCellElement myCell)
            {
                if (myCell._OverrideCell != null)
                {
                    // 本单元格已经被合并了,无需处理
                    return;
                }
                int cellRowIndex = myCell.RowIndex;
                int cellColIndex = myCell.ColIndex;
                if (myCell.RowSpan > 1)
                {
                    if (cellRowIndex + myCell.RowSpan > rrowsCount)
                    {
                        myCell.InternalSetRowSpan(tableRowsCount - cellRowIndex);
                    }
                }
                if (myCell.ColSpan > 1)
                {
                    if (cellColIndex + myCell.ColSpan > tableColumnsCount)
                    {
                        myCell.InternalSetColSpan(tableColumnsCount - cellColIndex);
                    }
                }
                int handleCount = 0;
            SetOverriedCells:;
                int runtimeRowSpan = myCell.RowSpan;
                if (runtimeRowSpan > 1 || myCell.ColSpan > 1)
                {
                    // 获得所有被当前单元格合并的单元格
                    cellRangs.Clear();
                    this.GetRangeArray(
                        cellRowIndex,
                        cellColIndex,
                        runtimeRowSpan,
                        myCell.ColSpan,
                        true,
                        cellRangs);
                    if (cellRangs.Count > 0)
                    {
                        foreach (DomTableCellElement e2 in cellRangs)
                        {
                            if (e2.OverrideCell != null && e2.OverrideCell != myCell)
                            {
                                handleCount++;
                                if (handleCount > 20)
                                {
                                    // 出现死循环，操作失败，退出
                                    break;
                                }
                                // 这个单元格已经被其他单元格合并了，因此出现了单元格合并操作冲突
                                // 需要修复当前单元格的合并列数
                                myCell.InternalSetColSpan(e2.ColIndex - cellColIndex);
                                goto SetOverriedCells;
                            }
                            // 设置这些单元格的合并单元格
                            e2.SetOverrideCell(myCell);
                        }
                    }
                    myCell.SetOverrideCell(null);
                }//if
            });
            cellRangs.Clear();
            cellRangs = null;
            if (overriedCells != null && overriedCells.Count > 0)
            {
                var arr = overriedCells.InnerGetArrayRaw();
                for (var iCount = overriedCells.Count - 1; iCount >= 0; iCount--)
                {
                    var cell = (DomTableCellElement)arr[iCount];
                    if (cell._OverrideCell == null)
                    {
                        // 单元格原先被合并的，现在有可见的，其内容需要重新排版
                        cell.FixElements();
                        cell.SizeInvalid = true;
                    }
                }
                overriedCells.Clear();
            }
        }

        internal void UpdateCellsState(bool cellExecuteLayout, bool updateForHiddenLineOnly = false)
        {
            float colLeft = this.RuntimeStyle.PaddingLeft;
            float totalColumnWidth = 0;
            foreach (DomTableColumnElement col in this.Columns)
            {
                col.Left = colLeft;
                if (col.RuntimeVisible)
                {
                    colLeft += col.Width;
                    totalColumnWidth += col.Width;
                }
            }
            var rowsArray = this.Rows.InnerGetArrayRaw();
            for (var iCount = this.Rows.Count - 1; iCount >= 0; iCount--)
            {
                var row = (DomTableRowElement)rowsArray[iCount];
                row._MaxCellHeight = -1;
                row._Width = totalColumnWidth;
            }
            var myRuntimeRows = this.RuntimeRows;
            var myRuntimeRowsCount = myRuntimeRows.Count;
            var myRuntimeRowsArray = myRuntimeRows.InnerGetArrayRaw();
            this.OwnerDocument.ContentStyles.CheckCreateRuntimeStyleInstnace();

            var args9 = new ParticalRefreshLinesEventArgs(
                    null,
                    null,
                    this.RuntimeStyle.VerticalAlign);
            args9.LayoutContentOnly = true;

            DomElementList columns = this.Columns;

            for (var ri = 0; ri < myRuntimeRowsCount; ri++)
            {
                var row = myRuntimeRowsArray[ri];
                row.StateFlag32 = false;
                var cellArray = row.Elements.InnerGetArrayRaw();
                var cellCount = row.Elements.Count;
                for (var ci = 0; ci < cellCount; ci++)
                {
                    var myCell = (DomTableCellElement)cellArray[ci];
                    if (myCell.IsOverrided == false && myCell.RowSpan > 1)
                    {
                        row.StateFlag32 = true;
                    }
                    int colIndex = myCell.ColIndex;
                    myCell.FixElements();
                    if (myCell.RuntimeVisible)
                    {
                        float cellWidthCount = 0;
                        if (myCell.ColSpan == 1)
                        {
                            cellWidthCount = columns[colIndex].Width;
                        }
                        else
                        {
                            for (int iCount = 0; iCount < myCell.ColSpan; iCount++)
                            {
                                var col = columns[colIndex + iCount];
                                if (col.RuntimeVisible)
                                {
                                    cellWidthCount += col.Width;
                                }
                            }
                        }
                        if (myCell.Width != cellWidthCount
                            || myCell.SizeInvalid == true
                            || myCell.LayoutInvalidate == true)
                        {
                            // 重新设置单元格的宽度
                            myCell.Width = cellWidthCount;
                            if (cellExecuteLayout)
                            {
                                myCell.ExecuteLayoutInner(true, args9);// .ExecuteLayout();
                            }
                        }
                        float heightCount = 0;
                        int runtimeRowSpan = myCell.RowSpan;
                        if (runtimeRowSpan == 1)
                        {
                            heightCount = myCell.OwnerRow.Height;
                        }
                        else
                        {
                            for (int iCount = 0; iCount < runtimeRowSpan; iCount++)
                            {
                                heightCount += this.Rows[myCell.RowIndex + iCount].Height;
                            }
                        }
                        if (updateForHiddenLineOnly)
                        {
                            VerticalAlignStyle cva = myCell.ContentVertialAlign;
                            myCell.UpdateLinePosition(cva, false, false);
                        }
                        else if (cellExecuteLayout)
                        {
                            if (myCell.Height == 0 || myCell.Height != heightCount || myCell.SizeInvalid)
                            {
                                //myCell.Height = HeightCount;
                                VerticalAlignStyle cva = myCell.ContentVertialAlign;
                                if (cva != VerticalAlignStyle.Top)
                                {
                                    myCell.UpdateLinePosition(cva, false, false);
                                    //myCell.ExecuteLayout();
                                }
                            }
                        }
                    }
                    else
                    {
                        myCell.Left = myCell.OwnerColumn.Left;
                        myCell.Top = 0;
                        myCell.Width = myCell.OwnerColumn.Width;
                        myCell.Height = myCell.OwnerRow.Height;
                    }
                }
            }
            args9 = null;

            ElementStack.ClearLastCache();
            DCObjectPool<DomLine>.Clear();
            // 计算表格行的高度
            //tick = CountDown.GetTickCountFloat();
            var thisRows = this.Rows;
            // 根据单元格的内容高度来设置表格行的高度
            for (var iCount = 0; iCount < myRuntimeRowsCount; iCount++)
            {
                ((DomTableRowElement)myRuntimeRowsArray[iCount]).CalcuteRowHeight();
            }
            for (var ri = 0; ri < myRuntimeRowsCount; ri++)
            {
                var row55 = myRuntimeRowsArray[ri];
                var cellArray = row55.Elements.InnerGetArrayRaw();
                var cellCount = row55.Elements.Count;
                //foreach (XTextTableCellElement cell in myCells.FastForEach())
                for (var cellIndex = 0; cellIndex < cellCount; cellIndex++)
                {
                    var cell = (DomTableCellElement)cellArray[cellIndex];
                    if (cell.IsOverrided == false && cell.RowSpan > 1)
                    {
                        float heightCount = 0;
                        // 累计表格行的高度
                        for (int rowIndex = 0; rowIndex < cell.RowSpan; rowIndex++)
                        {
                            heightCount += thisRows[rowIndex + cell._RowIndex].Height;
                        }
                        var rsCell = cell.OwnerRow.RuntimeStyle;
                        float ccHeight = cell.ContentHeight + rsCell.PaddingTop + rsCell.PaddingBottom;
                        if (ccHeight > heightCount)
                        {
                            //bool match = false;
                            for (int rowIndex = cell.RowSpan - 1; rowIndex >= 0; rowIndex--)
                            {
                                var row = (DomTableRowElement)thisRows[rowIndex + cell._RowIndex];
                                if (row.SpecifyHeight >= 0)
                                {
                                    row.Height = row.Height + ccHeight - heightCount;
                                    //match = true;
                                    break;
                                }
                            }//for
                        }
                        else
                        {
                            cell.Height = heightCount;
                        }
                    }//if
                }//foreach
            }
            // 设置各个单元格的位置
            var rsTable = this.RuntimeStyle;
            bool useGlobalGridLine = false;
            DCGridLineInfo ginfo = this.OwnerDocument._GlobalGridInfo;
            if (ginfo != null && ginfo.AlignToGridLine)
            {
                useGlobalGridLine = true;
            }
            float rowTop = useGlobalGridLine ? 0 : rsTable.PaddingTop;
            for (var ri = 0; ri < myRuntimeRowsCount; ri++)
            {
                var row = (DomTableRowElement)myRuntimeRowsArray[ri];
                row._Left = rsTable.PaddingLeft;
                row._Top = rowTop;
                rowTop += row.Height;
                var cellCount = row.Cells.Count;
                var cellArray = row.Cells.InnerGetArrayRaw();
                for (var ci = 0; ci < cellCount; ci++)
                {
                    var cell = (DomTableCellElement)cellArray[ci];
                    // 设置单元格的位置
                    if (cell._Parent != row)
                    {
                        // 状态不对，修正。
                        cell.SetParentAndDocumentRaw(row);
                    }
                    cell._Left = cell.OwnerColumn.Left;
                    cell._Top = 0;
                    cell.ResetLinesAbsLocation();
                    // 设置单元格的高度
                    if (cell.RowSpan == 1 || cell.IsOverrided)
                    {
                        cell.Height = row.Height;
                    }
                    else
                    {
                        float cellHeight = 0;
                        int rowIndex = thisRows.IndexOfFast(row, row.RowIndex);
                        for (int iCount = 0; iCount < cell.RowSpan; iCount++)
                        {
                            cellHeight += thisRows[rowIndex + iCount].Height;
                        }
                        cell.Height = cellHeight;
                    }
                    VerticalAlignStyle cva = cell.RuntimeStyle.VerticalAlign;
                    if (cva != VerticalAlignStyle.Top)
                    {
                        // 若单元格内容不是垂直顶端对齐，则重新设置文档行的位置
                        if (cell.Height > 0)
                        {
                            cell.UpdateLinePosition(cva, false, false);
                        }
                    }
                }//foreach
            }//foreach
            if (useGlobalGridLine)
            {
                this.Height = rowTop;
            }
            else
            {
                this.Height = rowTop + rsTable.PaddingTop + rsTable.PaddingBottom;
            }
            this.Width = totalColumnWidth + rsTable.PaddingLeft + rsTable.PaddingRight;
        }

        internal void ResetLinesAbsLocation()
        {
            var rowArray = this.Elements.InnerGetArrayRaw();
            for (int iCount = this.Elements.Count - 1; iCount >= 0; iCount--)
            {
                var cells = rowArray[iCount].Elements;// this._Elements[iCount].Elements;
                var cellArray = cells.InnerGetArrayRaw();
                for (int iCount2 = cells.Count - 1; iCount2 >= 0; iCount2--)
                {
                    var cell = (DomTableCellElement)cellArray[iCount2];//  cells.GetItemFast(iCount2);
                    if (cell.IsOverrided == false)
                    {
                        cell.ResetLinesAbsLocation();
                    }
                }
            }
        }

        /// <summary>
        /// 更新表格行的位置
        /// </summary>
        /// <remarks>当应用程序</remarks>
        internal void UpdateRowPosition()
        {
            RuntimeDocumentContentStyle rs2 = this.RuntimeStyle;
            float rowTop = rs2.PaddingTop;
            foreach (DomTableRowElement row in this.RuntimeRows)
            {
                row.Top = rowTop;
                rowTop = rowTop + row.Height;
                row.Left = rs2.PaddingLeft;
            }//foreach
            this.Height = rowTop + rs2.PaddingBottom;
            if (this.OwnerLine != null)
            {
                this.OwnerLine.RefreshStateNew();
            }
        }
        /// <summary>
        /// 修正单元格对象的个数
        /// </summary>
        public int FixCells(bool fixCellDomState = true)
        {
            int result = 0;
            int maxCellCount = 0;
            //float tick = CountDown.GetTickCountFloat();
            bool newInstance = false;
            var thisColumns = this.Columns;
            //var document = this.OwnerDocument;
            foreach (DomTableRowElement myRow in this.Rows.FastForEach())
            {
                var myRowCells = myRow.Cells;
                if (maxCellCount < myRowCells.Count)
                {
                    maxCellCount = myRowCells.Count;
                }
                if (myRowCells.Count > thisColumns.Count)
                {
                    for (int iCount = thisColumns.Count; iCount < myRowCells.Count; iCount++)
                    {
                        DomTableColumnElement newCol = this.CreateColumnInstance();
                        newCol.Width = myRowCells[iCount].Width;
                        if (newCol.Width == 0)
                        {
                            if (thisColumns.Count == 0)
                            {
                                newCol.Width = 100;
                            }
                            else
                            {
                                newCol.Width = thisColumns.LastElement.Width;
                            }
                        }
                        newCol.SetParentAndDocumentRaw(this);
                        thisColumns.Add(newCol);
                        newInstance = true;
                    }
                }//if
            }//foreach
            if (thisColumns.Count > maxCellCount)
            {
                thisColumns.RemoveToFixLenght(maxCellCount);
            }
            foreach (DomTableRowElement myRow in this.Elements.FastForEach())
            {
                //////////////////myRow.Items.Lock = false;
                var myRowCells = myRow.Cells;
                if (myRowCells.Count != thisColumns.Count)
                {
                    myRowCells.RemoveToFixLenght(thisColumns.Count);
                    if (myRowCells.Count < thisColumns.Count)
                    {
                        DomTableCellElement lastCell = (DomTableCellElement)myRowCells.LastElement;
                        for (int iCount = myRowCells.Count; iCount < thisColumns.Count; iCount++)
                        {
                            DomTableCellElement cell = null;
                            if (lastCell == null)
                            {
                                cell = this.CreateCellInstance();
                                //cell.Style.BorderWidth = this.intDefaultCellBorderWidth;
                            }
                            else
                            {
                                cell = (DomTableCellElement)lastCell.Clone(false);
                                //cell.Text = null;
                            }
                            result++;
                            //cell.BackColor = Color.Red ;
                            myRowCells.Add(cell);
                            cell.SetParentAndDocumentRaw(myRow);
                            cell.OwnerColumn = (DomTableColumnElement)thisColumns[myRowCells.Count - 1];
                            if (fixCellDomState)
                            {
                                cell.FixDomState();
                            }
                            newInstance = true;
                        }
                    }//if( myRow.Items.Count < myColumns.Count )
                }
            }
            if (newInstance)
            {
                // 创建了新的对象实例，则修正DOM状态
                //this.FixDomState();
            }
            this.UpdateRowIndexColIndex();
            //tick = CountDown.GetTickCountFloat() - tick;
            return result;
        }

        /// <summary>
        ///  表格高度发生改变，需要重新设置行状态和重新分页
        /// </summary>
        /// <param name="forceFixHeight">强制修正高度</param>
        internal void UpdatePagesForTable(bool forceFixHeight)
        {
            // 表格高度发生改变，需要重新设置行状态和重新分页
            DomLine tableLine = this.OwnerLine;
            if (tableLine == null)
            {
                return;
            }
            float lineHeight = tableLine.Height;
            // 刷新表格所在行的状态
            //tableLine.DesignWidth = tableLine.Width;
            tableLine.RefreshStateNew();
            tableLine.ResetAbsLocation();
            if (tableLine.Height != lineHeight || forceFixHeight)
            {
                // 则向上调整父表格和更上级的表格的排版。
                DomElement parent = this;
                while (parent != null)
                {
                    if (parent is DomTableCellElement)
                    {
                        DomTableCellElement cell = (DomTableCellElement)parent;
                        DomTableElement table = cell.OwnerTable;
                        float heightBack = cell.Height;
                        cell.Height = 0;
                        cell.SizeInvalid = true;
                        table.InnerExecuteLayout();
                        if (cell.Height != heightBack)
                        {
                            table.OwnerLine.RefreshStateNew();
                            table.OwnerLine.InvalidateState = true;
                        }
                    }
                    parent = parent.Parent;
                }

                // 更新文档行
                DomDocumentContentElement dce = this.DocumentContentElement;
                dce.UpdateLinePosition(dce.ContentVertialAlign, false, true);
                dce.UpdateHeightByContentHeight();
                // 更新分页状态
                //if (dce.ContentPartyStyle == PageContentPartyStyle.Body)
                {
                    this.OwnerDocument.RefreshPages();
                }
                if (this.OwnerDocument.EditorControl != null)
                {
                    this.OwnerDocument.EditorControl.UpdatePages();
                    this.OwnerDocument.EditorControl.GetInnerViewControl().Invalidate();
                }
            }
            if (this.OwnerDocument.EditorControl != null)
            {
                this.OwnerDocument.EditorControl.UpdateTextCaretWithoutScroll();
            }
        }

        /// <summary>
        /// 获得本文档元素容器包含的指定类型的第一个文档元素
        /// </summary>
        /// <param name="elementType">元素类型</param>
        /// <returns>获得的元素</returns>
        public override DomElement GetElementByType(Type elementType)
        {
            if (elementType == typeof(DomTableColumnElement))
            {
                if (this.Columns != null && this.Columns.Count > 0)
                {
                    return this.Columns[0];
                }
            }
            return base.GetElementByType(elementType);
        }

        /// <summary>
        /// 获得本文档元素容器包含的所有的指定类型的文档元素列表
        /// </summary>
        /// <param name="elementType">元素类型</param>
        /// <returns>获得的元素列表</returns>
        public override DomElementList GetElementsByType(Type elementType)
        {
            if (elementType == typeof(DomTableColumnElement))
            {
                if (this.Columns != null)
                {
                    DomElementList list = new DomElementList();
                    list.AddRangeByDCList(this.Columns);
                    return list;
                }
            }
            return base.GetElementsByType(elementType);
        }
        public override DomElement GetElementByHashCode(int hashCode)
        {
            if (this._Columns != null && this._Columns.Count > 0)
            {
                var result = this._Columns.GetElementByHashCode(hashCode);
                if (result != null)
                {
                    return result;
                }
            }
            if (this._Elements != null && this._Elements.Count > 0)
            {
                var arr = this._Elements.InnerGetArrayRaw();
                for (var iCount = this._Elements.Count - 1; iCount >= 0; iCount--)
                {
                    var row = (DomTableRowElement)arr[iCount];
                    if (row.GetHashCode() == hashCode)
                    {
                        return row;
                    }
                    if (row._Elements != null)
                    {
                        var arr2 = row._Elements.InnerGetArrayRaw();
                        for (var iCount2 = row._Elements.Count - 1; iCount2 >= 0; iCount2--)
                        {
                            var cell = (DomTableCellElement)arr2[iCount2];
                            if (cell.GetHashCode() == hashCode)
                            {
                                return cell;
                            }
                            var result = cell.GetElementByHashCode(hashCode);
                            if (result != null)
                            {
                                return result;
                            }
                        }
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// 获得文档中指定编号的元素对象,查找时ID值区分大小写的。
        /// </summary>
        /// <param name="id">指定的编号</param>
        /// <returns>找到的元素对象</returns>
        public override DomElement GetElementById(string id)
        {
            if (this.Columns != null)
            {
                foreach (DomTableColumnElement col in this.Columns)
                {
                    if (col.ID == id)
                    {
                        return col;
                    }
                }
            }
            return base.GetElementById(id);
        }
        /// <summary>
        /// 获得指定序号的单元格对象
        /// </summary>
        /// <param name="rowIndex">从0开始的行号</param>
        /// <param name="colIndex">从0开始的列号</param>
        /// <param name="throwException">若未找到单元格是否抛出异常</param>
        /// <returns>找的单元格对象，若未找到而且不抛出异常则返回空引用</returns>
        public DomTableCellElement GetCell(int rowIndex, int colIndex, bool throwException)
        {
            DomTableRowElement row = (DomTableRowElement)this.Rows.SafeGet(rowIndex);
            if (row != null)
            {
                DomTableCellElement cell = (DomTableCellElement)row.Cells.SafeGet(colIndex);
                if (cell != null)
                {
                    return cell;
                }
            }

            //XTextElementList rows = this.RuntimeRows;
            //if (rowIndex >= 0 && rowIndex < rows.Count)
            //{
            //    XTextTableRowElement row = (XTextTableRowElement)rows[rowIndex];
            //    if (colIndex >= 0 && colIndex < row.Cells.Count)
            //    {
            //        return (XTextTableCellElement)row.Cells[colIndex];
            //    }
            //}
            if (throwException)
            {
                throw new Exception("Bad RowIndex=" + rowIndex + ", ColIndex=" + colIndex);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 根据两个单元格位置获得区域中被选择的单元格列表，包括被合并而隐藏的单元格
        /// </summary>
        /// <param name="rowIndex1">第一个单元格的行号</param>
        /// <param name="colIndex1">第一个单元格的列号</param>
        /// <param name="rowIndex2">第二个单元格的行号</param>
        /// <param name="colIndex2">第二个单元格的列号</param>
        /// <returns>单元格对象列表</returns>
        public DomElementList GetSelectionCells(int rowIndex1, int colIndex1, int rowIndex2, int colIndex2)
        {
            // 检查参数
            if (rowIndex1 < 0 || rowIndex1 >= this.Rows.Count)
            {
                throw new ArgumentOutOfRangeException("rowIndex1=" + rowIndex1);
            }
            if (rowIndex2 < 0 || rowIndex2 >= this.Rows.Count)
            {
                throw new ArgumentOutOfRangeException("rowIndex2=" + rowIndex2);
            }
            if (colIndex1 < 0 || colIndex1 >= this.Columns.Count)
            {
                throw new ArgumentOutOfRangeException("colIndex1=" + colIndex1);
            }
            if (colIndex2 < 0 || colIndex2 >= this.Columns.Count)
            {
                throw new ArgumentOutOfRangeException("colIndex2=" + colIndex2);
            }

            var cells = GetRangeArray(
                Math.Min(rowIndex1, rowIndex2),
                Math.Min(colIndex1, colIndex2),
                Math.Abs(rowIndex1 - rowIndex2) + 1,
                Math.Abs(colIndex1 - colIndex2) + 1,
                true);

            for (int iCount = 0; iCount < cells.Length; iCount++)
            {
                DomTableCellElement cell = (DomTableCellElement)cells[iCount];
                if (cell.IsOverrided)
                {
                    cell = cell.OverrideCell;
                }
                rowIndex1 = Math.Min(rowIndex1, cell.RowIndex);
                rowIndex2 = Math.Max(rowIndex2, cell.RowIndex + cell.RowSpan - 1);
                colIndex1 = Math.Min(colIndex1, cell.ColIndex);
                colIndex2 = Math.Max(colIndex2, cell.ColIndex + cell.ColSpan - 1);
            }//for

            cells = GetRangeArray(
                rowIndex1,
                colIndex1,
                rowIndex2 - rowIndex1 + 1,
                colIndex2 - colIndex1 + 1,
                true);
            var result = new DomElementList();
            result.SetData(cells);
            return result;
            //return new XTextElementList(cells);
        }
        private class RowPositionComparer : IComparer<DomElement>
        {
            public RowPositionComparer(RectangleF tableBounds, float absY)
            {
                this._Y = absY - tableBounds.Top;
            }
            private float _Y;
            public int Compare(DomElement x, DomElement y)
            {
                float offset = this._Y - x.Top;
                if (offset < 0)
                {
                    return 1;
                }
                else if (offset < ((DomTableRowElement)x).MaxCellHeight())
                {
                    return 0;
                }
                else
                {
                    return -1;
                }
            }
        }
        /// <summary>
        /// 根据文档绝对位置获得单元格对象
        /// </summary>
        /// <param name="absX">绝对X坐标值</param>
        /// <param name="absY">绝对Y坐标值</param>
        /// <returns>获得的单元格对象</returns>
        public DomTableCellElement GetCellByAbsPosition(float absX, float absY)
        {
            if (this.RuntimeVisible == false)
            {
                return null;
            }
            RectangleF tableBounds = this.GetAbsBounds();
            if (tableBounds.Contains(absX, absY))
            {
                DomElementList rows = this.RuntimeRows;
                if (rows.Count > 10 && rows == this.Rows)
                {
                    // 在大多数情况下，使用二分法查找是可以找到对象的
                    int curIndex = rows.BinarySearch(null, new RowPositionComparer(tableBounds, absY));
                    if (curIndex >= 0)
                    {
                        var curRow = (DomTableRowElement)rows[curIndex];
                        float rowTop = tableBounds.Top + curRow.Top;
                        foreach (DomTableCellElement cell in curRow.Cells)
                        {
                            if (cell.RuntimeVisible)
                            {
                                RectangleF cellBounds = new RectangleF(
                                    tableBounds.Left + cell.Left,
                                    rowTop,
                                    cell.Width,
                                    cell.Height);
                                if (cellBounds.Contains(absX, absY))
                                {
                                    // 找到单元格
                                    return cell;
                                }//if
                            }//if
                        }//foreach
                    }
                }
                for (int iCount = rows.Count - 1; iCount >= 0; iCount--)
                {
                    DomTableRowElement row = (DomTableRowElement)rows[iCount];
                    if (row._RuntimeVisible)
                    {
                        float rowTop = tableBounds.Top + row.Top;
                        if (absY >= rowTop && absY < rowTop + row.MaxCellHeight())
                        {
                            foreach (DomTableCellElement cell in row.Cells)
                            {
                                if (cell.RuntimeVisible)
                                {
                                    RectangleF cellBounds = new RectangleF(
                                        tableBounds.Left + cell.Left,
                                        rowTop,
                                        cell.Width,
                                        cell.Height);
                                    if (cellBounds.Contains(absX, absY))
                                    {
                                        // 找到单元格
                                        return cell;
                                    }//if
                                }//if
                            }//foreach
                        }
                    }//if
                }//foreach
            }
            return null;
        }

        #region 为编辑提供支持 ******************************************************************

        /// <summary>
        /// 表格的宽度
        /// </summary>
        public float TableWidth
        {
            get
            {
                float oldWidth = 0;
                foreach (DomTableColumnElement col in this.Columns)
                {
                    if (col.RuntimeVisible)
                    {
                        oldWidth = oldWidth + col.Width;
                    }
                }
                return oldWidth;
            }
        }

        /// <summary>
        /// 设置表格的宽度
        /// </summary>
        /// <param name="newWidth">新宽度</param>
        public bool SetTableWidth(float newWidth)
        {
            if (newWidth <= 0)
            {
                throw new ArgumentOutOfRangeException("NewWidth=" + newWidth);
            }
            float oldWidth = 0;
            int visibleCount = 0;
            List<DomTableColumnElement> zeroCols = new List<DomTableColumnElement>();
            foreach (DomTableColumnElement col in this.Columns)
            {
                if (col.RuntimeVisible)
                {
                    oldWidth = oldWidth + col.Width;
                    visibleCount++;
                    if (col.ZeroWidth())
                    {
                        zeroCols.Add(col);
                    }
                }
            }
            bool result = false;
            if (zeroCols.Count == this.Columns.Count)
            {
                foreach (DomTableColumnElement col in zeroCols)
                {
                    col.Width = newWidth / zeroCols.Count;
                }
                result = true;
                zeroCols.Clear();
            }
            else if (oldWidth != newWidth && oldWidth > 0)
            {
                if (zeroCols.Count > 0 && newWidth > oldWidth)
                {
                    float zw = (newWidth - oldWidth) / zeroCols.Count;
                    foreach (DomTableColumnElement col in zeroCols)
                    {
                        col.Width = zw;
                        result = true;
                    }
                    zeroCols.Clear();
                }
                else
                {
                    float rate = newWidth / oldWidth;
                    foreach (DomTableColumnElement col in this.Columns)
                    {
                        if (col.RuntimeVisible)
                        {
                            col.Width = col.Width * rate;
                            result = true;
                        }
                    }
                }
            }
            if (zeroCols.Count > 0)
            {
                var minCW = this.GetDocumentViewOptions().MinTableColumnWidth;
                foreach (DomTableColumnElement col in zeroCols)
                {
                    if (this.OwnerDocument == null)
                    {
                        col.Width = 50f;
                    }
                    else
                    {
                        col.Width = minCW;// this.OwnerDocument.Options.ViewOptions.MinTableColumnWidth;
                    }
                    return true;
                }
            }
            return result;
        }

        /// <summary>
        /// 压缩行列数
        /// </summary>
        /// <remarks>操作是否修改了表格结构</remarks>
        internal bool CompressRowsColumns()
        {
            bool modified = false;
            // 压缩表格列
            for (int colIndex = this.Columns.Count - 1; colIndex > 0; colIndex--)
            {
                bool match = true;
                foreach (DomTableRowElement row in this.Rows)
                {
                    DomTableCellElement cell = (DomTableCellElement)row.Cells[colIndex];
                    if (cell.IsOverrided == false)
                    {
                        match = false;
                        break;
                    }
                }
                if (match == true)
                {
                    // 该表格列中所有的单元格都被合并了，则该表格列可以删除掉
                    modified = true;
                    DomElementList handledCells = new DomElementList();
                    foreach (DomTableRowElement row in this.Rows)
                    {
                        DomTableCellElement cell = (DomTableCellElement)row.Cells[colIndex];
                        DomTableCellElement oc = cell.OverrideCell;
                        if (handledCells.Contains(oc) == false)
                        {
                            handledCells.Add(oc);
                            oc.InternalSetColSpan(oc.ColSpan - 1);
                        }
                        row.Cells.RemoveAtRaw(colIndex);
                    }
                    DomTableColumnElement oldCol = (DomTableColumnElement)this.Columns[colIndex];
                    DomTableColumnElement preCol = (DomTableColumnElement)this.Columns[colIndex - 1];
                    preCol.Width = preCol.Width + oldCol.Width;
                    this.Columns.RemoveAtRaw(colIndex);
                }
            }
            // 压缩表格行
            for (int iCount = this.Rows.Count - 1; iCount > 0; iCount--)
            {
                bool match = true;
                DomTableRowElement row = (DomTableRowElement)this.Rows[iCount];
                foreach (DomTableCellElement cell in row.Cells)
                {
                    if (cell.IsOverrided == false)
                    {
                        match = false;
                        break;
                    }
                }
                if (match == true)
                {
                    modified = true;
                    // 该表格行的单元格全部被合并了，可以删除掉
                    DomElementList handledCells = new DomElementList();
                    foreach (DomTableCellElement cell in row.Cells)
                    {
                        DomTableCellElement oc = cell.OverrideCell;
                        if (handledCells.Contains(oc) == false)
                        {
                            handledCells.Add(oc);
                            oc.InternalSetRowSpan(oc.RowSpan - 1);
                        }
                    }
                    this.Rows.RemoveAtRaw(iCount);
                }
            }
            if (modified)
            {
                // 修正单元格行列号
                for (int rowIndex = 0; rowIndex < this.Rows.Count; rowIndex++)
                {
                    DomTableRowElement row = (DomTableRowElement)this.Rows[rowIndex];
                    row._ElementIndex = rowIndex;
                    for (int colIndex = 0; colIndex < row.Cells.Count; colIndex++)
                    {
                        DomTableCellElement cell = (DomTableCellElement)row.Cells[colIndex];
                        cell._RowIndex = rowIndex;
                        cell._ElementIndex = colIndex;
                    }
                }
            }
            return modified;
        }

        /// <summary>
        /// 在编辑器中插入表格行
        /// </summary>
        /// <param name="index">新表格行的序号</param>
        /// <param name="newRows">新表格行对象</param>
        /// <param name="logUndo">是否记录撤销操作信息</param>
        /// <param name="specifyRowSpan">用户指定的单元格跨行数</param>
        /// <param name="autoSetRowSpan">自动设置跨行数</param>

        public void EditorInsertRows2(
            int index,
            DomElementList newRows,
            bool logUndo,
            Dictionary<DomTableCellElement, int> specifyRowSpan,
            bool autoSetRowSpan)
        {
            // 检查参数
            if (newRows == null || newRows.Count == 0)
            {
                throw new ArgumentNullException("newRows");
            }

            if (index < 0 || index > this.Rows.Count)
            {
                throw new ArgumentOutOfRangeException(string.Format(
                    DCSR.ArgumentOutOfRange_Name_Value_Max_Min,
                    "index",
                    index,
                    this.Rows.Count - 1,
                    0));
            }
            var thisRows = this.Rows;
            foreach (DomTableRowElement row in newRows)
            {
                if (thisRows.Contains(row))
                {
                    throw new ArgumentException(DCSR.RowExistInTable);
                }
            }
            DCSoft.Writer.Dom.Undo.XTextUndoTableInfo undo = null;
            if (logUndo)
            {
                undo = new DCSoft.Writer.Dom.Undo.XTextUndoTableInfo();
                // 记录表格的旧的结构信息
                undo.OldTableInfo = new TableStructInfo(this);
            }
            var document = this.OwnerDocument;
            thisRows.EnsureCapacity(thisRows.Count + newRows.Count);

            var back9 = document._EnableInvalidateViewFunction;
            document._EnableInvalidateViewFunction = false;

            var back10 = DomContentElement._UseLineInfo_UpdateLinePosition;
            DomContentElement._UseLineInfo_UpdateLinePosition = false;
            DomContainerElement.GlobalEnabledResetChildElementStats = false;
            try
            {
                for (int iCount = 0; iCount < newRows.Count; iCount++)
                {
                    DomTableRowElement row = (DomTableRowElement)newRows[iCount];
                    // 设置新行的状态
                    row.InnerSetOwnerDocumentParentRaw(document, this);
                    // 将新表格行插入的表格中
                    thisRows.InsertRaw(index + iCount, row);
                    // 更新单元格的内容列表
                    foreach (DomTableCellElement cell in row.Cells)
                    {
                        cell.InnerSetOwnerDocumentParentRaw(document, row);
                        cell.UpdateContentElements(false);
                        cell.InnerExecuteLayout();
                    }
                }
            }
            finally
            {
                DomContainerElement.GlobalEnabledResetChildElementStats = true;
                document._EnableInvalidateViewFunction = back9;
                DomContentElement._UseLineInfo_UpdateLinePosition = back10;
            }
            this.InvalidateView();
            document.FixElementIDForInsertElements(newRows);
            // 更新表格内容版本号
            this.UpdateContentVersion();
            // 更新涉及到的合并单元格的跨行数
            if (specifyRowSpan != null && specifyRowSpan.Count > 0)
            {
                // 设置用户指定的单元格跨行数
                foreach (DomTableCellElement cell in specifyRowSpan.Keys)
                {
                    int newRowSpan = specifyRowSpan[cell];
                    if (cell.RowSpan != newRowSpan)
                    {
                        cell.InternalSetRowSpan(newRowSpan);
                        cell.UpdateContentVersion();
                    }
                }
            }
            else
            {
                if (autoSetRowSpan)
                {
                    // 自动设置单元格跨行数
                    for (int iCount = 0; iCount < index; iCount++)
                    {
                        DomTableRowElement row = (DomTableRowElement)thisRows[iCount];
                        foreach (DomTableCellElement cell in row.Cells)
                        {
                            if (cell.IsOverrided == false
                                && cell.RowSpan > 1
                                && cell.RowIndex + cell.RowSpan + 1 > index)
                            {
                                int newRowSpan = cell.RowSpan + newRows.Count;
                                // 增加单元格的跨行数
                                if (cell.RowSpan != newRowSpan)
                                {
                                    cell.InternalSetRowSpan(newRowSpan);
                                    cell.UpdateContentVersion();
                                }
                            }
                        }//foreach
                    }
                }
            }//foreach
            this._RuntimeRows = null;
            this.UpdateRowIndexColIndex();
            try
            {
                DomContainerElement.GlobalEnabledResetChildElementStats = false;
                DomContentElement._UseLineInfo_UpdateLinePosition = false;
                this.InnerExecuteLayout();
                this.ContentElement.UpdateContentElements(true);
                var cts = this.DocumentContentElement.Content;
                var dce = this.DocumentContentElement;
                dce.RefreshGlobalLines();
                this.InvalidateHighlightInfo();
                this.InvalidateView();
                document.Modified = true;
                this.UpdatePagesForTable(true);
                //XTextContentElement ce = this.ContentElement;
                //ce.UpdateContentElements(true);
                // 设置新的插入点的位置
                DomTableCellElement firstCell = (DomTableCellElement)newRows[0].Elements[0];
                if (firstCell.IsOverrided)
                {
                    firstCell = firstCell.OverrideCell;
                }
                dce.SetSelection(
                    firstCell.PrivateContent[0].ContentIndex, 0);
            }
            finally
            {
                DomContainerElement.GlobalEnabledResetChildElementStats = true;
                DomContentElement._UseLineInfo_UpdateLinePosition = back10;
            }
            if (undo != null)
            {
                // 记录表格的新的结构信息
                undo.NewTableInfo = new TableStructInfo(this);
            }
            if (logUndo && document.BeginLogUndo())
            {
                // 登记撤销操作信息
                document.AddUndo(undo);
                document.EndLogUndo();
                document.OnSelectionChanged();
                document.OnDocumentContentChanged();
            }
        }

        internal bool DOMDeleteRows(
             int startRowIndex,
             int rowsCount,
             bool logUndo)
        {
            if (startRowIndex < 0 || startRowIndex > this.Rows.Count)
            {
                throw new ArgumentOutOfRangeException("startRowIndex=" + startRowIndex);
            }
            if (rowsCount < 0)
            {
                throw new ArgumentOutOfRangeException(string.Format(
                    DCSR.ArgumentOutOfRange_Name_Value_Max_Min,
                    "rowsCount",
                    rowsCount,
                    this.Rows.Count - 1,
                    0));
            }
            if (startRowIndex + rowsCount > this.Rows.Count)
            {
                throw new ArgumentOutOfRangeException(string.Format(
                    DCSR.ArgumentOutOfRange_Name_Value_Max_Min,
                    "startRowIndex + rowsCount",
                    startRowIndex + rowsCount,
                    this.Rows.Count - 1,
                    0));
            }
            DCSoft.Writer.Dom.Undo.XTextUndoTableInfo undo = null;
            if (logUndo)
            {
                undo = new DCSoft.Writer.Dom.Undo.XTextUndoTableInfo();
                undo.OldTableInfo = new TableStructInfo(this);
            }
            this.InvalidateView();
            // 删除相关的高亮度显示区域
            var hm = this.OwnerDocument.HighlightManager;
            if (hm != null)
            {
                for (int iCount = 0; iCount < rowsCount; iCount++)
                {
                    hm.Remove(this.Rows[startRowIndex + iCount]);
                }
            }
            // 删除多个表格行
            this.Rows.RemoveRange(startRowIndex, rowsCount);
            //更新内容版本号
            this.UpdateContentVersion();
            this._RuntimeRows = null;
            this.UpdateRowIndexColIndex();
            //this.FixCells();
            this.UpdateCellOverrideState();
            this.UpdateCellsState(true);
            this.DocumentContentElement.RefreshGlobalLines();
            if (undo != null)
            {
                undo.NewTableInfo = new TableStructInfo(this);
            }
            if (logUndo && this.OwnerDocument.CanLogUndo)
            {
                // 登记撤销操作信息
                this.OwnerDocument.AddUndo(undo);
            }
            return true;
        }

        /// <summary>
        /// 在编辑器中删除多个连续的表格行
        /// </summary>
        /// <param name="startRowIndex">开始行号</param>
        /// <param name="rowsCount">要删除的行数</param>
        /// <param name="logUndo">是否记录撤销操作信息</param>
        /// <param name="specifyRowSpan">用户指定的单元格跨行数修正值</param>

        public bool EditorDeleteRows2(
            int startRowIndex,
            int rowsCount,
            bool logUndo,
            Dictionary<DomTableCellElement, int> specifyRowSpan)
        {
            if (startRowIndex < 0 || startRowIndex > this.Rows.Count)
            {
                throw new ArgumentOutOfRangeException("startRowIndex=" + startRowIndex);
            }
            if (rowsCount < 0)
            {
                throw new ArgumentOutOfRangeException(string.Format(
                    DCSR.ArgumentOutOfRange_Name_Value_Max_Min,
                    "rowsCount",
                    rowsCount,
                    this.Rows.Count - 1,
                    0));
            }
            if (startRowIndex + rowsCount > this.Rows.Count)
            {
                throw new ArgumentOutOfRangeException(string.Format(
                    DCSR.ArgumentOutOfRange_Name_Value_Max_Min,
                    "startRowIndex + rowsCount",
                    startRowIndex + rowsCount,
                    this.Rows.Count - 1,
                    0));
            }
            DCSoft.Writer.Dom.Undo.XTextUndoTableInfo undo = null;
            if (logUndo)
            {
                undo = new DCSoft.Writer.Dom.Undo.XTextUndoTableInfo();
                undo.OldTableInfo = new TableStructInfo(this);
            }
            this.InvalidateView();
            // 删除相关的高亮度显示区域
            if (this.OwnerDocument.HighlightManager != null)
            {
                for (int iCount = 0; iCount < rowsCount; iCount++)
                {
                    this.OwnerDocument.HighlightManager.Remove(this.Rows[startRowIndex + iCount]);
                }
            }
            // 删除多个表格行
            this.Rows.RemoveRange(startRowIndex, rowsCount);
            //更新内容版本号
            this.UpdateContentVersion();
            // 更新涉及到的合并单元格的跨行数
            if (specifyRowSpan != null && specifyRowSpan.Count > 0)
            {
                // 设置用户指定的跨行数
                foreach (DomTableCellElement cell in specifyRowSpan.Keys)
                {
                    cell.InternalSetRowSpan(specifyRowSpan[cell]);
                }
            }
            else
            {
                // 自动计算
                for (int iCount = 0; iCount < startRowIndex; iCount++)
                {
                    DomTableRowElement row = (DomTableRowElement)this.Rows[iCount];
                    foreach (DomTableCellElement cell in row.Cells)
                    {
                        if (cell.IsOverrided == false
                            && cell.RowSpan > 1
                            && cell.RowIndex + cell.RowSpan + 1 > startRowIndex)
                        {
                            // 计算新的跨行数
                            int newRowSpan = cell.RowSpan - rowsCount;
                            if (newRowSpan < startRowIndex - iCount)
                            {
                                newRowSpan = startRowIndex - iCount;
                            }
                            // 设置新的跨行数
                            cell.InternalSetRowSpan(newRowSpan);
                        }
                    }//for
                }//for
            }
            try
            {
                DomContainerElement.GlobalEnabledResetChildElementStats = false;
                this._RuntimeRows = null;
                this.InnerExecuteLayout();
                this.InvalidateView();
                this.OwnerDocument.Modified = true;
                this.ContentElement.UpdateContentElements(true);
                this.DocumentContentElement.RefreshGlobalLines();
                this.UpdatePagesForTable(true);
            }
            finally
            {
                DomContainerElement.GlobalEnabledResetChildElementStats = true;
            }
            // 设置当前插入点的位置
            if (startRowIndex > this.Rows.Count)
            {
                startRowIndex = this.Rows.Count - 1;
            }
            //设置新的插入点的位置
            DomTableCellElement cell2 = GetCell(Math.Max(0, startRowIndex - 1), 0, false);
            if (cell2.IsOverrided)
            {
                cell2 = cell2.OverrideCell;
            }
            this.DocumentContentElement.Content.LineEndFlag = false;
            this.DocumentContentElement.SetSelection(cell2.PrivateContent[0].ContentIndex, 0);
            if (undo != null)
            {
                undo.NewTableInfo = new TableStructInfo(this);
            }
            if (logUndo && this.OwnerDocument.BeginLogUndo())
            {
                // 登记撤销操作信息
                this.OwnerDocument.AddUndo(undo);
                this.OwnerDocument.EndLogUndo();
                this.OwnerDocument.OnSelectionChanged();
                this.OwnerDocument.OnDocumentContentChanged();
            }
            return true;
        }

        /// <summary>
        /// 在编辑器中插入多个表格列.DCWriter内部使用。
        /// </summary>
        /// <param name="index">插入的表格列的开始序号</param>
        /// <param name="newColumns">新表格列对象</param>
        /// <param name="newCells">指定的新表格列对应的单元格对象</param>
        /// <param name="logUndo">是否记录撤销操作信息</param>
        /// <param name="specifyColSpan">用户指定的新跨列数</param>
        /// <param name="keepTableWidth"> 保持表格宽度</param>
        /// <param name="specifyColumWidths">指定的列宽</param>

        public void EditorInsertColumns2(
            int index,
            DomElementList newColumns,
            DomElementList newCells,
            bool logUndo,
            Dictionary<DomTableCellElement, int> specifyColSpan,
            bool keepTableWidth,
            Dictionary<DomTableColumnElement, float> specifyColumWidths)
        {
            // 检查参数
            if (index < 0 || index > this.Columns.Count)
            {
                throw new ArgumentOutOfRangeException(string.Format(
                    DCSR.ArgumentOutOfRange_Name_Value_Max_Min,
                    "index",
                    index,
                    this.Columns.Count - 1,
                    0));
            }

            if (newColumns == null || newColumns.Count == 0)
            {
                throw new ArgumentNullException("newColumns");
            }
            // 获得旧的表格宽度
            float oldTableWidth = 0;
            foreach (DomTableColumnElement col in this.Columns)
            {
                if (col.RuntimeVisible)
                {
                    oldTableWidth = oldTableWidth + col.Width;
                }
            }
            var minTableColumnWidth = this.GetDocumentViewOptions().MinTableColumnWidth;
            foreach (DomTableColumnElement col in newColumns)
            {
                if (this.Columns.Contains(col))
                {
                    throw new ArgumentException("col existed in table");
                }//if
                // 修正表格列宽度
                if (col.Width < minTableColumnWidth)
                {
                    col.Width = minTableColumnWidth;
                }
                col.InnerSetOwnerDocumentParentRaw(this._OwnerDocument, this);
            }//foreach
            DCSoft.Writer.Dom.Undo.XTextUndoTableInfo undo = null;
            if (logUndo)
            {
                undo = new DCSoft.Writer.Dom.Undo.XTextUndoTableInfo();
                undo.OldTableInfo = new TableStructInfo(this);
            }
            DomElementList resultNewCells = new DomElementList();
            this.FixCells();
            // 插入表格列对象
            this.Columns.InsertRange(index, newColumns);
            // 更新表格内容版本号
            this.UpdateContentVersion();
            if (newCells != null && newCells.Count == this.Rows.Count * newColumns.Count)
            {
                // 方法参数提供了现成的单元格对象
                for (int iCount = 0; iCount < this.Rows.Count; iCount++)
                {
                    DomTableRowElement row = (DomTableRowElement)this.Rows[iCount];
                    for (int iCount2 = 0; iCount2 < newColumns.Count; iCount2++)
                    {
                        //将现成的单元格对象插入到各个表格行中
                        DomTableCellElement cell = (DomTableCellElement)newCells[
                            iCount * newColumns.Count + iCount2];
                        cell.InnerSetOwnerDocumentParentRaw(this._OwnerDocument, row);
                        cell.FixElements();
                        row.Cells.Insert(index + iCount2, cell);
                        resultNewCells.Add(cell);
                    }//for
                }//for
            }
            else
            {
                //自动为新增表格列创建单元格
                using (DCGraphics g = this.InnerCreateDCGraphics())
                {
                    foreach (DomTableRowElement row in this.Rows)
                    {
                        DomTableCellElement preCell = null;
                        if (index == 0)
                        {
                            preCell = (DomTableCellElement)row.Cells[index];
                        }
                        else
                        {
                            preCell = (DomTableCellElement)row.Cells[index - 1];
                        }
                        if (preCell.IsOverrided)
                        {
                            preCell = preCell.OverrideCell;
                        }
                        foreach (DomTableColumnElement col in newColumns)
                        {
                            // 为新增的表格列添加相应的单元格对象
                            DomTableCellElement cell = null;
                            if (preCell != null)
                            {
                                // 存在前置单元格则复制
                                cell = preCell.EditorClone();
                                cell.ColSpan = 1;
                            }
                            else
                            {
                                cell = this.CreateCellInstance();
                                // 创建新单元格中的第一个段落元素对象
                                DomParagraphFlagElement p = new DomParagraphFlagElement();
                                p.AutoCreate = true;
                                //if (preCell != null)
                                {
                                    foreach (DomElement element in preCell.Elements)
                                    {
                                        if (element is DomParagraphFlagElement)
                                        {
                                            // 设置新的段落元素对象的样式
                                            p.StyleIndex = element.StyleIndex;
                                            break;
                                        }
                                    }
                                }
                                cell.Elements.Clear();
                                p.InnerSetOwnerDocumentParentRaw(this._OwnerDocument, cell);
                                cell.Elements.Add(p);
                                cell.FixElements();
                            }
                            cell.InnerSetOwnerDocumentParentRaw(this._OwnerDocument, row);
                            cell._StyleIndex = preCell._StyleIndex;
                            row.Cells.Insert(index, cell);
                            resultNewCells.Add(cell);
                            InnerDocumentPaintEventArgs args = this.OwnerDocument.CreateInnerPaintEventArgs(g);
                            args.Element = cell;
                            cell.RefreshSize(args);
                        }//foreach
                    }//foreach
                }//using
            }

            // 修正涉及到的横向合并单元格的跨列数
            if (specifyColSpan != null && specifyColSpan.Count > 0)
            {
                // 设置用户指定的跨列数
                foreach (DomTableCellElement cell in specifyColSpan.Keys)
                {
                    cell.InternalSetColSpan(specifyColSpan[cell]);
                }
            }
            else
            {
                // 自动计算
                foreach (DomTableCellElement cell in this.VisibleCells)
                {
                    if (cell.ColSpan > 1
                        && cell.ColIndex + cell.ColSpan - 1 > index
                        && cell.ColIndex < index)
                    {
                        int newColSpan = cell.ColSpan + newColumns.Count;
                        cell.InternalSetColSpan(newColSpan);
                    }
                }//for
            }
            // 重新排版
            if (specifyColumWidths != null && specifyColumWidths.Count > 0)
            {
                // 用户指定的表格列的宽度
                foreach (DomTableColumnElement col in this.Columns)
                {
                    if (specifyColumWidths.ContainsKey(col))
                    {
                        col.Width = specifyColumWidths[col];
                    }
                }
            }
            else if (keepTableWidth)
            {
                // 保持表格总体宽度，因此需要调整各个表格列的宽度
                float newTableWidth = 0;
                foreach (DomTableColumnElement col in this.Columns)
                {
                    newTableWidth = newTableWidth + col.Width;
                }
                float rate = oldTableWidth / newTableWidth;
                foreach (DomTableColumnElement col in this.Columns)
                {
                    col.Width = col.Width * rate;
                }
            }
            if (resultNewCells.Count > 0)
            {
                this.OwnerDocument.FixElementIDForInsertElements(resultNewCells);
            }
            float oldHeight = this.Height;
            try
            {
                DomContainerElement.GlobalEnabledResetChildElementStats = false;
                this._RuntimeRows = null;
                this.InnerExecuteLayout();
                this.ContentElement.UpdateContentElements(true);
                this.DocumentContentElement.RefreshGlobalLines();
                this.InvalidateHighlightInfo();
                this.InvalidateView();
                this.OwnerDocument.Modified = true;
                this.UpdatePagesForTable(oldHeight != this.Height);
            }
            finally
            {
                DomContainerElement.GlobalEnabledResetChildElementStats = true;
            }
            DomTableCellElement cell2 = GetCell(0, index, false);
            // 设置新的插入点位置
            if (cell2.IsOverrided)
            {
                cell2 = cell2.OverrideCell;
            }
            var cet = this.DocumentContentElement.Content;
            this.DocumentContentElement.SetSelection(cell2.PrivateContent[0].ContentIndex, 0);
            if (logUndo && this.OwnerDocument.BeginLogUndo())
            {
                // 记录撤销操作信息
                undo.NewTableInfo = new TableStructInfo(this);
                this.OwnerDocument.AddUndo(undo);
                this.OwnerDocument.EndLogUndo();
                this.OwnerDocument.OnSelectionChanged();
                this.OwnerDocument.OnDocumentContentChanged();
            }
        }

        /// <summary>
        /// 在编辑器中删除多个连续的表格列对象
        /// </summary>
        /// <param name="startColumnIndex">要删除的表格列的开始序号</param>
        /// <param name="colCount">表格列个数</param>
        /// <param name="logUndo">是否记录撤销操作信息</param>
        /// <param name="specifyColSpan">指定的跨列数</param>
        /// <param name="keepTableWidth">保持表格宽度</param>
        /// <param name="specifyColumnWidths">指定的列宽</param>

        public bool EditorDeleteColumns2(
            int startColumnIndex,
            int colCount,
            bool logUndo,
            Dictionary<DomTableCellElement, int> specifyColSpan,
            bool keepTableWidth,
            Dictionary<DomTableColumnElement, float> specifyColumnWidths)
        {
            // 检查参数
            if (startColumnIndex < 0 || startColumnIndex >= this.Columns.Count)
            {
                throw new ArgumentOutOfRangeException(string.Format(
                    DCSR.ArgumentOutOfRange_Name_Value_Max_Min,
                    "startColumnIndex",
                    startColumnIndex,
                    this.Columns.Count - 1,
                    0));
            }
            if (colCount <= 0)
            {
                throw new ArgumentOutOfRangeException("colCount=" + colCount);
            }
            if (startColumnIndex + colCount > this.Columns.Count)
            {
                throw new ArgumentOutOfRangeException(string.Format(
                    DCSR.ArgumentOutOfRange_Name_Value_Max_Min,
                    "startIndex+Count",
                    startColumnIndex + colCount,
                    this.Columns.Count - 1,
                    0));
            }

            // 获得旧的表格宽度
            float oldTableWidth = 0;
            foreach (DomTableColumnElement col in this.Columns)
            {
                oldTableWidth = oldTableWidth + col.Width;
            }
            DCSoft.Writer.Dom.Undo.XTextUndoTableInfo undo = null;
            if (logUndo)
            {
                undo = new DCSoft.Writer.Dom.Undo.XTextUndoTableInfo();
                undo.OldTableInfo = new TableStructInfo(this);
            }
            var dce = this.DocumentContentElement;
            var document = this.OwnerDocument;
            var currentRowIndex = 0;
            var currentCell = (DomTableCellElement)dce.Content.GetCurrentContainer(typeof(DomTableCellElement));
            if (currentCell != null && currentCell.OwnerTable == this)
            {
                currentRowIndex = currentCell.RowIndex;
            }
            // 删除表格列
            this.Columns.RemoveRange(startColumnIndex, colCount);
            // 更新元素内容版本号
            this.UpdateContentVersion();
            // 删除表格列对应的单元格对象
            foreach (DomTableRowElement row in this.Rows)
            {
                if (row.Cells.Count > startColumnIndex)
                {
                    if (document.HighlightManager != null)
                    {
                        for (int iCount = 0; iCount < colCount; iCount++)
                        {
                            // 记录被删除的单元格对象
                            DomElement cell = row.Cells[startColumnIndex + iCount];
                            //if (undo != null)
                            //{
                            //    undo.RemovedCells.Add(row.Cells[startColumnIndex + iCount]);
                            //}
                            document.HighlightManager.Remove(cell);
                        }
                    }
                    // 删除表格列下的单元格对象
                    row.Cells.RemoveRange(startColumnIndex, colCount);
                    // 修正涉及到本操作的合并单元格的跨列数
                    if (specifyColSpan != null && specifyColSpan.Count > 0)
                    {
                        // 用户指定了合并单元格的跨列数
                        foreach (DomTableCellElement cell in specifyColSpan.Keys)
                        {
                            cell.InternalSetColSpan(specifyColSpan[cell]);
                        }
                    }
                    else
                    {
                        // 自动修正跨列数
                        for (int iCount = 0; iCount < startColumnIndex; iCount++)
                        {
                            DomTableCellElement cell = (DomTableCellElement)row.Cells[iCount];
                            if (cell.IsOverrided == false
                                && cell.ColSpan > 1
                                && cell.ColIndex + cell.ColSpan - 1 >= startColumnIndex)
                            {
                                int newColSpan = cell.ColSpan - colCount;
                                if (newColSpan < startColumnIndex - iCount)
                                {
                                    newColSpan = startColumnIndex - iCount;
                                }
                                cell.InternalSetColSpan(newColSpan);
                                break;
                            }
                        }//for
                    }
                }//if
            }//foreach
            // 重新排版
            if (specifyColumnWidths != null && specifyColumnWidths.Count > 0)
            {
                foreach (DomTableColumnElement col in this.Columns)
                {
                    if (specifyColumnWidths.ContainsKey(col))
                    {
                        col.Width = specifyColumnWidths[col];
                    }
                }
            }
            else if (keepTableWidth)
            {
                // 保持表格总体宽度，因此需要调整各个表格列的宽度
                float newTableWidth = 0;
                foreach (DomTableColumnElement col in this.Columns)
                {
                    newTableWidth = newTableWidth + col.Width;
                }
                float rate = oldTableWidth / newTableWidth;
                foreach (DomTableColumnElement col in this.Columns)
                {
                    col.Width = col.Width * rate;
                }
            }
            float oldHeight = this.Height;
            try
            {
                DomContainerElement.GlobalEnabledResetChildElementStats = false;
                this.InvalidateView();
                this.InnerExecuteLayout();
                this.InvalidateView();
                this.ContentElement.UpdateContentElements(true);
                dce.RefreshGlobalLines();
                this.UpdatePagesForTable(oldHeight != this.Height);
                document.Modified = true;
                // 设置新的插入点位置
                DomTableCellElement cell2 = GetCell(
                    currentRowIndex,
                    startColumnIndex >= this.Columns.Count ? startColumnIndex - 1 : startColumnIndex,
                    false);
                if (cell2 == null)
                {
                    cell2 = GetCell(
                        0,
                        startColumnIndex >= this.Columns.Count ? startColumnIndex - 1 : startColumnIndex,
                        false);
                }
                if (cell2.IsOverrided)
                {
                    cell2 = cell2.OverrideCell;
                }
                var c3 = dce.Content;
                dce.SetSelection(cell2.PrivateContent[0].ContentIndex, 0);
            }
            finally
            {
                DomContainerElement.GlobalEnabledResetChildElementStats = true;
            }
            if (undo != null && document.BeginLogUndo())
            {
                // 记录撤销操作信息
                undo.NewTableInfo = new TableStructInfo(this);
                document.AddUndo(undo);
                document.EndLogUndo();
                document.OnSelectionChanged();
                document.OnDocumentContentChanged();
            }
            return true;
        }

        /// <summary>
        /// 应用表格状态
        /// </summary>
        /// <param name="tableInfo">表格状态信息对象</param>
        /// <returns>操作是否成功</returns>
        internal bool EditorApplyTableStructInfo(TableStructInfo tableInfo)
        {
            // 检查参数
            if (tableInfo == null)
            {
                throw new ArgumentNullException("tableInfo");
            }
            if (tableInfo.TableInstance != this)
            {
                throw new ArgumentException("info 不属于该表格");
            }
            try
            {
                DomContainerElement.GlobalEnabledResetChildElementStats = false;
                this.InvalidateView();
                tableInfo.Write(this, true);
                this.InvalidateView();
                //this.ExecuteLayout();
                this.InvalidateView();
                this.ContentElement.UpdateContentElements(true);
                this.DocumentContentElement.RefreshGlobalLines();
                this.UpdatePagesForTable(false);
                this.DocumentContentElement.GlobalUpdateLineIndex();
                this.OwnerDocument.Modified = true;
                // 设置新的插入点位置
                if (tableInfo.SelectionPosition >= 0)
                {
                    this.DocumentContentElement.SetSelection(tableInfo.SelectionPosition, 0);
                }
                this.OwnerDocument.OnSelectionChanged();
                this.OwnerDocument.OnDocumentContentChanged();
            }
            finally
            {
                DomContainerElement.GlobalEnabledResetChildElementStats = true;
            }
            return true;
        }

        /// <summary>
        /// 设置表格标题行样式
        /// </summary>
        /// <param name="headerStyles">新的标题行样式</param>
        /// <param name="logUndo">是否记录撤销操作信息</param>
        /// <returns>操作是否修改了文档内容</returns>
        public bool EditorSetHeaderRow(
            Dictionary<DomTableRowElement, bool> headerStyles,
            bool logUndo)
        {
            if (headerStyles == null || headerStyles.Count == 0)
            {
                return false;
            }
            bool modified = false;
            var undo = new DCSoft.Writer.Dom.Undo.XTextUndoHeaderRow();
            undo.Table = this;
            foreach (DomTableRowElement row in headerStyles.Keys)
            {
                if (row.HeaderStyle == headerStyles[row])
                {
                    continue;
                }
                modified = true;
                undo.OldHeaderStyles[row] = row.HeaderStyle;
                undo.NewHeaderStyles[row] = headerStyles[row];
                row.HeaderStyle = headerStyles[row];
            }//foreach
            if (modified)
            {
                this.UpdateContentVersion();
                var document = this.OwnerDocument;
                document.Modified = true;
                if (logUndo && document.CanLogUndo)
                {
                    document.AddUndo(undo);
                }
                this._RuntimeRows = null;
                this.UpdateRowPosition();
                this.UpdatePagesForTable(true);
                // 触发文档内容发生改变事件
                ContentChangedEventArgs args2 = new ContentChangedEventArgs();
                args2.Element = this;
                this.RaiseBubbleOnContentChanged(args2);
            }
            return modified;
        }

        #endregion


        /// <summary>
        /// 创建一个新的单元格对象
        /// </summary>
        /// <remarks>
        /// 从XDesignerLib上派生的设计器中可以实现扩展表格模型,实现自己的
        /// 表格单元格元素类型,此时需要重载该函数来返回扩展的表格单元格
        /// 对象实例.
        /// </remarks>
        /// <returns>新的单元格对象</returns>
        public DomTableCellElement CreateCellInstance()
        {
            DomTableCellElement cell = new DomTableCellElement();
            cell.SetOwnerDocumentRaw(this._OwnerDocument);
            return cell;
        }

        /// <summary>
        /// 创建一个新的单元格对象
        /// </summary>
        /// <param name="templateCell">用作模板的单元格对象</param>
        /// <remarks>
        /// 从XDesignerLib上派生的设计器中可以实现扩展表格模型,实现自己的
        /// 表格单元格元素类型,此时需要重载该函数来返回扩展的表格单元格
        /// 对象实例.
        /// </remarks>
        /// <returns>新的单元格对象</returns>
        public DomTableCellElement CreateCellInstance(DomTableCellElement templateCell)
        {
            DomTableCellElement cell = CreateCellInstance();
            if (templateCell != null)
            {
                cell.StyleIndex = templateCell.StyleIndex;
                cell.Elements.Clear();
                if (templateCell.Elements.LastElement is DomParagraphFlagElement)
                {
                    DomParagraphFlagElement pf = new DomParagraphFlagElement();
                    pf.StyleIndex = templateCell.Elements.LastElement.StyleIndex;
                    if (pf.StyleIndex < 0)
                    {
                        pf.AutoCreate = true;
                    }
                    cell.Elements.Add(pf);
                }
            }
            return cell;
        }

        /// <summary>
        /// 创建一个新的表格列对象
        /// </summary>
        /// <remarks>
        /// 从XDesignerLib上派生的设计器中可以实现扩展表格模型,实现自己的
        /// 表格单元格元素类型,此时需要重载该函数来返回扩展的表格列
        /// 对象实例.
        /// </remarks>
        /// <returns>新的表格列对象</returns>
        public DomTableColumnElement CreateColumnInstance()
        {
            DomTableColumnElement col = new DomTableColumnElement();
            col.InnerSetOwnerDocumentParentRaw(this._OwnerDocument, this);
            return col;
        }
        /// <summary>
        /// 创建一个新的表格行对象
        /// </summary>
        /// <remarks>
        /// 从XDesignerLib上派生的设计器中可以实现扩展表格模型,实现自己的
        /// 表格单元格元素类型,此时需要重载该函数来返回扩展的表格行
        /// 对象实例.
        /// </remarks>
        /// <returns>新的表格行对象</returns>
        public DomTableRowElement CreateRowInstance()
        {
            DomTableRowElement row = new DomTableRowElement();
            row.InnerSetOwnerDocumentParentRaw(this._OwnerDocument, this);
            return row;
        }
        /// <summary>
        /// 复制对象
        /// </summary>
        /// <param name="Deeply">是否复制子孙节点</param>
        /// <returns>复制品</returns>
        public override DomElement Clone(bool Deeply)
        {
            DomTableElement table = (DomTableElement)base.Clone(Deeply);
            table._RuntimeRows = null;
            //if (Deeply)
            if (this.Columns != null)
            {
                table._Columns = new DomElementList();
                this._Columns.FastCloneDeeplyTo(table._Columns, table, this.OwnerDocument);
            }
            //else
            //{
            //    table._Columns = new XTextElementList();
            //}
            table.EnumerateCells(delegate (DomTableCellElement cell)
            {
                cell.Width = 0;
                cell.Height = 0;
            });
            //foreach (XTextTableCellElement cell in table.Cells)
            //{
            //    cell.Width = 0;
            //    cell.Height = 0;
            //}
            return table;
        }

        public override void InnerGetText(GetTextArgs args)
        {
            args.EnsureNewLine();
            foreach (DomTableRowElement row in this.RuntimeRows.FastForEach())
            {
                if (args.IncludeHiddenText == false && row.RuntimeVisible == false)
                {
                    continue;
                }
                args.EnsureNewLine();
                foreach (DomTableCellElement cell in row.Cells.FastForEach())
                {
                    if (cell.IsOverrided == false)
                    {
                        int len = args.Length;
                        cell.InnerGetText(args);
                        if (len == args.Length)
                        {
                            args.Append(' ');
                        }
                        args.Append(' ');
                        //string txt = cell.Text;
                        //if (string.IsNullOrEmpty(txt))
                        //{
                        //    str.Append(" ");
                        //}
                        //else
                        //{
                        //    str.Append(txt);
                        //}
                        //str.Append(" ");
                    }
                }
            }
        }
        public override void WriteText(WriteTextArgs args)
        {
            var rowsArr = this.RuntimeRows.InnerGetArrayRaw();
            var rowsLen = this.RuntimeRows.Count;
            //foreach (XTextTableRowElement row in this.RuntimeRows.FastForEach())
            for (var rowIndex = 0; rowIndex < rowsLen; rowIndex++)
            {
                var row = (DomTableRowElement)rowsArr[rowIndex];
                if (args.Length > 0)
                {
                    args.AppendLine();
                }
                var cellArr = row.Cells.InnerGetArrayRaw();
                var cellLen = row.Cells.Count;
                //foreach (XTextTableCellElement cell in row.Cells.FastForEach())
                for (var cellIndex = 0; cellIndex < cellLen; cellIndex++)
                {
                    var cell = (DomTableCellElement)cellArr[cellIndex];
                    if (cell.IsOverrided == false)
                    {
                        int len = args.Length;
                        cell.WriteText(args);
                        if (len == args.Length)
                        {
                            args.Append(' ');
                        }
                        args.Append(' ');
                    }
                }
            }
        }


        /// <summary>
        /// 返回表示对象的文本
        /// </summary>
        public override string OuterText
        {
            get
            {
                var args = new WriteTextArgs(this._OwnerDocument, true);
                WriteText(args);
                return args.ToString();
            }
            set
            {
                // 操作无效
            }
        }


        /// <summary>
        /// 返回表示对象的文本
        /// </summary>
        public override string Text
        {
            get
            {
                var args = new WriteTextArgs(this._OwnerDocument);
                WriteText(args);
                return args.ToString();
            }
            set
            {
                // 操作无效
            }
        }


        /// <summary>
        /// 判断是否包含直接子元素
        /// </summary>
        /// <param name="element">子元素</param>
        /// <returns>是否包含</returns>
        public override bool ContainsChild(DomElement element)
        {
            if (element is DomTableRowElement)
            {
                return base.ContainsChild(element);
            }
            else if (element is DomTableColumnElement && this._Columns != null)
            {
                if (this._Columns.EqualItem(element._ElementIndex, element))
                {
                    return true;
                }
                return this._Columns.Contains(element);
            }
            return false;
        }

        public override void OnGotFocus(ElementEventArgs args)
        {
            this.InvalidateView();
            base.OnGotFocus(args);
        }
        public override void OnLostFocus(ElementEventArgs args)
        {
            this.InvalidateView();
            base.OnLostFocus(args);
        }

        public override void Dispose()
        {
            base.Dispose();
            if (this._Columns != null)
            {
                foreach (DomTableColumnElement col in this._Columns)
                {
                    col.Dispose();
                }
                this._Columns.Clear();
                this._Columns = null;
            }
            if (this._RuntimeRows != null)
            {
                this._RuntimeRows.Clear();
                this._RuntimeRows = null;
            }
        }

    }//public class XTextTableElement : XTextContainerElement
}