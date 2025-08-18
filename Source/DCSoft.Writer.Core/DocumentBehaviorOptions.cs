using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Runtime.InteropServices;
using DCSoft.Common;
using DCSoft.Writer.Controls;

namespace DCSoft.Writer
{
    /// <summary>
    /// 文档行为设置
    /// </summary>
    /// <remarks>编制 袁永福</remarks>
    [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = false)]
    public partial class DocumentBehaviorOptions : ICloneable
    {
        /// <summary>
        /// 初始化对象
        /// </summary>
        public DocumentBehaviorOptions()
        {
        }


        private bool _ResetTextFormatWhenCreateNewLine = false;
        /// <summary>
        /// 创建新行时是否重置文本样式（设置字体为默认字体，去除边框和颜色）。
        /// </summary>
        [DefaultValue(false)]
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public bool ResetTextFormatWhenCreateNewLine
        {
            get
            {
                return this._ResetTextFormatWhenCreateNewLine;
            }
            set
            {
                this._ResetTextFormatWhenCreateNewLine = value;
            }
        }

        private bool _SelectionTextIncludeBackgroundText = true;
        /// <summary>
        /// 选择区域的文本是否包含输入域背景文本
        /// </summary>
        [DefaultValue(true)]
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public bool SelectionTextIncludeBackgroundText
        {
            get
            {
                return this._SelectionTextIncludeBackgroundText;
            }
            set
            {
                this._SelectionTextIncludeBackgroundText = value;
            }
        }

        private bool _IgnoreTopBottomPaddingWhenGridLineLayout = false;
        /// <summary>
        /// 在使用文档网格线排版时忽略掉容器元素的上下内边距设置
        /// </summary>
        [DefaultValue(false)]
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public bool IgnoreTopBottomPaddingWhenGridLineLayout
        {
            get
            {
                return _IgnoreTopBottomPaddingWhenGridLineLayout;
            }
            set
            {
                _IgnoreTopBottomPaddingWhenGridLineLayout = value;
            }
        }

        private bool _SaveBodyTextToXml = true;
        /// <summary>
        /// 保存文档对象的BodyText到XML文件中
        /// </summary>
        [DefaultValue(true)]
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public bool SaveBodyTextToXml
        {
            get
            {
                return this._SaveBodyTextToXml;
            }
            set
            {
                this._SaveBodyTextToXml = value;
            }
        }

        private bool _ForceRaiseEventAfterFieldContentEdit = false;
        /// <summary>
        /// 在弹出输入域下拉列表时，用户确认操作，即使没有修改输入域的内容，仍然强制触发编辑器控件的EventAfterFieldContentEdit事件。默认为false。
        /// </summary>
        [DefaultValue( false )]
        [System.Reflection.Obfuscation(Exclude = true , ApplyToMembers = true)]
        public bool ForceRaiseEventAfterFieldContentEdit 
        {
            get
            {
                return _ForceRaiseEventAfterFieldContentEdit;
            }
            set
            {
                _ForceRaiseEventAfterFieldContentEdit = value;
            }
        }
        internal static bool _SetParagraphFlagHeightUsePreElement = true ;
        /// <summary>
        /// 根据紧靠前面的元素设置段落符号元素的高度，默认为true。这是一个保持排版结果兼容性的设置。
        /// </summary>
        [DefaultValue( true )]
        [System.Reflection.Obfuscation(Exclude = true , ApplyToMembers = true)]
        public bool SetParagraphFlagHeightUsePreElement
        {
            get
            {
                return _SetParagraphFlagHeightUsePreElement;
            }
            set
            {
                _SetParagraphFlagHeightUsePreElement = value;
            }
        }

        private bool _ParseCrLfAsLineBreakElement = false;
        /// <summary>
        /// 将字符串中的CrLf字符解释为LineBreakElement元素，否则解释成ParagraphFlag元素。默认为false。
        /// </summary>
        [DefaultValue( false )]
        [System.Reflection.Obfuscation(Exclude = true , ApplyToMembers = true)]
        public bool ParseCrLfAsLineBreakElement
        {
            get
            {
                return _ParseCrLfAsLineBreakElement;
            }
            set
            {
                _ParseCrLfAsLineBreakElement = value;
            }
        }

        private bool _AutoClearInvalidateCharacter = true;
        /// <summary>
        /// 加载文件的时候是否自动将Ascii码小于等于8的字符转换为空格。
        /// </summary>
        [DefaultValue( true )]
        [System.Reflection.Obfuscation(Exclude = true , ApplyToMembers = true)]
        public bool AutoClearInvalidateCharacter
        {
            get
            {
                return this._AutoClearInvalidateCharacter;
            }
            set
            {
                this._AutoClearInvalidateCharacter = value;
            }
        }

         

        private bool _CompressXMLContent = false;
        /// <summary>
        /// 压缩XML内容
        /// </summary>
        [DefaultValue( false )]
        [System.Reflection.Obfuscation(Exclude = true , ApplyToMembers = true)]
        public bool CompressXMLContent
        {
            get
            {
                return _CompressXMLContent; 
            }
            set
            {
                _CompressXMLContent = value; 
            }
        }

        private bool _EnableContentChangedEventWhenLoadDocument = false;
        /// <summary>
        /// 在加载文档时是否允许触发ContentChanged事件。
        /// </summary>
        [DefaultValue(false)]
        public bool EnableContentChangedEventWhenLoadDocument
        {
            get 
            {
                return _EnableContentChangedEventWhenLoadDocument; 
            }
            set
            {
                _EnableContentChangedEventWhenLoadDocument = value; 
            }
        }



        private bool _AutoClearTextFormatWhenPasteOrInsertContent = false;
        /// <summary>
        /// 在粘贴或插入对象时自动清除文件格式，设置为插入点所在的当前格式。
        /// </summary>
        [DefaultValue( false )]
        [System.Reflection.Obfuscation(Exclude = true , ApplyToMembers = true)]
        public bool AutoClearTextFormatWhenPasteOrInsertContent
        {
            get
            {
                return _AutoClearTextFormatWhenPasteOrInsertContent; 
            }
            set
            {
                _AutoClearTextFormatWhenPasteOrInsertContent = value; 
            }
        }

        private bool _AutoFixElementIDWhenInsertElements = true;
        /// <summary>
        /// 插入文档元素时自动修正文档元素编号
        /// </summary>
        [DefaultValue( true )]
        [System.Reflection.Obfuscation(Exclude = true , ApplyToMembers = true)]
        public bool AutoFixElementIDWhenInsertElements
        {
            get
            {
                return _AutoFixElementIDWhenInsertElements; 
            }
            set
            {
                _AutoFixElementIDWhenInsertElements = value; 
            }
        }

        private bool _RemoveLastParagraphFlagWhenInsertDocument = false;
        /// <summary>
        /// 在插入文档时删除要插入的文档的最后一个段落符号。默认false。
        /// </summary>
        [DefaultValue( false )]
        [System.Reflection.Obfuscation(Exclude = true , ApplyToMembers = true)]
        public bool RemoveLastParagraphFlagWhenInsertDocument
        {
            get
            {
                return _RemoveLastParagraphFlagWhenInsertDocument; 
            }
            set
            {
                _RemoveLastParagraphFlagWhenInsertDocument = value; 
            }
        }

        private bool _AllowDeleteJumpOutOfField = true;
        /// <summary>
        /// 允许跳出输入域来删除内容
        /// </summary>
        [DefaultValue( true )]
        [System.Reflection.Obfuscation(Exclude = true , ApplyToMembers = true)]
        public bool AllowDeleteJumpOutOfField
        {
            get
            {
                return _AllowDeleteJumpOutOfField; 
            }
            set
            {
                _AllowDeleteJumpOutOfField = value; 
            }
        }

        private bool _ContinueHeaderParagrahStyle = false;
        /// <summary>
        /// 敲回车键新建段落时是否延续标题头段落样式，默认为false。
        /// </summary>
        [DefaultValue( false )]
        [System.Reflection.Obfuscation(Exclude = true , ApplyToMembers = true)]
        public bool ContinueHeaderParagrahStyle
        {
            get
            {
                return _ContinueHeaderParagrahStyle; 
            }
            set
            {
                _ContinueHeaderParagrahStyle = value; 
            }
        }

        private bool _EnableChineseFontSizeName = true;
        /// <summary>
        /// 是否使用中文字体大小名称
        /// </summary>
        /// <remarks>
        /// 如果设置为false，则字体下拉列表中不会出现"三号、小三、四号、小四"之类的中文字体名称。
        /// 本属性必须在创建控件对象实例后立即设置，否则无效。
        /// </remarks>
        [DefaultValue( true )]
        [System.Reflection.Obfuscation(Exclude = true , ApplyToMembers = true)]
        public bool EnableChineseFontSizeName
        {
            get
            {
                return _EnableChineseFontSizeName; 
            }
            set 
            {
                _EnableChineseFontSizeName = value; 
            }
        }

        private MoveFocusHotKeys _MoveFocusHotKey = MoveFocusHotKeys.None;
        /// <summary>
        /// 移动焦点使用的快捷键。该属性在WinForm版和WEB版中有效。
        /// </summary>
        [DefaultValue(MoveFocusHotKeys.None)]
        [System.Reflection.Obfuscation(Exclude = true , ApplyToMembers = true)]
        public MoveFocusHotKeys MoveFocusHotKey
        {
            get
            {
                return _MoveFocusHotKey;
            }
            set
            {
                _MoveFocusHotKey = value;
            }
        }

        private bool _EnabledElementEvent = true;
        /// <summary>
        /// 是否允许触发文档元素级事件
        /// </summary>
        [DefaultValue(true)]
        [System.Reflection.Obfuscation(Exclude = true , ApplyToMembers = true)]
        public bool EnabledElementEvent
        {
            get
            {
                return _EnabledElementEvent;
            }
            set
            {
                _EnabledElementEvent = value;
            }
        }

        private bool _ShowTooltip = true;
        /// <summary>
        /// 是否显示提示文本
        /// </summary>
        [System.Reflection.Obfuscation(Exclude = true , ApplyToMembers = true)]
        public bool ShowTooltip
        {
            get
            {
                return _ShowTooltip;
            }
            set
            {
                _ShowTooltip = value;
            }
        }

        private bool _AllowDragContent = false;
        /// <summary>
        /// 能否直接拖拽文档内容
        /// </summary>
        [System.ComponentModel.DefaultValue(false)]
        [System.Reflection.Obfuscation(Exclude = true , ApplyToMembers = true)]
        public bool AllowDragContent
        {
            get
            {
                return _AllowDragContent;
            }
            set
            {
                _AllowDragContent = value;
            }
        }

        private WriterDataFormats _AcceptDataFormats = WriterDataFormats.All;
        /// <summary>
        /// 编辑器控件能接受的数据格式，包括粘贴操作和OLE拖拽操作获得的数据
        /// </summary>
        [DefaultValue(WriterDataFormats.All)]
        [System.Reflection.Obfuscation(Exclude = true , ApplyToMembers = true)]
        public WriterDataFormats AcceptDataFormats
        {
            get
            {
                return _AcceptDataFormats;
            }
            set
            {
                _AcceptDataFormats = value;
            }
        }

        private WriterDataFormats _CreationDataFormats = WriterDataFormats.All;
        /// <summary>
        /// 编辑器控件能创建的数据格式，这些数据将用于复制操作和OLE拖拽操作
        /// </summary>
        [DefaultValue(WriterDataFormats.All)]
        [System.Reflection.Obfuscation(Exclude = true , ApplyToMembers = true)]
        public WriterDataFormats CreationDataFormats
        {
            get
            {
                return _CreationDataFormats;
            }
            set
            {
                _CreationDataFormats = value;
            }
        }

        private bool _CompressLayoutForFieldBorder = true;
        /// <summary>
        /// 对于输入域边框元素采用紧凑排版
        /// </summary>
        [DefaultValue( true )]
        [System.Reflection.Obfuscation(Exclude = true , ApplyToMembers = true)]
        public bool CompressLayoutForFieldBorder
        {
            get
            {
                return _CompressLayoutForFieldBorder; 
            }
            set
            {
                _CompressLayoutForFieldBorder = value; 
            }
        }

        private bool _EnableCalculateControl = true;
        /// <summary>
        /// 是否允许使用计算器控件(数字小键盘)
        /// </summary>
        [System.Reflection.Obfuscation(Exclude = true , ApplyToMembers = true)]
        public bool EnableCalculateControl
        {
            get
            {
                return _EnableCalculateControl;
            }
            set
            {
                _EnableCalculateControl = value;
            }
        }

        private bool _EnableEditElementValue = true;
        /// <summary>
        /// 是否允许编辑文档元素内容值操作
        /// </summary>
        [DefaultValue(true)]
        [System.Reflection.Obfuscation(Exclude = true , ApplyToMembers = true)]
        public bool EnableEditElementValue
        {
            get
            {
                return _EnableEditElementValue;
            }
            set
            {
                _EnableEditElementValue = value;
            }
        }


        private DCValidateIDRepeatMode _ValidateIDRepeatMode = DCValidateIDRepeatMode.None;
        /// <summary>
        /// ID重复性校验模式
        /// </summary>
        [DefaultValue( DCValidateIDRepeatMode.None )]
        [System.Reflection.Obfuscation(Exclude = true , ApplyToMembers = true)]
        public DCValidateIDRepeatMode ValidateIDRepeatMode
        {
            get
            {
                return _ValidateIDRepeatMode; 
            }
            set
            {
                _ValidateIDRepeatMode = value; 
            }
        }

        private bool _PageLineUnderPageBreak = false;
        /// <summary>
        /// 分页线在分页符的下面,如果为true，则分页线在分页符的上面,也就是分页符在上一页的最下面。本属性默认为false。
        /// </summary>
        [DefaultValue( false )]
        [System.Reflection.Obfuscation(Exclude = true , ApplyToMembers = true)]
        public bool PageLineUnderPageBreak
        {
            get 
            {
                return _PageLineUnderPageBreak; 
            }
            set
            {
                _PageLineUnderPageBreak = value; 
            }
        }

        private bool _ParagraphFlagFollowTableOrSection = false;
        /// <summary>
        /// 排版时段落符号紧跟在表格或文档节后面
        /// </summary>
        [DefaultValue( false )]
        [System.Reflection.Obfuscation(Exclude = true , ApplyToMembers = true)]
        public bool ParagraphFlagFollowTableOrSection
        {
            get
            {
                return _ParagraphFlagFollowTableOrSection; 
            }
            set
            {
                _ParagraphFlagFollowTableOrSection = value; 
            }
        }

        private bool _HandleCommandException = true    ;
        /// <summary>
        /// 是否处理编辑器命令过程的异常
        /// </summary>
        [DefaultValue( true )]
        [System.Reflection.Obfuscation(Exclude = true , ApplyToMembers = true)]
        public bool HandleCommandException
        {
            get
            {
                return _HandleCommandException; 
            }
            set
            {
                _HandleCommandException = value; 
            }
        }

        private bool _DisplayFormatToInnerValue = true ;
        /// <summary>
        /// 是否对输入域的InnerValue启用DisplayFormat设置
        /// </summary>
        [DefaultValue( true  )]
        [System.Reflection.Obfuscation(Exclude = true , ApplyToMembers = true)]
        public bool DisplayFormatToInnerValue
        {
            get
            {
                return _DisplayFormatToInnerValue; 
            }
            set
            {
                _DisplayFormatToInnerValue = value; 
            }
        }

        private bool _PromptJumpBackForSearch = true;
        /// <summary>
        /// 是否提示跳回去继续搜索文本
        /// </summary>
        [DefaultValue( true )]
        [System.Reflection.Obfuscation(Exclude = true , ApplyToMembers = true)]
        public bool PromptJumpBackForSearch
        {
            get 
            {
                return _PromptJumpBackForSearch; 
            }
            set
            {
                _PromptJumpBackForSearch = value; 
            }
        }

         
        private bool _MoveFieldWhenDragWholeContent = true ;
        /// <summary>
        /// 拖拽所有内容时移动整个输入域
        /// </summary>
        [DefaultValue( true  )]
        [System.Reflection.Obfuscation(Exclude = true , ApplyToMembers = true)]
        public bool MoveFieldWhenDragWholeContent
        {
            get
            {
                return _MoveFieldWhenDragWholeContent; 
            }
            set
            {
                _MoveFieldWhenDragWholeContent = value; 
            }
        }

        private bool _EnableLogUndo = true;
        /// <summary>
        /// 是否记录撤销操作信息
        /// </summary>
        /// <remarks>
        /// 如果为false，则系统不记录文档操作的信息，用户操作就不能撤销或重做。
        /// </remarks>
        [DefaultValue( true )]
        [System.Reflection.Obfuscation(Exclude = true , ApplyToMembers = true)]
        public bool EnableLogUndo
        {
            get
            {
                return _EnableLogUndo; 
            }
            set
            {
                _EnableLogUndo = value; 
            }
        }

        private bool _ShowDebugMessage = false    ;
        /// <summary>
        /// 显示内部调试消息
        /// </summary>
        [DefaultValue( false     )]
        [System.Reflection.Obfuscation(Exclude = true , ApplyToMembers = true)]
        public bool ShowDebugMessage
        {
            get
            {
                return _ShowDebugMessage; 
            }
            set
            {
                _ShowDebugMessage = value; 
            }
        }

        /// <summary>
        /// 是否启用文档元素事件,本属性等价于 EnabledElementEvent。
        /// </summary>
        [DefaultValue( true )]
        [System.Reflection.Obfuscation(Exclude = true , ApplyToMembers = true)]
        public bool EnableElementEvents
        {
            get
            {
                return this._EnabledElementEvent; 
            }
            set
            {
                this._EnabledElementEvent = value; 
            }
        }



        private bool _CloneSerialize = true;
        /// <summary>
        /// 复制序列化模式
        /// </summary>
        [DefaultValue(true)]
        [System.Reflection.Obfuscation(Exclude = true , ApplyToMembers = true)]
        public bool CloneSerialize
        {
            get
            {
                return _CloneSerialize; 
            }
            set
            {
                _CloneSerialize = value; 
            }
        }

        private bool _WeakMode = false;
        /// <summary>
        /// 脆弱模式，默认为false。
        /// </summary>
        /// <remarks>
        /// 当处于脆弱模式时，DCWriter中很多系统异常不经处理而直接抛出，这有利于暴露出错误的根源。但会造成系统不稳定。
        /// </remarks>
        [DefaultValue( false )]
        [System.Reflection.Obfuscation(Exclude = true , ApplyToMembers = true)]
        public bool WeakMode
        {
            get
            {
                return _WeakMode; 
            }
            set
            {
                _WeakMode = value; 
            }
        }

        private bool _ThreeClickToSelectParagraph = true;
        /// <summary>
        /// 鼠标连续三击选中段落，默认为true.
        /// </summary>
        [DefaultValue( true )]
        [System.Reflection.Obfuscation(Exclude = true , ApplyToMembers = true)]
        public bool ThreeClickToSelectParagraph
        {
            get
            {
                return _ThreeClickToSelectParagraph; 
            }
            set
            {
                _ThreeClickToSelectParagraph = value; 
            }
        }

        private bool _DoubleClickToSelectWord = true;
        /// <summary>
        /// 双击选中文字,默认为true.
        /// </summary>
        [DefaultValue(true) ]
        [System.Reflection.Obfuscation(Exclude = true , ApplyToMembers = true)]
        public bool DoubleClickToSelectWord
        {
            get
            {
                return _DoubleClickToSelectWord; 
            }
            set
            {
                _DoubleClickToSelectWord = value; 
            }
        }
        
        private bool _PromptForRejectFormat = true;
        /// <summary>
        /// 遇到拒绝的数据格式进行提示
        /// </summary>
        [DefaultValue( true )]
        [System.Reflection.Obfuscation(Exclude = true , ApplyToMembers = true)]
        public bool PromptForRejectFormat
        {
            get { return _PromptForRejectFormat; }
            set { _PromptForRejectFormat = value; }
        }


        private static bool _GlobalSpecifyDebugMode = false;
        /// <summary>
        /// 全局性的特别调试模式，内部使用，默认为false。
        /// </summary>
        [System.Reflection.Obfuscation(Exclude = true , ApplyToMembers = true)]
        public static bool GlobalSpecifyDebugMode
        {
            get
            {
                return _GlobalSpecifyDebugMode; 
            }
            set
            {
                _GlobalSpecifyDebugMode = value; 
            }
        }

        /// <summary>
        /// 全局性的特别调试模式值，内部使用，默认为false。
        /// </summary>
        [DefaultValue( false )]
        [System.Reflection.Obfuscation(Exclude = true , ApplyToMembers = true)]
        public bool GlobalSpecifyDebugModeValue
        {
            get
            {
                return _GlobalSpecifyDebugMode;
            }
            set
            {
                _GlobalSpecifyDebugMode = value;
            }
        }

        private bool _SpecifyDebugMode = false ;
        /// <summary>
        /// 特别的调试模式，内部使用，默认为false。
        /// </summary>
        [DefaultValue( false )]
        [System.Reflection.Obfuscation(Exclude = true , ApplyToMembers = true)]
        public bool SpecifyDebugMode
        {
            get
            {
                return _SpecifyDebugMode; 
            }
            set
            {
                _SpecifyDebugMode = value; 
            }
        }

        private bool _OutputFormatedXMLSource = true ;
        /// <summary>
        /// 是否输出格式化的XML文本，默认为true。
        /// </summary>
        /// <remarks>格式化的XML文本带有缩进控制，阅读方便，但文档比较大。</remarks>
        [DefaultValue( true )]
        [System.Reflection.Obfuscation(Exclude = true , ApplyToMembers = true)]
        public bool OutputFormatedXMLSource
        {
            get
            {
                return _OutputFormatedXMLSource; 
            }
            set
            {
                _OutputFormatedXMLSource = value; 
            }
        }

        private int _TableCellOperationDetectDistance = 3;
        /// <summary>
        /// 表格单元格操作检测时使用的距离长度，单位为屏幕像素，默认为3。
        /// </summary>
        [DefaultValue( 3 )]
        [System.Reflection.Obfuscation(Exclude = true , ApplyToMembers = true)]
        public int TableCellOperationDetectDistance
        {
            get
            {
                return _TableCellOperationDetectDistance; 
            }
            set
            {
                _TableCellOperationDetectDistance = value; 
            }
        }

        private bool _WidelyRaiseFocusEvent = false;
        /// <summary>
        /// 宽范围的触发焦点事件,都昌内部使用。
        /// </summary>
        [DefaultValue( false )]
        [System.Reflection.Obfuscation(Exclude = true , ApplyToMembers = true)]
        public bool WidelyRaiseFocusEvent
        {
            get
            {
                return _WidelyRaiseFocusEvent; 
            }
            set
            {
                _WidelyRaiseFocusEvent = value; 
            }
        }


        private bool _DebugMode = false;
        /// <summary>
        /// 系统是否处于调试模式。若为true，则系统处于调试模式，系统会输出一些调试文本信息。默认为false。
        /// </summary>
        [DefaultValue( false )]
        [System.Reflection.Obfuscation(Exclude = true , ApplyToMembers = true)]
        public bool DebugMode
        {
            get
            {
                return _DebugMode; 
            }
            set
            {
                _DebugMode = value; 
            }
        }
         
        private ValueEditorActiveMode _DefaultEditorActiveMode = ValueEditorActiveMode.None;
        /// <summary>
        /// 默认的数值编辑器激活模式，默认为None。
        /// </summary>
        [DefaultValue( ValueEditorActiveMode.None )]
        [System.Reflection.Obfuscation(Exclude = true , ApplyToMembers = true)]
        public ValueEditorActiveMode DefaultEditorActiveMode
        {
            get
            {
                return _DefaultEditorActiveMode; 
            }
            set
            {
                _DefaultEditorActiveMode = value; 
            }
        }

        private PromptProtectedContentMode _PromptContainUnDeleteContent 
            = PromptProtectedContentMode.Details    ;
        /// <summary>
        /// 当视图删除无法删除的内容时的提示方式，默认为Details。
        /// </summary>
        [DefaultValue(PromptProtectedContentMode.Details )]
        [System.Reflection.Obfuscation(Exclude = true , ApplyToMembers = true)]
        public PromptProtectedContentMode PromptProtectedContent
        {
            get
            {
                return _PromptContainUnDeleteContent; 
            }
            set
            {
                _PromptContainUnDeleteContent = value; 
            }
        }


        /// <summary>
        /// 复制对象
        /// </summary>
        /// <returns>复制品</returns>
        object ICloneable.Clone()
        {
            DocumentBehaviorOptions opt = (DocumentBehaviorOptions)this.MemberwiseClone();
            return opt;
        }

        /// <summary>
        /// 复制对象
        /// </summary>
        /// <returns>复制品</returns>
        [System.Reflection.Obfuscation(Exclude = true , ApplyToMembers = true)]
        public DocumentBehaviorOptions Clone()
        {
            return (DocumentBehaviorOptions)((ICloneable)this).Clone();
        }
    }

    /// <summary>
    /// 受保护内容的提示方式
    /// </summary>
    [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
    public enum PromptProtectedContentMode
    {
        /// <summary>
        /// 不提示
        /// </summary>
        None,
        /// <summary>
        /// 简单的提示
        /// </summary>
        Simple,
        /// <summary>
        /// 显示详细信息的提示
        /// </summary>
        Details
    }

    /// <summary>
    /// ID重复性校验行为
    /// </summary>
    [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
    public enum DCValidateIDRepeatMode
    {
        /// <summary>
        /// 不做任何校验
        /// </summary>
        None ,
        /// <summary>
        /// 只进行检查，不校验
        /// </summary>
        DetectOnly ,
        /// <summary>
        /// 自动修正ID号
        /// </summary>
        AutoFix ,
        /// <summary>
        /// 抛出异常
        /// </summary>
        ThrowException
    }
}
