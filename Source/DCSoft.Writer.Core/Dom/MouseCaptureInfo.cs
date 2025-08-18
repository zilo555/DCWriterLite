using System;

namespace DCSoft.Writer.Dom
{
	/// <summary>
	/// 鼠标捕获信息状态
	/// </summary>
    internal class MouseCaptureInfo
	{
        /// <summary>
        /// 初始化对象
        /// </summary>
        /// <param name="args"></param>
		public MouseCaptureInfo( DocumentEventArgs args )
		{
			intButton = args.Button ;
			intStartX = args.X ;
			intStartY = args.Y ;
			intLastX = args.X ;
			intLastY = args.Y ;
		}

        private bool _UsedFlag = false;
        /// <summary>
        /// 此对象发生
        /// </summary>
        public bool UsedFlag
        {
            get 
            {
                return _UsedFlag; 
            }
            set
            {
                _UsedFlag = value; 
            }
        }

		private readonly System.Windows.Forms.MouseButtons intButton = System.Windows.Forms.MouseButtons.None ;
		//      /// <summary>
		//      /// 鼠标按键值
		//      /// </summary>
		//public System.Windows.Forms.MouseButtons Button
		//{
		//	get{ return intButton ;}
		//	set{ intButton = value;}
		//}

		private readonly int intStartX = 0 ;
		//      /// <summary>
		//      /// 起始点X坐标
		//      /// </summary>
		//      public int StartX
		//{
		//	get{ return intStartX ;}
		//	set{ intStartX = value;}
		//}

		private readonly int intStartY = 0 ;
  //      /// <summary>
  //      /// 起始点Y坐标
  //      /// </summary>
		//public int StartY
		//{
		//	get{ return intStartY ;}
		//	//set{ intStartY = value;}
		//}

		private int intLastX = 0 ;
        /// <summary>
        /// 终点X坐标
        /// </summary>
		public int LastX 
		{
			//get{ return intLastX ;}
			set{ intLastX = value;}
		}

		private int intLastY = 0 ;
        /// <summary>
        /// 终点Y坐标
        /// </summary>
        public int LastY
		{
			//get{ return intLastY ;}
			set{ intLastY = value;}
		}

	}
}
