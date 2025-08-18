using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using DCSoft.Writer.Dom;
using System.Runtime.InteropServices;

namespace DCSoft.Writer.Controls
{
    /// <summary>
    /// 获得列表项目显示的文本的事件委托类型
    /// </summary>
    /// <param name="eventSender">参数</param>
    /// <param name="args">参数</param>
     
    [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true )]
    public delegate void FormatListItemsEventHandler(object eventSender, FormatListItemsEventArgs args);


    /// <summary>
    /// 获得列表项目文本的事件参数
    /// </summary>
     
    [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = false )]
    //[DCSoft.Common.DCPublishAPI]
    public partial class FormatListItemsEventArgs  
    {
        /// <summary>
        /// 初始化对象
        /// </summary>
        /// <param name="ctl"></param>
        /// <param name="formatString"></param>
        /// <param name="document"></param>
        /// <param name="element"></param>
        /// <param name="selectedItems"></param>
        /// <param name="unSelectedItems"></param>
        /// <param name="separator">指定的分隔字符</param>
        public FormatListItemsEventArgs(
            WriterControl ctl ,
            DomDocument document,
            DomElement element,
            string[] selectedItems,
            string[] unSelectedItems,
            string separator)
        {
            this._WriterControl = ctl;
            this._Document = document;
            this._Element = element;
            this._SelectedItems = selectedItems;
            this._UnselectedItems = unSelectedItems;
            this._Separator = separator;
        }

        private WriterControl _WriterControl = null;
        /// <summary>
        /// 编辑器控件对象
        /// </summary>
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public WriterControl WriterControl
        {
            get
            {
                return _WriterControl; 
            }
        }

        private DomDocument _Document = null;
        /// <summary>
        /// 文档对象
        /// </summary>
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public DomDocument Document
        {
            get
            {
                return _Document;
            }
        }

        private DomElement _Element = null;
        /// <summary>
        /// 相关的文档元素对象
        /// </summary>
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public DomElement Element
        {
            get
            {
                return _Element;
            }
        }

        private string _Separator = ",";
        /// <summary>
        /// 各个项目之间的分隔字符
        /// </summary>
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public string Separator
        {
            get
            {
                return _Separator; 
            }
        }

        /// <summary>
        /// 各个项目之间的分隔字符
        /// </summary>
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public char SeparatorChar
        {
            get
            {
                if (_Separator != null && _Separator.Length > 0)
                {
                    return _Separator[0];
                }
                else
                {
                    return (char)0;
                }
            }
        }


        private string[] _SelectedItems = null;
        /// <summary>
        /// 被选中的列表
        /// </summary>
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public string[] SelectedItems
        {
            get 
            {
                return _SelectedItems; 
            }
        }

        private string[] _UnselectedItems = null;
        /// <summary>
        /// 没有被选中的列表
        /// </summary>
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public string[] UnselectedItems
        {
            get 
            {
                return _UnselectedItems; 
            }
        }

        private string _Result = null;
        /// <summary>
        /// 计算结果
        /// </summary>
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public string Result
        {
            get
            {
                return _Result; 
            }
            set
            {
                _Result = value; 
            }
        }
    }



}
