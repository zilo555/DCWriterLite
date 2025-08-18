using System;
//using DCSoft.Common;

namespace DCSoft.Drawing
{
	/// <summary>
	///文档内容对齐方式
	/// </summary>
     
    [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
    [System.Text.Json.Serialization.JsonConverter(typeof(System.Text.Json.Serialization.JsonStringEnumConverter))]
    public enum DocumentContentAlignment
	{
		/// <summary>
		/// 左对齐
		/// </summary>
		Left = 0 ,
		/// <summary>
		/// 居中对齐
		/// </summary>
		Center = 1 ,
		/// <summary>
		/// 右对齐
		/// </summary>
		Right = 2 , 
		/// <summary>
		/// 两边对齐
		/// </summary>
		Justify = 3 ,
        /// <summary>
        /// 分散对齐
        /// </summary>
        Distribute =4
	}//public enum DocumentContentAlignment
}