
using System;
using System.ComponentModel;
using System.Windows.Forms;
using DCSoft.Drawing;
using DCSoft.WinForms.Native;

namespace DCSoft.WinForms
{
    /// <summary>
    /// 文档视图控件
    /// </summary>
    public class DocumentViewControl : System.Windows.Forms.Control
    {
        /// <summary>
        /// 初始化对象
        /// </summary>
        public DocumentViewControl()
        {
            //this.myScaleViewer.BindViewControl = this ;
        }

        /// <summary>
        /// 鼠标拖拽滚动时使用手形鼠标光标
        /// </summary>
        protected bool bolDragUseHandCursor = true;

        protected System.Windows.Forms.Cursor myDefaultCursor = System.Windows.Forms.Cursors.Default;

        protected float fXZoomRate = 1.0f;
        /// <summary>
        /// X方向缩放率
        /// </summary>
        public float XZoomRate
        {
            get
            {
                return fXZoomRate;
            }
            set
            {
                if (fXZoomRate != value)
                {
                    fXZoomRate = value;
                    this.UpdateViewBounds();
                    this.Invalidate();
                }
            }
        }

        protected float fYZoomRate = 1.0f;
        /// <summary>
        /// Y方向缩放率
        /// </summary>
        public float YZoomRate
        {
            get
            {
                return fYZoomRate;
            }
            set
            {
                if (fYZoomRate != value)
                {
                    fYZoomRate = value;
                    this.UpdateViewBounds();
                    this.Invalidate();
                }
            }
        }

        protected void CheckZoomRate()
        {
            if (this.fXZoomRate <= 0 || this.fYZoomRate <= 0)
                throw new System.InvalidOperationException("Bad zoom rate value");
        }
        /// <summary>
        /// 绘图单位
        /// </summary>
        private GraphicsUnit _GraphicsUnit = GraphicsUnit.Pixel;
        /// <summary>
        /// 绘图单位
        /// </summary>
        public virtual GraphicsUnit GraphicsUnit
        {
            get
            {
                return _GraphicsUnit;
            }
            set
            {
                _GraphicsUnit = value;
            }
        }


        /// <summary>
        /// 横向的客户区图形度量单位和文档视图度量单位的比率
        /// </summary>
		public double ClientToViewXRate
        {
            get
            {
                double rate = GraphicsUnitConvert.GetRate(
                    this.GraphicsUnit,
                     GraphicsUnit.Pixel);// * 96 / this._GraphicsDPIX;
                rate /= this.fXZoomRate;
                return rate;
            }
        }
        public double ClientToViewYRate
        {
            get
            {
                double rate = GraphicsUnitConvert.GetRate(
                    this.GraphicsUnit,
                     GraphicsUnit.Pixel);// * 96/ this._GraphicsDPIY;
                rate /= this.fYZoomRate;
                return rate;
            }
        }

        /// <summary>
        /// 从控件客户区到视图区的转换对象
        /// </summary>
        protected TransformBase myTransform = new SimpleRectangleTransform();

        /// <summary>
        /// 用于鼠标拖拽时使用的坐标转换对象
        /// </summary>
        protected virtual TransformBase MouseCaptureTransform
        {
            get
            {
                return this.myTransform;
            }
        }

        private DCSystem_Drawing.PointF myViewOffset = DCSystem_Drawing.PointF.Empty;
        /// <summary>
        /// 视图区域偏移量
        /// </summary>
        public DCSystem_Drawing.PointF ViewOffset
        {
            get
            {
                return myViewOffset;
            }
            set
            {
                myViewOffset = value;
            }
        }

        /// <summary>
        /// 内部的从控件客户区到视图区的转换对象
        /// </summary>
        public TransformBase Transform
        {
            get
            {
                return this.myTransform;
            }
        }

        /// <summary>
        /// 刷新坐标转换对象
        /// </summary>
        public virtual void RefreshScaleTransform(float dpi = 96)
        {
            SimpleRectangleTransform transform = this.myTransform as SimpleRectangleTransform;
            if (transform == null)
                return;

            DCSystem_Drawing.Rectangle rect = this.ClientRectangle;
            transform.SourceRect = rect;

            float xrate = (float)this.ClientToViewXRate;
            float yrate = (float)this.ClientToViewYRate;

            DCSystem_Drawing.RectangleF rect2 = new DCSystem_Drawing.RectangleF(
               0,
               0,
               rect.Width * xrate,
               rect.Height * yrate);

            rect2.Offset(this.ViewOffset);
            transform.DescRectF = rect2;
        }

        /// <summary>
        /// 将客户区坐标转换为视图区坐标
        /// </summary>
        /// <param name="x">客户区点X坐标</param>
        /// <param name="y">客户区点Y坐标</param>
        /// <returns>转换后的视图点坐标</returns>
        public virtual DCSystem_Drawing.Point ClientPointToView(int x, int y)
        {
            this.RefreshScaleTransform();
            return this.Transform.TransformPoint(x, y);
        }
        /// <summary>
        /// 将客户区坐标转换为视图区坐标
        /// </summary>
        /// <param name="p">客户区点坐标</param>
        /// <returns>视图区点坐标</returns>
        public virtual DCSystem_Drawing.Point ClientPointToView(DCSystem_Drawing.Point p)
        {
            this.RefreshScaleTransform();
            return this.Transform.TransformPoint(p);
        }

        /// <summary>
        /// 将视图区坐标转换为客户区坐标
        /// </summary>
        /// <param name="x">视图区X坐标</param>
        /// <param name="y">视图区Y坐标</param>
        /// <returns>客户区坐标</returns>
        public virtual DCSystem_Drawing.Point ViewPointToClient(int x, int y)
        {
            this.RefreshScaleTransform();
            return myTransform.UnTransformPoint(x, y);
        }

        /// <summary>
        /// 鼠标在控件上的位置
        /// </summary>
        public virtual DCSystem_Drawing.Point ClientMousePosition
        {
            get
            {
                DCSystem_Drawing.Point p = System.Windows.Forms.Control.MousePosition;
                //p = this.PointToClient(p);
                if (this.ClientRectangle.Contains(p))
                {
                    return p;
                }
                else
                {
                    return DCSystem_Drawing.Point.Empty;
                }
            }
        }

        /// <summary>
        /// 鼠标在视图区中的坐标
        /// </summary>
        public virtual DCSystem_Drawing.Point ViewMousePosition
        {
            get
            {
                DCSystem_Drawing.Point p = this.ClientMousePosition;
                if (p.IsEmpty == false)
                    return this.ClientPointToView(p);
                else
                    return DCSystem_Drawing.Point.Empty;
            }
        }

        protected DCSystem_Drawing.Rectangle _ViewBounds
            = DCSystem_Drawing.Rectangle.Empty;
        /// <summary>
        /// 整个视图区域的矩形区域
        /// </summary>
        public DCSystem_Drawing.Rectangle ViewBounds
        {
            get
            {
                return _ViewBounds;
            }
            set
            {
                if (_ViewBounds.Equals(value) == false)
                {
                    _ViewBounds = value;
                    this.UpdateViewBounds();
                }
            }
        }


        public virtual void UpdateViewBounds()
        {
            if (this.ViewBounds.IsEmpty)
            {
                return;
            }
            DCSystem_Drawing.Size size = new DCSystem_Drawing.Size(
                this.ViewBounds.Right - (int)this.ViewOffset.X,
                this.ViewBounds.Bottom - (int)this.ViewOffset.Y);
            this.RefreshScaleTransform();
            size = myTransform.UnTransformSize(size);
            this.Invalidate();
        }



        /// <summary>
        /// 已重载：鼠标按键按下事件处理
        /// </summary>
        /// <param name="e">事件参数</param>
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (this.myTransform.ContainsSourcePoint(e.X, e.Y))
            {
                DCSystem_Drawing.Point p = this.ClientPointToView(e.X, e.Y);
                OnViewMouseDown(
                    new System.Windows.Forms.MouseEventArgs(
                        e.Button,
                        e.Clicks,
                        p.X,
                        p.Y,
                        e.Delta));
            }
        }
        public System.Windows.Forms.MouseEventHandler ViewMouseDown = null;
        /// <summary>
        /// 鼠标按键在视图区中按下事件处理
        /// </summary>
        /// <param name="e">事件参数</param>
        protected virtual void OnViewMouseDown(MouseEventArgs e)
        {
            if (ViewMouseDown != null)
                ViewMouseDown(this, e);
        }

        /// <summary>
        /// 在OnMouseMove方法中是否执行了OnViewMouseMove方法的标记
        /// </summary>
        protected bool bolOnViewMouseMoveFlag = false;

        /// <summary>
        /// 已重载:鼠标移动事件处理
        /// </summary>
        /// <param name="e">事件参数</param>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            bolOnViewMouseMoveFlag = false;
            base.OnMouseMove(e);
            if (this.myTransform.ContainsSourcePoint(e.X, e.Y))
            {
                DCSystem_Drawing.Point p = this.ClientPointToView(e.X, e.Y);
                bolOnViewMouseMoveFlag = true;
                OnViewMouseMove(new System.Windows.Forms.MouseEventArgs(e.Button, e.Clicks, p.X, p.Y, e.Delta));
            }
        }

        public System.Windows.Forms.MouseEventHandler ViewMouseMove = null;
        /// <summary>
        /// 鼠标在视图区中的移动事件处理
        /// </summary>
        /// <param name="e">事件参数</param>
        protected virtual void OnViewMouseMove(MouseEventArgs e)
        {
            if (ViewMouseMove != null)
                ViewMouseMove(this, e);
        }

        /// <summary>
        /// 已重载:鼠标按键松开事件处理
        /// </summary>
        /// <param name="e">事件参数</param>
        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (this.myTransform.ContainsSourcePoint(e.X, e.Y))
            {
                DCSystem_Drawing.Point p = this.ClientPointToView(e.X, e.Y);
                OnViewMouseUp(new System.Windows.Forms.MouseEventArgs(e.Button, e.Clicks, p.X, p.Y, e.Delta));
            }
        }
        /// <summary>
        /// 视图区域中的鼠标按键松开事件
        /// </summary>
        public System.Windows.Forms.MouseEventHandler ViewMouseUp = null;
        /// <summary>
        /// 鼠标在视图区中的按键松开事件处理
        /// </summary>
        /// <param name="e">事件参数</param>
        protected virtual void OnViewMouseUp(MouseEventArgs e)
        {
            if (ViewMouseUp != null)
                ViewMouseUp(this, e);
        }


        /// <summary>
        /// 已重载:处理鼠标单击事件
        /// </summary>
        /// <param name="e"></param>
        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);
            DCSystem_Drawing.Point p = this.ViewMousePosition;
            OnViewClick(new System.Windows.Forms.MouseEventArgs(System.Windows.Forms.Control.MouseButtons, 1, p.X, p.Y, 0));
        }
        /// <summary>
        /// 鼠标在视图中的单击事件
        /// </summary>
        public System.Windows.Forms.MouseEventHandler ViewClick = null;
        /// <summary>
        /// 处理鼠标在视图区域中的单击事件
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnViewClick(System.Windows.Forms.MouseEventArgs e)
        {
            if (ViewClick != null)
                ViewClick(this, e);
        }

        /// <summary>
        /// 已重载:处理鼠标双击事件
        /// </summary>
        /// <param name="e">事件参数</param>
        protected override void OnMouseDoubleClick(MouseEventArgs e)
        {
            base.OnMouseDoubleClick(e);
            DCSystem_Drawing.Point p = this.ClientPointToView(e.X, e.Y);
            OnViewMouseDoubleClick(new MouseEventArgs(e.Button, e.Clicks, p.X, p.Y, e.Delta));
        }

        public MouseEventHandler ViewMouseDoubleClick = null;

        protected virtual void OnViewMouseDoubleClick(MouseEventArgs e)
        {
            if (ViewMouseDoubleClick != null)
            {
                ViewMouseDoubleClick(this, e);
            }
        }

        /// <summary>
        /// 已重载:重新绘制视图的事件处理
        /// </summary>
        /// <param name="e">事件参数</param>
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            {
                this.CheckZoomRate();
                this.RefreshScaleTransform();
                var e2 = CreatePaintEventArgs(
                    new InnerPaintEventArgs(e.Graphics, e.ClipRectangle),
                    this.Transform as SimpleRectangleTransform,
                    true);
                if (e2 != null)
                {
                    OnViewPaint(
                        e2,
                        this.Transform as SimpleRectangleTransform);
                }
            }
            //TransformPaint( e , this.myTransform as SimpleRectangleTransorm );
        }

        protected virtual InnerPaintEventArgs CreatePaintEventArgs(
            InnerPaintEventArgs e,
            SimpleRectangleTransform trans,
            bool checkClipRectangle)
        {
            if (trans == null)
            {
                return null;
            }

            DCSystem_Drawing.Rectangle rect = e.ClipRectangle;
            rect.Offset(-1, -1);
            rect.Width += 2;
            rect.Height += 2;
            //rect.X = (int)(rect.X * this.XZoomRate);
            //rect.Y = (int)(rect.Y * this.YZoomRate);
            //rect.Width = (int)(rect.Width * this.XZoomRate);
            //rect.Height = (int)(rect.Height * this.YZoomRate);
            if (checkClipRectangle)
            {
                var temp = DCSystem_Drawing.Rectangle.Intersect(trans.SourceRect, rect);
                rect.Y = temp.Top;
                rect.Height = temp.Height;
            }
            //if (rect.IsEmpty)
            //{
            //    return null ;
            //}

            DCSystem_Drawing.RectangleF rectf = trans.TransformRectangleF(
                rect.Left,
                rect.Top,
                rect.Width,
                rect.Height);// this.ClipRectangleToView( e.ClipRectangle );
            rect.X = (int)Math.Floor(rectf.Left);
            rect.Y = (int)Math.Floor(rectf.Top);
            //rect.Width = ( int ) System.Math.Ceiling( rectf.Width );
            //rect.Height = ( int ) System.Math.Ceiling( rectf.Height );
            rect.Width = (int)System.Math.Ceiling(rectf.Right) - rect.Left;
            rect.Height = (int)System.Math.Ceiling(rectf.Bottom) - rect.Top;

            e.Graphics.PageUnit = this.GraphicsUnit;
            e.Graphics.ResetTransform();

            //e.Graphics.TranslateTransform( trans.SourceRect.Left , trans.SourceRect.Top );

            e.Graphics.ScaleTransform(this.XZoomRate, this.YZoomRate);

            double rate = this.ClientToViewXRate;
            e.Graphics.TranslateTransform(
                (float)(trans.SourceRectF.Left * rate - trans.DescRectF.X),
                (float)(trans.SourceRectF.Top * rate - trans.DescRectF.Y));


            if (trans.XZoomRate < 1)
            {
                rect.Width = rect.Width + (int)System.Math.Ceiling(1 / trans.XZoomRate);
            }
            if (trans.YZoomRate < 1)
            {
                rect.Height = rect.Height + (int)System.Math.Ceiling(1 / trans.YZoomRate);
            }
            rect.Height += 6;
            var e2 =
                new InnerPaintEventArgs(
                e.Graphics,
                rect);

            e2.Graphics.ResetClip();
            int widthFix = GraphicsUnitConvert.Convert(2, GraphicsUnit.Pixel, e2.Graphics.PageUnit);
            e2.Graphics.SetClip(new DCSystem_Drawing.RectangleF(
                rect.Left - widthFix,
                rect.Top - widthFix,
                rect.Width + widthFix * 2,
                rect.Height + widthFix));
            return e2;
        }

        /// <summary>
        /// 重新绘制视图的事件处理
        /// </summary>
        /// <param name="e">事件参数</param>
        protected virtual void OnViewPaint(InnerPaintEventArgs e, SimpleRectangleTransform trans)
        {
            //if (ViewPaint != null)
            //{
            //    ViewPaint(this, e);
            //}
        }
        public virtual Graphics CreateViewGraphics()
        {
            Graphics g = this.CreateGraphics();
            g.PageUnit = this.GraphicsUnit;
            return g;
        }


        protected DCSystem_Drawing.Rectangle myInvalidateRect = DCSystem_Drawing.Rectangle.Empty;

        /// <summary>
        /// 根据当前控件的更新状态来修正坐标来修正无效矩形
        /// </summary>
        /// <param name="ViewRect">无效矩形</param>
        /// <returns>修正后的无效矩形</returns>
        protected virtual DCSystem_Drawing.Rectangle FixViewInvalidateRect(DCSystem_Drawing.Rectangle ViewRect)
        {
            if (myInvalidateRect.IsEmpty)
            {
                myInvalidateRect = ViewRect;
            }
            else if (ViewRect.IsEmpty == false)
            {
                myInvalidateRect = DCSystem_Drawing.Rectangle.Union(myInvalidateRect, ViewRect);
            }
            DCSystem_Drawing.Rectangle rect = myInvalidateRect;
            myInvalidateRect = DCSystem_Drawing.Rectangle.Empty;
            return rect;
        }

        /// <summary>
        /// 使用视图坐标来指定区域无效
        /// </summary>
        /// <param name="ViewBounds">无效区域</param>
        public virtual void ViewInvalidate(DCSystem_Drawing.Rectangle ViewBounds)
        {
            ViewBounds = this.FixViewInvalidateRect(ViewBounds);
            if (!ViewBounds.IsEmpty)
            {
                this.RefreshScaleTransform();
                DCSystem_Drawing.Rectangle rect = myTransform.UnTransformRectangle(ViewBounds);
                this.Invalidate(rect);
            }
        }


        protected class MyCapturer : MouseCapturer
        {
            public MyCapturer(System.Windows.Forms.Control ctl) : base(ctl)
            {

            }
            protected override CaptureMouseMoveEventArgs CreateArgs()
            {
                DocumentViewControl ctl = (DocumentViewControl)base.BindControl;
                CaptureMouseMoveEventArgs e = base.CreateArgs();
                e.StartPosition = ctl.MouseCaptureTransform.TransformPoint(e.StartPosition);
                e.CurrentPosition = ctl.MouseCaptureTransform.TransformPoint(e.CurrentPosition);
                return e;
            }
            public int DragStyle = 0;
            public DCSystem_Drawing.Rectangle[] Rects = null;
            public DCSystem_Drawing.Rectangle ViewClipRectangle = DCSystem_Drawing.Rectangle.Empty;
        }
        public delegate void CaptureMouseMoveFinishedEventHandler(DCSystem_Drawing.Point[] ps);

        /// <summary>
        /// 进行鼠标拖拽操作
        /// </summary>
        /// <param name="DrawFunction">鼠标拖拽期间的回调函数委托</param>
        /// <param name="ClipRectangle">使用视图坐标的剪切矩形</param>
        /// <returns>点坐标数组,包含开始拖拽和结束拖拽时鼠标的视图坐标位置</returns>
        public virtual DCSystem_Drawing.Point[] CaptureMouseMove(
            CaptureMouseMoveEventHandler DrawFunction,
            CaptureMouseMoveFinishedEventHandler finishedFunction,
            DCSystem_Drawing.Rectangle ClipRectangle,
            object Tag)
        {
            this.RefreshScaleTransform();

            MyCapturer mc = new MyCapturer(this);
            mc.Tag = Tag;
            if (ClipRectangle.IsEmpty == false)
            {
                ClipRectangle = myTransform.UnTransformRectangle(ClipRectangle);
                mc.ClipRectangle = ClipRectangle;
            }
            mc.ReversibleShape = ReversibleShapeStyle.Custom;
            if (DrawFunction != null)
            {
                mc.Draw = DrawFunction;
            }
            if (finishedFunction != null)
            {
                mc.EventOnFinished = new EventHandler(delegate (object sender4, EventArgs args4)
                {
                    DCSystem_Drawing.Point p1 = mc.StartPosition;
                    p1 = this.MouseCaptureTransform.TransformPoint(p1);

                    DCSystem_Drawing.Point p2 = mc.EndPosition;
                    p2 = this.MouseCaptureTransform.TransformPoint(p2);
                    finishedFunction(new DCSystem_Drawing.Point[] { p1, p2 });
                });
            }
            mc.CaptureMouseMove();
            return null;
        }




        #region 绘制可逆图形的成员群 ******************************************


        /// <summary>
        /// 使用视图坐标绘制一个可逆线段
        /// </summary>
        /// <param name="x1">线段起点X坐标</param>
        /// <param name="y1">线段起点Y坐标</param>
        /// <param name="x2">线段终点X坐标</param>
        /// <param name="y2">线段终点Y坐标</param>
        public virtual void ReversibleViewDrawLine(
            int x1,
            int y1,
            int x2,
            int y2)
        {
        }

        #endregion


        protected int ClientSizeWidth()
        {
            return base._ClientSizeWidth;
        }
        protected int ClientSizeHeight()
        {
            return base._ClientSizeHeight;
        }
        public override void Dispose()
        {
            if (this.myTransform != null)
            {
                this.myTransform.Dispose();
                this.myTransform = null;
            }
            this.myDefaultCursor = null;
            this.ViewClick = null;
            this.ViewMouseDoubleClick = null;
            this.ViewMouseDown = null;
            this.ViewMouseMove = null;
            this.ViewMouseUp = null;
        }
    }
}