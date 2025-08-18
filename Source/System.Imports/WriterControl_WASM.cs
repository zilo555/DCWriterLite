using DCSoft.Writer.Dom;
using DCSoft.Printing;
using System.Windows.Forms;
using System.Collections.Generic;
using DCSoft.Writer.Data;

namespace DCSoft.Writer.Controls
{
    

    partial class WriterControl
    {
        public virtual string InnerFlagString()
        {
            return null;
        }
        public virtual void JS_UpdatePages(string strPageCodes) 
        {
        }
        /// <summary>
        /// 显示对话框
        /// </summary>
        /// <param name="dialogName"></param>
        /// <param name="parameter"></param>
        public virtual void WASMShowDialog( string dialogName , object parameter )
        {

        }

        private float _WASMZoomRate = 1f;
        /// <summary>
        /// 视图缩放比率
        /// </summary>
        public float WASMZoomRate
        {
            get { return _WASMZoomRate; }
            set { this._WASMZoomRate = value; }
        }

        private float _WASMBaseZoomRate = 1f;
        /// <summary>
        /// 基础性的缩放比率
        /// </summary>
        public float WASMBaseZoomRate
        {
            get
            {
                return this._WASMBaseZoomRate;
            }
            set
            {
                this._WASMBaseZoomRate = value;
            }
        }

        private bool _WASMDoubleBufferForPaint = true;
        /// <summary>
        /// 是否启用双缓冲绘图
        /// </summary>
        public bool WASMDoubleBufferForPaint
        {
            get
            {
                return _WASMDoubleBufferForPaint;
            }
            set
            {
                _WASMDoubleBufferForPaint = value;
            }
        }


        public virtual void JS_BeginEditValueUseListBoxControl(
            DCSoft.Writer.Dom.DomInputFieldElement field,
            DCSoft.Writer.Data.ListItemCollection items,
            System.Action<string, string> callBack)
        {

        }
        public virtual void RaiseEventUpdateToolarState()
        {

        }
        public virtual void WASMRaiseEvent( string eventName , object args)
        {

        }
        public void WASMGetCommandStatus(string commandName, ref bool bolSupported, ref bool bolEnabled, ref bool bolChecked)
        {
            var cmd = this.CommandControler.GetCommand(commandName);
            if (cmd == null)
            {
                bolSupported = false;
            }
            else
            {
                bolSupported = true;
                var args = new DCSoft.Writer.Commands.WriterCommandEventArgs(
                        this,
                        this.Document,
                        DCSoft.Writer.Commands.WriterCommandEventMode.QueryState,
                        this.CommandControler);
                args.ShowUI = true;
                cmd.Execute(args);
                bolChecked = args.Checked;
                bolEnabled = args.Enabled;
            }
        }

        /// <summary>
        /// 绘制标尺控件图形
        /// </summary>
        /// <param name="ruleName">标尺名称</param>
        /// <param name="positionOffset">偏移量</param>
        /// <returns>生成的绘图代码</returns>
        public byte[] PaintRuleControl(string ruleName, int positionOffset, int viewSize)
        {
            var ctl = GetRuleControl(ruleName);
            if (ctl == this.ctlHRule)
            {
                ctl.PositionOffsetX = positionOffset;
                ctl.PositionOffsetY = 0;
            }
            else
            {
                ctl.PositionOffsetX = 0;
                ctl.PositionOffsetY = positionOffset;
            }
            var g = new Graphics();
            g.SetAbsoluteOffset(ctl.PositionOffsetX, ctl.PositionOffsetY);
            ctl.DoOnPaint(g, new Rectangle(0, 0, ctl.Width, ctl.Height));
            var strCode = g.ToByteArray();
            g.Dispose();
            return strCode;
        }
        /// <summary>
        /// 打印页面内容
        /// </summary>
        /// <param name="vPageIndex">页码</param>
        /// <param name="forPrintPreview">是否为了打印预览</param>
        /// <returns>生成的打印指令字符串</returns>
        public byte[] WASMPaintPageForPrint(int vPageIndex, bool forPrintPreview)
        {
            return this.myViewControl.WASMPaintPageForPrint(this.Pages.SafeGet(vPageIndex), forPrintPreview);
        }
        public override Cursor Cursor
        {
            get { return this.myViewControl.Cursor; }
            set { this.myViewControl.Cursor = value; }
        }
        public void InsertStringFromUI( string txt )
        {
            this.GetInnerViewControl().InsertStringFromUI(txt);
        }
 
        public string MyRaiseRuleMouseEvent(
            string strRuleName,
            MouseEventArgs args,
            DCSoft.WinForms.MouseEventType type)
        {
            var ctl = GetRuleControl(strRuleName);
            args.X -= ctl.PositionOffsetX;
            args.Y -= ctl.PositionOffsetY;
            if (type == WinForms.MouseEventType.MouseDown)
            {
                ctl.HandleMouseEvent(args, type);
                if (ctl._CurrentMouseCapturer != null)
                {
                    return "capturemouse";
                }
            }
            else
            {
                var oldValue1 = ctl._ToolTip;
                var oldValue2 = ctl.Cursor.Name;
                ctl.HandleMouseEvent(args, type);
                if(oldValue1 != ctl._ToolTip ||  oldValue2 != ctl.Cursor.Name )
                {
                    return ctl._ToolTip + "," + ctl.Cursor.Name;
                }
                //var newValue = ctl._ToolTip + "," + ctl.Cursor.Name;
                //if (oldValue != newValue)
                //{
                //    return newValue;
                //}
            }
            return null;
        }
         
        private DCDocumentRuleControl GetRuleControl( string ruleName )
        {
            if( ruleName == "hrule")
            {
                return this.ctlHRule;
            }
            else
            {
                return this.ctlVRule;
            }
        }
        public void MyRaiseDragDrop(DragEventArgs drgevent)
        {
            this.myViewControl.MyRaiseDragDrop(drgevent);
        }
        public void MyRaiseDragEnter(DragEventArgs drgevent)
        {
            this.myViewControl.MyRaiseDragEnter(drgevent);
        }
        public void MyRaiseDragLeave(EventArgs e)
        {
            this.myViewControl.MyRaiseDragLeave(e);
        }
        public void MyRaiseDragOver(System.Windows.Forms.DragEventArgs drgevent)
        {
            this.myViewControl.MyRaiseDragOver(drgevent);
        }
        public void MyRaiseMouseDown(MouseEventArgs args)
        {
            this.myViewControl.MyRaiseMouseDown(args);
        }
        public void MyRaiseMouseMove(MouseEventArgs args)
        {
            this.myViewControl.MyRaiseMouseMove(args);
        }
        public void MyRaiseMouseUp(MouseEventArgs args)
        {
            this.myViewControl.MyRaiseMouseUp(args);
        }
        public void MyRaiseMouseClick(MouseEventArgs args)
        {
            this.myViewControl.MyRaiseMouseClick(args);
        }
        public void MyRaiseMouseDoubleClick(MouseEventArgs args)
        {
            this.myViewControl.MyRaiseMouseDoubleClick(args);
        }

        public void MyRaiseKeyDown(KeyEventArgs args)
        {
            this.myViewControl.MyRaiseKeyDown(args);
        }
        public void MyRaiseKeyPress(KeyPressEventArgs args)
        {
            this.myViewControl.MyRaiseKeyPress(args);
        }
        public void MyRaiseKeyUp(KeyEventArgs args)
        {
            this.myViewControl.MyRaiseKeyUp(args);
        }
        public byte[] WASMPaintPage(int pageIndex)
        {
            return this.myViewControl.WASMPaintPage(pageIndex);
        }
        private DCSoft.WASM.WriterControlForWASM _InnerWASMParent = null;
        public virtual DCSoft.WASM.WriterControlForWASM WASMParent
        {
            get
            {
                return this._InnerWASMParent;
            }
        }

        public virtual string WASMClientID
        {
            get { return null; }
        }
        public virtual void StartForWASM( DCSoft.WASM.WriterControlForWASM pv)
        {
            //DCSoft.Writer.WinFormStarter.VoidMethod();
            this.myViewControl.Width = 1;
            this.myViewControl.Height = 1;
            //this.PageSpacing = 20;
            //this.myViewControl.RaiseOnLoad();
            this.CommandControler = new Commands.WriterCommandControler();
            this.CommandControler.Start();
            this.myViewControl.StartForWASM();
            this.myViewControl.HideCaretWhenHasSelection = false;
            this.myViewControl.Name = this.Name;
            this._InnerWASMParent = pv;
            WASMEnvironment.SetJSProivder(pv);
        }
        /// <summary>
        /// 软件产品发布日期
        /// </summary>
        public static readonly string PublishDateString = DCSystemInfo.PublishDateString;
        public override void Dispose()
        {
            base.Dispose();
            if (this.ctlHRule != null)
            {
                this.ctlHRule.ClearData();
                this.ctlHRule.Dispose();
                this.ctlHRule = null;
            }
            if (this.ctlVRule != null)
            {
                this.ctlVRule.ClearData();
                this.ctlVRule.Dispose();
                this.ctlVRule = null;
            }

            //if (this._TmrForUpdateToolarState != null)
            //{
            //    this._TmrForUpdateToolarState.Dispose();
            //    this._TmrForUpdateToolarState = null;
            //}
            if (this.myViewControl != null)
            {
                this.myViewControl._DisposingControl = true;
            }
            this.ClearDocumentState();
            if (this.AutoDisposeDocument)
            {
                if (this.myViewControl != null)
                {
                    this.myViewControl.DisposeDocument();
                }
            }
            if(this.myViewControl != null )
            {
                this.myViewControl.Dispose();
                this.myViewControl = null;
            }
            this._AppHost = null;
             
            this.InnerClearMemberValues();
        }



        /// <summary>
        /// 清除成员数值，DCWriter内部使用。
        /// </summary>
        protected virtual void InnerClearMemberValues()
        {
            if (this._Provider != null)
            {
                this._Provider.Dispose();
                this._Provider = null;
            }
            if (this.ctlHRule != null)
            {
                this.ctlHRule.ClearData();
                this.ctlHRule = null;
            }
            if (this.ctlVRule != null)
            {
                this.ctlVRule.ClearData();
                this.ctlVRule = null;
            }
            ClearDocumentState();
            if (this.AutoDisposeDocument && this.GetInnerViewControl() != null)
            {
                this.GetInnerViewControl().DisposeDocument();
            }
            //this._Clipboard = null;
            this.myViewControl = null;
            this.ctlHRule = null;
            this.ctlVRule = null;
            this._CommandStateNeedRefreshFlag = false;
            if (this.myViewControl != null)
            {
                this.myViewControl.Document.Dispose();
                this.myViewControl.Dispose();
            }
            this.myViewControl = null;
            this.ctlHRule = null;
            this.ctlVRule = null;
            this._AppHost = null;
        }
         
        /// <summary> 
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.myViewControl = new DCSoft.Writer.Controls.WriterViewControl(this);
            this.ctlHRule = new DCSoft.Writer.Controls.DCDocumentRuleControl();
            this.ctlVRule = new DCSoft.Writer.Controls.DCDocumentRuleControl();
            
            //this.myViewControl.AutoScroll = true;
            //this.myViewControl.Location = new Point(39, 28);
            //this.$1.Name = "myViewControl";
            //this.myViewControl.Size = new System.Drawing.Size(319, 170);
            //this.myViewControl.TabIndex = 0;
            // 
            // ctlHRule
            // 
            this.ctlHRule.BackColor = DCDocumentRuleControl.StdBackColor ;// Color.FromArgb(((int)(((byte)(155)))), ((int)(((byte)(187)))), ((int)(((byte)(227)))));
            //this.ctlHRule.Location = new Point(39, 4);
            //this.$1.Name = "ctlHRule";
            this.ctlHRule.Size = new Size(100000, 24);
            //this.ctlHRule.TabIndex = 1;
            this.ctlHRule.Name = "hrule";
            // 
            // ctlVRule
            // 
            this.ctlVRule.BackColor = DCDocumentRuleControl.StdBackColor;// Color.FromArgb(((int)(((byte)(155)))), ((int)(((byte)(187)))), ((int)(((byte)(227)))));
            this.ctlVRule.Direction = DCSoft.Writer.Controls.DCDocumentRuleDirection.Vertical;
            //this.ctlVRule.Location = new Point(15, 28);
            //this.$1.Name = "ctlVRule";
            this.ctlVRule.Size = new Size(24, 100000);
            //this.ctlVRule.TabIndex = 2;
            this.ctlVRule.Name = "vrule";
        }

        private WriterViewControl myViewControl;
        internal DCDocumentRuleControl ctlHRule;
        internal DCDocumentRuleControl ctlVRule;
    }



}

