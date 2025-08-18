using System;
using DCSoft.WinForms;
using DCSoft.Drawing ;
namespace DCSoft.Printing
{
	/// <summary>
	/// ReportPageTransform 的摘要说明。
	/// </summary>
    public class MultiPageTransform : MultiRectangleTransform
	{
		public MultiPageTransform()
		{
		}

		protected PrintPageCollection  myPages = null;
		/// <summary>
		/// 页面集合
		/// </summary>
		public PrintPageCollection Pages
		{
			get
			{
				return myPages ;
			}
			set
			{
				myPages = value;
			}
		}
         
		/// <summary>
		/// 根据页面位置添加矩形区域转换关系
		/// </summary>
        /// <param name="page">页面对象</param>
        /// <param name="pageTop">页面顶端位置</param>
        /// <param name="zoomRate">缩放比率</param>
		public virtual void AddPage(PrintPage page, float pageTop, float zoomRate)
        {
        }




        public void Refresh( float zoomRate , int pageStartTop , int pageSpacing , int preserveSpacing )
		{
            float maxPageWidth = 0;
            foreach (PrintPage page in this.Pages)
            {
                if (maxPageWidth < page.Width)
                {
                    maxPageWidth = page.Width;
                }
            }
            maxPageWidth = ( int )( maxPageWidth * zoomRate );
//			float leftmargin = ( float ) ( myPages.LeftMargin * ZoomRate );
			//float pageheight = ( float ) (  myPages.PaperHeight * ZoomRate );

			mySourceOffsetBack =  Point.Empty ;
			this.Clear();
            var middlePage = this.Pages.SafeGet((int)this.Pages.Count / 2);
            if(middlePage != null && middlePage.HasHeaderRows)
            {
                // 中间页存在标题行，则成员个数可能为页面个数的4倍
                this.myItems.Capacity = this.Pages.Count * 4;
            }
            else
            {
                // 一般而言成员个数是页面个数的3倍。
                this.myItems.Capacity = this.Pages.Count * 3;
            }

            //int iCount = 0 ;
            float topCount = pageStartTop + preserveSpacing;
           foreach ( PrintPage page in this.Pages )
			{
                //page.ClientLeftFix = (int)(ZoomRate * (MaxPageWidth - page.Width) / 2.0);
				//float PageTop = ( pageheight + PageSpacing ) * iCount + PageSpacing ;
				//iCount ++ ;

				AddPage( page , topCount , zoomRate );
                Rectangle pc = page.ClientBounds;
                pc.Y = (int)topCount;
                //page.ClientBounds = pc;
                page.SetClientBounds(pc);
                    topCount = topCount + page.PageSettings.ViewPaperHeight * zoomRate + pageSpacing;
			}//foreach( PrintPage page in myDocument.Pages )
		}

        private readonly int intLimitedPageIndex = -1;
        
		/// <summary>
		/// 是否使用绝对点坐标转换模式
		/// </summary>
		protected bool bolUseAbsTransformPoint = false;
		/// <summary>
        /// 是否使用绝对点坐标转换模式
		/// </summary>
		public bool UseAbsTransformPoint
		{
			get
            { 
                return bolUseAbsTransformPoint ;
            }
			set
            {
                bolUseAbsTransformPoint = value;
            }
		}

        public override  Point UnTransformPoint(int x, int y)
        {
            if (this.bolUseAbsTransformPoint)
            {
                 PointF p = AbsUnTransformPoint(x, y);
                return new  Point((int)p.X, (int)p.Y);
            }
            else
            {
                 Point p =  Point.Empty;
                for( int iCount = this.Count - 1 ; iCount >= 0 ; iCount -- ) 
                {
                    SimpleRectangleTransform item = this[ iCount ] ;
                    if (item.Enable && item.DescRectF.Contains(x, y))
                    {
                        p = item.UnTransformPoint(x, y);
                        return p;
                    }
                }
                return p;
            }
        }

        public Point UnTransformPointBodyDirect(int x, int y)
        {
            Point p = Point.Empty;
            for (int iCount = this.Count - 1; iCount >= 0; iCount--)
            {
                SimpleRectangleTransform item = this[iCount];
                if (item.ContentStyle == PageContentPartyStyle.Body 
                    && item.DescRectF.Contains(x, y))
                {
                    p = item.UnTransformPoint(x, y);
                    return p;
                }
            }
            return p;
        }

        public override  PointF UnTransformPointF(float x, float y)
        {
            if (this.bolUseAbsTransformPoint)
            {
                return AbsUnTransformPoint(x, y);
            }
            else
            {
                return base.UnTransformPointF(x, y);
            }
        }

        public override  Point TransformPoint(int x, int y)
		{
			if( this.bolUseAbsTransformPoint )
			{
                 PointF p = AbsTransformPoint(x, y);
                return new  Point((int)p.X, (int)p.Y);
			}
			else
			{
				return base.TransformPoint (x, y);
			}
		}

        public override  PointF TransformPointF(float x, float y)
        {
            if (this.bolUseAbsTransformPoint)
            {
                return AbsTransformPoint(x, y);
            }
            else
            {
                return base.TransformPointF(x, y);
            }
        }

		public override bool ContainsSourcePoint(int x, int y)
		{
			if( this.bolUseAbsTransformPoint )
				return true;
			else
				return base.ContainsSourcePoint (x, y);	
		}
        //public override  Point TransformPoint( Point p)
        //{
        //    return base.TransformPoint(p);
        //}


        public  PointF AbsUnTransformPoint(float x, float y)
        {
            SimpleRectangleTransform pre = null;
            SimpleRectangleTransform next = null;
            SimpleRectangleTransform cur = null;

            foreach (SimpleRectangleTransform item in this)
            {
                if (item.Enable == false)
                {
                    continue;
                }
                if (intLimitedPageIndex >= 0 && item.PageIndex != intLimitedPageIndex)
                {
                    continue;
                }
                var itemDescRectF = item.DescRectF;
                if (itemDescRectF.Contains(x, y))
                {
                    return item.UnTransformPointF(x, y);
                }
                if (y >= itemDescRectF.Top && y < itemDescRectF.Bottom)
                {
                    cur = item;
                    break;
                }
                if (y < itemDescRectF.Top)
                {
                    if (next == null || itemDescRectF.Top < next.DescRectF.Top)
                    {
                        next = item;
                    }
                }
                if (y > itemDescRectF.Bottom)
                {
                    if (pre == null || itemDescRectF.Bottom > pre.DescRectF.Bottom)
                    {
                        pre = item;
                    }
                }
            }//foreach
            if (cur == null)
            {
                if (pre != null)
                {
                    cur = pre;
                }
                else
                {
                    cur = next;
                }
            }
            if (cur == null)
            {
                return  PointF.Empty;
            }
             PointF p = new  PointF(x, y);
            p = RectangleCommon.MoveInto(p, cur.DescRectF);
            return cur.UnTransformPointF(p);
        }


		public  PointF AbsTransformPoint( float x , float y )
		{
			SimpleRectangleTransform pre = null;
			SimpleRectangleTransform next = null;
			SimpleRectangleTransform cur = null;
			
			foreach( SimpleRectangleTransform item in this )
			{
                if (item.Enable == false)
                {
                    continue;
                }
                var itemSourceRectF = item.SourceRectF;
                if (itemSourceRectF.Contains(x, y))
                {
                    return item.TransformPointF(x, y);
                }

				if( y >= itemSourceRectF.Top && y <= itemSourceRectF.Bottom )
				{
					cur = item ;
					break;
				}
				if( y < itemSourceRectF.Top )
				{
                    if (next == null || itemSourceRectF.Top < next.SourceRectF.Top)
                    {
                        next = item;
                    }
				}
				if( y > itemSourceRectF.Bottom )
				{
                    if (pre == null || itemSourceRectF.Bottom > pre.SourceRectF.Bottom)
                    {
                        pre = item;
                    }
				}
			}//foreach
			if( cur == null )
			{
                if (pre != null)
                {
                    cur = pre;
                }
                else
                {
                    cur = next;
                }
			}
            if (cur == null)
            {
                return  PointF.Empty;
            }
			 PointF p = new  PointF( x , y );
			p =  RectangleCommon.MoveInto( p , cur.SourceRectF  );
			return cur.TransformPointF( p );
		}
	}//public class MultiPageTransform : MultiRectangleTransform
}