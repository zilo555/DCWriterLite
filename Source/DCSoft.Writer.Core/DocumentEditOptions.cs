using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Runtime.InteropServices;
using DCSoft.Common;

namespace DCSoft.Writer
{
    /// <summary>
    /// 编辑器选项控制
    /// </summary>
    /// <remarks>编制 袁永福</remarks>
    [Serializable()]
    //[TypeConverter(typeof(DCSoft.Common.TypeConverterSupportProperties))]
     
    [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = false )]
    //[DCSoft.Common.DCPublishAPI]
    public partial class DocumentEditOptions : System.ICloneable 
    {
        /// <summary>
        /// 初始化对象
        /// </summary>
        public DocumentEditOptions()
        {
        }

        public static void VoidMethod()
        {

        }


        private bool _CopyWithoutInputFieldStructure = false;
        /// <summary>
        /// 复制时去掉输入域层次结构
        /// </summary>
        [DefaultValue( false )]
        [System.Reflection.Obfuscation(Exclude = true , ApplyToMembers = true)]
        public bool CopyWithoutInputFieldStructure
        {
            get
            {
                return _CopyWithoutInputFieldStructure; 
            }
            set
            {
                _CopyWithoutInputFieldStructure = value; 
            }
        }

        private bool _CopyInTextFormatOnly = false;
        /// <summary>
        /// 仅仅采用纯文本格式复制内容,若为true，则只复制纯文本内容，而且忽略掉控件的CreationDataFormats属性设置.
        /// </summary>
        [DefaultValue(false)]
        [System.Reflection.Obfuscation(Exclude = true , ApplyToMembers = true)]
        public bool CopyInTextFormatOnly
        {
            get
            {
                return _CopyInTextFormatOnly; 
            }
            set
            {
                _CopyInTextFormatOnly = value; 
            }
        }

        private bool _CloneWithoutElementBorderStyle = true;
        /// <summary>
        /// 复制文档时清掉文档元素的边框样式，默认为true。
        /// </summary>
        [DefaultValue( true )]
        [System.Reflection.Obfuscation(Exclude = true , ApplyToMembers = true)]
        public bool CloneWithoutElementBorderStyle
        {
            get
            {
                return _CloneWithoutElementBorderStyle; 
            }
            set
            {
                _CloneWithoutElementBorderStyle = value; 
            }
        }
         
        private bool _ClearFieldValueWhenCopy = false;
        /// <summary>
        /// 在复制内容时清空输入域的内容，默认为false。
        /// </summary>
        [DefaultValue( false )]
        [System.Reflection.Obfuscation(Exclude = true , ApplyToMembers = true)]
        public bool ClearFieldValueWhenCopy
        {
            get
            {
                return _ClearFieldValueWhenCopy; 
            }
            set
            {
                _ClearFieldValueWhenCopy = value; 
            }
        }

        private bool _KeepTableWidthWhenInsertDeleteColumn = true;
        /// <summary>
        /// 在插入和删除表格列时是否保持表格的总宽度不变。默认true。
        /// </summary>
        [DefaultValue(true)]
        [System.Reflection.Obfuscation(Exclude = true , ApplyToMembers = true)]
        public bool KeepTableWidthWhenInsertDeleteColumn
        {
            get
            {
                return _KeepTableWidthWhenInsertDeleteColumn;
            }
            set
            {
                _KeepTableWidthWhenInsertDeleteColumn = value;
            }
        }


        private bool _FixSizeWhenInsertImage = true;
        /// <summary>
        /// 在插入图片时自动修正图片的宽度，使得图片元素的宽度不会超过容器客户区宽度，而且高度不超过标准页高。默认为true。
        /// </summary>
        [DefaultValue(true)]
        [System.Reflection.Obfuscation(Exclude = true , ApplyToMembers = true)]
        public bool FixSizeWhenInsertImage
        {
            get
            {
                return _FixSizeWhenInsertImage;
            }
            set
            {
                _FixSizeWhenInsertImage = value;
            }
        }

        private bool _FixWidthWhenInsertTable = true;
        /// <summary>
        /// 在插入表格时为容器元素修正表格的宽度，使得表格元素的宽度不会超过容器客户区宽度，默认true。
        /// </summary>
        [DefaultValue(true)]
        [System.Reflection.Obfuscation(Exclude = true , ApplyToMembers = true)]
        public bool FixWidthWhenInsertTable
        {
            get
            {
                return _FixWidthWhenInsertTable;
            }
            set
            {
                _FixWidthWhenInsertTable = value;
            }
        }

        private bool _TabKeyToFirstLineIndent = true;
        /// <summary>
        /// 是否使用Tab键来设置段落首行缩进，默认为true。
        /// </summary>
        [DefaultValue(true)]
        [System.Reflection.Obfuscation(Exclude = true , ApplyToMembers = true)]
        public bool TabKeyToFirstLineIndent
        {
            get
            {
                return _TabKeyToFirstLineIndent;
            }
            set
            {
                _TabKeyToFirstLineIndent = value;
            }
        }

        private DocumentValueValidateMode _ValueValidateMode = DocumentValueValidateMode.LostFocus;
        /// <summary>
        /// 文档数据校验模式，默认为LostFocus。
        /// </summary>
        [DefaultValue(DocumentValueValidateMode.LostFocus)]
        [System.Reflection.Obfuscation(Exclude = true , ApplyToMembers = true)]
        public DocumentValueValidateMode ValueValidateMode
        {
            get
            {
                return _ValueValidateMode;
            }
            set
            {
                _ValueValidateMode = value;
            }
        }

        /// <summary>
        /// 复制对象
        /// </summary>
        /// <returns>复制品</returns>
        object ICloneable.Clone()
        {
            DocumentEditOptions opt = (DocumentEditOptions)this.MemberwiseClone();
            return opt;
        }

        /// <summary>
        /// 复制对象
        /// </summary>
        /// <returns>复制品</returns>
        [System.Reflection.Obfuscation(Exclude = true , ApplyToMembers = true)]
        public DocumentEditOptions Clone()
        {
            return (DocumentEditOptions)((ICloneable)this).Clone();
        }
    }

    /// <summary>
    /// 文档数据校验模式
    /// </summary>
     
    [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
    public enum DocumentValueValidateMode
    {
        /// <summary>
        /// 禁止数据校验
        /// </summary>
        //[DCDescription(typeof(DocumentValueValidateMode),"None") ]
        None ,
        /// <summary>
        /// 实时的数据校验
        /// </summary>
        //[DCDescription(typeof(DocumentValueValidateMode), "Dynamic")]
        Dynamic,
        /// <summary>
        /// 当文本输入域失去输入焦点，也就是说光标离开输入域时才进行数据校验。
        /// </summary>
        //[DCDescription(typeof(DocumentValueValidateMode), "LostFocus")]
        LostFocus,
        /// <summary>
        /// 编辑器不自动进行数据校验，由应用程序编程调用来进行数据校验。
        /// </summary>
        //[DCDescription(typeof(DocumentValueValidateMode), "Program")]
        Program 
    }
     
}
