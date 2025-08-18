using System;
using System.Collections.Generic;
using System.Text;

namespace DCSoft.Writer
{
    /// <summary>
    /// 编辑器控件处理的数据格式功能标记
    /// </summary>
    /// <remarks>编制 袁永福</remarks>
    [Flags]
    [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
    [System.Text.Json.Serialization.JsonConverter(typeof(System.Text.Json.Serialization.JsonStringEnumConverter))]
    public enum WriterDataFormats
    {
        /// <summary>
        /// 无任何格式
        /// </summary>
        None = 0,
        /// <summary>
        /// 普通文本格式
        /// </summary>
        Text = 1,
        /// <summary>
        /// XML格式
        /// </summary>
        XML = 8,
        /// <summary>
        /// 图片格式
        /// </summary>
        Image = 32,
        /// <summary>
        /// 编辑器命令格式
        /// </summary>
        CommandFormat = 64,
        /// <summary>
        /// 支持所有标准格式
        /// </summary>
        All = 0xfff 
        ///// <summary>
        ///// 支持任意格式，包括标准格式和用户自定义格式
        ///// </summary>
        //GlobalAll= 0xffff
    }
}