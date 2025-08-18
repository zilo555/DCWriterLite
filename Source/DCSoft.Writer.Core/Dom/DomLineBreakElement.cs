using System;
using DCSoft.Drawing;


namespace DCSoft.Writer.Dom
{
	/// <summary>
	/// 换行元素
	/// </summary>
    /// <remarks>编制 袁永福</remarks>
    public sealed partial class DomLineBreakElement : DomElement
	{
		/// <summary>
		/// 初始化对象
		/// </summary>
        //[DCSoft.Common.DCPublishAPI]
        public DomLineBreakElement()
		{
			
		}
        public override void InnerGetText(GetTextArgs args)
        {
            args.AppendLine();
        }
        [NonSerialized]
        private bool _AutoGenerateFlag = false;
        /// <summary>
        /// 自动创建的标记
        /// </summary>
        public bool AutoGenerateFlag
        {
            get
            {
                return _AutoGenerateFlag;
            }
            set
            {
                _AutoGenerateFlag = value;
            }
        }

        /// <summary>
        /// 计算大小 
        /// </summary>
        /// <param name="PreElement"></param>
		internal void RefreshSize2( DomElement PreElement )
		{
			float h =  this.OwnerDocument.DefaultStyle.DefaultLineHeight ;
			if( PreElement != null && PreElement.Height > 0 )
			{
				h = PreElement.Height ;
			}
			h = Math.Max( h , this.OwnerDocument.PixelToDocumentUnit( 10 ));
			this.Height = h ;
			this.Width = this.OwnerDocument.PixelToDocumentUnit( 10 );
			this.SizeInvalid = false;
		}

		/// <summary>
		/// 返回对象包含的字符串数据
		/// </summary>
		/// <returns>字符串数据</returns>
         
        public override string ToString()
		{
			return System.Environment.NewLine ;
		}


        private static Image _LineBreakIcon = null;

        public override void RefreshSize(InnerDocumentPaintEventArgs args)
        {
            //var args = (DocumentPaintEventArgs)objArgs;
            float h = this.OwnerDocument.DefaultStyle.DefaultLineHeight;
            this.Height = h;
            this.Width = this.OwnerDocument.PixelToDocumentUnit(10);
            this.SizeInvalid = false;
        }

        public override void Draw(InnerDocumentPaintEventArgs args)
        {
            if (_LineBreakIcon == null)
            {
                _LineBreakIcon = DCStdImageList.BmpLinebreak;
            }
            var rect = this.GetAbsBounds();
            if (this.GetDocumentViewOptions().ShowParagraphFlag
               && args.RenderMode == InnerDocumentRenderMode.Paint)
            {
                //lock (_LineBreakIcon)
                {
                    var size = _LineBreakIcon.Size;
                    size = this.OwnerDocument.PixelToDocumentUnit(size);
                    args.Graphics.DrawImage(_LineBreakIcon, rect.Left, rect.Bottom - size.Height);
                }
            }
        }
	}
}