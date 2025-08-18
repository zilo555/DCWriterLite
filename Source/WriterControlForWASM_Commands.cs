using DCSoft.Common;
using DCSoft.Drawing;
using DCSoft.Writer;
using DCSoft.Writer.Commands;
using DCSoft.Writer.Data;
using DCSoft.Writer.Dom;
using System.Text.Json;
using System.Text.Json.Nodes;
using DCSoft.WinForms.Design;
using DCSoft.WinForms;
using System.Text.Json.Serialization;
using DCSoft.Writer.Dom.Undo;
using DCSoft.Writer.Undo;
using DCSoft.Printing;
using DCSoft.Data;

namespace DCSoft.WASM
{
    /// <summary>
    /// 此处对 WriterControl的接口进行转发调用
    /// 对应的，需要在前端 WriterControl_Main.js中的 BindControl()中添加对应的调用。
    /// </summary>
    partial class WriterControlForWASM
    {


        /// 和旧版BS编辑器命令保持兼容 wyc20230509
        /// <summary>
        /// 执行命令
        /// </summary>
        /// <param name="commandName">命令名称</param>
        /// <param name="showUI">是否显示UI</param>
        /// <param name="parameter">参数</param>
        /// <returns>执行结果</returns>
        [JSInvokable]
        public JsonNode DCExecuteCommand(string commandName, bool showUI, object parameter)
        {
            CheckStateBeforeInvoke();
            try
            {
                string cmdName = commandName.ToLower();
                switch (cmdName)
                {
                    case "table_deletetable":
                        return command_deletetable(parameter);//删除指定ID的表格
                    case "table_deletecolumn":
                        return command_deletetablecolumn(parameter);//删除指定的表格列
                    case "table_deleterow":
                        return command_deletetablerow(parameter);//删除指定的表格行
                    case "table_insertrowdown":
                        return command_tableinsertrowdown(parameter, true);//在指定表格的表格行下面插入行
                    case "table_insertrowup":
                        return command_tableinsertrowdown(parameter, false);//在指定表格的表格行下面插入行
                    case "insertcheckboxorradio":
                        return command_insertcheckboxradio(showUI, parameter);
                    case "insertinputfield":
                        return command_insertinputfield(showUI, parameter);
                    case "insertimage":
                        return command_insertimageelement(showUI, parameter);
                    case "dcinsertimage": //与四代兼容命令
                        return command_insertimageelement(showUI, parameter);
                    case "indent":
                        return command_FirstLineIndent();//首行缩进
                    case "hangingindent":
                        return command_HangIndent(parameter);//悬挂缩进
                    case "insertorderedlist":
                        return command_InsertOrderedList(parameter);//有序段落列表
                    case "insertunorderedlist":
                        return command_InsertUnOrderedList(parameter);//无序段落列表
                    case "lineheight":
                        return command_LineHeight(parameter);//行间距
                    case "rowspacing":
                        return command_Rowspacing(parameter);//段前距、段后距
                    case "inserttable":
                        return command_InsertTable(parameter);//插入表格
                    case "insertpageinfoelement":
                        return command_insertpageinfoelement(showUI, parameter);//插入页码
                    case "insertxml":
                        return command_insertxml(showUI, parameter);//插入XML
                    //wyc20230523:简单命令也支持传true/false参数
                    case "bold":
                        return command_BoldItalicUnderlineStrikethroughSubscriptSupscript(cmdName, showUI, parameter);
                    case "italic":
                        return command_BoldItalicUnderlineStrikethroughSubscriptSupscript(cmdName, showUI, parameter);
                    case "underline":
                        return command_BoldItalicUnderlineStrikethroughSubscriptSupscript(cmdName, showUI, parameter);
                    case "strikethrough":
                        return command_BoldItalicUnderlineStrikethroughSubscriptSupscript(cmdName, showUI, parameter);
                    case "subscript":
                        return command_BoldItalicUnderlineStrikethroughSubscriptSupscript(cmdName, showUI, parameter);
                    case "supscript":
                        return command_BoldItalicUnderlineStrikethroughSubscriptSupscript(cmdName, showUI, parameter);

                    //wyc颜色相关
                    case "color":
                        return command_color(cmdName, showUI, parameter);
                    case "backcolor":
                        return command_color(cmdName, showUI, parameter);
                    case "colorforfieldtextcolor":
                        return command_colorpow(cmdName, showUI, parameter);

                    //wyc20230524插入字体
                    case "spechars":
                        return command_insertstring(cmdName, showUI, parameter);
                    case "insertstring":
                        return command_insertstring(cmdName, showUI, parameter);

                    case "documentvaluevalidate":
                        return command_documentvaluevalidate(showUI, parameter);
                    case "deletefield":
                        return command_deletefield(showUI, parameter);
                    case "removeformat"://兼容四代，清除格式
                        return Convert.ToBoolean(this._Control.ExecuteCommand(StandardCommandNames.ClearFormat, showUI, null));
                    case "removefontsize"://兼容四代，清除字体大小
                        var defaultFontSize = this._Control.Document.DefaultFont.Size;
                        return Convert.ToBoolean(this._Control.ExecuteCommand(StandardCommandNames.FontSize, showUI, defaultFontSize));
                    case "removefontfamily"://兼容四代，清除字体名称
                        var defaultFontName = this._Control.Document.DefaultFont.Name;
                        return Convert.ToBoolean(this._Control.ExecuteCommand(StandardCommandNames.FontName, showUI, defaultFontName));
                    case "cleardoc":
                        return Convert.ToBoolean(this._Control.ExecuteCommand(StandardCommandNames.FileNew, showUI, null));
                    case "fontname":
                        if (parameter != null)
                        {
                            JsonElement parameter2 = (JsonElement)parameter;
                            if (parameter2.ValueKind == JsonValueKind.String)
                            {
                                string value = parameter2.GetString();
                                return Convert.ToBoolean(this._Control.ExecuteCommand(StandardCommandNames.FontName, showUI, value));
                            }
                        }
                        break;
                    case "fontsize":
                        if (parameter != null)
                        {
                            JsonElement parameter2 = (JsonElement)parameter;
                            if (parameter2.ValueKind == JsonValueKind.String)
                            {
                                string value = parameter2.GetString();
                                float size = 12f;
                                if (value.Contains("px"))
                                {
                                    value = value.TrimEnd("px".ToCharArray());
                                    //wyc20241017：优化不同前端输入的字号处理逻辑
                                    if (float.TryParse(value, out size) == true)
                                    {
                                        size = size * 3f / 4f;
                                    }
                                    else
                                    {
                                        size = 12f;
                                    }
                                }
                                else if (value.Contains("pt"))
                                {
                                    value = value.TrimEnd("pt".ToCharArray());
                                    if (float.TryParse(value, out size) == false)
                                    {
                                        size = 12f;
                                    }
                                }
                                else if (float.TryParse(value, out size) == false)
                                {
                                    size = 12f;
                                }
                                return Convert.ToBoolean(this._Control.ExecuteCommand(StandardCommandNames.FontSize, showUI, size));
                            }
                            else if (parameter2.ValueKind == JsonValueKind.Number)
                            {
                                float value = parameter2.GetSingle();
                                return Convert.ToBoolean(this._Control.ExecuteCommand(StandardCommandNames.FontSize, showUI, value));
                            }
                            /////////////////////////////////////////////////////
                        }
                        break;
                    case "fontsizepro":
                        if (parameter != null)
                        {
                            JsonElement parameter2 = (JsonElement)parameter;
                            if (parameter2.ValueKind == JsonValueKind.String)
                            {
                                string value = parameter2.GetString();
                                if (value.Contains("px"))
                                {
                                    value = value.TrimEnd("px".ToCharArray());
                                }
                                command_fontsizepro(value);
                                return Convert.ToBoolean(this._Control.ExecuteCommand(StandardCommandNames.FontSize, showUI, value));
                            }
                        }
                        break;
                    case "insertparagraphbeforetable":
                        return command_insertparagraphbeforetable();
                    case "date":
                        if (parameter != null)
                        {
                            JsonElement parameter2 = (JsonElement)parameter;
                            if (parameter2.ValueKind == JsonValueKind.String)
                            {
                                string value = parameter2.GetString();
                                return Convert.ToBoolean(this._Control.ExecuteCommand(StandardCommandNames.InsertString, showUI, value));
                            }
                        }
                        break;
                    case "time":
                        if (parameter != null)
                        {
                            JsonElement parameter2 = (JsonElement)parameter;
                            if (parameter2.ValueKind == JsonValueKind.String)
                            {
                                string value = parameter2.GetString();
                                return Convert.ToBoolean(this._Control.ExecuteCommand(StandardCommandNames.InsertString, showUI, value));
                            }
                        }
                        break;
                    case "insertspecifycharacter":
                        if (parameter != null)//特殊字符
                        {
                            JsonElement parameter2 = (JsonElement)parameter;
                            if (parameter2.ValueKind == JsonValueKind.String)
                            {
                                string value = parameter2.GetString();
                                this._Control.ExecuteCommand(StandardCommandNames.InsertString, showUI, value);
                                return true;
                            }
                        }
                        break;

                    case "fontborder":
                        command_fontborder(parameter);
                        break;
                    case "touppercase":
                        return command_ToUppercase(true);
                    case "tolowercase":
                        return command_ToUppercase(false);
                    case "table_mergecell"://合并单元格
                        if (parameter != null)
                        {//扩展命令支持参数
                            return command_tablemergecell(parameter);
                        }
                        MegeCellCommandParameter param = this._Control.ExecuteCommand(commandName, showUI, parameter) as MegeCellCommandParameter;
                        if (param != null && param.Cell != null)
                        {//此时先返回布尔值，后期再根据客户需求添加返回合并的单元格对象
                            this._Control.Document.Modified = true;

                            //wyc20250408:需要先执行完合并并刷新表格后，再针对参数里的值进行修正
                            //否则会有绘制问题DUWRITER5_0-4329
                            //param.Cell.RowSpan = param.Cell.RowSpan - param.NewRowSpanFix;
                            //param.Cell.ColSpan = param.Cell.ColSpan - param.NewColSpanFix;

                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    case "table_splitcellext":
                        //拆分单元格
                        return command_tablesplitcellext(parameter);
                    case "inputfieldunderline":
                        if (parameter != null)
                        {//为兼容四代添加
                            return command_inputfieldunderline(parameter);
                        }
                        else
                        {//为空就是在切换
                            DomInputFieldElement input = this._Control.Document.CurrentInputField;
                            if (input != null && input.Style != null)
                            {
                                bool isUser = input.Style.BorderBottom;//是否有下划线
                                if (isUser)
                                {//取消
                                    input.Style.BorderTop = false;
                                    input.Style.BorderLeft = false;
                                    input.Style.BorderRight = false;
                                    input.Style.BorderBottom = false;
                                }
                                else
                                {//设置下划线
                                    input.Style.BorderTop = false;
                                    input.Style.BorderLeft = false;
                                    input.Style.BorderRight = false;
                                    input.Style.BorderBottom = true;
                                    //input.Style.BorderBottomColor = parameter.InputFieldUnderLineColor;
                                    input.Style.BorderStyle = DashStyle.Solid;
                                    input.Style.BorderWidth = 1;
                                }
                                input.EditorRefreshView();
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        }
                    case "alllineheight": //wyc20230710
                        return command_alllineheight(parameter);
                    case "table_writedatatotable": //wyc20230821
                        return command_table_writedatatotable((JsonElement)parameter);
                    case "movetopage":
                        if (parameter != null)
                        {
                            JsonElement parameter2 = (JsonElement)parameter;
                            if (parameter2.ValueKind == JsonValueKind.String)
                            {
                                string value = parameter2.GetString();
                                return Convert.ToBoolean(this._Control.ExecuteCommand(StandardCommandNames.MoveToPage, showUI, value));
                            }
                            if (parameter2.ValueKind == JsonValueKind.Number)
                            {
                                int pageNum = 0;
                                if (parameter2.TryGetInt32(out pageNum))
                                {
                                    return Convert.ToBoolean(this._Control.ExecuteCommand(StandardCommandNames.MoveToPage, showUI, pageNum));
                                }
                            }
                        }
                        break;
                    case "underlinestyle":
                        return command_UnderlineStyle(parameter);
                    //wyc20240716:细化fileprint命令，对外暴露参数接口
                    case "fileprint":
                        if (parameter == null || (parameter is JsonElement) == false)
                        {
                            return WASMJsonConvert.parseBoolean(this._Control.ExecuteCommand(commandName, showUI, parameter), false);
                        }
                        else
                        {
                            return command_fileprint(showUI, (JsonElement)parameter);
                        }
                    default:
                        return WASMJsonConvert.parseBoolean(this._Control.ExecuteCommand(commandName, showUI, parameter), false);
                }
                return false;
            }
            catch (Exception ex)
            {
                JavaScriptMethods.Tools_ReportException("DCExecuteCommand", ex.Message, ex.ToString(), 0);
                return false;
            }
        }


        private JsonNode command_fileprint(bool showui, JsonElement parameter)
        {
            DCSoft.Writer.Commands.FilePrintCommandParameter fpcp = new FilePrintCommandParameter();
            if (parameter.ValueKind == JsonValueKind.Object)
            {
                foreach (JsonProperty property in parameter.EnumerateObject())
                {
                    switch (property.Name.ToLower())
                    {
                        case "cleanprintmode":
                            fpcp.CleanPrintMode = WASMUtils.ConvertToBoolean(property, false);
                            break;
                        case "manualduplex":
                            fpcp.ManualDuplex = WASMUtils.ConvertToBoolean(property, false);
                            break;
                        case "specifycopies":
                            fpcp.SpecifyCopies = WASMUtils.ConvertToInt32(property, 0);
                            break;
                        case "specifypageindexsstring":
                            fpcp.SpecifyPageIndexsStringBase1 = WASMUtils.ConvertToString(property);
                            break;
                        case "specifyprintername":
                            fpcp.SpecifyPrinterName = WASMUtils.ConvertToString(property);
                            break;
                        default:
                            break;
                    }
                }
            }
            return WASMJsonConvert.parseBoolean(this._Control.ExecuteCommand("FilePrint", showui, fpcp), false); ;
        }

        private bool command_UnderlineStyle(object parameter)
        {
            if (parameter != null)
            {
                JsonElement parameter2 = (JsonElement)parameter;
                if (parameter2.ValueKind == JsonValueKind.Object)
                {
                    DCSystem_Drawing.Color mycolor = DCSystem_Drawing.Color.Black;

                    foreach (System.Text.Json.JsonProperty property in parameter2.EnumerateObject())
                    {
                        string name = property.Name.ToLower();
                        string value = WASMJsonConvert.parseJsonElementToString(property.Value);
                        switch (name)
                        {
                            case "color":
                                mycolor = DCValueConvert.HtmlToColor(value, DCSystem_Drawing.Color.Black);
                                break;
                        }
                    }
                    string newParameter = XMLSerializeHelper.ColorToString(mycolor); ;
                    return WASMJsonConvert.parseBoolean(this._Control.ExecuteCommand("UnderlineStyle", false, newParameter), false);
                }

            }
            return false;
        }

        private void command_fontsizepro(string value)
        {
            DCSelection selection = this._Control.Document.Selection;
            if (selection != null && selection.Length != 0)
            {
                if (selection.ContentElements.Count > 0)
                {
                    for (int i = 0; i < selection.ContentElements.Count; i++)
                    {
                        DomElement element = selection.ContentElements[i];
                        if (element is DomFieldBorderElement)
                        {
                            DomFieldBorderElement inputBorder = element as DomFieldBorderElement;
                            if (inputBorder != null)
                            {
                                DomInputFieldElement input = inputBorder.OwnerField as DomInputFieldElement;
                                if (input != null)
                                {
                                    InnerFontSize(value, input);
                                }
                            }
                        }
                        //InnerFontSize(value, element);
                    }
                }
            }
        }

        private static void InnerFontSize(string value, DomElement element)
        {
            if (element != null && element is DomInputFieldElement)
            {
                DomInputFieldElement input = element as DomInputFieldElement;
                if (input != null && input.UserEditable == false)
                {
                    if (input.Style == null)
                    {
                        input.Style = new DocumentContentStyle();
                    }
                    input.Style.FontSize = FontSizeInfo.GetFontSize(value, input.Style.FontSize);
                    if (input.Elements != null && input.Elements.Count > 0)
                    {
                        for (int j = 0; j < input.Elements.Count; j++)
                        {
                            DomElement tagElement = input.Elements[j];

                            if (tagElement.Style == null)
                            {
                                tagElement.Style = new DocumentContentStyle();
                            }
                            tagElement.Style.FontSize = FontSizeInfo.GetFontSize(value, input.Style.FontSize);
                            InnerFontSize(value, tagElement);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 删除指定的表格行
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        private bool command_deletetablerow(object parameter)
        {
            if (parameter != null)
            {
                JsonElement parameter2 = (JsonElement)parameter;
                if (parameter2.ValueKind == JsonValueKind.Object)
                {
                    string tableID = "";
                    int rowIndex = -1;

                    foreach (System.Text.Json.JsonProperty property in parameter2.EnumerateObject())
                    {
                        string name = property.Name.ToLower();
                        string value = WASMJsonConvert.parseJsonElementToString(property.Value);
                        int i = int.MinValue;
                        switch (name)
                        {
                            case "tableid":
                                tableID = value;
                                break;
                            case "rowindex":
                                if (int.TryParse(value, out i) == true)
                                {
                                    rowIndex = i;
                                }
                                break;
                        }
                    }
                    if (tableID != "" && rowIndex != -1)
                    {
                        DomTableElement table = this._Control.GetTableElementById(tableID);
                        if (table != null)
                        {
                            TableCommandArgs args = new TableCommandArgs();
                            args.TableID = tableID;
                            args.RowIndex = rowIndex;
                            this._Control.ExecuteCommand("Table_DeleteRow", false, args);
                            table.EditorRefreshView();
                            return true;

                        }
                    }
                }
            }
            else
            {
                this._Control.ExecuteCommand(StandardCommandNames.Table_DeleteRow, false, null);
                return true;
            }
            return false;
        }

        /// <summary>
        /// 删除指定的表格列
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        private bool command_deletetablecolumn(object parameter)
        {
            if (parameter != null)
            {
                JsonElement parameter2 = (JsonElement)parameter;
                if (parameter2.ValueKind == JsonValueKind.Object)
                {
                    string tableID = "";
                    int columnIndex = -1;

                    foreach (System.Text.Json.JsonProperty property in parameter2.EnumerateObject())
                    {
                        string name = property.Name.ToLower();
                        string value = WASMJsonConvert.parseJsonElementToString(property.Value);
                        int i = int.MinValue;
                        switch (name)
                        {
                            case "tableid":
                                tableID = value;
                                break;
                            case "columnindex":
                                if (int.TryParse(value, out i) == true)
                                {
                                    columnIndex = i;
                                }
                                break;
                        }
                    }
                    if (tableID != "" && columnIndex != -1)
                    {
                        DomTableElement table = this._Control.GetTableElementById(tableID);
                        if (table != null)
                        {
                            TableCommandArgs args = new TableCommandArgs();
                            args.TableID = tableID;
                            args.ColIndex = columnIndex;
                            this._Control.ExecuteCommand("Table_DeleteColumn", false, args);
                            table.EditorRefreshView();
                            return true;

                        }
                    }
                }
                else if (parameter2.ValueKind == JsonValueKind.True || parameter2.ValueKind == JsonValueKind.False)
                {
                    this._Control.ExecuteCommand(StandardCommandNames.Table_DeleteColumn, false, null);
                    return true;
                }
            }
            else
            {
                this._Control.ExecuteCommand(StandardCommandNames.Table_DeleteColumn, false, null);
                return true;
            }
            return false;
        }

        /// <summary>
        /// 删除指定ID的表格
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        private bool command_deletetable(object parameter)
        {
            string tableid = "";
            if (parameter is string)
            {
                tableid = (string)parameter;
            }
            else
            {
                if (parameter == null)
                {

                }
                else
                {
                    JsonElement parameter2 = (JsonElement)parameter;
                    if (parameter2.ValueKind == JsonValueKind.String)
                    {
                        tableid = parameter2.GetString();
                    }
                }
            }
            if (tableid == "" || tableid.Length == 0)
            {
                var result = this._Control.ExecuteCommand("Table_DeleteTable", false, null);
                if (result != null)
                {
                    return true;
                }
            }
            else
            {
                DomTableElement table = this._Control.GetTableElementById(tableid);
                if (table != null)
                {
                    return table.EditorDelete(true);
                }
            }
            return false;
        }

        /// <summary>
        /// 插入指定行
        /// </summary>
        /// <param name="parameter"></param>
        /// <param name="isDown">是否是在下面插入行</param>
        /// <returns></returns>
        private bool command_tableinsertrowdown(object parameter, bool isDown)
        {
            if (parameter != null)
            {
                JsonElement parameter2 = (JsonElement)parameter;
                if (parameter2.ValueKind == JsonValueKind.Object)
                {
                    string tableID = "";
                    int rowIndex = -1;
                    int rowCount = -1;
                    var bolFastMode = false;
                    var bolLogUndo = true;
                    foreach (System.Text.Json.JsonProperty property in parameter2.EnumerateObject())
                    {
                        string name = property.Name.ToLower();
                        string value = WASMJsonConvert.parseJsonElementToString(property.Value);
                        int i = int.MinValue;
                        switch (name)
                        {
                            case "logundo": bolLogUndo = property.ConvertToBoolean(true); break;
                            case "fastmode":
                                {
                                    bolFastMode = property.ConvertToBoolean(false);
                                }
                                break;
                            case "tableid":
                                tableID = value;
                                break;
                            case "rowindex":
                                if (int.TryParse(value, out i) == true)
                                {
                                    rowIndex = i;
                                }
                                break;
                            case "rowcount":
                                if (int.TryParse(value, out i) == true)
                                {
                                    rowCount = i;
                                }
                                break;
                        }
                    }
                    if (tableID != null && tableID.Length > 0)
                    {
                        DomTableElement table = this._Control.GetTableElementById(tableID);
                        if (table != null)
                        {
                            ////以下接口使用的CS的插入行的功能，但是在表单模式下失效
                            //TableCommandArgs args = new TableCommandArgs();
                            //args.TableElement = table;

                            //if (rowCount == -1)
                            //{//参数不包含指定的行数
                            //    args.RowsCount = 1;//默认只增加一行
                            //}
                            //else
                            //{
                            //    args.RowsCount = rowCount;
                            //}
                            //if (rowIndex == -1)
                            //{//参数不包含指定的位置
                            //    args.RowIndex = table.Rows.Count - 1;//最后一行
                            //}
                            //else
                            //{
                            //    args.RowIndex = rowIndex;
                            //}
                            //this._Control.ExecuteCommand(StandardCommandNames.Table_InsertRowDown, false, args);
                            //table.EditorRefreshView();


                            //以下解决在表单模式下增加行的问题
                            if (rowCount == -1)
                            {//参数不包含指定的行数
                                rowCount = 1;//默认只增加一行
                            }

                            if (rowIndex == -1)
                            {//参数不包含指定的位置
                                rowIndex = table.Rows.Count - 1;//最后一行
                            }
                            if (rowIndex > table.Rows.Count - 1)
                            {
                                rowIndex = table.Rows.Count - 1;//最后一行
                            }


                            var currentRow = (DomTableRowElement)table.Rows[rowIndex];
                            var tempRow = currentRow.EditorClone(); // 模板表格行对象
                            //tempRow.FixDomState();
                            //XTextElement newElement = currentRow.Clone(true);//这里好像对表格行的复制模式失效了，始终都是全部复制
                            var newRows = new DomElementList(rowCount);
                            for (int i = 0; i < rowCount; i++)
                            {
                                newRows.SuperFastAdd(tempRow.Clone(true));
                                //var newRow =  currentRow.EditorClone();
                                //newRow.FixDomState();
                                //newRows.FastAdd2(newRow);
                            }
                            if (bolFastMode)
                            {
                                // 快速模式
                                var rows = table.Rows;
                                rows.EnsureCapacity(rows.Count + newRows.Count);
                                if (isDown)
                                {
                                    rows.InsertRangeRaw(rowIndex + 1, newRows);
                                }
                                else
                                {
                                    rows.InsertRangeRaw(rowIndex, newRows);
                                }
                                table.UpdateContentVersion();
                                table.UpdateRowIndexColIndex();
                                table.InvalidateHighlightInfo();
                            }
                            else
                            {
                                if (isDown)
                                {
                                    table.EditorInsertRows2(rowIndex + 1, newRows, bolLogUndo, null, false);
                                }
                                else
                                {
                                    table.EditorInsertRows2(rowIndex, newRows, bolLogUndo, null, false);
                                }
                                table.EditorRefreshView(); //此处发现需要刷新才有效 20250428
                            }
                            //table.EditorRefreshView(); //yyf 2025-2-26 无需重复操作
                            return true;

                        }
                    }
                }
            }
            else
            {
                if (isDown)
                {
                    this._Control.ExecuteCommand(StandardCommandNames.Table_InsertRowDown, false, null);
                }
                else
                {
                    this._Control.ExecuteCommand(StandardCommandNames.Table_InsertRowUp, false, null);
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// 合并单元格
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        private bool command_tablemergecell(object parameter)
        {
            if (parameter != null)
            {
                JsonElement parameter2 = (JsonElement)parameter;
                if (parameter2.ValueKind == JsonValueKind.Object)
                {
                    string tableID = "";
                    int columnIndex = -1;
                    int rowIndex = -1;
                    int rowSpan = -1;
                    int columnSpan = -1;

                    foreach (System.Text.Json.JsonProperty property in parameter2.EnumerateObject())
                    {
                        string name = property.Name.ToLower();
                        string value = WASMJsonConvert.parseJsonElementToString(property.Value);
                        int i = int.MinValue;
                        switch (name)
                        {
                            case "tableid":
                                tableID = value;
                                break;
                            case "columnindex":
                                if (int.TryParse(value, out i) == true)
                                {
                                    columnIndex = i;
                                }
                                break;
                            case "rowindex":
                                if (int.TryParse(value, out i) == true)
                                {
                                    rowIndex = i;
                                }
                                break;
                            case "rowspan":
                                if (int.TryParse(value, out i) == true)
                                {
                                    rowSpan = i;
                                }
                                break;
                            case "columnspan":
                                if (int.TryParse(value, out i) == true)
                                {
                                    columnSpan = i;
                                }
                                break;
                        }
                    }
                    if (tableID != "" && columnIndex > -1 && rowIndex > -1 && rowSpan > 0 && columnSpan > 0)
                    {
                        DomTableElement table = this._Control.GetTableElementById(tableID);
                        DomTableCellElement cell = null;
                        if (table != null)
                        {
                            cell = table.GetCell(rowIndex, columnIndex, false);
                        }
                        if (cell != null)
                        {
                            MegeCellCommandParameter megeCellCommand = new MegeCellCommandParameter();
                            megeCellCommand.Cell = cell;
                            megeCellCommand.NewRowSpan = rowSpan;
                            megeCellCommand.NewColSpan = columnSpan;
                            this._Control.ExecuteCommand(StandardCommandNames.Table_MergeCell, false, megeCellCommand);
                            cell.EditorRefreshView();
                            this._Control.Document.Modified = true;
                            return true;
                        }
                    }
                }
            }

            return false;
        }


        /// <summary>
        /// 兼容四代的输入域下划线设置
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        private bool command_inputfieldunderline(object parameter)
        {
            DomInputFieldElement input = this._Control.Document.CurrentInputField;
            if (input != null && input.Style != null)
            {
                JsonElement parameter2 = (JsonElement)parameter;
                if (parameter2.ValueKind == JsonValueKind.Object)
                {
                    bool SetUnderline = false;
                    bool isAddStyle = false;//是否是按照四代的参数

                    bool IsAddLine = true;
                    string InputFieldUnderLineColor = "";
                    float InputFieldUnderLineWidth = 0;
                    string InputFieldUnderLineStyle = "Solid";

                    foreach (System.Text.Json.JsonProperty property in parameter2.EnumerateObject())
                    {
                        string name = property.Name.ToLower();
                        string value = WASMJsonConvert.parseJsonElementToString(property.Value);
                        float f = float.NaN;
                        switch (name)
                        {
                            case "setunderline":
                                isAddStyle = true;//按照四代的参数
                                SetUnderline = WASMUtils.ConvertToBoolean(property, true);// WASMJsonConvert.parseBoolean(value, true);
                                break;
                            case "isaddline":
                                isAddStyle = false;//按照CS的参数
                                IsAddLine = WASMUtils.ConvertToBoolean(property, true);// WASMJsonConvert.parseBoolean(value, true);
                                break;
                            case "inputfieldunderlinecolor":
                                isAddStyle = false;//按照CS的参数
                                InputFieldUnderLineColor = value;
                                break;
                            case "inputfieldunderlinewidth":
                                isAddStyle = false;//按照CS的参数
                                if (float.TryParse(value, out f) == true)
                                {
                                    InputFieldUnderLineWidth = f;
                                }
                                break;
                            case "inputfieldunderlinestyle":
                                isAddStyle = false;//按照CS的参数
                                InputFieldUnderLineStyle = value;
                                break;
                        }
                    }
                    if (isAddStyle)
                    {//按照四代的接口设置
                        if (SetUnderline)
                        {
                            input.Style.BorderTop = false;
                            input.Style.BorderLeft = false;
                            input.Style.BorderRight = false;
                            input.Style.BorderBottom = true;
                            //input.Style.BorderBottomColor = parameter.InputFieldUnderLineColor;
                            input.Style.BorderStyle = DashStyle.Solid;
                            input.Style.BorderWidth = 1;
                        }
                        else
                        {
                            input.Style.BorderTop = false;
                            input.Style.BorderLeft = false;
                            input.Style.BorderRight = false;
                            input.Style.BorderBottom = false;
                        }
                        input.EditorRefreshView();
                        return true;
                    }
                    else
                    {//按照CS的接口设置
                        InputFieldUnderLineCommandParameter underLineCommandParameter = new InputFieldUnderLineCommandParameter();
                        underLineCommandParameter.IsAddLine = IsAddLine;
                        underLineCommandParameter.InputFieldUnderLineColor = DCSystem_Drawing.ColorTranslator.FromHtml(InputFieldUnderLineColor);
                        underLineCommandParameter.InputFieldUnderLineWidth = InputFieldUnderLineWidth;
                        underLineCommandParameter.InputFieldUnderLineStyle = WASMJsonConvert.parseEnumValue<DashStyle>(InputFieldUnderLineStyle, DashStyle.Solid); ; ;
                        return Convert.ToBoolean(this._Control.ExecuteCommand("inputfieldunderline", false, underLineCommandParameter));
                    }
                }
            }
            return false;
        }
        private bool command_ToUppercase(bool toupper)
        {

            //wyc20230627重写逻辑
            if (this._Control.Selection == null || this._Control.Selection.Length == 0)
            {
                return false;
            }
            bool result = false;
            DCSelection selection = this._Control.Selection;
            //XTextElementList newlist = new XTextElementList();
            string lowerstrs = "abcdefghijklmnopqrstuvwxyz";
            string upperstrs = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            foreach (var element in selection.ContentElements)
            {
                if (element is DomCharElement)
                {
                    DomCharElement charele = element as DomCharElement;
                    char c = charele.GetCharValue();
                    char targetchar = c;
                    int index1 = lowerstrs.IndexOf(c);
                    int index2 = upperstrs.IndexOf(c);
                    if (toupper == true && index1 >= 0)
                    {
                        targetchar = upperstrs[index1];
                    }
                    else if (toupper == false && index2 >= 0)
                    {
                        targetchar = lowerstrs[index2];
                    }
                    if (targetchar != c)
                    {
                        charele.CharValue = targetchar;
                        result = true;
                    }
                }
            }
            if (result == true)
            {
                selection.DocumentContent.EditorRefreshView();
            }
            //ReplaceElementsArgs args = new ReplaceElementsArgs()
            //DCSoft.Writer.Commands.ContentSearchReplacer icsr =  new ContentSearchReplacer();
            //icsr.Document = this._Control.Document;

            //int result = 0;

            //string lowerstrs = "abcdefghijklmnopqrstuvwxyz";
            //string upperstrs = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            //SearchReplaceCommandArgs args = new SearchReplaceCommandArgs();
            //string searchgroup = toupper ? lowerstrs : upperstrs;
            //string targetgroup = toupper ? upperstrs : lowerstrs;
            //for (int i = 0; i < searchgroup.Length; i++)
            //{
            //    string ss = searchgroup[i].ToString();
            //    string ts = targetgroup[i].ToString();
            //    args.IgnoreCase = false;
            //    args.SearchString = ss;
            //    args.ReplaceString = ts;
            //    args.EnableReplaceString = true;

            //    result = result + icsr.ReplaceAll(args);

            //}

            return result;
        }
        private bool command_fontborder(object parameter)
        {
            bool setborder = WASMJsonConvert.parseBoolean(parameter, true);

            DCSoft.Writer.Commands.BorderBackgroundCommandParameter bbcp = new BorderBackgroundCommandParameter();
            bbcp.ApplyRange = StyleApplyRanges.Text;
            bbcp.BorderStyle = DashStyle.Solid;
            bbcp.BorderWidth = 1;
            bbcp.BorderSettingsStyle = BorderSettingsStyle.Rectangle;
            bbcp.BottomBorder = bbcp.TopBorder = bbcp.LeftBorder = bbcp.RightBorder = setborder;
            bbcp.BorderBottomColor = bbcp.BorderTopColor = bbcp.BorderLeftColor = bbcp.BorderRightColor = Color.Black;
            this._Control.ExecuteCommand(StandardCommandNames.BorderBackgroundFormat, false, bbcp);
            return true;
        }
        private bool command_insertparagraphbeforetable()
        {
            if (this._Control == null || this._Control.CurrentTable == null)
            {
                return false;
            }
            DomTableElement table = this._Control.CurrentTable;
            if (table != null && table.Parent != null)
            {
                table.Parent.Elements.InsertBefore(
                    table,
                    new DomParagraphFlagElement());
                table.Parent.EditorRefreshView();
                this._Control.Document.Modified = true;
                return true;
            }
            return false;
        }
        private JsonArray command_documentvaluevalidate(bool showUI, object parameter)
        {

            if (parameter != null)
            {//支持在指定的容器中校验。参数可以为id，或handle对象
                JsonElement parameter2 = (JsonElement)parameter;

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
                        ValueValidateResultList list = DefaultDOMDataProvider.ValueValidateInContainerElement(containerElement);
                        return InnerBuildValueValidateResultList(list);
                    }
                }
                return null;
            }
            else
            {
                ValueValidateResultList list = this._Control.DocumentValueValidate();
                return InnerBuildValueValidateResultList(list);
            }
        }

        private JsonArray InnerBuildValueValidateResultList(ValueValidateResultList list)
        {
            if (list != null && list.Count > 0)
            {
                JsonArray arr = new JsonArray();
                foreach (ValueValidateResult result in list)
                {
                    JsonObject obj = new JsonObject();
                    obj.Add("ElementID", result.ElementID);
                    obj.Add("Level", result.Level.ToString());
                    obj.Add("Message", result.Message);
                    obj.Add("Title", result.Title);
                    obj.Add("Type", result.Type.ToString());
                    arr.Add(obj);
                }
                return arr;
            }
            else
            {
                return null;
            }
        }

        private bool command_deletefield(bool showUI, object? parameter)
        {
            DomInputFieldElement input = null;
            if (parameter == null && this._Control.CurrentInputField == null)
            {
                return false;
            }
            if (parameter is string)
            {
                input = this._Control.GetInputFieldElementById((string)parameter);
            }
            else if (parameter is DotNetObjectReference<DomElement>)
            {
                DotNetObjectReference<DomElement> ele = parameter as DotNetObjectReference<DomElement>;
                input = ele.Value as DomInputFieldElement;
                if (ele != null)
                {
                    ele.Dispose();
                }
            }
            else
            {
                input = this._Control.CurrentInputField;
            }
            object o = this._Control.ExecuteCommand(StandardCommandNames.DeleteField, showUI, input);
            return o != null;
        }
        private bool command_insertstring(string? commandName, bool showUI, object parameter)
        {
            if (parameter == null)
            {
                return false;
            }
            if (parameter is JsonElement)
            {
                JsonElement para = (JsonElement)parameter;
                if (para.ValueKind == JsonValueKind.String)
                {
                    string str = para.GetString();

                    //wyc20230711:试图插入内容后选中内容增加高亮辅助提示,若当前有选区，先清除选区再插
                    int startindex = this._Control.Document.Content.SelectionStartIndex;
                    if (this._Control.Document.Content.SelectionLength != 0)
                    {
                        this._Control.Document.Content.SetSelection(startindex, 0);
                    }
                    DomElementList o = this._Control.ExecuteCommand(StandardCommandNames.InsertString, showUI, str) as DomElementList;
                    int endindex = this._Control.Document.Content.SelectionStartIndex;
                    if (o != null && o.Count > 0)
                    {
                        this._Control.Document.Content.SetSelection(endindex, (o.Count * -1));
                    }
                    /////////////////////////////////////////////////////

                    return o != null;
                }

            }
            return false;
        }
        private bool command_color(string? commandName, bool showUI, object parameter)
        {
            if (parameter is JsonElement)
            {
                JsonElement para = (JsonElement)parameter;
                if (para.ValueKind == JsonValueKind.String)
                {
                    string colorstring = para.GetString();
                    DCSystem_Drawing.Color c = DCValueConvert.HtmlToColor(colorstring, DCSystem_Drawing.Color.Black);
                    return Convert.ToBoolean(this._Control.ExecuteCommand(commandName, showUI, c));
                }

            }
            return false;
        }

        private bool command_colorpow(string commandName, bool showUI, object parameter)
        {
            if (parameter is JsonElement)
            {
                JsonElement para = (JsonElement)parameter;
                if (para.ValueKind == JsonValueKind.String)
                {
                    string colorstring = para.GetString();
                    Color c = DCValueConvert.HtmlToColor(colorstring, Color.Black);
                    bool res1 = Convert.ToBoolean(this._Control.ExecuteCommand("color", false, c));
                    return true;
                }
            }
            return false;
        }



        private bool command_BoldItalicUnderlineStrikethroughSubscriptSupscript(string? commandName, bool showUI, object parameter)
        {
            if (commandName == "strikethrough")
            {
                commandName = "strikeout";
            }
            object o = null;
            if (parameter is JsonElement)
            {
                JsonElement para = (JsonElement)parameter;
                if (para.ValueKind == JsonValueKind.Null || para.ValueKind == JsonValueKind.Undefined)
                {
                    o = null;
                }
                else if (para.ValueKind == JsonValueKind.True ||
                    (para.ValueKind == JsonValueKind.String && para.GetString().ToLower() == "true"))
                {
                    o = true;
                }
                else if (para.ValueKind == JsonValueKind.False ||
                    (para.ValueKind == JsonValueKind.String && para.GetString().ToLower() == "false"))
                {
                    o = false;
                }
            }

            return Convert.ToBoolean(this._Control.ExecuteCommand(commandName, showUI, o));
        }
        private bool command_insertxml(bool showUI, object parameter)
        {
            if (parameter != null && parameter is JsonElement)
            {
                JsonElement para = (JsonElement)parameter;
                if (para.ValueKind != JsonValueKind.String)
                {
                    return false;
                }
                string str = para.GetString();
                //wyc20230711:试图插入内容后选中内容增加高亮辅助提示,若当前有选区，先清除选区再插
                int startindex = this._Control.Document.Content.SelectionStartIndex;
                if (this._Control.Document.Content.SelectionLength != 0)
                {
                    this._Control.Document.Content.SetSelection(startindex, 0);
                }
                object o = this._Control.ExecuteCommand(StandardCommandNames.InsertXML, showUI, str);
                // 修复插入xml存在逻辑删除内容的问题【DUWRITER5_0-4335】
                var currentEl = this._Control.Document.CurrentContentElement;
                if (currentEl != null && currentEl.Parent != null)
                {
                    currentEl.Parent.EditorRefreshView();
                }
                int endindex = this._Control.Document.Content.SelectionStartIndex;
                this._Control.Document.Content.SetSelection(endindex, startindex - endindex);
                /////////////////////////////////////////////////////

                return o != null;
            }
            return false;
        }
        /// <summary>
        /// 拆分单元格
        /// ctl.DCExecuteCommand('table_splitcellext', true, '3,5');
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        private bool command_tablesplitcellext(object parameter)
        {
            DomTableCellElement cell = this._Control.CurrentTableCell;
            if (cell != null)
            {
                if (parameter != null)
                {
                    JsonElement parameter2 = (JsonElement)parameter;
                    if (parameter2.ValueKind == JsonValueKind.String)
                    {
                        string value = parameter2.GetString();
                        if (value != null && value != "" && value.Contains(",") && value.Length > 2)
                        {
                            var value2 = value.Split(',');
                            if (value2 != null && value2.Length == 2)
                            {
                                string v1 = value2[0];
                                string v2 = value2[1];

                                int iRowCount = 0;
                                int iColumnCount = 0;
                                bool result1 = int.TryParse(v1, out iRowCount);
                                bool result2 = int.TryParse(v2, out iColumnCount);
                                if (result1 && result2 && iRowCount > 0 && iColumnCount > 0)
                                {
                                    SplitCellExtCommandParameter parameterForSplitCellExt = new SplitCellExtCommandParameter();
                                    parameterForSplitCellExt.CellElement = cell;
                                    parameterForSplitCellExt.NewRowSpan = iRowCount;
                                    parameterForSplitCellExt.NewColSpan = iColumnCount;
                                    this._Control.ExecuteCommand("table_splitcellext", false, parameterForSplitCellExt);
                                    this._Control.Document.Modified = true;
                                    return true;
                                }
                            }
                        }
                    }
                }
            }
            return false;
        }

        //wyc20230710:新增allline命令兼容四代
        private bool command_alllineheight(object parameter)
        {
            if ((parameter is JsonElement) == false)
            {
                return false;
            }
            JsonElement ele = (JsonElement)parameter;
            float linespacing = 0;
            LineSpacingStyle lsstyle = LineSpacingStyle.SpaceSingle;
            string styleValue = WASMJsonConvert.parseJsonElementToString(ele);
            if (styleValue == null)
            {
                return false;
            }
            styleValue = styleValue.Trim().ToLower();
            if (styleValue.EndsWith("%"))
            {
                // 标准行高的倍数
                string sh = styleValue.Substring(0, styleValue.Length - 1);
                int p = 0;
                if (Int32.TryParse(sh, out p))
                {
                    if (p == 100)
                    {
                        linespacing = 0;
                        lsstyle = LineSpacingStyle.SpaceSingle;
                    }
                    else if (p == 150)
                    {
                        lsstyle = LineSpacingStyle.Space1pt5;
                    }
                    else if (p == 200)
                    {
                        lsstyle = LineSpacingStyle.SpaceDouble;
                    }
                    else
                    {
                        lsstyle = LineSpacingStyle.SpaceMultiple;
                        linespacing = p / 100.0f;
                    }
                }
            }
            // 针对line-height: 2em 
            else if (styleValue.EndsWith("em"))
            {
                // 标准行高的倍数
                string sh = styleValue.Substring(0, styleValue.Length - 2);
                float p = 0;
                if (float.TryParse(sh, out p))
                {
                    if (p == 1)
                    {
                        linespacing = 0;
                        lsstyle = LineSpacingStyle.SpaceSingle;
                    }
                    else if (p == 1.5)
                    {
                        lsstyle = LineSpacingStyle.Space1pt5;
                    }
                    else if (p == 2)
                    {
                        lsstyle = LineSpacingStyle.SpaceDouble;
                    }
                    else
                    {
                        lsstyle = LineSpacingStyle.SpaceMultiple;
                        linespacing = p / 1.0f;
                    }
                }
            }
            else //针对浮点数直接赋值
            {
                float p = 0;
                bool isMultiSpacing = styleValue.Contains("px") == false;
                if (float.TryParse(styleValue.Replace("px", ""), out p))
                {
                    if (p == 1)
                    {
                        linespacing = 0;
                        lsstyle = LineSpacingStyle.SpaceSingle;
                    }
                    else if (p == 1.5)
                    {
                        lsstyle = LineSpacingStyle.Space1pt5;
                    }
                    else if (p == 2)
                    {
                        lsstyle = LineSpacingStyle.SpaceDouble;
                    }
                    else
                    {
                        if (isMultiSpacing)
                        {
                            lsstyle = LineSpacingStyle.SpaceMultiple;
                            linespacing = p;
                        }
                        else
                        {
                            lsstyle = LineSpacingStyle.SpaceSpecify;
                            linespacing = GraphicsUnitConvert.Convert(
                                p,
                                GraphicsUnit.Pixel,
                                GraphicsUnit.Document);
                        }
                    }
                }
            }

            DomElementList ps = this._Control.Document.Body.GetElementsBySpecifyType<DomParagraphFlagElement>();
            foreach (DomParagraphFlagElement flag in ps)
            {
                flag.Style.LineSpacing = linespacing;
                flag.Style.LineSpacingStyle = lsstyle;
            }
            this._Control.RefreshDocument();
            return true;
        }

        /// <summary>
        /// 兼容四代段前距、断后距
        /// 例如：ctl.DCExecuteCommand('rowspacing', true, '10,top'); 
        /// 例如：ctl.DCExecuteCommand('rowspacing', true, '10,bottom'); 
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        private bool command_Rowspacing(object parameter)
        {
            if (parameter != null)
            {
                JsonElement parameter2 = (JsonElement)parameter;
                if (parameter2.ValueKind == JsonValueKind.String)
                {//字符串
                    string value = parameter2.GetString();
                    if (value != null && value != "" && value.Contains(",") && value.Length > 2)
                    {
                        var value2 = value.Split(',');
                        if (value2 != null && value2.Length == 2)
                        {
                            string v1 = value2[0];
                            string v2 = value2[1].ToLower();

                            float f = 0f;
                            bool result = float.TryParse(v1, out f);
                            if (result)
                            {
                                if (v2 == "top")
                                {
                                    ParagraphFormatCommandParameter commandParameter = GetCurrentParagraphFormatCommandParameter();//当前段落样式参数
                                    commandParameter.SpacingBefore = f;
                                    this._Control.ExecuteCommand("ParagraphFormat", false, commandParameter);
                                    return true;
                                }
                                else if (v2 == "bottom")
                                {
                                    ParagraphFormatCommandParameter commandParameter = GetCurrentParagraphFormatCommandParameter();//当前段落样式参数
                                    commandParameter.SpacingAfter = f;
                                    this._Control.ExecuteCommand("ParagraphFormat", false, commandParameter);
                                    return true;
                                }
                            }
                        }
                    }
                }
            }
            return false;
        }
        /// <summary>
        /// 兼容四代行间距
        /// 例如：ctl.DCExecuteCommand('lineheight', true, 2); 设置2倍
        /// 例如：ctl.DCExecuteCommand('lineheight', true, “20px”);设置具体的值
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        private bool command_LineHeight(object parameter)
        {
            if (parameter != null)
            {
                //声明对象
                DocumentContentStyle ns = this._Control.Document.CreateDocumentContentStyle();
                ns.DisableDefaultValue = true;

                JsonElement parameter2 = (JsonElement)parameter;
                if (parameter2.ValueKind == JsonValueKind.String)
                {//字符串
                    string value = parameter2.GetString();
                    if (value != null && value != "")
                    {
                        if (value.Contains("px") && value.Length > 2)
                        {
                            value = value.Substring(0, value.Length - 2);
                            float f = 0f;
                            bool result = float.TryParse(value, out f);
                            if (result)
                            {//数字
                                ns.LineSpacingStyle = LineSpacingStyle.SpaceSpecify;
                                ns.LineSpacing = f;//具体值
                                //ParagraphFormatCommandParameter commandParameter = GetCurrentParagraphFormatCommandParameter();//当前段落样式参数
                                //commandParameter.LineSpacingStyle = LineSpacingStyle.SpaceSpecify;
                                //commandParameter.LineSpacing = f;//具体值
                                //this._Control.ExecuteCommand("ParagraphFormat", false, commandParameter);
                                //return true;
                            }
                        }
                        else
                        {
                            float fvalue = 0f;
                            if (float.TryParse(value, out fvalue))
                            {
                                ns.LineSpacingStyle = LineSpacingStyle.SpaceMultiple;
                                ns.LineSpacing = fvalue;//具体值
                                //ParagraphFormatCommandParameter commandParameter = GetCurrentParagraphFormatCommandParameter();//当前段落样式参数
                                //commandParameter.LineSpacingStyle = LineSpacingStyle.SpaceMultiple;
                                //commandParameter.LineSpacing = fvalue;//倍数
                                //this._Control.ExecuteCommand("ParagraphFormat", false, commandParameter);
                                //return true;
                            }
                        }
                    }
                }
                else if (parameter2.ValueKind == JsonValueKind.Number)
                {//数字
                    float value = (float)parameter2.GetDecimal();

                    ns.LineSpacingStyle = LineSpacingStyle.SpaceMultiple;
                    ns.LineSpacing = value;//倍数                   
                    //ParagraphFormatCommandParameter commandParameter = GetCurrentParagraphFormatCommandParameter();//当前段落样式参数
                    //commandParameter.LineSpacingStyle = LineSpacingStyle.SpaceMultiple;
                    //commandParameter.LineSpacing = value;//倍数
                    //this._Control.ExecuteCommand("ParagraphFormat", false, commandParameter);
                    //return true;
                }

                this._Control.Document.BeginLogUndo();
                DomElementList list = this._Control.Document.Selection.SetParagraphStyle(ns);
                this._Control.Document.EndLogUndo();
                //args.Document.CurrentStyle.Underline = v;
                this._Control.Document.OnSelectionChanged();
                this._Control.Document.OnDocumentContentChanged();
                if (list != null && list.Count > 0)
                {
                    return true;
                }
                return false;
            }
            return false;
        }
        /// <summary>
        /// 兼容四代有序段落列表
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        private bool command_InsertOrderedList(object parameter)
        {
            ParagraphFormatCommandParameter commandParameter = GetCurrentParagraphFormatCommandParameter();//当前段落样式参数
            if (commandParameter.ListStyle == ParagraphListStyle.None)
            {
                commandParameter.ListStyle = ParagraphListStyle.ListNumberStyle;//默认列表样式
            }
            else
            {
                commandParameter.ListStyle = ParagraphListStyle.None;//默认列表样式
            }
            if (parameter != null)
            {
                JsonElement parameter2 = (JsonElement)parameter;
                foreach (System.Text.Json.JsonProperty property in parameter2.EnumerateObject())
                {
                    string name = property.Name.ToLower();
                    //string value =WASMJsonConvert.parseJsonElementToString(property.Value);
                    //int i = int.MinValue;
                    //float d = 0f;
                    switch (name)
                    {
                        case "liststyle":
                            commandParameter.ListStyle = WASMUtils.ConvertToEnum<ParagraphListStyle>(property, ParagraphListStyle.None);// WASMJsonConvert.parseEnumValue<ParagraphListStyle>(value, ParagraphListStyle.None); ;
                            break;
                        default:
                            break;
                    }
                }
            }
            this._Control.ExecuteCommand("ParagraphFormat", false, commandParameter);
            return true;
        }
        /// <summary>
        /// 兼容四代无序段落列表
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        private bool command_InsertUnOrderedList(object parameter)
        {
            ParagraphFormatCommandParameter commandParameter = GetCurrentParagraphFormatCommandParameter();//当前段落样式参数
            if (commandParameter.ListStyle == ParagraphListStyle.None)
            {
                commandParameter.ListStyle = ParagraphListStyle.BulletedList;//默认列表样式
            }
            else
            {
                commandParameter.ListStyle = ParagraphListStyle.None;//默认列表样式
            }
            if (parameter != null)
            {
                JsonElement parameter2 = (JsonElement)parameter;
                foreach (System.Text.Json.JsonProperty property in parameter2.EnumerateObject())
                {
                    string name = property.Name.ToLower();
                    //string value =WASMJsonConvert.parseJsonElementToString(property.Value);
                    //int i = int.MinValue;
                    //float d = 0f;
                    switch (name)
                    {
                        case "liststyle":
                            commandParameter.ListStyle = WASMUtils.ConvertToEnum<ParagraphListStyle>(property, ParagraphListStyle.None);// WASMJsonConvert.parseEnumValue<ParagraphListStyle>(value, ParagraphListStyle.None); ;
                            break;
                        default:
                            break;
                    }
                }
            }
            this._Control.ExecuteCommand("ParagraphFormat", false, commandParameter);
            return true;
        }

        /// <summary>
        /// 兼容四代设置悬挂缩进
        /// </summary>
        /// <param name="showUI"></param>
        /// <param name="parameter"></param>
        /// <returns></returns>
        private bool command_HangIndent(object parameter)
        {
            ParagraphFormatCommandParameter commandParameter = GetCurrentParagraphFormatCommandParameter();//当前段落样式参数

            //commandParameter.FirstLineIndent = -87;//参考CS的默认值
            //commandParameter.LeftIndent = 236;
            if (parameter != null)
            {
                JsonElement parameter2 = (JsonElement)parameter;
                foreach (System.Text.Json.JsonProperty property in parameter2.EnumerateObject())
                {
                    string name = property.Name.ToLower();
                    string value = WASMJsonConvert.parseJsonElementToString(property.Value);
                    float f = float.NaN;
                    switch (name)
                    {
                        case "leftindent":
                            if (float.TryParse(value, out f) == true)
                            {
                                commandParameter.LeftIndent = f;
                            }
                            break;
                        case "firstlineindent":
                            if (float.TryParse(value, out f) == true)
                            {
                                commandParameter.FirstLineIndent = f;
                            }
                            break;
                        default:
                            break;
                    }
                }
            }
            //wyc20230718:如果传入的参数为空则取消段落的缩进样式
            else
            {
                commandParameter.LeftIndent = 0;
                commandParameter.FirstLineIndent = 0;
            }
            /////////////////////////////////////////////////////

            this._Control.ExecuteCommand("ParagraphFormat", false, commandParameter);
            return true;
        }
        /// <summary>
        /// 获取当前段落的样式参数
        /// </summary>
        /// <returns></returns>
        private ParagraphFormatCommandParameter GetCurrentParagraphFormatCommandParameter()
        {
            var currentStyle = this._Control.Document.CurrentStyleInfo.Paragraph;
            ParagraphFormatCommandParameter commandParameter = new ParagraphFormatCommandParameter();
            commandParameter.LineSpacingStyle = currentStyle.LineSpacingStyle;
            commandParameter.LineSpacing = currentStyle.LineSpacing;
            commandParameter.SpacingBefore = currentStyle.SpacingBeforeParagraph;
            commandParameter.SpacingAfter = currentStyle.SpacingAfterParagraph;
            commandParameter.FirstLineIndent = currentStyle.FirstLineIndent;
            commandParameter.LeftIndent = currentStyle.LeftIndent;
            commandParameter.OutlineLevel = currentStyle.ParagraphOutlineLevel;
            commandParameter.ParagraphMultiLevel = currentStyle.ParagraphMultiLevel;
            commandParameter.ListStyle = currentStyle.ParagraphListStyle;
            return commandParameter;
        }

        /// <summary>
        /// 兼容四代设置首行缩进
        /// </summary>
        /// <param name="showUI"></param>
        /// <param name="parameter"></param>
        /// <returns></returns>
        private bool command_FirstLineIndent()
        {
            this._Control.ExecuteCommand("FirstLineIndent", false, null);
            return true;
        }

        /// <summary>
        /// 兼容四代接口命令用二维数组对表格插入数据 wyc20230821
        /// </summary>
        /// <param name="parameter">
        ///          {
        //                TableElement: table, //指定的表格
        //                DataArray: [
        //                    ["张三", "李四", "王五"],
        //                    ["123", "456", "789"],
        //                    ["汪汪汪", "喵喵喵", "咩咩咩"]
        //                ], //数据集数组
        //                RowIndex: 1, //指定从哪一行开始写
        //                TableRowExtendable: true //新增属性，设为false则不扩展表格行只写入数据，不处理溢出数据
        //            }
        /// </param>
        /// <returns></returns>
        private bool command_table_writedatatotable(JsonElement parameter)
        {
            if (parameter.ValueKind != JsonValueKind.Object)
            {
                return false;
            }
            DomTableElement table = null;
            JsonElement? dataArr = null;
            int rowindex = -1;
            bool TableRowExtendable = false;
            foreach (JsonProperty property in parameter.EnumerateObject())
            {
                string name = property.Name.ToLower();
                switch (name)
                {
                    case "tableelement":
                        if (property.Value.ValueKind == JsonValueKind.String)
                        {
                            table = this._Control.GetTableElementById(property.Value.GetString());
                        }
                        else if (property.Value.ValueKind == JsonValueKind.Number)
                        {
                            table = this.GetElementByNativeHandle(property.Value.GetInt32()) as DomTableElement;
                        }
                        break;
                    case "dataarray":
                        if (property.Value.ValueKind == JsonValueKind.Array)
                        {
                            dataArr = property.Value;
                        }
                        break;
                    case "rowindex":
                        if (property.Value.ValueKind == JsonValueKind.Number)
                        {
                            rowindex = property.Value.GetInt32();
                        }
                        break;
                    case "tablerowextendable":
                        TableRowExtendable = property.ConvertToBoolean(true);
                        break;
                    default:
                        break;
                }
            }
            if (table == null)
            {
                return false;
            }
            if (dataArr == null || dataArr.HasValue == false || dataArr.Value.ValueKind != JsonValueKind.Array)
            {
                return false;
            }
            if (rowindex < 0 || rowindex >= table.RowsCount)
            {
                rowindex = 0;
            }
            int arrCount = dataArr.Value.GetArrayLength();
            int rowCountToProcess = table.Rows.Count - rowindex;
            if (TableRowExtendable == true && arrCount > rowCountToProcess)
            {
                int count = arrCount - rowCountToProcess;
                DomTableRowElement clonerow = table.Rows[table.RowsCount - 1].Clone(true) as DomTableRowElement;
                foreach (DomTableCellElement cell in clonerow.Cells)
                {
                    //wyc20240711:额外处理单元格对齐元素样式继承的问题DUWRITER5_0-3098
                    int pstyleindex = cell.LastChild is DomParagraphFlagElement ? cell.LastChild.StyleIndex : -1;
                    cell.Elements.Clear();
                    DomParagraphFlagElement flag = new DomParagraphFlagElement();
                    flag.StyleIndex = pstyleindex;
                    cell.Elements.Add(flag);
                }
                for (int i = 1; i <= count; i++)
                {
                    table.Rows.Add(clonerow.Clone(true));
                }
            }
            DomTableRowElement processRow = table.Rows[rowindex] as DomTableRowElement;
            foreach (JsonElement rowObj in dataArr.Value.EnumerateArray())
            {
                if (rowObj.ValueKind != JsonValueKind.Array)
                {
                    rowindex++;
                    processRow = table.Rows[rowindex] as DomTableRowElement;
                    continue;
                }
                int index = 0;
                foreach (JsonElement cellObj in rowObj.EnumerateArray())
                {
                    if (index >= processRow.CellsCount)
                    {
                        break;
                    }
                    DomTableCellElement cell = processRow.Cells[index] as DomTableCellElement;
                    int contentstyleindex = cell.Elements.Count > 0 ? cell.Elements[0].StyleIndex : -1;
                    int pstyleindex = cell.Elements.Count > 0 ? cell.Elements[0].OwnerParagraphEOF.StyleIndex : -1;
                    string str = WASMJsonConvert.parseJsonElementToString(cellObj);
                    cell.SetTextRawDOM(str, contentstyleindex, pstyleindex);
                    index++;
                }
                if (rowindex == table.RowsCount - 1)
                {
                    break;
                }
                rowindex++;
                processRow = table.Rows[rowindex] as DomTableRowElement;
            }
            table.EditorRefreshView();
            return true;
        }




        /// 和旧版BS编辑器命令保持兼容 wyc20230512
        /// <summary>
        /// 获取元素属性
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [JSInvokable]
        public JsonNode DCGetElementProperties2(JsonElement element, bool smartMode = false)
        {

            smartMode = smartMode || WASM.WriterControlForWASM.bGetElementPropertiesExcludeDefault
                || WriteElementJsonWithoutCheckDefaultValue == false;

            CheckStateBeforeInvoke();
            try
            {
                //wyc20230615:兼容前端既传字符串ID，又传元素属性列表对象
                string id = null;
                if (element.ValueKind == JsonValueKind.String)
                {
                    id = element.GetString();
                }
                else if (element.ValueKind == JsonValueKind.Object)
                {
                    //现在修改为优先获取handle
                    foreach (JsonProperty pt in element.EnumerateObject())
                    {
                        if (pt.Name != null && pt.Name.ToLower() == "nativehandle")
                        {
                            if (pt.Value.ValueKind == JsonValueKind.Number)
                            {
                                int intHandle = pt.Value.GetInt32();
                                if (intHandle >= 0)
                                {
                                    DomElement ele = GetElementByNativeHandle(intHandle);
                                    return innerGetElementProperties(ele, smartMode);
                                }
                            }
                        }
                    }
                    foreach (JsonProperty pt in element.EnumerateObject())
                    {
                        if (pt.Value.ValueKind == JsonValueKind.String && pt.Name != null && pt.Name.ToLower() == "id")
                        {
                            id = pt.Value.GetString();
                            break;
                        }
                    }
                }
                //wyc20230705:新增获取前端传来的数字，将数字作为nativehandle来获取元素本身
                else if (element.ValueKind == JsonValueKind.Number)
                {
                    int handle = element.GetInt32();
                    if (handle == -1)
                    {
                        return null;
                    }
                    DomElement ele = GetElementByNativeHandle(handle);
                    if (ele == null)
                    {
                        ele = this.Document.GetElementByHashCode(handle);
                    }
                    if (ele == null)
                    {
                        return null;
                    }
                    else
                    {
                        return innerGetElementProperties(ele, smartMode);
                    }
                }
                ////////////////////////////////////////////////////////////////////////////

                DomElement element2 = null;
                if (id == null || id.Length == 0)
                {
                    return null;
                }
                element2 = this._Control.GetElementById(id);
                return innerGetElementProperties(element2, smartMode);
            }
            catch (Exception ex)
            {
                JavaScriptMethods.Tools_ReportException("DCGetElementProperties2", ex.Message, ex.ToString(), 0);
                return null;
            }
        }
        /// <summary>
        /// 获取元素属性
        /// </summary>
        /// <param name="frontelement">前端表示的.NET对象</param>
        /// <returns></returns>
        [JSInvokable]
        public JsonNode DCGetElementProperties(DotNetObjectReference<DomElement> frontelement, bool smartMode = false)
        {
            smartMode = smartMode || WASM.WriterControlForWASM.bGetElementPropertiesExcludeDefault
                || WriteElementJsonWithoutCheckDefaultValue == false;

            CheckStateBeforeInvoke();
            try
            {
                if (frontelement == null)
                {
                    return null;
                }
                var element = frontelement.Value;
                //frontelement.Dispose();
                return innerGetElementProperties(element, smartMode);
            }
            catch (Exception ex)
            {
                JavaScriptMethods.Tools_ReportException("DCGetElementProperties", ex.Message, ex.ToString(), 0);
                return null;
            }
        }
        internal JsonNode innerGetElementProperties(DomElement element, bool smartMode = false)
        {
            if (element == null)
            {
                return null;
            }

            smartMode = smartMode || WASM.WriterControlForWASM.bGetElementPropertiesExcludeDefault
                || WriteElementJsonWithoutCheckDefaultValue == false;

            //wyc20230703:通用属性处理
            JsonObject jsobj = new JsonObject();
            GetXTextElementProperties(element, jsobj, smartMode);
            if (element is DomObjectElement)
            {
                GetXTextObjectElementProperties((DomObjectElement)element, jsobj, smartMode);
            }
            else if (element is DomContainerElement)
            {
                GetXTextContainerElementProperties((DomContainerElement)element, jsobj, smartMode);
            }



            if (element is DomCheckBoxElementBase)
            {
                GetCheckBoxElementBaseProperties((DomCheckBoxElementBase)element, jsobj, smartMode);
            }
            else if (element is DomInputFieldElement)
            {
                GetInputFieldElementProperties((DomInputFieldElement)element, jsobj, smartMode);
            }
            else if (element is DomImageElement)
            {
                GetImageElementProperties((DomImageElement)element, jsobj, smartMode);
            }
            else if (element is DomPageInfoElement)
            {
                GetPageInfoProperties((DomPageInfoElement)element, jsobj, smartMode);
            }
            else if (element is DomTableElement)
            {
                GetTableElementProperties((DomTableElement)element, jsobj, smartMode);
            }
            else if (element is DomTableCellElement)
            {
                GetTableCellElementProperties((DomTableCellElement)element, jsobj, smartMode);
            }
            else if (element is DomDocument)
            {
                GetXTextDocumentProperties((DomDocument)element, jsobj, smartMode);
            }
            else if (element is DomTableRowElement)
            {
                GetTableRowElementProperties((DomTableRowElement)element, jsobj, smartMode);
            }
            else if (element is DomTableColumnElement)
            {
                GetTableColumnElementProperties((DomTableColumnElement)element, jsobj, smartMode);
            }
            return jsobj;
        }
        /// <summary>
        /// 设置元素属性
        /// </summary>
        /// <param name="id"></param>
        /// <param name="parameter"></param>
        [JSInvokable]
        public bool DCSetElementProperties2(JsonElement element, JsonElement parameter, JsonElement isrefresh)
        {
            CheckStateBeforeInvoke();
            try
            {
                //wyc20240819:添加刷新元素机制
                bool refresh = true;
                if (isrefresh.ValueKind == JsonValueKind.False ||
                    (isrefresh.ValueKind == JsonValueKind.String && isrefresh.GetString().ToLower() == "false"))
                {
                    refresh = false;
                }

                //wyc20230615:兼容前端既传字符串ID，又传元素属性列表对象
                string id = null;
                if (element.ValueKind == JsonValueKind.String)
                {
                    id = element.GetString();
                }
                else if (element.ValueKind == JsonValueKind.Object)
                {

                    //现在修改为优先获取handle
                    foreach (JsonProperty pt in element.EnumerateObject())
                    {
                        if (pt.Name != null && pt.Name.ToLower() == "nativehandle")
                        {
                            if (pt.Value.ValueKind == JsonValueKind.Number)
                            {
                                int intHandle = pt.Value.GetInt32();
                                if (intHandle >= 0)
                                {
                                    DomElement ele = GetElementByNativeHandle(intHandle);
                                    bool result1 = innerSetElementProperties(ele, parameter, false, refresh);
                                    return result1;
                                }
                            }
                        }
                    }
                    foreach (JsonProperty pt in element.EnumerateObject())
                    {
                        if (pt.Value.ValueKind == JsonValueKind.String && pt.Name.ToLower() == "id")
                        {
                            id = pt.Value.GetString();
                            break;
                        }
                    }
                }
                //wyc20230705:新增获取前端传来的数字，将数字作为nativehandle来获取元素本身
                else if (element.ValueKind == JsonValueKind.Number)
                {
                    int handle = element.GetInt32();
                    if (handle == -1)
                    {
                        return false;
                    }
                    DomElement ele = GetElementByNativeHandle(handle);
                    bool result1 = innerSetElementProperties(ele, parameter, false, refresh);
                    return result1;
                }
                ////////////////////////////////////////////////////////////////////////////

                DomElement element2 = null;
                if (id == null || id.Length == 0)
                {
                    element2 = this._Control.CurrentElement;//如果为空取当前
                }
                element2 = this._Control.GetElementById(id);
                bool result = innerSetElementProperties(element2, parameter, false, refresh);
                return result;
            }
            catch (Exception ex)
            {
                JavaScriptMethods.Tools_ReportException("DCSetElementProperties2", ex.Message, ex.ToString(), 0);
                return false;
            }
        }
        /// <summary>
        /// 设置元素属性
        /// </summary>
        /// <param name="id"></param>
        /// <param name="parameter"></param>
        [JSInvokable]
        public bool DCSetElementProperties(DotNetObjectReference<DomElement> frontelement, JsonElement parameter, JsonElement isrefresh)
        {
            CheckStateBeforeInvoke();
            try
            {
                if (frontelement == null)
                {
                    return false;
                }
                var element = frontelement.Value;
                if (frontelement != null)
                {
                    frontelement.Dispose();
                }

                bool refresh = true;
                if (isrefresh.ValueKind == JsonValueKind.False ||
                    (isrefresh.ValueKind == JsonValueKind.String && isrefresh.GetString().ToLower() == "false"))
                {
                    refresh = false;
                }
                return innerSetElementProperties(element, parameter, false, refresh);
            }
            catch (Exception ex)
            {
                JavaScriptMethods.Tools_ReportException("DCSetElementProperties", ex.Message, ex.ToString(), 0);
                return false;
            }
        }
        private bool innerSetElementProperties(DomElement element, JsonElement parameter, bool forinsert = false, bool isrefresh = true, DomContainerElement parent = null)
        {
            if (element == null)
            {
                return false;
            }

            var bolLogUndo = this._Control.DocumentOptions.BehaviorOptions.EnableLogUndo;
            //记录撤销
            if (parameter.ValueKind == JsonValueKind.Object)
            {
                foreach (JsonProperty property in parameter.EnumerateObject())
                {
                    if (string.Equals(property.Name, "LogUndo", StringComparison.OrdinalIgnoreCase))
                    {
                        bolLogUndo = property.ConvertToBoolean(true);
                        break;
                    }
                }
            }
            //bool logUndo = this._Control.DocumentOptions.BehaviorOptions.EnableLogUndo && this._Control.Document.CanLogUndo;
            if (bolLogUndo)// this._Control.DocumentOptions.BehaviorOptions.EnableLogUndo == true)
            {
                this._Control.Document.BeginLogUndo();
            }

            //wyc20241231:若有对输入域赋值，抢前对INNERVALUE进行赋值，
            //避免下拉选择赋值TEXT时引发了表达式但INNERVALUE还未赋值导致表达式错误DUWRITER5_0-4072
            JsonElement tempJsonEle;
            if (element is DomInputFieldElement &&
                parameter.TryGetProperty("InnerValue", out tempJsonEle) == true &&
                tempJsonEle.ValueKind == JsonValueKind.String)
            {
                ((DomInputFieldElement)element).InnerValue = tempJsonEle.JsonElementToString();
            }
            ////////////////////////////////////////////////////////////

            XTextDocumentUndoList undolist = element.OwnerDocument?.UndoList;

            //wyc20230703:通用属性处理
            bool result = SetXTextElementProperties(element, parameter, forinsert, undolist);
            bool result1 = false;
            bool result2 = false;
            bool result3 = false;
            bool result4 = false;
            bool result5 = false;
            bool result6 = false;
            bool result7 = false;
            bool result8 = false;
            bool result9 = false;
            bool result10 = false;
            bool result11 = false;
            bool result12 = false;
            bool result13 = false;
            bool result14 = false;
            bool result15 = false;
            bool result16 = false;
            bool result17 = false;
            bool result18 = false;
            bool result19 = false;
            bool result20 = false;
            bool result21 = false;
            bool result22 = false;
            bool result23 = false;



            if (element is DomObjectElement)
            {
                result1 = SetXTextObjectElementProperties((DomObjectElement)element, parameter, undolist);
            }
            else if (element is DomContainerElement)
            {
                result2 = SetXTextContainerElementProperties((DomContainerElement)element, parameter, undolist);
            }
            if (element is DomCheckBoxElementBase)
            {
                result5 = SetCheckBoxBaseElementProperties((DomCheckBoxElementBase)element, parameter, undolist);
            }
            else if (element is DomInputFieldElement)
            {
                result7 = SetInputFieldElementProperties((DomInputFieldElement)element, parameter, undolist);
            }
            else if (element is DomImageElement)
            {
                result8 = SetImageElementProperties((DomImageElement)element, parameter, isrefresh, parent, undolist);
            }
            else if (element is DomPageInfoElement)
            {
                result11 = SetPageInfoProperties((DomPageInfoElement)element, parameter, undolist);
            }
            else if (element is DomTableElement)
            {
                result15 = SetTableElementProperties((DomTableElement)element, parameter, undolist);
            }
            else if (element is DomTableCellElement)
            {
                result16 = SetTableCellElementProperties((DomTableCellElement)element, parameter, undolist);
            }
            else if (element is DomDocument)
            {
                result17 = SetXTextDocumentProperties((DomDocument)element, parameter, undolist);
            }
            else if (element is DomTableRowElement)
            {
                result18 = SetTableRowElementProperties((DomTableRowElement)element, parameter, undolist);
            }
            else if (element is DomTableColumnElement)
            {
                result19 = SetTableColumnElementProperties((DomTableColumnElement)element, parameter, undolist);
            }
            bool res = result || result1 || result2 || result3 || result4 || result5 || result6 || result7 || result8 || result9 || result10 || result11 || result12 || result13 || result14 || result15 || result16 || result17 || result18 || result19 || result20 || result21 || result22 || result23;
            if (res)
            {
                this._Control.Document.Modified = true;
            }
            if (this._Control.DocumentOptions.BehaviorOptions.EnableLogUndo == true)
            {
                this._Control.Document.EndLogUndo();
            }
            //wyc20240819:调整设置元素属性后的刷新机制
            if (isrefresh == true)
            {
                element.EditorRefreshView();
            }
            this._Control.Document._CurrentStyleInfo = null;

            return res;
        }


        ////////////////////////////////////////////////////////////////////////////////////////////////

        private bool command_insertcheckboxradio(bool showUI, object parameter)
        {
            if (parameter == null || (parameter is JsonElement) == false)
            {
                return false;
            }
            JsonElement para = (JsonElement)parameter;
            DomElementList list = ParseJSONToCheckBoxBaseElements(para);


            //自动修正重复的ID
            if (parameter != null)
            {
                bool AutoMaticCorrectionRepeatID = GetAutoMaticCorrectionRepeatID((JsonElement)parameter);
                if (AutoMaticCorrectionRepeatID)
                {
                    if (list != null && list.Count > 0)
                    {
                        for (int i = 0; i < list.Count; i++)
                        {
                            DomElement element = list[i];
                            if (element.ID != null && element.ID != "" && element.ID.Length > 0)
                            {
                                MaticCorrectionRepeatID(element);
                            }
                        }
                    }
                }
            }

            if (list == null || list.Count == 0)
            {
                return false;
            }

            object obj = this._Control.ExecuteCommand(
                DCSoft.Writer.StandardCommandNames.InsertElements,
                showUI,
                list);
            bool result = obj != null;
            return result;
        }
        private bool SetCheckBoxBaseElementProperties(DomCheckBoxElementBase element, JsonElement parameter, XTextDocumentUndoList undolist = null)
        {
            bool logUndo = this._Control.DocumentOptions.BehaviorOptions.EnableLogUndo &&
                this._Control.Document.CanLogUndo &&
                undolist != null;

            string oldvalues = null;
            bool oldvalueb = false;

            bool result = false;
            if (element == null)
            {
                return result;
            }
            foreach (System.Text.Json.JsonProperty property in parameter.EnumerateObject())
            {
                string name = property.Name.ToLower();
                //float f = float.NaN;
                switch (name)
                {
                    case "multiline":
                        oldvalueb = element.Multiline;
                        element.Multiline = WASMUtils.ConvertToBoolean(property, true);
                        if (logUndo && element.Multiline != oldvalueb)
                        {
                            undolist.AddProperty("Multiline", oldvalueb, element.Multiline, element);
                        }
                        result = true;
                        break;
                    case "checked":
                        oldvalueb = element.Checked;
                        element.Checked = WASMUtils.ConvertToBoolean(property, true);
                        if (logUndo && element.Checked != oldvalueb)
                        {
                            undolist.AddProperty("Checked", oldvalueb, element.Checked, element);
                        }
                        result = true;
                        break;
                    case "caption":
                    case "text":
                        oldvalues = element.Caption;
                        element.Caption = WASMUtils.ConvertToString(property);
                        if (logUndo && element.Caption != oldvalues)
                        {
                            undolist.AddProperty("Caption", oldvalues, element.Caption, element);
                        }
                        result = true;
                        break;
                    case "value":
                    case "checkedvalue":
                        oldvalues = element.CheckedValue;
                        element.CheckedValue = WASMUtils.ConvertToString(property);
                        if (logUndo && element.CheckedValue != oldvalues)
                        {
                            undolist.AddProperty("CheckedValue", oldvalues, element.CheckedValue, element);
                        }
                        result = true;
                        break;
                    case "requried":
                        oldvalueb = element.Requried;
                        element.Requried = WASMUtils.ConvertToBoolean(property, true);//WASMJsonConvert.parseBoolean(value, true);
                        if (logUndo && element.Requried != oldvalueb)
                        {
                            undolist.AddProperty("Requried", oldvalueb, element.Requried, element);
                        }
                        result = true;
                        break;
                    default:
                        break;
                }
            }
            if (element.OwnerDocument == null)
            {
                element.OwnerDocument = this._Control.Document;
            }
            //element.EditorRefreshView();
            return result;
        }
        private void GetCheckBoxElementBaseProperties(DomCheckBoxElementBase element, JsonObject obj, bool checkDefaultValue = false)
        {
            if (element == null)
            {
                return;
            }
            obj.Add("IsRadio", element is DomRadioBoxElement);
            obj.Add("Text", element.Caption);
            if (element.Caption != null || checkDefaultValue == false)
                obj.Add("Caption", element.Caption);
            if (element.CheckedValue != null || checkDefaultValue == false)
                obj.Add("Value", element.CheckedValue);
            if (element.Checked != false || checkDefaultValue == false)
                obj.Add("Checked", element.Checked);
            if (element.Multiline != false || checkDefaultValue == false)
                obj.Add("Multiline", element.Multiline);
            if (element.Requried != false || checkDefaultValue == false)
                obj.Add("Requried", element.Requried);
        }
        //wyc20230818:将四代格式的插入多选单选框组的JSON对象转成选框组的元素列表
        private DomElementList ParseJSONToCheckBoxBaseElements(JsonElement parameter, bool RefreshView = true)
        {
            DomElementList list = new DomElementList();
            string groupName = null;
            bool grouprequried = false;
            JsonElement para = (JsonElement)parameter;
            JsonElement jsonListItems = (JsonElement)parameter;
            bool hasList = false;
            bool ischeckbox = false;
            bool isNewParagraph = false;//是否在两个元素之间换行
            foreach (System.Text.Json.JsonProperty property in para.EnumerateObject())
            {
                string name = property.Name.ToLower();
                string value = null;
                if (name != "listitems")
                {
                    value = WASMJsonConvert.parseJsonElementToString(property.Value);
                }
                switch (name)
                {
                    case "name":
                        groupName = value;
                        break;
                    case "requried":
                        grouprequried = WASMUtils.ConvertToBoolean(property, true);// WASMJsonConvert.parseBoolean(value, true);
                        break;
                    case "type":
                        ischeckbox = value.ToLower() == "checkbox";
                        break;
                    case "listitems":
                        if (property.Value.ValueKind == JsonValueKind.Array)
                        {
                            jsonListItems = property.Value;
                            hasList = true;
                        }
                        break;
                    case "isnewparagraph":
                        isNewParagraph = WASMUtils.ConvertToBoolean(property, true);// WASMJsonConvert.parseBoolean(value, true);
                        break;
                    default:
                        break;
                }
            }
            if (hasList == false)
            {
                return null;
            }
            foreach (System.Text.Json.JsonElement xelement in jsonListItems.EnumerateArray())
            {
                DomCheckBoxElementBase chk = null;
                if (ischeckbox == true)
                {
                    chk = new DomCheckBoxElement();
                }
                else
                {
                    chk = new DomRadioBoxElement();
                }
                if (this._Control.CurrentStyle != null)
                {
                    chk.Style = this._Control.CurrentStyle;
                }

                innerSetElementProperties(chk, xelement, true, RefreshView);
                chk.Name = groupName;
                //chk.Requried = grouprequried;
                list.Add(chk);

                if (isNewParagraph)
                {//在每一个单选框后面追加一个回车
                    DomParagraphFlagElement newElement = new DomParagraphFlagElement();
                    list.InsertAfter(chk, newElement);
                }
            }

            if (isNewParagraph && list != null && list.Count > 0)
            {//移除最后一个回车
                list.RemoveAt(list.Count - 1);//移除最后一个
            }
            return list;
        }
        /// /////////////////////////////////////////////////////////////////////////////////////////////////

        //获取前端传的是否需要校验重复ID的属性
        private bool GetAutoMaticCorrectionRepeatID(JsonElement parameter)
        {//AutoMaticCorrectionRepeatID
            bool result = false;
            foreach (JsonProperty property in parameter.EnumerateObject())
            {
                string name = property.Name.ToLower();
                switch (name)
                {
                    case "automaticcorrectionrepeatid":
                        result = WASMUtils.ConvertToBoolean(property, true);//  WASMJsonConvert.parseBoolean(property.Value, true);

                        break;
                    default:
                        break;
                }
            }
            return result;
        }


        /// <summary>
        /// 自动修正前端指定的编号、制定的元素类型是否存在，如果存在需要在编号后面加序号
        /// </summary>
        /// <param name="element"></param>
        /// <param name="typeName"></param>
        private void MaticCorrectionRepeatID(DomElement element)
        {
            DomElement tagElement = this._Control.GetElementById(element.ID);
            if (tagElement != null && tagElement.TypeName == element.TypeName)
            {//存在重复的
                bool idRepeatID = true;//是否重复ID
                int index = 1;
                while (idRepeatID == true)
                {
                    tagElement = this._Control.GetElementById(element.ID + "_" + index);
                    if (tagElement != null && tagElement.TypeName == element.TypeName)
                    {//存在重复
                        index++;
                        continue;
                    }
                    else
                    {//不存在重复
                        element.ID = element.ID + "_" + index;
                        idRepeatID = false;
                        break;
                    }
                }
            }
            else
            {//不存在重复
                return;
            }
        }


        private JsonNode command_insertinputfield(bool showUI, object parameter)
        {
            DomInputFieldElement field = new DomInputFieldElement();
            field.Style = this._Control.CurrentStyle;
            if (parameter != null && parameter is JsonElement)
            {
                innerSetElementProperties(field, (JsonElement)parameter, true);
            }

            //自动修正重复的ID
            if (parameter != null)
            {
                bool AutoMaticCorrectionRepeatID = GetAutoMaticCorrectionRepeatID((JsonElement)parameter);
                if (AutoMaticCorrectionRepeatID)
                {
                    if (field.ID != null && field.ID != "" && field.ID.Length > 0)
                    {
                        MaticCorrectionRepeatID(field);
                    }
                }
            }


            field.OwnerDocument = this._Control.Document;
            object obj = this._Control.ExecuteCommand(
                DCSoft.Writer.StandardCommandNames.InsertInputField,
                showUI,
                field);
            if (obj != null)
            {
                DomInputFieldElement result = obj as DomInputFieldElement;
                return innerGetElementProperties(result); ;
            }
            return null;
            //bool result = obj != null;
            //return result;
        }
        private bool SetInputFieldElementProperties(DomInputFieldElement element, JsonElement parameter, XTextDocumentUndoList undolist)
        {
            bool logUndo = this._Control.DocumentOptions.BehaviorOptions.EnableLogUndo &&
                this._Control.Document.CanLogUndo &&
                undolist != null;

            string oldvalues = null;
            bool oldvalueb = false;

            bool result = false;
            if (element == null)
            {
                return result;
            }
            bool haschangedfieldsettings = false;
            InputFieldSettings oldifs = element.FieldSettings != null ? element.FieldSettings.Clone() : null;

            foreach (System.Text.Json.JsonProperty property in parameter.EnumerateObject())
            {
                string name = property.Name.ToLower();
                switch (name)
                {
                    case "name":
                        oldvalues = element.Name;
                        element.Name = WASMUtils.ConvertToString(property);
                        if (logUndo && element.Name != oldvalues)
                        {
                            undolist.AddProperty("Name", oldvalues, element.Name, element);
                        }
                        result = true;
                        break;
                    case "innervalue":
                        oldvalues = element.InnerValue;
                        element.InnerValue = WASMUtils.ConvertToString(property);
                        if (logUndo && element.InnerValue != oldvalues)
                        {
                            undolist.AddProperty("InnerValue", oldvalues, element.InnerValue, element);
                        }
                        result = true;
                        break;
                    case "backgroundtext":
                        oldvalues = element.BackgroundText;
                        element.BackgroundText = WASMUtils.ConvertToString(property);
                        if (logUndo && element.BackgroundText != oldvalues)
                        {
                            undolist.AddProperty("BackgroundText", oldvalues, element.BackgroundText, element);
                        }
                        result = true;
                        break;
                    case "enablehighlight":
                        EnableState oldvalue2 = element.EnableHighlight;
                        element.EnableHighlight = WASMUtils.ConvertToEnum<EnableState>(property, EnableState.Default);// WASMJsonConvert.parseEnumValue<EnableState>(value, EnableState.Default);
                        if (logUndo && element.EnableHighlight != oldvalue2)
                        {
                            undolist.AddProperty("EnableHighlight", oldvalue2, element.EnableHighlight, element);
                        }
                        result = true;
                        break;

                    case "usereditable":
                        oldvalueb = element.UserEditable;
                        element.UserEditable = WASMUtils.ConvertToBoolean(property, element.UserEditable);// WASMJsonConvert.parseBoolean(value, element.UserEditable);
                        if (logUndo && element.UserEditable != oldvalueb)
                        {
                            undolist.AddProperty("UserEditable", oldvalueb, element.UserEditable, element);
                        }
                        result = true;
                        break;
                    case "movefocushotkey":
                        MoveFocusHotKeys oldvalue7 = element.MoveFocusHotKey;
                        element.MoveFocusHotKey = WASMUtils.ConvertToEnum<MoveFocusHotKeys>(property, MoveFocusHotKeys.Default);// WASMJsonConvert.parseEnumValue<MoveFocusHotKeys>(value, MoveFocusHotKeys.Default);
                        if (logUndo && element.MoveFocusHotKey != oldvalue7)
                        {
                            undolist.AddProperty("MoveFocusHotKey", oldvalue7, element.MoveFocusHotKey, element);
                        }
                        result = true;
                        break;
                    case "editoractivemode":
                        ValueEditorActiveMode oldvalue8 = element.EditorActiveMode;
                        element.EditorActiveMode = WASMUtils.ConvertToEnum<ValueEditorActiveMode>(property, ValueEditorActiveMode.Default);// WASMJsonConvert.parseEnumValue<ValueEditorActiveMode>(value, ValueEditorActiveMode.Default); ;
                        if (logUndo && element.EditorActiveMode != oldvalue8)
                        {
                            undolist.AddProperty("EditorActiveMode", oldvalue8, element.EditorActiveMode, element);
                        }
                        result = true;
                        break;
                    case "displayformat":
                        ValueFormater oldvaluevf = element.DisplayFormat == null ? null : element.DisplayFormat.Clone();
                        element.DisplayFormat = WASMJsonConvert.parseJSONToValueFormater(property.Value);
                        if (logUndo && (element.DisplayFormat != null && element.DisplayFormat.Equals(oldvaluevf) == false))
                        {
                            undolist.AddProperty("DisplayFormat", oldvaluevf, element.DisplayFormat, element);
                        }
                        result = true;
                        break;
                    case "listitems":
                        ListItemCollection lic = WASMJsonConvert.parseJSONToListItems(property.Value);
                        //if (lic == null || lic.Count == 0)
                        //{
                        //    break;
                        //}
                        if (element.FieldSettings == null)
                        {
                            element.FieldSettings = new InputFieldSettings();
                        }
                        if (element.FieldSettings.ListSource == null)
                        {
                            element.FieldSettings.ListSource = new ListSourceInfo();
                        }
                        element.FieldSettings.ListSource.Items = lic;
                        haschangedfieldsettings = true; result = true;
                        break;
                    case "inneritemspliter":
                        if (element.FieldSettings == null)
                        {
                            element.FieldSettings = new InputFieldSettings();
                        }
                        element.FieldSettings.ListValueSeparatorChar = WASMUtils.ConvertToString(property);
                        haschangedfieldsettings = true; result = true;
                        break;
                    case "bordervisible":
                        DCVisibleState oldvalue9 = element.BorderVisible;
                        element.BorderVisible = WASMUtils.ConvertToEnum<DCVisibleState>(property, DCVisibleState.Default);
                        if (logUndo && element.BorderVisible != oldvalue9)
                        {
                            undolist.AddProperty("BorderVisible", oldvalue9, element.BorderVisible, element);
                        }
                        result = true;
                        break;
                    case "tabstop":
                        oldvalueb = element.TabStop;
                        element.TabStop = WASMUtils.ConvertToBoolean(property, false);
                        if (logUndo && element.TabStop != oldvalueb)
                        {
                            undolist.AddProperty("TabStop", oldvalueb, element.TabStop, element);
                        }
                        result = true;
                        break;
                    default:
                        break;
                }
            }
            if (logUndo == true && haschangedfieldsettings == true)
            {
                undolist.AddProperty("FieldSettings", oldifs, element.FieldSettings, element);
            }
            if (element.OwnerDocument == null)
            {
                element.OwnerDocument = this._Control.Document;
            }
            //element.EditorRefreshView();
            return result;
        }
        private void GetInputFieldElementProperties(DomInputFieldElement element, JsonObject obj, bool checkDefaultValue = false)
        {
            if (element == null)
            {
                return;
            }
            try
            {
                if (element.Name != null || checkDefaultValue == false)
                    obj.Add("Name", element.Name);
                if (element.InnerValue != null || checkDefaultValue == false)
                    obj.Add("InnerValue", element.InnerValue);
                if (element.BackgroundText != null || checkDefaultValue == false)
                    obj.Add("BackgroundText", element.BackgroundText);
                if (element.EnableHighlight != EnableState.Default || checkDefaultValue == false)
                    obj.Add("EnableHighlight", element.EnableHighlight.ToString());
                if (element.UserEditable != true || checkDefaultValue == false)
                    obj.Add("UserEditable", element.UserEditable);
                if (element.MoveFocusHotKey != MoveFocusHotKeys.Default || checkDefaultValue == false)
                    obj.Add("MoveFocusHotKey", element.MoveFocusHotKey.ToString());
                if (element.EditorActiveMode != ValueEditorActiveMode.Default || checkDefaultValue == false)
                    obj.Add("EditorActiveMode", element.EditorActiveMode.ToString());
                InputFieldSettings ifsettings = element.FieldSettings;
                InputFieldEditStyle ifes = ifsettings != null ? ifsettings.EditStyle : InputFieldEditStyle.Text;
                bool innermulitiselect = ifsettings != null ? ifsettings.MultiSelect : false;
                string listvalueseparatorchar = ifsettings != null ? ifsettings.ListValueSeparatorChar : ",";
                if (ifes != InputFieldEditStyle.Text || checkDefaultValue == false)
                    obj.Add("InnerEditStyle", ifes.ToString());
                if (innermulitiselect != false || checkDefaultValue == false)
                    obj.Add("InnerMultiSelect", innermulitiselect);
                if (listvalueseparatorchar != "," || checkDefaultValue == false)
                    obj.Add("ListValueSeparatorChar", listvalueseparatorchar);
                if (element.DisplayFormat != null || checkDefaultValue == false)
                    obj.Add("DisplayFormat", WASMJsonConvert.parseValueFormaterToJSON(element.DisplayFormat));
                ListItemCollection lic = ifsettings != null && ifsettings.ListSource != null ? ifsettings.ListSource.Items : null;
                if (lic != null || checkDefaultValue == false)
                    obj.Add("ListItems", WASMJsonConvert.parseListItemsToJSON(lic));
                if (element.BorderVisible != DCVisibleState.Default || checkDefaultValue == false)
                    obj.Add("BorderVisible", element.BorderVisible.ToString());
                //wyc20231215添加：
                if (element.TabStop != true || checkDefaultValue == false)
                    obj.Add("TabStop", element.TabStop);
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        /// /////////////////////////////////////////////////////////////////////////////////////////////////

        private bool command_insertimageelement(bool showUI, object parameter)
        {
            if (parameter == null || (parameter is JsonElement) == false)
            {
                return false;
            }
            JsonElement param = (JsonElement)parameter;

            if (param.ValueKind == JsonValueKind.Object)
            {
                //wyc20230810:针对图片元素新增插入位置参数设置
                JsonElement tempElement;
                DomElement afterElement = null;
                DomElement beforeElement = null;
                DomContainerElement insideElement = null;
                if (param.TryGetProperty("AfterElement", out tempElement) == true)
                {
                    if (tempElement.ValueKind == JsonValueKind.Number)
                    {
                        afterElement = this.GetElementByNativeHandle(tempElement.GetInt32());
                    }
                    else if (tempElement.ValueKind == JsonValueKind.String)
                    {
                        afterElement = this._Control.GetElementById(tempElement.GetString());
                    }
                }
                else if (param.TryGetProperty("BeforeElement", out tempElement) == true)
                {
                    if (tempElement.ValueKind == JsonValueKind.Number)
                    {
                        beforeElement = this.GetElementByNativeHandle(tempElement.GetInt32());
                    }
                    else if (tempElement.ValueKind == JsonValueKind.String)
                    {
                        beforeElement = this._Control.GetElementById(tempElement.GetString());
                    }
                }
                else if (param.TryGetProperty("InsideElement", out tempElement) == true)
                {
                    if (tempElement.ValueKind == JsonValueKind.Number)
                    {
                        insideElement = this.GetElementByNativeHandle(tempElement.GetInt32()) as DomContainerElement;
                    }
                    else if (tempElement.ValueKind == JsonValueKind.String)
                    {
                        insideElement = this._Control.GetElementById(tempElement.GetString()) as DomContainerElement;
                    }
                }
                //////////////////////////////////////////////////
                ///
                DomImageElement img = new DomImageElement();
                img.Style = this._Control.CurrentStyle;
                innerSetElementProperties(img, param, true);
                //wyc20230915:对未指定ID的图片新指派一个ID：DUWRITER5_0-866
                if ((img.ID == null || img.ID.Length == 0) && this._Control.DocumentOptions.BehaviorOptions.AutoFixElementIDWhenInsertElements)
                {
                    this._Control.Document.AllocElementID("image", typeof(DomImageElement));
                }
                /////////////////////////////////////////////////////////////

                //自动修正重复的ID
                if (parameter != null)
                {
                    bool AutoMaticCorrectionRepeatID = GetAutoMaticCorrectionRepeatID((JsonElement)parameter);
                    if (AutoMaticCorrectionRepeatID)
                    {
                        if (img.ID != null && img.ID != "" && img.ID.Length > 0)
                        {
                            MaticCorrectionRepeatID(img);
                        }
                    }
                }


                img.OwnerDocument = this._Control.Document;

                //wyc20230810:针对图片元素新增插入位置参数设置
                if (insideElement != null)
                {
                    insideElement.Elements.Add(img);
                    insideElement.EditorRefreshView();
                    return true;
                }
                else if (beforeElement != null)
                {
                    beforeElement.Parent.Elements.InsertBefore(beforeElement, img);
                    beforeElement.Parent.EditorRefreshView();
                    return true;
                }
                else if (afterElement != null)
                {
                    afterElement.Parent.Elements.InsertAfter(afterElement, img);
                    afterElement.Parent.EditorRefreshView();
                    return true;
                }
                else
                //////////////////////////////////////////////////////////
                {

                    object obj = this._Control.ExecuteCommand(
                        DCSoft.Writer.StandardCommandNames.InsertImage,
                        showUI,
                        img);
                    bool result = obj != null;
                    return result;
                }
            }
            else if (param.ValueKind == JsonValueKind.Array)
            {
                DomElementList list = new DomElementList();
                foreach (JsonElement element in param.EnumerateArray())
                {
                    DomImageElement img = new DomImageElement();
                    img.Style = this._Control.CurrentStyle;
                    innerSetElementProperties(img, element, true);
                    img.OwnerDocument = this._Control.Document;
                    list.Add(img);
                }
                if (list.Count > 0)
                {
                    this._Control.ExecuteCommand(
                        DCSoft.Writer.StandardCommandNames.InsertElements,
                        showUI,
                        list);
                    return true;
                }
                return false;
            }
            return false;
        }
        private bool SetImageElementProperties(DomImageElement element, JsonElement parameter, bool refresh = true, DomContainerElement parent = null, XTextDocumentUndoList undolist = null)
        {
            bool logUndo = this._Control.DocumentOptions.BehaviorOptions.EnableLogUndo &&
                this._Control.Document.CanLogUndo &&
                undolist != null;

            bool oldvalueb = false;

            bool result = false;
            if (element == null || parameter.ValueKind != JsonValueKind.Object)
            {
                return result;
            }

            bool isRefreshParent = false;//是否刷新图片。在通过对话框修改宽高时，不会实时显示，需要单独刷新；在批量修改时，又不需要单独刷新
            string src = null;
            //wyc20230811:优化图片元素尺寸判断逻辑
            float settedWidth = 0;
            float settedHeight = 0;
            bool autofitimagesize = false;
            /////////////////////////////////////
            foreach (JsonProperty property in parameter.EnumerateObject())
            {
                string name = property.Name.ToLower();
                float f = float.NaN;
                //int i = int.MinValue;
                switch (name)
                {
                    case "keepwidthheightrate":
                        oldvalueb = element.KeepWidthHeightRate;
                        element.KeepWidthHeightRate = WASMUtils.ConvertToBoolean(property, true);// WASMJsonConvert.parseBoolean(property.Value, true);
                        if (logUndo && element.KeepWidthHeightRate != oldvalueb)
                        {
                            undolist.AddProperty("KeepWidthHeightRate", oldvalueb, element.KeepWidthHeightRate, element);
                        }
                        result = true;
                        break;
                    case "src":
                        src = WASMUtils.ConvertToString(property);
                        break;
                    case "width":
                        string value = WASMUtils.ConvertToString(property);
                        if (float.TryParse(value, out f) == true)
                        {
                            settedWidth = f;
                        }
                        break;
                    case "height":
                        value = WASMUtils.ConvertToString(property);
                        if (float.TryParse(value, out f) == true)
                        {
                            settedHeight = f;
                        }
                        break;
                    case "autofitimagesize":
                        if (this._Control.CurrentTableCell != null)
                        {
                            autofitimagesize = WASMUtils.ConvertToBoolean(property, false);// WASMJsonConvert.parseBoolean(property.Value, false);
                        }
                        break;
                    case "valign":
                        VerticalAlignStyle oldvalue2 = element.VAlign;
                        element.VAlign = WASMUtils.ConvertToEnum<VerticalAlignStyle>(property, VerticalAlignStyle.Bottom);
                        if (logUndo && element.VAlign != oldvalue2)
                        {
                            undolist.AddProperty("VAlign", oldvalue2, element.VAlign, element);
                        }
                        break;
                    case "smoothzoom":
                        oldvalueb = element.SmoothZoom;
                        element.SmoothZoom = WASMUtils.ConvertToBoolean(property, false);
                        if (logUndo && element.SmoothZoom != oldvalueb)
                        {
                            undolist.AddProperty("SmoothZoom", oldvalueb, element.SmoothZoom, element);
                        }
                        break;
                    case "refreshparent":
                        isRefreshParent = WASMUtils.ConvertToBoolean(property, false);
                        break;
                    default:
                        break;
                }
            }
            if (src == null || src.Length == 0)
            {
                if (isRefreshParent && refresh)
                {//单独刷新元素
                    if (element != null && element.Parent != null)
                    {
                        element.Parent.EditorRefreshView();
                    }
                }
                return result;
            }

            //bool setsize = true;
            ////只有当手动设置了宽高，且不自动适应时，加载图片时才不使用图片原本的宽高数据
            //if (settedHeight > 0 && 
            //    settedWidth > 0 && 
            //    autofitimagesize == false)
            //{
            //    setsize = false;
            //}

            float originWidthHeightRate = 0;
            if (src.StartsWith("data:image/") == true)
            {
                string[] strs = src.Split(',');
                src = strs[1]; // src.Substring(22);
            }

            //wyc20231115:改成如果base64的话就存入image属性，否则存入source
            bool IsPicBase64String = false;
            XImageValue imgvalue = new XImageValue();
            imgvalue.SafeLoadMode = true;
            try
            {
                imgvalue.LoadBase64String(src);
                if (imgvalue.Value != null)
                {
                    IsPicBase64String = true;
                    element.Image = imgvalue;
                    if (imgvalue.Width > 0 && imgvalue.Height > 0)
                    {
                        element.Width = imgvalue.Width;
                        element.Height = imgvalue.Height;
                    }
                }
            }
            catch
            {
                IsPicBase64String = false;
                imgvalue.Dispose();
            }

            if (IsPicBase64String == false)
            {
                element.Width = settedWidth;
                element.Height = settedHeight;

                if (autofitimagesize && parent != null)
                {
                    float clientwidth = parent.ClientWidth;
                    float clientheight = parent.ClientHeight;
                    if (element.Width == 0)
                    {
                        element.Width = clientwidth;
                    }
                    if (element.Height == 0)
                    {
                        element.Height = clientheight;
                    }
                    float ratewidth = clientwidth / element.Width;
                    float rateheight = clientheight / element.Height;
                    float rate = Math.Min(ratewidth, rateheight);
                    element.Height = element.Height * rate;
                    element.Width = element.Width * rate;
                }

                if (isRefreshParent && refresh)
                {//单独刷新元素
                    if (element != null && element.Parent != null)
                    {
                        element.Parent.EditorRefreshView();
                    }
                }
                return true;
            }
            originWidthHeightRate = element.Width / element.Height;
            //wyc20231107如果不设置宽和高，则使用图片数据原本的宽高。
            //如果设置了宽高，则首先判断keepwidthheightrate先裁剪宽高的比例再赋值到图片
            if (settedWidth > 0 && settedHeight > 0)
            {
                if (element.KeepWidthHeightRate == false)
                {
                    element.Width = settedWidth;
                    element.Height = settedHeight;
                }
                else
                {
                    if (originWidthHeightRate > 1)
                    {
                        settedHeight = settedWidth / originWidthHeightRate;
                    }
                    else
                    {
                        settedWidth = settedHeight * originWidthHeightRate;
                    }
                    element.Width = settedWidth;
                    element.Height = settedHeight;
                }
            }

            //最后根据外围单元格客户区大小调整图片宽高
            if (autofitimagesize && parent != null)
            {
                float clientwidth = parent.ClientWidth;
                float clientheight = parent.ClientHeight;
                if (element.Width == 0)
                {
                    element.Width = clientwidth;
                }
                if (element.Height == 0)
                {
                    element.Height = clientheight;
                }
                float ratewidth = clientwidth / element.Width;
                float rateheight = clientheight / element.Height;
                float rate = Math.Min(ratewidth, rateheight);
                element.Height = element.Height * rate;
                element.Width = element.Width * rate;
                result = true;
            }
            if (isRefreshParent && refresh)
            {//单独刷新元素
                if (element != null && element.Parent != null)
                {
                    element.Parent.EditorRefreshView();
                }
            }
            return result;
        }
        private void GetImageElementProperties(DomImageElement element, JsonObject obj, bool checkDefaultValue = false)
        {
            if (element == null)
            {
                return;
            }
            string src = null;
            if (element.Image != null)
            {
                src = element.Image.ImageDataBase64String;
            }
            if (src != null || checkDefaultValue == false)
                obj.Add("Src", src);
            if (element.KeepWidthHeightRate != true || checkDefaultValue == false)
                obj.Add("KeepWidthHeightRate", element.KeepWidthHeightRate);
            if (element.VAlign != VerticalAlignStyle.Bottom || checkDefaultValue == false)
                obj.Add("VAlign", element.VAlign.ToString());
            if (element.SmoothZoom != true || checkDefaultValue == false)
                obj.Add("SmoothZoom", element.SmoothZoom);
        }

        /// /////////////////////////////////////////////////////////////////////////////////////////////////



        private bool command_insertpageinfoelement(bool showUI, object parameter)
        {
            DomPageInfoElement info = new DomPageInfoElement();
            info.Style = this._Control.CurrentStyle;
            if (parameter != null && parameter is JsonElement)
            {
                innerSetElementProperties(info, (JsonElement)parameter, true);
            }

            //自动修正重复的ID
            if (parameter != null)
            {
                bool AutoMaticCorrectionRepeatID = GetAutoMaticCorrectionRepeatID((JsonElement)parameter);
                if (AutoMaticCorrectionRepeatID)
                {
                    if (info.ID != null && info.ID != "" && info.ID.Length > 0)
                    {
                        MaticCorrectionRepeatID(info);
                    }
                }
            }

            info.OwnerDocument = this._Control.Document;
            object obj = this._Control.ExecuteCommand(
                DCSoft.Writer.StandardCommandNames.InsertPageInfo,
                showUI,
                info);
            bool result = obj != null;
            return result;
        }
        private bool SetPageInfoProperties(DomPageInfoElement element, JsonElement parameter, XTextDocumentUndoList undolist = null)
        {
            bool logUndo = this._Control.DocumentOptions.BehaviorOptions.EnableLogUndo &&
                this._Control.Document.CanLogUndo &&
                undolist != null;


            bool result = false;
            if (element == null || parameter.ValueKind != JsonValueKind.Object)
            {
                return result;
            }
            foreach (JsonProperty property in parameter.EnumerateObject())
            {
                string name = property.Name.ToLower();
                switch (name)
                {
                    case "valuetype":
                        PageInfoValueType oldvalue1 = element.ValueType;
                        element.ValueType = WASMUtils.ConvertToEnum<PageInfoValueType>(property, PageInfoValueType.PageIndex);//  WASMJsonConvert.parseEnumValue<PageInfoValueType>(value, PageInfoValueType.PageIndex);
                        if (logUndo && element.ValueType != oldvalue1)
                        {
                            undolist.AddProperty("ValueType", oldvalue1, element.ValueType, element);
                        }
                        result = true;
                        break;
                    default:
                        break;
                }
            }
            //if(setheight == false)
            //{
            //    element.AutoHeight = true;
            //}
            if (element.OwnerDocument == null)
            {
                element.OwnerDocument = this._Control.Document;
            }
            //element.EditorRefreshView();
            return result;
        }
        private void GetPageInfoProperties(DomPageInfoElement element, JsonObject obj, bool checkDefaultValue = false)
        {
            if (element == null)
            {
                return;
            }
            if (element.ValueType != PageInfoValueType.PageIndex || checkDefaultValue == false)
                obj.Add("ValueType", element.ValueType.ToString());
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////


        /// <summary>
        /// 兼容第四代插入表格
        /// 例如：var parameter = {  'RowCount': rows, 'ColumnCount': cols,  'TableID': 'img_box'  };
        /// 例如：ctl.DCExecuteCommand('InsertTable', true, '3,5');
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        private bool command_InsertTable(object parameter)
        {
            if (parameter == null)
            {
                return false;
            }
            JsonElement parameter2 = (JsonElement)parameter;
            if (parameter2.ValueKind == JsonValueKind.String)
            {
                string value = parameter2.GetString();
                if (value != null && value != "" && value.Contains(",") && value.Length > 2)
                {
                    var value2 = value.Split(',');
                    if (value2 != null && value2.Length == 2)
                    {
                        string v1 = value2[0];
                        string v2 = value2[1];

                        int iRowCount = 0;
                        int iColumnCount = 0;
                        bool result1 = int.TryParse(v1, out iRowCount);
                        bool result2 = int.TryParse(v2, out iColumnCount);
                        if (result1 && result2 && iRowCount > 0 && iColumnCount > 0)
                        {
                            XTextTableElementProperties properties = new XTextTableElementProperties();
                            properties.ID = "";
                            properties.RowsCount = iRowCount;
                            properties.ColumnsCount = iColumnCount;

                            this._Control.ExecuteCommand("Table_InsertTable", false, properties);
                            return true;
                        }
                    }
                }
            }
            else
            {
                string tableID = "";
                int RowCount = 0;
                int ColumnCount = 0;
                foreach (System.Text.Json.JsonProperty property in parameter2.EnumerateObject())
                {
                    string name = property.Name.ToLower();
                    string value = WASMJsonConvert.parseJsonElementToString(property.Value);
                    int i = int.MinValue;
                    //float d = 0f;
                    switch (name)
                    {
                        case "tableid":
                            tableID = value;
                            break;
                        case "rowcount":
                            if (int.TryParse(value, out i) == true)
                            {
                                RowCount = i;
                            }
                            break;
                        case "columncount":
                            if (int.TryParse(value, out i) == true)
                            {
                                ColumnCount = i;
                            }
                            break;
                        //case "Alignment":
                        //    if (int.TryParse(value, out i) == true)
                        //    {
                        //        ColumnCount = i;
                        //    }
                        //    break;
                        default:
                            break;
                    }
                }
                if (RowCount > 0 && ColumnCount > 0)
                {
                    XTextTableElementProperties properties = new XTextTableElementProperties();
                    properties.ID = tableID;
                    properties.RowsCount = RowCount;
                    properties.ColumnsCount = ColumnCount;

                    //自动修正重复的ID
                    bool AutoMaticCorrectionRepeatID = GetAutoMaticCorrectionRepeatID((JsonElement)parameter);
                    if (AutoMaticCorrectionRepeatID)
                    {
                        if (tableID != null && tableID.Length > 0)
                        {
                            DomTableElement table = new DomTableElement();
                            table.ID = tableID;
                            MaticCorrectionRepeatID(table);
                        }
                    }
                    this._Control.ExecuteCommand("Table_InsertTable", false, properties);
                    return true;
                }
            }
            return false;
        }

        /// wyc20230626补充表格属性设置接口
        private bool SetTableElementProperties(DomTableElement element, JsonElement parameter, XTextDocumentUndoList undolist = null)
        {
            bool logUndo = this._Control.DocumentOptions.BehaviorOptions.EnableLogUndo &&
                this._Control.Document.CanLogUndo &&
                undolist != null;

            bool result = false;
            if (element == null || parameter.ValueKind != JsonValueKind.Object)
            {
                return result;
            }

            bool oldvalueb = false;

            foreach (JsonProperty property in parameter.EnumerateObject())
            {
                string name = property.Name.ToLower();
                //string value = WASMJsonConvert.parseJsonElementToString(property.Value);
                //float f = float.NaN;
                switch (name)
                {
                    case "columnswidth":
                        if (property.Value.ValueKind == JsonValueKind.Array)
                        {
                            int index = 0;
                            foreach (JsonElement widthele in property.Value.EnumerateArray())
                            {
                                if (widthele.ValueKind == JsonValueKind.Number)
                                {
                                    float f = widthele.GetSingle();
                                    if (element.Columns[index].Width != f)
                                    {
                                        element.Columns[index].Width = f;
                                        result = true;
                                    }
                                }
                                if (index == element.Columns.Count - 1)
                                {
                                    break;
                                }
                                else
                                {
                                    index++;
                                }
                            }
                        }
                        break;
                    case "rowsheight":
                        if (property.Value.ValueKind == JsonValueKind.Array)
                        {
                            int index = 0;
                            foreach (JsonElement widthele in property.Value.EnumerateArray())
                            {
                                if (widthele.ValueKind == JsonValueKind.Number)
                                {
                                    DomTableRowElement row = element.Rows[index] as DomTableRowElement;
                                    float f = widthele.GetSingle();
                                    if (row.SpecifyHeight != f)
                                    {
                                        row.SpecifyHeight = f;
                                        result = true;
                                    }
                                }
                                if (index == element.Rows.Count - 1)
                                {
                                    break;
                                }
                                else
                                {
                                    index++;
                                }
                            }
                        }
                        break;

                    default:
                        break;
                }
            }
            if (element.OwnerDocument == null)
            {
                element.OwnerDocument = this._Control.Document;
            }
            //element.EditorRefreshView();
            return result;
        }
        private void GetTableElementProperties(DomTableElement element, JsonObject obj, bool checkDefaultValue = false)
        {
            if (element == null)
            {
                return;
            }
            if (checkDefaultValue == false)
            {
                JsonArray cellArray = new JsonArray();
                foreach (DomTableCellElement cell in element.Cells)
                {
                    cellArray.Add(this.GetElementNativeHandle(cell));
                    //cellArray.Add(DotNetObjectReference.Create<XTextElement>(cell));
                }
                obj.Add("Cells", cellArray);
            }

            JsonArray arr = new JsonArray();
            foreach (DomTableColumnElement col in element.Columns)
            {
                arr.Add(col.Width);
            }
            obj.Add("ColumnsWidth", arr);

            JsonArray arr2 = new JsonArray();
            foreach (DomTableRowElement row in element.Rows)
            {
                arr2.Add(row.Height);
            }
            obj.Add("RowsHeight", arr2);


            //wyc20230706:输出些只读属性
            obj.Add("RowsCount", element.RowsCount);
            obj.Add("ColumnsCount", element.ColumnsCount);
            JsonArray rowArray = new JsonArray();
            foreach (DomTableRowElement row in element.Rows)
            {
                rowArray.Add(this.GetElementNativeHandle(row));
            }
            obj.Add("Rows", rowArray);



            JsonArray columnArray = new JsonArray();
            foreach (DomTableColumnElement column in element.Columns)
            {
                columnArray.Add(this.GetElementNativeHandle(column));
                //columnArray.Add(DotNetObjectReference.Create<XTextElement>(column));
            }
            obj.Add("Columns", columnArray);
        }
        private bool SetTableCellElementProperties(DomTableCellElement element, JsonElement parameter, XTextDocumentUndoList undolist = null)
        {
            bool logUndo = this._Control.DocumentOptions.BehaviorOptions.EnableLogUndo &&
                this._Control.Document.CanLogUndo &&
                undolist != null;

            if (element.OwnerDocument == null)
            {
                element.OwnerDocument = this._Control.Document;
            }

            bool result = false;
            if (parameter.ValueKind != JsonValueKind.Object)
            {
                return result;
            }
            foreach (JsonProperty property in parameter.EnumerateObject())
            {
                string name = property.Name.ToLower();
                switch (name)
                {
                    case "rowspanfast":
                        {
                            var intOldValue = element.RowSpan;
                            var intNewValue = property.ConvertToInt32(intOldValue);
                            if (intOldValue != intNewValue)
                            {
                                element.InternalSetRowSpan(intNewValue);
                                if (logUndo)
                                {
                                    undolist.AddProperty("RowSpan", intOldValue, intNewValue, element);
                                }
                                result = true;
                            }
                        }
                        break;
                    case "colspanfast":
                        {
                            var intOldValue = element.ColSpan;
                            var intNewValue = property.ConvertToInt32(intOldValue);
                            if (intOldValue != intNewValue)
                            {
                                element.InternalSetColSpan(intNewValue);
                                if (logUndo)
                                {
                                    undolist.AddProperty("ColSpan", intOldValue, intNewValue, element);
                                }
                                result = true;
                            }
                        }
                        break;
                    case "colspan":
                        var intOldValue2 = element.ColSpan;
                        var intNewValue2 = property.ConvertToInt32(intOldValue2);
                        if (intOldValue2 != intNewValue2)
                        {
                            element.InternalSetColSpan(intNewValue2);
                            if (logUndo)
                            {
                                undolist.AddProperty("ColSpan", intOldValue2, intNewValue2, element);
                            }
                            result = true;
                        }
                        break;
                    case "rowspan":
                        var intOldValue3 = element.RowSpan;
                        var intNewValue3 = property.ConvertToInt32(intOldValue3);
                        if (intOldValue3 != intNewValue3)
                        {
                            element.InternalSetRowSpan(intNewValue3);
                            if (logUndo)
                            {
                                undolist.AddProperty("RowSpan", intOldValue3, intNewValue3, element);
                            }
                            result = true;
                        }
                        break;
                    default:
                        break;
                }
            }
            return result;
        }
        private void GetTableCellElementProperties(DomTableCellElement element, JsonObject obj, bool checkDefaultValue = false)
        {
            if (element == null)
            {
                return;
            }
            if (element.ColSpan != 1 || checkDefaultValue == false)
                obj.Add("ColSpan", element.ColSpan);
            if (element.RowSpan != 1 || checkDefaultValue == false)
                obj.Add("RowSpan", element.RowSpan);
            if (element.CellID != String.Empty || checkDefaultValue == false)
                obj.Add("CellID", element.CellID);
            if (element.IsOverrided != false || checkDefaultValue == false)
                obj.Add("IsOverrided", element.IsOverrided);

            obj.Add("RowIndex", element.RowIndex);
            obj.Add("ColIndex", element.ColIndex);
            //JsonArray arrtable = new JsonArray();
            //arrtable.Add(DotNetObjectReference.Create<XTextElement>(element.OwnerTable));
            // obj.Add("OwnerTable", this.GetElementNativeHandle(element.OwnerTable));

            //JsonArray arrcolumn = new JsonArray();
            //arrcolumn.Add(DotNetObjectReference.Create<XTextElement>(element.OwnerColumn));
            obj.Add("OwnerColumn", this.GetElementNativeHandle(element.OwnerColumn));

            //JsonArray arrrow = new JsonArray();
            //arrrow.Add(DotNetObjectReference.Create<XTextElement>(element.OwnerRow));
            obj.Add("OwnerRow", this.GetElementNativeHandle(element.OwnerRow));

        }
        private bool SetTableRowElementProperties(DomTableRowElement element, JsonElement parameter, XTextDocumentUndoList undolist = null)
        {
            bool logUndo = this._Control.DocumentOptions.BehaviorOptions.EnableLogUndo &&
                this._Control.Document.CanLogUndo &&
                undolist != null;

            bool result = false;
            if (element == null || parameter.ValueKind != JsonValueKind.Object)
            {
                return result;
            }

            bool oldvalueb = false;
            float oldvaluef = 0f;

            foreach (JsonProperty property in parameter.EnumerateObject())
            {
                string name = property.Name.ToLower();
                switch (name)
                {
                    case "headerstyle":
                        oldvalueb = element.HeaderStyle;
                        element.HeaderStyle = WASMUtils.ConvertToBoolean(property, false);// WASMJsonConvert.parseBoolean(value, false);
                        if (logUndo && element.HeaderStyle != oldvalueb)
                        {
                            undolist.AddProperty("HeaderStyle", oldvalueb, element.HeaderStyle, element);
                        }
                        result = true;
                        break;
                    case "newpage":
                        oldvalueb = element.NewPage;
                        element.NewPage = WASMUtils.ConvertToBoolean(property, false);// WASMJsonConvert.parseBoolean(value, false);
                        if (logUndo && element.NewPage != oldvalueb)
                        {
                            undolist.AddProperty("NewPage", oldvalueb, element.NewPage, element);
                        }
                        result = true;
                        break;
                    case "specifyheight":
                        float f = WASMUtils.ConvertToSingle(property, float.MinValue);
                        if (f != float.MinValue)
                        {
                            oldvaluef = element.SpecifyHeight;
                            element.SpecifyHeight = f;
                            if (logUndo && element.SpecifyHeight != oldvaluef)
                            {
                                undolist.AddProperty("SpecifyHeight", oldvaluef, element.SpecifyHeight, element);
                            }
                            result = true;
                        }
                        break;
                    case "cansplitbypageline":
                        oldvalueb = element.CanSplitByPageLine;
                        element.CanSplitByPageLine = WASMUtils.ConvertToBoolean(property, true);
                        if (logUndo && element.CanSplitByPageLine != oldvalueb)
                        {
                            undolist.AddProperty("CanSplitByPageLine", oldvalueb, element.CanSplitByPageLine, element);
                        }
                        result = true;
                        break;
                    default:
                        break;
                }
            }

            return result;
        }
        private void GetTableRowElementProperties(DomTableRowElement element, JsonObject obj, bool checkDefaultValue = false)
        {
            if (element == null)
            {
                return;
            }
            if (element.HeaderStyle != false || checkDefaultValue == false)
                obj.Add("HeaderStyle", element.HeaderStyle);
            if (element.NewPage != false || checkDefaultValue == false)
                obj.Add("NewPage", element.NewPage);
            if (element.SpecifyHeight != 0f || checkDefaultValue == false)
                obj.Add("SpecifyHeight", element.SpecifyHeight);
            if (element.CanSplitByPageLine != true || checkDefaultValue == false)
                obj.Add("CanSplitByPageLine", element.CanSplitByPageLine);
            //输出点只读属性
            obj.Add("RowIndex", element.RowIndex);
            //wyc20230706:
            obj.Add("CellsCount", element.CellsCount);
            JsonArray cellArr = new JsonArray();
            foreach (DomTableCellElement cell in element.Cells)
            {
                cellArr.Add(this.GetElementNativeHandle(cell));
                //cellArr.Add(DotNetObjectReference.Create<XTextElement>(cell));
            }
            obj.Add("Cells", cellArr);
            //JsonArray arrtable = new JsonArray();
            //arrtable.Add(DotNetObjectReference.Create<XTextElement>(element.OwnerTable));
            //obj.Add("OwnerTable", this.GetElementNativeHandle(element.OwnerTable));
        }
        private bool SetTableColumnElementProperties(DomTableColumnElement element, JsonElement parameter, XTextDocumentUndoList undolist = null)
        {
            bool logUndo = this._Control.DocumentOptions.BehaviorOptions.EnableLogUndo &&
                this._Control.Document.CanLogUndo &&
                undolist != null;

            bool result = false;
            if (element == null || parameter.ValueKind != JsonValueKind.Object)
            {
                return result;
            }
            foreach (JsonProperty property in parameter.EnumerateObject())
            {
                string name = property.Name.ToLower();
                switch (name)
                {
                    default:
                        break;
                }
            }
            return result;
        }
        private void GetTableColumnElementProperties(DomTableColumnElement element, JsonObject obj, bool checkDefaultValue = false)
        {
            if (element == null)
            {
                return;
            }
            //输出点只读属性
            obj.Add("Index", element.Index);
            JsonArray cellArr = new JsonArray();
            foreach (DomTableCellElement cell in element.Cells)
            {
                cellArr.Add(this.GetElementNativeHandle(cell));
                //cellArr.Add(DotNetObjectReference.Create<XTextElement>(cell));
            }
            obj.Add("Cells", cellArr);
        }

        private bool SetXTextDocumentProperties(DomDocument element, JsonElement parameter, XTextDocumentUndoList undolist = null)
        {
            bool logUndo = this._Control.DocumentOptions.BehaviorOptions.EnableLogUndo &&
                this._Control.Document.CanLogUndo &&
                undolist != null;

            bool result = false;
            if (parameter.ValueKind == JsonValueKind.Object)
            {
                foreach (JsonProperty property in parameter.EnumerateObject())
                {
                    string name = property.Name.ToLower();
                    string value = WASMJsonConvert.parseJsonElementToString(property.Value);
                    switch (name)
                    {
                        default:
                            break;
                    }
                }
            }
            return result;
        }
        private void GetXTextDocumentProperties(DomDocument doc, JsonObject obj, bool smartMode = false)
        {
            if (obj == null)
            {
                obj = new JsonObject();
            }
            //wyc20240628:对文档对象获取属性添加样式列表
            if (doc.ContentStyles != null && doc.ContentStyles.Styles != null)
            {
                JsonArray stylearr = new JsonArray();
                foreach (DocumentContentStyle style in doc.ContentStyles.Styles)
                {
                    stylearr.Add(WASMJsonConvert.DocumentContentStyleToJSON(style));
                }
                obj.Add("ContentStyles", stylearr);
            }
        }
        private bool SetXTextContainerElementProperties(DomContainerElement element, JsonElement parameter, XTextDocumentUndoList undolist = null)
        {
            //记录撤销
            bool logUndo = this._Control.DocumentOptions.BehaviorOptions.EnableLogUndo &&
                this._Control.Document.CanLogUndo &&
                undolist != null;

            string oldvalues = null;
            bool oldvalueb = false;

            bool result = false;
            if (parameter.ValueKind == JsonValueKind.Object)
            {
                foreach (JsonProperty property in parameter.EnumerateObject())
                {
                    string name = property.Name.ToLower();
                    switch (name)
                    {
                        case "accepttab":
                            oldvalueb = element.AcceptTab;
                            element.AcceptTab = WASMUtils.ConvertToBoolean(property, true);// WASMJsonConvert.parseBoolean(property.Value, true);
                            if (logUndo && element.AcceptTab != oldvalueb)
                            {
                                undolist.AddProperty("AcceptTab", oldvalueb, element.AcceptTab, element);
                            }
                            result = true;
                            break;
                        case "tooltip":
                            oldvalues = element.ToolTip;
                            element.ToolTip = WASMUtils.ConvertToString(property);
                            if (logUndo && element.ToolTip != oldvalues)
                            {
                                undolist.AddProperty("ToolTip", oldvalues, element.ToolTip, element);
                            }
                            result = true;
                            break;
                        case "validatestyle":
                            ValueValidateStyle oldvalueVS = element.ValidateStyle == null ? null : element.ValidateStyle.Clone();
                            element.ValidateStyle = WASMJsonConvert.parseJSONToValidateStyle(property.Value, element.ValidateStyle);
                            if (logUndo && oldvalueVS != null && oldvalueVS.Equals(oldvalueVS) == false)
                            {
                                undolist.AddProperty("ValidateStyle", oldvalueVS, element.ValidateStyle, element);
                            }
                            result = true;
                            break;
                        default:
                            break;
                    }
                }
            }
            return result;
        }
        private void GetXTextContainerElementProperties(DomContainerElement element, JsonObject obj, bool checkDefaultValue = false)
        {
            if (obj == null)
            {
                obj = new JsonObject();
            }
            if (element.AcceptTab != false || checkDefaultValue == false)
                obj.Add("AcceptTab", element.AcceptTab);
            if (element.ToolTip != null || checkDefaultValue == false)
                obj.Add("ToolTip", element.ToolTip);
            if (element.ValidateStyle != null || checkDefaultValue == false)
                obj.Add("ValidateStyle", WASMJsonConvert.parseValidateStyleToJSON(element.ValidateStyle));
            //wyc20230706:输出子元素引用列表
            if (checkDefaultValue == false)
            {
                JsonArray arr = new JsonArray();
                if (element.Elements != null)
                {
                    foreach (DomElement ele2 in element.Elements)
                    {
                        arr.Add(this.GetElementNativeHandle(ele2));
                        //arr.Add(DotNetObjectReference.Create<XTextElement>(ele2));
                    }
                }
                obj.Add("Elements", arr);
            }

        }
        private bool SetXTextObjectElementProperties(DomObjectElement element, JsonElement parameter, XTextDocumentUndoList undolist = null)
        {
            bool logUndo = this._Control.DocumentOptions.BehaviorOptions.EnableLogUndo
                && this._Control.Document.CanLogUndo
                && undolist != null;
            bool result = false;
            if (parameter.ValueKind == JsonValueKind.Object)
            {
                foreach (JsonProperty property in parameter.EnumerateObject())
                {
                    string name = property.Name.ToLower();
                    switch (name)
                    {
                        case "name":
                            {
                                var strNewValue = property.ConvertToStringEmptyAsNull();
                                if (WASMUtils.EqualsUIString(strNewValue, element.Name) == false)
                                {
                                    if (logUndo)
                                    {
                                        undolist.AddProperty("Name", element.Name, strNewValue, element);
                                    }
                                    element.Name = strNewValue;
                                    result = true;
                                }
                            }
                            break;
                        case "tooltip":
                            {
                                var strNewValue = property.ConvertToStringEmptyAsNull();
                                if (WASMUtils.EqualsUIString(strNewValue, element.ToolTip) == false)
                                {
                                    if (logUndo)
                                    {
                                        undolist.AddProperty("ToolTip", element.ToolTip, strNewValue, element);
                                    }
                                    element.ToolTip = strNewValue;
                                    result = true;
                                }
                            }
                            break;
                        default:
                            break;
                    }
                }

            }
            return result;
        }
        private void GetXTextObjectElementProperties(DomObjectElement element, JsonObject obj, bool checkDefaultValue = false)
        {
            if (obj == null)
            {
                obj = new JsonObject();
            }
            if (element.Name != null || checkDefaultValue == false)
                obj.Add("Name", element.Name);
            if (element.ToolTip != null || checkDefaultValue == false)
                obj.Add("ToolTip", element.ToolTip);
        }
        private bool SetXTextElementProperties(DomElement element, JsonElement parameter, bool forinsert, XTextDocumentUndoList undolist = null)
        {
            bool result = false;
            //wyc20230925:所有容器元素均不设置text属性，但非文本类型的输入域元素除外
            bool settext = true;
            if (element is DomContainerElement)
            {
                settext = false;
            }
            if (element is DomInputFieldElement)
            {
                //settext = true;//wyc20250409注释DUWRITER5_0-4317
                DomInputFieldElement input = element as DomInputFieldElement;

                if (input.FieldSettings != null && input.FieldSettings.EditStyle != InputFieldEditStyle.Text)
                {
                    settext = true;
                }

                if (input != null && input.Elements.Count > 0)
                {//当输入域中具有容器元素时，不需要设置text
                    for (int i = 0; i < input.Elements.Count; i++)
                    {
                        DomElement item = input.Elements[i];
                        if (item != null)
                        {
                            if (item is DomContainerElement)
                            {
                                settext = false;
                                break;
                            }
                        }
                    }
                }

            }

            //记录撤销
            bool logUndo = this._Control.DocumentOptions.BehaviorOptions.EnableLogUndo &&
                this._Control.Document.CanLogUndo &&
                undolist != null;
            string oldstr = null;
            int oldint = 0;
            float oldfloat = 0f;
            bool oldbool = false;

            if (parameter.ValueKind == JsonValueKind.Object)
            {
                foreach (JsonProperty property in parameter.EnumerateObject())
                {
                    string name = property.Name.ToLower();
                    //string value = WASMJsonConvert.parseJsonElementToString(property.Value);
                    float f = float.NaN;
                    switch (name)
                    {
                        case "id":
                            oldstr = element.ID;
                            element.ID = property.ConvertToString();
                            if (logUndo && oldstr != element.ID)
                            {
                                undolist.AddProperty("ID", oldstr, element.ID, element);
                            }
                            result = true;
                            break;
                        //wyc20230922:不能设置TEXT，否则对容器元素获取一整个属性列表再写回去会覆盖元素内部属性
                        //wyc20230925:新增开关
                        case "text":
                            if (settext == true || forinsert == true)
                            {
                                oldstr = element.Text;
                                //wyc20241231:输入域TEXT赋值会覆盖INNERVALUE,换成不触发表达式的写法并手动触发表达式DUWRITER5_0-4072
                                if (element is DomInputFieldElement)
                                {
                                    DomInputFieldElement field = element as DomInputFieldElement;
                                    string innervalue = field.InnerValue;
                                    field.SetInnerTextFast(property.ConvertToString());
                                    field.InnerValue = innervalue;
                                }
                                else
                                /////////////////////////////////////////////////////////////////////////////////
                                {
                                    element.Text = property.ConvertToString();
                                }

                                if (logUndo && oldstr != element.Text)
                                {
                                    undolist.AddProperty("Text", oldstr, element.Text, element);
                                }
                                result = true;
                            }
                            break;
                        case "innertext":
                            if (settext == true || forinsert == true)
                            {
                                oldstr = element.InnerText;
                                element.InnerText = property.ConvertToString();
                                if (logUndo && oldstr != element.InnerText)
                                {
                                    undolist.AddProperty("InnerText", oldstr, element.InnerText, element);
                                }
                                result = true;
                            }
                            break;
                        case "modified":
                            oldbool = element.Modified;
                            WASMUtils.SetContainerElementModified(element, property.ConvertToBoolean(element.Modified));
                            //element.Modified =  property.ConvertToBoolean(element.Modified);
                            if (logUndo && oldbool != element.Modified)
                            {
                                undolist.AddProperty("Modified", oldbool, element.Modified, element);
                            }
                            result = true;
                            break;
                        case "width":

                            if (property.Value.ValueKind == JsonValueKind.Number)
                            {
                                oldfloat = element.Width;
                                element.Width = property.Value.GetSingle();
                                result = true;
                            }
                            else
                            {
                                string ss = property.ConvertToString();
                                GraphicsUnit oldUnit = GraphicsUnit.Document;
                                if (ss.EndsWith("px") == true && ss.Length >= 3)
                                {
                                    oldUnit = GraphicsUnit.Pixel;
                                    ss = ss.Remove(ss.Length - 2);
                                }
                                if (float.TryParse(ss, out f) == true)
                                {
                                    if (oldUnit == GraphicsUnit.Pixel)
                                    {
                                        f = GraphicsUnitConvert.Convert(f, oldUnit, GraphicsUnit.Document);
                                    }
                                    oldfloat = element.Width;
                                    element.Width = f;
                                    result = true;
                                }
                            }
                            if (logUndo && oldfloat != element.Width)
                            {
                                undolist.AddProperty("Width", oldfloat, element.Width, element);
                            }
                            break;
                        case "height":
                            if (property.Value.ValueKind == JsonValueKind.Number)
                            {
                                oldfloat = element.Height;
                                element.Height = property.Value.GetSingle();
                                result = true;
                            }
                            else
                            {
                                string ss = property.ConvertToString();
                                GraphicsUnit oldUnit = GraphicsUnit.Document;
                                if (ss.EndsWith("px") == true && ss.Length >= 3)
                                {
                                    oldUnit = GraphicsUnit.Pixel;
                                    ss = ss.Remove(ss.Length - 2);
                                }
                                if (float.TryParse(ss, out f) == true)
                                {
                                    if (oldUnit == GraphicsUnit.Pixel)
                                    {
                                        f = GraphicsUnitConvert.Convert(f, oldUnit, GraphicsUnit.Document);
                                    }
                                    oldfloat = element.Height;
                                    element.Height = f;
                                    result = true;
                                }
                            }
                            if (logUndo && oldfloat != element.Height)
                            {
                                undolist.AddProperty("Height", oldfloat, element.Height, element);
                            }
                            break;
                        case "style":
                            oldint = element.StyleIndex;
                            bool changed = false;
                            DocumentContentStyle newstyle = WASMJsonConvert.JSONToDocumentContentStyle(property.Value, element.Style, out changed);
                            if (changed == true)
                            {
                                element.Style = newstyle;
                                element.CommitStyle(logUndo);
                            }
                            //if (logUndo)
                            //{
                            //    element.OwnerDocument?.UndoList.AddProperty("Style", oldvalue, element.Style, element);
                            //}
                            result = true;
                            break;
                        default:
                            break;
                    }
                }
                if (element.OwnerDocument == null)
                {
                    element.OwnerDocument = this._Control.Document;
                }
                //element.EditorRefreshView();
            }
            return result;
        }
        private void GetXTextElementProperties(DomElement element, JsonObject obj, bool checkDefaultValue = false)
        {
            if (obj == null)
            {
                obj = new JsonObject();
            }

            if (element.ID != null || checkDefaultValue == false)
                obj.Add("ID", element.ID);
            if (element.Width != 0 || checkDefaultValue == false)
                obj.Add("Width", element.Width);
            if (element.Height != 0 || checkDefaultValue == false)
                obj.Add("Height", element.Height);
            if (element.Visible != true || checkDefaultValue == false)
                obj.Add("Visible", element.Visible);
            if ((element is DomTableRowElement) == false && (element is DomTableElement) == false)// yyf2025-3-26 避免表格行的文本操作，减少内存占用
            {
                if (checkDefaultValue)
                {
                    var txt88 = element.InnerText;
                    if (string.IsNullOrEmpty(txt88) == false)
                    {
                        obj.Add("InnerText", txt88);
                    }
                }
                else
                {
                    obj.Add("InnerText", element.InnerText);
                }
            }
            if (element.Modified != false || checkDefaultValue == false)
                obj.Add("Modified", element.Modified);
            //wyc20230703第四代接口中用text属性表示选框的文本，在此先避开
            if ((element is DomCheckBoxElementBase) == false
                && (element is DomTableRowElement) == false)// yyf2025-3-26 避免表格行的文本操作，减少内存占用
            {
                if (element.Text != null || checkDefaultValue == false)
                    obj.Add("Text", element.Text);
            }
            obj.Add("Style", WASMJsonConvert.DocumentContentStyleToJSON(element.Style));
            obj.Add("OwnerPageIndex", element.OwnerPageIndex);
            obj.Add("OwnerLastPageIndex", element.OwnerLastPageIndex);
            obj.Add("Left", element.Left);
            obj.Add("AbsLeft", element.GetAbsLeft());
            obj.Add("AbsTop", element.GetAbsTop());
            obj.Add("Top", element.Top);
            obj.Add("TopInOwnerPage", element.TopInOwnerPage);
            obj.Add("LeftInOwnerPage", element.LeftInOwnerPage);
            obj.Add("TypeName", element.TypeName);
            //wyc20230714:新增父元素引用输出
            if (element.Parent != null)
            {
                //JsonArray arr = new JsonArray();
                //arr.Add(DotNetObjectReference.Create<XTextElement>(element.Parent));
                ////JsonNode node = arr[0];
                obj.Add("Parent", this.GetElementNativeHandle(element.Parent));
            }

            ////wyc20230710:新增输出属性
            //obj.Add("InnerXML", element.InnerXML);
            //obj.Add("OuterXML", element.OuterXML);

            //wyc20230810:添加元素的内部句柄方便前端引用
            obj.Add("NativeHandle", this.GetElementNativeHandle(element));

            //wyc20231216:添加元素所属父元素
            if (element.OwnerCell != null || checkDefaultValue == false)
                obj.Add("OwnerCell", this.GetElementNativeHandle(element.OwnerCell));
            if (element.OwnerTable != null || checkDefaultValue == false)
                obj.Add("OwnerTable", this.GetElementNativeHandle(element.OwnerTable));

            //wyc20240109:
            obj.Add("PreviousElement", this.GetElementNativeHandle(element.PreviousElement));
            obj.Add("NextElement", this.GetElementNativeHandle(element.NextElement));
            //wyc20240709:
            //if (element.OwnerLine != null)
            //{
            //    JsonObject lineobj = new JsonObject();
            //    lineobj.Add("Bottom", element.OwnerLine.Bottom);
            //    lineobj.Add("Top", element.OwnerLine.Top);
            //    lineobj.Add("GlobalIndex", element.OwnerLine.GlobalIndex);
            //    lineobj.Add("Height", element.OwnerLine.Height);
            //    lineobj.Add("IndexInPage", element.OwnerLine.IndexInPage);
            //    obj.Add("OwnerLine", lineobj);
            //}

            //wyc20241112: 新增
            if (element.StyleIndex != -1 || checkDefaultValue == false)
                obj.Add("StyleIndex", element.StyleIndex);
        }
    }

}
