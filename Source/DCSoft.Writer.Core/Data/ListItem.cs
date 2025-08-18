using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Runtime.InteropServices;
// // 
//using System.Drawing.Design;
//using System.Windows.Forms;
using DCSoft.Common;

namespace DCSoft.Writer.Data
{
    /// <summary>
    /// 列表项目
    /// </summary>
    /// <remarks>编制 袁永福</remarks>
    public partial class ListItem : ICloneable
    {
        /// <summary>
        /// 初始化对象
        /// </summary>
        //[DCSoft.Common.DCPublishAPI]
        public ListItem()
        {
        }
        /// <summary>
        /// 初始化对象
        /// </summary>
        /// <param name="strText">文本值</param>
        /// <param name="strValue">数值</param>
        internal ListItem( string strText , string strValue )
        {
            this._Text = strText;
            this._Value = strValue;
        }
         

       // [NonSerialized]
       // private float _DisplayWidth;

       // /// <summary>
       // /// 显示宽度
       // /// </summary>
         
       // [DCSystemXmlSerialization.XmlIgnore]
       //////[System.ComponentModel.Browsable( false )]
       //[System.Reflection.Obfuscation(Exclude = false , ApplyToMembers = true)]
       // public float DisplayWidth
       // {
       //     get 
       //     {
       //         return _DisplayWidth; 
       //     }
       //     set 
       //     {
       //         _DisplayWidth = value; 
       //     }
       // }

        private string _TextInList;
        /// <summary>
        /// 在下拉列表中的文本
        /// </summary>
        public string TextInList
        {
            get
            {
                return _TextInList; 
            }
            set
            {
                _TextInList = value; 
            }
        }

        private string _Text;
        /// <summary>
        /// 列表文本
        /// </summary>
        public string Text
        {
            get
            {
                return _Text;
            }
            set
            {
                if (_Text != value)
                {
                    _Text = value;
                    OnModified();
                }
            }
        }

        private string _Value;
        /// <summary>
        /// 列表项目值
        /// </summary>
        public string Value
        {
            get
            {
                return _Value;
            }
            set
            {
                if (_Value != value)
                {
                    _Value = value;
                    OnModified();
                }
            }
        }
          
        /// <summary>
        /// 运行时使用的数值
        /// </summary>
       //////[Browsable( false )]
        public string RuntimeValue
        {
            get
            {
                if (this._Value != null && this._Value.Length > 0)
                {
                    return this._Value;
                }
                else
                {
                    return this._Text;
                }

                //if (string.IsNullOrEmpty(_Value))
                //{
                //    return _Text;
                //}
                //else
                //{
                //    return _Value;
                //}
            }
        }

        private void OnModified()
        {
        }
#if !RELEASE
        /// <summary>
        /// 返回表示对象数据的字符串
        /// </summary>
        /// <returns>字符串</returns>
        public override string ToString()
        {
            //return "Text:" + this.Text + " Value:" + this.Value;
            return this.Text + "=" + this.Value;
        }
#endif
        /// <summary>
        /// 复制对象
        /// </summary>
        /// <returns>复制品</returns>
        //[System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public ListItem Clone()
        {
            return (ListItem)((ICloneable)this).Clone();
        }

        /// <summary>
        /// 复制对象
        /// </summary>
        /// <returns>复制品</returns>
        object ICloneable.Clone()
        {
            return this.MemberwiseClone();
        }
    }

    /// <summary>
    /// 列表项目列表
    /// </summary>
    /// <remarks>编制 袁永福</remarks>
#if !RELEASE
    [System.Diagnostics.DebuggerTypeProxy(typeof(DCSoft.Common.ListDebugView))]
#endif
    public partial class ListItemCollection : DCList<ListItem>
        , ICloneable//, IDCStringSerializable
    {
        /// <summary>
        /// 初始化对象
        /// </summary>
        public ListItemCollection()
        {
        }

        /// <summary>
        /// 数值转换为文本
        /// </summary>
        /// <param name="Value">数值</param>
        /// <returns>文本</returns>
        public string ValueToText(string Value)
        {
            foreach ( ListItem item in this)
            {
                if (item.Value == Value)
                {
                    return item.Text;
                }
            }
            return null;
        }
        /// <summary>
        /// 文本转换为数值
        /// </summary>
        /// <param name="Text">文本</param>
        /// <returns>数值</returns>

        public string TextToValue(string strText)
        {
            foreach ( ListItem item in this.FastForEach())
            {
                if (item.Text == strText)
                {
                    return item.Value;
                }
            }
            return null;
        }
        /// <summary>
        /// 复制对象
        /// </summary>
        /// <returns>复制品</returns>
        public ListItemCollection Clone()
        {
            return ( ListItemCollection ) ( ( ICloneable ) this ).Clone();
        }

        /// <summary>
        /// 复制对象
        /// </summary>
        /// <returns>复制品</returns>
        object ICloneable.Clone()
        {
            ListItemCollection list = new ListItemCollection();
            foreach (ListItem item in this)
            {
                list.Add(item.Clone());
            }
            return list;
        }
#if !RELEASE
        public override string ToString()
        {
            if (this.Count == 0)
            {
                return string.Empty;
            }
            else
            {
                return string.Format(DCSR.Items_Count, this.Count);
            }
        }
#endif
    }
}
