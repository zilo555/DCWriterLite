using System;
using System.Collections.Generic;
using System.Text;
using DCSoft.Writer.Dom;
using System.Runtime.InteropServices;
using DCSoft.Writer.Controls;

namespace DCSoft.Writer
{
    /// <summary>
    /// 文档元素加载事件处理
    /// </summary>
     
    
    [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = false )]
    //[DCSoft.Common.DCPublishAPI]
    public partial class ElementLoadEventArgs : ElementEventArgs
    {
        /// <summary>
        /// 初始化对象
        /// </summary>
        /// <param name="element">文档元素对象</param>
        /// <param name="format">加载文档的文件格式</param>
         
        public ElementLoadEventArgs(DomElement element, string format)
            : base(element)
        {
            this._Format = format;
            this.Support_PropertyValueExpressions = true;
            this.Support_ValueExpression = true;
        }
        /// <summary>
        /// 因为外界导入文档元素而执行的过程
        /// </summary>
        public bool ForImportElements = false;

        public readonly bool Support_PropertyValueExpressions;

        public readonly bool Support_ValueExpression;

        //private bool _IsImportOrAppendMode = false;
        ///// <summary>
        ///// 是否为导入或追加模式
        ///// </summary>
        //public bool IsImportOrAppendMode
        //{
        //    get
        //    {
        //        return _IsImportOrAppendMode;
        //    }
        //    set
        //    {
        //        _IsImportOrAppendMode = value;
        //    }
        //}

        private string _Format = null;
        /// <summary>
        /// 文件格式
        /// </summary>
        //[DCSoft.Common.DCPublishAPI]
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public string Format
        {
            get
            {
                return _Format;
            }
            set
            {
                _Format = value;
            }
        }
        
    }

}
