using System;
using System.Collections.Generic;
using System.Text;
using DCSoft.Writer.Dom;
using DCSoft.Writer;
using DCSoft.Writer.Controls;

namespace DCSoft.Writer.Dom.Undo
{
    internal class XTextUndoReplaceElements: XTextUndoBase
    {
  //      /// <summary>
		///// 初始化对象
		///// </summary>
		//public XTextUndoReplaceElements()
		//{
		//}

        /// <summary>
        /// 初始化对象
        /// </summary>
        /// <param name="container">容器元素对象</param>
        /// <param name="index">发生操作的序号</param>
        /// <param name="oldElements">旧元素列表</param>
        /// <param name="newElements">新元素列表</param>
        public XTextUndoReplaceElements(
            DomContainerElement container,
            int index,
            DomElementList oldElements,
            DomElementList newElements)
        {
            if (container == null)
            {
                throw new ArgumentNullException("container");
            }
            if (index == 0 && container.Elements.Count == 0)
            {
            }
            else
            {
                if (index < 0)
                {
                    throw new ArgumentOutOfRangeException("index<0");
                }
                int maxIndex = container.Elements.Count - 1;
                if (oldElements != null)
                {
                    maxIndex = maxIndex + oldElements.Count;
                }
                if (index > maxIndex)
                {
                    throw new ArgumentOutOfRangeException("index>" + maxIndex);
                }
            }
            this.myContainer = container;
            this.intIndex = index;
            if (oldElements != null && oldElements.Count > 0)
            {
                this._OldElements = new DomElementList();
                this._OldElements.AddRangeByDCList(oldElements);
            }
            if (newElements != null && newElements.Count > 0)
            {
                this._NewElements = new DomElementList();
                this._NewElements.AddRangeByDCList(newElements);
            }
        }
        public override void Dispose()
        {
            base.Dispose();
            this.myContainer = null;
            if (this._OldElements != null)
            {
                this._OldElements.Clear();
                this._OldElements = null;
            }
            if (this._NewElements != null)
            {
                this._NewElements.Clear();
                this._NewElements = null;
            }
        }
        private DomContainerElement myContainer = null;
        /// <summary>
        /// 容器元素对象
        /// </summary>
        //public XTextContainerElement Container
        //{
        //	get{ return myContainer ;}
        //}
        private readonly int intIndex = 0 ;
        /// <summary>
        /// 元素序号
        /// </summary>
        //public int Index
        //{
        //	get{ return intIndex ;}
        //}
        /// <summary>
        /// 旧元素列表
        /// </summary>
        private DomElementList _OldElements = null;
        /// <summary>
        /// 新元素列表
        /// </summary>
        private DomElementList _NewElements = null;
         
		/// <summary>
		/// 对象名称
		/// </summary>
		public override string Name
		{
			get
			{
                if (_OldElements != null && _OldElements.Count > 0)
                {
                    if (_OldElements.Count == 1)
                    {
                        return string.Format(
                            DCSR.DeleteElement_Content, 
                            _OldElements[0].ToString());
                    }
                    else
                    {
                        return string.Format(
                            DCSR.DeleteElements_Count,
                            _OldElements.Count);
                    }
                }
                else if (_NewElements != null && _NewElements.Count > 0)
                {
                    if (_NewElements.Count == 1)
                    {
                        return string.Format(
                            DCSR.InsertElement_Content,
                            _NewElements[0].ToString());
                    }
                    else
                    {
                        return string.Format(
                            DCSR.InsertElements_Count,
                            _NewElements.Count);
                    }
                }
                else
                {
                    return null;
                }

			}
			//set
			//{
			//}
		}

        private DomElement GetContentElement(DomElement element)
        {
            DomContentElement ce = myContainer.ContentElement;
            DomElement e = element;
            while (e != null)
            {
                // 可能存在元素不显示在文档中，例如处于图形模式下的XTextShapeInputFieldElement中，
                // 内容就不显示，在此向上查询显示出来的内容。
                if (ce.PrivateContent.Contains(e))
                {
                    return e;
                }
                e = e.Parent;
            }//while
            return element.FirstContentElement;
        }

        private void AddRefreshElements(DomElement startElement, DomElement endElement)
        {
            DomContentElement ce = myContainer.ContentElement;
            DomElementList res = this.OwnerList.RefreshElements ;
            if (res.Contains(startElement) == false)
            {
                res.Add(startElement);
            }
            DomLine startLine = null;
            while (startElement != null)
            {
                // 可能存在元素不显示在文档中，例如处于图形模式下的XTextShapeInputFieldElement中，
                // 内容就不显示，在此向上查询显示出来的内容。
                if (ce.PrivateContent.Contains(startElement))
                {
                    startLine = startElement.OwnerLine;
                    if (res.Contains(startElement) == false)
                    {
                        res.Add(startElement);
                    }
                    break;
                }
                startElement = startElement.Parent;
            }//while

            if (res.Contains(endElement) == false)
            {
                res.Add(endElement);
            }
            DomLine endLine = null;
            while (endElement != null)
            {
                if (ce.PrivateContent.Contains(endElement))
                {
                    endLine = endElement.OwnerLine;
                    if (res.Contains(endElement) == false)
                    {
                        res.Add(endElement);
                    }
                    break;
                }
                endElement = endElement.Parent;
            }//while

            ce.SetLinesInvalidateState(
                            startLine,
                            endLine );
        }

		/// <summary>
		/// 撤销操作
		/// </summary>
        public override void Undo( DCSoft.Writer.Undo.XUndoEventArgs args)
		{
            if (myContainer != null)
            {
                myContainer.ResetChildElementStats();
                //XTextDocument document = myContainer.OwnerDocument;
                DomContentElement ce = myContainer.ContentElement;
                args.Parameters["ContentElement"] = ce;
                if (_OldElements != null && _NewElements == null)
                {
                    // 光删除,撤销操作就是恢复数据
                    if (intIndex < 0 || intIndex > myContainer.Elements.Count)
                    {
                        // 序号不对
                        return;
                    }
                    myContainer.Elements.InsertRange(intIndex, _OldElements);
                    foreach (DomElement element in _OldElements)
                    {
                        DomElement e2 = GetContentElement(element);
                        if (e2 != null && e2.OwnerLine != null)
                        {
                            e2.OwnerLine.InvalidateState = true;
                        }
                        element.SetOwnerLine(null);
                        element.Parent = myContainer;
                    }
                    myContainer.UpdateContentVersion();
                    AddRefreshElements(
                        _OldElements.FirstContentElement, 
                        _OldElements.LastContentElement);
                }
                else if (_OldElements == null && _NewElements != null)
                {
                    // 光插入,撤销操作就是删除数据
                    int index = ce.PrivateContent.IndexOf(_NewElements.FirstContentElement);
                    int startIndex = index;
                    myContainer.Elements.RemoveRange(_NewElements);
                    foreach (DomElement element in _NewElements)
                    {
                        if (element is DomCharElement)
                        {
                            startIndex = Math.Min(startIndex, element.ContentIndex);
                            if (element.OwnerLine != null)
                            {
                                element.OwnerLine.InvalidateState = true;
                                element.SetOwnerLine(null);
                            }
                        }
                        else
                        {
                            if (element.FirstContentElementInPublicContent == null)
                            {
                                continue ;
                            }
                            startIndex = Math.Min(startIndex, element.FirstContentElementInPublicContent.ContentIndex);
                            if (myContainer.OwnerDocument.HighlightManager != null)
                            {
                                myContainer.OwnerDocument.HighlightManager.Remove(element);
                            }
                            DomElement fce = GetContentElement(element.FirstContentElementInPublicContent);
                            DomElement ece = GetContentElement(element.LastContentElementInPublicContent);

                            ce.SetLinesInvalidateState(
                                fce == null ? null : fce.OwnerLine,
                                ece == null ? null : ece.OwnerLine);

                            if (element.OwnerLine != null)
                            {
                                element.OwnerLine.InvalidateState = true;
                                element.SetOwnerLine(null);
                            }
                        }
                    }//foreach
                    myContainer.UpdateContentVersion();
                    this.OwnerList.AddContentRefreshInfo(  ce , startIndex);
                }
                else if( _OldElements != null && _NewElements != null )
                {
                    // 替换，撤销操作就是删除新数据并恢复旧数据
                    int index = ce.PrivateContent.IndexOf(_NewElements.FirstContentElement);
                    int startIndex = index;
                    myContainer.Elements.RemoveRange(_NewElements);
                    foreach (DomElement element in _NewElements)
                    {
                        if (element is DomCharElement)
                        {
                            startIndex = Math.Min(startIndex, element.ContentIndex);
                            if (element.OwnerLine != null)
                            {
                                element.OwnerLine.InvalidateState = true;
                                element.SetOwnerLine(null);
                            }
                        }
                        else
                        {
                            if (element.FirstContentElementInPublicContent == null)
                            {
                                continue;
                            }
                            startIndex = Math.Min(startIndex, element.FirstContentElementInPublicContent.ContentIndex);
                            if (myContainer.OwnerDocument.HighlightManager != null)
                            {
                                myContainer.OwnerDocument.HighlightManager.Remove(element);
                            }
                            DomElement fce = GetContentElement(element.FirstContentElementInPublicContent);
                            DomElement ece = GetContentElement(element.LastContentElementInPublicContent);
                            if (fce == ece)
                            {
                                if (fce.OwnerLine != null)
                                {
                                    fce.OwnerLine.InvalidateState = true;
                                }
                            }
                            else
                            {
                                ce.SetLinesInvalidateState(
                                    fce == null ? null : fce.OwnerLine,
                                    ece == null ? null : ece.OwnerLine);
                            }
                            element.SetOwnerLine(null);
                        }
                    }
                    this.OwnerList.AddContentRefreshInfo( ce , startIndex);
                    // 光删除,撤销操作就是恢复数据
                    if (intIndex < 0 || intIndex > myContainer.Elements.Count)
                    {
                        // 序号不对
                        return;
                    }
                    myContainer.Elements.InsertRange(intIndex, _OldElements);
                    foreach (DomElement element in _OldElements)
                    {
                        if (element.OwnerLine != null)
                        {
                            element.OwnerLine.InvalidateState = true;
                            element.SetOwnerLine(null);
                        }
                        element.Parent = myContainer;
                    }
                    myContainer.UpdateContentVersion();
                    this.OwnerList.RefreshElements.Add(
                        GetContentElement( 
                            _OldElements.FirstContentElement));
                    this.OwnerList.RefreshElements.Add(
                        GetContentElement( 
                            _OldElements.LastContentElement));
                }
                if (this.OwnerList.ContentChangedContainer.Contains(myContainer) == false)
                {
                    this.OwnerList.ContentChangedContainer.Add(myContainer);
                }
            }
		}

		/// <summary>
		/// 重复操作
		/// </summary>
        public override void Redo(DCSoft.Writer.Undo.XUndoEventArgs args)
		{
            if (myContainer != null)
            {
                myContainer.ResetChildElementStats();
                DomContentElement ce = myContainer.ContentElement;
                //args.Parameters["ContentElement"] = ce;
                if (_OldElements != null && _NewElements == null)
                {
                    // 光删除,重复操作就是重复删除数据
                    int index = ce.PrivateContent.IndexOf(_OldElements.FirstContentElement);
                    myContainer.Elements.RemoveRange(_OldElements);
                    foreach (DomElement element in _OldElements)
                    {
                        DomElement e2 = GetContentElement(element);
                        if (e2 != null)
                        {
                            if (e2.OwnerLine != null)
                            {
                                e2.OwnerLine.InvalidateState = true;
                            }
                        }
                        element.SetOwnerLine(null);
                        //myContainer.Elements.Remove(element);
                        if (myContainer.OwnerDocument.HighlightManager != null)
                        {
                            myContainer.OwnerDocument.HighlightManager.Remove(element);
                        }
                    }
                    myContainer.UpdateContentVersion();
                    this.OwnerList.AddContentRefreshInfo(ce, index);
                }
                else if (_OldElements == null && _NewElements != null)
                {
                    // 光插入,重复操作就是重复插入数据
                    // 光删除,撤销操作就是恢复数据
                    if (intIndex < 0 || intIndex > myContainer.Elements.Count)
                    {
                        // 序号不对
                        return;
                    }
                    myContainer.Elements.InsertRange(intIndex, _NewElements);
                    foreach (DomElement element in _NewElements)
                    {
                        DomElement e2 = GetContentElement(element);
                        if (e2 != null && e2.OwnerLine != null)
                        {
                            e2.OwnerLine.InvalidateState = true;
                        }
                        element.SetOwnerLine(null);
                    }//foreach
                    myContainer.UpdateContentVersion();
                    AddRefreshElements(
                        _NewElements.FirstContentElement,
                        _NewElements.LastContentElement);
                }
                else if (_OldElements != null && _NewElements != null)
                {
                    // 替换，撤销操作就是删除旧数据并插入新数据
                    int index = ce.PrivateContent.IndexOf(_OldElements.FirstContentElement);
                    myContainer.Elements.RemoveRange(_OldElements);
                    foreach (DomElement element in _OldElements)
                    {
                        DomElement e2 = GetContentElement(element);
                        if (e2 != null && e2.OwnerLine != null)
                        {
                            e2.OwnerLine.InvalidateState = true;
                        }
                        element.SetOwnerLine(null);
                        //myContainer.Elements.Remove(element);
                        if (myContainer.OwnerDocument.HighlightManager != null)
                        {
                            myContainer.OwnerDocument.HighlightManager.Remove(element);
                        }
                    }
                    this.OwnerList.AddContentRefreshInfo(ce, index);
                    // 光删除,撤销操作就是恢复数据
                    if (intIndex < 0 || intIndex > myContainer.Elements.Count)
                    {
                        // 序号不对
                        return;
                    }
                    myContainer.Elements.InsertRange(intIndex, _NewElements);
                    foreach (DomElement element in _NewElements)
                    {
                        if (element.OwnerLine != null)
                        {
                            element.OwnerLine.InvalidateState = true;
                            element.SetOwnerLine(null);
                        }
                    }
                    myContainer.UpdateContentVersion();
                    ce.UpdateContentElements(true);
                    AddRefreshElements(
                        _NewElements.FirstContentElement,
                        _NewElements.LastContentElement);
                }
                if (this.OwnerList.ContentChangedContainer.Contains(myContainer) == false)
                {
                    this.OwnerList.ContentChangedContainer.Add(myContainer);
                }
            }
		}
    }
}