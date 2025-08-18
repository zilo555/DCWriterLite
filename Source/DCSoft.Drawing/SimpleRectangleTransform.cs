using DCSoft.Printing;
using DCSoft.Writer.Dom;
using System;
// // 

namespace DCSoft.Drawing
{
	/// <summary>
	/// 简单矩形坐标转换对象
	/// </summary>
    public class SimpleRectangleTransform : TransformBase
	{
		/// <summary>
		/// 无作为的初始化对象
		/// </summary>
		public SimpleRectangleTransform()
		{
		}
		public override void Dispose()
		{
			this._DocumentObject = null;
			this._PageObject = null;
			
		}
		private PageContentPartyStyle _ContentStyle = PageContentPartyStyle.Body;
        /// <summary>
        /// 内容样式
        /// </summary>
        public PageContentPartyStyle ContentStyle
        {
            get { return _ContentStyle; }
            set { _ContentStyle = value; }
        }

        private bool _Enable = true;
		/// <summary>
		/// 对象是否可用
		/// </summary>
		public bool Enable
		{
			get
			{
				return _Enable ;
			}
			set
			{
				_Enable = value;
			}
		}

        private bool _DrawMaskWhenDisable = true;
        /// <summary>
        /// 当对象不可用时是否绘制遮盖颜色
        /// </summary>
        public bool DrawMaskWhenDisable
        {
            get
            {
                return _DrawMaskWhenDisable; 
            }
            set
            {
                _DrawMaskWhenDisable = value; 
            }
        }

		private bool _Visible = true;
		/// <summary>
		/// 对象是否可见
		/// </summary>
		public bool Visible
		{
			get
			{
				return _Visible ;
			}
			set
			{
				_Visible = value;
			}
		}

        private PrintPage _PageObject = null;
		/// <summary>
		/// 对象所属页面对象
		/// </summary>
		public PrintPage PageObject
		{
			get
			{
                return _PageObject;
			}
			set
			{
                _PageObject = value;
			}
		}
        private DomDocument _DocumentObject = null;
        /// <summary>
        /// 对象所属文档对象
        /// </summary>
        public DomDocument DocumentObject
        {
            get
            {
                return _DocumentObject; 
            }
            set
            {
                _DocumentObject = value; 
            }
        }

		private int _PageIndex = 0 ;
        /// <summary>
        /// 对象所属的从0开始的页码
        /// </summary>
		public int PageIndex
		{
			get
			{
				return _PageIndex ;
			}
			set
			{
				_PageIndex = value;
			}
		}

        //private int intFlag2 = 0 ;
        //public int Flag2
        //{
        //    get
        //    {
        //        return intFlag2 ;
        //    }
        //    set
        //    {
        //        intFlag2 = value;
        //    }
        //}

        //private int _Flag3 = 0 ;
        //      /// <summary>
        //      /// 标志3
        //      /// </summary>
        //public int Flag3
        //{
        //	get
        //	{
        //		return _Flag3 ;
        //	}
        //	set
        //	{
        //		_Flag3 = value;
        //	}
        //}

        internal DCSystem_Drawing.RectangleF _SourceRectF = DCSystem_Drawing.RectangleF.Empty ;
        /// <summary>
        /// 原始区域矩形边框，一般表示控件客户区中的位置
        /// </summary>
        public DCSystem_Drawing.RectangleF SourceRectF
		{
			get
			{
				return _SourceRectF ;
			}
			set
			{
				_SourceRectF = value;
			}
		}

        public void SourceRectFOffset( float dx , float dy )
        {
            this._SourceRectF.Offset(dx, dy);
        }


        private DCSystem_Drawing.PointF _SourceRectOffset = new DCSystem_Drawing.PointF(0, 0);
        /// <summary>
        /// 原始矩形偏移量
        /// </summary>
        public DCSystem_Drawing.PointF SourceRectOffset
        {
            get
            {
                return _SourceRectOffset; 
            }
            set
            {
                _SourceRectOffset = value; 
            }
        }

        //public void OffsetSourceRectF(float x, float y)
        //{
        //    this._SourceRect.Offset(x, y);
        //}

        /// <summary>
        /// 原始区域矩形边框，一般表示控件客户区中的位置
        /// </summary>
        public DCSystem_Drawing.Rectangle SourceRect
		{
			get
			{
				return new DCSystem_Drawing.Rectangle(
					( int ) _SourceRectF.Left ,
					( int ) _SourceRectF.Top , 
					( int ) _SourceRectF.Width , 
					( int ) _SourceRectF.Height );
			}
			set
			{
				_SourceRectF = new DCSystem_Drawing.RectangleF(
					value.Left , 
					value.Top , 
					value.Width ,
					value.Height );
			}
		}


        internal DCSystem_Drawing.Rectangle _PartialAreaSourceBounds = DCSystem_Drawing.Rectangle.Empty;
        /// <summary>
        /// 页眉分割区域的边界，一般用于保存分页文档处理
        /// </summary>
        public DCSystem_Drawing.Rectangle PartialAreaSourceBounds
        {
            get
            {
                if (_PartialAreaSourceBounds.IsEmpty)
                {
                    return this.SourceRect;
                }
                else
                {
                    return _PartialAreaSourceBounds;
                }
            }
            set 
            {
                _PartialAreaSourceBounds = value; 
            }
        }

        private DCSystem_Drawing.RectangleF _PageClipBounds = DCSystem_Drawing.RectangleF.Empty;
        /// <summary>
        /// 页面区域边界。
        /// </summary>
        public DCSystem_Drawing.RectangleF PageClipBounds
        {
            get
            {
                return this._PageClipBounds;
            }
            set
            {
                this._PageClipBounds = value;
            }
        }

        internal DCSystem_Drawing.RectangleF _DescRectF = DCSystem_Drawing.RectangleF.Empty ;
        /// <summary>
        /// 目标区域矩形边框，一般表示文档视图区中的位置
        /// </summary>
        public DCSystem_Drawing.RectangleF DescRectF
		{
			get
			{
				return _DescRectF ;
			}
			set
			{
				_DescRectF = value;
			}
		}
		public  float DescRectFTop
		{
			get
			{
				return this._DescRectF.Top;
			}
		}
        public bool DescRectFIntersectsWith(DCSystem_Drawing.RectangleF rect )
        {
            return this._DescRectF.IntersectsWith(rect);
        }

        /// <summary>
        /// 目标区域矩形边框，一般表示文档视图区中的位置
        /// </summary>
        public DCSystem_Drawing.Rectangle DescRect
		{
			get
			{
				return new DCSystem_Drawing.Rectangle(
					 ( int ) _DescRectF.Left , 
					 ( int ) _DescRectF.Top ,
					 ( int ) _DescRectF.Width , 
					 ( int ) _DescRectF.Height ); 
			}
			set
			{ 
				_DescRectF = new DCSystem_Drawing.RectangleF( 
					value.Left ,
					value.Top ,
					value.Width ,
					value.Height );
			}
		}
  //      /// <summary>
  //      /// 偏移量
  //      /// </summary>
		//public  Point OffsetPosition
		//{
		//	get
		//	{
		//		return new  Point( 
		//			( int ) ( _DescRect.Left - _SourceRect.Left - this._SourceRectOffset.X ) ,
		//			( int ) ( _DescRect.Top - _SourceRect.Top - this._SourceRectOffset.Y ) );
		//	}
		//}
  //      /// <summary>
  //      /// 偏移量
  //      /// </summary>
		//public  PointF OffsetPositionF
		//{
		//	get
		//	{
		//		return new  PointF( 
		//			_DescRect.Left - _SourceRect.Left - this._SourceRectOffset.X ,
		//			_DescRect.Top - _SourceRect.Top - this._SourceRectOffset.Y );
		//	}
		//}
        /// <summary>
        /// X方向缩放比率
        /// </summary>
		public float XZoomRate
		{
			get
			{
				float rate = _DescRectF.Width ;
				return rate / _SourceRectF.Width ;
			}
		}
        /// <summary>
        /// Y方向缩放比率
        /// </summary>
		public float YZoomRate
		{
			get
			{
				float rate = _DescRectF.Height ;
				return rate / _SourceRectF.Height ;
			}
		}

        /// <summary>
        /// 判断指定的点是否在源区域中
        /// </summary>
        /// <param name="x">点X坐标</param>
        /// <param name="y">点Y坐标</param>
        /// <returns>是否在源区域中</returns>
		public override bool ContainsSourcePoint(int x, int y)
		{
			return this._SourceRectF.Contains( x - this._SourceRectOffset.X , y - this._SourceRectOffset.Y );
		}

		/// <summary>
		/// 将原始区域的点转换为目标区域中的点
		/// </summary>
		/// <param name="x">原始区域中的点的X坐标</param>
		/// <param name="y">原始区域的点的Y坐标</param>
		/// <returns>目标区域中的点坐标</returns>
		public override DCSystem_Drawing.Point TransformPoint( int x , int y )
		{
            DCSystem_Drawing.PointF p = TransformPointF( ( float ) x , ( float ) y );
			return new DCSystem_Drawing.Point( ( int) p.X , ( int ) p.Y );
		}
		/// <summary>
		/// 将原始区域的点转换为目标区域中的点
		/// </summary>
		/// <param name="x">原始区域中的点的X坐标</param>
		/// <param name="y">原始区域的点的Y坐标</param>
		/// <returns>目标区域中的点坐标</returns>
		public override DCSystem_Drawing.PointF TransformPointF( float x , float y )
		{
			x = x - this._SourceRectF.Left - this._SourceRectOffset.X  ;
			y = y - this._SourceRectF.Top - this._SourceRectOffset.Y  ;
            if (this._SourceRectF.Width != this._DescRectF.Width && this._SourceRectF.Width != 0)
            {
                x = x * this._DescRectF.Width / this._SourceRectF.Width;
            }
            if (this._SourceRectF.Height != this._DescRectF.Height && this._SourceRectF.Height != 0)
            {
                y = y * this._DescRectF.Height / this._SourceRectF.Height;
            }
			return new DCSystem_Drawing.PointF( x + this._DescRectF.Left , y + this._DescRectF.Top );
		}
        /// <summary>
        /// 将原始区域重的大小转换未目标区域中的大小
        /// </summary>
        /// <param name="w">原始区域宽度</param>
        /// <param name="h">原始区域高度</param>
        /// <returns>目标区域中的大小</returns>
        public override DCSystem_Drawing.SizeF TransformSizeF( float w , float h )
		{
			if( _SourceRectF.Width != _DescRectF.Width && _SourceRectF.Width != 0 )
				w = w * _DescRectF.Width / _SourceRectF.Width ;
			if( _SourceRectF.Height != _DescRectF.Height && _SourceRectF.Height != 0 )
				h = h * _DescRectF.Height / _SourceRectF.Height ;
			return new DCSystem_Drawing.SizeF( w , h );
		}

		/// <summary>
		/// 将目标区域中的坐标转换为原始区域的坐标
		/// </summary>
		/// <param name="p">目标区域中的坐标</param>
		/// <returns>原始区域的坐标</returns>
		public override DCSystem_Drawing.Point UnTransformPoint(DCSystem_Drawing.Point p )
		{
            DCSystem_Drawing.PointF p1 = UnTransformPointF( ( float ) p.X  , ( float ) p.Y );
			return new DCSystem_Drawing.Point( ( int ) p1.X , ( int ) p1.Y );
		}
		/// <summary>
		/// 将目标区域中的坐标转换为原始区域中的坐标
		/// </summary>
		/// <param name="x">目标区域中的X坐标</param>
		/// <param name="y">目标区域中的Y坐标</param>
		/// <returns>原始区域中的坐标</returns>
		public override DCSystem_Drawing.Point UnTransformPoint( int x , int y )
		{
            DCSystem_Drawing.PointF p1 = UnTransformPointF( ( float ) x , ( float ) y );
			return new DCSystem_Drawing.Point( ( int ) p1.X , ( int ) p1.Y );
		}
		/// <summary>
		/// 将目标区域中的坐标转换为原始区域中的坐标
		/// </summary>
		/// <param name="x">目标区域中的X坐标</param>
		/// <param name="y">目标区域中的Y坐标</param>
		/// <returns>原始区域中的坐标</returns>
		public override DCSystem_Drawing.PointF UnTransformPointF( float x , float y )
		{
			x = x - this._DescRectF.Left ;
			y = y - this._DescRectF.Top ;
            if (this._SourceRectF.Width != this._DescRectF.Width && this._DescRectF.Width != 0)
            {
                x = x * this._SourceRectF.Width / this._DescRectF.Width;
            }
            if (this._SourceRectF.Height != this._DescRectF.Height && this._DescRectF.Height != 0)
            {
                y = y * this._SourceRectF.Height / this._DescRectF.Height;
            }
			return new DCSystem_Drawing.PointF(
                x + this._SourceRectF.Left + this._SourceRectOffset.X ,
                y + this._SourceRectF.Top + this._SourceRectOffset.Y  );
		}
		/// <summary>
		/// 将目标区域中的大小转换为原始区域中的大小
		/// </summary>
		/// <param name="w">目标区域宽度</param>
        /// <param name="h">目标区域高度</param>
		/// <returns>原始区域中的大小</returns>
		public override DCSystem_Drawing.Size UnTransformSize( int w , int h )
		{
			if( _SourceRectF.Width != _DescRectF.Width && _DescRectF.Width != 0 )
				w = ( int ) (  w * _SourceRectF.Width / _DescRectF.Width ) ;
			if( _SourceRectF.Height != _DescRectF.Height && _DescRectF.Height != 0 )
				h = ( int ) (  h * _SourceRectF.Height / _DescRectF.Height ) ;
			return new DCSystem_Drawing.Size( w , h );
		}
		/// <summary>
		/// 将目标区域中的大小转换为原始区域中的大小
		/// </summary>
		/// <param name="w">目标区域宽度</param>
        /// <param name="h">目标区域高度</param>
		/// <returns>原始区域中的大小</returns>
		public override DCSystem_Drawing.SizeF UnTransformSizeF( float w , float h )
		{
			if( this._SourceRectF.Width != this._DescRectF.Width && this._DescRectF.Width != 0 )
				w = w * this._SourceRectF.Width / this._DescRectF.Width ;
			if(this._SourceRectF.Height != this._DescRectF.Height && this._DescRectF.Height != 0 )
				h = h * this._SourceRectF.Height / this._DescRectF.Height ;
			return new DCSystem_Drawing.SizeF( w , h );
		}
#if ! RELEASE
        /// <summary>
        /// 返回表示对象的字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return this.PageIndex + " " +  this.ContentStyle + " {" + this._SourceRectF.X + "," + this._SourceRectF.Y + "," 
                + this._SourceRectF.Width + "," + this._SourceRectF.Height + "}->{" +
                this._DescRectF.X + "," + this._DescRectF.Y + "," 
                + this._DescRectF.Width + "," + this._DescRectF.Height + "}";
        }
#endif
    }//public class SimpleRectangleTransform



}