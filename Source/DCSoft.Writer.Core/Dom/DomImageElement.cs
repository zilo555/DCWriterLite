using System;
// // 
using DCSoft.Drawing;
using System.ComponentModel ;
using DCSoft.Writer.Controls;
using DCSoft.Writer.Data;
using System.Runtime.InteropServices;
using System.Collections.Generic ;
using System.Text;
using System.Collections;

namespace DCSoft.Writer.Dom
{
    /// <summary>
    /// 图片对象
    /// </summary>
    /// <remarks>编制袁永福</remarks>
    public sealed partial class DomImageElement :
        DomObjectElement,
        System.IDisposable
    {
        public static readonly string TypeName_XTextImageElement = "XTextImageElement";
        public override string TypeName => TypeName_XTextImageElement;

        /// <summary>
        /// 初始化对象
        /// </summary>
        public DomImageElement()
        {
            this.KeepWidthHeightRate = true;
            this.SmoothZoom = true;
        }

        private DCSoft.WinForms.ResizeableType _Resizeable = DCSoft.WinForms.ResizeableType.WidthAndHeight;
        /// <summary>
        /// 用户是否可以改变对象大小
        /// </summary>
        public override DCSoft.WinForms.ResizeableType Resizeable
        {
            get
            {
                {
                    return this._Resizeable;
                }
            }
            set
            {
                this._Resizeable = value;
            }
        }

        /// <summary>
		/// 对象宽度和高度的比例,若大于等于0.1则该设置有效，否则无效
		/// </summary>
		private float _WidthHeightRate;
        /// <summary>
        /// 对象宽度高度比,若大于等于0.1则该设置有效，否则无效
        /// </summary>
        public float WidthHeightRate
        {
            get
            {
                return _WidthHeightRate;
            }
            set
            {
                _WidthHeightRate = value;
            }
        }

        public override float RuntimeWidthHeightRate()
        {
            return this.WidthHeightRate;
        }

        private VerticalAlignStyle _VAlign = VerticalAlignStyle.Bottom;
        /// <summary>
        /// 垂直对齐方式
        /// </summary>
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public VerticalAlignStyle VAlign
        {
            get
            {
                return _VAlign;
            }
            set
            {
                _VAlign = value;
            }
        }

        /// <summary>
        /// 运行时使用的垂直对齐方式
        /// </summary>
        public override VerticalAlignStyle RuntimeVAlign()
        {

            return this.VAlign;

        }

        /// <summary>
        /// 文档元素编号前缀
        /// </summary>
        public override string ElementIDPrefix()
        {

            return "image";

        }

        /// <summary>
        /// 保持宽度、高度比例。若本属性值为true，
        /// 则用户鼠标拖拽改变图片大小时会保持图片的宽度高度比例，
        /// 否则用户可以随意改变图片的宽度和高度。
        /// </summary>
        [System.ComponentModel.DefaultValue(true)]
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public bool KeepWidthHeightRate
        {
            get
            {
                return this.StateFlag16;// _KeepWidthHeightRate; 
            }
            set
            {
                this.StateFlag16 = value;// _KeepWidthHeightRate = value;
                UpdateWidthHeightRate();
            }
        }


        /// <summary>
        /// 更新宽度高度比
        /// </summary>
        /// <returns>操作是否导致文档元素大小需要发生改变</returns>
        private bool UpdateWidthHeightRate()
        {
            float newRate = 0;
            if (this.KeepWidthHeightRate
                && _Image != null
                && _Image.Value != null
                && _Image.Height > 0)
            {
                newRate = (float)_Image.Width / (float)_Image.Height;
            }
            else
            {
                newRate = 0;
            }
            if (this.WidthHeightRate != newRate)
            {
                this.WidthHeightRate = newRate;
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 对象宽度
        /// </summary>
        public override float Width
        {
            get
            {
                return base.Width;
            }
            set
            {
                base.Width = value;
                this.InvalidateDocumentLayoutFast();
            }
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
                this.InvalidateDocumentLayoutFast();
            }
        }

        /// <summary>
        /// 内部的图像数据对象
        /// </summary>
        private XImageValue _Image;
        /// <summary>
        /// 内部的图像数据对象
        /// </summary>
        public XImageValue Image
        {
            get
            {
                if (_Image == null)
                {
                    _Image = new XImageValue();
                    _Image.SafeLoadMode = false;
                }
                if (this.OwnerDocument != null)
                {
                    _Image.FormatBase64String = GetDocumentBehaviorOptions().OutputFormatedXMLSource;
                }
                return _Image;
            }
            set
            {
                _Image = value;
            }
        }
        
        /// <summary>
        /// 内部的文档加载后的处理
        /// </summary>
        /// <param name="objArgs">事件参数</param>
        public override void AfterLoad(ElementLoadEventArgs args)
        {
            base.AfterLoad(args);
            // 试图修正一些数据错误。
            float maxWidth = 20000;
            float maxHeight = 20000;
            var ps = this.OwnerDocument?.PageSettings;
            if (ps != null)
            {
                maxWidth = ps.ViewPaperWidth * 5;
                maxHeight = ps.ViewPaperHeight * 5;
            }
            if (this.Width > maxWidth || this.Height > maxHeight)
            {
                if (this.Image != null && this.Image.HasContent)
                {
                    var rate = (float)this.Image.Width / (float)this.Image.Height;
                    if (this.Width < maxWidth)
                    {
                        this.Height = this.Width / rate;
                    }
                    else if (this.Height < maxHeight)
                    {
                        this.Width = this.Height * rate;
                    }
                    return;
                }
                if (this.Width > maxWidth)
                {
                    this.Width = maxWidth;
                }
                if (this.Height > maxHeight)
                {
                    this.Height = maxHeight;
                }
            }
        }

        /// <summary>
        /// 文档元素大小发生改变事件处理
        /// </summary>
        public override void OnResize()
        {
            //UpdateSize();
            UpdateWidthHeightRate();
            base.OnResize();
        }

        /// <summary>
        /// 根据图片内容更新元素的大小
        /// </summary>
        /// <param name="keepSizePossible">是否尽量保持大小不变或少变化</param>
        public void UpdateSize(bool keepSizePossible)
        {
            if (this.Image.HasContent)
            {
                // 设置为图片的原始大小
                SizeF contentSize = GraphicsUnitConvert.Convert(
                    new SizeF(this._Image.Width, this._Image.Height),
                     GraphicsUnit.Pixel,
                    DCSystem_Drawing.GraphicsUnit.Document);
                if (contentSize.IsEmpty)
                {
                    contentSize = new SizeF(100, 100);
                }
                //base.WidthHeightRate = ( double )size.Width / ( double ) size.Height ;
                bool ur = UpdateWidthHeightRate();
                if (keepSizePossible == false || ur == false)
                {
                    RuntimeDocumentContentStyle rs = this.RuntimeStyle;
                    // 设置图片的大小
                    if (keepSizePossible)
                    {
                        // 尽量保持图片元素大小不变
                        if (this.KeepWidthHeightRate && this.WidthHeightRate > 0.01)
                        {
                            // 若需要保持宽度高度比率，则计算新的高度
                            float newHeight = 0;
                            if (rs == null)
                            {
                                newHeight = (float)(this.Width / this.WidthHeightRate);
                            }
                            else
                            {
                                newHeight = (float)(this.ClientWidth / this.WidthHeightRate) + rs.PaddingTop + rs.PaddingBottom;
                            }
                            if (newHeight != this.Height)
                            {
                                this.EditorSize = new SizeF(
                                    this.Width,
                                    newHeight);
                            }
                        }
                    }
                    else
                    {
                        // 设置元素高度为图片高度
                        if (rs == null)
                        {
                            this.EditorSize = new SizeF(contentSize.Width, contentSize.Height);
                        }
                        else
                        {
                            this.EditorSize = new SizeF(
                                contentSize.Width + rs.PaddingLeft + rs.PaddingRight,
                                contentSize.Height + rs.PaddingTop + rs.PaddingBottom);
                        }
                    }
                }
            }
            else
            {
                this.EditorSize = new SizeF(100, 100);
            }
        }

        /// <summary>
        /// 销毁对象
        /// </summary>
        public override void Dispose()
        {
            if (_Image != null)
            {
                _Image.Dispose();
                _Image = null;
            }
        }

        /// <summary>
        /// 复制对象
        /// </summary>
        /// <param name="Deeply">是否深度复制</param>
        /// <returns>复制品</returns>
        public override DomElement Clone(bool Deeply)
        {
            DomImageElement img = (DomImageElement)base.Clone(Deeply);
            if (this._Image != null)
            {
                img._Image = this._Image.Clone();
            }
            return img;
        }

        private void InnerDrawImage(InnerDocumentPaintEventArgs args, XImageValue img)
        {
            if (img == null)
            {
                return;
            }
            RectangleF bounds = args.ClientViewBounds;// this.AbsBounds;
            RectangleF visibleBounds = RectangleF.Intersect(bounds, args.ClipRectangle);
                    visibleBounds.Intersect(this.OwnerLine.AbsBounds);
            PointF lb = visibleBounds.Location;
            visibleBounds.X = visibleBounds.X - bounds.X;
            visibleBounds.Y = visibleBounds.Y - bounds.Y;
            RectangleF imgBounds = new RectangleF(0, 0, img.Width, img.Height);
            RectangleF result = new RectangleF(
                (float)Math.Round(imgBounds.Width * visibleBounds.X / bounds.Width, 3),
                (float)Math.Round(imgBounds.Height * visibleBounds.Y / bounds.Height, 3),
                (float)Math.Round(imgBounds.Width * visibleBounds.Width / bounds.Width, 3),
                (float)Math.Round(imgBounds.Height * visibleBounds.Height / bounds.Height, 3));
            visibleBounds.Location = lb;
            args.Graphics.DrawImage(img, visibleBounds, result, GraphicsUnit.Pixel);
        }
        /// <summary>
        /// 绘制图片文档内容
        /// </summary>
        /// <param name="args"></param>
        public override void DrawContent(InnerDocumentPaintEventArgs args)
        {
            RectangleF bounds = args.ClientViewBounds;// this.AbsBounds;
            RectangleF visibleBounds = bounds;
            visibleBounds.Intersect(args.ClipRectangle);
            XImageValue rim = this.Image;
            if (rim.HasContent)
            {
                var img = rim;
                {
                    if (this.SmoothZoom)
                    {
                        args.NativeGraphics?.SetImageSmoothing(true);
                        InnerDrawImage(args, img);
                        args.NativeGraphics?.SetImageSmoothing(false);
                    }
                    else
                    {
                        InnerDrawImage(args, img);
                    }
                }
            }
            else
            {
                var format = new StringFormat();
                format.Alignment = StringAlignment.Center;
                format.LineAlignment = StringAlignment.Center;
                string text = DCSR.NoImage;
                args.Graphics.DrawString(text, new XFontValue(), Color.Red, bounds, format);
            }
        }

        /// <summary>
        /// 平滑缩放图像
        /// </summary>
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public bool SmoothZoom
        {
            get
            {
                return this.StateFlag18;// _SmoothZoom; 
            }
            set
            {
                this.StateFlag18 = value;// _SmoothZoom = value; 
            }
        }

#if !RELEASE
        /// <summary>
        /// 返回表示对象的字符串
        /// </summary>
        /// <returns>字符串</returns>
        public override string ToString()
        {
            string txt = "[IMG]";
            if (this.ID != null && this.ID.Length > 0)
            {
                txt = txt + "[ID:" + this.ID + "]";
            }
            if (this.Image == null)
            {
                txt = txt + "[NULL]";
            }
            else
            {
                txt = txt + "[" + this.Image.ToString() + "]";
            }
            return txt;
        }
        public override string ToDebugString()
        {
            return this.ToString();
        }
#endif
        /// <summary>
        /// 返回表示对象内容的字符串
        /// </summary>
        /// <returns>字符串</returns>
        public override string ToPlaintString()
        {
            return string.Empty;
        }

        /// <summary>
        /// 表示对象内容的纯文本数据
        /// </summary>
        public override string Text
        {
            get
            {
                return string.Empty;
            }
            set
            {
            }
        }
        public override void InnerGetText(GetTextArgs args)
        {
            args.Append("[图片]");
        }
        public override void WriteText(WriteTextArgs args)
        {

        }
    }
}