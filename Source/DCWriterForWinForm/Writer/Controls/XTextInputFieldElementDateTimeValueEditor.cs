using System;
using System.Collections.Generic;
using System.Text;
using DCSoft.Writer.Dom;
using System.Windows.Forms;
using DCSoft.WinForms;
using DCSoft.Writer.Data;
using DCSoft.Common;
using DCSoft.Drawing;
namespace DCSoft.Writer.Controls
{
    /// <summary>
    /// 弹出时间日期方式编辑文本输入域内容的编辑器
    /// </summary>
    public class XTextInputFieldElementDateTimeValueEditor : ElementValueEditor
    {
        /// <summary>
        /// 初始化对象
        /// </summary>
        public XTextInputFieldElementDateTimeValueEditor()
        {
        }
        /// <summary>
        /// 获得编辑样式
        /// </summary>
        /// <param name="host"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override ElementValueEditorEditStyle GetEditStyle(
            TextWindowsFormsEditorHostClass host,
            ElementValueEditContext context)
        {
            return ElementValueEditorEditStyle.DropDown;
        }

        /// <summary>
        /// 编辑数值
        /// </summary>
        /// <param name="inputHost">宿主对象</param>
        /// <param name="context">上下文对象</param>
        /// <returns>操作是否成功</returns>
        public override ElementValueEditResult EditValue(
            TextWindowsFormsEditorHostClass host,
            ElementValueEditContext context)
        {
            return StaticEditValue((TextWindowsFormsEditorHostClass)host, context, true);
        }

        internal static ElementValueEditResult StaticEditValue( 
            TextWindowsFormsEditorHostClass host,
            ElementValueEditContext context ,
            bool secondVisible )
        {
            DomInputFieldElement field = (DomInputFieldElement)context.Element;
            //InputFieldSettings settings = field.FieldSettings;
            string strDisplayFormat = null;
            if (field.DisplayFormat != null)
            {
                strDisplayFormat = field.DisplayFormat.Format;
            }
            // 编辑日期值
            string strValue = field.InnerValue;
            if (string.IsNullOrEmpty(strValue))
            {
                 strValue = field.Text;
            }
            DateTime dtmNow = field.OwnerDocument.GetNowDateTime();
            DateTime oldDate = dtmNow;
            //DateTime newDate = dtmNow;
            //bool NullValueFlag = false;
            if (DateTime.TryParseExact(
                    strValue,
                    strDisplayFormat,
                    System.Globalization.CultureInfo.InvariantCulture,
                    System.Globalization.DateTimeStyles.None,
                    out oldDate) == false)
            {
                if (strValue == string.Empty || DateTime.TryParse(strValue, out dtmNow) == false)
                {
                    oldDate = DateTime.MinValue;
                }
                else
                {
                    oldDate = DateTime.Parse(strValue);
                }
                //NullValueFlag = true;
            }
            else
            {
                //NullValueFlag = false;
            }
             host.EditControl.WASMParent.JSShowDateTimeControl (
                field,
                oldDate , 
                field.InnerEditStyle , 
                new Action<DateTime>(delegate(DateTime newDate2) { 
                    if(newDate2 != oldDate
                    //wyc20241106:DUWRITER5_0-3774
                    || (newDate2 == oldDate && oldDate == DateTime.MinValue))
                    {
                        ApplyNewValue(newDate2, field, host, secondVisible);
                    }
            }));
            return ElementValueEditResult.OpeingUI;
        }

        private static ElementValueEditResult ApplyNewValue(
            DateTime newDate , 
            DomInputFieldElement field , 
            TextWindowsFormsEditorHostClass host  ,
            bool secondVisible)
        {
            string newValue = null;
            string newText = null;
            if (newDate == DateTime.MinValue)
            {
                newText = string.Empty;
                newValue = string.Empty;
            }
            else
            {
                var format = field.RuntimeDisplayFormat;
                if (format != null && format.IsEmpty == false)
                {
                    newText = format.Execute(newDate.ToString());
                }
                else if (secondVisible)
                {
                    newText = newDate.ToString(WriterConst.Format_YYYY_MM_DD_HH_MM_SS);
                }
                else
                {
                    newText = newDate.ToString(WriterConst.Format_YYYY_MM_DD_HH_MM);
                }
                    newValue = newDate.ToString(WriterConst.Format_YYYY_MM_DD_HH_MM_SS);
            }


            if (field.Text != newText ||
                field.InnerValue != newValue)//wyc20220829:防止同日期但时间不同时不赋值
            {
                string oldText = field.Text;
                string oldValue = field.InnerValue;
                field.OwnerDocument.BeginLogUndo();
                DomInputFieldElementBase.EventBeforeOnContentChangedOnce = delegate (object eventSender, EventArgs args2)
                {
                    // 设置字段的InnerValue属性值
                    if (field.InnerValue != newValue)
                    {
                        if (host.Document.CanLogUndo)
                        {
                            host.Document.UndoList.AddProperty("InnerValue",
                                field.InnerValue,
                                newValue,
                                field);
                        }
                        field.InnerValue = newValue;
                    }
                };

                SetContainerTextArgs args3 = new SetContainerTextArgs();
                args3.NewText = newText;
                args3.AccessFlags = (DomAccessFlags)MathCommon.SetIntAttribute(
                        (int)DomAccessFlags.Normal,
                        (int)DomAccessFlags.CheckUserEditable,
                        false);
                args3.ShowUI = true;
                args3.LogUndo = true;
                args3.UpdateContent = true;
                args3.RaiseDocumentContentChangedEvent = false;
                args3.FocusContainer = true;
                args3.IgnoreDisplayFormat = true;
                args3.EventSource = ContentChangedEventSource.ValueEditor;
                if (field.SetEditorText(args3) ||
                    field.InnerValue != newValue)//wyc20220829:防止同日期但时间不同时不赋值
                {
                    field.InnerValue = newValue;
                    host.Document.EndLogUndo();
                    host.RaiseDocumentContentChangedOnceAfterEditValue = true;
                    host.EndEditingValue();
                    host.ResetState();
                    return ElementValueEditResult.UserAccept;
                }
                else
                {
                    host.Document.CancelLogUndo();
                    host.ResetState();
                    return ElementValueEditResult.UserCancel;
                }
            }
            return ElementValueEditResult.UserCancel;
        }
    }
}
