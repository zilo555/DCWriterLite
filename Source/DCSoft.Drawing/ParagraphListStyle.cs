using System;
using System.Collections.Generic;
using System.Text;

namespace DCSoft.Drawing
{

    /// <summary>
    /// 段落列表样式
    /// </summary>
     
    [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
    public enum ParagraphListStyle
    {
        /// <summary>
        /// 无样式
        /// </summary>
        None,
        ///<summary>	默认的数字样式。 	</summary>	
        ListNumberStyle,
        ///<summary>	阿拉伯 1 型数字样式。 	</summary>	
        ListNumberStyleArabic1,
        ///<summary>	阿拉伯 2 型数字样式。 	</summary>	
        ListNumberStyleArabic2,
        ///<summary>	小写字母样式。 	</summary>	
        ListNumberStyleLowercaseLetter,
        ///<summary>	小写罗马样式。 	</summary>	
        ListNumberStyleLowercaseRoman,
        ///<summary>	带圈数字样式。 	</summary>	
        ListNumberStyleNumberInCircle,
        ///<summary>	大写字母样式。 	</summary>	
        ListNumberStyleUppercaseLetter,
        ///<summary>	大写罗马样式。 	</summary>	
        ListNumberStyleUppercaseRoman,
        ///<summary>	Zodiac 1 样式。 	</summary>	
        ListNumberStyleZodiac1,
        ///<summary>	Zodiac 2 样式。 	</summary>	
        ListNumberStyleZodiac2,
        ///<summary>	阿拉伯 2 型数字样式。 	</summary>	
        ListNumberStyleArabic3,
        /// <summary>
        /// 圆头列表
        /// </summary>
        BulletedList = 10000,
        /// <summary>
        /// 方块列表
        /// </summary>
        BulletedListBlock = 10001,
        /// <summary>
        /// 菱形方块列表
        /// </summary>
        BulletedListDiamond = 10002,
        /// <summary>
        /// 钩子列表
        /// </summary>
        BulletedListCheck = 10003,
        /// <summary>
        /// 向右的箭头列表
        /// </summary>
        BulletedListRightArrow = 10004,
        /// <summary>
        /// 空心星列表
        /// </summary>
        BulletedListHollowStar = 10005,
    }
}
