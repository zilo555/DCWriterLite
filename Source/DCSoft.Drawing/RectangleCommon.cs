using System;
//using System.Drawing ;
using System.Collections.Generic;

namespace DCSoft.Drawing
{
	/// <summary>
	/// 定义矩形相关的例称模块，本对象不能实例化
	/// </summary>
    /// <remarks>编制 袁永福</remarks>
    public static class RectangleCommon
	{
        /// <summary>
        /// 获得指定点相对于指定矩形的距离
        /// </summary>
        /// <param name="x">指定点的X坐标</param>
        /// <param name="y">指定点的Y坐标</param>
        /// <param name="rectangle">矩形边框</param>
        /// <returns>距离，若小于0则点被包含在矩形区域中</returns>
        public static float GetDistance(float x, float y, DCSystem_Drawing.RectangleF rectangle)
        {
            float len = 0;
            int area = GetRectangleArea(rectangle, x, y);
            switch (area)
            {
                case 0:
                    len = Math.Min(x - rectangle.Left, rectangle.Right - x);
                    float len2 = Math.Min(y - rectangle.Top, rectangle.Bottom - y);
                    len = - Math.Min(len, len2);
                    break;
                case 1:
                    len = (float)Math.Sqrt(
                        (x - rectangle.Left) * (x - rectangle.Left)
                        + (y - rectangle.Top) * (y - rectangle.Top));
                    break;
                case 2:
                    len = rectangle.Top - y;
                    break;
                case 3:
                    len = (float)Math.Sqrt(
                        (x - rectangle.Right) * (x - rectangle.Right)
                        + (y - rectangle.Top) * (y - rectangle.Top));
                    break;
                case 4:
                    len = x - rectangle.Right;
                    break;
                case 5:
                    len = (float)Math.Sqrt(
                        (x - rectangle.Right) * (x - rectangle.Right)
                        + (y - rectangle.Bottom) * (y - rectangle.Bottom));
                    break;
                case 6:
                    len = y - rectangle.Bottom;
                    break;
                case 7:
                    len = (float)Math.Sqrt(
                        (x - rectangle.Left) * (x - rectangle.Left)
                        + (y - rectangle.Bottom) * (y - rectangle.Bottom));
                    break;
                case 8:
                    len = rectangle.Left - x;
                    break;
            }
            return len;
        }

        /// <summary>
        /// 获得指定点在指定矩形相对的区域编号
        /// </summary>
        /// <remarks>
        /// 
        ///   1  |   2   |  3
        /// -----+=======+-------
        ///      ||     ||
        ///   8  ||  0  ||  4
        ///      ||     ||
        /// -----+=======+-------
        ///   7  |   6   |  5
        ///   
        /// </remarks>
        /// <param name="rectangle"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static int GetRectangleArea(DCSystem_Drawing.RectangleF rectangle, float x, float y)
        {
            if (rectangle.Contains(x, y))
            {
                return 0;
            }
            else if (y < rectangle.Top)
            {
                if (x < rectangle.Left)
                {
                    return 1;
                }
                else if (x < rectangle.Right)
                {
                    return 2;
                }
                else
                {
                    return 3;
                }
            }
            else if (y < rectangle.Bottom)
            {
                if (x < rectangle.Left)
                {
                    return 8;
                }
                else if (x < rectangle.Right)
                {
                    return 0;
                }
                else
                {
                    return 4;
                }
            }
            else
            {
                if (x < rectangle.Left)
                {
                    return 7;
                }
                else if (x < rectangle.Right)
                {
                    return 6;
                }
                else
                {
                    return 5;
                }
            }
        }

        /// <summary>
        /// 根据两点坐标获得方框对象
        /// </summary>
        /// <param name="x1"></param>
        /// <param name="y1"></param>
        /// <param name="x2"></param>
        /// <param name="y2"></param>
        /// <returns></returns>
        public static DCSystem_Drawing.Rectangle GetRectangle( int x1 , int y1 , int x2 , int y2)
		{
			 var myRect = DCSystem_Drawing.Rectangle.Empty ;
			if( x1 < x2)
			{
				myRect.X 		= x1 ;
				myRect.Width 	= x2 - x1 ;
			}
			else
			{
				myRect.X 		= x2;
				myRect.Width	= x1 - x2 ;
			}
			if( y1 < y2)
			{
				myRect.Y 		= y1;
				myRect.Height	= y2 - y1 ;
			}
			else
			{
				myRect.Y 		= y2;
				myRect.Height	= y1 - y2;
			}
			if( myRect.Width < 1) myRect.Width = 1 ;
			if( myRect.Height < 1) myRect.Height = 1 ;
			return myRect ;
		}

		
		/// <summary>
		/// 根据两点坐标获得方框对象
		/// </summary>
		/// <param name="p1">第一个点的坐标</param>
		/// <param name="p2">第二个点的坐标</param>
		/// <returns></returns>
		public static DCSystem_Drawing.Rectangle GetRectangle(DCSystem_Drawing.Point p1 , DCSystem_Drawing.Point p2)
		{
			return GetRectangle( p1.X ,p1.Y , p2.X , p2.Y );
		}

        /// <summary>
        /// 将点移动到矩形区域中
        /// </summary>
        /// <param name="p">点坐标</param>
        /// <param name="Bounds">矩形区域</param>
        /// <returns>修正后的点坐标</returns>
        public static DCSystem_Drawing.Point MoveInto(
             DCSystem_Drawing.Point p ,
             DCSystem_Drawing.Rectangle Bounds)
		{
			if( !Bounds.IsEmpty )
			{
				if( p.X < Bounds.Left )
					p.X = Bounds.Left ;
				if( p.X >= Bounds.Right )
					p.X = Bounds.Right ;
				if( p.Y < Bounds.Top )
					p.Y = Bounds.Top ;
				if( p.Y >= Bounds.Bottom )
					p.Y = Bounds.Bottom ;
			}
			return p;
		}

        /// <summary>
        /// 将点移动到矩形区域中
        /// </summary>
        /// <param name="p">点坐标</param>
        /// <param name="Bounds">矩形区域</param>
        /// <returns>修正后的点坐标</returns>
        public static DCSystem_Drawing.PointF MoveInto(
             DCSystem_Drawing.PointF p ,
             DCSystem_Drawing.RectangleF Bounds)
		{
			if( !Bounds.IsEmpty )
			{
				if( p.X < Bounds.Left )
					p.X = Bounds.Left ;
				if( p.X >= Bounds.Right )
					p.X = Bounds.Right ;
				if( p.Y < Bounds.Top )
					p.Y = Bounds.Top ;
				if( p.Y >= Bounds.Bottom )
					p.Y = Bounds.Bottom ;
			}
			return p;
		}
	}//public sealed class RectangleCommon
}