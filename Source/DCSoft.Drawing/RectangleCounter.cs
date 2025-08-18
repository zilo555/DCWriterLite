using System;

namespace DCSoft.Drawing
{
    /// <summary>
    /// 矩形区域累计对象
    /// </summary>
    public class RectangleFCounter
    {
        /// <summary>
        /// 初始化对象
        /// </summary>
        public RectangleFCounter()
        {
        }
        private float _Left = 0;
        private float _Top = 0;
        private float _Width = 0;
        private float _Height = 0;
        /// <summary>
        /// 添加矩形区域
        /// </summary>
        /// <param name="rect">矩形区域对象</param>
        public void Add(DCSystem_Drawing.RectangleF rect)
        {
            if (this._Width == 0 && this._Height == 0)
            {
                this._Left = rect.Left;
                this._Top = rect.Top;
                this._Width = rect.Width;
                this._Height = rect.Height;
            }
            else
            {
                float small = this._Left <= rect.Left ? this._Left : rect.Left;
                float big = this._Left + this._Width >= rect.Right ? this._Left + this._Width : rect.Right;
                this._Left = small;
                this._Width = big - small;

                small = this._Top <= rect.Top ? this._Top : rect.Top;
                big = this._Top + this._Height >= rect.Bottom ? this._Top + this._Height : rect.Bottom;
                this._Top = small;
                this._Height = big - small;
            }
        }
        /// <summary>
        /// 添加矩形区域
        /// </summary>
        /// <param name="left">左端位置</param>
        /// <param name="top">顶端位置</param>
        /// <param name="width">宽度</param>
        /// <param name="height">高度</param>
        public void Add(float left, float top, float width, float height)
        {
            if (this._Width == 0 && this._Height == 0)
            {
                this._Left = left;
                this._Top = top;
                this._Width = width;
                this._Height = height;
            }
            else
            {
                float small = this._Left < left ? this._Left : left;
                float big = this._Left + this._Width > left + width ? this._Left + this._Width : left + width;
                this._Left = small;
                this._Width = big - small;

                small = this._Top < top ? this._Top : top;
                big = this._Top + this._Height > top + height  ? this._Top + this._Height : top + height;
                this._Top = small;
                this._Height = big - small;
            }
        }
        /// <summary>
        /// 对象数据是否为空
        /// </summary>
        public bool IsEmpty
        {
            get { return this._Width == 0 && this._Height == 0 ; }
        }
        /// <summary>
        /// 当前数值
        /// </summary>
        public DCSystem_Drawing.RectangleF Value
        {
            get { return new DCSystem_Drawing.RectangleF( this._Left , this._Top , this._Width , this._Height); }
        }
    }//public class RectangleCounter
}