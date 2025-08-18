using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel ;
using DCSoft.Data ;
using DCSoft.Writer.Data;
using DCSoft.Writer.Controls ;
using DCSoft.Drawing;
using DCSoft.Writer.Serialization;
using System.Runtime.InteropServices;
using System.Reflection ;
using DCSoft.Common;

namespace DCSoft.Writer.Dom
{
    /// <summary>
    /// 纯文本数据输入域
    /// </summary>
#if !RELEASE
    [System.Diagnostics.DebuggerDisplay("Input Name:{ID}")]
#endif
    public partial class DomInputFieldElement :
        DomInputFieldElementBase
    {
        public static readonly string TypeName_XTextInputFieldElement = "XTextInputFieldElement";
        public override string TypeName => TypeName_XTextInputFieldElement;

        /// <summary>
        /// 初始化对象
        /// </summary>
        public DomInputFieldElement()
        {
            this.EnableUserEditInnerValue = true;
        }

        /// <summary>
        /// 扩展的文本和数值映射表
        /// </summary>
        [NonSerialized]
        private List<string> _ExtTextValueMaps;
        /// <summary>
        /// 添加扩展的文本和数值映射
        /// </summary>
        /// <param name="text"></param>
        /// <param name="innerValue"></param>
        internal void AddExtTextValueMap(string text, string innerValue)
        {
            if (text != innerValue)
            {
                if (this._ExtTextValueMaps == null)
                {
                    this._ExtTextValueMaps = new List<string>();
                }
                for (int iCount = this._ExtTextValueMaps.Count - 2; iCount >= 0; iCount -= 2)
                {
                    if (this._ExtTextValueMaps[iCount] == text)
                    {
                        return;
                    }
                }
                this._ExtTextValueMaps.Add(text);
                this._ExtTextValueMaps.Add(innerValue);
            }
        }
        /// <summary>
        /// 根据扩展的文本和数值的映射将文本转换为InnerValue值
        /// </summary>
        /// <param name="text">文本值</param>
        /// <returns>InnerValue值</returns>
        internal string ConvertToInnerValueByExtTextValueMap(string text)
        {
            if (this._ExtTextValueMaps != null)
            {
                for (int iCount = this._ExtTextValueMaps.Count - 2; iCount >= 0; iCount -= 2)
                {
                    if (this._ExtTextValueMaps[iCount] == text)
                    {
                        return this._ExtTextValueMaps[iCount + 1];
                    }
                }
            }
            return text;
        }

        public override void InnerGetText(GetTextArgs args)
        {
            var oldLen = args.Length;
            int contentLength = args.Length;
            if (this._Elements != null && this._Elements.Count > 0)
            {
                // 输出内容
                base.InnerGetText(args);
            }
            if (contentLength == args.Length && args.IncludeBackgroundText)
            {
                // 输出背景文本
                args.Append(this.BackgroundText);
            }
        }
#if !RELEASE
        public override string ToString()
        {
            return "[" + this.Text + "]";
        }
#endif

        /// <summary>
        /// 文档元素编号前缀
        /// </summary>
        public override string ElementIDPrefix()
        {

            return "field";

        }



        /// <summary>
        /// 允许用户直接编辑内容而修改InnerValue值。
        /// </summary>
        public bool EnableUserEditInnerValue
        {
            get
            {
                return this.StateFlag19;// _EnableUserEditInnerValue;
            }
            set
            {
                this.StateFlag19 = value;// _EnableUserEditInnerValue = value;
            }
        }

        /// <summary>
        /// 重置对象内容为默认值
        /// </summary>
        /// <returns>操作是否修改了输入域内容</returns>
        public virtual bool ResetToDefaultValue()
        {
            if (this._Elements != null && this._Elements.Count > 0)
            {
                this._Elements.Clear();
                return true;
            }
            else
            {
                return false;
            }
        }

        private ValueEditorActiveMode _EditorActiveMode =
            ValueEditorActiveMode.F2
            | ValueEditorActiveMode.MouseDblClick
            | ValueEditorActiveMode.Program;
        /// <summary>
        /// 数值编辑器激活模式
        /// </summary>
        [DefaultValue(ValueEditorActiveMode.F2
            | ValueEditorActiveMode.MouseDblClick
            | ValueEditorActiveMode.Program)]
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public ValueEditorActiveMode EditorActiveMode
        {
            get
            {
                return _EditorActiveMode;
            }
            set
            {
                _EditorActiveMode = value;// (ValueEditorActiveMode)WriterUtilsInner.FixEnumValue(value);
            }
        }
        /// <summary>
        /// 运行时使用的数值编辑器激活模式
        /// </summary>
        internal ValueEditorActiveMode RuntimeEditorActiveMode()
        {

            ValueEditorActiveMode result = this.EditorActiveMode;
            if (result == ValueEditorActiveMode.Default)
            {
                if (this.OwnerDocument == null)
                {
                    result = ValueEditorActiveMode.None;
                }
                else
                {
                    result = GetDocumentBehaviorOptions().DefaultEditorActiveMode;
                }
            }
            return result;

        }
        

        private InputFieldSettings _FieldSettings;
        /// <summary>
        /// 输入域设置
        /// </summary>
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
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

        /// <summary>
        /// 开始编辑元素数值
        /// </summary>
        /// <returns>操作是否成功</returns>
        public virtual bool BeginEditValue()
        {
            if (this.OwnerDocument != null
                && this.OwnerDocument.EditorControl != null)
            {
                return this.OwnerDocument.EditorControl.InnerBeginEditElementValue(
                    this,
                    false,
                    ValueEditorActiveMode.Program,
                    false,
                    false);
            }
            return false;
        }
        /// <summary>
        /// 绝对边界
        /// </summary>
        public override RectangleF AbsBounds
        {
            get
            {
                DomElement se = null;
                DomElement ee = null;
                if (this._StartElement != null
                    && this._StartElement.OwnerLine != null
                    && this._EndElement != null
                    && this._EndElement.OwnerLine != null)
                {
                    se = this._StartElement;
                    ee = this._EndElement;
                }
                else
                {
                    var ce = this.DocumentContentElement?.Content;
                    if (ce != null && this._ContentIndex >= 0 && this._ContentIndex < ce.Count)
                    {
                        var e2 = ce[this._ContentIndex];
                        var arr = this._InnerBackgroundTextElements;
                        if (arr != null
                            && arr.Length > 0
                            && arr.ArrayContains<DomElement>(e2))
                        {
                            se = e2;
                            ee = arr[arr.Length - 1];
                        }
                        else if (e2.IsParentOrSupParent(this))
                        {
                            se = e2;
                            ee = e2;
                            for (var iCount = this._ContentIndex + 1; iCount < ce.Count; iCount++)
                            {
                                var e3 = ce[iCount];
                                if (e3.IsParentOrSupParent(this))
                                {
                                    ee = e3;
                                }
                                else
                                {
                                    break;
                                }
                            }
                        }
                    }
                }
                if (se != null && ee != null)
                {
                    RectangleF rect1 = se.AbsBounds;
                    RectangleF rect2 = ee.AbsBounds;
                    return RectangleF.Union(rect1, rect2);
                }
                else
                {
                    return RectangleF.Empty;
                }
            }
        }

        /// <summary>
        /// 获得运行时使用的下拉列表项目
        /// </summary>
        /// <param name="spellCode">检索用的拼音码</param>
        /// <returns>获得的列表对象</returns>

        public ListItemCollection GetRuntimeListItems()
        {
            return this._FieldSettings?.ListSource?.Items; 
        }
        /// <summary>
        /// 输入域获得输入焦点
        /// </summary>
        /// <param name="args">参数</param>
        public override void OnGotFocus(ElementEventArgs args)
        {
            base.OnGotFocus(args);
            if (WriterUtilsInner.HasFlag(
                (int)this.RuntimeEditorActiveMode(),
                (int)ValueEditorActiveMode.GotFocus)
                && this.DocumentContentElement.Selection.Length == 0
                //&& this._DisableActiveEditorOnce == false
                && this.WriterControl != null)
            {
                //this._DisableActiveEditorOnce = false;
                var host = this.WriterControl.EditorHost();
                if (host.CurrentEditContext == null
                    || host.CurrentEditContext.Element != this
                    || host.CurrentEditContext.EditStyle
                                != Controls.ElementValueEditorEditStyle.Modal)
                {
                    InputFieldEditStyle editStyle = InputFieldEditStyle.Text;
                    if (this.FieldSettings != null)
                    {
                        editStyle = this.FieldSettings.EditStyle;
                    }
                    if (this.FieldSettings != null
                        && this.WriterControl != null
                        && editStyle == InputFieldEditStyle.DropdownList)
                    {
                        //DCConsole.Default.WriteLine(this.BackgroundText);
                        if (this.FieldSettings.ListSource != null && this.FieldSettings.ListSource.IsEmpty == false)
                        {
                            this.OwnerDocument.EditorControl.InnerBeginEditElementValueAllowBeginInvoke(
                               this,
                               false,
                               ValueEditorActiveMode.GotFocus,
                               true,
                               true);
                        }
                    }
                    else if (editStyle == InputFieldEditStyle.Date
                        || editStyle == InputFieldEditStyle.DateTime
                        || editStyle == InputFieldEditStyle.Numeric
                        || editStyle == InputFieldEditStyle.Time)
                    {
                        this.OwnerDocument.EditorControl.InnerBeginEditElementValueAllowBeginInvoke(
                            this,
                            false,
                            ValueEditorActiveMode.GotFocus,
                            true,
                            true);
                    }
                }
            }
        }

        /// <summary>
        /// 输入域失去输入焦点
        /// </summary>
        /// <param name="args">参数</param>
        public override void OnLostFocus(ElementEventArgs args)
        {
            base.OnLostFocus(args);
            if (this.WriterControl != null)
            {
                this.WriterControl.CancelEditElementValue();
            }
        }
        /// <summary>
        /// 设置选中的列表项目
        /// </summary>
        /// <param name="indexs">从0开始计算的项目编号，各个编号之间用逗号分开</param>
        /// <returns>操作是否修改了文档内容</returns>
        public bool EditorSetSelectedIndexs(string indexs)
        {
            int[] ids = DCSoft.Common.StringConvertHelper.ToInt32Values(indexs);
            string txt = WriterToolsBase.Instance.FormatSelectedValueByIndexs(
                this.OwnerDocument.EditorControl,
                this,
                ids,
                true);
            bool result = this.SetEditorTextExt(txt, DomAccessFlags.None, true);
            return result;
        }

        public override DomElementList SetInnerTextFastExt(
            string txt,
            DocumentContentStyle newTextStyle,
            bool updateContentElements)
        {
            DomElementList list = base.SetInnerTextFastExt(
                txt,
                newTextStyle,
                updateContentElements);
            if (list != null && list.Count > 0)
            {
                this.UpdateInnerValue(true);
            }
            return list;
        }

        /// <summary>
        /// 将前台文本转换为后台数值
        /// </summary>
        /// <param name="text">文本</param>
        /// <returns>数值</returns>
        public override string TextToValue(string text)
        {
            bool raiseEvent = false;
            ListItemCollection items = this.GetRuntimeListItems();
            if (items == null)
            {
                return ConvertToInnerValueByExtTextValueMap(text);
            }
            if (items.Count > 0)
            {
                if (this.FieldSettings.MultiSelect)
                {
                    List<string> allTexts = new List<string>();
                    foreach (ListItem item in items)
                    {
                        if (string.IsNullOrEmpty(item.Text) == false)
                        {
                            allTexts.Add(item.Text);
                        }
                        else
                        {
                            allTexts.Add(item.Value);
                        }
                    }
                    // 多选列表
                    string spliter = this.FieldSettings.ListValueSeparatorChar;
                    string[] vs = null;
                    vs = WriterToolsBase.Instance.ParseSelectedValue(
                        this.OwnerDocument.EditorControl,
                        this,
                        allTexts,
                        spliter,
                        text);
                    if (vs == null || vs.Length == 0)
                    {
                        return string.Empty;
                    }
                    StringBuilder result = new StringBuilder();
                    foreach (ListItem item in items)
                    {
                        string v = string.IsNullOrEmpty(item.Text) ? item.Value : item.Text;
                        bool match = false;
                        for (int iCount3 = 0; iCount3 < vs.Length; iCount3++)
                        {
                            if (vs[iCount3] == v)
                            {
                                vs[iCount3] = null;
                                match = true;
                                break;
                            }
                        }
                        if (match)
                        {
                            if (result.Length > 0)
                            {
                                if (string.IsNullOrEmpty(spliter) == false)
                                {
                                    result.Append(spliter);
                                }
                            }
                            if (string.IsNullOrEmpty(item.Value))
                            {
                                result.Append(item.Text);
                            }
                            else
                            {
                                result.Append(item.Value);
                            }
                        }
                    }//foreach
                    return result.ToString();
                }
                else
                {
                    string result = items.TextToValue(text);
                    if (result == null)
                    {
                        result = text;
                    }
                    return result;
                }
            }
            return text;
        }

        /// <summary>
        /// 将后台数据转换为前台文本
        /// </summary>
        /// <param name="Value">原始文本</param>
        /// <returns>要显示的文本</returns>
        public override string ValueToText(string Value, bool structMode)
        {
            ListItemCollection items = this.GetRuntimeListItems();
            if (items != null)
            {
                if (this.FieldSettings.MultiSelect)
                {
                    List<string> allValues = new List<string>();
                    foreach (ListItem item in items)
                    {
                        if (string.IsNullOrEmpty(item.Value) == false)
                        {
                            allValues.Add(item.Value);
                        }
                        else
                        {
                            allValues.Add(item.Text);
                        }
                    }
                    // 多选列表
                    string spliter = this.FieldSettings.ListValueSeparatorChar;
                    string[] vs = null;
                    if (this.OwnerDocument != null)
                    {
                        vs = WriterToolsBase.Instance.ParseSelectedValue(
                            this.OwnerDocument.EditorControl,
                            this,
                            allValues,
                            spliter,
                            Value);
                    }
                    if (vs == null || vs.Length == 0)
                    {
                        return string.Empty;
                    }

                    StringBuilder result = new StringBuilder();
                    foreach (ListItem item in items)
                    {
                        string v = string.IsNullOrEmpty(item.Value) ? item.Text : item.Value;
                        bool match = false;
                        foreach (string v2 in vs)
                        {
                            if (v2 == v)
                            {
                                match = true;
                                break;
                            }
                        }
                        if (match)
                        {
                            if (result.Length > 0)
                            {
                                if (string.IsNullOrEmpty(spliter) == false)
                                {
                                    result.Append(spliter);
                                }
                            }
                            result.Append(item.Text);
                        }
                    }//foreach
                    return result.ToString();
                }
                else
                {
                    string result = items.ValueToText(Value);
                    if (result == null && structMode == false)
                    {
                        result = Value;
                    }
                    return result;
                }
            }
            return base.GetDisplayText(Value);
        }

        /// <summary>
        /// 根据对象内容更新InnerValue值。
        /// </summary>
        /// <param name="updateParent">是否更新各级父输入域的值</param>
        public virtual void UpdateInnerValue(bool updateParent)
        {
            if (this.FieldSettings != null)
            {
                if (this.FieldSettings.EditStyle == InputFieldEditStyle.Date
                    || this.FieldSettings.EditStyle == InputFieldEditStyle.DateTime)
                {
                    DateTime dtm = this.OwnerDocument.GetNowDateTime();
                    bool flag = false;
                    if (this.RuntimeDisplayFormat != null
                        && string.IsNullOrEmpty(this.RuntimeDisplayFormat.Format) == false)
                    {
                        flag = DateTime.TryParseExact(
                            this.Text,
                            this.RuntimeDisplayFormat.Format,
                            null,
                            System.Globalization.DateTimeStyles.AssumeLocal,
                            out dtm);
                    }
                    else
                    {
                        flag = DateTime.TryParse(this.Text, out dtm);
                    }
                    if (flag)
                    {
                        if (GetDocumentBehaviorOptions().DisplayFormatToInnerValue
                            && this.RuntimeDisplayFormat != null
                            && this.RuntimeDisplayFormat.IsEmpty == false)
                        {
                            this.InnerValue = this.RuntimeDisplayFormat.Execute(dtm);
                        }
                        else if (this.FieldSettings.EditStyle == InputFieldEditStyle.Date)
                        {
                            this.InnerValue = dtm.ToString(WriterConst.Format_YYYY_MM_DD);
                        }
                        else if (this.FieldSettings.EditStyle == InputFieldEditStyle.Time)
                        {
                            this.InnerValue = dtm.ToString("HH:mm:ss");
                        }
                        else
                        {
                            this.InnerValue = dtm.ToString(WriterConst.Format_YYYY_MM_DD_HH_MM_SS);
                        }

                    }
                    else
                    {
                        this.InnerValue = this.Text;
                    }
                }
                else if (this.FieldSettings.EditStyle == InputFieldEditStyle.Text)
                {
                    this.InnerValue = this.Text;
                }
                else if (this.FieldSettings.EditStyle == InputFieldEditStyle.DropdownList)
                {
                    // 在很多情况下，是对纯文本进行转换
                    if (this._Elements == null || this._Elements.Count == 0)
                    {
                        this.InnerValue = this.TextToValue(null);
                    }
                    else
                    {
                        var strText2 = new StringBuilder(this._Elements.Count);
                        var esArray = this._Elements.InnerGetArrayRaw();
                        var esCount = this._Elements.Count;
                        bool isOK = true;
                        for (var iCount = 0; iCount < esCount; iCount++)
                        {
                            var element = esArray[iCount];
                            if (element is DomCharElement)
                            {
                                    ((DomCharElement)element).AppendContent(strText2);
                            }
                            else
                            {
                                isOK = false;
                                break;
                            }
                        }
                        if (isOK)
                        {
                            this.InnerValue = this.TextToValue(strText2.ToString());
                        }
                        else
                        {
                            this.InnerValue = this.TextToValue(this.Text);
                        }
                    }
                }
                else
                {
                    this.InnerValue = this.Text;
                }

            }
            else
            {
                this.InnerValue = this.Text;
            }
            if (updateParent)
            {
                DomElement e = this.Parent;
                while (e != null)
                {
                    if (e is DomInputFieldElement)
                    {
                        ((DomInputFieldElement)e).UpdateInnerValue(false);
                    }
                    e = e.Parent;
                }
            }
        }

        /// <summary>
        /// 处理文档内容发生改变事件
        /// </summary>
        /// <param name="args">参数</param>
        public override void OnContentChanged(ContentChangedEventArgs args)
        {
            if (args.LoadingDocument)
            {
                base.OnContentChanged(args);
                return;
            }
            if (args.OnlyStyleChanged == false)
            {
                if (args.EventSource != ContentChangedEventSource.ValueEditor && this.EnableUserEditInnerValue)
                {
                    if (args.EventSource != ContentChangedEventSource.UndoRedo)
                    {
                        UpdateInnerValue(false);
                    }
                }
                // 执行联动列表操作
                base.OnContentChanged(args);
            }

        }



        /// <summary>
        /// DCWriter内部使用
        /// </summary>
        public InputFieldEditStyle InnerEditStyle
        {
            get
            {
                if (this._FieldSettings == null)
                {
                    return InputFieldEditStyle.Text;
                }
                else
                {
                    return this._FieldSettings.EditStyle;
                }
            }
        }

        /// <summary>
        /// 复制对象
        /// </summary>
        /// <param name="Deeply">是否深度复制</param>
        /// <returns>复制品</returns>
        public override DomElement Clone(bool Deeply)
        {
            DomInputFieldElement field = (DomInputFieldElement)base.Clone(Deeply);
            if (this._FieldSettings != null)
            {
                field._FieldSettings = this._FieldSettings.Clone();
            }
            return field;
        }

        public override void Dispose()
        {
            base.Dispose();
            if (this._ExtTextValueMaps != null)
            {
                this._ExtTextValueMaps.Clear();
                this._ExtTextValueMaps = null;
            }
            //if (this._FieldSettings != null)
            {
                this._FieldSettings = null;
            }
            this._FieldSettings = null;
        }

    }
}