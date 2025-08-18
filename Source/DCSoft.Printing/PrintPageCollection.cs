using System;
using DCSoft.Common;
using System.Collections.Generic;

namespace DCSoft.Printing
{
    /// <summary>
    /// 打印页集合对象
    /// </summary>
#if !RELEASE
    [System.Diagnostics.DebuggerDisplay("Count={ Count }")]
    [System.Diagnostics.DebuggerTypeProxy(typeof(DCSoft.Common.ListDebugView))]
#endif
    public partial class PrintPageCollection : List<PrintPage>
    {
        /// <summary>
        /// 初始化对象
        /// </summary>
        public PrintPageCollection()
        {
        }

        private float _Top = 0;
        public PrintPage SafeGet(int index)
        {
            if (index >= 0 && index < this.Count)
            {
                return this[index];
            }
            else
            {
                return null;
            }
        }
        private GraphicsUnit intGraphicsUnit
            = GraphicsUnit.Document;
        /// <summary>
        /// 度量单位
        /// </summary>
        public GraphicsUnit GraphicsUnit
        {
            get
            {
                return intGraphicsUnit;
            }
            set
            {
                intGraphicsUnit = value;
            }
        }


        private int intMinPageHeight = 50;
        /// <summary>
        /// 最小页高
        /// </summary>

        public int MinPageHeight
        {
            get
            {
                return intMinPageHeight;
            }
            set
            {
                intMinPageHeight = value;
            }
        }

        /// <summary>
        /// 对象的顶端位置
        /// </summary>
        public float Top
        {
            get
            {
                return _Top;
            }
            set
            {
                _Top = value;
            }
        }

        /// <summary>
        /// 获得第一页
        /// </summary>
        public PrintPage FirstPage
        {
            get
            {
                if (this.Count > 0)
                {
                    return this[0];
                }
                else
                {
                    return null;
                }
            }
        }
        /// <summary>
        /// 获得最后一页
        /// </summary>
        public PrintPage LastPage
        {
            get
            {
                if (this.Count > 0)
                {
                    return this[this.Count - 1];
                }
                else
                {
                    return null;
                }
            }
        }


        /// <summary>
        /// 所有页面的高度
        /// </summary>
        public float Height
        {
            get
            {
                float intHeight = 0;
                foreach (PrintPage myPage in this)
                {
                    intHeight += myPage.Height;
                }
                return intHeight;
            }
        }


        public int IndexOfByPosition(float position, int defaultValue)
        {
            if (this.Count == 0)
            {
                return -1;
            }
            if (position == 0 && this[0].Top == 0)
            {
                return 0;
            }
            // 二分法查找页码
            int startIndex = 0;
            int endIndex = this.Count - 1;
            while (endIndex - startIndex > 10)
            {
                int index = (startIndex + endIndex) / 2;
                if (position >= this[index].Top)
                {
                    startIndex = index;
                }
                else
                {
                    endIndex = index;
                }
            }
            for (int iCount = endIndex; iCount >= startIndex; iCount--)
            {
                if (position >= this[iCount].Top)
                {
                    return iCount;
                }
            }
            return defaultValue;
        }
    }
}