using System;
using System.Collections.Generic;
using System.Text;
using DCSoft.Common;
using System.Collections;
using DCSoft.Writer.Dom;
using DCSoft.Writer.Controls;
using System.Windows.Forms;

namespace DCSoft.Writer
{
    public class DCTabIndexManager
    {
        public DCTabIndexManager()
        {
        }
        public void Dispose()
        {
            this._Document = null;

        }
        private DomDocument _Document = null;
        public DomDocument Document
        {
            get
            {
                return this._Document;
            }
            set
            {
                this._Document = value;
            }
        }
        public DomElement GetNextSelectedElementForHotKey(
            DomElement currentElement,
            System.Windows.Forms.Keys keyCode,
            bool shiftFlag,
            KeyEventArgs srcArgs)
        {
            DomContainerElement c = currentElement.Parent;
            if (currentElement is DomFieldBorderElement)
            {
                DomFieldBorderElement fb = (DomFieldBorderElement)currentElement;
                if (fb.Position == BorderElementPosition.Start)
                {
                    c = fb.Parent.Parent;
                }
            }
            else if (currentElement is DomCharElement)
            {
            }
            MoveFocusHotKeys hotkeys = MoveFocusHotKeys.None;
            while (c != null)
            {
                if (c.AcceptTab)
                {
                    if (keyCode == System.Windows.Forms.Keys.Tab && shiftFlag == false)
                    {
                        // 容器元素能接受Tab键。
                        return null;
                    }
                }

                hotkeys = c.RuntimeMoveFocusHotKey;
                if (hotkeys != MoveFocusHotKeys.None)
                {
                    break;
                }

                c = c.Parent;
            }
            if (hotkeys == MoveFocusHotKeys.None)
            {
                return null;
            }
            DCTabMoveDirection dir = GetMoveFocusHotKeyDirection(hotkeys, keyCode, shiftFlag);
            if (dir == DCTabMoveDirection.Next || dir == DCTabMoveDirection.Forward)
            {
                DomElement nextElement = GetNextElement(currentElement, dir == DCTabMoveDirection.Forward);
                //if (nextElement != null)
                {
                    DomTableCellElement cell = currentElement.OwnerCell;
                    if (cell != null && shiftFlag == false)
                    {
                        DomTableCellElement cell2 = null;
                        if (nextElement != null)
                        {
                            cell2 = nextElement.OwnerCell;
                        }
                        if (cell != cell2)
                        {
                            // 当前单元格和下一个元素所在的单元格不一样，则判断是否可以在当前单元格的插入表格行的行为。
                            if (IsHotKeyForInsertNewRowDown(cell, keyCode))
                            {
                                // 当前单元格中按下快捷键是可以插入表格行的 
                                if (srcArgs != null)
                                {
                                    // 存在键盘事件参数对象，说明本函数是从控件的KeyDown事件中发起的，在此进行插入表格行
                                    if (ExeucteInsertRowDown(cell))
                                    {
                                        srcArgs.Handled = true;
                                        return null;
                                    }
                                }
                            }
                        }
                    }
                }
                if (nextElement != null && keyCode == Keys.Enter)
                {
                    WriterViewControl.EndTickForIgnoreEnterChar = WriterUtilsInner.TickCount;
                }
                return nextElement;
            }
            return null;
        }

        private bool ExeucteInsertRowDown(DomTableCellElement cell)
        {
            // 后续没有单元格，则新增表格行
            // 或者当前表格行允许按下Tab插入表格行而且当前单元格是最后一列的单元格。
            DomTableElement table = cell.OwnerTable;
            WriterControl ctl = cell.WriterControl;

            if (ctl.Readonly == false
                && ctl.DocumentControler.CanModifyContent(table, DomAccessFlags.Normal))
            {
                // 获得新增的表格行
                DomElementList newRows = (DomElementList)ctl.ExecuteCommandSpecifyRaiseFromUI(
                    StandardCommandNames.Table_InsertRowDown,
                    false,
                    null,
                    true);

                if (newRows != null && newRows.Count > 0)
                {
                    DomTableRowElement newRow = (DomTableRowElement)newRows[0];
                    DomTableCellElement nextCell = (DomTableCellElement)newRow.Cells[0];
                    if (nextCell.IsOverrided)
                    {
                        nextCell = nextCell.OverrideCell;
                    }
                    nextCell.Focus();
                    return true;
                }
            }
            return false;
        }
        private bool IsHotKeyForInsertNewRowDown(DomTableCellElement cell, System.Windows.Forms.Keys keyCode)
        {
            if (cell.OwnerDocument != null)
            {
                DomTableElement table = cell.OwnerTable;
                // 允许用户新增表格行
                if (cell.ColIndex + cell.ColSpan != table.ColumnsCount)
                {
                    // 不是所在表格行的最后一个单元格
                    return false;
                }
                DomTableRowElement row = cell.OwnerRow;
                if (cell == table.LastVisibleCell)
                {
                    // 单元格是表格中的最后一个单元格
                    return true;
                }
                else
                {
                    return false;
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// 检测是否命中移动焦点快键键
        /// </summary>
        /// <param name="mfk">快键键样式</param>
        /// <param name="keyCode">键盘按键值</param>
        /// <param name="shiftFlag"></param>
        /// <returns>是否命中</returns>
        private DCTabMoveDirection GetMoveFocusHotKeyDirection(
            MoveFocusHotKeys mfk,
            System.Windows.Forms.Keys keyCode,
            bool shiftFlag)
        {
            if (mfk != MoveFocusHotKeys.None && mfk != MoveFocusHotKeys.Default)
            {
                if ((mfk & MoveFocusHotKeys.Enter) == MoveFocusHotKeys.Enter
                    && keyCode == System.Windows.Forms.Keys.Enter)
                {
                    if (shiftFlag)
                    {
                        return DCTabMoveDirection.Forward;
                    }
                    else
                    {
                        return DCTabMoveDirection.Next;
                    }
                }
                if ((mfk & MoveFocusHotKeys.Tab) == MoveFocusHotKeys.Tab
                    && keyCode == System.Windows.Forms.Keys.Tab)
                {
                    if (shiftFlag)
                    {
                        return DCTabMoveDirection.Forward;
                    }
                    else
                    {
                        return DCTabMoveDirection.Next;
                    }
                }
            }
            return DCTabMoveDirection.None;
        }

        /// <summary>
        /// 获得Tab顺序中的下一个元素对象
        /// </summary>
        /// <param name="srcElement"></param>
        /// <param name="forward"></param>
        /// <returns></returns>
        public DomElement GetNextElement(DomElement srcElement, bool forward)
        {
            if (srcElement == null)
            {
                throw new ArgumentNullException("srcElement");
            }
            if (srcElement is DomFieldBorderElement && forward == false)
            {
                DomFieldBorderElement fb = (DomFieldBorderElement)srcElement;
                if (fb.Position == BorderElementPosition.Start)
                {
                    if (fb.OwnerField != null && fb.OwnerField.RuntimeTabStop())
                    {
                        return fb.OwnerField;
                    }
                }
            }
            ArrayList list = new ArrayList();
            DomElement p = srcElement;
            while (p != null)
            {
                list.Insert(0, p);
                if (p is DomDocumentContentElement)
                {
                    break;
                }
                p = p.Parent;
            }//while
            if (list.Count == 0)
            {
                return null;
            }
            MyElementEnumer enumer = new MyElementEnumer(list);
            enumer._ReverseMode = forward;
            foreach (DomElement element in enumer)
            {
                if (element.RuntimeVisible && list.Contains(element) == false)
                {
                    if (element is DomContainerElement)
                    {
                        DomContainerElement c = (DomContainerElement)element;
                        if (c.RuntimeTabStop())
                        {
                            // 进行表单模式判断
                            return c;
                        }
                    }
                }
            }//foreach
            return null;
        }

        private class MyElementEnumer : TreeNodeEnumerable
        {
            public MyElementEnumer(ArrayList list) : base(list, true)
            {
            }
            public bool _ReverseMode = false;
            public override IEnumerable GetChildNodes(object instance)
            {
                DomElementList list = null;
                if (instance is DomFieldElementBase)
                {
                    DomFieldElementBase field = (DomFieldElementBase)instance;
                    list = field.GetRuntimeElements();
                }
                else
                {
                    list = ((DomElement)instance).Elements;
                }
                if (this._ReverseMode && list != null)
                {
                    return new DCSoft.Common.ListReverseEnumrable(list);
                }
                return list;
            }
        }
    }
}
