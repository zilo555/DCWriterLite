using System;
using DCSoft.Writer.Undo;
using DCSoft.Writer;
using DCSoft.Writer.Dom;

namespace DCSoft.Writer.Dom.Undo
{
	

    /// <summary>
    /// 重复/撤销设置元素的可见性
    /// </summary>
    internal class XTextUndoElementVisible : XTextUndoBase
    {
        ///// <summary>
        ///// 初始化对象
        ///// </summary>
        //public XTextUndoElementVisible()
        //{
        //}

        /// <summary>
        /// 初始化对象
        /// </summary>
        /// <param name="element">操作的文档元素</param>
        /// <param name="newVisible">旧的可见性</param>
        /// <param name="oldVisible">新的可见性</param>
        public XTextUndoElementVisible(DomElement element, bool oldVisible , bool newVisible )
        {
            _Element = element;
            _OldVisible = oldVisible;
            _NewVisible = newVisible;
        }

        public override void Dispose()
        {
            base.Dispose();
            this._Element = null;
        }
        private DomElement _Element = null;
        private bool _OldVisible = false;
        private bool _NewVisible = false;
 
        /// <summary>
        /// 撤销操作
        /// </summary>
        /// <param name="args">参数</param>
        public override void Undo(XUndoEventArgs args)
        {
            _Element.EditorSetVisibleExt(_OldVisible , false , false );
            this.OwnerList.RefreshElements.Add(_Element);
        }
        /// <summary>
        /// 重做
        /// </summary>
        /// <param name="args">参数</param>
        public override void Redo(XUndoEventArgs args)
        {
            _Element.EditorSetVisibleExt(_NewVisible, false, false);
            this.OwnerList.RefreshElements.Add(_Element);
        }
    }

   
	/// <summary>
	/// 重复/撤销设置元素属性的信息对象
	/// </summary>
    public class XTextUndoProperty : XTextUndoBase
	{
		/// <summary>
		/// 初始化对象
		/// </summary>
		public XTextUndoProperty()
		{
		}

       
		/// <summary>
		/// 初始化对象
		/// </summary>
		/// <param name="style">动作类型</param>
		/// <param name="vOldValue">旧数值</param>
		/// <param name="vNewValue">新数值</param>
		/// <param name="element">元素对象</param>
		public XTextUndoProperty( 
			XTextUndoStyles style , 
			object vOldValue , 
			object vNewValue , 
			DomElement element )
		{
			this.intStyle = style ;
			this.objOldValue = vOldValue ;
			this.objNewValue = vNewValue ;
			this.myElement = element ;
		}
		/// <summary>
		/// 旧数值
		/// </summary>
		protected object objOldValue = null;
		/// <summary>
		/// 旧数值
		/// </summary>
		public object OldValue
		{
			get{ return objOldValue ;}
			set{ objOldValue = value;}
		}

		/// <summary>
		/// 新数值
		/// </summary>
		protected object objNewValue = null;
		/// <summary>
		/// 新数值
		/// </summary>
		public object NewValue
		{
			get{ return objNewValue ;}
			set{ objNewValue = value;}
		}

		/// <summary>
		/// 元素对象
		/// </summary>
		protected DomElement myElement = null;
		/// <summary>
		/// 元素对象
		/// </summary>
		public DomElement Element
		{
			get{ return myElement ;}
			set{ myElement = value;}
		}

		/// <summary>
		/// 样式
		/// </summary>
		protected XTextUndoStyles intStyle = 0 ;
		/// <summary>
		/// 样式
		/// </summary>
		public XTextUndoStyles Style
		{
			get{ return intStyle ;}
			set{ intStyle = value;}
		}

		/// <summary>
		/// 动作执行标记,若执行了动作,使得元素状态发生改变,
		/// 则该属性为 true , 若执行动作没有产生任何修改则该属性为 false 
		/// </summary>
		protected bool bolExecuteFlag = false;
		/// <summary>
		/// 动作执行标记,若执行了动作,使得元素状态发生改变,
		/// 则该属性为 true , 若执行动作没有产生任何修改则该属性为 false 
		/// </summary>
		public bool ExecuteFlag
		{
			get
            {
                return bolExecuteFlag ;
            }
			set
            {
                bolExecuteFlag  = value;
            }
		}

		/// <summary>
		/// 执行撤销操作
		/// </summary>
        public override void Undo(DCSoft.Writer.Undo.XUndoEventArgs args)
		{
			Execute( true );
		}
		/// <summary>
		/// 执行重复操作
		/// </summary>
        public override void Redo(DCSoft.Writer.Undo.XUndoEventArgs args)
		{
			Execute( false );
		}

		/// <summary>
		/// 执行动作
		/// </summary>
		/// <param name="undo">是否是撤销操作 true:撤销操作 false:重复操作</param>
		/// <returns>操作是否成功</returns>
		public virtual void Execute( bool undo )
		{
			bolExecuteFlag = false;
            switch (intStyle)
            {
                case XTextUndoStyles.InnerValue:
                    {
                        var field = (DomInputFieldElementBase)myElement;
                        if(undo )
                        {
                            field.InnerValue = (string)this.OldValue;
                        }
                        else
                        {
                            field.InnerValue = (string)this.NewValue;
                        }
                        bolExecuteFlag = true;
                    }
                    break;
                case XTextUndoStyles.Size :
                    SizeF oldSize = (SizeF)this.OldValue;
                    SizeF newSize = (SizeF)this.NewValue;
                    if (oldSize.Width > 0 && oldSize.Height > 0
                        && newSize.Width > 0 && newSize.Height > 0)
                    {
                        if (undo)
                        {
                            myElement.EditorSetSize(oldSize.Width, oldSize.Height, true, false);
                        }
                        else
                        {
                            myElement.EditorSetSize(newSize.Width, newSize.Height, true, false);
                        }
                        myElement.SizeInvalid = false;
                        bolExecuteFlag = true;
                    }
                    break;
                case XTextUndoStyles.EditorSize:
                    // 设置元素大小
                    {
                        var OldSize = (SizeF)objOldValue;
                        var NewSize = (SizeF)objNewValue;
                        if (OldSize.Width > 0
                            && OldSize.Height > 0
                            && NewSize.Width > 0
                            && NewSize.Height > 0
                            && OldSize.Equals(NewSize) == false)
                        {
                            if (undo)
                            {
                                myElement.EditorSize = OldSize;
                            }
                            else
                            {
                                myElement.EditorSize = NewSize;
                            }
                            myElement.SizeInvalid = true;
                            bolExecuteFlag = true;
                        }
                    }
                    break;
                case XTextUndoStyles.TableRowSpecifyHeight:
                    {
                        // 设置表格行用户指定的高度
                        DomTableRowElement row = (DomTableRowElement)myElement;
                        float OldHeight = (float)objOldValue;
                        float NewHeight = (float)objNewValue;
                        if (OldHeight != NewHeight && row != null)
                        {
                            if (undo)
                            {
                                row.EditorSetRowSpecifyHeight(OldHeight, false);
                            }
                            else
                            {
                                row.EditorSetRowSpecifyHeight(NewHeight, false);
                            }
                        }
                    }
                    break;
            }
            if (bolExecuteFlag)
            {
                myElement.UpdateContentVersion();
                this.OwnerList.RefreshElements.Add(myElement.FirstContentElementInPublicContent);
                this.OwnerList.RefreshElements.Add(myElement.LastContentElementInPublicContent);
            }
		}
	}
}