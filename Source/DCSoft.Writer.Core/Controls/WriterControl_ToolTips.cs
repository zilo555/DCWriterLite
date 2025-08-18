using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;
using DCSoft.Writer.Dom;

namespace DCSoft.Writer.Controls
{
    //编辑器中提示文本相关的功能
    public partial class WriterControl
    {
        
        /// <summary>
        /// 是否显示提示文本
        /// </summary>
        public bool ShowTooltip
        {
            get
            {
                return this.GetInnerViewControl().ShowTooltip; 
            }
            set
            {
                this.GetInnerViewControl().ShowTooltip = value;
            }
        }
         
        /// <summary>
        /// 根据元素提示文本信息列表来更新用户界面
        /// </summary>
        /// <param name="checkVersion">是否检测提示信息版本号</param>
        public void UpdateToolTip(bool checkVersion)
        {
            this.GetInnerViewControl().UpdateToolTip(checkVersion , null );
        }

    }
}
