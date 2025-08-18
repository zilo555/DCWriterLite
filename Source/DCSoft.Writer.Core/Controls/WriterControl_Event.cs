using DCSoft.Writer.Dom;
using DCSoft.Writer.Commands;
using System.Windows.Forms;
// // 
using System.ComponentModel;
using DCSoft.Writer.Data;
using System.Runtime.InteropServices;

namespace DCSoft.Writer.Controls
{
    // 编辑器控件事件处理模块
    public partial class WriterControl
    {

        public virtual void OnEventObjectElementMouseClick(DomObjectElement element, DocumentEventArgs args)
        {

        }

        private DefaultWriterControlProvider _Provider = null;
        internal DefaultWriterControlProvider Provider
        {
            get
            {
                if (_Provider == null)
                {
                    _Provider = new DefaultWriterControlProvider();
                    this._Provider.Control = this;
                }
                return _Provider;
            }
        }
        #region 事件管理
        ///<summary>事件 CommandError 的名称</summary>
        public const string EVENTNAME_CommandError = "CommandError";
        ///<summary>事件 DocumentContentChanged 的名称</summary>
        public const string EVENTNAME_DocumentContentChanged = "DocumentContentChanged";
        ///<summary>事件 DocumentLoad 的名称</summary>
        public const string EVENTNAME_DocumentLoad = "DocumentLoad";
        ///<summary>事件 EventContentChanged 的名称</summary>
        public const string EVENTNAME_EventContentChanged = "EventContentChanged";
        ///<summary>事件 EventContentChanging 的名称</summary>
        public const string EVENTNAME_EventContentChanging = "EventContentChanging";
        ///<summary>事件 EventElementCheckedChanged 的名称</summary>
        public const string EVENTNAME_EventElementCheckedChanged = "EventElementCheckedChanged";
        ///<summary>事件 EventElementGotFocus 的名称</summary>
        public const string EVENTNAME_EventElementGotFocus = "EventElementGotFocus";
        ///<summary>事件 EventElementLostFocus 的名称</summary>
        public const string EVENTNAME_EventElementLostFocus = "EventElementLostFocus";
        ///<summary>事件 EventElementMouseClick 的名称</summary>
        public const string EVENTNAME_EventElementMouseClick = "EventElementMouseClick";
        ///<summary>事件 EventElementMouseDblClick 的名称</summary>
        public const string EVENTNAME_EventElementMouseDblClick = "EventElementMouseDblClick";
        ///<summary>事件 EventElementMouseDown 的名称</summary>
        public const string EVENTNAME_EventElementMouseDown = "EventElementMouseDown";
        ///<summary>事件 EventElementMouseMove 的名称</summary>
        public const string EVENTNAME_EventElementMouseMove = "EventElementMouseMove";
        ///<summary>事件 EventElementMouseUp 的名称</summary>
        public const string EVENTNAME_EventElementMouseUp = "EventElementMouseUp";
        ///<summary>事件 EventElementValidating 的名称</summary>
        public const string EVENTNAME_EventElementValidating = "EventElementValidating";
        ///<summary>事件 SelectionChanged 的名称</summary>
        public const string EVENTNAME_SelectionChanged = "SelectionChanged";
        ///<summary>事件 SelectionChanging 的名称</summary>
        public const string EVENTNAME_SelectionChanging = "SelectionChanging";
        ///<summary>事件 StatusTextChanged 的名称</summary>
        public const string EVENTNAME_StatusTextChanged = "StatusTextChanged";

        #endregion

        #region 触发标准Windows控件事件

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (this.GetInnerViewControl() != null)
            {
                this.GetInnerViewControl().InnerOnKeyDown(e);
            }
        }

        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            if (this.GetInnerViewControl() != null)
            {
                this.GetInnerViewControl().InnerOnKeyPress(e);
            }
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            if (this.GetInnerViewControl() != null)
            {
                this.GetInnerViewControl().InnerOnKeyUp(e);
            }
        }

        #endregion


        /// <summary>
        /// 触发EventBeforePlayMedia事件
        /// </summary>
        internal virtual void OnEventElementValidating(ElementValidatingEventArgs args)
        {
            this.WASMRaiseEvent(WriterControl.EVENTNAME_EventElementValidating, args);
        }

        /// <summary>
        /// 触发EventElementCheckedChanged事件
        /// </summary>
        /// <param name="elementID">元素编号</param>
        /// <param name="elementName">元素名称</param>
        /// <param name="elementValue">元素数值</param>
        /// <param name="element">文档元素对象</param>
        public virtual void OnEventElementCheckedChanged(
           string elementID,
           string elementName,
           string elementValue,
           DomElement element)
        {
            this.WASMRaiseEvent(WriterControl.EVENTNAME_EventElementCheckedChanged, new string[] { elementID, elementName, elementValue });
        }



        /// <summary>
        /// 触发StatusTextChanged事件
        /// </summary>
        /// <param name="args">事件参数</param>
        internal virtual void OnStatusTextChanged(StatusTextChangedEventArgs args)
        {
            this.WASMRaiseEvent(WriterControl.EVENTNAME_StatusTextChanged, args.StatusText);
        }

        /// <summary>
        /// 触发EventElementMouseClick事件
        /// </summary>
        /// <param name="args">事件参数</param>
        internal virtual void OnEventElementMouseClick(ElementMouseEventArgs args)
        {
            this.WASMRaiseEvent(WriterControl.EVENTNAME_EventElementMouseClick, args);
        }

        /// <summary>
        /// 触发EventElementGotFocus事件
        /// </summary>
        /// <param name="args">事件参数</param>
        internal virtual void OnEventElementGotFocus(ElementEventArgs args)
        {
            this.WASMRaiseEvent(WriterControl.EVENTNAME_EventElementGotFocus, args);
        }
        /// <summary>
        /// 触发EventElementLostFocus事件
        /// </summary>
        /// <param name="args">事件参数</param>
        internal virtual void OnEventElementLostFocus(ElementEventArgs args)
        {
            this.WASMRaiseEvent(WriterControl.EVENTNAME_EventElementLostFocus, args);
        }

        internal virtual void OnEventElementMouseDblClick(ElementMouseEventArgs args)
        {
            this.WASMRaiseEvent(WriterControl.EVENTNAME_EventElementMouseDblClick, args);
        }

        /// <summary>
        /// 触发EventElementMouseDown事件
        /// </summary>
        /// <param name="args">事件参数</param>
       //[System.Reflection.Obfuscation(Exclude = false, ApplyToMembers = false)]
        internal virtual void OnEventElementMouseDown(ElementMouseEventArgs args)
        {
            this.WASMRaiseEvent(WriterControl.EVENTNAME_EventElementMouseDown, args);
        }


        /// <summary>
        /// 触发EventElementMouseMove事件
        /// </summary>
        /// <param name="args">事件参数</param>
        internal virtual void OnEventElementMouseMove(ElementMouseEventArgs args)
        {
            this.WASMRaiseEvent(WriterControl.EVENTNAME_EventElementMouseMove, args);
        }


        /// <summary>
        /// 触发EventElementMouseUp事件
        /// </summary>
        /// <param name="args">事件参数</param>
        internal virtual void OnEventElementMouseUp(ElementMouseEventArgs args)
        {
            this.WASMRaiseEvent(WriterControl.EVENTNAME_EventElementMouseUp, args);
        }

        /// <summary>
        /// 触发EventContentChanging事件
        /// </summary>
        /// <param name="args">参数</param>
        internal virtual void OnEventContentChanging(ContentChangingEventArgs args)
        {
            if (this.Document.EnableContentChangedEvent() == false)
            {
                // 禁止触发内容修改事件
                return;
            }
            this.WASMRaiseEvent(WriterControl.EVENTNAME_EventContentChanging, args);
        }

        /// <summary>
        /// 触发EventContentChanged事件
        /// </summary>
        /// <param name="args">参数</param>
        internal virtual void OnEventContentChanged(ContentChangedEventArgs args)
        {
            if (this.Document.EnableContentChangedEvent() == false)
            {
                // 禁止触发内容修改事件
                return;
            }
            this.WASMRaiseEvent(WriterControl.EVENTNAME_EventContentChanged, args);
        }

        /// <summary>
        /// DCWriter内部使用。触发文档加载事件，触发此事件时，文档已经加载成功，但尚未显示出来。
        /// </summary>
        /// <param name="args">事件参数</param>
        public virtual void OnDocumentLoad(WriterEventArgs args)
        {
            this.WASMRaiseEvent(WriterControl.EVENTNAME_DocumentLoad, args);
            args.Dispose();

        }


        /// <summary>
        /// DCWriter内部使用。向文档插入对象数据
        /// </summary>
        /// <param name="args">参数</param>

        public virtual void OnEventInsertObject(InsertObjectEventArgs args)
        {
                this.DocumentControler.InsertObject(args);
        }


        /// <summary>
        /// 触发文档选择状态发生改变事件
        /// </summary>
        /// <param name="args">事件参数</param>
        internal virtual void OnSelectionChanging(SelectionChangingEventArgs args)
        {
            this.WASMRaiseEvent(WriterControl.EVENTNAME_SelectionChanging, args); ;
        }

        /// <summary>
        /// DCWriter内部使用。触发文档选择状态发生改变后的事件
        /// </summary>
        /// <param name="args">事件参数</param>
        public virtual void OnSelectionChanged(EventArgs args)
        {
            var document = this.Document;
            this.ctlHRule.CurrentParagraphFlag = document.CurrentParagraphEOF;
            this.WASMRaiseEvent(WriterControl.EVENTNAME_SelectionChanged, args);
            if (this.CommandControler != null)
            {
                this.CommandControler.EditControl = this;
                this.GetInnerViewControl()?.InvalidateCommandState();
            }
        }

        /// <summary>
        /// 触发文档内容发生改变事件
        /// </summary>
        /// <param name="args">事件参数</param>

        public virtual void OnDocumentContentChanged(EventArgs args)
        {
            this.Provider.OnDocumentContentChanged(args);
        }


        /// <summary>
        /// DCWriter内部使用。触发自定义的错误事件
        /// </summary>
        /// <param name="cmd">编辑器命令对象</param>
        /// <param name="cmdArgs">参数</param>
        /// <param name="exp">异常对象</param>
        public virtual void OnCommandError(
            WriterCommand cmd,
            WriterCommandEventArgs cmdArgs,
            Exception exp)
        {
            this.Provider.OnCommandError(cmd, cmdArgs, exp);
        }

    }
}