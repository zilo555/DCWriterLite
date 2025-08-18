using System;
using System.Collections.Generic;
using System.Text;
using DCSoft.Writer.Dom ;

namespace DCSoft.Writer.Commands
{
    /// <summary>
    /// 合并单元格命令参数
    /// </summary>
    /// <remarks>编制 袁永福</remarks>
    //[System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = false)]
    //[DCSoft.Common.DCPublishAPI]
    public partial class MegeCellCommandParameter
    {
        /// <summary>
        /// 初始化对象
        /// </summary>
        public MegeCellCommandParameter()
        {
        }

        private string _CellID = null;
        /// <summary>
        /// 要处理的单元格编号
        /// </summary>
        //[System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public string CellID
        {
            get { return _CellID; }
            set { _CellID = value; }
        }

        private DomTableCellElement _Cell = null;
        /// <summary>
        /// 要处理的单元格对象
        /// </summary>
        //[System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public DomTableCellElement Cell
        {
            get { return _Cell; }
            set { _Cell = value; }
        }

        private int _NewRowSpan = 0;
        /// <summary>
        /// 新的跨行数
        /// </summary>
        //[System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public int NewRowSpan
        {
            get { return _NewRowSpan; }
            set { _NewRowSpan = value; }
        }

        private int _NewColSpan = 0;
        /// <summary>
        /// 新的跨列数
        /// </summary>
        //[System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public int NewColSpan
        {
            get { return _NewColSpan; }
            set { _NewColSpan = value; }
        }



        private int _NewRowSpanFix = 0;
        /// <summary>
        /// 新的跨行数修正
        /// </summary>
        //[System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public int NewRowSpanFix
        {
            get { return _NewRowSpanFix; }
            set { _NewRowSpanFix = value; }
        }

    }
}
