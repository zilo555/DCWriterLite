using System;
//using DCSoft.Common;

namespace DCSoft.Drawing
{
    /// <summary>
    /// 垂直对齐方式
    /// </summary>
     
    [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true )]
    [System.Text.Json.Serialization.JsonConverter(typeof(System.Text.Json.Serialization.JsonStringEnumConverter))]
    public enum VerticalAlignStyle
    {
        /// <summary>
        /// 对齐到顶端
        /// </summary>
        Top = 0 ,
        /// <summary>
        /// 垂直居中对齐
        /// </summary>
        Middle = 1 ,
        /// <summary>
        /// 对齐到底端
        /// </summary>
        Bottom = 2 
        ///// <summary>
        ///// 垂直两边对齐
        ///// </summary>
        //Justify = 3
    }
}
