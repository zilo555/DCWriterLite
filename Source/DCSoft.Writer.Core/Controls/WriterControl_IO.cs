using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.ComponentModel;
using DCSoft.Writer.Dom;
using System.Runtime.InteropServices;
using DCSoft.Common;
using DCSoft.Data;
using System.Windows.Forms;
using DCSoft.Writer.Serialization;
using DCSoft.Writer.Data;
using DCSoft.Drawing;

namespace DCSoft.Writer.Controls
{
    /// <summary>
    /// 加载和保存文件相关的操作
    /// </summary>
    public partial class WriterControl
    {

        /// <summary>
        /// 检测文件格式
        /// </summary>
        /// <param name="strData">文件内容</param>
        /// <returns>文件格式</returns>
        public string DetectFileFormat(string strData)
        {
            return WriterUtilsInner.DetectFileFormat(strData);
        }
        /// <summary>
        /// 以指定的格式加载文本文档内容
        /// </summary>
        /// <param name="text">文本</param>
        /// <param name="format">格式</param>
        /// <returns>操作是否成功</returns>
        public bool LoadDocumentFromString(string text, string format)
        {
            {
                return this.GetInnerViewControl().LoadDocumentFromString(text, format);
            }
        }

        /// <summary>
        /// 以指定的格式从XML读取器加载文本文档内容
        /// </summary>
        /// <param name="reader">XML读取器</param>
        /// <param name="format">格式</param>
        /// <returns>操作是否成功</returns>
        /// 2023-1-21 年三十
        public bool LoadDocumentFromXmlReader(DCSystemXml.XmlReader reader, string format)
        {
            {
                return this.GetInnerViewControl().LoadDocumentFromXmlReader(reader, format);
            }
        }

        /// <summary>
        /// 以指定的格式从BASE64字符串加载文档内容
        /// </summary>
        /// <param name="text">BASE64字符串</param>
        /// <param name="format">文件格式</param>
        /// <returns>操作是否成功</returns>
        public bool LoadDocumentFromBase64String(string text, string format)
        {
            {
                return this.GetInnerViewControl().LoadDocumentFromBase64String(text, format);
            }
        }


        /// <summary>
        /// 从指定的字节数组中加载文档
        /// </summary>
        /// <param name="bs">字节数组</param>
        /// <param name="format">文件格式</param>
        /// <returns>操作是否成功</returns>
        public bool LoadDocumentFromBinary(byte[] bs, string format)
        {
            {
                return this.GetInnerViewControl().LoadDocumentFromBinary(bs, format);
            }
        }
        /// <summary>
        /// 保存文档为字符串
        /// </summary>
        /// <param name="format">文件格式</param>
        /// <returns>输出的字符串</returns>
        public string SaveDocumentToString(string format)
        {
            string result = this.GetInnerViewControl().SaveDocumentToString(format);
            //if ( result != null )
            //{
            //    this.StopAutoSaveTask();
            //}
            return result;
        }
        /// <summary>
        /// 保存文档为BASE64字符串
        /// </summary>
        /// <param name="format">文件格式</param>
        /// <returns>输出的BASE64字符串</returns>
        public string SaveDocumentToBase64String(string format)
        {
            string result = this.GetInnerViewControl().SaveDocumentToBase64String(format);
            return result;
        }
    }
}
