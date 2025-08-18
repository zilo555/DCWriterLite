using System;
using System.Collections.Generic;
using System.Text;
using DCSoft.Writer.Dom ;
//using Microsoft.JSInterop;

namespace DCSoft.Writer.Controls
{
    /// <summary>
    /// 提示内容保护事件委托对象
    /// </summary>
    /// <param name="eventSender">事件发起者</param>
    /// <param name="args">参数</param>
    [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true )]
    public delegate void PromptProtectedContentEventHandler(
        object eventSender ,
        PromptProtectedContentEventArgs args );

    /// <summary>
    /// 提示内容保护事件参数
    /// </summary>
     
    [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = false )]
     
    public partial class PromptProtectedContentEventArgs : WriterEventArgs
    {
        /// <summary>
        /// 初始化对象
        /// </summary>
        /// <param name="ctl"></param>
        /// <param name="doc"></param>
        /// <param name="ele"></param>
        /// <param name="msg"></param>
        /// <param name="mode">模式</param>
        public PromptProtectedContentEventArgs(
            WriterControl ctl ,
            DomDocument doc , 
            DomElement ele , 
            string msg , 
            PromptProtectedContentMode mode )
            :base( ctl , doc , ele )
        {
            this._Message = msg;
            this._PromptMode = mode;
        }


        private string _Message = null;
        /// <summary>
        /// 提示信息
        /// </summary>
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public string Message
        {
            [JSInvokable]
            get
            {
                return _Message; 
            }
            //set { _Message = value; }
        }

        private PromptProtectedContentMode _PromptMode = PromptProtectedContentMode.Details;
        /// <summary>
        /// 提示模式
        /// </summary>
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public PromptProtectedContentMode PromptMode
        {
            [JSInvokable]
            get 
            { 
                return _PromptMode; 
            }
            //set { _PromptMode = value; }
        }
        private bool _Handled = false;
        /// <summary>
        /// 事件被处理了
        /// </summary>
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
    }
}
