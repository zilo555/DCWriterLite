using System;
using System.Collections.Generic;
using System.Text;
using DCSoft.Writer.Controls;
using DCSoft.Drawing;
//using System.Windows.Forms;
using DCSoft.Common;
using System.Runtime.InteropServices;
using System.Reflection;
//using DCSoft.Writer.Script;
using DCSoft.Printing;

// 处理文档事件模块
namespace DCSoft.Writer.Dom
{
    partial class DomDocument
    {

        /// <summary>
        /// 创建事件参数
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        /// <seealso cref="https://developer.mozilla.org/en-US/docs/Web/API/Document/createEvent"/>
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public DocumentEventArgs createEvent(string type)
        {
            DocumentEventStyles style2 = DocumentEventStyles.MouseClick;
            if (type != null && type.Length > 0)
            {
                style2 = (DocumentEventStyles)Enum.Parse(typeof(DocumentEventStyles), type, true);
            }
            DocumentEventArgs args = new DocumentEventArgs(this, this, style2);
            return args;
        }



        #region DocumentContentChanged

        /// <summary>
        /// 文档内容发生改变事件,当用户修改了文档的任何内容时就会触发该事件。
        /// </summary>
        public System.EventHandler DocumentContentChanged = null;
        /// <summary>
        /// 触发文档内容发生改变事件.本方法内部还会调用文档对象绑定的编辑器控件的OnDocumentContentChanged方法。
        /// </summary>
        public void OnDocumentContentChanged()
        {
            if (this.EditorControl != null)
            {
                this.EditorControl.CommandStateNeedRefreshFlag = true;
            }
            this._HoverElement = null;
            if (this.EnableContentChangedEvent())
            {
                if (this.EditorControl != null)
                {
                    this.EditorControl.OnDocumentContentChanged(EventArgs.Empty);
                }
                if (DocumentContentChanged != null)
                {
                    DocumentContentChanged(this, EventArgs.Empty);
                }
            }
        }

        #endregion

        #region SelectionChanged
        /// <summary>
        /// 文档选择状态发生改变后的事件,包括选择区域改变或插入点位置的改变。
        /// </summary>
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public EventHandler SelectionChanged = null;
        [NonSerialized]
        private int _OnSelectionChangedWithCheckVersion_SelectionVersion = -1;

        internal void OnSelectionChangedWithCheckVersion()
        {
            int v = this.Selection.StateVersion;
            if (v != this._OnSelectionChangedWithCheckVersion_SelectionVersion)
            {
                this._OnSelectionChangedWithCheckVersion_SelectionVersion = v;
                this.OnSelectionChanged();
            }
        }

        //private static readonly string _TaskName_OnSelectionChanged = "TaskName_OnSelectionChanged";
        /// <summary>
        /// DCWriter内部使用。文档内容状态发生改变处理,本方法还会调用文档对象绑定的编辑器控件的OnSelectionChanged方法。
        /// </summary>
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public void OnSelectionChanged()
        {
            if (this.IsLoadingDocument)
            {
                return;
            }
            this._OnSelectionChangedWithCheckVersion_SelectionVersion = this.Selection.StateVersion;
            this.LastSearchStartPosition = -1;
            if (this.EditorControl != null)
            {
                this.EditorControl.CommandStateNeedRefreshFlag = true;
                // 设置可以被拖拽的元素
                {
                    DomContainerElement c = null;
                    int index = 0;
                    this.Content.GetCurrentPositionInfo(out c, out index);
                    DomElement dge = null;
                    while (c != null)
                    {
                        if (c is DomTableElement)
                        {
                            var ctl = this.DocumentControler;
                            if (ctl != null && ctl.CanModify(c))
                            {
                                dge = c;
                                break;
                            }
                        }
                        c = c.Parent;
                    }
                    this.InnerViewControl.DragableElement = dge;
                }
            }
            if (this.EditorControl != null)
            {
                this.EditorControl.OnSelectionChanged(EventArgs.Empty);
                if (this.InnerViewControl.UpdateCurrentPage())
                {
                    this.InnerViewControl.OnCurrentPageChanged();
                }
            }
            if (this.CurrentContentElement is DomDocumentBodyElement
                && this.Selection.Length == 0)
            {
                //if (this._NormalViewMode)
                //{
                //    foreach (var page in this.PagesForNormalViewMode)
                //    {
                //        page.ClientSelectionBounds = Rectangle.Empty;
                //    }
                //}
                //else
                {
                    ClearPagesClientSelectionBounds();
                }
            }
            if (SelectionChanged != null)
            {
                SelectionChanged(this, EventArgs.Empty);
            }
        }

        internal void ClearPagesClientSelectionBounds()
        {
            foreach (var page in this.Pages)
            {
                page.ClientSelectionBounds = Rectangle.Empty;
            }
        }
        /// <summary>
        /// 仅仅触发文档和控件的SelectionChanged事件，不触发脚本事件和其他操作。
        /// </summary>
        private void RaiseSelectionChangedEvent()
        {
            if (this.EditorControl != null)
            {
                this.EditorControl.OnSelectionChanged(EventArgs.Empty);
            }
            if (SelectionChanged != null)
            {
                SelectionChanged(this, EventArgs.Empty);
            }
        }

        #endregion

        ///// <summary>
        ///// 只触发选择内容发生改变事件,但不更新文档的一些信息
        ///// </summary>
        //public virtual void RaiseSelectedChangedEvent()
        //{
        //    if (_EditorControl != null)
        //    {
        //        _EditorControl.OnSelectionChanged(EventArgs.Empty);
        //    }
        //    if (SelectionChanged != null)
        //    {
        //        SelectionChanged(this, null);
        //    }
        //}

        #region SelectionChanging
        /// <summary>
        /// 文档选择状态正在发生改变事件
        /// </summary>
        [field: NonSerialized]
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public SelectionChangingEventHandler SelectionChanging = null;

        /// <summary>
        /// 文档内容选择状态发生改变处理,本方法还会调用文档绑定的编辑器控件的OnSelectionChanging方法。
        /// </summary>
        /// <param name="args">事件参数</param>
        internal void OnSelectionChanging(SelectionChangingEventArgs args)
        {
            //XTextDocumentContentElement ce = this.CurrentContentElement;
            if (this.EditorControl != null)
            {
                this.EditorControl.OnSelectionChanging(args);
            }
            if (SelectionChanging != null)
            {
                SelectionChanging(this, args);
            }
        }

        #endregion

        //#region DocumentLoad

        ///// <summary>
        ///// 文档开始工作事件，这个是应用程序主动触发的事件，编辑器内部不触发这个事件。
        ///// </summary>
        //[field: NonSerialized]
        //[System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        ////[DCSoft.Common.DCPublishAPI]
        //public event EventHandler DocumentStartWork = null;
        ///// <summary>
        ///// 触发文档的DocumentStartWork事件。
        ///// </summary>
        ///// <param name="args">事件参数</param>
        //public void OnDocumentStartWork(EventArgs args)
        //{
        //    // 触发脚本
        //    this.RaiseEventScriptHandler("DocumentStartWork", new object[] { this, args });
        //    if (DocumentStartWork != null)
        //    {
        //        DocumentStartWork(this, args);
        //    }
        //    //if (this.EditorControl != null)
        //    //{
        //    //    this.EditorControl.OnDocumentLoad(args);
        //    //}
        //}

        ///// <summary>
        ///// 文档开始工作。
        ///// </summary>
        //[System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        ////[DCSoft.Common.DCPublishAPI]
        //[ComVisible( true )]
        //public void StartWork()
        //{
        //    OnDocumentStartWork(EventArgs.Empty);
        //}

        //#endregion

        #region DocumentLoad

        /// <summary>
        /// 文档加载事件
        /// </summary>
        [field: NonSerialized]
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        //[DCSoft.Common.DCPublishAPI]
        public EventHandler DocumentLoad = null;
        /// <summary>
        /// 触发文档的DocumentLoad事件。本方法内部还会调用文档绑定的编辑器控件的OnDocumentLoad方法。
        /// </summary>
        /// <param name="args">事件参数</param>
        public void OnDocumentLoad(EventArgs args)
        {
            if (this.DocumentLoad != null)
            {
                this.DocumentLoad(this, args);
            }
            //if (this.EditorControl != null)
            //{
            //    this.EditorControl.OnDocumentLoad(args);
            //}
        }

        #endregion

        #region EventElementValidating

        /// <summary>
        /// 文档元素内容校验事件
        /// </summary>
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        [field: NonSerialized]
        //[DCSoft.Common.DCPublishAPI]
        public ElementValidatingEventHandler EventElementValidating = null;

        /// <summary>
        /// 触发EventBeforePlayMedia事件
        /// </summary>
        [System.Reflection.Obfuscation(Exclude = false, ApplyToMembers = false)]
        public void OnEventElementValidating(ElementValidatingEventArgs args)
        {
            if (args.Handled)
            {
                return;
            }
            if (this.EventElementValidating != null)
            {
                this.EventElementValidating(this, args);
                if (args.Handled)
                {
                    return;
                }
            }
            if (this.EditorControl != null)
            {
                this.EditorControl.OnEventElementValidating(args);
            }
        }

        #endregion

        #region EventBeforePaintElement

        #endregion

        #region EventAfterPaintElement

        #endregion

    }
}
