using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel ;
using DCSoft.Drawing ;
using DCSoft.Common;
// // 
using DCSoft.WinForms;

namespace DCSoft.Writer.Dom
{
    /// <summary>
    /// 页码文档元素对象
    /// </summary>
    public sealed partial class DomPageInfoElement : DomObjectElement
    {
        public static readonly string TypeName_XTextPageInfoElement = "XTextPageInfoElement";
        public override string TypeName => TypeName_XTextPageInfoElement;
        /// <summary>
        /// 初始化对象
        /// </summary>
        public DomPageInfoElement()
        {
            this.Width = 100;
            this.Height = 100;
        }

        /// <summary>
        /// 文档元素编号前缀
        /// </summary>
        public override string ElementIDPrefix()
        {
            return "pi";
        }

        /// <summary>
        /// 复制对象
        /// </summary>
        /// <param name="Deeply">是否深度复制</param>
        /// <returns>复制品</returns>
        public override DomElement Clone(bool Deeply)
        {
            DomPageInfoElement e = (DomPageInfoElement)base.Clone(Deeply);
            return e;
        }

        public override void RefreshSize(InnerDocumentPaintEventArgs args)
        {
            if (this.RuntimeAutoHeight())
            {
                RuntimeDocumentContentStyle rs = this.RuntimeStyle;
                if (rs != null)
                {
                    this.Height = args.GraphicsGetFontHeight(rs) * 1.1f;
                }
            }
            this.SizeInvalid = false;
        }

        private static StringFormat _PageIndexFormat = null;

        public override void DrawContent(InnerDocumentPaintEventArgs args)
        {
            if (_PageIndexFormat == null)
            {
                var sf = new StringFormat();
                sf.Alignment = StringAlignment.Center;
                sf.LineAlignment = StringAlignment.Center;
                sf.FormatFlags = StringFormatFlags.NoWrap;
                _PageIndexFormat = sf;
            }
            RuntimeDocumentContentStyle rs = this.RuntimeStyle;
            //XTextDocument document = this.OwnerDocument;
            RectangleF rect = this.GetAbsBounds();
            rect.Y = rect.Y;// +XTextLine.StdContentTopFix;
            var args2 = (InnerDocumentPaintEventArgs)args;
            string txt = this.ContentText;
            {
                args2.Graphics.DrawString(
                    txt,
                    rs.Font,
                    rs.Color,
                    rect,
                    _PageIndexFormat);
            }
        }


        private bool _AutoHeight = false;
        /// <summary>
        /// 自动高度
        /// </summary>
        public bool AutoHeight
        {
            get
            {
                return _AutoHeight;
            }
            set
            {
                _AutoHeight = value;
                this.InvalidateDocumentLayoutFast();
            }
        }

        public bool RuntimeAutoHeight()
        {
            return this._AutoHeight;
        }

        /// <summary>
        /// 对象高度
        /// </summary>
        public override float Height
        {
            get
            {
                return base.Height;
            }
            set
            {
                base.Height = value;
            }
        }

        /// <summary>
        /// 用户能否改变对象大小
        /// </summary>
        public override ResizeableType Resizeable
        {
            get
            {
                if (this.RuntimeAutoHeight())
                {
                    return ResizeableType.Width;
                }
                else
                {
                    return ResizeableType.WidthAndHeight;
                }
            }
            set
            {
                //base.Resizeable = value;
            }
        }

        private PageInfoValueType _ValueType = PageInfoValueType.PageIndex;
        /// <summary>
        /// 内容样式
        /// </summary>
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public PageInfoValueType ValueType
        {
            get
            {
                return _ValueType;
            }
            set
            {
                _ValueType = value;
                this.InvalidateDocumentLayoutFast();
            }
        }

        public override void InnerGetText(GetTextArgs args)
        {
            args.Append(this.ContentText);
        }

        /// <summary>
        /// 内容文本
        /// </summary>
        public string ContentText
        {
            get
            {
                var document = this.OwnerDocument;
                switch (this.ValueType)
                {
                    case PageInfoValueType.NumOfPages:
                        // 全局总页数
                        if (document == null)
                        {
                            return "0";
                        }
                        else if (document.GlobalPages != null
                            && document.GlobalPages.Count > 0)
                        {
                            return document.GlobalPages.Count.ToString();
                        }
                        else if (document.Info != null)
                        {
                            return document.Info.NumOfPage.ToString();
                        }
                        else
                        {
                            return "0";
                        }
                    case PageInfoValueType.LocalNumOfPages:
                        // 文档总页数
                        if (document == null)
                        {
                            return "0";
                        }
                        else if (document.Pages != null
                            && document.Pages.Count > 0)
                        {
                            return document.Pages.Count.ToString();
                        }
                        else if (document.Info != null)
                        {
                            return document.Info.NumOfPage.ToString();
                        }
                        else
                        {
                            return "0";
                        }
                    case PageInfoValueType.PageIndex:
                        {
                            // 全局页码
                            return (document.GlobalPageIndex + 1).ToString();
                        }
                    case PageInfoValueType.LocalPageIndex:
                        {
                            // 文档页码
                            return (document.PageIndex + 1).ToString();
                        }
                }
                return "0";
            }
        }

        /// <summary>
        /// 返回纯文本数据
        /// </summary>
        /// <returns>文本数据</returns>
        public override string ToPlaintString()
        {
            if (this.OwnerDocument != null)
            {
                return this.ContentText;// this.OwnerDocument.PageIndex.ToString();
            }
            else
            {
                return string.Empty;
            }
        }
#if !RELEASE
        /// <summary>
        /// 获得调试信息字符串
        /// </summary>
        /// <returns>字符串</returns>
        public override string ToDebugString()
        {
            return "PageIndex:" + this.ToPlaintString();
        }
#endif
    }

    /// <summary>
    /// 页码信息类型
    /// </summary>
    [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
    [System.Text.Json.Serialization.JsonConverter(typeof(System.Text.Json.Serialization.JsonStringEnumConverter))]
    public enum PageInfoValueType
    {
        /// <summary>
        /// 从1开始计算的页码
        /// </summary>
        PageIndex = 0,
        /// <summary>
        /// 文档总页数
        /// </summary>
        NumOfPages,
        /// <summary>
        /// 在本文档中的从1开始计算的页码数
        /// </summary>
        /// <remarks>
        /// 当在一个打印预览控件中显示多个文档，或者批量打印文档时，该页码数就是该页
        /// 在文档中的页码数，而不是所有文档的文档页集合在一起时的页码数。
        /// </remarks>
        LocalPageIndex,
        /// <summary>
        /// 在本文档中的文档总页数
        /// </summary>
        LocalNumOfPages
    }
}