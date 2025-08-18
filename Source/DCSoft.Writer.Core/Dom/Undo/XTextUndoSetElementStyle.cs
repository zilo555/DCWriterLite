using System;
using System.Collections.Generic;
using System.Text;

namespace DCSoft.Writer.Dom.Undo
{
    /// <summary>
    /// 撤销、重复设置段落样式的对象
    /// </summary>
    internal class XTextUndoSetElementStyle : XTextUndoBase
    {
        /// <summary>
        /// 初始化对象
        /// </summary>
        public XTextUndoSetElementStyle()
        {
        }
        public override void Dispose()
        {
            base.Dispose();
            if (this._OldStyleIndex != null)
            {
                this._OldStyleIndex.Clear();
                this._OldStyleIndex = null;
            }
            if (this._NewStyleIndex != null)
            {
                this._NewStyleIndex.Clear();
                this._NewStyleIndex = null;
            }
        }

        private bool _ParagraphStyle = false;
        /// <summary>
        /// 设置段落样式
        /// </summary>
        public bool ParagraphStyle
        {
            get { return _ParagraphStyle; }
            set { _ParagraphStyle = value; }
        }

        private bool _CauseUpdateLayout = true;

        public bool CauseUpdateLayout
        {
            get { return _CauseUpdateLayout; }
            set { _CauseUpdateLayout = value; }
        }
        private string _CommandName = null;

        public string CommandName
        {
            //get { return _CommandName; }
            set { _CommandName = value; }
        }

        private Dictionary<DomElement, int> _OldStyleIndex 
            = new Dictionary<DomElement, int>();
        private Dictionary<DomElement, int> _NewStyleIndex
            = new Dictionary<DomElement, int>();

        public void AddInfo(DomElement element, int oldStyleIndex, int newStyleIndex)
        {
            _OldStyleIndex[element] = oldStyleIndex;
            _NewStyleIndex[element] = newStyleIndex;
        }

        /// <summary>
        /// 重做操作
        /// </summary>
        /// <param name="args"></param>
        public override void Redo(Writer.Undo.XUndoEventArgs args)
        {
            if (_NewStyleIndex.Count > 0)
            {
                if (this.ParagraphStyle)
                {
                    this.Document.EditorSetParagraphStyle(_NewStyleIndex, false);
                }
                else
                {
                    this.Document.EditorSetElementStyle(
                        _NewStyleIndex, 
                        false , 
                        this.CauseUpdateLayout , 
                        this._CommandName );
                }
            }
        }
        /// <summary>
        /// 撤销操作
        /// </summary>
        /// <param name="args"></param>
        public override void Undo(Writer.Undo.XUndoEventArgs args)
        {
            if (_OldStyleIndex.Count > 0)
            {
                if (this.ParagraphStyle)
                {
                    this.Document.EditorSetParagraphStyle(_OldStyleIndex, false);
                }
                else
                {
                    this.Document.EditorSetElementStyle(
                        _OldStyleIndex, 
                        false , 
                        this.CauseUpdateLayout  ,
                        this._CommandName );
                }
            }
        }
    }
}
