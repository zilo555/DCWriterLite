
using System;
using System.Collections.Generic;
using System.Text;
using DCSoft.Writer.Dom;
using DCSoft.Writer.Controls;
using System.Windows.Forms;
using DCSoft.Drawing;
using DCSoft.Writer.Undo;
using DCSoft.Common;
using DCSoft.WinForms.Design;
using DCSoft.Writer.Data;
using System.ComponentModel;
using System.IO;
using DCSoft.Writer.Serialization;
using DCSoft.WinForms;

namespace DCSoft.Writer.Commands
{
    /// <summary>
    /// 插入内容编辑器命令容器
    /// </summary>
    /// <remarks>编制 袁永福</remarks>
    internal sealed class WriterCommandModuleInsert : WriterCommandModule
    {
        /// <summary>
        /// 初始化对象
        /// </summary>
        public WriterCommandModuleInsert()
        {
        }
        // DCSoft.Writer.Commands.WriterCommandModuleInsert
        protected override WriterCommandList CreateCommands()
        {
            var list = new WriterCommandList();
            //AddCommandToList(list, StandardCommandNames.BeginInsertKBEntryByPreWord, this.BeginInsertKBEntryByPreWord, System.Windows.Forms.Keys.F4);
            AddCommandToList(list, StandardCommandNames.InsertCheckBox, this.InsertCheckBox);
            AddCommandToList(list, StandardCommandNames.InsertCheckBoxList, this.InsertCheckBoxList);
            AddCommandToList(list, StandardCommandNames.InsertElements, this.InsertElements);
            AddCommandToList(list, StandardCommandNames.InsertFileContent, this.InsertFileContent);
            AddCommandToList(list, StandardCommandNames.InsertImage, this.InsertImage);
            AddCommandToList(list, StandardCommandNames.InsertImageExt, this.InsertImageExt);
            AddCommandToList(list, StandardCommandNames.InsertInputField, this.InsertInputField);
            AddCommandToList(list, StandardCommandNames.InsertLineBreak, this.InsertLineBreak, System.Windows.Forms.Keys.Return | System.Windows.Forms.Keys.Shift);
            AddCommandToList(list, StandardCommandNames.InsertPageBreak, this.InsertPageBreak);
            AddCommandToList(list, StandardCommandNames.InsertPageInfo, this.InsertPageInfo);
            AddCommandToList(list, StandardCommandNames.InsertParagrahFlag, this.InsertParagrahFlag);
            AddCommandToList(list, StandardCommandNames.InsertRadioBox, this.InsertRadioBox);
            AddCommandToList(list, StandardCommandNames.InsertString, this.InsertString);
            AddCommandToList(list, StandardCommandNames.InsertXML, this.InsertXML);
            AddCommandToList(list, StandardCommandNames.InsertXMLWithClearFontNameSize, this.InsertXMLWithClearFontNameSize);
            AddCommandToList(list, StandardCommandNames.InsertXMLWithClearFormat, this.InsertXMLWithClearFormat);
            return list;
        }

        /// <summary>
        /// 插入若干个文档元素
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        [WriterCommandDescription(StandardCommandNames.InsertElements)]
        private void InsertElements(object sender, WriterCommandEventArgs args)
        {
            if (args.Mode == WriterCommandEventMode.QueryState)
            {
                args.Enabled = false;
                if (args.DocumentControler != null)
                {
                    args.Enabled = args.DocumentControler.CanInsertElementAtCurrentPosition(typeof(DomElement));
                }
            }
            else if (args.Mode == WriterCommandEventMode.Invoke)
            {
                args.Result = 0;
                DomElementList elements = args.Parameter as DomElementList;
                if (elements == null)
                {
                    if (args.Parameter is DomElement)
                    {
                        elements = new DomElementList();
                        elements.Add((DomElement)args.Parameter);
                    }
                    if (elements == null)
                    {
                        return;
                    }
                }
                if (args.Document.Options.BehaviorOptions.AutoClearTextFormatWhenPasteOrInsertContent)
                {//使用当前样式：DCWRITER-3857
                    foreach (DomElement newElement in elements)
                    {
                        newElement.OwnerDocument = args.Document;
                        newElement.Style = args.Document.InternalCurrentStyle;
                    }
                }
                int result = ((DocumentControler)args.DocumentControler).InsertElements(elements);
                args.RefreshLevel = UIStateRefreshLevel.All;
                args.Result = result;
            }
        }

        /// <summary>
        /// 插入强制分页符
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        [WriterCommandDescription(
            StandardCommandNames.InsertPageBreak,
            ReturnValueType = typeof(bool),
            DefaultResultValue = false)]
        // ImageResource = "DCSoft.Writer.Commands.Images.CommandInsertPageBreak.bmp")]
        private void InsertPageBreak(object sender, WriterCommandEventArgs args)
        {
            if (args.Mode == WriterCommandEventMode.QueryState)
            {
                args.Enabled = false;
                if (args.DocumentControler != null)
                {
                    args.Enabled = args.DocumentControler.CanInsertElementAtCurrentPosition(typeof(DomPageBreakElement));
                }
            }
            else if (args.Mode == WriterCommandEventMode.Invoke)
            {
                args.Result = false;
                var pf = new DomParagraphFlagElement();// (XTextParagraphFlagElement)args.Document.CreateElementByType(typeof(XTextParagraphFlagElement));
                pf.OwnerDocument = args.Document;
                var pb = new DomPageBreakElement();// (XTextPageBreakElement)args.Document.CreateElementByType(typeof(XTextPageBreakElement));
                pb.OwnerDocument = args.Document;
                pf.StyleIndex = args.Document.CurrentParagraphEOF.StyleIndex;
                pb.StyleIndex = -1;
                using (DCGraphics g = args.Document.InnerCreateDCGraphics())
                {
                    InnerDocumentPaintEventArgs args2 = args.Document.CreateInnerPaintEventArgs(g);
                    args2.Element = pf;
                    pf.RefreshSize(args2);
                    args2.Element = pb;
                    pb.RefreshSize(args2);
                }
                DomElementList list = new DomElementList();
                list.Add(pb);
                //list.Add(pf);
                args.Document.BeginLogUndo();
                args.Document.InsertElements(list, true);
                args.Document.Content.MoveToPosition(pb.ContentIndex + 1);
                DomElement element = args.Document.Content.SafeGet(pb.ContentIndex + 1);
                args.Document.Content.MoveToElement(element);
                //args.EditorControl.ScrollToCaretExt(WinForms.ScrollToViewStyle.Middle);
                args.Document.EndLogUndo();
                args.Document.OnDocumentContentChanged();
                args.Result = true;
                args.RefreshLevel = UIStateRefreshLevel.All;
            }
        }



        /// <summary>
        /// 插入图片
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        [WriterCommandDescription(StandardCommandNames.InsertImage,
            // ImageResource = "DCSoft.Writer.Commands.Images.CommandInsertImage.bmp",
            ReturnValueType = typeof(DomImageElement))]
        private void InsertImage(object sender, WriterCommandEventArgs args)
        {
            if (args.Mode == WriterCommandEventMode.QueryState)
            {
                args.Enabled = args.DocumentControler != null
                    && args.DocumentControler.CanInsertElementAtCurrentPosition(
                    typeof(DomImageElement));
            }
            else if (args.Mode == WriterCommandEventMode.Invoke)
            {
                DomImageElement newElement = null;
                if (args.Parameter is DomImageElement)
                {
                    newElement = (DomImageElement)args.Parameter;
                }
                //else if (args.Parameter is string)
                //{
                //    string fileName = (string)args.Parameter;
                //    fileName = fileName.Trim();
                //    if (string.IsNullOrEmpty(fileName) == false)
                //    {
                //        newElement = (XTextImageElement)args.Document.CreateElementByType(typeof(XTextImageElement));
                //        newElement.LoadImage(fileName, false);
                //        newElement.CompressSaveMode = ConfirmCompressSaveModeForFileSize(fileName, args);
                //    }
                //}
                else if (args.Parameter is Image)
                {
                    newElement = new DomImageElement();//(XTextImageElement)args.Document.CreateElementByType(typeof(XTextImageElement));
                    newElement.Image.Value = (Image)args.Parameter;
                }
                else if (args.Parameter is XImageValue)
                {
                    newElement = new DomImageElement();//(XTextImageElement)args.Document.CreateElementByType(typeof(XTextImageElement));
                    newElement.Image = (XImageValue)args.Parameter;
                }
                else if (args.Parameter is byte[])
                {
                    newElement = new DomImageElement();// (XTextImageElement)args.Document.CreateElementByType(typeof(XTextImageElement));
                    XImageValue img = new XImageValue();
                    img.ImageData = (byte[])args.Parameter;
                    newElement.Image = img;
                }
                args.Document.AllocElementID("image", newElement);
                if (newElement != null)
                {
                    newElement.OwnerDocument = args.Document;

                    //伍贻超20180206：如果元素在插入之前就已经设置了宽和高，在这里更新大小，否则设置图片宽高后再命令插入会不起作用
                    bool updateSize = newElement.Height != 0 && newElement.Width != 0;
                    newElement.UpdateSize(updateSize);

                    WriterUtilsInner.CheckImageSizeWhenInsertImage(
                        args.Document,
                        newElement,
                        newElement.KeepWidthHeightRate,
                        null);
                    args.Document.ValidateElementIDRepeat(newElement);
                    args.DocumentControler.InsertElement(newElement);
                    //args.Document.OnDocumentContentChanged();
                    args.Result = newElement;
                    args.RefreshLevel = UIStateRefreshLevel.All;
                }
            }
        }

        /// <summary>
        /// 直接插入图片
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        [WriterCommandDescription(StandardCommandNames.InsertImageExt,
            // ImageResource = "DCSoft.Writer.Commands.Images.CommandInsertImage.bmp",
            ReturnValueType = typeof(DomImageElement))]
        private void InsertImageExt(object sender, WriterCommandEventArgs args)
        {
            if (args.Mode == WriterCommandEventMode.QueryState)
            {
                args.Enabled = args.DocumentControler != null
                    && args.DocumentControler.CanInsertElementAtCurrentPosition(
                    typeof(DomImageElement));
            }
            else if (args.Mode == WriterCommandEventMode.Invoke)
            {
                DomImageElement newElement = null;
                if (args.Parameter is DomImageElement)
                {
                    newElement = (DomImageElement)args.Parameter;
                }
                else if (args.Parameter is Image)
                {
                    newElement = new DomImageElement();//(XTextImageElement)args.Document.CreateElementByType(typeof(XTextImageElement));
                    newElement.Image.Value = (Image)args.Parameter;
                }
                else if (args.Parameter is XImageValue)
                {
                    newElement = new DomImageElement();//(XTextImageElement)args.Document.CreateElementByType(typeof(XTextImageElement));
                    newElement.Image = (XImageValue)args.Parameter;
                }
                else if (args.Parameter is byte[])
                {
                    newElement = new DomImageElement();// (XTextImageElement)args.Document.CreateElementByType(typeof(XTextImageElement));
                    XImageValue img = new XImageValue();
                    img.ImageData = (byte[])args.Parameter;
                    newElement.Image = img;
                }
                if (newElement != null && string.IsNullOrEmpty(newElement.ID))
                {
                    args.Document.AllocElementID("image", newElement);
                }
                if (newElement != null)
                {
                    newElement.OwnerDocument = args.Document;
                    newElement.UpdateSize(false);
                    WriterUtilsInner.CheckImageSizeWhenInsertImage(
                        args.Document,
                        newElement,
                        newElement.KeepWidthHeightRate,
                        null);
                    args.DocumentControler.InsertElement(newElement);
                    //args.Document.OnDocumentContentChanged();
                    args.Result = newElement;
                    args.RefreshLevel = UIStateRefreshLevel.All;
                }
            }
        }

        /// <summary>
        /// 插入复选框元素的动作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        [WriterCommandDescription(
            StandardCommandNames.InsertCheckBox,
            // ImageResource = "DCSoft.Writer.Dom.XTextCheckBoxElement.bmp",
            ReturnValueType = typeof(DomCheckBoxElement))]
        private void InsertCheckBox(object sender, WriterCommandEventArgs args)
        {
            if (args.Mode == WriterCommandEventMode.QueryState)
            {
                args.Enabled = args.DocumentControler != null
                    && args.DocumentControler.CanInsertElementAtCurrentPosition(typeof(DomCheckBoxElement));
            }
            else if (args.Mode == WriterCommandEventMode.Invoke)
            {
                DomCheckBoxElement newElement = null;
                if (args.Parameter is DomCheckBoxElement)
                {
                    newElement = (DomCheckBoxElement)args.Parameter;
                }
                if (newElement == null)
                {
                    newElement = new DomCheckBoxElement();// (XTextCheckBoxElement)args.Document.CreateElementByType(typeof(XTextCheckBoxElement));
                }
                newElement.OwnerDocument = args.Document;
                if (string.IsNullOrEmpty(newElement.ID))
                {
                    args.Document.AllocElementID("checkbox", newElement);
                }
                if (args.ShowUI)
                {
                    if (WriterAppHost.CallElementEdtior(
                        args,
                        newElement,
                        ElementPropertiesEditMethod.Insert) == false)
                    {
                        newElement.Dispose();
                        //newElement = null;
                        return;
                    }
                }
                if (newElement != null)
                {
                    SetNewElementStyleIndexForAutoClearTextFormatWhenPasteOrInsertContent(newElement, args.Document);
                    newElement.OwnerDocument = args.Document;
                    // XTextElement element = args.Document.CurrentElement;
                    //newElement.StyleIndex = element.StyleIndex;
                    args.Document.ValidateElementIDRepeat(newElement);
                    WriterUtilsInner.RefreshElementsSize(args.Document, new DomElementList(newElement), true);
                    WriterUtilsInner.CheckImageSizeWhenInsertImage(
                        args.Document,
                        newElement,
                        false,
                        null);
                    args.Document.DocumentControler.InsertElement(newElement);
                    args.RefreshLevel = UIStateRefreshLevel.All;
                    args.Result = newElement;
                }

            }
        }
        /// <summary>
        /// 插入复选框元素的动作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        [WriterCommandDescription(
            StandardCommandNames.InsertRadioBox,
            ReturnValueType = typeof(DomRadioBoxElement))]
        private void InsertRadioBox(object sender, WriterCommandEventArgs args)
        {
            if (args.Mode == WriterCommandEventMode.QueryState)
            {
                args.Enabled = args.DocumentControler != null
                    && args.DocumentControler.CanInsertElementAtCurrentPosition(typeof(DomRadioBoxElement));
            }
            else if (args.Mode == WriterCommandEventMode.Invoke)
            {
                DomRadioBoxElement newElement = null;
                if (args.Parameter is DomRadioBoxElement)
                {
                    newElement = (DomRadioBoxElement)args.Parameter;
                }
                if (newElement == null)
                {
                    newElement = new DomRadioBoxElement();// (XTextRadioBoxElement)args.Document.CreateElementByType(typeof(XTextRadioBoxElement));
                }
                newElement.OwnerDocument = args.Document;
                if (args.ShowUI)
                {
                    if (WriterAppHost.CallElementEdtior(
                        args,
                        newElement,
                        ElementPropertiesEditMethod.Insert) == false)
                    {
                        newElement.Dispose();
                        newElement = null;
                        return;
                    }
                }
                if (newElement != null)
                {
                    if (string.IsNullOrEmpty(newElement.ID))
                    {
                        args.Document.AllocElementID("radio", newElement);
                    }
                    //XTextElement element = args.Document.CurrentElement;
                    //newElement.StyleIndex = element.StyleIndex;
                    SetNewElementStyleIndexForAutoClearTextFormatWhenPasteOrInsertContent(newElement, args.Document);
                    args.Document.ValidateElementIDRepeat(newElement);
                    args.Document.DocumentControler.InsertElement(newElement);
                    args.RefreshLevel = UIStateRefreshLevel.All;
                    args.Result = newElement;
                }

            }
        }

        /// <summary>
        /// 插入复选框列表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        [WriterCommandDescription(
            StandardCommandNames.InsertCheckBoxList,
            ReturnValueType = typeof(DomElementList))]
        private void InsertCheckBoxList(object sender, WriterCommandEventArgs args)
        {
            if (args.Mode == WriterCommandEventMode.QueryState)
            {
                args.Enabled = args.DocumentControler != null
                    && args.DocumentControler.CanInsertElementAtCurrentPosition(typeof(DomInputFieldElementBase));
            }
            else if (args.Mode == WriterCommandEventMode.Invoke)
            {
                DomInputFieldElement newElement = null;
                if (args.Parameter is DomInputFieldElement)
                {
                    newElement = (DomInputFieldElement)args.Parameter;
                }
                else
                {
                    XTextInputFieldElementProperties properties = null;
                    properties = args.Parameter as XTextInputFieldElementProperties;
                    if (args.Parameter is XTextInputFieldElementProperties)
                    {
                        properties = (XTextInputFieldElementProperties)args.Parameter;
                    }
                    if (properties == null)
                    {
                        properties = new XTextInputFieldElementProperties();// (XTextInputFieldElementProperties)args.Host.CreateProperties(typeof(XTextInputFieldElement));
                    }

                    properties.Document = args.Document;
                    if (args.Parameter is InputFieldSettings)
                    {
                        properties.FieldSettings = (InputFieldSettings)args.Parameter;
                    }
                    else if (args.Parameter is ValueValidateStyle)
                    {
                        properties.ValidateStyle = (ValueValidateStyle)args.Parameter;
                    }
                    newElement = (DomInputFieldElement)properties.CreateElement(args.Document);
                }
                DomElementList newElements = new DomElementList();
                var lsItems = newElement.FieldSettings?.ListSource?.Items;
                if (lsItems != null)
                {
                    foreach (ListItem item in lsItems)
                    {
                        DomCheckBoxElement chk = new DomCheckBoxElement();
                        chk.Name = newElement.Name;
                        chk.CheckedValue = item.Value;
                        chk.ID = newElement.ID;
                        //chk.Value = item.Value;
                        newElements.Add(chk);
                        if (string.IsNullOrEmpty(item.Text) == false)
                        {
                            foreach (char c in item.Text)
                            {
                                DomCharElement ce = new DomCharElement();
                                ce.CharValue = c;
                                newElements.Add(ce);
                            }
                        }
                    }//foreach
                }
                if (newElements.Count > 0)
                {
                    int si = args.Document.ContentStyles.GetStyleIndex(args.Document.CurrentStyleInfo.Content);
                    foreach (DomElement element in newElements)
                    {
                        element.StyleIndex = si;
                        element.OwnerDocument = args.Document;
                    }
                    ((DocumentControler)args.DocumentControler).InsertElements(newElements);
                    args.RefreshLevel = UIStateRefreshLevel.All;
                    args.Result = newElements;
                }
            }
        }

        /// <summary>
        /// 插入页码信息元素
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        [WriterCommandDescription(
            StandardCommandNames.InsertPageInfo,
            ReturnValueType = typeof(DomPageInfoElement))]
        private void InsertPageInfo(object sender, WriterCommandEventArgs args)
        {
            if (args.Mode == WriterCommandEventMode.QueryState)
            {
                args.Enabled = args.DocumentControler != null
                    && ((DocumentControler)args.DocumentControler).CanInsertElementAtCurrentPosition(typeof(DomPageInfoElement));
            }
            else if (args.Mode == WriterCommandEventMode.Invoke)
            {
                PageInfoValueType type = PageInfoValueType.PageIndex;
                bool autoHeight = true;
                DomPageInfoElement newElement = null;
                if (args.Parameter is DomPageInfoElement)
                {
                    newElement = (DomPageInfoElement)args.Parameter;
                    type = newElement.ValueType;
                    autoHeight = newElement.AutoHeight;
                }
                else if (args.Parameter is PageInfoValueType)
                {
                    type = (PageInfoValueType)args.Parameter;
                }
                else if (args.Parameter is string && args.Parameter.ToString().Length > 0)
                {
                    try
                    {
                        type = (PageInfoValueType)Enum.Parse(
                            typeof(PageInfoValueType),
                            (string)args.Parameter,
                            true);
                        newElement.ValueType = type;
                    }
                    catch
                    {
                    }
                }
                args.Document.AllocElementID("page", newElement);
                if (newElement == null)
                {
                    newElement = new DomPageInfoElement();// (XTextPageInfoElement)args.Document.CreateElementByType(typeof(XTextPageInfoElement));
                    newElement.AutoHeight = true;
                }
                newElement.OwnerDocument = args.Document;
                newElement.ValueType = type;
                newElement.AutoHeight = autoHeight;
                newElement.Style = WriterCommandModuleFormat.GetCurrentStyle(args.Document);
                SetNewElementStyleIndexForAutoClearTextFormatWhenPasteOrInsertContent(newElement, args.Document);
                args.DocumentControler.InsertElement(newElement);
                args.RefreshLevel = UIStateRefreshLevel.All;
                args.Result = newElement;
            }
        }
        /// <summary>
        /// 插入文本输入域
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        [WriterCommandDescription(
    StandardCommandNames.InsertInputField,
    // ImageResource = "DCSoft.Writer.Dom.XTextInputFieldElement.bmp",
    ReturnValueType = typeof(DomInputFieldElement))]
        private void InsertInputField(object sender, WriterCommandEventArgs args)
        {
            if (args.Mode == WriterCommandEventMode.QueryState)
            {
                args.Enabled = args.DocumentControler != null
                    && args.DocumentControler.CanInsertElementAtCurrentPosition(
                    typeof(DomInputFieldElementBase));
            }
            else if (args.Mode == WriterCommandEventMode.Invoke)
            {
                DomInputFieldElement newElement = null;
                //if (args.Parameter is string)
                //{
                //    string xml = (string)args.Parameter;
                //    if (xml.IndexOf("<XInputField") >= 0)
                //    {
                //        // 根据XML片段创建输入域对象
                //        newElement = args.Document.CreateElementByXMLFragment(xml) as XTextInputFieldElement;
                //    }
                //}
                //else 
                if (args.Parameter is DomInputFieldElement)
                {
                    newElement = (DomInputFieldElement)args.Parameter;
                }
                else if (args.Parameter is InputFieldSettings)
                {
                    newElement = new DomInputFieldElement();//(XTextInputFieldElement)args.Document.CreateElementByType(typeof(XTextInputFieldElement));
                    newElement.FieldSettings = (InputFieldSettings)args.Parameter;
                }
                else if (args.Parameter is ValueValidateStyle)
                {
                    newElement = new DomInputFieldElement();// (XTextInputFieldElement)args.Document.CreateElementByType(typeof(XTextInputFieldElement));
                    newElement.ValidateStyle = (ValueValidateStyle)args.Parameter;
                }
                //else if (args.Parameter is XTextInputFieldElementProperties)
                //{
                //    XTextInputFieldElementProperties p = (XTextInputFieldElementProperties)args.Parameter;
                //    newElement = ( XTextInputFieldElement ) p.CreateElement(args.Document);
                //}
                if (newElement == null)
                {
                    newElement = new DomInputFieldElement();// (XTextInputFieldElement)args.Document.CreateElementByType(typeof(XTextInputFieldElement));
                }
                newElement.OwnerDocument = args.Document;
                args.Document.AllocElementID("field", newElement);
                if (args.ShowUI)
                {
                    // 显示用户界面
                    if (WriterAppHost.CallElementEdtior(
                        args,
                        newElement,
                        ElementPropertiesEditMethod.Insert) == false)
                    {
                        newElement.Dispose();
                        newElement = null;
                    }
                }
                if (newElement != null)
                {
                    if (SetNewElementStyleIndexForAutoClearTextFormatWhenPasteOrInsertContent(newElement, args.Document) == false)
                    {
                        if (newElement.Style != null)
                        {
                            newElement.OwnerDocument = args.Document;
                            newElement.CommitStyle(true);
                        }
                        else if (newElement.StyleIndex == -1)
                        {
                            // 没有指定样式，则采用当前样式
                            newElement.StyleIndex = args.Document.ContentStyles.GetStyleIndex(args.Document.CurrentStyleInfo.ContentStyleForNewString);
                        }
                    }
                    newElement.StartElement.StyleIndex = newElement.StyleIndex;
                    newElement.EndElement.StyleIndex = newElement.StyleIndex;
                    newElement.OwnerDocument = args.Document;
                    foreach (DomElement sube in newElement.Elements)
                    {
                        sube.StyleIndex = newElement.StyleIndex;
                    }
                    args.Document.ValidateElementIDRepeat(newElement);
                    args.DocumentControler.InsertElement(newElement);
                    args.RefreshLevel = UIStateRefreshLevel.All;
                    args.Result = newElement;
                }
            }
        }


        internal static bool SetNewElementStyleIndexForAutoClearTextFormatWhenPasteOrInsertContent(DomElement element, DomDocument document)
        {
            if (document.Options.BehaviorOptions.AutoClearTextFormatWhenPasteOrInsertContent)
            {
                // 插入元素时清除文本格式，采用插入点处的文本格式
                element.StyleIndex = document.ContentStyles.GetStyleIndex(document.CurrentStyleInfo.ContentStyleForNewString);
                return true;
            }
            return false;
        }
        /// <summary>
        /// 插入文件内容
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        [WriterCommandDescription(
    StandardCommandNames.InsertFileContent,
    ReturnValueType = typeof(DomElementList))]
        private void InsertFileContent(object sender, WriterCommandEventArgs args)
        {
            if (args.Mode == WriterCommandEventMode.QueryState)
            {
                args.Enabled = args.DocumentControler != null
                    && args.Document != null
                    && args.DocumentControler.CanInsertElementAtCurrentPosition(
                    typeof(DomElement));
            }
            else if (args.Mode == WriterCommandEventMode.Invoke)
            {
                args.Result = null;
                DomDocument document = null;
                string fileName = null;
                if (args.Parameter is string)
                {
                    fileName = (string)args.Parameter;
                }
                else if (args.Parameter is DomDocument)
                {
                    document = (DomDocument)args.Parameter;
                }
                else if (args.Parameter is Stream)
                {
                    Stream stream = (Stream)args.Parameter;
                    document = (DomDocument)args.Document.Clone(false);
                    document.Load(stream, null);
                }
                else if (args.Parameter is TextReader)
                {
                    TextReader reader = (TextReader)args.Parameter;
                    document = (DomDocument)args.Document.Clone(false);
                    document.Load(reader, null);
                }
                else if (args.Parameter is byte[])
                {
                    System.IO.MemoryStream ms = new MemoryStream((byte[])args.Parameter);
                    document = (DomDocument)args.Document.Clone(false);
                    document.Load(ms, null);
                }

                if (document != null
                    && document.Body != null
                    && document.Body.Elements.Count > 0)
                {
                    // 导入文档内容
                    DomElementList list = document.Body.Elements;
                    args.Document.ApplyRemoveLastParagraphFlagWhenInsertDocument(list);
                    if (list.Count > 0)
                    {
                        args.Document.ImportElements(list);
                        ((DocumentControler)args.DocumentControler).InsertElements(list);
                        args.Result = list;
                    }
                }
            }
        }

        /// <summary>
        /// 向文档的当前位置插入XML内容。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        [WriterCommandDescription(
            StandardCommandNames.InsertXML,
            ReturnValueType = typeof(DomElementList))]
        private void InsertXML(object sender, WriterCommandEventArgs args)
        {
            if (args.Mode == WriterCommandEventMode.QueryState)
            {
                args.Enabled = args.DocumentControler != null
                    && args.Document != null
                    && args.DocumentControler.CanInsertElementAtCurrentPosition(
                    typeof(DomElement));
            }
            else if (args.Mode == WriterCommandEventMode.Invoke)
            {
                InnerInsertXML(args, 0);
            }
        }

        /// <summary>
        /// 向文档的当前位置插入XML内容。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        [WriterCommandDescription(
            StandardCommandNames.InsertXMLWithClearFormat,
            ReturnValueType = typeof(DomElementList))]
        private void InsertXMLWithClearFormat(object sender, WriterCommandEventArgs args)
        {
            if (args.Mode == WriterCommandEventMode.QueryState)
            {
                args.Enabled = args.DocumentControler != null
                    && args.Document != null
                    && args.DocumentControler.CanInsertElementAtCurrentPosition(
                    typeof(DomElement));
            }
            else if (args.Mode == WriterCommandEventMode.Invoke)
            {
                InnerInsertXML(args, 1);
            }
        }

        /// <summary>
        /// 向文档的当前位置插入XML内容。并只清理字体名称和大小
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        [WriterCommandDescription(
            StandardCommandNames.InsertXMLWithClearFontNameSize,
            ReturnValueType = typeof(DomElementList))]
        private void InsertXMLWithClearFontNameSize(object sender, WriterCommandEventArgs args)
        {
            if (args.Mode == WriterCommandEventMode.QueryState)
            {
                args.Enabled = args.DocumentControler != null
                    && args.Document != null
                    && args.DocumentControler.CanInsertElementAtCurrentPosition(
                    typeof(DomElement));
            }
            else if (args.Mode == WriterCommandEventMode.Invoke)
            {
                InnerInsertXML(args, 2);
            }
        }

        private void InnerInsertXML(WriterCommandEventArgs args, int clearFormatType)
        {
            args.Result = false;
            DomDocument document = null;
            if (args.Parameter is string)
            {
                string xml = (string)args.Parameter;
                xml = xml.Trim();
                //wyc20230209:防止导出来的XML字符串开头有看不见的字符导致报错
                int startindex = xml.IndexOf('<');
                if (startindex == -1)
                {
                    return;
                }
                else if (startindex > 0)
                {
                    xml = xml.Substring(startindex, xml.Length - startindex);
                }
                //if (xml.StartsWith("<") == false)
                //{
                //    return;
                //}
                ///////////////////////////////////////////////////////////
                System.IO.StringReader reader = new System.IO.StringReader(
                    xml);
                document = new DomDocument();// (XTextDocument)args.Document.CreateElementByType(args.Document.GetType());
                document.Load(reader, WriterConst.Format_XML, true);
                reader.Close();
            }
            else if (args.Parameter is System.IO.Stream)
            {
                document = new DomDocument();// (XTextDocument)args.Document.CreateElementByType(args.Document.GetType());
                document.Load((System.IO.Stream)args.Parameter, WriterConst.Format_XML, true);
            }
            else if (args.Parameter is System.IO.TextReader)
            {
                document = new DomDocument();// (XTextDocument)args.Document.CreateElementByType(args.Document.GetType());
                document.Load((System.IO.TextReader)args.Parameter, WriterConst.Format_XML, true);
            }
            args.Result = InsertDocument(args.Document, document, clearFormatType, true, args.ShowUI);
        }

        //private XTextElementList InsertDocument(
        //    WriterCommandEventArgs args,
        //    XTextDocument sourceDocument,
        //    int clearFormatType,
        //    bool checkImportDocument)
        //{
        //    if (sourceDocument == null)
        //    {
        //        return null;
        //    }
        //    if (sourceDocument.Body == null)
        //    {
        //        return null;
        //    }
        //    if (sourceDocument.Body.Elements.Count == 0)
        //    {
        //        return null;
        //    }
        //    if (sourceDocument.Body.Elements.Count == 1
        //        && sourceDocument.Body.Elements[0] is XTextParagraphFlagElement)
        //    {
        //        return null;
        //    }

        //    //if (args.UIStartEditContent() == false)
        //    //{
        //    //    // 无法启用UI层编辑操作
        //    //    return null;
        //    //}
        //    if (checkImportDocument)
        //    {
        //        if (args.DocumentControler.CheckImportDocumen(
        //            sourceDocument,
        //            args.ShowUI) == false)
        //        {
        //            return null;
        //        }
        //    }
        //    sourceDocument.FixDomState();
        //    XTextElementList list = sourceDocument.Body.Elements.Clone();
        //    if (clearFormatType == 1)
        //    {
        //        // 清空格式
        //        sourceDocument.ContentStyles.Styles.Clear();
        //        sourceDocument.ContentStyles.Default = args.Document.ContentStyles.Default;
        //        int si1 = -1;
        //        int si2 = -1;
        //        si1 = args.Document.ContentStyles.GetStyleIndex(
        //               args.Document.CurrentStyleInfo.Content);
        //        si2 = args.Document.ContentStyles.GetStyleIndex(
        //            args.Document.CurrentStyleInfo.Paragraph);
        //        DomTreeNodeEnumerable enumer = new DomTreeNodeEnumerable(list);
        //        enumer.ExcludeParagraphFlag = false;
        //        enumer.ExcludeCharElement = false;
        //        foreach (XTextElement element in enumer)
        //        {
        //            if (element is XTextParagraphFlagElement)
        //            {
        //                element.StyleIndex = si2;
        //            }
        //            else
        //            {
        //                element.StyleIndex = si1;
        //            }
        //        }
        //        //WriterUtils.Enumerate(list, delegate(
        //        //        object sender,
        //        //        ElementEnumerateEventArgs args2)
        //        //{
        //        //    if (args2.Element is XTextParagraphFlagElement)
        //        //    {
        //        //        args2.Element.StyleIndex = si2;
        //        //    }
        //        //    else
        //        //    {
        //        //        args2.Element.StyleIndex = si1;
        //        //    }
        //        //});
        //    }
        //    else if (clearFormatType == 2)
        //    {
        //        // 只清空字体名称和大小
        //        sourceDocument.ContentStyles.Default.FontName = args.Document.CurrentStyleInfo.Content.FontName;
        //        sourceDocument.ContentStyles.Default.FontSize = args.Document.CurrentStyleInfo.Content.FontSize;
        //        foreach (DocumentContentStyle style in sourceDocument.ContentStyles.Styles)
        //        {
        //            style.FontName = args.Document.CurrentStyleInfo.Content.FontName;
        //            style.FontSize = args.Document.CurrentStyleInfo.Content.FontSize;
        //        }
        //    }
        //    args.Document.ImportElements(list);
        //    args.Document.ApplyRemoveLastParagraphFlagWhenInsertDocument(list);
        //    if (list.Count > 0)
        //    {
        //        list.SetIsNewInputContent(true);
        //        args.DocumentControler.InsertElements(list);
        //    }
        //    return list;
        //}

        private static DomElementList InsertDocument(
            DomDocument mainDocument,
            DomDocument sourceDocument,
            int clearFormatType,
            bool checkImportDocument,
            bool showUI)
        {
            var list = CreateElementListForInsert(mainDocument, sourceDocument, clearFormatType, checkImportDocument, showUI);
            if (list != null)
            {
                ((DocumentControler)mainDocument.DocumentControler).InsertElements(list);
            }
            return list;
        }

        /// <summary>
        /// 由于CheckImportDocumen失败而取消操作的标记
        /// </summary>
        [ThreadStatic]
        internal static bool _CancelForCheckImportDocumen = false;


        public static DomElementList CreateElementListForInsert(
            DomDocument mainDocument,
            DomDocument sourceDocument,
            int clearFormatType,
            bool checkImportDocument,
            bool showUI)
        {
            _CancelForCheckImportDocumen = false;
            if (sourceDocument == null)
            {
                return null;
            }
            if (sourceDocument.Body == null)
            {
                return null;
            }
            if (sourceDocument.Body.Elements.Count == 0)
            {
                return null;
            }
            if (sourceDocument.Body.Elements.Count == 1
                && sourceDocument.Body.Elements[0] is DomParagraphFlagElement)
            {
                return null;
            }
            sourceDocument.FixDomState();
            DomElementList list = sourceDocument.Body.Elements.Clone();
            if (clearFormatType == 1)
            {
                // 清空格式
                sourceDocument.ContentStyles.Styles.Clear();
                sourceDocument.ContentStyles.Default = mainDocument.ContentStyles.Default;
                int si1 = -1;
                int si2 = -1;
                si1 = mainDocument.ContentStyles.GetStyleIndex(
                       mainDocument.CurrentStyleInfo.Content);
                si2 = mainDocument.ContentStyles.GetStyleIndex(
                    mainDocument.CurrentStyleInfo.Paragraph);
                DomTreeNodeEnumerable enumer = new DomTreeNodeEnumerable(list);
                enumer.ExcludeParagraphFlag = false;
                enumer.ExcludeCharElement = false;
                foreach (DomElement element in enumer)
                {
                    if (element is DomParagraphFlagElement)
                    {
                        element.StyleIndex = si2;
                    }
                    else
                    {
                        element.StyleIndex = si1;
                    }
                }
                //WriterUtils.Enumerate(list, delegate(
                //        object sender,
                //        ElementEnumerateEventArgs args2)
                //{
                //    if (args2.Element is XTextParagraphFlagElement)
                //    {
                //        args2.Element.StyleIndex = si2;
                //    }
                //    else
                //    {
                //        args2.Element.StyleIndex = si1;
                //    }
                //});
            }
            else if (clearFormatType == 2)
            {
                // 只清空字体名称和大小
                sourceDocument.ContentStyles.Default.FontName = mainDocument.CurrentStyleInfo.Content.FontName;
                sourceDocument.ContentStyles.Default.FontSize = mainDocument.CurrentStyleInfo.Content.FontSize;
                foreach (DocumentContentStyle style in sourceDocument.ContentStyles.Styles)
                {
                    style.FontName = mainDocument.CurrentStyleInfo.Content.FontName;
                    style.FontSize = mainDocument.CurrentStyleInfo.Content.FontSize;
                }
            }
            mainDocument.ImportElements(list);
            mainDocument.ApplyRemoveLastParagraphFlagWhenInsertDocument(list);
            if (list.Count > 0)
            {
                return list;
            }
            return null;
        }

        /// <summary>
        /// 插入纯文本
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        [WriterCommandDescription(
            StandardCommandNames.InsertString)]
        // ImageResource = "DCSoft.Writer.Commands.Images.CommandInsertString.bmp")]
        private void InsertString(object sender, WriterCommandEventArgs args)
        {
            if (args.Mode == WriterCommandEventMode.QueryState)
            {
                args.Enabled = args.DocumentControler != null
                    && args.DocumentControler.CanInsertElementAtCurrentPosition(
                    typeof(DomCharElement));
            }
            else if (args.Mode == WriterCommandEventMode.Invoke)
            {
                args.Result = 0;
                InsertStringCommandParameter parameter = null;
                if (args.Parameter is InsertStringCommandParameter)
                {
                    parameter = (InsertStringCommandParameter)args.Parameter;
                }
                else
                {
                    parameter = new InsertStringCommandParameter();
                    if (args.Parameter != null)
                    {
                        parameter.Text = Convert.ToString(args.Parameter);
                    }
                }
                if (args.ShowUI)
                {
                    var strNewString = args.EditorControl.WASMParent.JSShowPromptDialog(
                        "请输入文本", parameter.Text);
                    if (strNewString == null || strNewString.Length == 0)
                    {
                        return;
                    }
                    else
                    {
                        parameter.Text = strNewString;
                    }
                }
                if (string.IsNullOrEmpty(parameter.Text) == false)
                {
                    args.Result = args.DocumentControler.InsertString(
                        parameter.Text,
                        true,
                        InputValueSource.UI,
                        parameter.Style,
                        parameter.ParagraphStyle);
                }
            }
        }


        /// <summary>
        /// 插入软回车
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        [WriterCommandDescription(StandardCommandNames.InsertLineBreak,
            ShortcutKey = Keys.Shift | Keys.Enter)]
        private void InsertLineBreak(object sender, WriterCommandEventArgs args)
        {
            if (args.Mode == WriterCommandEventMode.QueryState)
            {
                args.Enabled = (args.DocumentControler != null
                    && args.DocumentControler.CanInsertElementAtCurrentPosition(typeof(DomLineBreakElement)));
            }
            else if (args.Mode == WriterCommandEventMode.Invoke)
            {
                args.Document._PreserveCurrentStyleInfoOnce = true;
                try
                {
                    args.Result = args.DocumentControler.InsertLineBreak();
                }
                finally
                {
                    args.Document._PreserveCurrentStyleInfoOnce = false;
                }
                args.RefreshLevel = UIStateRefreshLevel.All;

            }
        }

        /// <summary>
        /// 插入段落符号
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        [WriterCommandDescription(StandardCommandNames.InsertParagrahFlag,
            ReturnValueType = typeof(bool))]
        private void InsertParagrahFlag(object sender, WriterCommandEventArgs args)
        {
            if (args.Mode == WriterCommandEventMode.QueryState)
            {
                args.Enabled = (args.DocumentControler != null
                    && args.DocumentControler.CanInsertElementAtCurrentPosition(typeof(DomParagraphFlagElement)));
            }
            else if (args.Mode == WriterCommandEventMode.Invoke)
            {
                args.Result = false;
                DomElementList list = args.DocumentControler.InsertString(
                    "\r",
                    true,
                    InputValueSource.UI,
                    null,
                    null);
                if (list != null && list.Count > 0)
                {
                    args.Result = true;
                    args.RefreshLevel = UIStateRefreshLevel.All;
                }
            }
        }


    }
}
