using System;
using System.Collections.Generic;
using System.Text;
using DCSoft.Writer.Dom;
using DCSoft.Writer.Undo;

namespace DCSoft.Writer.Dom.Undo
{
    /// <summary>
    /// 重做、撤销表格结构信息
    /// </summary>
    internal class XTextUndoTableInfo : XTextUndoBase
    {
        public XTextUndoTableInfo()
        {
        }
        public override void Dispose()
        {
            base.Dispose();
            if(this._OldTableInfo != null )
            {
                this._OldTableInfo.Dispose();
                this._OldTableInfo = null;
            }
            if( this._NewTableInfo != null )
            {
                this._NewTableInfo.Dispose();
                this._NewTableInfo = null;
            }
        }

        private TableStructInfo _OldTableInfo = null;

        internal TableStructInfo OldTableInfo
        {
            get { return _OldTableInfo; }
            set { _OldTableInfo = value; }
        }

        private TableStructInfo _NewTableInfo = null;

        internal TableStructInfo NewTableInfo
        {
            get { return _NewTableInfo; }
            set { _NewTableInfo = value; }
        }

        /// <summary>
        /// 压缩对象其中保存的单元格子元素对象列表的备份信息
        /// </summary>
        internal void CompressElements()
        {
            if (_OldTableInfo != null && _NewTableInfo != null)
            {
                foreach (TableRowStructInfo row in _OldTableInfo.Rows)
                {
                    foreach (TableCellStructInfo cell in row.Cells)
                    {
                        foreach (TableRowStructInfo row2 in _NewTableInfo.Rows )
                        {
                            foreach (TableCellStructInfo cell2 in row2.Cells)
                            {
                                if (cell.CellInstance == cell2.CellInstance)
                                {
                                    DomElement[] es1 = cell.Elements;
                                    DomElement[] es2 = cell2.Elements;
                                    if (es1 != null
                                        && es2 != null 
                                        && es1.Length == es2.Length)
                                    {
                                        bool match = true ;
                                        for (int iCount = 0; iCount < es1.Length; iCount++)
                                        {
                                            if (es1[iCount] != es2[iCount])
                                            {
                                                match = false;
                                                break;
                                            }
                                        }
                                        if (match)
                                        {
                                            // 两个单元格的元素内容完全一样，无需保存相关信息
                                            cell.Elements = null;
                                            cell2.Elements = null;
                                        }
                                    }
                                    goto NextCell ;
                                }//if
                            }//foreach
                        }//foreach
                    NextCell: ;
                    }//foreach
                }//foreach
            }//if
        }

        public override void Undo(XUndoEventArgs args)
        {
            DomTableElement table = this.OldTableInfo.TableInstance;
            table.EditorApplyTableStructInfo(this.OldTableInfo);
        }

        public override void Redo(XUndoEventArgs args)
        {
            _NewTableInfo.TableInstance.EditorApplyTableStructInfo(this.NewTableInfo);
        }
    }
}
