using System;
using System.Collections.Generic;
using System.Text;

namespace DCSoft.Writer
{
    /// <summary>
    /// 字体文件管理器基础类型
    /// </summary>
    public class FontFileManagerBase
    {
        /// <summary>
        /// 对象全局静态实例
        /// </summary>
        protected static FontFileManagerBase _Instance = new FontFileManagerBase();
        /// <summary>
        /// 对象全局静态实例
        /// </summary>
        public static FontFileManagerBase Instance
        {
            get
            {
                return _Instance;
            }
        }

        /// <summary>
        /// 获得指定字符使用的默认字体名称
        /// </summary>
        /// <param name="c">字符值</param>
        /// <returns>默认字体名称</returns>
        public virtual string GetDefaultFontName(char c)
        {
            return null;
        }
#if ! LightWeight
        /// <summary>
        /// 设置默认字体
        /// </summary>
        /// <param name="startChar">字符开始编号</param>
        /// <param name="endChar">字符结束编号</param>
        /// <param name="fontName">字体名称</param>
        public virtual bool SetDefaultFont(char startChar, char endChar, string fontName)
        {
            return false;
        }
#endif
    }
}
