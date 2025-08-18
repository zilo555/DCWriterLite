using System;
using System.Collections.Generic;
using System.Text;
using DCSoft.Writer.Dom;

namespace DCSoft.Writer.Commands
{
    /// <summary>
    /// 执行拆分单元格命令参数
    /// </summary>
     
    //[System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = false)]
    //[DCSoft.Common.DCPublishAPI]
    public partial class SplitCellExtCommandParameter
    {
        /// <summary>
        /// 初始化对象
        /// </summary>
        public SplitCellExtCommandParameter()
        {
        }

        private string _CellID = null;
        /// <summary>
        /// 指定的单元格编号
        /// </summary>
        //[System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public string CellID
        {
            get { return _CellID; }
            set { _CellID = value; }
        }

        private DomTableCellElement _CellElement = null;
        /// <summary>
        /// 指定的单元格对象
        /// </summary>
        //[System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public DomTableCellElement CellElement
        {
            get { return _CellElement; }
            set { _CellElement = value; }
        }

        private int _NewRowSpan = -1;
        /// <summary>
        /// 新的跨行数
        /// </summary>
        //[System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public int NewRowSpan
        {
            get { return _NewRowSpan; }
            set { _NewRowSpan = value; }
        }

        private int _NewColSpan = -1;
        /// <summary>
        /// 新的跨列数
        /// </summary>
        //[System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public int NewColSpan
        {
            get { return _NewColSpan; }
            set { _NewColSpan = value; }
        }
    }
}
