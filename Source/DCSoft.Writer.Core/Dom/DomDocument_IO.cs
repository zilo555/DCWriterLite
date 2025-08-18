using System;
using DCSoft.Printing;
using DCSoft.Writer.Serialization;
using System.ComponentModel;
using DCSoft.Drawing;
using DCSoft.Common;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using DCSoft.Writer.Data;
using System.Text;
using System.Runtime.InteropServices;
using DCSoft.Writer.Controls;
using System.IO.Compression;

namespace DCSoft.Writer.Dom
{
    /// <summary>
    /// 文档加载和保存的代码群
    /// </summary>
    public partial class DomDocument
    {
        private string _FileName = null;
        /// <summary>
        /// 文件名
        /// </summary>
        public string FileName
        {
            get
            {
                return _FileName;
            }
            set
            {
                _FileName = value;
            }
        }

        private string _FileFormat = null;
        /// <summary>
        /// 文件格式
        /// </summary>
        public string FileFormat
        {
            get
            {
                return _FileFormat;
            }
            set
            {
                _FileFormat = value;
            }
        }

        private bool _EnableAfterLoad = true;
        /// <summary>
        /// 允许执行AfterLoad()功能
        /// </summary>
        internal bool EnableAfterLoad
        {
            get { return _EnableAfterLoad; }
            set { _EnableAfterLoad = value; }
        }
        // 触发元素的Load事件
        private void AfterLoad_ElementOnLoadEvents(DomContainerElement rootElement, ElementLoadEventArgs args, WriterViewControl ctl)
        {
            var arr = rootElement.GetCompressedElements();
            if (arr != null)
            {
                foreach (DomElement element in arr)
                {

                    if (element is DomContainerElement)
                    {
                        var c = (DomContainerElement)element;
                        if (c.HasElements())
                        {
                            AfterLoad_ElementOnLoadEvents(c, args, ctl);
                        }
                    }
                }
            }
        }


        /// <summary>
        /// 内部的文档加载后的处理
        /// </summary>
        /// <param name="objArgs">事件参数</param>
        public override void AfterLoad(ElementLoadEventArgs args)
        {
            if (this.EnableAfterLoad == false)
            {
                return;
            }
            this._Parent = null;
            this.States.Layouted = false;
            this.ContentStyles.Styles.SetValueLocked(false);
            _ObjectIDIncrease = 0;
            var opts = this.Options;
            if (string.IsNullOrEmpty(this.ContentStyles.Default.FontName))
            {
                this.ContentStyles.Default.FontName = XFontValue.DefaultFontName;
            }
            if (this.ContentStyles.Default.FontSize <= 0)
            {
                this.ContentStyles.Default.FontSize = 9;
            }
            this.ContentStyles.HandleAfterLoad();
            args.Element = this;
            this.FixDomStateWithCheckInvalidateFlag();

            base._OwnerDocument = this;
            base.AfterLoad(args);
            this.CurrentContentElement = this.Body;
            // 精简压缩文档样式列表
            CompressStyleList();
            // 触发文档加载事件
            this.OnDocumentLoad(EventArgs.Empty);
            if (opts.BehaviorOptions.EnableElementEvents)
            {
                var viewCtl = this.InnerViewControl;
                if (viewCtl != null)
                {
                    // 允许执行文档元素事件
                    // 触发所有元素的Load事件
                    AfterLoad_ElementOnLoadEvents(this, args, viewCtl);

                }
            }//if

            this.ContentStyles.Styles.SetValueLocked(true);
        }


        #region 加载文件的功能代码

        [NonSerialized]
        private int _LoadDocumentFlag = 0;
        /// <summary>
        /// 开始加载文档
        /// </summary>
        internal void BeginLoadDocument()
        {
            _LoadDocumentFlag++;
        }
        internal void EndLoadDocument()
        {
            _LoadDocumentFlag--;
            if (_LoadDocumentFlag < 0)
            {
                _LoadDocumentFlag = 0;
            }
        }
        /// <summary>
        /// 是否正在加载文档
        /// </summary>
        public bool IsLoadingDocument
        {
            get
            {
                return this._LoadDocumentFlag > 0;
            }
        }

        /// <summary>
        /// 判断是否允许触发ContentChanged事件
        /// </summary>
        /// <returns></returns>
        public bool EnableContentChangedEvent()
        {
            if (this.Options.BehaviorOptions.EnableContentChangedEventWhenLoadDocument == false)
            {
                // 在加载文档时是否触发ContentChanged事件。
                if (this.IsLoadingDocument)
                {
                    return false;
                }
            }
            return true;

        }

        /// <summary>
        /// 以指定的格式从指定名称的XML读取器中加载文档
        /// </summary>
        /// <param name="reader">XML读取器</param>
        /// <param name="format">文件格式</param>
        //[ComVisible(false)]
        //[System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public void LoadFromXmlReader(DCSystemXml.XmlReader reader, string format)
        {
            try
            {
                //var dtm = DateTime.Now;
                this.BeginLoadDocument();
                InnerDeserialize(null, reader, format, false, DocumentLoadMode.Normal, null);
                //Console.WriteLine("load5 : " + (DateTime.Now - dtm).TotalMilliseconds);
            }
            finally
            {
                this.EndLoadDocument();
            }
        }
        /// <summary>
        /// 以指定的格式从指定名称的文件中加载文档
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <param name="format">文件格式</param>
        public void LoadFromFile(string fileName, string format)
        {
            try
            {
                this.BeginLoadDocument();
                InnerDeserialize(null, fileName, format, false, DocumentLoadMode.Normal, null);
            }
            finally
            {
                this.EndLoadDocument();
            }
        }
        /// <summary>
        /// 以指定的格式从指定的文本中加载文档
        /// </summary>
        /// <param name="text">文本</param>
        /// <param name="format">文件格式</param>
        public void LoadFromString(string text, string format)
        {
            if (string.IsNullOrEmpty(format))
            {
                format = WriterUtilsInner.DetectFileFormat(text);
            }
            var ser = DCSoft.Writer.Serialization.Xml.XMLContentSerializer.Instance;
            if (ser != null
                && ser is DCSoft.Writer.Serialization.Xml.XMLContentSerializer
                && text != null)
            {
                // 修正XML字符串
                int index = text.IndexOf('<');
                if (index > 0)
                {
                    text = text.Substring(index);
                }
            }
            StringReader reader = new StringReader(text);
            try
            {
                this.BeginLoadDocument();
                InnerDeserialize(null, reader, format, false, DocumentLoadMode.Normal, null);
            }
            finally
            {
                this.EndLoadDocument();
            }
        }
        /// <summary>
        /// 从Base64的字符串中加载文档
        /// </summary>
        /// <param name="text">Base64字符串</param>
        /// <param name="format">指定的文件格式</param>
        public void LoadFromBase64String(string text, string format)
        {
            byte[] bs = Convert.FromBase64String(text);
            if (string.IsNullOrEmpty(format))
            {
                format = WriterUtilsInner.DetectFileFormat(bs);
            }
            MemoryStream ms = new MemoryStream(bs);
            Load(ms, format);
        }
        /// <summary>
        /// 以指定的格式从文件流中加载文档。
        /// </summary>
        /// <param name="stream">文件流对象</param>
        /// <param name="format">指定的格式</param>
        public void Load(System.IO.Stream stream, string format)
        {
            Load(stream, format, false);
        }
        /// <summary>
        /// 以指定的格式从文件流中加载文档。
        /// </summary>
        /// <param name="stream">文件流对象</param>
        /// <param name="format">指定的格式</param>
        /// <param name="fastMode">快速加载模式</param>
        public void Load(System.IO.Stream stream, string format, bool fastMode)
        {
            try
            {
                this.BeginLoadDocument();
                InnerDeserialize(null, stream, format, fastMode, DocumentLoadMode.Normal, null);
            }
            finally
            {
                this.EndLoadDocument();
            }
        }

        /// <summary>
        /// 以指定的格式从文件流中加载文档。
        /// </summary>
        /// <param name="reader">文本读取器</param>
        /// <param name="format">指定的格式</param>
        public void Load(System.IO.TextReader reader, string format)
        {
            try
            {
                this.BeginLoadDocument();
                InnerDeserialize(null, reader, format, false, DocumentLoadMode.Normal, null);
            }
            finally
            {
                this.EndLoadDocument();
            }
        }
        /// <summary>
        /// 以指定的格式从文件流中加载文档。
        /// </summary>
        /// <param name="reader">文本读取器</param>
        /// <param name="format">指定的格式</param>
        /// <param name="fastMode">快速加载模式</param>
        public void Load(System.IO.TextReader reader, string format, bool fastMode)
        {
            try
            {
                this.BeginLoadDocument();
                InnerDeserialize(null, reader, format, fastMode, DocumentLoadMode.Normal, null);
            }
            finally
            {
                this.EndLoadDocument();
            }
        }


        private enum DocumentLoadMode
        {
            Normal,
            ImportMode,
            AppendMode
        }

        [NonSerialized]
        internal DomElement _SrcElementForEventReadFileContent = null;

        [NonSerialized]
        private DateTime _NativeLoadedTime = DateTimeCommon.GetNow();// DateTime.Now;
        private void CleanContentDOMBeforeLoad()
        {
            int tick = Environment.TickCount;
            if (this._Elements != null)
            {
                foreach (var item in this._Elements.FastForEach())
                {
                    item.Dispose();
                }
                this._Elements.Clear();
            }
            if (this._ContentStyles != null)
            {
                foreach (var item in this._ContentStyles.Styles.FastForEach())
                {
                    item.Dispose();
                }
                this._ContentStyles.Styles.Clear();
            }
            this._HoverElement = null;
            //this._OutlineNodes = null;
            this._Pages = null;
            this._PageSettings = null;
            if (this._TypedElements != null)
            {
                this._TypedElements.Clear();
                this._TypedElements = null;
            }
            tick = Environment.TickCount - tick;
        }

        /// <summary>
        /// 文档反序列化
        /// </summary>
        /// <param name="fileSystemName">文件系统名称</param>
        /// <param name="source">文件来源</param>
        /// <param name="format">文件格式</param>
        /// <param name="fastMode">快速模式</param>
        /// <param name="loadMode">加载模式</param>
        /// <param name="instance">文档对象实例</param>
        /// <returns>加载生成的文档元素对象列表</returns>
        private DomElementList InnerDeserialize(
            string fileSystemName,
            object source,
            string format,
            bool fastMode,
            DocumentLoadMode loadMode,
            DomDocument instance)
        {
            this.InvalidateFixDomState();
            //System.Threading.Thread.Sleep(5000);
            CheckDisposed();
            this.SetReadyState(DomReadyStates.Loading);
            var _debugMode = this.Options.BehaviorOptions.DebugMode;
            if (_debugMode)
            {
                // 输出调试信息
                //tick = DCSoft.Common.CountDown.GetTickCountFloat();
                DCConsole.Default.WriteLine(string.Format(
                    DCSR.Loading_FileName_Format,
                    (fastMode ? "Fast " + source.ToString() : source.ToString()),
                    format ?? "xml"));
            }
            // 获得文件反序列化器
            var ser = DCSoft.Writer.Serialization.Xml.XMLContentSerializer.Instance;
            // 设置反序列化选项
            ContentSerializeOptions options = new ContentSerializeOptions();
            options.EnableDocumentSetting = true;
            options.WriterControl = this.EditorControl;
            options.FileName = this.FileName;
            options.FastMode = fastMode;
            DomDocument tempDocument = instance;
            if (tempDocument == null)
            {
                tempDocument = this;
            }
            if (loadMode == DocumentLoadMode.ImportMode || loadMode == DocumentLoadMode.AppendMode)
            {
                tempDocument = (DomDocument)this.Clone(false);
            }
            else if (instance != null)
            {
                instance.CopyContent(this, false);
            }
            tempDocument._NativeLoadedTime = DateTimeCommon.GetNow();// DateTime.Now; 

            if (source is TextReader)
            {
                // 文本读取器
                //if (this.EditorControl != null)
                //{
                //    WaittingUIManager.PaintMessage( 
                //        this.EditorControl,
                //        string.Format(WriterStringsCore.Loading_FileName, tempDocument.FileName));
                //} 
                tempDocument.CleanContentDOMBeforeLoad();
                ser.Deserialize((TextReader)source, tempDocument, options);
                this.ContentSourceType = DocumentContentSourceTypes.TextReader;
            }
            else if (source is DCSystemXml.XmlReader
                && ser is DCSoft.Writer.Serialization.Xml.XMLContentSerializer)
            {
                var ser2 = (DCSoft.Writer.Serialization.Xml.XMLContentSerializer)ser;
                tempDocument.CleanContentDOMBeforeLoad();
                ser2.Deserialize((DCSystemXml.XmlReader)source, tempDocument, options);
                this.ContentSourceType = DocumentContentSourceTypes.TextReader;
            }
            else if (source is Stream)
            {
                // 文件流
                //if (this.EditorControl != null)
                //{
                //     WaittingUIManager.PaintMessage( 
                //        this.EditorControl,
                //        string.Format( WriterStringsCore.Loading_FileName , tempDocument.FileName ));
                //} 
                tempDocument.CleanContentDOMBeforeLoad();
                ser.Deserialize((Stream)source, tempDocument, options);
                this.ContentSourceType = DocumentContentSourceTypes.Stream;
            }
            if (tempDocument._DocumentControler != null)
            {
                tempDocument._DocumentControler.Clear();
            }
            tempDocument._ContentProtectedInfos = null;
            _NativeLoadedTime = DateTimeCommon.GetNow();// DateTime.Now;
            tempDocument._NativeLoadedTime = _NativeLoadedTime;
            tempDocument._FileFormat = ser.Name;
            WriterEventArgs args2 = new WriterEventArgs(this.EditorControl, tempDocument, tempDocument);
            if (fastMode == false)
            {
                if (source is string)
                {
                    tempDocument.FileName = (string)source;
                }
                ElementLoadEventArgs args = new ElementLoadEventArgs(tempDocument, format);
                tempDocument.AfterLoad(args);
                tempDocument.Modified = false;

                if (tempDocument.UndoList != null)
                {
                    tempDocument.UndoList.Clear();
                }
            }
            if (_debugMode)
            {
                DCConsole.Default.WriteLine("Document loaded:" + tempDocument.Info.Title);
                DCConsole.Default.WriteLine("File name      :" + tempDocument.FileName);
                DCConsole.Default.WriteLine("Creation time  :" + DateTimeCommon.FastToYYYY_MM_DD_HH_MM_SS(tempDocument.Info.CreationTime));
                DCConsole.Default.WriteLine("Description    :" + tempDocument.Info.Description);
            }
            base._ContentVersion++;
            this._FileFormat = ser.Name;
            DomElementList list = tempDocument.Body.Elements;
            if (loadMode == DocumentLoadMode.ImportMode
                || loadMode == DocumentLoadMode.AppendMode)
            {
                list = tempDocument.Body.Elements;
                this.ImportElements(list);
                if (loadMode == DocumentLoadMode.AppendMode)
                {
                    this.Body.Elements.AddRangeByDCList(list);
                    this.Body.FixDomState();
                    this.Body.UpdateContentElements(false);
                    if (fastMode == false)
                    {
                        ElementLoadEventArgs args = new ElementLoadEventArgs(tempDocument, format);
                        this.AfterLoad(args);
                        this.Modified = false;

                        if (this.UndoList != null)
                        {
                            this.UndoList.Clear();
                        }
                    }
                }
            }
            return list;
        }

        #endregion

        #region 加载和保存文档内容的成员群 ************************************


        /// <summary>
        /// 以指定的格式将文档保存在文档文档中
        /// </summary>
        /// <param name="writer">文本书写器</param>
        /// <param name="format">文档格式</param>
        public void Save(TextWriter writer, string format)
        {
            this.Info.LastModifiedTime = this.GetNowDateTime();
            InnerSerialize(writer, format, false, null);
        }
        /// <summary>
        /// 以指定格式将文档保存而而生成文本
        /// </summary>
        /// <param name="format">文档格式</param>
        /// <returns>保存结果</returns>
        public string SaveToString(string format)
        {
            this.Info.LastModifiedTime = this.GetNowDateTime();
            System.IO.TextWriter writer = null;
            writer = new System.IO.StringWriter();
            InnerSerialize(writer, format, false, null);
            string xml = writer.ToString();
            writer.Dispose();
            return xml;
        }

        /// <summary>
        /// 以指定格式将文档保存为BASE64格式的字符串
        /// </summary>
        /// <param name="format">文档格式</param>
        /// <returns>保存的BASE64字符串</returns>
        public string SaveToBase64String(string format)
        {
            MemoryStream ms = new MemoryStream();
            this.Save(ms, format);
            byte[] bs = ms.ToArray();
            return Convert.ToBase64String(bs);
        }

        /// <summary>
        /// 以指定的格式将文档保存在文件流中。
        /// </summary>
        /// <param name="stream">文件流</param>
        /// <param name="format">文件格式</param>
        public void Save(System.IO.Stream stream, string format)
        {
            InnerSerialize(stream, format, false, null);
        }
        internal void InnerSerialize(
           object output,
           string format,
           bool backgroundMode,
           string descFileName)
        {
            InnerSerialize(output, format, backgroundMode, descFileName, null);
        }


        internal void InnerSerialize(
            object output,
            string format,
            bool backgroundMode,
            string descFileName,
            System.Text.Encoding contentEncoding)
        {
            this.ClearCachedOptions();

            bool needRefreshDocument = false;
            if (this.InvalidateLayoutFast)
            {
                needRefreshDocument = true;
                this.RefreshInnerView(false);
            }
            CheckDisposed();
            var bopts = this.Options.BehaviorOptions;
            if (bopts.CloneSerialize)
            {
                if (string.Compare(format, "pdf", true) != 0
                    && string.Compare(format, "ofd", true) != 0
                    && string.Compare(format, "json", true) != 0)
                {
                    using (DomDocument document = (DomDocument)this.Clone(true))
                    {
                        document._EditorControl = this._EditorControl;
                        //document.RefreshSizeWithoutParamter();
                        //document.ExecuteLayout();
                        //document.RefreshPages();
                        if (this._SerializeOptionsOnce != null)
                        {
                            document._SerializeOptionsOnce = this._SerializeOptionsOnce.Clone();
                            this._SerializeOptionsOnce = null;
                        }
                        document.PrivateSerialize(
                            output,
                            format,
                            backgroundMode,
                            descFileName,
                            contentEncoding);
                        if (backgroundMode == false)
                        {
                            this.Modified = false;
                            this.RaiseSelectionChangedEvent();
                            //this.OnSelectionChanged();
                        }
                    }
                    if (needRefreshDocument || this.InvalidateLayoutFast)
                    {
                        this.RefreshInnerView(true);
                        if (this.WriterControl != null)
                        {
                            this.WriterControl.UpdatePages();
                        }
                    }
                    return;
                }
            }
            this.PrivateSerialize(
                output,
                format,
                backgroundMode,
                descFileName,
                contentEncoding);
            if (needRefreshDocument || this.InvalidateLayoutFast)
            {
                this.RefreshInnerView(true);
                if (this.WriterControl != null)
                {
                    this.WriterControl.UpdatePages();
                }
            }
        }


        /// <summary>
        /// 只使用一次的序列化设置
        /// </summary>
        [System.NonSerialized]
        private ContentSerializeOptions _SerializeOptionsOnce = null;
        private void PrivateSerialize(
            object output,
            string format,
            bool backgroundMode,
            string descFileName,
            System.Text.Encoding contentEncoding)
        {
            CheckDisposed();
            if (output == null)
            {
                throw new ArgumentNullException("output");
            }
            var ser = DCSoft.Writer.Serialization.Xml.XMLContentSerializer.Instance;
            if (ser is DCSoft.Writer.Serialization.Xml.XMLContentSerializer)
            {
                // 无限制
            }

            if (ser != null)
            {
                if (output is string || output is Stream)
                {
                    // 文件名或文件流
                    if ((ser.Flags & ContentSerializerFlags.SupportSerialize)
                        != ContentSerializerFlags.SupportSerialize)
                    {
                        ser = null;
                        throw new NotSupportedException(string.Format(DCSR.NotSupportSerialize_Format, format));
                    }
                }
                else if (output is TextWriter)
                {
                    // 文本书写器
                    if ((ser.Flags & ContentSerializerFlags.SupportSerializeText)
                        != ContentSerializerFlags.SupportSerializeText)
                    {
                        ser = null;
                        throw new NotSupportedException(string.Format(DCSR.NotSupportSerializeText_Format, format));
                    }
                }
                else
                {
                    // 不支持其他对象
                    ser = null;
                }
            }
            if (ser == null)
            {
                throw new NotSupportedException(
                    string.Format(DCSR.NotSupportWrite_Format, format));
            }
            if (string.IsNullOrEmpty(format))
            {
                format = ser.Name;
            }



            ContentSerializeOptions options = this._SerializeOptionsOnce;
            this._SerializeOptionsOnce = null;
            if (options == null)
            {
                options = new ContentSerializeOptions();
            }
            if (string.IsNullOrEmpty(descFileName))
            {
                options.FileName = this.FileName;
            }
            else
            {
                options.FileName = descFileName;
            }
            options.EnableDocumentSetting = true;
            options.IncludeSelectionOnly = false;
            options.SerializeAttachFiles = true;
            options.FastMode = false;
            options.Formated = this.Options.BehaviorOptions.OutputFormatedXMLSource;

            if (contentEncoding != null)
            {
                options.ContentEncoding = contentEncoding;
            }
            try
            {
                if (ser.NeedPrepareSerialize(this))
                {
                    this.PrepareSerialize(format);
                }
                if (ser.NeedRefreshPages(this))
                {
                    this.CheckPageRefreshed();
                    //if (this.PageRefreshed == false)
                    //{
                    //    this.RefreshSizeWithoutParamter();
                    //    this.ExecuteLayout();
                    //    this.RefreshPages();
                    //}
                }
                DomDocument._DocumentSerializing = true;
                if (output is Stream)
                {
                    ser.Serialize((Stream)output, this, options);
                }
                else if (output is TextWriter)
                {
                    TextWriter writer = (TextWriter)output;
                    if (writer is System.IO.StreamWriter)
                    {
                        StreamWriter sw = (StreamWriter)writer;
                        options.ContentEncoding = sw.Encoding;
                    }
                    ser.Serialize(writer, this, options);
                }
                if (ser.NeedAfterSerialize(this))
                {
                    this.AfterSerialize();
                }
            }
            finally
            {
                DomDocument._DocumentSerializing = false;
            }
            if (backgroundMode == false)
            {
                this.Modified = false;
                this.RaiseSelectionChangedEvent();
                //this.OnSelectionChanged();
            }
        }

        private static readonly string _pdf = "pdf";
        private static readonly string _xml = "xml";
        /// <summary>
        /// 准备执行序列化操作
        /// </summary>
        /// <param name="format">文件格式</param>

        public override void PrepareSerialize(string format)
        {
            //this._SpecluateBinaryLength = 1024;
            if (string.Compare(format, _pdf, true) == 0 || string.Compare(format, "ofd", true) == 0)
            {
                // PDF格式不做处理
                return;
            }
            this.EditorVersionString = DomDocument.CurrentEditorVersionString;
            //this._ElementsForSerialize = null;
            this._Deserializing = false;
            if (this.Options.BehaviorOptions.SpecifyDebugMode == false)
            {
                // 不处于特别调试模式，压缩样式信息列表
                this.CompressStyleList();
            }
            this.ContentStyles.Styles.UpdateStyleIndex();
            //this._SpecluateBinaryLength += this.ContentStyles.Styles.Count * 100;
            this.FixDomStateWithCheckInvalidateFlag();
            if (string.IsNullOrEmpty(format) || string.Compare(format, _xml, true) == 0)
            {

            }
            else
            {
                foreach (DomElement element in this.Elements)
                {
                    DomDocumentContentElement dce = element as DomDocumentContentElement;
                    if (dce != null)
                    {
                        dce.RefreshParagraphListState(true, true);
                    }
                }
            }
            if (this.UndoList != null)
            {
                this.UndoList.Clear();
            }
            if (string.IsNullOrEmpty(format) || string.Compare(format, _xml, true) == 0)
            {
                //this._ElementsForSerialize = new XTextElementList();
                foreach (DomElement element in this.Elements.FastForEach())
                {
                    element.PrepareSerialize(format);
                }
                //this._ElementsForSerializeCount = 0;
            }
        }

        /// <summary>
        /// 完成序列化操作
        /// </summary>
        public void AfterSerialize()
        {
            //base.AfterSerialize();
            this.FixDomState();
        }

        /// <summary>
        /// 正在进行反序列化操作的标记
        /// </summary>
        [NonSerialized]
        internal bool _Deserializing = true;

        /// <summary>
        /// 从另外一个文本文档复制文档内容
        /// </summary>
        /// <param name="sourceDocument">文档内容来源</param>
        /// <param name="copyElements">是否复制元素</param>
        /// <remarks>
        /// 本方法一般用于执行XML反序列化时，将文档内容从临时文档复制到本文档。
        /// </remarks>

        public void CopyContent(DomDocument sourceDocument, bool copyElements, bool moveData = false)
        {
            this.ResetChildElementStats();
            this._CurrentContentElement = null;
            this._CurrentPage = null;
            this._CurrentStyleInfo = null;
            this._HoverElement = null;
            if (sourceDocument != null)
            {
                if (copyElements)
                {
                    // 复制数据
                    if (sourceDocument.Elements != null)
                    {
                        if (moveData)
                        {
                            // 完整的移动数据
                            this._Elements = sourceDocument.Elements;
                            sourceDocument.Elements = null;
                        }
                        else
                        {
                            this.Elements.Clear();
                            foreach (DomElement element in sourceDocument.Elements)
                            {
                                this.Elements.Add(element);
                            }
                        }
                    }
                    if (sourceDocument._ContentStyles != null)
                    {
                        if (moveData)
                        {
                            this._ContentStyles = sourceDocument._ContentStyles;
                            sourceDocument._ContentStyles = null;
                        }
                        else
                        {
                            this._ContentStyles = (DocumentContentStyleContainer)sourceDocument._ContentStyles.Clone();
                            this._ContentStyles.Document = this;
                        }
                    }
                }
                this.FileName = sourceDocument.FileName;
                this.FileFormat = sourceDocument.FileFormat;
                this._HighlightManager = null;
                this._ObjectIDIncrease = 0;
                this._CurrentStyleInfo = null;
                this._CurrentContentElement = null;
                this._HoverElement = null;
                this._DocumentControler = null;
                this._CurrentStyleInfo = null;
                this._GlobalPages = null;
                this._MouseCapture = null;
                this.PageRefreshed = false;
                this._Pages = null;
                this._CurrentPage = null;
                if (sourceDocument.Info != null)
                {
                    if (moveData)
                    {
                        this._Info = sourceDocument._Info;
                        sourceDocument._Info = null;
                    }
                    else
                    {
                        this.Info = sourceDocument.Info.Clone();
                    }
                }
                if (sourceDocument.PageSettings != null)
                {
                    if (moveData)
                    {

                        this._PageSettings = sourceDocument._PageSettings;
                        sourceDocument._PageSettings = null;
                    }
                    else
                    {
                        this._PageSettings = sourceDocument._PageSettings.Clone();
                    }
                }
            }
        }

        #endregion
    }
}
