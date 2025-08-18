using System;
using System.ComponentModel;
using System.Collections.Generic;
// // 
//using  Drawing2D;
using DCSoft.Common;

namespace DCSoft.Drawing
{
    /// <summary>
    /// 字体信息类型，本对象可以参与XML和二进制的序列化及反序列化。
    /// </summary>
    public partial class XFontValue : System.ICloneable,IDisposable
    {
        /// <summary>
        /// 静态构造函数
        /// </summary>
        static XFontValue()
        {
            SetDefaultFont("宋体", 9);
        }

        public static void VoidMethod()
        {

        }

        public static void SetDefaultFont(string fontName, float fontSize)
        {
            DefaultFont = new Font(fontName, fontSize);
            DefaultFontName = string.Intern(fontName);
            DefaultFontSize = fontSize;
            _Default = null;
        }

        private static XFontValue _Default = null;
        /// <summary>
        /// 默认值
        /// </summary>
        public static XFontValue Default
        {
            get
            {
                if( _Default == null )
                {
                    _Default = new XFontValue(DefaultFontName, DefaultFontSize);
                }
                return _Default;
            }
        }

        public delegate string FixFontNameEventHandler(string fontName);

        /// <summary>
        /// 修正字体名称事件
        /// </summary>
        public static FixFontNameEventHandler EventFixFontName = null;

        public delegate bool IsSupportFontNameHandler(string fontName);

        public static IsSupportFontNameHandler EventIsSupportFontName = null;
        private static readonly Dictionary<string, string> _FixedFontNames = new Dictionary<string, string>();
        /// <summary>
        /// 修正字体名称
        /// </summary>
        /// <param name="name">字体名称</param>
        /// <param name="throwException">是否抛出异常</param>
        /// <returns>修正后的字体名称</returns>
        public static string FixFontName(string name, bool throwException)
        {
            if (string.IsNullOrEmpty(name))
            {
                if (throwException)
                {
                    throw new ArgumentNullException("name");
                }
                return DefaultFontName;
            }
            string strResult = null;
            if (_FixedFontNames.TryGetValue(name, out strResult))
            {
                return strResult;
            }
            if( EventIsSupportFontName != null && EventIsSupportFontName( name ))
            {
                _FixedFontNames[name] = name;
                return name;
            }
            if(name.Contains('('))
            {
                // 字体名称中出现圆括号，则进行修正，再次尝试获得正确的字体名称
                var index1 = name.IndexOf('(');
                var index2 = name.IndexOf(')', index1);
                if(index2 > 0 )
                {
                    string name4 = name.Substring(index1, index2 - index1);
                    if (EventIsSupportFontName != null && EventIsSupportFontName(name))
                    {
                        _FixedFontNames[name] = name4;
                        return name4;
                    }
                    if ( DCSoft.Writer.Controls.WASMEnvironment.JSProvider.JSIsSupportFontName( name4))
                    {
                        _FixedFontNames[name] = name4;
                        return name4;
                    }
                }
            }
            if( DCSoft.TrueTypeFontSnapshort.Support( name ))
            {
                _FixedFontNames[name] = name;
                return name;
            }
            if (throwException)
            {
                throw new NotSupportedException("Not support Font:" + name);
            }
            else
            {
                _FixedFontNames[name] = DefaultFontName;
                return DefaultFontName;
            }
        }

        /// <summary>
        /// 默认字体
        /// </summary>
        [NonSerialized()]

        public static  Font DefaultFont = null;
        
        /// <summary>
        /// 默认字体名称
        /// </summary>

        public static string DefaultFontName = null;
        /// <summary>
        /// 默认字体大小
        /// </summary>

        public static float DefaultFontSize = 9;

        /// <summary>
        /// 初始化对象
        /// </summary>
        public XFontValue()
        {
        }

        /// <summary>
        /// 初始化对象
        /// </summary>
        /// <param name="name">字体名称</param>
        /// <param name="size">字体大小</param>
        public XFontValue(string name, float size)
        {
            _Name = name;
            _Size = size;
        }

        /// <summary>
        /// 初始化对象
        /// </summary>
        /// <param name="name">字体名称</param>
        /// <param name="size">字体大小</param>
        /// <param name="style">字体样式</param>
        public XFontValue(
            string name,
            float size,
            FontStyle style)
        {
            _Name = name;
            _Size = size;
            this.Style = style;
        }

        /// <summary>
		/// 初始化对象
		/// </summary>
		/// <param name="name">字体名称</param>
		/// <param name="size">字体大小</param>
		/// <param name="style">字体样式</param>
        /// <param name="unit">度量单位</param>
		public XFontValue(
            string name,
            float size,
             FontStyle style,
            GraphicsUnit unit)
        {
            _Name = name;
            _Size = size;
            this.Style = style;
            this.Unit = unit;
        }



        /// <summary>
        /// 初始化对象
        /// </summary>
        /// <param name="f">字体对象</param>
        public XFontValue( Font f , bool setFontValue = false )
        {
            if (f != null)
            {
                this.Name = f.Name;
                this.Size = f.Size;
                this.Style = f.Style;
                this.Unit = f.Unit;
                if( setFontValue )
                {
                    this.myValue = f;
                }
            }
            //this.Value = f ;
        }

        private string _Name = DefaultFontName;
        /// <summary>
        /// 字体名称
        /// </summary>
        public string Name
        {
            get
            {
                return _Name;
            }
            set
            {
                if (_Name != value)
                {
                    _Name = value;
                    if (string.IsNullOrEmpty(_Name))
                    {
                        _Name = DefaultFontName;
                    }
                    else
                    {
                        _Name = string.Intern(_Name);
                    }
                    myValue = null;
                    //this._RawFontIndex = -1;
                }
            }
        }

        private float _Size = DefaultFontSize;
        /// <summary>
        /// 字体大小
        /// </summary>
         public float Size
        {
            get
            {
                return _Size;
            }
            set
            {
                if (_Size != value)
                {
                    _Size = value;
                    if (_Size <= 0)
                    {
                        _Size = DefaultFontSize;
                    }
                    myValue = null;
                    //this._RawFontIndex = -1;
                }
            }
        }


        private GraphicsUnit _Unit = GraphicsUnit.Point;
        /// <summary>
        /// 字体大小的度量单位
        /// </summary>
        public GraphicsUnit Unit
        {
            get
            {
                return _Unit;
            }
            set
            {
                _Unit = value;
            }
        }

        private bool _Bold;
        /// <summary>
        /// 是否粗体
        /// </summary>
        public bool Bold
        {
            get
            {
                return _Bold;
            }
            set
            {
                if (_Bold != value)
                {
                    _Bold = value;
                    myValue = null;
                    //this._RawFontIndex = -1;
                }
            }
        }
        private bool _Italic;
        /// <summary>
        /// 是否斜体
        /// </summary>
        public bool Italic
        {
            get
            {
                return _Italic;
            }
            set
            {
                if (_Italic != value)
                {
                    _Italic = value;
                    myValue = null;
                    //this._RawFontIndex = -1;
                }
            }
        }

        private bool _Underline ;
        /// <summary>
        /// 下划线
        /// </summary>
        public bool Underline
        {
            get
            {
                return _Underline;
            }
            set
            {
                if (_Underline != value)
                {
                    _Underline = value;
                    myValue = null;
                    //this._RawFontIndex = -1;
                }
            }
        }

        private bool _Strikeout;
        /// <summary>
        /// 删除线
        /// </summary>
        public bool Strikeout
        {
            get
            {
                return _Strikeout;
            }
            set
            {
                if (_Strikeout != value)
                {
                    _Strikeout = value;
                    myValue = null;
                    //this._RawFontIndex = -1;
                }
            }
        }

        /// <summary>
        /// 字体样式
        /// </summary>
        public  FontStyle Style
        {
            get
            {
                 FontStyle style =  FontStyle.Regular;
                if (this._Bold)
                {
                    style =  FontStyle.Bold;
                }
                if (this._Italic)
                {
                    style = style |  FontStyle.Italic;
                }
                if (this._Underline)
                {
                    style = style |  FontStyle.Underline;
                }
                if (this._Strikeout)
                {
                    style = style |  FontStyle.Strikeout;
                }
                return style;
            }
            set
            {
                if (this.Style != value)
                {
                    this._Bold = GetStyle(value,  FontStyle.Bold);
                    this._Italic = GetStyle(value,  FontStyle.Italic);
                    this._Underline = GetStyle(value,  FontStyle.Underline);
                    this._Strikeout = GetStyle(value,  FontStyle.Strikeout);
                    myValue = null;
                    //this._RawFontIndex = -1;
                }
            }
        }

        private bool GetStyle(
             FontStyle intValue,
             FontStyle MaskFlag)
        {
            return (intValue & MaskFlag) == MaskFlag;
        }
        public  Font CreateFont()
        {
            return new  Font(this._Name, this._Size, this.Style, this._Unit);
        }

        [System.NonSerialized()]
        private  Font myValue;
        /// <summary>
        /// 字体对象
        /// </summary>
        public  Font Value
        {
            get
            {
                if(this.myValue == null )
                {
                    this.myValue = new Font(FixFontName(this._Name, false), this._Size , this.Style , this._Unit);
                }
                return this.myValue;
            }
            set
            {
                if (value == null)
                {
                    value = DefaultFont;
                }
                if (EqualsValue(value) == false)
                {
                    _Name = value.Name;
                    _Size = value.Size;
                    _Bold = value.Bold;
                    _Italic = value.Italic;
                    _Underline = value.Underline;
                    _Strikeout = value.Strikeout;
                    _Unit = value.Unit;
                    myValue = value;
                }
            }
        }


        /// <summary>
        /// 获得字体的以像素为单位的高度
        /// </summary>
        /// <returns>字体高度</returns>
        public float GetHeight()
        {
             Font f = this.Value;
            if (f == null)
            {
                return 0;
            }
            else
            {
                return f.GetHeight();
            }
        }
        
        /// <summary>
        /// 获得字体的高度
        /// </summary>
        /// <param name="g">绘图对象</param>
        /// <returns>字体高度</returns>
        public float GetHeight(DCGraphics g)
        {
             Font f = this.Value;
            if (f == null)
            {
                return 0;
            }
            else if (g.NativeGraphics != null)
            {
                return f.GetHeight(g.NativeGraphics);
            }
            else if (g.GraphisForMeasure != null)
            {
                return f.GetHeight(g.GraphisForMeasure);
            }
            else
            {
                return f.GetHeight();
            }
        }

        /// <summary>
        /// 获得字体的高度
        /// </summary>
        /// <param name="g">绘图对象</param>
        /// <returns>字体高度</returns>
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public float GetHeight(Graphics g)
        {
            Font f = this.Value;
            if (f == null)
            {
                return 0;
            }
            else
            {
                return f.GetHeight(g);
            }
        }

        /// <summary>
        /// 获得指定度量单位下的字体高度
        /// </summary>
        /// <param name="unit">指定的度量单位</param>
        /// <returns>字体高度</returns>
        public float GetHeight(GraphicsUnit unit)
        {
            return GraphicsUnitConvert.Convert(this.Value.SizeInPoints, GraphicsUnit.Point, unit);
        }


        /// <summary>
        /// 比较对象和指定字体的设置是否一致
        /// </summary>
        /// <param name="f">字体对象</param>
        /// <returns>是否一致</returns>
        public bool EqualsValue( Font f)
        {
            if (f == null)
            {
                return false;
            }
            if (_Name != f.Name)
            {
                return false;
            }
            if (_Size != f.Size)
            {
                return false;
            }
            if (_Bold != f.Bold)
            {
                return false;
            }
            if (_Italic != f.Italic)
            {
                return false;
            }
            if (_Underline != f.Underline)
            {
                return false;
            }
            if (_Strikeout != f.Strikeout)
            {
                return false;
            }
            if (_Unit != f.Unit)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 比较对象和指定字体的设置是否一致
        /// </summary>
        /// <param name="f">字体对象</param>
        /// <returns>是否一致</returns>
        public bool EqualsValue(XFontValue f)
        {
            if (f == null)
            {
                return false;
            }
            if (this == f)
            {
                return true;
            }
            if (_Name != f._Name)
            {
                return false;
            }
            if (_Size != f._Size)
            {
                return false;
            }
            if (_Bold != f._Bold)
            {
                return false;
            }
            if (_Italic != f._Italic)
            {
                return false;
            }
            if (_Underline != f._Underline)
            {
                return false;
            }
            if (_Strikeout != f._Strikeout)
            {
                return false;
            }
            if (_Unit != f._Unit)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 复制对象
        /// </summary>
        /// <returns>复制品</returns>
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public XFontValue Clone()
        {
            return (XFontValue)this.MemberwiseClone();
            //XFontValue font = new XFontValue();
            //font.CopySettings(this);
            //this._RawFontIndex = -1;
            //return font;
        }
        /// <summary>
        /// 复制对象
        /// </summary>
        /// <returns>复制品</returns>
        object System.ICloneable.Clone()
        {
            return this.MemberwiseClone();
            //XFontValue font = new XFontValue();
            //font.CopySettings(this);
            //this._RawFontIndex = -1;
            //return font;
        }

        /// <summary>
        /// 比较两个对象内容是否相同
        /// </summary>
        /// <param name="obj">对象</param>
        /// <returns>内容是否相同</returns>
        public override bool Equals(object obj)
        {
            if (obj == this)
                return true;
            if (!(obj is XFontValue))
                return false;
            XFontValue f = (XFontValue)obj;
            return f._Bold == this._Bold
                && f._Italic == this._Italic
                && f._Strikeout == this._Strikeout
                && f._Underline == this._Underline
                && f._Size == this._Size
                && f._Name == this._Name
                && f._Unit == this._Unit;
        }
        /// <summary>
        /// 获得对象的哈希代码
        /// </summary>
        /// <returns>哈希代码</returns>

        public override int GetHashCode()
        {
            return this.ToString().GetHashCode();
        }
#if !RELEASE
        /// <summary>
        /// 获得表示对象数据的字符串
        /// </summary>
        /// <returns>字符串</returns>
        public override string ToString()
        {
            System.Collections.ArrayList list = new System.Collections.ArrayList();
            list.Add(this.Name);
            list.Add(this.Size.ToString());
            if (this.Style !=  FontStyle.Regular)
            {
                list.Add("style=" + this.Style.ToString("G"));
            }
            if (this._Unit != GraphicsUnit.Point)
            {
                list.Add(this._Unit.ToString("G"));
            }
            return string.Join(", ", (string[])list.ToArray(typeof(string)));
        }
#endif
        /// <summary>
        /// 解析字符串，设置对象数据
        /// </summary>
        /// <param name="Text">要解析的字符串</param>
        public void Parse(string Text)
        {
            if (string.IsNullOrEmpty(Text))
            {
                return;
            }
            string[] items = Text.Split(',');
            if (items.Length < 1)
            {
                throw new ArgumentException("必须符合 name,size,style=Bold,Italic,Underline,Strikeout 样式");
            }
            string name = items[0];

            float size = 9f;
            if (items.Length >= 2)
            {
                if (float.TryParse(items[1].Trim(), out size) == false)
                {
                    size = 9f;
                }
            }
            if (size <= 0)
            {
                size = 1;
            }

             FontStyle style =  FontStyle.Regular;
            bool flag = false;
            for (int iCount = 2; iCount < items.Length; iCount++)
            {
                string item = items[iCount].Trim().ToLower();
                if (flag == false)
                {
                    if (item.StartsWith("style"))
                    {
                        int index = item.IndexOf('=');
                        if (index > 0)
                        {
                            flag = true;
                            item = item.Substring(index + 1);
                        }
                    }
                }
                if (flag)
                {
                    if (Enum.IsDefined(typeof(FontStyle), item.Trim()))
                    {
                        FontStyle s2 = (FontStyle)Enum.Parse(
                            typeof(FontStyle), item.Trim(), true);
                        style = style | s2;
                    }
                    else if (Enum.IsDefined(typeof(GraphicsUnit), item.Trim()))
                    {
                        this._Unit = (GraphicsUnit)Enum.Parse(
                            typeof(GraphicsUnit), item.Trim(), true);
                    }
                }
            }
            this.Name = name;
            this.Size = size;
            this.Style = style;
        }


#region IDisposable 成员

        /// <summary>
        /// 销毁对象
        /// </summary>
        public void Dispose()
        {
            //this.mySite = null;
            this.myValue = null;
            this._Name = null;
            //if (Disposed != null)
            //{
            //    Disposed(this, EventArgs.Empty);
            //}
        }

#endregion
    }//public class XFontValue : System.ICloneable


    /// <summary>
    /// 字体宽度样式
    /// </summary>
    public enum FontWidthStyle
    {
        /// <summary>
        /// 比例字体
        /// </summary>
        Proportional,
        /// <summary>
        /// 等宽字体
        /// </summary>
        Monospaced
    }
}