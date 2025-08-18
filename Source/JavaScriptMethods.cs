using System;
using DCSoft.Writer.Controls;
using Microsoft.JSInterop;
using DCSoft.Common;
using System.Collections.Generic;
using System.Runtime.InteropServices.JavaScript;
using System.Text.Json;

namespace DCSoft.WASM
{
    /// <summary>
    /// JS中定义的函数的转发器
    /// </summary>
    [System.Runtime.Versioning.SupportedOSPlatform("browser")]
    internal partial class JavaScriptMethods
    {
        private static readonly Dictionary<string, System.Reflection.MethodInfo> _PackageMethods = new Dictionary<string, System.Reflection.MethodInfo>();
        public static object CreateDotNetObjectReference(object obj, string typeName)
        {
            if (obj == null || typeName == null)
            {
                return null;
            }
            System.Reflection.MethodInfo m = null;
            if (_PackageMethods.TryGetValue(typeName, out m) == false)
            {
                var methodBase = typeof(DotNetObjectReference).GetMethod("Create");
                m = methodBase.MakeGenericMethod(obj.GetType());
                _PackageMethods[typeName] = m;
            }
            if (m != null)
            {
                return m.Invoke(null, new object[] { obj });
            }
            else
            {
                return null;
            }
        }

        // 定义前端JS函数的名称
        public const string JSNAME_WriterControl_Event_HandleDCWriterInnerEvent = "WriterControl_Event.HandleDCWriterInnerEvent";
        public const string JSNAME_DCTools20221228_DecodeString = "DCTools20221228.DecodeString";

        public const string JSNAME_DCTools20221228_LogTick = "DCTools20221228.LogTick";
        public const string JSNAME_DCTools20221228_HttpSend = "DCTools20221228.HttpSend";
        public const string JSNAME_WriterControl_UI_ReloadHostControls = "WriterControl_UI.ReloadHostControls";
        public const string JSNAME_WriterControl_UI_BeginEditValueUseListBoxControl = "WriterControl_UI.BeginEditValueUseListBoxControl";
        public const string JSNAME_WriterControl_UI_RaiseControlEvent = "WriterControl_Event.RaiseControlEvent";
        internal const string MODULE_WriterControl_UI = "WriterControl_UI";
        internal const string MODULE_WriterControl_Main = "WriterControl_Main";
        internal const string MODULE_WriterControl_Paint = "WriterControl_Paint";
        internal const string MODULE_WriterControl_IO = "WriterControl_IO";
        internal const string MODULE_WriterControl_Rule = "WriterControl_Rule";
        internal const string MODULE_DCTools20221228 = "DCTools20221228";
        internal const string MODULE_WriterControl_Event = "WriterControl_Event";
        internal const string MODULE_WriterControl_Dialog = "WriterControl_Dialog";
        internal const string MODULE_WriterControl_DOMPackage = "WriterControl_DOMPackage";
        internal const string MODULE_WriterControl_FontList = "WriterControl_FontList";
        /// <summary>
        /// 存储JS的根目录名
        /// </summary>
        internal static string _JS_RootPath = "./";
        private static async System.Threading.Tasks.Task ImportOneJsModule(string name)
        {
            string strUrl = null;
            if (_JS_RootPath.IndexOf("{0}") > 0)
            {
                strUrl = _JS_RootPath.Replace("{0}", name + ".js");
            }
            else
            {
                strUrl = _JS_RootPath + name + ".js";// WriterControlForWASM.GetJSAbsoluteUrl(name);
            }
            await System.Runtime.InteropServices.JavaScript.JSHost.ImportAsync(name, strUrl);
        }
        public static async System.Threading.Tasks.Task ImportJSModuleAsync(string strBasePath)
        {
            if (strBasePath != null && strBasePath.Length > 0)
            {
                if (strBasePath.IndexOf('?') > 0 && strBasePath.IndexOf("wasmres=") > 0)
                {
                    _JS_RootPath = strBasePath;
                }
                else
                {

                }
            }
            await ImportOneJsModule(MODULE_WriterControl_Main);
            await ImportOneJsModule(MODULE_WriterControl_UI);
            await ImportOneJsModule(MODULE_WriterControl_Paint);
            await ImportOneJsModule(MODULE_WriterControl_Rule);
            await ImportOneJsModule(MODULE_WriterControl_Event);
            await ImportOneJsModule(MODULE_WriterControl_Dialog);
            await ImportOneJsModule(MODULE_DCTools20221228);
            await ImportOneJsModule(MODULE_WriterControl_DOMPackage);
            await ImportOneJsModule(MODULE_WriterControl_IO);
            await ImportOneJsModule(MODULE_WriterControl_FontList);
            DCSoft.DCConsole.Default = new MyConsole();
        }

        private class MyConsole : DCSoft.DCConsole
        {
            public override void WriteLine(string value)
            {
                if (value != null && value.Length > 0)
                {
                    ConsoleWriteLine(value);
                }
            }
            public override void WriteLineError(string value)
            {
                if (value != null && value.Length > 0)
                {
                    ConsoleWarring(value);
                }
            }
        }

        public static DateTime GetNowDateTime()
        {
            return WriterControl.GetServerTime();
        }

        [JSImport(MODULE_WriterControl_FontList + ".GetFontSnapshort", MODULE_WriterControl_FontList)]
        internal static partial string GetFontSnapshort( string strKey);

        [JSImport("WriterControl_FontList.GetFontFileName", MODULE_WriterControl_FontList)]
        internal static partial string GetFontFileName(string strKey);

        [JSImport("WriterControl_FontList.GetFontInfoKeyName", MODULE_WriterControl_FontList)]
        internal static partial string GetFontInfoKeyName(string strName, bool bolBold, bool bolItalic);


        [JSImport("WriterControl_FontList.GetFontNames", MODULE_WriterControl_FontList)]
        internal static partial string[] GetFontNames();

        [JSImport("WriterControl_IO.PrepareInnerXmlReader", MODULE_WriterControl_IO)]
        internal static partial bool IO_PrepareInnerXmlReader(string strContainerID , string strXml, bool bolCheckDCWriterDocumentXmlHeader);

        [JSImport("WriterControl_Main.InitDefaultResourceStrings", MODULE_WriterControl_Main)]
        internal static partial void InitDefaultResourceStrings();

        [JSImport("WriterControl_Main.StartGlobal", MODULE_WriterControl_Main)]
        internal static partial void StartGlobal();

        [JSImport("DCTools20221228.PackageBigStringValue", MODULE_DCTools20221228)]
        internal static partial string PackageBigStringValue(string strValue);

        [JSImport("DCTools20221228.ConsoleWriteLine", MODULE_DCTools20221228)]
        internal static partial void ConsoleWriteLine(string txt);

        [JSImport("DCTools20221228.ConsoleWarring", MODULE_DCTools20221228)]
        internal static partial void ConsoleWarring(string txt);


        /// <summary>
        /// 报告系统异常
        /// </summary>
        /// <param name="strSourceName">错误来源</param>
        /// <param name="strExceptionMessage">消息文本</param>
        /// <param name="strExceptionString">详细信息</param>
        /// <param name="intLevel">错误等级</param>
        [JSImport("DCTools20221228.ReportException", MODULE_DCTools20221228)]
        internal static partial void Tools_ReportException(string strSourceName, string strExceptionMessage, string strExceptionString, int intLevel);


        [JSImport("WriterControl_DOMPackage.IsSupport", MODULE_WriterControl_DOMPackage)]
        internal static partial bool DOMPackage_IsSupport(string strTypeName);


        [JSImport("DCTools20221228.IsSupportFontName", MODULE_DCTools20221228)]
        internal static partial bool Tools_IsSupportFontName(string strFontName);


        [JSImport("WriterControl_UI.IsAPILogRecord", MODULE_WriterControl_UI)]
        internal static partial bool UI_IsAPILogRecord(string strContainerID);


        [JSImport("WriterControl_UI.SetToolTip", MODULE_WriterControl_UI)]
        internal static partial void UI_SetToolTip(string strContainerID, string strText);



        [JSImport("WriterControl_UI.RaiseEventUpdateToolarState", MODULE_WriterControl_UI)]
        internal static partial void UI_RaiseEventUpdateToolarState(string strContainerID);


        [JSImport("WriterControl_UI.ShowCalculateControl", MODULE_WriterControl_UI)]
        internal static partial void UI_ShowCalculateControl(string strContainerID, int intPageIndex, int intLeft, int intTop, int intHeight, double inputValue);


        [JSImport("WriterControl_UI.ShowDateTimeControl", MODULE_WriterControl_UI)]
        internal static partial void UI_ShowDateTimeControl(string strContainerID, int intPageIndex, int intLeft, int intTop, int intHeight, [JSMarshalAs<JSType.Date>] global::System.DateTime inputValue, int intStyle);

        [JSImport("WriterControl_UI.JSShowColorControl", MODULE_WriterControl_UI)]
        internal static partial void UI_JSShowColorControl(string strContainerID, string strDefaultValue);

        [JSImport("WriterControl_UI.SetElementCursor", MODULE_WriterControl_UI)]
        internal static partial void UI_SetElementCursor(string strContainerID, int intPageIndex, string strCursor);


        [JSImport("WriterControl_UI.ShowMessageBox", MODULE_WriterControl_UI)]
        internal static partial string UI_ShowMessageBox(string strText, string strCaption, string strButtons, string strIcon, string strDefaultButton);


        [JSImport("WriterControl_UI.WindowPrompt", MODULE_WriterControl_UI)]
        internal static partial string UI_WindowPrompt(string strTitle, string strDefaultValue);

        [JSImport("WriterControl_UI.ShowCaret", MODULE_WriterControl_UI)]
        internal static partial void UI_ShowCaret(string strContainerID, int intPageIndex, int intDX, int intDY, int intWidth, int intHeight, bool bolVisible, bool bolReadonly, bool bolScrollToView);

        [JSImport("WriterControl_UI.APILogRecordDebugPrint", MODULE_WriterControl_UI)]
        internal static partial void UI_APILogRecordDebugPrint(string strContainerID, string msg );

        [JSImport("WriterControl_Event.HasControlEvent", MODULE_WriterControl_Event)]
        internal static partial bool UI_HasControlEvent(
            string strContainerID,
            string strEventName);

        [JSImport("WriterControl_UI.CloseDropdownControl", MODULE_WriterControl_UI)]
        internal static partial void UI_CloseDropdownControl(string strContainerID);

        [JSImport("WriterControl_UI.QueryDeleteRowOfColumns", MODULE_WriterControl_UI)]
        internal static partial void UI_QueryDeleteRowOfColumns(string strContainerID);


        [JSImport("WriterControl_UI.IsDropdownControlVisible", MODULE_WriterControl_UI)]
        internal static partial bool UI_IsDropdownControlVisible(string strContainerID);

        //******************************************************************

        [JSImport("WriterControl_Paint.IsStandardImageListReady", MODULE_WriterControl_Paint)]
        internal static partial bool Paint_IsStandardImageListReady();



        [JSImport("WriterControl_Paint.CloseReversibleShape", MODULE_WriterControl_Paint)]
        internal static partial bool Paint_CloseReversibleShape(string strContainerID);

        [JSImport("WriterControl_Paint.ReversibleDraw", MODULE_WriterControl_Paint)]
        internal static partial bool Paint_ReversibleDraw(string strContainerID, int intPageIndex, int x1, int y1, int x2, int y2, int type);

        [JSImport("WriterControl_Paint.UpdatePages", MODULE_WriterControl_Paint)]
        internal static partial bool Paint_UpdatePages(string strContainerID, string strPageCodes);

        [JSImport("WriterControl_Paint.NeedUpdateView", MODULE_WriterControl_Paint)]
        internal static partial bool Paint_NeedUpdateView(string strContainerID, bool bolValue);

        [JSImport("WriterControl_Rule.SetRuleVisible", MODULE_WriterControl_Rule)]
        internal static partial bool Rule_SetRuleVisible(string strContainerID, bool bolVisible);

        [JSImport("WriterControl_Rule.InvalidateView", MODULE_WriterControl_Rule)]
        internal static partial bool Rule_InvalidateView(string strContainerID, string strCtlName);
        [JSImport("WriterControl_UI.IsPrintPreview", MODULE_WriterControl_UI)]
        internal static partial bool UI_IsPrintPreview(string strContainerID);
    }
}
