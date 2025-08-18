using System;
using System.Collections.Generic;
using System.Text;
using DCSoft.Writer.Undo;
using DCSoft.Drawing;

namespace DCSoft.Writer.Dom.Undo
{
    internal class XTextDocumentUndoGroup : XUndoGroup
    {
        public XTextDocumentUndoGroup(DomDocument doc)
        {
            if( doc == null )
            {
                throw new ArgumentNullException("doc");
            }
            this._Document = doc;
        }
        public override void Dispose()
        {
            base.Dispose();
            this._Document = null;
        }
        /// <summary>
        /// 本对象所属的文档对象
        /// </summary>
        protected DomDocument _Document = null;
        /// <summary>
        /// 本对象所属的文档对象
        /// </summary>
        public DomDocument Document
        {
            get { return _Document; }
            //set { myDocument = value; }
        }

        public override void Undo(XUndoEventArgs args)
        {
            Execute(args, true);
        }

        public override void Redo(XUndoEventArgs args)
        {
            Execute(args, false);
        }

        private void Execute(XUndoEventArgs args, bool undo)
        {
            try
            {
                if (this._Document.InnerViewControl != null)
                {
                    this._Document.InnerViewControl.InnerEnableEditElementValue = false ;
                }

                var undoRefreshElements = this._Document.UndoList.RefreshElements;
                undoRefreshElements.Clear();
                _Document.UndoList._ContentRefreshInfos.Clear();
                _Document.UndoList.ContentChangedContainer.Clear();
                _Document.UndoList.NeedInvokeMethods = UndoMethodTypes.None;
                //this._Document.EnsureCompressDom();
                if (undo)
                {
                    base.Undo(args);
                }
                else
                {
                    base.Redo(args);
                }
                if ((_Document.UndoList.NeedInvokeMethods & UndoMethodTypes.RefreshDocument)
                    == UndoMethodTypes.RefreshDocument)
                {
                    // 需要刷新整个文档。
                    if (this.Document.EditorControl != null)
                    {
                        this.Document.EditorControl.RefreshDocument();
                        return;
                    }
                }
                else
                {
                    if ((_Document.UndoList.NeedInvokeMethods & UndoMethodTypes.RefreshPages)
                        == UndoMethodTypes.RefreshPages)
                    {
                        if (this.Document.EditorControl != null)
                        {
                            this.Document.EditorControl.RefreshDocumentExt(false, false);
                        }
                    }
                    if ((_Document.UndoList.NeedInvokeMethods & UndoMethodTypes.RefreshLayout)
                        == UndoMethodTypes.RefreshLayout)
                    {
                        if (this.Document.EditorControl != null)
                        {
                            this.Document.EditorControl.RefreshDocumentExt(false, true);
                        }
                    }
                    if ((_Document.UndoList.NeedInvokeMethods & UndoMethodTypes.Invalidate)
                        == UndoMethodTypes.Invalidate)
                    {
                        if (this.Document.InnerViewControl != null)
                        {
                            this.Document.InnerViewControl.Invalidate();
                        }
                    }
                    if ((_Document.UndoList.NeedInvokeMethods & UndoMethodTypes.RefreshParagraphTree)
                        == UndoMethodTypes.RefreshParagraphTree)
                    {
                        DomDocumentContentElement dce2 = this.Document.Body;
                        if (undoRefreshElements != null
                            && undoRefreshElements.Count > 0)
                        {
                            dce2 = undoRefreshElements[0].DocumentContentElement;
                        }
                        if (dce2 == null)
                        {
                            if (this.Document.UndoList._ContentRefreshInfos != null)
                            {
                                foreach (DomElement e in this.Document.UndoList._ContentRefreshInfos.Keys)
                                {
                                    dce2 = e.DocumentContentElement;
                                    if (dce2 != null)
                                    {
                                        break;
                                    }
                                }
                            }
                        }
                        if (dce2 == null)
                        {
                            dce2 = this.Document.Body;
                        }
                        dce2.RefreshParagraphListState(false, true);
                    }
                }
                // 指定了要刷新的元素
                DomElementList RedrawElements = new DomElementList();

                Dictionary<DomContentElement, int> startIndexs
                    = new Dictionary<DomContentElement, int>();

                if (this.Document.UndoList._ContentRefreshInfos.Count > 0)
                {
                    // 直接获得要刷新排版的位置信息
                    DomDocumentContentElement dce2 = null;
                    foreach (DomContentElement ce in this.Document.UndoList._ContentRefreshInfos.Keys)
                    {
                        dce2 = ce.DocumentContentElement;
                        ce.UpdateContentElements(false);
                        startIndexs[ce] = this.Document.UndoList._ContentRefreshInfos[ce];
                    }
                    dce2.UpdateContentElements(false);
                }//if

                if (undoRefreshElements.Count > 0)
                {
                    // 获得要刷新排版的位置信息
                    foreach (DomElement element in undoRefreshElements)
                    {
                        DomContentElement ce = element.ContentElement;
                        if (ce == null)
                        {
                            continue;
                        }
                        if (startIndexs.ContainsKey(ce))
                        {
                            int index = ce.PrivateContent.IndexOf(element);
                            if (index >= 0 && index < startIndexs[ce])
                            {
                                for (int iCount = startIndexs[ce]; iCount >= index; iCount--)
                                {
                                    DomElement element2 = ce.PrivateContent[iCount];
                                    if (element2.OwnerLine != null)
                                    {
                                        // 声明文本行无效
                                        element2.OwnerLine.InvalidateState = true;
                                    }
                                }
                                startIndexs[ce] = index;
                            }
                        }
                        else
                        {
                            ce.UpdateContentElements(true);
                            element.FirstContentElement.EnsureInPrivateContent(true, true);
                            int index = ce.PrivateContent.IndexOf(element.FirstContentElementInPublicContent);
                            if (index < 0)
                            {
                                index = ce.PrivateContent.IndexOf(element);
                            }
                            if (element is DomParagraphFlagElement && index >= 0)
                            {
                                // 声明该段落中所有的文档行无效
                                int index2 = ce.PrivateContent.IndexOf(element);
                                bool endFlag = false;
                                if (index2 < 0)
                                {
                                    endFlag = true;
                                    index2 = ce.PrivateContent.Count - 1;
                                }

                                for (int iCount = index; iCount <= index2; iCount++)
                                {
                                    DomElement element2 = ce.PrivateContent[iCount];
                                    if (endFlag)
                                    {
                                        if (element2.HasSameParent(element))//.Parent == element.Parent)
                                        {
                                            if (element2.ElementIndex > element.ElementIndex)
                                            {
                                                break;
                                            }
                                        }
                                    }
                                    if (element2.OwnerLine != null)
                                    {
                                        element2.OwnerLine.InvalidateState = true;
                                    }
                                }
                            }
                            if (index >= 0)
                            {
                                startIndexs[ce] = index;
                            }
                        }
                    }//foreach
                    if (startIndexs.Count == 0)
                    {
                        // 没有找到任何命中的位置，通篇刷新
                        DomDocumentContentElement dce2 = null;
                        foreach (DomElement element in undoRefreshElements)
                        {
                            DomContentElement ce = element.ContentElement;
                            if (ce == null)
                            {
                                continue;
                            }
                            if (startIndexs.ContainsKey(ce) == false)
                            {
                                dce2 = ce.DocumentContentElement;
                                ce.UpdateContentElements(false);
                                startIndexs[ce] = 0;
                                foreach (DomLine line in ce.PrivateLines)
                                {
                                    line.InvalidateState = true;
                                }
                            }
                        }
                        if (dce2 != null)
                        {
                            dce2.UpdateContentElements(false);
                        }
                    }
                    using (DCGraphics g = this.Document.InnerCreateDCGraphics())
                    {
                        InnerDocumentPaintEventArgs args2 = this.Document.CreateInnerPaintEventArgs(g);
                        foreach (DomElement element in undoRefreshElements)
                        {
                            DomContentElement ce = element.ContentElement;
                            if (ce == null)
                            {
                                continue;
                            }
                            int index2 = ce.PrivateContent.IndexOf(element.FirstContentElementInPublicContent);
                            if (index2 >= 0)
                            {
                                //element.SizeInvalid = true;
                                if (element.SizeInvalid || element.ViewInvalid)
                                {
                                    RedrawElements.Add(element);
                                }
                                if (element.SizeInvalid)
                                {
                                    element.RefreshSize(args2);
                                    //this.Document.Render.RefreshSize(element, g);
                                }
                                //if (StartIndex == int.MinValue || StartIndex > index2)
                                //{
                                //    StartIndex = index2;
                                //}
                                //if (EndIndex == -1 || EndIndex < index2)
                                //{
                                //    EndIndex = index2;
                                //}
                            }//if
                        }//foreach
                    }//using

                }//if

                DomDocumentContentElement dce = this.Document.Body;
                foreach (DomContentElement ce in startIndexs.Keys)
                {
                    dce = ce.DocumentContentElement;
                    //ce.UpdateContentElements();
                    int index = startIndexs[ce];
                    if (index > 0)
                    {
                        //index--;
                    }
                    ce.RefreshPrivateContent(index, -1, true);
                }//foreach
                if (dce != null)
                {
                    dce.RefreshParagraphListState(false, true);
                }
                //XTextInputFieldElementBase.Clear_Cache_FixBorderElementWidth();
                if (this.Document.PageRefreshed == false)
                {
                    this.Document.RefreshPages();
                    if (this.Document.EditorControl != null)
                    {
                        this.Document.EditorControl.UpdatePages();
                        this.Document.EditorControl.GetInnerViewControl().Invalidate();
                    }
                }
                dce.Content.AutoClearSelection = true;
                dce.Content.LineEndFlag = false;
                if (undo)
                {
                    dce.SetSelection(intOldSelectionStart, 0);
                }
                else
                {
                    dce.SetSelection(intNewSelectionStart, 0);
                }
                //myDocument.Content.MoveSelectStart( intOldSelectionStart );
                foreach (DomElement element in RedrawElements)
                {
                    element.ViewInvalid = true;
                    element.InvalidateView();
                }
                if (this.Document.UndoList.ContentChangedContainer.Count > 0)
                {
                    // 触发文档内容修改事件
                    foreach (DomContainerElement container in
                        this.Document.UndoList.ContentChangedContainer)
                    {
                        // 触发文档事件
                        ContentChangedEventArgs args2 = new ContentChangedEventArgs();
                        args2.EventSource = ContentChangedEventSource.UndoRedo;
                        args2.Document = this.Document;
                        args2.Element = container;
                        container.RaiseBubbleOnContentChanged(args2);
                    }
                    this.Document.OnDocumentContentChanged();
                }
            }
            finally
            {
                if( this._Document.InnerViewControl != null )
                {
                    this._Document.InnerViewControl.InnerEnableEditElementValue = true;
                }
                //this.Document._CompressDom = null;
            }
        }

        private int intOldSelectionStart = 0;
        /// <summary>
        /// 开始登记操作时的文档选择区域开始序号
        /// </summary>
        public int OldSelectionStart
        {
            //get { return intOldSelectionStart; }
            set { intOldSelectionStart = value; }
        }
        private int intOldSelectionLength = 0;
        /// <summary>
        /// 开始登记操作时的文档选择区域长度
        /// </summary>
        public int OldSelectionLength
        {
            //get { return intOldSelectionLength; }
            set { intOldSelectionLength = value; }
        }

        private int intNewSelectionStart = 0;
        /// <summary>
        /// 结束登记操作时的文档选择区域开始序号
        /// </summary>
        public int NewSelectionStart
        {
            //get { return intNewSelectionStart; }
            set { intNewSelectionStart = value; }
        }
        private int intNewSelectionLength = 0;
        /// <summary>
        /// 结束登记操作时的文档选择区域长度
        /// </summary>
        public int NewSelectionLength
        {
           // get { return intNewSelectionLength; }
            set { intNewSelectionLength = value; }
        }
    }
}
