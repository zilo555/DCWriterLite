using DCSoft.Writer.Dom ;
using DCSoft.Drawing;
//using System.Data;
using DCSoft.Writer.Controls ;

namespace DCSoft.Writer
{
    /// <summary>
    /// 编辑器帮助类，定义了一些常用例程
    /// </summary>
    /// <remarks>编制 袁永福</remarks>
    public static class WriterUtils
    {
        public static Type GetDOMElementType(string typeName)
        {
            if (string.IsNullOrEmpty(typeName))
            {
                throw new ArgumentNullException("typeName");
            }
            if (DomCharElement.TypeName_XTextCharElement.Equals(typeName, StringComparison.OrdinalIgnoreCase)) return typeof(DomCharElement);
            if (DomCheckBoxElement.TypeName_XTextCheckBoxElement.Equals(typeName, StringComparison.OrdinalIgnoreCase)) return typeof(DomCheckBoxElement);
            if (DomDocument.TypeName_XTextDocument.Equals(typeName, StringComparison.OrdinalIgnoreCase)) return typeof(DomDocument);
            if (DomDocumentBodyElement.TypeName_XTextDocumentBodyElement.Equals(typeName, StringComparison.OrdinalIgnoreCase)) return typeof(DomDocumentBodyElement);
            if ("XTextDocumentFooterElement".Equals(typeName, StringComparison.OrdinalIgnoreCase)) return typeof(DomDocumentFooterElement);
            if ("XTextDocumentHeaderElement".Equals(typeName, StringComparison.OrdinalIgnoreCase)) return typeof(DomDocumentHeaderElement);
            if ("XTextFieldBorderElement".Equals(typeName, StringComparison.OrdinalIgnoreCase)) return typeof(DomFieldBorderElement);
            if (DomImageElement.TypeName_XTextImageElement.Equals(typeName, StringComparison.OrdinalIgnoreCase)) return typeof(DomImageElement);
            if (DomInputFieldElement.TypeName_XTextInputFieldElement.Equals(typeName, StringComparison.OrdinalIgnoreCase)) return typeof(DomInputFieldElement);
            if ("XTextLineBreakElement".Equals(typeName, StringComparison.OrdinalIgnoreCase)) return typeof(DomLineBreakElement);
            if ("XTextPageBreakElement".Equals(typeName, StringComparison.OrdinalIgnoreCase)) return typeof(DomPageBreakElement);
            if (DomPageInfoElement.TypeName_XTextPageInfoElement.Equals(typeName, StringComparison.OrdinalIgnoreCase)) return typeof(DomPageInfoElement);
            if ("XTextParagraphElement".Equals(typeName, StringComparison.OrdinalIgnoreCase)) return typeof(DomParagraphElement);
            if ("XTextParagraphFlagElement".Equals(typeName, StringComparison.OrdinalIgnoreCase)) return typeof(DomParagraphFlagElement);
            if ("XTextParagraphListItemElement".Equals(typeName, StringComparison.OrdinalIgnoreCase)) return typeof(DomParagraphListItemElement);
            if (DomRadioBoxElement.TypeName_XTextRadioBoxElement.Equals(typeName, StringComparison.OrdinalIgnoreCase)) return typeof(DomRadioBoxElement);
            if (DomStringElement.TypeName_XTextStringElement.Equals(typeName, StringComparison.OrdinalIgnoreCase)) return typeof(DomStringElement);
            if (DomTableCellElement.TypeName_XTextTableCellElement.Equals(typeName, StringComparison.OrdinalIgnoreCase)) return typeof(DomTableCellElement);
            if (DomTableColumnElement.TypeName_XTextTableColumnElement.Equals(typeName, StringComparison.OrdinalIgnoreCase)) return typeof(DomTableColumnElement);
            if (DomTableElement.TypeName_XTextTableElement.Equals(typeName, StringComparison.OrdinalIgnoreCase)) return typeof(DomTableElement);
            if (DomTableRowElement.TypeName_XTextTableRowElement.Equals(typeName, StringComparison.OrdinalIgnoreCase)) return typeof(DomTableRowElement);
            if ("XTextTextElement".Equals(typeName, StringComparison.OrdinalIgnoreCase)) return typeof(DomTextElement);
            return null;
        }

        public static void ReadImageDataBase64String(DCSystemXml.XmlReader reader, XImageValue img)
        {
            if (reader is DCSoft.Writer.Serialization.ArrayXmlReader)
            {
                var reader2 = (DCSoft.Writer.Serialization.ArrayXmlReader)reader;
                reader2.ReadForImageDataBase64String(img);
            }
            else
            {
                img.ImageDataBase64String = reader.ReadElementString();
            }
        }
        /// <summary>
        /// 获得运行时使用的文件名
        /// </summary>
        /// <param name="specifyfilename">指定的文件名</param>
        /// <param name="doc">文档对象</param>
        /// <returns>获得的运行时的文件名</returns>
        public static string GetRuntimeFileName(string specifyfilename, DomDocument doc)
        {
            string fileName = specifyfilename;
            if ((fileName == null || fileName.Length == 0) && doc != null)
            {
                fileName = doc.FileName;
                if (fileName == null || fileName.Length == 0)
                {
                    fileName = doc.Info.Title;
                }
                if (fileName == null || fileName.Length == 0)
                {
                    fileName = doc.ID;
                }
            }
            if (fileName != null && fileName.Length > 0)
            {
                int index = fileName.LastIndexOfAny(new char[] { '/', '\\' });
                if (index >= 0)
                {
                    fileName = fileName.Substring(index + 1);
                }
                index = fileName.LastIndexOf('.');
                if (index > 0)
                {
                    fileName = fileName.Substring(0, index);
                }
                fileName = DCSoft.Common.FileHelper.FixFileName(fileName, '_');
                fileName = fileName.Replace(' ', '_');
            }
            if (fileName == null || fileName.Length == 0)
            {
                fileName = WriterControl.GetServerTime().ToString("yyyyMMddHHmmss");
            }
            return fileName;
        }

    }
}
