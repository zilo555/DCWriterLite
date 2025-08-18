using System;
using System.Collections.Generic;
using System.Text;

namespace DCSoft.Writer.Dom
{
    internal class TableStructInfo : IDisposable
    {
        //public TableStructInfo()
        //{
        //}

        public TableStructInfo(DomTableElement table)
        {
            Read(table, false);
        }

        public TableStructInfo(DomTableElement table, bool backElements)
        {
            Read(table, backElements);
        }
        public void Dispose()
        {
            this._TableInstance = null;
            if (this._Rows != null)
            {
                foreach (var row in this._Rows)
                {
                    row.Dispose();
                }
                Array.Clear(this._Rows);
                this._Rows = null;
            }
            if (this._Columns != null)
            {
                foreach (var item in this._Columns)
                {
                    item.Dispose();
                }
                Array.Clear(this._Columns);
                this._Columns = null;
            }

        }
        private int _SelectionPosition = -1;

        public int SelectionPosition
        {
            get { return _SelectionPosition; }
            set { _SelectionPosition = value; }
        }

        private DomTableElement _TableInstance = null;

        public DomTableElement TableInstance
        {
            get { return _TableInstance; }
            set { _TableInstance = value; }
        }


        private float _Width = 0f;

        public float Width
        {
            get { return _Width; }
            set { _Width = value; }
        }

        private float _Height = 0f;

        public float Height
        {
            get { return _Height; }
            set { _Height = value; }
        }
        private TableColumnStructInfo[] _Columns = null;

        internal TableColumnStructInfo[] Columns
        {
            get { return _Columns; }
            //set { _Columns = value; }
        }

        private TableRowStructInfo[] _Rows = null;

        internal TableRowStructInfo[] Rows
        {
            get { return _Rows; }
            //set { _Rows = value; }
        }

        /// <summary>
        /// 读取表格DOM结构信息
        /// </summary>
        /// <param name="table">表格对象</param>
        /// <param name="backElements">备份内容文档元素对象</param>
        public void Read(DomTableElement table, bool backElements)
        {
            this.SelectionPosition = table.OwnerDocument.Selection.StartIndex;
            this.TableInstance = table;
            this.Width = table.Width;
            this.Height = table.Height;
            this._Columns = new TableColumnStructInfo[table.Columns.Count];
            var index = 0;
            foreach (DomTableColumnElement col in table.Columns)
            {
                this._Columns[index++] = new TableColumnStructInfo(col);
            }
            this._Rows = new TableRowStructInfo[table.Rows.Count];
            index = 0;
            foreach (DomTableRowElement row in table.Rows)
            {
                var rowInfo = new TableRowStructInfo(row);
                this._Rows[index++] = rowInfo;
                rowInfo.Cells = new TableCellStructInfo[row.Cells.Count];
                var index2 = 0;
                foreach (DomTableCellElement cell in row.Cells)
                {
                    rowInfo.Cells[index2++] = new TableCellStructInfo(cell, backElements);
                }
            }
        }

        /// <summary>
        /// 保存表格DOM结构
        /// </summary>
        /// <param name="table">表格对象</param>
        /// <param name="updateCellLayout">更新单元格排版</param>
        public void Write(DomTableElement table, bool updateCellLayout)
        {
            table.Width = this.Width;
            table.Height = this.Height;
            table.Columns.Clear();
            foreach (TableColumnStructInfo colInfo in this.Columns)
            {
                DomTableColumnElement col = colInfo.ColumnInstance;
                col.Width = colInfo.Width;
                col.Left = colInfo.Left;
                col.SetParentRaw(table);
                table.Columns.Add(col);
            }
            table.Rows.Clear();
            //if (table.RuntimeRows != null)
            //{
            //    table.RuntimeRows.Clear();
            //}
            foreach (TableRowStructInfo rowInfo in this.Rows)
            {
                DomTableRowElement row = rowInfo.RowInstance;
                table.Rows.Add(row);
                row.SpecifyHeight = rowInfo.SpecifyHeight;
                row.Top = rowInfo.Top;
                row.Height = rowInfo.Height;
                row.Parent = table;
                row.Cells.Clear();
                foreach (TableCellStructInfo cellInfo in rowInfo.Cells)
                {
                    DomTableCellElement cell = cellInfo.CellInstance;
                    cell.InternalSetRowSpan(cellInfo.RowSpan);
                    cell.InternalSetColSpan(cellInfo.ColSpan);
                    cell.SetOverrideCell(cellInfo.OverridedCell);
                    cell.Left = cellInfo.Left;
                    cell.Top = cellInfo.Top;
                    cell.Width = cellInfo.Width;
                    cell.Height = cellInfo.Height;
                    cell.Parent = row;

                    row.Cells.Add(cell);
                    if (cellInfo.Elements != null)
                    {
                        cell.Elements.Clear();
                        cell.Elements.AddRange(cellInfo.Elements);
                        cell.UpdateContentElements(true);
                    }
                    if (updateCellLayout && cell.OverrideCell == null)
                    {
                        cell.InnerExecuteLayout();
                        cell.RefreshPrivateLinesOwnerPage();
                    }
                }
            }
            table.InnerFixDomState(false, true);
        }
    }

    internal class TableRowStructInfo:IDisposable
    {
        public TableRowStructInfo( DomTableRowElement row )
        {
            this._RowInstance = row;
            this._SpecifyHeight = row.SpecifyHeight;
            this._Top = row.Top;
            this._Height = row.Height;
        }
        public void Dispose()
        {
            this._RowInstance = null;
            if (this.Cells != null)
            {
                foreach (var item in this.Cells)
                {
                    item.Dispose();
                }
                Array.Clear(this.Cells);
                this.Cells = null;
            }
        }

        private DomTableRowElement _RowInstance = null;

        public DomTableRowElement RowInstance
        {
            get { return _RowInstance; }
            //set { _RowInstance = value; }
        }

        private readonly float _SpecifyHeight = 0f;

        public float SpecifyHeight
        {
            get { return _SpecifyHeight; }
            //set { _SpecifyHeight = value; }
        }

        private readonly float _Top = 0f;

        public float Top
        {
            get { return _Top; }
            //set { _Top = value; }
        }
        private readonly float _Height = 0f;

        public float Height
        {
            get { return _Height; }
            //set { _Height = value; }
        }
        internal TableCellStructInfo[] Cells = null;
         
    }

    internal class TableColumnStructInfo : IDisposable
    {
        public TableColumnStructInfo( DomTableColumnElement col )
        {
            this._ColumnInstance = col;
            this._Left = col.Left;
            this._Width = col.Width;
        }
        public void Dispose()
        {
            this._ColumnInstance = null;
        }
        private DomTableColumnElement _ColumnInstance = null;

        public DomTableColumnElement ColumnInstance
        {
            get { return _ColumnInstance; }
            //set { _ColumnInstance = value; }
        }

        private readonly float _Left = 0f;

        public float Left
        {
            get { return _Left; }
            //set { _Left = value; }
        }

        private readonly float _Width = 0f;

        public float Width
        {
            get { return _Width; }
            //set { _Width = value; }
        }
    }

    internal class TableCellStructInfo:IDisposable
    {
        public TableCellStructInfo( DomTableCellElement cell , bool backElements )
        {
            this._CellInstance = cell;
            this._OverridedCell = cell.OverrideCell;
            this._RowSpan = cell.RowSpan;
            this._ColSpan = cell.ColSpan;
            this._Left = cell.Left;
            this._Top = cell.Top;
            this._Width = cell.Width;
            this._Height = cell.Height;
            if( backElements )
            {
                if( cell.Elements != null )
                {
                    this._Elements = cell.Elements.ToArray();
                }
            }
        }
        public void Dispose()
        {
            this._CellInstance = null;
            this._OverridedCell = null;
        }
        private DomTableCellElement _CellInstance = null;

        public DomTableCellElement CellInstance
        {
            get { return _CellInstance; }
            //set { _CellInstance = value; }
        }

        private DomTableCellElement _OverridedCell = null;

        public DomTableCellElement OverridedCell
        {
            get { return _OverridedCell; }
            //set { _OverridedCell = value; }
        }

        private readonly int _RowSpan = 1;

        public int RowSpan
        {
            get { return _RowSpan; }
            //set { _RowSpan = value; }
        }

        private readonly int _ColSpan = 1;

        public int ColSpan
        {
            get { return _ColSpan; }
            //set { _ColSpan = value; }
        }
        private readonly float _Left = 0f;

        public float Left
        {
            get { return _Left; }
            //set { _Left = value; }
        }

        private readonly float _Top = 0f;

        public float Top
        {
            get { return _Top; }
            //set { _Top = value; }
        }

        private readonly float _Width = 0f;

        public float Width
        {
            get { return _Width; }
            //set { _Width = value; }
        }

        private readonly float _Height = 0f;

        public float Height
        {
            get { return _Height; }
            //set { _Height = value; }
        }

        private DomElement[] _Elements = null;

        public DomElement[] Elements
        {
            get { return _Elements; }
            set { _Elements = value; }
        }
    }
}
