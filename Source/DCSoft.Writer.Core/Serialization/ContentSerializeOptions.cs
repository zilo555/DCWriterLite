using System;
using System.Collections.Generic;
using System.Text;
using DCSoft.Writer.Controls;

namespace DCSoft.Writer.Serialization
{
    /// <summary>
    /// 内容序列化选项
    /// </summary>
    /// <remarks>编制 袁永福</remarks>
    public class ContentSerializeOptions
    {
        /// <summary>
        /// 初始化对象
        /// </summary>
        public ContentSerializeOptions()
        {
        }

         

        private Encoding _ContentEncoding = null;
        /// <summary>
        /// 文本编码格式
        /// </summary>
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public Encoding ContentEncoding
        {
            get
            {
                return _ContentEncoding;
            }
            set
            {
                _ContentEncoding = value;
            }
        }

        private WriterControl _WriterControl = null;
        /// <summary>
        /// 相关的编辑器控件
        /// </summary>
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public WriterControl WriterControl
        {
            get
            {
                return _WriterControl; 
            }
            set
            {
                _WriterControl = value; 
            }
        }

        private object _Tag = null;
        /// <summary>
        /// 附加数据
        /// </summary>
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public object Tag
        {
            get { return _Tag; }
            set { _Tag = value; }
        }

        private bool _FastMode = false;
        /// <summary>
        /// 快速加载模式
        /// </summary>
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public bool FastMode
        {
            get { return _FastMode; }
            set { _FastMode = value; }
        }

        private string _FileName = null;
        /// <summary>
        /// 输出的文件名
        /// </summary>
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public string FileName
        {
            get
            {
                return _FileName; 
            }
            set
            {
                _FileName = value; 
            }
        }


        private bool _IncludeSelectionOnly = false;
        /// <summary>
        /// 只输出被选择的文档内容
        /// </summary>
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public bool IncludeSelectionOnly
        {
            get
            {
                return _IncludeSelectionOnly;
            }
            set
            {
                _IncludeSelectionOnly = value;
            }
        }

        private bool _SerializeAttachFiles = false;
        /// <summary>
        /// 是否序列化输出附加文件
        /// </summary>
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public bool SerializeAttachFiles
        {
            get
            {
                return _SerializeAttachFiles; 
            }
            set
            {
                _SerializeAttachFiles = value; 
            }
        }

        private bool _Formated = true;
        /// <summary>
        /// 是否进行格式化输出
        /// </summary>
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public bool Formated
        {
            get
            {
                return _Formated; 
            }
            set
            {
                _Formated = value; 
            }
        }

        private bool bolEnableDocumentSetting = true;
        /// <summary>
        /// 允许读取文档设置
        /// </summary>
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public bool EnableDocumentSetting
        {
            get
            {
                return bolEnableDocumentSetting;
            }
            set
            {
                bolEnableDocumentSetting = value;
            }
        }

        private bool _ImportTemplateGenerateParagraph = true ;
        /// <summary>
        /// 是否导入临时生成的段落符号
        /// </summary>
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public bool ImportTemplateGenerateParagraph
        {
            get
            {
                return _ImportTemplateGenerateParagraph; 
            }
            set
            {
                _ImportTemplateGenerateParagraph = value; 
            }
        }

        private Dictionary<string,string>  _Parameters = new Dictionary<string,string>();
        /// <summary>
        /// 用户参数
        /// </summary>
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public Dictionary<string, string> Parameters
        {
            get
            {
                return _Parameters; 
            }
        }

        private bool _ForPrint = false;
        /// <summary>
        /// 处于打印模式
        /// </summary>
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public bool ForPrint
        {
            get { return _ForPrint; }
            set { _ForPrint = value; }
        }
        /// <summary>
        /// 复制对象
        /// </summary>
        /// <returns>复制品</returns>
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public ContentSerializeOptions Clone()
        {
            ContentSerializeOptions opts = (ContentSerializeOptions)this.MemberwiseClone();
            opts._Parameters = new Dictionary<string, string>(this._Parameters);
            return opts;
        }
    }
}
