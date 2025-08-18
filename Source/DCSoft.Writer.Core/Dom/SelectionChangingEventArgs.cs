using System;
using System.Collections.Generic;
using System.Text;
using DCSoft.Writer.Controls ;
using System.Runtime.InteropServices;

namespace DCSoft.Writer.Dom
{
    /// <summary>
    /// 选择区域正在发生改变事件委托类型
    /// </summary>
    /// <param name="eventSender">参数</param>
    /// <param name="args">参数</param>
    public delegate void SelectionChangingEventHandler(
            object eventSender ,
            SelectionChangingEventArgs args );

    /// <summary>
    /// 选择区域正在发生改变事件参数类型
    /// </summary>
    /// <remarks> 编制 袁永福</remarks>
    public partial class SelectionChangingEventArgs  
    {
        /// <summary>
        /// 初始化对象
        /// </summary>
         
        public SelectionChangingEventArgs()
        {
        }
        ///// <summary>
        ///// 初始化对象
        ///// </summary>
        ///// <param name="oldSelectionIndex">旧的选择区域开始位置</param>
        ///// <param name="oldSelectionLength">旧的选择区域长度</param>
        ///// <param name="newSelectionIndex">新的选择区域开始位置</param>
        ///// <param name="newSelectionLength">新的选择区域长度</param>
        //public SelectionChangingEventArgs(
        //    int oldSelectionIndex, 
        //    int oldSelectionLength,
        //    int newSelectionIndex,
        //    int newSelectionLength)
        //{
        //    _OldSelectionIndex = oldSelectionIndex;
        //    _OldSelectionLength = oldSelectionLength;
        //    _NewSelectionIndex = newSelectionIndex;
        //    _NewSelectionLength = newSelectionLength;
        //}

        private DomDocument _Documnent = null;
        /// <summary>
        /// 文档对象
        /// </summary>
        [System.Text.Json.Serialization.JsonIgnore]
        public DomDocument Documnent
        {
            get { return _Documnent; }
            set { _Documnent = value; }
        }

        private bool _OldLineEndFlag = false;
        /// <summary>
        /// 旧的行尾标记
        /// </summary>
        //[DCSoft.Common.DCPublishAPI]
        //[System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public bool OldLineEndFlag
        {
            get { return _OldLineEndFlag; }
            set { _OldLineEndFlag = value; }
        }

        private int _OldSelectionIndex = 0;
        /// <summary>
        /// 旧的选择区域开始位置
        /// </summary>
        //[DCSoft.Common.DCPublishAPI]
        //[System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public int OldSelectionIndex
        {
            get { return _OldSelectionIndex; }
            set { _OldSelectionIndex = value; }
        }

        private int _OldSelectionLength = 0;
        /// <summary>
        /// 旧的选择区域长度
        /// </summary>
        //[DCSoft.Common.DCPublishAPI]
        //[System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public int OldSelectionLength
        {
            get { return _OldSelectionLength; }
            set { _OldSelectionLength = value; }
        }

        //private int _OldNativeSelectionIndex = 0;
        ///// <summary>
        ///// 旧的原始选择区域位置
        ///// </summary>
        //public int OldNativeSelectionIndex
        //{
        //    get { return _OldNativeSelectionIndex; }
        //    set { _OldNativeSelectionIndex = value; }
        //}
        //private int _OldNativeSelectionLength = 0;
        ///// <summary>
        ///// 旧的原始选择区域长度
        ///// </summary>
        //public int OldNativeSelectionLength
        //{
        //    get { return _OldNativeSelectionLength; }
        //    set { _OldNativeSelectionLength = value; }
        //}

        private bool _NewLineEndFlag = false;
        /// <summary>
        /// 新的行尾标记
        /// </summary>
        //[DCSoft.Common.DCPublishAPI]
        //[System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public bool NewLineEndFlag
        {
            get { return _NewLineEndFlag; }
            set { _NewLineEndFlag = value; }
        }

        private int _NewSelectionIndex = 0;
        /// <summary>
        /// 新的选择区域开始位置
        /// </summary>
        //[DCSoft.Common.DCPublishAPI]
        //[System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public int NewSelectionIndex
        {
            get { return _NewSelectionIndex; }
            set { _NewSelectionIndex = value; }
        }

        private int _NewSelectionLength = 0;
        /// <summary>
        /// 新的选择区域长度
        /// </summary>
        //[DCSoft.Common.DCPublishAPI]
        //[System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public int NewSelectionLength
        {
            get { return _NewSelectionLength; }
            set { _NewSelectionLength = value; }
        }

        //private int _NewNativeSelectionIndex = 0;
        ///// <summary>
        ///// 新的原始选择区域位置
        ///// </summary>
        //public int NewNativeSelectionIndex
        //{
        //    get { return _NewNativeSelectionIndex; }
        //    set { _NewNativeSelectionIndex = value; }
        //}
        //private int _NewNativeSelectionLength = 0;
        ///// <summary>
        ///// 新的原始选择区域长度
        ///// </summary>
        //public int NewNativeSelectionLength
        //{
        //    get { return _NewNativeSelectionLength; }
        //    set { _NewNativeSelectionLength = value; }
        //}


        private bool _Cancel = false;
        /// <summary>
        /// 用户取消操作
        /// </summary>
        //[DCSoft.Common.DCPublishAPI]
        //[System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public bool Cancel
        {
            get { return _Cancel; }
            set { _Cancel = value; }
        }
    }
}
