using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace DCSoft.Writer.Dom
{
    /// <summary>
    /// 文档内容当前样式信息对象
    /// </summary>
    /// <remarks>编制 袁永福</remarks>
    
    //[System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = false  )]
     
    public partial class CurrentContentStyleInfo
    {
        /// <summary>
        /// 初始化对象
        /// </summary>
        public CurrentContentStyleInfo()
        {
        }
        public void Dispose()
        {
            if (this._Content != null)
            {
                this._Content.Dispose();
                this._Content = null;
            }
            if (this._ContentStyleForNewString != null)
            {
                this._ContentStyleForNewString.Dispose();
                this._ContentStyleForNewString = null;
            }
            if (this._Paragraph != null)
            {
                this._Paragraph.Dispose();
                this._Paragraph = null;
            }
            if(this._Cell != null )
            {
                this._Cell.Dispose();
                this._Cell = null;
            }
        }
        private DocumentContentStyle _Content = null;
        /// <summary>
        /// 当前文档内容样式
        /// </summary>
        //[System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true  )]
        public DocumentContentStyle Content
        {
            get
            {
                return _Content; 
            }
            set
            {
                _Content = value;
                //_ContentStyleForNewString = null;
            }
        }

        private DocumentContentStyle _ContentStyleForNewString = null;
        /// <summary>
        /// 为新增文本而使用的文档内容样式
        /// </summary>
        //[System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true  )]
        public DocumentContentStyle ContentStyleForNewString
        {
            get
            {
                return _ContentStyleForNewString; 
            }
            set
            {
                _ContentStyleForNewString = value; 
            }
        }

        private DocumentContentStyle _Paragraph = null;
        /// <summary>
        /// 当前段落样式
        /// </summary>
        //[System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true  )]
        public DocumentContentStyle Paragraph
        {
            get
            {
                return _Paragraph; 
            }
            set
            {
                _Paragraph = value; 
            }
        }

        private DocumentContentStyle _Cell = null;
        /// <summary>
        /// 当前单元格样式
        /// </summary>
        //[System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true  )]
        public DocumentContentStyle Cell
        {
            get
            {
                return _Cell; 
            }
            set
            {
                _Cell = value; 
            }
        }

         
        public void Refresh(DomDocument document)
        {
            if (_Content == null)
            {
                _Content = (DocumentContentStyle)document.InternalCurrentStyle.Clone();
                _Content.ValueLocked = false;
                DomElement preElement = document.Content.SafeGet( document.Selection.AbsStartIndex -1 );
                DomElement endElement = document.Content.SafeGet(document.Selection.AbsEndIndex);
            }
            this.ContentStyleForNewString = (DocumentContentStyle)_Content.Clone();
            if (document.CurrentStyleElement() is DomObjectElement)
            {
                // DUWRITER5_0-1059 ，袁永福 2023-12-15
                //this.ContentStyleForNewString.Color = document.DefaultStyle.Color ;
                //this.ContentStyleForNewString.BackgroundColor = document.DefaultStyle.BackgroundColor;
            }
            if (_Content.HasVisibleBorder)
            {
                // 当前元素具有可见的边框，此时需要进行判断，尽量去掉边框
                DomElement element = document.CurrentStyleElement();
                if (element is DomCharElement)
                {
                    // 基准元素是字符元素，不去掉边框设置
                }
                else
                {
                    // 判断两边的元素是否也具有边框
                    DomElement nextElement = document.Content.GetNextElement(element);
                    if (nextElement != null
                        && nextElement is DomCharElement
                        && nextElement.RuntimeStyle.HasVisibleBorder)
                    {
                        // 存在下一个字符元素而且下一个字符元素也是有可见的边框，则不去掉边框设置
                    }
                    else
                    {
                        // 去掉边框的设置
                        _ContentStyleForNewString.BorderLeft = false;
                        _ContentStyleForNewString.BorderTop = false;
                        _ContentStyleForNewString.BorderRight = false;
                        _ContentStyleForNewString.BorderBottom = false;
                        _ContentStyleForNewString.BorderWidth = 0;
                        _ContentStyleForNewString.BorderColor = Color.Black;
                    }
                }
            }

            _Content.DisableDefaultValue = true;

            _ContentStyleForNewString.DisableDefaultValue = true;
            RefreshParagraph(document);
             
            DomTableCellElement cell = document.CurrentTableCell;
            if (cell == null)
            {
                _Cell = null;
            }
            else
            {
                _Cell = (DocumentContentStyle)cell.RuntimeStyle.CloneParent();
                _Cell.ValueLocked = false;
                _Cell.ValueLocked = true;
            }
            if (this._Cell != null)
            {
                this._Cell.ValueLocked = false ;
            }
            if( this._Content != null )
            {
                this._Content.ValueLocked = false ;
            }
            if (this._ContentStyleForNewString != null)
            {
                this._ContentStyleForNewString.ValueLocked = false ; 
            }
            if (this._Paragraph != null)
            {
                this._Paragraph.ValueLocked = false ;
            }
        }

        /// <summary>
        /// 刷新当前段落样式
        /// </summary>
        /// <param name="document"></param>
        internal void RefreshParagraph(DomDocument document )
        {
            var cpe = document.CurrentParagraphEOF;
            if (cpe == null)
            {
                _Paragraph = new DocumentContentStyle();
            }
            else
            {
                _Paragraph = (DocumentContentStyle)cpe.RuntimeStyle.CloneParent();
                _Paragraph.ValueLocked = false;
                _Paragraph.ValueLocked = true;
            }
        }

        /// <summary>
        /// 复制对象
        /// </summary>
        /// <returns>复制品</returns>
        public CurrentContentStyleInfo Clone()
        {
            CurrentContentStyleInfo info = new CurrentContentStyleInfo();
            if (this._Cell != null)
            {
                info._Cell = ( DocumentContentStyle ) this._Cell.Clone();
            }
            if( this._Content != null )
            {
                info._Content = ( DocumentContentStyle ) this._Content.Clone();
            }
            if (this._Paragraph != null)
            {
                info._Paragraph = (DocumentContentStyle)this._Paragraph.Clone();
            }
            if (this._ContentStyleForNewString != null)
            {
                info.ContentStyleForNewString = (DocumentContentStyle)this.ContentStyleForNewString.Clone();
            }
            return info;
        }
    }
     
}
