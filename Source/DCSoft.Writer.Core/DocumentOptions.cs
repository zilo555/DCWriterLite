using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Configuration;
using System.Reflection;
using System.Runtime.InteropServices;
using System.IO;
using DCSoft.Common;
// // 


namespace DCSoft.Writer
{
    /// <summary>
    /// 文档选项对象
    /// </summary>
    /// <remarks>
    /// 编写 袁永福
    /// </remarks>
    [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = false)]
    public partial class DocumentOptions : ICloneable 
    {
        /// <summary>
        /// 初始化对象
        /// </summary>
        public DocumentOptions()
        {
        }
        [NonSerialized]
        private DCSoft.Writer.Controls.WriterViewControl _WriterControl = null;
        /// <summary>
        /// 设置对象所属的编辑器控件
        /// </summary>
        /// <param name="ctl"></param>
        internal void SetWriterControl(DCSoft.Writer.Controls.WriterViewControl ctl)
        {
            this._WriterControl = ctl;
        }
        
        private DocumentViewOptions _ViewOptions = new DocumentViewOptions();
        /// <summary>
        /// 文档视图方面的选项
        /// </summary>
        //[DCDescription(typeof(DocumentOptions), "ViewOptions")]
        //[DCDisplayName(typeof(DocumentOptions), "ViewOptions")]
       //////[Browsable(true)]
       //ReadOnly(true)]
        //[DCSoft.Common.DCPublishAPI]
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public DocumentViewOptions ViewOptions
        {
            get
            {
                if (_ViewOptions == null)
                {
                    _ViewOptions = new DocumentViewOptions();
                }
                return _ViewOptions; 
            }
            set
            {
                _ViewOptions = value; 
            }
        }

        private DocumentBehaviorOptions _BehaviorOptions = new DocumentBehaviorOptions();
        /// <summary>
        /// 编辑器行为方面的选项
        /// </summary>
        //[DCDescription(typeof(DocumentOptions), "BehaviorOptions")]
        //[DCDisplayName(typeof(DocumentOptions), "BehaviorOptions")]
       //////[Browsable(true)]
       //ReadOnly(true)]
        //[DCSoft.Common.DCPublishAPI]
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public DocumentBehaviorOptions BehaviorOptions
        {
            get
            {
                if (_BehaviorOptions == null)
                {
                    _BehaviorOptions = new DocumentBehaviorOptions();
                }
                return _BehaviorOptions; 
            }
            set
            {
                _BehaviorOptions = value; 
            }
        }

        private DocumentEditOptions _EditOptions = new DocumentEditOptions();
        /// <summary>
        /// 编辑方面的选项
        /// </summary>
        public DocumentEditOptions EditOptions
        {
            get
            {
                if (_EditOptions == null)
                {
                    _EditOptions = new DocumentEditOptions();
                }
                return _EditOptions; 
            }
            set 
            {
                _EditOptions = value; 
            }
        }

        

        /// <summary>
        /// 复制对象
        /// </summary>
        /// <returns>复制品</returns>
         
        object ICloneable.Clone()
        {
            DocumentOptions opt = new DocumentOptions();
            if (this._EditOptions != null)
            {
                opt._EditOptions = this._EditOptions.Clone();
            }
            if (this._ViewOptions != null)
            {
                opt._ViewOptions = this._ViewOptions.Clone();
            }
            if (this._BehaviorOptions != null)
            {
                opt._BehaviorOptions = this._BehaviorOptions.Clone();
            }
            return opt;
        }

        /// <summary>
        /// 复制对象
        /// </summary>
        /// <returns>复制品</returns>
         
        public DocumentOptions Clone()
        {
            return (DocumentOptions)((ICloneable)this).Clone();
        }
        internal void InnerDispose()
        {
            this._BehaviorOptions = null;
            this._EditOptions = null;
            this._ViewOptions = null;
            this._WriterControl = null;
        }
    }
}
