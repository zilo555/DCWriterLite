using System.ComponentModel ;
// // 
//using  Imaging;
using DCSoft.Common;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks.Sources;

namespace DCSoft.Drawing
{
    /// <summary>
    /// 设计文档图片数据对象
    /// </summary>
    /// <remarks>
    /// 设计文档图片数据对象。它是 Image的一个封装，这个对象保存图片对象，还保存构造图片对象的原始二进制数据。
    /// <br />在设计器的属性列表中，需要从一个文件中加载图片数据，为了保持原始数据的完整性，在保存设计文档时是保存加载图片
    /// 的二进制数据的，加载设计文档时，会从这个原始的二进制数据来加载图片，这样保证的设计的完整性。本对象就是图片和二进制
    /// 数据的混合封装。方便程序加载和保存图片数据。
    /// <br />本对象内部使用了  Image 对象,可能使用了非托管资源,因此实现了IDisposable 接口,可以用来显式的释放
    /// 非托管资源.
    /// </remarks>
    public partial class XImageValue : System.IDisposable
    {
        /// <summary>
        /// 初始化对象
        /// </summary>
        public XImageValue()
        {
        }

        /// <summary>
        /// 初始化对象
        /// </summary>
        /// <param name="img">图片数据</param>
        public XImageValue( Image img)
        {
            this.Value = img;
        }

        /// <summary>
        /// 初始化对象
        /// </summary>
        /// <param name="bs">图片数据</param>
        public XImageValue(byte[] bs)
        {
            this.ImageData = bs;
        }

        /// <summary>
        /// 对象是否包含数据
        /// </summary>
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public bool HasContent
        {
            get
            {
                if (this.myValue != null)
                {
                    return true;
                }
                if (this.bsImage != null && this.bsImage.Length > 0)
                {
                    return true;
                }
                return false;
            }
        }

        /// <summary>
        /// 将图片转换为JPEG图片数据数组
        /// </summary>
        /// <returns>转换后的数组</returns>
        public byte[] ToJpegBytes()
        {
            return this.ImageData;
            
        }
        private static readonly string _Header_bmp = "data:image/bmp;base64,";
        private static readonly string _Header_png = "data:image/png;base64,";
        private static readonly string _Header_gif = "data:image/gif;base64,";
        private static readonly string _Header_jpeg = "data:image/jpeg;base64,";
        /// <summary>
        /// 解析DataUrl，获得其中的二进制数据
        /// </summary>
        /// <param name="dataUrl">URL字符串</param>
        /// <returns>二进制数据</returns>
        public static byte[] ParseEmitImageSource( string dataUrl )
        {
            if(string.IsNullOrEmpty(dataUrl) )
            {
                return null;
            }
            string strData = null;
            if (dataUrl.StartsWith(_Header_png, StringComparison.Ordinal))
            {
                strData = dataUrl.Substring(_Header_png.Length);
            }
            else  if (dataUrl.StartsWith(_Header_jpeg, StringComparison.Ordinal))
            {
                strData = dataUrl.Substring(_Header_jpeg.Length);
            }
            else if (dataUrl.StartsWith(_Header_bmp, StringComparison.Ordinal))
            {
                strData = dataUrl.Substring(_Header_bmp.Length);
            }
            else if (dataUrl.StartsWith(_Header_gif, StringComparison.Ordinal))
            {
                strData = dataUrl.Substring(_Header_gif.Length);
            }
            if( strData != null )
            {
                return Convert.FromBase64String(strData);
            }
            return null;
        }

        public static string StaticGetEmitImageSourceHeader(byte[] bs)
        {
            if (bs != null && bs.Length > 0)
            {
                if (FileHeaderHelper.HasBMPHeader(bs))
                {
                    return _Header_bmp;
                }
                else if (FileHeaderHelper.HasPNGHeader(bs))
                {
                    return _Header_png;
                }
                else if (FileHeaderHelper.HasGIFHeader(bs))
                {
                    return _Header_gif;
                }
                else if (FileHeaderHelper.HasJpegHeader(bs))
                {
                    return _Header_jpeg;
                }
                else
                {
                    return _Header_jpeg;
                }
            }
            return null;
        }

        public static string StaticGetEmitImageSource(byte[] bs )
        {
            if (bs != null)
            {
                if (FileHeaderHelper.HasBMPHeader(bs))
                {
                    return _Header_bmp + Convert.ToBase64String(bs);
                }
                else if (FileHeaderHelper.HasPNGHeader(bs))
                {
                    return _Header_png + Convert.ToBase64String(bs);
                }
                else if (FileHeaderHelper.HasGIFHeader(bs))
                {
                    return _Header_gif + Convert.ToBase64String(bs);
                }
                else if (FileHeaderHelper.HasJpegHeader(bs))
                {
                    return _Header_jpeg + Convert.ToBase64String(bs);
                }
                else
                {
                    return _Header_jpeg + Convert.ToBase64String(bs);
                }
            }
            return null;
        }

        private  Image myValue;
        /// <summary>
        /// 显示的图片对象
        /// </summary>
        public virtual  Image Value
        {
            get
            {
                //if (myValue != null)
                //{
                //    if( _BufferVersion != _ImageDataList.Version )
                //    {
                //        // 全局缓存区发生改变，检查图片对象
                //        _BufferVersion = _ImageDataList.Version;
                //        if( IsDisposed( myValue ))
                //        {
                //            myValue = null;
                //        }
                //    }
                //}
                
                if (myValue == null)
                {
                    CheckImageData();
                    //if( bsImage != null )
                    //{
                    //    byte[] bs = this.bsImage ;
                    //    this.ImageData = bs ;
                    //}
                }
                return myValue;
            }
            set
            {
                this._ByteSize = 0;
                myValue = value;
                bsImage = null;
            }
        }

        private byte[] bsImage;

        /// <summary>
        /// 保存图形数据的二进制数据
        /// </summary>
        public byte[] ImageData
        {
            get
            {
                if (bsImage == null && myValue is Bitmap)
                {
                    return ((Bitmap)myValue).Data;
                }
                return bsImage;
            }
            set
            {
                this._ByteSize = 0;
                if (myValue != null)
                {
                    myValue.Dispose();
                }
                bsImage = value;
                myValue = null;
                this._SizeFromBinary = GetImageSizeFromBinary(bsImage);
            }
        }

        public byte[] GetImageDataRaw()
        {
            return this.bsImage;
        }

        /// <summary>
        /// 是否为支持的图片文件格式
        /// </summary>
        /// <param name="bs"></param>
        /// <returns></returns>
        public static bool IsSupportedImageFormat(byte[] bs)
        {
            if (bs == null || bs.Length < 5)
            {
                return false;
            }
            return FileHeaderHelper.HasGIFHeader(bs) || FileHeaderHelper.HasPNGHeader(bs)
                || FileHeaderHelper.HasJpegHeader(bs) || FileHeaderHelper.HasBMPHeader(bs);

        }
        private void CheckImageData()
        {
            if (this.bsImage != null && this.bsImage.Length > 0 && myValue == null)
            {
                if (IsSupportedImageFormat(this.bsImage) == false)
                {
                    //throw new NotSupportedException("只支持GIF,PNG,JPG,BMP");
                }
                this.myValue = new  Bitmap(this.bsImage);
            }
        }

        private static object FieldNativeImage = null;
        /// <summary>
        /// 判断图片是否被销毁掉了。
        /// </summary>
        /// <param name="img"></param>
        /// <returns></returns>
        public static bool IsDisposed( Image img)
        {
            if (img == null)
            {
                throw new ArgumentNullException("img");
            }
            if (FieldNativeImage == null)
            {
                FieldNativeImage = typeof( Image).GetField(
                    "nativeImage",
                    BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                if (FieldNativeImage == null)
                {
                    FieldNativeImage = new object();
                }
            }
            if (FieldNativeImage is FieldInfo)
            {
                IntPtr p = (IntPtr)((FieldInfo)FieldNativeImage).GetValue(img);
                if (p == IntPtr.Zero)
                {
                    // 图片句柄为空，说明已经被销毁掉了。
                    return true;
                }
            }
            return false;
        }
        private bool _SafeLoadMode = true;
        /// <summary>
        /// 安全模式
        /// </summary>
        public bool SafeLoadMode
        {
            get
            {
                return _SafeLoadMode; 
            }
            set
            {
                _SafeLoadMode = value; 
            }
        }

        private bool _FormatBase64String;
        /// <summary>
        /// 是否格式化输出Base64字符串
        /// </summary>
        public bool FormatBase64String
        {
            get
            {
                return _FormatBase64String; 
            }
            set
            {
                _FormatBase64String = value; 
            }
        }

        /// <summary>
        /// 包含图片数据的Base64字符串
        /// </summary>
        public string ImageDataBase64String
        {
            get
            {
                byte[] bs = this.ImageData;
                if (bs != null && bs.Length > 0)
                {
                    string txt = Convert.ToBase64String(bs);
                    if (this.FormatBase64String)
                    {
                        txt = DCSoft.Common.StringFormatHelper.GroupFormatString(txt, 8, 16);
                    }
                    return txt;
                }
                else
                {
                    return null;
                }
            }
            set
            {
                this._ByteSize = 0;
                if (value != null && value.Length > 0)
                {
                    try
                    {
                        this.ImageData = Convert.FromBase64String(value );

                    }
                    catch (FormatException e)
                    {
                        // 数据格式错误，试图修复
                        var bs = DCSoft.Common.StringCommon.TryConvertFromBase64String(value);
                        this.ImageData = bs;
                    }
                    catch( System.Exception e3)
                    {
                        DCConsole.Default.WriteLine(e3.ToString());
                        this.ImageData = null;
                    }
                }
            }
        }


		/// <summary>
		/// 图片宽度
		/// </summary>
        public int Width
		{
			get
			{
                return this.Size.Width;
			}
		}
		/// <summary>
		/// 图片高度
		/// </summary>
        public int Height
		{
			get
			{
                return this.Size.Height;
			}
		}

        private DCSystem_Drawing.Size _SizeFromBinary = DCSystem_Drawing.Size.Empty;

		/// <summary>
		/// 图片大小
		/// </summary>
        public DCSystem_Drawing.Size Size
		{
			get
			{
                if(this.myValue != null )
                {
                    return this.myValue.Size;
                }
                if( this._SizeFromBinary.Width > 0 )
                {
                    return this._SizeFromBinary;
                }
                if (this.Value == null)
                {
                    return DCSystem_Drawing.Size.Empty;
                }
                else
                {
                    return this.Value.Size;
                }
			}
		}

		/// <summary>
		/// 复制对象
		/// </summary>
		/// <returns>复制后的对象</returns>
        public XImageValue Clone()
		{
            XImageValue obj = ( XImageValue ) this.MemberwiseClone();
            obj.myValue = null; ;
            obj.bsImage = null;
            if( bsImage != null )
			{
				obj.bsImage = new byte[ bsImage.Length ];
				Array.Copy( bsImage , 0 , obj.bsImage , 0 , bsImage.Length );
			}
            else if (myValue != null)
            {
                obj.myValue = ( Image)myValue.Clone();
            }
			return obj ;
		}

        /// <summary>
        /// 从Base64字符串加载图片数据
        /// </summary>
        /// <param name="base64">BASE64字符串</param>
        /// <returns>加载的字节数</returns>
        public int LoadBase64String(string base64)
        {
            this._ByteSize = 0;
            if (string.IsNullOrEmpty(base64) )// string.IsNullOrEmpty(base64))
            {
                this.bsImage = null;
                this.myValue = null;
                return 0;
            }
            else
            {
                byte[] bs = Convert.FromBase64String(base64);
                this.ImageData = bs;
                return bs.Length;
            }
        }

        [NonSerialized]
        private int _ByteSize;
        /// <summary>
        /// 图片内容的字节长度。可能不精确。
        /// </summary>
        public int ByteSize
        {
            get
            {
                if( _ByteSize == 0 )
                {
                    if( this.bsImage != null && this.bsImage .Length > 0  )
                    {
                        return this.bsImage.Length;
                    }
                }
                if( this.myValue != null && IsDisposed( this.myValue ) == false )
                {
                    int v = this.myValue.Width * this.myValue.Height ;
                    v = v / 2;
                    return v;
                }
                return _ByteSize;
            }
        }
        /// <summary>
        /// 销毁对象
        /// </summary>
        public void Dispose()
        {
            if (myValue != null)
            {
                myValue.Dispose();
                myValue = null;
            }
            bsImage = null;
        }
#if !RELEASE
        /// <summary>
        /// 返回表示对象内容的字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
             Image img = this.Value;
            byte[] bs = this.ImageData;
            if (bsImage == null || myValue == null)
            {
                return "None";
            }
            if (bsImage.Length < 1024)
            {
                return bsImage.Length + "B " + img.Width + "*" + img.Height;
            }
            else
            {
                return Convert.ToInt32(bsImage.Length / 1024) + "KB " + img.Width + "*" + img.Height;
            }
        }
#endif
        public static DCSystem_Drawing.Size GetImageSizeFromBinary(byte[] bs)
        {
            if (bs == null)
            {
                return DCSystem_Drawing.Size.Empty;// throw new ArgumentNullException("bs");
            }
            if (FileHeaderHelper.HasBMPHeader(bs))
            {
                var width = BitConverter.ToInt32(bs, 0x12);
                var height = BitConverter.ToInt32(bs, 0x16);
                return new DCSystem_Drawing.Size(width, height);
            }
            else if (FileHeaderHelper.HasPNGHeader(bs))
            {
                var width = BytesToInt32(bs, 16);
                var height = BytesToInt32(bs, 20);
                return new DCSystem_Drawing.Size(width, height);
            }
            else if (FileHeaderHelper.HasJpegHeader(bs))
            {
                return GetJpgSize(bs);
            }
            else if (FileHeaderHelper.HasGIFHeader(bs))
            {
                byte[] buffer = { bs[6], bs[7], bs[8], bs[9] };
                var width = BitConverter.ToInt16(buffer, 0);
                var height = BitConverter.ToInt16(buffer, 2);
                return new DCSystem_Drawing.Size(width, height);
            }
            return DCSystem_Drawing.Size.Empty;
        }

        private static int BytesToInt32( byte[] bs , int startIndex )
        {
            int result = bs[startIndex];
            result =( result << 8  ) + bs[startIndex + 1];
            result = (result << 8) + bs[startIndex + 2];
            result = (result << 8) + bs[startIndex + 3];
            return result;
        }
        /// <summary>
        /// 获取图片宽高和高度
        /// </summary>
        /// <param name="bs"></param>
        /// <returns></returns>
        private static DCSystem_Drawing.Size GetJpgSize(byte[] bs)
        {
            var JpgSize = new DCSystem_Drawing.Size(0, 0);
            if (!(bs[0] == 0xFF) && (bs[1] == 0xD8))
            {//不是jpg
                return JpgSize;
            }
            //段类型
            int mytype = -1;
            int mytype2 = -1;
            //记录当前读取位置
            long myps = 1;

            do
            {
                do
                {
                    //每个新段的开始标识为0xff，查找下一个新段
                    myps = myps + 1;
                    mytype2 = bs[myps];

                    if (mytype2 < 0)//文件结束
                    {
                        return JpgSize;
                    }
                } while (mytype2 != 0xff);
                do
                {
                    //段与段之间有一个或多个0xff间隔，跳过这些0xff之后的字节为段标识
                    myps = myps + 1;
                    mytype = bs[myps];
                } while (mytype == 0xff);
                //long s1 = myps;//调试使用
                switch (mytype)
                {
                    case 0x00:
                    case 0x01:
                    case 0xD0:
                    case 0xD1:
                    case 0xD2:
                    case 0xD3:
                    case 0xD4:
                    case 0xD5:
                    case 0xD6:
                    case 0xD7:
                        break;
                    case 0xc0://普通JPG的SOFO段
                    case 0xc2://JFIF格式的SOFO段
                        {
                            myps = myps + 3;//跳过：数据长度2个字节、精度1个字节
                            int height = bs[myps + 1] * 256;
                            height += bs[myps + 2];
                            int width = bs[myps + 3] * 256;
                            width += bs[myps + 4];
                            JpgSize.Width = width;
                            JpgSize.Height = height;
                            return JpgSize;
                        }
                    default:
                        int a1 = bs[myps + 1];//下一个
                        int ps1 = a1 * 256;
                        long p2 = myps + 1;
                        int a2 = bs[myps + 1 + 1];//再下一个
                        myps = p2 + ps1 + a2 - 2;
                        break;
                }
                if (myps + 1 >= bs.Length)//文件结束
                {
                    return JpgSize;
                }
            } while (mytype != 0xda);//扫描结束
            return JpgSize;
        }
    }//public class XImageValue : System.ICloneable , System.IDisposable

}