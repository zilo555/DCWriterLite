using System;
using System.Collections.Generic;
using System.Text;
using DCSoft.Writer.Dom;
using DCSoft.Writer.Controls;

namespace DCSoft.Writer.Dom.Undo
{
    internal class XTextUndoInvokeMethod : XTextUndoBase
    {
        public XTextUndoInvokeMethod(UndoMethodTypes method)
        {
            this._Method = method;
        }
        private UndoMethodTypes _Method = UndoMethodTypes.None;

        public override void Redo(Writer.Undo.XUndoEventArgs args)
        {
            this.OwnerList.NeedInvokeMethods = this.OwnerList.NeedInvokeMethods | this._Method;
        }
        public override void Undo(Writer.Undo.XUndoEventArgs args)
        {
            this.OwnerList.NeedInvokeMethods = this.OwnerList.NeedInvokeMethods | this._Method;
        }
    }
     
}
