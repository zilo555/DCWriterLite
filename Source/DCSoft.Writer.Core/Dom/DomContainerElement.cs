using System.Text;
using DCSoft.Writer.Controls;
using DCSoft.Writer.Data;
using DCSoft.Common;
using DCSystemXml;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Reflection;
namespace DCSoft.Writer.Dom
{
    /// <summary>
    /// 容器元素对象
    /// </summary>
    /// <remarks>
    /// 本类型是从XTextElement上派生的容器文本文档元素类型,它能包含其他的文本文档元素,
    /// 还可以包含其他的容器元素.是文本文档对象模型中比较基础的类型.
    /// 编制 袁永福 2007-3-21
    /// </remarks>
    public abstract partial class DomContainerElement : DomElement
    {
        /// <summary>
        /// 初始化对象
        /// </summary>
        protected DomContainerElement()
        {
            //this._Attributes = new XAttributeList();
            this._Elements = new DomElementList();
        }
        /// <summary>
        /// 专门为XTextStringElement提供的构造函数
        /// </summary>
        /// <param name="v"></param>
        internal protected DomContainerElement(int v)
        {
            this._Elements = new DomElementList();
            this.ChildElementTypeStateReady = false;
        }
        private string _ID;
        /// <summary>
        /// 对象编号
        /// </summary>
        public override string ID
        {
            get
            {
                return this._ID;
            }
            set
            {
                this._ID = value;
            }
        }
        internal bool ChildElementTypeStateReady
        {
            get
            {
                return this.StateFlag31;
            }
            set
            {
                this.StateFlag31 = value;
            }
        }

        internal bool HasCharElement
        {
            get
            {
                return this.StateFlag30;
            }
            set
            {
                this.StateFlag30 = value;
            }
        }
        internal bool HasParagraphFlagElement
        {
            get
            {
                return this.StateFlag29;
            }
            set
            {
                this.StateFlag29 = value;
            }
        }
        internal bool HasChildObjectElement
        {
            get
            {
                return this.StateFlag28;
            }
            set
            {
                this.StateFlag28 = value;
            }
        }
        internal bool HasChildContainerElement
        {
            get
            {
                return this.StateFlag27;
            }
            set
            {
                this.StateFlag27 = value;
            }
        }

        internal static bool GlobalEnabledResetChildElementStats = true;
        /// <summary>
        /// 不是字符和段落符号的子元素数组
        /// </summary>
        internal IList<DomElement> _ChildElementsNotCharOrParagraphFlag = null;
        internal virtual void ResetChildElementStats()
        {
            if (this.ChildElementTypeStateReady)
            {
                this.ChildElementTypeStateReady = false;
                if (this._ChildElementsNotCharOrParagraphFlag is System.Array)
                {
                    Array.Clear((System.Array)this._ChildElementsNotCharOrParagraphFlag);
                    this._ChildElementsNotCharOrParagraphFlag = null;
                }
            }
        }
        internal virtual IList<DomElement> GetCompressedElements()
        {
            this.CheckChildElementStatsReady();
            return this._ChildElementsNotCharOrParagraphFlag;
        }

        internal virtual void CheckChildElementStatsReady()
        {
            if (this.ChildElementTypeStateReady == false)
            {
                this.ChildElementTypeStateReady = true;
                this.HasCharElement = false;
                this.HasParagraphFlagElement = false;
                this.HasChildObjectElement = false;
                this.HasChildContainerElement = false;
                this._ChildElementsNotCharOrParagraphFlag = null;
                if (this._Elements != null && this._Elements.Count > 0)
                {
                    if (this._Elements.OnlyHasSingleParagraphFlagElement())
                    {
                        // 在一些情况下只包含一个段落符号元素，则快速设置。
                        this.HasCharElement = false;
                        this.HasParagraphFlagElement = true;
                        this.HasChildObjectElement = false;
                        this.HasChildContainerElement = false;
                        this._ChildElementsNotCharOrParagraphFlag = null;
                    }
                    else
                    {
                        var bolHasCharElement = false;
                        var bolHasParagraphFlagElement = false;
                        var bolHasObjectElement = false;
                        var bolHasContainerElement = false;
                        var list = _CachedList_SetTextRawDOM;
                        list.InnerSetArrayRaw(LoaderListBuffer<DomElement[]>.Instance.Alloc(), 0);
                        var arr = this._Elements.InnerGetArrayRaw();
                        var arrLen = this._Elements.Count;
                        for (var iCount = 0; iCount < arrLen; iCount++)
                        {
                            var element = arr[iCount];
                            if (element is DomCharElement)
                            {
                                bolHasCharElement = true;
                            }
                            else if (element is DomParagraphFlagElement)
                            {
                                bolHasParagraphFlagElement = true;
                            }
                            else
                            {
                                list.Add(element);
                                if (element is DomContainerElement)
                                {
                                    bolHasContainerElement = true;
                                }
                                else if (element is DomObjectElement)
                                {
                                    bolHasObjectElement = true;
                                }
                            }
                        }//for
                        this.HasCharElement = bolHasCharElement;
                        this.HasParagraphFlagElement = bolHasParagraphFlagElement;
                        this.HasChildObjectElement = bolHasObjectElement;
                        this.HasChildContainerElement = bolHasContainerElement;
                        if (list.Count > 0)
                        {
                            this._ChildElementsNotCharOrParagraphFlag = list.ToArray();
                        }
                        LoaderListBuffer<DomElement[]>.Instance.Return(list.InnerGetArrayRaw());
                        list.ClearAndEmpty();
                    }
                }
            }
        }


        /// <summary>
        /// 判断是否存在指定类型的子元素
        /// </summary>
        /// <typeparam name="ElementType">文档元素对象</typeparam>
        /// <returns>是否存在子元素</returns>
        public bool HasChildElement<ElementType>()
        {
            this.CheckChildElementStatsReady();
            if (typeof(DomCharElement).IsAssignableFrom(typeof(ElementType)))
            {
                return this.HasCharElement;
            }
            if (typeof(ElementType) == typeof(DomParagraphFlagElement))
            {
                return this.HasParagraphFlagElement;
            }
            if (typeof(DomContainerElement).IsAssignableFrom(typeof(ElementType)))
            {
                if (this.HasChildContainerElement == false)
                {
                    return false;
                }
            }
            else if (typeof(DomObjectElement).IsAssignableFrom(typeof(ElementType)))
            {
                if (this.HasChildObjectElement == false)
                {
                    return false;
                }
            }
            if (this._ChildElementsNotCharOrParagraphFlag != null)
            {
                foreach (var item in this._ChildElementsNotCharOrParagraphFlag)
                {
                    if (item is ElementType)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// 可靠的判断出只包含字符或段落符号元素
        /// </summary>
        /// <returns></returns>
        public bool OnlyHasCharOrParagraphFlag()
        {
            this.CheckChildElementStatsReady();
            {
                if (this.HasCharElement || this.HasParagraphFlagElement)
                {
                    return this._ChildElementsNotCharOrParagraphFlag == null;
                }
                else
                {
                    return this.HasCharElement == false && this.HasParagraphFlagElement == false;
                }
            }
            //var ct = this.ChildElementTypes;
            //return ct == DCChildElementType.Char
            //    || ct == (DCChildElementType.Char | DCChildElementType.ParagraphFlag)
            //    || ct == DCChildElementType.ParagraphFlag;
        }


        internal virtual void ClearRuntimeStyle()
        {
            if (this._Elements != null)
            {
                for (int iCount = this._Elements.Count - 1; iCount >= 0; iCount--)
                {
                    var e = this._Elements[iCount];
                    e._RuntimeStyle = null;
                    if (e is DomContainerElement)
                    {
                        ((DomContainerElement)e).ClearRuntimeStyle();
                    }
                }
            }
        }
        /// <summary>
        /// 内容版本号
        /// </summary>
        internal int _ContentVersion;
        /// <summary>
        /// 元素内容版本号，当元素内容发生任何改变时，该属性值都会改变
        /// </summary>
        public override int ContentVersion
        {
            //[JSInvokable]
            get
            {
                return _ContentVersion;
            }
        }


        private string _ToolTip;
        /// <summary>
        /// 提示文本
        /// </summary>
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public virtual string ToolTip
        {
            //[JSInvokable]
            get
            {
                return _ToolTip;
            }
            //[JSInvokable]
            set
            {
                _ToolTip = value;
            }
        }


        /// <summary>
        /// 实际使用的移动焦点所使用的快捷键样式
        /// </summary>
        public virtual MoveFocusHotKeys RuntimeMoveFocusHotKey
        {
            get
            {
                return MoveFocusHotKeys.None;
            }
        }

        /// <summary>
        /// 能否接受制表符，默认false。
        /// </summary>
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public virtual bool AcceptTab
        {
            get { return this.StateFlag03; }
            set { this.StateFlag03 = value; }
        }

        /// <summary>
        /// 获取或设置一个运行时的值，该值指示用户能否使用 Tab 键将焦点放到该元素中上。 
        /// </summary>
        public virtual bool RuntimeTabStop()
        {

            return false;

        }




        #region 属性校验相关的功能 ******************************************


        private ValueValidateResult _LastValidateResult;
        /// <summary>
        /// 最后一次数据校验的结果
        /// </summary>
        public ValueValidateResult LastValidateResult
        {
            //[JSInvokable]
            get
            {
                return _LastValidateResult;
            }

        }

        public void SetLastValidateResult(ValueValidateResult v)
        {
            if (this.OwnerDocument != null)
            {
                if (this._LastValidateResult != null)
                {
                    if (this._LastValidateResult.EqualsValue(v) == false)
                    {
                        this.OwnerDocument.IncreaseElementValueValidateVersion();
                    }
                }
                else if (v != null && v.EqualsValue(this._LastValidateResult) == false)
                {
                    this.OwnerDocument.IncreaseElementValueValidateVersion();
                }
            }
            this._LastValidateResult = v;
        }

        internal void InnerClearLastValidateResult()
        {
            //this._LastRenderResult = null;
            if (this._ValidateStyle != null)
            {
                this._ValidateStyle.ContentVersion = this.ContentVersion - 1;
            }
        }

        protected ValueValidateStyle _ValidateStyle;
        /// <summary>
        /// 数据验证样式
        /// </summary>
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public virtual ValueValidateStyle ValidateStyle
        {
            get
            {
                return _ValidateStyle;
            }
            set
            {
                _ValidateStyle = value;
            }
        }
        /// <summary>
        /// 清除数据校验结果
        /// </summary>
        /// <param name="fastMode">快速操作模式</param>
        /// <returns>操作是否改变了输入域状态</returns>

        public virtual bool ClearValidateResult(bool fastMode)
        {
            if (this._LastValidateResult != null)
            {
                this._LastValidateResult = null;
                if (this._ValidateStyle != null)
                {
                    this._ValidateStyle.ContentVersion = this.ContentVersion;
                }
                if (fastMode == false)
                {
                    this.OnValidated();
                }
                return true;
            }
            return false;
        }


        /// <summary>
        /// 进行数据验证
        /// </summary>
        public virtual ValueValidateResult Validating(bool loadingDocument)
        {
            return DefaultDOMDataProvider.Validating_XTextContainerElement(this, loadingDocument);
        }

        /// <summary>
        /// 获得高亮度设置信息
        /// </summary>
        /// <returns>获得的设置信息</returns>
        public override HighlightInfoList GetHighlightInfos()
        {
            if (this._LastValidateResult != null)
            {
                // 启用内容校验而且出现校验错误，则高亮度显示
                var vopts = GetDocumentViewOptions();
                Color bc = vopts.FieldInvalidateValueBackColor;
                Color fc = vopts.FieldInvalidateValueForeColor;
                if (bc.A != 0 || fc.A != 0)
                {
                    if (this.DocumentContentElement != null
                        && this.FirstContentElement != null
                        && this.LastContentElement != null)
                    {
                        HighlightInfo info = new HighlightInfo(
                            this,
                            new DCRange(
                                this.DocumentContentElement,
                                this.FirstContentElement,
                                this.LastContentElement),
                           bc,
                           fc,
                           HighlightActiveStyle.Static);
                        HighlightInfoList list = new HighlightInfoList();
                        list.Add(info);
                        return list;
                    }
                }
            }
            return null;
        }

        public override ElementToolTip GetToolTipInfo()
        {
            // 添加元素提示文本
            if (this._LastValidateResult != null)
            {
                // 显示错误提示文本
                ElementToolTip tip = new ElementToolTip(this, this._LastValidateResult.Message);
                tip.ContentType = ToolTipContentType.ValidateResult;
                tip.Level = ToolTipLevel.Warring;
                return tip;
            }
            var rtt = this.ToolTip;
            if (rtt != null && rtt.Length > 0)
            {
                ElementToolTip info = new ElementToolTip(this, rtt);
                info.ContentType = ToolTipContentType.ElementToolTip;
                return info;
            }
            return base.GetToolTipInfo();
        }

        /// <summary>
        /// 触发数据验证结束事件
        /// </summary>
        public virtual void OnValidated()
        {
        }
        protected void OnContentChanged_Validate(ContentChangedEventArgs args)
        {
            bool raiseValidate = false;
            DocumentValueValidateMode validateMode = GetDocumentEditOptions().ValueValidateMode;
            if (validateMode == DocumentValueValidateMode.Dynamic)
            {
                // 实时的进行数据校验
                raiseValidate = true;
            }
            else if (args.UndoRedoCause)
            {
                // 由于执行重复/撤销操作而引起的该事件
                if (validateMode == DocumentValueValidateMode.LostFocus)
                {
                    raiseValidate = true;
                }
            }

            ValueValidateStyle valStyle = this.ValidateStyle;
            if (valStyle != null
                && valStyle.Required
                && valStyle.RequiredInvalidateFlag)
            {
                // 对于必填内容设置实时的进行数据校验
                if (this.LastValidateResult != null)
                {
                    raiseValidate = true;
                }
            }

            if (raiseValidate)
            {
                // 执行数据校验
                this.Validating(args.LoadingDocument);
            }
        }


        #endregion

        /// <summary>
        /// 判断是否包含了未分页处理的分页符号
        /// </summary>
        internal virtual bool GetContainsUnHandledPageBreak()
        {
            return this.StateFlag06;// this._ContainsUnHandledPageBreak;
        }

        /// <summary>
        /// 设置是否包含了未分页处理的分页符号
        /// </summary>
        internal virtual void SetContainsUnHandledPageBreak(bool v)
        {
            this.StateFlag06 = v;// this._ContainsUnHandledPageBreak = v;
            //if (this._ContainsUnHandledPageBreak != v)
            //{
            //    _ContainsUnHandledPageBreak = v;
            //}
        }


        /// <summary>
        /// 元素内容是否改变
        /// </summary>
        public override bool Modified
        {
            //[JSInvokable]
            get
            {
                return this.StateFlag11;// this._Modified;
            }
            //[JSInvokable]
            set
            {
                //if (this._Modified != value)
                {
                    this.StateFlag11 = value;// this._Modified = value;
                }
            }
        }

        /// <summary>
        /// 是否具有内容元素
        /// </summary>
        public virtual bool HasContentElement
        {
            get { return this.StateFlag09; }
            set { this.StateFlag09 = value; }
        }

        public override string FormulaValue
        {
            //[JSInvokable]
            get
            {
                return this.Text;
            }
            //[JSInvokable]
            set
            {
                //XTextInputFieldElement field = (XTextInputFieldElement)this.Elements.GetElement(
                //           typeof(XTextInputFieldElement));
                SetContainerTextArgs args = new SetContainerTextArgs();
                args.NewText = this.FixInputFormulaValue(value);
                args.LogUndo = false;
                args.AccessFlags = DomAccessFlags.None;
                args.EventSource = ContentChangedEventSource.Default;
                args.FocusContainer = false;
                args.IgnoreDisplayFormat = false;
                args.ShowUI = false;
                args.UpdateContent = true;
                this.SetEditorText(args);
            }
        }


        /// <summary>
        /// 收集被保护的文档内容。
        /// </summary>
        /// <param name="list">容纳元素的列表,可以为空</param>
        /// <param name="limitedCount">限制的元素个数，可以为空</param>
        /// <remarks>如果参数pes为空，则表示仅仅是检查是否存在受保护的内容，而不收集</remarks>
        /// <returns>是否存在受保护的内容</returns>
        public virtual bool CollecProtectedElements(ContentProtectedInfoList list, int limitedCount)
        {
            DomDocument document = this.OwnerDocument;
            bool result = false;
            var ctl = document.DocumentControler;
            DomElementList elements = this.Elements;
            int endIndex = elements.Count - 1;
            if (this is DomContentElement)
            {
                endIndex = endIndex - 1;
            }
            ElementStateDetectEventArgs args = new ElementStateDetectEventArgs(null, DomAccessFlags.Normal & ~DomAccessFlags.CheckUserEditable);
            args.ResetLastContentProtectedInfo = true;
            args.CacheParentStateResult = true;
            for (int iCount = 0; iCount <= endIndex; iCount++)
            {
                DomElement element = elements[iCount];
                args.Element = element;
                args.Message = null;
                ctl.CanDelete(args);
                if (args.Result == false)
                {
                    ctl.AddLastContentProtectdInfoOnce(list);
                    result = true;
                    if (list == null)
                    {
                        break;
                    }
                    else
                    {
                        if (limitedCount == 0 || limitedCount < list.Count)
                        {
                            ctl.AddLastContentProtectdInfoOnce(list);
                        }
                    }
                    continue;
                }
                if (element is DomContainerElement)
                {
                    bool result2 = ((DomContainerElement)element).CollecProtectedElements(
                        list, limitedCount);
                    if (result2)
                    {
                        result = true;
                        if (list == null)
                        {
                            if (result)
                            {
                                break;
                            }
                        }
                    }
                    if (limitedCount > 0 && list.Count >= limitedCount)
                    {
                        break;
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 返回子元素背景样式
        /// </summary>
        public virtual RuntimeDocumentContentStyle GetContentBackgroundStyle(DomElement childElement)
        {
            RuntimeDocumentContentStyle rs = this.RuntimeStyle;
            if (rs != null && rs.HasVisibleBackground)
            {
                return rs;
            }
            else
            {
                return null;
            }
            //return null;
        }


        /// <summary>
        /// 对象所属文档对象
        /// </summary>
        public override DomDocument OwnerDocument
        {
            get
            {
                return base._OwnerDocument;
            }
            set
            {
                if (base._OwnerDocument != value)
                {
                    base._OwnerDocument = value;
                    if (this._Elements != null && this._Elements.Count > 0)
                    {
                        var arr = this._Elements.InnerGetArrayRaw();
                        for (var iCount = this._Elements.Count - 1; iCount >= 0; iCount--)
                        {
                            arr[iCount].OwnerDocument = value;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 创建新的文档对象，使其包含本文档元素的复制品
        /// </summary>
        /// <param name="includeThis">是否包含本文档原始对象</param>
        /// <returns>创建的文档对象</returns>
        public override DomDocument CreateContentDocument(bool includeThis)
        {
            DomElementList elements = new DomElementList();
            if (includeThis)
            {
                elements.Add(this);
            }
            else
            {
                elements.AddRangeByDCList(this.Elements);
            }
            return WriterUtilsInner.CreateDocument(this.OwnerDocument, elements, true);
        }

        /// <summary>
        /// 第一个子元素
        /// </summary>
        public virtual DomElement FirstChild
        {
            get
            {
                if (this._Elements == null)
                {
                    return null;
                }
                else
                {
                    return this._Elements.FirstElement;
                }
            }
        }

        /// <summary>
        /// 最后一个子元素
        /// </summary>
        public virtual DomElement LastChild
        {
            get
            {
                if (this._Elements == null)
                {
                    return null;
                }
                else
                {
                    return this._Elements.LastElement;
                }
            }
        }

        /// <summary>
        /// 获得所有的文档元素对象,包括自己
        /// </summary>
        /// <returns>元素列表</returns>
        public virtual DomElementList GetAllElements()
        {
            DomElementList list = new DomElementList();
            GetAllElements(this, list, true);
            return list;
        }

        private void GetAllElements(DomElement root, DomElementList result, bool inlucdeChar)
        {
            result.Add(root);
            if (root.Elements != null)
            {
                foreach (DomElement element in root.Elements.FastForEach())
                {
                    if (inlucdeChar == false && element is DomCharElement)
                    {
                        continue;
                    }
                    GetAllElements(element, result, inlucdeChar);
                }
            }
        }

        /// <summary>
        /// 是否存在子元素
        /// </summary>
        /// <returns></returns>
        public virtual bool HasElements()
        {
            return this._Elements != null && this._Elements.Count > 0;
        }

        /// <summary>
        /// 子元素列表
        /// </summary>
        internal protected DomElementList _Elements;
        /// <summary>
        /// 子元素列表
        /// </summary>
        public override DomElementList Elements
        {
            get
            {
                return _Elements;
            }
            set
            {
                this._Elements = value;
            }
        }

        /// <summary>
        /// 判断元素是否挂在指定文档的DOM结构中
        /// </summary>
        /// <param name="document">文档对象</param>
        /// <returns>是否挂在DOM结构中</returns>
        public override bool BelongToDocumentDom(DomDocument document)
        {
            if (document == null)
            {
                return false;
            }
            DomElement current = this;
            while (true)
            {
                if (current == document)
                {
                    return true;
                }
                var p = current._Parent;
                if (p != null && p.ContainsChild(current))
                {
                    current = p;
                }
                else
                {
                    return false;
                }
            }
            return false;
        }

        /// <summary>
        /// 判断是否包含直接子元素
        /// </summary>
        /// <param name="element">子元素对象</param>
        /// <returns>是否包含。</returns>
        public virtual bool ContainsChild(DomElement element)
        {
            if (this._Elements != null)
            {
                if (this._Elements.EqualItem(element._ElementIndex, element))
                {
                    return true;
                }
                var index = this._Elements.IndexOf(element);
                if (index >= 0)
                {
                    element._ElementIndex = index;
                    return true;
                }
            }
            return false;
            //return this._Elements != null && this._Elements.Contains(element);
        }
        public override void InnerCommitStyleDeeply()
        {
            base.InnerCommitStyleDeeply();
            if (this.Elements != null)
            {
                foreach (DomElement elment in this.Elements)
                {
                    elment.InnerCommitStyleDeeply();
                }
            }
        }
        /// <summary>
        /// 子元素的个数
        /// </summary>
        public virtual int ElementsCount
        {
            //[JSInvokable]
            get
            {
                return this._Elements == null ? 0 : this._Elements.Count;
                //if (this._Elements != null)
                //{
                //    return this._Elements.Count;
                //}
                //else
                //{
                //    return 0;
                //}
            }
        }

        /// <summary>
        /// 准备执行序列化操作
        /// </summary>
        public override void PrepareSerialize(string format)
        {
            if (this.HasElements())
            {
                var arr = this.Elements.InnerGetArrayRaw();
                var len = this.Elements.Count;
                //foreach (XTextElement element in this.Elements.FastForEach())
                for (var iCount = 0; iCount < len; iCount++)
                {
                    var element = arr[iCount];
                    if ((element is DomCharElement) == false
                        && (element is DomParagraphFlagElement) == false)
                    {
                        element.PrepareSerialize(format);
                    }
                }
            }
        }

        /// <summary>
        /// 内部的文档加载后的处理
        /// </summary>
        /// <param name="args">事件参数</param>
        public override void AfterLoad(ElementLoadEventArgs args)
        {
            this._ContentVersion++;
            //if (FixElementsForSerialize(false))
            //{
            //    //WriterUtils.SplitElements(this.Elements);
            //}
            //WriterUtilsInner.SplitElements(this.Elements, false, this.OwnerDocument , this,true );
            if (this is DomContentElement)
            {
                ((DomContentElement)this).FixElements();
            }
            //try
            //{
            //XTextDocument doc = this.ElementOwnerDocument();
            var es = this.Elements;
            var esCount = es.Count;
            //foreach (XTextElement element in this.Elements.FastForEach())
            var arr = es.InnerGetArrayRaw();
            for (var iCount = 0; iCount < esCount; iCount++)
            {
                var element = arr[iCount];// es.GetItemFast(iCount);
                element.SetParentAndDocumentRaw(this);
                if ((element is DomCharElement) == false
                    && (element is DomParagraphFlagElement) == false)
                {
                    args.Element = this;
                    element.AfterLoad(args);
                }
            }
            //}
            //catch (Exception ext)
            //{
            //    System.Diagnostics.Debug.WriteLine(ext.Message);
            //}
            args.Element = this;
            base.AfterLoad(args);
        }

        /// <summary>
        /// 子孙元素中第一个显示在文档内容中的元素
        /// </summary>
        public override DomElement FirstContentElement
        {
            get
            {
                foreach (DomElement element in this._Elements.FastForEach())
                {
                    if (element.RuntimeVisible)
                    {
                        if (element is DomTableElement)
                        {
                            DomTableElement table = (DomTableElement)element;
                            if (table.FirstCell != null)
                            {
                                return table.FirstCell.FirstContentElement;
                            }
                        }
                        if (element is DomContainerElement)
                        {
                            DomContainerElement c = (DomContainerElement)element;
                            DomElement fc = c.FirstContentElement;
                            if (fc != null)
                            {
                                return fc;
                            }
                        }
                        else
                        {
                            return element;
                        }
                    }
                }
                return null;
            }
        }
        /// <summary>
        /// 子孙元素中第一个显示在文档内容中的元素
        /// </summary>
        public override DomElement LastContentElement
        {
            get
            {
                for (int iCount = this._Elements.Count - 1; iCount >= 0; iCount--)
                {
                    DomElement element = this._Elements[iCount];
                    if (element.RuntimeVisible)
                    {
                        if (element is DomTableElement)
                        {
                            DomTableElement table = (DomTableElement)element;
                            if (table.LastVisibleCell != null)
                            {
                                return table.LastVisibleCell.LastContentElement;
                            }
                            return null;
                        }
                        if (element is DomContainerElement)
                        {
                            DomContainerElement c = (DomContainerElement)element;
                            DomElement e = c.LastContentElement;
                            if (e != null)
                            {
                                return e;
                            }
                        }
                        else
                        {
                            return element;
                        }
                    }
                }
                return null;
            }
        }

        /// <summary>
        /// 判断是否包含被用户选择的内容
        /// </summary>
        public override bool HasSelection
        {
            //[JSInvokable]
            get
            {

                DomElement first = this.FirstContentElement;
                DomElement last = this.LastContentElement;
                DomDocumentContentElement ce = this.DocumentContentElement;
                if (ce != null
                    && ce.Selection != null
                    && ce.Selection.Length > 0)
                {
                    if (ce.Selection.Ranges.IntersectsWith(first.ContentIndex, last.ContentIndex))
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        /// <summary>
        /// 对整个内容执行重新排版操作
        /// </summary>
        public virtual void InnerExecuteLayout()
        {
        }

        /// <summary>
        /// 允许接收的子元素类型
        /// </summary>
        public virtual ElementType RuntimeAcceptChildElementTypes()
        {
            return ElementType.All;
        }

        /// <summary>
        /// 插入子元素
        /// </summary>
        /// <param name="index">指定的序号</param>
        /// <param name="element">新添加的元素</param>
        /// <returns>操作是否成功</returns>
        public virtual bool InsertChildElement(int index, DomElement element)
        {
            if (element != null)
            {
                element.FixDomStateForMove(this);
                if (_Elements.Contains(element) == false)
                {
                    _Elements.Insert(index, element);
                }

                //element.Parent = this;
                //element.OwnerDocument = this.OwnerDocument;
                return true;
            }
            return false;
        }

        /// <summary>
        /// 添加子元素
        /// </summary>
        /// <param name="element">新添加的元素</param>
        /// <returns>操作是否成功</returns>
        public virtual bool AppendChildElement(DomElement element)
        {
            if (element is DomDocument)
            {
                DomDocument doc = (DomDocument)element;
                foreach (DomElement element2 in doc.Body.Elements.FastForEach())
                {
                    element2.OwnerDocument = this.OwnerDocument;
                    element2.Parent = this;
                    this._Elements.Add(element2);
                }
                return true;
            }
            else
            {
                if (element != null)
                {
                    if (element.Parent != null && element.Parent != this)
                    {
                        DomContainerElement p = element.Parent;
                        if (p.Elements.Contains(element))
                        {
                            // 从旧的父元素下脱离开来
                            p.Elements.Remove(element);
                        }
                    }
                    if (_Elements.Contains(element) == false)
                    {
                        _Elements.Add(element);
                    }
                    element.SetParentRaw(this);
                    if (element is DomContainerElement)
                    {
                        element.OwnerDocument = this.OwnerDocument;
                    }
                    else
                    {
                        element.SetOwnerDocumentRaw(this.OwnerDocument);
                    }

                    return true;
                }
                return false;
            }
        }

        /// <summary>
        /// 删除子元素
        /// </summary>
        /// <param name="element">要删除的子元素</param>
        /// <returns>操作是否成功</returns>
        public virtual bool RemoveChild(DomElement element)
        {
            if (element != null)
            {
                _Elements.Remove(element);
                element.SetParentRaw(null);
                return true;
            }
            return false;
        }

        /// <summary>
        /// 获得运行时可见性的值
        /// </summary>
        /// <returns>可见性</returns>
        public virtual bool GetRuntimeVisibleValue(UpdateElementsRuntimeVisibleArgs args)
        {
            var p = this.Parent;
            if (p != null && p.RuntimeVisible == false)
            {
                return false;
            }
            return this.Visible;
        }

        /// <summary>
        /// 获得元素运行时可见性的方法参数
        /// </summary>
        public class UpdateElementsRuntimeVisibleArgs
        {
            internal static UpdateElementsRuntimeVisibleArgs _Template = null;
            public static UpdateElementsRuntimeVisibleArgs Create(DomDocument document, bool deeply)
            {
                if (_Template == null || _Template.Document != document)
                {
                    return new UpdateElementsRuntimeVisibleArgs(document, deeply);
                }
                else
                {
                    _Template.Deeply = deeply;
                    return _Template;
                }
            }
            /// <summary>
            /// 初始化对象
            /// </summary>
            /// <param name="element">文档元素</param>
            /// <param name="deeply">是否为深入子孙节点判断</param>
            internal UpdateElementsRuntimeVisibleArgs(DomDocument document, bool deeply)
            {
                if (document == null)
                {
                    throw new ArgumentNullException("document");
                }
                this.Document = document;
                var styles = this.Document.ContentStyles;
                this.Deeply = deeply;
                this.StatesPrinting = this.Document.States.Printing;
            }
            public readonly DomDocument Document;
            public bool Deeply;
            public bool StatesPrinting;
        }

        /// <summary>
        /// 更新文档元素的可见性,DCWriter内部使用.
        /// </summary>
        /// <param name="deeply">是否深入设置子孙元素</param>
        public void UpdateElementsRuntimeVisible(bool deeply)
        {
            InnerUpdateElementsRuntimeVisible(UpdateElementsRuntimeVisibleArgs.Create(this.OwnerDocument, deeply));
        }

        /// <summary>
        /// 更新文档元素的可见性,DCWriter内部使用.
        /// </summary>
        /// <param name="args">参数</param>
        public virtual void InnerUpdateElementsRuntimeVisible(UpdateElementsRuntimeVisibleArgs args)
        {
            var deeply = args.Deeply;
            if (args.Document == null)
            {
                bool vs = this._RuntimeVisible;
                //this.RuntimeVisible = vs ;
                foreach (DomElement element in this._Elements.FastForEach())
                {
                    element._RuntimeVisible = vs && element.Visible;
                    if (deeply && element is DomContainerElement)
                    {
                        ((DomContainerElement)element).InnerUpdateElementsRuntimeVisible(args);
                    }
                }
                return;
            }
            this._RuntimeVisible = this.GetRuntimeVisibleValue(args);
            if (this._RuntimeVisible)
            {
                var thisElementsArray = this._Elements.InnerGetArrayRaw();
                var thisElementsCount = this._Elements.Count;
                var bolIsOnlyCharOrParagraphFlag = false;
                //var ce = DCChildElementType.Invalidate;
                if (this is DomContentElement)
                {
                    bolIsOnlyCharOrParagraphFlag = this.OnlyHasCharOrParagraphFlag();
                    //ce = this.ChildElementTypes;
                }
                // 容器元素可见
                if (bolIsOnlyCharOrParagraphFlag)
                {
                    // 纯文本内容
                    for (int iCount = thisElementsCount - 1; iCount >= 0; iCount--)
                    {
                        var element = thisElementsArray[iCount];
                        element._RuntimeVisible = element.Visible;
                    }
                }
                else
                {
                    for (int iCount = thisElementsCount - 1; iCount >= 0; iCount--)
                    {
                        var element = thisElementsArray[iCount];
                        if (element is DomContainerElement c)
                        {
                            if (deeply)
                            {
                                c.InnerUpdateElementsRuntimeVisible(args);
                            }
                            else
                            {
                                c._RuntimeVisible = c.GetRuntimeVisibleValue(args);
                            }
                        }
                        else
                        {
                            // 逻辑删除的内容都可见，则进行简单的判断。
                            element._RuntimeVisible = element.Visible;
                        }
                    }
                }
            }
            else
            {
                // 容器元素不可见
                if (deeply)
                {
                    // 设置所有的子孙元素不可见
                    HiddenElementsDeeply();
                }
            }
        }

        /// <summary>
        /// 设置所有子孙元素运行时不可见
        /// </summary>
        internal protected void HiddenElementsDeeply()
        {
            for (int iCount = this._Elements.Count - 1; iCount >= 0; iCount--)
            {
                var element = this._Elements[iCount];
                element._RuntimeVisible = false;
                if (element is DomContainerElement)
                {
                    ((DomContainerElement)element).HiddenElementsDeeply();
                }
            }
        }
        /// <summary>
        /// AppendViewContentElement函数使用的参数
        /// </summary>
        public class AppendViewContentElementArgs
        {
            /// <summary>
            /// 充当模板的参数对象
            /// </summary>
            [ThreadStatic]
            internal static AppendViewContentElementArgs _Template = null;
            internal static void SetTemplate(DomDocument document)
            {
                if (document == null)
                {
                    _Template = null;
                }
                else
                {
                    _Template = new AppendViewContentElementArgs(document, new DomElementList(), false);
                }
            }
            internal AppendViewContentElementArgs(DomDocument document, DomElementList list, bool privateMode)
            {
                if (document == null)
                {
                    throw new ArgumentNullException("document");
                }
                if (list == null)
                {
                    throw new ArgumentNullException("list");
                }
                this.Document = document;
                this.Content = list;
                this.PrivateMode = privateMode;
                if (_Template != null && _Template.Document == document)
                {
                    this.PrintingViewMode = _Template.PrintingViewMode;
                    this.PreserveBackgroundTextWhenPrint = _Template.PreserveBackgroundTextWhenPrint;
                    this.PrintBackgroundText = _Template.PrintBackgroundText;
                    this.RenderMode = _Template.RenderMode;
                }
                else
                {
                    this.PrintingViewMode = document.PrintingViewMode();
                    var vos = document.GetDocumentViewOptions();
                    this.PreserveBackgroundTextWhenPrint = vos.PreserveBackgroundTextWhenPrint;
                    this.PrintBackgroundText = vos.PrintBackgroundText;
                    this.RenderMode = document.States.RenderMode;
                }
            }

            public bool RuntimePrintBackgroundText(DCBooleanValue v)
            {
                if (v == DCBooleanValue.Inherit)
                {
                    return this.PrintBackgroundText;
                }
                else if (v == DCBooleanValue.True)
                {
                    return true;
                }
                else if (v == DCBooleanValue.False)
                {
                    return false;
                }
                else
                {
                    return this.PrintBackgroundText;
                }
            }
            /// <summary>
            /// 容器的子孙节点中是否存在对象类型元素
            /// </summary>
            public DCBooleanValue HasObjectElement = DCBooleanValue.False;
            /// <summary>
            /// 容器的子孙节点中是否存在输入域元素
            /// </summary>
            public bool HasFieldElements = false;
            public bool From_FixBorderElementWidth = false;
            public readonly DocumentRenderMode RenderMode;
            public readonly bool PrintBackgroundText;
            public readonly bool PreserveBackgroundTextWhenPrint;
            //public readonly DocumentViewOptions ViewOptions;
            public readonly DomDocument Document;
            public readonly DomElementList Content;
            public readonly bool PrivateMode;
            public readonly bool PrintingViewMode;
        }

        private static SubList<DomElement> _SubList;
        private int _LastAppendViewContentElement_ContentVersion = -1;
        /// <summary>
        /// DCWriter内部使用。将对象内容添加到文档视图元素内容中
        /// </summary>
        /// <param name="args">参数</param>
        /// <returns>添加的文档元素个数</returns>
        public virtual int AppendViewContentElement(AppendViewContentElementArgs args)
        {
            if (this._OwnerDocument == null)
            {
                return 0;
            }

            //var tick2 = _AppendViewContentElement_TickCount;
            //var tick = DateTime.Now.Ticks;
            if (args.PrivateMode == false
                && this._LastAppendViewContentElement_ContentVersion == this._ContentVersion
                && this._OwnerDocument.CheckDOMEffectBy_ReplaceElements_CurrentContainer(this))
            {
                // 正在执行XTextDocument.ReplaceElements()，则有条件的进行快速添加
                args.Content.AddRangeByDCList(this.Elements);
                args.HasObjectElement = DCBooleanValue.Inherit;
                //_AppendViewContentElement_TickCount += DateTime.Now.Ticks - tick;
                return this.Elements.Count;
            }
            AppendViewContentElementArgs argsBack = null;
            var content = args.Content;
            int result = content.Count;
            var privateMode = args.PrivateMode;
            bool isPrinting = args.PrintingViewMode;// args.IsPrinting;// this.OwnerDocument != null && this.OwnerDocument.States.Printing;
            var elements = this.Elements;
            int elementsCount = elements.Count;
            //在大多数情况下，容器元素都是可见的字符元素和段落符号元素，则尽量使用content.AddRangeRaw2()替代content.AddRaw()以提高性能。
            int lastCharIndex = 0;
            if (_SubList == null)
            {
                _SubList = new SubList<DomElement>();
            }
            var eArray = elements.InnerGetArrayRaw();
            content.EnsureCapacity(content.Count + elementsCount);
            bool addChildDirect = false;
            //if ( elementsCount > 5 
            //    && this is XTextContentElement
            //    && this.ChildElementTypes == ( DCChildElementType.Char | DCChildElementType.ParagraphFlag))
            //{
            //    // 判断是否需要快速添加
            //    addChildDirect = true;
            //    for( var iCount = 0;iCount < elementsCount; iCount ++ )
            //    {
            //        if(eArray[iCount]._RuntimeVisible == false )
            //        {
            //            addChildDirect = false;
            //            break;
            //        }
            //    }
            //}
            if (addChildDirect)
            {
                // 快速添加子元素
                content.AddRangeByDCList(elements);
            }
            else
            {
                for (int iCount = 0; iCount < elementsCount; iCount++)
                {
                    var element = eArray[iCount];
                    if (element._RuntimeVisible == false
                        || (element is DomCharElement || element is DomParagraphFlagElement) == false)
                    {
                        // 遇到不可见的元素，或者不是字符及段落符号类型的元素
                        if (element is DomObjectElement)
                        {
                            args.HasObjectElement = DCBooleanValue.True;
                            // 尽量挽救批量添加，进行判断。
                            if (isPrinting)
                            {
                                if (element.RuntimeVisible)
                                {
                                        continue;
                                }
                            }
                            else
                            {
                                if (element._RuntimeVisible)
                                {
                                        continue;
                                }
                            }
                        }
                        //isAllChar = false;
                        if (iCount > lastCharIndex)
                        {
                            _SubList.FastInit(elements, lastCharIndex, iCount - lastCharIndex);
                            content.AddRangeRaw2(_SubList);
                        }
                        lastCharIndex = iCount + 1;
                        if (isPrinting)
                        {
                            if (element.RuntimeVisible == false )
                            {
                                // 元素不参与排版
                                continue;
                            }
                        }
                        else
                        {
                            if (element._RuntimeVisible == false)
                            {
                                continue;
                            }
                        }
                        if (element is DomContainerElement)
                        {
                            if (element is DomTableElement)
                            {
                                // 特别处理表格元素
                                if (privateMode)
                                {
                                    content.FastAdd2(element);
                                }
                                else
                                {
                                    // 处理表格对象
                                    DomTableElement table = (DomTableElement)element;
                                    if (table.HasElements())
                                    {
                                        var rows = table.RuntimeRows;
                                        var rowsCount = rows.Count;
                                        var rowArr = rows.InnerGetArrayRaw();
                                        //foreach (XTextTableRowElement row in table.Rows.FastForEach())
                                        for (var rowIndex = 0; rowIndex < rowsCount; rowIndex++)
                                        {
                                            var row = rowArr[rowIndex];// rows.GetItemFast(rowIndex);
                                            var cells = row.Elements;
                                            var cellsCount = cells.Count;
                                            var cellsArray = cells.InnerGetArrayRaw();
                                            //foreach (XTextTableCellElement cell in row.Cells.FastForEach())
                                            for (var colIndex = 0; colIndex < cellsCount; colIndex++)
                                            {
                                                var cell = (DomTableCellElement)cellsArray[colIndex];
                                                if (cell.RuntimeVisible)
                                                {
                                                    cell.AppendViewContentElement(args);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                DomContainerElement c = (DomContainerElement)element;
                                c.AppendViewContentElement(args);
                            }
                        }
                        else
                        {
                            content.FastAdd2(element);
                        }

                    }
                }//for
                if (elementsCount > lastCharIndex)
                {
                    if (elementsCount - lastCharIndex == 1)
                    {
                        content.FastAdd2(elements[lastCharIndex]);
                    }
                    else if (lastCharIndex == 0)
                    {
                        content.AddRangeByDCList(elements);
                    }
                    else
                    {
                        _SubList.Init(elements, lastCharIndex, elementsCount - lastCharIndex);
                        content.AddRangeRaw2(_SubList);
                    }
                }
                _SubList.CleanStates();
            }
            result = content.Count - result;
            if (args.PrivateMode == false
                && result == this.Elements.Count
                && this._OwnerDocument.CheckDOMEffectBy_ReplaceElements_CurrentContainer(this))
            {
                // 为了未来的快速执行而进行判断
                this._LastAppendViewContentElement_ContentVersion = this._ContentVersion;
                var indexFix = content.Count - result;
                for (var iCount = result - 1; iCount >= 0; iCount--)
                {
                    if (content.GetItemFast(iCount + indexFix) != this._Elements.GetItemFast(iCount))
                    {
                        this._LastAppendViewContentElement_ContentVersion = -1;
                        break;
                    }
                }
            }
            this.HasContentElement = result > 0;
            if (argsBack != null)
            {
                argsBack.HasFieldElements = args.HasFieldElements;
                argsBack.HasObjectElement = args.HasObjectElement;
            }
            //_AppendViewContentElement_TickCount += DateTime.Now.Ticks - tick;
            return result;
        }

        /// <summary>
        /// 返回预览对象内容的字符串
        /// </summary>
        /// <returns></returns>
        public virtual string PreviewString
        {
            get
            {
                System.Text.StringBuilder str = new System.Text.StringBuilder();
                foreach (DomElement element in this.Elements.FastForEach())
                {
                    if ((element is DomParagraphFlagElement) == false)
                    {
                        str.Append(element.ToString());
                        if (str.Length > 20)
                        {
                            break;
                        }
                    }
                }
                return "Para:" + str.ToString();
            }
        }

        /// <summary>
        /// 复制对象
        /// </summary>
        /// <param name="Deeply">是否复制子孙节点</param>
        /// <returns>复制品</returns>
        public override DomElement Clone(bool Deeply)
        {
            DomContainerElement c = (DomContainerElement)base.Clone(Deeply);
            c.ChildElementTypeStateReady = false;
            c._ChildElementsNotCharOrParagraphFlag = null;
            c._ContentVersion = 0;
            if (this._ValidateStyle != null)
            {
                c._ValidateStyle = this._ValidateStyle.Clone();
            }
            c._LastValidateResult = null;

            c._Elements = new DomElementList();
            if (Deeply)
            {
                if (this._Elements != null && this._Elements.Count > 0)
                {
                    this._Elements.FastCloneDeeplyTo(c._Elements, c, this.OwnerDocument);
                }//if

            }

            return c;
        }

        /// <summary>
        /// 销毁对象
        /// </summary>
        public override void Dispose()
        {
            base.Dispose();
            this._ID = null;
            this.ResetChildElementStats();
            //if (this._ElementsForSerialize != null)
            //{
            //    this._ElementsForSerialize.DisposeItemsAndClear();

            //    //foreach (XTextElement e in this._ElementsForSerialize.FastForEach())
            //    //{
            //    //    e.Dispose();
            //    //}
            //    //this._ElementsForSerialize.ClearData();
            //    this._ElementsForSerialize = null;
            //}
            if (this._Elements != null)
            {
                this._Elements.DisposeItemsAndClear();
                this._Elements = null;
            }
            if (this._LastValidateResult != null)
            {
                this._LastValidateResult.Clear();
                this._LastValidateResult = null;
            }
            this._ToolTip = null;
            this._ValidateStyle = null;
        }


        /// <summary>
        /// 获得文档中指定name值的元素对象,查找时name值区分大小写的。
        /// </summary>
        /// <param name="name">指定的编号</param>
        /// <returns>找到的元素对象</returns>
        public DomElementList GetElementsByName(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return null;
            }
            var result = new DomElementList();
            InnerGetElementsByName(name, result, false);
            return result;
        }

        /// <summary>
        /// 获得文档中所有指定编号的元素对象列表,查找时ID值区分大小写的。
        /// </summary>
        /// <param name="id">指定的编号</param>
        /// <returns>找到的元素对象列表</returns>
        public virtual DomElementList GetElementsById(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return null;
            }
            var result = new DomElementList();
            InnerGetElementsByName(id, result, true);
            return result;
        }

        internal void InnerGetElementsByName(string name, DomElementList list, bool forID)
        {
            this.CheckChildElementStatsReady();
            var arr = this._ChildElementsNotCharOrParagraphFlag;
            if (arr != null)
            {
                var len = arr.Count;
                for (var iCount = 0; iCount < len; iCount++)
                {
                    var element = arr[iCount];
                    if (forID)
                    {
                        if (element.ID == name)
                        {
                            list.AddRaw(element);
                        }
                    }
                    if (element is DomContainerElement c2)
                    {
                        if (c2.HasElements())
                        {
                            c2.InnerGetElementsByName(name, list, forID);
                        }
                        if (element is DomInputFieldElementBase)
                        {
                            if (((DomInputFieldElementBase)element).Name == name)
                            {
                                list.AddRaw(element);
                            }
                        }
                    }
                    else if (element is DomObjectElement)
                    {
                        if (((DomObjectElement)element).Name == name)
                        {
                            list.AddRaw(element);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 获得具有指定样式编号的文档对象。
        /// </summary>
        /// <param name="styleIndex">样式编号</param>
        /// <returns>找到的文档元素列表</returns>
        public virtual DomElementList GetElementsByStyleIndex(int styleIndex)
        {
            DomElementList list = new DomElementList();
            DomTreeNodeEnumerable enumer = new DomTreeNodeEnumerable(this);
            enumer.ExcludeParagraphFlag = false;
            enumer.ExcludeCharElement = false;
            foreach (DomElement element in enumer)
            {
                if (element.StyleIndex == styleIndex)
                {
                    list.AddRaw(element);
                }
            }
            return list;
        }

        /// <summary>
        /// 根据对象哈希值获得文档元素对象
        /// </summary>
        /// <param name="hashCode">哈希值</param>
        /// <returns>文档元素对象</returns>
        public virtual DomElement GetElementByHashCode(int hashCode)
        {
            var list = this._Elements;
            if (list != null && list.Count > 0)
            {
                this.CheckChildElementStatsReady();
                if (this._ChildElementsNotCharOrParagraphFlag != null)
                {
                    var arr = this._ChildElementsNotCharOrParagraphFlag;
                    for (var iCount = arr.Count - 1; iCount >= 0; iCount--)
                    {
                        if (arr[iCount].GetHashCode() == hashCode)
                        {
                            return arr[iCount];
                        }
                        else if (arr[iCount] is DomContainerElement c3)
                        {
                            var result = c3.GetElementByHashCode(hashCode);
                            if (result != null)
                            {
                                return result;
                            }
                        }
                    }
                }
            }
            return null;
        }



        /// <summary>
        /// 获得文档中指定编号的元素对象,查找时ID值区分大小写的。
        /// </summary>
        /// <param name="id">指定的编号</param>
        /// <returns>找到的元素对象</returns>
        public virtual DomElement GetElementById(string id)
        {
            return GetElementByIdExt(id, false);
        }

        /// <summary>
        /// 获得文档中指定编号的元素对象,查找时ID值区分大小写的。
        /// </summary>
        /// <param name="id">指定的编号</param>
        /// <param name="idAttributeOnly">只匹配元素ID属性</param>
        /// <returns>找到的元素对象</returns>
        public virtual DomElement GetElementByIdExt(string id, bool idAttributeOnly)
        {
            return InnerGetElementByIdExt(id, idAttributeOnly);
        }

        /// <summary>
        /// 获得文档中指定编号的元素对象,查找时ID值区分大小写的。
        /// </summary>
        /// <param name="id">指定的编号</param>
        /// <param name="idAttributeOnly">只匹配元素ID属性</param>
        /// <returns>找到的元素对象</returns>
        internal DomElement InnerGetElementByIdExt(string id, bool idAttributeOnly)
        {
            if (string.IsNullOrEmpty(id))// == null || id.Length == 0)
            {
                return null;
            }
            DomElement nameElement = null;
            var idElement = InnerGetElementByIdExt_Enum(this, id, idAttributeOnly, ref nameElement);
            if (idElement != null)
            {
                return idElement;
            }
            else
            {
                return nameElement;
            }
        }

        private static DomElement InnerGetElementByIdExt_Enum(
            DomContainerElement rootElement,
            string id, bool idAttributeOnly,
            ref DomElement nameElement)
        {
            rootElement.CheckChildElementStatsReady();
            if (rootElement._ChildElementsNotCharOrParagraphFlag != null)
            {
                var ceArr = rootElement._ChildElementsNotCharOrParagraphFlag;
                var len = ceArr.Count;
                if (idAttributeOnly)
                {
                    for (var iCount = 0; iCount < len; iCount++)
                    {
                        var element = ceArr[iCount];
                        if (element.ID == id)
                        {
                            return element;
                        }
                        if (element is DomContainerElement)
                        {
                            var e2 = InnerGetElementByIdExt_Enum(
                                (DomContainerElement)element,
                                id,
                                idAttributeOnly,
                                ref nameElement);
                            if (e2 != null)
                            {
                                return e2;
                            }
                        }
                    }
                }
                else
                {
                    for (var iCount = 0; iCount < len; iCount++)
                    {
                        var element = ceArr[iCount];
                        if (element.ID == id)
                        {
                            return element;
                        }
                        else if (element is DomInputFieldElementBase && nameElement == null)
                        {
                            DomInputFieldElementBase field = (DomInputFieldElementBase)element;
                            if (field.Name == id)
                            {
                                nameElement = field;
                            }
                        }
                        else if (element is DomCheckBoxElementBase && nameElement == null)
                        {
                            DomCheckBoxElementBase chk = (DomCheckBoxElementBase)element;
                            if (chk.Name == id)
                            {
                                nameElement = chk;
                            }
                        }
                        if (element is DomContainerElement)
                        {
                            var e2 = InnerGetElementByIdExt_Enum(
                                (DomContainerElement)element,
                                id,
                                idAttributeOnly,
                                ref nameElement);
                            if (e2 != null)
                            {
                                return e2;
                            }
                        }
                    }
                }
            }
            return null;
        }


        /// <summary>
        /// 获得子孙元素中最后一个指定类型的文档元素对象
        /// </summary>
        /// <param name="elementType">元素类型</param>
        /// <returns>获得的文档元素对象</returns>
        public DomElement GetLastElementByType(Type elementType)
        {
            if (elementType == null)
            {
                throw new ArgumentNullException("elementType");
            }
            if (typeof(DomElement).IsAssignableFrom(elementType) == false)
            {
                throw new InvalidCastException(elementType.FullName);
            }
            bool excludeCharElement = elementType.Equals(typeof(DomCharElement));
            for (int iCount = this.Elements.Count - 1; iCount >= 0; iCount--)
            {
                DomElement element = this.Elements[iCount];
                if (excludeCharElement)
                {
                    // 忽略字符元素
                    if (element is DomCharElement)
                    {
                        continue;
                    }
                }
                if (elementType.IsInstanceOfType(element))
                {
                    return element;
                }
                if (element is DomContainerElement)
                {
                    DomContainerElement c = (DomContainerElement)element;
                    DomElement result = c.GetLastElementByType(elementType);
                    if (result != null)
                    {
                        return result;
                    }
                }
            }//foreach
            return null;
        }

        public ElementType GetFirstChild<ElementType>() where ElementType : DomElement
        {
            return this.Elements?.GetFirstElement<ElementType>();
        }
        public virtual DomElementList GetElementsByType<ElementType>()
        {
            static void InnerGetElementsByTypeWithRawDOM<ElementType>(DomContainerElement rootElement, DomElementList list)
            {
                if (rootElement.HasElements())
                {
                    var es = rootElement.Elements;
                    var esCount = es.Count;
                    for (var iCount = 0; iCount < esCount; iCount++)
                    {
                        var element = es[iCount];
                        if (element is ElementType)
                        {
                            list.FastAdd2(element);
                        }
                        if (element is DomContainerElement c)
                        {
                            if (c.HasElements())
                            {
                                InnerGetElementsByTypeWithRawDOM<ElementType>(c, list);
                            }
                        }
                    }
                }
            }
            static void InnerGetElementsByTypeWithCompressDOM<ElementType>(DomContainerElement rootElement, DomElementList list)
            {
                rootElement.CheckChildElementStatsReady();
                if (rootElement._ChildElementsNotCharOrParagraphFlag != null)
                {
                    var es = rootElement._ChildElementsNotCharOrParagraphFlag;
                    var esCount = es.Count;
                    for (var iCount = 0; iCount < esCount; iCount++)
                    {
                        var element = es[iCount];
                        if (element is ElementType)
                        {
                            list.FastAdd2(element);
                        }
                        if (element is DomContainerElement c)
                        {
                            if (c.HasElements())
                            {
                                InnerGetElementsByTypeWithCompressDOM<ElementType>(c, list);
                            }
                        }
                    }
                }
            }

            var list = new DomElementList();
            if (this is ElementType)
            {
                list.Add(this);
            }
            if (this.HasElements())
            {
                if (typeof(ElementType) == typeof(DomCharElement) || typeof(ElementType) == typeof(DomParagraphFlagElement))
                {
                    // 查找字符获得段落符号元素类型
                    InnerGetElementsByTypeWithRawDOM<ElementType>(this, list);
                }
                else
                {
                    InnerGetElementsByTypeWithCompressDOM<ElementType>(this, list);
                }
            }
            return list;
        }

        /// <summary>
        /// 获得本文档元素容器包含的所有的指定类型的文档元素列表
        /// </summary>
        /// <param name="elementType">元素类型</param>
        /// <returns>获得的元素列表</returns>
        public virtual DomElementList GetElementsByType(Type elementType)
        {
            static void InnerGetElementsByTypeWithCompressDOM2(DomContainerElement rootElement, DomElementList list, Type elementType)
            {
                rootElement.CheckChildElementStatsReady();
                if (rootElement._ChildElementsNotCharOrParagraphFlag != null)
                {
                    var es = rootElement._ChildElementsNotCharOrParagraphFlag;
                    var esCount = es.Count;
                    for (var iCount = 0; iCount < esCount; iCount++)
                    {
                        var element = es[iCount];
                        if (elementType.IsInstanceOfType(elementType))
                        {
                            list.FastAdd2(element);
                        }
                        if (element is DomContainerElement c)
                        {
                            if (c.HasElements())
                            {
                                InnerGetElementsByTypeWithCompressDOM2(c, list, elementType);
                            }
                        }
                    }
                }
            }
            static void InnerGetElementsByTypeWithRawDOM2(DomContainerElement rootElement, DomElementList list, Type elementType)
            {
                if (rootElement.HasElements())
                {
                    var es = rootElement.Elements;
                    var esCount = es.Count;
                    for (var iCount = 0; iCount < esCount; iCount++)
                    {
                        var element = es[iCount];
                        if (elementType.IsInstanceOfType(element))
                        {
                            list.FastAdd2(element);
                        }
                        if (element is DomContainerElement c)
                        {
                            if (c.HasElements())
                            {
                                InnerGetElementsByTypeWithRawDOM2(c, list, elementType);
                            }
                        }
                    }
                }
            }

            if (elementType == null)
            {
                throw new ArgumentNullException("elementType");
            }
            if (typeof(DomElement).IsAssignableFrom(elementType) == false)
            {
                throw new InvalidCastException(elementType.FullName);
            }
            var list = new DomElementList();
            if (elementType.IsInstanceOfType(this))
            {
                list.Add(this);
            }
            if (this.HasElements())
            {
                if (typeof(DomCharElement).IsAssignableFrom(elementType)
                    || typeof(DomParagraphFlagElement) == elementType)
                {
                    // 查找字符获得段落符号元素类型
                    InnerGetElementsByTypeWithRawDOM2(this, list, elementType);
                }
                else
                {
                    InnerGetElementsByTypeWithCompressDOM2(this, list, elementType);
                }
            }
            return list;
        }
        /// <summary>
        /// 获得指定类型的子孙元素，不包括当前元素自己
        /// </summary>
        /// <typeparam name="ET"></typeparam>
        /// <returns></returns>
        public virtual DomElementList GetElementsBySpecifyType<ET>() where ET : DomElement
        {
            var result = new DomElementList();
            this.InnerGetElementsBySpecifyType<ET>(result);
            return result;
        }

        protected virtual void InnerGetElementsBySpecifyType<ET>(DomElementList list) where ET : DomElement
        {
            if (this._Elements != null && this._Elements.Count > 0)
            {
                var len = this._Elements.Count;
                var arr = this._Elements.InnerGetArrayRaw();
                for (int iCount = 0; iCount < len; iCount++)
                {
                    var element = arr[iCount];
                    if (element is ET)
                    {
                        list.FastAdd2(element);
                    }
                    if (element is DomContainerElement)
                    {
                        ((DomContainerElement)element).InnerGetElementsBySpecifyType<ET>(list);
                    }
                }
            }
            //else if (this._ElementsForSerialize != null && this._ElementsForSerialize.Count > 0)
            //{
            //    var len = this._ElementsForSerialize.Count;
            //    for (int iCount = 0; iCount < len; iCount++)
            //    {
            //        var element = this._ElementsForSerialize[iCount];
            //        if (element is ET)
            //        {
            //            list.FastAdd2(element);
            //        }
            //        if (element is XTextContainerElement)
            //        {
            //            ((XTextContainerElement)element).InnerGetElementsBySpecifyType<ET>(list);
            //        }
            //    }
            //}

        }
        /// <summary>
        /// 获得本文档元素容器包含的指定类型的第一个文档元素
        /// </summary>
        /// <param name="elementType">元素类型</param>
        /// <returns>获得的元素</returns>
        public virtual DomElement GetElementByType(Type elementType)
        {
            if (elementType == null)
            {
                throw new ArgumentNullException("elementType");
            }
            if (typeof(DomElement).IsAssignableFrom(elementType) == false)
            {
                throw new InvalidCastException(elementType.FullName);
            }
            return Container_GetElementByType(elementType);
        }

        private DomElement Container_GetElementByType(Type elementType)
        {
            foreach (DomElement element in this.Elements.FastForEach())
            {
                if (elementType.IsInstanceOfType(element))
                {
                    return element;
                }
                if (element is DomContainerElement)
                {
                    DomContainerElement c = (DomContainerElement)element;
                    DomElement result = c.Container_GetElementByType(elementType);
                    if (result != null)
                    {
                        return result;
                    }
                }
            }
            return null;
        }


        /// <summary>
        /// 计算元素大小
        /// </summary>
        /// <param name="args">参数</param>
        public override void RefreshSize(InnerDocumentPaintEventArgs args)
        {
            //var tick = DateTime.Now.Ticks;
            if (this._Elements != null && base._OwnerDocument != null)
            {
                int eCount = this._Elements.Count;
                var eArray = this._Elements.InnerGetArrayRaw();
                if (args.IsLoadingDocument)
                {
                    for (int iCount = 0; iCount < eCount; iCount++)
                    {
                        eArray[iCount].RefreshSize(args);
                    }
                    this.SizeInvalid = false;
                    //_RefreshSizeTickCount += DateTime.Now.Ticks - tick;
                    return;
                }
                if (args._CheckSizeInvalidateWhenRefreshSize)
                {
                    for (int iCount = 0; iCount < eCount; iCount++)
                    {
                        var e = eArray[iCount];
                        if (e.SizeInvalid)
                        {
                            e.RefreshSize(args);
                        }
                    }
                }
                else
                {
                    for (int iCount = 0; iCount < eCount; iCount++)
                    {
                        var e = eArray[iCount];
                        e.RefreshSize(args);
                    }
                }
            }
            this.SizeInvalid = false;
            // _RefreshSizeTickCount += DateTime.Now.Ticks - tick;
        }

        #region 事件处理 ************************************************



        /// <summary>
        /// 以冒泡方式出发文档容器元素的OnContentChanging事件
        /// </summary>
        /// <param name="args">事件参数</param>

        public void RaiseBubbleOnContentChanging(ContentChangingEventArgs args)
        {
            DomContainerElement parent = this;
            while (parent != null)
            {
                parent.OnContentChanging(args);
                if (args.CancelBubble)
                {
                    break;
                }
                parent = parent.Parent;
            }
        }

        /// <summary>
        /// 触发内容正在改变事件
        /// </summary>
        /// <param name="args">参数</param>

        public virtual void OnContentChanging(ContentChangingEventArgs args)
        {
            if (this.OwnerDocument.EnableContentChangedEvent()
                && GetDocumentBehaviorOptions().EnableElementEvents)
            {
            }
        }

        /// <summary>
        /// 以冒泡方式出发文档容器元素的OnContentChanged事件
        /// </summary>
        /// <param name="args">事件参数</param>

        public virtual void RaiseBubbleOnContentChanged(ContentChangedEventArgs args)
        {
            DomContainerElement parent = this;
            while (parent != null)
            {
                parent.OnContentChanged(args);
                if (args.CancelBubble)
                {
                    break;
                }
                if (args.EventSource == ContentChangedEventSource.ValueEditor)
                {
                    args.EventSource = ContentChangedEventSource.Default;
                }
                parent = parent.Parent;
            }
        }

        /// <summary>
        /// 容器元素失去焦点事件
        /// </summary>
        /// <param name="args">参数</param>

        public override void OnLostFocus(ElementEventArgs args)
        {
            if (GetDocumentEditOptions().ValueValidateMode
                == DocumentValueValidateMode.LostFocus)
            {
                this.Validating(false);
            }
            base.OnLostFocus(args);
        }
        /// <summary>
        /// 触发内容已经改变事件
        /// </summary>
        /// <param name="args">参数</param>

        public virtual void OnContentChanged(ContentChangedEventArgs args)
        {
            DomDocument doc = this.OwnerDocument;
            bool loadingDocument = args.LoadingDocument;
            if (loadingDocument == false)
            {
                // 不是在加载文档时触发的事件，设置Modified属性
                this.Modified = true;
            }
            //if (doc.EnableContentChangedEvent() && doc.GetDocumentBehaviorOptions().EnableElementEvents)
            //{
            //}

            OnContentChanged_Validate(args);

        }

        internal bool FastIsFocused(DomDocumentContentElement dce, DomElement currentElement)
        {
            if (dce == this)
            {
                // 文档级容器对象一直是拥有焦点的
                return true;
            }
            if (currentElement == null)
            {
                return false;
            }
            DomElement element = currentElement;
            if (this is DomContentElement)
            {
                return element.IsParentOrSupParent(this);
            }
            DomElement fcep = this.FirstContentElementInPublicContent;
            while (element != null)
            {
                if (fcep != element)// this.FirstContentElementInPublicContent != element)
                {
                    if (element == this)
                    {
                        if (this is DomFieldElementBase)
                        {
                            DomFieldElementBase field = (DomFieldElementBase)this;
                            if (currentElement == field._StartElement)
                            {
                                element = element.Parent;
                                continue;
                            }
                        }
                        return true;
                    }
                }
                element = element.Parent;
            }//while
            return false;
        }


        /// <summary>
        /// 判断元素是否获得输入焦点
        /// </summary>
        public override bool Focused
        {
            get
            {
                DomDocument doc = this._OwnerDocument;
                if (doc == null)
                {
                    return false;
                }
                var p = this;
                while (p != null)
                {
                    if (p is DomDocumentContentElement)
                    {
                        var dce = (DomDocumentContentElement)p;
                        return FastIsFocused(dce, dce.CurrentElement);
                    }
                    p = p._Parent;
                }
                return false;

                //XTextDocumentContentElement dce = this.DocumentContentElement;
                //if (dce == null)
                //{
                //    return false;
                //}
                //return FastIsFocused(dce , dce.CurrentElement );
                //if (doc.CurrentContentElement == dce)
                //{
                //    if (dce == this)
                //    {
                //        // 文档级容器对象一直是拥有焦点的
                //        return true;
                //    }
                //    XTextElement curElement = dce.CurrentElement;
                //    if( curElement == null )
                //    {
                //        return false;
                //    }
                //    XTextElement element = curElement;
                //    if (this is XTextContentElement)
                //    {
                //        return element.IsParentOrSupParent(this);
                //    }
                //    XTextElement fcep = this.FirstContentElementInPublicContent;
                //    while (element != null)
                //    {
                //        if (fcep != element)// this.FirstContentElementInPublicContent != element)
                //        {
                //            if (element == this)
                //            {
                //                if (this is XTextFieldElementBase)
                //                {
                //                    XTextFieldElementBase field = (XTextFieldElementBase)this;
                //                    if (curElement == field.StartElement)
                //                    {
                //                        element = element.Parent;
                //                        continue;
                //                    }
                //                }
                //                return true;
                //            }
                //        }
                //        element = element.Parent;
                //    }//while
                //}
                //return false;
            }
        }

        /// <summary>
        /// 获得输入焦点
        /// </summary>
        public override void Focus()
        {
            if (this.BelongToDocumentDom(this.OwnerDocument) == false)
            {
                return;
            }
            DomElement firstElement = this.FirstContentElement;
            if (firstElement != null)
            {
                DomDocumentContentElement dce = this.DocumentContentElement;
                if (dce == null)
                {
                    return;
                }
                if (this.OwnerDocument.CurrentContentElement != dce)
                {
                    dce.Focus();
                }
                if (dce.CurrentElement == null
                    || dce.CurrentElement.IsParentOrSupParent(this) == false)
                {
                    int index = firstElement.ContentIndex;
                    dce.SetSelection(firstElement.ContentIndex, 0);
                    if (index != firstElement.ContentIndex)
                    {
                        // 这个过程中发生了文档内容改变，再次设置焦点
                        dce.SetSelection(firstElement.ContentIndex, 0);
                    }
                }
                var ctl = this.InnerViewControl;
            }
        }

        /// <summary>
        /// 设置文档元素样式
        /// </summary>
        /// <param name="style">文档样式</param>
        /// <returns>操作是否成功</returns>
        public virtual bool EditorSetStyleDeeply(DocumentContentStyle style)
        {
            if (this.OwnerDocument == null)
            {
                throw new NullReferenceException(DCSR.NeedSetOwnerDocument);
            }
            if (style == null)
            {
                throw new ArgumentNullException("style");
            }
            if (base.EditorSetStyle(style))
            {
                DomTreeNodeEnumerable enumer = new DomTreeNodeEnumerable(this);
                enumer.ExcludeParagraphFlag = false;
                enumer.ExcludeCharElement = false;
                foreach (DomElement element in enumer)
                {
                    element.StyleIndex = this.StyleIndex;
                }
                return true;
            }
            return true;
        }

        public override void InnerGetText(GetTextArgs args)
        {
            if (args.IncludeHiddenText || this.RuntimeVisible)
            {
                foreach (DomElement element in this.Elements.FastForEach())
                {
                    if (args.IncludeHiddenText || element.RuntimeVisible)
                    {
                            element.InnerGetText(args);
                    }
                }//foreach
            }
        }

        public override void WriteText(WriteTextArgs args)
        {
            if (this._Elements == null || this._Elements.Count == 0)
            {
                return;
            }
            if (args.IncludeHiddenText || this.RuntimeVisible)
            {
                var len = this._Elements.Count;
                var eArray = this._Elements.InnerGetArrayRaw();
                if (args.IncludeHiddenText == false)
                {
                    for (var iCount = 0; iCount < len; iCount++)
                    {
                        var element = eArray[iCount];
                        if (element.RuntimeVisible)
                        {
                                element.WriteText(args);
                        }
                    }//foreach
                    return;
                }
                //foreach (XTextElement element in this.Elements.FastForEach())
                for (var iCount = 0; iCount < len; iCount++)
                {
                    var element = eArray[iCount];
                    if (args.IncludeHiddenText || element.RuntimeVisible)
                    {
                            element.WriteText(args);
                    }
                }//foreach
            }
            else
            {
                var len = this._Elements.Count;
                var eArray = this._Elements.InnerGetArrayRaw();
                //foreach (XTextElement element in this.Elements.FastForEach())
                for (var iCount = 0; iCount < len; iCount++)
                {
                    var element = eArray[iCount];
                    element.WriteText(args);
                    //str.Append(element.OuterText);
                }//foreach
            }
        }

        /// <summary>
        /// 返回表示对象的文本
        /// </summary>
        public override string OuterText
        {
            //[JSInvokable]
            get
            {
                var args = new WriteTextArgs(this._OwnerDocument, true);
                this.WriteText(args);
                return args.ToString();
            }
            //[JSInvokable]
            set
            {
                this.Text = value;
            }
        }

        /// <summary>
        /// 返回表示对象的文本
        /// </summary>
        public override string InnerText
        {
            //[JSInvokable]
            get
            {
                var args = new WriteTextArgs(this._OwnerDocument, false);
                this.WriteText(args);
                return args.ToString();
            }
            //[JSInvokable]
            set
            {
                this.Text = value;
            }
        }

        /// <summary>
        /// 返回文本内容，不包含被逻辑删除的部分。
        /// </summary>
        public override string Text
        {
            //[JSInvokable]
            get
            {
                if (this._Elements == null || this._Elements.Count == 0)
                {
                    return string.Empty;
                }
                var args = new WriteTextArgs(this._OwnerDocument, false, false, this._Elements.Count);
                WriteText(args);
                return args.ToString();
            }
            //[JSInvokable]
            set
            {
                //if (string.IsNullOrEmpty(this.Text) == true &&
                //    string.IsNullOrEmpty(value) == true)
                var oldText = this.Text;
                if ((oldText == null || oldText.Length == 0) && (value == null || value.Length == 0))
                {
                    // 文本值都是空，不设置。
                    return;
                }
                if (oldText == value)
                {
                    // 文本值一样，不设置
                    return;
                }
                if (this.Parent == null)
                {
                    this.SetInnerTextFast(value);
                    return;
                }
                var document = this.OwnerDocument;
                if (document != null
                    && document.ReadyState != DomReadyStates.Complete
                    && document.PageRefreshed == false)
                {
                    this.SetInnerTextFast(value);
                    return;
                }
                if (string.IsNullOrEmpty(value))
                {
                    ReplaceElementsArgs args = new ReplaceElementsArgs(
                            this,
                            0,
                            this.Elements.Count,
                            null,
                            true,
                            true,
                            true);
                    args.AccessFlags = DomAccessFlags.None;
                    args.ChangeSelection = false;
                    document.ReplaceElements(args);
                }
                else
                {
                    DocumentContentStyle pStyle = null;
                    if (this.HasElements())
                    {
                        foreach (DomElement element in this.Elements)
                        {
                            if (element is DomParagraphFlagElement)
                            {
                                pStyle = element.Style;
                                break;
                            }
                        }
                    }
                    if (pStyle == null && this.OwnerParagraphEOF != null)
                    {
                        pStyle = this.OwnerParagraphEOF.Style;
                    }
                    DomElementList list = document.CreateTextElements(
                        value,
                        pStyle,
                        this.RuntimeStyle.CloneWithoutBorder());
                    if (list != null && list.Count > 0)
                    {
                        ReplaceElementsArgs args = new ReplaceElementsArgs(
                            this,
                            0,
                            this.Elements.Count,
                            list,
                            true,
                            true,
                            true);
                        args.AccessFlags = DomAccessFlags.None;
                        args.ChangeSelection = false;
                        document.ReplaceElements(args);
                    }
                }
            }
        }

        /// <summary>
        /// 在编辑器中设置/获得对象文本值,这个操作会被系统记录，能进行重复和撤销操作,而且不受用户界面层只读的限制。
        /// </summary>
        public string EditorTextExt
        {
            //[JSInvokable]
            get
            {
                return this.Text;
            }
            //[JSInvokable]
            set
            {
                SetEditorTextExt(value, DomAccessFlags.None, true);
            }
        }

        /// <summary>
        /// 设置文本
        /// </summary>
        /// <param name="newText">新文本</param>
        /// <param name="flags">标记</param>
        /// <param name="updateContent">是否更新文档内容</param>
        /// <returns>操作是否修改了对象内容</returns>
        public bool SetEditorTextExt(
            string newText,
            DomAccessFlags flags,
            bool updateContent)
        {
            SetContainerTextArgs args = new SetContainerTextArgs();
            args.NewText = newText;
            args.AccessFlags = flags;
            args.UpdateContent = updateContent;
            return SetEditorText(args);
        }



        /// <summary>
        /// 设置文本
        /// </summary>
        /// <param name="args">参数</param>
        /// <returns>操作是否修改了对象内容</returns>
        public virtual bool SetEditorText(SetContainerTextArgs args)
        {
            if (args == null)
            {
                throw new ArgumentNullException("args");
            }
            bool result = false;
            DomDocument document = this.OwnerDocument;
            if (this.BelongToDocumentDom(document))
            {
                // 处于DOM结构中
                //lock (document)
                {
                    string oldText = this.Text;
                    if (oldText == null || oldText != args.NewText)
                    {
                        // 为了提高效率，只有出现不同的文本才进行文本替换操作
                        bool innerLogUndo = document.CanLogUndo;
                        if (args.LogUndo)
                        {
                            if (innerLogUndo == false)
                            {
                                document.BeginLogUndo();
                            }
                        }
                        if (string.IsNullOrEmpty(args.NewText))
                        {
                            ReplaceElementsArgs args2 = new ReplaceElementsArgs(
                                    this,
                                    0,
                                    this.Elements.Count,
                                    null,
                                    true,
                                    true,
                                    true);
                            args2.EventSource = args.EventSource;
                            if (this is DomContentElement && args2.DeleteLength == this.Elements.Count)
                            {
                                if (this.Elements.LastElement is DomParagraphFlagElement)
                                {
                                    // 如果当前元素是文档块元素，则最后一个段落符号是不能删除的
                                    // 则对要删除的区域的长度进行修正
                                    args2.DeleteLength = this.Elements.Count - 1;
                                }
                            }
                            args2.PreserveSelection = true;
                            args2.FocusContainer = args.FocusContainer;
                            args2.AccessFlags = args.AccessFlags;
                            args2.ChangeSelection = false;
                            args2.UpdateContent = args.UpdateContent;
                            args2.LogUndo = args.LogUndo;
                            args2.RaiseEvent = args.RaiseEvent;
                            args2.ChangeSelection = false;
                            result = document.ReplaceElements(args2) != 0;
                        }
                        else
                        {
                            // 设置文本内容
                            DocumentContentStyle newTextStyle = args.NewTextStyle;
                            if (newTextStyle == null)
                            {
                                if (this.Elements.Count > 0)
                                {
                                    foreach (var e in this.Elements)
                                    {
                                        if (e.RuntimeVisible && e is DomCharElement)
                                        {
                                            newTextStyle = e.RuntimeStyle.CloneWithoutBorder();
                                            break;
                                        }
                                    }
                                    if (newTextStyle == null)
                                    {
                                        foreach (var e in this.Elements)
                                        {
                                            if (e.RuntimeVisible)
                                            {
                                                newTextStyle = e.RuntimeStyle.CloneWithoutBorder();
                                                break;
                                            }
                                        }
                                    }
                                }//if
                                if (newTextStyle == null && this.FirstContentElementInPublicContent != null)
                                {
                                    newTextStyle = this.FirstContentElementInPublicContent.RuntimeStyle.CloneWithoutBorder();
                                }
                                if (newTextStyle == null)
                                {
                                    newTextStyle = this.RuntimeStyle.CloneWithoutBorder();
                                }
                            }
                            DocumentContentStyle newParagraphStyle = args.NewParagraphStyle;
                            if (newParagraphStyle == null)
                            {
                                DomElement fe = this.FirstContentElement;
                                if (fe != null && fe.OwnerParagraphEOF != null)
                                {
                                    newParagraphStyle = fe.OwnerParagraphEOF.RuntimeStyle.Parent;
                                }
                                if (newParagraphStyle == null)
                                {
                                    newParagraphStyle = document.CurrentStyleInfo.Paragraph;
                                }
                            }
                            newParagraphStyle = (DocumentContentStyle)newParagraphStyle.Clone();
                            newParagraphStyle.ValueLocked = false;
                            DomElementList list = document.CreateTextElementsExt(
                                args.NewText,
                                newParagraphStyle,
                                newTextStyle);
                            if (list != null && list.Count > 0)
                            {
                                ReplaceElementsArgs args2 = new ReplaceElementsArgs(
                                    this,
                                    0,
                                    this.Elements.Count,
                                    list,
                                    true,
                                    true,
                                    true);
                                args2.PreserveSelection = true;
                                args2.FocusContainer = args.FocusContainer;
                                args2.AccessFlags = args.AccessFlags;
                                args2.ChangeSelection = false;
                                args2.UpdateContent = args.UpdateContent;
                                args2.LogUndo = args.LogUndo;
                                args2.RaiseEvent = args.RaiseEvent;
                                args2.EventSource = args.EventSource;
                                args2.ChangeSelection = false;
                                args2.SetNewInnerValue = args.SetNewInnerValue;
                                args2.NewInnerValue = args.NewInnerValue;
                                if (this is DomContentElement && args2.DeleteLength == this.Elements.Count)
                                {
                                    if (this.Elements.LastElement is DomParagraphFlagElement)
                                    {
                                        // 如果当前元素是文档块元素，则最后一个段落符号是不能删除的
                                        // 则对要删除的区域的长度进行修正
                                        args2.DeleteLength = this.Elements.Count - 1;
                                    }
                                }
                                DomDocumentContentElement._InsertOrDeleteTextOnlyFlag = true;
                                result = document.ReplaceElements(args2) != 0;
                            }
                        }

                        if (args.LogUndo)
                        {
                            if (innerLogUndo == false)
                            {
                                if (result)
                                {
                                    document.EndLogUndo();
                                }
                                else
                                {
                                    document.CancelLogUndo();
                                }
                            }
                        }
                        if (result)
                        {
                            if (args.RaiseEvent)
                            {
                                if (args.RaiseDocumentContentChangedEvent)
                                {
                                    document.OnDocumentContentChanged();
                                }
                                document.OnSelectionChanged();
                            }
                        }
                    }
                }//lock
            }
            else
            {
                //var ss = this._Elements != null && this._Elements.Count == 2 && this._Elements[0] == this._Elements[1];
                // 不处于DOM结构中，快速设置文本
                DomElementList list = this.SetInnerTextFast(args.NewText);
                result = list != null && list.Count > 0;
                if (document != null && document.IsLoadingDocument == false)
                {
                    ContentChangedEventArgs args3 = new ContentChangedEventArgs();
                    args3.Document = document;
                    args3.Element = this;
                    this.RaiseBubbleOnContentChanged(args3);
                }
            }
            if (this.OwnerDocument != null )
            {
                if (args.ShowUI)
                {
                    document.PromptProtectedContent(null);
                }
                document.ClearContentProtectedInfos();
            }
            return result;
        }

        /// <summary>
        /// 快速设置元素的文本内容
        /// </summary>
        /// <param name="txt">文本</param>
        /// <returns>创建的元素对象列表</returns>
        public DomElementList SetInnerTextFast(string txt)
        {
            return SetInnerTextFastExt(txt, null);
        }
        /// <summary>
        /// 快速设置元素的文本内容
        /// </summary>
        /// <param name="txt">文本</param>
        /// <param name="newTextStyle">文本样式</param>
        /// <returns>创建的元素对象列表</returns>
        public virtual DomElementList SetInnerTextFastExt(string txt, DocumentContentStyle newTextStyle)
        {
            return SetInnerTextFastExt(txt, newTextStyle, true);
        }

        /// <summary>
        /// 快速设置元素的文本内容
        /// </summary>
        /// <param name="txt">文本</param>
        /// <param name="newTextStyle">文本样式</param>
        /// <param name="updateContentElements">是否更新容器元素的状态,如果为false，则本函数能更快的执行。</param>
        /// <returns>创建的元素对象列表</returns>
        public virtual DomElementList SetInnerTextFastExt(
            string txt,
            DocumentContentStyle newTextStyle,
            bool updateContentElements)
        {
            return XTextContainerElement_SetInnerTextFastExt(txt, newTextStyle, updateContentElements);
        }

        /// <summary>
        /// 快速设置元素的文本内容
        /// </summary>
        /// <param name="txt">文本</param>
        /// <param name="newTextStyle">文本样式</param>
        /// <param name="updateContentElements">是否更新容器元素的状态,如果为false，则本函数能更快的执行。</param>
        /// <returns>创建的元素对象列表</returns>
        protected DomElementList XTextContainerElement_SetInnerTextFastExt(
            string txt,
            DocumentContentStyle newTextStyle,
            bool updateContentElements)
        {
            if (this._Elements == null)
            {
                this._Elements = new DomElementList();
            }
            DocumentContentStyle paragraphStyle = null;
            DomParagraphFlagElement ownerParagrahFlag = null;
            if (txt != null && txt.IndexOf('\r') >= 0)
            {
                ownerParagrahFlag = this.LastContentElementInPublicContent?.OwnerParagraphEOF;
                if (ownerParagrahFlag != null && ownerParagrahFlag.RuntimeStyle != null)
                {
                    paragraphStyle = (DocumentContentStyle)ownerParagrahFlag.RuntimeStyle.CloneParent();
                }
            }
            DomElement preservedElement = null;
            if (this is DomContentElement)
            {
                if (this._Elements.LastElement is DomParagraphFlagElement)
                {
                    preservedElement = this._Elements.LastElement;
                }
            }
            if (this._OwnerDocument == null)
            {
                // 没有设置所属文档，凭空创建字符对象
                DomElementList list = new DomElementList();
                if (txt != null && txt.Length > 0)// string.IsNullOrEmpty(txt) == false)
                {
                    int newStyleIndex = this.StyleIndex;
                    if (this._Elements.Count > 0)
                    {
                        foreach (DomElement item in this._Elements.FastForEach())
                        {
                            if (item is DomCharElement
                                || item is DomTextElement)
                            {
                                newStyleIndex = item.StyleIndex;
                                break;
                            }
                        }
                    }
                    foreach (char c in txt)
                    {
                        if (c == '\r')
                        {
                            DomParagraphFlagElement pe = new DomParagraphFlagElement();
                            if (ownerParagrahFlag != null)
                            {
                                pe.StyleIndex = ownerParagrahFlag.StyleIndex;
                            }
                            list.Add(pe);
                        }
                        else if (c == '\n')
                        {
                        }
                        else
                        {
                            DomCharElement chr = new DomCharElement(c, this, this._OwnerDocument, newStyleIndex);
                            //chr._StyleIndex = newStyleIndex;
                            list.Add(chr);
                        }
                    }
                }
                if (list.Count > 0 || this._Elements.Count > 0)
                {
                    this._Elements = new DomElementList();
                    this._Elements.AddRangeByDCList(list);
                    if (preservedElement != null)
                    {
                        this._Elements.Add(preservedElement);
                    }
                    this._ContentVersion++;
                }
                return list;
            }
            else
            {
                var document = this._OwnerDocument;
                if (newTextStyle == null)
                {
                    if (this._Elements.Count > 0)
                    {
                        foreach (DomElement item in this._Elements.FastForEach())
                        {
                            if (item is DomCharElement
                                || item is DomTextElement)
                            {
                                newTextStyle = (DocumentContentStyle)document.ContentStyles.GetStyle(item.StyleIndex);
                                break;
                            }
                        }
                        if (newTextStyle == null)
                        {
                            newTextStyle = (DocumentContentStyle)document.ContentStyles.GetStyle(this._Elements[0].StyleIndex);
                        }
                    }
                    else
                    {
                        newTextStyle = this.RuntimeStyle.CloneWithoutBorder();
                    }
                }
                var list = document.CreateTextElementsExt(
                    txt,
                    paragraphStyle,
                    newTextStyle);
                //lock (document)
                {
                    var ce = this.ContentElement;
                    if (list != null && list.Count > 0)
                    {
                        {
                            this._Elements.Clear();
                            foreach (DomElement e in list.FastForEach())
                            {
                                e.SetParentRaw(this);
                                this._Elements.FastAdd2(e);
                            }
                        }
                        if (preservedElement != null)
                        {
                            this._Elements.Add(preservedElement);
                        }
                        if (ce != null && updateContentElements)
                        {
                            var args = new DomContentElement.UpdateContentElementsArgs();
                            args.UpdateParentContentElement = true;
                            args.FastMode = true;
                            args.UpdateElementsVisible = true;
                            ce.UpdateContentElements(args);
                        }
                        if (document.IsLoadingDocument == false)
                        {
                            this.UpdateContentVersion();
                        }
                        //this._ContentVersion++;
                    }
                    else
                    {
                        if (this._Elements.Count > 0)
                        {
                            {
                                // 无条件的删除所有元素
                                this._Elements.Clear();
                            }
                            if (preservedElement != null
                                && this._Elements.LastElement != preservedElement)
                            {
                                this._Elements.Add(preservedElement);
                            }
                            if (ce != null && updateContentElements)
                            {
                                var args = new DomContentElement.UpdateContentElementsArgs();
                                args.UpdateParentContentElement = true;
                                args.FastMode = true;
                                args.UpdateElementsVisible = true;
                                ce.UpdateContentElements(args);
                            }
                            this.UpdateContentVersion();
                        }
                    }
                }//lock
                return list;
            }
        }

        private static DCList<DomElement> _CachedList_SetTextRawDOM = new DCList<DomElement>();

        /// <summary>
        /// 直接设置DOM内容文本，不触发任何事件，不更新任何状态，速度最快。
        /// </summary>
        /// <param name="text">文本</param>
        /// <param name="textStyleIndex">文本样式编号</param>
        /// <param name="paragraphStyleIndex">段落样式编号</param>
        public void SetTextRawDOM(string text, int textStyleIndex, int paragraphStyleIndex)
        {
            if (string.IsNullOrEmpty(text))
            {
                return;
            }
            DomElementList list = this.Elements;
            if (textStyleIndex == -2)
            {
                if (list.Count > 0)
                {
                    textStyleIndex = list.LastElement.StyleIndex;
                }
                else
                {
                    textStyleIndex = this.StyleIndex;
                }
            }
            if (paragraphStyleIndex == -2)
            {
                paragraphStyleIndex = textStyleIndex;
            }
            DomElement preservedElement = null;
            if (this is DomContentElement
                && list.LastElement is DomParagraphFlagElement)
            {
                preservedElement = list.LastElement;
            }
            var newList = _CachedList_SetTextRawDOM;
            newList.InnerSetArrayRaw(LoaderListBuffer<DomElement[]>.Instance.Alloc(), 0);
            // 本对象没有设置所属文档，凭空创建字符对象
            this.OwnerDocument.CreateTextElementsRaw(
                this,
                text,
                textStyleIndex,
                paragraphStyleIndex,
                newList);
            if (preservedElement != null)
            {
                newList.FastAdd2(preservedElement);
            }
            if (list.Capacity > newList.Count)
            {
                list.Clear();
                list.AddRangeByDCList(newList);
            }
            else
            {
                list.SetData(newList.ToArray());
            }
            LoaderListBuffer<DomElement[]>.Instance.Return(newList.InnerGetArrayRaw());
            newList.ClearAndEmpty();
        }

        #endregion


        private bool _Visible = true;
        /// <summary>
        ///  元素是否可见
        /// </summary>
        public override bool Visible
        {
            get
            {
                return _Visible;
            }
            set
            {
                _Visible = value;
                this.InvalidateDocumentLayoutFast();
            }
        }

        /// <summary>
        /// 在编辑器中删除整个对象
        /// </summary>
        /// <param name="logUndo">是否记录撤销操作信息</param>
        public virtual bool EditorDelete(bool logUndo)
        {
            if (this is DomDocumentContentElement)
            {
                return false;
            }
            if (this.OwnerDocument == null)
            {
                throw new Exception(DCSR.OwnerDocumentNUll);
            }
            if (this.BelongToDocumentDom(this.OwnerDocument) == false)
            {
                return false;
            }
            DomContainerElement container = this.Parent;
            int index = container.Elements.IndexOf(this);
            DomDocument document = this.OwnerDocument;
            if (logUndo)
            {
                document.BeginLogUndo();
            }
            var args9 = new ReplaceElementsArgs(
                 container,
                 index,
                 1,
                 null,
                 logUndo,
                 true,
                 true);
            int result = document.ReplaceElements(args9);
            if (logUndo)
            {
                document.EndLogUndo();
            }
            if (result > 0 && logUndo == false)
            {
                document.UndoList.Clear();
            }
            document.OnDocumentContentChanged();
            return result != 0;
        }

        /// <summary>
        /// 更新文档元素的ElementIndex值。
        /// </summary>
        public virtual void UpdateElementIndex(bool deeply)
        {
            //int index = 0;
            var es = this._Elements;
            var elementsCount = es.Count;
            //foreach (XTextElement element in this.Elements.FastForEach())
            if (deeply)
            {
                var arr = es.InnerGetArrayRaw();
                for (var iCount = 0; iCount < elementsCount; iCount++)
                {
                    var element = arr[iCount];// es.GetItemFast(iCount);
                    element._ElementIndex = iCount;
                    if (element is DomContainerElement)
                    {
                        ((DomContainerElement)element).UpdateElementIndex(true);
                    }
                }
            }
            else
            {
                var esArray = es.InnerGetArrayRaw();
                for (var iCount = 0; iCount < elementsCount; iCount++)
                {
                    esArray[iCount]._ElementIndex = iCount;
                }
            }
        }

        /// <summary>
        /// 内部的快速更新文档状态
        /// </summary>
        internal virtual void InnerFixDomStateFast()
        {
            if (this._Elements != null && this._Elements.Count > 0)
            {
                if (this._Elements.Count == 1 && (this._Elements[0] is DomContainerElement) == false)
                {
                    this._Elements[0].InnerSetOwnerDocumentParentRaw(this._OwnerDocument, this);
                }
                else
                {
                    var doc = this._OwnerDocument;
                    for (int iCount = this._Elements.Count - 1; iCount >= 0; iCount--)
                    {
                        var e = this._Elements[iCount];
                        e._ElementIndex = iCount;
                        e.InnerSetOwnerDocumentParentRaw(doc, this);
                        if (e is DomContainerElement)
                        {
                            ((DomContainerElement)e).InnerFixDomStateFast();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 修复DOM结构状态
        /// </summary>
        public override void FixDomState()
        {
            DomDocument document = base._OwnerDocument;

            //this._ElementsCountDeeply = 0;
            //float tick = DCSoft.Common.CountDown.GetTickCountFloat();
            //if (this._ElementsForSerialize != null && this._ElementsForSerialize.Count > 0)
            //{
            //    this._Elements.IsElementsSplited = false;
            //    if (FixElementsForSerialize(false))
            //    {
            //        //WriterUtils.SplitElements(this.Elements);
            //    }
            //    //WriterUtilsInner.SplitElements(this.Elements, false , document , this );
            //}
            if (DomContainerElement.GlobalEnabledResetChildElementStats)
            {
                this.ResetChildElementStats();
            }
            if (this._Elements != null && this._Elements.Count > 0)
            {
                //this._ElementsCountDeeply = this._Elements.Count;
                WriterUtilsInner.SplitElements(this._Elements, false, document, this, true);
                int elementsCount = this._Elements.Count;
                var elementArray = this._Elements.InnerGetArrayRaw();
                for (int iCount = 0; iCount < elementsCount; iCount++)
                {
                    var element = elementArray[iCount];// this._Elements[iCount];
                    element._ElementIndex = iCount;
                    element.InnerSetOwnerDocumentParentRaw(document, this);
                    element.CommitStyle(false);
                    //element.SetParentRaw(this);
                    //element.SetOwnerDocumentRaw(document);
                    if (element is DomCharElement || element is DomParagraphFlagElement)
                    {
                        // 大多数元素是字符和段落符号，不处理。
                    }
                    else
                    {
                        element.FixDomState();
                        //if( element is XTextContainerElement)
                        //{
                        //    this._ElementsCountDeeply += ((XTextContainerElement)element)._ElementsCountDeeply;
                        //}
                    }
                    //if ((element is XTextCharElement) == false )
                    //{
                    //    element.FixDomState();
                    //}
                }//foreach
            }
        }
    }//public class XTextElementContainer : XTextElement
}