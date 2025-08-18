using System;
// // 
using System.Collections;
using System.Collections.Generic;

namespace DCSoft.Drawing
{
	/// <summary>
	/// 几何以及数学运算的通用例程模块
	/// </summary>
    public static class MathCommon
	{
        /// <summary>
        /// 获得指定数值的所有的约数，返回一个整数数组，小的在前，大的在后。
        /// </summary>
        /// <param name="Value">数值</param>
        /// <returns>约数组成的数组</returns>
        public static int[] GetApproximateNumbers(int Value)
        {
            List<int> result = new List<int>();
            //result.Add(1);
            for (int iCount = 1; iCount <= Value; iCount++)
            {
                if (( Value % iCount) == 0)
                {
                    result.Add(iCount);
                }
            }
            return result.ToArray();
        }

        /// <summary>
        /// 修正元素的大小，使得能完全的放置在一个容器中而不被剪切掉
        /// </summary>
        /// <param name="containerSize">容器的大小</param>
        /// <param name="elementSize">元素的原始大小</param>
        /// <param name="keepWidthHeightRate">是否保持元素的宽度高度比率</param>
        /// <returns>修正后的元素大小</returns>
        public static DCSystem_Drawing.SizeF FixSize(DCSystem_Drawing.SizeF containerSize, DCSystem_Drawing.SizeF elementSize, bool keepWidthHeightRate)
        {
            if (elementSize.Width <= 0 || elementSize.Height <= 0)
            {
                // 元素宽度或者高度出现0值
                if (elementSize.Width <= 0)
                {
                    elementSize.Width = Math.Min(containerSize.Width, elementSize.Width);
                }
                if (elementSize.Height <= 0)
                {
                    elementSize.Height = Math.Min(containerSize.Height, elementSize.Height);
                }
                return elementSize;
            }
            if (elementSize.Width > containerSize.Width
                || elementSize.Height > containerSize.Height)
            {
                // 元素的宽度或者高度大于容器则需要进行修正
                if (keepWidthHeightRate)
                {
                    // 计算缩小比例
                    double zoomRate = Math.Min(
                        containerSize.Width / elementSize.Width,
                        containerSize.Height / elementSize.Height);
                    DCSystem_Drawing.SizeF result = new DCSystem_Drawing.SizeF(
                        (float)(elementSize.Width * zoomRate),
                        (float)(elementSize.Height * zoomRate));
                    return result;
                }
                else
                {
                    DCSystem_Drawing.SizeF result = new DCSystem_Drawing.SizeF(
                        Math.Min(elementSize.Width, containerSize.Width),
                        Math.Min(elementSize.Height, containerSize.Height));
                    return result;
                }
            }
            else
            {
                // 无需修正，返回原值
                return elementSize;
            }
        }

        /// <summary>
        /// 设置标志位
        /// </summary>
        /// <param name="intAttributes">原始的标志数据</param>
        /// <param name="intValue">要设置的标志位的数据</param>
        /// <param name="bolSet">是否设置或者清除</param>
        /// <returns>修改后的标志数据</returns>
        public static int SetIntAttribute(int intAttributes , int intValue , bool bolSet)
		{
			return bolSet ? intAttributes | intValue : intAttributes & ~ intValue ;
		}

	}//public class MathCommon
}