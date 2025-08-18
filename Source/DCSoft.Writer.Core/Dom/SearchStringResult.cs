using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace DCSoft.Writer.Dom
{

    /// <summary>
    /// 搜索字符串的结果信息列表
    /// </summary>
    [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
    public class SearchStringResultList : System.Collections.Generic.IEnumerable<SearchStringResult>
    {
        /// <summary>
        /// 初始化对象
        /// </summary>
        internal SearchStringResultList()
        {

        }
        /// <summary>
        /// 记录个数
        /// </summary>
        public int Count
        {
            get
            {
                return this._list.Count;
            }
        }
        /// <summary>
        /// 获得指定序号处的信息对象
        /// </summary>
        /// <param name="index">序号</param>
        /// <returns>信息对象</returns>
        public SearchStringResult this[int index]
        {
            get
            {
                return this._list[index];
            }
        }

        private readonly List<SearchStringResult> _list = new List<SearchStringResult>();
        /// <summary>
        /// 返回枚举器
        /// </summary>
        /// <returns>枚举器</returns>
        public IEnumerator<SearchStringResult> GetEnumerator()
        {
            return this._list.GetEnumerator();
        }

        /// <summary>
        /// 返回枚举器
        /// </summary>
        /// <returns>枚举器</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this._list.GetEnumerator();
        }

        internal void InnerAdd(SearchStringResult result)
        {
            this._list.Add(result);
        }
    }

    /// <summary>
    /// 搜索字符串的结果信息
    /// </summary>
    [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
    public class SearchStringResult
    {
        /// <summary>
        /// 初始化对象
        /// </summary>
        /// <param name="container"></param>
        /// <param name="firstElement"></param>
        /// <param name="text"></param>
        internal SearchStringResult(DomContainerElement container, DomCharElement firstElement, int elementLength, string text)
        {
            this._ContainerElement = container;
            this._FirstElement = firstElement;
            this._ElementsLength = elementLength;
            this._Text = text;
        }

        private readonly DomContainerElement _ContainerElement = null;
        /// <summary>
        /// 容器元素
        /// </summary>
        public DomContainerElement ContainerElement
        {
            get
            {
                return this._ContainerElement;
            }
        }
        private readonly string _Text = null;
        /// <summary>
        /// 文本
        /// </summary>
        public string Text
        {
            get
            {
                return this._Text;
            }
        }
        private readonly DomCharElement _FirstElement = null;
        /// <summary>
        /// 第一个字符元素
        /// </summary>
        public DomCharElement FirstElement
        {
            get
            {
                return _FirstElement;
            }
        }
        private readonly int _ElementsLength = 0;
        public int ElementsLength
        {
            get
            {
                return this._ElementsLength;
            }
        }
        /// <summary>
        /// 选中元素
        /// </summary>
        /// <returns>操作是否成功</returns>
        public bool Select()
        {
            var dce = this._FirstElement.DocumentContentElement;
            if (dce != null)
            {
                int index = dce.Content.IndexOfUseContentIndex(this._FirstElement);
                if (index >= 0)
                {
                    dce.Focus();
                    dce.SetSelection(index, this._ElementsLength);
                    return true;
                }
            }
            return false;
        }
    }

    internal class InnerSearchStringArgs
    {
        public InnerSearchStringArgs(
            DomDocument document,
            string text,
            bool ignoreCase,
            int maxResultCount)
        {
            this.Text = text;
            this.IgnoreCase = ignoreCase;
            this.MaxResultCount = maxResultCount;
            this.Result = new SearchStringResultList();
        }
        public readonly string Text;
        public readonly bool IgnoreCase;
        public readonly SearchStringResultList Result;
        public readonly int MaxResultCount;
    }

}