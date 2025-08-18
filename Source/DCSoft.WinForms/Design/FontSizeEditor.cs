using System;
using System.Text;
using System.ComponentModel;
//using System.Drawing.Design;
// // 
using System.Collections.Generic;

namespace DCSoft.WinForms.Design
{
    /// <summary>
    /// 字体大小信息
    /// </summary>
    public class FontSizeInfo
    {
        /// <summary>
        /// 根据字体大小获得字体大小的名称
        /// </summary>
        /// <param name="size">字体大小</param>
        /// <param name="includeChineseFontSize">包含中文字体大小名称</param>
        /// <returns>字体大小名称</returns>
        public static string GetStandSizeName(float size , bool includeChineseFontSize )
        {
            if (includeChineseFontSize)
            {
                foreach (FontSizeInfo item in StandSizes)
                {
                    if (Math.Abs(size - item.Size) < 0.01)
                    {
                        return item.Name;
                    }
                }
            }
            else
            {
                foreach (FontSizeInfo item in StandSizesWithoutChinese)
                {
                    if (Math.Abs(size - item.Size) < 0.01)
                    {
                        return item.Name;
                    }
                }
            }
            return size.ToString();
        }

        public static float GetFontSize(string fontSizeName , float defaultFontSize )
        {
            foreach (FontSizeInfo item in StandSizes)
            {
                if (string.Compare(fontSizeName, item.Name, true) == 0)
                {
                    return item.Size;
                }
            }
            float size = 0;
            if (float.TryParse(fontSizeName, out size))
            {
                return size;
            }
            else
            {
                return defaultFontSize ;
            }
        }

        private static FontSizeInfo[] myStandSizes = null;
        /// <summary>
        /// 获得标准字体大小列表
        /// </summary>
        public static FontSizeInfo[] StandSizes
        {
            get
            {
                if (myStandSizes == null)
                {
                    var list = new List<FontSizeInfo>();
                    if (System.Globalization.CultureInfo.CurrentUICulture.TwoLetterISOLanguageName
                        == "zh")
                    {
                        // 若为简体中文或者繁体中文系统则添加字号
                        list.Add(new FontSizeInfo("初号", 42));
                        list.Add(new FontSizeInfo("小初", 36));
                        list.Add(new FontSizeInfo("一号", 26.25f));
                        list.Add(new FontSizeInfo("小一", 24));
                        list.Add(new FontSizeInfo("二号", 21.75f));
                        list.Add(new FontSizeInfo("小二", 18));
                        list.Add(new FontSizeInfo("三号", 15.75f));
                        list.Add(new FontSizeInfo("小三", 15));
                        list.Add(new FontSizeInfo("四号", 14.25f));
                        list.Add(new FontSizeInfo("小四", 12));
                        list.Add(new FontSizeInfo("五号", 10.5f));
                        list.Add(new FontSizeInfo("小五", 9));
                        list.Add(new FontSizeInfo("六号", 7.5f));
                        list.Add(new FontSizeInfo("小六", 6.75f));
                        list.Add(new FontSizeInfo("七号", 5.25f));
                        list.Add(new FontSizeInfo("八号", 4.5f));
                    }
                    list.Add(new FontSizeInfo(8));
                    list.Add(new FontSizeInfo(9));
                    list.Add(new FontSizeInfo(10));
                    list.Add(new FontSizeInfo(11));
                    list.Add(new FontSizeInfo(12));
                    list.Add(new FontSizeInfo(14));
                    list.Add(new FontSizeInfo(16));
                    list.Add(new FontSizeInfo(18));
                    list.Add(new FontSizeInfo(20));
                    list.Add(new FontSizeInfo(22));
                    list.Add(new FontSizeInfo(24));
                    list.Add(new FontSizeInfo(26));
                    list.Add(new FontSizeInfo(28));
                    list.Add(new FontSizeInfo(36));
                    list.Add(new FontSizeInfo(48));
                    list.Add(new FontSizeInfo(72));
                    
                    myStandSizes = list.ToArray();
                }
                return myStandSizes;
            }
        }

        private static FontSizeInfo[] myStandSizesWithoutChinese = null;
        /// <summary>
        /// 获得标准字体大小列表,而且不含类似“三号、小三、四号、小四”等中文字体大小名称
        /// </summary>
        public static FontSizeInfo[] StandSizesWithoutChinese
        {
            get
            {
                if (myStandSizesWithoutChinese == null)
                {
                    var list = new List<FontSizeInfo>();
                    list.Add(new FontSizeInfo(8));
                    list.Add(new FontSizeInfo(9));
                    list.Add(new FontSizeInfo(10));
                    list.Add(new FontSizeInfo(11));
                    list.Add(new FontSizeInfo(12));
                    list.Add(new FontSizeInfo(14));
                    list.Add(new FontSizeInfo(16));
                    list.Add(new FontSizeInfo(18));
                    list.Add(new FontSizeInfo(20));
                    list.Add(new FontSizeInfo(22));
                    list.Add(new FontSizeInfo(24));
                    list.Add(new FontSizeInfo(26));
                    list.Add(new FontSizeInfo(28));
                    list.Add(new FontSizeInfo(36));
                    list.Add(new FontSizeInfo(48));
                    list.Add(new FontSizeInfo(72));

                    myStandSizesWithoutChinese = list.ToArray();
                }
                return myStandSizesWithoutChinese;
            }
        }

        /// <summary>
        /// 初始化对象
        /// </summary>
        /// <param name="name">字体名称</param>
        /// <param name="size">字体大小</param>
        public FontSizeInfo(string name, float size)
        {
            strName = name;
            fSize = size;
        }
        /// <summary>
        /// 初始化对象
        /// </summary>
        /// <param name="size">字体大小</param>
        public FontSizeInfo(float size)
        {
            strName = size.ToString();
            fSize = size;
        }

        private string strName = null;
        /// <summary>
        /// 字体名称
        /// </summary>
        public string Name
        {
            get { return strName; }
            set { strName = value; }
        }

        private float fSize = 9f;
        /// <summary>
        /// 字体大小
        /// </summary>
        public float Size
        {
            get { return fSize; }
            set { fSize = value; }
        }
        public override string ToString()
        {
            return strName;
        }
    }//public class FontSize
}
