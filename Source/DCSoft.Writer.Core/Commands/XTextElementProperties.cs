using System;
using System.Collections.Generic;
using System.Text;
using DCSoft.Writer.Dom ;

namespace DCSoft.Writer.Commands
{
    /// <summary>
    /// 文档元素属性
    /// </summary>
    public abstract class XTextElementProperties
    {
        private DomDocument _Document = null;
        /// <summary>
        /// 文档对象
        /// </summary>
        public DomDocument Document
        {
            get { return _Document; }
            set { _Document = value; }
        }
        /// <summary>
        /// 根据内容设置创建元素
        /// </summary>
        /// <param name="document">文档对象</param>
        /// <returns>创建的元素</returns>
        public abstract DomElement CreateElement(DomDocument document);
        /// <summary>
        /// 读取元素的属性值到本对象中
        /// </summary>
        /// <param name="element">文档元素对象</param>
        /// <returns>操作是否成功</returns>
        public abstract bool ReadProperties(DomElement element);
        /// <summary>
        /// 根据对象数据设置文档元素
        /// </summary>
        /// <param name="document">文档对象</param>
        /// <param name="element">要处理的文档元素对象</param>
        /// <param name="logUndo">是否记录撤销操作信息</param>
        /// <returns>操作是否成功</returns>
        public abstract bool ApplyToElement(DomDocument document, DomElement element , bool logUndo );
    }
}
