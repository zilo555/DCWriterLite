using System;
using System.Collections.Generic;
using System.Text;
using DCSoft.Common;
using DCSoft.Drawing;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Collections ;

namespace DCSoft.Writer.Dom
{
    /// <summary>
    /// 文档样式信息对象
    /// </summary>
    public partial class DocumentContentStyle : ContentStyle
    {
        /// <summary>
        /// 初始化对象
        /// </summary>
        public DocumentContentStyle()
        {
        }

        protected override bool FastSetValueMode()
        {
            return DCSoft.Writer.Dom.DomElementList._InnerDeserializing;
        }

        /// <summary>
        /// DCWriter内部使用.鎖定字體大小。
        /// </summary>
        public bool InnerLockFontSize = false;

        public override ContentStyle Clone()
        {
            var style = new DocumentContentStyle();
            foreach (var item in this._InnerValues)
            {
                style._InnerValues.Add(item.Key, item.Value);
            }
            style.ValueModified = true;

            //DocumentContentStyle style = (DocumentContentStyle)base.Clone();
            //style.SetMyRuntimeStyle(null);
            return style;

        }
        public override void Dispose()
        {
            base.Dispose();
            SetMyRuntimeStyle(null);
        }

        internal void SetMyRuntimeStyle(RuntimeDocumentContentStyle rs)
        {
            if (this._MyRuntimeStyle != rs && this._MyRuntimeStyle != null)
            {

            }
            this._MyRuntimeStyle = rs;
        }

        internal RuntimeDocumentContentStyle _MyRuntimeStyle = null;
        /// <summary>
        /// 运行时样式
        /// </summary>
        public RuntimeDocumentContentStyle MyRuntimeStyle
        {
            get
            {
                if (_MyRuntimeStyle == null)
                {
                    //CheckCrosThread();
                    SetMyRuntimeStyle(new RuntimeDocumentContentStyle(this));
                }
                return _MyRuntimeStyle;
            }
        }

        protected override void OnValueChanged(XDependencyProperty property)
        {
            //CheckCrosThread();
            //if( this._MyRuntimeStyle != null )
            //{

            //}
            SetMyRuntimeStyle(null);
            //this._MyRuntimeStyle = null;
        }


        /// <summary>
        /// 删除指定名称的属性值
        /// </summary>
        /// <param name="name">属性名称</param>
        /// <returns>操作是否修改了对象数据</returns>
        public bool RemovePropertyValue(string name)
        {
            foreach (XDependencyProperty p in this.InnerValues.Keys)
            {
                if (string.Compare(p.Name, name, true) == 0)
                {
                    this.InnerValues.Remove(p);
                    this.ValueModified = true;
                    return true;
                }
            }
            XDependencyProperty[] ps = XDependencyProperty.GetProperties(this.GetType(), false);
            foreach (XDependencyProperty p in ps)
            {
                if (string.Compare(p.Name, name, true) == 0)
                {
                    return false;
                }
            }
            throw new ArgumentOutOfRangeException(name);
            //return false;
        }

        [NonSerialized()]
        private float _TabWidth = 100f;
        /// <summary>
        /// 制表宽度
        /// </summary>
        public float TabWidth
        {
            get
            {
                return _TabWidth;
            }
        }

        [NonSerialized()]
        private float _DefaultLineHeight = 0f;
        /// <summary>
        /// 默认行高
        /// </summary>
        public float DefaultLineHeight
        {
            get
            {
                return _DefaultLineHeight;
            }
        }

        private float _FontHeight = 0f;
        /// <summary>
        /// 字体高度
        /// </summary>
        public float FontHeight
        {
            get
            {
                return _FontHeight;
            }
        }

        /// <summary>
        /// 更新状态
        /// </summary>
        /// <param name="g">图形绘制对象</param>
        public void UpdateState(DCGraphics g)
        {
            if (g == null)
            {
                throw new ArgumentNullException("g");
            }

            XFontValue font = null;
            font = this.MyRuntimeStyle.Font;// this.Font.Value;
            var size = CharacterMeasurer.MeasureString(
                g,
                str_____,
                font,
                1000,
                StringFormat.GenericTypographic);
            this._FontHeight = g.GetFontHeight(font);
            if (this._MyRuntimeStyle != null)
            {
                this._MyRuntimeStyle.SetFontHeight(this._FontHeight);
            }
            // 计算默认制表宽度
            this._TabWidth = (int)Math.Ceiling(size.Width);
            // 计算默认行高
            //XFontValue f = this.Font;// new XFontValue(XFontValue.DefaultFontName, this.FontSize);
            this._DefaultLineHeight = this._FontHeight + 4;
            float h2 = this._FontHeight + 4;
            if (this._DefaultLineHeight != h2)
            {
            }
        }

        private static readonly string str_____ = "____";
        internal DocumentContentStyle CloneWithoutBorder()
        {
            DocumentContentStyle style = (DocumentContentStyle)this.Clone();
            style.BorderLeft = false;
            style.BorderTop = false;
            style.BorderRight = false;
            style.BorderBottom = false;
            return style;
        }

        public static void VoidMethod2()
        {

        }
        internal Dictionary<XDependencyProperty, object> GetInnerValues()
        {
            return base._InnerValues;
        }
    }
}