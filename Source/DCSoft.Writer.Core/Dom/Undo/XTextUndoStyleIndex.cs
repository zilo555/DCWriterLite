using System;
using System.Collections.Generic;
using System.Text;
using DCSoft.Writer.Undo;

namespace DCSoft.Writer.Dom.Undo
{
    /// <summary>
    /// 重复/撤销设置元素的样式编号
    /// </summary>
    internal class XTextUndoStyleIndex : XTextUndoBase
    {
        ///// <summary>
        ///// 初始化对象
        ///// </summary>
        //public XTextUndoStyleIndex()
        //{
        //}

        /// <summary>
        /// 初始化对象
        /// </summary>
        /// <param name="element">操作的文档元素</param>
        /// <param name="oldStyleIndex">旧样式编号</param>
        /// <param name="newStyleIndex">新样式编号</param>
        public XTextUndoStyleIndex(DomElement element, int oldStyleIndex, int newStyleIndex)
        {
            _Element = element;
            _OldStyleIndex = oldStyleIndex;
            _NewStyleIndex = newStyleIndex;
        }
        public override void Dispose()
        {
            base.Dispose();
            this._Element = null;
        }

        private DomElement _Element = null;

        //public XTextElement Element
        //{
        //    get { return _Element; }
        //    set { _Element = value; }
        //}

        private readonly int _OldStyleIndex = -1;

        //public int OldStyleIndex
        //{
        //    get { return _OldStyleIndex; }
        //    set { _OldStyleIndex = value; }
        //}

        private readonly int _NewStyleIndex = -1;

        //public int NewStyleIndex
        //{
        //    get { return _NewStyleIndex; }
        //    set { _NewStyleIndex = value; }
        //}
        /// <summary>
        /// 撤销操作
        /// </summary>
        /// <param name="args">参数</param>
        public override void Undo(XUndoEventArgs args)
        {
            _Element.StyleIndex = _OldStyleIndex;
            this.OwnerList.RefreshElements.Add(_Element);
        }

        public override void Redo(XUndoEventArgs args)
        {
            _Element.StyleIndex = _NewStyleIndex;
            this.OwnerList.RefreshElements.Add(_Element);
        }
    }
}
