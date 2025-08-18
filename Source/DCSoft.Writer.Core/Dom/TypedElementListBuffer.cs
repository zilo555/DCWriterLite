using System;
using System.Collections.Generic;
using System.Text;

namespace DCSoft.Writer.Dom
{
    /// <summary>
    /// DCWriter内部使用。指定类型的元素列表的缓存区.
    /// </summary>
    public class TypedElementListBuffer
    {
        public TypedElementListBuffer()
        {
        }

        private bool _Lock = false;
        //锁定内容，禁止清空。
        internal bool Lock
        {
            //get { return _Lock; }
            set { _Lock = value; }
        }

        /// <summary>
        /// 同步内容版本号
        /// </summary>
        internal void SyncContentVersion()
        {
            if (this._Buffer != null)
            {
                foreach (DomContainerElement key in this._Buffer.Keys)
                {
                    BufferItem item = this._Buffer[key];
                    item._ContentVersion = key.ContentVersion;
                }
            }
        }

        /// <summary>
        /// 清空缓存区。
        /// </summary>
        internal void Clear()
        {
            if (this._Lock == false)
            {
                foreach (BufferItem item in _Buffer.Values)
                {
                    foreach (var list in item.Values)
                    {
                        list.Clear();
                    }
                    item.Clear();
                }
                _Buffer.Clear();
            }
        }

        internal void InnerDispose()
        {
            if (_Buffer != null)
            {
                this._Lock = false;
                this.Clear();
                _Buffer = null;
            }
        }

        private class BufferItem : Dictionary<Type, DomElementList>
        {
            public int _ContentVersion = 0;
        }

        private Dictionary<DomContainerElement, BufferItem> _Buffer = new Dictionary<DomContainerElement, BufferItem>();


        public DomElementList GetElementsByType(DomContainerElement rootElement, Type elementType)
        {
            if (rootElement == null)
            {
                throw new ArgumentNullException("rootElement");
            }
            if (elementType == null)
            {
                throw new ArgumentNullException("elementType");
            }
            if (elementType == typeof(DomCharElement))
            {
                // 不能接受字符类型的参数类型
                throw new ArgumentOutOfRangeException(elementType.FullName);
            }
            BufferItem item = null;
            if (_Buffer.TryGetValue(rootElement, out item) == false)
            {
                item = new BufferItem();
                item._ContentVersion = rootElement.ContentVersion;
                _Buffer[rootElement] = item;
            }
            if (item._ContentVersion != rootElement.ContentVersion)
            {
                item._ContentVersion = rootElement.ContentVersion;
                item.Clear();
            }
            if (item.ContainsKey(elementType))
            {
                return item[elementType];
            }
            else
            {
                DomElementList list = null;
                if (elementType == typeof(DomTableElement))
                {
                    list = new DomElementList();
                    GetTables(rootElement, list);
                }
                else if (elementType == typeof(DomContentElement))
                {
                    list = new DomElementList();
                    GetContentElements(rootElement, list);
                }
                else
                {
                    list = rootElement.GetElementsByType(elementType);
                }
                if (list == null)
                {
                    return null;
                }
                //if (XTextDocument._WebApplicationMode == false)
                {
                    list.Capacity = list.Count;
                }
                item[elementType] = list;
                return list;
            }
        }

        private static void GetContentElements(DomContainerElement rootElement, DomElementList list)
        {
            if (rootElement.HasChildElement<DomContainerElement>())// (XTextContainerElement.DCChildElementType.Container))
            {
                var es = rootElement.Elements;
                var arr = es.InnerGetArrayRaw();
                var len = es.Count;
                for (var iCount = 0; iCount < len; iCount++)
                {
                    if (arr[iCount] is DomContainerElement)
                    {
                        var item = (DomContainerElement)arr[iCount];
                        if (item is DomTableElement)
                        {
                            var table = (DomTableElement)item;
                            foreach (DomTableRowElement row in table.Rows.FastForEach())
                            {
                                list.AddRangeByDCList(row.Cells);
                                foreach (DomTableCellElement cell in row.Cells.FastForEach())
                                {
                                    GetContentElements(cell, list);
                                }
                            }
                        }
                        else
                        {
                            GetContentElements(item, list);
                        }
                    }
                }
            }
        }
        private static void GetTables(DomContainerElement rootElement, DomElementList list)
        {
            if (rootElement.HasChildElement<DomContainerElement>())// (XTextContainerElement.DCChildElementType.Container))
            {
                var es = rootElement.GetCompressedElements();
                if (es == null)
                {
                    return;
                }
                var len = es.Count;
                for (var iCount = 0; iCount < len; iCount++)
                {
                    if (es[iCount] is DomContainerElement item)
                    {
                        if (item is DomTableElement)
                        {
                            var table = (DomTableElement)item;
                            list.AddRaw(table);

                            var rowsArr = table.Rows.InnerGetArrayRaw();
                            var rowsCount= table.Rows.Count;
                            for (var rIndex = 0; rIndex < rowsCount; rIndex++)
                            {
                                var row = rowsArr[rIndex];
                                var cellsArr = row.Elements.InnerGetArrayRaw();
                                var cellsCount = row.Elements.Count;
                                for (var cIndex = 0; cIndex < cellsCount; cIndex++)
                                {
                                    var cell = (DomTableCellElement)cellsArr[cIndex];
                                    if (cell.OnlyHasSingleParagraphFlagElement() == false)
                                    {
                                        GetTables(cell, list);
                                    }
                                }
                            }
                            //foreach (XTextTableRowElement row in table.Rows.FastForEach())
                            //{
                            //    foreach (XTextTableCellElement cell in row.Cells.FastForEach())
                            //    {
                            //        GetTables(cell, list);
                            //    }
                            //}
                        }
                        else
                        {
                            GetTables(item, list);
                        }
                    }
                }//for
            }//if
        }
    }
}
