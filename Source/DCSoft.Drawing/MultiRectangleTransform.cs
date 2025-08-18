using System;
// // 
using System.Collections;
using System.Collections.Generic;

namespace DCSoft.Drawing
{
    /// <summary>
    /// 复合矩形区域坐标转换关系列表
    /// </summary>
#if !RELEASE
    [System.Diagnostics.DebuggerDisplay("Count={ Count }")]
    [System.Diagnostics.DebuggerTypeProxy(typeof(DCSoft.Common.ListDebugView))]
#endif 
    public class MultiRectangleTransform : TransformBase, System.Collections.IEnumerable
	{
		/// <summary>
		/// 无作为的初始化对象
		/// </summary>
		public MultiRectangleTransform()
		{
            //intItemsVersion = intGlobalItemsVersion ++;
		}

        public override void Dispose()
        {
            if (this.myItems != null)
            {
                foreach (var item in this.myItems)
                {
                    item.Dispose();
                }
                this.myItems.Clear();
                this.myItems = null;
            }
            this.myCurrentItem = null;
            base.Dispose();
        }
        internal List<SimpleRectangleTransform> myItems = new List<SimpleRectangleTransform>();
        
        public SimpleRectangleTransform GetNativeItem( int index )
        {
            return this.myItems[index];
        }
		private double dblRate = 1 ;
		/// <summary>
		/// 缩放比例
		/// </summary>
		public double Rate
		{
			get{ return dblRate ;}
			set{ dblRate = value;}
		}

        protected DCSystem_Drawing.Point mySourceOffsetBack = DCSystem_Drawing.Point.Empty ;
        /// <summary>
        /// 移动所有的来源矩形
        /// </summary>
        /// <param name="dx">X轴移动量</param>
        /// <param name="dy">Y轴移动量</param>
        /// <param name="Remark">是否记录</param>
		public void OffsetSource( int dx , int dy , bool Remark )
		{
            if (dx != 0 || dy != 0)
            {
                if (Remark)
                {
                    mySourceOffsetBack.Offset(dx, dy);
                }
                foreach (SimpleRectangleTransform item in this)
                {
                    item.SourceRectFOffset(dx, dy);
                    item._PartialAreaSourceBounds.Offset(dx, dy);
                    // RectangleF rect = item.SourceRectF;
                    //rect.Offset(dx, dy);
                    //item.SourceRectF = rect;

                    //Rectangle rect2 = item._PartialAreaSourceBounds;
                    //rect2.Offset(dx, dy);
                    //item._PartialAreaSourceBounds = rect2;
                }//foreach
            }
		}

        /// <summary>
        /// 返回指定序号的转换对应关系
        /// </summary>
        public SimpleRectangleTransform this[ int index ]
		{
			get{ return ( SimpleRectangleTransform ) myItems[ index ] ; }
		}
		/// <summary>
		/// 获得指定点所在的对应关系
		/// </summary>
		public SimpleRectangleTransform this[int x, int y]
        {
            get
            {
                for (var iCount = this.myItems.Count - 1; iCount >= 0; iCount--)
                {
                    SimpleRectangleTransform item = this.myItems[iCount];
                    if ( item.Enable && item._SourceRectF.Contains(x, y))
                    {
                        return item;
                    }
                }
                //foreach (SimpleRectangleTransform item in this)
                //{
                //    if (item.SourceRect.Contains(x, y) && item.Enable)
                //    {
                //        return item;
                //    }
                //}
                return null;
            }
        }

        /// <summary>
        /// 根据目标点获得项目
        /// </summary>
        /// <param name="x">目标点X坐标</param>
        /// <param name="y">目标点Y坐标</param>
        /// <returns>项目</returns>
        public SimpleRectangleTransform GetByDescPoint(float x, float y)
        {
            return GetItemByPoint(x, y, false , false , false );
        }

        /// <summary>
        /// 根据转换后的坐标位置查找转换信息对象
        /// </summary>
        /// <param name="x">点X坐标</param>
        /// <param name="y">点Y坐标</param>
        /// <param name="compatibility">是否启用兼容模式。若启用兼容模式，
        /// 如果没有找到和指定点精确匹配的坐标转换信息对象，
        /// 则尽量查找距离点最近的坐标转换信息对象</param>
        /// <param name="onlyIncludeEnabledItem">只对可用的对象进行处理</param>
        /// <param name="useSourceRect">模式，True:匹配源矩形；False:匹配目标矩形。</param>
        /// <returns>找到的坐标转换信息对象</returns>
        public SimpleRectangleTransform GetItemByPoint(
            float x,
            float y,
            bool useSourceRect ,
            bool compatibility,
            bool onlyIncludeEnabledItem )
        {
            if (this.Count == 0)
            {
                // 列表为空，没法获得值
                return null;
            }
            foreach (SimpleRectangleTransform item in this.myItems)
            {
                if (onlyIncludeEnabledItem && item.Enable == false)
                {
                    continue;
                }

                if (useSourceRect)
                {
                    if (item._SourceRectF.Contains(x, y))
                    {
                        return item;
                    }
                }
                else
                {
                    if (item._DescRectF.Contains(x, y))
                    {
                        return item;
                    }
                }
            }//foreach
            if (compatibility)
            {
                // 寻找距离最近的目标矩形区域
                float minLen = 0;
                int index = 0;
                int itemCount = myItems.Count;
                for (int iCount = 0; iCount < itemCount; iCount++)
                {
                    SimpleRectangleTransform item = myItems[iCount];
                    if (onlyIncludeEnabledItem && item.Enable == false)
                    {
                        continue;
                    }

                    DCSystem_Drawing.RectangleF rect = useSourceRect ? item._SourceRectF : item._DescRectF;
                    if (rect.Contains(x, y))
                    {
                        return myItems[iCount];
                    }
                    float len = RectangleCommon.GetDistance(x, y, rect);
                    if ( iCount == 0 || len < minLen)
                    {
                        minLen = len;
                        index = iCount;
                    }
                }//for
                return myItems[index];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 根据转换后的坐标位置查找转换信息对象
        /// </summary>
        /// <param name="x">点X坐标</param>
        /// <param name="y">点Y坐标</param>
        /// <param name="compatibility">是否启用兼容模式。若启用兼容模式，
        /// 如果没有找到和指定点精确匹配的坐标转换信息对象，
        /// 则尽量查找距离点最近的坐标转换信息对象</param>
        /// <param name="onlyIncludeEnabledItem">只对可用的对象进行处理</param>
        /// <param name="useSourceRect">模式，True:匹配源矩形；False:匹配目标矩形。</param>
        /// <returns>找到的坐标转换信息对象</returns>
        public SimpleRectangleTransform GetItemByPoint(
            float x,
            float y,
            bool useSourceRect,
            GetRectangleItemCompatibilityMode compatibilityMode ,
            bool onlyIncludeEnabledItem)
        {
            if (this.Count == 0)
            {
                // 列表为空，没法获得值
                return null;
            }
            foreach (SimpleRectangleTransform item in this.myItems)
            {
                if (onlyIncludeEnabledItem && item.Enable == false)
                {
                    continue;
                }

                if (useSourceRect)
                {
                    if (item.SourceRectF.Contains(x, y))
                    {
                        return item;
                    }
                }
                else
                {
                    if (item.DescRectF.Contains(x, y))
                    {
                        return item;
                    }
                }
            }//foreach
            if (compatibilityMode == GetRectangleItemCompatibilityMode.None)
            {
                return null;
            }
            else if( compatibilityMode == GetRectangleItemCompatibilityMode.Nearest )
            {
                // 寻找距离最近的目标矩形区域
                float minLen = 0;
                int index = 0;
                for (int iCount = 0; iCount < this.myItems.Count; iCount++)
                {
                    SimpleRectangleTransform item = myItems[iCount];
                    if (onlyIncludeEnabledItem && item.Enable == false)
                    {
                        continue;
                    }

                    DCSystem_Drawing.RectangleF rect = useSourceRect ? item.SourceRectF : item.DescRectF;
                    if (rect.Contains(x, y))
                    {
                        return myItems[iCount];
                    }
                    float len = RectangleCommon.GetDistance(x, y, rect);
                    if (iCount == 0 || len < minLen)
                    {
                        minLen = len;
                        index = iCount;
                    }
                }//for
                return myItems[index];
            }
            else if ( compatibilityMode == GetRectangleItemCompatibilityMode.Up
                || compatibilityMode == GetRectangleItemCompatibilityMode.Down)
            {
                // 寻找距离最近的目标矩形区域
                float minLen = float.MaxValue ;
                int index = 0;
                for (int iCount = 0; iCount < this.myItems.Count; iCount++)
                {
                    SimpleRectangleTransform item = myItems[iCount];
                    if (onlyIncludeEnabledItem && item.Enable == false)
                    {
                        continue;
                    }

                    DCSystem_Drawing.RectangleF rect = useSourceRect ? item.SourceRectF : item.DescRectF;
                    if (rect.Contains(x, y))
                    {
                        return myItems[iCount];
                    }
                    float len = float.MaxValue;
                    if (compatibilityMode == GetRectangleItemCompatibilityMode.Up)
                    {
                        len = y - rect.Bottom;
                        if ((len < minLen && len >= 0))
                        {
                            minLen = len;
                            index = iCount;
                        }
                    }
                    else if (compatibilityMode == GetRectangleItemCompatibilityMode.Down)
                    {
                        len = rect.Top - y;
                        if ((len < minLen && len >= 0))
                        {
                            minLen = len;
                            index = iCount;
                        }
                    }

                }//for
                return myItems[index];
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// 添加项目
        /// </summary>
        /// <param name="item">项目</param>
        /// <returns>新项目在列表中的序号</returns>
		public void Add( SimpleRectangleTransform item )
		{
            //this.intItemsVersion = intGlobalItemsVersion++;
			this.myItems.Add( item );
		}

		/// <summary>
		/// 判断指定坐标的点是否有相应的原始区域
		/// </summary>
		/// <param name="x">点X坐标</param>
		/// <param name="y">点Y坐标</param>
		/// <returns>是否有相应的原始区域</returns>
		public override bool ContainsSourcePoint( int x , int y )
		{
			return this[ x , y ] != null;
		}

		public override DCSystem_Drawing.Point TransformPoint(int x, int y)
		{
            var item = this[x, y];
            if(item == null )
            {
                return Point.Empty;
            }
            else
            {
                return item.TransformPoint(x, y);
            }
   //             //foreach( SimpleRectangleTransform item in this )
   //             for (var iCount = this.myItems.Count - 1; iCount >= 0; iCount--)
   //             {
   //                 var item = this.myItems[iCount];
   //                 if (item.Enable && item._SourceRectF.Contains(x, y))
   //                 {
   //                     return item.TransformPoint(x, y);
   //                 }
   //             }
			////{
			////	if( item.Enable && item.SourceRect.Contains( x , y  ) )
			////		return item.TransformPoint( x , y );
			////}
   //         return DCSystem_Drawing.Point.Empty ;
			//return new  Point( x , y );
		}


        public override DCSystem_Drawing.SizeF TransformSizeF(float w, float h)
		{
			return new DCSystem_Drawing.SizeF( ( float ) ( w * dblRate ) , ( float ) ( h * dblRate ));
		}
        /// <summary>
        /// 反向转换坐标
        /// </summary>
        /// <param name="x">X坐标值</param>
        /// <param name="y">Y坐标值</param>
        /// <returns>转换后的坐标</returns>
        public override DCSystem_Drawing.Point UnTransformPoint(int x, int y)
		{
            DCSystem_Drawing.Point p = DCSystem_Drawing.Point.Empty ;
			foreach( SimpleRectangleTransform item in this )
			{
				if( item.Enable && item.DescRect.Contains( x , y ))
				{
					p = item.UnTransformPoint( x , y );
					return p ;
				}
			}
			return p ;
		}

        /// <summary>
        /// 转换坐标
        /// </summary>
        /// <param name="x">X坐标值</param>
        /// <param name="y">Y坐标值</param>
        /// <returns>转换后的坐标</returns>
		public override DCSystem_Drawing.PointF TransformPointF(float x, float y)
		{
			foreach( SimpleRectangleTransform item in this )
			{
                if (item.SourceRectF.Contains(x, y) && item.Enable)
                {
                    return item.TransformPointF(x, y);
                }
			}
			return new DCSystem_Drawing.PointF( x , y );
		}

        /// <summary>
        /// 反向转换坐标
        /// </summary>
        /// <param name="x">X坐标值</param>
        /// <param name="y">Y坐标值</param>
        /// <returns>转换后的坐标</returns>
		public override DCSystem_Drawing.PointF UnTransformPointF(float x, float y)
		{
            DCSystem_Drawing.PointF p  = DCSystem_Drawing.PointF.Empty ;
            SimpleRectangleTransform lastItem = null;
			foreach( SimpleRectangleTransform item in this )
			{
                if (item.Enable && item.DescRectF.Contains(x, y))
                {
                    if (Math.Abs(y - item.DescRectF.Bottom) < 0.5)
                    {
                        lastItem = item;
                        continue;
                    }
                    else
                    {
                        p = item.UnTransformPointF(x, y);
                        break;
                    }
                }
		    }
            if (lastItem != null)
            {
                p = lastItem.UnTransformPointF(x, y);
            }
			return p ;	
		}

        /// <summary>
        /// 反向转换尺寸
        /// </summary>
        /// <param name="w">宽度</param>
        /// <param name="h">高度</param>
        /// <returns>转换后的尺寸</returns>
		public override DCSystem_Drawing.Size UnTransformSize(int w, int h)
		{
            DCSystem_Drawing.PointF p = DCSystem_Drawing.PointF.Empty;
            foreach (SimpleRectangleTransform item in this)
            {
                if (item.Enable)
                {
                    DCSystem_Drawing.Size size = item.UnTransformSize(w, h);
                }
            }
            return new DCSystem_Drawing.Size((int)(w / dblRate), (int)(h / dblRate));
		}
        /// <summary>
        /// 反向转换尺寸
        /// </summary>
        /// <param name="vSize">尺寸</param>
        /// <returns>转换后的尺寸</returns>
		public override DCSystem_Drawing.Size UnTransformSize(DCSystem_Drawing.Size vSize)
		{
            return UnTransformSize(vSize.Width, vSize.Height);
		}
        /// <summary>
        /// 反向转换尺寸
        /// </summary>
        /// <param name="w">宽度</param>
        /// <param name="h">高度</param>
        /// <returns>转换后的尺寸</returns>
		public override DCSystem_Drawing.SizeF UnTransformSizeF(float w, float h)
		{
            DCSystem_Drawing.PointF p = DCSystem_Drawing.PointF.Empty;
            foreach (SimpleRectangleTransform item in this)
            {
                if (item.Enable)
                {
                    DCSystem_Drawing.SizeF size = item.UnTransformSizeF(w, h);
                    return size;
                }
            }
			return new DCSystem_Drawing.SizeF( ( float ) ( w / dblRate ) , ( float ) ( h / dblRate ));
		}
        /// <summary>
        /// 反向转换尺寸
        /// </summary>
        /// <param name="vSize">尺寸</param>
        /// <returns>转换后的尺寸</returns>
		public override DCSystem_Drawing.SizeF UnTransformSizeF(DCSystem_Drawing.SizeF vSize)
		{
            return UnTransformSizeF(vSize.Width, vSize.Height);
		}

		/// <summary>
		/// 当前转换关系项目
		/// </summary>
		protected SimpleRectangleTransform myCurrentItem = null;

		public void Clear()
		{
			myItems.Clear();
		}


        /// <summary>
        /// 项目个数
        /// </summary>
		public int Count
		{
			get
			{
				return myItems.Count ;
			}
		}

		#region IEnumerable 成员

		public System.Collections.IEnumerator GetEnumerator()
		{
			return myItems.GetEnumerator();
		}

		#endregion
	}//public class RectangleTransform : System.Collections.CollectionBase

    public enum GetRectangleItemCompatibilityMode
    {
        /// <summary>
        /// 不兼容
        /// </summary>
        None ,
        /// <summary>
        /// 取最近的
        /// </summary>
        Nearest,
        /// <summary>
        /// 向上查找
        /// </summary>
        Up ,
        /// <summary>
        /// 向下查找
        /// </summary>
        Down
    }
}