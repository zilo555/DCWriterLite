using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using DCSoft.Drawing;
using DCSoft.Common;
using DCSoft.Writer.Data;
using DCSoft.Writer.Serialization;
using DCSoft.Writer.Controls;
using System.Runtime.InteropServices;
using DCSoft.WinForms;

namespace DCSoft.Writer.Dom
{
    /// <summary>
    /// 复选框控件基础对象类型
    /// </summary>
    /// <remarks>编制 袁永福</remarks>
#if !RELEASE
    [System.Diagnostics.DebuggerDisplay("CheckBox:Group={GroupName} , Checked={Checked}")]
#endif
    public abstract partial class DomCheckBoxElementBase : DomObjectElement
    {
        /// <summary>
        /// 初始化对象
        /// </summary>
        protected DomCheckBoxElementBase()
        {
        }
        /// <summary>
        /// 必填项。必须设置元素的Name属性，本设置才有效。
        /// </summary>
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public bool Requried
        {
            get
            {
                return this.StateFlag21;// _Requried;
            }
            set
            {
                this.StateFlag21 = value;// _Requried = value;
            }
        }

        /// <summary>
        /// 运行时使用的文档行垂直对齐方式
        /// </summary>
        public override VerticalAlignStyle RuntimeVAlign()
        {

            return VerticalAlignStyle.Bottom;

        }

        /// <summary>
        /// 元素内容是否改变
        /// </summary>
        public override bool Modified
        {
            get
            {
                return this.StateFlag12;// this._Modified;
            }
            set
            {
                this.StateFlag12 = value;// this._Modified = value;
            }
        }

        private int _LayoutVersion;
        private string _Caption;
        /// <summary>
        /// 复选框后面跟着的文本
        /// </summary>
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public string Caption
        {
            get
            {
                return _Caption;
            }
            set
            {
                _Caption = value;
                this._LayoutVersion++;
                this.InvalidateDocumentLayoutFast();
            }
        }

        public override DomElement FirstContentElementInPublicContent
        {
            get
            {
                return this;
            }
        }
        public override DomElement FirstContentElement
        {
            get
            {
                return this;
            }
        }


        private StringAlignment _CaptionAlign = StringAlignment.Center;
        /// <summary>
        /// 标题文本对齐方式
        /// </summary>
        public StringAlignment CaptionAlign
        {
            get
            {
                return _CaptionAlign;
            }
            set
            {
                _CaptionAlign = value;
            }
        }

        private string FixPrintText(string txt)
        {
            if (txt == null || string.IsNullOrEmpty(txt))
            {
                return txt;
            }
            if (txt.Length > 2 && txt[0] == '\"' && txt[txt.Length - 1] == '\"')
            {
                return txt.Substring(1, txt.Length - 2);
            }
            return txt;
        }


        private static Bitmap StaticDisplayImage(DomCheckBoxElementBase element)
        {
            if (element is DomCheckBoxElement)
            {
                if (element.Checked)
                {
                    return DCStdImageList.BmpCheckBoxChecked;
                }
                else
                {
                    return DCStdImageList.BmpCheckBoxUnchecked;
                }
            }
            else
            {
                if (element.Checked)
                {
                    return DCStdImageList.BmpRadioBoxChecked;
                }
                else
                {
                    return DCStdImageList.BmpRadioBoxUnchecked;
                }
            }
        }

        /// <summary>
        /// 多行模式可以改变大小，单行模式不能改变大小
        /// </summary>
        public override ResizeableType Resizeable
        {
            get
            {
                if (this.Multiline)
                {
                    if (this.AutoHeightForMultiline)
                    {
                        return ResizeableType.Width;
                    }
                    else
                    {
                        return ResizeableType.WidthAndHeight;
                    }
                }
                else
                {
                    return ResizeableType.FixSize;
                }
            }
            set
            {
                base.Resizeable = value;
            }
        }


        /// <summary>
        /// 处于多行模式时是否为自动计算高度
        /// </summary>
        public bool AutoHeightForMultiline
        {
            get
            {
                return this.StateFlag08;// _AutoHeightForMultiline;
            }
            set
            {
                this.StateFlag08 = value;// _AutoHeightForMultiline = value;
            }
        }

        /// <summary>
        /// 显示的名称
        /// </summary>
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


        /// <summary>
        /// 运行时使用的分组名称
        /// </summary>
        public string RuntimeGroupName()
        {
            if (string.IsNullOrEmpty(this.Name) == false)
            {
                return this.Name;
            }
            return this.ID;

        }
        /// <summary>
        /// 对象数值
        /// </summary>
        public override string FormulaValue
        {
            get
            {
                if (this.Checked)
                {
                    return this.CheckedValue;
                }
                else
                {
                    return null;
                }
            }
            set
            {
                bool v = WriterUtilsInner.StringToBoolean(value, this.Checked);
                if (v != this.Checked)
                {
                    this.EditorChecked = v;
                }
            }
        }

        private int _GroupInfoVersion = 0;
        /// <summary>
        /// 分组信息版本号
        /// </summary>
        internal int GroupInfoVersion
        {
            get
            {
                return _GroupInfoVersion;
            }
            set
            {
                _GroupInfoVersion = value;
            }
        }
        public string GetAllValueInDocument(char splitChar = ',')
        {
            if (this.OwnerDocument == null)
            {
                return null;
            }
            DomElementList list = GetElementsInSameGroup();
            if (list.Count == 1)
            {
                if (this.Checked)
                {
                    if (string.IsNullOrEmpty(this.CheckedValue))// == null || this.CheckedValue.Length == 0)
                    {
                        return "true";
                    }
                }
            }
            StringBuilder str = new StringBuilder();
            foreach (DomCheckBoxElementBase chk in list)
            {
                if (chk.Checked)
                {
                    if (chk.CheckedValue != null && chk.CheckedValue.Length > 0)
                    {
                        if (str.Length > 0)
                        {
                            str.Append(splitChar);
                        }
                        str.Append(chk.CheckedValue);
                    }
                    if (chk is DomRadioBoxElement)
                    {
                        break;
                    }
                }
            }
            return str.ToString();
        }

        /// <summary>
        /// 获得文档中所有同组的复选框对象列表
        /// </summary>
        /// <returns></returns>
        public DomElementList GetElementsInSameGroup()
        {
            if (this.OwnerDocument == null)
            {
                return null;
            }
            if (string.IsNullOrEmpty(this.RuntimeGroupName()))
            {
                return new DomElementList(this);
            }
            DomElementList elements = null;
            var p = this.Parent;
            while (p != null)
            {
                if (p is DomDocument)
                {
                    elements = ((DomDocument)p).CheckBoxGroupInfo.GetElementsInSameGroup(this);
                    break;
                }
                p = p.Parent;
            }
            //XTextElementList elements = element.OwnerDocument.CheckBoxGroupInfo().GetElementsInSameGroup(element);
            //    element.OwnerDocument.GetCheckBoxOrRadioBoxElementsSpecifyName(
            //    c , 
            //    element.RuntimeGroupName , 
            //    element.GetType());
            if (elements == null || elements.Count == 0)
            {
                elements = new DomElementList();
                elements.Add(this);
            }
            return elements;
        }

        /// <summary>
        /// 选中状态。本属性只是快速的设置一个状态，不执行表达式和事件VBA脚本，不触发任何事件。
        /// </summary>
        /// <remarks>
        /// 若要触发事件，执行相关的表达式，请调用EditorChecked属性。
        /// </remarks>
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public bool Checked
        {
            get
            {
                return this.StateFlag15;// _Checked;
            }
            set
            {
                if (this.StateFlag15 != value)
                {
                    this.StateFlag15 = value;// _Checked = value;
                    this._ContentVersion++;
                }
            }
        }

        private string _CheckedValue = null;
        /// <summary>
        /// 勾选状态的值
        /// </summary>
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public string CheckedValue
        {
            get
            {
                return _CheckedValue;
            }
            set
            {
                _CheckedValue = value;
            }
        }

        /// <summary>
        /// 对象数值
        /// </summary>
        public string Value
        {
            get
            {
                if (this.Checked)
                {
                    return this.CheckedValue;
                }
                else
                {
                    return null;// this.UnCheckedValue;
                }
            }
            set
            {
                if (value == this.CheckedValue)
                {
                    this.Checked = true;
                }
                else
                {
                    this.Checked = false;
                }
            }
        }

        /// <summary>
        /// 只读的
        /// </summary>
        public bool Readonly
        {
            get
            {
                return this.StateFlag20;// _Readonly;
            }
            set
            {
                this.StateFlag20 = value;// _Readonly = value;
            }
        }

        public override bool MouseClickToSelect
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// 获得提示文本信息
        /// </summary>
        /// <returns></returns>
        public override ElementToolTip GetToolTipInfo()
        {
            ElementToolTip tip = base.GetToolTipInfo();
            if (tip == null && string.IsNullOrEmpty(this.ToolTip) == false)
            {
                tip = new ElementToolTip(this, this.ToolTip);
                tip.ContentType = ToolTipContentType.ElementToolTip;
            }
            return tip;
        }
        /// <summary>
        /// 鼠标光标进入事件
        /// </summary>
        /// <param name="args">事件参数</param>

        public override void OnMouseEnter(ElementEventArgs args)
        {
            if (string.IsNullOrEmpty(this.Name) == false)
            {
                InvalidateHighlightInfo();
            }
            else
            {
                this.OwnerDocument.HighlightManager.Remove(this);
            }
            base.OnMouseEnter(args);
        }

        /// <summary>
        /// 鼠标光标离开事件
        /// </summary>
        /// <param name="args">参数</param>

        public override void OnMouseLeave(ElementEventArgs args)
        {
            if (string.IsNullOrEmpty(this.Name) == false)
            {
                InvalidateHighlightInfo();
            }
            else
            {
                if (this.OwnerDocument.HighlightManager != null)
                {
                    this.OwnerDocument.HighlightManager.Remove(this);
                }
            }
            base.OnMouseLeave(args);
        }

        /// <summary>
        /// 获得焦点事件
        /// </summary>
        /// <param name="args">参数</param>
        public override void OnGotFocus(ElementEventArgs args)
        {
            InvalidateHighlightInfo();
            base.OnGotFocus(args);
        }

        /// <summary>
        /// 失去焦点事件
        /// </summary>
        /// <param name="args">参数</param>
        public override void OnLostFocus(ElementEventArgs args)
        {
            InvalidateHighlightInfo();
            base.OnLostFocus(args);
        }

        /// <summary>
        /// 声明同组的所有的复选框元素高亮度显示信息无效
        /// </summary>
        public override void InvalidateHighlightInfo()
        {
            var document = this.OwnerDocument;
            if (string.IsNullOrEmpty(this.Name) == false && document.HighlightManager != null)
            {
                if (this.GetDocumentViewOptions().FieldFocusedBackColor.A != 0)
                {
                    DomElementList chks = GetElementsInSameGroup();
                    foreach (DomCheckBoxElementBase element2 in chks)
                    {
                        document.HighlightManager?.InvalidateHighlightInfo(element2);
                        element2.InvalidateView();
                    }
                }
            }

        }
        /// <summary>
        /// 编辑器中设置或获得选择状态，就完全模拟用户鼠标点击设置勾选状态。触发表达式和事件。
        /// </summary>
        public void SetEditorChecked(bool value)
        {
            if (this.Checked == value)
            {
                return;
            }
            var doc = this.OwnerDocument;
            var ctl = doc.DocumentControler;
            if (ctl == null || ctl.CanModify(this))
            {
                DomElementList chks = new DomElementList();
                if (value == true && this is DomRadioBoxElement)
                {
                    chks = GetElementsInSameGroup();// element.GetElementsInSameGroup();
                    for (int iCount = chks.Count - 1; iCount >= 0; iCount--)
                    {
                        DomCheckBoxElementBase chk = (DomCheckBoxElementBase)chks[iCount];
                        if (ctl == null || ctl.CanModify(chk) == false)
                        {
                            // 元素只读，不能修改。
                            chks.RemoveAt(iCount);
                        }
                        else if (chk != this && chk.Checked == false)
                        {
                            // 不处理不处于勾选状态的单选框元素
                            chks.RemoveAt(iCount);
                        }
                    }
                    if (chks.Contains(this))
                    {
                        // 设置本元素在列表中为最后一个元素
                        chks.Remove(this);
                        chks.Add(this);
                    }
                }
                else
                {
                    chks.Add(this);
                }

                // 触发元素值改变前事件

                ContentChangingEventArgs args2 = new ContentChangingEventArgs();
                args2.Document = doc;
                args2.Tag = this;
                args2.Element = this;
                foreach (DomCheckBoxElementBase chk in chks)
                {
                    chk.OnContentChanging(this, args2);
                    if (args2.Cancel)
                    {
                        // 取消操作
                        return;
                    }
                }
                if (doc.BeginLogUndo())
                {
                    foreach (DomCheckBoxElementBase chk in chks)
                    {
                        doc.UndoList.AddProperty("InnerEditorChecked",
                            chk.Checked,
                            !chk.Checked,
                            chk);
                        chk.Checked = !chk.Checked;
                    }
                    doc.EndLogUndo();
                }
                else
                {
                    foreach (DomCheckBoxElementBase chk in chks)
                    {
                        chk.Checked = !chk.Checked;
                    }
                }
                var ctlView = this.InnerViewControl;
                foreach (DomCheckBoxElementBase chk in chks)
                {
                    ContentChangedEventArgs args3 = new ContentChangedEventArgs();
                    args3.Document = doc;
                    args3.Element = this;
                    args3.Tag = this;
                    if (ctlView != null)
                    {
                        ctlView.MoveCaretWithScroll = false;
                    }
                    try
                    {
                        chk.OnContentChanged(chk, args3);
                    }
                    finally
                    {
                        if (ctlView != null)
                        {
                            ctlView.MoveCaretWithScroll = false;
                        }
                    }
                    chk.InvalidateView();
                }
                doc.Modified = true;
                doc.OnDocumentContentChanged();
                if (this.WriterControl != null)
                {
                    this.WriterControl.OnEventElementCheckedChanged(
                        this.ID,
                        this.Name,
                        this.Checked ? this.CheckedValue : string.Empty,
                        this);
                }
            }
        }

        /// <summary>
        /// DCWriter内部使用。编辑器中设置或获得选择状态，该属性内部使用，而且不会记录撤销操作信息
        /// </summary>
        public void SetInnerEditorChecked(bool value)
        {

            if (this.Checked != value)
            {
                //if (element.OwnerDocument.DocumentControler.CanModify(element))
                {
                    ContentChangingEventArgs args2 = new ContentChangingEventArgs();
                    args2.Document = this.OwnerDocument;
                    args2.Tag = this;
                    this.OnContentChanging(this, args2);
                    if (args2.Cancel)
                    {
                        return;
                    }
                    this.Checked = value;
                    ContentChangedEventArgs args3 = new ContentChangedEventArgs();
                    args3.Document = this.OwnerDocument;
                    args3.Tag = this;
                    this.OnContentChanged(this, args3);
                    this.InvalidateView();
                }
            }
        }

        /// <summary>
        /// 编辑器中设置或获得选择状态，就完全模拟用户鼠标点击设置勾选状态。触发表达式和事件。
        /// </summary>
        public virtual bool EditorChecked
        {
            get
            {
                return this.Checked;
            }
            set
            {
                SetEditorChecked(value);
            }
        }

        /// <summary>
        /// DCWriter内部使用。编辑器中设置或获得选择状态，该属性内部使用，而且不会记录撤销操作信息
        /// </summary>
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public bool InnerEditorChecked
        {
            set
            {
                SetInnerEditorChecked(value);
            }
        }

        /// <summary>
        /// 对象是否获取焦点
        /// </summary>
        public override bool Focused
        {
            get
            {
                if (this._Parent == null)
                {
                    return false;
                }
                DomDocumentContentElement dce = this.DocumentContentElement;
                if (dce == null)
                {
                    return false;
                }
                return dce.CurrentElement == this;
            }
        }
        /// <summary>
        /// 获得高亮度显示区域
        /// </summary>
        /// <returns></returns>
        public override HighlightInfoList GetHighlightInfos()
        {
            if (this.Parent == null)
            {
                // 元素不处于文档DOM结构中，返回空。 
                return null;
            }
            DomElementList list = GetElementsInSameGroup();
            if (list == null || list.Count <= 1)
            {
                return null;
            }

            var c = DCSystem_Drawing.Color.Transparent;
            var vp = this.GetDocumentViewOptions();
            foreach (DomCheckBoxElementBase element in list)
            {
                if (element.Focused)
                {
                    c = vp.FieldFocusedBackColor;
                    break;
                }
            }
            if (c.A == 0)
            {
                var chk2 = this.OwnerDocument.HoverElement;
                if (chk2 != null && list.Contains(chk2))
                {
                    c = vp.FieldHoverBackColor;
                }
            }
            if (c.A != 0)
            {
                HighlightInfoList result = new HighlightInfoList();
                foreach (DomCheckBoxElementBase element2 in list)
                {
                    var dce2 = element2.DocumentContentElement;
                    if (dce2 != null)
                    {
                        DCRange rng = null;
                        {
                            rng = new DCRange(dce2, element2.ContentIndex, 1);
                        }
                        HighlightInfo info = new HighlightInfo(
                            element2,
                            rng,
                            c);
                        info.Color = this.RuntimeStyle.Color;
                        result.Add(info);
                    }
                }//foreach
                return result;
            }
            else
            {
                return null;
            }
        }
        #region 事件处理 ************************************************

        /// <summary>
        /// 触发内容正在改变事件
        /// </summary>
        /// <param name="eventSender">参数</param>
        /// <param name="args">参数</param>
        public virtual void OnContentChanging(object eventSender, ContentChangingEventArgs args)
        {

        }


        /// <summary>
        /// 触发内容已经改变事件
        /// </summary>
        /// <param name="eventSender">参数</param>
        /// <param name="args">参数</param>
        public virtual void OnContentChanged(object eventSender, ContentChangedEventArgs args)
        {
            if (args.LoadingDocument == false)
            {
                // 不是在加载文档时触发的事件，设置Modified属性
                this.Modified = true;
            }
            if (this.GetDocumentBehaviorOptions().EnableElementEvents
                && this.OwnerDocument.EnableContentChangedEvent())
            {
                // 允许执行文档元素事件
                // 触发文档内容发生改变后事件
                if (this.Parent != null)
                {
                    this.Parent.RaiseBubbleOnContentChanged(args);
                }
            }
        }

        #endregion
        /// <summary>
        /// 复选框标题文本
        /// </summary>
        public string GetCheckBoxText()
        {
            bool checkStyle = this is DomCheckBoxElement;
            if (checkStyle)
            {
                if (this.Checked)
                {
                    return "■";
                }
                else
                {
                    return "□";
                }
            }
            else
            {
                if (this.Checked)
                {
                    return "●";
                }
                else
                {
                    return "○";
                }
            }

        }

        private DCSystem_Drawing.SizeF GetPrefersize(InnerDocumentPaintEventArgs args, float elementWidth, ref bool isMultiLine)
        {
            DomDocument document = this.OwnerDocument;
            // 自动计算高度
            var size = GraphicsUnitConvert.Convert(
                new DCSystem_Drawing.SizeF(20, 20),
                GraphicsUnit.Pixel,
                DCSystem_Drawing.GraphicsUnit.Document);
            var info = new RuntimeDrawInfo(this, args.RenderMode);
            if (info.ShowCheckBox == false)
            {
                size.Width = 0;
            }
            if (info.Text != null && info.Text.Length > 0)
            {
                RuntimeDocumentContentStyle rs = this.RuntimeStyle;
                var size2 = args.GraphicsMeasureString(
                    info.Text,
                    rs,
                    (int)(elementWidth - size.Width),
                    args.Render.NativeMeasureFormat);
                size.Width = size.Width + size2.Width + 10;
                float lineHeight = args.GraphicsGetFontHeight(rs);
                if (size2.Height > lineHeight)
                {
                    size.Height = size2.Height;
                    isMultiLine = true;
                }
                else
                {
                    size.Height = lineHeight;
                    isMultiLine = false;
                }
            }
            if (info.ShowCheckBox && info.ImageSize > 0 && info.ImageSize > size.Height)
            {
                size.Height = info.ImageSize;
            }
            return size;
        }


        public override void InnerGetText(GetTextArgs args)
        {
            args.Append(this.OuterText);
        }
        public override void WriteText(WriteTextArgs args)
        {
            if (args.IncludeOuterText)
            {
                args.Append(this.OuterText);
            }
            else
            {
                args.Append(this.Caption);
            }
        }

        /// <summary>
        /// 返回表示对象数据的文本
        /// </summary>
        public override string Text
        {
            get
            {
                return this.Caption;
            }
            set
            {
                this.Caption = value;
            }
        }

        public override string OuterText
        {
            get
            {
                return this.CheckBoxText() + this.Caption;
            }
            set
            {
                // 无法设置
            }
        }
        /// <summary>
        /// 复选框标题文本
        /// </summary>
        internal string CheckBoxText()
        {
            return GetCheckBoxText();
        }
        /// <summary>
        /// 返回纯文本
        /// </summary>
        /// <returns></returns>

        public override string ToPlaintString()
        {
            return this.Text;
        }
#if !RELEASE
        /// <summary>
        /// 返回表示对象的字符串
        /// </summary>
        /// <returns>字符串</returns>
        public override string ToString()
        {
            if (DomElement.DebugModeToString)
            {
                return this.OuterText;
            }
            else
            {
                if (this is DomCheckBoxElement)
                {
                    return "CheckBox:" + this.Checked;
                }
                else
                {
                    return "Radio:" + this.Checked;
                }
            }
        }
#endif
        /// <summary>
        /// 复制对象
        /// </summary>
        /// <param name="Deeply">是否深入复制，无效</param>
        /// <returns>复制的对象</returns>

        public override DomElement Clone(bool Deeply)
        {
            DomCheckBoxElementBase chk = (DomCheckBoxElementBase)base.Clone(Deeply);
            chk._GroupInfoVersion = -1;
            return chk;
        }

        /// <summary>
        /// 多行文本
        /// </summary>
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public bool Multiline
        {
            get
            {
                return this.StateFlag19;// _Multiline;
            }
            set
            {
                this.StateFlag19 = value;// _Multiline = value;
            }
        }
        /// <summary>
        /// 计算元素大小
        /// </summary>
        /// <param name="args">参数</param>
        public override void RefreshSize(InnerDocumentPaintEventArgs args)
        {
            if (this.Multiline && this.AutoHeightForMultiline)
            {
                float ew = this.Width;
                if (this.Width <= 0)
                {
                    if (this.ContentElement == null)
                    {
                        ew = this.OwnerDocument.Body.ClientWidth;
                    }
                    else
                    {
                        ew = this.ContentElement.ClientWidth;
                    }
                }

                bool v = false;
                var size = GetPrefersize(args, ew, ref v);
                this.Width = size.Width;
                this.Height = size.Height;
            }
            else if (this.Multiline == false || this.Width <= 0 || this.Height <= 0)
            {
                bool ml = false;
                var size = GetPrefersize(args, 10000, ref ml);
                this.Width = size.Width;
                this.Height = size.Height;
            }
            this.SizeInvalid = false;
        }

        internal class RuntimeDrawInfo
        {
            public RuntimeDrawInfo(DomCheckBoxElementBase element, InnerDocumentRenderMode renderMode)
            {
                this.Text = element.Caption;
                this.CheckBoxImage = StaticDisplayImage(element);
                //lock (this.CheckBoxImage)
                {
                    var imgSize = GraphicsUnitConvert.Convert(
                        new DCSystem_Drawing.SizeF(this.CheckBoxImage.Width, this.CheckBoxImage.Height),
                        GraphicsUnit.Pixel,
                        DCSystem_Drawing.GraphicsUnit.Document);
                    this.ImageSize = imgSize.Width;
                }
            }
            /// <summary>
            /// 复选框图片对象
            /// </summary>
            public readonly Image CheckBoxImage = null;
            /// <summary>
            /// 显示复选框图片
            /// </summary>
            public readonly bool ShowCheckBox = true;
            /// <summary>
            /// 对象是否可见
            /// </summary>
            public readonly bool Visible = true;
            /// <summary>
            /// 图片宽度
            /// </summary>
            public readonly float ImageSize = 0;
            /// <summary>
            /// 文本
            /// </summary>
            public readonly string Text = null;
        }

        public override void DrawContent(InnerDocumentPaintEventArgs args)
        {
            var info = new RuntimeDrawInfo(this, args.RenderMode);
            if (info.Visible == false)
            {
                return;
            }

            DCSystem_Drawing.RectangleF rect = this.GetAbsBounds();
            if (info.ShowCheckBox && info.CheckBoxImage != null)
            {
                // 绘制复选框勾选图片
                float imgLeft = rect.Left + 4;
                DomDocument.IsDrawingSystemImage = true;
                DrawerUtil.DrawImageUnscaledNearestNeighbor(
                    args.Graphics,
                    info.CheckBoxImage,
                    (int)imgLeft,
                    (int)(rect.Top + (rect.Height - info.ImageSize) / 2));
                DomDocument.IsDrawingSystemImage = false;
            }
            if (info.Text != null && info.Text.Length > 0)
            {
                // 绘制文本
                RuntimeDocumentContentStyle rs = this.RuntimeStyle;
                using (var format = args.Render.NativeMeasureFormat.Clone())// .MeasureFormat().Clone();
                {
                    float fh = args.GraphicsGetFontHeight(rs);
                    DCSystem_Drawing.RectangleF txtRect = args.ClientViewBounds;
                    if (info.ImageSize > 0)
                    {
                        txtRect.Width = txtRect.Width - info.ImageSize;
                        txtRect.X = txtRect.X + info.ImageSize;
                    }
                    format.Alignment = this.CaptionAlign;// StringAlignment.Center;
                    format.LineAlignment = StringAlignment.Near;
                    if (this.Multiline)
                    {
                        DrawerUtil.SetMultiLine(format, true);
                        format.LineAlignment = StringAlignment.Center;
                    }
                    else
                    {
                        DrawerUtil.SetMultiLine(format, false);
                        DomLine line = this.OwnerLine;
                        var rva = line.RuntimeVerticalAlign();
                        if (rva == VerticalAlignStyle.Top)
                        {
                        }
                        else if (rva == VerticalAlignStyle.Middle)
                        {
                            txtRect.Y += (txtRect.Height - fh) / 2;
                        }
                        else if (rva == VerticalAlignStyle.Bottom)
                        {
                            txtRect.Y += txtRect.Height - fh;
                        }
                        txtRect.Height = fh;
                        txtRect.Y += args.Render.CharMeasurer.CharTopFix;
                    }
                    var c = rs.Color;
                    args.Graphics.DrawString(
                        info.Text,
                        rs.Font,
                        c,
                        txtRect,
                        format);
                    if (rs.Underline)
                    {
                        args.Render.DrawUnderLine(
                            this,
                            args,
                            c,
                            new DCSystem_Drawing.RectangleF(
                                args.ViewBounds.Left,
                                txtRect.Top,
                                args.ViewBounds.Width,
                                txtRect.Height));
                    }
                }
            }
        }


        public override void Dispose()
        {
            base.Dispose();
            this._Caption = null;
            this._CheckedValue = null;
        }

        private bool ChangeCheckedByUI(bool setCurrentElement)
        {
            if (this.Readonly == false)
            {
                if (this.OwnerDocument.DocumentControler.CanModify(
                    this,
                    DomAccessFlags.CheckUserEditable
                    | DomAccessFlags.CheckControlReadonly))
                {
                    // 鼠标点击设置勾选状态。
                    if (this.WriterControl != null)
                    {
                        if (setCurrentElement)
                        {
                            this.DocumentContentElement.Content.CurrentElement = this;
                        }
                    }
                    this.SetEditorChecked(!this.Checked);
                    return true;
                }
            }
            return false;
        }

        public override void HandleDocumentEvent(DocumentEventArgs args)
        {
            if (args.Style == DocumentEventStyles.MouseClick)
            {
                if (ChangeCheckedByUI(true))
                {
                    args.Handled = true;
                    return;
                }
                base.HandleDocumentEvent(args);// Handle_XTextObjectElement(element, args);//base.XTextElement_HandleDocumentEvent(args);
                args.Handled = true;
            }
            else if (args.Style == DocumentEventStyles.MouseMove)
            {
                if (this.WriterControl != null)
                {
                    if (this.Multiline == false)
                    {
                        args.Cursor = System.Windows.Forms.Cursors.Arrow;
                    }
                    else
                    {
                        base.HandleDocumentEvent(args);//Handle_XTextObjectElement(element, args);//base.HandleDocumentEvent(args);
                    }
                    args.Handled = true;
                }
            }
            else if (args.Style == DocumentEventStyles.MouseDown)
            {
                base.HandleDocumentEvent(args);//Handle_XTextObjectElement(element, args);//base.HandleDocumentEvent(args);
                args.Handled = true;
            }
            else
            {
                base.HandleDocumentEvent(args);//Handle_XTextObjectElement(element, args);//base.HandleDocumentEvent(args);
            }

        }
    }

}