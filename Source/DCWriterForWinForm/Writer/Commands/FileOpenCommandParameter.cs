using System;
using System.Collections.Generic;
using System.Text;
using System.IO ;
using System.Runtime.InteropServices ;
namespace DCSoft.Writer.Commands
{
    /// <summary>
    /// FileOpen编辑器命令使用的参数
    /// </summary>
    /// <remarks>编制 袁永福</remarks>
    [Serializable]
    //[System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = false)]
    //[DCSoft.Common.DCPublishAPI]
    public partial class FileOpenCommandParameter
    {
        /// <summary>
        /// 初始化对象
        /// </summary>
        public FileOpenCommandParameter()
        {
        }

        private string _FileSystemName = null;
        /// <summary>
        /// 指定的文件系统名称
        /// </summary>
        //[System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public string FileSystemName
        {
            get
            {
                return _FileSystemName;
            }
            set
            {
                _FileSystemName = value;
            }
        }

        private string _FileName = null;
        /// <summary>
        /// 指定的文件名
        /// </summary>
        //[System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
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

        private System.Text.Encoding _ContentEncoding = null;
        /// <summary>
        /// 指定的文档编码格式
        /// </summary>
        //[System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public System.Text.Encoding ContentEncoding
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

        private string _Format = null;
        /// <summary>
        /// 指定的文件格式
        /// </summary>
        //[System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public string Format
        {
            get
            {
                return _Format;
            }
            set
            {
                _Format = value;
            }
        }

        private TextReader _InputReader = null;
        /// <summary>
        /// 读取文件时使用的文本读取器
        /// </summary>
        //[System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public TextReader InputReader
        {
            get { return _InputReader; }
            set { _InputReader = value; }
        }

        private Stream _InputStream = null;
        /// <summary>
        /// 读取文件时使用的文件流对象
        /// </summary>
        //[System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public Stream InputStream
        {
            get { return _InputStream; }
            set { _InputStream = value; }
        }

        private string _Message = null;
        /// <summary>
        /// 相关的提示信息
        /// </summary>
        //[System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public string Message
        {
            get { return _Message; }
            set { _Message = value; }
        }
    }
}
