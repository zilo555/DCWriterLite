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
using System.Reflection.PortableExecutable;

namespace DCSoft.Writer.Controls
{
    /// <summary>
    /// 加载和保存文件相关的操作
    /// </summary>
    public partial class WriterViewControl
    {
        /// <summary>
        /// 清空一些状态数据
        /// </summary>
        public void ClearState()
        {
            this._MouseCaptureTransform = null;
            this._SelectionAreaRectangles = null;
            this.StyleInfoForFormatBrush = null;
            if (this._ToolTips != null)
            {
                this._ToolTips.Clear();
            }
            base.CurrentTransformItem = null;
            this._DragableElement = null;
        }

        /// <summary>
        /// 以指定的格式加载文本文档内容
        /// </summary>
        /// <param name="text">文本</param>
        /// <param name="format">格式</param>
        /// <returns>操作是否成功</returns>
        public bool LoadDocumentFromString(string text, string format)
        {
            this._OldPageSizeList = null;
            if (string.IsNullOrEmpty(text))
            {
                throw new ArgumentNullException("text");
            }
            text = text.Trim();
            if (text.Length == 0)
            {
                return false;
            }
            if (text != null && (format == null || string.Compare(format, "xml", StringComparison.OrdinalIgnoreCase) == 0))
            {
                int index = text.IndexOf('<');
                if (index > 0)
                {
                    // 修正XML字符串
                    text = text.Substring(index);
                }
            }
            System.IO.StringReader reader = new System.IO.StringReader(text);
            return this.LoadDocument(reader, format);
        }

        /// <summary>
        /// 以指定的格式从BASE64字符串加载文档内容
        /// </summary>
        /// <param name="text">BASE64字符串</param>
        /// <param name="format">文件格式</param>
        /// <returns>操作是否成功</returns>
        internal bool LoadDocumentFromBase64String(string text, string format)
        {
            byte[] bs = Convert.FromBase64String(text);
            return LoadDocumentFromBinary(bs, format);
        }

        /// <summary>
        /// 从指定的字节数组中加载文档
        /// </summary>
        /// <param name="bs">字节数组</param>
        /// <param name="format">文件格式</param>
        /// <returns>操作是否成功</returns>
        internal bool LoadDocumentFromBinary(byte[] bs, string format)
        {
            System.IO.MemoryStream ms = new System.IO.MemoryStream(bs);
            return LoadDocument(ms, format);
        }
         
        /// <summary>
        /// 从指定的文件流中加载文档
        /// </summary>
        /// <param name="stream">文件流对象</param>
        /// <param name="format">文件格式</param>
        /// <returns>是否成功加载文档</returns>
        public bool LoadDocument(
            System.IO.Stream stream,
            string format)
        {
            this._OldPageSizeList = null;
            if (stream == null)
            {
                throw new ArgumentNullException("stream");
            }
            this.OwnerWriterControl.ClearDocumentState();
            {
                var document = this.Document;
                try
                {
                    document.BeginLoadDocument();
                    ClearState();
                    string fnBack = document.FileName;
                    try
                    {
                        document.EnableAfterLoad = false;
                        document.Load(stream, format);
                    }
                    finally
                    {
                        document.EnableAfterLoad = true;
                    }
                    this.OwnerWriterControl.ClearDocumentState();
                    document.FileName = fnBack;
                    this.RefreshDocument();
                    document.Modified = false;
                    this.Invalidate();
                    this.OwnerWriterControl.OnDocumentLoad(new WriterEventArgs(this.OwnerWriterControl, document, null));
                }
                finally
                {
                    document.EndLoadDocument();
                }
             }
            this._WASMInvalidateAll = false;
            return true;
        }

        public bool LoadDocumentFromDocument(DomDocument document)
        {
            if (document == null)
            {
                throw new ArgumentNullException("document");
            }
            this._OldPageSizeList = null;
            this.OwnerWriterControl.ClearDocumentState();
            var opts = this.DocumentOptions;
            document.EditorControl = this.OwnerWriterControl;
            document.Options = opts;
            document.FixDomState();
            try
            {
                document.BeginLoadDocument();
                ClearState();
                document.AfterLoad(new ElementLoadEventArgs(document, null));
                this.OwnerWriterControl.ClearDocumentState();
                this.RefreshDocument();
                document.Modified = false;
                this.Invalidate();
            }
            finally
            {
                document.EndLoadDocument();
            }
            this.OwnerWriterControl.OnDocumentLoad(new WriterEventArgs(this.OwnerWriterControl, document, null));
            return true;
        }
        /// <summary>
        /// 从指定的文件地址中加载文档
        /// </summary>
        /// <param name="reader">文件地址</param>
        /// <param name="format">文件格式</param>
        /// <returns>是否成功加载文档</returns>
        public bool LoadDocument(
            System.IO.TextReader reader,
            string format)
        {
            this._OldPageSizeList = null;
            if (reader == null)
            {
                throw new ArgumentNullException("reader");
            }
            this.OwnerWriterControl.ClearDocumentState();
            {
                var document = this.Document;
                try
                {
                    RuningLoadDocumentFromXmlReader = true;
                    document.BeginLoadDocument();
                    ClearState();
                    try
                    {
                        document.EnableAfterLoad = false;
                        document.Load(reader, format);
                    }
                    finally
                    {
                        document.EnableAfterLoad = true;
                    }
                    this.OwnerWriterControl.ClearDocumentState();
                    this.RefreshDocument();
                    document.Modified = false;
                    this.Invalidate();
                }
                finally
                {
                    document.EndLoadDocument();
                    RuningLoadDocumentFromXmlReader = false;
                }
                this.OwnerWriterControl.OnDocumentLoad(new WriterEventArgs(this.OwnerWriterControl, document, null));
            }
            return true;
        }

        internal static bool RuningLoadDocumentFromXmlReader = false;

        /// <summary>
        /// 从指定的文件地址中加载文档
        /// </summary>
        /// <param name="reader">文件地址</param>
        /// <param name="format">文件格式</param>
        /// <returns>是否成功加载文档</returns>
        public bool LoadDocumentFromXmlReader(
            DCSystemXml.XmlReader reader ,
            string format)
        {
            this._OldPageSizeList = null;
            if (reader == null)
            {
                throw new ArgumentNullException("reader");
            }
            this.OwnerWriterControl.ClearDocumentState();
            {
                var document = this.Document;
                try
                {
                    RuningLoadDocumentFromXmlReader = true;
                    document.BeginLoadDocument();
                    ClearState();
                    try
                    {
                        document.EnableAfterLoad = false;
                        document.LoadFromXmlReader(reader,format);
                    }
                    finally
                    {
                        document.EnableAfterLoad = true;
                    }
                    this.OwnerWriterControl.ClearDocumentState();
                    this.RefreshDocument();
                    document.Modified = false;
                    this.Invalidate();
                }
                finally
                {
                    RuningLoadDocumentFromXmlReader = false;
                    document.EndLoadDocument();
                }
                this.OwnerWriterControl.OnDocumentLoad(new WriterEventArgs(this.OwnerWriterControl, document, null));
            }
            return true;
        }

        /// <summary>
        /// 保存文档为字符串
        /// </summary>
        /// <param name="format">文件格式</param>
        /// <returns>输出的字符串</returns>
        public string SaveDocumentToString(string format)
        {
            return this.Document.SaveToString(format);
        }
        /// <summary>
        /// 保存文档为BASE64字符串
        /// </summary>
        /// <param name="format">文件格式</param>
        /// <returns>输出的BASE64字符串</returns>
        public string SaveDocumentToBase64String(string format)
        {
            return this.Document.SaveToBase64String(format);
        }

    }
}
