using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel ;
using DCSoft.Writer.Data;
using System.Runtime.InteropServices;
using DCSoft.Common;

namespace DCSoft.Writer.Dom
{
    /// <summary>
    /// 输入设置
    /// </summary>
    [Serializable]
    //[TypeConverter(typeof(DCSoft.Common.TypeConverterSupportProperties))]
   [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = false )]
    //[DCSoft.Common.DCPublishAPI]
    public partial class InputFieldSettings : ICloneable 
    {
        private static string[] _SupportCustomListSourceNames = null;
        /// <summary>
        /// 支持的自定义列表来源名称
        /// </summary>
        //[DCSoft.Common.DCPublishAPI]
       //[System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public static string[] SupportCustomListSourceNames
        {
            get
            {
                return _SupportCustomListSourceNames; 
            }
            set
            {
                _SupportCustomListSourceNames = value; 
            }
        }
        /// <summary>
        /// 初始化对象
        /// </summary>
        //[DCSoft.Common.DCPublishAPI]
        public InputFieldSettings()
        {
        }

        //private bool _UserEditable = true;
        ///// <summary>
        ///// 用户可以直接修改文本域中的内容
        ///// </summary>
        //[System.ComponentModel.DefaultValue(true)]
        //public bool UserEditable
        //{
        //    get
        //    {
        //        return _UserEditable;
        //    }
        //    set
        //    {
        //        _UserEditable = value;
        //    }
        //}

        private const int Mask_GetValueOrderByTime = 1;
        private const int Mask_MultiColumn = 2;
        private const int Mask_RepulsionForGroup = 4;
        private const int Mask_MultiSelect = 8;
        private const int Mask_DynamicListItems = 16;

        private int _EleStates = 0;

        private InputFieldEditStyle _EditStyle = InputFieldEditStyle.Text;
        /// <summary>
        /// 输入方式
        /// </summary>
        [DefaultValue( InputFieldEditStyle.Text )]
        //[DCSoft.Common.DCPublishAPI]
       //[System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public InputFieldEditStyle EditStyle
        {
            get
            {
                return _EditStyle; 
            }
            set
            {
                _EditStyle = value; 
            }
        }


        /// <summary>
        /// 允许多选列表项目
        /// </summary>
        [DefaultValue( false )]
        //[DCSoft.Common.DCPublishAPI]
       //[System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public bool MultiSelect
        {
            get
            {
                return (this._EleStates & Mask_MultiSelect) != 0;
            }
            set
            {
                this._EleStates = value ? (this._EleStates | Mask_MultiSelect) : (this._EleStates & ~Mask_MultiSelect);
            }
        }

        
        private ListSourceInfo _ListSource;
        /// <summary>
        /// 列表内容来源
        /// </summary>
        public ListSourceInfo ListSource
        {
            get 
            {
                return _ListSource; 
            }
            set
            {
                _ListSource = value; 
            }
        }


        private static readonly string _Default_ListValueSeparatorChar = ",";
        private string _ListValueSeparatorChar = _Default_ListValueSeparatorChar;
        /// <summary>
        /// 列表值之间的分隔字符
        /// </summary>
        [DefaultValue( ",")]
        //[DCSoft.Common.DCPublishAPI]
       //[System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public string ListValueSeparatorChar
        {
            get
            {
                return _ListValueSeparatorChar; 
            }
            set
            {
                _ListValueSeparatorChar = value; 
            }
        }
         
        /// <summary>
        /// 复制对象
        /// </summary>
        /// <returns>复制品</returns>
        public InputFieldSettings Clone()
        {
            return (InputFieldSettings)((ICloneable)this).Clone();
        }
    

        /// <summary>
        /// 复制对象
        /// </summary>
        /// <returns>复制品</returns>
        object ICloneable.Clone()
        {
            InputFieldSettings instance = (InputFieldSettings)this.MemberwiseClone();
            if (this._ListSource != null)
            {
                instance._ListSource = this._ListSource.Clone();
            }
            
            return instance;
        }
#if !RELEASE
        /// <summary>
        /// 返回表示对象的字符串
        /// </summary>
        /// <returns>字符串</returns>
        public override string ToString()
        {
            if (this.EditStyle == InputFieldEditStyle.Text)
            {
                return "Text";
            }
            else if (this.EditStyle == InputFieldEditStyle.DropdownList)
            {
                if (this.ListSource == null)
                {
                    return "None list item";
                }
                else
                {
                    return "List:" + this.ListSource.ToString();
                }
            }
            else if (this.EditStyle == InputFieldEditStyle.Date)
            {
                return "DateTime ";
            }
            return string.Empty;
        }
#endif
    }
    /// <summary>
    /// 文本输入域输入方式
    /// </summary>
   [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
    public enum InputFieldEditStyle
    {
        /// <summary>
        /// 直接输入纯文本
        /// </summary>
        Text = 0,
        /// <summary>
        /// 下拉列表方式
        /// </summary>
        DropdownList = 1,
        /// <summary>
        /// 日期类型
        /// </summary>
        Date = 2,
        /// <summary>
        /// 时间日期类型
        /// </summary>
        DateTime = 3,
        /// <summary>
        /// 时间类型
        /// </summary>
        Time = 5,
        /// <summary>
        /// 数值型
        /// </summary>
        Numeric = 6
    }

     
}
