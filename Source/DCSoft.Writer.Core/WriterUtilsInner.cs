using System;
using System.Collections.Generic;
using System.Text;
using DCSoft.Writer.Dom;
// // 
using DCSoft.Drawing;
using DCSoft.Common;
//using System.Data;
using DCSoft.Writer.Controls;
using System.Collections;
using DCSoft.Printing;

namespace DCSoft.Writer
{
    /// <summary>
    /// DCWriter内部使用。编辑器帮助类，定义了一些常用例程
    /// </summary>
    /// <remarks>编制 袁永福</remarks>
    public static class WriterUtilsInner
    {

        public static bool ArrayContains<T>(this T[] array, T item)
        {
            if (array == null || array.Length == 0)
            {
                return false;
            }
            for (var iCount = array.Length - 1; iCount >= 0; iCount--)
            {
                if (object.Equals(array[iCount], item))
                {
                    return true;
                }
            }
            return false;
        }

        private static DateTime _TodayDate = DateTime.Today;
        public static long TickCount
        {
            get
            {
                return (long)(DateTime.Now - _TodayDate).TotalMilliseconds;
            }
        }

        internal static IntPtr CurrentErrorDialogWin32Handle = IntPtr.Zero;

        static WriterUtilsInner()
        {
            _WhiteSpaceStrings = new string[30];
            for (int iCount = 0; iCount < _WhiteSpaceStrings.Length; iCount++)
            {
                _WhiteSpaceStrings[iCount] = new string(' ', iCount);
            }
        }

        public static List<int> GetSelectionPageIndexs(DomDocument document)
        {
            if (document == null)
            {
                return null;
            }
            DCSelection selection = document.Body.Selection;
            float contentTop = float.MaxValue;
            float contentBottom = float.MinValue;
            foreach (DomElement element in selection.ContentElements)
            {
                float pos = element.GetAbsTop();
                if (pos > 0)
                {
                    contentTop = Math.Min(contentTop, pos);
                    contentBottom = Math.Max(contentBottom, pos + element.Height);
                }
            }
            if (selection.Cells != null)
            {
                foreach (DomTableCellElement cell in selection.Cells)
                {
                    float pos = cell.GetAbsTop();
                    if (pos > 0)
                    {
                        contentTop = Math.Min(contentTop, pos);
                        contentBottom = Math.Max(contentBottom, pos + cell.Height);
                    }
                }
            }
            if (contentTop != float.MaxValue)
            {
                var pis = new List<int>();
                for (int iCount = 0; iCount < document.Pages.Count; iCount++)
                {
                    PrintPage page = document.Pages[iCount];
                    if (page.Top < contentBottom - 2 && page.Bottom > contentTop + 2)
                    {
                        pis.Add(page.PageIndex);
                    }
                }
                return pis;
            }
            return null;
        }

        public static string GetElementName( DomElement element )
        {
            if(element is DomObjectElement)
            {
                return ((DomObjectElement)element).Name;
            }
            if(element is DomInputFieldElementBase)
            {
                return ((DomInputFieldElementBase)element).Name;
            }
            return null;
        }
        /// <summary>
        /// 检测文件格式
        /// </summary>
        /// <param name="bsData">数据内容</param>
        /// <returns>文件格式名称</returns>
        public static string DetectFileFormat( byte[] bsData )
        {
            if (bsData == null || bsData.Length == 0 )
            {
                return null;
            }
            var strHeader = System.Text.Encoding.UTF8.GetString(bsData, 0, Math.Max(bsData.Length, 1000));
            return DetectFileFormat(strHeader);
        }

        /// <summary>
        /// 检测文件格式
        /// </summary>
        /// <param name="strData">文本数据</param>
        /// <returns>文件格式名称</returns>
        public static string DetectFileFormat(string strData)
        {
            if (string.IsNullOrEmpty(strData))
            {
                return null;
            }
            int len = Math.Min(strData.Length, 300);
            for (int iCount = 0; iCount < len; iCount++)
            {
                var firstChar = strData[iCount];
                if (char.IsWhiteSpace(firstChar) == false && firstChar > 10 && firstChar < 127)
                {
                    if (firstChar == '{')
                    {
                        int index = strData.IndexOf("Type", iCount, 200, StringComparison.Ordinal);
                        if (index >= 0)
                        {
                            int index2 = strData.IndexOf("\"DCDocument2022\"", index + 1, 200, StringComparison.Ordinal);
                            if (index2 > 0)
                            {
                                return "json";
                            }
                        }
                    }
                    else if (firstChar == '<')
                    {
                        int index = strData.IndexOf("<XTextDocument", iCount, 200, StringComparison.Ordinal);
                        if (index >= 0)
                        {
                            return "xml";
                        }
                        index = strData.IndexOf("<DCDocument2022", iCount, 200, StringComparison.Ordinal);
                        if (index >= 0)
                        {
                            return "xml2022";
                        }
                        index = strData.IndexOf("<html", iCount, 200, StringComparison.OrdinalIgnoreCase);
                        if (index >= 0)
                        {
                            return "html";
                        }
                    }
                    break;
                }
            }
            return null;
        }

        private static readonly string[] _WhiteSpaceStrings = null;
        /// <summary>
        /// 获得纯空格字符串
        /// </summary>
        /// <param name="len">长度</param>
        /// <returns>获得的字符串</returns>
        public static string GetWhitespaceString(int len)
        {
            if (len < 0)
            {
                throw new ArgumentOutOfRangeException("len+" + len);
            }
            if (len == 0)
            {
                return string.Empty;
            }
            else if (len < 30)
            {
                return _WhiteSpaceStrings[len];
            }
            else
            {
                return new string(' ', len);
            }
        }

        internal static bool StringToBoolean(string str, bool defaultValue)
        {
            if (string.IsNullOrEmpty(str))
            {
                return defaultValue;
            }
            if (str.Equals("true", StringComparison.OrdinalIgnoreCase)
                || str.Equals("yes", StringComparison.OrdinalIgnoreCase)
                || str == "1")
            {
                return true;
            }
            else if (str.Equals("false", StringComparison.OrdinalIgnoreCase)
                || str.Equals("no", StringComparison.OrdinalIgnoreCase)
                || str.Equals("null", StringComparison.OrdinalIgnoreCase)
                || str.Equals("undefined", StringComparison.OrdinalIgnoreCase)
                || str == "0")
            {
                return false;
            }
            else
            {
                double v9 = 0;
                if (double.TryParse(str, out v9))
                {
                    return v9 != 0;
                }
                else
                {
                    return defaultValue;
                    //try
                    //{
                    //    return Convert.ToBoolean(str);
                    //}
                    //catch (System.Exception ext)
                    //{
                    //    return defaultValue;
                    //}
                }
            }
        }
        /// <summary>
        /// 根据插入点所在的容器来修正表格的总宽度
        /// </summary>
        /// <param name="document">文档对象</param>
        /// <param name="table">表格元素</param>
        public static void CheckTableWidthWhenInsertTable(
            DomDocument document,
            DomTableElement table)
        {
            if (document.GetDocumentEditOptions().FixWidthWhenInsertTable)
            {
                DomContainerElement container = null;
                int elementIndex = 0;
                document.Content.GetCurrentPositionInfo(out container, out elementIndex);
                container = container.ContentElement;

                float newWidth = Math.Min(container.ClientWidth, table.TableWidth);
                foreach (DomTableColumnElement col in table.Columns)
                {
                    if (col.ZeroWidth())
                    {
                        // 表格中出现零宽度的表格列，则设置为容器的客户区宽度
                        newWidth = container.ClientWidth;
                        break;
                    }
                }
                if (newWidth != table.TableWidth)
                {
                    table.SetTableWidth(newWidth);
                }
            }
            else
            {
                var mwidth = document.GetDocumentViewOptions().MinTableColumnWidth;
                foreach (DomTableColumnElement col in table.Columns)
                {
                    if (col.ZeroWidth())
                    {
                        col.Width = mwidth;
                    }
                }
            }
        }


        /// <summary>
        /// 根据插入点所在的容器来修正图片元素的大小
        /// </summary>
        /// <param name="document">文档对象</param>
        /// <param name="element">图片元素</param>
        /// <param name="keepWidthHeightRate">保持长宽比</param>
        /// <param name="container">容器元素对象</param>
        public static void CheckImageSizeWhenInsertImage(
            DomDocument document,
            DomElement element,
            bool keepWidthHeightRate,
            DomContainerElement container)
        {
            if (document.GetDocumentEditOptions().FixSizeWhenInsertImage)
            {
                if (container == null)
                {
                    int elementIndex = 0;
                    document.Content.GetCurrentPositionInfo(out container, out elementIndex);
                }
                container = container.ContentElement;
                SizeF size = new SizeF(element.Width, element.Height);
                float ph = 100000;
                if (container.DocumentContentElement.ContentPartyStyle == PageContentPartyStyle.Body)
                {
                    if (document.CurrentPage != null)
                    {
                        // 标准页面高度
                        ph = document.CurrentPage.StandartPapeBodyHeight - 10;
                    }
                }
                size = MathCommon.FixSize(
                    new SizeF(container.ClientWidth - document.PixelToDocumentUnit(2), ph),
                    size,
                    keepWidthHeightRate);

                element.Width = size.Width;
                element.Height = size.Height;
            }
        }

        /// <summary>
        /// 比较两个元素在文档DOM树状结构中的位置。
        /// </summary>
        /// <param name="element1"></param>
        /// <param name="element2"></param>
        /// <returns></returns>
        internal static int CompareDOMLevel(DomElement element1, DomElement element2)
        {
            if (element1 == element2)
            {
                return 0;
            }
            if (element1 == null || element2 == null)
            {
                return 0;
            }
            if (element1.Parent == element2)
            {
                // 第一个元素是第二个元素的子元素
                return -1;
            }
            if (element1 == element2.Parent)
            {
                // 第一个元素是第二个元素的父元素。
                return 1;
            }
            // 获得第一个元素的所有父节点列表，近亲在前，远亲在后。
            List<DomElement> ps = new List<DomElement>();
            DomElement p = element1;
            while (p != null)
            {
                ps.Add(p);
                p = p.Parent;
            }
            p = element2.Parent;
            DomElement lastElement = element2;
            while (p != null)
            {
                int index = ps.IndexOf(p);
                if (index == 0)
                {
                    // 第一个节点是第二个节点的上层节点
                    return 1;
                }
                else if (index >= 0)
                {
                    // 遇到共同的最小的父节点
                    int elementIndex1 = ps[index - 1].ElementIndex;
                    int elementIndex2 = lastElement.ElementIndex;
                    return elementIndex1 - elementIndex2;
                }
                lastElement = p;
                p = p.Parent;
            }
            // 这是一个不应该到达的地方，说明DOM结构出现了问题，就返回0.
            return 0;
            //List<int> indexs1 = new List<int>();
            //XTextElement p = element1;

            //XTextElementList ps1 = GetParentList(element1);
            //ps1.Reverse();
            //ps1.AddRaw(element1);
            //XTextElementList ps2 = GetParentList(element2);
            //ps2.Reverse();
            //ps2.AddRaw(element2);
            //int len = Math.Min(ps1.Count, ps2.Count);
            //for (int iCount = 0; iCount < len ; iCount++)
            //{
            //    XTextElement p1 = ps1[iCount];
            //    int e1 = 0;
            //    if (p1.Parent != null)
            //    {
            //        e1 = p1.Parent.Elements.IndexOf(p1);
            //    }
            //    XTextElement p2 = ps2[iCount];
            //    int e2 = 0;
            //    if (p2.Parent != null)
            //    {
            //        e2 = p2.Parent.Elements.IndexOf(p2);
            //    }
            //    if (e1 < e2)
            //    {
            //        return -1;
            //    }
            //    else if (e1 > e2)
            //    {
            //        return 1;
            //    }
            // }
            //return ps1.Count  - ps2.Count ;
        }


        internal static int SmartIndexOf(
            DomElementList elements,
            DomElement element)
        {
            if (elements == null)
            {
                throw new ArgumentNullException("elements");
            }
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }
            if (elements.SafeGet(element._ContentIndex) == element)
            {
                return element._ContentIndex;
            }
            if (elements.SafeGet(element._PrivateContentIndex) == element)
            {
                return element._PrivateContentIndex;
            }
            return elements.IndexOf(element);

            //if( element.ContentIndex >= 0 && element.ContentIndex < elements.Count && elements[ element.ContentIndex ] == 
            //if ((element is XTextFieldBorderElement) == false )
            //{
            //    if (element.Parent != null && element.Parent.Elements == elements)
            //    {
            //        return element.ElementIndex;
            //    }
            //}
            //XTextDocumentContentElement dce = element.DocumentContentElement;
            //if (dce != null && dce.Content == elements)
            //{
            //    int index = element.ContentIndex;
            //    if (index >= 0)
            //    {
            //        return index;
            //    }
            //}
            //return elements.IndexOf(element);
        }

        /// <summary>
        /// 判断是否存在分页符号
        /// </summary>
        /// <param name="list">文档元素列表</param>
        /// <returns>是否包含分页符</returns>
        public static bool HasPageBreakElement(DomElementList list, int startIndex, int endIndex)
        {
            if (list == null || list.Count == 0)
            {
                return false;
            }
            if (startIndex > endIndex)
            {
                throw new ArgumentOutOfRangeException("startIndex>endIndex");
            }
            if (startIndex < 0)
            {
                startIndex = 0;
            }
            if (startIndex >= list.Count)
            {
                startIndex = list.Count - 1;
            }
            if (endIndex < 0)
            {
                endIndex = 0;
            }
            if (endIndex >= list.Count)
            {
                endIndex = list.Count - 1;
            }
            for (int iCount = startIndex; iCount <= endIndex; iCount++)
            {
                DomElement element = list[iCount];
                if (element is DomPageBreakElement)
                {
                    return true;
                }
                if (element is DomContainerElement)
                {
                    bool result = HasPageBreakElement(
                        element.Elements,
                        0,
                        element.Elements.Count - 1);
                    if (result)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// 删除输入域结构，但保存文档内容
        /// </summary>
        /// <param name="elements">要操作的元素列表</param>
        public static void RemoveInputFieldStructure(DomElementList elements)
        {
            if (elements == null || elements.Count == 0)
            {
                return;
            }
            for (int iCount = 0; iCount < elements.Count; iCount++)
            {
                DomElement element = elements[iCount];
                if (element is DomInputFieldElement)
                {
                    DomInputFieldElement field = (DomInputFieldElement)element;
                    elements.RemoveAt(iCount);
                    elements.InsertRange(iCount, field.Elements);
                }
                else if (element is DomContainerElement)
                {
                    RemoveInputFieldStructure(element.Elements);
                }
            }
        }

        /// <summary>
        /// 连接字符串
        /// </summary>
        /// <param name="items">若干个字符串项目集合</param>
        /// <param name="splitChar">分割字符</param>
        /// <returns>连接所得的大字符串</returns>
        public static string ConcatString(IEnumerable items, char splitChar)
        {
            if (items == null)
            {
                return null;
            }
            StringBuilder str = new StringBuilder();
            foreach (object item in items)
            {
                if (item != null)
                {
                    if (str.Length > 0)
                    {
                        str.Append(splitChar);
                    }
                    str.Append(Convert.ToString(item));
                }
            }
            return str.ToString();
        }

        private static Dictionary<Type, string> _TypeDisplayName = null;
        private static void CheckTypeDisplayNames()
        {
            if (_TypeDisplayName == null)
            {
                //lock (typeof(WriterUtilsInner))
                {
                    if (_TypeDisplayName == null)
                    {
                        var result = new Dictionary<Type, string>();
                        //_TypeDisplayName[typeof(XTextBarcodeFieldElement)] = DCSR.ElementType_Barcode;
                        result[typeof(DomDocumentBodyElement)] = DCSR.ElementType_Body;
                        result[typeof(DomCharElement)] = DCSR.ElementType_Char;
                        result[typeof(DomCheckBoxElement)] = DCSR.ElementType_CheckBox;
                        result[typeof(DomRadioBoxElement)] = DCSR.ElementType_RadioBox;
                        //result[typeof(XTextContentLinkFieldElement)] = DCSR.ElementType_ContentLink;
                        //result[typeof(XTextControlHostElement)] = DCSR.ElementType_ControlHost;
                        result[typeof(DomDocument)] = DCSR.ElementType_Document;
                        result[typeof(DomDocumentFooterElement)] = DCSR.ElementType_Footer;
                        result[typeof(DomDocumentHeaderElement)] = DCSR.ElementType_Header;
                        result[typeof(DomImageElement)] = DCSR.ElementType_Image;
                        result[typeof(DomInputFieldElement)] = DCSR.ElementType_InputField;
                        result[typeof(DomLineBreakElement)] = DCSR.ElementType_LineBreak;
                        result[typeof(DomPageBreakElement)] = DCSR.ElementType_PageBreak;
                        result[typeof(DomPageInfoElement)] = DCSR.ElementType_PageInfo;
                        result[typeof(DomParagraphFlagElement)] = DCSR.ElementType_ParagraphFlag;
                        result[typeof(DomTableElement)] = DCSR.ElementType_Table;
                        result[typeof(DomTableRowElement)] = DCSR.ElementType_TableRow;
                        result[typeof(DomTableCellElement)] = DCSR.ElementType_TableCell;
                        result[typeof(DomTableColumnElement)] = DCSR.ElementType_TableColumn;
                        _TypeDisplayName = result;
                    }
                }
            }
        }

        internal static void SetTypeDisplayName(Type t, string name)
        {
            CheckTypeDisplayNames();
            _TypeDisplayName[t] = name;
        }

        public static string GetTypeDisplayName(Type t)
        {
            CheckTypeDisplayNames();
            if (_TypeDisplayName.ContainsKey(t))
            {
                return _TypeDisplayName[t];
            }
            else
            {
                return t.Name;
            }
        }
        

        /// <summary>
        /// 重新计算文档元素的大小
        /// </summary>
        /// <param name="document">文档对象</param>
        /// <param name="elements">文档元素列表</param>
        /// <param name="checkSizeInvalidate">是否检测SizeInvalidate标记</param>
        /// <returns>元素大小是否发生改变了。</returns>
        public static bool RefreshElementsSize(
            DomDocument document,
            IEnumerable<DomElement> elements,
            bool checkSizeInvalidate)
        {
            bool result = false;
            using (var g = document.InnerCreateDCGraphics())
            {
                var args = document.CreateInnerPaintEventArgs(g);
                foreach (DomElement element in elements)
                {
                    if (checkSizeInvalidate == false || element.SizeInvalid)
                    {
                        args.Element = element;
                        element.OwnerDocument = document;
                        float oldWidth = element.Width;
                        element.RefreshSize(args);
                        if (oldWidth != element.Width)
                        {
                            result = true;
                        }
                    }
                }//foreach
            }//using
            return result;
        }



        /// <summary>
        /// 根据内容创建一个新的文档对象,而且不包含已经被逻辑删除的内容.
        /// </summary>
        /// <returns>创建的文档对象</returns>
        public static DomDocument CreateDocument(DomDocument sourceDocument, DomElementList elements, bool cloneElements)
        {
            if (elements == null)
            {
                return null;
            }
            DomDocument document = null;
            if (sourceDocument == null)
            {
                document = new DomDocument();
            }
            else
            {
                document = (DomDocument)sourceDocument.Clone(false);
            }
            document.ContentStyles.Styles.Clear();
            document.ContentStyles.Default = sourceDocument.ContentStyles.Default.Clone();
            if (document.UndoList != null)
            {
                document.UndoList.Clear();
            }
            if (elements.Count == 0)
            {
                return document;
            }
            // 寻找所有内容共同的文档容器元素
            foreach (DomElement element in elements)
            {
                if (element is DomContainerElement)
                {
                    ((DomContainerElement)element).FixDomState();
                }
            }
            DomElementList newElements = elements;
            if (cloneElements)
            {
                newElements = newElements.CloneDeeply();
            }
            if (newElements.Count > 0)
            {
                newElements[0].OwnerDocument = sourceDocument;
            }
            document.ImportElements(newElements);
            document.Body.Elements.Clear();
            document.Body.Elements.AddRangeByDCList(newElements);
            document.Body.FixElements();
            // 删除没有引用的样式
            document.CompressStyleList();
            document.EditorControl = null;
            document.DocumentControler = null;
            document.CurrentStyleInfo = null;
            document.HoverElement = null;
            document.HighlightManager = null;

            if (document.UndoList != null)
            {
                document.EndLogUndo();
                document.UndoList.Clear();
            }
            document.FixDomState();
            return document;
        }

        /// <summary>
        /// 获得限制长度的字符串
        /// </summary>
        /// <param name="txt">字符串</param>
        /// <param name="length">最大长度</param>
        /// <returns>字符串</returns>
        public static string GetLimitedString(string txt, int length)
        {
            if (string.IsNullOrEmpty(txt))
            {
                return string.Empty;
            }
            if (txt.Length > length)
            {
                return txt.Substring(0, length);
            }
            else
            {
                return txt;
            }
        }

        public static bool HasFormatFlag(WriterDataFormats format, WriterDataFormats mask)
        {
            return (format & mask) == mask;
        }

        /// <summary>
        /// 判断是否存在指定的字位标记
        /// </summary>
        /// <param name="Value">数值</param>
        /// <param name="Flag">掩码</param>
        /// <returns>是否存在标记</returns>
        public static bool HasFlag(int Value, int Flag)
        {
            return (Value & Flag) == Flag;
        }

        /// <summary>
        /// 试图将参数值转换为布尔值
        /// </summary>
        /// <param name="v">参数值</param>
        /// <param name="result">布尔值</param>
        /// <returns>操作是否成功</returns>
        public static bool TryGetArgumentBooleanValue(object v, ref bool result)
        {
            if (v == null || DBNull.Value.Equals(v))
            {
                return false;
            }
            if (v is bool)
            {
                result = (bool)v;
                return true;
            }
            else
            {
                string txt = Convert.ToString(v);
                txt = txt.Trim();
                if (txt.Equals("true", StringComparison.CurrentCultureIgnoreCase)
                    || txt == "1")
                {
                    result = true;
                    return true;
                }
                else if (txt.Equals("false", StringComparison.CurrentCultureIgnoreCase)
                    || txt == "0")
                {
                    result = false;
                    return true;
                }
            }
            return false;
        }


        /// <summary>
        /// 将参数值转换为整数值
        /// </summary>
        /// <param name="v">参数值</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>转换后的整数值</returns>
        public static int GetArgumentIntValue(object v, int defaultValue)
        {
            if (v == null || DBNull.Value.Equals(v))
            {
                return defaultValue;
            }
            int position = defaultValue;
            if (v is string)
            {
                if (int.TryParse((string)v, out position) == false)
                {
                    return defaultValue;
                }
            }
            else if (v is int || v is float || v is double
                || v is long || v is short || v is byte
                || v is uint)
            {
                position = Convert.ToInt32(v);
            }
            return position;
        }

        /// <summary>
        /// 将参数值转换为布尔值
        /// </summary>
        /// <param name="v">参数值</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>转换后的布尔值</returns>
        public static bool GetArgumentBooleanValue(object v, bool defaultValue)
        {
            if (v == null || DBNull.Value.Equals(v))
            {
                return defaultValue;
            }
            if (v is bool)
            {
                return (bool)v;
            }
            else
            {
                string txt = Convert.ToString(v);
                txt = txt.Trim();
                if (txt.Equals("true", StringComparison.CurrentCultureIgnoreCase)
                    || txt == "1")
                {
                    return true;
                }
                else if (txt.Equals("false", StringComparison.CurrentCultureIgnoreCase)
                    || txt == "0")
                {
                    return false;
                }
            }
            return defaultValue;
        }
        /// <summary>
        /// 格式化显示字节大小
        /// </summary>
        /// <param name="size">字节大写值</param>
        /// <returns>获得的文本</returns>
        public static string FormatByteSize(int size)
        {
            return DCSoft.Common.FileHelper.FormatByteSize(size);
        }

        /// <summary>
        /// 遍历子孙元素
        /// </summary>
        /// <param name="elements">要遍历的元素列表</param>
        /// <param name="handler">遍历过程的委托对象</param>
        public static void Enumerate(
            DomElementList elements,
            ElementEnumerateEventHandler handler)
        {
            if (handler == null)
            {
                throw new ArgumentNullException("handler");
            }
            ElementEnumerateEventArgs args = new ElementEnumerateEventArgs();
            //float v = CountDown.GetTickCountFloat();
            InnerEnumerate(elements, handler, args, true);
            //v = CountDown.GetTickCountFloat() - v;
            //int hc = args.HandlerCount;
        }

        private static void InnerEnumerate(
           DomElementList elements,
           ElementEnumerateEventHandler handler,
           ElementEnumerateEventArgs args,
            bool deeply)
        {
            if (args.ReverseMode)
            {
            }
            else
            {
                foreach (DomElement element in elements.FastForEach())
                {
                    args._Element = element;
                    args.CancelChild = false;
                    handler(null, args);
                    args.IncreaseHandlerCount();
                    if (args.Cancel)
                    {
                        break;
                    }
                    if (args.CancelChild == false && deeply)
                    {
                        if (element is DomContainerElement)
                        {
                            InnerEnumerate(((DomContainerElement)element).Elements, handler, args, deeply);
                            if (args.Cancel)
                            {
                                break;
                            }
                        }
                    }
                    args.CancelChild = false;
                }
            }
        }

        /// <summary>
        /// 计算指定位置处的制表符的宽度
        /// </summary>
        /// <remarks>
        /// 本函数根据作为参数传入的制表符开始的位置,计算制表符的宽度,以保证制表符的右端位置在某个制表位上
        /// 制表位的位置恒定为标准制表符的宽度的整数倍(在此处标准制表符为4个下划线的宽度)
        ///    在下面的标尺上
        ///   0___1___2___3___4___5___6___7___8___9___10
        ///    制表符的位置随意,但制表符右端必须在某个数字下面
        ///    若制表符恰好在制表位上则返回标准制表符宽度
        /// 由于有这样的限制,因此制表符的宽度不固定,而根据其位置而改变,本函数就是计算其宽度
        /// </remarks>
        /// <param name="Pos">制表符开始的位置</param>
        /// <param name="TabStep">制表宽度</param>
        /// <returns>制表符的宽度</returns>
        internal static float GetTabWidth(float Pos, float TabStep)
        {
            float iWidth = TabStep * (int)System.Math.Ceiling((double)Pos / TabStep) - Pos;
            if (iWidth == 0)
                iWidth = TabStep;
            return iWidth;
        }

        public static Font DefaultFont = new Font(XFontValue.DefaultFontName, (float)12);
        
        /// <summary>
        /// 获得指定元素的父节点对象列表，在该列表中，近亲在前，远亲在后。
        /// </summary>
        /// <remarks>
        /// 本方法和GetParentList的不同就是当元素是父元素的第一个文档内容元素则跳过这个父元素
        /// </remarks>
        /// <param name="element">文档元素对象</param>
        /// <returns>父节点对象列表</returns>
        internal static DomElementList GetParentList2(DomElement element)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }
            DomElementList result = new DomElementList();
            DomContainerElement parent = element.Parent;
            while (parent != null)
            {
                if (parent.FirstContentElement != element)
                {
                    result.Add(parent);
                }
                else if(parent is DomTableCellElement )
                {
                    result.Add(parent);
                }
                parent = parent.Parent;
            }
            return result;
        }

        /// <summary>
        /// 获得指定元素的父节点对象列表，在该列表中，近亲在前，远亲在后。
        /// </summary>
        /// <param name="element">文档元素对象</param>
        /// <returns>父节点对象列表</returns>
        internal static DomElementList GetParentList(DomElement element)
        {
            if (element == null)
            {
                throw new ArgumentNullException("GetParentList.element");
            }
            DomElementList result = new DomElementList();
            DomContainerElement parent = element.Parent;
            while (parent != null)
            {
                result.FastAdd2(parent);
                parent = parent.Parent;
            }
            return result;
        }

        /// <summary>
        /// 对字符元素进行合并操作.本代码被DCSoft.Writer.Html.WriterHtmlDocumentWriter.MergeElements()抄袭.
        /// </summary>
        /// <param name="sourceList">元素列表</param>
        /// <param name="includeSelectionOnly">只处理被选中的内容</param>
        /// <returns>合并后的元素列表</returns>
        public static DomElementList MergeElements(
            DomElementList sourceList,
            bool includeSelectionOnly,
            DomContainerElement container = null)
        {
            //bool containerVisible = container == null || container.RuntimeVisible;
            DomElementList result = new DomElementList();
            if (sourceList.Count == 0)
            {
                return result;
            }
            DomStringElement myString = null;
            //XTextParagraphList plist = null;
            //XTextDocumentContentElement ce = sourceList[0].DocumentContentElement;

            //if (writer != null)
            //{
            //    wopts = writer.InnerGetOptions();
            //}
            int elementCount = sourceList.Count;
            //XTextElement lastVisibleElement = null;
            List<DomStringElement> strings = new List<DomStringElement>();
            bool simpleList = true;
            for (var elementIndex = 0; elementIndex < elementCount; elementIndex++)
            {
                var element = sourceList[elementIndex];
                //if( element.RuntimeVisible )
                //{
                //    lastVisibleElement = element;
                //}
                if (includeSelectionOnly)
                {
                    if (element.HasSelection == false)
                    {
                        continue;
                    }
                }
                if (element is DomCharElement)
                {
                    DomCharElement c = (DomCharElement)element;
                    string txt = c.ToString();
                    bool isBackgroundText = false;
                    DomInputFieldElement parentField = null;
                    if (c.Parent is DomInputFieldElement)
                    {
                        isBackgroundText = ((DomInputFieldElement)c.Parent).FastIsBackgroundTextElement(c);
                    }
                    if (myString != null
                        && myString.CanMerge(c)
                        && myString.HasSameParent(c)// .Parent == c.Parent
                        && myString.IsBackgroundText == isBackgroundText)
                    {
                        myString.Merge(c, txt);
                    }
                    else
                    {
                        myString = new DomStringElement();
                        strings.Add(myString);
                        myString.InnerSetOwnerDocumentParentRaw2(c);
                        //myString.OwnerDocument = c.OwnerDocument;
                        //myString.SetParentRaw(c.Parent);
                        //if (c.RuntimeVisible == false )
                        //{
                        //    if( lastVisibleElement != null )
                        //    {
                        //        myString._PFlag = lastVisibleElement.OwnerParagraphEOF;
                        //    }
                        //    if (myString._PFlag == null)
                        //    {
                        //        myString._PFlag = c.OwnerParagraphEOF;
                        //    }
                        //}
                        //else
                        //{
                        //for (int iCount4 = elementIndex; iCount4 < elementCount; iCount4++)
                        //{
                        //    if (sourceList[iCount4] is XTextParagraphFlagElement)
                        //    {
                        //        myString._PFlag = (XTextParagraphFlagElement)sourceList[iCount4];
                        //        break;
                        //    }
                        //    else if( sourceList[ iCount4] is XTextContainerElement)
                        //    {
                        //        break;
                        //    }
                        //}
                        //if (myString._PFlag == null)
                        //{
                        //    myString._PFlag = c.OwnerParagraphEOF;
                        //}
                        //}
                        if (isBackgroundText == true)
                        {
                            if (parentField != null)
                            {
                                myString.StyleIndex = parentField.StyleIndex;
                            }
                        }
                        myString.IsBackgroundText = isBackgroundText;
                        result.Add(myString);
                        myString.Merge(c, txt);
                    }
                    continue;
                }
                else
                {
                    if (element is DomContainerElement)
                    {
                        simpleList = false;
                    }
                    myString = null;
                    result.Add(element);
                }
            }//foreach
            if (strings.Count > 0)
            {
                if (simpleList)
                {
                    int currentIndex = sourceList.Count - 1;
                    DomParagraphFlagElement lastFlag = null;
                    for (int iCount = strings.Count - 1; iCount >= 0; iCount--)
                    {
                        var str = strings[iCount];
                        //str._PFlag = str.EndElement.OwnerParagraphEOF;continue;
                        DomCharElement ee = str.EndElement;
                        while (currentIndex > 0)
                        {
                            DomElement ele = sourceList[currentIndex];
                            currentIndex--;
                            if (ele == ee)
                            {
                                break;
                            }
                            else if (ele is DomParagraphFlagElement)
                            {
                                lastFlag = (DomParagraphFlagElement)ele;
                                str._PFlag = lastFlag;
                                break;
                            }
                        }
                        if (str._PFlag == null)
                        {
                            str._PFlag = lastFlag;
                            if (str._PFlag == null)
                            {
                                currentIndex = -1;
                                //str._PFlag = ee.OwnerParagraphEOF;
                            }
                        }
                    }
                }

                foreach (var str in strings)
                {
                    if (str._PFlag == null)
                    {
                        var ce = strings[0].StartElement.ContentElement;
                        var map = ce.GetCharOwnerPFlags();
                        if (map != null && map.Count > 0)
                        {
                            foreach (var str2 in strings)
                            {
                                if (str2._PFlag == null)
                                {
                                    if (map.TryGetValue(str2.EndElement, out str2._PFlag) == false)
                                    {
                                        map.TryGetValue(str2.StartElement, out str2._PFlag);
                                    }
                                }
                            }
                        }
                        break;
                    }
                }

                //if (simpleList)
                //{
                //    int currentIndex = sourceList.Count - 1;
                //    XTextParagraphFlagElement lastFlag = null;
                //    for (int iCount = strings.Count - 1; iCount >= 0; iCount--)
                //    {
                //        XTextStringElement str = strings[iCount];
                //        //str._PFlag = str.EndElement.OwnerParagraphEOF;continue;
                //        XTextCharElement ee = str.EndElement;
                //        while (currentIndex > 0)
                //        {
                //            XTextElement ele = sourceList[currentIndex];
                //            currentIndex--;
                //            if (ele == ee)
                //            {
                //                break;
                //            }
                //            else if (ele is XTextParagraphFlagElement)
                //            {
                //                lastFlag = (XTextParagraphFlagElement)ele;
                //                str._PFlag = lastFlag;
                //                break;
                //            }
                //        }
                //        if (str._PFlag == null)
                //        {
                //            str._PFlag = lastFlag;
                //            if (str._PFlag == null && containerVisible )
                //            {
                //                currentIndex = -1;
                //                str._PFlag = ee.OwnerParagraphEOF;
                //            }
                //        }
                //    }
                //}

                //else
                //{
                //    foreach (XTextStringElement str in strings)
                //    {
                //        str._PFlag = str.EndElement.OwnerParagraphEOF;
                //    }
                //}
            }
            if (result.LastElement is DomParagraphFlagElement)
            {
                DomParagraphFlagElement pf = (DomParagraphFlagElement)result.LastElement;
                if (pf.AutoCreate)
                {
                    result.RemoveAt(result.Count - 1);
                }
            }
            foreach (DomElement element in result)
            {
                if (element is DomStringElement)
                {
                    ((DomStringElement)element).SetWhiteSpaceLength();
                }
            }
            return result;
        }

        public static bool SplitElements(
            DomElementList sourceList,
            bool deeply,
            DomDocument document,
            DomContainerElement parentElement,
            bool checkIsElementsSplited)
        {
            if (sourceList == null
                || sourceList.Count == 0 )
            {
                return false;
                //throw new ArgumentNullException("SourceList");
            }
            if(checkIsElementsSplited && sourceList.IsElementsSplited )
            {
                return false;
            }
            if( sourceList.GetTheSingleParagraphFlagElement() != null )
            {
                // 很多情况下列表中只有一个段落符号元素，则快速处理。
                sourceList[0].InnerSetOwnerDocumentParentRaw(document, parentElement);
                return true;
            }
            //if (sourceList.IsElementsSplited == true)
            //{
            //    return false;
            //}
            //if (sourceList.Count == 0)
            //{
            //    return false;
            //}
            var srcArr = sourceList.InnerGetArrayRaw();
            sourceList.IsElementsSplited = true;
            int sourceListCount = sourceList.Count;
            bool result = false;
            if (deeply == false)
            {
                for (var iCount = sourceListCount - 1; iCount >= 0; iCount--)
                {
                    if (srcArr[iCount] is DomTextElement || srcArr[iCount] is DomStringElement)
                    {
                        result = true;
                        break;
                    }
                }
                if (result == false)
                {
                    // 无需转换
                    return false;
                }
            }
            bool clearInvalidateCharacter = false;
            var parseCrLfAsLineBreakElement = false;
            if (document != null)
            {
                var bopts = document.GetDocumentBehaviorOptions();
                clearInvalidateCharacter = bopts.AutoClearInvalidateCharacter;
                parseCrLfAsLineBreakElement = bopts.ParseCrLfAsLineBreakElement;
            }
            //XTextElementList tempList = new XTextElementList();
            //tempList.AddRange(sourceList);
            //var newList = new List<XTextElement>();
            var newElements = new DCList<DomElement>(sourceList.Count);
            for (int elementIndex = 0; elementIndex < sourceListCount; elementIndex++)
            {
                var element = srcArr[elementIndex];// sourceList.GetItemFast(elementIndex);
                if (element is DomTextElement || element is DomStringElement)
                {
                    // 将文本元素拆分成多个字符元素
                    element.CommitStyle(false);
                    string text = element.Text;
                    if (text != null && text.Length > 0)// string.IsNullOrEmpty(text) == false)
                    {
                        int styleIndex = element._StyleIndex;
                        int itemLen = text.Length;
                        newElements.EnsureCapacity(newElements.Count + itemLen);
                        for (int cIndex2 = 0; cIndex2 < itemLen; cIndex2++)
                        {
                            char c = text[cIndex2];
                            if (c >= 32)
                            {
                                // 大多数情况下为普通正常字符
                                newElements.SuperFastAdd(new DomCharElement(c, parentElement, document, styleIndex));
                            }
                            else if (c == '\r')
                            {
                                if (parseCrLfAsLineBreakElement)
                                {
                                    DomLineBreakElement lb = new DomLineBreakElement();
                                    lb.InitDomState(parentElement, document, styleIndex);
                                    newElements.SuperFastAdd(lb);
                                }
                                else
                                {
                                    DomParagraphFlagElement p = new DomParagraphFlagElement();
                                    p.InitDomState(parentElement, document, styleIndex);
                                    newElements.SuperFastAdd(p);
                                }
                            }
                            else if (c == '\n')
                            {
                            }
                            else if (clearInvalidateCharacter && (c <= 8 || c == 31))
                            {
                                // 过滤无效字符,转换为空格
                                newElements.SuperFastAdd(new DomCharElement(' ', parentElement, document, styleIndex));
                            }
                            else
                            {
                                newElements.SuperFastAdd(new DomCharElement(c, parentElement, document, styleIndex));
                            }
                        }
                    }
                    result = true;
                }
                else
                {
                    if (deeply && element is DomContainerElement)
                    {
                        SplitElements(
                            ((DomContainerElement)element).Elements,
                            deeply,
                            document,
                            (DomContainerElement)element,
                            checkIsElementsSplited);
                    }
                    newElements.Add(element);
                }
            }//foreach
            if (result)
            {
                newElements.MoveDataTo(sourceList, true);
                //sourceList.Clear();
                //sourceList.Capacity = newElements.Count;
                //sourceList.InnerAddRangeRaw(newElements);
                //newElements.Clear();
            }
            return result;
        }

        /// <summary>
        /// 获得文档元素类型
        /// </summary>
        /// <param name="elementType">文档元素类型</param>
        /// <returns>获得的类型</returns>
        public static ElementType GetElementType(Type elementType)
        {
            if (elementType == null)
            {
                throw new ArgumentNullException("elementType");
            }
            if (typeof(DomCharElement).IsAssignableFrom(elementType))
            {
                return ElementType.Text;
            }
            if (typeof(DomParagraphFlagElement).IsAssignableFrom(elementType))
            {
                return ElementType.ParagraphFlag;
            }
            if (typeof(DomInputFieldElementBase).IsAssignableFrom(elementType))
            {
                return ElementType.InputField;
            }
            if (typeof(DomFieldElementBase).IsAssignableFrom(elementType))
            {
                return ElementType.Field;
            }
            if (typeof(DomImageElement).IsAssignableFrom(elementType))
            {
                return ElementType.Image;
            }
            if (typeof(DomObjectElement).IsAssignableFrom(elementType))
            {
                return ElementType.Object;
            }
            if (typeof(DomCheckBoxElementBase).IsAssignableFrom(elementType))
            {
                return ElementType.CheckRadioBox;
            }
            if (typeof(DomDocument).IsAssignableFrom(elementType))
            {
                return ElementType.Document;
            }
            if (typeof(DomLineBreakElement).IsAssignableFrom(elementType))
            {
                return ElementType.LineBreak;
            }
            if (typeof(DomPageBreakElement).IsAssignableFrom(elementType))
            {
                return ElementType.PageBreak;
            }
            if (typeof(DomTableCellElement).IsAssignableFrom(elementType))
            {
                return ElementType.TableCell;
            }
            if (typeof(DomTableRowElement).IsAssignableFrom(elementType))
            {
                return ElementType.TableRow;
            }
            if (typeof(DomTableElement).IsAssignableFrom(elementType))
            {
                return ElementType.Table;
            }
            if (typeof(DomTableColumnElement).IsAssignableFrom(elementType))
            {
                return ElementType.TableColumn;
            }
            return ElementType.None;
        }
    }
}
