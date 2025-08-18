using System;
using DCSoft.Writer.Undo;
using System.Collections.Generic;
using System.Collections;
using DCSoft.Writer.Dom ;
using DCSoft.Writer.Controls ;

namespace DCSoft.Writer.Dom.Undo
{
	/// <summary>
	/// 撤销操作列表
	/// </summary>
    public class XTextDocumentUndoList : XUndoList
	{
        /// <summary>
        /// 初始化对象
        /// </summary>
        /// <param name="document"></param>
        public XTextDocumentUndoList(DomDocument document)
        {
            this.Document = document ;
        }

        public override void Dispose()
        {
            base.Dispose();
            this.Clear();
            this._ContentChangedContainer = null;
            this._ContentRefreshInfos = null;
            this.myRefreshElements = null;
        }
        /// <summary>
        /// 内容清空事件
        /// </summary>
        public override void Clear()
        {
            base.Clear();
            if (this._ContentChangedContainer != null)
            {
                this._ContentChangedContainer.Clear();

            }
            if (this._ContentRefreshInfos != null)
            {
                this._ContentRefreshInfos.Clear();
            }
            if (this.myRefreshElements != null)
            {
                this.myRefreshElements.Clear();
            }
        }

		/// <summary>
		/// 强制使用撤销对象组操作
		/// </summary>
		protected override bool ForceUseGroup()
		{
				return true ;
		}

		/// <summary>
		/// 创建一个撤销对象组对象
		/// </summary>
		/// <returns>创建的对象</returns>
		protected override XUndoGroup CreateGroup()
		{
			XTextDocumentUndoGroup group = new XTextDocumentUndoGroup( myDocument );
			group.OldSelectionStart = this.intOldSelectionStart ;
			group.OldSelectionLength = this.intOldSelectionLength ;
			group.NewSelectionStart = this.intNewSelectionStart ;
			group.NewSelectionLength = this.intNewSelectionLength ;
			return group ;
		}

        private UndoMethodTypes _NeedInvokeMethods = UndoMethodTypes.None;
        /// <summary>
        /// 执行撤销和重做操作后执行的方法
        /// </summary>
        //[System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public UndoMethodTypes NeedInvokeMethods
        {
            get
            {
                return _NeedInvokeMethods; 
            }
            set
            {
                _NeedInvokeMethods = value; 
            }
        }

        //private bool _NeedRefreshDocument = false;
        ///// <summary>
        ///// 是否需要刷新整个文档
        ///// </summary>
        //public bool NeedRefreshDocument
        //{
        //    get { return _NeedRefreshDocument; }
        //    set { _NeedRefreshDocument = value; }
        //}

		private DomElementList myRefreshElements = new DomElementList();
		/// <summary>
		/// 状态发生改变的元素列表
		/// </summary>
		internal DomElementList RefreshElements
		{
			get{ return myRefreshElements ;}
		}

        private DomElementList _ContentChangedContainer = new DomElementList();
        /// <summary>
        /// 内容发生改变的容器元素对象
        /// </summary>
        //[System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public DomElementList ContentChangedContainer
        {
            get { return _ContentChangedContainer; }
        }

        internal Dictionary<DomContentElement, int> _ContentRefreshInfos = new Dictionary<DomContentElement, int>();

        /// <summary>
        /// 添加刷新的区域信息
        /// </summary>
        /// <param name="contentElement"></param>
        /// <param name="startIndex"></param>
        //[System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public void AddContentRefreshInfo(DomContentElement contentElement, int startIndex)
        {
            if (_ContentRefreshInfos.ContainsKey(contentElement) )
            {
                _ContentRefreshInfos[contentElement] = Math.Min(_ContentRefreshInfos[contentElement], startIndex);
            }
            else
            {
                _ContentRefreshInfos[contentElement] = startIndex;
            }
        }

        //private XTextContentElement _EffectContentElement = null;
        ///// <summary>
        ///// 操作涉及到的文档元素对象
        ///// </summary>
        //public XTextContentElement EffectContentElement
        //{
        //    get
        //    {
        //        return _EffectContentElement; 
        //    }
        //    set
        //    {
        //        _EffectContentElement = value; 
        //    }
        //}

        //private int intRefreshStartIndex = int.MinValue  ;
        ///// <summary>
        ///// 文档重新刷新的开始序号
        ///// </summary>
        //internal int RefreshStartIndex 
        //{
        //    get
        //    {
        //        return intRefreshStartIndex ;
        //    }
        //    set
        //    {
        //        if (intRefreshStartIndex == int.MinValue)
        //        {
        //            intRefreshStartIndex = value;
        //        }
        //        else if (intRefreshStartIndex > value)
        //        {
        //            intRefreshStartIndex = value;
        //        }
        //    }
        //}
		/// <summary>
		/// 开始登记操作时的文档选择区域开始序号
		/// </summary>
		private int intOldSelectionStart = 0 ;
		/// <summary>
		/// 开始登记操作时的文档选择区域长度
		/// </summary>
		private int intOldSelectionLength = 0 ;
		/// <summary>
		/// 结束登记操作时的文档选择区域开始序号
		/// </summary>
		private int intNewSelectionStart = 0 ;
		/// <summary>
		/// 结束登记操作时的文档选择区域长度
		/// </summary>
		private int intNewSelectionLength = 0 ;
	
		/// <summary>
		/// 开始登记撤销信息
		/// </summary>
		/// <returns>操作是否成功</returns>
		public override bool BeginLog()
		{
			if( base.BeginLog())
			{
				myRefreshElements.Clear();
                intOldSelectionStart = this.Document.CurrentContentElement.Selection.StartIndex;
                intOldSelectionLength = this.Document.CurrentContentElement.Selection.Length;
				return true ;
			}
			return false ;
		}
        /// <summary>
        /// 取消登记撤销信息操作
        /// </summary>
        public override void CancelLog()
        {
            base.CancelLog();
            myRefreshElements.Clear();
        }

		/// <summary>
		/// 结束登记撤销信息
		/// </summary>
		public override bool EndLog()
		{
			myRefreshElements.Clear();
			//this.intRefreshStartIndex = int.MinValue ;
            if (this.Document != null
                && this.Document.CurrentContentElement != null
                && this.Document.CurrentContentElement.Selection != null)
            {
                intNewSelectionStart = this.Document.CurrentContentElement.Selection.StartIndex;
                intNewSelectionLength = this.Document.CurrentContentElement.Selection.Length;
            }
            else
            {
                intNewSelectionStart = 0;
                intNewSelectionLength = 0;
            }
            var logItems = base.LogItems();
            if (logItems != null && logItems.Count > 0)
            {
                foreach (object obj in logItems)
                {
                    if (obj is XTextUndoBase)
                    {
                        XTextUndoBase undo = (XTextUndoBase)obj;
                        undo.Document = this.Document ;
                    }
                }
            }
			return base.EndLog ();
		}

        //protected override void OnStateChanged()
        //{
        //    base.OnStateChanged();
        //    if( this.Document != null && this.Document
        //}

		/// <summary>
		/// 本对象所属的文档对象
		/// </summary>
		protected DomDocument myDocument = null;
		/// <summary>
		/// 本对象所属的文档对象
		/// </summary>
       //[System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public DomDocument Document
		{
			get
            {
                return myDocument ;
            }
			set
            {
                myDocument = value;
            }
		}
        /// <summary>
        /// 添加额外执行的方法
        /// </summary>
        /// <param name="method">方法类型</param>
       //[System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public void AddMethod(UndoMethodTypes method)
        {
            if (CanLog())
            {
                XTextUndoInvokeMethod undo = new XTextUndoInvokeMethod( method);
                this.Add(undo);
            }
        }
        /// <summary>
        /// 添加一个删除多个元素的撤销信息
        /// </summary>
        /// <param name="c">容器对象</param>
        /// <param name="index">删除区域开始编号</param>
        /// <param name="list">删除的元素</param>
       //[System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public void AddRemoveElements(
			DomContainerElement c ,
			int index ,
			DomElementList list )
		{
			if( CanLog())
			{
                XTextUndoReplaceElements undo = new XTextUndoReplaceElements(c, index, list, null);
                undo.Document = this.Document;
                undo.InGroup = true;
                this.Add(undo);

                //XTextUndoRemoveElements undo = new XTextUndoRemoveElements( );
                //undo.Document = this.myDocument ;
                //undo.Container = c ;
                //undo.Index = index ;
                //undo.Items.AddRange( list );
                //this.Add( undo );
			}
		}

        /// <summary>
        /// 添加一个替换多个元素的撤销信息
        /// </summary>
        /// <param name="container">容器对象</param>
        /// <param name="index">操作区域开始编号</param>
        /// <param name="oldElements">旧元素列表</param>
        /// <param name="newElements">新元素列表</param>
       //[System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public void AddReplaceElements(
            DomContainerElement container,
            int index, 
            DomElementList oldElements,
            DomElementList newElements)
        {
            if (this.CanLog())
            {
                XTextUndoReplaceElements undo = new XTextUndoReplaceElements(
                    container, 
                    index,
                    oldElements,
                    newElements);
                undo.Document = this.Document;
                undo.InGroup = true;
                this.Add(undo);
            }
        }

        /// <summary>
        /// 添加一个项目
        /// </summary>
        /// <param name="element">文档元素</param>
        /// <param name="oldStyleIndex">旧的样式编号</param>
        /// <param name="newStyleIndex">新的样式编号</param>
        //[System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public void AddStyleIndex(DomElement element, int oldStyleIndex, int newStyleIndex)
        {
            XTextUndoStyleIndex undo = new XTextUndoStyleIndex(element, oldStyleIndex, newStyleIndex);
            undo.Document = myDocument;
            undo.InGroup = true;
            this.Add(undo);
        }

        /// <summary>
        /// 添加一个项目
        /// </summary>
        /// <param name="element">文档元素</param>
        /// <param name="oldVisible">旧的可见性</param>
        /// <param name="newVisible">新的可见性</param>
      // //[System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public void AddVisible(DomElement element, bool oldVisible, bool newVisible)
        {
            XTextUndoElementVisible undo = new XTextUndoElementVisible(element, oldVisible , newVisible );
            undo.Document = myDocument;
            undo.InGroup = true;
            this.Add(undo);
        }

		/// <summary>
		/// 添加一个项目
		/// </summary>
		/// <param name="style">动作类型</param>
		/// <param name="vOldValue">旧数据</param>
		/// <param name="vNewValue">新数据</param>
		/// <param name="element">元素对象</param>
       //[System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public void AddProperty( 
			XTextUndoStyles style , 
			object vOldValue ,
			object vNewValue ,
			DomElement element )
		{
			XTextUndoProperty undo = new XTextUndoProperty(
                style ,
                vOldValue ,
                vNewValue ,
                element );
			undo.Document = myDocument ;
			undo.InGroup = true ;
			this.Add( undo );
		}

		/// <summary>
		/// 添加设置对象属性值的撤销信息
		/// </summary>
		/// <param name="PropertyName">属性名称,不区分大小写</param>
		/// <param name="OldValue">旧的属性值</param>
		/// <param name="NewValue">新的属性值</param>
		/// <param name="ObjectInstance">对象实例</param>
       //[System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public void AddProperty( 
			string PropertyName ,
			object OldValue , 
			object NewValue , 
			object ObjectInstance )
		{
            if (string.IsNullOrEmpty(PropertyName))
            {
                throw new System.ArgumentNullException("PropertyName");
            }
            if (ObjectInstance == null)
            {
                throw new System.ArgumentNullException("ObjectInstance");
            }
			System.Type t = ObjectInstance.GetType();
			System.Reflection.PropertyInfo p = t.GetProperty( 
				PropertyName , 
				System.Reflection.BindingFlags.IgnoreCase | 
				System.Reflection.BindingFlags.Instance | 
				System.Reflection.BindingFlags.Public );
            if (p == null)
            {
                throw new System.Exception(string.Format(DCSR.MissProperty_Name, t.FullName + "!" + PropertyName));
            }
            System.Reflection.ParameterInfo[] ps = p.GetIndexParameters();
            if (ps != null && ps.Length > 0)
            {
                throw new System.Exception(string.Format(DCSR.PropertyCannotHasParameter_Name, PropertyName));
            }
            if (p.CanWrite == false)
            {
                throw new System.Exception(string.Format(DCSR.PropertyIsReadonly_Name, PropertyName));
            }
			Type pt = p.PropertyType ;
			if( OldValue != null )
			{
				Type vt = OldValue.GetType();
                if (vt.Equals(pt) == false && vt.IsSubclassOf(pt) == false)
                {
                    throw new System.Exception("旧数据值类型不匹配");
                }
			}
			if( NewValue != null )
			{
				Type vt = NewValue.GetType();
				if( vt.Equals( pt ) == false && vt.IsSubclassOf( pt ) == false )
				{
					throw new System.Exception("新数值类型不匹配");
				}
			}

			XTextUndoNameProperty undo = new XTextUndoNameProperty( ObjectInstance , p , OldValue , NewValue );
			undo.Document = myDocument ;
			undo.InGroup = true ;
			this.Add( undo );
		}
        /// <summary>
        /// 添加设置表格行用户指定高度的操作
        /// </summary>
        /// <param name="row">表格行对象</param>
        /// <param name="oldSpecifyHeight">旧高度</param>
        //[System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public void AddRowSpecifyHeight(DomTableRowElement row, float oldSpecifyHeight)
        {
            if (row != null && oldSpecifyHeight != row.SpecifyHeight )
            {
                this.Add(new XTextUndoProperty(
                    XTextUndoStyles.TableRowSpecifyHeight,
                     oldSpecifyHeight,
                     row.SpecifyHeight,
                     row ));
            }
        }
	}


}