using System;
using System.Collections.Generic;
using System.Text;

namespace DCSoft.Writer.Dom
{
    /// <summary>
    /// CanInsert函数参数类型
    /// </summary>
     
    [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = false )]
    //[DCSoft.Common.DCPublishAPI]
    public partial class CanInsertElementEventArgs
    {
        /// <summary>
        /// 初始化对象
        /// </summary>
        /// <param name="container">容器对象</param>
        /// <param name="index">序号</param>
        /// <param name="elementType">文档元素类型</param>
        /// <param name="flags">标记</param>
         
        public CanInsertElementEventArgs(DomContainerElement container, int index, Type elementType, DomAccessFlags flags)
        {
            _Container = container;
            _Index = index;
            _ElementType = elementType;
            _Flags = flags;
        }

        /// <summary>
        /// 初始化对象
        /// </summary>
        /// <param name="container">容器对象</param>
        /// <param name="index">序号</param>
        /// <param name="element">文档元素</param>
        /// <param name="flags">标记</param>
         
        public CanInsertElementEventArgs(DomContainerElement container, int index, DomElement element, DomAccessFlags flags)
        {
            _Container = container;
            _Index = index;
            _Element = element;
            _Flags = flags;
        }

        private DomContainerElement _Container = null;
        /// <summary>
        /// 容器元素
        /// </summary>
        //[DCSoft.Common.DCPublishAPI]
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public DomContainerElement Container
        {
            get { return _Container; }
            //set { _Container = value; }
        }

        private int _Index = 0;
        /// <summary>
        /// 序号
        /// </summary>
        //[DCSoft.Common.DCPublishAPI]
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public int Index
        {
            get { return _Index; }
            //set { _Index = value; }
        }

        private Type _ElementType = null;
        /// <summary>
        /// 文档元素类型
        /// </summary>
        //[DCSoft.Common.DCPublishAPI]
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public Type ElementType
        {
            get { return _ElementType; }
            //set { _ElementType = value; }
        }

        private DomElement _Element = null;
        /// <summary>
        /// 文档元素对象
        /// </summary>
        //[DCSoft.Common.DCPublishAPI]
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public DomElement Element
        {
            get { return _Element; }
            //set { _Element = value; }
        }

        private DomAccessFlags _Flags = DomAccessFlags.Normal;
        /// <summary>
        /// 标记
        /// </summary>
       // [DCSoft.Common.DCPublishAPI]
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public DomAccessFlags Flags
        {
            get { return _Flags; }
            set { _Flags = value; }
        }

        private bool _SetMessage = false;
        /// <summary>
        /// 设置的消息
        /// </summary>
       // [DCSoft.Common.DCPublishAPI]
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public bool SetMessage
        {
            get { return _SetMessage; }
            set { _SetMessage = value; }
        }

        private string _Message = null;
        /// <summary>
        /// 消息
        /// </summary>
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public string Message
        {
            get { return _Message; }
            set { _Message = value; }
        }

        private bool _Result = true;
        /// <summary>
        /// 结果
        /// </summary>
      //  [DCSoft.Common.DCPublishAPI]
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public bool Result
        {
            get { return _Result; }
            set { _Result = value; }
        }
    }
}
