using System;
using System.Text.Json;
using DCSoft.Writer.Controls;
using DCSoft.Writer.Dom;
using DCSoft.Common;
using DCSoft.WinForms;
using System.Collections.Generic;
using System.Windows.Forms;

namespace DCSoft.WASM
{
    public partial class WriterControlForWASM
    {
        /// <summary>
        /// 设置字符串资源
        /// </summary>
        /// <param name="json">数据包</param>
        [JSInvokable]
        public static void SetDCSRValues(JsonElement json)
        {
            if (json.ValueKind == JsonValueKind.Object)
            {
                foreach (JsonProperty property in json.EnumerateObject())
                {
                    if (property.Value.ValueKind == JsonValueKind.Null
                        || property.Value.ValueKind == JsonValueKind.Undefined)
                    {
                        DCSR.SetValue(property.Name, null);
                    }
                    else if (property.Value.ValueKind == JsonValueKind.String)
                    {
                        DCSR.SetValue(property.Name, property.Value.GetString());
                    }
                }
            }
        }
        /// <summary>
        /// 正在等待内容拖拽事件
        /// </summary>
        /// <returns></returns>
        [JSInvokable]
        public bool Is_WaittingForDragStart()
        {
            return this._Control.GetInnerViewControl()._WaittingForDragStart;
        }

        [JSInvokable]
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public void UpdateTextCaretWithoutScroll()
        {
            try
            {
                this._Control.UpdateTextCaretWithoutScroll();
            }
            catch (Exception ex)
            {
                JavaScriptMethods.Tools_ReportException("UpdateTextCaretWithoutScroll", ex.Message, ex.ToString(), 0);
                return;
            }
        }

        private System.Windows.Forms.MouseEventArgs CreateMouseEventArgs(
            DCSoft.Printing.PrintPage page,
            object altKey,
            object shiftKey,
            object ctrlKey,
            object clientX,
            object clientY,
            object buttons,
            object detail)
        {
            var bolAltKey = DCValueConvert.ObjectToBoolean(altKey);
            var bolShiftKey = DCValueConvert.ObjectToBoolean(shiftKey);
            var bolCtrlKey = DCValueConvert.ObjectToBoolean(ctrlKey);
            var offsetX = DCValueConvert.ObjectToInt32(clientX);
            var offsetY = DCValueConvert.ObjectToInt32(clientY);
            var intButtons = DCValueConvert.ObjectToInt32(buttons);
            var intDetail = DCValueConvert.ObjectToInt32(detail);

            System.Windows.Forms.Control.SetModifierKeys(bolAltKey, bolShiftKey, bolCtrlKey);
            int x = page == null ? offsetX : (int)((page.ClientBounds.X + (offsetX) / this.ZoomRate + 1));
            int y = page == null ? offsetY : (int)((page.ClientBounds.Y + (offsetY) / this.ZoomRate + 1));
            var args = new System.Windows.Forms.MouseEventArgs();
            if (intButtons != 0)
            {
                if ((intButtons & 1) == 1)
                {
                    args.Button = System.Windows.Forms.MouseButtons.Left;
                }
                if ((intButtons & 2) == 2)
                {
                    args.Button = args.Button | System.Windows.Forms.MouseButtons.Right;
                }
                if ((intButtons & 4) == 4)
                {
                    args.Button = args.Button | System.Windows.Forms.MouseButtons.Middle;
                }
            }
            args.X = x;
            args.Y = y;
            args.Clicks = intDetail;
            return args;
        }
        /// <summary>
        /// 处理标尺鼠标事件
        /// </summary>
        /// <param name="strRuleName"></param>
        /// <param name="eventType"></param>
        /// <param name="altKey"></param>
        /// <param name="shiftKey"></param>
        /// <param name="ctrlKey"></param>
        /// <param name="clientX"></param>
        /// <param name="clientY"></param>
        /// <param name="buttons"></param>
        /// <param name="detail"></param>
        /// <returns></returns>
        [JSInvokable]
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public string RuleHandleMouseEvent(
               string strRuleName,
               string eventType,
               object altKey,
               object shiftKey,
               object ctrlKey,
               object clientX,
               object clientY,
               object buttons,
               object detail)
        {
            try
            {
                var args = CreateMouseEventArgs(null, altKey, shiftKey, ctrlKey, clientX, clientY, buttons, detail);
                eventType = eventType.ToLower().Trim();
                var ctl = this._Control;
                switch (eventType)
                {
                    case "mousedown":
                        {
                            return ctl.MyRaiseRuleMouseEvent(strRuleName, args, MouseEventType.MouseDown);
                        }
                        break;
                    case "mousemove":
                        {
                            return ctl.MyRaiseRuleMouseEvent(strRuleName, args, MouseEventType.MouseMove);
                        }
                    case "mouseup":
                        {
                            return ctl.MyRaiseRuleMouseEvent(strRuleName, args, MouseEventType.MouseUp);
                        }
                        break;
                    case "click":
                        {
                        }
                        break;
                    case "dblclick":
                        {
                        }
                        break;
                }
                return null;
            }
            catch (Exception ex)
            {
                JavaScriptMethods.Tools_ReportException("RuleHandleMouseEvent", ex.Message, ex.ToString(), 0);
                return "";
            }
        }
        private System.Windows.Forms.MouseEventArgs _LastMouseDownEventArgs = null;
        private System.Windows.Forms.MouseEventArgs _LastMouseUpEventArgs = null;

        private DateTime _LastMouseDownTick = DateTime.MinValue;
        /// <summary>
        /// 执行编辑器鼠标事件
        /// </summary>
        /// <param name="intPageIndex">页码</param>
        /// <param name="eventType">事件类型</param>
        /// <param name="altKey">alt按键状态</param>
        /// <param name="shiftKey">shift按键状态</param>
        /// <param name="ctrlKey">ctrl按键状态</param>
        /// <param name="clientX">X坐标值</param>
        /// <param name="clientY">Y坐标值</param>
        /// <param name="buttons">鼠标按键值</param>
        /// <param name="detail">滚轮状态值</param>
        [JSInvokable]
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public void EditorHandleMouseEvent(
               int intPageIndex,
               string eventType,
               object altKey,
               object shiftKey,
               object ctrlKey,
               object clientX,
               object clientY,
               object buttons,
               object detail)
        {
            try
            {
                if (eventType == null)
                {
                    return;
                }
                var page = this._Control.Pages.SafeGet(intPageIndex);
                if (page == null)
                {
                    return;
                }
                _Current = this;
                var args = CreateMouseEventArgs(page, altKey, shiftKey, ctrlKey, clientX, clientY, buttons, detail);
                eventType = eventType.ToLower().Trim();
                var ctl = this._Control;
                var cur = ctl.Cursor;
                switch (eventType)
                {
                    case "mousedown":
                        {
                            ClearCachedDragData();
                            ctl.MyRaiseMouseDown(args);
                            this._LastMouseDownEventArgs = args;
                            this._LastMouseUpEventArgs = null;
                            this._LastMouseDownTick = DateTime.Now;
                        }
                        break;
                    case "mousemove":
                        {
                            ctl.MyRaiseMouseMove(args);
                        }
                        break;
                    case "mouseup":
                        {
                            if (this._LastMouseDownEventArgs != null)
                            {
                                args.Button = this._LastMouseDownEventArgs.Button;
                            }
                            ctl.MyRaiseMouseUp(args);
                            this._LastMouseDownEventArgs = null;
                            this._LastMouseUpEventArgs = args;
                        }
                        break;
                    case "click":
                        {
                            if (this._LastMouseUpEventArgs != null)
                            {
                                args.Button = this._LastMouseUpEventArgs.Button;
                            }
                            ctl.MyRaiseMouseClick(args);
                        }
                        break;
                    case "dblclick":
                        {
                            if (this._LastMouseUpEventArgs != null)
                            {
                                args.Button = this._LastMouseUpEventArgs.Button;
                            }
                            ctl.MyRaiseMouseDoubleClick(args);
                        }
                        break;
                }
                this.UpdateCusror(intPageIndex, cur);
            }
            catch (Exception ex)
            {
                JavaScriptMethods.Tools_ReportException("EditorHandleMouseEvent", ex.Message, ex.ToString(), 0);
                return;
            }
            finally
            {
                _Current = null;
            }
        }
        [JSInvokable]
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public string GetContextMenuTypeName()
        {
            try
            {
                string strElementTypeName = null;
                {
                    var singleElement = this._Control.SingleSelectedElement;
                    if (singleElement != null)
                    {
                        // 用户选择了单一的元素
                        if (singleElement is DomObjectElement)
                        {
                            strElementTypeName = singleElement.GetType().Name;
                        }
                    }
                    if (strElementTypeName == null)
                    {
                        if (this._Control.CurrentTableCell != null)
                        {
                            strElementTypeName = typeof(DomTableCellElement).Name;
                        }
                        else if (this._Control.CurrentInputField != null)
                        {
                            strElementTypeName = typeof(DomInputFieldElement).Name;
                        }
                        else if (this._Control.SingleSelectedElement is DomImageElement)
                        {
                            strElementTypeName = typeof(DomImageElement).Name;
                        }
                    }
                    if (strElementTypeName == null
                        && this._Control.Selection.Cells != null
                        && this._Control.Selection.Cells.Count > 0)
                    {
                        // 用户选择了单元格
                        strElementTypeName = typeof(DomTableCellElement).Name;
                    }
                    if (strElementTypeName == null && this._Control.Selection.ContentElements != null)
                    {
                        foreach (var element in this._Control.Selection.ContentElements)
                        {
                            if (element is DomObjectElement)
                            {
                                strElementTypeName = element.GetType().Name;
                                break;
                            }
                        }
                    }
                    if (strElementTypeName == null)
                    {
                        strElementTypeName = typeof(DomElement).Name;
                    }
                }
                return strElementTypeName;
            }
            catch (Exception ex)
            {
                JavaScriptMethods.Tools_ReportException("GetContextMenuTypeName", ex.Message, ex.ToString(), 0);
                return "";
            }
        }
        private System.Windows.Forms.DragEventArgs CreateDragEventArgs(
            Dictionary<string, object> datas,
            DCSoft.Printing.PrintPage page,
            object altKey,
            object shiftKey,
            object ctrlKey,
            object clientX,
            object clientY)
        {
            var bolAltKey = DCValueConvert.ObjectToBoolean(altKey);
            var bolShiftKey = DCValueConvert.ObjectToBoolean(shiftKey);
            var bolCtrlKey = DCValueConvert.ObjectToBoolean(ctrlKey);
            var offsetX = DCValueConvert.ObjectToInt32(clientX);
            var offsetY = DCValueConvert.ObjectToInt32(clientY);

            System.Windows.Forms.Control.SetModifierKeys(bolAltKey, bolShiftKey, bolCtrlKey);
            int x = page == null ? offsetX : (int)((page.ClientBounds.X + (offsetX) / this.ZoomRate + 1));
            int y = page == null ? offsetY : (int)((page.ClientBounds.Y + (offsetY) / this.ZoomRate + 1));
            var args = new System.Windows.Forms.DragEventArgs(
                new WASMDataObject( datas ) ,//this.WASMCreateDataObject(datas),
                (int)System.Windows.Forms.Control.ModifierKeys,
                x,
                y,
                System.Windows.Forms.DragDropEffects.Copy | System.Windows.Forms.DragDropEffects.Move,
                System.Windows.Forms.DragDropEffects.None);
            if (bolCtrlKey)
            {
                args.Effect = System.Windows.Forms.DragDropEffects.Copy;
            }
            else
            {
                args.Effect = System.Windows.Forms.DragDropEffects.Move;
            }
            args.AllowedEffect = args.Effect;
            return args;
        }

        private DCSystem_Drawing.Point ToViewControlClientPosition(int pageIndex, int x, int y)
        {
            var ctl = this.InnerViewControl;
            var page = ctl.Pages.SafeGet(pageIndex);
            if (page == null)
            {
                return new DCSystem_Drawing.Point(x, y);
            }
            else
            {
                return new DCSystem_Drawing.Point((int)((page.ClientBounds.X + x / this.ZoomRate + 1)),
                    (int)((page.ClientBounds.Y + y / this.ZoomRate + 1)));
            }
        }

        private static readonly string _DragDataFlag = DateTime.Now.Ticks.ToString();
        /// <summary>
        /// 开始执行拖拽功能
        /// </summary>
        /// <returns></returns>
        [JSInvokable]
        public string[] EditorStartDragSelection(int pageIndex, int x, int y)
        {
            var ctl = this.InnerViewControl;
            var p = ToViewControlClientPosition(pageIndex, x, y);
            if (ctl.IsInSelectionArea(p.X, p.Y))
            {
                DragDropEffects effects = DragDropEffects.None;
                var datas = (WASMDataObject)ctl.DragSelectionContent(ref effects);
                if (this.Readonly)
                {
                    effects = DragDropEffects.Copy;
                }
                var datas2 = datas.GetRawDatas();
                _CachedDragDataID = "dcid_" + DateTime.Now.Ticks.ToString();
                datas2[_CachedDragDataID] = "1";
                _CachedDragData = datas2;
                var strList = this.ToHtmlStringList(datas?.GetRawDatas());
                if (strList != null)
                {
                    strList.Insert(0, WASMUtils.ToHtmlString(effects));
                    strList.Add("DragDataFlag");
                    strList.Add(_DragDataFlag);
                    return strList.ToArray();
                }
            }
            return null;
        }


        private static string _CachedDragDataID = null;
        private static Dictionary<string, object> _CachedDragData = null;

        [JSInvokable]
        public bool IsCachedDragData(string strID)
        {
            return _CachedDragDataID == strID;
        }

        [JSInvokable]
        public void ClearCachedDragData()
        {
            _CachedDragDataID = null;
            if (_CachedDragData != null)
            {
                _CachedDragData.Clear();
                _CachedDragData = null;
            }
        }

        [JSInvokable]
        public bool SetCachedDragData(string strID, string[] dragDatas)
        {
            if (_CachedDragData != null)
            {
                _CachedDragData.Clear();
            }
            _CachedDragData = null;
            _CachedDragDataID = null;
            if (dragDatas != null && dragDatas.Length > 0 && strID != null && strID.Length > 0)
            {
                var datas = WASMUtils.CreateDataDictionary(dragDatas);
                if (datas != null && datas.Count > 0)
                {
                    _CachedDragData = datas;
                    _CachedDragDataID = strID;
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 执行编辑器OLD拖拽事件
        /// </summary>
        /// <param name="dragDatas">拖拽的数据</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="eventType">事件类型</param>
        /// <param name="altKey">alt按键状态</param>
        /// <param name="shiftKey">shift按键状态</param>
        /// <param name="ctrlKey">ctrl按键状态</param>
        /// <param name="clientX">X坐标值</param>
        /// <param name="clientY">Y坐标值</param>
        /// <param name="strAllowEffect">允许的拖拽效果</param>
        [JSInvokable]
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public string EditorHandleDragEvent(
               string[] dragDatas,
               int pageIndex,
               string eventType,
               object altKey,
               object shiftKey,
               object ctrlKey,
               int clientX,
               int clientY,
               string strAllowEffect)
        {
            try
            {
                if (this.Readonly)
                {
                    // 只读的控件，不处理拖拽事件
                    return null;
                }
                if (dragDatas == null || dragDatas.Length == 0)
                {
                    return null;
                }
                if (eventType == null)
                {
                    return null;
                }
                var page = this._Control.Pages.SafeGet(pageIndex);
                if (page == null)
                {
                    return null;
                }
                _Current = this;
                Dictionary<string, object> datas = null;
                if (_CachedDragDataID != null
                    && _CachedDragDataID.Length > 0
                    && _CachedDragData != null
                    && _CachedDragData.Count > 0)
                {
                    // 存在本地缓存的数据
                    for (var iCount = 0; iCount < dragDatas.Length; iCount += 2)
                    {
                        if (dragDatas[iCount] == _CachedDragDataID)
                        {
                            datas = _CachedDragData;
                            break;
                        }
                    }
                }
                if (datas == null)
                {
                    datas = WASMUtils.CreateDataDictionary(dragDatas);
                }
                var args = CreateDragEventArgs(datas, page, altKey, shiftKey, ctrlKey, clientX, clientY);
                if (strAllowEffect != null)
                {
                    switch (strAllowEffect.Trim().ToLower())
                    {
                        case "none": args.AllowedEffect = DragDropEffects.None; break;
                        case "copy": args.AllowedEffect = DragDropEffects.Copy; break;
                        case "copylink": args.AllowedEffect = DragDropEffects.Copy | DragDropEffects.Link; break;
                        case "copymove": args.AllowedEffect = DragDropEffects.Copy | DragDropEffects.Move; break;
                        case "link": args.AllowedEffect = DragDropEffects.Link; break;
                        case "linkmove": args.AllowedEffect = DragDropEffects.Link | DragDropEffects.Move; break;
                        case "move": args.AllowedEffect = DragDropEffects.Move; break;
                        case "all": args.AllowedEffect = DragDropEffects.All; break;
                    }
                }
                eventType = eventType.ToLower().Trim();
                var ctl = this._Control;
                var cur = ctl.Cursor;
                switch (eventType)
                {
                    case "dragenter":
                        {
                            ctl.MyRaiseDragEnter(args);
                        }
                        break;
                    case "dragleave":
                        {
                            ctl.MyRaiseDragLeave(EventArgs.Empty);
                        }
                        return args.Effect.ToString();
                    case "dragover":
                        {
                            ctl.MyRaiseDragOver(args);
                        }
                        break;
                    case "drop":
                        {
                            ctl.MyRaiseDragDrop(args);
                            ClearCachedDragData();
                        }
                        break;
                }
                if (args.Effect == System.Windows.Forms.DragDropEffects.None)
                {
                    this._Control.Cursor = new System.Windows.Forms.Cursor("not-allowed");
                }
                else if (args.Effect == System.Windows.Forms.DragDropEffects.Link)
                {
                    this._Control.Cursor = new System.Windows.Forms.Cursor("alias");
                }
                else if (args.Effect == System.Windows.Forms.DragDropEffects.Copy)
                {
                    this._Control.Cursor = new System.Windows.Forms.Cursor("copy");
                }
                else if (args.Effect == System.Windows.Forms.DragDropEffects.Move)
                {
                    this._Control.Cursor = new System.Windows.Forms.Cursor("move");
                }
                else
                {
                    this._Control.Cursor = System.Windows.Forms.Cursors.Arrow;
                }
                this.UpdateCusror(pageIndex, cur);
                return WASMUtils.ToHtmlString(args.Effect);
            }
            catch (Exception ex)
            {
                JavaScriptMethods.Tools_ReportException("EditorHandleDragEvent", ex.Message, ex.ToString(), 0);
                return null;
            }
            finally
            {
                _Current = null;
            }
        }
        /// <summary>
        /// 为拖拽内容在操作而删除被选中内容
        /// </summary>
        [JSInvokable]
        public void EditorDeleteSelectionForDragingSelection()
        {
            if (this._Control.GetInnerViewControl().DragState == DragOperationState.DragingSelection)
            {
                try
                {
                    _Current = this;
                    this._Control.DeleteSelection(true);
                }
                finally
                {
                    _Current = null;
                }
            }
        }
        private int _LastPageIndexOfUpdateCusror = -1;
        private void UpdateCusror(int intPageIndex, System.Windows.Forms.Cursor oldCur)
        {
            if (this._Control.Cursor != oldCur || intPageIndex != this._LastPageIndexOfUpdateCusror)
            {
                this._LastPageIndexOfUpdateCusror = intPageIndex;
                JavaScriptMethods.UI_SetElementCursor(
                    this._ContainerElementID,
                    intPageIndex,
                    this._Control.Cursor == null ? "default" : this._Control.Cursor.Name);
            }
        }

        [JSInvokable]
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public bool EditorHandleKeyEvent(
            string eventType,
            bool altKey,
            bool shiftKey,
            bool ctrlKey,
            object code,
            object key)
        {
            try
            {
                _Current = this;
                var strCode = Convert.ToString(code);
                var strKey = Convert.ToString(key);
                var args = CreateKeyEventArgs(altKey, shiftKey, ctrlKey, strKey, strCode);
                switch (eventType)
                {
                    case "keydown":
                        {
                            if (args.Control == true && args.Alt == false && args.Shift == false)
                            {
                                if (args.KeyCode == System.Windows.Forms.Keys.C
                                    || args.KeyCode == System.Windows.Forms.Keys.V)
                                {
                                    // 遇到Ctrl+C/Ctrl+V，则执行浏览器默认的复制粘贴操作。
                                    return false;
                                }
                            }
                            System.Windows.Forms.Control.SetModifierKeys(altKey, shiftKey, ctrlKey);
                            this._Control.MyRaiseKeyDown(args);
                        }
                        break;
                    case "keyup":
                        {
                            System.Windows.Forms.Control.SetModifierKeys(altKey, shiftKey, ctrlKey);
                            this._Control.MyRaiseKeyUp(args);
                        }
                        break;
                }
                return args.Handled || args.KeyData == System.Windows.Forms.Keys.Tab;
            }
            catch (Exception ex)
            {
                JavaScriptMethods.Tools_ReportException("EditorHandleKeyEvent", ex.Message, ex.ToString(), 0);
                return false;
            }
            finally
            {
                _Current = null;
            }
        }
        [JSInvokable]
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public void EditorInsertStringFromUI(object txt)
        {
            try
            {
                _Current = this;
                string str = DCValueConvert.ObjectToString(txt);
                if (str != null && str.Length > 0)
                {
                    this._Control.InsertStringFromUI(str);
                }
            }
            catch (Exception ex)
            {
                JavaScriptMethods.Tools_ReportException("EditorInsertStringFromUI", ex.Message, ex.ToString(), 0);
                return;
            }
            finally
            {
                _Current = null;
            }
        }

        [JSInvokable]
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public bool EditorHandleKeyPress(object vChar)
        {
            try
            {
                _Current = this;
                var strChar = Convert.ToString(vChar);
                if (strChar != null && strChar.Length > 0)
                {
                    if (strChar == "Enter")
                    {
                        strChar = "\r";
                    }
                    var args = new System.Windows.Forms.KeyPressEventArgs(strChar[0]);
                    this._Control.MyRaiseKeyPress(args);
                    return args.Handled;
                }
                return false;
            }
            catch (Exception ex)
            {
                JavaScriptMethods.Tools_ReportException("EditorHandleKeyPress", ex.Message, ex.ToString(), 0);
                return false;
            }
            finally
            {
                _Current = null;
            }
        }
        private static System.Windows.Forms.KeyEventArgs CreateKeyEventArgs(
            bool altKey,
            bool shiftKey,
            bool ctlKey,
            string strKey,
            string strCode)
        {
            var keyData = HtmlKeyNames.ParseKeys(strKey, strCode);
            if (altKey)
            {
                keyData = keyData | System.Windows.Forms.Keys.Alt;
            }
            if (shiftKey)
            {
                keyData = keyData | System.Windows.Forms.Keys.Shift;
            }
            if (ctlKey)
            {
                keyData = keyData | System.Windows.Forms.Keys.Control;
            }
            var result = new System.Windows.Forms.KeyEventArgs(keyData);
            return result;
        }


        public System.Windows.Forms.DialogResult JS_ShowMessageBox(
            string text,
            string caption,
            System.Windows.Forms.MessageBoxButtons buttons,
            System.Windows.Forms.MessageBoxIcon icon,
            System.Windows.Forms.MessageBoxDefaultButton defaultButton)
        {
            try
            {
                var strResult = JavaScriptMethods.UI_ShowMessageBox(
                    text,
                    caption,
                    ((int)buttons).ToString(),
                    ((int)icon).ToString(),
                    ((int)defaultButton).ToString()
                    );
                if (strResult != null && strResult.Length > 0)
                {
                    int dr = 0;
                    if (int.TryParse((string)strResult, out dr))
                    {
                        return (System.Windows.Forms.DialogResult)dr;
                    }
                }
                return System.Windows.Forms.DialogResult.OK;

            }
            catch (Exception ex)
            {
                JavaScriptMethods.Tools_ReportException("JS_ShowMessageBox", ex.Message, ex.ToString(), 0);
                return System.Windows.Forms.DialogResult.OK;
            }
        }

        public void JS_SetToolTip(string txt)
        {
            try
            {
                JavaScriptMethods.UI_SetToolTip(this._ContainerElementID, txt);
                //this.JS_InvokeVoidInstance(
                //    DCWriterModule.JSNAME_WriterControl_UI_SetToolTip,
                //    txt);
            }
            catch (Exception ex)
            {
                JavaScriptMethods.Tools_ReportException("JS_SetToolTip", ex.Message, ex.ToString(), 0);
                return;
            }
        }


        public void JS_ShowCaret(
            int pageIndex,
            int left,
            int top,
            int width,
            int height,
            bool visible,
            bool bolReadonly,
            bool bolScrollToView)
        {
            try
            {
                JavaScriptMethods.UI_ShowCaret(
                    this._ContainerElementID,
                    pageIndex,
                    left,
                    top,
                    width,
                    height,
                    visible,
                    bolReadonly,
                    bolScrollToView);
            }
            catch (Exception ex)
            {
                JavaScriptMethods.Tools_ReportException("JS_ShowCaret", ex.Message, ex.ToString(), 0);
                return;
            }
        }

        public void JS_UpdatePages(string strPageCodes)
        {
            try
            {
                JavaScriptMethods.Paint_UpdatePages(this._ContainerElementID, strPageCodes);
            }
            catch (Exception ex)
            {
                JavaScriptMethods.Tools_ReportException("JS_UpdatePages", ex.Message, ex.ToString(), 0);
                return;
            }
        }

        public void ReversibleDraw(int intPageIndex, int x1, int y1, int x2, int y2, int type)
        {
            try
            {
                JavaScriptMethods.Paint_ReversibleDraw(
                    this._ContainerElementID,
                    intPageIndex,
                    (int)(x1 * this.ZoomRate),
                    (int)(y1 * this.ZoomRate),
                    (int)(x2 * this.ZoomRate),
                    (int)(y2 * this.ZoomRate),
                    type);
            }
            catch (Exception ex)
            {
                JavaScriptMethods.Tools_ReportException("ReversibleDraw", ex.Message, ex.ToString(), 0);
                return;
            }
        }
        public void CloseReversibleShape()
        {
            try
            {
                JavaScriptMethods.Paint_CloseReversibleShape(this._ContainerElementID);
                //this.JS_InvokeInstance(DCWriterModule.JSNAME_WriterControl_Paint_CloseReversibleShape);
            }
            catch (Exception ex)
            {
                JavaScriptMethods.Tools_ReportException("CloseReversibleShape", ex.Message, ex.ToString(), 0);
                return;
            }
        }

        public void JSShowColorControl(
            DCSystem_Drawing.Color defaultColor,
            System.Action<DCSystem_Drawing.Color> callBack)
        {
            try
            {
                this._CurrentCallBack = callBack;
                JavaScriptMethods.UI_JSShowColorControl(
                    this._ContainerElementID,
                     DCSystem_Drawing.ColorTranslator.ToHtml(defaultColor));
            }
            catch (Exception ex)
            {
                JavaScriptMethods.Tools_ReportException("JSShowColorControl", ex.Message, ex.ToString(), 0);
                return;
            }
        }

        public void JSShowCalculateControl(
            DomInputFieldElement element,
            double inputValue,
            System.Action<double> callBack)
        {
            try
            {
                this._CurrentCallBack = callBack;
                var pos = new DropdownControlPosition(element);
                JavaScriptMethods.UI_ShowCalculateControl(
                    this._ContainerElementID,
                    pos.PageIndex,
                    pos.Left,
                    pos.Top,
                    pos.Height,
                    inputValue);

            }
            catch (Exception ex)
            {
                JavaScriptMethods.Tools_ReportException("JSShowCalculateControl", ex.Message, ex.ToString(), 0);
                return;
            }
        }
        public void JSShowDateTimeControl(
            DomInputFieldElement element,
            DateTime inputValue,
            DCSoft.Writer.Dom.InputFieldEditStyle style,
            System.Action<DateTime> callBack)
        {
            try
            {
                this._CurrentCallBack = callBack;
                var pos = new DropdownControlPosition(element);

                //wyc20230818:防止前端输入域内有非法字符时此处时间值为最小值后弹框报错
                if (inputValue == DateTime.MinValue)
                {
                    inputValue = DateTime.Now;
                }
                //////////////////////////////////////////

                JavaScriptMethods.UI_ShowDateTimeControl(
                    this._ContainerElementID,
                    pos.PageIndex,
                    pos.Left,
                    pos.Top,
                    pos.Height,
                    inputValue,
                    (int)style);
            }
            catch (Exception ex)
            {
                JavaScriptMethods.Tools_ReportException("JSShowDateTimeControl", ex.Message, ex.ToString(), 0);
                return;
            }
        }



        private object _CurrentCallBack = null;
        /// <summary>
        /// 将数值编辑器修改后的文档元素数值更新到文档中
        /// </summary>
        /// <param name="newInputValue"></param>
        [JSInvokable]
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public void ApplyCurrentEditorCallBack(object newInputValue, object newInputValue2, object newActualValue)
        {
            try
            {
                var cb = this._CurrentCallBack;
                this._CurrentCallBack = null;

                if (cb is System.Action<DateTime>)
                {
                    var handler = (System.Action<DateTime>)cb;
                    var dtm = DateTime.Now;
                    var strNewActualValue = DCValueConvert.ObjectToString(newActualValue);
                    if (strNewActualValue != null && strNewActualValue.Length > 0 && DateTime.TryParse(strNewActualValue, out dtm))
                    {
                        //如果是其他非时间的时候，根据第三个参数进行处理
                        handler(dtm);
                    }
                    else
                    {
                        var strNewInputValue = DCValueConvert.ObjectToString(newInputValue);
                        if (strNewInputValue != null && strNewInputValue.Length > 0 && DateTime.TryParse(DCValueConvert.ObjectToString(newInputValue), out dtm))
                        {
                            handler(dtm);
                        }
                        else
                        {
                            handler(DateTime.MinValue);
                        }
                    }
                }
                else if (cb is System.Action<double>)
                {
                    var handler = (System.Action<double>)cb;
                    double dbl = 0;
                    if (double.TryParse(DCValueConvert.ObjectToString(newInputValue), out dbl))
                    {
                        handler(dbl);
                    }
                }
                else if (cb is System.Action<DCSystem_Drawing.Color>)
                {
                    var handler = (System.Action<DCSystem_Drawing.Color>)cb;
                    var newColor = DCSystem_Drawing.ColorTranslator.FromHtml(DCValueConvert.ObjectToString(newInputValue));
                    handler(newColor);
                }
                else if (cb is System.Action<string, string>)
                {
                    var handler = (System.Action<string, string>)cb;
                    handler(
                        DCValueConvert.ObjectToString(newInputValue),
                        DCValueConvert.ObjectToString(newInputValue2));
                }
            }
            catch (Exception ex)
            {
                JavaScriptMethods.Tools_ReportException("ApplyCurrentEditorCallBack", ex.Message, ex.ToString(), 0);
                return;
            }
        }

        public string JSShowPromptDialog(string strTitle, string strDefaultValue)
        {
            try
            {
                var result = JavaScriptMethods.UI_WindowPrompt(strTitle, strDefaultValue);
                return Convert.ToString(result);
            }
            catch (Exception ex)
            {
                JavaScriptMethods.Tools_ReportException("JSShowPromptDialog", ex.Message, ex.ToString(), 0);
                return "";
            }
        }



        public bool IsDropdownControlVisible()
        {
            return JavaScriptMethods.UI_IsDropdownControlVisible(this._ContainerElementID);
        }
        public void CloseDropdownControl()
        {
            try
            {
                JavaScriptMethods.UI_CloseDropdownControl(this._ContainerElementID);
            }
            catch (Exception ex)
            {
                JavaScriptMethods.Tools_ReportException("CloseDropdownControl", ex.Message, ex.ToString(), 0);
                return;
            }
        }
    }
}
