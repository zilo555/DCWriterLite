using System;
using System.Collections.Generic;
using System.Text;
using System.IO ;
using System.Runtime.InteropServices;

namespace DCSoft.Writer.Commands
{
    /// <summary>
    /// 插入文档内容使用的参数
    /// </summary>
   //[System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = false)]
   // [DCSoft.Common.DCPublishAPI]
    public partial class InsertDocumentCommandParameter
    {
        /// <summary>
        /// 初始化对象
        /// </summary>
        public InsertDocumentCommandParameter()
        {
        }

        private string _StringSource = null;

        /// <summary>
        /// 插入文档内容来源
        /// </summary>
        //[System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public string StringSource
        {
            get { return _StringSource; }
            set { _StringSource = value; }
        }

        private TextReader _SourceTextReader = null;
        /// <summary>
        /// 插入文档内容来源读取器
        /// </summary>
        public TextReader SourceTextReader
        {
            get { return _SourceTextReader; }
            set { _SourceTextReader = value; }
        }

        private Stream _SourceStream = null;
        /// <summary>
        /// 插入文档内容来源文件流
        /// </summary>
        public Stream SourceStream
        {
            get { return _SourceStream; }
            set { _SourceStream = value; }
        }

        private string _FileFormat = null;

        /// <summary>
        /// 插入文档内容的文件格式
        /// </summary>
        //[System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public string FileFormat
        {
            get { return _FileFormat; }
            set { _FileFormat = value; }
        }

        private int _StyleIndex = int.MinValue ;

        /// <summary>
        /// 插入文档内容对应的样式索引
        /// </summary>
        //[System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public int StyleIndex
        {
            get { return _StyleIndex; }
            set { _StyleIndex = value; }
        }

        private int _ParagraphStyleIndex = int.MinValue ;

        /// <summary>
        /// 插入文档内容段落样式对应的索引
        /// </summary>
        //[System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public int ParagraphStyleIndex
        {
            get { return _ParagraphStyleIndex; }
            set { _ParagraphStyleIndex = value; }
        }
    }
}
