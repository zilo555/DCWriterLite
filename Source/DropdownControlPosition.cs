using System;
using DCSoft.Writer.Dom;

namespace DCSoft.WASM
{

    //[System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
    public class DropdownControlPosition
    {
        public DropdownControlPosition(DomElement element)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }
            element = DCSoft.Writer.Controls.TextWindowsFormsEditorHostClass.GetAnchorElement(element);
            if (element == null)
            {
                return;
            }
            var info = element.InnerViewControl.GetClientPosInfo(element);
            if (info == null)
            {
                return;
            }
            this.PageIndex = info.PageIndex;
            this.Left = info.dx;
            if( element is DomFieldBorderElement )
            {
                this.Left += (int)(element.PixelWidth * element.InnerViewControl.WASMZoomRate);
            }
            this.Top = info.dy;
            this.Height = (int)(element.PixelHeight * element.InnerViewControl.WASMZoomRate);
        }
        public readonly int PageIndex = 0;
        public readonly int Left = 0;
        public readonly int Top = 0;
        public readonly int Height = 0;
    }
}
