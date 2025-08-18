using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using DCSoft.Drawing;
using DCSoft.Writer.Dom;
using System.Windows.Forms;
using DCSoft.Printing;
namespace DCSoft.Writer.Controls
{
    public partial class WriterViewControl
    {
        internal void ActiveDocumentContentElement(DomDocumentContentElement dce)
        {
            this.CurrentTransformItem = null;
            //int pi = this.CurrentPageIndex;
            PrintPage cp = this.CurrentPage;
            if (cp == null)
            {
                cp = this.Document.Pages.FirstPage;
            }
            if (cp == null)
            {
                return;
            }
            foreach (SimpleRectangleTransform item in this.PagesTransform)
            {
                DomDocument document = (DomDocument)item.DocumentObject;
                if (item.PageObject != cp)
                {
                    continue;
                }
                if (item.ContentStyle == PageContentPartyStyle.HeaderRows)
                {
                    // 标题行区域不进行任何处理
                    continue;
                }
                if (item.ContentStyle != dce.PagePartyStyle)
                {
                    continue;
                }
                if (document.CurrentContentElement != dce)
                {
                    if (dce != document.Body)
                    {
                        this.CurrentTransformItem = item;
                    }
                    DomDocumentContentElement oldContentElement = document.CurrentContentElement;
                    document.CurrentContentElement = dce;
                    document.CurrentContentElement.RaiseFocusEvent(
                        oldContentElement.CurrentElement,
                        dce.CurrentElement);
                    // 若命中的区域是不可用的则更新文档状态
                    document.CurrentContentElement.FixElements();
                    this.UpdatePages();////////////////////
                    //this.RefreshScaleTransform();
                    this.Invalidate();
                    this.UpdateTextCaret();
                }
                break;
            }//foreach
        }
    }
}
