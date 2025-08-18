using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Reflection;

namespace DCSoft.Common
{
    /// <summary>
    /// 枚举类型帮助器
    /// </summary>
    /// <typeparam name="ET"></typeparam>
    public static class DCEnumHelper<ET> where ET : System.Enum
    {
        private const int MaxFastArraySize = 257;

        private static readonly Type _EnumType ;
        private static readonly bool _IsFlag = false ;
        private static readonly Dictionary<string, ET> _Names = null ;
        private static readonly ET _DefaultValue = default(ET) ;
        private static readonly string _DefaultValueName = null;
        private static readonly EnumValueItem _AllItem = null ;
        private static readonly EnumValueItem[] _Values = null ;
        private static readonly EnumValueItem[] _FastValues = null;
        private static readonly bool _IsEmpty = false;
        static DCEnumHelper()
        {
            _EnumType = typeof(ET);
            _IsFlag = Attribute.GetCustomAttribute(_EnumType, typeof(FlagsAttribute), false) != null;
            _Names = new Dictionary<string, ET>();
            var items = new List<EnumValueItem>();
            long maxValue = 0;
            foreach (var f in _EnumType.GetFields(BindingFlags.Public | BindingFlags.Static))
            {
                var fName = f.Name;
                var item = new EnumValueItem(Convert.ToInt64(f.GetValue(null)),(ET) f.GetValue(null), fName);
                if (string.Compare(f.Name, "all", true) == 0)
                {
                    _AllItem = item;
                }
                items.Add(item);
                _Names[fName] = (ET)f.GetValue(null);
                if (_IsFlag)
                {
                    maxValue = maxValue | item.IntValue;
                }
                else
                {
                    if (item.IntValue > maxValue)
                    {
                        maxValue = item.IntValue;
                    }
                }
            }//foreach
            _IsEmpty = items.Count == 0;
            if (items.Count > 0)
            {
                _DefaultValue = items[0].Value;
                _DefaultValueName = items[0].Name;
            }
            _Values = items.ToArray();
            if (items.Count > 0)
            {
                // 可设置快速访问用的数组
                int len = Math.Min((int)maxValue + 1, MaxFastArraySize);
                if (len < 0)
                {
                    // 可能超出INT32范围了。
                    len = MaxFastArraySize;
                }
                _FastValues = new EnumValueItem[len];
                foreach (var item in items)
                {
                    if (item.IntValue >= 0 && item.IntValue < len)
                    {
                        _FastValues[item.IntValue] = item;
                    }
                }
            }
        }
        /// <summary>
        /// 获得枚举值名称
        /// </summary>
        /// <param name="v">数值</param>
        /// <returns>名称</returns>
        public static string ValueToString(ET v)
        {
            if( _IsEmpty )
            {
                return string.Empty;
            }
            if( v.CompareTo( _DefaultValue) == 0)
            {
                return _DefaultValueName;
            }
            if (_AllItem != null && v.CompareTo( _AllItem.Value ) == 0)
            {
                return _AllItem.Name;
            }
            if (_FastValues != null)
            {
                var iv = ((IConvertible)v).ToInt64(null);// Convert.ToInt64(v);
                if (iv >= 0 && iv < _FastValues.Length)
                {
                    if (_FastValues[iv] != null)
                    {
                        return _FastValues[iv].Name;
                    }
                    else
                    {
                        string name = v.ToString();
                        _FastValues[iv] = new EnumValueItem(iv, v , name);
                        _Names[name] = v;
                        return name;
                    }
                }
            }
            return v.ToString();
        }

        public static ET StringToValue(string name)
        {
            if( _IsEmpty )
            {
                return default(ET);
            }
            if (string.IsNullOrEmpty(name) || _DefaultValueName == name)
            {
                return _DefaultValue;
            }
            ET result = default(ET);
            if (_Names.TryGetValue(name, out result))
            {
                return result;
            }
            result = (ET)Enum.Parse(_EnumType, name , true );
            _Names[name] = result;
            return result;
        }


        /// <summary>
        /// 枚举类型项目信息
        /// </summary>
        private class EnumValueItem
        {
            /// <summary>
            /// 初始化对象
            /// </summary>
            /// <param name="intValue">整数数值</param>
            /// <param name="V">数值</param>
            /// <param name="name">名称</param>
            /// <param name="desc">说明</param>
            public EnumValueItem(long intValue, ET V, string name)
            {
                this.IntValue = intValue;
                this.Value = V;
                this.Name = name;
                //this._Description = desc;
            }

            /// <summary>
            /// 整数数值
            /// </summary>
            public readonly long IntValue = 0;

            /// <summary>
            /// 数值
            /// </summary>
            public readonly ET Value = default( ET );

            /// <summary>
            /// 名称
            /// </summary>
            public readonly string Name = null;
        }

    }
}
