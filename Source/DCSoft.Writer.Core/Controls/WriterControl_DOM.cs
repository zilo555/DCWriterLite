using DCSoft.Writer.Dom;
using DCSoft.Writer.Commands;
using DCSoft.Drawing;
using System.ComponentModel;
using DCSoft.Common;
using System.Text;
using System.Runtime.InteropServices;
//using System.Collections.Specialized;

namespace DCSoft.Writer.Controls
{
    /// <summary>
    /// 编辑器控件的文档对象模型处理模块
    /// </summary>
    public partial class WriterControl
    {
        /// <summary>
        /// 文档内容进行校验，返回校验结果
        /// </summary>
        /// <returns></returns>
        public ValueValidateResultList DocumentValueValidate()
        {
            if (this.myViewControl == null)
            {
                return null;
            }
            else
            {
                return this.myViewControl.Document.ValueValidate();
            }
        }

        /// <summary>
        /// 文档内容改变标记
        /// </summary>
        public bool Modified
        {
            get
            {
                return this.myViewControl.Modified;
            }
            set
            {
                this.myViewControl.Modified = value;
            }
        }
        /// <summary>
        /// 文档中包含的内容被修改的文本输入域列表对象
        /// </summary>
        public DomElementList ModifiedInputFields
        {
            get
            {
                return this.Document?.ModifiedInputFields;
            }
        }

        /// <summary>
        /// 当前光标所在的粗体样式
        /// </summary>
        public bool CurrentBold
        {
            get
            {
                return this.Document.CurrentStyleInfo.Content.Bold;
            }
        }

        /// <summary>
        /// 当前光标所在的下划线样式
        /// </summary>
        public bool CurrentUnderline
        {
            get
            {
                return this.Document.CurrentStyleInfo.Content.Underline ;
            }
        }

        /// <summary>
        /// 当前光标所在的上标样式
        /// </summary>
        public bool CurrentSuperscript
        {
            get
            {
                return this.Document.CurrentStyleInfo.Content.Superscript;
            }
        }

        /// <summary>
        /// 当前光标所在的下标样式
        /// </summary>
        public bool CurrentSubscript
        {
            get
            {
                return this.Document.CurrentStyleInfo.Content.Subscript;
            }
        }

        /// <summary>
        /// 当前段落对齐方式
        /// </summary>
        public DocumentContentAlignment CurrentParagraphAlign
        {
            get
            {
                return this.Document.CurrentStyleInfo.Paragraph.Align;
            }
        }
   
        /// <summary>
        /// 当前段落对象
        /// </summary>
        public DomParagraphFlagElement CurrentParagraphEOF
        {
            get
            {
                return this.Document.CurrentParagraphEOF;
            }
        }
        /// <summary>
        /// 文档对象
        /// </summary>
        public DomDocument Document
        {
            get
            {
                return this.myViewControl?.Document;
            }
            set
            {
                if (this.myViewControl != null)
                {
                    this.myViewControl.Document = value;
                }
            }
        }


        /// <summary>
        /// 当前元素样式
        /// </summary>
        public DocumentContentStyle CurrentStyle
        {
            get
            {
                return this.Document?.CurrentStyleInfo.Content;
            }
        }
        /// <summary>
        /// 当前插入点所在的元素
        /// </summary>
        public DomElement CurrentElement
        {
            get
            {
                return this.Document?.CurrentElement;
            }
        }

        /// <summary>
        /// 当前单选的文档元素对象
        /// </summary>
        /// <remarks>
        /// 当只选中一个文档元素对象，则返回给文档元素对象，如果没有选中元素
        /// 或者选中多个元素，则返回空。
        /// </remarks>
        public DomElement SingleSelectedElement
        {
            get
            {
                var es = this.Selection?.ContentElements;
                if(es != null && es.Count == 1 )
                {
                    return es[0];
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// 当前插入点所在的输入域元素
        /// </summary>
        public DomInputFieldElement CurrentInputField
        {
            get
            {
                DomInputFieldElement element = this.GetCurrentElement(typeof(DomInputFieldElement)) as DomInputFieldElement;
                //this.CollectOuterReference(element);
                return element;
            }
        }

        /// <summary>
        /// 当前插入点所在的单元格元素
        /// </summary>
        public DomTableCellElement CurrentTableCell
        {
            get
            {
                var element = this.GetCurrentElement(typeof(DomTableCellElement)) as DomTableCellElement;
                //this.CollectOuterReference(element);
                return element;
            }
        }

        /// <summary>
        /// 当前插入点所在的表格行元素
        /// </summary>
        public DomTableRowElement CurrentTableRow
        {
            get
            {
                var element = this.GetCurrentElement(typeof(DomTableRowElement)) as DomTableRowElement;
                //this.CollectOuterReference(element);
                return element;
            }
        }

        /// <summary>
        /// 当前插入点所在的表格元素
        /// </summary>
        public DomTableElement CurrentTable
        {
            get
            {
                var table = this.GetCurrentElement(typeof(DomTableElement)) as DomTableElement;
                //this.CollectOuterReference(table);
                return table;
            }
        }

        /// <summary>
        /// 获得当前插入点所在的指定类型的文档元素对象。
        /// </summary>
        /// <param name="elementType">指定的文档元素类型</param>
        /// <returns>获得的文档元素对象</returns>
        public DomElement GetCurrentElement(Type elementType)
        {
            return this.Document?.GetCurrentElement(elementType, true);
        }
        /// <summary>
        /// 鼠标悬停的元素
        /// </summary>
        public DomElement HoverElement
        {
            get
            {
                return this.myViewControl?.HoverElement();
            }
        }

        /// <summary>
        /// 获得表单数据
        /// </summary>
        /// <param name="name">字段名称</param>
        /// <returns>获得的表单数值</returns>
        public string GetFormValue(string name)
        {
            return this.Document?.GetFormValue(name);
        }

        /// <summary>
        /// 获得指定ID号的文档元素对象,查找时ID值区分大小写的。
        /// </summary>
        /// <param name="id">编号</param>
        /// <returns>找到的文档元素对象</returns>
        public DomElement GetElementById(string id)
        {
            DomElement element = this.Document?.GetElementById(id);
            //this.CollectOuterReference(element);
            return element;
        }

        /// <summary>
        /// 获得指定ID号的输入域对象,查找时ID值区分大小写的。
        /// </summary>
        /// <param name="id">ID号</param>
        /// <returns>找到的输入域元素对象</returns>
        public DomInputFieldElement GetInputFieldElementById(string id)
        {
            return this.GetElementById(id) as DomInputFieldElement;
        }

        /// <summary>
        /// 获得指定ID号的表格对象,查找时ID值区分大小写的。
        /// </summary>
        /// <param name="id">ID号</param>
        /// <returns>找到的表格元素对象</returns>
        public DomTableElement GetTableElementById(string id)
        {
            return this.GetElementById(id) as DomTableElement;
        }

        /// <summary>
        /// 获得系统当前日期时间
        /// </summary>
        /// <returns>日期事件</returns>
        public virtual DateTime GetNowDateTime()
        {
            return DateTimeCommon.GetNow();
        }

        /// <summary>
        /// 清空重做/撤销操作信息
        /// </summary>
        public void ClearUndo()
        {
            if (this.Document != null)
            {
                this.Document.CancelLogUndo();
                this.Document.UndoList.Clear();
                if (this.myViewControl != null)
                {
                    this.myViewControl.InvalidateCommandState(StandardCommandNames.Undo);
                    this.myViewControl.InvalidateCommandState(StandardCommandNames.Redo);
                }
            }
        }

        /// <summary>
        /// 清空文档内容
        /// </summary>
        public void ClearContent()
        {
            this.ClearDocumentState();
            if (this.myViewControl != null)
            {
                this.myViewControl.ClearContent();
            }
        }
        internal void ClearDocumentState()
        {
            this.Document?.HighlightManager?.Clear();
            if (this.DocumentControler != null)
            {
                this.DocumentControler.Clear();
            }
            if (this.ctlHRule != null)
            {
                this.ctlHRule.ClearDocumentState();
            }
            if (this.ctlVRule != null)
            {
                this.ctlVRule.ClearDocumentState();
            }
        }

        /// <summary>
        /// 选择内容
        /// </summary>
        /// <param name="startContentIndex">选择区域起始编号</param>
        /// <param name="endContentIndex">选择区域终止编号</param>
        /// <returns>操作是否成功</returns>
        public bool SelectContentByStartEndIndex(int startContentIndex, int endContentIndex)
        {
            if (this.Document == null)
            {
                return false;
            }
            else
            {
                return this.Document.SelectContentByStartEndIndex(startContentIndex, endContentIndex);
            }
        }

        /// <summary>
        /// 选择内容
        /// </summary>
        /// <param name="startElement">选择区域起始元素</param>
        /// <param name="endElement">选择区域终止元素</param>
        /// <returns>操作是否成功</returns>
        public bool SelectContentByStartEndElement(DomElement startElement, DomElement endElement)
        {
            if (this.Document == null)
            {
                return false;
            }
            else
            {
                return this.Document.SelectContentByStartEndElement(startElement, endElement);
            }
        }

        /// <summary>
        /// 文档选择的部分
        /// </summary>
        public DCSelection Selection
        {
            get
            {
                return this.myViewControl?.Document?.Selection;
            }
        }

        /// <summary>
        /// 文档中被选中的文字
        /// </summary>
        public string SelectedText
        {
            get
            {
                return this.myViewControl?.Document?.Selection?.Text;
            }
        }

    }
}
