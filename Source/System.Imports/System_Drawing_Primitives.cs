// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Generic;
using System.Globalization;

namespace DCSystem_Drawing
{
#if ! RELEASE
    [System.Diagnostics.DebuggerDisplay("{NameAndARGBValue}")]
#endif
    [System.Reflection.Obfuscation( Exclude = true , ApplyToMembers = true )]
    public readonly struct Color : IEquatable<Color>
    {
#if ! RELEASE
        private string NameAndARGBValue => this._value.ToString("X8");// "A" + this.A + "" $"{ARGB=({A}, {R}, {G}, {B})}";
#endif
        public static readonly Color Empty = new Color(0);
        public static readonly Color Transparent = new Color(0x00FFFFFF);
        public static readonly Color AliceBlue = new Color(0xFFF0F8FF);
        //public static readonly Color AntiqueWhite = new Color(0xFFFAEBD7);
        //public static readonly Color Aqua = new Color(0xFF00FFFF);
        //public static readonly Color Aquamarine = new Color(0xFF7FFFD4);
        //public static readonly Color Azure = new Color(0xFFF0FFFF);
        //public static readonly Color Beige = new Color(0xFFF5F5DC);
        //public static readonly Color Bisque = new Color(0xFFFFE4C4);
        public static readonly Color Black = new Color(0xFF000000);
        //public static readonly Color BlanchedAlmond = new Color(0xFFFFEBCD);
        public static readonly Color Blue = new Color(0xFF0000FF);
        //public static readonly Color BlueViolet = new Color(0xFF8A2BE2);
        //public static readonly Color Brown = new Color(0xFFA52A2A);
        //public static readonly Color BurlyWood = new Color(0xFFDEB887);
        //public static readonly Color CadetBlue = new Color(0xFF5F9EA0);
        //public static readonly Color Chartreuse = new Color(0xFF7FFF00);
        //public static readonly Color Chocolate = new Color(0xFFD2691E);
        public static readonly Color Coral = new Color(0xFFFF7F50);
        //public static readonly Color CornflowerBlue = new Color(0xFF6495ED);
        //public static readonly Color Cornsilk = new Color(0xFFFFF8DC);
        //public static readonly Color Crimson = new Color(0xFFDC143C);
        //public static readonly Color Cyan = new Color(0xFF00FFFF);
        public static readonly Color DarkBlue = new Color(0xFF00008B);
        //public static readonly Color DarkCyan = new Color(0xFF008B8B);
        //public static readonly Color DarkGoldenrod = new Color(0xFFB8860B);
        public static readonly Color DarkGray = new Color(0xFFA9A9A9);
        //public static readonly Color DarkGreen = new Color(0xFF006400);
        //public static readonly Color DarkKhaki = new Color(0xFFBDB76B);
        //public static readonly Color DarkMagenta = new Color(0xFF8B008B);
        //public static readonly Color DarkOliveGreen = new Color(0xFF556B2F);
        //public static readonly Color DarkOrange = new Color(0xFFFF8C00);
        //public static readonly Color DarkOrchid = new Color(0xFF9932CC);
        //public static readonly Color DarkRed = new Color(0xFF8B0000);
        //public static readonly Color DarkSalmon = new Color(0xFFE9967A);
        //public static readonly Color DarkSeaGreen = new Color(0xFF8FBC8B);
        //public static readonly Color DarkSlateBlue = new Color(0xFF483D8B);
        //public static readonly Color DarkSlateGray = new Color(0xFF2F4F4F);
        //public static readonly Color DarkTurquoise = new Color(0xFF00CED1);
        //public static readonly Color DarkViolet = new Color(0xFF9400D3);
        //public static readonly Color DeepPink = new Color(0xFFFF1493);
        //public static readonly Color DeepSkyBlue = new Color(0xFF00BFFF);
        //public static readonly Color DimGray = new Color(0xFF696969);
        //public static readonly Color DodgerBlue = new Color(0xFF1E90FF);
        //public static readonly Color Firebrick = new Color(0xFFB22222);
        //public static readonly Color FloralWhite = new Color(0xFFFFFAF0);
        //public static readonly Color ForestGreen = new Color(0xFF228B22);
        //public static readonly Color Fuchsia = new Color(0xFFFF00FF);
        //public static readonly Color Gainsboro = new Color(0xFFDCDCDC);
        //public static readonly Color GhostWhite = new Color(0xFFF8F8FF);
        //public static readonly Color Gold = new Color(0xFFFFD700);
        //public static readonly Color Goldenrod = new Color(0xFFDAA520);
        public static readonly Color Gray = new Color(0xFF808080);
        public static readonly Color Green = new Color(0xFF008000);
        //public static readonly Color GreenYellow = new Color(0xFFADFF2F);
        //public static readonly Color Honeydew = new Color(0xFFF0FFF0);
        //public static readonly Color HotPink = new Color(0xFFFF69B4);
        //public static readonly Color IndianRed = new Color(0xFFCD5C5C);
        //public static readonly Color Indigo = new Color(0xFF4B0082);
        //public static readonly Color Ivory = new Color(0xFFFFFFF0);
        //public static readonly Color Khaki = new Color(0xFFF0E68C);
        //public static readonly Color Lavender = new Color(0xFFE6E6FA);
        //public static readonly Color LavenderBlush = new Color(0xFFFFF0F5);
        //public static readonly Color LawnGreen = new Color(0xFF7CFC00);
        //public static readonly Color LemonChiffon = new Color(0xFFFFFACD);
        public static readonly Color LightBlue = new Color(0xFFADD8E6);
        public static readonly Color LightCoral = new Color(0xFFF08080);
        //public static readonly Color LightCyan = new Color(0xFFE0FFFF);
        //public static readonly Color LightGoldenrodYellow = new Color(0xFFFAFAD2);
        //public static readonly Color LightGreen = new Color(0xFF90EE90);
        public static readonly Color LightGray = new Color(0xFFD3D3D3);
        //public static readonly Color LightPink = new Color(0xFFFFB6C1);
        //public static readonly Color LightSalmon = new Color(0xFFFFA07A);
        //public static readonly Color LightSeaGreen = new Color(0xFF20B2AA);
        //public static readonly Color LightSkyBlue = new Color(0xFF87CEFA);
        //public static readonly Color LightSlateGray = new Color(0xFF778899);
        //public static readonly Color LightSteelBlue = new Color(0xFFB0C4DE);
        public static readonly Color LightYellow = new Color(0xFFFFFFE0);
        //public static readonly Color Lime = new Color(0xFF00FF00);
        //public static readonly Color LimeGreen = new Color(0xFF32CD32);
        //public static readonly Color Linen = new Color(0xFFFAF0E6);
        //public static readonly Color Magenta = new Color(0xFFFF00FF);
        //public static readonly Color Maroon = new Color(0xFF800000);
        //public static readonly Color MediumAquamarine = new Color(0xFF66CDAA);
        //public static readonly Color MediumBlue = new Color(0xFF0000CD);
        //public static readonly Color MediumOrchid = new Color(0xFFBA55D3);
        //public static readonly Color MediumPurple = new Color(0xFF9370DB);
        //public static readonly Color MediumSeaGreen = new Color(0xFF3CB371);
        //public static readonly Color MediumSlateBlue = new Color(0xFF7B68EE);
        //public static readonly Color MediumSpringGreen = new Color(0xFF00FA9A);
        //public static readonly Color MediumTurquoise = new Color(0xFF48D1CC);
        //public static readonly Color MediumVioletRed = new Color(0xFFC71585);
        //public static readonly Color MidnightBlue = new Color(0xFF191970);
        //public static readonly Color MintCream = new Color(0xFFF5FFFA);
        //public static readonly Color MistyRose = new Color(0xFFFFE4E1);
        //public static readonly Color Moccasin = new Color(0xFFFFE4B5);
        //public static readonly Color NavajoWhite = new Color(0xFFFFDEAD);
        //public static readonly Color Navy = new Color(0xFF000080);
        //public static readonly Color OldLace = new Color(0xFFFDF5E6);
        //public static readonly Color Olive = new Color(0xFF808000);
        //public static readonly Color OliveDrab = new Color(0xFF6B8E23);
        //public static readonly Color Orange = new Color(0xFFFFA500);
        //public static readonly Color OrangeRed = new Color(0xFFFF4500);
        //public static readonly Color Orchid = new Color(0xFFDA70D6);
        //public static readonly Color PaleGoldenrod = new Color(0xFFEEE8AA);
        //public static readonly Color PaleGreen = new Color(0xFF98FB98);
        //public static readonly Color PaleTurquoise = new Color(0xFFAFEEEE);
        //public static readonly Color PaleVioletRed = new Color(0xFFDB7093);
        //public static readonly Color PapayaWhip = new Color(0xFFFFEFD5);
        //public static readonly Color PeachPuff = new Color(0xFFFFDAB9);
        //public static readonly Color Peru = new Color(0xFFCD853F);
        //public static readonly Color Pink = new Color(0xFFFFC0CB);
        //public static readonly Color Plum = new Color(0xFFDDA0DD);
        //public static readonly Color PowderBlue = new Color(0xFFB0E0E6);
        //public static readonly Color Purple = new Color(0xFF800080);
        public static readonly Color Red = new Color(0xFFFF0000);
        //public static readonly Color RebeccaPurple = new Color(0xFF663399);
        //public static readonly Color RosyBrown = new Color(0xFFBC8F8F);
        //public static readonly Color RoyalBlue = new Color(0xFF4169E1);
        //public static readonly Color SaddleBrown = new Color(0xFF8B4513);
        //public static readonly Color Salmon = new Color(0xFFFA8072);
        //public static readonly Color SandyBrown = new Color(0xFFF4A460);
        //public static readonly Color SeaGreen = new Color(0xFF2E8B57);
        //public static readonly Color SeaShell = new Color(0xFFFFF5EE);
        //public static readonly Color Sienna = new Color(0xFFA0522D);
        //public static readonly Color Silver = new Color(0xFFC0C0C0);
        //public static readonly Color SkyBlue = new Color(0xFF87CEEB);
        //public static readonly Color SlateBlue = new Color(0xFF6A5ACD);
        //public static readonly Color SlateGray = new Color(0xFF708090);
        //public static readonly Color Snow = new Color(0xFFFFFAFA);
        //public static readonly Color SpringGreen = new Color(0xFF00FF7F);
        //public static readonly Color SteelBlue = new Color(0xFF4682B4);
        //public static readonly Color Tan = new Color(0xFFD2B48C);
        //public static readonly Color Teal = new Color(0xFF008080);
        //public static readonly Color Thistle = new Color(0xFFD8BFD8);
        //public static readonly Color Tomato = new Color(0xFFFF6347);
        //public static readonly Color Turquoise = new Color(0xFF40E0D0);
        //public static readonly Color Violet = new Color(0xFFEE82EE);
        //public static readonly Color Wheat = new Color(0xFFF5DEB3);
        public static readonly Color White = new Color(0xFFFFFFFF);
        //public static readonly Color WhiteSmoke = new Color(0xFFF5F5F5);
        public static readonly Color Yellow = new Color(0xFFFFFF00);
        //public static readonly Color YellowGreen = new Color(0xFF9ACD32);



        internal const int ARGBAlphaShift = 24;
        internal const int ARGBRedShift = 16;
        internal const int ARGBGreenShift = 8;
        internal const int ARGBBlueShift = 0;
        internal const uint ARGBAlphaMask = 0xFFu << ARGBAlphaShift;
        internal readonly uint _value; // Do not rename (binary serialization)
        internal Color ( uint v )
        {
            this._value = v;
        }
        public string ToHtmlString()
        {
            return ColorTranslator.ToHtml(this);
        }
        public bool IsEmpty
        {
            get
            {
                return this._value == 0 ;
            }
        }

        public byte R => unchecked((byte)(this._value >> ARGBRedShift));

        public byte G => unchecked((byte)(this._value >> ARGBGreenShift));

        public byte B => unchecked((byte)(this._value >> ARGBBlueShift));

        public byte A => unchecked((byte)(this._value >> ARGBAlphaShift));

        private static void CheckByte(int value, string name)
        {
            static void ThrowOutOfByteRange(int v, string n) =>
                throw new ArgumentException("InvalidEx2BoundArgument");

            if (unchecked((uint)value) > byte.MaxValue)
                ThrowOutOfByteRange(value, name);
        }

        public static Color FromArgb(int argb) => new Color((uint)argb);

        public static Color FromArgb(int alpha, int red, int green, int blue)
        {
            CheckByte(alpha, nameof(alpha));
            CheckByte(red, nameof(red));
            CheckByte(green, nameof(green));
            CheckByte(blue, nameof(blue));

            return FromArgb(
                (int)alpha << ARGBAlphaShift |
                (int)red << ARGBRedShift |
                (int)green << ARGBGreenShift |
                (int)blue << ARGBBlueShift
            );
        }

        public static Color FromArgb(int alpha, Color baseColor)
        {
            CheckByte(alpha, nameof(alpha));

            return FromArgb(
                (int)(alpha << ARGBAlphaShift )|
                (int)(baseColor._value & ~ARGBAlphaMask)
            );
        }

        public static Color FromArgb(int red, int green, int blue) => FromArgb(byte.MaxValue, red, green, blue);

        public int ToArgb() => unchecked((int)this._value);
#if !RELEASE
        public override string ToString() =>
             $"{nameof(Color)} [A={A}, R={R}, G={G}, B={B}]" ;
#endif
        public static bool operator ==(Color left, Color right) =>
            left._value == right._value;

        public static bool operator !=(Color left, Color right) => left._value != right._value;

        public override bool Equals(object obj) => obj is Color other && Equals(other);

        public bool Equals(Color other) => this._value == other._value;

        public override int GetHashCode()
        {
            return (int)this._value;

            //// Three cases:
            //// 1. We don't have a name. All relevant data, including this fact, is in the remaining fields.
            //// 2. We have a known name. The name will be the same instance of any other with the same
            //// knownColor value, so we can ignore it for hashing. Note this also hashes different to
            //// an unnamed color with the same ARGB value.
            //// 3. Have an unknown name. Will differ from other unknown-named colors only by name, so we
            //// can usefully use the names hash code alone.
            //if (name != null && !IsKnownColor)
            //    return name.GetHashCode();

            //return HashCode.Combine(_value.GetHashCode(), state.GetHashCode(), knownColor.GetHashCode());
        }
    }

    internal static class ColorConverterCommon
    {
        public static Color ConvertFromString(string strValue, CultureInfo culture)
        {

            string text = strValue.Trim();

            if (text.Length == 0)
            {
                return Color.Empty;
            }

            //{
            //    Color c;
            //    // First, check to see if this is a standard name.
            //    //
            //    //if (ColorTable.TryGetNamedColor(text, out c))
            //    //{
            //    //    return c;
            //    //}
            //}

            char sep = culture.TextInfo.ListSeparator[0];

            // If the value is a 6 digit hex number only, then
            // we want to treat the Alpha as 255, not 0
            //
            if (!text.Contains(sep))
            {
                // text can be '' (empty quoted string)
                if (text.Length >= 2 && (text[0] == '\'' || text[0] == '"') && text[0] == text[text.Length - 1])
                {
                    // In quotes means a named value
                    string colorName = text.Substring(1, text.Length - 2);
                    //Color c;
                    //if(ColorTable.TryGetNamedColor( colorName , out c ))
                    //{
                    //    return c;
                    //}
                    //else
                    {
                        return Color.Empty;
                    }
                }
                else if ((text.Length == 7 && text[0] == '#') ||
                         (text.Length == 8 && (text.StartsWith("0x") || text.StartsWith("0X"))) ||
                         (text.Length == 8 && (text.StartsWith("&h") || text.StartsWith("&H"))))
                {
                    // Note: int.Parse will raise exception if value cannot be converted.
                    return Color.FromArgb((int)(0xFF000000 | (int)IntFromString(text, culture)));
                }
            }

            // Nope. Parse the RGBA from the text.
            //
            string[] tokens = text.Split(sep);
            int[] values = new int[tokens.Length];
            for (int i = 0; i < values.Length; i++)
            {
                values[i] = unchecked(IntFromString(tokens[i], culture));
            }

            // We should now have a number of parsed integer values.
            // We support 1, 3, or 4 arguments:
            //
            // 1 -- full ARGB encoded
            // 3 -- RGB
            // 4 -- ARGB
            //
            return values.Length switch
            {
                1 => Color.FromArgb((int)values[0]),
                3 => Color.FromArgb(values[0], values[1], values[2]),
                4 => Color.FromArgb(values[0], values[1], values[2], values[3]),
                _ => throw new ArgumentException("InvalidColor:" + text),
            };
        }

        private static int IntFromString(string text, CultureInfo culture)
        {
            text = text.Trim();

            try
            {
                if (text[0] == '#')
                {
                    return IntFromString(text.Substring(1), 16);
                }
                else if (text.StartsWith("0x", StringComparison.OrdinalIgnoreCase)
                         || text.StartsWith("&h", StringComparison.OrdinalIgnoreCase))
                {
                    return IntFromString(text.Substring(2), 16);
                }
                else
                {
                    var formatInfo = (NumberFormatInfo?)culture.GetFormat(typeof(NumberFormatInfo));
                    return IntFromString(text, formatInfo);
                }
            }
            catch (Exception e)
            {
                throw new ArgumentException("ConvertInvalidPrimitive:" + text, e);
            }
        }

        private static int IntFromString(string value, int radix)
        {
            return Convert.ToInt32(value, radix);
        }

        private static int IntFromString(string value, NumberFormatInfo formatInfo)
        {
            return int.Parse(value, NumberStyles.Integer, formatInfo);
        }
    }
     

    /// <summary>
    /// Translates colors to and from GDI+ <see cref='Color'/> objects.
    /// </summary>
    public static class ColorTranslator
    {
        static ColorTranslator()
        {
            var table = new Dictionary<string, uint>();
            table.Add("none", 0);
            table.Add("activeborder", 0xFFB4B4B4);
            table.Add("activecaption", 0xFFB9D1EA);
            table.Add("activecaptiontext", 0xFF000000);
            table.Add("aliceblue", 0xFFF0F8FF);
            table.Add("antiquewhite", 0xFFFAEBD7);
            table.Add("appworkspace", 0xFFABABAB);
            table.Add("aqua", 0xFF00FFFF);
            table.Add("aquamarine", 0xFF7FFFD4);
            table.Add("azure", 0xFFF0FFFF);
            table.Add("background", 0xFF000000);
            table.Add("beige", 0xFFF5F5DC);
            table.Add("bisque", 0xFFFFE4C4);
            table.Add("black", 0xFF000000);
            table.Add("blanchedalmond", 0xFFFFEBCD);
            table.Add("blue", 0xFF0000FF);
            table.Add("blueviolet", 0xFF8A2BE2);
            table.Add("brown", 0xFFA52A2A);
            table.Add("burlywood", 0xFFDEB887);
            table.Add("buttonface", 0xFFE3E3E3);
            table.Add("buttonhighlight", 0xFFFFFFFF);
            table.Add("buttonshadow", 0xFFA0A0A0);
            table.Add("buttontext", 0xFF000000);
            table.Add("cadetblue", 0xFF5F9EA0);
            table.Add("captiontext", 0xFF000000);
            table.Add("chartreuse", 0xFF7FFF00);
            table.Add("chocolate", 0xFFD2691E);
            table.Add("control", 0xFFF0F0F0);
            table.Add("controldark", 0xFFA0A0A0);
            table.Add("controldarkdark", 0xFF696969);
            table.Add("controllight", 0xFFE3E3E3);
            table.Add("controllightlight", 0xFFFFFFFF);
            table.Add("controltext", 0xFF000000);
            table.Add("coral", 0xFFFF7F50);
            table.Add("cornflowerblue", 0xFF6495ED);
            table.Add("cornsilk", 0xFFFFF8DC);
            table.Add("crimson", 0xFFDC143C);
            table.Add("cyan", 0xFF00FFFF);
            table.Add("darkblue", 0xFF00008B);
            table.Add("darkcyan", 0xFF008B8B);
            table.Add("darkgoldenrod", 0xFFB8860B);
            table.Add("darkgray", 0xFFA9A9A9);
            table.Add("darkgreen", 0xFF006400);
            table.Add("darkkhaki", 0xFFBDB76B);
            table.Add("darkmagenta", 0xFF8B008B);
            table.Add("darkolivegreen", 0xFF556B2F);
            table.Add("darkorange", 0xFFFF8C00);
            table.Add("darkorchid", 0xFF9932CC);
            table.Add("darkred", 0xFF8B0000);
            table.Add("darksalmon", 0xFFE9967A);
            table.Add("darkseagreen", 0xFF8FBC8B);
            table.Add("darkslateblue", 0xFF483D8B);
            table.Add("darkslategray", 0xFF2F4F4F);
            table.Add("darkturquoise", 0xFF00CED1);
            table.Add("darkviolet", 0xFF9400D3);
            table.Add("deeppink", 0xFFFF1493);
            table.Add("deepskyblue", 0xFF00BFFF);
            table.Add("desktop", 0xFF000000);
            table.Add("dimgray", 0xFF696969);
            table.Add("dodgerblue", 0xFF1E90FF);
            table.Add("firebrick", 0xFFB22222);
            table.Add("floralwhite", 0xFFFFFAF0);
            table.Add("forestgreen", 0xFF228B22);
            table.Add("fuchsia", 0xFFFF00FF);
            table.Add("gainsboro", 0xFFDCDCDC);
            table.Add("ghostwhite", 0xFFF8F8FF);
            table.Add("gold", 0xFFFFD700);
            table.Add("goldenrod", 0xFFDAA520);
            table.Add("gradientactivecaption", 0xFFB9D1EA);
            table.Add("gradientinactivecaption", 0xFFD7E4F2);
            table.Add("gray", 0xFF808080);
            table.Add("graytext", 0xFF6D6D6D);
            table.Add("green", 0xFF008000);
            table.Add("greenyellow", 0xFFADFF2F);
            table.Add("highlight", 0xFF0066CC);
            table.Add("highlighttext", 0xFF0078D7);
            table.Add("honeydew", 0xFFF0FFF0);
            table.Add("hotpink", 0xFFFF69B4);
            table.Add("hottrack", 0xFF0066CC);
            table.Add("inactiveborder", 0xFFF4F7FC);
            table.Add("inactivecaption", 0xFFBFCDDB);
            table.Add("inactivecaptiontext", 0xFF000000);
            table.Add("indianred", 0xFFCD5C5C);
            table.Add("indigo", 0xFF4B0082);
            table.Add("info", 0xFFFFFFE1);
            table.Add("infobackground", 0xFFFFFFE1);
            table.Add("infotext", 0xFF000000);
            table.Add("ivory", 0xFFFFFFF0);
            table.Add("khaki", 0xFFF0E68C);
            table.Add("lavender", 0xFFE6E6FA);
            table.Add("lavenderblush", 0xFFFFF0F5);
            table.Add("lawngreen", 0xFF7CFC00);
            table.Add("lemonchiffon", 0xFFFFFACD);
            table.Add("lightblue", 0xFFADD8E6);
            table.Add("lightcoral", 0xFFF08080);
            table.Add("lightcyan", 0xFFE0FFFF);
            table.Add("lightgoldenrodyellow", 0xFFFAFAD2);
            table.Add("lightgray", 0xFFD3D3D3);
            table.Add("lightgreen", 0xFF90EE90);
            table.Add("lightgrey", 0xFFD3D3D3);
            table.Add("lightpink", 0xFFFFB6C1);
            table.Add("lightsalmon", 0xFFFFA07A);
            table.Add("lightseagreen", 0xFF20B2AA);
            table.Add("lightskyblue", 0xFF87CEFA);
            table.Add("lightslategray", 0xFF778899);
            table.Add("lightsteelblue", 0xFFB0C4DE);
            table.Add("lightyellow", 0xFFFFFFE0);
            table.Add("lime", 0xFF00FF00);
            table.Add("limegreen", 0xFF32CD32);
            table.Add("linen", 0xFFFAF0E6);
            table.Add("magenta", 0xFFFF00FF);
            table.Add("maroon", 0xFF800000);
            table.Add("mediumaquamarine", 0xFF66CDAA);
            table.Add("mediumblue", 0xFF0000CD);
            table.Add("mediumorchid", 0xFFBA55D3);
            table.Add("mediumpurple", 0xFF9370DB);
            table.Add("mediumseagreen", 0xFF3CB371);
            table.Add("mediumslateblue", 0xFF7B68EE);
            table.Add("mediumspringgreen", 0xFF00FA9A);
            table.Add("mediumturquoise", 0xFF48D1CC);
            table.Add("mediumvioletred", 0xFFC71585);
            table.Add("menu", 0xFFF0F0F0);
            table.Add("menubar", 0xFFF0F0F0);
            table.Add("menuhighlight", 0xFF0078D7);
            table.Add("menutext", 0xFF000000);
            table.Add("midnightblue", 0xFF191970);
            table.Add("mintcream", 0xFFF5FFFA);
            table.Add("mistyrose", 0xFFFFE4E1);
            table.Add("moccasin", 0xFFFFE4B5);
            table.Add("navajowhite", 0xFFFFDEAD);
            table.Add("navy", 0xFF000080);
            table.Add("oldlace", 0xFFFDF5E6);
            table.Add("olive", 0xFF808000);
            table.Add("olivedrab", 0xFF6B8E23);
            table.Add("orange", 0xFFFFA500);
            table.Add("orangered", 0xFFFF4500);
            table.Add("orchid", 0xFFDA70D6);
            table.Add("palegoldenrod", 0xFFEEE8AA);
            table.Add("palegreen", 0xFF98FB98);
            table.Add("paleturquoise", 0xFFAFEEEE);
            table.Add("palevioletred", 0xFFDB7093);
            table.Add("papayawhip", 0xFFFFEFD5);
            table.Add("peachpuff", 0xFFFFDAB9);
            table.Add("peru", 0xFFCD853F);
            table.Add("pink", 0xFFFFC0CB);
            table.Add("plum", 0xFFDDA0DD);
            table.Add("powderblue", 0xFFB0E0E6);
            table.Add("purple", 0xFF800080);
            table.Add("red", 0xFFFF0000);
            table.Add("rosybrown", 0xFFBC8F8F);
            table.Add("royalblue", 0xFF4169E1);
            table.Add("saddlebrown", 0xFF8B4513);
            table.Add("salmon", 0xFFFA8072);
            table.Add("sandybrown", 0xFFF4A460);
            table.Add("scrollbar", 0xFFC8C8C8);
            table.Add("seagreen", 0xFF2E8B57);
            table.Add("seashell", 0xFFFFF5EE);
            table.Add("sienna", 0xFFA0522D);
            table.Add("silver", 0xFFC0C0C0);
            table.Add("skyblue", 0xFF87CEEB);
            table.Add("slateblue", 0xFF6A5ACD);
            table.Add("slategray", 0xFF708090);
            table.Add("snow", 0xFFFFFAFA);
            table.Add("springgreen", 0xFF00FF7F);
            table.Add("steelblue", 0xFF4682B4);
            table.Add("tan", 0xFFD2B48C);
            table.Add("teal", 0xFF008080);
            table.Add("thistle", 0xFFD8BFD8);
            table.Add("threeddarkshadow", 0xFF696969);
            table.Add("tomato", 0xFFFF6347);
            table.Add("transparent", 0x00FFFFFF);
            table.Add("turquoise", 0xFF40E0D0);
            table.Add("violet", 0xFFEE82EE);
            table.Add("wheat", 0xFFF5DEB3);
            table.Add("white", 0xFFFFFFFF);
            table.Add("whitesmoke", 0xFFF5F5F5);
            table.Add("window", 0xFFFFFFFF);
            table.Add("windowframe", 0xFF646464);
            table.Add("windowtext", 0xFF000000);
            table.Add("yellow", 0xFFFFFF00);
            table.Add("yellowgreen", 0xFF9ACD32);
            _ColorTable = table;
        }
        public static Color FromHtml(string htmlColor)
        {
            Color c;
            if(TryFromHtml( htmlColor , out c ))
            {
                return c;
            }
            else
            {
                throw new ArgumentException("Bad color[" + htmlColor + ']');
            }
        }
        public static bool TryFromHtml(string htmlColor, out Color result)
        {
            // empty color
            if ((htmlColor == null) || (htmlColor.Length == 0))
            {
                result = Color.Empty;
                return true;
            }
            htmlColor = htmlColor.Trim();
            // #RRGGBB or #RGB
            if ((htmlColor[0] == '#') &&
                ((htmlColor.Length == 7) || (htmlColor.Length == 4)))
            {
                if (htmlColor.Length == 7)
                {
                    result = Color.FromArgb(Convert.ToInt32(htmlColor.Substring(1, 2), 16),
                                       Convert.ToInt32(htmlColor.Substring(3, 2), 16),
                                       Convert.ToInt32(htmlColor.Substring(5, 2), 16));
                }
                else
                {
                    string r = char.ToString(htmlColor[1]);
                    string g = char.ToString(htmlColor[2]);
                    string b = char.ToString(htmlColor[3]);

                    result = Color.FromArgb(Convert.ToInt32(r + r, 16),
                                       Convert.ToInt32(g + g, 16),
                                       Convert.ToInt32(b + b, 16));
                }
                return true;
            }
            if (htmlColor.Length > 10)
            {
                if (htmlColor.StartsWith("rgb(", StringComparison.OrdinalIgnoreCase) && htmlColor[htmlColor.Length - 1] == ')')
                {
                    htmlColor = htmlColor.Substring(4, htmlColor.Length - 5);
                    var items = htmlColor.Split(',');
                    if (items.Length == 3)
                    {
                        int r = 0, g = 0, b = 0;
                        if (int.TryParse(items[0], out r)
                            && int.TryParse(items[1], out g)
                            && int.TryParse(items[2], out b))
                        {
                            result = Color.FromArgb(r, g, b);
                            return true;
                        }
                    }
                }
                else if (htmlColor.StartsWith("rgba(", StringComparison.CurrentCultureIgnoreCase) && htmlColor[htmlColor.Length - 1] == ')')
                {
                    htmlColor = htmlColor.Substring(5, htmlColor.Length - 6);
                    var items = htmlColor.Split(',');
                    int r = 0, g = 0, b = 0;
                    float a = 0;
                    if (items.Length == 4)
                    {
                        if (int.TryParse(items[0], out r)
                            && int.TryParse(items[1], out g)
                            && int.TryParse(items[2], out b)
                            && float.TryParse(items[3], out a))
                        {
                            result = Color.FromArgb((int)(a * 255), r, g, b);
                            return true;
                        }
                    }
                }
                //result = Color.Empty;
                //return true;
            }
            uint cv2 = 0;
            if (_ColorTable.TryGetValue(htmlColor.ToLower(), out cv2))
            {
                result = new Color(cv2);
                return true;
            }
            else
            {
                try
                {
                    result = ColorConverterCommon.ConvertFromString(htmlColor, CultureInfo.CurrentCulture);
                }
                catch (Exception ex)
                {
                    result = Color.Empty;
                    return false;
                }
            }
            return true;
        }

        private static Dictionary<string, uint> _ColorTable = null;
        /// <summary>
        /// Translates the specified <see cref='Color'/> to an Html string color representation.
        /// </summary>
        public static string ToHtml(Color c)
        {
            switch (c._value)
            {
                case 0: return "none";
                case (uint)0x00FFFFFF: return "Transparent";
                case (uint)0xFF000000: return "Black";
                case (uint)0xFF000080: return "Navy";
                case (uint)0xFF00008B: return "DarkBlue";
                case (uint)0xFF0000CD: return "MediumBlue";
                case (uint)0xFF0000FF: return "Blue";
                case (uint)0xFF006400: return "DarkGreen";
                case (uint)0xFF0066CC: return "highlight";
                case (uint)0xFF0078D7: return "highlight";
                case (uint)0xFF008000: return "Green";
                case (uint)0xFF008080: return "Teal";
                case (uint)0xFF008B8B: return "DarkCyan";
                case (uint)0xFF00BFFF: return "DeepSkyBlue";
                case (uint)0xFF00CED1: return "DarkTurquoise";
                case (uint)0xFF00FA9A: return "MediumSpringGreen";
                case (uint)0xFF00FF00: return "Lime";
                case (uint)0xFF00FF7F: return "SpringGreen";
                case (uint)0xFF00FFFF: return "Cyan";
                case (uint)0xFF191970: return "MidnightBlue";
                case (uint)0xFF1E90FF: return "DodgerBlue";
                case (uint)0xFF20B2AA: return "LightSeaGreen";
                case (uint)0xFF228B22: return "ForestGreen";
                case (uint)0xFF2E8B57: return "SeaGreen";
                case (uint)0xFF2F4F4F: return "DarkSlateGray";
                case (uint)0xFF32CD32: return "LimeGreen";
                case (uint)0xFF3CB371: return "MediumSeaGreen";
                case (uint)0xFF40E0D0: return "Turquoise";
                case (uint)0xFF4169E1: return "RoyalBlue";
                case (uint)0xFF4682B4: return "SteelBlue";
                case (uint)0xFF483D8B: return "DarkSlateBlue";
                case (uint)0xFF48D1CC: return "MediumTurquoise";
                case (uint)0xFF4B0082: return "Indigo";
                case (uint)0xFF556B2F: return "DarkOliveGreen";
                case (uint)0xFF5F9EA0: return "CadetBlue";
                case (uint)0xFF646464: return "windowframe";
                case (uint)0xFF6495ED: return "CornflowerBlue";
                case (uint)0xFF66CDAA: return "MediumAquamarine";
                case (uint)0xFF696969: return "DimGray";
                case (uint)0xFF6A5ACD: return "SlateBlue";
                case (uint)0xFF6B8E23: return "OliveDrab";
                case (uint)0xFF6D6D6D: return "graytext";
                case (uint)0xFF708090: return "SlateGray";
                case (uint)0xFF778899: return "LightSlateGray";
                case (uint)0xFF7B68EE: return "MediumSlateBlue";
                case (uint)0xFF7CFC00: return "LawnGreen";
                case (uint)0xFF7FFF00: return "Chartreuse";
                case (uint)0xFF7FFFD4: return "Aquamarine";
                case (uint)0xFF800000: return "Maroon";
                case (uint)0xFF800080: return "Purple";
                case (uint)0xFF808000: return "Olive";
                case (uint)0xFF808080: return "Gray";
                case (uint)0xFF87CEEB: return "SkyBlue";
                case (uint)0xFF87CEFA: return "LightSkyBlue";
                case (uint)0xFF8A2BE2: return "BlueViolet";
                case (uint)0xFF8B0000: return "DarkRed";
                case (uint)0xFF8B008B: return "DarkMagenta";
                case (uint)0xFF8B4513: return "SaddleBrown";
                case (uint)0xFF8FBC8B: return "DarkSeaGreen";
                case (uint)0xFF90EE90: return "LightGreen";
                case (uint)0xFF9370DB: return "MediumPurple";
                case (uint)0xFF9400D3: return "DarkViolet";
                case (uint)0xFF98FB98: return "PaleGreen";
                case (uint)0xFF9932CC: return "DarkOrchid";
                case (uint)0xFF99B4D1: return "activecaption";
                case (uint)0xFF9ACD32: return "YellowGreen";
                case (uint)0xFFA0522D: return "Sienna";
                case (uint)0xFFA52A2A: return "Brown";
                case (uint)0xFFA9A9A9: return "DarkGray";
                case (uint)0xFFABABAB: return "appworkspace";
                case (uint)0xFFADD8E6: return "LightBlue";
                case (uint)0xFFADFF2F: return "GreenYellow";
                case (uint)0xFFAFEEEE: return "PaleTurquoise";
                case (uint)0xFFB0C4DE: return "LightSteelBlue";
                case (uint)0xFFB0E0E6: return "PowderBlue";
                case (uint)0xFFB22222: return "Firebrick";
                case (uint)0xFFB4B4B4: return "activeborder";
                case (uint)0xFFB8860B: return "DarkGoldenrod";
                case (uint)0xFFB9D1EA: return "activecaption";
                case (uint)0xFFBA55D3: return "MediumOrchid";
                case (uint)0xFFBC8F8F: return "RosyBrown";
                case (uint)0xFFBDB76B: return "DarkKhaki";
                case (uint)0xFFBFCDDB: return "inactivecaption";
                case (uint)0xFFC0C0C0: return "Silver";
                case (uint)0xFFC71585: return "MediumVioletRed";
                case (uint)0xFFC8C8C8: return "scrollbar";
                case (uint)0xFFCD5C5C: return "IndianRed";
                case (uint)0xFFCD853F: return "Peru";
                case (uint)0xFFD2691E: return "Chocolate";
                case (uint)0xFFD2B48C: return "Tan";
                case (uint)0xFFD3D3D3: return "LightGrey";
                case (uint)0xFFD7E4F2: return "inactivecaption";
                case (uint)0xFFD8BFD8: return "Thistle";
                case (uint)0xFFDA70D6: return "Orchid";
                case (uint)0xFFDAA520: return "Goldenrod";
                case (uint)0xFFDB7093: return "PaleVioletRed";
                case (uint)0xFFDC143C: return "Crimson";
                case (uint)0xFFDCDCDC: return "Gainsboro";
                case (uint)0xFFDDA0DD: return "Plum";
                case (uint)0xFFDEB887: return "BurlyWood";
                case (uint)0xFFE0FFFF: return "LightCyan";
                case (uint)0xFFE3E3E3: return "buttonface";
                case (uint)0xFFE6E6FA: return "Lavender";
                case (uint)0xFFE9967A: return "DarkSalmon";
                case (uint)0xFFEE82EE: return "Violet";
                case (uint)0xFFEEE8AA: return "PaleGoldenrod";
                case (uint)0xFFF08080: return "LightCoral";
                case (uint)0xFFF0E68C: return "Khaki";
                //case (uint)0xFFF0F0F0: return "";
                case (uint)0xFFF0F8FF: return "AliceBlue";
                case (uint)0xFFF0FFF0: return "Honeydew";
                case (uint)0xFFF0FFFF: return "Azure";
                case (uint)0xFFF4A460: return "SandyBrown";
                case (uint)0xFFF4F7FC: return "inactiveborder";
                case (uint)0xFFF5DEB3: return "Wheat";
                case (uint)0xFFF5F5DC: return "Beige";
                case (uint)0xFFF5F5F5: return "WhiteSmoke";
                case (uint)0xFFF5FFFA: return "MintCream";
                case (uint)0xFFF8F8FF: return "GhostWhite";
                case (uint)0xFFFA8072: return "Salmon";
                case (uint)0xFFFAEBD7: return "AntiqueWhite";
                case (uint)0xFFFAF0E6: return "Linen";
                case (uint)0xFFFAFAD2: return "LightGoldenrodYellow";
                case (uint)0xFFFDF5E6: return "OldLace";
                case (uint)0xFFFF0000: return "Red";
                case (uint)0xFFFF00FF: return "Magenta";
                case (uint)0xFFFF1493: return "DeepPink";
                case (uint)0xFFFF4500: return "OrangeRed";
                case (uint)0xFFFF6347: return "Tomato";
                case (uint)0xFFFF69B4: return "HotPink";
                case (uint)0xFFFF7F50: return "Coral";
                case (uint)0xFFFF8C00: return "DarkOrange";
                case (uint)0xFFFFA07A: return "LightSalmon";
                case (uint)0xFFFFA500: return "Orange";
                case (uint)0xFFFFB6C1: return "LightPink";
                case (uint)0xFFFFC0CB: return "Pink";
                case (uint)0xFFFFD700: return "Gold";
                case (uint)0xFFFFDAB9: return "PeachPuff";
                case (uint)0xFFFFDEAD: return "NavajoWhite";
                case (uint)0xFFFFE4B5: return "Moccasin";
                case (uint)0xFFFFE4C4: return "Bisque";
                case (uint)0xFFFFE4E1: return "MistyRose";
                case (uint)0xFFFFEBCD: return "BlanchedAlmond";
                case (uint)0xFFFFEFD5: return "PapayaWhip";
                case (uint)0xFFFFF0F5: return "LavenderBlush";
                case (uint)0xFFFFF5EE: return "SeaShell";
                case (uint)0xFFFFF8DC: return "Cornsilk";
                case (uint)0xFFFFFACD: return "LemonChiffon";
                case (uint)0xFFFFFAF0: return "FloralWhite";
                case (uint)0xFFFFFAFA: return "Snow";
                case (uint)0xFFFFFF00: return "Yellow";
                case (uint)0xFFFFFFE0: return "LightYellow";
                case (uint)0xFFFFFFE1: return "infobackground";
                case (uint)0xFFFFFFF0: return "Ivory";
                case (uint)0xFFFFFFFF: return "White";
            }

            if (c.A != 255)
            {
                return "rgba(" + c.R + ',' + c.G + ',' + c.B + "," + (c.A / 255.0).ToString("0.0000") + ")";
            }
            else
            {
                return $"#{c.R:X2}{c.G:X2}{c.B:X2}";
            }
        }

    }
#if ! RELEASE
    [System.Diagnostics.DebuggerDisplay("Point:X={X},Y={Y}")]
#endif
    public struct Point : IEquatable<Point>
    {
        /// <summary>
        /// Creates a new instance of the <see cref=' Point'/> class with member data left uninitialized.
        /// </summary>
        public static readonly Point Empty;

        private int _x; // Do not rename (binary serialization)
        private int _y; // Do not rename (binary serialization)

        /// <summary>
        /// Initializes a new instance of the <see cref=' Point'/> class with the specified coordinates.
        /// </summary>
        public Point(int x, int y)
        {
            this._x = x;
            this._y = y;
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref=' Point'/> is empty.
        /// </summary>
        public readonly bool IsEmpty => this._x == 0 && this._y == 0;

        /// <summary>
        /// Gets the x-coordinate of this <see cref=' Point'/>.
        /// </summary>
        public int X
        {
            readonly get => this._x;
            set => this._x = value;
        }

        /// <summary>
        /// Gets the y-coordinate of this <see cref=' Point'/>.
        /// </summary>
        public int Y
        {
            readonly get => this._y;
            set => this._y = value;
        }


        /// <summary>
        /// Compares two <see cref=' Point'/> objects. The result specifies whether the values of the
        /// <see cref=' Point.X'/> and <see cref=' Point.Y'/> properties of the two
        /// <see cref=' Point'/> objects are equal.
        /// </summary>
        public static bool operator ==(Point left, Point right) => left._x == right._x && left._y == right._y;

        /// <summary>
        /// Compares two <see cref=' Point'/> objects. The result specifies whether the values of the
        /// <see cref=' Point.X'/> or <see cref=' Point.Y'/> properties of the two
        /// <see cref=' Point'/>  objects are unequal.
        /// </summary>
        public static bool operator !=(Point left, Point right) => !(left == right);


        /// <summary>
        /// Specifies whether this <see cref=' Point'/> contains the same coordinates as the specified
        /// <see cref='object'/>.
        /// </summary>
        public override readonly bool Equals(object obj) => obj is Point && Equals((Point)obj);

        public readonly bool Equals(Point other) => this == other;

        /// <summary>
        /// Returns a hash code.
        /// </summary>
        public override readonly int GetHashCode() => HashCode.Combine(this._x, this._y);

        /// <summary>
        /// Translates this <see cref=' Point'/> by the specified amount.
        /// </summary>
        public void Offset(int dx, int dy)
        {
            unchecked
            {
                this._x += dx;
                this._y += dy;
            }
        }

#if !RELEASE
        /// <summary>
        /// Converts this <see cref=' Point'/> to a human readable string.
        /// </summary>
        public override readonly string ToString() => $"{{X={X},Y={Y}}}";
#endif
    }
#if !RELEASE
    [System.Diagnostics.DebuggerDisplay("PointF:X={X},Y={Y}")]
#endif
    public struct PointF : IEquatable<PointF>
    {
        /// <summary>
        /// Creates a new instance of the <see cref=' PointF'/> class with member data left uninitialized.
        /// </summary>
        public static readonly PointF Empty;
        private float _x; // Do not rename (binary serialization)
        private float _y; // Do not rename (binary serialization)

        /// <summary>
        /// Initializes a new instance of the <see cref=' PointF'/> class with the specified coordinates.
        /// </summary>
        public PointF(float x, float y)
        {
            this._x = x;
            this._y = y;
        }

        /// <summary>
        /// Gets the x-coordinate of this <see cref=' PointF'/>.
        /// </summary>
        public float X
        {
            readonly get => this._x;
            set => this._x = value;
        }

        /// <summary>
        /// Gets the y-coordinate of this <see cref=' PointF'/>.
        /// </summary>
        public float Y
        {
            readonly get => this._y;
            set => this._y = value;
        }


        /// <summary>
        /// Compares two <see cref=' PointF'/> objects. The result specifies whether the values of the
        /// <see cref=' PointF.X'/> and <see cref=' PointF.Y'/> properties of the two
        /// <see cref=' PointF'/> objects are equal.
        /// </summary>
        public static bool operator ==(PointF left, PointF right) => left._x == right._x && left._y == right._y;

        /// <summary>
        /// Compares two <see cref=' PointF'/> objects. The result specifies whether the values of the
        /// <see cref=' PointF.X'/> or <see cref=' PointF.Y'/> properties of the two
        /// <see cref=' PointF'/> objects are unequal.
        /// </summary>
        public static bool operator !=(PointF left, PointF right) => !(left == right);

        public override readonly bool Equals(object obj) => obj is PointF && Equals((PointF)obj);

        public readonly bool Equals(PointF other) => this == other;

        public override readonly int GetHashCode() => HashCode.Combine(this._x.GetHashCode(), this._y.GetHashCode());
#if ! RELEASE
        public override readonly string ToString() => $"{{X={_x}, Y={_y}}}";
#endif
    }
#if !RELEASE
    [System.Diagnostics.DebuggerDisplay("Rectangle:Left={X},Top={Y},Width={Width},Height={Height}")]
#endif
    public struct Rectangle : IEquatable<Rectangle>
    {
        public static readonly Rectangle Empty;

        internal int _x; // Do not rename (binary serialization)
        internal int _y; // Do not rename (binary serialization)
        internal int _width; // Do not rename (binary serialization)
        internal int _height; // Do not rename (binary serialization)

        /// <summary>
        /// Initializes a new instance of the <see cref=' Rectangle'/> class with the specified location
        /// and size.
        /// </summary>
        public Rectangle(int x, int y, int width, int height)
        {
            this._x = x;
            this._y = y;
            this._width = width;
            this._height = height;
        }

        /// <summary>
        /// Initializes a new instance of the Rectangle class with the specified location and size.
        /// </summary>
        public Rectangle(Point location, Size size)
        {
            this._x = location.X;
            this._y = location.Y;
            this._width = size.Width;
            this._height = size.Height;
        }

        public RectangleF ToFloat()
        {
            return new RectangleF(this._x, this._y, this._width, this._height);
        }

        /// <summary>
        /// Gets or sets the coordinates of the upper-left corner of the rectangular region represented by this
        /// <see cref=' Rectangle'/>.
        /// </summary>
        public Point Location
        {
            readonly get => new Point(this._x, this._y);
            set
            {
                this._x = value.X;
                this._y = value.Y;
            }
        }

        /// <summary>
        /// Gets or sets the size of this <see cref=' Rectangle'/>.
        /// </summary>
        public Size Size
        {
            readonly get => new Size(this._width, this._height);
            set
            {
                this._width = value.Width;
                this._height = value.Height;
            }
        }

        /// <summary>
        /// Gets or sets the x-coordinate of the upper-left corner of the rectangular region defined by this
        /// <see cref=' Rectangle'/>.
        /// </summary>
        public int X
        {
            readonly get => this._x;
            set => this._x = value;
        }

        /// <summary>
        /// Gets or sets the y-coordinate of the upper-left corner of the rectangular region defined by this
        /// <see cref=' Rectangle'/>.
        /// </summary>
        public int Y
        {
            readonly get => this._y;
            set => this._y = value;
        }

        /// <summary>
        /// Gets or sets the width of the rectangular region defined by this <see cref=' Rectangle'/>.
        /// </summary>
        public int Width
        {
            readonly get => this._width;
            set => this._width = value;
        }

        /// <summary>
        /// Gets or sets the width of the rectangular region defined by this <see cref=' Rectangle'/>.
        /// </summary>
        public int Height
        {
            readonly get => this._height;
            set => this._height = value;
        }

        /// <summary>
        /// Gets the x-coordinate of the upper-left corner of the rectangular region defined by this
        /// <see cref=' Rectangle'/> .
        /// </summary>
        public readonly int Left => this._x;

        /// <summary>
        /// Gets the y-coordinate of the upper-left corner of the rectangular region defined by this
        /// <see cref=' Rectangle'/>.
        /// </summary>
        public readonly int Top => this._y;

        /// <summary>
        /// Gets the x-coordinate of the lower-right corner of the rectangular region defined by this
        /// <see cref=' Rectangle'/>.
        /// </summary>
        public readonly int Right => unchecked(this._x + this._width);

        /// <summary>
        /// Gets the y-coordinate of the lower-right corner of the rectangular region defined by this
        /// <see cref=' Rectangle'/>.
        /// </summary>
        public readonly int Bottom => unchecked(this._y + this._height);

        /// <summary>
        /// Tests whether this <see cref=' Rectangle'/> has a <see cref=' Rectangle.Width'/>
        /// or a <see cref=' Rectangle.Height'/> of 0.
        /// </summary>
        public readonly bool IsEmpty => this._height == 0 && this._width == 0 && this._x == 0 && this._y == 0;

        /// <summary>
        /// Tests whether <paramref name="obj"/> is a <see cref=' Rectangle'/> with the same location
        /// and size of this Rectangle.
        /// </summary>
        public override readonly bool Equals(object obj) => obj is Rectangle && Equals((Rectangle)obj);

        public readonly bool Equals(Rectangle other) => this == other;

        /// <summary>
        /// Tests whether two <see cref=' Rectangle'/> objects have equal location and size.
        /// </summary>
        public static bool operator ==(Rectangle left, Rectangle right) =>
            left._x == right._x && left._y == right._y && left._width == right._width && left._height == right._height;

        /// <summary>
        /// Tests whether two <see cref=' Rectangle'/> objects differ in location or size.
        /// </summary>
        public static bool operator !=(Rectangle left, Rectangle right) => !(left == right);

        /// <summary>
        /// Converts a RectangleF to a Rectangle by performing a ceiling operation on all the coordinates.
        /// </summary>
        public static Rectangle Ceiling(RectangleF value)
        {
            unchecked
            {
                return new Rectangle(
                    (int)Math.Ceiling(value.X),
                    (int)Math.Ceiling(value.Y),
                    (int)Math.Ceiling(value.Width),
                    (int)Math.Ceiling(value.Height));
            }
        }
        public static RectangleF CeilingFloat(RectangleF value)
        {
            unchecked
            {
                return new RectangleF(
                    (float)Math.Ceiling(value.X),
                    (float)Math.Ceiling(value.Y),
                    (float)Math.Ceiling(value.Width),
                    (float)Math.Ceiling(value.Height));
            }
        }

        /// <summary>
        /// Determines if the specified point is contained within the rectangular region defined by this
        /// <see cref=' Rectangle'/> .
        /// </summary>
        public readonly bool Contains(int x, int y) => this._x <= x && x < this._x + this._width && this._y <= y && y < this._y + this._height;

        /// <summary>
        /// Determines if the specified point is contained within the rectangular region defined by this
        /// <see cref=' Rectangle'/> .
        /// </summary>
        public readonly bool Contains(Point pt) => Contains(pt.X, pt.Y);

        /// <summary>
        /// Determines if the rectangular region represented by <paramref name="rect"/> is entirely contained within the
        /// rectangular region represented by this <see cref=' Rectangle'/> .
        /// </summary>
        public readonly bool Contains(Rectangle rect) =>
            (this._x <= rect._x) && (rect._x + rect._width <= this._x + this._width) &&
            (this._y <= rect._y) && (rect._y + rect._height <= this._y + this._height);

        public override readonly int GetHashCode() => HashCode.Combine(X, Y, Width, Height);

        /// <summary>
        /// Creates a rectangle that represents the intersection between a and b. If there is no intersection, an
        /// empty rectangle is returned.
        /// </summary>
        public static Rectangle Intersect(Rectangle a, Rectangle b)
        {
            int x1 = Math.Max(a._x, b._x);
            int x2 = Math.Min(a._x + a._width, b._x + b._width);
            int y1 = Math.Max(a._y, b._y);
            int y2 = Math.Min(a._y + a._height, b._y + b._height);

            if (x2 >= x1 && y2 >= y1)
            {
                return new Rectangle(x1, y1, x2 - x1, y2 - y1);
            }

            return Empty;
        }

        /// <summary>
        /// Determines if this rectangle intersects with rect.
        /// </summary>
        public readonly bool IntersectsWith(Rectangle rect) =>
            (rect._x < this._x + this._width) && (this._x < rect._x + rect._width) &&
            (rect._y < this._y + this._height) && (this._y < rect._y + rect._height);

        /// <summary>
        /// Creates a rectangle that represents the union between a and b.
        /// </summary>
        public static Rectangle Union(Rectangle a, Rectangle b)
        {
            int x1 = Math.Min(a._x, b._x);
            int x2 = Math.Max(a._x + a._width, b._x + b._width);
            int y1 = Math.Min(a._y, b._y);
            int y2 = Math.Max(a._y + a._height, b._y + b._height);

            return new Rectangle(x1, y1, x2 - x1, y2 - y1);
        }

        /// <summary>
        /// Adjusts the location of this rectangle by the specified amount.
        /// </summary>
        public void Offset(int x, int y)
        {
            unchecked
            {
                this._x += x;
                this._y += y;
            }
        }
#if !RELEASE
        /// <summary>
        /// Converts the attributes of this <see cref=' Rectangle'/> to a human readable string.
        /// </summary>
        public override readonly string ToString() => $"{{X={X},Y={Y},Width={Width},Height={Height}}}";
#endif

    }
#if ! RELEASE
    [System.Diagnostics.DebuggerDisplay("RectangleF:Left={Left},Top={Top},Width={Width},Height={Height}")]
#endif
    public struct RectangleF : IEquatable<RectangleF>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref=' RectangleF'/> class.
        /// </summary>
        public static readonly RectangleF Empty;

        private float _x; // Do not rename (binary serialization)
        private float _y; // Do not rename (binary serialization)
        private float _width; // Do not rename (binary serialization)
        private float _height; // Do not rename (binary serialization)

        /// <summary>
        /// Initializes a new instance of the <see cref=' RectangleF'/> class with the specified location
        /// and size.
        /// </summary>
        public RectangleF(float x, float y, float width, float height)
        {
            this._x = x;
            this._y = y;
            this._width = width;
            this._height = height;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref=' RectangleF'/> class with the specified location
        /// and size.
        /// </summary>
        public RectangleF(PointF location, SizeF size)
        {
            this._x = location.X;
            this._y = location.Y;
            this._width = size.Width;
            this._height = size.Height;
        }

        /// <summary>
        /// Gets or sets the coordinates of the upper-left corner of the rectangular region represented by this
        /// <see cref=' RectangleF'/>.
        /// </summary>
        public PointF Location
        {
            readonly get => new PointF(this._x, this._y);
            set
            {
                this._x = value.X;
                this._y = value.Y;
            }
        }

        /// <summary>
        /// Gets or sets the size of this <see cref=' RectangleF'/>.
        /// </summary>
        public SizeF Size
        {
            readonly get => new SizeF(this._width,this._height);
            set
            {
                this._width = value.Width;
                this._height = value.Height;
            }
        }

        /// <summary>
        /// Gets or sets the x-coordinate of the upper-left corner of the rectangular region defined by this
        /// <see cref=' RectangleF'/>.
        /// </summary>
        public float X
        {
            readonly get => this._x;
            set => this._x = value;
        }

        /// <summary>
        /// Gets or sets the y-coordinate of the upper-left corner of the rectangular region defined by this
        /// <see cref=' RectangleF'/>.
        /// </summary>
        public float Y
        {
            readonly get => this._y;
            set => this._y = value;
        }

        /// <summary>
        /// Gets or sets the width of the rectangular region defined by this <see cref=' RectangleF'/>.
        /// </summary>
        public float Width
        {
            readonly get => this._width;
            set => this._width = value;
        }

        /// <summary>
        /// Gets or sets the height of the rectangular region defined by this <see cref=' RectangleF'/>.
        /// </summary>
        public float Height
        {
            readonly get => this._height;
            set => this._height = value;
        }

        /// <summary>
        /// Gets the x-coordinate of the upper-left corner of the rectangular region defined by this
        /// <see cref=' RectangleF'/> .
        /// </summary>
        public readonly float Left => this._x;

        /// <summary>
        /// Gets the y-coordinate of the upper-left corner of the rectangular region defined by this
        /// <see cref=' RectangleF'/>.
        /// </summary>
        public readonly float Top => this._y;

        /// <summary>
        /// Gets the x-coordinate of the lower-right corner of the rectangular region defined by this
        /// <see cref=' RectangleF'/>.
        /// </summary>
        public readonly float Right => this._x + this._width;

        /// <summary>
        /// Gets the y-coordinate of the lower-right corner of the rectangular region defined by this
        /// <see cref=' RectangleF'/>.
        /// </summary>
        public readonly float Bottom => this._y + this._height;

        /// <summary>
        /// Tests whether this <see cref=' RectangleF'/> has a <see cref=' RectangleF.Width'/> or a <see cref=' RectangleF.Height'/> of 0.
        /// </summary>
        public readonly bool IsEmpty => (this._width <= 0) || (this._height <= 0);

        /// <summary>
        /// Tests whether <paramref name="obj"/> is a <see cref=' RectangleF'/> with the same location and
        /// size of this <see cref=' RectangleF'/>.
        /// </summary>
        public override readonly bool Equals(object obj) => obj is RectangleF && Equals((RectangleF)obj);

        public readonly bool Equals(RectangleF other) => this == other;

        /// <summary>
        /// Tests whether two <see cref=' RectangleF'/> objects have equal location and size.
        /// </summary>
        public static bool operator ==(RectangleF left, RectangleF right) =>
            left._x == right._x && left._y == right._y && left._width == right._width && left._height == right._height;

        /// <summary>
        /// Tests whether two <see cref=' RectangleF'/> objects differ in location or size.
        /// </summary>
        public static bool operator !=(RectangleF left, RectangleF right) => !(left == right);

        /// <summary>
        /// Determines if the specified point is contained within the rectangular region defined by this
        /// <see cref=' Rectangle'/> .
        /// </summary>
        public readonly bool Contains(float x, float y) => this._x <= x && x < this._x + this._width && this._y <= y && y < this._y + this._height;

        /// <summary>
        /// Determines if the specified point is contained within the rectangular region defined by this
        /// <see cref=' Rectangle'/> .
        /// </summary>
        public readonly bool Contains(PointF pt) => Contains(pt.X, pt.Y);

        /// <summary>
        /// Determines if the rectangular region represented by <paramref name="rect"/> is entirely contained within
        /// the rectangular region represented by this <see cref=' Rectangle'/> .
        /// </summary>
        public readonly bool Contains(RectangleF rect) =>
            (this._x <= rect._x) 
            && (rect._x + rect._width <= this._x + this._width)
            && (this._y <= rect._y)
            && (rect._y + rect._height <= this._y + this._height);

        /// <summary>
        /// Gets the hash code for this <see cref=' RectangleF'/>.
        /// </summary>
        public override readonly int GetHashCode() => HashCode.Combine(this._x, this._y, this._width, this._height);

        /// <summary>
        /// Inflates this <see cref=' Rectangle'/> by the specified amount.
        /// </summary>
        public void Inflate(float x, float y)
        {
            this._x -= x;
            this._y -= y;
            this._width += 2 * x;
            this._height += 2 * y;
        }


        /// <summary>
        /// Creates a Rectangle that represents the intersection between this Rectangle and rect.
        /// </summary>
        public void Intersect(RectangleF rect)
        {
            RectangleF result = Intersect(rect, this);

            this._x = result._x;
            this._y = result._y;
            this._width = result._width;
            this._height = result._height;
        }

        /// <summary>
        /// Creates a rectangle that represents the intersection between a and b. If there is no intersection, an
        /// empty rectangle is returned.
        /// </summary>
        public static RectangleF Intersect(RectangleF a, RectangleF b)
        {
            float x1 = a._x > b._x ? a._x : b._x;
            float x2 = a._x + a._width < b._x + b._width ? a._x + a._width : b._x + b._width;
            float y1 = a._y > b._y ? a._y : b._y;
            float y2 = a._y + a._height < b._y + b._height ? a._y + a._height : b._y + b._height;

            //float x1 = Math.Max(a._x, b._x);
            //float x2 = Math.Min(a._x + a._width, b._x + b._width);
            //float y1 = Math.Max(a._y, b._y);
            //float y2 = Math.Min(a._y + a._height, b._y + b._height);

            if (x2 >= x1 && y2 >= y1)
            {
                return new RectangleF(x1, y1, x2 - x1, y2 - y1);
            }

            return Empty;
        }

        /// <summary>
        /// Determines if this rectangle intersects with rect.
        /// </summary>
        public readonly bool IntersectsWith(RectangleF rect) =>
            (rect._x < this._x + this._width) 
            && (this._x < rect._x + rect._width)
            && (rect._y < this._y + this._height)
            && (this._y < rect._y + rect._height);

        /// <summary>
        /// Creates a rectangle that represents the union between a and b.
        /// </summary>
        public static RectangleF Union(RectangleF a, RectangleF b)
        {
            float x1 = Math.Min(a._x, b._x);
            float x2 = Math.Max(a._x + a._width, b._x + b._width);
            float y1 = Math.Min(a._y, b._y);
            float y2 = Math.Max(a._y + a._height, b._y + b._height);

            return new RectangleF(x1, y1, x2 - x1, y2 - y1);
        }

        /// <summary>
        /// Adjusts the location of this rectangle by the specified amount.
        /// </summary>
        public void Offset(PointF pos)
        {
            this._x += pos.X;
            this._y += pos.Y;
        }

        /// <summary>
        /// Adjusts the location of this rectangle by the specified amount.
        /// </summary>
        public void Offset(float x, float y)
        {
            this._x += x;
            this._y += y;
        }

        public Rectangle ToInt32()
        {
            var x2 = (int)this._x;
            var y2 = (int)this._y;
            return new Rectangle(
                x2, 
                y2, 
                (int)(this._x + this._width - x2), 
                (int)(this._y + this._height - y2));
        }

        public Rectangle ToOutInt32()
        {
            var x2 = (int)Math.Ceiling(this._x);
            var y2 = (int)Math.Ceiling(this._y);
            return new Rectangle(
                x2,
                y2,
                (int)Math.Ceiling(this._x + this._width) - x2,
                (int)Math.Ceiling(this._y + this._height) - y2);
        }

#if ! RELEASE
        /// <summary>
        /// Converts the <see cref=' RectangleF.Location'/> and <see cref=' RectangleF.Size'/>
        /// of this <see cref=' RectangleF'/> to a human-readable string.
        /// </summary>
        public override readonly string ToString() => $"{{X={X},Y={Y},Width={Width},Height={Height}}}";
#endif
    }
#if ! RELEASE
    [System.Diagnostics.DebuggerDisplay("Size:Width={Width},Height={Height}")]
#endif
    public struct Size : IEquatable<Size>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref=' Size'/> class.
        /// </summary>
        public static readonly Size Empty;

        private int _width; // Do not rename (binary serialization)
        private int _height; // Do not rename (binary serialization)

        /// <summary>
        /// Initializes a new instance of the <see cref=' Size'/> class from the specified dimensions.
        /// </summary>
        public Size(int width, int height)
        {
            this._width = width;
            this._height = height;
        }


        /// <summary>
        /// Tests whether two <see cref=' Size'/> objects are identical.
        /// </summary>
        public static bool operator ==(Size sz1, Size sz2) => sz1._width == sz2._width && sz1._height == sz2._height;

        /// <summary>
        /// Tests whether two <see cref=' Size'/> objects are different.
        /// </summary>
        public static bool operator !=(Size sz1, Size sz2) => !(sz1 == sz2);

        /// <summary>
        /// Tests whether this <see cref=' Size'/> has zero width and height.
        /// </summary>
        public readonly bool IsEmpty => this._width == 0 && this._height == 0;

        /// <summary>
        /// Represents the horizontal component of this <see cref=' Size'/>.
        /// </summary>
        public int Width
        {
            readonly get => this._width;
            set => this._width = value;
        }

        /// <summary>
        /// Represents the vertical component of this <see cref=' Size'/>.
        /// </summary>
        public int Height
        {
            readonly get => this._height;
            set => this._height = value;
        }

        /// <summary>
        /// Tests to see whether the specified object is a <see cref=' Size'/>  with the same dimensions
        /// as this <see cref=' Size'/>.
        /// </summary>
        public override readonly bool Equals(object obj) => obj is Size && Equals((Size)obj);

        public readonly bool Equals(Size other) => this == other;

        /// <summary>
        /// Returns a hash code.
        /// </summary>
        public override readonly int GetHashCode() => HashCode.Combine(this._width, this._height);
#if !RELEASE
        /// <summary>
        /// Creates a human-readable string that represents this <see cref=' Size'/>.
        /// </summary>
        public override readonly string ToString() => $"{{Width={_width}, Height={_height}}}";
#endif
    }
#if ! RELEASE
    [System.Diagnostics.DebuggerDisplay("SizeF:Width={Width},Height={Height}")]
#endif
    public struct SizeF : IEquatable<SizeF>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref=' SizeF'/> class.
        /// </summary>
        public static readonly SizeF Empty;
        private float _width; // Do not rename (binary serialization)
        private float _height; // Do not rename (binary serialization)


        /// <summary>
        /// Initializes a new instance of the <see cref=' SizeF'/> class from the specified dimensions.
        /// </summary>
        public SizeF(float width, float height)
        {
            this._width = width;
            this._height = height;
        }


        /// <summary>
        /// Tests whether two <see cref=' SizeF'/> objects are identical.
        /// </summary>
        public static bool operator ==(SizeF sz1, SizeF sz2) => sz1._width == sz2._width && sz1._height == sz2._height;

        /// <summary>
        /// Tests whether two <see cref=' SizeF'/> objects are different.
        /// </summary>
        public static bool operator !=(SizeF sz1, SizeF sz2) => !(sz1 == sz2);


        /// <summary>
        /// Tests whether this <see cref=' SizeF'/> has zero width and height.
        /// </summary>
        public readonly bool IsEmpty => this._width == 0 && this._height == 0;

        /// <summary>
        /// Represents the horizontal component of this <see cref=' SizeF'/>.
        /// </summary>
        public float Width
        {
            readonly get => this._width;
            set => this._width = value;
        }

        /// <summary>
        /// Represents the vertical component of this <see cref=' SizeF'/>.
        /// </summary>
        public float Height
        {
            readonly get => this._height;
            set => this._height = value;
        }


        /// <summary>
        /// Tests to see whether the specified object is a <see cref=' SizeF'/>  with the same dimensions
        /// as this <see cref=' SizeF'/>.
        /// </summary>
        public override readonly bool Equals(object obj) => obj is SizeF && Equals((SizeF)obj);

        public readonly bool Equals(SizeF other) => this == other;

        public override readonly int GetHashCode() => HashCode.Combine(this._width, this._height);

#if !RELEASE
        /// <summary>
        /// Creates a human-readable string that represents this <see cref=' SizeF'/>.
        /// </summary>
        public override readonly string ToString() => $"{{Width={_width}, Height={_height}}}";
#endif

    }

    public static class SystemColors
    {
        //public static readonly Color ActiveBorder = new Color(0xFFB4B4B4);
        //public static readonly Color ActiveCaption = new Color(0xFF99B4D1);
        //public static readonly Color ActiveCaptionText = new Color(0xFF000000);
        //public static readonly Color AppWorkspace = new Color(0xFFABABAB);
        //public static readonly Color ButtonFace = new Color(0xFFF0F0F0);
        //public static readonly Color ButtonHighlight = new Color(0xFFFFFFFF);
        //public static readonly Color ButtonShadow = new Color(0xFFA0A0A0);
        //public static readonly Color Control = new Color(0xFFF0F0F0);
        //public static readonly Color ControlDark = new Color(0xFFA0A0A0);
        //public static readonly Color ControlDarkDark = new Color(0xFF696969);
        public static readonly Color ControlLight = new Color(0xFFE3E3E3);
        //public static readonly Color ControlLightLight = new Color(0xFFFFFFFF);
        public static readonly Color ControlText = new Color(0xFF000000);
        //public static readonly Color Desktop = new Color(0xFF000000);
        //public static readonly Color GradientActiveCaption = new Color(0xFFB9D1EA);
        //public static readonly Color GradientInactiveCaption = new Color(0xFFD7E4F2);
        //public static readonly Color GrayText = new Color(0xFF6D6D6D);
        public static readonly Color Highlight = new Color(0xFF0078D7);
        public static readonly Color HighlightText = new Color(0xFFFFFFFF);
        //public static readonly Color HotTrack = new Color(0xFF0066CC);
        //public static readonly Color InactiveBorder = new Color(0xFFF4F7FC);
        //public static readonly Color InactiveCaption = new Color(0xFFBFCDDB);
        //public static readonly Color InactiveCaptionText = new Color(0xFF000000);
        //public static readonly Color Info = new Color(0xFFFFFFE1);
        //public static readonly Color InfoText = new Color(0xFF000000);
        //public static readonly Color Menu = new Color(0xFFF0F0F0);
        //public static readonly Color MenuBar = new Color(0xFFF0F0F0);
        //public static readonly Color MenuHighlight = new Color(0xFF0078D7);
        //public static readonly Color MenuText = new Color(0xFF000000);
        //public static readonly Color ScrollBar = new Color(0xFFC8C8C8);
        public static readonly Color Window = new Color(0xFFFFFFFF);
        //public static readonly Color WindowFrame = new Color(0xFF646464);
        public static readonly Color WindowText = new Color(0xFF000000);
    }
}
