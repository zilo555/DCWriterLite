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
    internal class XTextInputFieldElementNumericValueEditor : ElementValueEditor
    {
        /// <summary>
        /// 初始化对象
        /// </summary>
        public XTextInputFieldElementNumericValueEditor()
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
            if (host.EditControl != null
                   && host.EditControl.DocumentOptions.BehaviorOptions.EnableCalculateControl == false)
            {
                // 禁止数字小键盘
                return ElementValueEditorEditStyle.None;
            }
            else
            {
                return ElementValueEditorEditStyle.DropDown;
            }
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
            if (host.EditControl != null
                && host.EditControl.DocumentOptions.BehaviorOptions.EnableCalculateControl == false)
            {
                // 禁止数字小键盘
                return ElementValueEditResult.UserCancel;
            }
            DomInputFieldElement field = (DomInputFieldElement)context.Element;
            //InputFieldSettings settings = field.FieldSettings;

            // 编辑日期值
            string strValue = field.InnerValue;
            if (string.IsNullOrEmpty(strValue))
            {
                strValue = field.Text;
            }
            double dblValue = 0;
            if (double.TryParse(strValue, out dblValue) == false)
            {
                dblValue = double.NaN;
            }
            host.EditControl.WASMParent.JSShowCalculateControl(
                field,
                dblValue, 
                new Action<double>(delegate (double newDblValue) { 
                   if( dblValue != newDblValue )
                    {
                        ApplyNewValue(newDblValue, field, host);
                    }
            }));
            return ElementValueEditResult.OpeingUI;
        }

        private ElementValueEditResult ApplyNewValue(
            double dblValue,
            DomInputFieldElement field,
            TextWindowsFormsEditorHostClass host)
        {
            string newValue = null;
            string newText = null;
            if (DoubleNaN.IsNaN(dblValue))
            {
                newText = string.Empty;
                newValue = string.Empty;
            }
            else
            {
                if (field.DisplayFormat != null && field.DisplayFormat.IsEmpty == false)
                {
                    newText = field.DisplayFormat.Execute(dblValue.ToString());
                }
                else
                {
                    newText = dblValue.ToString();
                }
                if (host.EditControl.DocumentOptions.BehaviorOptions.DisplayFormatToInnerValue
                    && field.DisplayFormat != null
                    && field.DisplayFormat.IsEmpty == false)
                {
                    newValue = field.DisplayFormat.Execute(dblValue.ToString());
                }
                else
                {
                    newValue = dblValue.ToString();
                }
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
