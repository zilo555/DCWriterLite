using System;
using System.Collections.Generic;
using System.Text;
using System.Collections ;
using System.Reflection ;

// 袁永福到此一游

namespace DCSoft.Common
{
    /// <summary>
    /// 树状节点枚举器
    /// </summary>
    /// <remarks>
    /// 本类型遍历树状结构的所有的节点，能使用foreach()结构来遍历树状结构节点。
    /// 袁永福到此一游
    /// </remarks>
    public class TreeNodeEnumerable : IEnumerable ,IDisposable
    {
        /// <summary>
        /// 初始化对象
        /// </summary>
        protected TreeNodeEnumerable()
        {
        }
         
        /// <summary>
        /// 初始化对象,参数为根节点列表和子节点属性名
        /// </summary>
        /// <param name="rootNode">根节点</param>
        /// <param name="setStartNode">是否设置起始节点</param>
        protected TreeNodeEnumerable(object rootNode, bool setStartNode = false)
        {
            if (rootNode == null)
            {
                throw new ArgumentNullException("rootNode");
            }
            if (setStartNode)
            {
                this._StartNode = rootNode;
            }
            else
            {
                this._RootNodes = new object[]{ rootNode };
            }
        }

        /// <summary>
        /// 初始化对象,参数为根节点列表和子节点属性名
        /// </summary>
        /// <param name="rootNodes">根节点列表</param>
        protected TreeNodeEnumerable(IList rootNodes, bool setStartNodes = false)
        {
            if (rootNodes == null)
            {
                throw new ArgumentNullException("rootNode");
            }
            if (setStartNodes)
            {
                this._StartNodes = rootNodes;
            }
            else
            {
                this._RootNodes = rootNodes;
            }
        }

        private bool _CloneListMode = false;
        /// <summary>
        /// 复制列表模式。
        /// </summary>
        /// <remarks>
        /// 当循环遍历子元素时会修改子元素列表，导致遍历失败，此时使用复制列表模式可以避免这种情况。
        /// 但会降低遍历速度。
        /// </remarks>
        public bool CloneListMode
        {
            get
            {
                return _CloneListMode;
            }
            set
            {
                _CloneListMode = value;
            }
        }

        /// <summary>
        /// 清空所有的数据。准备销毁对象
        /// </summary>
        public virtual void Clear()
        {
            this._RootNodes = null;
            this._RootParent = null;
            this._StartNode = null;
            if (this._LastEnumerator != null)
            {
                this._LastEnumerator.Clear();
                this._LastEnumerator = null;
            }
            this._StartNodes = null;
        }

        protected object _RootParent = null;
        ///// <summary>
        ///// 根节点的父对象
        ///// </summary>
        //[System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true )]
        //protected object RootParent
        //{
        //    get { return _RootParent; }
        //    set { _RootParent = value; }
        //}

        private int _LastPosition = 0;
        /// <summary>
        /// 最后一次的移动到的位置
        /// </summary>
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public int LastPosition
        {
            get
            {
                return _LastPosition;
            }
        }
         
        /// <summary>
        /// 获得子节点列表
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public virtual IEnumerable GetChildNodes(object instance)
        {
            return null;
        }


        /// <summary>
        /// 获得父节点
        /// </summary>
        /// <param name="childNode"></param>
        /// <returns></returns>
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public virtual object GetParent(object childNode)
        {
            return null;
        }
         
        /// <summary>
        /// 判断是否是对外发布的节点对象
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        //[System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public virtual bool IsPublish(object instance)
        {
            return true ;
        }

        /// <summary>
        /// 枚举开始的节点对象
        /// </summary>
        private object _StartNode = null;
        /// <summary>
        /// 枚举开始的当前节点列表
        /// </summary>
        private IList _StartNodes = null;
        /// <summary>
        /// 根节点列表
        /// </summary>
        protected IEnumerable _RootNodes = null;

        //[System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        //protected IEnumerable RootNodes
        //{
        //    get { return _RootNodes; }
        //    set { _RootNodes = value; }
        //}
         
        /// <summary>
        /// 忽略掉当前子节点，立即退出当前层次的循环。
        /// </summary>
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public void CancelChild()
        {
            if (_LastEnumerator != null)
            {
                _LastEnumerator.CancelChild();
            }
        }
        /// <summary>
        /// 当前对象的父节点对象
        /// </summary>
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public object CurrentParent
        {
            get
            {
                if (this._LastEnumerator != null)
                {
                    return this._LastEnumerator.CurrentParent;
                }
                return null;
            }
        }

        private MyTreeNodeEnumerator _LastEnumerator = null;
        /// <summary>
        /// 获得枚举器
        /// </summary>
        /// <returns></returns>
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public IEnumerator GetEnumerator()
        {
            MyTreeNodeEnumerator enumer = new MyTreeNodeEnumerator( this );
            this._LastEnumerator = enumer;
            this._LastPosition = 0;
            return enumer;
        }

        /// <summary>
        /// 树状结构遍历器
        /// </summary>
        private class MyTreeNodeEnumerator : IEnumerator 
        {
            public MyTreeNodeEnumerator(TreeNodeEnumerable parent)
            {
                if (parent == null)
                {
                    throw new ArgumentNullException("parent");
                }
                this._Owner = parent;
                this.Reset();
            }
            public void Clear()
            {
                this._Owner = null;
                if (this._stack != null)
                {
                    this._stack.Clear();
                }
            }
            /// <summary>
            /// 重置状态
            /// </summary>
            public void Reset()
            {
                this._Owner._LastPosition = 0;
                this._stack.Clear();
                if (this._Owner._RootNodes != null)
                {
                    // 正常枚举
                    this._stack.Push( new MyEnumeratorItem( 
                        this._Owner._RootParent , 
                        this._Owner._RootNodes.GetEnumerator(),
                        this._Owner.CloneListMode ));
                    this._First = true;
                }
                else if (this._Owner._StartNode != null)
                {
                    // 中途开始枚举,创立枚举状态
                    // 首先获得上级节点列表
                    ArrayList list = new ArrayList();
                    object p = this._Owner._StartNode;
                    while (p != null)
                    {
                        list.Insert(0, p);
                        p = this._Owner.GetParent( p );// ._GetParentHandler(p);
                        if (p != null && list.Contains(p))
                        {
                            // 出现循环引用，退出
                            break;
                        }
                    }
                    this.ResetByStartNodes(list);
                }
                else if (this._Owner._StartNodes != null && this._Owner._StartNodes.Count > 0)
                {
                    // 中途开始枚举
                    ResetByStartNodes(this._Owner._StartNodes);
                }
            }

            //private ArrayList _IgnoreMoveNextOnce = new ArrayList();

            private void ResetByStartNodes(IList startNodes)
            {
                // 中途开始枚举,创立枚举状态
                this._Owner._LastPosition = 0;
                this._First = false;
                this._stack.Clear();
                this._stack.Push( new MyEnumeratorItem(
                    null , 
                    new object[] { startNodes[0] }.GetEnumerator() ,
                    this._Owner.CloneListMode));
                this._stack.Peek().Enumerator.MoveNext();
                for (int iCount = 0; iCount < startNodes.Count - 1; iCount++)
                {
                    object p = startNodes[iCount];
                    IEnumerable childEnums = this._Owner.GetChildNodes( p );// ._GetChildNodeHandler(p);
                    if (childEnums == null)
                    {
                        throw new Exception("TreeNodeEnumerable:this._Parent._GetChildNodeHandler(p)==null");
                    }
                    object nextP = startNodes[iCount + 1];
                    IEnumerator enumer = childEnums.GetEnumerator();
                    this._stack.Push( new MyEnumeratorItem( p , enumer , this._Owner.CloneListMode));
                    //enumer.Reset();
                    //enumer.MoveNext();
                    bool match = false;
                    int maxCount = 1000000;
                    while (enumer.MoveNext( ) )
                    {
                        if (enumer.Current == nextP)
                        {
                            this._IgnoreMoveNextOnce = enumer;
                            //_IgnoreMoveNextOnce.Add(enumer);
                            match = true;
                            break;
                        }
                        //if (enumer.MoveNext() == false)
                        //{
                        //    break;
                        //}
                        if (maxCount-- < 0)
                        {
                            // 设置最大循环次数，避免死循环
                            throw new Exception("TreeNodeEnumerable:可能死循环");
                        }
                    }//while
                    if (match == false)
                    {
                        // 未命中
                        throw new Exception("TreeNodeEnumerable:" + childEnums.GetType().FullName + "，没找到指定子节点");
                    }
                }//for
            }

            /// <summary>
            /// 第一次遍历标记
            /// </summary>
            private bool _First = true;

            private TreeNodeEnumerable _Owner = null;

            private class MyEnumeratorItem
            {
                public MyEnumeratorItem(object instance, IEnumerator enumer , bool cloneListMode )
                {
                    this.Instance = instance;
                    if (cloneListMode)
                    {
                        // 复制列表模式
                        System.Collections.ArrayList list = new ArrayList();
                        while( enumer.MoveNext ())
                        {
                            list.Add(enumer.Current);
                        }
                        this.Enumerator = list.GetEnumerator();
                    }
                    else
                    {
                        this.Enumerator = enumer;
                    }
                }
                public readonly  object Instance = null;
                public readonly  IEnumerator Enumerator = null;
            }

            private readonly DCStack<MyEnumeratorItem> _stack = new DCStack<MyEnumeratorItem>();
            /// <summary>
            /// 获得当前对象
            /// </summary>
            public object Current
            {
                get
                {
                    if (_First)
                    {
                        return null;
                    }
                    if (this._stack.Count > 0)
                    {
                        IEnumerator info = this._stack.Peek().Enumerator;
                        return info.Current;
                    }
                    return null;
                }
            }

            public object CurrentParent
            {
                get
                {
                    if (this._stack.Count > 0)
                    {
                        return this._stack.Peek().Instance;
                    }
                    return null;
                }
            }

            private IEnumerator _IgnoreMoveNextOnce = null;

            /// <summary>
            /// 移动到下一个对象
            /// </summary>
            /// <returns></returns>
            public bool MoveNext()
            {
                this._Owner._LastEnumerator = this;
                while (InnerMoveNext())
                {
                    object obj = this.Current;
                    if (this._Owner.IsPublish(obj))//._IsPublishCurrentHandler(obj) )
                    {
                        // 允许发布
                        this._Owner._LastPosition++;
                        return true;
                    }
                }
                return false;
            }

            private bool _CancelChild = false;
            /// <summary>
            /// 忽略当前节点的子节点列表
            /// </summary>
            public void CancelChild()
            {
                _CancelChild = true;
            }
            
            ///// <summary>
            ///// 忽略当前层次的节点。
            ///// </summary>
            //public void CancelCurrentLevel()
            //{
            //    if (this._stack.Count > 0)
            //    {
            //        MyEnumeratorItem top = this._stack.Pop();
            //        if (this._stack.Count > 0)
            //        {
            //            top = this._stack.Peek();
            //            if (top != null)
            //            {
            //                top.Enumerator.MoveNext();
            //            }
            //        }
            //    }
            //}
             

            private bool InnerMoveNext()
            {
                if (this._stack.Count > 0)
                {
                    IEnumerator top = this._stack.Peek().Enumerator;
                    if( this._First )
                    {
                        this._First = false ;
                        return top.MoveNext();
                    }
                    if (top != null && top.Current != null)
                    {
                        // 获得子节点遍历器
                        MyEnumeratorItem childInfo = null;
                        if (this._CancelChild)
                        {
                            // 忽略当前节点的子节点
                            this._CancelChild = false;
                        }
                        else
                        {
                            // 调用委托对象来获得子节点枚举器
                            IEnumerable en2 = this._Owner.GetChildNodes(top.Current);
                            if (en2 != null)
                            {
                                childInfo = new MyEnumeratorItem( top.Current, en2.GetEnumerator() , this._Owner.CloneListMode);
                            }
                        }
                        if (childInfo != null)
                        {
                            this._stack.Push(childInfo);
                        }
                    }
                    while (this._stack.Count > 0)
                    {
                        IEnumerator top2 = this._stack.Peek().Enumerator;
                        if (this._IgnoreMoveNextOnce == top2 )
                        {
                            this._IgnoreMoveNextOnce = null;
                            return true;
                        }
                        else
                        {
                            if (top2.MoveNext())
                            {
                                return true;
                            }
                        }
                        this._stack.Pop();
                    }//while
                }
                return false;
            }
        }
 
        /// <summary>
        /// 销毁对象
        /// </summary>
        public void Dispose()
        {
            this.Clear();
        }
       
    }
}
