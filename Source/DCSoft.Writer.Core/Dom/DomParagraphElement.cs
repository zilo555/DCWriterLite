using System;

// // 
using DCSoft.Drawing;

namespace DCSoft.Writer.Dom
{
	/// <summary>
	/// 段落对象
	/// </summary>
    /// <remarks >本对象只在加载或保存文档是临时生成。</remarks>
    public partial class DomParagraphElement : DomContainerElement
	{
		/// <summary>
		/// 初始化对象
		/// </summary>
		public DomParagraphElement(  )
		{
		}

        private bool _TemporaryFlag = false ;
        /// <summary>
        /// 临时标记
        /// </summary>
        public bool TemporaryFlag
        {
            get
            {
                return _TemporaryFlag; 
            }
            set
            {
                _TemporaryFlag = value; 
            }
        }
        public DomTableCellElement _OwnerTableCell = null;

        public DomParagraphFlagElement InnerFlagElement
        {
            get
            {
                if (this.Elements != null)
                {
                    return this.Elements.LastElement as DomParagraphFlagElement;
                }
                return null;
            }
        }

	}

}