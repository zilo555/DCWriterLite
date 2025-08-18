using System;
using System.Collections.Generic;
using System.Text;
using DCSoft.Writer.Dom ;

namespace DCSoft.Writer.Commands
{
    /// <summary>
    /// 插入字符串的命令参数对象
    /// </summary>
    /// <remarks>编写 袁永福</remarks>
    [Serializable]
     
    //[System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = false)]
    //[DCSoft.Common.DCPublishAPI]
    public partial class InsertStringCommandParameter
    {
        /// <summary>
        /// 初始化对象
        /// </summary>
        public InsertStringCommandParameter()
        {
        }

        private string _Text = null;
        /// <summary>
        /// 要插入的文本
        /// </summary>
        //[System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public string Text
        {
            get { return _Text; }
            set { _Text = value; }
        }

        private DocumentContentStyle _Style = null;
        /// <summary>
        /// 文本样式
        /// </summary>
        //[System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public DocumentContentStyle Style
        {
            get { return _Style; }
            set { _Style = value; }
        }

        private DocumentContentStyle _ParagraphStyle = null;
        /// <summary>
        /// 段落样式
        /// </summary>
        //[System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public DocumentContentStyle ParagraphStyle
        {
            get { return _ParagraphStyle; }
            set { _ParagraphStyle = value; }
        }

        //private bool _DeleteSelection = true;
        ///// <summary>
        ///// 插入文本时是否删除选中的文档内容
        ///// </summary>
        //public bool DeleteSelection
        //{
        //    get { return _DeleteSelection; }
        //    set { _DeleteSelection = value; }
        //}
    }
}
