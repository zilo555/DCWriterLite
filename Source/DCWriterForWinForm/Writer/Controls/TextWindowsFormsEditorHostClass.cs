using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using DCSoft.WinForms ;
using System.ComponentModel;
using DCSoft.Writer.Dom;
using DCSoft.WinForms.Native;

namespace DCSoft.Writer.Controls
{
    /// <summary>
    /// 窗体环境的编辑器宿主对象
    /// </summary>
    public class TextWindowsFormsEditorHostClass
    {
 
        public void SetOwner( WriterControl ctl , DomDocument doc )
        {
            this._EditControl = ctl;
            this._Document = doc;
        }

        private WriterControl _EditControl = null;
        /// <summary>
        /// 编辑器对象
        /// </summary>
        public WriterControl EditControl
        {
            get
            {
                return _EditControl; 
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
        }

        private bool _KeepWriterControlFocused = false;
        /// <summary>
        /// 弹出用户界面时仍然保持编辑器控件获得输入焦点
        /// </summary>
        public bool KeepWriterControlFocused
        {
            get 
            {
                return _KeepWriterControlFocused; 
            }
            set
            {
                _KeepWriterControlFocused = value; 
            }
        }

        public void ResetState()
        {
            this._CurrentEditContext = null;
            //this._ElementInstance = null;
        }

        private ElementValueEditContext _CurrentEditContext = null;
        /// <summary>
        /// 当前正在执行的文档元素值编辑操作上下文对象
        /// </summary>
        public ElementValueEditContext CurrentEditContext
        {
            get
            {
                return _CurrentEditContext; 
            }
        }

        private bool _EditingValue = false;
        /// <summary>
        /// 正在编辑数值
        /// </summary>
        public bool EditingValue
        {
            get { return _EditingValue; }
        }
        public void EndEditingValue()
        {
            //this._EditingValue = false;
            //this._CurrentEditContext = null;
            //if( this._EditControl != null && this._EditControl.InnerViewControl != null )
            //{
            //    this._EditControl.InnerViewControl.ClearWinMessage();
            //}
        }
        /// <summary>
        /// 编辑文档元素数值
        /// </summary>
        /// <param name="element">文档元素对象</param>
        /// <param name="editor">编辑器对象</param>
        /// <returns>操作是否成功</returns>
        public ElementValueEditResult EditValue(DomElement element, ElementValueEditor editor)
        {
            if (editor == null)
            {
                throw new ArgumentNullException("editor");
            }
            if (this._EditingValue)
            {
                return ElementValueEditResult.None;
            }
            this._EditingValue = true;
            try
            {
                this.EditControl.Focus();                
                ElementValueEditContext context = new ElementValueEditContext();
                context.Document = this.Document;
                context.Element = element;
                context.PropertyName = null;
                context.Editor = editor;
                this._CurrentEditContext = context;
                this._CurrentEditContext.EditStyle = editor.GetEditStyle(this, context);
                this.RaiseDocumentContentChangedOnceAfterEditValue = false;
                //System.Diagnostics.Debug.WriteLine(element.ID);
                ElementValueEditResult result = editor.EditValue(this, context);
                this._CurrentEditContext = null;
                this._EditingValue = false;
                if (this.RaiseDocumentContentChangedOnceAfterEditValue)
                {
                    this.RaiseDocumentContentChangedOnceAfterEditValue = false;
                    this.Document.OnDocumentContentChanged();
                }
                return result;
            }
            finally
            {
                this._CurrentEditContext = null;
                this._EditingValue = false;
            }
//#endif
        }

        /// <summary>
        /// 在编辑数值后触发一次DocumentContentChanged事件
        /// </summary>
        private bool _RaiseDocumentContentChangedOnceAfterEditValue = false;
        /// <summary>
        /// 在编辑数值后触发一次DocumentContentChanged事件
        /// </summary>
        public bool RaiseDocumentContentChangedOnceAfterEditValue
        {
            get
            {
                return _RaiseDocumentContentChangedOnceAfterEditValue; 
            }
            set
            {
                _RaiseDocumentContentChangedOnceAfterEditValue = value; 
            }
        }
        

        /// <summary>
        /// 取消当前编辑操作
        /// </summary>
        public void CancelEditValue()
        {
        }
        #region IWindowsFormsEditorService 成员
        public bool ShowingDropDown { get { return false; } }
        //public System.Windows.Forms.Control GetCurrentValueEditControl() { return null; }

        public static DomElement GetAnchorElement( DomElement element )
        {
            if ((element is DomParagraphFlagElement) == false )
            {
                var curLine = element.DocumentContentElement.Content.CurrentLine;
                if (element is DomInputFieldElement && curLine != null)
                {
                    var field = (DomInputFieldElement)element;
                    foreach (var e9 in curLine.LayoutElements)
                    {
                        if (e9.Parent == field)
                        {
                            element = e9;
                            break;
                        }
                    }
                }
                else
                {
                    {
                        element = element.FirstContentElement;
                    }
                }
            }
            return element;
        }

#endregion

        #region IDisposable 成员
        /// <summary>
        /// 销毁对象
        /// </summary>
        public void Dispose()
        {
            this._CurrentEditContext = null;
            this._Document = null;
            this._EditControl = null;
            //this._ElementInstance = null;
        }

        #endregion

    }
}
