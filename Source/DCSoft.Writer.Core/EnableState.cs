using System;
using System.Collections.Generic;
using System.Text;
using DCSoft.Common;

namespace DCSoft.Writer
{
    /// <summary>
    /// 可用状态
    /// </summary>
    [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
    [System.Text.Json.Serialization.JsonConverter(typeof(System.Text.Json.Serialization.JsonStringEnumConverter))]
    public enum EnableState
    {
        /// <summary>
        /// 默认状态
        /// </summary>
        Default ,
        /// <summary>
        /// 可用状态
        /// </summary>
        Enabled,
        /// <summary>
        /// 无效状态
        /// </summary>
        Disabled
    }
}
