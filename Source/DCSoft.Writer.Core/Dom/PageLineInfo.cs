using System;
using DCSoft.Printing;
using System.Collections.Generic;

namespace DCSoft.Writer.Dom
{
    /// <summary>
    /// 分页线信息
    /// </summary>
    public class PageLineInfo : IDisposable
    {
        public PageLineInfo()
        {

        }
        public void Dispose()
        {
            if (this.CutLineCells_HandledRows != null)
            {
                this.CutLineCells_HandledRows.Clear();
                this.CutLineCells_HandledRows = null;
            }
            if (this.CutLineCells != null)
            {
                this.CutLineCells.Clear();
                this.CutLineCells = null;
            }
            this.BodyLines = null;
            this.PageSettings = null;
            if (this.Table_HeaderRows != null)
            {
                this.Table_HeaderRows.Clear();
                this.Table_HeaderRows = null;
            }
            this.UnderContentBottomSource = null;
            this._HeaderRows = null;
            this._PageBreakElements = null;
            this._SourceElement = null;
            this._SourceElements = null;

        }
        /// <summary>
        /// 获得表格的标题行列表
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        internal DomElement[] GetHeaderRows(DomTableElement table)
        {
            DomElement[] list = null;
            if (this.Table_HeaderRows.TryGetValue(table, out list) == false)
            {
                list = table.HeaderRows;
                this.Table_HeaderRows[table] = list;
            }
            return list;
        }

        internal Dictionary<DomTableElement, DomElement[]> Table_HeaderRows = new Dictionary<DomTableElement, DomElement[]>();

        public void Clear()
        {
            if (this.Table_HeaderRows != null)
            {
                this.Table_HeaderRows.Clear();
                this.Table_HeaderRows = null;
            }
            this._HeaderRows = null;
            this._PageBreakElements = null;
            this.PageSettings = null;
            this._SourceElement = null;
            this._SourceElements = null;
        }

        private List<DomPageBreakElement> _PageBreakElements = null;
        /// <summary>
        /// 文档中所有的分页符元素
        /// </summary>
        public List<DomPageBreakElement> PageBreakElements
        {
            get
            {
                return this._PageBreakElements;
            }
            set
            {
                this._PageBreakElements = value;
            }
        }
        /// <summary>
        /// 由于强制分页而导致的分页
        /// </summary>
        public bool ForNewPage = false;

        private DomElement _SourceElement = null;
        /// <summary>
        /// 直接导致分页的文档元素对象
        /// </summary>
        public DomElement SourceElement
        {
            get
            {
                return _SourceElement;
            }
            set
            {
                _SourceElement = value;
                if (value != null && _SourceElements.Contains(value) == false)
                {
                    if (value.ContentElement == null)
                    {
                    }
                    _SourceElements.Add(value);
                }
            }
        }

        private DomElementList _SourceElements = new DomElementList();
        /// <summary>
        /// 曾经导致分页的文档元素对象
        /// </summary>
        public DomElementList SourceElements
        {
            get
            {
                return _SourceElements;
            }
            set
            {
                _SourceElements = value;
            }
        }
        internal DomLineList BodyLines = null;
        internal List<DomTableCellElement> CutLineCells = new List<DomTableCellElement>();
        internal List<DomTableRowElement> CutLineCells_HandledRows = new List<DomTableRowElement>();
        internal void AddCutLineCell(DomTableRowElement row)
        {
            if (row != null)
            {
                if (this.CutLineCells_HandledRows.Contains(row))
                {
                    return;
                }
                this.CutLineCells_HandledRows.Add(row);
                var rowAbsTop = row.AbsTop;
                //var dis = this._CurrentPoistion - rowAbsTop;
                // 允许可以将分页符移动到表格行顶端以避免切字
                var canMoveToRowTop = this._CurrentPoistion - rowAbsTop < 100;
                for (var iCount = row.Cells.Count - 1; iCount >= 0; iCount--)
                {
                    var cell = (DomTableCellElement)row.Cells[iCount];
                    if (this.SourceElements != null && this.SourceElements.Contains(cell))
                    {
                        continue;
                    }
                    var cellAbsTop = rowAbsTop;
                    if (cell.IsOverrided)
                    {
                        cell = cell.OverrideCell;
                        cellAbsTop = cell.AbsTop;
                        canMoveToRowTop = false;
                    }
                    if (cell.HasPrivateLines()
                        && this.BodyLines.ContainsByIndexOfDocumentContentLines(cell.PrivateLines.FirstLine))
                    {
                        var pvLines = cell.PrivateLines;
                        float topSpacing = pvLines[0].Top;
                        float bottomSpacing = cell.Height - pvLines.LastLine.Bottom;
                        if (this._CurrentPoistion >= cellAbsTop + topSpacing
                            && this._CurrentPoistion <= cellAbsTop + cell.Height - bottomSpacing)
                        {
                            // 分页线在单元格内容中间，有可能分割文档行了
                            if (canMoveToRowTop)
                            {
                                foreach (var line in pvLines)
                                {
                                    if (this._CurrentPoistion >= cellAbsTop + line.Top + 3
                                        && this._CurrentPoistion < cellAbsTop + line.Top + line.Height - 3)
                                    {
                                        // 确认分页线跨过表格行了，发生切字现象
                                        if (this._CurrentPoistion - cellAbsTop - line.Top > bottomSpacing
                                            && cellAbsTop + line.Top + line.Height - this._CurrentPoistion > topSpacing)
                                        {
                                            // 临时移动文档行无法达到目的，则移动分页线
                                            if (row == row.OwnerTable.Rows.FirstElement)
                                            {
                                                // 表格中的第一行
                                                this._CurrentPoistion = row.OwnerTable.OwnerLine.AbsTop;
                                            }
                                            else
                                            {
                                                this._CurrentPoistion = cellAbsTop;
                                            }
                                            this._SourceElements?.Clear();
                                            this._SourceElement = row;
                                            return;
                                        }
                                        break;
                                    }
                                }
                            }
                            if (this.CutLineCells.Contains(cell) == false)
                            {
                                this.CutLineCells.Add(cell);
                            }
                        }
                    }
                }
            }
        }

        internal void AddCutLineCell(DomTableCellElement cell)
        {
            if (cell != null)
            {
                var cellAbsTop = cell.AbsTop;
                if (cell.IsOverrided)
                {
                    cell = cell.OverrideCell;
                    cellAbsTop = cell.AbsTop;
                }
                if (cellAbsTop < this.CurrentPoistion - 10
                    && cellAbsTop + cell.Height > this.CurrentPoistion + 10
                    && cell.HasPrivateLines()
                    && this.BodyLines.ContainsByIndexOfDocumentContentLines(cell.PrivateLines.FirstLine))
                {
                    if (cellAbsTop + cell.PrivateLines[0].Top + cell.ContentHeight > this.CurrentPoistion + 3)
                    {
                        if (this.CutLineCells.Contains(cell) == false)
                        {
                            this.CutLineCells.Add(cell);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 当前使用的页面设置
        /// </summary>
        public XPageSettings PageSettings = null;

        /// <summary>
        /// 标准页面内容高度
        /// </summary>
        public float StdPageContentHeight = 0;


        /// <summary>
        /// 最小允许的页面内容高度
        /// </summary>
        public float MinPageContentHeight = 50;

        /// <summary>
        /// 从0开始计算的当前页码
        /// </summary>
        public int CurrentPageIndex = 0;


        /// <summary>
        /// 上一次的分页线位置
        /// </summary>
        public float LastPosition = 0;

        //internal XTextLine _CurrentLine = null;
        /// <summary>
        /// 产生分页的文档行对象
        /// </summary>
        public DomLine CurrentLine
        {
            get
            {
                if (_SourceElement == null)
                {
                    return null;
                }
                else
                {
                    return _SourceElement.OwnerLine;
                }
            }
            //set { _CurrentLine = value; }
        }
        internal float _CurrentPoistion = 0;
        /// <summary>
        /// 当前分页线位置
        /// </summary>
        public float CurrentPoistion
        {
            get
            {
                return _CurrentPoistion;
            }
            set
            {
                if (_CurrentPoistion > value)
                {
                    // 分页线位置只能变小，不能变大。
                    _CurrentPoistion = value;
                    this.Modified = true;
                }
            }
        }

        private DomElementList _HeaderRows = new DomElementList();
        /// <summary>
        /// 标题行
        /// </summary>
        internal DomElementList HeaderRows
        {
            get
            {
                return _HeaderRows;
            }
            set
            {
                _HeaderRows = value;
            }
        }

        public void AddHeaderRowRange(DomElement[] rows)
        {
            this._HeaderRows.AddRangeRaw2(rows);
        }

        /// <summary>
        /// 分页位置发生改变标记
        /// </summary>
        public bool Modified = false;


        /// <summary>
        /// 分页线的位置超出内容顶端
        /// </summary>
        internal bool UpContentTop = false;

        public DomElement UnderContentBottomSource = null;
        /// <summary>
        /// 分页线位置超出内容底端
        /// </summary>
        public bool UnderContentBottom = false;
        public int TableStackCount = 0;
        /// <summary>
        /// 复制对象
        /// </summary>
        /// <returns>复制品</returns>
        public PageLineInfo Clone()
        {
            return (PageLineInfo)this.MemberwiseClone();
        }
    }//public class PageLineInfo
}