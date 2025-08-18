using System;
//using System.Data ;
using System.Text;
using System.Reflection;

namespace DCSoft.Common
{
	/// <summary>
	/// DCWriter内部使用。字符串格式处理类
	/// </summary>
    /// <remarks>编制 袁永福</remarks>
    public static class StringFormatHelper
	{
		/// <summary>
		/// 判断是否是英文字母
		/// </summary>
		/// <param name="c">字符</param>
		/// <returns>是否为英文字母</returns>
		public static bool IsEnglishLetter(char c)
        {
            return c >= 'a' && c <= 'z' || c >= 'A' && c <= 'Z';
        }


		/// <summary>
		/// 格式化字符串，进行分组处理
		/// </summary>
		/// <param name="strData">连续的字符串</param>
		/// <param name="GroupSize">一组编码的字符个数</param>
		/// <param name="LineGroupCount">每行文本的编码组个数</param>
		/// <returns>格式化后的字符串</returns>
		public static string GroupFormatString(string strData , int GroupSize , int LineGroupCount)
		{
			if (string.IsNullOrEmpty(strData) || (GroupSize <= 0 && LineGroupCount <= 0))
			{
				return strData;
			}
			var myStr = new System.Text.StringBuilder( (int)(strData.Length * 1.1));
			int iSize = strData.Length ;
			int iCount = 0 ;
			LineGroupCount *= GroupSize ;

			while(true)
			{
				myStr.Append(' ');
				if(iCount + GroupSize < iSize)
				{
                    myStr.Append(strData, iCount, GroupSize);
				}
				else
				{
                    myStr.Append(strData, iCount, iSize - iCount);
					break;
				}
				iCount += GroupSize ;
                if (iCount % LineGroupCount == 0)
                {
                    myStr.AppendLine();
                }
			}
			return myStr.ToString();
		}
		 
	}//public sealed class StringFormatHelper
}