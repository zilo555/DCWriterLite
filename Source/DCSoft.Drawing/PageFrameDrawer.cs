using System;
//using System.Drawing ;
// 袁永福到此一游
namespace DCSoft.Drawing
{
	/// <summary>
	/// 页面边框绘制对象
	/// </summary>
    public class PageFrameDrawer
	{
		/// <summary>
		/// 初始化对象
		/// </summary>
		public PageFrameDrawer()
		{
		}



        private float _ZoomRate = 1f;
        /// <summary>
        /// 图形缩放比率
        /// </summary>
        public float ZoomRate
        {
            get
            {
                return _ZoomRate; 
            }
            set
            {
                _ZoomRate = value; 
            }
        }


		private DCSystem_Drawing.Rectangle myBounds 
            = DCSystem_Drawing.Rectangle.Empty ;
		/// <summary>
		/// 对象边框
		/// </summary>
		public DCSystem_Drawing.Rectangle Bounds
		{
			get
            {
                return myBounds ;
            }
			set
            {
                myBounds = value;
            }
		}

        private DCSystem_Drawing.Rectangle _BodyBounds = DCSystem_Drawing.Rectangle.Empty;
        /// <summary>
        /// 文档正文边框
        /// </summary>
        public DCSystem_Drawing.Rectangle BodyBounds
        {
            get
            {
                return _BodyBounds; 
            }
            set
            {
                _BodyBounds = value; 
            }
        }
	
        private int _LeftMargin = 20;
        /// <summary>
		/// 左页边距
		/// </summary>
		public int LeftMargin
		{
			get
            {
                return _LeftMargin ;
            }
			set
            {
                _LeftMargin = value;
            }
		}

        private int _TopMargin = 30;
        /// <summary>
		/// 顶页边距
		/// </summary>
		public int TopMargin
		{
			get
            {
                return _TopMargin ;
            }
			set
            {
                _TopMargin = value;
            }
		}

        private int _RightMargin = 20;
        /// <summary>
		/// 右页边距
		/// </summary>
		public int RightMargin
		{
			get
            {
                return _RightMargin ;
            }
			set
            {
                _RightMargin = value;
            }
		}
        
        private int _BottomMargin = 40;
        /// <summary>
		/// 底页边距
		/// </summary>
		public int BottomMargin
		{
			get
            {
                return _BottomMargin ;
            }
			set
            {
                _BottomMargin = value;
            }
		}
        /// <summary>
        /// 页边距对象
        /// </summary>
        public  Margins Margins
		{
			get
			{
				return new  Margins( 
					this._LeftMargin , 
					this._RightMargin ,
					this._TopMargin ,
					this._BottomMargin );
			}
			set
			{
				this._LeftMargin = value.Left ;
				this._TopMargin = value.Top ;
				this._RightMargin = value.Right ;
				this._BottomMargin = value.Bottom ;
			}
		}

		private int _MarginLineLength = 60 ;
		/// <summary>
		/// 边距线长度
		/// </summary>
		public int MarginLineLength
		{
			get
            {
                return _MarginLineLength ;
            }
			set
            {
                _MarginLineLength = value;
            }
		}

		private bool _DrawMargin = true;
		/// <summary>
		/// 是否绘制边距线
		/// </summary>
		public bool DrawMargin
		{
			get
            {
                return _DrawMargin ;
            }
			set
            {
                _DrawMargin = value;
            }
		}

        private DCSystem_Drawing.Color _MarginLineColor
            = DCSystem_Drawing.Color.FromArgb(170, 170, 170);
		/// <summary>
		/// 边距线颜色
		/// </summary>
		public DCSystem_Drawing.Color MarginLineColor
		{
			get
            {
                return this._MarginLineColor ;
            }
			set
            {
                this._MarginLineColor = value;
            }
		}

        private bool _FillBackGround = true;
        /// <summary>
        /// 填充背景
        /// </summary>
        public bool FillBackGround
        {
            get
            {
                return _FillBackGround; 
            }
            set
            {
                _FillBackGround = value; 
            }
        }



		/// <summary>
		/// 使用指定图形绘制对象从指定位置开始绘制页面框架
		/// </summary>
		/// <param name="g">图形绘制对象</param>
		/// <param name="ClipRectangle">剪切矩形</param>
		public virtual void DrawPageFrame(
			DCGraphics g ,
             DCSystem_Drawing.Rectangle ClipRectangle )
		{
                if( ClipRectangle.IsEmpty || ClipRectangle.IntersectsWith( this.myBounds))
                {
                    if (this.DrawMargin 
						&& this.MarginLineColor.A != 0 
						&& this.MarginLineLength > 0 )
					{
                    // 绘制页边距标记线
                    DCSystem_Drawing.Rectangle rect = new DCSystem_Drawing.Rectangle(
							myBounds.Left + this.LeftMargin - 1,
							myBounds.Top + this.TopMargin ,
							myBounds.Width - this.LeftMargin - this.RightMargin + 2,
							myBounds.Height - this.TopMargin - this.BottomMargin );

                    DCSystem_Drawing.Point[] ps = new DCSystem_Drawing.Point[ 16 ];
						ps[0] = rect.Location ;
						ps[1].X = rect.Left - this.MarginLineLength ;
						ps[1].Y = rect.Top ;
				
						ps[2] = ps[0];
						ps[3].X = rect.Left ;
						ps[3].Y = rect.Top - this.MarginLineLength ;

						ps[4].X = rect.Right ;
						ps[4].Y = rect.Top ;
						ps[5].X = rect.Right + this.MarginLineLength;
						ps[5].Y = rect.Top ;

						ps[6] = ps[4];
						ps[7].X = rect.Right ;
						ps[7].Y = rect.Top - this.MarginLineLength ;

						ps[8].X = rect.Right ;
						ps[8].Y = rect.Bottom ;
						ps[9].X = rect.Right + this.MarginLineLength ;
						ps[9].Y = rect.Bottom ;

						ps[10] = ps[8];
						ps[11].X = rect.Right ;
						ps[11].Y = rect.Bottom + this.MarginLineLength ;

						ps[12].X = rect.Left ;
						ps[12].Y = rect.Bottom ;
						ps[13].X = rect.Left ;
						ps[13].Y = rect.Bottom + this.MarginLineLength ;

						ps[14] = ps[12];
						ps[15].X = rect.Left - this.MarginLineLength;
						ps[15].Y = rect.Bottom ;
                        using (var p = new Pen(this.MarginLineColor))
                        {
                            for (int iCount = 0; iCount < ps.Length; iCount += 2)
                            {
                                if (ps[iCount].X != int.MinValue)
                                {
                                    var p1 = ps[iCount];
                                    var p2 = ps[iCount + 1];
                                    if (DrawerUtil.IsLineInClipRectangle(ClipRectangle.ToFloat(), p1.X, p1.Y, p2.X, p2.Y))
                                    {
                                        g.DrawLine(p, p1.X, p1.Y, p2.X, p2.Y);
                                    }
                                }
                            }
                        }
					}//if( this.bolDrawMargin && this.intMarginLineColor.A != 0 )
				}//if( drawer.Draw( g , ClipRectangle ))
		}//public void DrawPageFrame()
	}//public class PageFrameDrawer
}