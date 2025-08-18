using System;

using DCSoft.WinForms;

using DCSoft.Drawing;
using System.ComponentModel;
using System.Runtime.InteropServices;
using DCSoft.Common;
//using DCSoft.Writer.Script;
using DCSoft.Writer.Controls;
using DCSoft.Writer.Data;


namespace DCSoft.Writer.Dom
{
    /// <summary>
    /// 嵌入在文档中的对象基础类型
    /// </summary>
    [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = false)]
    public abstract partial class DomObjectElement : DomElement
    {
        /// <summary>
        /// 拖拽小矩形的边长 
        /// </summary>
        public static int DragBoxSize = 20;
        /// <summary>
        /// 初始化对象
        /// </summary>
        protected DomObjectElement()
        {
            this.Visible = true;
        }
        private string _ID;
        /// <summary>
        /// 对象编号
        /// </summary>
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public override string ID
        {
            get
            {
                return this._ID;
            }
            set
            {
                this._ID = value;
            }
        }

        /// <summary>
        /// 内容版本号
        /// </summary>
        internal int _ContentVersion;
        /// <summary>
        /// 元素内容版本号，当元素内容发生任何改变时，该属性值都会改变
        /// </summary>
        public override int ContentVersion
        {
            get
            {
                return _ContentVersion;
            }
        }

        private int _StateVersion;
        /// <summary>
        /// 表示文档元素状态版本号。本属性供第三方调用。编辑器内核不使用。
        /// </summary>
        public int StateVersion
        {
            get
            {
                return _StateVersion;
            }
            set
            {
                _StateVersion = value;
            }
        }

        private string _ToolTip;
        /// <summary>
        /// 提示文本
        /// </summary>
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public string ToolTip
        {
            get
            {
                return _ToolTip;
            }
            set
            {
                _ToolTip = value;
            }
        }

        /// <summary>
        /// 元素在文档视图中的绝对坐标值
        /// </summary>
        public override PointF AbsPosition
        {
            get
            {
                return new PointF(this.GetAbsLeft(), this.GetAbsTop());
            }
        }


        /// <summary>
        ///  元素是否可见
        /// </summary>
        public override bool Visible
        {
            get
            {
                return this.StateFlag07;// _Visible;
            }
            set
            {
                this.StateFlag07 = value;// _Visible = value;
                this.InvalidateDocumentLayoutFast();
            }
        }

        /// <summary>
        /// 运行时使用的垂直对齐方式
        /// </summary>
        public virtual VerticalAlignStyle RuntimeVAlign()
        {

            return VerticalAlignStyle.Top;

        }

        /// <summary>
        /// 获得提示文本信息
        /// </summary>
        /// <returns></returns>
        public override ElementToolTip GetToolTipInfo()
        {
            if (this._ToolTip != null && this._ToolTip.Length > 0)
            {
                return new ElementToolTip(this, this.ToolTip);
            }
            return null;
        }


        public virtual float RuntimeWidthHeightRate()
        {
            return 0;
        }
        /// <summary>
        /// 用户是否可以改变对象大小
        /// </summary>
        public virtual ResizeableType Resizeable
        {
            get
            {
                {
                    return ResizeableType.WidthAndHeight;
                }
            }
            set
            {
            }
        }

        /// <summary>
        /// 对象名称
        /// </summary>
        private string _Name;
        /// <summary>
        /// 对象名称
        /// </summary>
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public string Name
        {
            get
            {
                return _Name;
            }
            set
            {
                if (_Name != value)
                {
                    _Name = value;
                    var p = this.Parent;
                    while (p != null)
                    {
                        if (p is DomDocument)
                        {
                            ((DomDocument)p).CheckBoxGroupInfo.Invalidate();
                            break;
                        }
                        p = p.Parent;
                    }
                }
            }
        }

        /// <summary>
        /// 元素大小发生改变
        /// </summary>
        public virtual void OnResize()
        {
        }
        /// <summary>
        /// 创建一个拖拽矩形对象
        /// </summary>
        /// <returns>新的拖拽矩形对象</returns>

        public virtual DragRectangle CreateDragRectangle()
        {
            DragRectangle.DragRectSize = DragBoxSize;
            var rect = new DragRectangle(
                new Rectangle(
                (int)0,
                (int)0,
                (int)this.Width,
                (int)this.Height),
                true);
            rect.CanMove = true;
            rect.LineDashStyle = DashStyle.Solid;
            if (this.OwnerDocument.DocumentControler.CanModify(this, DomAccessFlags.Normal))
            {
                rect.Resizeable = this.Resizeable;

            }
            else
            {
                rect.Resizeable = ResizeableType.FixSize;
                rect.CanMove = false;
            }
            rect.Focus = true;// myOwnerDocument.Content.CurrentElement == this && myOwnerDocument.Content.SelectionLength == 1 ;
            return rect;
        }

        public override void RefreshSize(InnerDocumentPaintEventArgs args)
        {
            //var args = (DocumentPaintEventArgs)objArgs;
            this.SizeInvalid = false;
        }
        /// <summary>
        /// 表示在当前版本是否支持本功能
        /// </summary>
        protected virtual bool NotSupportInThisVersion
        {
            get
            {
                return false;
            }
        }
        /// <summary>
        /// 绘制对象
        /// </summary>
        /// <param name="args"></param>
        public override void Draw(InnerDocumentPaintEventArgs args)
        {
            if (this.DocumentContentElement == null)
            {
                // 在少数情况下元素不属于文档但仍然会触发元素绘图事件。比如删除ControlHost元素的一刹那
                // 会销毁承载的控件而导致编辑器控件重新绘制,而文档DOM视图还没有来得及更新，此时就有可能产生这种情况。
                // 此时进行判断元素所属文档容器元素对象是否为空，如果为空则本元素状态不对，不绘制图形。 
                return;
            }
            try
            {
                args.Render.DrawBackground(this, args);
                if (this.NotSupportInThisVersion)
                {
                    // 当前版本不支持本功能
                    using (var f = new StringFormat())
                    {
                        f.Alignment = StringAlignment.Near;
                        f.LineAlignment = StringAlignment.Center;
                        args.Graphics.DrawString(
                            string.Format(DCSR.NotSupportInThisVersion_Name, this.GetType().Name),
                            XFontValue.DefaultFont,
                            Brushes.Black,
                            args.ViewBounds,
                            f);
                    }
                }
                else
                {
                    this.DrawContent(args);
                }
                if (args.RenderMode == InnerDocumentRenderMode.Paint
                    && args.ActiveMode)
                {
                    if (this.ShowDragRect())
                    {
                        DragRectangle dr = this.CreateDragRectangle();
                        dr.Bounds = new Rectangle(
                            (int)this.GetAbsLeft(),
                            (int)this.GetAbsTop(),
                            dr.Bounds.Width,
                            dr.Bounds.Height);
                        dr.RefreshView(args.Graphics);
                    }
                }
                var bounds = args.ViewBounds;// this.GetAbsBounds();
                bounds.Width = bounds.Width - 1;
                bounds.Height = bounds.Height - 1;
                args.Render.RenderBorder(this, args, bounds);
            }
            catch (Exception ext)
            {
                using (var format = new StringFormat())
                {
                    format.Alignment = StringAlignment.Near;
                    format.LineAlignment = StringAlignment.Near;
                    args.Graphics.ResetClip();
                    var strMsg = ext.ToString();
                    DCConsole.Default.WriteLine(strMsg);
                    args.Graphics.DrawString(
                        strMsg,
                        new XFontValue(),
                        Color.Red,
                        args.ViewBounds,
                        format);
                }
            }
        }

        /// <summary>
        /// DCWriter内部使用。进行控制点测试
        /// </summary>
        /// <param name="x">测试点X坐标</param>
        /// <param name="y">测试点Y坐标</param>
        /// <returns>测试点所在控制点编号</returns>
        public virtual DragPointStyle GetDragHit(int x, int y)
        {
            var dr = this.CreateDragRectangle();
            if (dr == null)
            {
                return DragPointStyle.None;
            }
            else
            {
                return dr.DragHit(x, y);
            }
        }
        /// <summary>
        /// 鼠标点击时选择元素
        /// </summary>
        public virtual bool MouseClickToSelect
        {
            get
            {
                return true;
            }
        }

        internal override System.Windows.Forms.Cursor ElementDefaultCursor()
        {

            return System.Windows.Forms.Cursors.Arrow;

        }

        /// <summary>
        /// 判断能否使用鼠标拖拽该对象
        /// </summary>
        public bool ShowDragRect()
        {
            if (this.Resizeable == ResizeableType.FixSize)
            {
                return false;
            }
            DomDocumentContentElement dce = this.DocumentContentElement;
            return dce.IsSelected(this) && dce.Selection.AbsLength == 1;
        }


        private static Rectangle _LastDragBounds = Rectangle.Empty;
        /// <summary>
        /// DCWriter内部使用。
        /// </summary>

        public Rectangle LastDragBounds
        {
            get { return _LastDragBounds; }
            set { _LastDragBounds = value; }
        }


        /// <summary>
        /// 专为编辑器提供的对象大小属性
        /// </summary>
        public override SizeF EditorSize
        {
            get
            {
                return new SizeF(this.Width, this.Height);
            }
            set
            {
                float width = value.Width;
                float height = value.Height;
                if (this.OwnerDocument != null)
                {
                    if (width > this.OwnerDocument.Width)
                    {
                        width = this.OwnerDocument.Width;
                    }
                }
                double rate = this.RuntimeWidthHeightRate();
                if (rate > 0.1)
                {
                    height = (int)(width / rate);
                }
                if (this.OwnerDocument != null)
                {
                    if (height > this.OwnerDocument.PageSettings.ViewPaperHeight)
                    {
                        height = this.OwnerDocument.PageSettings.ViewPaperHeight;
                        if (rate > 0.1)
                        {
                            width = (int)(height * rate);
                        }
                    }
                }
                this.Width = width;
                this.Height = height;
            }
        }
        /// <summary>
        /// 复制对象
        /// </summary>
        /// <param name="Deeply">是否深度复制</param>
        /// <returns>复制品</returns>

        public override DomElement Clone(bool Deeply)
        {
            DomObjectElement e = (DomObjectElement)base.Clone(Deeply);
            e._ContentVersion = 0;
            return e;
        }
        public override void Dispose()
        {
            base.Dispose();
            this._ID = null;
            this._Name = null;
            this._ToolTip = null;
        }

    }
}