
using System;
using System.Collections.Generic;
using System.Text;
using DCSoft.Writer.Dom;
using System.Runtime.InteropServices;
using DCSoft.Writer.Controls;
using System.Drawing;
using System.Windows.Forms;
namespace DCSoft.Writer
{
    /// <summary>
    /// 文档元素键盘事件参数
    /// </summary>
    [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = false )]
    //[DCSoft.Common.DCPublishAPI]
    public partial class ElementKeyEventArgs : ElementEventArgs
    {
        /// <summary>
        /// 初始化对象
        /// </summary>
        /// <param name="args"></param>
        /// <param name="element"></param>
         
        public ElementKeyEventArgs(DocumentEventArgs args, DomElement element)
            : base(element)
        {
            this._Alt = args.AltKey;
            this._Shift = args.ShiftKey;
            this._Control = args.CtlKey;
            this._KeyCode = args.KeyCode;
            this._KeyChar = args.KeyChar;
        }


        private bool _Alt = false;
        /// <summary>
        /// Alt键状态
        /// </summary>
        //[DCSoft.Common.DCPublishAPI]
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public bool Alt
        {
            [JSInvokable]
            get { return _Alt; }
            set { _Alt = value; }
        }

        private bool _Control = false;
        /// <summary>
        /// Control键状态
        /// </summary>
        //[DCSoft.Common.DCPublishAPI]
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public bool Control
        {
            [JSInvokable]
            get { return _Control; }
            set { _Control = value; }
        }


        private bool _Shift = false;
        /// <summary>
        /// Shift键状态
        /// </summary>
        //[DCSoft.Common.DCPublishAPI]
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public bool Shift
        {
            [JSInvokable]
            get { return _Shift; }
            set { _Shift = value; }
        }
        private System.Windows.Forms.Keys _KeyCode = Keys.None;
        /// <summary>
        /// 按键值
        /// </summary>
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public System.Windows.Forms.Keys KeyCode
        {
            [JSInvokable]
            get { return _KeyCode; }
            set { _KeyCode = value; }
        }

        private char _KeyChar = Char.MinValue;
        /// <summary>
        /// 按键字符值
        /// </summary>
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public char KeyChar
        {
            [JSInvokable]
            get
            {
                return _KeyChar;
            }
        }

    }
     
}
