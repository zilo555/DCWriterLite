using System;
using System.Collections.Generic;
using System.Text;
using DCSoft.Writer.Dom;
using System.Runtime.InteropServices;
using DCSoft.Writer.Controls;
using System.Windows.Forms;

namespace DCSoft.Writer
{

    /// <summary>
    /// 元素鼠标事件参数类型
    /// </summary>
    [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = false )]
    public partial class ElementMouseEventArgs : ElementEventArgs
    {
        /// <summary>
        /// 初始化对象
        /// </summary>
        /// <param name="args"></param>
        /// <param name="element"></param>
        public ElementMouseEventArgs(DocumentEventArgs args, DomElement element)
            : base(element)
        {
            this._Button = args.Button;
            this._Clicks = args.MouseClicks;
            this._Delta = args.WheelDelta;
            this._DocumentX = args.X;
            this._DocumentY = args.Y;
            if (element != null)
            {
                RuntimeDocumentContentStyle rs = element.RuntimeStyle;
                RectangleF bounds = element.GetAbsBounds();
                this._ElementClientX = this._DocumentX - bounds.X - rs.PaddingLeft;
                this._ElementClientY = this._DocumentY - bounds.Y - rs.PaddingTop;
            }
        }

        private MouseButtons _Button = MouseButtons.None;
        /// <summary>
        /// 鼠标按键值
        /// </summary>
       ////[System.ComponentModel.Browsable(false)]
        //[DCSoft.Common.DCPublishAPI]
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public MouseButtons Button
        {
            [JSInvokable]
            get
            {
                return _Button;
            }
            set
            {
                _Button = value;
            }
        }
        /// <summary>
        /// 用户是否按下左按钮
        /// </summary>
        //[DCSoft.Common.DCPublishAPI]
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public bool HasLeftButton
        {
            [JSInvokable]
            get
            {
                return ( _Button & MouseButtons.Left ) == MouseButtons.Left ;
            }
        }

        /// <summary>
        /// 用户是否按下右按钮
        /// </summary>
        //[DCSoft.Common.DCPublishAPI]
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public bool HasRightButton
        {
            [JSInvokable]
            get
            {
                return (_Button & MouseButtons.Right) == MouseButtons.Right;
            }
        }

        private int _Clicks = 0;
        /// <summary>
        /// 按键点击次数
        /// </summary>
        //[DCSoft.Common.DCPublishAPI]
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public int Clicks
        {
            [JSInvokable]
            get
            {
                return _Clicks;
            }
            set
            {
                _Clicks = value;
            }
        }

        private int _Delta = 0;
        /// <summary>
        /// 鼠标滚轮计数
        /// </summary>
        //[DCSoft.Common.DCPublishAPI]
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public int Delta
        {
            [JSInvokable]
            get { return _Delta; }
            set { _Delta = value; }
        }

        private float _DocumentX = 0;
        /// <summary>
        /// 鼠标光标在文档中的X坐标
        /// </summary>
        //[DCSoft.Common.DCPublishAPI]
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public float DocumentX
        {
            [JSInvokable]
            get
            {
                return _DocumentX;
            }
            set
            {
                _DocumentX = value;
            }
        }

        private float _DocumentY = 0;
        /// <summary>
        /// 鼠标光标在文档中的Y坐标
        /// </summary>
        //[DCSoft.Common.DCPublishAPI]
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public float DocumentY
        {
            [JSInvokable]
            get
            {
                return _DocumentY;
            }
            set
            {
                _DocumentY = value;
            }
        }

        private float _ElementClientX = 0;
        /// <summary>
        /// 鼠标光标在元素客户区中的X坐标
        /// </summary>
        //[DCSoft.Common.DCPublishAPI]
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public float ElementClientX
        {
            [JSInvokable]
            get
            {
                return _ElementClientX;
            }
            set
            {
                _ElementClientX = value;
            }
        }

        private float _ElementClientY = 0;
        /// <summary>
        /// 鼠标光标在元素客户区中的Y坐标
        /// </summary>
        //[DCSoft.Common.DCPublishAPI]
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public float ElementClientY
        {
            [JSInvokable]
            get
            {
                return _ElementClientY;
            }
            set
            {
                _ElementClientY = value;
            }
        }
    }
     
}
