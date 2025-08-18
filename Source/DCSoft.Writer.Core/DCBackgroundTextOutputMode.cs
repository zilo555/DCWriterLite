using System;
using System.Collections.Generic;
using System.Text;

namespace DCSoft.Writer
{
    /// <summary>
    /// 背景文字输出模式
    /// </summary>
    [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
    [System.Text.Json.Serialization.JsonConverter(typeof(System.Text.Json.Serialization.JsonStringEnumConverter))]
    public enum DCBackgroundTextOutputMode
    {
        /// <summary>
        /// 不输出
        /// </summary>
        None,
        /// <summary>
        /// 输出空白字符
        /// </summary>
        Whitespace,
        /// <summary>
        /// 输出文本
        /// </summary>
        Output,
        /// <summary>
        /// 输出为下划线
        /// </summary>
        Underline
    }
}
