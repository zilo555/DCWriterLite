using System;
using System.Collections.Generic;
using System.Text;
using System.Collections ;
using DCSoft.Common;

namespace DCSoft.Writer.Dom
{
    // 袁永福到此一游 2016-3-18

/// <summary>
/// 文档树节点遍历器
/// </summary>
    public class DomTreeNodeEnumerable : DCSoft.Common.TreeNodeEnumerable
    {
        /// <summary>
        /// 初始化对象
        /// </summary>
        /// <param name="elements">文档元素列表</param>
        //[DCSoft.Common.DCPublishAPI]
        public DomTreeNodeEnumerable(DomElementList elements)
        {
            base._RootNodes = elements;
        }

        /// <summary>
        /// 初始化对象
        /// </summary>
        /// <param name="rootElement">根节点</param>
        /// <param name="includeRootElement">遍历时是否包含根节点</param>
        //[DCSoft.Common.DCPublishAPI]
        public DomTreeNodeEnumerable(DomElement rootElement, bool includeRootElement = false)
        {
            if (includeRootElement)
            {
                base._RootNodes = new DomElement[] { rootElement };
            }
            else
            {
                base._RootNodes = rootElement.Elements;
                base._RootParent = rootElement;
            }
        }

         
        public override object GetParent(object instance)
        {
            return ((DomElement)instance).Parent;
        }

         
        public override IEnumerable GetChildNodes(object instance)
        {
            return ((DomElement)instance).Elements;
        }

         
        public override bool IsPublish(object current)
        {
            if (this._ExcludeCharElement)
            {
                if (current is DomCharElement)
                {
                    return false;
                }
            }
            if (this._ExcludeParagraphFlag)
            {
                if (current is DomParagraphFlagElement)
                {
                    return false;
                }
            }
            return true;
        }

        private bool _ExcludeCharElement = true ;
        /// <summary>
        /// 忽略字符元素
        /// </summary>
        //[DCSoft.Common.DCPublishAPI]
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public bool ExcludeCharElement
        {
            get
            {
                return _ExcludeCharElement;
            }
            set
            {
                _ExcludeCharElement = value;
            }
        }

        private bool _ExcludeParagraphFlag = true ;
        /// <summary>
        /// 忽略段落符号元素
        /// </summary>
        //[DCSoft.Common.DCPublishAPI]
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public bool ExcludeParagraphFlag
        {
            get
            {
                return _ExcludeParagraphFlag;
            }
            set
            {
                _ExcludeParagraphFlag = value;
            }
        }
         
    }
}
