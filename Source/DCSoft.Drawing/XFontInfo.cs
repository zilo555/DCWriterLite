using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace DCSoft.Drawing
{
    /// <summary>
    /// 表示字体定义信息的对象
    /// </summary>
    public class XFontInfo
    {
        public XFontInfo(string fName, float si, FontStyle st, GraphicsUnit u)
        {
            this.Name = fName;
            this.Size = si;
            this.Style = st;
            this.Unit = u;

            this._HashCode = this.Name.GetHashCode();
            this._HashCode += this.Size.GetHashCode();
            this._HashCode += (int)this.Style;
            this._HashCode += 10 * (int)this.Unit;

        }

        public readonly string Name;
        public readonly float Size;
        public readonly FontStyle Style;
        public readonly GraphicsUnit Unit;
        private int _HashCode = 0 ;

        public override int GetHashCode()
        {
            //if( _HashCode == 0 )
            //{
                
            //}
            return this._HashCode;
        }

        public override bool Equals(object obj)
        {
            if (obj == this)
            {
                return true;
            }
            var info = (XFontInfo)obj;
            return this.Name == info.Name
                && this.Size == info.Size
                && this.Style == info.Style
                && this.Unit == info.Unit;
        }
    }
}
