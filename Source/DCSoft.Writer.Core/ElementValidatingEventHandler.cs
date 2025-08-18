using System;
using System.Collections.Generic;
using System.Text;
using DCSoft.Writer.Dom;
using System.Runtime.InteropServices;
using DCSoft.Writer.Controls;
using DCSoft.Common ;


namespace DCSoft.Writer
{

    /// <summary>
    /// 文档元素内容验证事件委托类型
    /// </summary>
    /// <param name="eventSender">参数</param>
    /// <param name="args">参数</param>
    public delegate void ElementValidatingEventHandler(
        object eventSender,
        ElementValidatingEventArgs args);

    /// <summary>
    /// 文档元素内容验证中事件参数
    /// </summary>
    [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = false )]
    public partial class ElementValidatingEventArgs : ElementEventArgs 
    {
        /// <summary>
        /// 初始化对象
        /// </summary>
        /// <param name="element">文档元素对象</param>
         
        public ElementValidatingEventArgs(DomElement element)
            : base(element)
        {
        }

        private ElementValidatingState _ResultState = ElementValidatingState.Success;
        /// <summary>
        /// 校验状态
        /// </summary>
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public ElementValidatingState ResultState
        {
            [JSInvokable]
            get
            {
                return _ResultState;
            }
            [JSInvokable]
            set
            {
                _ResultState = value;
            }
        }

        private ValueValidateLevel _ResultLevel = ValueValidateLevel.Error;
        /// <summary>
        /// 校验结果等级
        /// </summary>
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public ValueValidateLevel ResultLevel
        {
            [JSInvokable]
            get
            {
                return _ResultLevel; 
            }
            [JSInvokable]
            set
            {
                _ResultLevel = value; 
            }
        }

        private string _Message = null;
        /// <summary>
        /// 验证结果信息 
        /// </summary>
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public string Message
        {
            [JSInvokable]
            get
            {
                return _Message;
            }
            [JSInvokable]
            set
            {
                _Message = value;
            }
        }

        private bool _Cancel = false;
        /// <summary>
        /// 取消后续事件
        /// </summary>
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public bool Cancel
        {
            [JSInvokable]
            get
            {
                return _Cancel; 
            }
            [JSInvokable]
            set
            {
                _Cancel = value; 
            }
        }
        
    }



    /// <summary>
    /// 文档元素校验结果
    /// </summary>
         [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
    [System.Text.Json.Serialization.JsonConverter(typeof(System.Text.Json.Serialization.JsonStringEnumConverter))]
    public enum ElementValidatingState
    {
        /// <summary>
        /// 校验成功，无需进行后续的校验
        /// </summary>
        Success,
        /// <summary>
        /// 校验通过，还需进行后续校验
        /// </summary>
        Pass,
        /// <summary>
        /// 校验失败
        /// </summary>
        Fail
    }
     
}
