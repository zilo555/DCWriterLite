using System.Collections.Generic;
using System.Text;
using DCSoft.Writer.Dom;
using System.Collections;
using DCSoft.Common;
using System.IO;
using System.Text.Json.Nodes;
using DCSoft.WASM;
using System.ComponentModel;

namespace DCSoft.Writer.Data
{
    /// <summary>
    /// 默认的数据源功能提供者
    /// </summary>
    internal static partial class DefaultDOMDataProvider
    {
        /// <summary>
        /// 获得表单数据
        /// </summary>
        /// <param name="document"></param>
        /// <param name="name">字段名称</param>
        /// <returns>获得的表单数值</returns>
        public static string GetFormValue(DomDocument document, string name)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }
            name = name.Trim();
            if (name.Length == 0)
            {
                throw new ArgumentNullException("name");
            }
            string result = null;
            DomTreeNodeEnumerable enumer = new DomTreeNodeEnumerable(document);
            foreach (DomElement element in enumer)
            {
                if (element is DomInputFieldElementBase)
                {
                    // 文本输入域
                    DomInputFieldElementBase field = (DomInputFieldElementBase)element;
                    if (field.ID == name || field.Name == name)
                    {
                        result = field.Text;
                        break;
                    }
                }
                else if (element is DomCheckBoxElement)
                {
                    // 复选框
                    DomCheckBoxElement chk = (DomCheckBoxElement)element;
                    if (chk.Name == name)
                    {
                        DomElementList chks = chk.GetElementsInSameGroup();
                        StringBuilder str = new StringBuilder();
                        foreach (DomCheckBoxElementBase chk2 in chks)
                        {
                            if (chk2.Checked)
                            {
                                if (str.Length > 0)
                                {
                                    str.Append(',');
                                }
                                str.Append(chk2.CheckedValue);
                                if (chk is DomRadioBoxElement)
                                {
                                    break;
                                }
                            }
                        }//foreach
                        result = str.ToString();
                        break;
                    }
                }
                //wyc20221221添加判断
                else if (element is DomRadioBoxElement)
                {
                    DomRadioBoxElement rdo = element as DomRadioBoxElement;
                    if(rdo.Name == name)
                    {
                        DomElementList rdos = rdo.GetElementsInSameGroup();
                        foreach(DomRadioBoxElement rdo2 in rdos)
                        {
                            if(rdo2.Checked)
                            {
                                result = rdo2.CheckedValue;
                                break;
                            }
                        }
                    }
                }
            }//foreach
            return result;
        }

        /// <summary>
        /// 设置表单值
        /// </summary>
        /// <param name="document"></param>
        /// <param name="name">表单元素名称</param>
        /// <param name="Value">表单值</param>
        /// <returns>操作是否修改了文档内容</returns>
        public static bool SetFormValue(DomDocument document, string name, string Value)
        {
            bool result = false;
            DomElementList elements = document.GetElementsByName(name);
            if (elements != null && elements.Count > 0)
            {
                foreach (DomElement element in elements)
                {
                    if (element is DomFieldElementBase)
                    {
                        DomFieldElementBase field = (DomFieldElementBase)element;
                        if (field.EditorTextExt != Value)
                        {
                            field.EditorTextExt = Value;
                            result = true;
                        }
                    }
                    else if (element is DomCheckBoxElement)
                    {
                        IDList list = new IDList(Value, ',');
                        DomCheckBoxElement chk = (DomCheckBoxElement)element;
                        bool cv = list.Contains(chk.CheckedValue);
                        if (chk.EditorChecked != cv)
                        {
                            chk.EditorChecked = cv;
                            result = true;
                        }
                    }
                    else if (element is DomRadioBoxElement)
                    {
                        DomRadioBoxElement rdo = (DomRadioBoxElement)element;
                        bool cv = rdo.CheckedValue == Value;
                        if (rdo.EditorChecked != cv)
                        {
                            rdo.EditorChecked = cv;
                            result = true;
                        }
                    }
                }//foreach
            }
            if(result )
            {
                document.InvalidateFixDomState();
            }
            return result;
        }

        /// <summary>
        /// 获得文档中的表单数据字典
        /// </summary>
        public static Hashtable GetFormValues(DomDocument document)
        {
            Dictionary<string, List<string>> values = new Dictionary<string, List<string>>();
            foreach (DomInputFieldElementBase field in document.GetElementsByType<DomInputFieldElementBase>())
            {
                string name = field.Name;
                if (string.IsNullOrEmpty(name) == false)
                {
                    if (values.ContainsKey(name) == false)
                    {
                        values[name] = new List<string>();
                    }
                    values[name].Add(field.Text);
                }
            }
            foreach (DomCheckBoxElementBase chk in document.GetElementsByType<DomCheckBoxElementBase>())
            {
                if (chk.Checked)
                {
                    string name = chk.Name;
                    if (string.IsNullOrEmpty(name) == false)
                    {
                        if (values.ContainsKey(name) == false)
                        {
                            values[name] = new List<string>();
                        }
                        values[name].Add(chk.CheckedValue);
                    }
                }
            }
            Hashtable result = new Hashtable();
            foreach (string name in values.Keys)
            {
                List<string> items = values[name];
                StringBuilder str = new StringBuilder();
                for (int iCount = 0; iCount < items.Count; iCount++)
                {
                    if (str.Length > 0)
                    {
                        str.Append(',');
                    }
                    str.Append(items[iCount]);
                }//for
                result[name] = str.ToString();
            }//foreach
            return result;
        }

        /// <summary>
        /// 重置表单元素默认值
        /// </summary>
        /// <returns>是否导致文档内容发生改变</returns>
        public static bool ResetFormValue(DomDocument document)
        {
            bool changed = false;
            foreach (DomInputFieldElement field in document.GetElementsByType<DomInputFieldElement>())
            {
                if (field.ResetToDefaultValue())
                {
                    field.Modified = false;
                    changed = true;
                }
            }
            foreach (DomCheckBoxElementBase chk in document.GetElementsByType<DomCheckBoxElementBase>())
            {
                if (chk.Checked)
                {
                    chk.Checked = false;
                    chk.Modified = false;
                    changed = true;
                }
                var v = chk.StateVersion;
                if (v != chk.StateVersion)
                {
                    // 状态发送改变。
                    chk.Modified = true;
                    changed = true;
                }
            }
            return changed;
        }
    }
}