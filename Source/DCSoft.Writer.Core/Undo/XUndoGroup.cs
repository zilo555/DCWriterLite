using System;
using System.Collections.Generic;

namespace DCSoft.Writer.Undo
{
	/// <summary>
	/// 撤销操作对象组类型,本类型可以包含多个撤销对象
	/// </summary>
    public class XUndoGroup : IUndoable
	{
		/// <summary>
		/// 初始化对象
		/// </summary>
		public XUndoGroup()
		{
		}
		public virtual void Dispose()
		{
			if (this.myItems != null)
			{
				foreach (var item in this.myItems)
				{
					item.Dispose();
				}
				this.myItems.Clear();
				this.myItems = null;
			}
		}

		/// <summary>
		/// 内置的撤销动作列表
		/// </summary>
		protected List<IUndoable> myItems = new List<IUndoable>();
//		/// <summary>
//		/// 内置的撤销动作列表
//		/// </summary>
//		public System.Collections.ArrayList Items
//		{
//			get{ return myItems ;}
//			set{ myItems = value;}
//		}

		private bool bolInGroup = false;
		/// <summary>
		/// 对象是否在一个批处理中
		/// </summary>
		public bool InGroup
		{
			get{ return bolInGroup ;}
			set{ bolInGroup = value;}
		}
		/// <summary>
		/// 向组添加一个撤销操作对象
		/// </summary>
		/// <param name="undo">撤销操作对象</param>
		public virtual void Add( IUndoable undo )
		{
			undo.InGroup = true ;
			myItems.Add( undo );
		}
		
		/// <summary>
		/// 执行撤销操作
		/// </summary>
		public virtual void Undo( XUndoEventArgs args )
		{
			for( int iCount = myItems.Count - 1 ; iCount >= 0 ; iCount -- )
			{
				IUndoable undo = ( IUndoable ) myItems[ iCount ] ;
				undo.InGroup = true ;
				undo.Undo( args );
				//( ( IUndoable )myItems[ iCount ]).Undo();
			}
		}

		/// <summary>
		/// 执行重复操作
		/// </summary>
		public virtual void Redo( XUndoEventArgs args )
		{
			foreach( IUndoable undo in myItems )
			{
				undo.InGroup = true ;
				undo.Redo( args );
			}
		}
	}//public class XUndoGroup : IUndoable
}