using System;
using System.Collections.Generic;
using System.Text;

namespace DCSoft.Writer.Dom
{
    /// <summary>
    /// 文档元素状态检测事件参数
    /// </summary>
    [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = false )]
    public class ElementStateDetectEventArgs
    {
        /// <summary>
        /// 输出对象
        /// </summary>
        /// <param name="element">文档元素对象</param>
        /// <param name="flags">标记</param>
        public ElementStateDetectEventArgs(DomElement element, DomAccessFlags flags)
        {
            _Element = element;
            _Flags = flags;
        }

        /// <summary>
        /// 初始化对象
        /// </summary>
        /// <param name="element">文档元素对象</param>
        public ElementStateDetectEventArgs(DomElement element)
        {
            _Element = element;
        }

        private bool _ForContent = false;
        /// <summary>
        /// 为修改内容而执行判断
        /// </summary>
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true )]
        public bool ForContent
        {
            get { return _ForContent; }
            set { _ForContent = value; }
        }
        private bool _ResetLastContentProtectedInfo = false;

        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public bool ResetLastContentProtectedInfo
        {
            get
            {
                return _ResetLastContentProtectedInfo;
            }
            set
            {
                _ResetLastContentProtectedInfo = value;
            }
        }
        private DomAccessFlags _Flags = DomAccessFlags.Normal;
        /// <summary>
        /// 标记
        /// </summary>
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public DomAccessFlags Flags
        {
            get { return _Flags; }
            set { _Flags = value; }
        }

        private DomElement _Element = null;
        /// <summary>
        /// 文档元素对象
        /// </summary>
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public DomElement Element
        {
            get { return _Element; }
            set { _Element = value; }
        }

        private bool _SetMessage = true;
        /// <summary>
        /// 设置消息
        /// </summary>
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public bool SetMessage
        {
            get { return _SetMessage; }
            set { _SetMessage = value; }
        }

        private string _Message = null;
        /// <summary>
        /// 消息
        /// </summary>
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public string Message
        {
            get { return _Message; }
            set { _Message = value; }
        }

        private bool _Result = true;
        /// <summary>
        /// 检测结果
        /// </summary>
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public bool Result
        {
            get
            {
                return _Result; 
            }
            set
            {
                //if (_Result != value)
                {
                    _Result = value;
                }
            }
        }

        private ContentProtectedReason _ProtectedReason = ContentProtectedReason.None;
        /// <summary>
        /// 保护理由
        /// </summary>
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public ContentProtectedReason ProtectedReason
        {
            get
            {
                return _ProtectedReason;
            }
            set
            {
                _ProtectedReason = value;
            }
        }

        private ElementStateDetectEventArgs _ParentStateResult = null;
        public ElementStateDetectEventArgs ParentStateResult
        {
            get
            {
                return _ParentStateResult;
            }
            set
            {
                _ParentStateResult = value;
            }
        }

        private bool _CacheParentStateResult = false;
        /// <summary>
        /// 是否缓存父元素状态信息
        /// </summary>
        public bool CacheParentStateResult
        {
            get
            {
                return _CacheParentStateResult;
            }
            set
            {
                _CacheParentStateResult = value;
            }
        }
 

        public ElementStateDetectEventArgs Clone()
        {
            return (ElementStateDetectEventArgs)this.MemberwiseClone();
        }
    }
}
