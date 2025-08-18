using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms ;
using DCSoft.Writer.Controls ;
using DCSoft.Writer.Dom ;
using System.Runtime.InteropServices;
using System.ComponentModel ;
// // 

namespace DCSoft.Writer
{
    /// <summary>
    /// 编辑器鼠标事件参数
    /// </summary>
    [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = false)]
    //[DCSoft.Common.DCPublishAPI]
    public partial class WriterMouseEventArgs  
    {
        /// <summary>
        /// 初始化对象
        /// </summary>
        /// <param name="ctl"></param>
        /// <param name="args"></param>
         
        public WriterMouseEventArgs(WriterControl ctl, MouseEventArgs args)
        {
            this._WriterControl = ctl;
            this._Document = ctl.Document;
            this._X = args.X;
            this._Y = args.Y;
            this._Clicks = args.Clicks;
            this._Button = args.Button;
            this._Delta = args.Delta;
            this._HoverElement = ctl.HoverElement;
            if (ctl != null)
            {
                Point p = new Point(args.X, args.Y);
                //p = ctl.GetInnerViewControl().PointToScreen(p);
                this._ScreenX = p.X;
                this._ScreenY = p.Y;
            }
        }

        /// <summary>
        /// 初始化对象
        /// </summary>
        /// <param name="ctl"></param>
        /// <param name="args"></param>
        public WriterMouseEventArgs(WriterControl ctl, int x  ,int y , int clicks , MouseButtons button , int delta )
        {
            this._WriterControl = ctl;
            this._Document = ctl.Document;
            this._X = x;
            this._Y = y;
            this._Clicks = clicks;
            this._Button = button;
            this._Delta = delta;
            this._HoverElement = ctl.HoverElement;
            if (ctl != null)
            {
                Point p = new Point(x, y);
                //p = ctl.GetInnerViewControl().PointToScreen(p);
                this._ScreenX = p.X;
                this._ScreenY = p.Y;
            }
        }

        private bool _Handled = false;
        /// <summary>
        /// 事件已经被处理了。
        /// </summary>
        //[DCSoft.Common.DCPublishAPI]
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

        private WriterControl _WriterControl = null;
        /// <summary>
        /// 编辑器控件对象
        /// </summary>
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        [System.Text.Json.Serialization.JsonIgnore]
        public WriterControl WriterControl
        {
            get
            {
                return _WriterControl;
            }
        }

        private DomDocument _Document = null;
        /// <summary>
        /// 文档对象
        /// </summary>
        //[DCSoft.Common.DCPublishAPI]
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        [System.Text.Json.Serialization.JsonIgnore]
        public DomDocument Document
        {
            get
            {
                return _Document;
            }
        }

        private int _ScreenX = 0;
        /// <summary>
        /// 鼠标光标在屏幕中的X坐标
        /// </summary>
        //[DCSoft.Common.DCPublishAPI]
        public int ScreenX
        {
            get
            {
                return _ScreenX; 
            }
        }

        private int _ScreenY = 0;
        /// <summary>
        /// 鼠标光标在屏幕中的X坐标
        /// </summary>
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public int ScreenY
        {
            get
            {
                return _ScreenY;
            }
        }


        private DomElement _HoverElement = null;
        /// <summary>
        /// 鼠标悬停下的文档元素对象
        /// </summary>
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        [System.Text.Json.Serialization.JsonIgnore]
        public DomElement HoverElement
        {
            get
            {
                return _HoverElement; 
            }
        }

        private MouseButtons _Button = MouseButtons.None;
        /// <summary>
        /// 鼠标按钮
        /// </summary>
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public MouseButtons Button
        {
            [JSInvokable]
            get
            {
                return _Button;
            }
        }

        /// <summary>
        /// 判断是否是有鼠标左键
        /// </summary>
        //[DCSoft.Common.DCPublishAPI]
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public bool HasLeftButton
        {
            [JSInvokable]
            get
            {
                return (_Button & MouseButtons.Left) == MouseButtons.Left;
            }
        }

        /// <summary>
        /// 判断是否是有鼠标右键
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
        /// <summary>
        /// 按钮值
        /// </summary>
       //////[Browsable( false )]
        //[EditorBrowsable( EditorBrowsableState.Never )]
        //[DCSoft.Common.DCPublishAPI]
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public int ButtonValue
        {
            get
            {
                return (int)_Button;
            }
        }

        private int _Delta = 0;
        /// <summary>
        /// 滚轮值
        /// </summary>
        //[DCSoft.Common.DCPublishAPI]
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public int Delta
        {
            [JSInvokable]
            get
            {
                return _Delta;
            }
        }
        private int _Clicks = 0;
        /// <summary>
        /// 鼠标点击次数
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
        }
        private int _X = 0;
        /// <summary>
        /// 鼠标光标X坐标值
        /// </summary>
        //[DCSoft.Common.DCPublishAPI]
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public int X
        {
            [JSInvokable]
            get
            {
                return _X;
            }
        }

        private int _Y = 0;
        /// <summary>
        /// 鼠标光标Y坐标值
        /// </summary>
        //[DCSoft.Common.DCPublishAPI]
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public int Y
        {
            [JSInvokable]
            get
            {
                return _Y;
            }
        }
    }
}
