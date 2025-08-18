using System;
using DCSoft.WinForms;
//using System.Drawing ;
using DCSoft.Drawing ;
using System.Collections.Generic;
using DCSoft.Common;
using DCSoft.Writer.Dom;

namespace DCSoft.Printing
{
	/// <summary>
	/// 打印页对象
	/// </summary>
    public partial class PrintPage
	{

        /// <summary>
        /// 表示完全空白的页对象
        /// </summary>
        public static readonly PrintPage EmptyPage = new PrintPage();
        private PrintPage( )
        {

        }

        public PrintPage( 
            DomDocument document ,
            XPageSettings pageSettings, 
            PrintPageCollection pages , 
            float headerHeight ,
            float footerHeight )
        {
            myDocument = document;
            myPageSettings = pageSettings;
            myOwnerPages = pages;
            intHeaderContentHeight = headerHeight;
            intFooterContentHeight = footerHeight;
            intWidth = (int)myPageSettings.ViewClientWidth ;
            // 对标准页高缩小点，避免由于某个页高正好等于标准页高时该页最下面
            // 的线条无法显示和打印。（通融才能从容）
            this._Height = this.ViewStandardHeight - 10 ;
            //this._Top = 0;
            ResetTop();
        }
        public void Dispose()
        {
            if(this._HeaderRows != null )
            {
                this._HeaderRows.Clear();
                this._HeaderRows = null;
            }
            this._Margins = null;
            //if (this.OwneredTransformItems != null)
            //{
            //    foreach (var item in this.OwneredTransformItems)
            //    {
            //        item.Dispose();
            //    }
            //    this.OwneredTransformItems = null;
            //}
            this.ClientLayoutInfo = null;
            this.ClientMargins = null;
            this.myDocument = null;
            this.myOwnerPages = null;
            this.myPageSettings = null;
        }
        public DCSystem_Drawing.Rectangle ClientSelectionBounds = DCSystem_Drawing.Rectangle.Empty;
        public void AddClientSelectionBounds(DCSystem_Drawing.Rectangle rect )
        {
            if (rect.IsEmpty == false)
            {
                if (this.ClientSelectionBounds.IsEmpty)
                {
                    this.ClientSelectionBounds = rect;
                }
                else
                {
                    this.ClientSelectionBounds = DCSystem_Drawing.Rectangle.Union(this.ClientSelectionBounds, rect);
                }
            }
        }
        /// <summary>
        /// 是否以分页符结束
        /// </summary>
        internal bool EndByPageBreak = false;

        private bool _IsEmpty = false;
        /// <summary>
        /// 是否为空白页
        /// </summary>
        //[System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true )]
        public bool IsEmpty
        {
            get
            {
                if(this == EmptyPage )
                {
                    return true;
                }
                return _IsEmpty;
            }
            set
            {
                _IsEmpty = value;
            }
        }
#if ! RELEASE
        /// <summary>
        /// 返回表示对象的字符串
        /// </summary>
        /// <returns></returns>

        public override string ToString()
        {
            if (this.IsEmpty)
            {
                return "Empty page";
            }
            else
            {
                return "Page " + this.PageIndex  + ",Top:" + this.Top + " Height:" +this.Height;
            }
        }
#endif

        private System.Collections.IList _HeaderRows = null;
        /// <summary>
        /// 标题行
        /// </summary>
        public System.Collections.IList HeaderRows
        {
            get
            {
                return this._HeaderRows; 
            }
            set
            {
                this._HeaderRows = value; 
            }
        }
        public bool HasHeaderRows
        {
            get
            {
                return this._HeaderRows != null && this._HeaderRows.Count > 0;
            }
        }

        private DCSystem_Drawing.RectangleF _HeaderRowsBounds = DCSystem_Drawing.RectangleF.Empty;
        /// <summary>
        /// 标题行的边界,采用文档视图坐标
        /// </summary>
        public DCSystem_Drawing.RectangleF HeaderRowsBounds
        {
            get
            {
                return _HeaderRowsBounds; 
            }
            set
            {
                _HeaderRowsBounds = value; 
            }
        }

        

        private XPageSettings myPageSettings = null;
        /// <summary>
        /// 页面设置对象
        /// </summary>
        public XPageSettings PageSettings
        {
            get
            {
                return myPageSettings; 
            }
            set
            {
                myPageSettings = value; 
            }
        }

        private float intDocumentHeight = 0;
        /// <summary>
        /// 文档高度
        /// </summary>
        public float DocumentHeight
        {
            get
            {
                return intDocumentHeight; 
            }
            set
            {
                intDocumentHeight = value; 
            }
        }
        private DomDocument myDocument = null;
        /// <summary>
        /// 页面所属文档对象
        /// </summary>
        public DomDocument Document
        {
            get
            {
                return myDocument; 
            }
            set
            {
                myDocument = value; 
            }
        }

		private PrintPageCollection myOwnerPages = null;

        private float intHeaderContentHeight = 0;
        /// <summary>
        /// 页眉内容高度
        /// </summary>
        public float HeaderContentHeight
        {
            get 
            {
                return intHeaderContentHeight; 
            }
            set
            {
                intHeaderContentHeight = value; 
            }
        }
        internal float PageHeaderLineFixOffset = 0;

        /// <summary>
        /// 采用视图单位的标准的页面正文内容区域上边缘的位置,DCWriter内部使用。
        /// </summary>
        public float StandartPageBodyTop
        {
            get
            {
                var ps = this.PageSettings;
                float result = ps.ViewTopMargin;
                    var result2 = ps.ViewHeaderDistance + this.HeaderContentHeight;
                    return Math.Max(result, result2);
            }
        }

        /// <summary>
        /// 采用视图单位的标准的页面正文内容区域高度,DCWriter内部使用。
        /// </summary>
        public float StandartPapeBodyHeight
        {
            get
            {
                XPageSettings ps = this.PageSettings;
                float height = ps.ViewClientHeight;
                    if (this.HeaderContentHeight > ps.ViewHeaderHeight)
                    {
                        height = height - (this.HeaderContentHeight - ps.ViewHeaderHeight);
                    }
                    if (this.FooterContentHeight > ps.ViewFooterHeight)
                    {
                        height = height - (this.FooterContentHeight - ps.ViewFooterHeight);
                    }
                return height;
            }
        }

        private float intFooterContentHeight = 0;
        /// <summary>
        /// 页脚内容高度
        /// </summary>
        public float FooterContentHeight
        {
            get
            {
                return intFooterContentHeight; 
            }
            set
            {
                intFooterContentHeight = value; 
            }
        }

        public float Left
        {
            get
            {
                return 0;// intLeft; 
            }
            //set
            //{
            //    intLeft = value; 
            //}
        }

        /// <summary>
        /// 获得实时的顶端位置
        /// </summary>
        private float GetRealtimeTop()
        {
            float result = myOwnerPages.Top;
            foreach (PrintPage myPage in myOwnerPages)
            {
                if (myPage == this)
                {
                    break;
                }
                result += myPage.Height;
            }
            return result;
        }

        private float _Top = -1;

        /// <summary>
        /// 获得打印页的顶端位置
        /// </summary>
        public float Top
        {
            get
            {
                return _Top ;

                //if (_Top < 0)
                //{
                //    _Top = this.RealtimeTop;
                //}
                //return 
                //float intTop = myOwnerPages.Top;
                //foreach (PrintPage myPage in myOwnerPages)
                //{
                //    if (myPage == this)
                //    {
                //        break;
                //    }
                //    intTop += myPage.Height;
                //}
                //return intTop;
            }
        }

        public void ResetTop()
        {
            this._Top = this.GetRealtimeTop();
        }


        private float intWidth = 0;
        /// <summary>
        /// 页面对象的宽度
        /// </summary>
        public float Width
        {
            get
            {
                return intWidth; 
            }
            set
            {
                intWidth = value; 
            }
        }
        
        private float _Height = 0;
        /// <summary>
        /// 页高
        /// </summary>
        public float Height
        {
            get
            {
                return _Height; 
            }
            set
            {
                _Height = value;
                FixHeight(); 
            }
        }

        public void SetHeightRaw( float h )
        {
            this._Height = h;
        }

        /// <summary>
        /// 标准页高
        /// </summary>
        public float ViewStandardHeight
        {
            get
            {
                return  myPageSettings.ViewClientHeight
                    - intHeaderContentHeight
                    - intFooterContentHeight ; 
            }
        }

        
        /// <summary>
        /// 设置,返回页面对象的底线
        /// </summary>
        public float Bottom
        {
            get
            {
                return this._Top + this._Height; 
            }
            set
            {
                this._Height = value - this._Top; 
                FixHeight(); 
            }
        }
        
        private void FixHeight()
        {
            if (_Height < myOwnerPages.MinPageHeight)
            {
                _Height = myOwnerPages.MinPageHeight;
            }
        }

        /// <summary>
        /// 从0开始计算的全局页码
        /// </summary>
        private int _GlobalIndex = 0;
        /// <summary>
        /// 从0开始计算的全局页码
        /// </summary>
        public int GlobalIndex
        {
            get 
            {
                return this._GlobalIndex; 
            }
            set
            {
                if (value != this._GlobalIndex)
                {
                    this._GlobalIndex = value;
                }
            }
        }


        private bool _OddPageIndex = false;
        /// <summary>
        /// 是否为奇数页。文档的页码是从0开始计算的。
        /// </summary>
        public bool OddPageIndex
        {
            get { return _OddPageIndex; }
            set { _OddPageIndex = value; }
        }

        private Margins _Margins = null;
        /// <summary>
        /// 页面本地使用的页边距信息对象
        /// </summary>
        public Margins Margins
        {
            get { return _Margins; }
            set { _Margins = value; }
        }

        /// <summary>
        /// 运行时使用的页边距设置
        /// </summary>
        public Margins RuntimeMargins
        {
            get
            {
                if (this._Margins != null)
                {
                    return this._Margins;
                }
                return this.myPageSettings.Margins ;
            }
        }


        public float ViewLeftMargin
        {
            get
            {
                return (float)GraphicsUnitConvert.Convert(
                    this.RuntimeMargins.Left ,
                    GraphicsUnit.Document,
                    GraphicsUnit.Document) * 3;
            }
        }
        public float ViewTopMargin
        {
            get
            {
                return (float)GraphicsUnitConvert.Convert(
                     this.RuntimeMargins.Top,
                     GraphicsUnit.Document,
                     GraphicsUnit.Document) * 3;
            }
        }

        public float ViewRightMargin
        {
            get
            {
                return (float)GraphicsUnitConvert.Convert(
                    this.RuntimeMargins.Right,
                    GraphicsUnit.Document,
                    GraphicsUnit.Document) * 3;
            }
        }

        public float ViewBottomMargin
        {
            get
            {
                return (float)GraphicsUnitConvert.Convert(
                    this.RuntimeMargins.Bottom,
                    GraphicsUnit.Document,
                    GraphicsUnit.Document) * 3;
            }
        }

        public float ViewPaperWidth
        {
            get
            {
                return myPageSettings.ViewPaperWidth;
            }
        }
        public float ViewPaperHeight
        {
            get
            {
                return myPageSettings.ViewPaperHeight;
            }
        }

         
		/// <summary>
		/// 页面对象在未分割的文档视图中的边框
		/// </summary>
		/// <returns></returns>
        public DCSystem_Drawing.RectangleF Bounds 
		{
			get
			{
				return new DCSystem_Drawing.RectangleF( 0 , this.Top , intWidth , _Height );
			}
		}

        public PageLayoutInfo ClientLayoutInfo = null;

        public Margins ClientMargins = new Margins();

        internal DCSystem_Drawing.Rectangle _ClientBounds = DCSystem_Drawing.Rectangle.Empty;
        public DCSystem_Drawing.Rectangle ClientBounds
        {
            get
            {
                return this._ClientBounds;
            }
            set
            {
                this._ClientBounds = value;
            }
        }

        public void SetClientBounds( int left , int top , int w , int h )
        {
            //if( this._ClientBounds.Height != h && this._ClientBounds.Height != 0 )
            //{
            //    var s = 2;
            //}
            this._ClientBounds = new DCSystem_Drawing.Rectangle(left, top, w, h);
        }
        public void SetClientBounds(DCSystem_Drawing.Rectangle rect)
        {
            //if( this._ClientBounds.Height != rect.Height && this._ClientBounds.Height != 0 )
            //{
            //    var ss = 333;
            //}
            this._ClientBounds = rect;
        }

        public const int ClientLeftFix = PageViewControl.PageSpacing;


        private bool _ForNewPage = false;
        /// <summary>
        /// 由于文档元素设置了强制分页而导致了分页
        /// </summary>
        public bool ForNewPage
        {
            get
            {
                return _ForNewPage;
            }
            set
            {
                _ForNewPage = value;
            }
        }
         
		/// <summary>
		/// 获得从0开始的页号
		/// </summary>
        public int PageIndex
		{
            get
            {
                if (myOwnerPages == null)
                {
                    return -1;
                }
                else
                {
                    return myOwnerPages.IndexOf(this);
                }
            }
		}
	}
}