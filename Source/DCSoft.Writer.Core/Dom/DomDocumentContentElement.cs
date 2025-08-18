using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using DCSoft.Printing;
using DCSoft.Drawing;
using System.Runtime.InteropServices;
using DCSoft.Common;
using DCSoft.Writer.Controls;

namespace DCSoft.Writer.Dom
{
    /// <summary>
    /// 文档级内容对象
    /// </summary>
    [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = false)]
    public abstract class DomDocumentContentElement : DomContentElement
    {
        /// <summary>
        /// 初始化对象
        /// </summary>
        protected DomDocumentContentElement()
        {
        }

        /// <summary>
        /// 加载文档时预估的内容元素个数
        /// </summary>
        internal int _ContentCountForLoadDocument = 0;

        /// <summary>
        /// 元素所属的文档级内容元素对象
        /// </summary>         
        public override DomDocumentContentElement DocumentContentElement
        {
            get
            {
                return this;
            }
        }
        /// <summary>
        /// 无条件接受Tab字符
        /// </summary>
        public override bool AcceptTab
        {
            get
            {
                return true;
            }
            set
            {
            }
        }

        /// <summary>
        /// 段落树状列表的根段落对象
        /// </summary>
        private DomParagraphFlagElement _RootParagraphFlag = null;
        /// <summary>
        /// DCWriter内部使用。段落树状列表的根段落对象
        /// </summary>
        public DomParagraphFlagElement RootParagraphFlag()
        {
            return _RootParagraphFlag;
        }

        /// <summary>
        /// 游离的段落列表组
        /// </summary>
        private List<List<DomParagraphFlagElement>> _FreeParagraphFlagGroups = null;
        /// <summary>
        /// 游离的段落列表组
        /// </summary>
        public List<List<DomParagraphFlagElement>> FreeParagraphFlagGroups
        {
            get
            {
                return _FreeParagraphFlagGroups;
            }
            set
            {
                _FreeParagraphFlagGroups = value;
            }
        }

        private bool _ParagraphTreeInvalidte = true;
        /// <summary>
        /// 段落树状结构无效标记
        /// </summary>
        public bool ParagraphTreeInvalidte
        {
            get
            {
                return _ParagraphTreeInvalidte;
            }
            set
            {
                _ParagraphTreeInvalidte = value;
            }
        }

        /// <summary>
        /// 重新设置段落列表状态
        /// </summary>
        public void RefreshParagraphListState(bool checkFlag, bool updateListIndex)
        {
            var doc = this._OwnerDocument;
            if (doc == null)
            {
                return;
            }
            if (checkFlag)
            {
                if (this._ParagraphTreeInvalidte == false)
                {
                    return;
                }
            }
            this._ParagraphTreeInvalidte = false;
            //doc.ResetOutlineNodes();

            this._FreeParagraphFlagGroups = new List<List<DomParagraphFlagElement>>();
            if (this._RootParagraphFlag == null)
            {
                this._RootParagraphFlag = new DomParagraphFlagElement();
            }
            else
            {
                this._RootParagraphFlag.ChildParagraphs().Clear();
            }
            this._RootParagraphFlag.IsRootLevelElement = true;
            if (this.Content.Count == 0)
            {
                // 没有元素，不可能建立起段落列表。
                return;
            }
            bool hasParagrahList = false;
            // 在很多情况下，通篇文档没有段落列表，在此进行判断
            foreach (DocumentContentStyle style in this._OwnerDocument.ContentStyles.Styles.FastForEach())
            {
                var rs = style.MyRuntimeStyle;
                if (rs.ParagraphOutlineLevel >= 0
                    || rs.ParagraphListStyle != ParagraphListStyle.None)
                {
                    hasParagrahList = true;
                    break;
                }
            }
            if (hasParagrahList == false)
            {
                // 样式列表中没有段落列表样式，则退出处理
                return;
            }
            var pefs = new DCStack<DomParagraphFlagElement>();
            pefs.Push(this._RootParagraphFlag);
            //int listIndex = 0;
            DomParagraphFlagElement lastElement = null;
            // 首先建立起树状结构
            foreach (DomElement element in this.Content.FastForEach())
            {
                if ((element is DomParagraphFlagElement) == false)
                {
                    continue;
                }
                DomParagraphFlagElement pef = (DomParagraphFlagElement)element;
                pef.ParentParagraph = null;
                //if (pef.OutlineLevel < 0)
                //{
                //    continue;
                //}
                ParagraphListStyle lStyle = pef.ListStyle();
                if (pef.OutlineLevel >= 0)//&& ParagraphListHelper.IsListNumberStyle(lStyle))
                {
                    pef.ListIndex = 0;
                    pef.ParentParagraph = null;
                    pef.MaxListIndex = 0;
                    pef.ChildParagraphs().Clear();
                    if (pefs.Count == 1)
                    {
                        pefs.Peek().AppendChildParagraph(pef);
                    }
                    else
                    {
                        while (pefs.Count > 0)
                        {
                            DomParagraphFlagElement lastP = pefs.Peek();
                            if (pefs.Count == 1 || lastP.OutlineLevel < pef.OutlineLevel)
                            {
                                lastP.AppendChildParagraph(pef);
                                break;
                            }
                            else
                            {
                                if (pefs.Count == 1)
                                {
                                    break;
                                }
                                pefs.Pop();
                            }
                        }
                    }
                    pefs.Push(pef);
                    //listIndex++;
                }//if
                if (pef.ParentParagraph == null && lStyle != ParagraphListStyle.None)
                {
                    // 游离的段落列表
                    List<DomParagraphFlagElement> group = null;
                    if (pef.ResetListIndexFlag == false)
                    {
                        // 添加到上次累计的列表中
                        if (this._FreeParagraphFlagGroups.Count > 0)
                        {
                            group = this._FreeParagraphFlagGroups[this._FreeParagraphFlagGroups.Count - 1];
                        }
                        if (lastElement != null)
                        {
                            if (lastElement.ParentParagraph != null
                                || lastElement.ListStyle() != pef.ListStyle())
                            {
                                // 不添加到上次累计的列表中
                                group = null;
                            }
                        }
                    }
                    if (group == null)
                    {
                        group = new List<DomParagraphFlagElement>();
                        this._FreeParagraphFlagGroups.Add(group);
                    }
                    group.Add(pef);
                }
                lastElement = pef;
            }//foreach
            pefs.Clear();
            pefs = null;

            if (updateListIndex)
            {
                // 设置段落列表编号
                UpdateParagrahListIndex(this._RootParagraphFlag);
                foreach (List<DomParagraphFlagElement> group in this._FreeParagraphFlagGroups)
                {
                    for (int iCount = 0; iCount < group.Count; iCount++)
                    {
                        DomParagraphFlagElement pef = (DomParagraphFlagElement)group[iCount];
                        pef.ListIndex = iCount + 1;
                        pef.MaxListIndex = group.Count;
                    }
                }
            }
            //foreach (XTextParagraphFlagElement item in rootParagraphFlag.ChildParagraphs)
            //{
            //    item.ParentParagraph = null;
            //}
        }

        /// <summary>
        /// 更新段落在列表中的编号
        /// </summary>
        /// <param name="rootParagraph"></param>
        private void UpdateParagrahListIndex(DomParagraphFlagElement rootParagraph)
        {
            int index = 1;
            ParagraphListStyle lStyle = ParagraphListStyle.None;
            List<DomParagraphFlagElement> group = new List<DomParagraphFlagElement>();
            foreach (DomParagraphFlagElement pef in rootParagraph.ChildParagraphs())
            {
                pef.ParentParagraph = rootParagraph;
                if (ParagraphListHelper.IsListNumberStyle(pef.ListStyle()))
                {
                    if (index == 0)
                    {
                        lStyle = pef.ListStyle();
                    }
                    if (pef.ListStyle() != lStyle || pef.ResetListIndexFlag)
                    {
                        index = 0;
                        if (group.Count > 0)
                        {
                            foreach (DomParagraphFlagElement item in group)
                            {
                                item.MaxListIndex = group.Count;
                            }
                        }
                        group.Clear();
                    }
                    group.Add(pef);
                    lStyle = pef.ListStyle();
                    pef.ListIndex = index + 1;
                    index++;
                }
                if (pef.HasChildParagraphs())// .ChildParagraphs.Count > 0)
                {
                    UpdateParagrahListIndex(pef);
                }
            }//foreach
            if (group.Count > 0)
            {
                foreach (DomParagraphFlagElement item in group)
                {
                    item.MaxListIndex = group.Count + 1;
                }
            }
            group.Clear();
            group = null;
        }
        private DCContent _Content = null;
        /// <summary>
        /// 文档内容管理对象
        /// </summary>
        public DCContent Content
        {
            get
            {
                if (_Content == null)
                {
                    _Content = new DCContent(this);
                }
                if (_Content.Count == 0 && this._OwnerDocument != null)
                {
                    FillContent();
                    this.RefreshParagraphListState(true, true);
                    if (this._Selection != null)
                    {
                        this._Selection.FixIndex(this._Content);
                    }
                }
                return _Content;
            }
        }
        internal void GetSelectionElement(ref DomElement startElement, ref DomElement endElement)
        {
            if (_Content != null && this._Selection != null)
            {
                startElement = _Content.SafeGet(this._Selection.StartIndex);
                endElement = _Content.SafeGet(this._Selection.StartIndex + this._Selection.Length);
                if (startElement == null)
                {
                    startElement = endElement;
                }
                if (endElement == null)
                {
                    endElement = startElement;
                }
            }
        }
        internal void SetSelectionElement(DomElement startElement, DomElement endElement)
        {
            if (startElement == null && endElement == null)
            {
                return;
            }
            if (startElement == null)
            {
                startElement = endElement;
            }
            if (endElement == null)
            {
                endElement = startElement;
            }

            if (_Content != null && _Content.Count > 0)
            {
                int index1 = _Content.FastIndexOf(startElement);
                int index2 = _Content.FastIndexOf(endElement);
                bool swap = false;
                if (index1 < 0 || index2 < 0)
                {
                    if (WriterUtilsInner.CompareDOMLevel(startElement, endElement) > 0)
                    {
                        swap = true;
                        DomElement temp = startElement;
                        startElement = endElement;
                        endElement = temp;
                    }
                    DomTreeNodeEnumerable nodes = new DomTreeNodeEnumerable(this, false);
                    nodes.ExcludeCharElement = false;
                    nodes.ExcludeParagraphFlag = false;
                    //int currentIndex = 0;
                    int lastIndex = -1;
                    //bool matchStart = false;
                    bool matchEnd = false;
                    foreach (DomElement element in nodes)
                    {
                        if (lastIndex < 0)
                        {
                            // 首先在DOM树中定位到第一个显示的文档元素
                            if (element == this._Content[0])
                            {
                                lastIndex = 0;
                            }
                            else
                            {
                                continue;
                            }
                        }
                        // 元素是否在视图中可见。
                        if (index1 < 0)
                        {
                            if (element == startElement)
                            {
                                DomElement e2 = this._Content.SafeGet(lastIndex + 1);
                                if (e2 != null && e2.ContentElement == this._Content[lastIndex].ContentElement)
                                {
                                    index1 = lastIndex + 1;
                                }
                                else
                                {
                                    index1 = lastIndex;
                                }
                                if (startElement == endElement)
                                {
                                    index2 = index1;
                                    break;
                                }
                            }
                        }
                        bool elementVisible = element == this._Content.SafeGet(lastIndex + 1);
                        if (elementVisible)
                        {
                            lastIndex++;
                        }
                        else if (element.ContentIndex >= 0)
                        {
                            if (element == this._Content.SafeGet(element.ContentIndex))
                            {
                                elementVisible = true;
                                lastIndex = element.ContentIndex;
                            }

                        }
                        if (index2 < 0)
                        {
                            if (matchEnd == false)
                            {
                                if (element == endElement)
                                {
                                    matchEnd = true;
                                }
                            }
                            if (matchEnd)
                            {
                                if (elementVisible)
                                {
                                    index2 = lastIndex;
                                }
                            }
                        }
                        if (index1 >= 0 && index2 >= 0)
                        {
                            break;
                        }
                    }//foreach
                    nodes.Dispose();
                }//if
                if (index1 >= 0 && index2 >= 0)
                {
                    if (swap)
                    {
                        this.SetSelection(index2, index1 - index2);
                    }
                    else
                    {
                        this.SetSelection(index1, index2 - index1);
                    }
                }
            }
        }
        private DCBooleanValue _HasHeaderRow = DCBooleanValue.Inherit;
        /// <summary>
        /// 文档中是否存在排版后的标题表格行对象
        /// </summary>
        internal bool HasHeaderRow
        {
            get
            {
                if (this._HasHeaderRow == DCBooleanValue.Inherit)
                {
                    this._HasHeaderRow = DCBooleanValue.False;
                    var elementsCount = this._Elements.Count;
                    var elementArray = this._Elements.InnerGetArrayRaw();
                    //foreach (var element in this._Elements.FastForEach())
                    for (var iCount = 0; iCount < elementsCount; iCount++)
                    {
                        var element = elementArray[iCount];
                        if (element is DomTableElement)
                        {
                            var table = (DomTableElement)element;
                            foreach (DomTableRowElement row in table.Rows.FastForEach())
                            {
                                if (row.RuntimeVisible && row.HeaderStyle)
                                {
                                    this._HasHeaderRow = DCBooleanValue.True;
                                    return true;
                                }
                            }
                        }
                    }
                }
                return this._HasHeaderRow == DCBooleanValue.True;
            }
        }

        /// <summary>
        /// 填充 Content 列表
        /// </summary>
        internal void FillContent()
        {
            this._HasHeaderRow = DCBooleanValue.Inherit;
            var doc = this.OwnerDocument;
            if (doc == null)
            {
                // 文档DOM状态不对
                return;
            }
            doc.CacheOptions();
            doc.IncreaseLayoutVersion();
            doc.ClearBuffer();
            //lock (this._Content)
            {
                this._Content.Clear();
                if (_ContentCountForLoadDocument > 200)
                {
                    // 根据估算的元素个数，设置容量，尽量避免多次扩容
                    this._Content.Capacity = _ContentCountForLoadDocument + 30;
                    //_ContentCountForLoadDocument = 0;
                }
                var args3 = new AppendViewContentElementArgs(this.OwnerDocument, this._Content, false);
                this.AppendViewContentElement(args3);
                int clen = this._Content.Count;
                if (_ContentCountForLoadDocument > 0)
                {
                    _ContentCountForLoadDocument = 0;
                }
                var eArray = this._Content.InnerGetArrayRaw();
                if (_InsertOrDeleteTextOnlyFlag)
                {
                    // 如果_InsertStringFlag==true,表示正在插入字符串，不可能插入或删除XTextObjectElement元素，
                    // 因此无需刷新XTextObjectElement相关的信息，在此进行判断来减少工作量。
                    //_InsertStringFlag = false;
                    for (int iCount = 0; iCount < clen; iCount++)
                    {
                        var item = eArray[iCount];
                        item._ContentIndex = iCount;
                    }
                }
                else
                {
                    if (args3.HasObjectElement == DCBooleanValue.True)
                    {
                    }
                    else if (args3.HasObjectElement == DCBooleanValue.False)
                    {
                    }
                    else
                    {
                        this.CheckChildElementStatsReady();
                    }
                    for (int iCount = 0; iCount < clen; iCount++)
                    {
                        var item = eArray[iCount];
                        item._ContentIndex = iCount;
                    }//for
                }
            }
        }

        /// <summary>
        /// 当前只进行插入或者删除文字的操作标记
        /// </summary>
        internal static bool _InsertOrDeleteTextOnlyFlag = false;

        /// <summary>
        /// 复制对象
        /// </summary>
        /// <param name="Deeply">是否深度复制</param>
        /// <returns>复制品</returns>
        public override DomElement Clone(bool Deeply)
        {
            DomDocumentContentElement dce = (DomDocumentContentElement)base.Clone(Deeply);
            dce._Content = null;
            dce._Lines = null;
            dce._Selection = null;
            dce._FreeParagraphFlagGroups = null;
            dce._RootParagraphFlag = null;
            dce._ParagraphTreeInvalidte = true;
            return dce;
        }

        internal override void UpdateContentElementsForControlOnLoad()
        {
            this._Content = new DCContent(this, 1);
            this._Content.FastAdd2(this._Elements[0]);
            this._Content[0]._ContentIndex = 0;
            base.UpdateContentElementsForControlOnLoad();
        }

        /// <summary>
        /// 更新文档内容元素列表
        /// </summary>
        /// <param name="args">参数</param>
        internal override void UpdateContentElements(UpdateContentElementsArgs args)
        {
            var document = this.OwnerDocument;
            if (document == null)
            {
                return;
            }
            if (args.SimpleTextMode)
            {
                document.TypedElements.Lock = true;
                document.ClearBufferForSimpleTextMode();
            }
            else
            {
                document.ClearBuffer();
            }
            if (this._Content == null)
            {
                this._Content = new DCContent(this);
            }
            else
            {

                this._Content.Clear();
            }
            document.IncreaseLayoutVersion();
            this.InnerClearLines();
            base.UpdateContentElements(args);
            if (args.SimpleTextMode)
            {
                document.TypedElements.Lock = false;
                document.TypedElements.SyncContentVersion();
            }
        }

        private void GetContentElements(DomContainerElement rootElement, DomElementList result)
        {
            foreach (DomElement element in rootElement.Elements)
            {
                if (element is DomContainerElement)
                {
                    if (element is DomTableElement)
                    {
                        // 特别处理表格
                        if (element._RuntimeVisible)
                        {
                            DomTableElement table = (DomTableElement)element;
                            foreach (DomTableRowElement row in table.Rows)
                            {
                                if (row._RuntimeVisible)
                                {
                                    foreach (DomTableCellElement cell in row.Cells)
                                    {
                                        if (cell.RuntimeVisible)
                                        {
                                            result.FastAdd2(cell);
                                            GetContentElements(cell, result);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else if (element is DomContentElement)
                    {
                        if (element._RuntimeVisible)
                        {
                            result.FastAdd2(element);
                            GetContentElements((DomContainerElement)element, result);
                        }
                    }
                    else
                    {
                        if (element._RuntimeVisible)
                        {
                            GetContentElements((DomContainerElement)element, result);
                        }
                    }
                }
            }//foreach
        }

        /// <summary>
        /// 创建新的文档对象，使其包含本文档元素的复制品
        /// </summary>
        /// <param name="includeThis">是否包含本文档原始对象,对文档块元素该参数无意义</param>
        /// <returns>创建的文档对象</returns>
        public override DomDocument CreateContentDocument(bool includeThis)
        {
            DomElementList elements = this.Elements.CloneDeeply();
            return WriterUtilsInner.CreateDocument(this.OwnerDocument, elements, false);
        }

        /// <summary>
        /// 元素是否处于选择状态
        /// </summary>
        /// <param name="element">元素对象</param>
        /// <returns>是否选择</returns>
        public virtual bool IsSelected(DomElement element)
        {
            return this._Selection != null && this._Selection.Contains(element);
        }

        /// <summary>
        /// 获得焦点
        /// </summary>
        public override void Focus()
        {
            DomDocument document = this.OwnerDocument;
            if (document != null && document.EditorControl != null)
            {
                document.EditorControl.ActiveDocumentContentElement(this);
            }
            else
            {
                if (document.CurrentContentElement != this)
                {
                    document.CurrentContentElement = this;
                }
            }
        }
        /// <summary>
        /// 当前选择区域信息对象
        /// </summary>
        private DCSelection _Selection = null;
        /// <summary>
        /// 当前选择区域信息对象
        /// </summary>
        public DCSelection Selection
        {
            get
            {
                if (_Selection == null)
                {
                    _Selection = new DCSelection(this);
                }
                return _Selection;
            }
        }

        /// <summary>
        /// 创建被选择区域信息对象
        /// </summary>
        /// <param name="startIndex">开始位置</param>
        /// <param name="length">区域长度</param>
        /// <returns>生成的选择区域信息对象</returns>
        public DCSelection CreateSelection(int startIndex, int length)
        {
            DCSelection selection = new DCSelection(this);
            selection.Refresh(startIndex, length);
            return selection;
        }

        /// <summary>
        /// 判断是否存在被用户选择的内容元素
        /// </summary>
        new public bool HasSelection
        {
            get
            {
                if (this._Selection != null)
                {
                    return this.Selection.Length != 0;
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// 设置选择区域
        /// </summary>
        /// <param name="firstIndex">开始序号</param>
        /// <param name="lastIndex">结束序号</param>
        /// <returns>操作是否成功</returns>
        public bool SetSelectionRange(int firstIndex, int lastIndex)
        {
            if (firstIndex >= 0 && lastIndex >= 0)
            {
                int index1 = this.Content.InnerFixIndex(
                    firstIndex,
                    FixIndexDirection.Forward,
                    true);
                int index2 = this.Content.InnerFixIndex(
                    lastIndex,
                    FixIndexDirection.Back,
                    true);
                if (index1 != firstIndex || index2 != lastIndex)
                {
                    return this.DocumentContentElement.SetSelection(index1, 0);
                }
                else
                {
                    return this.DocumentContentElement.SetSelection(
                        firstIndex,
                        lastIndex - firstIndex + 1);
                }
            }
            return false;
        }
        /// <summary>
        /// 更新文内容选择状态
        /// </summary>
        /// <param name="firstCell">第一个单元格</param>
        /// <param name="lastCell">最后一个单元格</param>
        internal bool SetSelectionSpecifyCells(DomTableCellElement firstCell, DomTableCellElement lastCell)
        {
            if (this.Content.Count == 0)
            {
                // 无任何内容，无法执行操作
                return false;
            }
            if (firstCell == null)
            {
                throw new ArgumentNullException("firstCell");
            }
            if (lastCell == null)
            {
                lastCell = firstCell;
            }
            if (firstCell.RuntimeVisible == false
                || lastCell.RuntimeVisible == false
                || firstCell.FirstContentElementInPublicContent == null
                || lastCell.FirstContentElementInPublicContent == null)
            {
                // 单元格不可见或者状态不对
                return false;
            }
            int oldSelectionStart = this.Selection.StartIndex;
            int oldSelectionLength = this.Selection.Length;
            // 修正旧的选择区域
            // 由于执行了某些操作改变了文档内容但没有更新选择区域，因此旧的选择区域可能超出文档范围，
            // 在此进行修正。
            this.Content.FixRange(ref oldSelectionStart, ref oldSelectionLength);

            // 触发文档对象的OnSelectionChanging事件
            SelectionChangingEventArgs args = new SelectionChangingEventArgs();
            args.Documnent = this.OwnerDocument;
            args.OldSelectionIndex = this.Selection.StartIndex;
            args.OldSelectionLength = this.Selection.Length;
            args.NewSelectionIndex = firstCell.FirstContentElementInPublicContent.ContentIndex;
            args.NewSelectionLength = lastCell.LastContentElementInPublicContent.ContentIndex
                - firstCell.FirstContentElement.ContentIndex;
            this.OwnerDocument.OnSelectionChanging(args);
            if (args.Cancel)
            {
                // 用户取消操作
                return false;
            }
            // 使用用户设置的新的选择区域配置

            if (this.Selection.SelectCells(firstCell, lastCell))
            {
                if (this.OwnerDocument.EditorControl != null)
                {
                    //this.OwnerDocument.EditorControl.Invalidate();
                    this.OwnerDocument.EditorControl.UpdateTextCaret();
                }
                //XTextElement oldCurrentElement = this.Content[oldSelectionStart];
                RaiseFocusEvent(
                    this.Content.SafeGet(oldSelectionStart),
                    this.Content.SafeGet(this.Selection.StartIndex));
                // 触发文档对象的选择区域发生改变事件
                this.OwnerDocument.OnSelectionChanged();
                if (this.InnerViewControl != null)
                {
                    this.InnerViewControl.Invalidate();
                }
                return true;
            }
            return false;
        }
        internal void ResetSelectionRaw()
        {
            this.Selection.Reset();
        }
        /// <summary>
        /// 更新文内容选择状态
        /// </summary>
        /// <param name="startIndex">选择区域的起始位置</param>
        /// <param name="length">选择区域的包含文档内容元素的个数</param>
        public bool SetSelection(int startIndex, int length)
        {
            var document = this.OwnerDocument;
            if (document == null)
            {
                return false;
            }
            if (document.IsLoadingDocument && startIndex == 0 && length == 0)
            {
                return false;
            }
            if (this.Content.Count == 0)
            {
                // 无任何内容，无法执行操作
                return false;
            }
            if (startIndex < 0)
            {
                startIndex = 0;
            }
            int oldSelectionStart = this.Selection.StartIndex;
            int oldSelectionLength = this.Selection.Length;
            // 修正旧的选择区域
            // 由于执行了某些操作改变了文档内容但没有更新选择区域，因此旧的选择区域可能超出文档范围，
            // 在此进行修正。
            this.Content.FixRange(ref oldSelectionStart, ref oldSelectionLength);
            if (this.Selection.NeedRefresh(startIndex, length))
            {
                // 触发文档对象的OnSelectionChanging事件
                SelectionChangingEventArgs args = new SelectionChangingEventArgs();
                args.Documnent = document;
                args.OldSelectionIndex = this.Selection.StartIndex;
                args.OldSelectionLength = this.Selection.Length;
                args.NewSelectionIndex = startIndex;
                args.NewSelectionLength = length;
                document.OnSelectionChanging(args);
                if (args.Cancel)
                {
                    // 用户取消操作
                    return false;
                }
                // 使用用户设置的新的选择区域配置
                startIndex = args.NewSelectionIndex;
                length = args.NewSelectionLength;
                var bopts = GetDocumentBehaviorOptions();

                if (this.Selection.Refresh(startIndex, length))
                {
                    if (document.EditorControl != null)
                    {
                        //this.OwnerDocument.EditorControl.Invalidate();
                        if (this.Selection.IsBig())
                        {
                            // 选择区域很大，直接定位滚动，不需要平滑滚动动画效果。
                            document.EditorControl.UpdateTextCaret();
                        }
                        else
                        {
                            var bolUpdated4 = false;
                            if (this.InnerViewControl.DragState == DragOperationState.Drag
                                && document.Options.BehaviorOptions.ParagraphFlagFollowTableOrSection)
                            {
                                // 正在处于OLD拖拽
                                var curElement = this.CurrentElement;
                                if (curElement is DomParagraphFlagElement)
                                {
                                    var line = curElement.OwnerLine;
                                    if (line != null && line.IsTableLine)
                                    {
                                        this.InnerViewControl.UpdateTextCaretWithoutScroll();
                                        bolUpdated4 = true;
                                    }
                                }
                            }
                            if (bolUpdated4 == false)
                            {
                                document.EditorControl.UpdateTextCaret();
                            }
                        }
                    }
                    if (document.IsLoadingDocument == false)
                    {
                        // XTextElement oldCurrentElement = this.Content[oldSelectionStart];
                        RaiseFocusEvent(
                            this.Content.SafeGet(oldSelectionStart),
                            this.Content.SafeGet(this.Selection.StartIndex));
                    }
                    // 触发文档对象的选择区域发生改变事件
                    document.OnSelectionChanged();
                    //tick = CountDown.GetTickCountFloat() - tick;
                    return true;
                }
            }
            //tick = CountDown.GetTickCountFloat() - tick;
            return false;
        }
        internal void RaiseFocusEvent(DomElement oldCurrentElement, DomElement newCurrentElement)
        {
            if (oldCurrentElement == newCurrentElement)
            {
                return;
            }
            var document = this.OwnerDocument;
            if (document.DisableFocusEventOnce)
            {
                document.DisableFocusEventOnce = false;
                return;
            }
            var bopts = GetDocumentBehaviorOptions();
            DomElementList oldParents = oldCurrentElement == null ?
                new DomElementList() : WriterUtilsInner.GetParentList2(oldCurrentElement);
            if (bopts.WidelyRaiseFocusEvent)
            {
                oldParents = oldCurrentElement == null ?
                    new DomElementList() : WriterUtilsInner.GetParentList(oldCurrentElement);
            }
            if (oldCurrentElement != null)
            {
                oldParents.Insert(0, oldCurrentElement);
            }

            DomElementList newParents = newCurrentElement == null ?
                new DomElementList() : WriterUtilsInner.GetParentList2(newCurrentElement);
            if (bopts.WidelyRaiseFocusEvent)
            {
                newParents = newCurrentElement == null ?
                    new DomElementList() : WriterUtilsInner.GetParentList(newCurrentElement);
            }

            if (newCurrentElement != null)
            {
                newParents.Insert(0, newCurrentElement);
            }
            bool bolTableInvalidate = false;
            // 触发旧的容器元素的失去输入焦点事件
            foreach (DomElement oldParent in oldParents)
            {
                if (oldParent is DomTableElement)
                {
                    bolTableInvalidate = true;
                }
                if (newParents.Contains(oldParent) == false)
                {
                    //((XTextContainerElement)oldParent).OnLostFocus(this, EventArgs.Empty);
                    if (oldParent is DomCharElement)
                    {
                        // 字符元素不触发焦点事件
                        continue;
                    }
                    DocumentEventArgs args2 = new DocumentEventArgs(
                        document,
                        oldParent,
                        DocumentEventStyles.LostFocus);
                    oldParent.HandleDocumentEvent(args2);
                    args2.intStyle = DocumentEventStyles.LostFocusExt;
                    oldParent.HandleDocumentEvent(args2);
                }
            }
            DomTableElement lastTable = null;
            // 触发新的容器元素的获得输入焦点事件
            foreach (DomElement newParent in newParents)
            {
                if (bolTableInvalidate && newParent is DomTableElement)
                {
                    lastTable = (DomTableElement)newParent;
                }
                if (oldParents.Contains(newParent) == false)
                {
                    //((XTextContainerElement)newParent).OnGotFocus(this, EventArgs.Empty);
                    if (newParent is DomCharElement)
                    {
                        // 字符元素不触发焦点事件
                        continue;
                    }
                    DocumentEventArgs args2 = new DocumentEventArgs(
                        document,
                        newParent,
                        DocumentEventStyles.GotFocus);
                    newParent.HandleDocumentEvent(args2);
                }
            }
            if (lastTable != null)
            {
                if (lastTable.IsBigTable() == false)
                {
                    lastTable.InvalidateView();
                }
            }
        }

        /// <summary>
        /// 当前行
        /// </summary>
        public DomLine CurrentLine
        {
            get
            {
                return this._Content?.CurrentLine;
            }
        }

        /// <summary>
        /// 当前元素
        /// </summary>
        public DomElement CurrentElement
        {
            get
            {
                if (this._OwnerDocument == null)
                {
                    return null;
                }
                if (this._Content == null || this._Content.Count == 0)
                {
                    return null;
                }
                else if (this._Selection == null)
                {
                    return this._Content[this._Content.Count - 1];
                }
                else
                {
                    return this._Content.CurrentElement;
                }
            }
        }


        /// <summary>
        /// 当前段落对象
        /// </summary>
        public DomParagraphFlagElement CurrentParagraphEOF
        {
            get
            {
                return this.Content.CurrentParagraphEOF;
            }
        }
        private System.WeakReference _LinesBack = null;
        private void InnerClearLines()
        {
            if (this._Lines != null)
            {
                this._Lines.Clear();
                this._LinesBack = new WeakReference(this._Lines);
                this._Lines = null;
            }
        }
        /// <summary>
        /// 文本行列表
        /// </summary>
        private DomLineList _Lines = null;
        /// <summary>
        /// 文本行列表
        /// </summary>
        public DomLineList Lines
        {
            get
            {
                if (this._Lines == null)
                {
                    if (this._LinesBack != null && this._LinesBack.IsAlive)
                    {
                        this._Lines = (DomLineList)this._LinesBack.Target;
                    }
                    else
                    {
                        this._Lines = new DomLineList();
                    }
                    this._LinesBack = null;
                    FillLines(this, this._Lines);
                    //for(int iCount = this._Lines.Count -1; iCount >= 0; iCount --)
                    //{
                    //    this._Lines[iCount].IndexOfDocumentContentLines = iCount;
                    //}
                }
                return _Lines;
            }
        }
        private void SetLinesByOldLines(DomLineList oldLines)
        {
            if (this._Lines == null && oldLines != null)
            {
                this._Lines = oldLines;
                this._Lines.Clear();
                FillLines(this, this._Lines);
                for (int iCount = this._Lines.Count - 1; iCount >= 0; iCount--)
                {
                    this._Lines[iCount].IndexOfDocumentContentLines = iCount;
                }
            }
        }
        /// <summary>
        /// 获得所有的文档行对象，包括子容器元素的文档行
        /// </summary>
        /// <remarks>获得的文档行对象列表</remarks>
        public DomLineList GetAllLines()
        {
            if (this._PrivateLines == null || this._PrivateLines.Count == 0)
            {
                return null;
            }
            DomLineList result = new DomLineList(this.PrivateLines.Count);
            GetAllLinesDeeply(this, result);
            return result;
        }

        private void GetAllLinesDeeply(DomContentElement root, DomLineList lines)
        {
            if (root.PrivateLines == null)
            {
                return;
            }
            var lineCount = root.PrivateLines.Count;
            var lineArray = root.PrivateLines.InnerGetArrayRaw();
            //foreach (XTextLine line in root.PrivateLines.FastForEach())
            for (var iCount = 0; iCount < lineCount; iCount++)
            {
                var line = lineArray[iCount];
                lines.Add(line);
                if (line.IsTableLine)
                {
                    DomTableElement table = line.TableElement;
                    if (table.Columns != null)
                    {
                        lines.EnsureCapacity(lines.Count + table.Rows.Count * table.Columns.Count);
                        foreach (var row in table.Rows.FastForEach())
                        {
                            foreach (DomTableCellElement cell in row.Elements.FastForEach())
                            {
                                if (cell.IsOverrided == false)
                                {
                                    GetAllLinesDeeply(cell, lines);
                                }
                            }
                        }
                    }
                }
            }//foreach
        }

        /// <summary>
        /// 更新文档行的编号
        /// </summary>
        internal void GlobalUpdateLineIndex(DomLineList oldLines = null)
        {
            var doc = this._OwnerDocument;
            if (doc == null)
            {
                return;
            }
            if (doc.Pages == null || doc.Pages.Count == 0)
            {
                // 尚未分页，不执行本操作。
                return;
            }
            UpdateLineIndex(1);
            PrintPage lastPage = null;
            int pageFirstLineIndex = 0;
            if (oldLines != null && this._Lines == null)
            {
                SetLinesByOldLines(oldLines);
            }
            var lineArr = this.Lines.InnerGetArrayRaw();
            var lineCount = this.Lines.Count;
            //foreach (XTextLine line in this.Lines.FastForEach())
            for (var lineIndex = 0; lineIndex < lineCount; lineIndex++)
            {
                var line = lineArr[lineIndex];
                //if (line.OwnerPage == null)
                //{
                //}
                if (line._OwnerPage != lastPage)
                {
                    lastPage = line._OwnerPage;
                    pageFirstLineIndex = line.GlobalIndex;
                }
                line.IndexInPage = line.GlobalIndex - pageFirstLineIndex + 1;
            }//foreach
            lastPage = null;
            pageFirstLineIndex = 0;

            int iCount = 0;
            lineArr = this.PrivateLines.InnerGetArrayRaw();
            lineCount = this.PrivateLines.Count;
            //foreach (XTextLine line in this.PrivateLines.FastForEach())
            for (var lineIndex = 0; lineIndex < lineCount; lineIndex++)
            {
                var line = lineArr[lineIndex];
                if (line._OwnerPage != lastPage)
                {
                    lastPage = line._OwnerPage;
                    pageFirstLineIndex = iCount;
                }
                line.PrivateIndexInPage = iCount - pageFirstLineIndex + 1;
                iCount++;
            }
        }

        /// <summary>
        /// 内容视图宽度
        /// </summary>
        private float _ContentViewWidth = 0;
        /// <summary>
        /// 内容视图宽度
        /// </summary>
        public override float ViewWidth
        {
            get
            {
                return _ContentViewWidth;
            }
        }

        /// <summary>
        /// 对整个内容执行重新排版操作
        /// </summary>
        internal void InnerForceExecuteLayout()
        {
            var doc = this.OwnerDocument;
            if (doc == null)
            {
                return;
            }
            this.Width = doc.PageSettings.ViewClientWidth;
            this._PrivateLines?.Clear();
            this._PrivateContent?.ClearOwnerLine();
            var args = new ParticalRefreshLinesEventArgs(null, null, this.ContentVertialAlign);
            args.ForceRefreshAllContent = true;
            args.CheckStopLine = false;
            this.ParticalRefreshLines(args);
            DomContentElement._Cached_PrivateContent?.ClearAndEmptyAll();
            LoaderListBuffer<DomLine[]>.Instance.Clear(null);
        }
        private int _GlobalLineCount = 0;
        internal void IncreateGlobalLineCount(int v)
        {
            _GlobalLineCount += v;
        }
        /// <summary>
        /// 重新分行
        /// </summary>
        /// <param name="args">参数</param>
        /// <returns></returns>
        internal override bool ParticalRefreshLines(ParticalRefreshLinesEventArgs args)
        {
            this._GlobalLineCount = 0;
            if (WriterControl.RefreshingDocumentView)
            {
                var c44 = this.Content;
            }
            bool result = base.ParticalRefreshLines(args);
            ElementStack.ClearLastCache();
            DCObjectPool<DomLine>.Clear();
            //XTextInputFieldElementBase.Clear_Cache_FixBorderElementWidth();
            var oldLines = this._Lines;
            if (oldLines != null)
            {
                oldLines.Clear();
            }
            if (this._Lines == null)
            {
                this._Lines = new DomLineList();
            }
            else
            {
                this._Lines.Clear();
            }
            if (this._GlobalLineCount > 0)
            {
                if (this._Lines.Capacity < this._GlobalLineCount)
                {
                    this._Lines.Capacity = this._GlobalLineCount;
                }
            }
            FillLines(this, this._Lines);
            this.Height = this.ContentHeightExcludeLastLineAdditionHeight();
            float w = this.Width;
            //foreach (var line in this.PrivateLines.FastForEach())
            for (var iCount = this.PrivateLines.Count - 1; iCount >= 0; iCount--)
            {
                var line = this._PrivateLines[iCount];
                float v = line.Left + line.ViewWidth;
                if (v > w)
                {
                    w = v;
                }
                //w = Math.Max(w, line.Left + line.ViewWidth );
            }
            this._ContentViewWidth = w;
            GlobalUpdateLineIndex(oldLines);
            return result;
        }

        internal void RefreshGlobalLines()
        {
            InnerClearLines();
            //this._Lines = null;
        }

        public override void DrawContent(InnerDocumentPaintEventArgs args)
        {
            args.ViewBounds = this.GetAbsBounds();
            args.SetDocumentContentElement(this);
            args.Content = this.Content;
            base.DrawContent(args);
        }

        /// <summary>
        /// 收集各层元素包含的文档行对象
        /// </summary>
        /// <param name="contentElement"></param>
        /// <param name="lines"></param>
        private void FillLines(DomContentElement contentElement, DomLineList lines)
        {
            var pLines = contentElement.PrivateLines;
            var len = pLines.Count;
            var linesArray = pLines.InnerGetArrayRaw();
            for (var iCount = 0; iCount < len; iCount++)
            //foreach (XTextLine line in contentElement.PrivateLines.FastForEach())
            {
                var line = linesArray[iCount];// pLines.GetItemFast(iCount);
                //line.UpdateAbsLocation();
                if (line.IsTableLine)
                {
                    var rows = line.TableElement.Rows;
                    var rowsCount = rows.Count;
                    var rowsArray = rows.InnerGetArrayRaw();
                    for (var rowIndex = 0; rowIndex < rowsCount; rowIndex++)
                    //foreach (XTextTableRowElement row in line.TableElement.Rows.FastForEach())
                    {
                        var row = rowsArray[rowIndex];// (XTextTableRowElement)rows.GetItemFast(rowIndex);
                        if (row._RuntimeVisible)
                        {
                            var cells = row.Elements;
                            var cellsCount = cells.Count;
                            var cellsArray = cells.InnerGetArrayRaw();
                            for (var cindex = 0; cindex < cellsCount; cindex++)
                            //foreach (XTextTableCellElement cell in row.Cells.FastForEach())
                            {
                                var cell = (DomTableCellElement)cellsArray[cindex];// cells.GetItemFast(cindex);
                                if (cell.RuntimeVisible)
                                {
                                    FillLines(cell, lines);
                                }
                            }
                        }
                    }
                }
                else
                {
                    //line.ContentLineIndex = lines.Count;
                    line.IndexOfDocumentContentLines = lines.Count;
                    lines.Add(line);
                }
            }//foreach
        }

        /// <summary>
        /// 在编辑器中更新文档视图
        /// </summary>
        /// <param name="fastMode">快速模式</param>
        public override void EditorRefreshViewExt(bool fastMode)
        {
            this.SizeInvalid = true;
            this.FixDomState();
            this.UpdateContentElements(true);
            this.InvalidateView();
            // 重新计算大小
            using (DCGraphics g = this.InnerCreateDCGraphics())
            {
                InnerDocumentPaintEventArgs args = this.OwnerDocument.CreateInnerPaintEventArgs(g);
                args.ActiveMode = false;
                args.Element = this;
                if (fastMode)
                {
                    args.CheckSizeInvalidateWhenRefreshSize = true;
                }
                else
                {
                    args.CheckSizeInvalidateWhenRefreshSize = false;
                }
                this.RefreshSize(args);
            }
            //if (this is XTextContainerElement)
            //{
            //    ((XTextContainerElement)this).ExecuteLayout();
            //}
            //this.UpdateContentElements(true);
            EditorRefreshView_Enum(this, fastMode == false);
            //XTextElementList elements = new XTextElementList(this);
            //foreach( XTextElement element in elements )
            //{
            //    EditorRefreshView_Enum(element , fastMode == false );
            //}
            this.UpdateElementBorderInfos(false);
            this.RefreshPrivateContent(DomContentElement.FlagNotCheckStopLine);
            this.InvalidateView();
            this.Selection.UpdateState();
            //if (this.OwnerDocument.EditorControl != null)
            //{
            //    this.OwnerDocument.EditorControl.RefreshDocumentExt(false, false);
            //}
            //else
            //{
            //    this.OwnerDocument.RefreshPages();
            //}
            ////this.OwnerDocument.Modified = true;
            //// 触发文档内容修改事件
            //ContentChangedEventArgs args2 = new ContentChangedEventArgs();
            //args2.UndoRedoCause = true;
            //args2.Document = this.OwnerDocument;
            //args2.Element = this ;
            //this.RaiseBubbleOnContentChanged(args2);
            //this.OwnerDocument.OnDocumentContentChanged();
        }

        /// <summary>
        /// 内部的文档加载后的处理
        /// </summary>
        /// <param name="args">事件参数</param>
        public override void AfterLoad(ElementLoadEventArgs args)
        {
            if (this._Content != null)
            {
                this._Content.Clear();
            }
            if (this._Lines != null)
            {
                this._Lines.Clear();
            }
            this._ParagraphTreeInvalidte = true;
            base.AfterLoad(args);
        }

        /// <summary>
        /// 清空文档内容
        /// </summary>
        public override void Clear()
        {
            this.OwnerDocument.ClearBuffer();
            this.Elements.Clear();
            this.Elements.Add(this.OwnerDocument.CreateElement<DomParagraphFlagElement>())
                ;// .CreateParagraphEOF());
            //myElements.Add( this.myEOFElement );
            this.UpdateContentElements(true);
            this.InnerExecuteLayout();
        }

        #region 一些属性无效 ***********************************************

        /// <summary>
        /// 方法无效
        /// </summary>
        private new void EditorDelete(bool logUndo)
        {
            throw new NotSupportedException();
        }
        [System.Obsolete()]

        new private DomTableCellElement OwnerCell { get { return null; } }
        [System.Obsolete()]

        new private DomElement PreviousElement { get { return null; } }
        [System.Obsolete()]

        new private DocumentContentStyle RuntimeStyle { get { return null; } }
        [System.Obsolete()]

        new private DocumentContentStyle Style { get { return null; } }
        /// <summary>
        /// 废除
        /// </summary>
        [Obsolete()]
        public new int StyleIndex { get { return -1; } set { } }
        [System.Obsolete()]
        new private int ViewIndex { get { return 0; } }
        [System.Obsolete()]

        new private int ColumnIndex { get { return 0; } }
        [System.Obsolete()]
        new private int ElementIndex { get { return 0; } }

        #endregion

        /// <summary>
        /// 返回内容部分所属样式
        /// </summary>
        public virtual PageContentPartyStyle PagePartyStyle
        {
            get
            {
                return PageContentPartyStyle.Body;
            }
        }

        public override void Dispose()
        {
            base.Dispose();
            if (this._Content != null)
            {
                this._Content.InnerDispose();
                this._Content.Capacity = 0;
                this._Content = null;
            }
            if (this._FreeParagraphFlagGroups != null)
            {
                this._FreeParagraphFlagGroups.Clear();
                this._FreeParagraphFlagGroups = null;
            }
            if (this._Lines != null)
            {
                this._Lines.InnerDispose();
                this._Lines.Capacity = 0;
                this._Lines = null;
            }
            //if (this._RootParagraphFlag != null)
            {
                this._RootParagraphFlag = null;
            }
            if (this._Selection != null)
            {
                this._Selection.Dispose();
                this._Selection = null;
            }
        }
    }//public class XTextDocumentContentElement : XTextContentElement
}