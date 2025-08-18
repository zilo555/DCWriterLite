using System;
using DCSoft.Printing;
using DCSoft.Writer.Serialization;
using System.ComponentModel;
using DCSoft.Drawing;
using DCSoft.Common;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using DCSoft.Writer.Data;
using System.Text;
using DCSoft.Writer.Controls;
namespace DCSoft.Writer.Dom
{
    public partial class DomDocument
    {
        
        /// <summary>
        /// 成员无效
        /// </summary>
        [System.Obsolete()]
        
        new private DocumentContentStyle RuntimeStyle { get { return null ; } }
    
        /// <summary>
        /// 成员无效
        /// </summary>
       ////[System.ComponentModel.Browsable(false)]
        [System.Obsolete()]
        
        new private DocumentContentStyle Style { get { return null; } set { throw new NotSupportedException("Document.Style"); } }
        /// <summary>
        /// 成员无效
        /// </summary>
       //////[Browsable(false)]
        [Obsolete()]
        public new int StyleIndex
        {
            get
            {
                return -1;
                //throw new NotSupportedException("Document.StyleIndex"); 
            }
            set
            {

            }
        }

        private static float _DefaultFontSize = 12;
        /// <summary>
        /// 默认字体大小
        /// </summary>
        public static float DefaultFontSize
        {
            get { return _DefaultFontSize; }
            set { _DefaultFontSize = value; }
        }

        /// <summary>
        /// 创建文档样式对象
        /// </summary>
        /// <returns>创建的对象</returns>
        public DocumentContentStyle CreateDocumentContentStyle()
        {
            DocumentContentStyle style = new DocumentContentStyle();
            //XDependencyObject.CopyValueFast(this.ContentStyles.Default, style, false);
            return style;
        }

        //private XFontValue _DefaultFont = new XFontValue();
        /// <summary>
        /// 默认字体
        /// </summary>
        public XFontValue DefaultFont
        {
            get
            {
                return this.DefaultStyle.Font;
            }
            set
            {
                this.DefaultStyle.Font = value;
            }
        }

        internal DocumentContentStyleContainer _ContentStyles = null ;//= new DocumentContentStyleContainer();

        /// <summary>
        /// 文档样式容器
        /// </summary>
        public DocumentContentStyleContainer ContentStyles
        {
            get
            {
                if (_ContentStyles == null)
                {
                    _ContentStyles = new DocumentContentStyleContainer();
                    _ContentStyles.Document = this;
                }
                return _ContentStyles;
            }
            set
            {
                _ContentStyles = value;
                if (_ContentStyles != null)
                {
                    _ContentStyles.Document = this;
                }
            }
        }


        /// <summary>
        /// 当前文档样式
        /// </summary>
        public DocumentContentStyle InternalCurrentStyle
        {
            get
            {
                DomElement element = this.CurrentStyleElement();
                if (element == null)
                {
                    return (DocumentContentStyle)this.ContentStyles.Default;
                }
                else if (element is DomCharElement && ((DomCharElement)element).IsBackgroundText)
                {
                    // 输入域背景文本
                    var style = (DocumentContentStyle)element.RuntimeStyle.Parent.Clone();
                    style.RemovePropertyValue("BackgroundColor");
                    style.RemoveBorderValues();
                    return style;
                }
                else if (element is DomFieldBorderElement)
                {
                    // 输入域边界元素
                    var style = (DocumentContentStyle)element.RuntimeStyle.Parent.Clone();
                    style.RemovePropertyValue("BackgroundColor");
                    style.RemoveBorderValues();
                    return style;
                }
                else
                {
                    return element.RuntimeStyle.Parent;
                }
                //if (this.CurrentElement == null)
                //{
                //    return (DocumentContentStyle)this.ContentStyles.Default;
                //}
                //else
                //{
                //    XTextElement element = this.CurrentElement;
                //    XTextContentElement ce = element.ContentElement;
                //    if (this.CurrentContentElement.Selection.Length == 0)
                //    {
                //        XTextElement preElement = ce.PrivateContent.GetPreElement(element);
                //        DocumentContentStyle result = element.RuntimeStyle;
                //        if (preElement != null
                //            && (preElement is XTextParagraphFlagElement) == false
                //            && (preElement is XTextTableElement) == false)
                //        {
                //            if (preElement.Parent is XTextFieldElementBase)
                //            {
                //                XTextFieldElementBase field = (XTextFieldElementBase)preElement.Parent;
                //                if (field.EndElement != preElement)
                //                {
                //                    result = preElement.RuntimeStyle;
                //                }
                //            }
                //            else
                //            {
                //                result = preElement.RuntimeStyle;
                //            }
                //        }
                //        return result;
                //    }
                //    else
                //    {
                //        element = this.CurrentContentElement.Selection.ContentElements[0];
                //        DocumentContentStyle rs = element.RuntimeStyle;
                //        return rs;
                //    }
                //}
            }
        }

        /// <summary>
        /// 获得当前文档内容样式的基准的元素对象
        /// </summary>
        internal DomElement CurrentStyleElement()
        {
            if (this.CurrentElement == null)
            {
                return null;
            }
            else
            {
                DomElement element = this.CurrentElement;
                DomContentElement ce = element.ContentElement;
                var sel = this.CurrentContentElement.Selection;
                if (sel.Length == 0)
                {
                    //if (element is XTextParagraphFlagElement)
                    //{
                    //    return element;
                    //}
                    DomElement preElement = ce.PrivateContent.GetPreElement(element);
                    if (element is DomParagraphFlagElement && preElement is DomParagraphFlagElement)
                    {
                        return element;
                    }
                    if (preElement != null
                        && (preElement is DomParagraphFlagElement) == false
                        && (preElement is DomTableElement) == false)
                    {
                        if (preElement.Parent is DomFieldElementBase)
                        {
                            DomFieldElementBase field = (DomFieldElementBase)preElement.Parent;
                            if (field.EndElement != preElement)
                            {
                                //element = preElement;
                            }
                            else if( element is DomParagraphFlagElement)
                            {
                                return preElement;
                            }
                        }
                        else
                        {
                            element = preElement;
                        }
                    }
                    return element;
                }
                //else if( Math.Abs( sel.Length) == 1 )
                //{
                //    element = sel.ContentElements[0];
                //    return element;
                //}
                else
                {
                    //return this.CurrentContentElement.Content.CurrentElement;
                    element = this.CurrentContentElement.Selection.ContentElements[0];
                    return element;
                }
            }

        }


        [NonSerialized]
        internal CurrentContentStyleInfo _CurrentStyleInfo = null;
        /// <summary>
        /// 编辑器中当前样式信息对象
        /// </summary>
        public CurrentContentStyleInfo CurrentStyleInfo
        {
            get
            {
                if (this._CurrentStyleInfo == null)
                {
                    this._CurrentStyleInfo = new CurrentContentStyleInfo();
                    this._CurrentStyleInfo.Refresh(this);
                }
                return this._CurrentStyleInfo;
            }
            set
            {
                if( value == null && this._PreserveCurrentStyleInfoOnce )
                {
                    this._PreserveCurrentStyleInfoOnce = false;
                    return;
                }
                this._CurrentStyleInfo = value;
            }
        }
        /// <summary>
        /// 保留当前文档样式状态信息一次。
        /// </summary>
        internal bool _PreserveCurrentStyleInfoOnce = false;
        /// <summary>
        /// 更新文档元素的样式编号
        /// </summary>
        /// <param name="rootElement"></param>
        /// <param name="refs"></param>
        private void CompressStyleList_UpdateSytleIndex(DomElementList list, int[] refs)
        {
            int len = refs.Length;
            for (int iCount = list.Count - 1; iCount >= 0; iCount--)
            {
                var element = list[iCount];
                int si = element._StyleIndex;
                if (si >= 0 && si < len)
                {
                    element.StyleIndex = refs[si];
                    if (element is DomFieldElementBase)
                    {
                        ((DomFieldElementBase)element).FixInnerElementStyleIndex();
                    }
                }
                else
                {
                    element.StyleIndex = -1;
                }
                if (element is DomContainerElement)
                {
                    var list2 = ((DomContainerElement)element)._Elements;
                    if (list2 != null && list2.Count > 0)
                    {
                        CompressStyleList_UpdateSytleIndex(list2, refs);
                    }
                }
            }
        }

        private void CompressStyleList_BuildMap(DomElementList list, int[] references)
        {
            int len = references.Length;
            var array = list.InnerGetArrayRaw();
            for (int iCount = list.Count - 1; iCount >= 0; iCount--)
            {
                var element = array[iCount];
                int si = element._StyleIndex;
                if (si >= 0 && si < len)
                {
                    references[si]++;
                }
                if (element is DomContainerElement)
                {
                    var list2 = ((DomContainerElement)element)._Elements;
                    if (list2 != null && list2.Count > 0)
                    {
                        CompressStyleList_BuildMap(list2, references);
                    }
                }
            }
        }

        /// <summary>
        /// 精简压缩文档样式列表,DCWriter内部使用。
        /// </summary>
        /// <remarks>
        /// 加载的原始文档中，或者在文档编辑过程中，可能会产生没有任何文档元素使用到的文档样式，
        /// 或者内容为空的文档样式。
        /// 此时可以使用本方法来删除没有用的样式，减少文档数据量。
        /// </remarks>
         
        public void CompressStyleList()
        {
            bool modified = false;
            // 压缩重复引用的图片数据
            this.ContentStyles.Styles.SetValueLocked(false);

            // 旧新样式编号映射表
            ContentStyleList styles = this.ContentStyles.Styles;
            int sCount = styles.Count;
            // 累计引用次数
            int[] references = new int[sCount];
            CompressStyleList_BuildMap(this.Elements, references);


            for (int index = 0; index < sCount; index++)
            {
                DocumentContentStyle style = (DocumentContentStyle)styles[index];
                if (references[index] == 0)
                {
                    // 出现从未引用的文档样式，准备删除
                    modified = true;
                }
                else
                {
                    int removed = 0;// XDependencyObject.RemoveDefaultValues(style, null);
                    //removed += XDependencyObject.RemoveDefaultValues(style, this.ContentStyles.Default);
                    if (removed > 0)
                    {
                        modified = true;
                    }
                    if (XDependencyObject.GetValueCount(style) == 0)
                    //&& string.IsNullOrEmpty( style.DefaultValuePropertyNames ) )
                    {
                        // 样式没有任何有效内容，则删除掉
                        modified = true;
                        references[index] = 0;
                    }
                }
            }//for
            if (modified)
            {
                // 清空运行时文档样式列表
                this.ContentStyles.ClearRuntimeStyleList();
                DocumentContentStyle nullStyle = new DocumentContentStyle();
                nullStyle.FontName = "嘿嘿，袁永福到此一游";
                // 建立映射列表,数组序号是旧映射编号，数组元素内容是新样式编号
                int step = 0;
                for (int iCount = 0; iCount < sCount; iCount++)
                {
                    if (references[iCount] == 0)
                    {
                        references[iCount] = -1;
                    }
                }
                for (int iCount = 0; iCount < sCount; iCount++)
                {
                    if (references[iCount] == -1)
                    {
                        step++;
                        references[iCount] = -1;
                        styles[iCount].Dispose();
                        styles[iCount] = nullStyle;
                    }
                    else if (styles[iCount] == nullStyle)
                    {
                        step++;
                    }
                    else
                    {
                        DocumentContentStyle style = (DocumentContentStyle)styles[iCount];
                        if (style != nullStyle)
                        {
                            references[iCount] = iCount - step;
                            // 合并内容完全一样的样式
                            for (int iCount2 = iCount + 1; iCount2 < sCount; iCount2++)
                            {
                                if (styles[iCount2] != nullStyle
                                    && style.EqualsStyleValue(styles[iCount2]))
                                {
                                    references[iCount2] = references[iCount];
                                    styles[iCount2].Dispose();
                                    styles[iCount2] = nullStyle;
                                }
                            }
                        }
                    }
                }//for

                // 更新文档元素的样式编号
                CompressStyleList_UpdateSytleIndex(this.Elements, references);

                //DomTreeNodeEnumerable enumer2 = new DomTreeNodeEnumerable(this);
                //enumer2.ExcludeParagraphFlag = false;
                //enumer2.ExcludeCharElement = false;
                //foreach( XTextElement element in enumer2 )
                //{
                //    //if (element.ElementInstanceIndex == 42529)
                //    //{
                //    //}
                //    int si = element.StyleIndex;
                //    if (si >= 0 && si < sCount)
                //    {
                //        element.StyleIndex = references[si];
                //        if (element is XTextFieldElementBase)
                //        {
                //            ((XTextFieldElementBase)element).FixInnerElementStyleIndex();
                //        }
                //    }
                //    else
                //    {
                //        element.StyleIndex = -1;
                //    }
                //}
                // 删除没用的样式
                for (int iCount = styles.Count - 1; iCount >= 0; iCount--)
                {
                    if (styles[iCount] == nullStyle)
                    {
                        styles.RemoveAt(iCount);
                    }
                }//for
                using (DCGraphics g = this.InnerCreateDCGraphics())
                {
                    foreach (DocumentContentStyle style in styles.FastForEach())
                    {
                        style.UpdateState(g);
                    }
                    ((DocumentContentStyle)this.ContentStyles.Default).UpdateState(g);
                }//using
            }//if
        }


        /// <summary>
        /// 获得指定元素所在段落的样式
        /// </summary>
        /// <param name="element">元素对象</param>
        /// <returns>段落样式对象</returns>
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public RuntimeDocumentContentStyle GetParagraphStyle(DomElement element)
        {
            if (element == null)
            {
                return ((DocumentContentStyle)this.ContentStyles.Default).MyRuntimeStyle;
            }
            DomParagraphFlagElement eof = this.GetParagraphEOFElement(element);
            if (eof == null)
            {
                return ((DocumentContentStyle)this.ContentStyles.Default).MyRuntimeStyle;
            }
            else
            {
                return eof.RuntimeStyle;
            }
        }

        /// <summary>
        /// 默认的文档样式
        /// </summary>
        public DocumentContentStyle DefaultStyle
        {
            get
            {
                return (DocumentContentStyle)this.ContentStyles.Default;
            }
            set
            {
                this.ContentStyles.Default = value;
            }
        }

        /// <summary>
        /// 设置默认字体
        /// </summary>
        /// <param name="font">默认字体</param>
        /// <param name="color">默认文本颜色</param>
        /// <param name="raiseEvent">若修改了文档设置则是否触发事件</param>
        /// <returns>对视图的影响</returns>
        public ContentEffects SetDefaultFont(XFontValue font, Color color, bool raiseEvent)
        {
            // 控件字体发生改变标记
            bool fc = this.ContentStyles.Default.Font.EqualsValue(font) == false;
            // 控件颜色发生改变标记
            bool cc = this.ContentStyles.Default.Color != color;
            if (fc == true)
            {
                this.ContentStyles.Default.Font = font.Clone();
                foreach( DocumentContentStyle style in this.ContentStyles.Styles)
                {
                    if (style.Font.EqualsValue(font))
                    {
                        style.Font = font;
                    }
                }
                this.ContentStyles.ClearRuntimeStyleList();
            }
            if (cc == true)
            {
                this.ContentStyles.Default.Color = color;
                this.ContentStyles.ClearRuntimeStyleList();
            }
            if( this._CurrentStyleInfo != null )
            {
                this._CurrentStyleInfo.Dispose();
                this._CurrentStyleInfo = null;
            }
            if (fc || cc)
            {
                //this.Modified = true;
                if (raiseEvent)
                {
                    if (this.Body.Elements.Count > 0)
                    {
                        this.OnDocumentContentChanged();
                        this.OnSelectionChanged();
                    }
                }
            }
            if (fc == true)
            {
                return ContentEffects.Layout;
            }
            else if (cc == true)
            {
                return ContentEffects.Display;
            }
            else
            {
                return ContentEffects.None;
            }
        }

    }
}
