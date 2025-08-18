using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using DCSoft.Writer.Dom;
using DCSoft.Writer.Controls;
using DCSoft.Writer.Serialization;
using DCSoft.Writer.Commands;
using DCSoft.Drawing;
using DCSoft.Printing;
using System.Runtime.InteropServices;
using DCSoft.Common;
using DCSoft.Writer.Data;
using System.Collections;
using System.IO;
using System.Reflection;

namespace DCSoft.Writer
{

    /// <summary>
    /// 编辑器的一些工具方法
    /// </summary>
    internal sealed class WriterTools : DCSoft.Writer.WriterToolsBase
    {
        public WriterTools()
        {

        }
       

        public override ElementValueEditor CreateElementValueEditor(DomElement element)
        {
            DomInputFieldElement field = element as DomInputFieldElement;
            if (field != null)
            {
                if (field.FieldSettings != null)
                {
                    if (field.FieldSettings.EditStyle == InputFieldEditStyle.Date)
                    {
                        return new XTextInputFieldElementDateValueEditor();
                    }
                    else if (field.FieldSettings.EditStyle == InputFieldEditStyle.DateTime)
                    {
                        return new XTextInputFieldElementDateTimeValueEditor();
                    }
                    else if (field.FieldSettings.EditStyle == InputFieldEditStyle.Time)
                    {
                        return new XTextInputFieldElementTimeValueEditor();
                    }
                    else if (field.FieldSettings.EditStyle == InputFieldEditStyle.DropdownList)
                    {
                        return new XTextInputFieldElementListValueEditor();
                    }
                    else if (field.FieldSettings.EditStyle == InputFieldEditStyle.Numeric)
                    {
                        return new XTextInputFieldElementNumericValueEditor();
                    }
                }
            }
            return null;
        }

        public override string FormatSelectedValueByIndexs(
            WriterControl ctl,
            DomInputFieldElement element,
            int[] indexs,
            bool getText)
        {
            return XTextInputFieldElementListValueEditor.FormatSelectedValueByIndexs(ctl, element, indexs, getText);
        }
    }
}
