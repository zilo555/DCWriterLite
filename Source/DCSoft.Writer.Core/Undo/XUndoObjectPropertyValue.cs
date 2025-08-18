using System;

namespace DCSoft.Writer.Undo
{
	/// <summary>
	/// 设置对象属性撤销/重做对象
	/// </summary>
    public class XUndoObjectPropertyValue : IUndoable
	{
		/// <summary>
		/// 初始化对象
		/// </summary>
		public XUndoObjectPropertyValue()
		{
		}

		public void Dispose()
		{
			this.myProperty = null;
			this.objNewValue = null;
			this.objOldValue = null;
			this.objOldValue = null;
		}
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
		/// 属性信息
		/// </summary>
		protected System.Reflection.PropertyInfo myProperty = null;
		/// <summary>
		/// 属性信息
		/// </summary>
		public System.Reflection.PropertyInfo Property
		{
			get{ return myProperty ;}
			set{ myProperty = value;}
		}

		/// <summary>
		/// 对象实例
		/// </summary>
		protected object objObjectValue = null;
		/// <summary>
		/// 对象实例
		/// </summary>
		public object ObjectValue
		{
			get{ return objObjectValue ;}
			set{ objObjectValue = value;}
		}

		/// <summary>
		/// 旧数据
		/// </summary>
		protected object objOldValue = null;
		/// <summary>
		/// 旧数据
		/// </summary>
		public object OldValue
		{
			get{ return objOldValue ;}
			set{ objOldValue = value;}
		}

		/// <summary>
		/// 新数据
		/// </summary>
		protected object objNewValue = null;
		/// <summary>
		/// 新数据
		/// </summary>
		public object NewValue
		{
			get{ return objNewValue ;}
			set{ objNewValue = value;}
		}
		/// <summary>
		/// 重复操作
		/// </summary>
		public virtual void Redo( XUndoEventArgs args )
		{
			if( objObjectValue != null && myProperty != null )
			{
				myProperty.SetValue( objObjectValue , objNewValue , null );
			}
		}

		/// <summary>
		/// 撤销操作
		/// </summary>
		public virtual void Undo( XUndoEventArgs args )
		{
			if( objObjectValue != null && myProperty != null )
			{
				myProperty.SetValue( objObjectValue , objOldValue , null );
			}
		}
	}//public class XUndoObjectPropertyValue : IUndoable
}