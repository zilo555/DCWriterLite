
using System.Text;
using DCSoft.Common;
using DCSoft.Drawing;
using System.IO;

namespace DCSystem_Drawing.Drawing2D
{
    [System.Reflection.Obfuscation( Exclude = true , ApplyToMembers = true )]
    [System.Text.Json.Serialization.JsonConverter(typeof(System.Text.Json.Serialization.JsonStringEnumConverter))]
    public enum DashStyle
    {
        Solid,
        Dash,
        Dot,
        DashDot,
        DashDotDot,
        Custom
    }
   
}
namespace DCSystem_Drawing
{
    public sealed class StringFormat : IDisposable
    {
        static StringFormat()
        {
            _GenericDefault = new StringFormat();
            _GenericDefault._Alignment = StringAlignment.Near;
            _GenericDefault._LineAlignment = StringAlignment.Near;
            _GenericDefault._Readonly = true;

            _GenericTypographic = new StringFormat();
            _GenericTypographic._Alignment = StringAlignment.Near;
            _GenericTypographic._LineAlignment = StringAlignment.Near;
            _GenericTypographic._Readonly = true;
        }

        private static readonly StringFormat _GenericDefault;
        public static StringFormat GenericDefault
        {
            get
            {
                return _GenericDefault;
            }
        }

        private static readonly StringFormat _GenericTypographic;
        public static StringFormat GenericTypographic
        {
            get
            {
                return _GenericTypographic;
            }
        }

        public StringFormat(StringFormat format)
        {
            if (format == null)
            {
                throw new ArgumentNullException("format");
            }
            format.CheckDispose();
            this._Alignment = format._Alignment;
            this._LineAlignment = format._LineAlignment;
            this._FormatFlags = format._FormatFlags;
        }

        private StringFormatFlags _FormatFlags = StringFormatFlags.None;
        public StringFormatFlags FormatFlags
        {
            get
            {
                return this._FormatFlags;
            }
            set
            {
                CheckReadonly();
                this._FormatFlags = value;
            }
        }

        private StringAlignment _Alignment = StringAlignment.Near;
        public StringAlignment Alignment
        {
            get
            {
                CheckDispose();
                return this._Alignment;
            }
            set
            {
                CheckReadonly();
                this._Alignment = value;
            }
        }

        private StringAlignment _LineAlignment = StringAlignment.Near;
        public StringAlignment LineAlignment
        {
            get
            {
                CheckDispose();
                return this._LineAlignment;
            }
            set
            {
                CheckReadonly();
                this._LineAlignment = value;
            }
        }


        public StringFormat()
            : this((StringFormatFlags)0)
        {
        }


        public StringFormat(StringFormatFlags options)
        {
            this._FormatFlags = options;
        }
#if !RELEASE
        public override string ToString()
        {
            return "[StringFormat, FormatFlags=" + this.FormatFlags.ToString() + "]";
        }
#endif
        private bool _Readonly ;
        private void CheckReadonly()
        {
            if (this._Readonly)
            {
                throw new InvalidOperationException("对象是永恒的");
            }
            CheckDispose();
        }
        private bool _Disposed  ;
        public void Dispose()
        {
            if (this._Readonly)
            {
                throw new InvalidOperationException("对象是永恒的");
            }
            _Disposed = true;
        }
        private void CheckDispose()
        {
            if (this._Disposed)
            {
                throw new ObjectDisposedException(this.GetType().Name);
            }
        }

        public StringFormat Clone()
        {
            var f = (StringFormat)this.MemberwiseClone();
            f._Readonly = false;
            return f;
        }
    }

    [Flags]
    public enum StringFormatFlags
    {
        None = 0 ,
        NoWrap = 0x1000
    }
    

    [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
    [System.Text.Json.Serialization.JsonConverter(typeof(System.Text.Json.Serialization.JsonStringEnumConverter))]
    public enum StringAlignment
    {
        Near,
        Center,
        Far
    }


    [Flags]
    [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
    [System.Text.Json.Serialization.JsonConverter(typeof(System.Text.Json.Serialization.JsonStringEnumConverter))]
    public enum FontStyle
    {
        Regular = 0x0,
        Bold = 0x1,
        Italic = 0x2,
        Underline = 0x4,
        Strikeout = 0x8
    }
    [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
    public enum GraphicsUnit
    {
        World,
        Display,
        Pixel,
        Point,
        Inch,
        Document,
        Millimeter
    }
    public static class Pens
    {
        public static readonly Pen Black = new Pen(Color.Black, true);
        public static readonly Pen Blue = new Pen(Color.Blue, true);
        public static readonly Pen Gray = new Pen(Color.Gray, true);
        public static readonly Pen Green = new Pen(Color.Green, true);
        public static readonly Pen Red = new Pen(Color.Red, true);
        public static readonly Pen White = new Pen(Color.White, true);
        public static readonly Pen Yellow = new Pen(Color.Yellow, true);
        private static readonly int ARGB_Black = Color.Black.ToArgb();
        private static readonly int ARGB_White = Color.White.ToArgb();
        private static readonly int ARGB_Red = Color.Red.ToArgb();
        private static readonly int ARGB_Gray = Color.Gray.ToArgb();

        private static readonly Dictionary<int, Pen> _Pens = new Dictionary<int, Pen>();
        public static Pen GetValue(Color c)
        {
            var argb = c.ToArgb();

            if (argb == ARGB_Black)
            {
                return Black;
            }
            if (argb == ARGB_Gray)
            {
                return Gray;
            }
            if (argb == ARGB_White)
            {
                return White;
            }
            if (argb == ARGB_Red)
            {
                return Red;
            }
            Pen result = null;
            if (_Pens.TryGetValue(argb, out result))
            {
                return result;
            }
            else
            {
                result = new Pen(c);
                _Pens.Add(argb, result);
                return result;
            }
        }
    }
    public sealed class Pen : IDisposable
    {
        public Pen( Brush b, float width)
        {
            if (b is SolidBrush)
            {
                this._Color = ((SolidBrush)b).Color;
            }
            this._Width = width;
        }
        public Pen(Color c, float width,DashStyle style )
        {
            this._Color = c;
            this._Width = width;
            this._DashStyle = style;
        }

        public Pen( Color c)
        {
            this._Color = c;
        }
        public Pen( Color c, float w)
        {
            this._Color = c;
            this._Width = w;
        }

        internal Pen( Color c, bool immutable)
        {
            this._Color = c;
            this._immutable = immutable;
        }

        public Pen Clone()
        {
            return (Pen)this.MemberwiseClone();
        }
        private float _Width = 1;
        public float Width
        {
            get
            {
                this.CheckDisposed();
                return this._Width;
            }
            set
            {
                CheckImmutable();
                this.CheckDisposed();
                this._Width = value;
            }
        }
        private  Drawing2D.DashStyle _DashStyle =  Drawing2D.DashStyle.Solid;
        public  Drawing2D.DashStyle DashStyle
        {
            get
            {
                CheckDisposed();
                return this._DashStyle;
            }
            set
            {
                CheckImmutable();
                CheckDisposed();
                this._DashStyle = value;
            }
        }

        private DCSystem_Drawing.Color _Color = DCSystem_Drawing.Color.Black;
        public DCSystem_Drawing.Color Color
        {
            get
            {
                CheckDisposed();
                return this._Color;
            }
            set
            {
                CheckImmutable();
                CheckDisposed();
                this._Color = value;
            }
        }
        private bool _Disposed  ;
        public void Dispose()
        {
            CheckImmutable();
            this._Disposed = true;
        }
        private readonly  bool _immutable  ;
        private void CheckImmutable()
        {
            if (this._immutable)
            {
                throw new InvalidOperationException("对象是永恒的");
            }
        }
        private void CheckDisposed()
        {
            if (this._Disposed)
            {
                throw new ObjectDisposedException(this.GetType().Name);
            }
        }
        public bool EqualsValue( Pen p2 )
        {
            if(this == p2 )
            {
                return true;
            }
            return this._Color == p2._Color && this._Width == p2._Width && this._DashStyle == p2._DashStyle;
        }
    }

    public sealed class Font : IDisposable
    {
        public Font(string name, float size)
        {
            this._Name = name;
            this._Size = size;
        }
        public Font(string name, float size, FontStyle style)
        {
            this._Name = name;
            this._Size = size;
            this._Style = style;
        }
        public Font(string name, float size, FontStyle style, GraphicsUnit unit)
        {
            this._Name = name;
            this._Size = size;
            this._Style = style;
            this._Unit = unit;
        }
        public bool Equals(Font f)
        {
            if (f == null)
            {
                return false;
            }
            if(f == this )
            {
                return true;
            }
            return this._Name == f._Name
                && this._Size == f._Size
                && this._Style == f._Style
                && this._Unit == f._Unit;
        }
        public float SizeInPoints
        {
            get
            {
                return this._Size;
            }
        }
        private string _Name  ;
        public string Name
        {
            get
            {
                CheckDisposed();
                return this._Name;
            }
        }
        private float _Size ;
        public float Size
        {
            get
            {
                CheckDisposed();
                return this._Size;
            }
        }
        private FontStyle _Style = FontStyle.Regular;
        public FontStyle Style
        {
            get
            {
                CheckDisposed();
                return this._Style;
            }
        }
        private GraphicsUnit _Unit = GraphicsUnit.Point;
        public GraphicsUnit Unit
        {
            get
            {
                CheckDisposed();
                return this._Unit;
            }
        }
        public bool Italic
        {
            get
            {
                CheckDisposed();
                return HasStyle(FontStyle.Italic);
            }
        }
        public bool Underline
        {
            get
            {
                CheckDisposed();
                return HasStyle(FontStyle.Underline);
            }
        }
        public bool Strikeout
        {
            get
            {
                CheckDisposed();
                return HasStyle(FontStyle.Strikeout);
            }
        }
        public bool Bold
        {
            get
            {
                CheckDisposed();
                return HasStyle(FontStyle.Bold);
            }
            set
            {
                CheckDisposed();
                if (value)
                {
                    this._Style |= FontStyle.Bold;
                }
                else
                {
                    this._Style &= ~FontStyle.Bold;
                }
            }
        }

        public float GetHeight()
        {
            return this._Size;
        }
        public float GetHeight(Graphics g)
        {
            var info = DCSoft.TrueTypeFontSnapshort.GetInstance(this._Name, this._Style);
            if (info == null)
            {
                return GraphicsUnitConvert.Convert(this._Size, this._Unit, g.PageUnit);
            }
            else
            {
                return info.GetFontHeight(this._Size, g.PageUnit);
            }
        }
        public float GetHeight(GraphicsUnit unit)
        {
            var info = DCSoft.TrueTypeFontSnapshort.GetInstance(this._Name, this._Style);
            if (info == null)
            {
                return GraphicsUnitConvert.Convert(this._Size, this._Unit, unit);
            }
            else
            {
                return info.GetFontHeight(this._Size, unit);
            }
        }
        private bool HasStyle(FontStyle flag)
        {
            CheckDisposed();
            return (this._Style & flag) == flag;
        }
        private bool _Disposed  ;
        public void Dispose()
        {
            this._Name = null;
            this._Disposed = true;
        }
        private void CheckDisposed()
        {
            if (this._Disposed)
            {
                throw new ObjectDisposedException(" Font");
            }
        }
    }
    //public static class SystemBrushes
    //{
    //    //public static readonly Brush ActiveBorder = new SolidBrush( SystemColors.ActiveBorder, true);
    //    //public static readonly Brush ActiveCaption = new SolidBrush( SystemColors.ActiveCaption, true);
    //    //public static readonly Brush ActiveCaptionText = new SolidBrush( SystemColors.ActiveCaptionText, true);
    //    //public static readonly Brush AppWorkspace = new SolidBrush( SystemColors.AppWorkspace, true);
    //    //public static readonly Brush ButtonFace = new SolidBrush( SystemColors.ButtonFace, true);
    //    //public static readonly Brush ButtonHighlight = new SolidBrush( SystemColors.ButtonHighlight, true);
    //    //public static readonly Brush ButtonShadow = new SolidBrush( SystemColors.ButtonShadow, true);
    //    //public static readonly Brush Control = new SolidBrush( SystemColors.Control, true);
    //    //public static readonly Brush ControlLightLight = new SolidBrush( SystemColors.ControlLightLight, true);
    //    public static readonly Brush ControlLight = new SolidBrush( SystemColors.ControlLight, true);
    //    //public static readonly Brush ControlDark = new SolidBrush( SystemColors.ControlDark, true);
    //    //public static readonly Brush ControlDarkDark = new SolidBrush( SystemColors.ControlDarkDark, true);
    //    public static readonly Brush ControlText = new SolidBrush( SystemColors.ControlText, true);
    //    //public static readonly Brush Desktop = new SolidBrush( SystemColors.Desktop, true);
    //    //public static readonly Brush GradientActiveCaption = new SolidBrush( SystemColors.GradientActiveCaption, true);
    //    //public static readonly Brush GradientInactiveCaption = new SolidBrush( SystemColors.GradientInactiveCaption, true);
    //    //public static readonly Brush GrayText = new SolidBrush( SystemColors.GrayText, true);
    //    public static readonly Brush Highlight = new SolidBrush( SystemColors.Highlight, true);
    //    //public static readonly Brush HighlightText = new SolidBrush( SystemColors.HighlightText, true);
    //    //public static readonly Brush HotTrack = new SolidBrush( SystemColors.HotTrack, true);
    //    //public static readonly Brush InactiveCaption = new SolidBrush( SystemColors.InactiveCaption, true);
    //    //public static readonly Brush InactiveBorder = new SolidBrush( SystemColors.InactiveBorder, true);
    //    //public static readonly Brush InactiveCaptionText = new SolidBrush( SystemColors.InactiveCaptionText, true);
    //    //public static readonly Brush Info = new SolidBrush( SystemColors.Info, true);
    //    //public static readonly Brush InfoText = new SolidBrush( SystemColors.InfoText, true);
    //    //public static readonly Brush Menu = new SolidBrush( SystemColors.Menu, true);
    //    //public static readonly Brush MenuBar = new SolidBrush( SystemColors.MenuBar, true);
    //    //public static readonly Brush MenuHighlight = new SolidBrush( SystemColors.MenuHighlight, true);
    //    //public static readonly Brush MenuText = new SolidBrush( SystemColors.MenuText, true);
    //    //public static readonly Brush ScrollBar = new SolidBrush( SystemColors.ScrollBar, true);
    //    public static readonly Brush Window = new SolidBrush( SystemColors.Window, true);
    //    //public static readonly Brush WindowFrame = new SolidBrush( SystemColors.WindowFrame, true);
    //    //public static readonly Brush WindowText = new SolidBrush( SystemColors.WindowText, true);
    //}
    public static class Brushes
    {
        internal static readonly int ARGB_White = DCSystem_Drawing.Color.White.ToArgb();
        internal static readonly int ARGB_Black = DCSystem_Drawing.Color.Black.ToArgb();
        internal static readonly int ARGB_AliceBlue = DCSystem_Drawing.Color.AliceBlue.ToArgb();
        internal static readonly int ARGB_Red = DCSystem_Drawing.Color.Red.ToArgb();
        internal static readonly int ARGB_ControlText = DCSystem_Drawing.SystemColors.ControlText.ToArgb();
        internal static readonly int ARGB_Gray = DCSystem_Drawing.Color.Gray.ToArgb();


        /// <summary>
        /// 获得指定颜色的纯色画刷对象
        /// </summary>
        /// <param name="color">指定的颜色</param>
        /// <returns>画刷对象</returns>
        public static SolidBrush GetValue(DCSystem_Drawing.Color color)
        {
            var argb = color.ToArgb();
            if (argb == ARGB_Black)
            {
                return (SolidBrush)Black;
            }
            if (argb == ARGB_White)
            {
                return (SolidBrush)White;
            }
            if (argb == ARGB_AliceBlue)
            {
                return (SolidBrush)AliceBlue;
            }
            if (argb == ARGB_Red)
            {
                return (SolidBrush)Red;
            }
            if (argb == ARGB_ControlText)
            {
                return (SolidBrush)ControlText;
            }
            if (argb == ARGB_Gray)
            {
                return (SolidBrush)Gray;
            }
            if (_brushes == null)
            {
                _brushes = new Dictionary<int, DCSystem_Drawing.SolidBrush>();
            }
            //if (_GetSolibBrushCounter.ContainsKey(color) == false)
            //{
            //    _GetSolibBrushCounter[color] = 1;
            //}
            //else
            //{
            //    _GetSolibBrushCounter[color]++;
            //}
            //if (color == Color.Gray)
            //{

            //}
            DCSystem_Drawing.SolidBrush b = null;
            if (_brushes.TryGetValue(argb, out b) == false)
            {
                b = new DCSystem_Drawing.SolidBrush(color);
                _brushes[argb] = b;
            }
            return b;

            //if (_brushes.ContainsKey(color))
            //{
            //    return _brushes[color];
            //}
            //else
            //{
            //    SolidBrush b = new SolidBrush(color);
            //    _brushes[color] = b;
            //    return b;
            //}
        }
        private static Dictionary<int, SolidBrush> _brushes = new Dictionary<int, SolidBrush>();

        public static readonly Brush ControlText = new SolidBrush(SystemColors.ControlText, true);
        //public static readonly Brush Transparent = new SolidBrush( Color.Transparent, true);
        public static readonly Brush AliceBlue = new SolidBrush( Color.AliceBlue, true);
        public static readonly Brush Blue = new SolidBrush( Color.Blue, true);
        //public static readonly Brush AntiqueWhite = new SolidBrush( Color.AntiqueWhite, true);
        //public static readonly Brush Aqua = new SolidBrush( Color.Aqua, true);
        //public static readonly Brush Aquamarine = new SolidBrush( Color.Aquamarine, true);
        //public static readonly Brush Azure = new SolidBrush( Color.Azure, true);
        //public static readonly Brush Beige = new SolidBrush( Color.Beige, true);
        //public static readonly Brush Bisque = new SolidBrush( Color.Bisque, true);
        public static readonly Brush Black = new SolidBrush( Color.Black, true);
        //public static readonly Brush BlanchedAlmond = new SolidBrush( Color.BlanchedAlmond, true);
        //public static readonly Brush Blue = new SolidBrush( Color.Blue, true);
        //public static readonly Brush BlueViolet = new SolidBrush( Color.BlueViolet, true);
        //public static readonly Brush Brown = new SolidBrush( Color.Brown, true);
        //public static readonly Brush BurlyWood = new SolidBrush( Color.BurlyWood, true);
        //public static readonly Brush CadetBlue = new SolidBrush( Color.CadetBlue, true);
        //public static readonly Brush Chartreuse = new SolidBrush( Color.Chartreuse, true);
        //public static readonly Brush Chocolate = new SolidBrush( Color.Chocolate, true);
        //public static readonly Brush Coral = new SolidBrush( Color.Coral, true);
        //public static readonly Brush CornflowerBlue = new SolidBrush( Color.CornflowerBlue, true);
        //public static readonly Brush Cornsilk = new SolidBrush( Color.Cornsilk, true);
        //public static readonly Brush Crimson = new SolidBrush( Color.Crimson, true);
        //public static readonly Brush Cyan = new SolidBrush( Color.Cyan, true);
        //public static readonly Brush DarkBlue = new SolidBrush( Color.DarkBlue, true);
        //public static readonly Brush DarkCyan = new SolidBrush( Color.DarkCyan, true);
        //public static readonly Brush DarkGoldenrod = new SolidBrush( Color.DarkGoldenrod, true);
        //public static readonly Brush DarkGray = new SolidBrush( Color.DarkGray, true);
        //public static readonly Brush DarkGreen = new SolidBrush( Color.DarkGreen, true);
        //public static readonly Brush DarkKhaki = new SolidBrush( Color.DarkKhaki, true);
        //public static readonly Brush DarkMagenta = new SolidBrush( Color.DarkMagenta, true);
        //public static readonly Brush DarkOliveGreen = new SolidBrush( Color.DarkOliveGreen, true);
        //public static readonly Brush DarkOrange = new SolidBrush( Color.DarkOrange, true);
        //public static readonly Brush DarkOrchid = new SolidBrush( Color.DarkOrchid, true);
        //public static readonly Brush DarkRed = new SolidBrush( Color.DarkRed, true);
        //public static readonly Brush DarkSalmon = new SolidBrush( Color.DarkSalmon, true);
        //public static readonly Brush DarkSeaGreen = new SolidBrush( Color.DarkSeaGreen, true);
        //public static readonly Brush DarkSlateBlue = new SolidBrush( Color.DarkSlateBlue, true);
        //public static readonly Brush DarkSlateGray = new SolidBrush( Color.DarkSlateGray, true);
        //public static readonly Brush DarkTurquoise = new SolidBrush( Color.DarkTurquoise, true);
        //public static readonly Brush DarkViolet = new SolidBrush( Color.DarkViolet, true);
        //public static readonly Brush DeepPink = new SolidBrush( Color.DeepPink, true);
        //public static readonly Brush DeepSkyBlue = new SolidBrush( Color.DeepSkyBlue, true);
        //public static readonly Brush DimGray = new SolidBrush( Color.DimGray, true);
        //public static readonly Brush DodgerBlue = new SolidBrush( Color.DodgerBlue, true);
        //public static readonly Brush Firebrick = new SolidBrush( Color.Firebrick, true);
        //public static readonly Brush FloralWhite = new SolidBrush( Color.FloralWhite, true);
        //public static readonly Brush ForestGreen = new SolidBrush( Color.ForestGreen, true);
        //public static readonly Brush Fuchsia = new SolidBrush( Color.Fuchsia, true);
        //public static readonly Brush Gainsboro = new SolidBrush( Color.Gainsboro, true);
        //public static readonly Brush GhostWhite = new SolidBrush( Color.GhostWhite, true);
        //public static readonly Brush Gold = new SolidBrush( Color.Gold, true);
        //public static readonly Brush Goldenrod = new SolidBrush( Color.Goldenrod, true);
        public static readonly Brush Gray = new SolidBrush( Color.Gray, true);
        //public static readonly Brush Green = new SolidBrush( Color.Green, true);
        //public static readonly Brush GreenYellow = new SolidBrush( Color.GreenYellow, true);
        //public static readonly Brush Honeydew = new SolidBrush( Color.Honeydew, true);
        //public static readonly Brush HotPink = new SolidBrush( Color.HotPink, true);
        //public static readonly Brush IndianRed = new SolidBrush( Color.IndianRed, true);
        //public static readonly Brush Indigo = new SolidBrush( Color.Indigo, true);
        //public static readonly Brush Ivory = new SolidBrush( Color.Ivory, true);
        //public static readonly Brush Khaki = new SolidBrush( Color.Khaki, true);
        //public static readonly Brush Lavender = new SolidBrush( Color.Lavender, true);
        //public static readonly Brush LavenderBlush = new SolidBrush( Color.LavenderBlush, true);
        //public static readonly Brush LawnGreen = new SolidBrush( Color.LawnGreen, true);
        //public static readonly Brush LemonChiffon = new SolidBrush( Color.LemonChiffon, true);
        //public static readonly Brush LightBlue = new SolidBrush( Color.LightBlue, true);
        //public static readonly Brush LightCoral = new SolidBrush( Color.LightCoral, true);
        //public static readonly Brush LightCyan = new SolidBrush( Color.LightCyan, true);
        //public static readonly Brush LightGoldenrodYellow = new SolidBrush( Color.LightGoldenrodYellow, true);
        //public static readonly Brush LightGreen = new SolidBrush( Color.LightGreen, true);
        public static readonly Brush LightGray = new SolidBrush( Color.LightGray, true);
        //public static readonly Brush LightPink = new SolidBrush( Color.LightPink, true);
        //public static readonly Brush LightSalmon = new SolidBrush( Color.LightSalmon, true);
        //public static readonly Brush LightSeaGreen = new SolidBrush( Color.LightSeaGreen, true);
        //public static readonly Brush LightSkyBlue = new SolidBrush( Color.LightSkyBlue, true);
        //public static readonly Brush LightSlateGray = new SolidBrush( Color.LightSlateGray, true);
        //public static readonly Brush LightSteelBlue = new SolidBrush( Color.LightSteelBlue, true);
        //public static readonly Brush LightYellow = new SolidBrush( Color.LightYellow, true);
        //public static readonly Brush Lime = new SolidBrush( Color.Lime, true);
        //public static readonly Brush LimeGreen = new SolidBrush( Color.LimeGreen, true);
        //public static readonly Brush Linen = new SolidBrush( Color.Linen, true);
        //public static readonly Brush Magenta = new SolidBrush( Color.Magenta, true);
        //public static readonly Brush Maroon = new SolidBrush( Color.Maroon, true);
        //public static readonly Brush MediumAquamarine = new SolidBrush( Color.MediumAquamarine, true);
        //public static readonly Brush MediumBlue = new SolidBrush( Color.MediumBlue, true);
        //public static readonly Brush MediumOrchid = new SolidBrush( Color.MediumOrchid, true);
        //public static readonly Brush MediumPurple = new SolidBrush( Color.MediumPurple, true);
        //public static readonly Brush MediumSeaGreen = new SolidBrush( Color.MediumSeaGreen, true);
        //public static readonly Brush MediumSlateBlue = new SolidBrush( Color.MediumSlateBlue, true);
        //public static readonly Brush MediumSpringGreen = new SolidBrush( Color.MediumSpringGreen, true);
        //public static readonly Brush MediumTurquoise = new SolidBrush( Color.MediumTurquoise, true);
        //public static readonly Brush MediumVioletRed = new SolidBrush( Color.MediumVioletRed, true);
        //public static readonly Brush MidnightBlue = new SolidBrush( Color.MidnightBlue, true);
        //public static readonly Brush MintCream = new SolidBrush( Color.MintCream, true);
        //public static readonly Brush MistyRose = new SolidBrush( Color.MistyRose, true);
        //public static readonly Brush Moccasin = new SolidBrush( Color.Moccasin, true);
        //public static readonly Brush NavajoWhite = new SolidBrush( Color.NavajoWhite, true);
        //public static readonly Brush Navy = new SolidBrush( Color.Navy, true);
        //public static readonly Brush OldLace = new SolidBrush( Color.OldLace, true);
        //public static readonly Brush Olive = new SolidBrush( Color.Olive, true);
        //public static readonly Brush OliveDrab = new SolidBrush( Color.OliveDrab, true);
        //public static readonly Brush Orange = new SolidBrush( Color.Orange, true);
        //public static readonly Brush OrangeRed = new SolidBrush( Color.OrangeRed, true);
        //public static readonly Brush Orchid = new SolidBrush( Color.Orchid, true);
        //public static readonly Brush PaleGoldenrod = new SolidBrush( Color.PaleGoldenrod, true);
        //public static readonly Brush PaleGreen = new SolidBrush( Color.PaleGreen, true);
        //public static readonly Brush PaleTurquoise = new SolidBrush( Color.PaleTurquoise, true);
        //public static readonly Brush PaleVioletRed = new SolidBrush( Color.PaleVioletRed, true);
        //public static readonly Brush PapayaWhip = new SolidBrush( Color.PapayaWhip, true);
        //public static readonly Brush PeachPuff = new SolidBrush( Color.PeachPuff, true);
        //public static readonly Brush Peru = new SolidBrush( Color.Peru, true);
        //public static readonly Brush Pink = new SolidBrush( Color.Pink, true);
        //public static readonly Brush Plum = new SolidBrush( Color.Plum, true);
        //public static readonly Brush PowderBlue = new SolidBrush( Color.PowderBlue, true);
        //public static readonly Brush Purple = new SolidBrush( Color.Purple, true);
        public static readonly Brush Red = new SolidBrush( Color.Red, true);
        //public static readonly Brush RosyBrown = new SolidBrush( Color.RosyBrown, true);
        //public static readonly Brush RoyalBlue = new SolidBrush( Color.RoyalBlue, true);
        //public static readonly Brush SaddleBrown = new SolidBrush( Color.SaddleBrown, true);
        //public static readonly Brush Salmon = new SolidBrush( Color.Salmon, true);
        //public static readonly Brush SandyBrown = new SolidBrush( Color.SandyBrown, true);
        //public static readonly Brush SeaGreen = new SolidBrush( Color.SeaGreen, true);
        //public static readonly Brush SeaShell = new SolidBrush( Color.SeaShell, true);
        //public static readonly Brush Sienna = new SolidBrush( Color.Sienna, true);
        //public static readonly Brush Silver = new SolidBrush( Color.Silver, true);
        //public static readonly Brush SkyBlue = new SolidBrush( Color.SkyBlue, true);
        //public static readonly Brush SlateBlue = new SolidBrush( Color.SlateBlue, true);
        //public static readonly Brush SlateGray = new SolidBrush( Color.SlateGray, true);
        //public static readonly Brush Snow = new SolidBrush( Color.Snow, true);
        //public static readonly Brush SpringGreen = new SolidBrush( Color.SpringGreen, true);
        //public static readonly Brush SteelBlue = new SolidBrush( Color.SteelBlue, true);
        //public static readonly Brush Tan = new SolidBrush( Color.Tan, true);
        //public static readonly Brush Teal = new SolidBrush( Color.Teal, true);
        //public static readonly Brush Thistle = new SolidBrush( Color.Thistle, true);
        //public static readonly Brush Tomato = new SolidBrush( Color.Tomato, true);
        //public static readonly Brush Turquoise = new SolidBrush( Color.Turquoise, true);
        //public static readonly Brush Violet = new SolidBrush( Color.Violet, true);
        //public static readonly Brush Wheat = new SolidBrush( Color.Wheat, true);
        public static readonly Brush White = new SolidBrush( Color.White, true);
        //public static readonly Brush WhiteSmoke = new SolidBrush( Color.WhiteSmoke, true);
        //public static readonly Brush Yellow = new SolidBrush( Color.Yellow, true);
        //public static readonly Brush YellowGreen = new SolidBrush( Color.YellowGreen, true);
    }
    public sealed class SolidBrush : Brush
    {
        //private  Color _Color ;
        public SolidBrush(DCSystem_Drawing.Color c)
        {
            this.Color = c;
            this.ColorValue = c._value;
        }
        internal SolidBrush(DCSystem_Drawing.Color c, bool immutable)
        {
            this.Color = c;
            this.ColorValue = c._value;
            this._immutable = immutable;
        }
        private readonly  bool _immutable ;
        public readonly DCSystem_Drawing.Color Color;
        public readonly uint ColorValue;

        //public  Color Color
        //{
        //    get
        //    {
        //        return this._Color;
        //    }
        //    //set
        //    //{
        //    //    if (this._immutable)
        //    //    {
        //    //        throw new InvalidOperationException("对象是永恒的");
        //    //    }
        //    //    this._Color = value;
        //    //}
        //}
        public override void Dispose()
        {
            if (this._immutable)
            {
                throw new InvalidOperationException("对象是永恒的");
            }
            base.Dispose();
        }
        public override bool EqualsValue(Brush b)
        {
            if (b is SolidBrush)
            {
                return ((SolidBrush)b).ColorValue == this.ColorValue;
            }
            return false;
        }
        public override bool EqualsValue(DCSystem_Drawing.Color c)
        {
            return this.Color == c;//.ToArgb() == c.ToArgb();
        }
    }

    public abstract class Brush : IDisposable
    {
        public virtual void Dispose()
        {
            //this._Disposed = true;
        }
        public virtual bool EqualsValue(Brush b)
        {
            return false;
        }
        public virtual bool EqualsValue( Color c)
        {
            return false;
        }
    }
    //[System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
    public abstract class Image : IDisposable
    {
        public Image()
        {

        }        

        private int _StandardImageIndex = int.MinValue;
        /// <summary>
        /// 标准图标序号
        /// </summary>
        public int StandardImageIndex
        {
            get
            {
                return this._StandardImageIndex;
            }
            internal set
            {
                this._StandardImageIndex = value;
            }
        }
        protected int _Width ;
        public int Width
        {
            get
            {
                CheckDispose();
                return this._Width;
            }
        }
        protected int _Height ;
        public int Height
        {
            get
            {
                CheckDispose();
                return this._Height;
            }
        }
        public  Size Size
        {
            get
            {
                return new  Size(this._Width, this._Height);
            }
        }
        private bool _Disposed  ;
        public virtual void Dispose()
        {
            _Disposed = true;
        }
        internal void CheckDispose()
        {
            if (this._Disposed)
            {
                throw new ObjectDisposedException(this.GetType().Name);
            }
        }
        public static Image FromStream(System.IO.Stream stream)
        {
            return null;
        }
        public virtual object Clone()
        {
            return this.MemberwiseClone();
        }
        public virtual byte[] ToBinary()
        {
            return null;
        }
    }

    [System.Text.Json.Serialization.JsonConverter(typeof(System.Text.Json.Serialization.JsonStringEnumConverter))]
    [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
    public enum ContentAlignment
    {
        TopLeft = 1,
        TopCenter = 2,
        TopRight = 4,
        MiddleLeft = 0x10,
        MiddleCenter = 0x20,
        MiddleRight = 0x40,
        BottomLeft = 0x100,
        BottomCenter = 0x200,
        BottomRight = 0x400
    }
    public sealed class Bitmap : Image
    {
        /// <summary>
        /// 判断是否为支持的文件格式
        /// </summary>
        /// <param name="bs"></param>
        /// <returns></returns>
        public static bool IsSupportData(byte[] bs)
        {
            if (bs != null && bs.Length > 0)
            {
                return FileHeaderHelper.HasJpegHeader(bs)
                    || FileHeaderHelper.HasPNGHeader(bs)
                    || FileHeaderHelper.HasBMPHeader(bs)
                    || FileHeaderHelper.HasGIFHeader(bs);
            }
            else
            {
                return false;
            }
        }
        public Bitmap(byte[] bs)
        {
            if (bs == null || bs.Length == 0)
            {
                throw new ArgumentNullException("bs");
            }
            CheckImageFormatSupported(bs);
            this.Data = bs;
        }
        public Bitmap(int w, int h)
        {
            this._Width = w;
            this._Height = h;
        }
       
        private static void CheckImageFormatSupported(byte[] bs )
        {
            if( bs == null || bs.Length < 5 )
            {
                throw new NotSupportedException("图片数据太少");
            }
            if( IsSupportData( bs ) == false )
            {
                //throw new NotSupportedException("只支持PNG/BMP/JPG/GIF图片格式");
            }
        }
        private byte[] _Data ;
        public byte[] Data
        {
            get
            {
                base.CheckDispose();
                return this._Data;
            }
            set
            {
                this._Data = value;
                if (this._Data == null || this._Data.Length == 0 )
                {
                    base._Width = 0;
                    base._Height = 0;
                }
                else
                {
                    var size = DCSoft.Drawing.XImageValue.GetImageSizeFromBinary(this._Data);
                    base._Width = size.Width;
                    base._Height = size.Height;
                    if(this._Width == 0 || this._Height ==0)
                    {
                        throw new NotSupportedException("image format");
                    }
                }
            }
        }
        public string ToDataUrl()
        {
            if (this._Data == null || this._Data.Length == 0)
            {
                return null;
            }
            else
            {
                return XImageValue.StaticGetEmitImageSource(this._Data);
            }
        }
        public const byte GraphicsImageHeader = 133;

        private byte[] _DataFromGraphics = null;
        /// <summary>
        /// 源自画布的数据
        /// </summary>
        public byte[] DataFromGraphics
        {
            get
            {
                return this._DataFromGraphics;
            }
            set
            {
                this._DataFromGraphics = value;
            }
        }
        public override void Dispose()
        {
            base.Dispose();
            this._Data = null;
            this._DataFromGraphics = null;
        }
        public override byte[] ToBinary()
        {
            return this._Data;
        }
    }
} 
