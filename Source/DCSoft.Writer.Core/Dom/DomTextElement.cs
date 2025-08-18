using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
namespace DCSoft.Writer.Dom
{
    /// <summary>
    /// 纯文本文档元素
    /// </summary>
    public sealed partial class DomTextElement : DomElement
    {
        /// <summary>
        /// 初始化对象
        /// </summary>
        public DomTextElement()
        {

        }

        /// <summary>
        /// 初始化对象
        /// </summary>
        /// <param name="txt">文本内容</param>
        public DomTextElement(string txt )
        {
            this._TextValue = txt;
        }

        public override string ToString()
        {
            return this._TextValue;
        }

        public override void WriteText( WriteTextArgs args )
        {
            args.Append(this._TextValue);
        }
        public override string Text
        {
            get
            {
                return this._TextValue;
            }
            set
            {
                this._TextValue = value;
            }
        }

        private string _TextValue;
        /// <summary>
        /// 文本值
        /// </summary>
        public string TextValue
        {
            get
            {
                return this._TextValue;
            }
            set
            {
                this._TextValue = value;
                if (value != null && value.Length > 0)
                {
                    int index = value.IndexOf(DomCharElement.CHAR_ENSP);
                    if (index >= 0)
                    {
                        var str = new StringBuilder(value.Length);
                        if (index > 0)
                        {
                            str.Append(value, 0, index);
                        }
                        str.Append(' ');
                        int len = value.Length;
                        for (int iCount = index + 1; iCount < len; iCount++)
                        {
                            var c = value[iCount];
                            if (c == DomCharElement.CHAR_ENSP)
                            {
                                str.Append(' ');
                            }
                            else
                            {
                                str.Append(c);
                            }
                        }
                        this._TextValue = str.ToString();
                    }
                }
            }
        }
    }
}
