using DCSoft.Printing;
using DCSoft.Writer.Dom;
using DCSoft.Drawing;
using System.Windows.Forms;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace DCSoft.Writer.Controls
{
    /// <summary>
    /// 编辑器打印相关代码
    /// </summary>
    public partial class WriterViewControl
    {
        /// <summary>
        /// 从0开始计算的获得焦点的当前页码号
        /// </summary>
        public int FocusedPageIndexBase0
        {
            get
            {
                if (this.CurrentTransformItem == null)
                {
                    return this.CurrentLineOwnerPageIndex();
                }
                else
                {
                    return this.CurrentTransformItem.PageIndex;
                }
            }
        }

        /// <summary>
        /// 页面设置
        /// </summary>
        public XPageSettings PageSettings
        {
            get
            {
                if (this._Document == null)
                {
                    return null;
                }
                else
                {
                    return this._Document.PageSettings;
                }
            }
            set
            {
                if (this._Document != null)
                {
                    this._Document.PageSettings = value;
                }
            }
        }
         
    }
}
