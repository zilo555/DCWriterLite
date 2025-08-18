using System;
using System.Collections.Generic;
using System.Text;
using DCSoft.Drawing;
using DCSoft.Writer.Dom;
using DCSoft.Writer.Controls ;
namespace DCSoft.Writer.Dom.Undo
{
    /// <summary>
    /// 设置默认字符撤销信息对象
    /// </summary>
    public class XTextUndoSetDefaultFont : XTextUndoBase
    {
        /// <summary>
        /// 初始化对象
        /// </summary>
        /// <param name="ctl">编辑器控件对象</param>
        /// <param name="of">旧字体</param>
        /// <param name="oc">旧文本颜色</param>
        /// <param name="nf">新字体</param>
        /// <param name="nc">新文本颜色</param>
        public XTextUndoSetDefaultFont( WriterControl ctl , XFontValue of, Color oc, XFontValue nf, Color nc)
        {
            _Control = ctl;
            _OldFont = of;
            _OldColor = oc;
            _NewFont = nf;
            _NewColor = nc;
        }
        public override void Dispose()
        {
            base.Dispose();
            this._Control = null;
            this._OldFont = null;
            this._NewFont = null;
        }
        private WriterControl _Control = null;
        private XFontValue _OldFont = null;
        private Color _OldColor = Color.Empty;
        private XFontValue _NewFont = null;
        private Color _NewColor = Color.Empty;
        /// <summary>
        /// 撤销操作
        /// </summary>
        /// <param name="args">参数</param>
        public override void Undo(Writer.Undo.XUndoEventArgs args)
        {
            _Control.EditorSetDefaultFont(_OldFont, _OldColor, true);
        }
        /// <summary>
        /// 重做操作
        /// </summary>
        /// <param name="args">参数</param>
        public override void Redo(Writer.Undo.XUndoEventArgs args)
        {
            _Control.EditorSetDefaultFont(_NewFont, _NewColor, true);
        }
    }
}
