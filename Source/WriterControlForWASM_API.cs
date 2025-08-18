using DCSoft.Common;
using DCSoft.Drawing;
using DCSoft.Printing;
using DCSoft.Writer;
using DCSoft.Writer.Commands;
using DCSoft.Writer.Controls;
using DCSoft.Writer.Data;
using DCSoft.Writer.Dom;
using System.Collections.Generic;
using System.Text.Json.Nodes;
using System.Text.Json;
using System.Text;
using DCSoft.Writer.Serialization;
using System.IO;

namespace DCSoft.WASM
{
    /// <summary>
    /// 此处对 WriterControl的接口进行转发调用
    /// 对应的，需要在前端 WriterControl_Main.js中的 BindControl()中添加对应的调用。
    /// </summary>
    partial class WriterControlForWASM
    {
        [JSInvokable]
        public bool ApplyDocumentOptions(DCSoft.Writer.DocumentOptions options)
        {
            CheckStateBeforeInvoke();
            try
            {
                bool result = false;
                try
                {
                    //if (options. .ValueKind == JsonValueKind.Object)
                    {
                        //string str = options.ToString();
                        this._Control.DocumentOptions = options;// WASMJsonConvert.parseJSONToDocumentOptions(options);
                        result = true;
                    }
                    //else
                    //{
                    //    return false;
                    //}
                }
                catch { result = false; }
                return result;
                //return this._Control.DocumentOptions;
            }
            catch (Exception ex)
            {
                JavaScriptMethods.Tools_ReportException("ApplyDocumentOptions", ex.Message, ex.ToString(), 0);
                return false;
            }
        }

        /// <summary>
        /// 获取当前元素的类型名称
        /// </summary>
        /// <returns></returns>
        [JSInvokable]
        public string GetCurrentElementTypeName()
        {
            try
            {
                var element = this._Control.Document.CurrentElement;
                if (this._Control.Document.Selection.ContentElements.Count == 1)
                {
                    element = this._Control.Document.Selection.ContentElements[0];
                }
                bool targetting = false;
                while (targetting == false)
                {
                    if (element != null)
                    {
                        if( element is DomInputFieldElement
                            || element is DomCheckBoxElementBase
                            || element is DomImageElement
                            || element is DomPageInfoElement
                            || element is DomTableCellElement
                            || element is DomTableElement
                            || element is DomTableRowElement
                            || element is DomTableColumnElement)
                        {
                            return element.TypeName.ToLower();
                        }
                        else
                        {
                            if (element is DomDocumentContentElement)
                            {
                                return string.Empty;
                            }
                            else
                            {
                                element = element.Parent;
                            }
                        }
                    }
                    else
                    {
                        return string.Empty;
                    }
                }
                return string.Empty;
            }
            catch (Exception ex)
            {
                JavaScriptMethods.Tools_ReportException("GetCurrentElementTypeName", ex.Message, ex.ToString(), 0);
                return string.Empty;
            }
        }


        /// <summary>
        /// 获得当前文档编辑状态,将一些常用信息集成到一个JSON对象中，减少JS调用C#函数的次数，提高程序性能。
        /// </summary>
        /// <returns>状态信息</returns>
        [JSInvokable]
        public JsonObject GetCurrentContentInfo()
        {
            var ctl = this._Control.GetInnerViewControl();
            var result = new JsonObject();
            result["PageCount"] = ctl.Document.Pages.Count;
            result["InsertMode"] = ctl.InsertMode;
            result["FocusedPageIndexBase0"] = ctl.FocusedPageIndexBase0;
            result["CurrentLineIndexInPage"] = ctl.CurrentLineIndexInPage();
            result["CurrentColumnIndex"] = ctl.CurrentColumnIndex();
            result["SelectionLength"] = ctl.Selection.Length;
            result["SelectionStartPosition"] = this.SelectionStartPosition;
            var info = this._Control.Document.CurrentStyleInfo.Content;
            result["CurrentFontName"] = info.FontName;
            result["CurrentFontSize"] = info.FontSize;
            result["CurrentBold"] = (info.FontStyle & FontStyle.Bold) == FontStyle.Bold;
            result["CurrentItalic"] = (info.FontStyle & FontStyle.Italic) == FontStyle.Italic;
            result["CurrentUnderline"] = (info.FontStyle & FontStyle.Underline) == FontStyle.Underline;
            result["TextColor"] = info.Color.ToHtmlString();
            result["BackColor"] = info.BackgroundColor.ToHtmlString();
            result["InsertMode"] = this._Control.InsertMode;
            return result;
        }

        /// <summary>
        /// 获得命令的状态
        /// </summary>
        /// <param name="commandName">命令名称</param>
        /// <returns>状态信息对象</returns>
        [JSInvokable]
        public JsonObject GetCommandStatus(string commandName)
        {
            //#if LightWeight
            //            throw new LightWeightNotSupportedException("GetCommandStatus");
            //#else
            try
            {
                var bolSuported = false;
                var bolChecked = false;
                var bolEnabled = false;
                this._Control.WASMGetCommandStatus(commandName, ref bolSuported, ref bolEnabled, ref bolChecked);
                var result = new JsonObject();
                result.Add("Name", commandName);
                result.Add("Supported", bolSuported);
                result.Add("Checked", bolChecked);
                result.Add("Enabled", bolEnabled);
                return result;
            }
            catch (Exception ex)
            {
                JavaScriptMethods.Tools_ReportException("GetCommandStatus", ex.Message, ex.ToString(), 0);
                return null;
            }
            //#endif
        }

        /// <summary>
        /// 清空重做 / 撤销操作信息
        /// </summary>
        [JSInvokable]
        public void ClearUndo()
        {
            try
            {
                this._Control.ClearUndo();
            }
            catch (Exception ex)
            {
                JavaScriptMethods.Tools_ReportException("ClearUndo", ex.Message, ex.ToString(), 0);
                return;
            }
        }

        /// <summary>
        /// 兼容第四代格式刷
        /// </summary>
        [JSInvokable]
        public void BeginFormatBrush()
        {
            try
            {
                this._Control.ExecuteCommand("FormatBrush", false, null);
            }
            catch (Exception ex)
            {
                JavaScriptMethods.Tools_ReportException("BeginFormatBrush", ex.Message, ex.ToString(), 0);
                return;
            }
        }

        /// <summary>
        /// 兼容第四代撤销
        /// </summary>
        [JSInvokable]
        public void Undo()
        {
            try
            {
                this._Control.ExecuteCommand("Undo", false, null);
            }
            catch (Exception ex)
            {
                JavaScriptMethods.Tools_ReportException("Undo", ex.Message, ex.ToString(), 0);
                return;
            }
        }

        /// <summary>
        /// 兼容第四代重做
        /// </summary>
        [JSInvokable]
        public void Redo()
        {
            try
            {
                this._Control.ExecuteCommand("Redo", false, null);
            }
            catch (Exception ex)
            {
                JavaScriptMethods.Tools_ReportException("Redo", ex.Message, ex.ToString(), 0);
                return;
            }
        }

        /// <summary>
        /// 设置段落格式接口
        /// </summary>
        /// <param name="paragraphFormatParameter"></param>
        [JSInvokable]
        public void ParagraphFormat(JsonElement paragraphFormatParameter)
        {
            try
            {
                ParagraphFormatCommandParameter commandParameter = GetCurrentParagraphFormatCommandParameter();//当前段落样式参数

                foreach (System.Text.Json.JsonProperty property in paragraphFormatParameter.EnumerateObject())
                {
                    string name = property.Name.ToLower();
                    string value = WASMJsonConvert.parseJsonElementToString(property.Value);
                    int i = int.MinValue;
                    float d = 0f;
                    switch (name)
                    {
                        case "linespacingstyle":
                            commandParameter.LineSpacingStyle = WASMUtils.ConvertToEnum<LineSpacingStyle>(property, LineSpacingStyle.SpaceSingle);//  WASMJsonConvert.parseEnumValue<LineSpacingStyle>(value, LineSpacingStyle.SpaceSingle); ;
                            break;
                        case "linespacing":
                            if (float.TryParse(value, out d) == true)
                            {
                                commandParameter.LineSpacing = d;
                            }
                            break;
                        case "spacingbefore":
                            if (float.TryParse(value, out d) == true)
                            {
                                commandParameter.SpacingBefore = d;
                            }
                            break;
                        case "spacingafter":
                            if (float.TryParse(value, out d) == true)
                            {
                                commandParameter.SpacingAfter = d;
                            }
                            break;
                        case "spacingbeforeparagraph":
                            if (float.TryParse(value, out d) == true)
                            {
                                commandParameter.SpacingBefore = d;
                            }
                            break;
                        case "spacingafterparagraph":
                            if (float.TryParse(value, out d) == true)
                            {
                                commandParameter.SpacingAfter = d;
                            }
                            break;
                        case "firstlineindent":
                            if (float.TryParse(value, out d) == true)
                            {
                                commandParameter.FirstLineIndent = d;
                            }
                            break;
                        case "leftindent":
                            if (float.TryParse(value, out d) == true)
                            {
                                commandParameter.LeftIndent = d;
                            }
                            break;
                        case "outlinelevel":
                            if (int.TryParse(value, out i) == true)
                            {
                                commandParameter.OutlineLevel = i;
                            }
                            break;
                        case "paragraphoutlinelevel":
                            if (int.TryParse(value, out i) == true)
                            {
                                commandParameter.OutlineLevel = i;
                            }
                            break;
                        case "paragraphmultilevel":
                            commandParameter.ParagraphMultiLevel = WASMUtils.ConvertToBoolean(property, false);// WASMJsonConvert.parseBoolean(value, true);
                            break;
                        case "paragraphliststyle":
                            commandParameter.ListStyle = WASMUtils.ConvertToEnum<ParagraphListStyle>(property, ParagraphListStyle.None);// WASMJsonConvert.parseEnumValue<ParagraphListStyle>(value, ParagraphListStyle.None); ;
                            break;
                        default:
                            break;
                    }
                }
                this._Control.ExecuteCommand("ParagraphFormat", false, commandParameter);
            }
            catch (Exception ex)
            {
                JavaScriptMethods.Tools_ReportException("ParagraphFormat", ex.Message, ex.ToString(), 0);
                return;
            }
        }

        /// <summary>
        /// 兼容第四代获取当前输入域
        /// </summary>
        /// <returns></returns>
        [JSInvokable]
        public DotNetObjectReference<DomElement> CurrentInputField()
        {
            try
            {
                DomInputFieldElement input = null;
                DomElement element = this._Control.Document.CurrentElement;
                if (this._Control.Document.Selection != null && this._Control.Document.Selection.ContentElements.Count > 0)
                {
                    element = this._Control.Document.Selection.ContentElements[0];
                }

                while (element != null)
                {
                    if (element is DomInputFieldElement)
                    {
                        input = element as DomInputFieldElement;
                        break;
                    }
                    element = element.Parent;
                    continue;
                }//while
                if (input != null)
                {
                    return DotNetObjectReference.Create<DomElement>(input);// innerGetElementProperties(currentElement);
                }
                //XTextInputFieldElement currentElement = this._Control.CurrentInputField;
                //if (currentElement != null)
                //{
                //    return DotNetObjectReference.Create<XTextElement>(currentElement);// innerGetElementProperties(currentElement);
                //}
                return null;
            }
            catch (Exception ex)
            {
                JavaScriptMethods.Tools_ReportException("CurrentInputField", ex.Message, ex.ToString(), 0);
                return null;
            }
        }

        /// <summary>
        /// 当前唯一选中的元素或当前插入点右边的元素
        /// </summary>
        public DomElement SingleSelectedOrCurrentElement
        {
            get
            {
                var doc = this._Control.Document;
                var es = doc.Selection.ContentElements;
                if (es != null && es.Count == 1)
                {
                    return es[0];
                }
                else
                {
                    return doc.CurrentElement;
                }
            }
        }

        /// <summary>
        /// 兼容四代接口，获取当前类型的元素
        /// </summary>
        /// <param name="typename"></param>
        /// <returns></returns>
        [JSInvokable]
        public DotNetObjectReference<DomElement> CurrentElement(string typename)
        {
            try
            {
                //wyc20240709:如果完全不传类型则取字符元素避免取到空的。五代BS后台直接CurrentElement有可能会取到空的
                if ( string.IsNullOrEmpty( typename ))
                {
                    DomElement element1 = this.SingleSelectedOrCurrentElement;
                    if (element1 != null)
                    {
                        return DotNetObjectReference.Create<DomElement>(element1); ;
                    }
                    typename = DomCharElement.TypeName_XTextCharElement;
                }
                ////////////////////////////////////////////////////////////////////////////////////////////////////

                if (typename != null && typename.Length > 0)
                {
                    typename = typename.ToLower();
                    if (string.Equals(DomInputFieldElement.TypeName_XTextInputFieldElement, typename, StringComparison.OrdinalIgnoreCase))
                    {
                        return CurrentInputField();
                    }
                    if (string.Equals(DomRadioBoxElement.TypeName_XTextRadioBoxElement, typename, StringComparison.OrdinalIgnoreCase)
                        || string.Equals(DomCheckBoxElement.TypeName_XTextCheckBoxElement, typename, StringComparison.OrdinalIgnoreCase))
                    {
                        var ele = this._Control.SingleSelectedElement;
                        if (ele == null)
                        {
                            ele = this._Control.CurrentElement;
                        }
                        if (ele != null)
                        {
                            if (string.Equals(DomRadioBoxElement.TypeName_XTextRadioBoxElement, typename, StringComparison.OrdinalIgnoreCase))
                            {
                                ele = ele as DomRadioBoxElement;
                            }
                            else
                            {
                                ele = ele as DomCheckBoxElement;
                            }
                        }
                        if (ele != null)
                        {
                            return DotNetObjectReference.Create<DomElement>(ele);
                        }
                        else
                        {
                            return null;
                        }
                    }

                    if (string.Equals(DomTableElement.TypeName_XTextTableElement, typename, StringComparison.OrdinalIgnoreCase))
                    {
                        return CurrentTable();
                    }
                    if (string.Equals(DomTableCellElement.TypeName_XTextTableCellElement, typename, StringComparison.OrdinalIgnoreCase))
                    {
                        return CurrentTableCell();
                    }
                    if (string.Equals(DomTableRowElement.TypeName_XTextTableRowElement, typename, StringComparison.OrdinalIgnoreCase))
                    {
                        return CurrentTableRow();
                    }
                    if (string.Equals(DomImageElement.TypeName_XTextImageElement, typename, StringComparison.OrdinalIgnoreCase))
                    {
                        return CurrentImage();
                    }
                    if (string.Equals(DomPageInfoElement.TypeName_XTextPageInfoElement, typename, StringComparison.OrdinalIgnoreCase))
                    {
                        return CurrentPageInfo();
                    }
                    //wyc20230720:获取当前表格列对象
                    if (string.Equals(DomTableColumnElement.TypeName_XTextTableColumnElement, typename, StringComparison.OrdinalIgnoreCase))
                    {
                        if (this._Control.CurrentTableCell == null)
                        {
                            return null;
                        }
                        return DotNetObjectReference.Create<DomElement>(this._Control.CurrentTableCell.OwnerColumn);
                    }
                }
                var element = this._Control.CurrentElement;
                if (element == null)
                {
                    return null;
                }
                else
                {
                    return DotNetObjectReference.Create<DomElement>(element); ;
                }
            }
            catch (Exception ex)
            {
                JavaScriptMethods.Tools_ReportException("CurrentElement", ex.Message, ex.ToString(), 0);
                return null;
            }
        }


        /// <summary>
        /// 当前图片
        /// </summary>
        /// <returns></returns>
        private DotNetObjectReference<DomElement> CurrentImage()
        {
            DomElement element = SingleSelectedOrCurrentElement;//this._Control.SingleSelectedElement;
            if (element != null && element is DomImageElement)
            {
                DomImageElement newElement = element as DomImageElement;
                if (newElement != null)
                {
                    return DotNetObjectReference.Create<DomElement>(newElement);
                }
            }
            return null;
            //XTextImageElement element = this._Control.GetCurrentElement(typeof(XTextImageElement)) as XTextImageElement;
            //if (element != null)
            //{
            //    return DotNetObjectReference.Create<XTextElement>(element); //innerGetElementProperties(element);
            //}
            //else
            //{
            //    return null;
            //}
        }

        /// <summary>
        /// 当前页码
        /// </summary>
        /// <returns></returns>
        private DotNetObjectReference<DomElement> CurrentPageInfo()
        {
            DomElement element = SingleSelectedOrCurrentElement;// this._Control.SingleSelectedElement;
            if (element != null && element is DomPageInfoElement)
            {
                DomPageInfoElement newElement = element as DomPageInfoElement;
                if (newElement != null)
                {
                    return DotNetObjectReference.Create<DomElement>(newElement);
                }
            }
            return null;
            //XTextPageInfoElement element = this._Control.GetCurrentElement(typeof(XTextPageInfoElement)) as XTextPageInfoElement;
            //if (element != null)
            //{
            //    return DotNetObjectReference.Create<XTextElement>(element); //innerGetElementProperties(element);
            //}
            //else
            //{
            //    return null;
            //}
        }


        ///// <summary>
        ///// 兼容四代执行粘贴操作
        ///// </summary>
        ///// <returns></returns>
        //[JSInvokable]
        //public bool DCPaste()
        //{
        //    try
        //    {
        //        return (bool)this._Control.ExecuteCommand("Paste", false, null);
        //    }
        //    catch (Exception ex)
        //    {
        //        JavaScriptMethods.Tools_ReportException("DCPaste", ex.Message, ex.ToString(), 0);
        //        return false;
        //    }
        //}

        /// <summary>
        /// 查找
        /// </summary>
        /// <param name="parameter"></param>
        [JSInvokable]
        public int Search(JsonElement parameter)
        {
            try
            {
                SearchReplaceCommandArgs commandArgs = BuildSearchReplaceCommandArgs(parameter);
                if (commandArgs != null)
                {
                    commandArgs.EnableReplaceString = false;//启用查找
                    var sr = new ContentSearchReplacer(this.Document);
                    int index = sr.Search(commandArgs, commandArgs.SetSearchResultSelection, -1);
                    return index;
                }
                return -1;
            }
            catch (Exception ex)
            {
                JavaScriptMethods.Tools_ReportException("Search", ex.Message, ex.ToString(), 0);
                return -1;
            }
        }

        /// <summary>
        /// 查找
        /// </summary>
        /// <param name="parameter"></param>
        [JSInvokable]
        public int SearchAllText(JsonElement parameter)
        {
            try
            {
                SearchReplaceCommandArgs commandArgs = BuildSearchReplaceCommandArgs(parameter);
                if (commandArgs != null)
                {
                    commandArgs.EnableReplaceString = false;//启用查找
                    var sr = new ContentSearchReplacer(this.Document);
                    int index = sr.SearchAllText(commandArgs);
                    return index;
                }
                return -1;
            }
            catch (Exception ex)
            {
                JavaScriptMethods.Tools_ReportException("Search", ex.Message, ex.ToString(), 0);
                return -1;
            }
        }

        /// <summary>
        /// 替换
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        [JSInvokable]
        public int Reaplace(JsonElement parameter)
        {
            try
            {
                SearchReplaceCommandArgs commandArgs = BuildSearchReplaceCommandArgs(parameter);
                if (commandArgs != null)
                {
                    commandArgs.EnableReplaceString = true;//启用替换
                    var sr = new ContentSearchReplacer(this.Document);
                    int index = sr.Replace(commandArgs);
                    return index;
                }
                return -1;
            }
            catch (Exception ex)
            {
                JavaScriptMethods.Tools_ReportException("Reaplace", ex.Message, ex.ToString(), 0);
                return -1;
            }
        }

        /// <summary>
        /// 替换所有
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        [JSInvokable]
        public int ReplaceAll(JsonElement parameter)
        {
            try
            {
                SearchReplaceCommandArgs commandArgs = BuildSearchReplaceCommandArgs(parameter);
                if (commandArgs != null)
                {
                    commandArgs.EnableReplaceString = true;//启用替换
                    var sr = new ContentSearchReplacer(this.Document);
                    int index = sr.ReplaceAll(commandArgs);
                    return index;
                }
                return -1;
            }
            catch (Exception ex)
            {
                JavaScriptMethods.Tools_ReportException("ReplaceAll", ex.Message, ex.ToString(), 0);
                return -1;
            }
        }

        /// <summary>
        /// 内部封装查询替换的参数
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        private SearchReplaceCommandArgs BuildSearchReplaceCommandArgs(JsonElement parameter)
        {
            if (parameter.ValueKind != JsonValueKind.Object && parameter.ValueKind != JsonValueKind.String)
            {
                return null;
            }
            SearchReplaceCommandArgs commandArgs = new SearchReplaceCommandArgs();
            if (parameter.ValueKind == JsonValueKind.String)
            {
                commandArgs.SearchString = parameter.GetString();
                return commandArgs;
            }
            foreach (System.Text.Json.JsonProperty property in parameter.EnumerateObject())
            {
                string name = property.Name.ToLower();
                //string value = WASMJsonConvert.parseJsonElementToString(property.Value);
                //int i = int.MinValue;
                //float d = 0f;
                // DateTime dt = DateTime.MinValue;
                switch (name)
                {
                    case "searchstring":
                        commandArgs.SearchString = property.ConvertToString();//要查找的字符串
                        break;
                    case "enablereplacestring":
                        commandArgs.EnableReplaceString = WASMUtils.ConvertToBoolean(property, false);// property.ConvertToBoolean(false);// WASMJsonConvert.parseBoolean(value, false); //是否启用替换
                        break;
                    case "replacestring":
                        commandArgs.ReplaceString = property.ConvertToString();// value; //要替换的字符串
                        break;
                    case "backward":
                        commandArgs.Backward = WASMUtils.ConvertToBoolean(property, false);// property.ConvertToBoolean(false);// WASMJsonConvert.parseBoolean(value, false); //是否向后替换
                        break;
                    case "ignorecase":
                        commandArgs.IgnoreCase = WASMUtils.ConvertToBoolean(property, false);// property.ConvertToBoolean(false);// WASMJsonConvert.parseBoolean(value, false); //是否区分大小写
                        break;
                    case "logundo":
                        commandArgs.LogUndo = WASMUtils.ConvertToBoolean(property, false);// property.ConvertToBoolean(false);// WASMJsonConvert.parseBoolean(value, false); //记录撤销操作信息
                        break;
                    case "excludebackgroundtext":
                        commandArgs.ExcludeBackgroundText = WASMUtils.ConvertToBoolean(property, false);// property.ConvertToBoolean(false);// WASMJsonConvert.parseBoolean(value, false); //忽略掉背景文字
                        break;
                    case "searchid":
                        commandArgs.SearchID = WASMUtils.ConvertToBoolean(property, false);// property.ConvertToBoolean(false);// WASMJsonConvert.parseBoolean(value, false); //是否限制为查询元素编号
                        break;
                    case "setsearchresultselection":
                        commandArgs.SetSearchResultSelection = WASMUtils.ConvertToBoolean(property, true);
                        break;
                    case "highlightbackcolor":
                        if (property.Value.ValueKind == JsonValueKind.String)
                        {
                            commandArgs.HighlightBackColor = ColorTranslator.FromHtml(property.Value.GetString());
                        }
                        break;
                    case "specifycontainer":
                        if (property.Value.ValueKind == JsonValueKind.String)
                        {
                            commandArgs.SpecifyContainer = this._Control.GetElementById(property.Value.GetString()) as DomContainerElement;
                        }
                        else if (property.Value.ValueKind == JsonValueKind.Number)
                        {
                            commandArgs.SpecifyContainer = this.GetElementByNativeHandle(property.Value.GetInt32()) as DomContainerElement;
                        }
                        break;
                    default:
                        break;
                }
            }
            return commandArgs;
        }


        /// <summary>
        /// 获取当前段落的样式信息
        /// </summary>
        /// <returns></returns>
        [JSInvokable]
        public JsonNode GetCurrentParagraphStyle()
        {
            try
            {
                //wyc20230711:重写逻辑
                if (this._Control.CurrentParagraphEOF == null)
                {
                    return null;
                }
                return WASMJsonConvert.DocumentContentStyleToJSON(this._Control.CurrentParagraphEOF.Style);
                //var style = this._Control.Document.CurrentStyleInfo.Paragraph;
                //if (style != null)
                //{
                //    return WASMJsonConvert.ParagraphStyleToJsonObject(style);
                //}
                //return null;
            }
            catch (Exception ex)
            {
                JavaScriptMethods.Tools_ReportException("GetCurrentParagraphStyle", ex.Message, ex.ToString(), 0);
                return null;
            }
        }

        /// <summary>
        /// 设置当前段落的样式信息
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        [JSInvokable]
        public bool SetCurrentParagraphStyle(JsonElement parameter)
        {
            try
            {
                ParagraphFormat(parameter);
                return true;
            }
            catch (Exception ex)
            {
                JavaScriptMethods.Tools_ReportException("SetCurrentParagraphStyle", ex.Message, ex.ToString(), 0);
                return false;
            }
        }

        /// <summary>
        /// 设置文档的默认字体
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        [JSInvokable]
        public bool DocumentDefaultFont(JsonElement parameter)
        {
            try
            {
                var defaultStyle = this._Control.Document.DefaultStyle;
                XFontValue font = new XFontValue();
                font.Name = defaultStyle.FontName;
                font.Size = defaultStyle.FontSize;
                font.Bold = defaultStyle.Bold;
                font.Italic = defaultStyle.Italic;
                font.Underline = defaultStyle.Underline;
                font.Strikeout = defaultStyle.Strikeout;
                WASMJsonConvert.JsonToFont(parameter, font, defaultStyle.FontSize);

                this._Control.EditorSetDefaultFont(font, defaultStyle.Color, true);
                return true;
            }
            catch (Exception ex)
            {
                JavaScriptMethods.Tools_ReportException("DocumentDefaultFont", ex.Message, ex.ToString(), 0);
                return false;
            }
        }

    }
}