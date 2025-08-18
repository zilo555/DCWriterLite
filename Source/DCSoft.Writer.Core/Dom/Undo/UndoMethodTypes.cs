using System;
using System.Collections.Generic;
using System.Text;

namespace DCSoft.Writer.Dom.Undo
{
    /// <summary>
    /// 撤销操作方法类型
    /// </summary>
    public enum UndoMethodTypes
    {
        /// <summary>
        /// 无任何操作
        /// </summary>
        None,
        /// <summary>
        /// 重新分页
        /// </summary>
        RefreshPages,
        /// <summary>
        /// 刷新文档布局
        /// </summary>
        RefreshLayout ,
        /// <summary>
        /// 刷新整个文档
        /// </summary>
        RefreshDocument,
        /// <summary>
        /// 刷新文档视图
        /// </summary>
        UpdateDocumentView ,
        /// <summary>
        /// 重新绘制整个文档
        /// </summary>
        Invalidate,
        /// <summary>
        /// 刷新段落层次树
        /// </summary>
        RefreshParagraphTree,
        /// <summary>
        /// 更新表达式引擎
        /// </summary>
        UpdateExpressionExecuter
    }
}
