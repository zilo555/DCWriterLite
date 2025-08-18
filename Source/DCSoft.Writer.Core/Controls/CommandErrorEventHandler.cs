using System;
using System.Collections.Generic;
using System.Text;
using DCSoft.Writer.Dom ;
using System.Runtime.InteropServices;
using DCSoft.Common;

namespace DCSoft.Writer.Controls
{
    /// <summary>
    /// 编辑器名称错误事件参数
    /// </summary>
     
    [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = false   )]
    //[DCSoft.Common.DCPublishAPI]
    public partial class CommandErrorEventArgs  
    {
        /// <summary>
        /// 初始化对象
        /// </summary>
         
        public CommandErrorEventArgs()
        {
        }

        private WriterControl _WriterControl = null;
        /// <summary>
        /// 编辑器控件对象
        /// </summary>
        //[DCSoft.Common.DCPublishAPI]
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        [System.Text.Json.Serialization.JsonIgnore]
        public WriterControl WriterControl
        {
            get
            {
                return _WriterControl;}
            set
            {
                _WriterControl = value; 
            }
        }

        private DomDocument _Document = null;
        /// <summary>
        /// 正在处理的文档对象
        /// </summary>
        //[DCSoft.Common.DCPublishAPI]
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        [System.Text.Json.Serialization.JsonIgnore]
        public DomDocument Document
        {
            get { return _Document; }
            set { _Document = value; }
        }

        private string _CommandName = null;
        /// <summary>
        /// 命令名称
        /// </summary>
        //[DCSoft.Common.DCPublishAPI]
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public string CommandName
        {
            get { return _CommandName; }
            set { _CommandName = value; }
        }

        private object _CommmandParameter = null;
        /// <summary>
        /// 命令参数
        /// </summary>
        //[DCSoft.Common.DCPublishAPI]
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        [System.Text.Json.Serialization.JsonIgnore]
        public object CommmandParameter
        {
            get { return _CommmandParameter; }
            set { _CommmandParameter = value; }
        }

        private Exception _Exception = null;
        /// <summary>
        /// 异常对象
        /// </summary>
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        [System.Text.Json.Serialization.JsonIgnore]
        public Exception Exception
        {
            get { return _Exception; }
            set { _Exception = value; }
        }
        /// <summary>
        /// 错误消息
        /// </summary>
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public string ExceptionMessage
        {
            get
            {
                return this._Exception?.Message;
            }
        }
        private string _Message = null;
        /// <summary>
        /// 错误提示信息
        /// </summary>
        //[DCSoft.Common.DCPublishAPI]
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public string Message
        {
            get { return _Message; }
            set { _Message = value; }
        }

        private bool _Handled = false;
        /// <summary>
        /// 是否处理过了本事件
        /// </summary>
        //[DCSoft.Common.DCPublishAPI]
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        [System.Text.Json.Serialization.JsonIgnore]
        public bool Handled
        {
            get { return _Handled; }
            set { _Handled = value; }
        }
    }
}
