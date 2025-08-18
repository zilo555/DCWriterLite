using System;
using System.Collections.Generic;
using System.Text;
using DCSoft.Common;
using DCSoft.Writer.Controls;
using System.ComponentModel;
using DCSoft.Writer.Data;
using DCSoft.Data;
using DCSoft.Drawing;
using DCSoft.Writer.Serialization;
//using System.Drawing ;
using System.Reflection;

namespace DCSoft.Writer.Dom
{
    /// <summary>
    /// 基础的纯文本数据输入域,DCWriter内部使用。
    /// </summary>
#if !RELEASE
    [System.Diagnostics.DebuggerDisplay("Base Input Name:{Name}")]
#endif
    public abstract partial class DomInputFieldElementBase : DomFieldElementBase
    {
        /// <summary>
        /// 初始化对象
        /// </summary>
        protected DomInputFieldElementBase()
        {
            this.TabStop = true;
            this.UserEditable = true;
        }


        public override string FormulaValue
        {
            get
            {
                if (string.IsNullOrEmpty(this.InnerValue))
                {
                    return this.Text;
                }
                else
                {
                    return this.InnerValue;
                }
            }
            set
            {
                base.FormulaValue = ValueToText(value, false);
            }
        }



        /// <summary>
        /// 获取或设置一个值，该值指示用户能否使用 Tab 键将焦点放到该元素中上。 
        /// </summary>
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public bool TabStop
        {
            get
            {
                return this.StateFlag15;// _TabStop;
            }
            set
            {
                this.StateFlag15 = value;// _TabStop = value;
            }
        }


        /// <summary>
        /// 获取或设置一个运行时的值，该值指示用户能否使用 Tab 键将焦点放到该元素中上。 
        /// </summary>
        public override bool RuntimeTabStop()
        {

            return this.TabStop;

        }

        private MoveFocusHotKeys _MoveFocusHotKey = MoveFocusHotKeys.Default;
        /// <summary>
        /// 移动焦点使用的快捷键
        /// </summary>
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public MoveFocusHotKeys MoveFocusHotKey
        {
            get
            {
                return _MoveFocusHotKey;
            }
            set
            {
                _MoveFocusHotKey = value;//(MoveFocusHotKeys)WriterUtilsInner.FixEnumValue(value);
            }
        }

        /// <summary>
        /// 实际使用的移动焦点所使用的快捷键样式
        /// </summary>
        public override MoveFocusHotKeys RuntimeMoveFocusHotKey
        {
            get
            {
                MoveFocusHotKeys result = this.MoveFocusHotKey;
                if (result == MoveFocusHotKeys.Default)
                {
                    if (this.OwnerDocument != null)
                    {
                        result = GetDocumentBehaviorOptions().MoveFocusHotKey;
                    }
                }
                if (result == MoveFocusHotKeys.Default)
                {
                    result = MoveFocusHotKeys.None;
                }
                return result;
            }
        }

        /// <summary>
        /// 用户可以直接修改文本域中的内容
        /// </summary>
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public bool UserEditable
        {
            get
            {
                return this.StateFlag16;// _UserEditable; 
            }
            set
            {
                this.StateFlag16 = value;// _UserEditable = value; 
            }
        }

        public bool InnerRuntimeUserEditable()
        {
            return this.UserEditable;
        }

        private string _Name;
        /// <summary>
        /// 对象名称
        /// </summary>
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public string Name
        {
            get
            {
                return _Name;
            }
            set
            {
                _Name = value;
            }
        }

        public override void WriteText(WriteTextArgs args)
        {
            if (args.IncludeOuterText == false)
            {
                args.IncludeOuterText = true;
                if (this.HasElements())
                {
                    base.WriteText(args);
                }
                else if (args.IncludeBackgroundText && this._BackgroundText != null)
                {
                    args.Append(this._BackgroundText);
                }
            }
            else
            {
                if (this.HasElements())
                {
                    base.WriteText(args);
                }
                else if (args.IncludeBackgroundText && this._BackgroundText != null)
                {
                    args.Append(this._BackgroundText);
                }
            }
        }

        /// <summary>
        /// 显示的名称
        /// </summary>
        //////[Browsable( false )]
        //[System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public string DisplayName
        {
            get
            {
                if (string.IsNullOrEmpty(this.ID) == false)
                {
                    return this.ID;
                }
                else
                {
                    return this.Name;
                }
            }
        }

        private ValueFormater _DisplayFormat;
        /// <summary>
        /// 显示的格式化对象
        /// </summary>
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
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

        public ValueFormater RuntimeDisplayFormat
        {
            get
            {
                if (this._DisplayFormat != null
                    && this._DisplayFormat.IsEmpty == false)
                {
                    return this._DisplayFormat;
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// 获得下一个可以获得焦点的输入域对象
        /// </summary>
        /// <returns></returns>
        public DomInputFieldElementBase GetNextFocusFieldElement()
        {
            DomInputFieldElementBase field = this;
            while (true)
            {
                field = (DomInputFieldElementBase)
                               this.OwnerDocument.GetNextElement(
                                   field,
                                   typeof(DomInputFieldElementBase),
                                   false);
                if (field == null)
                {
                    break;
                }
                else
                {
                    if (field.TabStop)
                    {
                        break;
                    }
                }
            }
            return field;
        }


        /// <summary>
        /// 内部的文档加载后的处理
        /// </summary>
        /// <param name="args">事件参数</param>
        public override void AfterLoad(ElementLoadEventArgs args)
        {
            base.AfterLoad(args);
            var rf = this._DisplayFormat;
            if (rf != null && rf.IsEmpty == false)
            {
                if (this._Elements != null
                    && this._Elements.Count > 0
                    && rf.Style == ValueFormatStyle.DateTime)
                {
                    // 快速判断文本格式是否符合预期，减少后期处理
                    var f4 = rf.Format;
                    if (f4 != null && f4.Length == this._Elements.Count)
                    {
                        var bolMatch5 = true;
                        for (var len4 = f4.Length - 1; len4 >= 0; len4--)
                        {
                            var e1 = this._Elements[len4] as DomCharElement;
                            if (e1 == null || e1._RuntimeVisible == false)
                            {
                                bolMatch5 = false;
                                break;
                            }
                            var c1 = e1.CharValue;
                            var c2 = f4[len4];
                            if (c1 != c2)
                            {
                                if (c2 == 'y' || c2 == 'Y' || c2 == 'h' || c2 == 'H' || c2 == 'm'
                                    || c2 == 'M' || c2 == 'd' || c2 == 'D' || c2 == 's' || c2 == 'S')
                                {
                                    if (c1 < '0' || c1 > '9')
                                    {
                                        bolMatch5 = false;
                                        break;
                                    }
                                }
                                else
                                {
                                    bolMatch5 = false;
                                    break;
                                }
                            }
                        }
                        if (bolMatch5)
                        {
                            // 认为是匹配的，无需后续处理
                            return;
                        }
                    }
                }

                string curText = this.Text;

                string txt = this.GetDisplayText(curText);
                if (string.IsNullOrEmpty(txt) && string.IsNullOrEmpty(curText))
                {
                    // 都是空白字符，不处理。
                    return;
                }
                if (txt != curText)
                {
                    // 更新显示的文本格式,但InnerValue值不变。
                    var back = this._InnerValue;

                    base.XTextContainerElement_SetInnerTextFastExt(txt, null, true);

                    //this.SetInnerTextFastExt(txt, null, true);


                    //this.SetInnerTextFast(txt);
                    this._InnerValue = back;
                }
            }
        }

        /// <summary>
        /// 将前台文本转换为后台数值
        /// </summary>
        /// <param name="text">文本</param>
        /// <returns>数值</returns>
        public virtual string TextToValue(string text)
        {
            return text;
        }

        /// <summary>
        /// 将后台数据转换为前台文本
        /// </summary>
        /// <param name="Value">原始文本</param>
        /// <param name="structMode">严格模式</param>
        /// <returns>要显示的文本</returns>

        public virtual string ValueToText(string Value, bool structMode)
        {
            return Value;
        }

        /// <summary>
        /// 将数值转换为文本
        /// </summary>
        /// <param name="Value">数值</param>
        /// <returns>转换后的文本</returns>
        public virtual string GetDisplayText(string Value)
        {
            var format = this.RuntimeDisplayFormat;
            if (format != null && format.IsEmpty == false)
            {
                Value = format.Execute(Value);
            }
            return Value;
        }
        private string _InnerValue;
        /// <summary>
        /// 内置的数值
        /// </summary>
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public string InnerValue
        {
            get
            {
                return this._InnerValue;
            }
            set
            {
                this._InnerValue = value;
            }
        }

        private DCBooleanValue _PrintBackgroundText = DCBooleanValue.Inherit;
        /// <summary>
        /// 打印背景文字
        /// </summary>
        public DCBooleanValue PrintBackgroundText
        {
            get
            {
                return this._PrintBackgroundText;
            }
            set
            {
                this._PrintBackgroundText = value;
            }
        }
        /// <summary>
        /// 运行时的是否打印背景文字
        /// </summary>
        public override bool RuntimePrintBackgroundText()
        {

            if (this.PrintBackgroundText == DCBooleanValue.False)
            {
                return false;
            }
            if (this.PrintBackgroundText == DCBooleanValue.True)
            {
                return true;
            }
            if (this.OwnerDocument != null)
            {
                return GetDocumentViewOptions().PrintBackgroundText;
            }
            return true;

        }

        private string _BackgroundText;
        /// <summary>
        /// 背景文本
        /// </summary>
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public string BackgroundText
        {
            get
            {
                return this._BackgroundText;
            }
            set
            {
                this._BackgroundText = value;
                this._InnerBackgroundTextElements = null;
            }
        }

        /// <summary>
        /// 运行时使用的背景文字
        /// </summary>
        public override string RuntimeBackgroundText
        {
            get
            {
                return this._BackgroundText;
            }
        }

        /// <summary>
        /// 快速设置输入域的文本内容
        /// </summary>
        /// <param name="txt">新的文本内容</param>
        /// <param name="newTextStyle">新文本使用的样式</param>
        /// <returns>操作生成的文档元素列表</returns>
        public override DomElementList SetInnerTextFastExt(string txt, DocumentContentStyle newTextStyle)
        {
            //this.InnerBackgroundTextElements = null;
            string txt2 = GetDisplayText(txt);
            DomElementList list = base.SetInnerTextFastExt(txt2, newTextStyle);
            //this.InnerValue = txt2;
            //this.CheckBackgroundTextElements();
            return list;
        }

        /// <summary>
        /// 刷新视图
        /// </summary>
        public override void EditorRefreshView()
        {
            if (this._Elements != null)
            {
                this._Elements.IsElementsSplited = false;
                WriterUtilsInner.SplitElements(this._Elements, false, this._OwnerDocument, this, true);
            }
            var hm = this.OwnerDocument.HighlightManager;
            if (this._InnerBackgroundTextElements != null && hm != null)
            {
                foreach (DomElement element in this._InnerBackgroundTextElements)
                {
                    hm.Remove(element);
                }
            }
            this.InnerBackgroundTextElements = null;
            string txt = this.Text;
            string txt2 = this.GetDisplayText(txt);
            if (txt != txt2)
            {
                this.SetInnerTextFast(txt2);
                this.InnerValue = txt2;
            }
            base.EditorRefreshView();
        }

        [NonSerialized]
        private int _LastAppendViewContentElement_InputField_ContentVersion = -1;

        /// <summary>
        /// 添加内容元素
        /// </summary>
        /// <param name="args">参数</param>
        public override int AppendViewContentElement(AppendViewContentElementArgs args)
        {
            if (this._OwnerDocument == null)
            {
                return 0;
            }
            args.HasFieldElements = true;
            if (args.PrivateMode == false
                && this._LastAppendViewContentElement_InputField_ContentVersion == this._ContentVersion
                && this._OwnerDocument.CheckDOMEffectBy_ReplaceElements_CurrentContainer(this))
            {
                // 正在执行XTextDocument.ReplaceElements()，则有条件的进行快速添加
                var argsContent = args.Content;
                var thisElements = this.Elements;
                int result2 = argsContent.Count;
                if (thisElements.Count > 0)
                {
                    argsContent.EnsureCapacity(argsContent.Count + thisElements.Count + 2);
                    argsContent.SuperFastAdd(this._StartElement);
                    this.HasContentElement = true;
                    argsContent.AddRangeByDCList(thisElements);
                    argsContent.SuperFastAdd(this._EndElement);
                }
                else
                {
                    argsContent.EnsureCapacity(
                        argsContent.Count + 2 +
                        (this._InnerBackgroundTextElements == null ? 0 : this._InnerBackgroundTextElements.Length));
                    argsContent.SuperFastAdd(this._StartElement);
                    this.HasContentElement = false;
                    if (this._InnerBackgroundTextElements != null)
                    {
                        argsContent.AddArray(this._InnerBackgroundTextElements);
                    }
                    argsContent.SuperFastAdd(this._EndElement);
                }
                if (args.PrivateMode == false)
                {
                    if (result2 == argsContent.Count)
                    {
                        this.SetContentIndex(-1);
                    }
                    else
                    {
                        this.SetContentIndex(result2);
                    }
                }
                return argsContent.Count - result2;
            }
            AppendViewContentElementArgs argsBack = null;
            this.HasContentElement = false;
            var content = args.Content;

            var privateMode = args.PrivateMode;
            DomDocument document = args.Document;
            bool printMode = args.PrintingViewMode;
            bool showBackgroundText = true;
            if (printMode)
            {
                showBackgroundText = false;
                if (args.RuntimePrintBackgroundText(this._PrintBackgroundText)
                    || args.PreserveBackgroundTextWhenPrint)
                {
                    showBackgroundText = true;
                    printMode = false;
                }
            }
            int result = content.Count;
            if (this._StartElement == null)
            {
                this.CheckStartEndElement();
            }
            var se = this._StartElement;
            var ee = this._EndElement;
            content.FastAdd2(se);
            if (printMode)
            {
                //  若处于打印模式，则只添加可见元素
                base.XTextContainerElement_AppendViewContentElement(args);
            }
            else
            {
                // 判断是否存在可见元素
                bool hasVisibleElement = false;
                if (this._Elements != null && this._Elements.Count > 0)
                {
                    hasVisibleElement = this._Elements.GetItemFast(0)._RuntimeVisible;
                    if (hasVisibleElement == false)
                    {
                        // 首先判断第一个元素的可见性，大多数情况下是可见的，则可以避免一次循环判断。
                        foreach (DomElement element in this.Elements.FastForEach())
                        {
                            if (element._RuntimeVisible)
                            {
                                hasVisibleElement = true;
                                break;
                            }
                        }
                    }
                }
                if (hasVisibleElement)
                {
                    this.HasContentElement = true;
                    // 添加可见的元素
                    //  若处于打印模式，则只添加可见元素
                    {
                        int oldLen = content.Count;
                        base.XTextContainerElement_AppendViewContentElement(args);


                    }
                }
                else
                {
                    if (showBackgroundText)
                    {
                        if (this._InnerBackgroundTextElements == null)
                        {
                            CheckBackgroundTextElements();
                        }
                        var es = this._InnerBackgroundTextElements;
                        // 添加背景文字元素
                        if (es != null && es.Length > 0)
                        {
                            content.AddArray(es);
                            this.HasContentElement = true;
                        }
                    }
                }
            }
            content.FastAdd2(ee);

            if (args.PrivateMode == false)
            {
                if (content.Count == result)
                {
                    this.SetContentIndex(-1);
                }
                else
                {
                    this.SetContentIndex(result);
                }
            }
            result = content.Count - result;
            if (args.PrivateMode == false
                && this._OwnerDocument.CheckDOMEffectBy_ReplaceElements_CurrentContainer(this))
            {
                this._LastAppendViewContentElement_InputField_ContentVersion = -1;
                if (content.GetItemFast(content.Count - result) == this._StartElement
                    && content.LastElement == this._EndElement)
                {
                    var list = this._Elements.Count > 0 || (this._InnerBackgroundTextElements == null)
                        ? (System.Collections.Generic.IList<DomElement>)this._Elements
                        : (System.Collections.Generic.IList<DomElement>)this._InnerBackgroundTextElements;
                    if (list != null && result == list.Count + 2)
                    {
                        this._LastAppendViewContentElement_InputField_ContentVersion
                            = this._ContentVersion;
                        if (list.Count > 0)
                        {
                            var indexFix = content.Count - list.Count - 1;
                            for (var iCount = list.Count - 1; iCount >= 0; iCount--)
                            {
                                if (content.GetItemFast(iCount + indexFix) != list[iCount])
                                {
                                    this._LastAppendViewContentElement_InputField_ContentVersion = -1;
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            if (argsBack != null)
            {
                argsBack.HasFieldElements = args.HasFieldElements;
                argsBack.HasObjectElement = args.HasObjectElement;
            }
            return result;
        }


        /// <summary>
        /// DCWriter内部使用。内置的内容发生改变前的处理委托对象，而且该委托对象执行一遍后就删除掉。
        /// </summary>
        public static EventHandler EventBeforeOnContentChangedOnce;

        public override void RaiseBubbleOnContentChanged(ContentChangedEventArgs args)
        {
            // 冒泡触发内容修改事件，本函数基本上是用户界面操作而触发的
            // 此时本元素是事件发源地，因此暂时停止文本格式化输出。
            var back = this._DisplayFormat;
            try
            {
                this._DisplayFormat = null;
                base.RaiseBubbleOnContentChanged(args);
            }
            finally
            {
                this._DisplayFormat = back;
            }
        }

        /// <summary>
        /// 处理字段内容发生改变事件
        /// </summary>
        /// <param name="args">参数</param>
        public override void OnContentChanged(ContentChangedEventArgs args)
        {
            if (this.OwnerDocument.EnableContentChangedEvent())
            {
                if (EventBeforeOnContentChangedOnce != null)
                {
                    EventHandler handler = EventBeforeOnContentChangedOnce;
                    EventBeforeOnContentChangedOnce = null;
                    handler(this, args);
                }
            }
            if (this.OwnerDocument.HighlightManager != null)
            {
                this.OwnerDocument.HighlightManager.InvalidateHighlightInfo(this);
            }
            base.OnContentChanged(args);
            //UpdateToolTip();
        }
        /// <summary>
        /// 处理获得输入焦点事件
        /// </summary>
        /// <param name="args">事件参数</param>
        public override void OnGotFocus(ElementEventArgs args)
        {
            if (GetDocumentViewOptions().FieldFocusedBackColor.A != 0)
            {
                // 在失去输入焦点时进行数据校验
                if (this.OwnerDocument.HighlightManager != null)
                {
                    this.OwnerDocument.HighlightManager.InvalidateHighlightInfo(this);
                    this.InvalidateView();
                }
            }
            base.OnGotFocus(args);
        }

        /// <summary>
        /// 处理失去输入焦点事件
        /// </summary>
        /// <param name="args">事件参数</param>
        public override void OnLostFocus(ElementEventArgs args)
        {
            if (GetDocumentViewOptions().FieldFocusedBackColor.A != 0)
            {
                if (this.OwnerDocument.HighlightManager != null)
                {
                    this.OwnerDocument.HighlightManager.InvalidateHighlightInfo(this);
                    this.InvalidateView();
                }
            }
            base.OnLostFocus(args);

        }

        /// <summary>
        /// 处理鼠标进入事件
        /// </summary>
        /// <param name="args">事件参数</param>
        public override void OnMouseEnter(ElementEventArgs args)
        {
            if (GetDocumentViewOptions().FieldHoverBackColor.A != 0)
            {
                if (this.OwnerDocument.HighlightManager != null)
                {
                    this.OwnerDocument.HighlightManager.InvalidateHighlightInfo(this);
                }
                this.InvalidateView();
            }
            base.OnMouseEnter(args);
        }

        /// <summary>
        /// 处理鼠标离开事件
        /// </summary>
        /// <param name="args">事件参数</param>
        public override void OnMouseLeave(ElementEventArgs args)
        {
            if (GetDocumentViewOptions().FieldHoverBackColor.A != 0)
            {
                if (this.OwnerDocument.HighlightManager != null)
                {
                    this.OwnerDocument.HighlightManager.InvalidateHighlightInfo(this);
                    this.InvalidateView();
                }
            }
            base.OnMouseLeave(args);
        }
        private DCVisibleState _BorderVisible = DCVisibleState.Default;
        /// <summary>
        /// 边框元素是否可见
        /// </summary>
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public DCVisibleState BorderVisible
        {
            get
            {
                return _BorderVisible;
            }
            set
            {
                _BorderVisible = value;
            }
        }

        private EnableState _EnableHighlight = EnableState.Enabled;
        /// <summary>
        /// 是否允许高亮度显示状态
        /// </summary>
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public virtual EnableState EnableHighlight
        {
            get
            {
                return _EnableHighlight;
            }
            set
            {
                _EnableHighlight = value;//( EnableState)WriterUtilsInner.FixEnumValue( value );
            }
        }

        /// <summary>
        /// 运行时的是否允许高亮度显示模式
        /// </summary>
        public EnableState RuntimeEnableHighlight
        {
            get
            {
                EnableState state = this._EnableHighlight;
                if (state == EnableState.Enabled)
                {
                    return EnableState.Enabled;
                }
                if (state == EnableState.Default)
                {
                    if (this._OwnerDocument != null)
                    {
                        state = GetDocumentViewOptions().DefaultInputFieldHighlight;
                    }
                }
                if (state == EnableState.Default)
                {
                    state = EnableState.Enabled;
                }
                return state;
            }
            set
            {

            }
        }

        /// <summary>
        /// 获得高亮度显示区域列表
        /// </summary>
        /// <returns></returns>
        public override HighlightInfoList GetHighlightInfos()
        {
            EnableState state = this.RuntimeEnableHighlight;
            if (state == EnableState.Disabled)
            {
                return null;
            }
            DomDocument document = this._OwnerDocument;
            if (document == null)
            {
                return null;
            }
            var se = this._StartElement;
            if (se == null)
            {
                this.CheckStartEndElement();
                se = this._StartElement;
            }
            var ee = this._EndElement;
            if (se == null || ee == null)
            {
                return null;
            }
            if (se._ContentIndex < 0 || ee._ContentIndex < 0)
            {
                return null;
            }
            DomDocumentContentElement dce = this.DocumentContentElement;
            if (dce == null)
            {
                return null;
            }
            var content = dce.Content;
            if (content.SafeGet(se._ContentIndex) != se
                || content.SafeGet(ee._ContentIndex) != ee)
            {
                return null;
            }
            var bolHeightClass = false;
            DocumentViewOptions viewOptions = document.GetDocumentViewOptions();
            var bc = Color.Transparent;
            if (this.LastValidateResult != null)
            {
                bc = viewOptions.FieldInvalidateValueBackColor;
                bolHeightClass = true;
            }
            if (bc.A == 0 && state == EnableState.Enabled)
            {
                if (this.Focused
                    && viewOptions.FieldFocusedBackColor.A != 0)
                {
                    bc = viewOptions.FieldFocusedBackColor;
                }
                else if (document.IsHover(this)
                    && viewOptions.FieldHoverBackColor.A != 0)
                {
                    bc = viewOptions.FieldHoverBackColor;
                }
                else
                {
                    Color c3 = viewOptions.FieldBackColor;
                    //bool find = false;
                    if (c3.A != 0)
                    {
                        if (state == EnableState.Disabled)
                        {
                            return null;
                        }
                        else if (state == EnableState.Default)
                        {
                            DomElement parent = this.Parent;
                            while (parent != null)
                            {
                                if (parent is DomInputFieldElementBase)
                                {
                                    return null;// 使用父文本输入域的高亮度显示区域
                                    //break;
                                }
                                parent = parent.Parent;
                            }
                        }
                        bc = c3;
                    }
                }
            }
            var fc = Color.Empty;
            if (this.LastValidateResult != null)
            {
                bolHeightClass = true;
                fc = viewOptions.FieldInvalidateValueForeColor;
            }
            if (bc.A != 0 || fc.A != 0)
            {
                var info = new HighlightInfo(
                    this,
                    new DCRange(
                        dce,
                        se,
                        ee),
                   bc,
                   fc,
                   HighlightActiveStyle.Static);
                info.HeightClass = bolHeightClass;
                //info.ActiveStyle = HighlightActiveStyle.Static;
                var list = new HighlightInfoList(1);
                list.Add(info);
                return list;

            }
            return null;
        }



        /// <summary>
        /// 设置文本内容
        /// </summary>
        /// <param name="args">参数</param>
        /// <returns>是否修改内容</returns>
        public override bool SetEditorText(SetContainerTextArgs args)
        {
            // 数据格式化处理
            if (args.IgnoreDisplayFormat == false)
            {
                args.NewText = GetDisplayText(args.NewText);
            }
            bool result = base.SetEditorText(args);
            if (result)
            {
                // 清除数据校验结果
                if (ClearValidateResult(false))
                {
                    if (this.OwnerDocument != null && this.OwnerDocument.EditorControl != null)
                    {
                        this.OwnerDocument.EditorControl.UpdateToolTip(true);
                    }
                }
                if (this.OwnerDocument != null)
                {
                    // 执行数据校验
                    if (GetDocumentEditOptions().ValueValidateMode == DocumentValueValidateMode.LostFocus
                        || GetDocumentEditOptions().ValueValidateMode == DocumentValueValidateMode.Dynamic)
                    {
                        if (this.ValidateStyle != null)// this.ValidateStyle != null && this.RuntimeSupportValidateStyle())
                        {
                            this.ValidateStyle.ContentVersion = this.ContentVersion - 1;
                            this.Validating(false);
                        }
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 清除数据校验结果
        /// </summary>
        /// <param name="fastMode">快速操作模式</param>
        /// <returns>操作是否改变了输入域状态</returns>
        public override bool ClearValidateResult(bool fastMode)
        {
            if (base.ClearValidateResult(fastMode))
            {
                if (fastMode == false)
                {
                    //UpdateToolTip();
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// 复制对象
        /// </summary>
        /// <param name="Deeply">是否深度复制</param>
        /// <returns>复制品</returns>
        public override DomElement Clone(bool Deeply)
        {
            DomInputFieldElementBase field = (DomInputFieldElementBase)base.Clone(Deeply);

            if (this._DisplayFormat != null)
            {
                field._DisplayFormat = this._DisplayFormat.Clone();
            }
            field._InnerBackgroundTextElements = null;
            return field;
        }
#if !RELEASE
        /// <summary>
        /// 返回调试信息文本
        /// </summary>
        /// <returns>返回的文本</returns>
        public override string ToDebugString()
        {
            if (string.IsNullOrEmpty(this.ID) == false)
            {
                return "Field[" + this.ID + "]";
            }
            if (string.IsNullOrEmpty(this.Name) == false)
            {
                return "Field[" + this.Name + "]";
            }
            return "Field";
        }
#endif
        public override void Dispose()
        {
            base.Dispose();
            this._BackgroundText = null;
            this._DisplayFormat = null;
            this._InnerValue = null;
            this._Name = null;
        }
    }
}