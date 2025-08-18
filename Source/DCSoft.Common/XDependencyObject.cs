using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Collections;

namespace DCSoft.Common
{
    /// <summary>
    /// 具有附加属性系统的对象类型
    /// </summary>
    /// <remarks>编制 袁永福</remarks>
    public abstract class XDependencyObject
    {

        #region 静态成员 ****************************************************

        public static int GetValueCount(XDependencyObject instance)
        {
            if (instance == null)
            {
                return 0;
            }
            else
            {
                return instance._InnerValues.Count;
            }
        }

        /// <summary>
        /// 删除默认的属性值
        /// </summary>
        /// <param name="instance">要处理的对象</param>
        /// <param name="defaultValues">默认值</param>
        public static int RemoveDefaultValues(XDependencyObject instance, XDependencyObject defaultValues)
        {
            if (instance == null)
            {
                return 0;
            }
            if (instance == defaultValues)
            {
                return 0;
            }
            if (instance.DisableDefaultValue)
            {
                return 0;
            }
            int result = 0;
            instance._ValueHashCode = 0;
            if (defaultValues == null)
            {
                var ps = new List<XDependencyProperty>(instance._InnerValues.Keys);
                foreach (XDependencyProperty p in ps)
                {
                    if (object.Equals(p.DefaultValue, instance._InnerValues[p]))
                    {
                        instance._InnerValues.Remove(p);
                        instance.OnValueChanged(p);
                        instance.ValueModified = true;
                        result++;
                    }
                }
            }
            else
            {
                var ps = new List<XDependencyProperty>(instance._InnerValues.Keys);
                foreach (XDependencyProperty p in ps)
                {
                    foreach (XDependencyProperty p2 in defaultValues._InnerValues.Keys)
                    {
                        if (p == p2)
                        {
                            if (object.Equals(instance._InnerValues[p], defaultValues._InnerValues[p]))
                            {
                                //if (p.Name == "FontSize")
                                //{
                                //}
                                instance._InnerValues.Remove(p);
                                instance.ValueModified = true;
                                instance.OnValueChanged(p);
                                result++;
                            }
                        }
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 获得style样式的文本
        /// </summary>
        /// <param name="instance">对象实例</param>
        /// <returns>获得的文本</returns>
        public static string GetStyleString(XDependencyObject instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException("instance");
            }
            StringBuilder str = new StringBuilder();
            foreach (XDependencyProperty p in instance._InnerValues.Keys)
            {
                if (str.Length > 0)
                {
                    str.Append(";");
                }
                str.Append(p.Name + ":" + instance._InnerValues[p]);
            }
            return str.ToString();
        }
        /// <summary>
        /// 判断对象是否存在指定名称的属性值，名称不区分大小写
        /// </summary>
        /// <param name="instance">对象实例</param>
        /// <param name="propertyName">属性名</param>
        /// <returns>是否存在指定名称的属性值</returns>
        public static bool HasPropertyValue(XDependencyObject instance, string propertyName)
        {
            if (instance == null)
            {
                throw new ArgumentNullException("instance");
            }
            if (propertyName == null || propertyName.Trim().Length == 0)
            {
                throw new ArgumentNullException("propertyName");
            }
            if (instance._InnerValues != null && instance._InnerValues.Count > 0)
            {
                propertyName = propertyName.Trim();
                foreach (XDependencyProperty p in instance._InnerValues.Keys)
                {
                    if (p == null)
                    {
                        //continue;
                    }
                    if (string.Compare(p.Name, propertyName, StringComparison.OrdinalIgnoreCase) == 0)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// 快速复制对象数据，不进行默认值的判断
        /// </summary>
        /// <param name="source">数据来源</param>
        /// <param name="destination">复制目标</param>
        public static void CopyValueFast(XDependencyObject source, XDependencyObject destination)
        {
            if (source == destination)
            {
                return;
            }
            if (source == null || destination == null)
            {
                return;
            }
            destination.InnerValues.Clear();
            destination._ValueHashCode = 0;
            foreach (var item in source._InnerValues)
            {
                object v = item.Value;
                if (v is ICloneable)
                {
                    v = ((ICloneable)v).Clone();
                }
                destination._InnerValues.Add(item.Key, v);
            }
            destination.ValueModified = true;
        }

        /// <summary>
        /// 快速复制对象数据，不进行默认值的判断
        /// </summary>
        /// <param name="source">数据来源</param>
        /// <param name="destination">复制目标</param>
        public static void CopyValueFast(XDependencyObject source, XDependencyObject destination, bool overWrite)
        {
            if (source == destination)
            {
                return;
            }
            if (source == null || destination == null)
            {
                return;
            }
            //destination.InnerValues.Clear();
            destination._ValueHashCode = 0;
            foreach (XDependencyProperty p in source._InnerValues.Keys)
            {
                if (p == null)
                {
                    throw new NullReferenceException("p");
                }

                if (overWrite == false)
                {
                    if (destination._InnerValues.ContainsKey(p))
                    {
                        continue;
                    }
                }
                object v = source._InnerValues[p];
                if (v is ICloneable)
                {
                    v = ((ICloneable)v).Clone();
                }
                destination._InnerValues[p] = v;
                destination.ValueModified = true;
            }
        }

        /// <summary>
        /// 合并数据
        /// </summary>
        /// <param name="source">数据源</param>
        /// <param name="destination">数据目标对象</param>
        /// <param name="overWrite">源数据是否覆盖目标数据</param>
        /// <returns>修改了目标对象的属性个数</returns>
        public static int MergeValues(
            XDependencyObject source,
            XDependencyObject destination,
            bool overWrite)
        {
            if (source == destination)
            {
                return 0;
            }
            if (source == null || destination == null)
            {
                return 0;
            }
            int result = 0;
            destination._ValueHashCode = 0;
            foreach (XDependencyProperty p in source.InnerValues.Keys)
            {
                if (destination.InnerValues.ContainsKey(p) == false)
                {
                    object v = source.GetValue(p);
                    if (v is ICloneable)
                    {
                        v = ((ICloneable)v).Clone();
                    }
                    if (source._DisableDefaultValue || destination._DisableDefaultValue)
                    {
                        destination.InnerValues[p] = v;
                        destination.ValueModified = true;
                        destination.OnValueChanged(p);
                    }
                    else
                    {
                        destination.SetValue(p, v);
                    }
                    result++;
                }
                else
                {
                    if (overWrite)
                    {
                        bool back = destination._DisableDefaultValue;
                        destination._DisableDefaultValue = source._DisableDefaultValue;
                        destination.InnerValues[p] = source.InnerValues[p];
                        destination.ValueModified = true;
                        destination._DisableDefaultValue = back;
                        destination.OnValueChanged(p);
                        result++;
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 删除指定名称的属性，属性名不区分大小写
        /// </summary>
        /// <param name="instance">对象实例</param>
        /// <param name="propertyName">属性名</param>
        /// <returns>操作是否修改了数据</returns>
        public static bool RemoveProperty(XDependencyObject instance, string propertyName)
        {
            if (instance == null)
            {
                throw new ArgumentNullException("instance");
            }
            if (string.IsNullOrEmpty(propertyName))
            {
                throw new ArgumentNullException("propertyName");
            }
            //instance._InnerRuntimeStyle = null;
            instance.CheckValueLocked();
            foreach (XDependencyProperty p in instance._InnerValues.Keys)
            {
                if (string.Compare(p.Name, propertyName, StringComparison.Ordinal) == 0)
                {
                    instance._ValueHashCode = 0;
                    instance._InnerValues.Remove(p);
                    instance.ValueModified = true;
                    instance.OnValueChanged(p);
                    return true;
                }
            }
            return false;
        }

        protected int _ValueHashCode = 0;
        public virtual int GetValueHashCode()
        {
            if (this._ValueHashCode == 0)
            {
                int v = 0;
                foreach (var item in this._InnerValues)
                {
                    v += item.Key.GetHashCode();
                    if (item.Value != null)
                    {
                        v += item.Value.GetHashCode();
                    }
                }
                this._ValueHashCode = v;
            }
            return this._ValueHashCode;
        }
        #endregion

        /// <summary>
        /// 初始化对象
        /// </summary>
        protected XDependencyObject()
        {
        }

        /// <summary>
        /// 内置的数据字典
        /// </summary>
        protected XDependencyPropertyObjectValues _InnerValues
            = new XDependencyPropertyObjectValues();

        /// <summary>
        /// 内部的数据列表
        /// </summary>
        protected Dictionary<XDependencyProperty, object> InnerValues
        {
            get
            {
                return this._InnerValues;
            }
        }


        [NonSerialized]
        private bool _ValueLocked = false;
        /// <summary>
        /// 是否锁定数据
        /// </summary>
        public bool ValueLocked
        {
            get
            {
                return _ValueLocked;
            }
            set
            {
                _ValueLocked = value;
            }
        }

        /// <summary>
        /// 带Dispose的清空对象数值
        /// </summary>
        protected void ClearWithDispose()
        {
            //this._InnerRuntimeStyle = null;
            if (this._InnerValues != null)
            {
                foreach (object obj in this._InnerValues.Values)
                {
                    if (obj is IDisposable)
                    {
                        ((IDisposable)obj).Dispose();
                    }
                }
                this._InnerValues.Clear();
                OnValueChanged(null);
            }
        }


        /// <summary>
        /// 获得对象数据
        /// </summary>
        /// <param name="property">属性对象</param>
        /// <returns>获得的数据</returns>
        protected virtual object GetValue(XDependencyProperty property)
        {
            if (property == null)
            {
                throw new ArgumentNullException("property");
            }
            object v = null;
            if (_InnerValues.TryGetValue(property, out v))
            {
                return v;
            }
            return property.DefaultValue;
        }


        private bool _DisableDefaultValue = false;
        /// <summary>
        /// 禁止默认值规则
        /// </summary>
        public bool DisableDefaultValue
        {
            get
            {
                return _DisableDefaultValue;
            }
            set
            {
                _DisableDefaultValue = value;
            }
        }

        private void CheckValueLocked()
        {
            if (_ValueLocked)
            {
                throw new InvalidOperationException("属性值被锁定了");
            }
        }

        protected virtual bool FastSetValueMode()
        {
            return false;
        }

        /// <summary>
        /// 设置对象数据
        /// </summary>
        /// <param name="property">属性</param>
        /// <param name="Value">属性值</param>
        public virtual void SetValue(XDependencyProperty property, object Value)
        {
            if (FastSetValueMode())
            {
                if (this._InnerValues.ContainsKey(property) && property.EqualsDefaultValue(Value))
                {
                    this._InnerValues.Remove(property);
                }
                else
                {
                    this._InnerValues[property] = Value;
                }
                return;
            }
            //_InnerRuntimeStyle = null;
            CheckValueLocked();
            if (property == null)
            {
                throw new ArgumentNullException("property");
            }
            if (property.OwnerType.IsInstanceOfType(this) == false)
            {
                throw new ArgumentException("need " + property.OwnerType.FullName);
            }
            this._ValueHashCode = 0;
            if (this._DisableDefaultValue == false
                && property.EqualsDefaultValue(Value))
            {
                if (_InnerValues.ContainsKey(property))
                {
                    _InnerValues.Remove(property);
                }
                else
                {
                    return;
                }
            }
            else
            {
                _InnerValues[property] = Value;
            }
            OnValueChanged(property);
            this.ValueModified = true;
        }

        protected virtual void OnValueChanged(XDependencyProperty property)
        {
        }

        /// <summary>
        /// 对象数据是否改变标记
        /// </summary>

        public bool ValueModified = false;

    }


    /// <summary>
    /// 数值字典
    /// </summary>
    public class XDependencyPropertyObjectValues : Dictionary<XDependencyProperty, object>
    {
        /// <summary>
        /// 初始化对象
        /// </summary>
        public XDependencyPropertyObjectValues()
        {
        }
    }
}