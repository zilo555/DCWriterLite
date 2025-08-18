using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing ;
using System.ComponentModel;

namespace DCSoft.Writer.Commands
{
    /// <summary>
    /// 动作方法声明特性
    /// </summary>
     
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class , AllowMultiple=false )]
    public sealed class WriterCommandDescriptionAttribute : Attribute 
    {
        /// <summary>
        /// 初始化对象
        /// </summary>
        public WriterCommandDescriptionAttribute()
        {
        }

        /// <summary>
        /// 初始化对象
        /// </summary>
        /// <param name="name">动作名称</param>
        public WriterCommandDescriptionAttribute(string name)
        {
            _Name = name;
        }
         

        private string _Name = null;
        /// <summary>
        /// 动作名称
        /// </summary>
        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }

        private int _Priority = 10;
        /// <summary>
        /// 优先级，值越低则越优先
        /// </summary>
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public int Priority
        {
            get { return _Priority; }
            set { _Priority = value; }
        }


        private System.Windows.Forms.Keys _ShortcutKey = System.Windows.Forms.Keys.None;
        /// <summary>
        /// 快捷键
        /// </summary>
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public System.Windows.Forms.Keys ShortcutKey
        {
            get { return _ShortcutKey; }
            set { _ShortcutKey = value; }
        }

        private Type _ReturnValueType = null;
        /// <summary>
        /// 返回值的数据类型
        /// </summary>
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public Type ReturnValueType
        {
            get
            {
                return _ReturnValueType; 
            }
            set
            {
                _ReturnValueType = value; 
            }
        }

        private object _DefaultResultValue = null;
        /// <summary>
        /// 默认的结果返回值
        /// </summary>
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public object DefaultResultValue
        {
            get
            {
                return _DefaultResultValue;
            }
            set
            {
                _DefaultResultValue = value;
            }
        }
         
    }
}
