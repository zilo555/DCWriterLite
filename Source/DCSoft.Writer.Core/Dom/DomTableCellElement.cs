using System.Collections.Generic;
using DCSoft.Common;
using DCSoft.Drawing;
using System.ComponentModel;
using DCSoft.Printing;


namespace DCSoft.Writer.Dom
{
    /// <summary>
    /// 单元格元素
    /// </summary>
    /// <remarks>编写 袁永福</remarks>
    public sealed partial class DomTableCellElement : DomContentElement
    {
        public static readonly string TypeName_XTextTableCellElement = "XTextTableCellElement";
        public override string TypeName => TypeName_XTextTableCellElement;

        /// <summary>
        /// 初始化对象
        /// </summary>
        public DomTableCellElement()
        {
            this.TabStop = true;
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
        /// 在选择区域中的单元格列表中的编号，用于改善大量单元格被选中的性能。
        /// </summary>
        internal int IndexInSelectionCells = -1;


        /// <summary>
        /// 获取或设置一个值，该值指示用户能否使用 Tab 键将焦点放到该元素中上。 
        /// </summary>
        public bool TabStop
        {
            get
            {
                return this.StateFlag22;// _TabStop;
            }
            set
            {
                this.StateFlag22 = value;// _TabStop = value;
            }
        }


        /// <summary>
        /// 获取或设置一个运行时的值，该值指示用户能否使用 Tab 键将焦点放到该元素中上。 
        /// </summary>
        public override bool RuntimeTabStop()
        {
            return this.TabStop;
        }

        public override bool Visible
        {
            get
            {
                return true;
            }
            set
            {
                base.Visible = value;
            }
        }

        /// <summary>
        /// 单元格可否可见
        /// </summary>
        public override bool RuntimeVisible
        {
            get
            {
                return this._OverrideCell == null && this._RuntimeVisible;
            }
            set
            {
                base._RuntimeVisible = value;
            }
        }

        /// <summary>
        /// 获得运行时可见性的值
        /// </summary>
        /// <returns>可见性</returns>
        public override bool GetRuntimeVisibleValue(UpdateElementsRuntimeVisibleArgs args)
        {
            if (this._OverrideCell != null)
            {
                return false;
            }
            if (this._Parent != null)
            {
                return this._Parent.RuntimeVisible;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// 文档元素编号前缀
        /// </summary>
        public override string ElementIDPrefix()
        {

            return "cell";

        }

        /// <summary>
        /// 元素在父节点的子节点列表中的序号
        /// </summary>
        public override int ElementIndex
        {
            get
            {
                if (this._Parent == null)
                {
                    return -1;
                }
                else
                {
                    var es = this._Parent.Elements;
                    if (this._ElementIndex >= 0 && this._ElementIndex < es.Count && es[this._ElementIndex] == this)
                    {
                        return this._ElementIndex;
                    }
                    return es.IndexOf(this);
                }
            }
            set
            {
                base.ElementIndex = value;
            }
        }
        /// <summary>
        /// 对象所属页码
        /// </summary>
        public override int OwnerPageIndex
        {
            get
            {
                if (this.OwnerRow == null)
                {
                    return -1;
                }
                else
                {
                    return this.OwnerRow.OwnerPageIndex;
                }
            }
        }


        /// <summary>
        /// 不支持该方法
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
        /// 实际使用的移动焦点所使用的快捷键样式.DCWriter内部使用。
        /// </summary>
        public override MoveFocusHotKeys RuntimeMoveFocusHotKey
        {
            get
            {
                return MoveFocusHotKeys.Tab;
            }
        }

        public override string FormulaValue
        {
            get
            {
                var field = this._Elements.GetFirstElement<DomInputFieldElement>();
                if (field == null)
                {
                    return this.Text;
                }
                else
                {
                    return field.FormulaValue;
                }
            }
            set
            {
                var field = this.Elements.GetFirstElement<DomInputFieldElement>();
                var args = new SetContainerTextArgs();
                args.NewText = this.FixInputFormulaValue(value);
                args.LogUndo = false;
                args.AccessFlags = DomAccessFlags.None;
                args.EventSource = ContentChangedEventSource.Default;
                args.FocusContainer = false;
                args.IgnoreDisplayFormat = false;
                args.ShowUI = false;
                args.UpdateContent = true;
                if (field == null)
                {
                    this.SetEditorText(args);
                }
                else
                {
                    field.SetEditorText(args);
                }
            }
        }


        /// <summary>
        /// 对象所属表格对象
        /// </summary>
        public override DomTableElement OwnerTable
        {
            get
            {
                if (this._Parent == null)
                {
                    return null;
                }
                else
                {
                    return (DomTableElement)(this._Parent.Parent);
                }
            }
        }
        /// <summary>
        /// 对象所属表格行对象
        /// </summary>
        public DomTableRowElement OwnerRow
        {
            get
            {
                return (DomTableRowElement)this._Parent;
            }
        }

        public override PointF AbsPosition
        {
            get
            {
                var row = this._Parent as DomTableRowElement;
                if (row == null)
                {
                    return new PointF(this.Left, this.Top);
                }
                else
                {
                    var p = row.AbsPosition;
                    return new PointF(p.X + this.Left, p.Y + this.Top);
                }
            }
        }

        /// <summary>
        /// 左端绝对坐标值
        /// </summary>
        public override float AbsLeft
        {
            get
            {
                if (this.OwnerRow == null)
                {
                    return this.Left;
                }
                else
                {
                    return this.OwnerRow.GetAbsLeft() + this.Left;
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
                DomTableRowElement row = this.Parent as DomTableRowElement;
                if (row != null)
                {
                    DomTableElement table = row.Parent as DomTableElement;
                    if (table != null)
                    {
                        return table.GetAbsTop() + row.Top;
                    }
                }
                return 0;

                //XTextTableElement table = this.OwnerTable;
                //if (table == null)
                //{
                //    return 0;
                //}
                //else
                //{
                //    return table.AbsTop + this.Parent.Top;
                //}
                //return this.OwnerRow.AbsTop + this.Top;
            }
        }

        /// <summary>
        /// 对象所属的最下面的表格行对象
        /// </summary>
        /// <remarks>当对象没有合并单元格时，该属性就返回单元格所在表格行对象，
        /// 当纵向合并了单元格时( RowSpan 属性大于1)则该属性返回该单元格所跨过的
        /// 表格行中最下面的一个表格行对象</remarks>
        public DomTableRowElement LastOwnerRow
        {
            get
            {
                DomTableElement table = this.OwnerTable;
                if (table == null)
                {
                    return null;
                }
                else
                {
                    return (DomTableRowElement)table.RuntimeRows.SafeGet(this.Parent.ElementIndex + this.RowSpan - 1);
                }
                //return (XTextTableRowElement)this.OwnerTable.Elements.SafeGet(this.Parent.ElementIndex + this.intRowSpan - 1);
            }
        }

        private DomTableColumnElement _OwnerColumn;
        /// <summary>
        /// 单元格所属表格列对象
        /// </summary>
        public DomTableColumnElement OwnerColumn
        {
            get
            {
                if (this._OwnerColumn == null)
                {
                    DomTableElement table = this.OwnerTable;
                    if (table != null)
                    {
                        this._OwnerColumn = (DomTableColumnElement)table.Columns.SafeGet(this.ElementIndex);
                    }
                }
                return this._OwnerColumn;
            }
            set
            {
                _OwnerColumn = value;
            }
        }

        /// <summary>
        /// 对象所属的最右边的表格列对象
        /// </summary>
        public DomTableColumnElement LastOwnerColumn
        {
            get
            {
                DomTableElement table = this.OwnerTable;
                if (table == null)
                {
                    return null;
                }
                else
                {
                    return (DomTableColumnElement)table.Columns.SafeGet(
                        this.ElementIndex + this._ColSpan - 1);
                }
            }
        }
        internal DomTableCellElement _OverrideCell;
        /// <summary>
        /// 若单元格被其他单元格合并了则返回合并本单元格的单元格对象
        /// </summary>
        public DomTableCellElement OverrideCell
        {
            get
            {
                return _OverrideCell;
            }
        }

        /// <summary>
        /// 设置单元格被合并覆盖后的父单元格对象
        /// </summary>
        /// <param name="cell">父单元格对象</param>
        internal void SetOverrideCell(DomTableCellElement cell)
        {
            //if (_OverrideCell != cell)
            //{
            _OverrideCell = cell;
            //}
        }

        /// <summary>
        /// 单元格是否处于选择状态
        /// </summary>
        public bool IsSelected
        {
            get
            {
                DomDocumentContentElement dce = this.DocumentContentElement;
                if (dce != null)
                {
                    return dce.Selection.ContainsCell(this);// .Cells.Contains(this);
                }
                return false;
            }
        }

        /// <summary>
        /// 单元格在表格中的编号,这个编号是只读的，比如“A1”、“B2”、“C3”等。
        /// </summary>
        public string CellID
        {
            get
            {
                if (this._Parent?.Parent is DomTableElement)
                {
                    return CellIndex.GetCellIndex(this._RowIndex + 1, this._ElementIndex + 1);
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        /// <summary>
        /// 内容垂直对齐方式
        /// </summary>
        public override VerticalAlignStyle ContentVertialAlign
        {
            get
            {
                return this.RuntimeStyle.VerticalAlign;
            }
        }


        /// <summary>
        /// 判断本单元格是否被其他单元格合并了
        /// </summary>
        public bool IsOverrided
        {
            get
            {
                return this._OverrideCell != null;
            }
        }

        /// <summary>
        /// 跨行数
        /// </summary>
        private int _RowSpan = 1;
        /// <summary>
        /// 跨行数
        /// </summary>
        /// <remarks>
        /// 单元格所占据的表格行数，本属性为1则占据一行，单元格纵向没有合并单元格，若该属性值大于1则纵向合并
        /// 单元格。本属性类似于 HTML 的 TD 元素的 ROWSPAN 属性。
        /// </remarks>
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public int RowSpan
        {
            get
            {
                return this._RowSpan;
            }
            set
            {
                if (this.OwnerTable == null
                    || this.OwnerDocument == null
                    || this.OwnerDocument.ReadyState != DomReadyStates.Complete)
                {
                    this._RowSpan = value;
                    return;
                }
                int iValue = FixRowSpan(value);
                if (_RowSpan != iValue)
                {
                    _RowSpan = iValue;
                    this.OwnerTable.InnerExecuteLayout();////////////
                }

            }
        }
        /// <summary>
        /// 修正单元格跨行数
        /// </summary>
        /// <param name="rowSpan">单元格跨行数</param>
        /// <returns>修正后的值</returns>
        public int FixRowSpan(int rowSpan)
        {
            if (rowSpan < 1)
            {
                rowSpan = 1;
            }
            if (this.RowIndex + rowSpan - 1 >= this.OwnerTable.Rows.Count)
            {
                rowSpan = this.OwnerTable.Rows.Count - this.RowIndex;
            }
            return rowSpan;
        }


        /// <summary>
        /// 跨列数
        /// </summary>
        private int _ColSpan = 1;
        /// <summary>
        /// 跨列数
        /// </summary>
        /// <remarks>
        /// 单元格所占据的表格列数，本属性值为1则单元格占据一列，单元格横向没有合并单元格，若该属性值大于1则横向合并
        /// 单元格。本属性类似 HTML 的 TD 元素的 COLSPAN 属性。
        /// </remarks>
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public int ColSpan
        {
            get
            {
                return this._ColSpan;
            }
            set
            {
                if (this.OwnerDocument == null || this.OwnerDocument.ReadyState != DomReadyStates.Complete)
                {
                    this._ColSpan = value;
                    return;
                }
                int iValue = FixColSpan(value);
                if (this._ColSpan != iValue)
                {
                    this._ColSpan = iValue;
                    this.OwnerTable.InnerExecuteLayout();//////////////////
                }
            }
        }

        /// <summary>
        /// 修正单元格跨列数
        /// </summary>
        /// <param name="colSpan">单元格跨列数</param>
        /// <returns>修正后的值</returns>
        public int FixColSpan(int colSpan)
        {
            if (this.ColIndex + colSpan - 1 >= this.OwnerTable.Columns.Count)
            {
                colSpan = this.OwnerTable.Columns.Count - this.ColIndex;
            }
            if (colSpan < 1)
            {
                colSpan = 1;
            }
            return colSpan;
        }

        /// <summary>
        /// 内部调用
        /// </summary>
        /// <param name="colSpan"></param>
        public void InternalSetColSpan(int colSpan)
        {
            this._ColSpan = Math.Max(colSpan, 1);
        }

        /// <summary>
        /// 内部调用
        /// </summary>
        /// <param name="rowSpan"></param>
        public void InternalSetRowSpan(int rowSpan)
        {
            this._RowSpan = Math.Max(rowSpan, 1);
        }

        /// <summary>
        /// 运行时的行号
        /// </summary>
        internal int _RowIndex = -1;
        /// <summary>
        /// 从0开始的行号
        /// </summary>
        public int RowIndex
        {
            get
            {
                //if (_RowIndex < 0 && this.Parent != null )
                //{
                //    return this.Parent.ElementIndex;
                //}
                return this._RowIndex;
            }
        }
        /// <summary>
        /// 从0开始的列号
        /// </summary>
        public int ColIndex
        {
            get
            {
                //if (_ColIndex < 0)
                //{
                //    return this.ElementIndex;
                //}
                return this._ElementIndex;
            }
        }


        /// <summary>
        /// 无操作
        /// </summary>
        public override void UpdateHeightByContentHeight()
        {
            // 无操作
            //base.UpdateHeightByContentHeight();
        }

        /// <summary>
        /// 创建新的文档对象，使其包含本文档元素的复制品
        /// </summary>
        /// <param name="includeThis">是否包含本文档原始对象</param>
        /// <returns>创建的文档对象</returns>
        public override DomDocument CreateContentDocument(bool includeThis)
        {
            if (this.OwnerTable == null)
            {
                return null;
            }
            if (includeThis)
            {
                DomTableElement tempTable = (DomTableElement)this.OwnerTable.Clone(false);
                tempTable.OwnerDocument = this.OwnerDocument;
                tempTable.Columns = new DomElementList();
                DomTableColumnElement col = new DomTableColumnElement();
                col.Width = this.Width;
                tempTable.Columns.Add(col);

                tempTable.Elements = new DomElementList();
                DomTableRowElement tempRow = (DomTableRowElement)this.OwnerRow.Clone(false);
                tempTable.Rows.Add(tempRow);

                DomTableCellElement tempCell = (DomTableCellElement)this.Clone(true);
                tempCell._RowSpan = 1;
                tempCell._ColSpan = 1;
                tempRow.Cells.Add(tempCell);
                tempTable.FixDomState();
                DomDocument document = WriterUtilsInner.CreateDocument(
                    this.OwnerDocument,
                    new DomElementList(tempTable),
                    false);
                tempTable.Dispose();
                return document;
            }
            else
            {
                return base.CreateContentDocument(false);
            }
        }

        public override DCGridLineInfo InnerRuntimeGridLine()
        {
            DCGridLineInfo info = base.InnerRuntimeGridLine();
            if (info != null)
            {
                return info;
            }
            return null;
        }

        /// <summary>
        /// 绘制单元格内容
        /// </summary>
        /// <param name="args">参数</param>
        public override void DrawContent(InnerDocumentPaintEventArgs args)
        {
            InnerDrawContent(args, false);
        }
        internal void InnerDrawContent(InnerDocumentPaintEventArgs args, bool drawContentOnly)
        {
            if (this._OwnerDocument == null)
            {
                return;
            }
            var vopts = args.ViewOptions;// this.GetDocumentViewOptions();
            var rs = this.RuntimeStyle;
            base.DrawContent(args);
        }
        /// <summary>
        /// 执行排版
        /// </summary>
        public override void InnerExecuteLayout()
        {
            ExecuteLayoutInner(false, null);
        }
        internal void ExecuteLayoutInner(bool layoutContentOnly, ParticalRefreshLinesEventArgs args)
        {

            //this.Width = this.OwnerDocument.PageSettings.ViewClientWidth;
            //this.UpdateContentElements();
            if (this._PrivateLines == null
                && this._PrivateContent == null
                && this._OwnerDocument.IsLoadingDocument)
            {
                // 加载文档时第一次进行内容排版
                this._PrivateLines = new DomLineList();
            }
            else
            {
                this.PrivateLines.Clear();
                if (this.PrivateContent.Count == 1)
                {
                    this._PrivateContent[0]._OwnerLine = null;
                }
                else
                {
                    this._PrivateContent.ClearOwnerLine();
                    //for (int iCount = this.PrivateContent.Count - 1; iCount >= 0; iCount--)
                    //{
                    //    this._PrivateContent[iCount]._OwnerLine = null;
                    //}
                }
            }
            this.DocumentContentElement.RefreshGlobalLines();
            //foreach (XTextElement element in this.PrivateContent)
            //{
            //    element.SetOwnerLine(null);
            //}
            //if (this.UIIsUpdating)
            //{
            //    return;
            //}
            if (args == null)
            {
                args = new ParticalRefreshLinesEventArgs(
                    null,
                    null,
                    this.RuntimeStyle.VerticalAlign);
                args.LayoutContentOnly = layoutContentOnly;
            }
            if (this.ParticalRefreshLines(args))
            {
                if (this.Height > 0)
                {
                    this.UpdateLinePosition(this.ContentVertialAlign, true, true);
                }
            }
            this.SizeInvalid = false;
        }

        /// <summary>
        /// 选择单元格
        /// </summary>
        public override bool Select()
        {
            DomTableElement table = this.OwnerTable;
            if (table == null)
            {
                return false;
            }
            DomTableCellElement cell = this;
            if (cell.IsOverrided)
            {
                cell = cell.OverrideCell;
            }
            int firstIndex = cell.FirstContentElement.ContentIndex;
            int lastIndex = cell.LastContentElement.ContentIndex;
            cell.DocumentContentElement.Focus();
            this.OwnerDocument.ClearPagesClientSelectionBounds();
            return cell.DocumentContentElement.SetSelectionRange(firstIndex, lastIndex);
        }
        /// <summary>
        /// 处理内容改变事件
        /// </summary>
        /// <param name="args">参数</param>
        public override void OnContentChanged(ContentChangedEventArgs args)
        {
            if (this.OwnerColumn != null
                && args.LoadingDocument == false
                && this.OwnerDocument.EnableContentChangedEvent())
            {
                this.OwnerColumn.Modified = true;
            }
            base.OnContentChanged(args);
            //if (args.LoadingDocument == false)
            //{
            //    // 加载文档时不执行表达式
            //    this.OwnerDocument.ExpressionExecuter.ExecuteEffectValueExpression(this);
            //}
        }

        /// <summary>
        /// 单独的设置单元格的宽度
        /// </summary>
        /// <param name="newWidth">新宽度</param>
        /// <param name="logUndo">是否记录撤销操作信息</param>
        /// <returns>操作是否成功</returns>
        public bool EditorSetCellWidthSingle(float newWidth, bool logUndo)
        {
            float minTableColumnWidth = GetDocumentViewOptions().MinTableColumnWidth;
            if (Math.Abs(newWidth - this.Width) < minTableColumnWidth / 5)
            {
                // 新旧宽度差别不大，不予处理
                return false;
            }
            DomTableElement table = this.OwnerTable;
            //int currentColIndex = this.ColIndex;
            if (this.ColIndex + this.ColSpan >= table.Columns.Count)
            {
                // 当前表格列是表格的最后一列
                return false;
            }
            DomTableRowElement currentRow = this.OwnerRow;
            if (newWidth > this.Width)
            {
                DomTableCellElement nextCell = (DomTableCellElement)currentRow.Cells[this.ColIndex + this.ColSpan];
                if (nextCell.OverrideCell != null)
                {
                    nextCell = nextCell.OverrideCell;
                }
                newWidth = Math.Min(newWidth, nextCell.Width + this.Width - minTableColumnWidth);
            }
            if (newWidth < minTableColumnWidth)
            {
                newWidth = minTableColumnWidth;
            }
            // 获得同行中下一个单元格
            //XTextElementList nextCells = new XTextElementList();
            int endColIndex = -1;
            for (int ri = 0; ri < this.RowSpan; ri++)
            {
                DomTableCellElement nextCell = table.GetCell(
                    ri + this.RowIndex,
                    this.ColIndex + this.ColSpan,
                    true);
                if (nextCell.IsOverrided)
                {
                    nextCell = nextCell.OverrideCell;
                }
                if (nextCell.RowIndex < this.RowIndex)
                {
                    // 单元格行号小于当前行号，不予处理
                    return false;
                }
                if (nextCell.RowIndex + nextCell.RowSpan > this.RowIndex + this.RowSpan)
                {
                    // 单元格的下端行号大于当前单元格的下端行号，不予处理
                    return false;
                }
                if (newWidth < this.Width)
                {
                    if (endColIndex == -1)
                    {
                        endColIndex = nextCell.ColIndex + nextCell.ColSpan;
                    }
                    else
                    {
                        endColIndex = Math.Min(
                            endColIndex,
                            nextCell.ColIndex + nextCell.ColSpan);
                    }
                }
                else
                {
                    if (endColIndex == -1)
                    {
                        endColIndex = nextCell.ColIndex + nextCell.ColSpan;
                    }
                    else
                    {
                        endColIndex = Math.Min(
                            endColIndex,
                            nextCell.ColIndex + nextCell.ColSpan);
                    }
                }
            }
            if (endColIndex >= table.Columns.Count)
            {
                endColIndex = table.Columns.Count - 1;
                //return false;
            }
            // 记录表格的当前信息
            DCSoft.Writer.Dom.Undo.XTextUndoTableInfo undo = null;
            if (logUndo)
            {
                undo = new DCSoft.Writer.Dom.Undo.XTextUndoTableInfo();
                undo.OldTableInfo = new TableStructInfo(table, false);
            }
            float pos = 0;
            bool handled = false;
            for (int ci = this.ColIndex; ci <= endColIndex; ci++)
            {
                pos = pos + table.Columns[ci].Width;
                if (Math.Abs(pos - newWidth) < minTableColumnWidth * 0.4f)
                {
                    // 新宽度正好匹配某个表格列，无需创建新的表格列，只需要设置单元格跨列数。
                    if (ci + 1 < table.Columns.Count)
                    {
                        for (int ri = 0; ri < this.RowSpan; ri++)
                        {
                            DomTableRowElement row = (DomTableRowElement)table.Rows[ri + this.RowIndex];
                            // 获得后面的单元格
                            DomTableCellElement nextCell = (DomTableCellElement)row.Cells[this.ColIndex + this.ColSpan];
                            // 获得最后表格列的序号
                            int ec = nextCell.ColIndex + nextCell.ColSpan;
                            // 交换位置
                            row.Cells.MoveElement(row.Cells.IndexOf(nextCell), ci + 1);
                            // 设置新的跨列数
                            nextCell.InternalSetColSpan(ec - ci - 1);
                        }
                        // 设置本单元格的跨列数
                        this.InternalSetColSpan(ci - this.ColIndex + 1);
                        // 处理完毕
                        handled = true;
                    }
                    break;
                }
                else if (pos > newWidth)
                {
                    DomTableColumnElement newCol = table.CreateColumnInstance();
                    newCol.Width = pos - newWidth;
                    DomTableColumnElement oldCol = (DomTableColumnElement)table.Columns[ci];
                    oldCol.Width = oldCol.Width - newCol.Width;
                    using (DCGraphics g = table.InnerCreateDCGraphics())
                    {
                        for (int ri = 0; ri < table.Rows.Count; ri++)
                        {
                            DomTableRowElement row = (DomTableRowElement)table.Rows[ri];
                            // 创建新的单元格对象
                            DomTableCellElement newCell = table.CreateCellInstance();
                            newCell.OwnerDocument = this.OwnerDocument;
                            newCell.StyleIndex = this.StyleIndex;
                            newCell.FixElements();
                            // 计算新的单元格的大小
                            InnerDocumentPaintEventArgs args = table.OwnerDocument.CreateInnerPaintEventArgs(g);
                            args.Element = newCell;
                            newCell.RefreshSize(args);
                            newCell.Width = 0;
                            newCell.Height = 0;
                            newCell.Parent = row;
                            newCell.OwnerDocument = this.OwnerDocument;
                            row.Cells.Insert(ci + 1, newCell);
                            row.FixDomState();
                        }//for
                    }//using
                    table.Columns.Insert(ci + 1, newCol);
                    for (int ri = 0; ri < table.Rows.Count; ri++)
                    {
                        DomTableRowElement row = (DomTableRowElement)table.Rows[ri];
                        if (ri < this.RowIndex || ri >= this.RowIndex + this.RowSpan)
                        {
                            DomTableCellElement cell = (DomTableCellElement)row.Cells[ci];
                            if (cell.IsOverrided)
                            {
                                if (cell.RowIndex == cell.OverrideCell.RowIndex)
                                {
                                    cell = cell.OverrideCell;
                                }
                                else
                                {
                                    cell = null;
                                }
                            }
                            if (cell != null)
                            {
                                cell.InternalSetColSpan(cell.ColSpan + 1);
                            }
                        }
                        else
                        {
                            if (newWidth > this.Width)
                            {
                                // 获得后面的单元格
                                DomTableCellElement nextCell = (DomTableCellElement)row.Cells[this.ColIndex + this.ColSpan];
                                int ce = nextCell.ColIndex + nextCell.ColSpan;
                                row.Cells.MoveElement(row.Cells.IndexOf(nextCell), ci + 1);
                                nextCell.InternalSetColSpan(ce - row.Cells.IndexOf(nextCell) + 1);
                            }
                            else
                            {
                                // 获得后面的单元格
                                DomTableCellElement nextCell = (DomTableCellElement)row.Cells[this.ColIndex + this.ColSpan + 1];
                                // 获得最后表格列的序号
                                int ec = nextCell.ColIndex + nextCell.ColSpan;
                                // 如果新宽度小于旧宽度，则新增的表格列被后面的单元格合并了

                                // 交换位置
                                row.Cells.MoveElement(row.Cells.IndexOf(nextCell), ci + 1);
                                // 设置新的跨列数
                                nextCell.InternalSetColSpan(ec - ci - 1);
                            }
                        }
                    }//for
                    if (newWidth > this.Width)
                    {
                        // 如果新宽度大于旧宽度，则新增的表格列被当前单元格合并了
                        this.InternalSetColSpan(ci - this.ColIndex + 1);
                    }
                    else
                    {
                        this.InternalSetColSpan(ci - this.ColIndex + 1);
                    }
                    handled = true;
                    break;
                }//if
            }//for

            if (handled)
            {
                table.FixDomState();
                //this.intRowSpan = newRowSpan;
                this.UpdateContentVersion();
                // 更新单元格的合并状态
                table.UpdateCellOverrideState();
                if (table.CompressRowsColumns())
                {
                    table.UpdateCellOverrideState();
                }
                this.UpdateContentElements(true);
                // 设置文档的选择状态
                //XTextContent c = this.DocumentContentElement.Content;

                table.InvalidateView();
                using (DCGraphics g = this.InnerCreateDCGraphics())
                {
                    table.RefreshInvalidateCellSize(g);
                }
                table.InnerExecuteLayout();
                table.InvalidateView();
                table.UpdatePagesForTable(false);

                this.DocumentContentElement.SetSelection(
                    this.FirstContentElement.ContentIndex,
                    this.LastContentElement.ContentIndex - this.FirstContentElement.ContentIndex + 1);
                if (undo != null)
                {
                    undo.NewTableInfo = new TableStructInfo(table, false);
                    undo.CompressElements();
                    // 添加撤销操作信息
                    if (this.OwnerDocument.BeginLogUndo())
                    {
                        this.OwnerDocument.AddUndo(undo);
                        this.OwnerDocument.EndLogUndo();
                    }
                }
                this.OwnerDocument.OnSelectionChanged();
                this.OwnerDocument.OnDocumentContentChanged();
            }
            else
            {
            }
            return handled;
        }

        /// <summary>
        /// 编辑器中拆分单元格
        /// </summary>
        /// <param name="newRowsNum">新的横向合并行数,该行数必须是单元格跨行数的约数</param>
        /// <param name="newColsNum">新的纵向合并列数</param>
        /// <param name="logUndo">是否记录撤销操作信息</param>
        /// <returns>操作是否成功</returns>
        public bool EditorSplitCellExt(
            int newRowsNum,
            int newColsNum,
            bool logUndo)
        {
            if (newRowsNum < 1 || newColsNum < 1)
            {
                return false;
            }
            if (newRowsNum == 1 && newColsNum == 1)
            {
                // 不进行拆分，直接返回
                return false;
            }
            bool matchAns = false;
            if (this.RowSpan > 1)
            {
                int[] ans = MathCommon.GetApproximateNumbers(this.RowSpan);
                for (int iCount = 0; iCount < ans.Length; iCount++)
                {
                    if (newRowsNum == ans[iCount])
                    {
                        matchAns = true;
                        break;
                    }
                }
                if (matchAns == false)
                {
                    return false;
                }
            }

            DomTableElement table = this.OwnerTable;
            table.FixDomState();
            DCSoft.Writer.Dom.Undo.XTextUndoTableInfo undo = null;
            if (logUndo)
            {
                // 创建撤销操作记录信息对象
                undo = new DCSoft.Writer.Dom.Undo.XTextUndoTableInfo();
                undo.OldTableInfo = new TableStructInfo(table, true);
            }
            var document = this.OwnerDocument;
            //List<float> heights = new List<float>();
            //List<float> newHeights = new List<float>();
            bool result = false;
            int currentRowIndex = this.RowIndex;
            int currentColIndex = this.ColIndex;
            int currentColSpan = this.ColSpan;
            int newRowSpan = this.RowSpan / newRowsNum;
            int currentRowSpan = this.RowSpan;
            if (currentRowSpan == 1)
            {
                newRowSpan = 1;
                //newRowSpan = newRowsNum;
            }
            float currentWidth = this.Width;

            if (newRowsNum == this.RowSpan
                && newColsNum == this.ColSpan)
            {
                // 拆分行列数等于当前跨行列数，进行简单的拆分
                var cells = table.GetRangeArray(
                    this.RowIndex,
                    this.ColIndex,
                    this.RowSpan,
                    this.ColSpan,
                    true);
                foreach (DomTableCellElement cell in cells)
                {
                    cell.InternalSetColSpan(1);
                    cell.InternalSetRowSpan(1);
                    cell.SetOverrideCell(null);
                    cell.Width = 0;
                    cell.Height = 0;
                    cell.SizeInvalid = true;
                }
                result = true;
            }
            else
            {
                if (newRowsNum > 1)
                {
                    if (currentRowSpan == 1)
                    {
                        // 当前行进行简单拆分
                        DomTableRowElement currentRow = (DomTableRowElement)table.Rows[currentRowIndex];
                        float rh = currentRow.SpecifyHeight;
                        if (rh > 0)
                        {
                            // 设置新的指定行高
                            rh = rh / newRowsNum;
                            currentRow.SpecifyHeight = rh;
                        }
                        for (int ri = 1; ri < newRowsNum; ri++)
                        {
                            DomTableRowElement newRow = table.CreateRowInstance();
                            newRow.SpecifyHeight = rh;
                            newRow.StyleIndex = currentRow.StyleIndex;
                            newRow.HeaderStyle = currentRow.HeaderStyle;
                            newRow.InnerSetOwnerDocumentParentRaw(document, table);

                            //newRow.Parent = table;
                            //newRow.OwnerDocument = this.OwnerDocument;
                            foreach (DomTableCellElement cell in currentRow.Cells)
                            {
                                DomTableCellElement newCell = table.CreateCellInstance(cell);// (XTextTableCellElement)cell.Clone(false);
                                newCell.InnerSetOwnerDocumentParentRaw(document, newRow);
                                //newCell.Parent = newRow;
                                //newCell.OwnerDocument = this.OwnerDocument;
                                newCell.Width = 0;
                                newCell.Height = 0;
                                newCell.SizeInvalid = true;
                                newCell.InternalSetRowSpan(1);
                                newCell.InternalSetColSpan(1);
                                newCell.FixElements();
                                newRow.Cells.Add(newCell);
                            }
                            table.Rows.InsertRaw(currentRowIndex + 1, newRow);
                        }
                        List<DomTableCellElement> hcells = new List<DomTableCellElement>();
                        for (int ci = 0; ci < table.Columns.Count; ci++)
                        {
                            DomTableCellElement cell = (DomTableCellElement)currentRow.Cells[ci];
                            if (cell.OverrideCell != null)
                            {
                                cell = cell.OverrideCell;
                            }
                            if (hcells.Contains(cell))
                            {
                                continue;
                            }
                            hcells.Add(cell);
                            if (ci < currentColIndex || ci >= currentColIndex + currentColSpan)
                            {
                                cell.InternalSetRowSpan(cell.RowSpan + newRowsNum - 1);
                                cell.Width = 0;
                            }
                        }
                        currentRowSpan = newRowSpan;
                    }
                    else
                    {
                        for (int ri = 0; ri < currentRowSpan; ri++)
                        {
                            DomTableRowElement row = (DomTableRowElement)table.Rows[currentRowIndex + ri];

                            for (int ci = 0; ci < this.ColSpan; ci++)
                            {
                                DomTableCellElement cell = (DomTableCellElement)row.Cells[currentColIndex + ci];
                                if ((ri % newRowsNum) == 0)
                                {
                                    // 一组表格行中的第一行
                                    // 设置新的跨行数
                                    cell.InternalSetRowSpan(newRowSpan);
                                }
                                else
                                {
                                    cell.InternalSetRowSpan(1);
                                }
                                cell.Width = 0;
                                cell.Height = 0;
                            }
                        }
                    }
                    result = true;
                }
                if (newColsNum == this.ColSpan)
                {
                    if (this.ColSpan > 1)
                    {
                        // 如果新列数等于跨列数，则进行简单的纵向拆分
                        var cells = table.GetRangeArray(
                            this.RowIndex,
                            this.ColIndex,
                            currentRowSpan,
                            this.ColSpan,
                            true);
                        foreach (DomTableCellElement cell in cells)
                        {
                            cell.InternalSetColSpan(1);
                            cell.Width = 0;
                            cell.Height = 0;
                            if (cell.RowIndex == this.RowIndex)
                            {
                                cell.InternalSetRowSpan(newRowSpan);
                            }
                            cell.SizeInvalid = true;
                        }
                        result = true;
                    }
                }
                else
                {

                    result = true;

                    // 获得旧的表格列的位置
                    List<NewColInfo> infos = new List<NewColInfo>();
                    float left = 0;
                    for (int ci = 0; ci < this.ColSpan; ci++)
                    {
                        DomTableColumnElement col = (DomTableColumnElement)table.Columns[ci + currentColIndex];
                        NewColInfo info = new NewColInfo();
                        info.Pos = left;
                        info.Width = col.Width;
                        info.OldColumn = col;
                        infos.Add(info);
                        left = left + col.Width;
                    }
                    // 添加新增的表格列的位置
                    float step = currentWidth / newColsNum;
                    for (int ci = 0; ci < newColsNum; ci++)
                    {
                        float pos = step * ci;
                        bool match = false;
                        for (int iCount = 0; iCount < infos.Count; iCount++)
                        {
                            if (Math.Abs(pos - infos[iCount].Pos) < step * 0.2)
                            {
                                infos[iCount].NewColumnPosFlag = true;
                                pos = infos[iCount].Pos;
                                match = true;
                                break;
                            }
                        }
                        if (match == false)
                        {
                            NewColInfo info = new NewColInfo();
                            info.Pos = pos;
                            info.NewColumnPosFlag = true;
                            infos.Add(info);
                        }
                    }
                    infos.Sort();
                    // 计算新的列宽度
                    for (int iCount = 0; iCount < infos.Count; iCount++)
                    {
                        infos[iCount].Index = iCount;
                        if (iCount > 0)
                        {
                            infos[iCount - 1].Width = infos[iCount].Pos - infos[iCount - 1].Pos;
                        }
                    }
                    infos[infos.Count - 1].Width = currentWidth - infos[infos.Count - 1].Pos;
                    NewColInfo lastInfo = null;
                    for (int iCount = 0; iCount < infos.Count; iCount++)
                    {
                        NewColInfo info = infos[iCount];
                        // 创建新的表格列
                        DomTableColumnElement newCol = table.CreateColumnInstance();
                        newCol.InnerSetOwnerDocumentParentRaw(document, table);
                        //newCol.OwnerDocument = this.OwnerDocument;
                        //newCol.Parent = table;
                        newCol.Width = info.Width;
                        newCol.InnerCells = new DomElementList();
                        info.NewColumn = newCol;
                        if (info.OldColumn != null)
                        {
                            // 添加旧的单元格
                            DomElementList oldCells = info.OldColumn.Cells;
                            foreach (DomTableCellElement cell in oldCells)
                            {
                                newCol.InnerCells.Add(cell);
                            }
                            lastInfo = info;
                        }
                        else
                        {
                            // 创建新的单元格
                            DomElementList oldCells = lastInfo.OldColumn.Cells;
                            foreach (DomTableCellElement cell in oldCells)
                            {
                                DomTableCellElement newCell = table.CreateCellInstance(cell);
                                newCell.InnerSetOwnerDocumentParentRaw(document, cell.Parent);
                                //newCell.OwnerDocument = this.OwnerDocument;
                                //newCell.Parent = cell.Parent;
                                newCol.InnerCells.FastAdd2(newCell);
                                int ri = oldCells.IndexOf(cell);
                                if (ri >= currentRowIndex && ri < currentRowIndex + newRowsNum)
                                {

                                }
                                else
                                {
                                    if (cell.OverrideCell != null)
                                    {
                                        DomTableCellElement oc = cell.OverrideCell;
                                        if (oc.OwnerRow == cell.OwnerRow)
                                        {
                                            oc.InternalSetColSpan(oc.ColSpan + 1);
                                        }
                                    }
                                    else
                                    {
                                        cell.InternalSetColSpan(cell.ColSpan + 1);
                                    }
                                }
                            }
                        }
                        if (info.NewColumnPosFlag)
                        {
                            if (iCount == infos.Count - 1)
                            {
                                // 为最后一列
                                for (int ri = currentRowIndex; ri < currentRowIndex + newRowsNum; ri++)
                                {
                                    DomTableCellElement cell = (DomTableCellElement)info.NewColumn.InnerCells[ri];
                                    cell.InternalSetColSpan(1);
                                    cell.Width = 0;
                                }
                            }
                            else
                            {
                                // 设置单元格的跨列数
                                for (int iCount2 = iCount + 1; iCount2 < infos.Count; iCount2++)
                                {
                                    int newColSpan = -1;
                                    if (iCount2 == infos.Count - 1)
                                    {
                                        newColSpan = iCount2 - iCount;
                                        if (newColsNum < currentColSpan && infos[iCount2].NewColumnPosFlag == false)
                                        {
                                            newColSpan++;
                                        }
                                    }
                                    else if (infos[iCount2].NewColumnPosFlag)
                                    {
                                        newColSpan = iCount2 - iCount;
                                    }
                                    if (newColSpan > 0)
                                    {
                                        for (int ri = currentRowIndex; ri < currentRowIndex + newRowsNum; ri++)
                                        {
                                            DomTableCellElement cell = (DomTableCellElement)info.NewColumn.InnerCells[ri];
                                            cell.InternalSetColSpan(newColSpan);
                                            cell.Width = 0;
                                        }
                                    }
                                    if (newColSpan > 0)
                                    {
                                        break;
                                    }
                                }//for
                            }//if
                            // 设置单元格的跨行数
                            for (int ri = currentRowIndex; ri < currentRowIndex + currentRowSpan; ri += newRowSpan)
                            {
                                DomTableCellElement cell = (DomTableCellElement)info.NewColumn.InnerCells[ri];
                                cell.InternalSetRowSpan(newRowSpan);
                                cell.Width = 0;
                            }
                        }
                    }//for
                    // 对于新增的元素，执行元素大小计算
                    using (DCGraphics g = document.InnerCreateDCGraphics())
                    {
                        InnerDocumentPaintEventArgs args = document.CreateInnerPaintEventArgs(g);
                        for (int iCount = 0; iCount < infos.Count; iCount++)
                        {
                            NewColInfo info = infos[iCount];
                            if (info.NewColumnPosFlag)
                            {
                                // 对于旧单元格，以前被合并掉了，没有参与计算单元格的宽度
                                // 现在再次显示，需要重新计算大小
                                foreach (DomTableCellElement cell in info.NewColumn.InnerCells)
                                {
                                    if (cell.IsOverrided)
                                    {
                                        args.Element = cell;
                                        cell.FixElements();
                                        cell.RefreshSize(args);
                                    }
                                }

                                //if (info.OldColumn == null)
                                //{
                                //    foreach (XTextTableCellElement cell in info.NewColumn.InnerCells)
                                //    {
                                //        args.Element = cell;
                                //        cell.FixElements();
                                //        cell.RefreshSize(args);
                                //    }
                                //}
                                //else
                                //{
                                //    // 对于旧单元格，以前被合并掉了，没有参与计算单元格的宽度
                                //    // 现在再次显示，需要重新计算大小
                                //    foreach (XTextTableCellElement cell in info.OldColumn.InnerCells)
                                //    {
                                //        if (cell.IsOverrided)
                                //        {
                                //            args.Element = cell;
                                //            cell.FixElements();
                                //            cell.RefreshSize(args);
                                //        }
                                //    }
                                //}
                            }//if
                        }//foreach
                    }//using

                    // 进行表格列替换
                    for (int iCount = currentColIndex + currentColSpan - 1;
                        iCount >= currentColIndex; iCount--)
                    {
                        table.Columns.RemoveAtRaw(iCount);
                        // 删除对应的单元格
                        foreach (DomTableRowElement row in table.Rows)
                        {
                            row.Cells.RemoveAtRaw(iCount);
                        }
                    }
                    // 添加新的表格列
                    for (int iCount = 0; iCount < infos.Count; iCount++)
                    {
                        DomTableColumnElement col = infos[iCount].NewColumn;
                        table.Columns.Insert(currentColIndex + iCount, col);
                        // 添加对应的单元格
                        for (int ri = 0; ri < table.Rows.Count; ri++)
                        {
                            DomTableRowElement row = (DomTableRowElement)table.Rows[ri];
                            DomTableCellElement cell2 = (DomTableCellElement)col.InnerCells[ri];
                            row.Cells.Insert(currentColIndex + iCount, cell2);
                        }
                        col.InnerCells = null;
                    }
                }
            }

            if (result)
            {
                table.FixDomState();
                this.UpdateContentVersion();
                // 更新单元格的合并状态
                table.UpdateCellOverrideState();
                // 压缩表格行列
                if (table.CompressRowsColumns())
                {
                    table.UpdateCellOverrideState();
                }
                foreach (DomTableCellElement cell in table.Cells)
                {
                    cell.FixElements();
                }
                if (this.BelongToDocumentDom(document) == false)
                {
                    // 表格还不属于某个文档DOM结构中，无法执行后续操作。
                    return result;
                }
                var dce = this.DocumentContentElement;
                if (dce != null)
                {
                    dce.RefreshGlobalLines();
                }
                this.UpdateContentElements(true);
                // 设置文档的选择状态
                //XTextContent c = this.DocumentContentElement.Content;
                table.InvalidateView();
                using (DCGraphics g = document.InnerCreateDCGraphics())
                {
                    table.RefreshInvalidateCellSize(g);
                }
                table.ExecuteLayout(false);
                table.InvalidateView();
                table.UpdatePagesForTable(false);
                if (dce != null)
                {
                    dce.SetSelection(
                        this.FirstContentElement.ContentIndex,
                        this.LastContentElement.ContentIndex - this.FirstContentElement.ContentIndex);
                }
                if (undo != null)
                {
                    undo.NewTableInfo = new TableStructInfo(table, true);
                    undo.CompressElements();
                    // 添加撤销操作信息
                    if (document.BeginLogUndo())
                    {
                        document.AddUndo(undo);
                        document.EndLogUndo();
                        document.OnSelectionChanged();
                        document.OnDocumentContentChanged();
                    }
                }
            }
            return result;
        }

        private class NewColInfo : IComparable<NewColInfo>
        {
            public int Index = 0;
            public float Pos = 0;
            public float Width = 0;
            public DomTableColumnElement OldColumn = null;
            public DomTableColumnElement NewColumn = null;
            public bool NewColumnPosFlag = false;
            //public bool MatchOldColumnFlag = false;

            public int CompareTo(NewColInfo other)
            {
                return (int)(this.Pos - other.Pos);
            }
#if !RELEASE
            public override string ToString()
            {
                string txt = this.Pos.ToString("0.0") + " " + this.Width.ToString("0.0");
                if (NewColumnPosFlag)
                {
                    txt = txt + " NewFlag";
                }
                if (OldColumn != null)
                {
                    txt = txt + " HasOldCol";
                }
                return txt;
            }
#endif
        }



        /// <summary>
        /// 编辑器中设置单元格的合并信息
        /// </summary>
        /// <param name="newRowSpan">新的横向合并行数</param>
        /// <param name="newColSpan">新的纵向合并列数</param>
        /// <param name="logUndo">是否记录撤销操作信息</param>
        /// <param name="cellContents">单元格内容</param>
        /// <returns>操作是否成功</returns>
        public bool EditorSetCellSpan(
            int newRowSpan,
            int newColSpan,
            bool logUndo,
            Dictionary<DomTableCellElement, DomElementList> cellContents)
        {
            newRowSpan = FixRowSpan(newRowSpan);
            newColSpan = FixColSpan(newColSpan);
            if (newRowSpan == this.RowSpan && newColSpan == this.ColSpan)
            {
                // 无需进行处理
                return false;
            }
            DCSoft.Writer.Dom.Undo.XTextUndoTableInfo undo = null;
            if (logUndo)
            {
                // 创建撤销操作记录信息对象
                undo = new DCSoft.Writer.Dom.Undo.XTextUndoTableInfo();
                undo.OldTableInfo = new TableStructInfo(this.OwnerTable, true);
            }
            DomTableElement table = this.OwnerTable;
            var document = this.OwnerDocument;
            DocumentContentStyle newStyle = (DocumentContentStyle)this.RuntimeStyle.CloneParent();
            newStyle.DisableDefaultValue = true;
            bool applyNewStyle = false;
            if (this.ColSpan < newColSpan)
            {
                DomTableCellElement rightCell = table.GetCell(this.RowIndex, this.ColIndex + newColSpan - 1, false);
                if (rightCell != null && rightCell.IsOverrided == false)
                {
                    newStyle.BorderRight = rightCell.RuntimeStyle.BorderRight;
                    newStyle.BorderRightColor = rightCell.RuntimeStyle.BorderRightColor;
                    applyNewStyle = true;
                }
            }
            if (this.RowSpan < newRowSpan)
            {
                DomTableCellElement bottomCell = table.GetCell(this.RowIndex + newRowSpan - 1, this.ColIndex, false);
                if (bottomCell != null && bottomCell.IsOverrided == false)
                {
                    newStyle.BorderBottom = bottomCell.RuntimeStyle.BorderBottom;
                    newStyle.BorderBottomColor = bottomCell.RuntimeStyle.BorderBottomColor;
                    applyNewStyle = true;
                }
            }
            int oldStyleIndex = this.StyleIndex;
            int newStyleIndex = oldStyleIndex;
            if (applyNewStyle)
            {
                newStyleIndex = document.ContentStyles.GetStyleIndex(newStyle);
            }
            if (cellContents != null)
            {
                // 用户指定了单元格的内容
                foreach (DomTableCellElement cell in cellContents.Keys)
                {
                    DomElementList list = cellContents[cell];
                    cell.Elements.Clear();
                    cell.Elements.AddRangeByDCList(list);
                    foreach (DomElement element in cell.Elements)
                    {
                        element.InnerSetOwnerDocumentParentRaw(document, cell);
                        //element.Parent = cell;
                        //element.OwnerDocument = this.OwnerDocument;
                    }//foreach
                    cell.UpdateContentElements(false);
                    cell.Width = 0;
                }//foreach
            }
            else
            {
                // 将合并后的单元格的内容进行合并
                var oldCells = table.GetRangeArray(
                    this.RowIndex,
                    this.ColIndex,
                    this._RowSpan,
                    this._ColSpan,
                    true);

                var cells = table.GetRangeArray(
                    this.RowIndex,
                    this.ColIndex,
                    newRowSpan,
                    newColSpan,
                    true);

                foreach (DomTableCellElement cell in oldCells)
                {
                    if (Array.IndexOf(cells, cell) < 0)// cells.Contains(cell) == false )
                    {
                        // 原先已经本合并的单元格重现天日，即将要显示出来
                        // 修正内容
                        cell.UpdateContentElements(false);
                        cell.Width = 0;
                    }
                }//foreach

                DomElementList tempList = new DomElementList();
                foreach (DomTableCellElement cell in cells)
                {
                    if (cell != this && cell.OverrideCell == null)
                    {
                        if (cell.Elements.Count > 1)
                        {
                            // 单元格内容不为空，则移动到合并的大单元格中
                            tempList.AddRangeByDCList(cell.Elements);
                            //if (tempList.LastElement is XTextParagraphFlagElement)
                            //{
                            //    XTextParagraphFlagElement pf = (XTextParagraphFlagElement)tempList.LastElement;
                            //    if (pf.AutoCreate)
                            //    {
                            //        tempList.RemoveAt(tempList.Count - 1);
                            //    }
                            //}
                            cell.Elements.Clear();
                            cell.Width = 0;
                            cell.UpdateContentElements(false);
                        }
                    }
                }
                for (int iCount = tempList.Count - 2; iCount >= 0; iCount--)
                {
                    // 删除中间的自动产生的段落符号
                    if (tempList[iCount] is DomParagraphFlagElement)
                    {
                        DomParagraphFlagElement pf = (DomParagraphFlagElement)tempList[iCount];
                        if (pf.AutoCreate)
                        {
                            tempList.RemoveAtRaw(iCount);
                        }
                    }
                }
                this.Elements.AddRangeByDCList(tempList);
                foreach (DomElement e in tempList)
                {
                    e.Parent = this;
                }
                //if (undo != null)
                //{
                //    // 记录新单元格内容
                //    undo.LogNewCellsContent();
                //}
            }

            // 设置新的跨行跨列数
            this._RowSpan = newRowSpan;
            this._ColSpan = newColSpan;
            this.StyleIndex = newStyleIndex;
            this.Width = 0;
            this.UpdateContentVersion();
            // 更新单元格的合并状态
            table.UpdateCellOverrideState();
            if (table.CompressRowsColumns())
            {
                // 合并表格行列
                table.UpdateCellOverrideState();
            }

            this.UpdateContentElements(true);
            // 设置文档的选择状态
            // XTextContent c = this.DocumentContentElement.Content;

            this.DocumentContentElement.SetSelection(
                this.FirstContentElement.ContentIndex,
                this.LastContentElement.ContentIndex - this.FirstContentElement.ContentIndex);

            table.InvalidateView();
            table.InnerExecuteLayout();
            table.UpdatePagesForTable(true);
            table.InvalidateView();
            table.UpdatePagesForTable(false);
            if (undo != null)
            {
                undo.NewTableInfo = new TableStructInfo(table, true);
                undo.CompressElements();
                // 添加撤销操作信息
                if (document.BeginLogUndo())
                {
                    if (oldStyleIndex != newStyleIndex)
                    {
                        document.UndoList.AddStyleIndex(this, oldStyleIndex, newStyleIndex);
                    }
                    document.AddUndo(undo);
                    document.EndLogUndo();
                    document.OnSelectionChanged();
                    document.OnDocumentContentChanged();
                }
            }
            return true;
        }

        /// <summary>
        /// 计算元素大小
        /// </summary>
        /// <param name="args">参数</param>
        public override void RefreshSize(InnerDocumentPaintEventArgs args)
        {
            this.LayoutInvalidate = true;
            base.RefreshSize(args);
        }

        /// <summary>
        /// 复制对象
        /// </summary>
        /// <param name="Deeply">是否深度复制</param>
        /// <returns>复制品</returns>
        public override DomElement Clone(bool Deeply)
        {
            DomTableCellElement cell = (DomTableCellElement)base.Clone(Deeply);
            cell._OwnerColumn = null;
            cell._OverrideCell = null;

            //cell.SetOverrideCell(null);
            return cell;
        }
#if !RELEASE
        public override string ToString()
        {
            return this.ToDebugString();
        }

        public override string ToDebugString()
        {
            string result = null;
            if (this.OwnerTable == null && string.IsNullOrEmpty(this.OwnerTable.ID))
            {
                result = "Cell:" + this.CellID;
            }
            else
            {
                result = "Cell:" + this.OwnerTable.ID + "!" + this.CellID;
            }
            if (this.ID != null && this.ID.Length > 0)
            {
                result = result + "(" + this.ID + ")";
            }
            return result;
        }
#endif
        /// <summary>
        /// 为编辑器的上层操作而复制表格行对象
        /// </summary>
        /// <returns>复制品</returns>
        public DomTableCellElement EditorClone()
        {
            var newCell = (DomTableCellElement)this.Clone(false);
            newCell.RowSpan = 1;
            newCell.SetOverrideCell(null);
            newCell.FixElements();
            if (this.Elements.Count > 0)
            {
                newCell.Elements.LastElement.StyleIndex = this.Elements.LastElement.StyleIndex;
                if (newCell.Elements.LastElement is DomParagraphFlagElement)
                {
                    ((DomParagraphFlagElement)newCell.Elements.LastElement).AutoCreate = false;
                }
            }
            return newCell;
        }

        public override void Dispose()
        {
            base.Dispose();
            this._OverrideCell = null;
            this._OwnerColumn = null;
        }
    }
}