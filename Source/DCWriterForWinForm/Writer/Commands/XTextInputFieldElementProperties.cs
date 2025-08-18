using System;
using System.Collections.Generic;
using System.Text;
using DCSoft.Common;
using DCSoft.Writer.Dom;
using DCSoft.Writer.Data;
using DCSoft.Data;
using System.ComponentModel;

namespace DCSoft.Writer.Commands
{
    /// <summary>
    /// 文本输入域设置信息对象
    /// </summary>
    internal class XTextInputFieldElementProperties : XTextElementProperties
    {
        /// <summary>
        /// 初始化对象
        /// </summary>
        public XTextInputFieldElementProperties()
        { 
        }

        /// <summary>
        /// 初始化对象
        /// </summary>
        /// <param name="field">要读取设置的文本输入域对象</param>
        public XTextInputFieldElementProperties(DomInputFieldElement field)
        {
            if (field != null)
            {
                ReadProperties(field);
            }
        }

        /// <summary>
        /// 读取属性值
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public override bool ReadProperties(DomElement element)
        {
            DomInputFieldElement field = (DomInputFieldElement)element;
            _ID = field.ID;
            _Name = field.Name;
            _UserEditable = field.UserEditable;
            _ValidateStyle = field.ValidateStyle;
            _FieldSettings = field.FieldSettings;
            _DisplayFormat = field.DisplayFormat;
            _ToolTip = field.ToolTip;
            _BackgroundText = field.BackgroundText;
            this.EnableHighlight = field.EnableHighlight;
            //this.MultiParagraph = field.MultiParagraph;
            return true;
        }

        private string _ID = null;
        /// <summary>
        /// 编号
        /// </summary>
        public string ID
        {
            get { return _ID; }
            set { _ID = value; }
        }


        private EnableState _EnableHighlight = EnableState.Enabled;
        /// <summary>
        /// 是否允许高亮度显示状态
        /// </summary>
        [DefaultValue(EnableState.Enabled)]
        public EnableState EnableHighlight
        {
            get
            {
                return _EnableHighlight;
            }
            set
            {
                _EnableHighlight = value;
            }
        }


        private string _Name = null;
        /// <summary>
        /// 字段名称
        /// </summary>
        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }

        private string _ToolTip = null;
        /// <summary>
        /// 提示文本
        /// </summary>
        [DefaultValue(null)]
        public string ToolTip
        {
            get
            {
                return _ToolTip;
            }
            set
            {
                _ToolTip = value;
            }
        }

        private string _BackgroundText = null;
        /// <summary>
        /// 背景文本
        /// </summary>
        [DefaultValue(null)]
        public string BackgroundText
        {
            get
            {
                return _BackgroundText;
            }
            set
            {
                _BackgroundText = value;
            }
        }

        private bool _UserEditable = true;
        /// <summary>
        /// 用户可以直接修改文本域中的内容
        /// </summary>
        [System.ComponentModel.DefaultValue(true)]
        public bool UserEditable
        {
            get
            {
                return _UserEditable;
            }
            set
            {
                _UserEditable = value;
            }
        }

        private ValueFormater _DisplayFormat = null;
        /// <summary>
        /// 显示的格式化对象
        /// </summary>
        public ValueFormater DisplayFormat
        {
            get
            {
                return _DisplayFormat;
            }
            set
            {
                _DisplayFormat = value;
            }
        }



        private string _Text = null;
        /// <summary>
        /// 文本值 
        /// </summary>
        public string Text
        {
            get { return _Text; }
            set { _Text = value; }
        }

        private string _InitalizeInnerValue = null;
        /// <summary>
        /// 初始化的表单值，本属性只能用于新增元素操作
        /// </summary>
        public string InitalizeInnerValue
        {
            get
            {
                return _InitalizeInnerValue; 
            }
            set
            {
                _InitalizeInnerValue = value; 
            }
        }
        

        private string _InitalizeText = null;
        /// <summary>
        /// 初始化的文本值，本属性只能用于新增元素操作
        /// </summary>
        public string InitalizeText
        {
            get 
            {
                return _InitalizeText; 
            }
            set
            {
                _InitalizeText = value; 
            }
        }

        //private string _Text = null;
        ///// <summary>
        ///// 文本值
        ///// </summary>
        //public string Text
        //{
        //    get 
        //    {
        //        return _Text; 
        //    }
        //    set
        //    {
        //        _Text = value; 
        //    }
        //}

        private InputFieldSettings _FieldSettings = null;
        /// <summary>
        /// 输入域设置
        /// </summary>
        public InputFieldSettings FieldSettings
        {
            get
            {
                return _FieldSettings;
            }
            set
            {
                _FieldSettings = value;
            }
        }

        private ValueValidateStyle _ValidateStyle = null;
        /// <summary>
        /// 数据校验格式
        /// </summary>
        public ValueValidateStyle ValidateStyle
        {
            get { return _ValidateStyle; }
            set { _ValidateStyle = value; }
        }

        internal bool _ValidateStyleModified = true;
        /// <summary>
        /// 根据对象设置创建输入域元素对象
        /// </summary>
        /// <param name="document">文档对象</param>
        /// <returns>创建的输入域元素对象</returns>
        public override DomElement CreateElement(DomDocument document)
        {
            DomInputFieldElement element = new DomInputFieldElement();
            element.Elements = new DomElementList();
            element.OwnerDocument = document;
            ApplyToElement(document ,  element , false );
            if (string.IsNullOrEmpty(this.InitalizeText) == false)
            {
                element.SetInnerTextFast(this.InitalizeText);
            }
            if (string.IsNullOrEmpty(this.InitalizeInnerValue) == false)
            {
                element.InnerValue = this.InitalizeInnerValue;
            }
            return element;
        }

        /// <summary>
        /// 应用到元素中
        /// </summary>
        /// <param name="document">文档对象</param>
        /// <param name="element">元素对象</param>
        /// <param name="logUndo">是否记录撤销信息</param>
        /// <returns>操作是否成功</returns>
        public override bool ApplyToElement(DomDocument document, DomElement element, bool logUndo)
        {
            DomInputFieldElement field = (DomInputFieldElement)element;
            if (_Name != null)
            {
                _Name = _Name.Trim();
            }
            bool result = false;
            if (field.ID != this.ID)
            {
                if (logUndo && document.CanLogUndo)
                {
                    document.UndoList.AddProperty("ID", field.ID, this.ID, field);
                }
                field.ID = this.ID;
                result = true;
            }
            //if (field.EventExpressions != this.EventExpressions)
            //{
            //    if (logUndo && document.CanLogUndo)
            //    {
            //        document.UndoList.AddProperty(
            //            "EventExpressions",
            //            field.EventExpressions,
            //            this.EventExpressions,
            //            field);
            //    }
            //    field.EventExpressions = this.EventExpressions;
            //    result = true;
            //}
            if (field.EnableHighlight != this.EnableHighlight)
            {
                if (logUndo && document.CanLogUndo)
                {
                    document.UndoList.AddProperty("EnableHighlight",
                        field.EnableHighlight,
                        this.EnableHighlight,
                        field);
                }
                field.EnableHighlight = this.EnableHighlight;
                result = true;
            }
            //if (field.MultiParagraph != this.MultiParagraph)
            //{
            //    if (logUndo && document.CanLogUndo)
            //    {
            //        document.UndoList.AddProperty(
            //            "MultiParagraph",
            //            field.MultiParagraph,
            //            this.MultiParagraph,
            //            field);
            //    }
            //    field.MultiParagraph = this.MultiParagraph;
            //    result = true;
            //}
            if ( field.Name != this.Name)
            {
                if ( logUndo && document.CanLogUndo)
                {
                    document.UndoList.AddProperty("Name", field.Name, this.Name, field);
                }
                field.Name = this.Name;
                result = true;
            }


            if (field.ToolTip != this.ToolTip)
            {
                if (logUndo && document.CanLogUndo)
                {
                    document.UndoList.AddProperty("ToolTip", field.ToolTip, this.ToolTip, field);
                }
                field.ToolTip = this.ToolTip;
                result = true;
            }

            if (field.BackgroundText != this.BackgroundText)
            {
                if (logUndo && document.CanLogUndo)
                {
                    document.UndoList.AddProperty("BackgroundText",
                        field.BackgroundText,
                        this.BackgroundText,
                        field);
                }
                field.BackgroundText = this.BackgroundText;
                result = true;
            }

            if (field.UserEditable != this.UserEditable)
            {
                if (logUndo && document.CanLogUndo)
                {
                    document.UndoList.AddProperty("UserEditable", field.UserEditable, this.UserEditable, field);
                }
                field.UserEditable = this.UserEditable;
                result = true;
            }
            if ( field.ValidateStyle != this.ValidateStyle )
            {
                if ( logUndo && document.CanLogUndo)
                {
                    document.UndoList.AddProperty("ValidateStyle", field.ValidateStyle, this.ValidateStyle , field);
                }
                field.ValidateStyle = this.ValidateStyle ;
                result = true;
            }
            if (field.FieldSettings != this.FieldSettings)
            {
                if (logUndo && document.CanLogUndo)
                {
                    document.UndoList.AddProperty("FieldSettings", field.FieldSettings, this.FieldSettings, field);
                }
                field.FieldSettings = this.FieldSettings;
                result = true;
            }
            if (field.DisplayFormat != this.DisplayFormat)
            {
                if (logUndo && document.CanLogUndo)
                {
                    document.UndoList.AddProperty("DisplayFormat", field.DisplayFormat, this.DisplayFormat, field);
                }
                field.DisplayFormat = this.DisplayFormat;
                result = true;
            }
            if (result)
            {
                ContentChangedEventArgs args = new ContentChangedEventArgs();
                args.Document = field.OwnerDocument;
                args.Element = field;
                args.LoadingDocument = false;
                field.RaiseBubbleOnContentChanged(args);
            }
            return result;
        }

    }
}
