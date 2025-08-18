using System.Text.Json.Nodes;
using System.Text.Json;
using DCSoft.Writer;
using DCSoft.Writer.Dom;
using DCSystemXml;
using DCSoft.Common;
using DCSoft.Writer.Data;
using DCSoft.Data;
using System.Reflection;
using DCSoft.Drawing;
using System.Collections.Generic;
using DCSoft.WinForms.Design;
using DCSoft.Printing;

namespace DCSoft.WASM
{
    /// <summary>
    /// 内部对象和JSON对象之间相互转换的代码
    /// </summary>
    public static class WASMJsonConvert
    {
        static WASMJsonConvert()
        {
            _JsonOptions = new JsonSerializerOptions();
            _JsonOptions.PropertyNameCaseInsensitive = true;
            _JsonOptions.WriteIndented = false;
            _JsonOptions.PropertyNamingPolicy = null;
            _JsonOptions.Converters.Add(new MyDateTimeConverter());
            _JsonOptions.Converters.Add(new MyColorConverter());
            _JsonOptions.Converters.Add(new MyBytesConverter());
            DCJsonSerialization.AddConverter(_JsonOptions);
            var c2 = new System.Text.Json.Serialization.JsonStringEnumConverter(new MyJsonNamingPolicy(), true);
            _JsonOptions.Converters.Add(c2);
        }
        //private static readonly List<int> _RuntimeHashCode = new List<int>();
        //public static void AddConverter()
        //{
        //    var rt = WASMJSRuntime.Instance;
        //    if (_RuntimeHashCode.Contains(rt.GetHashCode()))
        //    {
        //        return;
        //    }
        //    _RuntimeHashCode.Add(rt.GetHashCode());
        //    var opt = rt.JsonSerializerOptions;
        //    if (opt != null)
        //    {
        //        var field = opt.GetType().GetField("_isImmutable", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        //        if (field == null)
        //        {
        //            field = opt.GetType().GetField("_isReadOnly", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        //        }
        //        if (field != null)
        //        {
        //            field.SetValue(opt, false);
        //            opt.Converters.Add(new MyColorConverter());
        //            opt.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter(new MyJsonNamingPolicy(), true));
        //            opt.Converters.Add(new MyDateTimeConverter());
        //            DCJsonSerialization.AddConverter(opt);
        //            opt.PropertyNameCaseInsensitive = true;
        //            opt.PropertyNamingPolicy = null;
        //            field?.SetValue(opt, true);
        //        }
        //    }
        //}

        public static JsonSerializerOptions CreateJsonSerializerOptions(JSRuntime rst)
        {
            var opts = new JsonSerializerOptions
            {
                MaxDepth = 32,
                PropertyNamingPolicy = null,
                PropertyNameCaseInsensitive = true,
                Converters =
                {
                    new Microsoft.JSInterop.Infrastructure.DotNetObjectReferenceJsonConverterFactory(rst),
                    //new JSObjectReferenceJsonConverter(this),
                    //new JSStreamReferenceJsonConverter(this),
                    //new DotNetStreamReferenceJsonConverter(this),
                    new Microsoft.JSInterop.Infrastructure.ByteArrayJsonConverter(rst),
                    new MyColorConverter(),
                    new System.Text.Json.Serialization.JsonStringEnumConverter(new MyJsonNamingPolicy(), true),
                    new MyDateTimeConverter()
                }
            };
            DCJsonSerialization.AddConverter(opts);
            return opts;
        }

        private class MyJsonNamingPolicy : JsonNamingPolicy
        {
            public MyJsonNamingPolicy()
            {

            }
            public override string ConvertName(string name)
            {
                return name;
            }
        }
        private class MyBytesConverter : System.Text.Json.Serialization.JsonConverter<byte[]>
        {
            public override byte[] Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                byte[] result = null;
                if(reader.TryGetBytesFromBase64( out result ))
                {
                    return result;
                }
                else
                {
                    return null;
                }
            }
            public override void Write(Utf8JsonWriter writer, byte[] value, JsonSerializerOptions options)
            {
                if( value == null || value.Length == 0 )
                {
                    writer.WriteNullValue();
                }
                else
                {
                    writer.WriteBase64StringValue(value);
                }
            }
        }
        private class MyColorConverter : System.Text.Json.Serialization.JsonConverter<DCSystem_Drawing.Color>
        {
            public override DCSystem_Drawing.Color Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                var str = reader.GetString();
                if (str == null || str.Length == 0)
                {
                    return DCSystem_Drawing.Color.Empty;
                }
                else
                {
                    return DCSoft.Common.XMLSerializeHelper.StringToColor(str, DCSystem_Drawing.Color.Empty);
                    //return  ColorTranslator.FromHtml(str);
                }
            }

            public override void Write(Utf8JsonWriter writer, DCSystem_Drawing.Color value, JsonSerializerOptions options)
            {
                if (value.A != 255)
                {
                    writer.WriteStringValue(DCSoft.Common.XMLSerializeHelper.ColorToString(value));
                }
                else
                {
                    writer.WriteStringValue(DCSystem_Drawing.ColorTranslator.ToHtml(value));
                }
            }
        }
        private class MyDateTimeConverter : System.Text.Json.Serialization.JsonConverter<DateTime>
        {
            public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                var str = reader.GetString();
                if (str == null)
                {
                    return DateTime.MinValue;
                }
                else
                {
                    return DateTime.Parse(str);
                }
            }

            public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
            {
                writer.WriteStringValue(DCSoft.Common.DateTimeCommon.FastToYYYY_MM_DD_HH_MM_SS(value));
            }
        }
        public static readonly System.Text.Json.JsonSerializerOptions _JsonOptions = null;


        #region Font
        public static void JsonToFont(JsonElement parameter, XFontValue font, float defaultFontSize)
        {
            foreach (System.Text.Json.JsonProperty property in parameter.EnumerateObject())
            {
                string name = property.Name.ToLower();
                string value = WASMJsonConvert.parseJsonElementToString(property.Value);
                switch (name)
                {
                    case "fontfamily":
                        font.Name = value;
                        break;
                    case "fontsize":
                        font.Size = FontSizeInfo.GetFontSize(value, defaultFontSize);// this._Control.Document.DefaultStyle.FontSize);
                        break;
                    case "bold":
                        font.Bold = property.ConvertToBoolean(false);
                        break;
                    case "italic":
                        font.Italic = property.ConvertToBoolean(false);
                        break;
                    case "underline":
                        font.Underline = property.ConvertToBoolean(false);
                        break;
                    case "strikeout":
                        font.Strikeout = property.ConvertToBoolean(false);
                        break;
                    default:
                        break;
                }
            }
        }
        #endregion




        #region ValueValidateStyle
        public static ValueValidateStyle parseJSONToValidateStyle(JsonElement parameter, ValueValidateStyle thestyle)
        {
            ValueValidateStyle style = thestyle != null ? thestyle : new ValueValidateStyle();
            if (thestyle == null)
            {
                style.DateTimeMinValue = DateTime.MinValue;
                style.DateTimeMaxValue = DateTime.MinValue;
            }
            if (parameter.ValueKind == JsonValueKind.Null)
            {
                return null;
            }
            if (parameter.ValueKind != JsonValueKind.Object)
            {
                return null;
            }
            //解析对象格式的以后再慢慢补充
            foreach (System.Text.Json.JsonProperty property in parameter.EnumerateObject())
            {
                string name = property.Name.ToLower();
                string value = WASMJsonConvert.parseJsonElementToString(property.Value);
                double d = double.NaN;
                int i = int.MinValue;
                DateTime dt = DateTime.MinValue;

               
                switch (name)
                {
                    case "valuetype":
                        style.ValueType = property.ConvertToEnum<ValueTypeStyle>(ValueTypeStyle.Text);// WASMJsonConvert.parseEnumValue<ValueTypeStyle>(value, ValueTypeStyle.Text);
                        break;
                    case "maxdecimaldigits":
                        if (int.TryParse(value, out i) == true)
                        {
                            style.MaxDecimalDigits = i;
                        }
                        break;
                    case "maxlength":
                        if (int.TryParse(value, out i) == true)
                        {
                            style.MaxLength = i;
                        }
                        break;
                    case "minlength":
                        if (int.TryParse(value, out i) == true)
                        {
                            style.MinLength = i;
                        }
                        break;
                    case "maxvalue":
                        if (double.TryParse(value, out d) == true)
                        {
                            style.MaxValue = d;
                        }
                        break;
                    case "minvalue":
                        if (double.TryParse(value, out d) == true)
                        {
                            style.MinValue = d;
                        }
                        break;
                    case "checkdecimaldigits":
                        style.CheckDecimalDigits = property.ConvertToBoolean(style.CheckDecimalDigits); //WASMJsonConvert.parseBoolean(value, style.CheckDecimalDigits);
                        break;
                    case "checkmaxvalue":
                        style.CheckMaxValue = property.ConvertToBoolean(style.CheckMaxValue); //WASMJsonConvert.parseBoolean(value, style.CheckMaxValue);
                        break;
                    case "checkminvalue":
                        style.CheckMinValue = property.ConvertToBoolean(style.CheckMinValue); //WASMJsonConvert.parseBoolean(value, style.CheckMinValue);
                        break;
                    case "required":
                        style.Required = property.ConvertToBoolean(style.Required); //WASMJsonConvert.parseBoolean(value, style.Required);
                        break;
                    case "datetimemaxvalue":

                        if (DateTime.TryParse(value, out dt) == true)
                        {
                            //if (dt.Year == 0001)
                            //{
                            //    style.DateTimeMinValue = DateTime.MinValue;
                            //}
                            //else
                            //{
                            //    DateTimeFormatInfo dtFormat = new DateTimeFormatInfo();
                            //    dtFormat.ShortDatePattern = "yyyy/MM/dd HH:mm:ss";
                            //    dt = Convert.ToDateTime(value, dtFormat);

                            //    style.DateTimeMaxValue = dt;
                            //}
                            style.DateTimeMaxValue = dt;
                        }                       
                        break;
                    case "datetimeminvalue":
                        
                        if (DateTime.TryParse(value, out dt) == true)
                        {
                            //if (dt.Year == 0001)
                            //{
                            //    style.DateTimeMinValue = DateTime.MinValue;
                            //}
                            //else
                            //{
                            //    DateTimeFormatInfo dtFormat = new DateTimeFormatInfo();
                            //    dtFormat.ShortDatePattern = "yyyy/MM/dd HH:mm:ss";
                            //    dt = Convert.ToDateTime(value, dtFormat);

                            //    style.DateTimeMinValue = dt;
                            //}
                            style.DateTimeMinValue = dt;
                        }
                        break;
                    case "binarylength":
                        style.BinaryLength = property.ConvertToBoolean(style.BinaryLength);// WASMJsonConvert.parseBoolean(value, style.BinaryLength);
                        break;
                    case "valuename":
                        style.ValueName = value;
                        break;
                    case "requiredinvalidateflag":
                        style.RequiredInvalidateFlag = property.ConvertToBoolean(false);// WASMJsonConvert.parseBoolean(value, false);
                        break;
                    case "message":
                        style.Message = value;
                        break;
                    case "level":
                        style.Level = property.ConvertToEnum<ValueValidateLevel>(ValueValidateLevel.Info);// WASMJsonConvert.parseEnumValue<ValueValidateLevel>(value, ValueValidateLevel.Info);
                        break;
                    default:
                        break;
                }
            }
            return style;
        }
        public static JsonNode parseValidateStyleToJSON(ValueValidateStyle style)
        {
            if (style == null)
            {
                return null;
            }
            JsonObject jsObj = new JsonObject();
            jsObj.Add("BinaryLength", style.BinaryLength);
            jsObj.Add("CheckDecimalDigits", style.CheckDecimalDigits);
            jsObj.Add("CheckMaxValue", style.CheckMaxValue);
            jsObj.Add("CheckMinValue", style.CheckMinValue);
            jsObj.Add("DateTimeMaxValue", style.DateTimeMaxValue.ToString("yyyy-MM-dd HH:mm:ss"));
            jsObj.Add("DateTimeMinValue", style.DateTimeMinValue.ToString("yyyy-MM-dd HH:mm:ss"));
            jsObj.Add("Level", style.Level.ToString());
            jsObj.Add("MaxDecimalDigits", style.MaxDecimalDigits);
            jsObj.Add("MaxLength", style.MaxLength);
            jsObj.Add("MaxValue", double.IsNaN(style.MaxValue) ? null : style.MaxValue);
            jsObj.Add("Message", style.Message);
            jsObj.Add("MinLength", style.MinLength);
            jsObj.Add("MinValue", double.IsNaN(style.MinValue) ? null : style.MinValue);
            jsObj.Add("Required", style.Required);
            jsObj.Add("RequiredInvalidateFlag", style.RequiredInvalidateFlag);
            jsObj.Add("ValueName", style.ValueName);
            jsObj.Add("ValueType", style.ValueType.ToString());
            return jsObj;
        }
        #endregion


        #region InputFieldListItems
        public static ListItemCollection parseJSONToListItems(JsonElement parameter)
        {
            ListItemCollection lic = new ListItemCollection();
            if (parameter.ValueKind != JsonValueKind.Array)
            {
                return null;
            }
            foreach (JsonElement item in parameter.EnumerateArray())
            {
                if (item.ValueKind != JsonValueKind.Object)
                {
                    continue;
                }
                ListItem li = new ListItem();
                foreach (JsonProperty property in item.EnumerateObject())
                {
                    string name = property.Name.ToLower();
                    string value = property.ConvertToString();
                    switch (name)
                    {
                        case "text":
                            li.Text = value;
                            //hasvalue = true;
                            break;
                        case "value":
                            li.Value = value;
                            //hasvalue = true;
                            break;
                        case "textinlist":
                            li.TextInList = value;
                            //hasvalue = true;
                            break;
                    }
                }
                //if (hasvalue)
                {
                    lic.Add(li);
                }
            }
            if (lic.Count > 0)
            {
                return lic;
            }
            else
            {
                return null;
            }
        }
        public static JsonNode parseListItemsToJSON(ListItemCollection lic)
        {
            if (lic == null)
            {
                return null;
            }
            JsonArray array = new JsonArray();
            foreach (ListItem li in lic)
            {
                JsonObject jsObj = new JsonObject();
                jsObj.Add("Text", li.Text);
                //jsObj.Add("Text2", li.Text2);
                if (string.IsNullOrEmpty(li.TextInList) == false)
                {
                    jsObj.Add("TextInList", li.TextInList);
                }
                if (string.IsNullOrEmpty(li.Value) == false)
                {
                    jsObj.Add("Value", li.Value);
                }
                array.Add(jsObj);
            }
            return array;
        }
        #endregion

        #region ValueFormatter
        public static ValueFormater parseJSONToValueFormater(JsonElement parameter)
        {
            ValueFormater vf = new ValueFormater();
            bool hasvalue = false;
            if (parameter.ValueKind != JsonValueKind.Object)
            {
                return null;
            }
            foreach (System.Text.Json.JsonProperty property in parameter.EnumerateObject())
            {
                string name = property.Name.ToLower();
                string value = property.Value.ValueKind == JsonValueKind.String ? property.Value.GetString() : null;
                //float f = float.NaN;
                switch (name)
                {
                    case "format":
                        vf.Format = value;
                        hasvalue = true;
                        break;
                    case "nonetext":
                        vf.NoneText = value;
                        hasvalue = true;
                        break;
                    case "style":
                        vf.Style = WASMUtils.ConvertToEnum<ValueFormatStyle>(property, ValueFormatStyle.None);// WASMJsonConvert.parseEnumValue<ValueFormatStyle>(value, ValueFormatStyle.None);
                        hasvalue = true;
                        break;
                    default:
                        break;
                }
            }
            if (hasvalue == true)
            {
                return vf;
            }
            else
            {
                return null;
            }
        }
        public static JsonNode parseValueFormaterToJSON(ValueFormater vf)
        {
            if (vf == null)
            {
                return null;
            }
            JsonObject jsObj = new JsonObject();
            jsObj.Add("NoneText", vf.NoneText);
            jsObj.Add("Format", vf.Format);
            jsObj.Add("Style", vf.Style.ToString());
            return jsObj;
        }
        #endregion




        #region DocumentContentStyle 以后慢慢补
        public static string ParseColorToRGBAString(Color c)
        {
            //if (c.A != byte.MaxValue)
            //{
            //    return "rgba(" + c.R + "," + c.G + "," + c.B + "," + ((double)(int)c.A / 255.0).ToString("0.0000") + ")";
            //}
            if (c == Color.Black)
            {
                return "#000000FF";
            }
            else if (c == Color.White)
            {
                return "#FFFFFFFF";
            }
            else if(c == Color.Transparent)
            {
                return "#FFFFFF00";
            }
            else if( c == Color.Empty)
            {
                return "#00000000";
            }
            else
            {
                return $"#{c.R:X2}{c.G:X2}{c.B:X2}{c.A:X2}";
            }
        }
        public static Color ParseRGBAStringToColor(string htmlColor)
        {
            if (htmlColor != null && htmlColor.StartsWith("#") && htmlColor.Length == 9)
            {
                Color c = Color.FromArgb(
                    Convert.ToInt32(htmlColor.Substring(7, 2), 16),
                    Convert.ToInt32(htmlColor.Substring(1, 2), 16),
                    Convert.ToInt32(htmlColor.Substring(3, 2), 16),
                    Convert.ToInt32(htmlColor.Substring(5, 2), 16));
                return c;
            }
            return ColorTranslator.FromHtml(htmlColor);
        }
        public static JsonNode DocumentContentStyleToJSON(DocumentContentStyle style)
        {
            if(style == null)
            {
                return null;
            }
            JsonObject obj = new JsonObject();
            obj.Add("Bold", style.Bold);
            obj.Add("ColorString", ParseColorToRGBAString(style.Color));
            obj.Add("VerticalAlign", style.VerticalAlign.ToString());
            obj.Add("Align", style.Align.ToString());
            obj.Add("BorderTop", style.BorderTop);
            obj.Add("BorderRight", style.BorderRight);
            obj.Add("BorderLeft", style.BorderLeft);
            obj.Add("BorderBottom", style.BorderBottom);
            obj.Add("BorderBottomColorString", ParseColorToRGBAString(style.BorderBottomColor));
            obj.Add("BorderRightColorString", ParseColorToRGBAString(style.BorderRightColor));
            obj.Add("BorderLeftColorString", ParseColorToRGBAString(style.BorderLeftColor));
            obj.Add("BorderTopColorString", ParseColorToRGBAString(style.BorderTopColor));
            obj.Add("BorderStyle", style.BorderStyle.ToString());
            obj.Add("BorderWidth", style.BorderWidth);
            obj.Add("PaddingBottom", style.PaddingBottom);
            obj.Add("PaddingLeft", style.PaddingLeft);
            obj.Add("PaddingRight", style.PaddingRight);
            obj.Add("PaddingTop", style.PaddingTop);
            obj.Add("BackgroundColorString", ParseColorToRGBAString(style.BackgroundColor));
            obj.Add("LineSpacing", style.LineSpacing);
            obj.Add("LeftIndent", style.LeftIndent);
            obj.Add("LineSpacingStyle", style.LineSpacingStyle.ToString());
            obj.Add("SpacingAfterParagraph", style.SpacingAfterParagraph);
            obj.Add("SpacingBeforeParagraph", style.SpacingBeforeParagraph);
            obj.Add("FirstLineIndent", style.FirstLineIndent);
            obj.Add("ParagraphOutlineLevel", style.ParagraphOutlineLevel);
            obj.Add("ParagraphMultiLevel", style.ParagraphMultiLevel);
            obj.Add("ParagraphListStyle", style.ParagraphListStyle.ToString());
            obj.Add("FontStyle", style.FontStyle.ToString());
            obj.Add("FontHeight", style.FontHeight);
            obj.Add("FontName", style.FontName);
            obj.Add("FontSize", style.FontSize);
            obj.Add("Underline", style.Underline);
            obj.Add("Italic", style.Italic);
            obj.Add("Strikeout", style.Strikeout);
            obj.Add("Index", style.Index);
            return obj;
        }
        public static DocumentContentStyle JSONToDocumentContentStyle(JsonElement parameter, DocumentContentStyle style, out bool changed)
        {
            changed = false;
            Color cc = Color.Black;
            if(parameter.ValueKind != JsonValueKind.Object)
            {
                return null;
            }
            DocumentContentStyle resultstyle = style != null ? (DocumentContentStyle)style.Clone() : new DocumentContentStyle();
            foreach(JsonProperty property in parameter.EnumerateObject())
            {
                string name = property.Name.ToLower();
                string value = parseJsonElementToString(property.Value);
                int i = int.MinValue;
                float f = float.NaN;
                switch(name)
                {
                    case "strikeout":
                        resultstyle.Strikeout = property.ConvertToBoolean(false);
                        if (style.Strikeout != resultstyle.Strikeout)
                        {
                            changed = true;
                        }
                        break;
                    case "underline":
                        resultstyle.Underline = property.ConvertToBoolean(false);
                        if (style.Underline != resultstyle.Underline)
                        {
                            changed = true;
                        }
                        break;
                    case "italic":
                        resultstyle.Italic = property.ConvertToBoolean(false);
                        if (style.Italic != resultstyle.Italic)
                        {
                            changed = true;
                        }
                        break;
                    case "bold":
                        resultstyle.Bold = property.ConvertToBoolean(false);
                        if (style.Bold != resultstyle.Bold)
                        {
                            changed = true;
                        }
                        break;
                    case "colorstring":
                        if (value == null || value.Length == 0)
                        {
                            break;
                        }
                        cc = ParseRGBAStringToColor(value);
                        if (resultstyle.Color != cc)
                        {
                            resultstyle.Color = cc;
                            changed = true;
                        }
                        break;
                    case "verticalalign":
                        resultstyle.VerticalAlign = parseEnumValue<VerticalAlignStyle>(value, VerticalAlignStyle.Top);
                        if (style.VerticalAlign != resultstyle.VerticalAlign)
                        {
                            changed = true;
                        }
                        break;
                    case "align":
                        resultstyle.Align = parseEnumValue<DocumentContentAlignment>(value, DocumentContentAlignment.Left);
                        if (style.Align != resultstyle.Align)
                        {
                            changed = true;
                        }
                        break;
                    case "bordertop":
                        resultstyle.BorderTop = property.ConvertToBoolean(false);
                        if (style.BorderTop != resultstyle.BorderTop)
                        {
                            changed = true;
                        }
                        break;
                    case "borderleft":
                        resultstyle.BorderLeft = property.ConvertToBoolean(false);
                        if (style.BorderLeft != resultstyle.BorderLeft)
                        {
                            changed = true;
                        }
                        break;
                    case "borderright":
                        resultstyle.BorderRight = property.ConvertToBoolean(false);
                        if (style.BorderRight != resultstyle.BorderRight)
                        {
                            changed = true;
                        }
                        break;
                    case "borderbottom":
                        resultstyle.BorderBottom = property.ConvertToBoolean(false);
                        if (style.BorderBottom != resultstyle.BorderBottom)
                        {
                            changed = true;
                        }
                        break;
                    case "borderrightcolorstring":
                        if (value == null || value.Length == 0)
                        {
                            break;
                        }
                        cc = ParseRGBAStringToColor(value);
                        if (resultstyle.BorderRightColor != cc)
                        {
                            resultstyle.BorderRightColor = cc;
                            changed = true;
                        }
                        break;
                    case "borderleftcolorstring":
                        if (value == null || value.Length == 0)
                        {
                            break;
                        }
                        cc = ParseRGBAStringToColor(value);
                        if (resultstyle.BorderLeftColor != cc)
                        {
                            resultstyle.BorderLeftColor = cc;
                            changed = true;
                        }
                        break;
                    case "bordertopcolorstring":
                        if (value == null || value.Length == 0)
                        {
                            break;
                        }
                        cc = ParseRGBAStringToColor(value);
                        if (resultstyle.BorderTopColor != cc)
                        {
                            resultstyle.BorderTopColor = cc;
                            changed = true;
                        }
                        break;
                    case "borderbottomcolorstring":
                        if (value == null || value.Length == 0)
                        {
                            break;
                        }
                        cc = ParseRGBAStringToColor(value);
                        if (resultstyle.BorderBottomColor != cc)
                        {
                            resultstyle.BorderBottomColor = cc;
                            changed = true;
                        }
                        break;
                    case "borderrightcolor":
                        if (value == null || value.Length == 0)
                        {
                            break;
                        }
                        cc = ParseRGBAStringToColor(value);
                        if (resultstyle.BorderRightColor != cc)
                        {
                            resultstyle.BorderRightColor = cc;
                            changed = true;
                        }
                        break;
                    case "borderleftcolor":
                        if (value == null || value.Length == 0)
                        {
                            break;
                        }
                        cc = ParseRGBAStringToColor(value);
                        if (resultstyle.BorderLeftColor != cc)
                        {
                            resultstyle.BorderLeftColor = cc;
                            changed = true;
                        }
                        break;
                    case "bordertopcolor":
                        if (value == null || value.Length == 0)
                        {
                            break;
                        }
                        cc = ParseRGBAStringToColor(value);
                        if (resultstyle.BorderTopColor != cc)
                        {
                            resultstyle.BorderTopColor = cc;
                            changed = true;
                        }
                        break;
                    case "borderbottomcolor":
                        if (value == null || value.Length == 0)
                        {
                            break;
                        }
                        cc = ParseRGBAStringToColor(value);
                        if (resultstyle.BorderBottomColor != cc)
                        {
                            resultstyle.BorderBottomColor = cc;
                            changed = true;
                        }
                        break;
                    case "borderstyle":
                        resultstyle.BorderStyle = parseEnumValue<DashStyle>(value, DashStyle.Solid);
                        if (style.BorderStyle != resultstyle.BorderStyle)
                        {
                            changed = true;
                        }
                        break;
                    case "borderwidth":
                        if(float.TryParse(value,out f ))
                        {
                            resultstyle.BorderWidth = f;
                            if (style.BorderWidth != resultstyle.BorderWidth)
                            {
                                changed = true;
                            }
                        }
                        break;
                    case "paddingbottom":
                        if (float.TryParse(value, out f))
                        {
                            resultstyle.PaddingBottom = f;
                            if (style.PaddingBottom != resultstyle.PaddingBottom)
                            {
                                changed = true;
                            }
                        }
                        break;
                    case "paddingleft":
                        if (float.TryParse(value, out f))
                        {
                            resultstyle.PaddingLeft = f;
                            if (style.PaddingLeft != resultstyle.PaddingLeft)
                            {
                                changed = true;
                            }
                        }
                        break;
                    case "paddingright":
                        if (float.TryParse(value, out f))
                        {
                            resultstyle.PaddingRight = f;
                            if (style.PaddingRight != resultstyle.PaddingRight)
                            {
                                changed = true;
                            }
                        }
                        break;
                    case "paddingtop":
                        if (float.TryParse(value, out f))
                        {
                            resultstyle.PaddingTop = f;
                            if (style.PaddingTop != resultstyle.PaddingTop)
                            {
                                changed = true;
                            }
                        }
                        break;
                    case "backgroundcolorstring":
                        if (value == null || value.Length == 0)
                        {
                            break;
                        }
                        cc = ParseRGBAStringToColor(value);
                        if (resultstyle.BackgroundColor != cc)
                        {
                            resultstyle.BackgroundColor = cc;
                            changed = true;
                        }
                        break;

                    case "linespacing":
                        if (float.TryParse(value, out f))
                        {
                            resultstyle.LineSpacing = f;
                            if (style.LineSpacing != resultstyle.LineSpacing)
                            {
                                changed = true;
                            }
                        }
                        break;
                    case "leftindent":
                        if (float.TryParse(value, out f))
                        {
                            resultstyle.LeftIndent = f;
                            if (style.LeftIndent != resultstyle.LeftIndent)
                            {
                                changed = true;
                            }
                        }
                        break;
                    case "linespacingstyle":
                        resultstyle.LineSpacingStyle = parseEnumValue<LineSpacingStyle>(value, LineSpacingStyle.SpaceSingle);
                        if (style.LineSpacingStyle != resultstyle.LineSpacingStyle)
                        {
                            changed = true;
                        }
                        break;
                    case "spacingafterparagraph":
                        if (float.TryParse(value, out f))
                        {
                            resultstyle.SpacingAfterParagraph = f;
                            if (style.SpacingAfterParagraph != resultstyle.SpacingAfterParagraph)
                            {
                                changed = true;
                            }
                        }
                        break;
                    case "spacingbeforeparagraph":
                        if (float.TryParse(value, out f))
                        {
                            resultstyle.SpacingBeforeParagraph = f;
                            if (style.SpacingBeforeParagraph != resultstyle.SpacingBeforeParagraph)
                            {
                                changed = true;
                            }
                        }
                        break;
                    case "firstlineindent":
                        if (float.TryParse(value, out f))
                        {
                            resultstyle.FirstLineIndent = f;
                            if (style.FirstLineIndent != resultstyle.FirstLineIndent)
                            {
                                changed = true;
                            }
                        }
                        break;
                    case "paragraphoutlinelevel":
                        if (int.TryParse(value, out i))
                        {
                            resultstyle.ParagraphOutlineLevel = i;
                            if (style.ParagraphOutlineLevel != resultstyle.ParagraphOutlineLevel)
                            {
                                changed = true;
                            }
                        }
                        break;
                    case "paragraphmultilevel":
                        resultstyle.ParagraphMultiLevel = property.ConvertToBoolean(false);
                        if (style.ParagraphMultiLevel != resultstyle.ParagraphMultiLevel)
                        {
                            changed = true;
                        }
                        break;
                    case "paragraphliststyle":
                        resultstyle.ParagraphListStyle = parseEnumValue<ParagraphListStyle>(value, ParagraphListStyle.None);
                        if (style.ParagraphListStyle != resultstyle.ParagraphListStyle)
                        {
                            changed = true;
                        }
                        break;
                    case "fontstyle":
                        resultstyle.FontStyle = parseEnumValue<FontStyle>(value, FontStyle.Regular);
                        if (style.FontStyle != resultstyle.FontStyle)
                        {
                            changed = true;
                        }
                        break;
                    case "fontname":
                        resultstyle.FontName = value;
                        if (style.FontName != resultstyle.FontName)
                        {
                            changed = true;
                        }
                        break;
                    case "fontsize":
                        if(float.TryParse(value, out f) == true)
                        {
                            resultstyle.FontSize = f;
                            if (style.FontSize != resultstyle.FontSize)
                            {
                                changed = true;
                            }
                        }
                        break;
                    default:
                        break;
                }
            }
            return resultstyle;
        }
        #endregion

         


        #region 杂项转换
        public static string parseJsonElementToString(JsonElement parameter)
        {
            string value = null;
            switch (parameter.ValueKind)
            {
                case JsonValueKind.String:
                    value = parameter.GetString();
                    break;
                case JsonValueKind.Null:
                    break;
                case JsonValueKind.False:
                    value = "false";
                    break;
                case JsonValueKind.True:
                    value = "true";
                    break;
                case JsonValueKind.Number:
                    value = parameter.GetSingle().ToString();
                    break;
                case JsonValueKind.Undefined:
                    break;
                case JsonValueKind.Object:
                    break;
                case JsonValueKind.Array:
                    break;
            }
            return value;
        }


        //我抄抄抄抄抄抄抄
        public static T parseEnumValue<T>(string v, T defaultValue) where T : System.Enum
        {
            if (v == null || v.Length == 0 || v == "null")
            {
                return defaultValue;
            }
            try
            {
                return DCEnumHelper<T>.StringToValue(v);
            }
            catch (System.Exception ext)
            {
                return defaultValue;
            }
        }
        public static bool parseBoolean(object v, bool db)
        {
            bool b = db;

            if (v == null)
            {
                return db;
            }

            if (v is string && System.Boolean.TryParse((string)v, out b) == true)
            {
                return b;
            }

            if (v is JsonElement)
            {
                JsonElement ele = (JsonElement)v;
                if (ele.ValueKind == JsonValueKind.True ||
                  (ele.ValueKind == JsonValueKind.String && ele.GetString().ToLower() == "true"))
                {
                    return true;
                }
                else if (ele.ValueKind == JsonValueKind.False ||
                  (ele.ValueKind == JsonValueKind.String && ele.GetString().ToLower() == "false"))
                {
                    return false;
                }
            }

            return b;
        }
        #endregion
    }
}
