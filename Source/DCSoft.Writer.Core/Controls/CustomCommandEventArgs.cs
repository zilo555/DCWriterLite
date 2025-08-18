#if ! LightWeight

using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace DCSoft.Writer.Controls
{
    /// <summary>
    /// 自定义命令事件参数
    /// </summary>
     
    [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = false  )]
    //[DCSoft.Common.DCPublishAPI]
    public partial class CustomCommandEventArgs
    {
        /// <summary>
        /// 初始化对象
        /// </summary>
        /// <param name="txt">命令文本</param>
        /// <param name="cmd">命令名称</param>
         
        public CustomCommandEventArgs(string txt , string cmd)
        {
            this._Text = txt;
            _CommandName = cmd;
        }

        private string _Text = null;
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public string Text
        {
            get
            {
                return this._Text;
            }
        }

        private string _CommandName = null;
        /// <summary>
        /// 命令名称
        /// </summary>
        //[DCSoft.Common.DCPublishAPI]
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public string CommandName
        {
            get 
            {
                return _CommandName; 
            }
        }
    }
}
#endif