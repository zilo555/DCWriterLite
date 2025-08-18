using System;
using DCSoft.Printing;
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
    public partial class DomDocument
    {
        private ContentProtectedInfoList _ContentProtectedInfos = null;
        /// <summary>
        /// 内部使用：当前受保护的文档元素信息列表
        /// </summary>
        public ContentProtectedInfoList ContentProtectedInfos
        {
            get
            {
                if (_ContentProtectedInfos == null)
                {
                    _ContentProtectedInfos = new ContentProtectedInfoList();
                }
                return _ContentProtectedInfos;
            }
        }

        /// <summary>
        /// 内部使用：设置当前受保护的文档元素信息列表
        /// </summary>
        public void SetContentProtectedInfos(ContentProtectedInfoList list)
        {
            _ContentProtectedInfos = list;
        }

        /// <summary>
        /// 判断是否存在内容保护相关信息
        /// </summary>
        public bool HasContentProtectedInfos()
        {
             
                return _ContentProtectedInfos != null && _ContentProtectedInfos.Count > 0;
             
        }
        /// <summary>
        /// 清空内容保护信息列表
        /// </summary>
        public void ClearContentProtectedInfos()
        {
            this._ContentProtectedInfos = null;
        }
         
        /// <summary>
        /// 提示遇到受保护的文档内容
        /// </summary>
        /// <param name="specifyInfos">指定的内容保护信息对象</param>
        /// <returns>是否显示的用户界面</returns>
         
        public bool PromptProtectedContent(ContentProtectedInfoList specifyInfos)
        {
            ContentProtectedInfoList es = specifyInfos;
            if (es == null || es.Count == 0)
            {
                es = this._ContentProtectedInfos;
                this._ContentProtectedInfos = null;
            }
            if (es == null || es.Count == 0)
            {
                return false;
            }
            for (int iCount = es.Count - 1; iCount >= 0; iCount--)
            {
                if (es[iCount].Element is DomParagraphFlagElement)
                {
                    // 删除文档块中的最后一个段落符号元素
                    DomParagraphFlagElement pf = (DomParagraphFlagElement)es[iCount].Element;
                    if (pf.IsLastElementInContentElement())
                    {
                        es.RemoveAt(iCount);
                        continue;
                    }
                }
            }
            if (es != null && es.Count > 0)
            {
                for (int iCount = es.Count - 1; iCount >= 0; iCount--)
                {
                    DomElement element = es[iCount].Element;
                    if (element is DomFieldBorderElement)
                    {
                        DomElement parent = element.Parent;
                        if (es.GetInfo(element.Parent) == null)
                        {
                            es[iCount].Element = parent;
                        }
                        else
                        {
                            es.RemoveAt(iCount);
                        }
                    }
                    else if (element.Parent is DomFieldElementBase && element is DomCharElement)
                    {
                        DomFieldElementBase field = (DomFieldElementBase)element.Parent;
                        if (field.IsBackgroundTextElement(element))
                        {
                            DomElement parent = element.Parent;
                            if (es.GetInfo(element.Parent) == null)
                            {
                                es[iCount].Element = parent;
                            }
                            else
                            {
                                es.RemoveAt(iCount);
                            }
                        }
                    }
                }//for
                var _promptProtectedContent = this.GetDocumentBehaviorOptions().PromptProtectedContent;

                string msg = null;
                var str_PromptProtectedContent = DCSR.PromptProtectedContent;
                //if (str_PromptProtectedContent == null || str_PromptProtectedContent.Length == 0)
                //{
                //    str_PromptProtectedContent = DCSR.PromptProtectedContent;
                //}
                switch (_promptProtectedContent)
                {
                    case PromptProtectedContentMode.None:
                        msg = str_PromptProtectedContent + Environment.NewLine + es[0].Message;
                        break;
                    case PromptProtectedContentMode.Simple:
                        msg = str_PromptProtectedContent + Environment.NewLine + es[0].Message;
                        break;
                    case PromptProtectedContentMode.Details:
                        msg = str_PromptProtectedContent + Environment.NewLine + es.ToDetailsString();
                        break;
                }
                switch (_promptProtectedContent)
                {
                    case PromptProtectedContentMode.None:
                        break;
                    case PromptProtectedContentMode.Simple:
                        DCSoft.WinForms.DCUIHelper.ShowWarringMessageBox(
                            this.EditorControl,
                            msg );
                        break;
                    case PromptProtectedContentMode.Details:
                        DCSoft.WinForms.DCUIHelper.ShowWarringMessageBox(
                            this.EditorControl,
                            msg );
                        break;
                }
                return true;
            }
            return false;
        }
    }
}
