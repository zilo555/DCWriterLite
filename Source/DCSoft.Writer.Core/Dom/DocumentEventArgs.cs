using System;
using System.Runtime.InteropServices ;
//using System.Windows.Forms;

namespace DCSoft.Writer.Dom
{
	/// <summary>
	/// 文档用户界面事件类型
	/// </summary>
    /// <remarks>编制 袁永福</remarks>
    [System.Reflection.Obfuscation(Exclude = true,ApplyToMembers=true )]
    public enum DocumentEventStyles
	{
        /// <summary>
        /// 无效类型
        /// </summary>
		None ,
        /// <summary>
        /// 鼠标按键按下事件
        /// </summary>
		MouseDown ,
        /// <summary>
        /// 鼠标移动事件
        /// </summary>
		MouseMove ,
        /// <summary>
        /// 鼠标按键松开事件
        /// </summary>
		MouseUp ,
        /// <summary>
        /// 鼠标单击事件
        /// </summary>
        MouseClick ,
        /// <summary>
        /// 鼠标双击事件
        /// </summary>
        MouseDblClick,
        /// <summary>
        /// 键盘按键按下事件
        /// </summary>
		KeyDown ,
        /// <summary>
        /// 键盘字符事件
        /// </summary>
		KeyPress,
        /// <summary>
        /// 键盘按键松开事件
        /// </summary>
		KeyUp,
        /// <summary>
        /// 失去输入焦点事件
        /// </summary>
        LostFocus,
        /// <summary>
        /// 由于控件失去输入焦点而触发的文档元素失去输入焦点事件
        /// </summary>
        ControlLostFoucs,
        /// <summary>
        /// 增强型的失去输入焦点事件
        /// </summary>
        LostFocusExt ,
        /// <summary>
        /// 获得输入焦点事件
        /// </summary>
        GotFocus,
        /// <summary>
        /// 由于控件获得输入焦点而触发的文档元素获得输入焦点事件
        /// </summary>
        ControlGotFoucs,
        /// <summary>
        /// 鼠标光标进入事件
        /// </summary>
        MouseEnter ,
        /// <summary>
        /// 鼠标光标离开事件
        /// </summary>
        MouseLeave ,
        /// <summary>
        /// 执行默认编辑方法
        /// </summary>
        DefaultEditMethod
	}

    /// <summary>
    /// 文档事件委托类型
    /// </summary>
    /// <param name="eventSender">参数</param>
    /// <param name="args">参数</param>
    [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true )]
    public delegate void DocumentEventHandelr(object eventSender, DocumentEventArgs args);

	/// <summary>
	/// 文档事件参数
	/// </summary>
    [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = false  )]
    public partial class DocumentEventArgs
	{
        /// <summary>
        /// 默认鼠标光标对象
        /// </summary>
		public static System.Windows.Forms.Cursor DefaultCursor 
            = System.Windows.Forms.Cursors.IBeam ;

		/// <summary>
		/// 创建鼠标按键按下事件参数
		/// </summary>
		/// <param name="doc">文档对象</param>
		/// <param name="e">原始事件参数</param>
		/// <returns>创建的参数</returns>
         
        public static DocumentEventArgs CreateMouseDown(
			DomDocument doc ,
			System.Windows.Forms.MouseEventArgs e )
		{
			return CreateMouseEvent( doc , e , DocumentEventStyles.MouseDown );
		}
		/// <summary>
		/// 创建键盘按键按下事件参数对象
		/// </summary>
		/// <param name="doc">文档对象</param>
		/// <param name="e">原始事件参数</param>
		/// <returns>创建的事件参数对象</returns>
         
        public static DocumentEventArgs CreateKeyDown( 
			DomDocument doc ,
			System.Windows.Forms.KeyEventArgs e )
		{
			DocumentEventArgs args = new DocumentEventArgs();
			args.myDocument = doc ;
			args.bolAltKey = e.Alt ;
			args.bolCtlKey = e.Control ;
			args.bolShiftKey = e.Shift ;
			args._KeyCode = e.KeyCode ;
			args.intKeyChar = ( char ) e.KeyCode ;
			args.intStyle = DocumentEventStyles.KeyDown ;
			return args ;
		}

		/// <summary>
		/// 创建键盘按键按下事件参数对象
		/// </summary>
		/// <param name="doc">文档对象</param>
		/// <param name="e">原始事件参数</param>
		/// <returns>创建的事件参数对象</returns>
         
        public static DocumentEventArgs CreateKeyPress( 
			DomDocument doc ,
			System.Windows.Forms.KeyPressEventArgs e )
		{
			DocumentEventArgs args = new DocumentEventArgs();
			args.myDocument = doc ;
			args.UpdateKeyState();
			args.intKeyChar = e.KeyChar ;
			args.intStyle = DocumentEventStyles.KeyPress ;
			return args ;
		}

        /// <summary>
        /// 创建键盘按键按下事件参数对象
        /// </summary>
        /// <param name="doc">文档对象</param>
        /// <param name="chr">字符值</param>
        /// <returns>创建的事件参数对象</returns>
         
        public static DocumentEventArgs CreateKeyPress(
            DomDocument doc,
            char chr )
        {
            DocumentEventArgs args = new DocumentEventArgs();
            args.myDocument = doc;
            args.UpdateKeyState();
            args.intKeyChar = chr;
            args.intStyle = DocumentEventStyles.KeyPress;
            return args;
        }

		/// <summary>
		/// 创建键盘按键松开事件参数对象
		/// </summary>
		/// <param name="doc">文档对象</param>
		/// <param name="e">原始事件参数</param>
		/// <returns>创建的事件参数对象</returns>
         
        public static DocumentEventArgs CreateKeyUp( 
			DomDocument doc ,
			System.Windows.Forms.KeyEventArgs e )
		{
			DocumentEventArgs args = new DocumentEventArgs();
			args.myDocument = doc ;
			args.bolAltKey = e.Alt ;
			args.bolCtlKey = e.Control ;
			args.bolShiftKey = e.Shift ;
			args._KeyCode = e.KeyCode ;
			args.intKeyChar = ( char ) e.KeyCode ;
			args.intStyle = DocumentEventStyles.KeyUp ;
			return args ;
		}

         
        private static DocumentEventArgs CreateMouseEvent(
			DomDocument doc ,
			System.Windows.Forms.MouseEventArgs e ,
			DocumentEventStyles style )
		{
			DocumentEventArgs args = new DocumentEventArgs();
            args._MouseClicks = e.Clicks;
			args.myDocument = doc ;
			args.intX = e.X ;
			args.intY = e.Y ;
            args._ViewX = e.X;
            args._ViewY = e.Y;
			args._Button = e.Button ;
			args.intWheelDelta = e.Delta ;
			args.UpdateKeyState();
			args.intStyle =style ;
			return args ;
		}

        /// <summary>
        /// 初始化对象
        /// </summary>
        /// <param name="document">文档对象</param>
        /// <param name="element">文档元素对象</param>
        /// <param name="style">事件类型</param>
         
        public DocumentEventArgs(DomDocument document, DomElement element, DocumentEventStyles style)
        {
            this.myDocument = document;
            this._Element = element;
            this.intStyle = style;
            this.UpdateKeyState();
        }

		/// <summary>
		/// 内部使用的构造函数
		/// </summary>
		internal DocumentEventArgs()
		{
			_Cursor = DefaultCursor ;
			strTooltip = null;
		}


        private void UpdateKeyState()
		{
			System.Windows.Forms.Keys key = 
                System.Windows.Forms.Control.ModifierKeys ;
			bolAltKey = ( ( key & System.Windows.Forms.Keys.Shift) != 0);
			bolCtlKey = ( ( key & System.Windows.Forms.Keys.Control ) != 0 );
			bolShiftKey = ( ( key & System.Windows.Forms.Keys.Shift ) != 0 );
		}



		internal DocumentEventStyles intStyle = DocumentEventStyles.None ;
        /// <summary>
        /// 文档事件类型
        /// </summary>
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public DocumentEventStyles Style
		{
			get
            { 
                return intStyle ;
            }
		}

		private DomDocument myDocument = null;
        /// <summary>
        /// 对象所在文档对象
        /// </summary>
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public DomDocument Document
		{
			get
            {
                return myDocument ;
            }
		}

        private DomElement _Element = null;
        /// <summary>
        /// 事件相关的文档元素对象
        /// </summary>
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public DomElement Element
        {
            get { return _Element; }
            set { _Element = value; }
        }

		internal string strName = null;
        /// <summary>
        /// 事件名称
        /// </summary>
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public string Name
		{
			get
            { 
                return strName ;
            }
		}

		private bool bolAltKey = false;
        /// <summary>
        /// 用户是否按下了 Alt 键
        /// </summary>
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public bool AltKey
		{
			get
            { 
                return bolAltKey ;
            }
       }

		private bool bolCtlKey = false;
        /// <summary>
        /// 用户是否按下的 Ctl 键
        /// </summary>
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public bool CtlKey 
		{
			get
            { 
                return bolCtlKey ;
            }
		}

		private bool bolShiftKey = false;
        /// <summary>
        /// 用户是否按下了 Shift 键
        /// </summary>
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public bool ShiftKey
		{
			get
            {
                return bolShiftKey ;
            }
		}
        private System.Windows.Forms.Keys _KeyCode = System.Windows.Forms.Keys.None;
        /// <summary>
        /// 按键值
        /// </summary>
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public System.Windows.Forms.Keys KeyCode
        {
            get
            {
                return _KeyCode; 
            }
        }


		internal char intKeyChar = char.MinValue ;
		/// <summary>
		/// 键盘字符值
		/// </summary>
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public char KeyChar
		{
			get
            { 
                return intKeyChar ;
            }
		}

        /// <summary>
        /// 键盘字符值
        /// </summary>
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public int KeyCharValue
        {
            get
            {
                return (int)intKeyChar;
            }
        }

        private bool _CancelBubble = false;
        /// <summary>
        /// 取消事件冒泡
        /// </summary>
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public bool CancelBubble
        {
            get
            {
                return _CancelBubble; 
            }
            set
            {
                _CancelBubble = value; 
            }
        }

		private bool _Handled = false;
        /// <summary>
        /// 事件已经处理了
        /// </summary>
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public bool Handled
		{
			get
            {
                return _Handled ;
            }
			set
            { 
                _Handled = value;
            }
		}

        private bool _AlreadSetSelection = false;
        /// <summary>
        /// 已经设置了文档内容选择区域，无需自动设置选择区域
        /// </summary>
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public bool AlreadSetSelection
        {
            get
            {
                return _AlreadSetSelection; 
            }
            set
            {
                _AlreadSetSelection = value; 
            }
        }

        /// <summary>
        /// 鼠标光标坐标转换时出现了严格命中
        /// </summary>
        internal bool _StrictMatch = true;
        /// <summary>
        /// 鼠标光标坐标转换时出现了严格命中
        /// </summary>
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public bool StrictMatch
        {
            get { return _StrictMatch; }
        }

        private int _MouseClicks = 0;
        /// <summary>
        /// 鼠标点击次数
        /// </summary>
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public int MouseClicks
        {
            get { return _MouseClicks; }
            set { _MouseClicks = value; }
        }

		internal int intClientX = 0 ;
        /// <summary>
        /// 鼠标在文档控件客户区的X坐标
        /// </summary>
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public int ClientX
		{
			get
            { 
                return intClientX ;
            }
		}

		internal int intClientY = 0 ;
        /// <summary>
        /// 鼠标在文档控件客户区的Y坐标
        /// </summary>
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public int ClientY 
		{
			get
            { 
                return intClientY ;
            }
		}

        internal float _ViewX = 0;

        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public float ViewX
        {
            get { return _ViewX; }
        }

        internal float _ViewY = 0;

        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public float ViewY
        {
            get { return _ViewY; }
        }

		internal int intX = 0 ;
        /// <summary>
        /// 鼠标光标在视图中的X坐标
        /// </summary>
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public int X
		{
			get
            { 
                return intX ;
            }
		}

		internal int intY = 0 ;
        /// <summary>
        /// 鼠标光标在视图中的Y坐标
        /// </summary>
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public int Y
		{
			get
            { 
                return intY ;
            }
		}
        private System.Windows.Forms.MouseButtons _Button = System.Windows.Forms.MouseButtons.None;
        /// <summary>
        /// 鼠标按键值
        /// </summary>
       ////[System.ComponentModel.Browsable(false)]
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public System.Windows.Forms.MouseButtons Button
        {
            get
            {
                return _Button;
            }
        }


		internal int intWheelDelta = 0 ;
        /// <summary>
        /// 鼠标滚轮值
        /// </summary>
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public int WheelDelta
		{
			get
            { 
                return intWheelDelta ;
            }
		}

		private object objReturnValue = null;
        /// <summary>
        /// 事件返回值
        /// </summary>
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public object ReturnValue
		{
			get
            { 
                return objReturnValue ;
            }
			set
            { 
                objReturnValue = value;
            }
		}
        internal static System.Windows.Forms.Cursor _Cursor = null;
		/// <summary>
		/// 视图区鼠标光标对象
		/// </summary>
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public System.Windows.Forms.Cursor Cursor
		{
			get
            { 
                return _Cursor ;
            }
			set
            { 
                _Cursor = value;
            }
		}

        internal System.Windows.Forms.Cursor RuntimeCursor()
        {
            if (_Cursor != null)
            {
                return _Cursor;
            }
            if (DefaultCursor != null)
            {
                return DefaultCursor;
            }
            return System.Windows.Forms.Cursors.IBeam;

        }
		/// <summary>
		/// 提示文本
		/// </summary>
		internal static string strTooltip = null;
        /// <summary>
        /// 提示文本
        /// </summary>
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public string Tooltip
		{
			get
            { 
                return strTooltip ;
            }
			set
            {
                strTooltip = value;
            }
		}


		/// <summary>
		/// 复制对象
		/// </summary>
		/// <returns>对象复制品</returns>
        public DocumentEventArgs Clone()
		{
            DocumentEventArgs args = (DocumentEventArgs)this.MemberwiseClone();
			//args.myCursor = this.myCursor ;
			return args ;
		}
	}
     
}