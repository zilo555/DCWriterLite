using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using DCSoft.Drawing ;
namespace DCSoft.Writer.Dom
{
    /// <summary>
    /// 输入域边界元素
    /// </summary>

    [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = false)]
    //[DCSoft.Common.DCPublishAPI]
    //[Serializable]
    public partial class DomFieldBorderElement : DomElement
    {

        /// <summary>
        /// 初始化对象
        /// </summary>
        /// <param name="parent">父对象</param>
        internal DomFieldBorderElement(DomFieldElementBase parent)
        {
            this.InnerSetOwnerDocumentParentRaw(parent.ElementOwnerDocument(), parent);
        }


        /// <summary>
        /// 对象在文档中的绝对坐标位置
        /// </summary>
        public override float AbsLeft
        {
            get
            {
                if (this._OwnerLine == null)
                {
                    return 0;
                }
                else
                {
                    return this._Left + this._OwnerLine.AbsLeft;
                }
            }
        }

        /// <summary>
        /// 对象在文档中的绝对坐标位置
        /// </summary>
        public override float AbsTop
        {
            get
            {
                if (this._OwnerLine == null)
                {
                    return this._Top;
                }
                else
                {
                    return this._Top + this._OwnerLine.AbsTop;
                }
            }
        }
        /// <summary>
        /// 元素在文档视图中的绝对坐标值
        /// </summary>
        public override PointF AbsPosition
        {
            get
            {
                float x = 0;
                float y = 0;
                if (this._OwnerLine == null)
                {
                    return new PointF(this._Left, this._Top);
                }
                else
                {
                    var p = this._OwnerLine.AbsPosition;
                    x = this._Left + p.X;
                    y = this._Top + p.Y;
                }
                return new PointF(x, y);
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


        /// <summary>
        /// 运行时使用的边框颜色
        /// </summary>
        /// <returns></returns>
        private Color RuntimeBorderColor(DocumentViewOptions vp)
        {
            DomFieldElementBase field = (DomFieldElementBase)this.Parent;
            Color bc = Color.Blue;
            if (vp.FieldBorderColor.A != 0)
            {
                // 使用系统配置的颜色
                bc = vp.FieldBorderColor;
            }
            else if (field is DomInputFieldElement)
            {
                DomInputFieldElement field2 = (DomInputFieldElement)field;
                if (field2.InnerRuntimeUserEditable() == false)
                {
                    bc = vp.UnEditableFieldBorderColor;
                }
                else
                {
                    bc = vp.NormalFieldBorderColor;
                }
            }
            if (bc.A == 0)
            {
                bc = Color.Blue;
            }
            return bc;

        }

        /// <summary>
        /// 视图宽度
        /// </summary>
        public override float ViewWidth
        {
            get
            {
                return Math.Max(this.Width, 12.5f);
            }
        }

        /// <summary>
        /// 位置
        /// </summary>
        public BorderElementPosition Position
        {
            get
            {
                if (((DomFieldElementBase)this._Parent)._StartElement == this)
                {
                    return BorderElementPosition.Start;
                }
                else
                {
                    return BorderElementPosition.End;
                }
            }
        }

        /// <summary>
        /// 判断元素是否挂在指定文档的DOM结构中
        /// </summary>
        /// <param name="document">文档对象</param>
        /// <returns>是否挂在DOM结构中</returns>
        public override bool BelongToDocumentDom(DomDocument document)
        {
            if (this._Parent == null)
            {
                return false;
            }
            else
            {
                return this._Parent.BelongToDocumentDom(document);
            }
        }

        /// <summary>
        /// 对象所属的文档域对象
        /// </summary>
        public DomFieldElementBase OwnerField
        {
            get
            {
                return this._Parent as DomFieldElementBase;
            }
        }

        /// <summary>
        /// 对象所属文档对象
        /// </summary>
        public override DomDocument OwnerDocument
        {
            get
            {
                if (this._OwnerDocument != null)
                {
                    return this._OwnerDocument;
                }
                if (this._Parent != null)
                {
                    return this._Parent.ElementOwnerDocument();
                }
                return null;
            }
            set
            {
                base._OwnerDocument = value;
            }
        }

        /// <summary>
        /// 文本
        /// </summary>
        public override string Text
        {
            get
            {
                return null;
                //return _Text;
            }
            set
            {
                throw new NotSupportedException("XTextFieldBorderElement.set_Text");
                //_Text = value;
            }
        }

        private static readonly float _StandHeightDocumentUnit =
            (float)GraphicsUnitConvert.Convert(16, GraphicsUnit.Pixel, GraphicsUnit.Document);
        /// <summary>
        /// 标准高度
        /// </summary>
        internal float StandardHeight()
        {
                return _StandHeightDocumentUnit;
        }

        /// <summary>
        /// 返回空
        /// </summary>
        /// <param name="includeThis"></param>
        /// <returns></returns>
        public override DomDocument CreateContentDocument(bool includeThis)
        {
            return null;
        }
        /// <summary>
        /// 返回纯文本数据
        /// </summary>
        /// <returns>文本数据</returns>
        public override string ToPlaintString()
        {
            return null;
        }
#if !RELEASE
        /// <summary>
        /// 返回表示对象的文本
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            if (this.Position == BorderElementPosition.Start)
            {
                return "【";
            }
            else if (this.Position == BorderElementPosition.End)
            {
                return "】";
            }
            return "FieldBorder";
        }
#endif

        public override void RefreshSize(InnerDocumentPaintEventArgs args)
        {
            this.Width = args.ViewOptions.FieldBorderElementPixelWidth * args.RateOf_PixelToDocumentUnit;
            this.SizeInvalid = false;
        }

        public override void DrawContent(InnerDocumentPaintEventArgs args)
        {
            if (args.RenderMode == InnerDocumentRenderMode.Print)
            {
                return;
            }
            DomDocument document = this.OwnerDocument;
            DomFieldElementBase field = this.OwnerField;
            if (field == null)
            {
                return;
            }
            var absPositionX = args.ViewBounds.Left;
            var absPositionY = args.ViewBounds.Top;
            RectangleF rect = new RectangleF(
                absPositionX,
                absPositionY,
                this.Width,
                this.Height);
            args.Render.DrawBackground(this, args, rect);
            rect.Y = rect.Y + args.CharMeasurer_CharTopFix;
            Color bc = RuntimeBorderColor(args.ViewOptions);
            bool show = args.HiddenFieldBorderWhenLostFocus == false
                    || args.IsFocused(field)
                    || args.Document.IsHover(field);
            if (show == false && field is DomInputFieldElement field2)
            {
                if (field2.BorderVisible == DCVisibleState.AlwaysVisible)
                {
                    show = true;
                }
            }
            if (show)
            {
                PointF[] ps = new PointF[4];
                float dx = rect.Width * 0.2f;
                if (this == field._StartElement)
                {
                    ps[0].X = rect.Right;
                    ps[0].Y = rect.Top;
                    ps[1].X = rect.Left + dx;
                    ps[1].Y = rect.Top;
                    ps[2].X = rect.Left + dx;
                    ps[2].Y = rect.Bottom;
                    ps[3].X = rect.Right;
                    ps[3].Y = rect.Bottom;
                }
                else
                {
                    ps[0].X = rect.Left;
                    ps[0].Y = rect.Top;
                    ps[1].X = rect.Right - dx;
                    ps[1].Y = rect.Top;
                    ps[2].X = rect.Right - dx;
                    ps[2].Y = rect.Bottom;
                    ps[3].X = rect.Left;
                    ps[3].Y = rect.Bottom;
                }
                using (var p = new Pen(bc))
                {
                    args.Graphics.DrawLines(p, ps);
                }
            }
        }
    }

    /// <summary>
    /// 边框元素位置
    /// </summary>
    public enum BorderElementPosition
    {
        /// <summary>
        /// 开始位置
        /// </summary>
        Start,
        /// <summary>
        /// 结束位置
        /// </summary>
        End
    }
}