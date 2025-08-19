global using Microsoft.JSInterop;

using DCSoft.Writer.Controls;
using DCSoft.Common;
using System.Collections.Generic;
using DCSoft.Writer.Dom;
using DCSoft.Writer;
using DCSoft.Drawing;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.ComponentModel;
using DCSoft.Writer.Commands;
using System.Windows.Forms;


namespace DCSoft.WASM
{
    [System.Runtime.Versioning.SupportedOSPlatform("browser")]
    [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = false)]
    public partial class WriterControlForWASM : System.IDisposable
    {
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = false)]
        [DefaultValue(false)]
        public bool ForceCopyDragContent { get; set; }

        private static WriterControlForWASM _Current = null;
        public static WriterControlForWASM Current
        {
            get
            {
                return _Current;
            }
        }

        public WriterControlForWASM(string strID = null)
        {
            DCConsole.Default.WriteLine(DCSR.BeginLoadWriterControlForWASM + DateTimeCommon.GetNow().ToString("HH:mm:ss.fff"));
            var tick = Environment.TickCount;
            this._ContainerElementID = strID;
            this._Control = new MyWriterControlWASM(this);// DCSoft.Writer.Controls.WriterControl();
            this._Control.Name = strID;
            this._Control.StartForWASM(this);
            WASMEnvironment.SetJSProivder(this);
            tick = Math.Abs(Environment.TickCount - tick);
            DCConsole.Default.WriteLine(DCSR.EndLoadWriterControlForWASM + tick);
        }

        [JSInvokable]
        public virtual DotNetObjectReference<DomElement> GetElementByHashCode(int hashCode)
        {
            var element = this.Document?.GetElementByHashCode(hashCode);
            if (element == null)
            {
                return null;
            }
            else
            {
                return DotNetObjectReference.Create<DomElement>(element);
            }
        }

        /// <summary>
        /// 获得编辑器程序版本号
        /// </summary>
        /// <returns>版本号字符串</returns>
        [JSInvokable]
        public string GetDCWriterVersion()
        {
            return DCSystemInfo.PublishDateString;
        }

        [JSInvokable]
        public void BeginPlayAPILogRecord()
        {
            this._IsHashCodeAsNativeHandle = true;
            this._ElementNativeHandles?.Clear();
        }

        /// <summary>
        /// 获得指定编号的元素的在客户端的排版信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [JSInvokable]
        public JsonObject GetElementLayoutInfoByID(string id)
        {
            if (id == null || id.Length == 0)
            {
                return null;
            }
            WriterViewControl.WASMClientPosInfo info = null;
            var ctl = this._Control.GetInnerViewControl();
            var ele = ctl.Document.GetElementById(id);
            if (ele != null)
            {
                info = ctl.GetClientPosInfo(ele);
            }
            if (info != null)
            {
                var result = new JsonObject();
                result.Add("PageIndex", info.PageIndex);
                result.Add("Left", info.dx);
                result.Add("Top", info.dy);
                result.Add("Width", info.Width);
                result.Add("Height", info.Height);
                return result;
            }
            return null;
        }
        /// <summary>
        /// 判断系统是否支持指定的字体
        /// </summary>
        /// <param name="strFontName">字体名称</param>
        /// <returns>是否支持</returns>
        public bool JSIsSupportFontName(string strFontName)
        {
            return DCSoft.TrueTypeFontSnapshort.Support(strFontName);
        }

        private bool _IsHashCodeAsNativeHandle = false;
        /// <summary>
        /// 设置是否使用文档元素的HashCode作为原始句柄
        /// </summary>
        /// <param name="bolValue"></param>
        [JSInvokable]
        public void SetHashCodeAsNativeHandle(bool bolValue)
        {
            this._IsHashCodeAsNativeHandle = bolValue;
        }
        [JSInvokable]
        public void FocusElementByNativeHandle(int handle)
        {
            try
            {
                var element = this.GetElementByNativeHandle(handle);// ._ElementNativeHandles.GetInstance(handle);
                if (element != null)
                {
                    element.Focus();
                }
            }
            catch (Exception ex)
            {
                JavaScriptMethods.Tools_ReportException("FocusElementByNativeHandle", ex.Message, ex.StackTrace.ToString(), 0);
                return;
            }
        }
        private WASMInstanceHandleContainer<DomElement> _ElementNativeHandles = new WASMInstanceHandleContainer<DomElement>();
        /// <summary>
        /// 根据原始句柄获得文档元素对象的JS引用
        /// </summary>
        /// <param name="handle">句柄对象</param>
        /// <returns>获得的文档元素对象的JS引用</returns>
        [JSInvokable]
        public DotNetObjectReference<DomElement> GetElementRefByNativeHandle(int handle)
        {
            try
            {
                var e = this.GetElementByNativeHandle(handle);// ._ElementNativeHandles.GetInstance(handle);
                if (e == null)
                {
                    return null;
                }
                else
                {
                    return DotNetObjectReference.Create<DomElement>(e);
                }
            }
            catch (Exception ex)
            {
                JavaScriptMethods.Tools_ReportException("GetElementRefByNativeHandle", ex.Message, ex.StackTrace.ToString(), 0);
                return null;
            }
        }
        /// <summary>
        /// 根据原始句柄获得文档元素对象的JS引用
        /// </summary>
        /// <param name="handle">句柄对象</param>
        /// <returns>获得的文档元素对象的JS引用</returns>
        public DomElement GetElementByNativeHandle(int handle)
        {
            //if (this._IsHashCodeAsNativeHandle)
            //{
            //    return this.Document.GetElementByHashCode(handle);
            //}
            //else
            {
                return this._ElementNativeHandles.GetInstance(handle);
            }
        }

        /// <summary>
        /// 获得文档元素对象的原始句柄
        /// </summary>
        /// <param name="element">文档元素对象</param>
        /// <returns>句柄对象</returns>
        public int GetElementNativeHandle(DomElement element)
        {
            if (element == null)
            {
                return -1;
            }
            //if (this._IsHashCodeAsNativeHandle)
            //{
            //    return element.GetHashCode();
            //}
            //else
            {
                return this._ElementNativeHandles.GetHandle(element);
            }
        }
        /// <summary>
        /// 删除原始句柄相关的信息，清理内存。
        /// </summary>
        [JSInvokable]
        public void ClearNativeHandleInfo()
        {
            this._ElementNativeHandles?.Clear();
        }
        /// <summary>
        /// 获得编辑器状态版本号,用户对文档的任何编辑和移动光标的操作都会修改版本号。
        /// </summary>
        /// <returns>版本号</returns>
        [JSInvokable]
        public int GetEditStateVersion()
        {
            try
            {
                var str = new System.Text.StringBuilder();
                str.Append(this._Control.Document.ContentVersion.ToString());
                str.Append(',');
                str.Append(this._Control.Document.CurrentContentPartyStyle.ToString());
                str.Append(',');
                var sel = this._Control.Document.Selection;
                str.Append(sel.StateVersion);
                var str2 = str.ToString();
                return str2.GetHashCode();
            }
            catch (Exception ex)
            {
                JavaScriptMethods.Tools_ReportException("GetEditStateVersion", ex.Message, ex.StackTrace.ToString(), 0);
                return -1;
            }
        }

        [JSInvokable]
        public void CheckForLoadDefaultDocument()
        {
            try
            {
                this._Control.GetInnerViewControl().CheckForLoadDefaultDocument();
            }
            catch (Exception ex)
            {
                JavaScriptMethods.Tools_ReportException("CheckForLoadDefaultDocument", ex.Message, ex.StackTrace.ToString(), 0);
                return;
            }
        }

        [JSInvokable]
        public string GetRuntimeFileName(string specifyFileName)
        {
            try
            {
                return WriterUtils.GetRuntimeFileName(specifyFileName, this._Control.Document);
            }
            catch (Exception ex)
            {
                JavaScriptMethods.Tools_ReportException("GetRuntimeFileName", ex.Message, ex.StackTrace.ToString(), 0);
                return "";
            }
        }

        private string _DisposedIDBack = null;
        /// <summary>
        /// 在所有公开的API函数的第一行都需要调用本函数来进行程序状态的判断和调整。
        /// </summary>
        /// <exception cref="ObjectDisposedException"></exception>
        internal void CheckStateBeforeInvoke()
        {
            if (this._IsDisposed)
            {
                throw new ObjectDisposedException("WriterControl:" + this._DisposedIDBack);
            }
            WASMEnvironment.SetJSProivder(this);
        }

        private bool _IsDisposed = false;
        /// <summary>
        /// 对象是否被销毁了
        /// </summary>
        /// <returns></returns>
        public bool IsDisposed()
        {
            return this._IsDisposed;
        }
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        [JSInvokable]
        public void Dispose()
        {
            try
            {
                if (this._DocumentStatsBackForPrint != null)
                {
                    foreach (var item in this._DocumentStatsBackForPrint)
                    {
                        item.Value.Dispose();
                    }
                    this._DocumentStatsBackForPrint.Clear();
                    this._DocumentStatsBackForPrint = null;
                }
                this.ClearCachedDragData();
                if (this._ReferenceAtJS != null)
                {
                    this._ReferenceAtJS.Dispose();
                    this._ReferenceAtJS = null;
                }
                if (this._Control != null)
                {
                    this._Control.Dispose();
                    this._Control = null;
                }
                if (this._ElementNativeHandles != null)
                {
                    this._ElementNativeHandles.Dispose();
                    this._ElementNativeHandles = null;
                }
                this._CurrentCallBack = null;
                this._DocumentBackupForPrint = null;
                this._ElementNativeHandles = null;
                this._NotSupportFontNames = null;
                this._OptionsBackForPrint = null;
                this._ReferenceAtJS = null;
                this._IsDisposed = true;
                this._DisposedIDBack = this._ContainerElementID;
                DCConsole.Default.WriteLine(string.Format(DCSR.ControlDisposed_ID, this._ContainerElementID));
                this._ContainerElementID = null;
                var time = DateTime.Now.Second;
                if (Math.Abs(time - _LastGCTime) > 2)
                {
                    GC.Collect();
                    _LastGCTime = time;
                }
            }
            catch (Exception ex)
            {
                JavaScriptMethods.Tools_ReportException("Dispose", ex.Message, ex.StackTrace.ToString(), 0);
                return;
            }
        }
        private static int _LastGCTime = 0;
        [JSInvokable]
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public string GetProductVersion()
        {
            return DCSoft.DCSystemInfo.PublishDateString;
        }

        [JSInvokable]
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public int TestMethod(string[] strTable, byte[] strIndexs)
        {
            return strTable.Length;
        }
        /// <summary>
        /// 处理编辑器内部事件
        /// </summary>
        /// <param name="eventName">事件名称</param>
        /// <param name="args">事件参数</param>
        /// <returns>是否处理完毕事件而无需后续处理</returns>
        public bool HandleDCWriterInnerEvent(string eventName, object arg)
        {
            return this.JS_InvokeInstance<bool>(
                JavaScriptMethods.JSNAME_WriterControl_Event_HandleDCWriterInnerEvent,
                eventName,
                arg);
        }

        public bool HasControlEvent(string eventName)
        {
            return JavaScriptMethods.UI_HasControlEvent(this._ContainerElementID, eventName);
        }

        public void RaiseControlEvent(string eventName, object args)
        {
            if (args is JsonObject)
            {
                this.JS_InvokeInstance(
                    JavaScriptMethods.JSNAME_WriterControl_UI_RaiseControlEvent,
                    eventName,
                    args,
                    null);
            }
            else
            {
                var typeName = args == null ? null : args.GetType().Name;
                bool bolPackage = false;
                if (JavaScriptMethods.DOMPackage_IsSupport(typeName))
                {
                    bolPackage = true;
                    args = JavaScriptMethods.CreateDotNetObjectReference(args, typeName);
                }
                this.JS_InvokeInstance(
                    JavaScriptMethods.JSNAME_WriterControl_UI_RaiseControlEvent,
                    eventName,
                    args,
                    typeName);
                if (bolPackage)
                {
                    ((System.IDisposable)args).Dispose();
                }
            }
        }


        //wyc20250115:DUWRITER5_0-4090
        /// <summary>
        /// 日期时间格式化字符串时的英文是否使用大写
        /// </summary>
        internal static DCBooleanValue FormatDateTimeUsingUpperString = DCBooleanValue.Inherit;

        //wyc20250312:DUWRITER5_0-4237
        /// <summary>
        /// 校验必填项时是否将单个回车也视为空白
        /// </summary>
        internal static bool bValidateBlankIncludingNewLine = false;
        /// <summary>
        /// 输出元素JSON时，不检查默认值，默认为true
        /// </summary>
        internal static bool WriteElementJsonWithoutCheckDefaultValue = true;
        //wyc20250328:
        /// <summary>
        /// 控制前端GetElementProperties获取元素属性时，是否排除未修改的带默认值的属性，默认为false
        /// </summary>
        internal static bool bGetElementPropertiesExcludeDefault = false;
        /// <summary>
        /// 从HTML属性中加载对象设置
        /// </summary>
        /// <param name="cfgName"></param>
        /// <param name="cfgValue"></param>
        [JSInvokable]
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public void LoadConfigByHtmlAttribute(string cfgName, string cfgValue)
        {
            try
            {
                if (cfgName == null || cfgValue.Length == 0)
                {
                    return;
                }
                cfgName = cfgName.ToLower().Trim();
                switch (cfgName)
                {
                    case "getelementpropertiesexcludedefault":
                        bGetElementPropertiesExcludeDefault = DCValueConvert.ObjectToBoolean(cfgValue, false);
                        break;
                    case "validateblankincludingnewline":
                        bValidateBlankIncludingNewLine = DCValueConvert.ObjectToBoolean(cfgValue, false);
                        break;
                    case "formatdatetimeusingupperstring":
                        DCBooleanValue val = DCBooleanValue.Inherit;
                        if (Enum.TryParse<DCBooleanValue>(cfgValue, true, out val) == true)
                        {
                            FormatDateTimeUsingUpperString = val;
                        }
                        break;
                    case "defaultfontname":
                        // 默认字体名称
                        cfgValue = cfgValue.Trim();
                        if (cfgValue.Length > 0)
                        {
                            XFontValue.DefaultFontName = cfgValue;
                        }
                        break;
                    #region 标尺相关的,满足 DUWRITER5_0-2963
                    case "rulevisible": // 标尺是否可见
                        this._Control.RuleVisible = DCValueConvert.ObjectToBoolean(cfgValue, true);
                        break;
                    case "rulebackcolor": // 标尺背景色
                        this._Control.RuleBackColor = DCValueConvert.HtmlToColor(cfgValue, DCDocumentRuleControl.StdBackColor);
                        break;
                    case "ruleclientbackcolor":// 标尺客户区背景色
                        DCSoft.Writer.Controls.DCDocumentRuleControl.StdClientBackColorBrush = new SolidBrush(DCValueConvert.HtmlToColor(cfgValue, Color.FromArgb(177, 202, 235)));
                        break;
                    case "ruleclientbordercolor":// 标尺客户区边框线颜色
                        DCSoft.Writer.Controls.DCDocumentRuleControl.StdClientBorderColorPen = new Pen(DCValueConvert.HtmlToColor(cfgValue, Color.FromArgb(213, 226, 243)));
                        break;
                    case "ruletextcolor": // 标尺文本颜色
                        DCSoft.Writer.Controls.DCDocumentRuleControl.StdTextColorBrush = new SolidBrush(DCValueConvert.HtmlToColor(cfgValue, Color.FromArgb(90, 97, 108)));
                        break;
                    case "rulelinecolor": // 标尺刻度线条颜色
                        DCSoft.Writer.Controls.DCDocumentRuleControl.StdRuleLineColorPen = new Pen(DCValueConvert.HtmlToColor(cfgValue, Color.FromArgb(128, 128, 128)));
                        break;
                    #endregion
                    case "readonly":
                        this._Control.Readonly = DCValueConvert.ObjectToBoolean(cfgValue, false); break;
                    case "allowdragcontent":
                        this._Control.AllowDragContent = DCValueConvert.ObjectToBoolean(cfgValue, false); break;
                    case "doublebuffer":
                        {
                            // 绘图时是否使用双缓冲功能
                            this._Control.WASMDoubleBufferForPaint = DCValueConvert.ObjectToBoolean(cfgValue);
                        }
                        break;
                    case "debugmode":
                        {
                            DomDocument._DebugMode = DCValueConvert.ObjectToBoolean(cfgValue, false);
                            this._Control.DocumentOptions.BehaviorOptions.DebugMode = DomDocument._DebugMode;
                        }
                        break;
                    default:
                        {
                            object targetObject = this._Control;
                            if (cfgName.StartsWith("documentoptions.behavioroptions.", StringComparison.Ordinal))
                            {
                                targetObject = this._Control.DocumentOptions.BehaviorOptions;
                                cfgName = cfgName.Substring("documentoptions.behavioroptions.".Length);
                            }
                            else if (cfgName.StartsWith("documentoptions.viewoptions.", StringComparison.Ordinal))
                            {
                                targetObject = this._Control.DocumentOptions.ViewOptions;
                                cfgName = cfgName.Substring("documentoptions.viewoptions.".Length);
                            }
                            else if (cfgName.StartsWith("documentoptions.editoptions.", StringComparison.Ordinal))
                            {
                                targetObject = this._Control.DocumentOptions.EditOptions;
                                cfgName = cfgName.Substring("documentoptions.editoptions.".Length);
                            }
                            if (targetObject != null)
                            {
                                var pinfo = targetObject.GetType().GetProperty(
                                    cfgName,
                                    System.Reflection.BindingFlags.Public
                                    | System.Reflection.BindingFlags.Instance
                                    | System.Reflection.BindingFlags.IgnoreCase);
                                if (pinfo != null)
                                {
                                    try
                                    {
                                        if (pinfo.PropertyType == typeof(bool))
                                        {
                                            pinfo.SetValue(targetObject, DCValueConvert.ObjectToBoolean(cfgValue));
                                        }
                                        else if (pinfo.PropertyType == typeof(string))
                                        {
                                            pinfo.SetValue(targetObject, cfgValue);
                                        }
                                        else if (pinfo.PropertyType == typeof(int))
                                        {
                                            int intV = 0;
                                            if (int.TryParse(cfgValue, out intV))
                                            {
                                                pinfo.SetValue(targetObject, intV);
                                            }
                                        }
                                        else if (pinfo.PropertyType == typeof(float))
                                        {
                                            float intV = 0;
                                            if (float.TryParse(cfgValue, out intV))
                                            {
                                                pinfo.SetValue(targetObject, intV);
                                            }
                                        }
                                        else if (pinfo.PropertyType == typeof(Color))
                                        {
                                            if (cfgValue != null && cfgValue.Length > 0)
                                            {
                                                // 设置颜色值
                                                Color cv;
                                                if (DCValueConvert.TryHtmlToColor(cfgValue, out cv))
                                                {
                                                    pinfo.SetValue(targetObject, cv);
                                                }
                                            }
                                        }
                                        else if (pinfo.PropertyType.IsEnum)
                                        {
                                            if (cfgValue != null && cfgValue.Length > 0)
                                            {
                                                object newValue = null;
                                                if (Enum.TryParse(pinfo.PropertyType, cfgValue, true, out newValue))
                                                {
                                                    pinfo.SetValue(targetObject, newValue);
                                                }
                                            }
                                        }
                                        else
                                        {
                                            pinfo.SetValue(targetObject, Convert.ChangeType(cfgValue, pinfo.PropertyType));
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        DCConsole.Default.WriteLine("LoadConfigByHtmlAttribute 错误:" + ex.Message);
                                    }
                                }
                            }
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                JavaScriptMethods.Tools_ReportException("LoadConfigByHtmlAttribute", ex.Message, ex.StackTrace.ToString(), 0);
                return;
            }
        }


        internal DotNetObjectReference<WriterControlForWASM> _ReferenceAtJS = null;

        public DCSoft.Writer.Serialization.ArrayXmlReader CreateXmlReaderByXmlText(string strXml)
        {
            if (strXml == null || strXml.Length == 0)
            {
                return null;
            }
            CheckStateBeforeInvoke();
            if (JavaScriptMethods.IO_PrepareInnerXmlReader(this._ContainerElementID, strXml, false))
            {
                return CreateInnerXmlReader();
            }
            return null;
        }

        public static DCSoft.Writer.Serialization.ArrayXmlReader StaticCreateXmlReaderByXmlText(string strXml)
        {
            if (JavaScriptMethods.IO_PrepareInnerXmlReader(null, strXml, false))
            {
                return CreateInnerXmlReader();
            }
            return null;
        }

        private MyWriterControlWASM _Control = null;
        internal WriterViewControl InnerViewControl
        {
            get
            {
                return this._Control.GetInnerViewControl();
            }
        }
        internal DomDocument Document
        {
            get
            {
                return this._Control?.GetInnerViewControl()?.Document;
            }
        }

        internal string _ContainerElementID = null;
        /// <summary>
        /// 袁永福 2023-12-25，DUWRITER5_0-1514
        /// </summary>
        /// <returns></returns>
        [JSInvokable]
        public DotNetObjectReference<DomElement> Get_JS_BeginEditValueUseListBoxControl_Field()
        {
            var input = this._Control._JS_BeginEditValueUseListBoxControl_Field;
            if (input != null)
            {
                return DotNetObjectReference.Create<DomElement>(input);
            }
            return null;
        }

        internal class MyWriterControlWASM : DCSoft.Writer.Controls.WriterControl
        {
            public MyWriterControlWASM(WriterControlForWASM p)
            {
                this._WASMParent = p;
            }

            public override IDataObject CreateDataObject()
            {
                return new WASMDataObject(new Dictionary<string, object>());
            }

            /// <summary>
            /// 触发对象类型元素的鼠标点击事件
            /// </summary>
            /// <param name="element">文档元素对象</param>
            /// <param name="args">事件参数</param>
            public override void OnEventObjectElementMouseClick(DomObjectElement element, DocumentEventArgs args)
            {
                if (this._WASMParent.HasControlEvent("EventObjectElementMouseClick"))
                {
                    var argsJson = new JsonObject();
                    argsJson["ElementID"] = element.ID;
                    argsJson["ElementType"] = element.GetType().Name;
                    argsJson["ElementName"] = element.Name;
                    argsJson["ElementHashCode"] = element.GetHashCode();
                    argsJson["ElementText"] = element.Text;
                    if (args.Style == DocumentEventStyles.MouseClick)
                    {
                        argsJson["EventType"] = "MouseClick";
                    }
                    else if (args.Style == DocumentEventStyles.MouseDblClick)
                    {
                        argsJson["EventType"] = "MouseDblClick";
                    }
                    argsJson["MouseClicks"] = args.MouseClicks;
                    argsJson["X"] = args.X;
                    argsJson["Y"] = args.Y;
                    argsJson["Readonly"] = this.Document.DocumentControler.CanModify(element, DomAccessFlags.Normal) == false;
                    if (element is DomCheckBoxElementBase chk)
                    {
                        argsJson["Checked"] = chk.Checked;
                        argsJson["GroupName"] = chk.Name;
                    }
                    this._WASMParent.RaiseControlEvent("EventObjectElementMouseClick", argsJson);
                }
            }

            public override void Dispose()
            {
                base.Dispose();
                if (this._Cache_HasControlEvents != null)
                {
                    this._Cache_HasControlEvents.Clear();
                    this._Cache_HasControlEvents = null;
                }
                this._JS_BeginEditValueUseListBoxControl_Field = null;
            }

            public override bool IsPrintOrPreviewMode
            {
                get
                {
                    return JavaScriptMethods.UI_IsPrintPreview(this._WASMParent._ContainerElementID);
                }
            }

            public override bool IsAPILogRecord
            {
                get
                {
                    if (this._WASMParent == null)
                    {
                        return false;
                    }
                    else
                    {
                        return JavaScriptMethods.UI_IsAPILogRecord(this._WASMParent._ContainerElementID);
                    }
                }
            }
            public override void APILogRecordDebugPrint(string txt)
            {
                if (this._WASMParent != null)
                {
                    JavaScriptMethods.UI_APILogRecordDebugPrint(this._WASMParent._ContainerElementID, txt);
                }
            }

            public override string WASMClientID => this._WASMParent._ContainerElementID;

            public override void OnCommandError(WriterCommand cmd, WriterCommandEventArgs cmdArgs, Exception exp)
            {
                base.OnCommandError(cmd, cmdArgs, exp);
                JavaScriptMethods.ConsoleWarring(cmd.Name + ":" + exp.ToString() + Environment.NewLine + exp.StackTrace);
            }

            public override string InnerFlagString()
            {
                if (JavaScriptMethods.UI_IsAPILogRecord(this._WASMParent._ContainerElementID))
                {
                    return "[API Record]";
                }
                return null;
            }
            public JsonNode ShowAboutDialog(bool shouUI)
            {
                if (shouUI)
                {
                    var strText = new System.Text.StringBuilder();
                    strText.AppendLine("第五代WEB版DCWriter文本编辑器，南京都昌信息科技有限公司版权所有。");
                    strText.AppendLine("系统当前时刻:" + DateTimeCommon.FastToYYYY_MM_DD_HH_MM_SS(DateTime.Now));
                    var len2 = strText.Length;
                    strText.AppendLine("底层框架版本:9");
                    strText.AppendLine("软件发布时间:" + DCSystemInfo.PublishDateString);
                    System.Windows.Forms.MessageBox.Show(null, strText.ToString());
                }

                JsonObject obj = new JsonObject();

                obj.Add("DCVersion", "DCWriterForWASM");
                obj.Add("DCPublishDate", DCSystemInfo.PublishDateString);


                return obj;
            }

            private WriterControlForWASM _WASMParent = null;
            public override WriterControlForWASM WASMParent
            {
                get
                {
                    return this._WASMParent;
                }
            }

            public override void JS_UpdatePages(string strPageCodes)
            {
                this._WASMParent.JS_UpdatePages(strPageCodes);
            }
            public override DateTime GetNowDateTime()
            {
                return JavaScriptMethods.GetNowDateTime();
            }
            internal DCSoft.Writer.Dom.DomInputFieldElement _JS_BeginEditValueUseListBoxControl_Field = null;

            public override void JS_BeginEditValueUseListBoxControl(
                DCSoft.Writer.Dom.DomInputFieldElement field,
                DCSoft.Writer.Data.ListItemCollection items,
                System.Action<string, string> callBack)
            {
                this._WASMParent._CurrentCallBack = callBack;
                var pos = new DropdownControlPosition(field);
                this._JS_BeginEditValueUseListBoxControl_Field = field;
                var args = new JsonObject();
                args.Add("ElementID", field.ID);
                args.Add("ElementName", field.Name);
                if (field.FieldSettings != null)
                {
                    var settings = field.FieldSettings;
                    args.Add("EditStyle", settings.EditStyle.ToString());
                    args.Add("MultiSelect", settings.MultiSelect);
                }
                args.Add("OldText", field.Text);
                args.Add("OldValue", field.InnerValue);
                this._WASMParent.JS_InvokeInstance(
                    JavaScriptMethods.JSNAME_WriterControl_UI_BeginEditValueUseListBoxControl,
                    pos.PageIndex,
                    pos.Left,
                    pos.Top,
                    pos.Height,
                    items,
                    args);
            }
            public override void RaiseEventUpdateToolarState()
            {
                JavaScriptMethods.UI_RaiseEventUpdateToolarState(this._WASMParent._ContainerElementID);
            }
            private DateTime _CreationTime__Cache_HasControlEvents = DateTime.Now;
            private Dictionary<string, bool> _Cache_HasControlEvents = new Dictionary<string, bool>();
            public override void WASMRaiseEvent(string eventName, object args)
            {
                if (eventName == null || eventName.Length == 0)
                {
                    throw new ArgumentNullException("eventName");
                }
                // 执行HandleDCWriterInnerEvent()非常耗时，能不调用就不调用。
                if (eventName == "EventButtonClick")
                {
                    // 执行按钮点击事件
                    this._WASMParent.HandleDCWriterInnerEvent(eventName, args);
                }
                {
                    // 此处使用一个缓存区来保存信息，避免频繁调用JS函数。缓存区信息维持5秒钟
                    if ((DateTime.Now - this._CreationTime__Cache_HasControlEvents).TotalSeconds > 5)
                    {
                        this._CreationTime__Cache_HasControlEvents = DateTime.Now;
                        this._Cache_HasControlEvents.Clear();
                    }
                    bool bolV = false;
                    if (this._Cache_HasControlEvents.TryGetValue(eventName, out bolV) == false)
                    {
                        bolV = this._WASMParent.HasControlEvent(eventName);
                        this._Cache_HasControlEvents[eventName] = bolV;
                    }
                    if (bolV == false)
                    {
                        return;
                    }
                }
                // 进行必要的包装

                this._WASMParent.RaiseControlEvent(eventName, args);
            }
        }
    }
}
