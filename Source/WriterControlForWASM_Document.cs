using DCSoft.Common;
using DCSoft.Drawing;
using DCSoft.Printing;
using DCSoft.Writer;
using DCSoft.Writer.Controls;
using DCSoft.Writer.Data;
using DCSoft.Writer.Dom;
using System.Collections.Generic;
using System.Text.Json.Nodes;
using System.Text.Json;
using System.Text;
using DCSoft.Writer.Dom.Undo;

namespace DCSoft.WASM
{
    /// <summary>
    /// 此处对 WriterControl的接口进行转发调用
    /// 对应的，需要在前端 WriterControl_Main.js中的 BindControl()中添加对应的调用。
    /// </summary>
    partial class WriterControlForWASM
    {

        [JSInvokable]
        public JsonNode GetSelection()
        {
            CheckStateBeforeInvoke();
            try
            {
                DCSelection selection = this._Control.Selection;
                if (selection == null)
                {
                    return null;
                }
                JsonObject jsobj = new JsonObject();
                jsobj.Add("Length", selection.Length);
                jsobj.Add("Mode", selection.Mode.ToString());
                jsobj.Add("Text", selection.Text);
                jsobj.Add("XMLText", selection.XMLText);
                jsobj.Add("StartIndex", selection.StartIndex);
                jsobj.Add("AbsStartIndex", selection.AbsStartIndex);
                jsobj.Add("AbsEndIndex", selection.AbsEndIndex);

                JsonArray arr1 = new JsonArray();
                foreach (var item in selection.SelectionParagraphFlags)
                {
                    arr1.Add(this.GetElementNativeHandle(item));
                }
                jsobj.Add("SelectionParagraphFlags", arr1);
                JsonArray arr2 = new JsonArray();
                foreach (var item in selection.Cells)
                {
                    arr2.Add(this.GetElementNativeHandle(item));
                }
                jsobj.Add("Cells", arr2);
                return jsobj;
                //return WASMJsonConvert.parseSelectionToJsonNode(this._Control.Selection);
            }
            catch (Exception ex)
            {
                JavaScriptMethods.Tools_ReportException("GetSelection", ex.Message, ex.ToString(), 0);
                return null;
            }
        }

        [JSInvokable]
        public DotNetObjectReference<DomElement> GetDocument()
        {
            CheckStateBeforeInvoke();

            try
            {
                return DotNetObjectReference.Create<DomElement>(this._Control.Document);
            }
            catch (Exception ex)
            {
                JavaScriptMethods.Tools_ReportException("GetDocument", ex.Message, ex.ToString(), 0);
                return null;
            }

        }
        [JSInvokable]
        public DCSoft.Writer.DocumentOptions GetDocumentOptions()
        {
            CheckStateBeforeInvoke();
            //#if LightWeight
            //            throw new LightWeightNotSupportedException("GetDocumentOptions");
            //#else
            try
            {
                return this._Control.DocumentOptions;
            }
            catch (Exception ex)
            {
                JavaScriptMethods.Tools_ReportException("GetDocumentOptions", ex.Message, ex.ToString(), 0);
                return null;
            }
            //#endif
        }
         
        private DocumentOptions _OptionsBackForPrint = null;


        public string DocumentTitle
        {
            [JSInvokable]
            get
            {
                return this._Control.Document.Title;
            }
            [JSInvokable]
            set
            {
                this._Control.Document.Title = value;
            }
        }

        public string FileFormat
        {
            [JSInvokable]
            get
            {
                return this._Control.Document.FileFormat;
            }
            [JSInvokable]
            set
            {
                this._Control.Document.FileFormat = value;
            }
        }

        public bool Modified
        {
            [JSInvokable]
            get
            {
                return this._Control.Modified;
            }
            [JSInvokable]
            set
            {
                this._Control.Modified = value;
            }
        }

        public JsonObject SelectionStartPosition
        {
            [JSInvokable]
            get
            {
                var p = this._Control.SelectionStartPosition;
                foreach (var page in this._Control.Pages)
                {
                    if (page.ClientBounds.Contains(p))
                    {
                        p.X -= page.ClientBounds.Left;
                        p.Y -= page.ClientBounds.Top;
                        break;
                    }
                }
                JsonObject jsPoint = new JsonObject();
                jsPoint.Add("X", p.X);
                jsPoint.Add("Y", p.Y);
                return jsPoint;
            }
        }

        /// <summary>
        /// 是否只读
        /// </summary>
        public bool Readonly
        {
            [JSInvokable]
            get
            {
                return this._Control.Readonly;
            }
            [JSInvokable]
            set
            {
                this._Control.Readonly = value;
            }
        }


        [JSInvokable]
        public bool IsReadOnly()
        {
            return this._Control.Readonly;
        }

        /// <summary>
        /// 编辑器控件能接受的数据格式，包括粘贴操作和OLE拖拽操作获得的数据
        /// </summary>
        public WriterDataFormats AcceptDataFormats
        {
            [JSInvokable]
            get
            {
                return this._Control.AcceptDataFormats;
            }
            [JSInvokable]
            set
            {
                this._Control.AcceptDataFormats = value;
            }
        }

        /// <summary>
        /// 能否直接拖拽文档内容
        /// </summary>
        public bool AllowDragContent
        {
            [JSInvokable]
            get
            {
                return this._Control.AllowDragContent;
            }
            [JSInvokable]
            set
            {
                this._Control.AllowDragContent = value;
            }
        }

        /// <summary>
        /// 控件是否可以接受用户拖放到它上面的数据
        /// </summary>
        public bool AllowDrop
        {
            [JSInvokable]
            get
            {
                return this._Control.AllowDrop;
            }
            [JSInvokable]
            set
            {
                this._Control.AllowDrop = value;
            }
        }

        /// <summary>
        /// 控件类型全名
        /// </summary>
        public string ControlTypeName
        {
            [JSInvokable]
            get
            {
                return this._Control.ControlTypeName;
            }
        }

        /// <summary>
        /// 当前光标所在的粗体样式
        /// </summary>
        public bool CurrentBold
        {
            [JSInvokable]
            get
            {
                return this._Control.CurrentBold;
            }
        }

        /// <summary>
        /// 当前段落对齐方式
        /// </summary>
        public DocumentContentAlignment CurrentParagraphAlign
        {
            [JSInvokable]
            get
            {
                return this._Control.CurrentParagraphAlign;
            }
        }

        /// <summary>
        /// 当前元素样式
        /// </summary>
        public JsonNode CurrentStyle
        {
            [JSInvokable]
            get
            {
                var result = this._Control.CurrentStyle;
                return WASMJsonConvert.DocumentContentStyleToJSON(result);
            }
        }


        /// <summary>
        /// 当前光标所在的下标样式
        /// </summary>
        public bool CurrentSubscript
        {
            [JSInvokable]
            get
            {
                return this._Control.CurrentSubscript;
            }
        }

        /// <summary>
        /// 当前光标所在的上标样式
        /// </summary>
        public bool CurrentSuperscript
        {
            [JSInvokable]
            get
            {
                return this._Control.CurrentSuperscript;
            }
        }


        /// <summary>
        /// 当前光标所在的下划线样式
        /// </summary>
        public bool CurrentUnderline
        {
            [JSInvokable]
            get
            {
                return this._Control.CurrentUnderline;
            }
        }


        /// <summary>
        /// 文档行为选项
        /// </summary>
        public DocumentBehaviorOptions DocumentBehaviorOptions
        {
            [JSInvokable]
            get
            {
                return this._Control.DocumentBehaviorOptions;
            }
        }

        /// <summary>
        /// 文档编辑选项
        /// </summary>
        public DocumentEditOptions DocumentEditOptions
        {
            [JSInvokable]
            get
            {
                return this._Control.DocumentEditOptions;
            }
        }

        /// <summary>
        /// 文档设置
        /// </summary>
        public DocumentOptions DocumentOptions
        {
            [JSInvokable]
            get
            {
                return this._Control.DocumentOptions;
            }
            [JSInvokable]
            set
            {
                this._Control.DocumentOptions = value;
            }
        }


        /// <summary>
        /// 是否强制显示光标而不管控件是否获得输入焦点
        /// </summary>
        public bool ForceShowCaret
        {
            [JSInvokable]
            get
            {
                return this._Control.ForceShowCaret;
            }
            [JSInvokable]
            set
            {
                this._Control.ForceShowCaret = value;
            }
        }

        /// <summary>
        /// 表单数据组成的字符串数组，序号为偶数的元素为名称，序号为奇数的元素为数值。
        /// </summary>
        public string[] FormValuesArray
        {
            [JSInvokable]
            get
            {
                return this._Control.FormValuesArray;
            }
        }


        /// <summary>
        /// 当前是否处于插入模式,若处于插入模式,则光标比较细,否则处于改写模式,光标比较粗
        /// </summary>
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public bool InsertMode
        {
            [JSInvokable]
            get
            {
                return this._Control.InsertMode;
            }
            [JSInvokable]
            set
            {
                this._Control.InsertMode = value;
            }
        }
         
        /// <summary>
        /// 文档中包含的内容被修改的文本输入域列表对象
        /// </summary>
        public JsonArray ModifiedInputFields
        {
            [JSInvokable]
            get
            {
                JsonArray jsonArray = new JsonArray();
                DomElementList list = this._Control.ModifiedInputFields;
                if (list != null && list.Count > 0)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        DomInputFieldElement input = list[i] as DomInputFieldElement;
                        JsonNode node = innerGetElementProperties(input);
                        jsonArray.Add(node);
                    }
                }
                return jsonArray;
            }
        }
        /// <summary>
        /// 移动焦点使用的快捷键
        /// </summary>
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public MoveFocusHotKeys MoveFocusHotKey
        {
            [JSInvokable]
            get
            {
                return this._Control.MoveFocusHotKey;
            }
            [JSInvokable]
            set
            {
                this._Control.MoveFocusHotKey = value;
            }
        }

        /// <summary>
        /// 总页数
        /// </summary>
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public int PageCount
        {
            [JSInvokable]
            get
            {
                return this._Control.PageCount;
            }
        }

        /// <summary>
        /// 设置或返回从1开始的当前页号
        /// </summary>
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public int PageIndex
        {
            [JSInvokable]
            get
            {
                return this.InnerViewControl.PageIndex;
            }
            [JSInvokable]
            set
            {
                this.InnerViewControl.PageIndex = value;
            }
        }
        /// <summary>
        /// 页面设置
        /// </summary>
        public XPageSettings PageSettings
        {
            [JSInvokable]
            get
            {
                return this._Control.PageSettings;
            }
            [JSInvokable]
            set
            {
                this._Control.PageSettings = value;
            }
        }
         
        /// <summary>
        /// 表示当前插入点位置信息的字符串
        /// </summary>
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public string PositionInfoText
        {
            [JSInvokable]
            get
            {
                return this._Control.PositionInfoText;
            }
        }

        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        [JSInvokable]
        public string GetRuleBackColorString()
        {
            return DCSystem_Drawing.ColorTranslator.ToHtml(this._Control.RuleBackColor);
        }

        /// <summary>
        /// 标尺是否可用
        /// </summary>
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public bool RuleEnabled
        {
            [JSInvokable]
            get
            {
                return this._Control.RuleEnabled;
            }
            [JSInvokable]
            set
            {
                this._Control.RuleEnabled = value;
            }
        }

        /// <summary>
        /// 标尺是否可见,为了提高兼容性，默认不显示标尺。
        /// </summary>
        public bool RuleVisible
        {
            [JSInvokable]
            get
            {
                return this._Control.RuleVisible;
            }
            [JSInvokable]
            set
            {
                if (this._Control.RuleVisible != value)
                {
                    this._Control.RuleVisible = value;
                    this.SetRuleVisible(this._Control.RuleVisible);
                }
            }
        }

        /// <summary>
        /// 文档中被选中的文字
        /// </summary>
        public string SelectedText
        {
            [JSInvokable]
            get
            {
                return this._Control.SelectedText;
            }
        }

        /// <summary>
        /// 文档选择的部分
        /// </summary>
        public DCSelection Selection
        {
            [JSInvokable]
            get
            {
                return this._Control.Selection;
            }
        }

        /// <summary>
        /// 是否显示提示文本
        /// </summary>
        public bool ShowTooltip
        {
            [JSInvokable]
            get
            {
                return this._Control.ShowTooltip;
            }
            [JSInvokable]
            set
            {
                this._Control.ShowTooltip = value;
            }
        }

        /// <summary>
        /// 当前单选的文档元素对象
        /// 当只选中一个文档元素对象，则返回给文档元素对象，如果没有选中元素 或者选中多个元素，则返回空。
        /// </summary>
        public DomElement SingleSelectedElement
        {
            [JSInvokable]
            get
            {
                return this._Control.SingleSelectedElement;
            }
        }

        /// <summary>
        /// 清空文档内容
        /// </summary>
        [JSInvokable]
        public void ClearContent()
        {
            CheckStateBeforeInvoke();
            try
            {
                this._Control.ClearContent();
            }
            catch (Exception ex)
            {
                JavaScriptMethods.Tools_ReportException("ClearContent", ex.Message, ex.ToString(), 0);
                return;
            }
        }

        /// <summary>
        /// 显示关于对话框
        /// </summary>
        [JSInvokable]
        public JsonNode ShowAboutDialog(bool shouUI)
        {
            try
            {
                return this._Control.ShowAboutDialog(shouUI);
            }
            catch (Exception ex)
            {
                JavaScriptMethods.Tools_ReportException("ShowAboutDialog", ex.Message, ex.ToString(), 0);
                return null;
            }
        }

        /// <summary>
        /// 刷新
        /// </summary>
        [JSInvokable]
        public void RefreshDocument()
        {
            CheckStateBeforeInvoke();
            try
            {
                this._Control.RefreshDocument();
            }
            catch (Exception ex)
            {
                JavaScriptMethods.Tools_ReportException("RefreshDocument", ex.Message, ex.ToString(), 0);
                return;
            }
        }

        /// <summary>
        /// 执行命令
        /// </summary>
        /// <param name="commandName">命令名称</param>
        /// <param name="showUI">是否显示UI</param>
        /// <param name="parameter">参数</param>
        /// <returns>执行结果</returns>
        [JSInvokable]
        public string ExecuteCommand(string commandName, bool showUI, JsonElement parameter)
        {
            try
            {
                object obj = null;
                switch (parameter.ValueKind)
                {
                    case JsonValueKind.String: obj = parameter.GetString(); break;
                    case JsonValueKind.Number: obj = parameter.GetInt32(); break;
                    case JsonValueKind.True: obj = true; break;
                    case JsonValueKind.False: obj = false; break;
                    case JsonValueKind.Null: obj = null; break;
                    default: obj = parameter; break;
                }
                return Convert.ToString(this._Control.ExecuteCommand(commandName, showUI, obj));
            }
            catch (Exception ex)
            {
                JavaScriptMethods.Tools_ReportException("ExecuteCommand", ex.Message, ex.ToString(), 0);
                return "";
            }
        }

        /// <summary>
        /// 删除选择区域
        /// </summary>
        /// <param name="showUI"></param>
        [JSInvokable]
        public void DeleteSelection(JsonElement showUI)
        {
            try
            {
                this._Control.DeleteSelection(showUI.JsonElementToBoolean(true));
            }
            catch (Exception ex)
            {
                JavaScriptMethods.Tools_ReportException("DeleteSelection", ex.Message, ex.ToString(), 0);
                return;
            }
        }

        /// <summary>
        /// 文档内容进行校验，返回校验结果
        /// </summary>
        /// <returns></returns>
        [JSInvokable]
        public JsonArray DocumentValueValidate(JsonElement parameter2)
        {
            CheckStateBeforeInvoke();
            try
            {
                ValueValidateResultList list = null;
                if (parameter2.ValueKind == JsonValueKind.Null || parameter2.ValueKind == JsonValueKind.Undefined)
                {
                    list = this._Control.DocumentValueValidate();
                }
                else
                {
                    DomElement element = null;
                    if (parameter2.ValueKind == JsonValueKind.String)
                    {
                        element = this._Control.GetElementById(parameter2.GetString());
                    }
                    else if (parameter2.ValueKind == JsonValueKind.Number)
                    {
                        element = this.GetElementByNativeHandle(parameter2.GetInt32());
                    }
                    if (element != null && element is DomContainerElement)
                    {
                        DomContainerElement containerElement = element as DomContainerElement;
                        if (containerElement != null)
                        {
                            list = DefaultDOMDataProvider.ValueValidateInContainerElement(containerElement);
                        }
                    }
                }
                return InnerBuildValueValidateResultList(list);
            }
            catch (Exception ex)
            {
                JavaScriptMethods.Tools_ReportException("DocumentValueValidate", ex.Message, ex.ToString(), 0);
                return null;
            }
        }
        /// <summary>
        /// 编辑器控件获得输入焦点
        /// </summary>
        /// <returns></returns>
        [JSInvokable]
        public bool Focus()
        {
            try
            {
                return this._Control.Focus();
            }
            catch (Exception ex)
            {
                JavaScriptMethods.Tools_ReportException("Focus", ex.Message, ex.ToString(), 0);
                return false;
            }
        }


        /// <summary>
        /// 获得所有支持的命令名称组成的字符串，各个名称之间用逗号分开
        /// </summary>
        /// <returns>字符串列表</returns>
        [JSInvokable]
        public string GetCommandNameList()
        {
            try
            {
                return this._Control.GetCommandNameList();
            }
            catch (Exception ex)
            {
                JavaScriptMethods.Tools_ReportException("GetCommandNameList", ex.Message, ex.ToString(), 0);
                return "";
            }
        }

        /// <summary>
        /// 获得表单数据
        /// </summary>
        /// <param name="name">字段名称</param>
        /// <returns>获得的表单数值</returns>
        [JSInvokable]
        public string GetFormValue(string name)
        {
            try
            {
                return this._Control.GetFormValue(name);
            }
            catch (Exception ex)
            {
                JavaScriptMethods.Tools_ReportException("GetFormValue", ex.Message, ex.ToString(), 0);
                return "";
            }
        }




        /// <summary>
        /// 设置所有文档视图无效，需要全部重新绘制
        /// </summary>
        /// <param name="range"></param>
        [JSInvokable]
        public void InvalidateAll()
        {
            try
            {
                this._Control.GetInnerViewControl().Invalidate();
            }
            catch (Exception ex)
            {
                JavaScriptMethods.Tools_ReportException("InvalidateAll", ex.Message, ex.ToString(), 0);
                return;
            }
        }

        /// <summary>
        /// 刷新文档内部排版和分页。不更新用户界面。
        /// </summary>
        /// <param name="fastMode"></param>
        [JSInvokable]
        public void RefreshInnerView(bool fastMode)
        {
            try
            {
                var isPrintPreview = JavaScriptMethods.UI_IsPrintPreview(this._ContainerElementID); //this._Control.Document.States.PrintPreviewing;
                if (this.IsPrintOrPrintPreview==true|| isPrintPreview==true)
                {
                    return;
                }
                bool fm = fastMode;// WASMJsonConvert.ConvertToBoolean(fastMode, false);
                this._Control.RefreshInnerView(fm);
            }
            catch (Exception ex)
            {
                JavaScriptMethods.Tools_ReportException("RefreshInnerView", ex.Message, ex.ToString(), 0);
                return;
            }
        }


        /// <summary>
        /// 重置表单元素默认值
        /// </summary>
        /// <returns>是否导致文档内容发生改变</returns>
        [JSInvokable]
        public bool ResetFormValue()
        {
            try
            {
                return this._Control.ResetFormValue();
            }
            catch (Exception ex)
            {
                JavaScriptMethods.Tools_ReportException("ResetFormValue", ex.Message, ex.ToString(), 0);
                return false;
            }
        }

        /// <summary>
        /// 选中文档所有内容
        /// </summary>
        [JSInvokable]
        public void SelectAll()
        {
            try
            {
                this._Control.SelectAll();
            }
            catch (Exception ex)
            {
                JavaScriptMethods.Tools_ReportException("SelectAll", ex.Message, ex.ToString(), 0);
                return;
            }
        }

        /// <summary>
        /// 选择内容
        /// </summary>
        /// <param name="startElement">选择区域起始元素</param>
        /// <param name="endElement">选择区域终止元素</param>
        /// <returns>操作是否成功</returns>
        [JSInvokable]
        public bool SelectContentByStartEndElement(DotNetObjectReference<DomElement> startOptions, DotNetObjectReference<DomElement> endOptions)
        {
            try
            {
                if (startOptions != null && endOptions != null)
                {
                    DomElement startElement = startOptions.Value as DomElement;
                    DomElement endElement = endOptions.Value as DomElement;
                    startOptions.Dispose();
                    endOptions.Dispose();
                    if (startElement != null && endElement != null)
                    {
                        return this._Control.SelectContentByStartEndElement(startElement, endElement);
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                JavaScriptMethods.Tools_ReportException("SelectContentByStartEndElement", ex.Message, ex.ToString(), 0);
                return false;
            }
        }

        /// <summary>
        /// 根据其实元素编号和结束元素编号选择内容
        /// </summary>
        /// <param name="startElement">选择区域起始元素编号</param>
        /// <param name="endElement">选择区域终止元素编号</param>
        /// <returns>操作是否成功</returns>
        [JSInvokable]
        public bool SelectContentByStartEndElementID(JsonElement startID, JsonElement endID)
        {
            try
            {
                DomElement startElement =null;
                DomElement endElement = null;
                if (startID.ValueKind == JsonValueKind.String)
                {
                    startElement = this._Control.GetElementById(startID.GetString());
                }
                else if (startID.ValueKind == JsonValueKind.Number)
                {
                    startElement = this.GetElementByNativeHandle(startID.GetInt32());
                }
                if (endID.ValueKind == JsonValueKind.String)
                {
                    endElement = this._Control.GetElementById(endID.GetString());
                }
                else if (endID.ValueKind == JsonValueKind.Number)
                {
                    endElement = this.GetElementByNativeHandle(endID.GetInt32());
                }
                if (startElement != null && endElement != null)
                {
                    return this._Control.SelectContentByStartEndElement(startElement, endElement);
                }
                //if (startID != null && startID != "" && startID.Length > 0 && endID != null && endID != "" && endID.Length > 0)
                //{
                //    XTextElement startElement = this._Control.GetElementById(startID);
                //    XTextElement endElement = this._Control.GetElementById(endID);
                //    if (startElement != null && endElement != null)
                //    {
                //        return this._Control.SelectContentByStartEndElement(startElement, endElement);
                //    }
                //}
                return false;
            }
            catch (Exception ex)
            {
                JavaScriptMethods.Tools_ReportException("SelectContentByStartEndElementID", ex.Message, ex.ToString(), 0);
                return false;
            }
        }

        /// <summary>
        /// 选择内容
        /// </summary>
        /// <param name="startContentIndex">选择区域起始编号</param>
        /// <param name="endContentIndex">选择区域终止编号</param>
        /// <returns>操作是否成功</returns>
        [JSInvokable]
        public bool SelectContentByStartEndIndex(int startContentIndex, int endContentIndex)
        {
            try
            {
                return this._Control.SelectContentByStartEndIndex(startContentIndex, endContentIndex);
            }
            catch (Exception ex)
            {
                JavaScriptMethods.Tools_ReportException("SelectContentByStartEndIndex", ex.Message, ex.ToString(), 0);
                return false;
            }
        }

        /// <summary>
        /// 获取文档的页面设置
        /// </summary>
        /// <returns></returns>
        [JSInvokable]
        public JsonNode GetDocumentPageSettings()
        {
            try
            {
                if (this._Control.PageSettings == null)
                {
                    this._Control.PageSettings = new XPageSettings();
                }
                JsonObject obj = new JsonObject();
                obj.Add("PaperKind", this._Control.PageSettings.PaperKind.ToString());
                obj.Add("PaperHeight", this._Control.PageSettings.PaperHeight);
                obj.Add("PaperWidth", this._Control.PageSettings.PaperWidth);
                obj.Add("Landscape", this._Control.PageSettings.Landscape);
                obj.Add("LeftMargin", this._Control.PageSettings.LeftMargin);
                obj.Add("RightMargin", this._Control.PageSettings.RightMargin);
                obj.Add("TopMargin", this._Control.PageSettings.TopMargin);
                obj.Add("BottomMargin", this._Control.PageSettings.BottomMargin);
                obj.Add("HeaderDistance", this._Control.PageSettings.HeaderDistance);
                obj.Add("FooterDistance", this._Control.PageSettings.FooterDistance);
                obj.Add("LeftMarginInCM", this._Control.PageSettings.LeftMarginInCM);
                obj.Add("RightMarginInCM", this._Control.PageSettings.RightMarginInCM);
                obj.Add("TopMarginInCM", this._Control.PageSettings.TopMarginInCM);
                obj.Add("BottomMarginInCM", this._Control.PageSettings.BottomMarginInCM);
                obj.Add("PaperHeightInCM", GraphicsUnitConvert.ConvertToCM((float)this._Control.PageSettings.PaperHeight / 100f, GraphicsUnit.Inch));
                obj.Add("PaperWidthInCM", GraphicsUnitConvert.ConvertToCM((float)this._Control.PageSettings.PaperWidth / 100f, GraphicsUnit.Inch));
                if (this._Control.Pages != null && this._Control.Pages.Count > 0)
                {
                    obj.Add("ClientBoundsWidth", this._Control.Pages[0].ClientBounds.Width.ToString());
                    obj.Add("ClientBoundsHeight", this._Control.Pages[0].ClientBounds.Height.ToString());
                }
                return obj;
            }
            catch (Exception ex)
            {
                JavaScriptMethods.Tools_ReportException("GetDocumentPageSettings", ex.Message, ex.ToString(), 0);
                return null;
            }
        }

        /// <summary>
        /// 页面设置
        /// </summary>
        /// <param name="settingsInfo"></param>
        [JSInvokable]
        public void ChangeDocumentSettings(JsonElement settingsInfo)
        {
            try
            {
                if (this._Control.PageSettings == null)
                {
                    this._Control.PageSettings = new XPageSettings();
                }
                foreach (System.Text.Json.JsonProperty property in settingsInfo.EnumerateObject())
                {
                    string name = property.Name.ToLower();
                    string value = WASMJsonConvert.parseJsonElementToString(property.Value);
                    float d = 0f;
                    int i = int.MinValue;
                    switch (name)
                    {
                        case "paperkind":
                            this._Control.PageSettings.PaperKind = WASMUtils.ConvertToEnum<PaperKind>(property, PaperKind.Custom);// WASMJsonConvert.parseEnumValue< PaperKind>(value,  PaperKind.Custom); ;
                            break;
                        case "paperheight":
                            if (int.TryParse(value, out i) == true)
                            {
                                this._Control.PageSettings.PaperHeight = i;
                            }
                            break;
                        case "paperwidth":
                            if (int.TryParse(value, out i) == true)
                            {
                                this._Control.PageSettings.PaperWidth = i;
                            }
                            break;

                        case "paperheightincm":
                            if (float.TryParse(value, out d) == true)
                            {
                                this._Control.PageSettings.PaperHeight = (int)(GraphicsUnitConvert.ConvertFromCM(d, GraphicsUnit.Inch) * 100f);
                            }
                            break;
                        case "paperwidthincm":
                            if (float.TryParse(value, out d) == true)
                            {
                                this._Control.PageSettings.PaperWidth = (int)(GraphicsUnitConvert.ConvertFromCM(d, GraphicsUnit.Inch) * 100f);
                            }
                            break;
                        case "leftmarginincm":
                            if (float.TryParse(value, out d) == true)
                            {
                                this._Control.PageSettings.LeftMarginInCM = d;
                            }
                            break;
                        case "rightmarginincm":
                            if (float.TryParse(value, out d) == true)
                            {
                                this._Control.PageSettings.RightMarginInCM = d;
                            }
                            break;
                        case "topmarginincm":
                            if (float.TryParse(value, out d) == true)
                            {
                                this._Control.PageSettings.TopMarginInCM = d;
                            }
                            break;
                        case "bottommarginincm":
                            if (float.TryParse(value, out d) == true)
                            {
                                this._Control.PageSettings.BottomMarginInCM = d;
                            }
                            break;

                        case "landscape":
                            this._Control.PageSettings.Landscape = WASMUtils.ConvertToBoolean(property, false);// WASMJsonConvert.parseBoolean(value, false);
                            break;
                        case "leftmargin":
                            if (int.TryParse(value, out i) == true)
                            {
                                this._Control.PageSettings.LeftMargin = i;
                            }
                            break;
                        case "rightmargin":
                            if (int.TryParse(value, out i) == true)
                            {
                                this._Control.PageSettings.RightMargin = i;
                            }
                            break;
                        case "topmargin":
                            if (int.TryParse(value, out i) == true)
                            {
                                this._Control.PageSettings.TopMargin = i;
                            }
                            break;
                        case "bottommargin":
                            if (int.TryParse(value, out i) == true)
                            {
                                this._Control.PageSettings.BottomMargin = i;
                            }
                            break;
                        case "headerdistance":
                            if (int.TryParse(value, out i) == true)
                            {
                                this._Control.PageSettings.HeaderDistance = i;
                            }
                            break;
                        case "footerdistance":
                            if (int.TryParse(value, out i) == true)
                            {
                                this._Control.PageSettings.FooterDistance = i;
                            }
                            break;
                        default:
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                JavaScriptMethods.Tools_ReportException("ChangeDocumentSettings", ex.Message, ex.ToString(), 0);
                return;
            }
        }


        /// <summary>
        /// 设置文档网格线
        /// </summary>
        /// <param name="gridLineInfo"></param>
        [JSInvokable]
        public void SetDocumentGridLine(JsonElement gridLineInfo)
        {
            try
            {
                if (this._Control.PageSettings == null)
                {
                    this._Control.PageSettings = new XPageSettings();
                }
                if (this._Control.PageSettings.DocumentGridLine == null)
                {
                    this._Control.PageSettings.DocumentGridLine = new DCGridLineInfo();
                }

                foreach (System.Text.Json.JsonProperty property in gridLineInfo.EnumerateObject())
                {
                    string name = property.Name.ToLower();
                    string value = WASMJsonConvert.parseJsonElementToString(property.Value);
                    //float d = 0f;
                    int i = int.MinValue;
                    switch (name)
                    {
                        case "visible":
                            this._Control.PageSettings.DocumentGridLine.Visible = WASMUtils.ConvertToBoolean(property, false);// WASMJsonConvert.parseBoolean(value, false);
                            break;
                        case "aligntogridline":
                            this._Control.PageSettings.DocumentGridLine.AlignToGridLine = WASMUtils.ConvertToBoolean(property, true);// WASMJsonConvert.parseBoolean(value, true);
                            break;
                        case "colorvalue":
                            this._Control.PageSettings.DocumentGridLine.ColorValue = value;
                            break;
                        case "gridnuminonepage":
                            if (int.TryParse(value, out i) == true)
                            {
                                this._Control.PageSettings.DocumentGridLine.GridNumInOnePage = i;
                            }
                            break;
                        case "linestyle":
                            this._Control.PageSettings.DocumentGridLine.LineStyle = WASMUtils.ConvertToEnum<DashStyle>(property, DashStyle.Solid);// WASMJsonConvert.parseEnumValue<DashStyle>(value, DashStyle.Solid); ;
                            break;
                        case "printable":
                            this._Control.PageSettings.DocumentGridLine.Printable = WASMUtils.ConvertToBoolean(property, true);// WASMJsonConvert.parseBoolean(value, true);
                            break;
                        default:
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                JavaScriptMethods.Tools_ReportException("SetDocumentGridLine", ex.Message, ex.ToString(), 0);
                return;
            }
        }

        /// <summary>
        /// 获取文档的网格线
        /// </summary>
        /// <returns></returns>
        [JSInvokable]
        public JsonNode GetDocumentGridLine()
        {
            try
            {
                if (this._Control.PageSettings == null)
                {
                    this._Control.PageSettings = new XPageSettings();
                }
                if (this._Control.PageSettings.DocumentGridLine == null)
                {
                    this._Control.PageSettings.DocumentGridLine = new DCGridLineInfo();
                }

                JsonObject obj = new JsonObject();
                obj.Add("Visible", this._Control.PageSettings.DocumentGridLine.Visible);
                obj.Add("AlignToGridLine", this._Control.PageSettings.DocumentGridLine.AlignToGridLine);
                obj.Add("ColorValue", this._Control.PageSettings.DocumentGridLine.ColorValue);
                obj.Add("GridNumInOnePage", this._Control.PageSettings.DocumentGridLine.GridNumInOnePage);
                obj.Add("LineStyle", this._Control.PageSettings.DocumentGridLine.LineStyle.ToString());
                obj.Add("Printable", this._Control.PageSettings.DocumentGridLine.Printable);
                return obj;
            }
            catch (Exception ex)
            {
                JavaScriptMethods.Tools_ReportException("GetDocumentGridLine", ex.Message, ex.ToString(), 0);
                return null;
            }
        }

         
        /// <summary>
        ///  返回选择的内容
        /// </summary>
        /// <param name="format"></param>
        /// <param name="containHeaderFooter">是否包含页眉</param>
        /// <returns></returns>
        private string GetSelection(string format, object parameter)
        {
            bool containHeaderFooter = WASMJsonConvert.parseBoolean(parameter, false);
            if (format == null || format == "")
            {
                format = "xml";
            }
            if (format.ToLower() == "txt")
            {
                return this._Control.Document.Selection.Text;
            }
            else
            {
                DomDocument document = new DomDocument();
                document.LoadFromString(this._Control.Document.Selection.XMLText, "xml");
                document.Options = this._Control.DocumentOptions.Clone();
                if (containHeaderFooter)
                {
                    DomDocument documentOld = this._Control.Document.Clone(true) as DomDocument;
                    DomElementList headerList = documentOld.Header.Elements.Clone();
                    document.ImportElements(headerList);
                    document.Header.Elements.AddRangeByDCList(headerList);

                    DomElementList footerList = documentOld.Footer.Elements.Clone();
                    document.ImportElements(footerList);
                    document.Footer.Elements.AddRangeByDCList(footerList);

                    documentOld = null;
                }
                if (format.ToLower() == "xml")
                {
                    return document.SaveToString("XML");
                }
                else if (format.ToLower() == "json")
                {
                    return document.SaveToString("json");
                }
                return "";
            }
        }
         
        /// <summary>
        /// 获取选择的文本内容
        /// </summary>
        /// <param name="clearBorder"></param>
        /// <returns></returns>
        [JSInvokable]
        public string SelectionText(object clearBorder)
        {
            try
            {
                return GetSelection("txt", clearBorder);
            }
            catch (Exception ex)
            {
                JavaScriptMethods.Tools_ReportException("SelectionText", ex.Message, ex.ToString(), 0);
                return "";
            }
        }

        /// <summary>
        /// 获取选择的xml内容
        /// </summary>
        /// <param name="containHeaderFooter"></param>
        /// <returns></returns>
        [JSInvokable]
        public string SelectionXml(object containHeaderFooter)
        {
            CheckStateBeforeInvoke();
            try
            {
                return GetSelection("xml", containHeaderFooter);
            }
            catch (Exception ex)
            {
                JavaScriptMethods.Tools_ReportException("SelectionXml", ex.Message, ex.ToString(), 0);
                return "";
            }
        }


        /// <summary>
        /// 兼容四代接口，获取当前选择内容
        /// </summary>
        /// <param name="format"></param>
        /// <returns></returns>
        [JSInvokable]
        public string DocumentSelection(string format)
        {
            try
            {
                if (format == null || format == "" || format.Length == 0)
                {
                    format = "xml";
                }
                format = format.ToLower();
                if (format == "xml")
                {
                    return this.SelectionXml(false);
                }
                if (format == "text")
                {
                    return this.SelectionText(false);
                }
                return "";
            }
            catch (Exception ex)
            {
                JavaScriptMethods.Tools_ReportException("DocumentSelection", ex.Message, ex.ToString(), 0);
                return "";
            }
        }

        /// <summary>
        /// 兼容四代接口
        /// </summary>
        /// <returns></returns>
        [JSInvokable]
        public void SetDocumentInfos(JsonElement parameter)
        {
#if LightWeight
            throw new LightWeightNotSupportedException("SetDocumentInfos");
#else
            try
            {
                if (this._Control.Document.Info == null)
                {
                    this._Control.Document.Info = new DocumentInfo();
                }
                foreach (System.Text.Json.JsonProperty property in parameter.EnumerateObject())
                {
                    string name = property.Name.ToLower();
                    string value = WASMJsonConvert.parseJsonElementToString(property.Value);
                    int i = int.MinValue;
                    float d = 0f;
                    DateTime dt = DateTime.MinValue;
                    switch (name)
                    {
                        case "readonly":
                            this._Control.Document.Info.Readonly = WASMUtils.ConvertToBoolean(property, false);// WASMJsonConvert.parseBoolean(value, false);
                            break;
                        case "showheaderbottomline":
                            this._Control.Document.Info.ShowHeaderBottomLine = WASMUtils.ConvertToEnum<DCBooleanValue>(property, DCBooleanValue.Inherit);//  WASMJsonConvert.parseEnumValue<DCBooleanValue>(value, DCBooleanValue.Inherit); ;
                            break;
                        case "fieldborderelementwidth":
                            if (float.TryParse(value, out d) == true)
                            {
                                this._Control.Document.Info.FieldBorderElementWidth = d;
                            }
                            break;
                        case "runtimetitle":
                            this._Control.Document.Info.RuntimeTitle = value;
                            break;
                        case "id":
                            this._Control.Document.Info.ID = value;
                            break;
                        case "istemplate":
                            this._Control.Document.Info.IsTemplate = WASMUtils.ConvertToBoolean(property, false);// WASMJsonConvert.parseBoolean(value, false);
                            break;
                        case "version":
                            this._Control.Document.Info.Version = value;
                            break;
                        case "title":
                            this._Control.Document.Info.Title = value;
                            break;
                        case "description":
                            this._Control.Document.Info.Description = value;
                            break;
                        case "creationtime":
                            if (DateTime.TryParse(value, out dt) == true)
                            {
                                //if (dt.Year == 0001)
                                //{

                                //}
                                //else
                                //{
                                //    DateTimeFormatInfo dtFormat = new DateTimeFormatInfo();
                                //    dtFormat.ShortDatePattern = "yyyy/MM/dd HH:mm:ss";
                                //    dt = Convert.ToDateTime(value, dtFormat);

                                //    this._Control.Document.Info.CreationTime = dt;
                                //}
                                this._Control.Document.Info.CreationTime = dt;
                            }
                            break;
                        case "lastmodifiedtime":
                            if (DateTime.TryParse(value, out dt) == true)
                            {
                                //if (dt.Year == 0001)
                                //{

                                //}
                                //else
                                //{
                                //    DateTimeFormatInfo dtFormat = new DateTimeFormatInfo();
                                //    dtFormat.ShortDatePattern = "yyyy/MM/dd HH:mm:ss";
                                //    dt = Convert.ToDateTime(value, dtFormat);

                                //    this._Control.Document.Info.LastModifiedTime = dt;
                                //}
                                this._Control.Document.Info.LastModifiedTime = dt;
                            }
                            break;
                        case "editminute":
                            if (int.TryParse(value, out i) == true)
                            {
                                this._Control.Document.Info.EditMinute = i;
                            }
                            break;
                        case "lastprinttime":
                            if (DateTime.TryParse(value, out dt) == true)
                            {
                                //if (dt.Year == 0001)
                                //{

                                //}
                                //else
                                //{
                                //    DateTimeFormatInfo dtFormat = new DateTimeFormatInfo();
                                //    dtFormat.ShortDatePattern = "yyyy/MM/dd HH:mm:ss";
                                //    dt = Convert.ToDateTime(value, dtFormat);

                                //    this._Control.Document.Info.LastPrintTime = dt;
                                //}
                                this._Control.Document.Info.LastPrintTime = dt;
                            }
                            break;
                        case "author":
                            this._Control.Document.Info.Author = value;
                            break;
                        case "authorname":
                            this._Control.Document.Info.AuthorName = value;
                            break;
                        case "departmentid":
                            this._Control.Document.Info.DepartmentID = value;
                            break;
                        case "departmentname":
                            this._Control.Document.Info.DepartmentName = value;
                            break;
                        case "documentformat":
                            this._Control.Document.Info.DocumentFormat = value;
                            break;
                        case "documenttype":
                            this._Control.Document.Info.DocumentType = value;
                            break;
                        case "documentprocessstate":
                            if (int.TryParse(value, out i) == true)
                            {
                                this._Control.Document.Info.DocumentProcessState = i;
                            }
                            break;
                        case "comment":
                            this._Control.Document.Info.Comment = value;
                            break;
                        case "operator":
                            this._Control.Document.Info.Operator = value;
                            break;
                        case "printable":
                            this._Control.Document.Info.Printable = WASMUtils.ConvertToBoolean(property, false);// WASMJsonConvert.parseBoolean(value, false);
                            break;
                        case "startpositioninpringjob":
                            if (int.TryParse(value, out i) == true)
                            {
                                this._Control.Document.Info.StartPositionInPringJob = i;
                            }
                            break;
                        case "heightinprintjob":
                            if (int.TryParse(value, out i) == true)
                            {
                                this._Control.Document.Info.HeightInPrintJob = i;
                            }
                            break;
                        default:
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                JavaScriptMethods.Tools_ReportException("SetDocumentInfos", ex.Message, ex.ToString(), 0);
                return;
            }
#endif
        }

        /// <summary>
        /// 兼容四代接口
        /// </summary>
        /// <returns></returns>
        [JSInvokable]
        public JsonNode GetDocumentInfos()
        {
#if LightWeight
            throw new LightWeightNotSupportedException("GetDocumentInfos");
#else
            try
            {
                if (this._Control.Document.Info == null)
                {
                    this._Control.Document.Info = new DocumentInfo();
                }
                JsonObject obj = new JsonObject();
                obj.Add("Readonly", this._Control.Document.Info.Readonly);
                obj.Add("ShowHeaderBottomLine", this._Control.Document.Info.ShowHeaderBottomLine.ToString());
                obj.Add("FieldBorderElementWidth", this._Control.Document.Info.FieldBorderElementWidth);
                obj.Add("RuntimeTitle", this._Control.Document.Info.RuntimeTitle);
                obj.Add("ID", this._Control.Document.Info.ID);
                obj.Add("IsTemplate", this._Control.Document.Info.IsTemplate);
                obj.Add("Version", this._Control.Document.Info.Version);
                obj.Add("Title", this._Control.Document.Info.Title);
                obj.Add("Description", this._Control.Document.Info.Description);
                obj.Add("CreationTime", this._Control.Document.Info.CreationTime);
                obj.Add("LastModifiedTime", this._Control.Document.Info.LastModifiedTime);
                obj.Add("EditMinute", this._Control.Document.Info.EditMinute);
                obj.Add("LastPrintTime", this._Control.Document.Info.LastPrintTime);
                obj.Add("Author", this._Control.Document.Info.Author);
                obj.Add("AuthorName", this._Control.Document.Info.AuthorName);
                obj.Add("DepartmentID", this._Control.Document.Info.DepartmentID);
                obj.Add("DepartmentName", this._Control.Document.Info.DepartmentName);
                obj.Add("DocumentFormat", this._Control.Document.Info.DocumentFormat);
                obj.Add("DocumentType", this._Control.Document.Info.DocumentType);
                obj.Add("DocumentProcessState", this._Control.Document.Info.DocumentProcessState);
                obj.Add("Comment", this._Control.Document.Info.Comment);
                obj.Add("Operator", this._Control.Document.Info.Operator);
                obj.Add("Printable", this._Control.Document.Info.Printable);
                obj.Add("StartPositionInPringJob", this._Control.Document.Info.StartPositionInPringJob);
                obj.Add("HeightInPrintJob", this._Control.Document.Info.HeightInPrintJob);
                return obj;
            }
            catch (Exception ex)
            {
                JavaScriptMethods.Tools_ReportException("GetDocumentInfos", ex.Message, ex.ToString(), 0);
                return null;
            }
#endif
        }

        /// wyc20230524获取文档是否被修改
        /// <summary>
        /// 获取文档是否被修改
        /// </summary>
        /// <returns></returns>
        [JSInvokable]
        public bool getModified()
        {
            try
            {
                return this._Control.Modified;
            }
            catch (Exception ex)
            {
                JavaScriptMethods.Tools_ReportException("getModified", ex.Message, ex.ToString(), 0);
                return false;
            }
        }

        /// <summary>
        /// 重置文档修改状态
        /// </summary>
        /// <returns></returns>
        [JSInvokable]
        public bool resetModified(bool parameter)
        {
            try
            {
                this._Control.Modified = parameter;

                if (parameter == false)
                {//更新为未修改状态时，需要重置元素的状态为非修改状态
                    DomElementList list = this._Control.Document.GetAllElements();
                    if (list != null && list.Count > 0)
                    {
                        for (int i = 0; i < list.Count; i++)
                        {
                            DomElement element = list[i];
                            if (element.Modified
                                && element.TypeName != null)
                            {
                                element.Modified = false;
                            }
                        }
                    }
                }
                if (this._Control.Modified == parameter)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                JavaScriptMethods.Tools_ReportException("resetModified", ex.Message, ex.ToString(), 0);
                return false;
            }
        }

        /// <summary>
        /// 设置控件只读状态
        /// </summary>
        /// <returns></returns>
        [JSInvokable]
        public bool setControlReadonly(object parameter)
        {
            try
            {
                if (parameter == null || (parameter is JsonElement) == false)
                {
                    return false;
                }
                JsonElement element = (JsonElement)parameter;
                if (element.ValueKind == JsonValueKind.False ||
                    (element.ValueKind == JsonValueKind.String && element.GetString().ToLower() == "false"))
                {
                    this._Control.Readonly = false;
                    return true;
                }
                else if (element.ValueKind == JsonValueKind.True ||
                    (element.ValueKind == JsonValueKind.String && element.GetString().ToLower() == "true"))
                {
                    this._Control.Readonly = true;
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                JavaScriptMethods.Tools_ReportException("setControlReadonly", ex.Message, ex.ToString(), 0);
                return false;
            }
        }

        /// <summary>
        /// 在当前位置插入XML
        /// </summary>
        /// <returns></returns>
        [JSInvokable]
        public bool insertXmlString(object parameter)
        {
            CheckStateBeforeInvoke();
            try
            {
                string xml = null;
                if (parameter is string)
                {
                    xml = (string)parameter;
                }
                else if (parameter is JsonElement)
                {
                    JsonElement ele = (JsonElement)parameter;
                    if (ele.ValueKind == JsonValueKind.String)
                    {
                        xml = ele.GetString();
                    }
                }//wyc20230711:试图插入内容后选中内容增加高亮辅助提示,若当前有选区，先清除选区再插
                int startindex = this._Control.Document.Content.SelectionStartIndex;
                if (this._Control.Document.Content.SelectionLength != 0)
                {
                    this._Control.Document.Content.SetSelection(startindex, 0);
                }
                object o = this._Control.ExecuteCommand(StandardCommandNames.InsertXML, false, xml);
                // 修复插入xml存在逻辑删除内容的问题【DUWRITER5_0-4335】
                var currentEl = this._Control.Document.CurrentContentElement;
                if (currentEl != null && currentEl.Parent != null)
                {
                    currentEl.Parent.EditorRefreshView();
                }
                int endindex = this._Control.Document.Content.SelectionStartIndex;
                this._Control.Document.Content.SetSelection(endindex, startindex - endindex);
                return o != null;
            }
            catch (Exception ex)
            {
                JavaScriptMethods.Tools_ReportException("insertXmlString", ex.Message, ex.ToString(), 0);
                return false;
            }
        }

        /// <summary>
        /// 在指定的输入域内插入文档
        /// </summary>
        /// <returns></returns>
        [JSInvokable]
        public bool InsertXmlById(JsonElement obj, JsonElement id)
        {
            CheckStateBeforeInvoke();
            try
            {
                DomContainerElement input = null;
                if (id.ValueKind == JsonValueKind.String)
                {
                    input = this._Control.GetElementById(id.GetString()) as DomContainerElement;
                }
                else if (id.ValueKind == JsonValueKind.Number)
                {
                    input = this.GetElementByNativeHandle(id.GetInt32()) as DomContainerElement;
                }
                if (input == null)
                {
                    return false;
                }
                string fileformat = "xml";
                bool isBase64 = false;
                //bool append = true; //从尾部插入
                string filexml = null;
                string positionstr = null;


                bool logundo = false;

                if (obj.ValueKind != JsonValueKind.Object)
                {
                    return false;
                }

                foreach (JsonProperty property in obj.EnumerateObject())
                {
                    string name = property.Name.ToLower();
                    switch (name)
                    {
                        case "file":
                            if (property.Value.ValueKind == JsonValueKind.String)
                            {
                                filexml = property.Value.GetString();
                            }
                            break;
                        case "format":
                            if (property.Value.ValueKind == JsonValueKind.String)
                            {
                                fileformat = property.Value.GetString();
                            }
                            break;
                        case "base64":
                            isBase64 = WASMUtils.ConvertToBoolean(property, false);// WASMJsonConvert.parseBoolean(property.Value, false);
                            break;
                        case "position":
                            if (property.Value.ValueKind == JsonValueKind.String)
                            {
                                positionstr = property.Value.GetString().ToLower();
                            }
                            break;
                        case "logundo":
                            logundo = WASMUtils.ConvertToBoolean(property, false);
                            break;
                        default:
                            break;
                    }
                }
                if (filexml == null)
                {
                    return false;
                }

                DomDocument document = new DomDocument();
                if (isBase64)
                {
                    document.LoadFromBase64String(filexml, fileformat);
                }
                else
                {
                    document.LoadFromString(filexml, fileformat);
                }


                this._Control.Document.ImportElements(document.Body.Elements);

                var newList = document.Body.Elements.Clone();
                if (newList != null && newList.Count >= 1)
                {//必须大于一个元素，如果只有一个元素，就认定为只是段落符号，不操作它。
                    var lastElement = newList[newList.Count - 1];
                    if (lastElement != null && lastElement is DomParagraphFlagElement)
                    {
                        newList.RemoveAt(newList.Count - 1);//移除最后的段落符号
                    }
                    
                    switch(positionstr)
                    {
                        case "start":
                            if (logundo)
                            {
                                //foreach(XTextElement element in input.Elements)
                                //{
                                //    newList.Add(element);
                                //}
                                //wyc20230830:新增记录撤销功能
                                this._Control.Document.BeginLogUndo();
                                ReplaceElementsArgs rea = new ReplaceElementsArgs();
                                rea.LogUndo = true;
                                rea.NewElements = newList;
                                rea.Container = input;
                                rea.StartIndex = 0;
                                rea.DeleteLength = 0;// input.Elements.Count;
                                this._Control.Document.ReplaceElements(rea);
                                this._Control.Document.EndLogUndo();
                            }
                            else
                            {
                                input.Elements.InsertRangeRaw(0, newList);
                            }
                            break;
                        case "end":
                            int index = input.Elements.Count - 1;
                            while (index > 0 && input.Elements[index] is DomParagraphFlagElement)
                            {
                                index--;
                            }
                            if (logundo)
                            {
                                //wyc20230830:新增记录撤销功能
                                this._Control.Document.BeginLogUndo();
                                ReplaceElementsArgs rea = new ReplaceElementsArgs();
                                rea.LogUndo = true;
                                rea.NewElements = newList;
                                rea.Container = input;
                                rea.StartIndex = index + 1;
                                this._Control.Document.ReplaceElements(rea);
                                this._Control.Document.EndLogUndo();
                            }
                            else
                            {
                                input.Elements.InsertRangeRaw(index + 1, newList);
                            }
                            break;
                        case "append":
                            if (logundo)
                            {
                                //wyc20230830:新增记录撤销功能
                                this._Control.Document.BeginLogUndo();
                                ReplaceElementsArgs rea = new ReplaceElementsArgs();
                                rea.LogUndo = true;
                                rea.NewElements = newList;
                                rea.Container = input;
                                rea.StartIndex = input.Elements.Count;
                                this._Control.Document.ReplaceElements(rea);
                                this._Control.Document.EndLogUndo();
                            }
                            else
                            {
                                input.Elements.AddRangeByDCList(newList);
                            }
                            break;
                        default:
                            break;
                    }
                    input.EditorRefreshView();
                    //document.Dispose();//Dispose的话底层会报错
                    return true;
                }
                input.EditorRefreshView();
                //document.Dispose();//Dispose的话底层会报错
                return false;
            }
            catch (Exception ex)
            {
                JavaScriptMethods.Tools_ReportException("InsertXmlById", ex.Message, ex.ToString(), 0);
                return false;
            }
        }

//        /// wyc20230526与四代兼容接口
//        /// <summary>
//        /// 分别提供页眉，文档体，页脚的XML来组合成一个文档进行加载
//        /// </summary>
//        /// <returns></returns>
//        [DevAPI("2023-8-3", "第五代.分别提供页眉，文档体，页脚的XML来组合成一个文档进行加载")]
//        [DevLog("2023-8-3", "刘帅", null, "创建API")]
//        [DevLog("2024-3-4", "wyc", null, "对header/footer置null才会清空页眉页脚，不设置或空字符串则不处理")]
//        [DevLog("2025-4-27", "wyc", null, "补充处理首页眉页脚的内容填充")]
//        [JSInvokable]
//        public bool loadDocumentFromMixString(JsonElement parameter)
//        {
//            CheckStateBeforeInvoke();
//#if LightWeight
//            throw new LightWeightNotSupportedException("loadDocumentFromMixString");
//#else
//            try
//            {
//                if (parameter.ValueKind != JsonValueKind.Object)
//                {
//                    return false;
//                }
//                string fileFormat = "xml";
//                bool usebase64 = false;

//                string headercontent = null;
//                bool isclearheader = false;
//                string bodycontent = null;
//                //bool isclearbody = false;
//                string footercontent = null;
//                bool isclearfooter = false;

//                foreach (JsonProperty property in parameter.EnumerateObject())
//                {
//                    string name = property.Name.ToLower();
//                    switch (name)
//                    {
//                        case "fileformat":
//                            if (property.Value.ValueKind == JsonValueKind.String)
//                            {
//                                fileFormat = property.Value.GetString();
//                            }
//                            break;
//                        case "usebase64string":
//                            usebase64 = WASMUtils.ConvertToBoolean(property, false);// WASMJsonConvert.parseBoolean(property.Value, false);
//                            break;
//                        case "headercontent":
//                            if (property.Value.ValueKind == JsonValueKind.String)
//                            {
//                                headercontent = property.Value.GetString();
//                            }
//                            else if (property.Value.ValueKind == JsonValueKind.Null)
//                            {
//                                isclearheader = true;
//                            }
//                            break;
//                        case "bodycontent":
//                            if (property.Value.ValueKind == JsonValueKind.String)
//                            {
//                                bodycontent = property.Value.GetString();
//                            }
//                            else if (property.Value.ValueKind == JsonValueKind.Null)
//                            {
//                                //isclearbody = true;
//                            }
//                            break;
//                        case "footercontent":
//                            if (property.Value.ValueKind == JsonValueKind.String)
//                            {
//                                footercontent = property.Value.GetString();
//                            }
//                            else if (property.Value.ValueKind == JsonValueKind.Null)
//                            {
//                                isclearfooter = true;
//                            }
//                            break;
//                        default:
//                            break;
//                    }
//                }
//                XTextDocument mainDoc = new XTextDocument();
//                XTextDocument headDoc = null;// new XTextDocument();
//                XTextDocument footDoc = null;// new XTextDocument();

//                if (headercontent != null && headercontent.Length > 0)
//                {
//                    headDoc = new XTextDocument();
//                    if (usebase64)
//                    {
//                        headDoc.LoadFromBase64String(headercontent, fileFormat);
//                    }
//                    else
//                    {
//                        headDoc.LoadFromString(headercontent, fileFormat);
//                    }
//                }

//                if (footercontent != null && footercontent.Length > 0)
//                {
//                    footDoc = new XTextDocument();
//                    if (usebase64)
//                    {
//                        footDoc.LoadFromBase64String(footercontent, fileFormat);
//                    }
//                    else
//                    {
//                        footDoc.LoadFromString(footercontent, fileFormat);
//                    }

//                }

//                if (bodycontent != null && bodycontent.Length > 0)
//                {
//                    if (usebase64)
//                    {
//                        mainDoc.LoadFromBase64String(bodycontent, fileFormat);
//                    }
//                    else
//                    {
//                        mainDoc.LoadFromString(bodycontent, fileFormat);
//                    }

//                    //wyc20240301:追加清空页眉页脚，只加载body时只保留文档体
//                    if (isclearheader == true)
//                    {
//                        mainDoc.Header.Elements.Clear();
//                    }
//                    if (isclearfooter == true)
//                    {
//                        mainDoc.Footer.Elements.Clear();
//                    }
//                }

//                if (headDoc != null)
//                {
//                    mainDoc.Header.Elements.Clear();
//                    mainDoc.ImportElements(headDoc.Header.Elements);
//                    foreach (var element in headDoc.Header.Elements)
//                    {
//                        mainDoc.Header.Elements.Add(element);
//                    }
//                }

//                if (footDoc != null)
//                {
//                    mainDoc.Footer.Elements.Clear();
//                    mainDoc.ImportElements(footDoc.Footer.Elements);
//                    foreach (var element in footDoc.Footer.Elements)
//                    {
//                        mainDoc.Footer.Elements.Add(element);
//                    }
//                }
//                this._ElementNativeHandles?.Clear();
//                this._Control.GetInnerViewControl().LoadDocumentFromString(mainDoc.SaveToString("xml"), "xml");
//                //this._Control.Document = mainDoc;
//                this._Control.RefreshInnerView(false);
//                //this._Control.GetInnerViewControl().LoadDocumentFromString(mainDoc.SaveToString("xml"), "xml");
//                this._ElementNativeHandles?.Clear();
//                return false;
//            }
//            catch (Exception ex)
//            {
//                JavaScriptMethods.Tools_ReportException("loadDocumentFromMixString", ex.Message, ex.ToString(), 0);
//                return false;
//            }
//#endif
//        }

//        /// <summary>
//        /// 根据base64字符串获取到HTML字符串
//        /// </summary>
//        /// <returns></returns>
//        [DevAPI("2023-8-3", "第五代.根据base64字符串获取到HTML字符串")]
//        [DevLog("2023-8-3", "刘帅", null, "创建API")]
//        [JSInvokable]
//        public string getHtmlByXMLBase64String(JsonElement parameter)
//        {
//#if LightWeight
//            throw new LightWeightNotSupportedException("getHtmlByXMLBase64String");
//#else
//            try
//            {
//                if (parameter.ValueKind != JsonValueKind.String)
//                {
//                    return null;
//                }
//                string str = parameter.GetString();
//                XTextDocument doc = new XTextDocument();
//                try
//                {
//                    doc.LoadFromBase64String(str, "xml");
//                }
//                catch
//                {
//                    doc = null;
//                }
//                if (doc == null)
//                {
//                    return null;
//                }
//                doc.RefreshDocument();
//                return doc.SaveToString("html");
//            }
//            catch (Exception ex)
//            {
//                JavaScriptMethods.Tools_ReportException("getHtmlByXMLBase64String", ex.Message, ex.ToString(), 0);
//                return "";
//            }
//#endif
//        }
//        /// <summary>
//        /// 根据XML字符串获取到HTML字符串
//        /// </summary>
//        /// <returns></returns>
//        [DevAPI("2023-8-15", "第五代.根据XML字符串获取到HTML字符串")]
//        [DevLog("2023-8-15", "刘帅", null, "创建API")]
//        [JSInvokable]
//        public string getHtmlByXMLString(JsonElement parameter)
//        {
//#if LightWeight
//            throw new LightWeightNotSupportedException("getHtmlByXMLString");
//#else
//            try
//            {
//                if (parameter.ValueKind != JsonValueKind.String)
//                {
//                    return null;
//                }
//                string str = parameter.GetString();
//                XTextDocument doc = new XTextDocument();
//                try
//                {
//                    doc.LoadFromString(str, "xml");
//                }
//                catch
//                {
//                    doc = null;
//                }
//                if (doc == null)
//                {
//                    return null;
//                }
//                doc.RefreshDocument();
//                return doc.SaveToString("html");
//            }
//            catch (Exception ex)
//            {
//                JavaScriptMethods.Tools_ReportException("getHtmlByXMLString", ex.Message, ex.ToString(), 0);
//                return "";
//            }
//#endif
//        }
         

        /// <summary>
        /// 获取文档的选项
        /// </summary>
        /// <returns></returns>
        [JSInvokable]
        public DocumentBehaviorOptions GetBehaviorOptions()
        {
#if LightWeight
            throw new LightWeightNotSupportedException("GetBehaviorOptions");
#else
            try
            {
                var options = this._Control.Document.Options.BehaviorOptions;
                return options;
            }
            catch (Exception ex)
            {
                JavaScriptMethods.Tools_ReportException("GetBehaviorOptions", ex.Message, ex.ToString(), 0);
                return null;
            }
#endif
        }

        /// <summary>
        /// 获取文档的选项
        /// </summary>
        /// <returns></returns>
        [JSInvokable]
        public DocumentEditOptions GetEditOptions()
        {
            try
            {
                var options = this._Control.Document.Options.EditOptions;
                return options;
            }
            catch (Exception ex)
            {
                JavaScriptMethods.Tools_ReportException("GetEditOptions", ex.Message, ex.ToString(), 0);
                return null;
            }
        }

        /// <summary>
        /// 获取文档的选项
        /// </summary>
        /// <returns></returns>
        [JSInvokable]
        public DocumentViewOptions GetViewOptions()
        {
            try
            {
                var options = this._Control.Document.Options.ViewOptions;
                return options;
            }
            catch (Exception ex)
            {
                JavaScriptMethods.Tools_ReportException("GetViewOptions", ex.Message, ex.ToString(), 0);
                return null;
            }
        }
         
        /// <summary>
        /// 兼容第四代获取文档的总页数
        /// </summary>
        /// <returns></returns>
        [JSInvokable]
        public int GetDocumentPageNum()
        {
            CheckStateBeforeInvoke();
            try
            {
                return this.PageCount;
            }
            catch (Exception ex)
            {
                JavaScriptMethods.Tools_ReportException("GetDocumentPageNum", ex.Message, ex.ToString(), 0);
                return -1;
            }
        }


        /// <summary>
        /// 兼容第四代获取正文内容
        /// </summary>
        /// <returns></returns>
        [JSInvokable]
        public string SaveBodyDocumentToString(string fileFormat)
        {
            CheckStateBeforeInvoke();
            try
            {
                if (fileFormat == null || fileFormat.Length == 0 || fileFormat == "")
                {
                    fileFormat = "xml";
                }
                var strContent = this._Control.Document.SaveToString("XML");
                DomDocument document = new DomDocument();
                document.LoadFromString(strContent, "XML");

                document.Header.Clear();
                document.Footer.Clear();
                return document.SaveToString(fileFormat);
            }
            catch (Exception ex)
            {
                JavaScriptMethods.Tools_ReportException("SaveBodyDocumentToString", ex.Message, ex.ToString(), 0);
                return "";
            }
        }

        /// <summary>
        /// 判断当前位置能否插入内容
        /// </summary>
        /// <returns></returns>
        [JSInvokable]
        public bool CanInsertAtCurrentPosition()
        {
            CheckStateBeforeInvoke();
            try
            {
                return this._Control.Document.DocumentControler.CanInsertAtCurrentPosition;
            }
            catch (Exception ex)
            {
                JavaScriptMethods.Tools_ReportException("CanInsertAtCurrentPosition", ex.Message, ex.ToString(), 0);
                return false;
            }
        }

        /// <summary>
        /// 判断当前能否插入指定类型的元素
        /// </summary>
        /// <param name="typeaName"></param>
        /// <returns></returns>
        [JSInvokable]
        public bool CanInsertElementAtCurrentPosition(string typeaName)
        {
            CheckStateBeforeInvoke();
            try
            {

                if (typeaName == null || typeaName == "" || typeaName.Length == 0)
                {
                    return this._Control.Document.DocumentControler.CanInsertAtCurrentPosition;
                }
                typeaName = typeaName.ToLower();

                if ( string.Equals( typeaName, DomInputFieldElement.TypeName_XTextInputFieldElement , StringComparison.OrdinalIgnoreCase))
                {
                    return this._Control.Document.DocumentControler.CanInsertElementAtCurrentPosition(typeof(DomInputFieldElement));
                }
                else if (string.Equals(typeaName, DomRadioBoxElement.TypeName_XTextRadioBoxElement , StringComparison.OrdinalIgnoreCase))
                {
                    return this._Control.Document.DocumentControler.CanInsertElementAtCurrentPosition(typeof(DomRadioBoxElement));
                }
                else if (string.Equals(typeaName, DomInputFieldElement.TypeName_XTextInputFieldElement, StringComparison.OrdinalIgnoreCase))
                {
                    return this._Control.Document.DocumentControler.CanInsertElementAtCurrentPosition(typeof(DomCheckBoxElement));
                }
                else if (string.Equals(typeaName,DomTableElement.TypeName_XTextTableElement, StringComparison.OrdinalIgnoreCase))
                {
                    return this._Control.Document.DocumentControler.CanInsertElementAtCurrentPosition(typeof(DomTableElement));
                }
                else if (string.Equals(typeaName, DomImageElement.TypeName_XTextImageElement , StringComparison.OrdinalIgnoreCase))
                {
                    return this._Control.Document.DocumentControler.CanInsertElementAtCurrentPosition(typeof(DomImageElement));
                }
                else if (string.Equals(typeaName, DomPageInfoElement.TypeName_XTextPageInfoElement , StringComparison.OrdinalIgnoreCase))
                {
                    return this._Control.Document.DocumentControler.CanInsertElementAtCurrentPosition(typeof(DomPageInfoElement));
                }
                else if (string.Equals(typeaName, DomDocument.TypeName_XTextDocument , StringComparison.OrdinalIgnoreCase))
                {
                    return this._Control.Document.DocumentControler.CanInsertElementAtCurrentPosition(typeof(DomDocument));
                }
                else if (string.Equals(typeaName, DomStringElement.TypeName_XTextStringElement, StringComparison.OrdinalIgnoreCase))
                {
                    return this._Control.Document.DocumentControler.CanInsertElementAtCurrentPosition(typeof(DomStringElement));
                }
                else
                {
                    return this._Control.Document.DocumentControler.CanInsertAtCurrentPosition;
                }
            }
            catch (Exception ex)
            {
                JavaScriptMethods.Tools_ReportException("CanInsertAtCurrentPosition", ex.Message, ex.ToString(), 0);
                return false;
            }
        }

        /// <summary>
        /// 获取选择内容的开始位置
        /// </summary>
        /// <returns></returns>
        [JSInvokable]
        public int GetSelectionStartIndex()
        {
            CheckStateBeforeInvoke();
            try
            {
                if (this._Control.Document.Selection != null)
                {
                    return this._Control.Document.Selection.StartIndex;
                }
                return -1;
            }
            catch (Exception ex)
            {
                JavaScriptMethods.Tools_ReportException("GetSelectionStartIndex", ex.Message, ex.ToString(), 0);
                return -1;
            }
        }

        /// <summary>
        /// 获取选择内容的结束位置
        /// </summary>
        /// <returns></returns>
        [JSInvokable]
        public int GetSelectionEndIndex()
        {
            CheckStateBeforeInvoke();
            try
            {
                if (this._Control.Document.Selection != null)
                {
                    return this._Control.Document.Selection.AbsEndIndex;
                }
                return -1;
            }
            catch (Exception ex)
            {
                JavaScriptMethods.Tools_ReportException("GetSelectionEndIndex", ex.Message, ex.ToString(), 0);
                return -1;
            }
        }


        [JSInvokable]
        public bool InsertXMLbyEementTag(JsonElement id, string content, string format, string tag)
        {
            CheckStateBeforeInvoke();
            try
            {
                DomElement element = null;
                if (id.ValueKind == JsonValueKind.String)
                {
                    element = this._Control.GetElementById(id.GetString());
                }
                else if (id.ValueKind == JsonValueKind.Number)
                {
                    element = this.GetElementByNativeHandle(id.GetInt32());
                }
                if (element == null)
                {
                    return false;
                }

                return InnerInsertXMLbyEementTag(content, format, tag, element);
            }
            catch (Exception ex)
            {
                JavaScriptMethods.Tools_ReportException("InsertXMLbyEementTag", ex.Message, ex.ToString(), 0);
                return false;
            }
        }
        [JSInvokable]
        public bool InsertXMLbyEementTag2(DotNetObjectReference<DomElement> ele, string content, string format, string tag)
        {
            CheckStateBeforeInvoke();
            try
            {
                DomElement element = null;
                if (ele != null && ele.Value != null)
                {
                    element = ele.Value;
                }

                return InnerInsertXMLbyEementTag(content, format, tag, element);
            }
            catch (Exception ex)
            {
                JavaScriptMethods.Tools_ReportException("InsertXMLbyEementTag2", ex.Message, ex.ToString(), 0);
                return false;
            }
        }

        private bool InnerInsertXMLbyEementTag(string content, string format, string tag, DomElement element)
        {
            if (string.IsNullOrEmpty(content))
            {
                return false;
            }
            if (string.IsNullOrEmpty(format))
            {
                format = "xml";
            }
            if (string.IsNullOrEmpty(tag))
            {
                tag = "after";
            }
            tag = tag.ToLower();
            if (tag != "after" && tag != "befor")
            {
                return false;
            }
            DomContainerElement parentElement = element.Parent;
            if (parentElement != null)
            {
                DomDocument document = new DomDocument();
                document.LoadFromString(content, format);
                this._Control.Document.ImportElements(document.Body.Elements);
                if (tag == "after")
                {//后面
                    int index = parentElement.Elements.IndexOf(element);
                    DomElementList listElement = document.Body.Elements.Clone();
                    parentElement.Elements.InsertRange(index + 1, listElement);
                    listElement = null;
                    document = null;
                    parentElement.EditorRefreshView();
                    return true;
                }
                if (tag == "befor")
                {//前面
                    DomElementList listElement = document.Body.Elements.Clone();
                    parentElement.Elements.InsertRangeBefore(element, listElement);
                    listElement = null;
                    document = null;
                    parentElement.EditorRefreshView();
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 修改全文的样式
        /// </summary>
        /// <returns></returns>
        [JSInvokable]
        public bool UpdateDocumentAllStyle(JsonElement styleObj)
        {
            CheckStateBeforeInvoke();
            try
            {
                if (styleObj.ValueKind != JsonValueKind.Null)
                {
                    //修改默认样式
                    ContentStyle defaulyStyle = this._Control.Document.ContentStyles.Default;
                    bool changed = false;
                    DocumentContentStyle dcs = WASMJsonConvert.JSONToDocumentContentStyle(styleObj, (DocumentContentStyle)defaulyStyle, out changed);
                    this._Control.Document.ContentStyles.Default = dcs;

                    //循环修改所有样式
                    ContentStyleList styles = this._Control.Document.ContentStyles.Styles;
                    if (styles != null && styles.Count > 0)
                    {
                        for (int i = 0; i < styles.Count; i++)
                        {
                            ContentStyle style = styles[i];
                            bool changed2 = false;
                            DocumentContentStyle dcs2 = WASMJsonConvert.JSONToDocumentContentStyle(styleObj, (DocumentContentStyle)style, out changed2);
                            styles[i] = dcs2;
                        }
                    }
                    this._Control.RefreshDocument();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                JavaScriptMethods.Tools_ReportException("UpdateDocumentAllStyle", ex.Message, ex.ToString(), 0);
                return false;
            }
        }
    }
}