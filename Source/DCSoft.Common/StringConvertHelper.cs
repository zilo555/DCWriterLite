using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

namespace DCSoft.Common
{
	/// <summary>
	/// DCWriter内部使用。将字符串转换为各种类型数值的通用例程
	/// </summary>
	/// <remarks>本模块不依赖其他代码文件。编制袁永福</remarks>
    public sealed class StringConvertHelper
	{

        public static string GetLetterNumber(int n , string letters )
        {
            if (string.IsNullOrEmpty(letters))
            {
                throw new ArgumentNullException("letters");
            }
            if (n <= 0)
            {
                return letters[0].ToString();
            }
            StringBuilder str = new StringBuilder();
            int len = letters.Length;
            while (n > 0)
            {
                int v = n % len;
                str.Insert(0, letters[v]);
                n = (n - v) / len;
            }
            return str.ToString();
        }

        /// <summary>
        /// 将数字转换为大写的罗马数字文本
        /// </summary>
        /// <param name="n">数字</param>
        /// <returns>转换后的罗马数字文本</returns>
        public static string GetRomanNumber(int n)
        {
            int[] arabic = new int[13];
            string[] roman = new string[13];
            int i = 0;
            
            arabic[0] = 1000;
            arabic[1] = 900;
            arabic[2] = 500;
            arabic[3] = 400;
            arabic[4] = 100;
            arabic[5] = 90;
            arabic[6] = 50;
            arabic[7] = 40;
            arabic[8] = 10;
            arabic[9] = 9;
            arabic[10] = 5;
            arabic[11] = 4;
            arabic[12] = 1;

            roman[0] = "M";
            roman[1] = "CM";
            roman[2] = "D";
            roman[3] = "CD";
            roman[4] = "C";
            roman[5] = "XC";
            roman[6] = "L";
            roman[7] = "XL";
            roman[8] = "X";
            roman[9] = "IX";
            roman[10] = "V";
            roman[11] = "IV";
            roman[12] = "I";
            StringBuilder str = new StringBuilder();
            while (n > 0)
            {
                while (n >= arabic[i])
                {
                    n = n - arabic[i];
                    str.Append(roman[i]);
                    //o = o + roman[i];
                }
                i++;
            }
            return str.ToString();
            //return o;
        }

        /// <summary>
        /// 判断一个字符串是否全部由数字字符组成
        /// </summary>
        /// <param name="strData">要测试的字符串</param>
        /// <returns>若全部由数字字符组成则返回true 否则返回false ，字符串对象为空时也返回false</returns>
        public static bool IsIntegerString(string strData)
        {
            if (strData != null && strData.Length > 0)
            {
                var len = strData.Length;
                for (int iCount = 0; iCount < len; iCount++)
                {
                    char c = strData[iCount];
                    if (c > '9' || c < '0')
                    {
                        return false;
                    }
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        public static DateTime ToDBTime(string txt, DateTime defaultValue)
        {
            //bolConvertFalseFlag = true;
            try
            {
                if (IsIntegerString(txt) == false)
                {
                    return defaultValue;
                }
                else
                {
                    var hour = 0;
                    var min = 0;
                    var second = 0;
                    if (txt.Length >= 2)
                    {
                        hour = Convert.ToInt32(txt.Substring(0, 2));
                    }
                    if (txt.Length >= 4)
                    {
                        min = Convert.ToInt32(txt.Substring(2, 2));
                    }
                    if (txt.Length >= 6)
                    {
                        second = Convert.ToInt32(txt.Substring(4, 2));
                    }
                    //bolConvertFalseFlag = true;
                    return new DateTime(1900, 1, 1, hour, min, second);
                }
            }
            catch
            {
                return defaultValue;
            }
        }

		/// <summary>
		/// 将yyyyMMddHHmmss 格式的字符串转化为时间数据
		/// </summary>
		/// <param name="strData">原始字符串</param>
		/// <param name="DefaultValue">默认值</param>
		/// <returns>转换后的时间数据</returns>
		public static System.DateTime ToDBDateTime(string strData , System.DateTime DefaultValue)
		{
			//bolConvertFalseFlag = true;
			try
			{
				if( IsIntegerString( strData ) == false )
				{
					return DefaultValue ;
				}
				else
				{
                    if (strData.Length < 4)
                    {
                        return DefaultValue;
                    }
					int year = 1 ;
					int month = 1 ;
					int day = 1 ;
					int hour = 0 ;
					int minutes = 0 ;
					int secend = 0 ;
					// 年数
					year = Convert.ToInt32( strData.Substring( 0 , 4 ));
					if( strData.Length >= 6 )
					{
						// 月份数
						month = Convert.ToInt32( strData.Substring( 4 , 2 ));
						if( month <= 0 || month > 12 )
							return DefaultValue ;
					}	
					if( strData.Length >= 8 )
					{
						// 天数
						day = Convert.ToInt32( strData.Substring( 6 , 2 ));
						if( day <= 0 || day > DateTime.DaysInMonth( year , month ) )
							return DefaultValue ;
					}
					if( strData.Length >= 10 )
					{
						// 小时数
						hour = Convert.ToInt32( strData.Substring( 8 , 2 ));
						if( hour > 60 )
							return DefaultValue ;
					}
					if( strData.Length >= 12 )
					{
						// 分钟
						minutes = Convert.ToInt32( strData.Substring( 10 , 2 ));
						if( minutes > 60 )
							return DefaultValue ;
					}
					if( strData.Length >= 14 )
					{
						// 秒
						secend = Convert.ToInt32( strData.Substring( 12 , 2 ));
						if( secend > 60 )
							return DefaultValue ;
					}
					DateTime dtm = new DateTime( year , month , day , hour , minutes , secend );
					//bolConvertFalseFlag = false;
					return dtm ;
				}
			}
			catch
			{
				return DefaultValue;
			}
		}


		/// <summary>
		/// 处理指定的字符串，将其中包含的所有的整数数值提取出来组成一个整数数组
		/// </summary>
		/// <param name="strText">要处理的字符串</param>
		/// <returns>整数数组</returns>
		public static int[] ToInt32Values( string strText )
		{
            if (string.IsNullOrEmpty(strText))
            {
                return null;
            }
			var myList = new System.Collections.ArrayList();
			int iData = 0 ;
			bool Reading = false;
			bool bolNegative = false;
			for(int iCount = 0 ; iCount < strText.Length ; iCount ++)
			{
				bool bolAdd = ( iCount == strText.Length -1 );
				char c = strText[iCount];
				if( c >= '0' && c <= '9' )
				{
					iData = iData * 10 + c - '0';
					Reading = true;
				}
				else if( c == '-')
				{
					if( Reading )
					{
						bolAdd = true;
						iCount -- ;
					}
					else
						bolNegative = true;
				}
				else
				{
					bolAdd = true;
				}//else
				if( bolAdd )
				{
					if( Reading )
					{
						if( bolNegative )
							iData = 0 - iData ;
						myList.Add( iData );
						iData = 0 ;
					}
					bolNegative = false;
					Reading = false;
				}
			}//for
			if( myList.Count > 0 )
			{
				return ( int[]) myList.ToArray( typeof( int ));
			}
			else
				return null;
		}//public static int[] ToInt32Values( string strText )
    }//public class StringConvertHelper
}