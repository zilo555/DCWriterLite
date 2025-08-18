using System;
// // 
using DCSoft.Drawing;

namespace DCSoft.WinForms
{
    
	/// <summary>
	/// 绘制和辅助处理8个点的控制矩形的对象
	/// </summary>
    public class DragRectangle
	{
		/// <summary>
		/// 无作为的初始化对象
		/// </summary>
		public DragRectangle()
		{
		}
		/// <summary>
		/// 初始化对象
		/// </summary>
		/// <param name="vBounds">矩形对象</param>
		/// <param name="vInnerRect">是否是内置拖拽控制点</param>
		public DragRectangle(DCSystem_Drawing.Rectangle vBounds , bool vInnerRect )
		{
			myBounds = vBounds ;
			bolInnerDragRect = vInnerRect ;
			this.Refresh();
		}

		private DCSystem_Drawing.Rectangle myBounds = DCSystem_Drawing.Rectangle.Empty ;
		private readonly DCSystem_Drawing.Rectangle[] myDragRect = new DCSystem_Drawing.Rectangle[8];
		/// <summary>
		/// 拖拽控制点的大小
		/// </summary>
		public static int DragRectSize = 6 ;
		private bool bolInnerDragRect = false;
		 
		private bool bolCanMove = true;
		private bool bolFocus = true;
		private bool bolBoundsBorder = true;

        private ResizeableType _Resizeable = ResizeableType.WidthAndHeight;
        /// <summary>
        /// 能否改变大小的类型
        /// </summary>
        public ResizeableType Resizeable
        {
            get
            {
                return _Resizeable; 
            }
            set
            {
                _Resizeable = value; 
            }
        }

		/// <summary>
		/// 是否获得焦点(当前对象)
		/// </summary>
		public bool Focus
		{
			get{ return bolFocus ;}
			set{ bolFocus = value;}
		}

		/// <summary>
		/// 对象边框
		/// </summary>
		public DCSystem_Drawing.Rectangle Bounds
		{
			get{ return myBounds;}
			set{ myBounds = value; this.Refresh() ;}
		}
		/// <summary>
		/// 是否能移动对象
		/// </summary>
		public bool CanMove
		{
			get{ return bolCanMove ;}
			set{ bolCanMove = value;}
		}
        public static DCSystem_Drawing.Color ActiveDragRectBackColor = DCSystem_Drawing.Color.Blue;
        public static DCSystem_Drawing.Color ActiveDragRectBorderColor = DCSystem_Drawing.Color.LightGray;
        public static DCSystem_Drawing.Color DisableDragRectBackColor = DCSystem_Drawing.Color.White;
        public static DCSystem_Drawing.Color DisableDragRectBorderColor = DCSystem_Drawing.Color.LightGray;

		/// <summary>
		/// 计算指定矩形的拖拽控制矩形
		/// </summary>
		/// <remarks>
		/// 拖拽矩形主要用于有用户参与的图形化用户界面,在一个矩形区域的的4个顶
		/// 点和边框中间点共有8个控制点,用户使用鼠标拖拽操作来拖动这8个控制点
		/// 可以用于改变矩形区域的位置和大小,这些控制点可以在区域区域的内部,
		/// 也可在矩形区域的外部,拖拽矩形有8个,分别编号从0至7
		/// <pre>
		///               内拖拽矩形
		/// ┌─────────────────┐
		/// │■0            1■             2■│
		/// │                                  │
		/// │                                  │
		/// │                                  │
		/// │                                  │
		/// │■7                            3■│
		/// │                                  │
		/// │                                  │
		/// │                                  │
		/// │                                  │
		/// │■6           5■              4■│
		/// └─────────────────┘
		/// 
		///              外拖拽矩形
		///              
		/// ■               ■                  ■
		///   ┌────────────────┐
		///   │0            1                 2│
		///   │                                │
		///   │                                │
		///   │                                │
		///   │                                │
		/// ■│7                              3│■ 
		///   │                                │
		///   │                                │
		///   │                                │
		///   │                                │
		///   │6             5               4 │
		///   └────────────────┘
		/// ■                ■                 ■
		/// </pre>
		/// </remarks>
		public void Refresh()
		{
			if( bolInnerDragRect)
			{
				myDragRect[0] = new DCSystem_Drawing.Rectangle( 
					myBounds.X ,
					myBounds.Y , 
					DragRectSize ,
					DragRectSize );
				myDragRect[1] = new DCSystem_Drawing.Rectangle( 
					myBounds.X + (int)((myBounds.Width - DragRectSize)/2) , 
					myBounds.Y , 
					DragRectSize ,
					DragRectSize );
				myDragRect[2] = new DCSystem_Drawing.Rectangle( 
					myBounds.Right - DragRectSize , 
					myBounds.Y , 
					DragRectSize ,
					DragRectSize );
				myDragRect[3] = new DCSystem_Drawing.Rectangle(
					myBounds.Right - DragRectSize , 
					myBounds.Y + (int)(( myBounds.Height - DragRectSize)/2)  , 
					DragRectSize , 
					DragRectSize );
				myDragRect[4] = new DCSystem_Drawing.Rectangle( 
					myBounds.Right - DragRectSize ,
					myBounds.Bottom - DragRectSize , 
					DragRectSize ,
					DragRectSize );
				myDragRect[5] = new DCSystem_Drawing.Rectangle( 
					myBounds.X + (int)((myBounds.Width - DragRectSize)/2) ,
					myBounds.Bottom - DragRectSize ,
					DragRectSize ,
					DragRectSize );
				myDragRect[6] = new DCSystem_Drawing.Rectangle( 
					myBounds.X  , 
					myBounds.Bottom - DragRectSize , 
					DragRectSize ,
					DragRectSize );
				myDragRect[7] = new DCSystem_Drawing.Rectangle(
					myBounds.X  , 
					myBounds.Y + (int)(( myBounds.Height - DragRectSize)/2 ) , 
					DragRectSize ,
					DragRectSize );
			}
			else
			{
				myDragRect[0] = new DCSystem_Drawing.Rectangle( 
					myBounds.X - DragRectSize ,
					myBounds.Y - DragRectSize ,
					DragRectSize , 
					DragRectSize );
				myDragRect[1] = new DCSystem_Drawing.Rectangle(
					myBounds.X + (int)((myBounds.Width - DragRectSize)/2) , 
					myBounds.Y - DragRectSize , 
					DragRectSize ,
					DragRectSize );
				myDragRect[2] = new DCSystem_Drawing.Rectangle( 
					myBounds.Right  ,
					myBounds.Y - DragRectSize ,
					DragRectSize ,
					DragRectSize );
				myDragRect[3] = new DCSystem_Drawing.Rectangle(
					myBounds.Right  ,
					myBounds.Y + (int)(( myBounds.Height - DragRectSize)/2)  , 
					DragRectSize , 
					DragRectSize );
				myDragRect[4] = new DCSystem_Drawing.Rectangle( 
					myBounds.Right  , 
					myBounds.Bottom  ,
					DragRectSize ,
					DragRectSize );
				myDragRect[5] = new DCSystem_Drawing.Rectangle( 
					myBounds.X + (int)((myBounds.Width - DragRectSize)/2) ,
					myBounds.Bottom  ,
					DragRectSize ,
					DragRectSize );
				myDragRect[6] = new DCSystem_Drawing.Rectangle( 
					myBounds.X - DragRectSize ,
					myBounds.Bottom  ,
					DragRectSize ,
					DragRectSize );
				myDragRect[7] = new DCSystem_Drawing.Rectangle( 
					myBounds.X - DragRectSize  ,
					myBounds.Y + (int)(( myBounds.Height - DragRectSize)/2 ) , 
					DragRectSize , 
					DragRectSize );
			}
		}//public void Refresh()


		/// <summary>
		/// 绘制拖拉矩形
		/// </summary>
		/// <param name="g">绘制对象</param>
		/// <param name="Rect">矩形区域</param>
		/// <param name="vFocus">是否是焦点绘制模式</param>
        /// <param name="resizeable">是否可以改变大小</param>
		public static void DrawDragRect( 
			DCGraphics g ,
             DCSystem_Drawing.Rectangle Rect , 
			bool vFocus ,
            DragPointStyle point ,
			ResizeableType resizeable ,
            bool canMove )
		{
            bool enabled = GetDragPointEnable( point , resizeable , canMove );
			 var color = DCSystem_Drawing.Color.Blue ;
            if (enabled)
            {
                if (vFocus)
                {
                    color = DCSystem_Drawing.Color.Blue;
                }
                else
                {
                    color = DCSystem_Drawing.Color.Black;
                }
            }
            else
            {
                color = DCSystem_Drawing.Color.White;
            }
            //g.FillRectangle(color, Rect);
            using(  SolidBrush myBrush = new  SolidBrush( color ))
            {
                g.FillRectangle( myBrush , Rect );
            }
            if (enabled)
            {
                if (vFocus)
                {
                    color = DCSystem_Drawing.Color.White;
                }
                else
                {
                    color = DCSystem_Drawing.Color.Blue;
                }
            }
            else
            {
                color = DCSystem_Drawing.Color.Black;
            }
			using (var p = new Pen(color))
			{
				g.DrawRectangle(p, Rect.Left, Rect.Top, Rect.Width, Rect.Height);
			}
            //using(  Pen myPen = new  Pen ( color ))
            //{
            //    g.DrawRectangle( myPen , Rect );
            //}
		}


        private  DashStyle _LineDashStyle =  DashStyle.Dash;
        /// <summary>
        /// 边框线虚线样式
        /// </summary>
        public  DashStyle LineDashStyle
        {
            get { return _LineDashStyle; }
            set { _LineDashStyle = value; }
        }

		/// <summary>
		/// 绘制拖拽矩形,本函数根据主矩形区域计算8个拖拽矩形区域并用指定的颜色
		/// 填充和绘制边框,本函数不绘制主矩形区域
		/// </summary>
        /// <param name="myGraph">图像绘制对象</param>
        public void RefreshView(DCGraphics myGraph)
        {
            if (myGraph != null)
            {
                if (bolBoundsBorder)
                {
      //              myGraph.DrawRectangle(
						//new SimplePenValue( Color.Black, 1, this.LineDashStyle), 
						//myBounds.Left , 
						//myBounds.Top , 
						//myBounds.Width ,
						//myBounds.Height);
                    using ( Pen myPen =
                               new  Pen(DCSystem_Drawing.Color.Black))
                    {
                        myPen.DashStyle = this.LineDashStyle;
                        myGraph.DrawRectangle(myPen, myBounds);
                    }
                }
                DrawDragRect(myGraph, myDragRect[0], this.Focus, DragPointStyle.LeftTop, this.Resizeable, this.CanMove);
                DrawDragRect(myGraph, myDragRect[1], this.Focus, DragPointStyle.TopCenter, this.Resizeable, this.CanMove);
                DrawDragRect(myGraph, myDragRect[2], this.Focus, DragPointStyle.TopRight, this.Resizeable, this.CanMove);
                DrawDragRect(myGraph, myDragRect[3], this.Focus, DragPointStyle.RightCenter , this.Resizeable, this.CanMove);
                DrawDragRect(myGraph, myDragRect[4], this.Focus, DragPointStyle.RightBottom , this.Resizeable, this.CanMove);
                DrawDragRect(myGraph, myDragRect[5], this.Focus, DragPointStyle.BottomCenter, this.Resizeable, this.CanMove);
                DrawDragRect(myGraph, myDragRect[6], this.Focus, DragPointStyle.LeftBottom, this.Resizeable, this.CanMove);
                DrawDragRect(myGraph, myDragRect[7], this.Focus, DragPointStyle.LeftCenter, this.Resizeable, this.CanMove);
            }
        }// void DrawDragRect()

		/// <summary>
		/// 判断指定拖拽点是否可用
		/// </summary>
		/// <param name="point">拖拽点样式</param>
		/// <returns>是否可用</returns>
		private bool DragRectEnable( DragPointStyle point )
		{
            return GetDragPointEnable(point, this.Resizeable, this.CanMove);
		}//bool DragRectEnable()

        private static bool GetDragPointEnable(DragPointStyle point, ResizeableType resizeable , bool canMove  )
        {
            switch (point)
            {
                case DragPointStyle.Move:
                    return canMove ;
                case DragPointStyle.LeftTop:
                    return canMove && resizeable == ResizeableType.WidthAndHeight ;
                case DragPointStyle.TopCenter:
                    return canMove && ( resizeable == ResizeableType.WidthAndHeight || resizeable == ResizeableType.Height );
                case DragPointStyle.TopRight:
                    return canMove && resizeable == ResizeableType.WidthAndHeight ;
                case DragPointStyle.RightCenter:
                    return resizeable == ResizeableType.WidthAndHeight || resizeable == ResizeableType.Width;
                case DragPointStyle.RightBottom:
                    return resizeable == ResizeableType.WidthAndHeight;
                case DragPointStyle.BottomCenter:
                    return resizeable == ResizeableType.Height || resizeable == ResizeableType.WidthAndHeight;
                case DragPointStyle.LeftBottom:
                    return canMove && resizeable == ResizeableType.WidthAndHeight;
                case DragPointStyle.LeftCenter:
                    return canMove && ( resizeable == ResizeableType.WidthAndHeight || resizeable == ResizeableType.Width );
                default:
                    return false;
            }
        }

		/// <summary>
		/// 计算指定坐标在对象中的区域 -1:在对象区域中,0-7:在对象的某个拖拉矩形中,-2:不在对象中或不需要进行拖拽操作
		/// </summary>
		/// <param name="x">X坐标</param>
		/// <param name="y">Y坐标</param>
		/// <returns>控制号</returns>
        public DragPointStyle DragHit(int x, int y)
        {
            if (this.Resizeable == ResizeableType.FixSize && myBounds.Contains(x, y))
            {
                return DragPointStyle.Move;
            }
            for (int iCount = 0; iCount < 8; iCount++)
            {
                if (myDragRect[iCount].Contains(x, y))
                {
                    if (this.DragRectEnable((DragPointStyle)iCount))
                    {
                        return (DragPointStyle)iCount;
                    }
                    else
                    {
                        return DragPointStyle.None;
                    }
                }
            }
            if (myBounds.Contains(x, y) && this.CanMove)
            {
                return DragPointStyle.Move;
            }
            else
            {
                return DragPointStyle.None;
            }
        }

		/// <summary>
		/// 移动矩形
		/// </summary>
		/// <param name="dx">横向移动量</param>
		/// <param name="dy">纵向移动量</param>
		/// <param name="DragStyle">移动方式</param>
		/// <param name="SourceRect">原始矩形</param>
		/// <returns>移动后的矩形</returns>
		public static DCSystem_Drawing.Rectangle CalcuteDragRectangle( 
			int dx , 
			int dy , 
			DragPointStyle DragStyle ,
             DCSystem_Drawing.Rectangle SourceRect )
		{
			// 中间
			if(DragStyle == DragPointStyle.Move )
				SourceRect.Offset(dx,dy);
			// 左边
			if(DragStyle == DragPointStyle.LeftTop 
                || DragStyle == DragPointStyle.LeftCenter
                || DragStyle == DragPointStyle.LeftBottom )
			{
				SourceRect.Offset(dx,0);
				SourceRect.Width = SourceRect.Width - dx;
			}
			// 顶边
			if(DragStyle == DragPointStyle.LeftTop
                || DragStyle == DragPointStyle.TopCenter
                || DragStyle == DragPointStyle.TopRight )
			{
				SourceRect.Offset(0,dy);
				SourceRect.Height = SourceRect.Height -dy;
			}
			// 右边
			if(DragStyle == DragPointStyle.TopRight
                || DragStyle == DragPointStyle.RightCenter
                || DragStyle == DragPointStyle .RightBottom )
			{
				SourceRect.Width = SourceRect.Width + dx;
			}
			// 底边
			if(DragStyle == DragPointStyle.RightBottom
                || DragStyle == DragPointStyle.BottomCenter 
                || DragStyle == DragPointStyle.LeftBottom )
			{
				SourceRect.Height = SourceRect.Height + dy;
			}
			return SourceRect ;
		}

		/// <summary>
		/// 计算拖拉矩形上的鼠标光标位置
		/// </summary>
		/// <remarks>
		/// 鼠标设置如下
		/// 西北-东南          南北                东北-西南
		///	   ■               ■                  ■
		///     ┌────────────────┐
		///     │0            1                 2│
		///     │                                │
		///     │                                │
		///     │                                │
		///     │                                │
		///   ■│7 西-南                        3│■ 西-南
		///     │                                │
		///     │                                │
		///     │                                │
		///     │                                │
		///     │6             5               4 │
		///     └────────────────┘
		///   ■                ■                 ■
		/// 东北-西南          南北                   西北-东南
		/// </remarks>
		/// <param name="point">拖拽矩形的序号,从0至7</param>
		/// <returns>鼠标光标对象,若序号小于0或大于7则返回空引用</returns>
		public static System.Windows.Forms.Cursor GetMouseCursor( DragPointStyle point )
		{
			switch( point )
			{
				case DragPointStyle.Move : 
					return System.Windows.Forms.Cursors.Arrow ;
				case DragPointStyle.LeftTop :
					return System.Windows.Forms.Cursors.SizeNWSE ;
				case  DragPointStyle.TopCenter :
					return System.Windows.Forms.Cursors.SizeNS ;
				case DragPointStyle.TopRight :
					return System.Windows.Forms.Cursors.SizeNESW ;
				case DragPointStyle.RightCenter :
					return System.Windows.Forms.Cursors.SizeWE ;
				case DragPointStyle.RightBottom :
					return System.Windows.Forms.Cursors.SizeNWSE ;
				case  DragPointStyle.BottomCenter :
					return System.Windows.Forms.Cursors.SizeNS ;
				case DragPointStyle.LeftBottom :
					return System.Windows.Forms.Cursors.SizeNESW ;
				case DragPointStyle.LeftCenter :
					return System.Windows.Forms.Cursors.SizeWE ;
				default:
					return System.Windows.Forms.Cursors.Default ;
			}
			//return null;
		}

        public static DCSystem_Drawing.Point GetPoint(DCSystem_Drawing.Rectangle rect, DragPointStyle ps)
        {
            int x = rect.Left ;
            int y = rect.Top ;
            switch (ps)
            {
                case DragPointStyle.LeftTop:
                    x = rect.Left;
                    y = rect.Top;
                    break;
                case DragPointStyle.TopCenter:
                    x = rect.Left + rect.Width / 2;
                    y = rect.Top;
                    break;
                case DragPointStyle.TopRight:
                    x = rect.Right;
                    y = rect.Top;
                    break;
                case DragPointStyle.RightCenter:
                    x = rect.Right;
                    y = rect.Top + rect.Height / 2;
                    break;
                case DragPointStyle.RightBottom:
                    x = rect.Right;
                    y = rect.Bottom;
                    break;
                case DragPointStyle.BottomCenter:
                    x = rect.Left + rect.Width / 2;
                    y = rect.Bottom;
                    break;
                case DragPointStyle.LeftBottom:
                    x = rect.Left;
                    y = rect.Bottom;
                    break;
                case DragPointStyle.LeftCenter:
                    x = rect.X;
                    y = rect.Top + rect.Height / 2;
                    break;
            }
            return new DCSystem_Drawing.Point(x, y);
        }
	}//public class DragRectangle

    /// <summary>
    /// 拖拽点样式
    /// </summary>
    public enum DragPointStyle
    {
        /// <summary>
        /// 无效拖拽点
        /// </summary>
        None = -2,
        /// <summary>
        /// 进行移动的拖拽点，可改变图形位置
        /// </summary>
        Move = -1,
        /// <summary>
        /// 左上角拖拽点
        /// </summary>
        LeftTop = 0,
        /// <summary>
        /// 上边中央拖拽点
        /// </summary>
        TopCenter = 1,
        /// <summary>
        /// 右上角拖拽点
        /// </summary>
        TopRight = 2,
        /// <summary>
        /// 右边中央拖拽点
        /// </summary>
        RightCenter = 3,
        /// <summary>
        /// 右下角拖拽点
        /// </summary>
        RightBottom = 4,
        /// <summary>
        /// 底边中央拖拽点
        /// </summary>
        BottomCenter = 5,
        /// <summary>
        /// 左下角拖拽点
        /// </summary>
        LeftBottom = 6,
        /// <summary>
        /// 左边中央拖拽点
        /// </summary>
        LeftCenter = 7
    }
}