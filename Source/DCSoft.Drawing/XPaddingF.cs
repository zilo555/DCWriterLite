using System;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Collections;
// // 
using System.Collections.Generic;

namespace DCSoft.Drawing
{
    /// <summary>
    /// 内间距信息
    /// </summary>
    //[System.ComponentModel.TypeConverter(typeof(XPaddingFConverter))]
    //[Serializable()]
     
    //[System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = false )]
    public partial class XPaddingF : ICloneable
    {
        /// <summary>
        /// 初始化对象
        /// </summary>
        public XPaddingF()
        {
        }

        /// <summary>
        /// 初始化对象
        /// </summary>
        /// <param name="v">间距值</param>
        public XPaddingF(float v)
        {
            fLeft = v;
            fTop = v;
            fRight = v;
            fBottom = v;
        }

        /// <summary>
        /// 初始化对象
        /// </summary>
        /// <param name="left">内左间距</param>
        /// <param name="top">内上间距</param>
        /// <param name="right">内右间距</param>
        /// <param name="bottom">内下间距</param>
        public XPaddingF(float left, float top, float right, float bottom)
        {
            fLeft = left;
            fTop = top;
            fRight = right;
            fBottom = bottom;
        }

        private float fLeft = 0;

        /// <summary>
        /// 内左间距
        /// </summary>
        [System.ComponentModel.DefaultValue(0f)]
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public float Left
        {
            get
            {
                return fLeft;
            }
            set
            {
                fLeft = value;
            }
        }

        private float fTop = 0;
        /// <summary>
        /// 内上间距
        /// </summary>
        [System.ComponentModel.DefaultValue(0f)]
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public float Top
        {
            get
            {
                return fTop;
            }
            set
            {
                fTop = value;
            }
        }

        private float fRight = 0;
        /// <summary>
        /// 内右间距
        /// </summary>
        [System.ComponentModel.DefaultValue(0f)]
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public float Right
        {
            get
            {
                return fRight;
            }
            set
            {
                fRight = value;
            }
        }

        private float fBottom = 0;
        /// <summary>
        /// 内下间距
        /// </summary>
        [System.ComponentModel.DefaultValue(0f)]
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public float Bottom
        {
            get
            {
                return fBottom;
            }
            set
            {
                fBottom = value;
            }
        }

        /// <summary>
        /// 复制对象
        /// </summary>
        /// <returns>复制品</returns>
        object ICloneable.Clone()
        {
            return this.MemberwiseClone();
        }

        /// <summary>
        /// 将字符串解析成对象数据
        /// </summary>
        /// <param name="text">字符串</param>
        /// <param name="padding">保存解析结果的对象</param>
        /// <returns>操作是否成功</returns>
        public static bool Parse(string text, XPaddingF padding)
        {
            if (string.IsNullOrEmpty(text))
            {
                return false;
            }
            if (padding == null)
            {
                return false;
            }
            char ch = ',';// culture.TextInfo.ListSeparator[0];
            string[] strArray = text.Split(new char[] { ch });
            List<float> nums = new List<float>();
            for (int iCount = 0; iCount < strArray.Length; iCount++)
            {
                float v = 0;
                if (float.TryParse(strArray[iCount], out v))
                {
                    nums.Add(v);
                }
            }
            XPaddingF result = new XPaddingF();
            switch (nums.Count)
            {
                case 0:
                    return false;
                case 1:
                    padding.Left = nums[0];
                    padding.Top = nums[0];
                    padding.Right = nums[0];
                    padding.Bottom = nums[0];
                    break;
                case 2:
                    padding.Left = nums[0];
                    padding.Top = nums[1];
                    padding.Right = nums[0];
                    padding.Bottom = nums[0];
                    break;
                case 3:
                    padding.Left = nums[0];
                    padding.Top = nums[1];
                    padding.Right = nums[2];
                    padding.Bottom = nums[2];
                    break;
                default:
                    if (nums.Count >= 4)
                    {
                        padding.Left = nums[0];
                        padding.Top = nums[1];
                        padding.Right = nums[2];
                        padding.Bottom = nums[3];
                    }
                    break;
            }
            return true;
        }
#if !(RELEASE || LightWeight)
        /// <summary>
        /// 返回表示对象数据的字符串
        /// </summary>
        /// <returns>字符串</returns>
        public override string ToString()
        {
            if (this.Left == this.Top
                && this.Left == this.Right
                && this.Left == this.Bottom)
            {
                return this.Left.ToString();
            }
            else
            {
                return this.Left + "," + this.Top + "," + this.Right + "," + this.Bottom;
            }
        }
#endif
    }

    ///// <summary>
    ///// 内边距信息对象转换器
    ///// </summary>
    //[System.Runtime.InteropServices.ComVisible(false)]
    //public class XPaddingFConverter : TypeConverter
    //{
    //    /// <summary>
    //    /// 初始化对象
    //    /// </summary>
    //    public XPaddingFConverter()
    //    {
    //    }
    //    // Methods
    //    /// <summary>
    //    /// 判断能否进行数据转换
    //    /// </summary>
    //    /// <param name="context"></param>
    //    /// <param name="sourceType"></param>
    //    /// <returns></returns>
    //    public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
    //    {
    //        return ((sourceType == typeof(string)) || base.CanConvertFrom(context, sourceType));
    //    }

    //    /// <summary>
    //    /// 判断能否进行数据转换
    //    /// </summary>
    //    /// <param name="context"></param>
    //    /// <param name="destinationType"></param>
    //    /// <returns></returns>
    //    public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
    //    {
    //        return ((destinationType == typeof(InstanceDescriptor)) || base.CanConvertTo(context, destinationType));
    //    }

    //    /// <summary>
    //    /// 把原始数据转换为内边距信息对象
    //    /// </summary>
    //    /// <param name="context"></param>
    //    /// <param name="culture"></param>
    //    /// <param name="Value"></param>
    //    /// <returns></returns>
    //    public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object Value)
    //    {
    //        if (Value is string)
    //        {
    //            XPaddingF result = new XPaddingF();
    //            XPaddingF.Parse((string)Value, result);
    //            return result;
    //        }
    //        else
    //        {
    //            return base.ConvertFrom( context , CultureInfo.CurrentCulture , Value );
    //        }
    //    }

    //    /// <summary>
    //    /// 将对象转换为指定类型
    //    /// </summary>
    //    /// <param name="context"></param>
    //    /// <param name="culture"></param>
    //    /// <param name="Value"></param>
    //    /// <param name="destinationType"></param>
    //    /// <returns></returns>
    //    public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object Value, Type destinationType)
    //    {
    //        if (destinationType == null)
    //        {
    //            throw new ArgumentNullException("destinationType");
    //        }
    //        if (Value is XPaddingF)
    //        {
    //            if (destinationType == typeof(string))
    //            {
    //                XPaddingF padding = (XPaddingF)Value;
    //                return padding.ToString();
    //            }
    //            if (destinationType == typeof(InstanceDescriptor))
    //            {
    //                XPaddingF padding2 = (XPaddingF)Value;
    //                if (padding2.Left == padding2.Top
    //                    && padding2.Left == padding2.Right
    //                    && padding2.Left == padding2.Bottom)
    //                {
    //                    return new InstanceDescriptor(typeof(XPaddingF).GetConstructor(
    //                        new Type[] { 
    //                            typeof(float)
    //                            }),
    //                        new object[] { 
    //                            padding2.Left
    //                            });
    //                }
    //                else
    //                {
    //                    return new InstanceDescriptor(typeof(XPaddingF).GetConstructor(
    //                        new Type[] { 
    //                            typeof(float), 
    //                            typeof(float),
    //                            typeof(float),
    //                            typeof(float) }),
    //                        new object[] { 
    //                            padding2.Left,
    //                            padding2.Top,
    //                            padding2.Right, 
    //                            padding2.Bottom });
    //                }
    //            }
    //        }
    //        return base.ConvertTo(context, culture, Value, destinationType);
    //    }

    //    /// <summary>
    //    /// 根据设置创建对象实例
    //    /// </summary>
    //    /// <param name="context"></param>
    //    /// <param name="propertyValues"></param>
    //    /// <returns></returns>
    //    public override object CreateInstance(ITypeDescriptorContext context, IDictionary propertyValues)
    //    {
    //        if (context == null)
    //        {
    //            throw new ArgumentNullException("context");
    //        }
    //        if (propertyValues == null)
    //        {
    //            throw new ArgumentNullException("propertyValues");
    //        }
    //        return new XPaddingF(
    //            (float)propertyValues["Left"],
    //            (float)propertyValues["Top"],
    //            (float)propertyValues["Right"],
    //            (float)propertyValues["Bottom"]);
    //    }
    //    /// <summary>
    //    /// 可获得属性
    //    /// </summary>
    //    /// <param name="context"></param>
    //    /// <returns></returns>
    //    public override bool GetCreateInstanceSupported(ITypeDescriptorContext context)
    //    {
    //        return true;
    //    }

    //    /// <summary>
    //    /// 获得可设计的属性
    //    /// </summary>
    //    /// <param name="context"></param>
    //    /// <param name="value"></param>
    //    /// <param name="attributes"></param>
    //    /// <returns></returns>
    //    public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
    //    {
    //        return TypeDescriptor.GetProperties(typeof(XPaddingF), attributes).Sort(
    //            new string[] { "Left", "Top", "Right", "Bottom" });
    //    }
    //    /// <summary>
    //    /// 支持获得属性
    //    /// </summary>
    //    /// <param name="context"></param>
    //    /// <returns></returns>
    //    public override bool GetPropertiesSupported(ITypeDescriptorContext context)
    //    {
    //        return true;
    //    }
    //}
}
