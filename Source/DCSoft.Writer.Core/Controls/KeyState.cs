using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel ;
using System.Windows.Forms;

namespace DCSoft.Writer.Controls
{
    /// <summary>
    /// 按键状态
    /// </summary>     
    internal class KeyState : ICloneable
    {
        /// <summary>
        /// 初始化对象
        /// </summary>
        public KeyState()
        {
        }

        /// <summary>
        /// 初始化对象
        /// </summary>
        /// <param name="key">按键值</param>
        public KeyState(System.Windows.Forms.Keys key)
        {
            _Control = ((key & Keys.Control) == Keys.Control);
            _Alt = ((key & Keys.Alt) == Keys.Alt);
            _Shift = ((key & Keys.Shift) == Keys.Shift);
            _Key = ( key & Keys.KeyCode );
        }

        private bool _Control = false;
        /// <summary>
        /// Control键是否按下
        /// </summary>
        [System.ComponentModel.DefaultValue( false )]
        public bool Control
        {
            get { return _Control; }
            set { _Control = value; }
        }

        private bool _Alt = false;
        /// <summary>
        /// 是否按下Alt键
        /// </summary>
        [DefaultValue( false )]
        public bool Alt
        {
            get { return _Alt; }
            set { _Alt = value; }
        }

        private bool _Shift = false;
        /// <summary>
        /// 是否按下Shift键
        /// </summary>
        [DefaultValue( false )]
        public bool Shift
        {
            get { return _Shift; }
            set { _Shift = value; }
        }

        private System.Windows.Forms.Keys _Key = System.Windows.Forms.Keys.None;
        /// <summary>
        /// 按键值
        /// </summary>
        [DefaultValue( System.Windows.Forms.Keys.None )]
        public System.Windows.Forms.Keys Key
        {
            get { return _Key; }
            set { _Key = value; }
        }
#if !RELEASE
        /// <summary>
        /// 返回表示对象的字符串
        /// </summary>
        /// <returns>字符串</returns>
        public override string ToString()
        {
            StringBuilder str = new StringBuilder();
            if (_Control)
            {
                str.Append("Control ");
            }
            if (_Shift)
            {
                str.Append("Shift ");
            }
            if (_Alt)
            {
                str.Append("Alt ");
            }
            str.Append(_Key.ToString());
            return str.ToString();
        }
#endif
        /// <summary>
        /// 复制对象
        /// </summary>
        /// <returns>复制品</returns>
        object ICloneable.Clone()
        {
            KeyState result = new KeyState();
            result._Alt = this._Alt;
            result._Control = this._Control;
            result._Key = this._Key;
            result._Shift = this._Shift;
            return result;
        }
    }
}
