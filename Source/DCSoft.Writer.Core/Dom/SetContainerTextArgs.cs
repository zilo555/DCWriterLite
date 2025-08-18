using System;
using System.Collections.Generic;
using System.Text;

namespace DCSoft.Writer.Dom
{
    /// <summary>
    /// 设置容器元素方法参数
    /// </summary>
     
    [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = false)]
    //[DCSoft.Common.DCPublishAPI]
    public partial class SetContainerTextArgs
    {
        /// <summary>
        /// 初始化对象
        /// </summary>
        public SetContainerTextArgs()
        {
        }

        private ContentChangedEventSource _EventSource = ContentChangedEventSource.Default;
        /// <summary>
        /// 事件来源
        /// </summary>
        //[DCSoft.Common.DCPublishAPI]
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public ContentChangedEventSource EventSource
        {
            get 
            {
                return _EventSource; 
            }
            set
            {
                _EventSource = value; 
            }
        }

        private string _NewText = null;
        /// <summary>
        /// 新文本
        /// </summary>
        //[DCSoft.Common.DCPublishAPI]
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public string NewText
        {
            get
            {
                return _NewText; 
            }
            set
            {
                _NewText = value; 
            }
        }
        private bool _SetNewInnerValue = false;
        public bool SetNewInnerValue
        {
            get 
            {
                return this._SetNewInnerValue; 
            }
            set
            {
                this._SetNewInnerValue = value;
            }
        }
        private string _NewInnerValue = null;
        /// <summary>
        /// 对于输入域，新的内部文本值
        /// </summary>
        //[DCSoft.Common.DCPublishAPI]
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public string NewInnerValue
        {
            get
            {
                return _NewInnerValue;
            }
            set
            {
                _NewInnerValue = value;
            }
        }

        private bool _IgnoreDisplayFormat = false ;
        /// <summary>
        /// 忽略掉显示格式
        /// </summary>
       // [DCSoft.Common.DCPublishAPI]
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public bool IgnoreDisplayFormat
        {
            get
            {
                return _IgnoreDisplayFormat; 
            }
            set
            {
                _IgnoreDisplayFormat = value; 
            }
        }

        private DomAccessFlags _AccessFlags = DomAccessFlags.None;
        /// <summary>
        /// 访问标记
        /// </summary>
       // [DCSoft.Common.DCPublishAPI]
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public DomAccessFlags AccessFlags
        {
            get
            {
                return _AccessFlags; 
            }
            set
            {
                _AccessFlags = value; 
            }
        }


        private bool _UpdateContent = true;
        /// <summary>
        /// 文本文档视图
        /// </summary>
       // [DCSoft.Common.DCPublishAPI]
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public bool UpdateContent
        {
            get
            {
                return _UpdateContent; 
            }
            set
            {
                _UpdateContent = value; 
            }
        }

        private bool _LogUndo = true;
        /// <summary>
        /// 是否记录撤销操作信息
        /// </summary>
      //  [DCSoft.Common.DCPublishAPI]
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public bool LogUndo
        {
            get
            {
                return _LogUndo; 
            }
            set
            {
                _LogUndo = value; 
            }
        }

        private bool _FocusContainer = false;
        /// <summary>
        /// 让容器获得输入焦点
        /// </summary>
      //  [DCSoft.Common.DCPublishAPI]
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public bool FocusContainer
        {
            get
            {
                return _FocusContainer; 
            }
            set
            {
                _FocusContainer = value; 
            }
        }

        private bool _RaiseDocumentContentChangedEvent = true;
        /// <summary>
        /// 是否触发文档的DocumentContentChanged事件
        /// </summary>
        //[DCSoft.Common.DCPublishAPI]
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public bool RaiseDocumentContentChangedEvent
        {
            get
            {
                return _RaiseDocumentContentChangedEvent; 
            }
            set
            {
                _RaiseDocumentContentChangedEvent = value; 
            }
        }

        //private bool _AutoEndLogUndo = true;
        ///// <summary>
        ///// 自动执行EndLogUndo
        ///// </summary>
        //public bool AutoEndLogUndo
        //{
        //    get
        //    {
        //        return _AutoEndLogUndo; 
        //    }
        //    set
        //    {
        //        _AutoEndLogUndo = value; 
        //    }
        //}

        private DocumentContentStyle _NewTextStyle = null;
        /// <summary>
        /// 新文本的样式
        /// </summary>
      //  [DCSoft.Common.DCPublishAPI]
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public DocumentContentStyle NewTextStyle
        {
            get
            {
                return _NewTextStyle; 
            }
            set
            {
                _NewTextStyle = value; 
            }
        }

        private DocumentContentStyle _NewParagraphStyle = null;
        /// <summary>
        /// 新的段落样式
        /// </summary>
        //[DCSoft.Common.DCPublishAPI]
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public DocumentContentStyle NewParagraphStyle
        {
            get
            {
                return _NewParagraphStyle; 
            }
            set
            {
                _NewParagraphStyle = value; 
            }
        }

        private bool _RaiseEvent = true;
        /// <summary>
        /// 是否触发事件
        /// </summary>
      //  [DCSoft.Common.DCPublishAPI]
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public bool RaiseEvent
        {
            get
            {
                return _RaiseEvent; 
            }
            set
            {
                _RaiseEvent = value; 
            }
        }

        private bool _ShowUI = false;
        /// <summary>
        /// 是否显示用户界面
        /// </summary>
        //[DCSoft.Common.DCPublishAPI]
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
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
    }
}
