using System;
using DCSoft.Printing;
using DCSoft.WinForms;
using DCSoft.Writer.Serialization;
using System.ComponentModel;
using DCSoft.Drawing;
using DCSoft.Common;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using DCSoft.Writer.Data;
using System.Text;
using System.Runtime.InteropServices;

using DCSoft.Writer.Controls;


namespace DCSoft.Writer.Dom
{
    public partial class DomDocument
    {
        /// <summary>
        /// 只发生一次的禁止触发文档元素获取焦点事件
        /// </summary>
        [NonSerialized]
        internal bool DisableFocusEventOnce = false;

        public void CheckForClearTextFormat(DomElementList sourceElements)
        {
            TypeProviders.TypeProvider_XTextDocument.CheckForClearTextFormat(this, sourceElements);
        }


        /// <summary>
        /// 修正文档元素中重复的ID号
        /// </summary>
        /// <param name="elements">文档元素列表</param>
        /// <returns>修改的ID的个数</returns>
       //[System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public int ValidateElementsIDRepeat(DomElementList elements)
        {
            if (elements == null || elements.Count == 0)
            {
                return 0;
            }
            if (this.GetDocumentBehaviorOptions().ValidateIDRepeatMode == DCValidateIDRepeatMode.None)
            {
                return 0;
            }
            int result = 0;
            DomTreeNodeEnumerable enumer = new DomTreeNodeEnumerable(elements);
            foreach (DomElement element in enumer)
            {
                if (string.IsNullOrEmpty(element.ID) == false)
                {
                    bool r = ValidateElementIDRepeat(element);
                    if (r)
                    {
                        result++;
                    }
                }
            }
            //WriterUtils.Enumerate(elements, delegate(object sender, ElementEnumerateEventArgs args)
            //    {
            //        if ((args.Element is XTextCharElement) == false)
            //        {
            //            if (string.IsNullOrEmpty(args.Element.ID) == false)
            //            {
            //                bool r = ValidateElementIDRepeat(args.Element);
            //                if (r)
            //                {
            //                    result++;
            //                }
            //            }
            //        }
            //    });
            return result;
        }

        private int _ValidateElementIDRepeatCount = 0;
        /// <summary>
        /// 对文档元素ID值进行重复性校验
        /// </summary>
        /// <param name="element">文档元素对象</param>
        /// <returns>校验是否通过</returns>
       //[System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public bool ValidateElementIDRepeat(DomElement element)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }
            if (element is DomCharElement)
            {
                return true;
            }
            DCValidateIDRepeatMode mode = this.GetDocumentBehaviorOptions().ValidateIDRepeatMode;
            if (mode == DCValidateIDRepeatMode.None)
            {
                return true;
            }
            string id = element.ID;
            if (string.IsNullOrEmpty(id))
            {
                // 空ID号无需校验 
                return true;
            }
            DomElement e = GetElementByIdExt(id, true);
            if (e != null && e != element)
            {
                if (mode == DCValidateIDRepeatMode.AutoFix)
                {
                    id = id + "_" + _ValidateElementIDRepeatCount;
                    _ValidateElementIDRepeatCount++;
                    element.ID = id;
                    return true;
                }
                else if (mode == DCValidateIDRepeatMode.ThrowException)
                {
                    string msg = string.Format(DCSR.IDRepeat_ID, id);
                    throw new InvalidOperationException(msg);
                }
                else if (mode == DCValidateIDRepeatMode.DetectOnly)
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 方法无效
        /// </summary>

        private new void EditorDelete(bool logUndo)
        {
            throw new NotSupportedException("EditorDelete");
        }

        #region 编辑器直接调用的方法 *******************************************************

        /// <summary>
        /// 设置文档默认样式
        /// </summary>
        /// <param name="newStyle">新默认样式</param>
        /// <param name="logUndo">是否记录撤销操作信息</param>
        //////[Browsable(false)]
        //[EditorBrowsable(EditorBrowsableState.Advanced)]
        //[System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public void EditorSetDefaultStyle(DocumentContentStyle newStyle, bool logUndo)
        {
            if (newStyle == null)
            {
                throw new ArgumentNullException("newStyle");
            }
            if (logUndo)
            {
                if (this.BeginLogUndo())
                {
                    this.AddUndo(new DCSoft.Writer.Dom.Undo.XTextUndoSetDocumentProperty(
                        this,
                        "DefaultStyle",
                        this.DefaultStyle.Clone(),
                        newStyle));
                    this.EndLogUndo();
                }
            }
            this.ContentStyles.Default = newStyle;
            //this.ContentStyles.RefreshRuntimeStyleList();
            this.Modified = true;
            if (this.EditorControl != null)
            {
                this.EditorControl.RefreshDocument();
            }
            //this.OnSelectionChanged();
        }

        /// <summary>
        /// 在编辑器中设置段落样式
        /// </summary>
        /// <param name="newStyleIndexs">新段落样式编号字典</param>
        /// <param name="logUndo">是否记录撤销信息</param>
        /// <returns>操作的段落元素对象列表</returns>
        internal DomElementList EditorSetParagraphStyle(
            Dictionary<DomElement, int> newStyleIndexs,
            bool logUndo)
        {
            if (newStyleIndexs == null)
            {
                throw new ArgumentNullException("newStyleIndexs");
            }

            //XTextElementList modifiedElements = new XTextElementList();
            //XTextContentElement ce = null;
            DCSoft.Writer.Dom.Undo.XTextUndoSetElementStyle undo = null;
            if (logUndo)
            {
                undo = new DCSoft.Writer.Dom.Undo.XTextUndoSetElementStyle();
                undo.Document = this;
                undo.ParagraphStyle = true;
            }
            this.ContentStyles.Styles.SetValueLocked(false);
            List<DomLine> invalidateLines = new List<DomLine>();
            Dictionary<DomContentElement, DomElementList> modifiedElements
                = new Dictionary<DomContentElement, DomElementList>();
            DomElementList result = new DomElementList();
            DomElementList containers = new DomElementList();
            foreach (DomParagraphFlagElement p in newStyleIndexs.Keys)
            {
                if (this.DocumentControler.CanModify(p) == false)
                {
                    // 不能修改属性
                    continue;
                }
                DomContentElement ce = p.ContentElement;
                if (undo != null)
                {
                    undo.AddInfo(p, p.StyleIndex, newStyleIndexs[p]);
                }
                // 旧的段落样式
                //RuntimeDocumentContentStyle oldStyle = p.RuntimeStyle;
                p.StyleIndex = newStyleIndexs[p];
                // 段落样式被修改了，不再是自动创建的了。
                p.AutoCreate = false;
                p.UpdateContentVersion();
                result.Add(p);
                // 新的段落样式
                //RuntimeDocumentContentStyle newStyle = p.RuntimeStyle;
                // 比较一些重要的段落样式属性，看看是否可以避免文档内容重新分行
                bool refreshLine = true;
                //if (oldStyle.LeftIndent == newStyle.LeftIndent
                //    && oldStyle.FirstLineIndent == newStyle.FirstLineIndent
                //    && oldStyle.BulletedList == newStyle.BulletedList
                //    && oldStyle.NumberedList == newStyle.NumberedList
                //    && oldStyle.Align == newStyle.Align )
                //{
                //    refreshLine = false;
                //}
                if (containers.Contains(p.Parent) == false)
                {
                    containers.Add(p.Parent);
                }
                if (modifiedElements.ContainsKey(ce) == false)
                {
                    modifiedElements[ce] = new DomElementList();
                }

                modifiedElements[ce].Add(p.FirstContentElement);
                modifiedElements[ce].Add(p);

                if (refreshLine)
                {
                    // 设置段落所在的文本行状态无效，需要放弃。
                    DomLine line = p.FirstContentElement.OwnerLine;
                    DomLine line2 = p.OwnerLine;
                    if (line != null && line2 != null)
                    {
                        int endIndex = ce.PrivateLines.IndexOf(line2);
                        for (int iCount = ce.PrivateLines.IndexOf(line);
                            iCount <= endIndex;
                            iCount++)
                        {
                            invalidateLines.Add(ce.PrivateLines[iCount]);
                            ce.PrivateLines[iCount].InvalidateState = true;
                        }//for
                    }
                }
                else
                {

                }
            }//foreach

            if (modifiedElements.Count > 0)
            {
                if (logUndo)
                {
                    if (this.BeginLogUndo())
                    {
                        this.AddUndo(undo);
                        this.EndLogUndo();
                    }
                }
                foreach (DomContainerElement c in containers)
                {
                    DomContainerElement c2 = c;
                    while (c2 != null)
                    {
                        c2.Modified = true;
                        c2 = c2.Parent;
                    }
                }
                //this.UpdateContentVersion();
                this.Modified = true;
                this.CurrentStyleInfo.RefreshParagraph(this);
                bool flag = false;

                DomDocumentContentElement dce = null;
                foreach (DomContentElement ce in modifiedElements.Keys)
                {
                    if (dce == null)
                    {
                        dce = ce.DocumentContentElement;
                        dce.RefreshParagraphListState(false, true);
                    }
                    //ce.RefreshParagraphListState();
                    //ce.RefreshParagraphState(null);
                    ce.RefreshContentByElements(modifiedElements[ce], true, false);
                    if (ce.DocumentContentElement == this.CurrentContentElement)
                    {
                        flag = true;
                    }

                    ContentChangedEventArgs args = new ContentChangedEventArgs();
                    args.Document = this;
                    args.Element = ce;
                    ce.RaiseBubbleOnContentChanged(args);
                }
                if (this.EditorControl != null)
                {
                    this.EditorControl.UpdateTextCaret();
                }
                if (flag)
                {
                    if (this.InnerViewControl != null)
                    {
                        this.InnerViewControl.Invalidate();
                    }
                    this.OnSelectionChanged();
                }
            }
            this.ContentStyles.Styles.SetValueLocked(true);
            return result;
        }

        /// <summary>
        /// 在编辑器中设置元素样式
        /// </summary>
        /// <param name="newStyleIndexs">新元素样式编号</param>
        /// <param name="logUndo">是否记录撤销信息</param>
        /// <param name="causeUpdateLayout">操作是否会导致重新排版</param>
        /// <param name="commandName">相关的命令对象</param>
        /// <returns>操作修改的元素列表</returns>
        //[System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        // 
        public DomElementList EditorSetElementStyle(
            Dictionary<DomElement, int> newStyleIndexs,
            bool logUndo,
            bool causeUpdateLayout,
            string commandName)
        {
            foreach (DomElement element in newStyleIndexs.Keys)
            {
                element.SetOwnerDocumentRaw(this);
                element.FixDomState();
            }
            //this.ContentStyles.ResetFastValue();
            Dictionary<DomContentElement, DomElementList> modifiedElements
                = new Dictionary<DomContentElement, DomElementList>();
            DCSoft.Writer.Dom.Undo.XTextUndoSetElementStyle undo = null;
            if (logUndo)
            {
                undo = new DCSoft.Writer.Dom.Undo.XTextUndoSetElementStyle();
                undo.Document = this;
                undo.ParagraphStyle = false;
                undo.CauseUpdateLayout = causeUpdateLayout;
                undo.CommandName = commandName;
            }
            this.ContentStyles.Styles.SetValueLocked(false);
            DomElementList result = new DomElementList();
            List<DomContainerElement> containers = new List<DomContainerElement>();
            List<DomTableElement> tables = new List<DomTableElement>();
            List<DomContentElement> paragraphInvalidateContents = new List<DomContentElement>();
            DomAccessFlags adf = DomAccessFlags.Normal;
            using (DCGraphics g = this.InnerCreateDCGraphics())
            {
                var ctl = this.DocumentControler;
                ctl.BeginCacheValue();
                try
                {
                    foreach (DomElement element in newStyleIndexs.Keys)
                    {
                        if (ctl.CanModify(element, adf) == false)
                        {
                            // 不能修改元素属性
                            continue;
                        }
                        var newStyleIndexValue = newStyleIndexs[element];
                        DomElement styleParentElement = null;
                        if (element is DomCharElement && ((DomCharElement)element).IsBackgroundText)
                        {
                            styleParentElement = element.Parent;
                        }
                        if (styleParentElement != null)
                        {
                            if (newStyleIndexs.ContainsKey(styleParentElement))
                            {
                                newStyleIndexValue = newStyleIndexs[styleParentElement];
                            }
                            else
                            {
                                newStyleIndexValue = styleParentElement._StyleIndex;
                            }
                        }
                        DomContentElement ce = element.ContentElement;
                        if (undo != null)
                        {
                            undo.AddInfo(element, element.StyleIndex, newStyleIndexValue);
                        }
                        if (element is DomParagraphFlagElement)
                        {
                            DomParagraphFlagElement p = (DomParagraphFlagElement)element;
                            p.AutoCreate = false;
                            p.UpdateContentVersion();
                            if (causeUpdateLayout && ce != null)
                            {
                                // 设置段落所在的文本行状态无效，需要放弃。
                                DomLine line = p.FirstContentElement.OwnerLine;
                                DomLine line2 = p.OwnerLine;
                                if (line != null && line2 != null)
                                {
                                    int endIndex = ce.PrivateLines.IndexOf(line2);
                                    int iCount = ce.PrivateLines.IndexOf(line);
                                    if (iCount >= 0)
                                    {
                                        for (;
                                            iCount <= endIndex;
                                            iCount++)
                                        {
                                            //invalidateLines.Add(ce.PrivateLines[iCount]);
                                            ce.PrivateLines[iCount].InvalidateState = true;
                                        }//for
                                    }
                                }
                            }
                            if (ce != null && paragraphInvalidateContents.Contains(ce) == false)
                            {
                                paragraphInvalidateContents.Add(ce);
                            }
                        }
                        // 设置新的样式编号
                        element.SetStyleIndexForce(newStyleIndexValue);// .StyleIndex = newStyleIndexs[element];
                        if (containers.Contains(element.Parent) == false)
                        {
                            DomContainerElement c = element.Parent;
                            containers.Add(c);
                            c.SetOwnerDocumentRaw(this);
                            c.FixDomState();
                        }
                        // 触发元素样式发生改变事件
                        element.OnStyleChanged();
                        element.UpdateContentVersion();
                        if (causeUpdateLayout && ce != null)
                        {
                            if (element is DomContentElement)
                            {
                                DomContentElement ce2 = (DomContentElement)element;
                                ce2.UpdateLinePosition(ce.ContentVertialAlign, false, false);
                            }
                            element.SizeInvalid = true;
                        }
                        result.Add(element);
                        if (ce != null)
                        {
                            if (ce.PrivateContent.Contains(element.FirstContentElement))
                            {
                                if (modifiedElements.ContainsKey(ce))
                                {
                                    modifiedElements[ce].Add(element.FirstContentElement);
                                }
                                else
                                {
                                    DomElementList list = new DomElementList();
                                    list.Add(element.FirstContentElement);
                                    modifiedElements[ce] = list;
                                }
                            }
                        }
                        if (causeUpdateLayout)
                        {
                            InnerDocumentPaintEventArgs dpea = this.CreateInnerPaintEventArgs(g);
                            dpea.Element = element;
                            element.RefreshSize(dpea);
                            //this.Render.RefreshSize(element, g);
                            //if (element is XTextInputFieldElementBase)
                            //{
                            //    XTextInputFieldElementBase field = (XTextInputFieldElementBase)element;

                            //}
                        }
                        if (element is DomContentElement)
                        {
                            if (element is DomTableCellElement)
                            {
                                DomTableElement table = ((DomTableCellElement)element).OwnerTable;
                                if (tables.Contains(table) == false)
                                {
                                    tables.Add(table);
                                }
                            }
                            else
                            {
                                // 元素是套签的容器元素
                                if (causeUpdateLayout)
                                {
                                    DomContentElement ce2 = (DomContentElement)element;
                                    ce2.ParticalRefreshLines(new ParticalRefreshLinesEventArgs(null, null, ce2.ContentVertialAlign));
                                }
                            }
                        }
                        if (causeUpdateLayout)
                        {
                            if (element is DomObjectElement)
                            {
                                ((DomObjectElement)element).OnResize();
                            }
                        }
                    }//foreach
                }
                finally
                {
                    ctl.EndCacheValue();
                }
            }//using
            if (tables.Count > 0)
            {
                // 操作修改了表格的布局，对表格进行重新布局
                foreach (DomTableElement table in tables)
                {
                    table.InvalidateView();
                    if (causeUpdateLayout)
                    {
                        table.InnerExecuteLayout();
                        table.InvalidateView();
                    }
                    DomElementList list2 = new DomElementList();
                    list2.Add(table);
                    modifiedElements[table.ContentElement] = list2;
                }
            }
            if (paragraphInvalidateContents.Count > 0)
            {
                DomDocumentContentElement dce = paragraphInvalidateContents[0].DocumentContentElement;
                dce.RefreshParagraphListState(false, true);
                //foreach (XTextContentElement ce in paragraphInvalidateContents)
                //{
                //    ce.RefreshParagraphListState();
                //}
            }
            if (modifiedElements.Count > 0)
            {
                if (logUndo)
                {
                    if (this.CanLogUndo)
                    {
                        this.AddUndo(undo);
                    }
                    else
                    {
                        if (this.BeginLogUndo())
                        {
                            this.AddUndo(undo);
                            this.EndLogUndo();
                        }
                    }
                }
                //this.UpdateContentVersion();
                this.Modified = true;
                bool refreshPage = false;
                if (causeUpdateLayout)
                {
                    foreach (DomContentElement ce in modifiedElements.Keys)
                    {
                        if (ce.BelongToDocumentDom(ce.OwnerDocument))
                        {
                            ce.RefreshContentByElements(modifiedElements[ce], true, true);
                            if (ce.NeedRefreshPage)
                            {
                                refreshPage = true;
                            }
                        }
                    }//foreach
                }
                if (containers.Count > 0)
                {
                    foreach (DomContainerElement c in containers)
                    {
                        // 触发容器元素内容发生改变事件
                        if (c.BelongToDocumentDom(c.OwnerDocument))
                        {
                            DomContainerElement c2 = c;
                            while (c2 != null)
                            {
                                c2.Modified = true;
                                c2 = c2.Parent;
                            }
                            ContentChangedEventArgs cde = new ContentChangedEventArgs();
                            cde.Document = this;
                            cde.Element = c;
                            cde.OnlyStyleChanged = true;
                            c.RaiseBubbleOnContentChanged(cde);
                        }
                    }
                }
                this.OnDocumentContentChanged();
                if (refreshPage)
                {
                    // 需要刷新分页
                    this.PageRefreshed = false;
                    this.RefreshPages();
                    if (this.EditorControl != null)
                    {
                        this.EditorControl.UpdatePages();
                        this.EditorControl.UpdateTextCaret();
                        this.InnerViewControl.Invalidate();
                    }
                }
                else
                {
                    if (this.EditorControl != null)
                    {
                        this.EditorControl.UpdateTextCaret();
                        this.InnerViewControl.Invalidate();
                    }
                }
                this.OnSelectionChanged();
            }
            this.ContentStyles.Styles.SetValueLocked(true);
            return result;
        }



        #endregion

    }
}
