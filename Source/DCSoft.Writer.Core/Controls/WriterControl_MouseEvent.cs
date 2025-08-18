using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
// // 
using DCSoft.Writer.Dom;
using System.ComponentModel;
using System.Runtime.InteropServices;
using DCSoft.Printing;
using DCSoft.Drawing;
using DCSoft.WinForms;
using DCSoft.WinForms.Native;
using DCSoft.Common;

namespace DCSoft.Writer.Controls
{
    /// <summary>
    /// 编辑器鼠标事件处理
    /// </summary>
    public partial class WriterControl
    {
        /// <summary>
        /// 触发鼠标悬停的文档元素改变事件。
        /// </summary>
        /// <param name="oldHoverElement">旧的鼠标悬停元素</param>
        /// <param name="newHoverElement">新的鼠标悬停元素</param>

        public virtual void SetHoverElement(
            DomElement oldHoverElement,
            DomElement newHoverElement)
        {
            this.GetInnerViewControl().SetHoverElement(oldHoverElement, newHoverElement);
        }


        #region 处理OLE拖拽事件 ***********************************************

        [DefaultValue(false)]
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        ////[DCPublishAPI]
        public override bool AllowDrop
        {
            get
            {
                return this.GetInnerViewControl().AllowDrop;
            }
            set
            {
                this.GetInnerViewControl().AllowDrop = value;
            }
        }

        /// <summary>
        /// 能否直接拖拽文档内容
        /// </summary>
        [System.ComponentModel.DefaultValue(false)]
        //[Category("Behavior")]
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        ////[DCPublishAPI]
        public bool AllowDragContent
        {
            get
            {
                return this.GetInnerViewControl().AllowDragContent;
            }
            set
            {
                this.GetInnerViewControl().AllowDragContent = value;
            }
        }

        /// <summary>
        /// 编辑器控件能接受的数据格式，包括粘贴操作和OLE拖拽操作获得的数据
        /// </summary>
        //[System.ComponentModel.Editor(
        //    "DCSoft.Writer.Controls.WriterDataFormatsUIEditor",
        //    typeof(System.Drawing.Design.UITypeEditor))]
        [DefaultValue(WriterDataFormats.All)]
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        ////[DCPublishAPI]
        public WriterDataFormats AcceptDataFormats
        {
            get
            {
                return this.GetInnerViewControl().AcceptDataFormats;
            }
            set
            {
                this.GetInnerViewControl().AcceptDataFormats = value;
            }
        }

        /// <summary>
        /// 编辑器控件能创建的数据格式，这些数据将用于复制操作和OLE拖拽操作
        /// </summary>
        public WriterDataFormats CreationDataFormats
        {
            get
            {
                return this.GetInnerViewControl().CreationDataFormats;
            }
            set
            {
                this.GetInnerViewControl().CreationDataFormats = value;
            }
        }

        ///// <summary>
        ///// 执行粘贴操作
        ///// </summary>
        ///// <param name="specifyFormat">指定的数据格式</param>
        ///// <param name="showUI">是否显示用户界面</param>
        ///// <returns>操作是否成功</returns>
        //public bool InnerPaste(string specifyFormat, bool showUI)
        //{
        //    return this.GetInnerViewControl().InnerPaste(specifyFormat, showUI);
        //}

        #endregion
    }
}
