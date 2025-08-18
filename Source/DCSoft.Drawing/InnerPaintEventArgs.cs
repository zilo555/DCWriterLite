using System;
using System.Collections.Generic;
using System.Text;
// // 

namespace DCSoft.Drawing
{
    /// <summary>
    /// 内部的绘图事件参数
    /// </summary>
    public class InnerPaintEventArgs //: GraphicsMemberPackage
    {
        /// <summary>
        /// 初始化对象
        /// </summary>
        /// <param name="g">画布对象</param>
        /// <param name="clip">剪切矩形</param>
        public InnerPaintEventArgs(Graphics g, DCSystem_Drawing.Rectangle clip)//:base( g )
        {
            if (g == null)
            {
                throw new ArgumentNullException("g");
            }
            this._Graphics = new DCGraphics(g);
            this._ClipRectangle = clip;
        }
        /// <summary>
        /// 初始化对象
        /// </summary>
        /// <param name="g">画布对象</param>
        /// <param name="clip">剪切矩形</param>
        public InnerPaintEventArgs(DCGraphics g, DCSystem_Drawing.Rectangle clip)//:base( g )
        {
            if (g == null)
            {
                throw new ArgumentNullException("g");
            }
            this._Graphics = g;
            this._ClipRectangle = clip;
        }
        private DCGraphics _Graphics = null;
        public DCGraphics Graphics
        {
            get
            {
                return this._Graphics;
            }
        }

        private readonly DCSystem_Drawing.Rectangle _ClipRectangle = DCSystem_Drawing.Rectangle.Empty;
        /// <summary>
        /// 剪切矩形
        /// </summary>
        public DCSystem_Drawing.Rectangle ClipRectangle
        {
            get
            {
                return _ClipRectangle;
            }
        }

        public DCSystem_Drawing.Rectangle IntersectWithClipRectangle(DCSystem_Drawing.Rectangle rect )
        {
            if( this._ClipRectangle.IsEmpty )
            {
                return rect;
            }
            else
            {
                return DCSystem_Drawing.Rectangle.Intersect(rect, this._ClipRectangle);
            }
        }
    }
}
