
using DCSoft.WASM;
using System.Collections.Generic;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace DCSoft
{
    /// <summary>
    /// 字符串资源清单
    /// </summary>
    internal static class DCSR
    {
        #region 基础代码
        private static readonly Dictionary<string, string> _Values = new Dictionary<string, string>();
        public static void SetValue(string strID, string strValue)
        {
            if (strID != null && strID.Length > 0)
            {
                _Values[strID] = strValue;
            }
        }
        public static string GetValue(string strID)
        {
            if (strID != null && strID.Length > 0)
            {
                string strValue = null;
                if (_Values.TryGetValue(strID, out strValue) && strValue != null &&strValue.Length > 0)
                {
                    return strValue;
                }
            }
            return strID;
        }
#if ! RELEASE
        internal static void GenJsCode()
        {
            var ps = new List<PropertyInfo>(typeof(DCSR).GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static));
            ps.Sort(delegate (PropertyInfo p1, PropertyInfo p2)
            {
                return p1.Name.CompareTo(p2.Name);
            });
            foreach (var p in ps)
            {
                var txt = (string)p.GetValue(null);
                txt = txt.Replace("\"", "\\\"");
                JavaScriptMethods.ConsoleWriteLine("\"" + p.Name + "\":\"" + txt + "\",");
            }
        }

#endif
        #endregion
        internal static string Footer { get { return GetValue("Footer"); } }
        internal static string Header { get { return GetValue("Header"); } }
        internal static string PageBreak { get { return GetValue("PageBreak"); } }
        internal static string FixLayoutForPrint { get { return GetValue("FixLayoutForPrint"); } }
        ///<summary>所有文件|*.*</summary>
        internal static string AllFileFilter { get { return GetValue("AllFileFilter"); } }
        ///<summary>参数“{0}”值为“{1}”，超出范围，最大值“{2}”，最小值“{3}”。</summary>
        internal static string ArgumentOutOfRange_Name_Value_Max_Min { get { return GetValue("ArgumentOutOfRange_Name_Value_Max_Min"); } }
        ///<summary>由</summary>
        internal static string By { get { return GetValue("By"); } }
        ///<summary>单/复选框组“{0}”必须有勾选项。</summary>
        internal static string CheckRequired_Name { get { return GetValue("CheckRequired_Name"); } }
        ///<summary> 可能是360等安全软件实时监控系统剪切板所造成的。</summary>
        internal static string ClipboardErrorMessage { get { return GetValue("ClipboardErrorMessage"); } }
        ///<summary>删除”{0}“</summary>
        internal static string DeleteElement_Content { get { return GetValue("DeleteElement_Content"); } }
        ///<summary>删除{0}个元素</summary>
        internal static string DeleteElements_Count { get { return GetValue("DeleteElements_Count"); } }
        ///<summary>编辑器控件是只读的。</summary>
        internal static string EditControlReadonly { get { return GetValue("EditControlReadonly"); } }
        ///<summary>文档正文</summary>
        internal static string ElementType_Body { get { return GetValue("ElementType_Body"); } }
        ///<summary>字符</summary>
        internal static string ElementType_Char { get { return GetValue("ElementType_Char"); } }
        ///<summary>复选框</summary>
        internal static string ElementType_CheckBox { get { return GetValue("ElementType_CheckBox"); } }
        ///<summary>文档</summary>
        internal static string ElementType_Document { get { return GetValue("ElementType_Document"); } }
        ///<summary>页脚</summary>
        internal static string ElementType_Footer { get { return GetValue("ElementType_Footer"); } }
        ///<summary>页眉</summary>
        internal static string ElementType_Header { get { return GetValue("ElementType_Header"); } }
        ///<summary>图片</summary>
        internal static string ElementType_Image { get { return GetValue("ElementType_Image"); } }
        ///<summary>输入域</summary>
        internal static string ElementType_InputField { get { return GetValue("ElementType_InputField"); } }
        ///<summary>断行符</summary>
        internal static string ElementType_LineBreak { get { return GetValue("ElementType_LineBreak"); } }
        ///<summary>分页符</summary>
        internal static string ElementType_PageBreak { get { return GetValue("ElementType_PageBreak"); } }
        ///<summary>页码</summary>
        internal static string ElementType_PageInfo { get { return GetValue("ElementType_PageInfo"); } }
        ///<summary>段落符号</summary>
        internal static string ElementType_ParagraphFlag { get { return GetValue("ElementType_ParagraphFlag"); } }
        ///<summary>单选框</summary>
        internal static string ElementType_RadioBox { get { return GetValue("ElementType_RadioBox"); } }
        ///<summary>表格</summary>
        internal static string ElementType_Table { get { return GetValue("ElementType_Table"); } }
        ///<summary>单元格</summary>
        internal static string ElementType_TableCell { get { return GetValue("ElementType_TableCell"); } }
        ///<summary>表格列</summary>
        internal static string ElementType_TableColumn { get { return GetValue("ElementType_TableColumn"); } }
        ///<summary>表格行</summary>
        internal static string ElementType_TableRow { get { return GetValue("ElementType_TableRow"); } }
        ///<summary>数据不能为空。</summary>
        internal static string ForbidEmpty { get { return GetValue("ForbidEmpty"); } }
        ///<summary>发现重复的元素ID值“{0}”。</summary>
        internal static string IDRepeat_ID { get { return GetValue("IDRepeat_ID"); } }
        ///<summary>插入“{0}”</summary>
        internal static string InsertElement_Content { get { return GetValue("InsertElement_Content"); } }
        ///<summary>插入{0}个元素</summary>
        internal static string InsertElements_Count { get { return GetValue("InsertElements_Count"); } }
        ///<summary>错误的命令“{0}”，系统支持类似的命令有“{1}”</summary>
        internal static string InvalidateCommandName_Name_SimiliarNames { get { return GetValue("InvalidateCommandName_Name_SimiliarNames"); } }
        ///<summary>无效的页面设置，请仔细调整文档页面设置。</summary>
        internal static string InvalidatePageSettings { get { return GetValue("InvalidatePageSettings"); } }
        ///<summary>有{0}个项目。</summary>
        internal static string Items_Count { get { return GetValue("Items_Count"); } }
        ///<summary>文本过短，不得少于 {0} 个字符。</summary>
        internal static string LessThanMinLength_Length { get { return GetValue("LessThanMinLength_Length"); } }
        ///<summary>小于最小值 {0}。</summary>
        internal static string LessThanMinValue_Value { get { return GetValue("LessThanMinValue_Value"); } }
        ///<summary>第{0}页 第{1}行 第{2}列。</summary>
        internal static string LineInfo_PageIndex_LineIndex_ColumnIndex { get { return GetValue("LineInfo_PageIndex_LineIndex_ColumnIndex"); } }
        ///<summary>加载完成，共加载了{0}。</summary>
        internal static string LoadComplete_Size { get { return GetValue("LoadComplete_Size"); } }
        ///<summary>正在以 {1} 格式加载文件“{0}”...</summary>
        internal static string Loading_FileName_Format { get { return GetValue("Loading_FileName_Format"); } }
        internal static string MissProperty_Name { get { return GetValue("MissProperty_Name"); } }
        ///<summary>小数位数超过上限，上限为{0}。</summary>
        internal static string MoreThanMaxDemicalDigits { get { return GetValue("MoreThanMaxDemicalDigits"); } }
        ///<summary>文本过长，不得超过 {0} 个字符。</summary>
        internal static string MoreThanMaxLength_Length { get { return GetValue("MoreThanMaxLength_Length"); } }
        ///<summary>超过最大值 {0}。</summary>
        internal static string MoreThanMaxValue_Value { get { return GetValue("MoreThanMaxValue_Value"); } }
        ///<summary>必须为日期时间格式。</summary>
        internal static string MustDateTimeType { get { return GetValue("MustDateTimeType"); } }
        ///<summary>必须为日期格式。</summary>
        internal static string MustDateType { get { return GetValue("MustDateType"); } }
        ///<summary>必须为整数。</summary>
        internal static string MustInteger { get { return GetValue("MustInteger"); } }
        ///<summary>必须为数值。</summary>
        internal static string MustNumeric { get { return GetValue("MustNumeric"); } }
        ///<summary>必须为时间格式。</summary>
        internal static string MustTimeType { get { return GetValue("MustTimeType"); } }
        ///<summary>需要首先设置OwnerDocument属性值。</summary>
        internal static string NeedSetOwnerDocument { get { return GetValue("NeedSetOwnerDocument"); } }
        ///<summary>无文档。</summary>
        internal static string NoDocument { get { return GetValue("NoDocument"); } }
        ///<summary>没有图片</summary>
        internal static string NoImage { get { return GetValue("NoImage"); } }
        ///<summary>当前版本不支持功能“{0}”。</summary>
        internal static string NotSupportInThisVersion_Name { get { return GetValue("NotSupportInThisVersion_Name"); } }
        ///<summary>不支持以“{0}”格式进行存储。</summary>
        internal static string NotSupportSerialize_Format { get { return GetValue("NotSupportSerialize_Format"); } }
        ///<summary>不支持以纯文本格式存储为“{0}”文件格式。</summary>
        internal static string NotSupportSerializeText_Format { get { return GetValue("NotSupportSerializeText_Format"); } }
        ///<summary>不支持保存“{0}”格式的文件。</summary>
        internal static string NotSupportWrite_Format { get { return GetValue("NotSupportWrite_Format"); } }
        ///<summary>文档元素尚未属于某个文档，无法执行操作。</summary>
        internal static string OwnerDocumentNUll { get { return GetValue("OwnerDocumentNUll"); } }
        ///<summary>下页边距</summary>
        internal static string PageBottomMargin { get { return GetValue("PageBottomMargin"); } }
        ///<summary>左页边距</summary>
        internal static string PageLeftMargin { get { return GetValue("PageLeftMargin"); } }
        ///<summary>右页边距</summary>
        internal static string PageRightMargin { get { return GetValue("PageRightMargin"); } }
        ///<summary>当前文档的分页状态被锁定，无法执行重新分页操作。请不要此时调用RefreshPages()/RefreshDocument()/UpdateDocumentView()/EditorRefreshView()等容易导致重新分页的函数。</summary>
        internal static string PageStateLocked { get { return GetValue("PageStateLocked"); } }
        ///<summary>上页边距</summary>
        internal static string PageTopMargin { get { return GetValue("PageTopMargin"); } }
        ///<summary>首行缩进</summary>
        internal static string ParagraphFirstLineIndent { get { return GetValue("ParagraphFirstLineIndent"); } }
        ///<summary>左缩进</summary>
        internal static string ParagraphLeftIndent { get { return GetValue("ParagraphLeftIndent"); } }
        ///<summary>已到达文档的结尾处，是否继续从开始处搜索？</summary>
        internal static string PromptJumpStartForSearch { get { return GetValue("PromptJumpStartForSearch"); } }
        ///<summary>有内容受到保护，操作受到限制或无法执行。</summary>
        internal static string PromptProtectedContent { get { return GetValue("PromptProtectedContent"); } }
        ///<summary>系统被设定为拒绝“{0}”格式的数据。</summary>
        internal static string PromptRejectFormat_Format { get { return GetValue("PromptRejectFormat_Format"); } }
        ///<summary>属性“{0}”不能有参数。</summary>
        internal static string PropertyCannotHasParameter_Name { get { return GetValue("PropertyCannotHasParameter_Name"); } }
        ///<summary>属性“{0}”是只读的。</summary>
        internal static string PropertyIsReadonly_Name { get { return GetValue("PropertyIsReadonly_Name"); } }
        ///<summary>{0}类型的元素不能接受{1}类型的子元素。</summary>
        internal static string ReadonlyCannotAcceptElementType_ParentType_ChildType { get { return GetValue("ReadonlyCannotAcceptElementType_ParentType_ChildType"); } }
        ///<summary>不能删除输入域的背景文本。</summary>
        internal static string ReadonlyCanNotDeleteBackgroundText { get { return GetValue("ReadonlyCanNotDeleteBackgroundText"); } }
        ///<summary>不能删除输入域边界元素。</summary>
        internal static string ReadonlyCanNotDeleteBorderElement { get { return GetValue("ReadonlyCanNotDeleteBorderElement"); } }
        ///<summary>任何时候都不能删除最后一个段落符号。</summary>
        internal static string ReadonlyCanNotDeleteLastParagraphFlag { get { return GetValue("ReadonlyCanNotDeleteLastParagraphFlag"); } }
        ///<summary>输入域[{0}]的内容设置为不能直接修改。</summary>
        internal static string ReadonlyInputFieldUserEditable_ID { get { return GetValue("ReadonlyInputFieldUserEditable_ID"); } }
        ///<summary>表格行已经在表格中了。</summary>
        internal static string RowExistInTable { get { return GetValue("RowExistInTable"); } }
        ///<summary>系统提示</summary>
        internal static string SystemAlert { get { return GetValue("SystemAlert"); } }
        ///<summary>TXT文件(*.txt)|*.txt</summary>
        internal static string TXTFileFilter { get { return GetValue("TXTFileFilter"); } }
        ///<summary>对象“{0}”内容为“{1}”，数据校验错误“{2}”。</summary>
        internal static string ValueInvalidate_Source_Value_Result { get { return GetValue("ValueInvalidate_Source_Value_Result"); } }
        ///<summary>数据校验失败.</summary>
        internal static string ValueValidateFail { get { return GetValue("ValueValidateFail"); } }
        ///<summary>数据校验成功.</summary>
        internal static string ValueValidateOK { get { return GetValue("ValueValidateOK"); } }
        ///<summary>复制到何处？</summary>
        internal static string WhereToCopy { get { return GetValue("WhereToCopy"); } }
        ///<summary>移动到何处？</summary>
        internal static string WhereToMove { get { return GetValue("WhereToMove"); } }
        ///<summary>XML文件|*.xml</summary>
        internal static string XMLFilter { get { return GetValue("XMLFilter"); } }
    }
}