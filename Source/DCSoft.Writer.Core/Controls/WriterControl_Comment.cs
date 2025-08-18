using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using DCSoft.Drawing;
using DCSoft.Writer.Dom;
// // 
using System.Windows.Forms;
using DCSoft.Printing;
using DCSoft.Common;
namespace DCSoft.Writer.Controls
{
    public partial class WriterControl
    {

        internal void ActiveDocumentContentElement(DomDocumentContentElement dce)
        {
            this.myViewControl?.ActiveDocumentContentElement(dce);
        }

    }
}
