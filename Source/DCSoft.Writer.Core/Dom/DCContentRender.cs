using System;
using System.Collections.Generic;
using System.Text;
// // 
using System.ComponentModel;
using DCSoft.Writer.Controls ;
using DCSoft.Drawing;
using DCSoft.Common;
using DCSoft.WinForms;
using System.Reflection.PortableExecutable;

namespace DCSoft.Writer.Dom
{
    /// <summary>
    /// 文档内容呈现器
    /// </summary>
    /// <remarks>编制 袁永福</remarks>
    public class DCContentRender : IDisposable
    {
        public static void VoidMethod()
        {

        }
        /// <summary>
        /// 初始化对象
        /// </summary>
        public DCContentRender(DomDocument doc)
        {
            if (doc == null)
            {
                throw new ArgumentNullException("doc");
            }
            this._Document = doc;

        }

        private readonly DomDocument _Document = null;
        /// <summary>
        /// 对象所属文档对象
        /// </summary>
        public DomDocument Document
        {
            get
            {
                return _Document;
            }
            //set
            //{
            //    _Document = value;
            //    ClearCharSizeBuffer();
            //}
        }

        /// <summary>
        /// 绘制文档元素背景
        /// </summary>
        /// <param name="element">文档元素对象</param>
        /// <param name="args">参数</param>
        /// <param name="bounds">要绘制背景的区域</param>
        /// <returns>是否真的执行了绘图操作</returns>
        public virtual bool DrawCellBackground(
            DomTableCellElement element,
            InnerDocumentPaintEventArgs args,
            RectangleF bounds)
        {
            //if (bounds.IsEmpty)
            //{
            //    // 不绘制图形
            //    return false;
            //}
            RuntimeDocumentContentStyle rs = args.Style;
            if (rs.HasVisibleBackground == false)
            {
                var rs2 = element._Parent?.RuntimeStyle;
                if (rs2 != null && rs2.HasVisibleBackground)
                {
                    rs = rs2;
                }
            }
            Color c = rs.BackgroundColor;
            if (c.A != 0)
            {
                //if (element is XTextCharElement)
                //{
                //    bounds.Offset(0, 6.25f);
                //}
                // SolidBrush b = GraphicsObjectBuffer.GetSolidBrush(c);
                //bounds =  RectangleF.Intersect(bounds, args.ClipRectangle);
                if (bounds.IsEmpty == false)
                {
                    if (bounds.Height < 8)
                    {
                        // 高度很小，进行页面下边缘细微判断
                        if (args.Page != null)
                        {
                            float dis = bounds.Top - args.Page.Bottom;
                            if (dis < 0.5)
                            {
                                // 差距非常的小，则认为是属于下一页的内容。
                                return false;
                            }
                        }
                    }
                    args.Graphics.FillRectangle(c, bounds);
                    return true;
                }
            }
            if (args.AllowDrawHighlightBackground)
            {
                var highlight = args.HighlightManager[element];
                if (highlight != null)
                {
                    if (args.IsPrintRenderMode
                        && highlight.ActiveStyle != HighlightActiveStyle.AllTime)
                    {
                        // 当进行打印时不显示不能打印的高亮度色
                        return false;
                    }
                    var line = element._OwnerLine;
                    if (line != null)
                    {
                        bounds.Y = line.AbsTop + line.ContentTopFix;
                        bounds.Height = line.ContentHeight;
                    }
                    // 如果需要高亮度显示
                    // 则绘制高亮度背景
                    bounds = RectangleF.Intersect(bounds, args.ClipRectangle);
                    if (bounds.IsEmpty == false && highlight.BackColor.A != 0)
                    {
                        args.Graphics.FillRectangle(
                            highlight.BackColor,// GraphicsObjectBuffer.GetSolidBrush(highlight.BackColor),
                            bounds.Left,
                            bounds.Top,
                            bounds.Width,
                            bounds.Height);
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// 绘制文档元素背景
        /// </summary>
        /// <param name="element">文档元素对象</param>
        /// <param name="args">参数</param>
        /// <param name="bounds">要绘制背景的区域</param>
        /// <returns>是否真的执行了绘图操作</returns>
        public virtual bool DrawBackground(
            DomElement element,
            InnerDocumentPaintEventArgs args,
            RectangleF bounds)
        {
            //if (bounds.IsEmpty)
            //{
            //    // 不绘制图形
            //    return false;
            //}
            RuntimeDocumentContentStyle rs = null;
            if (args.Style.HasVisibleBackground == false
                && (element.Parent is DomContentElement) == false)
            {
                rs = element.Parent?.GetContentBackgroundStyle(element);
                if (rs == null)
                {
                    rs = args.Style;
                }
            }
            else
            {
                rs = args.Style;
            }
            //if (args.Style.HasVisibleBackground == false
            //    && element.Parent != null
            //    && ((element.Parent is XTextContentElement) == false))
            //{
            //    rs = element.Parent.GetContentBackgroundStyle(element);
            //}
            //if (args.Style.HasVisibleBackground)
            //{
            //    rs = args.Style;
            //}
            //else
            //{
            //    if (element.Parent != null)
            //    {
            //        if ((element.Parent is XTextContentElement) == false )
            //        {
            //            rs = element.Parent.GetContentBackgroundStyle(element);
            //        }
            //    }
            //}
            //if (rs == null)
            //{
            //    rs = args.Style;
            //}
            Color c = rs.BackgroundColor;
            if (c.A != 0)
            {
                //if (element is XTextCharElement)
                //{
                //    bounds.Offset(0, 6.25f);
                //}
                // SolidBrush b = GraphicsObjectBuffer.GetSolidBrush(c);
                bounds = RectangleF.Intersect(bounds, args.ClipRectangle);
                if (bounds.IsEmpty == false)
                {
                    if (bounds.Height < 8)
                    {
                        // 高度很小，进行页面下边缘细微判断
                        if (args.Page != null)
                        {
                            float dis = bounds.Top - args.Page.Bottom;
                            if (dis < 0.5)
                            {
                                // 差距非常的小，则认为是属于下一页的内容。
                                return false;
                            }
                        }
                    }
                    args.Graphics.FillRectangle(c, bounds);
                    return true;
                }
            }
            if (args.AllowDrawHighlightBackground)
            {
                HighlightInfo highlight = null;
                highlight = args.HighlightManager[element];
                args._LastCharElement = element as DomCharElement;
                if (args._LastCharElement != null)
                {
                    args._LastHighlightInfoForCharElement = highlight;
                }
                if (highlight != null)
                {
                    args.LastHighlightInfo = highlight;
                    if (args.IsPrintRenderMode
                        && highlight.ActiveStyle != HighlightActiveStyle.AllTime)
                    {
                        // 当进行打印时不显示不能打印的高亮度色
                        return false;
                    }
                    var line = element._OwnerLine;
                    if (line != null)
                    {
                        bounds.Y = line.AbsTop + line.ContentTopFix;
                        bounds.Height = line.ContentHeight;
                    }
                    // 如果需要高亮度显示
                    // 则绘制高亮度背景
                    bounds = RectangleF.Intersect(bounds, args.ClipRectangle);
                    if (bounds.IsEmpty == false && highlight.BackColor.A != 0)
                    {
                        args.Graphics.FillRectangle(
                            highlight.BackColor,// GraphicsObjectBuffer.GetSolidBrush(highlight.BackColor),
                            bounds.Left,
                            bounds.Top,
                            bounds.Width,
                            bounds.Height);
                        return true;
                    }
                }
            }

            //#endif
            return false;
        }

        /// <summary>
        /// 绘制元素边框
        /// </summary>
        /// <param name="element">元素对象</param>
        /// <param name="args">绘制事件参数</param>
        /// <param name="bounds">边界</param>
        public virtual void RenderBorder(
            DomElement element,
            InnerDocumentPaintEventArgs args,
            RectangleF bounds)
        {
            RuntimeDocumentContentStyle style = element.RuntimeStyle;

            if (style.HasVisibleBorder)
            {
                    style.DrawBorder(args.Graphics, bounds);
            }
        }



        /// <summary>
        /// 绘制对象背景
        /// </summary>
        /// <param name="element">文档元素对象</param>
        /// <param name="args">参数</param>
        public virtual void DrawBackground(DomElement element, InnerDocumentPaintEventArgs args)
        {
            if (element == null || element.OwnerLine == null)
            {
                return;
            }

            var absp = element.AbsPosition;

            RectangleF bounds = new RectangleF(
               absp.X,
               absp.Y,
               element.Width + element.WidthFix,
               element.Height);
            DrawBackground(element, args, bounds);
        }


        #region 计算和绘制字符元素

        //internal TickCountInfo _CharRefreshSize_TickInfo = new TickCountInfo( true );

        private CharacterMeasurer _CharMeasurer = null;

        public CharacterMeasurer CharMeasurer
        {
            get
            {
                if (_CharMeasurer == null)
                {
                    var c = new CharacterMeasurer();

                    //// 添加藏文默认字体
                    //c.SetDefaultFont((char)0xf00, (char)0xfff, "Microsoft Himalaya");
                    //// 添加中文默认字体
                    //c.SetDefaultFont((char)CharacterMeasurer.ChineseStartCode, (char)CharacterMeasurer.ChineseEndCode, "宋体");

                    _CharMeasurer = c;
                    ///// <summary>
                    ///// 判断是否为藏文字符
                    ///// </summary>
                    ///// <param name="c"></param>
                    ///// <returns></returns>
                    //public static bool IsTibetanChar(char c)
                    //{
                    //    if (0xf00 <= c && c <= 0xfff)
                    //    {
                    //        return true;
                    //    }
                    //    return false;
                    //}

                }
                return _CharMeasurer;
            }
        }

        private static readonly StringFormat _NativeMeasureFormat = Create_NativeMeasureFormat();

        public StringFormat NativeMeasureFormat
        {
            get
            {
                return _NativeMeasureFormat;
            }
        }

        private static StringFormat Create_NativeMeasureFormat()
        {
            var f = new StringFormat(StringFormat.GenericTypographic);
            return f;
        }

        private static StringFormat _MeasureFormat = null;
        public StringFormat MeasureFormat()
        {
            return StaticMeasureFormat();
        }
        /// <summary>
        /// 计算字体大小使用的字体格式化对象。
        /// </summary>
        public static StringFormat StaticMeasureFormat()
        {
            if (_MeasureFormat == null)
            {
                _MeasureFormat = new StringFormat(StringFormat.GenericTypographic);
            }
            return _MeasureFormat;
        }

        internal void ResetStateForDocument(DCGraphics g)
        {
            DomDocument document = this.Document;
            if (this._DocumentOptions == null)
            {
                this._DocumentOptions = document.GetDocumentOptions();
            }
            if (this._DocumentViewOptions == null)
            {
                this._DocumentViewOptions = _DocumentOptions.ViewOptions;
            }
            this._PageUnit = g.PageUnit;
            foreach (DocumentContentStyle style in this.Document.ContentStyles.Styles.FastForEach())
            {
                if (style._MyRuntimeStyle != null)
                {
                    style._MyRuntimeStyle.StdSize10FontInfoForMeasure = null;
                }
            }
            var cmr = this.CharMeasurer;
            cmr.GraphicsUnit = this._PageUnit;
            this._SpecifyDebugMode = this._DocumentOptions.BehaviorOptions.SpecifyDebugMode;
        }

        internal bool _SpecifyDebugMode = false;
        private GraphicsUnit _PageUnit = GraphicsUnit.Document;
        private DocumentOptions _DocumentOptions = null;
        private DocumentViewOptions _DocumentViewOptions = null;


        /// <summary>
        /// 绘制字符使用的格式化对象
        /// </summary>
        private static StringFormat _NativeDrawCharFormat = null;

        public StringFormat NativeDrawCharFormat
        {
            get
            {
                if (_NativeDrawCharFormat == null)
                {
                    _NativeDrawCharFormat = new StringFormat(StringFormat.GenericTypographic);
                }
                return _NativeDrawCharFormat;
            }
        }


        /// <summary>
        /// 绘制字符使用的格式化对象
        /// </summary>
        private static StringFormat _DrawCharFormat = null;

        public StringFormat DrawCharFormat()
        {
            if (_DrawCharFormat == null)
            {
                _DrawCharFormat = new StringFormat(StringFormat.GenericTypographic);
            }
            return _DrawCharFormat;
        }

        public void DrawUnderLine(
            DomElement element,
            InnerDocumentPaintEventArgs args,
            Color defaultColor,
            RectangleF rect)
        {
            // 绘制下划线
            {
                var pos = rect.Bottom - 3;
                RuntimeDocumentContentStyle rs = element.RuntimeStyle;
                ////TextUnderlineStyle tus = rs.UnderLineStyle;
                //float fh = args.GraphicsGetFontHeight(rs) ;
                Color uc = defaultColor;
                //Pen p = GraphicsObjectBuffer.GetPen(uc);
                using (var p = new Pen(uc))
                {
                    args.Graphics.DrawLine(
                        p,
                        rect.Left,
                        pos,
                        rect.Left + element.Width + element.WidthFix,
                        pos);
                }
            }
        }

        #endregion
        public void Dispose()
        {
            this._CharMeasurer = null;
        }
    }
}
