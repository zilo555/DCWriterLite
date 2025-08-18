using System;
using System.Collections.Generic;
using System.Text;
using DCSoft.Writer.Dom;
using DCSoft.Writer.Controls;
using System.Windows.Forms;
using DCSoft.Drawing;
using DCSoft.Writer.Undo;
//  
using DCSoft.Common;
using DCSoft.WinForms.Design;
using DCSoft.Writer.Data;
using System.ComponentModel;
//using DCSoft.Drawing.Design;
using DCSoft.Writer.Dom.Undo;
using System.Reflection ;
using DCSoft.WinForms;
namespace DCSoft.Writer.Commands
{
    /// <summary>
    /// 文档内容格式命令模块
    /// </summary>
    /// <remarks>编制 袁永福</remarks>
    internal sealed class WriterCommandModuleFormat : WriterCommandModule
    {
        /// <summary>
        /// 初始化对象
        /// </summary>
        public WriterCommandModuleFormat()
        {
        }
        // DCSoft.Writer.Commands.WriterCommandModuleFormat
        protected override WriterCommandList CreateCommands()
        {
            var list = new WriterCommandList();
            AddCommandToList(list, StandardCommandNames.AlignCenter, this.AlignCenter);
            AddCommandToList(list, StandardCommandNames.AlignDistribute, this.AlignDistribute);
            AddCommandToList(list, StandardCommandNames.AlignJustify, this.AlignJustify);
            AddCommandToList(list, StandardCommandNames.AlignLeft, this.AlignLeft);
            AddCommandToList(list, StandardCommandNames.AlignRight, this.AlignRight);
            AddCommandToList(list, StandardCommandNames.BackColor, this.BackColor);
            AddCommandToList(list, StandardCommandNames.Bold, this.Bold, System.Windows.Forms.Keys.B | System.Windows.Forms.Keys.Control);
            AddCommandToList(list, StandardCommandNames.BorderBackgroundFormat, this.BorderBackgroundFormat);
            AddCommandToList(list, StandardCommandNames.BorderBottom, this.BorderBottom);
            AddCommandToList(list, StandardCommandNames.BorderLeft, this.BorderLeft);
            AddCommandToList(list, StandardCommandNames.BorderRight, this.BorderRight);
            AddCommandToList(list, StandardCommandNames.BorderTop, this.BorderTop);
            AddCommandToList(list, StandardCommandNames.BulletedList, this.BulletedList);
            AddCommandToList(list, StandardCommandNames.ClearFormat, this.ClearFormat);
            AddCommandToList(list, StandardCommandNames.Color, this.ColorFunction);
            AddCommandToList(list, StandardCommandNames.FirstLineIndent, this.FirstLineIndent);
            AddCommandToList(list, StandardCommandNames.Font, this.Font);
            AddCommandToList(list, StandardCommandNames.FontName, this.FontName);
            AddCommandToList(list, StandardCommandNames.FontSize, this.FontSize);
            AddCommandToList(list, StandardCommandNames.FormatBrush, this.FormatBrush);
            AddCommandToList(list, StandardCommandNames.Header1, this.Header1);
            AddCommandToList(list, StandardCommandNames.Header1WithMultiNumberlist, this.Header1WithMultiNumberlist);
            AddCommandToList(list, StandardCommandNames.Header2, this.Header2);
            AddCommandToList(list, StandardCommandNames.Header2WithMultiNumberlist, this.Header2WithMultiNumberlist);
            AddCommandToList(list, StandardCommandNames.Header3, this.Header3);
            AddCommandToList(list, StandardCommandNames.Header3WithMultiNumberlist, this.Header3WithMultiNumberlist);
            AddCommandToList(list, StandardCommandNames.Header4, this.Header4);
            AddCommandToList(list, StandardCommandNames.Header4WithMultiNumberlist, this.Header4WithMultiNumberlist);
            AddCommandToList(list, StandardCommandNames.Header5, this.Header5);
            AddCommandToList(list, StandardCommandNames.Header5WithMultiNumberlist, this.Header5WithMultiNumberlist);
            AddCommandToList(list, StandardCommandNames.Header6, this.Header6);
            AddCommandToList(list, StandardCommandNames.Header6WithMultiNumberlist, this.Header6WithMultiNumberlist);
            AddCommandToList(list, StandardCommandNames.HeaderFormat, this.HeaderFormat);
            AddCommandToList(list, StandardCommandNames.IncreaseFirstLineIndent, this.IncreaseFirstLineIndent);
            AddCommandToList(list, StandardCommandNames.InputFieldUnderLine, this.InputFieldUnderLine);
            AddCommandToList(list, StandardCommandNames.Italic, this.Italic, System.Windows.Forms.Keys.I | System.Windows.Forms.Keys.Control);
            AddCommandToList(list, StandardCommandNames.NumberedList, this.NumberedList);
            AddCommandToList(list, StandardCommandNames.Padding, this.Padding);
            AddCommandToList(list, StandardCommandNames.ParagraphFormat, this.ParagraphFormat);
            AddCommandToList(list, StandardCommandNames.ParagraphListStyle, this.ParagraphListStyle);
            AddCommandToList(list, StandardCommandNames.Strikeout, this.Strikeout);
            AddCommandToList(list, StandardCommandNames.Subscript, this.Subscript);
            AddCommandToList(list, StandardCommandNames.Superscript, this.Superscript);
            AddCommandToList(list, StandardCommandNames.Underline, this.Underline, System.Windows.Forms.Keys.U | System.Windows.Forms.Keys.Control);
            return list;
        }

        private static DomElementList GetFormatHandleElements(
            DomDocument document ,
            bool includeContent,
            bool includeParagraphs,
            bool includeCells,
            DocumentControler ctl )
        {
            if (ctl == null)
            {
                ctl = (DocumentControler)document.DocumentControler;
            }
            if( ctl == null )
            {
                return null;
            }
            DomElementList list = new DomElementList();
            ctl.BeginCacheValue();
            try
            {
                if (includeContent)
                {
                    // 获得要处理的文档元素
                    if (document != null && document.Selection.Length != 0)
                    {
                        foreach (DomElement element in document.Selection.ContentElements)
                        {
                            if (ctl.CanModify(element))
                            {
                                list.FastAdd2(element);
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
                        if (p != null && ctl.CanModify(p))
                        {
                            list.FastAdd2(p);
                        }
                    }
                    else
                    {
                        foreach (DomElement element in document.Selection.SelectionParagraphFlags)
                        {
                            if (ctl.CanModify(element))
                            {
                                list.FastAdd2(element);
                            }
                        }
                    }
                }
                if (includeCells)
                {
                    // 获得要处理的表格单元格
                    if (document.Selection.Cells != null && document.Selection.Cells.Count > 0)
                    {
                        foreach (DomTableCellElement cell in document.Selection.Cells)
                        {
                            if (cell.IsOverrided == false
                                && ctl.CanModify(cell))
                            {
                                list.FastAdd2(cell);
                            }
                        }
                    }
                    else
                    {
                        DomElement ce = document.CurrentElement;
                        if (ce != null)
                        {
                            DomTableCellElement cell = ce.OwnerCell;
                            if (cell != null
                                && cell.IsOverrided == false
                                && ctl.CanModify(cell))
                            {
                                list.FastAdd2(cell);
                            }
                        }
                    }
                }
            }
            finally
            {
                ctl.EndCacheValue();
            }
            if (list.Count > 0)
            {
                return list;
            }

            return null;
        }

        /// <summary>
        /// 用设置下边框的方式来设置下划线，此命令会强制更改操作元素的边框
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        [WriterCommandDescription(
            StandardCommandNames.InputFieldUnderLine,
            ReturnValueType = typeof(bool),
            DefaultResultValue = false)]
        // ImageResource = "DCSoft.Writer.Commands.Images.CommandUnderline.bmp")]
        private void InputFieldUnderLine(object sender, WriterCommandEventArgs args)
        {

            if (args.Mode == WriterCommandEventMode.QueryState)
            {
                args.Enabled = false;
                if (args.EditorControl != null &&
                    args.EditorControl.Readonly == false &&
                    args.EditorControl.CurrentInputField != null &&
                    args.DocumentControler.CanModify(args.EditorControl.CurrentInputField))
                {
                    args.Enabled = true;
                }
            }
            else if (args.Mode == WriterCommandEventMode.Invoke)
            {
                args.Result = false;
                bool bSet = false;  //是取消下划线还是要加上下划线
                InputFieldUnderLineCommandParameter parameter = new InputFieldUnderLineCommandParameter();

                DomInputFieldElement input = args.EditorControl.CurrentInputField;
                DocumentContentStyle style = null;

                if (input != null)
                {
                    style = input.Style;
                }
                else
                {
                    return;
                }

                //命令参数接受简单的BOOL值或复杂的下划线样式对象，其余类型不处理
                if (args.Parameter is bool)
                {
                    bSet = (bool)args.Parameter;
                }
                else if (args.Parameter is InputFieldUnderLineCommandParameter)
                {
                    bSet = true;
                    parameter = (InputFieldUnderLineCommandParameter)args.Parameter;
                }

                if (bSet)
                {
                    style.BorderTop = false;
                    style.BorderLeft = false;
                    style.BorderRight = false;
                    style.BorderBottom = true;
                    style.BorderBottomColor = parameter.InputFieldUnderLineColor;
                    style.BorderStyle = parameter.InputFieldUnderLineStyle;
                    style.BorderWidth = parameter.InputFieldUnderLineWidth;
                }
                else
                {
                    style.BorderTop = false;
                    style.BorderLeft = false;
                    style.BorderRight = false;
                    style.BorderBottom = false;
                }

                //开始设置元素的STYLE
                args.Document.BeginLogUndo();
                if (input != null)
                {
                    input.Style = style;                 
                    input.EditorRefreshView();
                }
                args.Document.EndLogUndo();
                args.Document.OnDocumentContentChanged();
                args.Result = true;
            }
        }

        /// <summary>
        /// 格式刷
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        [WriterCommandDescription(
            StandardCommandNames.FormatBrush,
            ReturnValueType = typeof(bool),
            DefaultResultValue = false)]
        private void FormatBrush(object sender, WriterCommandEventArgs args)
        {
            if (args.Mode == WriterCommandEventMode.QueryState)
            {
                args.Enabled = false;
                if (args.EditorControl != null && args.EditorControl.Readonly == false)
                {
                    args.Enabled = true;
                    args.Checked = args.EditorControl.GetInnerViewControl().StyleInfoForFormatBrush != null;
                }
            }
            else if (args.Mode == WriterCommandEventMode.Invoke)
            {
                args.Result = false;
                args.RefreshLevel = UIStateRefreshLevel.Current;
                args.EditorControl.CancelEditElementValue();
                args.EditorControl.GetInnerViewControl().StyleInfoForFormatBrush = args.Document.CurrentStyleInfo.Clone();
                args.Result = true;
            }
        }

        /// <summary>
        /// 清空文档样式格式
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        [WriterCommandDescription( 
            StandardCommandNames.ClearFormat ,
            ReturnValueType = typeof( bool ),
            DefaultResultValue = false )]
        private void ClearFormat(object sender, WriterCommandEventArgs args)
        {
            if (args.Mode == WriterCommandEventMode.QueryState)
            {
                args.Enabled = false;
                if (args.Document != null)
                {
                    DomElementList list = GetFormatHandleElements( 
                        args.Document ,
                        true ,
                        true , 
                        true ,
                        (DocumentControler)args.DocumentControler );
                    if( list != null && list.Count > 0 )
                    {
                        args.Enabled = true ;
                    }
                }
            }
            else if (args.Mode == WriterCommandEventMode.Invoke)
            {
                args.Result = false;
                DomElementList list = GetFormatHandleElements(
                    args.Document ,
                    true,
                    true,
                    true,
                    (DocumentControler)args.DocumentControler );
                if (list != null && list.Count > 0)
                {
                    args.EditorControl.CancelFormatBrush();
                    args.Document.BeginLogUndo();
                    DocumentContentStyle ns = ( DocumentContentStyle ) args.Document.ContentStyles.Default.Clone();
                    ns.DisableDefaultValue = true;
                    args.Result = DCSelection.SetElementStyle(
                        ns , 
                        ns ,
                        ns ,
                        args.Document,
                        list,
                        true, 
                        args.Name,
                        true );
                    args.Document.EndLogUndo();
                    if ((bool)args.Result)
                    {
                        args.Document.OnSelectionChanged();
                        args.Document.OnDocumentContentChanged();
                    }
                }
            }
        }

        /// <summary>
        /// 设置段落格式
        /// </summary>
        /// <param name="sender">参数</param>
        /// <param name="args">参数</param>
        [WriterCommandDescription(
            StandardCommandNames.ParagraphFormat,
            ReturnValueType = typeof(bool))]
        private void ParagraphFormat(object sender, WriterCommandEventArgs args)
        {
            if (args.Mode == WriterCommandEventMode.QueryState)
            {
                args.Enabled = args.DocumentControler != null
                    && args.DocumentControler.Snapshot.CanModifySelection;
            }
            else if (args.Mode == WriterCommandEventMode.Invoke)
            {
                args.Result = false;
                ParagraphFormatCommandParameter parameter = args.Parameter
                    as ParagraphFormatCommandParameter;
                if (parameter == null)
                {
                    parameter = new ParagraphFormatCommandParameter();
                    parameter.Read(args.Document.CurrentStyleInfo.Paragraph );
                }
                DocumentContentStyle ns = new DocumentContentStyle();
                ns.DisableDefaultValue = true;
                parameter.Save(ns);
                args.Document.BeginLogUndo();
                DomElementList list = args.Document.Selection.SetParagraphStyle(ns);
                if (list != null && list.Count > 0)
                {
                    args.Result = true ;
                }
                args.Document.EndLogUndo();
                args.Document.OnSelectionChanged();
                args.Document.OnDocumentContentChanged();
                args.EditorControl.UpdateRuleState();
                args.RefreshLevel = UIStateRefreshLevel.All;
            }
        }

        ///// <summary>
        ///// 设置输入域的边框和背景
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="args"></param>
        //[WriterCommandDescription( 
        //    StandardCommandNames.FieldBorderBackgroundFormat ,
        //    ReturnValueType = typeof( bool ) ,
        //    DefaultResultValue = false ,
        //    // ImageResource = "DCSoft.Writer.Commands.Images.CommandBorderBackgroundFormat.bmp")]
        //private void FieldBorderBackgroundFormat(object sender, WriterCommandEventArgs args)
        //{
        //    if (args.Mode == WriterCommandEventMode.QueryState)
        //    {
        //        args.Enabled = false;
        //        XTextFieldElementBase field = ( XTextFieldElementBase ) args.Document.GetCurrentElement(typeof(XTextFieldElementBase));
        //        if (field != null)
        //        {
        //            if (args.DocumentControler.CanModify(field))
        //            {
        //                args.Enabled = true;
        //            }
        //        }
        //    }
        //    else if (args.Mode == WriterCommandEventMode.Invoke)
        //    {
        //        args.Result = false;
        //        XTextFieldElementBase field = (XTextFieldElementBase)args.Document.GetCurrentElement(typeof(XTextFieldElementBase));
        //        if (field == null || args.DocumentControler.CanModify(field) == false)
        //        {
        //            return;
        //        }
        //        BorderBackgroundCommandParameter parameter = args.Parameter as BorderBackgroundCommandParameter;
        //        if (parameter == null)
        //        {
        //            parameter = new BorderBackgroundCommandParameter();
        //            ReadElementBorderBackgroundFormatSettings(parameter, args, field);
        //        }
        //        if (args.ShowUI)
        //        {
        //            using (dlgDocumentBorderBackground dlg = new dlgDocumentBorderBackground())
        //            {
        //                if (args.EditorControl != null)
        //                {
        //                    dlg.Text = args.EditorControl.DialogTitlePrefix + dlg.Text;
        //                }
        //                dlg.CommandParameter = parameter;
        //                dlg.ShowApplyRange = false;
        //                dlg.CompleMode = false;
        //                if (dlg.ShowDialog(args.EditorControl) == DialogResult.OK)
        //                {
        //                }
        //                else
        //                {
        //                    // 用户取消操作
        //                    return;
        //                }
        //            }//using
        //        }//if
        //        SetElementBorderBackgroundFormat(parameter, args, field);
        //    }
        //}


        /// <summary>
        /// 边框和背景样式
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        [WriterCommandDescription(
            StandardCommandNames.BorderBackgroundFormat,
            ReturnValueType=typeof( bool ))]
        // ImageResource = "DCSoft.Writer.Commands.Images.CommandBorderBackgroundFormat.bmp")]
        private void BorderBackgroundFormat(object sender, WriterCommandEventArgs args)
        {
            if (args.Mode == WriterCommandEventMode.QueryState)
            {
                StyleApplyRanges ranges = GetAllowApplyRange(args);
                args.Enabled = ranges != StyleApplyRanges.None;
            }
            else if (args.Mode == WriterCommandEventMode.Invoke)
            {
                StyleApplyRanges allowRanges = GetAllowApplyRange(args);
                args.Result = false;
                DomElement simpleElement = null;
                if (Math.Abs(args.Document.Selection.Length) == 1)
                {
                    simpleElement = args.Document.Selection.ContentElements[0];
                    if (simpleElement is DomObjectElement)
                    {
                    }
                    else
                    {
                        simpleElement = null;
                    }
                }
                if (simpleElement != null)
                {
                    if (args.DocumentControler.CanModify(simpleElement) == false)
                    {
                        // 唯一的要处理的元素不能修改，则退出
                        return;
                    }
                }
                BorderBackgroundCommandParameter parameter = args.Parameter
                    as BorderBackgroundCommandParameter;
                if (parameter == null && args.ShowUI == false)
                {
                    // 操作无意义
                    return;
                }
                if (parameter == null)
                {
                    parameter = new BorderBackgroundCommandParameter();
                    if (args.Document.Selection.Length != 0)
                    {
                        if (args.Document.Selection.Mode == ContentRangeMode.Cell)
                        {
                            ReadCellBorderBackgroundFormatSettings(parameter, args);
                            parameter.ApplyRange = StyleApplyRanges.Cell;
                        }
                        else
                        {
                            ReadElementBorderBackgroundFormatSettings(
                                parameter,
                                args,
                                args.Document.Selection.ContentElements[0]);
                            if ((allowRanges & StyleApplyRanges.Text) == StyleApplyRanges.Text)
                            {
                                parameter.ApplyRange = StyleApplyRanges.Text;
                            }
                            else if ((allowRanges & StyleApplyRanges.Field) == StyleApplyRanges.Field)
                            {
                                parameter.ApplyRange = StyleApplyRanges.Field;
                            }
                            else if ((allowRanges & StyleApplyRanges.Paragraph) == StyleApplyRanges.Paragraph)
                            {
                                parameter.ApplyRange = StyleApplyRanges.Paragraph;
                            }
                            else if ((allowRanges & StyleApplyRanges.Cell) == StyleApplyRanges.Cell)
                            {
                                parameter.ApplyRange = StyleApplyRanges.Cell;
                            }
                            //parameter.ApplyRange = StyleApplyRanges.Text;
                        }
                    }
                    else
                    {
                        if ((allowRanges & StyleApplyRanges.Field) == StyleApplyRanges.Field)
                        {
                            parameter.ApplyRange = StyleApplyRanges.Field;
                            ReadElementBorderBackgroundFormatSettings(
                                parameter,
                                args,
                                args.Document.GetCurrentElement(typeof(DomInputFieldElement)));
                            parameter.ApplyRange = StyleApplyRanges.Field;
                        }
                        else if ((allowRanges & StyleApplyRanges.Cell) == StyleApplyRanges.Cell)
                        {
                            parameter.ApplyRange = StyleApplyRanges.Cell;
                            ReadCellBorderBackgroundFormatSettings(parameter, args);
                        }
                        else if ((allowRanges & StyleApplyRanges.Paragraph) == StyleApplyRanges.Paragraph)
                        {
                            parameter.ApplyRange = StyleApplyRanges.Paragraph;
                            ReadElementBorderBackgroundFormatSettings(
                                parameter,
                                args, 
                                args.Document.CurrentElement.OwnerParagraphEOF );
                        }
                    }
                    //parameter.Read(args.Document.Selection);
                }

                args.Result = false;
                if(parameter.ApplyRange == StyleApplyRanges.Text )
                {
                    SetElementsBorderBackgroundFormat(
                        parameter , 
                        args ,
                        args.Document.Selection.ContentElements );
                }
                else if( parameter.ApplyRange == StyleApplyRanges.Cell )
                {
                    SetCellBorderBackgroundFormat( parameter , args , false  );
                }
                else if( parameter.ApplyRange == StyleApplyRanges.Paragraph )
                {
                    SetElementsBorderBackgroundFormat(
                        parameter ,
                        args , 
                        args.Document.Selection.ParagraphsEOFs );
                }
                else 
                {
                    DomElement element = args.Document.CurrentElement ;
                    var elements = new DomElementList();
                    while(element != null )
                    {
                        if (args.DocumentControler.CanModify(element))
                        {
                            if (parameter.ApplyRange == StyleApplyRanges.Field && element is DomInputFieldElement)
                            {
                                elements.Add(element);
                                break;
                            }
                            if (parameter.ApplyRange == StyleApplyRanges.Row && element is DomTableRowElement)
                            {
                                elements.Add(element);
                                break;
                            }
                            if (parameter.ApplyRange == StyleApplyRanges.Table && element is DomTableElement)
                            {
                                elements.Add(element);
                                break;
                            }
                        }
                        element = element.Parent ;
                    }
                    if( elements.Count > 0 )
                    {
                        SetElementsBorderBackgroundFormat( parameter , args , elements );
                    }
                }
            }
        }

        private StyleApplyRanges GetAllowApplyRange(WriterCommandEventArgs args)
        {
            StyleApplyRanges result = StyleApplyRanges.None;
            if (args.EditorControl == null)
            {
                return result;
            }
            if (args.EditorControl != null && args.EditorControl.Readonly)
            {
                return result;
            }
            
            if ( args.Document.Selection.Mode == ContentRangeMode.Content 
                && args.Document.Selection.Length != 0 
                && args.DocumentControler.Snapshot.CanModifySelection )
            {
                result = result | StyleApplyRanges.Text | StyleApplyRanges.Paragraph ;
                return result;
            }
            if (args.DocumentControler.Snapshot.CanModifyParagraphs)
            {
                result = result | StyleApplyRanges.Paragraph;
            }
            if (args.Document.Selection.Mode == ContentRangeMode.Cell)
            {
                result = result | StyleApplyRanges.Cell;
            }
            DomElement element = args.Document.CurrentElement;
            while (element != null)
            {
                if (args.DocumentControler.CanModify(element))
                {
                    if (element is DomObjectElement)
                    {
                        result = result | StyleApplyRanges.Text;
                    }
                    else if (element is DomInputFieldElement)
                    {
                        result = result | StyleApplyRanges.Field;
                    }
                    else if (element is DomTableCellElement)
                    {
                        result = result | StyleApplyRanges.Cell | StyleApplyRanges.Row | StyleApplyRanges.Table;
                    }
                }
                element = element.Parent;
            }
            return result;
        }

        /// <summary>
        /// 获得最大和最小单元格行号和列号
        /// </summary>
        /// <param name="cells"></param>
        /// <param name="minRowIndex"></param>
        /// <param name="minColIndex"></param>
        /// <param name="maxRowIndex"></param>
        /// <param name="maxColIndex"></param>
        public static void GetCellIndexRange(
            DomElementList cells,
            ref int minRowIndex,
            ref int minColIndex,
            ref int maxRowIndex,
            ref int maxColIndex)
        {
            minRowIndex = 10000;
            minColIndex = 10000;
            maxRowIndex = 0;
            maxColIndex = 0;
            foreach (DomTableCellElement cell in cells)
            {
                if (cell.RowIndex < minRowIndex)
                {
                    minRowIndex = cell.RowIndex;
                }
                if (cell.ColIndex < minColIndex)
                {
                    minColIndex = cell.ColIndex;
                }
                if (cell.RowIndex + cell.RowSpan > maxRowIndex)
                {
                    maxRowIndex = cell.RowIndex + cell.RowSpan;
                }
                if (cell.ColIndex + cell.ColSpan > maxColIndex)
                {
                    maxColIndex = cell.ColIndex + cell.ColSpan;
                }
            }//foreach
        }

        /// <summary>
        /// 读取单元格的边框背景设置
        /// </summary>
        /// <param name="parameter">参数</param>
        /// <param name="args">参数</param>
        /// <param name="element">元素对象</param>
        /// <returns>操作是否成功</returns>
        private bool ReadElementBorderBackgroundFormatSettings(
            BorderBackgroundCommandParameter parameter,
            WriterCommandEventArgs args,
            DomElement element)
        {
            RuntimeDocumentContentStyle rs = element.RuntimeStyle;
            parameter.BorderLeftColor = rs.BorderLeftColor;
            parameter.BorderTopColor = rs.BorderTopColor;
            parameter.BorderRightColor = rs.BorderRightColor;
            parameter.BorderBottomColor = rs.BorderBottomColor;
            parameter.BorderWidth = rs.BorderWidth;
            parameter.BorderStyle = rs.BorderStyle;
            parameter.LeftBorder = rs.BorderLeft;
            parameter.TopBorder = rs.BorderTop;
            parameter.RightBorder = rs.BorderRight;
            parameter.BottomBorder = rs.BorderBottom;
            parameter.BackgroundColor = rs.BackgroundColor;
            parameter.MiddleHorizontalBorder = false;
            parameter.CenterVerticalBorder = false;
            parameter.ApplyRange = StyleApplyRanges.Text;
            parameter.SetBorderSettingsStyle();
            return true;
        }

        /// <summary>
        /// 设置多个文档元素的边框和背景样式
        /// </summary>
        /// <param name="parameter">参数</param>
        /// <param name="args">参数</param>
        /// <param name="elements">文档元素对象列表</param>
        private void SetElementsBorderBackgroundFormat(
            BorderBackgroundCommandParameter parameter,
            WriterCommandEventArgs args,
            DomElementList elements)
        {
            args.Document.BeginLogUndo();
            bool globalModified = false;
            var ctl = args.DocumentControler;
            try
            {
                ctl.BeginCacheValue();
                foreach (DomElement element in elements)
                {
                    if (ctl.CanModify(element) == false)
                    {
                        // 文档元素不能修改，忽略
                        continue;
                    }
                    DocumentContentStyle rs = (DocumentContentStyle)element.RuntimeStyle.CloneParent();
                    bool modified = parameter.SetContentStyle(rs);
                    if (modified)
                    {
                        globalModified = true;
                        int newStyleIndex = args.Document.ContentStyles.GetStyleIndex(rs);
                        if (newStyleIndex != element.StyleIndex)
                        {
                            if (args.Document.CanLogUndo)
                            {
                                args.Document.UndoList.AddStyleIndex(
                                    element,
                                    element.StyleIndex,
                                    newStyleIndex);
                            }
                            element.StyleIndex = newStyleIndex;
                            element.ContentElement.UpdateElementBorderInfos(true);
                            element.InvalidateView();
                            element.UpdateContentVersion();
                        }
                    }
                }//foreach
            }
            finally
            {
                ctl.EndCacheValue();
            }
            args.Document.EndLogUndo();
            if (globalModified)
            {
                args.Result = true;
                args.Document.Modified = true;
                //args.Document.UpdateContentVersion();
                args.Document.OnDocumentContentChanged();
            }
         }


        /// <summary>
        /// 读取单元格的边框背景设置
        /// </summary>
        /// <param name="parameter">参数</param>
        /// <param name="args">参数</param>
        /// <returns>操作是否成功</returns>
        private bool ReadCellBorderBackgroundFormatSettings(
            BorderBackgroundCommandParameter parameter,
            WriterCommandEventArgs args)
        {
            DCSelection selection = args.Document.Selection;
            DomElementList cells = new DomElementList();
            if (selection.Mode == ContentRangeMode.Cell)
            {
                cells = selection.Cells;
            }
            else
            {
                DomTableCellElement cell = args.Document.CurrentElement.OwnerCell;
                if (cell != null)
                {
                    cells.Add(cell);
                }
            }
            if (cells.Count > 0)
            {
                int minRowIndex = 0;
                int minColIndex = 0;
                int maxRowIndex = 0;
                int maxColIndex = 0;
                GetCellIndexRange(
                    cells,
                    ref minRowIndex,
                    ref minColIndex,
                    ref maxRowIndex,
                    ref maxColIndex);
                parameter.LeftBorder = true;
                parameter.TopBorder = true;
                parameter.RightBorder = true;
                parameter.BottomBorder = true;
                parameter.CenterVerticalBorder = (maxRowIndex - minRowIndex) > 1;
                parameter.MiddleHorizontalBorder = (maxColIndex - minColIndex) > 1;
                foreach (DomTableCellElement cell in cells)
                {
                    RuntimeDocumentContentStyle rs = cell.RuntimeStyle;
                    if (cell.RowIndex == minRowIndex)
                    {
                        if (rs.BorderTop == false)
                        {
                            parameter.TopBorder = false;
                        }
                    }
                    else if (cell.RowIndex < maxRowIndex)
                    {
                        if (rs.BorderTop == false)
                        {
                            parameter.MiddleHorizontalBorder = false;
                        }
                    }
                    if (cell.RowIndex + cell.RowSpan == maxRowIndex)
                    {
                        if (rs.BorderBottom == false)
                        {
                            parameter.BottomBorder = false;
                        }
                    }
                    if (cell.ColIndex == minColIndex)
                    {
                        if (rs.BorderLeft == false)
                        {
                            parameter.LeftBorder = false;
                        }
                    }
                    else if (cell.ColIndex < maxColIndex)
                    {
                        if (rs.BorderLeft == false)
                        {
                            parameter.CenterVerticalBorder = false;
                        }
                    }
                    if (cell.ColIndex + cell.ColSpan == maxColIndex)
                    {
                        if (rs.BorderRight == false)
                        {
                            parameter.RightBorder = false;
                        }
                    }
                }//foreach
                parameter.SetBorderSettingsStyle();

                RuntimeDocumentContentStyle rs2 = cells[0].RuntimeStyle;
                parameter.BorderLeftColor = rs2.BorderLeftColor;
                parameter.BorderTopColor = rs2.BorderTopColor;
                parameter.BorderRightColor = rs2.BorderRightColor;
                parameter.BorderBottomColor = rs2.BorderBottomColor;
                parameter.BorderStyle = rs2.BorderStyle;
                parameter.BorderWidth = rs2.BorderWidth;
                parameter.BackgroundColor = rs2.BackgroundColor;
                return true;
            }//if
            return false;
        }

        /// <summary>
        /// 设置表格单元格的边框和背景样式
        /// </summary>
        /// <param name="parameter">参数</param>
        /// <param name="args">参数</param>
        /// <param name="setBorderVisibleOnly">只设置边框线可见性</param>
        public static void SetCellBorderBackgroundFormat(
            BorderBackgroundCommandParameter parameter,
            WriterCommandEventArgs args,
            bool setBorderVisibleOnly )
        {
            

            //if (parameter.ForTable)
            {
                DomElementList cells = new DomElementList();
                if (parameter.Elements != null)
                {
                    // 获得用户指定的单元格对象
                    foreach (DomElement element in parameter.Elements)
                    {
                        if (element is DomTableCellElement)
                        {
                            cells.Add(element);
                        }
                    }
                }
                if (cells.Count == 0)
                {
                    if (args.Parameter is DomTableCellElement)
                    {
                        cells.Add((DomTableCellElement)args.Parameter);
                    }
                    else if (args.Parameter is DomElementList)
                    {
                        cells = (DomElementList)args.Parameter;
                    }
                    else
                    {
                        DCSelection selection = args.Document.Selection;
                        if (selection.Mode == ContentRangeMode.Cell)
                        {
                            cells = selection.Cells;
                        }
                        else if (args.Document.CurrentElement != null
                            && args.Document.CurrentElement.OwnerCell != null)
                        {
                            cells.Add(args.Document.CurrentElement.OwnerCell);
                        }
                    }
                }
                if (cells.Count == 0)
                {
                    return;
                }
                // 获得最大和最小单元格行号和列号
                int minRowIndex = 0;
                int minColIndex = 0;
                int maxRowIndex = 0;
                int maxColIndex = 0;
                if (parameter.ApplyRange == StyleApplyRanges.Cell )
                {
                    GetCellIndexRange(cells, ref minRowIndex, ref minColIndex, ref maxRowIndex, ref maxColIndex);
                }
                else
                {
                    DomTableElement table = ((DomTableCellElement)cells[0]).OwnerTable;
                    cells = table.Cells;
                    minRowIndex = 0;
                    minColIndex = 0;
                    maxRowIndex = table.Rows.Count;
                    maxColIndex = table.Columns.Count;
                }

                args.Document.BeginLogUndo();
                bool globalModified = false;
                var controler = (DocumentControler)args.DocumentControler;
                // 设置边框
                foreach (DomTableCellElement cell in cells)
                {
                    if (cell.IsOverrided)
                    {
                        // 被合并的单元格不处理
                        continue;
                    }
                    if (controler.CanModify(cell) == false)
                    {
                        // 元素属性不能改变
                        continue;
                    }
                    DocumentContentStyle rs = (DocumentContentStyle)cell.RuntimeStyle.CloneParent();
                    bool modified = false;
                    // 判断单元格是否要设置上边框线
                    bool showBorder = rs.BorderTop;
                    if (cell.RowIndex == minRowIndex)
                    {
                        // 最上面的单元格
                        showBorder = parameter.TopBorder;
                    }
                    else
                    {
                        //  水平中间的边框线
                        showBorder = parameter.MiddleHorizontalBorder;
                    }
                    if (showBorder != rs.BorderTop)
                    {
                        rs.BorderTop = showBorder;
                        modified = true;
                    }

                    // 判断是否要设置左边框线
                    showBorder = rs.BorderLeft;
                    if (cell.ColIndex == minColIndex)
                    {
                        // 最左边的单元格
                        showBorder = parameter.LeftBorder;
                    }
                    else
                    {
                        // 垂直中间的边框线
                        showBorder = parameter.CenterVerticalBorder;
                    }
                    if (showBorder != rs.BorderLeft)
                    {
                        rs.BorderLeft = showBorder;
                        modified = true;
                    }

                    // 判断是否要设置右边的边框线
                    showBorder = rs.BorderRight;
                    if (cell.ColIndex + cell.ColSpan == maxColIndex)
                    {
                        // 最右边的单元格
                        showBorder = parameter.RightBorder;
                    }
                    else
                    {
                        // 垂直中间的边框线
                        showBorder = parameter.CenterVerticalBorder;
                    }
                    if (showBorder != rs.BorderRight)
                    {
                        rs.BorderRight = showBorder;
                        modified = true;
                    }

                    // 判断是否要显示下边框线
                    showBorder = rs.BorderBottom;
                    if (cell.RowIndex + cell.RowSpan == maxRowIndex)
                    {
                        // 最下面的单元格
                        showBorder = parameter.BottomBorder;
                    }
                    else
                    {
                        // 水平中间的边框线
                        showBorder = parameter.MiddleHorizontalBorder;
                    }
                    if (showBorder != rs.BorderBottom)
                    {
                        rs.BorderBottom = showBorder;
                        modified = true;
                    }

                    if (setBorderVisibleOnly == false)
                    {
                        if (rs.BorderLeftColor.ToArgb() != parameter.BorderLeftColor.ToArgb())
                        {
                            rs.BorderLeftColor = parameter.BorderLeftColor;
                            modified = true;
                        }

                        if (rs.BorderTopColor.ToArgb() != parameter.BorderTopColor.ToArgb())
                        {
                            rs.BorderTopColor = parameter.BorderTopColor;
                            modified = true;
                        }

                        if (rs.BorderRightColor.ToArgb() != parameter.BorderRightColor.ToArgb())
                        {
                            rs.BorderRightColor = parameter.BorderRightColor;
                            modified = true;
                        }

                        if (rs.BorderBottomColor.ToArgb() != parameter.BorderBottomColor.ToArgb())
                        {
                            rs.BorderBottomColor = parameter.BorderBottomColor;
                            modified = true;
                        }

                        if (rs.BorderStyle != parameter.BorderStyle)
                        {
                            rs.BorderStyle = parameter.BorderStyle;
                            modified = true;
                        }

                        if (rs.BorderWidth != parameter.BorderWidth)
                        {
                            rs.BorderWidth = parameter.BorderWidth;
                            modified = true;
                        }
                        if (rs.BackgroundColor.IsEmpty == false)
                        {
                            if (rs.BackgroundColor.ToArgb() != parameter.BackgroundColor.ToArgb())
                            {
                                rs.BackgroundColor = parameter.BackgroundColor;
                                modified = true;
                            }
                        }
                    }//if
                    if (modified)
                    {
                        globalModified = true;
                        int newStyleIndex = args.Document.ContentStyles.GetStyleIndex(rs);
                        if (newStyleIndex != cell.StyleIndex)
                        {
                            if (args.Document.CanLogUndo)
                            {
                                args.Document.UndoList.AddStyleIndex(cell, cell.StyleIndex, newStyleIndex);
                            }
                            cell.StyleIndex = newStyleIndex;
                            cell.InvalidateView();
                            cell.UpdateContentVersion();
                            // 触发单元格内容发生改变事件
                            ContentChangedEventArgs cde = new ContentChangedEventArgs();
                            cde.Document = args.Document ;
                            cde.Element = cell;
                            cell.RaiseBubbleOnContentChanged(cde);
                        }
                    }
                }//foreach
                args.Document.EndLogUndo();
                if (globalModified)
                {
                    args.Document.Modified = true;
                    //args.Document.UpdateContentVersion();
                    args.Document.OnDocumentContentChanged();
                    args.Result = true;
                }
            }//if
        }

        /// <summary>
        /// 设置背景颜色
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        [WriterCommandDescription(
            StandardCommandNames.BackColor,
            // ImageResource = "DCSoft.Writer.Commands.Images.CommandBackColor.bmp",
            ReturnValueType=typeof( bool ) )]
        private void BackColor(object sender, WriterCommandEventArgs args)
        {
            CommonColorFunction(sender, args, StandardCommandNames.BackColor);
        }

        /// <summary>
        /// 设置文本颜色
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        [WriterCommandDescription(
            StandardCommandNames.Color,
            // ImageResource = "DCSoft.Writer.Commands.Images.CommandColor.bmp",
            ReturnValueType = typeof(bool))]
        private void ColorFunction(object sender, WriterCommandEventArgs args)
        {
            CommonColorFunction(sender, args , StandardCommandNames.Color );
        }
        private void CommonColorFunction(object sender, WriterCommandEventArgs args, string commandName)
        {
            if (args.Mode == WriterCommandEventMode.QueryState)
            {
                args.Enabled = args.DocumentControler != null
                     && args.DocumentControler.Snapshot.CanModifySelection;
            }
            else if (args.Mode == WriterCommandEventMode.Invoke)
            {
                args.Result = false;
                DocumentContentStyle cs = GetCurrentStyle(args.Document);
                DCSystem_Drawing.Color color = cs.Color;
                if (commandName == StandardCommandNames.BackColor)
                {
                    color = cs.BackgroundColor;
                    if (args.Parameter != null)
                    {
                        color = ConvertToColor(args.Parameter, DCSystem_Drawing.Color.Transparent);
                    }
                }
                else
                {
                    if (args.Parameter != null)
                    {
                        color = ConvertToColor(args.Parameter, DCSystem_Drawing.Color.Black);
                    }
                }
                if (args.ShowUI)
                {
                    DCSystem_Drawing.Color defaultColor = DCSystem_Drawing.Color.Black;
                    if (commandName == StandardCommandNames.Color)
                    {
                        defaultColor = DCSystem_Drawing.Color.Black;
                    }
                    else if (commandName == StandardCommandNames.BackColor)
                    {
                        defaultColor = DCSystem_Drawing.Color.Transparent;
                    }
                    args.EditorControl.WASMParent.JSShowColorControl(
                        defaultColor,
                        new Action<DCSystem_Drawing.Color>(delegate (DCSystem_Drawing.Color selectedColor)
                    {
                        if (selectedColor != defaultColor)
                        {
                            args.Parameter = selectedColor;
                            SetStyleProperty(sender, args, commandName);
                        }
                    }));
                }
                else
                {
                    args.Parameter = color;
                    SetStyleProperty(sender, args, commandName);
                }

            }
        }
        private DCSystem_Drawing.Color ConvertToColor(object parameter, DCSystem_Drawing.Color defaultValue)
        {
            DCSystem_Drawing.Color result = defaultValue;
            if (parameter is DCSystem_Drawing.Color)
            {
                result = (DCSystem_Drawing.Color)parameter;
            }
            else if (parameter is int)
            {
                int v = (int)parameter;
                result = DCSystem_Drawing.Color.FromArgb(v);
            }
            else if (parameter is string)
            {
                try
                {
                    string txt = (string)parameter;
                    result = DCSoft.Common.XMLSerializeHelper.StringToColor(txt, DCSystem_Drawing.Color.Empty); //(Color)TypeDescriptor.GetConverter(typeof(Color)).ConvertFromString(txt);
                }
                catch (Exception ext)
                {
                    DCConsole.Default.WriteLineError( ext.Message);
                }
            }
            return result;
        }
        /// <summary>
        /// 设置字体名称
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        [WriterCommandDescription(
            StandardCommandNames.FontName,
            ReturnValueType = typeof(bool))]
        private void FontName(object sender, WriterCommandEventArgs args)
        {
            args.EnableSetUITextAsParamter = true;
            if (args.Mode == WriterCommandEventMode.InitalizeUIElement)
            {
            }
            else if (args.Mode == WriterCommandEventMode.QueryState)
            {
                if (args.Document == null)
                {
                    args.Enabled = false;
                    return;
                }
                args.Enabled = args.DocumentControler != null
                             && args.DocumentControler.Snapshot.CanModifySelection;
                if (args.Parameter == null)
                {
                    // 没有指定用户参数
                    var fn = args.Document.CurrentStyleInfo.Content.FontName;
                    if(fn == null || fn.Length == 0 )
                    {
                        fn = args.Document.DefaultFont.Name;
                    }
                    args.Parameter =  fn ;
                }
                args.SetParameterToUIText = true;
            }
            else if (args.Mode == WriterCommandEventMode.Invoke)
            {
                SetStyleProperty(sender, args, StandardCommandNames.FontName);
                args.RefreshLevel = UIStateRefreshLevel.Current;
                if (args.EditorControl != null)
                {
                    args.EditorControl.Focus();
                }
            }
        }

        /// <summary>
        /// 设置字体大小
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        [WriterCommandDescription(
            StandardCommandNames.FontSize,
            ReturnValueType = typeof(bool))]
        private void FontSize(object sender, WriterCommandEventArgs args)
        {
            args.EnableSetUITextAsParamter = true;
            if (args.Mode == WriterCommandEventMode.InitalizeUIElement)
            {
            }
            else if (args.Mode == WriterCommandEventMode.QueryState)
            {
                if (args.Document == null)
                {
                    args.Enabled = false;
                    return;
                }
                args.Enabled = args.DocumentControler != null
                            && args.DocumentControler.Snapshot.CanModifySelection;
                if (args.Parameter == null)
                {
                    var size = GetCurrentStyle(args.Document).FontSize;
                    if(size <= 0 )
                    {
                        size = args.Document.DefaultFont.Size;
                    }
                    args.Parameter = FontSizeInfo.GetStandSizeName(
                        size, 
                        args.EditorControl.DocumentOptions.BehaviorOptions.EnableChineseFontSizeName);
                }
                args.SetParameterToUIText = true;
            }
            else if (args.Mode == WriterCommandEventMode.Invoke)
            {
                SetStyleProperty(sender, args, StandardCommandNames.FontSize);
                if (args.EditorControl != null)
                {
                    args.EditorControl.Focus();
                }
            }
        }

        /// <summary>
        /// 设置字体样式
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        [WriterCommandDescription(
            StandardCommandNames.Font,
            // ImageResource = "DCSoft.Writer.Commands.Images.CommandFont.bmp",
            ReturnValueType = typeof(bool))]
        private void Font(object sender, WriterCommandEventArgs args)
        {
            if (args.Mode == WriterCommandEventMode.QueryState)
            {
                args.Enabled = args.DocumentControler != null
                                 && args.DocumentControler.Snapshot.CanModifySelection;
            }
            else if (args.Mode == WriterCommandEventMode.Invoke)
            {
                DocumentContentStyle cs = GetCurrentStyle(args.Document);
                XFontValue font = new XFontValue();
                if (args.Parameter == null)
                {
                    font = cs.Font;
                }
                else if (args.Parameter is string)
                {
                    font = cs.Font.Clone();
                    font.Parse((string)args.Parameter);
                }
                else if (args.Parameter is XFontValue)
                {
                    font = ((XFontValue)args.Parameter).Clone();
                    //MessageBox.Show("2:" + font.Size);
                }
                else if (args.Parameter is  Font)
                {
                    font = new XFontValue(( Font)args.Parameter);
                    //MessageBox.Show("3:" + font.Size);
                }
                else
                {
                    // 未知参数
                    font = cs.Font;
                }
                args.Parameter = font;
                SetStyleProperty(sender, args, StandardCommandNames.Font);
            }
        }

        /// <summary>
        /// 设置下划线样式
        /// </summary>
        /// <param name="sender">参数</param>
        /// <param name="args">参数</param>
        [WriterCommandDescription(
            StandardCommandNames.Underline,
            ShortcutKey = Keys.Control | Keys.U,
            // ImageResource = "DCSoft.Writer.Commands.Images.CommandUnderline.bmp",
            ReturnValueType = typeof(bool) )]
        private void Underline(object sender, WriterCommandEventArgs args)
        {
            SetStyleProperty(sender, args, StandardCommandNames.Underline);
        }


        /// <summary>
        /// 设置斜体模式
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        [WriterCommandDescription(
            StandardCommandNames.Italic,
            ShortcutKey = Keys.Control | Keys.I,
            // ImageResource = "DCSoft.Writer.Commands.Images.CommandItalic.bmp",
            ReturnValueType = typeof(bool))]
        private void Italic(object sender, WriterCommandEventArgs args)
        {
            SetStyleProperty(sender, args, StandardCommandNames.Italic);
        }

        /// <summary>
        /// 设置或取消粗体样式
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        [WriterCommandDescription(
            StandardCommandNames.Bold,
            ShortcutKey = Keys.Control | Keys.B,
            // ImageResource = "DCSoft.Writer.Commands.Images.CommandBold.bmp",
            ReturnValueType = typeof(bool))]
        private void Bold(object sender, WriterCommandEventArgs args)
        {
            SetStyleProperty(sender, args, StandardCommandNames.Bold);
        }

        /// <summary>
        /// 段落左对齐
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        [WriterCommandDescription(
            StandardCommandNames.AlignLeft,
            // ImageResource = "DCSoft.Writer.Commands.Images.CommandAlignLeft.bmp",
            ReturnValueType = typeof(bool))]
        private void AlignLeft(object sender, WriterCommandEventArgs args)
        {
            SetParagraphStyleProperty(sender, args, StandardCommandNames.AlignLeft);
        }

        /// <summary>
        /// 段落左对齐
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        [WriterCommandDescription(
            StandardCommandNames.AlignCenter,
            // ImageResource = "DCSoft.Writer.Commands.Images.CommandAlignCenter.bmp",
            ReturnValueType=typeof( bool ))]
        private void AlignCenter(object sender, WriterCommandEventArgs args)
        {
            SetParagraphStyleProperty(sender, args, StandardCommandNames.AlignCenter);
        }


        /// <summary>
        /// 段落左对齐
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        [WriterCommandDescription(
            StandardCommandNames.AlignDistribute,
            // ImageResource = "DCSoft.Writer.Commands.Images.CommandAlignDistribute.bmp",
            ReturnValueType = typeof(bool))]
        private void AlignDistribute(object sender, WriterCommandEventArgs args)
        {
            SetParagraphStyleProperty(sender, args, StandardCommandNames.AlignDistribute);
        }

        /// <summary>
        /// 段落左对齐
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        [WriterCommandDescription(
            StandardCommandNames.AlignRight,
            // ImageResource = "DCSoft.Writer.Commands.Images.CommandAlignRight.bmp",
            ReturnValueType = typeof(bool))]
        private void AlignRight(object sender, WriterCommandEventArgs args)
        {
            SetParagraphStyleProperty(sender, args, StandardCommandNames.AlignRight);
        }

        /// <summary>
        /// 段落左对齐
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        [WriterCommandDescription(
            StandardCommandNames.AlignJustify,
            // ImageResource = "DCSoft.Writer.Commands.Images.CommandAlignJustify.bmp",
            ReturnValueType = typeof(bool))]
        private void AlignJustify(object sender, WriterCommandEventArgs args)
        {
            SetParagraphStyleProperty(sender, args, StandardCommandNames.AlignJustify);
        }

        /// <summary>
        /// 设置段落的左边框
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        [WriterCommandDescription(
            StandardCommandNames.BorderLeft,
            // ImageResource = "DCSoft.Writer.Commands.Images.CommandBorderLeft.bmp",
            ReturnValueType = typeof(bool))]
        private void BorderLeft(object sender, WriterCommandEventArgs args)
        {
            SetParagraphStyleProperty(sender, args, StandardCommandNames.BorderLeft);
        }

        /// <summary>
        /// 段落左对齐
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        [WriterCommandDescription(
            StandardCommandNames.BorderTop,
            // ImageResource = "DCSoft.Writer.Commands.Images.CommandBorderTop.bmp",
            ReturnValueType = typeof(bool))]
        private void BorderTop(object sender, WriterCommandEventArgs args)
        {
            SetParagraphStyleProperty(sender, args, StandardCommandNames.BorderTop);
        }

        /// <summary>
        /// 段落左对齐
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        [WriterCommandDescription(
            StandardCommandNames.BorderRight,
            // ImageResource = "DCSoft.Writer.Commands.Images.CommandBorderRight.bmp",
            ReturnValueType = typeof(bool))]
        private void BorderRight(object sender, WriterCommandEventArgs args)
        {
            SetParagraphStyleProperty(sender, args, StandardCommandNames.BorderRight);
        }

        /// <summary>
        /// 段落左对齐
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        [WriterCommandDescription(
            StandardCommandNames.BorderBottom,
            // ImageResource = "DCSoft.Writer.Commands.Images.CommandBorderBottom.bmp",
            ReturnValueType = typeof(bool))]
        private void BorderBottom(object sender, WriterCommandEventArgs args)
        {
            SetParagraphStyleProperty(sender, args, StandardCommandNames.BorderBottom);
        }

        /// <summary>
        /// 设置/取消删除线样式
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        [WriterCommandDescription(
            StandardCommandNames.Strikeout,
            ReturnValueType = typeof(bool))]
        private void Strikeout(object sender, WriterCommandEventArgs args)
        {
            SetStyleProperty(sender, args, StandardCommandNames.Strikeout);
        }

        /// <summary>
        /// 设置内边距
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        [WriterCommandDescription(
            StandardCommandNames.Padding,
            // ImageResource = "DCSoft.Writer.Commands.Images.CommandPadding.bmp",
            ReturnValueType = typeof(bool))]
        private void Padding(object sender, WriterCommandEventArgs args)
        {
            if (args.Mode == WriterCommandEventMode.QueryState)
            {
                DomElementList list = GetPaddingableElement(args.Document);
                args.Enabled = list != null && list.Count > 0;
            }
            else if (args.Mode == WriterCommandEventMode.Invoke)
            {
                args.Result = false;
                DomElementList elements = GetPaddingableElement(args.Document);
                if (elements == null || elements.Count == 0)
                {
                    return;
                }
                bool setValue = false;
                XPaddingF padding = null;
                if (args.Parameter is XPaddingF)
                {
                    padding = (XPaddingF)args.Parameter;
                    setValue = true;
                }
                else if (args.Parameter is string)
                {
                    padding = new XPaddingF();
                    if (XPaddingF.Parse((string)args.Parameter, padding))
                    {
                        setValue = true;
                    }
                }
                if (setValue == false)
                {
                    DocumentContentStyle rs = elements[0].RuntimeStyle.Parent;
                    padding = new XPaddingF(
                        rs.PaddingLeft,
                        rs.PaddingTop,
                        rs.PaddingRight,
                        rs.PaddingBottom);
                }
                if (setValue && padding != null)
                {
                    DocumentContentStyle ns = new DocumentContentStyle();
                    ns.DisableDefaultValue = true;
                    ns.PaddingLeft = padding.Left;
                    ns.PaddingTop = padding.Top;
                    ns.PaddingRight = padding.Right;
                    ns.PaddingBottom = padding.Bottom;
                    args.Document.BeginLogUndo();
                    args.Result = DCSelection.SetElementStyle(
                        ns,
                        ns,
                        ns,
                        args.Document,
                        elements , 
                        true ,
                        args.Name ,
                        true );
                    args.Document.EndLogUndo();
                    args.Document.OnSelectionChanged();
                    args.Document.OnDocumentContentChanged();
                }
                else
                {
                    // 用户没有改变设置，不进行操作
                    return;
                }
            }
        }

        /// <summary>
        /// 获得文档中可以设置内边距的元素对象
        /// </summary>
        /// <param name="document">文档对象</param>
        /// <returns>元素对象列表</returns>
        private DomElementList GetPaddingableElement(DomDocument document)
        {
            if (document == null)
            {
                return null;
            }
            if (document.Selection != null && document.Selection.Length != 0)
            {
                DomElementList result = new DomElementList();
                var ctroler = document.DocumentControler;
                ctroler.BeginCacheValue();
                try
                {
                    foreach (DomElement element in document.Selection.ContentElements)
                    {
                        if (element is DomObjectElement)
                        {
                            if (ctroler.CanModify(element))
                            {
                                result.FastAdd2(element);
                            }
                        }
                    }
                    if (document.Selection.Cells != null)
                    {
                        foreach (DomTableCellElement cell in document.Selection.Cells)
                        {
                            if (cell.IsOverrided == false && ctroler.CanModify(cell))
                            {
                                result.FastAdd2(cell);
                            }
                        }
                    }
                }
                finally
                {
                    ctroler.EndCacheValue();
                }
                return result;
            }
            else
            {
                DomContainerElement c = null;
                int index = 0;
                document.Content.GetCurrentPositionInfo(out c, out index);
                while (c != null)
                {
                    if (c is DomTableCellElement)
                    {
                        DomElementList result = new DomElementList();
                        result.FastAdd2(c);
                        return result;
                    }
                    c = c.Parent;
                }
            }
            return null;
        }

        /// <summary>
        /// 设置段落的数字列表样式
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        [WriterCommandDescription(StandardCommandNames.NumberedList,
            // ImageResource = "DCSoft.Writer.Commands.Images.CommandNumberedList.bmp",
            ReturnValueType = typeof(bool))]
        private void NumberedList(object sender, WriterCommandEventArgs args)
        {
            SetParagraphStyleProperty(sender, args, StandardCommandNames.NumberedList);
        }

        /// <summary>
        /// 设置段落的原点列表样式
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        [WriterCommandDescription(StandardCommandNames.BulletedList,
            // ImageResource = "DCSoft.Writer.Commands.Images.CommandBulletedList.bmp",
            ReturnValueType = typeof(bool))]
        private void BulletedList(object sender, WriterCommandEventArgs args)
        {
            SetParagraphStyleProperty(sender, args, StandardCommandNames.BulletedList);
        }
        /// <summary>
        /// 设置段落的数字列表样式
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        [WriterCommandDescription(StandardCommandNames.ParagraphListStyle,
            ReturnValueType = typeof(bool))]
        private void ParagraphListStyle(object sender, WriterCommandEventArgs args)
        {
            SetParagraphStyleProperty(sender, args, StandardCommandNames.ParagraphListStyle);
        }

        /// <summary>
        /// 特殊的段落首行缩进
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        [WriterCommandDescription(StandardCommandNames.IncreaseFirstLineIndent,
            // ImageResource = "DCSoft.Writer.Commands.Images.CommandFirstLineIndent.bmp",
            ReturnValueType = typeof(bool))]
        private void IncreaseFirstLineIndent(object sender, WriterCommandEventArgs args)
        {
             SetParagraphStyleProperty(sender, args, StandardCommandNames.IncreaseFirstLineIndent);
        }

        /// <summary>
        /// 段落首行缩进
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        [WriterCommandDescription(StandardCommandNames.FirstLineIndent,
            // ImageResource = "DCSoft.Writer.Commands.Images.CommandFirstLineIndent.bmp",
            ReturnValueType = typeof(bool))]
        private void FirstLineIndent(object sender, WriterCommandEventArgs args)
        {
            SetParagraphStyleProperty(sender, args, StandardCommandNames.FirstLineIndent);
        }


        /// <summary>
        /// 下标样式
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        [WriterCommandDescription(StandardCommandNames.Subscript,
            // ImageResource = "DCSoft.Writer.Commands.Images.CommandSubscript.bmp",
            ReturnValueType=typeof( bool ))]
        private void Subscript(object sender, WriterCommandEventArgs args)
        {
            SetStyleProperty(sender, args, StandardCommandNames.Subscript);
        }

        /// <summary>
        ///上标样式
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        [WriterCommandDescription(StandardCommandNames.Superscript,
            // ImageResource = "DCSoft.Writer.Commands.Images.CommandSuperscript.bmp",
            ReturnValueType = typeof(bool) )]
        private void Superscript(object sender, WriterCommandEventArgs args)
        {
            SetStyleProperty(sender, args, StandardCommandNames.Superscript);
        }

        internal static DocumentContentStyle GetCurrentStyle(DomDocument document)
        {
            if (document == null)
            {
                return null;
            }
            return document.CurrentStyleInfo.Content;
            //return document.EditorCurrentStyle;

            //DocumentContentStyle style = document.CurrentStyle;
            //if (document.Selection.Length != 0)
            //{
            //    document._UserSpecifyStyle = null;
            //}
            //if (document._UserSpecifyStyle != null)
            //{
            //    // 设置为用户指定的样式
            //    style = document._UserSpecifyStyle;
            //}
            //return style;
        }


        private void SetStyleProperty(
            object sender,
            WriterCommandEventArgs args,
            string commandName)
        {
            if (args.Mode == WriterCommandEventMode.QueryState)
            {
                args.Enabled = false;
                if (args.EditorControl == null)
                {
                    return;
                }
                
                DocumentContentStyle style = GetCurrentStyle(args.Document);
                args.Enabled = args.EditorControl != null 
                    && args.DocumentControler != null
                    && args.DocumentControler.Snapshot.CanModifySelection;

                if (args.Enabled == false)
                {
                    return;
                }
                switch (commandName)
                {
                    case StandardCommandNames.Bold:
                        args.Checked = style.Bold;
                        break;
                    case StandardCommandNames.BorderBottom:
                        args.Checked = style.BorderBottom;
                        break;
                    case StandardCommandNames.BorderLeft:
                        args.Checked = style.BorderLeft;
                        break;
                    case StandardCommandNames.BorderRight:
                        args.Checked = style.BorderRight;
                        break;
                    case StandardCommandNames.BorderTop:
                        args.Checked = style.BorderTop;
                        break;
                    case StandardCommandNames.Italic:
                        args.Checked = style.Italic;
                        break;
                    case StandardCommandNames.Strikeout:
                        args.Checked = style.Strikeout;
                        break;
                    case StandardCommandNames.Subscript:
                        args.Checked = style.Subscript;
                        break;
                    case StandardCommandNames.Superscript:
                        args.Checked = style.Superscript;
                        break;
                    case StandardCommandNames.Underline:
                        args.Checked = style.Underline;
                        break;
                    default:
                        args.Enabled = false;
                        return;
                }
            }
            else if (args.Mode == WriterCommandEventMode.Invoke)
            {
                DCSelection selection = args.Document.Selection;
                args.Result = false;
                DocumentContentStyle cs = GetCurrentStyle(args.Document);
                DocumentContentStyle ns = args.Document.CreateDocumentContentStyle();
                ns.DisableDefaultValue = true;
                bool causeUpdateLayout = true;
                bool includeCells = false;
                switch (commandName)
                {
                    case StandardCommandNames.Bold:
                        ns.Bold = WriterUtilsInner.GetArgumentBooleanValue(
                            args.Parameter,
                            !cs.Bold);
                        causeUpdateLayout = true;
                        break;
                    case StandardCommandNames.BorderBottom:
                        ns.BorderBottom = WriterUtilsInner.GetArgumentBooleanValue(
                            args.Parameter,
                            !cs.BorderBottom);
                        causeUpdateLayout = false;
                        break;
                    case StandardCommandNames.BorderLeft:
                        ns.BorderLeft = WriterUtilsInner.GetArgumentBooleanValue(
                            args.Parameter,
                            !cs.BorderLeft);
                        causeUpdateLayout = false;
                        break;
                    case StandardCommandNames.BorderRight:
                        ns.BorderRight = WriterUtilsInner.GetArgumentBooleanValue(
                            args.Parameter,
                            !cs.BorderRight);
                        break;
                    case StandardCommandNames.BorderTop:
                        ns.BorderTop = WriterUtilsInner.GetArgumentBooleanValue(
                            args.Parameter,
                            !cs.BorderTop);
                        causeUpdateLayout = false;
                        break;
                    case StandardCommandNames.Italic:
                        ns.Italic = WriterUtilsInner.GetArgumentBooleanValue(
                            args.Parameter,
                            !cs.Italic);
                        causeUpdateLayout = true;
                        break;
                    case StandardCommandNames.Strikeout:
                        ns.Strikeout = WriterUtilsInner.GetArgumentBooleanValue(
                            args.Parameter,
                            !cs.Strikeout);
                        causeUpdateLayout = false;
                        break;
                    case StandardCommandNames.Subscript:
                        ns.Subscript = WriterUtilsInner.GetArgumentBooleanValue(
                            args.Parameter,
                            !cs.Subscript);
                        ns.Superscript = false;
                        causeUpdateLayout = true;
                        break;
                    case StandardCommandNames.Superscript:
                        ns.Subscript = false;
                        ns.Superscript = WriterUtilsInner.GetArgumentBooleanValue(
                            args.Parameter,
                            !cs.Superscript);
                        causeUpdateLayout = true;
                        break;
                    case StandardCommandNames.Underline:
                        ns.Underline = WriterUtilsInner.GetArgumentBooleanValue(
                            args.Parameter,
                            !cs.Underline);
                        causeUpdateLayout = false;
                        break;
                    case StandardCommandNames.Color:
                        // 设置文本颜色
                        ns.Color = ConvertToColor(args.Parameter, cs.Color);
                        causeUpdateLayout = false;
                        break;
                    case StandardCommandNames.BackColor:
                        ns.BackgroundColor = ConvertToColor(args.Parameter, cs.BackgroundColor);
                        causeUpdateLayout = false;
                        break;
                    case StandardCommandNames.Font:
                        if (args.Parameter is Font)
                        {
                            ns.Font = new XFontValue((Font)args.Parameter);
                        }
                        else if (args.Parameter is XFontValue)
                        {
                            ns.Font = ((XFontValue)args.Parameter).Clone();
                        }
                        causeUpdateLayout = true;
                        break;
                    case StandardCommandNames.FontName:
                        if (args.Parameter is string)
                        {
                            ns.FontName = (string)args.Parameter;
                            args.Document.CurrentStyleInfo.Content.FontName = ns.FontName;
                            //if (args.EditorControl != null)
                            //{
                            //    args.EditorControl.Focus();
                            //}
                        }
                        causeUpdateLayout = true;
                        break;
                    case StandardCommandNames.FontSize:
                        if (args.Parameter is string)
                        {
                            ns.FontSize = FontSizeInfo.GetFontSize(
                                (string)args.Parameter, 
                                args.Document.DefaultStyle.FontSize);
                        }
                        else if (args.Parameter is float
                            || args.Parameter is double
                            || args.Parameter is int)
                        {
                            ns.FontSize = Convert.ToSingle(args.Parameter);
                        }
                        args.Document.CurrentStyleInfo.Content.FontSize = ns.FontSize;
                        causeUpdateLayout = true;
                        break;
                    default:
                        {
                            throw new NotSupportedException(commandName);
                        }
                }//switch
                XDependencyObject.MergeValues(ns, args.Document.CurrentStyleInfo.Content , true);
                //if (commandName == StandardCommandNames.Bold
                //    || commandName == StandardCommandNames.Italic
                //    || commandName == StandardCommandNames.Underline
                //    || commandName == StandardCommandNames.Superscript
                //    || commandName == StandardCommandNames.Subscript
                //    || commandName == StandardCommandNames.Font
                //    || commandName == StandardCommandNames.FontName
                //    || commandName == StandardCommandNames.FontSize
                //    || commandName == StandardCommandNames.Color
                //    || commandName == StandardCommandNames.BackColor
                //    || commandName == StandardCommandNames.Strikeout
                //    || commandName == StandardCommandNames.PrintColor 
                //    || commandName == StandardCommandNames.PrintBackColor 
                //    || commandName == StandardCommandNames.ContentProtect
                //    || commandName == StandardCommandNames.Visibility
                //    || commandName == StandardCommandNames.TitleLevel
                //    || commandName == StandardCommandNames.TextSurroundings
                //    || commandName == StandardCommandNames.EmbedInText
                //    || commandName == StandardCommandNames.CharacterCircle
                //    || commandName == StandardCommandNames)
                {
                    XDependencyObject.MergeValues(
                        ns, 
                        args.Document.CurrentStyleInfo.ContentStyleForNewString, 
                        true);
                    //ns.DefaultValuePropertyNames = null ;// = args.Document.CurrentStyleInfo.ContentStyleForNewString.DefaultValuePropertyNames;
                }
                if (selection.Length == 0)
                {
                    // 没有选取内容
                    // 若光标在一个空白的输入域中，则设置这个输入域
                    DomContainerElement c = null;
                    int index = 0;
                    args.Document.Content.GetCurrentPositionInfo(out c, out index);
                    if (c is DomInputFieldElement)
                    {
                        if (c.Elements.Count == 0 && index == 0 )
                        {
                            selection = new DCSelection(selection.DocumentContent);
                            selection.ContentElements.Add(( (DomInputFieldElement ) c).StartElement );
                        }
                    }
                }
                //if (selection.Length != 0)
                {

                    args.Document.BeginLogUndo();
                    bool bolResult = selection.SetElementStyle(
                        ns , 
                        causeUpdateLayout ,
                        includeCells ,
                        args.Name );
                    args.Result = bolResult;
                    if (bolResult)
                    {
                        //解决DCWRITER-3705 
                        args.RefreshLevel = UIStateRefreshLevel.Current;
                        //args.Document.EditorRefreshView();

                        args.Document.EndLogUndo();
                        args.Document.OnSelectionChanged();
                        args.Document.OnDocumentContentChanged();
                    }
                    //if (args.Name == StandardCommandNames.TitleLevel)
                    //{
                    //    if (args.EditorControl != null)
                    //    {
                    //        // 触发文档导航数据发生改变事件
                    //        args.EditorControl.OnDocumentNavigateContentChanged(EventArgs.Empty);
                    //    }
                    //}
                }

                //args.Document.CurrentStyle.Underline = v;
            }
        }


        /// <summary>
        /// 根据段落列表样式设置段落的缩进量
        /// </summary>
        /// <param name="ns"></param>
        /// <param name="document"></param>
        private static void SetParagraphIndentByListStyle(DocumentContentStyle ns, DomDocument document )
        {
            if (ns.ParagraphListStyle == DCSoft.Drawing.ParagraphListStyle.None)
            {
                ns.FirstLineIndent = 0;
                ns.LeftIndent = 0;
            }
            else
            {
                DocumentContentStyle oldStyle = document.CurrentStyleInfo.Paragraph;
                int maxListIndex = 1;
                //float itemWidth = 0;
                foreach (DomParagraphFlagElement p in document.Selection.ParagraphsEOFs)
                {
                    maxListIndex = Math.Max(maxListIndex, p.MaxListIndex);
                }
                using (DCGraphics g = document.InnerCreateDCGraphics())
                {
                    DocumentContentStyle rs = (DocumentContentStyle)oldStyle.Clone();
                    rs.ParagraphListStyle = ns.ParagraphListStyle;
                    DCSystem_Drawing.SizeF size = DomParagraphListItemElement.MeasureSize(rs, g, maxListIndex);
                    if (size.Width > 0)
                    {
                        if (oldStyle.FirstLineIndent < 0)
                        {
                            ns.LeftIndent = oldStyle.LeftIndent + oldStyle.FirstLineIndent;
                        }
                        else
                        {
                            ns.LeftIndent = oldStyle.LeftIndent;// +size.Width;
                        }
                        ns.LeftIndent = ns.LeftIndent + size.Width;
                        ns.FirstLineIndent = -size.Width;
                    }
                }
            }
        }
        /// <summary>
        /// 带参数的执行标题样式命令
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        [WriterCommandDescription(StandardCommandNames.HeaderFormat)]
        private void HeaderFormat(object sender, WriterCommandEventArgs args)
        {
            if (args.Mode == WriterCommandEventMode.QueryState)
            {
                args.Enabled = true;
            }
            else
            {
                HeaderFormatCommandParameter p = args.Parameter as HeaderFormatCommandParameter;
                if (p != null)
                {
                    SetHeaderParagraphFormat(args, p);
                }
            }
        }

        /// <summary>
        /// 设置标准的标题1样式
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        [WriterCommandDescription( StandardCommandNames.Header1 )]
        private void Header1(object sender, WriterCommandEventArgs args)
        {
            HeaderFormatCommandParameter p = HeaderFormatCommandParameter.CreateHeader1();
            SetHeaderParagraphFormat(args , p );
        }
        /// <summary>
        /// 设置标准的标题1样式
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        [WriterCommandDescription(StandardCommandNames.Header2)]
        private void Header2(object sender, WriterCommandEventArgs args)
        {
            HeaderFormatCommandParameter p = HeaderFormatCommandParameter.CreateHeader2();
            SetHeaderParagraphFormat(args, p);
        }
        /// <summary>
        /// 设置标准的标题1样式
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        [WriterCommandDescription(StandardCommandNames.Header3)]
        private void Header3(object sender, WriterCommandEventArgs args)
        {
            HeaderFormatCommandParameter p = HeaderFormatCommandParameter.CreateHeader3();
            SetHeaderParagraphFormat(args, p);
        }
        /// <summary>
        /// 设置标准的标题1样式
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        [WriterCommandDescription(StandardCommandNames.Header4)]
        private void Header4(object sender, WriterCommandEventArgs args)
        {
            HeaderFormatCommandParameter p = HeaderFormatCommandParameter.CreateHeader4();
            SetHeaderParagraphFormat(args, p);
        }
        /// <summary>
        /// 设置标准的标题1样式
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        [WriterCommandDescription(StandardCommandNames.Header5)]
        private void Header5(object sender, WriterCommandEventArgs args)
        {
            HeaderFormatCommandParameter p = HeaderFormatCommandParameter.CreateHeader5();
            SetHeaderParagraphFormat(args, p);
        }
        /// <summary>
        /// 设置标准的标题1样式
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        [WriterCommandDescription(StandardCommandNames.Header6)]
        private void Header6(object sender, WriterCommandEventArgs args)
        {
            HeaderFormatCommandParameter p = HeaderFormatCommandParameter.CreateHeader6();
            SetHeaderParagraphFormat(args, p);
        }

        /// <summary>
        /// 设置标准的标题1样式
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        [WriterCommandDescription(StandardCommandNames.Header1WithMultiNumberlist)]
        private void Header1WithMultiNumberlist(object sender, WriterCommandEventArgs args)
        {
            HeaderFormatCommandParameter p = HeaderFormatCommandParameter.CreateHeader1WithMultiNumberlist();
            SetHeaderParagraphFormat(args, p);
        }
        /// <summary>
        /// 设置标准的标题1样式
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        [WriterCommandDescription(StandardCommandNames.Header2WithMultiNumberlist)]
        private void Header2WithMultiNumberlist(object sender, WriterCommandEventArgs args)
        {
            HeaderFormatCommandParameter p = HeaderFormatCommandParameter.CreateHeader2WithMultiNumberlist();
            SetHeaderParagraphFormat(args, p);
        }
        /// <summary>
        /// 设置标准的标题1样式
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        [WriterCommandDescription(StandardCommandNames.Header3WithMultiNumberlist)]
        private void Header3WithMultiNumberlist(object sender, WriterCommandEventArgs args)
        {
            HeaderFormatCommandParameter p = HeaderFormatCommandParameter.CreateHeader3WithMultiNumberlist();
            SetHeaderParagraphFormat(args, p);
        }
        /// <summary>
        /// 设置标准的标题1样式
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        [WriterCommandDescription(StandardCommandNames.Header4WithMultiNumberlist)]
        private void Header4WithMultiNumberlist(object sender, WriterCommandEventArgs args)
        {
            HeaderFormatCommandParameter p = HeaderFormatCommandParameter.CreateHeader4WithMultiNumberlist();
            SetHeaderParagraphFormat(args, p);
        }
        /// <summary>
        /// 设置标准的标题1样式
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        [WriterCommandDescription(StandardCommandNames.Header5WithMultiNumberlist)]
        private void Header5WithMultiNumberlist(object sender, WriterCommandEventArgs args)
        {
            HeaderFormatCommandParameter p = HeaderFormatCommandParameter.CreateHeader5WithMultiNumberlist();
            SetHeaderParagraphFormat(args, p);
        }
        /// <summary>
        /// 设置标准的标题1样式
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        [WriterCommandDescription(StandardCommandNames.Header6WithMultiNumberlist)]
        private void Header6WithMultiNumberlist(object sender, WriterCommandEventArgs args)
        {
            HeaderFormatCommandParameter p = HeaderFormatCommandParameter.CreateHeader6WithMultiNumberlist();
            SetHeaderParagraphFormat(args, p);
        }

        private void SetHeaderParagraphFormat(
            WriterCommandEventArgs args, 
            HeaderFormatCommandParameter parameter )
        {
            if (args.Mode == WriterCommandEventMode.QueryState)
            {
                args.Enabled = false;
                if (args.Document != null)
                {
                    DocumentContentStyle style = args.Document.CurrentStyleInfo.Paragraph;
                    args.Enabled = args.DocumentControler != null
                        && args.DocumentControler.Snapshot.CanModifyParagraphs;
                    if (args.Enabled)
                    {
                        if ( parameter.ParagraphMultiLevel)
                        {
                            args.Checked = style.ParagraphOutlineLevel == parameter.ParagraphOutlineLevel  && style.ParagraphMultiLevel;
                        }
                        else
                        {
                            args.Checked = style.ParagraphOutlineLevel == parameter.ParagraphOutlineLevel ;
                        }
                    }
                }
            }
            else if (args.Mode == WriterCommandEventMode.Invoke)
            {
                DocumentContentStyle pStyle = new DocumentContentStyle();
                pStyle.DisableDefaultValue = true;
                pStyle.ParagraphOutlineLevel = parameter.ParagraphOutlineLevel ;
                pStyle.ParagraphMultiLevel = parameter.ParagraphMultiLevel ;
                pStyle.LeftIndent = parameter.LeftIndent;
                pStyle.FirstLineIndent = parameter.FirstLineIndent;
                pStyle.ParagraphListStyle = parameter.ParagraphListStyle;
                pStyle.LineSpacingStyle = parameter.LineSpacingStyle;
                pStyle.LineSpacing = parameter.LineSpacing;
                pStyle.FontSize = parameter.FontSize;
                if (string.IsNullOrEmpty(parameter.FontName) == false)
                {
                    pStyle.FontName = parameter.FontName;//20150424刘帅加字体名称
                }
                pStyle.FontStyle = parameter.FontStyle;
                DocumentContentStyle txtStyle = new DocumentContentStyle();
                txtStyle.FontSize = parameter.FontSize;
                txtStyle.FontStyle = parameter.FontStyle;
                if (string.IsNullOrEmpty(parameter.FontName) == false)
                {
                    txtStyle.FontName = parameter.FontName;//20150424刘帅加字体名称
                }                
                DCContent content = args.Document.Content ;
                // 获得所有要处理的段落符号对象
                DomElementList pFlags = new DomElementList();
                if (args.Document.Selection.Length == 0)
                {
                    pFlags.Add(args.Document.CurrentParagraphEOF);
                }
                else
                {
                    foreach (DomElement element in args.Document.Selection.ContentElements)
                    {
                        DomParagraphFlagElement p = content.GetParagraphEOFElement(element);
                        if (pFlags.Contains(p) == false)
                        {
                            pFlags.Add(p);
                        }
                    }
                }
                DomElementList elements = new DomElementList();
                foreach (DomParagraphFlagElement pf in pFlags)
                {
                    DomElement e = pf.FirstContentElement;
                    int index = args.Document.Content.IndexOf(e);
                    int index2 = args.Document.Content.IndexOf(pf);
                    for (int iCount = index; iCount <= index2; iCount++)
                    {
                        elements.FastAdd2(content[iCount]);
                    }
                }//foreach
                //elements.AddRange(pFlags);
                args.Document.BeginLogUndo();
                //XTextElementList result = args.Document.Selection.SetParagraphStyle(pStyle);
                bool result = DCSelection.SetElementStyle(
                    txtStyle,
                    pStyle , 
                    null, 
                    args.Document, 
                    elements, 
                    true, 
                    args.Name, 
                    true);
                if (args.Document.CanLogUndo)
                {
                    args.Document.UndoList.AddMethod(UndoMethodTypes.RefreshParagraphTree);
                }
                args.Document.EndLogUndo();
                args.Document.Modified = true;
                args.Document.OnSelectionChanged();
                args.Document.OnDocumentContentChanged();
                args.RefreshLevel = UIStateRefreshLevel.All;
                args.Result = result;
            }
        }


        /// <summary>
        /// 设置段落样式
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        /// <param name="commandName"></param>
        private void SetParagraphStyleProperty(
            object sender,
            WriterCommandEventArgs args,
            string commandName)
        {
            if (args.Mode == WriterCommandEventMode.QueryState)
            {
                if (args.EditorControl == null)
                {
                    args.Enabled = false;
                    return;
                }
                DocumentContentStyle style = args.Document.CurrentStyleInfo.Paragraph ;
                args.Enabled = args.DocumentControler != null
                    && args.DocumentControler.Snapshot.CanModifyParagraphs;
                switch (commandName)
                {
                    case StandardCommandNames.AlignLeft:
                        args.Checked = (style.Align == DocumentContentAlignment.Left);
                        break;
                    case StandardCommandNames.AlignCenter:
                        args.Checked = (style.Align == DocumentContentAlignment.Center);
                        break;
                    case StandardCommandNames.AlignRight:
                        args.Checked = (style.Align == DocumentContentAlignment.Right);
                        break;
                    case StandardCommandNames.AlignJustify:
                        args.Checked = (style.Align == DocumentContentAlignment.Justify);
                        break;
                    case StandardCommandNames.AlignDistribute :
                        args.Checked = (style.Align == DocumentContentAlignment.Distribute);
                        break;
                    case StandardCommandNames.BorderBottom:
                        args.Checked = style.BorderBottom;
                        break;
                    case StandardCommandNames.BorderLeft:
                        args.Checked = style.BorderLeft;
                        break;
                    case StandardCommandNames.BorderRight:
                        args.Checked = style.BorderRight;
                        break;
                    case StandardCommandNames.BorderTop:
                        args.Checked = style.BorderTop;
                        break;
                    case StandardCommandNames.BulletedList:
                        args.Checked = style.IsBulletedList;
                        break;
                    case StandardCommandNames.NumberedList:
                        args.Checked = style.IsListNumberStyle ;
                        break;
                    case StandardCommandNames.FirstLineIndent:
                        args.Checked = style.FirstLineIndent > 1.0f;
                        break;
                    case StandardCommandNames.ParagraphListStyle :
                        args.Enabled = true;
                        break;
                    case StandardCommandNames.IncreaseFirstLineIndent :
                        args.Enabled = true;
                        break;
                    default:
                        args.Enabled = false;
                        return;
                }
            }
            else if (args.Mode == WriterCommandEventMode.Invoke)
            {
                args.Result = false;
                
                DocumentContentStyle cs = args.Document.CurrentStyleInfo.Paragraph ;
                DocumentContentStyle ns = args.Document.CreateDocumentContentStyle();
                ns.DisableDefaultValue = true;
                switch (commandName)
                {
                    case StandardCommandNames.AlignCenter:
                        ns.Align = DocumentContentAlignment.Center;
                        args.RefreshLevel = UIStateRefreshLevel.All;
                        break;
                    case StandardCommandNames.AlignJustify:
                        ns.Align = DocumentContentAlignment.Justify;
                        args.RefreshLevel = UIStateRefreshLevel.All;
                        break;
                    case StandardCommandNames.AlignLeft:
                        ns.Align = DocumentContentAlignment.Left;
                        args.RefreshLevel = UIStateRefreshLevel.All;
                        break;
                    case StandardCommandNames.AlignRight:
                        ns.Align = DocumentContentAlignment.Right;
                        args.RefreshLevel = UIStateRefreshLevel.All;
                        break;
                    case StandardCommandNames.AlignDistribute :
                        ns.Align = DocumentContentAlignment.Distribute;
                        args.RefreshLevel = UIStateRefreshLevel.All;
                        break;
                    case StandardCommandNames.BorderBottom:
                        // 从用户参数中获得设置
                        ns.BorderBottom = WriterUtilsInner.GetArgumentBooleanValue(
                            args.Parameter,
                            !cs.BorderBottom);
                        break;
                    case StandardCommandNames.BorderLeft:
                        // 从用户参数中获得设置
                        ns.BorderLeft = WriterUtilsInner.GetArgumentBooleanValue(
                            args.Parameter,
                            !cs.BorderLeft);
                        break;
                    case StandardCommandNames.BorderRight:
                        // 从用户参数中获得设置
                        ns.BorderRight = WriterUtilsInner.GetArgumentBooleanValue(
                            args.Parameter,
                            !cs.BorderRight);
                        break;
                    case StandardCommandNames.BorderTop:
                        // 从用户参数中获得设置
                        ns.BorderTop = WriterUtilsInner.GetArgumentBooleanValue(
                            args.Parameter,
                            !cs.BorderTop);
                        break;
                    case StandardCommandNames.ParagraphListStyle:
                        {
                            bool setStyle = false;
                            ParagraphListStyle pls = args.Document.CurrentStyleInfo.Paragraph.ParagraphListStyle;
                            // 从用户参数中获得设置
                            if (args.Parameter is ParagraphListStyle)
                            {
                                pls = (ParagraphListStyle)args.Parameter;
                                setStyle = true;
                            }
                            else if (args.Parameter is string)
                            {
                                try
                                {
                                    pls = (ParagraphListStyle)Enum.Parse(typeof(ParagraphListStyle), (string)args.Parameter);
                                    setStyle = true;
                                }
                                catch
                                {
                                }
                            }
                            if (setStyle == false)
                            {
                                // 操作无意义，无需进行后续设置
                                return;
                            }
                            ns.ParagraphListStyle = pls;
                            SetParagraphIndentByListStyle(ns, args.Document); 
                            args.RefreshLevel = UIStateRefreshLevel.All;
                        }
                        break;
                    case StandardCommandNames.BulletedList:
                        {
                            // 从用户参数中获得设置
                            bool v = WriterUtilsInner.GetArgumentBooleanValue(
                                args.Parameter,
                                !cs.IsBulletedList);
                            if (v != cs.IsBulletedList)
                            {
                                if (v)
                                {
                                    ns.ParagraphListStyle = Drawing.ParagraphListStyle.BulletedList;
                                }
                                else
                                {
                                    ns.ParagraphListStyle = Drawing.ParagraphListStyle.None;
                                }
                            }
                            SetParagraphIndentByListStyle(ns, args.Document); 
                            args.RefreshLevel = UIStateRefreshLevel.All;
                        }
                        break;
                    case StandardCommandNames.NumberedList:
                        // 从用户参数中获得设置
                        {
                            bool v = WriterUtilsInner.GetArgumentBooleanValue(
                                args.Parameter,
                                !cs.IsListNumberStyle);
                            if (v != cs.IsListNumberStyle)
                            {
                                if (v)
                                {
                                    ns.ParagraphListStyle = Drawing.ParagraphListStyle.ListNumberStyle;
                                }
                                else
                                {
                                    ns.ParagraphListStyle = Drawing.ParagraphListStyle.None;
                                }
                            }
                            SetParagraphIndentByListStyle(ns, args.Document); 
                            args.RefreshLevel = UIStateRefreshLevel.All;
                        }
                        break;
                    case StandardCommandNames.FirstLineIndent:
                        {
                            bool bolSet = false;
                            // 试图读取用户参数中的设置
                            if (WriterUtilsInner.TryGetArgumentBooleanValue(args.Parameter, ref bolSet) == false)
                            {
                                if (cs.FirstLineIndent > 1f)
                                {
                                    // 已经是首行缩进，则取消首行缩进
                                    bolSet = false;
                                }
                                else
                                {
                                    bolSet = true;
                                }
                            }
                            if (bolSet)
                            {
                                // 设置段落首行缩进
                                ns.FirstLineIndent = 100;
                                ns.LeftIndent = 0;
                            }
                            else
                            {
                                // 取消设置
                                ns.FirstLineIndent = 0;
                                ns.LeftIndent = 0;
                            }
                            args.RefreshLevel = UIStateRefreshLevel.All;
                        }
                        break;
                    case StandardCommandNames.IncreaseFirstLineIndent :
                        {
                            // 增加段落首行缩进
                            bool bolSet = WriterUtilsInner.GetArgumentBooleanValue(args.Parameter, true);
                            if (bolSet)
                            {
                                // 追加段落首行缩进
                                ns.FirstLineIndent += 100;
                                if (ns.FirstLineIndent > 180)
                                {
                                    // 增加段落整体缩进 
                                    ns.LeftIndent += 100;
                                }
                            }
                            else
                            {
                                // 减少段落首行缩进
                                ns.FirstLineIndent -= 100;
                                if (ns.FirstLineIndent < 0)
                                {
                                    ns.FirstLineIndent = 0;
                                }
                                if (ns.FirstLineIndent < 80)
                                {
                                    ns.LeftIndent = 0;
                                }
                            }
                            args.RefreshLevel = UIStateRefreshLevel.All;
                        }
                        break;
                    default:
                        throw new NotSupportedException(commandName);
                }//switch
                args.Document.BeginLogUndo();
                DomElementList list = args.Document.Selection.SetParagraphStyle(ns);
                args.Document.EndLogUndo();
                //args.Document.CurrentStyle.Underline = v;
                args.Document.OnSelectionChanged();
                args.Document.OnDocumentContentChanged();
                if (list != null && list.Count > 0)
                {
                    args.Result = true;
                }
            }
        }

    }
}
