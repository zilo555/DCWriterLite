using System;
using DCSoft.Common;
using System.ComponentModel;
using DCSoft.Drawing ;

namespace DCSoft.Printing
{
    /// <summary>
    /// 网格线信息对象
    /// </summary>
    public partial class DCGridLineInfo
    {
        /// <summary>
        ///初始化对象
        /// </summary>
        public DCGridLineInfo()
        {
        }
        /// <summary>
        /// 对象数据是空白的
        /// </summary>
        public bool IsEmpty
        {
            get
            {
                return this._Visible == false && this._GridNumInOnePage == 0 && this._GridSpanInCM == 0;
            }
        }
        private bool _Visible = false;
        /// <summary>
        /// 对象可见
        /// </summary>
        public bool Visible
        {
            get
            {
                return _Visible;
            }
            set
            {
                _Visible = value;
            }
        }

        private bool _AlignToGridLine = true;
        /// <summary>
        /// 对齐到网格线
        /// </summary>
        public bool AlignToGridLine
        {
            get
            {
                return _AlignToGridLine; 
            }
            set
            {
                _AlignToGridLine = value; 
            }
        }
        /// <summary>
        /// 判断是否为透明色
        /// </summary>
        /// <returns></returns>
        public bool IsTransparent()
        {
            return this.Color.A == 0;
        }

        private Color _Color = Color.Black;
        /// <summary>
        /// 网格线颜色
        /// </summary>
        public Color Color
        {
            get
            {
                return _Color;
            }
            set
            {
                _Color = value;
            }
        }

        /// <summary>
        /// 颜色值
        /// </summary>
        public string ColorValue
        {
            get
            {
                return XMLSerializeHelper.ColorToString(this.Color, Color.Black);
            }
            set
            {
                this.Color = XMLSerializeHelper.StringToColor(value, Color.Black);
            }
        }

        private float _LineWidth = 1f;
        /// <summary>
        /// 网格线宽度
        /// </summary>
        public float LineWidth
        {
            get
            {
                return _LineWidth;
            }
            set
            {
                _LineWidth = value;
            }
        }

        private int _GridNumInOnePage = 0;
        /// <summary>
        /// 每页网格线行数。该属性仅对文档正文对象有效
        /// </summary>
        public int GridNumInOnePage
        {
            get
            {
                return _GridNumInOnePage; 
            }
            set
            {
                _GridNumInOnePage = value; 
            }
        }

        private float _GridSpanInCM = 0f;
        /// <summary>
        /// 厘米为单位的网格线跨度
        /// </summary>
        public float GridSpanInCM
        {
            get
            {
                return _GridSpanInCM; 
            }
            set
            {
                _GridSpanInCM = value; 
            }
        }
         
        private DashStyle _LineStyle = DashStyle.Solid;
        /// <summary>
        /// 线条样式
        /// </summary>
        public DashStyle LineStyle
        {
            get
            {
                return _LineStyle;
            }
            set
            {
                _LineStyle = value;
            }
        }

        private bool _Printable = true;
        /// <summary>
        /// 打印
        /// </summary>
        public bool Printable
        {
            get
            {
                return _Printable;
            }
            set
            {
                _Printable = value;
            }
        }

        private float _RuntimeGridSpan = 0;
        /// <summary>
        /// 运行时使用的网格线间距。DCWriter内部使用。
        /// </summary>
        public float RuntimeGridSpan
        {
            get
            {
                return _RuntimeGridSpan; 
            }
            //set
            //{
            //    _RuntimeGridSpan = value; 
            //}
        }
        /// <summary>
        /// 更新运行时网格线跨度。DCWriter内部使用。
        /// </summary>
        /// <param name="pageBodyHeight">页面正文区域高度</param>
        /// <param name="unit">视图单位</param>
        /// <param name="zoomRate">缩放比率</param>
        public void UpdateRuntimeGridSpan(float pageBodyHeight, GraphicsUnit unit , float zoomRate )
        {
            if (this.Visible)
            {
                if (this.GridNumInOnePage > 0 && pageBodyHeight > 0 )
                {
                    this._RuntimeGridSpan = zoomRate * (pageBodyHeight - 2) / this.GridNumInOnePage;
                }
                else
                {
                    this._RuntimeGridSpan = zoomRate * DCSoft.Drawing.GraphicsUnitConvert.ConvertFromCM(this.GridSpanInCM, unit);
                }
            }
        }

        /// <summary>
        /// 修正文档行高度。DCWriter内部使用。
        /// </summary>
        /// <param name="height"></param>
        /// <returns></returns>
        public float FixLineHeight(float height)
        {
            if (this._Visible && this._AlignToGridLine && this._RuntimeGridSpan > 0)
            {
                float v = height / this._RuntimeGridSpan;
                float v2 = (float)(v - Math.Floor(v)) ;
                if( v2 > 0.05 )
                {
                    v = (float)Math.Ceiling(v);
                }
                else
                {
                    v = (float)Math.Floor(v);
                }
                v = v * this._RuntimeGridSpan;
                return v;
            }
            return height;
        }
        /// <summary>
        /// 运行时的对齐到网格线的设置
        /// </summary>
        public bool RuntimeAlignToGridLine
        {
            get
            {
                return this._Visible && this._AlignToGridLine && this._RuntimeGridSpan > 0;
            }
        }

        /// <summary>
        /// 创建画笔对象。DCWriter内部使用。
        /// </summary>
        /// <returns></returns>

        public Pen CreatePen()
        {
            Pen p = new Pen(this._Color, this._LineWidth);
            p.DashStyle = this.LineStyle;
            return p;
        }

        /// <summary>
        /// 复制对象
        /// </summary>
        /// <returns>复制的对象</returns>
        public DCGridLineInfo Clone()
        {
            return (DCGridLineInfo)this.MemberwiseClone();
        }
    }
}
