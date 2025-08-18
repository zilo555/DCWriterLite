using System;
using System.Collections.Generic;
using System.Text;
using DCSoft.Writer.Dom ;
using System.Runtime.InteropServices;

namespace DCSoft.Writer.Controls
{
    /// <summary>
    /// 元素提示文本信息对象
    /// </summary>
    public class ElementToolTip
    {
        /// <summary>
        /// 初始化对象
        /// </summary>
        public ElementToolTip()
        {
        }

        /// <summary>
        /// 初始化对象
        /// </summary>
        /// <param name="element">文档元素对象</param>
        /// <param name="text">文本</param>
        public ElementToolTip(DomElement element, string text)
        {
            this._Element = element;
            this._Text = text;
        }

        /// <summary>
        /// 比较对象数据是否一致
        /// </summary>
        /// <param name="tip"></param>
        /// <returns></returns>
        public bool EqualsValue(ElementToolTip tip)
        {
            if (tip == null)
            {
                return false;
            }
            if (tip == this)
            {
                return true;
            }
            return this._ContentType == tip._ContentType
                && this._DisplayOnce == tip._DisplayOnce
                && this._Element == tip._Element
                && this._Level == tip._Level
                && this._Style == tip._Style
                && this._Text == tip._Text
                && this._Title == tip._Title;
        }

        private DomElement _Element = null;
        /// <summary>
        /// 元素
        /// </summary>
        public DomElement Element
        {
            get { return _Element; }
            set { _Element = value; }
        }

        private string _Title = null;
        /// <summary>
        /// 标题
        /// </summary>
        public string Title
        {
            get { return _Title; }
            set { _Title = value; }
        }

        private string _Text = null;
        /// <summary>
        /// 文本
        /// </summary>
        public string Text
        {
            get { return _Text; }
            set { _Text = value; }
        }

        private ToolTipStyle _Style = ToolTipStyle.ToolTip;
        /// <summary>
        /// 样式
        /// </summary>
        public ToolTipStyle Style
        {
            get { return _Style; }
            set { _Style = value; }
        }

        private ToolTipLevel _Level = ToolTipLevel.Normal;
        /// <summary>
        /// 等级
        /// </summary>
        public ToolTipLevel Level
        {
            get { return _Level; }
            set { _Level = value; }
        }

        private ToolTipContentType _ContentType = ToolTipContentType.ElementToolTip;
        /// <summary>
        /// 内容类型
        /// </summary>
        public ToolTipContentType ContentType
        {
            get { return _ContentType; }
            set { _ContentType = value; }
        }
        private bool _DisplayOnce = false ;
        /// <summary>
        /// 一次性的提示文本
        /// </summary>
        public bool DisplayOnce
        {
            get { return _DisplayOnce; }
            set { _DisplayOnce = value; }
        }
    }
    /// <summary>
    /// 提示文本信息容器对象
    /// </summary>
    internal class ElementToolTipContainer 
    {
        /// <summary>
        /// 初始化对象
        /// </summary>
        public ElementToolTipContainer()
        {
        }

        private Dictionary<DomElement, ElementToolTip> _ToolTips 
            = new Dictionary<DomElement, ElementToolTip>();

        /// <summary>
        /// 获得指定元素的项目
        /// </summary>
        /// <param name="element">文档元素对象</param>
        /// <returns>获得的项目</returns>
        public ElementToolTip this[DomElement element]
        {
            get
            {
                if (element == null)
                {
                    return null;
                }
                ElementToolTip tip = null;
                if (this._ToolTips.TryGetValue(element, out tip))
                {
                    return tip;
                }
                return null;
            }
        }

        /// <summary>
        /// 清除所有数据
        /// </summary>
        public void Clear()
        {
            this._ToolTips.Clear();
            IncreateVersion();
        }

        private int _Version = 0;
        /// <summary>
        /// 列表的内容版本号，对列表内容的所有的修改都增加该版本号
        /// </summary>
        public int Version
        {
            get
            {
                return _Version; 
            }
            set
            {
                _Version = value;
            }
        }
        /// <summary>
        /// 增加版本号
        /// </summary>
        public void IncreateVersion()
        {
            _Version++;
        }

    }
    /// <summary>
    /// 提示文本样式
    /// </summary>
    public enum ToolTipStyle
    {
        /// <summary>
        /// 普通提示文本，当鼠标移动到元素上就显示提示
        /// </summary>
        ToolTip  ,
        /// <summary>
        /// 静态提示文本，一直显示在用户界面上
        /// </summary>
        Static ,
        /// <summary>
        /// 右边提示文本，一直显示在用户界面的右侧
        /// </summary>
        RightSide
    }

    /// <summary>
    /// 提示文本内容样式
    /// </summary>
    [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
    public enum ToolTipContentType
    {
        /// <summary>
        /// 未知类型
        /// </summary>
        Unknow,
        /// <summary>
        /// 元素配置的提示信息
        /// </summary>
        ElementToolTip,
        /// <summary>
        /// 校验结果提示信息
        /// </summary>
        ValidateResult 
    }

    /// <summary>
    /// 提示文本等级
    /// </summary>
    public enum ToolTipLevel
    {
        /// <summary>
        /// 普通提示文本
        /// </summary>
        Normal = 0 ,
        /// <summary>
        /// 警告提示文本
        /// </summary>
        Warring = 1,
        /// <summary>
        /// 错误提示文本
        /// </summary>
        Error =2
    }
}
     
