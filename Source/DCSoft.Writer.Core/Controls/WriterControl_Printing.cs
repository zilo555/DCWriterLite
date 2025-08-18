using System;
using DCSoft.WinForms;
using DCSoft.Printing;
using DCSoft.Writer.Dom;
using DCSoft.Writer.Commands;
using DCSoft.Writer.Dom.Undo;
using DCSoft.Drawing;
using System.Windows.Forms;
// // 
using DCSoft.WinForms.Native;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Collections;
using System.Collections.Generic;
using DCSoft.Common;
using DCSoft.Writer.Undo;
using System.Text;
//using DCSoft.Writer.Printing;
using DCSoft.Writer.Data;
//using DCSoft.Writer.Script;
using System.Runtime.InteropServices;

namespace DCSoft.Writer.Controls
{
    /// <summary>
    /// 编辑器打印相关代码
    /// </summary>
    public partial class WriterControl
    {
        /// <summary>
        /// 页面设置
        /// </summary>
        public XPageSettings PageSettings
        {
            get
            {
                return this.GetInnerViewControl().PageSettings;                 
            }
            set
            {
                this.GetInnerViewControl().PageSettings = value;
            }
        }

    }
}
