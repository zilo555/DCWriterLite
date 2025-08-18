using System;
using System.Collections.Generic;
using System.Text;

namespace DCSoft.Writer.Dom
{
    /// <summary>
    /// 遍历文档元素事件委托类型 
    /// </summary>
    /// <param name="eventSender"></param>
    /// <param name="args"></param>
    [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true )]
    public delegate void ElementEnumerateEventHandler(
        object eventSender ,
        ElementEnumerateEventArgs args );

    /// <summary>
    /// 遍历元素事件参数
    /// </summary>
    [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = false  )]
    public class ElementEnumerateEventArgs   
    {
        /// <summary>
        /// 初始化对象
        /// </summary>
        public ElementEnumerateEventArgs()
        {
        }

        private bool _ReverseMode = false;
        /// <summary>
        /// 逆向遍历模式
        /// </summary>
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public bool ReverseMode
        {
            get
            {
                return _ReverseMode; 
            }
            set
            {
                _ReverseMode = value; 
            }
        }

        private object _Parameter = null;
        /// <summary>
        /// 参数
        /// </summary>
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public object Parameter
        {
            get
            {
                return _Parameter; 
            }
            set
            {
                _Parameter = value; 
            }
        }

        internal bool _Cancel = false;
        /// <summary>
        /// 取消操作
        /// </summary>
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public bool Cancel
        {
            get
            {
                return _Cancel; 
            }
            set
            {
                _Cancel = value; 
            }
        }

        internal bool _CancelChild = false;
        /// <summary>
        /// 取消遍历子孙元素
        /// </summary>
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public bool CancelChild
        {
            get
            {
                return _CancelChild; 
            }
            set
            {
                _CancelChild = value; 
            }
        }

        internal DomContainerElement _Parent = null;
        /// <summary>
        /// 父节点
        /// </summary>
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public DomContainerElement Parent
        {
            get
            {
                return _Parent; 
            }
            //set { _Parent = value; }
        }

        internal DomElement _Element = null;
        /// <summary>
        /// 当前处理的元素
        /// </summary>
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public DomElement Element
        {
            get
            {
                return _Element; 
            }
            //set { _Element = value; }
        }

        internal bool _ExcludeCharElement = false;
        /// <summary>
        /// 忽略字符元素
        /// </summary>
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public bool ExcludeCharElement
        {
            get
            {
                return _ExcludeCharElement; 
            }
            set
            {
                _ExcludeCharElement = value; 
            }
        }

        internal bool _ExcludeParagraphFlag = false;
        /// <summary>
        /// 忽略段落符号元素
        /// </summary>
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public bool ExcludeParagraphFlag
        {
            get
            {
                return _ExcludeParagraphFlag; 
            }
            set
            {
                _ExcludeParagraphFlag = value; 
            }
        }

        private int _HandlerCount = 0;
        /// <summary>
        /// 累加执行次数
        /// </summary>
        internal void IncreaseHandlerCount()
        {
            this._HandlerCount++;
        }
        /// <summary>
        /// 累计执行的次数
        /// </summary>
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public int HandlerCount
        {
            get
            {
                return _HandlerCount; 
            }
        }
    }
}
