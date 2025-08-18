using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using DCSoft.Writer.Dom;

namespace DCSoft.Writer.Controls
{

    /// <summary>
    /// 解析被选中文本的事件委托类型
    /// </summary>
    /// <param name="eventSender">参数</param>
    /// <param name="args">参数</param>
    [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
    public delegate void ParseSelectedItemsEventHandler(object eventSender, ParseSelectedItemsEventArgs args);

    /// <summary>
    /// 解释被选中项目的事件参数
    /// </summary>
    [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = false)]
    //[DCSoft.Common.DCPublishAPI]
    public partial class ParseSelectedItemsEventArgs  
    {
        /// <summary>
        /// 初始化对象
        /// </summary>
        /// <param name="ctl"></param>
        /// <param name="formatString"></param>
        /// <param name="document"></param>
        /// <param name="element"></param>
        /// <param name="text"></param>
        /// <param name="items"></param>
        /// <param name="separator"></param>
        public ParseSelectedItemsEventArgs(
            WriterControl ctl,
            string formatString,
            DomDocument document,
            DomElement element,
            string text,
            string[] items,
            string separator)
        {
            this._WriterControl = ctl;
            this._FormatString = formatString;
            this._Document = document;
            if( this._Document == null && element != null )
            {
                this._Document = element.OwnerDocument;
            }
            this._Element = element;
            this._Text = text;
            this._Items = items;
            this._Separator = separator;
            if (document == null && element != null)
            {
                document = element.OwnerDocument;
            }
        }

        private WriterControl _WriterControl = null;
        /// <summary>
        /// 编辑器控件对象
        /// </summary>
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        [System.Text.Json.Serialization.JsonIgnore]
        public WriterControl WriterControl
        {
            get
            {
                return _WriterControl;
            }
        }

        private DomDocument _Document = null;
        /// <summary>
        /// 文档对象
        /// </summary>
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        [System.Text.Json.Serialization.JsonIgnore]
        public DomDocument Document
        {
            get
            {
                return _Document;
            }
        }

        private DomElement _Element = null;
        /// <summary>
        /// 相关的文档元素对象
        /// </summary>
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        [System.Text.Json.Serialization.JsonIgnore]
        public DomElement Element
        {
            get
            {
                return _Element;
            }
        }


        private string _Separator = ",";
        /// <summary>
        /// 各个项目之间的分隔字符
        /// </summary>
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public string Separator
        {
            get
            {
                return _Separator;
            }
        }

        /// <summary>
        /// 各个项目之间的分隔字符
        /// </summary>
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public char SeparatorChar
        {
            get
            {
                if (_Separator != null && _Separator.Length > 0)
                {
                    return _Separator[0];
                }
                else
                {
                    return (char)0;
                }
            }
        }

        private string _FormatString = null;
        /// <summary>
        /// 格式化字符串
        /// </summary>
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public string FormatString
        {
            get
            {
                return _FormatString;
            }
        }

        private string _Text = null;
        /// <summary>
        /// 文本
        /// </summary>
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public string Text
        {
            get
            {
                return _Text;
            }
        }

        private string[] _Items = null;
        /// <summary>
        /// 项目列表
        /// </summary>
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public string[] Items
        {
            get
            {
                return _Items;
            }
        }

        private string[] _Result = null;
        /// <summary>
        /// 计算结果
        /// </summary>
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public string[] Result
        {
            get
            {
                return _Result;
            }
            set
            {
                _Result = value;
            }
        }
    }
}
