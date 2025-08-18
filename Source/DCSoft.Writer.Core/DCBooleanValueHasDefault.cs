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
    public enum DCBooleanValueHasDefault
    {
        /// <summary>
        /// 是
        /// </summary>
        //[DCDescription(typeof(DCBooleanValueHasDefault), "True")]
        True = 0,
        /// <summary>
        /// 否
        /// </summary>
        //[DCDescription(typeof(DCBooleanValueHasDefault), "False")]
        False,
        /// <summary>
        /// 默认
        /// </summary>
        //[DCDescription(typeof(DCBooleanValueHasDefault), "Default")]
        Default
    }
}
