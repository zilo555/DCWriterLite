using System.Collections.Generic;
using System.ComponentModel;
using DCSoft.Printing;
using DCSoft.Drawing;
using DCSoft.Common;
using DCSoft.Writer.Controls;

namespace DCSoft.Writer.Dom
{
    /// <summary>
    /// 文档内容元素类型
    /// </summary> 
    /// <remarks>编制 袁永福</remarks>
    public abstract partial class DomContentElement : DomContainerElement
    {
        /// <summary>
        /// 初始化对象
        /// </summary>
        protected DomContentElement()
        {
            this.LayoutInvalidate = true;
        }


        public virtual DCGridLineInfo InnerRuntimeGridLine()
        {
            return this._OwnerDocument?._GlobalGridInfo;
        }


        /// <summary>
        /// 选中所有内容
        /// </summary>
        /// <returns>操作是否成功</returns>
        public override bool Select()
        {
            DomDocumentContentElement dce = this.DocumentContentElement;
            int startIndex = dce.Content.IndexOf(this.FirstContentElement);
            int endIndex = dce.Content.IndexOf(this.LastContentElement) - 1;
            if (endIndex >= startIndex)
            {
                return dce.SetSelection(startIndex, endIndex - startIndex + 1);
            }
            return false;
        }

        /// <summary>
        /// 表格内容排版无效标志
        /// </summary>
        internal bool LayoutInvalidate
        {
            get { return this.StateFlag17; }
            set { this.StateFlag17 = value; }
        }

        /// <summary>
        /// 文本行列表
        /// </summary>
        protected DomLineList _PrivateLines;// new XTextLineList();
        /// <summary>
        /// 元素私有的文本行列表
        /// </summary>
        public DomLineList PrivateLines
        {
            get
            {
                if (this._PrivateLines == null)
                {
                    this._PrivateLines = new DomLineList();
                }
                return this._PrivateLines;
            }
        }
        /// <summary>
        /// 判断是否只包含一个段落符号元素
        /// </summary>
        /// <returns></returns>
        public bool OnlyHasSingleParagraphFlagElement()
        {
            return this._Elements != null && this._Elements.OnlyHasSingleParagraphFlagElement();
        }

        /// <summary>
        /// 是否具有内部文档行信息
        /// </summary>
        /// <returns>是否有内部行</returns>
        internal bool HasPrivateLines()
        {
            return this._PrivateLines != null && this._PrivateLines.Count > 0;
        }

        internal DomLine GetLineAt(float viewX, float viewY, bool strict)
        {
            var plines = this.PrivateLines;
            if (plines == null || plines.Count == 0)
            {
                return null;
            }
            float x = viewX - this.GetAbsLeft();
            float y = viewY - this.GetAbsTop();
            DomLine result = null;
            if (strict)
            {
                // 进行严格判断
                var lineArray = plines.InnerGetArrayRaw();
                var startIndex = 0;
                var endIndex = plines.Count - 1;
                while (startIndex < endIndex - 8)
                {
                    int midIndex = (startIndex + endIndex) / 2;
                    var line = lineArray[midIndex];
                    if (y < line.Top)
                    {
                        endIndex = midIndex;
                    }
                    else if (y > line.Top + line.ViewHeight)
                    {
                        startIndex = midIndex;
                    }
                    else if (line.ContainsXYByViewHeight(x, y))
                    {
                        result = line;
                        break;
                    }
                    else
                    {
                        // 遇到特殊情况，退出循环
                        break;
                    }
                }
                if (result == null)
                {
                    for (var iCount = startIndex; iCount <= endIndex; iCount++)
                    {
                        if (lineArray[iCount].ContainsXYByViewHeight(x, y))
                        {
                            result = lineArray[iCount];
                            break;
                        }
                    }
                }
            }
            else
            {
                // 进行不严格的判断
                // 进行非严格判断
                // 确定当前行,指定的Y坐标在当前行低边缘上面
                int len = plines.Count;
                for (int iCount = 0; iCount < len; iCount++)
                {
                    var myLine = plines[iCount];
                    if (myLine.Top + myLine.ViewHeight > y)
                    {
                        if (iCount > 0)
                        {
                            DomLine preLine = plines[iCount - 1];
                            float pos = (preLine.Top + preLine.ViewHeight + myLine.Top) / 2;
                            if (y > pos)
                            {
                                result = myLine;
                            }
                            else
                            {
                                result = preLine;
                            }
                        }
                        else
                        {
                            result = myLine;
                        }
                        break;
                    }
                }//foreach
            }
            if (strict == false)
            {
                // 若没有找到当前行则设置最后一行为当前行
                if (result == null)
                {
                    result = plines.LastLine;
                }
            }
            return result;
        }

        /// <summary>
        /// 内容高度
        /// </summary>
        public float ContentHeight
        {
            get
            {
                var document = this._OwnerDocument;
                if (document == null)
                {
                    return 0;
                }
                float ch = 0;
                DomLineList pLines = this._PrivateLines;
                if (pLines == null || pLines.Count == 0)
                {
                    ch = 0;
                }
                else if (pLines.Count == 1)
                {
                    ch = pLines.GetItemFast(0)._Height;
                }
                else
                {
                    ch = pLines.LastLine.Bottom - pLines[0].Top;
                }
                if (this is DomTableCellElement)
                {
                    var info = document._GlobalGridInfo;
                    if (info != null && info.AlignToGridLine)
                    {
                        // 遇到单元格/文档节而且存在全局文档网格线，则直接返回内容高度。
                        return ch;
                    }
                }
                if (document._GlobalGridInfo == null)
                {
                    if (this.ZeroRuntimePaddingTopBottom == false)
                    {
                        var rs = this.RuntimeStyle;
                        ch = ch + rs.PaddingTop + rs.PaddingBottom;
                    }
                    //ch = ch + this._RuntimePaddingTop + this._RuntimePaddingBottom;
                }
                return ch;
            }
        }

        /// <summary>
        /// 除去最后一行额外高度的内容高度
        /// </summary>
        internal protected float ContentHeightExcludeLastLineAdditionHeight()
        {
            float ch = 0;
            DomLineList pLines = this.PrivateLines;
            if (pLines.Count > 0)
            {
                ch = pLines.LastLine.Bottom - pLines[0].Top;
            }
            if (this.OwnerDocument?._GlobalGridInfo == null)
            {
                // 没有全局网格线时，则累计上下内边距。
                if (this.ZeroRuntimePaddingTopBottom == false)
                {
                    var rs = this.RuntimeStyle;
                    ch = ch + rs.PaddingTop + rs.PaddingBottom;
                }
                //ch = ch + this._RuntimePaddingTop + this._RuntimePaddingBottom;// rs.PaddingTop + rs.PaddingBottom;
            }
            return ch;

        }

        /// <summary>
        /// 更新文档行的行号
        /// </summary>
        /// <param name="startIndex">起始行号</param>
        /// <returns>本文档中累计的行数</returns>
        internal protected int UpdateLineIndex(int startIndex)
        {
            if (this._PrivateLines != null && this._PrivateLines.Count > 0)
            {
                var lines = this._PrivateLines;
                int len = lines.Count;
                var lineArr = lines.InnerGetArrayRaw();
                for (int iCount = 0; iCount < len; iCount++)
                {
                    var line = lineArr[iCount];
                    line._GlobalIndex = startIndex;
                    if (line.IsTableLine)
                    {
                        DomTableElement table = line.TableElement;
                        var rowArr = table.Rows.InnerGetArrayRaw();
                        var rowCount = table.Rows.Count;
                        for (var rowIndex = 0; rowIndex < rowCount; rowIndex++)
                        {
                            var row = (DomTableRowElement)rowArr[rowIndex];
                            if (row._RuntimeVisible)
                            {
                                int maxLines = startIndex;
                                var cellArr = row.Cells.InnerGetArrayRaw();
                                var cellCount = row.Cells.Count;
                                for (var cellIndex = 0; cellIndex < cellCount; cellIndex++)
                                {
                                    var cell = (DomTableCellElement)cellArr[cellIndex];
                                    if (cell.RuntimeVisible)
                                    {
                                        int result = cell.UpdateLineIndex(startIndex);
                                        if (maxLines < result)
                                        {
                                            maxLines = result;
                                        }
                                    }
                                }//foreach
                                startIndex = maxLines;
                            }//if
                        }//foreach
                    }
                    else
                    {
                        startIndex++;
                    }
                }//foreach
                return startIndex;
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// 设置多个文本行为无效状态
        /// </summary>
        /// <param name="startLine">开始行</param>
        /// <param name="endLine">结束行</param>

        public void SetLinesInvalidateState(DomLine startLine, DomLine endLine)
        {
            if (startLine == endLine && startLine != null)
            {
                startLine.InvalidateState = true;
                return;
            }
            int startIndex = this.PrivateLines.IndexOf(startLine);
            if (startIndex < 0)
            {
                startIndex = 0;
            }
            int endIndex = this.PrivateLines.IndexOf(endLine);
            if (endIndex < 0)
            {
                endIndex = this.PrivateLines.Count - 1;
            }
            for (int iCount = startIndex; iCount <= endIndex; iCount++)
            {
                this.PrivateLines[iCount].InvalidateState = true;
            }
        }

        /// <summary>
        /// 获得字符元素和所属段落符号的映射表
        /// </summary>
        /// <returns></returns>
        public Dictionary<DomCharElement, DomParagraphFlagElement> GetCharOwnerPFlags()
        {
            var maps = new Dictionary<DomCharElement, DomParagraphFlagElement>();
            DomParagraphFlagElement flag = null;
            for (int iCount = this.PrivateContent.Count - 1; iCount >= 0; iCount--)
            {
                var e = this._PrivateContent[iCount];
                if (e is DomParagraphFlagElement)
                {
                    flag = (DomParagraphFlagElement)e;
                }
                else if (e is DomCharElement)
                {
                    maps[(DomCharElement)e] = flag;
                }
            }
            return maps;
        }
        /// <summary>
        /// 在PrivateContent中是否有XTextObjectElement类型的元素
        /// </summary>
        private bool HasObjectElementInPrivateContent
        {
            get { return this.StateFlag15; }
            set { this.StateFlag15 = value; }
        }

        protected DomElementList _PrivateContent;
        internal void ClearPrivateContent()
        {
            if (this._PrivateContent != null)
            {
                this._PrivateContent.SetToNone();
            }
        }
        /// <summary>
        /// 全局性的用于缓存列表，用于减少扩容导致的内存占用。
        /// </summary>
        internal static DomElementList _Cached_PrivateContent = null;

        /// <summary>
        /// 文档内容管理对象
        /// </summary>
        public DomElementList PrivateContent
        {
            get
            {
                if (this._PrivateContent == null || this._PrivateContent.IsNone)
                {
                    if (GlobalEnabledResetChildElementStats)
                    {
                        this.ResetChildElementStats();
                    }
                    this.HasObjectElementInPrivateContent = false;
                    this.IncreaseDocumentLayoutVersion();
                    var document = this.OwnerDocument;
                    if (document != null)
                    {
                        document.CheckCacheOptions();
                    }
                    this._ParagraphsFlags = null;
                    DomElementList elements = this.Elements;
                    if (this._PrivateContent == null)
                    {
                        if (_Cached_PrivateContent != null)
                        {
                            this._PrivateContent = new DomElementList();
                        }
                        else
                        {
                            this._PrivateContent = new DomElementList(elements.Count);
                        }
                    }
                    else
                    {
                        this._PrivateContent.IsNone = false;
                    }
                    var addChildDirect = false;
                    if (this.OnlyHasCharOrParagraphFlag())// cet == (DCChildElementType.Char | DCChildElementType.ParagraphFlag))
                    {
                        // 在很多情况下，容器元素只包含字符和段落符号，则判断是否要快速创建
                        addChildDirect = true;
                        var arr2 = this._Elements.InnerGetArrayRaw();
                        for (var iCount = this._Elements.Count - 1; iCount >= 0; iCount--)
                        {
                            if (arr2[iCount]._RuntimeVisible == false)
                            {
                                addChildDirect = false;
                                break;
                            }
                        }
                    }
                    if (addChildDirect)
                    {
                        // 直接添加所有子元素
                        this._PrivateContent.Clear();
                        this._PrivateContent.AddRangeByDCList(this._Elements);
                        this.HasFieldElement = false;
                        int startIndex = 0;
                        int pcCount = this._PrivateContent.Count;
                        var pArray = this._PrivateContent.InnerGetArrayRaw();
                        for (int iCount = 0; iCount < pcCount; iCount++)
                        {
                            var element = pArray[iCount];
                            element._PrivateContentIndex = iCount;
                            if (element is DomParagraphFlagElement flag)
                            {
                                flag.ParagraphFirstContentElement = pArray[startIndex];
                                startIndex = iCount + 1;
                            }//if
                        }//for
                    }
                    else
                    {
                        var args3 = new AppendViewContentElementArgs(
                            document,
                            _Cached_PrivateContent == null ? this._PrivateContent : _Cached_PrivateContent,
                            true);
                        args3.HasFieldElements = false;
                        this.AppendViewContentElement(args3);
                        if (_Cached_PrivateContent != null)
                        {
                            this._PrivateContent.Clear();
                            this._PrivateContent.AddRangeByDCList(_Cached_PrivateContent);
                            _Cached_PrivateContent.FastClear();
                        }
                        this.HasFieldElement = args3.HasFieldElements;
                        if (elements.Count > 0 && (this._PrivateContent.LastElement is DomParagraphFlagElement) == false)
                        {
                            var le = elements.LastElement;// [elements.Count - 1];
                            if (this._PrivateContent.LastIndexOf(le) < 0)// .IndexOfFromLast( le ) < 0 )
                            {
                                // 将最后一个段落符号强制添加到列表中。
                                le._RuntimeVisible = true;
                                this._PrivateContent.FastAdd2(le);
                            }
                        }
                        // 更新段落首元素和序号
                        int startIndex = 0;
                        int pcCount = this._PrivateContent.Count;
                        //var pefIndexs = new List<int>();
                        //__GlobalPEFIndexs.FastClear();
                        var pArray = this._PrivateContent.InnerGetArrayRaw();
                        this.CheckChildElementStatsReady();
                        var hasObjectElement = this.HasChildObjectElement || this.HasChildContainerElement;// this.HasChildElement(DCChildElementType.Object) || this.HasChildElement( DCChildElementType.Container );
                        for (int iCount = 0; iCount < pcCount; iCount++)
                        {
                            var element = pArray[iCount];
                            element._PrivateContentIndex = iCount;
                            if (element is DomParagraphFlagElement flag)
                            {
                                //__GlobalPEFIndexs.Add(iCount);
                                flag.ParagraphFirstContentElement = pArray[startIndex];
                                startIndex = iCount + 1;
                            }//if
                            else if (hasObjectElement && element is DomObjectElement)
                            {
                                this.HasObjectElementInPrivateContent = true;
                            }
                        }//for
                    }
                }
                return this._PrivateContent;
            }
        }


        /// <summary>
        /// 内部的文档加载后的处理
        /// </summary>
        /// <param name="objArgs">事件参数</param>
        public override void AfterLoad(ElementLoadEventArgs args)
        {

            this._PrivateLines = null;
            this._PrivateContent = null;
            base.AfterLoad(args);
            this.FixElements();
        }


        /// <summary>
        /// 清空文档内容
        /// </summary>
        public virtual void Clear()
        {
            this._PrivateContent = null;
            this._PrivateLines = null;
            this.Elements.Clear();
            if (this.OwnerDocument != null)
            {
                this.IncreaseDocumentLayoutVersion();
                this.Elements.Add(this.OwnerDocument.CreateElement<DomParagraphFlagElement>());
            }
            //myElements.Add( this.myEOFElement );
            this.UpdateContentElements(true);
            this.InnerExecuteLayout();
        }

        /// <summary>
        /// 删除子元素
        /// </summary>
        /// <param name="element">元素对象</param>
        /// <returns>操作是否成功</returns>
        public override bool RemoveChild(DomElement element)
        {
            if (element == this.Elements.LastElement)
            {
                return false;
            }
            this.IncreaseDocumentLayoutVersion();
            return base.RemoveChild(element);
        }

        public override void FixDomState()
        {

            base.FixDomState();
            FixElements();
        }

        /// <summary>
        /// 修正元素内容
        /// </summary>
        public virtual void FixElements()
        {
            if (this._Elements == null)
            {
                this._Elements = new DomElementList();
            }
            if (this._Elements.LastElementIsXTextParagraphFlagElement() == false)// .LastElement is XTextParagraphFlagElement) == false)
            {
                DomParagraphFlagElement flag = new DomParagraphFlagElement();
                flag.AutoCreate = true;
                flag.SetParentAndDocumentRaw(this);
                if (this._Elements.Capacity == 0)
                {
                    this._Elements.InnerSetArrayRaw(new DomElement[] { flag }, 1);
                }
                else
                {
                    //this._IsEmpty = false;
                    this._Elements.FastAdd2(flag);
                }
                this.IncreaseDocumentLayoutVersion();
            }
        }


        /// <summary>
        /// 执行UpdateContentElements使用的参数
        /// </summary>
        internal class UpdateContentElementsArgs
        {
            private bool _SimpleTextMode = false;
            /// <summary>
            /// 简单的纯文本操作模式，新增，删除纯文本内容。此时可以进行一些优化操作。
            /// </summary>
            public bool SimpleTextMode
            {
                get { return _SimpleTextMode; }
                set { _SimpleTextMode = value; }
            }

            private int _PrivateStartContentIndex = -1;
            /// <summary>
            /// 开始刷新的元素在私有内容列表中的序号
            /// </summary>
            public int PrivateStartContentIndex
            {
                //get { return _PrivateStartContentIndex; }
                set { _PrivateStartContentIndex = value; }
            }

            private bool _FastMode = false;
            /// <summary>
            /// 快速执行模式
            /// </summary>
            public bool FastMode
            {
                get { return _FastMode; }
                set { _FastMode = value; }
            }

            private bool _UpdateElementsVisible = true;
            /// <summary>
            /// 更新文档元素可见性
            /// </summary>
            public bool UpdateElementsVisible
            {
                get { return _UpdateElementsVisible; }
                set { _UpdateElementsVisible = value; }
            }

            private bool _FromReplaceElements = false;
            /// <summary>
            /// 在XTextDocument.ReplaceElements()中调用的本功能
            /// </summary>
            public bool FromReplaceElements
            {
                get { return _FromReplaceElements; }
                set { _FromReplaceElements = value; }
            }
            ///// <summary>
            ///// 更新上级节点的可见性
            ///// </summary>
            //public bool UpdateParentElementVisible = false ;

            private bool _UpdateParentContentElement = true;
            /// <summary>
            /// 更新上级节点内容
            /// </summary>
            public bool UpdateParentContentElement
            {
                get { return _UpdateParentContentElement; }
                set { _UpdateParentContentElement = value; }
            }
            /// <summary>
            /// 更新下级节点的内容
            /// </summary>
            public bool UpdateChildContentElement = false;
        }

        /// <summary>
        /// 更新文档内容元素列表
        /// </summary>
        public virtual void UpdateContentElements(bool updateParentContentElement)
        {
            UpdateContentElementsArgs args = new UpdateContentElementsArgs();
            args.UpdateParentContentElement = updateParentContentElement;
            args.FastMode = false;
            args.UpdateElementsVisible = true;
            UpdateContentElements(args);
        }

        internal virtual void UpdateContentElementsForControlOnLoad()
        {
            this._PrivateContent = new DomElementList(1);
            this._PrivateContent.FastAdd2(this._Elements[0]);
        }

        /// <summary>
        /// 更新文档内容元素列表
        /// </summary>
        /// <param name="args">参数</param>
        internal virtual void UpdateContentElements(UpdateContentElementsArgs args)
        {
            if (args.UpdateElementsVisible)
            {
                InnerUpdateElementsRuntimeVisible(
                    UpdateElementsRuntimeVisibleArgs.Create(
                        this.OwnerDocument,
                        args.FromReplaceElements == false));
            }
            var document = this.OwnerDocument;
            if (document != null)
            {
                if (document.IsLoadingDocument == false)
                {
                    document.UpdateContentVersion();
                    document.IncreaseLayoutVersion();
                }
            }
            this._RenderBorderInfos = null;
            ClearPrivateContent();
            //this._PrivateLines = null;
            //if (this._PrivateLines != null)
            //{
            //    this._PrivateLines.Clear();
            //}
            if (args.FastMode == false)
            {
                this.FixElements();
                //this.RefreshParagraphState(null);
            }
            if (args.UpdateParentContentElement)
            {
                // 更新父元素的文档内容元素列表
                DomElement element = this.Parent;
                while (element != null)
                {
                    if (element is DomContentElement)
                    {
                        DomContentElement c = (DomContentElement)element;
                        //args.UpdateElementsVisible = args.UpdateParentElementVisible;

                        //if( args.UpdateParentElementVisible)
                        //{

                        //}
                        if (args.FromReplaceElements)
                        {
                            // 由ReplaceElements()调用的，无需更新上级元素的内容可见性
                            args.UpdateElementsVisible = false;
                        }
                        c.UpdateContentElements(args);
                    }
                    element = element.Parent;
                }// while
            }
            if (args.UpdateChildContentElement)
            {
                // 更新子孙节点的内容
                InnerUpdateChildContentElement(this);
            }
        }
        private static void InnerUpdateChildContentElement(DomContentElement rootElement)
        {
            rootElement.ClearPrivateContent();
            rootElement._ContentVersion++;
            rootElement.ResetChildElementStats();
            var array = rootElement.Elements.InnerGetArrayRaw();
            var len = rootElement.Elements.Count;
            for (var iCount = 0; iCount < len; iCount++)// rootElement.Elements.Count -1 ; iCount >=0;iCount --)
            {
                var item = array[iCount];
                if (item is DomContainerElement)
                {
                    if (item is DomTableElement)
                    {
                        // 特别处理表格
                        var table = (DomTableElement)item;
                        table.LayoutInvalidate = true;
                        foreach (var row in table.Rows.FastForEach())
                        {
                            foreach (DomTableCellElement cell in row.Elements.FastForEach())
                            {
                                cell.LayoutInvalidate = true;
                                InnerUpdateChildContentElement(cell);
                            }
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 内容垂直对齐方式
        /// </summary>
        public virtual VerticalAlignStyle ContentVertialAlign
        {
            get
            {
                return VerticalAlignStyle.Top;
            }
        }

        /// <summary>
        /// 刷新内容
        /// </summary>
        /// <param name="StartIndex">刷新区域起始编号</param>
        /// <returns>操作是否成功</returns>
        public bool RefreshPrivateContent(int StartIndex)
        {
            return RefreshPrivateContent(StartIndex, -1, false);
        }

        /// <summary>
        /// 调用 RefreshPrivateContent 函数中是否需要重新分页的标志
        /// </summary>
        public bool NeedRefreshPage
        {
            get { return this.StateFlag16; }
            set { this.StateFlag16 = value; }
        }

        /// <summary>
        /// 刷新私有内容
        /// </summary>
        /// <param name="StartIndex">需要刷新区域的起始序号</param>
        /// <param name="EndIndex">需要刷新区域的结束序号</param>
        /// <param name="fastMode">快速模式</param>
        /// <returns></returns>
        public virtual bool RefreshPrivateContent(int StartIndex, int EndIndex, bool fastMode)
        {
            return RefreshPrivateContent(StartIndex, EndIndex, fastMode, true);
        }
        /// <summary>
        /// 不检查停止行的标记值
        /// </summary>
        internal const int FlagNotCheckStopLine = -10000;
        /// <summary>
        /// 刷新私有内容
        /// </summary>
        /// <param name="StartIndex">需要刷新区域的起始序号</param>
        /// <param name="EndIndex">需要刷新区域的结束序号</param>
        /// <param name="fastMode">快速模式</param>
        /// <param name="updateTextCaret">是否更新光标位置</param>
        /// <returns></returns>
        public virtual bool RefreshPrivateContent(int StartIndex, int EndIndex, bool fastMode, bool updateTextCaret)
        {
            bool checkStopLine = StartIndex != FlagNotCheckStopLine;
            NeedRefreshPage = false;
            var privateContent = this.PrivateContent;
            StartIndex = privateContent.FixElementIndex(StartIndex);
            DomDocument document = this.OwnerDocument;
            bool fix = true;
            DomElement element = privateContent[StartIndex];
            if (element.OwnerLine != null)
            {
                if (element.OwnerLine.IndexOf(element) == 0)
                {
                    int index = this.PrivateLines.IndexOf(element.OwnerLine);
                    if (index > 0)
                    {
                        DomLine line2 = this.PrivateLines[index - 1];
                        if (line2.HasLineEndElement)
                        {
                            DomElement le = line2.LastElement;
                            if (privateContent.Contains(le))
                            {
                                fix = false;
                            }
                        }//if
                    }//if
                }//if
            }//if
            if (fix)
            {
                if (StartIndex > 0)
                {
                    StartIndex--;
                }
            }
            StartIndex = privateContent.FixElementIndex(StartIndex);
            DomElement EndElement = privateContent.SafeGet(EndIndex);
            float heightBack = this.Height;
            ParticalRefreshLinesEventArgs args = new ParticalRefreshLinesEventArgs(
                privateContent[StartIndex],
                EndElement,
                this.ContentVertialAlign);
            args.CheckStopLine = checkStopLine;
            if (this.ParticalRefreshLines(args))
            {
                ElementStack.ClearLastCache();
                this.DocumentContentElement.UpdateLineIndex(0);
                bool refreshPage = !document.PageRefreshed;// false;
                if (this is DomTableCellElement)
                {
                    if (this.SplitByPageLine())
                    {
                        refreshPage = true;
                    }
                    // 若当前对象是表格单元格，则向上调整表格和更上级的表格的排版。
                    DomTableCellElement cell2 = (DomTableCellElement)this;
                    if (cell2.RowSpan == 1 && cell2.OwnerRow.SpecifyHeight < 0)
                    {
                        // 该单元格是固定高度，表格无需重新排版
                    }
                    else
                    {
                        // 对于单元格需要调整表格和更上级的表格的排版
                        DomElement parent = this;
                        while (parent != null)
                        {
                            if (parent is DomTableCellElement)
                            {
                                DomTableCellElement cell = (DomTableCellElement)parent;
                                DomTableElement table = cell.OwnerTable;
                                if (table.OwnerLine == null)
                                {
                                    return false;
                                }
                                if (cell != this)
                                {
                                    heightBack = cell.Height;
                                }
                                if (cell != this)
                                {
                                    cell.Height = 0;
                                    cell.Width = 0;
                                }
                                table.ExecuteLayout(true);
                                if (cell.Height != heightBack)
                                {
                                    table.OwnerLine.RefreshStateNew();
                                    table.OwnerLine.InvalidateState = true;
                                    table.ContentElement.SetInvalidateContentHeightIncludeParent();
                                    refreshPage = true;
                                }
                            }
                            parent = parent.Parent;
                        }
                        //XTextTableCellElement cell = (XTextTableCellElement)this;
                        //XTextTableElement table = cell.OwnerTable;
                        //float heightBack = cell.Height;
                        //table.ExecuteLayout();
                        if (refreshPage)// cell.Height != heightBack)
                        {
                            // 单元格内容发生改变导致单元格高度发生改变，此时就刷新整个文档的分页设置
                            refreshPage = true;
                            this.DocumentContentElement.UpdateLinePosition(
                                this.DocumentContentElement.ContentVertialAlign,
                                true,
                                true);
                        }
                    }
                }
                else
                {
                    refreshPage = true;
                    if (document.PageRefreshed == false)
                    {
                        refreshPage = true;
                    }
                }
                NeedRefreshPage = refreshPage;

                //tick = DCSoft.Common.CountDown.GetTickCountFloat() - tick;

                if (refreshPage)
                {
                    document.PageRefreshed = false;

                    if (fastMode == false)
                    {
                        if (document != null)
                        {
                            document.RefreshPages();
                        }
                        if (document.EditorControl != null)
                        {
                            document.EditorControl.UpdatePages();
                            if (updateTextCaret)
                            {
                                document.EditorControl.UpdateTextCaret();
                            }
                            document.InnerViewControl.Invalidate();
                        }
                    }
                }
                //else
                //{
                //    if (this is XTextTableCellElement)
                //    {
                //        this.FixLinePositionForPageLine();
                //    }
                //}
            }
            else
            {
                //tick = DCSoft.Common.CountDown.GetTickCountFloat() - tick;
                if (fastMode == false && document != null)
                {
                    if (document.PageRefreshed == false)
                    {
                        document.RefreshPages();
                    }
                    if (document != null
                        && document.EditorControl != null)
                    {
                        if (updateTextCaret)
                        {
                            document.EditorControl.UpdateTextCaret();
                        }
                        //tick = DCSoft.Common.CountDown.GetTickCountFloat();
                        ////this.OwnerDocument.EditorControl.Update();
                        //tick = DCSoft.Common.CountDown.GetTickCountFloat() - tick;
                    }
                }
            }
            ElementStack.ClearLastCache();
            DCObjectPool<DomLine>.Clear(5);
            // tick = DCSoft.Common.CountDown.GetTickCountFloat() - tick;
            return true;
        }
        /// <summary>
        /// 是否被分页线分割
        /// </summary>
        /// <returns></returns>
        private bool SplitByPageLine()
        {
            var pages = this._OwnerDocument?.Pages;
            if (pages != null && pages.Count > 0)
            {
                var pos = this.AbsTop;
                var btn = pos + this.Height;
                foreach (var page in pages)
                {
                    if (page.Top > pos && page.Top < btn)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// 针对若干个元素状态发生改变而刷新文档
        /// </summary>
        /// <param name="list">元素列表</param>
        /// <param name="PreserveSelection">是否保存选择状态</param>
        /// <param name="fastMode">是否是快速模式</param>
        /// <remarks>若某些操作导致文档中部分元素视图或大小发生改变但并未新增或删除文档元素,则可以
        /// 使用本函数来高效率的更新文档而不必刷新整个文档的视图.</remarks>
        public void RefreshContentByElements(
            DomElementList list,
            bool PreserveSelection,
            bool fastMode)
        {
            if (list == null || list.Count == 0)
            {
                return;
            }
            int StartIndex = int.MaxValue;
            int EndIndex = -1;
            //XTextElement LastElement = null;
            using (DCGraphics g = this.InnerCreateDCGraphics())
            {
                InnerDocumentPaintEventArgs dpArgs = this.OwnerDocument.CreateInnerPaintEventArgs(g);
                foreach (DomElement element in list.FastForEach())
                {
                    int elementIndex = this.PrivateContent.IndexOf(element);
                    if (elementIndex < 0)
                    {
                        throw new Exception("Element no in content");
                    }
                    if (StartIndex > elementIndex)
                    {
                        StartIndex = elementIndex;
                    }
                    if (element.SizeInvalid || element.ViewInvalid)
                    {
                        element.InvalidateView();
                    }
                    if (element.SizeInvalid)
                    {
                        //this.OwnerDocument.Render.RefreshSize(element, g);
                        element.RefreshSize(dpArgs);
                        element.InvalidateView();
                    }
                    element.ViewInvalid = false;
                    if (elementIndex > 0 && elementIndex > EndIndex)
                    {
                        EndIndex = elementIndex;
                    }
                    //LastElement = element;
                }//foreach
                this.OwnerDocument.ContentStyles.UpdateState(g);
                //foreach (DocumentContentStyle style in this.OwnerDocument.ContentStyles.Styles)
                //{
                //    style.UpdateState(g);
                //}
            }//using

            this.RefreshPrivateContent(StartIndex, EndIndex, fastMode);
            if (PreserveSelection == false)
            {
                this.DocumentContentElement.Content.AutoClearSelection = true;
                this.DocumentContentElement.Content.MoveToPosition(StartIndex);
            }
        }

        /// <summary>
        /// 对整个内容执行重新排版操作
        /// </summary>
        public override void InnerExecuteLayout()
        {
            var doc = this.OwnerDocument;
            if (doc == null)
            {
                return;
            }
            this.Width = doc.PageSettings.ViewClientWidth;
            this._PrivateLines?.Clear();
            if (this._PrivateContent != null)
            {
                foreach (DomElement element in this._PrivateContent.FastForEach())
                {
                    element.SetOwnerLine(null);
                }
            }
            ParticalRefreshLinesEventArgs args = new ParticalRefreshLinesEventArgs(null, null, this.ContentVertialAlign);
            this.ParticalRefreshLines(args);
        }

        /// <summary>
        /// 根据文档内容高度来设置本文档元素的高度
        /// </summary>
        public virtual void UpdateHeightByContentHeight()
        {
            this.InvalidateContentHeight = false;
            this.Height = this.ContentHeight;
        }

        /// <summary>
        /// 声明文档内容高度发生改变，需要更新容器元素高度
        /// </summary>
        internal bool InvalidateContentHeight
        {
            get { return this.StateFlag14; }
            set { this.StateFlag14 = value; }
        }

        internal void SetInvalidateContentHeightIncludeParent()
        {
            DomElement element = this;
            while (element != null)
            {
                if (element is DomContentElement)
                {
                    ((DomContentElement)element).InvalidateContentHeight = true;
                }
                element = element.Parent;
            }
        }

        /// <summary>
        /// 运行时的上下内边距为0的标记
        /// </summary>
        internal bool ZeroRuntimePaddingTopBottom
        {
            get
            {
                return this.StateFlag20;
            }
            set
            {
                this.StateFlag20 = value;
            }
        }

        public override float ClientHeight
        {
            get
            {
                if (this.ZeroRuntimePaddingTopBottom)
                {
                    return this.Height;
                }
                else
                {
                    var rs = this.RuntimeStyle;
                    return this.Height - rs.PaddingTop - rs.PaddingBottom;// this._RuntimePaddingTop - this._RuntimePaddingBottom;
                }
            }
            set
            {
                if (this.ZeroRuntimePaddingTopBottom)
                {
                    this.Height = value;
                }
                else
                {
                    var rs = this.RuntimeStyle;
                    this.Height = value + rs.PaddingTop + rs.PaddingBottom;
                }
                //    this.Height = value + this._RuntimePaddingTop + this._RuntimePaddingBottom;
            }
        }
        /// <summary>
        /// 曾经由于为了分页线不切字而微调了文档行的位置。
        /// </summary>
        [NonSerialized]
        private bool _Existed_FixLinePositionForPageLine = false;
        internal void ClearFixLinePositionForPageLine()
        {
            if (this._PrivateLines != null
                && this._PrivateLines.Count > 0
                && this._Existed_FixLinePositionForPageLine)
            {
                this._Existed_FixLinePositionForPageLine = false;
                foreach (var line in this._PrivateLines.FastForEach())
                {
                    if (line.Top != line.NativeTop)
                    {
                        line.Top = line.NativeTop;
                        line.ResetAbsLocation();
                    }
                }
            }
        }

        /// <summary>
        /// 为分页线而微小的修正文档行的位置
        /// </summary>
        /// <returns>操作是否导致了文档行的位置发生改变</returns>
        internal bool FixLinePositionForPageLine(float pagePos)
        {
            this._Existed_FixLinePositionForPageLine = false;
            var pvLines = this.PrivateLines;
            if (pvLines == null || pvLines.Count == 0)
            {
                // 没有文档行，状态不对，不处理。
                return false;
            }
            //XTextDocument document = this.OwnerDocument;
            var thisAbsTop = this.AbsTop;
            var thisAbsBottom = thisAbsTop + this.Height;
            if (pagePos < 0)
            {
                var pages = this.OwnerDocument.Pages;
                for (var iCount = pages.Count - 2; iCount >= 0; iCount--)
                {
                    // 反向遍历所有的文档页，判断是否跨过分页线
                    var pos2 = pages[iCount].Bottom;
                    if (pos2 > thisAbsTop && pos2 < thisAbsBottom)
                    {
                        pagePos = pos2;
                        break;
                    }
                    else if (pos2 < thisAbsTop)
                    {
                        return false;
                    }
                }
                foreach (DomLine line in pvLines.FastForEach())
                {
                    line.Top = line.NativeTop;
                }
            }
            int lineIndex = 0;
            bool result = false;
            // 分页线位置命中文档元素内容
            float topSpacing = pvLines[0].Top;
            float bottomSpacing = this.Height - pvLines.LastLine.Bottom;
            if (topSpacing <= 0 && bottomSpacing <= 0)
            {
                // 没有腾挪的空间，不处理
                return false;
            }
            //float lastSpacing = 0;
            //int pvLinesCount = pvLines.Count;
            //if (pvLinesCount > 0)
            //{
            //    lastSpacing = this._RuntimePaddingTop /*this.RuntimeStyle.PaddingTop*/ + this.ClientHeight - pvLines.LastLine.Bottom;
            //}
            var cellAbsTop = this.AbsTop;
            var pvLinesCount = pvLines.Count;
            for (int iCount = lineIndex; iCount < pvLinesCount; iCount++)
            {
                DomLine line = pvLines[iCount];
                var lineAbsTop = cellAbsTop + line.Top;
                if (pagePos >= lineAbsTop + 5
                    && pagePos <= lineAbsTop + line.Height - 3)
                {
                    // 分页线跨越了文档行，需要进行文档行位置的修正
                    // 将后续的文档行向下移动
                    if (pagePos - lineAbsTop - bottomSpacing < lineAbsTop + line.Height - pagePos - topSpacing)
                    {
                        // 向下移动效果大些
                        float posFix = Math.Min(pagePos - lineAbsTop, bottomSpacing);
                        for (var iCount2 = iCount; iCount2 < pvLinesCount; iCount2++)
                        {
                            var line2 = pvLines[iCount2];
                            line2.Top += posFix;
                            line2.ResetAbsLocation();
                            result = true;
                        }
                    }
                    else
                    {
                        // 向上移动效果大些
                        float posFix = Math.Min(lineAbsTop + line.Height - pagePos, topSpacing);
                        for (var iCount2 = 0; iCount2 <= iCount; iCount2++)
                        {
                            var line2 = pvLines[iCount2];
                            line2.Top -= posFix;
                            line2.ResetAbsLocation();
                            result = true;
                        }
                    }
                    this._Existed_FixLinePositionForPageLine = true;
                    break;
                }
            }//for
            return result;
        }

        internal static bool _UseLineInfo_UpdateLinePosition = true;
        /// <summary>
        /// 更新文档行位置
        /// </summary>
        /// <param name="align">文档行垂直对齐方式</param>
        /// <param name="refreshLineState">刷新行状态</param>
        /// <param name="deeply">是否深入更新</param>
        /// <returns>是否有文档行的位置发生改变</returns>
        internal virtual bool UpdateLinePosition(
            VerticalAlignStyle align,
            bool refreshLineState,
            bool deeply,
            bool updateForHiddenLineOnly = false)
        {
            var pvLines = this._PrivateLines;
            if (pvLines == null || pvLines.Count == 0)
            {
                // 没有文档行，不进行处理
                return false;
            }
            if (this.Height == 0f)
            {
                // 高度为0，无法处理
                return false;
            }
            this._RenderBorderInfos = null;
            if (this is DomDocumentContentElement)
            {
            }
            DomDocument document = this.OwnerDocument;
            // 记下旧的文档行位置和高度
            float[] oldLinesInfo = null;
            if (_UseLineInfo_UpdateLinePosition)
            {
                if (document.States.Printing == false
                    && document.States.PrintPreviewing == false
                    && document.AllowInvalidateForUILayoutOrView())
                {
                    oldLinesInfo = new float[pvLines.Count * 2];
                    for (int iCount = 0; iCount < pvLines.Count; iCount++)
                    {
                        DomLine line = (DomLine)pvLines[iCount];
                        oldLinesInfo[iCount * 2] = line.NativeTop;
                        oldLinesInfo[iCount * 2 + 1] = line.Height;
                    }
                }
            }
            // 计算总行高
            float totalHeight = 0;
            var pvLinesCount = pvLines.Count;
            var pvArray = pvLines.InnerGetArrayRaw();
            //foreach (XTextLine line in pvLines.FastForEach())
            for (var iCount = 0; iCount < pvLinesCount; iCount++)
            {
                var line = pvArray[iCount];// pvLines.GetItemFast(iCount);
                {
                    if (updateForHiddenLineOnly && deeply)
                    {
                        // 为隐藏行而深入更新
                        if (line.IsTableLine)
                        {
                            var table = line.TableElement;
                            table.UpdateCellsState(false, true);
                            line.RefreshStateNew();
                        }
                    }
                    totalHeight += line.Height;// totalHeight = totalHeight + line.Height;
                }
            }
            // 计算行开始位置
            float topCount = 0;
            float paddingTop = 0;
            float paddingBottom = 0;
            var gridLine = this.InnerRuntimeGridLine();
            if (gridLine != null && (gridLine.Visible == false || gridLine.RuntimeGridSpan <= 0))
            {
                gridLine = null;
            }
            if (gridLine == null && this.ZeroRuntimePaddingTopBottom == false)
            {
                var rs = this.RuntimeStyle;
                paddingTop = rs.PaddingTop;
                paddingBottom = rs.PaddingBottom;
            }
            switch (align)
            {
                case VerticalAlignStyle.Top:
                    // 顶端位置
                    topCount = paddingTop;
                    break;
                case VerticalAlignStyle.Middle:
                    // 垂直居中对齐
                    topCount = (this.Height - paddingTop - paddingBottom - totalHeight) / 2.0f;
                    if (gridLine != null)
                    {
                        topCount = (float)Math.Round(topCount / gridLine.RuntimeGridSpan) * gridLine.RuntimeGridSpan;
                    }
                    topCount = paddingTop + Math.Max(0, topCount);
                    break;
                case VerticalAlignStyle.Bottom:
                    // 低端对齐
                    topCount = this.Height - paddingTop - paddingBottom - totalHeight;
                    if (gridLine != null)
                    {
                        topCount = (float)Math.Round(topCount / gridLine.RuntimeGridSpan) * gridLine.RuntimeGridSpan;
                    }
                    topCount = paddingTop + Math.Max(0, topCount);
                    break;
            }//switch

            // 累计计算文档行的位置
            bool bolHasHiddenLine = false;
            //foreach (XTextLine line in pvLines.FastForEach())
            for (var iCount = 0; iCount < pvLinesCount; iCount++)
            {
                var line = pvArray[iCount];// pvLines.GetItemFast(iCount);
                                           //if (line.IsTableLine)
                                           //{
                                           //    Console.WriteLine("");
                                           //}
                line.NativeTop = topCount;
                line.Top = topCount;
                topCount += line.Height;
                line.ResetAbsLocation();
            }//foreach

            bool result = false;
            if (updateForHiddenLineOnly)
            {
                result = bolHasHiddenLine;
            }
            else if (deeply)
            {
                // 递归处理表格单元格中的文档行位置
                for (var iCount = 0; iCount < pvLinesCount; iCount++)
                //foreach (XTextLine line in pvLines.FastForEach())
                {
                    var line = pvArray[iCount];
                    if (line.IsTableLine)
                    {
                        DomTableElement table = line.TableElement;
                        foreach (DomTableRowElement row in table.Rows)
                        {
                            if (row._RuntimeVisible)
                            {
                                foreach (DomTableCellElement cell in row.Cells)
                                {
                                    if (cell.RuntimeVisible)
                                    {
                                        if (cell.UpdateLinePosition(
                                            cell.RuntimeStyle.VerticalAlign,
                                            refreshLineState,
                                            deeply))
                                        {
                                            result = true;
                                        }
                                    }
                                }
                            }
                        }
                    }//if
                }//foreach
            }//if
            if (result == false && oldLinesInfo != null)
            {
                // 比较新旧文档行的位置是否发生改变
                for (int iCount = 0; iCount < pvLines.Count; iCount++)
                {
                    DomLine line = pvLines[iCount];
                    if (line.NativeTop != oldLinesInfo[iCount * 2]
                        || line.Height != oldLinesInfo[iCount * 2 + 1])
                    {
                        //  文档行的位置和旧的文档行的位置有不符合
                        foreach (DomElement element in line.FastForEach())
                        {
                            // 声明该文档行中所有的元素排版发生了改变
                            document.InvalidateLayout(element);
                        }
                        result = true;
                        //break;
                    }
                }//for
            }//if
            if (result || this.InvalidateContentHeight)
            {
                // 文档行的位置发生则更新文档元素的高度
                this.UpdateHeightByContentHeight();
            }
            return result;
        }

        /// <summary>
        /// 判断在占据整行的文档元素后能否强制添加元素
        /// </summary>
        /// <param name="element">文档元素对象</param>
        /// <returns>能否强制添加</returns>
        internal static bool CanAddElementForce(DomElement element)
        {
            //DocumentBehaviorOptions bo = element.GetDocumentBehaviorOptions();//.OwnerDocument.GetDocumentBehaviorOptions();
            if (element is DomFieldBorderElement)
            {
                DomFieldBorderElement fb = (DomFieldBorderElement)element;
                {
                    DomFieldElementBase field = (DomFieldElementBase)element.Parent;
                    if (fb == field.EndElement)
                    {
                        // 为后置文档域边界元素，而且文本为空
                        if (element.GetDocumentBehaviorOptions().CompressLayoutForFieldBorder)
                        {
                            // 可以强制添加后置文档域边界元素
                            return true;
                        }
                    }
                }
            }
            else if (element is DomParagraphFlagElement)
            {
                if (element.GetDocumentBehaviorOptions().ParagraphFlagFollowTableOrSection)
                {
                    // 可以强制添加段落符号
                    return true;
                }
            }
            return false;
        }


        /// <summary>
        /// 排版信息版本号
        /// </summary>
        internal int _LayoutVersion;

        /// <summary>
        /// 文档行的高度是否对齐到网格线高度
        /// </summary>
        internal bool AlignLineHeightToGridLine
        {
            get { return this.StateFlag12; }
            set { this.StateFlag12 = value; }
        }
        /// <summary>
        /// 打印时是否检查边框剪切矩形,0=尚未判断；1=不需要;2=需要
        /// </summary>
        private DCBooleanValueHasDefault RequireCheckClipBoundsForPrint
        {
            get
            {
                if (this.StateFlag18)
                {
                    if (this.StateFlag19)
                    {
                        return DCBooleanValueHasDefault.True;
                    }
                    else
                    {
                        return DCBooleanValueHasDefault.False;
                    }
                }
                else
                {
                    return DCBooleanValueHasDefault.Default;
                }
            }
            set
            {
                if (value == DCBooleanValueHasDefault.Default)
                {
                    this.StateFlag18 = false;
                }
                else if (value == DCBooleanValueHasDefault.True)
                {
                    this.StateFlag18 = true;
                    this.StateFlag19 = true;
                }
                else if (value == DCBooleanValueHasDefault.False)
                {
                    this.StateFlag18 = true;
                    this.StateFlag19 = false;
                }
            }
        }

        /// <summary>
        /// 进行部分分行操作
        /// </summary>
        /// <param name="args">参数</param>
        /// <returns>操作是否导致文档需要重新分页</returns>
        internal virtual bool ParticalRefreshLines(ParticalRefreshLinesEventArgs args)
        {
            this.RequireCheckClipBoundsForPrint = DCBooleanValueHasDefault.Default;
            //_VersionIndex_ParticalRefreshLines++;
            args._CreateLine_LastParagraphElement = null;
            this._LayoutVersion++;
            this.AlignLineHeightToGridLine = false;
            this.ZeroRuntimePaddingTopBottom = false;
            //this._RuntimePaddingTop = this.RuntimeStyle.PaddingTop;
            //this._RuntimePaddingBottom = this.RuntimeStyle.PaddingBottom;
            var document = this.OwnerDocument;
            if (document == null)
            {
                var jiejie = "JIEJIE.NET.SWITCH:-controlfow";
                args.HasCalculate = false;
                return false;
            }
            if (document.FixLayoutForPrint)
            {
                throw new Exception(DCSR.FixLayoutForPrint);
            }
            document.IncreaseLayoutVersion();

            // 内容只有一个段落符号的空白容器元素
            var thisElements = this.Elements;
            var isBlankContentElement = thisElements.Count == 1
                && thisElements[0] is DomParagraphFlagElement
                && thisElements[0]._StyleIndex < 0;
            //if (this is XTextTableCellElement && ((XTextTableCellElement)this).CellID == "C1")
            //{
            //}
            args.FastClientWidth = this.ClientWidth;
            var dce = this.DocumentContentElement;
            if (dce is DomDocumentBodyElement)
            {
                ((DomDocumentBodyElement)dce).ClearLineZones();
            }
            var irgl = this.InnerRuntimeGridLine();
            if (irgl != null)
            {
                if (irgl != document._GlobalGridInfo)
                {
                    irgl.UpdateRuntimeGridSpan(
                        document.GetStandartPapeViewHeight(document.PageSettings),
                        DCSystem_Drawing.GraphicsUnit.Document,
                        1);
                }
                var thisRS = this.RuntimeStyle;
                this.AlignLineHeightToGridLine = irgl.RuntimeAlignToGridLine;
                if (this.AlignLineHeightToGridLine
                    && thisRS.PaddingBottom > 0
                    && thisRS.PaddingTop > 0
                    && document.GetDocumentBehaviorOptions().IgnoreTopBottomPaddingWhenGridLineLayout)
                {
                    this.ZeroRuntimePaddingTopBottom = true;
                    //this._RuntimePaddingBottom = 0;
                    //this._RuntimePaddingTop = 0;
                }
            }
            if (this.ZeroRuntimePaddingTopBottom == false)
            {
                var rs3 = this.RuntimeStyle;
                if (rs3.PaddingTop == 0 && rs3.PaddingBottom == 0)
                {
                    this.ZeroRuntimePaddingTopBottom = true;
                }
            }
            DomElement startElement = args.StartElement;
            DomElement endElement = args.EndElement;
            VerticalAlignStyle vertialAlign = args.VertialAlign;
            args._CreateLine_LastFirstIndex = 0;
            this._RenderBorderInfos = null;
            //float tick = DCSoft.Common.CountDown.GetTickCountFloat();
            //document.ContentStyles.ResetFastValue();
            this.LayoutInvalidate = false;
            // 是否是正文块,若为正文块，可能导致重新分页。
            bool isBodyContent = true;
            if (this.Width == 0)
            {
                // 宽度为0
                if (this is DomDocumentContentElement)
                {
                    this.Width = document.PageSettings.ViewClientWidth;
                }
            }
            else if (this.Width < 0)
            {
                this.Width = document.PageSettings.ViewClientWidth;
            }
            DomElementList privateContent = this.PrivateContent;
            DomLineList privateLines = this.PrivateLines;
            if (privateContent.Count == 0)
            {
                privateLines.Clear();
                args.HasCalculate = false;
                //this.Height = 0;
                return isBodyContent;
            }

            // 本容器元素中唯一的参与排版的元素对象
            DomElement theOnlyElement = privateContent.Count == 1 ? privateContent[0] : null;
            if (theOnlyElement != null && theOnlyElement._StyleIndex >= 0)
            {
                theOnlyElement = null;
            }
            // 是否是全局的刷新文档行信息
            bool globalRefreshLines = startElement == null || privateLines.Count == 0;

            // 行号编号备份
            int[] lineIndexBack = null;
            // 对旧的行状态数据进行备份，以便重新分行后判断是否需要重新分页
            float[] linesInfoBack = null;
            var needReturnPrivateLines = false;
            if (DomDocument.IsExecuteingLayout)
            {
                if (privateLines.Capacity == 0)
                {
                    privateLines.InnerSetArrayRaw(LoaderListBuffer<DomLine[]>.Instance.Alloc(), 0);
                    needReturnPrivateLines = true;
                }
            }
            else
            {
                if (privateLines.Count > 0 && args.ForceRefreshAllContent == false)
                {
                    var len = privateLines.Count;
                    lineIndexBack = new int[len * 2];
                    linesInfoBack = new float[len * 2];
                    for (int iCount = 0; iCount < len; iCount++)
                    {
                        DomLine line = privateLines.GetItemFast(iCount);
                        linesInfoBack[iCount * 2] = line.NativeTop;
                        linesInfoBack[iCount * 2 + 1] = line.Height;

                        lineIndexBack[iCount * 2] = line.GlobalIndex;
                        lineIndexBack[iCount * 2 + 1] = line.IndexInPage;
                    }//for
                }
            }
            int endIndex = -1;
            if (endElement != null)
            {
                endIndex = privateContent.FastIndexOf(endElement);
            }
            int startIndex = 0;
            int startRefreshLineIndex = 0;
            int endRefreshLineIndex = privateLines.Count - 1;// plCount - 1;

            if (DomDocument.IsExecuteingLayout == false)
            {
                // 通篇刷新，省掉一些操作。
                if (theOnlyElement == null)
                {
                    if (startElement == null)
                    {
                        startElement = privateContent[0];
                    }
                    else
                    {
                        startIndex = privateContent.FastIndexOf(startElement);
                    }
                }
                else
                {
                    startElement = theOnlyElement;
                    startIndex = 0;
                }
                if (startIndex < 0)
                {
                    throw new System.Exception("未找到起始元素");
                }
                if (theOnlyElement == null && startIndex > 0)
                {
                    for (int iCount = startIndex; iCount >= 0; iCount--)
                    {
                        var ol = privateContent[iCount]._OwnerLine;
                        if (ol != null)
                        {
                            startRefreshLineIndex = privateLines.IndexOf(ol);
                            if (startRefreshLineIndex >= 0)
                            {
                                startRefreshLineIndex--;
                                if (startRefreshLineIndex < 0)
                                {
                                    startRefreshLineIndex = 0;
                                }
                                break;
                            }
                        }
                    }
                    if (startRefreshLineIndex < 0)
                    {
                        startRefreshLineIndex = 0;
                    }
                }
                //XTextLine startRefreshLine = null;
                //if (startIndex > 0)
                //{
                //    startRefreshLine = privateContent[startIndex - 1].OwnerLine;
                //}
                int plCount = privateLines.Count;
                if (endIndex >= 0)
                {
                    for (int iCount = endIndex; iCount < plCount; iCount++)
                    {
                        if (privateContent[iCount].OwnerLine != null)
                        {
                            endRefreshLineIndex = privateLines.IndexOf(privateContent[iCount].OwnerLine);
                            if (endRefreshLineIndex >= 0)
                            {
                                for (int iCount2 = endRefreshLineIndex; iCount2 < plCount; iCount2++)
                                {
                                    if (privateLines[iCount2].LastElement is DomParagraphFlagElement)
                                    {
                                        endRefreshLineIndex = iCount2;
                                        break;
                                    }
                                }
                                endRefreshLineIndex = Math.Min(endRefreshLineIndex + 2, plCount - 1);
                                break;
                            }
                        }
                    }
                }
                //用户删除的文档开头部分多行标记
                //bool DeleteHeadLines = false;
                if (startElement.OwnerLine != null)
                {
                    DomLine line2 = startElement.OwnerLine;
                    if (startIndex == 0)
                    {
                        // 检查旧的第一行是否被完整的删除了
                        if (privateLines.Count > 0)
                        {
                            DomLine line = privateLines[0];
                            bool fullDelete = true;
                            foreach (DomElement element in line.FastForEach())
                            {
                                if (privateContent.FastIndexOf(element) >= 0)
                                {
                                    fullDelete = false;
                                    break;
                                }
                            }
                            if (fullDelete)
                            {
                                startElement.SetOwnerLine(null);
                                privateLines.Clear();
                            }
                        }
                    }
                    else
                    {
                        foreach (DomElement element in line2.FastForEach())
                        {
                            int index = privateContent.IndexOfUsePrivateContentIndex(element);// .IndexOf(element);
                            if (index >= 0 && startIndex > index)
                            {
                                startIndex = index;// this.PrivateContent.IndexOf(element);
                                startElement = element;
                            }
                        }//foreach
                    }//else
                }//if
                else
                {
                    startIndex = 0;
                    //var index9 = privateContent.FastIndexOfForPrivateContent(startElement);
                    //if (index9 >= 0)
                    //{
                    //    for (var iCount = 0; iCount < 100; iCount++)
                    //    {
                    //        var line = privateContent.SafeGet(iCount + index9)?.OwnerLine;
                    //        if (line != null)
                    //        {
                    //            startIndex = privateContent.FastIndexOfForPrivateContent(line[0]);
                    //            break;
                    //        }
                    //    }
                    //}
                }
            }
            else
            {
                startElement = privateContent.FirstElement;
            }

            RuntimeDocumentContentStyle rs = this.RuntimeStyle;
            // 保存反复无常的元素的列表
            List<DomElement> freakElements = null;
            DomLine stopLine = null;
            var newLines = LoaderListBuffer<DomLineList>.Instance.Alloc(); //new XTextLineList();
            newLines.InnerSetArrayRaw(LoaderListBuffer<DomLine[]>.Instance.Alloc(), 0);
            DomLine newLine = null;
            if (theOnlyElement == null)
            {
                if (privateContent.GetTheSingleParagraphFlagElement() == null)
                {
                    freakElements = DCSoft.Common.LoaderListBuffer<List<DomElement>>.Instance.Alloc();// new List<XTextElement>();
                }
                var sline = privateContent[startIndex].OwnerLine;
                var firstPreline = privateLines.GetPreLine(sline);
                if (firstPreline != null && document._ReplaceElements_CurrentContainer != null)
                {
                    // 文档内容改变而导致内容排版，由于可能是插入段落符号，因此更新第一个文档行的所属段落符号。
                    var index99 = privateContent.IndexOfUsePrivateContentIndex(firstPreline.LastElement);
                    if (index99 >= 0)
                    {
                        var pf3 = privateContent.GetFirstParagraphFlag(index99);
                        if (pf3 != null && firstPreline.ParagraphFlagElement != pf3)
                        {
                            // 段落元素发生改变，更新旧的文档行的所属段落元素
                            pf3.ListItemElement = null;
                            var oldPF = firstPreline.ParagraphFlagElement;
                            for (var lineIndex3 = privateLines.FastIndexOf(firstPreline) - 1; lineIndex3 >= 0; lineIndex3--)
                            {
                                var line3 = privateLines[lineIndex3];
                                //var pf5 = privateContent.GetOwnerParagraphFlag(line3.LastElement);
                                if (line3.ParagraphFlagElement == oldPF)
                                {
                                    line3.ParagraphFlagElement = pf3;
                                }
                                else
                                {
                                    break;
                                }
                            }
                            firstPreline.ParagraphFlagElement = pf3;
                        }
                    }
                }
                newLine = CreateLine(args, firstPreline);
            }
            else
            {
                newLine = CreateLine(args, null);
            }
            newLines.Add(newLine);
            if (theOnlyElement != null)
            {
                newLine.InnerSetArrayRaw(new DomElement[] { theOnlyElement }, 1);
                //newLine.Capacity = 1;
                //newLine.FastAdd2(theOnlyElement);
                //this._IsOnlyOneParagraphFlag = theOnlyElement is XTextParagraphFlagElement;
            }
            else
            {
                if (newLine.Capacity == 0)
                {
                    newLine.InnerSetArrayRaw(LoaderListBuffer<DomElement[]>.Instance.Alloc(), 0);
                }
                //else
                //{
                //    var s = 1;
                //}
                //this._IsOnlyOneParagraphFlag = false;
                // 将参与分行计算的元素从列表形式转化为堆栈形式
                var myStack = new ElementStack(
                    privateContent,
                    startIndex,
                    privateContent.Count - startIndex);
                // 产生第一个新行对象标记
                bool first = true;
                while (myStack.Count > 0)
                {
                    bool bolNewLine = false;
                    var preElement = myStack.LastPopElement;
                    DomElement element = myStack.Pop();
                    element.CheckRuntimeStyle();
                    if (newLine.Count == 0)
                    {
                        // 文档行没有元素，肯定能添加文档元素
                        if (element is DomCharElement)
                        {
                            DomCharElement chr = (DomCharElement)element;
                            if (chr._CharValue == '\t')
                            {
                                // 文档行第一个字符就是制表符，计算其宽度
                                chr.SetWidthForTab();
                            }
                        }
                        newLine.AddElement(element, preElement);
                        if (LayoutHelper.OwnerHoleLine(element))
                        {
                            bolNewLine = true;
                            if (myStack.Count > 0)
                            {
                                if (CanAddElementForce(myStack.Peek()))
                                {
                                    bolNewLine = false;
                                }
                            }
                        }
                        else if (LayoutHelper.IsNewLine(element))
                        {
                            bolNewLine = true;
                        }
                    }
                    else
                    {
                        bool ownerHoleLine = LayoutHelper.OwnerHoleLine(element);
                        if (element is DomTableElement)
                        {
                            // 特别处理表格,若新增的文档行只有一个输入域边界元素,则还可以添加表格
                            if (newLine.Count == 1 && newLine[0] is DomFieldBorderElement)
                            {
                                ownerHoleLine = false;
                            }
                            else
                            {

                            }
                        }
                        if (ownerHoleLine)
                        {
                            myStack.Push(element);
                            bolNewLine = true;
                        }
                        else
                        {
                            if (newLine.AddElement(element, preElement) == false)
                            {
                                // 向文本行添加内容失败，说明文本行已经无法添加内容了，准备换行。
                                myStack.Push(element);
                                bolNewLine = true;
                                bool freakFlag = false;
                                if (newLine.Count > 1 && LayoutHelper.CanBeLineEnd(newLine.LastElement) == false)
                                {
                                    freakFlag = true;
                                    if (newLine.Count == 2 && newLine[0] is DomParagraphListItemElement)
                                    {
                                        // 遇到段落列表符号的特殊情况。
                                        freakFlag = false;
                                    }
                                }
                                if (freakFlag)
                                {
                                    // 文档行中最后一个元素不能放置在行尾，则提前换行
                                    DomElement lastElement = newLine.PopupLastElement();
                                    if (lastElement != null)
                                    {
                                        myStack.Push(lastElement);
                                        freakElements.Add(lastElement);
                                    }
                                }
                                else
                                {
                                    bool pushFlag = false;
                                    if (LayoutHelper.CanBeLineHead(element) == false
                                        || LayoutHelper.CanBeLineEnd(newLine.LastElement) == false)
                                    {
                                        pushFlag = true;
                                        if (newLine.Count == 2 && newLine[0] is DomParagraphListItemElement)
                                        {
                                            // 遇到段落列表符号的特殊情况
                                            pushFlag = false;
                                        }
                                    }
                                    if (pushFlag)
                                    {
                                        DomElement LastElement = newLine.PopupLastElement();
                                        if (LastElement != null)
                                        {
                                            myStack.Push(LastElement);
                                            // 此处由于修整行首字符和行尾字符导致已经加入本行的元素退出本行
                                            // 由于XTextLine.Add函数会设置元素的Left值，因此导致退出本行的元素
                                            // 退出本行但没有恢复Left值，因此换行操作后需要对这些元素所在的文本
                                            // 行重新进行行内排版操作,此处保存这些立场反复无常的元素，便于以后
                                            // 对所影响到的文本行进行内部排版操作
                                            freakElements.Add(LastElement);
                                            if (LayoutHelper.CanBeLineHead(LastElement) == false && newLine.Count > 2)
                                            {
                                                // 再搞一下
                                                LastElement = newLine.PopupLastElement();
                                                if (LastElement != null)
                                                {
                                                    myStack.Push(LastElement);
                                                    freakElements.Add(LastElement);
                                                }
                                            }
                                        }
                                    }
                                    DomCharElement c = myStack.Peek() as DomCharElement;
                                    // 判断是否要反悔而提前换行
                                    bool needDetectFreakLineBreak = false;
                                    if (c != null)
                                    {
                                        if (LayoutHelper.IsEnglishLetterOrDigit(c._CharValue))
                                        {
                                            needDetectFreakLineBreak = true;
                                        }
                                    }
                                    if (needDetectFreakLineBreak)
                                    {
                                        // 检查是否需要提前换行
                                        // 在当前行中向前搜索，判断能否执行提前换行
                                        float widthCount = 0;
                                        float contentWidth = newLine.ContentWidth;
                                        int chrElementCount = 0;
                                        int freakIndex = -1;
                                        {
                                            for (int iCount = newLine.Count - 1; iCount >= 0; iCount--)
                                            {
                                                if (newLine[iCount] is DomCharElement)
                                                {
                                                    char preChar = ((DomCharElement)newLine[iCount])._CharValue;
                                                    bool match = false;
                                                    if (LayoutHelper.IsEnglishLetterOrDigit(preChar))
                                                    {
                                                        match = true;
                                                    }
                                                    if (match)
                                                    {
                                                        widthCount = widthCount + newLine[iCount].Width;
                                                        chrElementCount++;
                                                        freakIndex = iCount;
                                                    }
                                                    else
                                                    {
                                                        break;
                                                    }
                                                }
                                                else if (newLine[iCount] is DomParagraphListItemElement)
                                                {
                                                    // 出现段落列表元素类型，该元素不能反悔提前换行。
                                                    freakIndex = 0;
                                                    break;

                                                }
                                                else
                                                {
                                                    break;
                                                }
                                            }//for
                                        }

                                        if (freakIndex > newLine.Count / 3
                                            && widthCount < contentWidth / 3)
                                        {
                                            // 提前换行
                                            for (int iCount = newLine.Count - 1; iCount >= freakIndex; iCount--)
                                            {
                                                DomElement element2 = newLine[iCount];
                                                newLine.RemoveAt(iCount);
                                                myStack.Push(element2);
                                                // 此处由于修整连续的英文单词和数字导致已经加入本行的元素退出本行
                                                // 由于XTextLine.Add函数会设置元素的Left值，因此导致退出本行的元素
                                                // 退出本行但没有恢复Left值，因此换行操作后需要对这些元素所在的文本
                                                // 行重新进行行内排版操作,此处保存这些立场反复无常的元素，便于以后
                                                // 对所影响到的文本行进行内部排版操作
                                                freakElements.Add(element2);
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (LayoutHelper.IsNewLine(element))
                                {
                                    bolNewLine = true;
                                }

                            }
                        }
                    }
                    DomCharElement CharElement = element as DomCharElement;
                    if (CharElement != null && CharElement._CharValue == '\t')
                    {
                        CharElement.SetWidthForTab();
                    }
                    if (bolNewLine)
                    {
                        if (first)
                        {
                            first = false;
                        }
                        else
                        {
                            if (privateLines.Count > 0)
                            {
                                if ((endIndex <= 0 || privateContent.FastIndexOfForPrivateContent(element) > endIndex)
                                    && endRefreshLineIndex >= 0)
                                {
                                    DomLine LastLine = newLines.LastLine;
                                    var privateLinesArray = privateLines.InnerGetArrayRaw();
                                    for (int iCount = endRefreshLineIndex; iCount >= startRefreshLineIndex; iCount--)
                                    {
                                        // 首先在小范围内搜索截止行
                                        DomLine oldLine = privateLinesArray[iCount];// privateLines.GetItemFast(iCount);
                                        if (oldLine.InvalidateState)
                                        {
                                            // 只要之前出现任何状态无效的行则不会命中截止行。
                                            break;
                                        }
                                        if (args.CheckStopLine
                                            && DomLine.CheckStopLine(LastLine, privateLinesArray[iCount]))
                                        {
                                            // 若已经存在的文本行和新建的最后一个文本行的内部设置一样,包括元素清单
                                            // 和行设置,而且该行所有元素的大小没有发生改变,则设置该旧行为停止行
                                            // 停止行以后的旧行就没有必要进行刷新操作,提前退出刷新行操作,避免不必要
                                            // 的工作量
                                            // 某些情况下某些位置处元素发生改变影响行分布,主调程序就使用EndIndex指明
                                            // 停止行必须在这些位置后出现,因此本段程序稍前进行了 EndIndex 判断操作
                                            stopLine = privateLinesArray[iCount];
                                            RefreshLineState(LastLine, args.ForceRefreshAllContent);
                                            goto EndAddLine;
                                        }//if
                                    }//for
                                    for (int iCount = privateLines.Count - 1; iCount >= endRefreshLineIndex; iCount--)
                                    {
                                        // 扩大范围搜索截止行
                                        DomLine oldLine = privateLines[iCount];
                                        if (oldLine.InvalidateState)
                                        {
                                            // 只要之前出现任何状态无效的行则不会命中截止行。
                                            break;
                                        }
                                        if (args.CheckStopLine
                                            && DomLine.CheckStopLine(LastLine, privateLines[iCount]))
                                        {
                                            // 若已经存在的文本行和新建的最后一个文本行的内部设置一样,包括元素清单
                                            // 和行设置,而且该行所有元素的大小没有发生改变,则设置该旧行为停止行
                                            // 停止行以后的旧行就没有必要进行刷新操作,提前退出刷新行操作,避免不必要
                                            // 的工作量
                                            // 某些情况下某些位置处元素发生改变影响行分布,主调程序就使用EndIndex指明
                                            // 停止行必须在这些位置后出现,因此本段程序稍前进行了 EndIndex 判断操作
                                            stopLine = privateLines[iCount];
                                            this.RefreshLineState(LastLine, args.ForceRefreshAllContent);
                                            goto EndAddLine;
                                        }//if
                                    }//for
                                }//if
                            }//if
                        }//else
                        if (myStack.Count > 0)
                        {
                            if (newLine != null && newLine.Count > 0)
                            {
                                newLine.Width = args.FastClientWidth;
                                //newLine.Width = args.FastClientWidth;
                                this.RefreshLineState(newLine, args.ForceRefreshAllContent);
                                // 进行数组替换操作,减少内存浪费
                                LoaderListBuffer<DomElement[]>.Instance.Return(newLine.InnerGetArrayRaw());
                                newLine.ReturnPublicArray();
                            }
                            // 创新新的文档行对象
                            newLine = CreateLine(args, newLines.LastLine);
                            newLine.InnerSetArrayRaw(LoaderListBuffer<DomElement[]>.Instance.Alloc(), 0);
                            newLines.Add(newLine);
                        }
                        else
                        {
                            // 所有的元素都排版完毕了。
                            if (newLine != null && newLine.Count > 0)
                            {
                                this.RefreshLineState(newLine, args.ForceRefreshAllContent);
                            }
                            break;
                        }
                    }//if
                }//while( myStack.Count > 0 )
                myStack.Close();
                LoaderListBuffer<DomElement[]>.Instance.Return(newLine.InnerGetArrayRaw());
                newLine.ReturnPublicArray();
            }//if
        EndAddLine:
            args._CreateLine_LastParagraphElement = null;
            bool needUpdateLinePosion = false;
            bool pageFlag = false;

            if (args.ForBlankDocument)
            {
                newLine = newLines[0];
                newLines.ReturnToLoaderListBuffer(true);
                privateLines.Add(newLine);
                if (needReturnPrivateLines)
                {
                    privateLines.ReturnToLoaderListBuffer(false);
                }
                newLine.UpdateOwnerLine();
                newLine.InvalidateState = false;
                args.HasCalculate = true;
                dce.IncreateGlobalLineCount(privateLines.Count);
                return isBodyContent;
            }
            // 用户界面无效的文本行对象
            DomLineList viewInvalidateLines = null;
            if (WriterControl.RefreshingDocumentView == false)
            {
                viewInvalidateLines = new DomLineList();
                viewInvalidateLines.InnerSetArrayRaw(LoaderListBuffer<DomLine[]>.Instance.Alloc(), 0);
            }
            if (isBlankContentElement && newLines.Count == 1)
            {
                newLine.RefreshStateNew();
                newLine[0]._OwnerLine = newLine;
                if (privateLines.Count > 0)
                {
                    viewInvalidateLines?.AddRangeByDCList(privateLines);
                    privateLines.Clear();
                }
                privateLines.Add(newLine);
                needUpdateLinePosion = true;
            }
            else
            {
                //linesRefreshed2 = new List<XTextLine>();
                // 被进行了二次刷出的文档行对象
                if (newLines.Count > 0)
                {
                    DomLine LastLine = newLines.LastElement;
                    if (LastLine.Count == 0)
                    {
                        newLines.RemoveAt(newLines.Count - 1);
                        //newLines.Remove(LastLine);
                        if (newLines.Count > 0)
                        {
                            this.FixLastLineHeight(newLines.LastElement);
                        }
                    }
                    else
                    {
                        this.FixLastLineHeight(LastLine);
                    }
                }

                int startLineIndex = 0;
                // 计算新的文档行的位置
                // float topCount = rs.PaddingTop;// this.Top + this.OwnerDocument.Top;
                if (document._GlobalGridInfo != null)
                {
                    // 有全局网格线时，不计上下内边距。
                    //topCount = 0;
                }
                if (startElement.OwnerLine != null)
                {
                    //topCount = startElement.OwnerLine.Top;
                    startLineIndex = privateLines.IndexOf(startElement.OwnerLine);
                }
                if (startLineIndex < 0)
                {
                    startLineIndex = 0;
                    //throw new System.Exception("AllElements 和 Content 内容不同步");
                }
                // 补充查找 StopLine
                if (stopLine == null && newLines.Count > 1 && privateLines.Count > 0)
                {
                    DomLine LastLine = newLines.LastElement;
                    //XTextElement FirstElement = LastLine.FirstElement;
                    for (int iCount = privateLines.Count - 1; iCount >= 0; iCount--)
                    {
                        if (DomLine.CheckStopLine(LastLine, privateLines[iCount]))
                        {
                            stopLine = privateLines[iCount];
                            break;
                        }
                    }
                }

                if (stopLine == null
                    && newLines.Count == 1
                    && privateLines.Count > 0)
                {
                    if (privateLines.LastLine.FirstElement == newLines[0].FirstElement)
                    {
                        stopLine = privateLines.LastLine;
                    }
                }

                // 是否需要重新分页标记
                if (stopLine == null)
                {
                    pageFlag = true;
                    needUpdateLinePosion = true;
                    if (privateLines.Count > 0)
                    {
                        if (startLineIndex == 0)
                        {
                            viewInvalidateLines?.AddRangeByDCList(privateLines);
                            DCObjectPool<DomLine>.ReturnRange(privateLines);
                            privateLines.Clear();
                        }
                        else
                        {
                            for (int iCount = privateLines.Count - 1;
                                iCount >= startLineIndex;
                                iCount--)
                            {
                                var line4 = privateLines[iCount];
                                viewInvalidateLines?.Add(line4);
                                DCObjectPool<DomLine>.Return(line4);
                                privateLines.RemoveAt(iCount);
                            }
                        }
                    }
                    privateLines.AddRangeByDCList(newLines);
                    for (int iCount = newLines.Count - 1; iCount >= 0; iCount--)
                    {
                        newLines[iCount].UpdateOwnerLine();
                    }
                }
                else
                {
                    DomLine startLine = startElement.OwnerLine;
                    if (startLine == null)
                    {
                        startLine = privateLines[0];
                    }
                    int endLineIndex = privateLines.IndexOf(stopLine);
                    if (endLineIndex - startLineIndex + 1 != newLines.Count)
                    {
                        // 新增的行和要删除的行数不一致,需要重新分页
                        pageFlag = true;
                        needUpdateLinePosion = true;
                    }
                    else
                    {
                        var newLinesCount = newLines.Count;
                        for (int iCount = 0; iCount < newLinesCount; iCount++)
                        {
                            DomLine line1 = newLines[iCount];
                            DomLine line2 = privateLines[iCount + startLineIndex];
                            if (line1.Height != line2.Height)
                            {
                                // 行高发生改变,则需要重新分页
                                pageFlag = true;
                                break;
                            }
                        }
                    }
                    // 判断 StopLine 是否更新
                    if (newLines.LastLine.Top != stopLine.Top)
                    {
                        needUpdateLinePosion = true;
                    }
                    if (pageFlag == false && newLines.Count > 1)
                    {
                        endLineIndex--;
                        newLines.RemoveAt(newLines.Count - 1);
                    }
                    // 将新的文档行对象替换掉旧的文档行对象
                    if (pageFlag == false)
                    {
                        // 若没有分页则直接替换文本行
                        for (int iCount = 0; iCount < newLines.Count; iCount++)
                        {
                            DomLine line = newLines[iCount];
                            viewInvalidateLines?.Add(line);
                            //if (this.OwnerDocument.EditorControl != null)
                            //{
                            //    this.OwnerDocument.EditorControl.ViewInvalidate(
                            //        line.AbsBounds ,
                            //        this.ContentPartyStyle );
                            //}
                            line.OwnerPage = privateLines[iCount + startLineIndex].OwnerPage;
                            line.UpdateOwnerLine();
                            DomLine oldLine = privateLines[iCount + startLineIndex];
                            viewInvalidateLines?.Add(oldLine);
                            privateLines.Replace(iCount + startLineIndex, line);
                            DCObjectPool<DomLine>.Return(oldLine);
                        }
                    }
                    else
                    {
                        // 若需要分页则删除旧行,然后插入新的文本行
                        for (int iCount = endLineIndex; iCount >= startLineIndex; iCount--)
                        {
                            if (iCount >= 0)
                            {
                                if (viewInvalidateLines != null && document.EditorControl != null)
                                {
                                    DomLine OldLine = privateLines[iCount];
                                    viewInvalidateLines.Add(OldLine);

                                    //this.OwnerDocument.EditorControl.ViewInvalidate(
                                    //    OldLine.AbsBounds ,
                                    //    this.ContentPartyStyle );
                                }
                                DCObjectPool<DomLine>.Return(privateLines[iCount]);
                                privateLines.RemoveAt(iCount);
                            }
                        }//for
                        for (int iCount = 0; iCount < newLines.Count; iCount++)
                        {
                            if (viewInvalidateLines != null && document.EditorControl != null)
                            {
                                DomLine line = newLines[iCount];
                                viewInvalidateLines.Add(line);

                                //this.OwnerDocument.EditorControl.ViewInvalidate(
                                //    line.AbsBounds,
                                //    this.ContentPartyStyle );
                            }
                            newLines[iCount].UpdateOwnerLine();
                            privateLines.Insert(iCount + startLineIndex, newLines[iCount]);
                        }//for
                    }
                }
            }

            if (needUpdateLinePosion
                && document.IsLoadingDocument == false)
            {
                this.ResetLinesAbsLocation();
            }
            // 更新文档行位置
            if (vertialAlign != VerticalAlignStyle.Top || needUpdateLinePosion)
            {
                if (this.Height > 0)
                {
                    UpdateLinePosition(vertialAlign, false, false);
                }
            }
            var thisContentPartyStyle = this.ContentPartyStyle;

            if (args.LayoutContentOnly == false)
            {
                var ctl = document.WriterControl;
                if (ctl != null && globalRefreshLines == false && viewInvalidateLines != null)
                {
                    //if (vertialAlign == VerticalAlignStyle.Top)
                    {
                        foreach (DomLine line in viewInvalidateLines)
                        {
                            ctl.ViewInvalidate(
                                line,
                                thisContentPartyStyle);
                        }
                        foreach (DomLine line in newLines.FastForEach())
                        {
                            ctl.ViewInvalidate(
                                line,
                                thisContentPartyStyle);
                        }
                    }
                }
            }
            if (viewInvalidateLines != null)
            {
                LoaderListBuffer<DomLine[]>.Instance.Return(viewInvalidateLines.InnerGetArrayRaw());
                viewInvalidateLines.Clear();
            }
            LoaderListBuffer<DomLine[]>.Instance.Return(newLines.InnerGetArrayRaw());
            newLines.ClearAndEmpty();
            LoaderListBuffer<DomLineList>.Instance.Return(newLines);

            if (freakElements != null && freakElements.Count > 0)
            {
                // 由于修整连续的英文字母和数字导致元素进入某个新行而又退出行
                // 元素Left值发生改变，此处使用文本行的 RefreshState 函数刷新
                // 行内设置来恢复这些反复无常的元素的位置
                //XTextLine lastFixLine = null;
                List<DomLine> lines = new List<DomLine>();
                foreach (DomElement element in freakElements)
                {
                    DomLine line = element.OwnerLine;
                    if (line != null
                        && privateLines.Contains(line)
                        && lines.Contains(line) == false)
                    {
                        lines.Add(element.OwnerLine);
                    }
                }
                if (lines.Count > 0)
                {
                    foreach (DomLine line in lines)
                    {
                        line.SpecifyLineSpacing = 0;
                        RefreshLineState(line, args.ForceRefreshAllContent);
                    }
                }
            }
            if (freakElements != null)
            {
                freakElements.Clear();
                LoaderListBuffer<List<DomElement>>.Instance.Return(freakElements);
            }
            if (this is DomDocumentBodyElement
                && privateLines.Count > 0
                && privateLines.LastElement.IsTableLine)
            {
                // 文档正文最后一行是表格行，则修正该行的高度
                var line2 = privateLines.LastElement;
                line2.Height = line2.TableElement.Top + line2.TableElement.Height;
            }

            pageFlag = false;
            // 比较新的文本行的高度和旧的文本行的高度是否一致
            // 若文本行个数或高度发生改变,则会导致重新分页
            // 若文本行个数和高度都没变,则无需重新分页
            if (linesInfoBack == null)
            {
                pageFlag = true;
            }
            else if (linesInfoBack.Length != privateLines.Count * 2)
            {
                pageFlag = true;
            }
            else
            {
                for (int iCount = 0; iCount < privateLines.Count; iCount++)
                {
                    DomLine line = privateLines[iCount];
                    if (line.NativeTop != linesInfoBack[iCount * 2])
                    {
                        pageFlag = true;
                        break;
                    }
                    if (line.Height != linesInfoBack[iCount * 2 + 1])
                    {
                        pageFlag = true;
                        break;
                    }
                    line.GlobalIndex = lineIndexBack[iCount * 2];
                    line.IndexInPage = lineIndexBack[iCount * 2 + 1];
                }//for
            }
            if (theOnlyElement == null)
            {
                //int indexInParagraph = 0;
                var len55 = privateLines.Count;
                var arr55 = privateLines.InnerGetArrayRaw();
                //foreach (XTextLine line in privateLines.FastForEach())
                for (var iCount55 = 0; iCount55 < len55; iCount55++)
                {
                    var line = arr55[iCount55];
                    line.InvalidateState = false;
                }
            }
            else
            {
                privateLines[0].InvalidateState = false;
            }
            if (needReturnPrivateLines)
            {
                privateLines.ReturnToLoaderListBuffer(false);
            }
            dce.IncreateGlobalLineCount(privateLines.Count);
            if (pageFlag)
            {
                if (args.LayoutContentOnly == false
                     && document.WriterControl != null
                     && globalRefreshLines == false)
                {
                    document.WriterControl.ViewInvalidate(
                            this.GetAbsBounds(),
                            thisContentPartyStyle);
                }
                if (isBodyContent)
                {
                    this.RefreshPrivateLinesOwnerPage();
                }
                //tick = CountDown.GetTickCountFloat() - tick;
                args.HasCalculate = true;
                return isBodyContent;
            }
            else
            {
                if (isBodyContent)
                {
                    if (this._Existed_FixLinePositionForPageLine)
                    {
                        this.FixLinePositionForPageLine(-1);
                    }
                    this.RefreshPrivateLinesOwnerPage();
                }
                //tick = CountDown.GetTickCountFloat() - tick; 
                args.HasCalculate = true;
                return false;
            }
        }
        internal void ResetLinesAbsLocation()
        {
            if (this._PrivateLines != null && this._PrivateLines.Count > 0)
            {
                //if (this._PrivateLines.Count == 1
                //    && this._PrivateLines[0][0] is XTextParagraphFlagElement)
                if (this._Elements.OnlyHasSingleParagraphFlagElement())//this.IsOnlyOneParagraphFlag )
                {
                    // 在不少情况下，只有一个无内容的文档行
                    this._PrivateLines[0].ResetAbsLocation();
                    return;
                }
                var lineCount = this._PrivateLines.Count;
                var arr = this._PrivateLines.InnerGetArrayRaw();
                for (var iCount = 0; iCount < lineCount; iCount++)
                {
                    var line = arr[iCount];// this._PrivateLines.GetItemFast(iCount);
                    line.ResetAbsLocation();
                    if (line.IsTableLine)
                    {
                        line.TableElement.ResetLinesAbsLocation();
                    }
                }
            }
        }

        private bool HasFieldElement
        {
            get { return this.StateFlag13; }
            set { this.StateFlag13 = value; }
        }

        private void RefreshLineState(DomLine myLine, bool forceRefreshAllContent)
        {
            //_RefreshLineStateTickInfo.Enable = this.OwnerDocument.Options.BehaviorOptions.DebugMode;
            //_RefreshLineStateTickInfo.StartOneTick();
            myLine.InvalidateState = false;
            myLine.UpdateContentHeight();
            //myLine.UpdateAbsLocation();

            if (myLine.IsTableLine)
            {
                myLine.RefreshStateNew();
                DomTableElement table = myLine.TableElement;
                if (forceRefreshAllContent || table.LayoutInvalidate || DomDocument.IsExecuteingLayout)
                {
                    table.InnerExecuteLayout();
                    myLine.RefreshStateNew();
                }
            }
            else
            {
                myLine.RefreshStateNew();
            }
        }

        /// <summary>
        /// 修正最后一个文本行的高度
        /// </summary>
        private void FixLastLineHeight(DomLine line)
        {
            float h = this.GetRuntimeSpecifyLineSpacing(line);
            line.SpecifyLineSpacing = h;
            if (line.LayoutInvalidate && line.IsTableLine == false)
            {
                // 行间距过小，调整行内元素位置
                line.RefreshStateNew();
            }
        }

        /// <summary>
        /// 修正最后一个文本行的高度
        /// </summary>
        internal float GetRuntimeSpecifyLineSpacing(DomLine line)
        {
            RuntimeDocumentContentStyle ps = line.ParagraphFlagElement.RuntimeStyle;
            float result = 0;
            if (ps.LineSpacingStyle != LineSpacingStyle.SpaceExactly)
            {
                //RuntimeDocumentContentStyle rs = this.RuntimeStyle;
                {
                    // 上一行和新文档行是在同一个段落中，因此只要设置行间距即可。
                    // 计算行间距
                    result = ps.GetLineSpacing(
                        line.ContentHeight,
                        line.MaxFontHeight,
                        DCSystem_Drawing.GraphicsUnit.Document);
                }
                if (line.IsTableOrPageBreakLine)
                {
                    result = 0;
                }
            }
            return result;
        }
        /// <summary>
        /// 创建文档行对象
        /// </summary>
        /// <param name="preLine"></param>
        /// <returns></returns>
        private DomLine CreateLine(ParticalRefreshLinesEventArgs args, DomLine preLine)
        {
            if (this._PrivateContent == null || this._PrivateContent.Count == 0)
            {
                return null;
            }

            DomDocument document = this.OwnerDocument;

            //_CreateLineTick.StartOneTick();
            //_CreateLineTick.Enable = document.Options.BehaviorOptions.DebugMode;

            RuntimeDocumentContentStyle crs = this.RuntimeStyle;
            //// 大概率和上一行的元素个数接近
            //XTextLine newLine = DCSoft.Common.DCObjectPool<XTextLine>.TryGet();
            //if (newLine == null)
            //{
            //    newLine = new XTextLine();
            //    //newLine = new XTextLine(
            //    //    this,
            //    //    preLine == null ? 0 : preLine.Count);
            //}
            var newLine = new DomLine();
            DomElement firstElement = this._PrivateContent[0];
            int firstIndex = 0;
            if (preLine != null)
            {
                firstElement = preLine.LastElement;
                if (firstElement != null)
                {
                    firstIndex = this._PrivateContent.IndexOf(firstElement, args._CreateLine_LastFirstIndex);
                    if (firstIndex >= 0)
                    {
                        args._CreateLine_LastFirstIndex = firstIndex - 1;
                        if (args._CreateLine_LastFirstIndex < 0)
                        {
                            args._CreateLine_LastFirstIndex = 0;
                        }
                        firstIndex++;
                        firstElement = this._PrivateContent.SafeGet(firstIndex);
                    }
                    else
                    {
                    }
                }
            }
            if (firstElement == null)
            {
                firstElement = this.PrivateContent.FirstElement;
                firstIndex = 0;
                args._CreateLine_LastFirstIndex = 0;
            }
            if (firstIndex < 0)
            {
                firstIndex = 0;
                args._CreateLine_LastFirstIndex = 0;
            }
            var lastPIndex = 0;
            if (args._CreateLine_LastParagraphElement != null
                && this._PrivateContent.SafeGet(args._CreateLine_LastParagraphElement._PrivateContentIndex) == args._CreateLine_LastParagraphElement)
            {
                lastPIndex = args._CreateLine_LastParagraphElement._PrivateContentIndex;
            }
            if (lastPIndex > firstIndex)
            {
                newLine.ParagraphFlagElement = args._CreateLine_LastParagraphElement;
            }
            else
            {
                newLine.ParagraphFlagElement = this._PrivateContent.GetFirstParagraphFlag(firstIndex);
                args._CreateLine_LastParagraphElement = newLine.ParagraphFlagElement;
            }
            // 整个容器中固定行高
            float specifyLineHeight = 0;
            RuntimeDocumentContentStyle ps = newLine.ParagraphFlagElement.RuntimeStyle;
            if (specifyLineHeight <= 0)
            {
                // 应用段落前间距
                if (preLine == null
                    || preLine.ParagraphFlagElement != newLine.ParagraphFlagElement)
                {
                    newLine.SpacingBeforeForParagraph = ps.SpacingBeforeParagraph;
                }
                else
                {
                    newLine.SpacingBeforeForParagraph = 0;
                }
                //newLine.Spacing = ps.Spacing;
            }
            newLine.Align = ps.Align;
            newLine.Left = crs.PaddingLeft;
            if (preLine == null)
            {
                // 没有上一行，那就是容器中的第一行
                if (this.InnerRuntimeGridLine() == null && this.ZeroRuntimePaddingTopBottom == false) // document._GlobalGridInfo == null)
                {
                    newLine.Top = crs.PaddingTop;// this._RuntimePaddingTop;// crs.PaddingTop;
                }
                else
                {
                    // 有全局网格线时，不计上下内边距。
                    newLine.Top = 0;
                }
            }
            else
            {
                //if (preLine.ParagraphFlagElement == newLine.ParagraphFlagElement)
                {
                    // 上一行和新文档行是在同一个段落中，因此只要设置行间距即可。
                    // 计算行间距
                    //if (preLine[0].ViewIndex == 275)
                    //{
                    //}
                    float h = 0;
                    if (specifyLineHeight > 0)
                    {
                        // 设置了固定排版行距的
                        h = specifyLineHeight;
                        preLine.SpecifyLineSpacing = h;
                    }
                    else
                    {
                        float maxFontHeight = preLine.MaxFontHeight;
                        h = preLine.ParagraphFlagElement.RuntimeStyle.GetLineSpacing(
                            preLine.ContentHeight,
                            maxFontHeight,
                            DCSystem_Drawing.GraphicsUnit.Document);
                        if (preLine.IsTableLine)
                        {
                            // 表格行，行间距固定为SingleLine
                            h = (float)(preLine.TableElement.Height + maxFontHeight * 0.1);
                            //preLine.Height = h;
                        }
                        else
                        {
                            if (ps.LineSpacingStyle == LineSpacingStyle.SpaceExactly)
                            {
                                preLine.SpecifyLineSpacing = 0;
                            }
                            else
                            {
                                // 指定行间距
                                preLine.SpecifyLineSpacing = h;
                            }
                        }
                    }
                    //if (preLine.AdditionHeight < 3)
                    //{
                    //    preLine.AdditionHeight = 0;
                    //}
                    //if (preLine.AdditionHeight < 0 && preLine.IsTableLine == false)
                    //{
                    //    // 行间距过小，调整行内元素位置
                    //    preLine.RefreshState();
                    //}
                    //else
                    if (preLine.ParagraphFlagElement != newLine.ParagraphFlagElement)
                    {
                        // 上一行和新文档行不是同一个段落的，说明上一行是其所在段落的最后一行
                        // 设置断后间距为上一行的多余的行高
                        preLine.SpacingAfterForParagraph = preLine.ParagraphFlagElement.RuntimeStyle.SpacingAfterParagraph;
                    }
                }
                if (preLine.IsPageBreakLine)
                {
                    //preLine.Height = 0;
                    preLine.SpecifyLineSpacing = 0;
                    preLine.SpacingBeforeForParagraph = 0;
                    preLine.SpacingAfterForParagraph = 0;
                    preLine.LayoutInvalidate = false;
                }
                if (preLine.LayoutInvalidate)
                {
                    // 重新计算上一行的内容排版
                    preLine.RefreshStateNew();
                }
                // 根据网格线设置来调整行高
                preLine.FixHeightForGridLine();
                newLine.Top = preLine.Top + preLine.Height;
            }
            newLine.NativeTop = newLine.Top;
            newLine.Width = args.FastClientWidth;
            newLine.Height = Math.Max(
                document.DefaultStyle.DefaultLineHeight,
                firstElement.Height);
            if (specifyLineHeight > 0)
            {
                newLine.Height = specifyLineHeight;
            }
            newLine._OwnerContentElement = this;
            newLine._OwnerDocument = document;

            //_CreateLineTick.EndOneTick();

            return newLine;
        }

        /// <summary>
        /// 刷新私有文本行所在的文档页对象
        /// </summary>
        internal void RefreshPrivateLinesOwnerPage()
        {
            DomLineList pLines = this._PrivateLines;
            if (pLines == null || pLines.Count == 0)
            {
                return;
            }
            PrintPageCollection pages = this._OwnerDocument.Pages;
            if (pages == null || pages.Count == 0)
            {
                // 文档尚未分页，不执行操作。
                return;
            }
            //foreach (XTextLine line in pLines)
            //{
            //    line.OwnerPage = null;
            //    line.LastOwnerPage = null;
            //}
            float aTop = this.GetAbsTop();
            //float aBottom = aTop + this.Height;
            //float lastLineTop = aTop + pLines[0].Top;
            //float pageTop =pages.Top;
            //int pLinesCount = pLines.Count;
            int pagesCount = pages.Count;
            int startPageIndex = pages.IndexOfByPosition(aTop, 0);
            var pLinesCount = pLines.Count;
            //foreach (XTextLine line in pLines.FastForEach())
            for (var lineIndex = 0; lineIndex < pLinesCount; lineIndex++)
            {
                var line = pLines.GetItemFast(lineIndex);
                line.OwnerPage = null;
                var lineTop = aTop + line.Top;
                var lineBottom = lineTop + line.Height;
                var currentPage = pages[startPageIndex];
                if (lineTop > currentPage.Top - 1
                    && lineBottom < currentPage.Bottom + 1)
                {
                    // 在大多数情况下文档行没有跨页，在当前页面中
                    line.OwnerPage = currentPage;
                }
                else
                {
                    if (lineTop > currentPage.Bottom - 1)
                    {
                        startPageIndex = Math.Min(startPageIndex + 1, pages.Count - 1);
                    }
                    for (int pageIndex = startPageIndex; pageIndex < pagesCount; pageIndex++)
                    {
                        var page = pages[pageIndex];
                        var pageBottom = page.Bottom;
                        if (lineTop < pageBottom - 2)
                        {
                            line.OwnerPage = page;
                            startPageIndex = pageIndex;
                            if (lineBottom < pageBottom + 2)
                            {
                            }
                            else
                            {
                                for (int pageIndex2 = pageIndex + 1; pageIndex2 < pagesCount; pageIndex2++)
                                {
                                    var page2 = pages[pageIndex2];
                                    if (page2.Top > lineBottom)
                                    {
                                        startPageIndex = pageIndex2 - 1;
                                        break;
                                    }
                                }
                            }
                            break;
                        }
                    }//for
                }
            }//foreach

        }

        /// <summary>
        /// 内容部分样式
        /// </summary>
        public virtual PageContentPartyStyle ContentPartyStyle
        {
            get
            {
                if (this.OwnerDocument == null)
                {
                    return PageContentPartyStyle.Body;
                }
                DomDocument document = this.OwnerDocument;
                DomElement parent = this;
                while (parent != null)
                {
                    if (parent is DomDocumentContentElement)
                    {
                        if (parent == document.Body)
                        {
                            return PageContentPartyStyle.Body;
                        }
                        else if (parent == document.Header)
                        {
                            return PageContentPartyStyle.Header;
                        }
                        else if (parent == document.Footer)
                        {
                            return PageContentPartyStyle.Footer;
                        }
                    }
                    parent = parent.Parent;
                }//while
                return PageContentPartyStyle.Body;
            }
        }

        /// <summary>
        /// 计算分页符
        /// </summary>
        /// <param name="info">分页符位置信息</param>
        public void FixPageLine(PageLineInfo info)
        {
            if (this._PrivateLines == null || this._PrivateLines.Count == 0)
            {
                // 没有任何文本行，不进行分页
                return;
            }
            // 获得当前行
            DomLine lastLine = this._PrivateLines.LastLine;
            if ((this is DomDocumentBodyElement) == false)
            {
                if (lastLine.AbsTop + lastLine.Height <= info.CurrentPoistion)
                {
                    // 分页线的位置超过最后一行的下边缘，则退出。
                    bool exitFlag = true;
                    if (info.PageBreakElements != null && info.PageBreakElements.Count > 0)
                    {
                        foreach (DomPageBreakElement pb in info.PageBreakElements)
                        {
                            if (pb.Handled == false && pb.IsParentOrSupParent(this))
                            {
                                // 文档中存在没有处理过的分页符号，则不退出本次处理。
                                exitFlag = false;
                                break;
                            }
                        }
                    }
                    if (exitFlag)
                    {
                        // 确定退出本次处理。
                        info.UnderContentBottomSource = this;
                        info.UnderContentBottom = true;
                        //info.PageIndexAsNewPageForNextPage = info.CurrentPageIndex + 1;
                        return;
                    }
                }
            }
            var bopts = GetDocumentBehaviorOptions();

            DomLine currentLine = null;
            float containerAbsTop = this.GetAbsTop();
            DomLineList pLines = this._PrivateLines;
            int startLineIndex = 0;
            // 正常分页模式，则计算开始的行号
            for (int iCount = pLines.Count - 1; iCount >= 0; iCount--)
            {
                if (pLines[iCount].Top + containerAbsTop < info.LastPosition)
                {
                    startLineIndex = iCount;
                    break;
                }
            }
            int pLinesCount = pLines.Count;
            var pLineArr = pLines.InnerGetArrayRaw();
            for (int iCount = startLineIndex; iCount < pLinesCount; iCount++)
            {
                DomLine line = pLineArr[iCount];// pLines[iCount];
                if (info._CurrentPoistion >= containerAbsTop + line.Bottom)
                {
                    // 分页线的高度超过文档行的下边缘
                    if (line.IsTableOrPageBreakLine == false)
                    {
                        // 不是表格行，分页符或者文档节行，不包含复杂的内容，则不处理本文档行
                        continue;
                    }
                }
                // 计算续打线位置时不考虑强制分页
                // 正常分页时需要考虑强制分页
                if (line.IsTableLine)
                {
                    if (line.TableElement.GetContainsUnHandledPageBreak())
                    {
                        line.TableElement.FixPageLine(info);
                        break;
                    }
                }
                if (Math.Abs(info.CurrentPoistion - containerAbsTop - line.Top) < 2)
                {
                    // 出现微小的差别，可以忽略不计
                    currentLine = line;
                    break;
                }
                if (info.CurrentPoistion > containerAbsTop + line.Top)
                {

                    currentLine = line;

                    if (currentLine[0] is DomPageBreakElement)
                    {
                        // 出现强制分页
                        DomPageBreakElement pb = (DomPageBreakElement)currentLine[0];
                        if (pb.Handled == false)
                        {
                            //进行强制分页
                            pb.Handled = true;
                            if (bopts.PageLineUnderPageBreak)
                            {
                                int newPos = (int)(currentLine.AbsTop + currentLine.Height);
                                info.SourceElement = pb;
                                if (pLines.LastLine == line)
                                {
                                    // 容器元素的最后一行，尽量将分页线放置在容器元素的下边界
                                    float pos = this.GetAbsBounds().Bottom;
                                    if (pos - info.CurrentPoistion < 50)
                                    {
                                        newPos = (int)pos;
                                        info.SourceElement = this;
                                    }
                                }
                                info.CurrentPoistion = newPos;
                            }
                            else
                            {
                                info.CurrentPoistion = (int)(currentLine.AbsTop);
                            }
                            info.SourceElement = pb;
                            info.ForNewPage = true;
                            return;
                        }
                    }
                    if (pLines.LastLine == line)
                    {
                        if (info.CurrentPoistion > line.AbsBottom)// containerAbsTop + line.Top + line.Height + 10 )
                        {
                            // 分页线超出内容了，在容器元素下半部分的没有内容的区域中
                            // 无需修改分页线的位置
                            currentLine = null;
                            info.UnderContentBottomSource = this;
                            info.UnderContentBottom = true;
                            break;
                        }
                    }
                    if (info.CurrentPoistion < containerAbsTop + currentLine.Top + currentLine.Height)
                    {
                        // 低于文档行下边缘，就是这了。
                        break;
                    }
                }
                else
                {
                    break;
                }
            }//foreach
            if (currentLine == null)
            {
                // 没有找到当前行，不修改分页
                if (info.UnderContentBottom == false)
                {
                    info.UpContentTop = true;
                }
                return;
            }
            if (info.CurrentPoistion > currentLine.AbsBottom)
            {
                // 若分页线位置超过文本行，则不修改分页
                if (currentLine == pLines.LastLine)
                {
                    // 若当前行是最后一行文本
                    RectangleF bounds = this.GetAbsBounds();
                    if (info.CurrentPoistion >= bounds.Bottom)
                    {
                        // 若分页线位置大于对象边框低边界位置，则设置为对象低边界位置
                        info.CurrentPoistion = bounds.Bottom;
                        info.SourceElement = this;
                    }
                    else if (bounds.Bottom - info.CurrentPoistion
                        < this.OwnerDocument.PixelToDocumentUnit(15))
                    {
                        // 若分页线的位置在最后一行和对象底边界位置之间，
                        // 而且距离底边界之间距离小于15个像素
                        // 则进行提前一行分页
                        goto SetPagePosition;
                    }
                }
                info.UnderContentBottomSource = this;
                info.UnderContentBottom = true;
                return;
                //return (int)currentLine.AbsBottom;
            }

        SetPagePosition:
            {
                if (currentLine.TableElement != null)
                {
                    // 如果出现表格，则针对表格修正分页线位置
                    DomTableElement table = currentLine.TableElement;
                    table.FixPageLine(info);
                    if (info.CurrentPoistion == table.AbsTop)
                    {
                        info.CurrentPoistion = table.OwnerLine.AbsTop;
                    }
                }
                else
                {
                    int index = pLines.IndexOf(currentLine);
                    float newPos = 0;
                    if (index > 0)
                    {
                        // 分页线移动到上一个文本行和当前文本行中间
                        DomLine preLine = pLines[index - 1];
                        newPos = (preLine.AbsTop + preLine.Height + currentLine.AbsTop) / 2;
                    }
                    else
                    {
                        // 分页线移动到本文本行的顶端位置
                        newPos = currentLine.AbsTop;
                    }
                    DomElement element = currentLine[0];
                    if (element is DomParagraphListItemElement)
                    {
                        if (currentLine.Count > 1)
                        {
                            element = currentLine[1];
                        }
                    }
                    if (element != null)
                    {
                        info.SourceElement = element;
                    }
                    if (newPos - info.LastPosition > info.MinPageContentHeight)
                    {
                        // 设置新分页线
                        info.CurrentPoistion = newPos;
                        RectangleF bounds = this.GetAbsBounds();
                        if (info.CurrentPoistion - bounds.Top
                            < this.OwnerDocument.PixelToDocumentUnit(15))
                        {
                            info.CurrentPoistion = bounds.Top;
                            info.SourceElement = this;
                        }
                        if (index == 0)
                        {
                            info.UpContentTop = true;
                        }
                    }
                }
            }
        }

        /// 内容是否为空
        /// </summary>
        public override bool HasContentElement
        {
            get
            {
                if (this._Elements == null || this._Elements.Count == 0)
                {
                    return false;
                }
                if (this._Elements.Count == 1 && this._Elements[0] is DomParagraphFlagElement)
                {
                    //return this.Elements[0].StyleIndex >= 0 ;
                    return false;
                }
                return true;
            }
        }

        /// <summary>
        /// 绘制文档内容
        /// </summary>
        /// <param name="args">参数</param>
        public override void DrawContent(InnerDocumentPaintEventArgs args)
        {
            if (this._PrivateLines == null
                || this._PrivateLines.Count == 0)
            //|| this._PrivateContent == null 
            //|| this._PrivateContent.Count == 0 )
            {
                // 状态不对，退出处理
                return;
            }
            //if (this._PrivateContent == null || this._PrivateContent.Count == 0)
            //{
            //    var s = 1;
            //}
            var bolHasGroupShape = false;
            try
            {
                //if ((System.Environment.TickCount % 4) == 0)
                //{
                //    throw new System.Exception("aaaa");
                //}
                if (args.Graphics.IsSVGMode)
                {
                    var rect3 = args.ViewBounds;
                    if (args.ClipRectangle.IsEmpty == false)
                    {
                        rect3 = RectangleF.Intersect(args.ClipRectangle, rect3);
                    }
                    if (rect3.IsEmpty == false && args.PageClipRectangle.IsEmpty == false)
                    {
                        rect3 = RectangleF.Intersect(rect3, args.PageClipRectangle);
                    }
                    if (rect3.IsEmpty == false)
                    {
                        if (this is DomDocumentContentElement)
                        {
                            rect3.X -= 2000;
                            rect3.Width += 4000;
                        }
                        else
                        {
                            var localRequireCheckClipBoundsForPrint = this.RequireCheckClipBoundsForPrint;
                            if (localRequireCheckClipBoundsForPrint == DCBooleanValueHasDefault.Default)
                            {
                                localRequireCheckClipBoundsForPrint = DCBooleanValueHasDefault.True;
                                {
                                    var plines = this._PrivateLines;
                                    if (plines != null && plines.Count > 0)
                                    {
                                        // 判断内容是否超出对象边界
                                        if (plines[0].Top < 0)
                                        {
                                            localRequireCheckClipBoundsForPrint = DCBooleanValueHasDefault.False;
                                        }
                                        if (plines[plines.Count - 1].Bottom > this._Height)
                                        {
                                            localRequireCheckClipBoundsForPrint = DCBooleanValueHasDefault.False;
                                        }
                                        if (localRequireCheckClipBoundsForPrint == DCBooleanValueHasDefault.True)
                                        {
                                            for (var iCount = plines.Count - 1; iCount >= 0; iCount--)
                                            {
                                                var line = plines[iCount];
                                                if (line.Width > this._Width)
                                                {
                                                    localRequireCheckClipBoundsForPrint = DCBooleanValueHasDefault.False;
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                }
                                this.RequireCheckClipBoundsForPrint = localRequireCheckClipBoundsForPrint;
                            }
                            if (localRequireCheckClipBoundsForPrint == DCBooleanValueHasDefault.True)
                            {
                                rect3 = RectangleF.Empty;
                            }
                        }
                        RuntimeDocumentContentStyle rsFont = null;
                        foreach (var element in this.PrivateContent)
                        {
                            if (element is DomCharElement)
                            {
                                rsFont = element.RuntimeStyle;
                                break;
                            }
                        }
                        if (rsFont == null)
                        {
                            args.Graphics.SVG.BeginGroupShape(this.ID, rect3, null, 0, FontStyle.Regular, false);
                        }
                        else
                        {
                            args.Graphics.SVG.BeginGroupShape(
                                this.ID,
                                rect3,
                                rsFont.FontName == null || rsFont.FontName.Length == 0 ? XFontValue.DefaultFontName : rsFont.FontName,
                                rsFont.FontSize,
                                rsFont.FontStyle,
                                false);
                        }
                        bolHasGroupShape = true;
                    }
                }
                InnerContentElementDrawContent(args);
            }
            catch (System.Exception ext)
            {
                if (DomDocument._DebugMode)
                {
                    DCConsole.Default.WriteLineError(ext.ToString());
                }
                var strErrorMessage = ext.ToString();
                DCConsole.Default.WriteLine("DCWriter内部错误:" + strErrorMessage);
                var opts = this.GetDocumentViewOptions();
                if (opts == null || opts.ShowRedErrorMessageForPaint)
                {
                    // 绘制错误信息
                    args.Graphics.ResetClip();
                    using (var format = new StringFormat())
                    {
                        format.Alignment = StringAlignment.Near;
                        format.LineAlignment = StringAlignment.Near;
                        args.Graphics.DrawString(
                            strErrorMessage,
                            XFontValue.Default,
                            Color.Red,
                            new RectangleF(
                                0,
                                0,
                                this.Width,
                                this.Height),
                            format);
                    }
                }
            }
            finally
            {
                if (bolHasGroupShape)
                {
                    args.Graphics.SVG.EndGroupShape();
                }
            }
        }
        internal void InnerContentElementDrawContent(object objArgs_InnerDocumentPaintEventArgs)
        {
            var args = (InnerDocumentPaintEventArgs)objArgs_InnerDocumentPaintEventArgs;
            RectangleF clipRectangle = args.ClipRectangle;
            RectangleF thisAbsBounds = args.ViewBounds;// this.GetAbsBounds();
            thisAbsBounds.Width = this.ViewWidth;
            clipRectangle = RectangleF.Intersect(clipRectangle, thisAbsBounds);
            if (clipRectangle.IsEmpty)
            {
                // 剪切矩形为空， 则表示状态不对，无法绘图
                return;
            }
            var argsPageClipRectangleIsEmpty = args.PageClipRectangle.IsEmpty;
            DomDocument document = this.OwnerDocument;
            bool showParagraphFlag = args.ShowParagraphFlag;


            DomDocumentContentElement dce = args.DocumentContentElement;
            if (dce == null)
            {
                dce = this.DocumentContentElement;
            }
            bool isInBody = (dce is DomDocumentBodyElement);
            //var thisAbsBounts = this.AbsBounds;
            // 当前容器元素为被选择状态的单元格
            bool ThisIsSelectedCell = this is DomTableCellElement
                && args.IsSelectedCell((DomTableCellElement)this);

            DCSelection selection = dce.Selection;
            var dceSelectionMode = selection.Mode;
            var viewClipRectangle = clipRectangle;// RectangleF.Intersect(thisAbsBounds, args.ClipRectangle);
            if (argsPageClipRectangleIsEmpty == false)
            {
                viewClipRectangle = RectangleF.Intersect(viewClipRectangle, args.PageClipRectangle);
            }

            var privateLinesCount = this._PrivateLines.Count;
            var lineArray = this._PrivateLines.InnerGetArrayRaw();
            var thisAbsBoundsLeft = thisAbsBounds.Left;
            var thisAbsBoundsTop = thisAbsBounds.Top;
            //var thisAbsBoundsBottom = thisAbsBounds.Bottom;
            for (var lineIndex = 0; lineIndex < privateLinesCount; lineIndex++)
            {
                var line = lineArray[lineIndex];
                var lineAbsBoundsLeft = line.Left + thisAbsBoundsLeft;
                var lineAbsBoundsTop = line.Top + thisAbsBoundsTop;
                var lineAbsBoundsWidth = line.Width;
                var lineAbsBoundsHeight = line.Height;
                if (lineAbsBoundsTop + lineAbsBoundsHeight < clipRectangle.Top - 1)
                {
                    continue;
                }
                else if (lineAbsBoundsTop > clipRectangle.Bottom + 2)
                {
                    break;
                }
                if (line.LastElement is DomParagraphFlagElement)
                {
                    // 最后一个元素是段落符号元素，修正行边界的宽度
                    lineAbsBoundsWidth += line.LastElement.ViewWidth;
                }
                bool drawLine = true;
                //lineAbsBounds.Width++;
                //if (clipRectangleIsEmpty == false)
                //{
                var lineAbsBounds = new RectangleF(
                    lineAbsBoundsLeft,
                    lineAbsBoundsTop,
                    lineAbsBoundsWidth,
                    lineAbsBoundsHeight);
                RectangleF cr = RectangleF.Intersect(
                        clipRectangle,
                        lineAbsBounds);
                if (cr.IsEmpty)
                {
                    drawLine = false;
                }
                else if (line.IsCharOrParagraphFlagOnly
                    && args.RenderMode != InnerDocumentRenderMode.Paint
                    && cr.Height < lineAbsBoundsHeight * 0.4f)
                {
                    // 当非UI绘图模式，对于纯文本的行，高度太小则不输出
                    drawLine = false;
                }
                else
                {
                    if (args.RenderMode != InnerDocumentRenderMode.Paint)
                    {
                        if (cr.Height <= 5)
                        {
                            drawLine = false;
                        }
                    }
                }
                //}
                if (drawLine == false)
                {
                    // 文档行超出显示区域范围，不绘制
                    continue;
                }
                if (isInBody && args.Page != null)
                {
                    if (lineAbsBoundsTop > args.PageBottom - 4)
                    {
                        break;
                        //continue;
                    }
                }
                if (isInBody && argsPageClipRectangleIsEmpty == false)
                {
                    RectangleF rect2 = RectangleF.Intersect(args.PageClipRectangle, lineAbsBounds);
                    if (rect2.Height < 10 && rect2.Height < lineAbsBoundsHeight * 0.2)
                    {
                        // 少数情况下，分页计算有误差，使得某行在某页中显示了很小的一部分
                        // 为了美观就不显示了。
                        continue;
                    }
                }
                RectangleFCounter lightRect = null;
                if (args.ActiveMode && args.HighlightSelection)
                {
                    lightRect = new RectangleFCounter();
                }
                var elements = line;
                RectangleF lineClipBounds = lineAbsBounds;
                float lr = lineClipBounds.Right + 20;
                lineClipBounds.X = thisAbsBoundsLeft;
                lineClipBounds.Width = lr - lineClipBounds.Left;
                lineClipBounds = RectangleF.Intersect(lineClipBounds, viewClipRectangle);
                lineClipBounds.Width += 2;
                lineClipBounds.X -= 2;// = lineClipBounds.X - 2;
                var clipVersionBack = args.Graphics.ClipVersion;

                if (lineClipBounds.Top != lineAbsBoundsTop || lineClipBounds.Height != lineAbsBoundsHeight)
                {
                    args.Graphics.SetClip(lineClipBounds);
                }
                var elementsCount = elements.Count;
                var elementsArray = elements.InnerGetArrayRaw();
                for (var elementIndex = 0; elementIndex < elementsCount; elementIndex++)
                {
                    var element = elementsArray[elementIndex];
                    var elementIsCharElement = element is DomCharElement;
                    //// 当前元素是否为字符元素或者段落符号元素
                    //var isCharOrParagraphFlagElement = element is XTextCharElement || element is XTextParagraphFlagElement;
                    // 当前元素是否为字符元素、段落符号元素或者输入域边界元素
                    var elementIsFieldBorderElement = elementIsCharElement == false && element is DomFieldBorderElement;
                    var isCharOrParagraphFlagOrFieldBorderElement =
                        elementIsCharElement
                        || elementIsFieldBorderElement
                        || element is DomParagraphFlagElement
                        || element is DomFieldBorderElement;

                    if (args.RenderMode == InnerDocumentRenderMode.Print)
                    {
                        // 处于打印模式
                        if (elementIsCharElement)
                        {
                            if (((DomCharElement)element).IsBackgroundText)
                            {
                                // 背景文本
                                var field = element.Parent as DomInputFieldElementBase;
                                if (field.RuntimePrintBackgroundText() == false)
                                {
                                    continue;
                                }
                                //if( args.DrawFieldBackgroundText == false )
                                //{
                                //    continue;
                                //}
                            }
                        }
                    }
                    var elementRuntimeStyle = element.RuntimeStyle;
                    // 遍历文档行中的元素，绘制图形
                    if (isCharOrParagraphFlagOrFieldBorderElement)
                    {
                        if (element._Width <= 0)
                        {
                            // 宽度为0，不绘制。
                            continue;
                        }
                    }
                    else if (element.Width <= 0)
                    {
                        // 宽度为0，不绘制。
                        continue;
                    }
                    float elementAbsLeft;
                    float elementAbsTop;
                    float elementViewWidth;
                    if (elementIsCharElement)
                    {
                        elementAbsLeft = lineAbsBoundsLeft + element._Left;
                        elementAbsTop = lineAbsBoundsTop + element._Top;
                        elementViewWidth = element._Width;
                    }
                    else
                    {
                        elementAbsLeft = lineAbsBoundsLeft + element.Left;
                        elementAbsTop = lineAbsBoundsTop + element.Top;
                        elementViewWidth = element.ViewWidth;
                    }
                    RectangleF bounds = new RectangleF(
                       elementAbsLeft,// lineAbsBounds.Left + element.Left ,
                       lineAbsBoundsTop + line._ContentTopFix,
                       elementViewWidth + element._WidthFix,
                       line._Height - line._ContentTopFix);
                    var elementVisibleAbsBounds = RectangleF.Intersect(clipRectangle, bounds);
                    if (elementVisibleAbsBounds.IsEmpty == false)
                    {
                        if (args.IsPrintRenderMode)
                        {
                            if ((element is DomTableElement) == false)
                            {
                                // 计算绘制区域最小外切矩形
                                document.AddDrawContentBounds(elementVisibleAbsBounds);
                            }
                        }

                        args.Element = element;
                        args.Style = elementRuntimeStyle;
                        RectangleF ab = new RectangleF(
                            elementAbsLeft,// lineAbsBounds.Left + element.Left,
                            elementAbsTop,// lineAbsBounds.Top + element.Top,
                            elementViewWidth,
                            element.Height);
                        //element.AbsBounds;
                        args.ViewBounds = ab;
                        if (isCharOrParagraphFlagOrFieldBorderElement == false)
                        {
                            ab.X = ab.X + elementRuntimeStyle.PaddingLeft;
                            ab.Y = ab.Y + elementRuntimeStyle.PaddingRight;
                            ab.Width = element.ClientWidth;
                            ab.Height = element.ClientHeight;
                            args.ClientViewBounds = ab;
                        }
                        bool showElement = true;
                        if (showParagraphFlag == false
                            && elementIsCharElement == false
                            && element is DomParagraphFlagElement)
                        {
                            showElement = false;
                        }
                        if (showElement)
                        {
                            {
                                if (isCharOrParagraphFlagOrFieldBorderElement)
                                {
                                }
                                else
                                {
                                    args.Graphics.SetClip(
                                        lineClipBounds);
                                }
                                if (element._RuntimeVisible)
                                {
                                    args.ContentElement = this;
                                    if (args.NativeGraphics != null)
                                    {
                                        if (elementIsCharElement || element is DomFieldBorderElement)
                                        {
                                            args.NativeGraphics.BeginCacheFillRectangle();
                                        }
                                        else
                                        {
                                            args.NativeGraphics.EndCacheFillRectangle();
                                        }
                                    }
                                    if (elementIsFieldBorderElement)
                                    {
                                        if (elementIndex > 0 && elementsArray[elementIndex - 1] is DomTableElement)
                                        {
                                        }
                                    }
                                    element.Draw(args);
                                }

                            }
                        }
                        if (lightRect != null)
                        {
                            bool isSelectedFlag = false;
                            if (ThisIsSelectedCell)//  this is XTextTableCellElement && dce.IsSelected(this))
                            {
                                isSelectedFlag = false;
                            }
                            else if (dceSelectionMode != ContentRangeMode.Cell)
                            {
                                isSelectedFlag = selection.Contains(element);// dce.IsSelected(element);
                            }
                            if (isSelectedFlag)
                            {
                                if (dce.Selection.LastElement == element)
                                {
                                    // 对于选择区域的最后一个元素，忽略扩展宽度
                                    RectangleF b2 = bounds;
                                    b2.Width = element.Width;
                                    lightRect.Add(b2.Left, lineAbsBoundsTop, b2.Width, lineAbsBoundsHeight);// b2);
                                }
                                else
                                {
                                    lightRect.Add(bounds.Left, lineAbsBoundsTop, bounds.Width, lineAbsBoundsHeight);// bounds);
                                }
                            }//if
                        }//if
                    }//if
                }//foreach( XTextElement e in line )
                args.NativeGraphics?.EndCacheFillRectangle();
                DrawBorderInfos(args, line);
                if (args.Graphics.ClipVersion != clipVersionBack)
                {
                    args.Graphics.ResetClip();
                }
                if (lightRect != null && lightRect.IsEmpty == false)
                {
                    if (document.EditorControl != null)
                    {
                        RectangleF rect = RectangleF.Empty;
                        if (this is DomTableCellElement)
                        {
                            rect = RectangleF.Intersect(this.GetAbsBounds(), lightRect.Value);
                        }
                        else
                        {
                            rect = lightRect.Value;
                        }

                        document.EditorControl.AddSelectionAreaRectangle(
                            Rectangle.Ceiling(rect));
                    }
                }
                //}
            }//foreach( XTextLine line in myLines )
        }

        private void DrawBorderInfos(InnerDocumentPaintEventArgs args, DomLine line)
        {
            var renderBorders = this.RenderBorderInfos();
            if (renderBorders == null || renderBorders.Count == 0)
            {
                return;
            }

            if (renderBorders == null || renderBorders.Count > 0)
            {
                RectangleF ceBounds = this.GetAbsBounds();
                foreach (RenderBorderInfo info in renderBorders)
                {
                    if (info.Line != line)
                    {
                        continue;
                    }
                    RectangleF bounds = info.Bounds;
                    bounds.Offset(ceBounds.Left, ceBounds.Top);
                    var bounds2 = RectangleF.Intersect(bounds, args.ClipRectangle);
                    // 袁永福2019-1-5：这里额外判断，解决DCWRITER-2346
                    if (bounds2.Height > 5)// bounds.IntersectsWith(args.ClipRectangle))
                    {
                        if (args.PageClipRectangle.IsEmpty == false)
                        {
                            RectangleF rect = RectangleF.Intersect(args.PageClipRectangle, bounds);
                            if (rect.Height < 2)
                            {
                                continue;
                            }
                        }
                        var g3 = args.Graphics.NativeGraphics;
                        if (g3 != null)
                        {
                            var cp = g3._ClipBounds;
                            g3._ClipBounds = RectangleF.Empty;
                            //bounds.Offset(0, 6.25f);
                            info.Style.DrawBorder(args.Graphics, bounds);
                            g3._ClipBounds = cp;
                        }
                        else if (args.Graphics.PDFGraphics != null)
                        {
                            info.Style.DrawBorder(args.Graphics, bounds);
                        }
                    }//if
                }//foreach
            }
        }

        private RenderBorderInfoList _RenderBorderInfos;
        /// <summary>
        /// 更新文档元素边框样式
        /// </summary>
        /// <param name="invalidateView">是否声明视图无效</param>
        public virtual void UpdateElementBorderInfos(bool invalidateView)
        {
            if (invalidateView)
            {
                if (_RenderBorderInfos != null
                    && _RenderBorderInfos.Count > 0
                    && this.OwnerDocument.EditorControl != null)
                {
                    RectangleF bounds = RectangleF.Empty;
                    foreach (RenderBorderInfo info in _RenderBorderInfos)
                    {
                        if (bounds.IsEmpty)
                        {
                            bounds = info.Bounds;
                        }
                        else
                        {
                            bounds = RectangleF.Union(info.Bounds, bounds);
                        }
                    }//foreach
                    this.OwnerDocument.EditorControl.ViewInvalidate(
                        bounds,
                        this.DocumentContentElement.PagePartyStyle);
                }
            }
            _RenderBorderInfos = null;
        }
        private static readonly RenderBorderInfoList _EmptyRenderBorderInfoList = new RenderBorderInfoList();
        /// <summary>
        /// 文档元素边框信息列表
        /// </summary>
        internal virtual RenderBorderInfoList RenderBorderInfos()
        {
            if (this._RenderBorderInfos == _EmptyRenderBorderInfoList)
            {
                return null;
            }
            if (this._RenderBorderInfos == null)
            {
                var resultList = new RenderBorderInfoList();
                //foreach (XTextLine line in this.PrivateLines.FastForEach())
                var lineCount = this._PrivateLines.Count;
                for (var lineIndex = 0; lineIndex < lineCount; lineIndex++)
                {
                    var line = this._PrivateLines[lineIndex];
                    if (line.IsTableLine
                        || line.ParagraphFlagElement == null)
                    {
                        continue;
                    }
                    //RuntimeDocumentContentStyle pBorder = line.ParagraphFlagElement.RuntimeStyle;
                    //if (pBorder.HasFullBorder == false)
                    //{
                    //    pBorder = null;
                    //}
                    RuntimeDocumentContentStyle pBorder = null;
                    if (line.ParagraphFlagElement.RuntimeStyle.HasFullBorder)
                    {
                        pBorder = line.ParagraphFlagElement.RuntimeStyle;
                    }
                    RenderBorderInfo info = null;
                    DomElement lastCharElement = null;
                    RenderBorderInfo lastCharBorderInfo = null;
                    //foreach (XTextElement element in line.FastForEach())
                    var elementCount = line.Count;
                    var eArray = line.InnerGetArrayRaw();
                    for (var elementIndex = 0; elementIndex < elementCount; elementIndex++)
                    {
                        var element = eArray[elementIndex];
                        DomElement parent = null;// element.Parent;
                        if (element is DomCharElement)
                        {
                            if (lastCharElement != null
                                && lastCharElement.EqualsParent(element)
                                && lastCharElement._StyleIndex == element._StyleIndex)
                            {
                                // 使用上一个字符元素的信息来重复利用
                                if (lastCharBorderInfo != null)
                                {
                                    lastCharBorderInfo.Elements.FastAdd2(element);
                                }
                                continue;
                            }
                            lastCharElement = element;
                            parent = element;
                        }
                        else
                        {
                            lastCharElement = null;
                            parent = element.Parent;
                        }
                        RuntimeDocumentContentStyle bStyle = null;
                        while (parent != null && parent != this)
                        {
                            //if (parent is XTextContentElement)
                            //{
                            //    break;
                            //}
                            if (parent.RuntimeStyle.HasVisibleBorder)
                            {
                                // 拥有边框是小概率事件，此处调整代码加速执行
                                bStyle = parent.RuntimeStyle;
                                break;
                            }
                            //RuntimeDocumentContentStyle style = parent.RuntimeStyle;
                            //if (style.HasVisibleBorder)
                            //{
                            //    bStyle = style;
                            //    break;
                            //}
                            parent = parent.Parent;
                        }//while
                        if (bStyle == null)
                        {
                            // 元素本身乜有边框设置，则试图采用段落的边框设置
                            bStyle = pBorder;
                        }
                        if (bStyle == null)
                        {
                            info = null;
                        }
                        else
                        {
                            if (info == null || info.Style.EqualsBorderStyle(bStyle) == false)
                            {
                                info = new RenderBorderInfo();
                                info.Elements.Add(element);
                                info.Line = line;
                                info.Style = bStyle;
                                resultList.Add(info);
                            }
                            else
                            {
                                info.Elements.Add(element);
                            }
                        }
                        lastCharBorderInfo = info;
                    }
                }//foreach
                if (resultList.Count > 0)
                {
                    foreach (RenderBorderInfo info in resultList)
                    {
                        //XTextElement fe = info.Elements.FirstElement;
                        //XTextElement le = info.Elements.LastElement;
                        RectangleF bounds = RectangleF.Empty;
                        RectangleF cdBounds = this.GetAbsBounds();
                        foreach (DomElement element in info.Elements)
                        {
                            if (bounds.IsEmpty)
                            {
                                bounds = element.GetAbsBounds();
                            }
                            else
                            {
                                bounds = RectangleF.Union(bounds, element.GetAbsBounds());
                            }
                        }
                        bounds.Offset(-cdBounds.Left, -cdBounds.Top);
                        //RectangleF bounds = info.Line.AbsBounds;
                        //bounds.X = fe.AbsLeft;
                        //bounds.Width = le.AbsLeft + le.Width - bounds.X;
                        info.Bounds = bounds;
                    }
                    this._RenderBorderInfos = resultList;
                }
                else
                {
                    this._RenderBorderInfos = _EmptyRenderBorderInfoList;
                }
            }//if
            return this._RenderBorderInfos;

        }
        /// <summary>
        /// DCWriter内部使用：为输出HTML/RTF而获得段落行分组信息
        /// </summary>
        /// <param name="includeSelectionOnly">是否只包含选择的内容</param>
        /// <param name="clipRectangle">内容剪切矩形</param>
        /// <returns>分组信息</returns>
        public DCSoft.Common.ListDictionary<DomParagraphFlagElement, List<DomElementList>> InnerGetParagraphInfos(
            bool includeSelectionOnly,
            Rectangle clipRectangle,
            bool forHtmlEditable)
        {
            int paragraphStartLineIndex = 0;
            // 段落分组列表
            var paragraphs = new DCSoft.Common.ListDictionary<DomParagraphFlagElement, List<DomElementList>>();
            if (forHtmlEditable)
            {
                this.FixElements();
                DomElementList elements = new DomElementList();
                foreach (DomElement element in this.Elements)
                {
                    elements.FastAdd2(element);
                    if (element is DomParagraphFlagElement)
                    {
                        List<DomElementList> list = new List<DomElementList>();
                        list.Add(elements);
                        paragraphs[(DomParagraphFlagElement)element] = list;
                        elements = new DomElementList();
                    }
                }
                return paragraphs;
            }

            // 按照所属段落对所有的文本行进行分组
            for (int iCount = 0; iCount < this.PrivateLines.Count; iCount++)
            {
                DomLine line = this.PrivateLines[iCount];
                if (line.LastElement is DomParagraphFlagElement)
                {
                    List<DomElementList> lines = new List<DomElementList>();
                    for (int iCount2 = paragraphStartLineIndex; iCount2 <= iCount; iCount2++)
                    {
                        DomLine line2 = this.PrivateLines[iCount2];
                        // 判断该行是否输出
                        if (clipRectangle.IsEmpty == false)
                        {
                            if (clipRectangle.IntersectsWith(
                                Rectangle.Ceiling(line2.AbsBounds)) == false)
                            {
                                // 该文本行不输出
                                continue;
                            }//if
                        }//if
                        if (includeSelectionOnly)
                        {
                            // 判断是否包含被选中的内容
                            bool output = true;
                            if (includeSelectionOnly)
                            {
                                output = line2.HasContainsSelection();
                            }
                            if (output == false)
                            {
                                continue;
                            }
                        }
                        lines.Add(line2);
                    }//for
                    if (lines.Count > 0)
                    {
                        paragraphs[(DomParagraphFlagElement)line.LastElement] = lines;
                    }
                    paragraphStartLineIndex = iCount + 1;
                }//if
            }//for
            return paragraphs;
        }

        /// <summary>
        /// 段落符号元素列表
        /// </summary>
        private DomElementList _ParagraphsFlags;
        /// <summary>
        /// 获得区间包含的段落对象列表
        /// </summary>
        public DomElementList ParagraphsFlags
        {
            get
            {
                if (_ParagraphsFlags == null)
                {
                    _ParagraphsFlags = new DomElementList();
                    if (this._PrivateContent != null && this._PrivateContent.Count > 0)
                    {
                        foreach (DomElement element in this._PrivateContent)
                        {
                            if (element is DomParagraphFlagElement)
                            {
                                this._ParagraphsFlags.Add(element);
                            }
                        }
                    }
                }
                return this._ParagraphsFlags;
            }
        }

        /// <summary>
        /// 复制对象
        /// </summary>
        /// <param name="Deeply">是否深入复制子对象</param>
        /// <returns>复制品</returns>
        public override DomElement Clone(bool Deeply)
        {
            DomContentElement ce = (DomContentElement)base.Clone(Deeply);
            ce._PrivateContent = null;
            ce._PrivateLines = null;
            ce._RenderBorderInfos = null;
            ce.LayoutInvalidate = true;
            return ce;
        }

        public override void InnerGetText(GetTextArgs args)
        {
            DomElementList elements = this.Elements;
            if (elements != null && elements.Count > 0)
            {
                DomElement lastElement = elements.LastElement;
                if ((lastElement is DomParagraphFlagElement) == false)
                {
                    lastElement = null;
                }
                foreach (DomElement element in elements.FastForEach())
                {
                    if (element != lastElement)
                    {
                        if (args.IncludeHiddenText == false && element.RuntimeVisible == false)
                        {
                            continue;
                        }
                        element.InnerGetText(args);
                    }//if
                }//foreach
            }
        }
        public override void WriteText(WriteTextArgs args)
        {
            DomElementList elements = this.Elements;
            //if (this._PrivateContent != null && this._PrivateContent.Count > 0)
            //{
            //    elements = this._PrivateContent;
            //}
            if (elements != null && elements.Count > 0)
            {
                DomElement lastElement = elements.LastElement;
                if ((lastElement is DomParagraphFlagElement) == false)
                {
                    lastElement = null;
                }
                var len = elements.Count;
                var arr = elements.InnerGetArrayRaw();
                //foreach (XTextElement element in elements.FastForEach())
                for (var iCount = 0; iCount < len; iCount++)
                {
                    var element = arr[iCount];
                    if (element != lastElement)
                    {
                        if (args.IncludeHiddenText == false && element.RuntimeVisible == false)
                        {
                            continue;
                        }
                        element.WriteText(args);
                    }//if
                }//foreach
            }
        }

        /// <summary>
        /// 返回表示对象的文本
        /// </summary>
        public override string Text
        {
            get
            {
                var args = new WriteTextArgs(this._OwnerDocument, true, false);
                this.WriteText(args);
                return args.ToString();
            }
            set
            {
                base.Text = value;
            }
        }
        public override void Dispose()
        {
            base.Dispose();
            if (this._ParagraphsFlags != null)
            {
                this._ParagraphsFlags.ClearAndEmpty();
                this._ParagraphsFlags = null;
            }
            if (this._PrivateContent != null)
            {
                this._PrivateContent.ClearAndEmpty();
                this._PrivateContent = null;
            }
            if (this._PrivateLines != null)
            {
                this._PrivateLines.InnerDispose();
                this._PrivateLines.Capacity = 0;
                this._PrivateLines = null;
            }
            if (this._RenderBorderInfos != null)
            {
                this._RenderBorderInfos.Clear();
                this._RenderBorderInfos.Capacity = 0;
                this._RenderBorderInfos = null;
            }
        }
    }//public class XTextDocumentContent : XTextElementContainer

    /// <summary>
    /// 调用ParticalRefreshLines使用的参数
    /// </summary>
    internal class ParticalRefreshLinesEventArgs
    {
        public ParticalRefreshLinesEventArgs(
            DomElement startElement,
            DomElement endElement,
            VerticalAlignStyle vertialAlign)
        {
            this._StartElement = startElement;
            this._EndElement = endElement;
            this._VertialAlign = vertialAlign;
        }

        internal int _CreateLine_LastFirstIndex;
        internal DomParagraphFlagElement _CreateLine_LastParagraphElement;
        /// <summary>
        /// 强制刷新所有的内容
        /// </summary>
        internal bool ForceRefreshAllContent = false;
        internal float FastClientWidth = 0;
        /// <summary>
        /// 是否检查停止行
        /// </summary>
        public bool CheckStopLine = true;
        private readonly DomElement _StartElement = null;

        public DomElement StartElement
        {
            get { return _StartElement; }
            //set { _StartElement = value; }
        }

        private readonly DomElement _EndElement = null;

        public DomElement EndElement
        {
            get { return _EndElement; }
            //set { _EndElement = value; }
        }

        private readonly VerticalAlignStyle _VertialAlign = VerticalAlignStyle.Top;

        public VerticalAlignStyle VertialAlign
        {
            get { return _VertialAlign; }
            //set { _VertialAlign = value; }
        }
        /// <summary>
        /// 从XTextDocumentContentElement.InnerExecuteLayoutForBlankDocument()调用本功能
        /// </summary>
        internal bool ForBlankDocument = false;

        private bool _LayoutContentOnly = false;
        /// <summary>
        /// 只对内容进行排版，不调整容器元素的高度和其他属性,不重绘文档视图。
        /// </summary>
        public bool LayoutContentOnly
        {
            get
            {
                return _LayoutContentOnly;
            }
            set
            {
                _LayoutContentOnly = value;
            }
        }

        private bool _HasCalculate = false;
        public bool HasCalculate
        {
            get
            {
                return this._HasCalculate;
            }
            set
            {
                this._HasCalculate = value;
            }
        }
    }
}