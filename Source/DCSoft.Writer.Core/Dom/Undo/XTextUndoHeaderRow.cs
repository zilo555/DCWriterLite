using System;
using System.Collections.Generic;
using System.Text;

namespace DCSoft.Writer.Dom.Undo
{
    internal class XTextUndoHeaderRow : XTextUndoBase
    {
        public XTextUndoHeaderRow()
        {

        }
        public override void Dispose()
        {
            base.Dispose();
            this._Table = null;
            if(this._OldHeaderStyles != null)
            {
                this._OldHeaderStyles.Clear();
                this._OldHeaderStyles = null;
            }
            if(this._NewHeaderStyles != null )
            {
                this._NewHeaderStyles.Clear();
                this._NewHeaderStyles = null;
            }

        }
        private DomTableElement _Table = null;

        public DomTableElement Table
        {
            get { return _Table; }
            set { _Table = value; }
        }

        private Dictionary<DomTableRowElement, bool> _OldHeaderStyles = new Dictionary<DomTableRowElement, bool>();

        public Dictionary<DomTableRowElement, bool> OldHeaderStyles
        {
            get { return _OldHeaderStyles; }
            //set { _OldHeaderStyles = value; }
        }

        private Dictionary<DomTableRowElement, bool> _NewHeaderStyles = new Dictionary<DomTableRowElement, bool>();
        /// <summary>
        /// 新标题行样式
        /// </summary>
        public Dictionary<DomTableRowElement, bool> NewHeaderStyles
        {
            get { return _NewHeaderStyles; }
            //set { _NewHeaderStyles = value; }
        }

        public override void Undo(Writer.Undo.XUndoEventArgs args)
        {
            this.Table.EditorSetHeaderRow(this.OldHeaderStyles, false);
        }

        public override void Redo(Writer.Undo.XUndoEventArgs args)
        {
            this.Table.EditorSetHeaderRow(this.NewHeaderStyles, false);
        }

    }
}
