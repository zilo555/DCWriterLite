using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;
// // 

namespace DCSoft.WinForms
{
    /// <summary>
    /// 用户界面工具箱
    /// </summary>
    public static class DCUIHelper
    {
        public static void ShowMessageBox(object p1, string msg)
        {
            MessageBox.Show(null, msg,DCSR.SystemAlert);
        }
        public static void ShowErrorMessageBox(object p1, string msg )
        {
            System.Windows.Forms.MessageBox.Show(null, msg , DCSR.SystemAlert);
        }
        public static void ShowWarringMessageBox(object p1, string msg)
        {
            System.Windows.Forms.MessageBox.Show(null, msg, DCSR.SystemAlert);
        }
    }
}
