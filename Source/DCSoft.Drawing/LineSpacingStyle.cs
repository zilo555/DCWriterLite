using System;
using System.Collections.Generic;
using System.Text;

namespace DCSoft.Drawing
{
    /// <summary>
    /// 行间距样式，它决定了LineSpacing值的计量单位。
    /// </summary>
    [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
    public enum LineSpacingStyle
    {
        /// <summary>
        /// 单倍行距,此时LineSpacing值无意义。
        /// </summary>
        SpaceSingle = 0,
        /// <summary>
        /// 1.5倍行距,此时LineSpacing值无意义。
        /// </summary>
        Space1pt5,
        /// <summary>
        /// 双倍行距,此时LineSpacing值无意义。
        /// </summary>
        SpaceDouble,
        /// <summary>
        /// 最小值,此时LineSpacing值无意义。
        /// </summary>
        SpaceExactly,
        /// <summary>
        /// 固定值,此时LineSpacing指定了行间距。以三百分之一英寸为单位。
        /// </summary>
        SpaceSpecify,
        /// <summary>
        /// 多倍行距,此时LineSpacing指定的默认行高的倍数。
        /// </summary>
        SpaceMultiple
    }
}
