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
    /// 弹出日期选择框的文本输入域内容编辑器对象
    /// </summary>
    internal class XTextInputFieldElementDateValueEditor : ElementValueEditor
    {
        /// <summary>
        /// 初始化对象
        /// </summary>
        public XTextInputFieldElementDateValueEditor()
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
            TextWindowsFormsEditorHostClass inputHost,
            ElementValueEditContext context)
        {
            var host = (TextWindowsFormsEditorHostClass)inputHost;
            DomInputFieldElement field = (DomInputFieldElement)context.Element;
            //InputFieldSettings settings = field.FieldSettings;

            // 编辑日期值
            string strValue = field.InnerValue;
            if (strValue == null || strValue.Length == 0)
            {
                strValue = field.Text;
            }
            DateTime dtmNow = field.OwnerDocument.GetNowDateTime();
            DateTime oldDate = dtmNow;
            //DateTime newDate = dtmNow;
            //bool modified = false;
            string strDisplayFormat = null;
            if (field.DisplayFormat != null)
            {
                strDisplayFormat = field.DisplayFormat.Format;
            }
            if (DateTime.TryParseExact(
                strValue,
                strDisplayFormat,
                System.Globalization.CultureInfo.InvariantCulture,
                System.Globalization.DateTimeStyles.None,
                out oldDate) == false)
            {
                if (strValue == string.Empty || DateTime.TryParse(strValue, out dtmNow) == false)//判断输入的内容是否为空或者是否是时间格式 要是条件不满足 则返回系统时间
                {
                    oldDate = dtmNow;
                }
                else
                {
                    oldDate = DateTime.Parse(strValue);
                }
                //oldDate = dtmNow;
            }
            host.EditControl.WASMParent.JSShowDateTimeControl(
                field, 
                oldDate, 
                InputFieldEditStyle.Date, 
                new Action<DateTime>(delegate( DateTime newDate2) {
                if(newDate2 != oldDate)
                {
                    ApplyNewValue(newDate2 , field , host );
                }
            }));
            return ElementValueEditResult.OpeingUI;
        }

        private ElementValueEditResult ApplyNewValue(
            DateTime newDate ,
            DomInputFieldElement field , 
            TextWindowsFormsEditorHostClass host )
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
                else
                {
                    newText = newDate.ToString(WriterConst.Format_YYYY_MM_DD);
                }
                    newValue = newDate.ToString(WriterConst.Format_YYYY_MM_DD);
            }


            if (field.Text != newText)
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
                if (field.SetEditorText(args3))
                {
                    field.InnerValue = newValue;
                    host.Document.EndLogUndo();
                    host.RaiseDocumentContentChangedOnceAfterEditValue = true;
                    host.EndEditingValue();
                    return ElementValueEditResult.UserAccept;
                }
                else
                {
                    host.Document.CancelLogUndo();
                    return ElementValueEditResult.UserCancel;
                }
            }
            return ElementValueEditResult.UserCancel;
        }
    }
}
