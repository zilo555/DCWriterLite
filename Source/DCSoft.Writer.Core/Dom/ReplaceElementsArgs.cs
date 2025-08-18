using System;
using System.Collections.Generic;
using System.Text;

namespace DCSoft.Writer.Dom
{
    /// <summary>
    /// 执行ReplaceElements函数使用的参数
    /// </summary>
    public class ReplaceElementsArgs : EventArgs
    {
        /// <summary>
        /// 初始化对象
        /// </summary>
        public ReplaceElementsArgs()
        {
        }

        /// <summary>
        /// 初始化对象
        /// </summary>
        /// <param name="container">要替换子元素的容器元素对象</param>
        /// <param name="startIndex">替换区域的开始序号</param>
        /// <param name="deleteLength">要删除的子元素的个数</param>
        /// <param name="newElements">要插入的新元素列表</param>
        /// <param name="logUndo">是否记录撤销操作信息</param>
        /// <param name="updateContent">是否更新文档视图</param>
        /// <param name="raiseEvent">是否触发事件</param>
        public ReplaceElementsArgs(
            DomContainerElement container,
            int startIndex,
            int deleteLength,
            DomElementList newElements,
            bool logUndo,
            bool updateContent,
            bool raiseEvent)
        {
            _Container = container;
            _StartIndex = startIndex;
            _DeleteLength = deleteLength;
            _NewElements = newElements;
            _LogUndo = logUndo;
            _UpdateContent = updateContent;
            _RaiseEvent = raiseEvent;
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
        public string NewInnerValue
        {
            get
            {
                return this._NewInnerValue; 
            }
            set
            {
                this._NewInnerValue = value;
            }
        }
        private bool _PreserveSelection = false;
        /// <summary>
        /// 尽量保留当前选择状态
        /// </summary>
        public bool PreserveSelection
        {
            get
            {
                return this._PreserveSelection;
            }
            set
            {
                this._PreserveSelection = value;
            }
        }
        //private bool _ExecuteCopySource = false;
        ///// <summary>
        ///// 是否执行内容复制操作
        ///// </summary>
        //public bool ExecuteCopySource1
        //{
        //    get { return _ExecuteCopySource; }
        //    set { _ExecuteCopySource = value; }
        //}

        private bool _DetectOnly = false;
        /// <summary>
        /// 仅仅是检测模式,并不真的执行替换元素操作
        /// </summary>
        internal bool DetectOnly
        {
            get
            {
                return _DetectOnly;
            }
            set
            {
                _DetectOnly = value;
            }
        }

        private float _TickSpan = 0f;
        /// <summary>
        /// 操作执行销毁的时刻数
        /// </summary>
        public float TickSpan
        {
            get
            {
                return _TickSpan;
            }
            set
            {
                _TickSpan = value;
            }
        }

        private ContentChangedEventSource _EventSource = ContentChangedEventSource.Default;
        /// <summary>
        /// 事件来源
        /// </summary>
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

        private DomContainerElement _Container = null;
        /// <summary>
        /// 容器元素对象
        /// </summary>
        public DomContainerElement Container
        {
            get { return _Container; }
            set { _Container = value; }
        }

        private int _StartIndex = 0;
        /// <summary>
        /// 操作的开始区域序号
        /// </summary>
        public int StartIndex
        {
            get { return _StartIndex; }
            set { _StartIndex = value; }
        }
        private int _DeleteLength = 0;
        /// <summary>
        /// 要删除的区域的长度
        /// </summary>
        public int DeleteLength
        {
            get { return _DeleteLength; }
            set { _DeleteLength = value; }
        }

        private DomElementList _NewElements = null;
        /// <summary>
        /// 新插入的元素对象列表
        /// </summary>
        public DomElementList NewElements
        {
            get { return _NewElements; }
            set { _NewElements = value; }
        }

        //private bool _AutoRemoveLastParagraphFlagInNewElements = false ;
        ///// <summary>
        ///// 是否自动删除新元素中最后一个段落符号元素。
        ///// </summary>
        //public bool AutoRemoveLastParagraphFlagInNewElements
        //{
        //    get
        //    {
        //        return _AutoRemoveLastParagraphFlagInNewElements;
        //    }
        //    set
        //    {
        //        _AutoRemoveLastParagraphFlagInNewElements
        //    }
        //}
        private bool _AutoFixNewElements = true;
        /// <summary>
        /// 自动过滤新插入的元素列表
        /// </summary>
        public bool AutoFixNewElements
        {
            get
            {
                return _AutoFixNewElements;
            }
            set
            {
                _AutoFixNewElements = value;
            }
        }

        private bool _LogUndo = true;
        /// <summary>
        /// 是否记录撤销操作信息
        /// </summary>
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
        private bool _UpdateContent = true;
        /// <summary>
        /// 是否更新文档视图
        /// </summary>
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

        private bool _RaiseEvent = true;
        /// <summary>
        /// 是否触发相关事件
        /// </summary>
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

        private DomAccessFlags _AccessFlags = DomAccessFlags.Normal;
        /// <summary>
        /// 访问行为标记
        /// </summary>
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

        private bool _ChangeSelection = true;
        /// <summary>
        /// 是否修改文档文档的选择状态
        /// </summary>
        public bool ChangeSelection
        {
            get
            {
                return _ChangeSelection;
            }
            set
            {
                _ChangeSelection = value;
            }
        }

        private bool _FocusContainer = false;
        /// <summary>
        /// 容器获得焦点
        /// </summary>
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

        private EventHandler _AfterReplaceDOM = null;
        /// <summary>
        /// 替换文档元素后事件
        /// </summary>
        public EventHandler AfterReplaceDOM
        {
            get
            {
                return _AfterReplaceDOM;
            }
            set
            {
                _AfterReplaceDOM = value;
            }
        }

        private object _Tag = null;
        /// <summary>
        /// 额外的参数
        /// </summary>
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

        private int _Result = 0;
        /// <summary>
        /// 执行结果
        /// </summary>
        public int Result
        {
            get
            {
                return _Result;
            }
            set
            {
                _Result = value;
            }
        }
    }
}
