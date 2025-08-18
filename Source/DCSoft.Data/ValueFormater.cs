using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Runtime.InteropServices;
using DCSoft.Common;
using DCSoft.Writer.Data;


namespace DCSoft.Data
{
    /// <summary>
    /// 数据格式化对象
    /// </summary>
    public partial class ValueFormater
    {

        static ValueFormater()
        {
            _dtmFormat = new System.Globalization.DateTimeFormatInfo();
            _dtmFormat.DateSeparator = "/";
        }

        private static readonly System.Globalization.DateTimeFormatInfo _dtmFormat = null;
        /// <summary>
        /// 初始化对象
        /// </summary>
        public ValueFormater()
        {
        }


        private ValueFormatStyle _Style = ValueFormatStyle.None;
        /// <summary>
        /// 数据源格式化样式
        /// </summary>
        [System.ComponentModel.DefaultValue(ValueFormatStyle.None)]
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true )]
        public ValueFormatStyle Style
        {
            get
            {
                return _Style;
            }
            set
            {
                _Style = value;
            }
        }


        private string _Format = null;
        /// <summary>
        /// 格式化字符串
        /// </summary>
        [System.ComponentModel.DefaultValue(null)]
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true )]
        public string Format
        {
            get
            {
                return _Format;
            }
            set
            {
                _Format = value;
            }
        }

        private string _NoneText = null;
        /// <summary>
        /// 对于非数字显示的文本
        /// </summary>
        [System.ComponentModel.DefaultValue(null)]
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true )]
        public string NoneText
        {
            get
            {
                return _NoneText;
            }
            set
            {
                _NoneText = value;
            }
        }

        /// <summary>
        /// 对象没有任何有效设置
        /// </summary>
       ////[System.ComponentModel.Browsable(false)]
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true )]
        public bool IsEmpty
        {
            get
            {
                if (this._Style == ValueFormatStyle.None)
                {
                    return true;
                }
                if (this._Format != null && _Format.Length > 0)
                {
                    return false;
                }
                if (this._NoneText != null && this._NoneText.Length > 0)
                {
                    return false;
                }
                return true;
            }
        }

        /// <summary>
        /// 复制对象
        /// </summary>
        /// <returns>复制品</returns>
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true  )]
        public ValueFormater Clone()
        {
            return (ValueFormater)this.MemberwiseClone();
        }
#if !RELEASE
        public override string ToString()
        {
            System.Text.StringBuilder str = new System.Text.StringBuilder();
            str.Append(_Style.ToString());
            if (_Style != ValueFormatStyle.None)
            {
                if (HasContent(_Format))
                {
                    str.Append("," + _Format);
                }
                if (HasContent(_NoneText))
                {
                    str.Append(",NoneText=" + _NoneText);
                }
            }
            return str.ToString();
        }
#endif
        internal static bool HasContent(string txt)
        {
            return txt != null && txt.Trim().Length > 0;
        }

        /// <summary>
        /// 执行数据格式转换,生成转换后的文本
        /// </summary>
        /// <param name="Value">原始数据</param>
        /// <returns>格式化后生成的文本</returns>
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true )]
        public string Execute(object Value)
        {
            if (_Style == ValueFormatStyle.None)
            {
                if (Value == null || DBNull.Value.Equals(Value))
                {
                    return null;
                }
                else
                {
                    return Convert.ToString( Value );
                }
            }
            else
            {
                return EventExecute(
                    this._Style,
                    this._Format,
                    this._NoneText,
                    Value);
            }
        }

        public static ExecuteHandler EventExecute = null;

        public delegate string ExecuteHandler(
            ValueFormatStyle style,
            string format,
            string noneText,
            object Value);
    }

    /// <summary>
    /// 数据源格式类型
    /// </summary>
    [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true )]
    [System.Text.Json.Serialization.JsonConverter(typeof(System.Text.Json.Serialization.JsonStringEnumConverter))]
    public enum ValueFormatStyle
    {
        /// <summary>
        /// 无样式
        /// </summary>
        None,
        /// <summary>
        /// 数字
        /// </summary>
        Numeric,
        /// <summary>
        /// 货币
        /// </summary>
        Currency,
        /// <summary>
        /// 时间日期
        /// </summary>
        DateTime,
        /// <summary>
        /// 字符串
        /// </summary>
        String,
        /// <summary>
        /// 固定文本长度
        /// </summary>
        SpecifyLength,
        /// <summary>
        /// 布尔类型
        /// </summary>
        Boolean,
        /// <summary>
        /// 百分比
        /// </summary>
        Percent,
    }
}