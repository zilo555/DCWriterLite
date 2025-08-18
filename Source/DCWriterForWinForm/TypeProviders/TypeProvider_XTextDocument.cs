using System.Text;
using DCSoft.Writer.Dom;
using DCSoft.Common;
using DCSoft.Writer.Data;
using DCSoft.WinForms;
using DCSoft.Writer;

namespace DCSoft.TypeProviders
{
    internal static class TypeProvider_XTextDocument
    {
        /// <summary>
        /// 设置文档元素文本值
        /// </summary>
        /// <param name="document"></param>
        /// <param name="element">文档元素对象</param>
        /// <param name="text">文本值</param>
        /// <returns>操作是否成功</returns>
        public static bool SetElementText(DomDocument document, DomElement element, string text)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }
            if (element is DomContainerElement)
            {
                DomContainerElement c = (DomContainerElement)element;
                return c.SetEditorTextExt(text, DomAccessFlags.None, true);
            }
            if (element is DomCheckBoxElementBase)
            {
                DomCheckBoxElementBase chk = (DomCheckBoxElementBase)element;
                if (chk.Caption != text)
                {
                    chk.Caption = text;
                    chk.EditorRefreshView();
                }
                return true;
            }
            element.Text = text;
            return true;
        }

        /// <summary>
        /// 获得文档中所有的勾选的复选框元素的值
        /// </summary>
        /// <param name="document"></param>
        /// <param name="spliter">各个项目之间的分隔字符串</param>
        /// <param name="includeCheckbox">是否包含复选框</param>
        /// <param name="includeRadio">是否包含单选框</param>
        /// <param name="includeElementID">是否包含元素ID号</param>
        /// <param name="includeElementName">是否包含元素Name属性值</param>
        /// <returns>获得的字符串</returns>
        /// <remarks>
        /// 例如调用 document.GetCheckedValueList(";",true,true,true,true) 返回类似字符串
        /// “xbzw;胸部正位;gpzw;骨盆正位;fbww;腹部卧位”
        /// </remarks>
        public static string GetCheckedValueList(
            DomDocument document,
            string spliter,
            bool includeCheckbox,
            bool includeRadio,
            bool includeElementID,
            bool includeElementName)
        {
            if (includeCheckbox == false && includeRadio == false)
            {
                return string.Empty;
            }
            if (includeElementID == false && includeElementName == false)
            {
                return string.Empty;
            }
            DomElementList elements = document.GetElementsByType<DomCheckBoxElementBase>();
            StringBuilder str = new StringBuilder();
            foreach (DomCheckBoxElementBase chk in elements)
            {
                if (chk.Checked == false)
                {
                    continue;
                }
                if (includeCheckbox == false && chk is DomCheckBoxElement)
                {
                    continue;
                }
                if (includeRadio == false && chk is DomRadioBoxElement)
                {
                    continue;
                }
                if (str.Length > 0 && spliter != null)
                {
                    str.Append(spliter);
                }
                if (includeElementID)
                {
                    str.Append(chk.ID);
                }
                if (spliter != null)
                {
                    str.Append(spliter);
                }
                if (includeElementName)
                {
                    str.Append(chk.Name);
                }
            }//foreach
            return str.ToString();
        }


        public static void CheckForClearTextFormat(DomDocument document, DomElementList sourceElements)
        {
            if (document.GetDocumentBehaviorOptions().AutoClearTextFormatWhenPasteOrInsertContent)
            {
                int txtStyleIndex = document.ContentStyles.GetStyleIndex(document.CurrentStyleInfo.Content);
                int pStyleIndex = document.ContentStyles.GetStyleIndex(document.CurrentStyleInfo.Paragraph);
                int cellStyleIndex = document.ContentStyles.GetStyleIndex(document.CurrentStyleInfo.Cell);
                DomTreeNodeEnumerable enums = new DomTreeNodeEnumerable(sourceElements);
                enums.ExcludeCharElement = false;
                enums.ExcludeParagraphFlag = false;

                foreach (DomElement element in enums)
                {
                    if (element is DomParagraphFlagElement)
                    {
                        element.StyleIndex = pStyleIndex;
                    }
                    else if (element is DomTableCellElement)
                    {
                        element.StyleIndex = cellStyleIndex;
                    }
                    else
                    {
                        if (element is DomCharElement)
                        {
                            if(element.Style.Subscript)
                            {
                                element.StyleIndex = txtStyleIndex;
                                element.Style.Subscript = true;
                            }
                            else if (element.Style.Superscript)
                            {
                                element.StyleIndex = txtStyleIndex;
                                element.Style.Superscript = true;
                            }
                            else
                            {
                                element.StyleIndex = txtStyleIndex;
                            }
                        }
                        else
                        {
                            element.StyleIndex = txtStyleIndex;
                        }

                            
                    }
                }//foreach
            }
        }
    }
}
