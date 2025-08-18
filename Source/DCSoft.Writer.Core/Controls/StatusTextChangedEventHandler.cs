using System;
using System.Collections.Generic;
using System.Text;
using DCSoft.Common;

namespace DCSoft.Writer.Controls
{
    /// <summary>
    /// 状态栏文本发生改变事件参数
    /// </summary>
    [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = false )]
    //[DCSoft.Common.DCPublishAPI]
    public partial class StatusTextChangedEventArgs  : EventArgs
    {
        /// <summary>
        /// 初始化对象
        /// </summary>
        /// <param name="ctl"></param>
        /// <param name="statusText"></param>
         
        public StatusTextChangedEventArgs(WriterControl ctl, string statusText)
        {
            _WriterControl = ctl;
            _StatusText = statusText;
        }

        private WriterControl _WriterControl = null;
        /// <summary>
        /// 编辑器控件对象
        /// </summary>
        ////[DCPublishAPI]
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        [System.Text.Json.Serialization.JsonIgnore]
        public WriterControl WriterControl
        {
            get
            {
                return _WriterControl; 
            }
        }

        private string _StatusText = null;
        /// <summary>
        /// 状态栏文本
        /// </summary>
        ////[DCPublishAPI]
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public string StatusText
        {
            get
            {
                return _StatusText; 
            }
        }
    }
}
