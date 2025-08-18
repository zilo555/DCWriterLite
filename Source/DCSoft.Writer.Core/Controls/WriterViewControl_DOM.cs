using System;
using DCSoft.WinForms;
using DCSoft.Printing;
using DCSoft.Writer.Dom;
using DCSoft.Writer.Commands;
using DCSoft.Writer.Dom.Undo;
using DCSoft.Drawing;
using System.Windows.Forms;
// // 
using DCSoft.WinForms.Native;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Collections;
using System.Collections.Generic;
using DCSoft.Common;
using DCSoft.Writer.Undo;
using System.Text;
//using DCSoft.Writer.Printing;
using DCSoft.Writer.Data;
//using DCSoft.Writer.Script;
using System.Runtime.InteropServices;

namespace DCSoft.Writer.Controls
{
    /// <summary>
    /// 编辑器控件的文档对象模型处理模块
    /// </summary>
    public partial class WriterViewControl
    {
        internal bool AutoDisposeDocument = true;

        /// <summary>
        /// 文档内容改变标记
        /// </summary>
        public bool Modified
        {
            get
            {
                return this.Document.Modified;
            }
            set
            {
                //if (this.Document.Modified != value)
                {
                    this.Document.Modified = value;
                }
            }
        }

        internal void DisposeDocument()
        {
            if (_Document != null)
            {
                _Document.Dispose();
                _Document = null;
            }
        }


        /// <summary>
        /// 文档对象
        /// </summary>
        private DomDocument _Document = null;
        /// <summary>
        /// 文档对象
        /// </summary>
        public DomDocument Document
        {
            get
            {
                if (this.IsDisposed)
                {
                    return null;
                }
                if (this._DisposingControl)
                {
                    return this._Document;
                }
                if (this._Document == null)
                {
                    this._Document = DomDocument.CreateWithBaseDOM();
                    this._Document.BeginLoadDocument();
                    try
                    {
                        this._Document.Options = this.DocumentOptions;
                        this._Document.EditorControl = this.OwnerWriterControl;
                        this._Document.DocumentControler = this.DocumentControler;
                        if (this.AutoSetDocumentDefaultFont)
                        {
                            UpdateDefaultFont(false);
                        }
                    }
                    finally
                    {
                        this._Document.EndLoadDocument();
                    }
                }
                else
                {
                    if (this._Document.States.Printing == false && this._Document.States.PrintPreviewing == false)
                    {
                        this._Document.Options = this.DocumentOptions;
                        this._Document.EditorControl = this._OwnerWriterControl;
                        this._Document.DocumentControler = this._DocumentControler;
                    }
                }
                if (this._DocumentControler != null)
                {
                    this._DocumentControler.Document = this._Document;
                }
                return _Document;
            }
            set
            {
                this._Document = value;
                if (this._Document != null)
                {
                    if (this.DocumentOptions != null)
                    {
                        this._Document.Options = this.DocumentOptions;
                    }
                    this._Document.EditorControl = this.OwnerWriterControl;
                    this._Document.DocumentControler = this.DocumentControler;
                    this._DocumentControler.Document = this._Document;
                    this._Document.Options = this.DocumentOptions;
                    this._Document.Body.FixElements();
                }
            }
        }

        /// <summary>
        /// 当前插入点所在的元素
        /// </summary>
        public DomElement CurrentElement()
        {
            return this._Document?.CurrentElement;
        }

        /// <summary>
        /// 获得当前插入点所在的指定类型的文档元素对象。
        /// </summary>
        /// <param name="elementType">指定的文档元素类型</param>
        /// <returns>获得的文档元素对象</returns>
        public DomElement GetCurrentElement(Type elementType)
        {
            return this._Document?.GetCurrentElement(elementType, true);
        }


        /// <summary>
        /// 鼠标悬停的元素
        /// </summary>
        public DomElement HoverElement()
        {
            return this._Document?.HoverElement;
        }


        /// <summary>
        /// 获得控件客户区中指定位置处的文档元素对象
        /// </summary>
        /// <param name="x">X坐标</param>
        /// <param name="y">Y坐标</param>
        /// <returns></returns>
        public DomElement GetElementByPosition(int x, int y)
        {
            SimpleRectangleTransform item = this.PagesTransform.GetItemByPoint(x, y, true, false, true);
            if (item != null)
            {
                DomDocument document = (DomDocument)item.DocumentObject;
                PointF p = new PointF(x, y);
                p = RectangleCommon.MoveInto(p, item.SourceRectF);
                if (p.Y == item.SourceRect.Bottom)
                {
                    p.Y = item.SourceRect.Bottom - 2;
                }
                p = item.TransformPointF(p.X, p.Y);
                DomElement element = document.GetElementAt(p.X, p.Y, true);
                return element;
            }
            return null;
        }

        /// <summary>
        /// 获得指定的文档元素在编辑器控件客户区中的边界
        /// </summary>
        /// <param name="element">文档元素对象</param>
        /// <returns>边界</returns>
        public Rectangle GetElementClientBounds(DomElement element)
        {
            if( element is DomCharElement 
                || element is DomParagraphFlagElement
                || element is DomObjectElement
                || element is DomFieldBorderElement)
            {
                // 大多少情况下是字符和段落符号元素
                RectangleF rect = element.GetAbsBounds();
                foreach (SimpleRectangleTransform item in this.PagesTransform)
                {
                    if (item.DescRectFIntersectsWith(rect))
                    {
                        RectangleF rect2 = item.UnTransformRectangleF(rect);
                        Rectangle result2 = new Rectangle(
                            (int)Math.Ceiling(rect2.Left),
                            (int)Math.Ceiling(rect2.Top), 0, 0);
                        result2.Width = (int)Math.Ceiling(rect2.Right - result2.Left);
                        result2.Height = (int)Math.Ceiling(rect2.Bottom - result2.Top);
                        if (result2.Width < 0)
                        {
                            result2.Width = 0;
                        }
                        if (result2.Height < 0)
                        {
                            result2.Height = 0;
                        }
                        if (result2.Width > 0 && result2.Height > 0)
                        {
                            return result2;
                        }
                        break;
                    }//if
                }//foreach
                return Rectangle.Empty;
            }
            var elements = new DomElementList();
            if (element is DomInputFieldElement)
            {

            }
            else
            {
                elements.FastAdd2(element);
            }
            if (elements.Count == 0)
            {
                DomDocumentContentElement dce = element.DocumentContentElement;
                if (element is DomInputFieldElementBase)
                {
                    if (element is DomContainerElement)
                    {
                        DomElementList tempList = new DomElementList();
                        DomContainerElement c = (DomContainerElement)element;
                        c.AppendViewContentElement(new DomContainerElement.AppendViewContentElementArgs( c.OwnerDocument , tempList, true));
                        if (tempList.Count > 0)
                        {
                            foreach (DomElement e in tempList)
                            {
                                if (e.OwnerLine != null && dce.Content.Contains(e))
                                {
                                    elements.FastAdd2(e);
                                }
                            }
                        }
                    }
                }
            }
            Rectangle result = Rectangle.Empty;
            foreach (DomElement e in elements)
            {
                RectangleF rect = e.GetAbsBounds();
                foreach (SimpleRectangleTransform item in this.PagesTransform)
                {
                    if (item.DescRectFIntersectsWith(rect))
                    {
                        RectangleF rect2 = item.UnTransformRectangleF(rect);
                        Rectangle result2 = new Rectangle(
                            (int)Math.Ceiling(rect2.Left),
                            (int)Math.Ceiling(rect2.Top), 0, 0);
                        result2.Width = (int)Math.Ceiling(rect2.Right - result2.Left);
                        result2.Height = (int)Math.Ceiling(rect2.Bottom - result2.Top);
                        if (result2.Width < 0)
                        {
                            result2.Width = 0;
                        }
                        if (result2.Height < 0)
                        {
                            result2.Height = 0;
                        }
                        if (result2.Width > 0 && result2.Height > 0)
                        {
                            if (result.IsEmpty)
                            {
                                result = result2;
                            }
                            else
                            {
                                result = Rectangle.Union(result2, result);
                            }
                        }
                        break;
                    }//if
                }//foreach
            }//foreach
            return result;
        }

        /// <summary>
        /// 清空文档内容
        /// </summary>
        public void ClearContent()
        {
            this._OldPageSizeList = null;
            ClearState();
            if (this.AutoSetDocumentDefaultFont)
            {
                this.UpdateDefaultFont(false);
            }
            if (this._Document != null)
            {
                this._Document.Clear();

                this.RefreshDocument();
                this.Invalidate();
            }
        }


        /// <summary>
        /// 获得从1开始计算的当前列号
        /// </summary>
        public int CurrentColumnIndex()
        {
            var element = this._Document?.Content?.CurrentElement;
            if(element == null )
            {
                return -1;
            }
            else
            {
                return element.ColumnIndex;
            }
        }
        /// <summary>
        /// 获得从1开始计算的当前文本行在页中的序号
        /// </summary>
        public int CurrentLineIndexInPage()
        {
            var line = this._Document?.Content?.CurrentLine;
            if(line == null )
            {
                return -1;
            }
            else
            {
                return line.IndexInPage;
            }
            //if (this.Document.Content.CurrentLine == null)
            //{
            //    return -1;
            //}
            //else
            //{
            //    return this.Document.Content.CurrentLine.IndexInPage;
            //}
        }

        /// <summary>
        /// 获得从1开始计算的当前文本行所在的页码
        /// </summary>
        public int CurrentLineOwnerPageIndex()
        {
            var page = this._Document?.Content?.CurrentLine?.OwnerPage;
            if( page == null || this._Pages == null )
            {
                return 0;
            }
            else
            {
                return this._Pages.IndexOf(page);
            }
        }

        /// <summary>
        /// 文档选择的部分
        /// </summary>
        public DCSelection Selection
        {
            get
            {
                return this._Document?.Selection;
            }
        }
    }
}