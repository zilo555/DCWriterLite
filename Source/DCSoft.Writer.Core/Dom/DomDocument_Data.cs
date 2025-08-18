using System;
using DCSoft.Printing;
using DCSoft.WinForms;

//using DCSoft.Writer.Dom.Undo;
using DCSoft.Writer.Serialization;


using System.ComponentModel;
using DCSoft.Drawing;
using DCSoft.Common;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using DCSoft.Writer.Data;
using System.Text;
using System.Runtime.InteropServices;

using DCSoft.Writer.Controls;

namespace DCSoft.Writer.Dom
{
    /// <summary>
    /// 文档参数、数据相关的代码群
    /// </summary>
    public partial class DomDocument
    {
        /// <summary>
        /// 重置表单元素默认值
        /// </summary>
        /// <returns>是否导致文档内容发生改变</returns>
        public bool ResetFormValue()
        {
            CheckDisposed();
            return DefaultDOMDataProvider.ResetFormValue(this);
        }
                /// <summary>
                 /// 获得文档中的表单数据字典
                 /// </summary>
        public Hashtable FormValues
        {
            get
            {
                return DefaultDOMDataProvider.GetFormValues(this);
            }
        } 

        /// <summary>
        /// 设置表单值
        /// </summary>
        /// <param name="name">表单元素名称</param>
        /// <param name="Value">表单值</param>
        /// <returns>操作是否修改了文档内容</returns>
       //[System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        //[DCSoft.Common.DCPublishAPI]
        public bool SetFormValue(string name, string Value)
        {
            return DefaultDOMDataProvider.SetFormValue(this, name, Value);
        }
#if ! LightWeight
        /// <summary>
        /// 获得表单数据
        /// </summary>
        /// <param name="name">字段名称</param>
        /// <returns>获得的表单数值</returns>
       //[System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        //[DCSoft.Common.DCPublishAPI]
        public string GetFormValue(string name)
        {
            return DefaultDOMDataProvider.GetFormValue(this, name);
        }
#endif


        /// <summary>
        /// 清空数据校验结果信息
        /// </summary>
        /// <returns>清除的信息个数</returns>
       //[System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        //[DCSoft.Common.DCPublishAPI]
        public int ClearValueValidateResult()
        {
            int result = 0;
            var fields = this.GetElementsByType<DomInputFieldElementBase>();
            if (fields.Count > 0)
            {
                foreach (DomInputFieldElementBase field in fields)
                {
                    if (field.ClearValidateResult(false))
                    {
                        result++;
                        field.InvalidateView();
                        if (this.HighlightManager != null)
                        {
                            this.HighlightManager.InvalidateHighlightInfo(field);
                        }
                    }
                }
            }
            if (result > 0)
            {
                if (this.EditorControl != null)
                {
                    this.EditorControl.UpdateToolTip(false);
                    this.InnerViewControl.Invalidate();
                }
            }
            return result;
        }

        [NonSerialized]
        internal int _ElementValueValidateVersion = 0;
        internal void IncreaseElementValueValidateVersion()
        {
            this._ElementValueValidateVersion++;
        }
        /// <summary>
        /// 对整个文档的输入域进行数据校验
        /// </summary>
        /// <returns>校验信息列表，如果返回空列表则校验全部通过</returns>
        //[System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        //[DCSoft.Common.DCPublishAPI]
        public ValueValidateResultList ValueValidate()
        {
            return DefaultDOMDataProvider.ValueValidate_Document(this, false);

        }




        /// <summary>
        /// 获得系统当前日期事件
        /// </summary>
        /// <returns>日期事件</returns>
        public DateTime GetNowDateTime()
        {
            return WriterControl.GetServerTime();
            //if(this._EditorControl == null )
            //{
            //    return DateTime.Now;
            //}
            //else
            //{
            //    return this._EditorControl.GetNowDateTime();
            //}

        }

    }
}
