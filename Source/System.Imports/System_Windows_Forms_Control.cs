
using DCSoft.Drawing;
using DCSoft.Writer.Controls;
using DCSoft.Writer.Data;
//using Microsoft.Extensions.Options;
using System;
using System.Collections;
using System.Collections.Generic;
// // 

namespace System.Windows.Forms
{
    public static class SystemInformation
    {
        public static DCSystem_Drawing.Size DoubleClickSize
        {
            get { return new DCSystem_Drawing.Size(4, 4); }
        }
        public static int DoubleClickTime
        {
            get { return 500; }
        }
        public static DCSystem_Drawing.Size DragSize
        {
            get { return new DCSystem_Drawing.Size(4,4); }
        }
    }
    public enum DialogResult
    {
        None = 0,
        OK = 1,
        Cancel = 2,
        Abort = 3,
        Retry = 4,
        Ignore = 5,
        Yes = 6,
        No = 7
    }
    public static class MessageBox
    {
        public static string _DefaultCaption = "南京都昌信息科技有限公司提示";

        public static System.Windows.Forms.DialogResult Show(
            System.Windows.Forms.IWin32Window owner,
            string text,
            string caption,
            System.Windows.Forms.MessageBoxButtons buttons,
            System.Windows.Forms.MessageBoxIcon icon,
            System.Windows.Forms.MessageBoxDefaultButton defaultButton)
        {
            if (WASMEnvironment.JSProvider == null)
            {
                return DialogResult.No;
            }
            else
            {
                return WASMEnvironment.JSProvider.JS_ShowMessageBox(text, caption, buttons, icon, defaultButton);
            }
        }
        public static System.Windows.Forms.DialogResult Show(System.Windows.Forms.IWin32Window owner, string text)
        { 
            return Show(owner, text, _DefaultCaption, MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1); 
        }
        public static System.Windows.Forms.DialogResult Show(System.Windows.Forms.IWin32Window owner, string text, string caption)
        {
            return Show(owner, text, caption, MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
        }
        public static System.Windows.Forms.DialogResult Show(System.Windows.Forms.IWin32Window owner, string text, string caption, System.Windows.Forms.MessageBoxButtons buttons, System.Windows.Forms.MessageBoxIcon icon)
        {
            return Show(owner, text, caption, buttons, icon, MessageBoxDefaultButton.Button1);
        }
        public static System.Windows.Forms.DialogResult Show(string text)
        {
            return Show(null, text, _DefaultCaption, MessageBoxButtons.OK , MessageBoxIcon.Information , MessageBoxDefaultButton.Button1);
        }
    }
    public enum MessageBoxButtons
    {
        OK = 0,
        OKCancel = 1,
        AbortRetryIgnore = 2,
        YesNoCancel = 3,
        YesNo = 4,
        RetryCancel = 5
    }
    public enum MessageBoxDefaultButton
    {
        Button1 = 0,
        Button2 = 256,
        Button3 = 512
    }
    public enum MessageBoxIcon
    {
        None = 0,
        Hand = 16,
        Question = 32,
        Exclamation = 48,
        Asterisk = 64,
        Stop = 16,
        Error = 16,
        Warning = 48,
        Information = 64
    }
    [Flags]
    public enum MouseButtons
    {
        Left = 1048576,
        None = 0,
        Right = 2097152,
        Middle = 4194304,
        XButton1 = 8388608,
        XButton2 = 16777216
    }
    public class MouseEventArgs : EventArgs
    {
        public MouseEventArgs()
        { }

        public MouseEventArgs(System.Windows.Forms.MouseButtons button, int clicks, int x, int y, int delta)
        {
            this._Button = button;
            this._Clicks = clicks;
            this._X = x;
            this._Y = y;
            this._Delta = delta;
        }

        private System.Windows.Forms.MouseButtons _Button = MouseButtons.None;

        public System.Windows.Forms.MouseButtons Button
        {
            get { return this._Button; }
            set
            {
                this._Button = value;
            }
        }
        private int _Clicks = 0;
        public int Clicks
        {
            get { return this._Clicks; }
            set
            {
                this._Clicks = value;
            }
        }
        private int _Delta = 0;
        public int Delta
        {
            get { return this._Delta; }
            set
            {
                this._Delta = value;
            }
        }
        private int _X = 0;
        public int X
        {
            get { return this._X; }
            set { this._X = value; }
        }
        private int _Y = 0;
        public int Y
        {
            get { return this._Y; }
            set
            {
                this._Y = value;
            }
        }
    }    
    public delegate void MouseEventHandler(object sender, System.Windows.Forms.MouseEventArgs e);
    public interface IWin32Window
    {
        IntPtr Handle { get; }
    }
#if ! RELEASE
    [System.Diagnostics.DebuggerDisplay("{DebugName}")]
#endif
    public class Control :IWin32Window
    {
        public Control()
        { }
        public virtual bool AllowDrop
        {
            get; set;
        }
        public virtual DCSystem_Drawing.Color BackColor
        {
            get; set;
        }
        public bool Capture
        {
            get; set;
        }
        public DCSystem_Drawing.Rectangle ClientRectangle
        {
            get { return new DCSystem_Drawing.Rectangle(0, 0, this._ClientSizeWidth, this._ClientSizeHeight); }
        }
        protected int _ClientSizeWidth = 0;
        protected int _ClientSizeHeight = 0;
        private System.Windows.Forms.Cursor _Cursor = System.Windows.Forms.Cursors.Default;
        public virtual System.Windows.Forms.Cursor Cursor
        {
            get
            {
                return this._Cursor;
            }
            set
            {
                this._Cursor = value;
            }
        }
        private bool _Enabled = true;
        public bool Enabled
        {
            get
            {
                return this._Enabled;
            }
            set
            {
                this._Enabled = value;
            }
        }
        public virtual bool Focused
        {
            get { return default(bool); }
        }
        public virtual Font Font
        {
            get; set;
        }
        public virtual DCSystem_Drawing.Color ForeColor
        {
            get; set;
        }
        private static int _HandleCounter = 3000;
        private IntPtr _Handle = new IntPtr(_HandleCounter++);
        public virtual IntPtr Handle
        {
            get
            {
                return this._Handle;
            }
        }
        public int Height
        {
            get; set;
        }
        private bool _IsDisposed = false;
        public bool IsDisposed
        {
            get { return _IsDisposed; }
        }
        protected void CheckDisposed()
        {
            if(this.IsDisposed)
            {
                throw new ObjectDisposedException("控件已经被销毁了，无法执行操作");
            }
        }

        public bool IsHandleCreated
        {
            get { return true; }
        }

        private static System.Windows.Forms.Keys _ModifierKeys = Keys.None;
        public static System.Windows.Forms.Keys ModifierKeys
        {
            get
            {
                return _ModifierKeys;
            }
            set
            {
                _ModifierKeys = value;
            }
        }
        public static void SetModifierKeys(bool altKey, bool shiftKey ,bool ctrlKey )
        {
            _ModifierKeys = Keys.None;
            if( altKey )
            {
                _ModifierKeys = _ModifierKeys | Keys.Alt;
            }
            if (shiftKey)
            {
                _ModifierKeys = _ModifierKeys | Keys.Shift;
            }
            if ( ctrlKey )
            {
                _ModifierKeys = _ModifierKeys | Keys.Control;
            }
        }
        protected static System.Windows.Forms.MouseButtons _StaticMouseButtons = MouseButtons.None;

        public static System.Windows.Forms.MouseButtons MouseButtons
        {
            get { return _StaticMouseButtons; }
        }
        protected static DCSystem_Drawing.Point _StaticMousePosition = DCSystem_Drawing.Point.Empty;

        public static DCSystem_Drawing.Point MousePosition
        {
            get { return _StaticMousePosition; }
        }
        public string Name
        {
            get; set;
        }
        public DCSystem_Drawing.Size Size
        {
            get { return new DCSystem_Drawing.Size(this.Width, this.Height); }
            set { this.Width = value.Width; this.Height = value.Height; }
        }
        private bool _Visible = true;
        public bool Visible
        {
            get
            {
                return this._Visible;
            }
            set
            {
                if (this._Visible != value)
                {
                    this._Visible = value;
                    this.OnVisibleChanged(null);
                }
            }
        }
        public int Width
        {
            get; set;
        }
        public Graphics CreateGraphics()
        {
            return new Graphics();
        }
        public virtual void Dispose()
        {
            this._IsDisposed = true;
            this._Cursor = null;
            this.Name = null;
            //this.Dispose(true);
        }
        public virtual bool Focus()
        { return false; }
        public virtual void Invalidate()
        { }
        public virtual void Invalidate(bool invalidateChildren)
        {
        }
        public void Invalidate(DCSystem_Drawing.Rectangle rc)
        { }
        protected virtual void OnClick(EventArgs e)
        { }
        protected virtual void OnDragDrop(System.Windows.Forms.DragEventArgs drgevent)
        { }
        protected virtual void OnDragEnter(System.Windows.Forms.DragEventArgs drgevent)
        { }
        protected virtual void OnDragLeave(EventArgs e)
        { }
        protected virtual void OnDragOver(System.Windows.Forms.DragEventArgs drgevent)
        { }
        protected virtual void OnGotFocus(EventArgs e)
        { }
        protected virtual void OnKeyDown(System.Windows.Forms.KeyEventArgs e)
        { }
        protected virtual void OnKeyPress(System.Windows.Forms.KeyPressEventArgs e)
        { }
        protected virtual void OnKeyUp(System.Windows.Forms.KeyEventArgs e)
        { }
        protected virtual void OnLostFocus(EventArgs e)
        { }
        protected virtual void OnMouseClick(System.Windows.Forms.MouseEventArgs e)
        { }
        protected virtual void OnMouseDoubleClick(System.Windows.Forms.MouseEventArgs e)
        { }
        protected virtual void OnMouseDown(System.Windows.Forms.MouseEventArgs e)
        { }
        protected virtual void OnMouseEnter(EventArgs e)
        { }
        protected virtual void OnMouseLeave(EventArgs e)
        { }
        protected virtual void OnMouseMove(System.Windows.Forms.MouseEventArgs e)
        { }
        protected virtual void OnMouseUp(System.Windows.Forms.MouseEventArgs e)
        { }
        protected virtual void OnPaint(System.Windows.Forms.PaintEventArgs e)
        { }
        public void DoOnPaint(Graphics g, DCSystem_Drawing.Rectangle clipBounds)
        {
            this.OnPaint(new PaintEventArgs(g, clipBounds));
        }
        protected virtual void OnVisibleChanged(EventArgs e)
        { }
        public void Update()
        { }
    }

    public class ToolTip
    {
        public object Tag = null;
        public void RemoveAll()
        {

        }
        public void SetToolTip(object control, string caption)
        {
            if (control is DCSoft.Writer.Controls.WriterViewControl)
            {
                var ctl = (DCSoft.Writer.Controls.WriterViewControl)control;
                if (caption != null && caption.Length > 0)
                {
                }
                else
                {
                    caption = String.Empty;
                }
                ctl.WASMParent.JS_SetToolTip(caption);
            }
        }
    }
}

