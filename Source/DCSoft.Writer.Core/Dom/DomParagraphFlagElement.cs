using System;
using System.Collections.Generic;
// // 
using DCSoft.Drawing;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Text;

namespace DCSoft.Writer.Dom
{

    /// <summary>
    /// 段落结束标记对象,XWriterLib内部使用
    /// </summary>
    public sealed partial class DomParagraphFlagElement : DomEOFElement
    {
       

        /// <summary>
        /// 初始化对象
        /// </summary>
        public DomParagraphFlagElement()
        {
        }



        private int _TopicID = -1;
        /// <summary>
        /// 主题编号,DCWriter内部使用。
        /// </summary>
        public int TopicID
        {
            get
            {
                return _TopicID; 
            }
            set
            {
                _TopicID = value; 
            }
        }

        [NonSerialized]
        internal DomParagraphListItemElement _ListItemElement;
        /// <summary>
        /// DCWriter内部使用。段落列表元素对象
        /// </summary>
        public DomParagraphListItemElement ListItemElement
        {
            get
            {
                return _ListItemElement; 
            }
            set
            {
                _ListItemElement = value; 
            }
        }

        /// <summary>
        /// 是否为根元素
        /// </summary>
        internal bool IsRootLevelElement
        {
            get { return this.StateFlag06; }
            set { this.StateFlag06 = value; }
        }
         

        /// <summary>
        /// 在公共内容中的第一个元素就是自己
        /// </summary>
        public override DomElement FirstContentElementInPublicContent
        {
            get
            {
                return this;
            }
        }

        /// <summary>
        /// 设置对象获得焦点
        /// </summary>
        public override void Focus()
        {
            DomDocumentContentElement dce = this.DocumentContentElement;
            dce.Focus();
            int index = dce.Content.IndexOf(this);
            if (index >= 0)
            {
                dce.SetSelection(index, 0);
            }
        }

        public override DomParagraphFlagElement OwnerParagraphEOF
        {
            get
            {
                return this;
            }
        }

        /// <summary>
        /// 对象所属页码
        /// </summary>
       //////[Browsable(false)]
        public override int OwnerPageIndex
        {
            get
            {

                DomLine line = this.OwnerLine;
                if (line != null && line.OwnerPage != null)
                {
                    if (this.OwnerDocument == null)
                    {
                        return line.OwnerPage.PageIndex;
                    }
                    else
                    {
                        return this.OwnerDocument.Pages.IndexOf(line.OwnerPage);
                    }
                }
                return -1;
            }
        }

        /// <summary>
        /// 判断是否是文档容器中最后一个段落元素 
        /// </summary>
        public bool IsLastElementInContentElement()
        {
            DomContentElement ce = this.ContentElement;
            if (ce != null && ce.PrivateContent.LastElement == this)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 视图宽度
        /// </summary>
        public override float ViewWidth
        {
            get
            {
                return DomDocument._ParagraphFlagViewWidth;// _ViewWidth;
            }
        }

        

        /// <summary>
        /// 段落标记元素不能设置可见性
        /// </summary>
        /// <param name="visible"></param>
        /// <param name="logUndo"></param>
        /// <param name="fastMode"></param>
        /// <returns></returns>
        public override bool EditorSetVisibleExt(bool visible, bool logUndo, bool fastMode)
        {
            return false;
        }
        private DomElement _ParagraphFirstContentElement;
        /// <summary>
        /// 段落中第一个文档内容元素
        /// </summary>
        public DomElement ParagraphFirstContentElement
        {
            get
            {
                return _ParagraphFirstContentElement; 
            }
            set
            {
                _ParagraphFirstContentElement = value; 
            }
        }

        /// <summary>
        /// 段落列表样式
        /// </summary>
        internal ParagraphListStyle ListStyle()
        {
            
                RuntimeDocumentContentStyle style = this.RuntimeStyle;
                if (style == null)
                {
                    return ParagraphListStyle.None;
                }
                else
                {
                    return style.ParagraphListStyle;
                }
             
        }


        /// <summary>
        /// 段落在段落列表中的序号
        /// </summary>
        [NonSerialized]
        private int intListIndex;
        /// <summary>
        /// 段落在段落列表中的序号
        /// </summary>
        public int ListIndex
        {
            get
            {
                return intListIndex;
            }
            set
            {
                if (intListIndex != value)
                {
                    intListIndex = value;
                    this.AutoCreate = false;
                }
            }
        }
        private int _MaxListIndex;
        /// <summary>
        /// 列表的最大值
        /// </summary>
         
        public int MaxListIndex
        {
            get
            {
                return this._MaxListIndex; 
            }
            set
            {
                //if( value > 0 )
                //{
                //    var s = 1;
                //}
                this._MaxListIndex = value; 
            }
        }


        /// <summary>
        /// 段落大纲层次
        /// </summary>
        public int OutlineLevel
        {
            get
            {
                RuntimeDocumentContentStyle style = this.RuntimeStyle;
                if (style == null)
                {
                    return -1;
                }
                else
                {
                    return style.ParagraphOutlineLevel;
                }
            }
        }
        public const bool SupportRTFParagraphList = true;
        private int _RTFListOverriedID = -1;
         
        public int InnerRTFListOverriedID
        {
            get
            {
                return _RTFListOverriedID; 
            }
            set
            {
                _RTFListOverriedID = value; 
            }
        }

        [NonSerialized]
        private int _RTFOutlineLevel = -1;         
        public int InnerRTFOutlineLevel
        {
            get
            {
                return _RTFOutlineLevel;
            }
            set
            {
                _RTFOutlineLevel = value;
            }
        }
        /// <summary>
        /// 重新设置列表序号标记
        /// </summary>
        public bool ResetListIndexFlag
        {
            get
            {
                return this.StateFlag07;// _ResetListIndexFlag; 
            }
            set
            {
                this.StateFlag07 = value;// _ResetListIndexFlag = value; 
            }
        }

        /// <summary>
        /// 本对象是否是自动产生的
        /// </summary>
         
        public bool AutoCreate
        {
            get
            {
                if( this.StateFlag05 && this.ResetListIndexFlag == false )
                {
                    if( this.StyleIndex < 0 )
                    {
                        return true ;
                    }
                    if( this.OutlineLevel < 0 )
                    {
                        return true;
                    }
                }
                return false;
                //return this._AutoCreate
                //    && this.StyleIndex < 0
                //    && this._ResetListIndexFlag == false
                //    && this.OutlineLevel < 0;
            }
            set
            {
                this.StateFlag05 = value;// this._AutoCreate = value; 
            }
        }

        /// <summary>
        /// 上级段落元素对象
        /// </summary>
        internal DomParagraphFlagElement ParentParagraph;
        
        [NonSerialized]
        private List<DomParagraphFlagElement> _ChildParagraphs;
        /// <summary>
        /// DCWriter内部使用。子段落元素列表
        /// </summary>
        public List<DomParagraphFlagElement> ChildParagraphs()
        {
            if (_ChildParagraphs == null)
            {
                _ChildParagraphs = new List<DomParagraphFlagElement>();
            }
            return _ChildParagraphs;
        }
        /// <summary>
        /// 是否存在子段落元素
        /// </summary>
        /// <returns></returns>
        public bool HasChildParagraphs()
        {
            return this._ChildParagraphs != null && this._ChildParagraphs.Count > 0;
        }

        internal void AppendChildParagraph(DomParagraphFlagElement element)
        {
            this.ChildParagraphs().Add(element);
            element.ParentParagraph = this;
        }

        /// <summary>
        /// 所在段落的第一个内容元素对象
        /// </summary>
        public override DomElement FirstContentElement
        {
            get
            {
                if (this._ParagraphFirstContentElement == null)
                {
                    return this;
                }
                else
                {
                    return this._ParagraphFirstContentElement;
                }
                //if (this.OwnerDocument == null)
                //{
                //    return null;
                //}
                //else
                //{
                //    XTextContentElement ce = this.ContentElement;
                //    XTextElementList content = ce.PrivateContent;
                //    int index = content.IndexOf(this);
                //    if (index >= 0)
                //    {
                //        for (int iCount = index - 1; iCount >= 0; iCount--)
                //        {
                //            if (content[iCount] is XTextParagraphFlagElement)
                //            {
                //                return content[iCount + 1];
                //            }
                //        }
                //    }
                //    return content[0];
                //}
            }
        }

        
        /// <summary>
        /// 复制对象
        /// </summary>
        /// <param name="Deeply">是否深入复制子元素</param>
        /// <returns>复制品</returns>
        public override DomElement Clone(bool Deeply)
        {
            DomParagraphFlagElement pe = (DomParagraphFlagElement)base.Clone( Deeply );
#if ! LightWeight && ! NO_RTFParagraphList
            pe._RTFListOverriedID = -1;
            pe._RTFOutlineLevel = -1;
#endif
            //this.CopyAttributesTo(pe);
            return pe;
        }

        /// <summary>
        /// 绘制段落符号
        /// </summary>
        /// <param name="args">参数</param>
        public override void Draw(InnerDocumentPaintEventArgs args)
        {
            if (args.RenderMode != InnerDocumentRenderMode.Paint)
            {
                return;
            }
            args.Render.DrawBackground(this, args, args.ViewBounds);

            Bitmap bmp = null;
            var rect = args.ViewBounds;
            bmp = args.BmpParagraphFlagLeftToRight;
            DomDocument.IsDrawingSystemImage = true;
            args.Graphics.DrawImage(
                bmp,
                rect.Left,
                rect.Bottom - args.SizeOfPaintBmpParagraphFlag.Height);
            DomDocument.IsDrawingSystemImage = false;
        }

        /// <summary>
        /// 计算段落符号元素大小
        /// </summary>
        /// <param name="args">参数</param>
        public override void RefreshSize(InnerDocumentPaintEventArgs args)
        {
            DomDocument doc = this._OwnerDocument;
            if (doc != null)
            {
                this.Width = doc._ParagraphFlagSize.Width;
                float h = doc._ParagraphFlagSize.Height;
                    h = this.RuntimeStyle.FontHeight;
                    if( this.RuntimeStyle != this.RuntimeStyle.Parent?._MyRuntimeStyle)
                    {

                    }
                    if(h == 0 )
                    {
                        h = doc._ParagraphFlagSize.Height;
                    }
                this.Height = h;
                //this._ViewWidth = doc._ParagraphFlagViewWidth;
            }
        }

        /// <summary>
        /// 高度
        /// </summary>
        public override float Height
        {
            get
            {
                return base._Height;
            }
            set
            {
                //if (base._Height != value)
                {
                    base._Height = value;
                }
            }
        }

#if !RELEASE
        /// <summary>
        /// 返回对象包含的字符串数据
        /// </summary>
        /// <returns>字符串数据</returns>
        public override string ToString()
        {
            if (DomElement.DebugModeToString)
            {
                return Environment.NewLine;
            }
            else
            {
                return "[P]";
            }
        }
        public override string ToDebugString()
        {
            return "[P]";
        }
#endif
        /// <summary>
        /// 返回纯文本
        /// </summary>
        /// <returns>文本内容</returns>
        public override string ToPlaintString()
        {
            return Environment.NewLine;
        }
        public override void InnerGetText(GetTextArgs args)
        {
            args.AppendLine();
        }
        public override void WriteText(WriteTextArgs args)
        {
            args.AppendLine();
        }
        /// <summary>
        /// 表示对象内容的纯文本数据
        /// </summary>
        public override string Text
        {
            get
            {
                return Environment.NewLine;
            }
            set
            {
            }
        }
        /// <summary>
        /// 整个段落的文字
        /// </summary>
        public string ParagraphText
        {
            get
            {
                DomElementList content = this.DocumentContentElement?.Content;
                if (content != null)
                {
                    int index = content.FastIndexOf(this);
                    if (index > 0)
                    {
                        System.Text.StringBuilder str = new System.Text.StringBuilder();
                        for (int iCount = index - 1; iCount >= 0; iCount--)
                        {
                            DomElement e = content[iCount];
                            if (e is DomParagraphFlagElement)
                            {
                                break;
                            }
                        }
                        if (this._ListItemElement != null)
                        {
                            str.Insert(0, this._ListItemElement.Text);
                        }
                        return str.ToString();
                    }
                }
                return string.Empty;
            }
        }

        private bool _Visible = true;
        /// <summary>
        /// 元素是否可见。本属性不参与XML序列化。
        /// </summary>
        public override bool Visible
        {
            get
            {
                return this._Visible;
            }
            set
            {
                this._Visible = value;
            }
        }

        /// <summary>
        /// 销毁对象
        /// </summary>
        public override void Dispose()
        {
            base.Dispose();
            if (this._ChildParagraphs != null)
            {
                this._ChildParagraphs.Clear();
                this._ChildParagraphs = null;
            }
            this._ListItemElement = null;
            this._ParagraphFirstContentElement = null;
            this.ParentParagraph = null;
        }
    }//public class XTextParagraphEOF : XTextEOF
}
