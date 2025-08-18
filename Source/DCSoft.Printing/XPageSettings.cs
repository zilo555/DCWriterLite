using System;
using System.Text;
using System.ComponentModel;
//using System.Drawing.Design;
using DCSoft.Drawing;
// // 
//using System.Drawing.Printing;
using System.Collections.Generic;
using DCSoft.Common;

namespace DCSoft.Printing
{
    /// <summary>
    /// 页面设置对象
    /// </summary>
    public partial class XPageSettings :IDisposable
    {
        /// <summary>
        /// 静态构造函数
        /// </summary>
        static XPageSettings()
        {
        }

        /// <summary>
        /// 初始化对象
        /// </summary>
        public XPageSettings()
        {
        }

        private bool _PowerDocumentGridLine = false;
        /// <summary>
        /// 通篇强制绘制文档网格线
        /// </summary>
        public bool PowerDocumentGridLine
        {
            get
            {
                return _PowerDocumentGridLine;
            }
            set
            {
                _PowerDocumentGridLine = value;
            }
        }

        private DCGridLineInfo _DocumentGridLine = null;
        /// <summary>
        /// 文档网格线设置
        /// </summary>
        public DCGridLineInfo DocumentGridLine
        {
            get
            {
                return _DocumentGridLine;
            }
            set
            {
                _DocumentGridLine = value;
            }
        }

        private PaperKind intPaperKind = PaperKind.A4;
        /// <summary>
        /// 纸张尺寸类型
        /// </summary>
        public PaperKind PaperKind
        {
            get
            {
                return intPaperKind;
            }
            set
            {
                intPaperKind = value;
            }
        }

        private int _HeaderDistance = 50;
        /// <summary>
        /// 页眉顶端距离页面上边缘的距离，,单位百分之一英寸
        /// </summary>
        public int HeaderDistance
        {
            get
            {
                return _HeaderDistance;
            }
            set
            {
                _HeaderDistance = value;
                if (_HeaderDistance < 0)
                {
                    _HeaderDistance = 0;
                }
            }
        }

        /// <summary>
        /// 采用视图单位的页眉距离
        /// </summary>
        public float ViewHeaderDistance
        {
            get
            {
                return (float)GraphicsUnitConvert.Convert(
                        this.HeaderDistance,
                        GraphicsUnit.Document,
                        GraphicsUnit.Document) * 3;
            }
        }

        /// <summary>
        /// 获得页眉视图高度
        /// </summary>
        public float ViewHeaderHeight
        {
            get
            {
                return (float)GraphicsUnitConvert.Convert(
                    this.TopMargin - this.HeaderDistance,
                    GraphicsUnit.Document,
                    GraphicsUnit.Document) * 3;
            }
        }

        private int _FooterDistance = 50;
        /// <summary>
        /// 页脚低端距离页面下边缘的距离，,单位百分之一英寸
        /// </summary>
        public int FooterDistance
        {
            get
            {
                return _FooterDistance;
            }
            set
            {
                _FooterDistance = value;
            }
        }


        /// <summary>
        /// 获得页脚视图高度
        /// </summary>
        public float ViewFooterHeight
        {
            get
            {
                return (float)GraphicsUnitConvert.Convert(
                    this.BottomMargin - this._FooterDistance,
                    GraphicsUnit.Document,
                    GraphicsUnit.Document) * 3;
            }
        }


        private int _DesignerPaperWidth = 0;
        /// <summary>
        /// 设计器纸张宽度,单位百分之一英寸
        /// </summary>
        public int DesignerPaperWidth
        {
            get
            {
                return _DesignerPaperWidth;
            }
            set
            {
                _DesignerPaperWidth = value;
            }
        }

        private int intPaperWidth = 827;
        /// <summary>
        /// 纸张宽度,单位百分之一英寸
        /// </summary>
        public int PaperWidth
        {
            get
            {
                if (intPaperKind != PaperKind.Custom)
                {
                    var size = (DCSystem_Drawing.Size)StandardPaperSizeInfo.GetStandardPaperSize(intPaperKind);
                    if (size.IsEmpty == false)
                    {
                        return size.Width;
                    }
                }
                return intPaperWidth;
            }
            set
            {
                DesignerPaperWidth = value;
                intPaperWidth = value;
            }
        }

        private int _DesignerPaperHeight = 0;
        /// <summary>
        /// 设计器纸张高度
        /// </summary>

        public int DesignerPaperHeight
        {
            get { return _DesignerPaperHeight; }
            set { _DesignerPaperHeight = value; }
        }

        public DCSystem_Drawing.Size GetPageLayoutSize()
        {
            if (intPaperKind != PaperKind.Custom)
            {
                var size = StandardPaperSizeInfo.GetStandardPaperSize(intPaperKind);
                if (size.IsEmpty == false)
                {
                    return size;
                }
            }
            return new DCSystem_Drawing.Size(intPaperWidth, intPaperHeight);// intPaperHeight;
        }

        private int intPaperHeight = 1169;
        /// <summary>
        /// 纸张高度 单位百分之一英寸
        /// </summary>
        public int PaperHeight
        {
            get
            {
                if (intPaperKind != PaperKind.Custom)
                {
                    var size = StandardPaperSizeInfo.GetStandardPaperSize(intPaperKind);
                    if (size.IsEmpty == false)
                    {
                        return size.Height;
                    }
                }
                return intPaperHeight;
            }
            set
            {
                DesignerPaperHeight = value;
                intPaperHeight = value;
            }
        }

        private const int DefaultMarginValue = 100;
        private Margins _Margins = null;
        /// <summary>
        /// 页边距,单位百分之一英寸
        /// </summary>
        public Margins Margins
        {
            get
            {
                if (_Margins == null)
                {
                    _Margins = new Margins(DefaultMarginValue, DefaultMarginValue, DefaultMarginValue, DefaultMarginValue);
                }
                return _Margins;
            }
            set
            {
                _Margins = value;
            }
        }

        /// <summary>
        /// 左页边距 单位百分之一英寸
        /// </summary>
        public int LeftMargin
        {
            get
            {
                if (this._Margins == null)
                {
                    return DefaultMarginValue;
                }
                else
                {
                    return this._Margins.Left;
                }
            }
            set
            {
                this.Margins.Left = value;
            }
        }

        /// <summary>
        /// 顶页边距 单位百分之一英寸
        /// </summary>
        public int TopMargin
        {
            get
            {
                if (this._Margins == null)
                {
                    return DefaultMarginValue;
                }
                else
                {
                    return this._Margins.Top;
                }
            }
            set
            {
                this.Margins.Top = value;
            }
        }

        /// <summary>
        /// 右页边距 单位百分之一英寸
        /// </summary>
        public int RightMargin
        {
            get
            {
                if (this._Margins == null)
                {
                    return DefaultMarginValue;
                }
                else
                {
                    return this._Margins.Right;
                }
            }
            set
            {
                this.Margins.Right = value;
            }
        }

        /// <summary>
        /// 底页边距 单位百分之一英寸
        /// </summary>
        public int BottomMargin
        {
            get
            {
                if (this._Margins == null)
                {
                    return DefaultMarginValue;
                }
                else
                {
                    return this._Margins.Bottom;
                }
            }
            set
            {
                this.Margins.Bottom = value;
            }
        }

        /// <summary>
        /// 厘米为单位的下页边距
        /// </summary>
        public float BottomMarginInCM
        {
            get
            {
                return GraphicsUnitConvert.ConvertToCM(this.BottomMargin / 100f, GraphicsUnit.Inch);
            }
            set
            {
                this.BottomMargin = (int)(GraphicsUnitConvert.ConvertFromCM(value, GraphicsUnit.Inch) * 100f);
            }
        }
        /// <summary>
        /// 厘米为单位的左页边距
        /// </summary>
        public float LeftMarginInCM
        {
            get
            {
                return GraphicsUnitConvert.ConvertToCM(this.LeftMargin / 100f, GraphicsUnit.Inch);
            }
            set
            {
                this.LeftMargin = (int)(GraphicsUnitConvert.ConvertFromCM(value, GraphicsUnit.Inch) * 100f);
            }
        }
        /// <summary>
        /// 厘米为单位的上页边距
        /// </summary>
        public float TopMarginInCM
        {
            get
            {
                return GraphicsUnitConvert.ConvertToCM(this.TopMargin / 100f, GraphicsUnit.Inch);
            }
            set
            {
                this.TopMargin = (int)(GraphicsUnitConvert.ConvertFromCM(value, GraphicsUnit.Inch) * 100f);
            }
        }
        /// <summary>
        /// 厘米为单位的右页边距
        /// </summary>
        public float RightMarginInCM
        {
            get
            {
                return GraphicsUnitConvert.ConvertToCM(this.RightMargin / 100f, GraphicsUnit.Inch);
            }
            set
            {
                this.RightMargin = (int)(GraphicsUnitConvert.ConvertFromCM(value, GraphicsUnit.Inch) * 100f);
            }
        }

        private bool _Landscape = false;
        /// <summary>
        /// 横向打印标记
        /// </summary>
        public bool Landscape
        {
            get
            {
                return _Landscape;
            }
            set
            {
                _Landscape = value;
            }
        }

        /// <summary>
        /// 视图单位的左页边距
        /// </summary>
        public float ViewLeftMargin
        {
            get
            {
                return (float)GraphicsUnitConvert.Convert(
                    this.LeftMargin,
                    GraphicsUnit.Document,
                    GraphicsUnit.Document) * 3;
            }
        }

        /// <summary>
        /// 视图单位的顶页边距
        /// </summary>
        public float ViewTopMargin
        {
            get
            {
                return (float)GraphicsUnitConvert.Convert(
                    this.TopMargin,
                    GraphicsUnit.Document,
                    GraphicsUnit.Document) * 3;
            }
        }

        public DCSystem_Drawing.SizeF GetViewPaperSize()
        {
            var psize = GetPageLayoutSize();
            var rate = (float)GraphicsUnitConvert.GetRate(GraphicsUnit.Document, GraphicsUnit.Document) * 3;
            if (this.Landscape)
            {
                return new DCSystem_Drawing.SizeF(psize.Height * rate, psize.Width * rate);
            }
            else
            {
                return new DCSystem_Drawing.SizeF(psize.Width * rate, psize.Height * rate);
            }
        }

        /// <summary>
        /// 纸张的视图宽度
        /// </summary>
        public float ViewPaperWidth
        {
            get
            {
                return (float)GraphicsUnitConvert.Convert(
                    this._Landscape ? this.PaperHeight : this.PaperWidth,
                    GraphicsUnit.Document,
                    GraphicsUnit.Document)
                    * 3;
            }
        }

        /// <summary>
        /// 纸张的视图高度
        /// </summary>
        public float ViewPaperHeight
        {
            get
            {
                return (float)GraphicsUnitConvert.Convert(
                    (float)(this.Landscape ? this.PaperWidth : this.PaperHeight),
                    GraphicsUnit.Document,
                    GraphicsUnit.Document)
                    * 3.0f;
            }
        }

        /// <summary>
        /// 纸张可打印的客户区域的宽度,单位 Document
        /// </summary>
        public float ViewClientWidth
        {
            get
            {
                int w = this._Landscape ? this.PaperHeight : this.PaperWidth;
                w = w - this.LeftMargin - this.RightMargin;
                float result = (float)GraphicsUnitConvert.Convert(w, GraphicsUnit.Document, GraphicsUnit.Document) * 3;
                return result;
            }
        }

        /// <summary>
        /// 纸张可打印的客户区域的高度
        /// </summary>
        public float ViewClientHeight
        {
            get
            {
                int h = this.Landscape ? this.PaperWidth : this.PaperHeight;
                h = h - this.TopMargin - this.BottomMargin;
                float result = (float)GraphicsUnitConvert.Convert(h, GraphicsUnit.Document, GraphicsUnit.Document) * 3;
                return result;
            }
        }
        /// <summary>
        /// 复制对象
        /// </summary>
        /// <returns>复制品</returns>
        public XPageSettings Clone()
        {
            return (XPageSettings)this.MemberwiseClone();
        }

        private float ConvertToCM(float v)
        {
            return GraphicsUnitConvert.ConvertToCM(v * 3, GraphicsUnit.Document);
        }
#if !RELEASE
        private void AddValue(StringBuilder str, string name)
        {
            if (str.Length > 0)
            {
                str.Append(',');
            }
            str.Append(name);
        }
        private void AddValue(StringBuilder str, string name, string v)
        {
            if (str.Length > 0)
            {
                str.Append(',');
            }
            str.Append(name);
            str.Append('=');
            str.Append(v);
        }
        public override string ToString()
        {
            System.Text.StringBuilder str = new StringBuilder();
            str.Append(intPaperKind.ToString());
            if (this.PaperKind == PaperKind.Custom)
            {
                AddValue(str, "PaperWidth", this.PaperWidth + "(" + ConvertToCM(this.PaperWidth).ToString("0.00") + "CM)");
                AddValue(str, "PaperHeight", this.PaperHeight + "(" + ConvertToCM(this.PaperHeight).ToString("0.00") + "CM)");
            }
            if (this.Landscape)
            {
                AddValue(str, "Landscape");
            }
            if (this.LeftMargin != 100)
            {
                AddValue(str, "LeftMargin", this.LeftMargin + "(" + ConvertToCM(this.LeftMargin).ToString("0.00") + "CM)");
            }
            if (this.TopMargin != 100)
            {
                AddValue(str, "TopMargin", this.TopMargin + "(" + ConvertToCM(this.TopMargin).ToString("0.00") + "CM)");
            }
            if (this.RightMargin != 100)
            {
                AddValue(str, "RightMargin", this.RightMargin + "(" + ConvertToCM(this.RightMargin).ToString("0.00") + "CM)");
            }
            if (this.BottomMargin != 100)
            {
                AddValue(str, "BottomMargin", this.BottomMargin + "(" + ConvertToCM(this.BottomMargin).ToString("0.00") + "CM)");
            }
            if (this.HeaderDistance > 0)
            {
                AddValue(str, "HeaderDistance=" + this.HeaderDistance);
            }
            if (this.FooterDistance > 0)
            {
                AddValue(str, "FooterDistance=" + this.FooterDistance);
            }
            return str.ToString();
        }
#endif
        /// <summary>
        /// 销毁对象
        /// </summary>
        public void Dispose()
        {
        }
    }//public class XPageSettings
}