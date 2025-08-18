using System.Collections.Generic;
using System.IO;
using DCSoft.Common;
//using DCSoft.MyPDFDrawing;
namespace DCSoft.Drawing
{
    public class NewPDFGraphics : IDisposable
    {
        public NewPDFGraphics(DCSoft.SVG.SVGGraphics svg)
        {
            this._svg = svg;
        }
        private DCSoft.SVG.SVGGraphics _svg = null;
        public DCSoft.SVG.SVGGraphics SVG
        {
            get
            {
                return this._svg;
            }
        }

        public void Dispose()
        {
            if (this._svg != null)
            {
                this._svg.Dispose();
                this._svg = null;
            }
        }
        public void TranslateTransform(float dx, float dy)
        {
            if (this._svg != null)
            {
                this._svg.TranslateTransform(dx, dy);
            }
        }

        public Matrix Transform
        {
            get
            {
                if (this._svg != null)
                {
                    return this._svg.Transform;
                }
                return null;
            }
            set
            {
                if (this._svg != null)
                {
                    this._svg.Transform = value;
                }
            }
        }

        public GraphicsUnit PageUnit
        {
            get
            {
                if (this._svg != null)
                {
                    return this._svg.PageUnit;
                }
                return GraphicsUnit.Pixel;
            }
            set
            {
                if (this._svg != null)
                {
                    this._svg.PageUnit = value;
                }
            }
        }

        public void DrawImage(DCSystem_Drawing.Image img, DCSystem_Drawing.RectangleF rect, bool autoDisposeImage, byte[] nativeImageData)
        {
            if (this._svg != null)
            {
                // 输出OFD文档时不需要转化为JPG图片格式。
                this._svg.DrawImage(img, rect.X, rect.Y, rect.Width, rect.Height, nativeImageData);
                return;
            }
        }

        public void DrawLine(Pen pen, float x1, float y1, float x2, float y2)
        {
            if (this._svg != null)
            {
                this._svg.DrawLine(pen, x1, y1, x2, y2);
            }
        }

        public void DrawLines(Pen pen, DCSystem_Drawing.PointF[] points)
        {
            if (this._svg != null)
            {
                this._svg.DrawLines(pen, points);
            }
        }

        public void DrawRectangle(Pen pen, float x, float y, float width, float height, float roundRadio)
        {
            if (this._svg != null)
            {
                this._svg.DrawRectangle(pen, x, y, width, height, roundRadio);
            }
        }


        public void DrawString(string s, Font font, Brush brush, DCSystem_Drawing.RectangleF bounds, StringFormat format)
        {
            if (this._svg != null)
            {
                this._svg.DrawString(s, font.Name, font.Size, font.Style, brush is DCSystem_Drawing.SolidBrush ? ((DCSystem_Drawing.SolidBrush)brush).Color : DCSystem_Drawing.Color.Black, bounds, format);
            }
        }
        public void DrawString(string s, string fontName, float fontSize, DCSystem_Drawing.FontStyle vFontStyle, DCSystem_Drawing.Brush brush, DCSystem_Drawing.RectangleF bounds, DCSystem_Drawing.StringFormat format)
        {
            if (this._svg != null)
            {
                this._svg.DrawString(s, fontName, fontSize, vFontStyle, brush is DCSystem_Drawing.SolidBrush ? ((DCSystem_Drawing.SolidBrush)brush).Color : DCSystem_Drawing.Color.Black, bounds, format);
            }
        }

        public void FillRectangle(Brush brush, float x, float y, float width, float height, float roundRadio)
        {
            if (this._svg != null)
            {
                this._svg.FillRectangle(brush, x, y, width, height, roundRadio);
            }
        }


        public void SetClipRectangle(DCSystem_Drawing.RectangleF rect)
        {
            if (this._svg != null)
            {
                this._svg.SetClip(rect);
            }
        }

        public object Save()
        {
            if (this._svg != null)
            {
                return this._svg.SaveState();
            }
            return null;
        }
        public void Restore(object obj)
        {
            if (this._svg != null)
            {
                this._svg.RestoreState(obj);
            }
        }
    }
}
