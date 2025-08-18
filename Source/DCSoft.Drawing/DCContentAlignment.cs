using System;
using System.Collections.Generic;
using System.Text;
using DCSoft.Common;

namespace DCSoft.Drawing
{
    /// <summary>
    /// 内容对齐方式
    /// </summary>
    /// <remarks>
    /// 本类型的数值和 ContentAlignment一样。
    /// </remarks>
    [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true )]
    [System.Text.Json.Serialization.JsonConverter(typeof(System.Text.Json.Serialization.JsonStringEnumConverter))]
    public enum DCContentAlignment
    {
        /// <summary>
        /// 顶端左对齐
        /// </summary>
        TopLeft = 1,
        /// <summary>
        /// 顶端居中对齐
        /// </summary>
        TopCenter = 2,
        /// <summary>
        /// 顶端右对齐
        /// </summary>
        TopRight = 4,
        /// <summary>
        /// 水平居中左对齐
        /// </summary>
        MiddleLeft = 16,
        /// <summary>
        /// 居中对齐
        /// </summary>
        MiddleCenter = 32,
        /// <summary>
        /// 水平居中右对齐
        /// </summary>
        MiddleRight = 64,
        /// <summary>
        /// 低端左对齐
        /// </summary>
        BottomLeft = 256,
        /// <summary>
        /// 低端居中对齐
        /// </summary>
        BottomCenter = 512,
        /// <summary>
        /// 低端右对齐
        /// </summary>
        BottomRight = 1024,
    }    
}
