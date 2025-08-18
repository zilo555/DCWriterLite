using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel ;

namespace DCSoft.Common
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>编制 袁永福</remarks>
    public sealed class XDependencyProperty
    {
        /// <summary>
        /// 表示没有默认值的数值
        /// </summary>
        public readonly static object NullDefaultValue = new object();

        private readonly static Dictionary<Type, Dictionary<string, XDependencyProperty>> _PropertiyTable 
            = new Dictionary<Type, Dictionary<string, XDependencyProperty>>();
        /// <summary>
        /// 注册属性
        /// </summary>
        /// <param name="name">属性名</param>
        /// <param name="propertyType">属性数据类型</param>
        /// <param name="ownerType">属性所属对象类型</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>属性注册对象</returns>
        public static XDependencyProperty Register(
            int id,
            string name,
            Type propertyType,
            Type ownerType,
            object defaultValue)
        {
            if (name == null || name.Trim().Length == 0)
            {
                throw new ArgumentNullException("name");
            }
            name = name.Trim();
            if (propertyType == null)
            {
                throw new ArgumentNullException("propertyType");
            }
            if (ownerType == null)
            {
                throw new ArgumentNullException("ownerType");
            }
            if (defaultValue != null)
            {
                if (propertyType.IsInstanceOfType(defaultValue) == false)
                {
                    throw new ArgumentException("bad defaultValue:" + defaultValue);
                }
            }
            Dictionary<string, XDependencyProperty> table = null;
            if (_PropertiyTable.TryGetValue(ownerType, out table) == false)
            {
                table = new Dictionary<string, XDependencyProperty>();
                _PropertiyTable[ownerType] = table;
            }
            if (table.ContainsKey(name))
            {
                throw new ArgumentException("Multi " + name);
            }
            XDependencyProperty property = new XDependencyProperty(
                id,
                ownerType,
                name,
                propertyType,
                defaultValue);
            //property.DefaultValue = defaultValue;
            table[name] = property;
            return property;
        }

        /// <summary>
        /// 获得属性对象
        /// </summary>
        /// <param name="ownerType">对象类型</param>
        /// <param name="name">名称</param>
        /// <returns>获得的属性</returns>
        public static XDependencyProperty GetProperty(Type ownerType, string name)
        {
            if (ownerType == null)
            {
                throw new ArgumentNullException("ownerType");
            }
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }
            foreach (Type type in _PropertiyTable.Keys)
            {
                if (type == ownerType || ownerType.IsSubclassOf(type))
                {
                    Dictionary<string, XDependencyProperty> table = _PropertiyTable[type];
                    if( table.TryGetValue( name , out var p2 ))
                    {
                        return p2;
                    }
                    //if (table.ContainsKey(name))
                    //{
                    //    return table[name];
                    //}
                }
            }
            return null;
        }
        /// <summary>
        /// 获得属性信息
        /// </summary>
        /// <param name="ownerType">对象类型</param>
        /// <param name="declearTypeOnly">仅获得本对象类型声明的属性</param>
        /// <returns>属性信息数组</returns>
        public static XDependencyProperty[] GetProperties(Type ownerType, bool declearTypeOnly)
        {
            if (ownerType == null)
            {
                throw new ArgumentNullException("ownerType");
            }

            List<XDependencyProperty> result = new List<XDependencyProperty>();
            if (declearTypeOnly)
            {
                if (_PropertiyTable.TryGetValue(ownerType , out var table ))
                {
                    //Dictionary<string, XDependencyProperty> table = _PropertiyTable[ownerType];
                    foreach (XDependencyProperty p in table.Values)
                    {
                        result.Add(p);
                    }
                }
            }
            else
            {
                foreach (Type type in _PropertiyTable.Keys)
                {
                    if (type == ownerType || ownerType.IsSubclassOf(type))
                    {
                        Dictionary<string, XDependencyProperty> table = _PropertiyTable[type];
                        foreach (XDependencyProperty p in table.Values)
                        {
                            result.Add(p);
                        }
                    }
                }
            }
            return result.ToArray();
        }

        private XDependencyProperty(
            int id,
            Type ownerType,
            string name, 
            Type propertyType , 
            object defaultValue )
        {
            this._ID = id;
            this._OwnerType = ownerType;
            this._Name = name;
            this._PropertyType = propertyType;
            //this._DefaultValue = ValueTypeHelper.GetDefaultValue(propertyType);
            this._DefaultValue = defaultValue;
        }

        private readonly int _ID;
        public int ID
        {
            get
            {
                return this._ID;
            }
        }

        internal readonly Type _OwnerType;
        /// <summary>
        /// 对象所属的类型
        /// </summary>
        public Type OwnerType
        {
            get
            {
                return _OwnerType; 
            }
            //set
            //{
            //    _OwnerType = value; 
            //}
        }

        private readonly string _Name;
        /// <summary>
        /// 对象名称
        /// </summary>
        [DefaultValue(null)]
        public string Name
        {
            get
            {
                return _Name; 
            }
            //set
            //{
            //    _Name = value; 
            //}
        }

        private readonly Type _PropertyType ;
        /// <summary>
        /// 数据类型
        /// </summary>
        public Type PropertyType
        {
            get 
            {
                return _PropertyType; 
            }
            //set
            //{
            //    _PropertyType = value; 
            //}
        }

        private readonly object _DefaultValue;
        /// <summary>
        /// 属性默认值
        /// </summary>
        public object DefaultValue
        {
            get 
            {
                return _DefaultValue; 
            }
            //set
            //{
            //    _DefaultValue = value; 
            //}
        }
        /// <summary>
        /// 判断数据是否等于默认值
        /// </summary>
        /// <param name="Value">数值</param>
        /// <returns>是否等于默认值</returns>
        public bool EqualsDefaultValue(object Value)
        {
            if (_DefaultValue == NullDefaultValue)
            {
                return false;
            }
            if (_DefaultValue is IComparable)
            {
                return ((IComparable)_DefaultValue).CompareTo(Value) == 0;
            }
            return object.Equals(_DefaultValue, Value);
        }

    }//public class XDependencyProperty


}
