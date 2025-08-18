using System;
using System.Collections.Generic;
using System.Text;
// // 
//using System.Drawing.Printing;
using System.ComponentModel;

namespace DCSoft.Printing
{
    /// <summary>
    /// 打印选项
    /// </summary>
    public partial class DCPrintDocumentOptions
    {
        /// <summary>
        /// 初始化对象
        /// </summary>
        public DCPrintDocumentOptions()
        {
        }
        
        private PrintRange _PrintRange = PrintRange.AllPages;
        /// <summary>
        /// 打印区域
        /// </summary>
        //[System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public PrintRange PrintRange
        {
            get
            {
                return _PrintRange;
            }
            set
            {
                _PrintRange = value;
            }
        }
    }
}
