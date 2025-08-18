using System;
using System.Collections.Generic;
using System.Text;
using DCSoft.Writer.Dom;
using System.Windows.Forms;
using DCSoft.WinForms;
using DCSoft.Common;

namespace DCSoft.Writer.Controls
{
    /// <summary>
    /// 元素属性值编辑相关的上下文
    /// </summary>
    public class ElementValueEditContext
    {
        /// <summary>
        /// 初始化对象
        /// </summary>
        public ElementValueEditContext()
        {
        }


        private DomDocument _Document = null;
        /// <summary>
        /// 文档对象
        /// </summary>
        //[System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
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

        /// <summary>
        /// 编辑器控件
        /// </summary>
        //[System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public WriterControl WriterControl
        {
            get
            {
                if (this._Document == null)
                {
                    return null;
                }
                else
                {
                    return this._Document.EditorControl;
                }
            }
        }

        private DomElement _Element = null;
        /// <summary>
        /// 当前编辑的元素对象
        /// </summary>
        //[System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
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

        private string _PropertyName = null;
        /// <summary>
        /// 编辑的属性名
        /// </summary>
        //[System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public string PropertyName
        {
            get
            {
                return _PropertyName;
            }
            set
            {
                _PropertyName = value;
            }
        }

        private ElementValueEditor _Editor = null;
        /// <summary>
        /// 正在运行的文档元素值编辑器
        /// </summary>
        //[System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public ElementValueEditor Editor
        {
            get { return _Editor; }
            set { _Editor = value; }
        }

        private ElementValueEditorEditStyle _EditStyle = ElementValueEditorEditStyle.None;
        /// <summary>
        /// 正在使用的编辑器编辑样式
        /// </summary>
        //[System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public ElementValueEditorEditStyle EditStyle
        {
            get { return _EditStyle; }
            set { _EditStyle = value; }
        }
    }
}
