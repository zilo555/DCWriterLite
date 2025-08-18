using DCSoft.Common;

namespace DCSoft.Drawing
{
    /// <summary>
    /// 样式对象列表
    /// </summary>
#if !RELEASE
    [System.Diagnostics.DebuggerDisplay("Count={ Count }")]
    [System.Diagnostics.DebuggerTypeProxy(typeof(ListDebugView))]
#endif
    public partial class ContentStyleList : DCSoft.Common.DCList<ContentStyle>
    {         
        public ContentStyle SafeGet(int index, ContentStyle defaultValue)
        {
            if (index >= 0 && index < this._size)
            {
                return this._items[index];
            }
            else
            {
                return defaultValue;
            }
        }
        /// <summary>
        /// 设置数值锁定状态
        /// </summary>
        /// <param name="vLock"></param>
        public void SetValueLocked( bool vLock )
        {
            //return;// 暂时禁止
            //foreach (ContentStyle style in this)
            //{
            //    style.ValueLocked = vLock;
            //}
        }

        /// <summary>
        /// 项目在列表中的序号
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public int IndexOfExt(ContentStyle item)
        {
            int index = this.IndexOfByReference(item);// this.IndexOf(item);
            if (index >= 0)
            {
                return index;
            }
            int len = this.Count;
            for (int iCount = 0; iCount < len; iCount++)
            {
                if (((ContentStyle)this[iCount]).EqualsStyleValue(item))
                {
                    return iCount;
                }
            }
            return -1;
        }

        /// <summary>
        /// 更新项目列表的编号属性值
        /// </summary>
        public void UpdateStyleIndex()
        {
            int len = this.Count;
            for (int iCount = 0; iCount < len; iCount++)
            {
                this[iCount].Index = iCount;
            }
        }

        /// <summary>
        /// 修复字体名称
        /// </summary>
        public void FixFontName()
        {
            foreach ( ContentStyle style in this.FastForEach() )
            {
                var fontName = style.FontName;
                if (fontName != XFontValue.DefaultFontName)
                {
                    var newFontName = XFontValue.FixFontName(fontName, false);
                    if(newFontName != fontName )
                    {
                        // 修复错误的字体名称
                        style.FontName = newFontName;
                    }
                }
                //XFontValue f = style.Font;
                //if (f.FixFontName())
                //{
                //    // 修复错误的字体名称
                //    style.FontName = f.Name;
                //}
            }//foreach
        }

    }
}
