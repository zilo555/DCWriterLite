//using System;
//using System.Collections.Generic;
//using System.Text;
//using DCSystemXml;
//using DCSystemXml.Schema;

//namespace DCSoft.Writer.Serialization.Xml
//{
//    [System.Runtime.InteropServices.ComVisible(false)]
//    internal class MyXmlReaderPackage : XmlReader
//    {
//        public MyXmlReaderPackage( XmlReader r )
//        {
//            this._BaseReader = r;
//            this._NullID = r.NameTable.Add("null");
//            this._NilID = r.NameTable.Add("nil");
//        }

//        private readonly XmlReader _BaseReader = null;
//        public override XmlNodeType NodeType
//        {
//            get
//            {
//                return this._BaseReader.NodeType;
//            }
//        }
//        public override string LocalName
//        {
//            get
//            {
//                return this._BaseReader.LocalName;
//            }
//        }
//        public override string NamespaceURI
//        {
//            get
//            {
//                return this._BaseReader.NamespaceURI;
//            }
//        }
//        public override string Prefix
//        {
//            get
//            {
//                return this._BaseReader.Prefix;
//            }
//        }


//        public override bool HasValue
//        {
//            get
//            {
//                return this._BaseReader.HasValue;
//            }
//        }


//        public override string Value
//        {
//            get
//            {
//                return this._BaseReader.Value;
//            }
//        }


//        public override int Depth
//        {
//            get
//            {
//                return this._BaseReader.Depth;
//            }
//        }


//        public override string BaseURI
//        {
//            get
//            {
//                return this._BaseReader.BaseURI;
//            }
//        }


//        public override bool IsEmptyElement
//        {
//            get
//            {
//                return this._BaseReader.IsEmptyElement;
//            }
//        }


//        public override int AttributeCount
//        {
//            get
//            {
//                return this._BaseReader.AttributeCount;
//            }
//        }


//        public override bool EOF
//        {
//            get
//            {
//                return this._BaseReader.EOF;
//            }
//        }


//        public override ReadState ReadState
//        {
//            get
//            {
//                return this._BaseReader.ReadState;
//            }
//        }


//        public override XmlNameTable NameTable
//        {
//            get
//            {
//                return this._BaseReader.NameTable;
//            }
//        }


//        public override void Close()
//        {
//            this._BaseReader.Close();
//        }

//        public override string GetAttribute(string name)
//        {
//            return this._BaseReader.GetAttribute(name);
//        }
//        private static int _TCount = 0;
//        private static int _NCount = 0;

//        private object _NullID = null;
//        private object _NilID = null;

//        public override string GetAttribute(string name, string namespaceURI)
//        {
//            if(_NilID == (object)name || _NullID == (object ) name )
//            {
//                return null;
//            }
//            //if (name[0] == 'n') // = null 或 nil
//            //{
//            //    //if( name != "null" && name != "nil")
//            //    //{

//            //    //}
//            //    //_NCount++;
//            //    //return null;
//            //    //string v2 = this._BaseReader.GetAttribute(name, namespaceURI);
//            //    //return v2;
//            //}
//            //_TCount++;
//            //else if ( name[0] == 't')// = type
//            //{
//            //    _TCount++;
//            //    string v2 = this._BaseReader.GetAttribute(name, namespaceURI);
//            //    if( v2 == null )
//            //    {

//            //    }
//            //    return v2;
//            //}
//            return this._BaseReader.GetAttribute(name, namespaceURI);
            
//        }

//        public override string GetAttribute(int i)
//        {
//            return this._BaseReader.GetAttribute(i);
//        }

//        public override string LookupNamespace(string prefix)
//        {
//            return this._BaseReader.LookupNamespace(prefix);
//        }

//        public override bool MoveToAttribute(string name)
//        {
//            return this._BaseReader.MoveToAttribute(name);
//        }

//        public override bool MoveToAttribute(string name, string ns)
//        {
//            return this._BaseReader.MoveToAttribute(name, ns);
//        }

//        public override bool MoveToElement()
//        {
//            return this._BaseReader.MoveToElement();
//        }

//        public override bool MoveToFirstAttribute()
//        {
//            return this._BaseReader.MoveToFirstAttribute();
//        }

//        public override bool MoveToNextAttribute()
//        {
//            return this._BaseReader.MoveToNextAttribute();
//        }

//        public override bool Read()
//        {
//            return this._BaseReader.Read();
//        }

//        public override bool ReadAttributeValue()
//        {
//            return this._BaseReader.ReadAttributeValue();
//        }

//        public override void ResolveEntity()
//        {
//            this._BaseReader.ResolveEntity();
//        }
//        public override bool CanReadBinaryContent => this._BaseReader.CanReadBinaryContent;

//        public override bool CanReadValueChunk => this._BaseReader.CanReadValueChunk;

//        public override bool CanResolveEntity => this._BaseReader.CanResolveEntity;
//        public override bool HasAttributes => this._BaseReader.HasAttributes;

//        public override bool IsDefault => this._BaseReader.IsDefault;

//        public override bool IsStartElement()
//        {
//            return this._BaseReader.IsStartElement();
//        }
//        public override bool IsStartElement(string localname, string ns)
//        {
//            return this._BaseReader.IsStartElement(localname, ns);
//        }
//        public override bool IsStartElement(string name)
//        {
//            return this._BaseReader.IsStartElement(name);
//        }
//        public override void MoveToAttribute(int i)
//        {
//            this._BaseReader.MoveToAttribute(i);
//        }
//        public override XmlNodeType MoveToContent()
//        {
//            return this._BaseReader.MoveToContent();
//        }
//        public override string Name => this._BaseReader.Name;

//        public override char QuoteChar => this._BaseReader.QuoteChar;

//        public override object ReadContentAs(Type returnType, IXmlNamespaceResolver namespaceResolver)
//        {
//            return this._BaseReader.ReadContentAs(returnType, namespaceResolver);

//        }
//        public override int ReadContentAsBase64(byte[] buffer, int index, int count)
//        {
//            return this._BaseReader.ReadContentAsBase64(buffer, index, count);
//        }
//        public override int ReadContentAsBinHex(byte[] buffer, int index, int count)
//        {
//            return this._BaseReader.ReadContentAsBinHex(buffer, index, count);
//        }
//        public override bool ReadContentAsBoolean()
//        {
//            return this._BaseReader.ReadContentAsBoolean();
//        }
//        public override DateTime ReadContentAsDateTime()
//        {
//            return this._BaseReader.ReadContentAsDateTime();
//        }
//        public override decimal ReadContentAsDecimal()
//        {
//            return this._BaseReader.ReadContentAsDecimal();
//        }
//        public override double ReadContentAsDouble()
//        {
//            return this._BaseReader.ReadContentAsDouble();
//        }
//        public override float ReadContentAsFloat()
//        {
//            return this._BaseReader.ReadContentAsFloat();
//        }
//        public override int ReadContentAsInt()
//        {
//            return this._BaseReader.ReadContentAsInt();
//        }
//        public override long ReadContentAsLong()
//        {
//            return this._BaseReader.ReadContentAsLong();
//        }
//        public override object ReadContentAsObject()
//        {
//            return this._BaseReader.ReadContentAsObject();
//        }
//        public override string ReadContentAsString()
//        {
//            return this._BaseReader.ReadContentAsString();
//        }
//        public override object ReadElementContentAs(Type returnType, IXmlNamespaceResolver namespaceResolver, string localName, string namespaceURI)
//        {
//            return this._BaseReader.ReadElementContentAs(returnType, namespaceResolver, localName, namespaceURI);
//        }
//        public override object ReadElementContentAs(Type returnType, IXmlNamespaceResolver namespaceResolver)
//        {
//            return this._BaseReader.ReadElementContentAs(returnType, namespaceResolver);
//        }
//        public override int ReadElementContentAsBase64(byte[] buffer, int index, int count)
//        {
//            return this._BaseReader.ReadElementContentAsBase64(buffer, index, count);
//        }
//        public override int ReadElementContentAsBinHex(byte[] buffer, int index, int count)
//        {
//            return this._BaseReader.ReadElementContentAsBinHex(buffer, index, count);
//        }
//        public override bool ReadElementContentAsBoolean()
//        {
//            return this._BaseReader.ReadElementContentAsBoolean();
//        }
//        public override bool ReadElementContentAsBoolean(string localName, string namespaceURI)
//        {
//            return this._BaseReader.ReadElementContentAsBoolean(localName, namespaceURI);
//        }
//        public override DateTime ReadElementContentAsDateTime()
//        {
//            return this._BaseReader.ReadElementContentAsDateTime();
//        }
//        public override DateTime ReadElementContentAsDateTime(string localName, string namespaceURI)
//        {
//            return this._BaseReader.ReadElementContentAsDateTime(localName, namespaceURI);
//        }
//        public override decimal ReadElementContentAsDecimal()
//        {
//            return this._BaseReader.ReadElementContentAsDecimal();
//        }
//        public override decimal ReadElementContentAsDecimal(string localName, string namespaceURI)
//        {
//            return this._BaseReader.ReadElementContentAsDecimal(localName, namespaceURI);
//        }
//        public override double ReadElementContentAsDouble()
//        {
//            return this._BaseReader.ReadElementContentAsDouble();
//        }
//        public override double ReadElementContentAsDouble(string localName, string namespaceURI)
//        {
//            return this._BaseReader.ReadElementContentAsDouble(localName, namespaceURI);
//        }
//        public override float ReadElementContentAsFloat()
//        {
//            return this._BaseReader.ReadElementContentAsFloat();
//        }
//        public override float ReadElementContentAsFloat(string localName, string namespaceURI)
//        {
//            return this._BaseReader.ReadElementContentAsFloat(localName, namespaceURI);
//        }
//        public override int ReadElementContentAsInt()
//        {
//            return this._BaseReader.ReadElementContentAsInt();
//        }
//        public override int ReadElementContentAsInt(string localName, string namespaceURI)
//        {
//            return this._BaseReader.ReadElementContentAsInt(localName, namespaceURI);
//        }
//        public override long ReadElementContentAsLong()
//        {
//            return this._BaseReader.ReadElementContentAsLong();
//        }
//        public override long ReadElementContentAsLong(string localName, string namespaceURI)
//        {
//            return this._BaseReader.ReadElementContentAsLong(localName, namespaceURI);
//        }
//        public override object ReadElementContentAsObject()
//        {
//            return this._BaseReader.ReadElementContentAsObject();
//        }
//        public override object ReadElementContentAsObject(string localName, string namespaceURI)
//        {
//            return this._BaseReader.ReadElementContentAsObject(localName, namespaceURI);
//        }
//        public override string ReadElementContentAsString()
//        {
//            return this._BaseReader.ReadElementContentAsString();
//        }
//        public override string ReadElementContentAsString(string localName, string namespaceURI)
//        {
//            return this._BaseReader.ReadElementContentAsString(localName, namespaceURI);
//        }
//        public override string ReadElementString()
//        {
//            return this._BaseReader.ReadElementString();
//        }
//        public override string ReadElementString(string localname, string ns)
//        {
//            return this._BaseReader.ReadElementString(localname, ns);
//        }
//        public override string ReadElementString(string name)
//        {
//            return this._BaseReader.ReadElementString(name);
//        }
//        public override void ReadEndElement()
//        {
//            this._BaseReader.ReadEndElement();
//        }
//        public override string ReadInnerXml()
//        {
//            return this._BaseReader.ReadInnerXml();
//        }
//        public override string ReadOuterXml()
//        {
//            return this._BaseReader.ReadOuterXml();
//        }
//        public override void ReadStartElement()
//        {
//            this._BaseReader.ReadStartElement();
//        }
//        public override void ReadStartElement(string localname, string ns)
//        {
//            this._BaseReader.ReadStartElement(localname, ns);
//        }
//        public override void ReadStartElement(string name)
//        {
//            this._BaseReader.ReadStartElement(name);
//        }
//        public override string ReadString()
//        {
//            return this._BaseReader.ReadString();
//        }
//        public override XmlReader ReadSubtree()
//        {
//            return this._BaseReader.ReadSubtree();
//        }
//        public override bool ReadToDescendant(string localName, string namespaceURI)
//        {
//            return this._BaseReader.ReadToDescendant(localName, namespaceURI);
//        }
//        public override bool ReadToDescendant(string name)
//        {
//            return this._BaseReader.ReadToDescendant(name);
//        }
//        public override bool ReadToFollowing(string localName, string namespaceURI)
//        {
//            return this._BaseReader.ReadToFollowing(localName, namespaceURI);
//        }
//        public override bool ReadToFollowing(string name)
//        {
//            return this._BaseReader.ReadToFollowing(name);
//        }
//        public override bool ReadToNextSibling(string localName, string namespaceURI)
//        {
//            return this._BaseReader.ReadToNextSibling(localName, namespaceURI);
//        }
//        public override bool ReadToNextSibling(string name)
//        {
//            return this._BaseReader.ReadToNextSibling(name);
//        }
//        public override int ReadValueChunk(char[] buffer, int index, int count)
//        {
//            return this._BaseReader.ReadValueChunk(buffer, index, count);
//        }
//        public override IXmlSchemaInfo SchemaInfo
//        {
//            get
//            {
//                return this._BaseReader.SchemaInfo;
//            }
//        }

//        public override XmlReaderSettings Settings
//        {
//            get
//            {
//                return this._BaseReader.Settings;
//            }
//        }

//        public override void Skip()
//        {
//            this._BaseReader.Skip();
//        }
//        public override Type ValueType
//        {
//            get
//            {
//                return this._BaseReader.ValueType;
//            }
//        }

//        public override string XmlLang
//        {
//            get
//            {
//                return this._BaseReader.XmlLang;
//            }
//        }

//        public override XmlSpace XmlSpace
//        {
//            get
//            {
//                return this._BaseReader.XmlSpace;
//            }
//        }
       
//    }
//}
