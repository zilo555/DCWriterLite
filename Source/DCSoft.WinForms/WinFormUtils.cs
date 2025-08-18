using System;
using System.Collections.Generic;
using System.Text;
using DCSoft.WinForms.Native;
using System.Windows.Forms;
using DCSoft.Drawing ;
// // 
using DCSoft.Common;
//using System.Collections.Specialized;
using System.IO;
using DCSystemXml;
using System.Reflection;
using System.ComponentModel;


namespace DCSoft.WinForms
{
    /// <summary>
    /// WinForm窗体工具类
    /// </summary>
    public static class WinFormUtils
    {
        /// <summary>
        /// 设置窗体的默认字体
        /// </summary>
        /// <param name="frm">窗体对象</param>
        public static void SetFormDefaultFont( System.Windows.Forms.Control frm )
        {
            if( frm != null )
            {
                frm.Font = FormDefaultFont;
            }
        }
        /// <summary>
        /// 窗体的默认字体
        /// </summary>
        internal static readonly Font FormDefaultFont = new Font("宋体", 9);
    }
}
