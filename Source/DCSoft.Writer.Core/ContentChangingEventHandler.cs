using System;
using System.Collections.Generic;
using System.Text;
using DCSoft.Writer.Dom;
using System.Runtime.InteropServices;

namespace DCSoft.Writer
{


    /// <summary>
    /// 文档内容正在发生改变事件参数
    /// </summary>
    /// <remarks>编写 袁永福</remarks>
     
    [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = false )]
    public partial class ContentChangingEventArgs : EventArgs  
    {
        /// <summary>
        /// 初始化对象
        /// </summary>
        //[DCSoft.Common.DCPublishAPI]
        public ContentChangingEventArgs()
        {
        }

        private DomDocument _Document = null;
        /// <summary>
        /// 文档对象
        /// </summary>
        //[DCSoft.Common.DCPublishAPI]
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        [System.Text.Json.Serialization.JsonIgnore]
        public DomDocument Document
        {
            get
            {
                return _Document; 
            }
            set
            {
                _Document = value; 
            }
        }

        private DomElement _Element = null;
        /// <summary>
        /// 容器元素对象
        /// </summary>
        //[DCSoft.Common.DCPublishAPI]
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        [System.Text.Json.Serialization.JsonIgnore]
        public DomElement Element
        {
            get
            {
                return _Element; 
            }
            set
            {
                _Element = value; 
            }
        }
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public string ElementID
        {
            [JSInvokable]
            get
            {
                return this._Element?.ID;
            }
        }
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public string ElementName
        {
            [JSInvokable]
            get
            {
                return WriterUtilsInner.GetElementName(this._Element);
            }
        }
        /// <summary>
        /// 获得操作完成后容器元素的预计文本内容
        /// </summary>
        /// <returns>预计的文本</returns>
        //[DCSoft.Common.DCPublishAPI]
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        [JSInvokable]
        public string GetContainerNewText()
        {
            DomContainerElement c = this.Element as DomContainerElement;
            if (c == null)
            {
                return null ;
            }
            DomElementList elements = c.Elements.Clone();
            if (this.DeletingElements != null)
            {
                foreach (DomElement element in this.DeletingElements)
                {
                    int index = elements.IndexOf(element);
                    if (index >= 0)
                    {
                        elements.RemoveAt(index);
                    }
                }
            }
            if (this.InsertingElements != null)
            {
                int index = this.ElementIndex;
                if (index < 0)
                {
                    index = 0;
                }
                if (elements.Count > 0)
                {
                    if (index >= elements.Count)
                    {
                        index = elements.Count - 1;
                    }
                }
                else
                {
                    index = 0;
                }
                for (int iCount = 0; iCount < this.InsertingElements.Count; iCount++)
                {
                    elements.Insert(
                        index + iCount, 
                        this.InsertingElements[iCount]);
                }
            }
            string txt = elements.GetInnerText();
            return txt;
        }

        private object _Tag = null;
        /// <summary>
        /// 额外的数据
        /// </summary>
        //[DCSoft.Common.DCPublishAPI]
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        [System.Text.Json.Serialization.JsonIgnore]
        public object Tag
        {
            get
            {
                return _Tag; 
            }
            set
            {
                _Tag = value; 
            }
        }

        private int _ElementIndex = 0;
        /// <summary>
        /// 发生操作时的元素位置序号
        /// </summary>
        //[DCSoft.Common.DCPublishAPI]
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public int ElementIndex
        {
            [JSInvokable]
            get
            {
                return _ElementIndex; 
            }
            set
            {
                _ElementIndex = value; 
            }
        }

        private DomElementList _DeletingElements = null;
        /// <summary>
        /// 正要删除的元素列表
        /// </summary>
        //[DCSoft.Common.DCPublishAPI]
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        [System.Text.Json.Serialization.JsonIgnore]
        public DomElementList DeletingElements
        {
            get
            {
                return _DeletingElements; 
            }
            set
            {
                _DeletingElements = value; 
            }
        }

        private DomElementList _InsertingElements = null;
        /// <summary>
        /// 准备新增的元素列表
        /// </summary>
        //[DCSoft.Common.DCPublishAPI]
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        [System.Text.Json.Serialization.JsonIgnore]
        public DomElementList InsertingElements
        {
            get
            {
                return _InsertingElements; 
            }
            set
            {
                _InsertingElements = value; 
            }
        }

        private bool _CancelBubble = false;
        /// <summary>
        /// 取消事件向上层元素冒泡传递
        /// </summary>
        //[DCSoft.Common.DCPublishAPI]
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public bool CancelBubble
        {
            [JSInvokable]
            get
            {
                return _CancelBubble; 
            }
            [JSInvokable]
            set
            {
                _CancelBubble = value; 
            }
        }

        private bool _Cancel = false;
        /// <summary>
        /// 用户取消操作
        /// </summary>
        //[DCSoft.Common.DCPublishAPI]
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public bool Cancel
        {
            [JSInvokable]
            get
            {
                return _Cancel; 
            }
            [JSInvokable]
            set
            {
                _Cancel = value; 
            }
        }

        private bool _Handled = false;
        /// <summary>
        /// 事件已经被处理掉了，无需后续处理
        /// </summary>
        //[DCSoft.Common.DCPublishAPI]
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public bool Handled
        {
            [JSInvokable]
            get
            {
                return _Handled; 
            }
            [JSInvokable]
            set
            {
                _Handled = value; 
            }
        }
    }
     
}
