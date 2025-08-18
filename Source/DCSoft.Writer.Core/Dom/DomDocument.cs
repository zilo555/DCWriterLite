//
//                       _oo0oo_
//                      o8888888o
//                      88" . "88
//                      (| -_- |)
//                      0\  =  /0
//                    ___/`---'\___
//                  .' \\|     |// '.
//                 / \\|||  :  |||// \
//                / _||||| -:- |||||- \
//               |   | \\\  -  /// |   |
//               | \_|  ''\---/''  |_/ |
//               \  .-\__  '-'  ___/-. /
//             ___'. .'  /--.--\  `. .'___
//          ."" '<  `.___\_<|>_/___.' >' "".
//         | | :  `- \`.;`\ _ /`;.`/ - ` : | |
//         \  \ `_.   \_ __\ /__ _/   .-` /  /
//     =====`-.____`.___ \_____/___.-`___.-'=====
//                       `=---='
//
//
//     ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//
//               佛祖保佑         永无BUG
//
//
//
global using System.Threading;
global using System.Threading.Tasks;
global using System.Collections.Generic;

using DCSoft.Common;
using DCSoft.Drawing;
using DCSoft.Printing;
using DCSoft.Writer.Controls;
using DCSoft.Writer.Data;
using System;
using System.Collections;
using System.ComponentModel;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
namespace DCSoft.Writer.Dom
{

#if ! RELEASE
    [System.Diagnostics.DebuggerDisplay("<{DocInstanceIndex}>Document:{Title}")]
#endif
    public sealed partial class DomDocument : DomContainerElement
    {
        public static readonly string TypeName_XTextDocument = "XTextDocument";
        public override string TypeName => TypeName_XTextDocument;

        /// <summary>
        /// 系统处于调试模式
        /// </summary>
        internal static bool _DebugMode = false;

        internal static bool IsDrawingSystemImage = false;

        /// <summary>
        /// 正在加载XML文档
        /// </summary>
        public static bool InnerReadingXML = false;


        /// <summary>
        /// 创建对象,该对象具有最基本的DOM结构。
        /// </summary>
        /// <returns>文档对象</returns>
        public static DomDocument CreateWithBaseDOM()
        {
            return new DomDocument(2);
        }

        /// <summary>
        /// 内部的创建对象
        /// </summary>
        /// <param name="v">创建模式</param>
        internal DomDocument( int v )
        {
            if( v == 1 )
            {
                // 没有任何实质内容
            }
            else if( v == 2 )
            {
                this._OwnerDocument = this;
                DomElementList elements = this.Elements;
                elements.Capacity = 3;
                elements.FastAdd2(new DomDocumentHeaderElement());
                elements.FastAdd2(new DomDocumentBodyElement());
                elements.FastAdd2(new DomDocumentFooterElement());
                this._ContentStyles = new DocumentContentStyleContainer();
                this._ContentStyles.Default.FontSize = _DefaultFontSize;
                var style = new DocumentContentStyle();
                style.Align = DocumentContentAlignment.Center;
                style.FontSize = _DefaultFontSize;
                this._ContentStyles.Styles.Add(style);

                foreach (DomDocumentContentElement item in elements)
                {
                    item.ParagraphTreeInvalidte = false;
                    item.InnerSetOwnerDocumentParentRaw(this, this);
                    var p = new DomParagraphFlagElement();
                    p.AutoCreate = true;
                    p.InnerSetOwnerDocumentParentRaw(this, item);
                    item.Elements.FastAdd2(p);
                    if( item is DomDocumentHeaderElement)
                    {
                        p.StyleIndex = 0;
                    }
                    //item.IsEmpty = true;
                }
                {
                    this._UndoList = new Undo.XTextDocumentUndoList(this);
                }
                
                this._Info = new DocumentInfo();
                this._Info.CreationTime = this.GetNowDateTime();
                this._Info.LastModifiedTime = this._Info.CreationTime;
                this._Info.EditMinute = 0;
            }
        }
        internal override IList<DomElement> GetCompressedElements()
        {
            return this._Elements;
        }

        /// <summary>
        /// 是否处于免费模式。免费模式只能打印，不能编辑和显示
        /// </summary>
        internal bool IsFreeMode()
        {
            if (this._States != null)
            {
                if (this._States.Printing || this._States.PrintPreviewing)
                {
                    // 2023-2-17 yyf 打印模式免费
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 初始化对象
        /// </summary>
        public DomDocument()
        {
            //if (_WebApplicationMode == false)
            //{
            //    WriterUtilsInner.RunDCWriterPublishStart();
            //}
            this._OwnerDocument = this;
            this._Parent = null;
            //base.InnerSetOwnerDocumentParentRaw(this, this);
            this.CheckRootElement();
            //base.OwnerDocument = this;
            //base.Parent = this;
            //this.AppendChildElement(new XTextDocumentHeaderElement());
            //this.AppendChildElement(new XTextDocumentBodyElement());
            //this.AppendChildElement(new XTextDocumentFooterElement());
            {
                _UndoList = new Undo.XTextDocumentUndoList(this);
            }
            //this.ContentStyles.Default.Spacing = 1;
            //this.ContentStyles.Default.LineSpacing = 4;
            //this.ContentStyles.Default.SpacingBeforeParagraph = 9f;
            this.ContentStyles.Default.FontSize = _DefaultFontSize ;
            //this.Elements.Add(this.CreateParagraphEOF());
           
        }

        /// <summary>
        /// 判断指定元素是否是本元素的父节点或者更高层次的父节点。
        /// </summary>
        /// <param name="parentElement">要判断的元素</param>
        /// <returns>是否是父节点或者更高级的父节点</returns>
        public override bool IsParentOrSupParent(DomElement parentElement)
        {
            return false;
        }

        [NonSerialized]
        private DomReadyStates _ReadyState = DomReadyStates.UnInitialized;
        /// <summary>
        /// 文档加载状态
        /// </summary>
        public DomReadyStates ReadyState
        {
            get
            {
                return _ReadyState; 
            }
            //set
            //{
            //    _ReadyState = value; 
            //}
        }
        /// <summary>
        /// 设置文档加载状态
        /// </summary>
        /// <param name="state"></param>
         
        public void SetReadyState(DomReadyStates state)
        {
            this._ReadyState = state;
        }
        /// <summary>
        /// 应用BehaviorOptions.RemoveLastParagraphFlagWhenInsertDocument选项。DCWriter内部使用。
        /// </summary>
        /// <param name="list"></param>
         
        public void ApplyRemoveLastParagraphFlagWhenInsertDocument(DomElementList list)
        {
            if ( list != null && this.GetDocumentBehaviorOptions().RemoveLastParagraphFlagWhenInsertDocument)
            {
                if (list.LastElement is DomParagraphFlagElement)
                {
                    list.RemoveAt(list.Count - 1);
                }
            }
        }

        [NonSerialized]
        private TypedElementListBuffer _TypedElements = null;
        /// <summary>
        /// DCWriter内部使用，强类型的元素列表查询结果缓存区
        /// </summary>
        //////[Browsable(false)]
        //[XmlIgnore]
        // 
        ////[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public TypedElementListBuffer TypedElements
        {
            get
            {
                if (_TypedElements == null)
                {
                    this._TypedElements = new TypedElementListBuffer();
                }
                return _TypedElements;
            }
        }


        /// <summary>
        /// 清空一些缓存的数据
        /// </summary>
        internal void ClearBufferForSimpleTextMode()
        {

            if (this._HighlightManager != null)
            {
                this._HighlightManager.Clear();
            }
            if (this._TypedElements != null)
            {
                this._TypedElements.Clear();
            }
 
        }

        /// <summary>
        /// 清空一些缓存的数据
        /// </summary>
        internal void ClearBuffer()
        {
            //if (_PreserveOnce_ExpressionExecuter)
            //{
            //    _PreserveOnce_ExpressionExecuter = false;
            //}
            if ( this._HighlightManager != null )
            {
                this._HighlightManager.Clear();
            }
            if (this._TypedElements != null)
            {
                this._TypedElements.Clear();
            }
        }
        
        internal bool EnabledFixDomState = true ;
        internal bool _Invalidate_FixDomState = false;
        /// <summary>
        /// 文档状态无效，需要重新刷新。
        /// </summary>
        public void InvalidateFixDomState()
        {
            this._Invalidate_FixDomState = true;
        }

        /// <summary>
        /// 带检查状态的修正文档对象模型
        /// </summary>
        public void FixDomStateWithCheckInvalidateFlag()
        {
            if (_Invalidate_FixDomState)
            {
                this.FixDomState();
            }
        }
        internal int _DefaultHeaderPFStyleIndex = -1;
        /// <summary>
        /// 修正文档对象模型
        /// </summary>
        public override void FixDomState()
        {
            this.ResetChildElementStats();
            this._Parent = null;
            this._Invalidate_FixDomState = false;
            ClearBuffer();
            if (this.EnabledFixDomState == false )
            {
                return;
            }
            this.CacheOptions();
            if( this._Elements == null )
            {
                this._Elements = new DomElementList(this);
            }
            //float tick = CountDown.GetTickCountFloat();
            //if( this._ElementsForSerialize != null && this._ElementsForSerialize.Count > 0 )
            //{
            //    this._ElementsForSerialize.MoveDataTo(this._Elements);
            //    //this._Elements.Clear();
            //    //this._Elements.AddRange(this._ElementsForSerialize);
            //    //this._ElementsForSerialize.Clear();
            //    this._ElementsForSerialize = null;
            //}
            var df = this.ContentStyles.Default;
            if(string.IsNullOrEmpty(df.FontName ))
            {
                df.FontName = XFontValue.DefaultFontName;
            }
            this.CheckRootElement();
            //base.FixDomState();
            //XTextDocumentBodyElement body = this.Body;
            //body.Elements.Clear();
            foreach (DocumentContentStyle item in this.ContentStyles.Styles)
            {
                var fn = item.FontName;
                if (fn != XFontValue.DefaultFontName
                    && TrueTypeFontSnapshort.GetInstance(fn, item.FontStyle) == null)
                {
                    // 遇到不支持的字体
                    item.FontName = XFontValue.DefaultFontName;
                    item.SetMyRuntimeStyle(null);
                }
            }
            var dpst = new DocumentContentStyle();
            dpst.Align = DocumentContentAlignment.Center;
            this._DefaultHeaderPFStyleIndex = this.ContentStyles.GetStyleIndex(dpst);
            foreach( DomDocumentContentElement dce in this.Elements.FastForEach() )
            {
                dce.InnerSetOwnerDocumentParentRaw(this, this);
                dce.FixDomState();
            }
            this.ContentStyles.Document = this;
            this.ClearCachedOptions();
        }


        private int _LastSearchStartPosition = -1;
        /// <summary>
        /// 上一次开始搜索的起始位置
        /// </summary>
        public int LastSearchStartPosition
        {
            get
            {
                return _LastSearchStartPosition; 
            }
            set
            {
                _LastSearchStartPosition = value; 
            }
        }
        

#region 一些属性无效 ***********************************************

        
        /// <summary>
        /// 成员无效
        /// </summary>
        [Obsolete()]
        new private DomTableCellElement OwnerCell { get { return null; } }
        /// <summary>
        /// 成员无效
        /// </summary>
        [System.Obsolete()]
        
        new private DomDocumentContentElement DocumentContentElement { get { return null; } }
        /// <summary>
        /// 成员无效
        /// </summary>
        [System.Obsolete()]
        
        new private DomContainerElement ContentElement { get { return null; } }
        /// <summary>
        /// 成员无效
        /// </summary>
        [System.Obsolete()]
        
        new private DomLine OwnerLine { get { return null; } }
        /// <summary>
        /// 成员无效
        /// </summary>
        [System.Obsolete()]
        
        new private DomParagraphFlagElement OwnerParagraphEOF { get { return null; } }
        /// <summary>
        /// 成员无效
        /// </summary>
        [System.Obsolete()]
        
        new private DomElement PreviousElement { get { return null; } }
            
        /// <summary>
        /// 成员无效
        /// </summary>
        [System.Obsolete()]
        
        new private int ViewIndex { get { return 0; } }
        /// <summary>
        /// 成员无效
        /// </summary>
        [System.Obsolete()]
        
        new private int ColumnIndex { get { return 0; } }
        /// <summary>
        /// 成员无效
        /// </summary>
        [System.Obsolete()]
        
        new private int ElementIndex { get { return 0; } }

#endregion
         

        /// <summary>
        /// 添加子元素
        /// </summary>
        /// <param name="element">新添加的元素</param>
        /// <returns>操作是否成功</returns>
        public override bool AppendChildElement(DomElement element)
        {
            if (element is DomDocumentContentElement)
            {
                return base.AppendChildElement(element);
            }
            else
            {
                // 添加到文档正文中
                return this.Body.AppendChildElement(element);
            }
        }
        /// <summary>
        ///  分配新的文档元素编号
        /// </summary>
        /// <param name="prefix">编号前缀</param>
        /// <param name="element">元素对象</param>
        /// <returns>是否修改了元素ID属性值</returns>
        public bool AllocElementID(string prefix, DomElement element)
        {
            if (element != null)
            {
                if (string.IsNullOrEmpty(prefix))
                {
                    prefix = element.ElementIDPrefix();
                }
                if (string.IsNullOrEmpty(element.ID))
                {
                    element.ID = AllocElementID(prefix, element.GetType());
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 分配新的文档元素编号
        /// </summary>
        /// <param name="prefix">编号前缀</param>
        /// <param name="elementType">文档元素类型</param>
        /// <returns>分配的编号</returns>
        public string AllocElementID(string prefix, Type elementType)
        {
            if (elementType == null)
            {
                throw new ArgumentNullException("elementType");
            }
            if (string.IsNullOrEmpty(prefix))
            {
                prefix = "e";
            }
            DomElementList elements = GetElementsByType(elementType);
            int index = elements.Count + 1;
            while (true)
            {
                string result = prefix + index.ToString();
                bool match = false;
                foreach (DomElement element in elements)
                {
                    if (string.Compare(element.ID, result, true) == 0)
                    {
                        match = true;
                        index++;
                        break;
                    }
                }
                if (match == false)
                {
                    return result;
                }
            }
            //return prefix + index.ToString();
        }


        private static readonly string _CurrentEditorVersionString = typeof(DomDocument).Assembly.GetName().Version.ToString();
        /// <summary>
        /// 当前编辑器组件版本号
        /// </summary>
        public static string CurrentEditorVersionString
        {
            get
            {
                return _CurrentEditorVersionString;
            }
        }
        private string _EditorVersionString = null;
        /// <summary>
        /// 编辑器版本号
        /// </summary>
        public string EditorVersionString
        {
            get
            {
                return _EditorVersionString;
            }
            set
            {
                _EditorVersionString = value;
            }
        }

        private DocumentContentSourceTypes _ContentSourceType = DocumentContentSourceTypes.None;
        /// <summary>
        /// 文档内容来源
        /// </summary>
        public DocumentContentSourceTypes ContentSourceType
        {
            get
            {
                return _ContentSourceType; 
            }
            set
            {
                //if (_ContentSourceType != value)
                {
                    _ContentSourceType = value;
                }
            }
        }         

        [NonSerialized()]
        internal DocumentOptions _Options = null;
        /// <summary>
        /// 文档相关的配置项目,该对象不参与二进制和XML序列化.
        /// </summary>
        public DocumentOptions Options
        {
            get
            {

                if (this._EditorControl != null)
                {
                    if (this._EditorControl.DocumentOptions != null)
                    {
                        return this._EditorControl.DocumentOptions;
                    }
                }
                if (_Options == null)
                {
                    _Options = new DocumentOptions();
                    //_Options.LoadConfig();
                }
                return _Options;
            }
            set
            {
                if (this._Options != value)
                {
                    this._Options = value;
                    this.ClearCachedOptions();
                }
            }
        }
        /// <summary>
        /// 缓存文档选项，和ClearCachedOptions()成对使用。
        /// </summary>
        public void CacheOptions()
        {
            this._Cached_Options = this.Options;
            this._Cached_BehaviorOptions = this._Cached_Options.BehaviorOptions;
            this._Cached_EditOptions = this._Cached_Options.EditOptions;
            this._Cached_ViewOptions = this._Cached_Options.ViewOptions;
        }
        internal void CheckCacheOptions()
        {
            if( this._Cached_Options == null )
            {
                CacheOptions();
            }
        }
        /// <summary>
        /// 清除缓存的文档选项,和CacheOptions()成对使用。
        /// </summary>
        public void ClearCachedOptions()
        {
            this._Cached_Options = null;
            this._Cached_BehaviorOptions = null;
            this._Cached_EditOptions = null;
            this._Cached_ViewOptions = null;
        }

        private DocumentViewOptions _Cached_ViewOptions = null;
        /// <summary>
        /// 获得视图文档选项
        /// </summary>
        /// <returns>视图文档选项</returns>
        public override DocumentViewOptions GetDocumentViewOptions()
        {
            if( _Cached_ViewOptions == null )
            {
                return this.Options.ViewOptions;
            }
            else
            {
                return _Cached_ViewOptions;
            }
        }

        private DocumentBehaviorOptions _Cached_BehaviorOptions = null;
        /// <summary>
        /// 获得文档行为配置选项
        /// </summary>
        /// <returns></returns>
        public override DocumentBehaviorOptions GetDocumentBehaviorOptions()
        {
            if( this._Cached_BehaviorOptions == null )
            {
                return this.Options.BehaviorOptions;
            }
            else
            {
                return this._Cached_BehaviorOptions;
            }
        }
        private DocumentEditOptions _Cached_EditOptions = null;
        /// <summary>
        /// 获得编辑配置选项
        /// </summary>
        /// <returns></returns>
        public override DocumentEditOptions GetDocumentEditOptions()
        {
            if( this._Cached_EditOptions == null )
            {
                return this.Options.EditOptions;
            }
            else
            {
                return this._Cached_EditOptions;
            }
        }

        /// <summary>
        /// 缓存的文档选项对象
        /// </summary>
        private DocumentOptions _Cached_Options = null;
        /// <summary>
        /// 获得文档选项
        /// </summary>
        /// <returns></returns>
        public override DocumentOptions GetDocumentOptions()
        {
            if (_Cached_Options == null)
            {
                return this.Options;
            }
            else
            {
                return this._Cached_Options;
            }
        }


        private DocumentInfo _Info = null;
        /// <summary>
        /// 文档相关信息
        /// </summary>
        public DocumentInfo Info
        {
            get
            {
                if( _Info == null )
                {
                    _Info = new DocumentInfo();
                }
                return _Info;
            }
            set
            {
                _Info = value;
            }
        }

        /// <summary>
        /// 文档标题
        /// </summary>
        public string Title
        {
            get
            {
                if (this.Info == null)
                {
                    return null;
                }
                else
                {
                    return this.Info.Title;
                }
            }
            set
            {
                if (this.Info == null)
                {
                    this.Info = new DocumentInfo();
                }
                this.Info.Title = value;
            }
        }

        /// <summary>
        /// 运行时标题
        /// </summary>
        public string RuntimeTitle
        {
            get
            {
                if (string.IsNullOrEmpty(this.Info.Title) == false )
                {
                    return this.Info.Title;
                }
                if (string.IsNullOrEmpty(this.FileName) == false )
                {
                    return this.FileName;
                }
                else
                {
                    return this.ID ;
                }
            }
        }
        /// <summary>
        /// 对象所属文档就是自己
        /// </summary>
        public override DomDocument OwnerDocument
        {
            get
            {
                return this;
            }
            set
            {
            }
        }

        /// <summary>
        /// 文档对象没有父节点
        /// </summary>
        public override DomContainerElement Parent
        {
            get
            {
                return null;
            }
            set
            {
            }
        }


#region 管理文档元素内容的成员群 **************************************

        [NonSerialized]
        private DocumentControler _DocumentControler = null;
        /// <summary>
        /// 文档控制器
        /// </summary>
        /// <remarks>
        /// 本属性内部首先使用其绑定的编辑器控件的控制器，若没有使用
        /// 文档自己的控制器，若没有则创建一个新的控制器。</remarks>
        public DocumentControler DocumentControler
        {
            get
            {
                DocumentControler result = null;
                if (this.EditorControl != null)
                {
                    result = this.EditorControl.DocumentControler;
                    if (result != null)
                    {
                        return result;
                    }
                }
                if (_DocumentControler == null)
                {
                    this._DocumentControler = new DocumentControler();
                }
                if( _DocumentControler == null )
                {
                    return null;
                }
                _DocumentControler.Document = this;
                return _DocumentControler;
            }
            set
            {
                _DocumentControler = value;
            }
        }

        /// <summary>
        /// 更新文档元素的状态，包括OwernDocument、Parent、Content、PrivateContent属性
        /// </summary>
        public void UpdateElementState()
        {
            DomTreeNodeEnumerable enumer = new DomTreeNodeEnumerable(this);
            enumer.ExcludeParagraphFlag = false;
            enumer.ExcludeCharElement = false;
            foreach (DomElement element in enumer)
            {
                if (enumer.CurrentParent is DomContainerElement)
                {
                    element.Parent = (DomContainerElement)enumer.CurrentParent;
                }
                else
                {
                }
                //element.Parent = (XTextContainerElement)enumer.CurrentParent;
                element.OwnerDocument = this;
                if (element is DomContentElement)
                {
                    ((DomContentElement)element).UpdateContentElements(false);
                }
                else if (element is DomTableElement)
                {
                    ((DomTableElement)element).UpdateCellOverrideState();
                }
            }//foreach
            enumer.Dispose();
        }
         
        /// <summary>
        /// 页眉对象
        /// </summary>
        public DomDocumentHeaderElement Header
        {
            get
            {
                CheckRootElement();
                return (DomDocumentHeaderElement)this.Elements[0];//.GetElement(typeof(XTextDocumentHeaderElement));
            }
        }

        /// <summary>
        /// 文档正文对象
        /// </summary>
        public DomDocumentBodyElement Body
        {
            get
            {
                CheckRootElement();
                return (DomDocumentBodyElement)this.Elements[1];//.GetElement(typeof(XTextDocumentBodyElement));

                //foreach (XTextElement element in this.Elements)
                //{
                //    if (element is XTextDocumentBodyElement)
                //    {
                //        return (XTextDocumentBodyElement)element;
                //    }
                //}
                //XTextDocumentBodyElement body = new XTextDocumentBodyElement();
                //this.AppendChildElement(body);
                ////body.FixElements();
                ////body.Elements.Add(new XTextParagraphFlagElement());// this.CreateParagraphEOF());
                //return body;
            }
        }

        private void CheckRootElement()
        {
            //if (this._ElementsForSerialize != null
            //    && this._ElementsForSerialize.Count > 0)
            //{
            //    this.FixElementsForSerialize(false);
            //}
            DomElementList elements = this.Elements;
            if (elements.Count == 3
                && elements.GetItemFast(0) is DomDocumentHeaderElement
                && elements.GetItemFast(1) is DomDocumentBodyElement
                && elements.GetItemFast(2) is DomDocumentFooterElement)
            {
                return;
            }
            if (elements.Count == 0)
            {
                elements.Capacity = 3;
                elements.FastAdd2(new DomDocumentHeaderElement());
                elements.FastAdd2(new DomDocumentBodyElement());
                elements.FastAdd2(new DomDocumentFooterElement());
                foreach (var item in elements.FastForEach())
                {
                    item.InnerSetOwnerDocumentParentRaw(this, this);
                }
                this.ResetChildElementStats();
                return;
            }
            var vheader = elements.GetFirstElement<DomDocumentHeaderElement>();
            if (vheader == null)
            {
                vheader = new DomDocumentHeaderElement();
                vheader.InnerSetOwnerDocumentParentRaw(this, this);
            }
            var vfooter = elements.GetFirstElement<DomDocumentFooterElement>();
            if (vfooter == null)
            {
                vfooter = new DomDocumentFooterElement();
                vfooter.InnerSetOwnerDocumentParentRaw(this, this);
            }
            var vbody = elements.GetFirstElement<DomDocumentBodyElement>();
            if (vbody == null)
            {
                vbody = new DomDocumentBodyElement();
                vbody.InnerSetOwnerDocumentParentRaw(this, this);
            }
            foreach (DomElement e in elements.FastForEach())
            {
                if ((e is DomDocumentContentElement) == false)
                {
                    vbody.Elements.Add(e);
                }
            }
            elements.SetData(new DomElement[] { vheader , vbody , vfooter });
            this.ResetChildElementStats();
        }

        /// <summary>
        /// 文档正在进行序列化中。
        /// </summary>
        [ThreadStatic]
        internal static bool _DocumentSerializing = false;

        /// <summary>
        /// 文档正文纯文本内容
        /// </summary>
        public string BodyText
        {
            get
            {
                if( _DocumentSerializing && this.Options.BehaviorOptions.SaveBodyTextToXml == false )
                {
                    return null;
                }
                foreach( var item in this.Elements.FastForEach() )
                {
                    if(item is DomDocumentBodyElement )
                    {
                        return item.Text;
                    }
                }
                return string.Empty;
            }
            set
            {
            }
        }

        /// <summary>
        /// 页脚对象
        /// </summary>
        public DomDocumentFooterElement Footer
        {
            get
            {
                CheckRootElement();
                return (DomDocumentFooterElement)this.Elements[2];//.GetElement(typeof(XTextDocumentFooterElement));
            }
        }
         
        /// <summary>
        /// 快速判断是否为当前文档区域
        /// </summary>
        /// <param name="dce"></param>
        /// <returns></returns>
        internal bool IsCurrentContentElement(DomDocumentContentElement dce)
        {
            if (this._CurrentContentElement == dce)
            {
                return true;
            }
            if (this._CurrentContentElement == null && dce is DomDocumentBodyElement)
            {
                return true;
            }
            return false;
        }

        private DomDocumentContentElement _CurrentContentElement = null;
        /// <summary>
        /// 当前插入点所在的文档内容块对象，它是Header,Body或Footer中的某个。
        /// </summary>
        public DomDocumentContentElement CurrentContentElement
        {
            get
            {
                if (this._CurrentContentElement == null)
                {
                    this._CurrentContentElement = this.Body;
                }
                return this._CurrentContentElement;
            }
            set
            {
                if (this._CurrentContentElement != value)
                {
                    this._CurrentContentElement = value;
                }
            }
        }

        /// <summary>
        /// 文档中是否有内容被选择
        /// </summary>
        public override bool HasSelection
        {
            get
            {
                DomDocumentContentElement dce = this.CurrentContentElement;
                return dce != null && dce.Selection.Length != 0;
            }
        }

        /// <summary>
        /// 当前文档内容模块样式
        /// </summary>
        public PageContentPartyStyle CurrentContentPartyStyle
        {
            get
            {
                if( this._CurrentContentElement == null || this._CurrentContentElement is DomDocumentBodyElement )
                {
                    return PageContentPartyStyle.Body;
                }
                var cce = this.CurrentContentElement;
                if(cce is DomDocumentBodyElement)
                {
                    return PageContentPartyStyle.Body;
                }
                else if (cce is DomDocumentHeaderElement)
                {
                    return PageContentPartyStyle.Header;
                }
                else if (cce is DomDocumentFooterElement)
                {
                    return PageContentPartyStyle.Footer;
                }
                return PageContentPartyStyle.Body;
            }
        }

        /// <summary>
        /// 当前内容
        /// </summary>
        public DCContent Content
        {
            get
            {
                return this.CurrentContentElement.Content;
            }
        }


        /// <summary>
        /// 当前被选择的内容
        /// </summary>
        public DCSelection Selection
        {
            get
            {
                return this.CurrentContentElement.Selection;
            }
        }

        /// <summary>
        /// 根据起始编号和终止编号来选择内容
        /// </summary>
        /// <param name="startContentIndex">选择区域起始编号</param>
        /// <param name="endContentIndex">选择区域终止编号</param>
        /// <returns>操作是否成功</returns>
        public bool SelectContentByStartEndIndex(int startContentIndex, int endContentIndex)
        {
            if (startContentIndex < 0)
            {
                throw new ArgumentException("startContentIndex=" + startContentIndex);
            }
            if (endContentIndex < 0)
            {
                throw new ArgumentException("endContentIndex=" + endContentIndex);
            }
            if (endContentIndex > startContentIndex)
            {
                return this.Content.SetSelection(startContentIndex, endContentIndex - startContentIndex + 1);
            }
            else
            {
                return this.Content.SetSelection(startContentIndex, endContentIndex - startContentIndex - 1);
            }
        }

        /// <summary>
        /// 选择内容
        /// </summary>
        /// <param name="startElement">选择区域起始元素</param>
        /// <param name="endElement">选择区域终止元素</param>
        /// <returns>操作是否成功</returns>
        public bool SelectContentByStartEndElement(DomElement startElement, DomElement endElement)
        {
            if (startElement == null)
            {
                throw new ArgumentNullException("startElement");
            }
            if (endElement == null)
            {
                throw new ArgumentNullException("endElement");
            }
            DomDocumentContentElement dce = startElement.DocumentContentElement;
            if (dce != endElement.DocumentContentElement)
            {
                throw new ArgumentException("两个元素不属于同一个区域");
            }
            if (this.CurrentContentElement != dce)
            {
                dce.Focus();
            }
            DomElement e1 = startElement.FirstContentElementInPublicContent;
            DomElement e2 = endElement.FirstContentElementInPublicContent;
            if (e1 == null || e2 == null)
            {
                e1 = startElement.FirstContentElementInPublicContent;
                e2 = endElement.FirstContentElementInPublicContent;
                return false;
            }
            else
            {
                if (e1.ContentIndex > e2.ContentIndex)
                {
                    return dce.Content.SetSelection(e1.ContentIndex, e2.ContentIndex - e1.ContentIndex - 1);
                }
                else
                {
                    return dce.Content.SetSelection(e1.ContentIndex, e2.ContentIndex - e1.ContentIndex + 1);
                }
            }
        }

        /// <summary>
        /// 获得当前被选中的唯一的元素
        /// </summary>
        public DomElement SingleSelectedElement
        {
            get
            {
                if (this.Selection.AbsLength == 1)
                {
                    return this.Selection.ContentElements[0];
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// 当前元素。在WEB开发中本属性也是有效的。
        /// </summary>
        public DomElement CurrentElement
        {
            get
            {
                return this.CurrentContentElement?.CurrentElement;
                //if (this.CurrentContentElement == null)
                //{
                //    return null;
                //}
                //else
                //{
                //    return this.CurrentContentElement.CurrentElement;
                //}
            }
        }

        /// <summary>
        /// 当前输入域元素
        /// </summary>
        public DomInputFieldElement CurrentInputField
        {
            get
            {
                return (DomInputFieldElement)GetCurrentElement(typeof(DomInputFieldElement), true);
            }
        }

        /// <summary>
        /// 当前单元格元素
        /// </summary>
        public DomTableCellElement CurrentTableCell
        {
            get
            {
                return (DomTableCellElement)GetCurrentElement(typeof(DomTableCellElement), true);
            }
        }


        /// <summary>
        /// 获得指定类型的插入点所在的文档对象
        /// </summary>
        /// <param name="elementType">指定的文档元素对象</param>
        /// <returns>获得的文档元素对象</returns>
        public DomElement GetCurrentElement(Type elementType)
        {
            return GetCurrentElement(elementType, true);
        }

        /// <summary>
        /// 获得指定类型的插入点所在的文档对象
        /// </summary>
        /// <param name="elementType">指定的文档元素对象</param>
        /// <param name="includeThis">是否对当前元素本身进行类型判断</param>
        /// <returns>获得的文档元素对象</returns>
        public DomElement GetCurrentElement(Type elementType, bool includeThis)
        {
            if (elementType == null)
            {
                throw new ArgumentNullException("elementType");
            }
            if (typeof(DomElement).IsAssignableFrom(elementType) == false)
            {
                throw new ArgumentException(elementType.FullName);
            }
            if (this.CurrentElement == null)
            {
                return null;
            }
            if (includeThis)
            {
                if (elementType.IsInstanceOfType(this.CurrentElement))
                {
                    return this.CurrentElement;
                }
            }
            DomContainerElement c = null;
            if (this.CurrentElement is DomFieldBorderElement)
            {
                c = this.CurrentElement.Parent;
            }
            else if (this.Content != null)
            {
                int index = 0;
                this.Content.GetCurrentPositionInfo(out c, out index);
            }
            while (c != null)
            {
                if (elementType.IsInstanceOfType(c))
                {
                    return c;
                }
                c = c.Parent;
            }
            return null;
        }
        /// <summary>
        /// 获得当前插入点所在的指定类型的文档元素对象。
        /// </summary>
        /// <param name="typeName">指定的文档元素类型的名称</param>
        /// <returns>获得的文档元素对象</returns>
        public DomElement GetCurrentElementByTypeName(string typeName)
        {
            Type t = WriterUtils.GetDOMElementType(typeName);
            if (t == null)
            {
                throw new ArgumentOutOfRangeException(typeName);
            }
            else
            {
                return this.GetCurrentElement(t, true);
            }
        }

        /// <summary>
        /// 当前段落对象
        /// </summary>
        public DomParagraphFlagElement CurrentParagraphEOF
        {
            get
            {
                return this.CurrentContentElement.CurrentParagraphEOF;
            }
        }

#endregion

        /// <summary>
        /// 创建新的文档对象，使其包含本文档元素的复制品
        /// </summary>
        /// <param name="includeThis">是否包含本文档原始对象,对文档对象该参数无意义</param>
        /// <returns>创建的文档对象</returns>
        public override DomDocument CreateContentDocument(bool includeThis)
        {
            return ( DomDocument ) this.Clone(true);
        }

        private int _ObjectIDIncrease = 0;
        public int AllocObjectID()
        {
            return _ObjectIDIncrease++;
        }

        /// <summary>
        /// 处理ContentChanged事件
        /// </summary>
        /// <param name="args">参数</param>
        public override void OnContentChanged(ContentChangedEventArgs args)
        {
            this.LastSearchStartPosition = -1;
            if (args.LoadingDocument == false)
            {
                this.Modified = true;
            }
            //base.OnContentChanged(args);

            if (this.EditorControl != null && this.IsLoadingDocument == false )
            {
                this.EditorControl.OnEventContentChanged(args);
            }
        }

        /// <summary>
        /// 处理ContentChanging事件
        /// </summary>
        /// <param name="args">参数</param>
        public override void OnContentChanging(ContentChangingEventArgs args)
        {
            base.OnContentChanging(args);

            if (this.EditorControl != null && this.IsLoadingDocument == false )
            {
                this.EditorControl.OnEventContentChanging(args);
            }
        }


        /// <summary>
        /// 处理编辑器控件获得焦点事件
        /// </summary>
        internal void OnControlGotFocus()
        {
            if (this.CurrentElement != null)
            {
                DocumentEventArgs args = new DocumentEventArgs(
                            this.OwnerDocument,
                            this.CurrentElement,
                            DocumentEventStyles.ControlGotFoucs);
                args.Cursor = null;
                this.BubbleHandleElementEvent(this.CurrentElement, args);
            }
        }

        /// <summary>
        /// 处理编辑控件失去焦点事件
        /// </summary>
        internal void OnControlLostFocus()
        {
            if (this.CurrentElement != null)
            {
                DocumentEventArgs args = new DocumentEventArgs(
                            this.OwnerDocument,
                            this.CurrentElement,
                            DocumentEventStyles.ControlLostFoucs);

                args.Cursor = null;
                this.BubbleHandleElementEvent(this.CurrentElement, args);
            }
        }

        /// <summary>
        /// 处理编辑控件失去焦点事件
        /// </summary>
        internal void OnControlLostFocusExt()
        {
            if (this.CurrentElement != null)
            {
                DocumentEventArgs args = new DocumentEventArgs(
                            this.OwnerDocument,
                            this.CurrentElement,
                            DocumentEventStyles.LostFocusExt);
                args.Cursor = null;
                this.BubbleHandleElementEvent(this.CurrentElement, args);
            }
        }


        /// <summary>
        /// 创建绘图事件参数
        /// </summary>
        /// <param name="g">画布对象</param>
        /// <param name="renderMode">呈现方式</param>
        /// <returns>创建的参数对象</returns>
        internal InnerDocumentPaintEventArgs CreateInnerPaintEventArgs(
            DCGraphics g , 
            InnerDocumentRenderMode renderMode = InnerDocumentRenderMode.Paint)
        {
            InnerDocumentPaintEventArgs arg = new InnerDocumentPaintEventArgs(this, g, RectangleF.Empty , renderMode);
            return arg;
        }

        private static Graphics _MemoryGraphics = null;
        /// <summary>
        /// 内部的创建画布对象
        /// </summary>
        /// <returns>创建的画布对象</returns>
        public override DCGraphics InnerCreateDCGraphics()
        {
            //var css = this.ContentStyles;
            //bool back = css.AllowResetFastValue;
            //css.AllowResetFastValue = true;
            //css.ResetFastValue();
            //css.AllowResetFastValue = back;
            DCGraphics g2 = null;
            if( _MemoryGraphics != null )
            {
                _MemoryGraphics.Transform = new Matrix();
                g2 = new DCGraphics(_MemoryGraphics);
                g2.PageUnit = DCSystem_Drawing.GraphicsUnit.Document;
                g2.AutoDisposeNativeGraphics = false;
                return g2;
            }
            //if (this._EditorControl == null || this._EditorControl.IsHandleCreated == false)
            //{
            _MemoryGraphics = DrawerUtil.SafeCreateGraphics();
                _MemoryGraphics.PageUnit = DCSystem_Drawing.GraphicsUnit.Document;
                g2 = new DCGraphics(_MemoryGraphics);
                g2.AutoDisposeNativeGraphics = false;
            //}
            //else
            //{
            //    _MemoryGraphics = this._EditorControl.CreateViewGraphics();
            //    g2 = new DCGraphics(  _MemoryGraphics );
            //    g2.PageUnit = this._DocumentGraphicsUnit;
            //    g2.AutoDisposeNativeGraphics = false;
            //}
            return g2;
        }
        /// <summary>
        /// 清空文档内容
        /// </summary>
        public void Clear()
        {
            this.ClearCachedOptions();
            this.Modified = false;
            ClearBuffer();
            // 设置页面方式
            this.PageSettings = new XPageSettings();
            this.FileName = null;
            this.FileFormat = null;
            this.ContentStyles.Clear();
            this.Info.CreationTime = this.GetNowDateTime();
            this.Info.LastModifiedTime = this.Info.CreationTime;
            this.Info.EditMinute = 0;
            this.ClearContent();
            if (this.IsLoadingDocument == false)
            {
                using (DCGraphics g = this.InnerCreateDCGraphics())
                {
                    this.InnerRefreshSize(g);
                }
                this.InnerExecuteLayout();
                this.OnSelectionChanged();
                this.OnDocumentContentChanged();
            }
        }

        public void ClearContent()
        {
            this.ResetChildElementStats();
            if( this._Elements_FixLinePositionForPageLine != null )
            {
                this._Elements_FixLinePositionForPageLine.Clear();
                this._Elements_FixLinePositionForPageLine = null;
            }
            this.ClearCachedOptions();
            if (this._DocumentControler != null)
            {
                this._DocumentControler.ClearBuffer();
            }
            this._CurrentStyleInfo = null;
            ClearBuffer();
            this._States = new DocumentStates();
            this._CheckBoxGroupInfo = null;
            this.LastSearchStartPosition = -1;
            if (this._HighlightManager != null)
            {
                this._HighlightManager.Clear();
            }
            this._CurrentContentElement = null;
            this.ContentSourceType = DocumentContentSourceTypes.Unknown;
            this._CurrentPage = null;
            this._CurrentStyleInfo = null;
            this.Info = new DocumentInfo();

            if (this._UndoList != null)
            {
                this._UndoList.Clear();
            }
            this.ResetRuntimeStyle();
            if (this._ContentStyles != null)
            {
                this._ContentStyles.Clear();
                this._ContentStyles.Default.FontSize = DefaultFontSize;
            }
            //if (this.DefaultFont != null)
            //{
            //    this.ContentStyles.Default.Font = this.DefaultFont;
            //}
            this._CurrentStyleInfo = null;
            if (this._Elements != null)
            {
                DomElementList list = new DomElementList();
                list.AddRangeByDCList(this._Elements);
                this._Elements.Clear();
                DomTreeNodeEnumerable enumer = new DomTreeNodeEnumerable(list);
                enumer.ExcludeParagraphFlag = true;
                enumer.ExcludeCharElement = true;

                foreach (DomElement element in enumer)
                {
                    if (element is IDisposable)
                    {
                        ((IDisposable)element).Dispose();
                    }
                }
                enumer.Dispose();
            }
            this._CurrentContentElement = null;
            this._ObjectIDIncrease = 0;
            if (this._Disposed == false && this.IsLoadingDocument == false )
            {
                try
                {
                    DomDocumentContentElement ce = this.Body;
                    ce.FixElements();
                    //ce.AppendChildElement(this.CreateParagraphEOF());
                    ce.UpdateContentElements(true);
                    ce.ResetSelectionRaw();// .SetSelection(0, 0);

                    ce = this.Header;
                    ce.FixElements();
                    //DocumentContentStyle style = new DocumentContentStyle();

                    ce.UpdateContentElements(true);
                    ce.ResetSelectionRaw();// SetSelection(0, 0);

                    ce = this.Footer;
                    ce.FixElements();
                    ce.UpdateContentElements(true);
                    ce.ResetSelectionRaw();// SetSelection(0, 0);
                    this.FixDomState();
                }
                finally
                {
                }
            }
        }

#region 撤销/重复操作相关的成员群 *************************************

        /// <summary>
        /// 撤销操作信息列表
        /// </summary>
        [NonSerialized()]
        private Undo.XTextDocumentUndoList _UndoList = null;
        /// <summary>
        /// 开始记录撤销操作信息
        /// </summary>
        /// <returns>操作是否成功</returns>
         
        public bool BeginLogUndo()
        {
            if( this.IsFreeMode())
            {
                return false;
            }
            if (this.GetDocumentBehaviorOptions().EnableLogUndo == false)
            {
                return false;
            }
            if (_UndoList != null)
            {
                return _UndoList.BeginLog();
            }
 
            return false;
        }
        /// <summary>
        /// 当前是否可以记录撤销操作信息
        /// </summary>
         
        public bool CanLogUndo
        {
            get
            {
                if (this.IsFreeMode())
                {
                    return false;
                }
                if (this.ReadyState != DomReadyStates.Complete)
                {
                    return false;
                }
                if (this.GetDocumentBehaviorOptions().EnableLogUndo == false)
                {
                    return false;
                }
                if (_UndoList != null)
                {
                    return _UndoList.CanLog();
                }
                else
                {
                    return false;
                }
            }
        }
        /// <summary>
        /// 撤销信息列表
        /// </summary>
         
        public Undo.XTextDocumentUndoList UndoList
        {
            get
            {
                if (this.IsFreeMode())
                {
                    return null ;
                }
                return _UndoList;
            }
        }

        /// <summary>
        /// 添加一个撤销动作对象到列表中
        /// </summary>
        /// <param name="undo">要添加的撤销动作对象</param>
        public void AddUndo(DCSoft.Writer.Undo.IUndoable undo)
        {
            if( this._UndoList != null && this.IsFreeMode() == false )
            {
                this._UndoList.Add(undo);
            }
        }

        /// <summary>
        /// 完成记录撤销操作信息
        /// </summary>
        /// <remarks>操作是否保存了新的撤销信息</remarks>
         
        public bool EndLogUndo()
        {
            if( this.IsFreeMode())
            {
                return false;
            }
            if (this.GetDocumentBehaviorOptions().EnableLogUndo == false)
            {
                return false;
            }
            if (_UndoList != null)
            {
                if (_UndoList.EndLog())
                {
                    // 重复/撤销操信息列表内容发生改变，需要更新用户界面
                    if (this.InnerViewControl != null)
                    {
                        this.InnerViewControl.InvalidateCommandState(StandardCommandNames.Undo);
                        this.InnerViewControl.InvalidateCommandState(StandardCommandNames.Redo);
                    }
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// 撤销记录的重做/撤销操作信息
        /// </summary>
         
        public void CancelLogUndo()
        {
            if (_UndoList != null)
            {
                _UndoList.CancelLog();
            }
        }
#endregion

        /// <summary>
        /// 创建多个文本元素
        /// </summary>
        /// <param name="strText">文本内容</param>
        /// <param name="paragraphStyle">段落样式</param>
        /// <param name="textStyle">文本样式</param>
        /// <returns>创建的字符元素对象列表</returns>
        public DomElementList CreateTextElements(
            string strText,
            DocumentContentStyle paragraphStyle,
            DocumentContentStyle textStyle)
        {
            return CreateTextElementsExt(
                strText,
                paragraphStyle,
                textStyle);
        }

        /// <summary>
        /// 创建多个文本元素
        /// </summary>
        /// <param name="strText">文本内容</param>
        /// <param name="paragraphStyle">段落样式</param>
        /// <param name="textStyle">文本样式</param>
        /// <returns>创建的字符元素对象列表</returns>
        public DomElementList CreateTextElementsExt(
            string strText,
            DocumentContentStyle paragraphStyle,
            DocumentContentStyle textStyle)
        {
            if (string.IsNullOrEmpty(strText))
            {
                return new DomElementList();
            }
            bool hasP = false;
            foreach( var c in strText)
            {
                if( c == '\r' || c == '\n')
                {
                    hasP = true;
                    break;
                }
            }
            if (hasP)
            {
                if (paragraphStyle != null)
                {
                    paragraphStyle = (DocumentContentStyle)paragraphStyle.CloneEnableDefaultValue();
                }
            }
            else
            {
                paragraphStyle = null;
            }
            if (textStyle != null)
            {
                textStyle = (DocumentContentStyle)textStyle.CloneEnableDefaultValue();
            }
            DomElementList result = new DomElementList(strText.Length);
            if (textStyle != null)
            {
                XDependencyObject.RemoveDefaultValues(textStyle, null);
                XDependencyObject.RemoveDefaultValues(textStyle, this.ContentStyles.Default);
            }
            if (paragraphStyle != null)
            {
                XDependencyObject.RemoveDefaultValues(paragraphStyle, null);
                XDependencyObject.RemoveDefaultValues(paragraphStyle, this.ContentStyles.Default);
            }
            int si = this.ContentStyles.GetStyleIndex(textStyle);
            int psi = this.ContentStyles.GetStyleIndex(paragraphStyle);
            //var xxxx = paragraphStyle.Underline;
            if (strText.Contains('\n') && strText.Contains('\r') == false)
            {
                // 这段文本包含\n，但没有找到\r，则这段文本有异常，为此将\n修改成\r
                strText = strText.Replace('\n', '\r');
            }
            DCGraphics g = this.InnerCreateDCGraphics();
            InnerDocumentPaintEventArgs dpArgs = null;
            if (g != null)
            {
                dpArgs = this.CreateInnerPaintEventArgs(g);
            }
            try
            {
                var parseCrLfAsLineBreak = this.GetDocumentBehaviorOptions().ParseCrLfAsLineBreakElement;
                strText = strText.Replace("\r\n", "\r", StringComparison.OrdinalIgnoreCase).Replace('\n', '\r');
                var textLength = strText.Length;
                for (var iCount = 0; iCount < textLength; iCount++)
                {
                    var c = strText[iCount];
                    if (c == '\r')
                    {
                        if (parseCrLfAsLineBreak)
                        {
                            // 解释成断行符
                            DomLineBreakElement lb = new DomLineBreakElement();
                            lb.SetParentAndDocumentRaw(this);
                            if (dpArgs != null)
                            {
                                lb.RefreshSize(dpArgs);
                            }
                            result.Add(lb);
                        }
                        else
                        {
                            // 解释成段落符号
                            DomParagraphFlagElement pe = new DomParagraphFlagElement();
                            pe.SetOwnerDocumentRaw(this);
                            pe.SetParentRaw(this);
                            pe.StyleIndex = psi;
                            if (dpArgs != null)
                            {
                                pe.RefreshSize(dpArgs);
                            }
                            result.FastAdd2(pe);
                        }
                    }
                    else if (c == '\n')
                    {
                    }
                    else
                    {
                        DomCharElement c2 = new DomCharElement(c, si);
                        c2.SetOwnerDocumentRaw(this);
                        c2.SetParentRaw(this);
                        //c2.StyleIndex = si;
                        if (dpArgs != null)
                        {
                            c2.RefreshSize(dpArgs);
                            //this.Render.RefreshSize(c2, g);
                        }
                        result.FastAdd2(c2);
                    }
                }//foreach
            }
            finally
            {
                if (g != null)
                {
                    g.Dispose();
                }
            }

            return result;
        }

        internal void CreateTextElementsRaw(
            DomContainerElement rootElement,
            string text,
            int textStyleIndex,
            int paragraphStyleIndex,
            DCList<DomElement> list)
        {
            if (string.IsNullOrEmpty(text))
            {
                return;
            }
            if (text.IndexOf('\n') >= 0)
            {
                text = text.Replace("\r\n", "\r", StringComparison.OrdinalIgnoreCase).Replace('\n', '\r');
            }
            var parseCrLfAsLineBreakElement = this.GetDocumentBehaviorOptions().ParseCrLfAsLineBreakElement;
            var textLength = text.Length;
            var doc = rootElement.OwnerDocument;
            for (var iCount = 0; iCount < textLength; iCount++)
            {
                var c = text[iCount];
                if (c == '\r')
                {
                    if (parseCrLfAsLineBreakElement)
                    {
                        DomLineBreakElement lb = new DomLineBreakElement();
                        lb.StyleIndex = textStyleIndex;
                        lb.InnerSetOwnerDocumentParentRaw2(rootElement);
                        list.FastAdd2(lb);
                    }
                    else
                    {
                        DomParagraphFlagElement pe = new DomParagraphFlagElement();
                        pe.StyleIndex = paragraphStyleIndex;
                        pe.InnerSetOwnerDocumentParentRaw2(rootElement);
                        list.FastAdd2(pe);
                    }
                }
                //else if (c == '\n')
                //{
                //}
                else
                {
                    DomCharElement c2 = new DomCharElement(c, rootElement, doc, textStyleIndex);
                    list.FastAdd2(c2);
                }
            }//foreach
        }

        /// <summary>
        /// 导入文档元素
        /// </summary>
        /// <param name="element">要导入的文档元素</param>
        public void ImportElement(DomElement element)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }
            ImportElements(new DomElementList(element));
        }
        /// <summary>
        /// 导入文档元素
        /// </summary>
        /// <param name="elements">要导入的文档元素</param>
        public void ImportElements(DomElementList elements)
        {
            if (elements == null)
            {
                throw new ArgumentNullException("elements");
            }
            if (elements.Count == 0)
            {
                return;
            }
            if (elements.LastElement is DomParagraphFlagElement)
            {
                DomParagraphFlagElement p = (DomParagraphFlagElement)elements.LastElement;
                if (p.AutoCreate && p.StyleIndex < 0 )
                {
                    // 删除最后一个自动生成的段落标记对象
                    elements.RemoveAt(elements.Count - 1);
                }
            }
            if (elements.Count == 0)
            {
                return;
            }
            if (elements.Count == 0)
            {
                // 删光了
                return;
            }
            Dictionary<int, DocumentContentStyle> styleIndexMap = new Dictionary<int, DocumentContentStyle>();
            DomDocument sourceDocument = elements[0].OwnerDocument;
            if (sourceDocument == null)
            {
                sourceDocument = this;
            }
            DomTreeNodeEnumerable enumer = new DomTreeNodeEnumerable(elements);
            enumer.ExcludeCharElement = false;
            enumer.ExcludeParagraphFlag = false;
            if (sourceDocument != this)
            {
                foreach (DomElement element in enumer)
                {
                    if ((element is DomCharElement || element is DomParagraphFlagElement) == false)
                    {
                        this.ValidateElementIDRepeat(element);
                    }
                    DocumentContentStyle style = (DocumentContentStyle)sourceDocument.ContentStyles.GetStyle(element.StyleIndex);
                    styleIndexMap[element._StyleIndex] = style;
                }
                DomTreeNodeEnumerable currentEnumer = new DomTreeNodeEnumerable(this);
                currentEnumer.ExcludeCharElement = false;
                currentEnumer.ExcludeParagraphFlag = false;
                currentEnumer.Dispose();
                // 复制文档样式信息
                // 创建文档内容样式映射列表
                using (DCGraphics g = this.InnerCreateDCGraphics())
                {
                    foreach (DocumentContentStyle style in styleIndexMap.Values )
                    {
                        //int oldIndex = sourceDocument.ContentStyles.GetStyleIndex(style);
                        //if (oldIndex < 0)
                        //{
                        //    // 默认样式，不进行处理
                        //    continue;
                        //}
                        DocumentContentStyle newStyle = (DocumentContentStyle)style.Clone();
                        int newIndex = this.ContentStyles.GetStyleIndex(newStyle);
                        style.Index = newIndex;
                        DocumentContentStyle newStyle2 = (DocumentContentStyle)this.ContentStyles.GetStyle(newIndex);
                        newStyle2.UpdateState(g);

                    }//for
                }//using
                this._ObjectIDIncrease = 0;

                foreach (DomElement element in enumer)
                {
                    if (styleIndexMap.ContainsKey(element.StyleIndex))
                    {
                        // 更新文档内容样式编号
                        element.StyleIndex = styleIndexMap[element.StyleIndex].Index;
                    }
                    element.SetOwnerDocumentRaw(this);
                }
            }
            enumer.Dispose();
            foreach (DomElement element in elements)
            {
                element.SetOwnerDocumentRaw(this);// .OwnerDocument = this;
                element.SizeInvalid = true;
                element.FixDomState();
            }//using
            this.ValidateElementsIDRepeat(elements);
            var loadArgs = new ElementLoadEventArgs(this, WriterConst.Format_XML);
            loadArgs.ForImportElements = true;
            foreach (DomElement element in elements)
            {
                if (element is DomCharElement || element is DomParagraphFlagElement)
                {
                    element.CommitStyleFast();
                }
                else
                {
                    loadArgs.Element = element;
                    element.AfterLoad(loadArgs);
                }
            }
        }

        /// <summary>
        /// 在当前位置插入一个元素
        /// </summary>
        /// <param name="element">要插入的元素对象</param>
        /// <returns>操作是否成功</returns>
        public bool InsertElement(DomElement element)
        {
            DomElementList list = new DomElementList();
            list.Add(element);
            int result = InsertElements(list, true);
            return result > 0;
            //InsertElementsBefore( this.CurrentElement , list , true );
        }

        /// <summary>
        /// 判断当前位置是否在一个表格的第一个单元格的第一个位置，而且
        /// 在这个表格所在的文档容器元素中，该表格前面没有任何元素
        /// </summary>
        public bool IsCurrentPositionAtFirstCell
        {
            get
            {
                if (this.Selection.Length == 0)
                {
                    DomElement element = this.CurrentElement;
                    if( element == null )
                    {
                        return false;
                    }
                    DomTableCellElement cell = element.OwnerCell;
                    if (cell != null
                        && cell.PrivateContent.IndexOf(element) == 0
                        && cell.RowIndex == 0
                        && cell.ColIndex == 0)
                    {
                        DomTableElement table = cell.OwnerTable;
                        DomContentElement ce = table.ContentElement;
                        int index = ce.PrivateContent.IndexOf(table);
                        if (index == 0)
                        {
                            // 表格为所在容器中的第一个内容元素
                            return true;
                        }
                        if (index > 0 && ce.PrivateContent[index - 1] is DomFieldBorderElement)
                        {
                            DomFieldBorderElement b = (DomFieldBorderElement)ce.PrivateContent[index - 1];
                            if (b.Position == BorderElementPosition.Start)
                            {
                                // 表格前面是文档域前置边界元素
                                // 也就是说表格是所在输入域的第一个元素
                                return true;
                            }
                        }
                        //if (ce.PrivateContent.IndexOf(table) == 0)
                        //{
                        //    // 表格为所在容器中的第一个内容元素
                        //    return true;
                        //}
                        DomElement preElement = ce.PrivateContent.GetPreElement(table);
                        if (preElement is DomTableElement
                            || preElement is DomTableElement)
                        {
                            // 表格前面就是表格或文档节元素
                            return true;
                        }
                    }
                }
                return false;
            }
        }


        /// <summary>
        /// 插入多个元素到文档中
        /// </summary>
        /// <param name="newElements">要插入的新元素</param>
        /// <param name="updateContent">是否更新文档视图</param>
        /// <returns>插入的元素个数</returns>
        public int InsertElements(DomElementList newElements, bool updateContent)
        {
            InsertElementsArgs args = new InsertElementsArgs();
            args.NewElements = newElements;
            args.UpdateContent = updateContent;
            return InsertElements( args );
        }

        /// <summary>
        /// 插入多个元素到文档中
        /// </summary>
        /// <param name="args3">参数</param>
        /// <returns>插入的元素个数</returns>
        public int InsertElements( InsertElementsArgs args3 )
        {
            if (args3 == null)
            {
                return 0;
            }
            if (args3.NewElements == null || args3.NewElements.Count == 0)
            {
                return 0;
            }
            //float tick = CountDown.GetTickCountFloat();
            DomElement curElement = args3.CurrentElement;
            int insertIndexFix = 0;
            if (curElement == null)
            {
                // 没指定当前元素，则使用文档中的插入点所在的元素
                curElement = this.CurrentElement;
            }
            bool isSingleParagraphFlag = args3.NewElements.Count == 1 && args3.NewElements[0] is DomParagraphFlagElement;
            if (isSingleParagraphFlag)
            {
                if (this.IsCurrentPositionAtFirstCell)
                {
                    DomTableCellElement cell = curElement.OwnerCell;
                    DomTableElement table = cell.OwnerTable;
                    
                    // 如果当前位置是一个表格的第一个单元格的第一个位置而且该表格是容器元素中的第一个元素
                    // 则在表格前面插入这个段落符号
                    DomContainerElement p = table.Parent;
                    {
                        // 容器是
                        ReplaceElementsArgs args2 = new ReplaceElementsArgs(
                            table.Parent,
                            p.Elements.IndexOf(table),
                            0,
                            args3.NewElements,
                            true,
                            args3.UpdateContent,
                            true);
                        args2.DetectOnly = args3.DetectOnly;
                        if (args2.DetectOnly)
                        {
                            args2.AutoFixNewElements = false;
                        }
                        int result = this.ReplaceElements(args2);
                        if (args3.NewElements.Count > 0)
                        {
                            this.CurrentContentElement.SetSelection(args3.NewElements[0].ContentIndex, 0);
                        }
                        return result;
                    }
                }
            }

            DomDocumentContentElement dce = curElement.DocumentContentElement;
            if (dce != this.Body)
            {
            }
            DomContainerElement container = null;
            int index = 0;
            dce.Content.GetPositonInfo(
                dce.Content.IndexOf(curElement),
                out container,
                out index,
                dce.Content.LineEndFlag);
            ReplaceElementsArgs args = new ReplaceElementsArgs(
                    container,
                    index + insertIndexFix ,
                    0,
                    args3.NewElements,
                    true,
                    args3.UpdateContent,
                    true);
            if( args3.FromUI)
            {
                args.EventSource = ContentChangedEventSource.UI;
            }
            args.DetectOnly = args3.DetectOnly ;
            if (args.DetectOnly)
            {
                args.AutoFixNewElements = false;
            }
            if (isSingleParagraphFlag)
            {
                // 插入一个单独的段落符号
                DomParagraphFlagElement nextP = container.Elements.SafeGet(index ) as DomParagraphFlagElement;
                if (nextP != null)
                {
                    // 紧跟着一个段落符号前面插入段落符号
                    RuntimeDocumentContentStyle nextPStyle = nextP.RuntimeStyle;
                    if (nextPStyle.ParagraphOutlineLevel >= 0 
                        && this.DocumentControler.CanModify(nextP)
                        && this.GetDocumentBehaviorOptions().ContinueHeaderParagrahStyle == false )
                    {
                        args.Tag = nextP;
                        args.AfterReplaceDOM = new EventHandler(this.InnerAfterInsertElements);
                    }
                }
            }
            if (container is DomContentElement
                && index == container.Elements.Count)
            {
                // 若容器元素是文本块元素，则最后一个元素固定保证为一个段落符号，
                // 因此当插入点位置在元素列表最后则进行修正
                index = container.Elements.Count - 1;
            }
            args.StartIndex = index + insertIndexFix;
            int result2 = this.ReplaceElements(args);
            return result2;
        }
        private bool _FixCurrentStyleInfoForEnter = false;
        /// <summary>
        /// 为插入回车而修正当前样式信息
        /// </summary>
        public bool InnerFixCurrentStyleInfoForEnter
        {
            get { return _FixCurrentStyleInfoForEnter; }
            set { _FixCurrentStyleInfoForEnter = value; }
        }

        private void InnerAfterInsertElements(object sender, EventArgs args)
        {
            ReplaceElementsArgs rargs = (ReplaceElementsArgs)args;
            DomParagraphFlagElement p = (DomParagraphFlagElement)rargs.Tag;
            DocumentContentStyle style = (DocumentContentStyle)p.RuntimeStyle.CloneParent();

            // 遇到标题类型的段落，则设置一些属性为默认值
            style.ParagraphOutlineLevel = -1;
            style.ParagraphMultiLevel = false;
            style.ParagraphListStyle = ParagraphListStyle.None;
            style.Font = this.DefaultStyle.Font;
            style.LeftIndent = 0;
            style.FirstLineIndent = 0;
            style.LineSpacingStyle = LineSpacingStyle.SpaceSingle;
            style.LineSpacing = 1;
            int si = this.ContentStyles.GetStyleIndex(style);
            if (this.CanLogUndo)
            {
                this.UndoList.AddStyleIndex(p, p.StyleIndex, si);
            }
            p.StyleIndex = si;
            p.SizeInvalid = true;
            p.ViewInvalid = true;
            this.InnerFixCurrentStyleInfoForEnter = false;
        }

        private static void CollectionElementIDs( DomContainerElement rootElement , Dictionary<string,string> ids )
        {
            var es = rootElement._Elements;
            if( es != null && es.Count >0 )
            {
                var arr = es.InnerGetArrayRaw();
                for( var iCount = es.Count -1;iCount >=0;iCount --)
                {
                    var e2 = arr[iCount];
                    if(e2 is DomObjectElement || e2 is DomContainerElement )
                    {
                        if( string.IsNullOrEmpty(e2.ID ) == false )
                        {
                            ids[e2.ID] = null;
                        }
                        if(e2 is DomContainerElement )
                        {
                            CollectionElementIDs((DomContainerElement)e2, ids);
                        }
                    }
                }
            }
        }
        /// <summary>
        /// DCWriter内部使用。修正文档元素编号
        /// </summary>
        /// <param name="elements"></param>
        /// <returns></returns>
         
        public int FixElementIDForInsertElements(DomElementList elements)
        {
            if (this.GetDocumentBehaviorOptions().AutoFixElementIDWhenInsertElements == false)
            {
                return 0;
            }
            // 获得文档中已经存在的编号
            Dictionary<string,string> existedID = null;
            //DomTreeNodeEnumerable enumer = new DomTreeNodeEnumerable(this, true );
            //enumer.ExcludeCharElement = true;
            //enumer.ExcludeParagraphFlag = true;
            //// 收集所有已经存在的ID列表
            //foreach (XTextElement element in enumer)
            //{
            //    string id = element.ID;
            //    if (string.IsNullOrEmpty(id) == false && existedID.Contains(id) == false)
            //    {
            //        existedID.Add(id);
            //    }
            //}
            //ElementEnumerateEventArgs args = new ElementEnumerateEventArgs();
            //args.ExcludeCharElement = true;
            //args.ExcludeParagraphFlag = true;
            //this.Enumerate(delegate(object sender, ElementEnumerateEventArgs args2)
            //{
            //    string id = args2.Element.ID;
            //    if (string.IsNullOrEmpty(id) == false && existedID.Contains(id) == false)
            //    {
            //        existedID.Add(id);
            //    }
            //}, args, true);
            DomTreeNodeEnumerable enumer = new DomTreeNodeEnumerable(elements);
            enumer.ExcludeParagraphFlag = true;
            enumer.ExcludeCharElement = true;
            int result = 0;
            //long idCount = DateTime.Now.Ticks;
            Dictionary<string, string> idMaps = new Dictionary<string, string>();
            foreach (DomElement element in enumer)
            {
                if (element is DomObjectElement || element is DomContainerElement)
                {
                    if (string.IsNullOrEmpty(element.ID) == false)
                    {
                        if (existedID == null)
                        {
                            // 获得文档中已经存在的编号
                            existedID = new Dictionary<string, string>();
                            CollectionElementIDs(this, existedID);
                            //DomTreeNodeEnumerable enumer2 = new DomTreeNodeEnumerable(this, true);
                            //enumer2.ExcludeCharElement = true;
                            //enumer2.ExcludeParagraphFlag = true;
                            //// 收集所有已经存在的ID列表
                            //foreach (XTextElement element3 in enumer2)
                            //{
                            //    string id3 = element3.ID;
                            //    if (string.IsNullOrEmpty(id3) == false
                            //        && existedID.ContainsKey(id3) == false)
                            //    {
                            //        existedID.Add(id3,null);
                            //    }
                            //}//foreach
                            //enumer2.Dispose();
                        }
                        string id = element.ID;
                        if (existedID.ContainsKey(id))
                        {
                            // 存在重复的ID
                            for(var iCount = 1; iCount < 100;iCount ++)
                            {
                                var newID = id + "_" + iCount.ToString();
                                if(existedID.ContainsKey(newID ) == false )
                                {
                                    idMaps[newID] = id;
                                    element.ID = newID;
                                    existedID[newID] = null;
                                    result++;
                                    break;
                                }
                            }
                            //string newID = this.AllocElementID(element.ElementIDPrefix(), element.GetType());
                            //if (existedID.ContainsKey(newID))
                            //{
                            //    // 还是存在重复，则使用随机编号
                            //    newID = newID + Convert.ToString(idCount++);
                            //}
                            //idMaps[newID] = id;
                            //element.ID = newID;
                            //existedID[newID] = null;
                            //result++;
                        }
                    }
                }
            }
            enumer.Dispose();
            if (idMaps.Count > 0)
            {
            }
            //WriterUtils.Enumerate(elements, delegate(object sender2, ElementEnumerateEventArgs args3)
            //{
            //    XTextElement element = args3.Element;
            //    if (element is XTextObjectElement || element is XTextContainerElement)
            //    {
            //        if (string.IsNullOrEmpty(element.ID) == false )
            //        {
            //            string id = element.ID;
            //            if (existedID.Contains(id))
            //            {
            //                // 存在重复的ID
            //                string newID = this.AllocElementID(element.ElementIDPrefix, element.GetType());
            //                if (existedID.Contains(newID))
            //                {
            //                    // 还是存在重复，则使用随机编号
            //                    newID = newID + Convert.ToString(idCount++);
            //                }
            //                element.ID = newID;
            //                existedID.Add(newID);
            //                result++;
            //            }
            //        }
            //    }
            //}, true);
            return result;
        }

        /// <summary>
        /// 当前执行元素替换的容器元素对象
        /// </summary>
        [NonSerialized]
        internal DomContainerElement _ReplaceElements_CurrentContainer = null;
        internal bool CheckDOMEffectBy_ReplaceElements_CurrentContainer(DomContainerElement element)
        {
            if (this._ReplaceElements_CurrentContainer != null)
            {
                var c = this._ReplaceElements_CurrentContainer;
                while (c != null)
                {
                    if (c == element)
                    {
                        return false;
                    }
                    c = c.Parent;
                }
                return true;
            }
            else
            {
                return false;
            }
        }


        /// <summary>
        /// 只触发一次的执行一次ReplaceElements()之前的事件
        /// </summary>
        internal Action<ReplaceElementsArgs> EventBeforeReplaceElementsOnce = null;

        /// <summary>
        /// 替换元素
        /// </summary>
        /// <param name="args">参数对象</param>
        /// <returns>操作的元素个数</returns>
        public int ReplaceElements(ReplaceElementsArgs args)
        {
            _ReplaceElements_CurrentContainer = null;
            if (args == null)
            {
                throw new ArgumentNullException("args");
            }
            if (this.EventBeforeReplaceElementsOnce != null)
            {
                var h = this.EventBeforeReplaceElementsOnce;
                this.EventBeforeReplaceElementsOnce = null;
                h(args);
            }
            if (args.Container == null)
            {
                throw new ArgumentNullException("container");
            }
            args.Result = 0;
            if (args.DeleteLength == 0
                && (args.NewElements == null || args.NewElements.Count == 0))
            {
                // 出现无效操作
                return 0;
            }
            if (args.StartIndex < 0)
            {
                // 出现无效操作
                return 0;
            }
            if( args.NewElements != null && args.NewElements.Count > 0 )
            {
                args.NewElements.IsElementsSplited = false;
                WriterUtilsInner.SplitElements(args.NewElements, true, this, args.Container,true);
            }
            this.States.RenderMode = DocumentRenderMode.Paint;
            //XTextDocumentContentElement dce = container.DocumentContentElement;
            DomElement currentElementBack = this.CurrentElement;
            DomContentElement ce = args.Container.ContentElement;
            if (ce == null)
            {
                throw new ArgumentNullException("ContentElement");
            }
            float tick = Environment.TickCount;
            //XTextContent content = dce.Content;
            //if (content == null)
            //{
            //    throw new ArgumentNullException("Content");
            //}

            //int selectionPos = content.AbsSelectStart;

            DomElementList elements = args.Container.Elements;
            DomElementList oldElements = new DomElementList();
            DomElementList oldElementsForGetStartElement = new DomElementList();
            if (elements.Count > 0)
            {
                // 元素列表中有内容
                if (args.StartIndex >= 0 && args.StartIndex + args.DeleteLength <= elements.Count)
                {
                    if (args.DeleteLength > 0)
                    {
                        for (int iCount = 0; iCount < args.DeleteLength; iCount++)
                        {
                            DomElement old = elements[iCount + args.StartIndex];
                            if (this.DocumentControler.CanDelete(old, args.AccessFlags) == false)
                            {
                                if (old is DomParagraphFlagElement
                                    && elements.LastElement == old
                                    && args.Container is DomContentElement)
                                {
                                    // 遇到文本块元素的最后一个段落符号，则该元素不能删除，修正参数
                                    args.DeleteLength--;
                                }
                                else
                                {
                                    // 出现只读元素，退出替换元素操作
                                    ContentProtectedInfo info = this.DocumentControler.GetLastContentProtectedInfoOnce();
                                    if (info != null)
                                    {
                                        this.ContentProtectedInfos.Add(info);
                                        return 0;
                                    }
                                    //this.DocumentControler.AddLastContentProtectdInfoOnce(this.ContentProtectedInfos);
                                    //this.ContentProtectedInfos.Add(
                                    //    old , 
                                    //    this.DocumentControler.GetLastContentProtectedMessageOnce());
                                    //return 0;
                                }
                            }
                            oldElements.Add(elements[iCount + args.StartIndex]);
                            oldElementsForGetStartElement.Add(elements[iCount + args.StartIndex]);
                        }//for
                    }
                }//if
            }
            if (args.NewElements != null && args.NewElements.Count > 0)
            {
                for (int iCount = args.NewElements.Count - 1; iCount >= 0; iCount--)
                {
                    DomElement element = args.NewElements[iCount];
                    if (this.DocumentControler.CanInsert(
                        args.Container,
                        args.StartIndex,
                        element,
                        args.AccessFlags) == false)
                    {
                        if (args.AutoFixNewElements)
                        {
                            args.NewElements.RemoveAt(iCount);
                        }
                        else
                        {
                            // 出现不能插入的元素，退出替换元素操作
                            return 0;
                        }
                    }
                }//foreach
            }
            if (args.DeleteLength == 0
                && (args.NewElements == null || args.NewElements.Count == 0))
            {
                // 出现无效操作
                return 0;
            }
            if (args.DetectOnly)
            {
                args.Result = args.DeleteLength;
                if (args.NewElements != null)
                {
                    args.Result += args.NewElements.Count;
                }
                return args.Result;
            }
            int privateStartContentIndex = 0;
            DomElement nextElement = null;
            if (args.UpdateContent && this.ReadyState == DomReadyStates.Complete)
            {
                // 更新文档内容，重新排版
                // 获得更新区域的第一个文档内容元素
                DomElement startElement = null;
                if (elements.Count == 0)
                {
                    startElement = args.Container.FirstContentElement;
                }
                else if (args.DeleteLength > 0)
                {
                    startElement = oldElementsForGetStartElement.FirstContentElement;
                }
                else if (args.StartIndex == elements.Count)
                {
                    // 插入位置在最后
                    startElement = elements[args.StartIndex - 1].FirstContentElement;
                }
                else
                {
                    var se9 = elements[args.StartIndex];
                    if (se9 is DomParagraphFlagElement)
                    {
                        startElement = se9;
                    }
                    else
                    {
                        startElement = elements[args.StartIndex].FirstContentElement;
                    }
                }
                if (startElement == null
                    || ce.PrivateContent.ContainsUsePrivateIndex(startElement) == false)
                {
                    startElement = args.Container.FirstContentElement;
                }
                if (startElement == null
                    || ce.PrivateContent.ContainsUsePrivateIndex(startElement) == false)
                {
                    startElement = ce.PrivateContent.SafeGet(0);
                }

                // 获得更新区域中最后一个文档内容元素
                DomElement endElement = null;
                if (elements.Count == 0)
                {
                    endElement = args.Container.LastContentElement;
                }
                else if (args.DeleteLength > 0)
                {
                    endElement = oldElementsForGetStartElement.LastContentElement;
                }
                else if (args.StartIndex == elements.Count)
                {
                    // 插入位置在最后
                    endElement = elements[args.StartIndex - 1].LastContentElement;
                }
                else
                {
                    endElement = elements[args.StartIndex].LastContentElement;
                }
                if (endElement == null
                    || ce.PrivateContent.ContainsUsePrivateIndex(endElement) == false)
                {
                    endElement = args.Container.LastContentElement;
                }

                nextElement = ce.PrivateContent.LastElement;
                if (endElement != null)
                {
                    nextElement = ce.PrivateContent.GetNextElement(endElement);
                }
                privateStartContentIndex = ce.PrivateContent.IndexOfUsePrivateContentIndex(startElement);
                if (privateStartContentIndex >= 0 && ce.PrivateLines.Count > 0)
                {
                    // 设置无效行
                    DomElement tempElement = ce.PrivateContent.SafeGet(privateStartContentIndex - 1);
                    int startLineIndex = 0;
                    int endLineIndex = ce.PrivateLines.Count - 1;
                    if (tempElement != null && tempElement.OwnerLine != null)
                    {
                        startLineIndex = ce.PrivateLines.IndexOf(tempElement.OwnerLine);
                        if (tempElement.OwnerLine.LastElement == tempElement)
                        {
                            startLineIndex++;
                        }
                    }
                    int index3 = ce.PrivateContent.IndexOf(endElement);
                    if (index3 >= 0)
                    {
                        tempElement = ce.PrivateContent.SafeGet(index3 + 1);
                        if (tempElement != null && tempElement.OwnerLine != null)
                        {
                            endLineIndex = ce.PrivateLines.IndexOf(tempElement.OwnerLine);
                        }
                    }
                    for (int iCount = startLineIndex; iCount <= endLineIndex; iCount++)
                    {
                        if (iCount < 0 || iCount > ce.PrivateLines.Count)
                        {
                        }
                        else
                        {
                            ce.PrivateLines[iCount].InvalidateState = true;
                        }
                    }
                }//if
            }
            // 替换简单文本元素
            bool simpleTextReplace = true;
            DomElementList deletedElement = elements.GetRange(
                args.StartIndex,
                args.DeleteLength);
            foreach (DomElement element in deletedElement)
            {
                if ((element is DomCharElement) == false && ( element is DomParagraphFlagElement) == false )
                {
                    simpleTextReplace = false;
                    break;
                }
            }
            if (simpleTextReplace && args.NewElements != null)
            {
                foreach (DomElement element in args.NewElements)
                {
                    if ((element is DomCharElement) == false && (element is DomParagraphFlagElement) == false)
                    {
                        simpleTextReplace = false;
                        break;
                    }
                }
            }
            bool contentChangingEvent = false;
            if (deletedElement != null && deletedElement.Count > 0)
            {
                // 出现要删除的内容
                contentChangingEvent = true;
            }
            if (args.NewElements != null && args.NewElements.Count > 0)
            {
                // 出现要新增的内容
                contentChangingEvent = true;
            }
            if (contentChangingEvent && args.RaiseEvent)
            {
                // 触发文档内容正在发生改变事件
                ContentChangingEventArgs args2 = new ContentChangingEventArgs();
                args2.Document = this;
                args2.Element = args.Container;
                args2.ElementIndex = args.StartIndex;
                args2.DeletingElements = deletedElement;
                args2.InsertingElements = args.NewElements;
                args.Container.RaiseBubbleOnContentChanging(args2);
                if (args2.Cancel)
                {
                    // 用户取消操作
                    return 0;
                }
            }
            this.ContentStyles.Styles.SetValueLocked(false);
            DomDocumentContentElement dce = args.Container.DocumentContentElement;
            DomElement oldSelectionStartElement = dce.Selection.FirstElement;
            DomElement oldSelectionEndElement = dce.Selection.LastElement;
            if (oldSelectionStartElement == null)
            {
                oldSelectionStartElement = dce.Content.SafeGet(dce.Selection.AbsStartIndex);//.CurrentElement;
                oldSelectionEndElement = oldSelectionStartElement;
            }
            int result = 0;
            bool setCurrentContainer = true;
            if (args.NewElements != null && args.NewElements.Count > 0)
            {
                foreach (var item in args.NewElements.FastForEach())
                {
                    if (item is DomContainerElement)
                    {
                        setCurrentContainer = false;
                        break;
                    }
                }
            }

            if (args.DeleteLength > 0)
            {
                    // 物理删除旧元素
                    if (args.DeleteLength == elements.Count)
                    {
                        if (WriterUtilsInner.HasPageBreakElement(elements, 0, elements.Count - 1))
                        {
                            this.PageRefreshed = false;
                        }
                        elements.Clear();
                        dce.ParagraphTreeInvalidte = true;

                    }
                    else
                    {
                        if (WriterUtilsInner.HasPageBreakElement(
                            elements,
                            args.StartIndex,
                            args.StartIndex + args.DeleteLength - 1))
                        {
                            this.PageRefreshed = false;
                        }
                        elements.RemoveRange(args.StartIndex, args.DeleteLength);
                        dce.ParagraphTreeInvalidte = true;
                    }
                    //this._UserTrackListChangedFlag = true;
                result += args.DeleteLength;
            }
            if (args.NewElements != null && args.NewElements.Count > 0)
            {
                // 插入新元素
                    foreach (DomElement element in args.NewElements)
                    {
                        DocumentContentStyle style = (DocumentContentStyle)element.RuntimeStyle.CloneParent();
                        element.StyleIndex = this.ContentStyles.GetStyleIndex(style);
                }
                {
                    using (DCGraphics g = this.InnerCreateDCGraphics())
                    {
                        //WriterUtils.SetRuntimeChars(args.NewElements);
                        foreach (DomElement element in args.NewElements)
                        {
                            // 对要插入的文档元素进行初始化
                            element.SetParentRaw(args.Container);
                            element.SetOwnerDocumentRaw(this);
                            if ((element is DomCharElement || element is DomParagraphFlagElement) == false)
                            {
                                element.FixDomState();
                                element.AfterLoad(new ElementLoadEventArgs(element, null));
                            }
                            if (element is DomParagraphFlagElement)
                            {
                                dce.ParagraphTreeInvalidte = true;
                            }
                            if (element.SizeInvalid)
                            {
                                // 若要插入的元素的大小未知则重新计算大小
                                InnerDocumentPaintEventArgs args2 = CreateInnerPaintEventArgs(g);
                                args2.Element = element;
                                element.RefreshSize(args2);
                            }
                            {
                                var man = this.HighlightManager;
                                if (man != null)
                                {
                                    man.InvalidateHighlightInfo(element);
                                }
                            }
                        }//foreach
                    }//using
                }
                if (args.NewElements.Count == 1)
                {
                    elements.InsertRaw(args.StartIndex, args.NewElements.GetItemFast(0));
                }
                else
                {
                    elements.InsertRange(args.StartIndex, args.NewElements);
                }
                if (WriterUtilsInner.HasPageBreakElement(args.NewElements, 0, args.NewElements.Count - 1))
                {
                    this.PageRefreshed = false;
                }
                result += args.NewElements.Count;
            }//if
            if( args.SetNewInnerValue && args.Container is DomInputFieldElementBase)
            {
                var field4 = (DomInputFieldElementBase)args.Container;
                if( args.LogUndo && this.CanLogUndo)
                {
                    this.UndoList.AddProperty(
                        Undo.XTextUndoStyles.InnerValue,
                        field4.InnerValue, 
                        args.NewInnerValue, 
                        field4);
                }
                field4.InnerValue = args.NewInnerValue;
            }
            if( simpleTextReplace == false )
            {
                args.Container.ResetChildElementStats();
            }
            //if (args.ExecuteCopySource && this.CopySourceExecuter != null)
            //{
            //    this.CopySourceExecuter.Execute(args.Container);
            //}
            if (args.AfterReplaceDOM != null)
            {
                args.AfterReplaceDOM(this, args);
                result++;
            }
            if (result > 0)
            {
                // 更新容器元素的内容版本号
                args.Container.UpdateContentVersion();
                args.Container.UpdateElementIndex(false);
            }
            // 记录操作日志
            if (args.LogUndo && this.CanLogUndo)
            {
                    this.UndoList.AddReplaceElements(
                        args.Container,
                        args.StartIndex,
                        oldElements,
                        args.NewElements);
            }
            this.Modified = true;
            if (args.UpdateContent)// && this.PageRefreshed)
            {
                if (args.NewElements != null && args.NewElements.Count > 0)
                {
                    //ElementEnumerateEventArgs args3 = new ElementEnumerateEventArgs();
                    //args3.ExcludeCharElement = true;
                    //args3.ExcludeParagraphFlag = true;
                    WriterUtilsInner.Enumerate(
                        args.NewElements,
                        new ElementEnumerateEventHandler(this.ReplaceElements_EnumerateLayout));
                }
                DomContentElement.UpdateContentElementsArgs ucArgs
                    = new DomContentElement.UpdateContentElementsArgs();
                ucArgs.PrivateStartContentIndex = privateStartContentIndex;
                ucArgs.UpdateElementsVisible = false;
                ucArgs.FromReplaceElements = true;
                //ucArgs.UpdateParentElementVisible = false;
                ucArgs.UpdateParentContentElement = true;

                ucArgs.SimpleTextMode = simpleTextReplace;
                ce.UpdateContentElements(ucArgs);
                if (setCurrentContainer)
                {
                    // 设置当前处理的容器元素对象
                    this._ReplaceElements_CurrentContainer = args.Container;
                }
                try
                {
                    DomContainerElement.GlobalEnabledResetChildElementStats = false;// simpleTextReplace == false;
                    ce.RefreshPrivateContent(
                            privateStartContentIndex,
                            nextElement == null ? -1 : ce.PrivateContent.IndexOfUsePrivateContentIndex(nextElement),
                            false,
                            false);
                }
                finally
                {
                    DomContainerElement.GlobalEnabledResetChildElementStats = true;
                    this._ReplaceElements_CurrentContainer = null;
                }

                // 确认新的插入点的位置
                //XTextDocumentContentElement dce = args.Container.DocumentContentElement;
                dce.RefreshGlobalLines();
                try
                {
                    DomContainerElement.GlobalEnabledResetChildElementStats = false;// simpleTextReplace == false;
                    if (this.InnerViewControl != null)
                    {
                        this.InnerViewControl.InnerEnableEditElementValue = false;
                    }
                    if (args.FocusContainer)
                    {
                        // 容器获得焦点
                        args.Container.Focus();
                    }
                    else if (args.PreserveSelection)
                    {
                        // 尽量使用当前的插入点的位置
                        DomElement lastContentElement = null;
                        if (args.NewElements != null && args.NewElements.Count > 0)
                        {
                            lastContentElement = args.NewElements.LastContentElement;
                        }
                        if (lastContentElement != null)
                        {
                            lastContentElement.EnsureInPrivateContent(true, true);
                        }
                        int index1 = dce.Content.IndexOf(oldSelectionStartElement);
                        int index2 = dce.Content.IndexOf(oldSelectionEndElement);
                        if (index1 >= 0 && index2 >= 0)
                        {
                            dce.Content.SetSelection(index1, index2 - index1);
                        }
                        else if (index1 >= 0 && index2 < 0)
                        {
                            dce.Content.SetSelection(index1, 0);
                        }
                        else if (index1 < 0 && index2 >= 0)
                        {
                            dce.Content.SetSelection(index2, 0);
                        }
                        else
                        {
                            dce.Content.SetSelection(dce.Selection.StartIndex, dce.Selection.Length);
                        }
                    }
                    else if (args.ChangeSelection)
                    {
                        // 设置新的插入点位置
                        if (currentElementBack != null)
                        {
                            dce.Content.AutoClearSelection = true;
                            dce.Content.LineEndFlag = false;

                            int newSelectionPosition = currentElementBack.ContentIndex;
                            if (dce.Content.IndexOfUseContentIndex(currentElementBack) < 0)
                            {
                                newSelectionPosition--;
                            }
                            if (args.NewElements != null)
                            {
                                DomElement lastContentElement = args.NewElements.LastContentElement;
                                if (lastContentElement is DomTableElement)
                                {
                                    DomTableElement table = (DomTableElement)lastContentElement;
                                    lastContentElement = table.GetCell(0, 0, true).FirstContentElement;
                                    if (lastContentElement != null)
                                    {
                                        newSelectionPosition = lastContentElement.ContentIndex;
                                    }
                                }
                                else if (lastContentElement != null && dce.Content.ContainsUseContentIndex(lastContentElement))
                                {
                                    //if (XTextLine.__NewLayoutMode)
                                    //{
                                        newSelectionPosition = lastContentElement.ContentIndex;
                                        {
                                            DomElement e5 = lastContentElement.RightLayoutElement();
                                            if (e5 != null )
                                            {
                                                newSelectionPosition = e5.ContentIndex;
                                            }
                                            else
                                            {
                                                newSelectionPosition = lastContentElement.ContentIndex + 1;
                                            }
                                        }
                                    //}
                                    //else
                                    //{
                                    //    newSelectionPosition = lastContentElement.ContentIndex + 1;
                                    //}
                                }
                                if (lastContentElement != null)
                                {
                                    lastContentElement.EnsureInPrivateContent(true, true);
                                }
                            }
                            bool bolLineEndFlag = false;
                            if( args.EventSource == ContentChangedEventSource.UI 
                                && args.DeleteLength == 0 )
                            {
                                var nextElement4 = dce.Content.SafeGet(newSelectionPosition);
                                if(nextElement4 != null 
                                    && DCSoft.Writer.Dom.LayoutHelper.OwnerHoleLine(nextElement4 ))
                                {
                                    bolLineEndFlag = true;
                                }
                            }
                            dce.Content.MoveToPosition(newSelectionPosition,bolLineEndFlag);
                        }
                    }

                }
                finally
                {
                    DomContainerElement.GlobalEnabledResetChildElementStats = true;

                    if (this.InnerViewControl != null)
                    {
                        this.WriterControl.GetInnerViewControl().InnerEnableEditElementValue = true;
                    }
                }
            }
            var validateVersionBack = this._ElementValueValidateVersion;
            if (args.RaiseEvent)
            {
                // 触发文档内容发生改变后事件
                ContentChangedEventArgs args2 = new ContentChangedEventArgs();
                args2.Document = this;
                args2.Element = args.Container;
                args2.ElementIndex = args.StartIndex;
                args2.EventSource = args.EventSource;
                if (args.DeleteLength > 0)
                {
                    args2.DeletedElements = deletedElement;
                }
                args2.InsertedElements = args.NewElements;
                args.Container.RaiseBubbleOnContentChanged(args2);
                if (args.SetNewInnerValue && args.Container is DomInputFieldElementBase)
                {
                    ((DomInputFieldElementBase)args.Container).InnerValue = args.NewInnerValue;
                }
            }
            if (this._ElementValueValidateVersion != validateVersionBack
                || this.GetDocumentEditOptions().ValueValidateMode == DocumentValueValidateMode.Dynamic)
            {
                // 执行动态内容校验
                    DefaultDOMDataProvider.ValueValidate_Document(this, true);
            }
            //this.ContentStyles.Styles.SetValueLocked(true);
            tick = Environment.TickCount - tick;
            args.TickSpan = tick;
            args.Result = result;
            return result;
        }
        private void ReplaceElements_EnumerateLayout(
            object eventSender, 
            ElementEnumerateEventArgs args3)
        {
            DomElement newElement = args3.Element;
            if (newElement is DomTableElement)
            {
                // 表格内容进行排版
                DomTableElement table = (DomTableElement)newElement;
                if (table.OwnerLine == null)
                {
                    args3.Cancel = true;
                }
                foreach (DomTableCellElement cell in table.Cells)
                {
                    cell.Width = 0;
                    cell.Height = 0;
                }
                table.InnerExecuteLayout();
                args3.CancelChild = true;
            }
        }
         
        public DomElementList CreateParagraphs(IEnumerable elements)
        {
            if (elements == null)
            {
                throw new ArgumentNullException("elements");
            }
            DomElementList result = new DomElementList();
            DomParagraphElement p = new DomParagraphElement();
            p.OwnerDocument = this;
            p.Parent = this;
            result.Add(p);
            DomParagraphFlagElement tempFlag = null;
            foreach (DomElement element in elements)
            {
                if (element is DomParagraphFlagElement)
                {
                    DomParagraphFlagElement eof = (DomParagraphFlagElement)element;
                    p.StyleIndex = eof.StyleIndex;
                    //p.myEOFElement = eof;
                    p.Elements.FastAdd2(element);

                    p = new DomParagraphElement();
                    p.OwnerDocument = this;
                    p.Parent = this;
                    result.FastAdd2(p);
                }
                else
                {
                    p.Elements.FastAdd2(element);
                }
                if (tempFlag == null)
                {
                    tempFlag = element.OwnerParagraphEOF;
                }
            }//foreach
            for (int iCount = result.Count - 1; iCount >= 0; iCount--)
            {
                DomParagraphElement p2 = (DomParagraphElement)result[iCount];
                if (p2.Elements.Count == 0)
                {
                    result.RemoveAt(iCount);
                }
                else if(( p2.Elements.LastElement is DomParagraphFlagElement ) == false )
                {
                    p2.Elements.FastAdd2(tempFlag);
                }
            }
            foreach (DomParagraphElement p2 in result)
            {
                DomElementList list = WriterUtilsInner.MergeElements(
                    p2.Elements,
                    false);
                //if (list.LastElement is XTextParagraphEOF)
                //{
                //    list.RemoveAt(list.Count - 1);
                //}
                p2.Elements.Clear();
                p2.Elements.AddRangeByDCList(list);
            }
            return result;
        }
        /// <summary>
        /// 判断文档当前状态是否可以声明文档元素排版位置无效或者视图无效.只有存在编辑器控件而且不处于锁定状态才可以声明无效。
        /// </summary>
        /// <returns>是否允许声明无效</returns>
        public bool AllowInvalidateForUILayoutOrView()
        {
            if (this.IsLoadingDocument)
            {
                return false;
            }
            if (this.WriterControl != null)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 文档绑定的控件
        /// </summary>
        private DCSoft.Writer.Controls.WriterControl _EditorControl = null;
        /// <summary>
        /// 文档绑定的控件
        /// </summary>
        public DCSoft.Writer.Controls.WriterControl EditorControl
        {
            get
            {
                return _EditorControl;
            }
            set
            {
                //if (_EditorControl != value)
                {
                    _EditorControl = value;
                }
            }
        }
        /// <summary>
        /// 文档所属编辑器控件
        /// </summary>
        public override DCSoft.Writer.Controls.WriterControl WriterControl
        {
            get
            {
                return this._EditorControl;
            }
        }
        public override void EditorRefreshViewExt(bool fastMode)
        {
            if (fastMode == false)
            {
                ElementLoadEventArgs args3 = new ElementLoadEventArgs(this, null);
                this.AfterLoad(args3);
            }
            if (this.EditorControl != null)
            {
                return;
            }
            this.RefreshSizeWithoutParamter();
            this.InnerExecuteLayout();
            this.RefreshPages();
        }
        /// <summary>
        /// 文档中包含的内容被修改的文本输入域列表对象
        /// </summary>
        public DomElementList ModifiedInputFields
        {
            get
            {
                var list = GetElementsByType<DomInputFieldElement>();
                for (int iCount = list.Count - 1; iCount >= 0; iCount--)
                {
                    if (((DomInputFieldElement)list[iCount]).Modified == false)
                    {
                        list.RemoveAt(iCount);
                    }
                }
                return list;
            }
        }

        private CheckBoxGroupInfo _CheckBoxGroupInfo = null;
        /// <summary>
        /// 复选框分组信息
        /// </summary>
        public CheckBoxGroupInfo CheckBoxGroupInfo
        {
            get
            {
                if (this._CheckBoxGroupInfo == null || this._CheckBoxGroupInfo.RootElement != this)
                {
                    this._CheckBoxGroupInfo = new CheckBoxGroupInfo(this);
                }
                return _CheckBoxGroupInfo;
            }
        }

        /// <summary>
        /// 获得指定表格中的指定单元格
        /// </summary>
        /// <param name="tableID">编号编号</param>
        /// <param name="rowIndex">从0开始计算的行号</param>
        /// <param name="colIndex">从0开始计算的列号</param>
        /// <returns>获得的单元格对象</returns>
        public DomTableCellElement GetTableCell(string tableID, int rowIndex, int colIndex)
        {
            DomTableElement table = null;
            DomElement element = this.Body.FirstChild;
            if (element is DomTableElement && element.ID == tableID)
            {
                // 在不少情况下正文的第一个文档元素就是目标表格对象，在此进行快速获取。
                table = (DomTableElement)element;
            }
            else
            {
                table = this.GetElementById(tableID) as DomTableElement;
            }
            if (table != null)
            {
                return table.GetCell(rowIndex, colIndex, false);
            }
            return null;
        }

        /// <summary>
        /// 获得表格单元格的文本内容
        /// </summary>
        /// <param name="tableID">表格编号</param>
        /// <param name="rowIndex">从0开始计算的行号</param>
        /// <param name="colIndex">从0开始计算的列号</param>
        /// <returns>单元格文本</returns>
        public string GetTableCellText(
            string tableID, 
            int rowIndex, 
            int colIndex)
        {
            DomTableCellElement cell = GetTableCell(tableID, rowIndex, colIndex);
            if (cell == null)
            {
                return null;
            }
            else
            {
                return cell.Text;
            }
        }

        /// <summary>
        /// 设置单元格文本值
        /// </summary>
        /// <param name="tableID">表格编号</param>
        /// <param name="rowIndex">从0开始计算的行号</param>
        /// <param name="colIndex">从0开始计算的列号</param>
        /// <param name="newText">新文本值</param>
        /// <returns>操作是否成功</returns>
        public bool SetTableCellText(
            string tableID, 
            int rowIndex, 
            int colIndex, 
            string newText)
        {
            DomTableCellElement cell = GetTableCell(tableID, rowIndex, colIndex);
            if (cell != null)
            {
                return SetElementText(cell, newText);
            }
            return false;
        }

        /// <summary>
        /// 获得指定编号的输入域的InnerValue属性值。
        /// </summary>
        /// <param name="id">输入域编号</param>
        /// <returns>获得的属性值</returns>
        public string GetInputFieldInnerValue(string id)
        {
            DomInputFieldElement field = GetElementById(id) as DomInputFieldElement;
            if (field != null)
            {
                return field.InnerValue;
            }
            return null;
        }

        /// <summary>
        /// 设置指定编号的输入域的InnerValue属性值。
        /// </summary>
        /// <param name="id">输入域编号</param>
        /// <param name="newValue">新的属性值</param>
        /// <returns>操作是否成功</returns>
        public bool SetInputFieldInnerValue(string id, string newValue)
        {
            DomInputFieldElement field = GetElementById(id) as DomInputFieldElement;
            if (field != null)
            {
                field.InnerValue = newValue;
                return true;
            }
            return false ;
        }

        /// <summary>
        /// 设置输入域选择多个下拉项目
        /// </summary>
        /// <param name="id">输入域编号</param>
        /// <param name="indexs">从0开始的下拉项目序号，各个序号之间用逗号分开</param>
        /// <returns>操作是否修改文档内容</returns>
        public bool SetInputFieldSelectedIndexs(string id, string indexs)
        {
            DomInputFieldElement field = GetElementById(id) as DomInputFieldElement;
            if (field != null)
            {
                bool result = field.EditorSetSelectedIndexs(indexs);
                return result;
            }
            return false;
        }

        /// <summary>
        /// 设置文档元素文档的文本值
        /// </summary>
        /// <param name="id">文档元素编号</param>
        /// <param name="text">文本值</param>
        /// <returns>操作是否成功</returns>
        public bool SetElementTextByID(string id, string text)
        {
            DomElement element = GetElementById(id);
            if (element != null)
            {
                return SetElementText(element, text);
            }
            return false;
        }
        /// <summary>
        /// 设置文档元素文本值
        /// </summary>
        /// <param name="element">文档元素对象</param>
        /// <param name="text">文本值</param>
        /// <returns>操作是否成功</returns>
        public bool SetElementText(DomElement element, string text)
        {
            return TypeProviders.TypeProvider_XTextDocument.SetElementText(this, element, text);
        }
        /// <summary>
        /// 获得指定文档元素的可见性
        /// </summary>
        /// <param name="id">文档元素编号</param>
        /// <returns>可见性</returns>
        public bool GetElementVisible(string id)
        {
            DomElement element = GetElementById(id);
            if (element != null)
            {
                return element.RuntimeVisible;
            }
            return false;
        }

        /// <summary>
        /// 设置文档元素的可见性
        /// </summary>
        /// <param name="id">文档元素编号</param>
        /// <param name="visible">文本值</param>
        /// <returns>操作是否成功</returns>
        public bool SetElementVisible(string id, bool visible)
        {
            DomElement element = this.GetElementById(id);
            if (element != null)
            {
                return element.EditorSetVisibleExt(visible, false, false);
            }
            return false;
        }
        /// <summary>
        /// 获得文档中所有指定编号的元素对象列表,查找时ID值区分大小写的。
        /// </summary>
        /// <param name="id">指定的编号</param>
        /// <returns>找到的元素对象列表</returns>
        public override DomElementList GetElementsById(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return null;
            }
            if (string.Compare(id, WriterConst.Header, true) == 0)
            {
                // 返回页眉
                return new DomElementList(this.Header);
            }
            else if (string.Compare(id, WriterConst.Body, true) == 0)
            {
                // 返回正文
                return new DomElementList(this.Body);
            }
            else if (string.Compare(id, WriterConst.Footer, true) == 0)
            {
                // 返回页脚
                return new DomElementList(this.Footer);
            }
            return base.GetElementsById(id);
        }
        
        /// <summary>
        /// 获得文档中指定编号的元素对象,查找时ID值区分大小写的。
        /// </summary>
        /// <param name="id">指定的编号</param>
        /// <returns>找到的元素对象</returns>
        public override DomElement GetElementById(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return null;
            }
            if (string.Compare(id, WriterConst.Header, true) == 0)
            {
                // 返回页眉
                return this.Header;
            }
            else if (string.Compare(id, WriterConst.Body, true) == 0)
            {
                // 返回正文
                return this.Body;
            }
            else if (string.Compare(id, WriterConst.Footer, true) == 0)
            {
                // 返回页脚
                return this.Footer;
            }
            return base.GetElementById(id);
        }

        /// <summary>
        /// 获得文档中指定类型的下一个元素
        /// </summary>
        /// <param name="startElement">开始查找的起始元素</param>
        /// <param name="nextElementType">要查找的元素的类型</param>
        /// <param name="includeHiddenElement">查找的时候是否查找隐藏的文档元素对象</param>
        /// <returns>找到的元素</returns>
        public DomElement GetNextElement(
            DomElement startElement,
            Type nextElementType,
            bool includeHiddenElement )
        {
            if (startElement == null)
            {
                throw new ArgumentNullException("startElement");
            }
            if (nextElementType == null)
            {
                throw new ArgumentNullException("nextElementType");
            }
            DomElement result = null;
            bool ready = false;
            DomDocumentContentElement dce = startElement.DocumentContentElement;
            DomTreeNodeEnumerable enumer = new DomTreeNodeEnumerable(dce);
            foreach (DomElement element in enumer)
            {
                if (includeHiddenElement == false)
                {
                    if (element.RuntimeVisible == false)
                    {
                        enumer.CancelChild();
                        continue;
                    }
                }
                if (ready)
                {
                    if (nextElementType.IsInstanceOfType(element))
                    {
                        result = element;
                        break;
                    }
                }
                else if (element == startElement)
                {
                    ready = true;
                }
            }
            enumer.Dispose();
            //dce.Enumerate(delegate(object eventSender, ElementEnumerateEventArgs args)
            //    {
            //        if (includeHiddenElement == false)
            //        {
            //            if (args.Element.RuntimeVisible == false)
            //            {
            //                args.CancelChild = true;
            //                return;
            //            }
            //        }
            //        if (ready)
            //        {
            //            if (nextElementType.IsInstanceOfType(args.Element))
            //            {
            //                result = args.Element;
            //                args.Cancel = true;
            //            }
            //        }
            //        else if (args.Element == startElement)
            //        {
            //            ready = true;
            //        }
            //    });
            return result;
        }

        /// <summary>
        /// 获得文档中所有的勾选的复选框元素的值
        /// </summary>
        /// <param name="spliter">各个项目之间的分隔字符串</param>
        /// <param name="includeCheckbox">是否包含复选框</param>
        /// <param name="includeRadio">是否包含单选框</param>
        /// <param name="includeElementID">是否包含元素ID号</param>
        /// <param name="includeElementName">是否包含元素Name属性值</param>
        /// <returns>获得的字符串</returns>
        /// <remarks>
        /// 例如调用 document.GetCheckedValueList(";",true,true,true,true) 返回类似字符串
        /// “xbzw;胸部正位;gpzw;骨盆正位;fbww;腹部卧位”
        /// </remarks>
        public string GetCheckedValueList(
            string spliter , 
            bool includeCheckbox, 
            bool includeRadio, 
            bool includeElementID, 
            bool includeElementName)
        {
            return TypeProviders.TypeProvider_XTextDocument.GetCheckedValueList(
                this, 
                spliter, 
                includeCheckbox, 
                includeRadio, 
                includeElementID, 
                includeElementName);
        }

         
        public DomDocumentContentElement GetDocumentContentElement(PageContentPartyStyle style)
        {
            switch (style)
            {
                case PageContentPartyStyle.Header :
                    return this.Header;
                case PageContentPartyStyle.Footer :
                    return this.Footer;
                case PageContentPartyStyle.Body :
                    return this.Body;
            }
            return this.Body;
        }
        private bool _InvalidateLayoutFast = false;
        /// <summary>
        /// 快速设置排版状态无效标记
        /// </summary>
        public bool InvalidateLayoutFast
        {
            get
            {
                return _InvalidateLayoutFast;
            }
            set
            {
                _InvalidateLayoutFast = value;
            }
        }

        /// <summary>
        /// 声明指定的元素的布局发生改变
        /// </summary>
        /// <param name="element">文档元素对象</param>
        public void InvalidateLayout(DomElement element)
        {
        }

        /// <summary>
        /// 获得声明元素对象视图无效时的视图坐标中的矩形区域
        /// </summary>
        /// <param name="element">文档元素对象</param>
        /// <returns>矩形区域</returns>
        internal RectangleF GetViewBoundsForInvalidateElementView(DomElement element)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }
            if (element is DomCharElement || element is DomParagraphFlagElement || element is DomFieldBorderElement)
            {
                // 快速处理一些常见元素
                var line = element.OwnerLine;
                if (line != null)
                {
                    var rect2 = new RectangleF(
                        line.AbsLeft + element.Left,
                        line.AbsTop,
                        element.Width + element.WidthFix,
                        line.Height);
                    return rect2;
                }
            }
            RectangleF rect = element.GetAbsBounds();
            rect.Width = element.ViewWidth + element.WidthFix;
            if ( element.OwnerLine != null)
            {
                rect.Y = element.OwnerLine.AbsTop;
                    rect.Height = Math.Max(element.OwnerLine.Height, element.Height);
                    if (element.Height > element.OwnerLine.Height)
                    {
                        rect.Y = rect.Y - (element.Height - element.OwnerLine.Height);
                        rect.Height = element.Height;
                    }
            }
            return rect;
        }
        /// <summary>
        /// 是否启用InvalidateView系列函数
        /// </summary>
        internal bool _EnableInvalidateViewFunction = true;

        /// <summary>
        /// 声明指定元素的视图无效,需要重新绘制
        /// </summary>
        /// <param name="element">文本元素对象</param>
         
        public bool InvalidateElementView(DomElement element)
        {
            if (this._EnableInvalidateViewFunction == false)
            {
                return false ;
            }
            if (this.IsLoadingDocument || this._IsDrawingPageContent)
            {
                return false ;
            }
            var viewCtl = this.EditorControl?.GetInnerViewControl();
            if(viewCtl == null )
            {
                return false;
            }
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }
            if (viewCtl._EnabledViewInvalidate )
            {
                if (element == this)
                {
                    viewCtl.Invalidate();
                    return true ;
                }
                if (element.DocumentContentElement != null)
                {
                    RectangleF rect = GetViewBoundsForInvalidateElementView(element);
                    this.EditorControl.ViewInvalidate(
                        rect,
                        element.DocumentContentElement.ContentPartyStyle);
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// 声明指定区域的文档视图无效,需要重新绘制
        /// </summary>
        /// <param name="range">指定的区域</param>

        public void InvalidateView(DCRange range)
        {
            if (this._EnableInvalidateViewFunction == false)
            {
                return;
            }
            if (this.IsLoadingDocument)
            {
                return;
            }
            if (range == null)
            {
                return;
                //throw new ArgumentNullException("range");
            }

            if (this.EditorControl != null )
            {
                foreach (DomElement element in range)
                {
                    this.InvalidateElementView(element);
                }//foreach
            }
        }

        public ET CreateElement<ET>() where ET : DomElement , new()
        {
            var result = new ET();
            result.OwnerDocument = this;
            return result;
        }



        /// <summary>
        /// 根据一个字符串创建若干个字符文本元素
        /// </summary>
        /// <param name="strText">字符串</param>
        /// <param name="styleIndex">指定的样式编号</param>
        /// <returns>创建的字符文本元素组成的列表</returns>
        public DomElementList CreateChars(string strText, int styleIndex)
        {
            if (string.IsNullOrEmpty(strText))
            {
                return null;
            }
            var list = new DomElementList();
            var len = strText.Length;
            for (var iCount = 0; iCount < len; iCount++)
            {
                var c = strText[iCount];
                DomCharElement c2 = CreateChar(c, styleIndex);
                if (c2 != null)
                {
                    list.Add(c2);
                }
            }
            return list;
        }
        /// <summary>
        /// 创建字符元素
        /// </summary>
        /// <param name="v">字符值</param>
        /// <param name="styleIndex">样式编号</param>
        /// <returns>创建的元素</returns>
        public DomCharElement CreateChar(char v, int styleIndex)
        {
            if (v == '\n' || v == '\r')
            {
                return null;
            }
            if (v < 32 && v != '\t')
            {
                // 出现不可接收的字符
                return null;
            }

            DomCharElement myChar = new DomCharElement(v , this , this , styleIndex);
            //myChar._StyleIndex = styleIndex;
            //myChar.InnerSetOwnerDocumentParentRaw(this, this);
            return myChar;
        }
        
        /// <summary>
        /// 获得指定元素所在段落的样式
        /// </summary>
        /// <param name="element">元素对象</param>
        /// <returns>段落样式对象</returns>
        public DomParagraphFlagElement GetParagraphEOFElement(DomElement element)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }
            if (element is DomParagraphFlagElement)
            {
                return (DomParagraphFlagElement)element;
            }
            DomContentElement ce = element.ContentElement;
            if (ce != null)
            {
                int index = 0;
                DomElementList pc = ce.PrivateContent;
                if (element is DomFieldElementBase)
                {
                    var field = (DomFieldElementBase)element;
                    if (field.StartElement != null)
                    {
                        index = pc.IndexOf(field.StartElement);
                    }
                    else
                    {
                        index = pc.IndexOf(element);
                    }
                }
                else
                {
                    index = pc.IndexOf(element);
                }
                if (index < 0)
                {
                    // 发生状态错误
                    //System.Diagnostics.Debug.WriteLine("GetParagraphEOFElement:元素不包含在内容中。");
                    return pc.LastElement as DomParagraphFlagElement ;
                }

                for (; index < pc.Count; index++)
                {
                    if (pc[index] is DomParagraphFlagElement)
                    {
                        return (DomParagraphFlagElement)pc[index];
                    }
                }
            }
            return null;
        }

        [NonSerialized]
        private MouseCaptureInfo _MouseCapture = null;
        /// <summary>
        /// 鼠标捕获操作对象
        /// </summary>
        internal MouseCaptureInfo MouseCapture
        {
            get
            {
                return _MouseCapture; 
            }
            set
            {
                //if (_MouseCapture != value)
                {
                    _MouseCapture = value;
                }
            }
        }
         
        //#endif
        [NonSerialized()]
        private DomElement _HoverElement = null;

        private DCTabIndexManager _TabIndexManager = null;
        /// <summary>
        /// Tab切换操作管理器
        /// </summary>
        internal DCTabIndexManager TabIndexManager
        {
            get
            {
                if (this._TabIndexManager == null)
                {
                    this._TabIndexManager = new DCTabIndexManager();
                    if( this._TabIndexManager != null )
                    {
                        this._TabIndexManager.Document = this;
                    }
                }
                return _TabIndexManager;
            }
        }
        private HighlightManager _HighlightManager = null;
        /// <summary>
        /// 文档视图中高亮度显示区域管理器
        /// </summary>
        internal HighlightManager HighlightManager
        {
            get
            {
                if (_HighlightManager == null)
                {
                    this._HighlightManager = new HighlightManager();
                    if (this._HighlightManager != null)
                    {
                        this._HighlightManager.Document = this;
                    }
                }
                return _HighlightManager;
            }
            set
            {
                _HighlightManager = value;
            }
        }
        /// <summary>
        /// 方法无效
        /// </summary>
        /// <returns></returns>
        new private HighlightInfoList GetHighlightInfos()
        {
            return null;
        }


        /// <summary>
        /// 鼠标悬停的元素
        /// </summary>
        public DomElement HoverElement
        {
            get
            {
                return _HoverElement;
            }
            set
            {
                _HoverElement = value;
                if (this.HighlightManager != null)
                {
                    this.HighlightManager.HoverHighlightInfo = null;
                }
                //if (_HoverElement != null)
                //{
                //    _HoverHighlightInfo = GetHighlightInfo(_HoverElement);
                //}
            }
        }

        /// <summary>
        /// 判断鼠标是否悬停在对象上面
        /// </summary>
        /// <param name="element">文档元素</param>
        /// <returns>是否悬停</returns>
        //[System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        ////[DCSoft.Common.DCPublishAPI]
        public bool IsHover(DomElement element)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }
            if (this.HoverElement == null)
            {
                return false;
            }
            else
            {
                DomElement e = this.HoverElement ;
                while (e != null)
                {
                    if (e == element)
                    {
                        return true;
                    }
                    e = e.Parent;
                }
                return false;
            }
        }

        /// <summary>
        /// 鼠标悬停元素改变事件处理
        /// </summary>
        /// <param name="oldHoverElement"></param>
        /// <param name="newHoverElement"></param>
        private void OnHoverElementChanged(
            DomElement oldHoverElement,
            DomElement newHoverElement)
        {
            if (this.EditorControl != null)
            {
                this.EditorControl.SetHoverElement(oldHoverElement, newHoverElement);
            }
        }

        /// <summary>
        /// 鼠标按键按下时的选择区域的长度
        /// </summary>
        [NonSerialized]
        private int _MouseDown_SelectionLength = int.MinValue;
        /// <summary>
        /// 鼠标按键按下时的选择区域对象
        /// </summary>
        [NonSerialized]
        private DCSelection _MouseDown_Selection = null;
       

        /// <summary>
        /// 处理用户界面事件
        /// </summary>
        /// <param name="args">事件参数</param>
        public override void HandleDocumentEvent(DocumentEventArgs args)
        {
            if (this.IsFreeMode())
            {
                return;
            }
            this.CacheOptions();
            if( args.Style == DocumentEventStyles.MouseClick)
            {
                this._MouseDown_Selection = null;
                this._MouseDown_SelectionLength = int.MinValue;
            }
            if (args.Style == DocumentEventStyles.MouseDown)
            {
                if (args.Button == System.Windows.Forms.MouseButtons.Left)
                {
                    this._MouseDown_Selection = this.Selection;
                    this._MouseDown_SelectionLength = this.Selection.Length;
                }
                if( this.InnerViewControl != null )
                {
                    this.InnerViewControl.UseAbsTransformPoint = true;
                }
                DomElement element = GetElementAt(args.X, args.Y, true);// this.Content.GetElementAt(args.X, args.Y, true);
                if (element != null)
                {
                    // 冒泡方式触发文档元素事件
                    BubbleHandleElementEvent(element, args);
                    if (args.Cursor == null)
                    {
                        args.Cursor = element.RuntimeDefaultCursor();
                        if (args.Cursor == null)
                        {
                            var sel = element.DocumentContentElement?.Selection;
                            if (sel.Contains(element))
                            {
                                args.Cursor = WriterControl.InnerCursorArrow;
                            }
                        }
                    }
                }
            }
            else if (args.Style == DocumentEventStyles.MouseMove)
            {
                if (args.Button == System.Windows.Forms.MouseButtons.None)
                {
                    _MouseCapture = null;
                }
                if (_MouseCapture != null)
                {
                    _MouseCapture.LastX = args.X;
                    _MouseCapture.LastY = args.Y;
                    _MouseCapture.UsedFlag = true;
                    this.Content.AutoClearSelection = false;
                    this.Content.MoveToPoint(args.X, args.Y);
                    this.Content.AutoClearSelection = true;
                    //myBindControl.MoveTo( args.X , args.Y );
                    this.InnerViewControl.MoveCaretWithScroll = false;
                    this.EditorControl.UpdateTextCaret();
                    this.InnerViewControl.MoveCaretWithScroll = true;
                }
                else
                {
                    if (args.Button == System.Windows.Forms.MouseButtons.None)
                    {
                        // 当不是严格命中文档,则鼠标光标实际上是在文档范围之外,此时当前鼠标悬浮的元素为空引用.
                        DomElement element = args.StrictMatch ? GetElementAt(args.X, args.Y, true) : null;// this.Content.GetElementAt(args.X, args.Y, true);
                        if (element != this._HoverElement)
                        {
                            DomElementList parents1 = this.HoverElement == null ?
                                new DomElementList() : WriterUtilsInner.GetParentList(this.HoverElement);
                            if (this.HoverElement != null)
                            {
                                parents1.Insert(0, this.HoverElement);
                            }
                            DomElementList parents2 = element == null ?
                                new DomElementList() : WriterUtilsInner.GetParentList(element);
                            if (element != null)
                            {
                                parents2.Insert(0, element);
                            }
                            else
                            {
                                parents2.Add(this);
                            }
                            // 触发鼠标离开文档元素事件
                            foreach (DomElement element2 in parents1)
                            {
                                if (parents2.Contains(element2) == false)
                                {
                                    DocumentEventArgs args2 = args.Clone();
                                    args2.intStyle = DocumentEventStyles.MouseLeave;
                                    args2.Element = element2;
                                    element2.HandleDocumentEvent(args2);
                                    if (args2.Handled)
                                    {
                                        break;
                                    }
                                }
                                else
                                {
                                    break;
                                }
                            }//foreach
                            // 触发鼠标进入文档元素事件
                            foreach (DomElement element2 in parents2)
                            {
                                if (parents1.Contains(element2) == false)
                                {
                                    DocumentEventArgs args2 = args.Clone();
                                    args2.intStyle = DocumentEventStyles.MouseEnter;
                                    args2.Element = element2;
                                    element2.HandleDocumentEvent(args2);
                                    if (args2.Handled)
                                    {
                                        break;
                                    }
                                }
                                else
                                {
                                    break;
                                }
                            }//foreach
                        }
                        if (element != this._HoverElement)
                        {
                            DomElement back = this._HoverElement;
                            this.HoverElement = element;
                            this.OnHoverElementChanged(back, element);
                        }
                        // 触发元素的鼠标移动事件
                        if (element == null)
                        {
                            element = GetElementAt(args.X, args.Y, true);
                        }
                        if (element != null)
                        {
                            BubbleHandleElementEvent(element, args);
                            if (args.Cursor == null)
                            {
                                args.Cursor = element.RuntimeDefaultCursor();
                                if (args.Cursor == null)
                                {
                                    var sel = element.DocumentContentElement?.Selection;
                                    if (sel.Contains(element))
                                    {
                                        args.Cursor = WriterControl.InnerCursorArrow;
                                    }
                                }
                            }
                        }
                    }//if
                }
            }
            else if (args.Style == DocumentEventStyles.MouseUp
                || args.Style == DocumentEventStyles.MouseClick
                || args.Style == DocumentEventStyles.MouseDblClick)
            {
                this.InnerViewControl.UseAbsTransformPoint = false;
                if (_MouseCapture == null || _MouseCapture.UsedFlag == false )
                {
                    DomElement element = GetElementAt(args.X, args.Y, true);// this.Content.GetElementAt(args.X, args.Y, true);
                    if (element != null)
                    {
                        BubbleHandleElementEvent(element, args);
                        if (args.Style == DocumentEventStyles.MouseClick)
                        {
                            this._MouseDown_Selection = null;
                            this._MouseDown_SelectionLength = int.MinValue;
                        }
                        if (args.Cursor == null)
                        {
                            args.Cursor = element.RuntimeDefaultCursor();
                        }
                        if( element is DomObjectElement)
                        {
                            this.WriterControl.OnEventObjectElementMouseClick((DomObjectElement)element, args);
                        }
                        //element.HandleDocumentEvent(args);
                    }         
                }
                _MouseCapture = null;
            }

            else if (args.Style == DocumentEventStyles.MouseLeave)
            {
                if (this.HoverElement != null)
                {
                    DomElement back = this.HoverElement;
                    BubbleHandleElementEvent(this.HoverElement, args);
                    this.HoverElement = null;
                    this.OnHoverElementChanged(back, null);
                }
            }
            else if (args.Style == DocumentEventStyles.KeyDown)
            {
                DomElement element = null;
                if (Math.Abs(this.Selection.Length) == 1)
                {
                    element = this.Selection.ContentElements[0];
                }
                else
                {
                    DomContainerElement container = null;
                    int index = 0;
                    this.Content.GetCurrentPositionInfo(out container, out index);
                    element = container;
                }
                BubbleHandleElementEvent(element, args);
            }
            else if (args.Style == DocumentEventStyles.KeyPress)
            {
                DomElement element = null;
                if (Math.Abs(this.Selection.Length) == 1)
                {
                    element = this.Selection.ContentElements[0];
                }
                else
                {
                    DomContainerElement container = null;
                    int index = 0;
                    this.Content.GetCurrentPositionInfo(out container, out index);
                    element = container;
                }
                BubbleHandleElementEvent(element, args);
            }
            else
            {
                base.HandleDocumentEvent(args);
            }
            this.ClearCachedOptions();
        }
        /// <summary>
        /// 进行冒泡式事件处理
        /// </summary>
        /// <param name="elements">要处理事件的元素列表</param>
        /// <param name="args">事件参数</param>
        private void BubbleHandleElementEvent(DomElementList elements, DocumentEventArgs args)
        {
            if (this.IsFreeMode())
            {
                return;
            }
            int x = args.X;
            int y = args.Y;
            args.Handled = false;
            int endIndex = elements.Count - 1;
            for (int iCount = 0; iCount <= endIndex; iCount++)//list.Count - 1; iCount >= 0; iCount--)
            {
                DomElement item = elements[iCount];
                if (args.Style == DocumentEventStyles.MouseDown
                       || args.Style == DocumentEventStyles.MouseMove
                       || args.Style == DocumentEventStyles.MouseUp
                       || args.Style == DocumentEventStyles.MouseClick
                       || args.Style == DocumentEventStyles.MouseDblClick)
                {
                    if (item is DomFieldElementBase )
                    {
                    }
                    else
                    { 
                        args.intX = (int)(x - item.GetAbsLeft());
                        args.intY = (int)(y - item.GetAbsTop());
                        if (args.X < 0 || args.X > item.Width || args.Y < 0 || args.Y > item.Height)
                        {
                            continue;
                        }
                    }
                }
                args.Element = item;
                item.HandleDocumentEvent(args);
                if (args.Handled || args.CancelBubble )
                {
                    break;
                }
                if (item.BelongToDocumentDom(this) == false)
                {
                    // 文档DOM结构发生改变，元素已经脱离DOM结构，不再执行事件
                    break;
                }
            }//for
            args.intX = x;
            args.intY = y;
        }

        private void BubbleHandleElementEvent(DomElement element, DocumentEventArgs args)
        {
            if( this.IsFreeMode())
            {
                return;
            }
            // 获得事件冒泡顺序列表
            DomElementList list = new DomElementList();
            DomElement item = element;
            while (item != null && item != this)
            {
                list.Add(item);
                item = item.Parent;
            }//while
            BubbleHandleElementEvent(list, args);
            if (args.Style == DocumentEventStyles.MouseDown
                || args.Style == DocumentEventStyles.MouseMove
                || args.Style == DocumentEventStyles.MouseUp
                || args.Style == DocumentEventStyles.MouseClick
                || args.Style == DocumentEventStyles.MouseDblClick
                || args.Style == DocumentEventStyles.MouseEnter)
            {
                if (args.Cursor == null)
                {
                    args.Cursor = element.RuntimeDefaultCursor();
                }
            }
        }
                    /// <summary>
                    /// 获得文档视图中指定位置处的文档元素对象
                    /// </summary>
                    /// <param name="x">指定的X坐标</param>
                    /// <param name="y">指定的Y坐标</param>
                    /// <param name="strict">是否严格匹配</param>
                    /// <returns>获得的文档元素</returns>
                   //[System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public DomElement GetElementAt(float x, float y, bool strict)
        {
            // 先搜索文档内容
            DomElement element = this.Content.GetElementAt(x, y, strict);
            if (element == null)
            {
                // 未找到文档内容则搜索表格
                DomElementList tables = this.TypedElements.GetElementsByType(
                    this.CurrentContentElement, 
                    typeof(DomTableElement));//  .CurrentContentElement.GetElementsByType(typeof(XTextTableElement));
                if (tables != null && tables.Count > 0)
                {
                    for (int iCount = tables.Count - 1; iCount >= 0; iCount--)
                    {
                        DomTableElement table = (DomTableElement)tables[iCount];
                            DomTableCellElement cell = table.GetCellByAbsPosition(x, y);
                            if (cell != null)
                            {
                                return cell;
                            }
                    }//for
                }//if
            }//element
            return element;
        }


        [NonSerialized]
        private DCContentRender _Render = null;

        /// <summary>
        /// 绘制文档内容的视图对象
        /// </summary>
        public DCContentRender Render
        {
            get
            {
                if (this._Disposed)
                {
                    return null;
                }
                if (_Render == null )
                {
                    _Render = new DCContentRender( this );
                }
                //if (this._Render != null)
                //{
                //    _Render.Document = this;
                //}
                return _Render;
            }
            set
            {
                _Render = value;
            }
        }

        /// <summary>
        /// 元素是否处于选择状态
        /// </summary>
        /// <param name="element">元素对象</param>
        /// <returns>是否选择</returns>
        //[System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        // 
        ////[DCSoft.Common.DCPublishAPI]
        public bool IsSelected(DomElement element)
        {
            DomDocumentContentElement ce = element.DocumentContentElement;
            return ce.IsSelected(element);
        }

        /// <summary>
        /// 元素是否可见
        /// </summary>
        /// <param name="element">元素对象</param>
        /// <returns>是否可见</returns>
        // 
        public bool IsVisible(DomElement element)
        {
            return true;
        }
        /// <summary>
        /// 将像素长度转换为文档长度
        /// </summary>
        /// <param name="Value">像素长度</param>
        /// <returns>文档长度</returns>
        public float PixelToDocumentUnit(float Value)
        {
            return GraphicsUnitConvert.Convert(
                Value,
                GraphicsUnit.Pixel,
                DCSystem_Drawing.GraphicsUnit.Document);
        }

        /// <summary>
        /// 将像素大小转换为文档大小
        /// </summary>
        /// <param name="Value">像素大小</param>
        /// <returns>文档大小</returns>
        public Size PixelToDocumentUnit(Size Value)
        {
            return GraphicsUnitConvert.Convert(
                Value,
                GraphicsUnit.Pixel,
                DCSystem_Drawing.GraphicsUnit.Document);
        }
        /// <summary>
        /// 将文档长度转换为像素长度
        /// </summary>
        /// <param name="Value">文档长度</param>
        /// <returns>像素长度</returns>
        public int ToPixel(float Value)
        {
            if(Value == 0 )
            {
                return 0;
            }
            if(Value == 1 )
            {
                return 1;
            }
            
            var result = (int)GraphicsUnitConvert.Convert(
                Value,
                DCSystem_Drawing.GraphicsUnit.Document,
                GraphicsUnit.Pixel);
            if( result == 0 && Value >= 0.999 )
            {
                return 1;
            }
            return result;
        }

        /// <summary>
        /// 将文档长度转换为浮点数的像素长度
        /// </summary>
        /// <param name="Value">文档长度</param>
        /// <returns>像素长度</returns>
        public float ToPixelFloat(float v)
        {
            return GraphicsUnitConvert.Convert(
                v,
                DCSystem_Drawing.GraphicsUnit.Document,
                GraphicsUnit.Pixel);
        }
        /// <summary>
        /// 将文档大小转换为像素大小
        /// </summary>
        /// <param name="Value">文档大小</param>
        /// <returns>像素大小</returns>
        public Size ToPixel(Size Value)
        {
            return GraphicsUnitConvert.Convert(
                Value,
                DCSystem_Drawing.GraphicsUnit.Document,
                GraphicsUnit.Pixel);
        }

        [NonSerialized]
        internal DocumentStates _States = new DocumentStates();
        /// <summary>
        /// 文档状态
        /// </summary>
        public DocumentStates States
        {
            get
            {
                return _States; 
            }
            //set
            //{
            //    _States = value; 
            //}
        }

        /// <summary>
        /// 运行时的文档处于打印视图模式
        /// </summary>
        internal bool PrintingViewMode()
        {
            if (this.States.Printing)
            {
                return true;
            }
            if( this.WriterControl != null )
            {
                if(this.WriterControl.IsPrintOrPreviewMode)
                {
                    return true;
                }
            }
            return false;
        }

        #region IPageDocument 成员


         
        private XPageSettings _PageSettings = new XPageSettings();
        /// <summary>
        /// 页面设置信息对象
        /// </summary>
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public XPageSettings PageSettings
        {
            get
            {
                if (_PageSettings == null)
                {
                    _PageSettings = new XPageSettings();
                    this.PageRefreshed = false;
                }
                return _PageSettings;
            }
            set
            {
                _PageSettings = value;
                if (_PageSettings == null)
                {
                    _PageSettings = new XPageSettings();
                }
                this.PageRefreshed = false;
            }
        }

        /// <summary>
        /// 页面集合
        /// </summary>
        private PrintPageCollection _Pages = null;
        /// <summary>
        /// 页面集合
        /// </summary>
        public PrintPageCollection Pages
        {
            get
            {
                if (_Pages == null)
                {
                    _Pages = new PrintPageCollection();
                }
                return _Pages;
            }
            set
            {
                _Pages = value;
            }
        }

        private PrintPageCollection _GlobalPages = null;
        /// <summary>
        /// 全局页面集合
        /// </summary>
        public PrintPageCollection GlobalPages
        {
            get
            {
                if (this._GlobalPages == null)
                {
                    return this.Pages;
                }
                else
                {
                    return this._GlobalPages;
                }
            }
            set
            {
                this._GlobalPages = value;
            }
        }

        [NonSerialized()]
        private bool _PageRefreshed = false;
        /// <summary>
        /// 文档已经执行的排版和分页操作
        /// </summary>
        public bool PageRefreshed
        {
            get
            {
                return this._PageRefreshed;
            }
            set
            {
                //if (this._PageRefreshed != value)
                {
                    this._PageRefreshed = value;
                }
            }
        }


        private PrintPage _CurrentPage = null;
        /// <summary>
        /// 当前处理的文档页对象
        /// </summary>
        public PrintPage CurrentPage
        {
            get
            {
                return this._CurrentPage;
            }
            set
            {
                this._CurrentPage = value;
            }
        }

        /// <summary>
        /// 从0开始计算的全局页码数
        /// </summary>
        public int GlobalPageIndex
        {
            get
            {
                if (this.CurrentPage == null || this.GlobalPages == null)
                {
                    return 0;
                }
                else
                {
                    return Math.Max( 0 , this.GlobalPages.IndexOf(this.CurrentPage)) ;
                }
            }
        }

        /// <summary>
        /// 从0开始计算的当前打印的页面序号
        /// </summary>
        public int PageIndex
        {
            get
            {
                if (this.CurrentPage == null || this.Pages == null)
                {
                    return 0;
                }
                else
                {
                    return this.Pages.IndexOf(this.CurrentPage) ;
                }
            }
            set
            {
                if (value < 0 || value > this.Pages.Count)
                {
                    throw new ArgumentOutOfRangeException("value=" + value);
                }
                this.CurrentPage = this.Pages[value];
            }
        }

        public void DrawPageContentByGraphics(
            object g,
            PrintPage page,
            bool showMarginLine,
            float zoomRate,
            DocumentRenderMode renderMode,
            DCSoft.WASM.MyPrintTaskOptions printOptions)
        {
            if (g == null)
            {
                throw new ArgumentNullException("g");
            }
            if (g is Graphics || g is DCGraphics)
            {

            }
            else
            {
                throw new NotSupportedException(g.ToString());
            }
            DocumentPageDrawer drawer = new DocumentPageDrawer();
            if (printOptions != null)
            {
                drawer.Options = printOptions.CreateStdOptions();
            }
            drawer.Document = this;
            drawer.Pages = this.Pages;
            switch (renderMode)
            {
                case DocumentRenderMode.Print: drawer.RenderMode = ContentRenderMode.Print; break;
                default: drawer.RenderMode = ContentRenderMode.UIPaint; break;
            }
            drawer.PageMarginLineLength = this.GetDocumentViewOptions().PageMarginLineLength;
            drawer.XZoomRate = zoomRate;
            drawer.YZoomRate = zoomRate;
            if (g is Graphics)
            {
                drawer.InnerDrawPage2(page, new DCGraphics((Graphics)g), showMarginLine);
            }
            else if (g is DCGraphics)
            {
                drawer.InnerDrawPage2(page, (DCGraphics)g, showMarginLine);
            }
        }

 
        [NonSerialized]
        private RectangleF _EnclosingBoundsForDrawContent = RectangleF.Empty;
        /// <summary>
        /// 最后一次绘制的所有文档内容的最小外切矩形
        /// </summary>
        public RectangleF EnclosingBoundsForDrawContent
        {
            get
            {
                return this._EnclosingBoundsForDrawContent; 
            }
            set 
            {
                this._EnclosingBoundsForDrawContent = value; 
            }
        }
        internal void AddDrawContentBounds(RectangleF rect)
        {
            if (rect.IsEmpty == false)
            {
                if (this._EnclosingBoundsForDrawContent.IsEmpty)
                {
                    this._EnclosingBoundsForDrawContent = rect;
                }
                else
                {
                    this._EnclosingBoundsForDrawContent = RectangleF.Union(this._EnclosingBoundsForDrawContent, rect);
                }
            }
        }
 
       
        ///// <summary>
        ///// 当前打印的页面序号
        ///// </summary>
        //int PageIndex
        //{
        //    get;
        //    set;
        //}

        internal bool _IsDrawingPageContent = false;
        /// <summary>
        /// 绘制文档内容
        /// </summary>
        /// <param name="args">参数</param>
        public void DrawPageContent(InnerPageDocumentPaintEventArgs args)
        {
            var states = this.States;
            this._EnclosingBoundsForDrawContent = RectangleF.Empty;
            try
            {
                this._IsDrawingPageContent = true;
                this.CacheOptions();
                this.DocumentControler?.BeginCacheValue();
                this.CurrentPage = args.Page;
                DomDocumentContentElement dce = GetDocumentContentElement(args.ContentStyle);
                if (dce != this.Body && dce.HasContentElement == false)
                {
                    if (this.CurrentContentElement != dce)
                    {
                        return;
                    }
                }

                if (dce == null || dce.Visible == false)
                {
                    // 不可见
                    return;
                }
                this._LastHorizLinePositions = 0;
                var args2RenderMode = InnerDocumentRenderMode.Paint;
                if (args.RenderMode == ContentRenderMode.Print)
                {
                    args2RenderMode = InnerDocumentRenderMode.Print;
                }
                else
                {
                    if (this.PrintingViewMode())
                    {
                        args2RenderMode = InnerDocumentRenderMode.Print;
                    }
                    else
                    {
                        args2RenderMode = InnerDocumentRenderMode.Paint;
                    }
                }

                InnerDocumentPaintEventArgs args2 = new InnerDocumentPaintEventArgs(
                    this,
                    args.Graphics,
                    args.ClipRectangle.ToFloat(),
                    args2RenderMode);
                args2.Type = args.ContentStyle;
                args2.ActiveMode = (dce == this.CurrentContentElement);
                args2.HighlightSelection = dce.HasSelection;
                args2.SetDocumentContentElement(dce);
                args2.Options = args.Options;
                var vopts = this.GetDocumentViewOptions();
                //args.ViewBoundsF = e.AbsBounds;
                args2.Element = dce;
                args2.Style = dce.RuntimeStyle;
                args2.PageClipRectangle = args.PageClipRectangle;// args.ContentBounds;
                args2.PageIndex = args.PageIndex;
                args2.PageCount = this.Pages.Count;
                args2.Page = args.Page;
                // 试图触发文档元素的BeforePaint事件
                args2.Graphics.AutoSetInnerMatrix();
                args2.ViewBounds = dce.GetAbsBounds();
                if( args.RenderMode == ContentRenderMode.Print && this.InnerViewControl != null )
                {
                    args2.PrintTaskOptions = this.InnerViewControl._CurrentPrintTaskOptions;
                }
                    dce.DrawContent(args2);
                if ((dce == this.Header)
                    && this.RuntimeShowHeaderBottomLine())
                {
                    // 显示页眉下面的横线
                    if (dce.Content.Count > 0 && vopts.HeaderBottomLineWidth > 0 )
                    {
                        {
                            float lw = 1;
                            if(vopts.HeaderBottomLineWidth != 1 )
                            {
                                lw = DCGraphics.ConvertPenWidth(
                                    vopts.HeaderBottomLineWidth,
                                    GraphicsUnit.Pixel,
                                    args.Graphics.PageUnit);
                            }
                            using (var p = new Pen(Color.Black, lw))
                            {
                                if (args2.SVG != null)
                                {
                                    args2.Graphics.DrawLine(
                                     //args.GraphicsDrawLine(
                                     p,
                                     dce.Left,
                                     dce.Bottom +1,
                                     dce.Right,
                                     dce.Bottom +1);
                                }
                                else
                                {
                                    args2.Graphics.DrawLine(
                                        //args.GraphicsDrawLine(
                                        p,
                                        dce.Left,
                                        dce.Bottom - lw - 2,
                                        dce.Right,
                                        dce.Bottom - lw - 2);
                                }
                            }
                        }
                    }
                }
                args2.Graphics.CleanInnerMatrix();
            }
            finally
            {
                this._IsDrawingPageContent = false;
                //GraphicsUnitConvert.SetDpi(dpiBack);
                this.ClearCachedOptions();
                this.DocumentControler?.EndCacheValue();
            }
        }

        /// <summary>
        /// 运行时是否显示页眉下面的横线
        /// </summary>
        public bool RuntimeShowHeaderBottomLine()
        {
            bool showHeaderBottomLine = true;
            if (this.Info.ShowHeaderBottomLine == DCBooleanValue.True)
            {
                showHeaderBottomLine = true;
            }
            else if (this.Info.ShowHeaderBottomLine == DCBooleanValue.False)
            {
                showHeaderBottomLine = false;
            }
            else
            {
                showHeaderBottomLine = this.GetDocumentViewOptions().ShowHeaderBottomLine;
            }
            return showHeaderBottomLine;
        }

        #endregion
#if !RELEASE
        /// <summary>
        /// 返回表示内容的字符串
        /// </summary>
        /// <returns>字符串</returns>
        public override string ToString()
        {
            return "Document:" + this.Info.Title;
        }
#endif

        /// <summary>
        /// 复制对象
        /// </summary>
        /// <param name="Deeply">是否</param>
        /// <returns></returns>
        public override DomElement Clone(bool Deeply)
        {
            DomDocument doc = (DomDocument)base.Clone(Deeply);
            doc.ResetChildElementStats();
            doc.FixLayoutForPrint = false;
            doc._Elements_FixLinePositionForPageLine = null;
            doc.SetOwnerDocumentRaw( doc );
            doc._TypedElements = null;
            doc._TypedElements = null;
            doc._PageRefreshed = false;
            doc._HoverElement = null;
            doc.ClearCachedOptions();
            doc._MouseCapture = null;
            doc._States = new DocumentStates();
            doc._CheckBoxGroupInfo = null;
            doc._TabIndexManager = null;
            doc._MouseCapture = null;
            doc._EditorControl = null;
            doc._UndoList = null;
            doc._HighlightManager = null;
            doc._SrcElementForEventReadFileContent = null;
            doc._CheckBoxGroupInfo = null;
            doc._ContentProtectedInfos = null;
            doc._CurrentContentElement = null;
            doc._CurrentPage = null;
            doc._CurrentStyleInfo = null;
            doc._DocumentControler = null;
            doc._GlobalPages = null;
            doc._HoverElement = null;
            doc._Pages = null;
            doc.CopyContent(this, false);
            if (Deeply)
            {
                if (doc.HasElements())
                {
                    foreach (var item in doc.Elements)
                    {
                        item.OwnerDocument = doc;
                    }
                }
                doc._ContentStyles = (DocumentContentStyleContainer)this.ContentStyles.Clone();
            }
            else
            {
                doc._ContentStyles = null;
            }
            doc.FixDomState();
            return doc;
        }

        private float _LastHorizLinePositions = 0;
        /// <summary>
        /// 记录水平线的位置,包括水平边框线和下边框线。
        /// </summary>
        /// <param name="pos">位置</param>
        internal void LogHorizLinePosition(float pos)
        {
            if (pos > _LastHorizLinePositions)
            {
                _LastHorizLinePositions = pos;
            }
        }
    }
}