using System;
using DCSoft.Writer.Controls ;
using DCSoft.Writer;
using DCSoft.Writer.Dom;
using System.Collections.Generic ;

namespace DCSoft.Writer.Commands
{
    /// <summary>
    /// 文本编辑器动作列表
    /// </summary>
#if !RELEASE
    [System.Diagnostics.DebuggerTypeProxy(typeof(DCSoft.Common.ListDebugView))]
#endif
    public class WriterCommandList : List<WriterCommand>
	{
         
         
		/// <summary>
		/// 获得指定名称的动作对象,名称比较不区分大小写
		/// </summary>
		public WriterCommand this[ string name ]
		{
			get
			{
                if (name == null)
                {
                    return null;
                }
                foreach (WriterCommand a in this)
                {
                    if (string.Compare(a.Name, name, true) == 0)
                    {
                        return a;
                    }
                }
				return null;
			}
		}
		/// <summary>
		/// 添加动作,添加前会删除列表中相同名称的动作
		/// </summary>
		/// <param name="a">动作对象</param>
		/// <returns>动作在列表中的需要</returns>
		public int AddCommand( WriterCommand a )
		{
			WriterCommand old = this[ a.Name ] ;
            if (old != null)
            {
                this.Remove(old);
            }
            //a.myDocument = this.Document;
         	base.Add( a );
            return this.Count - 1;
		}
    }
}