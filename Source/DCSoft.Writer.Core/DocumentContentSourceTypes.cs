using System;
using System.Collections.Generic;
using System.Text;

namespace DCSoft.Writer
{
    /// <summary>
    /// 文档内容来源类型
    /// </summary>
    [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
    //[DCSoft.Common.DCPublishAPI]
    [System.Text.Json.Serialization.JsonConverter(typeof(System.Text.Json.Serialization.JsonStringEnumConverter))]
    public enum DocumentContentSourceTypes
    {
        /// <summary>
        /// 未知类型
        /// </summary>
        Unknown ,
        /// <summary>
        /// 无来源
        /// </summary>
        None ,
        /// <summary>
        /// 使用NewFile创建的文档
        /// </summary>
        NewFile ,
        /// <summary>
        /// 来自文本读取器
        /// </summary>
        TextReader,
        /// <summary>
        /// 来自文件流
        /// </summary>
        Stream,
        /// <summary>
        /// 来自文件
        /// </summary>
        File
    }
}
