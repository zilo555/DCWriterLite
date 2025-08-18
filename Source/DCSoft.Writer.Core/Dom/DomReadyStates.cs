using System;
using System.Collections.Generic;
using System.Text;

namespace DCSoft.Writer.Dom
{
    /// <summary>
    /// 元素加载状态
    /// </summary>
    public enum DomReadyStates
    {
        /// <summary>
        /// 元素尚未初始化
        /// </summary>
        UnInitialized,
        /// <summary>
        /// 正在加载
        /// </summary>
        Loading,
        /// <summary>
        /// 数据加载完毕
        /// </summary>
        Loaded,
        /// <summary>
        /// 完全的加载完毕。
        /// </summary>
        Complete
    }
}
