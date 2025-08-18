using System;
using DCSoft.Drawing;

namespace DCSoft.Writer.Dom
{
	/// <summary>
	/// 文档结束标记
	/// </summary>
    [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = false)]
    public abstract class DomEOFElement : DomElement
	{
		/// <summary>
		/// 初始化对象
		/// </summary>
		protected DomEOFElement()
		{
		}
        /// <summary>
        /// 初始化对象
        /// </summary>
        /// <param name="doc">对象所属文本</param>
        protected DomEOFElement( DomDocument doc )
		{
			this.OwnerDocument = doc ;
			//RefreshSize( null );
		}
         
	}
}