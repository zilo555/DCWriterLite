using System;
using System.Collections.Generic;
using System.Text;
using DCSoft.Writer.Dom;
using DCSoft.Writer.Commands;
namespace DCSoft.Writer.Controls
{
    /// <summary>
    /// 内容格式例程
    /// </summary>
    internal static class ContentFormatUtils
    {

        internal static DomElementList GetFormatHandleElements(
            DomDocument document,
            bool includeContent,
            bool includeParagraphs,
            bool includeCells,
            DocumentControler ctl
            )
        {
            if (ctl == null)
            {
                ctl = document.DocumentControler;
            }
            DomElementList list = new DomElementList();
            if (includeContent)
            {
                // 获得要处理的文档元素
                if (document != null && document.Selection.Length != 0)
                {
                    foreach (DomElement element in document.Selection.ContentElements)
                    {
                        if (ctl.CanModify(element))
                        {
                            list.Add(element);
                        }
                    }
                }
            }
            if (includeParagraphs)
            {
                // 获得要处理的段落
                if (document.Selection.Length == 0)
                {
                    DomParagraphFlagElement p = document.CurrentParagraphEOF;
                    if (ctl.CanModify(p))
                    {
                        list.Add(p);
                    }
                }
                else
                {
                    foreach (DomElement element in document.Selection.SelectionParagraphFlags)
                    {
                        if (ctl.CanModify(element))
                        {
                            list.Add(element);
                        }
                    }
                }
            }
            if (includeCells)
            {
                // 获得要处理的表格单元格
                if (document.Selection.Cells != null && document.Selection.Cells.Count > 0)
                {
                    foreach (DomElement cell in document.Selection.Cells)
                    {
                        if (ctl.CanModify(cell))
                        {
                            list.Add(cell);
                        }
                    }
                }
                else
                {
                    DomElement ce = document.CurrentElement;
                    DomTableCellElement cell = ce.OwnerCell;
                    if (cell != null && ctl.CanModify(cell))
                    {
                        list.Add(cell);
                    }
                }
            }
            if (list.Count > 0)
            {
                return list;
            }

            return null;
        }

    }
}
