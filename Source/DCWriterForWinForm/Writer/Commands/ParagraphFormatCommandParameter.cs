using System;
using System.Collections.Generic;
using System.Text;
using DCSoft.Drawing;
using System.ComponentModel ;
using DCSoft.Writer.Dom ;

namespace DCSoft.Writer.Commands
{
    /// <summary>
    /// 段落格式命令参数对象
    /// </summary>
    /// <remarks>编制 袁永福</remarks>
    [Serializable ]
    //[System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = false)]
    //[DCSoft.Common.DCPublishAPI]
    public partial class ParagraphFormatCommandParameter
    {
        /// <summary>
        /// 初始化对象
        /// </summary>
        public ParagraphFormatCommandParameter()
        {
        }

        /// <summary>
        /// 从文档样式中读取设置
        /// </summary>
        /// <param name="style">文档样式</param>
        //[System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public void Read(ContentStyle style)
        {
            if (style == null)
            {
                throw new ArgumentNullException("style");
            }
            this.LineSpacing = style.LineSpacing;
            this.LeftIndent = style.LeftIndent;
            this.LineSpacingStyle = style.LineSpacingStyle;
            this.SpacingAfter = style.SpacingAfterParagraph;
            this.SpacingBefore = style.SpacingBeforeParagraph;
            this.FirstLineIndent = style.FirstLineIndent;
            this.OutlineLevel = style.ParagraphOutlineLevel;
            this.ParagraphMultiLevel = style.ParagraphMultiLevel;
            this.ListStyle = style.ParagraphListStyle;
        }

        /// <summary>
        /// 将设置写入到文档样式中
        /// </summary>
        /// <param name="style">文档样式</param>
        //[System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public void Save(ContentStyle style)
        {
            if (style == null)
            {
                throw new ArgumentNullException("style");
            }
            style.FirstLineIndent = this.FirstLineIndent;
            style.LeftIndent = this.LeftIndent;
            style.LineSpacing = this.LineSpacing;
            style.LineSpacingStyle = this.LineSpacingStyle;
            style.SpacingAfterParagraph = this.SpacingAfter;
            style.SpacingBeforeParagraph = this.SpacingBefore;
            style.ParagraphOutlineLevel = this.OutlineLevel;
            style.ParagraphMultiLevel = this.ParagraphMultiLevel;
            style.ParagraphListStyle = this.ListStyle;
        }

        private ParagraphListStyle _ListStyle = ParagraphListStyle.None;
        /// <summary>
        /// 段落列表样式
        /// </summary>
        //[System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public ParagraphListStyle ListStyle
        {
            get { return _ListStyle; }
            set { _ListStyle = value; }
        }

        private bool _ParagraphMultiLevel = false;
        /// <summary>
        /// 多层次段落列表
        /// </summary>
        //[System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public bool ParagraphMultiLevel
        {
            get { return _ParagraphMultiLevel; }
            set { _ParagraphMultiLevel = value; }
        }
        private int _OutlineLevel = -1;
        /// <summary>
        /// 段落大纲等级
        /// </summary>
        //[System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public int OutlineLevel
        {
            get 
            {
                return _OutlineLevel; 
            }
            set
            {
                _OutlineLevel = value; 
            }
        }
        private LineSpacingStyle _LineSpacingStyle = LineSpacingStyle.SpaceSingle;
        /// <summary>
        /// 行间距样式
        /// </summary>
        [DefaultValue( LineSpacingStyle.SpaceSingle )]
        //[System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public LineSpacingStyle LineSpacingStyle
        {
            get
            {
                return _LineSpacingStyle; 
            }
            set
            {
                _LineSpacingStyle = value; 
            }
        }

        private float _LineSpacing = 0f;
        /// <summary>
        /// 行间距
        /// </summary>
        [DefaultValue( 0f )]
        //[System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public float LineSpacing
        {
            get
            {
                return _LineSpacing; 
            }
            set
            {
                _LineSpacing = value; 
            }
        }

        private float _SpacingBefore = 0f;
        /// <summary>
        /// 段落前间距
        /// </summary>
        [DefaultValue( 0f )]
        //[System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public float SpacingBefore
        {
            get
            {
                return _SpacingBefore; 
            }
            set
            {
                _SpacingBefore = value; 
            }
        }

        private float _SpacingAfter = 0f;
        /// <summary>
        /// 段落后间距
        /// </summary>
        [DefaultValue( 0f )]
        //[System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public float SpacingAfter
        {
            get
            {
                return _SpacingAfter; 
            }
            set
            {
                _SpacingAfter = value; 
            }
        }

        private float _FirstLineIndent = 0f;
        /// <summary>
        /// 首行缩进量
        /// </summary>
        [DefaultValue( 0f )]
        //[System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public float FirstLineIndent
        {
            get
            {
                return _FirstLineIndent; 
            }
            set
            {
                _FirstLineIndent = value; 
            }
        }

        private float _LeftIndent = 0f;
        /// <summary>
        /// 段落左边缩进量
        /// </summary>
        [DefaultValue( 0f )]
        //[System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public float LeftIndent
        {
            get
            {
                return _LeftIndent; 
            }
            set
            {
                _LeftIndent = value; 
            }
        }
    }
}
