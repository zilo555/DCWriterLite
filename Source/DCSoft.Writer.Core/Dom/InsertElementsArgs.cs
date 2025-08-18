using System;
using System.Collections.Generic;
using System.Text;

// 袁永福到此一游

namespace DCSoft.Writer.Dom
{
    /// <summary>
    /// 插入文档元素的参数.DCWriter内部使用。
    /// </summary>
    public class InsertElementsArgs
    {
        /// <summary>
        /// 初始化对象
        /// </summary>
        public InsertElementsArgs()
        {
        }

        private DomElement _CurrentElement = null;
        /// <summary>
        /// 指定插入位置的文档元素对象
        /// </summary>
        public DomElement CurrentElement
        {
            get { return _CurrentElement; }
            set { _CurrentElement = value; }
        }

        private DomElementList _NewElements = null;
        /// <summary>
        /// 要插入的文档元素对象列表
        /// </summary>
        public DomElementList NewElements
        {
            get { return _NewElements; }
            set { _NewElements = value; }
        }

        private bool _LogUndo = true;
        /// <summary>
        /// 是否记录撤销操作信息
        /// </summary>
        public bool LogUndo
        {
            get { return _LogUndo; }
            set { _LogUndo = value; }
        }

        private bool _DetectOnly = false;
        /// <summary>
        /// 检测模式，不真的执行插入操作
        /// </summary>
        public bool DetectOnly
        {
            get { return _DetectOnly; }
            set { _DetectOnly = value; }
        }

        private int _Result = 0;
        /// <summary>
        /// 操作结果
        /// </summary>
        public int Result
        {
            get { return _Result; }
            set { _Result = value; }
        }

        private bool _UpdateContent = true;
        /// <summary>
        /// 更新文档内容
        /// </summary>
        public bool UpdateContent
        {
            get { return _UpdateContent; }
            set { _UpdateContent = value; }
        }

        private bool _FromUI = true;

        public bool FromUI
        {
            get { return _FromUI; }
            set { _FromUI = value; }
        }
    }
}