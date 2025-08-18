using System;
using System.Collections.Generic;
using System.Text;
using DCSoft.Common;

namespace DCSoft.Writer
{
    /// <summary>
    /// 可见性状态
    /// </summary>
    [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
    [System.Text.Json.Serialization.JsonConverter(typeof(System.Text.Json.Serialization.JsonStringEnumConverter))]
    public enum DCVisibleState
    {
        /// <summary>
        /// 可见
        /// </summary>
        Visible = 0,
        /// <summary>
        /// 隐藏的
        /// </summary>
        Hidden ,
        /// <summary>
        /// 默认
        /// </summary>
        Default,
        /// <summary>
        /// 一直可见
        /// </summary>
        AlwaysVisible
    }
}
