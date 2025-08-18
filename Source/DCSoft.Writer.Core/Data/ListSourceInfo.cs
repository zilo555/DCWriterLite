using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using DCSoft.Writer.Dom;
//using System.Data;
using System.Collections;
using DCSoft.Writer.Controls;
using System.Runtime.InteropServices;

namespace DCSoft.Writer.Data
{
    /// <summary>
    /// 列表数据源信息
    /// </summary>
    /// <remarks>编制 袁永福</remarks>
    public sealed partial class ListSourceInfo : ICloneable
    {
        /// <summary>
        /// 初始化对象
        /// </summary>
        //[DCSoft.Common.DCPublishAPI]
        public ListSourceInfo()
        {
        }
         
        /// <summary>
        /// 对象内容是否为空
        /// </summary>
       //////[Browsable( false )]
        //[DCSoft.Common.DCPublishAPI]
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public bool IsEmpty
        {
            get
            {
                if( this.Items != null && this.Items.Count > 0 )
                {
                    // 存在静态列表项目
                    return false ;
                }
                return true; 
            }
        }

        private ListItemCollection _Items;
        /// <summary>
        /// 内置的列表项目
        /// </summary>
        public ListItemCollection Items
        {
            get
            {
                return _Items; 
            }
            set
            {
                _Items = value; 
            }
        }

         
        /// <summary>
        /// 复制对象
        /// </summary>
        /// <returns>复制品</returns>
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public ListSourceInfo Clone()
        {
            return (ListSourceInfo)((ICloneable)this).Clone();
        }

        /// <summary>
        /// 复制对象
        /// </summary>
        /// <returns>复制品</returns>
        object ICloneable.Clone()
        {
            ListSourceInfo info = (ListSourceInfo)this.MemberwiseClone();
            if (this._Items != null)
            {
                info._Items = new ListItemCollection();
                foreach (ListItem item in this._Items.FastForEach())
                {
                    info._Items.Add(item.Clone());
                }
            }
            return info;
        }
#if !RELEASE
        /// <summary>
        /// 返回表示对象的字符串
        /// </summary>
        /// <returns>字符串</returns>
        public override string ToString()
        {
            if (this.Items != null && this.Items.Count > 0)
            {
                return this.Items.Count + " items";
            }
            else
            {
                return string.Empty;
            }
        }
#endif
    }
}