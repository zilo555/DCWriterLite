using System;
using System.Collections.Generic;
using System.Text;
using DCSoft.Common;
using System.ComponentModel;
namespace DCSoft.Writer
{
    /// <summary>
    /// 布尔状态值
    /// </summary>
    [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
    [System.Text.Json.Serialization.JsonConverter(typeof(System.Text.Json.Serialization.JsonStringEnumConverter))]
    public enum DCBooleanValue
    {
        /// <summary>
        /// 只读
        /// </summary>
        //[DCDescription(typeof(DCBooleanValue), "True")]
        True = 0,
        /// <summary>
        /// 不只读
        /// </summary>
        //[DCDescription(typeof(DCBooleanValue), "False")]
        False,
        /// <summary>
        /// 继承父节点
        /// </summary>
        //[DCDescription(typeof(DCBooleanValue), "Inherit")]
        Inherit
    }
}
