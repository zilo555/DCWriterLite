using System;
using System.Collections.Generic;
using System.Text;
using DCSoft.Common;
using DCSoft.Printing;
using DCSoft.Writer;

namespace DCSoft.Writer.Dom
{
    /// <summary>
    /// 文档对象列表
    /// </summary>
    /// <remarks>编制 袁永福</remarks>
#if !RELEASE
    [System.Diagnostics.DebuggerDisplay("Count={ Count }")]
    [System.Diagnostics.DebuggerTypeProxy(typeof(DCSoft.Common.ListDebugView))]
#endif
    public partial class DomDocumentList : List<DomDocument>
    {
        /// <summary>
        /// 初始化对象
        /// </summary>
        public DomDocumentList()
        {
        }
        /// <summary>
        /// 初始化对象并添加一个文档对象
        /// </summary>
        /// <param name="document"></param>
        public DomDocumentList(DomDocument document)
        {
            this.Add(document);
        }


        /// <summary>
        /// 第一个文档对象
        /// </summary>
        public DomDocument FirstDocument
        {
            get
            {
                if (this.Count > 0)
                {
                    return this[0];
                }
                else
                {
                    return null;
                }
            }
        }
        /// <summary>
        /// 所有文档对象的文档页集合
        /// </summary>
        public PrintPageCollection AllPages
        {
            get
            {
                PrintPageCollection ps = new PrintPageCollection();
                foreach (DomDocument doc in this)
                {
                    ps.AddRange(doc.Pages);
                }
                for (int iCount = 0; iCount < ps.Count; iCount++)
                {
                    ps[iCount].GlobalIndex = iCount;
                }
                return ps;
            }
        }
    }
}
