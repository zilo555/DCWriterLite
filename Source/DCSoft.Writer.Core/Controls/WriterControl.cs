using DCSoft.WinForms;
using DCSoft.Printing ;
using DCSoft.Writer.Dom;
using DCSoft.Drawing;
using System.Windows.Forms;
using System.ComponentModel ;
using System.Collections ;
using System.Collections.Generic ;
using DCSoft.Common;
using System.Text;
using DCSoft.Writer.Data;
using System.Runtime.InteropServices;
namespace DCSoft.Writer.Controls
{
    /// <summary>
    /// 文本文档编辑控件
    /// </summary>
    /// <remarks>编制 袁永福</remarks>
    public partial class WriterControl : System.Windows.Forms.Control
    {
      
        /// <summary>
        /// 初始化对象
        /// </summary>
        public WriterControl()
        {
            DCSoft.WinForms.WinFormUtils.SetFormDefaultFont(this);
            InitializeComponent();

            this.ctlHRule.BindControl(this);
            this.ctlVRule.BindControl(this);
            //this.ctlHRule.Visible = false;
            //this.ctlVRule.Visible = false;
            this.RuleVisible = true;
            Font f = new Font(this.Font.Name, 9);
            this.ctlHRule.Font = f;
            this.ctlVRule.Font = f;
            this.RuleVisible = false;

            this.myViewControl.BackColor = this.BackColor;
            DCConsole.Default.WriteLine("Create " + this.GetType().Name + " :" + Convert.ToString(_CtlInstanceCounter++));
        }
        #region 粘贴板相关的代码 ************************************************

        //internal IDataObject _DataObjectFromPaste = null;

        public virtual IDataObject CreateDataObject()
        {
            return null;
        }

        #endregion

        /// <summary>
        /// 是否处于打印或者打印预览状态
        /// </summary>
        public virtual bool IsPrintOrPreviewMode
        {
            get
            {
                return false;
            }
        }

        public virtual bool IsAPILogRecord
        {
            get
            {
                return false;
            }
        }

        public virtual void APILogRecordDebugPrint( string txt )
        {

        }

        public override void Invalidate()
        {
            this.myViewControl?.Invalidate();
        }
        /// <summary>
        /// 控件类型全名
        /// </summary>
        public string ControlTypeName
        {
            get
            {
                return this.GetType().FullName;
            }
        }

        private static int _CtlInstanceCounter = 0;


        internal void IncreaseSelectionVersion()
        {
        }

        /// <summary>
        /// 背景色
        /// </summary>
        public override Color BackColor
        {
            get
            {
                return base.BackColor;
            }
            set
            {
                base.BackColor = Color.FromArgb( 255 , value);
                this.myViewControl.BackColor = value;
            }
        }
        /// <summary>
        /// 重置表单元素默认值
        /// </summary>
        /// <returns>是否导致文档内容发生改变</returns>
        public bool ResetFormValue()
        {
            return this.Document.ResetFormValue();
        }
 
        /// <summary>
        /// 返回出精确到秒的当前时间
        /// </summary>
        /// <returns></returns>
        public static DateTime GetServerTime()
        {
            var dtm = DateTimeCommon.GetNow();
            return new DateTime(dtm.Year, dtm.Month, dtm.Day, dtm.Hour, dtm.Minute, dtm.Second);
        }

        /// <summary>
        /// 是否强制显示光标而不管控件是否获得输入焦点
        /// </summary>
        public bool ForceShowCaret
        {
            get
            {
                return this.GetInnerViewControl().ForceShowCaret;
            }
            set
            {
                this.GetInnerViewControl().ForceShowCaret = value;
            }
        }

#region 标尺相关

        /// <summary>
        /// 更新标尺状态,DCWriter内部使用。
        /// </summary>
        public void UpdateRuleState()
        {
            if (this.ctlHRule.Visible)
            {
                this.ctlHRule.UpdateState(true);
                this.ctlHRule.Invalidate();
                this.ctlVRule.UpdateState(true);
                this.ctlVRule.Invalidate();
            }
        }

        private bool _RuleVisible = false ;
        /// <summary>
        /// 标尺是否可见,为了提高兼容性，默认不显示标尺。
        /// </summary>
        public bool RuleVisible
        {
            get
            {
                return this._RuleVisible;
            }
            set
            {
                if (this._RuleVisible != value)
                {
                    this._RuleVisible = value;
                }
            }
        }

        /// <summary>
        /// 标尺背景色
        /// </summary>
        public Color RuleBackColor
        {
            get
            {
                return this.ctlHRule.BackColor;
            }
            set
            {
                this.ctlHRule.BackColor = value;
                this.ctlVRule.BackColor = value;
            }
        }

        /// <summary>
        /// 标尺是否可用
        /// </summary>
        public bool RuleEnabled
        {
            get
            {
                return this.ctlHRule.Enabled;
            }
            set
            {
                this.ctlHRule.Enabled = value;
                this.ctlHRule.Enabled = value;
            }
        }

        #endregion

        /// <summary>
        /// DCWriter内部使用。内置的文档视图控件
        /// </summary>
        /// <returns>获得的控件对象</returns>
        public WriterViewControl GetInnerViewControl()
        {
            return this.myViewControl;
        }

        /// <summary>
        /// 判断是否存在有效的内置文档视图控件
        /// </summary>
         
        public bool HasValidateInnerViewControl()
        {
            if (this.myViewControl != null && this.myViewControl.IsDisposed == false)
            {
                return true;
            }
            return false;
        }
         
        /// <summary>
        /// 自动设置文档的默认字体
        /// </summary>
        /// <remarks>若该属性值为true，则编辑器会自动将控件的字体和前景色设置
        /// 到文档的默认字体和文本颜色。修改本属性值不会立即更新文档视图，
        /// 此时需要调用“UpdateDefaultFont( true )”来更新文档视图。</remarks>
        public bool AutoSetDocumentDefaultFont
        {
            get
            {
                if (this.GetInnerViewControl() == null)
                {
                    return false;
                }
                else
                {
                    return this.GetInnerViewControl().AutoSetDocumentDefaultFont;
                }
            }
            set
            {
                if (this.GetInnerViewControl() != null)
                {
                    this.GetInnerViewControl().AutoSetDocumentDefaultFont = value;
                }
            }
        }
        /// <summary>
        /// 设置输入域选择多个下拉项目
        /// </summary>
        /// <param name="id">输入域编号</param>
        /// <param name="indexs">从0开始的下拉项目序号，各个序号之间用逗号分开</param>
        /// <returns>操作是否修改文档内容</returns>
        public virtual bool SetInputFieldSelectedIndexs(string id, string indexs)
        {
            if (this.GetInnerViewControl() == null)
            {
                return false;
            }
            else
            {
                return this.Document.SetInputFieldSelectedIndexs(id, indexs);
            }
        }

        /// <summary>
        /// 编辑器调用的设置文档的默认字体和颜色
        /// </summary>
        /// <param name="font">默认字体</param>
        /// <param name="color">默认文本颜色</param>
        /// <param name="updateUI">是否更新用户界面</param>
        public void EditorSetDefaultFont(XFontValue font, Color color, bool updateUI)
        {
            if (this.GetInnerViewControl() != null)
            {
                this.GetInnerViewControl().EditorSetDefaultFont(font, color, updateUI);
            }
        }

        /// <summary>
        /// 移动焦点使用的快捷键
        /// </summary>
        [DefaultValue(MoveFocusHotKeys.None)]
        public MoveFocusHotKeys MoveFocusHotKey
        {
            get
            {
                if (this.GetInnerViewControl() == null)
                {
                    return MoveFocusHotKeys.None;
                }
                else
                {
                    return this.GetInnerViewControl().MoveFocusHotKey;
                }
            }
            set
            {
                if (this.GetInnerViewControl() != null)
                {
                    this.GetInnerViewControl().MoveFocusHotKey = value;
                }
            }
        }

      
        /// <summary>
        /// 文档内容只读标记
        /// </summary>
        public bool Readonly
        {
            get
            {
                if (this.GetInnerViewControl() == null)
                {
                    return false;
                }
                else
                {
                    return this.GetInnerViewControl().Readonly;
                }
            }
            set
            {
                if (this.GetInnerViewControl() != null)
                {
                    this.GetInnerViewControl().Readonly = value;
                }
            }
        }
         
        /// <summary>
        /// 文档设置
        /// </summary>
        public DocumentOptions DocumentOptions
        {
            get
            {
                if (this.myViewControl == null)
                {
                    return null;
                }
                else
                {
                    return this.myViewControl.DocumentOptions;
                }
            }
            set
            {
                if (this.GetInnerViewControl() != null)
                {
                    this.GetInnerViewControl().DocumentOptions = value;
                }
            }
        }
        /// <summary>
        /// 文档视图选项
        /// </summary>
        public DocumentViewOptions DocumentViewOptions
        {
            get
            {
                if (this.myViewControl == null)
                {
                    return null;
                }
                else
                {
                    return this.myViewControl.DocumentOptions.ViewOptions;
                }
            }
        }
        /// <summary>
        /// 文档行为选项
        /// </summary>
        public DocumentBehaviorOptions DocumentBehaviorOptions
        {
            get
            {
                if (this.myViewControl  == null)
                {
                    return null;
                }
                else
                {
                    return this.myViewControl.DocumentOptions.BehaviorOptions;
                }
            }
        }

        /// <summary>
        /// 文档编辑选项
        /// </summary>
        public DocumentEditOptions DocumentEditOptions
        {
            get
            {
                if (this.myViewControl == null)
                {
                    return null;
                }
                else
                {
                    return this.myViewControl.DocumentOptions.EditOptions;
                }
            }
        }
        

        /// <summary>
        /// 文档控制器
        /// </summary>
        internal DocumentControler DocumentControler
        {
            get
            {
                if (this.GetInnerViewControl() == null )
                {
                    return null;
                }
                else
                {
                    return this.GetInnerViewControl().DocumentControler;
                }
            }
            set
            {
                if (this.GetInnerViewControl() != null)
                {
                    this.GetInnerViewControl().DocumentControler = value;
                }
            }
        }

        protected WriterAppHost _AppHost = null;
        /// <summary>
        /// 编辑器宿主对象
        /// </summary>
        public WriterAppHost AppHost
        {
            get
            {
                {
                    if (this._AppHost != null)
                    {
                        return this._AppHost;
                    }
                    else
                    {
                        return WriterAppHost.Default;
                    }
                }
            }
            set
            {
                this._AppHost = value;
            }
        }
       

        /// <summary>
        /// 取消格式刷操作,DCWriter内部使用。
        /// </summary>
        public void CancelFormatBrush()
        {
            this.myViewControl?.CancelFormatBrush();
        }


        /// <summary>
        /// 刷新文档
        /// </summary>
        public void RefreshDocument()
        {
            try
            {
                this.myViewControl.RefreshDocument();
            }
            catch (Exception ext)
            {
                MessageBox.Show(ext.ToString());
            }
        }
        /// <summary>
        /// 刷新文档
        /// </summary>
        /// <param name="refreshSize">是否重新计算元素大小</param>
        /// <param name="executeLayout">是否进行文档内容重新排版</param>
        public void RefreshDocumentExt(bool refreshSize, bool executeLayout)
        {
            {
                this.myViewControl.RefreshDocumentExt(refreshSize, executeLayout);
                //InnerOnEventRefreshDomTree(this.myViewControl.Document);
                //if (this.DeveloperToolsVisible)
                //{
                //    // 更新开发者工具中的文档视图
                //    ((IDeveloperToolsControl)this._DevTools).RefreshView(false);
                //}
            }
        }
        /// <summary>
        /// 正在刷新整个文档的视图
        /// </summary>
        internal static bool RefreshingDocumentView = false;
        /// <summary>
        /// 刷新文档内部排版和分页。
        /// </summary>
        /// <param name="bolFastMode">是否为快速模式，如果为快速模式，则不重新绘制文档</param>
        public void RefreshInnerView(bool bolFastMode)
        {
            var doc = this.myViewControl?.Document;
            if (this.IsHandleCreated && doc != null)
            {
                try
                {
                    RefreshingDocumentView = true;
                    doc.RefreshInnerView(bolFastMode);
                    //var pages = this.Pages;
                    this.UpdatePages();
                    foreach (DomDocumentContentElement dce in doc.Elements)
                    {
                        var s = dce.Content;
                    }
                    this.myViewControl.Invalidate();
                }
                finally
                {
                    RefreshingDocumentView = false;
                }
            }
        }

        /// <summary>
        /// 更新文档页状态
        /// </summary>

        public void UpdatePages()
        {
            {
                this.myViewControl.UpdatePages();
                this.UpdateRuleState();
            }
        }
        
        /// <summary>
        /// 表单数据组成的字符串数组，序号为偶数的元素为名称，序号为奇数的元素为数值。
        /// </summary>
        public string[] FormValuesArray
        {
            get
            {
                Hashtable values = this.Document?.FormValues;
                if (values != null && values.Count > 0 )
                {
                    List<string> result = new List<string>();
                    foreach (string name in values.Keys)
                    {
                        result.Add(name);
                        result.Add((string)values[name]);
                    }
                    return result.ToArray();
                }
                else
                {
                    return null;
                }
            }
             
        } 
       
        /// <summary>
        /// 根据当前元素更新光标
        /// </summary>
        public void UpdateTextCaret()
        {
            if (this.myViewControl != null)
            {
                this.myViewControl.UpdateTextCaret();
            }
        }

        /// <summary>
        /// 根据当前元素更新光标，而且不会造成用户视图区域的滚动
        /// </summary>
        public void UpdateTextCaretWithoutScroll()
        {
            if (this.myViewControl != null)
            {
                this.myViewControl.UpdateTextCaretWithoutScroll();
            }
        }

        /// <summary>
        /// 文档中当前元素或被选择区域的开始位置在编辑器控件客户区中的坐标
        /// </summary>
        public Point SelectionStartPosition
        {
            get
            {
                if (this.myViewControl == null)
                {
                    return Point.Empty;
                }
                else
                {
                    return this.myViewControl.SelectionStartPosition();
                }
            }
        }

        /// <summary>
        /// 选中文档所有内容
        /// </summary>
        public void SelectAll()
        {
            if (this.myViewControl != null)
            {
                this.myViewControl.SelectAll();
            }
        }

        /// <summary>
        /// 删除选择区域
        /// </summary>
        public void DeleteSelection(bool showUI)
        {
            if (this.myViewControl != null)
            {
                this.myViewControl.DeleteSelection(showUI);
            }
        }

        /// <summary>
        ///  表示当前插入点位置信息的字符串
        /// </summary>
        public string PositionInfoText
        {
            get
            {
                if (this.myViewControl == null)
                {
                    return null;
                }
                else
                {
                    return this.myViewControl.PositionInfoText();
                }
            }
        }


        /// <summary>
        /// 执行文档元素默认方法
        /// </summary>
        /// <param name="element">文档元素对象</param>
        public void ExecuteElementDefaultMethod(DomElement element)
        {
            if (this.myViewControl != null)
            {
                this.myViewControl.ExecuteElementDefaultMethod(element);
            }
        }
        /// <summary>
        /// 取消当前的编辑元素内容的操作
        /// </summary>
        /// <returns>操作是否成功</returns>
        public bool CancelEditElementValue()
        {
            if (this.myViewControl == null)
            {
                return false;
            }
            else
            {
                return this.myViewControl.CancelEditElementValue();
            }
        }
         
        /// <summary>
        /// 开始执行编辑元素内容值的操作
        /// </summary>
        /// <param name="element">文档元素对象</param>
        /// <returns>操作是否成功</returns>
        public bool BeginEditElementValue(DomElement element)
        {
            if (this.myViewControl == null)
            {
                return false;
            }
            else
            {
                return this.myViewControl.BeginEditElementValue(element);
            }
        }
        internal bool InnerBeginEditElementValueAllowBeginInvoke(
            DomElement element,
            bool detectOnly,
            ValueEditorActiveMode sourceMode,
            bool checkActiveMode,
            bool checkRecursion)
        {
            if( this.myViewControl == null )
            {
                return false;
            }
            if( this.EditorHost().EditingValue   )
            {
                // 正在显示下拉列表，则关闭下拉列表，然后延时显示新的下拉列表。
                this.EditorHost().CancelEditValue();
                return false;
            }
            else
            {
                return this.myViewControl.BeginEditElementValue(
                    element,
                    detectOnly,
                    sourceMode,
                    checkActiveMode,
                    checkRecursion);
            }
        }

        /// <summary>
        /// 开始执行编辑元素内容值的操作
        /// </summary>
        /// <param name="element">元素对象</param>
        /// <param name="detectOnly">本次调用只是检测当前元素的值能否编辑，但不执行编辑操作</param>
        /// <param name="sourceMode">发起编辑操作的来源</param>
        /// <param name="checkActiveMode">是否检测激活模式</param>
        /// <param name="checkRecursion">检查递归</param>
        /// <returns>操作是否成功</returns>
        public bool InnerBeginEditElementValue(
            DomElement element,
            bool detectOnly,
            ValueEditorActiveMode sourceMode,
            bool checkActiveMode,
            bool checkRecursion)
        {
            if (this.myViewControl == null)
            {
                return false;
            }
            else
            {
                return this.myViewControl.BeginEditElementValue(
                    element,
                    detectOnly,
                    sourceMode,
                    checkActiveMode,
                    checkRecursion);
            }
        }

        /// <summary>
        /// DCWriter内部使用。
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public SimpleRectangleTransform GetTransformItemByDescPoint(float x, float y)
        {
            return this.myViewControl?.GetTransformItemByDescPoint(x, y);
        }

        /// <summary>
        /// 编辑器宿主对象
        /// </summary>
        internal TextWindowsFormsEditorHostClass EditorHost()
        {
            if (this.myViewControl == null)
            {
                return null;
            }
            else
            {
                return this.myViewControl.EditorHost;
            }
        }

#region 兼容性接口

        /// <summary>
        /// 页面集合
        /// </summary>
        public PrintPageCollection Pages
        {
            get
            {
                return this.myViewControl.Pages;
            }
            set
            {
                this.myViewControl.Pages = value;
            }
        }


        /// <summary>
        /// 总页数
        /// </summary>
        public int PageCount
        {
            get
            {
                return this.myViewControl.PageCount;
            }
        }

        /// <summary>
        /// 跳到指定页,页号从0开始计算。
        /// </summary>
        /// <param name="index">从0开始的页号</param>
        /// <returns>操作是否成功</returns>
        public bool MoveToPage(int index)
        {
            return this.myViewControl.MoveToPage(index);
        }

        /// <summary>
        /// 从1开始计算的当前页号
        /// </summary>
        public int CurrentPageIndex
        {
            get
            {
                if (this.myViewControl == null)
                {
                    return 1;
                }
                else
                {
                    return this.myViewControl.CurrentPageIndex;
                }
            }
            set
            {
                if (this.myViewControl != null)
                {
                    this.myViewControl.CurrentPageIndex = value;
                }
            }
        }

        /// <summary>
        /// 当前页对象
        /// </summary>
        public PrintPage CurrentPage
        {
            get
            {
                return this.myViewControl?.CurrentPage;
            }
            set
            {
                if (this.myViewControl != null)
                {
                    this.myViewControl.CurrentPage = value;
                }
            }
        }
       

        /// <summary>
        /// 当前是否处于插入模式,若处于插入模式,则光标比较细,否则处于改写模式,光标比较粗
        /// </summary>
        public virtual bool InsertMode
        {
            get
            {
                return this.myViewControl.InsertMode;
            }
            set
            {
                this.myViewControl.InsertMode = value;
            }
        }
       
#endregion
          
        /// <summary>
        /// 系统清理内存。这个过程是耗时间的。
        /// </summary>
        public void GCCollect()
        {
            GC.Collect();
            //GC.WaitForPendingFinalizers();
            //GC.WaitForFullGCComplete();
        }
    }//public partial class WriterControl : TextPageViewControl
}