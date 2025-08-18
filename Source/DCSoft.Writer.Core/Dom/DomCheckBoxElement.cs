using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel ;
//using System.Drawing ;
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
    [System.Diagnostics.DebuggerDisplay("CheckBox:Group={GroupName} , Checked={Checked}")]
#endif
    public sealed partial class DomCheckBoxElement : DomCheckBoxElementBase
    {
        public static readonly string TypeName_XTextCheckBoxElement = "XTextCheckBoxElement";
        public override string TypeName => TypeName_XTextCheckBoxElement;

        /// <summary>
        /// 初始化对象
        /// </summary>
        public DomCheckBoxElement()
        {
        }

       
        /// <summary>
        /// 文档元素编号前缀
        /// </summary>
         
        public override string ElementIDPrefix()
        {
            
                return "chk";
             
        }

    }
}
