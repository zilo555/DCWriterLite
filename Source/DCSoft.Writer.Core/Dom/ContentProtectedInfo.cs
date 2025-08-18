using System;
using System.Collections.Generic;
using System.Text;

namespace DCSoft.Writer.Dom
{
    /// <summary>
    /// 内容保护信息对象
    /// </summary>
    public class ContentProtectedInfo
    {
        /// <summary>
        /// 初始化对象
        /// </summary>
        /// <param name="element">文档元素对象</param>
        /// <param name="msg">消息</param>
        public ContentProtectedInfo(DomElement element, string msg , ContentProtectedReason reason )
        {
            _Element = element;
            _Message = msg;
            this._Reason = reason;
        }

        private DomElement _Element = null;
        /// <summary>
        /// 文档元素对象
        /// </summary>
        public DomElement Element
        {
            get { return _Element; }
            set { _Element = value; }
        }
        private string _Message = null;
        /// <summary>
        /// 消息
        /// </summary>
        public string Message
        {
            get { return _Message; }
            set { _Message = value; }
        }

        private ContentProtectedReason _Reason = ContentProtectedReason.None;
        /// <summary>
        /// 内容被保护的理由
        /// </summary>
        public ContentProtectedReason Reason
        {
            get { return _Reason; }
            set { _Reason = value; }
        }
    }

    /// <summary>
    /// 内容保护的理由
    /// </summary>
    [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
    public enum ContentProtectedReason
    {
        /// <summary>
        /// 无理由
        /// </summary>
        None ,
        ControlReadonly,
        Lock,
        ContentProtectStyle,
        UserEvent,
        ContainerReadonly ,
        UnDeleteable
 
    }
    /// <summary>
    /// 内容报表提示信息列表
    /// </summary>
#if ! RELEASE
    [System.Diagnostics.DebuggerDisplay("Count={ Count }")]
    [System.Diagnostics.DebuggerTypeProxy(typeof(DCSoft.Common.ListDebugView))]
#endif  
    public class ContentProtectedInfoList : List<ContentProtectedInfo>
    {
        /// <summary>
        /// 初始化对象
        /// </summary>
        public ContentProtectedInfoList()
        {
        }

        /// <summary>
        /// 获得提示信息
        /// </summary>
        /// <param name="element">文档元素对象</param>
        /// <returns>获得的信息</returns>
        public ContentProtectedInfo GetInfo(DomElement element)
        {
            foreach (ContentProtectedInfo item in this)
            {
                if (item.Element == element)
                {
                    return item;
                }
            }
            return null;
        }
        /// <summary>
        /// 显示详细内容的文本
        /// </summary>
        /// <returns>文本</returns>
        public string ToDetailsString()
        {
            StringBuilder str = new StringBuilder();
            //XTextElement lastElement = null;
            ContentProtectedInfo lastInfo = null;
            foreach (ContentProtectedInfo info in this )
            {
                bool showTypeName = false;
                if (lastInfo == null)
                {
                    showTypeName = true;
                }
                else
                {
                    if (info.Element.GetType().Equals(lastInfo.Element.GetType()) == false)
                    {
                        str.AppendLine();
                        showTypeName = true;
                    }
                }
                if (showTypeName)
                {
                    str.Append(info.Element.DispalyTypeName());
                    str.Append(":");
                }
                lastInfo = info;
                if (info.Element is DomCharElement)
                {
                    DomCharElement c = (DomCharElement)info.Element;
                    str.Append(c._CharValue.ToString());
                }
                else
                {
                    if (lastInfo != null && lastInfo.Element is DomCharElement)
                    {
                        str.Append("#" + lastInfo.Message);
                    }
                    if (info.Element is DomInputFieldElementBase)
                    {
                        DomInputFieldElementBase field = (DomInputFieldElementBase)info.Element;
                        if (string.IsNullOrEmpty(field.Name) == false)
                        {
                            str.Append(field.Name);
                        }
                        else
                        {
                            str.Append(field.DisplayName);
                        }
                        str.Append(":");
                        //str.Append("ID:[" + field.ID + "] Name:[" + field.Name + "] ");
                        str.Append(WriterUtilsInner.GetLimitedString(field.Text, 50));
                        str.Append("#");
                        str.Append(info.Message);
                    }
                    else
                    {
                        str.Append(info.Element + "#" + info.Message);
                    }
                }
                if (str.Length > 1000)
                {
                    // 最多为1000个字符。
                    break;
                }
            }//foreach
            if (lastInfo != null && lastInfo.Element is DomCharElement)
            {
                str.Append("#" + lastInfo.Message);
            }
            return str.ToString();
        }
    }
}
