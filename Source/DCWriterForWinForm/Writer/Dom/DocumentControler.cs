using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using DCSoft.Common;
using DCSoft.Writer.Controls;
using DCSoft.Drawing;
using DCSoft.Writer.Serialization;
using System.ComponentModel;
using DCSoft.Writer.Data;


namespace DCSoft.Writer.Dom
{
    /// <summary>
    /// 文档控制器
    /// </summary>
    /// <remarks>编制 袁永福</remarks>
    public class DocumentControler
    {
        /// <summary>
        /// 初始化对象
        /// </summary>
        public DocumentControler()
        {
        }
        /// <summary>
        /// 清空
        /// </summary>
        public void Clear()
        {
            this._LastContentProtectedInfo = null;
            this._Snapshot = null;
        }


        private DomDocument _Document = null;
        /// <summary>
        /// 控制器操作的文档对象
        /// </summary>
       //[System.Reflection.Obfuscation(Exclude = true)]
        public DomDocument Document
        {
            get
            {
                return _Document;
            }
            set
            {
                _Document = value;
            }
        }

        private WriterAppHost _AppHost = null;
        /// <summary>
        /// 编辑器宿主对象
        /// </summary>
        public WriterAppHost AppHost
        {
            get
            {
                if (_AppHost != null)
                {
                    return _AppHost;
                }
                else if (this.EditorControl != null)
                {
                    return this.EditorControl.AppHost;
                }
                else
                {
                    return WriterAppHost.Default;
                }
            }
            set
            {
                _AppHost = value;
            }
        }
        [NonSerialized]
        private WriterControl _EditorControl = null;
        /// <summary>
        /// 控制器操作的文本编辑器控件对象
        /// </summary>
        public WriterControl EditorControl
        {
            get
            {
                return _EditorControl;
            }
            set
            {
                _EditorControl = value;
            }
        }

        private class MyStateBuffer : IDisposable
        {
            public Dictionary<CanModifyState, bool> _CanModify = new Dictionary<CanModifyState, bool>();
            public void Dispose()
            {
                if (this._CanModify != null)
                {
                    this._CanModify.Clear();
                    this._CanModify = null;
                }
            }
        }

        private MyStateBuffer _StateBuffer = null;


        private int _CacheLevel = 0;

        /// <summary>
        /// 开始缓存一些数据
        /// </summary>
        public void BeginCacheValue()
        {
            if (this._CacheLevel == 0)
            {
                this._StateBuffer = new MyStateBuffer();
                this._Cached_EditorControlReadonly = this.EditorControl != null && this.EditorControl.Readonly;
                this._Cached_EnableElementEvents = this.Document.GetDocumentBehaviorOptions().EnabledElementEvent;
                this._Cached_BehaviorOptions = this.Document.GetDocumentBehaviorOptions();
                this.Document.CacheOptions();
            }
            this._CacheLevel++;
        }

        /// <summary>
        /// 结束缓存数据
        /// </summary>
        public void EndCacheValue()
        {
            this._CacheLevel--;
            if (this._CacheLevel <= 0)
            {
                this._CacheLevel = 0;
                this._Cached_BehaviorOptions = null;
                this._StateBuffer?.Dispose();
                this._StateBuffer = null;
            }
        }

        private DocumentBehaviorOptions _Cached_BehaviorOptions = null;
        /// <summary>
        /// 运行时使用的文档行为选项
        /// </summary>
        private DocumentBehaviorOptions RuntimeBehaviorOptions
        {
            get
            {
                if (this._CacheLevel > 0)
                {
                    return this._Cached_BehaviorOptions;
                }
                return this.Document.Options.BehaviorOptions;
            }
        }

        private bool _Cached_EditorControlReadonly = false;
        private bool _Cached_EnableElementEvents = false;
        /// <summary>
        /// 控件是否只读
        /// </summary>
        public bool EditorControlReadonly
        {
            get
            {
                if (this._CacheLevel > 0)
                {
                    // 读取缓存数据
                    return this._Cached_EditorControlReadonly;
                }
                return this.EditorControl != null && this.EditorControl.Readonly;
            }
        }

        /// <summary>
        /// 运行时的允许文档元素事件。
        /// </summary>
        public bool RuntimeEnableElementEvents
        {
            get
            {
                return false;
            }
        }

        private DCContent Content
        {
            get
            {
                return this.Document.Content;
            }
        }

        private DomDocumentContentElement DocumentContent
        {
            get
            {
                return this.Document.CurrentContentElement;
            }
        }

        private DCSelection Selection
        {
            get
            {
                return this.Document.Selection;
            }
        }
        /// <summary>
        /// 判断父元素能否容纳指定的子元素
        /// </summary>
        /// <param name="parentElement">父元素</param>
        /// <param name="element">子元素</param>
        /// <param name="flags">访问标记</param>
        /// <returns>能否容纳子元素</returns>
        public virtual bool AcceptChildElement(DomElement parentElement, DomElement element, DomAccessFlags flags)
        {
            if (parentElement == null)
            {
                throw new ArgumentNullException("parentElement");
            }
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }
            return AcceptChildElement(parentElement, element.GetType(), flags);
        }

        /// <summary>
        /// 判断父元素能否容纳指定类型的子元素
        /// </summary>
        /// <param name="parentElement">父元素对象</param>
        /// <param name="elementType">子元素列表</param>
        /// <param name="flags">访问标记</param>
        /// <returns>能否容纳子元素</returns>
        public virtual bool AcceptChildElement(
            DomElement parentElement,
            Type elementType,
            DomAccessFlags flags)
        {
            if (parentElement == null)
            {
                throw new ArgumentNullException("parentElement");
            }
            if (elementType == null)
            {
                throw new ArgumentNullException("elementType");
            }
            if (parentElement is DomContainerElement)
            {
                ElementType acceptType = ((DomContainerElement)parentElement).RuntimeAcceptChildElementTypes();
                if (acceptType != ElementType.All)
                {
                    ElementType et = WriterUtilsInner.GetElementType(elementType);
                    if (et == ElementType.None)
                    {
                        if (acceptType == ElementType.None)
                        {
                            return false;
                        }
                    }
                    if (et != ElementType.None)
                    {
                        if ((acceptType & et) != et)
                        {
                            return false;
                        }
                    }
                }
            }

            if (typeof(DomTableRowElement).IsAssignableFrom(elementType))
            {
                // 表格行对象的父对象只能是表格元素
                return parentElement is DomTableElement;
            }
            else if (typeof(DomTableCellElement).IsAssignableFrom(elementType))
            {
                // 单元格对象只能以表格行元素作为父元素
                return parentElement is DomTableRowElement;
            }
            else if (parentElement is DomTableElement)
            {
                // 表格对象只能接受表格行对象
                return elementType.Equals(typeof(DomTableRowElement))
                    || elementType.IsSubclassOf(typeof(DomTableRowElement))
                    || elementType.Equals(typeof(DomTableColumnElement))
                    || elementType.IsSubclassOf(typeof(DomTableColumnElement));
            }
            else if (parentElement is DomTableRowElement)
            {
                // 表格行元素只能接受单元格对象
                return elementType.Equals(typeof(DomTableCellElement))
                    || elementType.IsSubclassOf(typeof(DomTableCellElement));
            }
            else if (parentElement is DomInputFieldElementBase)
            {
                DomInputFieldElementBase field = (DomInputFieldElementBase)parentElement;
            }
            else if (typeof(DomDocumentContentElement).IsAssignableFrom(elementType))
            {
                // 文档级容器元素对象只能在文档对象下面
                return parentElement is DomDocument;
            }
            return true;
        }

        /// <summary>
        /// 判断能否在文档的指定位置插入元素
        /// </summary>
        /// <param name="args">参数</param>
        public virtual void CanInsertObject(CanInsertObjectEventArgs args)
        {
            if (args == null || args.DataObject == null || args.Handled == true)
            {
                return;
            }
            if (this.EditorControlReadonly)
            {
                // 控件只读
                args.Result = false;
                args.Handled = true;
                return;
            }
            string specifyFormat = args.SpecifyFormat;
            if (specifyFormat != null)
            {
                specifyFormat = specifyFormat.Trim();
                if (specifyFormat.Length == 0)
                {
                    specifyFormat = null;
                }
            }
            if (specifyFormat != null)
            {
                if (args.DataObject.GetDataPresent(specifyFormat) == false)
                {
                    args.Result = false;
                    args.Handled = true;
                    return;
                }
            }
            // 判断当前位置能否插入元素
            if (this.Document == null)
            {
                args.Result = false;
                args.Handled = true;
                return;
            }
            //XTextElement element = this.Document.CurrentElement;
            //if (element == null)
            //{
            //    return false;
            //}
            DomDocumentContentElement dce = this.Document.CurrentContentElement;// element.DocumentContentElement;
            DomContainerElement container = null;
            int specifyPosition = args.SpecifyPosition;
            if (specifyPosition < 0 || specifyPosition >= dce.Content.Count)
            {
                DomElement element = this.Document.CurrentElement;
                if (element == null)
                {
                    args.Result = false;
                    args.Handled = true;
                    return;
                }
                specifyPosition = dce.Content.IndexOf(element);
            }
            int index = 0;
            dce.Content.GetPositonInfo(
                specifyPosition,
                out container,
                out index,
                dce.Content.LineEndFlag);
            if (this.CanInsert(container, index, typeof(DomElement), args.AccessFlags) == false)
            {
                args.Result = false;
                args.Handled = true;
                return;
            }

            DataObjectHelper helper = new DataObjectHelper(args.DataObject);
            if (specifyFormat == System.Windows.Forms.DataFormats.Bitmap || specifyFormat == null)
            {
                if (helper.HasImage && WriterUtilsInner.HasFormatFlag(args.AllowDataFormats, WriterDataFormats.Image))
                {
                    args.Result = this.CanInsert(
                        container,
                        index,
                        typeof(DomImageElement),
                        args.AccessFlags);
                    args.Handled = true;
                    return;
                }
                if (specifyFormat != null)
                {
                    args.Result = false;
                    args.Handled = true;
                    return;
                }
            }
            if (specifyFormat == System.Windows.Forms.DataFormats.Text || specifyFormat == null)
            {
                if (helper.HasText && WriterUtilsInner.HasFormatFlag(
                    args.AllowDataFormats,
                    WriterDataFormats.Text))
                {
                    args.Result = true;
                    args.Handled = true;
                    return;
                }
                if (specifyFormat != null)
                {
                    args.Result = false;
                    args.Handled = true;
                    return;
                }
            }
            if (specifyFormat == XMLDataFormatName || specifyFormat == null)
            {
                if (args.DataObject.GetDataPresent(XMLDataFormatName)
                    && WriterUtilsInner.HasFormatFlag(args.AllowDataFormats, WriterDataFormats.XML))
                {
                    args.Result = true;
                    args.Handled = true;
                    return;
                }
                if (specifyFormat != null)
                {
                    args.Result = false;
                    args.Handled = true;
                    return;
                }
            }

            if (args.AllowDataFormats == WriterDataFormats.All)
            {
                args.Result = true;
            }
            else
            {
                args.Result = false;
            }
        }

        private void InsertObjectWithXMLFormat(InsertObjectEventArgs args)
        {
            if (args.DataObject.GetDataPresent(XMLDataFormatName) == false)
            {
                // 没有XML格式的数据
                args.Result = false;
                return;
            }
            // 检测到XML序列化数据
            if (WriterUtilsInner.HasFormatFlag(args.AllowDataFormats, WriterDataFormats.XML) == false)
            {
                // 被拒绝的数据格式
                args.RejectFormats.Add(XMLDataFormatName);
                args.Result = false;
                return;
            }

            // 插入XML序列化的数据
            if (this.CanInsertAtCurrentPosition == false)
            {
                // 当前位置不能插入内容
                args.Result = false;
                return;
            }

            DomDocument document = null;
            DomElementList list = null;
            if (args._LastLoadedDocument != null)
            {
                document = args._LastLoadedDocument;
                list = args._LastLoadedElements;
                args._LastLoadedDocument = null;
                args._LastLoadedElements = null;
            }
            else
            {
                string xml = (string)args.DataObject.GetData(XMLDataFormatName);

                if (string.IsNullOrEmpty(xml))
                {
                    // 无内容
                    args.Result = false;
                    return;
                }
                var reader2 = WASMEnvironment.JSProvider?.CreateXmlReaderByXmlText(xml);// this.EditorControl.CreateArrayXmlReader(xml);
                document = new DomDocument();
                if (reader2 == null)
                {
                    System.IO.StringReader reader = new System.IO.StringReader(xml);
                    //document = new XTextDocument();// (XTextDocument)this.Document.CreateElementByType(this.Document.GetType());
                    document.Load(reader, WriterConst.Format_XML, true);
                }
                else
                {
                    document.LoadFromXmlReader(reader2, null);
                }
                list = document.Body.Elements.Clone();
                document.UpdateElementState();
                if (list == null || list.Count == 0)
                {
                    // 文档内容为空
                    document.Dispose();
                    args.Result = false;
                    return;
                }
                if (list.LastElement is DomParagraphFlagElement)
                {
                    DomParagraphFlagElement flag = (DomParagraphFlagElement)list.LastElement;
                    if (flag.AutoCreate == true)
                    {
                        list.RemoveAt(list.Count - 1);
                    }
                    else
                    {
                        int pcount = 0;
                        foreach (DomElement element in list)
                        {
                            if (element is DomParagraphFlagElement)
                            {
                                pcount++;
                                if (pcount > 1)
                                {
                                    break;
                                }
                            }
                        }//foreach
                        if (pcount == 1)
                        {
                            // 是唯一的段落标记符号，删掉
                            list.RemoveAt(list.Count - 1);
                        }
                    }
                    if (list.Count == 0)
                    {
                        args.Result = false;
                        return;
                    }
                }
            }
            if (args.DetectForDragContent)
            {
                // 仅仅进行检测
                args.Result = this.InnerInsertElements(args.CurrentElement, list, false, true) > 0;
                    document.Dispose();
                    list.Clear();
                return;
            }
            this.Document.BeginLogUndo();
            if (this.DocumentContent.HasSelection)
            {
                this.Content.DeleteSelection(true, false, false);
            }
            this.Document.ImportElements(list);
            this.Document.CheckForClearTextFormat(list);
            args.Result = this.InnerInsertElements(null, list, false, false) > 0;
            if (args.Result)
            {
                args.NewElements = list;
            }
            if (args.Result && args.AutoSelectContent)
            {
                this.SelectContent(list);
            }
            this.Document.EndLogUndo();
            this.Document.OnDocumentContentChanged();
            return;
        }

        /// <summary>
        /// 在文档的当前位置插入数据
        /// </summary>
        /// <param name="args">参数</param>
        /// <returns>操作是否成功</returns>
        public virtual void InsertObject(InsertObjectEventArgs args)
        {
            if (args == null || args.DataObject == null)
            {
                args.Result = false;
                return;
            }
            if (args.DetectForDragContent)
            {
                // 进行内容检测

            }
            string specifyFormat = args.SpecifyFormat;
            if (specifyFormat != null)
            {
                specifyFormat = specifyFormat.Trim();
                if (specifyFormat.Length == 0)
                {
                    specifyFormat = null;
                }
            }
            if (specifyFormat != null)
            {
                // 判断是否存在指定的数据格式
                if (args.DataObject.GetDataPresent(specifyFormat) == false)
                {
                    args.Result = false;
                    return;
                }
            }
            if (specifyFormat == XMLDataFormatName || specifyFormat == null)
            {
                if (args.DataObject.GetDataPresent(XMLDataFormatName))
                {
                    if (WriterUtilsInner.HasFormatFlag(args.AllowDataFormats, WriterDataFormats.XML) == false)
                    {
                        args.RejectFormats.Add(XMLDataFormatName.ToString());
                    }
                    else
                    {
                        InsertObjectWithXMLFormat(args);
                        return;
                    }
                }
                if (specifyFormat != null)
                {
                    args.Result = false;
                    return;
                }
            }

            DataObjectHelper helper = new DataObjectHelper(args.DataObject);
            if (specifyFormat == System.Windows.Forms.DataFormats.Bitmap || specifyFormat == null)
            {
                if (helper.HasImage)
                {
                    if (WriterUtilsInner.HasFormatFlag(args.AllowDataFormats, WriterDataFormats.Image) == false)
                    {
                        args.RejectFormats.Add("Image");
                    }
                    else
                    {
                        // 插入图片内容
                        Image img = helper.Image;
                        if (img != null)
                        {
                            if (this.CanInsertElementAtCurrentPosition(typeof(DomImageElement)))
                            {
                                XImageValue imgValue = new XImageValue(img);
                                DomImageElement img2 = InsertImage(imgValue, true, args.InputSource);
                                args.Result = img2 != null;
                                if (args.Result)
                                {
                                    args.NewElements = new DomElementList(img2);
                                }
                                if (args.Result && args.AutoSelectContent)
                                {
                                    this.SelectContent(new DomElementList(img2));
                                }
                                return;
                            }
                        }
                        args.Result = false;
                        return;
                    }
                }
                if (specifyFormat != null)
                {
                    args.Result = false;
                    return;
                }
            }
            if (specifyFormat == System.Windows.Forms.DataFormats.Text || specifyFormat == null)
            {
                if (helper.HasText)
                {
                    if (WriterUtilsInner.HasFormatFlag(args.AllowDataFormats, WriterDataFormats.Text) == false)
                    {
                        args.RejectFormats.Add("Text");
                    }
                    else
                    {
                        // 插入纯文本
                        if (this.CanInsertAtCurrentPosition)
                        {
                            string txt = helper.Text;
                            DomElementList list = this.InsertString(
                                txt,
                                true,
                                args.InputSource,
                                null,
                                null);

                            args.Result = list != null && list.Count > 0;
                            if (args.Result)
                            {
                                args.NewElements = list;
                            }
                            if (args.Result && args.AutoSelectContent)
                            {
                                this.SelectContent(list);
                            }
                        }
                        else
                        {
                            args.Result = false;
                        }
                        return;
                    }
                }
                if (specifyFormat != null)
                {
                    args.Result = false;
                    return;
                }
            }
            args.Result = false;
        }



        /// <summary>
        /// 插入图片文件内容
        /// </summary>
        /// <param name="img">图片对象</param>
        /// <param name="logUndo">是否撤销操作</param>
        /// <param name="inputSource">输入来源</param>
        /// <returns>插入的图片元素对象</returns>
        public virtual DomImageElement InsertImage(
            XImageValue img,
            bool logUndo,
            InputValueSource inputSource)
        {
            if (img == null)
            {
                throw new ArgumentNullException("img");
            }
            DomImageElement imgElement = new DomImageElement();// this.Document.CreateImage();
            imgElement.OwnerDocument = this.Document;
            imgElement.Image = img;
            imgElement.UpdateSize(false);
            WriterUtilsInner.CheckImageSizeWhenInsertImage(
                this.Document,
                imgElement,
                imgElement.KeepWidthHeightRate,
                null);
            if (logUndo)
            {
                this.Document.BeginLogUndo();
            }
            if (this.DocumentContent.HasSelection)
            {
                this.Content.DeleteSelection(true, false, false);
            }
            this.Document.InsertElement(imgElement);
            this.Document.Modified = true;
            if (logUndo)
            {
                this.Document.EndLogUndo();
            }
            this.Document.OnDocumentContentChanged();
            return imgElement;
        }

        /// <summary>
        /// 执行删除操作
        /// </summary>
        /// <returns>本操作是否删除了元素</returns>
        public virtual bool Delete(bool showUI)
        {
            bool result = false;
            int index = -1;
            this.Document.ClearContentProtectedInfos();
            this.EditorControl.CancelEditElementValue();
            if (this.DocumentContent.HasSelection == false)
            {
                DomElement element = this.Document.CurrentElement;
                if (element is DomFieldBorderElement)
                {
                    DomFieldElementBase field = (DomFieldElementBase)element.Parent;
                    if (field.StartElement == element && field.Elements.Count > 0)
                    {
                        // 如果遇到输入域前边界元素而且输入域内容不为空，则提前进入到输入域之中
                        this.Document.Content.MoveToPosition(this.Document.Content.IndexOf(element) + 1);
                    }
                    else if (field.EndElement == element && field.Elements.Count > 0)
                    {
                        if (this.Document.GetDocumentBehaviorOptions().AllowDeleteJumpOutOfField == false)
                        {
                            // 如果遇到非空输入域的结束边界元素，则无法删除。
                            MoveCaretForDeleteFail(true);
                            return false;
                        }
                        int iCount = 0;
                        while (iCount++ < 4 && element is DomFieldBorderElement)
                        {
                            DomElement nextElement = this.Document.Content.GetNextElement(element);
                            if (nextElement == null)
                            {
                                break;
                            }
                            DomFieldElementBase field2 = (DomFieldElementBase)element.Parent;
                            if (field2 == null)
                            {
                                break;
                            }
                            bool emptyField = IsEmptyInContent(field2);
                            if (field2.StartElement == element
                                && emptyField == false
                                && this.Document.Content.Contains(field2) == false)
                            {
                                // 光标紧跟在一个输入域的前面，则插入点移动到输入域中
                                if (this.Document.Content.MoveToPosition(
                                    this.Document.Content.IndexOf(nextElement)))
                                {
                                    element = this.Document.CurrentElement;
                                    nextElement = this.Document.Content.GetNextElement(element);
                                }
                                else
                                {
                                    break;
                                }
                            }
                            else if (field2.EndElement == element)
                            {
                                // 光标紧跟在一个输入域的后置边界元素，则将插入点放置在输入域的后面
                                if (this.Document.Content.MoveToPosition(
                                    this.Document.Content.IndexOf(nextElement)))
                                {
                                    element = this.Document.CurrentElement;
                                    nextElement = this.Document.Content.GetNextElement(element);
                                }
                                else
                                {
                                    break;
                                }
                            }
                        }//while
                    }
                }
            }
            if (this.DocumentContent.HasSelection == false)
            {
                if (DeleteEmptyField(this.Content.CurrentElement, false))
                {
                    return true;
                }
            }
            this.Document.BeginLogUndo();
            if (this.DocumentContent.HasSelection)
            {
                index = this.Content.DeleteSelection(true, false, false);
            }
            else
            {
                index = this.Content.DeleteCurrentElement(true);
            }
            this.Document.EndLogUndo();

            if (index >= 0)
            {
                if (index > 0)
                {
                    index--;
                }
                this.Document.OnDocumentContentChanged();
                //this.Document.CurrentContentElement.RefreshPrivateContent(index);
                result = true;
            }
            if (this.Document.HasContentProtectedInfos())
            {
                if (showUI)
                {
                    this.Document.PromptProtectedContent(null);
                }
            }
            this.Document.SetContentProtectedInfos(null);
            if (result == false)
            {
                MoveCaretForDeleteFail(true);
            }
            return result;
        }
        private bool DeleteEmptyField(DomElement element, bool detectOnly)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }
            DomFieldElementBase field = null;
            if (element is DomFieldBorderElement)
            {
                field = element.Parent as DomFieldElementBase;

            }
            if (element.Parent is DomFieldElementBase)
            {
                DomFieldElementBase field2 = (DomFieldElementBase)element.Parent;
                if (field2.IsBackgroundTextElement(element))
                {
                    field = field2;
                }
            }
            if (field == null)
            {
                return false;
            }
            if (field.ElementsCount > 0)
            {
                return false;
            }
            // 遇到内容为空的输入域
            if (this.CanDelete(field) == false)
            {
                if (detectOnly == false)
                {
                    this.AddLastContentProtectdInfoOnce(this.Document.ContentProtectedInfos);
                    //this.Document.ContentProtectedInfos.Add(field, this.GetLastContentProtectedMessageOnce());
                }
                return false;
            }
            else
            {
                if (detectOnly)
                {
                    return true;
                }
                else
                {
                    var parentField = field.Parent as DomInputFieldElement;
                    DomDocumentContentElement dce = field.DocumentContentElement;
                    int index = dce.Content.IndexOf(field.StartElement);
                    if (index < 0)
                    {
                        dce.Content.IndexOf(field);
                    }
                    if (field.EditorDelete(true))
                    {
                        if (index >= 0)
                        {
                            var bolFlag3 = false;
                            if (parentField != null && IsEmptyInContent(parentField))
                            {
                                var index9 = dce.Content.IndexOfUseContentIndex(parentField.EndElement);
                                if (index9 > 0)
                                {
                                    dce.Content.MoveToPosition(index9);
                                    bolFlag3 = true;
                                }
                            }
                            if (bolFlag3 == false)
                            {
                                dce.Content.MoveToPosition(index);
                            }
                        }
                        return true;
                    }
                }
            }
            return false;
        }
        ///// <summary>
        ///// 执行剪切操作
        ///// </summary>
        ///// <returns>操作是否成功</returns>
        //public bool Cut(bool showUI)
        //{
        //    if (this.DocumentContent.HasSelection)
        //    {
        //        this.Copy();
        //        this.Delete(showUI);
        //        return true;
        //    }
        //    return false;
        //}

        /// <summary>
        /// 判断能否进行剪切操作
        /// </summary>
        /// <returns>能否进行操作</returns>
        public bool CanCut()
        {
            if (this.DocumentContent.HasSelection)
            {
                return this.Snapshot.CanDeleteSelection;
            }
            return false;
        }

        /// <summary>
        /// 判断能否执行复制操作
        /// </summary>
        public bool CanCopy
        {
            get
            {
                return this.DocumentContent.HasSelection;
            }
        }

        /// <summary>
        /// 程序集版本信息
        /// </summary>
        public static string XMLDataFormatName
        {
            get
            {
                return WriterConst.XMLDataFormatName;// "DCWriterXML V:" + DCSystemInfo.VersionString;// typeof(DocumentControler).Assembly.GetName().Version;
            }
        }

        public System.Windows.Forms.IDataObject WASMCreateSelectionDatas(bool textOnly)
        {
            System.Windows.Forms.IDataObject obj = CreateSelectionDataObject(
                this.EditorControl.CreationDataFormats,
                textOnly || this.Document.GetDocumentEditOptions().CopyInTextFormatOnly,
                this.Document.GetDocumentEditOptions().ClearFieldValueWhenCopy,
                this.Document.GetDocumentEditOptions().CopyWithoutInputFieldStructure);
            if (obj != null)
            {
                if (this._DataByteSize_CreateSelectionDataObject > 50 * 1024)
                {
                    if ((DateTime.Now - this._LastCopyTime).TotalMilliseconds < 500)
                    {
                        // 对于大数据量的复制操作不能进行频繁操作。
                        return null;
                    }
                }
                this._LastCopyTime = DateTime.Now;
                return obj;
            }
            else
            {
                return null;
            }
        }

        private DateTime _LastCopyTime = DateTime.MinValue;
        ///// <summary>
        ///// 执行复制操作
        ///// </summary>
        ///// <returns>操作是否成功</returns>
        //public bool Copy()
        //{
        //    System.Windows.Forms.IDataObject obj = CreateSelectionDataObject(
        //        this.EditorControl.CreationDataFormats,
        //        this.Document.GetDocumentEditOptions().CopyInTextFormatOnly,
        //        this.Document.GetDocumentEditOptions().ClearFieldValueWhenCopy,
        //        this.Document.GetDocumentEditOptions().CopyWithoutInputFieldStructure);
        //    if (obj != null)
        //    {
        //        if (this._DataByteSize_CreateSelectionDataObject > 50 * 1024)
        //        {
        //            if ((DateTime.Now - this._LastCopyTime).TotalMilliseconds < 500)
        //            {
        //                // 对于大数据量的复制操作不能进行频繁操作。
        //                return false;
        //            }
        //        }
        //        this._LastCopyTime = DateTime.Now;
        //        this.EditorControl.InnerSetSourceDataObject(obj);
        //        return true;
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}

        ///// <summary>
        ///// 执行复制操作
        ///// </summary>
        ///// <returns>操作是否成功</returns>
        //public bool CopyAsText()
        //{
        //    System.Windows.Forms.IDataObject obj = CreateSelectionDataObject(
        //        this.EditorControl.CreationDataFormats,
        //        true,
        //        this.Document.GetDocumentEditOptions().ClearFieldValueWhenCopy,
        //        this.Document.GetDocumentEditOptions().CopyWithoutInputFieldStructure);
        //    if (obj != null)
        //    {
        //        this.EditorControl._DataObjectFromPaste = obj;
        //        //this.EditorControl.InnerSetSourceDataObject(obj);
        //        return true;
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}

        ///// <summary>
        ///// 复制内容，而且复制的内容中清空输入域中的数据
        ///// </summary>
        ///// <returns>操作是否成功</returns>
        //public bool CopyWithClearFieldValue()
        //{
        //    System.Windows.Forms.IDataObject obj = CreateSelectionDataObject(
        //        this.EditorControl.CreationDataFormats,
        //        true,
        //        true,
        //        this.Document.GetDocumentEditOptions().CopyWithoutInputFieldStructure);
        //    if (obj != null)
        //    {
        //        this.EditorControl.InnerSetSourceDataObject(obj);
        //        return true;
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}

        private int _DataByteSize_CreateSelectionDataObject = 0;
        /// <summary>
        /// 创建选择数据对象
        /// </summary>
        /// <param name="allowFormats"></param>
        /// <param name="textOnly"></param>
        /// <param name="clearFieldValueWhenCopy"></param>
        /// <param name="copyWithoutInputFieldStructure"></param>
        /// <returns></returns>
        public System.Windows.Forms.IDataObject CreateSelectionDataObject(
            WriterDataFormats allowFormats,
            bool textOnly,
            bool clearFieldValueWhenCopy,
            bool copyWithoutInputFieldStructure)
        {
            _DataByteSize_CreateSelectionDataObject = 0;
            DCSelection selection = this.Selection;
            if (selection != null && selection.Length != 0)
            {
                var obj = this.EditorControl.CreateDataObject();// .GetInnerViewControl().RuntimeClipboard.CreateDataObject();// new System.Windows.Forms.DataObject();
                string txt = selection.Text;
                if (txt != null && txt.Length > 0)
                {
                    txt = DCSoft.Common.StringCommon.CompressWhiteSpace(txt);
                    if (txt.Length > 1024)
                    {
                        txt = txt.Substring(0, 1024);
                    }
                    obj.SetTitle(txt);
                    _DataByteSize_CreateSelectionDataObject += System.Text.Encoding.UTF8.GetByteCount(txt);
                }
                if (this.EditorControl != null)
                {
                    // 设置控件句柄
                    obj.SetData(WriterConst.Name_WriterHandle, this.EditorControl.Handle.ToString());
                }
                if (WriterUtilsInner.HasFormatFlag(allowFormats, WriterDataFormats.Text) || textOnly)
                {
                    var txt2 = selection.Text;
                    // 设置纯文本数据
                    obj.SetData(
                        System.Windows.Forms.DataFormats.Text,
                        txt2);
                    //obj.SetData(
                    //    System.Windows.Forms.DataFormats.UnicodeText,
                    //    txt2);
                    _DataByteSize_CreateSelectionDataObject += System.Text.Encoding.UTF8.GetByteCount(txt2);
                }
                if (textOnly)
                {
                    return obj;
                }
                if (WriterUtilsInner.HasFormatFlag(allowFormats, WriterDataFormats.Image))
                {
                    // 设置图片数据
                    if (selection.Length == 1 && selection.Mode == ContentRangeMode.Content
                        && selection.ContentElements[0] is DomImageElement)
                    {
                        DomImageElement img = (DomImageElement)selection.ContentElements[0];
                        if (img.Image.Value != null)
                        {
                            obj.SetData(System.Windows.Forms.DataFormats.Bitmap, img.Image.Value);//// .SetImage(img.Image.Value);
                            _DataByteSize_CreateSelectionDataObject += img.Image.ByteSize;
                        }
                    }
                }
                string strNativeXml = null;
                if (WriterUtilsInner.HasFormatFlag(allowFormats, WriterDataFormats.XML))
                {
                    // 设置XML数据
                    using (DomDocument selectionDocument = selection.CreateDocument())
                    {
                        //if (this.EditorControl != null)
                        //{
                        //    selectionDocument.EditorControlHandle = this.EditorControl.Handle.ToInt32();
                        //}
                        if (clearFieldValueWhenCopy)
                        {
                            // 清空输入域内容
                            foreach (DomInputFieldElementBase field in selectionDocument.GetElementsByType<DomInputFieldElementBase>())
                            {
                                field.SetInnerTextFast(null);
                                field.InnerValue = null;
                            }
                        }
                        if (copyWithoutInputFieldStructure)
                        {
                            // 删除输入域层次结构
                            WriterUtilsInner.RemoveInputFieldStructure(selectionDocument.Elements);
                        }
                        System.IO.StringWriter writer = new System.IO.StringWriter();
                        selectionDocument.Save(writer, WriterConst.Format_XML);
                        string xml = writer.ToString();
                        _DataByteSize_CreateSelectionDataObject += System.Text.Encoding.UTF8.GetByteCount(xml);
                        obj.SetData(XMLDataFormatName, xml);
                        strNativeXml = xml;
                    }
                }
                return obj;
            }
            return null;
        }

        /// <summary>
        /// 执行在当前位置插入一个字符串的操作
        /// </summary>
        /// <param name="strText">要插入的字符串值</param>
        /// <param name="logUndo">是否记录操作过程</param>
        /// <param name="inputSource">来源</param>
        /// <param name="textStyle">字符样式</param>
        /// <param name="paragraphStyle">段落样式</param>
        /// <returns>插入的文档元素对象列表</returns>
        public virtual DomElementList InsertString(
            string strText,
            bool logUndo,
            InputValueSource inputSource,
            DocumentContentStyle textStyle,
            DocumentContentStyle paragraphStyle)
        {
            if (string.IsNullOrEmpty(strText))// strText == null || strText.Length == 0)
            {
                return null;
            }
            if (textStyle == null)
            {
                textStyle = (DocumentContentStyle)this.Document.CurrentStyleInfo.ContentStyleForNewString;
            }
            if (paragraphStyle == null)
            {
                paragraphStyle = (DocumentContentStyle)this.Document.CurrentStyleInfo.Paragraph.Clone();
                paragraphStyle.FontName = textStyle.FontName;
                paragraphStyle.FontSize = textStyle.FontSize;
                paragraphStyle.Italic = textStyle.Italic;
                paragraphStyle.Bold = textStyle.Bold;
                paragraphStyle.Underline = textStyle.Underline;
                paragraphStyle.Strikeout = textStyle.Strikeout;
                paragraphStyle.Color = textStyle.Color;
                //if (paragraphStyle.ParagraphOutlineLevel >= 0)
                //{
                //    // 遇到标题类型的段落，则设置一些属性为默认值
                //    paragraphStyle.ParagraphOutlineLevel = -1;
                //    paragraphStyle.ParagraphMultiLevel = false;
                //    paragraphStyle.ParagraphListStyle = ParagraphListStyle.None;
                //    paragraphStyle.Font = this.Document.DefaultStyle.Font;
                //    paragraphStyle.LeftIndent = 0;
                //    paragraphStyle.FirstLineIndent = 0;
                //}
                if (paragraphStyle.ParagraphOutlineLevel >= 0 && paragraphStyle.ParagraphListStyle != ParagraphListStyle.None)
                {
                    // 带大纲层次的列表样式,则使用普通列表样式
                    //paragraphStyle = ( DocumentContentStyle ) this.Document.DefaultStyle.Clone();
                }
            }
            DomContainerElement c = (DomContainerElement)this.Document.GetCurrentElement(typeof(DomContainerElement));
            if (c is DomInputFieldElement)
            {
                var ce = this.Document.CurrentElement;
                if ((ce is DomFieldBorderElement) && ((DomFieldBorderElement)ce).Position == BorderElementPosition.Start)
                {
                    c = c.Parent;
                }
            }
            if (inputSource == InputValueSource.UI && strText == "\t")
            {
                if (c.AcceptTab == false)
                {
                    // 容器元素不接受来自用户输入的Tab字符。
                    return null;
                }
            }
            DomElementList newElements = this.Document.CreateTextElementsExt(
                strText,
                paragraphStyle,
                textStyle);

            if (newElements == null || newElements.Count == 0)
            {
                return null;
            }
            DomCharElement autoUppercaseCharElement = null;
            if (logUndo)
            {
                this.Document.BeginLogUndo();
            }
            if (this.DocumentContent.HasSelection)
            {
                int result2 = this.Content.DeleteSelection(true, false, false);
                if (result2 > 0)
                {
                    this.Document.Modified = true;
                }
            }
            else
            {
                if (this.EditorControl != null)
                {
                    if (this.EditorControl.InsertMode == false)
                    {
                        int result2 = this.Content.DeleteCurrentElement(true);
                        if (result2 > 0)
                        {
                            this.Document.Modified = true;
                        }
                    }
                }
            }
            if (newElements.Count == 1
                && newElements[0] is DomParagraphFlagElement)
            {
                if (this.Document.IsCurrentPositionAtFirstCell)
                {
                    DomParagraphFlagElement pe = (DomParagraphFlagElement)newElements[0];
                    DocumentContentStyle style = new DocumentContentStyle();
                    style.Align = DocumentContentAlignment.Left;
                    pe.StyleIndex = this.Document.ContentStyles.GetStyleIndex(style);
                }
            }
            DocumentContentStyle styleBack = this.Document.CurrentStyleInfo.Content;
            DocumentContentStyle styleForNewStringBack = this.Document.CurrentStyleInfo.ContentStyleForNewString;
            // 正在插入字符串，不可能插入或删除XTextObjectElement元素，因此无需刷新XTextObjectElement相关的信息
            DomDocumentContentElement._InsertOrDeleteTextOnlyFlag = true;
            int result = 0;
            try
            {
                var args4 = new InsertElementsArgs();
                args4.NewElements = newElements;
                args4.UpdateContent = true;
                args4.FromUI = inputSource == InputValueSource.UI;
                result = this.Document.InsertElements(args4);
            }
            finally
            {
                DomDocumentContentElement._InsertOrDeleteTextOnlyFlag = false;
            }
            if (autoUppercaseCharElement != null
                && DCSoft.Common.StringFormatHelper.IsEnglishLetter(strText[0]) == false
                && autoUppercaseCharElement != null)
            {
                // 试图自动设置首字母大写
                char newChar = char.ToUpper(autoUppercaseCharElement.GetCharValue());
                char oldValue = autoUppercaseCharElement.GetCharValue();
                autoUppercaseCharElement.CharValue = newChar;
                if (logUndo && this.Document.UndoList.CanLog())
                {
                    this.Document.UndoList.AddProperty("CharValue",
                        oldValue,
                        newChar,
                        autoUppercaseCharElement);
                }
                autoUppercaseCharElement.EditorRefreshView();
                this.Document.OnSelectionChanged();
            }
            if (logUndo)
            {
                this.Document.EndLogUndo();
            }
            if (this.Document.InnerFixCurrentStyleInfoForEnter)
            {
                if (styleBack != this.Document.CurrentStyleInfo.Content)
                {
                    this.Document.CurrentStyleInfo.Content = styleBack;
                    this.Document.CurrentStyleInfo.Refresh(this.Document);
                    this.Document.CurrentStyleInfo.ContentStyleForNewString = styleForNewStringBack;
                }
            }
            this.Document.Modified = true;
            if (result > 0)
            {
                this.Document.OnDocumentContentChanged();
                return newElements;
            }
            return null;
        }

        /// <summary>
        /// 插入一个换行元素
        /// </summary>
        public virtual DomLineBreakElement InsertLineBreak()
        {
            this.Document.BeginLogUndo();
            DomLineBreakElement br = new DomLineBreakElement();// (XTextLineBreakElement)this.Document.CreateElementByType(typeof(XTextLineBreakElement));
            br.OwnerDocument = this.Document;
            var st = this.Document.CurrentStyleInfo?.ContentStyleForNewString;
            if (st != null)
            {
                br.Style.FontName = st.FontName;
                br.Style.FontSize = st.FontSize;
                br.Style.FontStyle = st.FontStyle;
                br.Style.Color = st.Color;
                br.CommitStyleFast();
            }
            using (DCGraphics g = this.Document.InnerCreateDCGraphics())
            {
                InnerDocumentPaintEventArgs args = this.Document.CreateInnerPaintEventArgs(g);
                args.Element = br;
                br.RefreshSize(args);
            }
            this.Document.InsertElement(br);
            this.Document.EndLogUndo();
            this.Document.OnDocumentContentChanged();
            return br;
        }

        /// <summary>
        /// 在当前插入点插入一个元素
        /// </summary>
        /// <param name="element">要插入的元素</param>
        /// <returns>操作是否成功</returns>
        public bool InsertElement(DomElement element)
        {
            if (element != null)
            {
                element.OwnerDocument = this.Document;
                this.Document.BeginLogUndo();
                if (element.SizeInvalid)
                {
                    using (DCGraphics g = this.Document.InnerCreateDCGraphics())
                    {
                        InnerDocumentPaintEventArgs args = this.Document.CreateInnerPaintEventArgs(g);
                        args.Element = element;
                        element.RefreshSize(args);
                    }
                }
                bool result = this.Document.InsertElement(element);
                this.Document.EndLogUndo();
                this.Document.OnDocumentContentChanged();
                return result;
            }
            return false;
        }

        /// <summary>
        /// 在当前插入点处插入若干个元素
        /// </summary>
        /// <param name="list">要插入的元素的列表</param>
        /// <remarks>插入的元素个数</remarks>
        public int InsertElements(DomElementList list)
        {
            if (list != null && list.Count > 0)
            {
                WriterUtilsInner.SplitElements(list, true, this.Document, list[0].Parent, false);
                return InnerInsertElements(null, list, true, false);
            }
            else
            {
                return 0;
            }
        }
        /// <summary>
        /// 在当前插入点处插入若干个元素
        /// </summary>
        /// <param name="logUndo">是否记录撤销操作信息</param>
        /// <param name="list">要插入的元素的列表</param>
        /// <returns>插入的元素个数</returns>
        private int InnerInsertElements(DomElementList list, bool logUndo)
        {
            return InnerInsertElements(null, list, logUndo, false);
        }

        /// <summary>
        /// 在当前插入点处插入若干个元素
        /// </summary>
        /// <param name="currentElement">当前元素</param>
        /// <param name="list">要插入的元素的列表</param>
        /// <param name="logUndo">是否记录撤销信息</param>
        /// <param name="detectOnly">检测模式</param>
        /// <returns>插入的元素个数</returns>
        private int InnerInsertElements(
            DomElement currentElement,
            DomElementList list,
            bool logUndo,
            bool detectOnly)
        {
            if (list == null || list.Count == 0)
            {
                return 0;
            }
            //WriterUtilsInner.SplitElements(list, true, this.Document, currentElement?.Parent,false);
            MeasureElementSize(list);
            if (detectOnly == false)
            {
                if (logUndo)
                {
                    this.Document.BeginLogUndo();
                }
            }
            InsertElementsArgs args = new InsertElementsArgs();
            args.CurrentElement = currentElement;
            args.NewElements = list;
            args.LogUndo = true;
            args.DetectOnly = detectOnly;
            if (detectOnly == false)
            {
                this.Document.FixElementIDForInsertElements(list);
            }
            int result = this.Document.InsertElements(args);
            if (detectOnly == false)
            {
                this.Document.Modified = true;
                if (logUndo)
                {
                    this.Document.EndLogUndo();
                }
            }
            return result;
        }

        /// <summary>
        /// 选择内容
        /// </summary>
        /// <param name="elements">文档元素列表</param>
        /// <returns>操作是否成功</returns>
        public virtual bool SelectContent(DomElementList elements)
        {
            if (elements == null)
            {
                throw new ArgumentNullException("elements");
            }
            DomElement fe = elements.FirstContentElement;
            DomElement le = elements.LastContentElement;
            DomDocumentContentElement dce = fe.DocumentContentElement;
            if (dce != null)
            {
                int index1 = dce.Content.IndexOf(fe);
                int index2 = dce.Content.IndexOf(le);
                if (index1 >= 0 && index2 >= index1)
                {
                    dce.SetSelection(index1, index2 - index1 + 1);
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 计算文档元素的大小
        /// </summary>
        /// <param name="list">文档元素列表</param>
        public virtual void MeasureElementSize(DomElementList list)
        {
            if (list == null)
            {
                throw new ArgumentNullException("list");
            }
            if (list.Count == 0)
            {
                return;
            }
            using (DCGraphics g = this.Document.InnerCreateDCGraphics())
            {
                foreach (DomElement element in list)
                {
                    element.OwnerDocument = this.Document;
                    element.FixDomState();
                    //if (element.SizeInvalid)
                    {
                        // 插入元素时全部计算大小
                        InnerDocumentPaintEventArgs args = this.Document.CreateInnerPaintEventArgs(g);
                        args.Element = element;
                        element.RefreshSize(args);

                        //this.Render.RefreshSize(element, g);
                    }
                    if (element is DomImageElement)
                    {
                        // 修改图片大小
                        DomImageElement img = (DomImageElement)element;
                        WriterUtilsInner.CheckImageSizeWhenInsertImage(
                            this.Document,
                            img,
                            img.KeepWidthHeightRate,
                            null);
                    }
                    else if (element is DomTableElement)
                    {
                        // 修改表格宽度
                        WriterUtilsInner.CheckTableWidthWhenInsertTable(
                            this.Document,
                            (DomTableElement)element);
                    }
                }//foreach
            }//using
        }

        /// <summary>
        /// 删除内容失败后移动光标位置
        /// </summary>
        /// <param name="deleteKey"></param>
        private void MoveCaretForDeleteFail(bool deleteKey)
        {
            if (this.Document.GetDocumentBehaviorOptions().AllowDeleteJumpOutOfField == true)
            {
                if (this.Document.Content.SelectionLength == 0)
                {
                    bool allowMoveLeft = true;
                    bool allowMoveRight = true;
                    if (deleteKey)
                    {
                        if (allowMoveRight)
                        {
                            this.Document.Content.MoveRight();
                        }
                    }
                    else
                    {
                        if (allowMoveLeft)
                        {
                            this.Document.Content.MoveLeft();
                        }
                    }
                }
            }
        }
        private bool IsEmptyInContent(DomFieldElementBase field)
        {
            DomDocumentContentElement dce = field.DocumentContentElement;
            if (dce != null && dce.Content != null)
            {
                var content = dce.Content;
                var indexEnd = content.IndexOfUseContentIndex(field.EndElement);
                var indexStart = content.IndexOfUseContentIndex(field.StartElement);
                if (indexEnd == indexStart + 1)
                {
                    // 直接排在一起，则为空输入域
                    return true;
                }
                if (indexStart < indexEnd - 1
                    && content[indexStart + 1] is DomCharElement
                    && ((DomCharElement)content[indexStart + 1]).IsBackgroundText)
                {
                    // 两者之间是一个输入域背景文本，则为空输入域
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 执行删除插入点前一个元素的操作
        /// </summary>
        /// <returns>本操作是否删除了元素</returns>
        public virtual bool Backspace(bool showUI)
        {
            if (this.EditorControlReadonly)
            {
                // 控件是只读的
                return false;
            }
            this.EditorControl.CancelEditElementValue();
            this.Document.ClearContentProtectedInfos();
            int index = -1;
            this.Document.BeginLogUndo();
            if (this.DocumentContent.HasSelection == false)
            {
                // 没有选择任何内容，则删除当前位置的前一个元素
                DomElement element = this.Document.CurrentElement;
                DomElement preElement = this.Document.Content.GetPreElement(element);
                if (DomLine.__NewLayoutMode)
                {
                    preElement = this.Document.Content.GetPreLayoutElement(element);
                    if (element == this.Document.Content.LastElement && preElement == null)
                    {
                        if (element is DomParagraphFlagElement)
                        {
                            if (this.SpecificChangeParagraphIndent(false))
                            {
                                this.Document.EndLogUndo();
                                return true;
                            }
                        }
                        this.Document.EndLogUndo();
                        return false;
                    }
                }
                if (preElement != null)
                {
                    if (preElement is DomFieldBorderElement)
                    {
                        DomFieldElementBase field = (DomFieldElementBase)preElement.Parent;
                        if (field.StartElement == preElement)
                        {
                            if (IsEmptyInContent(field) == false)
                            {
                                // 要删除的元素是一个非空输入域的前置元素，无法删除
                                MoveCaretForDeleteFail(false);
                                this.Document.ClearContentProtectedInfos();
                                return false;

                            }
                            else
                            {
                                // 遇到一个空白的输入域，试图删除
                                bool result2 = DeleteEmptyField(preElement, false);
                                if (result2 == false)
                                {
                                    MoveCaretForDeleteFail(false);
                                }
                                this.Document.ClearContentProtectedInfos();
                                return result2;
                            }
                        }
                    }

                    int iCount = 0;
                    while (preElement is DomFieldBorderElement)
                    {
                        if (iCount++ > 4)
                        {
                            // 避免不明原因死循环
                            break;
                        }
                        DomFieldElementBase field = (DomFieldElementBase)preElement.Parent;
                        if (field != null)
                        {
                            bool emptyField = IsEmptyInContent(field);
                            if (field.EndElement == preElement
                                && emptyField == false
                                && this.Document.Content.Contains(field) == false)
                            {
                                // 光标紧跟在一个输入域的后面，则插入点移动到输入域之中
                                var vc = this._EditorControl?.GetInnerViewControl();
                                bool back = vc == null ? true : vc.InnerEnableEditElementValue;
                                try
                                {
                                    if (vc != null)
                                    {
                                        vc.InnerEnableEditElementValue = false;
                                    }
                                    if (this.Document.Content.MoveToPosition(
                                       this.Document.Content.IndexOf(preElement)))
                                    {
                                        element = this.Document.CurrentElement;
                                        preElement = this.Document.Content.GetPreElement(element);
                                    }
                                    else
                                    {
                                        break;
                                    }
                                }
                                finally
                                {
                                    if (vc != null)
                                    {
                                        vc.InnerEnableEditElementValue = back;
                                    }
                                }

                            }
                            else if (field.StartElement == preElement
                                && emptyField == false
                                && this.Document.Content.Contains(field) == false)
                            {
                                // 光标紧跟在一个输入域的前置边界元素，而且输入域不为空，
                                // 则试图将插入点放置到输入域之前
                                {
                                    if (this.Document.Content.MoveToPosition(
                                        this.Document.Content.IndexOf(preElement)))
                                    {
                                        element = this.Document.CurrentElement;
                                        preElement = this.Document.Content.GetPreElement(element);
                                    }
                                    else
                                    {
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                // 没有遇到上述清空，退出循环
                                break;
                            }
                        }
                        else
                        {
                            break;
                        }
                    }//while
                }
                if (preElement != null)
                {
                    this._LastContentProtectedInfo = null;
                    if (DeleteEmptyField(preElement, false))
                    {
                        this.Document.ClearContentProtectedInfos();
                        return true;
                    }
                    else
                    {
                        if (this.Document.HasContentProtectedInfos())
                        {
                            if (showUI)
                            {
                                this.Document.PromptProtectedContent(null);
                            }
                            this.Document.SetContentProtectedInfos(null);
                            MoveCaretForDeleteFail(false);
                            return false;
                        }
                    }
                    //else
                    //{
                    //    return false;
                    //}
                }

                if (SpecificChangeParagraphIndent(false) == true)
                {
                    return true;
                }
            }

            this.Document.SetContentProtectedInfos(null);
            var viewControl = this.EditorControl.GetInnerViewControl();
            if (this.DocumentContent.HasSelection)
            {
                // 若选择内容则删除用户选择的内容
                if (this.Content.Selection.Mode == ContentRangeMode.Cell)
                {

                }
                int vIndex = this.Content.Selection.AbsStartIndex;
                index = this.Content.DeleteSelection(true, false, false);
                if (index > 0)
                {
                    this.Content.SetSelection(vIndex, 0);
                }
                else
                {
                }
            }
            else
            {
                // 若没有选择内容则删除前一个元素
                index = this.Content.DeletePreElement(true);
                if (index < 0)
                {
                    MoveCaretForDeleteFail(false);
                }
            }
            bool result = false;
            if (index >= 0)
            {
                this.Document.EndLogUndo();
                //this.Document.CurrentContentElement.RefreshPrivateContent(index);
                this.Document.OnSelectionChangedWithCheckVersion();// .OnSelectionChanged();
                this.Document.OnDocumentContentChanged();
                result = true;
            }
            else
            {
                this.Document.EndLogUndo();
            }
            if (this.Document.HasContentProtectedInfos() && showUI)
            {
                this.Document.PromptProtectedContent(null);
            }
            this.Document.SetContentProtectedInfos(null);
            return result;
        }

        /// <summary>
        /// 特殊的修改段落的缩进
        /// </summary>
        /// <param name="increase">true:增加缩进；false:减小缩进</param>
        /// <returns>操作是否成功</returns>
        public bool SpecificChangeParagraphIndent(bool increase)
        {
            DomElement element = this.Document.CurrentElement;
            if (element == null)
            {
                return false;
            }
            DomParagraphFlagElement peof = element.OwnerParagraphEOF;
            //XTextDocumentContentElement dce = peof.DocumentContentElement;
            if (peof != null
                //&& peof != element 
                && element == peof.ParagraphFirstContentElement)
            {
                // 若当前插入点是在一个内容不为空的段落的最前面,也就是段首。
                if (this.CanModify(peof, DomAccessFlags.CheckUserEditable | DomAccessFlags.Normal))
                {
                    DocumentContentStyle style =
                        (DocumentContentStyle)peof.RuntimeStyle.CloneParent();
                    style.DisableDefaultValue = true;
                    bool modify = false;
                    bool paragraphListModify = false;
                    if (increase)
                    {
                        DomContentElement ce = peof.ContentElement;
                        float maxPos = 0;
                        if (ce == null)
                        {
                            maxPos = this.Document.Body.ClientWidth - 100;
                        }
                        else
                        {
                            maxPos = ce.ClientWidth - 100;
                        }
                        if (maxPos < 0)
                        {
                            maxPos = 0;
                        }
                        // 增加段落缩进
                        float newFirstLineIndent = style.FirstLineIndent;
                        float newLeftIndent = style.LeftIndent;
                        if (style.FirstLineIndent >= 90)
                        {
                            // 段落整体缩进
                            newLeftIndent += 100;
                            //float newValue = style.LeftIndent + 100;
                            //if (newValue > maxPos)
                            //{
                            //    newValue = maxPos;
                            //}
                            //if (newValue + style.FirstLineIndent > maxPos)
                            //{
                            //    newValue = maxPos;
                            //    style.FirstLineIndent = 0;
                            //    modify = true;
                            //}
                            //if (newValue != style.LeftIndent)
                            //{
                            //    style.LeftIndent = newValue;
                            //    modify = true;
                            //}
                        }
                        else
                        {
                            newFirstLineIndent += 100;

                            //float newValue = style.FirstLineIndent + 100;

                            //style.FirstLineIndent += 100;
                            //modify = true;
                        }
                        if (newLeftIndent > maxPos)
                        {
                            newLeftIndent = maxPos;
                        }
                        if (newFirstLineIndent > maxPos)
                        {
                            newFirstLineIndent = maxPos;
                        }
                        if (newLeftIndent + newFirstLineIndent > maxPos)
                        {
                            newFirstLineIndent = 0;
                            newLeftIndent = maxPos;
                        }
                        if (style.FirstLineIndent != newFirstLineIndent)
                        {
                            style.FirstLineIndent = newFirstLineIndent;
                            modify = true;
                        }
                        if (style.LeftIndent != newLeftIndent)
                        {
                            style.LeftIndent = newLeftIndent;
                            modify = true;
                        }
                    }
                    else
                    {
                        if (style.ParagraphListStyle != ParagraphListStyle.None)
                        {
                            // 取消段落列表样式
                            modify = true;
                            if (style.IsListNumberStyle)
                            {
                                paragraphListModify = true;
                            }
                            style.ParagraphListStyle = ParagraphListStyle.None;
                            style.LeftIndent = 0;
                            style.FirstLineIndent = 0;
                            if (style.ParagraphOutlineLevel >= 0)
                            {
                                // 标题样式
                                style = (DocumentContentStyle)this.Document.DefaultStyle.Clone();
                            }
                        }
                        else if (style.Align == DocumentContentAlignment.Right)
                        {
                            // 右对齐变成居中对齐
                            modify = true;
                            style.Align = DocumentContentAlignment.Center;
                        }
                        else if (style.Align == DocumentContentAlignment.Center)
                        {
                            // 居中对齐变成左对齐
                            modify = true;
                            style.Align = DocumentContentAlignment.Left;
                        }
                        else if (style.FirstLineIndent + style.LeftIndent < 30 && style.FirstLineIndent != 0)
                        {
                            // 缩进量相互抵消了
                            style.FirstLineIndent = 0;
                            style.LeftIndent = 0;
                            modify = true;
                        }
                        // 减小段落缩进
                        else if (style.FirstLineIndent > 0)
                        {
                            // 首先减少首行缩进
                            style.FirstLineIndent -= 100f;
                            if (style.FirstLineIndent < 0)
                            {
                                style.FirstLineIndent = 0;
                            }
                            modify = true;
                        }
                        else
                        {
                            // 减少段落左缩进
                            float back = style.LeftIndent;
                            style.LeftIndent -= 100;
                            if (style.LeftIndent < 0)
                            {
                                style.LeftIndent = 0;
                            }
                            modify = back != style.LeftIndent;
                        }
                        //if (style.LeftIndent > 0)
                        //{
                        //    style.LeftIndent -= 100 ;
                        //    if (style.LeftIndent < 0)
                        //    {
                        //        style.LeftIndent = 0;
                        //    }
                        //    modify = true;
                        //}
                        //else if (style.FirstLineIndent > 0)
                        //{
                        //    // 取消段首缩进
                        //    style.FirstLineIndent -= 100;
                        //    if (style.FirstLineIndent < 0)
                        //    {
                        //        style.FirstLineIndent = 0;
                        //    }
                        //    modify = true;
                        //}
                        //else if (style.ParagraphListStyle != ParagraphListStyle.None)
                        //{
                        //    // 取消段落列表样式
                        //    modify = true;
                        //    style.ParagraphListStyle = ParagraphListStyle.None;
                        //}
                    }
                    if (modify)
                    {
                        // 修改了段落设置
                        this.Document.CurrentStyleInfo = null;
                        int styleIndex = this.Document.ContentStyles.GetStyleIndex(style);
                        if (this.Document.BeginLogUndo())
                        {
                            this.Document.UndoList.AddStyleIndex(peof, peof.StyleIndex, styleIndex);
                            //.AddProperty(
                            //    "StyleIndex",
                            //    peof.StyleIndex,
                            //    styleIndex,
                            //    peof);
                        }
                        peof.StyleIndex = styleIndex;
                        peof.UpdateContentVersion();
                        DomElementList list = new DomElementList();
                        list.Add(peof.ParagraphFirstContentElement);
                        list.Add(peof.LastContentElement);
                        if (paragraphListModify)
                        {
                            peof.DocumentContentElement.ParagraphTreeInvalidte = true;
                            peof.DocumentContentElement.RefreshParagraphListState(false, true);
                            if (this.Document.CanLogUndo)
                            {
                                this.Document.UndoList.AddMethod(DCSoft.Writer.Dom.Undo.UndoMethodTypes.RefreshParagraphTree);
                            }
                        }
                        WriterUtilsInner.RefreshElementsSize(this.Document, new DomElementList(peof), false);
                        peof.SizeInvalid = true;
                        peof.ContentElement.RefreshContentByElements(
                            list,
                            true,
                            false);
                        //BindControl .UpdateInvalidateRect();
                        this.EditorControl.UpdateTextCaret();
                        //this.Document.CurrentStyle.FirstLineIndent = 0;
                        this.Document.OnSelectionChanged();
                        this.Document.EndLogUndo();
                        this.EditorControl.UpdateRuleState();
                        return true;
                    }//if
                }//if
            }//if
            return false;
        }
        /// <summary>
        /// 判断能否修改当前段落或被选择的段落
        /// </summary>
        public virtual bool CanModifyParagraphs
        {
            get
            {
                var ps = this.Selection.ParagraphsEOFs;
                foreach (DomElement p in ps)
                {
                    if (CanModify(p))
                    {
                        return true;
                    }
                }
                return false;
            }
        }
        /// <summary>
        /// 判断能否修改被选择的内容
        /// </summary>
        public virtual bool CanModifySelection(bool checkContentProtect)
        {
            if (this.EditorControlReadonly)
            {
                return false;
            }
            DomAccessFlags flags = DomAccessFlags.Normal;
            if (this.Selection.Length == 0)
            {
                DomContainerElement container = null;
                int index = 0;
                this.Document.Content.GetCurrentPositionInfo(out container, out index);
                if (container is DomInputFieldElementBase)
                {
                    DomInputFieldElementBase field = (DomInputFieldElementBase)container;
                    return field.InnerRuntimeUserEditable();
                }
                return CanModifyContent(container, flags);// CanModify(container , flags );
            }
            else
            {
                foreach (DomElement element in this.Selection.ContentElements)
                {
                    if (CanModify(element, flags))
                    {
                        return true;
                    }
                }
                return false;
            }
        }


        /// <summary>
        /// 判断指定的元素能否被修改
        /// </summary>
        /// <param name="element">元素对象</param>
        /// <returns>元素能否被修改</returns>
        public virtual bool CanModify(DomElement element)
        {
            return CanModify(element, DomAccessFlags.Normal);
        }

        internal struct CanModifyState
        {
            public CanModifyState(DomElement element, DomAccessFlags flags, bool forContent = false)
            {
                this.Element = element;
                this.Flags = flags;
                this.ForContent = forContent;
                this._HasCode = this.Element.GetHashCode() + 2 * (int)this.Flags + this.ForContent.GetHashCode();
            }
            public DomElement Element;
            public DomAccessFlags Flags;
            public bool ForContent;
            public readonly int _HasCode;
            public override int GetHashCode()
            {
                return this._HasCode;
            }
            public override bool Equals(object obj)
            {
                var state = (CanModifyState)obj;
                return this.Element == state.Element
                    && this.Flags == state.Flags
                    && this.ForContent == state.ForContent;
            }
        }

        /// <summary>
        /// 判断指定的元素能否被修改
        /// </summary>
        /// <param name="element">元素对象</param>
        /// <param name="flags">访问标记</param>
        /// <returns>元素能否被修改</returns>
        public virtual bool CanModify(DomElement element, DomAccessFlags flags)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }
            if (_StateBuffer != null)
            {
                var state = new CanModifyState(element, flags, false);
                bool v = false;
                if (_StateBuffer._CanModify.TryGetValue(state, out v))
                {
                    return v;
                }
            }
            ElementStateDetectEventArgs args = new ElementStateDetectEventArgs(element, flags);
            CanModify(args);
            if (_StateBuffer != null)
            {
                _StateBuffer._CanModify[new CanModifyState(element, flags, false)] = args.Result;
            }
            return args.Result;
        }

        /// <summary>
        /// 判断指定的元素的内容能否被修改
        /// </summary>
        /// <param name="element">元素对象</param>
        /// <param name="flags">访问标记</param>
        /// <returns>元素能否被修改</returns>
        public virtual bool CanModifyContent(DomElement element, DomAccessFlags flags)
        {
            if (_StateBuffer != null)
            {
                var state = new CanModifyState(element, flags, true);
                bool v = false;
                if (_StateBuffer._CanModify.TryGetValue(state, out v))
                {
                    return v;
                }
            }
            ElementStateDetectEventArgs args = new ElementStateDetectEventArgs(element, flags);
            args.ForContent = true;
            CanModify(args);
            if (_StateBuffer != null)
            {
                _StateBuffer._CanModify[new CanModifyState(element, flags, true)] = args.Result;
            }
            return args.Result;
        }

        /// <summary>
        /// 判断能否修改文档元素
        /// </summary>
        /// <param name="args">参数</param>
        public virtual void CanModify(ElementStateDetectEventArgs args)
        {
            if (args == null)
            {
                throw new ArgumentNullException("args");
            }
            if (args.Element == null)
            {
                throw new ArgumentNullException("args.Element");
            }
            if (args.Flags == DomAccessFlags.None)
            {
                // 没有任何标记，表示可以修改
                args.Result = true;
                return;
            }
            if (HasFlag(args.Flags, DomAccessFlags.CheckControlReadonly))
            {
                // 检查控件是否只读
                if (this.EditorControlReadonly)
                {
                    if (args.SetMessage)
                    {
                        args.Message = DCSR.EditControlReadonly;
                    }
                    SetLastContentProtected(args.Element, args.Message, ContentProtectedReason.ControlReadonly);
                    //SetLastContentProtectedMessage(args.Message);
                    args.Result = false;
                    return;
                }
            }

            DomElement parent = args.Element;
            while (parent != null && (parent is DomDocument) == false)
            {
                if (parent is DomInputFieldElementBase && parent != args.Element)
                {
                    // 容器元素为文本域
                    DomInputFieldElementBase field = (DomInputFieldElementBase)parent;
                    if (field.StartElement == args.Element || field.EndElement == args.Element)
                    {
                    }
                    else
                    {
                        // 不是起始或结束元素
                        if (HasFlag(args.Flags, DomAccessFlags.CheckUserEditable))
                        {
                            // 文本域内容用户不能直接修改
                            if (field.InnerRuntimeUserEditable() == false)
                            {
                                if (args.SetMessage)
                                {
                                    args.Message = string.Format(
                                        DCSR.ReadonlyInputFieldUserEditable_ID,
                                        field.DisplayName);
                                }
                                SetLastContentProtected(args.Element, args.Message, ContentProtectedReason.ContainerReadonly);
                                args.Result = false;
                                return;
                            }
                        }
                    }
                }

                parent = parent.Parent;
            }//while
            args.Result = true;
        }


        /// <summary>
        /// 判断指定的元素能否被删除
        /// </summary>
        /// <param name="element">指定的元素对象</param>
        /// <returns>元素能否删除</returns>
        public bool CanDelete(DomElement element)
        {
            return CanDelete(element, DomAccessFlags.Normal);
        }

        private ContentProtectedInfo _LastContentProtectedInfo = null;
        private void SetLastContentProtected(DomElement element, string msg, ContentProtectedReason reason)
        {
            _LastContentProtectedInfo = new ContentProtectedInfo(element, msg, reason);
        }

        /// <summary>
        /// 获得最后一次内容保护描述信息并清空该信息
        /// </summary>
        /// <returns>获得的信息</returns>
        public ContentProtectedInfo GetLastContentProtectedInfoOnce()
        {
            if (this._LastContentProtectedInfo != null)
            {
                ContentProtectedInfo p = this._LastContentProtectedInfo;
                this._LastContentProtectedInfo = null;
                return p;
            }
            return null;
        }
        /// <summary>
        /// 添加最后一次内容保护描述信息
        /// </summary>
        /// <param name="list"></param>
        public void AddLastContentProtectdInfoOnce(ContentProtectedInfoList list)
        {
            if (this._LastContentProtectedInfo != null)
            {
                if (list != null)
                {
                    list.Add(this._LastContentProtectedInfo);
                }
                this._LastContentProtectedInfo = null;
            }
        }

        /// <summary>
        /// 扩展性的判断元素能否被删除
        /// </summary>
        /// <param name="element">文档元素对象</param>
        /// <param name="flags">标记</param>
        /// <returns>能否被删除</returns>
        public bool CanDeleteExt(DomElement element, DomAccessFlags flags)
        {
            if (element is DomFieldBorderElement)
            {
                element = element.Parent;
                ElementStateDetectEventArgs args = new ElementStateDetectEventArgs(element, flags);
                CanDelete(args);
                return args.Result;
            }
            ElementStateDetectEventArgs args2 = new ElementStateDetectEventArgs(element, flags);
            CanDelete(args2);
            return args2.Result;
        }

        /// <summary>
        /// 判断指定元素能否被删除
        /// </summary>
        /// <param name="element">元素对象</param>
        /// <param name="flags">访问标记</param>
        /// <returns>元素能否删除</returns>
        public virtual bool CanDelete(DomElement element, DomAccessFlags flags)
        {
            this._LastContentProtectedInfo = null;
            ElementStateDetectEventArgs args = new ElementStateDetectEventArgs(element, flags);
            CanDelete(args);
            return args.Result;
        }

        /// <summary>
        /// 判断能否删除文档元素
        /// </summary>
        /// <param name="args">参数</param>
        public virtual void CanDelete(ElementStateDetectEventArgs args)
        {
            if (args == null)
            {
                throw new ArgumentNullException("args");
            }
            if (args.ResetLastContentProtectedInfo)
            {
                this._LastContentProtectedInfo = null;
            }
            DomElement element = args.Element;
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }
            if (element is DomTableCellElement)
            {
                var cell2 = (DomTableCellElement)element;
                if (cell2.OverrideCell != null)
                {
                    element = cell2.OverrideCell;
                }
            }
            var elementRuntimeStyle = element.RuntimeStyle;
            if (args.Flags == DomAccessFlags.None)
            {
                // 没有任何标记，表示可以删除
                args.Result = true;
                return;
            }
            DomAccessFlags flags = args.Flags;
            if (HasFlag(flags, DomAccessFlags.CheckControlReadonly))
            {
                // 检查控件是否是只读的
                if (this.EditorControlReadonly)
                {
                    // 控件被设置为只读的
                    if (args.SetMessage)
                    {
                        args.Message = DCSR.EditControlReadonly;
                    }
                    args.ProtectedReason = ContentProtectedReason.ControlReadonly;
                    SetLastContentProtected(args.Element, args.Message, args.ProtectedReason);
                    args.Result = false;
                    return;
                }
            }
            if (element is DomParagraphFlagElement)
            {
                DomContentElement ce = element.ContentElement;
                if (ce.PrivateContent.LastElement == element)
                {
                    // 文本块元素中最后一个段落符号不能删除
                    if (args.SetMessage)
                    {
                        args.Message = DCSR.ReadonlyCanNotDeleteLastParagraphFlag;
                    }
                    args.ProtectedReason = ContentProtectedReason.UnDeleteable;
                    SetLastContentProtected(args.Element, args.Message, args.ProtectedReason);
                    args.Result = false;
                    return;
                }
            }
            if (this.RuntimeEnableElementEvents)
            {
                // 允许文档元素事件
                if ((element is DomCharElement || element is DomParagraphFlagElement) == false)
                {
                }
            }//if
            DomContainerElement parent = element.Parent;
            if (args.CacheParentStateResult && args.ParentStateResult != null)
            {
                //使用缓存的数据
                args.Result = args.ParentStateResult.Result;
                args.Element = args.ParentStateResult.Element;
                args.Message = args.ParentStateResult.Message;
                args.ProtectedReason = args.ParentStateResult.ProtectedReason;
            }
            else
            {
                CanDelete_CheckParent(args);
            }
            if (args.CacheParentStateResult && args.ParentStateResult == null)
            {
                args.ParentStateResult = args.Clone();
            }
            if (args.Result == false)
            {
                return;
            }
            args.Result = true;
        }
        private void CanDelete_CheckParent(ElementStateDetectEventArgs args)
        {
            DomElement element = args.Element;
            DomElement parent = element.Parent;
            DomAccessFlags flags = args.Flags;
            while (parent != null)
            {
                if (parent is DomFieldElementBase)
                {
                    DomFieldElementBase field = (DomFieldElementBase)parent;
                    if (field.StartElement == element || field.EndElement == element)
                    {
                        // 字段的开始元素和结束元素不能删除
                        if (args.SetMessage)
                        {
                            args.Message = DCSR.ReadonlyCanNotDeleteBorderElement;
                        }
                        args.ProtectedReason = ContentProtectedReason.UnDeleteable;
                        SetLastContentProtected(args.Element, args.Message, args.ProtectedReason);
                        args.Result = false;
                        return;
                    }
                    if (field.IsBackgroundTextElement(element))
                    {
                        // 字段的背景文本元素不能删除
                        if (args.SetMessage)
                        {
                            args.Message = DCSR.ReadonlyCanNotDeleteBackgroundText;
                        }
                        args.ProtectedReason = ContentProtectedReason.UnDeleteable;
                        SetLastContentProtected(args.Element, args.Message, args.ProtectedReason);
                        args.Result = false;
                        return;
                    }
                }
                if (parent is DomInputFieldElementBase)
                {
                    // 容器元素为文本域
                    DomInputFieldElementBase field = (DomInputFieldElementBase)parent;
                    if (HasFlag(flags, DomAccessFlags.CheckUserEditable))
                    {
                        // 检查输入域的可直接输入状态
                        if (field.InnerRuntimeUserEditable() == false)
                        {
                            if (args.SetMessage)
                            {
                                args.Message = string.Format(
                                    DCSR.ReadonlyInputFieldUserEditable_ID,
                                    field.DisplayName);
                            }
                            args.ProtectedReason = ContentProtectedReason.ContainerReadonly;
                            SetLastContentProtected(args.Element, args.Message, args.ProtectedReason);
                            args.Result = false;
                            return;
                        }
                    }
                }//if
                if (this.RuntimeEnableElementEvents)
                {
                }
                parent = parent.Parent;
            }//while
        }
        /// <summary>
        /// 判断能否删除文档中选中的区域或者当前元素
        /// </summary>
        public virtual bool CanDeleteSelection
        {
            get
            {
                if (this.EditorControlReadonly)
                {
                    // 控件只读
                    return false;
                }
                if (this.Selection.Length == 0)
                {
                    return true;
                }
                else
                {
                    int result = this.Content.DeleteSelection(false, true, false);
                    return result != 0;
                }
                //return false;
            }
        }



        /// <summary>
        /// 判断能否在指定容器元素的指定序号处插入新的元素
        /// </summary>
        /// <param name="container">容器元素</param>
        /// <param name="index">指定序号</param>
        /// <param name="newElement">准备插入的新元素</param>
        /// <param name="flags">访问标记</param>
        /// <returns>能否插入元素</returns>
        public virtual bool CanInsert(
            DomContainerElement container,
            int index,
            DomElement newElement,
            DomAccessFlags flags)
        {
            CanInsertElementEventArgs args = new CanInsertElementEventArgs(
                container,
                index,
                newElement,
                flags);
            CanInsetElementInstance(args);
            return args.Result;
        }

        /// <summary>
        /// 判断能否创建文档元素
        /// </summary>
        /// <param name="args">参数</param>
        public virtual void CanInsetElementInstance(CanInsertElementEventArgs args)
        {
            if (args == null)
            {
                throw new ArgumentNullException("args");
            }
            if (args.Container == null)
            {
                throw new ArgumentNullException("args.Container");
            }
            if (args.Element == null)
            {
                throw new ArgumentNullException("args.Element");
            }
            if (HasFlag(args.Flags, DomAccessFlags.CheckControlReadonly))
            {
                if (this.EditorControlReadonly)
                {
                    // 控件是只读的
                    if (args.SetMessage)
                    {
                        args.Message = DCSR.EditControlReadonly;
                    }
                    args.Result = false;
                    return;
                }
            }
            if (this.AcceptChildElement(args.Container, args.Element, args.Flags) == false)
            {
                if (args.SetMessage)
                {
                    args.Message = string.Format(
                        DCSR.ReadonlyCannotAcceptElementType_ParentType_ChildType,
                        args.Container.DispalyTypeName(),
                        args.Element.DispalyTypeName());
                }
                args.Result = false;
                return;
            }
            DomContainerElement element = args.Container;
            while (element != null)
            {
                if (element is DomInputFieldElementBase)
                {
                    DomInputFieldElementBase field = (DomInputFieldElementBase)element;
                    if (HasFlag(args.Flags, DomAccessFlags.CheckUserEditable))
                    {
                        // 检查输入域用户是否可以直接修改
                        if (field.InnerRuntimeUserEditable() == false)
                        {
                            if (args.SetMessage)
                            {
                                args.Message = string.Format(
                                    DCSR.ReadonlyInputFieldUserEditable_ID,
                                    field.DisplayName);
                            }
                            args.Result = false;
                            return;
                        }
                    }
                }//if
                element = element.Parent;
            }
            args.Result = true;
        }

        /// <summary>
        /// 判断指定容器元素中指定位置能否插入指定类型的子元素
        /// </summary>
        /// <param name="container">容器元素</param>
        /// <param name="index">要插入子元素的序号</param>
        /// <param name="elementType">子元素类型</param>
        /// <returns>能否插入子元素</returns>
        public virtual bool CanInsert(
            DomContainerElement container,
            int index,
            Type elementType)
        {
            CanInsertElementEventArgs args = new CanInsertElementEventArgs(
                container,
                index,
                elementType,
                DomAccessFlags.Normal);
            CanInsertSpecifyElementType(args);
            return args.Result;
        }

        /// <summary>
        /// 判断指定容器元素中指定位置能否插入指定类型的子元素
        /// </summary>
        /// <param name="container">容器元素</param>
        /// <param name="index">要插入子元素的序号</param>
        /// <param name="elementType">子元素类型</param>
        /// <param name="flags">访问标记</param>
        /// <returns>能否插入子元素</returns>
        public virtual bool CanInsert(
            DomContainerElement container,
            int index,
            Type elementType,
            DomAccessFlags flags)
        {
            CanInsertElementEventArgs args = new CanInsertElementEventArgs(
                container,
                index,
                elementType,
                flags);
            CanInsertSpecifyElementType(args);
            return args.Result;
        }

        /// <summary>
        /// 判断指定容器元素中指定位置能否插入指定类型的子元素
        /// </summary>
        /// <param name="args">参数</param>
        public virtual void CanInsertSpecifyElementType(CanInsertElementEventArgs args)
        {
            if (args == null)
            {
                throw new ArgumentNullException("args");
            }
            // 检查参数
            if (args.Container == null)
            {
                throw new ArgumentNullException("args.Container");
            }
            if (args.ElementType == null)
            {
                throw new ArgumentNullException("args.ElementType");
            }
            if (HasFlag(args.Flags, DomAccessFlags.CheckControlReadonly))
            {
                // 检查控件是否只读
                if (this.EditorControlReadonly)
                {
                    if (args.SetMessage)
                    {
                        args.Message = DCSR.EditControlReadonly;
                    }
                    SetLastContentProtected(args.Element, args.Message, ContentProtectedReason.ControlReadonly);
                    args.Result = false;
                    return;
                }
            }

            DomContainerElement element = args.Container;
            if (this.AcceptChildElement(args.Container, args.ElementType, args.Flags) == false)
            {
                if (args.SetMessage)
                {
                    args.Message = string.Format(
                        DCSR.ReadonlyCannotAcceptElementType_ParentType_ChildType,
                        args.Container.DispalyTypeName(),
                        WriterUtilsInner.GetTypeDisplayName(args.ElementType));
                }
                SetLastContentProtected(args.Element, args.Message, ContentProtectedReason.ContainerReadonly);
                args.Result = false;
                return;
            }
            while (element != null)
            {
                if (element is DomInputFieldElementBase)
                {
                    DomInputFieldElementBase field = (DomInputFieldElementBase)element;
                    if (HasFlag(args.Flags, DomAccessFlags.CheckUserEditable))
                    {
                        if (field.InnerRuntimeUserEditable() == false)
                        {
                            // 输入域内容不能直接修改
                            if (args.SetMessage)
                            {
                                args.Message = string.Format(
                                    DCSR.ReadonlyInputFieldUserEditable_ID,
                                    field.DisplayName);
                            }
                            SetLastContentProtected(args.Element, args.Message, ContentProtectedReason.ContainerReadonly);
                            args.Result = false;
                            return;
                        }
                    }
                }
                if (this.RuntimeEnableElementEvents)
                {
                }
                element = element.Parent;
            }//while
            args.Result = true;
        }

        /// <summary>
        /// 判断能否在当前位置插入元素
        /// </summary>
        /// <returns></returns>
        public bool CanInsertAtCurrentPosition
        {
            get
            {
                return CanInsertElementAtCurrentPosition(
                    typeof(DomElement),
                    DomAccessFlags.Normal);
            }
        }

        /// <summary>
        /// 判断能否在当前位置插入指定类型的元素
        /// </summary>
        /// <param name="newElementType">新插入元素的类型</param>
        /// <returns>能否插入新元素</returns>
        public bool CanInsertElementAtCurrentPosition(Type newElementType)
        {
            return CanInsertElementAtCurrentPosition(
                newElementType,
                DomAccessFlags.Normal);
        }

        /// <summary>
        /// 判断能否在当前位置插入指定类型的元素
        /// </summary>
        /// <param name="newElementType">新插入元素的类型</param>
        /// <param name="flags">访问标记</param>
        /// <returns>能否插入新元素</returns>
        public virtual bool CanInsertElementAtCurrentPosition(
            Type newElementType,
            DomAccessFlags flags)
        {
            if ((flags & DomAccessFlags.CheckControlReadonly)
                == DomAccessFlags.CheckControlReadonly)
            {
                if (this.EditorControlReadonly)
                {
                    // 控件只读
                    return false;
                }
            }
            DomContainerElement container = null;
            int elementIndex = 0;
            this.Document.Content.GetPositonInfo(
                this.Document.Selection.StartIndex,
                out container,
                out elementIndex,
                this.Document.Content.LineEndFlag);
            if (container == null)
            {
                return false;
            }
            return CanInsert(container, elementIndex, newElementType, flags);
        }


        private DocumentControlerSnapshot _Snapshot = null;
        /// <summary>
        /// 文档状态快照
        /// </summary>
        public DocumentControlerSnapshot Snapshot
        {
            get
            {
                if (_Snapshot != null)
                {
                    if (_Snapshot.DocumentContentVersion != this.Document.ContentVersion
                        || _Snapshot.SelectionVerion != this.Document.Selection.SelectionVersion)
                    {
                        _Snapshot = null;
                    }
                }
                if (_Snapshot == null)
                {
                    _Snapshot = new DocumentControlerSnapshot();
                    _Snapshot.DocumentContentVersion = this.Document.ContentVersion;
                    _Snapshot.SelectionVerion = this.Document.Selection.SelectionVersion;
                    _Snapshot.OwnerControler = this;
                    _Snapshot.CanModifySelection = this.CanModifySelection(true);
                    _Snapshot.CanModifyParagraphs = this.CanModifyParagraphs;
                    _Snapshot.CanDeleteSelection = this.CanDeleteSelection;
                }
                return _Snapshot;
            }
        }

        /// <summary>
        /// 清除状态快照
        /// </summary>
        public void ClearSnapshot()
        {
            _Snapshot = null;
        }
        /// <summary>
        /// 清除缓存
        /// </summary>
        public void ClearBuffer()
        {
            this._LastContentProtectedInfo = null;
            this._Snapshot = null;
        }
        private bool HasFlag(DomAccessFlags flags, DomAccessFlags specifyFlag)
        {
            return (flags & specifyFlag) == specifyFlag;
        }

    }
}
