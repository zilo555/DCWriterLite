using System;
using System.Text;
using System.Collections.Generic;
using DCSoft.Common;
using DCSoft.Drawing;
using DCSoft.Writer.Dom;
using DCSystemXml;
using DCSoft.WASM;
using DCSoft.Writer;

namespace DCSoft.SVG
{
    /// <summary>
    /// OFD画布对象
    /// </summary>
    public class SVGGraphics : IDisposable
    {
        private SVGGraphics()
        {

        }
        public static SVGGraphics CreateForSVG(XmlTextWriter writer, DomDocument document)
        {
            if (writer == null)
            {
                throw new ArgumentNullException("writer");
            }
            if (document == null)
            {
                throw new ArgumentNullException("document");
            }
            var g = new SVGGraphics();
            g._AutoDisposeWriter = false;
            g._SVG = writer;

            g.PrepareAddFont(document.DefaultFont);
            foreach (var item in document.ContentStyles.Styles)
            {
                g.PrepareAddFont(item.Font);
            }
            g.PrepareAddFont(new XFontValue("Wingdings", 10));
            return g;
        }
        private bool _AutoDisposeWriter = true;
        private XmlTextWriter _SVG = null;

        private Matrix _Transform = new Matrix();
        public Matrix Transform
        {
            get
            {
                return this._Transform;
            }
            set
            {
                this._Transform = value;
            }
        }
        public void TranslateTransform(float dx, float dy)
        {
            this._Transform.Reset();
            this._Transform.Translate(dx, dy);
        }

        private class OFDState
        {
            public Matrix Transform = null;
            public GraphicsUnit PageUnit = GraphicsUnit.Point;
            public DCSystem_Drawing.RectangleF ClipRectangle = DCSystem_Drawing.RectangleF.Empty;
        }
        public object SaveState()
        {
            var result = new OFDState();
            result.Transform = this._Transform.Clone();
            result.PageUnit = this._PageUnit;
            result.ClipRectangle = this._ClipRectangle;
            return result;
        }
        public void RestoreState(object state)
        {
            var obj = state as OFDState;
            if (obj != null)
            {
                this._Transform = obj.Transform;
                this._PageUnit = obj.PageUnit;
                this._ClipRectangle = obj.ClipRectangle;
            }
        }
        private float _UnitConvertRate = 1;
        private GraphicsUnit _PageUnit = GraphicsUnit.Pixel;
        public GraphicsUnit PageUnit
        {
            get
            {
                return this._PageUnit;
            }
            set
            {
                this._PageUnit = value;
                if (this._SVG == null)
                {
                    switch (this._PageUnit)
                    {
                        case GraphicsUnit.Document: this._UnitConvertRate = 0.08466666666667f; break;
                        case GraphicsUnit.Inch: this._UnitConvertRate = 25.4f; break;
                        case GraphicsUnit.Millimeter: this._UnitConvertRate = 1; break;
                        case GraphicsUnit.Pixel: this._UnitConvertRate = 0.264583341218531f; break;
                        case GraphicsUnit.Point: this._UnitConvertRate = 0.352777777777778f; break;
                        default: throw new NotSupportedException(this._PageUnit.ToString());
                    }
                }
                else
                {
                    switch (this._PageUnit)
                    {
                        case GraphicsUnit.Document: this._UnitConvertRate = 0.32f; break;
                        case GraphicsUnit.Inch: this._UnitConvertRate = 96f; break;
                        case GraphicsUnit.Millimeter: this._UnitConvertRate = 3.7795f; break;
                        case GraphicsUnit.Pixel: this._UnitConvertRate = 1; break;
                        case GraphicsUnit.Point: this._UnitConvertRate = 1.3333333f; break;
                        default: throw new NotSupportedException(this._PageUnit.ToString());
                    }
                }
            }
        }


        public void Dispose()
        {
            if (this._CurrentCharsX != null)
            {
                this._CurrentCharsX.Clear();
                this._CurrentCharsX = null;
                this._CurrentChars.Clear();
                this._CurrentChars = null;
            }
            if (this._SystemImageIDs != null)
            {
                this._SystemImageIDs.Clear();
                this._SystemImageIDs = null;
            }

            //this._LastYString = null;
            if (this._GroupClipRectangles != null)
            {
                this._GroupClipRectangles.Clear();
                this._GroupClipRectangles = null;
            }
            if (this._SVG != null)
            {
                //this._SVG.EventBeforeWriteStartElement = null;
                this.CloseSVGPage();
                if (this._AutoDisposeWriter)
                {
                    this._SVG.WriteEndDocument();
                    this._SVG.Close();
                    this._SVG = null;
                }
                if (this._SVGFontStyles != null)
                {
                    this._SVGFontStyles.Clear();
                    this._SVGFontStyles = null;
                }
                return;
            }
        }

        private float _PageWidth = 100;
        private float _PageHeight = 100;

        private bool _HasAddPage = false;

        private float _PageContentHeight = 0;
        private void CloseSVGPage()
        {
            if (this._HasAddPage)
            {
                this._HasAddPage = false;
                this._SVG.WriteStartElement("rect");
                this._SVG.WriteAttributeString("dctype", "contentheight");
                this._SVG.WriteAttributeString("width", "0");
                this._SVG.WriteAttributeString("height", "0");
                this._SVG.WriteAttributeString("value", this._PageContentHeight.ToString());
                this._SVG.WriteEndElement();
                this._SVG.WriteEndElement();
            }
        }

        private static int _SVGEntryIndex = 1;
        public void StartSVGPageContent(float pageWidth, float pageHeight)
        {
            this._HasAddPage = true;
            if (this._SystemImageIDs != null)
            {
                this._SystemImageIDs.Clear();
                this._SystemImageIDs = null;
            }
            this._PageWidth = pageWidth;
            this._PageHeight = pageHeight;
            this._PageContentHeight = 0;
            this._SVG.WriteStartElement("g");
            if (this._SVGFontStyles != null && this._SVGFontStyles.Count > 0)
            {
                this._SVG.WriteStartElement("style");
                var strCss = new StringBuilder();

                foreach (var item in this._SVGFontStyles)
                {
                    item.UserID = "dcf_" + (_SVGEntryIndex++);
                    strCss.AppendLine("  ." + item.UserID + "{" + ToCSSString(item.Name, item.Size, item.Style) + ";fill:black}");
                }

                // 处理医学表达式线变虚线和表格边框展示不全的问题【DUWRITER5_0-3873】
                string lineCssStr = "line{shape-rendering:auto;}";
                strCss.AppendLine(lineCssStr);
                // 添加不可选中样式给存在属性user-select=none的元素
                string unselectableCssStr = "[user-select=none]{-webkit-user-select:none;-ms-user-select:none;-o-user-select:none;-moz-user-select:none;-khtml-user-select:none;user-select:none}";
                strCss.AppendLine(unselectableCssStr);

                this._SVG.WriteString(strCss.ToString());
                this._SVG.WriteEndElement();
            }
            this._CurrentChars.Clear();
            this._CurrentCharsX.Clear();
            //this._SVG.EventBeforeWriteStartElement = this.CommitCurrentChars;
        }

        private DCSystem_Drawing.RectangleF _ClipRectangle = DCSystem_Drawing.RectangleF.Empty;// MaxRect;
        public void SetClip(DCSystem_Drawing.RectangleF rect)
        {
            this._ClipRectangle = this.ToLocalRectangleFloat(rect.X, rect.Y, rect.Width, rect.Height);
        }
        private class GroupInfo
        {
            public bool ClipRectangleEmpty = false;
            public DCSystem_Drawing.RectangleF ClipRectangle;
            public string FontName;
            public float FontSize;
            public FontStyle vFontStyle;
            public int StartPosition;
            public int ContentStartPosition;
            public StringBuilder BaseStringBuilder;
        }
        private Stack<GroupInfo> _GroupClipRectangles = null;
        public void BeginGroupShape(string strID, RectangleF clipRect, string vFontName, float vFontSize, FontStyle vFontStyle, bool bolShapeRenderingAuto)
        {
            this.CommitCurrentCharsForSVG();
            if (this._SVG != null)
            {
                if (this._GroupClipRectangles == null)
                {
                    this._GroupClipRectangles = new Stack<GroupInfo>();
                }
                var info = new GroupInfo();
                if (clipRect.IsEmpty == false)
                {
                    info.ClipRectangle = this.ToLocalRectangleFloat(
                        clipRect.Left - 2,
                        clipRect.Top - 2,
                        clipRect.Width + 4,
                        clipRect.Height + 7);
                    info.ClipRectangleEmpty = false;
                }
                else
                {
                    info.ClipRectangleEmpty = true;
                }
                info.FontName = vFontName;
                info.FontSize = vFontSize;
                info.vFontStyle = vFontStyle;
                if (this._SVG.BaseTextWriter is System.IO.StringWriter)
                {
                    this._SVG.AutoComplete(XmlTextWriter.Token.Content);
                    //this._SVG.WriteString(" ");
                    var str2 = ((System.IO.StringWriter)this._SVG.BaseTextWriter).GetStringBuilder();
                    info.BaseStringBuilder = str2;
                    info.StartPosition = str2.Length;
                }
                string strClipID = null;
                if (clipRect.Width > 0 && clipRect.Height > 0)
                {
                    this._ClipRectangle = info.ClipRectangle;// this.ToLocalRectangle(clipRect.X, clipRect.Y, clipRect.Width, clipRect.Height);
                    strClipID = GetSVGClipPathID(new DCSystem_Drawing.RectangleF(-10000, -10000, 1, 1));
                }
                this._SVG.WriteStartElement("g");
                if (strID != null && strID.Length > 0)
                {
                    this._SVG.WriteAttributeString("id", strID);
                }
                if (vFontName != null && vFontName.Length > 0)
                {
                    this.WriteSVGFont(vFontName, vFontSize, vFontStyle, DCSystem_Drawing.Color.Black);
                }
                this.WriteSVGClipID(strClipID);
                if (bolShapeRenderingAuto)
                {
                    this._SVG.WriteAttributeStringRaw("shape-rendering", "auto");
                }
                this._GroupClipRectangles.Push(info);
                if (info.BaseStringBuilder != null)
                {
                    info.ContentStartPosition = info.BaseStringBuilder.Length;
                }
            }
        }

        public void EndGroupShape()
        {
            this.CommitCurrentCharsForSVG();
            if (this._SVG != null)
            {
                var info = this._GroupClipRectangles.Pop();
                this._ClipRectangle = info.ClipRectangle;
                this._SVG.WriteEndElement();
                if (info.BaseStringBuilder != null)
                {
                    if (info.BaseStringBuilder.Length - info.ContentStartPosition < 5)
                    {
                        // 认为这组SVG没有输出任何内容，则删除已经输出过的内容。
                        info.BaseStringBuilder.Remove(info.StartPosition, info.BaseStringBuilder.Length - info.StartPosition);
                    }
                    info.BaseStringBuilder = null;
                }
            }
        }
        private static readonly int _Black_ARGB = DCSystem_Drawing.Color.Black.ToArgb();
        private void WriteSVGBrush(Brush b)
        {
            if (b is SolidBrush)
            {
                var fc = ((SolidBrush)b).Color;
                if (fc.ToArgb() == _Black_ARGB)
                {
                    this._SVG.WriteAttributeString("fill", "black");
                }
                else
                {
                    if (fc.A == 255)
                    {
                        this._SVG.WriteAttributeString("fill", DCSystem_Drawing.ColorTranslator.ToHtml(fc));
                    }
                    else
                    {
                        var c2 = DCSystem_Drawing.Color.FromArgb(fc.R, fc.G, fc.B);
                        this._SVG.WriteAttributeString("fill", DCSystem_Drawing.ColorTranslator.ToHtml(c2));
                        this._SVG.WriteAttributeString("fill-opacity", (fc.A / 255.0).ToString());
                    }
                }
            }
            else
            {
                throw new NotSupportedException(b.ToString());
            }
        }
        private void WriteSVGBounds(Rectangle rect)
        {
            this._SVG.WriteAttributeInt32("x", rect.X);
            this._SVG.WriteAttributeInt32("y", rect.Y);
            this._SVG.WriteAttributeInt32("width", rect.Width);
            this._SVG.WriteAttributeInt32("height", rect.Height);
            if (rect.Bottom > this._PageContentHeight)
            {
                this._PageContentHeight = rect.Bottom;
            }
        }

        //wyc20241009:添加开关控制，白色颜色输出成NONE。避免低版本谷歌浏览器显示灰线问题DUWRITER5_0-3695
        internal static bool OutputSVGWhiteColorUsingNoneStroke = false;
        private void WriteSVGPen(Pen p)
        {
            string colorstring = DCSystem_Drawing.ColorTranslator.ToHtml(p.Color);
            if (OutputSVGWhiteColorUsingNoneStroke == true && p.Color == Color.White)
            {
                colorstring = "none";
            }
            ////////////////////////////////////////////////////////////////////////////////////////////
            this._SVG.WriteAttributeString("stroke", colorstring);
            if (p.Width > 1)
            {
                //this._SVG.WriteAttributeString("stroke-width", p.Width.ToString());
                var w2 = GraphicsUnitConvert.ToPixel(p.Width, GraphicsUnit.Document, 96);
                if (w2 > 1)
                {
                    this._SVG.WriteAttributeString("stroke-width", w2.ToString());
                }
            }
            var pd = p.DashStyle;
            if (pd == DashStyle.Dash)
            {
                this._SVG.WriteAttributeString("stroke-dasharray", "5,5");
            }
            else if (pd == DashStyle.DashDot)
            {
                this._SVG.WriteAttributeString("stroke-dasharray", "5,2");
            }
            else if (pd == DashStyle.DashDotDot)
            {
                this._SVG.WriteAttributeString("stroke-dasharray", "5,2,2");
            }
            else if (pd == DashStyle.Dot)
            {
                this._SVG.WriteAttributeString("stroke-dasharray", "2,2");
            }
        }
        public void DrawRectangle(Pen pen, float x, float y, float w, float h, float roundRadio)
        {
            this.CommitCurrentCharsForSVG();
            var rect = this.ToLocalRectangleFloat(x, y, w, h);
            var strClipID = GetSVGClipPathID(rect);
            this._SVG.WriteStartElement("rect");
            this.WriteSVGBounds(rect.ToInt32());
            if (roundRadio > 0)
            {
                var ri = ToLocalLength(roundRadio);
                if (ri > 1)
                {
                    this._SVG.WriteAttributeSingle("rx", ri);
                    this._SVG.WriteAttributeSingle("ry", ri);
                }
            }
            this.WriteSVGClipID(strClipID);
            this._SVG.WriteAttributeString("fill", "none");
            this.WriteSVGPen(pen);
            this._SVG.WriteEndElement();
        }
        public void FillRectangle(Brush b, float x, float y, float w, float h, float roundRadio)
        {
            var rect = this.ToLocalRectangleFloat(x, y, w, h);
            var strClipID = GetSVGClipPathID(rect);
            this._SVG.WriteStartElement("rect");
            this.WriteSVGBounds(rect.ToInt32());
            if (roundRadio > 0)
            {
                var ri = ToLocalLength(roundRadio);
                if (ri > 1)
                {
                    this._SVG.WriteAttributeSingle("rx", ri);
                    this._SVG.WriteAttributeSingle("ry", ri);
                }
            }
            this.WriteSVGClipID(strClipID);
            this.WriteSVGBrush(b);
            this._SVG.WriteEndElement();
        }
        public void DrawLine(Pen p, float x1, float y1, float x2, float y2)
        {
            if (x1 == x2 && y1 == y2)
            {
                // 无意义的数据
                return;
            }
            this.CommitCurrentCharsForSVG();
            var pp1 = this.ToLocalPoint(x1, y1);
            var pp2 = this.ToLocalPoint(x2, y2);
            string strClipID = null;
            if (this._ClipRectangle.IsEmpty == false)
            {
                float vLeft, vTop, vRight, vBottom;
                if (pp1.X < pp2.X)
                {
                    vLeft = pp1.X;
                    vRight = pp2.X;
                }
                else
                {
                    vLeft = pp2.X;
                    vRight = pp1.X;
                }
                if (pp1.Y < pp2.Y)
                {
                    vTop = pp1.Y;
                    vBottom = pp2.Y;
                }
                else
                {
                    vTop = pp2.Y;
                    vBottom = pp1.Y;
                }
                if (vLeft < this._ClipRectangle.Left - 1
                    || vTop < this._ClipRectangle.Top - 1
                    || vRight > this._ClipRectangle.Right + 1
                    || vBottom > this._ClipRectangle.Bottom + 1)
                {
                    strClipID = GetSVGClipPathID(new DCSystem_Drawing.RectangleF(vLeft, vTop, vRight - vLeft, vBottom - vTop));
                }
            }
            this._SVG.WriteStartElement("line");
            if (pp1.X == pp2.X)
            {
                // 竖线
                this._SVG.WriteAttributeInt32AddHalf("x1", pp1.X);
                this._SVG.WriteAttributeInt32("y1", pp1.Y);
                this._SVG.WriteAttributeInt32AddHalf("x2", pp2.X);
                this._SVG.WriteAttributeInt32("y2", pp2.Y);
            }
            else if (pp1.Y == pp2.Y)
            {
                // 横线
                this._SVG.WriteAttributeInt32("x1", pp1.X);
                this._SVG.WriteAttributeInt32AddHalf("y1", pp1.Y);
                this._SVG.WriteAttributeInt32("x2", pp2.X);
                this._SVG.WriteAttributeInt32AddHalf("y2", pp2.Y);
            }
            else
            {
                this._SVG.WriteAttributeInt32("x1", pp1.X);
                this._SVG.WriteAttributeInt32("y1", pp1.Y);
                this._SVG.WriteAttributeInt32("x2", pp2.X);
                this._SVG.WriteAttributeInt32("y2", pp2.Y);
            }
            //this._SVG.WriteAttributeInt32("x1", pp1.X);
            //this._SVG.WriteAttributeInt32("x2", pp2.X);
            //if (pp1.Y == pp2.Y )
            //{
            //    // 横线
            //    this._SVG.WriteAttributeInt32("y1", pp1.Y + 1);
            //    this._SVG.WriteAttributeInt32("y2", pp2.Y + 1);
            //}
            //else
            //{
            //    this._SVG.WriteAttributeInt32("y1", pp1.Y);
            //    this._SVG.WriteAttributeInt32("y2", pp2.Y);
            //}
            this.WriteSVGClipID(strClipID);
            this.WriteSVGPen(p);
            this._SVG.WriteEndElement();
            return;
        }
        public void DrawLines(DCSystem_Drawing.Pen p, DCSystem_Drawing.PointF[] ps)
        {
            if (ps == null || ps.Length < 2)
            {
                return;
            }
            float maxx = 0;
            float maxy = 0;
            float minx = 0;
            float miny = 0;
            for (var iCount = 0; iCount < ps.Length; iCount++)
            {
                var pl = this.ToLocalPointFloat(ps[iCount].X, ps[iCount].Y);
                ps[iCount] = pl;
                if (iCount == 0 || maxx < pl.X) maxx = pl.X;
                if (iCount == 0 || minx > pl.X) minx = pl.X;
                if (iCount == 0 || maxy < pl.Y) maxy = pl.Y;
                if (iCount == 0 || miny > pl.Y) miny = pl.Y;
                if (this._PageContentHeight < maxy)
                {
                    this._PageContentHeight = maxy;
                }
            }
            minx--;
            miny--;
            maxx += 2;
            maxy += 2;
            this.CommitCurrentCharsForSVG();
            this._SVG.WriteStartElement("path");
            var strLines = new StringBuilder();
            strLines.Append("M ");
            strLines.Append(XmlTextWriter.SingleToString(ps[0].X));
            strLines.Append(' ');
            strLines.Append(XmlTextWriter.SingleToString(ps[0].Y));
            for (var iCount = 1; iCount < ps.Length; iCount++)
            {
                strLines.Append(" L ");
                strLines.Append(XmlTextWriter.SingleToString(ps[iCount].X));
                strLines.Append(' ');
                strLines.Append(XmlTextWriter.SingleToString(ps[iCount].Y));
            }
            this._SVG.WriteAttributeString("fill", "none");
            this._SVG.WriteAttributeString("d", strLines.ToString());
            //this.WriteSVGClipID(strClipID);
            this.WriteSVGPen(p);
            this._SVG.WriteEndElement();
        }


        private class SystemImageInfo
        {
            public int X = 0;
            public int Y = 0;
            public string ID = null;
        }

        private Dictionary<byte[], SystemImageInfo> _SystemImageIDs = null;
        public void DrawImage(Image img, float x, float y, float width, float height, byte[] rawData)
        {
            this.CommitCurrentCharsForSVG();
            if (rawData == null || rawData.Length == 0)
            {
                if (img is Bitmap)
                {
                    rawData = ((Bitmap)img).Data;
                }
                else
                {
                    throw new ArgumentNullException("img");
                }
            }
            var rect = this.ToLocalRectangle(x, y, width, height);
            if (DomDocument.IsDrawingSystemImage)
            {
                SystemImageInfo imgBack = null;
                if (this._SystemImageIDs != null
                    && this._SystemImageIDs.TryGetValue(rawData, out imgBack))
                {
                    this._SVG.WriteStartElement("use");
                    this._SVG.WriteAttributeStringRaw("href", imgBack.ID);
                    this._SVG.WriteAttributeInt32("x", rect.Left - imgBack.X);
                    if (rect.Top != imgBack.Y)
                    {
                        this._SVG.WriteAttributeInt32("y", rect.Top - imgBack.Y);
                    }
                    this._SVG.WriteEndElement();
                    DomDocument.IsDrawingSystemImage = false;
                    return;
                }
            }
            this._SVG.WriteStartElement("image");
            this.WriteSVGBounds(rect);
            if (DomDocument.IsDrawingSystemImage)
            {
                DomDocument.IsDrawingSystemImage = false;
                if (this._SystemImageIDs == null)
                {
                    this._SystemImageIDs = new Dictionary<byte[], SystemImageInfo>(new DCSoft.Common.BinaryEqualityComparer());
                }
                var strID3 = "dcsi" + (_SVGEntryIndex++);
                this._SVG.WriteAttributeStringRaw("id", strID3);
                var info = new SystemImageInfo();
                info.ID = "#" + strID3;
                info.X = rect.Left;
                info.Y = rect.Top;
                this._SystemImageIDs[rawData] = info;
            }
            this._SVG.WriteAttributeImageData("href", rawData);
            //this._SVG.WriteAttributeStringRaw("href", XImageValue.StaticGetEmitImageSource(rawData));
            this._SVG.WriteAttributeString("decoding", "sync");

            //wyc20250319:针对图片元数据与图片元素宽高比差距较大的情况下，输出preserveAspectRatio属性使其显示正常DUWRITER5_0-4162
            if (img != null)
            {
                float originrate = (float)img.Width / (float)img.Height;
                float nowrate = width / height;
                if (Math.Abs(originrate - nowrate) > 0.01)
                {
                    this._SVG.WriteAttributeString("preserveAspectRatio", "none");
                }
            }
            this._SVG.WriteEndElement();
        }

        public delegate DCSystem_Drawing.RectangleF[] LayoutStringHandler(
            GraphicsUnit pageUnit,
            string text,
            string fontName,
            float fontSize,
            FontStyle vStyle,
             DCSystem_Drawing.RectangleF bounds,
            StringFormat format,
            out string[] lines);
        /// <summary>
        /// 多行文本排版事件
        /// </summary>
        public static LayoutStringHandler EventLayoutString = DCSoft.Writer.Dom.CharacterMeasurer.LayoutString;

        private int _CurrentCharStyleIndex = -100;

        private string _CurrentCharFontName = null;
        private float _CurrentCharFontSize = 0;
        private FontStyle _CurrentCharFontStyle = FontStyle.Regular;
        private Color _CurrentCharTextColor = Color.Black;
        private float _CurrentCharY = 0;
        private List<float> _CurrentCharsX = new List<float>();
        private DCList<char> _CurrentChars = new DCList<char>();

        private static char[] _Buffer_CurrentChars = new char[40];

        internal void CommitCurrentCharsForSVG()
        {
            if (this._CurrentChars == null || this._CurrentChars.Count == 0)
            {
                return;
            }
            this._SVG.WriteStartElement("text");
            this._SVG.WriteStartAttribute(null, "x", null);
            this._SVG.AutoComplete(XmlTextWriter.Token.Content);
            var charCount = this._CurrentCharsX.Count;
            var w = ((System.IO.StringWriter)this._SVG.BaseTextWriter).GetStringBuilder();
            for (var iCount = 0; iCount < charCount; iCount++)
            {
                var lx = this.ToLocalPoint(this._CurrentCharsX[iCount], 0).X;
                var len = XmlTextWriter.StaticAppendInt32(_Buffer_CurrentChars, 0, lx);
                if (iCount < charCount - 1)
                {
                    _Buffer_CurrentChars[len++] = ' ';
                }
                w.Append(_Buffer_CurrentChars, 0, len);
            }
            this._SVG.WriteEndAttribute();
            this._SVG.WriteAttributeInt32UseLastValue("y", this.ToLocalPoint(0, this._CurrentCharY).Y);
            this.WriteSVGFont(
                this._CurrentCharFontName,
                this._CurrentCharFontSize,
                this._CurrentCharFontStyle,
                this._CurrentCharTextColor);
            this._SVG.AutoComplete(XmlTextWriter.Token.Content);
            w.Append(this._CurrentChars.InnerGetArrayRaw(), 0, this._CurrentChars.Count);
            this._SVG.WriteEndElement();
            this._CurrentChars.Clear();
            this._CurrentCharsX.Clear();
            this._CurrentCharFontName = null;
        }

        public void FastDrawChar(
            char cv,
            int styleIndex,
            string fontName,
            float fontSize,
            FontStyle vStyle,
            Color txtColor,
            float x,
            float y,
            float fontHeight)
        {
            styleIndex = _CurrentCharStyleIndex;
            if( cv == ' ' || cv == '\t' || DomCharElement.IsHtmlWhitespace(cv))
            {
                return;
            }
            if (XmlCharType.IsAttributeValueChar(cv))
            {
                if (this._CurrentChars.Count > 0)
                {
                    if (this._CurrentCharFontName != fontName
                        || this._CurrentCharFontSize != fontSize
                        || this._CurrentCharFontStyle != vStyle
                        || this._CurrentCharTextColor != txtColor
                        || this._CurrentCharY != y
                        || this._CurrentCharStyleIndex != styleIndex)
                    {
                        this.CommitCurrentCharsForSVG();
                    }
                }
                if (this._CurrentChars.Count == 0)
                {
                    this._CurrentCharFontName = fontName;
                    this._CurrentCharFontSize = fontSize;
                    this._CurrentCharFontStyle = vStyle;
                    this._CurrentCharTextColor = txtColor;
                    this._CurrentCharY = y;
                    this._CurrentCharStyleIndex = styleIndex;
                }
                this._CurrentCharsX.Add(x);
                this._CurrentChars.Add(cv);
                return;
            }
            // 遇到特殊字符，直接绘制
            this.CommitCurrentCharsForSVG();
            var p = this.ToLocalPoint(x, y);
            this._SVG.WriteStartElement("text");
            this._SVG.WriteAttributeInt32("x", p.X);
            this._SVG.WriteAttributeInt32UseLastValue("y", p.Y);
            if (p.Y > this._PageContentHeight)
            {
                this._PageContentHeight = p.Y;
            }
            if (fontHeight > 0)
            {
                var fh2 = this.ToLocalLength(fontHeight);
                if (this._PageContentHeight < fh2 + p.Y)
                {
                    this._PageContentHeight = fh2 + p.Y;
                }
            }
            else if (this._PageContentHeight < p.Y)
            {
                this._PageContentHeight += p.Y;
            }
            var bolWriteFont = true;
            if (this._GroupClipRectangles != null && this._GroupClipRectangles.Count > 0)
            {
                var info4 = this._GroupClipRectangles.Peek();
                if (info4.FontName == fontName
                    && info4.FontSize == fontSize
                    && info4.vFontStyle == vStyle
                    && txtColor.ToArgb() == _Black_ARGB)
                {
                    bolWriteFont = false;
                }
            }
            if (bolWriteFont)
            {
                this.WriteSVGFont(fontName, fontSize, vStyle, txtColor);
            }
            this._SVG.WriteChar(cv);
            this._SVG.WriteEndElement();
        }
        public void DrawString(
            string text,
            string fontName,
            float fontSize,
            FontStyle vStyle,
            Color txtColor,
            RectangleF layoutRectangle,
            StringFormat format)
        {
            if (text == null || text.Length == 0)
            {
                return;
            }
            if (text.Length == 1 && text[0] == ' ')
            {
                // 不绘制空格
                return;
            }
            this.CommitCurrentCharsForSVG();
            if (format == null)
            {
                format = StringFormat.GenericTypographic;
            }
            var info = TrueTypeFontSnapshort.GetInstance(fontName, vStyle);
            if (info == null || info.SupportAllChars(text) == false)
            {
                info = DefaultTrueTypeFont;
                if (info == null)
                {
                    throw new InvalidOperationException("DefaultTrueTypeFont=null");
                }
                fontName = info.FontName;
            }
            if (EventLayoutString == null)
            {
                throw new NotSupportedException("OFD.EventLayoutString");
            }
            string[] strLines = null;

            var rects = EventLayoutString(
                this._PageUnit,
                text,
                fontName,
                fontSize,
                vStyle,
                layoutRectangle,
                format,
                out strLines);
            if (rects == null || rects.Length == 0)
            {
                return;
            }
            for (var iCount = 0; iCount < rects.Length; iCount++)
            {
                var rect = rects[iCount];
                //this.DrawRectangle(Pens.Blue, rect.Left, rect.Top, rect.Width, rect.Height,0);
                this.DrawSingleLineText(
                    strLines[iCount],
                    fontName,
                    fontSize,
                    vStyle,
                    txtColor,
                    rect.Left,
                    rect.Top,
                    rect.Width,
                    rect.Height,
                    info);
            }
        }

        public static TrueTypeFontSnapshort DefaultTrueTypeFont = TrueTypeFontSnapshort.GetInstance(XFontValue.DefaultFontName, FontStyle.Regular);
        private static string ToCSSString(string fontName, float fontSize, FontStyle vStyle)
        {
            var str = new StringBuilder();
            str.Append("dominant-baseline:text-before-edge");
            if (fontName != null && fontName.Length > 0)
            {
                str.Append(";font-family:" + fontName);
            }
            else
            {
                str.Append(";font-family:" + XFontValue.DefaultFontName);
            }
            if (fontSize > 0)
            {
                str.Append(";font-size:" + fontSize + "pt");
            }
            if ((vStyle & FontStyle.Italic) == FontStyle.Italic)
            {
                str.Append(";font-style:italic");
            }
            else
            {
                str.Append(";font-style:normal");
            }
            if ((vStyle & FontStyle.Bold) == FontStyle.Bold)
            {
                str.Append(";font-weight:bold");
            }
            else
            {
                str.Append(";font-weight:normal");
            }
            return str.ToString();
        }
        private List<MyPDFFontInfo> _SVGFontStyles = null;
        public void PrepareAddFont(XFontValue f)
        {
            if (f != null)
            {
                var info = new MyPDFFontInfo(f.Name, f.Size, f.Style);
                if (this._SVGFontStyles == null)
                {
                    this._SVGFontStyles = new List<MyPDFFontInfo>();
                }
                foreach (var item in this._SVGFontStyles)
                {
                    if (item.EqualsValue(info))
                    {
                        return;
                    }
                }
                this._SVGFontStyles.Add(info);
            }
        }

        private static int _ClipRectangleEntryIndex = 0;
        private string GetSVGClipPathID(RectangleF localRect)
        {
            if (this._ClipRectangle.IsEmpty == false
                && this._ClipRectangle.Contains(localRect) == false)
            {
                // 超出了剪切矩形
                if (this._GroupClipRectangles != null && this._GroupClipRectangles.Count > 0)
                {
                    var info2 = this._GroupClipRectangles.Peek();
                    if (info2.ClipRectangleEmpty || info2.ClipRectangle == this._ClipRectangle)
                    {
                        // 无需重复设置剪切矩形
                        return null;
                    }
                }
                var strID = "dccp_" + (_ClipRectangleEntryIndex++);
                this._SVG.WriteStartElement("clippath");
                this._SVG.WriteAttributeString("id", strID);
                this._SVG.WriteStartElement("rect");
                var rect2 = this._ClipRectangle.ToInt32();
                this._SVG.WriteAttributeInt32("x", rect2.Left - 1);
                this._SVG.WriteAttributeInt32("y", rect2.Top - 1);
                this._SVG.WriteAttributeInt32("width", rect2.Width + 2);
                this._SVG.WriteAttributeInt32("height", rect2.Height + 2);
                this._SVG.WriteEndElement();
                this._SVG.WriteEndElement();
                return strID;
            }
            return null;
        }
        private void WriteSVGClipID(string strPathID)
        {
            if (strPathID != null && strPathID.Length > 0)
            {
                this._SVG.WriteAttributeString("clip-path", "url(#" + strPathID + ")");
            }
        }
        private void WriteSVGFont(string vFontName, float vFontSize, FontStyle vStyle, Color txtColor)
        {
            var bolWriteCss = true;
            if (this._SVGFontStyles != null && this._SVGFontStyles.Count > 0)
            {
                //var f2 = new MyPDFFontInfo(vFontName, vFontSize, vStyle);
                foreach (var item2 in this._SVGFontStyles)
                {
                    if (item2.EqualsValue(vFontName, vFontSize, vStyle))
                    {
                        //wyc20241024:针对宋体加粗补充classname
                        string classname = item2.UserID;
                        //if (vFontName == "宋体" && vStyle.HasFlag( FontStyle.Bold) == true)
                        //{
                        //    classname = item2.UserID + " simsunbold";
                        //}
                        //////////////////////////////////////////

                        // 命中字体样式
                        this._SVG.WriteAttributeStringRaw("class", classname);
                        bolWriteCss = false;
                        break;
                    }
                }
            }

            if (bolWriteCss)
            {
                this._SVG.WriteAttributeStringRaw(
                    "style",
                    ToCSSString(vFontName, vFontSize, vStyle)
                    + ";fill:" + ColorTranslator.ToHtml(txtColor));
            }
            else if (txtColor.ToArgb() != _Black_ARGB)
            {
                // 非黑色文字
                this._SVG.WriteAttributeStringRaw("style", "fill:" + ColorTranslator.ToHtml(txtColor));
            }
        }

        private void DrawSingleLineText(
            string text,
            string fontName,
            float fontSize,
            FontStyle vStyle,
             DCSystem_Drawing.Color txtColor,
            float x,
            float y,
            float w,
            float h,
            TrueTypeFontSnapshort info)
        {
            RectangleF rect = this.ToLocalRectangleFloat(x, y, w, h);
            //var strClipID = GetSVGClipPathID(rect);
            //if( strClipID != null && text.Length == 1 )
            //{
            //    strClipID = GetSVGClipPathID(rect);
            //}
            this._SVG.WriteStartElement("text");
            if (this._PageContentHeight < rect.Bottom)
            {
                this._PageContentHeight = rect.Bottom;
            }
            var bolHasScaleTransform = this._Transform.HasScale();
            if (text.Contains(' ', StringComparison.Ordinal))
            {
                // 文本出现空格，则需要特殊处理，来体现空格的排版宽度
                var xScaleRate = this._Transform._Element0;
                if (xScaleRate <= 0)
                {
                    xScaleRate = 1;
                }
                var yScaleRate = this._Transform._Element3;
                if (yScaleRate <= 0)
                {
                    yScaleRate = 1;
                }
                var txtLen = text.Length;
                var strXPos = new StringBuilder();
                var leftCount = rect.X;
                var strNewText = new StringBuilder();
                for (var iCount = 0; iCount < txtLen; iCount++)
                {
                    var cv = text[iCount];
                    {
                        if (cv == ' ' || DomCharElement.IsHtmlWhitespace(cv))
                        {
                            // 不输出空格字符，只让它占个位置
                            leftCount += info.GetCharWidth(cv, fontSize, this._PageUnit) * this._UnitConvertRate;
                            continue;
                        }
                        strNewText.Append(cv);
                        strXPos.Append(Math.Round(leftCount / xScaleRate));
                        strXPos.Append(' ');
                        leftCount += info.GetCharWidth(cv, fontSize, this._PageUnit) * this._UnitConvertRate;
                    }
                }

                //wyc20241016:0宽度防止报错
                if (strXPos.Length > 0)
                {
                    strXPos.Remove(strXPos.Length - 1, 1);
                }

                text = strNewText.ToString();
                this._SVG.WriteAttributeStringRaw("x", strXPos.ToString());
                this._SVG.WriteAttributeInt32("y", (int)Math.Round(rect.Y / yScaleRate));
            }
            else
            {
                if (bolHasScaleTransform)
                {
                    this._SVG.WriteAttributeInt32("x", (int)(rect.X / this._Transform._Element0));
                    this._SVG.WriteAttributeInt32("y", (int)Math.Round(rect.Y / this._Transform._Element3));
                }
                else
                {
                    this._SVG.WriteAttributeInt32("x", (int)rect.X);
                    this._SVG.WriteAttributeInt32("y", (int)Math.Round(rect.Y));
                }
                this._SVG.WriteAttributeSingle("textLength", rect.Width);
            }
            if (bolHasScaleTransform)
            {
                this._SVG.WriteAttributeStringRaw(
                    "transform",
                    "scale(" + this._Transform._Element0 + " " + this._Transform._Element3 + ")");
            }
            //this.WriteSVGClipID(strClipID);
            var bolWriteFont = true;
            if (this._GroupClipRectangles != null && this._GroupClipRectangles.Count > 0)
            {
                var info4 = this._GroupClipRectangles.Peek();
                if (info4.FontName == fontName
                    && info4.FontSize == fontSize
                    && info4.vFontStyle == vStyle
                    && txtColor.ToArgb() == _Black_ARGB)
                {
                    bolWriteFont = false;
                }
            }
            if (bolWriteFont)
            {
                this.WriteSVGFont(fontName, fontSize, vStyle, txtColor);
            }
            this._SVG.WriteString(text);
            this._SVG.WriteEndElement();
        }

        private Point ToLocalPoint(float x, float y)
        {
            var p = this._Transform.TransformPointF(x, y);
            return new Point(
                (int)Math.Round(p.X * this._UnitConvertRate),
                (int)Math.Round(p.Y * this._UnitConvertRate));
        }
        private PointF ToLocalPointFloat(float x, float y)
        {
            var p = this._Transform.TransformPointF(x, y);
            p.X = FixDecimal(p.X * this._UnitConvertRate);
            p.Y = FixDecimal(p.Y * this._UnitConvertRate);
            return p;
            //var ps = new PointF[] { new PointF(x, y) };
            //this._Transform.TransformPoints(ps);
            //var p = ps[0];
            //p.X = p.X * this._UnitConvertRate;
            //p.Y = p.Y * this._UnitConvertRate;
            //return p;
        }
        private float ToLocalLength(float len)
        {
            return FixDecimal(len * this._UnitConvertRate);
        }

        private static float FixDecimal(float v)
        {
            var v2 = (int)(v * 100);
            return (float)v2 / 100;
        }
        public Rectangle ToLocalRectangle(float x, float y, float width, float height)
        {
            var p = this.ToLocalPointFloat(x, y);
            var vLeft = (int)Math.Round(p.X);
            var vTop = (int)Math.Round(p.Y);
            var vRight = (int)Math.Round(p.X + width * this._UnitConvertRate);
            var vBottom = (int)Math.Round(p.Y + height * this._UnitConvertRate);
            return new Rectangle(vLeft, vTop, vRight - vLeft, vBottom - vTop);
        }

        private RectangleF ToLocalRectangleFloat(float x, float y, float width, float height)
        {
            var p = this.ToLocalPointFloat(x, y);
            return new RectangleF(
                p.X,
                p.Y,
                FixDecimal(width * this._UnitConvertRate),
                FixDecimal(height * this._UnitConvertRate));
        }

        /// <summary>
        /// 表示字体定义信息的对象
        /// </summary>
        public class MyPDFFontInfo
        {
            public MyPDFFontInfo(string fName, float si, FontStyle st = FontStyle.Regular, GraphicsUnit u = GraphicsUnit.Point)
            {
                this.Name = fName;
                this.Size = si;
                this.Style = st;
                this.Unit = u;
                this._HashCode = fName.GetHashCode();
                this._HashCode += si.GetHashCode();
                this._HashCode += (int)st;
                this._HashCode += 10 * (int)u;
            }
            public string UserID = null;

            public bool EqualsValue(string vFontName, float vFontSize, FontStyle vStyle)
            {
                return this.Name == vFontName && this.Size == vFontSize && this.Style == vStyle;
            }
            public bool EqualsValue(MyPDFFontInfo f)
            {
                if (f == null)
                {
                    return false;
                }
                if (f == this)
                {
                    return true;
                }
                if (f._HashCode == this._HashCode)
                {
                    if (this.Name == f.Name && this.Size == f.Size && this.Style == f.Style)
                    {
                        return true;
                    }
                }
                return false;
            }
#if !RELEASE
            public override string ToString()
            {
                return this.Name + " " + this.Size + " " + this.Style;
            }
#endif

            public readonly string Name;
            public readonly float Size;
            public readonly FontStyle Style;
            public readonly GraphicsUnit Unit;
            private readonly int _HashCode;
            public override int GetHashCode()
            {
                return this._HashCode;
            }
            public override bool Equals(object obj)
            {
                if (obj == this)
                {
                    return true;
                }
                var info = (MyPDFFontInfo)obj;
                return this.Name == info.Name
                    && this.Size == info.Size
                    && this.Style == info.Style
                    && this.Unit == info.Unit;
            }
        }

    }
}