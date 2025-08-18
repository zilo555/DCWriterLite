using System;
using System.Collections;
using System.Collections.Generic;
using DCSoft.Writer.Controls;
using DCSoft.Writer.Data;
using System.Runtime.InteropServices;
using System.ComponentModel;
using DCSoft.Printing;
using DCSoft.Drawing;

namespace DCSoft.Writer.Dom
{
    /// <summary>
    /// 文档内容管理对象
    /// </summary>
#if !RELEASE
    [System.Diagnostics.DebuggerDisplay("Count={ Count }")]
    [System.Diagnostics.DebuggerTypeProxy(typeof(DCSoft.Common.ListDebugView))]
#endif
    public partial class DCContent : DomElementList
    {
        /// <summary>
        /// 初始化对象
        /// </summary>
        public DCContent(DomDocumentContentElement dce)
        {
            if (dce == null)
            {
                throw new ArgumentNullException("dce");
            }
            this._DocumentContentElement = dce;
        }

        /// <summary>
		/// 初始化对象
		/// </summary>
        internal DCContent(DomDocumentContentElement dce, int capacity) : base(capacity)
        {
            if (dce == null)
            {
                throw new ArgumentNullException("dce");
            }
            this._DocumentContentElement = dce;
        }

        internal bool ContainsUseContentIndex(DomElement element)
        {
            if (element != null)
            {
                if (this.SafeGet(element._ContentIndex) == element)
                {
                    return true;
                }
                else
                {
                    return base.Contains(element);
                }
            }
            return false;
        }

        internal DomElementList LayoutElements()
        {
            return this;
        }

        /// <summary>
        /// 对象所属的文档对象
        /// </summary>
        public virtual DomDocument OwnerDocument
        {
            get
            {
                return this._DocumentContentElement.OwnerDocument;
            }
        }

        private readonly DomDocumentContentElement _DocumentContentElement = null;
        /// <summary>
        /// 对象所属的文档区域对象
        /// </summary>
        public DomDocumentContentElement DocumentContentElement
        {
            get
            {
                return _DocumentContentElement;
            }
        }

        private bool _LineEndFlag = false;
        /// <summary>
        /// 光标在行尾标记
        /// </summary>
        public bool LineEndFlag
        {
            get
            {
                return _LineEndFlag;
            }
            set
            {
                _LineEndFlag = value;
                if (_LineEndFlag)
                {
                    //System.Console.Write("");
                }
            }
        }


        /// <summary>
        /// 内容清空事件
        /// </summary>
        new public void Clear()
        {
            _MatchContentElement_GetLineAt = null;
            {
                base.Clear();
                _AutoClearSelection = true;
                _LineEndFlag = false;
            }
        }

        /// <summary>
        /// 选中区域
        /// </summary>
        public DCSelection Selection
        {
            get
            {
                return this._DocumentContentElement.Selection;
            }
        }
        /// <summary>
        /// 选择区域开始位置
        /// </summary>
        public int SelectionStartIndex
        {
            get
            {
                return this._DocumentContentElement.Selection.StartIndex;
            }
        }
        /// <summary>
        /// 选择区域长度
        /// </summary>
        public int SelectionLength
        {
            get
            {
                return this._DocumentContentElement.Selection.Length;
            }
        }
        /// <summary>
        /// 修正区域范围，避免超出边界
        /// </summary>
        /// <param name="startIndex">区域起始序号</param>
        /// <param name="length">区域长度</param>
        internal void FixRange(ref int startIndex, ref int length)
        {
            if (this.Count == 0)
            {
                startIndex = 0;
                length = 0;
            }
            else
            {
                int endIndex = startIndex + length;
                if (startIndex >= this.Count)
                {
                    startIndex = this.Count - 1;
                }
                if (startIndex < 0)
                {
                    startIndex = 0;
                }
                if (endIndex >= this.Count)
                {
                    endIndex = this.Count - 1;
                }
                if (endIndex < 0)
                {
                    endIndex = 0;
                }
                length = endIndex - startIndex;
            }
        }
        /// <summary>
        /// 获得当前插入点的信息
        /// </summary>
        /// <param name="container">插入点所在的容器对象</param>
        /// <param name="elementIndex">插入点所在的容器元素子元素列表中的序号</param>
        public void GetCurrentPositionInfo(out DomContainerElement container, out int elementIndex)
        {
            if (this.Selection == null)
            {
                container = null;
                elementIndex = 0;
                return;
            }
            GetPositonInfo(
                this.Selection.StartIndex,
                out container,
                out elementIndex,
                this.LineEndFlag);
        }

        /// <summary>
        /// 获得位置
        /// </summary>
        /// <param name="container">容器对象</param>
        /// <param name="elementIndex">元素编号</param>
        /// <param name="lineEndFlag">行尾标记</param>
        /// <returns>位置</returns>
        public int GetPosition(
            DomContainerElement container,
            int elementIndex,
            bool lineEndFlag)
        {
            if (container == null)
            {
                throw new ArgumentNullException("container");
            }
            if (elementIndex <= 0)
            {
                elementIndex = container.FirstContentElement.ContentIndex;
            }
            else if (elementIndex >= container.Elements.Count)
            {
                elementIndex = container.LastContentElement.ContentIndex;
            }
            else
            {
                DomElement element = container.Elements[elementIndex];
                if (element is DomParagraphFlagElement)
                {
                    elementIndex = this.IndexOfUseContentIndex(element);
                }
                else
                {
                    elementIndex = element.FirstContentElement.ContentIndex;
                }
            }
            return elementIndex;
        }

        /// <summary>
        /// 根据指定的插入点的位置获得插入点信息
        /// </summary>
        /// <param name="contentIndex">插入点位置</param>
        /// <param name="container">插入点所在的容器元素对象</param>
        /// <param name="elementIndex">插入点在容器元素列表中的位置</param>
        /// <param name="lineEndFlag">行尾标记</param>
        public void GetPositonInfo(
            int contentIndex,
            out DomContainerElement container,
            out int elementIndex,
            bool lineEndFlag)
        {
            if (this.Count == 0)
            {
                container = null;
                elementIndex = 0;
                return;
            }
            if (contentIndex < 0)
            {
                contentIndex = 0;
            }
            if (contentIndex >= this.Count)
            {
                contentIndex = this.Count - 1;
            }
            if (contentIndex < 0 || contentIndex > this.Count)
            {
                throw new ArgumentOutOfRangeException("contentIndex=" + contentIndex);
            }
            if (contentIndex >= this.Count)
            {
                // 当前位置为最后一个文档元素之后。
                container = this.DocumentContentElement;
                elementIndex = this.DocumentContentElement.Elements.Count;
                return;
            }
            DomElement element = this[contentIndex];
            container = element.Parent;
            elementIndex = container.Elements.IndexOfUsePrivateContentIndex(element);
            if (container is DomFieldElementBase)
            {
                DomFieldElementBase field = (DomFieldElementBase)container;
                DomElement startElement = field.StartElement;
                DomElement endElement = field.EndElement;
                if (startElement.LayoutIndex > endElement.LayoutIndex)
                {
                    // 从右到左排版
                    startElement = field.EndElement;
                    endElement = field.StartElement;
                }
                if (startElement == element)
                {
                    // 指定元素是某个字段元素的开始元素
                    container = field.Parent;
                    element = field;
                    elementIndex = container.Elements.IndexOfUsePrivateContentIndex(field);
                }
                else if (endElement == element)
                {
                    container = field;
                    elementIndex = field.Elements.Count;
                    if (contentIndex > 0)
                    {
                        var preElement2 = this[contentIndex - 1];
                        var index11 = field.Elements.IndexOf(preElement2);
                        if (index11 >= 0 && index11 < field.Elements.Count)
                        {
                            elementIndex = index11 + 1;
                        }
                    }
                }
                else if (elementIndex < 0)
                {
                    elementIndex = 0;
                }
            }
            else if (lineEndFlag)
            {
                if (contentIndex == 0)
                {
                    contentIndex = 1;
                    //System.Console.Write("");
                }
                DomElement preElement = this[contentIndex - 1];
                container = preElement.Parent;
                if (container is DomFieldElementBase)
                {
                    DomFieldElementBase field = (DomFieldElementBase)container;
                    DomElement startElement = field.StartElement;
                    DomElement endElement = field.EndElement;
                    if (startElement.LayoutIndex > endElement.LayoutIndex)
                    {
                        // 从右到左排版
                        startElement = field.EndElement;
                        endElement = field.StartElement;
                    }
                    if (endElement == preElement)
                    {
                        container = field.Parent;
                        elementIndex = container.Elements.IndexOfUsePrivateContentIndex(field) + 1;
                        return;
                    }
                    else if (elementIndex < 0)
                    {
                        elementIndex = 0;
                    }
                }
                elementIndex = container.Elements.IndexOfUsePrivateContentIndex(preElement) + 1;
            }
        }


        /// <summary>
        /// 根据指定的插入点的位置获得插入点信息
        /// </summary>
        /// <param name="contentIndex">插入点位置</param>
        /// <param name="container">插入点所在的容器元素对象</param>
        /// <param name="elementIndex">插入点在容器元素列表中的位置</param>
        /// <param name="lineEndFlag">行尾标记</param>
        public void GetPositonInfo(
            DomElement currentElement,
            out DomContainerElement container,
            out int elementIndex,
            bool lineEndFlag)
        {
            if (this.Count == 0)
            {
                container = null;
                elementIndex = 0;
                return;
            }
            if (currentElement == null)
            {
                container = null;
                elementIndex = 0;
                return;
            }
            if (currentElement == this.LastElement)
            {
                // 当前元素为最后一个文档元素。
                container = this.DocumentContentElement;
                elementIndex = this.DocumentContentElement.Elements.Count;
                return;
            }
            container = currentElement.Parent;
            elementIndex = container.Elements.IndexOfUsePrivateContentIndex(currentElement);
            if (container is DomFieldElementBase)
            {
                DomFieldElementBase field = (DomFieldElementBase)container;
                DomElement startElement = field.StartElement;
                DomElement endElement = field.EndElement;
                if (startElement.LayoutIndex > endElement.LayoutIndex)
                {
                    // 从右到左排版
                    startElement = field.EndElement;
                    endElement = field.StartElement;
                }
                if (startElement == currentElement)
                {
                    // 指定元素是某个字段元素的开始元素
                    container = field.Parent;
                    currentElement = field;
                    elementIndex = container.Elements.IndexOfUsePrivateContentIndex(field);
                }
                else if (endElement == currentElement)
                {
                    container = field;
                    elementIndex = field.Elements.Count;
                }
                else if (elementIndex < 0)
                {
                    elementIndex = 0;
                }
            }
            else if (lineEndFlag)
            {
                int contentIndex = this.IndexOfUseContentIndex(currentElement);
                if (contentIndex == 0)
                {
                    contentIndex = 1;
                    //System.Console.Write("");
                }
                DomElement preElement = this[contentIndex - 1];
                container = preElement.Parent;
                if (container is DomFieldElementBase)
                {
                    DomFieldElementBase field = (DomFieldElementBase)container;
                    if (field.EndElement == preElement)
                    {
                        container = field.Parent;
                        elementIndex = container.Elements.IndexOfUsePrivateContentIndex(field) + 1;
                        return;
                    }
                    else if (elementIndex < 0)
                    {
                        elementIndex = 0;
                    }
                }
                elementIndex = container.Elements.IndexOfUsePrivateContentIndex(preElement) + 1;
            }
        }

        private bool _AutoClearSelection = true;
        /// <summary>
        /// DCWriter内部使用。是否自动清除选择状态,若为True则插入点位置修改时会自动设置SelectionLength属性，否则会根据
        /// 旧的插入点的位置计算SelectionLength长度
        /// </summary>
        public bool AutoClearSelection
        {
            get
            {
                return _AutoClearSelection;
            }
            set
            {
                _AutoClearSelection = value;
            }
        }

        /// <summary>
        /// 删除当前元素前一个元素
        /// </summary>
        /// <param name="raiseEvent">是否触发事件</param>
        /// <returns>若删除成功,则返回新的插入点的位置,若操作失败则返回-1</returns>
        public int DeletePreElement(bool raiseEvent)
        {
            if (this.Count == 0)
            {
                return -1;
            }

            DomElement myElement = this.CurrentElement;
            var back_CurrentElement = myElement as DomFieldBorderElement;
            DomElement ceFirstElement = myElement.ContentElement.PrivateContent.FirstElement;
            if (ceFirstElement == myElement)
            {
                if (this.LineEndFlag == false)
                {
                    // 若当前位置为文本块的第一个位置，则不能跨区域删除上一个文本块的最后一个文档元素
                    //this.OwnerDocument.CurrentProtectedElements.Add(myElement);
                    return -1;
                }
            }
            myElement = this.GetPreLayoutElement(myElement);
            if (myElement == null)
            {
                return -1;
            }
            DomContentElement ce = myElement.ContentElement;
            var document = this.OwnerDocument;
            if (document.DocumentControler.CanDelete(myElement) == false)
            {
                // 元素不能删除
                document.DocumentControler.AddLastContentProtectdInfoOnce(document.ContentProtectedInfos);

                //this.OwnerDocument.ContentProtectedInfos.Add(
                //    myElement,
                //    this.OwnerDocument.DocumentControler.GetLastContentProtectedMessageOnce());
                return -1;
            }
            DomParagraphFlagElement pe1 = GetParagraphEOFElement(myElement);
            DomParagraphFlagElement pe2 = GetParagraphEOFElement(this.CurrentElement);
            DomContainerElement c = myElement.Parent;
            var defaultNewSelectionIndex = this.Selection.StartIndex - 1;
            int index = ce.PrivateContent.IndexOfUsePrivateContentIndex(myElement);// c.Elements.IndexOf(myElement);
            if (myElement is DomCheckBoxElementBase)
            {
                var fce4 = myElement.FirstContentElementInPublicContent;
                if (fce4 != myElement)
                {
                    index = ce.PrivateContent.IndexOfUsePrivateContentIndex(fce4);
                    defaultNewSelectionIndex = fce4.ContentIndex;
                }
            }
            DomElement currentElementBack = this.CurrentElement;
            ContentChangedEventArgs cea = new ContentChangedEventArgs();
            int selStartIndex = this.Selection.StartIndex;
            if (InnerRemoveElement(c, myElement, raiseEvent, ref cea))
            {
                DomDocumentContentElement._InsertOrDeleteTextOnlyFlag = myElement is DomCharElement;
                if (myElement is DomCharElement || myElement is DomParagraphFlagElement)
                {
                }
                try
                {
                    document.CacheOptions();
                    this.LineEndFlag = false;
                    document.Modified = true;
                    {
                        this.SetParagraphSettings(pe1, pe2);
                    }

                    var args2 = new DomContentElement.UpdateContentElementsArgs();
                    args2.UpdateParentContentElement = true;
                    args2.FastMode = false;
                    args2.UpdateElementsVisible = false;
                    args2.SimpleTextMode = myElement is DomCharElement;
                    if ((myElement is DomContainerElement) == false)
                    {
                        document._ReplaceElements_CurrentContainer = c;
                    }
                    ce.UpdateContentElements(args2);
                    //ce.UpdateContentElements(true);
                    //this.OwnerDocument.ContentStyles.ResetFastValue();
                    ce.RefreshPrivateContent(index, -1, false, false);
                    var cc2 = this.DocumentContentElement.Content;
                    //XTextInputFieldElementBase.Clear_Cache_FixBorderElementWidth();
                    if (myElement is DomParagraphFlagElement)
                    {
                        this.DocumentContentElement.SetSelection(selStartIndex - 1, 0);
                        //return this.Selection.StartIndex ;
                    }
                    else
                    {
                        if (currentElementBack != null && this.IndexOfUseContentIndex(currentElementBack) >= 0)// this.Contains( currentElementBack ))
                        {
                            int newIndex = this.IndexOfUseContentIndex(currentElementBack);
                            document.DisableFocusEventOnce = true;
                            this.DocumentContentElement.SetSelection(newIndex, 0);
                            document.DisableFocusEventOnce = false;
                        }
                        else
                        {
                            if (back_CurrentElement != null && back_CurrentElement.Position == BorderElementPosition.End)
                            {
                                var ce56 = this.DocumentContentElement.Content;
                                if (this.SafeGet(back_CurrentElement.ContentIndex) == back_CurrentElement)
                                {
                                    var c2 = this.SafeGet(defaultNewSelectionIndex);
                                    if (c2 is DomCharElement
                                        && ((DomCharElement)c2).IsBackgroundText
                                        && c2.Parent == back_CurrentElement.Parent)
                                    {
                                        defaultNewSelectionIndex = back_CurrentElement.ContentIndex;
                                    }
                                }
                            }
                            this.DocumentContentElement.SetSelection(defaultNewSelectionIndex, 0);
                        }
                        //return this.Selection.StartIndex ;
                    }
                    c.RaiseBubbleOnContentChanged(cea);

                    if (this[this.Selection.StartIndex] is DomFieldBorderElement)
                    {
                        DomFieldBorderElement fb = (DomFieldBorderElement)this[this.Selection.StartIndex];
                        DomInputFieldElementBase field = fb.Parent as DomInputFieldElementBase;
                        if (field != null && field.EndElement == fb)
                        {
                            // 当前元素是一个输入域的结束元素
                        }
                    }
                }
                finally
                {
                    DomDocumentContentElement._InsertOrDeleteTextOnlyFlag = false;
                    document._ReplaceElements_CurrentContainer = null;
                    document.ClearCachedOptions();
                }
                return this.Selection.StartIndex;
            }
            return -1;
        }

        /// <summary>
        /// 内部的删除一个文档元素
        /// </summary>
        /// <param name="c">容器对象</param>
        /// <param name="element">要删除的元素对象</param>
        /// <param name="raiseEvent">是否触发文档ContentChanging , ContentChanged事件</param>
        /// <param name="changedArgs">参数</param>
        /// <returns>操作是否成功</returns>
		private bool InnerRemoveElement(
            DomContainerElement c,
            DomElement element,
            bool raiseEvent,
            ref ContentChangedEventArgs changedArgs)
        {
            var document = this.OwnerDocument;
            int index = c.Elements.FastIndexOf(element);//.IndexOf( element );
            if (raiseEvent)
            {
                ContentChangingEventArgs args = new ContentChangingEventArgs();
                args.Document = document;
                args.Element = c;
                args.ElementIndex = index;
                args.DeletingElements = new DomElementList();
                args.DeletingElements.Add(element);
                c.RaiseBubbleOnContentChanging(args);
                if (args.Cancel)
                {
                    // 用户取消操作
                    return false;
                }
            }
            DCSoft.Writer.Dom.Undo.XTextUndoReplaceElements undo = null;
            bool deleteResult = false;
                // 物理删除
                if (document.CanLogUndo)
                {
                    // 记录撤销操作信息
                    DomElementList list = new DomElementList();
                    list.Add(element);
                    undo = new DCSoft.Writer.Dom.Undo.XTextUndoReplaceElements(c, index, list, null);
                    undo.Document = document;
                    undo.InGroup = true;
                }
                deleteResult = c.RemoveChild(element);
                c.ResetChildElementStats();
            if (deleteResult)
            {
                // 更新文档内容快照
                if (element is DomParagraphFlagElement && element.RuntimeStyle.IsListNumberStyle)
                {
                    c.DocumentContentElement.ParagraphTreeInvalidte = true;
                    if (document.CanLogUndo)
                    {
                        document.UndoList.AddMethod(DCSoft.Writer.Dom.Undo.UndoMethodTypes.RefreshParagraphTree);
                    }
                }
                //this.OwnerDocument.ContentSnapshot().UpdateContentState(element);
                // 删除相关的高亮度显示区域
                document.HighlightManager?.Remove(element);

                // 更新文档元素的版本号
                c.UpdateContentVersion();
                if (undo != null && document.CanLogUndo)
                {
                    document.AddUndo(undo);
                }
                if (changedArgs != null)
                {
                    // 触发事件
                    changedArgs.Document = document;
                    changedArgs.Element = c;
                    changedArgs.ElementIndex = index;
                    changedArgs.DeletedElements = new DomElementList();
                    changedArgs.DeletedElements.Add(element);
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// 删除元素
        /// </summary>
        /// <param name="raiseEvent">是否触发事件</param>
        /// <returns>若删除成功,则返回新的插入点的位置,若操作是否则返回-1</returns>
        public int DeleteCurrentElement(bool raiseEvent)
        {
            if (this.Count == 0)
            {
                return -1;
            }
            DomElement myElement = this.CurrentElement;
            if (myElement == null)
            {
                return -1;
            }
            DomContentElement ce = myElement.ContentElement;
            var document = this.OwnerDocument;
            //XTextDocumentContentElement dce = myElement.DocumentContentElement;
            if (document.DocumentControler.CanDelete(myElement) == false)
            {
                // 元素不能删除
                document.DocumentControler.AddLastContentProtectdInfoOnce(document.ContentProtectedInfos);
                //this.OwnerDocument.ContentProtectedInfos.Add(
                //    myElement ,
                //    this.OwnerDocument.DocumentControler.GetLastContentProtectedMessageOnce());
                return -1;
            }
            if (ce.PrivateContent.LastElement == myElement)
            {
                // 文本块区域中最后一个段落符号不能删除
                return -1;
            }
            DomParagraphFlagElement cpe = myElement.OwnerParagraphEOF;
            DomContainerElement c = myElement.Parent;
            int index = ce.PrivateContent.IndexOfUsePrivateContentIndex(myElement);
            DomElement elementBack = myElement.OwnerLine.GetNextElement(myElement);// this.GetNextElement(myElement);
            if (DomLine.__NewLayoutMode)
            {
                elementBack = this.GetNextLayoutElement(myElement);
            }
            if (elementBack == null)
            {
                elementBack = this.GetNextElement(myElement);
            }
            if (elementBack == null)
            {
                elementBack = this.LastElement;
            }
            ContentChangedEventArgs cea = new ContentChangedEventArgs();
            DomParagraphFlagElement nextP = null;
            if (myElement is DomParagraphFlagElement)
            {
                var thisCount = this.Count;
                for (var iCount = this.FastIndexOf(myElement) + 1; iCount < thisCount; iCount++)
                {
                    if (this[iCount] is DomParagraphFlagElement)
                    {
                        nextP = (DomParagraphFlagElement)this[iCount];
                        break;
                    }
                }
            }
            if (InnerRemoveElement(c, myElement, raiseEvent, ref cea))
            {
                if (nextP != null && nextP.RuntimeStyle.IsListNumberStyle)
                {
                    this.DocumentContentElement.ParagraphTreeInvalidte = true;
                }
                DomDocumentContentElement._InsertOrDeleteTextOnlyFlag = myElement is DomCharElement;
                if (myElement is DomCharElement || myElement is DomParagraphFlagElement)
                {
                }
                try
                {
                    document.CacheOptions();
                    this.LineEndFlag = false;
                    DomElement e2 = this.SafeGet(this.Selection.StartIndex + 1);
                    if (e2 != null)
                    {
                        SetParagraphSettings(cpe, e2.OwnerParagraphEOF);
                    }
                    document.Modified = true;
                    DomContentElement.UpdateContentElementsArgs args2
                        = new DomContentElement.UpdateContentElementsArgs();
                    args2.UpdateParentContentElement = true;
                    args2.FastMode = false;
                    args2.UpdateElementsVisible = false;
                    args2.SimpleTextMode = myElement is DomCharElement;
                    ce.UpdateContentElements(args2);
                    if ((myElement is DomContainerElement) == false)
                    {
                        document._ReplaceElements_CurrentContainer = c;
                    }
                    ce.RefreshPrivateContent(index);

                    if (raiseEvent)
                    {
                        c.RaiseBubbleOnContentChanged(cea);
                    }
                    if (elementBack != null && this.Contains(elementBack))
                    {
                        this.DocumentContentElement.SetSelection(this.IndexOfUseContentIndex(elementBack), 0);
                    }
                    else
                    {
                        this.DocumentContentElement.SetSelection(this.Selection.StartIndex, 0);
                    }
                }
                finally
                {
                    DomDocumentContentElement._InsertOrDeleteTextOnlyFlag = false;
                    document._ReplaceElements_CurrentContainer = null;
                    document.ClearCachedOptions();
                }
                return this.Selection.StartIndex;
            }//if
            return -1;
        }

        private void SetParagraphSettings(
            DomParagraphFlagElement source,
            DomParagraphFlagElement desc)
        {
            if (source == desc || source == null || desc == null)
            {
                return;
            }
            if (this.OwnerDocument.CanLogUndo)
            {
                    this.OwnerDocument.UndoList.AddStyleIndex(desc, desc.StyleIndex, source.StyleIndex);
            }
            desc.StyleIndex = source.StyleIndex;
        }

        /// <summary>
        /// 删除文档树状结构
        /// </summary>
        /// <param name="rootElement">要删除的根节点</param>
        /// <param name="startIndexs">用于记录容器元素刷新其内容的起始序号的字典</param>
        /// <param name="detect">检测是否可以执行删除元素操作,但不真的进行删除</param>
        /// <param name="specifySelection">指定的操作元素</param>
        /// <param name="changedArgs">参数</param>
        /// <returns>删除的文档元素个数</returns>
        private int InnerDeleteSelectionDomTree(
             DomContainerElement rootElement,
             Dictionary<DomContentElement, int> startIndexs,
             bool detect,
             DCSelection specifySelection,
             List<ContentChangedEventArgs> changedArgs)
        {
            DomContentElement ce = rootElement.ContentElement;
            if (rootElement is DomContentElement)
            {
                ce = (DomContentElement)rootElement;
            }
            var document = this.OwnerDocument;
            DomElementList pce = ce.PrivateContent;
            DomElementList selectElements = specifySelection.ContentElements;
            var controler = document.DocumentControler;
            // 获得开始删除和结束删除的区域的标记元素列表
            //XTextElementList flagElements = new XTextElementList();
            // 首先获得所有子元素的选择状态
            SelectionPartialType[] selectionTypes =
                new SelectionPartialType[rootElement.Elements.Count];
            int startIndex = 0;
            DomElement element2 = selectElements[0];
            while (element2 != null)
            {
                if (element2.Parent == rootElement)
                {
                    startIndex = rootElement.Elements.IndexOf(element2);
                    if (startIndex < 0)
                    {
                        startIndex = 0;
                    }
                    break;
                }
                element2 = element2.Parent;
            }

            int endIndex = rootElement.Elements.Count - 1;
            element2 = selectElements.LastElement;
            while (element2 != null)
            {
                if (element2.Parent == rootElement)
                {
                    endIndex = rootElement.Elements.IndexOf(element2);
                    if (endIndex < 0)
                    {
                        endIndex = rootElement.Elements.Count - 1;
                    }
                    break;
                }
                element2 = element2.Parent;
            }

            for (int iCount = startIndex; iCount <= endIndex; iCount++)
            {
                DomElement element = rootElement.Elements[iCount];
                bool deleteable = controler.CanDelete(element);
                if (deleteable)
                {
                    if (element is DomContainerElement)
                    {
                        if (((DomContainerElement)element).CollecProtectedElements(
                            document.ContentProtectedInfos,
                            100))
                        {
                            deleteable = false;
                        }
                    }
                }
                else
                {
                    controler.AddLastContentProtectdInfoOnce(document.ContentProtectedInfos);
                    //this.OwnerDocument.ContentProtectedInfos.Add(
                    //    element, 
                    //    controler.GetLastContentProtectedMessageOnce());
                }
                if (deleteable)
                {
                    if (selectElements.Contains(element))
                    {
                        selectionTypes[iCount] = SelectionPartialType.Both;
                    }
                    else if (element is DomContainerElement)
                    {
                        selectionTypes[iCount] = GetSelectionPartialType(
                            (DomContainerElement)element, specifySelection);
                    }
                    else
                    {
                        selectionTypes[iCount] = SelectionPartialType.None;
                    }
                    if (detect)
                    {
                        if (selectionTypes[iCount] == SelectionPartialType.Both
                            || selectionTypes[iCount] == SelectionPartialType.Partial)
                        {
                            // 仅仅做检测
                            controler.AddLastContentProtectdInfoOnce(null);//.GetLastContentProtectedMessageOnce();
                            this.OwnerDocument.ContentProtectedInfos.Clear();
                            return 1;
                        }
                    }
                }
                else
                {
                    selectionTypes[iCount] = SelectionPartialType.None;
                }
            }//for

            if (detect)
            {
                controler.AddLastContentProtectdInfoOnce(null);
                //controler.GetLastContentProtectedMessageOnce();
                document.ContentProtectedInfos.Clear();
                return 0;
            }

            // 修改所有选择状态为NoContent的对象
            for (int iCount = startIndex; iCount <= endIndex; iCount++)
            {
                if (selectionTypes[iCount] == SelectionPartialType.NoContent)
                {
                    bool preFlag = false;
                    // 向前搜索选中状态
                    SelectionPartialType preType = SelectionPartialType.None;
                    for (int iCount2 = iCount - 1; iCount2 >= endIndex; iCount2--)
                    {
                        if (selectionTypes[iCount2] != SelectionPartialType.NoContent)
                        {
                            preType = selectionTypes[iCount2];
                            preFlag = true;
                            break;
                        }
                    }
                    // 向后搜索选中状态
                    bool nextFlag = false;
                    SelectionPartialType nextType = SelectionPartialType.None;
                    for (int iCount2 = iCount + 1; iCount2 <= endIndex; iCount2++)
                    {
                        if (selectionTypes[iCount2] != SelectionPartialType.NoContent)
                        {
                            nextType = selectionTypes[iCount2];
                            nextFlag = true;
                            break;
                        }
                    }
                    if (preType != SelectionPartialType.None
                        && nextType != SelectionPartialType.None)
                    {
                        // 前后都处于选中状态,则本元素也被选中
                        selectionTypes[iCount] = SelectionPartialType.Both;
                    }
                    if (preFlag == false)
                    {
                        // 向前没找到则用后置状态
                        selectionTypes[iCount] = nextType;
                    }
                    else if (nextFlag == false)
                    {
                        // 向后没找到则使用前置状态
                        selectionTypes[iCount] = preType;
                    }
                    else
                    {
                        // 否则设置为不选中
                        selectionTypes[iCount] = SelectionPartialType.None;
                    }
                }
            }//for

            int result = 0;

            int startRefreshIndex = pce.Count;

            DomElement firstDeleteElement = null;
            DomElement lastDeleteElement = null;
            //List<DCSoft.Writer.Dom.Undo.XTextUndoReplaceElements> undos = new List<DCSoft.Writer.Dom.Undo.XTextUndoReplaceElements>();
            Dictionary<DomElement, SelectionPartialType> selectionTypesTable
                = new Dictionary<DomElement, SelectionPartialType>();
            for (int iCount = startIndex; iCount <= endIndex; iCount++)
            {
                selectionTypesTable[rootElement.Elements[iCount]] = selectionTypes[iCount];
            }

            for (int iCount = startIndex;
                iCount < rootElement.Elements.Count && iCount <= endIndex;
                iCount++)
            {
                DomElement element = rootElement.Elements[iCount];
                if (element.RuntimeVisible == false)
                {
                    continue;
                }
                if (selectionTypesTable[element] == SelectionPartialType.Both)
                {
                    lastDeleteElement = rootElement.Elements[iCount];
                    if (firstDeleteElement == null)
                    {
                        firstDeleteElement = lastDeleteElement;
                    }
                    int index = pce.IndexOfUsePrivateContentIndex(firstDeleteElement.FirstContentElement);
                    if (index >= 0 && startRefreshIndex > index)
                    {
                        startRefreshIndex = index;
                    }
                }
                else
                {
                    if (firstDeleteElement != null)
                    {
                        // 删除已识别的连片的处于全选状态的元素
                        int deleted = DeleteElements(
                            rootElement,
                            firstDeleteElement,
                            lastDeleteElement,
                            true,
                            changedArgs);
                        if (deleted > 0)
                        {
                            //result = result + deleted;
                            endIndex = endIndex - deleted;
                            iCount = iCount - deleted;
                        }
                        firstDeleteElement = null;
                        lastDeleteElement = null;
                        result = result + Math.Abs(deleted);
                        //iCount = rootElement.Elements.IndexOf(element);
                    }
                    if (selectionTypesTable[element] == SelectionPartialType.Partial)
                    {
                        // 深入子节点进行部分内容的删除
                        result = result + InnerDeleteSelectionDomTree(
                            (DomContainerElement)element,
                            startIndexs,
                            detect,
                            specifySelection,
                            changedArgs);
                    }
                    else if (selectionTypesTable[element] == SelectionPartialType.None)
                    {
                        if (element is DomContainerElement)
                        {
                            var c99 = (DomContainerElement)element;
                            {
                                // 容器元素内容不是只读的，则可以深入子节点进行删除操作
                                result = result + InnerDeleteSelectionDomTree(
                                    c99,
                                    startIndexs,
                                    detect,
                                    specifySelection,
                                    changedArgs);
                            }
                        }
                    }
                }
            }//for
            if (firstDeleteElement != null)
            {
                int index = pce.IndexOfUsePrivateContentIndex(firstDeleteElement);
                if (index >= 0 && startRefreshIndex > index)
                {
                    startRefreshIndex = index;
                }
                int deleted = DeleteElements(
                    rootElement,
                    firstDeleteElement,
                    lastDeleteElement,
                    true,
                    changedArgs);
                result = result + Math.Abs(deleted);
            }
            if (startIndexs.ContainsKey(ce))
            {
                startIndexs[ce] = Math.Min(startIndexs[ce], startRefreshIndex);
            }
            else
            {
                startIndexs[ce] = startRefreshIndex;
            }
            return result;
        }

        /// <summary>
        /// 删除元素
        /// </summary>
        /// <param name="container">容器元素</param>
        /// <param name="firstDeleteElement">删除区域的开始元素</param>
        /// <param name="lastDeleteElement">删除区域的结束元素</param>
        /// <param name="raiseEvent">是否触发事件</param>
        /// <param name="changedArgs">参数</param>
        /// <returns>删除的元素个数,若等于0则没有删除任何元素，若大于0则物理删除了元素，若小于0则逻辑删除了元素</returns>
        private int DeleteElements(
            DomContainerElement container,
            DomElement firstDeleteElement,
            DomElement lastDeleteElement,
            bool raiseEvent,
            List<ContentChangedEventArgs> changedArgs)
        {
            try
            {
                var document = this.OwnerDocument;

                int firstDeleteIndex = container.Elements.IndexOf(firstDeleteElement);
                int lastDeleteIndex = container.Elements.IndexOf(lastDeleteElement);
                DomParagraphFlagElement p1 = GetParagraphEOFElement(firstDeleteElement);
                DomParagraphFlagElement p2 = null;

                if (container.Elements.LastElement != lastDeleteElement)
                {
                    p2 = GetParagraphEOFElement(container.Elements.GetNextElement(lastDeleteElement));
                }

                //container.Elements.BeginFastRemove();

                // 要删除的元素列表
                DomElementList deletedElements = container.Elements.GetRange(
                        firstDeleteIndex,
                        lastDeleteIndex - firstDeleteIndex + 1);
                if (deletedElements.Count == 0)
                {
                    return 0;
                }
                // 检测是否要刷新段落层次结构
                foreach (DomElement element in deletedElements)
                {
                    if (element is DomParagraphFlagElement && element.RuntimeStyle.IsListNumberStyle)
                    {
                        this.DocumentContentElement.ParagraphTreeInvalidte = true;
                        if (document.CanLogUndo)
                        {
                            document.UndoList.AddMethod(DCSoft.Writer.Dom.Undo.UndoMethodTypes.RefreshParagraphTree);
                        }
                        break;
                    }
                }
                if (raiseEvent)
                {
                    // 触发事件
                    ContentChangingEventArgs args = new ContentChangingEventArgs();
                    args.Document = document;
                    args.Element = container;
                    args.ElementIndex = firstDeleteIndex;
                    args.DeletingElements = deletedElements;
                    container.RaiseBubbleOnContentChanging(args);
                    if (args.Cancel)
                    {
                        // 用户取消操作
                        return 0;
                    }
                }
                {
                    // 物理删除
                    if (container is DomTableElement)
                    {
                        var table = (DomTableElement)container;
                        table.DOMDeleteRows(firstDeleteIndex, lastDeleteIndex - firstDeleteIndex + 1, true);
                    }
                    else
                    {
                        container.ResetChildElementStats();
                        var es3 = container.Elements;
                        if (document.CanLogUndo)
                        {
                            // 记录撤销信息
                            document.UndoList.AddRemoveElements(
                                container,
                                es3.IndexOf(firstDeleteElement),
                                deletedElements);
                        }
                        if (DomDocumentContentElement._InsertOrDeleteTextOnlyFlag)
                        {
                            for (var iCount = firstDeleteIndex; iCount <= lastDeleteIndex; iCount++)
                            {
                                if (es3[iCount] is DomCharElement)
                                {
                                    DomDocumentContentElement._InsertOrDeleteTextOnlyFlag = false;
                                    break;
                                }
                            }
                        }
                        var firstP = es3[firstDeleteIndex].OwnerParagraphEOF;
                        var lastP = es3[lastDeleteIndex].OwnerParagraphEOF;
                        if (firstP == null
                            || lastP == null
                            || firstP == lastP
                            || firstP.StyleIndex == lastP.StyleIndex)
                        {
                            firstP = null;
                            lastP = null;
                        }
                        es3.RemoveRange(
                            firstDeleteIndex,
                            lastDeleteIndex - firstDeleteIndex + 1);
                        if (firstP != null && lastP != null && es3.Contains(firstP) == false)
                        {
                            if (document.CanLogUndo)
                            {
                                document.UndoList.AddStyleIndex(lastP, lastP.StyleIndex, firstP.StyleIndex);
                            }
                            lastP.StyleIndex = firstP.StyleIndex;
                        }
                    }
                }

                // 更新文档内容快照
                //this.OwnerDocument.ContentSnapshot().UpdateContentState(deletedElements);
                // 删除相关的高亮度显示区域
                if (document.HighlightManager != null)
                {
                    foreach (DomElement element in deletedElements)
                    {
                        if ((element is DomCharElement) == false)
                        {
                            document.HighlightManager.Remove(element);
                        }
                    }
                }
                // 更新容器元素的内容版本号
                container.UpdateContentVersion();
                // 设置删除的元素所在的文本行状态无效
                foreach (DomElement re in deletedElements)
                {
                    if (re.OwnerLine != null)
                    {
                        re.OwnerLine.InvalidateState = true;
                    }
                    else
                    {
                        // 若元素没有所属文本行,则该元素本身没有参与文本排版
                        // 此时检测其包含的文本文档元素所在的文本行无效.
                        if (re is DomContainerElement)
                        {
                            DomContainerElement c2 = (DomContainerElement)re;
                            DomElementList ces = new DomElementList();
                            c2.AppendViewContentElement(new DomContainerElement.AppendViewContentElementArgs(c2.OwnerDocument, ces, false));
                            if (ces.Count > 0)
                            {
                                foreach (DomElement ce in ces)
                                {
                                    if (ce.OwnerLine != null)
                                    {
                                        ce.OwnerLine.InvalidateState = true;
                                    }
                                }
                            }
                        }
                    }
                }
                if (raiseEvent)
                {
                    // 触发事件
                    ContentChangedEventArgs args = new ContentChangedEventArgs();
                    args.Document = document;
                    args.Element = container;
                    args.ElementIndex = firstDeleteIndex;
                    args.DeletedElements = deletedElements;
                    changedArgs.Add(args);
                    //container.RaiseBubbleOnContentChanged(args);
                }
                if (p1 != p2 && p2 != null)
                {
                    SetParagraphSettings(p1, p2);
                }
                {
                    return deletedElements.Count;
                }
            }//try
            finally
            {
                //container.Elements.EndFastRemove();
            }
        }

        /// <summary>
        /// 内容选择状态
        /// </summary>
        private enum SelectionPartialType
        {
            /// <summary>
            /// 内容全部选中
            /// </summary>
            Both,
            /// <summary>
            /// 部分选中
            /// </summary>
            Partial,
            /// <summary>
            /// 没有任何内容被选中
            /// </summary>
            None,
            /// <summary>
            /// 元素没有任何内容,因此等同于周围的选中状态
            /// </summary>
            NoContent
        }

        /// <summary>
        /// 判断指定容器的元素全部被选中
        /// </summary>
        /// <param name="rootElement">容器元素</param>
        /// <param name="specifySelection">指定的选择区域对象</param>
        /// <returns>内容是否被全部选中</returns>
        private SelectionPartialType GetSelectionPartialType(
            DomContainerElement rootElement,
            DCSelection specifySelection)
        {
            if (specifySelection == null)
            {
                specifySelection = this.Selection;
            }
            bool include = false;
            bool exclude = false;
            if ((rootElement is DomTableElement || rootElement is DomTableRowElement)
                && specifySelection.Mode == ContentRangeMode.Cell)
            {
                // 单元格选择模式
                if (rootElement is DomTableElement)
                {
                    DomTableElement table = (DomTableElement)rootElement;
                    foreach (DomTableCellElement cell in table.VisibleCells)
                    {
                        if (specifySelection.ContainsCell(cell))
                        {
                            include = true;
                        }
                        else
                        {
                            exclude = true;
                        }
                        if (include && exclude)
                        {
                            break;
                        }
                    }
                }
                else if (rootElement is DomTableRowElement)
                {
                    var row = (DomTableRowElement)rootElement;
                    foreach (DomTableCellElement cell in row.Cells)
                    {
                        if (specifySelection.ContainsCell(cell))
                        {
                            include = true;
                        }
                        else
                        {
                            exclude = true;
                        }
                        if (include && exclude)
                        {
                            break;
                        }
                    }
                }
            }
            else
            {
                int tick = Environment.TickCount;
                DomElementList cc = new DomElementList();
                rootElement.AppendViewContentElement(new DomContainerElement.AppendViewContentElementArgs(rootElement.OwnerDocument, cc, true));
                if (cc.Count == 0)
                {
                    // 没有任何内容,则
                    return SelectionPartialType.NoContent;
                }
                foreach (DomElement element in cc)
                {
                    if (specifySelection.ContainsContentElement(element))
                    {
                        include = true;
                    }
                    else
                    {
                        exclude = true;
                    }
                    if (include && exclude)
                    {
                        break;
                    }
                }
                tick = Math.Abs(Environment.TickCount - tick);
            }
            SelectionPartialType result = SelectionPartialType.None;
            if (include && exclude)
            {
                // 既存在被选中的又存在没有选中的元素
                result = SelectionPartialType.Partial;
            }
            else if (include && exclude == false)
            {
                result = SelectionPartialType.Both;
                if (rootElement is DomTableCellElement)
                {
                    if (specifySelection.ContainsCell((DomTableCellElement)rootElement) == false)
                    {
                        // 单元格不处于全选状态，只能处于部分选择状态
                        result = SelectionPartialType.Partial;
                    }
                }
                else if (rootElement is DomTableRowElement)
                {
                    //var row = (XTextTableRowElement)rootElement;
                    foreach (DomTableCellElement cell in rootElement.Elements)
                    {
                        if (specifySelection.ContainsCell(cell.OverrideCell == null ? cell : cell.OverrideCell) == false)
                        {
                            // 表格行中不是所有的单元格处于选中状态
                            result = SelectionPartialType.Partial;
                            break;
                        }
                    }
                }
            }
            else if (include == false && exclude)
            {
                result = SelectionPartialType.None;
            }
            return result;
        }
        /// <summary>
        /// 已删除文档树结构的方式来删除选中的元素
        /// </summary>
        /// <param name="raiseEvent">是否触发文档ContentChanging , ContentChanged事件</param>
        /// <param name="detect">检测是否可以执行删除元素操作,但不真的进行删除</param>
        /// <param name="fastMode">快速模式，不更新用户界面，不更新内容元素列表</param>
        /// <returns>删除的文档元素对象个数</returns>
        public int DeleteSelection(
            bool raiseEvent,
            bool detect,
            bool fastMode)
        {
            return DeleteSelection(raiseEvent, detect, fastMode, null);
        }
        /// <summary>
        /// 已删除文档树结构的方式来删除选中的元素
        /// </summary>
        /// <param name="raiseEvent">是否触发文档ContentChanging , ContentChanged事件</param>
        /// <param name="detect">检测是否可以执行删除元素操作,但不真的进行删除</param>
        /// <param name="fastMode">快速模式，不更新用户界面，不更新内容元素列表</param>
        /// <param name="specifySelection">指定的选择区域</param>
        /// <returns>删除的文档元素对象个数</returns>
        public int DeleteSelection(
            bool raiseEvent,
            bool detect,
            bool fastMode,
            DCSelection specifySelection)
        {
            if (this.Count == 0)
            {
                //本对象无任何元素
                return -1;
            }
            if (specifySelection == null)
            {
                specifySelection = this.Selection;
            }
            if (specifySelection.Length == 0)
            {
                // 没有包括任何元素
                return -1;
            }

            DomContainerElement rootElement = null;
            DomElementList parents1 = WriterUtilsInner.GetParentList(
                specifySelection.ContentElements.FirstElement);
            DomElementList parents2 = WriterUtilsInner.GetParentList(
                specifySelection.ContentElements.LastElement);
            for (int iCount = 0; iCount < parents1.Count; iCount++)
            {
                if (parents2.Contains(parents1[iCount]))
                {
                    rootElement = (DomContainerElement)parents1[iCount];
                    if (rootElement is DomFieldElementBase)
                    {
                        DomFieldElementBase field = (DomFieldElementBase)rootElement;
                        if (specifySelection.ContentElements.FirstElement == field.StartElement
                            && specifySelection.ContentElements.LastElement == field.EndElement)
                        {
                            // 刚好选中文档域的全部内容，则设置根元素为上层元素
                            rootElement = rootElement.Parent;
                        }
                    }
                    break;
                }
            }

            if ((rootElement is DomContentElement) == false
                && rootElement.Parent != null)
            {
                if (GetSelectionPartialType(rootElement, specifySelection) == SelectionPartialType.Both)
                {
                    rootElement = rootElement.Parent;
                }
            }
            Dictionary<DomContentElement, int> startIndexs
                = new Dictionary<DomContentElement, int>();
            int selectionIndex = this.IndexOfUseContentIndex(specifySelection.ContentElements[0]);// this.IndexOf(specifySelection.ContentElements[0]);
            DomElement selectionStartElement = this.SafeGet(specifySelection.AbsEndIndex);
            DomTableCellElement singleCell = null;
            if (specifySelection.Mode == ContentRangeMode.Cell
                && specifySelection.Cells != null
                && specifySelection.Cells.Count == 1)
            {
                singleCell = (DomTableCellElement)specifySelection.Cells[0];
            }
            if (selectionStartElement == null)
            {
                selectionStartElement = this.LastElement;
            }
            List<ContentChangedEventArgs> changedArgs = new List<ContentChangedEventArgs>();
            int result = 0;
            DomTableCellElement firstCell = null;
            if ((rootElement is DomTableElement
                || rootElement is DomTableRowElement) && specifySelection.Mode == ContentRangeMode.Cell)
            {
                // 删除单元格中的内容
                foreach (DomTableCellElement cell in specifySelection.Cells)
                {
                    if (cell.RuntimeVisible && cell.IsOverrided == false)
                    {
                        if (firstCell == null)
                        {
                            firstCell = cell;
                        }
                        result += InnerDeleteSelectionDomTree(cell, startIndexs, detect, specifySelection, changedArgs);
                    }
                }
            }
            else
            {
                result = InnerDeleteSelectionDomTree(
                    rootElement,
                    startIndexs,
                    detect,
                    specifySelection,
                    changedArgs);
            }
            if (detect)
            {
                // 仅仅做检测,不进行后续处理
                return result;
            }

            if (result > 0 && fastMode == false)
            {
                var document = this.OwnerDocument;
                bool refreshPage = false;
                try
                {
                    DomContainerElement.GlobalEnabledResetChildElementStats = false;
                    this.LineEndFlag = false;
                    document.Modified = true;
                    foreach (DomContentElement ce in startIndexs.Keys)
                    {
                        int index = startIndexs[ce];
                        ce.UpdateContentElements(false);
                        ce.RefreshPrivateContent(index, -1, true);
                        if (ce.NeedRefreshPage)
                        {
                            refreshPage = true;
                        }
                    }//foreach
                    this.DocumentContentElement.UpdateContentElements(false);
                    this.DocumentContentElement.FillContent();
                    if(this.DocumentContentElement.ParagraphTreeInvalidte )
                    {
                        this.DocumentContentElement.RefreshParagraphListState(true, true);
                    }
                }
                finally
                {
                    DomContainerElement.GlobalEnabledResetChildElementStats = true;
                }
                if (changedArgs.Count > 0)
                {
                    // 集中触发文档内容发生改变事件
                    for (int iCount = changedArgs.Count - 1; iCount >= 0; iCount--)
                    {
                        ContentChangedEventArgs args = changedArgs[iCount];
                        DomContainerElement c = args.Element as DomContainerElement;
                        if (c != null)
                        {
                            c.RaiseBubbleOnContentChanged(args);
                        }
                    }//for
                }
                if (refreshPage)
                {
                    //this.OwnerDocument.PageRefreshed = false;
                    document.RefreshPages();

                    if (document.EditorControl != null)
                    {
                        document.EditorControl.UpdatePages();
                        if (specifySelection != this.Selection)
                        {
                            document.EditorControl.UpdateTextCaret();
                        }
                        document.EditorControl.GetInnerViewControl().Invalidate();
                    }
                }
                else
                {
                    if (document.EditorControl != null)
                    {
                        document.EditorControl.UpdateTextCaret();
                        document.EditorControl.Update();
                    }
                }
                if (specifySelection == this.Selection)
                {
                    if (firstCell != null)
                    {
                        this.SetSelection(this.IndexOfUseContentIndex(firstCell.FirstContentElementInPublicContent), 0);
                    }
                    else
                    {
                        if (singleCell != null && selectionStartElement.IsParentOrSupParent(singleCell) == false)
                        {
                            this.SetSelection(singleCell.FirstContentElement.ContentIndex, 0);
                        }
                        else if (selectionStartElement != null && this.Contains(selectionStartElement))
                        {
                            this.SetSelection(this.IndexOfUseContentIndex(selectionStartElement), 0);
                        }
                        else
                        {
                            this.SetSelection(selectionIndex, 0);
                        }
                    }
                }
            }
            return result;
            //return 0;
        }


        private bool _SelectionStartElementAsCurrentElement = true;
        /// <summary>
        /// 选择区域中的第一个元素作为当前元素
        /// </summary>
        internal bool SelectionStartElementAsCurrentElement
        {
            get
            {
                return _SelectionStartElementAsCurrentElement;
            }
            set
            {
                _SelectionStartElementAsCurrentElement = value;
            }
        }

        /// <summary>
        /// 获得当前元素
        /// </summary>
        public DomElement CurrentElement
        {
            get
            {
                if (this.Count > 0)
                {
                    var sel = this._DocumentContentElement.Selection;
                    if (sel != null)
                    {
                        var asi = sel.AbsStartIndex;
                        if (asi >= 0 && asi < this.Count)
                        {
                            if (this._SelectionStartElementAsCurrentElement)
                            {
                                return SafeGet(sel.StartIndex);
                            }
                            else if (sel.Mode == ContentRangeMode.Cell)
                            {
                                if (sel.Length > 0)
                                {
                                    // 选中的单元格
                                    return sel.ContentElements.LastElement;
                                }
                                else
                                {
                                    return sel.ContentElements.FirstElement;
                                }
                            }
                            else
                            {
                                return SafeGet(sel.StartIndex + sel.Length);
                            }
                            //return SafeGet(this.Selection.AbsStartIndex);
                        }
                    }
                    else
                    {
                        return this[this.Count - 1];
                    }
                }
                return null;
            }
            set
            {
                int index = this.IndexOfUseContentIndex(value);
                if (index >= 0)
                {
                    this.MoveToPosition(index);
                }
                //intSelectionStart = this.FixElementIndex(intSelectionStart);
            }
        }

        /// <summary>
        /// 当前段落结束标记对象
        /// </summary>
        public DomParagraphFlagElement CurrentParagraphEOF
        {
            get
            {
                //if (this.Selection.Length != 0)
                //{

                //}
                return GetParagraphEOFElement(this.CurrentElement);
            }
        }
        /// <summary>
        /// 获得当前位置的前一个元素
        /// </summary>
        public DomElement PreElement
        {
            get
            {
                int index = this.SelectionStartIndex;
                if (this.Count > 0
                    && index > 0
                    && index < this.Count)
                {
                    return this[index - 1];
                }
                else
                {
                    return null;
                }
            }
        }
        /// <summary>
        /// 当前文本行对象
        /// </summary>
        public DomLine CurrentLine
        {
            get
            {
                if (this.Count == 0)
                {
                    return null;
                }
                var ce = this.CurrentElement;
                if (ce != null)
                {
                    DomLine myLine = ce.OwnerLine;
                    if (myLine != null)
                    {
                        if (this._LineEndFlag)
                        {
                            int index2 = this._DocumentContentElement.Lines.FastIndexOf(myLine);
                            if (index2 > 0)
                            {
                                return this._DocumentContentElement.Lines[index2 - 1];
                            }
                        }
                        return myLine;
                    }
                }
                int index = this.SelectionStartIndex;
                if (index >= 0 && index < this.Count)
                {
                    DomLine myLine = this[index].OwnerLine;
                    if (this._LineEndFlag
                        && this._DocumentContentElement.Lines.FastIndexOf(myLine) > 0)
                    {
                        return this._DocumentContentElement.Lines[
                            this._DocumentContentElement.Lines.FastIndexOf(myLine) - 1];
                    }
                    else
                    {
                        return myLine;
                    }
                }
                else
                {
                    return this.LastElement.OwnerLine;
                }
            }
        }

        /// <summary>
        /// 当前行的上一个文本行对象
        /// </summary>
        public DomLine GetPreLine(bool ignoreHeaderRow)
        {
            DomLine myLine = this.CurrentLine;
            if (myLine[0].OwnerCell != null)
            {
                DomTableCellElement cell = (DomTableCellElement)myLine[0].OwnerCell;
                if (cell.PrivateLines[0] == myLine)
                {
                    // 若当前文本行是一个单元格中的第一行，
                    DomTableElement table = cell.OwnerTable;

                    if (cell.RowIndex == 0)
                    {
                        // 若该单元格是表格中的第一个行则获得表格前一行
                        DomTableCellElement firstCell = table.GetCell(0, 0, true);
                        int index = this.DocumentContentElement.Content.IndexOf(firstCell.PrivateContent[0]) - 1;
                        if (index >= 0)
                        {
                            DomLine line = this.DocumentContentElement.Content[index].OwnerLine;
                            if (line.Contains(table))
                            {
                                int index2 = this.DocumentContentElement.Content.IndexOf(line[0]);
                                if (index2 > 0)
                                {
                                    line = this.DocumentContentElement.Content[index2 - 1].OwnerLine;
                                }
                            }
                            return line;
                        }
                        else
                        {
                            return null;
                        }
                    }
                    else
                    {
                        // 获得上一个单元格中的最后一行
                        DomTableRowElement preRow = (DomTableRowElement)table.RuntimeRows.GetPreElement(cell.OwnerRow);
                        DomTableCellElement preCell = (DomTableCellElement)preRow.Cells.SafeGet(cell.ColIndex);// table.GetCell(cell.RowIndex - 1, cell.ColIndex, true);
                        if (preCell == null)
                        {
                            throw new System.Exception("RowIndex:" + cell.RowIndex + " ColIndex:" + cell.ColIndex);
                        }
                        if (preCell.IsOverrided)
                        {
                            preCell = preCell.OverrideCell;
                        }
                        return preCell.PrivateLines.LastLine;
                    }
                }
            }
            if (this.DocumentContentElement.Lines.IndexOf(myLine) > 0)
            {
                for (int iCount = this.SelectionStartIndex - 1; iCount >= 0; iCount--)
                {
                    DomLine line = this[iCount].OwnerLine;
                    if (line != myLine)
                    {
                        if (line.IsTableLine == false)
                        {
                            return line;
                        }
                    }
                }//for
                return null;
            }
            else
            {
                for (int iCount = this.SelectionStartIndex - 1; iCount >= 0; iCount--)
                {
                    DomLine line = this[iCount].OwnerLine;
                    if (line != myLine)
                    {
                        if (line.IsTableLine == false )
                        {
                            return line;
                        }
                    }
                }
                return myLine;
            }
        }

        /// <summary>
        /// 获得当前行的下一个文本行对象
        /// </summary>
        public DomLine GetNextLine()
        {
            DomLine myLine = this.CurrentLine;
            if (myLine[0].OwnerCell != null)
            {
                DomTableCellElement cell = (DomTableCellElement)myLine[0].OwnerCell;
                if (cell.PrivateLines.LastLine == myLine)
                {
                    // 若当前文本行是一个单元格中的最后一行，

                    // 获得表格中最后一个可见单元格
                    DomTableCellElement nextCell = null;
                    DomTableElement table = cell.OwnerTable;
                    DomTableRowElement row = (DomTableRowElement)table.RuntimeRows.SafeGetNextElement(cell.OwnerRow, cell.RowSpan);
                    if (row != null)
                    {
                        nextCell = (DomTableCellElement)row.Cells.SafeGet(cell.ColIndex);
                    }
                    if (nextCell == null)
                    {
                        // 若该单元格下面没有单元格，则获得紧跟着该表格下面的文本行
                        DomTableCellElement lastCell = null;
                        DomTableRowElement lastRow = (DomTableRowElement)table.RuntimeRows.LastElement;
                        if (lastRow != null)
                        {
                            lastCell = (DomTableCellElement)lastRow.Cells.LastElement;
                        }
                        if (lastCell == null)
                        {
                            throw new IndexOutOfRangeException("RowInde:" + cell.RowIndex + " ColIndex:" + cell.ColIndex);
                        }
                        //XTextTableCellElement lastCell = table.GetCell(
                        //    table.Rows.Count - 1,
                        //    table.Columns.Count - 1, 
                        //    true);
                        if (lastCell.IsOverrided)
                        {
                            // 获得合并的单元格
                            lastCell = lastCell.OverrideCell;
                            if (lastCell.RowSpan > 1)
                            {
                                // 若单元格的跨行数大于1，则该单元格不一定是表格中最后一个可见的单元格
                                // 则查找表格中最后一个可见的单元格
                                DomElementList cells = table.Cells;
                                for (int iCount = cells.Count - 1; iCount >= 0; iCount--)
                                {
                                    DomTableCellElement cell2 = (DomTableCellElement)cells[iCount];
                                    if (cell2.IsOverrided == false)
                                    {
                                        lastCell = cell2;
                                        break;
                                    }
                                }//if
                            }//if
                        }//if

                        int index = this.DocumentContentElement.Content.IndexOf(lastCell.PrivateContent.LastElement) + 1;
                        DomLine line = this.DocumentContentElement.Content[index].OwnerLine;
                        if (line.Contains(lastCell.OwnerTable))
                        {
                            int index2 = this.DocumentContentElement.Content.IndexOf(line.LastElement);
                            if (index2 > 0 && index2 < this.DocumentContentElement.Content.Count - 1)
                            {
                                line = this.DocumentContentElement.Content[index2 + 1].OwnerLine;
                            }
                        }
                        return line;
                    }
                    else
                    {
                        // 若该单元格下面有其他单元格，则获得该单元格中的第一个文本行。
                        if (nextCell.OverrideCell != null)
                        {
                            nextCell = nextCell.OverrideCell;
                        }
                        DomLine resultLine = nextCell.PrivateLines[0];
                        while (resultLine.IsTableLine)
                        {
                            DomTableElement subTable = resultLine.TableElement;
                            DomTableCellElement subCell = subTable.GetCell(0, 0, false);
                            if (subCell != null)
                            {
                                resultLine = subCell.PrivateLines[0];
                            }
                        }
                        return resultLine;
                    }
                }
            }
            if (this.DocumentContentElement.Lines.IndexOf(myLine)
                < this.DocumentContentElement.Lines.Count - 1)
            {
                for (int iCount = this.SelectionStartIndex + 1; iCount < this.Count; iCount++)
                {
                    DomLine line = this[iCount].OwnerLine;
                    if (line != myLine)
                    {
                        if (line.IsTableLine == false)
                        {
                            return line;
                        }
                    }
                }//for
                return null;
            }
            return myLine;
        }

        /// <summary>
        /// 获得指定元素所在段落的样式
        /// </summary>
        /// <param name="element">元素对象</param>
        /// <returns>段落样式对象</returns>
        public DomParagraphFlagElement GetParagraphEOFElement(DomElement element)
        {
            if (element == null)
            {
                return null;
            }
            if (element is DomParagraphFlagElement)
            {
                return (DomParagraphFlagElement)element;
            }
            int index = this.IndexOfUseContentIndex(element);
            if (index >= 0)
            {
                var pef = element._OwnerLine?.ParagraphFlagElement;
                if (pef != null
                    && this.SafeGet(pef._ContentIndex) == pef
                    && pef._ContentIndex > index)
                {
                    return pef;
                }
                return base.InnerGetFirstElement<DomParagraphFlagElement>(index);
            }
            return null;
        }

        /// <summary>
        /// 选择所有的元素
        /// </summary>
        public void SelectAll()
        {
            if (this.Count >= 1)
            {
                    this.SetSelection(this.Count - 1, 1 - this.Count);
                    this._SelectAllFlag = true;
            }
        }
        internal bool _SelectAllFlag = false;

        /// <summary>
        /// 移动位置
        /// </summary>
        /// <param name="target"></param>
        public void MoveToTarget(MoveTarget target)
        {
            switch (target)
            {
                case MoveTarget.None:
                    // 无操作
                    return;
                case MoveTarget.PageHome:
                    {
                        // 移动到页首
                        int currentIndex = this.SelectionStartIndex;
                        int targetIndex = 0;
                        PrintPage page = this.CurrentElement.OwnerLine.OwnerPage;
                        for (int iCount = currentIndex - 1; iCount >= 0; iCount--)
                        {
                            DomElement element = this[iCount];
                            if (element.OwnerLine.OwnerPage != page)
                            {
                                targetIndex = iCount + 1;
                                break;
                            }
                        }//for
                        if (targetIndex != currentIndex)
                        {
                            this.MoveToPosition(
                                this.InnerFixIndex(targetIndex, FixIndexDirection.Back, true));
                        }
                    }
                    break;
                case MoveTarget.PageEnd:
                    {
                        // 移动到页首
                        int currentIndex = this.SelectionStartIndex;
                        int targetIndex = this.Count - 1;
                        PrintPage page = this.CurrentElement.OwnerLine.OwnerPage;
                        for (int iCount = currentIndex + 1; iCount < this.Count; iCount++)
                        {
                            DomElement element = this[iCount];
                            if (element.OwnerLine.OwnerPage != page)
                            {
                                targetIndex = iCount - 1;
                                break;
                            }
                        }//for
                        if (targetIndex != currentIndex)
                        {
                            this.MoveToPosition(
                                this.InnerFixIndex(targetIndex, FixIndexDirection.Forward, true));
                        }
                    }
                    break;
                case MoveTarget.DocumentHome:
                    {
                        this.MoveToPosition(
                            this.InnerFixIndex(0, FixIndexDirection.Both, true));
                    }
                    break;
                case MoveTarget.CellHome:
                    {
                        DomTableCellElement cell = this.CurrentElement.OwnerCell;
                        if (cell != null)
                        {
                            int index = this.IndexOfUseContentIndex(cell.FirstContentElement);
                            this.MoveToPosition(this.InnerFixIndex(index, FixIndexDirection.Both, true));
                        }
                    }
                    break;
                case MoveTarget.ParagraphHome:
                    {
                        int index = 0;
                        for (int iCount = this.SelectionStartIndex - 1; iCount >= 0; iCount--)
                        {
                            if (this[iCount] is DomParagraphFlagElement)
                            {
                                index = iCount + 1;
                                break;
                            }
                        }
                        this.MoveToPosition(this.InnerFixIndex(index, FixIndexDirection.Both, true));
                    }
                    break;
                case MoveTarget.Home:
                    this.MoveHome();
                    break;
                case MoveTarget.End:
                    this.MoveEnd();
                    break;
                case MoveTarget.PreLine:
                    this.MoveUpOneLine();
                    break;
                case MoveTarget.NextLine:
                    this.MoveDownOneLine();
                    break;
                case MoveTarget.ParagraphEnd:
                    {
                        int index = this.SelectionStartIndex;
                        for (int iCount = this.SelectionStartIndex; iCount < this.Count; iCount++)
                        {
                            if (this[iCount] is DomParagraphFlagElement)
                            {
                                index = iCount;
                                break;
                            }
                        }
                        this.MoveToPosition(
                            this.InnerFixIndex(index, FixIndexDirection.Both, true));
                    }
                    break;
                case MoveTarget.CellEnd:
                    {
                        DomTableCellElement cell = this.CurrentElement.OwnerCell;
                        if (cell != null)
                        {
                            int index = this.IndexOfUseContentIndex(cell.LastContentElement);
                            this.MoveToPosition(
                                this.InnerFixIndex(index, FixIndexDirection.Both, true));
                        }
                    }
                    break;
                case MoveTarget.PreCell:
                    {
                        DomTableCellElement cell = this.CurrentElement.OwnerCell;
                        if (cell != null)
                        {
                            DomElementList cells = cell.OwnerTable.VisibleCells;
                            DomTableCellElement preCell = (DomTableCellElement)cells.GetPreElement(cell);
                            if (preCell != null)
                            {
                                preCell.Focus();
                            }
                        }
                    }
                    break;
                case MoveTarget.NextCell:
                    {
                        DomTableCellElement cell = this.CurrentElement.OwnerCell;
                        if (cell != null)
                        {
                            DomElementList cells = cell.OwnerTable.VisibleCells;
                            DomTableCellElement cell2 = (DomTableCellElement)cells.GetNextElement(cell);
                            if (cell2 != null)
                            {
                                cell2.Focus();
                            }
                        }
                    }
                    break;
                case MoveTarget.BeforeField:
                    {
                        DomFieldElementBase field = (DomFieldElementBase)GetCurrentContainer(
                            typeof(DomInputFieldElementBase));
                        if (field != null)
                        {
                            int index = this.IndexOfUseContentIndex(field.StartElement);
                            if (index < 0)
                            {
                                index = this.IndexOfUseContentIndex(field);
                            }
                            if (index >= 0)
                            {
                                this.MoveToPosition(
                                    this.InnerFixIndex(index, FixIndexDirection.Both, true));
                            }
                        }
                    }
                    break;
                case MoveTarget.AfterField:
                    {
                        DomFieldElementBase field = (DomFieldElementBase)GetCurrentContainer(
                            typeof(DomInputFieldElementBase));
                        if (field != null)
                        {
                            int index = this.IndexOfUseContentIndex(field.EndElement);
                            if (index < 0)
                            {
                                index = this.IndexOfUseContentIndex(field);
                            }
                            if (index >= 0)
                            {
                                this.MoveToPosition(
                                    this.InnerFixIndex(index + 1, FixIndexDirection.Both, true));
                            }
                        }
                    }
                    break;
                case MoveTarget.BeforeTable:
                    {
                        DomTableElement table = (DomTableElement)GetCurrentContainer(
                            typeof(DomTableElement));
                        if (table != null && table.FirstCell != null)
                        {
                            int index = this.IndexOfUseContentIndex(table.FirstCell.FirstContentElement);
                            if (index > 0)
                            {
                                this.MoveToPosition(
                                    this.InnerFixIndex(
                                        index - 1,
                                        FixIndexDirection.Both,
                                        true));
                            }
                        }
                    }
                    break;
                case MoveTarget.AfterTable:
                    {
                        DomTableElement table = (DomTableElement)GetCurrentContainer(
                            typeof(DomTableElement));
                        if (table != null && table.LastVisibleCell != null)
                        {
                            int index = this.IndexOfUseContentIndex(table.LastVisibleCell.LastContentElement);
                            if (index > 0)
                            {
                                this.MoveToPosition(
                                    this.InnerFixIndex(
                                        index + 1,
                                        FixIndexDirection.Both,
                                        true));
                            }
                        }
                    }
                    break;
                case MoveTarget.DocumentEnd:
                    {
                        int index = this.Count - 1;
                        index = this.FixElementIndex(index);
                        this.MoveToPosition(this.InnerFixIndex(index, FixIndexDirection.Both, true));
                    }
                    break;
                case MoveTarget.FieldHome:
                    {
                        DomFieldElementBase field = (DomFieldElementBase)GetCurrentContainer(typeof(DomInputFieldElementBase));
                        if (field != null)
                        {
                            int index = this.IndexOfUseContentIndex(field.StartElement);
                            if (index >= 0)
                            {
                                this.MoveToPosition(
                                    this.InnerFixIndex(index + 1, FixIndexDirection.Forward, true));
                            }
                        }
                    }
                    break;
                case MoveTarget.FieldEnd:
                    {
                        DomFieldElementBase field = (DomFieldElementBase)GetCurrentContainer(typeof(DomInputFieldElementBase));
                        if (field != null)
                        {
                            int index = this.IndexOfUseContentIndex(field.EndElement);
                            if (index >= 0)
                            {
                                this.MoveToPosition(
                                    this.InnerFixIndex(index, FixIndexDirection.Forward, true));
                            }
                        }
                    }
                    break;
            }
        }
     

        /// <summary>
        /// 获得当前光标所在的指定类型的容器元素对象
        /// </summary>
        /// <param name="containerType">元素类型</param>
        /// <returns>获得的元素对象</returns>
        public DomElement GetCurrentContainer(Type containerType)
        {
            if (containerType == null)
            {
                throw new ArgumentNullException("containerType");
            }
            DomContainerElement c = null;
            int index = -1;
            GetCurrentPositionInfo(out c, out index);
            while (c != null)
            {
                if (containerType.IsInstanceOfType(c))
                {
                    return c;
                }
                c = c.Parent;
            }
            return null;
        }

        /// <summary>
        /// 将插入点向上移动一行
        /// </summary>
        public void MoveUpOneLine()
        {
            ClearLineEndFlag();
            DomLine myLine = this.GetPreLine(true);
            if (myLine != null)
            {
                if (_LastXPos <= 0 )
                {
                    DomElement StartElement = this[this.FixElementIndex(this.SelectionStartIndex)];
                    _LastXPos = StartElement.GetAbsLeft();
                }
                DomElement curElement = null;
                {
                    foreach (DomElement myElement in myLine)
                    {
                        if (myElement.GetAbsLeft() >= _LastXPos)
                        {
                            curElement = myElement;
                            break;
                        }
                    }
                }
                if (curElement == null)
                {
                        curElement = myLine.LastElement;
                }
                if (curElement is DomParagraphListItemElement)
                {
                    curElement = myLine.GetNextElement(curElement);
                }
                int index = this.IndexOfUseContentIndex(curElement.FirstContentElementInPublicContent);
                if (index >= 0)
                {
                    index = InnerFixIndex(index, FixIndexDirection.Forward, true);
                    this.MoveToPosition(index);
                }
            }
        }

        /// <summary>
        /// 将插入点向下移动一行
        /// </summary>
        public void MoveDownOneLine()
        {
            ClearLineEndFlag();
            var thisCurrentLine = this.CurrentLine;
            if (thisCurrentLine == null || thisCurrentLine.Count == 0)
            {
                return;
            }
            DomLine myLine = this.GetNextLine();
            if (myLine != null)
            {
                if (_LastXPos <= 0)
                {
                    DomElement StartElement = this[this.FixElementIndex(this.SelectionStartIndex)];
                    _LastXPos = StartElement.GetAbsLeft();
                }
                DomElement curElement = null;
                {
                    foreach (DomElement element in myLine)
                    {
                        if (!(element is DomParagraphListItemElement)
                            && element.GetAbsLeft() >= _LastXPos)
                        {
                            curElement = element;
                            break;
                        }
                    }
                }
                FixIndexDirection fid = FixIndexDirection.Back;
                if (curElement == null)
                {
                    // 修改表单模式下的插入点修正方向 袁永福 2017-10-20 DCWRITER-1669号问题。
                    fid = FixIndexDirection.Forward;
                    {
                        curElement = myLine.LastElement;
                    }
                }
                int index = this.IndexOfUseContentIndex(curElement.FirstContentElementInPublicContent);
                if (curElement is DomParagraphFlagElement)
                {
                    index = this.IndexOfUseContentIndex(curElement);
                }
                if (index >= 0)
                {
                    var index2 = InnerFixIndex(index, fid, true);
                    if (index2 <= this.SelectionStartIndex && fid == FixIndexDirection.Forward)
                    {
                        // DCWRITER-1669，向前修正没导致有效移动，则向后修正
                        index2 = InnerFixIndex(index2 + 1, FixIndexDirection.Back, true);
                    }
                    else if (this.SafeGet(index2).OwnerLine == thisCurrentLine && fid == FixIndexDirection.Forward)
                    {
                        index2 = InnerFixIndex(index2 + 1, FixIndexDirection.Back, true);
                    }
                    var targetCell = myLine[0].OwnerCell;
                    this.MoveToPosition(index2);
                }
                //this.MoveSelectStart( myLine.LastElement );
            }
        }

        private int GetBorderIndex(bool forLeft)
        {
            int index1 = this.SelectionStartIndex;
            int index2 = this.SelectionStartIndex + this.SelectionLength;
            if (forLeft)
            {
                return Math.Min(index1, index2);
            }
            else
            {
                return Math.Max(index1, index2);
            }
        }
        /// <summary>
        /// 将插入点先左移动一个元素
        /// </summary>
        public void MoveLeft()
        {
            try
            {
                DCSelection._DisableSetLineEndFlagInCellOnce = true;
                if (this.LineEndFlag)
                {
                    this.LineEndFlag = false;
                    this.MoveToPosition(this.SelectionStartIndex);
                    this.LineEndFlag = false;
                    return;
                }
                ClearLineEndFlag();
                _LastXPos = -1;

                if (this.AutoClearSelection && this.SelectionLength != 0)
                {
                    // 当需要清除选择状态是，移动插入点到选择区域的第一个元素之前
                    int newIndex = GetBorderIndex(true);
                    if (newIndex < 0)
                    {
                        newIndex = 0;
                    }
                    newIndex = InnerFixIndex(newIndex, FixIndexDirection.Forward, true);
                    this.MoveToPosition(newIndex);
                }
                else
                {
                    if (DomLine.__NewLayoutMode)
                    {
                        DomElement element = this[this.SelectionStartIndex];
                        DomElement nextElement = GetPreLayoutElement(element);
                        if (nextElement != null)
                        {
                            int newIndex = this.IndexOfUseContentIndex(nextElement);
                            if (newIndex >= 0)
                            {
                                    newIndex = InnerFixIndex(newIndex, FixIndexDirection.Forward, true);
                                if (newIndex < this.Count - 1)
                                {
                                    this.MoveToPosition(newIndex);
                                }
                            }
                        }
                        return;
                    }

                    if (this.SelectionStartIndex > 0)
                    {
                        int index = this.SelectionStartIndex - 1;
                        index = InnerFixIndex(index, FixIndexDirection.Forward, true);
                        this.MoveToPosition(index);
                    }
                }
            }
            finally
            {
                DCSelection._DisableSetLineEndFlagInCellOnce = false;
            }
        }
        private int GetLayoutElementIndex(DomElement element)
        {
            DomElementList list = this.LayoutElements();
            int index = -1;
            if (list == this)
            {
                if (list.SafeGet(element.ContentIndex) == element)
                {
                    index = element.ContentIndex;
                }
                else
                {
                    index = list.IndexOf(element);
                }
            }
            else
            {
                if (list.SafeGet(element.LayoutIndex) == element)
                {
                    index = element.LayoutIndex;
                }
                else
                {
                    index = list.IndexOf(element);
                }
            }
            return index;
        }

        /// <summary>
        /// 获得在排版顺序列表中的上一个元素
        /// </summary>
        /// <param name="element">文档元素</param>
        /// <returns>上一个元素</returns>
        public DomElement GetPreLayoutElement(DomElement element)
        {
            if (element.OwnerLine != null
                )
            {
                var line = element.OwnerLine.LayoutElements;
                var result = line.GetPreElement(element);
                if (result != null)
                {
                    if (result is DomTableElement)
                    {
                    }
                    else
                    {
                        return result;
                    }
                }
            }
            DomElementList list = this.LayoutElements();
            int index = GetLayoutElementIndex(element);
            if (index > 0)
            {
                return list[index - 1];
            }
            else
            {
                return null;
            }
        }
        internal DomElement GetNextLayoutElement(DomElement element)
        {
            if (element.OwnerLine != null
                )
            {
                var line = element.OwnerLine.LayoutElements;
                var result = line.GetNextElement(element);
                if (result != null)
                {
                    return result;
                }
            }
            DomElementList list = this.LayoutElements();
            int index = GetLayoutElementIndex(element);
            if (index >= 0 && index < list.Count - 1)
            {
                return list[index + 1];
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// 将插入点向右移动一个元素
        /// </summary>
        public void MoveRight()
        {
            ClearLineEndFlag();
            _LastXPos = -1;
            if (this.AutoClearSelection && this.SelectionLength != 0)
            {
                // 当需要清除选择状态是，移动插入点到选择区域的最后一个元素之后
                int newIndex = GetBorderIndex(false);
                if (newIndex >= this.Count)
                {
                    newIndex = this.Count - 1;
                }
                {
                    newIndex = InnerFixIndex(newIndex, FixIndexDirection.Back, true);
                }
                this.MoveToPosition(newIndex);
            }
            else
            {
                if (DomLine.__NewLayoutMode)
                {
                    DomElement element = this[this.SelectionStartIndex];
                    DomElement nextElement = GetNextLayoutElement(element);
                    if (nextElement != null)
                    {
                        int newIndex = this.IndexOfUseContentIndex(nextElement);
                        if (newIndex >= 0)
                        {
                            newIndex = InnerFixIndex(newIndex, FixIndexDirection.Back, true);
                            this.MoveToPosition(newIndex);
                        }
                    }
                    return;
                }
                if (this.SelectionStartIndex < this.Count - 1)
                {
                    int index = this.SelectionStartIndex + 1;
                    index = InnerFixIndex(index, FixIndexDirection.Back, true);
                    this.MoveToPosition(index);
                }
            }
        }

        /// <summary>
        /// 将插入点移动到当前行的最后一个元素处
        /// </summary>
        public void MoveEnd()
        {
            try
            {
                DomLine myLine = this.CurrentLine;
                if (myLine != null && _LineEndFlag == false)
                {
                    _LastXPos = -1;
                    var les = myLine.LayoutElements;
                    this.CurrentElement = les.LastElement;
                    if (LayoutHelper.IsNewLine(les.LastElement))
                    {
                        int index = this.IndexOfUseContentIndex(les.LastElement);
                        if (index >= 0)
                        {
                            index = InnerFixIndex(index, FixIndexDirection.Forward, true);
                            this.MoveToPosition(index);
                        }
                    }
                    else
                    {
                        int index = this.IndexOfUseContentIndex(les.LastElement) + 1;
                        int newIndex = InnerFixIndex(index, FixIndexDirection.Forward, true);
                        if (index != newIndex)
                        {
                            this.MoveToPosition(newIndex);
                        }
                        else
                        {
                            this.MoveToPosition(index);
                            _LineEndFlag = true;
                        }
                        //myOwnerDocument.update
                    }
                }
            }
            catch { }
        }

        /// <summary>
        /// 移动插入点到当前行的行首
        /// </summary>
        public void MoveHome()
        {
            ClearLineEndFlag();
            DomLine myLine = this.CurrentLine;
            if (myLine != null)
            {
                _LastXPos = -1;
                int FirstIndex = this.IndexOfUseContentIndex(myLine.LayoutElements.FirstElement);
                int FirstNBlank = 0;
                foreach (DomElement myElement in myLine.LayoutElements)
                {
                    DomCharElement myChar = myElement as DomCharElement;
                    if (myChar == null || char.IsWhiteSpace(myChar.GetCharValue()) == false)
                    {
                        FirstNBlank = myLine.LayoutElements.IndexOf(myElement);
                        break;
                    }
                }
                if (FirstNBlank == 0 || this.Selection.StartIndex == (FirstIndex + FirstNBlank))
                {
                    FirstIndex = InnerFixIndex(FirstIndex, FixIndexDirection.Back, true);
                    this.MoveToPosition(FirstIndex);
                }
                else
                {
                    int index = FirstIndex + FirstNBlank;
                    index = InnerFixIndex(index, FixIndexDirection.Back, true);
                    this.MoveToPosition(index);
                }
            }
        }

        /// <summary>
        /// 移动当前插入点的位置
        /// </summary>
        /// <param name="index">插入点的新的位置</param>
        /// <returns>操作是否成功</returns>
        public bool MoveToPosition(int index, bool forceLineEndFlag = false)
        {
            this._LineEndFlag = forceLineEndFlag;

            index = FixElementIndex(index);
            index = this.InnerFixIndex(index, FixIndexDirection.Both, true);
            int NewLength = 0;
            if (this._AutoClearSelection == false)
            {
                NewLength = this.SelectionLength + (this.SelectionStartIndex - index);
            }
            this.SelectionStartElementAsCurrentElement = true;
            return SetSelection(index, NewLength);
        }

        /// <summary>
        /// 将插入点移动到指定元素前面
        /// </summary>
        /// <param name="element">指定的元素</param>
        /// <returns>操作是否成功</returns>
        public bool MoveToElement(DomElement element)
        {
            int index = this.IndexOfUseContentIndex(element);
            if (index >= 0)
            {
                return MoveToPosition(index);
            }
            return false;
        }

        /// <summary>
        /// 垂直方向移动插入点指定距离
        /// </summary>
        /// <param name="Step">移动距离</param>
        public void MoveStep(float Step)
        {
            ClearLineEndFlag();
            _LineEndFlag = false;
            DomElement element = this.CurrentElement;
            if (element != null)
            {
                this.MoveToPoint(element.GetAbsLeft(), element.GetAbsTop() + Step);
            }
        }

        private void ClearLineEndFlag()
        {
            if (this._LineEndFlag)
            {
                this._LineEndFlag = false;
            }
        }
        private System.WeakReference _MatchContentElement_GetLineAt = null;
        /// <summary>
        /// 获得文档中指定位置下的文档行对象
        /// </summary>
        /// <param name="x">指定位置的X坐标</param>
        /// <param name="y">指定位置的Y坐标</param>
        /// <param name="strict">是否进行严格判断</param>
        /// <returns>找到的文档行对象,若未找到则返回空引用</returns>
        public DomLine GetLineAt(float x, float y, bool strict)
        {
            _MatchContentElement_GetLineAt = null;
            var document = this.OwnerDocument;
            if (document == null)
            {
                return null;
            }
            // 查找指定位置下的容器元素对象
            DomContentElement contentElement = this.DocumentContentElement;
            // 首先处理表格对象
            DomElementList tables = document.TypedElements.GetElementsByType(
                this.DocumentContentElement,
                typeof(DomTableElement));
            if (tables != null && tables.Count > 0)
            {
                for (int iCount = tables.Count - 1; iCount >= 0; iCount--)
                {
                    DomTableElement table = (DomTableElement)tables[iCount];
                    DomTableCellElement cell = table.GetCellByAbsPosition(x, y);
                    if (cell != null)
                    {
                        contentElement = cell;
                        break;
                    }
                }
            }//if

            if (contentElement == this.DocumentContentElement)
            {
                // 没有找到单元格，进一步搜索。
                DomElementList ces = document.TypedElements.GetElementsByType(
                    this.DocumentContentElement,
                    typeof(DomContentElement));// this.DocumentContentElement.ContentElements;

                for (int iCount = ces.Count - 1; iCount >= 0; iCount--)
                {
                    DomContentElement ce = (DomContentElement)ces[iCount];
                    if (ce is DomTableCellElement)
                    {
                        // 所有的单元格都已经处理过了。
                        continue;
                    }
                    if (ce.GetAbsBounds().Contains(x, y))
                    {
                        contentElement = (DomContentElement)ce;
                        break;
                    }
                }
            }
            DomLine currentLine = contentElement.GetLineAt(x, y, strict);
            if( currentLine == null && (contentElement is DomDocumentContentElement) == false )
            {
                _MatchContentElement_GetLineAt = new WeakReference(contentElement);
            }
            if (currentLine != null)
            {
                if (currentLine.IsTableLine
                    && currentLine.LastElement is DomParagraphFlagElement)
                {
                    if (x > currentLine.AbsLeft + currentLine.Width)
                    {
                        return currentLine;
                    }
                }
            }
            if (currentLine != null && currentLine.IsTableLine)
            {
                // 若找到的当前行是一个表格行，则深入表格内部找到当前行
                DomTableElement table = currentLine.TableElement;
                RectangleF tableBounds = table.GetAbsBounds();
                DomTableRowElement currentRow = null;
                foreach (DomTableRowElement row in table.RuntimeRows)
                {
                    if (tableBounds.Top + row.Top + row.Height >= y)
                    {
                        currentRow = row;
                        break;
                    }
                }
                if (currentRow == null)
                {
                    // 确定最后一行为当前行
                    currentRow = (DomTableRowElement)table.RuntimeRows.LastElement;
                }
                if (currentRow == null || currentRow.Cells.Count == 0)
                {
                    return null;
                }
                DomTableCellElement currentCell = (DomTableCellElement)currentRow.Cells.LastElement;
                if (x <= tableBounds.Left)
                {
                    // 指定的位置在表格的左侧，则使用该表格的第一个单元格
                    currentCell = (DomTableCellElement)currentRow.Cells[0];
                }
                else if (x >= tableBounds.Right)
                {
                    // 指定的位置在表格的右侧，则使用该表格的最后一个单元格
                    currentCell = (DomTableCellElement)currentRow.Cells.LastElement;
                }
                if (currentCell.IsOverrided)
                {
                    currentCell = currentCell.OverrideCell;
                }
                // 确定出当前的单元格了,重新搜索当前行
                currentLine = currentCell.GetLineAt(x, y, strict);
                if (currentLine == null)
                {
                    currentLine = currentCell.PrivateLines.LastLine;
                }
            }//if
            return currentLine;
        }

        /// <summary>
        /// 获得文档中指定位置下的元素对象
        /// </summary>
        /// <param name="x">指定位置的X坐标</param>
        /// <param name="y">指定位置的Y坐标</param>
        /// <param name="strict">是否进行严格判断</param>
        /// <returns>找到的元素,若未找到则返回空引用</returns>
        public DomElement GetElementAt(float x, float y, bool strict)
        {
            if (this.OwnerDocument == null)
            {
                return null;
            }
            DomDocumentContentElement dce = this.DocumentContentElement;
            // 确定当前行
            DomLine currentLine = GetLineAt(x, y, strict);
            if( _MatchContentElement_GetLineAt != null && _MatchContentElement_GetLineAt.IsAlive )
            {
                var c3 = (DomElement)_MatchContentElement_GetLineAt.Target;
                _MatchContentElement_GetLineAt = null;
                return c3;
            }
            // 若没有找到当前行则函数处理失败，立即返回
            if (currentLine == null)
            {
                return null;
            }
            {
                // 修正X坐标值
                x = x - currentLine.AbsLeft;//- CurrentLine.Margin ;
                if (strict)
                {
                    // 进行严格判断
                    if (x >= 0 && x <= currentLine.Width)
                    {
                        foreach (DomElement element in currentLine.FastForEach())
                        {
                            if (element is DomParagraphListItemElement)
                            {
                                // 段落列表元素不能选择
                                continue;
                            }
                            if (x >= element.Left && x <= element.Left + element.Width + element.WidthFix)
                            {
                                return element;
                            }
                        }//foreach
                    }
                    return null;
                }
                else
                {
                    // 进行非严格判断
                    if (x < 0)
                    {
                        return currentLine.FirstElement;
                    }
                    foreach (DomElement myElement in currentLine.FastForEach())
                    {
                        if (myElement is DomParagraphListItemElement)
                        {
                            // 段落列表元素不能选择
                            continue;
                        }
                        if (x < myElement.Left + myElement.Width)
                        {
                            return myElement;
                        }
                    }
                    return currentLine.LastElement;
                }
            }
        }


        private float _LastXPos = -1;
        /// <summary>
        /// 将插入点尽量移动到指定位置
        /// </summary>
        /// <param name="x">指定位置的X坐标</param>
        /// <param name="y">指定位置的Y坐标</param>
        public void MoveToPoint(float x, float y)
        {
            var document = this.OwnerDocument;
            if (document == null)
            {
                return;
            }
            _LastXPos = -1;
            // 确定当前行,指定的Y坐标在当前行低边缘上面
            DomLine currentLine = GetLineAt(x, y, false);
            // 若最后还是没有找到当前行则函数处理失败，立即返回
            if (currentLine == null)
            {
                return;
            }
            bool bolFlag = false;
            int newPosition = 0;
            x = x - currentLine.AbsLeft;

            // 确定当前元素,当前元素是当前行中右边缘大于指定的Ｘ坐标的元素
            DomElement currentElement = null;
            {
                foreach (DomElement myElement in currentLine.LayoutElements)
                {
                    if (myElement is DomParagraphListItemElement)
                    {
                        // 段落列表元素不能选择
                        continue;
                    }
                    if (x < myElement.Left + myElement.Width)
                    {
                        if (myElement is DomFieldBorderElement)
                        {
                            // 对于输入域边界元素，沾到就进入到输入域的内部
                            var fb = (DomFieldBorderElement)myElement;
                            if (fb.Position == BorderElementPosition.Start)
                            {
                                if (x >= myElement.Left)
                                {
                                    currentElement = currentLine.GetNextElement(myElement);
                                }
                            }
                            else
                            {
                                currentElement = myElement;// currentLine.GetPreElement(myElement);
                            }
                            if (currentElement != null)
                            {
                                break;
                            }
                        }
                        float detectRight = myElement.Left + myElement.Width / 2;
                        if (x > detectRight)
                        {
                            continue;
                        }
                        currentElement = myElement;
                        break;
                    }
                }
            }
            if (currentElement == null)
            {
                DomElement element = currentLine.LayoutElements.FirstElement;
                if (x < element.Left)
                {
                    // 位置超出文档行的左边
                    if (element is DomParagraphFlagElement || element is DomLineBreakElement)
                    {
                        newPosition = this.IndexOfUseContentIndex(element);
                        bolFlag = false;
                    }
                    else
                    {
                        {
                            newPosition = this.IndexOfUseContentIndex(element);
                        }
                    }
                }
                else if (x > currentLine.LayoutElements.LastElement.Left)
                {
                    // 当前位置超出文档行的右边
                    element = currentLine.LayoutElements.LastElement;
                    if (element is DomParagraphFlagElement || element is DomLineBreakElement)
                    {
                        newPosition = this.IndexOfUseContentIndex(element);
                        bolFlag = false;
                    }
                    else
                    {
                        {
                            newPosition = this.IndexOfUseContentIndex(element) + 1;
                            bolFlag = true;
                        }
                    }
                }
                if (this.Count == 1)
                {
                    newPosition = 0;
                    bolFlag = false;
                }
                else
                {
                    DomContentElement ce = currentLine.OwnerContentElement;
                    if (ce.PrivateContent.Count == 1)//|| ce is XTextTableCellElement)
                    {
                        newPosition = this.IndexOfUseContentIndex(currentLine.LayoutElements.LastElement);
                        bolFlag = false;
                    }
                }
            }
            else
            {
                // 若找到当前元素则设置当前位置到当前元素
                newPosition = this.IndexOfUseContentIndex(currentElement);
                bolFlag = false;
            }
            if (newPosition < 0)
            {
                return;
            }
            // 修正当前元素序号
            if (newPosition > this.Count)
            {
                newPosition = this.Count - 1;
                bolFlag = false;
            }
            if (newPosition < 0)
            {
                newPosition = 0;
                bolFlag = false;
            }

            newPosition = FixElementIndex(newPosition);
            int newLength = 0;
            if (this.AutoClearSelection == false)
            {
                DomContentElement oldCe = this[this.Selection.NativeStartIndex].ContentElement;
                DomContentElement newCe = currentLine.OwnerContentElement;
                this.SelectionStartElementAsCurrentElement = false;
                newLength = newPosition - this.Selection.NativeStartIndex;
                newPosition = this.Selection.NativeStartIndex;
                if (oldCe != newCe && (oldCe is DomTableCellElement || newCe is DomTableCellElement))
                {
                    // 出现跨单元格选择操作
                    if (newLength > 0)
                    {
                        newLength++;
                    }
                }
            }
            int fixIndex = this.InnerFixIndex(newPosition, FixIndexDirection.Both, true);

            if (bolFlag && newLength == 0)
            {
                if (newPosition > 1)
                {
                    if (this[newPosition - 1] is DomParagraphFlagElement)
                    {
                        bolFlag = false;
                    }
                }
            }
            this.LineEndFlag = bolFlag;
            if (SetSelection(newPosition, newLength) == false)
            {
                this.OwnerDocument.EditorControl.UpdateTextCaretWithoutScroll();
            }
        }

        /// <summary>
        /// 根据表单视图状态修正插入点位置
        /// </summary>
        /// <param name="index">要修正的插入点位置编号</param>
        /// <param name="direction">修正方向</param>
        /// <param name="enableSetAutoClearSelectionFlag">是否允许设置AutoClearSelectionFlag标记</param>
        /// <returns>修正后的插入点位置</returns>
        public int InnerFixIndex(
            int index,
            FixIndexDirection direction,
            bool enableSetAutoClearSelectionFlag)
        {
            if (this.Count == 0)
            {
                return 0;
            }
            var document = this.OwnerDocument;
            if (document == null
                || document.EditorControl == null)
            {
                return index;
            }
            if (index < 0)
            {
                index = 0;
            }
            if (index > this.Count - 1)
            {
                index = this.Count - 1;
            }

            return index;
        }

        /// <summary>
        /// 设置选择区域大小
        /// </summary>
        /// <param name="newSelectionStart">新的选择区域开始的序号</param>
        /// <param name="newSelectionLength">新选择区域的长度</param>
        /// <returns>操作是否成功</returns>
        public bool SetSelection(int newSelectionStart, int newSelectionLength)
        {
            FixRange(
                ref newSelectionStart,
                ref newSelectionLength);
            return this.DocumentContentElement.SetSelection(
                newSelectionStart,
                newSelectionLength);
        }

        /// <summary>
        /// 选择插入点所在的段落
        /// </summary>
        /// <returns>操作是否成功</returns>
        public bool SelectParagraph()
        {
            if (this.Count == 0)
            {
                return false;
            }
            var document = this.OwnerDocument;
            if (document == null || document.DocumentControler == null)
            {
                return false;
            }
            DomElement element = this.CurrentElement;
            if (element != null)
            {
                int index = this.IndexOfUseContentIndex(element);
                int startIndex = 0;
                int endIndex = this.Count - 1;
                for (int start = index - 1; start >= 0; start--)
                {
                    if (this[start] is DomParagraphFlagElement)
                    {
                        startIndex = start + 1;
                        break;
                    }
                }
                for (int end = index; end < this.Count; end++)
                {
                    if (this[end] is DomParagraphFlagElement)
                    {
                        endIndex = end;
                        break;
                    }
                }
                this._LineEndFlag = false;
                this.SetSelection(startIndex, endIndex - startIndex + 1);
                this._AutoClearSelection = true;
                return true;
            }
            return false;
        }

        /// <summary>
        /// 选择插入点处的连续的字母和数字
        /// </summary>
        /// <returns>操作是否成功</returns>
        public bool SelectWord()
        {
            if (this.Count == 0)
            {
                return false;
            }
            DomContainerElement container = null;
            int start = -1;
            for (int iCount = Math.Min(this.SelectionStartIndex, this.Count - 1); iCount >= 0; iCount--)
            {
                if (container == null)
                {
                    container = this[iCount].Parent;
                }
                DomCharElement c = this[iCount] as DomCharElement;
                if (c == null)
                {
                    break;
                }
                if (c.Parent == container)
                {
                    if (char.IsLetter(c.GetCharValue()) || char.IsDigit(c.GetCharValue()))
                    {
                        start = iCount;
                    }
                    else
                    {
                        break;
                    }
                }
                else
                {
                    break;
                }
            }//for
            if (start >= 0)
            {
                int end = -1;
                for (int iCount = this.SelectionStartIndex; iCount < this.Count; iCount++)
                {
                    if (container == null)
                    {
                        container = this[iCount].Parent;
                    }
                    DomCharElement c = this[iCount] as DomCharElement;
                    if (c == null)
                    {
                        break;
                    }
                    if (c.Parent == container)
                    {
                        if (char.IsLetter(c.GetCharValue()) || char.IsDigit(c.GetCharValue()))
                        {
                            end = iCount;
                        }
                        else
                        {
                            break;
                        }
                    }
                    else
                    {
                        break;
                    }
                }//for
                if (end != -1)
                {
                    this._LineEndFlag = false;
                    this.SetSelection(start, end - start + 1);
                    return true;
                }
            }
            return false;
        }
        internal void InnerDispose()
        {
            this.Clear();
        }
    }
}