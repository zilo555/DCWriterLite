//*************************************************************************
//* 项目名称：
//* 当前版本: 
//* 开始时间:
//* 开发者:
//* 重要描述:
//*************************************************************************
//* 最后更新时间: 20230710
//* 最后更新人:zhangbin
//*************************************************************************

"use strict";
import { DCTools20221228 } from "./DCTools20221228.js";
import { PageContentDrawer } from "./PageContentDrawer.js";
import { WriterControl_Paint } from "./WriterControl_Paint.js";
import { WriterControl_Print } from "./WriterControl_Print.js";
import { WriterControl_Rule } from "./WriterControl_Rule.js";
import { DCBinaryReader } from "./DCTools20221228.js";
import { WriterControl_Event } from "./WriterControl_Event.js";
import { WriterControl_UI } from "./WriterControl_UI.js";

export let WriterControl_IO = {
    DownloadFontFileContent: function (strKeys, strFiles) {

    },
    HttpDownloadFontFile: function (strFontName, bolBold, bolItalic) {
    },
    /**
     * 从一个GZIP压缩生成的BASE64字符串加载内容
     * @param {string | HTMLElement} rootElement 编辑器对象
     * @param {string} strBase64String BASE64字符串
     * @param {string} strFormat 文件格式
     * @returns {boolean} 操作是否成功
     */
    LoadDocumentFromGZIPBase64String: function (rootElement, strBase64String, strFormat) {
        var rootElement = DCTools20221228.GetOwnerWriterControl(rootElementID);
        if (rootElement != null
            && strBase64String != null
            && strBase64String.length > 10
            && strBase64String.substring(0, 3) == "H4s") {
            rootElement.CheckDisposed();
            var bsData = DCTools20221228.FromBase64String(strBase64String); //看结构是把base64字符串转成uint8array
            var bsNativeData = DCTools20221228.GZipDecompress(bsData);
            var decoder = new TextDecoder();
            var strData = decoder.decode(bsNativeData);
            return WriterControl_IO.LoadDocumentFromString({ WriterControl: rootElement, Data: strData, Format: strFormat });
        }
        throw "不是GZIP格式";
    },
    /**
     * 从一个GZIP压缩的二进制内容中加载文档
     * @param {string | HTMLElement} rootElement 编辑器对象
     * @param {Uint8Array} bsData 二进制数据
     * @param {string} strFormat 文件格式
     * @returns {boolean} 操作是否成功
     */
    LoadDocumentFromGZIPData: function (rootElement, bsData, strFormat) {
        var rootElement = DCTools20221228.GetOwnerWriterControl(rootElementID);
        if (rootElement != null
            && bsData != null
            && bsData.length > 4
            && bsData[0] == 0x1F
            && bsData[1] == 0x8B
            && bsData[2] == 0x08) {
            var bsNativeData = DCTools20221228.GZipDecompress(bsData);
            if (bsNativeData != null && bsNativeData.length > 0) {
                var decoder = new TextDecoder();
                var strData = decoder.decode(bsNativeData);
                return WriterControl_IO.LoadDocumentFromString({ WriterControl: rootElement, Data: strData, Format: strFormat });
            }
        }
        throw "不是GZIP格式";
    },
    /**
     * 根据XML字符串创建一个XML读取器
     * @param {string} rootElementID 容器元素对象
     * @param {string} strXml XML字符串对象
     * @returns {boolean} 操作是否成功
     */
    CreateInnerXmlReader: function (rootElementID, strXml) {
        var rootElement = DCTools20221228.GetOwnerWriterControl(rootElementID);
        if (rootElement != null
            && strXml != null
            && strXml.length > 0
            && WriterControl_IO.PrepareInnerXmlReader(rootElement, strXml, rootElement.__DCWriterReference) == true) {
            return true;
        }
        return false;
    },
    /**
     * 从OFD格式的文件中获取病历XML字符串
     * @param {string | ArrayBuffer } data 数据
     * @param { boolean} isBase64String 是否为BASE64字符串
     * @returns {string} 处理后的数据
     */
    GetXmlStringFromOFD: function (data, isBase64String) {
        if (DCTools20221228.HasZIPFileHeader(data)) {
            var bsContent = data;
            if (isBase64String == true) {
                bsContent = DCTools20221228.FromBase64String(data);
            }
            var strXml = DotNet.invokeMethod(
                window.DCWriterEntryPointAssemblyName,
                "GetXMLStringFromOFD",
                bsContent);
            if (WriterControl_IO.HasXmlHeader(strXml)) {
                return strXml;
            }
        }
        return null;
    },
    /**
    * 从一个字符串中加载文档
    * @param {object} 参数
    * @param {string | HTMLElement} [args.WriterControl] 编辑器控件或者编号
    * @param {string } [args.Data] 文档数据
    * @param { null | boolean} [args.IsBase64String] 是否为BASE64字符串
    * @param { null | string } [args.SpecifyLoadPart] 加载特定部位
    * @param { null | Function} [args.ErrorCallBack] 加载错误时的回调函数
    * @returns {boolean} 操作是否成功
    */
    LoadDocumentFromString: function (args) {// rootElement, strData, strFormat, specifyLoadPart, isbase64string) {
        if (args == null) {
            throw "参数为空";
        }
        if (args.Data == null || args.Data.length == 0) {
            throw "文档内容为空";
        }
        var rootElement = DCTools20221228.GetOwnerWriterControl(args.WriterControl);
        if (rootElement == null) {
            throw "未指定编辑器控件";
        }
        //在此处清除掉光标的事件
        WriterControl_UI.HideCaret(rootElement);
        //rootElement.__DCWriterReference.invokeMethod("SetColor2", "#eeeeff" , "2023-1-2 9:1:19" , 3);
        var tick = new Date().valueOf();
        // 删除所有的页面图形元素
        var cnode = rootElement.firstChild;
        rootElement.LoadRemoveChild = true;
        while (cnode != null) {
            if (cnode.nodeName == "CANVAS" && cnode.getAttribute("dctype") == "page") {
                var tempNode = cnode;
                cnode = cnode.nextSibling;
                rootElement.removeChild(tempNode);
            }
            else if (cnode.nodeName == "DIV" && cnode.getAttribute("dctype") == "page-container") {
                while (cnode.firstChild != null) {
                    cnode.removeChild(cnode.firstChild);
                }
                break;
            }
            else {
                cnode = cnode.nextSibling;
            }
        }
        rootElement.LoadRemoveChild = false;
        rootElement.__WaterMarkData = null;
        WriterControl_Print.ClosePrintPreview(rootElement, false);
        rootElement.__LastLoadDocumentTime = new Date().valueOf();
        if (args.IsBase64String == true && args.Data.length > 100) {
            var strXml = WriterControl_IO.GetXmlStringFromOFD(args.Data, args.isBase64String);
            if (strXml != null && strXml.length > 0) {
                args.Data = strXml;
            }
            else {
                var strHeader = args.Data.substring(0, 40);
                var bs2 = DCTools20221228.FromBase64String(strHeader);
                if (bs2 != null
                    && bs2.length > 20) {
                    if (bs2[0] == 60) {
                        // 第一个字符就是‘<’，很可能是XML格式
                        var decoder = new TextDecoder("utf-8");
                        var strData2 = decoder.decode(DCTools20221228.FromBase64String(args.Data));
                        if (WriterControl_IO.HasXmlHeader(strData2) == true) {
                            args.Data = strData2;
                            args.IsBase64String = false;
                        }
                    }
                    else if (bs2[0] == 239 && bs2[1] == 187 && bs2[2] == 191 && bs2[3] == 60) {
                        // 具有UTF8文件头
                        var decoder = new TextDecoder("utf-8");
                        var strData2 = decoder.decode(DCTools20221228.FromBase64String(args.Data));
                        if (WriterControl_IO.HasXmlHeader(strData2) == true) {
                            args.Data = strData2;
                            args.IsBase64String = false;
                        }
                    }
                }
            }
        }
        var result = null;
        //if (typeof (args.Data) == "string") {
        //    var strHeader2 = args.Data.substring(0, 100);
        //    var index99 = strHeader2.indexOf("<html");
        //    if (index99 < 0) {
        //        index99 = strHeader2.indexOf("<HTML");
        //    }
        //    if (index99 >= 0 && index99 < 100) {
        //        // 认为是HTML格式
        //        result = WriterControl_IO.ParseHtmlToDocument(rootElement, "LoadDocumentFromString", args.Data);
        //    }
        //}
        if (result == null) {
            if (args.SpecifyLoadPart == null
                && args.IsBase64String != true
                && WriterControl_IO.PrepareInnerXmlReader(rootElement, args.Data, true) == true) {
                // 以快速XMLReader的模式加载文档
                result = rootElement.__DCWriterReference.invokeMethod("LoadDocumentFromInnerXmlReader");
            }
            else {
                if (WriterControl_IO.__LastXmlParserError != null
                    && WriterControl_IO.__LastXmlParserError.length > 0) {
                    if (typeof (args.ErrorCallBack) == "function") {
                        args.ErrorCallBack.call(rootElement, WriterControl_IO.__LastXmlParserError);
                    }
                }
                else if (args.IsBase64String === true) {
                    result = rootElement.__DCWriterReference.invokeMethod("LoadDocumentFromBase64String", args.Data, args.Format, args.SpecifyLoadPart);
                } else {
                    result = rootElement.__DCWriterReference.invokeMethod("LoadDocumentFromString", args.Data, args.Format, args.SpecifyLoadPart);
                }
            }
        }
        rootElement.TempElementForDoubleBuffer = null;
        WriterControl_Rule.InvalidateView(rootElement, "hrule");
        WriterControl_Rule.InvalidateView(rootElement, "vrule");
        //var pc = WriterControl_UI.GetPageContainer(rootElement);
        //if (pc != null) {
        //    while (pc.firstChild != null) {
        //        pc.removeChild(pc.firstChild);
        //    }
        //}
        WriterControl_Paint.InvalidateAllView(rootElement);
        tick = new Date().valueOf() - tick;
        console.log(window.__DCSR.MillisecondsForLoadDocument + tick);

        try {
            //存储加载文档花费毫秒，用于提供给性能页面
            let indexPerformanceTiming = {};
            if (window.localStorage.getItem('indexPerformanceTiming')) {
                indexPerformanceTiming = {
                    ...JSON.parse(window.localStorage.getItem('indexPerformanceTiming'))
                };
            }
            indexPerformanceTiming['documentTiming'] = [...(indexPerformanceTiming.documentTiming || [])];
            indexPerformanceTiming['documentTiming'].push({
                title: window.__DCSR.MillisecondsForLoadDocument,
                useTime: tick,
                id: rootElement.id
            });
            window.localStorage.setItem('indexPerformanceTiming', JSON.stringify(indexPerformanceTiming));
        } catch (error) {

        }
        //表示为文档正常加载
        if (WriterControl_Task.__Tasks && WriterControl_Task.__Tasks.length > 0) {
            WriterControl_Task.AddCallbackForCompletedAllTasks(function () {
                WriterControl_Event.RaiseControlEvent(rootElement, "EventAfterDocumentDraw");
                scrollToRoof();
            });
            return result;
        } else {
            WriterControl_Event.RaiseControlEvent(rootElement, "EventAfterDocumentDraw");
            scrollToRoof();
        }

        function scrollToRoof() {
            if (typeof rootElement.getClientRects == "function") {
                //滚动视图到页面的顶部
                var pageContainer = rootElement.querySelector('[dctype="page-container"]');
                if (DCTools20221228.IsReadonlyAutoFocus(rootElement)) {
                    //编辑器内部页面存在滚动
                    if (pageContainer && pageContainer.scrollHeight > pageContainer.clientHeight) {
                        pageContainer.scrollTo(0, 0);
                    }
                }
            }

        }
        return result;
    },

    /**
    * 从一个字符串中加载文档
    * @param {object} rootElement 编辑器控件对象
    * @param {string} strData 文件内容
    * @param {string} strFormat 文件格式
    * @param {boolean} useBase64 使用BASE64加载
    * @returns {boolean} 操作是否成功
    */
    AddDocumentByText: function (rootElement, strData, strFormat, useBase64) {
        var tick = new Date().valueOf();
        //// 删除所有的页面图形元素
        //var cnode = rootElement.firstChild;
        //while (cnode != null) {
        //    if (cnode.nodeName == "svg" && cnode.getAttribute("dctype") == "page") {
        //        var tempNode = cnode;
        //        cnode = cnode.nextSibling;
        //        rootElement.removeChild(tempNode);
        //    }
        //    else if (cnode.nodeName == "DIV" && cnode.getAttribute("dctype") == "page-container") {
        //        while (cnode.firstChild != null) {
        //            cnode.removeChild(cnode.firstChild);
        //        }
        //        break;
        //    }
        //    else {
        //        cnode = cnode.nextSibling;
        //    }
        //}
        //rootElement.__WaterMarkData = null;
        //WriterControl_Print.ClosePrintPreview(rootElement, false);
        rootElement.__LastLoadDocumentTime = new Date().valueOf();
        var result = null;
        if (useBase64 !== true && WriterControl_IO.PrepareInnerXmlReader(rootElement, strData, rootElement.__DCWriterReference) == true) {
            // 以快速XMLReader的模式加载文档
            result = rootElement.__DCWriterReference.invokeMethod("AddDocumentByInnerXmlReader");
        }
        else {
            result = rootElement.__DCWriterReference.invokeMethod("AddDocumentByText", strData, strFormat, useBase64);
        }
        rootElement.TempElementForDoubleBuffer = null;
        // WriterControl_Rule.InvalidateView(rootElement, "hrule");
        // WriterControl_Rule.InvalidateView(rootElement, "vrule");
        //WriterControl_Paint.InvalidateAllView(rootElement);
        tick = new Date().valueOf() - tick;
        console.log(window.__DCSR.MillisecondsForLoadDocument + tick);
        return result;
    },

    /**
    * 添加混合合并的预览文档集合
    * @param {object} rootElement 编辑器控件对象
    * @param {object} parameters 混合合并的参数对象，定义同编辑器GetPrintPreviewHTML2的接口
    * @returns {boolean} 操作是否成功
    */
    AddDocumentsByMixedFiles: function (rootElement, parameters) {
        var tick = new Date().valueOf();
        rootElement.__LastLoadDocumentTime = new Date().valueOf();
        var result = rootElement.__DCWriterReference.invokeMethod("AddDocumentsByMixedFilesForPrintPreview", parameters);;
        rootElement.TempElementForDoubleBuffer = null;
        tick = new Date().valueOf() - tick;
        console.log(window.__DCSR.MillisecondsForLoadDocument + tick);
        return result;
    },

    /**
    * 下载文件
    * @param {object} rootElement 编辑器控件对象
    * @param {string} strFormat 文件格式
    * @param {string} strFileName 指定的文件名
    * @param {Function} callBack 获取数据的回调函数
    * @returns {boolean} 操作是否成功
    */
    DownLoadFile: function (rootElement, strFormat, strFileName, callBack) {
        if (strFormat == null || strFormat.length == 0) {
            strFormat = "xml";
        }
        else {
            strFormat = strFormat.trim().toLowerCase();
        }
        rootElement.CheckDisposed();
        strFileName = rootElement.__DCWriterReference.invokeMethod("GetRuntimeFileName", strFileName);
        if (strFormat == "xml") {
            // 这些文件格式可以在本地生成
            var strResult = DCTools20221228.UnPackageStringValue(rootElement.__DCWriterReference.invokeMethod("SaveDocumentToString", strFormat));
            if (typeof (callBack) == "function") {
                callBack(strResult);
            } else {
                let blob = new Blob([strResult], { type: strBlobType });
                let downloadElement = rootElement.ownerDocument.createElement("a");
                let href = window.URL.createObjectURL(blob); //创建下载的链接
                downloadElement.href = href;
                //console.log(file.name, "文件名");
                downloadElement.download = strFileName;// file.name; //下载后文件名
                rootElement.ownerDocument.body.appendChild(downloadElement);
                downloadElement.click(); //点击下载
                rootElement.ownerDocument.body.removeChild(downloadElement); //下载完成移除元素
                window.URL.revokeObjectURL(href); //释放掉blob对象
            }
            return true;
        }
        else {
            console.log(window.__DCSR.NotSupportedFileFormat + strFormat);
        }
        return false;
    },
    HasXmlHeader: function (strData) {
        if (typeof (strData) == "string" && strData.length > 20) {
            var strHeader = strData.substring(0, 200);
            if (strHeader.indexOf("<XTextDocument") >= 0
                || strHeader.indexOf("<DCDocument2022") >= 0) {
                if (strHeader.indexOf("HasEncrypted=") > 0) {
                    // 该XML具有透明加密部分，不处理。
                    return false;
                }
                // 是标准的XML。
                return true;
            }
        }
        return false;
    },
    /**
     * 准备以XMLReader的方式加载文档XML内容
     * @param {string| HTMLElement} 编辑器对象
     * @param {string} strXML XML字符串
     * @param {boolean} bolCheckDCWriterDocumentXmlHeader 是否检查文档XML头标记
     * @returns {boolean} 操作是否成功
     */
    PrepareInnerXmlReader: function (
        rootElement,
        strXML,
        bolCheckDCWriterDocumentXmlHeader) {
        rootElement = DCTools20221228.GetOwnerWriterControl(rootElement);
        if (rootElement == null) {
            return false;
        }
        WriterControl_IO.__LastXmlParserError = null;
        if (strXML == null || strXML.length == 0) {
            // 参数为空，不处理
            return false;
        }
        if (bolCheckDCWriterDocumentXmlHeader == true && WriterControl_IO.HasXmlHeader(strXML) == false) {
            // 不是标准的病历文档XML，不处理。
            return false;
        }
        var starTick = new Date().valueOf();
        var strHeader = strXML.substring(0, 200);
        var index933 = strHeader.indexOf("?>");
        if (index933 > 0) {
            // 去掉XML声明头
            var headerLen = strHeader.length;
            strHeader = strHeader.substring(index933 + 2).trim();
            strXML = strHeader + strXML.substring(headerLen);
        }
        var tick = new Date().valueOf();
        var tick2 = tick;
        var pr = new DOMParser();
        var xmldoc = pr.parseFromString(strXML, "text/xml");//"application/xml" "text/xml"
        if (xmldoc.documentElement.nodeName == "html") {
            WriterControl_IO.__LastXmlParserError = xmldoc.documentElement.innerText;
            // 解析错误
            return false;
        }
        var intResetEnumCount = 0;
        for (var node4 = xmldoc.documentElement.firstChild; node4 != null; node4 = node4.nextSibling) {
            if (intResetEnumCount == 1) {
                intResetEnumCount++;
                node4 = xmldoc.documentElement.firstChild;
            }
            if (node4.nodeName == "parsererror") {
                // 解析错误
                if (intResetEnumCount > 1) {
                    // 无法自我修复的错误
                    WriterControl_IO.__LastXmlParserError = node4.innerText;
                    console.error("XML解析错误:" + node4.innerText);
                    return false;
                }
                // 解析错误
                if (strXML.indexOf("&#x0;") > 0) {
                    // 出现非法字符，尝试修复
                    var strXML2 = DCTools20221228.StringReplaceAll("&#x0;", "?", strXML);
                    // var strXML2 = strXML.replaceAll("&#x0;", "?");
                    xmldoc = pr.parseFromString(strXML2, "text/xml");
                    if (xmldoc.documentElement.nodeName == "html") {
                        WriterControl_IO.__LastXmlParserError = xmldoc.documentElement.innerText;
                        // 解析错误
                        return false;
                    }
                    for (var node5 = xmldoc.documentElement.firstChild; node5 != null; node5 = node5.nextSibling) {
                        if (node5.nodeName == "parsererror") {
                            // 无法修复的解析错误
                            WriterControl_IO.__LastXmlParserError = node5.innerText;
                            console.error("XML解析错误:" + node5.innerText);
                            return false;
                        }
                    }
                } else {
                    if (node4.textContent.indexOf('\nBelow') >= 0) {
                        //通过前端修复常见的问题来获取到正确的xmldoc
                        // strXML = strXML.replaceAll('\x00', '\\x00');
                        // strXML = strXML.replaceAll('\x1f', '\\x1f');
                        // strXML = strXML.replaceAll('\x7f', '\\x7f');
                        // strXML = strXML.replaceAll('&#x1F;', ' ');
                        strXML = DCTools20221228.StringReplaceAll("\x00", "\\x00", strXML);
                        strXML = DCTools20221228.StringReplaceAll("\x1f", "\\x1f", strXML);
                        strXML = DCTools20221228.StringReplaceAll("\x7f", "\\x7f", strXML);
                        strXML = DCTools20221228.StringReplaceAll("&#x1F;", " ", strXML);
                        xmldoc = pr.parseFromString(strXML, "text/xml");
                        node4 = xmldoc.documentElement.firstChild;
                        intResetEnumCount = 1;
                    } else {
                        WriterControl_IO.__LastXmlParserError = node4.innerText;
                        console.error("XML解析错误:" + node4.innerText);
                        return false;
                    }
                }
            }
        }
        var binaryDataArray = new Array();
        tick2 = new Date().valueOf() - tick2;
        var strTable = new Array();
        var strTableValues = new Array();
        strTableValues.push("");// 先放置一个空白字符串
        var idNames = new Set();
        var GetStringIndex = function (txt, isName) {
            if (txt == null || txt.length == 0) {
                return 0;
            }
            //var index = strTable.indexOf( txt );//[ txt ];
            //if(index <0){
            //    strTable.push( txt );
            //    index = strTable.length - 1;
            //}
            var index = strTable[txt];
            if (typeof (index) == "undefined") {
                index = strTableValues.length;
                strTableValues.push(txt);
                strTable[txt] = index;
            }
            if (isName == true) {
                idNames.add(index);
            }
            //if (strTableValues[ index ] != txt )
            //{
            //    var len4 = txt;
            //}
            return index;
        };
        var funcOutputAttributes = function (attributes, array) {
            var attrLen = attributes.length;
            array.push(attrLen);
            for (var iCount = 0; iCount < attrLen; iCount++) {
                var attr = attributes[iCount];
                array.push(GetStringIndex(attr.localName, true));
                array.push(GetStringIndex(attr.nodeValue, false));
            }
        };
        var funcOutputNodes = function (firstNode, array, stackLevel) {
            //var nodeLen = nodes.length;
            array.push(0);
            var lenIndex = array.length - 1;
            array.push(0);
            var outputLength = 0;
            //for (var iCount = 0; iCount < nodeLen; iCount++) {
            var preNode = null;
            for (var node = firstNode; node != null; node = node.nextSibling) {
                //var node = nodes[iCount];
                var nodeType = node.nodeType;
                if (nodeType == 1) {
                    // 元素
                    funcOutputOneNode(node, array, stackLevel + 1);
                    outputLength++;
                }
                else if (nodeType == 3) {
                    // #text
                    var nextNode = node.nextSibling;
                    if (nextNode != null && nextNode.nodeType == 1) {
                        // 夹杂在XML元素中间的纯文本节点，则忽略掉。
                        continue;
                    }
                    var preNode = node.previousSibling;
                    if (preNode != null && preNode.nodeType == 1) {
                        // 夹杂在XML元素中间的纯文本节点，则忽略掉。
                        continue;
                    }
                    //var strText = node.nodeValue;
                    //if (strText.charCodeAt(0) == 13 || strText.charCodeAt(0) == 10) {
                    //    if (preNode == null || preNode.nodeType == 1) {
                    //        var nextNode = node.nextSibling;
                    //        if (nextNode == null || nextNode.nodeType == 1) {
                    //            if (strText.trim().length == 0) {
                    //                // 为一个无意义的空行
                    //                continue;
                    //            }
                    //        }
                    //    }
                    //}
                    //else if (node.nextSibling != null
                    //    && strText.charCodeAt(0) == 32
                    //    && node.nextSibling.nodeType != 3
                    //    && strText.trim().length == 0) {
                    //    // 为第一个元素间空白,忽略
                    //    continue;
                    //}
                    array.push(0);
                    array.push(GetStringIndex(node.nodeValue, false));
                    outputLength++;
                }
                preNode = node;
            }//for
            array[lenIndex] = outputLength;
            array[lenIndex + 1] = array.length - 1;
        };
        /**@param { HTMLElement} rootNode
         @param {Array} array*/
        var funcOutputOneNode = function (rootNode, array, stackLevel) {
            //if (stackLevel > 30)
            //{
            //    var str44 = "";
            //    for (var i2 = 0; i2 < stackLevel; i2++) {
            //        str44 = str44 + " ";
            //    }
            //    console.log(stackLevel + " " + str44 + " " + rootNode.nodeName);
            //}
            var attrLen = 0;
            if (rootNode.hasAttributes()) {
                attrLen = rootNode.attributes.length;
            }
            var firstChild = rootNode.firstChild;
            if (attrLen == 0 && firstChild == null) {
                // 没有任何数据
                array.push(1);//ElementType.EmptyElement
                array.push(GetStringIndex(rootNode.nodeName, true));
            }
            else if (attrLen == 0 && firstChild != null) {
                // 没有属性，有子节点
                if (firstChild.nodeType == 3 && firstChild.nextSibling == null) {
                    // 只有一个文本数据
                    array.push(2);//ElementType.ElementString
                    array.push(GetStringIndex(rootNode.nodeName, true));
                    var strNodeValue = firstChild.nodeValue;
                    if (strNodeValue.length > 1024 * 5
                        && rootNode.nodeName == "ImageDataBase64String") {
                        // 这是一个比较大的BASE64字符串,提前进行转换
                        var bsData = DCTools20221228.FromBase64String(strNodeValue);
                        array.push(GetStringIndex("$BINARY_" + binaryDataArray.length));
                        binaryDataArray.push(bsData);
                    }
                    else {
                        array.push(GetStringIndex(firstChild.nodeValue, false));
                    }
                    return;
                }
                array.push(3);// ElementType.ElementNoAttribute
                array.push(GetStringIndex(rootNode.nodeName, true));
                funcOutputNodes(firstChild, array, stackLevel);
            }
            else if (attrLen > 0 && firstChild == null) {
                // 有属性，没有子节点
                array.push(4);// ElementType.ElementNoChild
                array.push(GetStringIndex(rootNode.nodeName, true));
                funcOutputAttributes(rootNode.attributes, array);
            }
            else {
                // 有属性有子节点
                array.push(5);//ElementType.ElementFull
                array.push(GetStringIndex(rootNode.nodeName, true));
                funcOutputAttributes(rootNode.attributes, array);
                funcOutputNodes(firstChild, array, stackLevel);
            }
        };
        var list = new Array();
        funcOutputOneNode(xmldoc.documentElement, list, 0);
        var tickSpan = new Date().valueOf() - starTick;
        //strTable.push( list.toString());
        var byteIndexs = new Uint8Array(new Int32Array(list).buffer);
        var byteIndexNames = new Uint8Array(new Int32Array(idNames).buffer);
        //alert("处理XML毫秒:" + tick2 + "/" + tick + "   " + byteIndexs.length );
        strTable = null;
        list = null;
        idNames = null;
        // 传递二进制的参数
        rootElement.__DCWriterReference.invokeMethod("ClearInnerXmlReader");
        rootElement.__DCWriterReference.invokeMethod("AddBianryDataForXmlReader", byteIndexs);
        rootElement.__DCWriterReference.invokeMethod("AddBianryDataForXmlReader", byteIndexNames);
        if (binaryDataArray.length > 0) {
            // 设置预先解析出的二进制数据
            for (var iCount = 0; iCount < binaryDataArray.length; iCount++) {
                rootElement.__DCWriterReference.invokeMethod("AddBianryDataForXmlReader", binaryDataArray[iCount]);
            }
        }
        rootElement.__DCWriterReference.invokeMethod("SetStringTableForXmlReader", strTableValues);
        strTableValues = null;
        return true;
    }
};