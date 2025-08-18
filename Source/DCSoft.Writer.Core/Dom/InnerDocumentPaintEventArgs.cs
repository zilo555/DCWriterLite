using System;
using System.Collections.Generic;
using System.Text;
using DCSoft.Printing;
using DCSoft.Drawing;
using DCSoft.Writer.Controls;
namespace DCSoft.Writer.Dom
{
    /// <summary>
    /// 正在呈现的的文档模式
    /// </summary>
    public enum InnerDocumentRenderMode
    {
        /// <summary>
        /// 正在WinForm用户界面上绘制图形
        /// </summary>
        Paint = (int)DocumentRenderMode.Paint,
        /// <summary>
        /// 正在打印
        /// </summary>
        Print = (int)DocumentRenderMode.Print
    }
    /// <summary>
    /// DCWriter内部使用的文档内容绘图事件参数
    /// </summary>
    public class InnerDocumentPaintEventArgs : ICloneable
    {
        internal InnerDocumentPaintEventArgs(InnerDocumentPaintEventArgs args, InnerDocumentRenderMode mode)
        {
            this.CharMeasurer = args.CharMeasurer;
            this.bolForCreateImage = args.bolForCreateImage;
            this.ActiveMode = args.ActiveMode;
            this.HighlightSelection = args.HighlightSelection;
            this.AllowDrawHighlightBackground = args.AllowDrawHighlightBackground;
            this.CharMeasurer_CharTopFix = args.CharMeasurer_CharTopFix;
            this.Content = args.Content;
            this.ContentElement = args.ContentElement;
            this.Document = args.Document;
            this.DocumentSingleSelectedElement = args.DocumentSingleSelectedElement;
            this.DrawCharFormat = args.DrawCharFormat;
            this.DrawFieldBackgroundText = args.DrawFieldBackgroundText;
            this.Element = args.Element;
            this.EnableElementEvents = args.EnableElementEvents;
            this.FocusedElement = args.FocusedElement;
            this.Graphics = args.Graphics;
            this.SVG = args.Graphics.SVG;
            this.GraphicsDpiX = args.GraphicsDpiX;
            this.GraphicsDpiY = args.GraphicsDpiY;
            this.GraphicsPageUnit = args.GraphicsPageUnit;
            this.GraphisForMeasure = args.GraphisForMeasure;
            this.HiddenFieldBorderWhenLostFocus = args.HiddenFieldBorderWhenLostFocus;
            this.HighlightManager = args.HighlightManager;
            this.intType = args.intType;
            this.IsLoadingDocument = args.IsLoadingDocument;
            this.IsPrintRenderMode = args.IsPrintRenderMode;
            this.LastHighlightInfo = args.LastHighlightInfo;
            this.myClipRectangle = args.myClipRectangle;
            this.NativeDrawCharFormat = args.NativeDrawCharFormat;
            this.NativeGraphics = args.NativeGraphics;
            this.PageBottom = args.PageBottom;
            this.RateOf_PixelToDocumentUnit = args.RateOf_PixelToDocumentUnit;
            this.Render = args.Render;
            this.RenderMode = args.RenderMode;
            this.Selection = args.Selection;
            this.ShowParagraphFlag = args.ShowParagraphFlag;
            this.SizeOfPaintBmpParagraphFlag = args.SizeOfPaintBmpParagraphFlag;
            this.SpecifyDebugMode = args.SpecifyDebugMode;
            this.Style = args.Style;
            this.ViewBounds = args.ViewBounds;
            this.ViewOptions = args.ViewOptions;
            this._Bounds = args._Bounds;
            this._Cancel = args._Cancel;
            this._CheckSizeInvalidateWhenRefreshSize = args.CheckSizeInvalidateWhenRefreshSize;
            this._ClientViewBounds = args._ClientViewBounds;
            this._CurrentElement = args._CurrentElement;
            this._DocumentContentElement = args._DocumentContentElement;
            this._DrawRectangles = args._DrawRectangles;
            this._EnabledDrawGridLine = args._EnabledDrawGridLine;
            this._IsCurrentDocumentContentElement = args._IsCurrentDocumentContentElement;
            this._LastCharElement = args._LastCharElement;
            this._LastHighlightInfoForCharElement = args._LastHighlightInfoForCharElement;
            this._Options = args._Options;
            this._Page = args._Page;
            this._PageClipRectangle = args._PageClipRectangle;
            this._PageCount = args._PageCount;
            this._PageIndex = args._PageIndex;
            this._PageLinePositions = args._PageLinePositions;
            this._ScaleRate = args._ScaleRate;
            this.RenderMode = mode;
        }

        /// <summary>
        /// 初始化对象
        /// </summary>
        /// <param name="document">文档对象</param>
        /// <param name="g">绘图对象</param>
        /// <param name="clipRectangle">剪切矩形</param>
        /// <param name="vRenderMode">呈现模式</param>
        internal InnerDocumentPaintEventArgs(
            DomDocument document,
            DCGraphics g,
             RectangleF clipRectangle,
            InnerDocumentRenderMode vRenderMode = InnerDocumentRenderMode.Paint)
        {
            if (g == null)
            {
                throw new ArgumentNullException("g");
            }

            this.NativeGraphics = g.NativeGraphics;
            this.GraphicsPageUnit = g.PageUnit;
            this.GraphisForMeasure = g.GraphisForMeasure;
            this.RenderMode = vRenderMode;
            this.IsPrintRenderMode =
                this.RenderMode == InnerDocumentRenderMode.Print;
            this.Graphics = g;
            this.SVG = g.SVG;
            this.myClipRectangle = clipRectangle;
            this.ViewBounds = clipRectangle;
            this._DrawRectangles = new  RectangleF[] { clipRectangle };
            if (document != null)
            {
                SetDocument(document);
            }
            //this._Recorder = g.NewRecorder; 
        }
        //internal bool AllowDrawGridLine = true;
        internal DCSoft.WASM.MyPrintTaskOptions PrintTaskOptions = null;
        internal bool IsLoadingDocument = false;
        //internal DCGrapchisRecorder _Recorder = null;
         
        internal bool EnableElementEvents = true;
        internal bool ShowParagraphFlag = true;

        internal DomElement DocumentSingleSelectedElement = null;
        internal SizeF SizeOfPaintBmpParagraphFlag = SizeF.Empty;

        internal readonly Bitmap BmpParagraphFlagLeftToRight = DCStdImageList.BmpParagraphFlagLeftToRight;

        internal HighlightInfo LastHighlightInfo = null;
        
        internal DomElement _LastCharElement = null;
        internal HighlightInfo _LastHighlightInfoForCharElement = null;

        internal HighlightManager HighlightManager;
        /// <summary>
        /// 是否为Print呈现模式。
        /// </summary>
        public readonly bool IsPrintRenderMode;
        internal DomElement FocusedElement = null;
        internal void SetDocument(DomDocument document)
        {
            if (document == null)
            {
                throw new ArgumentNullException("document");
            }
            this.IsLoadingDocument = document.IsLoadingDocument;
            this.EnableElementEvents = document.GetDocumentBehaviorOptions().EnableElementEvents;
            this.DocumentSingleSelectedElement = document.SingleSelectedElement;
            try
            {
                var bmp = this.BmpParagraphFlagLeftToRight;
                if (bmp != null)
                {
                    //lock (bmp)
                    {
                        this.SizeOfPaintBmpParagraphFlag = GraphicsUnitConvert.Convert(
                           new SizeF( bmp.Width , bmp.Height ), 
                           GraphicsUnit.Pixel,
                           DCSystem_Drawing.GraphicsUnit.Document);
                    }
                }
            }
            catch
            {
                this.SizeOfPaintBmpParagraphFlag = GraphicsUnitConvert.Convert(
                   new  SizeF(9, 12),
                   GraphicsUnit.Pixel,
                   DCSystem_Drawing.GraphicsUnit.Document);
            }
            this.Document = document;
            this.ViewOptions = document.GetDocumentViewOptions();
            this.HiddenFieldBorderWhenLostFocus = this.ViewOptions.HiddenFieldBorderWhenLostFocus;
            this.ShowParagraphFlag = this.ViewOptions.ShowParagraphFlag;

            this.Render = document.Render;
            this.HighlightManager = document.HighlightManager;

            if (this.RenderMode == InnerDocumentRenderMode.Paint)
            {
                // 在绘图模式下输出文字
                this.DrawFieldBackgroundText = true;
            }
            else
            {
                if ((this.RenderMode == InnerDocumentRenderMode.Print)
                    && this.ViewOptions.PrintBackgroundText)
                {
                    // 打印模式下允许绘制背景文字
                    this.DrawFieldBackgroundText = true;
                }
                else
                {
                    // 在非绘图模式下不显示
                    this.DrawFieldBackgroundText = false;
                }
            }
            this.DrawCharFormat = document.Render.DrawCharFormat();
            this.NativeDrawCharFormat = document.Render.NativeDrawCharFormat;
            this.CharMeasurer = document.Render.CharMeasurer;
            this.CharMeasurer_CharTopFix = this.CharMeasurer.CharTopFix;
            var bops = document.GetDocumentBehaviorOptions();
            this.SpecifyDebugMode = bops.SpecifyDebugMode;
            this.RateOf_PixelToDocumentUnit = (float)GraphicsUnitConvert.GetRate(DCSystem_Drawing.GraphicsUnit.Document, GraphicsUnit.Pixel);

        }

        public bool HighlightSelection = true;
        public bool HiddenFieldBorderWhenLostFocus = true;
        public bool DrawFieldBackgroundText = true;

        public float CharMeasurer_CharTopFix = 0;
        public DCContent Content = null;

        public StringFormat NativeDrawCharFormat = null;
        public CharacterMeasurer CharMeasurer = null;
        public StringFormat DrawCharFormat = null;

        public  Graphics NativeGraphics = null;
        public readonly float GraphicsDpiX = 96;
        public readonly float GraphicsDpiY = 96;

        public bool SpecifyDebugMode = false;

        public float RateOf_PixelToDocumentUnit = 0;

#region 绘图函数 ***************************************************

        /// <summary>
        /// 获得字体高度
        /// </summary>
        /// <param name="font"></param>
        /// <returns></returns>
        public float GraphicsGetFontHeight(RuntimeDocumentContentStyle rs)
        {
            return rs.GetFontHeight(this.Graphics);//.GraphisForMeasure);
            //if (CharacterMeasurer.Options_MeasureUseTrueTypeFont)
            //{
            //    return CharacterMeasurer.GetFontHeight(this.GraphisForMeasure(), rs);
            //}
            //else
            //{
            //    return this.Graphics.GetFontHeight(rs.FontValue);
            //}
        }

        /// <summary>
        /// 获得字体高度
        /// </summary>
        /// <param name="font"></param>
        /// <returns></returns>
        public float GraphicsGetFontHeight(XFontValue font)
        {
            //if (CharacterMeasurer.Options_MeasureUseTrueTypeFont)
            //{
                return CharacterMeasurer.GetFontHeightUseTrueTypeFont(this.Graphics.PageUnit ,font);
            //}
            //else
            //{
            //    return this.Graphics.GetFontHeight(font);
            //}
        }
        public float GraphicsGetFontHeight(DCSystem_Drawing.Font font)
        {
            //if (CharacterMeasurer.Options_MeasureUseTrueTypeFont)
            //{
            return CharacterMeasurer.GetFontHeightUseTrueTypeFont(this.Graphics.PageUnit, font);
            //}
            //else
            //{
            //    return this.Graphics.GetFontHeight(font);
            //}
        }

        public readonly GraphicsUnit GraphicsPageUnit;

        /// <summary>
        /// 绘制字符串
        /// </summary>
        /// <param name="s">字符串</param>
        /// <param name="rs">样式</param>
        /// <param name="specifyFontSize">指定字体大小</param>
        /// <param name="brush">画刷对象</param>
        /// <param name="layoutRectangle">排版矩形</param>
        /// <param name="format">格式化对象</param>
        public void GraphicsDrawString(
            string s,
            RuntimeDocumentContentStyle rs,
            XFontValue runtimeFont,
            Color txtColor,
            RectangleF layoutRectangle,
            StringFormat format)
        {
            bool useRuntimeFont = false;
            if (runtimeFont != null && rs.Font != runtimeFont)
            {
                if (rs.FontSize != runtimeFont.Size
                    || rs.Bold != runtimeFont.Bold
                    || rs.Italic != runtimeFont.Italic
                    || rs.Strikeout != runtimeFont.Strikeout
                    || rs.Underline != runtimeFont.Underline)
                {
                    useRuntimeFont = true;
                }
            }

            {
                if (useRuntimeFont)
                {
                    this.Graphics.DrawString(
                        s, 
                        runtimeFont, 
                        txtColor, 
                        layoutRectangle, 
                        format);
                }
                else
                {
                    this.Graphics.DrawString(
                        s,
                        rs.Font, 
                        txtColor,
                        layoutRectangle, 
                        format);
                }
            }
        }


        public readonly Graphics GraphisForMeasure;

        public SizeF GraphicsMeasureString(string text, RuntimeDocumentContentStyle rs, int width, StringFormat format)
        {
            //if (CharacterMeasurer.Options_MeasureUseTrueTypeFont)
            //{
                return CharacterMeasurer.MeasureString(this.GraphisForMeasure, text, rs, width, format);
            //}
            //else
            //{
            //    return this.Graphics.MeasureString(text, rs.Font , width, format);
            //}
        }

        public SizeF GraphicsMeasureString(string text, XFontValue font, int width, StringFormat format)
        {
            //if (CharacterMeasurer.Options_MeasureUseTrueTypeFont)
            //{
                return CharacterMeasurer.MeasureString(this.Graphics , text, font, width, format);
            //}
            //else
            //{
            //    return this.Graphics.MeasureString(text, font, width, format);
            //}
        }


#endregion

        private bool _EnabledDrawGridLine = true;
        /// <summary>
        /// 允许绘制网格线
        /// </summary>
        public bool EnabledDrawGridLine
        {
            get
            {
                return _EnabledDrawGridLine; 
            }
            set
            {
                _EnabledDrawGridLine = value; 
            }
        }
        internal bool _CheckSizeInvalidateWhenRefreshSize = false;
        /// <summary>
        /// 在计算元素大小时是否检查元素的SizeInvalidte属性值。
        /// </summary>
        public bool CheckSizeInvalidateWhenRefreshSize
        {
            get
            {
                return _CheckSizeInvalidateWhenRefreshSize;
            }
            set
            {
                _CheckSizeInvalidateWhenRefreshSize = value;
            }
        }
        /// <summary>
        /// 图形内容呈现器
        /// </summary>
        public DCContentRender Render;
        /// <summary>
        /// 相关的文档对象
        /// </summary>
        public DomDocument Document = null;
        /// <summary>
        /// 视图选项
        /// </summary>
        public DocumentViewOptions ViewOptions = null;


        private DomDocumentContentElement _DocumentContentElement = null;
        internal void SetDocumentContentElement(DomDocumentContentElement dce)
        {
            if (dce == null)
            {
                throw new ArgumentNullException("dce");
            }
            this._DocumentContentElement = dce;
            this._CurrentElement = dce.CurrentElement;
            this._IsCurrentDocumentContentElement = this.Document.IsCurrentContentElement(dce);
            this.Content = dce.Content;
            this.Selection = dce.Selection;
            if(this.RenderMode != InnerDocumentRenderMode.Paint)
            {
                this.ActiveMode = false;
            }
            this.AllowDrawHighlightBackground = 
                this.ActiveMode
                && this.RenderMode == InnerDocumentRenderMode.Paint
                && this.HighlightManager != null;
        }
        /// <summary>
        /// 是否允许绘制高亮度区域的背景色
        /// </summary>
        public bool AllowDrawHighlightBackground = false;

        internal bool IsFocused(DomContainerElement element)
        {
            if (element == null)
            {
                return false;
            }
            if (this._DocumentContentElement == null)
            {
                return element.Focused;
            }
            if (this._IsCurrentDocumentContentElement)
            {
                return element.FastIsFocused(this._DocumentContentElement, this._CurrentElement);
            }
            else
            {
                return false;
            }
        }

        internal bool IsSelectedCell(DomTableCellElement cell)
        {
            if (this.DocumentContentElement == null)
            {
                return cell.IsSelected;
            }
            return this.Selection != null && this.Selection.ContainsCell(cell);
        }

        private DomElement _CurrentElement = null;
        private DCSelection Selection = null;
        private bool _IsCurrentDocumentContentElement = false;
        /// <summary>
        /// 内容元素对象
        /// </summary>
        public DomDocumentContentElement DocumentContentElement
        {
            get
            {
                return this._DocumentContentElement;
            }
        }
        internal DomContentElement ContentElement = null;

        /// <summary>
        /// 要绘制的内容处于激活模式
        /// </summary>
        public bool ActiveMode = true;
        public DomElement Element = null;

        private PageContentPartyStyle intType = PageContentPartyStyle.Body;
        /// <summary>
        /// 文档内容类型
        /// </summary>
        public PageContentPartyStyle Type
        {
            get
            {
                return intType;
            }
            set
            {
                intType = value;
            }
        }
        public RuntimeDocumentContentStyle Style = null;
        private bool _Cancel = false;
        /// <summary>
        /// 用户取消操作
        /// </summary>
        public bool Cancel
        {
            get
            {
                return _Cancel;
            }
            set
            {
                _Cancel = value;
            }
        }


        /// <summary>
        /// 正在呈现的文档样式。这个字段应该是DocumentRenderMode枚举类型，但为了迷惑破解者而改用整数类型。
        /// </summary>
        public readonly InnerDocumentRenderMode RenderMode = InnerDocumentRenderMode.Paint;

        private bool bolForCreateImage = false;
        /// <summary>
        /// 正在为创建图片而绘制图形
        /// </summary>
        public bool ForCreateImage
        {
            get
            {
                return bolForCreateImage;
            }
            set
            {
                bolForCreateImage = value;
            }
        }

        /// <summary>
        /// 绘图对象
        /// </summary>
        public readonly DCGraphics Graphics = null;
        public readonly DCSoft.SVG.SVGGraphics SVG = null;

        private int[] _PageLinePositions = null;
        /// <summary>
        /// 分页线位置
        /// </summary>
        public int[] PageLinePositions
        {
            get
            {
                return _PageLinePositions;
            }
            set
            {
                _PageLinePositions = value;
            }
        }


        private  RectangleF _PageClipRectangle
            =  RectangleF.Empty;
        /// <summary>
        /// 页面剪切矩形
        /// </summary>
        public  RectangleF PageClipRectangle
        {
            get
            {
                return _PageClipRectangle;
            }
            set
            {
                _PageClipRectangle = value;
            }
        }


        private PrintPage _Page = null;
        /// <summary>
        /// 页面对象
        /// </summary>
        public PrintPage Page
        {
            get
            {
                return _Page;
            }
            set
            {
                _Page = value;
                if (value != null)
                {
                    this._PageIndex = value.PageIndex;
                    this.PageBottom = value.Bottom;
                }
            }
        }
        public float PageBottom = 0;

        private int _PageIndex = 0;
        /// <summary>
        /// 从0开始计算的页码号
        /// </summary>
        public int PageIndex
        {
            get
            {
                return _PageIndex;
            }
            set
            {
                _PageIndex = value;
            }
        }

        private int _PageCount = 0;
        /// <summary>
        /// 总页数
        /// </summary>
        public int PageCount
        {
            get
            {
                return _PageCount;
            }
            set
            {
                _PageCount = value;
            }
        }

        /// <summary>
        /// 剪切矩形
        /// </summary>
        protected  RectangleF myClipRectangle
            =  RectangleF.Empty;
        /// <summary>
        /// 剪切矩形
        /// </summary>
        public  RectangleF ClipRectangle
        {
            get
            {
                return myClipRectangle;
            }
            set
            {
                myClipRectangle = value;
            }
        }

        public bool IntersectsWithClipRectangle(RectangleF rect)
        {
            return this.myClipRectangle.IsEmpty || this.myClipRectangle.IntersectsWith(rect);
        }

        private DCPrintDocumentOptions _Options = null;
        /// <summary>
        /// 打印文档选项
        /// </summary>
        public DCPrintDocumentOptions Options
        {
            get
            {
                return _Options; 
            }
            set
            {
                _Options = value;
            }
        }

        private  RectangleF _Bounds = RectangleF.Empty;
        /// <summary>
        /// 边界
        /// </summary>
        public  RectangleF Bounds
        {
            get
            {
                return _Bounds;
            }
            set
            {
                _Bounds = value;
            }
        }

        /// <summary>
        /// 绘图区域的矩形数组
        /// </summary>
        protected  RectangleF[] _DrawRectangles = null;
        /// <summary>
        /// 绘图区域的矩形数组
        /// </summary>
        public  RectangleF[] DrawRectangles
        {
            get
            {
                return _DrawRectangles;
            }
            set
            {
                _DrawRectangles = value;
            }
        }

        private float _ScaleRate = 1.0f;
        /// <summary>
        /// 缩放比率
        /// </summary>
        public float ScaleRate
        {
            get
            {
                return _ScaleRate;
            }
            set
            {
                _ScaleRate = value;
            }
        }

        /// <summary>
        /// 对象的区域
        /// </summary>
        public  RectangleF ViewBounds =  RectangleF.Empty;
        
        private RectangleF _ClientViewBounds = RectangleF.Empty;
        /// <summary>
        /// 对象客户区的边界
        /// </summary>
        public RectangleF ClientViewBounds
        {
            get
            {
                return _ClientViewBounds;
            }
            set
            {
                _ClientViewBounds = value;
            }
        }
        /// <summary>
        /// 复制对象的一个复本
        /// </summary>
        /// <returns>复制的对象</returns>
        object System.ICloneable.Clone()
        {
            InnerDocumentPaintEventArgs a = (InnerDocumentPaintEventArgs)this.MemberwiseClone();
            return a;
        }

        /// <summary>
        /// 复制对象的一个复本
        /// </summary>
        /// <returns>复制的对象</returns>
        public InnerDocumentPaintEventArgs Clone()
        {
            var args = (InnerDocumentPaintEventArgs)this.MemberwiseClone();// ((ICloneable)this).Clone();
            //args._PublishArgs = null;
            return args;
        }
    }

}
