using System;
using System.Collections.Generic;
using System.Text;
using DCSoft.Writer.Dom;
//using DCSoft.Writer.Dom.Undo;
using DCSoft.Writer.Undo;

namespace DCSoft.Writer.Dom.Undo
{
    /// <summary>
    /// 设置文档属性
    /// </summary>
    /// <remarks>编制 袁永福</remarks>
    internal class XTextUndoSetDocumentProperty : XTextUndoBase
    {
        /// <summary>
        /// 初始化对象
        /// </summary>
        /// <param name="document">文档对象</param>
        /// <param name="propertyName">属性名称</param>
        /// <param name="oldValue">旧属性值</param>
        /// <param name="newValue">新属性值</param>
        public XTextUndoSetDocumentProperty(DomDocument document , string propertyName, object oldValue , object newValue)
        {
            _Document = document;
            _PropertyName = propertyName;
            _OldValue = oldValue;
            _NewValue = newValue;
        }
        public override void Dispose()
        {
            base.Dispose();
            this._Document = null;
            this._PropertyName = null;
            this._OldValue = null;
            this._NewValue = null;
        }
        private DomDocument _Document = null;
        private string _PropertyName = null;
        private object _NewValue = null;
        private object _OldValue = null;

        public override void Undo( XUndoEventArgs args)
        {
            switch (_PropertyName)
            {
                case "DefaultStyle":
                    _Document.EditorSetDefaultStyle((DocumentContentStyle)_OldValue, false);
                    break;
            }
        }

        public override void Redo(XUndoEventArgs args)
        {
            switch (_PropertyName)
            {
                case "DefaultStyle":
                    _Document.EditorSetDefaultStyle((DocumentContentStyle)_NewValue, false);
                    break;
            }
        }

    }
}
