using System;
using System.Collections.Generic;
using System.Text;
using DCSoft.Common;
using DCSoft.Drawing;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace DCSoft.Writer.Dom
{
    /// <summary>
    /// 文档样式容器
    /// </summary>
    public partial class DocumentContentStyleContainer :
        ContentStyleContainer
    {
        /// <summary>
        /// 初始化对象
        /// </summary>

        public DocumentContentStyleContainer()
        {
        }
        
        public override void ClearRuntimeStyleList()
        {
            if (base.Default != null)
            {
                ((DocumentContentStyle)base.Default).SetMyRuntimeStyle(null);
            }
            foreach (DocumentContentStyle style in this.Styles.FastForEach())
            {
                style.RuntimeStyle = null;
                style.SetMyRuntimeStyle(null);
            }
        }

        /// <summary>
        /// 获得运行时的样式
        /// </summary>
        /// <param name="styleIndex"></param>
        /// <returns></returns>
        public RuntimeDocumentContentStyle GetDCRuntimeStyle(int styleIndex)
        {
            var style = (DocumentContentStyle)this._Styles.SafeGet(styleIndex, this._Default);// GetStyle(styleIndex);
            if (style == null)
            {
                return ((DocumentContentStyle)this._Default).MyRuntimeStyle;
            }
            else
            {
                return style.MyRuntimeStyle;
            }
        }

        //[NonSerialized]
        //internal bool AllowResetFastValue = true;

        ///// <summary>
        ///// 重置某些需要快速获取的数值
        ///// </summary>
        //internal void ResetFastValue()
        //{
        //    if (this.AllowResetFastValue == false)
        //    {
        //        return;
        //    }
        //    if (this.Default != null)
        //    {
        //        ((DocumentContentStyle)this.Default).ResetFastValue();
        //    }
        //    foreach (ContentStyle style in this.Styles)
        //    {
        //        ((DocumentContentStyle)style).ResetFastValue();
        //        if (style.RuntimeStyle != null)
        //        {
        //            ((DocumentContentStyle)style).ResetFastValue();
        //        }
        //    }
        //}

        //private float[] _LetterSpacings = null;
        ///// <summary>
        ///// 快速的获取字符间距
        ///// </summary>
        ///// <param name="styleIndex">样式编号</param>
        ///// <returns>字符串间距值</returns>
        //internal float GetLetterSpacings(int styleIndex)
        //{
        //    if (_LetterSpacings == null)
        //    {
        //        _LetterSpacings = new float[this.Styles.Count + 1];
        //        _LetterSpacings[0] = this.Default.LetterSpacingFast;
        //        for (int iCount = 0; iCount < this.Styles.Count; iCount++)
        //        {
        //            _LetterSpacings[iCount + 1] = this.Styles[iCount].LetterSpacingFast;
        //        }
        //    }
        //    if (styleIndex >= -1 && styleIndex < this.Styles.Count - 1)
        //    {
        //        return _LetterSpacings[styleIndex + 1];
        //    }
        //    return 0;
        //}
        [NonSerialized]
        private DomDocument _Document = null;
        /// <summary>
        /// 对象所示文档对象
        /// </summary>
        internal DomDocument Document
        {
            get
            {
                return _Document;
            }
            set
            {
                _Document = value;
            }
        }

        /// <summary>
        /// 默认样式
        /// </summary>
        public override ContentStyle Default
        {
            get
            {
                return base._Default;
            }
            set
            {
                if (value != null && value.GetType().Equals(typeof(ContentStyle)))
                {
                    DocumentContentStyle style = new DocumentContentStyle();
                    XDependencyObject.CopyValueFast(value, style);
                    base.Default = style;
                }
                else
                {
                    base.Default = value;
                }
            }
        }

        /// <summary>
        /// 样式列表
        /// </summary>
        public override ContentStyleList Styles
        {
            get
            {
                return base.Styles;
            }
            set
            {
                base.Styles = value;
            }
        }
         
        /// <summary>
        /// 创建文档样式信息对象
        /// </summary>
        /// <returns>创建的信息对象</returns>
        public override ContentStyle CreateStyleInstance()
        {
            return new DocumentContentStyle();
        }


        /// <summary>
        /// 更新所有的样式对象的内部状态
        /// </summary>
        /// <param name="g"></param>
        public void UpdateState(DCGraphics g)
        {
            if (g == null)
            {
                throw new ArgumentNullException("g");
            }
            
            //if (this. != null)
            //{
            //    this.Current.UpdateState(g);
            //}
            this.ClearRuntimeStyleList();
            if (this.Default != null)
            {
                ((DocumentContentStyle)this.Default).UpdateState(g);
            }
            //foreach (DocumentContentStyle style in this.Styles)
            //{
            //    style.UpdateState(g);
            //}
            for (int iCount = 0; iCount < this.Styles.Count; iCount++)
            {
                var item = (DocumentContentStyle)this.Styles[iCount];
                item.UpdateState(g);
                DocumentContentStyle rs = ( DocumentContentStyle ) this.GetRuntimeStyle(iCount);
                if (rs != item)
                {
                    rs.UpdateState(g);
                }
            }
        }

        internal void CheckCreateRuntimeStyleInstnace()
        {
            foreach( DocumentContentStyle item in this.Styles )
            {
                var  v = item.MyRuntimeStyle;
            }
        }
    }
}
