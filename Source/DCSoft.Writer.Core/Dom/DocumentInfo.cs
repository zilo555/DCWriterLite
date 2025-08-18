using System;
using System.Runtime.InteropServices;
using System.ComponentModel ;
using DCSoft.Common;

namespace DCSoft.Writer.Dom
{
	/// <summary>
	/// 文档设置信息对象
	/// </summary>
    public partial class DocumentInfo
	{
        /// <summary>
        /// 初始化对象
        /// </summary>
        public DocumentInfo()
        {
        }


        private bool _Readonly = false;
        /// <summary>
        /// 文档内容只读
        /// </summary>
        public bool Readonly
        {
            get
            {
                return _Readonly; 
            }
            set
            {
                _Readonly = value; 
            }
        }

        //private bool _SubDocumentReadonly = false;
        ///// <summary>
        ///// 子文档时只读设置
        ///// </summary>
        //[DefaultValue( false )]
        //public bool SubDocumentReadonly
        //{
        //    get { return _SubDocumentReadonly; }
        //    set { _SubDocumentReadonly = value; }
        //}

        //private bool _SubDocumentLocked = false;
        ///// <summary>
        ///// 子文档时的锁定标记
        ///// </summary>
        //[DefaultValue( false )]
        //public bool SubDocumentLocked
        //{
        //    get { return _SubDocumentLocked; }
        //    set { _SubDocumentLocked = value; }
        //}

        ///// <summary>
        ///// 初始化对象
        ///// </summary>
        ///// <param name="doc">文档对象</param>
        //internal DocumentSettings( TextDocument doc )
        //{
        //    myDocument = doc ;
        //}

        //private XTextDocument myDocument = null;
        ///// <summary>
        ///// 对象所属文档对象
        ///// </summary>
        ////[System.ComponentModel.Browsable( false )]
        //[DCSystemXmlSerialization.XmlIgnore()]
        //public XTextDocument Document
        //{
        //    get
        //    { 
        //        return myDocument ;
        //    }
        //}

        private bool _RefreshLayoutFlag = false;
        /// <summary>
        /// 刷新文档排版标记,DCWriter内部使用。
        /// </summary>
        public bool RefreshLayoutFlag
        {
            get
            {
                return _RefreshLayoutFlag; 
            }
            set
            {
                _RefreshLayoutFlag = value; 
            }
        }

        private bool _RefreshViewFlag = false;
        /// <summary>
        /// 刷新文档视图标记,DCWriter内部使用。
        /// </summary>
        public bool RefreshViewFlag
        {
            get
            {
                return _RefreshViewFlag; 
            }
            set
            {
                _RefreshViewFlag = value; 
            }
        }

        private DCBooleanValue _ShowHeaderBottomLine = DCBooleanValue.Inherit;
        /// <summary>
        /// 是否显示页眉下面的横线
        /// </summary>
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public DCBooleanValue ShowHeaderBottomLine
        {
            get 
            {
                return _ShowHeaderBottomLine; 
            }
            set
            {
                _ShowHeaderBottomLine = value;
                this._RefreshViewFlag = true;
            }
        }

        private float _FieldBorderElementWidth = 1f;
        /// <summary>
        /// 输入域边框元素像素宽度
        /// </summary>
        public float FieldBorderElementWidth
        {
            get
            {
                return _FieldBorderElementWidth; 
            }
            set
            {
                _FieldBorderElementWidth = value;
                this._RefreshLayoutFlag = true;
            }
        }
        private string _RuntimeTitle = null;
        /// <summary>
        /// 实际使用的文档标题
        /// </summary>
        public string RuntimeTitle
        {
            get 
            {
                return _RuntimeTitle; 
            }
            set
            {
                _RuntimeTitle = value; 
            }
        }

        private string _ID = null;
        /// <summary>
        /// 文档编号
        /// </summary>
        public string ID
        {
            get
            {
                return _ID; 
            }
            set
            {
                _ID = value; 
            }
        }

        private bool _IsTemplate = false;
        /// <summary>
        /// 该文档是模板文档
        /// </summary>
        public bool IsTemplate
        {
            get 
            {
                return _IsTemplate; 
            }
            set
            {
                _IsTemplate = value; 
            }
        }

        private int _TimeoutHours = 0;
        
        private string _Version = null;
        /// <summary>
        /// 内容版本号
        /// </summary>
        public string Version
        {
            get 
            {
                return _Version; 
            }
            set
            {
                _Version = value; 
            }
        }

        private string _Title = null;
        /// <summary>
        /// 文档标题
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

        private string _Description = null;
        /// <summary>
        /// 文档说明
        /// </summary>
        public string Description
        {
            get
            {
                return _Description; 
            }
            set
            {
                _Description = value; 
            }
        }


        private DateTime _CreationTime = WriterConst.NullDateTime;//XTextDocument.NullDateTime;
        /// <summary>
        /// 文档创建日期
        /// </summary>
        public DateTime CreationTime
        {
            get 
            {
                return _CreationTime; 
            }
            set
            {
                _CreationTime = value; 
            }
        }

        private DateTime _LastModifiedTime = WriterConst.NullDateTime;
        /// <summary>
        /// 最后修改日期
        /// </summary>
        public DateTime LastModifiedTime
        {
            get
            {
                return _LastModifiedTime; 
            }
            set
            {
                _LastModifiedTime = value; 
            }
        }

        private int _EditMinute = 0;
        /// <summary>
        /// 文档编辑的时间长度，单位分钟。
        /// </summary>
        public int EditMinute
        {
            get
            {
                return _EditMinute; 
            }
            set
            {
                _EditMinute = value; 
            }
        }

        private DateTime _LastPrintTime = WriterConst.NullDateTime;
        /// <summary>
        /// 最后一次打印的时间
        /// </summary>
        public DateTime LastPrintTime
        {
            get
            {
                return _LastPrintTime ; 
            }
            set
            {
                _LastPrintTime = value; 
            }
        }

        private string _Author = null;
        /// <summary>
        /// 作者
        /// </summary>
        public string Author
        {
            get { return _Author; }
            set { _Author = value; }
        }

        private string _AuthorName = null;
        /// <summary>
        /// 作者姓名
        /// </summary>
        public string AuthorName
        {
            get { return _AuthorName; }
            set { _AuthorName = value; }
        }

        private string _DepartmentID = null;
        /// <summary>
        /// 部门编号
        /// </summary>
        public string DepartmentID
        {
            get { return _DepartmentID; }
            set { _DepartmentID = value; }
        }

        private string _DepartmentName = null;
        /// <summary>
        /// 部门名称
        /// </summary>
        public string DepartmentName
        {
            get { return _DepartmentName; }
            set { _DepartmentName = value; }
        }

        private string _DocumentFormat = null;
        /// <summary>
        /// 文件格式
        /// </summary>
        public string DocumentFormat
        {
            get
            {
                return _DocumentFormat; 
            }
            set
            {
                _DocumentFormat = value; 
            }
        }

        private string _DocumentType = null;
        /// <summary>
        /// 文档类型
        /// </summary>
        public string DocumentType
        {
            get { return _DocumentType; }
            set { _DocumentType = value; }
        }

        private int _DocumentProcessState = 0;
        /// <summary>
        /// 文档操作状态
        /// </summary>
        public int DocumentProcessState
        {
            get { return _DocumentProcessState; }
            set { _DocumentProcessState = value; }
        }

        private int _DocumentEditState = 0;
        /// <summary>
        /// 文档编辑状态
        /// </summary>
         public int DocumentEditState
        {
            get { return _DocumentEditState; }
            set { _DocumentEditState = value; }
        }
         
        private string _Comment = null;
        /// <summary>
        /// 内容说明
        /// </summary>
        public string Comment
        {
            get { return _Comment; }
            set { _Comment = value; }
        }

        private static readonly string _DefaultOperator = "DCSoft.Writer Version:" + typeof(DocumentInfo).Assembly.GetName().Version;

        private string _Operator = _DefaultOperator;
        /// <summary>
        /// 操作者
        /// </summary>
        public string Operator
        {
            get { return _Operator; }
            set { _Operator = value; }
        }

        private int _NumOfPage = 0;
        /// <summary>
        /// 文档总页数
        /// </summary>
        public int NumOfPage
        {
            get
            {
                return _NumOfPage; 
            }
            set
            {
                _NumOfPage = value; 
            }
        }

        private bool _Printable = true;
        /// <summary>
        /// 文档能否打印
        /// </summary>
        public bool Printable
        {
            get
            {
                return _Printable; 
            }
            set
            {
                _Printable = value; 
            }
        }
         
        private int _StartPositionInPringJob = 0;
        /// <summary>
        /// 文档在打印任务中的开始的打印位置
        /// </summary>
        public int StartPositionInPringJob
        {
            get { return _StartPositionInPringJob; }
            set { _StartPositionInPringJob = value; }
        }

        private int _HeightInPrintJob = 0;
        /// <summary>
        /// 文档在打印任务中的打印高度
        /// </summary>
        public int HeightInPrintJob
        {
            get { return _HeightInPrintJob; }
            set { _HeightInPrintJob = value; }
        }

        /// <summary>
        /// 复制对象
        /// </summary>
        /// <returns>复制品</returns>
        public DocumentInfo Clone()
        {
            DocumentInfo info = (DocumentInfo)this.MemberwiseClone();
            return info;
        }
    }
     
}