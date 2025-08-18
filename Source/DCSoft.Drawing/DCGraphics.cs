using DCSoft.Writer;
using DCSoft.SVG;

// 袁永福到此一游

namespace DCSoft.Drawing
{
    /// <summary>
    /// 画布对象
    /// </summary>
    public class DCGraphics : IDisposable
    {

        protected DCGraphics() { }

        /// <summary>
        /// 初始化对象
        /// </summary>
        /// <param name="g"></param>
        public DCGraphics(Graphics g)
        {
            if (g == null)
            {
                throw new ArgumentNullException("g");
            }
            this._Graphis = g;
            this._PageUnitFast = g.PageUnit;
        }
        private Graphics _Graphis = null;

        /// <summary>
        /// 原始的画布对象
        /// </summary>

        public Graphics NativeGraphics
        {
            get
            {
                return _Graphis;
            }
        }


        public static DCGraphics FromPDFGraphics(NewPDFGraphics g)
        {
            DCGraphics result = new DCGraphics();
            result._PDFGraphics = g;
            result._PageUnitFast = g.PageUnit;
            return result;
        }

        private NewPDFGraphics _PDFGraphics = null;
        public NewPDFGraphics PDFGraphics
        {
            get
            {
                return _PDFGraphics;
            }
        }
        public SVGGraphics SVG
        {
            get
            {
                return this._PDFGraphics?.SVG;
            }
        }
        public bool IsSVGMode
        {
            get
            {
                return this._PDFGraphics != null && this._PDFGraphics.SVG != null;
            }
        }

        public bool IsPDFMode
        {
            get
            {
                return _PDFGraphics != null;
            }
        }

        public void Clear(DCSystem_Drawing.Color c)
        {
            if (this._Graphis != null)
            {
                this._Graphis.Clear(c);
            }
            //if (this._Recorder != null)
            //{
            //    this._Recorder.Clear(c);
            //}
        }
        public virtual void ResetClip()
        {
            this._Graphis?.ResetClip();
            this._PDFGraphics?.SetClipRectangle(RectangleF.Empty);
            //if(this._NewRecorder != null )
            //{
            //    this._NewRecorder.ResetClip();
            //    return;
            //}
            //if (this._Recorder != null)
            //{
            //    this._Recorder.ResetClip();
            //}
            //if (_PDFGraphics != null)
            //{
            //    _PDFGraphics.SetClipRectangle(Rectangle.Empty);
            //    //_PDFGraphics.Clip = null;
            //    return;
            //}

            //if (_Graphis != null)
            //{
            //    _Graphis.ResetClip();
            //}

        }

        /// <summary>
        /// 获得字体高度
        /// </summary>
        /// <param name="font"></param>
        /// <returns></returns>
        public virtual float GetFontHeight(XFontValue font)
        {
            if (font == null)
            {
                throw new ArgumentNullException("font");
            }
            try
            {
                if (this._Graphis != null)
                {
                    return font.GetHeight(this._Graphis);
                }
                else if (this.GraphisForMeasure != null)
                {
                    return font.GetHeight(this.GraphisForMeasure);
                }
                else
                {
                    return font.GetHeight();
                }
            }
            catch (System.Exception ext)
            {
                DCConsole.Default.WriteLine("GetFontHeight报错。" + Environment.NewLine + ext.ToString());
                //XFontValue.ClearBuffer();
                throw;
            }
            return font.Size;
        }
        private GraphicsUnit _PageUnitFast = GraphicsUnit.Document;

        public virtual GraphicsUnit PageUnit
        {
            get
            {
                if (_PDFGraphics != null)
                {
                    return _PDFGraphics.PageUnit;
                }
                if (this._Graphis == null)
                {
                    return GraphicsUnit.Pixel;
                }
                else
                {
                    return this._Graphis.PageUnit;
                }
            }
            set
            {
                //if(this._NewRecorder != null )
                //{
                //    this._NewRecorder.SetPageUnit(value);
                //    return;
                //}
                //if (this._Recorder != null)
                //{
                //    this._Recorder.PageUnit = value;
                //}
                if (_PDFGraphics != null)
                {
                    _PDFGraphics.PageUnit = value;
                }
                if (this._Graphis != null)
                {
                    this._Graphis.PageUnit = value;
                }
                this._PageUnitFast = value;
                //if (_Writer != null)
                //{
                //    WriteRecordType(DCMetaRecordType.PageUnit);
                //    _Writer.Write((int)value);
                //}
            }
        }


        public virtual void TransformPoints(DCSystem_Drawing.PointF[] pts)
        {
            //if (this._MyMartrix == null)
            {
                this.Transform.TransformPoints(pts);
            }
            //else
            //{
            //    this._MyMartrix.TransformPoints(pts);
            //}
        }


        /// <summary>
        /// 转换画笔线条宽度
        /// </summary>
        /// <param name="vValue">旧宽度</param>
        /// <param name="OldUnit">旧单位</param>
        /// <param name="NewUnit">新单位</param>
        /// <returns>新宽度</returns>
        public static float ConvertPenWidth(
           float vValue,
           GraphicsUnit OldUnit,
           GraphicsUnit NewUnit)
        {
            //if (LinuxMode)
            //{
            //    return vValue;
            //}
            //else
            {
                return GraphicsUnitConvert.Convert(vValue, OldUnit, NewUnit);
            }
        }

        public bool AutoSetInnerMatrix()
        {
            ////var os = System.Environment.OSVersion.Platform;
            //if (LinuxMode )// os == PlatformID.MacOSX || os == PlatformID.Unix )
            //{
            //    this._MyMartrix = new MyMartrixClass(this._Graphis.Transform);
            //    this._Graphis.ResetTransform();
            //    //this._Graphis.ResetClip();
            //    return true;
            //}
            return false;
        }
        public void CleanInnerMatrix()
        {
            //if (this._MyMartrix != null)
            //{
            //    this._Graphis.Transform = this._MyMartrix.CreateMatrix();
            //    this._MyMartrix = null;
            //}
        }
        public virtual Matrix Transform
        {
            get
            {
                if (_Graphis != null)
                {
                    return _Graphis.Transform;
                }
                else if (_PDFGraphics != null)
                {
                    return _PDFGraphics.Transform;

                    //if (_Transform == null)
                    //{
                    //    _Transform = new Matrix();
                    //}
                    //return _Transform;
                }
                return null;
            }
            set
            {
                //if( this._NewRecorder != null )
                //{
                //    this._NewRecorder.SetTransform(value);
                //    return;
                //}
                //if (this._Recorder != null)
                //{
                //    this._Recorder.Transform = value;
                //}
                if (_PDFGraphics != null)
                {
                    _PDFGraphics.Transform = value;
                }
                if (_Graphis != null)
                {
                    _Graphis.Transform = value;
                }
            }
        }

        public virtual void ResetTransform()
        {
            this._Graphis?.ResetTransform();
            //if(this._NewRecorder != null )
            //{
            //    this._NewRecorder.SetTransform(null);
            //    return;
            //}
            //if (this._Recorder != null)
            //{
            //    this._Recorder.ResetTransform();
            //}
            if (_PDFGraphics != null)
            {
                _PDFGraphics.TranslateTransform(0, 0);
                this.Transform.Reset();
                return;
            }
        }

        public virtual void TranslateTransform(float x, float y)
        {
            this._Graphis?.TranslateTransform(x, y);
            //if( this._NewRecorder != null )
            //{
            //    this._NewRecorder.TranslateTransform(x, y);
            //    return;
            //}
            //if (this._Recorder != null)
            //{
            //    this._Recorder.TranslateTransform(x, y);
            //}
            if (this._PDFGraphics != null)
            {
                this._PDFGraphics.Transform.Translate(x, y);
                return;
            }

            //if (_Graphis != null)
            //{
            //    if (this._MyMartrix != null)
            //    {
            //        this._MyMartrix.SetTransform(x, y);
            //        //this._MyMartrix.SetMatrix(_Graphis.Transform);
            //    }
            //    else
            //    {
            //        this._Graphis.TranslateTransform(x, y);
            //    }
            //}
        }

        public virtual void ScaleTransform(float sx, float sy)
        {
            this._Graphis?.ScaleTransform(sx, sy);
            this._PDFGraphics?.Transform.Scale(sx, sy);
            ////if( this._NewRecorder != null )
            ////{
            ////    this._NewRecorder.ScaleTransform(sx, sy);
            ////    return;
            ////}
            ////if (this._Recorder != null)
            ////{
            ////    this._Recorder.ScaleTransform(sx, sy);
            ////}
            //if (this._PDFGraphics != null)
            //{
            //    this._PDFGraphics.Transform.Scale(sx, sy);
            //    return;
            //}

            //if (this._Graphis != null)
            //{
            //    if (this._MyMartrix != null)
            //    {
            //        this._MyMartrix.SetScaleTransform(sx, sy);
            //    }
            //    else
            //    {
            //        this._Graphis.ScaleTransform(sx, sy);
            //    }
            //}
        }
        public void DrawImage(DCSystem_Drawing.Image img, float x, float y, byte[] nativeImageData = null)
        {
            if (_PDFGraphics != null)
            {
                float width = GraphicsUnitConvert.Convert(
                    img.Width,
                    DCSystem_Drawing.GraphicsUnit.Pixel,
                    _PDFGraphics.PageUnit);
                float height = GraphicsUnitConvert.Convert(
                    img.Height,
                    DCSystem_Drawing.GraphicsUnit.Pixel,
                    _PDFGraphics.PageUnit);
                _PDFGraphics.DrawImage(img, new DCSystem_Drawing.RectangleF(x, y, width, height), this.AutoDisposeImageForPDF, nativeImageData);
            }
            if (_Graphis != null)
            {
                _Graphis.DrawImage(img, x, y);
            }
        }

        private bool _AutoDisposeImageForPDF = true;
        /// <summary>
        /// 在输出PDF时自动销毁PDF里面的图片对象
        /// </summary>
        public bool AutoDisposeImageForPDF
        {
            get
            {
                return this._AutoDisposeImageForPDF;
            }
            set
            {
                //if (this._Recorder != null)
                //{
                //    this._Recorder.AutoDisposeImageForPDF = value;
                //}
                this._AutoDisposeImageForPDF = value;
            }
        }

        public virtual void DrawImage(XImageValue img, DCSystem_Drawing.RectangleF descRect, DCSystem_Drawing.RectangleF sourceRect, DCSystem_Drawing.GraphicsUnit unit)
        {
            if (_PDFGraphics != null)
            {
                _PDFGraphics.DrawImage(img.Value, descRect, this.AutoDisposeImageForPDF, img.GetImageDataRaw());
            }
            if (_Graphis != null)
            {
                    _Graphis.DrawImage(img.Value, descRect, sourceRect, unit);
            }
        }
        public void DrawImageUnscaled(DCSystem_Drawing.Image image, int x, int y)
        {
            //if (this._Recorder != null)
            //{
            //    this._Recorder.DrawImageUnscaled(image, x, y);
            //}
            //if( this._NewRecorder != null )
            //{
            //    this._NewRecorder.DrawImage(image, x, y);
            //    return;
            //}
            if (_PDFGraphics != null)
            {
                this.DrawImage(image, x, y);
            }
            if (_Graphis != null)
            {
                //if (this._MyMartrix != null)
                //{
                //    this._MyMartrix.Transform(ref x, ref y);
                //}
                this._Graphis.DrawImageUnscaled(image, x, y);
            }
        }

        public void DrawLine(DCSystem_Drawing.Color c, float x1, float y1, float x2, float y2)
        {
            if (c == DCSystem_Drawing.Color.Black)
            {
                this.DrawLine(Pens.Black, x1, y1, x2, y2);
            }
            else if (c == DCSystem_Drawing.Color.White)
            {
                this.DrawLine(Pens.White, x1, y1, x2, y2);
            }
            else
            {
                using (var p = new Pen(c))
                {
                    this.DrawLine(p, x1, y1, x2, y2);
                }
            }
        }

        public virtual void DrawLine(Pen pen, float x1, float y1, float x2, float y2)
        {
            this._Graphis?.DrawLine(pen, x1, y1, x2, y2);
            //if (float.IsNaN(x1) || float.IsNaN(y1) || float.IsNaN(x2) || float.IsNaN(y2))
            //{
            //}
            //if (this._Recorder != null)
            //{
            //    this._Recorder.DrawLine(pen, x1, y1, x2, y2);
            //}
            //if( this._NewRecorder != null )
            //{
            //    this._NewRecorder.DrawLine(pen, x1, y1, x2, y2);
            //    return;
            //}
            if (_PDFGraphics != null)
            {
                _PDFGraphics.DrawLine(pen, x1, y1, x2, y2);
            }

            //if (_Graphis != null)
            //{
            //    if (this._MyMartrix != null)
            //    {
            //        this._MyMartrix.Transform(ref x1, ref y1);
            //        this._MyMartrix.Transform(ref x2, ref y2);
            //    }
            //    this._Graphis.DrawLine(pen, x1, y1, x2, y2);
            //}
            //if (_Writer != null)
            //{
            //    WriteRecordType(DCMetaRecordType.DrawLine);
            //    WritePen(pen);
            //    _Writer.Write(x1);
            //    _Writer.Write(y1);
            //    _Writer.Write(x2);
            //    _Writer.Write(y2);
            //}
        }

        public void DrawLine(Pen pen, int x1, int y1, int x2, int y2)
        {
            DrawLine(pen, (float)x1, (float)y1, (float)x2, (float)y2);
        }
        public virtual void DrawLines(DCSystem_Drawing.Pen pen, DCSystem_Drawing.PointF[] points)
        {
            //if (this._Recorder != null)
            //{
            //    this._Recorder.DrawLines(pen, points);
            //}
            if (_PDFGraphics != null)
            {
                _PDFGraphics.DrawLines(pen, points);
            }
            if (_Graphis != null)
            {
                //this._MyMartrix?.TransformPoints(points);
                this._Graphis.DrawLines(pen, points);
            }
        }

        private static readonly int ARGB_BLACK = Color.Black.ToArgb();
        private static readonly int ARGB_White = Color.White.ToArgb();

        public virtual void DrawRectangle(Pen pen, float x, float y, float width, float height, float roundRadio = 0)
        {
            //if (Math.Abs(y + height - 3078.67969) < 3)
            //{
            //}
            //if (this._Recorder != null)
            //{
            //    this._Recorder.DrawRectangle(pen, x, y, width, height);
            //}
            if (_PDFGraphics != null)
            {
                _PDFGraphics.DrawRectangle(pen, x, y, width, height, roundRadio);
            }
            if (_Graphis != null)
            {
                //if (this._MyMartrix != null)
                //{
                //    this._MyMartrix.TransformRectangleF(ref x, ref y, ref width, ref height);
                //}
                this._Graphis.DrawRectangle(pen, x, y, width, height);
            }
        }

        public void DrawRectangle(Pen pen, DCSystem_Drawing.Rectangle rect)
        {
            this.DrawRectangle(pen, (float)rect.Left, (float)rect.Top, (float)rect.Width, (float)rect.Height);
            ////if (this._Recorder != null)
            ////{
            ////    this._Recorder.DrawRectangle(pen, rect);
            ////}
            //if (_PDFGraphics != null)
            //{
            //    _PDFGraphics.DrawRectangle(pen, rect);
            //}
            //if (this._Graphis != null)
            //{
            //    if (this._MyMartrix == null)
            //    {
            //        this._Graphis.DrawRectangle(pen, rect);
            //    }
            //    else
            //    {
            //        this._Graphis.DrawRectangle(pen, this._MyMartrix.Transform(rect));
            //    }
            //}
        }
        public virtual void DrawString(
            string s,
            XFontValue font,
            DCSystem_Drawing.Color txtColor,
            DCSystem_Drawing.RectangleF layoutRectangle,
            StringFormat format)
        {
            this._Graphis?.DrawString(
                    s,
                    font.Value,
                    Brushes.GetValue(txtColor),
                    layoutRectangle,
                    format);
            if (_PDFGraphics != null)
            {
                _PDFGraphics.DrawString(
                    s,
                    font.Name,
                    font.Size,
                    font.Style,
                    Brushes.GetValue(txtColor),
                    layoutRectangle,
                    format);
            }
            //if (_Graphis != null)
            //{
            //    if (this._MyMartrix != null)
            //    {
            //        layoutRectangle = this._MyMartrix.Transform(layoutRectangle);
            //    }
            //    this._Graphis.DrawString(
            //        s, 
            //        font.Value,
            //        GraphicsObjectBuffer.GetSolidBrush(txtColor),
            //        layoutRectangle, 
            //        format.Value());
            //}
        }
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public void DrawString(string s, DCSystem_Drawing.Font font, DCSystem_Drawing.Brush brush, DCSystem_Drawing.RectangleF layoutRectangle, DCSystem_Drawing.StringFormat format)
        {
            this._Graphis?.DrawString(s, font, brush, layoutRectangle, format);
            //if (this._Recorder != null)
            //{
            //    this._Recorder.DrawString(s, font, brush, layoutRectangle, format);
            //}
            //if( this._NewRecorder != null )
            //{
            //    this._NewRecorder.DrawString(s, font , brush, layoutRectangle, format);
            //    return;
            //}
            if (_PDFGraphics != null)
            {
                _PDFGraphics.DrawString(s, font, brush, layoutRectangle, format);
            }
            //if (_Graphis != null)
            //{
            //    if (this._MyMartrix != null)
            //    {
            //        layoutRectangle = this._MyMartrix.Transform(layoutRectangle);
            //    }
            //    this._Graphis.DrawString(s, font, brush, layoutRectangle, format);
            //}
            //if (this._LogType == DCGraphicsLogType.Content)
            //{
            //    LogContent(s);
            //}
        }
        public virtual void DrawString(
            string s,
            XFontValue font,
            DCSystem_Drawing.Color txtColor,
            float x,
            float y,
            StringFormat format)
        {
            //if (this._Recorder != null)
            //{
            //    this._Recorder.DrawString(s, font, c, x, y, format);
            //}
            if (_PDFGraphics != null)
            {
                _PDFGraphics.DrawString(
                    s,
                    font.Name,
                    font.Size,
                    font.Style,
                    Brushes.GetValue(txtColor),
                    new DCSystem_Drawing.RectangleF(x, y, 10000, 1000),
                    format);
            }
            //lock (typeof(XFontValue))
            {
                if (_Graphis != null)
                {
                    //this._MyMartrix?.Transform(ref x, ref y);
                    this._Graphis.DrawString(
                        s,
                        font.Value,
                        Brushes.GetValue(txtColor),
                        x,
                        y,
                        format);
                }
            }
        }

        public void DrawString(
            string text,
            DCSystem_Drawing.Font font,
            DCSystem_Drawing.Brush brush,
            float x,
            float y,
            StringFormat format)
        {
            //if (this._Recorder != null)
            //{
            //    this._Recorder.DrawString(text, font, brush, x, y, format);
            //}
            if (_PDFGraphics != null)
            {
                _PDFGraphics.DrawString(text, font, brush, new DCSystem_Drawing.RectangleF(x, y, 10000, 1000), format);
            }
            if (_Graphis != null)
            {
                //this._MyMartrix.Transform(ref x, ref y);
                this._Graphis.DrawString(text, font, brush, x, y, format);
            }
        }
        public void DrawString(string text, XFontValue font, DCSystem_Drawing.Color txtColor, float x, float y)
        {
            DrawString(text, font, txtColor, x, y, StringFormat.GenericDefault);
            //var brush = GraphicsObjectBuffer.GetSolidBrush(c);
            //if (this._PDFGraphics != null)
            //{
            //    this._PDFGraphics.DrawString(s, font.Name, font.Size, font.Style, brush, new RectangleF(x, y, 1000, 1000), StringFormat.GenericDefault);
            //}
            //lock (typeof(XFontValue))
            //{
            //    if (this._Recorder != null)
            //    {
            //        this._Recorder.DrawString(s, font.Value, brush, x, y);
            //    }
            //    if (_Graphis != null)
            //    {
            //        this._Graphis.DrawString(s, font.Value, brush, x, y);
            //    }
            //}
        }

        private static Graphics _GraphisForMeasure = null;

        public Graphics GraphisForMeasure
        {
            get
            {
                if (this._Graphis != null)
                {
                    return this._Graphis;
                }
                if (_PDFGraphics != null)
                {
                    if (_GraphisForMeasure == null)
                    {
                        Bitmap bmp = new Bitmap(1, 1);
                        _GraphisForMeasure = Graphics.FromImage(bmp);
                    }
                    _GraphisForMeasure.PageUnit = _PDFGraphics.PageUnit;
                    return _GraphisForMeasure;
                }
                return null;
            }
        }
        public virtual DCSystem_Drawing.SizeF MeasureString(string text, XFontValue font, int width, StringFormat format)
        {
            return this.GraphisForMeasure.MeasureString(text, font.Value, width, format);
        }
        public DCSystem_Drawing.SizeF MeasureString(string text, XFontValue font)
        {
            return this.MeasureString(text, font, 100000, (StringFormat)null);
        }

        public void FillRectangle(Brush brush, DCSystem_Drawing.Rectangle rect)
        {
            FillRectangle(brush, (float)rect.Left, (float)rect.Top, (float)rect.Width, (float)rect.Height);
        }

        public virtual void FillRectangle(Brush brush, float x, float y, float width, float height, float roundRadio = 0)
        {
            //if(this._NewRecorder != null )
            //{
            //    this._NewRecorder.FillRectangle(brush, new RectangleF( x, y, width, height));
            //    return;
            //}
            //if (this._Recorder != null)
            //{
            //    this._Recorder.FillRectangle(brush, x, y, width, height);
            //}
            if (_PDFGraphics != null)
            {
                _PDFGraphics.FillRectangle(brush, x, y, width, height, roundRadio);
            }
            if (_Graphis != null)
            {
                //try
                //{
                //this._MyMartrix?.TransformRectangleF(ref x, ref y, ref width, ref height);
                this._Graphis.FillRectangle(brush, x, y, width, height);
                //}
                //catch (Exception ext)
                //{
                //    System.Diagnostics.Debug.WriteLine(ext.Message);
                //}
            }
        }
        public void FillRectangle(Color c, RectangleF rect)
        {
            FillRectangle(c, rect.Left, rect.Top, rect.Width, rect.Height);
        }
        private static readonly int _AliceBlueARGB = Color.AliceBlue.ToArgb();
        private static readonly int _LighGrayARGB = Color.LightGray.ToArgb();
        public virtual void FillRectangle(Color c, float x, float y, float width, float height)
        {
            if (this._Graphis != null)
            {
                if (c.ToArgb() == _AliceBlueARGB)
                {
                    this._Graphis.FillRectangle(Brushes.AliceBlue, x, y, width, height);
                }
                else if (c.ToArgb() == _LighGrayARGB)
                {
                    this._Graphis.FillRectangle(DCSystem_Drawing.Brushes.LightGray, x, y, width, height);
                }
                else
                {
                    this._Graphis.FillRectangle(new DCSystem_Drawing.SolidBrush(c), x, y, width, height);
                }
            }
            else if (this._PDFGraphics != null)
            {
                this._PDFGraphics.FillRectangle(new DCSystem_Drawing.SolidBrush(c), x, y, width, height, 0);
            }
        }

        private int _ClipVersion = 0;
        public int ClipVersion
        {
            get
            {
                return this._ClipVersion;
            }
        } 
        public void SetClip(DCSystem_Drawing.RectangleF rect)
        {
            this._ClipVersion++;
            this._Graphis?.SetClip(rect);
            if (_PDFGraphics != null)
            {
                _PDFGraphics.SetClipRectangle(rect);
            }
        }


        public virtual object Save()
        {
            if (this._PDFGraphics != null)
            {
                return this._PDFGraphics.Save();
            }
            else
            if (_Graphis != null)
            {
                return _Graphis.Save();
            }
            else
            {
                return null;
            }
        }

        public virtual void Restore(object obj)
        {
            if (obj != null)
            {
                this._ClipVersion++;
                if (this._PDFGraphics != null)
                {
                    this._PDFGraphics.Restore(obj);
                }
                else
                if (_Graphis != null && obj is GraphicsState)
                {
                    _Graphis.Restore((GraphicsState)obj);
                }
            }
        }


        private bool _AutoDisposeNativeGraphics = false;
        /// <summary>
        /// 是否自动销毁掉底层的画布对象
        /// </summary>

        public bool AutoDisposeNativeGraphics
        {
            get
            {
                return _AutoDisposeNativeGraphics;
            }
            set
            {
                //if (this._Recorder != null)
                //{
                //    this._Recorder.AutoDisposeNativeGraphics = value;
                //}
                _AutoDisposeNativeGraphics = value;
            }
        }
        public virtual void Dispose()
        {
            if (this.AutoDisposeNativeGraphics)
            {
                if (_Graphis != null)
                {
                    Graphics g = this._Graphis;
                    this._Graphis = null;
                    g.Dispose();
                }
            }
        }
    }
}