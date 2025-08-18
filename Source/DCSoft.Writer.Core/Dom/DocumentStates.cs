using System;
using System.Collections.Generic;
using System.Text;
using DCSoft.Printing;

namespace DCSoft.Writer.Dom
{
    /// <summary>
    /// 文档状态
    /// </summary>
    public class DocumentStates
    {
        /// <summary>
        /// 文档状态
        /// </summary>
         
        public DocumentStates()
        {
        }

        private bool _Layouted = false ;
        /// <summary>
        /// 文档内容已经经过排版了
        /// </summary>
        //[DCSoft.Common.DCPublishAPI]
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public bool Layouted
        {
            get
            {
                return _Layouted; 
            }
            set
            {
                _Layouted = value; 
            }
        }

        //private bool _LowLevelDom = true ;
        ///// <summary>
        ///// 文档正处于低层次的DOM模式，内容没有进行过排版。此时文档不能显示，也无需作重做、撤销记录。
        ///// </summary>
        //public bool LowLevelDom
        //{
        //    get { return _LowLevelDom; }
        //    set { _LowLevelDom = value; }
        //}

        private DocumentRenderMode _RenderMode = DocumentRenderMode.Paint;
        /// <summary>
        /// 文档当前使用的呈现模式
        /// </summary>
        //[DCSoft.Common.DCPublishAPI]
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public DocumentRenderMode RenderMode
        {
            get
            {
                return _RenderMode; 
            }
            set
            {
                _RenderMode = value; 
            }
        }

        private bool _Printing = false;
        /// <summary>
        /// 文档正在打印中
        /// </summary>
        //[DCSoft.Common.DCPublishAPI]
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public bool Printing
        {
            get
            {
                return _Printing; 
            }
            set
            {
                _Printing = value; 
            }
        }

        private bool _PrintPreviewing = false;
        /// <summary>
        /// 文档正在生成打印预览内容
        /// </summary>
        //[DCSoft.Common.DCPublishAPI]
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public bool PrintPreviewing
        {
            get
            {
                return _PrintPreviewing; 
            }
            set
            {
                _PrintPreviewing = value; 
            }
        }

        //private readonly bool _ExecuteingGlobalLayout = false;
        /// <summary>
        /// 正在执行全局文档内容布局
        /// </summary>
        ////[DCSoft.Common.DCPublishAPI]
        //[System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        //public bool ExecuteingGlobalLayout
        //{
        //    get
        //    {
        //        return _ExecuteingGlobalLayout; 
        //    }
        //    set
        //    {
        //        _ExecuteingGlobalLayout = value; 
        //    }
        //}

        private bool _ExecutingUndo = false;
        /// <summary>
        /// 正在执行UNDO操作
        /// </summary>
        //[DCSoft.Common.DCPublishAPI]
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public bool ExecutingUndo
        {
            get { return _ExecutingUndo; }
            set { _ExecutingUndo = value; }
        }

        private bool _ExecutingRedo = false;
        /// <summary>
        /// 正在执行REDO操作
        /// </summary>
        //[DCSoft.Common.DCPublishAPI]
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public bool ExecutingRedo
        {
            get { return _ExecutingRedo; }
            set { _ExecutingRedo = value; }
        }

        private bool _GenerateLongBmp = false;
        /// <summary>
        /// 正在生成长图片
        /// </summary>
        //[DCSoft.Common.DCPublishAPI]
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public bool GenerateLongBmp
        {
            get { return _GenerateLongBmp; }
            set { _GenerateLongBmp = value; }
        }

        private bool _GenerateBmp = false;
        /// <summary>
        /// 正在生成文档内容图片
        /// </summary>
        //[DCSoft.Common.DCPublishAPI]
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public bool GenerateBmp
        {
            get { return _GenerateBmp; }
            set { _GenerateBmp = value; }
        }
    }
}
