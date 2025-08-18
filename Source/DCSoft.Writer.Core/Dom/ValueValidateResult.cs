using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using DCSoft.Common ;
using System.ComponentModel;

namespace DCSoft.Writer.Dom
{
    /// <summary>
    /// 数值校验结果集合
    /// </summary>
    /// <remarks>编制 袁永福</remarks>
#if !RELEASE
    [System.Diagnostics.DebuggerDisplay("Count={ Count }")]
    [System.Diagnostics.DebuggerTypeProxy(typeof(DCSoft.Common.ListDebugView))]
#endif
    public partial class ValueValidateResultList : List<ValueValidateResult>
    {
        //[DCSoft.Common.DCPublishAPI]
        public ValueValidateResultList()
        {
        }
#if !RELEASE
        /// <summary>
        /// 返回表示对象内容的字符串
        /// </summary>
        /// <returns>字符串</returns>
        public override string ToString()
        {
            StringBuilder str = new StringBuilder();
            int index = 1;
            foreach (ValueValidateResult item in this)
            {
                if (str.Length > 0)
                {
                    str.Append(Environment.NewLine);
                }
                str.Append(Convert.ToString(index) + ".");
                index++;
                str.Append(item.Message);
            }
            return str.ToString();
        }
#endif
        /// <summary>
        /// 根据元素的DOM层次结构进行排序
        /// </summary>
        public void SoryByDOMLevel()
        {
            this.Sort(new ItemComparer());
        }

        private class ItemComparer : IComparer<ValueValidateResult>
        {
            public int Compare(ValueValidateResult x, ValueValidateResult y)
            {
                if (x.Element != null && y.Element != null)
                {
                    return WriterUtilsInner.CompareDOMLevel(x.Element, y.Element);
                }
                return 0;
            }
        }
    }

    /// <summary>
    /// 数值校验结果信息对象
    /// </summary>
    public partial class ValueValidateResult
    {
        /// <summary>
        /// 初始化对象
        /// </summary>
        public ValueValidateResult()
        {
        }

        /// <summary>
        /// 获得内容哈希值
        /// </summary>
        /// <returns></returns>
        public int GetContentHasCode()
        {
            int result = 0;
            if( this._Title != null )
            {
                result += this._Title.GetHashCode();
            }
            if( this._Message != null )
            {
                result += this._Message.GetHashCode();
            }
            result += (int)this._Type;
            result += (int)this._Level;
            return result;
        }

        private string _Title = null;
        /// <summary>
        /// 标题
        /// </summary>
        public string Title
        {
            get
            {
                return _Title; 
            }
            set
            {
                _Title = value; 
            }
        }
        private DomElement _Element = null;
        /// <summary>
        /// 文档元素对象
        /// </summary>
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

        /// <summary>
        /// 相关的文档元素名称
        /// </summary>
        public string ElementID
        {
            get
            {
                if (this._Element == null)
                {
                    return null;
                }
                return this._Element.ID;
            }
            set
            {
            }
        }

        private ValueValidateLevel _Level = ValueValidateLevel.Error;
        /// <summary>
        /// 信息等级
        /// </summary>
        public ValueValidateLevel Level
        {
            get
            {
                return _Level; 
            }
            set
            {
                _Level = value; 
            }
        }

        private string _Message = null;
        /// <summary>
        /// 信息
        /// </summary>
        public string Message
        {
            get
            {
                return _Message; 
            }
            set
            {
                _Message = value; 
            }
        }

        private ValueValidateResultTypes _Type = ValueValidateResultTypes.ValueValidate;
        /// <summary>
        /// 类型
        /// </summary>
        public ValueValidateResultTypes Type
        {
            get
            {
                return _Type; 
            }
            set
            {
                _Type = value; 
            }
        }

        
        public bool EqualsValue( ValueValidateResult info )
        {
            if( info == null )
            {
                return false;
            }
            if( info == this )
            {
                return true;
            }
            return  this._Type == info._Type
                && this._Element == info._Element
                && this._Level == info._Level
                && this._Message == info._Message
                && this._Title == info._Title;
        }

        /// <summary>
        /// 在文档中选择相关内容
        /// </summary>
        /// <returns>操作是否成功</returns>
        public bool Selet()
        {
            if (this.Element == null)
            {
                return false;
            }
            DomDocument document = this.Element.OwnerDocument;
            {
                this.Element.Focus();
                return true;
            }
        }
        /// <summary>
        /// 获得所引用的文档元素列表
        /// </summary>
        /// <returns></returns>
        internal DomElementList GetReferenceElements()
        {
            if (this.Element == null)
            {
                return null;
            }
                return new DomElementList(this.Element);
        }
#if ! RELEASE
        /// <summary>
        /// 返回表示对象数据的字符串
        /// </summary>
        /// <returns>字符串</returns>
        public override string ToString()
        {
            if (this.Element == null)
            {
                return this.Message;
            }
            else
            {
                return this.Element.ID + ":" + this.Message;
            }
        }
#endif
        internal void Clear()
        {
            this._Element = null;
            this._Message = null;
        }
    }

    /// <summary>
    /// 数据校验结果类型
    /// </summary>
    public enum ValueValidateResultTypes
    {
        /// <summary>
        /// 数据校验
        /// </summary>
        ValueValidate 
    }
}
