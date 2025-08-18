using System;
using System.Collections.Generic;
using System.Text;
using DCSoft.Writer.Dom ;
using System.Windows.Forms;
using DCSoft.WinForms;
using DCSoft.Common;

namespace DCSoft.Writer.Controls
{    
    /// <summary>
    /// 文档元素内容编辑器
    /// </summary>
    public abstract class ElementValueEditor
    {
        /// <summary>
        /// 编辑元素数值
        /// </summary>
        /// <param name="host">编辑器宿主对象</param>
        /// <param name="context">上下文对象</param>
        /// <returns>编辑结果</returns>
        public virtual ElementValueEditResult EditValue(
            TextWindowsFormsEditorHostClass host,
            ElementValueEditContext context )
        {
            return ElementValueEditResult.None ;
        }

        /// <summary>
        /// 获得编辑元素的方式
        /// </summary>
        /// <param name="host">编辑器宿主</param>
        /// <param name="context">编辑上下文对象</param>
        /// <returns>编辑的方式</returns>
        public virtual ElementValueEditorEditStyle GetEditStyle(
            TextWindowsFormsEditorHostClass host,
            ElementValueEditContext context)
        {
            return ElementValueEditorEditStyle.None;
        }

        //private static List<ElementValueEditor> _Instances = new List<ElementValueEditor>();
        ///// <summary>
        ///// 获得元素值编辑器对象实例
        ///// </summary>
        ///// <param name="editorType">编辑器对象类型</param>
        ///// <returns>获得的编辑器对象实例</returns>
        //public static ElementValueEditor GetInstance(Type editorType)
        //{
        //    if (editorType == null)
        //    {
        //        throw new ArgumentNullException("editorType");
        //    }
        //    if (typeof(ElementValueEditor).IsAssignableFrom(editorType) == false)
        //    {
        //        throw new ArgumentOutOfRangeException(editorType.FullName);
        //    }
        //    foreach (ElementValueEditor editor in _Instances)
        //    {
        //        if (editor.GetType().Equals(editorType))
        //        {
        //            return editor;
        //        }
        //    }
        //    foreach (ElementValueEditor editor in _Instances)
        //    {
        //        if (editorType.IsInstanceOfType(editor))
        //        {
        //            return editor;
        //        }
        //    }
        //    ElementValueEditor newEditor = (ElementValueEditor)System.Activator.CreateInstance(editorType);
        //    _Instances.Add(newEditor);
        //    return newEditor;
        //}
    }
}
