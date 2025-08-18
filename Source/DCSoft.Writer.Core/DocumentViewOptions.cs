using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Runtime.InteropServices;
using DCSoft.Writer.Data ;
using DCSoft.Common;
using DCSoft.Writer.Dom;
using DCSoft.Drawing;

namespace DCSoft.Writer
{ 
    /// <summary>
    /// 文档视图相关选项
    /// </summary>
    /// <remarks>编制 袁永福</remarks>
    [Serializable()]
    [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = false )]
    public partial class DocumentViewOptions : ICloneable
    {
        /// <summary>
        /// 初始化对象
        /// </summary>
        public DocumentViewOptions()
        {
        }
        

        private bool _ShowRedErrorMessageForPaint = true;
        /// <summary>
        /// 在绘制文档发生错误时是否绘制红色错误文本信息
        /// </summary>
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        [DefaultValue(true)]
        public bool ShowRedErrorMessageForPaint
        {
            get
            {
                return _ShowRedErrorMessageForPaint;
            }
            set
            {
                _ShowRedErrorMessageForPaint = value;
            }
        }
        private bool _ShowGlobalGridLineInTableAndSection = true;
        /// <summary>
        /// 在表格和文档节中显示全局网格线，默认为true。
        /// </summary>
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        [DefaultValue( true )]
        public bool ShowGlobalGridLineInTableAndSection
        {
            get
            {
                return _ShowGlobalGridLineInTableAndSection;
            }
            set
            {
                _ShowGlobalGridLineInTableAndSection = value;
            }
        }


        private float _FieldBorderElementPixelWidth = 1f;
        /// <summary>
        /// 输入域边框元素像素宽度
        /// </summary>
        [DefaultValue(1f)]
        [System.Reflection.Obfuscation(Exclude = true , ApplyToMembers = true)]
        public float FieldBorderElementPixelWidth
        {
            get
            {
                return _FieldBorderElementPixelWidth;
            }
            set
            {
                _FieldBorderElementPixelWidth = value;
            }
        }

        private bool _ShowPageBreak = false;
        /// <summary>
        /// 显示分页符
        /// </summary>
        [DefaultValue( false )]
        [System.Reflection.Obfuscation(Exclude = true , ApplyToMembers = true)]
        public bool ShowPageBreak
        {
            get
            {
                return _ShowPageBreak; 
            }
            set
            {
                _ShowPageBreak = value; 
            }
        }

        private int _PageMarginLineLength = 30;
        /// <summary>
        /// 表示页面边界的线条长度,
        /// </summary>
        [DefaultValue( 30 )]
        [System.Reflection.Obfuscation(Exclude = true , ApplyToMembers = true)]
        public int PageMarginLineLength
        {
            get
            {
                return _PageMarginLineLength; 
            }
            set
            {
                _PageMarginLineLength = value; 
            }
        }
        
        private EnableState _DefaultInputFieldHighlight = EnableState.Enabled;
        /// <summary>
        /// 默认的输入域高亮度显示模式
        /// </summary>
        [DefaultValue(EnableState.Enabled)]
        [System.Reflection.Obfuscation(Exclude = true , ApplyToMembers = true)]
        public virtual EnableState DefaultInputFieldHighlight
        {
            get
            {
                return _DefaultInputFieldHighlight;
            }
            set
            {
                _DefaultInputFieldHighlight = value;// (EnableState)WriterUtilsInner.FixEnumValue(value);
            }
        }

        private bool _PreserveBackgroundTextWhenPrint = false;
        /// <summary>
        /// 在打印的时候保留背景文字的位置，但不打印背景文字。
        /// </summary>
        [DefaultValue( false )]
        [System.Reflection.Obfuscation(Exclude = true , ApplyToMembers = true)]
        public bool PreserveBackgroundTextWhenPrint
        {
            get
            {
                return _PreserveBackgroundTextWhenPrint; 
            }
            set
            {
                _PreserveBackgroundTextWhenPrint = value; 
            }
        }

        private bool _PrintBackgroundText = false;
        /// <summary>
        /// 打印时是否显示输入域的背景文字，默认为false。
        /// </summary>
        [DefaultValue( false )]
        [System.Reflection.Obfuscation(Exclude = true , ApplyToMembers = true)]
        public bool PrintBackgroundText
        {
            get
            {
                return _PrintBackgroundText; 
            }
            set
            {
                _PrintBackgroundText = value; 
            }
        }

        private bool _ShowHeaderBottomLine = true;
        /// <summary>
        /// 当页眉有内容时显示页眉下边框线。若为false，则页眉和正文之间就没有分隔横线。默认值为true。
        /// </summary>
        [DefaultValue( true )]
        [System.Reflection.Obfuscation(Exclude = true , ApplyToMembers = true)]
        public bool ShowHeaderBottomLine
        {
            get
            {
                return _ShowHeaderBottomLine; 
            }
            set
            {
                _ShowHeaderBottomLine = value; 
            }
        }

        private float _HeaderBottomLineWidth = 1f;
        /// <summary>
        /// 页眉下边框线的宽度.
        /// </summary>
        [DefaultValue( 1f )]
        [System.Reflection.Obfuscation(Exclude = true , ApplyToMembers = true)]
        //[DCDescription(typeof(DocumentViewOptions), "HeaderBottomLineWidth")]
        public float HeaderBottomLineWidth
        {
            get
            {
                return _HeaderBottomLineWidth; 
            }
            set
            {
                _HeaderBottomLineWidth = value; 
            }
        }

        private bool _ShowCellNoneBorder = true;
        /// <summary>
        /// 是否显示表格单元格的隐藏的边框线。当为true时，若表格没有边框线，
        /// 则在编辑器中也会使用NoneBorderColor选项指定的颜色显示出边框线。
        /// 该设置不影响打印结果。该选项默认值为true。
        /// </summary>
        [DefaultValue( true )]
        [System.Reflection.Obfuscation(Exclude = true , ApplyToMembers = true)]
        public bool ShowCellNoneBorder
        {
            get
            {
                return _ShowCellNoneBorder; 
            }
            set
            {
                _ShowCellNoneBorder = value; 
            }
        }

        private Color _NoneBorderColor = Color.LightGray;
        /// <summary>
        /// 绘制隐藏的边框线使用的颜色。默认淡灰色。
        /// </summary>
        [System.Reflection.Obfuscation(Exclude = true , ApplyToMembers = true)]
        public Color NoneBorderColor
        {
            get
            {
                return _NoneBorderColor; 
            }
            set
            {
                _NoneBorderColor = value; 
            }
        }
         
         
        private bool _ShowParagraphFlag = true;
        /// <summary>
        /// 显示段落标记。不影响打印，默认为true。
        /// </summary>
        [DefaultValue( true )]
        [System.Reflection.Obfuscation(Exclude = true , ApplyToMembers = true)]
        public bool ShowParagraphFlag
        {
            get
            {
                return _ShowParagraphFlag; 
            }
            set
            {
                _ShowParagraphFlag = value; 
            }
        }

        private bool _HiddenFieldBorderWhenLostFocus = true;
        /// <summary>
        /// 输入域失去焦点时隐藏边框元素
        /// </summary>
        [DefaultValue( true )]
        [System.Reflection.Obfuscation(Exclude = true , ApplyToMembers = true)]
        public bool HiddenFieldBorderWhenLostFocus
        {
            get
            {
                return _HiddenFieldBorderWhenLostFocus; 
            }
            set
            {
                _HiddenFieldBorderWhenLostFocus = value; 
            }
        }

        private Color _FieldBorderColor = Color.Empty;
        /// <summary>
        /// 输入域边框元素颜色，默认为空颜色
        /// </summary>
        [System.Reflection.Obfuscation(Exclude = true , ApplyToMembers = true)]
        public Color FieldBorderColor
        {
            get
            {
                return _FieldBorderColor; 
            }
            set
            {
                _FieldBorderColor = value; 
            }
        }

        private bool _ShowPageLine = true;
        /// <summary>
        /// 当编辑器处于普通视图模式（不分页），是否显示分页线。默认为true。
        /// </summary>
        [DefaultValue(true)]
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public bool ShowPageLine
        {
            get
            {
                return _ShowPageLine;
            }
            set
            {
                _ShowPageLine = value;
            }
        }

        private float _MinTableColumnWidth = 50f;
        /// <summary>
        /// 表格列的最小宽度，单位为1/300英寸，默认为50。
        /// </summary>
        [DefaultValue(50f )]
        [System.Reflection.Obfuscation(Exclude = true , ApplyToMembers = true)]
        public float MinTableColumnWidth
        {
            get
            {
                return _MinTableColumnWidth; 
            }
            set
            {
                _MinTableColumnWidth = value; 
            }
        }

         
        private Color _FieldBackColor = Color.AliceBlue ;
        /// <summary>
        /// 文本输入域的默认背景颜色，默认为浅蓝色。
        /// </summary>
        [System.Reflection.Obfuscation(Exclude = true , ApplyToMembers = true)]
        public Color FieldBackColor
        {
            get
            {
                return _FieldBackColor; 
            }
            set
            {
                _FieldBackColor = value; 
            }
        }


        private Color _FieldHoverBackColor = Color.LightBlue;
        /// <summary>
        /// 鼠标悬浮在文本输入域时文本输入域的高亮度背景颜色，默认为淡蓝色。
        /// </summary>
        [System.Reflection.Obfuscation(Exclude = true , ApplyToMembers = true)]
        public Color FieldHoverBackColor
        {
            get
            {
                return _FieldHoverBackColor; 
            }
            set
            {
                _FieldHoverBackColor = value; 
            }
        }


        private Color _FieldFocusedBackColor = Color.LightBlue;
        /// <summary>
        /// 文本输入域获得输入焦点时的高亮度背景颜色,默认为淡蓝色。
        /// </summary>
        [System.Reflection.Obfuscation(Exclude = true , ApplyToMembers = true)]
        public Color FieldFocusedBackColor
        {
            get
            {
                return _FieldFocusedBackColor;
            }
            set
            {
                _FieldFocusedBackColor = value;
            }
        }

        private Color _FieldInvalidateValueForeColor = Color.LightCoral;
        /// <summary>
        /// 文本输入域数据异常时的高亮度文本色，默认为淡红色。
        /// </summary>
        [System.Reflection.Obfuscation(Exclude = true , ApplyToMembers = true)]
        public Color FieldInvalidateValueForeColor
        {
            get
            {
                return _FieldInvalidateValueForeColor;
            }
            set
            {
                _FieldInvalidateValueForeColor = value;
            }
        }


        private Color _FieldInvalidateValueBackColor = Color.Transparent  ;
        /// <summary>
        /// 文本输入域数据异常时的高亮度背景色，默认为全透明。
        /// </summary>
        [System.Reflection.Obfuscation(Exclude = true , ApplyToMembers = true)]
        public Color FieldInvalidateValueBackColor
        {
            get
            {
                return _FieldInvalidateValueBackColor; 
            }
            set
            {
                _FieldInvalidateValueBackColor = value; 
            }
        }
        
        private Color _UnEditableFieldBorderColor = Color.Red ;
        /// <summary>
        /// 内容不能被用户直接录入修改的输入域的边界元素颜色，默认为红色
        /// </summary>
        [System.Reflection.Obfuscation(Exclude = true , ApplyToMembers = true)]
        public Color UnEditableFieldBorderColor
        {
            get
            {
                return _UnEditableFieldBorderColor;
            }
            set
            {
                _UnEditableFieldBorderColor = value;
            }
        }


        internal static readonly int DefaultNormalBorderColorARGB = Color.Blue.ToArgb();
        internal static readonly Color DefaultNormalBorderColor = Color.Blue;

        private Color _NormalFieldBorderColor = DefaultNormalBorderColor;
        /// <summary>
        /// 常规的输入域的边界元素颜色，用户可以在常规的输入域中直接输入内容。该属性值默认为蓝色
        /// </summary>
        [System.Reflection.Obfuscation(Exclude = true , ApplyToMembers = true)]
        public Color NormalFieldBorderColor
        {
            get
            {
                return _NormalFieldBorderColor;
            }
            set
            {
                _NormalFieldBorderColor = value;
            }
        }


        internal static Color _BackgroundTextColor = Color.Gray;
        /// <summary>
        /// 字段域的背景文本颜色，默认为灰色。
        /// </summary>
        [System.Reflection.Obfuscation(Exclude = true , ApplyToMembers = true)]
        public Color BackgroundTextColor
        {
            get
            {
                return _BackgroundTextColor;
            }
            set
            {
                _BackgroundTextColor = value;
            }
        }

        private Color _SelectionHightlightMaskColor = Color.FromArgb(50, Color.Blue);
        /// <summary>
        /// 选择区域高亮度遮盖色，本选项和SelectionHighlight搭配使用。默认为半透明淡蓝色。
        /// </summary>
        [System.Reflection.Obfuscation(Exclude = true , ApplyToMembers = true)]
        public Color SelectionHightlightMaskColor
        {
            get
            {
                return _SelectionHightlightMaskColor; 
            }
            set
            {
                _SelectionHightlightMaskColor = value; 
            }
        }

        /// <summary>
        /// 复制对象
        /// </summary>
        /// <returns>复制品</returns>
        object ICloneable.Clone()
        {
            DocumentViewOptions opt = (DocumentViewOptions)this.MemberwiseClone();
            return opt;
        }

        /// <summary>
        /// 复制对象
        /// </summary>
        /// <returns>复制品</returns>
        [System.Reflection.Obfuscation(Exclude = true , ApplyToMembers = true)]
        public DocumentViewOptions Clone()
        {
            return ( DocumentViewOptions ) ((ICloneable)this).Clone();
        }
    }
}
