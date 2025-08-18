using System;
using System.Collections.Generic;
using System.Text;
using DCSoft.Writer.Dom;
using DCSoft.Writer.Controls;
using DCSoft.Writer.Data;

namespace DCSoft.Writer
{

    /// <summary>
    /// 编辑器事件参数类型
    /// </summary>
    /// <remarks>编制 袁永福</remarks>
    [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = false)]
    [System.Serializable]
    //[DCSoft.Common.DCPublishAPI]
    public partial class WriterEventArgs : EventArgs , IDisposable
    {
        ///// <summary>
        ///// 无参数的构造函数
        ///// </summary>
        //protected WriterEventArgs()
        //{
        //}
 
        /// <summary>
        /// 初始化对象
        /// </summary>
        /// <param name="ctl"></param>
        /// <param name="document"></param>
        /// <param name="element"></param>
         
        public WriterEventArgs(
            WriterControl ctl, 
            DomDocument document,
            DomElement element )
        {
            _WriterControl = ctl;
            _Document = document;
            _Element = element;
        }

        /// <summary>
        /// 销毁对象
        /// </summary>
        public virtual void Dispose()
        {
            this._WriterControl = null;
            this._Document = null;
            this._Element = null;
            //if(this._TargetElement != null )
            //{
            //    this._TargetElement.Dispose();
            //    this._TargetElement = null;
            //}
        }

        [NonSerialized]
        private WriterControl _WriterControl = null;
        /// <summary>
        /// 编辑器控件
        /// </summary>
        [System.Text.Json.Serialization.JsonIgnore]
        public WriterControl WriterControl
        {
            get 
            {
                return _WriterControl; 
            }
        }
 
        [NonSerialized]
        private DomDocument _Document = null;
        /// <summary>
        /// 文档对象
        /// </summary>
        [System.Text.Json.Serialization.JsonIgnore]
        public DomDocument Document
        {
            get
            {
                return _Document; 
            }
        }

        [NonSerialized]
        private DomElement _Element = null;
        /// <summary>
        /// 文档元素对象
        /// </summary>
        [System.Text.Json.Serialization.JsonIgnore]
        public DomElement Element
        {
            get
            {
                return _Element; 
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
                if(this._Element == null )
                {
                    return 0;
                }
                else
                {
                    return this._Element.GetHashCode();
                }
            }
        }
    }
}
