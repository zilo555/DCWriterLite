using System;
using System.Reflection;

namespace DCSoft.Common
{ 
	/// <summary>
	/// 单元格序号对象
	/// </summary>
	/// <remarks>本对象用于管理类似Excel单元格序号的信息,
	/// 位置字符串前面是一个以上连续的字母,后面是一个以上连续的数字字符,
	/// 字母部分表示列号,而数字部分表示行号,比如字符串"B1"表示第1行第2列,
	/// 本对象的行号和列号都是从1开始计数的。
    /// 编制 袁永福</remarks>
    public class CellIndex
	{
        private static System.Text.StringBuilder _String_CellIndex = new System.Text.StringBuilder(10);
		/// <summary>
		/// 根据从1开始计算的行号和列号获得位置字符串,行列号从1开始
		/// </summary>
		/// <param name="RowIndex">从1开始计算的行号</param>
		/// <param name="ColIndex">从1开始计算的列号</param>
		/// <returns>位置字符串</returns>
		public static string GetCellIndex( int RowIndex , int ColIndex ) 
		{
            GetColWord(ColIndex, _String_CellIndex);
            _String_CellIndex.Append(RowIndex);
            var s = _String_CellIndex.ToString();
            _String_CellIndex.Clear();
            return s;
		}
        private static readonly char[] _ColWords = "0ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
        /// <summary>
        /// 将从1开始计算的列号转换为列字符串
        /// </summary>
        /// <param name="ColIndex">列号</param>
        /// <returns>列字符串</returns>
        public static void GetColWord(int ColIndex, System.Text.StringBuilder myStr)
        {
            if (ColIndex < 26)
            {
                myStr.Append(_ColWords[ColIndex]);
            }
            else
            {
                while (true)
                {
                    int index = ColIndex % 26;
                    if (index == 0)
                    {
                        myStr.Insert(0, 'Z');
                        index = 26;
                    }
                    else
                    {
                        myStr.Insert(0, _ColWords[index]);
                    }
                    if (ColIndex <= 26)
                    {
                        break;
                    }
                    ColIndex = (ColIndex - index) / 26;
                }
            }
        }
    }//public class CellIndex
}