using System;
using System.Collections.Generic;
using System.Text;
using DCSoft.Common;

namespace DCSoft.Writer.Dom
{
    /// <summary>
    /// 文档高亮度显示区域管理器
    /// </summary>
    internal class HighlightManager 
    {
        /// <summary>
        /// 初始化对象
        /// </summary>
        public HighlightManager()
        {
        }
        public void Dispose()
        {
            this._Document = null;
            this.Clear();
            
        }
        private DomDocument _Document = null;
        /// <summary>
        /// 操作的文档对象
        /// </summary>
        public DomDocument Document
        {
            get
            {
                return _Document;
            }
            set
            {
                _Document = value;
            }
        }


        /// <summary>
        /// 鼠标悬停处的高亮度显示区域对象
        /// </summary>
        [NonSerialized]
        private HighlightInfo _HoverHighlightInfo = null;

        /// <summary>
        /// 鼠标悬停处的高亮度显示区域对象
        /// </summary>
        public HighlightInfo HoverHighlightInfo
        {
            get
            {
                return _HoverHighlightInfo; 
            }
            set
            {
                _HoverHighlightInfo = value; 
            }
        }

        /// <summary>
        /// 清空
        /// </summary>
        public void Clear()
        {
            Array.Clear(this._ParentElements , 0 , this._ParentElements.Length);
            this._BodyElement = null;
            this._HoverHighlightInfo = null;
            //this._InnerHighlightInfos = null;
            if( this._MyHighlightInfos != null )
            {
                foreach( var item in this._MyHighlightInfos.Values )
                {
                    item?.Clear();
                }
                this._MyHighlightInfos.Clear();
                this._MyHighlightInfos = null;
            }
            this._InvalidateHighlightInfoElements = null;
            //this._Maps = null;
        }

        /// <summary>
        /// 删除指定元素相关的高亮度区域信息
        /// </summary>
        /// <param name="element">文档元素对象</param>
        public virtual void Remove(DomElement element)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }
            if (this.Document.IsLoadingDocument)
            {
                return;
            }
            this._BodyElement = null;
            //lock (this)
            {
                if (this._MyHighlightInfos != null)
                {
                    bool allowInvalidate = this.Document.AllowInvalidateForUILayoutOrView();
                    DomElement p = element;
                    while (p != null)
                    {
                        if (allowInvalidate)
                        {
                            HighlightInfo info = null;
                            if (this._MyHighlightInfos.TryGetValue(p, out info))
                            {
                                if (allowInvalidate && info != null && info.Range != null)
                                {
                                    this._Document.InvalidateView(info.Range);
                                }
                                this._MyHighlightInfos.Remove(p);
                            }
                        }
                        else
                        {
                            this._MyHighlightInfos.Remove(p);
                        }
                        p = p.Parent;
                    }
                }
                if (_InvalidateHighlightInfoElements != null
                    && _InvalidateHighlightInfoElements.Count > 0)
                {
                    for (int iCount = _InvalidateHighlightInfoElements.Count - 1; iCount >= 0; iCount--)
                    {
                        DomElement e2 = _InvalidateHighlightInfoElements[iCount];
                        if (e2 == element || e2.IsParentOrSupParent(element))
                        {
                            _InvalidateHighlightInfoElements.RemoveAt(iCount);
                        }
                    }
                }
            }
        }

        private void InvalidateHighlightInfoDeeply( DomContainerElement rootElement )
        {
            rootElement.CheckChildElementStatsReady();
            if(rootElement._ChildElementsNotCharOrParagraphFlag != null )
            {
                foreach( var item in rootElement._ChildElementsNotCharOrParagraphFlag )
                {
                    if(item is DomContainerElement)
                    {
                        this._MyHighlightInfos.Remove(item);
                        InvalidateHighlightInfoDeeply((DomContainerElement)item);
                    }
                }
            }
            //var arr = rootElement.Elements.InnerGetArrayRaw();
            //if( rootElement.FastHasChildElement( XTextContainerElement.DCChildElementType.Container , true ) == false )
            //{
            //    return;
            //}
            ////foreach( var item in rootElement.Elements)
            //for(var iCount = rootElement.Elements.Count -1;iCount >=0;iCount --)
            //{
            //    var item = arr[iCount];
            //    if (item is XTextContainerElement)
            //    {
            //        this._MyHighlightInfos.Remove(item);
            //        InvalidateHighlightInfoDeeply((XTextContainerElement)item);
            //    }
            //}
        }
        /// <summary>
        /// 声明指定的元素相关的高亮度显示区域无效,需要重新设置
        /// </summary>
        /// <param name="element">文档元素对象</param>
        public void InvalidateHighlightInfo(DomElement element)
        {
            if (this.Document.IsLoadingDocument)
            {
                return;
            }

            if (element is DomCharElement)
            {
                // 字符元素不能设置为高亮度区域，因此不处理，提高效率
                return;
            }
            if (this._MyHighlightInfos != null)
            {
                this._BodyElement = null;
                if( element is DomContainerElement)
                {
                    InvalidateHighlightInfoDeeply((DomContainerElement)element);
                }
                DomElement p = element;
                //System.Diagnostics.Debug.WriteLine("------------------ " + Environment.TickCount.ToString() + "   " + element.ID);
                while (p != null)
                {
                    this._MyHighlightInfos.Remove(p);
                    //System.Diagnostics.Debug.WriteLine(p.ID + " : " + p.ToString());
                    p = p.Parent;
                }
                //foreach( var item in this._MyHighlightInfos.Keys)
                //{
                //    System.Diagnostics.Debug.Write(item.ID + "  " + item.ToString() + "  $  ");
                //}
                //System.Diagnostics.Debug.WriteLine("");
            }
             
        }
         
        [NonSerialized]
        private DomElementList _InvalidateHighlightInfoElements = new DomElementList();
        
        private Dictionary<DomElement, HighlightInfo> _MyHighlightInfos 
            = new Dictionary<DomElement, HighlightInfo>();
        
        private DomDocumentBodyElement _BodyElement = null;
        private readonly DomElement[] _ParentElements = new DomElement[20];
        private HighlightInfo GetInnerHighlightInfo2(DomElement element)
        {
            // 目前只支持容器元素和单、复选框有高亮度显示功能
            //if (element == null)
            //{
            //    throw new ArgumentNullException("element");
            //}
            if( this._BodyElement == null )
            {
                this._BodyElement = this.Document.Body;
            }
            DomElement startElement = null;
            if(element is DomCharElement )
            {
                // 大多数情况下是字符元素
                startElement = element.Parent;
                if( startElement == this._BodyElement )// is XTextDocumentContentElement)
                {
                    // 有不小的可能性是正文的直接子元素，不可能高亮度显示。
                    return null;
                }
                if (startElement is DomTableCellElement)
                {
                    startElement = startElement.Parent?.Parent;
                }
                if( startElement == null )
                {
                    return null;
                }
                //// startElement = XTextCheckBoxElementBase.FixForFlowlayoutCheckBox(element);
                //if ( element is XTextCheckBoxElementBase.CheckBoxLayoutCharElement)
                //{
                //    startElement = ((XTextCheckBoxElementBase.CheckboxLayoutContainerElement)element.Parent)?._CheckboxElement;
                //}
                //else
                //{
                //    startElement = element.Parent;
                //}
                //if( startElement == null )
                //{
                //    return null;
                //}
                //if (startElement == null || startElement is XTextCharElement)
                //{
                //    startElement = element.Parent;
                //    if (startElement == null)
                //    {
                //        return null;
                //    }
                //}
            }
            else if (element is DomContainerElement || element is DomCheckBoxElementBase)
            {
                startElement = element;
                if (startElement  == this._BodyElement )// is XTextDocumentContentElement)
                {
                    // 文档级内容元素，不可能高亮度显示。
                    return null;
                }
                if( element is DomTableCellElement)
                {
                    startElement = element.Parent?.Parent;
                    if( startElement == null )
                    {
                        return null;
                    }
                }
            }
            else
            {
                startElement = element.Parent;
                if( startElement is DomTableCellElement)
                {
                    startElement = startElement.Parent?.Parent;
                }
                if (startElement == null)
                {
                    return null;
                }
                if (startElement == this._BodyElement )// is XTextDocumentContentElement)
                {
                    // 文档级内容元素，不可能高亮度显示。
                    return null;
                }
            }
            HighlightInfo result = null;
            if( this._MyHighlightInfos == null )
            {
                this._MyHighlightInfos = new Dictionary<DomElement, HighlightInfo>();
            }
            if (this._MyHighlightInfos.TryGetValue(startElement, out result))
            {
                // 已有信息，则返回。
                //result.UpdateState();
                return result;
            }
            if (startElement is DomCheckBoxElementBase)
            {
                // 专门处理单复选框
                var infos = startElement.GetHighlightInfos();
                if (infos != null && infos.Count > 0)
                {
                    foreach (HighlightInfo info in infos)
                    {
                        if (info.OwnerElement != null)//|| _InnerHighlightInfos.ContainsOwnerElement(info.OwnerElement) == false)
                        {
                            if (info.Range != null && info.Range.CheckState(false))
                            {
                                this._MyHighlightInfos[info.OwnerElement] = info;
                                if (info.OwnerElement == startElement)
                                {
                                    result = info;
                                }
                            }
                        }
                    }//foreach
                }
                return result;
            }
            var parents = this._ParentElements;// new XTextElement[20];
            var p = startElement;
            var parentsCount = 0;
            while( p != null )
            {
                parents[parentsCount++] = p;
                p = p.Parent;
                if( p == this._BodyElement)
                {
                    break;
                }
                else if(p is DomDocumentContentElement || parentsCount > 15)
                {
                    break;
                }
            }
            //var parents = new List<XTextElement>(6);
            //var p = startElement;
            //while (p != null)
            //{
            //    parents.Add(p);
            //    p = p.Parent;
            //    if(p is XTextDocumentContentElement)
            //    {
            //        break;
            //    }
            //}
            for (int iCount = parentsCount - 1; iCount >= 0; iCount--)
            {
                var item = parents[iCount];
                if (item is DomInputFieldElement)
                {
                    // 特别针对输入域元素
                    var input = (DomInputFieldElement)item;
                    if (this._MyHighlightInfos.TryGetValue(input, out result) == false)
                    {
                        // 获得高亮度显示区域信息
                        var list = input.GetHighlightInfos();
                        if (list != null && list.Count > 0)
                        {
                            if (list.Count == 1)
                            {
                                if (list[0].Range.CheckState(false))
                                {
                                    result = list[0];
                                    //if(result != null )
                                    //{
                                        this._MyHighlightInfos[input] = result;
                                    //}
                                }
                            }
                        }//if
                    }
                    if (result != null)
                    {
                        for (int iCount2 = iCount - 1; iCount2 >= 0; iCount2--)
                        {
                            var p2 = parents[iCount2];
                            var list2 = p2.GetHighlightInfos();
                            var bolMatch = false;
                            if (list2 != null && list2.Count > 0)
                            {
                                foreach (var item3 in list2)
                                {
                                    if (item3.Range.CheckState(false) && item3.HeightClass)
                                    {
                                        this._MyHighlightInfos[p2] = item3;
                                        result = item3;
                                        bolMatch = true;
                                        break;
                                    }
                                }
                            }
                            if (bolMatch == false)
                            {
                                this._MyHighlightInfos[p2] = result;
                            }
                        }
                        break;
                    }
                }
            }
            this._MyHighlightInfos[startElement] = result;
            return result;
        }
         
        /// <summary>
        /// 获得指定元素所在的高亮度显示区域
        /// </summary>
        /// <param name="element">指定的文档元素对象</param>
        /// <returns>获得的高亮度显示区域</returns>
        public virtual HighlightInfo this[DomElement element]
        {
            get
            {
                if (element == null)
                {
                    return null;
                }
                //float tick = CountDown.GetTickCountFloat();
                // 首先搜索鼠标悬浮高亮度显示区域,该区域优先级最高而且最容易被命中
                if (this._HoverHighlightInfo != null && this._HoverHighlightInfo.Contains(element))
                {
                    return this._HoverHighlightInfo;
                }
                return this.GetInnerHighlightInfo2(element);
            }
        }
    }
}
