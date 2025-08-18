using System;
using System.Collections.Generic;
using System.Text;

namespace DCSoft.Writer
{
    /// <summary>
    /// 影响文档内容的方式
    /// </summary>
    [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
    //[DCSoft.Common.DCPublishAPI]
    public enum ContentEffects
    {
        /// <summary>
        /// 对文档内容没有任何修改，文档无需保存
        /// </summary>
        None = 0,
        /// <summary>
        /// 修改了文档内容，但还没有修改DOM结构。
        /// </summary>
        Content,
        ///// <summary>
        ///// 修改了文档DOM结构，但无需更新视图
        ///// </summary>
        //Dom,
        /// <summary>
        /// 只是影响到显示，不影响排版，重新绘制即可。
        /// </summary>
        Display ,
        /// <summary>
        /// 影响到文档的排版，需要重新排版重新绘制。
        /// </summary>
        Layout
    }
}
