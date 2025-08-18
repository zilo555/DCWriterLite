using System;
using System.Collections.Generic;
using System.Text;
using DCSoft.Writer.Controls ;
using System.ComponentModel;
using DCSoft.Common ;
using DCSoft.Drawing;
using DCSoft.Writer.Dom;
namespace DCSoft.Writer.Commands
{
    /// <summary>
    /// 编辑器命令事件委托类型
    /// </summary>
    /// <param name="eventSender">参数</param>
    /// <param name="args">参数</param>
    public delegate void WriterCommandEventHandler(
        object eventSender,
        WriterCommandEventArgs args);

    /// <summary>
    /// 编辑器命令事件参数
    /// </summary>
    public partial class WriterCommandEventArgs  
    {
        /// <summary>
        /// 初始化对象
        /// </summary>
        /// <param name="ctl">编辑器控件</param>
        /// <param name="document">文档对象</param>
        /// <param name="mode">命令模式</param>
        /// <param name="cmdCtl">控制器对象</param>
        public WriterCommandEventArgs(
            WriterControl ctl,
            DomDocument document,
            WriterCommandEventMode mode,
            WriterCommandControler cmdCtl )
        {
            _CommandControler = cmdCtl;
            _EditorControl = ctl;
            if (_EditorControl != null)
            {
                _CommandControler = _EditorControl.CommandControler;
                _Host = _EditorControl.AppHost;
            }
            _Document = document;
            _Mode = mode;
        }

        private WriterCommandControler _CommandControler = null;
        /// <summary>
        /// 命令控制器
        /// </summary>
        [System.Text.Json.Serialization.JsonIgnore]
        public WriterCommandControler CommandControler
        {
            get { return _CommandControler; }
        }

        private bool _RaiseFromUI = false;
        /// <summary>
        /// 由于用户界面菜单按钮操作而触发命令
        /// </summary>
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public bool RaiseFromUI
        {
            get
            {
                return _RaiseFromUI; 
            }
            set
            {
                _RaiseFromUI = value; 
            }
        }

        private string _Name = null;
        /// <summary>
        /// 命令名称
        /// </summary>
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }
        private WriterAppHost _Host = null;
        /// <summary>
        /// 编辑器宿主对象
        /// </summary>
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        [System.Text.Json.Serialization.JsonIgnore]
        public WriterAppHost Host
        {
            get
            {
                return _Host; 
            }
            set
            {
                _Host = value; 
            }
        }

        private DomDocument _Document = null;
        /// <summary>
        /// 文档对象
        /// </summary>
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        [System.Text.Json.Serialization.JsonIgnore]
        public DomDocument Document
        {
            get { return _Document; }
        }

        /// <summary>
        /// 文档内容控制器
        /// </summary>
        public DocumentControler DocumentControler
        {
            get
            {
                if (_EditorControl != null)
                {
                    return _EditorControl.DocumentControler;
                }
                if (_Document != null)
                {
                    return _Document.DocumentControler;
                }
                return null;
            }
        }

        [NonSerialized]
        private WriterControl _EditorControl = null;
        /// <summary>
        /// 编辑器控件对象
        /// </summary>
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        [System.Text.Json.Serialization.JsonIgnore]
        public WriterControl EditorControl
        {
            get
            {
                return _EditorControl; 
            }
        }
        [System.Text.Json.Serialization.JsonIgnore]
        public WriterViewControl ViewControl
        {
            get
            {
                return this._EditorControl.GetInnerViewControl();
            }
        }

        public WriterViewControl GetInnerViewControl()
        {
            if( this._EditorControl == null )
            {
                return null;
            }
            else
            {
                return this._EditorControl.GetInnerViewControl();
            }
        }
        

        private WriterCommandEventMode _Mode = WriterCommandEventMode.QueryState;
        /// <summary>
        /// 参数模式
        /// </summary>
        ////[DCPublishAPI]
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public WriterCommandEventMode Mode
        {
            get
            {
                return _Mode; 
            }
            set
            {
                _Mode = value; 
            }
        }


        private bool bolAltKey = false;
        /// <summary>
        /// 用户是否按下了 Alt 键
        /// </summary>
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public bool AltKey
        {
            get
            {
                return bolAltKey;
            }
            set
            {
                bolAltKey = value;
            }
        }

        private bool bolCtlKey = false;
        /// <summary>
        /// 用户是否按下的 Ctl 键
        /// </summary>
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public bool CtlKey
        {
            get
            {
                return bolCtlKey;
            }
            set
            {
                bolCtlKey = value;
            }
        }

        private bool bolShiftKey = false;
        /// <summary>
        /// 用户是否按下了 Shift 键
        /// </summary>
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public bool ShiftKey
        {
            get
            {
                return bolShiftKey;
            }
            set
            {
                bolShiftKey = value;
            }
        }

        private System.Windows.Forms.Keys intKeyCode
            = System.Windows.Forms.Keys.None;
        /// <summary>
        /// 键盘按键值
        /// </summary>
        public System.Windows.Forms.Keys KeyCode
        {
            get
            {
                return intKeyCode;
            }
            set
            {
                intKeyCode = value;
            }
        }

        /// <summary>
        /// KeyCode的COM接口
        /// </summary>
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public int KeyCodeValue
        {
            get
            {
                return (int)intKeyCode;
            }
            set
            {
                intKeyCode = (System.Windows.Forms.Keys)value;
            }
        }



        internal char intKeyChar = char.MinValue;
        /// <summary>
        /// 键盘字符值
        /// </summary>
        ////[DCPublishAPI]
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public char KeyChar
        {
            get
            {
                return intKeyChar;
            }
        }

        private DomElement _SourceElement = null;
        /// <summary>
        /// 执行动作相关的元素对象
        /// </summary>
        ////[DCPublishAPI]
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        [System.Text.Json.Serialization.JsonIgnore]
        public DomElement SourceElement
        {
            get { return _SourceElement; }
            set { _SourceElement = value; }
        }

        private bool _EnableSetUITextAsParamter = false;
        /// <summary>
        /// 能否将用户界面元素的文本当做命令参数
        /// </summary>
         
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public bool EnableSetUITextAsParamter
        {
            get
            {
                return _EnableSetUITextAsParamter; 
            }
            set
            {
                _EnableSetUITextAsParamter = value; 
            }
        }

        private bool _SetParameterToUIText = false;
        /// <summary>
        /// 是否将命令参数设置到UI界面元素的文本值
        /// </summary>
         
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public bool SetParameterToUIText
        {
            get
            {
                return _SetParameterToUIText; 
            }
            set
            {
                _SetParameterToUIText = value; 
            }
        }

        private object _Parameter = null;
        /// <summary>
        /// 相关参数对象
        /// </summary>
        ////[DCPublishAPI]
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        [System.Text.Json.Serialization.JsonIgnore]
        public object Parameter
        {
            get
            {
                return _Parameter; 
            }
            set
            {
                _Parameter = value; 
            }
        }

        private bool _ShowUI = true;
        /// <summary>
        /// 允许显示图形化用户界面
        /// </summary>
        ////[DCPublishAPI]
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public bool ShowUI
        {
            get 
            {
                return _ShowUI; 
            }
            set
            {
                _ShowUI = value; 
            }
        }


        private bool _Enabled = true;
        /// <summary>
        /// 动作是否可用
        /// </summary>
        ////[DCPublishAPI]
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public bool Enabled
        {
            get { return _Enabled; }
            set { _Enabled = value; }
        }

        private bool _Visible = true;
        /// <summary>
        /// 对象是否可见
        /// </summary>
        ////[DCPublishAPI]
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public bool Visible
        {
            get { return _Visible; }
            set { _Visible = value; }
        }

        private bool _Checked = false ;
        /// <summary>
        /// 动作是否处于选择状态
        /// </summary>
        ////[DCPublishAPI]
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public bool Checked
        {
            get { return _Checked; }
            set { _Checked = value; }
        }

        private bool _Actived = false;
        /// <summary>
        /// 动作是否处于激活状态
        /// </summary>
        ////[DCPublishAPI]
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public bool Actived
        {
            get { return _Actived; }
            set { _Actived = value; }
        }

        private bool _Cancel = false;
        /// <summary>
        /// 取消操作
        /// </summary>
        ////[DCPublishAPI]
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public bool Cancel
        {
            get
            {
                return _Cancel; 
            }
            set
            {
                _Cancel = value; 
            }
        }

        private object _Result = null;
        /// <summary>
        /// 命令返回值
        /// </summary>
        ////[DCPublishAPI]
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        [System.Text.Json.Serialization.JsonIgnore]
        public object Result
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

        private UIStateRefreshLevel _RefreshLevel = UIStateRefreshLevel.Current ;
        /// <summary>
        /// 用户界面命令控件刷新等级
        /// </summary>
        ////[DCPublishAPI]
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public UIStateRefreshLevel RefreshLevel
        {
            get
            {
                return _RefreshLevel; 
            }
            set
            {
                _RefreshLevel = value; 
            }
        }

    }

    /// <summary>
    /// 用户界面命令控件刷新等级
    /// </summary>
    public enum UIStateRefreshLevel
    {
        /// <summary>
        /// 不刷新
        /// </summary>
        None ,
        /// <summary>
        /// 只刷新当前命令绑定的用户界面控件的状态
        /// </summary>
        Current ,
        /// <summary>
        /// 刷新所有的用户界面控件的状态
        /// </summary>
        All 
    }
    

    /// <summary>
    /// 执行动作参数类型
    /// </summary>
    public enum WriterCommandEventMode
    {
        /// <summary>
        /// 初始化用户界面控件
        /// </summary>
        InitalizeUIElement ,
        /// <summary>
        /// 更新用户界面控件
        /// </summary>
        UpdateUIElement ,
        /// <summary>
        /// 查询参数状态
        /// </summary>
        QueryState ,
        /// <summary>
        /// 查询命令激活状态
        /// </summary>
        QueryActive ,
        /// <summary>
        /// 开始执行动作
        /// </summary>
        BeforeExecute ,
        /// <summary>
        /// 执行动作
        /// </summary>
        Invoke ,
        /// <summary>
        /// 结束执行动作
        /// </summary>
        AfterExecute,
        /// <summary>
        /// 无任何操作，只是内部使用。
        /// </summary>
        InnerNop
    }
}
