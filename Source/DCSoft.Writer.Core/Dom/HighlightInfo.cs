using System;
using System.Collections.Generic;
using System.Text;
using DCSoft.Common;

namespace DCSoft.Writer.Dom
{
    /// <summary>
    /// 高亮度显示区域
    /// </summary>
    /// <remarks>编写 袁永福</remarks>
    public class HighlightInfo
    {
        /// <summary>
        /// 初始化对象
        /// </summary>
        /// <param name="range">区域</param>
        /// <param name="backColor">背景色</param>
        /// <param name="color">前景色</param>
        public HighlightInfo(DomElement ownerElement , DCRange range, Color backColor, Color color , HighlightActiveStyle style )
        {
            if (range == null)
            {
                throw new ArgumentNullException("range");
            }
            this._Range = range;
            this._BackColor = backColor;
            this._Color = color;
            this._OwnerElement = ownerElement;
            this._ActiveStyle = style;
        }

        /// <summary>
        /// 初始化对象
        /// </summary>
        /// <param name="range">区域</param>
        /// <param name="backColor">背景色</param>
        /// <param name="color">前景色</param>
        public HighlightInfo(DomElement ownerElement, DCRange range, Color backColor )
        {
            if (range == null)
            {
                throw new ArgumentNullException("range");
            }
            this._Range = range;
            this._BackColor = backColor;
            this._OwnerElement = ownerElement;
        }

        public void Clear()
        {
            this._OwnerElement = null;
            if (this._Range != null)
            {
                this._Range.Clear();
                this._Range = null;
            }
        }

        private bool _HeightClass = false;
        /// <summary>
        /// 高等级
        /// </summary>
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public bool HeightClass
        {
            get
            {
                return this._HeightClass;
            }
            set
            {
                this._HeightClass = value;
            }
        }
        
        internal DomElement _OwnerElement = null;
        /// <summary>
        /// 该高亮度显示区域相关的文档元素对象
        /// </summary>
        public DomElement OwnerElement
        {
            get
            {
                return _OwnerElement; 
            }
        }

        private DCRange _Range = null;
        /// <summary>
        /// 高亮度区域
        /// </summary>
        public DCRange Range
        {
            get
            {
                return _Range; 
            }
        }
        private static readonly Color _SystemColors_Highlight = SystemColors.Highlight;
        private Color _BackColor = _SystemColors_Highlight;
        /// <summary>
        /// 背景色
        /// </summary>
        public Color BackColor
        {
            get
            {
                return _BackColor; 
            }
            set
            {
                _BackColor = value;
            }
        }

        private Color _Color = SystemColors.HighlightText;
        /// <summary>
        /// 文本颜色
        /// </summary>
        public Color Color
        {
            get
            {
                return _Color; 
            }
            set
            {
                _Color = value;
            }
        }

        private HighlightActiveStyle _ActiveStyle = HighlightActiveStyle.Hover;
        /// <summary>
        /// 激活模式
        /// </summary>
        public HighlightActiveStyle ActiveStyle
        {
            get
            {
                return _ActiveStyle; 
            }
        }

        /// <summary>
        /// 判断指定的元素是否处于高亮度显示区域中
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public bool Contains(DomElement element)
        {
            return this._Range.Contains(element);
        }
#if !(RELEASE || LightWeight)
        /// <summary>
        /// 返回表示对象的字符串
        /// </summary>
        /// <returns>字符串</returns>
        public override string ToString()
        {
            return this.Range.ToString() + " BC:" + this.BackColor.ToString();
        }
#endif
    }
        

    /// <summary>
    /// 高亮度激活模式
    /// </summary>
     
    [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
    public enum HighlightActiveStyle
    {
        /// <summary>
        /// 鼠标悬停时才激活
        /// </summary>
        Hover,
        /// <summary>
        /// 静态的，一直处于激活状态,但不能打印
        /// </summary>
        Static,
        /// <summary>
        /// 永久的，一直处于激活状态，而且能打印
        /// </summary>
        AllTime
    }

    /// <summary>
    /// 高亮度显示区域列表
    /// </summary>

#if !RELEASE
    [System.Diagnostics.DebuggerDisplay("Count={ Count }")]
    [System.Diagnostics.DebuggerTypeProxy(typeof(DCSoft.Common.ListDebugView))]
#endif
    public class HighlightInfoList : List<HighlightInfo>
    {
        public HighlightInfoList()
        { }
        public HighlightInfoList(int cap ) : base( cap)
        {

        }
        /// <summary>
        /// 最后一次检索的元素对象
        /// </summary>
        private DomElement _LastElement = null;
        /// <summary>
        /// 最后一次检索的值
        /// </summary>
        private HighlightInfo _LastValue = null;
        /// <summary>
        /// 查找包含自定元素的
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public HighlightInfo this[DomElement element]
        {
            get
            {
                if (element.ContentIndex < 0)
                {
                    return null;
                }
                if (this._LastElement == element)
                {
                    // 使用最后一次检索的值
                    return this._LastValue;
                }
                this._LastElement = element;
                this._LastValue = null;
                // 首先进行快速检索
                DomElement field = element.GetOwnerParent( typeof( DomInputFieldElement ) , false ) ;
                if (field != null)
                {
                    for (int iCount = this.Count - 1; iCount >= 0; iCount--)
                    {
                        HighlightInfo info = this[iCount];
                        if (info._OwnerElement == field)
                        {
                            if (info.Contains(element))
                            {
                                this._LastValue = info;
                                return info;
                            }
                        }
                    }//for
                }
                DomElementList parents = new DomElementList();
                DomElement p = element;
                if (element is DomCharElement || element is DomParagraphFlagElement)
                {
                    // 纯文本
                    p = element.Parent;
                }
                while (p != null)
                {
                    if (p is DomDocumentContentElement)
                    {
                        break;
                    }
                    parents.Add(p);
                    p = p.Parent;
                }
                if (parents.Count == 0)
                {
                    // 正文中的纯文本，不可能高亮度显示
                    return null;
                }
                for (int iCount = this.Count - 1; iCount >= 0; iCount--)
                {
                    HighlightInfo info = this[iCount];
                    if (info.OwnerElement != null)
                    {
                        if (parents.Contains(info.OwnerElement))
                        {
                            if (info.OwnerElement == element.Parent)
                            {
                                if (info.Contains(element))
                                {
                                    this._LastValue = info;
                                    return info;
                                }
                            }
                        }//if
                    }
                }//for
                for (int iCount = this.Count - 1; iCount >= 0; iCount--)
                {
                    HighlightInfo info = this[iCount];
                    if (info.OwnerElement == null)
                    {
                        if (this[iCount].Contains(element))
                        {
                            this._LastValue = this[iCount];
                            return this[iCount];
                        }
                    }
                }
                return null;
            }
        }
    }
}
