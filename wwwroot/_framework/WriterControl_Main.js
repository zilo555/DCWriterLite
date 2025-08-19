"use strict";

import { WriterControl_UI } from "./WriterControl_UI.js";
import { WriterControl_Paint } from "./WriterControl_Paint.js";
import { DCTools20221228 } from "./DCTools20221228.js";
import { WriterControl_API } from "./WriterControl_API.js";
import { WriterControl_Task } from "./WriterControl_Task.js";
import { WriterControl_Rule } from "./WriterControl_Rule.js";
import { WriterControl_Event } from "./WriterControl_Event.js";
import { WriterControl_Dialog } from "./WriterControl_Dialog.js";
import { WriterControl_DOMPackage } from "./WriterControl_DOMPackage.js";
import { WriterControl_DateTimeControl } from "./WriterControl_DateTimeControl.js";
import { WriterControl_ToolBar } from "./WriterControl_ToolBar.js";

export let WriterControl_Main = {

    /**初始化默认的字符串资源 */
    InitDefaultResourceStrings: function () {
        /**
        * 设置字符串资源值的全局函数
        * @param {Object} json 字符串对象
        */
        window.__SetDCStringResourceValues = function (json) {
            DotNet.invokeMethod(
                window.DCWriterEntryPointAssemblyName,
                "SetDCSRValues",
                json);
        };
        // 定义标准的字符串资源
        /*window.__DCSR = {
            "NotSupportedFileFormat": "不支持的文件格式:",
            "MillisecondsForLoadDocument": "加载文档耗时毫秒数:",
            "NotSupportedXmlNode": "不支持的XML节点:",
            "LoadFontData": "加载字体信息: ",
            "ControlDisposed_ID": "DCWriter控件{0}被销毁了.",
            "BeginLoadWriterControlForWASM": "开始加载 WriterControlForWASM：",
            "EndLoadWriterControlForWASM": "WriterControlForWASM 初始化耗时",
            "Waitting": "请稍候...",
            "AllFileFilter": "所有文件|*.*",
            "ArgumentOutOfRange_Name_Value_Max_Min": "参数'{0}'值为'{1}'，超出范围，最大值'{2}'，最小值'{3}'。",
            "ButtonNewText": "按钮",
            "By": "由",
            "CheckRequired_Name": "单/复选框组'{0}'必须有勾选项。",
            "ClipboardErrorMessage": " 可能是360等安全软件实时监控系统剪切板所造成的。",
            "DeleteElement_Content": "删除'{0}'",
            "DeleteElements_Count": "删除{0}个元素",
            "EditControlReadonly": "编辑器控件是只读的。",
            "ElementType_Body": "文档正文",
            "ElementType_Char": "字符",
            "ElementType_CheckBox": "复选框",
            "ElementType_Comment": "文档批注",
            "ElementType_Document": "文档",
            "ElementType_Footer": "页脚",
            "ElementType_HL": "水平线",
            "ElementType_Header": "页眉",
            "ElementType_Image": "图片",
            "ElementType_InputField": "输入域",
            "ElementType_Label": "文本标签",
            "ElementType_LineBreak": "断行符",
            "ElementType_PageBreak": "分页符",
            "ElementType_PageInfo": "页码",
            "ElementType_ParagraphFlag": "段落符号",
            "ElementType_RadioBox": "单选框",
            "ElementType_Table": "表格",
            "ElementType_TableCell": "单元格",
            "ElementType_TableColumn": "表格列",
            "ElementType_TableRow": "表格行",
            "FailToAcceptChildElement_Parent_Child": "容器元素{0}不能接受子元素{0}。",
            "FixLayoutForPrint": "文档处于打印或打印预览时无法修改文档内容排版。",
            "Footer": "页脚",
            "ForbidEmpty": "数据不能为空。",
            "Header": "页眉",
            "IDRepeat_ID": "发现重复的元素ID值'{0}'。",
            "InsertElement_Content": "插入'{0}'",
            "InsertElements_Count": "插入{0}个元素",
            "InvalidateCommandName_Name_SimiliarNames": "错误的命令'{0}'，系统支持类似的命令有'{1}'",
            "InvalidatePageSettings": "无效的页面设置，请仔细调整文档页面设置。",
            "Items_Count": "有{0}个项目。",
            "LabelNewText": "标签文本",
            "LessThanMinLength_Length": "文本过短，不得少于 {0} 个字符。",
            "LessThanMinValue_Value": "小于最小值 {0}。",
            "LineInfo_PageIndex_LineIndex_ColumnIndex": "第{0}页 第{1}行 第{2}列。",
            "LoadComplete_Size": "加载完成，共加载了{0}。",
            "Loading_FileName_Format": "正在以 {1} 格式加载文件'{0}'...",
            "MissProperty_Name": "没能找到属性'{0}'。",
            "MoreThanMaxDemicalDigits": "小数位数超过上限，上限为{0}。",
            "MoreThanMaxLength_Length": "文本过长，不得超过 {0} 个字符。",
            "MoreThanMaxValue_Value": "超过最大值 {0}。",
            "MustDateTimeType": "必须为日期时间格式。",
            "MustDateType": "必须为日期格式。",
            "MustInteger": "必须为整数。",
            "MustNumeric": "必须为数值。",
            "MustTimeType": "必须为时间格式。",
            "NeedSetOwnerDocument": "需要首先设置OwnerDocument属性值。",
            "NoDocument": "无文档。",
            "NoImage": "没有图片",
            "NotSupportInThisVersion_Name": "当前版本不支持功能'{0}'。",
            "NotSupportSerializeText_Format": "不支持以纯文本格式存储为'{0}'文件格式。",
            "NotSupportSerialize_Format": "不支持以'{0}'格式进行存储。",
            "NotSupportWrite_Format": "不支持保存'{0}'格式的文件。",
            "OwnerDocumentNUll": "文档元素尚未属于某个文档，无法执行操作。",
            "PageBottomMargin": "下页边距",
            "PageBreak": "分页符",
            "PageLeftMargin": "左页边距",
            "PageRightMargin": "右页边距",
            "PageStateLocked": "当前文档的分页状态被锁定，无法执行重新分页操作。请不要此时调用RefreshPages()/RefreshDocument()/UpdateDocumentView()/EditorRefreshView()等容易导致重新分页的函数。",
            "PageTopMargin": "上页边距",
            "ParagraphFirstLineIndent": "首行缩进",
            "ParagraphLeftIndent": "左缩进",
            "PromptDisableOSClipboardData": "程序禁止从外部获得数据。",
            "PromptJumpStartForSearch": "已到达文档的结尾处，是否继续从开始处搜索？",
            "PromptMaxTextLengthForPaste_Length": "系统设置为粘贴或插入内容时不能接收超过{0}个字符。",
            "PromptProtectedContent": "有内容受到保护，操作受到限制或无法执行。",
            "PromptRejectFormat_Format": "系统被设定为拒绝'{0}'格式的数据。",
            "PropertyCannotHasParameter_Name": "属性'{0}'不能有参数。",
            "PropertyIsReadonly_Name": "属性'{0}'是只读的。",
            "RTFFileFilter": "RTF文件(*.rtf)|*.rtf",
            "ReadonlyCanNotDeleteBackgroundText": "不能删除输入域的背景文本。",
            "ReadonlyCanNotDeleteBorderElement": "不能删除输入域边界元素。",
            "ReadonlyCanNotDeleteLastParagraphFlag": "任何时候都不能删除最后一个段落符号。",
            "ReadonlyCannotAcceptElementType_ParentType_ChildType": "{0}类型的元素不能接受{1}类型的子元素。",
            "ReadonlyContainerReadonly": "容器元素设置为内容只读。",
            "ReadonlyContentProtect": "由于强制设置了内容保护而导致内容只读。",
            "ReadonlyInputFieldUserEditable_ID": "输入域[{0}]的内容设置为不能直接修改。",
            "RowExistInTable": "表格行已经在表格中了。",
            "SystemAlert": "系统提示",
            "TXTFileFilter": "TXT文件(*.txt)|*.txt",
            "ValueInvalidate": "数据校验错误",
            "ValueInvalidate_Source_Value_Result": "对象'{0}'内容为'{1}'，数据校验错误'{2}'。",
            "ValueValidateFail": "数据校验失败.",
            "ValueValidateOK": "数据校验成功.",
            "WhereToCopy": "复制到何处？",
            "WhereToMove": "移动到何处？",
            "XMLFilter": "XML文件|*.xml"
        };*/
        window.__DCSR = {
            "NotSupportedFileFormat": " Unsupported file format:",
            "MillisecondsForLoadDocument": "load time document milliseconds:",
            "NotSupportedXmlNode": "Unsupported XML node :",
            "LoadFontData": "Load font information:",
            "ControlDisposed_ID": "DCWriter control {0} has been destroyed.",
            "BeginLoadWriterControlForWASM": "start loading WriterControlForWASM:",
            "EndLoadWriterControlForWASM": "WriterControlForWASM initialization time consuming,",
            "Waitting": " Please wait a moment...",
            "AllFileFilter": "All files *.*",
            "ArgumentOutOfRange_Name_Value_Max_Min": "parameters' {0} 'value is' {1}', beyond the scope of the maximum '{2}' minimum '{3}'.",
            "ButtonNewText": "Button",
            "By": "By",
            "CheckRequired_Name": "Single/checkbox group '{0}' must have a checkbox.",
            "ClipboardErrorMessage": "It might be caused by the clipboard of the real-time monitoring system of security software such as 360.",
            "DeleteElement_Content": "Delete '{0}'",
            "DeleteElements_Count": "Delete {0} elements",
            "EditControlReadonly": "The editor control is read-only.",
            "ElementType_Body": "Document Body",
            "ElementType_Char": "Character",
            "ElementType_CheckBox": "Checkbox",
            "ElementType_Comment": "Document Comment",
            "ElementType_Document": "Document",
            "ElementType_Footer": "Footer",
            "ElementType_HL": "Horizontal line",
            "ElementType_Header": "Header",
            "ElementType_Image": "Image",
            "ElementType_InputField": "InputField",
            "ElementType_Label": "Text Label",
            "ElementType_LineBreak": "Line break",
            "ElementType_PageBreak": "Page break",
            "ElementType_PageInfo": "Page Number",
            "Youdaoplaceholder0 ": " paragraph symbols",
            "ElementType_RadioBox": "RadioBox",
            "ElementType_Table": "Table",
            "ElementType_TableCell": "Cell",
            "ElementType_TableColumn": "TableColumn",
            "ElementType_TableRow": "Table Rows",
            "FailToAcceptChildElement_Parent_Child": "Container element {0} cannot accept child element {0}.",
            "FixLayoutForPrint": "The layout of the document content cannot be modified when the document is in print or print preview.",
            "Footer": "footer",
            "ForbidEmpty": "The data cannot be empty.",
            "Header": "Page Header",
            "IDRepeat_ID": "Found duplicate element ID value '{0}'.",
            "InsertElement_Content": "Insert '{0}'",
            "InsertElements_Count": "Insert {0} elements",
            "InvalidateCommandName_Name_SimiliarNames": "Incorrect command '{0}'. The system supports similar commands such as '{1}'.",
            "InvalidatePageSettings": "Invalid page Settings. Please adjust the document page Settings carefully.",
            "Items_Count": "There are {0} items.",
            "LabelNewText": "Label Text",
            "LessThanMinLength_Length": "The text is too short and must not be less than {0} characters.",
            "LessThanMinValue_Value": "Less than the minimum value {0}.",
            "LineInfo_PageIndex_LineIndex_ColumnIndex": "Page {0}, row {1}, column {2}.",
            "LoadComplete_Size": "Loading completed. A total of {0} was loaded.",
            "Loading_FileName_Format": "is loaded in {1} format file '{0}'...",
            "MissProperty_Name": "Property '{0}' was not found.",
            "MoreThanMaxDemicalDigits": "The number of decimal places exceeds the upper limit, which is {0}.",
            "MoreThanMaxLength_Length": "The text is too long and must not exceed {0} characters.",
            "MoreThanMaxValue_Value": "Exceeding the maximum value {0}.",
            "MustDateTimeType": "Must be in date-time format.",
            "MustDateType": "Must be in date format.",
            "MustInteger": "Must be an integer.",
            "MustNumeric": "Must be numeric.",
            "MustTimeType": "Must be in time format.",
            "NeedSetOwnerDocument": "The OwnerDocument property value needs to be set first.",
            "NoDocument": "No document.",
            "NoImage": "No image",
            "NotSupportInThisVersion_Name": "Feature '{0}' is not supported in the current version.",
            "NotSupportSerializeText_Format": "Does not support storing in plain text format as the '{0}' file format.",
            "NotSupportSerialize_Format": "Storage in the '{0}' format is not supported.",
            "NotSupportWrite_Format": "Files in the '{0}' format are not supported for saving.",
            "OwnerDocumentNUll": "The document element does not yet belong to a certain document, and the operation cannot be performed.",
            "PageBottomMargin": "Next page margin",
            "PageBreak": "Page break",
            "PageLeftMargin": "Left margin",
            "PageRightMargin": "Right margin",
            "PageStateLocked": "The pagination status of the current document is locked, and the re-pagination operation cannot be performed.  Please do not call functions such as RefreshPages()/RefreshDocument()/UpdateDocumentView()/EditorRefreshView() that are prone to causing re-pagination at this time. ",
            "PageTopMargin": "Upper margin",
            "Youdaoplaceholder0 ": " first line indentation",
            "Youdaoplaceholder0 ": " left indentation",
            "PromptDisableOSClipboardData": "programs from external data is prohibited.",
            "PromptJumpStartForSearch": "You have reached the end of the document. Do you want to continue searching from the beginning?",
            "PromptMaxTextLengthForPaste_Length": "system Settings for paste or insert content cannot receive more than {0} characters.",
            "PromptProtectedContent": "Content is protected, operations are restricted or cannot be performed.",
            "PromptRejectFormat_Format": "The system is set to reject data in the '{0}' format.",
            "PropertyCannotHasParameter_Name": "attributes' {0} 'cannot have parameters.",
            "PropertyIsReadonly_Name": "the property '{0}' is read-only.",
            "RTFFileFilter": "RTF file (*.rtf) / *.rtf",
            "ReadonlyCanNotDeleteBackgroundText": "can't delete the background text input fields.",
            "ReadonlyCanNotDeleteBorderElement": "can't delete the input domain boundary element.",
            "ReadonlyCanNotDeleteLastParagraphFlag": "at any time can't delete the last paragraph.",
            "ReadonlyCannotAcceptElementType_ParentType_ChildType": "{0} type element cannot accept {1} type of child elements.",
            "ReadonlyContainerReadonly": "container element is set to read-only content.",
            "ReadonlyContentProtect": "The content is read-only due to mandatory content protection Settings.",
            "ReadonlyInputFieldUserEditable_ID": "the content of the input domain ({0}) is set to not directly modify.",
            "RowExistInTable": "The table rows are already in the table.",
            "SystemAlert": "System prompt",
            "TXTFileFilter": "TXT file (*.txt) *.txt",
            "ValueInvalidate": "Data validation error",
            "ValueInvalidate_Source_Value_Result": "The content of the object '{0}' is '{1}', and the data validation error '{2}'.",
            "ValueValidateFail": "Data verification failed.",
            "ValueValidateOK": "Data verification successful.",
            "WhereToCopy": "Where to copy?",
            "WhereToMove": "Where to move?",
            "XMLFilter": "XML file *.xml"
        };
        window.__SetDCStringResourceValues(window.__DCSR);
    },

    /**
     * DCWriter软件C#模块全部加载完毕，然后自动的初始化所有编辑器控件对象
     */
    StartGlobal: function () {
        // 设置默认字体
        DotNet.invokeMethod(window.DCWriterEntryPointAssemblyName, "SetDefaultFont", 30, 128, "Microsoft Sans Serif");
        DotNet.invokeMethod(window.DCWriterEntryPointAssemblyName, "SetDefaultFont", 0xf00, 0xfff, "Microsoft Himalaya");
        DotNet.invokeMethod(window.DCWriterEntryPointAssemblyName, "SetDefaultFont", 19968, 40869, "宋体");
        DotNet.invokeMethod(window.DCWriterEntryPointAssemblyName, "SetDefaultFont", 8731, 8731, "Segoe UI Symbol");

        //DotNet.invokeMethod(window.DCWriterEntryPointAssemblyName, "SetDefaultFont", 32, 65510, "Arial Unicode MS");

        // 设置替换字符
        DotNet.invokeMethod(window.DCWriterEntryPointAssemblyName, "AddReplaceCharsForLoad", 10004/* ✔ */, 8730 /*√ */);

        WriterControl_Paint.RefreshStandardImageList();
        if (window.queryLocalFonts) {
            try {
                // 尝试获取本地字体名称列表
                window.queryLocalFonts().then(function (list2) {
                    if (list2.length > 0) {
                        var localFontNames = new Array();
                        for (const item2 of list2) {
                            if (localFontNames.indexOf(item2.family) == false) {
                                localFontNames.push(item2.family);
                            }
                        }
                        window.__DCLocalFontNames = localFontNames;
                    }
                    else {
                        window.__LocalFontsErrorFlag = true;
                    }
                }).catch((err) => {
                    window.__LocalFontsErrorFlag = true;
                    console.log("Query local font list error:" + err);
                });
            }
            catch (ext) {
                window.__LocalFontsErrorFlag = true;
            }
        }
        window.__DCWriter5FullLoaded = true;
        var divs = document.querySelectorAll("div[dctype='WriterControlForWASM']");
        if (divs != null && divs.length > 0) {
            for (var iCount = 0; iCount < divs.length; iCount++) {
                var div = divs[iCount];
                if (div.__DCWriterReference == null) {
                    WriterControl_Event.RaiseControlEvent(div, 'EventBeforeCreateControl');
                    //判断是否存在属性autoCreateControl = false  DUWRITER5_0-861
                    var hasAutoCreateControlAttr = div.getAttribute("autocreatecontrol");//获取DIV元素上的autocreatecontrol属性的值
                    if (hasAutoCreateControlAttr != null) {
                        hasAutoCreateControlAttr = hasAutoCreateControlAttr.trim().toLowerCase();
                    }
                    // !== 不同类型不比较，且无结果，同类型才比较；
                    // 下面的判断表示autocreatecontrol属性不等于'false'还有'False'并且没有AboutControl时自动创建编辑器
                    if (hasAutoCreateControlAttr != 'false' && !div.AboutControl) {
                        WriterControl_Main.CreateWriterControlForWASM(div);
                    }
                }
            }
        }

        // 触发全局的DCWriter5Started事件
        var handler = window["DCWriter5Started"];
        if (typeof (handler) == "function") {
            handler();
        }
    },

    /**
     * 创建编辑器实例
     * @param {string | HTMLDivElement} strContainerID 容器HTML元素编号或者对象
     */
    CreateWriterControlForWASM: function (strContainerID, type) {
        if (window.__DCWriter5FullLoaded != true) {
            // 编辑器创建失败
            if (!!window.WriterControl_OnLoadError && typeof (window.WriterControl_OnLoadError) == "function") {
                window.WriterControl_OnLoadError.call(strContainerID, strContainerID);
            }
            throw "The functional modules of DCWriter5 have not been fully loaded yet, so it is temporarily impossible to create editor controls. ";
        }
        var strRuntimeID = strContainerID;
        if (typeof strContainerID == "object") {
            strRuntimeID = strContainerID.id;
            // 编辑器承载的元素可能在iframe中
            if (window.__DCWriterControls == null) {
                window.__DCWriterControls = new Array();
            }
            if (strContainerID.ownerDocument != document) {
                strRuntimeID = "!" + new Date().valueOf() + Math.random();
                window.__DCWriterControls[strRuntimeID] = strContainerID;
            }
            else if (DCTools20221228.IsNullOrEmptyString(strRuntimeID)) {
                // 没有提供ID值，则使用内部ID值
                strRuntimeID = "DC_" + new Date().valueOf() + Math.random();
                window.__DCWriterControls[strRuntimeID] = strContainerID;
            }
        }
        var rootElement = DCTools20221228.GetOwnerWriterControl(strContainerID);
        if (rootElement == null) {
            // 编辑器创建失败
            if (!!window.WriterControl_OnLoadError && typeof (window.WriterControl_OnLoadError) == "function") {
                window.WriterControl_OnLoadError.call(strContainerID, strContainerID);
            }
            return null;
        }

        //在此处进行RuleVisible值的判断
        rootElement.ruleVisible = rootElement.getAttribute('rulevisible');
        if (rootElement.ruleVisible != null && typeof rootElement.ruleVisible == 'string' && rootElement.ruleVisible.indexOf(',') > 0) {
            //对ruleVisible进行解析
            rootElement.ruleVisible = rootElement.ruleVisible.split(',');
            if (rootElement.ruleVisible && rootElement.ruleVisible.length == 2) {
                if (rootElement.ruleVisible[0].toLowerCase().trim() == 'false' && rootElement.ruleVisible[1].toLowerCase().trim() == 'false') {
                    rootElement.setAttribute('rulevisible', 'false');
                } else {
                    rootElement.setAttribute('rulevisible', 'true');
                }
            }
        }

        rootElement.__BKImgStyleName = "__dcbkimg_" + parseInt(Math.random() * 1000000);
        DCTools20221228.LogTick("Initalizing control:" + rootElement.id);

        try {
            //存储加载文档花费毫秒，用于提供给性能页面
            let indexPerformanceTiming = {};
            if (window.localStorage.getItem('indexPerformanceTiming')) {
                indexPerformanceTiming = {
                    ...JSON.parse(window.localStorage.getItem('indexPerformanceTiming'))
                };
            }
            indexPerformanceTiming['myWriterControl'] = {
                ...(indexPerformanceTiming.myWriterControl || {}),
                [rootElement.id]: {
                    startTime: (new Date()).valueOf(),
                }
            };
            window.localStorage.setItem('indexPerformanceTiming', JSON.stringify(indexPerformanceTiming));
        } catch (error) {

        }

        /** 编辑器是否已经创建过了*/
        var rootElementIsCreated = rootElement.__DCWriterReference != null;

        var nativeControl = DotNet.invokeMethod(
            window.DCWriterEntryPointAssemblyName,
            "CreateWriterControlForWASM",
            strRuntimeID);
        if (nativeControl == null) {
            // 编辑器创建失败
            if (!!window.WriterControl_OnLoadError && typeof (window.WriterControl_OnLoadError) == "function") {
                window.WriterControl_OnLoadError.call(rootElement, rootElement);
            }
            return null;
        }
        rootElement.__DCWriterReference = nativeControl;
        nativeControl.invokeMethod("set_WASMBaseZoomRate", window.devicePixelRatio);
        rootElement.CheckDisposed = function () {
            if (rootElement.__DCDisposed == true) {
                throw "DCWriter control{" + rootElement.id + "} is disposed,can not use again.";
            }
        };
        if (rootElement.getAttribute("enabledlogapi") == "true") {
            // 记录API调用
            WriterControl_UI.StartAPILogRecord(rootElement, true);
        }
        var allchild = rootElement.childNodes;
        allchild = Array.prototype.slice.call(allchild);
        allchild.forEach((childEle) => {
            if (childEle.nodeName == "#text") {
                rootElement.removeChild(childEle);
            }
        })
        var strProductVersion = nativeControl.invokeMethod("GetProductVersion");
        //rootElement.setAttribute("dctype", "WriterControlForWASM");
        rootElement.setAttribute("dcversion", strProductVersion);
        console.log("DCWriter published time:" + strProductVersion);
        if (rootElementIsCreated == false) {
            // 没有创建时，直接创建
            WriterControl_API.BindControlForCommon(rootElement, rootElement.__DCWriterReference);
            // 添加编辑器控件的成员
            WriterControl_API.BindControlForWriterControlForWASM(rootElement, rootElement.__DCWriterReference);
        }

        // 加载系统配置
        for (var iCount = 0; iCount < rootElement.attributes.length; iCount++) {
            var attr = rootElement.attributes[iCount];
            rootElement.__DCWriterReference.invokeMethod(
                "LoadConfigByHtmlAttribute",
                attr.name,
                attr.value);
        }
        rootElement.DocumentOptions = nativeControl.invokeMethod("GetDocumentOptions");
        //zhangbin 20230201 判断是否存在自定义高度,如不存在,默认设置600px 
        if (rootElement.style.height == '') {
            rootElement.style.height = '100%';
        }
        // 移除事件监听,防止重复添加
        rootElement.ownerDocument.body.removeEventListener('mousedown', WriterControl_UI.OwnerDocumentBodyMouseDownFunc);
        // 此处的方法用于处理关闭下拉和光标的
        rootElement.ownerDocument.body.addEventListener('mousedown', WriterControl_UI.OwnerDocumentBodyMouseDownFunc);

        //禁用浏览器默认的右键
        rootElement.addEventListener('contextmenu', function (e) {
            e.stopPropagation();
            e.preventDefault();
            e.returnValue = false;
            return false;
        });

        if (rootElement.CustomFontFamily) {
            DCTools20221228.LoadCustomFont(rootElement, false);
        }

        rootElement.ownerDocument.body.oncut = function (e) {
            // 修复存在多个编辑器时无法剪切的问题【DUWRITER5_0-3618】
            var srcElement = e.srcElement || e.target;
            var rootElement = this.ownerDocument.WriterControl || DCTools20221228.GetOwnerWriterControl(srcElement);
            if (rootElement == null) {
                return;
            }
            // 20240313 [DUWRITER5_0-2031] lxy如果属性对话框存在，则不执行编辑器的剪切
            var dc_dialogContainer = rootElement.ownerDocument.getElementById('dc_dialogContainer');
            if (dc_dialogContainer) {
                return true;
            }
            //在document中判断同组是否存在选中，如果存在清除并将选中也清除
            var rootEle = rootElement;
            if (rootEle == null || rootEle.ownerDocument.documentElement.contains(rootEle) == false) {
                // 编辑器元素不在文档中时，复制剪切走默认逻辑【DUWRITER5_0-2449】
                return;
            }
            // 判断window上是否存在选中，如果存在，直接执行
            // 修复使用iframe嵌套剪切和复制判断选中范围出错的问题【DUWRITER5_0-4199】
            var sel;
            try {
                var e_window = rootEle.ownerDocument && rootEle.ownerDocument.defaultView;
                if (e_window && typeof e_window.getSelection === "function") {
                    sel = e_window.getSelection();
                } else if (typeof window.getSelection === "function") {
                    sel = window.getSelection();
                } else {
                    sel = null;
                }
            } catch (error) {
                // console.error("Error getting selection:", error);
                sel = null;
            }
            //判断是否存在divCaret元素
            if (sel && sel.isCollapsed === true && rootEle.__DCWriterReference.invokeMethod("HasSelection") == true) {
                var datas = '';
                var ref9 = rootEle.__DCWriterReference;
                if (ref9 != null) {
                    datas = ref9.invokeMethod(
                        "DoCut", false, false);
                }
                WriterControl_UI.SetClipboardData(datas, e, 'cut', rootEle);
                e.stopPropagation();
                e.preventDefault();
                e.returnValue = false;
            }
        };

        rootElement.ownerDocument.body.oncopy = function (e) {
            var srcElement = e.srcElement || e.target;
            var rootElement = this.ownerDocument.WriterControl || DCTools20221228.GetOwnerWriterControl(srcElement);
            if (rootElement == null) {
                return;
            }
            // 20240313 [DUWRITER5_0-2031] lxy如果属性对话框存在，则不执行编辑器的复制
            var dc_dialogContainer = rootElement.ownerDocument.getElementById('dc_dialogContainer');
            if (dc_dialogContainer) {
                return true;
            }
            //在document中判断同组是否存在选中，如果存在清除并将选中也清除
            var rootEle = rootElement;
            // var allShowEle = null;
            // //判断所在的编辑器是否被展示
            // WriterControl_UI.GetCurrentWriterControl(function (allEle) {
            //     allShowEle = allEle;
            // }, rootElement);

            // if (allShowEle != null && allShowEle.length > 0) {
            //     //循环allShowEle
            //     for (var i = 0; i < allShowEle.length; i++) {
            //         var isEle = allShowEle[i];
            //         if (isEle.__DCWriterReference.invokeMethod("HasSelection") == true) {
            //             rootEle = isEle;
            //             break;
            //         }
            //     }
            //     //if (allShowEle.indexOf(rootEle) < 0) {
            //     //    //如果不存再选中的元素
            //     //    rootEle = allShowEle[0];
            //     //}
            // };
            if (rootEle == null || rootEle.ownerDocument.documentElement.contains(rootEle) == false) {
                // 编辑器元素不在文档中时，复制剪切走默认逻辑【DUWRITER5_0-2449】
                return;
            }
            // 判断window上是否存在选中，如果存在，直接执行
            // 修复使用iframe嵌套剪切和复制判断选中范围出错的问题【DUWRITER5_0-4199】
            var sel;
            try {
                var e_window = rootEle.ownerDocument && rootEle.ownerDocument.defaultView;
                if (e_window && typeof e_window.getSelection === "function") {
                    sel = e_window.getSelection();
                } else if (typeof window.getSelection === "function") {
                    sel = window.getSelection();
                } else {
                    sel = null;
                }
            } catch (error) {
                // console.error("Error getting selection:", error);
                sel = null;
            }
            // 在打印预览中只选中编辑器文本时处理复制逻辑，在其他场景下不处理【DUWRITER5_0-3894】
            if (rootEle.IsPrintPreview() == true) {
                // 打印预览控件
                var PrintPrewViewPageContainer = DCTools20221228.GetChildNodeByDCType(rootEle, "page-printpreview");
                if (sel && sel.isCollapsed === false && sel.rangeCount == 1 && typeof (sel.getRangeAt) == "function") {
                    var range = sel.getRangeAt(0);
                    if (range && range.commonAncestorContainer && typeof (range.cloneContents) == "function") {
                        var commonAncestorContainer = range.commonAncestorContainer;
                        if (PrintPrewViewPageContainer.contains(commonAncestorContainer) == true) {
                            // 打印预览控件触发EventBeforeCopy事件,data目前值为{IsPrintPreview: true}【DUWRITER5_0-3931】
                            if (rootEle.EventBeforeCopy != null && typeof (rootEle.EventBeforeCopy) == "function") {
                                var data = {
                                    IsPrintPreview: true
                                };
                                var CoptResult = rootElement.EventBeforeCopy(e, data);
                                if (CoptResult == false) {
                                    e.stopPropagation();
                                    e.preventDefault();
                                    return false;
                                }
                            }
                            /** 复制的内容 */
                            var clipboardText;
                            /** 剪切板数据对象 */
                            var clipboardData = e.clipboardData || window.clipboardData;
                            var clonedRange = range.cloneContents();
                            var divNode = document.createElement("div");
                            divNode.appendChild(clonedRange);
                            // 过滤掉不需要的内容
                            var removeNodes = divNode.querySelectorAll("[user-select='none'],style");
                            if (removeNodes.length > 0) {
                                for (var i = 0; i < removeNodes.length; i++) {
                                    var removeNode = removeNodes[i];
                                    removeNode.parentNode.removeChild(removeNode);
                                }
                            }
                            var textNodes = divNode.querySelectorAll("text");
                            if (textNodes.length > 0) {
                                var NowY = textNodes[0].getAttribute("y");
                                var text = "";
                                for (var i = 0; i < textNodes.length; i++) {
                                    var textNode = textNodes[i];
                                    var textNodeY = textNode.getAttribute("y");
                                    // 判断text标签是否在一行中
                                    // 如果不在一行中，则换行
                                    if (NowY != textNodeY) {
                                        NowY = textNodeY;
                                        text += "\n";
                                    }
                                    text += textNode.innerHTML.replace(/&nbsp;/g, " ");
                                }
                                if (text != "") {
                                    clipboardText = text;
                                }
                            }

                            //在这里再加一个判读用于复制了一段字符串的情况
                            var hasText = divNode.childNodes;
                            if (hasText.length > 0) {
                                try {
                                    var newText = "";
                                    hasText.forEach(item => {
                                        if (item.nodeName != "#text") {
                                            throw new Error("");
                                        }
                                    })
                                    //纯文本的情况下执行此操作
                                    newText = divNode.innerHTML.replace(/&nbsp;/g, " ");
                                    if (newText != "" && !clipboardText) {
                                        clipboardText = newText;
                                    }
                                } catch (err) {

                                }
                            }
                            if (clipboardText && clipboardData) {
                                clipboardData.setData("text/plain", clipboardText);
                                divNode = null;
                                e.preventDefault();
                                return false;
                            }
                            divNode = null;
                        }
                    }
                }
            }

            if (sel && sel.isCollapsed === true && rootEle.__DCWriterReference.invokeMethod("HasSelection") == true) {
                //存在选中
                var datas = '';
                var ref9 = rootEle.__DCWriterReference;
                if (ref9 != null) {
                    datas = ref9.invokeMethod(
                        "DoCopy", false);
                }
                WriterControl_UI.SetClipboardData(datas, e, 'copy', rootEle);
                e.stopPropagation();
                e.preventDefault();
                e.returnValue = false;
            }
        };

        //开启监听
        /**
         * 
         * @param {any} message 错误文本
         * @param {any} source  错误所在资源
         * @param {any} lineno  错误所在行
         * @param {any} colno   错误所在列
         * @param {any} error   详细信息
         */
        window.onerror = function (message, source, lineno, colno, error) {
            //var hasFrameWork = false;
            //在此处判断source是否存在_framework
            //if (source != null && typeof source == 'string') {
            //    if (source.indexOf('_framework') >= 0) {
            //        hasFrameWork = true;
            //    }
            //}
            if (rootElement.EventOnError && typeof rootElement.EventOnError == 'function') {
                var options = {
                    message,
                    source,
                    lineno,
                    colno,
                    error
                };
                var needLogError = rootElement.EventOnError(options);
                if (needLogError === false) {
                    //这里为处理控制台实现显示错误，默认显示
                    return true;
                }
            }
        };

        //当整个窗口失去焦点的时候也需要失去焦点
        //判断是否为移动端
        if (rootElement.ownerDocument && !('ontouchstart' in rootElement.ownerDocument.documentElement)) {
            window.addEventListener('onblur', function () {
                var dropdown = rootElement.querySelector('#divDropdownContainer20230111');
                if (dropdown != null) {
                    dropdown.CloseDropdown();
                }
                //关闭表格下拉输入域
                var dropdownTable = rootElement.querySelector(`#DCTableControl20240625151400`);
                if (dropdownTable && dropdownTable.CloseDropdownTable) {
                    dropdownTable.CloseDropdownTable();
                }


                WriterControl_UI.HideCaret(rootElement);
            });
        };

        //zhangbin 20230607 当最外层包裹的div元素大小改变的监听事件
        rootElement.resizeObserver = new ResizeObserver(entries => {
            if (rootElement.ownerDocument && !('ontouchstart' in rootElement.ownerDocument.documentElement)) {
                var dropdown = rootElement.querySelector('#divDropdownContainer20230111');
                //当rootElement尺寸发生改变时.关闭下拉
                if (dropdown != null) {
                    dropdown.CloseDropdown();
                }
                //关闭表格下拉输入域
                var dropdownTable = rootElement.querySelector(`#DCTableControl20240625151400`);
                if (dropdownTable && dropdownTable.CloseDropdownTable) {
                    dropdownTable.CloseDropdownTable();
                }
                WriterControl_UI.HideCaret(rootElement);
                //在此处判断最外层包裹大小改变并处理标尺位置
                WriterControl_Rule.InvalidateView(rootElement, "hrule");
                WriterControl_Rule.InvalidateView(rootElement, "vrule");
                //此处判断如果元素在页面不显示则不执行大小改变监听
                if (rootElement.getClientRects) {
                    if (rootElement.getClientRects().length > 0) {
                        rootElement.SetAutoZoom(WriterControl_Event.InnerRaiseEvent, 'EventDocumentResize', true);
                    }
                } else {
                    rootElement.SetAutoZoom(WriterControl_Event.InnerRaiseEvent, 'EventDocumentResize', true);
                }
                WriterControl_Paint.HandleScrollView(rootElement, true);
                //判断区域选择是否存在//判断是否为打印预览
                //if (rootElement.RectInfo && !rootElement.IsPrintPreview()) {
                //    rootElement.SetBoundsSelectionViewMode(false);
                //}
                // 在编辑器元素大小改变时修正区域选择打印蒙版位置
                if (rootElement.RectInfo && typeof (rootElement.RectInfo.AdjustBoundsSelectionStyle) == "function") {
                    rootElement.RectInfo.AdjustBoundsSelectionStyle();
                }
                // 在编辑器元素大小改变时修正自定义批注的位置
                if (rootElement && WriterControl_UI && typeof (WriterControl_UI.AdjustCustomDocumentCommentStyle) == "function") {
                    WriterControl_UI.AdjustCustomDocumentCommentStyle(rootElement);
                }
            }
            //WriterControl_Event.InnerRaiseEvent(rootElement, "EventDocumentResize");
        });
        rootElement.resizeObserver.disconnect(rootElement);
        //确保该元素身上只有一个事件监听
        rootElement.resizeObserver.observe(rootElement);

        rootElement.addEventListener("mousewheel", function (e) {
            if (e.altKey == false && e.ctrlKey == true && e.shiftKey == false) {
                // Ctrl+鼠标滚动则进行缩放操作
                // var elements = WriterControl_UI.GetPageCanvasElements(this);
                var zoomRate = rootElement.__DCWriterReference.invokeMethod("get_ZoomRate");
                //for (var iCount = 0; iCount < elements.length; iCount++) {
                //    var element = elements[iCount];
                //    if (element.hasAttribute("native-width")) {
                //        zoomRate = parseFloat(element.width)
                //            / parseFloat(element.getAttribute("native-width"));
                //        break;
                //    }
                //}
                //if (isNaN(zoomRate)) {
                //    zoomRate = 1;
                //}
                var newZoomRate = zoomRate;
                if (e.wheelDelta > 0 || e.detail < 0) {
                    // 向上滚动
                    newZoomRate = zoomRate + 0.05;
                }
                else {
                    // 向下滚动
                    newZoomRate = zoomRate - 0.05;
                }
                rootElement.SetZoomRate(newZoomRate);
                e.preventDefault && e.preventDefault();
                return false;
            }
        }, false);

        window.setTimeout(async function () {
            //在此处加载时间控件样式
            WriterControl_DateTimeControl.CreateCalendarCss(rootElement);
            WriterControl_Event.RaiseControlEvent(rootElement, "OnLoad");
            rootElement.__DCWriterReference.invokeMethod("CheckForLoadDefaultDocument");
            WriterControl_Rule.UpdateRuleVisible(rootElement);
            if (!rootElement.AboutControl || !rootElement.firstChild) {
                // 编辑器创建失败
                if (!!window.WriterControl_OnLoadError && typeof (window.WriterControl_OnLoadError) == "function") {
                    window.WriterControl_OnLoadError.call(rootElement, rootElement);
                }
            }
            if (navigator.clipboard && navigator.clipboard.read) {
                try {
                    const { state } = await navigator.permissions.query({
                        name: "clipboard-read",
                    });
                    if (state == 'prompt' || state == 'denied') {
                        await navigator.clipboard.read();
                    }
                } catch (err) {
                    //console.log(err)
                }
            }
            if (rootElement.attributes['showtoolbar'] && rootElement.attributes['showtoolbar'].value == 'true') {
                WriterControl_ToolBar.CreateToolBarControl(rootElement);
            }
        }, 1);

        //以下为编辑器父元素开启监听模式
        const rootElementParentNode = rootElement.parentNode; // 控件的父元素
        // 释放观察者模式的函数
        const disconnectObserver = () => {
            observer.disconnect();
            console.log('Release observer mode.');
        };
        // 监听执行的函数
        const handleMutation = (mutation) => {
            //判断变化的节点是否为子元素
            if (mutation.type === 'childList') {
                //判断删除节点中是否有编辑器dom
                const removedNodes = mutation.removedNodes;
                if (removedNodes.length > 0) {
                    for (let i = 0; i < removedNodes.length; i++) {
                        const item = removedNodes[i];
                        if (item && item.AboutControl) {
                            var AutoDispose = item.getAttribute('AutoDispose');
                            if (AutoDispose === 'true' || AutoDispose === true) {
                                item.Dispose && item.Dispose();
                            }
                        }
                    }
                }
                const childList = mutation.target.children;
                //没有子节点时释放观察者模式
                if (childList.length === 0) {
                    return disconnectObserver();
                }

                let flag = false;//用于标记剩余子节点中是否含有编辑器dom
                for (let i = 0; i < childList.length; i++) {
                    if (childList[i] && childList[i].AboutControl) {
                        flag = true;
                        break;
                    }
                }
                (flag === false) && disconnectObserver(); //子节点没有编辑器dom释放观察者模式
            }
        };
        const observer = new MutationObserver(function (mutationsList) {
            //此处方法会触发两遍，其中一次是子节点被脱离父节点，另一次是子节点被完全删除。这是 MutationObserver 的工作方式，
            for (const mutation of mutationsList) {
                handleMutation(mutation);
            }
        });
        observer.observe(rootElementParentNode, { childList: true, subtree: false });// subtree:是否监听多层子节点
    }
};

window.WriterControl_Main = WriterControl_Main;
window.WriterControl_Paint = WriterControl_Paint;
window.WriterControl_UI = WriterControl_UI;
window.WriterControl_Task = WriterControl_Task;
window.WriterControl_Rule = WriterControl_Rule;
window.WriterControl_Event = WriterControl_Event;
window.DCTools20221228 = DCTools20221228;
window.WriterControl_Dialog = WriterControl_Dialog;
window.WriterControl_DOMPackage = WriterControl_DOMPackage;
window.CreateWriterControlForWASM = WriterControl_Main.CreateWriterControlForWASM;
window.DisposeDCWriterDocument = WriterControl_API.DisposeDCWriterDocument;
// 入口点程序集名称
window.DCWriterEntryPointAssemblyName = "DCSoft.WASM";
window.DCWriterStaticInvokeMethod = function (e, ...t) {
    DotNet.invokeMethod(
        window.DCWriterEntryPointAssemblyName,
        e,
        ...t);
};