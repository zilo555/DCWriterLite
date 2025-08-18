using System;
using System.Collections.Generic;
using System.Text;

namespace DCSoft.Writer.Dom
{
    /// <summary>
    /// 访问文档对象模型时的标记
    /// </summary>
    [Flags]
    [System.Text.Json.Serialization.JsonConverter(typeof(System.Text.Json.Serialization.JsonStringEnumConverter))]
    public enum DomAccessFlags
    {
        /// <summary>
        /// 没有任何标记
        /// </summary>
        None = 0 ,
        /// <summary>
        /// 检查控件是否只读
        /// </summary>
        CheckControlReadonly= 1 ,
        /// <summary>
        /// 是否检查用户可直接编辑设置
        /// </summary>
        CheckUserEditable = 2 ,
        /// <summary>
        /// 所有的标记
        /// </summary>
        Normal = CheckControlReadonly 
            | CheckUserEditable,
        /// <summary>
        /// 允许最大的值
        /// </summary>
        Max = 300
    }
}
