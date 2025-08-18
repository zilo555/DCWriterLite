
using System;
// // 
using System.Collections;

namespace System.Windows.Forms
{

    public class PaintEventArgs : EventArgs
    {
        //public PaintEventArgs()
        //{ }

        public PaintEventArgs(Graphics graphics, DCSystem_Drawing.Rectangle clipRect)
        {
            this._Graphics = graphics;
            this._ClipRectangle = clipRect;
        }
        private Graphics _Graphics = null;
        private DCSystem_Drawing.Rectangle _ClipRectangle = DCSystem_Drawing.Rectangle.Empty;

        public DCSystem_Drawing.Rectangle ClipRectangle
        {
            get
            {
                return this._ClipRectangle;
            }
        }
        public Graphics Graphics
        {
            get
            {
                return this._Graphics;
            }
        }
        public virtual void Dispose()
        {
            this._Graphics = null;
        }
    }
    //public delegate void PaintEventHandler(object sender, System.Windows.Forms.PaintEventArgs e);

    public static class Cursors
    {
        public static readonly System.Windows.Forms.Cursor AppStarting = new Cursor("progress");
        public static readonly System.Windows.Forms.Cursor Arrow = new Cursor("default");
        public static readonly System.Windows.Forms.Cursor Cross = new Cursor("crosshair");
        public static readonly System.Windows.Forms.Cursor Default = new Cursor("default");
        public static readonly System.Windows.Forms.Cursor Hand = new Cursor("pointer");
        public static readonly System.Windows.Forms.Cursor Help = new Cursor("help");
        public static readonly System.Windows.Forms.Cursor HSplit = new Cursor("col-resize");
        public static readonly System.Windows.Forms.Cursor IBeam = new Cursor("text");
        public static readonly System.Windows.Forms.Cursor No = new Cursor("not-allowed");
        public static readonly System.Windows.Forms.Cursor NoMove2D = new Cursor("all-scroll");
        public static readonly System.Windows.Forms.Cursor NoMoveHoriz = new Cursor("ns-resize");
        public static readonly System.Windows.Forms.Cursor NoMoveVert = new Cursor("ew-resize");
        public static readonly System.Windows.Forms.Cursor PanEast = new Cursor("e-resize");
        public static readonly System.Windows.Forms.Cursor PanNE = new Cursor("ne-resize");
        public static readonly System.Windows.Forms.Cursor PanNorth = new Cursor("n-resize");
        public static readonly System.Windows.Forms.Cursor PanNW = new Cursor("nw-resize");
        public static readonly System.Windows.Forms.Cursor PanSE = new Cursor("se-resize");
        public static readonly System.Windows.Forms.Cursor PanSouth = new Cursor("s-resize");
        public static readonly System.Windows.Forms.Cursor PanSW = new Cursor("sw-resize");
        public static readonly System.Windows.Forms.Cursor PanWest = new Cursor("w-resize");
        public static readonly System.Windows.Forms.Cursor SizeAll = new Cursor("move");
        public static readonly System.Windows.Forms.Cursor SizeNESW = new Cursor("nesw-resize");
        public static readonly System.Windows.Forms.Cursor SizeNS = new Cursor("ns-resize");
        public static readonly System.Windows.Forms.Cursor SizeNWSE = new Cursor("nwse-resize");
        public static readonly System.Windows.Forms.Cursor SizeWE = new Cursor("ew-resize");
        public static readonly System.Windows.Forms.Cursor UpArrow = new Cursor("n-resize");
        public static readonly System.Windows.Forms.Cursor VSplit = new Cursor("col-resize");
        public static readonly System.Windows.Forms.Cursor WaitCursor = new Cursor("wait");
    }
    public sealed class Cursor
    {
        private string _Name = null;
        public string Name
        {
            get
            {
                return this._Name;
            }
        }
        public Cursor(string name)
        {
            this._Name = name;
        }
#if LightWeight
        public override string ToString()
        {
            return "Cursor:" + this._Name;
        }
#endif
    }
}
