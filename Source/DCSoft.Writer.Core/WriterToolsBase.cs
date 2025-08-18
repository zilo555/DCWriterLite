using System;
using System.Collections.Generic;
using System.Text;
using DCSoft.Writer.Controls;
using DCSoft.Writer.Dom;
using System.Runtime.InteropServices;
using System.ComponentModel;
using DCSoft.Common;
using System.Collections;
using DCSoft.Printing;
using DCSoft.Writer.Data;

namespace DCSoft.Writer
{
    /// <summary>
    /// 编辑器使用的工具集基础类型
    /// </summary>
    public class WriterToolsBase
    {
        
        public virtual ElementValueEditor CreateElementValueEditor(DomElement element)
        {
            return null;
        }

        public virtual string FormatSelectedValueByIndexs(
            WriterControl ctl,
            DomInputFieldElement element,
            int[] indexs,
            bool getText)
        {
            return null;
        }

        public virtual string[] ParseSelectedValue(
            WriterControl ctl,
            DomElement element,
            List<string> allValues,
            string itemSpliter,
            string Value)
        {
            return null;
        }

        protected WriterToolsBase()
        {
        }

        private static WriterToolsBase _Instance = null;
        /// <summary>
        /// 对象实例
        /// </summary>
        public static WriterToolsBase Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = new WriterToolsBase();
                }
                return _Instance;
            }
            set
            {
                _Instance = value;
            }
        }
    }
}
