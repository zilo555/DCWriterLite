// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text;
using System;

namespace DCSystemXml
{
#if ! RELEASE
    // Represents a reader that provides fast, non-cached forward only stream access to XML data.
    [DebuggerDisplay($"{{{nameof(DebuggerDisplayProxy)}}}")]
#endif
    public abstract partial class XmlReader : IDisposable
    {
        private const uint IsTextualNodeBitmap = 0x6018;


        public abstract XmlNodeType NodeType { get; }

        public virtual string Name => Prefix.Length == 0 ? LocalName : NameTable.Add($"{Prefix}:{LocalName}");

        public abstract string LocalName { get; }

        public abstract string NamespaceURI { get; }

        public abstract string Prefix { get; }

        public abstract string Value { get; }

        public abstract int Depth { get; }

        // Gets the base URI of the current node.
        //public abstract string BaseURI { get; }

        // Gets a value indicating whether the current node is an empty element (for example, <MyElement/>).
        public abstract bool IsEmptyElement { get; }

        //// Gets the value of the attribute with the specified Name
        //public abstract string GetAttribute(string name);

        // Gets the value of the attribute with the LocalName and NamespaceURI
        public abstract string GetAttribute(string name, string namespaceURI);


        public abstract bool MoveToNextAttribute();

        public abstract bool MoveToElement();

        public abstract bool Read();

        public virtual void Close() { }

        public abstract ReadState ReadState { get; }

        public virtual void Skip()
        {
            if (ReadState != ReadState.Interactive)
            {
                return;
            }
            SkipSubtree();
        }

        public abstract XmlNameTable NameTable { get; }


        public abstract void ResolveEntity();

        // Virtual helper methods
        // Reads the contents of an element as a string. Stops of comments, PIs or entity references.
        public virtual string ReadString()
        {
            if (ReadState != ReadState.Interactive)
            {
                return string.Empty;
            }
            MoveToElement();
            if (NodeType == XmlNodeType.Element)
            {
                if (IsEmptyElement)
                {
                    return string.Empty;
                }

                if (!Read())
                {
                    throw new InvalidOperationException();
                }
                if (NodeType == XmlNodeType.EndElement)
                {
                    return string.Empty;
                }
            }
            string result = string.Empty;
            while (IsTextualNode(NodeType))
            {
                result += Value;
                if (!Read())
                {
                    break;
                }
            }
            return result;
        }

        // Checks whether the current node is a content (non-whitespace text, CDATA, Element, EndElement, EntityReference
        // or EndEntity) node. If the node is not a content node, then the method skips ahead to the next content node or
        // end of file. Skips over nodes of type ProcessingInstruction, DocumentType, Comment, Whitespace and SignificantWhitespace.
        public virtual XmlNodeType MoveToContent()
        {
            do
            {
                switch (NodeType)
                {
                    case XmlNodeType.Attribute:
                        MoveToElement();
                        return NodeType;
                    case XmlNodeType.Element:
                    case XmlNodeType.EndElement:
                    case XmlNodeType.CDATA:
                    case XmlNodeType.Text:
                    case XmlNodeType.EntityReference:
                    case XmlNodeType.EndEntity:
                        return NodeType;
                }
            } while (Read());
            return NodeType;
        }

        public virtual void ReadStartElement()
        {
            if (MoveToContent() != XmlNodeType.Element)
            {
                throw new XmlException();
            }
            Read();
        }

        public virtual string ReadElementString()
        {
            string result = string.Empty;

            if (MoveToContent() != XmlNodeType.Element)
            {
                throw new XmlException();
            }
            if (!IsEmptyElement)
            {
                Read();
                result = ReadString();
                if (NodeType != XmlNodeType.EndElement)
                {
                    throw new XmlException();
                }
                Read();
            }
            else
            {
                Read();
            }
            return result;
        }

        public virtual void ReadEndElement()
        {
            if (MoveToContent() != XmlNodeType.EndElement)
            {
                throw new XmlException();
            }
            Read();
        }

        public virtual bool IsStartElement(string localname, string ns)
        {
            return MoveToContent() == XmlNodeType.Element && LocalName == localname && NamespaceURI == ns;
        }

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing && ReadState != ReadState.Closed)
            {
                Close();
            }
        }

        ////
        //// Internal methods
        ////
        //// Validation support
        //internal virtual XmlNamespaceManager? NamespaceManager => null;
        internal static bool IsTextualNode(XmlNodeType nodeType)
        {
#if DEBUG
            // This code verifies IsTextualNodeBitmap mapping of XmlNodeType to a bool specifying
            // whether the node is 'textual' = Text, CDATA, Whitespace or SignificantWhitespace.
           //Debug.Assert(0 == (IsTextualNodeBitmap & (1 << (int)XmlNodeType.None)));
           //Debug.Assert(0 == (IsTextualNodeBitmap & (1 << (int)XmlNodeType.Element)));
           //Debug.Assert(0 == (IsTextualNodeBitmap & (1 << (int)XmlNodeType.Attribute)));
           //Debug.Assert(0 != (IsTextualNodeBitmap & (1 << (int)XmlNodeType.Text)));
           //Debug.Assert(0 != (IsTextualNodeBitmap & (1 << (int)XmlNodeType.CDATA)));
           //Debug.Assert(0 == (IsTextualNodeBitmap & (1 << (int)XmlNodeType.EntityReference)));
           //Debug.Assert(0 == (IsTextualNodeBitmap & (1 << (int)XmlNodeType.Entity)));
           //Debug.Assert(0 == (IsTextualNodeBitmap & (1 << (int)XmlNodeType.ProcessingInstruction)));
           //Debug.Assert(0 == (IsTextualNodeBitmap & (1 << (int)XmlNodeType.Comment)));
           //Debug.Assert(0 == (IsTextualNodeBitmap & (1 << (int)XmlNodeType.Document)));
           //Debug.Assert(0 == (IsTextualNodeBitmap & (1 << (int)XmlNodeType.DocumentType)));
           //Debug.Assert(0 == (IsTextualNodeBitmap & (1 << (int)XmlNodeType.DocumentFragment)));
           //Debug.Assert(0 == (IsTextualNodeBitmap & (1 << (int)XmlNodeType.Notation)));
           //Debug.Assert(0 != (IsTextualNodeBitmap & (1 << (int)XmlNodeType.Whitespace)));
           //Debug.Assert(0 != (IsTextualNodeBitmap & (1 << (int)XmlNodeType.SignificantWhitespace)));
           //Debug.Assert(0 == (IsTextualNodeBitmap & (1 << (int)XmlNodeType.EndElement)));
           //Debug.Assert(0 == (IsTextualNodeBitmap & (1 << (int)XmlNodeType.EndEntity)));
           //Debug.Assert(0 == (IsTextualNodeBitmap & (1 << (int)XmlNodeType.XmlDeclaration)));
#endif
            return 0 != (IsTextualNodeBitmap & (1 << (int)nodeType));
        }
        private bool SkipSubtree()
        {
            MoveToElement();
            if (NodeType == XmlNodeType.Element && !IsEmptyElement)
            {
                int depth = Depth;

                while (Read() && depth < Depth)
                {
                    // Nothing, just read on
                }

                // consume end tag
                if (NodeType == XmlNodeType.EndElement)
                    return Read();
            }
            else
            {
                return Read();
            }

            return false;
        }
#if !RELEASE
        private object DebuggerDisplayProxy => new XmlReaderDebuggerDisplayProxy(this);

        [DebuggerDisplay("{ToString()}")]
        private readonly struct XmlReaderDebuggerDisplayProxy
        {
            private readonly XmlReader _reader;

            internal XmlReaderDebuggerDisplayProxy(XmlReader reader)
            {
                _reader = reader;
            }
            public override string ToString()
            {
                XmlNodeType nt = _reader.NodeType;
                string result = nt.ToString();
                switch (nt)
                {
                    case XmlNodeType.Element:
                    case XmlNodeType.EndElement:
                    case XmlNodeType.EntityReference:
                    case XmlNodeType.EndEntity:
                        result += $", Name=\"{_reader.Name}\"";
                        break;
                    case XmlNodeType.Attribute:
                    case XmlNodeType.ProcessingInstruction:
                        result += $", Name=\"{_reader.Name}\", Value=\"{_reader.Value}\"";
                        break;
                    case XmlNodeType.Text:
                    case XmlNodeType.Whitespace:
                    case XmlNodeType.SignificantWhitespace:
                    case XmlNodeType.Comment:
                    case XmlNodeType.XmlDeclaration:
                    case XmlNodeType.CDATA:
                        result += $", Value=\"{_reader.Value}\"";
                        break;
                    case XmlNodeType.DocumentType:
                        result += $", Name=\"{_reader.Name}'";
                        //result += $", SYSTEM=\"{_reader.GetAttribute("SYSTEM")}\"";
                        //result += $", PUBLIC=\"{_reader.GetAttribute("PUBLIC")}\"";
                        result += $", Value=\"{_reader.Value}\"";
                        break;
                }
                return result;
            }
        }
#endif
    }
}
