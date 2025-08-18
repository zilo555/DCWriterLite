using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
//using System.Windows.Forms ;
using DCSoft.Common ;
using DCSoft.Writer.Dom;

namespace DCSoft.Writer.Commands
{
    /// <summary>
    /// 查找和替换命令参数
    /// </summary>
    //[Serializable]
    //[DCPublishAPI]
     
    //[System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = false  )]
    public partial class SearchReplaceCommandArgs
    {
        /// <summary>
        /// 初始化对象
        /// </summary>
        //[DCPublishAPI]
        public SearchReplaceCommandArgs()
        {
        }


        private string _SearchString = null;
        /// <summary>
        /// 要查找的字符串
        /// </summary>
        [DefaultValue( null)]
        //[DCPublishAPI]
        //[System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public string SearchString
        {
            get
            {
                return _SearchString; 
            }
            set
            {
                _SearchString = value; 
            }
        }


        private bool _SetSearchResultSelection = true;
        /// <summary>
        /// 是否选中查找结果
        /// </summary>
        [DefaultValue(true)]
        public bool SetSearchResultSelection
        {
            get
            {
                return _SetSearchResultSelection;
            }
            set
            {
                _SetSearchResultSelection = value;
            }
        }


        private DomContainerElement _SpecifyContainer = null;
        /// <summary>
        /// 指定要搜索的父元素
        /// </summary>
        public DomContainerElement SpecifyContainer
        {
            get
            {
                return _SpecifyContainer;
            }
            set
            {
                _SpecifyContainer = value;
            }
        }



        private Color _HighlightBackColor = Color.Yellow;
        /// <summary>
        /// 高亮所用的背景色
        /// </summary>
        public Color HighlightBackColor
        {
            get
            {
                return _HighlightBackColor;
            }
            set
            {
                _HighlightBackColor = value;
            }
        }




        private bool _EnableReplaceString = false;
        /// <summary>
        /// 是否启用替换模式
        /// </summary>
        [DefaultValue(false)]
        //[DCPublishAPI]
        //[System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public bool EnableReplaceString
        {
            get
            {
                return _EnableReplaceString; 
            }
            set
            {
                _EnableReplaceString = value; 
            }
        }

        private string _ReplaceString = null;
        /// <summary>
        /// 要替换的字符串
        /// </summary>
        [DefaultValue(null)]
        //[DCPublishAPI]
        //[System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public string ReplaceString
        {
            get
            {
                return _ReplaceString;
            }
            set
            {
                _ReplaceString = value; 
            }
        }

        private bool _Backward = true;
        /// <summary>
        /// True:向后查找；False:向前查找。
        /// </summary>
        [DefaultValue( false )]
        //[DCPublishAPI]
        //[System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public bool Backward
        {
            get
            {
                return _Backward; 
            }
            set 
            {
                _Backward = value; 
            }
        }
         
        private bool _IgnoreCase = false;
        /// <summary>
        /// 不区分大小写
        /// </summary>
       // [DCPublishAPI]
        //[System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public bool IgnoreCase
        {
            get 
            {
                return _IgnoreCase; 
            }
            set
            {
                _IgnoreCase = value; 
            }
        }

        private bool _SearchID = false;
        /// <summary>
        /// 搜索文档元素编号模式
        /// </summary>
        //[DCPublishAPI]
        //[System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public bool SearchID
        {
            get
            {
                return _SearchID; 
            }
            set
            {
                _SearchID = value; 
            }
        }

        private int _Result = 0;
        /// <summary>
        /// 替换的次数
        /// </summary>
        [DefaultValue( 0 )]
        //[DCPublishAPI]
        //[System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public int Result
        {
            get
            {
                return _Result; 
            }
            set
            {
                _Result = value; 
            }
        }

        private List<int> _MatchedIndexs = new List<int>();
        /// <summary>
        /// 搜索到的文本元素在文档内容中的序号
        /// </summary>
        //[DCPublishAPI]
        //[System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public List<int> MatchedIndexs
        {
            get
            {
                if( _MatchedIndexs == null )
                {
                    _MatchedIndexs = new List<int>();
                }
                return _MatchedIndexs; 
            }
        }

        /// <summary>
        /// 搜索到的字符串的个数
        /// </summary>
        //[DCPublishAPI]
        //[System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public int MatchedCount
        {
            get
            {
                if (_MatchedIndexs == null)
                {
                    return 0;
                }
                else
                {
                    return _MatchedIndexs.Count;
                }
            }
        }

        private bool _ExcludeBackgroundText = true;
        /// <summary>
        /// 忽略掉背景文字
        /// </summary>
        [DefaultValue( 0 )]
        //[DCPublishAPI]
        //[System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public bool ExcludeBackgroundText
        {
            get
            {
                return _ExcludeBackgroundText; 
            }
            set
            {
                _ExcludeBackgroundText = value; 
            }
        }

        private bool _LogUndo = true;
        /// <summary>
        /// 记录撤销操作信息
        /// </summary>
       // [DCPublishAPI]
        //[System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public bool LogUndo
        {
            get
            {
                return _LogUndo; 
            }
            set
            {
                _LogUndo = value; 
            }
        }

        /// <summary>
        /// 复制对象
        /// </summary>
        /// <returns>复制品</returns>
        //[DCPublishAPI]
        //[System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public SearchReplaceCommandArgs Clone()
        {
            return (SearchReplaceCommandArgs)this.MemberwiseClone();
        }
    }
}
