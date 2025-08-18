using System;
using System.Collections.Generic;
using System.Text;
using DCSoft.Writer.Dom ;
using DCSoft.Writer ;
using DCSoft.Common;

namespace DCSoft.Writer.Controls
{

    /// <summary>
    /// 收集内容保护信息事件参数
    /// </summary>     
    public partial class CollectProtectedElementsEventArgs : WriterEventArgs
    {
        /// <summary>
        /// 初始化对象
        /// </summary>
        /// <param name="ctl">控件对象</param>
        /// <param name="document">文档对象</param>
        /// <param name="rootElements">根节点列表</param>
        /// <param name="infos">信息列表对象</param>
        public CollectProtectedElementsEventArgs(
            WriterControl ctl , 
            DomDocument document , 
            DomElementList rootElements,
            ContentProtectedInfoList infos )
            : base( ctl , document , null )
        {
            this._RootElements = rootElements;
            this._Infos = infos;
        }

        private DomElementList _RootElements = null;
        /// <summary>
        /// 文档元素列表
        /// </summary>
        public DomElementList RootElements
        {
            get
            {
                if (_RootElements == null)
                {
                    this._RootElements = new DomElementList();
                }
                return _RootElements; 
            }
        }

        private ContentProtectedInfoList _Infos = null;
        /// <summary>
        /// 内容保护信息列表
        /// </summary>
        public ContentProtectedInfoList Infos
        {
            get
            {
                if (_Infos == null)
                {
                    _Infos = new ContentProtectedInfoList();
                }
                return _Infos; 
            }
            set
            {
                _Infos = value;
            }
        }

        private int _LimitedCount = 100;
        /// <summary>
        /// 信息个数限制
        /// </summary>
        public int LimitedCount
        {
            get { return _LimitedCount; }
            set { _LimitedCount = value; }
        }
    }
}
