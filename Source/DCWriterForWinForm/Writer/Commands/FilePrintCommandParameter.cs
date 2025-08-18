
using System;
using System.Collections.Generic;
using System.Text;
using DCSoft.Printing ;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace DCSoft.Writer.Commands
{
    /// <summary>
    /// 打印功能命令参数
    /// </summary>
    //[System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = false)]
    //[DCSoft.Common.DCPublishAPI]
    public partial class FilePrintCommandParameter
    {
        /// <summary>
        /// 初始化对象
        /// </summary>
        public FilePrintCommandParameter()
        {
        }


        private bool _CleanPrintMode = false;
        /// <summary>
        /// 清洁打印模式
        /// </summary>
        //[System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public bool CleanPrintMode
        {
            get { return _CleanPrintMode; }
            set { _CleanPrintMode = value; }
        }


        private bool _DoubleBufferPrint = false;
        /// <summary>
        /// 获取或设置是否采用双缓冲打印模式
        /// </summary>
        //[System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public bool DoubleBufferPrint
        {
            get { return _DoubleBufferPrint; }
            set { _DoubleBufferPrint = value; }
        }


        private bool _ManualDuplex = false;
        /// <summary>
        /// 手动双面打印
        /// </summary>
        [DefaultValue(false)]
        //[System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public bool ManualDuplex
        {
            get
            {
                return _ManualDuplex;
            }
            set
            {
                _ManualDuplex = value;
            }
        }


        private string _SpecifyPageIndexsStringBase1 = null;
        /// <summary>
        /// 从1开始计算的指定打印页码合并在一起的字符串，各个页码之间用英文逗号分开。
        /// </summary>
        //[System.ComponentModel.Browsable(true)]
        //[System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public string SpecifyPageIndexsStringBase1
        {
            get
            {
                return _SpecifyPageIndexsStringBase1;
            }
            set
            {
                _SpecifyPageIndexsStringBase1 = value;
            }
        }

        private List<int> _SpecifyPageIndexs = null;
        /// <summary>
        /// 从0开始计算的指定打印的页码数
        /// </summary>
        public List<int> SpecifyPageIndexs
        {
            get
            {
                return _SpecifyPageIndexs;
            }
            set
            {
                _SpecifyPageIndexs = value;
            }
        }

        private int _SpecifyCopies = -1;
        /// <summary>
        /// 强制指定的打印份数,小于等于0则不强制指定
        /// </summary>
        public int SpecifyCopies
        {
            get
            {
                return _SpecifyCopies;
            }
            set
            {
                _SpecifyCopies = value;
            }
        }

        private string _SpecifyPrinterName = null;
        /// <summary>
        /// 指定的打印机名称
        /// </summary>
        //[System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public string SpecifyPrinterName
        {
            get
            {
                return _SpecifyPrinterName; 
            }
            set
            {
                _SpecifyPrinterName = value; 
            }
        }

        ///// <summary>
        ///// 添加指定的从0开始计算的页码
        ///// </summary>
        ///// <param name="pageIndex">页码</param>
        //[ComVisible( true )]
        ////[System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        //public void AddSpecifyPageIndex(int pageIndex)
        //{
        //    if (_SpecifyPageIndexs == null)
        //    {
        //        _SpecifyPageIndexs = new List<int>();
        //    }
        //    _SpecifyPageIndexs.Add(pageIndex);
        //}

        ///// <summary>
        ///// 设置指定的从1开始计算的页码数
        ///// </summary>
        ///// <param name="indexs">页码数组成的字符串，比如“1,3,5,7,9”。</param>
        ////[System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        //public void SetSpecifyPageIndexs(string indexsBase1)
        //{
        //    this._SpecifyPageIndexsStringBase1 = indexsBase1;
        //    if (string.IsNullOrEmpty(indexsBase1) == false)
        //    {
        //        List<int> values = new List<int>();
        //        foreach (string item in indexsBase1.Split(','))
        //        {
        //            int v = 0;
        //            if (int.TryParse(item, out v))
        //            {
        //                values.Add(v);
        //            }
        //        }
        //        _SpecifyPageIndexs = values;
        //    }
        //}
    }
}
