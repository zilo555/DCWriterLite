using System;
using System.Collections.Generic;
using System.Text;
using DCSoft.Writer ;
using DCSoft.Writer.Commands ;
using DCSoft.Writer.Dom;
using DCSoft.Writer.Controls ;
//using System.Windows.Forms;
 using System.Windows.Forms;
using System.Reflection.Metadata;
using System.Reflection;

namespace DCSoft.Writer.Commands
{
    /// <summary>
    /// 文档内容查找和替换操作器
    /// </summary>
    /// <remarks>编制 袁永福</remarks>
    public class ContentSearchReplacer //: DCSoft.Writer.Commands.IContentSearchReplacer
    {
        /// <summary>
        /// 初始化对象
        /// </summary>
        public ContentSearchReplacer( DomDocument document )
        {
            if(document == null )
            {
                throw new ArgumentNullException("document");
            }
            this.Document = document;
        }

        private bool _ShowUI = true;
        /// <summary>
        /// 是否允许显示用户界面
        /// </summary>
        public bool ShowUI
        {
            get
            {
                return _ShowUI; 
            }
            set
            {
                _ShowUI = value; 
            }
        }

        private DomDocument _Document = null;
        /// <summary>
        /// 文档对象
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
                if (_Document != null)
                {
                    _Content = _Document.Content;
                }
            }
        }

        private DCContent _Content = null;
        /// <summary>
        /// 文档内容对象
        /// </summary>
        public DCContent Content
        {
            get
            {
                return _Content; 
            }
            set
            {
                _Content = value; 
            }
        }

        /// <summary>
        /// 全局替换
        /// </summary>
        /// <param name="args">参数</param>
        /// <returns>替换的次数</returns>
        public int ReplaceAll(SearchReplaceCommandArgs args)
        {
            if (args == null)
            {
                throw new ArgumentNullException("args");
            }
            if ( args.SearchID )
            {
                // 处于查找编号的模式，本函数无效。
                return 0;
            }
            int result = 0;
            //List<int> indexs = new List<int>();
            SearchReplaceCommandArgs args2 = args.Clone();
            //args2.Backward = false ;
            //args2.LogUndo = false;
            //args2.Backward = true ;
            //args2.ExcludeBackgroundText = true;
            this.Document.BeginLogUndo();
            //XTextDocumentContentElement dce = this.Document.CurrentContentElement;
            //dce.SetSelection(dce.Content.Count - 1, 0);
            while (true)
            {
                int result2 = this.Replace(args2);
                if (result2 >= 0)
                {
                    result++;
                }
                else
                {
                    break;
                }
            }
            this.Document.EndLogUndo();
            return result;
    
            //int currentIndex = this.Content.Count - 1 ;
            //this.Document.BeginLogUndo();
            //DocumentControler controler = this.Document.DocumentControler;
            //Dictionary<XTextContentElement, int> startIndexs = new Dictionary<XTextContentElement, int>();
            //WriterControl ctl = this.Document.EditorControl ;
            //try
            //{
            //    if (ctl != null)
            //    {
            //        //ctl.BeginUpdate();
            //    }
            //    while (true)
            //    {
            //        int index = Search(args2, true, currentIndex);
            //        if (index >= 0)
            //        {
            //            XTextSelection mySelection = new XTextSelection(this.Document.CurrentContentElement);
            //            mySelection.Refresh(index, args.SearchString.Length);
            //            XTextContainerElement container = null;
            //            int elementIndex = 0;
            //            this.Content.GetPositonInfo(index, out container, out elementIndex, false);
            //            XTextContentElement contentElement = container.ContentElement;
            //            int pi = contentElement.PrivateContent.IndexOf(this.Content[index]);
            //            if (startIndexs.ContainsKey(contentElement))
            //            {
            //                startIndexs[contentElement] = Math.Min(startIndexs[contentElement], pi);
            //            }
            //            else
            //            {
            //                startIndexs[contentElement] = pi;
            //            }
            //            indexs.Add(index);
            //            if (string.IsNullOrEmpty(args.ReplaceString))
            //            {
            //                this.Content.DeleteSelection(true, false, true, mySelection);
            //            }
            //            else
            //            {
            //                XTextElementList newElements = this.Document.CreateTextElements(
            //                    args.ReplaceString,
            //                    (DocumentContentStyle)this.Document.CurrentStyleInfo.Paragraph,
            //                    (DocumentContentStyle)this.Document.CurrentStyleInfo.ContentStyleForNewString.Clone());
            //                ReplaceElementsArgs args3 = new ReplaceElementsArgs(
            //                    container,
            //                    index,
            //                    args.SearchString.Length,
            //                    newElements,
            //                    true,
            //                    false,
            //                    true);
            //                int repResult = this.Document.ReplaceElements(args3);
            //            }
            //            result++;
            //        }
            //        else
            //        {
            //            break;
            //        }
            //        currentIndex = index;// +args2.SearchString.Length;
            //    }//while
            //}
            //finally
            //{
            //    if (ctl != null)
            //    {
            //        //ctl.EndUpdate();
            //    }
            //}
            //this.Document.EndLogUndo();
            //if (startIndexs.Count > 0)
            //{
            //    bool refreshPage = false;
            //    foreach (XTextContentElement ce in startIndexs.Keys)
            //    {
            //        ce.UpdateContentElements(true);
            //        ce.UpdateContentVersion();
            //        ce._NeedRefreshPage = false;
            //        ce.RefreshPrivateContent(startIndexs[ce]);
            //        if (ce._NeedRefreshPage)
            //        {
            //            refreshPage = true;
            //        }
            //    }
            //    if (refreshPage)
            //    {
            //        this.Document.RefreshPages();
            //        if (this.Document.EditorControl != null)
            //        {
            //            this.Document.EditorControl.UpdatePages();
            //            this.Document.EditorControl.UpdateTextCaret();
            //            this.Document.EditorControl.Invalidate();
            //        }
            //    }
            //}
            //return startIndexs.Count;
        }

        /// <summary>
        /// 替换
        /// </summary>
        /// <param name="args">参数</param>
        /// <returns>操作的元素在文档中的序号,-1表示没找打，-2表示找到了但内容只读没法替换</returns>
        public int Replace(SearchReplaceCommandArgs args)
        {
            if (args == null)
            {
                throw new ArgumentNullException("args");
            }
            if (args.SearchID)
            {
                // 处于查找编号的模式，本函数无效。
                return 0;
            }
            bool insertModeBack = false;
            bool firstSearch = true;
            int index = -1;
        ReplaceAgain: ;
            index = Search(args, true, index );
            int back = this.Document.LastSearchStartPosition;
            if (index >= 0)
            {
                if (firstSearch)
                {
                    firstSearch = false;
                    if (args.LogUndo )
                    {
                        // 找到内容了，执行替换操作
                        this.Document.BeginLogUndo();
                    }
                    if (this.Document.EditorControl != null)
                    {
                        insertModeBack = this.Document.EditorControl.InsertMode;
                        this.Document.EditorControl.InsertMode = false;
                    }
                }
                int selectionStart = this.Content.Selection.AbsStartIndex;
                if (string.IsNullOrEmpty(args.ReplaceString))
                {
                    if (this.Document.DocumentControler.Delete( true ) == false)
                    {
                        // 删除失败，继续查找下一个区域
                        if (args.Backward)
                        {
                            index = index + args.SearchString.Length + 1;
                        }
                        else
                        {
                            index = index - args.SearchString.Length - 1;
                        }
                        goto ReplaceAgain;
                        //index = -2;
                    }
                }
                else
                {
                    //wyc20240817:处理输入域背景文字查找替换代码DUWRITER5_0-2039
                    bool IsBackgroundTextInsideSameInput = true;
                    DomInputFieldElement parentInput = null;
                    if (args.ExcludeBackgroundText == false)
                    {
                        foreach (DomElement element in this.Content.Selection)
                        {
                            if (element != null && element.Parent is DomInputFieldElement)
                            {
                                if (parentInput == null)
                                {
                                    parentInput = element.Parent as DomInputFieldElement;
                                    if (parentInput.Elements.Count > 0)
                                    {
                                        IsBackgroundTextInsideSameInput = false;
                                        break;
                                    }
                                }
                                else
                                {
                                    if (element.Parent != parentInput)
                                    {
                                        IsBackgroundTextInsideSameInput = false;
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                IsBackgroundTextInsideSameInput = false;
                                break;
                            }
                        }
                    }
                    if (args.ExcludeBackgroundText == false &&
                        IsBackgroundTextInsideSameInput == true &&
                        parentInput != null)
                    {
                        //System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(args.SearchString);
                        //parentInput.BackgroundText = regex.Replace(parentInput.BackgroundText, args.ReplaceString, 1);
                        int index2 = parentInput.BackgroundText.IndexOf(args.ReplaceString);
                        if (index2 >= 0)
                        {
                            parentInput.BackgroundText = parentInput.BackgroundText.Substring(0, index2) + args.ReplaceString + parentInput.BackgroundText.Substring(index2 + args.ReplaceString.Length);
                        }
                        parentInput.EditorRefreshView();
                        if (args.Backward)
                        {
                            index = index + args.SearchString.Length + 1;
                        }
                        else
                        {
                            index = index - args.SearchString.Length - 1;
                        }
                    }
                    else
                    ///////////////////////////////////////////////////////////////////////
                    {
                        DomElementList list = this.Document.DocumentControler.InsertString(
                        args.ReplaceString,
                        args.LogUndo,
                        InputValueSource.Unknow,
                        null,
                        null);
                        if (list == null || list.Count == 0)
                        {
                            // 删除失败，继续查找下一个区域
                            if (args.Backward)
                            {
                                index = index + args.SearchString.Length + 1;
                            }
                            else
                            {
                                index = index - args.SearchString.Length - 1;
                            }
                            goto ReplaceAgain;
                            //index = -2;
                        }
                    }
                }
                if (index >= 0)
                {
                    args.MatchedIndexs.Add(selectionStart);
                    this.Content.SetSelection(selectionStart, args.ReplaceString.Length);
                }
                if (this.Document.EditorControl != null)
                {
                    this.Document.EditorControl.InsertMode = insertModeBack;
                }
                if (args.LogUndo)
                {
                    this.Document.EndLogUndo();
                }
            }
            else
            {
                index = -1;
            }
            this.Document.LastSearchStartPosition = back;
            return index;
        }


        /// <summary>
        /// 执行查找
        /// </summary>
        /// <param name="args">参数</param>
        /// <returns>找到的元素在文档中的序号</returns>
        public int SearchAllText(SearchReplaceCommandArgs args )
        {
            if (args == null)
            {
                throw new ArgumentNullException("args");
            }
            if (string.IsNullOrEmpty(args.SearchString))
            {
                return -1;
            }
            args.MatchedIndexs.Clear();
            DCContent content = this.Content;
            int searchStringLength = args.SearchString.Length;
            int newStartIndex = -1;
            int contentLength = content.Count;
            for (int iCount = 0; iCount < contentLength; iCount++)
            {
                DomElement element = content[iCount];
                if (element is DomCharElement && (args.SpecifyContainer == null || element.IsParentOrSupParent(args.SpecifyContainer) == true))
                {
                    if (args.ExcludeBackgroundText)
                    {
                        if (element.Parent is DomFieldElementBase)
                        {
                            // 遇到背景文字，快速跳转
                            DomFieldElementBase field = (DomFieldElementBase)element.Parent;
                            if (field.IsBackgroundTextElement(element))
                            {
                                iCount = content.IndexOf(field.EndElement);
                                continue;
                            }
                        }
                    }
                    char c = ((DomCharElement)element).GetCharValue();
                    if (EqualsChar(c, args.SearchString[0], args.IgnoreCase))
                    {
                        if (searchStringLength == 1)
                        {
                            newStartIndex = iCount;
                            args.MatchedIndexs.Add(iCount);
                            continue;
                        }
                        else if (contentLength - iCount >= searchStringLength)
                        {
                            for (int iCount2 = 1; iCount2 < searchStringLength; iCount2++)
                            {
                                DomElement element2 = content[iCount + iCount2];
                                if (element2 is DomCharElement)
                                {
                                    if (EqualsChar(
                                        ((DomCharElement)element2).GetCharValue(),
                                        args.SearchString[iCount2],
                                        args.IgnoreCase))
                                    {
                                        if (iCount2 == searchStringLength - 1)
                                        {
                                            newStartIndex = iCount;
                                            args.MatchedIndexs.Add(iCount);
                                            break;
                                        }
                                    }
                                    else
                                    {
                                        // 判断失败
                                        break;
                                    }
                                }
                                else
                                {
                                    // 不是字符元素，判断失败
                                    break;
                                }
                            }//for
                        }//else if
                    }
                }//if
            }//for

            return args.MatchedCount;
        }


        private bool _AgainFlag = false;
        /// <summary>
        /// 执行查找
        /// </summary>
        /// <param name="args">参数</param>
        /// <param name="setSelection">找到后是否设置文档选择状态</param>
        /// <param name="startIndex">开始序号</param>
        /// <returns>找到的元素在文档中的序号</returns>
        public int Search(SearchReplaceCommandArgs args, bool setSelection , int startIndex )
        {
            if (args == null)
            {
                throw new ArgumentNullException("args");
            }
            if (args.SearchID)
            {
                // 处于搜索ID的模式
                DomElement element = this.Document.GetElementById(args.SearchString);
                if (element != null)
                {
                    int result = element.FirstContentElementInPublicContent.ContentIndex;
                    if (setSelection)
                    {
                        if (setSelection)
                        {
                            element.Focus();
                        }
                    }
                    else
                    {
                        if (args.Backward == true)
                        {
                            this.Content.MoveToPosition(result + args.SearchString.Length);
                        }
                        else
                        {
                            this.Content.MoveToPosition(result);
                        }
                    }
                    return result;
                }
                else
                {
                    return -1;
                }
            }
            if (string.IsNullOrEmpty(args.SearchString))
            {
                return -1;
            }
            DCContent content = this.Content;
            int searchStringLength = args.SearchString.Length;
            int newStartIndex = -1;
            var ignoreCase = args.IgnoreCase;
            var excludeBackgroundText = args.ExcludeBackgroundText;
            var searchString = args.SearchString;
            if (args.Backward)
            {
            BackwardAgain: ;
                // 向后查找
                if (startIndex < 0)
                {
                    startIndex = content.Selection.AbsEndIndex;
                }
                if (this.Document.LastSearchStartPosition < 0)
                {
                    this.Document.LastSearchStartPosition = startIndex;
                    this._AgainFlag = false;
                }
                int contentLength = content.Count;
                var firstSearchChar = searchString[0];
                for (int iCount = startIndex; iCount < contentLength; iCount++)
                {
                    DomElement element = content[iCount];
                    if (element is DomCharElement && (args.SpecifyContainer == null || element.IsParentOrSupParent(args.SpecifyContainer) == true))
                    {
                        if (excludeBackgroundText)
                        {
                            if (element.Parent is DomFieldElementBase)
                            {
                                // 遇到背景文字，快速跳转
                                DomFieldElementBase field = (DomFieldElementBase)element.Parent;
                                if (field.IsBackgroundTextElement(element))
                                {
                                    iCount = content.FastIndexOf(field.EndElement);
                                    continue;
                                }
                            }
                        }
                        char c = ((DomCharElement)element).GetCharValue();
                        if (EqualsChar(c, firstSearchChar, ignoreCase))
                        {
                            if (searchStringLength == 1)
                            {
                                newStartIndex = iCount;
                                break;
                            }
                            else if (contentLength - iCount >= searchStringLength)
                            {
                                for (int iCount2 = 1; iCount2 < searchStringLength; iCount2++)
                                {
                                    DomElement element2 = content[iCount + iCount2];
                                    if (element2 is DomCharElement)
                                    {
                                        if (EqualsChar(
                                            ((DomCharElement)element2).GetCharValue(),
                                            searchString[iCount2],
                                            ignoreCase))
                                        {
                                            if (iCount2 == searchStringLength - 1)
                                            {
                                                newStartIndex = iCount;
                                                goto EndSetNewStartIndex;
                                            }
                                        }
                                        else
                                        {
                                            // 判断失败
                                            break;
                                        }
                                    }
                                    else
                                    {
                                        // 不是字符元素，判断失败
                                        break;
                                    }
                                }//for
                            }//else if
                        }
                    }//if
                }//for
                if (newStartIndex < 0)
                {
                    if (this._AgainFlag == false)
                    {
                        bool jump = true ;
                        if ( this.ShowUI && this.Document.GetDocumentBehaviorOptions().PromptJumpBackForSearch)
                        {
                            jump = MessageBox.Show(
                                null,
                                DCSR.PromptJumpStartForSearch,
                                DCSR.SystemAlert,
                                MessageBoxButtons.YesNo,
                                MessageBoxIcon.Question) == DialogResult.Yes;
                        }
                        if (jump)
                        {
                            // 没找到内容,试图跳到文档头重新查找
                            this._AgainFlag = true;
                            startIndex = 0;
                            goto BackwardAgain;
                        }
                    }
                    return newStartIndex;
                }
            }//if
            else
            {
            ForwardAgain: ;
                // 向前查找
                if (startIndex < 0)
                {
                    startIndex = content.Selection.AbsStartIndex;// - searchString.Length ;
                }
                if (this.Document.LastSearchStartPosition < 0)
                {
                    this.Document.LastSearchStartPosition = startIndex;
                    this._AgainFlag = false;
                }
                char lastChar = searchString[searchString.Length - 1];
                for (int iCount = startIndex - 1; iCount >= 0; iCount--)
                {
                    DomElement element = content[iCount];
                    if (element is DomCharElement && (args.SpecifyContainer == null || element.IsParentOrSupParent(args.SpecifyContainer) == true))
                    {
                        if (excludeBackgroundText)
                        {
                            if (element.Parent is DomFieldElementBase)
                            {
                                // 遇到背景文字，快速跳转
                                DomFieldElementBase field = (DomFieldElementBase)element.Parent;
                                if (field.IsBackgroundTextElement(element))
                                {
                                    iCount = content.FastIndexOf(field.StartElement);
                                    continue;
                                }
                            }
                        }
                        char c = ((DomCharElement)element).GetCharValue();
                        if (EqualsChar(c, lastChar, ignoreCase))
                        {
                            if (searchStringLength == 1)
                            {
                                newStartIndex = iCount;
                                break;
                            }
                            else if (iCount >= searchStringLength - 1 )
                            {
                                for (int iCount2 = searchStringLength - 2; iCount2 >= 0; iCount2--)
                                {
                                    DomElement element2 = content[iCount - searchStringLength + iCount2 + 1];
                                    if (element2 is DomCharElement)
                                    {
                                        if (EqualsChar(
                                            ((DomCharElement)element2).GetCharValue(),
                                            searchString[iCount2],
                                            ignoreCase))
                                        {
                                            if (iCount2 == 0)
                                            {
                                                newStartIndex = iCount - searchStringLength + 1 ;
                                                goto EndSetNewStartIndex;
                                            }
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
                                }
                            }
                        }
                    }
                }//for
                if (newStartIndex < 0)
                {
                    if (this._AgainFlag == false)
                    {
                        bool jump = true;
                        if (this.ShowUI && this.Document.GetDocumentBehaviorOptions().PromptJumpBackForSearch)
                        {
                        }
                        if (jump)
                        {
                            // 没找到内容,试图跳到文档尾重新查找
                            this._AgainFlag = true;
                            startIndex = content.Count - 1;
                            goto ForwardAgain;
                        }
                    }
                }
            }
        EndSetNewStartIndex: ;
            if (setSelection)
            {
                if (newStartIndex >= 0)
                {
                    int back = this.Document.LastSearchStartPosition;
                    
                    if (content.SetSelection(newStartIndex, args.SearchString.Length))
                    {
                        this.Document.LastSearchStartPosition = back;
                    }
                }
            }
            else
            {
                if (newStartIndex >= 0)
                {
                    if (args.Backward == true)
                    {
                        this.Content.MoveToPosition(newStartIndex + args.SearchString.Length);
                    }
                    else
                    {
                        this.Content.MoveToPosition(newStartIndex);
                    }
                }
            }

            return newStartIndex;
        }

        /// <summary>
        /// 执行查找
        /// </summary>
        /// <param name="args">参数</param>
        /// <param name="elements">文档元素对象列表</param>
        /// <param name="setSelection">找到后是否设置文档选择状态</param>
        /// <param name="startIndex">开始序号</param>
        /// <returns>找到的元素在文档中的序号</returns>
        public int Search(
            SearchReplaceCommandArgs args,
            DomElementList elements ,
            bool setSelection, 
            int startIndex)
        {
            if (args == null)
            {
                throw new ArgumentNullException("args");
            }
            if (elements == null)
            {
                throw new ArgumentNullException("elements");
            }
            if (elements.Count == 0)
            {
                return -1;
            }
            if (string.IsNullOrEmpty(args.SearchString))
            {
                return -1;
            }
            if (elements == null)
            {
                elements = this.Content;
            }
            int searchStringLength = args.SearchString.Length;
            int newStartIndex = -1;
            if (args.Backward)
            {
                // 向后查找
                if (startIndex < 0)
                {
                    startIndex = 0 ;
                }
                int contentLength = elements.Count;
                for (int iCount = startIndex; iCount < contentLength; iCount++)
                {
                    DomElement element = elements[iCount];
                    if (element is DomCharElement && (args.SpecifyContainer == null || element.IsParentOrSupParent(args.SpecifyContainer) == true))
                    {
                        char c = ((DomCharElement)element).GetCharValue();
                        if (EqualsChar(c, args.SearchString[0], args.IgnoreCase))
                        {
                            if (searchStringLength == 1)
                            {
                                newStartIndex = iCount;
                                break;
                            }
                            else if (contentLength - iCount >= searchStringLength)
                            {
                                for (int iCount2 = 1; iCount2 < searchStringLength; iCount2++)
                                {
                                    DomElement element2 = elements[iCount + iCount2];
                                    if (element2 is DomCharElement)
                                    {
                                        if (EqualsChar(
                                            ((DomCharElement)element2).GetCharValue(),
                                            args.SearchString[iCount2],
                                            args.IgnoreCase))
                                        {
                                            if (iCount2 == searchStringLength - 1)
                                            {
                                                newStartIndex = iCount;
                                                goto EndSetNewStartIndex2;
                                            }
                                        }
                                        else
                                        {
                                            // 判断失败
                                            break;
                                        }
                                    }
                                    else
                                    {
                                        // 不是字符元素，判断失败
                                        break;
                                    }
                                }//for
                            }//else if
                        }
                    }//if
                }//for
            }//if
            else
            {
                // 向前查找
                if (startIndex < 0)
                {
                    startIndex = elements.Count-1;
                }
                char lastChar = args.SearchString[args.SearchString.Length - 1];
                for (int iCount = startIndex; iCount >= 0; iCount--)
                {
                    DomElement element = elements[iCount];
                    if (element is DomCharElement && (args.SpecifyContainer == null || element.IsParentOrSupParent(args.SpecifyContainer) == true))
                    {
                        char c = ((DomCharElement)element).GetCharValue();
                        if (EqualsChar(c, lastChar, args.IgnoreCase))
                        {
                            if (searchStringLength == 1)
                            {
                                newStartIndex = iCount;
                                break;
                            }
                            else if (iCount >= searchStringLength - 1)
                            {
                                for (int iCount2 = searchStringLength - 2; iCount2 >= 0; iCount2--)
                                {
                                    DomElement element2 = elements[iCount - searchStringLength + iCount2 + 1];
                                    if (element2 is DomCharElement)
                                    {
                                        if (EqualsChar(
                                            ((DomCharElement)element2).GetCharValue(),
                                            args.SearchString[iCount2],
                                            args.IgnoreCase))
                                        {
                                            if (iCount2 == 0)
                                            {
                                                newStartIndex = iCount - searchStringLength + 1;
                                                goto EndSetNewStartIndex2;
                                            }
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
                                }
                            }
                        }
                    }
                }//for
            }
        EndSetNewStartIndex2: ;
            if (setSelection)
            {
                if (newStartIndex >= 0)
                {
                    DomElement element = elements[newStartIndex];
                    int index = this.Content.IndexOf(element);
                    if (index >= 0)
                    {
                        if (this.Content.SetSelection(index, args.SearchString.Length))
                        {
                        }
                    }
                }
            }
            else
            {
                if (newStartIndex >= 0)
                {
                    if (args.Backward == true)
                    {
                        this.Content.MoveToPosition(newStartIndex + args.SearchString.Length);
                    }
                    else
                    {
                        this.Content.MoveToPosition(newStartIndex);
                    }
                }
            }

            //////////////////////////////////////////
            return newStartIndex;
        }

        private bool EqualsChar(char c1, char c2, bool ignoreCase)
        {
            if (c1 == c2)
            {
                return true;
            }
            if (ignoreCase)
            {
                int index1 = -1;
                if( c1 >='a' && c1 <='z')
                {
                    index1 = c1 - 'a';
                }
                else if( c1 >='A' && c1 <='Z')
                {
                    index1 = c1 - 'A';
                }
                int index2 = -1;
                if (c2 >= 'a' && c2 <= 'z')
                {
                    index2 = c2 - 'a';
                }
                else if (c2 >= 'A' && c2 <= 'Z')
                {
                    index2 = c2 - 'A';
                }
                return index1 >= 0 && index1 == index2;
            }
            return false;
        }
    }
}
