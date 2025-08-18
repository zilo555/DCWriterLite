using System;
using System.Collections.Generic;
using System.Text;
using DCSoft.Common;

// 袁永福到此一游 2015-4-18

namespace DCSoft.Writer.Dom
{
    /// <summary>
    /// 复选框分组信息对象
    /// </summary>
    public class CheckBoxGroupInfo
    {
        public CheckBoxGroupInfo(DomContainerElement rootElement)
        {
            if (rootElement == null)
            {
                throw new ArgumentNullException("rootElement");
            }
            this._RootElement = rootElement;
            if( (rootElement is DomDocument)== false )
            {
                var info2 = rootElement.OwnerDocument?.CheckBoxGroupInfo;
                if(info2 != null )
                {
                    if( info2._SubInfos == null )
                    {
                        info2._SubInfos = new List<CheckBoxGroupInfo>();
                    }
                    info2._SubInfos.Add(this);
                }
            }
        }
        public void Dispose()
        {
            this._RootElement = null;
            if(this._SubInfos != null )
            {
                var list  = this._SubInfos;
                this._SubInfos = null;
                foreach(var item in list)
                {
                    item.Dispose();
                }
                list.Clear();
            }
            if (this._Checkboxs != null)
            {
                foreach( var item in this._Checkboxs.Values)
                {
                    item.Clear();
                }
                this._Checkboxs.Clear();
                this._Checkboxs = null;
            }
            if (this._Radioboxs != null)
            {
                foreach( var item in this._Radioboxs.Values )
                {
                    item.Clear();
                }
                this._Radioboxs.Clear();
                this._Radioboxs = null;
            }
        }
        private DomContainerElement _RootElement = null;
        /// <summary>
        /// 所属文档对象
        /// </summary>
        public DomContainerElement RootElement
        {
            get { return _RootElement; }
        }

        private List<CheckBoxGroupInfo> _SubInfos = null;

        private int _Version = 1;
        ///// <summary>
        ///// 当前状态版本号
        ///// </summary>
        //public int Version
        //{
        //    get
        //    {
        //        return _Version;
        //    }
        //}

        /// <summary>
        /// 状态无效
        /// </summary>
        public void Invalidate()
        {
            this._Version++;
            this._Checkboxs = null;
            this._Radioboxs = null;
        }
        /// <summary>
        /// 状态无效
        /// </summary>
        public void InvalidateAll()
        {
            this.Invalidate();
            if (this._SubInfos != null)
            {
                foreach (var item in this._SubInfos)
                {
                    item.Invalidate();
                }
            }
        }
        /// <summary>
        /// 获得同一组的复选框元素对象
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public DomElementList GetElementsInSameGroup(DomCheckBoxElementBase element)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }
            if (element.GroupInfoVersion != this._Version)
            {
                this.Invalidate();
            }
            CheckState();
            string name = element.RuntimeGroupName();
            if (name == null)
            {
                name = string.Empty;
            }
            Dictionary<string, DomElementList> dic = null;
            if (element is DomCheckBoxElement)
            {
                dic = this._Checkboxs;
            }
            else if( element is DomRadioBoxElement )
            {
                dic = this._Radioboxs;
            }
            DomElementList list = null;
            if (dic.ContainsKey(name))
            {
                list = dic[name];
            }
            if (list != null)
            {
                list = list.Clone();
            }
            return list;
        }

        /// <summary>
        /// 复选框分组信息
        /// </summary>
        private Dictionary<string, DomElementList> _Checkboxs = null;
        /// <summary>
        /// 单选框分组信息
        /// </summary>
        private Dictionary<string, DomElementList> _Radioboxs = null;
        /// <summary>
        /// 检查状态
        /// </summary>
        private void CheckState()
        {
            if (this._Checkboxs != null)
            {
                return;
            }
            //float tick = DCSoft.Common.CountDown.GetTickCountFloat();
            this._Version++;
            this._Checkboxs = new Dictionary<string, DomElementList>();
            this._Radioboxs = new Dictionary<string, DomElementList>();
            DomElementList elements = this._RootElement.GetElementsByType<DomCheckBoxElementBase>();
            foreach (DomCheckBoxElementBase element in elements)
            {
                element.GroupInfoVersion = this._Version;
                string name = element.RuntimeGroupName();
                if (string.IsNullOrEmpty(name))
                {
                    name = string.Empty;
                }
                Dictionary<string, DomElementList> dis = null;
                if (element is DomCheckBoxElement)
                {
                    // 复选框
                    dis = this._Checkboxs;
                }
                else if (element is DomRadioBoxElement)
                {
                    dis = this._Radioboxs;
                }
                if (dis.ContainsKey(name))
                {
                    dis[name].Add(element);
                }
                else
                {
                    DomElementList list = new DomElementList();
                    dis[name] = list;
                    list.Add(element);
                }
            }//foreach
            //tick = DCSoft.Common.CountDown.GetTickCountFloat() - tick;

        }

    }
}
