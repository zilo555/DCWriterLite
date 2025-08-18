using System;
using System.Collections.Generic;
using System.Text;

namespace DCSoft.Writer.Dom
{
    /// <summary>
    /// 获得文本元素参数
    /// </summary>
    //[System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = false )]
    public class GetTextArgs
    {
        public GetTextArgs()
        {

        }


        private bool _IncludeHiddenText = false;
        /// <summary>
        /// 是否包含隐藏的内容，默认false.
        /// </summary>
        //[System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public bool IncludeHiddenText
        {
            get
            {
                return this._IncludeHiddenText;
            }
            set
            {
                this._IncludeHiddenText = value;
            }
        }

        private bool _IncludeBackgroundText = false;
        /// <summary>
        /// 是否包含输入域背景文本，默认false。
        /// </summary>
        //[System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public bool IncludeBackgroundText
        {
            get
            {
                return this._IncludeBackgroundText;
            }
            set
            {
                this._IncludeBackgroundText = value;
            }
        }
       
        internal void Init(DomDocument document)
        {
            this._Buffer = new StringBuilder();
        }
        private StringBuilder _Buffer = new StringBuilder();
        internal int Length
        {
            get
            {
                return this._Buffer.Length;
            }
        }
        //public void SetLength(int len )
        //{
        //    if(len < this._Buffer.Length )
        //    {
        //        this._Buffer.Remove(len, this._Buffer.Length - len);
        //    }
        //}
        internal void Append( string text )
        {
            if(text != null && text.Length > 0 )
            {
                this._Buffer.Append(text);
            }
        }
        internal void Append(char c)
        {
            this._Buffer.Append(c);
        }
        internal void EnsureNewLine()
        {
            if( this._Buffer.Length > 0 )
            {
                var c = this._Buffer[this._Buffer.Length - 1];
                if( c != '\r' && c != '\n')
                {
                    this._Buffer.AppendLine();
                }
            }
        }
        internal void AppendLine()
        {
            this._Buffer.AppendLine();
        }
        public override string ToString()
        {
            if(this._Buffer == null )
            {
                return null;
            }
            else
            {
                return this._Buffer.ToString();
            }
        }
    }
}
