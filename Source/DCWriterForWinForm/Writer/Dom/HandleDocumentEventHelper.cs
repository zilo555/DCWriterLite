using System;
using System.Collections.Generic;
using System.Text;
using DCSoft.Writer.Controls;
using DCSoft.Drawing;
using System.ComponentModel;
using System.Windows.Forms;
using DCSoft.WinForms;
using DCSoft.WinForms.Native;
//  
using DCSoft.Common;
using DCSoft.Writer.Dom.Undo;
using System.Runtime.CompilerServices;

// 袁永福到此一游 2018-7-2

namespace DCSoft.Writer.Dom
{
    /// <summary>
    /// 处理文档元素事件的代码
    /// </summary>
    internal static class HandleDocumentEventHelper
    {
        /// <summary>
        /// 处理文档元素事件
        /// </summary>
        /// <param name="element">文档元素对象</param>
        /// <param name="args">事件参数</param>
        public static void HandleDocumentEvent(DomElement element, DocumentEventArgs args)
        {
            if (element is DomTableCellElement)
            {
                Handle_XTextTableCellElement((DomTableCellElement)element, args);
            }
            else if (element is DomInputFieldElement)
            {
                Handle_XTextInputFieldElement((DomInputFieldElement)element, args);
            }
            else if (element is DomInputFieldElementBase)
            {
                Handle_XTextInputFieldElementBase((DomInputFieldElementBase)element, args);
            }
            else if (element is DomContainerElement)
            {
                Handle_XTextContainerElement((DomContainerElement)element, args);
            }
            else if (element is DomObjectElement)
            {
                Handle_XTextObjectElement((DomObjectElement)element, args);
            }
            else
            {
                Handle_XTextElement(element, args);
            }
        }

        /// <summary>
        /// 处理文档事件
        /// </summary>
        /// <param name="element"></param>
        /// <param name="args">参数</param>
        private static void Handle_XTextTableCellElement(DomTableCellElement element, DocumentEventArgs args)
        {
            if (args.Style == DocumentEventStyles.MouseMove
                || args.Style == DocumentEventStyles.MouseDown)
            {
                // 处理鼠标移动或鼠标按键事件
                float size = element.OwnerDocument.PixelToDocumentUnit(
                    element.GetDocumentBehaviorOptions().TableCellOperationDetectDistance);
                float sizeLeft = Math.Max(size * 2, element.RuntimeStyle.PaddingLeft);
                DomAccessFlags detectFlags = DomAccessFlags.Normal;
                bool localHandled = false;
                var docCtrol = element.OwnerDocument.DocumentControler;
                if (Math.Abs(args.X) <= sizeLeft && Math.Abs(args.X) > size)
                {
                    // 鼠标靠近单元格左边框而且单元格是最左边的，
                    // 此时若鼠标点击，则选择整个单元格
                    if (docCtrol.CanModify(
                            element,
                            detectFlags))
                    {
                        args.Cursor = WriterControl.InnerCursorSelectCell;// XCursors.SelectCell;
                        if (args.Style == DocumentEventStyles.MouseDown && args.StrictMatch)
                        {
                            element.Select();
                            args.Handled = true;
                        }
                    }
                }
                else if (Math.Abs(args.X) <= size)
                {
                    DomTableColumnElement col = element.OwnerColumn;
                    col = (DomTableColumnElement)element.OwnerTable.Columns.GetPreElement(col);
                    if (col == null)
                    {
                        return;
                    }

                    if (docCtrol.CanModify(element, detectFlags))
                    {
                        args.Cursor = WriterControl.InnerCursorSizeWE;// System.Windows.Forms.Cursors.SizeWE;// .VSplit;
                        if (args.Style == DocumentEventStyles.MouseDown
                            && args.StrictMatch)
                        {
                            if (col != null)
                            {
                                // 对表格列执行鼠标拖拽左右移动修改表格列宽度操作
                                localHandled = true;
                                Handle_XTextTableCellElement_DragSetColumnWidth(element, col, args);
                            }
                        }
                    }
                }
                else if (Math.Abs(args.Y) <= size)
                {
                    DomTableRowElement row = element.LastOwnerRow;
                    row = (DomTableRowElement)row.PreviousElement;


                    if (row != null && docCtrol.CanModify(element, detectFlags))// element.OwnerTable.AllowUserToResizeRows)
                    {
                        args.Cursor = WriterControl.InnerCursorSizeNS;// System.Windows.Forms.Cursors.SizeNS;// .HSplit;
                        if (args.Style == DocumentEventStyles.MouseDown
                            && args.StrictMatch)
                        {
                            // 对上一行的单元格执行鼠标拖拽上下移动修改表格行高操作
                            localHandled = true;
                            Handle_XTextTableCellElement_DragSetRowSpecifyHeight(element, row, args);
                        }
                    }

                }
                else if (Math.Abs(args.Y - element.Height) <= size)
                {

                    if (docCtrol.CanModify(element, detectFlags))// element.OwnerTable.AllowUserToResizeRows)
                    {
                        args.Cursor = WriterControl.InnerCursorSizeNS;// System.Windows.Forms.Cursors.SizeNS;// .HSplit;
                        if (args.Style == DocumentEventStyles.MouseDown
                            && args.StrictMatch)
                        {
                            // 对上一行的单元格执行鼠标拖拽上下移动修改表格行高操作
                            localHandled = true;
                            Handle_XTextTableCellElement_DragSetRowSpecifyHeight(element, element.LastOwnerRow, args);
                        }
                    }

                }
                else if (Math.Abs(args.X - element.Width) <= size)
                {

                    if (docCtrol.CanModify(element, detectFlags))
                    {
                        args.Cursor = WriterControl.InnerCursorSizeWE;// System.Windows.Forms.Cursors.SizeWE;// .VSplit;
                        if (args.Style == DocumentEventStyles.MouseDown
                            && args.StrictMatch)
                        {
                            // 对表格列执行鼠标拖拽左右移动修改表格列宽度操作
                            localHandled = true;
                            Handle_XTextTableCellElement_DragSetColumnWidth(element, element.LastOwnerColumn, args);
                        }
                    }

                }//if
                if (args.Handled == false && localHandled == false)
                {
                    if (args.Style == DocumentEventStyles.MouseMove && args.Button == MouseButtons.None)
                    {
                        if (element.DocumentContentElement.Selection.ContainsCell(element))
                        {
                            args.Cursor = WriterControl.InnerCursorArrow;
                            args.Handled = true;
                            return;
                        }
                    }
                    Handle_XTextContainerElement(element, args);
                }
            }//if
            else
            {
                Handle_XTextContainerElement(element, args);// base.HandleDocumentEvent(args);
            }
        }

        /// <summary>
        /// 拖拽方式设置表格行的高度
        /// </summary>
        /// <param name="element"></param>
        /// <param name="row"></param>
        /// <param name="args"></param>
        private static void Handle_XTextTableCellElement_DragSetRowSpecifyHeight(
            DomTableCellElement element,
            DomTableRowElement row,
            DocumentEventArgs args)
        {

            WriterControl ctl = element.WriterControl;
            //ctl.PagesTransform.UseAbsTransformPoint = true;
            ctl.GetInnerViewControl().InnerSetMouseCaptureTransformByElement(element);
            var handlerFinished = new DocumentViewControl.CaptureMouseMoveFinishedEventHandler(delegate (Point[] ps)
            {
                args.Handled = true;
                float newHeight = row.Height + ps[1].Y - ps[0].Y;
                row.EditorSetRowSpecifyHeight(newHeight, true);
            });
            ctl.GetInnerViewControl().CaptureMouseMove(
                            new CaptureMouseMoveEventHandler(Handle_XTextTableCellElement_mc_ReversibleDrawCallback),
                            handlerFinished,
                            Rectangle.Empty,
                            element);
            ctl.GetInnerViewControl().InnerSetMouseCaptureTransformByElement(null);
            args.Handled = true;
            args.CancelBubble = true;

        }

        /// <summary>
        /// 拖拽方式设置表格列的宽度
        /// </summary>
        /// <param name="element"></param>
        /// <param name="col"></param>
        /// <param name="args"></param>
        private static void Handle_XTextTableCellElement_DragSetColumnWidth(
            DomTableCellElement element,
            DomTableColumnElement col,
            DocumentEventArgs args)
        {
            element.InnerViewControl.PagesTransform.UseAbsTransformPoint = true;
            var handlerFinished = new DocumentViewControl.CaptureMouseMoveFinishedEventHandler(delegate (Point[] ps)
            {
                args.Handled = true;
                float newWidth = col.Width + ps[1].X - ps[0].X;
                float dWidth = ps[1].X - ps[0].X;
                if ((System.Windows.Forms.Control.ModifierKeys & System.Windows.Forms.Keys.Control)
                    == System.Windows.Forms.Keys.Control)
                {
                    // 若按下Control键，则设置表格列的宽度而且不修改下一个表格列的宽度，
                    // 这样会修改整个表格的宽度
                    col.EditorSetWidth(newWidth, true, false);
                }
                else
                {
                    // 判断是否只拖拽单元格宽度，而不修改其他单元格的宽度
                    DCSelection selection = element.OwnerDocument.Selection;
                    DomTableCellElement cellForSetSingleWidth = null;
                    DomTableCellElement currentSelectedCell = null;
                    if (selection.Mode == ContentRangeMode.Cell)
                    {
                        DomElementList selectCells = selection.CellsWithoutOverried;
                        if (selectCells != null
                            && selectCells.Count == 1)
                        {
                            currentSelectedCell = (DomTableCellElement)selectCells[0];
                            DomTableRowElement row2 = currentSelectedCell.OwnerRow;
                            cellForSetSingleWidth = (DomTableCellElement)row2.Cells[col.Index];
                            if (cellForSetSingleWidth.OverrideCell != null)
                            {
                                cellForSetSingleWidth = cellForSetSingleWidth.OverrideCell;
                            }
                            if (cellForSetSingleWidth != null)
                            {
                                if (cellForSetSingleWidth.ColIndex + cellForSetSingleWidth.ColSpan
                                    == element.OwnerTable.Columns.Count)
                                {
                                    // 单元格是最后一列的
                                    cellForSetSingleWidth = null;
                                }
                                else
                                {
                                    if (element.RowIndex < cellForSetSingleWidth.RowIndex
                                        || element.RowIndex >= cellForSetSingleWidth.RowIndex + cellForSetSingleWidth.RowSpan)
                                    {
                                        // 当前单元格的行号和要拖拽的单元格无关联，不执行拖拽单个单元格的宽度
                                        cellForSetSingleWidth = null;
                                    }
                                    else if (col != cellForSetSingleWidth.OwnerColumn && col != cellForSetSingleWidth.LastOwnerColumn)
                                    {
                                        cellForSetSingleWidth = null;
                                    }

                                }
                            }
                        }
                    }
                    if (cellForSetSingleWidth != null)
                    {
                        // 单独的设置单元格的宽度，而不影响其他单元格的宽度
                        if (cellForSetSingleWidth.EditorSetCellWidthSingle(
                            cellForSetSingleWidth.Width + dWidth,
                            true) == false)
                        {
                            // 设置表格列的宽度
                            col.EditorSetWidth(newWidth, true, true);
                        }
                    }
                    else
                    {
                        // 设置表格列的宽度
                        col.EditorSetWidth(newWidth, true, true);
                    }
                    if (currentSelectedCell != null)
                    {
                        currentSelectedCell.Select();
                    }
                }
                args.Handled = true;
            });
            element.InnerViewControl.CaptureMouseMove(
                        new CaptureMouseMoveEventHandler(Handle_XTextTableCellElement_mc2_ReversibleDrawCallback),
                        handlerFinished,
                        Rectangle.Empty,
                        element);
            args.Handled = true;
            args.CancelBubble = true;
        }

        private static void Handle_XTextTableCellElement_mc_ReversibleDrawCallback(
            object eventSender,
            CaptureMouseMoveEventArgs e)
        {
            DomTableCellElement element = (DomTableCellElement)e.Tag;
            RectangleF rect = element.OwnerTable.GetAbsBounds();
            element.InnerViewControl.ReversibleViewDrawLine(
                (int)rect.Left,
                (int)e.CurrentPosition.Y,
                (int)rect.Right,
                (int)e.CurrentPosition.Y);
        }

        private static void Handle_XTextTableCellElement_mc2_ReversibleDrawCallback(
            object eventSender,
            CaptureMouseMoveEventArgs e)
        {
            DomTableCellElement element = (DomTableCellElement)e.Tag;
            RectangleF rect = element.OwnerTable.GetAbsBounds();
            element.InnerViewControl.ReversibleViewDrawLine(
                (int)e.CurrentPosition.X,
                (int)rect.Top,
                e.CurrentPosition.X,
                (int)rect.Bottom);
        }

        /// <summary>
        /// 处理文档事件
        /// </summary>
        /// <param name="element"></param>
        /// <param name="args"></param>
        private static void Handle_XTextInputFieldElement(DomInputFieldElement element, DocumentEventArgs args)
        {
            if (args.Style == DocumentEventStyles.DefaultEditMethod)
            {
                if (element.WriterControl.InnerBeginEditElementValue(
                    element,
                    false,
                    ValueEditorActiveMode.F2,
                    true,
                    true))
                {
                    args.Handled = true;
                }
            }
            else
            {
                if (args.Style == DocumentEventStyles.KeyDown)
                {
                    if (args.KeyCode == System.Windows.Forms.Keys.Enter
                       && ((element.EditorActiveMode & ValueEditorActiveMode.Enter) == ValueEditorActiveMode.Enter))
                    {
                        element.InnerViewControl.IgnoreNextKeyPressEventOnce = true;
                        if (element.WriterControl.InnerBeginEditElementValue(
                           element,
                           false,
                           ValueEditorActiveMode.Enter,
                           true,
                           true))
                        {
                            args.Handled = true;
                            return;
                        }
                    }
                }
                Handle_XTextInputFieldElementBase(element, args);//base.HandleDocumentEvent(args);
                if (_Handle_XTextInputFieldElementBase_DisplayFormat == false && element.OwnerDocument != null)
                {
                    _Handle_XTextInputFieldElementBase_DisplayFormat = false;
                    var states = element.OwnerDocument.States;
                    if (args.Style == DocumentEventStyles.LostFocus
                        && states.ExecutingRedo == false
                        && states.ExecutingUndo == false)
                    {
                        if (element.Modified
                            && element.FieldSettings != null
                            && element.UserEditable == true)
                        {
                            var mode = element.FieldSettings.EditStyle;
                            if (mode == InputFieldEditStyle.Date
                                || mode == InputFieldEditStyle.DateTime
                                || mode == InputFieldEditStyle.Time)
                            {
                                // 特别针对日期输入模式进行修正
                                var text = element.Text;
                                if (text != null
                                    && text.Length > 0
                                    && StringConvertHelper.IsIntegerString(text))
                                {
                                    if (mode == InputFieldEditStyle.Date
                                        || mode == InputFieldEditStyle.DateTime)
                                    {
                                        var dtm = StringConvertHelper.ToDBDateTime(text, DateTime.MinValue);
                                        if (dtm != DateTime.MinValue)
                                        {
                                            if (mode == InputFieldEditStyle.Date)
                                            {
                                                element.EditorTextExt = dtm.ToString(WriterConst.Format_YYYY_MM_DD);
                                            }
                                            else if (mode == InputFieldEditStyle.DateTime)
                                            {
                                                element.EditorTextExt = dtm.ToString(WriterConst.Format_YYYY_MM_DD_HH_MM_SS);
                                            }
                                        }
                                    }
                                    else if (mode == InputFieldEditStyle.Time)
                                    {
                                        var dtm = StringConvertHelper.ToDBTime(text, DateTime.MinValue);
                                        if (dtm != DateTime.MinValue)
                                        {
                                            element.EditorTextExt = dtm.ToString(WriterConst.Format_HH_mm_ss);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        private static bool _Handle_XTextInputFieldElementBase_DisplayFormat = false;

        /// <summary>
        /// 处理文档事件
        /// </summary>
        /// <param name="element"></param>
        /// <param name="args"></param>
        private static void Handle_XTextInputFieldElementBase(DomInputFieldElementBase element, DocumentEventArgs args)
        {
            _Handle_XTextInputFieldElementBase_DisplayFormat = false;
            var states = element.OwnerDocument.States;
            if (args.Style == DocumentEventStyles.LostFocus
                && states.ExecutingRedo == false
                && states.ExecutingUndo == false)
            {
                if (element.Modified)
                {
                    //wyc20250210:防止2024-2-29日期被MM-dd格式化后光标移开丢失内容
                    string usetext = element.Text;
                    if (element is DomInputFieldElement)
                    {
                        DomInputFieldElement input2 = element as DomInputFieldElement;
                        if (input2.FieldSettings != null &&
                            input2.FieldSettings.EditStyle != InputFieldEditStyle.DropdownList &&
                            input2.FieldSettings.EditStyle != InputFieldEditStyle.Text)
                        {
                            usetext = input2.InnerValue;
                        }
                    }
                    //////////////////////////////////////////////////////////////

                    string txt = element.GetDisplayText(usetext);
                    if (txt != element.Text)
                    {
                        var args9 = new SetContainerTextArgs();
                        args9.IgnoreDisplayFormat = true;
                        args9.NewText = txt;
                        args9.AccessFlags = DomAccessFlags.None;
                        element.SetEditorText(args9);
                        _Handle_XTextInputFieldElementBase_DisplayFormat = true;
                    }
                }
            }
            Handle_XTextContainerElement(element, args);// base.HandleDocumentEvent(args);
        }

        /// <summary>
        /// 处理文档消息事件
        /// </summary>
        /// <param name="element"></param>
        /// <param name="args">参数</param>
        private static void Handle_XTextContainerElement(DomContainerElement element, DocumentEventArgs args)
        {
            Handle_XTextElement(element, args);// base.HandleDocumentEvent(args);
        }

        /// <summary>
        /// 处理文档用户界面事件
        /// </summary>
        /// <param name="element"></param>
        /// <param name="args">事件参数</param>
        private static void Handle_XTextObjectElement(DomObjectElement element, DocumentEventArgs args)
        {
            var bopts = element.GetDocumentBehaviorOptions();
            if (args.Style == DocumentEventStyles.MouseDown)
            {
                    DragPointStyle hit = element.GetDragHit(args.X, args.Y);
                    if (element.ShowDragRect())
                    {
                        if (hit >= 0)
                        {
                            Handle_XTextObjectElement_DragBounds(element, hit);
                            args.Handled = true;
                            if (element.WriterControl != null)
                            {
                                element.WriterControl.UpdateTextCaret();
                            }
                            return;
                        }
                    }
            }
            if (args.Style == DocumentEventStyles.MouseClick)
            {
                    DragPointStyle hit = element.GetDragHit(args.X, args.Y);
                    if (element.ShowDragRect())
                    {
                        if (hit >= 0)
                        {
                            Handle_XTextObjectElement_DragBounds(element, hit);
                            args.Handled = true;
                            if (element.WriterControl != null)
                            {
                                element.WriterControl.UpdateTextCaret();
                            }
                            return;
                        }
                    }
                    args.Handled = true;
                    if (element.RuntimeSelectable() || element.MouseClickToSelect) //if (element.AbsBounds.Contains(args.X, args.Y))
                    {
                        int index = element.ContentIndex;
                        index = element.OwnerDocument.Content.InnerFixIndex(index, FixIndexDirection.Both, true);
                        if (index == element.ContentIndex)
                        {
                            element.OwnerDocument.Content.SetSelection(index, 1);
                        }
                        else
                        {
                            element.OwnerDocument.Content.SetSelection(index, 0);
                        }
                        element.OwnerDocument.Selection.IncreaseStateVersion();// .StateVersion++;
                        args.AlreadSetSelection = true;
                        //element.OwnerDocument.CurrentContentElement.SetSelection(element.ViewIndex, 1);
                        if (hit != DragPointStyle.Move)
                        {
                            args.Handled = true;
                        }
                        else
                        {
                            args.Handled = true;
                            Handle_XTextElement(element, args);
                        }
                    }
            }
            else if (args.Style == DocumentEventStyles.MouseMove)
            {
                    if (element.ShowDragRect())
                    {
                        DragRectangle dr = element.CreateDragRectangle();
                        DragPointStyle hit = dr.DragHit(args.X, args.Y);
                        if (hit >= 0)
                        {
                            args.Cursor = DragRectangle.GetMouseCursor(hit);
                            Handle_XTextElement(element, args);
                            return;
                        }
                    }
                    args.Handled = true;
            }
            else
            {
                Handle_XTextElement(element, args);
            }
        }


        private static bool Handle_XTextObjectElement_DragBounds(DomObjectElement element, DragPointStyle hit)
        {
            bool bolResult = false;
            MouseCapturer cap = new MouseCapturer(element.InnerViewControl);
            cap.Tag = hit;
            cap.Tag2 = element;

            cap.ReversibleShape = ReversibleShapeStyle.Custom;
            cap.Draw = new CaptureMouseMoveEventHandler(Handle_XTextObjectElement_cap_Draw);
            DomDocumentContentElement ce = element.DocumentContentElement;
            cap.EventOnFinished = new EventHandler(delegate (object sender9, EventArgs args9)
            {
                if (element.LastDragBounds.Width > 0 && element.LastDragBounds.Height > 0)
                {
                    var targetWidth = element.LastDragBounds.Width;
                    var targetHeight = element.LastDragBounds.Height;
                    element.LastDragBounds = Rectangle.Empty;
                    if (targetWidth != element.Width
                        || targetHeight != element.Height)
                    {
                        SizeF OldSize = new SizeF(
                           element.Width,
                           element.Height);
                        element.InvalidateView();
                        SizeF oldSize = element.Size;
                        element.EditorSize = new SizeF(targetWidth, targetHeight);
                        WriterUtilsInner.RefreshElementsSize(
                            element.OwnerDocument,
                            new DomElementList(element), false);
                        if (oldSize != element.Size)
                        {
                            // 大小发生了改变
                            element.OnResize();
                            element.InvalidateView();
                            ce.SetSelection(element.ContentIndex, 1);
                            if (element.OwnerDocument.BeginLogUndo())
                            {
                                element.OwnerDocument.UndoList.AddProperty(
                                    XTextUndoStyles.EditorSize,
                                    OldSize,
                                    new SizeF(element.Width, element.Height),
                                    element);
                                element.OwnerDocument.EndLogUndo();
                            }
                            element.ContentElement.RefreshPrivateContent(
                                element.ContentElement.PrivateContent.IndexOf(element));
                            element.OwnerDocument.Modified = true;
                            element.UpdateContentVersion();
                            // 触发文档内容修改事件
                            ContentChangedEventArgs args2 = new ContentChangedEventArgs();
                            args2.EventSource = ContentChangedEventSource.UndoRedo;
                            args2.Document = element.OwnerDocument;
                            DomContainerElement c = element.Parent;
                            args2.Element = c;
                            c.RaiseBubbleOnContentChanged(args2);
                            element.OwnerDocument.OnDocumentContentChanged();
                            bolResult = true;
                            return;
                        }
                    }
                }
            });
            cap.CaptureMouseMove();
            return bolResult;
        }

        private static void Handle_XTextObjectElement_cap_Draw(object eventSender, CaptureMouseMoveEventArgs e)
        {
            DragPointStyle hit = (DragPointStyle)e.Sender.Tag;
            DomObjectElement element = (DomObjectElement)e.Sender.Tag2;
            Rectangle rect = Rectangle.Ceiling(element.GetAbsBounds());
            Point p1 = e.StartPosition;
            Point p2 = e.CurrentPosition;
            Point p = DragRectangle.GetPoint(rect, hit);
            SimpleRectangleTransform trans = element.WriterControl.GetTransformItemByDescPoint(
                p.X,
                p.Y);
            if (trans != null)
            {
                p1 = trans.TransformPoint(p1);
                p2 = trans.TransformPoint(p2);
                rect = DragRectangle.CalcuteDragRectangle(
                    (int)(p2.X - p1.X),
                    (int)(p2.Y - p1.Y),
                    hit,
                    Rectangle.Ceiling(element.GetAbsBounds()));
                if (rect.Width > (int)element.OwnerDocument.Width)
                {
                    rect.Width = (int)element.OwnerDocument.Width;
                }
                if (element.RuntimeWidthHeightRate() > 0.1)
                {
                    rect.Height = (int)(rect.Width / element.RuntimeWidthHeightRate());
                }
                element.LastDragBounds = rect;
                element.InnerViewControl.InnerReversibleDraw(
                    rect.Left,
                    rect.Top,
                    rect.Right,
                    rect.Bottom, ReversibleShapeStyle.Rectangle);
            }
        }

        /// <summary>
        /// 处理文档用户界面事件
        /// </summary>
        /// <param name="element"></param>
        /// <param name="args"></param>
        private static void Handle_XTextElement(DomElement element, DocumentEventArgs args)
        {
            switch (args.Style)
            {
                case DocumentEventStyles.GotFocus:
                    {
                        // 触发获得焦点事件
                        ElementEventArgs args2 = new ElementEventArgs(element);
                        element.OnGotFocus(args2);
                        if (args2.Handled)
                        {
                            args.Handled = true;
                        }
                        if (args2.CancelBubble)
                        {
                            args.CancelBubble = true;
                        }
                        args2.Dispose();
                    }
                    break;
                case DocumentEventStyles.LostFocus:
                    {
                        // 触发失去焦点事件
                        ElementEventArgs args2 = new ElementEventArgs(element);
                        element.OnLostFocus(args2);
                        if (args2.Handled)
                        {
                            args.Handled = true;
                        }
                        if (args2.CancelBubble)
                        {
                            args.CancelBubble = true;
                        }
                        args2.Dispose();
                    }
                    break;
                case DocumentEventStyles.MouseEnter:
                    {
                        // 触发鼠标进入事件
                        ElementEventArgs args2 = new ElementEventArgs(element);
                        element.OnMouseEnter(args2);
                        if (args2.Handled)
                        {
                            args.Handled = true;
                        }
                        if (args2.CancelBubble)
                        {
                            args.CancelBubble = true;
                        }
                        args2.Dispose();
                    }
                    break;
                case DocumentEventStyles.MouseLeave:
                    {
                        // 触发鼠标离开事件
                        ElementEventArgs args2 = new ElementEventArgs(element);
                        element.OnMouseLeave(args2);
                        if (args2.Handled)
                        {
                            args.Handled = true;
                        }
                        if (args2.CancelBubble)
                        {
                            args.CancelBubble = true;
                        }
                        args2.Dispose();
                    }
                    break;
                case DocumentEventStyles.MouseDown:
                    {
                        // 处理鼠标按键按下事件
                        ElementMouseEventArgs args2 = new ElementMouseEventArgs(args, element);
                        element.OnMouseDown(args2);
                        if (args2.Handled)
                        {
                            args.Handled = true;
                        }
                        if (args2.CancelBubble)
                        {
                            args.CancelBubble = true;
                        }
                        args2.Dispose();
                        break;
                    }
                case DocumentEventStyles.MouseMove:
                    {
                        // 处理鼠标移动事件
                        ElementMouseEventArgs args2 = new ElementMouseEventArgs(args, element);
                        element.OnMouseMove(args2);
                        if (args2.Handled)
                        {
                            args.Handled = true;
                        }
                        if (args2.CancelBubble)
                        {
                            args.CancelBubble = true;
                        }
                        args2.Dispose();
                        break;
                    }
                case DocumentEventStyles.MouseUp:
                    {
                        // 处理鼠标按键松开事件
                        ElementMouseEventArgs args2 = new ElementMouseEventArgs(args, element);
                        element.OnMouseUp(args2);
                        if (args2.Handled)
                        {
                            args.Handled = true;
                        }
                        if (args2.CancelBubble)
                        {
                            args.CancelBubble = true;
                        }
                        args2.Dispose();
                        break;
                    }
                case DocumentEventStyles.MouseClick:
                    {
                        // 处理鼠标按键点击事件
                        ElementMouseEventArgs args2 = new ElementMouseEventArgs(args, element);
                        element.OnMouseClick(args2);
                        if (args2.Handled)
                        {
                            args.Handled = true;
                        }
                        if (args2.CancelBubble)
                        {
                            args.CancelBubble = true;
                        }
                        args2.Dispose();
                        break;
                    }
                case DocumentEventStyles.MouseDblClick:
                    {
                        // 处理鼠标按键双击事件
                        ElementMouseEventArgs args2 = new ElementMouseEventArgs(args, element);
                        element.OnMouseDblClick(args2);
                        if (args2.Handled)
                        {
                            args.Handled = true;
                        }
                        if (args2.CancelBubble)
                        {
                            args.CancelBubble = true;
                        }
                        args2.Dispose();
                        break;
                    }
                case DocumentEventStyles.KeyDown:
                    {
                        // 键盘按键按下事件
                        ElementKeyEventArgs args2 = new ElementKeyEventArgs(args, element);
                        element.OnKeyDown(args2);
                        if (args2.Handled)
                        {
                            args.Handled = true;
                        }
                        if (args2.CancelBubble)
                        {
                            args.CancelBubble = true;
                        }
                        args2.Dispose();
                        break;
                    }
                case DocumentEventStyles.KeyPress:
                    {
                        // 键盘字符事件
                        ElementKeyEventArgs args2 = new ElementKeyEventArgs(args, element);
                        element.OnKeyPress(args2);
                        if (args2.Handled)
                        {
                            args.Handled = true;
                        }
                        if (args2.CancelBubble)
                        {
                            args.CancelBubble = true;
                        }
                        args2.Dispose();
                        break;
                    }
                case DocumentEventStyles.KeyUp:
                    {
                        // 键盘按键松开事件
                        ElementKeyEventArgs args2 = new ElementKeyEventArgs(args, element);
                        element.OnKeyUp(args2);
                        if (args2.Handled)
                        {
                            args.Handled = true;
                        }
                        if (args2.CancelBubble)
                        {
                            args.CancelBubble = true;
                        }
                        args2.Dispose();
                        break;
                    }
            }
        }
    }
}