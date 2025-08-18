using DCSoft.Writer.Dom;
using System.Collections.Generic;
using System.Text.Json;
using System.Text;
//using Microsoft.Extensions.Options;

namespace DCSoft.WASM
{


    /// <summary>
    /// 插入文档的参数
    /// </summary>
    [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
    public class InsertDocumentArgs
    {
        public InsertDocumentArgs()
        {
        }
        private string _FileContent = null;
        /// <summary>
        /// 文件内容
        /// </summary>
        public string FileContent
        {
            get { return this._FileContent; }
            set { this._FileContent = value; }
        }
        private string _FileFormat = null;
        /// <summary>
        /// 文件格式
        /// </summary>
        public string FileFormat
        {
            get
            {
                return this._FileFormat;
            }
            set
            {
                this._FileFormat = value;
            }
        }
        private int _ClearFormatType = 0;
        /// <summary>
        /// 清空内容格式的模式
        /// </summary>
        public int ClearFormatType
        {
            get { return this._ClearFormatType; }
            set { this._ClearFormatType = value; }
        }
        private bool _CheckImportDocument = false;
        /// <summary>
        /// 是否进行导入文档的检查
        /// </summary>
        public bool CheckImportDocument
        {
            get { return this._CheckImportDocument; }
            set { this._CheckImportDocument = value; }
        }
        private bool _ShowUI = true;
        /// <summary>
        /// 是否显示用户界面
        /// </summary>
        public bool ShowUI
        {
            get { return this._ShowUI; }
            set { this._ShowUI = value; }
        }
    }
    public partial class WriterControlForWASM
    {
        private string _NotSupportFontNames = null;
        /// <summary>
        /// 获得最后一次加载的文档中不支持的字体名称列表，各个名称之间用逗号分开
        /// </summary>
        /// <returns>字体名称列表</returns>
        [JSInvokable]
        public string GetNotSupportFontNames()
        {
            try
            {
                return this._NotSupportFontNames;
            }
            catch (Exception ex)
            {
                JavaScriptMethods.Tools_ReportException("GetNotSupportFontNames", ex.Message, ex.ToString(), 0);
                return "";
            }
        }
        /// <summary>
        /// 检查文档中不支持的字体名称
        /// </summary>
        /// <param name="document">文档对象</param>
        private void CheckNotSupportFontNames(DomDocument document)
        {
            this._NotSupportFontNames = null;
            if (document != null && document.ContentStyles != null)
            {
                var str = new StringBuilder();
                foreach (DocumentContentStyle style in document.ContentStyles.Styles)
                {
                    var fn = style.FontName;
                    if (JavaScriptMethods.Tools_IsSupportFontName(fn) == false
                        || DCSoft.TrueTypeFontSnapshort.Support(fn) == false)
                    {
                        if (str.Length > 0)
                        {
                            str.Append(',');
                        }
                        str.Append(fn);
                    }
                }
                if (str.Length > 0)
                {
                    this._NotSupportFontNames = str.ToString();
                }
            }
        }

        /// <summary>
        /// 以指定的格式从BASE64字符串加载文档内容
        /// </summary>
        /// <param name="text">BASE64字符串</param>
        /// <param name="format">文件格式</param>
        /// <returns>操作是否成功</returns>
        [JSInvokable]
        public bool LoadDocumentFromBase64String(string strData, string strformat, string specifyLoadPart)
        {
            CheckStateBeforeInvoke();
            try
            {
                var format = this._Control.DetectFileFormat(strformat);
                if (format == null)
                {
                    format = strformat;
                }
                //wyc20230621:处理四代兼容接口参数specifyLoadPart的分部加载功能
                string loadPart = specifyLoadPart != null ? specifyLoadPart.ToLower() : string.Empty;
                if (loadPart != "header" &&
                   loadPart != "footer" &&
                   loadPart != "headerfooter" &&
                   loadPart != "body")
                {
                    this._ElementNativeHandles?.Clear();
                    var result = this._Control.LoadDocumentFromBase64String(strData, format);
                    this.CheckNotSupportFontNames(this._Control.Document);
                    return result;
                }

                //XTextDocument mainDoc = new XTextDocument();
                //mainDoc.LoadFromString(this._Control.SaveDocumentToString("xml"), "xml");

                DomDocument tempdoc = new DomDocument();
                tempdoc.LoadFromBase64String(strData, format);
                this.CheckNotSupportFontNames(tempdoc);
                var mainDoc = this._Control.Document;
                switch (loadPart)
                {
                    case "header":
                        mainDoc.ImportElements(tempdoc.Header.Elements);
                        mainDoc.Header.Elements.Clear();
                        mainDoc.Header.Elements.AddRangeByDCList(tempdoc.Header.Elements);
                        break;
                    case "footer":
                        mainDoc.ImportElements(tempdoc.Footer.Elements);
                        mainDoc.Footer.Elements.Clear();
                        mainDoc.Footer.Elements.AddRangeByDCList(tempdoc.Footer.Elements);
                        break;
                    case "headerfooter":
                        mainDoc.ImportElements(tempdoc.Header.Elements);
                        mainDoc.ImportElements(tempdoc.Footer.Elements);
                        mainDoc.Header.Elements.Clear();
                        mainDoc.Header.Elements.AddRangeByDCList(tempdoc.Header.Elements);
                        mainDoc.Footer.Elements.Clear();
                        mainDoc.Footer.Elements.AddRangeByDCList(tempdoc.Footer.Elements);
                        break;
                    case "body":
                        mainDoc.ImportElements(tempdoc.Body.Elements);
                        mainDoc.Body.Elements.Clear();
                        mainDoc.Body.Elements.AddRangeByDCList(tempdoc.Body.Elements);
                        break;
                    default:
                        break;
                }
                //this.RefreshDocument();
                this._ElementNativeHandles?.Clear();
                //return true;
                bool res = this._Control.LoadDocumentFromString(mainDoc.SaveToString("xml"), "xml");
                return res;
            }
            catch (Exception ex)
            {
                JavaScriptMethods.Tools_ReportException("LoadDocumentFromString", ex.Message, ex.ToString(), 0);
                return false;
            }
        }
        /// <summary>
        /// 加载二进制数组内容
        /// </summary>
        /// <param name="bs"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        [JSInvokable]
        public bool LoadDocumentFromBinary(byte[] bs, string format)
        {
            CheckStateBeforeInvoke();
            try
            {
                this._ElementNativeHandles?.Clear();
                var result = this._Control.LoadDocumentFromBinary(bs, format);
                this.CheckNotSupportFontNames(this._Control.Document);
                return result;
            }
            catch (Exception ex)
            {
                JavaScriptMethods.Tools_ReportException("LoadDocumentFromBinary", ex.Message, ex.ToString(), 0);
                return false;
            }
        }

        private static List<byte[]> _BianryDatasForXmlReader = null;
        [JSInvokable]
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public void ClearInnerXmlReader()
        {
            CheckStateBeforeInvoke();
            try
            {
                _BianryDatasForXmlReader = null;
                _StringTableForXmlReader = null;
            }
            catch (Exception ex)
            {
                JavaScriptMethods.Tools_ReportException("ClearInnerXmlReader", ex.Message, ex.ToString(), 0);
                return;
            }
        }
        [JSInvokable]
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public void AddBianryDataForXmlReader(byte[] bs)
        {
            CheckStateBeforeInvoke();
            try
            {
                if (_BianryDatasForXmlReader == null)
                {
                    _BianryDatasForXmlReader = new List<byte[]>();
                }
                _BianryDatasForXmlReader.Add(bs);
            }
            catch (Exception ex)
            {
                JavaScriptMethods.Tools_ReportException("AddBianryDataForXmlReader", ex.Message, ex.ToString(), 0);
                return;
            }
        }
        private static string[] _StringTableForXmlReader = null;
        [JSInvokable]
        public void SetStringTableForXmlReader(string[] strTable)
        {
            CheckStateBeforeInvoke();
            _StringTableForXmlReader = strTable;
        }

        public static DCSoft.Writer.Serialization.ArrayXmlReader CreateInnerXmlReader()
        {
            if (_BianryDatasForXmlReader != null
                && _BianryDatasForXmlReader.Count >= 2
                && _StringTableForXmlReader != null
                && _StringTableForXmlReader.Length > 0)
            {
                var bs1 = _BianryDatasForXmlReader[0];
                var bs2 = _BianryDatasForXmlReader[1];
                _BianryDatasForXmlReader.RemoveAt(0);
                _BianryDatasForXmlReader.RemoveAt(0);
                if (_BianryDatasForXmlReader.Count == 0)
                {
                    _BianryDatasForXmlReader = null;
                }
                var reader = new DCSoft.Writer.Serialization.ArrayXmlReader(
                    _StringTableForXmlReader,
                    bs1,
                    bs2,
                    _BianryDatasForXmlReader);
                _BianryDatasForXmlReader = null;
                _StringTableForXmlReader = null;
                return reader;
            }
            else
            {
                return null;
            }
        }

        ////判断加载文档后是否执行表达式
        //internal static bool IsExecuteAllValueExpressionAfterLoad = false;

        [JSInvokable]
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public bool LoadDocumentFromInnerXmlReader()
        {
            CheckStateBeforeInvoke();
            try
            {
                _Current = this;
                var reader = CreateInnerXmlReader();
                if (reader == null)
                {
                    return false;
                }
                try
                {
                    var result = this._Control.LoadDocumentFromXmlReader(reader, "xml");
                    //if (IsExecuteAllValueExpressionAfterLoad)
                    //{//加载文档后执行表达式
                    //    this._Control.Document.ExecuteAllValueExpression();
                    //}
                    this.CheckNotSupportFontNames(this._Control.Document);
                    reader.Close();
                    this._ElementNativeHandles?.Clear();
                    return result;
                }
                catch (System.Exception ext)
                {
                    DCConsole.Default.WriteLine(ext.ToString());
                    return false;
                }
            }
            catch (Exception ex)
            {
                JavaScriptMethods.Tools_ReportException("LoadDocumentFromInnerXmlReader", ex.Message, ex.ToString(), 0);
                return false;
            }
            finally
            {
                _Current = null;
            }
        }

        /// <summary>
        /// 以指定的格式加载文本文档内容
        /// </summary>
        /// <param name="text">文本</param>
        /// <param name="format">格式</param>
        /// <returns>操作是否成功</returns>
        [JSInvokable]
        public bool LoadDocumentFromString(string strData, string strFormat, string specifyLoadPart)
        {
            CheckStateBeforeInvoke();
            try
            {
                _Current = this;
                var format = this._Control.DetectFileFormat(strData);

                //wyc20241021:前端直接加载text格式的纯文本内容文档会报错DUWRITER5_0-3738
                if (format == null && strFormat == "text" && strData != null)
                {
                    format = strFormat;
                }


                //wyc20230621:处理四代兼容接口参数specifyLoadPart的分部加载功能
                string loadPart = specifyLoadPart != null ? specifyLoadPart.ToLower() : string.Empty;
                if (loadPart != "header" &&
                   loadPart != "footer" &&
                   loadPart != "headerfooter" &&
                   loadPart != "body")
                {
                    this._ElementNativeHandles?.Clear();
                    var result = this._Control.LoadDocumentFromString(strData, format);
                    this.CheckNotSupportFontNames(this._Control.Document);
                    return result;
                }

                //XTextDocument mainDoc = new XTextDocument();
                //mainDoc.LoadFromString(this._Control.SaveDocumentToString("xml"), "xml");

                DomDocument tempdoc = new DomDocument();
                tempdoc.LoadFromString(strData, format);
                this.CheckNotSupportFontNames(tempdoc);
                var mainDoc = this._Control.Document;
                switch (loadPart)
                {
                    case "header":
                        mainDoc.ImportElements(tempdoc.Header.Elements);
                        mainDoc.Header.Elements.Clear();
                        mainDoc.Header.Elements.AddRangeByDCList(tempdoc.Header.Elements);
                        break;
                    case "footer":
                        mainDoc.ImportElements(tempdoc.Footer.Elements);
                        mainDoc.Footer.Elements.Clear();
                        mainDoc.Footer.Elements.AddRangeByDCList(tempdoc.Footer.Elements);
                        break;
                    case "headerfooter":
                        mainDoc.ImportElements(tempdoc.Header.Elements);
                        mainDoc.ImportElements(tempdoc.Footer.Elements);
                        mainDoc.Header.Elements.Clear();
                        mainDoc.Header.Elements.AddRangeByDCList(tempdoc.Header.Elements);
                        mainDoc.Footer.Elements.Clear();
                        mainDoc.Footer.Elements.AddRangeByDCList(tempdoc.Footer.Elements);
                        break;
                    case "body":
                        mainDoc.ImportElements(tempdoc.Body.Elements);
                        mainDoc.Body.Elements.Clear();
                        mainDoc.Body.Elements.AddRangeByDCList(tempdoc.Body.Elements);
                        break;
                    default:
                        break;
                }
                //this.RefreshDocument();
                this._ElementNativeHandles?.Clear();
                //return true;
                bool res = this._Control.GetInnerViewControl().LoadDocumentFromDocument(mainDoc);
                return res;
            }
            catch (Exception ex)
            {
                JavaScriptMethods.Tools_ReportException("LoadDocumentFromString", ex.Message, ex.ToString(), 0);
                return false;
            }
            finally
            {
                _Current = null;
            }
        }

        private GetTextArgs InnerGetTextArgs(GetTextArgs textArgs, JsonElement options)
        {
            if (options.ValueKind != JsonValueKind.Object)
            {

            }
            else
            {
                foreach (JsonProperty property in options.EnumerateObject())
                {
                    string name = property.Name.ToLower();
                    switch (name)
                    {
                        case "includebackgroundtext":
                            textArgs.IncludeBackgroundText = WASMUtils.ConvertToBoolean(property, true);
                            break;
                        case "includehiddentext":
                            textArgs.IncludeHiddenText = WASMUtils.ConvertToBoolean(property, true);
                            break;
                        default:
                            break;
                    }
                }
            }
            return textArgs;
        }


        public static string PackageBigString(string v)
        {
            if (v != null && v.Length > 1024 * 1024)
            {
                return JavaScriptMethods.PackageBigStringValue(v);
            }
            else
            {
                return v;
            }
        }

        private string InnerSaveDocumentToText(DomElementList list, JsonElement options)
        {
            string strResult = "";

            GetTextArgs textArgs = new GetTextArgs();
            textArgs = InnerGetTextArgs(textArgs, options);

            bool IncludeCheck = true;//是否包含未选择的单选框或复选框，为兼容之前的设置，默认值为true，包含

            if (options.ValueKind == JsonValueKind.Object)
            {
                foreach (JsonProperty property in options.EnumerateObject())
                {
                    string name = property.Name.ToLower();
                    switch (name)
                    {
                        case "includecheck":
                            IncludeCheck = WASMUtils.ConvertToBoolean(property, true);
                            break;
                        default:
                            break;
                    }
                }
            }

            for (int i = 0; i < list.Count; i++)
            {
                DomElement childElement = list[i];
                {
                    if (IncludeCheck == false)
                    {//不包含未选择的单选框或复选框
                        if (childElement is DomRadioBoxElement)
                        {//单选框
                            DomRadioBoxElement element = childElement as DomRadioBoxElement;
                            if (element.Checked == false)
                            {
                                continue;
                            }
                        }
                        if (childElement is DomCheckBoxElement)
                        {//复选框
                            DomCheckBoxElement element = childElement as DomCheckBoxElement;
                            if (element.Checked == false)
                            {
                                continue;
                            }
                        }
                    }
                    {
                        strResult += (childElement.GetText(textArgs));
                    }

                }

            }
            return strResult;
        }

        /// <summary>
        /// 保存文档的文本内容
        /// </summary>
        /// <param name="parameter"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        [JSInvokable]
        public string SaveDocumentToText(JsonElement options)
        {
            CheckStateBeforeInvoke();
            try
            {
                DomDocument document = this._Control.Document.Clone(true) as DomDocument;
                if (options.ValueKind == JsonValueKind.Null)
                {
                    var strResult1 = PackageBigString(document.SaveToString("text"));
                    return strResult1;
                }
                else
                {
                    string headertxt = InnerSaveDocumentToText(document.Header.Elements, options);
                    string bodytxt = InnerSaveDocumentToText(document.Body.Elements, options);
                    string footertxt = InnerSaveDocumentToText(document.Footer.Elements, options);
                    string resultstr = string.Empty;
                    if (headertxt != null && headertxt.Trim().Length > 0)
                    {
                        resultstr = resultstr + headertxt + Environment.NewLine;
                    }
                    resultstr = resultstr + bodytxt;
                    if (footertxt != null && footertxt.Trim().Length > 0)
                    {
                        resultstr = resultstr + Environment.NewLine + footertxt;
                    }
                    return resultstr;
                    //return InnerSaveDocumentToText(document.Body.Elements, options);
                }
            }
            catch (Exception ex)
            {
                JavaScriptMethods.Tools_ReportException("SaveDocumentToText", ex.Message, ex.ToString(), 0);
                return "";
            }
        }


        /// <summary>
        /// 保存文档为字符串
        /// </summary>
        /// <param name="format">文件格式</param>
        /// <returns>输出的字符串</returns>
        [JSInvokable]
        public string SaveDocumentToString(JsonElement parameter)
        {
            CheckStateBeforeInvoke();
            try
            {
                if (parameter.ValueKind == JsonValueKind.String)
                {
                    return this._Control.SaveDocumentToString(parameter.GetString());
                }
                else if (parameter.ValueKind == JsonValueKind.Null || parameter.ValueKind == JsonValueKind.Undefined)
                {
                    return PackageBigString(this._Control.SaveDocumentToString("xml"));
                }
                /////////////////////////////////////////////

                DomDocument document = this._Control.Document.Clone(true) as DomDocument;
                document.Options = this._Control.DocumentOptions.Clone();
                /////////////////////////////////////////////////////////////////////////
                //var tick44 = Environment.TickCount;
                if (parameter.ValueKind == JsonValueKind.String)
                {
                    string formatstr = parameter.GetString();
                    if (formatstr != "xml" && formatstr != "json")
                    {
                        document.RefreshDocument();
                    }
                        var strResult = PackageBigString(document.SaveToString(formatstr));
                        //Console.WriteLine("4444444 " + (Environment.TickCount - tick44));
                        return strResult;
                }
                if (parameter.ValueKind != JsonValueKind.Object)
                {
                    var strResult = PackageBigString(document.SaveToString("xml"));
                    //Console.WriteLine("4444444 " + (Environment.TickCount - tick44));
                    return strResult;
                }

                string format = "xml";
                bool outputformatxml = true;
                //bool outputformatxmlback = this._Control.DocumentOptions.BehaviorOptions.OutputFormatedXMLSource;
                string encodename = null; //暂时不处理这个了
                bool savebase64 = false;
                string specifysavepart = null;
                bool cleardatabinding = false;

                foreach (JsonProperty property in parameter.EnumerateObject())
                {
                    string name = property.Name.ToLower();
                    switch (name)
                    {
                        case "fileformat":
                            if (property.Value.ValueKind == JsonValueKind.String)
                            {
                                format = property.Value.GetString();
                            }
                            break;
                        case "outputformatxml":
                            outputformatxml = WASMUtils.ConvertToBoolean(property, true);// WASMJsonConvert.parseBoolean(property.Value, true);
                            break;
                        case "encodingname":
                            if (property.Value.ValueKind == JsonValueKind.String)
                            {
                                encodename = property.Value.GetString();
                            }
                            break;
                        case "savebase64string":
                            savebase64 = WASMUtils.ConvertToBoolean(property, false);// WASMJsonConvert.parseBoolean(property.Value, false);
                            break;
                        case "specifysavepart":
                            if (property.Value.ValueKind == JsonValueKind.String)
                            {
                                specifysavepart = property.Value.GetString();
                            }
                            break;
                        case "cleardatabindingcontent":
                            cleardatabinding = WASMUtils.ConvertToBoolean(property, false);// WASMJsonConvert.parseBoolean(property.Value, false);
                            break;
                        case "":
                            break;
                        default:
                            break;
                    }
                }
                if (specifysavepart != null)
                {
                    bool savefooter = true;
                    bool savebody = true;
                    bool saveheader = true;
                    switch (specifysavepart.ToLower())
                    {
                        case "header":
                            savebody = false;
                            savefooter = false;
                            break;
                        case "footer":
                            savebody = false;
                            saveheader = false;
                            break;
                        case "headerfooter":
                            savebody = false;
                            break;
                        case "body":
                            savefooter = false;
                            saveheader = false;
                            break;
                        default:
                            break;
                    }
                    if (saveheader == false)
                    {
                        document.Header.Elements.Clear();
                    }
                    if (savefooter == false)
                    {
                        document.Footer.Elements.Clear();
                    }
                    if (savebody == false)
                    {
                        document.Body.Elements.Clear();
                    }
                }
                document.Options.BehaviorOptions.OutputFormatedXMLSource = outputformatxml;
                //wyc20240628:替换refreshdocument，避免公式计算结果被洗掉
                document.RefreshInnerView(false);
                string result = null;
                    result = savebase64 == true ? document.SaveToBase64String(format) : document.SaveToString(format);
                document.Dispose();
                return result;
            }
            catch (Exception ex)
            {
                JavaScriptMethods.Tools_ReportException("SaveDocumentToString", ex.Message, ex.ToString(), 0);
                return "";
            }
        }
        /// <summary>
        /// 保存文档为BASE64字符串
        /// </summary>
        /// <param name="strFormat">文件格式</param>
        /// <returns>输出的BASE64字符串 </returns>
        [JSInvokable]
        public string SaveDocumentToBase64String(string format)
        {
            CheckStateBeforeInvoke();
            try
            {
                return this._Control.SaveDocumentToBase64String(format);
            }
            catch (Exception ex)
            {
                JavaScriptMethods.Tools_ReportException("SaveDocumentToBase64String", ex.Message, ex.ToString(), 0);
                return "";
            }
        }
    }
}
