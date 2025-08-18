using System;
using System.Collections.Generic;
using System.Text;
using DCSoft.Writer.Dom;
using DCSoft.Writer.Controls;

namespace DCSoft.Writer
{
    /// <summary>
    /// 编辑文档元素属性的操作模式
    /// </summary>
    public enum ElementPropertiesEditMethod
    {
        /// <summary>
        /// 新增元素时编辑文档元素属性
        /// </summary>
        Insert,
        /// <summary>
        /// 编辑元素时编辑文档元素属性
        /// </summary>
        Edit
    }
}
