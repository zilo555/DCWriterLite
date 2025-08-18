using System;
using System.Collections.Generic;
using System.Text;
using DCSystemXml;
using System.Collections;
using DCSoft.WASM;

namespace DCSoft.Writer.NewSerialization
{
    public abstract class DCXmlSerializerBase
    {
        protected abstract void Serialize(object objectToSerialize, DCXmlSerializationWriter writer);

        protected abstract object Deserialize(DCXmlSerializationReader reader);

        public void Serialize(DCSystemXml.XmlWriter writer, object instance )
        {
            var w2 = this.DCCreateWriter();
            w2.Init(writer);
            this.Serialize(instance, w2);
        }

        public object Deserialize(DCSystemXml.XmlReader reader)
        {
            var r2 = this.DCCreateReader();
            r2.Init(reader);
            var result = this.Deserialize(r2);
            r2.Close();
            return result;
        }

        public object Deserialize(System.IO.TextReader reader)
        {
            var strXml = reader.ReadToEnd();
            var reader2 = WriterControlForWASM.StaticCreateXmlReaderByXmlText(strXml);
            return Deserialize( reader2 );
        }



        public virtual System.Boolean CanDeserialize(DCSystemXml.XmlReader xmlReader)
        {
            return false;
        }
        protected virtual DCXmlSerializationReader DCCreateReader()
        {
            return null;
        }
        protected virtual DCXmlSerializationWriter DCCreateWriter()
        {
            return null;
        }
    }
    public class DCXmlSerializationReader //: DCSystemXmlSerialization.XmlSerializationReader
    {
        private DCSystemXml.XmlReader _r = null;
        public void Close()
        {
        }
        public void Init(DCSystemXml.XmlReader r )
        {
            //DCSystemXml.XmlDocument a = null;
            if(r == null )
            {
                throw new ArgumentNullException("r");
            }
            this._r = r;
            _typeID = r.NameTable.Add("type");
            _instanceNs2000ID = r.NameTable.Add("http://www.w3.org/2000/10/XMLSchema-instance");
            _instanceNs1999ID = r.NameTable.Add("http://www.w3.org/1999/XMLSchema-instance");
            _instanceNsID = r.NameTable.Add("http://www.w3.org/2001/XMLSchema-instance");
            _nilID = r.NameTable.Add("nil");
            _nullID = r.NameTable.Add("null");
            this.InitIDs();
        }
        public DCSystemXml.XmlReader Reader
        {
            get
            {
                return this._r;
            }
        }
        
        protected virtual void InitCallbacks()
        {
        }
        protected string _typeID = null;
        protected string _instanceNsID = null;
        protected string _instanceNs2000ID = null;
        protected string _instanceNs1999ID = null;
        protected string _nilID = null;
        protected string _nullID;
        protected virtual void InitIDs()
        {

        }
        public void CheckReaderCount(ref int a , ref int b )
        {

        }
        public System.Exception CreateUnknownNodeException()
        {
            return new System.Exception("UnknowNode:" + this._r.LocalName);
        }
        protected string GetXsiType()
        {
            var strValue = _r.GetAttribute(_typeID, _instanceNsID);
            if (strValue != null)
            {
                return _r.NameTable.Add(strValue);
            }
            return null;
        }
      
        protected static DateTime ToDateTime(string value)
        {
            return XmlConvert.ToDateTime(value);
        }
        protected void ReadEndElement()
        {
            while (_r.NodeType == DCSystemXml.XmlNodeType.Whitespace)
            {
                _r.Skip();
            }
            if (_r.NodeType == DCSystemXml.XmlNodeType.None)
            {
                _r.Skip();
            }
            else
            {
                _r.ReadEndElement();
            }
        }

        protected int ReaderCount { get { return 0; } }

        protected void UnknownNode(object o, string qnames)
        {
            var r = this.Reader;
            if (r.NodeType == DCSystemXml.XmlNodeType.None || r.NodeType == DCSystemXml.XmlNodeType.Whitespace)
            {
                r.Read();
            }
            else
            {
                if (r.NodeType == DCSystemXml.XmlNodeType.EndElement)
                {
                    return;
                }
                else if (r.NodeType != DCSystemXml.XmlNodeType.Attribute )//|| events.OnUnknownAttribute != null)
                {
                    r.Skip();
                }
            }
        }
    }
    public class DCXmlSerializationWriter //: DCSystemXmlSerialization.XmlSerializationWriter
    {
        static DCXmlSerializationWriter()
        {
            var v = System.TimeZone.CurrentTimeZone;
            var sp = v.GetUtcOffset(DateTime.Now);
            _TimeZone = "+" + sp.Hours.ToString("00") + ":" + sp.Minutes.ToString("00");
        }


        private DCSystemXml.XmlWriter _w = null;
        public DCSystemXml.XmlWriter Writer { get { return this._w; } }
        public void Init( DCSystemXml.XmlWriter w )
        {
            if( w == null )
            {
                throw new ArgumentNullException("w");
            }
            this._IsFirstWriteStartElement = true;
            this._w = w;
        }

        private static readonly string _TimeZone;
        public static string FromDateTime(DateTime dtm)
        {
            return dtm.ToString("s") + _TimeZone;
        }

        protected void WriteXsiType(string name, string ns)
        {
            WriteAttribute("type", "http://www.w3.org/2001/XMLSchema-instance", GetQualifiedName(name, ns));
        }
        private string GetQualifiedName(string name, string ns)
        {
            return name;
        }
        protected void WriteAttribute(string localName, string ns, string value)
        {
            if (value == null) return;
            if( ns == XmlReservedNs.NsXsi)
            {
                this._w.WriteAttributeString("xsi:type", value);
            }
            else
            {
                this._w.WriteAttributeString(localName, value);
            }
        }

        protected void WriteStartElement(string name, string ns, object o, bool writePrefixed)
        {
            WriteStartElement(name, ns, o, writePrefixed, null);
        }

        private bool _IsFirstWriteStartElement = false;
        protected void WriteStartElement(string name, string ns, object o, bool writePrefixed, string xmlns)
        {
            _w.WriteStartElement( name, ns);
            if( _IsFirstWriteStartElement )
            {
                _IsFirstWriteStartElement = false;
                _w.WriteAttributeString("xmlns:xsd", XmlReservedNs.NsXs);
                _w.WriteAttributeString("xmlns:xsi", XmlReservedNs.NsXsi);
            }
        }
        protected void WriteNullTagLiteral(string name, string ns)
        {
            if (name == null || name.Length == 0)
                return;
            WriteStartElement(name, ns, null, false);
            _w.WriteAttributeString("nil", "http://www.w3.org/2001/XMLSchema-instance", "true");
            _w.WriteEndElement();
        }
        protected void WriteStartDocument()
        {
            if (_w.WriteState == WriteState.Start)
            {
                _w.WriteStartDocument();
            }
        }
        protected void WriteElementStringRaw(string localName, string ns, byte[] value, object xsiType)
        {
            if (value == null) return;
            _w.WriteStartElement(localName, ns);
            if (value != null && value.Length > 0)
            {
                _w.WriteString(Convert.ToBase64String(value));
            }
            _w.WriteEndElement();
        }

        protected void WriteElementString(string localName, string ns, string value)
        {
            WriteElementString(localName, ns, value, null);
        }

        protected void WriteElementString(string localName, string ns, string value, object xsiType)
        {
            if (value == null) return;
            if (xsiType == null)
                _w.WriteElementString(localName, ns, value);
        }

        internal static string FromEnum(long val, string[] vals, long[] ids, string typeName)
        {
            if (ids.Length != vals.Length) throw new InvalidOperationException("Invalid enum");
            long originalValue = val;
            StringBuilder sb = new StringBuilder();
            int iZero = -1;

            for (int i = 0; i < ids.Length; i++)
            {
                if (ids[i] == 0)
                {
                    iZero = i;
                    continue;
                }
                if (val == 0)
                {
                    break;
                }
                if ((ids[i] & originalValue) == ids[i])
                {
                    if (sb.Length != 0)
                        sb.Append(' ');
                    sb.Append(vals[i]);
                    val &= ~ids[i];
                }
            }
            if (val != 0)
            {
                throw new InvalidOperationException("XmlUnknownConstant:" + originalValue + ( typeName == null ? "enum" : typeName));
            }
            if (sb.Length == 0 && iZero >= 0)
            {
                sb.Append(vals[iZero]);
            }
            return sb.ToString();
        }
        protected Exception CreateUnknownTypeException(object o)
        {
            return CreateUnknownTypeException(o.GetType());
        }
        protected Exception CreateUnknownTypeException(Type type)
        {
            return new InvalidOperationException("XmlUnxpectedType:" +type.FullName);
        }
        protected void TopLevelElement()
        {
        }

        protected void WriteEndElement(object o)
        {
            _w.WriteEndElement();

        }
    }
}
