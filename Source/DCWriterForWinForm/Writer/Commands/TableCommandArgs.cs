using System;
using System.Collections.Generic;
using System.Text;
using DCSoft.Writer.Dom ;

namespace DCSoft.Writer.Commands
{
    /// <summary>
    /// 表格命令的指定参数
    /// </summary>
    /// <remarks>编制 袁永福</remarks>
     
    //[System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = false)]
    //[DCSoft.Common.DCPublishAPI]
    public partial class TableCommandArgs
    {
        /// <summary>
        /// 初始化对象
        /// </summary>
        public TableCommandArgs()
        {
        }

        private string _TableID = null;
        /// <summary>
        /// 要操作的表格编号
        /// </summary>
        //[System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public string TableID
        {
            get
            { 
                return _TableID;
            }
            set
            {
                _TableID = value;
            }
        }

        private DomTableElement _TableElement = null;
        /// <summary>
        /// 要操作的表格对象
        /// </summary>
        //[System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public DomTableElement TableElement
        {
            get
            {
                return _TableElement; 
            }
            set
            {
                _TableElement = value;
            }
        }

        private int _RowIndex = -1;
        /// <summary>
        /// 指定的行号
        /// </summary>
        //[System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public int RowIndex
        {
            get
            {
                return _RowIndex;
            }
            set
            {
                _RowIndex = value;
            }
        }

        private int _ColIndex = -1;
        /// <summary>
        /// 指定的列号
        /// </summary>
        //[System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public int ColIndex
        {
            get
            {
                return _ColIndex; 
            }
            set
            {
                _ColIndex = value;
            }
        }

        private int _RowsCount = 1;
        /// <summary>
        /// 新增或删除的行数
        /// </summary>
        //[System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public int RowsCount
        {
            get
            {
                return _RowsCount; 
            }
            set
            {
                _RowsCount = value; 
            }
        }

        private int _ColsCount = 1;
        /// <summary>
        /// 新增或删除的列数
        /// </summary>
        //[System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public int ColsCount
        {
            get
            {
                return _ColsCount; 
            }
            set
            {
                _ColsCount = value; 
            }
        }

        private bool _Result = false  ;
        /// <summary>
        /// 操作执行结果
        /// </summary>
        //[System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public bool Result
        {
            get { return _Result; }
            set { _Result = value; }
        }
    }
}
