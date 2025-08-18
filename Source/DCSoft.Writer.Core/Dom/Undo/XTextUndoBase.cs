using System;
using DCSoft.Writer.Undo;
namespace DCSoft.Writer.Dom.Undo
{
	/// <summary>
	/// 文档撤销操作对象基础类型
	/// </summary>
    public class XTextUndoBase : IUndoable
	{
		/// <summary>
		/// 初始化对象
		/// </summary>
		public XTextUndoBase()
		{
		}
		public virtual void Dispose()
		{
			this.myDocument = null;
		}
		/// <summary>
		/// 文档对象的撤销操作列表
		/// </summary>
		public XTextDocumentUndoList OwnerList
		{
			get
            {
                return myDocument.UndoList ;
            }
		}

        //private XTextDocumentUndoGroup _OwnerGroup = null;

        //internal XTextDocumentUndoGroup OwnerGroup
        //{
        //    get { return _OwnerGroup; }
        //    set { _OwnerGroup = value; }
        //}
        

		/// <summary>
		/// 对象所属的文档对象
		/// </summary>
		protected DomDocument myDocument = null;
		/// <summary>
		/// 对象所属的文档对象
		/// </summary>
		public DomDocument Document
		{
			get{ return myDocument ;}
			set{ myDocument = value;}
		}
		/// <summary>
		/// 对象名称
		/// </summary>
		public virtual string Name
		{
			get{ return null;}
			//set{ ; }
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
		/// 执行撤销操作
		/// </summary>
		public virtual void Undo( XUndoEventArgs args ){}
		
		/// <summary>
		/// 执行重复操作
		/// </summary>
		public virtual void Redo(XUndoEventArgs args ){}
	}
}