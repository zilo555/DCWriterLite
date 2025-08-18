using System;
using System.Collections.Generic;
using System.Text;
using DCSoft.Writer.Dom;
using System.Runtime.InteropServices ;

namespace DCSoft.Writer
{

    /// <summary>
    /// 文档内容发生改变事件参数
    /// </summary>
    /// <remarks>编写 袁永福</remarks>
     
    [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = false )]
    public partial class ContentChangedEventArgs   : EventArgs
    {
        /// <summary>
        /// 初始化对象
        /// </summary>
        //[DCSoft.Common.DCPublishAPI]
        public ContentChangedEventArgs()
        {
        }
        /// <summary>
        /// 初始化对象
        /// </summary>
        /// <param name="doc">文档对象</param>
        /// <param name="element">文档元素对象</param>
        /// <param name="src">事件来源</param>
        public ContentChangedEventArgs( DomDocument doc , DomElement element , ContentChangedEventSource src )
        {
            this._Document = doc;
            this._Element = element;
            this._EventSource = src;
        }

        private ContentChangedEventSource _EventSource = ContentChangedEventSource.Default;
        /// <summary>
        /// 事件来源
        /// </summary>
        //[DCSoft.Common.DCPublishAPI]
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public ContentChangedEventSource EventSource
        {
            [JSInvokable]
            get
            {
                return _EventSource; 
            }
            set
            {
                _EventSource = value; 
            }
        }

        private bool _OnlyStyleChanged = false;
        /// <summary>
        /// 只是样式发生了改变
        /// </summary>
        //[DCSoft.Common.DCPublishAPI]
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public bool OnlyStyleChanged
        {
            [JSInvokable]
            get
            {
                return _OnlyStyleChanged; 
            }
            set
            {
                _OnlyStyleChanged = value; 
            }
        }

        //private bool _UndoRedoCause = false;
        /// <summary>
        /// 由于进行重做/撤销操作而引发了该事件
        /// </summary>
        //[DCSoft.Common.DCPublishAPI]
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public bool UndoRedoCause
        {
            [JSInvokable]
            get
            {
                return this.EventSource == ContentChangedEventSource.UndoRedo;
            }
            //set
            //{
            //    _UndoRedoCause = value;
            //}
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

        private bool _LoadingDocument = false;
        /// <summary>
        /// 正在加载文档标志
        /// </summary>
        //[DCSoft.Common.DCPublishAPI]
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public bool LoadingDocument
        {
            get
            {
                return _LoadingDocument;
            }
            set
            {
                _LoadingDocument = value;
            }
        }

        private object _Tag = null;
        /// <summary>
        /// 额外的数据
        /// </summary>
        //[DCSoft.Common.DCPublishAPI]
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        [System.Text.Json.Serialization.JsonIgnore]
        public object Tag
        {
            get
            {
                return _Tag;
            }
            set
            {
                _Tag = value;
            }
        }

        private DomElement _Element = null;
        /// <summary>
        /// 容器元素对象
        /// </summary>
        //[DCSoft.Common.DCPublishAPI]
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

        private int _ElementIndex = 0;
        /// <summary>
        /// 发生操作时的元素位置序号
        /// </summary>
        //[DCSoft.Common.DCPublishAPI]
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public int ElementIndex
        {
            [JSInvokable]
            get
            {
                return _ElementIndex;
            }
            set
            {
                _ElementIndex = value;
            }
        }

        private DomElementList _DeletedElements = null;
        /// <summary>
        /// 正要删除的元素列表
        /// </summary>
        //[DCSoft.Common.DCPublishAPI]
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        [System.Text.Json.Serialization.JsonIgnore]
        public DomElementList DeletedElements
        {
            get
            {
                return _DeletedElements;
            }
            set
            {
                _DeletedElements = value;
            }
        }

        private DomElementList _InsertedElements = null;
        /// <summary>
        /// 准备新增的元素列表
        /// </summary>
        //[DCSoft.Common.DCPublishAPI]
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        [System.Text.Json.Serialization.JsonIgnore]
        public DomElementList InsertedElements
        {
            get { return _InsertedElements; }
            set { _InsertedElements = value; }
        }

        private bool _CancelBubble = false;
        /// <summary>
        /// 取消事件向上层元素冒泡传递
        /// </summary>
        //[DCSoft.Common.DCPublishAPI]
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public bool CancelBubble
        {
            [JSInvokable]
            get { return _CancelBubble; }
            [JSInvokable]
            set { _CancelBubble = value; }
        }

        private bool _Handled = false;
        /// <summary>
        /// 事件已经被处理了，无需后续处理
        /// </summary>
        //[DCSoft.Common.DCPublishAPI]
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public bool Handled
        {
            [JSInvokable]
            get { return _Handled; }
            [JSInvokable]
            set { _Handled = value; }
        }

    }

    /// <summary>
    /// 内容修改事件来源
    /// </summary>
     
    [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
    public enum ContentChangedEventSource
    {
        /// <summary>
        /// 默认方式
        /// </summary>
        Default ,
        /// <summary>
        /// 撤销操作而导致的内容修改
        /// </summary>
        UndoRedo ,
        /// <summary>
        /// 数值编辑器而导致的内容修改
        /// </summary>
        ValueEditor ,
        /// <summary>
        /// 编程而导致的内容修改
        /// </summary>
        Program,
        /// <summary>
        /// 数据源绑定而导致的内容修改
        /// </summary>
        DataBinding,
        /// <summary>
        /// 用户界面插入删除内容而导致的内容修改
        /// </summary>
        UI
    }
}
