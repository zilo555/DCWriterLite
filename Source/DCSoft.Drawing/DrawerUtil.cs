using System.Collections.Generic;
using System.Text;

namespace DCSoft.Drawing
{
	/// <summary>
	/// DrawerUtil 的摘要说明。
	/// </summary>
    public static class DrawerUtil
	{
        public static  Pen CreatePen(DCSystem_Drawing.Color c , float w , DCSystem_Drawing.Drawing2D.DashStyle style )
        {
            var p = new  Pen(c, w);
            p.DashStyle = style;
            return p;
        }
        /// <summary>
        /// 判断一个线段是否在一个剪切矩形中
        /// </summary>
        /// <param name="clipRectangle">剪切矩形</param>
        /// <param name="x1">线段坐标</param>
        /// <param name="y1">线段坐标</param>
        /// <param name="x2">线段坐标</param>
        /// <param name="y2">线段坐标</param>
        /// <returns>是否可见</returns>
        public static bool IsLineInClipRectangle(DCSystem_Drawing.RectangleF clipRectangle , float x1 , float y1 , float x2 , float y2 )
        {
            if(clipRectangle.IsEmpty )
            {
                return true;
            }
            if( x1 == x2 )
            {
                // 竖线
                if( x1 < clipRectangle.Left || x1 > clipRectangle.Right )
                {
                    return false;
                }
                if(y1 > y2 )
                {
                    var temp = y1;
                    y2 = y1;
                    y1 = temp;
                }
                if( y1 > clipRectangle.Bottom || y2 < clipRectangle.Top )
                {
                    return false;
                }
            }
            else if( y1 == y2 )
            {
                // 横线
                if( y1 < clipRectangle.Top || y1 > clipRectangle.Bottom )
                {
                    return false;
                }
                if( x1 > x2 )
                {
                    var temp = x1;
                    x1 = x2;
                    x2 = temp ;
                }
                if( x1 > clipRectangle.Right || x2 < clipRectangle.Left )
                {
                    return false;
                }
            }
            else
            {
                //  斜线
                throw new NotSupportedException("oblique line:002");
            }
            return true ;
        }
        /// <summary>
        /// 设置字符串格式为多行文本
        /// </summary>
        /// <param name="multiLine">是否多行文本</param>
        public static void SetMultiLine( StringFormat format, bool multiLine)
        {
            if( format == null )
            {
                throw new ArgumentNullException("format");
            }
            var flags = format.FormatFlags;
            var newFlags = flags;
            if (multiLine)
            {
                newFlags = flags & ~StringFormatFlags.NoWrap;
            }
            else
            {
                newFlags = flags | StringFormatFlags.NoWrap;
            }
            if( flags != newFlags)
            {
                format.FormatFlags = newFlags;
            }
        }

        /// <summary>
        /// 安全的在内存中创建画布对象
        /// </summary>
        /// <returns>创建的对象</returns>
        public static Graphics SafeCreateGraphics()
        {
            return new Graphics();
        }

        /// <summary>
        /// 判断是否为页眉页脚样式
        /// </summary>
        /// <param name="style"></param>
        /// <returns></returns>
        public static bool IsHeaderFooter(PageContentPartyStyle style)
        {
            return style == PageContentPartyStyle.Header
                || style == PageContentPartyStyle.Footer;
        }

        public static void DrawImageUnscaledNearestNeighbor(DCGraphics g, Image img, int x, int y)
        {
            g.DrawImageUnscaled(img, x, y);
        }

        /// <summary>
        /// 修正剪切矩形
        /// </summary>
        /// <param name="g"></param>
        /// <param name="left"></param>
        /// <param name="top"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public static DCSystem_Drawing.RectangleF FixClipBounds( 
            DCGraphics g ,
            float left , 
            float top , 
            float width ,
            float height )
		{
			 GraphicsUnit unit = g.PageUnit ;
			double ratex = 96f;
			double ratey = 96f;
			switch( unit )
			{
				case  GraphicsUnit.Document :
					ratex = 300 ;
					ratey = 300 ;
					break;
				case  GraphicsUnit.Inch :
					ratex = 1 ;
					ratey = 1 ;
					break;
				case  GraphicsUnit.Millimeter :
					ratex = 25.4 ;
					ratey = 25.4 ;
					break;
				case  GraphicsUnit.Point :
					ratex = 72 ;
					ratey = 72 ;
					break;
			}
			ratex = ratex / 96f;
			ratey = ratey / 96f;

			double left2 = Math.Ceiling(  left / ratex ) * ratex ;
			double top2 = Math.Ceiling( top / ratey ) * ratey ;
			double width2 = Math.Ceiling( width / ratex ) * ratex ;
			double height2 = Math.Ceiling( height / ratey ) * ratey ;

			if( left2 > left )
			{
				left2 = left2 - ratex ;
                if (width2 < width)
                {
                    width2 += ratex;
                }
				width2 += ratex ;
			}
			if( top2 > top )
			{
				top2 = top - ratey ;
                if (height2 < height)
                {
                    height2 += ratey;
                }
				height2 += ratey ;
			}
			return new DCSystem_Drawing.RectangleF(
                ( float ) left2 , 
                ( float ) top2 , 
                ( float ) width2 ,
                ( float ) height2 );
		}
    }



    public struct HLSColor
    {
        //private const int ShadowAdj = -333;
        //private const int HilightAdj = 500;
        //private const int WatermarkAdj = -50;
        //private const int Range = 240;
        //private const int HLSMax = 240;
        //private const int RGBMax = 0xff;
        //private const int Undefined = 160;
        private readonly int hue;
        private readonly int saturation;
        private readonly int luminosity;
        //private bool isSystemColors_Control;
        public HLSColor(Color color)
        {
            //this.isSystemColors_Control = color.ToKnownColor() == DCSystem_Drawing.SystemColors.Control.ToKnownColor();
            int r = color.R;
            int g = color.G;
            int b = color.B;
            int num4 = Math.Max(Math.Max(r, g), b);
            int num5 = Math.Min(Math.Min(r, g), b);
            int num6 = num4 + num5;
            this.luminosity = ((num6 * 240) + 0xff) / 510;
            int num7 = num4 - num5;
            if (num7 == 0)
            {
                this.saturation = 0;
                this.hue = 160;
            }
            else
            {
                if (this.luminosity <= 120)
                {
                    this.saturation = ((num7 * 240) + (num6 / 2)) / num6;
                }
                else
                {
                    this.saturation = ((num7 * 240) + ((510 - num6) / 2)) / (510 - num6);
                }
                int num8 = (((num4 - r) * 40) + (num7 / 2)) / num7;
                int num9 = (((num4 - g) * 40) + (num7 / 2)) / num7;
                int num10 = (((num4 - b) * 40) + (num7 / 2)) / num7;
                if (r == num4)
                {
                    this.hue = num10 - num9;
                }
                else if (g == num4)
                {
                    this.hue = (80 + num8) - num10;
                }
                else
                {
                    this.hue = (160 + num9) - num8;
                }
                if (this.hue < 0)
                {
                    this.hue += 240;
                }
                if (this.hue > 240)
                {
                    this.hue -= 240;
                }
            }
        }

        public Color Darker(float percDarker)
        {
            //if (this.isSystemColors_Control)
            //{
            //    if (percDarker == 0f)
            //    {
            //        return DCSystem_Drawing.SystemColors.ControlDark;
            //    }
            //    if (percDarker == 1f)
            //    {
            //        return DCSystem_Drawing.SystemColors.ControlDarkDark;
            //    }
            //    DCSystem_Drawing.Color controlDark = DCSystem_Drawing.SystemColors.ControlDark;
            //    DCSystem_Drawing.Color controlDarkDark = DCSystem_Drawing.SystemColors.ControlDarkDark;
            //    int num = controlDark.R - controlDarkDark.R;
            //    int num2 = controlDark.G - controlDarkDark.G;
            //    int num3 = controlDark.B - controlDarkDark.B;
            //    return DCSystem_Drawing.Color.FromArgb((byte)(controlDark.R - ((byte)(num * percDarker))), (byte)(controlDark.G - ((byte)(num2 * percDarker))), (byte)(controlDark.B - ((byte)(num3 * percDarker))));
            //}
            int num4 = 0;
            int num5 = this.NewLuma(-333, true);
            return this.ColorFromHLS(this.hue, num5 - ((int)((num5 - num4) * percDarker)), this.saturation);
        }

        public Color Lighter(float percLighter)
        {
            //if (this.isSystemColors_Control)
            //{
            //    if (percLighter == 0f)
            //    {
            //        return SystemColors.ControlLight;
            //    }
            //    if (percLighter == 1f)
            //    {
            //        return SystemColors.ControlLightLight;
            //    }
            //    Color controlLight = SystemColors.ControlLight;
            //    Color controlLightLight = SystemColors.ControlLightLight;
            //    int num = controlLight.R - controlLightLight.R;
            //    int num2 = controlLight.G - controlLightLight.G;
            //    int num3 = controlLight.B - controlLightLight.B;
            //    return Color.FromArgb((byte)(controlLight.R - ((byte)(num * percLighter))), (byte)(controlLight.G - ((byte)(num2 * percLighter))), (byte)(controlLight.B - ((byte)(num3 * percLighter))));
            //}
            int luminosity = this.luminosity;
            int num5 = this.NewLuma(500, true);
            return this.ColorFromHLS(this.hue, luminosity + ((int)((num5 - luminosity) * percLighter)), this.saturation);
        }

        private int NewLuma(int n, bool scale)
        {
            return this.NewLuma(this.luminosity, n, scale);
        }

        private int NewLuma(int luminosity, int n, bool scale)
        {
            if (n == 0)
            {
                return luminosity;
            }
            if (scale)
            {
                if (n > 0)
                {
                    return (int)(((luminosity * (0x3e8 - n)) + (0xf1L * n)) / 0x3e8L);
                }
                return ((luminosity * (n + 0x3e8)) / 0x3e8);
            }
            int num = luminosity;
            num += (int)((n * 240L) / 0x3e8L);
            if (num < 0)
            {
                num = 0;
            }
            if (num > 240)
            {
                num = 240;
            }
            return num;
        }

        private DCSystem_Drawing.Color ColorFromHLS(int hue, int luminosity, int saturation)
        {
            byte num;
            byte num2;
            byte num3;
            if (saturation == 0)
            {
                num = num2 = num3 = (byte)((luminosity * 0xff) / 240);
                if (hue == 160)
                {
                }
            }
            else
            {
                int num5;
                if (luminosity <= 120)
                {
                    num5 = ((luminosity * (240 + saturation)) + 120) / 240;
                }
                else
                {
                    num5 = (luminosity + saturation) - (((luminosity * saturation) + 120) / 240);
                }
                int num4 = (2 * luminosity) - num5;
                num = (byte)(((this.HueToRGB(num4, num5, hue + 80) * 0xff) + 120) / 240);
                num2 = (byte)(((this.HueToRGB(num4, num5, hue) * 0xff) + 120) / 240);
                num3 = (byte)(((this.HueToRGB(num4, num5, hue - 80) * 0xff) + 120) / 240);
            }
            return DCSystem_Drawing.Color.FromArgb(num, num2, num3);
        }

        private int HueToRGB(int n1, int n2, int hue)
        {
            if (hue < 0)
            {
                hue += 240;
            }
            if (hue > 240)
            {
                hue -= 240;
            }
            if (hue < 40)
            {
                return (n1 + ((((n2 - n1) * hue) + 20) / 40));
            }
            if (hue < 120)
            {
                return n2;
            }
            if (hue < 160)
            {
                return (n1 + ((((n2 - n1) * (160 - hue)) + 20) / 40));
            }
            return n1;
        }
    }
}
