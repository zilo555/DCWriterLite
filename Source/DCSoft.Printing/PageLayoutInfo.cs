using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing ;
using DCSoft.Drawing;

namespace DCSoft.Printing
{
    /// <summary>
    /// 页码内容排版信息
    /// </summary>
    public class PageLayoutInfo
    {
        public PageLayoutInfo( XPageSettings ps , bool convertToViewUnit , bool oddPageIndex )
        {
            InnerUpdateState(null, ps, convertToViewUnit, oddPageIndex);
        }

        public PageLayoutInfo(PrintPage page , bool convertToViewUnit = false )
        {
            if (page == null)
            {
                throw new ArgumentNullException("page");
            }
            InnerUpdateState(page, null, convertToViewUnit, page.OddPageIndex);
        }

        private void InnerUpdateState( PrintPage page , XPageSettings ps , bool convertToViewUnit , bool oddPageIndex )
        {
            var jiejie = "JIEJIE.NET.SWITCH:-controlfow";

            if (ps == null && page != null )
            {
                ps = page.PageSettings;
            }
            if (ps == null)
            {
                throw new ArgumentNullException("ps");
            }

            var mg = page == null ? ps.Margins : page.RuntimeMargins;

            this._LeftMargin = mg.Left;
            this._TopMargin = mg.Top;
            this._RightMargin = mg.Right;
            this._BottomMargin = mg.Bottom;
                this._HeaderDistance = ps.HeaderDistance;
                this._FooterDistance = ps.FooterDistance;
            var pss9 = ps.GetPageLayoutSize();
            //this._PageWidth = pss9.Width;
            //this._PageHeight = pss9.Height;
            if (ps.Landscape)
            {
                // 横向打印
                this._PageWidth = pss9.Height;
                this._PageHeight = pss9.Width;
            }
            else
            {
                this._PageWidth = pss9.Width;
                this._PageHeight = pss9.Height;
            }
            if(convertToViewUnit)
            {
                ConvertUnit( GraphicsUnit.Document);
            }
        }
        /// <summary>
        /// 将标准度量单位(百分之一英寸)转换为指定的度量单位
        /// </summary>
        /// <param name="targetUnit">目标度量单位。</param>
        /// <remarks>在数据初始化后，默认的度量单位是百分之一英寸。</remarks>
        public void ConvertUnit(GraphicsUnit targetUnit)
        {
            float rate = (float)GraphicsUnitConvert.GetRate(targetUnit, GraphicsUnit.Document) * 3;
            Zoom(rate);
        }

        /// <summary>
        /// 进行缩放
        /// </summary>
        /// <param name="rate">缩放比率</param>
        public void Zoom(float rate)
        {
            this._LeftMargin *= rate;
            this._TopMargin *= rate;
            this._RightMargin *= rate;
            this._BottomMargin *= rate;
            this._HeaderDistance *= rate;
            this._FooterDistance *= rate;
            this._PageWidth *= rate;
            this._PageHeight *= rate;
        }

        private float _PageWidth = 0;
        /// <summary>
        /// 页面宽度
        /// </summary>
        public float PageWidth
        {
            get { return _PageWidth; }
        }

        private float _PageHeight = 0;
        /// <summary>
        /// 页面高度
        /// </summary>
        public float PageHeight
        {
            get { return _PageHeight; }
        }

        private float _LeftMargin = 0;
        /// <summary>
        /// 左页边距
        /// </summary>
        public float LeftMargin
        {
            get { return _LeftMargin; }
        }

        private float _TopMargin = 0;
        /// <summary>
        /// 上页边距
        /// </summary>
        public float TopMargin
        {
            get { return _TopMargin; }
        }

        private float _RightMargin = 0;
        /// <summary>
        /// 右页边距
        /// </summary>
        public float RightMargin
        {
            get { return _RightMargin; }
        }

        private float _BottomMargin = 0;
        /// <summary>
        /// 下页边距
        /// </summary>
        public float BottomMargin
        {
            get { return _BottomMargin; }
        }

        private float _HeaderDistance = 0;
        /// <summary>
        /// 页眉距离页面顶端距离
        /// </summary>
        public float HeaderDistance
        {
            get { return _HeaderDistance; }
        }

        private float _FooterDistance = 0;
        /// <summary>
        /// 页脚下边缘距离页码下边缘的距离
        /// </summary>
        public float FooterDistance
        {
            get { return _FooterDistance; }
        }
    }
}
