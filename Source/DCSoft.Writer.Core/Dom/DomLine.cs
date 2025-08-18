using System;
using DCSoft.Printing ;
using DCSoft.Drawing;
using System.Collections;
using System.Collections.Generic;
// // 
using System.Runtime.InteropServices;
using System.ComponentModel;
using DCSoft.Common ;

namespace DCSoft.Writer.Dom
{
    /// <summary>
    /// 文本行对象
    /// </summary>
#if !RELEASE
    [System.Diagnostics.DebuggerDisplay("Count={ Count }")]
    [System.Diagnostics.DebuggerTypeProxy(typeof(DCSoft.Common.ListDebugView))]
#endif
    public partial class DomLine : DomElementList
    {
        internal static bool CheckStopLine(DomLine line1, DomLine line2)
        {
            if (line1._size != line2._size)
            {
                return false;
            }
            if (line2.InvalidateState)
            {
                return false;
            }
            if (line1._PaddingLeft != line2._PaddingLeft)
            {
                return false;
            }
            var arr1 = line1._items;
            var arr2 = line2._items;
            for (int iCount = line1.Count - 1; iCount >= 0; iCount--)
            {
                var element = arr1[iCount];
                if (element != arr2[iCount])
                {
                    return false;
                }
                if (element.SizeInvalid)
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 使用新的排版模式
        /// </summary>
        public const bool __NewLayoutMode = true;

        /// <summary>
        /// 初始化对象
        /// </summary>
        internal DomLine()
        {
            this.AllIsCharOrParaphFlag = true;
            this.IsCharOrParagraphFlagOnly = true;
            this.InnerHtmlVisible = true;
        }
        /// <summary>
        /// 是否包含被选择区域
        /// </summary>
        /// <returns></returns>
        public bool HasContainsSelection()
        {
            var fe = this.FirstElement.FirstContentElementInPublicContent;
            if (fe is DomParagraphListItemElement)
            {
                var index2 = this.IndexOf(fe);
                if (index2 >= 0)
                {
                    fe = this.SafeGet(index2 + 1);
                }
            }
            var le = this.LastElement.LastContentElementInPublicContent;
            if (fe != null
                && le != null
                && fe.ContentIndex >= 0
                && le.ContentIndex >= 0)
            {
                var sec = fe.DocumentContentElement?.Selection;
                if (sec != null && sec.Length != 0)
                {
                    if (sec.Ranges.IntersectsWith(fe.ContentIndex, le.ContentIndex))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// 文档行对齐到网格线了。
        /// </summary>
        internal bool AlignToGridLine
        {
            get { return GetStateFlag(4); }
            set { SetStateFlag(4, value); }
        }

        /// <summary>
        /// 内容全部都是字符或者段落符号
        /// </summary>
        internal bool AllIsCharOrParaphFlag
        {
            get { return GetStateFlag(8); }
            set { SetStateFlag(8, value); }
        }
        //private bool _IsCharOrParagraphFlagOnly = true;
        /// <summary>
        /// 是否只包含字符和段落符号
        /// </summary>
        internal bool IsCharOrParagraphFlagOnly
        {
            get { return GetStateFlag(16); }
            set { SetStateFlag(16, value); }
        }
        /// <summary>
        /// 在输出HTML中是否可见
        /// </summary>
        public bool InnerHtmlVisible
        {
            get { return GetStateFlag(128); }
            set { SetStateFlag(128, value); }
        }
        /// <summary>
        /// 行状态无效标记
        /// </summary>

        internal bool InvalidateState
        {
            get { return GetStateFlag(256); }
            set { SetStateFlag(256, value); }
        }
        /// <summary>
        /// 内容排版状态无效标记
        /// </summary>
        internal bool LayoutInvalidate
        {
            get { return GetStateFlag(512); }
            set { SetStateFlag(512, value); }
        }
        public DomElementList LayoutElements
        {
            get
            {
                return this;
            }
        }

        internal void ResetFieldValues()
        {
            var array = base.InnerGetArrayRaw();
            for (var iCount = this.Count - 1; iCount >= 0; iCount--)
            {
                if (array[iCount]._OwnerLine == this)
                {
                    array[iCount]._OwnerLine = null;
                }
            }
            this._AbsLeft = NonePosition;
            this._AbsTop = NonePosition;
            this._Align = DocumentContentAlignment.Left;
            this.AllIsCharOrParaphFlag = true;
            this._ContentHeight = -1;
            this._ContentTopFix = 0f;
            //this.DesignWidth = 0f;
            this._GlobalIndex = 0;
            this._Height = 0;
            this.InnerHtmlVisible = true;
            this._IndexInPage = 0;
            this.InvalidateState = false;
            this.IsCharOrParagraphFlagOnly = true;
            this.LayoutInvalidate = false;
            this._Left = 0;
            this._NativeTop = 0;
            //this._NeedCheckParticalDirection = true ;
            this._OwnerContentElement = null;
            this._OwnerDocument = null;
            this._OwnerPage = null;
            this._PaddingLeft = 0;
            this._PaddingRight = 0;
            this._PageBreakElement = null;
            this._ParagraphFlagElement = null;
            this._PrivateIndexInPage = 0;
            this._RuntimeVerticalAlign = VerticalAlignStyle.Bottom;
            this._SpacingAfterForParagraph = 0;
            this._SpacingBeforeForParagraph = 0;
            this._SpecifyLineSpacing = 0;
            this._TableElement = null;
            this._Top = 0;
            this.VerticalAlign = VerticalAlignStyle.Bottom;
            this._Width = 0;
            //this._WidthCountForAddElement = 0;
        }

        /// <summary>
        /// 对象所属的文档对象
        /// </summary>
        internal DomDocument _OwnerDocument = null;
        /// <summary>
        /// 对象所属的文档对象
        /// </summary>
        public virtual DomDocument OwnerDocument
        {
            get
            {
                return _OwnerDocument;
            }
            //set
            //{
            //    myOwnerDocument = value;
            //}
        }

        //private bool _EffectByFreeLayoutElement = false;
        ///// <summary>
        ///// 受到了自由布局元素影响
        ///// </summary>
        //[DefaultValue( false )]
        //[System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        //public bool EffectByFreeLayoutElement
        //{
        //    get
        //    { 
        //        return _EffectByFreeLayoutElement; 
        //    }
        //    set
        //    { 
        //        _EffectByFreeLayoutElement = value;
        //    }
        //}

        private DomParagraphFlagElement _ParagraphFlagElement = null;
        /// <summary>
        /// 文档行所在的段落的段落标记文档元素对象
        /// </summary>
        public DomParagraphFlagElement ParagraphFlagElement
        {
            get
            {
                return this._ParagraphFlagElement;
            }
            set
            {
                this._ParagraphFlagElement = value;
                if (this._ParagraphFlagElement != null && this.FirstElement is DomParagraphListItemElement)
                {
                    this._ParagraphFlagElement.ListItemElement = (DomParagraphListItemElement)this.FirstElement;
                }
            }
        }

        internal DomContentElement _OwnerContentElement = null;
        /// <summary>
        /// 对象所属的文档区域对象
        /// </summary>
        public DomContentElement OwnerContentElement
        {
            get
            {
                return this._OwnerContentElement;
            }
            //set
            //{
            //    _DocumentContentElement = value;
            //}
        }

        /// <summary>
        /// 对象所在的文档页对象
        /// </summary>
        internal PrintPage _OwnerPage = null;
        /// <summary>
        /// 对象所在的文档页对象
        /// </summary>
        public PrintPage OwnerPage
        {
            get
            {
                return this._OwnerPage;
            }
            set
            {
                //if (_OwnerPage != value)
                {
                    //if (value != null && value.PageIndex > 0)
                    //{
                    //}
                    this._OwnerPage = value;
                }
            }
        }
        /// <summary>
        /// 文档行在所属正文文档行列表中的序号
        /// </summary>
        internal int IndexOfDocumentContentLines = -1;

        internal int _GlobalIndex = 0;
        /// <summary>
        /// 在整个文档中的从1开始的行号
        /// </summary>
        public int GlobalIndex
        {
            get
            {
                return _GlobalIndex;
            }
            set
            {
                _GlobalIndex = value;
            }
        }

        private int _IndexInPage = 0;
        /// <summary>
        /// 在所在文档页中的从1开始的行号
        /// </summary>
        public int IndexInPage
        {
            get
            {
                return _IndexInPage;
            }
            set
            {
                //if (this._IndexInPage != value)
                {
                    this._IndexInPage = value;
                }
            }
        }
        /// <summary>
        /// 是否为段落中的首行
        /// </summary>
        /// <returns></returns>
        public bool IsFirstLineInParagraph()
        {
            if (this._ParagraphFlagElement != null)
            {
                if (this.Contains(this._ParagraphFlagElement.ParagraphFirstContentElement))
                {
                    return true;
                }
            }
            return false;
        }
        private int _PrivateIndexInPage = 0;
        /// <summary>
        /// 所在文档页中的从1开始的私有行号
        /// </summary>
        public int PrivateIndexInPage
        {
            get
            {
                return _PrivateIndexInPage;
            }
            set
            {
                if (value == 0)
                {
                }
                //if (_PrivateIndexInPage != value)
                {
                    _PrivateIndexInPage = value;
                }
            }
        }


        /// <summary>
        /// 垂直对齐方式
        /// </summary>
        public VerticalAlignStyle VerticalAlign
        {
            get
            {
                return VerticalAlignStyle.Bottom;// this._VerticalAlign;
            }
            set
            {
                //_VerticalAlign = value;
            }
        }

        private VerticalAlignStyle _RuntimeVerticalAlign = VerticalAlignStyle.Bottom;
        /// <summary>
        /// 运行时使用的垂直对齐方式
        /// </summary>
        public VerticalAlignStyle RuntimeVerticalAlign()
        {

            return _RuntimeVerticalAlign;

        }

        private DocumentContentAlignment _Align = DocumentContentAlignment.Left;
        /// <summary>
        /// 内容的水平对齐方式
        /// </summary>
        public DocumentContentAlignment Align
        {
            get
            {
                return _Align;
            }
            set
            {
                _Align = value;
            }
        }

        /// <summary>
        /// 左内边距
        /// </summary>
        private float _PaddingLeft = 0;
        /// <summary>
        /// 左内边距
        /// </summary>
        public float PaddingLeft
        {
            get
            {
                return _PaddingLeft;
            }
            set
            {
                _PaddingLeft = value;
            }
        }

        private float _PaddingRight = 0f;
        /// <summary>
        /// 右内边距
        /// </summary>
        public float PaddingRight
        {
            get
            {
                return _PaddingRight;
            }
            set
            {
                _PaddingRight = value;
            }
        }

        /// <summary>
        /// 对象左端位置
        /// </summary>
        private float _Left = 0;
        /// <summary>
        /// 对象左端位置
        /// </summary>
        public float Left
        {
            get
            {
                return _Left;
            }
            set
            {
                _Left = value;
            }
        }

        internal bool ContainsXYByViewHeight(float x, float y)
        {
            return x >= this._Left
                && x <= this._Left + this._Width
                && y >= this._Top
                && y <= this._Top + this._Height;
        }

        internal void SetAbsPosition(float cpx, float cpy)
        {
            this._AbsLeft = this._Left + cpx;
            this._AbsTop = this._Top + cpy;
        }

        /// <summary>
        /// 重置对象在文档视图中的绝对地址,也就是重置AbsLeft、AbsTop的状态。
        /// </summary>
        internal void ResetAbsLocation()
        {
            this._AbsTop = NonePosition;
            this._AbsLeft = NonePosition;

            //if (this._OwnerContentElement == null)
            //{
            //    this._AbsLeft = this._Left + this._OwnerDocument.Left;
            //    this._AbsTop = this._Top + this._OwnerDocument.Top;
            //}
            //else
            //{
            //    this._AbsLeft = this._Left + this._OwnerContentElement.AbsLeft;
            //    this._AbsTop = this._Top + this._OwnerContentElement.AbsTop;
            //}
        }

        internal void OffsetPosition(float dx, float dy)
        {
            if (dx != 0)
            {
                this._Left += dx;
                if (this._AbsLeft != NonePosition)
                {
                    this._AbsLeft += dx;
                }
            }
            if (dy != 0)
            {
                this._Top += dy;
                if (this._AbsTop != NonePosition)
                {
                    this._AbsTop += dy;
                }
            }
        }

        private const float NonePosition = -1234567.972f;

        private float _AbsLeft = NonePosition;
        /// <summary>
        /// 文本行的绝对左端位置
        /// </summary>
        public float AbsLeft
        {
            get
            {
                if (this._AbsLeft == NonePosition)
                {
                    if (this._OwnerDocument == null)
                    {
                        this._AbsLeft = this._Left;
                    }
                    else if (this._OwnerContentElement == null)
                    {
                        this._AbsLeft = this._Left + this._OwnerDocument.Left;
                    }
                    else
                    {
                        this._AbsLeft = this._Left + this._OwnerContentElement.AbsLeft;
                    }
                }
                return this._AbsLeft;
                //if (this._OwnerContentElement == null)
                //{
                //    return _Left + this._OwnerDocument.Left;
                //}
                //else
                //{
                //    return _Left + _OwnerContentElement.AbsLeft;
                //}
            }
        }

        /// <summary>
        /// 对象顶端位置
        /// </summary>
        private float _Top = 0;
        /// <summary>
        /// 对象顶端位置
        /// </summary>
        public float Top
        {
            get
            {
                return _Top;
            }
            set
            {
                //if (_Top != value)
                {
                    _Top = value;
                }
            }
        }

        private float _NativeTop = 0;
        /// <summary>
        /// 原始的文档行顶端位置，一般的经过排版后，文档行的Top值等于该值，
        /// 但由于进行二次分页线位置修正后，文档行的Top可能大于NativeTop值。
        /// </summary>
        [DefaultValue(0f)]
        internal float NativeTop
        {
            get
            {
                return _NativeTop;
            }
            set
            {
                _NativeTop = value;
            }
        }

        /// <summary>
        /// 文档行在文档视图中的绝对坐标位置
        /// </summary>
        public PointF AbsPosition
        {
            get
            {
                if (this._AbsLeft == NonePosition || this._AbsTop == NonePosition)
                {
                    if (this._OwnerDocument == null)
                    {
                        this._AbsLeft = this._Left;
                        this._AbsTop = this._Top;
                    }
                    else if (this._OwnerContentElement == null)
                    {
                        this._AbsLeft = this._Left + this._OwnerDocument.Left;
                        this._AbsTop = this._Top + this._OwnerDocument.Top;
                    }
                    else
                    {
                        var p = this._OwnerContentElement.AbsPosition;
                        this._AbsLeft = this._Left + p.X;
                        this._AbsTop = this._Top + p.Y;
                    }
                }
                return new PointF(this._AbsLeft, this._AbsTop);
            }
        }

        private float _AbsTop = NonePosition;
        /// <summary>
        /// 文档行在文档视图中的绝对顶端位置
        /// </summary>
        public float AbsTop
        {
            get
            {
                if (this._AbsTop == NonePosition)
                {
                    if (this._OwnerDocument == null)
                    {
                        this._AbsTop = this._Top;
                    }
                    else if (this._OwnerContentElement == null)
                    {
                        this._AbsTop = this._Top + this._OwnerDocument.Top;
                    }
                    else
                    {
                        this._AbsTop = this._Top + this._OwnerContentElement.AbsTop;
                    }
                }
                return this._AbsTop;
                //if (_AbsTopBuffered >= 0f)
                //{
                //    return _AbsTopBuffered;
                //}
                //else
                //{
                //    if (this._OwnerContentElement == null)
                //    {
                //        return _Top + this._OwnerDocument.Top;
                //    }
                //    else
                //    {
                //        return _Top + _OwnerContentElement.AbsTop;
                //    }
                //}
            }
        }
        /// <summary>
        /// 对象宽度
        /// </summary>
        private float _Width = 0;
        /// <summary>
        /// 对象宽度
        /// </summary>
        public float Width
        {
            get
            {
                return _Width;
            }
            set
            {
                //if (_Width != value)
                {
                    _Width = value;
                }
            }
        }
        /// <summary>
        /// 对象高度
        /// </summary>
        internal float _Height;
        /// <summary>
        /// 对象高度
        /// </summary>
        public float Height
        {
            get
            {
                return this._Height;
            }
            set
            {
                if (this._Height != value)
                {
                    this._Height = value;
                    this.LayoutInvalidate = true;
                    //this._LayoutVersion = -1;
                }
            }
        }

        /// <summary>
        /// 文档行的显示高度
        /// </summary>
        public float ViewHeight
        {
            get
            {
                return this.Height;
            }
        }

        /// <summary>
        /// 视图宽度
        /// </summary>
        public float ViewWidth
        {
            get
            {
                float width = this.Width;
                if (this.LastElement is DomParagraphFlagElement)
                {
                    width = width + this.OwnerDocument.PixelToDocumentUnit(7);
                }
                return width;
            }
        }


        private float _SpacingBeforeForParagraph = 0;
        /// <summary>
        /// 由于段落前间距而导致的文档行额外的上间距
        /// </summary>
        public float SpacingBeforeForParagraph
        {
            get
            {
                return _SpacingBeforeForParagraph;
            }
            set
            {
                if (_SpacingBeforeForParagraph != value)
                {
                    _SpacingBeforeForParagraph = value;
                    this.LayoutInvalidate = true;
                    //this._LayoutVersion = -1;
                }
            }
        }

        private float _SpacingAfterForParagraph = 0;
        /// <summary>
        /// 由于段落后间距而导致的文档行额外的下间距
        /// </summary>
        public float SpacingAfterForParagraph
        {
            get
            {
                return _SpacingAfterForParagraph;
            }
            set
            {
                if (_SpacingAfterForParagraph != value)
                {
                    _SpacingAfterForParagraph = value;
                    this.LayoutInvalidate = true;
                    //this._LayoutVersion = -1;
                }
            }
        }

        private float _SpecifyLineSpacing = 0f;
        /// <summary>
        /// 指定的行间距，这是由段落的行间距设置计算所得,为0表示自动行高
        /// </summary>
        public float SpecifyLineSpacing
        {
            get
            {
                return this._SpecifyLineSpacing;
            }
            set
            {
                if (this._SpecifyLineSpacing != value)
                {
                    this._SpecifyLineSpacing = value;
                    this.LayoutInvalidate = true;
                    //this._LayoutVersion = -1;
                }
            }
        }

        /// <summary>
        /// 对象底端位置
        /// </summary>
        public float Bottom
        {
            get
            {
                return this._Top + this._Height;
            }
        }
        /// <summary>
        /// 底端绝对坐标
        /// </summary>
        public float AbsBottom
        {
            get
            {
                return this.AbsTop + this._Height;
            }
        }

        /// <summary>
        /// 采用绝对坐标下的对象边框
        /// </summary>
        public RectangleF AbsBounds
        {
            get
            {
                return new RectangleF(
                    this.AbsLeft,
                    this.AbsTop,
                    this._Width,
                    this._Height);
            }
        }
        /// <summary>
        /// 文档行中所有元素的内容宽度和
        /// </summary>
        //[System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public float ContentWidth
        {
            get
            {
                float result = 0;
                if (this.Count > 0)
                {
                    for (var iCount = this.Count - 1; iCount >= 0; iCount--)
                    {
                        result += this[iCount].Width;
                    }
                }
                return result;
            }
        }

        /// <summary>
        /// 行高修正量
        /// </summary>
        public static float LineHeighFix = 0;

        ///// <summary>
        ///// 声明整个文本行的显示无效,需要重新绘制
        ///// </summary>
        //public virtual void InvalidateView()
        //{
        //          this.OwnerDocument.InvalidateView(this.Bounds);
        //}

        private float _ContentHeight = -1;

        private void SetContentHeight(float ch)
        {
            //if (this._ContentHeight != ch)
            //{
            this._ContentHeight = ch;
            //}
        }
        ///// <summary>
        ///// 获得标准行高。
        ///// </summary>
        ///// <param name="g"></param>
        ///// <param name="font"></param>
        ///// <returns></returns>
        // 
        //public static float GetStandardLineHeight(System.Drawing.Graphics g, XFontValue font)
        //{
        //    float h = font.Value.GetHeight(g);
        //    return h + StdContentTopFix;
        //}

        /// <summary>
        /// 更新行内容高度
        /// </summary>

        public void UpdateContentHeight()
        {
            this.SetContentHeight(-1);
        }
        ///// <summary>
        ///// 标准的顶端位置修正量
        ///// </summary>
        private static float StdContentTopFix = 3.125f;



        internal float _ContentTopFix = 0f;
        /// <summary>
        /// 内容顶端位置修正
        /// </summary>
        public float ContentTopFix
        {
            get
            {
                return this._ContentTopFix;
            }
        }

        /// <summary>
        /// 内容高度
        /// </summary>
        public float ContentHeight
        {
            get
            {
                if (_ContentHeight < 0)
                {
                    SetContentHeight(GetContentHeight());
                }
                return _ContentHeight;
            }
        }

        private float GetContentHeight()
        {
            if (this._OwnerDocument == null)
            {
                return 0;
            }
            if (this.Count == 1 && this[0] is DomParagraphFlagElement)
            {
                // 有不小的概率文档行只有一个段落符号。
                var pf = this[0];
                var result2 = Math.Max(pf.Height, this._OwnerDocument.DefaultStyle.DefaultLineHeight);
                if (pf.RuntimeStyle.LineSpacingStyle == LineSpacingStyle.SpaceSpecify)
                {
                    result2 = Math.Min(result2, pf.RuntimeStyle.LineSpacing);
                }
                result2 += StdContentTopFix;
                return result2;
            }
            float result = 0;
            var arr = this.InnerGetArrayRaw();
            var len = this.Count;
            //foreach (XTextElement element in this.FastForEach())
            for (var iCount = 0; iCount < len; iCount++)
            {
                var element = arr[iCount];
                result = Math.Max(element.Height, result);
            }
            result = Math.Max(result, this.OwnerDocument.DefaultStyle.DefaultLineHeight);
            if (this.IsTableLine == false)
            {
                if (this.ParagraphFlagElement != null)
                {
                    RuntimeDocumentContentStyle rs = this.ParagraphFlagElement.RuntimeStyle;
                    if (rs.LineSpacingStyle == LineSpacingStyle.SpaceSpecify)
                    {
                        result = Math.Min(result, rs.LineSpacing);
                    }
                }
            }
            result += StdContentTopFix;
            return result;
        }

        /// <summary>
        /// 获得本行中最大字体高度
        /// </summary>
        public float MaxFontHeight
        {
            get
            {
                return 50;
                //if (_MaxFontHeight < 0)
                //{
                //    bool flag = this.InGridLineContentElement;
                //    foreach (XTextElement element in this)
                //    {
                //        RuntimeDocumentContentStyle rs = element.RuntimeStyle;
                //        if (flag)
                //        {
                //            if( element is XTextParagraphFlagElement )
                //            {
                //                continue;
                //            }
                //        }

                //    }
                //    _MaxFontHeight = Math.Max(_MaxFontHeight, 50);
                //}
                //return _MaxFontHeight;
            }
        }

        /// <summary>
        /// DCWriter内部使用。判断本行是分页行（只包含一个分页符）
        /// </summary>

        public bool IsPageBreakLine
        {
            get
            {
                return this._PageBreakElement != null;
            }
            //bool result = false;
            //if (this.Count == 1)
            //{
            //    result = this[0] is XTextPageBreakElement;
            //}
            //else if (this.Count >= 2)
            //{
            //    result = this[0] is XTextPageBreakElement || this[1] is XTextPageBreakElement;
            //}
            //return result;
        }

        private DomPageBreakElement _PageBreakElement = null;
        /// <summary>
        /// 获得分页元素
        /// </summary>
        internal DomPageBreakElement PageBreakElement
        {
            get
            {
                return this._PageBreakElement;
            }
            //if (this.Count == 1)
            //{
            //    return this[0] as XTextPageBreakElement;
            //}
            //if (this.Count >= 2)
            //{
            //    if (this[0] is XTextPageBreakElement)
            //    {
            //        return ( XTextPageBreakElement ) this[0];
            //    }
            //    if (this[1] is XTextPageBreakElement)
            //    {
            //        return (XTextPageBreakElement)this[1];
            //    }
            //}
            //return null;

        }

        internal bool IsTableOrPageBreakLine
        {
            get
            {
                return this._TableElement != null || this._PageBreakElement != null;
            }
        }

        /// <summary>
        /// 判断本行是否为表格行（只包含一个表格元素）
        /// </summary>
        public bool IsTableLine
        {
            get
            {
                return this._TableElement != null;
            }
        }

        private DomTableElement _TableElement = null;

        /// <summary>
        /// 文档行中所包含的表格对象
        /// </summary>
        internal DomTableElement TableElement
        {
            get
            {
                return _TableElement;
            }
            //if (this.Count == 1)
            //{
            //    return this[0] as XTextTableElement;
            //}
            //if (this.Count >= 2)
            //{
            //    if (this[0] is XTextTableElement)
            //    {
            //        return (XTextTableElement)this[0];
            //    }
            //    if (this[1] is XTextTableElement)
            //    {
            //        return (XTextTableElement)this[1];
            //    }
            //}
            //return null;

        }

        /// <summary>
        /// 是否为只包含一个段落符号的空白行
        /// </summary>
        public bool IsEmptyLine()
        {
            return this.Count == 1 && this[0] is DomParagraphFlagElement;
        }

        /// <summary>
        /// 是否允许检测空白行(只有一个段落符号的行)
        /// </summary>
        public static bool AllowDetectBlankLine = true;

        private bool IsBlankLine()
        {
            if (AllowDetectBlankLine)
            {
                return this.GetTheSingleParagraphFlagElement() != null;
            }
            return false;
        }
        /// <summary>
        /// 使用一个数组来缓存元素转换为字符元素的结果
        /// </summary>
        private static DomCharElement[] _LocalElementsAsCharElement = null;
        /// <summary>
        /// 使用一个数组来缓存需要修正宽度的元素数组
        /// </summary>
        private static DomElement[] _LocalFixedElements = null;
        private bool ParticalRefreshStateNew(
            int startIndex,
            int elementCount,
            float localStartPosition,
            float localEndPosition,
            bool lastGroup)
        {

            if (elementCount == 0)
            {
                var jiejie = "JIEJIE.NET.SWITCH:-controlfow";
                return false;
            }
            DomDocument document = this.OwnerDocument;
            // 当前是否为空白行，有不小的概率是空白行。只包含一个段落符号，此时能避免不少计算过程。
            bool isBlankLine = this.IsBlankLine();
            DomParagraphFlagElement theOnlyPF = this[0] as DomParagraphFlagElement;
            var prs = this._ParagraphFlagElement?.RuntimeStyle;
            bool allowInvalidateLayout = document.AllowInvalidateForUILayoutOrView();
            // 最大允许行高
            float maxLineHeight = 100000000;
            DomElementList localElements = null;
            if (isBlankLine)
            {
                if (prs.LineSpacingStyle == LineSpacingStyle.SpaceSpecify)
                {
                    maxLineHeight = prs.GetLineSpacing(0, 0, DCSystem_Drawing.GraphicsUnit.Document);
                }
                localElements = this;
            }
            else
            {
                if (this.IsTableLine == false)
                {
                    if (this.ParagraphFlagElement != null)
                    {
                        if (prs.LineSpacingStyle == LineSpacingStyle.SpaceSpecify)
                        {
                            maxLineHeight = prs.GetLineSpacing(0, 0, DCSystem_Drawing.GraphicsUnit.Document);
                        }
                    }
                }
                DomElementList layouts = this.LayoutElements;
                if (startIndex == 0 && elementCount == layouts.Count)
                {
                    localElements = layouts;
                    //localElements.AddRange(this);
                }
                else
                {
                    localElements = new DomElementList(elementCount);
                    for (int iCount = 0; iCount < elementCount; iCount++)
                    {
                        localElements.FastAdd2(layouts[iCount + startIndex]);
                    }
                }
                DomLineBreakElement lb = this.LastElement as DomLineBreakElement;
                if (lb != null)
                {
                    lb.RefreshSize2(this.SafeGet(this.Count - 2));
                }
            }
            if (_LocalElementsAsCharElement == null || _LocalElementsAsCharElement.Length < localElements.Count)
            {
                _LocalElementsAsCharElement = new DomCharElement[localElements.Count];
            }
            var localElementsAsCharElement = _LocalElementsAsCharElement;
            var localElementsArray = localElements.InnerGetArrayRaw();
            for (var iCount = localElements.Count - 1; iCount >= 0; iCount--)
            {
                //var e = localElementsArray[iCount];
                localElementsAsCharElement[iCount] = localElementsArray[iCount] as DomCharElement;
                //e.CheckRuntimeStyle();
            }
            float localContentHeight = -1;
            float lineHeight = 10;
            // 采用围绕模式的文档元素对象
            List<DomElement> surroundingsElements = null;
            int contentElementCount = elementCount;
            if (isBlankLine == false && lastGroup)
            {
                // 遇到最后一组
                for (int iCount = elementCount - 1; iCount >= 0; iCount--)
                {
                    var chr = localElementsAsCharElement[iCount];
                    if (chr != null)
                    {
                        if (chr._CharValue == DomCharElement.CHAR_WHITESPACE)
                        {
                            contentElementCount--;
                            chr.WidthFix = 0;
                        }
                        else
                        {
                            break;
                        }
                    }
                    else
                    {
                        break;
                    }
                }//for
                if (contentElementCount < elementCount * 0.8)
                {
                    // 最后面的空白字符个数不足0.8则不处理
                    contentElementCount = elementCount;
                }
            }//if
            DocumentContentAlignment pAlign = prs.Align;
            float contentWidth = 0;
            if (isBlankLine)
            {
                contentWidth = theOnlyPF.Width ;
                if (theOnlyPF.Height > lineHeight)
                {
                    lineHeight = theOnlyPF.Height;
                }
            }
            else
            {
                for (int iCount = 0; iCount < contentElementCount; iCount++)
                {
                    // 遍历文档元素列表，进行元素水平位置的计算
                    DomElement element = localElementsArray[iCount];
                    if (localElementsAsCharElement[iCount] == null)
                    {
                        // 确认不是字符元素
                        if (element is DomFieldBorderElement)
                        {
                            // 动态设置字段边界元素的高度
                            var border = (DomFieldBorderElement)element;
                            if (string.IsNullOrEmpty(border.Text))
                            {
                                //if( localElements.LastElement is XTextParagraphFlagElement)
                                //{

                                //}
                                DomElement refElement = null;
                                if (border.Position == BorderElementPosition.Start)
                                {
                                    refElement = localElements.SafeGet(iCount + 1);// localElements.GetNextElement(border);
                                }
                                else
                                {
                                    refElement = localElements.SafeGet(iCount - 1);// localElements.GetPreElement(border);
                                }
                                //if (border.Position == BorderElementPosition.Start)
                                //{
                                //    refElement = localElements.SafeGet(iCount + 1);// localElements.GetNextElement(border);
                                //    if (refElement is XTextFieldBorderElement && iCount > 0)
                                //    {
                                //        // 往前查找非边界元素
                                //        for(var iCount99 = iCount -1;iCount99 >= 0;iCount99 --)
                                //        {
                                //            if ((localElements[iCount99] is XTextFieldBorderElement) == false )
                                //            {
                                //                refElement = localElements[iCount99];
                                //                break;
                                //            }
                                //        }
                                //    }
                                //}
                                //else
                                //{
                                //    refElement = localElements.SafeGet(iCount - 1);// localElements.GetPreElement(border);
                                //    if (refElement is XTextFieldBorderElement)
                                //    {
                                //        if (iCount > 2)
                                //        {
                                //            refElement = localElements.SafeGet(iCount - 2);
                                //        }
                                //        else
                                //        {
                                //            refElement = localElements.SafeGet(iCount + 1);
                                //        }
                                //    }
                                //}
                                if (refElement is DomFieldBorderElement)
                                {
                                    // 向前查找最近的非边界元素
                                    refElement = null;
                                    for (var iCount33 = iCount - 1; iCount33 >= 0; iCount33--)
                                    {
                                        if ((localElements[iCount33] is DomFieldBorderElement) == false)
                                        {
                                            refElement = localElements[iCount33];
                                            break;
                                        }
                                    }
                                    if (refElement == null)
                                    {
                                        // 向后查找第一个非边界元素
                                        for (var iCount33 = iCount + 1; iCount33 < localElements.Count; iCount33++)
                                        {
                                            if ((localElements[iCount33] is DomFieldBorderElement) == false)
                                            {
                                                refElement = localElements[iCount33];
                                                break;
                                            }
                                        }
                                    }
                                }
                                if (refElement == null || refElement is DomFieldBorderElement)
                                {
                                    border.Height = border.StandardHeight();
                                }
                                else
                                {
                                    border.Height = Math.Max(refElement.Height, border.StandardHeight());
                                }
                            }
                            //if (border.Parent is XTextInputFieldElementBase)
                            //{
                            //    XTextInputFieldElementBase field = (XTextInputFieldElementBase)border.Parent;
                            //    if (border == field.EndElement && field.RuntimeSpecifyWidth() > 0)
                            //    {

                            //    }
                            //}
                        }//if
                        else if (element is DomPageBreakElement)
                        {
                            element.Width = this.Width - 20;
                            if (element.Height == 0)
                            {
                                element.Height = this.MaxFontHeight;
                            }
                        }
                        else if (iCount > 1)
                        {
                            if (element is DomLineBreakElement)
                            {
                                var lb4 = (DomLineBreakElement)element;
                                lb4.RefreshSize2(localElementsArray[iCount - 1]);
                            }
                            else if (element is DomParagraphFlagElement
                                && DocumentBehaviorOptions._SetParagraphFlagHeightUsePreElement)
                            {
                                var preElement = localElementsArray[iCount - 1];
                                if (preElement is DomFieldBorderElement)
                                {
                                    if (iCount > 2)
                                    {
                                        var pre2 = this[iCount - 2];
                                        if (element.Height > pre2.Height && pre2.Height > 0.1)
                                        {
                                            element.Height = pre2.Height;
                                        }
                                    }
                                }
                                else if (element.Height > preElement.Height
                                    && preElement.Height > 0.1)
                                {
                                    element.Height = preElement.Height;
                                }
                            }
                        }

                        //else if (iCount > 1 && element is XTextLineBreakElement)
                        //{
                        //    var lb4 = (XTextLineBreakElement)element;
                        //    lb4.RefreshSize2(localElementsArray[iCount - 1]);
                        //}
                    }
                    RuntimeDocumentContentStyle es = element._RuntimeStyle;
                    if (es == null)
                    {
                        es = element.RuntimeStyle;
                        //throw new NullReferenceException("element._RuntimeStyle");
                    }
                        contentWidth += element.Width;
                    element._WidthFix = 0;
                    if (element.Height > lineHeight)
                    {
                        lineHeight = element.Height;
                    }
                    //element.OwnerLine = this;
                }//for
                for (var iCount2 = localElements.Count - 1; iCount2 >= contentElementCount; iCount2--)
                {
                    // 补充一些判断过程
                    if (localElementsArray[iCount2]._RuntimeStyle == null)
                    {
                        localElementsArray[iCount2].CheckRuntimeStyle();
                    }
                }
                if (lineHeight > maxLineHeight)
                {
                    lineHeight = maxLineHeight;
                }
                //lineHeight = Math.Min(lineHeight, maxLineHeight);
            }
            localContentHeight = (float)Math.Ceiling(lineHeight + StdContentTopFix);
            if (localContentHeight > this._ContentHeight)
            {
                this.SetContentHeight(localContentHeight);
            }
            float localContentTopFix = this._ContentTopFix;
            if (this._SpecifyLineSpacing > 0)
            {
                localContentTopFix = StdContentTopFix + this._SpacingBeforeForParagraph
                    + (this._SpecifyLineSpacing - localContentHeight) / 2;
            }
            else
            {
                localContentTopFix = StdContentTopFix + this._SpacingBeforeForParagraph;
            }
            if (isBlankLine == false)
            {
                if (this.IsPageBreakLine)
                {
                    localContentTopFix = StdContentTopFix;
                }
            }
            this._ContentTopFix = localContentTopFix;
            bool CanFix = elementCount > 1;
            if (CanFix)
            {
                if (pAlign == DocumentContentAlignment.Distribute)
                {
                    // 所属段落分散对齐不修正空白值
                    CanFix = true;
                }
                else
                {
                    if (localElements.InnerHasLineEndElement())// this.OwnerDocument.IsNewLine( this.LastElement ) || this.LastElement is XTextEOF )
                    {
                        // 遇到断行元素则不修正
                        CanFix = false;
                    }
                    else if (this._OwnerContentElement.PrivateLines.LastElement == this)
                    {
                        // 最后一行则不修正
                        CanFix = false;
                    }
                    else if (this._TableElement != null)
                    {
                        // 表格行或者文档节行不修正
                        CanFix = false;
                    }
                    else
                    {
                        DomElementList pc = this._OwnerContentElement.PrivateContent;
                        if (pc.SafeGet(pc.IndexOfUsePrivateContentIndex(localElements.LastElement) + 1) is DomPageBreakElement)
                        {
                            // 文档行最后一个元素的下一个元素是分页符号，也不要修正空白量
                            CanFix = false;
                        }
                    }
                }
            }
            DocumentContentAlignment align = this._Align;
            if (isBlankLine == false)
            {
                var localElementsCount = localElements.Count;
            }
            // 元素空白平均修正量
            //float fix = 0;
            // 总空白量
            if (CanFix)
            {
                float Blank = Math.Abs(localStartPosition - localEndPosition) - contentWidth;
                //if (Blank < 0)
                //{
                //    //return false;
                //}
                if (Blank > 0)
                {
                    if (Blank > 0)
                    {
                        // 计算由于元素分组而损失的空白区域个数
                        // 文档中连续的英文和数字字符之间没有修正空白,制表符和书签也拒绝修正空白
                        // 由于存在拒绝修正空白,导致空白分摊的元素减少,此处就修正这种空白分摊元素
                        // 减少而带来的影响
                        if (_LocalFixedElements == null || _LocalFixedElements.Length < contentElementCount)
                        {
                            // 准备信息缓冲区
                            _LocalFixedElements = new DomElement[contentElementCount];
                        }
                        var fixedElements = _LocalFixedElements;
                        int fixedElementsCount = 0;
                        //var controler = document.DocumentControler;

                        // 开头的连续的空格不参与分摊
                        int startSplitIndex = 0;
                        for (int iCount = 0; iCount < contentElementCount; iCount++)
                        {
                            if (localElementsAsCharElement[iCount] != null)
                            {
                                char c = localElementsAsCharElement[iCount]._CharValue;
                                if (c == ' ' || c == '\t')
                                {
                                    startSplitIndex = iCount + 1;
                                }
                                else
                                {
                                    break;
                                }
                            }
                            else
                            {
                                break;
                            }
                        }
                        if (startSplitIndex == -1)
                        {
                            // 该行全部为空格
                            startIndex = 0;
                        }
                        for (int iCount = startSplitIndex; iCount < contentElementCount - 1; iCount++)
                        {
                            DomElement element = localElementsArray[iCount];
                            if (localElementsAsCharElement[iCount] != null)
                            {
                                // 字符元素
                                DomCharElement c = localElementsAsCharElement[iCount];// (XTextCharElement)element;
                                if (c.IsBackgroundText)
                                {
                                    // 背景文字元素不修正
                                    continue;
                                }
                                //if (element.Parent is XTextFieldElementBase)
                                //{
                                //    var field = (XTextFieldElementBase)element.Parent;
                                //    if (field.FastIsBackgroundTextElement(element))// .IsBackgroundTextElement(element))
                                //    {
                                //        // 背景文字元素不修正
                                //        continue;
                                //    }
                                //}
                                if (c._CharValue == '\t')
                                {
                                    // 制表符不修正
                                    continue;
                                }
                                if (LayoutHelper.IsEnglishLetterOrDigit(c._CharValue))
                                {
                                    // 连续的英文和数字之间不修正
                                    continue;
                                }
                                if (iCount == elementCount - 1 && fixedElementsCount > 0)
                                {
                                    break;
                                }
                                fixedElements[fixedElementsCount++] = element;
                                //fixedElements.Add(element);
                            }
                            else
                            {
                                if (element is DomFieldBorderElement)
                                {
                                    if (((DomFieldBorderElement)element).Position == BorderElementPosition.Start)
                                    {
                                        // 遇到输入域起始边界元素不修正
                                        continue;
                                    }
                                    //// 遇到域边界元素不进行修正
                                    //XTextFieldBorderElement fb = (XTextFieldBorderElement)element;
                                    //if (fb.Position == BorderElementPosition.Start)
                                    //{
                                    //    // 文档域开始边界元素不修正
                                    //    continue;
                                    //}
                                }
                                else if (element is DomParagraphFlagElement)
                                {
                                    // 遇到段落元素不进行修正
                                    continue;
                                }
                                fixedElements[fixedElementsCount++] = element;
                                //fixedElements.Add(element);
                            }
                        }//for
                        if (fixedElementsCount > 1
                            && fixedElements[fixedElementsCount - 1] == localElementsArray[contentElementCount - 2]
                            && localElementsArray[contentElementCount - 1] is DomParagraphFlagElement)
                        {
                            // 当文档行最后一个元素是段落符号，可它之前的元素考虑不参与分配多余空白
                            fixedElements[fixedElementsCount - 1] = null;
                            fixedElementsCount--;
                        }
                        if (fixedElementsCount > 0)
                        {
                            float fix2 = Blank / fixedElementsCount;
                            for (var iCount9 = fixedElementsCount - 1; iCount9 >= 0; iCount9--)
                            {
                                var element = fixedElements[iCount9];
                                element.WidthFix = fix2;
                            }
                            //foreach (XTextElement element in fixedElements)
                            //{
                            //    element.WidthFix = element._RuntimeStyle.LetterSpacing + fix2;
                            //}
                            // 清空缓存的数据
                            Array.Clear(fixedElements, 0, fixedElementsCount);
                        }
                        //fixedElements = null;
                        // fix = 0;
                        Blank = 0;
                    }
                    align = DocumentContentAlignment.Justify;
                }
            }
            VerticalAlignStyle va = this.VerticalAlign;
            this._RuntimeVerticalAlign = va;
            if (this.IsCharOrParagraphFlagOnly == false
                && isBlankLine == false
                && va != VerticalAlignStyle.Top)
            {
                var len5 = localElements.Count;
                for (var iCount5 = 0; iCount5 < len5; iCount5++)
                {
                    var element = localElementsArray[iCount5];
                    if (element is DomObjectElement)
                    {
                            DomObjectElement obj = (DomObjectElement)element;
                            va = obj.RuntimeVAlign();
                            break;
                    }
                }
            }
            // 设置运行时使用的垂直对齐方式
            this._RuntimeVerticalAlign = va;

            // 计算所有字符元素的最大高度
            float charMaxHeight = 0;
            if (isBlankLine == false)
            {
                var localElementsCount = localElements.Count;
                for (var elementIndex = 0; elementIndex < localElementsCount; elementIndex++)
                {
                    var element = localElementsArray[elementIndex];// localElements.GetItemFast( elementIndex );
                    //foreach (XTextElement element in localElements.FastForEach())
                    //{
                    if (localElementsAsCharElement[elementIndex] != null)// element is XTextCharElement
                    {
                        if (charMaxHeight < element._Height)
                        {
                            charMaxHeight = element._Height;
                        }
                    }
                }//for
            }//if
            float localWidth = Math.Abs(localStartPosition - localEndPosition);
            localStartPosition = Math.Min(localStartPosition, localEndPosition);
            float leftCount = localStartPosition;// +this.PaddingLeft;
            if (align == DocumentContentAlignment.Left)
            {
                leftCount = localStartPosition;
            }
            else if (align == DocumentContentAlignment.Center)
            {
                leftCount = localStartPosition + (localWidth - contentWidth) / 2;
                if (leftCount < localStartPosition)
                {
                    leftCount = localStartPosition;
                }
            }
            else if (align == DocumentContentAlignment.Right)
            {
                leftCount = localStartPosition + (localWidth - contentWidth);
                if (leftCount < localStartPosition)
                {
                    leftCount = localStartPosition;
                }
            }
            else
            {
                leftCount = localStartPosition;// +this.PaddingLeft;
            }
            if (isBlankLine)
            {
                if (theOnlyPF.SetPositionForLineLayout(leftCount, localContentTopFix))// element.Left != newLeft || element.Top != newTop)
                {
                    // 元素位置发生改变
                    if (allowInvalidateLayout)
                    {
                        document.InvalidateLayout(theOnlyPF);
                    }
                }
            }
            else
            {
                // 字符元素基准顶端位置
                float charTop = localContentTopFix;
                switch (va)
                {
                    case VerticalAlignStyle.Top:
                        charTop = localContentTopFix;
                        break;
                    case VerticalAlignStyle.Middle:
                        charTop = localContentTopFix + (lineHeight - charMaxHeight) / 2f;
                        break;
                    case VerticalAlignStyle.Bottom:
                        charTop = localContentTopFix + lineHeight - charMaxHeight;
                        break;
                    default:
                        charTop = localContentTopFix + lineHeight - charMaxHeight;
                        break;
                }
                var charTop_charMaxHeight = charTop + charMaxHeight;
                for (int iCount = 0; iCount < elementCount; iCount++)
                {
                    //if (iCount == 48)
                    //{
                    //}
                    DomElement element = localElementsArray[iCount];// localElements.GetItemFast(iCount);
                    float newLeft = leftCount;
                    float ctf = localContentTopFix;
                    float newTop = 0;
                    var isCharElement = localElementsAsCharElement[iCount] != null;
                    if (surroundingsElements == null || surroundingsElements.Contains(element) == false)
                    {
                        if (isCharElement)// element is XTextCharElement
                        {
                            newTop = charTop_charMaxHeight - element._Height;
                        }
                        else
                        {
                            switch (va)
                            {
                                case VerticalAlignStyle.Top:
                                    newTop = localContentTopFix;
                                    break;
                                case VerticalAlignStyle.Middle:
                                    newTop = localContentTopFix + (lineHeight - element.Height) / 2.0f;
                                    break;
                                case VerticalAlignStyle.Bottom:
                                    newTop = ctf + lineHeight - element.Height;
                                    break;
                                default:
                                    newTop = ctf + lineHeight - element.Height;
                                    break;
                            }//switch
                        }
                    }
                    else
                    {
                        newTop = localContentTopFix;
                    }
                    if (element.SetPositionForLineLayout(newLeft, newTop))// element.Left != newLeft || element.Top != newTop)
                    {
                        // 元素位置发生改变
                        if (allowInvalidateLayout && isCharElement == false)
                        {
                            document.InvalidateLayout(element);
                        }
                    }
                    if (isCharElement)
                    {
                        leftCount += element._Width + element._WidthFix;
                    }
                    else
                    {
                        leftCount += element.Width + element.WidthFix;
                    }
                    if (surroundingsElements != null && surroundingsElements.Contains(element))
                    {
                        RectangleF b = element.Bounds;
                        b.Offset(this.Left, this.Top);
                    }
                }//for
            }
            Array.Clear(localElementsAsCharElement, 0, localElements.Count);
            //}
            //this.Height += this.LineSpacing  ;
            return true;
        }

        internal void UpdateOwnerLine()
        {
            //if (XTextDocument.IsWebApplicationMode() == false)
            //{
            //    if (this.Capacity > this.Count + 20)
            //    {
            //        this.Capacity = this.Count;
            //    }
            //}
            var arr = this.InnerGetArrayRaw();
            for (int iCount = this.Count - 1; iCount >= 0; iCount--)
            {
                arr[iCount]._OwnerLine = this;
            }
        }

        /// <summary>
        /// 刷新状态
        /// </summary>
        /// <returns>操作是否成功</returns>
        internal bool RefreshStateNew()
        {
            this.LayoutInvalidate = false;
            if (this.Count == 0)
            {
                return false;
            }
            this.SetContentHeight(-1);
            var layoutElements = this;
            bool isBlankLine = this.IsBlankLine();
            bool result = false;
            if (isBlankLine)
            {
                if (ParticalRefreshStateNew(
                    0,
                    1,
                    this.PaddingLeft,
                    this.Width - this.PaddingRight,
                    true))
                {
                    result = true;
                }
            }
            else
            {
                // 整体进行内容排版操作
                if (ParticalRefreshStateNew(
                    0,
                    this.LayoutElements.Count,
                    this.PaddingLeft,
                    this.Width - this.PaddingRight,
                    true))
                {
                    result = true;
                }
            }



            // 计算新的行高

            this._SpecifyLineSpacing = this._OwnerContentElement.GetRuntimeSpecifyLineSpacing(this);

            float nh = this._SpacingAfterForParagraph + this._SpacingBeforeForParagraph + LineHeighFix;
            if (this._SpecifyLineSpacing > 0)
            {
                nh += this._SpecifyLineSpacing;
            }
            else
            {
                nh += this.ContentHeight + StdContentTopFix;
                nh = (float)Math.Ceiling(nh);
            }
            bool hasGlobalGridLineLayout = this._OwnerDocument._GlobalGridInfo != null
                && this._OwnerDocument._GlobalGridInfo.AlignToGridLine;
            this.Height = nh;
            if (isBlankLine == false)
            {
                if (this.IsTableLine)
                {
                    //包含表格
                    DomTableElement table = this.TableElement;
                    if (hasGlobalGridLineLayout)
                    {
                        // 压缩行高
                        table.Top = 0;
                        this.Height = table.Height;
                    }
                    table.ResetLinesAbsLocation();
                    var nw = this.LastElement.Right;
                    if (this._Width < nw)
                    {
                        this._Width = nw;
                    }
                }
                else if (this.IsPageBreakLine)
                {
                    // 包含了分页符
                    this.PageBreakElement.Top = 0;
                    this.Height = this.PageBreakElement.Height;
                }
                else if (this.LastElement is DomParagraphFlagElement)
                {
                    var p2 = (DomParagraphFlagElement)this.LastElement;
                    if (p2.Height < 40 && this.Height > p2.Height + 5)
                    {
                        p2.Height = Math.Min(
                            this.OwnerDocument._ParagraphFlagSize.Height,
                            this.Height - p2.Top - 4);
                    }
                }
            }
            this.FixHeightForGridLine();
            //this._NeedCheckParticalDirection = false;
            this.LayoutInvalidate = false;
            this.ResetAbsLocation();
            return result;
        }

        /// <summary>
        /// 根据网格线设置来调整行高
        /// </summary>
        internal void FixHeightForGridLine()
        {
            if (this._OwnerContentElement == null)
            {
                return;
            }
            this.AlignToGridLine = false;
            DCGridLineInfo info = this._OwnerContentElement.InnerRuntimeGridLine();
            if (info != null)
            {
                // 根据网格线设置来调整行高
                if (info.RuntimeAlignToGridLine == false)
                {
                }
                else
                {
                    // 需要对齐到网格线
                    this.AlignToGridLine = true;
                    float h = info.FixLineHeight(this.Height);
                    if (this.Height != h)
                    {
                        float dh = (h - this.Height) / 2;
                        if (this.IsTableLine)
                        {

                        }
                        else
                        {
                            this._ContentTopFix += dh;
                        }
                        this.Height = h;
                        if (dh > 0)
                        {
                            var arr = this.InnerGetArrayRaw();
                            var len = this.Count;
                            if (this._TableElement == null)
                            {
                                for (var i = 0; i < len; i++)
                                {
                                    arr[i]._Top += dh;
                                }
                            }
                            else
                            {
                                for (var i = 0; i < len; i++)
                                {
                                    var e = arr[i];
                                    if (e is DomTableElement)
                                    {

                                    }
                                    else
                                    {
                                        e.Top += dh;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }


        ///// <summary>
        ///// 判断是否可以添加元素
        ///// </summary>
        ///// <param name="element"></param>
        ///// <returns></returns>
        //      internal bool CanAdd(XTextElement element)
        //      {
        //          float WidthCount = this.ContentWidth;
        //          if (WidthCount + element.Width + this._Spacing > this.Width - this.PaddingLeft)
        //          {
        //              return false;
        //          }
        //          else
        //          {
        //              return true;
        //          }
        //      }

        //private float ClientWidth()
        //{

        //    return this._Width - this._PaddingLeft - this._PaddingRight;

        //}



        //private float _WidthCountForAddElement = 0;// 为了省点内存，不得不用 this._AbsLeft 字段来临时替换。

        internal bool AddElement(DomElement element, DomElement preElement)
        {
            if (this._TableElement != null || this._PageBreakElement != null)// this.IsTableLine() || this.IsPageBreakLine())
            {
                if (DomContentElement.CanAddElementForce(element) == false)
                {
                    // 无法强制添加元素
                    var jiejie = "JIEJIE.NET.SWITCH:-controlfow";
                    return false;
                }
            }
            RuntimeDocumentContentStyle es = element._RuntimeStyle;
            bool elementIsParagraphFlag = false;
            bool elementIsCharOrParagraphFlag = false;
            if (element is DomCharElement)
            {
                elementIsCharOrParagraphFlag = true;
            }
            else if (element is DomParagraphFlagElement)
            {
                elementIsCharOrParagraphFlag = true;
                elementIsParagraphFlag = true;
            }
            if (elementIsCharOrParagraphFlag == false)
            {
                this.AllIsCharOrParaphFlag = false;
                this.IsCharOrParagraphFlagOnly = false;
            }
            float elementWidth = element.Width;
            bool noElement = this.Count == 0;
            if (noElement)
            {
                this.SetContentHeight(-1);
                this._AbsLeft = 0;// this._WidthCountForAddElement = 0;
            }
            if (elementIsCharOrParagraphFlag == false && element is DomFieldBorderElement)
            {
                DomFieldBorderElement b = (DomFieldBorderElement)element;
                if (b.Parent is DomInputFieldElementBase)
                {
                    DomInputFieldElementBase field = (DomInputFieldElementBase)b.Parent;
                    // 根据输入域固定宽度的设置来修正结尾边框元素的宽度
                    elementWidth = element.Width;
                    //if (field.EndElement == element)
                    //{
                    //    // 根据输入域固定宽度的设置来修正结尾边框元素的宽度
                    //    field.FixEndElementWidth();
                    //    elementWidth = element.Width;
                    //}
                }
            }

            if (noElement)
            {
                // 文档行没有内容，则肯定能添加元素
                //intLeftCount = 0 ;
                DomParagraphFlagElement p = this._ParagraphFlagElement;
                bool firstElementInParagraph = false;
                this.FastAdd2(element);
                CheckElementype(element);
                this._AbsLeft += elementWidth;// this._WidthCountForAddElement += elementWidth;
                if (elementIsCharOrParagraphFlag
                    || ((element is DomTableElement) == false))
                {
                    //p = element.OwnerParagraphEOF;
                    if (p != null)
                    {
                        RuntimeDocumentContentStyle rs = p.RuntimeStyle;
                        if (element == p.ParagraphFirstContentElement)
                        {
                            firstElementInParagraph = true;
                            this._PaddingLeft = rs.LeftIndent + rs.FirstLineIndent;
                        }
                        else if (preElement is DomTableElement)
                        {
                            this._PaddingLeft = rs.LeftIndent + rs.FirstLineIndent;
                        }
                        else
                        {
                            this._PaddingLeft = rs.LeftIndent;
                        }
                    }
                    else
                    {
                        this._PaddingLeft = 0;
                    }
                    //this.Width = this.Width - this.PaddingLeft;
                }
                element.Left = this._PaddingLeft;
                if (firstElementInParagraph)
                {
                    if (p.ListStyle() != ParagraphListStyle.None)
                    {
                        var item = new DomParagraphListItemElement();
                        item.SetOwnerLine(this);
                        RuntimeDocumentContentStyle rs = element.RuntimeStyle;
                        float size = rs.Font.GetHeight(DCSystem_Drawing.GraphicsUnit.Document);
                        item.Width = size;
                        item.Height = size;
                        using (DCGraphics g = element.InnerCreateDCGraphics())
                        {
                            item.InnerRefreshSize(g);
                        }
                        //if (this.ParagraphListStyle == Dom.ParagraphListStyle.NumberedList)
                        //{
                        //    item.Width = item.Width * 2;
                        //}
                        item.OwnerDocument = element.OwnerDocument;
                        item.StyleIndex = element.StyleIndex;
                        this.Insert(0, item);
                        CheckElementype(item);
                        this._AbsLeft += item.Width;// this._WidthCountForAddElement += item.Width ;
                        p._ListItemElement = item;
                        // float size = rs.Font.GetHeight( 
                    }
                    else if (p._ListItemElement != null)
                    {
                        p._ListItemElement.Dispose();
                        p._ListItemElement = null;
                    }
                }
                return true;
            }
            if (elementIsParagraphFlag == false)//(element is XTextParagraphFlagElement) == false)
            {
                // 在排版中段落元素不计宽度，可以无条件的添加到文档行中。
                // 若文档行中剩余的空间不能放下字符，则无法添加
                if (this._AbsLeft // this._WidthCountForAddElement
                    + elementWidth
                    > this._Width - this._PaddingLeft - this._PaddingRight + 0.1f)// 这里加上0.1的增量用于低调浮点运算误差。
                {
                    //_AddElement_TickInfo.EndOneTick();
                    bool exit = true;
                    if (this.Count == 1 && this[0] is DomFieldBorderElement)
                    {
                        DomFieldBorderElement fb = (DomFieldBorderElement)this[0];
                        if (fb.Width < 5)
                        {
                            exit = false;
                        }
                    }
                    if (exit)
                    {
                        if (element is DomFieldBorderElement)
                        {
                            DomFieldBorderElement fb = (DomFieldBorderElement)element;
                            if (fb.Position == BorderElementPosition.End)
                            {
                                exit = false;
                            }
                        }
                    }
                    if (exit)
                    {
                        return false;
                    }
                }
            }
            DomElement localPreElement = this.LastElement;// (XTextElement)this[this.Count - 1];
            element.Left = localPreElement.Left + localPreElement.Width;

            if (elementIsParagraphFlag/* element is XTextParagraphFlagElement */&& DocumentBehaviorOptions._SetParagraphFlagHeightUsePreElement)
            {
                if (localPreElement is DomFieldBorderElement)
                {
                    if (this.Count > 2)
                    {
                        var pre2 = this[this.Count - 2];
                        if (element.Height > pre2.Height && pre2.Height > 0.1)
                        {
                            element.Height = pre2.Height;
                        }
                    }
                }
                else if (element.Height > localPreElement.Height
                    && localPreElement.Height > 0.1)
                {
                    element.Height = localPreElement.Height;
                }
            }
            this.FastAdd2(element);// this.AddRaw(element);
            //CheckElementype(element);
            if (element is DomCharElement)
            {
                DomCharElement chr = (DomCharElement)element;
                if (chr._CharValue == '\t')
                {
                    chr.SetWidthForTab();
                    elementWidth = chr.Width;
                }
            }
            else
            {
                CheckElementype(element);
            }
            this._AbsLeft += elementWidth ;
            return true;
        }

        /// <summary>
        /// 检查文档元素类型
        /// </summary>
        /// <param name="element"></param>
        private void CheckElementype(DomElement element)
        {
            if (element is DomTableElement)
            {
                this._TableElement = (DomTableElement)element;
            }
            else if (element is DomPageBreakElement)
            {
                this._PageBreakElement = (DomPageBreakElement)element;
            }
        }

        /// <summary>
        /// 删除并返回最后一个元素,并保证列表中有内容
        /// </summary>
        /// <returns>返回的最后一个元素</returns>
        internal DomElement PopupLastElement()
        {
            if (this.Count > 1)
            {
                DomElement e = this[this.Count - 1];
                this.RemoveAt(this.Count - 1);
                return e;
            }
            return null;
        }

#if !RELEASE
        /// <summary>
        /// 返回表示对象数据的字符串
        /// </summary>
        /// <returns>字符串</returns>
        public override string ToString()
        {
            System.Text.StringBuilder str = new System.Text.StringBuilder();
            foreach (DomElement e in this.FastForEach())
            {
                str.Append(e.ToString());
            }
            return str.ToString();
        }
#endif
        internal void InnerDispose()
        {
            this.ClearAndEmpty();
            this._OwnerContentElement = null;
            this._OwnerDocument = null;
            this._OwnerPage = null;
            this._PageBreakElement = null;
            this._ParagraphFlagElement = null;
            this._TableElement = null;
        }
    }
}