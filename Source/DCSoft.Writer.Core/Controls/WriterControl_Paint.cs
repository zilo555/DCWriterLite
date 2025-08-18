using System;
using System.Collections.Generic;
using System.Text;
// // 
using DCSoft.Drawing;
using DCSoft.WinForms;
using DCSoft.Printing;
using DCSoft.Writer.Dom;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using DCSoft.Common;
using DCSoft.WinForms.Native;

namespace DCSoft.Writer.Controls
{
    /// <summary>
    /// 编辑器中绘制用户界面的操作
    /// </summary>
    public partial class WriterControl
    {
            
        /// <summary>
        /// 添加被选择区域矩形
        /// </summary>
        /// <param name="rect">矩形</param>
        public void AddSelectionAreaRectangle(Rectangle rect)
        {
            this.GetInnerViewControl().AddSelectionAreaRectangle(rect);
        }

        /// <summary>
        /// 声明指定文档行区域无效，需要重新绘制。
        /// </summary>
        /// <param name="line"></param>
        /// <param name="party"></param>
        public void ViewInvalidate(DomLine line, PageContentPartyStyle party)
        {
            this.myViewControl?.ViewInvalidate(line, party);
        }

        /// <summary>
        /// 声明指定区域无效，需要重新绘制。无效矩形采用视图坐标。
        /// </summary>
        /// <param name="rect">无效矩形</param>
        /// <param name="party">文档区域</param>
        public void ViewInvalidate(RectangleF rect, PageContentPartyStyle party)
        {
            this.myViewControl?.ViewInvalidate(rect, party);
        }
    }
}
