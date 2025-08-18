// 打印相关函数
"use strict";

import { DCTools20221228 } from "./DCTools20221228.js";
import { DCBinaryReader } from "./DCTools20221228.js";
import { PageContentDrawer } from "./PageContentDrawer.js";
import { WriterControl_Paint } from "./WriterControl_Paint.js";
import { WriterControl_Task } from "./WriterControl_Task.js";
import { WriterControl_Event } from "./WriterControl_Event.js";
import { WriterControl_UI } from "./WriterControl_UI.js";
import { WriterControl_Rule } from "./WriterControl_Rule.js";


/** 打印相关模块 */
export let WriterControl_Print = {

    /**
     * 创建Print()/PrintPreview()函数使用的参数对象
     * @returns
     */
    CreatePrintOptions: function () {
        return {
            NotPrintWatermark: true | false | null,// 默认为false,不打印水印
            PrintTableCellBorder: true | false | null,// 默认为true,是否打印表格单元格边框
            CleanMode: true | false | null,// 默认为空，是否为整洁打印模式，可选值为true:整洁打印，false:留痕打印，空：采用编辑器当前的留痕显示设置。
            PrintRange: "AllPages" | "Selection" | "SomePages" | "CurrentPage",// 默认AllPages,打印范围，为一个字符串，可以为 AllPages,Selection,SomePages,CurrentPage
            PrintMode: "Normal" | "OddPage" | "EvenPage",// 默认Normal,打印模式，为一个字符串，可以为 Normal,OddPage,EvenPage。这里的页码是从0开始计算的。
            Collate: true,// 默认false,是否为逐份打印，为一个布尔值。需要本地打印才有效果
            Copies: 1,// 默认1,打印份数，为一个整数。需要本地打印才有效果
            FromPage: 0, // 默认0，从0开始计算的打印开始页码，只有PrintRange=SomePages时本设置才有效
            ToPage: 1,//默认为总页数, 从0开始计算的打印结束开始页码，只有PrintRange=SomePages时本设置才有效
            SpecifyPageIndexs: "1,3,6-11,12",//默认空，打印指定页码列表，页码从0开始计算，各个项目之间用逗号分开，如果项目中间有个横杠，表示一个页码范围
            BodyLayoutOffset: 0,// 默认为0，正文偏移续打的纵向偏移量。当该值大于0，则续打设置无效。
            PageIndexFix: 0// 默认为0，打印出来的页码值的修正量。
        };
    },

    /** 打印预览控件的属性文档视图 
     * @param {any} options 打印设置
     */
    InvalidatePreview: function (containerID, options) {
        var rootElement = DCTools20221228.GetOwnerWriterControl(containerID);
        if (rootElement != null) {
            WriterControl_Print.PrintPreview(rootElement, options);
        }
    },
    /**
     * 获得打印预览的页面所在的容器元素
     * @param {string | HTMLElement } strContainerID 编辑器控件编号或者对象
     * @returns { HTMLElement } 容器元素对象
     */
    GetPrintPrewViewPageContainer: function (strContainerID) {
        var div = DCTools20221228.GetChildNodeByDCType(strContainerID, "page-printpreview");
        return div;
    },
    /**
     * 是否处于打印预览状态
     * @param {string | HTMLElement} continaerID
     * @returns {boolean} 是否处于打印预览状态
     */
    IsInPrintPreview: function (continaerID) {
        var div = DCTools20221228.GetChildNodeByDCType(continaerID, "page-printpreview");
        return div != null && DCTools20221228.IsNullOrEmptyString(div.style.display);
    },
    /**
     * 关闭打印预览
     * @param {string | HTMLDivElement} containerID 根元素
     * @param {boolean} bolRefreshView  是否恢复文档内容排版
     */
    ClosePrintPreview: function (containerID, bolRefreshView) {
        var rootElement = DCTools20221228.GetOwnerWriterControl(containerID);
        if (rootElement != null) {
            var div = DCTools20221228.GetChildNodeByDCType(rootElement, "page-printpreview");
            if (div != null && DCTools20221228.IsNullOrEmptyString(div.style.display)) {
                // 删除打印预览的部件
                rootElement.removeChild(div);
                // 显示编辑视图
                WriterControl_Print.SetPageContainerVisible(rootElement, true);
                if (bolRefreshView == true) {
                    // 恢复文档排版
                    rootElement.__DCWriterReference.invokeMethod("RefreshViewAfterPrint", true);
                }
            }
        }
    },
    /**
     * 设置编辑页面容器的可见性
     * @param {string | HTMLDivElement} containerID 根元素
     * @param {boolean} bolVisible - 是否可见
     */
    SetPageContainerVisible: function (containerID, bolVisible) {
        // 检查bolVisible是否为布尔类型
        if (typeof bolVisible !== "boolean") {
            console.error("bolVisible must be a boolean");
            return false;
        }
        // 获取容器的根元素
        var rootElement = DCTools20221228.GetOwnerWriterControl(containerID);
        // 如果根元素不存在，输出错误信息并返回
        if (!rootElement) {
            console.error("rootElement not found for containerID:", containerID);
            return false;
        }
        // 获取标尺打印预览是否占位
        var PrintRuleOccupying = rootElement.getAttribute("PrintRuleOccupying") == "true";// 打印预览下，标尺是否占位。
        // 显示或隐藏容器
        for (var element = rootElement.firstChild; element != null; element = element.nextElementSibling) {
            // 如果元素为空，跳过
            if (!element) {
                continue;
            }
            // 如果元素节点类型不是元素节点，跳过
            if (element.nodeType != 1) {
                continue;
            }
            // 如果元素是样式表，跳过
            if (element.nodeName == "STYLE") {
                // 跳过样式表
                continue;
            }
            //打印预览
            if (element.className == 'DC-toolBar-panel') {
                continue;
            }
            // 获取元素的dctype属性
            var DctypeStr = element.getAttribute("dctype");
            // 获取是否为标尺元素
            var isRule = (element.nodeName === "CANVAS") && (DctypeStr === "hrule" || DctypeStr === "vrule");
            // 当前为标尺元素，并且设置了打印时标尺隐藏占位
            if (isRule && PrintRuleOccupying) {
                // 显示隐藏标尺元素
                element.style.visibility = bolVisible ? "visible" : "hidden";
            } else {
                if (bolVisible == true) {
                    // 显示元素
                    element.style.display = element.__display_back;
                } else {
                    // 隐藏元素
                    if (element.style.display != "none") {
                        element.__display_back = element.style.display;
                        element.style.display = "none";
                    }
                }
            }
            
        }
        return true;
    },
    GetRuntimePageIndexString: function (rootElement, options) {
        // 触发准备打印事件
        WriterControl_Event.InnerRaiseEvent(rootElement, "EventPreparePrint", options);
        // 获得实际打印输出的页码列表
        var strCode = rootElement.__DCWriterReference.invokeMethod("GetPageIndexWidthHeightForPrint", true, options, false);
        var datas = JSON.parse(strCode);
        var pageCount = datas.length; // datas.length / 3;
        var strResult = "";
        for (var iCount = 0; iCount < pageCount; iCount++) {
            if (strResult.length > 0) {
                strResult += ",";
            }
            strResult += iCount.toString();
        }
        return strResult;
    },
    /**
     * 重新绘制打印预览中所有的内容
     * @param {string | HTMLElement} containerID
     */
    PrintPreviewInvalidateAllView: function (containerID) {
        var rootElement = DCTools20221228.GetOwnerWriterControl(containerID);
        var pageContainer = DCTools20221228.GetChildNodeByDCType(rootElement, "page-printpreview");
        if (pageContainer != null && DCTools20221228.IsNullOrEmptyString(pageContainer.style.display)) {
            for (var node2 = pageContainer.firstChild; node2 != null; node2 = node2.nextSibling) {
                if (node2.nodeName.toLowerCase() == "svg") {
                    while (node2.firstChild != null) {
                        node2.removeChild(node2.firstChild);
                    }
                    node2._isRendered = false;
                }
            }
            pageContainer.__DoPageContainerScroll && pageContainer.__DoPageContainerScroll.call(pageContainer);
        }
    },
    /**
     * 为缩放而更新打印预览
     * @param {string | HTMLElement} containerID
     */
    UpdateZoomRateForPrintPreview: function (containerID) {
        var rootElement = DCTools20221228.GetOwnerWriterControl(containerID);
        var pageContainer = DCTools20221228.GetChildNodeByDCType(rootElement, "page-printpreview");
        if (pageContainer != null && DCTools20221228.IsNullOrEmptyString(pageContainer.style.display)) {
            for (var node2 = pageContainer.firstChild; node2 != null; node2 = node2.nextSibling) {
                if (node2.nodeName == "CANVAS") {
                    WriterControl_Paint.SetPageElementSize(rootElement, node2);
                    node2._isRendered = false;
                } else if (node2.nodeName == "svg") {
                    // 处理svg打印预览的缩放【DUWRITER5_0-3312】
                    WriterControl_Paint.SetPageElementSize(rootElement, node2);
                }
            }
            pageContainer.__DoPageContainerScroll && pageContainer.__DoPageContainerScroll.call(pageContainer);
        }
    },
    /**
     * 打印预览
     * @param {string | HTMLDivElement} containerID 根节点对象
     * @param {any} options 选项
     * @param {string} eleString 元素的数组 zhangbin 20230601
     */
    PrintPreview: function (containerID, options, eleString) {
        var rootElement = DCTools20221228.GetOwnerWriterControl(containerID);
        if (rootElement == null) {
            return false;
        }
        
        // 修复打印预览模式下还可以调用打印预览的问题【DUWRITER5_0-3318】
        // 当目前是打印预览模式时，就不需要进行再次渲染
        if (rootElement.IsPrintPreview() == true) {
            return false;
        }
        
        // 隐藏编辑视图
        WriterControl_Print.SetPageContainerVisible(rootElement, false);
        // 处理滚动事件,懒加载打印预览内容【DUWRITER5_0-3310】
        function DoPageContainerScroll() {
            var DivRect;
            // 此处修改nextSibling为nextElementSibling处理nextSibling为text的情况
            for (var element = this.firstChild; element != null; element = element.nextElementSibling) {
                var lowNodeName = element.nodeName.toLowerCase();
                if ((lowNodeName == "canvas" || lowNodeName == "svg") && element.getAttribute("dctype") == "page") {
                    // 已经渲染完成的无需重新渲染
                    if (element._isRendered == true) {
                        continue;
                    }
                    // 获取打印预览盒子的DOMRect对象
                    if (DivRect == null) {
                        DivRect = this.getBoundingClientRect();
                    }
                    // 获取元素的DOMRect对象
                    var PageRect = element.getBoundingClientRect();
                    if (DivRect.top > PageRect.bottom || DivRect.bottom < PageRect.top) {
                        // 不在可视区域中
                        continue;
                    }
                    // 【?】目前会被渲染卡一下
                    WriterControl_Print.InnerDrawOnePage(element, true);
                    element._isRendered = true;
                    //var drawer = new PageContentDrawer(element, null);
                    //drawer.PageIndex = element.PageIndex;
                    //drawer.EventQueryCodes = function (drawer2) {
                    //    // 获得该页面的绘图代码
                    //    this.CanvasElement._isRendered = true;
                    //    var strCodePage = rootElement.__DCWriterReference.invokeMethod(
                    //        "PaintPageForPrint",
                    //        drawer2.PageIndex,
                    //        true);
                    //    return strCodePage;
                    //};
                    //drawer.AddToTask();
                }
            }//for
        };

        //var editorContainer = DCTools20221228.GetChildNodeByDCType(rootElement, "page-container");
        var pageContainer = DCTools20221228.GetChildNodeByDCType(rootElement, "page-printpreview");
        if (pageContainer == null) {
            pageContainer = rootElement.ownerDocument.createElement("DIV");
            pageContainer.setAttribute("dctype", "page-printpreview");
            //pageContainer.style.setProperty('position', 'relative', 'important');
            //pageContainer.style.margin = "10px 10px 10px 10px";
            pageContainer.style.height = "100%";
            pageContainer.style.overflow = "auto";
            //pageContainer.style.backgroundColor = "#ffffff";
            pageContainer.style.textAlign = "center";
            pageContainer.style.position = "relative";
            //使用动画开启硬件加速，使打印预览效果更流畅
            pageContainer.style.transform = "translate3d(0,0,0)";
            pageContainer.style['-moz-transform'] = "translate3d(0,0,0)";
            pageContainer.style['-webkit-transform'] = "translate3d(0,0,0)";
            //对pageContainer添加事件监听
            pageContainer.addEventListener('contextmenu', function (e) {
                //修改为svg打印预览
                //[DUWRITER5_0-3585] 20240918 lxy 修复svg打印预览右键菜单问题
                if (e.target != null) {
                    if (typeof (rootElement.EventPrintPreviewContextMenu) == "function"
                        || typeof (rootElement.SetPreviewContextMenu) == "function") {
                        //判断是否为文本或者输入域或者表格
                        if (typeof (rootElement.EventPrintPreviewContextMenu) == "function") {
                            // 获取pageContainer元素的位置
                            var elementPosition = this.getBoundingClientRect();
                            // 计算偏移量
                            var offsetX = e.clientX - elementPosition.left + this.scrollLeft;
                            var offsetY = e.clientY - elementPosition.top + this.scrollTop;
                            // 修改EventPrintPreviewContextMenu事件中参数X，Y表示基于打印预览页面的位置；OriginallyEvent是默认的右键事件的Event【DUWRITER5_0-3907】
                            rootElement.EventPrintPreviewContextMenu(rootElement, {
                                ElementType: null,//svg的打印预览时，无法获取到当前元素的类型。 且e.target可能为子元素text
                                PageElement: e.target,
                                TypeName: "快捷菜单信息",
                                X: offsetX,
                                Y: offsetY,
                                clientX: e.clientX,
                                clientY: e.clientY,
                                OriginallyEvent: e
                            });
                        } else if (typeof (rootElement.SetPreviewContextMenu) == "function") {
                            rootElement.SetPreviewContextMenu(rootElement, {
                                ElementType: null,//svg的打印预览时，无法获取到当前元素的类型。 且e.target可能为子元素text
                                PageElement: e.target,
                                TypeName: "快捷菜单信息",
                                X: e.offsetX,
                                Y: e.offsetY,
                                clientX: e.clientX,
                                clientY: e.clientY
                            });
                        }
                    }
                }
            });
            pageContainer.addEventListener('mousedown', function (e) {
                //在页面中是否存在dcContextMenu
                var hasContextMenu = rootElement.querySelector('#dcContextMenu');
                if (hasContextMenu) {
                    hasContextMenu.remove();
                }
                //判断rootElement.MouseDragScrollMode是否为true，鼠标拖拽滚动
                if (rootElement.MouseDragScrollMode === true) {
                    //对整个编辑器进行绑定
                    pageContainer.removeEventListener('mousemove', mousemoveEvent);
                    pageContainer.addEventListener('mousemove', mousemoveEvent);
                    window.removeEventListener('mouseup', mouseupEvent);
                    window.addEventListener('mouseup', mouseupEvent);

                    function mousemoveEvent(e) {
                        var newX = -(1 * e.movementX) + pageContainer.scrollLeft;
                        var newY = -(1 * e.movementY) + pageContainer.scrollTop;
                        pageContainer.scrollTo(newX, newY);
                    }

                    function mouseupEvent(e) {
                        pageContainer.removeEventListener('mousemove', mousemoveEvent);
                        window.removeEventListener('mouseup', mouseupEvent);
                        mousemoveEvent = null;
                        mouseupEvent = null;
                    }
                    return;
                }
            });
            //用于改善打印预览的滚动体验【DUWRITER5_0-3310】
            pageContainer.addEventListener("wheel", WriterControl_UI.PageContainerOnWheelFunc);
        }
        else {
            while (pageContainer.firstChild != null) {
                pageContainer.removeChild(pageContainer.firstChild);
            }
        }
        pageContainer.style.display = "";
        rootElement.appendChild(pageContainer);

        // 打印预览模式下，需要修复打印预览界面的高度【DUWRITER5_0-2724】
        var hruleNode = rootElement.querySelector("canvas[dctype='hrule']");//获取标尺元素
        if (hruleNode != null && hruleNode.offsetHeight > 0) {
            // 标尺占位时的处理
            pageContainer.style.height = "calc(100% - " + hruleNode.offsetHeight + "px)";//设置高度
        } else {
            pageContainer.style.height = "100%";
        }
        //// xuyiming 添加打印预览前事件【EventBeforePrintPreview】
        //if (rootElement.EventBeforePrintPreview != null && typeof (rootElement.EventBeforePrintPreview) == "function") {
        //    rootElement.EventBeforePrintPreview(rootElement);
        //}
        //如果eleArr存在在此处单独处理对元素page-printpreview元素进行赋值
        if (eleString) {
            pageContainer.innerHTML = eleString;
        } else {
            pageContainer.onscroll = DoPageContainerScroll;
            // 触发准备打印事件
            WriterControl_Event.InnerRaiseEvent(rootElement, "EventPreparePrint", options);
            // 这里返回一个JSON数组，长度是要打印页数的四倍。四个整数一小组，第一个是页码，第二个是页宽，第三个是页高，第四个无意义。
            var strCode = rootElement.__DCWriterReference.invokeMethod(
                "GetPageIndexWidthHeightForPrint",
                true,
                options,
                false);
            if (strCode == null || strCode.length == 0) {
                // 无数据
                return;
            }
            var datas = JSON.parse(strCode);
            //计算出样式区别
            var pageBorderColor = rootElement.getAttribute("pagebordercolor");
            var pagelayoutmode = rootElement.getAttribute('pagelayoutmode');

            var baseZoomRate = rootElement.__DCWriterReference.invokeMethod("get_WASMBaseZoomRate");
            var zoomRate = rootElement.__DCWriterReference.invokeMethod("get_ZoomRate");

            for (var iCount = 0; iCount < datas.length; iCount++) {
                var pageInfo = datas[iCount];
                var element = rootElement.ownerDocument.createElementNS("http://www.w3.org/2000/svg", "svg");
                element.setAttribute("width", pageInfo.Width + "px");
                element.setAttribute("height", pageInfo.Height + "px");
                element.setAttribute("dctype", "page");
                element.setAttribute("PageIndex", pageInfo.PageIndex);
                element.PageIndex = pageInfo.PageIndex;
                element.setAttribute("native-width", pageInfo.Width);
                element.setAttribute("native-height", pageInfo.Height);
                element.__PageInfo = pageInfo;
                element.style.border = `1px solid ${pageBorderColor ? pageBorderColor : "black"}`;
                element.style.backgroundColor = "white";
                element.style.verticalAlign = "top";
                element.style.boxSizing = "content-box";
                //判断编辑器是否为单栏展示,如果是则设置为块级元素并且设置为居中显示
                if (typeof pagelayoutmode == 'string' && pagelayoutmode.trim().toLowerCase() == 'singlecolumn') {
                    element.style.margin = "5px auto";
                    element.style.display = 'block';
                } else {
                    element.style.margin = "5px";
                    element.style.display = 'inline-block';
                }
                WriterControl_Paint.SetPageElementSize(rootElement, element, false, 0 , baseZoomRate, zoomRate);
                //if (bkImg == 1 && strBKImageData != null) {
                //    jQuery(element).addClass(rootElement.__BKImgStyleName)
                //element.style.backgroundImage = "url(" + strBKImageData + ")";
                //}
                element.innerHTML = "<text x='50' y='50'>" + window.__DCSR.Waitting + "</text>";
                element._isRendered = false;
                element.onclick = function (eve) {
                    if (rootElement.__DCDisposed == true) {
                        return;
                    }
                    var strPageList = rootElement.__DCWriterReference.invokeMethod(
                        "WASMPrintPreviewHandleMouseClick",
                        this.__PageInfo.PageIndex,
                        eve.offsetX,
                        eve.offsetY,
                        eve.ctrlKey);
                    var pageIndexs = DCTools20221228.ParseInt32Values(strPageList);
                    if (pageIndexs != null && pageIndexs.length == 2) {
                        for (var node2 = this.parentNode.firstChild; node2 != null; node2 = node2.nextSibling) {
                            var lowNodeName = node2.nodeName.toLowerCase();
                            if (lowNodeName == "canvas" || lowNodeName == "svg") {
                                if (node2.PageIndex >= pageIndexs[0]
                                    && node2.PageIndex <= pageIndexs[1]) {
                                    WriterControl_Print.InnerDrawOnePage(node2, true);
                                    //DCTools20221228.ClearCanvasElementContent(node2);
                                    //node2._isRendered = false;
                                }
                            }
                        }
                        //DoPageContainerScroll.call(this.parentNode);
                    }
                };
                pageContainer.appendChild(element);
            }
            //在此处执行判断是否需要执行区域选择打印
            if (options == null && rootElement.RectInfo && typeof rootElement.RectInfo.printPreviewFun == "function") {
                rootElement.RectInfo.printPreviewFun(pageContainer);
            }
            DoPageContainerScroll.call(pageContainer);
            pageContainer.__DoPageContainerScroll = DoPageContainerScroll;
        }
        // 修复打印预览无法触发EventAfterPrintPreview事件的问题
        if (rootElement.EventAfterPrintPreview != null
            && typeof (rootElement.EventAfterPrintPreview) == "function") {
            rootElement.EventAfterPrintPreview(rootElement);
        }
        // WriterControl_Task.AddCallbackForCompletedAllTasks(function () {
        //     if (rootElement.EventAfterPrintPreview != null
        //         && typeof (rootElement.EventAfterPrintPreview) == "function") {
        //         rootElement.EventAfterPrintPreview(rootElement);
        //     }
        // });
    },

    ///**
    // * 为打印预览而填充页面信息
    // * @param {any} containerID
    // * @param {any} strCodes
    // */
    //FillPagesForPreview: function (containerID, strCodes) {
    //    var rootElement = DCTools20221228.GetOwnerWriterControl(containerID);
    //    if (rootElement == null) {
    //        return null;
    //    }
    //},
    /**
     * 获得打印用的内置框架元素对象
     * @param {string} containerID 容器元素对象
     * @param {boolean} autoCreate 是否自动创建
     * @returns {HTMLIFrameElement} 内置框架对象
     */
    GetIFrame: function (containerID, autoCreate) {
        var rootElement = DCTools20221228.GetOwnerWriterControl(containerID);
        if (rootElement == null) {
            return null;
        }
        var result = rootElement.ownerDocument.getElementById(rootElement.id + "_IFrame_Print");
        if (result == null) {
            if (autoCreate == false) {
                return null;
            }
            result = rootElement.ownerDocument.createElement("iframe");
            result.id = rootElement.id + "_IFrame_Print";
            result.style.position = "absolute";
        }
        rootElement.appendChild(result);
        result.style.width = rootElement.offsetWidth + "px";
        result.style.height = rootElement.offsetHeight + "px";
        result.style.left = "0px";
        result.style.top = "0px";// (rootElement.offsetTop + 600) + "px";
        result.style.border = "1px solid blue";
        result.style.display = "";
        result.style.backgroundColor = "white";
        result.style.zIndex = 10000;
        return result;
    },
    /**
     * 绘制单页内容的内部方法
     * 
     * 此方法负责在指定的元素内绘制单页视图，主要用于打印预览的页面呈现或者打印等功能
     * 
     * @param {HTMLElement} element - 需要在其中绘制页面的DOM元素
     * @param {boolean} bolPrintPreview - 是否获取打印预览的内容
     * @param {Object} oldCtl - 旧的控制元素，用于在更新或重新绘制时进行引用
     */
    InnerDrawOnePage: function (element, bolPrintPreview, oldCtl) {
        var rootElement = DCTools20221228.GetOwnerWriterControl(element);
        if (rootElement == null) {
            if (oldCtl) {
                rootElement = oldCtl;
            } else {
                return;
            }
        }
        rootElement.style.shapeRendering = "crispEdges";
        //if (element.nodeName.toLowerCase() == "svg") {
        // 采用SVG模式进行打印预览
        var strSVG = rootElement.__DCWriterReference.invokeMethod(
            "PaintPageForPrintUseSVG",
            element.PageIndex,
            bolPrintPreview);
        if (strSVG != null) {
            // 为了点击续打时，svg元素没有渲染时需要渲染时重新添加续打蒙版【DUWRITER5_0-3424】
            if (element.JumpMarkNodeStr) {
                strSVG += element.JumpMarkNodeStr;
                delete element.JumpMarkNodeStr;
            }
            element.innerHTML = strSVG;
            element._isRendered = true;
            /** 是否允许选中页眉页脚内容 */
            var isHeaderFooterSelect = rootElement.getAttribute("HeaderFooterSelect");
            if (typeof (isHeaderFooterSelect) == "string" && isHeaderFooterSelect.toLowerCase() == "true") {
                // 设置页眉页脚元素不可选【DUWRITER5_0-4108】
                var HeaderAndFooterNodes = element.querySelectorAll("#divXTextDocumentHeaderForFirstPageElement,#divXTextDocumentHeaderElement,#divXTextDocumentFooterForFirstPageElement,#divXTextDocumentFooterElement");
                for (var iCount = 0; iCount < HeaderAndFooterNodes.length; iCount++) {
                    var node = HeaderAndFooterNodes[iCount];
                    if (node) {
                        node.setAttribute("user-select", "none");
                    }
                }
            }
            var intCount45 = 10;
            for (var node45 = element.lastChild;
                node45 != null && intCount45 > 0;
                node45 = node45.previousSibling, intCount45--) {
                if (node45.nodeName == "rect" && node45.getAttribute("dctype") == "contentheight") {
                    var intContentHeight = parseInt(node45.getAttribute("value"));
                    element.__ContentHeight = intContentHeight;
                    // 修复SVG打印时每一页多出一页的问题【DUWRITER5_0-3320】
                    // svg打印时不需要增加高度
                    // if (bolPrintPreview == false) {
                    //     // 不是打印预览，而是打印
                    //     element.setAttribute("height", (intContentHeight + 2) + "px");
                    // }
                    break;
                }
            }
        }
        return;
        //}
        //if (bolPrintPreview == false ) {
        //    // 处于打印模式下，用全白色填充背景
        //    var ctx = element.getContext("2d");
        //    ctx.fillStyle = "white";
        //    ctx.fillRect(0, 0, element.width, element.height);
        //}
        //var pageInfo = element.__PageInfo;
        //if (pageInfo != null && pageInfo.PageSpan != null && pageInfo.PageSpan.length > 1) {
        //    // 拼接打印
        //    var zoomRate = rootElement.__DCWriterReference.invokeMethod("get_ZoomRate");
        //    var baseZoomRate = rootElement.__DCWriterReference.invokeMethod("get_WASMBaseZoomRate");
        //    if (bolPrintPreview == false) {
        //        baseZoomRate = 1;
        //        zoomRate = 1;
        //    }
        //    var pageSpan = pageInfo.PageSpan;
        //    var tempElement = WriterControl_Print.__TempCanvasElement;
        //    if (tempElement == null) {
        //        tempElement = rootElement.ownerDocument.createElement("canvas");
        //        WriterControl_Print.__TempCanvasElement = tempElement;
        //    }
        //    tempElement.width = element.width * zoomRate * baseZoomRate;
        //    tempElement.height = pageSpan[0] * zoomRate * baseZoomRate;
        //    for (var iCount = 1; iCount < pageSpan.length; iCount++) {
        //        var pi = pageSpan[iCount];
        //        var drawer = new PageContentDrawer(tempElement);
        //        drawer.PageIndex = pi;
        //        drawer.TopPos = pageSpan[0] * (iCount - 1) * zoomRate * baseZoomRate;
        //        drawer.EventQueryCodes = function () {
        //            if (rootElement.__DCDisposed == true) {
        //                return null;// 控件已经被销毁了，无法打印
        //            }
        //            DCTools20221228.ClearCanvasElementContent(tempElement);
        //            return rootElement.__DCWriterReference.invokeMethod(
        //                "PaintPageForPrint",
        //                this.PageIndex,
        //                bolPrintPreview);
        //        };
        //        drawer.EventAfterDraw = function () {
        //            if (element._isRendered == true) {
        //                DCTools20221228.ClearCanvasElementContent(element);
        //            }
        //            var ctx = element.getContext("2d");
        //            ctx.drawImage(tempElement, 0, this.TopPos);
        //            DCTools20221228.ClearCanvasElementContent(tempElement);
        //            element._isRendered = true;
        //        };
        //        drawer.AddToTask();
        //    }
        //}
        //else {
        //    var drawer = new PageContentDrawer(element, null);
        //    drawer.PageIndex = element.PageIndex;
        //    drawer.EventQueryCodes = function (drawer2) {
        //        // 获得该页面的绘图代码
        //        var strCodePage = rootElement.__DCWriterReference.invokeMethod(
        //            "PaintPageForPrint",
        //            drawer2.PageIndex,
        //            bolPrintPreview);
        //        if (this.CanvasElement._isRendered == true) {
        //            DCTools20221228.ClearCanvasElementContent(this.CanvasElement);
        //        }
        //        this.CanvasElement._isRendered = true;
        //        return strCodePage;
        //    };
        //    drawer.AddToTask();
        //}
    },
    /**
     * 打印
     * @param {string | HTMLElement} containerID 容器元素编号
     * @param {any} options 打印参数
     * @returns {boolean} 操作是否成功，但打印是异步操作，函数虽然返回，但打印还在继续。
     */
    Print: function (containerID, options) {
        //console.log('print');
        var rootElement = DCTools20221228.GetOwnerWriterControl(containerID);
        if (rootElement == null) {
            return false;
        }
        //自定义属性用于保存是否在打印预览设置
        var printSelectionMask = false;
        if (options && options.PrintRange == "Selection") {
            if (rootElement.__DCWriterReference.invokeMethod("HasSelection") === false) {
                if (rootElement.IsPrintPreview() == false) {
                    console.log("编辑器未选中数据");
                    return;
                } else {
                    var selection = rootElement.ownerDocument.getSelection();
                    if (selection.type != "Range") {
                        console.log("编辑器未选中数据");
                        return;
                    } else {
                        //在此处将options的PrintRange: "Selection"拆出已保证数据的准确
                        printSelectionMask = true;
                        options.PrintRange = undefined;
                    }
                }
            }
            // 在此处修改页眉下划线配置
            WriterControl_Print.oldShowHeaderBottomLine = rootElement.DocumentOptions.ViewOptions.ShowHeaderBottomLine;
            if (WriterControl_Print.oldShowHeaderBottomLine === true) {
                rootElement.DocumentOptions.ViewOptions.ShowHeaderBottomLine = false;
                rootElement.ApplyDocumentOptions();
            }
        }
        var iframe = WriterControl_Print.GetIFrame(containerID, true);
        iframe.style.display = 'none';

        if (iframe == null) {
            return false;
        }

        // var bkImage = null;
        // if (rootElement.__WaterMarkData != null && rootElement.__WaterMarkData.length > 0) {
        //     bkImage = rootElement.ownerDocument.createElement("img");
        //     bkImage.src = rootElement.__WaterMarkData;
        // }
        var targetDocument = iframe.contentDocument;
        targetDocument.open();
        targetDocument.write("");
        targetDocument.close();
        var previewContainer = DCTools20221228.GetChildNodeByDCType(rootElement, "page-printpreview");
        var isFromPrintPreview = previewContainer != null && DCTools20221228.IsNullOrEmptyString(previewContainer.style.display);
        if (isFromPrintPreview == true) {
            // 从打印预览开始进行打印
        } else {
            // 从文档编辑状态开始进行打印，触发准备打印事件
            WriterControl_Event.InnerRaiseEvent(rootElement, "EventPreparePrint", options);
        }
        var strCode = null;
            // 编辑器控件
            if (isFromPrintPreview == true) {
                strCode = rootElement.__DCWriterReference.invokeMethod("GetPageIndexWidthHeightForPrintFromPrintPreview", options);
            }
            else {
                strCode = rootElement.__DCWriterReference.invokeMethod("GetPageIndexWidthHeightForPrint", false, options, false);
            }
        if (strCode == null || strCode.length == 0) {
            // 没有获得任何数据
            return;
        }
        var datas = JSON.parse(strCode);
        var div = targetDocument.createElement("DIV");
        targetDocument.body.appendChild(div);
        var strPageStyle = datas.shift();// 页面样式
        var divZoom = datas.shift();// 缩放比例
        //div.style.zoom = divZoom; // 这里缩小显示被放大的内容，用于改进打印输出的精细度。
        //div.style.transform = "scale(" + divZoom + ")";// 用于兼容Firefox
        var styleElement = targetDocument.createElement("STYLE");
        styleElement.innerText = strPageStyle;
        targetDocument.head.appendChild(styleElement);

        if (rootElement.CustomFontFamily) {
            DCTools20221228.LoadCustomFont(rootElement, true, targetDocument.head)
        }

        targetDocument.body.style.margin = "0px";
        targetDocument.title = " ";
        // 获得实际打印输出的页码列表
        // var isFirstPage = true;
        //console.log('pageCount')
        for (var iCount = 0; iCount < datas.length; iCount++) {
            var pageInfo = datas[iCount];
            // if (isFirstPage == false) {
            //     div.appendChild(targetDocument.createElement("BR"));
            // } else {
            //     isFirstPage = false;
            // }
            //var element = targetDocument.createElement("CANVAS");
            var element = targetDocument.createElementNS("http://www.w3.org/2000/svg", "svg");
            //var element = rootElement.ownerDocument.createElement("CANVAS");
            //if (element.nodeName.toLowerCase() == "svg") {
            //element.setAttribute("xmlns", "http://www.w3.org/2000/svg");
            element.setAttribute("width", pageInfo.Width + "px");
            element.setAttribute("height", pageInfo.Height + "px");
            //}
            element.__PageInfo = pageInfo;
            element.PageIndex = pageInfo.PageIndex;
            //element.width = pageInfo.Width;
            //element.height = pageInfo.Height;
            //var bolBKImg = datas[iCount * 4 + 3];
            //element.style.pageBreakAfter = "always";
            element.style.pageBreakInside = "avoid";
            element.style.display = "block";
            //element.style.border = "1px solid black";
            div.appendChild(element);
            WriterControl_Print.InnerDrawOnePage(element, false);
        }//for
        //返回打印的html
        if (rootElement != null
            && rootElement.EventBeforePrintToGetHtml != null
            && typeof rootElement.EventBeforePrintToGetHtml == 'function') {
            rootElement.PrintAsHtml(null, null, {
                isPrint: true,
                printCallBack: function (html, printFun) {
                    rootElement.EventBeforePrintToGetHtml(html);
                }
            });
        }
        // 打印先渲染页面展示，再进行打印【DUWRITER5_0-3379】
        if (isFromPrintPreview == false) {
            rootElement.__DCWriterReference.invokeMethod("RefreshViewAfterPrint", true);
        }
        // 在此处将是否需要页眉下划线修改回来
        if (WriterControl_Print.oldShowHeaderBottomLine === true) {
            rootElement.DocumentOptions.ViewOptions.ShowHeaderBottomLine = true;
            rootElement.ApplyDocumentOptions();
        }
        var printFun = function () {
            if (options == null && rootElement.RectInfo && typeof rootElement.RectInfo.printPreviewFun == "function") {
                rootElement.RectInfo.printPreviewFun(div, null, "print");
            }
            if (options != null && typeof (options.CompletedCallback) == "function") {
                iframe.contentWindow.onafterprint = function (e) {
                    var divWASM = rootElement.ownerDocument.querySelectorAll("[dctype='WriterControlForWASM']");
                    if (divWASM && divWASM.length > 0) {
                        divWASM = Array.from(divWASM);
                        for (var i = 0; i < divWASM.length; i++) {
                            divWASM[i].RefreshInnerView();
                        }
                    }
                    // !! 这里有个问题，用户按下取消，本事件也会触发
                    // 执行打印完毕事件
                    options.CompletedCallback.call(rootElement, rootElement);
                    iframe.contentDocument.close();
                    iframe.style.display = "none";
                    // 修改打印后销毁打印iframe，DebugMode="true"进入调试模式时不销毁【DUWRITER5_0-3554】
                    if (rootElement && rootElement.getAttribute("DebugMode") != "true") {
                        DCTools20221228.destroyIframe(iframe);
                    }
                };
            } else {
                // 修改打印后销毁打印iframe，DebugMode="true"进入调试模式时不销毁【DUWRITER5_0-3554】
                if (rootElement && rootElement.getAttribute("DebugMode") != "true") {
                    iframe.contentWindow.onafterprint = function (e) {
                        DCTools20221228.destroyIframe(iframe);
                    };
                }
            }
            //在此处处理打印前事件
            if (rootElement != null && rootElement.EventBeforePrint != null
                && typeof rootElement.EventBeforePrint == 'function') {
                rootElement.EventBeforePrint(targetDocument);
            }
            // 所有绘图任务完成，进行打印
            // 获取iframe内部文档
            var iframeDoc = iframe.contentDocument || iframe.contentWindow.document;
            // 确保svg图片元素都渲染完成才进行打印，防止某些图片无法正常显示【DUWRITER5_0-3554】
            // 获取所有的SVG image元素和html的img元素
            const images = iframeDoc.querySelectorAll("svg image, img");
            if (images != null && images.length > 0) {
                // 定义一个变量来记录加载完成的图片数量
                let loadedImages = 0;
                // 为每个image元素的图片资源添加load,error事件监听器
                var CompleteFunc = function () {
                    // 当图片加载完成时，执行的操作
                    loadedImages++; // 增加加载完成的图片数量
                    if (loadedImages === images.length) {
                        // 当所有图片都加载完成时，执行的操作
                        // console.log('所有图片都加载完成');
                        if (printSelectionMask) {
                            WriterControl_Print.PrintSelectionMask(rootElement, iframeDoc);
                        }
                        iframe.contentWindow.print();
                    }
                };
                images.forEach(function (image) {
                    let img;
                    if (image.nodeName == "IMG") {
                        img = image;
                    } else {
                        const href = image.href.baseVal; // 获取image元素引用的图片URL
                        img = new Image(); // 创建一个新的Image对象
                        img.src = href; // 设置Image对象的src属性
                    }
                    img.onload = CompleteFunc;
                    img.onerror = CompleteFunc;
                    // 检查图片是否已经加载完成
                    if (img.complete && typeof (CompleteFunc) == "function") {
                        CompleteFunc();
                    }
                });
            } else {
                if (printSelectionMask) {
                    WriterControl_Print.PrintSelectionMask(rootElement, iframeDoc);
                }
                window.setTimeout(function () { iframe.contentWindow.print(); }, 1000);
                //iframe.contentWindow.print();
            }
        };
        // 如果有任务正在运行，则等待任务完成后再打印
        // 因为之前是CANVAS，需要绘制，默认有任务需要处理，SVG是同步的无需等待
        // 修复打印预览模式下时打印不直接出来的问题
        if (WriterControl_Task.__Tasks && WriterControl_Task.__Tasks.length > 0) {
            WriterControl_Task.AddCallbackForCompletedAllTasks(printFun);
        } else {
            printFun();
        }
        return true;
    }
};