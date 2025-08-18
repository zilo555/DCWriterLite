using System;

namespace DCSoft.Drawing
{
	/// <summary>
	/// 绘图单位转换
	/// </summary>
    public static class GraphicsUnitConvert
	{
        /// <summary>
        /// 进行单位换算
        /// </summary>
        /// <param name="vValue">长度值</param>
        /// <param name="OldUnit">旧单位</param>
        /// <param name="NewUnit">新单位</param>
        /// <returns>换算结果</returns>
        public static double Convert(
			double vValue ,
			GraphicsUnit OldUnit ,
			GraphicsUnit  NewUnit )
		{
			if( OldUnit == NewUnit )
				return vValue ;
			else
				return vValue * GetRate( NewUnit , OldUnit );
		}

        /// <summary>
        /// 进行单位换算
        /// </summary>
        /// <param name="vValue">长度值</param>
        /// <param name="OldUnit">旧单位</param>
        /// <param name="NewUnit">新单位</param>
        /// <returns>换算结果</returns>
        public static float Convert(
			float vValue ,
			GraphicsUnit OldUnit ,
			GraphicsUnit  NewUnit )
		{
            if (OldUnit == NewUnit)
            {
                return vValue;
            }
            else
            {
                return (float)(vValue * GetRate(NewUnit, OldUnit));
            }
		}

        /// <summary>
        /// 将长度转换为厘米
        /// </summary>
        /// <param name="vValue">长度值</param>
        /// <param name="oldUnit">长度单位</param>
        /// <returns>转换的厘米值</returns>
        public static float ConvertToCM(float vValue, GraphicsUnit oldUnit)
        {
            return (float)(Convert(vValue, oldUnit, GraphicsUnit.Millimeter) / 10.0);
        }

        /// <summary>
        /// 将厘米长度值转换为指定单位的长度
        /// </summary>
        /// <param name="cmValue">厘米长度值</param>
        /// <param name="unit">新的长度单位</param>
        /// <returns>转换结果</returns>
        public static float ConvertFromCM(float cmValue, GraphicsUnit unit)
        {
            return Convert( cmValue * 10.0f , GraphicsUnit.Millimeter , unit );
        }

        /// <summary>
        /// 进行单位换算
        /// </summary>
        /// <param name="vValue">长度值</param>
        /// <param name="OldUnit">旧单位</param>
        /// <param name="NewUnit">新单位</param>
        /// <returns>换算结果</returns>
        public static int Convert(
			int vValue ,
			 GraphicsUnit OldUnit ,
			 GraphicsUnit  NewUnit )
		{
			if( OldUnit == NewUnit )
				return vValue ;
			else
				return ( int ) ( vValue * GetRate( NewUnit , OldUnit ) );
		}

        /// <summary>
        /// 进行单位换算
        /// </summary>
        /// <param name="size">旧值</param>
        /// <param name="OldUnit">旧单位</param>
        /// <param name="NewUnit">新单位</param>
        /// <returns>换算结果</returns>
        public static DCSystem_Drawing.Size Convert(
            DCSystem_Drawing.Size size ,
			 GraphicsUnit OldUnit ,
			 GraphicsUnit NewUnit )
		{
			if( OldUnit == NewUnit )
				return size ;
			else
			{
				double rate = GetRate( NewUnit , OldUnit );
				return new DCSystem_Drawing.Size(
					( int ) ( size.Width * rate ) , 
					( int ) ( size.Height * rate ));
			}
		}

        /// <summary>
        /// 进行单位换算
        /// </summary>
        /// <param name="size">旧值</param>
        /// <param name="OldUnit">旧单位</param>
        /// <param name="NewUnit">新单位</param>
        /// <returns>换算结果</returns>
        public static DCSystem_Drawing.SizeF Convert(
            DCSystem_Drawing.SizeF size , 
			 GraphicsUnit OldUnit ,
			 GraphicsUnit NewUnit )
		{
            if (OldUnit == NewUnit)
            {
                return size;
            }
            else
            {
                double rate = GetRate(NewUnit, OldUnit);
                return new DCSystem_Drawing.SizeF(
                    (float)(size.Width * rate),
                    (float)(size.Height * rate));
            }
		}

        /// <summary>
        /// 将一个长度从旧单位换算成新单位使用的比率
        /// </summary>
        /// <param name="NewUnit">新单位</param>
        /// <param name="OldUnit">旧单位</param>
        /// <returns>比率数</returns>
        public static double GetRate(
			 GraphicsUnit NewUnit ,
			 GraphicsUnit OldUnit )
		{
            if(NewUnit == OldUnit )
            {
                return 1;
            }
			return GetUnit( OldUnit ) / GetUnit( NewUnit )  ;
		}


        /// <summary>
        /// 将像素单位转换为打印单位
        /// </summary>
        /// <param name="v">像素单位</param>
        /// <returns>转换后的打印单位</returns>
        public static double PixelToPrintUnit(double v)
        {
            return Convert(v, GraphicsUnit.Pixel, GraphicsUnit.Inch) * 100;
        }

		/// <summary>
		/// 获得一个单位占据的英寸数
		/// </summary>
		/// <param name="unit">单位类型</param>
		/// <returns>英寸数</returns>
		public static double GetUnit(  GraphicsUnit unit )
		{
			switch( unit )
			{
				case  GraphicsUnit.Display :
					return 1 / 96.0 ;
				case  GraphicsUnit.Document :
					return 1 / 300.0 ;
				case  GraphicsUnit.Inch :
					return 1 ;
				case  GraphicsUnit.Millimeter :
					return 1 / 25.4 ;
				case  GraphicsUnit.Pixel :
					return 1 / 96.0 ;
				case  GraphicsUnit.Point :
					return 1 / 72.0 ;
				default:
					throw new System.NotSupportedException("Not support " + unit.ToString());
			}
		}

        public static double ToPixel(double Value, GraphicsUnit unit, float dpi)
        {
            if (dpi <= 0)
            {
                throw new ArgumentOutOfRangeException("dpi=" + dpi);
            }
            double v = GetUnit(unit);
            double result = Value * v * dpi;
            return result;
        }


        /// <summary>
        /// 将指定单位的指定长度转化为 Twips 单位
        /// </summary>
        /// <param name="Value">长度</param>
        /// <param name="unit">度量单位</param>
        /// <returns>转化的 Twips 数</returns>
        public static int ToTwips(float Value,  GraphicsUnit unit)
        {
            double v = GetUnit(unit);
            return (int)(Value * v * 1440);
        }
	}
}
