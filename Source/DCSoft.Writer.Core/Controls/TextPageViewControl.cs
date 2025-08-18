using DCSoft.Printing ;
using DCSoft.Drawing;

namespace DCSoft.Writer.Controls
{
    /// <summary>
    /// 支持文本编辑的分页视图控件
    /// </summary>
    public class TextPageViewControl : PageViewControl
	{
		/// <summary>
		/// 初始化对象
		/// </summary>
		public TextPageViewControl()
		{
			
		}

        #region 光标操作函数群 ************************************************

        /// <summary>
        /// 是否强制显示光标而不管控件是否获得输入焦点
        /// </summary>
        private bool _ForceShowCaret = false;
		/// <summary>
		/// 是否强制显示光标而不管控件是否获得输入焦点
		/// </summary>
        public bool ForceShowCaret
		{
			get
            {
                return _ForceShowCaret ;
            }
			set
            {
                _ForceShowCaret = value;
            }
		}

		/// <summary>
		/// 移动光标时是否自动滚动到光标区域
		/// </summary>
		protected bool	_MoveCaretWithScroll	= true;
		/// <summary>
		/// 移动光标时是否自动滚动到光标区域
		/// </summary>
        public bool MoveCaretWithScroll
		{
			get
            {
                return _MoveCaretWithScroll ;
            }
			set
            {
                _MoveCaretWithScroll = value;
            }
		}
		/// <summary>
		/// 当前是否处于插入模式,若处于插入模式,则光标比较细,否则处于改写模式,光标比较粗
		/// </summary>
		private bool	_InsertMode = true;
		/// <summary>
		/// 当前是否处于插入模式,若处于插入模式,则光标比较细,否则处于改写模式,光标比较粗
		/// </summary>
        public virtual bool InsertMode
		{
			get
            {
                return _InsertMode ;
            }
			set
            {
                _InsertMode = value;
            }
		}


		/// <summary>
		/// 默认光标宽度
		/// </summary>
		public static int DefaultCaretWidth = 2;
		/// <summary>
		/// 默认的宽光标的宽度,应用于修改模式中的文本编辑器
		/// </summary>
		public static int DefaultBroadCaretWidth = 6 ;
         
        /// <summary>
        /// 显示插入点光标
        /// </summary>
        public void ShowCaret()
        {

        }
        /// <summary>
        /// 隐藏光标
        /// </summary>
        protected void WASMHideCaret()
        {
            if (this is DCSoft.Writer.Controls.WriterViewControl)
            {
                ((WriterViewControl)this).WASMParent.JS_ShowCaret(0, 0, 0, 0, 0, false,false,false);
            }
        }
        /// <summary>
        /// 已重载:失去焦点,隐藏光标
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLostFocus(EventArgs e)
		{
            WASMHideCaret();
			base.OnLostFocus (e);
		}
         
		/// <summary>
		/// 移动光标到新的光标区域
		/// </summary>
		/// <param name="vLeft">在视图中光标区域的左端位置</param>
		/// <param name="vTop">在视图中光标区域的顶端位置</param>
		/// <param name="vWidth">光标区域宽度</param>
		/// <param name="vHeight">光标区域高度</param>
        /// <param name="contentWidth">内容区域宽度</param>
		/// <returns>移动光标是否造成滚动</returns>
		public bool MoveCaretTo(
            int vLeft , 
            int vTop , 
            int vWidth , 
            int vHeight ,
            int contentWidth ,
            bool bolReadonly )
		{
            if (this.ForceShowCaret == false
                && this.Focused == false)
            {
                WASMHideCaret();
                return false;
            }
			if( vWidth > 0 && vHeight > 0 )
			{
                {
                    if(this is DCSoft.Writer.Controls.WriterViewControl)
                    {
                        var ctl2 = (DCSoft.Writer.Controls.WriterViewControl)this;
                            var info = ctl2.GetClientPosInfo(vLeft, vTop);
                            if (info != null)
                            {
                                var curWidth = (int)(vWidth * ctl2.WASMZoomRate);
                                //if (ctl2.OwnerWriterControl.IsAPILogRecord)
                                //{
                                //    ctl2.OwnerWriterControl.APILogRecordDebugPrint(
                                //        "View(" + vLeft + "," + vTop + ")=>(Page:"
                                //        + info.PageIndex + ",X:" + info.dx + ",Y:" + info.dy + ")");
                                //}
                                var curHeight = (int)(ctl2.Document.ToPixel(vHeight) * ctl2.WASMZoomRate);
                                ctl2.WASMParent?.JS_ShowCaret(info.PageIndex,
                                    info.dx,
                                    info.dy,
                                    curWidth,
                                    curHeight,
                                    true,
                                    bolReadonly,
                                    this.MoveCaretWithScroll);
                            }
                    }
                    return this.MoveCaretWithScroll;
				}
			}
			return false;
		}
		
		/// <summary>
		/// 针对文本编辑器的移动光标到指定位置
		/// </summary>
		/// <param name="vLeft">光标的左端区域</param>
		/// <param name="vBottom">光标的底端区域</param>
		/// <param name="vHeight">光标的高度</param>
        /// <param name="contentWidth">内容宽度</param>
		/// <returns>移动光标是否造成滚动</returns>
		public bool MoveTextCaretTo( int vLeft , int vBottom , int vHeight , int contentWidth ,bool bolReadonly)
		{
			return MoveCaretTo(
				vLeft , 
				vBottom - vHeight , 
				( _InsertMode ? DefaultCaretWidth : DefaultBroadCaretWidth ) , 
				vHeight ,
                contentWidth ,
                bolReadonly);
		}

		/// <summary>
		/// 隐藏光标
		/// </summary>
		public void HideOwnerCaret()
		{
            WASMHideCaret();
		}
         
#endregion
	}//public class TextPageViewControl : PageScrollableControl
}