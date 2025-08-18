using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;
using DCSoft.Writer.Dom;

namespace DCSoft.Writer.Controls
{
    //编辑器中提示文本相关的功能
    public partial class WriterViewControl
    {
        /// <summary>
        /// 是否显示提示文本
        /// </summary>
        public bool ShowTooltip
        {
            get
            {
                return this.DocumentOptions.BehaviorOptions.ShowTooltip; 
            }
            set
            {
                this.DocumentOptions.BehaviorOptions.ShowTooltip = value;
                if (value == false)
                {
                    {
                        this.TooltipControl.SetToolTip(this, null);
                    }
                }
            }
        }

        private System.Windows.Forms.ToolTip _TooltipControl = null;
        /// <summary>
        /// 内置的提示信息控件
        /// </summary>
        public System.Windows.Forms.ToolTip TooltipControl
        {
            get
            {
                if (this._TooltipControl == null && this.IsDisposed == false)
                {
                    this._TooltipControl = new ToolTip();
                }
                return _TooltipControl;
            }
        }
        private ElementToolTipContainer _ToolTips = new ElementToolTipContainer();
        /// <summary>
        /// 元素提示文本信息列表
        /// </summary>
        internal ElementToolTipContainer ToolTips
        {
            get
            {
                return _ToolTips;
            }
        }
         
        private int _ToolTipsVersion = 0;
        /// <summary>
        /// 根据元素提示文本信息列表来更新用户界面
        /// </summary>
        /// <param name="checkVersion">是否检测提示信息版本号</param>
        /// <param name="args">相关的文档事件参数</param>
        public void UpdateToolTip(bool checkVersion , DocumentEventArgs args )
        {
            if (this.ShowTooltip == false)
            {
                return;
            }
            //if (checkVersion)
            //{
            //    if ( this.ToolTips.Version == _ToolTipsVersion)
            //    {
            //        return;
            //    }
            //}
            _ToolTipsVersion ++;//= this.InternalToolTips.Version + this.ToolTips.Version;
            //this.InternalToolTips.Version = _ToolTipsVersion;
            this.ToolTips.Version = _ToolTipsVersion;

            // 显示提示文本
            ElementToolTip tip = null;
            DomElement element = this.HoverElement();
            // 搜索公开的文档元素提示信息
            while (element != null)
            {
                tip = this.ToolTips[element];
                if (tip != null)
                {
                    break;
                }
                element = element.Parent;
            }
            if (tip == null)
            {
                // 搜索内部的文档元素提示信息
                element = this.HoverElement();
                while (element != null)
                {
                    tip = element.GetToolTipInfo();// this.InternalToolTips[element];
                    if (tip != null)
                    {
                        break;
                    }
                    element = element.Parent;
                }
            }
            if(args != null && string.IsNullOrEmpty( args.Tooltip ) == false )
            {
                tip = new ElementToolTip(element, args.Tooltip);
            }
            // 最后一次显示的提示信息
            var tipControl = this.TooltipControl;
            ElementToolTip lastTip = tipControl.Tag as ElementToolTip;
            if (tip == null)
            {
                tipControl.SetToolTip(this, null);
                tipControl.Tag = null;
                return;
            }
            if (tip.EqualsValue(lastTip))
            {
                if (checkVersion)
                {
                    return;
                }
            }
            else
            {
                tipControl.RemoveAll();
                tipControl.Tag = null;
            }
            if (tip.Style == ToolTipStyle.ToolTip)
            {
                if (tip.DisplayOnce)
                {
                    //this.InternalToolTips.Remove(tip.Element);
                    //this.ToolTips.IncreateVersion();
                }
                if (this.EditorHost != null && this.EditorHost.ShowingDropDown)
                {
                    tipControl.Tag = null;
                    tipControl.SetToolTip(this, null);
                }
                else
                {
                    tipControl.Tag = tip;
                    {
                        tipControl.SetToolTip(this, tip.Text);
                    }
                }
            }
            //this.InternalToolTips.Version = _ToolTipsVersion;
            this.ToolTips.Version = _ToolTipsVersion;
        }

        internal void HideToolTip()
        {
            //this._TooltipControl.SetToolTip(this, null);
            if (this.TooltipControl != null)
            {
                this.TooltipControl.Tag = null;
                this.TooltipControl.SetToolTip(this, null);
            }
        }
    }
}
