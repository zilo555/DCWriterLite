using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel ;

namespace DCSoft.Common
{
    /// <summary>
    /// 属性字符串列表
    /// </summary>
    /// <remarks>编写 袁永福</remarks>
    public class AttributeString : List<AttributeStringItem>//, ICloneable
    {
        /// <summary>
        /// 初始化对象
        /// </summary>
        public AttributeString()
        {
        }

        /// <summary>
        /// 初始化对象
        /// </summary>
        /// <param name="listStr">字符串值</param>
        public AttributeString(string listStr)
        {
            this.Parse(listStr);
        }


        public AttributeStringItem GetItem( string name )
        {
            foreach (AttributeStringItem item in this)
            {
                if (string.Compare(item.Name, name, true) == 0)
                {
                    return item;
                }
            }
            return null;
        }
        /// <summary>
        /// 设置项目值
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="Value">值</param>
        public void SetValue(string name, string Value )
        {
            AttributeStringItem item = GetItem(name);
            if (item == null)
            {
                item = new AttributeStringItem();
                item.Name = name;
                item.Value = Value;
                this.Add(item);
            }
            else
            {
                item.Value = Value;
            }
        }


        /// <summary>
        /// 解析字符串，获得其中的数据
        /// </summary>
        /// <param name="text"></param>
        public void Parse(string text)
        {
            this.Clear();
            if (string.IsNullOrEmpty(text ))
            {
                return;
            }
            while (text.Length > 0 )
            {
                string newName = null;
                string newValue = null;
                int index = text.IndexOf(':');
                if (index > 0)
                {
                    newName = text.Substring(0, index);
                    text = text.Substring(index + 1);
                    if (text.StartsWith("'"))
                    {
                        int index2 = text.IndexOf('\'', 1);
                        if (index2 < 0)
                        {
                            index2 = text.IndexOf(';');
                        }
                        if (index2 >= 0)
                        {
                            newValue = text.Substring(1, index2-1);
                            text = text.Substring(index2 + 1) ;
                            if (text.StartsWith("'"))
                            {
                                text = text.Substring(1);
                            }
                        }
                        else
                        {
                            newValue = text.Substring(1);
                            text = string.Empty;
                        }
                    }//if
                    else if (text.StartsWith("\""))
                    {
                        int index2 = text.IndexOf('"', 1);
                        if (index2 < 0)
                        {
                            index2 = text.IndexOf(';');
                        }
                        if (index2 >= 0)
                        {
                            newValue = text.Substring(1, index2-1);
                            text = text.Substring(index2 + 1);
                            if (text.StartsWith("\""))
                            {
                                text = text.Substring(1);
                            
                            }
                            else if (text.StartsWith(";"))
                            {
                                text = text.Substring(1);
                            }
                        }
                        else
                        {
                            newValue = text.Substring(1);
                            text = string.Empty;
                        }
                    }//if
                    else
                    {
                        int index3 = text.IndexOf(';');
                        if (index3 >= 0)
                        {
                            newValue = text.Substring(0, index3);
                            text = text.Substring(index3 + 1);
                        }
                        else
                        {
                            newValue = text;
                            text = string.Empty;
                        }
                    }
                }
                else
                {
                    newName = text.Trim();
                    text = string.Empty;
                }
                AttributeStringItem item = new AttributeStringItem();
                item.Name = newName;
                item.Value = newValue;
                this.Add(item);
            }
        }

        /// <summary>
        /// 返回表示对象内容的字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder str = new StringBuilder();
            foreach (AttributeStringItem item in this)
            {
                if (str.Length > 0)
                {
                    str.Append(';');
                }
                str.Append(item.Name);
                str.Append(':');
                string txt = item.Value;
                if (txt != null && txt.Length > 0)
                {
                    if (txt.Contains(':') || txt.Contains(';'))
                    {
                        txt = '\'' + txt + '\'';
                    }
                    str.Append(txt);
                }
            }
            return str.ToString();
        }

        /// <summary>
        /// 将对象数据设置到
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="throwException"></param>
        /// <returns></returns>
        public int ApplyToInstance(object instance , bool throwException )
        {
            int result = 0;
            foreach (AttributeStringItem item in this)
            {
                if (ValueTypeHelper.SetPropertyValue(instance, item.Name, item.Value, false))
                {
                    result++;
                }
            }
            return result;
        }
    }

    /// <summary>
    /// 属性项目
    /// </summary>
    public class AttributeStringItem
    {
        /// <summary>
        /// 初始化对象
        /// </summary>
        public AttributeStringItem()
        {
        }

        private string _Name = null;
        /// <summary>
        /// 属性名
        /// </summary>
        public string Name
        {
            get
            {
                return _Name; 
            }
            set 
            {
                _Name = value; 
            }
        }

        private string _Value = null;
        /// <summary>
        /// 属性值
        /// </summary>
        [DefaultValue(null)]
        //[DCSystemXmlSerialization.XmlText]
        public string Value
        {
            get
            {
                return _Value; 
            }
            set
            {
                _Value = value; 
            }
        }
#if !(RELEASE || LightWeight)
        /// <summary>
        /// 返回表示对象的字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return this.Name + '=' + this.Value;
        }
#endif
    }
}
