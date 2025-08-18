using System;
using System.Collections.Generic;
using System.Text;

namespace DCSoft.Writer.Commands
{
    /// <summary>
    /// 插入RTF文本的命令参数对象
    /// </summary>
    public class InsertRTFCommandParameter
    {
        /// <summary>
        /// 初始化对象
        /// </summary>
        /// <remarks>编写 袁永福</remarks>
        public InsertRTFCommandParameter()
        {
        }

        private string _RTFText = null;
        /// <summary>
        /// RTF文本
        /// </summary>
        //[System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public string RTFText
        {
            get
            {
                return _RTFText; 
            }
            set
            {
                _RTFText = value; 
            }
        }
    }
}
