using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using DCSoft.Writer.Dom ;
using System.Runtime.InteropServices;
using DCSoft.Writer.Controls;
namespace DCSoft.Writer
{
    /// <summary>
    /// 向文档插入对象事件参数
    /// </summary>
    public partial class InsertObjectEventArgs  
    {

         /// <summary>
        /// 初始化对象
        /// </summary>
        /// <param name="document"></param>
        public InsertObjectEventArgs(DomDocument document)
        {
            _Document = document;
            if (_Document != null)
            {
                _WriterControl = document.EditorControl;
                //_DocumentControler = document.DocumentControler;
                _Document.Content.GetCurrentPositionInfo(out this._ContainerElement, out this._PositionInContainer);
                if (_Document.EditorControl != null)
                {
                    this._AllowDataFormats = _Document.EditorControl.AcceptDataFormats;
                }
            }

        }


        private DomElement _CurrentElement = null;
        /// <summary>
        /// 指定插入位置的文档元素对象
        /// </summary>
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        [System.Text.Json.Serialization.JsonIgnore]
        public DomElement CurrentElement
        {
            get
            {
                return _CurrentElement; 
            }
            set
            {
                _CurrentElement = value; 
            }
        }

        private bool _DetectForDragContent = false;
        /// <summary>
        /// 为拖拽文档内容而进行检测
        /// </summary>
        public bool DetectForDragContent
        {
            [JSInvokable]
            get
            {
                return _DetectForDragContent; 
            }
            set
            {
                _DetectForDragContent = value; 
            }
        }

        private WriterControl _WriterControl = null;
        /// <summary>
        /// 编辑器控件
        /// </summary>
        //[DCSoft.Common.DCPublishAPI]
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        [System.Text.Json.Serialization.JsonIgnore]
        public WriterControl WriterControl
        {
            get { return _WriterControl; }
            set { _WriterControl = value; }
        }
        
        private DomDocument _Document = null;
        /// <summary>
        /// 文档对象
        /// </summary>
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

        //private DocumentControler _DocumentControler = null;
        ///// <summary>
        ///// 文档控制器对象
        ///// </summary>
        //[System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        //[System.Text.Json.Serialization.JsonIgnore]
        //internal DocumentControler DocumentControler
        //{
        //    get
        //    {
        //        return _DocumentControler;
        //    }
        //    set
        //    {
        //        _DocumentControler = value;
        //    }
        //}

        private WriterDataFormats _AllowDataFormats = WriterDataFormats.All;
        /// <summary>
        /// 允许的数据格式
        /// </summary>
        //[DCSoft.Common.DCPublishAPI]
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public WriterDataFormats AllowDataFormats
        {
            [JSInvokable]
            get { return _AllowDataFormats; }
            set { _AllowDataFormats = value; }
        }

        
        private DomContainerElement _ContainerElement = null;
        /// <summary>
        /// 容器元素对象
        /// </summary>
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        [System.Text.Json.Serialization.JsonIgnore]
        public DomContainerElement ContainerElement
        {
            get { return _ContainerElement; }
            set { _ContainerElement = value; }
        }

        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public string ContainerElementID
        {
            [JSInvokable]
            get
            {
                return this._ContainerElement?.ID;
            }
        }
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public string ContainerElementName
        {
            [JSInvokable]
            get
            {
                return WriterUtilsInner.GetElementName(this._ContainerElement); 
            }
        }
        private int _PositionInContainer = -1;
        /// <summary>
        /// 插入点位置
        /// </summary>
        //[DCSoft.Common.DCPublishAPI]
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public int Position
        {
            [JSInvokable]
            get { return _PositionInContainer; }
            set { _PositionInContainer = value; }
        }

        private DragDropEffects _AllowedEffect = DragDropEffects.None;
        /// <summary>
        /// 允许的拖拽样式
        /// </summary>
        //[DCSoft.Common.DCPublishAPI]
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public DragDropEffects AllowedEffect
        {
            [JSInvokable]
            get { return _AllowedEffect; }
            set { _AllowedEffect = value; }
        }

        private DragDropEffects _DragEffect = DragDropEffects.None;
        /// <summary>
        /// 拖拽样式
        /// </summary>
        //[DCSoft.Common.DCPublishAPI]
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public DragDropEffects DragEffect
        {
            [JSInvokable]
            get { return _DragEffect; }
            set { _DragEffect = value; }
        }

        private IDataObject _DataObject = null;
        /// <summary>
        /// 要插入的对象
        /// </summary>
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        [System.Text.Json.Serialization.JsonIgnore]
        public IDataObject DataObject
        {
            get
            {
                return _DataObject;
            }
            set
            {
                _DataObject = value;
            }
        }
        /// <summary>
        /// 获得数据格式列表
        /// </summary>
        /// <returns></returns>
        //[DCSoft.Common.DCPublishAPI]
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        [JSInvokable]
        public string[] GetFormats()
        {
            if (this.DataObject == null)
            {
                return null;
            }
            else
            {
                return this.DataObject.GetFormats();
            }
        }
        /// <summary>
        /// 获得数据
        /// </summary>
        /// <param name="format">数据格式名称</param>
        /// <returns>数据</returns>
        //[DCSoft.Common.DCPublishAPI]
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        [JSInvokable]
        public object GetData(string format)
        {
            if (this.DataObject == null)
            {
                return null;
            }
            else
            {
                return this.DataObject.GetData(format);
            }
        }
        /// <summary>
        /// 获得是否存在指定格式的数据
        /// </summary>
        /// <param name="format">数据格式名称</param>
        /// <returns>是否存在</returns>
        //[DCSoft.Common.DCPublishAPI]
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        [JSInvokable]
        public bool GetDataPresent(string format)
        {
            if (this.DataObject == null)
            {
                return false;
            }
            else
            {
                return this.DataObject.GetDataPresent(format);
            }
        }


        private string _SpecifyFormat = null;
        /// <summary>
        /// 指定的数据格式
        /// </summary>
        //[DCSoft.Common.DCPublishAPI]
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public string SpecifyFormat
        {
            get
            {
                return _SpecifyFormat;
            }
            set
            {
                _SpecifyFormat = value;
            }
        }

        private bool _ShowUI = true;
        /// <summary>
        /// 是否显示用户界面
        /// </summary>
        //[DCSoft.Common.DCPublishAPI]
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public bool ShowUI
        {
            [JSInvokable]
            get
            {
                return _ShowUI;
            }
            set
            {
                _ShowUI = value;
            }
        }

        private bool _Handled = false;
        /// <summary>
        /// 事件已经处理过了标记
        /// </summary>
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        //[DCSoft.Common.DCPublishAPI]
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

        private InputValueSource _InputSource = InputValueSource.Clipboard ;
        /// <summary>
        /// 数据操作来源
        /// </summary>
        //[DCSoft.Common.DCPublishAPI]
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public InputValueSource InputSource
        {
            [JSInvokable]
            get { return _InputSource; }
            set { _InputSource = value; }
        }

        private bool _AutoSelectContent = false;
        /// <summary>
        /// 自动选择插入的内容
        /// </summary>
        //[DCSoft.Common.DCPublishAPI]
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public bool AutoSelectContent
        {
            [JSInvokable]
            get
            {
                return _AutoSelectContent; 
            }
            set
            {
                _AutoSelectContent = value; 
            }
        }

        private DomElementList _NewElements = null;
        /// <summary>
        /// 新增的文档元素列表
        /// </summary>
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        [System.Text.Json.Serialization.JsonIgnore]
        public DomElementList NewElements
        {
            get { return _NewElements; }
            set { _NewElements = value; }
        }
        

        private bool _Result = true;
        /// <summary>
        /// 操作是否成功
        /// </summary>
        //[DCSoft.Common.DCPublishAPI]
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public bool Result
        {
            [JSInvokable]
            get
            {
                return _Result;
            }
            [JSInvokable]
            set
            {
                _Result = value;
            }
        }
        internal DomDocument _LastLoadedDocument = null;
        internal DomElementList _LastLoadedElements = null;

        private List<string> _RejectFormats = new List<string>();
        /// <summary>
        /// 拒绝的数据格式名称
        /// </summary>
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        //[DCSoft.Common.DCPublishAPI]
        public List<string> RejectFormats
        {
            get { return _RejectFormats; }
            set { _RejectFormats = value; }
        }
    }
     
}