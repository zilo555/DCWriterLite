using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel ;
using DCSoft.Writer.Data ;
using DCSoft.Printing;
using DCSoft.Data;

namespace DCSoft.Writer.Dom
{
    /// <summary>
    /// 表格行元素
    /// </summary>
    /// <remarks>编写 袁永福</remarks>
    public sealed partial class DomTableRowElement : DomContainerElement
    {
        public static readonly string TypeName_XTextTableRowElement = "XTextTableRowElement";
        public override string TypeName => TypeName_XTextTableRowElement;
        /// <summary>
        /// 初始化对象
        /// </summary>
        public DomTableRowElement()
        {
            this.CanSplitByPageLine = true;
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
            return ElementType.TableCell;
        }


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
        /// 行号
        /// </summary>
        public int RowIndex
        {
            get
            {
                //if (_RowIndex < 0)
                //{
                //    _RowIndex = this.ElementIndex;
                //}
                return this._ElementIndex;
            }
        }

        public string DomDisplayName
        {
            get
            {
                string txt = "Row:" + Convert.ToString(this.Index + 1);
                if (string.IsNullOrEmpty(this.ID) == false)
                {
                    txt = txt + "-" + this.ID;
                }
                return txt;
            }
        }

        public override string ToString()
        {
            return this.DomDisplayName;
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

        public override string Text
        {
            get
            {
                var args = new WriteTextArgs(this._OwnerDocument, true, false);
                foreach( DomTableCellElement cell in this.Cells )
                {
                    if(cell.RuntimeVisible)
                    {
                        if( args.Length > 0 )
                        {
                            args.Append(' ');
                        }
                        cell.WriteText(args);
                    }
                }
                return args.ToString();

                //StringBuilder str = new StringBuilder();
                //foreach (XTextTableCellElement cell in this.Cells)
                //{
                //    if (cell.IsOverrided == false)
                //    {
                //        if (str.Length > 0)
                //        {
                //            str.Append(' ');
                //        }
                //        str.Append(cell.Text);
                //    }
                //}
                //return str.ToString();
            }
            set
            {
                // 操作无效
            }
        }

        internal override bool GetContainsUnHandledPageBreak()
        {
            if ( this.HandledNewPage == false && this.NewPage)
            {
                return true;
            }
            return base.GetContainsUnHandledPageBreak();//base.GetContainsUnHandledPageBreak();

        }

        /// <summary>
        /// 强制分页
        /// </summary>
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public bool NewPage
        {
            get
            {
                return this.StateFlag16;// _NewPage;
            }
            set
            {
                this.StateFlag16 = value;// _NewPage = value;
            }
        }

        /// <summary>
        /// 已经处理的强制分页标记.DCWriter内部使用。
        /// </summary>
        internal bool HandledNewPage
        {
            get { return this.StateFlag19; }
            set { this.StateFlag19 = value; }
        }

        internal float _MaxCellHeight = -1;
        /// <summary>
        /// 表格行中最高的单元格的高度
        /// </summary>
        internal float MaxCellHeight()
        {

            if (_MaxCellHeight < 0)
            {
                _MaxCellHeight = 0;
                foreach (DomTableCellElement cell in this.Cells)
                {
                    if (cell.IsOverrided == false && _MaxCellHeight < cell.Height)
                    {
                        _MaxCellHeight = cell.Height;
                    }
                }
            }
            return _MaxCellHeight;

        }
        internal void ResetMaxCellHeight()
        {
            this._MaxCellHeight = -1;
        }

        /// <summary>
        /// 文档元素编号前缀
        /// </summary>
        public override string ElementIDPrefix()
        {

            return "row";

        }

        /// <summary>
        /// 能否被分页线分割，也就是是否允许被分配到两页上
        /// </summary>
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public bool CanSplitByPageLine
        {
            get
            {
                return this.StateFlag12;// _CanSplitByPageLine;
            }
            set
            {
                this.StateFlag12 = value;// _CanSplitByPageLine = value;
            }
        }


        /// <summary>
        /// 从0开始计算的表格行所在的页码数
        /// </summary>
        public override int OwnerPageIndex
        {
            get
            {
                if (this.OwnerDocument == null)
                {
                    return -1;
                }
                float top = this.GetAbsTop() + 0.01f;
                int result = this.OwnerDocument.Pages.IndexOfByPosition(top, 0);
                return result;
            }
        }


        /// <summary>
        /// 对象所属的表格对象
        /// </summary>
        public override DomTableElement OwnerTable
        {
            get
            {
                return (DomTableElement)this.Parent;
            }
        }

        /// <summary>
        /// 行号
        /// </summary>
        public int Index
        {
            get
            {
                if (this.OwnerTable == null)
                {
                    return 0;
                }
                else
                {
                    return this.OwnerTable.RuntimeRows.IndexOf(this);
                }
            }
        }

        public override PointF AbsPosition
        {
            get
            {
                var table = this.Parent as DomTableElement;
                if (table == null)
                {
                    return new PointF(this.Left, this.Top);
                }
                else
                {
                    var p = table.AbsPosition;
                    return new PointF(p.X + this.Left, p.Y + this.Top);
                }
            }
        }

        /// <summary>
        /// 左端绝对坐标
        /// </summary>
        public override float AbsLeft
        {
            get
            {
                if (this.OwnerTable == null)
                {
                    return this.Left;
                }
                else
                {
                    return this.OwnerTable.AbsLeft + this.Left;
                }
            }
        }

        /// <summary>
        /// 顶端绝对坐标值
        /// </summary>
        public override float AbsTop
        {
            get
            {
                if (this.OwnerTable == null)
                {
                    return this.Top;
                }
                else
                {
                    return this.OwnerTable.AbsTop + this.Top;
                }
            }
        }
        /// <summary>
        /// 本表格行包含的单元格对象
        /// </summary>
        public DomElementList Cells
        {
            get
            {
                return this.Elements;
            }
            set
            {
                this.Elements = value;
            }
        }

        public int CellsCount
        {
            get
            {
                if (this.Elements == null)
                {
                    return 0;
                }
                else
                {
                    return this.Elements.Count;
                }
            }
        }
        /// <summary>
        /// 表格行中最后一个可见的单元格（未被其他单元格合并覆盖的单元格）
        /// </summary>
        public DomTableCellElement LastVisibleCell
        {
            get
            {
                for (int iCount = this.Cells.Count - 1; iCount >= 0; iCount--)
                {
                    DomTableCellElement cell = (DomTableCellElement)this.Cells[iCount];
                    if (cell.IsOverrided == false)
                    {
                        return cell;
                    }
                }
                return null;
            }
        }


        private float _SpecifyHeight;
        /// <summary>
        /// 用户指定的高度
        /// </summary>
        /// <remarks>
        /// 若等于0则表格行自动设置高度，
        /// 若大于0则表格行高度自动设置高度而且高度不小于用户指定的高度，
        /// 若小于0则固定设置表格行的高度为用户指定的高度。
        /// </remarks>
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public float SpecifyHeight
        {
            get
            {
                return _SpecifyHeight;
            }
            set
            {
                _SpecifyHeight = value;
            }
        }


        /// <summary>
        /// 表格行的刷新内容和排版
        /// </summary>
        public override void EditorRefreshView()
        {
            DomTableElement table = this.OwnerTable;
            table.EditorRefreshView();
            //base.EditorRefreshView();
        }



        /// <summary>
        /// 标题行样式
        /// </summary>
        /// <remarks>
        /// 在分页时，若导致分页的表格行的DataRow属性为false则不自动插入临时标题行
        /// </remarks>
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public bool HeaderStyle
        {
            get
            {
                return this.StateFlag15;// _HeaderStyle;
            }
            set
            {
                this.StateFlag15 = value;// _HeaderStyle = value;
            }
        }


        /// <summary>
        /// 获得同一层次中的上一个元素
        /// </summary>
        public override DomElement PreviousElement
        {
            get
            {
                DomTableElement table = (DomTableElement)this.Parent;
                if (table != null)
                {
                    return table.RuntimeRows.GetPreElement(this);
                }
                else
                {
                    return null;
                }
            }
        }
        /// <summary>
        /// 获得元素的同一层次的下一个元素
        /// </summary>
        public override DomElement NextElement
        {
            get
            {
                DomTableElement table = (DomTableElement)this.Parent;
                if (table != null)
                {
                    var rs = table.RuntimeRows;
                    var rowIndex = this.RowIndex;
                    if (rowIndex >= 0 && rowIndex < rs.Count && rs[rowIndex] == this)
                    {
                        if (rowIndex + 1 == rs.Count)
                        {
                            return null;
                        }
                        else
                        {
                            return rs[rowIndex + 1];
                        }
                    }
                    return rs.GetNextElement(this);
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// 元素编号
        /// </summary>
        public override int ElementIndex
        {
            get
            {
                if (this.Parent != null)
                {
                    DomTableElement table = (DomTableElement)this.Parent;
                    var rs = table.RuntimeRows;
                    if( this._ElementIndex >= 0 && this._ElementIndex < rs.Count && rs[this._ElementIndex] == this )
                    {
                        return this._ElementIndex;
                    }
                    else
                    {
                        return rs.IndexOf(this);
                    }
                }
                return -1;
                //return base.ElementIndex;
            }
        }



        /// <summary>
        /// 不支持设置表格行的可见性
        /// </summary>
        /// <param name="visible"></param>
        /// <param name="logUndo"></param>
        /// <param name="fastMode"></param>
        /// <returns></returns>
        public override bool EditorSetVisibleExt(bool visible, bool logUndo, bool fastMode)
        {
            return false;
        }

        /// <summary>
        /// 创建新的文档对象，使其包含本文档元素的复制品
        /// </summary>
        /// <param name="includeThis">是否包含本文档原始对象,对表格行该参数无作用</param>
        /// <returns>创建的文档对象</returns>
        public override DomDocument CreateContentDocument(bool includeThis)
        {
            if (this.OwnerTable == null)
            {
                return null;
            }
            DomElementList sourceCells = new DomElementList();
            foreach (DomTableCellElement cell in this.Cells)
            {
                if (cell.IsOverrided == false)
                {
                    sourceCells.Add(cell);
                }
            }
            if (sourceCells.Count == 0)
            {
                // 该表格行单元格全部被覆盖了，无法创建内容文档对象
                return null;
            }
            DomTableElement tempTable = (DomTableElement)this.OwnerTable.Clone(false);
            tempTable.Columns = new DomElementList();
            DomTableRowElement tempRow = (DomTableRowElement)this.Clone(false);
            tempTable.Rows = new DomElementList();
            tempTable.Rows.Add(tempRow);
            foreach (DomTableCellElement cell in sourceCells)
            {
                DomTableColumnElement tempCol = (DomTableColumnElement)cell.OwnerColumn.Clone(false);
                tempCol.Width = cell.Width;
                tempTable.Columns.Add(tempCol);

                DomTableCellElement tempCell = (DomTableCellElement)cell.Clone(true);
                tempCell.InternalSetRowSpan(1);
                tempCell.InternalSetColSpan(1);
                tempRow.Cells.Add(tempCell);
            }
            tempTable.OwnerDocument = this.OwnerDocument;
            DomDocument document = tempTable.CreateContentDocument(true);
            tempTable.Dispose();
            return document;
        }


        /// <summary>
        /// 选中整个表格行
        /// </summary>
        /// <returns></returns>
        public override bool Select()
        {
            DomTableElement table = this.OwnerTable;
            if (table == null)
            {
                return false;
            }
            if(this.Cells.Count == 0 )
            {
                return false;
            }
            //if (this.TemporaryHeaderRow)
            //{
            //    // 由于标题行而生成的临时表格行内容不可选择
            //    return false;
            //}
            DomElementList cells = table.GetSelectionCells(
                            table.RuntimeRows.IndexOf(this),
                            0,
                            table.RuntimeRows.IndexOf(this),
                            this.Cells.Count - 1);
            int firstIndex = int.MaxValue;
            int lastIndex = 0;
            foreach (DomTableCellElement cell in cells)
            {
                if (cell.IsOverrided == false)
                {
                    firstIndex = Math.Min(firstIndex, cell.Elements.FirstContentElement.ContentIndex);
                    lastIndex = Math.Max(lastIndex, cell.Elements.LastElement.ContentIndex);
                }

            }
            if (firstIndex < int.MaxValue && firstIndex >= 0 && lastIndex >= 0)
            {
                return this.DocumentContentElement.SetSelectionRange(firstIndex, lastIndex);
            }
            else
            {
                return false;
            }
        }


        /// <summary>
        /// 在编辑器中设置表格行的用户指定高度,DCWriter内部使用。
        /// </summary>
        /// <param name="newHeight">新高度</param>
        /// <param name="logUndo">是否记录撤销操作信息</param>
        public void EditorSetRowSpecifyHeight(float newHeight, bool logUndo)
        {
            DomTableElement table = this.OwnerTable;
            table.InvalidateView();
            float tabelHeight = table.Height;
            float oldHeight = this.SpecifyHeight;
            if (this.SpecifyHeight < 0)
            {
                newHeight = -newHeight;
            }
            if (this.SpecifyHeight != newHeight)
            {
                DomDocument document = this.OwnerDocument;

                float oldRowHeight = this.Height;
                this.SpecifyHeight = newHeight;

                if (logUndo && document.BeginLogUndo())
                {
                    document.UndoList.AddRowSpecifyHeight(this, oldHeight);
                    document.EndLogUndo();
                }

                document.Modified = true;
                this.Modified = true;
                this.Height = 0;
                var thisCells = this.Cells;
                for (var iCount = thisCells.Count - 1; iCount >= 0; iCount--)
                {
                    var cell = (DomTableCellElement)thisCells[iCount];
                    if (cell.IsOverrided)
                    {
                        cell = cell.OverrideCell;
                    }
                    cell.LayoutInvalidate = true;
                    cell.Width = 0;
                    cell.Height = 0;
                }
                table.ExecuteLayout(false);
                for (var iCount = thisCells.Count - 1; iCount >= 0; iCount--)
                {
                    var cell = (DomTableCellElement)thisCells[iCount];
                    if (cell.IsOverrided)
                    {
                        cell = cell.OverrideCell;
                    }
                    cell.FixLinePositionForPageLine(-1);
                }
                table.InvalidateView();
                if (table.Height != tabelHeight)
                {
                    // 表格高度发生改变，需要重新设置行状态和重新分页
                    table.UpdatePagesForTable(false);
                }
            }
        }

        /// <summary>
        /// 是否包含被选中的单元格
        /// </summary>
        public override bool HasSelection
        {
            get
            {

                if (this.OwnerDocument.Selection.CellsWithoutOverried != null)
                {
                    foreach (DomTableCellElement cell in this.OwnerDocument.Selection.CellsWithoutOverried)
                    {
                        if (this.Cells.Contains(cell))
                        {
                            return true;
                        }
                    }
                }
                return base.HasSelection;
            }
        }

        /// <summary>
        /// 删除表格行
        /// </summary>
        public override bool EditorDelete(bool logUndo)
        {
            DomTableElement table = this.OwnerTable;
            if (table != null)
            {
                return table.EditorDeleteRows2(
                    table.Rows.IndexOf(this),
                    1,
                    logUndo,
                    null);
            }
            return false;
        }

        /// <summary>
        /// 计算表格行的高度，DCWriter内部使用
        /// </summary>
        public void CalcuteRowHeight()
        {
            var document = this._OwnerDocument;
            DCGridLineInfo info = document._GlobalGridInfo;
            float rtsh = this.SpecifyHeight;
            if (info != null && info.AlignToGridLine)
            {
                // 当存在全局文档网格线时，表格行固定高度功能失效。
                rtsh = 0;
            }
            if (rtsh < 0)
            {
                this.Height = Math.Abs(rtsh);
            }
            else
            {
                bool match = false;
                RuntimeDocumentContentStyle rs = this.RuntimeStyle;
                float rowPaddingTop = rs.PaddingTop;
                float rowPaddingBottom = rs.PaddingBottom;
                float newRowHeight = rtsh;
                var cellArr = this.Cells.InnerGetArrayRaw();
                var cellCount = this.Cells.Count;
                for( var iCount = 0;iCount < cellCount;iCount++) 
                {
                    var cell = (DomTableCellElement)cellArr[iCount];
                    if (cell.IsOverrided == false && cell.RowSpan == 1)
                    {
                        float h = cell.ContentHeight + rowPaddingTop + rowPaddingBottom;
                        if (newRowHeight < h)
                        {
                            newRowHeight = h;
                        }
                        match = true;
                    }
                }
                if (match == false)
                {
                    // 设置表格行的高度为默认文本行的高度
                    float dh = document.DefaultStyle.DefaultLineHeight
                        + rowPaddingTop
                        + rowPaddingBottom;
                    newRowHeight = Math.Max(rtsh, dh);
                }
                if (info != null && info.AlignToGridLine)
                {
                    newRowHeight = info.FixLineHeight(newRowHeight);
                }
                if (this.Height != newRowHeight)
                {
                    this.Height = newRowHeight;
                }
            }
        }

        /// <summary>
        /// 为编辑器复制表格行对象
        /// </summary>
        /// <returns>复制品</returns>
        public DomTableRowElement EditorClone()
        {
            DomTableRowElement row = (DomTableRowElement)this.Clone(false);
            row.InnerSetOwnerDocumentParentRaw(this._OwnerDocument, this);
            row.HeaderStyle = false;
            row.Elements = new DomElementList();
            foreach (DomTableCellElement cell in this.Cells)
            {
                DomTableCellElement newCell = (DomTableCellElement)cell.Clone(false);
                newCell.InnerSetOwnerDocumentParentRaw(this._OwnerDocument, row);
                if( cell.IsOverrided )
                {
                    // DUWRITER5_0-1384。若原始单元格被合并了，则使用合并单元格的样式。
                    var c2 = cell.OverrideCell;
                    newCell.StyleIndex = c2.StyleIndex;
                    if(newCell.ElementsCount == 1 && newCell.FirstChild is DomParagraphFlagElement)
                    {
                        var le = c2.LastChild as DomParagraphFlagElement;
                        if( le != null )
                        {
                            newCell.FirstChild.StyleIndex = le.StyleIndex;
                        }
                    }
                }
                row.Cells.Add(newCell);
            }//foreach
            return row;
        }

    }
}