using System;
using System.Collections.Generic;
using System.Text;
using DCSoft.Writer.Dom;
using System.Runtime.InteropServices;
using DCSoft.Writer.Controls;
//using Microsoft.JSInterop;

namespace DCSoft.Writer
{

    /// <summary>
    /// 文档元素事件参数
    /// </summary>
    public partial class ElementEventArgs : EventArgs ,IDisposable
    {
        /// <summary>
        /// 初始化对象
        /// </summary>
         
        public ElementEventArgs()
        {
        }

        /// <summary>
        /// 初始化对象
        /// </summary>
        /// <param name="element">文档元素对象</param>
         
        public ElementEventArgs(DomElement element)
        {
            _Document = element.OwnerDocument;
            _Element = element;
            if (_Document != null)
            {
                _WriterControl = _Document.EditorControl;
            }
        }


        /// <summary>
        /// 初始化对象
        /// </summary>
        /// <param name="ctl">编辑器控件</param>
        /// <param name="document">文档对象</param>
        /// <param name="element">文档元素对象</param>
         
        public ElementEventArgs(WriterControl ctl, DomDocument document, DomElement element)
        {
            _WriterControl = ctl;
            _Document = document;
            _Element = element;
        }

        private WriterControl _WriterControl = null;
        /// <summary>
        /// 编辑器控件对象
        /// </summary>
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        [System.Text.Json.Serialization.JsonIgnore]
        public WriterControl WriterControl
        {
            get
            {
                return _WriterControl;
            }
            set
            {
                _WriterControl = value;
            }
        }

        private DomDocument _Document = null;
        /// <summary>
        /// 文档对象
        /// </summary>
        //[DCSoft.Common.DCPublishAPI]
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        [System.Text.Json.Serialization.JsonIgnore]
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


        private DomElement _Element = null;
        /// <summary>
        /// 文档元素对象
        /// </summary>
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        [System.Text.Json.Serialization.JsonIgnore]
        public DomElement Element
        {
            get
            {
                return _Element;
            }
            set
            {
                _Element = value;
            }
        }
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public string ElementTypeName
        {
            [JSInvokable]
            get
            {
                return this._Element?.GetType().Name;
            }
        }
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public string ElementID
        {
            [JSInvokable]
            get
            {
                return this._Element?.ID;
            }
        }
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public string ElementName
        {
            [JSInvokable]
            get
            {
                return WriterUtilsInner.GetElementName(this._Element);
            }
        }
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public int ElementHashCode
        {
            [JSInvokable]
            get
            {
                if (this._Element == null)
                {
                    return 0;
                }
                else
                {
                    return this._Element.GetHashCode();
                }
            }
        }

        private bool _Handled = false;
        /// <summary>
        /// 事件已经被处理了，后续无需继续处理
        /// </summary>
        //[DCSoft.Common.DCPublishAPI]
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public bool Handled
        {
            [JSInvokable]
            get
            {
                return _Handled;
            }
            [JSInvokable]
            set
            {
                _Handled = value;
            }
        }

        private bool _CancelBubble = false;
        /// <summary>
        /// 取消事件冒泡
        /// </summary>
        //[DCSoft.Common.DCPublishAPI]
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public bool CancelBubble
        {
            [JSInvokable]
            get
            {
                return _CancelBubble;
            }
            [JSInvokable]
            set
            {
                _CancelBubble = value;
            }
        }

        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public virtual void Dispose()
        {
            this._Element = null;
            this._WriterControl = null;
            //if (this._TargetElement != null)
            //{
            //    this._TargetElement.Dispose();
            //    this._TargetElement = null;
            //}
        }
    }
     
}
