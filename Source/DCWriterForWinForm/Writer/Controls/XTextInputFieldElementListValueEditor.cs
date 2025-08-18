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
    /// 列表方式的文本输入域编辑器对象
    /// </summary>
    internal class XTextInputFieldElementListValueEditor : ElementValueEditor
    {
        /// <summary>
        /// 初始化对象
        /// </summary>
        public XTextInputFieldElementListValueEditor()
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
        /// <param name="host">宿主对象</param>
        /// <param name="context">上下文对象</param>
        /// <returns>操作是否成功</returns>
        public override ElementValueEditResult EditValue(
            TextWindowsFormsEditorHostClass host,
            ElementValueEditContext context)
        {
            if (host.EditControl.Modified == false)
            {
            }
            if (context.WriterControl != null)
            {

            }
            DomInputFieldElement field = context.Element as DomInputFieldElement;
            if (field != null)
            {
                var list = MyQueryList(field);
                var result = ShowDropdownList((TextWindowsFormsEditorHostClass)host, context, field, list);
                return result;
            }
            return ElementValueEditResult.UserCancel;
        }

        private ListItemCollection MyQueryList(DomInputFieldElement field)
        {
            return field.FieldSettings?.ListSource?.Items;
        }
        private ElementValueEditResult ShowDropdownList(
            TextWindowsFormsEditorHostClass host,
            ElementValueEditContext context,
            DomInputFieldElement field,
            ListItemCollection list)
        {
            if (host.EditControl == null || host.EditControl.HasValidateInnerViewControl() == false)
            {
                return ElementValueEditResult.None;
            }
            //InputFieldSettings settings = field.FieldSettings;
            if (host == null
                || host.EditControl == null
                || host.EditControl.IsDisposed
                || host.EditControl.IsHandleCreated == false)
            {
                return ElementValueEditResult.None;
            }
            field.OwnerDocument.CacheOptions();
            host.EditControl.Focus();
            if (list == null)
            {
                list = new ListItemCollection();
            }

            string strValue = field.InnerValue;
            if (string.IsNullOrEmpty(strValue))
            {
                strValue = field.Text;
            }
            host.EditControl.JS_BeginEditValueUseListBoxControl(
                field, 
                list , 
                new Action<string,string>(delegate (string newText , string newValue ) {
                    ApplyNewValue(field, newText, newValue);
            }));
            return ElementValueEditResult.OpeingUI;
        }

        private ElementValueEditResult ApplyNewValue(
          DomInputFieldElement field,
          string newText,
          string newValue)
        {
            string oldText = field.Text;
            string oldValue = field.InnerValue;
             

            if (field.GetDocumentBehaviorOptions().ForceRaiseEventAfterFieldContentEdit // 指定强制触发事件
                || field.Text != newText)
            {
                //int indexCount = -1;
                StringBuilder strIndexs = new StringBuilder();
                ListItemCollection selectedItems = new ListItemCollection();
                var document = field.OwnerDocument;
                document.BeginLogUndo();
                DomInputFieldElementBase.EventBeforeOnContentChangedOnce = delegate (
                    object eventSender, EventArgs args2)
                {
                    // 设置字段的InnerValue属性值
                    if (field.InnerValue != newValue)
                    {
                        if (document.CanLogUndo)
                        {
                            document.UndoList.AddProperty("InnerValue",
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
                args3.RaiseDocumentContentChangedEvent = true;
                args3.FocusContainer = true;
                args3.EventSource = ContentChangedEventSource.ValueEditor;
                if (field.SetEditorText(args3)
                    || field.GetDocumentBehaviorOptions().ForceRaiseEventAfterFieldContentEdit) // 指定强制触发事件
                {
                    field.AddExtTextValueMap(newText, newValue);
                    document.EndLogUndo();
                    field.WriterControl.Focus();
                    return ElementValueEditResult.UserAccept;
                }
                else
                {
                    document.CancelLogUndo();
                    return ElementValueEditResult.UserAccept;
                }
            }
            return ElementValueEditResult.UserCancel;
        }


        internal static string FormatSelectedValueByIndexs(
            WriterControl ctl,
            DomInputFieldElement element,
            int[] indexs,
            bool getText)
        {
            List<string> selectedItems = new List<string>();
            List<string> unSelectedItems = new List<string>();
            StringBuilder myStr = new StringBuilder();
            ListItemCollection listItems = element.GetRuntimeListItems();
            if (listItems == null || listItems.Count == 0)
            {
                return null;
            }
            for (int iCount = 0; iCount < listItems.Count; iCount++)
            {
                ListItem item2 = listItems[iCount];
                bool selected = Array.IndexOf(indexs, iCount) >= 0;
                string v = getText ? item2.Text : item2.Value;
                if (string.IsNullOrEmpty(v))
                {
                    v = item2.Text;
                }
                if (string.IsNullOrEmpty(v) == false)
                {
                    if (selected)
                    {
                        selectedItems.Add(v);
                        if (myStr.Length > 0)
                        {
                            myStr.Append(element.FieldSettings.ListValueSeparatorChar);
                        }
                        myStr.Append(v);
                    }
                    else
                    {
                        unSelectedItems.Add(v);
                    }
                }
            }//for
            string spc = element.FieldSettings.ListValueSeparatorChar;
            FormatListItemsEventArgs getArgs = new FormatListItemsEventArgs(
                ctl,
                ctl.Document,
                element,
                selectedItems.ToArray(),
                unSelectedItems.ToArray(),
                spc);
            getArgs.Result = myStr.ToString();
            return getArgs.Result;
        }
    }
}
