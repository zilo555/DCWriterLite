using System;
using System.ComponentModel;
using System.Reflection;
using System.Collections.Generic ;
using System.Text;
using System.Collections;

namespace DCSoft.Common
{
	/// <summary>
	/// 数值,类型转换相关帮助类
	/// </summary>
    /// <remarks>编制 袁永福</remarks>
    public static class ValueTypeHelper
	{
        private class PropertyValueInfo
        {
            public static PropertyValueInfo GetInfo(Type t, string name)
            {
                if (t == null)
                {
                    throw new ArgumentNullException("t");
                }
                if (string.IsNullOrEmpty(name))
                {
                    throw new ArgumentNullException("name");
                }
                name = name.Trim().ToLower();
                Dictionary<string, PropertyValueInfo> result = GetInfos(t);
                PropertyValueInfo info = null;
                if (result.TryGetValue(name, out info))
                {
                    return info;
                }
                else
                {
                    return null;
                }
            }
            private static Dictionary<Type, Dictionary<string, PropertyValueInfo>> _Infos
                = new Dictionary<Type, Dictionary<string, PropertyValueInfo>>();
            private static Dictionary<string, PropertyValueInfo> GetInfos(Type t)
            {
                if (t == null)
                {
                    throw new ArgumentNullException("t");
                }
                Dictionary<string, PropertyValueInfo> result = null;
                //lock (_Infos)
                {
                    if (_Infos.TryGetValue(t, out result) == false)
                    {
                        result = new Dictionary<string, PropertyValueInfo>();
                        _Infos[t] = result;
                        foreach (PropertyInfo p in t.GetProperties(BindingFlags.Public | BindingFlags.Instance))
                        {
                            if (p.IsSpecialName)
                            {
                                continue;
                            }
                            result[p.Name.ToLower()] = new PropertyValueInfo(p);
                        }//foreach
                    }
                }
                return result;
            }

            private static bool _ReadColorDefalutValueError = false;
            public PropertyValueInfo(PropertyInfo p)
            {
                this.Name = p.Name;
                this.LowcaseName = p.Name.ToLower();
                this.RawInfo = p;
                this.CanRead = p.CanRead;
                this.CanWrite = p.CanWrite;
                if (_ReadColorDefalutValueError && p.PropertyType == typeof(Color))
                {
                    // 曾经出现读取颜色默认值错误，则今后不再读取任何默认颜色值
                    this.HasDefaultValue = false;
                }
                else
                {
                    DefaultValueAttribute dva = (DefaultValueAttribute)Attribute.GetCustomAttribute(p, typeof(DefaultValueAttribute), true);
                    if (dva == null)
                    {
                        this.HasDefaultValue = false;
                    }
                    else
                    {
                        if (dva.Value == null && p.PropertyType == typeof(Color))
                        {
                            // 读取默认颜色值错误
                            this.HasDefaultValue = false;
                            _ReadColorDefalutValueError = true;
                        }
                        else
                        {
                            this.HasDefaultValue = true;
                            this.DefaultValue = dva.Value;
                        }
                    }
                }
                this.IsEnumType = p.PropertyType.IsEnum;
                this.PropertyType = p.PropertyType;
            }

            private readonly string Name;
            private readonly string LowcaseName;
            private readonly PropertyInfo RawInfo;
            private readonly bool CanRead;
            private readonly bool CanWrite;
            private readonly Type PropertyType;
            private readonly bool HasDefaultValue;
            private readonly object DefaultValue;
            private readonly bool IsEnumType;
            //private readonly TypeConverter Converter;
            public bool SetValue(object instance, object newValue, bool throwException)
            {
                object pValue = null;
                if (newValue == null || DBNull.Value.Equals(newValue))
                {
                    // 原始值为空
                    if (this.HasDefaultValue)
                    {
                        // 使用默认值
                        pValue = this.DefaultValue;
                    }
                    else
                    {
                        // 使用类型默认值
                        pValue = GetDefaultValue(this.PropertyType);
                    }
                }
                else if (this.IsEnumType && newValue is string)
                {
                    // 枚举类型
                    if (throwException)
                    {
                        pValue = Enum.Parse(this.PropertyType, (string)newValue, true);
                    }
                    else
                    {
                        try
                        {
                            pValue = Enum.Parse(this.PropertyType, (string)newValue, true);
                        }
                        catch( System.Exception ext )
                        {
                            // 转换失败，退出
                            return false;
                        }
                    }
                }
                else if (this.PropertyType.IsInstanceOfType(newValue) == false)
                {
                    // 需要进行类型转换
                    if (throwException)
                    {

                        //if (this.Converter != null)
                        //{
                        //    pValue = this.Converter.ConvertFrom(newValue);
                        //}
                        //else
                        {
                            pValue = Convert.ChangeType(newValue, this.PropertyType);
                        }
                    }
                    else
                    {
                        try
                        {
                            //if (this.Converter != null)
                            //{
                            //    pValue = this.Converter.ConvertFrom(newValue);
                            //}
                            //else
                            {
                                if (this.IsEnumType)
                                {
                                    if (newValue is string)
                                    {
                                        pValue = Enum.Parse(this.PropertyType, (string)newValue);
                                    }
                                    else
                                    {
                                        pValue = Enum.ToObject(this.PropertyType, newValue);
                                    }
                                }
                                else
                                {
                                    pValue = Convert.ChangeType(newValue, this.PropertyType);
                                }

                            }
                        }
                        catch (Exception)
                        {
                            return false;

                        }
                    }
                }
                else
                {
                    pValue = newValue;
                }
                if (throwException)
                {
                    this.RawInfo.SetValue(instance, pValue, null);
                    return true;
                }
                else
                {
                    try
                    {
                        this.RawInfo.SetValue(instance, pValue, null);
                        return true;
                    }
                    catch (Exception)
                    {
                    }
                }
                return false;
            }

        }

        /// <summary>
        /// 设置对象的属性值
        /// </summary>
        /// <param name="instance">对象实例</param>
        /// <param name="propertyName">属性名称</param>
        /// <param name="Value">属性值</param>
        /// <param name="throwException">是否抛出异常</param>
        /// <returns>操作是否成功</returns>
        public static bool SetPropertyValue(
            object instance,
            string propertyName,
            object Value,
            bool throwException)
        {
            if (instance == null)
            {
                throw new ArgumentNullException("instance");
            }
            if (string.IsNullOrEmpty(propertyName))
            {
                throw new ArgumentNullException("propertyName");
            }
            PropertyValueInfo info = PropertyValueInfo.GetInfo(instance.GetType(), propertyName);
            if( info == null )
            {
                if (throwException)
                {
                    throw new ArgumentException(instance.GetType().FullName + "." + propertyName);
                }
                else
                {
                    return false;
                }
            }
            return info.SetValue(instance, Value, throwException);
        }

        private readonly static Dictionary<PropertyInfo, object> _PropertyDefaultValues
            = new Dictionary<PropertyInfo, object>();

        private static readonly System.Collections.Hashtable _TypeDefaultValue = new Hashtable();
        /// <summary>
        /// 获得指定类型的默认值
        /// </summary>
        /// <param name="ValueType">数据类型</param>
        /// <returns>默认值</returns>
        public static object GetDefaultValue(Type ValueType)
		{
            if( ValueType == null )
            {
                throw new ArgumentNullException("ValueType");
            }
            //lock (_TypeDefaultValue)
            {
                if (_TypeDefaultValue.ContainsKey(ValueType))
                {
                    return _TypeDefaultValue[ValueType];
                }
                if (_TypeDefaultValue.Count == 0 )
                {
                    _TypeDefaultValue[typeof(object)] = null;
                    _TypeDefaultValue[typeof(byte)] = (byte)0;
                    _TypeDefaultValue[typeof(sbyte)] = (sbyte)0;
                    _TypeDefaultValue[typeof(short)] = (short)0;
                    _TypeDefaultValue[typeof(ushort)] = (ushort)0;
                    _TypeDefaultValue[typeof(int)] = (int)0;
                    _TypeDefaultValue[typeof(uint)] = (uint)0;
                    _TypeDefaultValue[typeof(long)] = (long)0;
                    _TypeDefaultValue[typeof(ulong)] = (ulong)0;
                    _TypeDefaultValue[typeof(char)] = (char)0;
                    _TypeDefaultValue[typeof(float)] = (float)0;
                    _TypeDefaultValue[typeof(double)] = (double)0;
                    _TypeDefaultValue[typeof(decimal)] = (decimal)0;
                    _TypeDefaultValue[typeof(bool)] = false;
                    _TypeDefaultValue[typeof(string)] = null;
                    _TypeDefaultValue[typeof(DateTime)] = DateTime.MinValue;
                    _TypeDefaultValue[typeof(Point)] = Point.Empty;
                    _TypeDefaultValue[typeof(PointF)] = PointF.Empty;
                    _TypeDefaultValue[typeof(Size)] = Size.Empty;
                    _TypeDefaultValue[typeof(SizeF)] = SizeF.Empty;
                    _TypeDefaultValue[typeof(Rectangle)] = Rectangle.Empty;
                    _TypeDefaultValue[typeof(RectangleF)] = RectangleF.Empty;
                    _TypeDefaultValue[typeof(Color)] = Color.Transparent;
                    _TypeDefaultValue[typeof(System.IntPtr)] = IntPtr.Zero;
                    _TypeDefaultValue[typeof(System.UIntPtr)] = UIntPtr.Zero;
                }

                if (ValueType.IsEnum)
                {
                    // 处理枚举类型
                    Array vs = Enum.GetValues(ValueType);
                    if (vs != null && vs.Length > 0)
                    {
                        object v = vs.GetValue(0);
                        _TypeDefaultValue[ValueType] = v;
                        return v;
                    }
                    else
                    {
                        object v = System.Activator.CreateInstance(ValueType);
                        _TypeDefaultValue[ValueType] = v;
                        return v;
                    }
                }
                
                if (ValueType.IsValueType)
                {
                    object v = System.Activator.CreateInstance(ValueType);
                    _TypeDefaultValue[ValueType] = v;
                    return v;
                }
            }
			return null;
		}
         
	}
}