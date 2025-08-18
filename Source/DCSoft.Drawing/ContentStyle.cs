using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using DCSoft.Common;
using System.Runtime.InteropServices;

namespace DCSoft.Drawing
{
    /// <summary>
    /// 样式对象
    /// </summary>
    /// <remarks>编制 袁永福</remarks>
    public abstract partial class ContentStyle :
        XDependencyObject,
        IDisposable
    {
        public static void VoidMethod()
        {

        }
        /// <summary>
        /// 初始化对象
        /// </summary>
        public ContentStyle()
        {
            //this.FontName = DefaultFontName;
            //this.FontSize = DefaultFontSize;
        }

        /// <summary>
        /// 对象数据是否为空
        /// </summary>
        public virtual bool IsEmpty
        {
            get
            {
                return this.InnerValues == null || this.InnerValues.Count == 0;
            }
        }

        [NonSerialized]
        private ContentStyle _RuntimeStyle = null;
        /// <summary>
        /// 运行时使用的样式列表
        /// </summary>
        public ContentStyle RuntimeStyle
        {
            get
            {
                return _RuntimeStyle;
            }
            set
            {
                _RuntimeStyle = value;
            }
        }
        /// <summary>
        /// 比较数值
        /// </summary>
        /// <param name="style"></param>
        /// <returns></returns>

        public bool EqualsStyleValue(ContentStyle style)
        {
            if (this._InnerValues.Count != style._InnerValues.Count)
            {
                // 个数都不一致，肯定不相等。
                return false;
            }
            if (this.GetValueHashCode() != style.GetValueHashCode())
            {
                return false;
            }
            foreach (var item in this._InnerValues)
            {
                object v2 = null;
                if (style._InnerValues.TryGetValue(item.Key, out v2))
                {
                    if (object.Equals(item.Value, v2) == false)
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            return true;
        }


        /// <summary>
        /// 删除与默认样式相同的项目，只保留不同的项目
        /// </summary>
        /// <param name="defaultStyle">默认样式对象</param>
        /// <returns>经过操作后本对象剩余的项目</returns>
        public int RemoveSameStyle(ContentStyle defaultStyle)
        {
            if (defaultStyle == null)
            {
                throw new ArgumentNullException("defaultStyle");
            }
            bool changed = false;
            foreach (XDependencyProperty p in defaultStyle.InnerValues.Keys)
            {
                if (this.InnerValues.ContainsKey(p))
                {
                    object v1 = defaultStyle.InnerValues[p];
                    object v2 = this.InnerValues[p];
                    if (v1 == v2)
                    {
                        this.InnerValues.Remove(p);
                        changed = true;
                    }
                }
            }
            if (changed)
            {
                OnValueChanged(null);
            }
            return this.InnerValues.Count;
        }

        private int _Index = -1;
        /// <summary>
        /// 内部编号，本属性供内部使用，没有意义，也不做任何参考
        /// </summary>
        public int Index
        {
            get
            {
                return _Index;
            }
            set
            {
                _Index = value;
            }
        }

        #region 背景和颜色

        /// <summary>
        /// BackgroundColor属性名
        /// </summary>
        public const string PropertyName_BackgroundColor = "BackgroundColor";

        public static readonly XDependencyProperty _backgroundColor = XDependencyProperty.Register(
            ContentStylePropertyIDConsts.ID_BackgroundColor,
            PropertyName_BackgroundColor,
            typeof(DCSystem_Drawing.Color),
            typeof(ContentStyle),
            DCSystem_Drawing.Color.Transparent);
        /// <summary>
        /// 背景色
        /// </summary>
        public DCSystem_Drawing.Color BackgroundColor
        {
            get
            {
                return (DCSystem_Drawing.Color)GetValue(_backgroundColor);
            }
            set
            {
                SetValue(_backgroundColor, value);
            }
        }

        /// <summary>
        /// 字符串格式的背景色
        /// </summary>
        public string BackgroundColorString
        {
            get
            {
                return XMLSerializeHelper.ColorToString(this.BackgroundColor, DCSystem_Drawing.Color.Transparent);
            }
            set
            {
                this.BackgroundColor = XMLSerializeHelper.StringToColor(value, DCSystem_Drawing.Color.Transparent);
            }
        }


        /// <summary>
        /// Color属性名
        /// </summary>
        public const string PropertyName_Color = "Color";


        internal static readonly XDependencyProperty _Key_Color = XDependencyProperty.Register(
            ContentStylePropertyIDConsts.ID_Color,
            PropertyName_Color,
            typeof(DCSystem_Drawing.Color),
            typeof(ContentStyle),
            DCSystem_Drawing.Color.Black);
        /// <summary>
        /// 颜色
        /// </summary>
        public DCSystem_Drawing.Color Color
        {
            get
            {
                return (DCSystem_Drawing.Color)GetValue(_Key_Color);
            }
            set
            {
                SetValue(_Key_Color, value);
            }
        }

        /// <summary>
        /// 字符串格式的对象颜色
        /// </summary>

        public string ColorString
        {
            get
            {
                return XMLSerializeHelper.ColorToString(this.Color, DCSystem_Drawing.Color.Black);
            }
            set
            {
                this.Color = XMLSerializeHelper.StringToColor(value, DCSystem_Drawing.Color.Black);
            }
        }


        #endregion

        #region 字体和文本

        /// <summary>
        /// 字体对象
        /// </summary>
        public XFontValue Font
        {
            get
            {
                XFontValue f = new XFontValue();
                foreach (var item in base._InnerValues)
                {
                    if (item.Key == _FontName)
                    {
                        f.Name = (string)item.Value;
                    }
                    else if (item.Key == _FontSize)
                    {
                        var v3 = (float)item.Value;
                        if (v3 > 0)
                        {
                            f.Size = v3;
                        }
                    }
                    else if (item.Key == _Bold)
                    {
                        f.Bold = (bool)item.Value;
                    }
                    else if (item.Key == _Italic)
                    {
                        f.Italic = (bool)item.Value;
                    }
                    else if (item.Key == _Underline)
                    {
                        f.Underline = (bool)item.Value;
                    }
                    else if (item.Key == _Strikeout)
                    {
                        f.Strikeout = (bool)item.Value;
                    }
                }
                //string fn = this.FontName;
                //if ( fn != null && fn.Length >0 )// string.IsNullOrEmpty( fn ) == false )
                //{
                //    f.Name = fn;
                //}
                //float fs = this.FontSize;
                //if (fs > 0)
                //{
                //    f.Size = fs;
                //}
                //f.Bold = this.Bold;
                //f.Italic = this.Italic;
                //f.Underline = this.Underline;
                //f.Strikeout = this.Strikeout;
                return f;
            }
            set
            {
                if (value != null)
                {
                    this.FontName = value.Name;
                    this.FontSize = value.Size;
                    this.Bold = value.Bold;
                    this.Italic = value.Italic;
                    this.Underline = value.Underline;
                    this.Strikeout = value.Strikeout;
                }
            }
        }

        /// <summary>
        /// 默认字体名称
        /// </summary>
        public static string DefaultFontName
            = XFontValue.DefaultFontName;
        /// <summary>
        /// 默认字体大小
        /// </summary>
        public static float DefaultFontSize
            = XFontValue.DefaultFontSize;

        /// <summary>
        /// FontName属性名
        /// </summary>
        public const string PropertyName_FontName = "FontName";

        private readonly static XDependencyProperty _FontName = XDependencyProperty.Register(
            ContentStylePropertyIDConsts.ID_FontName,
            PropertyName_FontName,
            typeof(string),
            typeof(ContentStyle),
            null);
        /// <summary>
        /// 字体名称
        /// </summary>
        public string FontName
        {
            get
            {
                string name = (string)GetValue(_FontName);
                if (string.IsNullOrEmpty(name))
                {
                    return XFontValue.DefaultFontName;
                }
                //if (string.IsNullOrEmpty(name))
                //{
                //    SetValue(_FontName, DefaultFontName);
                //    return DefaultFontName;
                //}
                //else
                //{
                return name;
                //}
            }
            set
            {
                if (value != null)
                {
                    value = string.Intern(value);
                }
                SetValue(_FontName, value);
            }
        }
        /// <summary>
        /// FontSize属性名
        /// </summary>
        public const string PropertyName_FontSize = "FontSize";


        private readonly static XDependencyProperty _FontSize = XDependencyProperty.Register(
            ContentStylePropertyIDConsts.ID_FontSize,
            PropertyName_FontSize,
            typeof(float),
            typeof(ContentStyle),
            0f);
        /// <summary>
        /// 字体大小
        /// </summary>
        public float FontSize
        {
            get
            {
                float v = (float)GetValue(_FontSize);
                //if (v == 0f)
                //{
                //    return DefaultFontSize;
                //}
                //else
                {
                    return v;
                }
                //object v = GetValue(_FontSize);
                //if (v == null)
                //{
                //    return 9f;
                //}
                //else
                //{
                //    return (float)v;
                //}
            }
            set
            {
                SetValue(_FontSize, value);
            }
        }

        /// <summary>
        /// Bold属性名
        /// </summary>
        public const string PropertyName_Bold = "Bold";


        private readonly static XDependencyProperty _Bold = XDependencyProperty.Register(
            ContentStylePropertyIDConsts.ID_Bold,
            PropertyName_Bold,
            typeof(bool),
            typeof(ContentStyle),
            false);
        /// <summary>
        /// 粗体
        /// </summary>
        public bool Bold
        {
            get
            {
                return (bool)GetValue(_Bold);
            }
            set
            {
                SetValue(_Bold, value);
            }
        }

        /// <summary>
        /// Italic属性名
        /// </summary>
        public const string PropertyName_Italic = "Italic";

        private readonly static XDependencyProperty _Italic = XDependencyProperty.Register(
            ContentStylePropertyIDConsts.ID_Italic,
            PropertyName_Italic,
            typeof(bool),
            typeof(ContentStyle),
            false);
        /// <summary>
        /// 斜体
        /// </summary>
        public bool Italic
        {
            get
            {
                return (bool)GetValue(_Italic);
            }
            set
            {
                SetValue(_Italic, value);
            }
        }

        /// <summary>
        /// 字体样式
        /// </summary>
        public FontStyle FontStyle
        {
            get
            {
                if (this._InnerValues == null)
                {
                    return FontStyle.Regular;
                }
                FontStyle style = FontStyle.Regular;
                object v = null;
                if (this._InnerValues.TryGetValue(_Bold, out v) && (bool)v)
                {
                    style = style | FontStyle.Bold;
                }
                if (this._InnerValues.TryGetValue(_Italic, out v) && (bool)v) //if (this.Italic)
                {
                    style = style | FontStyle.Italic;
                }
                if (this._InnerValues.TryGetValue(_Underline, out v) && (bool)v) //if (this.Underline)
                {
                    style = style | FontStyle.Underline;
                }
                if (this._InnerValues.TryGetValue(_Strikeout, out v) && (bool)v) //if (this.Strikeout)
                {
                    style = style | FontStyle.Strikeout;
                }
                return style;
            }
            set
            {
                this.Bold = ((value & FontStyle.Bold) == FontStyle.Bold);
                this.Italic = ((value & FontStyle.Italic) == FontStyle.Italic);
                this.Underline = ((value & FontStyle.Underline) == FontStyle.Underline);
                this.Strikeout = ((value & FontStyle.Strikeout) == FontStyle.Strikeout);
            }
        }

        /// <summary>
        /// Underline属性名
        /// </summary>
        public const string PropertyName_Underline = "Underline";

        private readonly static XDependencyProperty _Underline = XDependencyProperty.Register(
            ContentStylePropertyIDConsts.ID_Underline,
            PropertyName_Underline,
            typeof(bool),
            typeof(ContentStyle),
            false);
        /// <summary>
        /// 下划线
        /// </summary>
        public bool Underline
        {
            get
            {
                return (bool)GetValue(_Underline);
            }
            set
            {
                SetValue(_Underline, value);
            }
        }

        /// <summary>
        /// Strikeout属性名
        /// </summary>
        public const string PropertyName_Strikeout = "Strikeout";


        private readonly static XDependencyProperty _Strikeout = XDependencyProperty.Register(
            ContentStylePropertyIDConsts.ID_Strikeout,
            PropertyName_Strikeout,
            typeof(bool),
            typeof(ContentStyle),
            false);
        /// <summary>
        /// 删除线
        /// </summary>
        public bool Strikeout
        {
            get
            {
                return (bool)GetValue(_Strikeout);
            }
            set
            {
                SetValue(_Strikeout, value);
            }
        }
        /// <summary>
        /// Superscript属性名
        /// </summary>
        public const string PropertyName_Superscript = "Superscript";


        private readonly static XDependencyProperty _Superscript = XDependencyProperty.Register(
            ContentStylePropertyIDConsts.ID_Superscript,
            PropertyName_Superscript,
            typeof(bool),
            typeof(ContentStyle),
            false);
        /// <summary>
        /// 上标样式
        /// </summary>
        public bool Superscript
        {
            get
            {
                return (bool)GetValue(_Superscript);
            }
            set
            {
                SetValue(_Superscript, value);
            }
        }

        /// <summary>
        /// Subscript属性名
        /// </summary>
        public const string PropertyName_Subscript = "Subscript";


        private readonly static XDependencyProperty _Subscript = XDependencyProperty.Register(
            ContentStylePropertyIDConsts.ID_Subscript,
            PropertyName_Subscript,
            typeof(bool),
            typeof(ContentStyle),
            false);
        /// <summary>
        /// 下标样式
        /// </summary>
        public bool Subscript
        {
            get
            {
                return (bool)GetValue(_Subscript);
            }
            set
            {
                SetValue(_Subscript, value);
            }
        }

        /// <summary>
        /// SpacingAfterParagraph属性名
        /// </summary>
        public const string PropertyName_SpacingAfterParagraph = "SpacingAfterParagraph";

        private readonly static XDependencyProperty _SpacingAfterParagraph = XDependencyProperty.Register(
           ContentStylePropertyIDConsts.ID_SpacingAfterParagraph,
            PropertyName_SpacingAfterParagraph,
           typeof(float),
           typeof(ContentStyle),
           0f);
        /// <summary>
        /// 段落后间距
        /// </summary>
        public float SpacingAfterParagraph
        {
            get
            {
                return (float)GetValue(_SpacingAfterParagraph);
            }
            set
            {
                SetValue(_SpacingAfterParagraph, Math.Max(0, value));
            }
        }

        /// <summary>
        /// SpacingBeforeParagraph属性名
        /// </summary>
        public const string PropertyName_SpacingBeforeParagraph = "SpacingBeforeParagraph";

        private readonly static XDependencyProperty _SpacingBeforeParagraph = XDependencyProperty.Register(
           ContentStylePropertyIDConsts.ID_SpacingBeforeParagraph,
            PropertyName_SpacingBeforeParagraph,
           typeof(float),
           typeof(ContentStyle),
           0f);
        /// <summary>
        /// 段落前间距
        /// </summary>
        public float SpacingBeforeParagraph
        {
            get
            {
                return (float)GetValue(_SpacingBeforeParagraph);
            }
            set
            {
                SetValue(_SpacingBeforeParagraph, Math.Max(0, value));
            }
        }

        /// <summary>
        /// LineSpacingStyle属性名
        /// </summary>
        public const string PropertyName_LineSpacingStyle = "LineSpacingStyle";


        private readonly static XDependencyProperty _LineSpacingStyle = XDependencyProperty.Register(
           ContentStylePropertyIDConsts.ID_LineSpacingStyle,
            PropertyName_LineSpacingStyle,
           typeof(LineSpacingStyle),
           typeof(ContentStyle),
           LineSpacingStyle.SpaceSingle);
        /// <summary>
        /// 行间距样式，它决定了LineSpacing属性值的计量单位。
        /// </summary>
        public LineSpacingStyle LineSpacingStyle
        {
            get
            {
                return (LineSpacingStyle)GetValue(_LineSpacingStyle);
            }
            set
            {
                SetValue(_LineSpacingStyle, value);
            }
        }

        /// <summary>
        /// LineSpacing属性名
        /// </summary>
        public const string PropertyName_LineSpacing = "LineSpacing";

        private readonly static XDependencyProperty _LineSpacing = XDependencyProperty.Register(
           ContentStylePropertyIDConsts.ID_LineSpacing,
            PropertyName_LineSpacing,
           typeof(float),
           typeof(ContentStyle),
           0f);
        /// <summary>
        /// 行间距,单位取决于LineSpacingStyle属性值。
        /// </summary>
        public float LineSpacing
        {
            get
            {
                return (float)GetValue(_LineSpacing);
            }
            set
            {
                SetValue(_LineSpacing, Math.Max(0, value));
            }
        }

        /// <summary>
        /// 以Twips作为单位的标准行高
        /// </summary>
        public const int _StandLineHeightTwips = 240;

        /// <summary>
        /// Align属性名
        /// </summary>
        public const string PropertyName_Align = "Align";


        private readonly static XDependencyProperty _Align = XDependencyProperty.Register(
           ContentStylePropertyIDConsts.ID_Align,
            PropertyName_Align,
           typeof(DocumentContentAlignment),
           typeof(ContentStyle),
           DocumentContentAlignment.Left);
        /// <summary>
        /// 文本水平对齐方式
        /// </summary>
        public DocumentContentAlignment Align
        {
            get
            {
                return (DocumentContentAlignment)GetValue(_Align);
            }
            set
            {
                SetValue(_Align, value);
            }
        }

        /// <summary>
        /// VerticalAlign属性名
        /// </summary>
        public const string PropertyName_VerticalAlign = "VerticalAlign";

        private readonly static XDependencyProperty _VerticalAlign = XDependencyProperty.Register(
           ContentStylePropertyIDConsts.ID_VerticalAlign,
            PropertyName_VerticalAlign,
           typeof(VerticalAlignStyle),
           typeof(ContentStyle),
           VerticalAlignStyle.Top);
        /// <summary>
        /// 文本垂直对齐方式
        /// </summary>
        public VerticalAlignStyle VerticalAlign
        {
            get
            {
                return (VerticalAlignStyle)GetValue(_VerticalAlign);
            }
            set
            {
                SetValue(_VerticalAlign, value);
            }
        }

        /// <summary>
        /// FirstLineIndent属性名
        /// </summary>
        public const string PropertyName_FirstLineIndent = "FirstLineIndent";


        private readonly static XDependencyProperty _FirstLineIndent = XDependencyProperty.Register(
          ContentStylePropertyIDConsts.ID_FirstLineIndent,
             PropertyName_FirstLineIndent,
           typeof(float),
           typeof(ContentStyle),
           0f);

        /// <summary>
        /// 首行缩进量
        /// </summary>
        public float FirstLineIndent
        {
            get
            {
                float v = (float)GetValue(_FirstLineIndent);
                if (v > 0)
                {
                }
                return v;
            }
            set
            {
                SetValue(_FirstLineIndent, value);
            }
        }

        /// <summary>
        /// LeftIndent属性名
        /// </summary>
        public const string PropertyName_LeftIndent = "LeftIndent";

        private readonly static XDependencyProperty _LeftIndent = XDependencyProperty.Register(
          ContentStylePropertyIDConsts.ID_LeftIndent,
            PropertyName_LeftIndent,
          typeof(float),
          typeof(ContentStyle),
          0f);

        /// <summary>
        /// 段落左缩进量
        /// </summary>
        public float LeftIndent
        {
            get
            {
                return (float)GetValue(_LeftIndent);
            }
            set
            {
                if (value < 0)
                {
                    value = 0;
                }
                SetValue(_LeftIndent, value);
            }
        }

        #endregion

        #region 布局

        /// <summary>
        /// BorderLeftColor属性名
        /// </summary>
        public const string PropertyName_BorderLeftColor = "BorderLeftColor";
        private readonly static XDependencyProperty _BorderLeftColor = XDependencyProperty.Register(
          ContentStylePropertyIDConsts.ID_BorderLeftColor,
            PropertyName_BorderLeftColor,
          typeof(DCSystem_Drawing.Color),
          typeof(ContentStyle),
          DCSystem_Drawing.Color.Black);
        /// <summary>
        /// 左边框颜色
        /// </summary>
        public DCSystem_Drawing.Color BorderLeftColor
        {
            get
            {
                return (DCSystem_Drawing.Color)GetValue(_BorderLeftColor);
            }
            set
            {
                SetValue(_BorderLeftColor, value);
            }
        }
        /// <summary>
        /// 字符串格式的左边框颜色
        /// </summary>
        public string BorderLeftColorString
        {
            get
            {
                return XMLSerializeHelper.ColorToString(this.BorderLeftColor, DCSystem_Drawing.Color.Black);
            }
            set
            {
                this.BorderLeftColor = XMLSerializeHelper.StringToColor(value, DCSystem_Drawing.Color.Black);
            }
        }
        public DCSystem_Drawing.Color BorderColor
        {
            get
            {
                return this.BorderLeftColor;
            }
            set
            {
                this.BorderLeftColor = value;
                this.BorderTopColor = value;
                this.BorderRightColor = value;
                this.BorderBottomColor = value;
            }
        }

        /// <summary>
        /// BorderTopColor属性名
        /// </summary>
        public const string PropertyName_BorderTopColor = "BorderTopColor";
        private readonly static XDependencyProperty _BorderTopColor = XDependencyProperty.Register(
         ContentStylePropertyIDConsts.ID_BorderTopColor,
             PropertyName_BorderTopColor,
          typeof(DCSystem_Drawing.Color),
          typeof(ContentStyle),
          DCSystem_Drawing.Color.Black);
        /// <summary>
        /// 上边框颜色
        /// </summary>
        public DCSystem_Drawing.Color BorderTopColor
        {
            get
            {
                return (DCSystem_Drawing.Color)GetValue(_BorderTopColor);
            }
            set
            {
                SetValue(_BorderTopColor, value);
            }
        }

        /// <summary>
        /// 字符串格式的上边框颜色
        /// </summary>
        public string BorderTopColorString
        {
            get
            {
                return XMLSerializeHelper.ColorToString(this.BorderTopColor, DCSystem_Drawing.Color.Black);
            }
            set
            {
                this.BorderTopColor = XMLSerializeHelper.StringToColor(value, DCSystem_Drawing.Color.Black);
            }
        }

        /// <summary>
        /// BorderRightColor属性名
        /// </summary>
        public const string PropertyName_BorderRightColor = "BorderRightColor";
        private readonly static XDependencyProperty _BorderRightColor = XDependencyProperty.Register(
          ContentStylePropertyIDConsts.ID_BorderRightColor,
            PropertyName_BorderRightColor,
          typeof(DCSystem_Drawing.Color),
          typeof(ContentStyle),
          DCSystem_Drawing.Color.Black);
        /// <summary>
        /// 右边框颜色
        /// </summary>
        public DCSystem_Drawing.Color BorderRightColor
        {
            get
            {
                return (DCSystem_Drawing.Color)GetValue(_BorderRightColor);
            }
            set
            {
                SetValue(_BorderRightColor, value);
            }
        }

        /// <summary>
        /// 字符串格式的右边框颜色
        /// </summary>
        public string BorderRightColorString
        {
            get
            {
                return XMLSerializeHelper.ColorToString(this.BorderRightColor, DCSystem_Drawing.Color.Black);
            }
            set
            {
                this.BorderRightColor = XMLSerializeHelper.StringToColor(value, DCSystem_Drawing.Color.Black);
            }
        }

        /// <summary>
        /// BorderBottomColor属性名
        /// </summary>
        public const string PropertyName_BorderBottomColor = "BorderBottomColor";
        private readonly static XDependencyProperty _BorderBottomColor = XDependencyProperty.Register(
            ContentStylePropertyIDConsts.ID_BorderBottomColor,
              PropertyName_BorderBottomColor,
          typeof(DCSystem_Drawing.Color),
          typeof(ContentStyle),
          DCSystem_Drawing.Color.Black);
        /// <summary>
        /// 下边框颜色
        /// </summary>
        public DCSystem_Drawing.Color BorderBottomColor
        {
            get
            {
                return (DCSystem_Drawing.Color)GetValue(_BorderBottomColor);
            }
            set
            {
                SetValue(_BorderBottomColor, value);
            }
        }

        /// <summary>
        /// 字符串格式的下边框颜色
        /// </summary>
        public string BorderBottomColorString
        {
            get
            {
                return XMLSerializeHelper.ColorToString(this.BorderBottomColor, DCSystem_Drawing.Color.Black);
            }
            set
            {
                this.BorderBottomColor = XMLSerializeHelper.StringToColor(value, DCSystem_Drawing.Color.Black);
            }
        }

        /// <summary>
        /// BorderStyle属性名
        /// </summary>
        public const string PropertyName_BorderStyle = "BorderStyle";

        private readonly static XDependencyProperty _BorderStyle = XDependencyProperty.Register(
             ContentStylePropertyIDConsts.ID_BorderStyle,
            PropertyName_BorderStyle,
             typeof(DashStyle),
             typeof(ContentStyle),
              DashStyle.Solid);
        /// <summary>
        /// 边框线型
        /// </summary>
        public DashStyle BorderStyle
        {
            get
            {
                return (DashStyle)GetValue(_BorderStyle);
            }
            set
            {
                SetValue(_BorderStyle, value);
            }
        }
        /// <summary>
        /// BorderWidth属性名
        /// </summary>
        public const string PropertyName_BorderWidth = "BorderWidth";

        private readonly static XDependencyProperty _BorderWidth = XDependencyProperty.Register(
            ContentStylePropertyIDConsts.ID_BorderWidth,
            PropertyName_BorderWidth,
            typeof(float),
            typeof(ContentStyle),
            0f);
        /// <summary>
        /// 边框线粗细
        /// </summary>
        public float BorderWidth
        {
            get
            {
                return (float)GetValue(_BorderWidth);
            }
            set
            {
                SetValue(_BorderWidth, value);
            }
        }
        /// <summary>
        /// 删除边框设置，包括BorderLeft,BorderTop,BorderRight,BorderBottom,BorderColor,BorderStyle,BorderWidth值
        /// </summary>
        public void RemoveBorderValues()
        {
            base._InnerValues.Remove(_BorderLeft);
            base._InnerValues.Remove(_BorderTop);
            base._InnerValues.Remove(_BorderRight);
            base._InnerValues.Remove(_BorderBottom);
            base._InnerValues.Remove(_BorderLeftColor);
            base._InnerValues.Remove(_BorderTopColor);
            base._InnerValues.Remove(_BorderRightColor);
            base._InnerValues.Remove(_BorderBottomColor);
            base._InnerValues.Remove(_BorderStyle);
            base._InnerValues.Remove(_BorderWidth);
        }
        /// <summary>
        /// BorderLeft属性名
        /// </summary>
        public const string PropertyName_BorderLeft = "BorderLeft";

        private readonly static XDependencyProperty _BorderLeft = XDependencyProperty.Register(
            ContentStylePropertyIDConsts.ID_BorderLeft,
            PropertyName_BorderLeft,
            typeof(bool),
            typeof(ContentStyle),
            false);
        /// <summary>
        /// 是否显示左边框线
        /// </summary>
        public bool BorderLeft
        {
            get
            {
                return (bool)GetValue(_BorderLeft);
            }
            set
            {
                SetValue(_BorderLeft, value);
            }
        }

        /// <summary>
        /// BorderBottom属性名
        /// </summary>
        public const string PropertyName_BorderBottom = "BorderBottom";

        private readonly static XDependencyProperty _BorderBottom = XDependencyProperty.Register(
            ContentStylePropertyIDConsts.ID_BorderBottom,
            PropertyName_BorderBottom,
            typeof(bool),
            typeof(ContentStyle),
            false);

        /// <summary>
        /// 是否显示下边框线
        /// </summary>
        public bool BorderBottom
        {
            get
            {
                return (bool)GetValue(_BorderBottom);
            }
            set
            {
                SetValue(_BorderBottom, value);
            }
        }

        /// <summary>
        /// BorderTop属性名
        /// </summary>
        public const string PropertyName_BorderTop = "BorderTop";

        private readonly static XDependencyProperty _BorderTop = XDependencyProperty.Register(
            ContentStylePropertyIDConsts.ID_BorderTop,
            PropertyName_BorderTop,
            typeof(bool),
            typeof(ContentStyle),
            false);

        /// <summary>
        /// 是否显示上边框线
        /// </summary>
        public bool BorderTop
        {
            get
            {
                return (bool)GetValue(_BorderTop);
            }
            set
            {
                SetValue(_BorderTop, value);
            }
        }

        /// <summary>
        /// BorderRight属性名
        /// </summary>
        public const string PropertyName_BorderRight = "BorderRight";

        private readonly static XDependencyProperty _BorderRight = XDependencyProperty.Register(
            ContentStylePropertyIDConsts.ID_BorderRight,
            PropertyName_BorderRight,
            typeof(bool),
            typeof(ContentStyle),
            false);
        /// <summary>
        /// 是否显示右边框线
        /// </summary>
        public bool BorderRight
        {
            get
            {
                return (bool)GetValue(_BorderRight);
            }
            set
            {
                SetValue(_BorderRight, value);
            }
        }


        /// <summary>
        /// 判断样式是否存在可见的边框效果
        /// </summary>
        public bool HasVisibleBorder
        {
            get
            {
                if (this.BorderLeft == false
                    && this.BorderTop == false
                    && this.BorderRight == false
                    && this.BorderBottom == false)
                {
                    return false;
                }
                if (this.BorderTopColor.A == 0
                    && this.BorderLeftColor.A == 0
                    && this.BorderRightColor.A == 0
                    && this.BorderBottomColor.A == 0)
                {
                    return false;
                }
                if (this.BorderWidth == 0)
                {
                    return false;
                }
                return true;
            }
        }


        /// <summary>
        /// PaddingLeft属性名
        /// </summary>
        public const string PropertyName_PaddingLeft = "PaddingLeft";

        private readonly static XDependencyProperty _PaddingLeft = XDependencyProperty.Register(
            ContentStylePropertyIDConsts.ID_PaddingLeft,
            PropertyName_PaddingLeft,
            typeof(float),
            typeof(ContentStyle),
            0f);
        /// <summary>
        /// 左内边距
        /// </summary>
        public float PaddingLeft
        {
            get
            {
                return (float)GetValue(_PaddingLeft);
            }
            set
            {
                SetValue(_PaddingLeft, value);
            }
        }

        /// <summary>
        /// PaddingTop属性名
        /// </summary>
        public const string PropertyName_PaddingTop = "PaddingTop";


        private readonly static XDependencyProperty _PaddingTop = XDependencyProperty.Register(
            ContentStylePropertyIDConsts.ID_PaddingTop,
            PropertyName_PaddingTop,
            typeof(float),
            typeof(ContentStyle),
            0f);
        /// <summary>
        /// 上内边距
        /// </summary>
        public float PaddingTop
        {
            get
            {
                return (float)GetValue(_PaddingTop);
            }
            set
            {
                SetValue(_PaddingTop, value);
            }
        }

        /// <summary>
        /// PaddingRight属性名
        /// </summary>
        public const string PropertyName_PaddingRight = "PaddingRight";

        private readonly static XDependencyProperty _PaddingRight = XDependencyProperty.Register(
            ContentStylePropertyIDConsts.ID_PaddingRight,
            PropertyName_PaddingRight,
            typeof(float),
            typeof(ContentStyle),
            0f);
        /// <summary>
        /// 右内边距
        /// </summary>
        public float PaddingRight
        {
            get
            {
                return (float)GetValue(_PaddingRight);
            }
            set
            {
                SetValue(_PaddingRight, value);
            }
        }

        /// <summary>
        /// PaddingBottom属性名
        /// </summary>
        public const string PropertyName_PaddingBottom = "PaddingBottom";

        private readonly static XDependencyProperty _PaddingBottom = XDependencyProperty.Register(
            ContentStylePropertyIDConsts.ID_PaddingBottom,
            PropertyName_PaddingBottom,
            typeof(float),
            typeof(ContentStyle),
            0f);
        /// <summary>
        /// 下内边距
        /// </summary>
        public float PaddingBottom
        {
            get
            {
                return (float)GetValue(_PaddingBottom);
            }
            set
            {
                SetValue(_PaddingBottom, value);
            }
        }

        #endregion


        /// <summary>
        /// ParagraphMultiLevel属性名
        /// </summary>
        public const string PropertyName_ParagraphMultiLevel = "ParagraphMultiLevel";

        private readonly static XDependencyProperty _ParagraphMultiLevel = XDependencyProperty.Register(
            ContentStylePropertyIDConsts.ID_ParagraphMultiLevel,
            PropertyName_ParagraphMultiLevel,
            typeof(bool),
            typeof(ContentStyle),
            false);
        /// <summary>
        /// 多层次段落列表
        /// </summary>
        public bool ParagraphMultiLevel
        {
            get
            {
                //return false;
                return (bool)GetValue(_ParagraphMultiLevel);
            }
            set
            {
                SetValue(_ParagraphMultiLevel, value);
            }
        }

        /// <summary>
        /// ParagraphOutlineLevel属性名
        /// </summary>
        public const string PropertyName_ParagraphOutlineLevel = "ParagraphOutlineLevel";

        private readonly static XDependencyProperty _ParagraphOutlineLevel = XDependencyProperty.Register(
            ContentStylePropertyIDConsts.ID_ParagraphOutlineLevel,
            PropertyName_ParagraphOutlineLevel,
            typeof(int),
            typeof(ContentStyle),
            -1);
        /// <summary>
        /// 段落大纲层次
        /// </summary>
        public int ParagraphOutlineLevel
        {
            get
            {
                //return -1;
                return (int)GetValue(_ParagraphOutlineLevel);
            }
            set
            {
                SetValue(_ParagraphOutlineLevel, value);
            }
        }

        /// <summary>
        /// ParagraphListStyle属性名
        /// </summary>
        public const string PropertyName_ParagraphListStyle = "ParagraphListStyle";

        private readonly static XDependencyProperty _ParagraphListStyle = XDependencyProperty.Register(
            ContentStylePropertyIDConsts.ID_ParagraphListStyle,
            PropertyName_ParagraphListStyle,
            typeof(ParagraphListStyle),
            typeof(ContentStyle),
            DCSoft.Drawing.ParagraphListStyle.None);
        /// <summary>
        /// paragraph list style
        /// </summary>
        public DCSoft.Drawing.ParagraphListStyle ParagraphListStyle
        {
            get
            {
                return (DCSoft.Drawing.ParagraphListStyle)GetValue(_ParagraphListStyle);
            }
            set
            {
                SetValue(_ParagraphListStyle, value);
            }
        }

        /// <summary>
        /// 判断是否是圆点列表方式
        /// </summary>
       //////[Browsable(false)]
        [System.Reflection.Obfuscation(Exclude = true)]
        public bool IsBulletedList
        {
            get
            {
                return ParagraphListHelper.IsBulletedList(this.ParagraphListStyle);
            }
        }

        /// <summary>
        /// 判断是否是数字列表方式
        /// </summary>
        public bool IsListNumberStyle
        {
            get
            {
                return ParagraphListHelper.IsListNumberStyle(this.ParagraphListStyle);
            }
        }


        public string GetParagraphListText(int number)
        {
            return ParagraphListHelper.GetParagraphListText(number, this.ParagraphListStyle);
        }
        public string BulletedString
        {
            get
            {
                return ParagraphListHelper.GetBulletedListString(this.ParagraphListStyle);
            }
        }

        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public ContentStyle CloneEnableDefaultValue()
        {
            ContentStyle style = (ContentStyle)this.Clone();
            style.DisableDefaultValue = false;
            return style;
        }

        /// <summary>
        /// 复制对象
        /// </summary>
        /// <returns>复制品</returns>
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]

        public virtual ContentStyle Clone()
        {
            ContentStyle style = (ContentStyle)this.MemberwiseClone();
            style._InnerValues = new XDependencyPropertyObjectValues();
            style.ValueLocked = false;
            //style.InnerValues.Clear();
            XDependencyObject.CopyValueFast(this, style);
            return style;
        }


        /// <summary>
        /// 返回表示对象内容的字符串
        /// </summary>
        /// <returns></returns>

        public override string ToString()
        {
            return XDependencyObject.GetStyleString(this);
        }

        /// <summary>
        /// 销毁对象
        /// </summary>
        [System.Reflection.Obfuscation(Exclude = true)]

        public virtual void Dispose()
        {
            base.ClearWithDispose();
            if (this._RuntimeStyle != null)
            {
                this._RuntimeStyle.Dispose();
                this._RuntimeStyle = null;
            }
        }
    }
}