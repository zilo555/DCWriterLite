
using DCSoft.Common;
using DCSoft.Drawing;
//using DCSoft.HtmlDom;
using DCSoft.Printing;
using DCSoft.Writer.Controls;
using DCSoft.Writer.Dom;
using System.Collections.Generic;
using System.ComponentModel;
//using System.Drawing.Drawing2D;
using System.Text;

namespace DCSystem_Drawing
{

    public class MyBinaryBuilder
    {

        public MyBinaryBuilder()
        {

            this._Buffer = new byte[1024];
            this._BufferLength = this._Buffer.Length;

        }
        private int _BufferLength;
        internal byte[] _Buffer;
        public void Close()
        {
            this._BufferLength = 0;
            this._Buffer = null;
        }
        internal int _Length;
        public int Length
        {
            get
            {
                return this._Length;
            }
        }
        private void EnsureCapacity(int newSize)
        {
            if (this._BufferLength < newSize)
            {
                var temp = new byte[(int)(newSize * 1.5)];
                System.Buffer.BlockCopy(this._Buffer, 0, temp, 0, this._Length);
                this._Buffer = temp;
                this._BufferLength = temp.Length;
            }
        }
        public void AppendRecordType(GraphicsRecordType type)
        {
            this.AppendByte((byte)type);
        }
        public void AppendByteArray(byte[] bs)
        {
            if (bs == null || bs.Length == 0)
            {
                EnsureCapacity(this._Length + 2);
                this._Buffer[this._Length++] = 0;
                this._Buffer[this._Length++] = 0;
            }
            else
            {
                EnsureCapacity(this._Length + bs.Length + 4);
                this.AppendInt32(bs.Length);
                System.Buffer.BlockCopy(bs, 0, this._Buffer, this._Length, bs.Length);
                this._Length += bs.Length;
            }
        }
        public void AppendBoolean(bool v)
        {
            this.AppendByte(v ? (byte)1 : (byte)0);
        }
        public void AppendByte(byte b)
        {
            if (this._Length == this._BufferLength)
            {
                EnsureCapacity(this._Length + 1);
            }
            this._Buffer[this._Length++] = b;
        }

        //public void AppendLine()
        //{
        //    EnsureCapacity(this._Length + 1);
        //    this._Buffer[this._Length++] = '\n';
        //}
        private System.Text.Encoding _UTF8 = System.Text.Encoding.UTF8;

        [ThreadStatic]
        private static byte[] _TextBuffer = new byte[1024];
        public void AppendString(string txt)
        {
            if (string.IsNullOrEmpty(txt))
            {
                // 空字符串
                EnsureCapacity(this._Length + 4);
                this._Buffer[this._Length] = 0;
                this._Buffer[this._Length + 1] = 0;
                this._Buffer[this._Length + 2] = 0;
                this._Buffer[this._Length + 3] = 0;
                this._Length += 4;
                return;
            }
            if (_TextBuffer == null || _TextBuffer.Length < txt.Length * 3)
            {
                _TextBuffer = new byte[txt.Length * 3];
            }
            var len = _UTF8.GetBytes(txt, 0, txt.Length, _TextBuffer, 0);
            EnsureCapacity(this._Length + len + 4);
            this.AppendInt32(len);
            System.Buffer.BlockCopy(_TextBuffer, 0, this._Buffer, this._Length, len);
            this._Length += len;
        }

        public void AppendBinaryBuilder(MyBinaryBuilder str)
        {
            EnsureCapacity(this._Length + str._Length);
            Array.Copy(str._Buffer, 0, this._Buffer, this._Length, str._Length);
            this._Length += str._Length;
        }

        public void AppendInt32(int v)
        {
            var len = this._Length;
            if (this._BufferLength < len + 4)
            {
                EnsureCapacity(len + 4);
            }
            var buffer = this._Buffer;
            buffer[len] = (byte)(v >> 24);
            buffer[len + 1] = (byte)(v >> 16);
            buffer[len + 2] = (byte)(v >> 8);
            buffer[len + 3] = (byte)(v);
            this._Length = len + 4;
        }

        public void AppendSingle(float v)
        {
            var intv = System.BitConverter.SingleToInt32Bits(v);
            AppendInt32(intv);
        }

        public void AppendInt16(short v)
        {
            var len = this._Length;
            EnsureCapacity(len + 2);
            this._Buffer[len] = (byte)((v >> 8) & 0xff);
            this._Buffer[len + 1] = (byte)(v & 0xff);
            this._Length = len + 2;
        }
        public byte[] ToByteArray()
        {
            var bs = new byte[this._Length];
            System.Buffer.BlockCopy(this._Buffer, 0, bs, 0, this._Length);
            return bs;
        }
#if ! RELEASE
        public override string ToString()
        {
            return "Bytes " + this._Length;
        }
#endif
    }

    public sealed class Graphics : IDisposable
    {
        /// <summary>
        /// 是否启用标准图片列表
        /// </summary>
        private bool _IsStandardImageListReady = false;

        public delegate bool IsStandardImageListReadyHandler();

        public static IsStandardImageListReadyHandler EventIsStandardImageListReady = null;

        public Graphics()
        {
            this._String = new MyBinaryBuilder();
            if (EventIsStandardImageListReady != null)
            {
                this._IsStandardImageListReady = EventIsStandardImageListReady();
            }
        }

        private float _AbsoluteOffsetX;
        private float _AbsoluteOffsetY;
        /// <summary>
        /// 设置绝对化的坐标偏移量
        /// </summary>
        /// <param name="x">X偏移量</param>
        /// <param name="y">Y偏移量</param>
        public void SetAbsoluteOffset(float x, float y)
        {
            this._AbsoluteOffsetX = x;
            this._AbsoluteOffsetY = y;
        }
        private float _ZoomRate = 1f;
        /// <summary>
        /// 缩放比率
        /// </summary>
        public float ZoomRate
        {
            get
            {
                return this._ZoomRate;
            }
            set
            {
                this._ZoomRate = value;
                this._FontTable.ZoomRate = value;
            }
        }
        internal RectangleF _ClipBounds = RectangleF.Empty;
        public RectangleF ClipBounds
        {
            get
            {
                CheckDispose();
                return this._ClipBounds;
            }
        }

        private GraphicsUnit _PageUnit = GraphicsUnit.Pixel;
        public GraphicsUnit PageUnit
        {
            get
            {
                return this._PageUnit;
            }
            set
            {
                if (this._PageUnit != value)
                {
                    CheckDispose();
                    this._PageUnit = value;
                    this._String.AppendRecordType(GraphicsRecordType.SetPageUnit);
                    this._String.AppendByte((byte)value);
                    /*  World : 0, Display : 1, Pixel : 2, Point : 3, Inch : 4, Document : 5, Millimeter : 6, */
                    switch (value)
                    {
                        case GraphicsUnit.World: this._PageUnitRate = 1; break;
                        case GraphicsUnit.Display: this._PageUnitRate = 1; break;
                        case GraphicsUnit.Pixel: this._PageUnitRate = 1; break;
                        case GraphicsUnit.Point: this._PageUnitRate = 1.3333333333f; break;
                        case GraphicsUnit.Inch: this._PageUnitRate = 96; break;
                        case GraphicsUnit.Document: this._PageUnitRate = 0.32f; break;
                        case GraphicsUnit.Millimeter: this._PageUnitRate = 3.77952744641642f; break;
                        default: throw new Exception("不支持的单位:" + value);
                    }
                }
            }
        }
        private float _PageUnitRate = 1;
        private Matrix _Transform = new Matrix();
        public Matrix Transform
        {
            get
            {
                CheckDispose();
                return this._Transform;
            }
            set
            {
                CheckDispose();
                if (value == null)
                {
                    this._Transform.Reset();
                }
                else
                {
                    this._Transform = value;
                }
                this.WriteTransformData();
            }
        }

        private void WriteTransformData()
        {
            this._String.AppendRecordType(GraphicsRecordType.SetTransform);
            var es = this._Transform.Elements;
            this._String.AppendSingle(es[0]);
            this._String.AppendSingle(es[1]);
            this._String.AppendSingle(es[2]);
            this._String.AppendSingle(es[3]);
            var dx = es[4] * this._PageUnitRate * this._ZoomRate;
            var dy = es[5] * this._PageUnitRate * this._ZoomRate;
            this._String.AppendSingle(dx);
            this._String.AppendSingle(dy);
        }
        public void ApplyTransformData()
        {
            this._String.AppendRecordType(GraphicsRecordType.SetTransform);
            var es = this._Transform.Elements;
            this._String.AppendSingle(es[0]);
            this._String.AppendSingle(es[1]);
            this._String.AppendSingle(es[2]);
            this._String.AppendSingle(es[3]);
            this._String.AppendSingle(this._AbsoluteOffsetX * (1 - es[0]) * this._ZoomRate + es[4] * this._PageUnitRate * this._ZoomRate);
            this._String.AppendSingle(this._AbsoluteOffsetY * (1 - es[3]) * this._ZoomRate + es[5] * this._PageUnitRate * this._ZoomRate);
        }

        public void ResetClip()
        {
            this._ClipBounds = RectangleF.Empty;
            this._String.AppendRecordType(GraphicsRecordType.ResetClip);
            //Console.WriteLine("ResetClip");
        }

        public void SetClip(RectangleF rect)
        {
            //return;
            //Console.WriteLine("clip:" + rect.Left + "," + rect.Top + "," + rect.Width + "," + rect.Height);
            //CheckDispose();
            this._ClipBounds = rect;
            //var str = rect.ToString();
            this._String.AppendRecordType(GraphicsRecordType.SetClip);
            this.AppendRectangle(rect.Left, rect.Top, rect.Width, rect.Height);
        }

        public void ResetTransform()
        {
            this.Transform = null;
        }

        public void TranslateTransform(float x, float y)
        {
            this._Transform.Translate(x, y);
            this.WriteTransformData();
        }

        public void ScaleTransform(float sx, float sy)
        {
            this._Transform.Scale(sx, sy);
            if (sx != 1 || sy != 1)
            {
                this.ApplyTransformData();
            }
            else
            {
                this.WriteTransformData();
            }
        }

        #region DrawRectangle

        public void DrawRectangle(Pen pen, Rectangle rect)
        {
            DrawRectangle(pen, rect.X, rect.Y, rect.Width, rect.Height);
        }
        public void DrawRectangle(Pen pen, float x, float y, float width, float height, float radius = 0)
        {
            if (pen == null)
            {
                throw new ArgumentNullException("pen");
            }
            if (this._ClipBounds.IsEmpty == false)
            {
                if (x + width < this._ClipBounds.Left
                    || x > this._ClipBounds.Right
                    || y + height < this._ClipBounds.Top
                    || y > this._ClipBounds.Bottom)
                {
                    return;
                }
            }
            var strIndex = this._PenTable.GetIndex(pen);
            if (radius > 0)
            {
                this._String.AppendRecordType(GraphicsRecordType.DrawRoundRectangle);
                this._String.AppendInt16(strIndex);// this._String.Append(strIndex);
                this._String.AppendInt16((short)(radius * this._PageUnitRate));
                this.AppendRectangle(x, y, width, height);
            }
            else
            {
                this._String.AppendRecordType(GraphicsRecordType.DrawRectangle);
                this._String.AppendInt16(strIndex);// this._String.Append(strIndex);
                this.AppendRectangle(x, y, width, height);
            }
        }

        public void DrawRectangle(Pen pen, int x, int y, int width, int height)
        {
            DrawRectangle(pen, (float)x, (float)y, (float)width, (float)height);
        }
        #endregion

        public void Clear(Color color)
        {
        }

        #region FillRectangle

        public void FillRectangle(Brush brush, RectangleF rect)
        {
            FillRectangle(brush, (float)rect.X, (float)rect.Y, (float)rect.Width, (float)rect.Height);
        }

        private class FillRectangleCache
        {
            private uint FillColorValue;
            private float Left;
            private float Top;
            private float Right;
            private float Height;
            private int CharIndex;
            private int MergeCount = 0;
            public FillRectangleCache(SolidBrush brush, float x, float y, float w, float h, int cIndex)
            {
                this.FillColorValue = brush.ColorValue;
                this.Left = x;
                this.Top = y;
                this.Right = x + w;
                this.Height = h;
                this.CharIndex = cIndex;
            }
            public bool Merge(SolidBrush brush, float x, float y, float w, float h)
            {
                if (this.Top == y
                    && this.Height == h
                    && brush.ColorValue == this.FillColorValue)
                {
                    var ids = x - this.Right;
                    if (-0.5f < ids && ids < 0.5f)
                    {
                        // 进行合并
                        this.Right = x + w;
                        this.MergeCount++;
                        return true;
                    }
                }
                return false;
            }
            public void Commit(Graphics g)
            {
                if (this.MergeCount > 0)
                {
                    // 修改已经输出的结果
                    int back = g._String._Length;
                    //g._String.ClearRange(' ', this.CharIndex, 10);
                    g._String._Length = this.CharIndex;
                    g._String.AppendInt32((int)((this.Right - this.Left) * g._ZoomRate * g._PageUnitRate));
                    g._String._Length = back;
                }
            }
        }
        private bool _AllowUseFillRectangleCache = false;
        private FillRectangleCache _CurrentFillRectangleCache = null;
        public void BeginCacheFillRectangle()
        {
            this._AllowUseFillRectangleCache = true;
        }
        public void EndCacheFillRectangle()
        {
            this._AllowUseFillRectangleCache = false;
            if (this._CurrentFillRectangleCache != null)
            {
                this._CurrentFillRectangleCache.Commit(this);
                this._CurrentFillRectangleCache = null;
            }
        }

        public void FillRectangle(Brush brush, float x, float y, float width, float height, float radius = 0)
        {
            if (brush == null)
            {
                throw new ArgumentNullException("brush");
            }
            if (radius > 0)
            {
                // 填充圆角矩形
                if (this._AllowUseFillRectangleCache && this._CurrentFillRectangleCache != null)
                {
                    this._CurrentFillRectangleCache.Commit(this);
                    this._CurrentFillRectangleCache = null;
                }
                var vIndex = this._BrushTable.GetIndex(brush);
                this._String.AppendRecordType(GraphicsRecordType.FillRoundRectangle);
                this._String.AppendInt16(vIndex);// this._String.Append(strIndex);
                this._String.AppendInt16((short)(radius * this._PageUnitRate));
                this.AppendRectangle(x, y, width, height);
            }
            else
            {
                if (this._AllowUseFillRectangleCache
                    && brush is SolidBrush
                    && this._CurrentFillRectangleCache != null)
                {
                    if (this._CurrentFillRectangleCache.Merge((SolidBrush)brush, x, y, width, height))
                    {
                        // 本次操作被合并了，无需后续处理
                        return;
                    }
                    else
                    {
                        // 合并操作失败，则进行提交，并清空状态
                        this._CurrentFillRectangleCache.Commit(this);
                        this._CurrentFillRectangleCache = null;
                    }
                }
                var vIndex = this._BrushTable.GetIndex(brush);
                this._String.AppendRecordType(GraphicsRecordType.FillRectangle);
                this._String.AppendInt16(vIndex);// this._String.Append(strIndex);
                if (this._AllowUseFillRectangleCache
                    && brush is SolidBrush
                    && this._CurrentFillRectangleCache == null)
                {
                    // 创建缓存对象
                    this._CurrentFillRectangleCache = new FillRectangleCache(
                        (SolidBrush)brush,
                        x,
                        y,
                        width,
                        height,
                        this.AppendRectangleForCache(x, y, width, height));
                }
                else
                {
                    this.AppendRectangle(x, y, width, height);
                }
            }
        }

        public void FillRectangle(Brush brush, Rectangle rect)
        {
            FillRectangle(brush, (float)rect.X, (float)rect.Y, (float)rect.Width, (float)rect.Height);
        }

        #endregion

        #region DrawString
        public void DrawString(string s, Font font, Brush brush, float x, float y, StringFormat format)
        {
            DrawString(s, font, brush, new RectangleF(x, y, 0f, 0f), format);
        }

        public void DrawString(string txt, DCSystem_Drawing.Font font, DCSystem_Drawing.Brush brush, DCSystem_Drawing.RectangleF layoutRectangle, DCSystem_Drawing.StringFormat format)
        {
            if (string.IsNullOrEmpty(txt))
            {
                return;
            }
            //if (txt.Length > 10 && ((SolidBrush)brush).ColorARGB == Color.Red.ToArgb())
            //{
            //    return;
            //}

            SetCurrentBrush(brush);
            SetCurrentFont(font);
            if (layoutRectangle.Width == 0
                && layoutRectangle.Height == 0
                && txt.Length == 1)
            {
                // 快速绘制单个字符
                _String.AppendRecordType(GraphicsRecordType.DrawChar);
                this._String.AppendInt32((int)txt[0]);
                var fh = font.GetHeight(this.PageUnit);
                const float rate2 = 0.26f;// _CharTopFixHeightRate; // 0.3f
                AppendPoint(layoutRectangle.Left, layoutRectangle.Top + fh * rate2);
                if (font.Strikeout)
                {
                    // 删除线
                    var fw = this.MeasureString(txt, font).Width;
                    var linetop = layoutRectangle.Top + (fh / 2) + fh * rate2;// 0.3f;
                    this.DrawLine(
                        new Pen(brush, 1),
                        layoutRectangle.Left,
                        linetop,
                        layoutRectangle.Left + fw,
                        linetop);
                    //var fw = DCSoft.Writer.Dom.CharacterMeasurer.MeasureStringUseTrueTypeFont
                }
            }
            else
            {
                if (txt.Length == 1)
                {
                    // 只显示一个字符
                    if (format == null
                        || (format.Alignment == StringAlignment.Near
                        && format.LineAlignment == StringAlignment.Near))
                    {
                        _String.AppendRecordType(GraphicsRecordType.DrawChar);
                        this._String.AppendInt32((int)txt[0]);
                        var fh = font.GetHeight(this);
                        AppendPoint(
                            layoutRectangle.Left,
                            layoutRectangle.Top + fh * 1f);
                        if (font.Strikeout)
                        {
                            // 删除线
                            var fw = this.MeasureString(txt, font).Width;
                            var linetop = layoutRectangle.Top + fh / 2 + fh * 0.3f;
                            this.DrawLine(
                                new Pen(brush, 1),
                                layoutRectangle.Left,
                                linetop,
                                layoutRectangle.Left + fw,
                                linetop);
                        }
                        return;
                    }
                }
                SizeF layoutSize;
                string[] strLines;
                strLines = CharacterMeasurer.StaticSplitToLines(
                    this.PageUnit,
                    txt,
                    font.Name,
                    font.Size,
                    font.Style,
                    layoutRectangle.Width,
                    format,
                    out layoutSize,
                    null);
                if (strLines == null || strLines.Length == 0)
                {
                    return;
                }
                var strLinesLength = strLines.Length;
                var fontHeight = font.GetHeight(this);
                var oldLayoutTop = layoutRectangle.Top;
                if (layoutRectangle.Height > 0)
                {
                    layoutRectangle.Offset(0, fontHeight * _CharTopFixHeightRate);
                }
                else
                {
                    layoutRectangle.Offset(0, fontHeight * 0.26f);
                }

                var layoutTop = layoutRectangle.Top;
                if (layoutRectangle.Height > 0)
                {
                    if (format.LineAlignment == StringAlignment.Center)
                    {
                        layoutTop = layoutRectangle.Top + (layoutRectangle.Height - fontHeight * strLinesLength) / 2;
                    }
                    else if (format.LineAlignment == StringAlignment.Far)
                    {
                        layoutTop = layoutRectangle.Bottom - fontHeight * strLinesLength;
                    }
                }
                bool needClip = false;
                var layoutRectangleWidth = layoutRectangle.Width;
                float maxLineWidth = 0;
                if (layoutRectangleWidth > 0)
                {
                    for (var lineIndex = 0; lineIndex < strLinesLength; lineIndex++)
                    {
                        var strLine = strLines[lineIndex];
                        var lineWidth = DCSoft.Writer.Dom.CharacterMeasurer.MeasureSingleLineStringUseTrueTypeFont(this._PageUnit, strLine, font).Width;
                        if (lineWidth > maxLineWidth)
                        {
                            maxLineWidth = lineWidth;
                        }
                    }
                    if (layoutRectangle.Height > 0)
                    {
                        if ((maxLineWidth > layoutRectangleWidth)
                            || (fontHeight * strLinesLength > layoutRectangle.Height))
                        {
                            needClip = true;
                        }
                    }
                }
                if (needClip)
                {
                    var rect2 = new RectangleF(
                        layoutRectangle.Left,
                        oldLayoutTop,
                        layoutRectangle.Width,
                        layoutRectangle.Height);
                    //this.DrawRectangle(Pens.Red, rect2);
                    this.SetClip(rect2);
                }
                for (var lineIndex = 0; lineIndex < strLinesLength; lineIndex++)
                {
                    var strLine = strLines[lineIndex];
                    var layoutLeft = layoutRectangle.Left;
                    float lineWidth = 0;
                    if (strLinesLength == 1 && maxLineWidth > 0)
                    {
                        lineWidth = maxLineWidth;
                    }
                    else
                    {
                        lineWidth = this.MeasureString(strLine, font).Width;
                    }
                    if (layoutRectangleWidth > 0)
                    {
                        if (format.Alignment == StringAlignment.Center)
                        {
                            layoutLeft = /*6.25f +*/ layoutRectangle.Left + (layoutRectangleWidth - lineWidth) / 2;
                        }
                        else if (format.Alignment == StringAlignment.Far)
                        {
                            layoutLeft = layoutRectangle.Right - lineWidth;
                        }
                    }
                    //if( strLine == null || strLine.Length == 0 )
                    //{
                    //    var ss = 1;
                    //}
                    this._String.AppendRecordType(GraphicsRecordType.DrawString);
                    this._String.AppendString(strLine);
                    AppendPoint(layoutLeft, layoutTop);
                    if (layoutRectangleWidth > 0)
                    {
                        this._String.AppendInt16((short)(layoutRectangleWidth * this._PageUnitRate * this._ZoomRate));
                    }
                    else
                    {
                        this._String.AppendInt16((short)10000);
                    }
                    if (font.Strikeout)
                    {
                        // 删除线
                        var lineTop = layoutTop - fontHeight * 0.24f;
                        this.DrawLine(
                            new Pen(brush, 1),
                            layoutLeft,
                            lineTop,
                            layoutLeft + lineWidth,
                            lineTop);
                    }
                    layoutTop += fontHeight;
                }//for
                if (needClip)
                {
                    this.ResetClip();
                }
            }
        }

        private Brush _CurrentBrush = null;
        private void SetCurrentBrush(Brush b)
        {
            if (b == null)
            {
                this._CurrentBrush = null;
                var index = this._BrushTable.GetIndex(Color.Black);
                _String.AppendRecordType(GraphicsRecordType.SetCurrentBrush);
                this._String.AppendInt16(index);// this._String.Append(this._BrushTable.GetIndex(Color.Black));
            }
            else if (this._CurrentBrush == null || this._CurrentBrush.EqualsValue(b) == false)
            {
                var strIndex = this._BrushTable.GetIndex(b);
                this._CurrentBrush = b;
                _String.AppendRecordType(GraphicsRecordType.SetCurrentBrush);
                this._String.AppendInt16(strIndex);// this._String.Append(strIndex);
            }
        }
        /// <summary>
        /// 快速复制一个字符
        /// </summary>
        /// <param name="c"></param>
        /// <param name="f"></param>
        /// <param name="tc"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void FastDrawChar(char c, Font f, Color tc, float x, float y, float fontHeight)
        {
            if (this._CurrentBrush == null || this._CurrentBrush.EqualsValue(tc) == false)
            {
                var intIndex = this._BrushTable.GetIndex(tc);
                this._CurrentBrush = new SolidBrush(tc);
                _String.AppendRecordType(GraphicsRecordType.SetCurrentBrush);
                this._String.AppendInt16(intIndex);// this._String.Append(strIndex);
            }
            SetCurrentFont(f);
            _String.AppendRecordType(GraphicsRecordType.DrawChar);
            this._String.AppendInt32((int)c);
            if (fontHeight > 0)
            {
                AppendPoint(x, y + fontHeight * _CharTopFixHeightRate);
            }
            else
            {
                AppendPoint(x, y + f.GetHeight(this._PageUnit) * _CharTopFixHeightRate);
            }
        }

        private const float _CharTopFixHeightRate = 0.74f;

        #endregion


        #region MeasureString()


        public DCSystem_Drawing.SizeF MeasureString(string text, Font font)
        {
            var result = DCSoft.Writer.Dom.CharacterMeasurer.MeasureStringUseTrueTypeFont(
                this._PageUnit,
                text,
                font,
                100000,
                null);
            //if (this._ZoomRate != 1)
            //{
            //    result.Width = result.Width * this._ZoomRate;
            //    result.Height = result.Height * this._ZoomRate;
            //}
            return result;
        }
        public DCSystem_Drawing.SizeF MeasureString(string text, DCSystem_Drawing.Font font, int width, DCSystem_Drawing.StringFormat format)
        {
            if (string.IsNullOrEmpty(text))
            {
                return SizeF.Empty;
            }
            if (text.Length == 1)
            {
                return DCSoft.Writer.Dom.CharacterMeasurer.MeasureCharUseTrueTypeFont(this._PageUnit, text[0], font);
            }
            SizeF layoutSize;
            var strLines = CharacterMeasurer.StaticSplitToLines(
                this.PageUnit,
                text,
                font.Name,
                font.Size,
                font.Style,
                width,
                format,
                out layoutSize,
                null);
            //var strLines = DCSoft.Drawing.DrawerUtil.StaticSplitToLines(this, text, font, width, format, out layoutSize);
            if (layoutSize.IsEmpty)
            {
                layoutSize = DCSoft.Writer.Dom.CharacterMeasurer.MeasureSingleLineStringUseTrueTypeFont(
                    this._PageUnit,
                    text,
                    font);
            }
            return layoutSize;

            //var result = DCSoft.Writer.Dom.CharacterMeasurer.MeasureStringUseTrueTypeFont(
            //    this._PageUnit,
            //    text,
            //    font,
            //    width,
            //    format);
            ////if (this._ZoomRate != 1)
            ////{
            ////    result.Width = result.Width * this._ZoomRate;
            ////    result.Height = result.Height * this._ZoomRate;
            ////}
            //return result;
        }

        #endregion


        #region DrawImage()

        public void SetImageSmoothing(bool v)
        {
            this._String.AppendRecordType(GraphicsRecordType.SetImageSmoothing);
            this._String.AppendBoolean(v);
        }

        public void DrawImage(Image image, float x, float y)
        {
            if (image == null)
            {
                throw new ArgumentNullException("image");
            }
            var bs = image.ToBinary();
            if (bs == null || bs.Length == 0)
            {
                return;
            }
            if (this._IsStandardImageListReady && image.StandardImageIndex >= 0)
            {
                // 使用标准图标列表
                if (this._ZoomRate == 1 || image.Width == 0 || image.Height == 0)
                {
                    this._String.AppendRecordType(GraphicsRecordType.DrawImageXYStdImageIndex);
                    this._String.AppendInt16((short)image.StandardImageIndex);// this._String.Append(this._ImageTable.GetIndex(bs));
                    AppendPoint(x, y);
                }
                else
                {
                    this._String.AppendRecordType(GraphicsRecordType.DrawImageStdImageIndex);
                    this._String.AppendInt16((short)image.StandardImageIndex);// this._String.Append(this._ImageTable.GetIndexString(bs));
                                                                              //this.AppendRectangle(x, y, image.Width / this._PageUnitRate, image.Height / this._PageUnitRate);
                    AppendPoint(x, y);
                    this._String.AppendInt32((int)(this._ZoomRate * image.Width)); //AppendFloat(this._ZoomRate * width * this._PageUnitRate);
                    this._String.AppendInt32((int)(this._ZoomRate * image.Height)); // AppendFloat(this._ZoomRate * height * this._PageUnitRate);
                }
            }
            else
            {
                // 直接绘制图片数据
                var index = this._ImageTable.GetIndex(bs);
                if (this._ZoomRate == 1 || image.Width == 0 || image.Height == 0)
                {
                    this._String.AppendRecordType(GraphicsRecordType.DrawImageXY);
                    this._String.AppendInt16(index);// this._String.Append(this._ImageTable.GetIndex(bs));
                    AppendPoint(x, y);
                }
                else
                {
                    this._String.AppendRecordType(GraphicsRecordType.DrawImage);
                    this._String.AppendInt16(index);// this._String.Append(this._ImageTable.GetIndexString(bs));
                                                    //this.AppendRectangle(x, y, image.Width / this._PageUnitRate, image.Height / this._PageUnitRate);
                    AppendPoint(x, y);
                    this._String.AppendInt32((int)(this._ZoomRate * image.Width)); //AppendFloat(this._ZoomRate * width * this._PageUnitRate);
                    this._String.AppendInt32((int)(this._ZoomRate * image.Height)); // AppendFloat(this._ZoomRate * height * this._PageUnitRate);
                }
            }
        }

        public void DrawImageUnscaled(Image image, int x, int y)
        {
            DrawImage(image, (float)x, (float)y);
        }

        public void DrawImageUnscaled(Image image, Rectangle rect)
        {
            DrawImage(image, (float)rect.X, (float)rect.Y);
        }

        public void DrawImage(Image image, RectangleF destRect, RectangleF srcRect, GraphicsUnit srcUnit)
        {
            if (image == null)
            {
                throw new ArgumentNullException("image");
            }
            if (destRect.Width == 0)
            {

            }
            //if(image.ToBinary() == null )
            //{
            //    return;
            //}
            var bs = image.ToBinary();
            if (bs == null || bs.Length == 0)
            {
                return;
            }
            var index = this._ImageTable.GetIndex(bs);
            _String.AppendRecordType(GraphicsRecordType.DrawImageExt);
            this._String.AppendInt16(index);// this._String.Append(this._ImageTable.GetIndexString(bs));
            this._String.AppendInt16((short)srcRect.Left);
            this._String.AppendInt16((short)srcRect.Top);
            this._String.AppendInt16((short)srcRect.Width);
            this._String.AppendInt16((short)srcRect.Height);
            AppendRectangle(destRect);
        }

        #endregion

        public void DrawLine(Pen pen, float x1, float y1, float x2, float y2)
        {
            if (pen == null)
            {
                throw new ArgumentNullException("pen");
            }

            if (this._Transform.HasScale() == false && DrawerUtil.IsLineInClipRectangle(this._ClipBounds, x1, y1, x2, y2) == false)
            {
                //return;
            }
            //if(pen.Color.R == 255 )
            //{
            //    Console.WriteLine(pen.Color.ToString() + " " + pen.Width);
            //}
            var intIndex = this._PenTable.GetIndex(pen);
            _String.AppendRecordType(GraphicsRecordType.DrawLine);
            this._String.AppendInt16(intIndex);// this._String.Append(strIndex);
            this.AppendPoint(x1, y1);
            this.AppendPoint(x2, y2);
        }

        public void DrawLines(Pen pen, PointF[] points)
        {
            if (pen == null)
            {
                throw new ArgumentNullException("pen");
            }
            if (points == null)
            {
                throw new ArgumentNullException("points");
            }
            if (points.Length > 0)
            {
                var intIndex = this._PenTable.GetIndex(pen);
                this._String.AppendRecordType(GraphicsRecordType.DrawLines);
                this._String.AppendInt16(intIndex);
                this.AppendPoints(points);
            }
        }


        private Image _ParentImage = null;
        public static Graphics FromImage(Image image)
        {
            if (image == null)
            {
                throw new ArgumentNullException("image");
            }
            image.CheckDispose();
            var g = new Graphics();
            g._ParentImage = image;
            g.FillRectangle(Brushes.White, 0, 0, image.Width, image.Height);
            return g;
        }

        public void Restore(GraphicsState gstate)
        {
            if (gstate == null)
            {
                throw new ArgumentNullException("gstate");
            }
            CheckDispose();
            gstate.Restore(this);
            //this._String.AppendRecordType(GraphicsRecordType.Restore);
        }

        public GraphicsState Save()
        {
            //this._String.AppendRecordType(GraphicsRecordType.Save);
            return new Drawing2D.GraphicsState(this);
        }

        private bool _Disposed;
        public void Dispose()
        {
            if (this._ParentImage is Bitmap)
            {
                var writer = new MyBinaryBuilder();
                writer.AppendByte(Bitmap.GraphicsImageHeader);
                writer.AppendInt16((short)this._ParentImage.Width);
                writer.AppendInt16((short)this._ParentImage.Height);
                this.WriteTo(writer);
                ((Bitmap)this._ParentImage).DataFromGraphics = writer.ToByteArray();
                writer.Close();
            }
            this._Disposed = true;
            this._ParentImage = null;
            if (this._ColorTable != null)
            {
                this._ColorTable.Clear();
                this._ColorTable = null;
            }
            if (this._FontTable != null)
            {
                this._FontTable.Clear();
                this._FontTable = null;
            }
            if (this._PenTable != null)
            {
                this._PenTable.Clear();
                this._PenTable = null;
            }
            if (this._ImageTable != null)
            {
                this._ImageTable.Clear();
                this._ImageTable = null;
            }
            if (this._String != null)
            {
                this._String.Close();
                this._String = null;
            }
        }
        protected void CheckDispose()
        {
            if (this._Disposed)
            {
                throw new ObjectDisposedException(this.GetType().Name);
            }
        }

        public byte[] ToByteArray()
        {
            var str = new MyBinaryBuilder();
            this.WriteTo(str);
            var bs = str.ToByteArray();
            str.Close();
            return bs;
        }
        public void WriteTo(MyBinaryBuilder strResult)
        {
            if (strResult == null)
            {
                throw new ArgumentNullException(nameof(strResult));
            }
            strResult.AppendRecordType(GraphicsRecordType.CheckVersion);
            strResult.AppendInt32(FormatVersion);
            if (this._ColorTable != null)
            {
                this._ColorTable.WriteTo(strResult, this);
            }
            if (this._FontTable != null)
            {
                this._FontTable.WriteTo(strResult, this);
            }
            if (this._PenTable != null)
            {
                this._PenTable.ZoomRate = GraphicsUnitConvert.Convert(this.ZoomRate, this.PageUnit, GraphicsUnit.Pixel);
                this._PenTable.WriteTo(strResult, this);
            }
            if (this._BrushTable != null)
            {
                this._BrushTable.WriteTo(strResult, this);
            }
            if (this._ImageTable != null)
            {
                this._ImageTable.WriteTo(strResult, this);
            }
            strResult.AppendRecordType(GraphicsRecordType.UpdateClearRectangle);
            //strResult.AppendLine();
            strResult.AppendBinaryBuilder(this._String);
            strResult.AppendRecordType(GraphicsRecordType.First);
            int len = strResult.Length;
        }


        /// <summary>
        /// 版本号
        /// </summary>
        public const int FormatVersion = 20221031;
        private void AppendPoints(PointF[] ps)
        {
            this._String.AppendInt16((short)ps.Length);
            var len = ps.Length;
            for (int iCount = 0; iCount < len; iCount++)
            {
                this.AppendPoint(ps[iCount].X, ps[iCount].Y);
            }
        }
        private void AppendRectangle(RectangleF rect)
        {
            this.AppendRectangle(rect.Left, rect.Top, rect.Width, rect.Height);
        }

        private void AppendRectangle(float x, float y, float width, float height)
        {
            //var v1 = (int)(this._ZoomRate * (this._AbsoluteOffsetX + x * this._PageUnitRate));
            //var v2 = (int)(this._ZoomRate * (this._AbsoluteOffsetY + y * this._PageUnitRate));
            //var v3 = (int)(this._ZoomRate * width * this._PageUnitRate);
            //var v4 = (int)(this._ZoomRate * height * this._PageUnitRate);
            //if(v1 == -199 &&v2 == -330 &&v3 == 0 &&v4 == 0 )
            //{
            //    v1 = v1;
            //}
            if (this._ZoomRate == 1)
            {
                this._String.AppendInt32((int)Math.Round(this._AbsoluteOffsetX + x * this._PageUnitRate)); //AppendFloat(this._ZoomRate * (this._AbsoluteOffsetX + x * this._PageUnitRate));
                this._String.AppendInt32((int)Math.Round(this._AbsoluteOffsetY + y * this._PageUnitRate)); //AppendFloat(this._ZoomRate * (this._AbsoluteOffsetY + y * this._PageUnitRate));
                this._String.AppendInt32((int)(width * this._PageUnitRate)); //AppendFloat(this._ZoomRate * width * this._PageUnitRate);
                this._String.AppendInt32((int)(height * this._PageUnitRate)); // AppendFloat(this._ZoomRate * height * this._PageUnitRate);
            }
            else
            {
                this._String.AppendInt32((int)Math.Round(this._ZoomRate * (this._AbsoluteOffsetX + x * this._PageUnitRate))); //AppendFloat(this._ZoomRate * (this._AbsoluteOffsetX + x * this._PageUnitRate));
                this._String.AppendInt32((int)Math.Round(this._ZoomRate * (this._AbsoluteOffsetY + y * this._PageUnitRate))); //AppendFloat(this._ZoomRate * (this._AbsoluteOffsetY + y * this._PageUnitRate));
                this._String.AppendInt32((int)(this._ZoomRate * width * this._PageUnitRate)); //AppendFloat(this._ZoomRate * width * this._PageUnitRate);
                this._String.AppendInt32((int)(this._ZoomRate * height * this._PageUnitRate)); // AppendFloat(this._ZoomRate * height * this._PageUnitRate);
            }
            //AppendFloat(x);
            //this._String.Append(',');
            //AppendFloat(y);
            //this._String.Append(',');
            //AppendFloat(width);
            //this._String.Append(',');
            //AppendFloat(height);
        }
        private int AppendRectangleForCache(float x, float y, float width, float height)
        {
            int result = 0;
            if (this._ZoomRate == 1)
            {
                this._String.AppendInt32((int)(this._AbsoluteOffsetX + x * this._PageUnitRate)); //AppendFloat(this._ZoomRate * (this._AbsoluteOffsetX + x * this._PageUnitRate));
                this._String.AppendInt32((int)(this._AbsoluteOffsetY + y * this._PageUnitRate)); //AppendFloat(this._ZoomRate * (this._AbsoluteOffsetY + y * this._PageUnitRate));
                result = this._String.Length;
                this._String.AppendInt32((int)(width * this._PageUnitRate)); //AppendFloat(this._ZoomRate * width * this._PageUnitRate);
                this._String.AppendInt32((int)(height * this._PageUnitRate)); // AppendFloat(this._ZoomRate * height * this._PageUnitRate);
            }
            else
            {
                this._String.AppendInt32((int)(this._ZoomRate * (this._AbsoluteOffsetX + x * this._PageUnitRate))); //AppendFloat(this._ZoomRate * (this._AbsoluteOffsetX + x * this._PageUnitRate));
                this._String.AppendInt32((int)(this._ZoomRate * (this._AbsoluteOffsetY + y * this._PageUnitRate))); //AppendFloat(this._ZoomRate * (this._AbsoluteOffsetY + y * this._PageUnitRate));
                result = this._String.Length;
                this._String.AppendInt32((int)(this._ZoomRate * width * this._PageUnitRate)); //AppendFloat(this._ZoomRate * width * this._PageUnitRate);
                this._String.AppendInt32((int)(this._ZoomRate * height * this._PageUnitRate)); // AppendFloat(this._ZoomRate * height * this._PageUnitRate);
            }
            return result;
            //AppendFloat(x);
            //this._String.Append(',');
            //AppendFloat(y);
            //this._String.Append(',');
            //AppendFloat(width);
            //this._String.Append(',');
            //AppendFloat(height);
        }

        private void AppendPoint(float x, float y)
        {
            if (this._ZoomRate == 1)
            {
                this._String.AppendInt32((int)Math.Round(this._AbsoluteOffsetX + x * this._PageUnitRate));//AppendFloat(this._ZoomRate * (this._AbsoluteOffsetX + x * this._PageUnitRate));
                this._String.AppendInt32((int)Math.Round(this._AbsoluteOffsetY + y * this._PageUnitRate)); //AppendFloat(this._ZoomRate * (this._AbsoluteOffsetY + y * this._PageUnitRate));
            }
            else
            {
                this._String.AppendInt32((int)Math.Round(this._ZoomRate * (this._AbsoluteOffsetX + x * this._PageUnitRate)));//AppendFloat(this._ZoomRate * (this._AbsoluteOffsetX + x * this._PageUnitRate));
                this._String.AppendInt32((int)Math.Round(this._ZoomRate * (this._AbsoluteOffsetY + y * this._PageUnitRate))); //AppendFloat(this._ZoomRate * (this._AbsoluteOffsetY + y * this._PageUnitRate));
            }
            //AppendFloat(x);
            //this._String.Append(',');
            //AppendFloat(y);
        }

        private MyBinaryBuilder _String;

        private MyImageTable _ImageTable = new MyImageTable();
        private class MyImageTable : ObjectCache<byte[]>
        {
            protected override GraphicsRecordType GetRecordType()
            {
                return GraphicsRecordType.ImageTable;
            }
            protected override void WriteItemTo(MyBinaryBuilder strResult, byte[] item, Graphics g)
            {
                if (FileHeaderHelper.HasJpegHeader(item))
                {
                    strResult.AppendByte((byte)0);// strResult.Append("data:image/jpeg;base64,");
                }
                else if (FileHeaderHelper.HasPNGHeader(item))
                {
                    strResult.AppendByte((byte)1);//strResult.Append("data:image/png;base64,");
                }
                else if (FileHeaderHelper.HasGIFHeader(item))
                {
                    strResult.AppendByte((byte)2);//strResult.Append("data:image/gif;base64,");
                }
                else if (FileHeaderHelper.HasBMPHeader(item))
                {
                    strResult.AppendByte((byte)3);//strResult.Append("data:image/bmp;base64,");
                }
                else
                {
                    strResult.AppendByte((byte)4);//strResult.Append("data:image/png;base64,");
                }
                strResult.AppendByteArray(item);
            }

            private static readonly DCSoft.Common.BinaryEqualityComparer _Comparer = new BinaryEqualityComparer(true);
            public override bool Equals(byte[] item1, byte[] item2)
            {
                return _Comparer.Equals(item1, item2);
            }
            public override int GetHashCode(byte[] item)
            {
                return _Comparer.GetHashCode(item);
            }
        }
        private MyBrushTable _BrushTable = new MyBrushTable();
        private class MyBrushTable : ObjectCache<Brush>
        {
            public short GetIndex(Color fillColor)
            {
                if (base._LastKeyValue is SolidBrush
                    && ((SolidBrush)base._LastKeyValue).ColorValue == fillColor._value)
                {
                    return base._LastIndex;
                }
                return base.GetIndex(new SolidBrush(fillColor));
            }

            public override short GetIndex(Brush v)
            {
                if ((v is SolidBrush) == false)
                {
                    return base.GetIndex(v);
                    //DCSoft.DCConsole.Default.WriteLine("xxxx");
                }
                if ((object)v == (object)base._LastKeyValue)
                {
                    return base._LastIndex;
                }
                if (base._LastKeyValue != null && v.EqualsValue(base._LastKeyValue))
                {
                    return base._LastIndex;
                }
                return base.GetIndex(v);
            }
            protected override GraphicsRecordType GetRecordType()
            {
                return GraphicsRecordType.BrushTable;
            }
            protected override void WriteItemTo(MyBinaryBuilder strResult, Brush item, Graphics g)
            {
                if (item is SolidBrush)
                {
                    strResult.AppendString(ColorToString(((SolidBrush)item).Color));
                }
                else
                {
                    strResult.AppendString("red");
                }
            }
            private static string ColorToString(Color c)
            {
                if (c.A == 255)
                {
                    return ColorTranslator.ToHtml(c);
                }
                else
                {
                    return "rgba(" + c.R + "," + c.G + "," + c.B + "," + (c.A / 255.0).ToString("0.00") + ")";
                }
            }
            public override bool Equals(Brush x, Brush y)
            {
                var sb1 = x as SolidBrush;
                var sb2 = y as SolidBrush;
                if (sb1 != null && sb2 != null)
                {
                    return sb1.ColorValue == sb2.ColorValue;
                }
                else
                {
                    return false;// x.EqualsValue(y);
                }
            }

            public override int GetHashCode(Brush obj)
            {
                if (obj is SolidBrush)
                {
                    return (int)(((SolidBrush)obj).ColorValue);
                }
                else
                {
                    return obj.GetHashCode();
                }
            }
        }
        private MyPenTable _PenTable = new MyPenTable();
        private class MyPenTable : ObjectCache<Pen>
        {
            public override short GetIndex(Pen v)
            {
                if (v != null
                    && base._LastKeyValue != null
                    && v.EqualsValue(base._LastKeyValue))
                {
                    return base._LastIndex;
                }
                return base.GetIndex(v);
            }
            public float ZoomRate = 1f;

            protected override Pen CloneObject(Pen v)
            {
                return v.Clone();
            }
            protected override GraphicsRecordType GetRecordType()
            {
                return GraphicsRecordType.PenTable;
            }
            protected override void WriteItemTo(MyBinaryBuilder strResult, Pen item, Graphics g)
            {
                strResult.AppendString(ColorTranslator.ToHtml(item.Color));
                if (item.Width == 0)
                {
                    strResult.AppendByte(0);
                }
                else if (item.Width == 1)
                {
                    strResult.AppendByte(1);
                }
                else
                {
                    var lw = (byte)Math.Ceiling(this.ZoomRate * item.Width);
                    //if( lw == 7 )
                    //{
                    //    lw = 2;
                    //}
                    strResult.AppendByte(lw);
                }
                strResult.AppendByte((byte)item.DashStyle);
            }
            public override bool Equals(Pen x, Pen y)
            {

                return x.Color.ToArgb() == y.Color.ToArgb() && x.Width == y.Width && x.DashStyle == y.DashStyle;
            }

            public override int GetHashCode(Pen obj)
            {
                return obj.Color.ToArgb() + obj.Width.GetHashCode() + obj.DashStyle.GetHashCode();
            }
        }
        private MyColorTable _ColorTable = new MyColorTable();
        private class MyColorTable : ObjectCache<Color>
        {
            protected override GraphicsRecordType GetRecordType()
            {
                return GraphicsRecordType.ColorTable;
            }
            protected override void WriteItemTo(MyBinaryBuilder strResult, Color item, Graphics g)
            {
                if (item.A != 255)
                {
                    strResult.AppendString("argb(" + item.R + "," + item.G + "," + item.B + "," + (item.A / 255.0) + ")");
                }
                else
                {
                    strResult.AppendString(ColorTranslator.ToHtml(item));
                }
            }
            public override bool Equals(Color x, Color y)
            {
                return x.ToArgb() == y.ToArgb();
            }
            public override int GetHashCode(Color obj)
            {
                return obj.ToArgb();
            }
        }
        private Font _CurrentFont = null;
        private void SetCurrentFont(Font font)
        {
            if (font == null || string.IsNullOrEmpty(font.Name))
            {
                var intIndex = this._FontTable.GetIndex(XFontValue.DefaultFont);
                this._CurrentFont = null;
                _String.AppendRecordType(GraphicsRecordType.SetCurrentFont);
                this._String.AppendInt16((short)intIndex);
            }
            if (this._CurrentFont == null || this._CurrentFont.Equals(font) == false)
            {
                var intIndex = this._FontTable.GetIndex(font);
                _String.AppendRecordType(GraphicsRecordType.SetCurrentFont);
                this._String.AppendInt16((short)intIndex);
                this._CurrentFont = font;
            }
        }
        private MyFontTable _FontTable = new MyFontTable();
        private class MyFontTable : ObjectCache<Font>
        {
            public float ZoomRate = 1f;
            protected override GraphicsRecordType GetRecordType()
            {
                return GraphicsRecordType.FontTable;
            }
            private StringBuilder _StrTemp = new StringBuilder();
            protected override void WriteItemTo(MyBinaryBuilder strResult, Font item, Graphics g)
            {
                if ((item.Style & FontStyle.Bold) == FontStyle.Bold)
                {
                    this._StrTemp.Append("bold ");
                }
                if ((item.Style & FontStyle.Italic) == FontStyle.Italic)
                {
                    this._StrTemp.Append("italic ");
                }
                if (item.Size > 0)
                {
                    var size2 = this.ZoomRate * item.Size;
                    var pxSize = GraphicsUnitConvert.Convert(size2, GraphicsUnit.Point, GraphicsUnit.Pixel);
                    if (Math.Abs(pxSize - (int)pxSize) < 0.5)
                    {
                        this._StrTemp.Append(StringCommon.Int32ToString((int)pxSize)).Append("px ");
                    }
                    else
                    {
                        this._StrTemp.Append(pxSize.ToString("0.00")).Append("px ");
                    }
                }
                if (string.IsNullOrEmpty(item.Name))
                {
                    this._StrTemp.Append("宋体");
                }
                else
                {
                    this._StrTemp.Append(item.Name);
                }
                strResult.AppendString(_StrTemp.ToString());
                this._StrTemp.Clear();
            }

            public override void Clear()
            {
                this._StrTemp = null;
                base.Clear();
            }
            public override bool Equals(Font x, Font y)
            {
                return x.Name == y.Name && x.Size == y.Size && x.Style == y.Style;
            }

            public override int GetHashCode(Font obj)
            {
                var result = obj.Name == null ? 0 : obj.Name.GetHashCode();
                result = result + obj.Size.GetHashCode() + (int)obj.Style;
                return result;
            }
        }
        private abstract class ObjectCache<T> : IEqualityComparer<T>
        {
            protected ObjectCache()
            {
                this._Values = new Dictionary<T, short>(this);
            }
            private Dictionary<T, short> _Values = null;
            protected short _LastIndex = -1;
            protected T _LastKeyValue;
            public virtual short GetIndex(T v)
            {
                short index = 0;
                if (this._Values.TryGetValue(v, out index) == false)
                {
                    index = (short)this._Values.Count;
                    v = CloneObject(v);
                    this._Values[v] = index;
                    this._LastKeyValue = v;
                }
                else
                {
                    this._LastKeyValue = v;
                }
                this._LastIndex = index;
                return index;// StringCommon.Int32ToString(index);
            }
            public virtual void Clear()
            {
                if (this._Values != null)
                {
                    this._Values.Clear();
                    this._Values = null;
                }
            }

            private T[] ToArrary()
            {
                if (this._Values.Count == 0)
                {
                    return null;
                }
                var result = new T[this._Values.Count];
                foreach (var item in this._Values)
                {
                    result[item.Value] = item.Key;
                }
                return result;
            }

            protected virtual T CloneObject(T v)
            {
                return v;
            }

            protected abstract GraphicsRecordType GetRecordType();

            protected abstract void WriteItemTo(MyBinaryBuilder strResult, T item, Graphics g);

            public void WriteTo(MyBinaryBuilder strResult, Graphics g)
            {
                if (this._Values.Count > 0)
                {
#if OutputGraphicsRecordTypeName
                    strResult.Append('"' + this.GetRecordType().ToString() + '"');
#else
                    strResult.AppendRecordType(this.GetRecordType());
#endif
                    var array = this.ToArrary();
                    strResult.AppendInt16((short)array.Length);
                    foreach (var item in array)
                    {
                        WriteItemTo(strResult, item, g);
                    }
                }
            }
            public abstract bool Equals(T item1, T item2);
            public abstract int GetHashCode(T item);
        }
    }
    public enum GraphicsRecordType
    {
        First = 0,
        SetCurrentFont = 1,
        SetCurrentBrush = 2,
        SetCurrentPen = 3,
        DrawLine = 5,
        DrawString = 6,
        DrawChar = 7,
        DrawRoundRectangle = 8,
        DrawRectangle = 9,
        FillRectangle = 10,
        DrawEllipse = 11,
        FillEllipse = 12,
        CheckVersion = 13,
        SetTransform = 14,
        DrawImageExt = 15,
        Save = 16,
        Restore = 17,
        FontTable = 18,
        ColorTable = 19,
        PenTable = 20,
        DrawImageXY = 21,
        DrawImage = 22,
        SetPageUnit = 23,
        SetClip = 27,
        ResetClip = 28,
        ImageTable = 29,
        BrushTable = 30,
        DrawLines = 31,
        UpdateClearRectangle = 32,
        DrawImageXYStdImageIndex = 38,
        DrawImageStdImageIndex = 39,
        ClearRect = 40,
        SetImageSmoothing = 41,
        FillRoundRectangle = 43,
    }
}

namespace DCSystem_Drawing.Drawing2D
{

    public sealed class GraphicsState
    {
        public GraphicsState(Graphics g)
        {
            this._Matrix = g.Transform.Clone();
            this._Unit = g.PageUnit;
            this._ClipRectangle = g.ClipBounds;
            if (this._Matrix.OffsetX != (int)this._Matrix.OffsetX)
            {
                this._ClipRectangle.Width += 1;
            }
        }
        public void Restore(Graphics g)
        {
            g.PageUnit = this._Unit;
            g.Transform = this._Matrix;
            g.ResetClip();
            //g.SetClip(this._ClipRectangle);
        }
        private Matrix _Matrix = null;
        private GraphicsUnit _Unit = GraphicsUnit.Document;
        private RectangleF _ClipRectangle = RectangleF.Empty;
    }

    public sealed class Matrix : IDisposable
    {
        public Matrix()
        {
        }
        internal float _Element0 = 1;
        internal float _Element1 = 0;
        internal float _Element2 = 0;
        internal float _Element3 = 1;
        internal float _Element4 = 0;
        internal float _Element5 = 0;
        public bool HasScale()
        {
            return this._Element0 != 1 || this._Element3 != 1;
        }
        public float[] Elements
        {
            get
            {
                return new float[] {
                    this._Element0 ,
                    this._Element1,
                    this._Element2,
                    this._Element3 ,
                    this._Element4,
                    this._Element5};
            }
        }
        public float OffsetX
        {
            get
            {
                return this._Element4;
            }
        }
        public void Dispose()
        {
        }
        public Matrix Clone()
        {
            return (Matrix)this.MemberwiseClone();
        }
        public void Reset()
        {
            this._Element0 = 1;
            this._Element1 = 0;
            this._Element2 = 0;
            this._Element3 = 1;
            this._Element4 = 0;
            this._Element5 = 0;
        }
        public void Rotate(float angle)
        {
            float rad = angle * (float)Math.PI / 180;
            float cos = (float)Math.Cos(rad);
            float sin = (float)Math.Sin(rad);

            float m11 = this._Element0, m12 = this._Element1, m21 = this._Element2, m22 = this._Element3;

            this._Element0 = m11 * cos + m21 * sin;
            this._Element1 = m12 * cos + m22 * sin;
            this._Element2 = m11 * -sin + m21 * cos;
            this._Element3 = m12 * -sin + m22 * cos;
        }

        public void Translate(float offsetX, float offsetY)
        {
            this._Element4 += this._Element0 * offsetX + this._Element2 * offsetY;
            this._Element5 += this._Element1 * offsetX + this._Element3 * offsetY;
        }

        public void Scale(float scaleX, float scaleY)
        {
            if (scaleX != 1)
            {
                this._Element0 *= scaleX;
                this._Element1 *= scaleX;
            }
            if (scaleY != 1)
            {
                this._Element2 *= scaleY;
                this._Element3 *= scaleY;
            }
        }
        public DCSystem_Drawing.PointF TransformPointF(float x, float y)
        {
            return new DCSystem_Drawing.PointF(
                x * this._Element0 + y * this._Element2 + this._Element4,
                x * this._Element1 + y * this._Element3 + this._Element5
                );
        }
        public void TransformPoints(DCSystem_Drawing.PointF[] pts)
        {
            for (int iCount = pts.Length - 1; iCount >= 0; iCount--)
            {
                float x = pts[iCount].X;
                float y = pts[iCount].Y;
                // a b c d e f
                // 0 1 2 3 4 5
                float x2 = x * this._Element0 + y * this._Element2 + this._Element4;
                float y2 = x * this._Element1 + y * this._Element3 + this._Element5;
                pts[iCount] = new DCSystem_Drawing.PointF(x2, y2);
            }
        }

    }
}