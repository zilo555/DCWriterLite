using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace DCSoft.WinForms
{
    /// <summary>
    /// 鼠标点击计数器
    /// </summary>
    public class MouseClickCounter
    {
        private readonly static MouseClickCounter _Instance = new MouseClickCounter();
        /// <summary>
        /// 对象唯一静态实例
        /// </summary>
        public static MouseClickCounter Instance
        {
            get { return _Instance; }
        }


        private class ClickItem
        {
            public int X = 0;
            public int Y = 0;
            public int TickCount = 0;
        }

        private readonly List<ClickItem> myItems = new List<ClickItem>();
        /// <summary>
        /// 当前记录的鼠标连续点击次数
        /// </summary>
        public int Value
        {
            get
            {
                return myItems.Count;
            }
        }
        /// <summary>
        /// 进行点击次数累计
        /// </summary>
        /// <param name="x">鼠标X坐标值</param>
        /// <param name="y">鼠标Y坐标值</param>
        /// <returns>连续点击的次数</returns>
        public int Count(int x, int y)
        {
            return Count(x, y, System.Environment.TickCount);
        }

        /// <summary>
        /// 进行鼠标点击次数累计
        /// </summary>
        /// <param name="x">鼠标光标X坐标</param>
        /// <param name="y">鼠标光标Y坐标</param>
        /// <param name="tickCount">鼠标点击时的系统时刻数</param>
        /// <returns>连续点击的次数</returns>
        public int Count(int x, int y , int tickCount )
        {
            ClickItem item = new ClickItem();
            item.X = x;
            item.Y = y;
            item.TickCount = System.Environment.TickCount;
            if (myItems.Count == 0)
            {
                myItems.Add(item);
            }
            else
            {
                ClickItem LastItem = (ClickItem)myItems[myItems.Count - 1];
                 Size size = SystemInformation.DoubleClickSize;
                if (Math.Abs(item.X - LastItem.X) <= size.Width
                    && Math.Abs(item.Y - LastItem.Y) <= size.Height
                    && Math.Abs( item.TickCount - LastItem.TickCount ) <= 
                        SystemInformation.DoubleClickTime)
                {
                    myItems.Add(item);
                    return myItems.Count;
                }
                else
                {
                    myItems.Clear();
                    myItems.Add(item);
                }
            }
            return myItems.Count ;
        }
    }
}
