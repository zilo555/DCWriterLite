using System;
using System.Collections.Generic;
using System.Text;

namespace DCSoft.Writer.Undo
{
    /// <summary>
    /// 重做/撤销操作事件参数
    /// </summary>
    public class XUndoEventArgs  
    {
        /// <summary>
        /// 初始化对象
        /// </summary>
        public XUndoEventArgs()
        {
        }

        private Dictionary<string, object> _Parameters = new Dictionary<string, object>();
        /// <summary>
        /// 参数
        /// </summary>
        public Dictionary<string, object> Parameters
        {
            get { return _Parameters; }
            set { _Parameters = value; }
        }

        private bool _Cancel = false;
        /// <summary>
        /// 取消操作
        /// </summary>
        public bool Cancel
        {
            get { return _Cancel; }
            set { _Cancel = value; }
        }
    }
}
