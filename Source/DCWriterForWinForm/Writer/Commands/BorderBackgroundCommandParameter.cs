using System;
using System.Collections.Generic;
using System.Text;
using DCSoft.Writer.Dom;
using DCSoft.Drawing ;

namespace DCSoft.Writer.Commands
{
    /// <summary>
    /// 边框命令参数对象
    /// </summary>
    public partial class BorderBackgroundCommandParameter
    {
        /// <summary>
        /// 初始化对象
        /// </summary>
        public BorderBackgroundCommandParameter()
        {
        }
        private DomElementList _Elements = null;
        /// <summary>
        /// 指定参与处理的文档元素列表 
        /// </summary>
        public DomElementList Elements
        {
            get { return _Elements; }
            set { _Elements = value; }
        }

        private bool _EnabledBorderSettings = true;
        /// <summary>
        /// 是否启用边框线的设置
        /// </summary>
        public bool EnabledBorderSettings
        {
            get
            {
                return this._EnabledBorderSettings;
            }
            set
            {
                this._EnabledBorderSettings = value;
            }
        }
        private bool _EnabledBackgroundColor = true;
        /// <summary>
        /// 是否启用背景色的设置
        /// </summary>
        public bool EnabledBackgroundColor
        {
            get
            {
                return this._EnabledBackgroundColor;
            }
            set
            {
                this._EnabledBackgroundColor = value;
            }
        }
        private bool _TopBorder = true;
        /// <summary>
        /// 是否显示顶端边框线
        /// </summary>
        public bool TopBorder
        {
            get { return _TopBorder; }
            set { _TopBorder = value; }
        }

        private bool _MiddleHorizontalBorder = true;
        /// <summary>
        /// 是否显示水平中间的边框线
        /// </summary>
        public bool MiddleHorizontalBorder
        {
            get { return _MiddleHorizontalBorder; }
            set { _MiddleHorizontalBorder = value; }
        }

        private bool _BottomBorder = true;
        /// <summary>
        /// 是否显示低端边框线
        /// </summary>
        public bool BottomBorder
        {
            get { return _BottomBorder; }
            set { _BottomBorder = value; }
        }

        private bool _LeftBorder = true;
        /// <summary>
        /// 是否显示左端边框线
        /// </summary>
        public bool LeftBorder
        {
            get { return _LeftBorder; }
            set { _LeftBorder = value; }
        }

        private bool _CenterVerticalBorder = true;
        /// <summary>
        /// 是否显示垂直居中的边框线
        /// </summary>
        public bool CenterVerticalBorder
        {
            get { return _CenterVerticalBorder; }
            set { _CenterVerticalBorder = value; }
        }

        private bool _RightBorder = true;
        /// <summary>
        /// 是否显示右边的边框线
        /// </summary>
        public bool RightBorder
        {
            get { return _RightBorder; }
            set { _RightBorder = value; }
        }

        private DCSystem_Drawing.Color _BorderLeftColor = DCSystem_Drawing.Color.Black;
        /// <summary>
        /// 边框线颜色
        /// </summary>
        public DCSystem_Drawing.Color BorderLeftColor
        {
            get { return _BorderLeftColor; }
            set { _BorderLeftColor = value; }
        }

        private DCSystem_Drawing.Color _BorderTopColor = DCSystem_Drawing.Color.Black;
        /// <summary>
        /// 边框线颜色
        /// </summary>
        public DCSystem_Drawing.Color BorderTopColor
        {
            get { return _BorderTopColor; }
            set { _BorderTopColor = value; }
        }
        private DCSystem_Drawing.Color _BorderRightColor = DCSystem_Drawing.Color.Black;
        /// <summary>
        /// 边框线颜色
        /// </summary>
        public DCSystem_Drawing.Color BorderRightColor
        {
            get { return _BorderRightColor; }
            set { _BorderRightColor = value; }
        }
        private DCSystem_Drawing.Color _BorderBottomColor = DCSystem_Drawing.Color.Black;
        /// <summary>
        /// 边框线颜色
        /// </summary>
        public DCSystem_Drawing.Color BorderBottomColor
        {
            get { return _BorderBottomColor; }
            set { _BorderBottomColor = value; }
        }

        private DashStyle _BorderStyle = DashStyle.Solid;
        /// <summary>
        /// 边框线样式
        /// </summary>
        public DashStyle BorderStyle
        {
            get { return _BorderStyle; }
            set { _BorderStyle = value; }
        }

        private float _BorderWidth = 1;
        /// <summary>
        /// 边框线宽度
        /// </summary>
        public float BorderWidth
        {
            get { return _BorderWidth; }
            set { _BorderWidth = value; }
        }

        private DCSystem_Drawing.Color _BackgroundColor = DCSystem_Drawing.Color.Transparent;
        /// <summary>
        /// 背景色
        /// </summary>
        public DCSystem_Drawing.Color BackgroundColor
        {
            get { return _BackgroundColor; }
            set { _BackgroundColor = value; }
        }

        private StyleApplyRanges _ApplyRange = StyleApplyRanges.Text;
        /// <summary>
        /// 设置应用范围
        /// </summary>
        public StyleApplyRanges ApplyRange
        {
            get
            {
                return _ApplyRange; 
            }
            set
            {
                _ApplyRange = value; 
            }
        }
         
        private BorderSettingsStyle _BorderSettingsStyle = BorderSettingsStyle.None;
        /// <summary>
        /// 边框设置样式
        /// </summary>
        public BorderSettingsStyle BorderSettingsStyle
        {
            get { return _BorderSettingsStyle; }
            set { _BorderSettingsStyle = value; }
        }

        /// <summary>
        /// 设置边框样式
        /// </summary>
         
        public void SetBorderSettingsStyle()
        {
            if (this.LeftBorder
                && this.TopBorder
                && this.RightBorder
                && this.BottomBorder)
            {
                if (this.CenterVerticalBorder && this.MiddleHorizontalBorder)
                {
                    this.BorderSettingsStyle = BorderSettingsStyle.Both;
                }
                else if (this.CenterVerticalBorder == false
                    && this.MiddleHorizontalBorder == false)
                {
                    this.BorderSettingsStyle = BorderSettingsStyle.Rectangle;
                }
                else
                {
                    this.BorderSettingsStyle = BorderSettingsStyle.Custom;
                }
            }
            else if (this.LeftBorder == false
                && this.TopBorder == false
                && this.RightBorder == false
                && this.BottomBorder == false
                && this.CenterVerticalBorder == false
                && this.MiddleHorizontalBorder == false)
            {
                this.BorderSettingsStyle = BorderSettingsStyle.None;
            }
            else
            {
                this.BorderSettingsStyle = BorderSettingsStyle.Custom;
            }
        }

        /// <summary>
        /// 设置文档样式信息对象
        /// </summary>
        /// <param name="style">样式信息</param>
        /// <returns>操作是否修改了样式信息内容</returns>
        public bool SetContentStyle(ContentStyle style)
        {
            bool modified = false;
            if (this._EnabledBorderSettings)
            {
                if (style.BorderLeft != this.LeftBorder)
                {
                    style.BorderLeft = this.LeftBorder;
                    modified = true;
                }
                if (style.BorderTop != this.TopBorder)
                {
                    style.BorderTop = this.TopBorder;
                    modified = true;
                }
                if (style.BorderRight != this.RightBorder)
                {
                    style.BorderRight = this.RightBorder;
                    modified = true;
                }
                if (style.BorderBottom != this.BottomBorder)
                {
                    style.BorderBottom = this.BottomBorder;
                    modified = true;
                }
                if (style.BorderTopColor != this.BorderTopColor)
                {
                    style.BorderTopColor = this.BorderTopColor;
                    modified = true;
                }
                if (style.BorderLeftColor != this.BorderLeftColor)
                {
                    style.BorderLeftColor = this.BorderLeftColor;
                    modified = true;
                }
                if (style.BorderRightColor != this.BorderRightColor)
                {
                    style.BorderRightColor = this.BorderRightColor;
                    modified = true;
                }
                if (style.BorderBottomColor != this.BorderBottomColor)
                {
                    style.BorderBottomColor = this.BorderBottomColor;
                    modified = true;
                }
                if (style.BorderStyle != this.BorderStyle)
                {
                    style.BorderStyle = this.BorderStyle;
                    modified = true;
                }
                if (style.BorderWidth != this.BorderWidth)
                {
                    style.BorderWidth = this.BorderWidth;
                    modified = true;
                }
            }
            if (this._EnabledBackgroundColor)
            {
                if (style.BackgroundColor != this.BackgroundColor)
                {
                    style.BackgroundColor = this.BackgroundColor;
                    modified = true;
                }
            }
            return modified;
        }

    }
     
    /// <summary>
    /// 边框设置样式
    /// </summary>
    [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
    [Flags]
    public enum BorderSettingsStyle
    {
        /// <summary>
        /// 无边框
        /// </summary>
        None = 0 ,
        /// <summary>
        /// 方框
        /// </summary>
        Rectangle ,
        /// <summary>
        /// 全部
        /// </summary>
        Both,
        /// <summary>
        /// 自定义
        /// </summary>
        Custom
    }

}
