using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;
using DCSoft.Writer.Dom;
using DCSoft.Writer.Commands;
using System.Runtime.InteropServices;
using DCSoft.Common;

namespace DCSoft.Writer.Controls
{
    /// <summary>
    /// 处理键盘事件
    /// </summary>
    public partial class WriterViewControl
    {

        private bool _IgnoreNextKeyPressEventOnce = false;
        /// <summary>
        /// 取消下一次的KePress事件
        /// </summary>
        public bool IgnoreNextKeyPressEventOnce
        {
            get
            {
                return _IgnoreNextKeyPressEventOnce;
            }
            set
            {
                _IgnoreNextKeyPressEventOnce = value;
            }
        }
        public void InnerOnKeyDown(KeyEventArgs e)
        {
            this.OnKeyDown(e);
        }

        public void InnerOnKeyPress(KeyPressEventArgs e)
        {
            this.OnKeyPress(e);
        }

        internal void InnerOnKeyUp(KeyEventArgs e)
        {
            this.OnKeyUp(e);
        }
        //internal bool _CurrentEventHandled = false;
        
        //private bool IsCurrentEventHandled()
        //{
        //    var v = this._CurrentEventHandled;
        //    this._CurrentEventHandled = false;
        //    return v;
        //}
        //private DateTime _LastBackKeyTime = DateTime.MinValue;

        //internal enum DCUIEventType
        //{
        //    None,
        //    KeyDown,
        //    KeyUp,
        //    KeyPress,
        //    MouseDown,
        //    MouseMove,
        //    MouseUp
        //}
        //private DCUIEventType _CurrentUIEventType = DCUIEventType.None;
        ///// <summary>
        ///// 当前用户界面事件类型
        ///// </summary>
        //internal DCUIEventType CurrentUIEventType
        //{
        //    get
        //    {
        //        return this._CurrentUIEventType;
        //    }
        //}
        /// <summary>
        /// 处理键盘按键按下事件
        /// </summary>
        /// <param name="e">事件参数</param>
        protected override void OnKeyDown(System.Windows.Forms.KeyEventArgs e)
        {
            //System.Diagnostics.Debug.WriteLine( Environment.TickCount + " " + e.KeyCode.ToString());
            //this._CurrentEventHandled = false;
            //this._LastUIEventTime = DateTime.Now;
            base.OnKeyDown(e);
            if( e.Handled )
            {
                return;
            }
            if (e.Modifiers == Keys.None)
            {
                // 2016-6-3 袁永福：按下字符键不会触发元素的KeyDown事件，在此注释以下代码。
                //// 应该是输入字符，则退出处理
                //if (e.KeyCode >= Keys.A && e.KeyCode <= Keys.Z
                //    || e.KeyCode >= Keys.NumPad0 && e.KeyCode <= Keys.NumPad9
                //    || e.KeyCode == Keys.Space
                //    || e.KeyCode == Keys.Decimal
                //    || e.KeyCode == Keys.Subtract
                //    || e.KeyCode == Keys.Add
                //    || e.KeyCode == Keys.Divide
                //    || e.KeyCode == Keys.Multiply)
                //{
                //    // 字符键,则退出处理，不执行快捷键操作
                //    return;
                //}
            }
            if (this._Document != null)
            {
                if (e.KeyCode == Keys.Escape)
                {
                    if (this.IsEditingElementValue)
                    {
                        // 按下ESC键取消操作
                        this.CancelEditElementValue();
                        e.Handled = true;
                        return;
                    }
                    if (this.StyleInfoForFormatBrush != null)
                    {
                        // 按下ESC键取消格式刷功能
                        CancelFormatBrush();
                        return;
                    }
                }
                DocumentEventArgs args = DocumentEventArgs.CreateKeyDown(this._Document, e);
                try
                {
                    //this._CurrentUIEventType = DCUIEventType.KeyDown;
                    // 优先让文档对象处理键盘按键事件
                    this._Document.HandleDocumentEvent(args);
                    if (args.Handled)
                    {
                        // 事件得到处理
                        return;
                    }
                    if (this._Document.TabIndexManager != null)
                    {
                        var ce = this._Document.CurrentElement;
                        if (ce != null)
                        {
                            DomElement nextElement = this._Document.TabIndexManager.GetNextSelectedElementForHotKey(
                                   ce,
                                   e.KeyCode,
                                   e.Shift,
                                   e);
                            if (nextElement != null)
                            {
                                this.IgnoreNextKeyPressEventOnce = true;
                                nextElement.Focus();
                                args.Handled = true;
                                e.Handled = true;
                                return;
                            }
                        }
                    }
                    if (e.Handled)
                    {
                        // 事件被处理了
                        this.IgnoreNextKeyPressEventOnce = true;
                        return;
                    }
                    //int tick3 = Environment.TickCount;
                    // 文档对象处理完毕，并允许进行后续事件时，调用快捷键命令
                    WriterCommand cmd = this.AppHost.CommandContainer.Active(
                        this.OwnerWriterControl,
                        this._Document,
                        e);
                    //tick3 = Environment.TickCount - tick3;
                    //System.Diagnostics.Debug.WriteLine(WriterCommandDelegate.DebugText.ToString());
                    //System.Diagnostics.Debug.WriteLine("Total " + Convert.ToString( Environment.TickCount - tick3));
                    //MessageBox.Show("bbbbbbbbbb " + Convert.ToString( Environment.TickCount - tick3));
                    if (cmd != null)
                    {
                        this.ExecuteCommand(cmd.Name, true, null);

                        e.Handled = true;
                        if (e.Control && e.KeyCode == Keys.I)
                        {
                            this.IgnoreNextKeyPressEventOnce = true;
                        }
                    }//if
                    //else if(e.KeyCode == Keys.F12 &&  e.Shift )
                    //{
                    //    this.ShowAboutDialog();
                    //}
                }
                finally
                {
                    //this._CurrentUIEventType = DCUIEventType.None;
                }
            }
        }
        private int _MinTickForIgnoreKeyPressEvent = 0;
        public static long EndTickForIgnoreEnterChar = 0;
        /// <summary>
        /// 处理键盘字符事件
        /// </summary>
        /// <param name="e">事件参数</param>
        protected override void OnKeyPress(System.Windows.Forms.KeyPressEventArgs e)
        {
            if (e.KeyChar == 13 && EndTickForIgnoreEnterChar > 0)
            {
                var tickspan = WriterUtilsInner.TickCount - EndTickForIgnoreEnterChar;
                if ( tickspan < 80)
                {
                    return;
                }
                EndTickForIgnoreEnterChar = 0;
            }


            //this._CurrentEventHandled = false;
            //this._LastUIEventTime = DateTime.Now;
            if (this._MinTickForIgnoreKeyPressEvent > 0)
            {
                if (Environment.TickCount < this._MinTickForIgnoreKeyPressEvent)
                {
                    // 短暂的取消键盘字符事件
                    return;
                }
            }
            if (this.IgnoreNextKeyPressEventOnce)
            {
                // 取消键盘字符事件
                this.IgnoreNextKeyPressEventOnce = false;
                return;
            }
            if (e.KeyChar == '\r'
                && Control.ModifierKeys == Keys.Shift)
            {
                return;
            }
            if (e.KeyChar == '\b')
            {
                // 退格键
                return;
            }
            if (e.KeyChar == '\n')
            {
                return ;
            }
            if (e.KeyChar < 32)
            {
                if (e.KeyChar != '\t' && e.KeyChar != '\r')
                {
                    // 出现不可接收的字符
                    return ;
                }
            }
            if (this._Document != null)
            {
               
                DocumentEventArgs args2 = DocumentEventArgs.CreateKeyPress(this._Document, e);
                // 优先让文档对此处理键盘字符事件
                this._Document.HandleDocumentEvent(args2);
                if (args2.Handled)
                {
                    // 已经被文档对象处理了，不再进行后续操作
                    return;
                }
                // 处理表格快键键
                //this.HandleTableHotKey(e);
                if (e.Handled)
                {
                    return;
                }
                if (e.KeyChar == '\t' && Control.ModifierKeys != Keys.Control)
                {
                    if (this.DocumentOptions.EditOptions.TabKeyToFirstLineIndent)
                    {
                        // 对于Tab字符试图设置当前段落的首行缩进
                        if (this.Selection.Length == 0)
                        {
                            if (this.DocumentControler.SpecificChangeParagraphIndent(true))
                            {
                                //if (this.OwnerWriterControl != null)
                                //{
                                //    this.OwnerWriterControl.UpdateRuleState();
                                //}
                                e.Handled = true;
                                return;
                            }
                        }
                    }
                }
                if (this.Readonly == false
                    && this.DocumentControler.CanInsertElementAtCurrentPosition(
                        typeof(DomElement),
                        DomAccessFlags.Normal))
                {
                    // 若当前位置能插入字符则插入字符
                    DocumentContentStyle ccs = ( DocumentContentStyle ) this._Document.CurrentStyleInfo.Content.Clone();
                    string inputStr = e.KeyChar.ToString();
                    DomElementList list = null;
                    try
                    {
                        this._Document.InnerFixCurrentStyleInfoForEnter = true;
                        list = this.DocumentControler.InsertString(
                                inputStr,
                                true,
                                InputValueSource.UI,
                                null,
                                null);
                    }
                    finally
                    {
                    }
                    if (list != null && list.Count > 0)
                    {
                        //this.Document.CurrentStyleInfo = null;
                        if (inputStr == "\r" && this._Document.InnerFixCurrentStyleInfoForEnter )
                        {
                            //if (this.Document.CurrentStyleInfo.Content.ValueLocked == false)
                            if( this.Document.Options.BehaviorOptions.ResetTextFormatWhenCreateNewLine)
                            {
                                var df = (DocumentContentStyle)this.Document.DefaultStyle.Clone();
                                this._Document.CurrentStyleInfo.Content = df;
                                this._Document.CurrentStyleInfo.ContentStyleForNewString = df;
                            }
                            else
                            {
                                this._Document.CurrentStyleInfo.Content.Font = ccs.Font;
                                this._Document.CurrentStyleInfo.ContentStyleForNewString.Font = ccs.Font;
                            }
                        }
                    }
                }
            }
            e.Handled = true;
        }
        /// <summary>
        /// 处理键盘按钮松开事件
        /// </summary>
        /// <param name="e">事件参数</param>
        protected override void OnKeyUp(System.Windows.Forms.KeyEventArgs e)
        {
            this.IgnoreNextKeyPressEventOnce = false;
            if (this._Document != null)
            {
                this._Document.HandleDocumentEvent(
                    DocumentEventArgs.CreateKeyUp(
                        this._Document,
                        e));
            }
        }

    }
}
