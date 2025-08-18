using System;
using System.Text;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace DCSoft.Common
{
    
    /// <summary>
    /// 文本验证样式对象
    /// </summary>
    /// <remarks>编制 袁永福</remarks>
    public partial class ValueValidateStyle
    {

        /// <summary>
        /// 初始化对象
        /// </summary>
        public ValueValidateStyle()
        {
        }
         

        private ValueValidateLevel _Level = ValueValidateLevel.Error;
        /// <summary>
        /// 校验等级
        /// </summary>
        public ValueValidateLevel Level
        {
            get
            {
                return _Level; 
            }
            set
            {
                _Level = value; 
            }
        }

        private string _ValueName = null;
        /// <summary>
        /// 数据的名称
        /// </summary>
        public string ValueName
        {
            get
            {
                return _ValueName; 
            }
            set
            {
                _ValueName = value; 
            }
        }


        private int _EleStates = 0;


        /// <summary>
        /// 必填数据
        /// </summary>
        public bool Required
        {
            get
            {
                return (this._EleStates & 1) != 0;
            }
            set
            {
                this._EleStates = value ? (this._EleStates | 1) : (this._EleStates & ~1);
            }
        }

        private ValueTypeStyle _ValueType = ValueTypeStyle.Text;
        /// <summary>
        /// 数值类型
        /// </summary>
        public ValueTypeStyle ValueType
        {
            get
            {
                return _ValueType; 
            }
            set
            {
                _ValueType = value; 
            }
        }

        /// <summary>
        /// 进行的是二进制长度判断
        /// </summary>
        public bool BinaryLength
        {
            get
            {
                return (this._EleStates & 2) != 0;
            }
            set
            {
                this._EleStates = value ? (this._EleStates | 2) : (this._EleStates & ~2);
            }
        }

        private int _MaxLength = 0;
        /// <summary>
        /// 最大文本长度
        /// </summary>
        public int MaxLength
        {
            get 
            {
                return _MaxLength; 
            }
            set
            {
                _MaxLength = value; 
            }
        }

        private int _MinLength = 0;
        /// <summary>
        /// 最小文本长度
        /// </summary>
        public int MinLength
        {
            get
            {
                return _MinLength; 
            }
            set
            {
                _MinLength = value; 
            }
        }

        /// <summary>
        /// 检查数值或者日期值的最大值
        /// </summary>
        public bool CheckMaxValue
        {
            get
            {
                return (this._EleStates & 4) != 0;
            }
            set
            {
                this._EleStates = value ? (this._EleStates | 4) : (this._EleStates & ~4);
            }
        }

        /// <summary>
        /// 检查数值或者日期值的最小值
        /// </summary>
        public bool CheckMinValue
        {
            get
            {
                return (this._EleStates & 8) != 0;
            }
            set
            {
                this._EleStates = value ? (this._EleStates | 8) : (this._EleStates & ~8);
            }
        }

        private double _MaxValue = 0.0;
        /// <summary>
        /// 数值最大值
        /// </summary>
        public double MaxValue
        {
            get 
            {
                return _MaxValue; 
            }
            set
            {
                _MaxValue = value; 
            }
        }

        private double _MinValue = 0.0;
        /// <summary>
        /// 数值最小值
        /// </summary>
        public double MinValue
        {
            get
            {
                return _MinValue; 
            }
            set
            {
                _MinValue = value; 
            }
        }


        /// <summary>
        /// 是否需要判断小数位数
        /// </summary>
        public bool CheckDecimalDigits
        {
            get
            {
                return (this._EleStates & 16) != 0;
            }
            set
            {
                this._EleStates = value ? (this._EleStates | 16) : (this._EleStates & ~16);
            }
        }

        private int _MaxDecimalDigits = 0;

        /// <summary>
        /// 小数最大位数
        /// </summary>
        public int MaxDecimalDigits
        {
            get
            {
                return _MaxDecimalDigits;
            }
            set
            {
                _MaxDecimalDigits = value;
            }
        }

        /// <summary>
        /// 表示为空的日期数值
        /// </summary>
        public readonly static DateTime NullDateTime =  new DateTime(1980, 1, 1, 0, 0, 0);

        private DateTime _DateTimeMaxValue = NullDateTime;
        /// <summary>
        /// 最大时间日期值
        /// </summary>
        public DateTime DateTimeMaxValue
        {
            get
            {
                return _DateTimeMaxValue;
            }
            set
            {
                _DateTimeMaxValue = value;
            }
        }


        private DateTime _DateTimeMinValue = NullDateTime;
        /// <summary>
        /// 最小时间日期值
        /// </summary>
        public DateTime DateTimeMinValue
        {
            get
            {
                return _DateTimeMinValue;
            }
            set
            {
                _DateTimeMinValue = value;
            }
        }

        private int _ContentVersion = -1;
        /// <summary>
        /// 进行数据校验时的相关的内容版本号
        /// </summary>
        internal int ContentVersion
        {
            get
            {
                return _ContentVersion; 
            }
            set
            {
                _ContentVersion = value; 
            }
        }

        /// <summary>
        /// 对象没有任何有效设置
        /// </summary>
         
        public bool IsEmpty
        {
            get
            {
                if (this.Required)
                {
                    return false;
                }
                if (_ValueType == ValueTypeStyle.Text)
                {
                    if (this.CheckMaxValue || this.CheckMinValue)
                    {
                        return false;
                    }
                }
                else
                {
                    return false ;
                }
                if (this._ValueName != null && _ValueName.Trim().Length > 0)
                {
                    return false;
                }
                return true;
            }
        }
        private string strMessage = null;
        /// <summary>
        /// 验证结果
        /// </summary>
        internal string Message
        {
            get 
            {
                return strMessage; 
            }
            set
            {
                strMessage = value; 
            }
        }

        /// <summary>
        /// 由于必填项而导致的数据校验无效的标记
        /// </summary>
        internal bool RequiredInvalidateFlag
        {
            get
            {
                return (this._EleStates & 32) != 0;
            }
            set
            {
                this._EleStates = value ? (this._EleStates | 32) : (this._EleStates & ~32);
            }
        }

#if !RELEASE
        /// <summary>
        /// 返回表示对象的文本
        /// </summary>
        /// <returns>文本</returns>
        public override string ToString()
        {
            return ToStyleString();// "Type:" + _ValueType.ToString();
        }
        /// <summary>
        /// 返回style样式的文本
        /// </summary>
        /// <returns>文本</returns>
        public string ToStyleString()
        {
            StringBuilder myStr = new StringBuilder();
            if(this._MaxLength > 0 )
            {
                AddItem(myStr, "MaxLength", this._MaxLength.ToString());
            }
            if( _ValueName != null && _ValueName.Trim().Length > 0 )
            {
                AddItem( myStr , "ValueName" , _ValueName );
            }
            if (this.Required)
            {
                AddItem(myStr, "Required", "true");
            }
            if (_ValueType != ValueTypeStyle.Text)
            {
                AddItem(myStr, "ValueType", _ValueType.ToString());
            }
            if (_ValueType == ValueTypeStyle.Text)
            {
                if ( this.CheckMaxValue && this.MaxLength > 0)
                {
                    AddItem(myStr, "MaxLength", this.MaxLength.ToString());
                }
                if ( this.CheckMinValue && this.MinLength > 0)
                {
                    AddItem(myStr, "MinLength", this.MinLength.ToString());
                }
            }
            if (_ValueType == ValueTypeStyle.Numeric
                || _ValueType == ValueTypeStyle.Integer)
            {
                if ( this.CheckMaxValue && DoubleNaN.IsNaN(_MaxValue) == false)
                {
                    AddItem(myStr, "MaxValue", _MaxValue.ToString());
                }
                if ( this.CheckMinValue && DoubleNaN.IsNaN(_MinValue) == false)
                {
                    AddItem(myStr, "MinValue", _MinValue.ToString());
                }
                if (this.CheckDecimalDigits && this.MaxDecimalDigits > 0)
                {
                    AddItem(myStr, "MaxDecimalDigits", this.MaxDecimalDigits.ToString());
                }
            }
            if (_ValueType == ValueTypeStyle.Date
                || _ValueType == ValueTypeStyle.Time
                || _ValueType == ValueTypeStyle.DateTime)
            {
                if (_DateTimeMaxValue != DateTime.MinValue)
                {
                    if (this.CheckMaxValue && this.DateTimeMaxValue != NullDateTime)
                    {
                        AddItem(myStr, "DateTimeMaxValue", DateTimeCommon.FastToYYYY_MM_DD_HH_MM_SS( _DateTimeMaxValue));
                    }
                    if (this.CheckMinValue && this.DateTimeMinValue != NullDateTime )
                    {
                        AddItem(myStr, "DateTimeMaxValue", DateTimeCommon.FastToYYYY_MM_DD_HH_MM_SS(_DateTimeMinValue));
                    }
                }
            }
            return myStr.ToString();
        }
        private void AddItem(StringBuilder str, string name, string Value)
        {
            if (str.Length > 0)
            {
                str.Append(";");
            }
            str.Append(name + ":" + Value);
        }
#endif

    
        /// <summary>
        /// 复制对象
        /// </summary>
        /// <returns>复制品</returns>
        public ValueValidateStyle Clone()
        {
            return (ValueValidateStyle)this.MemberwiseClone();
        }
    }


    /// <summary>
    /// 数据校验无效等级
    /// </summary>
    [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
    [System.Text.Json.Serialization.JsonConverter(typeof(System.Text.Json.Serialization.JsonStringEnumConverter))]
    public enum ValueValidateLevel
    {
        /// <summary>
        /// 错误
        /// </summary>
        Error = 0,
        /// <summary>
        /// 警告
        /// </summary>
        Warring,
        /// <summary>
        /// 只是提醒
        /// </summary>
        Info
    }

    /// <summary>
    /// 数据类型
    /// </summary>
    [System.Reflection.Obfuscation(Exclude = true,ApplyToMembers=true)]
    [System.Text.Json.Serialization.JsonConverter(typeof(System.Text.Json.Serialization.JsonStringEnumConverter))]
    public enum ValueTypeStyle
    {
        /// <summary>
        /// 文本
        /// </summary>
        Text ,
        /// <summary>
        /// 整数
        /// </summary>
        Integer,
        /// <summary>
        /// 数值
        /// </summary>
        Numeric ,
        /// <summary>
        /// 日期
        /// </summary>
        Date ,
        /// <summary>
        /// 时间
        /// </summary>
        Time ,
        /// <summary>
        /// 日期时间
        /// </summary>
        DateTime
    }

}
