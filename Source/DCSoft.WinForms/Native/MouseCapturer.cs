using System;
using System.Runtime.InteropServices;
// // 
using System.Windows.Forms ;

namespace DCSoft.WinForms.Native
{
    /// <summary>
    /// 可逆图形样式
    /// </summary>
    public enum ReversibleShapeStyle
    {
        /// <summary>
        /// 可逆的直线
        /// </summary>
        Line =0,
        /// <summary>
        /// 可逆矩形边框
        /// </summary>
        Rectangle =1,
        /// <summary>
        /// 椭圆形
        /// </summary>
        Ellipse =2,
        /// <summary>
        /// 可逆的填充的矩形
        /// </summary>
        FillRectangle=3,
        /// <summary>
        /// 自定义,触发事件
        /// </summary>
        Custom = 4
    }

    /// <summary>
    /// 鼠标拖拽回调函数委托类型
    /// </summary>
    public delegate void CaptureMouseMoveEventHandler(object sender, CaptureMouseMoveEventArgs e);
    /// <summary>
    /// 鼠标拖拽事件消息对象
    /// </summary>
    public class CaptureMouseMoveEventArgs : System.EventArgs
    {
        /// <summary>
        /// 初始化对象
        /// </summary>
        /// <param name="sender">消息发送者</param>
        /// <param name="sp">开始点坐标</param>
        /// <param name="cp">当前点坐标</param>
        public CaptureMouseMoveEventArgs(
            MouseCapturer sender,
             DCSystem_Drawing.Point sp,
             DCSystem_Drawing.Point cp)
        {
            this.mySender = sender;
            this.myStartPosition = sp;
            this.myCurrentPosition = cp;
            if (sender != null)
            {
                this._Tag = sender.Tag;
            }
            //this.bolCancel = false ;
        }

        private object _Tag = null;
        /// <summary>
        /// 额外数据
        /// </summary>
        public object Tag
        {
            get { return _Tag; }
            set { _Tag = value; }
        }

        private readonly MouseCapturer mySender = null;
        /// <summary>
        /// 消息发送者
        /// </summary>
        public MouseCapturer Sender
        {
            get
            {
                return mySender; 
            }
        }

        private DCSystem_Drawing.Point myStartPosition = DCSystem_Drawing.Point.Empty;
        /// <summary>
        /// 鼠标开始拖拽的点坐标
        /// </summary>
        public DCSystem_Drawing.Point StartPosition
        {
            get
            {
                return myStartPosition; 
            }
            set
            {
                myStartPosition = value; 
            }
        }

        private DCSystem_Drawing.Point myCurrentPosition = DCSystem_Drawing.Point.Empty;
        /// <summary>
        /// 鼠标当前点坐标
        /// </summary>
        public DCSystem_Drawing.Point CurrentPosition
        {
            get
            {
                return myCurrentPosition; 
            }
            set
            {
                myCurrentPosition = value; 
            }
        }

        /// <summary>
        /// 当前横向移动的距离
        /// </summary>
        public int DX
        {
            get
            {
                return myCurrentPosition.X - myStartPosition.X; 
            }
        }

        /// <summary>
        /// 当前纵向移动的距离
        /// </summary>
        public int DY
        {
            get
            {
                return myCurrentPosition.Y - myStartPosition.Y; 
            }
        }

        private bool bolResumeView = false;
        /// <summary>
        /// 绘图操作是否是恢复原始视图
        /// </summary>
        public bool ResumeView
        {
            get
            {
                return bolResumeView; 
            }
            set
            {
                bolResumeView = value; 
            }
        }
        //private bool bolCancel = false;
        /// <summary>
        /// 是否取消拖拽
        /// </summary>
        public bool Cancel
        {
            get
            {
                return mySender.CancelFlag; 
            }
            set
            {
                mySender.CancelFlag = value; 
            }
        }
    }//public class CaptureMouseMoveEventArgs

    ///// <summary>
    ///// 绘制矩形的鼠标拖拽对象
    ///// </summary>
    ///// <remarks>编写 袁永福</remarks>
    //[System.Runtime.InteropServices.ComVisible(false)]
    //public class RectangleMouseCapturer : MouseCapturer
    //{
    //    /// <summary>
    //    /// 初始化对象
    //    /// </summary>
    //    public RectangleMouseCapturer()
    //    {
    //    }
    //    /// <summary>
    //    /// 初始化对象
    //    /// </summary>
    //    /// <param name="ctl">进行鼠标拖拽的控件对象</param>
    //    public RectangleMouseCapturer(System.Windows.Forms.Control ctl)
    //    {
    //        this.BindControl = ctl;
    //    }
    //    protected int intDragStyle = 0;
    //    /// <summary>
    //    /// 拖拽类型
    //    /// </summary>
    //    public int DragStyle
    //    {
    //        get { return intDragStyle; }
    //        set { intDragStyle = value; }
    //    }
    //    protected  Rectangle mySourceRectangle =  Rectangle.Empty;
    //    /// <summary>
    //    /// 原始矩形
    //    /// </summary>
    //    public  Rectangle SourceRectangle
    //    {
    //        get { return mySourceRectangle; }
    //        set { mySourceRectangle = value; }
    //    }

    //    protected  Rectangle myDescRectangle =  Rectangle.Empty;
    //    /// <summary>
    //    /// 处理后的矩形
    //    /// </summary>
    //    public  Rectangle DescRectangle
    //    {
    //        get { return myDescRectangle; }
    //        set { myDescRectangle = value; }
    //    }

    //    protected bool bolCustomAction = false;
    //    /// <summary>
    //    /// 自定义动作样式
    //    /// </summary>
    //    public bool CustomAction
    //    {
    //        get { return bolCustomAction; }
    //        set { bolCustomAction = value; }
    //    }

    //    /// <summary>
    //    /// 更新矩形
    //    /// </summary>
    //    /// <param name="rect">原始矩形</param>
    //    /// <param name="dx">水平移动量</param>
    //    /// <param name="dy">垂直移动量</param>
    //    /// <returns>处理后的矩形</returns>
    //    public  Rectangle UpdateRectangle( Rectangle rect, int dx, int dy)
    //    {
    //        // 中间
    //        if (intDragStyle == -1)
    //            rect.Offset(dx, dy);
    //        // 左边
    //        if (intDragStyle == 0 || intDragStyle == 7 || intDragStyle == 6)
    //        {
    //            rect.Offset(dx, 0);
    //            rect.Width = rect.Width - dx;
    //        }
    //        // 顶边
    //        if (intDragStyle == 0 || intDragStyle == 1 || intDragStyle == 2)
    //        {
    //            rect.Offset(0, dy);
    //            rect.Height = rect.Height - dy;
    //        }
    //        // 右边
    //        if (intDragStyle == 2 || intDragStyle == 3 || intDragStyle == 4)
    //        {
    //            rect.Width = rect.Width + dx;
    //        }
    //        // 底边
    //        if (intDragStyle == 4 || intDragStyle == 5 || intDragStyle == 6)
    //        {
    //            rect.Height = rect.Height + dy;
    //        }
    //        return rect;
    //    }

    //    /// <summary>
    //    /// 绘制可逆矩形
    //    /// </summary>
    //    protected override void OnDraw(bool ResumeView)
    //    {
    //        base.OnDraw(ResumeView);
    //        if (bolCustomAction)
    //            return;
    //        ReversibleDrawer drawer = null;
    //        if (this.BindControl != null)
    //        {
    //            drawer = ReversibleDrawer.FromHwnd(this.BindControl.Handle);
    //        }
    //        else
    //        {
    //            drawer = ReversibleDrawer.FromScreen();
    //        }
    //        drawer.DrawRectangle(myDescRectangle);
    //        drawer.Dispose();
    //    }
    //    /// <summary>
    //    /// 当前拖拽的矩形区域
    //    /// </summary>
    //    public  Rectangle CurrentRectangle
    //    {
    //        get
    //        {
    //            return MouseCapturer.GetRectangle(this.StartPosition, this.CurrentPosition);
    //        }
    //    }

    //    /// <summary>
    //    /// 处理鼠标移动事件
    //    /// </summary>
    //    protected override void OnMouseMove()
    //    {
    //        base.OnMouseMove();
    //        if (bolCustomAction)
    //            return;
    //        int dx = base.CurrentPosition.X - base.StartPosition.X;
    //        int dy = base.CurrentPosition.Y - base.StartPosition.Y;
    //        this.myDescRectangle = UpdateRectangle(this.mySourceRectangle, dx, dy);
    //    }
    //}

    /// <summary>
    /// 捕获鼠标的模块
    /// </summary>
    /// <remarks>编写 袁永福</remarks>
    public class MouseCapturer
    {

        ///// <summary>
        ///// 无作为的初始化对象
        ///// </summary>
        //public MouseCapturer()
        //{
        //}
        /// <summary>
        /// 初始化对象并设置绑定的控件
        /// </summary>
        /// <param name="ctl">绑定的控件</param>
        public MouseCapturer(System.Windows.Forms.Control ctl)
        {
            this.BindControl = ctl;
        }
        public void Dispose()
        {
            this._BindControl = null;
            //this._ReverDrawer = null;
            this.objTag = null;
            this.objTag2 = null;
        }


        private System.Windows.Forms.Control _BindControl = null;
        /// <summary>
        /// 对象绑定的控件,若该控件有效则鼠标光标是用控件客户区坐标,否则采用屏幕坐标
        /// </summary>
        public System.Windows.Forms.Control BindControl
        {
            get
            {
                return this._BindControl; 
            }
            set
            {
                this._BindControl = value;
                if(this._BindControl is DCSoft.Writer.Controls.WriterViewControl)
                {
                    var ctl2 = (DCSoft.Writer.Controls.WriterViewControl)this._BindControl;
                    ctl2._CurrentMouseCapturer = this;
                }
            }
        }

        public void HandleMouseDown(System.Windows.Forms.MouseEventArgs args)
        {
            this._DragState = MyDragState.Detecting;
            this._CurrentPosition = new DCSystem_Drawing.Point(args.X, args.Y);
            this._InitStartPosition = this._CurrentPosition;
            this._StartPosition = this._CurrentPosition;
        }
        private enum MyDragState
        {
            Detecting,
            Drag,
            Cancel 
        }
        private MyDragState _DragState = MyDragState.Detecting;
        public bool HandleMouseMove( System.Windows.Forms.MouseEventArgs args )
        {
            if(this._DragState == MyDragState.Detecting)
            {
                // 正在检测是否要执行鼠标拖拽操作
                var size = System.Windows.Forms.SystemInformation.DragSize;
                if( Math.Abs( args.X - this._StartPosition.X ) >= size.Width 
                    || Math.Abs( args.Y - this._StartPosition.Y ) >= size.Height )
                {
                    this._DragState = MyDragState.Drag;
                    return true;
                }
            }
            else if( this._DragState == MyDragState.Drag)
            {
                // 正在进行拖拽操作
                if( args.X != this._CurrentPosition.X || args.Y != this._CurrentPosition.Y )
                {
                    // 鼠标位置移动了
                    this._CurrentPosition = new DCSystem_Drawing.Point(args.X, args.Y);
                    this.OnMouseMove();
                    this.OnDraw(true);
                    this._LastPosition = this._CurrentPosition;
                    if(this._CancelFlag )
                    {
                        this._DragState = MyDragState.Cancel;
                    }
                }
                return true;
            }
            return false;
        }
        public void HandleMouseUp(System.Windows.Forms.MouseEventArgs args)
        {
            if( this._DragState == MyDragState.Drag )
            {
                this._EndPosition = new DCSystem_Drawing.Point(args.X, args.Y);
                this._MoveSize = new DCSystem_Drawing.Size(args.X - this._StartPosition.X, args.Y - this._StartPosition.Y);
                if(this.EventOnFinished != null )
                {
                    this.EventOnFinished(this, args );
                }
            }
            this._DragState = MyDragState.Cancel;
        }
        //public void Finished()
        //{
        //    if (this._DragState == MyDragState.Drag)
        //    {
        //        this._EndPosition = this._CurrentPosition;
        //        this._MoveSize = new Size(
        //            this._CurrentPosition.X - this._StartPosition.X, 
        //            this._CurrentPosition.Y - this._StartPosition.Y);
        //        if (this.EventOnFinished != null)
        //        {
        //            this.EventOnFinished(this, null );
        //        }
        //    }
        //}

        public EventHandler EventOnFinished = null;
        private DCSystem_Drawing.Point _InitStartPosition = DCSystem_Drawing.Point.Empty;
        /// <summary>
        /// 初始化时的鼠标开始位置
        /// </summary>
        public DCSystem_Drawing.Point InitStartPosition
        {
            get
            {
                return this._InitStartPosition; 
            }
            set
            {
                this._InitStartPosition = value; 
            }
        }
        private DCSystem_Drawing.Point _StartPosition = DCSystem_Drawing.Point.Empty;
        /// <summary>
        /// 开始捕获时的鼠标光标的位置
        /// </summary>
        public DCSystem_Drawing.Point StartPosition
        {
            get
            {
                return _StartPosition; 
            }
            set
            {
                _StartPosition = value;
            }
        }
        private DCSystem_Drawing.Point _EndPosition = DCSystem_Drawing.Point.Empty;
        /// <summary>
        /// 结束捕获时鼠标光标位置
        /// </summary>
        public DCSystem_Drawing.Point EndPosition
        {
            get
            {
                return _EndPosition; 
            }
        }
        private DCSystem_Drawing.Point _LastPosition = DCSystem_Drawing.Point.Empty;
        /// <summary>
        /// 上一次处理时鼠标光标位置
        /// </summary>
        public DCSystem_Drawing.Point LastPosition
        {
            get 
            {
                return _LastPosition; 
            }
        }
        private DCSystem_Drawing.Point _CurrentPosition = DCSystem_Drawing.Point.Empty;
        /// <summary>
        /// 当前鼠标光标的位置
        /// </summary>
        public DCSystem_Drawing.Point CurrentPosition
        {
            get 
            {
                return _CurrentPosition; 
            }
        }

        private DCSystem_Drawing.Size _MoveSize = DCSystem_Drawing.Size.Empty;
        /// <summary>
        /// 整个操作中鼠标移动的距离,属性的Width值表示光标横向移动的距离,Height值表示纵向移动距离
        /// </summary>
        public DCSystem_Drawing.Size MoveSize
        {
            get
            {
                return _MoveSize; 
            }
            set
            {
                _MoveSize = value; 
            }
        }
        /// <summary>
        /// 整个操作中鼠标横向移动距离
        /// </summary>
        public int DX
        {
            get 
            {
                return this._EndPosition.X - this._StartPosition.X; 
            }
        }
        /// <summary>
        /// 整个操作中鼠标纵向移动距离
        /// </summary>
        public int DY
        {
            get
            {
                return this._EndPosition.Y - this._StartPosition.Y; 
            }
        }
        /// <summary>
        /// 鼠标移动起点和终点组成的矩形区域
        /// </summary>
        public DCSystem_Drawing.Rectangle CaptureRectagle
        {
            get
            {
                // 根据起点坐标和终点坐标确定选择区域矩形
                 var rect = GetRectangle(_StartPosition, _EndPosition);
                //rect.Location = this.FixPointForControl(rect.Location);
                return rect;
            }
        }

        //internal static Rectangle GetRectangle(Point p1, Point p2)
        //{
        //    Rectangle rect = new Rectangle(Math.Min(p1.X, p2.X), Math.Min(p1.Y, p2.Y), 0, 0);
        //    rect.Width = Math.Max(p1.X, p2.X) - rect.Left;
        //    rect.Height = Math.Max(p1.Y, p2.Y) - rect.Top;
        //    return rect;
        //}
        private DCSystem_Drawing.Rectangle _ClipRectangle = DCSystem_Drawing.Rectangle.Empty;
        /// <summary>
        /// 鼠标光标的活动范围
        /// </summary>
        public DCSystem_Drawing.Rectangle ClipRectangle
        {
            get 
            {
                return _ClipRectangle; 
            }
            set
            {
                _ClipRectangle = value; 
            }
        }

        protected virtual CaptureMouseMoveEventArgs CreateArgs()
        {
            CaptureMouseMoveEventArgs e = new CaptureMouseMoveEventArgs(
                this,
                this._StartPosition,
                this._CurrentPosition);
            return e;
        }


        /// <summary>
        /// 鼠标捕获期间移动时的回调处理事件
        /// </summary>
        public CaptureMouseMoveEventHandler MouseMove = null;

        /// <summary>
        /// 鼠标捕获期间移动时的回调处理
        /// </summary>
        protected virtual void OnMouseMove()
        {
            if (MouseMove != null)
                MouseMove(this, this.CreateArgs());
        }
        /// <summary>
        /// 鼠标捕获期间绘制可逆图形的回调处理事件
        /// </summary>
        public CaptureMouseMoveEventHandler Draw = null;

        protected virtual void OnDraw(bool ResumeView)
        {
            if (this.ReversibleShape == ReversibleShapeStyle.Custom)
            {
                if (Draw != null)
                {
                    CaptureMouseMoveEventArgs args = this.CreateArgs();
                    args.ResumeView = ResumeView;
                    Draw(this, args);
                }
            }
            else
            {
                var rect = GetRectangle(this.StartPosition, this.CurrentPosition);
                var ctl = this.BindControl as DCSoft.Writer.Controls.WriterViewControl;
                ctl.InnerReversibleDraw(
                    rect.Left , 
                    rect.Top , 
                    rect.Right , 
                    rect.Bottom, 
                    this.ReversibleShape);
            }
        }

        private ReversibleShapeStyle intReversibleShape = ReversibleShapeStyle.Custom;
        /// <summary>
        /// 可逆图形样式
        /// </summary>
        public ReversibleShapeStyle ReversibleShape
        {
            get
            {
                return intReversibleShape;
            }
            set
            {
                intReversibleShape = value;
            }
        }

        private object objTag = null;
        /// <summary>
        /// 对象额外数据
        /// </summary>
        public object Tag
        {
            get
            {
                return objTag;
            }
            set
            {
                objTag = value;
            }
        }

        private object objTag2 = null;
        /// <summary>
        /// 对象额外数据2
        /// </summary>
        public object Tag2
        {
            get
            {
                return objTag2;
            }
            set
            {
                objTag2 = value;
            }
        }
        private bool _CancelFlag = false;
        public bool CancelFlag
        {
            get 
            {
                return _CancelFlag; 
            }
            set
            {
                _CancelFlag = value; 
            }
        }

        /// <summary>
        /// 捕获鼠标移动
        /// </summary>
        /// <returns>是否完成操作</returns>
        public bool CaptureMouseMove()
        {
            return CaptureMouseMove(true);
        }

        /// <summary>
        /// 捕获鼠标移动
        /// </summary>
        /// <param name="needDragDetect">是否事先检测拖拽操作</param>
        /// <returns>是否成功的完成了操作</returns>
        public bool CaptureMouseMove(bool needDragDetect)
        {
            return false;
        }

        internal static DCSystem_Drawing.Rectangle GetRectangle(DCSystem_Drawing.Point p1, DCSystem_Drawing.Point p2)
        {
            DCSystem_Drawing.Rectangle rect = DCSystem_Drawing.Rectangle.Empty;
            rect.X = Math.Min(p1.X, p2.X);
            rect.Y = Math.Min(p1.Y, p2.Y);
            rect.Width = Math.Max(p1.X, p2.X) - rect.Left;
            rect.Height = Math.Max(p1.Y, p2.Y) - rect.Top;
            return rect;
        }
    }//public class MouseCapturer
}