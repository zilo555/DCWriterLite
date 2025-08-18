using System;
using System.Collections.Generic;
using System.Text;
using DCSoft.Common;
using System.ComponentModel;
using System.Runtime.InteropServices ;
namespace DCSoft.Drawing
{
    /// <summary>
    /// 文档样式容器
    /// </summary>
    /// <remarks>编写 袁永福</remarks>
    public partial class ContentStyleContainer :
        IDisposable
    {
        /// <summary>
        /// 初始化对象
        /// </summary>
        public ContentStyleContainer()
        {
            _Default = CreateStyleInstance();
        }


        protected ContentStyle _Default = null;
        /// <summary>
        /// 默认样式
        /// </summary>
        public virtual ContentStyle Default
        {
            get
            {
                return _Default;
            }
            set
            {
                _Default = value;
                this.ClearRuntimeStyleList();
            }
        }

        protected ContentStyleList _Styles = new ContentStyleList();
        /// <summary>
        /// 样式列表
        /// </summary>
        /// <remarks>在此声明本属性不能进行XML序列化，为得是能在应用中重载本属性，然后添加
        /// 扩展的样式成员类型，这样就不会造成由于样式对象重载而导致的XML序列化的不方便。郁闷啊。</remarks>
        public virtual ContentStyleList Styles
        {
            get
            {
                return _Styles;
            }
            set
            {
                _Styles = value;
                this.ClearRuntimeStyleList();
            }
        }

        public void HandleAfterLoad()
        {
            ContentStyle ds = this.CreateStyleInstance();
            ds.FontSize = 9;
            ds.FontName = ContentStyle.DefaultFontName;
            foreach (ContentStyle style in this.Styles.FastForEach())
            {
                XDependencyObject.CopyValueFast(this.Default, style, false);
                if (XDependencyObject.HasPropertyValue(style, _FontSize) == false)
                {
                    style.FontSize = 9;
                }
            }
            if (_AutoFixFontNameForHandleAfterLoad)
            {
                this.Styles.FixFontName();
            }
            this.RefreshIndexs();
        }

        public static bool _AutoFixFontNameForHandleAfterLoad = false;

        private static readonly string _FontSize = "FontSize";
         
        public void RefreshIndexs()
        {
            this._Default.Index = -1;
            for (int iCount = 0; iCount < this.Styles.Count; iCount++)
            {
                this.Styles[iCount].Index = iCount;
            }
        }

        /// <summary>
        /// 获得指定编号的样式对象
        /// </summary>
        /// <param name="styleIndex">样式编号</param>
        /// <returns>获得的样式对象</returns>
        public ContentStyle GetStyle(int styleIndex)
        {
            return this._Styles.SafeGet(styleIndex, this._Default);
        }

        
        /// <summary>
        /// 获得样式在列表中的编号
        /// </summary>
        /// <param name="style">样式</param>
        /// <returns>获得的编号</returns>
        public int GetStyleIndex(ContentStyle style)
        {
            if ( style == null || XDependencyObject.GetValueCount(style) == 0)
            {
                // 没有任何设置，就使用默认设置，返回-1
                return -1;
            }
            if (style == _Default
                || _Default.EqualsStyleValue(style))
            {
                //style.SourceStyleIndex = -1;
                return -1;
            }
            else
            {
                int index = this.Styles.IndexOfExt(style);
                if (index < 0)
                {
                    var s2 = style.Clone();
                    if (s2.RemoveSameStyle(this.Default) > 0)
                    {
                        XDependencyObject.CopyValueFast(this.Default, s2, false);
                        index = this.Styles.IndexOfExt(s2);
                        if (index < 0)
                        {
                            this.Styles.Add(s2);
                            index = this.Styles.Count - 1;
                            this.Styles.UpdateStyleIndex();
                        }
                        ClearRuntimeStyleList();
                    }
                    else
                    {
                        //style.SourceStyleIndex = -1;
                        return -1;
                    }
                }
                //style.SourceStyleIndex = index;
                return index;
            }
        }

        
        /// <summary>
        /// 创建一个样式对象实例
        /// </summary>
        /// <returns>创建的对象</returns>
        public virtual ContentStyle CreateStyleInstance()
        {
            throw new NotSupportedException();
            //return new ContentStyle();
        }

        /// <summary>
        /// 清空对象
        /// </summary>
        public void Clear()
        {
            this._Default = this.CreateStyleInstance();
            this._Default.FontName = XFontValue.DefaultFontName;
            this._Default.FontSize = XFontValue.DefaultFontSize;
            //this._Current = (DocumentContentStyle)this._Default.Clone();
            this._Styles.Clear();
        }

        /// <summary>
        /// 清空运行时样式列表
        /// </summary>
        public virtual void ClearRuntimeStyleList()
        {
            if (this._Default != null)
            {
                this._Default.RuntimeStyle = null;
            }
            foreach (ContentStyle item in this.Styles)
            {
                item.RuntimeStyle = null;
            }
            //runtimeStyles = null;
        }

        /// <summary>
        /// 获得运行时的样式
        /// </summary>
        /// <param name="styleIndex"></param>
        /// <returns></returns>
        public ContentStyle GetRuntimeStyle(int styleIndex)
        {
            ContentStyle style = GetStyle(styleIndex);
            if ( style == null || style == _Default)
            {
                return _Default ;
            }
            else
            {
                return style;
            }
        }

        /// <summary>
        /// 深度复制对象
        /// </summary>
        /// <returns>复制品</returns>
        public ContentStyleContainer Clone()
        {
            ContentStyleContainer c = (ContentStyleContainer)this.MemberwiseClone();
            c._Default = (ContentStyle)this._Default.Clone();
            c._Styles = new ContentStyleList();
            foreach (ContentStyle item in this._Styles.FastForEach())
            {
                c._Styles.Add((ContentStyle)item.Clone());
            }
            c.ClearRuntimeStyleList();
            return c;
        }

       

        /// <summary>
        /// 销毁对象
        /// </summary>
        public virtual void Dispose()
        {
            if (this._Default != null)
            {
                this._Default.Dispose();
                this._Default = null;
            }
            if (this._Styles != null)
            {
                foreach (ContentStyle style in this._Styles)
                {
                    style.Dispose();
                }
                this._Styles = null;
            }
        }         
    }
}
