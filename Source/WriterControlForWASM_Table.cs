using DCSoft.Common;
using DCSoft.Drawing;
using DCSoft.Printing;
using DCSoft.Writer;
using DCSoft.Writer.Commands;
using DCSoft.Writer.Dom;
using System.Text.Json.Nodes;
using System.Text.Json;

namespace DCSoft.WASM
{
    /// <summary>
    /// 此处对 WriterControl的接口进行转发调用
    /// 对应的，需要在前端 WriterControl_Main.js中的 BindControl()中添加对应的调用。
    /// </summary>
    partial class WriterControlForWASM
    {
        /// <summary>
        /// 获取当前单元格
        /// </summary>
        /// <returns></returns>
        [JSInvokable]
        public DotNetObjectReference<DomElement> CurrentTableCell()
        {
            try
            {
                DomTableCellElement element = this._Control.Document.CurrentTableCell;

                if (element != null)
                {
                    return DotNetObjectReference.Create<DomElement>(element); //WASMJsonConvert.ToJsonObject(element);
                                                                                //JsonObject obj = new JsonObject();
                                                                                //obj.Add("TableID", element.OwnerTable.ID);
                                                                                //obj.Add("ID", element.ID);
                                                                                //obj.Add("Width", element.Width.ToString());
                                                                                //obj.Add("Height", element.Height.ToString());
                                                                                //obj.Add("ValueBinding", parseValueBindingToJSON(element.ValueBinding));
                                                                                //obj.Add("Attributes", parseAttributesToJSON(element.Attributes));
                                                                                //obj.Add("Visible", element.Visible);
                                                                                //obj.Add("RowSpan", element.RowSpan);
                                                                                //obj.Add("ColSpan", element.ColSpan);
                                                                                //obj.Add("RowIndex", element.RowIndex);
                                                                                //obj.Add("ColIndex", element.ColIndex);
                                                                                //obj.Add("Text", element.Text);
                                                                                //obj.Add("Deleteable", element.Deleteable);
                                                                                //obj.Add("ContentReadonly", element.ContentReadonly.ToString());
                                                                                //obj.Add("CloneType", element.CloneType.ToString());
                                                                                //obj.Add("SlantSplitLineStyle", element.SlantSplitLineStyle.ToString());
                                                                                //obj.Add("Style", parseBorderStyleToJSON(element.Style));
                                                                                //return obj;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                JavaScriptMethods.Tools_ReportException("CurrentTableCell", ex.Message, ex.ToString(), 0);
                return null;
            }
        }


        /// <summary>
        /// 兼容第四代获取当前表格
        /// </summary>
        /// <returns></returns>
        [JSInvokable]
        public DotNetObjectReference<DomElement> CurrentTable()
        {
            try
            {
                DomTableElement element = this._Control.CurrentTable;
                if (element != null)
                {
                    return DotNetObjectReference.Create<DomElement>(element); //BuildTableElement(element);
                }
                return null;
            }
            catch (Exception ex)
            {
                JavaScriptMethods.Tools_ReportException("CurrentTable", ex.Message, ex.ToString(), 0);
                return null;
            }
        }

        /// <summary>
        /// 兼容第四代获取当前表格行
        /// </summary>
        /// <returns></returns>
        [JSInvokable]
        public DotNetObjectReference<DomElement> CurrentTableRow()
        {
            try
            {
                DomTableRowElement element = this._Control.CurrentTableRow;
                if (element != null)
                {
                    return DotNetObjectReference.Create<DomElement>(element);
                }
                return null;
            }
            catch (Exception ex)
            {
                JavaScriptMethods.Tools_ReportException("CurrentTableRow", ex.Message, ex.ToString(), 0);
                return null;
            }
        }
    }
}