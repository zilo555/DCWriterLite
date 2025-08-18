using System;
using System.Collections.Generic;
using System.Text;
using System.IO ;
using DCSoft.Writer.Dom ;

namespace DCSoft.Writer.Serialization
{
    /// <summary>
    /// 文档内容编码解码器
    /// </summary>
    /// <remarks>编制 袁永福</remarks>
    public abstract class ContentSerializer
    {
        /// <summary>
        /// 文件格式名称
        /// </summary>
        public virtual string Name
        {
            get
            {
                return null;
            }
        }
        /// <summary>
        /// 判断序列化前是否需要调用PrepareSerialize()
        /// </summary>
        /// <param name="document">文档对象</param>
        /// <returns>是否需要调用</returns>
        public virtual bool NeedPrepareSerialize( DomDocument document )
        {
            return true;
        }
        /// <summary>
        /// 序列化后需要调用AfterSerialize()
        /// </summary>
        /// <param name="document">文档对象</param>
        /// <returns>是否需要调用</returns>
        public virtual bool NeedAfterSerialize( DomDocument document )
        {
            return true;
        }
        /// <summary>
        /// 是否需要刷新文档页面
        /// </summary>
        public virtual bool NeedRefreshPages( DomDocument document )
        {
            return false;
        }

        private int _Priority = 0;
        /// <summary>
        /// 级别
        /// </summary>
        public virtual int Priority
        {
            get
            {
                return this._Priority ;
            }
            set
            {
                this._Priority = value;
            }
        }


        private bool _IsDefault = false;
        /// <summary>
        /// 是否是默认文件编码器
        /// </summary>
        public bool IsDefault
        {
            get
            {
                return _IsDefault;
            }
            set
            {
                _IsDefault = value;
            }
        }

        /// <summary>
        /// 文件扩展名
        /// </summary>
        public virtual string FileExtension
        {
            get
            {
                return null;
            }
        }

        /// <summary>
        /// 文件名过滤器
        /// </summary>
        public virtual string FileFilter
        {
            get
            {
                return DCSR.AllFileFilter;
            }
        }

        /// <summary>
        /// 对象标记
        /// </summary>
        public virtual ContentSerializerFlags Flags
        {
            get
            {
                return ContentSerializerFlags.None;
            }
        }

        private Encoding _ContentEncoding = null ;
        /// <summary>
        /// 文本编码格式
        /// </summary>
        public Encoding ContentEncoding
        {
            get
            {
                return _ContentEncoding;
            }
            set
            {
                _ContentEncoding = value;
            }
        }

        /// <summary>
        /// 反序列化文档内容
        /// </summary>
        /// <param name="stream">文件流对象</param>
        /// <param name="document">文档对象</param>
        /// <param name="options">选项</param>
        /// <returns>操作是否成功</returns>
        public virtual bool Deserialize(
            Stream stream,
            DomDocument document,
            ContentSerializeOptions options)
        {
            return false;
        }

        /// <summary>
        /// 反序列化文档内容
        /// </summary>
        /// <param name="reader">文本读取器</param>
        /// <param name="document">文档对象</param>
        /// <param name="options">选项</param>
        /// <returns>操作是否成功</returns>
        public virtual bool Deserialize(
            TextReader reader,
            DomDocument document,
            ContentSerializeOptions options)
        {
            return false;
        }

        /// <summary>
        /// 序列化文档内容
        /// </summary>
        /// <param name="stream">文件流对象</param>
        /// <param name="document">文档对象</param>
        /// <param name="options">选项</param>
        /// <returns>操作是否成功</returns>
        public virtual bool Serialize(
            Stream stream,
            DomDocument document,
            ContentSerializeOptions options)
        {
            return false;
        }

        /// <summary>
        /// 序列化文档内容
        /// </summary>
        /// <param name="writer">文本书写器</param>
        /// <param name="document">文档对象</param>
        /// <param name="options">选项</param>
        /// <returns>操作是否成功</returns>
        public virtual bool Serialize(
            TextWriter writer,
            DomDocument document,
            ContentSerializeOptions options)
        {
            return false;
        }
    }



    /// <summary>
    /// 文件序列化器标记
    /// </summary>
    [Flags]
    public enum ContentSerializerFlags
    {
        /// <summary>
        /// 无状态
        /// </summary>
        None = 0,
        /// <summary>
        /// 支持序列化
        /// </summary>
        SupportDeserialize = 1,
        /// <summary>
        /// 支持反序列化
        /// </summary>
        SupportSerialize = 2,
        /// <summary>
        /// 支持以文本格式进行反序列化
        /// </summary>
        SupportDeserializeText = 4,
        /// <summary>
        /// 支持以文本格式进行序列化
        /// </summary>
        SupportSerializeText = 8,
        /// <summary>
        /// 支持过程选项控制
        /// </summary>
        SupportOptions = 16
    }
}