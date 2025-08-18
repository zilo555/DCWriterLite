using System;

using DCSoft.Drawing;

using System.ComponentModel;
using DCSystemXml;
using System.Text;
using System.Runtime.InteropServices;
using DCSoft.Common;
using DCSoft.Writer.Data;

namespace DCSoft.Writer.Dom
{
    /// <summary>
    /// 文档字符串对象
    /// </summary>
    /// <remarks>本对象只是在加载或保存文档时临时生成。</remarks>
    public class DomStringElement : DomContainerElement// , DCSystemXmlSerialization.IXmlSerializable
    {
        public static readonly string TypeName_XTextStringElement = "XTextStringElement";
        public override string TypeName => TypeName_XTextStringElement;
        /// <summary>
        /// 初始化对象TextLength
        /// </summary>
        public DomStringElement():base(0)
        {
            //base.Attributes = null;
        }


        #region 无效的属性 ******************************************************


        /// 无效的属性
        /// </summary>
        public override bool Visible
        {
            get
            {
                return true;
            }
            set
            {
            }
        }
        /// <summary>
        /// 无效的属性
        /// </summary>
        public override ValueValidateStyle ValidateStyle
        {
            get
            {
                return null;
            }
            set
            {
            }
        }
        /// <summary>
        /// 无效的属性
        /// </summary>
        public override string ToolTip
        {
            get
            {
                return null ;
            }
            set
            {
            }
        }


        /// <summary>
        /// 无效的属性
        /// </summary>
        public override bool AcceptTab
        {
            get
            {
                return false;
            }
            set
            {
            }
        }

        public override ElementType RuntimeAcceptChildElementTypes()
        {
            return ElementType.Text;
        }
        
         
        /// <summary>
        /// 无效的属性
        /// </summary>
        public override DomElementList Elements
        {
            get
            {
                return base.Elements;
            }
            set
            {
                base.Elements = value;
            }
        }


#endregion
        /// <summary>
        /// 所关联的段落符号原始
        /// </summary>
        public DomParagraphFlagElement _PFlag;

        /// <summary>
        /// 对象所属段落符号元素
        /// </summary>
        public override DomParagraphFlagElement OwnerParagraphEOF
        {
            get
            {
                //throw new NotSupportedException();
                return _PFlag;
            }
        }
         
        private DomCharElement _StartElement;
        /// <summary>
        /// 开始字符元素
        /// </summary>
        public DomCharElement StartElement
        {
            get { return _StartElement; }
            //set { _StartElement = value; }
        }

        private DomCharElement _EndElement;
        /// <summary>
        /// 结束字符元素
        /// </summary>
        public DomCharElement EndElement
        {
            get { return _EndElement; }
            set { _EndElement = value; }
        }

        private bool _IsBackgroundText;
        /// <summary>
        /// 是否为背景文字模式
        /// </summary>
        public bool IsBackgroundText
        {
            get { return _IsBackgroundText; }
            set { _IsBackgroundText = value; }
        }


        private int _WhiteSpaceLength;
        /// <summary>
        /// 空格长度
        /// </summary>
        public int WhiteSpaceLength
        {
            get
            {
                return _WhiteSpaceLength;
            }
            set
            {
                _WhiteSpaceLength = value;
            }
        }

        /// <summary>
        /// 设置空格长度
        /// </summary>
        public void SetWhiteSpaceLength()
        {
            bool match = true;
            string txt = this.Text;
            foreach (char c in txt)
            {
                if (c != ' ')
                {
                    match = false;
                    break;
                }
            }
            if (match)
            {
                this._WhiteSpaceLength = txt.Length;
            }
        }


        private System.Text.StringBuilder myText
            = new System.Text.StringBuilder();

        private const string _DCSPACEFLAG = "{{DCSPACES}}";
        /// <summary>
        /// 对象文本
        /// </summary>
        public override string Text
        {
            get
            {
                if (this.WhiteSpaceLength > 0)
                {
                    return WriterUtilsInner.GetWhitespaceString(this.WhiteSpaceLength);
                }
                else
                {
                    return myText.ToString();
                }
            }
            set
            {
                string txt = value;
                if ( txt != null && txt.Length > 0 )// string.IsNullOrEmpty(txt) == false)
                {
                    if (txt.StartsWith(_DCSPACEFLAG,StringComparison.Ordinal))
                    {
                        txt = txt.Substring(_DCSPACEFLAG.Length);
                        int len = 0;
                        if (int.TryParse(txt, out len))
                        {
                            txt = new string(' ', len);
                        }
                        else
                        {
                            txt = string.Empty;
                        }
                    }
                }
                this.myText = new System.Text.StringBuilder(txt);
                if (DomElementList.InnerDeserializing == false)
                {
                    this._Elements?.ClearAndEmpty();
                }
            }
        }
        

        /// <summary>
        /// 对象是否有内容
        /// </summary>
        public bool HasContent
        {
            get
            {
                return myText.Length > 0 || this.Elements.Count > 0;
            }
        }

        /// <summary>
        /// 获得输出文本
        /// </summary>
        /// <param name="includeSelectionOnly">只包含被选择的部分</param>
        /// <returns>获得的文本</returns>
        public string GetOutputText(bool includeSelectionOnly)
        {
            //if (this.Elements.Count == 0)
            //{
            //    return string.Empty;
            //}
            string txt = string.Empty;
            if (includeSelectionOnly == false || this.Elements.Count == 0)
            {
                txt = this.Text;
            }
            else
            {
                System.Text.StringBuilder myStr = new System.Text.StringBuilder();
                DomDocumentContentElement dce = this.Elements[0].DocumentContentElement;
                foreach (DomCharElement c in this.Elements)
                {
                    if (dce.IsSelected(c))
                    {
                        c.AppendContent(myStr);//  myStr.Append(c.GetCharValue());
                    }
                }//foreach
                txt = myStr.ToString();
            }
            return txt;
        }

        /// <summary>
        /// 判断对象能否合并一个字符元素对象
        /// </summary>
        /// <param name="c">字符元素对象</param>
        /// <returns>能否合并</returns>
        public bool CanMerge(DomCharElement c)
        {
            if (c != null)
            {
                if (myText.Length == 0)
                {
                    return true;
                }
                //if (this.MergeForPrintHtml)
                //{
                //    // 为输出打印用的HTML代码而进行合并
                //    bool v1 = this.StartElement.GetCharValue() == ' ';
                //    bool v2 = c.GetCharValue() == ' ';
                //    if (v1 != v2)
                //    {
                //        // 空格状态发生改变
                //        return false;
                //    }
                //    //if (this.StartElement.CharValue == ' ')
                //    //{
                //    //    // 开始字符是空格，而当前字符不是空格，则不合并
                //    //    if( c.CharValue != ' ')
                //    //    {
                //    //        return false ;
                //    //    }
                //    //}
                //}
                return this._StyleIndex == c._StyleIndex;
            }
            return false;
        }

        /// <summary>
        /// 合并一个字符元素对象
        /// </summary>
        /// <param name="c">字符元素对象</param>
        /// <param name="txt">文本</param>
        public void Merge(DomCharElement c, string txt)
        {
            if (myText.Length == 0)
            {
                this.StyleIndex = c.StyleIndex;
                SetStartElement(c);
            }
            //if (this._StartElement == null)
            //{
            //    this._StartElement = c;
            //}
            this._EndElement = c;
            if (txt != null)
            {
                myText.Append(txt);
            }
            //myText.ToString().IndexOf("未见溃疡。") >= 0 
            //this.Elements.AddRaw(c);
        }
        internal void SetStartElement(DomCharElement c )
        {
            this._StartElement = c;
            this._OwnerDocument = c.OwnerDocument;
            this._Parent = c.Parent;
        }
#if !RELEASE
        /// <summary>
        /// 返回表示对象的字符串
        /// </summary>
        /// <returns>字符串</returns>
        public override string ToString()
        {
            if (this.myText.Length > 20)
            {
                return myText.ToString(0, 20);
            }
            else
            {
                return myText.ToString();
            }
        }
#endif
    }//public class XTextString : XTextElement
}