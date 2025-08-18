using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using DCSoft.Writer.Dom;
using DCSoft.Common;
using DCSoft.Writer.Controls;

namespace DCSoft.Writer.Serialization.Xml
{
    /// <summary>
    /// 标准的XML内容编码器
    /// </summary> 
    public class XMLContentSerializer : ContentSerializer
    {
        public static bool IsTransparentEncrypt(string xml)
        {
            if (xml != null && xml.Length > 0)
            {

            }
            return false;
        }

        private readonly static XMLContentSerializer _Instance = new XMLContentSerializer();
        /// <summary>
        /// 对象唯一静态实例
        /// </summary>
        public static XMLContentSerializer Instance
        {
            get { return _Instance; }
        }

        public override int Priority
        {
            get
            {
                return -1;
            }
        }

        public XMLContentSerializer()
        {
            base.ContentEncoding = null;
        }

        public override bool NeedRefreshPages(DomDocument document)
        {
            return false;
        }
        private static readonly string _XML = "XML";

        public override string Name
        {
            get
            {
                return _XML;
            }
        }

        private static readonly string _FE_Xml = ".xml";

        public override string FileExtension
        {
            get
            {
                return _FE_Xml;
            }
        }

        public override string FileFilter
        {
            get
            {
                return DCSR.XMLFilter;
            }
        }

        public override ContentSerializerFlags Flags
        {
            get
            {
                return ContentSerializerFlags.SupportDeserialize
                    | ContentSerializerFlags.SupportSerialize
                    | ContentSerializerFlags.SupportDeserializeText
                    | ContentSerializerFlags.SupportSerializeText;
            }
        }

        public override bool Deserialize(
            TextReader reader,
            DomDocument document,
            ContentSerializeOptions options)
        {
            DCSystemXml.XmlReader runtimeReader = null;
            if (runtimeReader == null)
            {
                var strXml = reader.ReadToEnd();
                var xr = WASMEnvironment.JSProvider?.CreateXmlReaderByXmlText(strXml);
                runtimeReader = xr;// new MyXmlReaderPackage(xr);
            }
            if (runtimeReader == null)
            {
                throw new InvalidOperationException("创建XML读取器错误");
            }
            return this.Deserialize(runtimeReader, document, options);
        }


        public delegate DCSoft.Writer.Dom.DomDocument DCReadXTextDocumentHandler(
            DCSystemXml.XmlReader reader,
            DCSoft.Writer.Dom.DomDocument instance);

        public static DCReadXTextDocumentHandler ReadXTextDocumentHandler = null;


        public bool Deserialize(
            DCSystemXml.XmlReader reader,
            DomDocument document,
            ContentSerializeOptions options)
        {
            //var startTime44 = DateTime.Now;

            //WriterToolsBase.CheckProductStarter();
            //var handler = EventLoadXMLDocumentCallbackOnce;
            //EventLoadXMLDocumentCallbackOnce = null;
            //float tick = CountDown.GetTickCountFloat();
            //XmlSerializer ser = MyXmlSerializeHelper.GetDocumentXmlSerializer(document.GetType());
            DCSystemXml.XmlReader runtimeReader = reader;

            DomDocument doc2 = null;
            //DCSystemXml.Serialization.XmlDeserializationEvents events = new XmlDeserializationEvents();
            //events.OnUnknownAttribute = new XmlAttributeEventHandler(ser_UnknownAttribute);
            //events.OnUnknownElement = new XmlElementEventHandler(ser_UnknownElement);
            //events.OnUnknownNode = new XmlNodeEventHandler(ser_UnknownNode);
            //events.OnUnreferencedObject = new UnreferencedObjectEventHandler(ser_UnreferencedObject);
            //System.Threading.Thread.Sleep(2000);
            if (DocumentBehaviorOptions.GlobalSpecifyDebugMode)
            {
                // 特殊调试模式
                try
                {
                    DomElementList.InnerDeserializing = true;
                    doc2 = ReadXTextDocumentHandler(runtimeReader, null);
                    AfterLoad(doc2);
                }
                catch (Exception ext)
                {
                    DCConsole.Default.WriteLineError(ext.ToString());
                    throw ext;
                }
                finally
                {
                    DomElementList.InnerDeserializing = false;
                }
            }
            else
            {
                try
                {
                    DomElementList.InnerDeserializing = true;
                    doc2 = ReadXTextDocumentHandler(runtimeReader, null);
                    AfterLoad(doc2);

                }
                finally
                {
                    DomElementList.InnerDeserializing = false;
                }
            }
            if (doc2 == null)
            {
                return false;
            }
            if (doc2.Info.FieldBorderElementWidth == 0.5f)
            {
                doc2.Info.FieldBorderElementWidth = 1;
            }
            document.CopyContent(doc2, true, true);

            doc2.Dispose();
            return true;
        }

        private void BeforeSave(DomDocument document)
        {
        }
        private void AfterLoad(DomDocument document)
        {
            var body = document._Elements?.GetFirstElement<DomDocumentBodyElement>();
            if (body != null
                && body._Elements != null
                && body._Elements.Count > 0
                && body._Elements.Count < 100)
            {
                var list = body._Elements;
                for (int iCount = list.Count - 1; iCount >= 0; iCount--)
                {
                    if (list[iCount] is DomDocument)
                    {
                        // 修复一种文档结构错误，将其转换为子文档节。
                        var doc3 = (DomDocument)list[iCount];
                        list.RemoveAt(iCount);
                        var es5 = doc3.Body.Elements;
                        list.InsertRangeRaw(iCount, es5);
                        es5.Clear();
                    }
                }
            }
        }

      


        public override bool Deserialize(
            System.IO.Stream stream,
            Dom.DomDocument document,
            ContentSerializeOptions options)
        {
                StreamReader reader = new StreamReader(
                    stream,
                    Encoding.UTF8,
                    true);
                string txt = reader.ReadToEnd();
                var reader2 = new StringReader(txt);
                return Deserialize(reader2, document, options);
        }

        public override bool Serialize(
            Stream stream,
            DomDocument document,
            ContentSerializeOptions options)
        {
            StreamWriter writer = new StreamWriter(stream, Encoding.UTF8);
            bool v = Serialize(writer, document, options);
            return v;
        }

        public override bool Serialize(
            TextWriter writer,
            DomDocument document,
            ContentSerializeOptions options)
        {
            var ser = new DCSoft.Writer.NewSerializationNoStringEncrypt.XTextDocumentSerializer();
            bool compressXml = document.Options.BehaviorOptions.CompressXMLContent;
            DCSystemXml.XmlTextWriter xmlWriter = null;
            System.IO.StringWriter baseWriter = null;
            if (compressXml)
            {
                baseWriter = new StringWriter();
                xmlWriter = new DCSystemXml.XmlTextWriter(baseWriter);
                xmlWriter.Formatting = DCSystemXml.Formatting.None;
            }
            else
            {
                xmlWriter = new DCSystemXml.XmlTextWriter(writer);
                if (options.Formated)
                {
                    xmlWriter.Formatting = DCSystemXml.Formatting.Indented;
                    xmlWriter.Indentation = 3;
                    xmlWriter.IndentChar = ' ';
                }
                else
                {
                    xmlWriter.Formatting = DCSystemXml.Formatting.None;
                }
            }
            DomDocument._DocumentSerializing = true;
            if (DocumentBehaviorOptions.GlobalSpecifyDebugMode)
            {
                // 特殊调试模式
                try
                {
                    BeforeSave(document);
                    ser.Serialize(xmlWriter, document);
                }
                catch (Exception ext)
                {
                    DCConsole.Default.WriteLineError(ext.ToString());
                    throw ext;
                }
                finally
                {
                    DomDocument._DocumentSerializing = false;
                }
            }
            else
            {
                try
                {
                    BeforeSave(document);
                    ser.Serialize(xmlWriter, document);
                }
                finally
                {
                    DomDocument._DocumentSerializing = false;
                }
            }
            writer.Flush();
            return true;
        }
    }
}