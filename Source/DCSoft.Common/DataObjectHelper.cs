using System;
using System.Windows.Forms;
// // 
using System.IO;

namespace DCSoft.Common
{
	/// <summary>
	/// 数据对象的帮助模块
	/// </summary>
    /// <remarks>编制 袁永福</remarks>
    public sealed class DataObjectHelper
	{
		/// <summary>
		/// 初始化对象
		/// </summary>
		/// <param name="obj">数据对象</param>
		public DataObjectHelper( System.Windows.Forms.IDataObject obj )
		{
			myObject = obj ;
		}

        private readonly System.Windows.Forms.IDataObject myObject = null;
		/// <summary>
		/// 判断是否有图片数据
		/// </summary>
        public bool HasImage
		{
			get
			{
                if (myObject == null)
                {
                    return false;
                }
                if (myObject.GetDataPresent(System.Windows.Forms.DataFormats.Bitmap))
                {
                    return true;
                }
                //if (myObject.GetDataPresent(DataFormats.MetafilePict))
                //{
                //    return true;
                //}
                //if (myObject.GetDataPresent(DataFormats.Tiff))
                //{
                //    return true;
                //}
                if (myObject.GetDataPresent("PNG"))
                {
                    return true;
                }
                if (myObject.GetDataPresent("GIF"))
                {
                    return true;
                }
                return false;
			}
		}

		/// <summary>
		/// 设置/返回位图数据
		/// </summary>
        public Image Image
		{
			get
			{
                if (myObject == null)
                {
                    return null;
                }
                object objValue = null;
                if( myObject.GetDataPresent( DataFormats.Bitmap ))
                {
                    objValue = myObject.GetData ( DataFormats.Bitmap );    
                }
                //else if (myObject.GetDataPresent(DataFormats.MetafilePict))
                //{
                //    objValue = myObject.GetData(DataFormats.MetafilePict);
                //}
                //else if (myObject.GetDataPresent(DataFormats.Tiff))
                //{
                //    objValue = myObject.GetData(DataFormats.Tiff);
                //}
                else if (myObject.GetDataPresent("PNG"))
                {
                    objValue = myObject.GetData("PNG");
                }
                else if (myObject.GetDataPresent("GIF"))
                {
                    objValue = myObject.GetData("GIF");
                }
                if (objValue is Stream)
                {
                    Image img = Image.FromStream((Stream)objValue);
                    return img ;
                }
                else if (objValue is Image)
                {
                    return (Image)objValue;
                }
                return null;
			}
			set
			{
				if( myObject != null )
				myObject.SetData( System.Windows.Forms.DataFormats.Bitmap , value ); 
			}
		}

		/// <summary>
		/// 判断是否有纯文本数据
		/// </summary>
        public bool HasText
		{
			get
			{
                if (myObject == null)
                {
                    return false;
                }
				return myObject.GetDataPresent( System.Windows.Forms.DataFormats.UnicodeText )
                    || myObject.GetDataPresent(System.Windows.Forms.DataFormats.Text); 
			}
		}
		/// <summary>
		/// 设置/返回纯文本数据
		/// </summary>
        public string Text
		{
			get
			{
				if( myObject == null )
					return null;
                var txt = myObject.GetData(System.Windows.Forms.DataFormats.UnicodeText) as string;
                if(txt == null )
                {
                    txt = myObject.GetData(System.Windows.Forms.DataFormats.Text) as string;
                }
                return txt;
			}
			set
			{
				if( myObject == null )
					myObject.SetData( System.Windows.Forms.DataFormats.UnicodeText  , value ); 
			}
		}
	}//public sealed class DataObjectHelper
}
