using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using DCSoft.Writer.Dom ;
using System.Runtime.InteropServices;
namespace DCSoft.Writer
{

    /// <summary>
    /// 判断能否插入对象事件参数
    /// </summary>
     
    
    [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = false )]
    public partial class CanInsertObjectEventArgs   
    {

        /// <summary>
        /// 初始化对象
        /// </summary>
        /// <param name="document"></param>
        public CanInsertObjectEventArgs(DomDocument document)
        {
            _Document = document;
            if (_Document != null)
            {
                _DocumentControler = document.DocumentControler;
                _Document.Content.GetCurrentPositionInfo(
                    out this._ContainerElement, 
                    out this._PositionInContainer);
            }
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

        private DocumentControler _DocumentControler = null;
        /// <summary>
        /// 文档控制器对象
        /// </summary>
        public DocumentControler DocumentControler
        {
            get
            {
                return _DocumentControler; 
            }
            set
            {
                _DocumentControler = value; 
            }
        }

        private WriterDataFormats _AllowDataFormats = WriterDataFormats.All;
        /// <summary>
        /// 允许接收的数据格式
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

        private int _PositionInContainer = -1;
        /// <summary>
        /// 插入的位置
        /// </summary>
        //[DCSoft.Common.DCPublishAPI]
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public int Position
        {
            [JSInvokable]
            get { return _PositionInContainer; }
            set { _PositionInContainer = value; }
        }
        private int _SpecifyPosition = -1;
        /// <summary>
        /// 文档中指定的位置序号
        /// </summary>
        //[DCSoft.Common.DCPublishAPI]
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public int SpecifyPosition
        {
            [JSInvokable]
            get
            {
                return _SpecifyPosition;
            }
            set
            {
                _SpecifyPosition = value;
            }
        }

        private IDataObject _DataObject = null;
        /// <summary>
        /// 要插入的数据对象
        /// </summary>
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
        /// 获得所有可用的数据格式名称
        /// </summary>
        /// <returns>数据格式名称数组</returns>
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
        /// 获得指定格式的数据
        /// </summary>
        /// <param name="format">数据格式</param>
        /// <returns>获得的数据</returns>
        //[DCSoft.Common.DCPublishAPI]
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
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
        /// 判断是否包含指定格式的数据
        /// </summary>
        /// <param name="format">数据格式</param>
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
        /// 用户指定的数据格式
        /// </summary>
        //[DCSoft.Common.DCPublishAPI]
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public string SpecifyFormat
        {
            [JSInvokable]
            get
            {
                return _SpecifyFormat;
            }
            [JSInvokable]
            set
            {
                _SpecifyFormat = value;
            }
        }

        private DomAccessFlags _AccessFlags = DomAccessFlags.Normal;
        /// <summary>
        /// 访问标记
        /// </summary>
        //[DCSoft.Common.DCPublishAPI]
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public DomAccessFlags AccessFlags
        {
            get
            {
                return _AccessFlags;
            }
            set
            {
                _AccessFlags = value;
            }
        }

        private bool _Handled = false;
        /// <summary>
        /// 事件已经被处理了，无需后续处理
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

        private bool _Result = false ;
        /// <summary>
        /// 判断结果,true为可以插入对象数据；false不可插入对象数据。
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
    }
     
}