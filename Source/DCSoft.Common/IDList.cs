using System;
using System.Collections.Generic;
using System.Text;

namespace DCSoft.Common
{
    /// <summary>
    /// 编号列表
    /// </summary>
    public class IDList : System.Collections.IEnumerable 
    {
        /// <summary>
        /// 初始化对象
        /// </summary>
        public IDList()
        {
        }
        /// <summary>
        /// 初始化对象
        /// </summary>
        /// <param name="strIDList">编号列表，各个项目之间用逗号分开</param>
        public IDList(string strIDList)
        {
            this.Parse(strIDList, ',', '\0', '\0');
        }
        /// <summary>
        /// 初始化对象
        /// </summary>
        /// <param name="strIDList">编号列表，各个项目之间用逗号分开</param>
        /// <param name="spliter">分割字符</param>
        public IDList(string strIDList , char spliter )
        {
            this.Parse(strIDList, spliter, '\0', '\0');
        }
        private readonly List<string> _Values = new List<string>();
        /// <summary>
        /// 数值
        /// </summary>
        public List<string> Values
        {
            get
            {
                return _Values; 
            }
        }


        private bool _IgnoreCase = true;
        /// <summary>
        /// 是否忽略大小写
        /// </summary>
        public bool IgnoreCase
        {
            get
            {
                return _IgnoreCase; 
            }
            set
            {
                _IgnoreCase = value; 
            }
        }
        /// <summary>
        /// 编号个数
        /// </summary>
        public int Count
        {
            get
            {
                return _Values.Count;
            }
        }
        /// <summary>
        /// 获得指定序号处的编号值
        /// </summary>
        /// <param name="index">序号</param>
        /// <returns>编号值</returns>
        public string this[int index]
        {
            get
            {
                return _Values[index];
            }
        }

        /// <summary>
        /// 获得编号在列表中的序号
        /// </summary>
        /// <param name="id">编号值</param>
        /// <returns>序号</returns>
        public int IndexOf(string id)
        {
            if ( string.IsNullOrEmpty(id) )// string.IsNullOrEmpty(id))
            { 
                return -1;
            }
            for (int iCount = 0; iCount < this._Values.Count; iCount++)
            {
                if (string.Compare(this._Values[iCount], id, this.IgnoreCase) == 0)
                {
                    return iCount;
                }
            }
            return -1;
        }

        /// <summary>
        /// 判断列表中是否包含指定的编号
        /// </summary>
        /// <param name="id">编号</param>
        /// <returns>是否包含</returns>
        public bool Contains(string id)
        {
            return this.IndexOf(id) >= 0;
        }

        /// <summary>
        /// 添加编号
        /// </summary>
        /// <param name="id">编号</param>
        /// <returns>新编号在列表中的序号</returns>
        public int Add(string id)
        {
            int index = this.IndexOf(id);
            if (index >= 0)
            {
                return index;
            }
            else
            {
                this._Values.Add(id);
                return this._Values.Count - 1;
            }
        }
        /// <summary>
        /// 解析编号列表字符串
        /// </summary>
        /// <param name="Value">要解析的字符串</param>
        /// <param name="splitChar">拆分字符</param>
        /// <param name="prefixChar">编号前缀字符</param>
        /// <param name="endfixChar">编号后缀字符</param>
        public void Parse(string Value , char splitChar , char prefixChar , char endfixChar )
        {
            try
            {
                this._Values.Clear();
                if ( string.IsNullOrEmpty(Value) )// string.IsNullOrEmpty(Value))
                {
                    return;
                }
                string[] items = Value.Split(splitChar);
                for (int iCount = 0; iCount < items.Length; iCount++)
                {
                    string item = items[iCount].Trim();
                    if (item.Length > 0)
                    {
                        if (prefixChar > 0 && item[0] == prefixChar)
                        {
                            item = item.Substring(1);
                        }
                        if (endfixChar > 0 && item.Length > 0 && item[item.Length - 1] == endfixChar)
                        {
                            item = item.Substring(0, item.Length - 1);
                        }
                        if (item.Length > 0)
                        {
                            Add(item);
                        }
                    }
                }
            }
            catch
            {
                this._Values.Clear();
            }
        }
        /// <summary>
        /// 返回表示对象数据的字符串
        /// </summary>
        /// <param name="splitChar">分隔字符</param>
        /// <param name="prefixChar">编号前缀字符</param>
        /// <param name="endfixChar">编号后缀字符</param>
        /// <returns>字符串</returns>
        public string ToString(char splitChar, char prefixChar, char endfixChar)
        {
            StringBuilder str = new StringBuilder();
            for (int iCount = 0; iCount < _Values.Count; iCount++)
            {
                if (splitChar > 0 && str.Length > 0)
                {
                    str.Append(splitChar);
                }
                if (prefixChar > 0)
                {
                    str.Append(prefixChar);
                }
                str.Append(_Values[iCount]);
                if (endfixChar > 0)
                {
                    str.Append(endfixChar);
                }
            }
            return str.ToString();
        }

        /// <summary>
        /// 返回表示对象数据的字符串
        /// </summary>
        /// <returns>字符串</returns>
        public override string ToString()
        {
            return ToString(',', '\0', '\0');
        }


        public System.Collections.IEnumerator GetEnumerator()
        {
            if (this.Values == null)
            {
                return null;
            }
            else
            {
                return this.Values.GetEnumerator();
            }
        }
    }
}
