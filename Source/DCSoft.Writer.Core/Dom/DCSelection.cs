using System;
using DCSoft.Drawing;
using DCSoft.Common;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using DCSoft.Writer.Serialization;

namespace DCSoft.Writer.Dom
{
    /// <summary>
    /// 文档区域对象
    /// </summary>
#if !RELEASE
    [System.Diagnostics.DebuggerDisplay("StartIndex={StartIndex},Length={Length}")]
    [System.Diagnostics.DebuggerTypeProxy(typeof(DCSoft.Common.ListDebugView))]
#endif
    public partial class DCSelection : System.Collections.IEnumerable , IDisposable
    {
        /// <summary>
        /// 初始化对象
        /// </summary>
        public DCSelection(DomDocumentContentElement dce)
        {
            _DocumentContent = dce;
        }
#if !RELEASE
        public override string ToString()
        {
            return this._NativeStartIndex + "=>" + this._NativeLength + " Ver:" + this._StateVersion;
        }
#endif
        private ContentRangeMode _Mode = ContentRangeMode.Content;
        /// <summary>
        /// 选择的内容的模式
        /// </summary>
        public ContentRangeMode Mode
        {
            get { return _Mode; }
        }

        private DomDocumentContentElement _DocumentContent = null;
        /// <summary>
        /// 所属的文档容器元素对象
        /// </summary>

        public DomDocumentContentElement DocumentContent
        {
            get { return _DocumentContent; }
        }
        /// <summary>
        /// 所属的文档对象
        /// </summary>
        public DomDocument Document
        {
            get
            {
                return _DocumentContent.OwnerDocument;
            }
        }
        /// <summary>
        /// 内容容器
        /// </summary>
        private DCContent Content
        {
            get
            {
                return _DocumentContent.Content;
            }
        }
        internal void FixIndex(DCContent c)
        {
            if (c != null)
            {
                if (this._StartIndex < 0)
                {
                    this._StartIndex = 0;
                }
                if (c.Count > 0 && this._StartIndex > c.Count - 1)
                {
                    this._StartIndex = c.Count - 1;
                }
                if (this._NativeStartIndex < 0)
                {
                    this._NativeStartIndex = 0;
                }
                if (this._NativeStartIndex > c.Count - 1)
                {
                    this._NativeStartIndex = c.Count - 1;
                }
            }
        }
        private DomElementList _Cells = new DomElementList();
        /// <summary>
        /// 对象包含的单元格对象列表，该列表包括被合并而隐藏的单元格
        /// </summary>
        public DomElementList Cells
        {
            get
            {
                return _Cells;
            }
        }

        internal bool ContainsCell(DomTableCellElement cell)
        {
            if (this._Cells != null && this._Cells.Count > 0)
            {
                if( this._Cells.EqualsElement( cell , cell.IndexInSelectionCells))
                {
                    return true;
                }
                //int index = cell.IndexInSelectionCells;
                //if (index >= 0 && index < this._Cells.Count && this._Cells[index] == cell)
                //{
                //    return true;
                //}
                //var bolResult =  this._Cells.Contains(cell);
                //if( bolResult )
                //{

                //}
                //return bolResult;
            }
            return false;
        }

        private bool ContainsCell(DomElementList cells, DomTableCellElement cell)
        {
            if (cells != null && cells.Count > 0)
            {
                int index = cell.IndexInSelectionCells;
                if (index >= 0 && index < cells.Count && cells[index] == cell)
                {
                    return true;
                }
                //if (cell.IndexInSelectionCells >= 0 && cell.IndexInSelectionCells < cells.Count)
                //{
                //    if (cells[cell.IndexInSelectionCells] == cell)
                //    {
                //        return true;
                //    }
                //}
                return cells.Contains(cell);
            }
            return false;
        }

        /// <summary>
        /// 对象包含的单元格对象列表，该列表不包含合并而隐藏的单元格
        /// </summary>
        public DomElementList CellsWithoutOverried
        {
            get
            {
                if (_Cells != null && _Cells.Count > 0)
                {
                    DomElementList result = new DomElementList();
                    foreach (DomTableCellElement cell in this.Cells)
                    {
                        if (cell.IsOverrided == false)
                        {
                            result.Add(cell);
                        }
                    }
                    return result;
                }
                else
                {
                    return null;
                }
            }
        }

        ///// <summary>
        ///// 判断视图元素编号是否包含内容中
        ///// </summary>
        ///// <param name="viewIndex">视图元素编号</param>
        ///// <returns>是否包含</returns>
        //[System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        //public bool ContainsViewIndex(int viewIndex)
        //{
        //    if (viewIndex >= 0 )
        //    {
        //        return viewIndex >= this.ContentElements.FirstElement.ViewIndex
        //            && viewIndex <= this.ContentElements.LastElement.ViewIndex;
        //    }
        //    return false;
        //}

        private DomElementList _ContentElements = new DomElementList();
        /// <summary>
        /// 内容元素列表
        /// </summary>
        public DomElementList ContentElements
        {
            get
            {
                return _ContentElements;
            }
        }

        private DomElementList _SelectionParagraphFlags = null;
        /// <summary>
        /// 被选择的段落标记符号元素
        /// </summary>
        public DomElementList SelectionParagraphFlags
        {
            get
            {
                if (_SelectionParagraphFlags == null)
                {
                    _SelectionParagraphFlags = new DomElementList();
                    if (_ContentElements != null && _ContentElements.Count > 0)
                    {
                        foreach (DomElement element in _ContentElements)
                        {
                            if (element is DomParagraphFlagElement)
                            {
                                _SelectionParagraphFlags.Add(element);
                            }
                        }//foreach
                        DomElement lastElement = _ContentElements.LastElement;
                        if ((lastElement is DomParagraphFlagElement) == false)
                        {
                            DomElementList ces = lastElement.ContentElement.PrivateContent;
                            for (int iCount = ces.IndexOf(lastElement) + 1; iCount < ces.Count; iCount++)
                            {
                                if (ces[iCount] is DomParagraphFlagElement)
                                {
                                    _SelectionParagraphFlags.Add(ces[iCount]);
                                    break;
                                }
                            }
                        }
                    }
                }
                return _SelectionParagraphFlags;
            }
        }

        /// <summary>
        /// 区域中第一个文档内容元素
        /// </summary>
        public DomElement FirstElement
        {
            get
            {
                if (_ContentElements == null)
                {
                    return null;
                }
                else
                {
                    return _ContentElements.FirstElement;
                }
            }
        }

        /// <summary>
        /// 区域中最后一个文档内容元素
        /// </summary>
        public DomElement LastElement
        {
            get
            {
                if (_ContentElements == null)
                {
                    return null;
                }
                else
                {
                    return _ContentElements.LastElement;
                }
            }
        }


        /// <summary>
        /// 原始的起始位置
        /// </summary>
        private int _NativeStartIndex = 0;
        /// <summary>
        /// 原始的起始位置
        /// </summary>
        public int NativeStartIndex
        {
            get { return _NativeStartIndex; }
        }

        private int _NativeLength = 0;
        /// <summary>
        /// 原始的区域长度
        /// </summary>
        public int NativeLength
        {
            get { return _NativeLength; }
        }


        /// <summary>
        /// 实际的起始位置
        /// </summary>
        private int _StartIndex = 0;
        /// <summary>
        /// 实际的起始位置
        /// </summary>
        public int StartIndex
        {
            get
            {
                return _StartIndex;
            }
        }

        private int _Length = 0;
        /// <summary>
        /// 实际的区域长度
        /// </summary>
        public int Length
        {
            get
            {
                return this._Length;
            }
        }
        /// <summary>
        /// 区域的绝对的开始位置
        /// </summary>
        public int AbsStartIndex
        {
            get
            {
                if (this._Length >= 0)
                {
                    return this._StartIndex;
                }
                else
                {
                    return this._StartIndex + this._Length;
                }
            }
        }
        /// <summary>
        /// 绝对的结束位置
        /// </summary>
        public int AbsEndIndex
        {
            get
            {
                if (this._Length >= 0)
                {
                    return this._StartIndex + this._Length;
                }
                else
                {
                    return this._StartIndex;
                }
            }
        }
        /// <summary>
        /// 选择区域的长度的绝对值
        /// </summary>
        public int AbsLength
        {
            get
            {
                return Math.Abs(this._Length);
            }
        }
        /// <summary>
        /// 判断元素是否包含在区域中
        /// </summary>
        /// <param name="element">元素对象</param>
        /// <returns>是否包含在区域中</returns>
        public bool Contains(DomElement element)
        {

            if (element == null)
            {
                return false;
            }
            if (element is DomTableCellElement)
            {
                if (this._Cells != null && this._Cells.Count > 0)
                {
                    return ContainsCell(this._Cells, (DomTableCellElement)element);// this._Cells.Contains(element);
                }
                return false;
            }
            else
            {
                if (this._ContentElements != null && this._ContentElements.Count > 0)
                {
                    if ((element is DomContainerElement) == false
                        && this.Content.SafeGet(element._ContentIndex) == element)
                    {
                        // 进行快速判断
                        int index = element._ContentIndex;
                        int index2 = index - this._ContentElements.FirstElement.ContentIndex;
                        if (index2 >= 0)
                        {
                            if (this._ContentElements.SafeGet(index2) == element)
                            {
                                return true;
                            }
                            if (index <= this._ContentElements.LastElement.ContentIndex)
                            {
                                return _ContentElements.Contains(element);
                            }
                        }
                    }
                    else
                    {
                        return this._ContentElements.Contains(element);
                    }
                }
                return false;
            }
        }
        /// <summary>
        /// 判断是否完全包含单元格
        /// </summary>
        /// <param name="cell"></param>
        /// <returns></returns>
        internal bool CompletedContainsForPrint(DomTableCellElement cell)
        {
            if (this._Cells != null && this._Cells.Contains(cell))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 判断元素是否包含在区域中
        /// </summary>
        /// <param name="element">元素对象</param>
        /// <returns>是否包含在区域中</returns>
        internal bool ContainsForPrint(DomElement element)
        {
            if (element == null)
            {
                return false;
            }
            if (element is DomTableCellElement)
            {
                if (_Cells != null && _Cells.Contains(element))
                {
                    return true;
                }
            }
            else if (element is DomTableElement)
            {
                // 表格元素
                if (this._Cells != null)
                {
                    foreach (DomTableCellElement cell in this._Cells)
                    {
                        if (cell.OwnerTable == element)
                        {
                            return true;
                        }
                    }
                }
                DomElement p = element;
                while (p != null)
                {
                    if (_ContentElements.Contains(p))
                    {
                        return true;
                    }
                    if (this._Cells != null && this._Cells.Contains(p))
                    {
                        return true;
                    }
                    p = p.Parent;
                }
            }
            else
            {
                if (_ContentElements.Contains(element))
                {
                    return true;
                }
            }
            foreach (DomElement e2 in this._ContentElements)
            {
                if (e2.IsParentOrSupParent(element))
                {
                    return true;
                }
            }
            if (this._Cells != null && this._Cells.Count > 0)
            {
                foreach (DomElement e2 in this._Cells)
                {
                    if (e2.IsParentOrSupParent(element))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private int _SelectionVersion = 0;
        /// <summary>
        /// 选择状态版本号，每修改一次选择状态则该版本号就会增加1
        /// </summary>
        public int SelectionVersion
        {
            get
            {
                return _SelectionVersion;
            }
        }

        /// <summary>
        /// 最后一次设置状态时的文档内容版本号
        /// </summary>
        private int _ContentVersion = 0;
        /// <summary>
        /// 最后一次设置状态时的文档行尾标记值
        /// </summary>
        private bool _LineEndFlag = false;
        /// <summary>
        /// 判断是否需要刷新选择区域状态
        /// </summary>
        /// <param name="newStartIndex">新的选择区域位置</param>
        /// <param name="newLength">新的选择区域长度</param>
        /// <returns>是否需要刷新选择区域状态</returns>
        internal bool NeedRefresh(int newStartIndex, int newLength)
        {
            if (_NativeStartIndex == newStartIndex
                && _NativeLength == newLength
                && this.DocumentContent.OwnerDocument.ContentVersion == _ContentVersion
                && this.Content.LineEndFlag == _LineEndFlag)
            {
                return false;
            }
            else
            {
                return true;
            }
            return false;
        }
        internal bool SelectCells(DomTableCellElement firstCell, DomTableCellElement lastCell)
        {
            if (firstCell == null)
            {
                throw new ArgumentNullException("firstCell");
            }
            if (lastCell == null)
            {
                lastCell = firstCell;
            }
            DomTableElement table = firstCell.OwnerTable;
            if (table == null)
            {
                throw new ArgumentNullException("firstCell.OwnerTable");
            }
            if (lastCell.OwnerTable != table)
            {
                throw new ArgumentException("不是同一个表格");
            }
            this._ContentElements = new DomElementList();
            this._Cells = new DomElementList();

            this._Mode = ContentRangeMode.Cell;

            // 纯粹的在同一个表格中选择单元格，只选择开始单元格和结束单元格之间的矩形选择区域中的单元格

            // 获得选中的单元格对象
            _Cells = table.GetSelectionCells(
                firstCell.RowIndex,
                firstCell.ColIndex,
                lastCell.RowIndex,
                lastCell.ColIndex);
            foreach (DomTableCellElement cell in _Cells)
            {
                if (cell.IsOverrided == false)
                {
                    MyAddRange(_ContentElements, cell.PrivateContent, null, false);
                    //this._Cells.Add(cell);
                }
            }//foreach
            if (this._ContentElements.Count > 0)
            {
                this._StartIndex = _ContentElements[0].ContentIndex;
                this._Length = _ContentElements.Count;
                this._NativeStartIndex = this._StartIndex;
                this._NativeLength = this._Length;
                this._ContentVersion = this.DocumentContent.OwnerDocument.ContentVersion;
                this._LineEndFlag = false;
                this._SelectionParagraphFlags = null;
                this._SelectionVersion++;
                return true;
            }
            return false;
        }
        private DomElement _StartElement = null;
        private DomElement _EndElement = null;

        private void UpdateStartEndElement()
        {
            this._StartElement = this.Content.SafeGet(this._StartIndex);
            this._EndElement = this.Content.SafeGet(this._StartIndex + this._Length);
        }
        /// <summary>
        /// 更新状态
        /// </summary>

        public void UpdateState()
        {
            Refresh(this._NativeStartIndex, this._NativeLength, true);
        }


        /// <summary>
        /// 更新文内容选择状态
        /// </summary>
        /// <param name="startIndex">选择区域的起始位置</param>
        /// <param name="length">选择区域的包含文档内容元素的个数</param>
        /// <returns>成功的改变了选择区域状态</returns>
        public bool Refresh(int startIndex, int length)
        {
            return Refresh(startIndex, length, false);
        }
        /// <summary>
        /// 被选择区域是否很大。
        /// </summary>
        /// <returns>是否很大</returns>
        public bool IsBig()
        {
            if (this._Cells != null && this._Cells.Count > 100)
            {
                return true;
            }
            if (this._ContentElements != null && this._ContentElements.Count > 1000)
            {
                return true;
            }
            return false;
        }
        private int _StateVersion = 0;
        /// <summary>
        /// 对象状态版本号，只要修改了选择区域该版本号就会自增1。
        /// </summary>
        public int StateVersion
        {
            get
            {
                return _StateVersion;
            }
            set
            {
                _StateVersion = value;
            }
        }

        /// <summary>
        /// 增加状态版本号
        /// </summary>
        public void IncreaseStateVersion()
        {
            this._StateVersion++;
        }

        internal void Reset()
        {
            if( this._Cached_ParagraphsEOFs != null )
            {
                this._Cached_ParagraphsEOFs.Clear();
                this._Cached_ParagraphsEOFs = null;
            }
            this._Cells = new DomElementList();
            this._ContentElements = new DomElementList();
            this._ContentVersion = this.Document.ContentVersion;
            this._EndElement = null;
            this._Length = 0;
            this._Mode = ContentRangeMode.Content;
            this._NativeLength = 0;
            this._NativeStartIndex = 0;
            this._SelectionParagraphFlags = null;
            this._SelectionVersion = 0;
            this._StartElement = null;
            this._StartIndex = 0;
            this._Ranges = null;
            this._StateVersion++;
        }

        private XTextContentRangeList _Ranges = null;
        /// <summary>
        /// 文档区域信息列表
        /// </summary>
        public XTextContentRangeList Ranges
        {
            get
            {
                if (this._Ranges == null)
                {
                    this._Ranges = new XTextContentRangeList(this._ContentElements);
                }
                return this._Ranges;
            }
        }
        /// <summary>
        /// 判断所有的内容是否属于文档内容
        /// </summary>
        /// <returns></returns>
        internal bool AllBelongToContent()
        {
            var document = this.Document;
            var ce = this.Content;
            if (this._ContentElements != null && this._ContentElements.Count > 0)
            {
                foreach (var element in this._ContentElements)
                {
                    if( ce.FastIndexOf( element ) < 0 )
                    {
                        return false;
                    }
                }
            }
            if( this._Cells != null && this._Cells.Count > 0 )
            {
                foreach( DomTableCellElement cell in this._Cells)
                {
                    if( cell.BelongToDocumentDom( document ) == false )
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        /// <summary>
        /// 更新文内容选择状态
        /// </summary>
        /// <param name="startIndex">选择区域的起始位置</param>
        /// <param name="length">选择区域的包含文档内容元素的个数</param>
        /// <param name="forceRefresh">强制刷新</param>
        /// <returns>成功的改变了选择区域状态</returns>
        internal bool Refresh(int startIndex, int length, bool forceRefresh)
        {
            //if(length != 0 )
            //{
            //    var s = 3;
            //}
            //Console.WriteLine("Pos:" + startIndex);
            var document = this.Document;
            if (document == null)
            {
                return false;
            }

            var content = this.Content;
            content._SelectAllFlag = false;
            if (forceRefresh == false)
            {
                if (this._NativeStartIndex == startIndex
                    && this._NativeLength == length
                    && document.ContentVersion == this._ContentVersion
                    && content.LineEndFlag == this._LineEndFlag)
                {
                    return false;
                }
            }
            document.CacheOptions();
            this._Ranges = null;
            if (document != null && document.EditorControl != null)
            {
                document.EditorControl.IncreaseSelectionVersion();
                document.EditorControl.CommandStateNeedRefreshFlag = true;
                //if (document.InnerViewControl() != null
                //    && document.InnerViewControl().ViewHandleManager() != null)
                //{
                //    //this.Document.EditorControl.InnerViewControl.ViewHandleManager.ClearLastDrawItems();
                //}
            }
            this._StateVersion++;
            //if (startIndex == 0 || startIndex + length == 0)
            //{
            //    System.Console.Write(string.Empty);
            //}
            //if (Math.Abs(length) > 10)
            //{
            //}
            if (startIndex < 0)
            {
                throw new ArgumentOutOfRangeException("startIndex=" + startIndex);
            }
            var contentCount = content.Count;
            if (startIndex >= contentCount)
            {
                startIndex = contentCount - 1;
            }
            if (startIndex + length >= contentCount)
            {
                length = contentCount - startIndex;
            }
            if (startIndex + length < 0)
            {
                length = -startIndex;
            }
            //if (length != 0)
            //{
            //}
            this._SelectionVersion++;
            document.CurrentStyleInfo = null;
            // 备份原先被选择的元素的列表 
            DomElementList contentElementsBack = null;
            if (this._ContentElements.Count > 0)
            {
                contentElementsBack = this._ContentElements.Clone();
            }
            this._ContentElements = new DomElementList();
            this._SelectionParagraphFlags = null;
            DomElementList oldCells = this._Cells.Clone();
            this._Cells = new DomElementList();

            this._ContentVersion = document.ContentVersion;

            if (startIndex == 0 && length == 0)
            {
                this._NativeStartIndex = 0;
                this._NativeLength = 0;
                this._StartIndex = 0;
                this._Length = 0;
                this.UpdateStartEndElement();
                this._LineEndFlag = content.LineEndFlag;
                this._Mode = ContentRangeMode.Content;
                this._ContentElements = new DomElementList();
                this._Cells = new DomElementList();
                this._StartElement = content.SafeGet(this._NativeStartIndex);
                this._EndElement = content.SafeGet(this._NativeStartIndex + this._NativeLength);
                //return true;
            }
            else
            {
                this._NativeStartIndex = startIndex;
                this._NativeLength = length;
                this._LineEndFlag = content.LineEndFlag;
                //if (contentChanged)
                //{
                //    XTextContentElement ce = element6.ContentElement;
                //    ce.RefreshPrivateContent(ce.PrivateContent.IndexOf(element6));
                //}


                this._Mode = ContentRangeMode.Content;
                // 第一个单元格
                DomTableCellElement firstCell = null;
                // 最后一个单元格
                DomTableCellElement lastCell = null;
                // 所经过的单元格个数
                int cellCount = 0;
                //XTextElementList spanCells = new XTextElementList();
                // 是否有不包含在表格单元格中的文档内容元素
                bool hasContentElement = false;

                int absStartIndex = length > 0 ? startIndex : startIndex + length;
                int absLength = Math.Abs(length);
                int lengthFix = 1;
                if (content.LineEndFlag && length < 0)
                {
                    lengthFix = 0;
                }
                else
                {
                    lengthFix = 0;
                }
                // 遍历所有的文档元素，查找所经过的单元格
                //float tick3 = CountDown.GetTickCountFloat();
                int totalCount = absLength + lengthFix;
                // 选择区域经过的单元格列表
                var spanElements = new List<DomElement>(totalCount);
                DomElement currentParent = null;
                DomTableCellElement currentOwnerCell = null;
                for (int iCount = 0; iCount < totalCount; iCount++)
                {
                    if (absStartIndex + iCount >= contentCount)
                    {
                        break;
                    }
                    DomElement element3 = content[absStartIndex + iCount];
                    var element3Parent = element3.Parent;
                    if (element3Parent == this._DocumentContent)
                    {
                        spanElements.Add(element3);
                        hasContentElement = true;
                    }
                    else
                    {
                        if (element3Parent != currentParent)
                        {
                            currentParent = element3Parent;
                            currentOwnerCell = element3.OwnerCell;
                        }
                        if (currentOwnerCell != null)
                        {
                            if (currentOwnerCell != lastCell)
                            {
                                spanElements.Add(currentOwnerCell);
                                cellCount++;
                            }
                            if (firstCell == null)
                            {
                                firstCell = currentOwnerCell;
                            }
                            //if (lastCell != currentOwnerCell)
                            {
                                lastCell = currentOwnerCell;
                            }
                        }
                        else
                        {
                            spanElements.Add(element3);
                            hasContentElement = true;
                        }
                    }
                }//for
                //tick3 = CountDown.GetTickCountFloat() - tick3;
                // 添加插入点所在的单元格
                DomTableCellElement cell2 = content[startIndex].OwnerCell;
                if (cell2 != null && spanElements.Contains(cell2) == false)
                {
                    if (firstCell == null)
                    {
                        firstCell = cell2;
                    }
                    if (length < 0 || lastCell == null)
                    {
                        lastCell = cell2;
                    }
                    else
                    {

                    }
                    spanElements.Add(cell2);
                    cellCount++;
                }
                // 首先判断内容模式
                this._Mode = ContentRangeMode.Content;
                if (hasContentElement == false && cellCount == 1)
                {
                    // 全部都只在一个单元格中选择内容
                    if (absStartIndex == firstCell.PrivateContent[0].ContentIndex
                        && absLength == firstCell.PrivateContent.Count)
                    {
                        // 若选择了单元格的全部内容，则认为是纯粹选择了该单元格
                        this._Mode = ContentRangeMode.Cell;
                    }
                    else
                    {
                        // 认为是文档内容选择模式
                        this._Mode = ContentRangeMode.Content;
                    }
                }
                else
                {
                    if (hasContentElement == true && cellCount > 0
                        || cellCount > 1)
                    {
                        // 出现跨单元格选择
                        DomTableElement table = firstCell.OwnerTable;
                        if (hasContentElement)
                        {
                            // 混合模式
                            this._Mode = ContentRangeMode.Mixed;
                        }
                        else
                        {
                            // 可能是纯粹的选择单元格,需要进一步判断
                            this._Mode = ContentRangeMode.Cell;
                            foreach (DomElement element4 in spanElements)
                            {
                                if (element4 is DomTableCellElement)
                                {
                                    var table2 = ((DomTableCellElement)element4).OwnerTable;
                                    if (table2 != table && table2.IsParentOrSupParent(table) == false)
                                    {
                                        if (table.IsParentOrSupParent(table2))
                                        {
                                            continue;
                                        }
                                        // 出现跨表格的选择，则修改为混合模式
                                        this._Mode = ContentRangeMode.Mixed;
                                        break;
                                    }
                                }
                            }
                        }//else 
                    }//if
                }

                if (this._Mode == ContentRangeMode.Content)
                {
                    // 选择纯文档内容
                    this._ContentElements = content.GetRange(absStartIndex, absLength);
                    this._StartIndex = startIndex;
                    this._Length = length;
                    UpdateStartEndElement();
                }
                else
                {
                    // 选择了单元格
                    this._ContentElements = new DomElementList();
                    DomTableElement table = firstCell.OwnerTable;
                    if (this._Mode == ContentRangeMode.Cell)
                    {
                        // 纯粹的在同一个表格中选择单元格，只选择开始单元格和结束单元格之间的矩形选择区域中的单元格
                        if (lastCell.OwnerTable != firstCell.OwnerTable)
                        {
                            if (firstCell.IsParentOrSupParent(lastCell.OwnerTable))
                            {
                                firstCell = lastCell;
                                table = lastCell.OwnerTable;
                            }
                            else
                            {
                                DomContainerElement p2 = lastCell.OwnerTable;
                                lastCell = firstCell;
                                while (p2 != null)
                                {
                                    if (p2 is DomTableCellElement)
                                    {
                                        if (((DomTableCellElement)p2).OwnerTable == firstCell.OwnerTable)
                                        {
                                            lastCell = (DomTableCellElement)p2;
                                            break;
                                        }
                                    }
                                    p2 = p2.Parent;
                                }
                            }
                        }
                        // 获得选中的单元格对象
                        this._Cells = table.GetSelectionCells(
                            firstCell.RowIndex,
                            firstCell.ColIndex,
                            lastCell.RowIndex,
                            lastCell.ColIndex);
                        foreach (DomTableCellElement cell in _Cells)
                        {
                            if (cell.IsOverrided == false)
                            {
                                MyAddRange(this._ContentElements, cell.PrivateContent, null, false);
                            }
                        }//foreach
                    }
                    else if (this._Mode == ContentRangeMode.Mixed)
                    {
                        // 混合模式下选择单元格，单元格是整行整行的选择
                        this._Cells = new DomElementList();
                        // 表格中的第一行
                        DomTableRowElement firstRow = null;
                        DomTableRowElement lastRow = null;
                        List<DomTableElement> handledTables = new List<DomTableElement>();
                        for (int iCount = 0; iCount < spanElements.Count; iCount++)
                        {
                            DomElement element4 = spanElements[iCount];
                            if (element4 is DomTableCellElement)
                            {
                                DomTableCellElement cell = (DomTableCellElement)element4;
                                if (firstRow == null)
                                {
                                    firstRow = cell.OwnerRow;
                                }
                                if (cell.OwnerTable == firstRow.OwnerTable)
                                {
                                    lastRow = cell.OwnerRow;
                                }
                                else
                                {
                                    // 选择表格中的单元格
                                    if (handledTables.Contains(firstRow.OwnerTable) == false)
                                    {
                                        handledTables.Add(firstRow.OwnerTable);
                                        DomElementList cs = firstRow.OwnerTable.GetSelectionCells(
                                            firstRow.Index,
                                            0,
                                            lastRow.Index,
                                            lastRow.Cells.Count - 1);
                                        foreach (DomTableCellElement cell4 in cs)
                                        {
                                            if (cell4.IsOverrided == false)
                                            {
                                                MyAddRange(this._ContentElements, cell4.PrivateContent, handledTables, false);
                                            }
                                        }
                                        this._Cells.AddRangeByDCList(cs);
                                        firstRow = cell.OwnerRow;
                                        lastRow = cell.OwnerRow;
                                    }
                                }
                            }
                            else
                            {
                                if (firstRow != null && handledTables.Contains(firstRow.OwnerTable) == false)
                                {
                                    // 选择表格中的单元格 
                                    handledTables.Add(firstRow.OwnerTable);
                                    DomElementList cs = firstRow.OwnerTable.GetSelectionCells(
                                        firstRow.Index,
                                        0,
                                        lastRow.Index,
                                        lastRow.Cells.Count - 1);
                                    foreach (DomTableCellElement cell4 in cs)
                                    {
                                        if (cell4.IsOverrided == false)
                                        {
                                            MyAddRange(this._ContentElements, cell4.PrivateContent, handledTables, false);

                                            //_ContentElements.AddRange(cell4.PrivateContent);
                                        }
                                    }
                                    this._Cells.AddRangeByDCList(cs);
                                    firstRow = null;
                                    lastRow = null;
                                }
                                this._ContentElements.FastAdd2(element4);
                            }
                        }//for
                        if (firstRow != null)
                        {
                            // 选择表格中的单元格
                            if (handledTables.Contains(firstRow.OwnerTable) == false)
                            {
                                handledTables.Add(firstRow.OwnerTable);
                                DomElementList cs = firstRow.OwnerTable.GetSelectionCells(
                                    firstRow.Index,
                                    0,
                                    lastRow.Index,
                                    lastRow.Cells.Count - 1);
                                foreach (DomTableCellElement cell4 in cs)
                                {
                                    if (cell4.IsOverrided == false)
                                    {
                                        MyAddRange(this._ContentElements, cell4.PrivateContent, handledTables, false);
                                        //_ContentElements.AddRange(cell4.PrivateContent);
                                    }
                                }
                                this._Cells.AddRangeByDCList(cs);
                            }
                        }
                    }
                }
                if (length == 0 || this._ContentElements.Count == 0)
                {
                    this._StartIndex = startIndex;
                    this._Length = length;
                    UpdateStartEndElement();

                }
                else
                {
                    if (length > 0)
                    {
                        this._StartIndex = this._ContentElements[0].ContentIndex;
                        this._Length = this._ContentElements.Count;
                        UpdateStartEndElement();
                        content.LineEndFlag = false;
                        var e9 = this._ContentElements.LastElement;
                        if ( this._Mode != ContentRangeMode.Cell && e9.OwnerLine != null)
                        {
                            var line9 = e9.OwnerLine;
                            if( line9.LayoutElements.LastElement == e9 )
                            {
                                content.LineEndFlag = true;
                            }
                        }
                    }
                    else
                    {
                        this._StartIndex = this._ContentElements.LastElement.ContentIndex + 1;
                        this._Length = 0 - this._ContentElements.Count;
                        UpdateStartEndElement();
                        if ( _DisableSetLineEndFlagInCellOnce == false && this._ContentElements.LastElement.OwnerCell != null)
                        {
                            //this.DocumentContent.Content.LineEndFlag = true;
                            this._LineEndFlag = true;
                        }
                        else
                        {
                            _DisableSetLineEndFlagInCellOnce = false ;
                            //System.Console.Write("");
                        }
                        if( this._Mode == ContentRangeMode.Cell)
                        {
                            this._LineEndFlag = false;
                            this.DocumentContent.Content.LineEndFlag = false;
                        }
                    }
                    this._LineEndFlag = content.LineEndFlag;
                }
            }//if


            //让选择状态发生改变的单元格声明用户界面无效，需要重新绘制
            RectangleFCounter invalidateRect = new RectangleFCounter();
            //float tickInvalidate = CountDown.GetTickCountFloat();
            if (oldCells != null && oldCells.Count > 0)
            {
                foreach (DomTableCellElement cell in oldCells)
                {
                    cell.IndexInSelectionCells = -1;
                    if (this._Cells == null || this._Cells.Contains(cell) == false)
                    {
                        RectangleF rect = document.GetViewBoundsForInvalidateElementView(cell);
                        if (rect.IsEmpty == false)
                        {
                            invalidateRect.Add(rect);
                        }
                        //cell.InvalidateView();
                    }
                }
            }
            if (this._Cells != null && _Cells.Count > 0)
            {
                int len = this._Cells.Count;
                for (var iCount = 0; iCount < len; iCount++)
                {
                    DomTableCellElement cell = (DomTableCellElement)this._Cells[iCount];
                    if (ContainsCell(oldCells, cell) == false)
                    {
                        RectangleF rect = document.GetViewBoundsForInvalidateElementView(cell);
                        if (rect.IsEmpty == false)
                        {
                            invalidateRect.Add(rect);
                        }
                        //cell.InvalidateView();
                    }
                    cell.IndexInSelectionCells = iCount;
                }
            }
            if (this.Mode == ContentRangeMode.Content || this.Mode == ContentRangeMode.Mixed)
            {
                int viewStartIndex = -1;
                int viewEndIndex = -1;
                if( contentElementsBack != null && contentElementsBack.Count > 0 )
                {
                    viewStartIndex = contentElementsBack[0].ContentIndex;
                    viewEndIndex = contentElementsBack.LastElement.ContentIndex ;
                }
                if (this._ContentElements != null && this._ContentElements.Count > 0)
                {
                    if (viewStartIndex == -1 || viewStartIndex > this._ContentElements[0].ContentIndex)
                    {
                        viewStartIndex = this._ContentElements[0].ContentIndex;
                    }
                    if (viewEndIndex == -1 || viewEndIndex < this._ContentElements.LastElement.ContentIndex)
                    {
                        viewEndIndex = this._ContentElements.LastElement.ContentIndex ;
                    }
                }
                if(viewStartIndex >= 0 && viewStartIndex >= content.Count )
                {
                    viewStartIndex = content.Count - 1;
                }
                if( viewEndIndex >=0 && viewEndIndex >= content.Count )
                {
                    viewEndIndex = content.Count - 1;
                }
                if (viewStartIndex >= 0 && viewEndIndex >= viewStartIndex)
                {
                    DomLine currentLine = null;
                    for (var iCount = viewStartIndex; iCount <= viewEndIndex; iCount++)
                    {
                        var element = content[iCount];
                        if (element is DomCharElement
                            || element is DomParagraphFlagElement
                            || element is DomFieldBorderElement)
                        {
                            var line = element._OwnerLine;
                            if (currentLine == null || currentLine != line)
                            {
                                currentLine = line;
                                if (currentLine != null)
                                {
                                    invalidateRect.Add(currentLine.AbsBounds);
                                }
                            }
                        }
                        else
                        {
                            var rect = document.GetViewBoundsForInvalidateElementView(element);
                            if (rect.IsEmpty == false)
                            {
                                invalidateRect.Add(rect);
                            }
                        }
                    }
                }
                else
                {
                    // 让选择状态发生改变的文档内容元素用户界面无效，需要重新绘制
                    if (contentElementsBack != null && contentElementsBack.Count > 0)
                    {
                        var _ContentElementsFirstElement = this._ContentElements?.FirstElement;
                        var _ContentElementsLastElement = this._ContentElements?.LastElement;
                        foreach (DomElement element in contentElementsBack.FastForEach())
                        {
                            if (this._ContentElements.Contains(element) == false
                                || _ContentElementsFirstElement == element
                                || _ContentElementsLastElement == element)
                            {
                                RectangleF rect = document.GetViewBoundsForInvalidateElementView(element);
                                if (rect.IsEmpty == false)
                                {
                                    invalidateRect.Add(rect);
                                }
                                //element.InvalidateView();
                            }
                        }
                    }
                    if (this._ContentElements.Count > 0)
                    {
                        var contentElementsBackFirstElement = contentElementsBack?.FirstElement;
                        var contentElementsBackLastElement = contentElementsBack?.LastElement;
                        foreach (DomElement element in this._ContentElements.FastForEach())
                        {
                            if (contentElementsBack == null
                                || contentElementsBack.Contains(element) == false
                                || contentElementsBackFirstElement == element
                                || contentElementsBackLastElement == element)
                            {
                                RectangleF rect = document.GetViewBoundsForInvalidateElementView(element);
                                if (rect.IsEmpty == false)
                                {
                                    invalidateRect.Add(rect);
                                }
                                //element.InvalidateView();
                            }
                        }
                    }
                }
            }
            //tickInvalidate = CountDown.GetTickCountFloat() - tickInvalidate;
            if (invalidateRect.IsEmpty == false && document.EditorControl != null)
            {
                RectangleF rect = invalidateRect.Value;
                //if (rect.Top <= 0 
                //    && (this.DocumentContent.ContentPartyStyle == PageContentPartyStyle.Body 
                //    || this.DocumentContent.ContentPartyStyle == PageContentPartyStyle.HeaderRows ))
                //{
                //    rect.Y = 1;
                //    rect.Height -= 1;
                //}
                document.EditorControl.ViewInvalidate(
                    rect,
                    this.DocumentContent.ContentPartyStyle);
            }
            //if (document.InnerViewControl() != null
            //    && document.InnerViewControl() != null
            //    && document.InnerViewControl().ViewHandleManager() != null)
            //{
            //    this.Document.EditorControl.InnerViewControl.ViewHandleManager.ClearLastDrawItems();
            //}
            //if (this.StartIndex == -1)
            //{
            //}
            document.ClearCachedOptions();
            return true;
        }
        private void MyAddRange(
            DomElementList list,
            DomElementList content,
            List<DomTableElement> handledTables,
            bool checkContains)
        {
            var contentCount = content.Count;
            bool isAllChar = true;
            for (int iCount = 0; iCount < contentCount; iCount++)
            {
                var element = content[iCount];
                if (element is DomCharElement
                    || element is DomParagraphFlagElement
                    || element is DomFieldBorderElement)
                {

                }
                else
                {
                    isAllChar = false;
                    if (iCount > 0)
                    {
                        list.AddRangeRaw2(new SubList<DomElement>(content, 0, iCount));
                    }
                    for (int iCount2 = iCount; iCount2 < contentCount; iCount2++)
                    {
                        element = content[iCount2];
                        if (element is DomCharElement
                            || element is DomParagraphFlagElement
                            || element is DomFieldBorderElement)
                        {
                            list.FastAdd2(element);
                        }
                        else
                        {
                            if (element is DomTableElement)
                            {
                                DomTableElement table = (DomTableElement)element;
                                if (handledTables == null || handledTables.Contains(table) == false)
                                {
                                    if (handledTables != null)
                                    {
                                        handledTables.Add(table);
                                    }
                                    foreach (DomTableCellElement cell in table.Cells)
                                    {
                                        if (cell.IsOverrided == false)
                                        {
                                            MyAddRange(list, cell.PrivateContent, handledTables, checkContains);
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (checkContains)
                                {
                                    if (list.Contains(element))
                                    {
                                        // 已经添加过内容了，立即退出函数
                                        return;
                                    }
                                }
                                list.FastAdd2(element);
                            }
                        }
                    }
                    break;
                }
            }
            if (isAllChar)
            {
                // 全部为字符类型元素，则整体添加
                list.AddRangeByDCList(content);
            }
        }

        internal static bool _DisableSetLineEndFlagInCellOnce = false;

        public bool ContainsContentElement(DomElement element)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }
            if (this._ContentElements.Count == 0)
            {
                return false;
            }
            if (this._Mode == ContentRangeMode.Content)
            {
                int index = element.ContentIndex - this._ContentElements[0].ContentIndex;
                if (index >= 0
                    && index < this._ContentElements.Count
                    && this._ContentElements[index] == element)
                {
                    return true;
                }
                return false;
            }
            else if (this._Mode == ContentRangeMode.Cell)
            {
                DomElement p = element;
                while (p != null)
                {
                    if (p is DomTableCellElement)
                    {
                        if (this.ContainsCell((DomTableCellElement)p))// ._Cells.Contains(p))
                        {
                            return true;
                        }
                    }
                    p = p.Parent;
                }
                return false;
            }
            else if (this._Mode == ContentRangeMode.Mixed)
            {
                DomElement p = element;
                while (p != null)
                {
                    if (p is DomTableCellElement)
                    {
                        if (this.ContainsCell((DomTableCellElement)p))// this._Cells.Contains(p))
                        {
                            return true;
                        }
                    }
                    p = p.Parent;
                }
                return this._ContentElements.Contains(element);
            }
            return false;
        }
        /// <summary>
        /// 获得元素枚举器
        /// </summary>
        /// <returns></returns>
        public IEnumerator GetEnumerator()
        {
            if (_ContentElements == null)
            {
                return null;
            }
            else
            {
                return _ContentElements.GetEnumerator();
            }
        }


        /// <summary>
        /// 获得纯文本内容
        /// </summary>
        public string Text
        {
            get
            {
                if (_ContentElements == null)
                {
                    return string.Empty;
                }
                else
                {
                    var includeBKText = this.Document.Options.BehaviorOptions.SelectionTextIncludeBackgroundText;
                    System.Text.StringBuilder myStr = new System.Text.StringBuilder();
                    if (this.Mode == ContentRangeMode.Cell)
                    {
                        DomTableRowElement lastRow = null;
                        foreach (DomTableCellElement cell in this.Cells)
                        {
                            if (cell.IsOverrided)
                            {
                                continue;
                            }
                            if (lastRow != null && cell.OwnerRow != lastRow)
                            {
                                myStr.AppendLine();
                            }
                            if (cell.OwnerRow == lastRow)
                            {
                                myStr.Append("   ");
                            }
                            var args2 = new DomElement.WriteTextArgs( this.Document, true, false);
                            args2.IncludeBackgroundText = includeBKText;
                            cell.WriteText(args2);
                            myStr.Append(args2.ToString());
                            lastRow = cell.OwnerRow;
                        }
                    }
                    else
                    {
                        int lastParagraphIndex = 0;
                        //var renderMode = (InnerDocumentRenderMode)this.Document.States.RenderMode;
                        foreach (DomElement element in _ContentElements)
                        {
                            if (element is DomParagraphFlagElement pef)
                            {
                                //XTextParagraphFlagElement pef = (XTextParagraphFlagElement)element;
                                if (pef.ListItemElement != null)
                                {
                                    myStr.Insert(lastParagraphIndex, pef.ListItemElement.Text);
                                }
                            }
                            if (element is DomCharElement ce)
                            {
                                //var ce = (XTextCharElement)element;
                                if (ce.IsBackgroundText)
                                {
                                    if( includeBKText == false )
                                    {
                                        continue;
                                    }
                                    //var f = (XTextFieldElementBase)element.Parent;
                                    //if (f.CanDrawBackgroundText(renderMode) == false)
                                    //{
                                    //    continue;
                                    //}
                                }
                                //if (element.Parent is XTextFieldElementBase)
                                //{
                                //    var f = (XTextFieldElementBase)element.Parent;
                                //    if (f.IsBackgroundTextElement(element))
                                //    {
                                //        if (f.CanDrawBackgroundText((InnerDocumentRenderMode)this.Document.States.RenderMode) == false)
                                //        {
                                //            continue;
                                //        }
                                //    }
                                //}
                                ce.AppendContent(myStr);
                            }
                            else if (element is DomFieldBorderElement fb)
                            {
                            }
                            else
                            {
                                myStr.Append(element.ToPlaintString());
                                if (element is DomParagraphFlagElement)
                                {
                                    lastParagraphIndex = myStr.Length;
                                }
                            }
                        }
                    }
                    return myStr.ToString();
                }
            }
        }

        /// <summary>
        /// 获得表示内容的XML文本
        /// </summary>
        public string XMLText
        {
            get
            {
                return GetContentText(WriterConst.Format_XML);
            }
        }

        /// <summary>
        /// 获得指定格式的表示对象内容的文本值
        /// </summary>
        /// <param name="format">文件格式</param>
        /// <returns>文本值</returns>
        public string GetContentText(string format)
        {
            DomDocument document = this.CreateDocument();
            System.IO.StringWriter writer = new System.IO.StringWriter();
            document.Save(writer, format);
            document.Dispose();
            string txt = writer.ToString();
            return txt;
        }
        /// <summary>
        /// 设置段落样式
        /// </summary>
        /// <param name="newStyle"></param>
        /// <returns></returns>
        public DomElementList SetParagraphStyle(DocumentContentStyle newStyle)
        {
            DomDocument document = this._DocumentContent.OwnerDocument;
            Dictionary<DomElement, int> styleIndexs
                = new Dictionary<DomElement, int>();
                newStyle.DisableDefaultValue = true;
            foreach (DomParagraphFlagElement p in this.ParagraphsEOFs)
            {
                if (document.DocumentControler.CanModify(p))
                {
                    DocumentContentStyle rs = (DocumentContentStyle)p.RuntimeStyle.CloneParent();
                    if (XDependencyObject.MergeValues(newStyle, rs, true) > 0)
                    {
                        //rs.DefaultValuePropertyNames = newStyle.GetDefaultValuePropertyNames2( rs.DefaultValuePropertyNames );
                        int newStyleIndex = document.ContentStyles.GetStyleIndex(rs);
                        if (newStyleIndex != p.StyleIndex)
                        {
                            styleIndexs[p] = newStyleIndex;
                        }
                    }
                }
            }//foreach
            if (styleIndexs.Count > 0)
            {
                DomElementList result = document.EditorSetParagraphStyle(styleIndexs, true);
                return result;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 设置文档元素的样式
        /// </summary>
        /// <param name="newStyle">新样式</param>
        /// <param name="causeUpdateLayout">操作是否导致刷新文档布局</param>
        /// <param name="includeCells">包含单元格</param>
        /// <param name="commandName">命令名称</param>
        /// <returns>是否修改了文档内容</returns>
        public bool SetElementStyle(
            DocumentContentStyle newStyle,
            bool causeUpdateLayout,
            bool includeCells,
            string commandName)
        {
            DomElementList list = new DomElementList();
            if (this.ContentElements != null)
            {
                list.AddRangeByDCList(this.ContentElements);
            }
            if (this.Content._SelectAllFlag && list.Contains(this.Content.LastElement) == false)
            {
                list.Add(this.Content.LastElement);
            }
            if (includeCells)
            {
                if (this.Cells != null)
                {
                    list.AddRangeByDCList(this.Cells);
                }
            }
            for (int iCount = list.Count - 1; iCount >= 0; iCount--)
            {
                DomElement element = list[iCount];
                if (element is DomFieldBorderElement)
                {
                    DomFieldElementBase field = (DomFieldElementBase)element.Parent;
                    if (list.Contains(field) == false)
                    {
                        list.Insert(iCount, field);
                    }
                }
            }
            if (list.Count == 0)
            {
                return false;
            }

            return SetElementStyle(
                newStyle,
                newStyle,
                newStyle,
                this._DocumentContent.OwnerDocument,
                list,
                causeUpdateLayout,
                commandName,
                true);
        }
        /// <summary>
        /// 设置多个元素的样式
        /// </summary>
        /// <param name="newContentStyle">新样式</param>
        /// <param name="newCellStyle">新的单元格样式</param>
        /// <param name="newParagraphStyle">新的段落采用的样式</param>
        /// <param name="document">文档对象</param>
        /// <param name="elements">要设置的元素列表</param>
        /// <param name="causeUpdateLayout">操作是否导致刷新文档布局</param>
        /// <param name="commandName">调用本方法的命令名称</param>
        /// <param name="logUndo">是否记录撤销操作信息</param>
        /// <returns>操作是否成功</returns>

        public static bool SetElementStyle(
            DocumentContentStyle newContentStyle,
            DocumentContentStyle newParagraphStyle,
            DocumentContentStyle newCellStyle,
            DomDocument document,
            System.Collections.IEnumerable elements,
            bool causeUpdateLayout,
            string commandName,
            bool logUndo)
        {
            if (newContentStyle == null)
            {
                throw new ArgumentNullException("newStyle");
            }
            if (document == null)
            {
                throw new ArgumentNullException("document");
            }
            if (elements == null)
            {
                throw new ArgumentNullException("elements");
            }
            if (newParagraphStyle == null)
            {
                newParagraphStyle = newContentStyle;
            }
            if (newCellStyle == null)
            {
                newCellStyle = newContentStyle;
            }
            newContentStyle.ValueLocked = false;
            Dictionary<DomElement, int> newStyleIndexs
                = new Dictionary<DomElement, int>();
            DomElementList parents = new DomElementList();
            //XTextElement lastParent = null;

            foreach (DomElement element in elements)
            {
                //XTextElement parent = element.Parent;
                //if (parent != lastParent)
                {
                    // 记录所有涉及到的父元素
                    //lastParent = parent;
                    bool addParent = false;
                    //if (element is XTextFieldBorderElement)
                    //{
                    //    addParent = true;
                    //}
                    //else 
                    if (element.Parent is DomFieldElementBase field)
                    {
                        //XTextInputFieldElementBase field =
                        //    (XTextInputFieldElementBase)element.Parent;
                        if (field.IsBackgroundTextElement(element)
                            || element is DomFieldBorderElement)
                        {
                            // 若处理的元素是输入域的背景元素或边框元素则处理输入域元素
                            addParent = true;
                        }
                    }
                    if (addParent)
                    {
                        if (parents.Contains(element.Parent) == false)
                        {
                            parents.Add(element.Parent);
                        }
                    }
                }//if
                // 获得新的样式
                DocumentContentStyle ns = newContentStyle;
                if (element is DomParagraphFlagElement)
                {
                    ns = newParagraphStyle;
                }
                else if (element is DomTableCellElement)
                {
                    ns = newCellStyle;
                }
                if (ns == null)
                {
                    ns = newContentStyle;
                }
                bool styleChanged = false;
                DocumentContentStyle rs = (DocumentContentStyle)element.RuntimeStyle.CloneParent();
                if (commandName == StandardCommandNames.ClearFormat || commandName == StandardCommandNames.FormatBrush)
                {
                    styleChanged = element.StyleIndex != document.ContentStyles.GetStyleIndex(ns);
                    if (styleChanged)
                    {
                        rs = (DocumentContentStyle)ns.Clone();
                    }
                }
                else
                {
                    if (XDependencyObject.MergeValues(ns, rs, true) > 0)
                    {
                        //rs.DefaultValuePropertyNames = ns.GetDefaultValuePropertyNames2( rs.DefaultValuePropertyNames );
                        styleChanged = true;
                    }
                }
                if (styleChanged)
                {
                    int styleIndex = document.ContentStyles.GetStyleIndex(rs);
                    if (styleIndex != element.StyleIndex)
                    {
                        newStyleIndexs[element] = styleIndex;
                    }
                }
            }//foreach

            if (parents.Count > 0)
            {
                // 对涉及到的父元素设置样式
                foreach (DomElement element in parents)
                {
                    DocumentContentStyle rs = (DocumentContentStyle)element.RuntimeStyle.CloneParent();
                    if (XDependencyObject.MergeValues(newContentStyle, rs, true) > 0)
                    {
                        //rs.DefaultValuePropertyNames = newContentStyle.GetDefaultValuePropertyNames( rs.DefaultValuePropertyNames );
                        int styleIndex = document.ContentStyles.GetStyleIndex(rs);
                        if (styleIndex != element.StyleIndex)
                        {
                            newStyleIndexs[element] = styleIndex;
                        }
                    }
                }
            }
            if (newStyleIndexs.Count > 0)
            {
                DomElementList result = document.EditorSetElementStyle(
                    newStyleIndexs,
                    logUndo,
                    causeUpdateLayout,
                    commandName);
                if (result != null && result.Count > 0)
                {
                    return true;
                }
            }
            return false;
        }

        private DomElementList _Cached_ParagraphsEOFs = new DomElementList ();
        /// <summary>
        /// 获得区间包含的段落对象列表
        /// </summary>
        public DomElementList ParagraphsEOFs
        {
            get
            {
                var list = _Cached_ParagraphsEOFs;
                if (list == null)
                {
                    list = new DomElementList();
                    _Cached_ParagraphsEOFs = list;
                }
                if (this.Length != 0)
                {
                    foreach (DomElement element in this._ContentElements)
                    {
                        if (element is DomParagraphFlagElement)
                        {
                            list.Add(element);
                        }
                    }
                    if ((this._ContentElements.LastElement is DomParagraphFlagElement) == false)
                    {
                        var pe = this._ContentElements.LastElement.OwnerParagraphEOF;
                        if (pe != null)
                        {
                            list.Add(pe);
                        }
                    }
                }
                else
                {
                    DomElement element = this.Content.SafeGet(this.StartIndex);
                    if (element != null)
                    {
                        var pf = element.OwnerParagraphEOF;
                        if (pf != null)
                        {
                            list.Add(pf);
                        }
                    }
                }
                return list;
            }
        }


        /// <summary>
        /// 根据内容创建一个新的文档对象
        /// </summary>
        /// <returns>创建的文档对象</returns>
        public DomDocument CreateDocument()
        {

            DomDocument document = (DomDocument)this.Document.Clone(false);
            document.Info = new DocumentInfo();
            document.ContentStyles = (DocumentContentStyleContainer)this.Document.ContentStyles.Clone();
            //XTextDocument sourceDocument = this.Document;
            DomContainerElement body = document.Body;
            if (this.ContentElements == null || this.ContentElements.Count == 0)
            {
                return document;
            }
            body.Elements.Clear();
            // 寻找所有内容共同的文档容器元素
            DomElementList parents1 = WriterUtilsInner.GetParentList(this.ContentElements.FirstElement);
            DomElementList parents2 = WriterUtilsInner.GetParentList(this.ContentElements.LastElement);
            foreach (DomContainerElement parent in parents1)
            {
                if (parents2.Contains(parent))
                {
                    if (parent is DomTableRowElement || parent is DomTableElement)
                    {
                        continue;
                    }
                    //XTextContentElement ce = parent.ContentElement;
                    CloneElements(parent, ref body);
                    break;
                }
            }
            if (this.Document.GetDocumentEditOptions().CloneWithoutElementBorderStyle)
            {
                // 去掉字符，图片等文档元素边框设置,但不处理表格
                foreach (DomElement element in document.GetElementsByType<DomElement>())
                {
                    if (element is DomCharElement
                        || element is DomObjectElement
                        || element is DomFieldElementBase
                        || element is DomParagraphFlagElement)
                    {
                        element.OwnerDocument = document;
                        element.Style.RemoveBorderValues();
                        //element.Style.RemovePropertyValue("BorderStyle");
                        //element.Style.RemovePropertyValue("BorderWidth");
                        //element.Style.RemovePropertyValue("BorderTop");
                        //element.Style.RemovePropertyValue("BorderLeft");
                        //element.Style.RemovePropertyValue("BorderRight");
                        //element.Style.RemovePropertyValue("BorderBottom");
                        //element.Style.RemovePropertyValue("BorderTopColor");
                        //element.Style.RemovePropertyValue("BorderLeftColor");
                        //element.Style.RemovePropertyValue("BorderRightColor");
                        //element.Style.RemovePropertyValue("BorderBottomColor");
                        //element.Style.RemovePropertyValue("RoundRadio");
                    }
                }
            }
            // 删除没有引用的样式
            document.CompressStyleList();
            document.EditorControl = null;
            document.DocumentControler = null;

            document.CurrentStyleInfo = null;
            document.HoverElement = null;
            document.HighlightManager = null;
            if (document.UndoList != null)
            {
                document.EndLogUndo();
                document.UndoList.Clear();
            }
            document.FixDomState();
            return document;
        }

        ///// <summary>
        ///// 处理刚刚复制的元素
        ///// </summary>
        ///// <param name="eventSender">事件发起者</param>
        ///// <param name="args">参数</param>
        //private void FixCloneElement(object eventSender, ElementEnumerateEventArgs args)
        //{
        //    if (args.Element is XTextTableElement)
        //    {
        //        XTextTableElement table = (XTextTableElement)args.Element;
        //        table.UpdateCellOverrideState();
        //        args.CancelChild = true;
        //    }
        //}
        private int CloneElements(
            DomContainerElement sourceContainer,
            ref DomContainerElement descContainer)
        {
            int result = 0;
            if (sourceContainer is DomFieldElementBase)
            {
                DomFieldElementBase field = (DomFieldElementBase)sourceContainer;
                if (this.ContentElements.Contains(field.StartElement)
                    && this.ContentElements.Contains(field.EndElement))
                {
                    // 文档域的开始元素和结束元素被选中了，则可以认为文档域被完全的选中了
                    // 此时复制是需要完整的复制。
                    DomContainerElement nc = (DomContainerElement)sourceContainer.Clone(true);
                    if (descContainer == null)
                    {
                        descContainer = nc;
                    }
                    else
                    {
                        descContainer.AppendChildElement(nc);
                    }
                    return 1;
                }
            }
            Dictionary<DomContentElement, DomElement> lastCopySourceElements = new Dictionary<DomContentElement, DomElement>();
            foreach (DomElement element in sourceContainer.Elements)
            {
                if (this.ContentElements.Contains(element) || this.Cells.Contains(element))
                {
                    if (descContainer == null)
                    {
                        descContainer = (DomContainerElement)sourceContainer.Clone(false);
                        descContainer.SetOwnerLine(null);
                        result++;
                    }
                    result++;
                    var newElement = element.Clone(true);
                    descContainer.AppendChildElement(newElement);
                    if (descContainer is DomContentElement)
                    {
                        // 记录对每一个文本内容元素复制的最后一个原始文档元素对象
                        lastCopySourceElements[(DomContentElement)descContainer] = element;
                    }
                }
                else if (element is DomContainerElement)
                {
                    DomContainerElement oldC = (DomContainerElement)element;
                    DomContainerElement newC = null;
                    int result2 = CloneElements(oldC, ref newC);
                    if (result2 == 0)
                    {
                        if (element is DomContainerElement)
                        {
                            DomContainerElement container = (DomContainerElement)element;
                            if (oldC.Elements.Count == 0)
                            {
                                // 有些特殊的容器元素没有标准意义上的子元素,例如设置了背景文本的没有实际
                                // 内容的输入域等,此时需要判断文档内容元素
                                DomElementList ces = new DomElementList();
                                container.AppendViewContentElement(new DomContainerElement.AppendViewContentElementArgs(container.OwnerDocument, ces, true));
                                foreach (DomElement ce in ces)
                                {
                                    if (this.ContentElements.Contains(ce))
                                    {
                                        if (newC == null)
                                        {
                                            // 发现有内容在包含区域中,则创建新的容器元素对象
                                            newC = (DomContainerElement)oldC.Clone(false);
                                            newC.SetOwnerLine(null);
                                            break;
                                        }
                                    }
                                }//foreach
                            }//if
                        }//if
                    }//if
                    if (newC != null)
                    {
                        if (descContainer == null)
                        {
                            result++;
                            descContainer = (DomContainerElement)sourceContainer.Clone(false);
                            descContainer.SetOwnerLine(null);
                            if (sourceContainer is DomTableElement && oldC is DomTableRowElement)
                            {
                                var oldTable = (DomTableElement)sourceContainer;
                                var newTable = (DomTableElement)descContainer;
                                var startColumnIndex = int.MaxValue;
                                var endColumnIndex = -1;
                                foreach (DomTableCellElement cell4 in this._Cells)
                                {
                                    if (cell4.OwnerTable == oldTable)
                                    {
                                        var cell5 = cell4.OverrideCell == null ? cell4 : cell4.OverrideCell;
                                        if (cell5.ColIndex < startColumnIndex)
                                        {
                                            startColumnIndex = cell5.ColIndex;
                                        }
                                        if (cell5.ColIndex + cell5.ColSpan > endColumnIndex)
                                        {
                                            endColumnIndex = cell5.ColIndex + cell5.ColSpan;
                                        }
                                    }
                                }
                                if (startColumnIndex < endColumnIndex && endColumnIndex <= oldTable.Columns.Count)
                                {
                                    newTable.Columns.Clear();
                                    for (var colIndex = startColumnIndex; colIndex < endColumnIndex; colIndex++)
                                    {
                                        newTable.Columns.Add(oldTable.Columns[colIndex].Clone(false));
                                    }
                                }
                            }
                        }
                        descContainer.AppendChildElement(newC);
                    }
                }
            }//foreach
            if (lastCopySourceElements.Count > 0)
            {
                foreach (DomContentElement ce in lastCopySourceElements.Keys)
                {
                    DomElement lastE = lastCopySourceElements[ce];
                    if ((lastE is DomParagraphFlagElement) == false)
                    {
                        // 获得最后一个原始文档原始对象对应的段落符号对象
                        DomParagraphFlagElement p = lastE.OwnerParagraphEOF;
                        if (p != null && p.AutoCreate == false && p.StyleIndex >= 0)
                        {
                            // 如果段落符号对象不是自动创建的而且使用了自定义的样式，则也随着复制。
                            ce.AppendChildElement(p.Clone(true));
                        }
                    }
                }
            }
            if (descContainer != null && descContainer is DomTableElement)
            {
                // 为复制的表格对象补充表格列
                DomTableElement table = (DomTableElement)descContainer;
                DomTableElement sourceTable = (DomTableElement)sourceContainer;
                int startColIndex = 10000;
                int endColIndex = 0;
                foreach (DomTableCellElement cell in this.Cells)
                {
                    if (cell.OwnerTable == sourceTable
                        && cell.IsOverrided == false)
                    {
                        if (cell.ColIndex < startColIndex)
                        {
                            startColIndex = cell.ColIndex;
                        }
                        if (cell.ColIndex + cell.ColSpan > endColIndex)
                        {
                            endColIndex = cell.ColIndex + cell.ColSpan;
                        }
                    }
                }//foreach

                endColIndex = Math.Min(endColIndex, sourceTable.Columns.Count);
                for (int iCount = startColIndex; iCount < endColIndex; iCount++)
                {
                    DomTableColumnElement newCol = (DomTableColumnElement)sourceTable.Columns[iCount].Clone(false);
                    newCol.Parent = table;
                    table.Columns.Add(newCol);
                }
                //foreach (XTextTableRowElement row in table.Rows)
                //{
                //    if (table.Columns.Count < row.Cells.Count)
                //    {
                //        for (int iCount = table.Columns.Count; iCount < row.Cells.Count; iCount++)
                //        {
                //            XTextTableColumnElement newCol = table.CreateColumnInstance();
                //            XTextTableCellElement cell = ( XTextTableCellElement ) row.Cells[iCount];
                //            newCol.Width =  cell.Width;
                //            table.Columns.Add(newCol);
                //        }
                //    }
                //}//foreach
                table.FixCells();
                table.UpdateCellOverrideState();
            }
            return result;
        }

        public void Dispose()
        {
            if (this._Cached_ParagraphsEOFs != null)
            {
                this._Cached_ParagraphsEOFs.ClearAndEmpty();
                this._Cached_ParagraphsEOFs = null;
            }
            if (this._Cells != null)
            {
                this._Cells.Clear();
                this._Cells = null;
            }
            if (this._ContentElements != null)
            {
                this._ContentElements.ClearAndEmpty();
                this._ContentElements = null;
            }
            if (this._SelectionParagraphFlags != null)
            {
                this._SelectionParagraphFlags.ClearAndEmpty();
                this._SelectionParagraphFlags = null;
            }
            if (this._Ranges != null)
            {
                this._Ranges.Clear();
                this._Ranges = null;
            }
            this._DocumentContent = null;
            this._StartElement = null;
            this._EndElement = null;
        }
         

        /// <summary>
        /// 保存状态
        /// </summary>
        /// <returns>状态信息对象</returns>
        public object SaveState()
        {
            SelectionState state = new SelectionState();
            state.Selection = this;
            state.StartElement = this._StartElement;
            state.EndElement = this._EndElement;
            return state;
        }

        /// <summary>
        /// 恢复状态
        /// </summary>
        /// <param name="obj">状态对象</param>
        /// <returns>操作是否成功</returns>
        public bool RestoreState(object obj)
        {
            SelectionState state = obj as SelectionState;
            if (state == null
                || state.Selection != this
                || state.StartElement == null
                || state.EndElement == null)
            {
                if (this._StartIndex < 0 || this._StartIndex + this._Length < 0)
                {
                    this._StartIndex = 0;
                    this._Length = 0;
                    this._NativeStartIndex = this._StartIndex;
                    this._NativeLength = this._Length;
                }
                return false;
            }

            int si = this.Content.FastIndexOf(state.StartElement);
            int ei = this.Content.FastIndexOf(state.EndElement);
            if (si >= 0 && ei >= 0)
            {
                this._StartIndex = si;
                this._Length = ei - si;
                this._NativeStartIndex = this._StartIndex;
                this._NativeLength = this._Length;
                return true;
            }
            return false;
        }

        /// <summary>
        /// 状态对象
        /// </summary>
        private class SelectionState
        {
            public DCSelection Selection = null;
            public DomElement StartElement = null;
            public DomElement EndElement = null;
        }
    }

    /// <summary>
    /// 文档区域对象列表
    /// </summary>
    public sealed class XTextContentRangeList
    {
        internal XTextContentRangeList(DomElementList list)
        {
            int len = list.Count;
            int startIndex = list[0].ContentIndex;
            int lastIndex = startIndex;
            for (int iCount = 1; iCount < len; iCount++)
            {
                int index = list[iCount].ContentIndex;
                if (index > lastIndex + 1)
                {
                    // 不是连续的编号，发生断裂，则添加区域信息
                    this._Values.Add(new XTextContentRange(startIndex, lastIndex - startIndex));
                    startIndex = index;
                    lastIndex = startIndex;
                }
                lastIndex++;
            }
            if (lastIndex > startIndex)
            {
                this._Values.Add(new XTextContentRange(startIndex, lastIndex - startIndex));
            }
        }

        internal void Clear()
        {
            if (this._Values != null)
            {
                this._Values.Clear();
                this._Values = null;
            }
        }

        private List<XTextContentRange> _Values = new List<XTextContentRange>();
        /// <summary>
        /// 判断区域列表是否和制定的区域有交集
        /// </summary>
        /// <param name="startIndex">判断的区域的开始编号</param>
        /// <param name="endIndex">判断的区域的结束编号</param>
        /// <returns>是否有交集</returns>
        public bool IntersectsWith(int startIndex, int endIndex)
        {
            if (startIndex > endIndex)
            {
                int temp = endIndex;
                endIndex = startIndex;
                startIndex = temp;
            }
            foreach (var item in this._Values)
            {
                if (startIndex <= item._EndIndex && endIndex >= item._StartIndex)
                {
                    return true;
                }
            }
            return false;
        }
    }
    /// <summary>
    /// 文档区域对象
    /// </summary>
    public sealed class XTextContentRange
    {
        internal XTextContentRange(int startIndex, int length)
        {
            this._StartIndex = startIndex;
            this._EndIndex = startIndex + length;
            this._Length = length;
        }

        internal int _StartIndex = 0;
        /// <summary>
        /// 区域开始编号
        /// </summary>
        public int StartIndex
        {
            get
            {
                return this._StartIndex;
            }
        }
        internal int _Length = 0;
        /// <summary>
        /// 区域长度
        /// </summary>
        public int Length
        {
            get
            {
                return this._Length;
            }
        }
        internal int _EndIndex = 0;
        /// <summary>
        /// 结束编号
        /// </summary>
        public int EndIndex
        {
            get
            {
                return this._EndIndex;
            }
        }
#if !RELEASE
        public override string ToString()
        {
            return this.StartIndex + "->" + this.EndIndex + "  Length:" + this.Length;
        }
#endif
    }

    /// <summary>
    /// 内容区域样式
    /// </summary>
    [System.Reflection.Obfuscation( Exclude = true , ApplyToMembers = true )]
    public enum ContentRangeMode
    {
        /// <summary>
        /// 文档内容
        /// </summary>
        Content,
        /// <summary>
        /// 纯表格单元格
        /// </summary>
        Cell,
        /// <summary>
        /// 混合模式，包括文档内容和表格单元格
        /// </summary>
        Mixed
    }


}