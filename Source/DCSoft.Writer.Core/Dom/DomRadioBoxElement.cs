using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using DCSoft.Drawing;

using DCSoft.Common;
using DCSoft.Writer.Data;
using System.Runtime.InteropServices;


namespace DCSoft.Writer.Dom
{
    /// <summary>
    /// 复选框控件
    /// </summary>
#if !RELEASE
    [System.Diagnostics.DebuggerDisplay("Radio:Name={Name} , Checked={Checked}")]
#endif
    public sealed partial class DomRadioBoxElement : DomCheckBoxElementBase
    {
        public static readonly string TypeName_XTextRadioBoxElement = "XTextRadioBoxElement";
        public override string TypeName => TypeName_XTextRadioBoxElement;

        /// <summary>
        /// 初始化对象
        /// </summary>
        //[DCSoft.Common.DCPublishAPI]
        public DomRadioBoxElement()
        {
        }

        /// <summary>
        /// 文档元素编号前缀
        /// </summary>
        public override string ElementIDPrefix()
        {
             
                return "rdo";
            
        }
    }
}
