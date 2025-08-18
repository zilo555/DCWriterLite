// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
using System;


namespace DCSystemXml
{
    // Specifies the state of the XmlWriter.
    public enum WriteState
    {
        // Nothing has been written yet.
        Start,

        // Writing the prolog.
        Prolog,

        // Writing a the start tag for an element.
        Element,

        // Writing an attribute value.
        Attribute,

        // Writing element content.
        Content,

        // XmlWriter is closed; Close has been called.
        Closed,

        // Writer is in error state.
        Error
    };

    // Represents a writer that provides fast non-cached forward-only way of generating XML streams containing XML documents
    // that conform to the W3C Extensible Markup Language (XML) 1.0 specification and the Namespaces in XML specification.
    public abstract partial class XmlWriter : IDisposable
    {
        public abstract void WriteStartDocument();

        public abstract void WriteEndDocument();

        public void WriteStartElement(string localName, string ns)
        {
            WriteStartElement(null, localName, ns);
        }

        public abstract void WriteStartElement(string prefix, string localName, string ns);

        public void WriteStartElement(string localName)
        {
            WriteStartElement(null, localName, null);
        }

        public abstract void WriteEndElement();

        public abstract void WriteFullEndElement();

        public void WriteAttributeString(string localName, string ns, string value)
        {
            WriteStartAttribute(null, localName, ns);
            WriteString(value);
            WriteEndAttribute();
        }
        public void WriteAttributeString(string localName, string ns, int value)
        {
            WriteStartAttribute(null, localName, ns);
            WriteString( DCSystemXml.XmlConvert.ToString( value));
            WriteEndAttribute();
        }
        // Writes out the attribute with the specified LocalName and value.
        public void WriteAttributeString(string localName, string value)
        {
            WriteStartAttribute(null, localName, null);
            WriteString(value);
            WriteEndAttribute();
        }
       
        public abstract void WriteStartAttribute(string prefix, string localName, string ns);


        public abstract void WriteEndAttribute();


        public abstract void WriteString(string text);


        public abstract void WriteRaw(string data);

        public abstract WriteState WriteState { get; }

        public virtual void Close() { }

        public void WriteElementString(string localName, string value)
        {
            WriteElementString(localName, null, value);
        }

        public void WriteElementString(string localName, string ns, string value)
        {
            WriteStartElement(localName, ns);
            if (!string.IsNullOrEmpty(value))
            {
                WriteString(value);
            }

            WriteEndElement();
        }


        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing && WriteState != WriteState.Closed)
            {
                Close();
            }
        }
    }
}
