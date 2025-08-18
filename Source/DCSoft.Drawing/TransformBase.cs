using System;

namespace DCSoft.Drawing
{
	/// <summary>
	/// 坐标转换对象基础类
	/// </summary>
    public abstract class TransformBase
	{
		public virtual void Dispose()
		{

		}
        /// <summary>
        /// 本区域是否包含源区域中的一个点
        /// </summary>
        /// <param name="x">X坐标值</param>
        /// <param name="y">Y坐标值</param>
        /// <returns>是否包含点</returns>
		public virtual bool ContainsSourcePoint( int x , int y )
		{
			return false;
		}
		/// <summary>
		/// 将原始区域的点转换为目标区域中的点
		/// </summary>
		/// <param name="p">原始区域中的点坐标</param>
		/// <returns>目标区域中的点坐标</returns>
		public virtual DCSystem_Drawing.Point TransformPoint(DCSystem_Drawing.Point p )
		{
			return TransformPoint( p.X , p.Y );
		}
		/// <summary>
		/// 将原始区域的点转换为目标区域中的点
		/// </summary>
		/// <param name="x">原始区域中的点的X坐标</param>
		/// <param name="y">原始区域的点的Y坐标</param>
		/// <returns>目标区域中的点坐标</returns>
		public virtual DCSystem_Drawing.Point TransformPoint( int x , int y )
		{
			return DCSystem_Drawing.Point.Empty ;
		}

		/// <summary>
		/// 将原始区域的点转换为目标区域中的点
		/// </summary>
		/// <param name="p">原始区域中的点的坐标</param>
		/// <returns>目标区域中的点坐标</returns>
		public virtual DCSystem_Drawing.PointF TransformPointF(DCSystem_Drawing.PointF p )
		{
			return TransformPointF( p.X , p.Y );
		}
		/// <summary>
		/// 将原始区域的点转换为目标区域中的点
		/// </summary>
		/// <param name="x">原始区域中的点的X坐标</param>
		/// <param name="y">原始区域的点的Y坐标</param>
		/// <returns>目标区域中的点坐标</returns>
		public virtual DCSystem_Drawing.PointF TransformPointF( float x , float y )
		{
			return DCSystem_Drawing.PointF.Empty ;
		}
        /// <summary>
        /// 将原始区域重的大小转换未目标区域中的大小
        /// </summary>
        /// <param name="w">原始区域宽度</param>
        /// <param name="h">原始区域高度</param>
        /// <returns>目标区域中的大小</returns>
        public virtual DCSystem_Drawing.SizeF TransformSizeF( float w , float h )
		{
			return DCSystem_Drawing.SizeF.Empty ;
		}
		/// <summary>
		/// 将原始区域中的矩形转换未目标区域中的矩形
		/// </summary>
		/// <param name="left">原始区域中矩形的左端位置</param>
		/// <param name="top">原始区域中矩形的顶端位置</param>
		/// <param name="width">原始区域中矩形的宽度</param>
		/// <param name="height">原始区域中矩形的高度</param>
		/// <returns>目标区域中的矩形</returns>
		public virtual DCSystem_Drawing.RectangleF TransformRectangleF(
			float left ,
			float top , 
			float width , 
			float height )
		{
			return new DCSystem_Drawing.RectangleF( 
				TransformPointF( left , top ) ,
				TransformSizeF( width , height ));
		}

		/// <summary>
		/// 将目标区域中的坐标转换为原始区域的坐标
		/// </summary>
		/// <param name="p">目标区域中的坐标</param>
		/// <returns>原始区域的坐标</returns>
		public virtual DCSystem_Drawing.Point UnTransformPoint(DCSystem_Drawing.Point p )
		{
			return UnTransformPoint( p.X  , p.Y );
		}
		/// <summary>
		/// 将目标区域中的坐标转换为原始区域中的坐标
		/// </summary>
		/// <param name="x">目标区域中的X坐标</param>
		/// <param name="y">目标区域中的Y坐标</param>
		/// <returns>原始区域中的坐标</returns>
		public virtual DCSystem_Drawing.Point UnTransformPoint( int x , int y )
		{
			return DCSystem_Drawing.Point.Empty ;
		}

		/// <summary>
		/// 将目标区域中的坐标转换为原始区域的坐标
		/// </summary>
		/// <param name="p">目标区域中的坐标</param>
		/// <returns>原始区域的坐标</returns>
		public virtual DCSystem_Drawing.PointF UnTransformPointF(DCSystem_Drawing.PointF p )
		{
			return UnTransformPointF( p.X , p.Y );
		}
		/// <summary>
		/// 将目标区域中的坐标转换为原始区域中的坐标
		/// </summary>
		/// <param name="x">目标区域中的X坐标</param>
		/// <param name="y">目标区域中的Y坐标</param>
		/// <returns>原始区域中的坐标</returns>
		public virtual DCSystem_Drawing.PointF UnTransformPointF( float x , float y )
		{
			return DCSystem_Drawing.PointF.Empty ;
		}

		/// <summary>
		/// 将目标区域中的大小转换为原始区域中的大小
		/// </summary>
		/// <param name="vSize">目标区域中的大小</param>
		/// <returns>原始区域中的大小</returns>
		public virtual DCSystem_Drawing.Size UnTransformSize(DCSystem_Drawing.Size vSize )
		{
			return UnTransformSize( vSize.Width , vSize.Height );
		}
		/// <summary>
		/// 将目标区域中的大小转换为原始区域中的大小
		/// </summary>
        /// <param name="w">目标区域宽度</param>
        /// <param name="h">目标区域高度</param>
        /// <returns>原始区域中的大小</returns>
		public virtual DCSystem_Drawing.Size UnTransformSize( int w , int h )
		{
			return DCSystem_Drawing.Size.Empty ;
		}

		/// <summary>
		/// 将目标区域中的大小转换为原始区域中的大小
		/// </summary>
		/// <param name="vSize">目标区域中的大小</param>
		/// <returns>原始区域中的大小</returns>
		public virtual DCSystem_Drawing.SizeF UnTransformSizeF(DCSystem_Drawing.SizeF vSize )
		{
			return UnTransformSizeF( vSize.Width , vSize.Height );
		}
		/// <summary>
		/// 将目标区域中的大小转换为原始区域中的大小
		/// </summary>
        /// <param name="w">目标区域宽度</param>
        /// <param name="h">目标区域高度</param>
        /// <returns>原始区域中的大小</returns>
		public virtual DCSystem_Drawing.SizeF UnTransformSizeF( float w , float h )
		{
			return DCSystem_Drawing.SizeF.Empty ;
		}

		/// <summary>
		/// 将目标区域中的矩形转换为原始区域中的矩形
		/// </summary>
		/// <param name="rect">目标区域中的矩形</param>
		/// <returns>原始区域中的矩形</returns>
		public virtual DCSystem_Drawing.Rectangle UnTransformRectangle(DCSystem_Drawing.Rectangle rect )
		{
			return new DCSystem_Drawing.Rectangle(
				UnTransformPoint( rect.Location ) , 
				UnTransformSize( rect.Size ));
		}

		/// <summary>
		/// 将目标区域中的矩形转换为原始区域中的矩形
		/// </summary>
		/// <param name="rect">目标区域中的矩形</param>
		/// <returns>原始区域中的矩形</returns>
		public virtual DCSystem_Drawing.RectangleF UnTransformRectangleF(DCSystem_Drawing.RectangleF rect )
		{
			return new DCSystem_Drawing.RectangleF(
				UnTransformPointF( rect.Location ) , 
				UnTransformSizeF( rect.Size ));
		}
    }//public abstract class TransformBase
}