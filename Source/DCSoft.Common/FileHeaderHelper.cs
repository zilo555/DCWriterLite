using System;
using System.Text;
using System.IO;

namespace DCSoft.Common
{
    /// <summary>
    /// 文件头判断 编写袁永福
    /// </summary>
    public static class FileHeaderHelper
    {
        private readonly static byte[] pngHeader = new byte[] { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A };
        /// <summary>
        /// 判断数据块是否具有PNG图片文件标记头
        /// </summary>
        /// <param name="bs">数据块</param>
        /// <returns>是否具有PNG图片文件标记头</returns>
        public static bool HasPNGHeader(byte[] bs)
        {
            if (bs != null && bs.Length >= pngHeader.Length)
            {
                for (int iCount = 0; iCount < pngHeader.Length; iCount++)
                {
                    if (bs[iCount] != pngHeader[iCount])
                        return false;
                }
                return true;
            }
            return false;
        }


        /// <summary>
        /// 判断数据块是否具有GIF图像文件标记头
        /// </summary>
        /// <param name="bs">数据块</param>
        /// <returns>是否具有GIF图像文件标记头</returns>
        public static bool HasGIFHeader(byte[] bs)
        {
            if (bs != null && bs.Length >= 6)
            {
                if (bs[0] == 0x47 && bs[1] == 0x49 && bs[2] == 0x46)
                {
                    if (bs[3] == 0x38 && bs[4] == 0x37 && bs[5] == 0x61)
                    {
                        // 以 GIF87a 开头
                        return true;
                    }
                    if (bs[3] == 0x38 && bs[4] == 0x39 && bs[5] == 0x61)
                    {
                        // 以 GIF89a 开头
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// 判断数据块是否具有BMP标记头
        /// </summary>
        /// <param name="bs">数据块</param>
        /// <returns>是否具有BMP标记头</returns>
        public static bool HasBMPHeader(byte[] bs)
        {
            if (bs != null && bs.Length >= 2)
            {
                if (bs[0] == 0x42 && bs[1] == 0x4d)
                    return true;
            }
            return false;
        }
        /// <summary>
        /// 判断二进制数据是否具有JPEG格式的标记头
        /// </summary>
        /// <param name="bs"></param>
        /// <returns></returns>
        public static bool HasJpegHeader(byte[] bs)
        {
            return HasJpegHeader(bs, false);
        }
        /// <summary>
        /// 判断二进制数据是否具有JPEG格式的标记头
        /// </summary>
        /// <param name="bs">二进制数据</param>
        /// <param name="strict">是否进行严格的判断</param>
        /// <returns>是否有JPEG标记头</returns>
        public static bool HasJpegHeader(byte[] bs, bool strict)
        {
            if (bs != null && bs.Length >= 12)
            {
                int length = bs.Length;
                if (strict)
                {
                    if (bs[length - 2] != 0xff
                        || bs[length - 1] != 0xd9)
                    {
                        return false;
                    }
                }
                //调整判断JPG的逻辑，只判断开头为FFD8则通过
                if (bs[0] == 0xff
                    && bs[1] == 0xd8
                    && bs[2] == 0xff
                    //&& bs[10] == 0x00
                    //&& bs[length - 2] == 0xff
                    //&& bs[length - 1] == 0xd9
                    )
                {
                    return true;
                    //if (bs[3] == 0xe0
                    //    && bs[6] == 0x4a
                    //    && bs[7] == 0x46
                    //    && bs[8] == 0x49
                    //    && bs[9] == 0x46)//JFIF
                    //{
                    //    return true;
                    //}

                    //if (bs[3] == 0xe1
                    //    && bs[6] == 0x45
                    //    && bs[7] == 0x78
                    //    && bs[8] == 0x69
                    //    && bs[9] == 0x66)//Exif
                    //{
                    //    return true;
                    //}
                }
            }
            return false;
        }
    }
}