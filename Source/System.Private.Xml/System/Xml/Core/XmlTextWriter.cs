// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.IO;
using System.Text;
using System.Diagnostics;
using DCSoft.Drawing;

namespace DCSystemXml
{
    // Specifies formatting options for XmlTextWriter.
    public enum Formatting
    {
        // No special formatting is done (this is the default).
        None,

        Indented,
    }


    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public class XmlTextWriter : XmlWriter
    {

        private struct TagInfo
        {
            internal string name;
            internal string xmlLang;
            internal int prevNsTop;
            //internal int prefixCount;
            internal bool mixed; // whether to pretty print the contents of this element.

            internal void Init(int nsTop)
            {
                name = null;
                //defaultNs = string.Empty;
                //defaultNsState = NamespaceState.Uninitialized;
                xmlLang = null;
                prevNsTop = nsTop;
                //prefixCount = 0;
                mixed = false;
            }
        }

        // State machine is working through autocomplete
        private enum State
        {
            Start,
            Prolog,
            PostDTD,
            Element,
            Attribute,
            Content,
            AttrOnly,
            Epilog,
            Error,
            Closed,
        }

        public enum Token
        {
            PI,
            Doctype,
            Comment,
            CData,
            StartElement,
            EndElement,
            LongEndElement,
            StartAttribute,
            EndAttribute,
            Content,
            Base64,
            RawData,
            Whitespace,
            Empty
        }

        //
        // Fields
        //
        // output
        private readonly TextWriter _textWriter = null!;
        public TextWriter BaseTextWriter
        {
            get { return this._textWriter; }
        }
        private readonly XmlTextEncoder _xmlEncoder = null!;
        private readonly Encoding _encoding;

        // formatting
        private Formatting _formatting;
        private bool _indented; // perf - faster to check a boolean.
        private int _indentation;
        private char[] _indentChars;
        private static readonly char[] s_defaultIndentChars = CreateDefaultIndentChars();

        private static char[] CreateDefaultIndentChars()
        {
            var result = new char[IndentArrayLength];
            Array.Fill(result, DefaultIndentChar);
            return result;
        }

        // element stack
        private TagInfo[] _stack;
        private int _top;

        // state machine for AutoComplete
        private State[] _stateTable;
        private State _currentState;
        private Token _lastToken;

        private char _quoteChar;
        private char _curQuoteChar;
        private bool _namespaces;
        //private string? _prefixForXmlNs;
        private bool _flush;

        private int _nsTop;
        private const int IndentArrayLength = 64;
        private const char DefaultIndentChar = ' ';

        private static readonly State[] s_stateTableDefault = {
            //                          State.Start      State.Prolog     State.PostDTD    State.Element    State.Attribute  State.Content   State.AttrOnly   State.Epilog
            //
            /* Token.PI             */ State.Prolog,    State.Prolog,    State.PostDTD,   State.Content,   State.Content,   State.Content,  State.Error,     State.Epilog,
            /* Token.Doctype        */ State.PostDTD,   State.PostDTD,   State.Error,     State.Error,     State.Error,     State.Error,    State.Error,     State.Error,
            /* Token.Comment        */ State.Prolog,    State.Prolog,    State.PostDTD,   State.Content,   State.Content,   State.Content,  State.Error,     State.Epilog,
            /* Token.CData          */ State.Content,   State.Content,   State.Error,     State.Content,   State.Content,   State.Content,  State.Error,     State.Epilog,
            /* Token.StartElement   */ State.Element,   State.Element,   State.Element,   State.Element,   State.Element,   State.Element,  State.Error,     State.Element,
            /* Token.EndElement     */ State.Error,     State.Error,     State.Error,     State.Content,   State.Content,   State.Content,  State.Error,     State.Error,
            /* Token.LongEndElement */ State.Error,     State.Error,     State.Error,     State.Content,   State.Content,   State.Content,  State.Error,     State.Error,
            /* Token.StartAttribute */ State.AttrOnly,  State.Error,     State.Error,     State.Attribute, State.Attribute, State.Error,    State.Error,     State.Error,
            /* Token.EndAttribute   */ State.Error,     State.Error,     State.Error,     State.Error,     State.Element,   State.Error,    State.Epilog,     State.Error,
            /* Token.Content        */ State.Content,   State.Content,   State.Error,     State.Content,   State.Attribute, State.Content,  State.Attribute, State.Epilog,
            /* Token.Base64         */ State.Content,   State.Content,   State.Error,     State.Content,   State.Attribute, State.Content,  State.Attribute, State.Epilog,
            /* Token.RawData        */ State.Prolog,    State.Prolog,    State.PostDTD,   State.Content,   State.Attribute, State.Content,  State.Attribute, State.Epilog,
            /* Token.Whitespace     */ State.Prolog,    State.Prolog,    State.PostDTD,   State.Content,   State.Attribute, State.Content,  State.Attribute, State.Epilog,
        };

        private static readonly State[] s_stateTableDocument = {
            //                          State.Start      State.Prolog     State.PostDTD    State.Element    State.Attribute  State.Content   State.AttrOnly   State.Epilog
            //
            /* Token.PI             */ State.Error,     State.Prolog,    State.PostDTD,   State.Content,   State.Content,   State.Content,  State.Error,     State.Epilog,
            /* Token.Doctype        */ State.Error,     State.PostDTD,   State.Error,     State.Error,     State.Error,     State.Error,    State.Error,     State.Error,
            /* Token.Comment        */ State.Error,     State.Prolog,    State.PostDTD,   State.Content,   State.Content,   State.Content,  State.Error,     State.Epilog,
            /* Token.CData          */ State.Error,     State.Error,     State.Error,     State.Content,   State.Content,   State.Content,  State.Error,     State.Error,
            /* Token.StartElement   */ State.Error,     State.Element,   State.Element,   State.Element,   State.Element,   State.Element,  State.Error,     State.Error,
            /* Token.EndElement     */ State.Error,     State.Error,     State.Error,     State.Content,   State.Content,   State.Content,  State.Error,     State.Error,
            /* Token.LongEndElement */ State.Error,     State.Error,     State.Error,     State.Content,   State.Content,   State.Content,  State.Error,     State.Error,
            /* Token.StartAttribute */ State.Error,     State.Error,     State.Error,     State.Attribute, State.Attribute, State.Error,    State.Error,     State.Error,
            /* Token.EndAttribute   */ State.Error,     State.Error,     State.Error,     State.Error,     State.Element,   State.Error,    State.Error,     State.Error,
            /* Token.Content        */ State.Error,     State.Error,     State.Error,     State.Content,   State.Attribute, State.Content,  State.Error,     State.Error,
            /* Token.Base64         */ State.Error,     State.Error,     State.Error,     State.Content,   State.Attribute, State.Content,  State.Error,     State.Error,
            /* Token.RawData        */ State.Error,     State.Prolog,    State.PostDTD,   State.Content,   State.Attribute, State.Content,  State.Error,     State.Epilog,
            /* Token.Whitespace     */ State.Error,     State.Prolog,    State.PostDTD,   State.Content,   State.Attribute, State.Content,  State.Error,     State.Epilog,
        };

        private XmlTextWriter()
        {
            _namespaces = true;
            _formatting = Formatting.None;
            _indentation = 2;
            _indentChars = s_defaultIndentChars;

            // namespaces
            //_nsStack = new Namespace[NamespaceStackInitialSize];
            _nsTop = -1;
            // element stack
            _stack = new TagInfo[10];
            _top = 0; // 0 is an empty sentanial element
            _stack[_top].Init(-1);
            _quoteChar = '"';

            _stateTable = s_stateTableDefault;
            _currentState = State.Start;
            _lastToken = Token.Empty;
        }



        // Creates an instance of the XmlTextWriter class using the specified TextWriter.
        public XmlTextWriter(TextWriter w) : this()
        {
            _textWriter = w;
            if(w is System.IO.StringWriter)
            {
                this._MyStrBuilder = ((System.IO.StringWriter)w).GetStringBuilder();
            }
            _encoding = w.Encoding;
            _xmlEncoder = new XmlTextEncoder(w);
            _xmlEncoder.QuoteChar = _quoteChar;
        }

        private System.Text.StringBuilder _MyStrBuilder = null;
        public Formatting Formatting
        {
            get { return _formatting; }
            set { _formatting = value; _indented = value == Formatting.Indented; }
        }

        public int Indentation
        {
            get { return _indentation; }
            set
            {
                if (value < 0)
                    throw new ArgumentException();
                _indentation = value;
            }
        }

        public char IndentChar
        {
            get { return _indentChars[0]; }
            set
            {
                if (value == DefaultIndentChar)
                {
                    _indentChars = s_defaultIndentChars;
                    return;
                }

                if (ReferenceEquals(_indentChars, s_defaultIndentChars))
                {
                    _indentChars = new char[IndentArrayLength];
                }

                for (int i = 0; i < IndentArrayLength; i++)
                {
                    _indentChars[i] = value;
                }
            }
        }

        public override void WriteStartDocument()
        {
            StartDocument(-1);
        }

        public override void WriteEndDocument()
        {
            try
            {
                AutoCompleteAll();
                if (_currentState != State.Epilog)
                {
                    if (_currentState == State.Closed)
                    {
                        throw new ArgumentException();
                    }
                    else
                    {
                        throw new ArgumentException();
                    }
                }
                _stateTable = s_stateTableDefault;
                _currentState = State.Start;
                _lastToken = Token.Empty;
            }
            catch
            {
                _currentState = State.Error;
                throw;
            }
        }

        public override void WriteStartElement(string prefix, string localName, string ns)
        {
            //if(this.EventBeforeWriteStartElement != null )
            //{
            //    this.EventBeforeWriteStartElement();
            //}
            try
            {
                AutoComplete(Token.StartElement);
                PushStack();
                _textWriter.Write('<');

                //if (_namespaces)
                //{
                //    // Propagate default namespace and mix model down the stack.
                //    _stack[_top].defaultNs = _stack[_top - 1].defaultNs;
                //    if (_stack[_top - 1].defaultNsState != NamespaceState.Uninitialized)
                //        _stack[_top].defaultNsState = NamespaceState.NotDeclaredButInScope;
                //    _stack[_top].mixed = _stack[_top - 1].mixed;
                //    if (ns == null)
                //    {
                //        // use defined prefix
                //        if (prefix != null && prefix.Length != 0 && (LookupNamespace(prefix) == -1))
                //        {
                //            throw new ArgumentException();
                //        }
                //    }
                //    else
                //    {
                //        if (prefix == null)
                //        {
                //            string? definedPrefix = FindPrefix(ns);
                //            if (definedPrefix != null)
                //            {
                //                prefix = definedPrefix;
                //            }
                //            else
                //            {
                //                PushNamespace(null, ns, false); // new default
                //            }
                //        }
                //        else if (prefix.Length == 0)
                //        {
                //            PushNamespace(null, ns, false); // new default
                //        }
                //        else
                //        {
                //            if (ns.Length == 0)
                //            {
                //                prefix = null;
                //            }

                //            VerifyPrefixXml(prefix, ns);
                //            PushNamespace(prefix, ns, false); // define
                //        }
                //    }
                //    _stack[_top].prefix = null;
                //    if (prefix != null && prefix.Length != 0)
                //    {
                //        _stack[_top].prefix = prefix;
                //        _textWriter.Write(prefix);
                //        _textWriter.Write(':');
                //    }
                //}
                //else
                //{
                //    if ((ns != null && ns.Length != 0) || (prefix != null && prefix.Length != 0))
                //    {
                //        throw new ArgumentException();
                //    }
                //}
                _stack[_top].name = localName;
                _textWriter.Write(localName);
            }
            catch
            {
                _currentState = State.Error;
                throw;
            }
        }

        public override void WriteEndElement()
        {
            InternalWriteEndElement(false);
        }

        public override void WriteFullEndElement()
        {
            InternalWriteEndElement(true);
        }

        public override void WriteStartAttribute(string prefix, string localName, string ns)
        {
            try
            {
                AutoComplete(Token.StartAttribute);


                //if (_namespaces)
                //{
                //    if (prefix != null && prefix.Length == 0)
                //    {
                //        prefix = null;
                //    }
                //    if( ns == XmlReservedNs.NsXml)
                //    {

                //    }
                //    if (ns == XmlReservedNs.NsXmlNs && prefix == null && localName != "xmlns")
                //    {
                //        prefix = "xmlns";
                //    }
                //    //if( ns == XmlReservedNs.NsXsi)
                //    //{
                //    //    prefix = "xsi";
                //    //}
                //    //else if( ns == XmlReservedNs.NsXs )
                //    //{
                //    //    prefix = "xsd";
                //    //}
                //    if (prefix == "xml")
                //    {
                //        if (localName == "lang")
                //        {
                //            _specialAttr = SpecialAttr.XmlLang;
                //        }
                //        else if (localName == "space")
                //        {
                //            _specialAttr = SpecialAttr.XmlSpace;
                //        }
                //        /* bug54408. to be fwd compatible we need to treat xml prefix as reserved
                //        and not really insist on a specific value. Who knows in the future it
                //        might be OK to say xml:blabla
                //        else {
                //            throw new ArgumentException(Xml_InvalidPrefix);
                //        }*/
                //    }
                //    else if (prefix == "xmlns")
                //    {
                //        if (XmlReservedNs.NsXmlNs != ns && ns != null)
                //        {
                //            throw new ArgumentException();
                //        }

                //        if (localName == null || localName.Length == 0)
                //        {
                //            localName = prefix;
                //            prefix = null;
                //            _prefixForXmlNs = null;
                //        }
                //        else
                //        {
                //            _prefixForXmlNs = localName;
                //        }

                //        _specialAttr = SpecialAttr.XmlNs;
                //    }
                //    else if (prefix == null && localName == "xmlns")
                //    {
                //        if (XmlReservedNs.NsXmlNs != ns && ns != null)
                //        {
                //            // add the below line back in when DOM is fixed
                //            throw new ArgumentException();
                //        }

                //        _specialAttr = SpecialAttr.XmlNs;
                //        _prefixForXmlNs = null;
                //    }
                //    else
                //    {
                //        if (ns == null)
                //        {
                //            // use defined prefix
                //            if (prefix != null && (LookupNamespace(prefix) == -1))
                //            {
                //                throw new ArgumentException();
                //            }
                //        }
                //        else if (ns.Length == 0)
                //        {
                //            // empty namespace require null prefix
                //            prefix = string.Empty;
                //        }
                //        else
                //        { // ns.Length != 0
                //            VerifyPrefixXml(prefix, ns);
                //            if (prefix != null && LookupNamespaceInCurrentScope(prefix) != -1)
                //            {
                //                prefix = null;
                //            }

                //            // Now verify prefix validity
                //            string? definedPrefix = FindPrefix(ns);
                //            if (definedPrefix != null && (prefix == null || prefix == definedPrefix))
                //            {
                //                prefix = definedPrefix;
                //            }
                //            else
                //            {
                //                if (prefix == null)
                //                {
                //                    if( ns == XmlReservedNs.NsXsi)
                //                    {
                //                        prefix = "xsi";
                //                    }
                //                    else if(ns == XmlReservedNs.NsXs)
                //                    {
                //                        prefix = "xsd";
                //                    }
                //                    else
                //                    {
                //                        prefix = GeneratePrefix(); // need a prefix if
                //                    }
                //                }
                //                PushNamespace(prefix, ns, false);
                //            }
                //        }
                //    }

                //    if (prefix != null && prefix.Length != 0)
                //    {
                //        _textWriter.Write(prefix);
                //        _textWriter.Write(':');
                //    }
                //}
                //else
                //{
                //    if ((ns != null && ns.Length != 0) || (prefix != null && prefix.Length != 0))
                //    {
                //        throw new ArgumentException();
                //    }

                //    if (localName == "xml:lang")
                //    {
                //        _specialAttr = SpecialAttr.XmlLang;
                //    }

                //    else if (localName == "xml:space")
                //    {
                //        _specialAttr = SpecialAttr.XmlSpace;
                //    }
                //}

                _xmlEncoder.StartAttribute();

                _textWriter.Write(localName);
                _textWriter.Write('=');
                if (_curQuoteChar != _quoteChar)
                {
                    _curQuoteChar = _quoteChar;
                    _xmlEncoder.QuoteChar = _quoteChar;
                }
                _textWriter.Write(_curQuoteChar);
            }
            catch
            {
                _currentState = State.Error;
                throw;
            }
        }

        public override void WriteEndAttribute()
        {
            try
            {
                AutoComplete(Token.EndAttribute);
            }
            catch
            {
                _currentState = State.Error;
                throw;
            }
        }


        public override void WriteString(string text)
        {
            try
            {
                if (string.IsNullOrEmpty( text ) == false )// null != text && text.Length != 0)
                {
                    AutoComplete(Token.Content);
                    _xmlEncoder.Write(text);
                }
            }
            catch
            {
                _currentState = State.Error;
                throw;
            }
        }

        public void WriteChar(char c)
        {
            try
            {
                AutoComplete(Token.Content);
                if (XmlCharType.IsAttributeValueChar(c))
                {
                    if (this._MyStrBuilder != null)
                    {
                        this._MyStrBuilder.Append(c);
                    }
                    else
                    {
                        this._textWriter.Write(c);
                    }
                    return;
                }
                else
                {
                    _xmlEncoder.Write(c.ToString());
                }
            }
            catch
            {
                _currentState = State.Error;
                throw;
            }
        }

        private char[] _Base64Buffer = null;

        public void WriteAttributeImageData(string localName, byte[] imgData )
        {
            this.WriteStartAttribute(null, localName, null);
            this.AutoComplete(Token.Content);
            if (imgData != null && imgData.Length > 0)
            {
                var strHeader = XImageValue.StaticGetEmitImageSourceHeader(imgData);
                if (this._MyStrBuilder != null)
                {
                    this._MyStrBuilder.Append(strHeader);
                }
                else
                {
                    this._textWriter.Write(strHeader);
                }
                var len2 = (int)(imgData.Length * 4.0 / 3.0) + 100;
                if (this._Base64Buffer == null || this._Base64Buffer.Length < len2)
                {
                    this._Base64Buffer = new char[len2];
                }
                var len = Convert.ToBase64CharArray(imgData, 0, imgData.Length, this._Base64Buffer, 0);
                if (this._MyStrBuilder != null)
                {
                    this._MyStrBuilder.Append(this._Base64Buffer, 0, len);
                }
                else
                {
                    this._textWriter.Write(this._Base64Buffer, 0, len);
                }
            }
            this.WriteEndAttribute();
        }

        public void WriteAttributeStringRaw(string localName, string value)
        {
            this.WriteStartAttribute(null, localName, null);
            this.AutoComplete(Token.Content);
            if (value != null && value.Length > 0)
            {
                if (this._MyStrBuilder != null)
                {
                    this._MyStrBuilder.Append(value);
                }
                else
                {
                    this._textWriter.Write(value);
                }
            }
            this.WriteEndAttribute();
        }
        public void WriteAttributeCharsRaw( string name , char[] buffer , int len )
        {
            this.WriteStartAttribute(null, name, null);
            this.AutoComplete(Token.Content);
            if (this._MyStrBuilder != null)
            {
                this._MyStrBuilder.Append(buffer, 0, len);
            }
            else
            {
                this._textWriter.Write(buffer, 0, len);
            }
            this.WriteEndAttribute();
        }

        private static int _LastInt32Value = int.MinValue;
        private static int _lastValueLength = 0;
        private static char[] _LastChars = null;

        public void WriteAttributeInt32UseLastValue(string name, int v)
        {
            if (v == _LastInt32Value && _lastValueLength > 0)
            {
                this.WriteAttributeCharsRaw(name, _LastChars, _lastValueLength);
            }
            else
            {
                var len = StaticAppendInt32(_Buffer_SVGSingleToString, 0, v);
                if (len > 0)
                {
                    _LastInt32Value = v;
                    _lastValueLength = len;
                    if (_LastChars == null)
                    {
                        _LastChars = (char[])_Buffer_SVGSingleToString.Clone();
                    }
                    else
                    {
                        Array.Copy(_Buffer_SVGSingleToString, _LastChars, len);
                    }
                    this.WriteAttributeCharsRaw(name, _Buffer_SVGSingleToString, len);
                }
            }
        }

        private static readonly char[] _Buffer_SVGSingleToString = new char[40];

        public void WriteAttributeInt32(string name, int v)
        {
            var len = StaticAppendInt32(_Buffer_SVGSingleToString, 0, v);
            if (len > 0)
            {
                this.WriteAttributeCharsRaw(name, _Buffer_SVGSingleToString, len);
            }
        }

        public void WriteAttributeInt32AddHalf(string name, int v, bool addHalf = false)
        {
            var buf = _Buffer_SVGSingleToString;
            var len = StaticAppendInt32(buf, 0, v);
            if (len > 0)
            {
                buf[len++] = '.';
                buf[len++] = '5';
                this.WriteAttributeCharsRaw(name, buf, len);
            }
        }
        public void WriteAttributeSingle(string name, float v)
        {
            var len = StaticAppendSingle(_Buffer_SVGSingleToString, 0, v, 10000);
            if (len > 0)
            {
                this.WriteAttributeCharsRaw(name, _Buffer_SVGSingleToString, len);
            }
        }
        public static string SingleToString(float v)
        {
            //if( _Buffer_SVGSingleToString == null )
            //{
            //    _Buffer_SVGSingleToString = new char[40];
            //}
            var len = StaticAppendSingle(_Buffer_SVGSingleToString, 0, v, 10000);
            if (len > 0)
            {
                return new string(_Buffer_SVGSingleToString, 0, len);
            }
            else
            {
                return string.Empty;
            }
        }
        public static int StaticAppendSingle(char[] chrBuffer, int position, float v, int maskDigsAfterZero)
        {
            var bufferLength = chrBuffer.Length;
            if (position >= bufferLength)
            {
                return position;
            }
            if (v == 0)
            {
                chrBuffer[position++] = '0';
                return position;
            }
            else if (v == 1)
            {
                chrBuffer[position++] = '1';
                return position;
            }
            if (v > 10000000000000000f || v < -10000000000000000f || float.IsNaN(v))
            {
                // 超出范围
                var str2 = v.ToString();
                var len = Math.Min(str2.Length, bufferLength - position);
                Array.Copy(str2.ToCharArray(), 0, chrBuffer, position, len);
                return position + len;
            }
            if (v < 0)
            {
                v = -v;
                chrBuffer[position++] = '-';
            }
            var startIndex = position;
            if (maskDigsAfterZero <= 1)
            {
                maskDigsAfterZero = 1000000;
            }
            var intValue = (int)Math.Truncate(v);
            var intValueAfterZero = (int)((v - intValue) * maskDigsAfterZero);
            //long intValue = (long)(v * maskDigsAfterZero);
            //long intValueAfterZero = intValue % maskDigsAfterZero;
            //intValue = (intValue - intValueAfterZero) / maskDigsAfterZero;
            if (intValue == 0)
            {
                chrBuffer[startIndex++] = '0';
            }
            else if (intValue < 1000)
            {
                // 大多数情况下处于这个区间
                if (intValue >= 100)
                {
                    var v2 = intValue % 10;
                    chrBuffer[startIndex + 2] = (char)(v2 + '0');
                    intValue = (intValue - v2) / 10;
                    v2 = intValue % 10;
                    chrBuffer[startIndex + 1] = (char)(v2 + '0');
                    chrBuffer[startIndex] = (char)(((intValue - v2) / 10) + '0');
                    startIndex += 3;
                }
                else if (intValue >= 10)
                {
                    var v2 = intValue % 10;
                    chrBuffer[startIndex + 1] = (char)(v2 + '0');
                    chrBuffer[startIndex] = (char)(((intValue - v2) / 10) + '0');
                    startIndex += 2;
                }
                else
                {
                    chrBuffer[startIndex++] = (char)(intValue + '0');
                }
            }
            else
            {
                int oldStartIndex = startIndex;
                while (intValue > 0)
                {
                    var index = (int)(intValue % 10);
                    chrBuffer[startIndex++] = (char)(index + '0');
                    intValue = (intValue - index) / 10;
                }
                if (startIndex > oldStartIndex + 1)
                {
                    Array.Reverse(chrBuffer, oldStartIndex, startIndex - oldStartIndex);
                }
            }
            if (intValueAfterZero > 0)
            {
                // 处理小数部分
                chrBuffer[startIndex++] = '.';
                int oldStartIndex = startIndex;
                while (maskDigsAfterZero > 1)
                {
                    var index = (int)(intValueAfterZero % 10);
                    chrBuffer[startIndex++] = (char)(index + '0');
                    intValueAfterZero = (intValueAfterZero - index) / 10;
                    maskDigsAfterZero = maskDigsAfterZero / 10;
                }
                if (startIndex > oldStartIndex + 1)
                {
                    Array.Reverse(chrBuffer, oldStartIndex, startIndex - oldStartIndex);
                    while (chrBuffer[startIndex - 1] == '0')
                    {
                        startIndex--;
                    }
                }
            }
            return startIndex;
        }

        public static int StaticAppendInt32(char[] chrBuffer, int position, int intValue)
        {
            var bufferLength = chrBuffer.Length;
            if (position >= bufferLength)
            {
                return position;
            }
            if (intValue == 0)
            {
                chrBuffer[position++] = '0';
                return position;
            }
            else if (intValue == 1)
            {
                chrBuffer[position++] = '1';
                return position;
            }
            if (intValue < 0)
            {
                intValue = -intValue;
                chrBuffer[position++] = '-';
            }
            var startIndex = position;
            if (intValue < 1000)
            {
                // 大多数情况下处于这个区间
                if (intValue >= 100)
                {
                    var v2 = intValue % 10;
                    chrBuffer[startIndex + 2] = (char)(v2 + '0');
                    intValue = (intValue - v2) / 10;
                    v2 = intValue % 10;
                    chrBuffer[startIndex + 1] = (char)(v2 + '0');
                    chrBuffer[startIndex] = (char)(((intValue - v2) / 10) + '0');
                    startIndex += 3;
                }
                else if (intValue >= 10)
                {
                    var v2 = intValue % 10;
                    chrBuffer[startIndex + 1] = (char)(v2 + '0');
                    chrBuffer[startIndex] = (char)(((intValue - v2) / 10) + '0');
                    startIndex += 2;
                }
                else
                {
                    chrBuffer[startIndex++] = (char)(intValue + '0');
                }
            }
            else
            {
                int oldStartIndex = startIndex;
                while (intValue > 0)
                {
                    var index = (int)(intValue % 10);
                    chrBuffer[startIndex++] = (char)(index + '0');
                    intValue = (intValue - index) / 10;
                }
                if (startIndex > oldStartIndex + 1)
                {
                    Array.Reverse(chrBuffer, oldStartIndex, startIndex - oldStartIndex);
                }
            }
            return startIndex;
        }


        public override void WriteRaw(string data)
        {
            try
            {
                AutoComplete(Token.RawData);
                _xmlEncoder.WriteRawWithSurrogateChecking(data);
            }
            catch
            {
                _currentState = State.Error;
                throw;
            }
        }

        public override WriteState WriteState
        {
            get
            {
                switch (_currentState)
                {
                    case State.Start:
                        return WriteState.Start;
                    case State.Prolog:
                    case State.PostDTD:
                        return WriteState.Prolog;
                    case State.Element:
                        return WriteState.Element;
                    case State.Attribute:
                    case State.AttrOnly:
                        return WriteState.Attribute;
                    case State.Content:
                    case State.Epilog:
                        return WriteState.Content;
                    case State.Error:
                        return WriteState.Error;
                    case State.Closed:
                        return WriteState.Closed;
                    default:
                        Debug.Fail($"Unexpected state {_currentState}");
                        return WriteState.Error;
                }
            }
        }

        public override void Close()
        {
            this._Base64Buffer = null;
            this._MyStrBuilder = null;
            try
            {
                AutoCompleteAll();
            }
            catch
            { // never fail
            }
            finally
            {
                _currentState = State.Closed;
                _textWriter.Dispose();
            }
        }

        private void StartDocument(int standalone)
        {
            try
            {
                if (_currentState != State.Start)
                {
                    throw new InvalidOperationException();
                }
                _stateTable = s_stateTableDocument;
                _currentState = State.Prolog;

                //StringBuilder bufBld = new StringBuilder(128);
                //bufBld.Append($"version={_quoteChar}1.0{_quoteChar}");
                //if (_encoding != null)
                //{
                //    bufBld.Append(" encoding=");
                //    bufBld.Append(_quoteChar);
                //    bufBld.Append(_encoding.WebName);
                //    bufBld.Append(_quoteChar);
                //}
                //if (standalone >= 0)
                //{
                //    bufBld.Append(" standalone=");
                //    bufBld.Append(_quoteChar);
                //    bufBld.Append(standalone == 0 ? "no" : "yes");
                //    bufBld.Append(_quoteChar);
                //}
                //InternalWriteProcessingInstruction("xml", bufBld.ToString());
            }
            catch
            {
                _currentState = State.Error;
                throw;
            }
        }

        public void AutoComplete(Token token)
        {
            if (_currentState == State.Closed)
            {
                throw new InvalidOperationException();
            }
            else if (_currentState == State.Error)
            {
                throw new InvalidOperationException("WrongToken:" + token.ToString() +  "Error");
            }

            State newState = _stateTable[((int)token <<3) + (int)_currentState];
            if (newState == State.Error)
            {
                throw new InvalidOperationException("WrongToken:" + token.ToString()  +_currentState.ToString());
            }

            switch (token)
            {
                case Token.Doctype:
                    if (_indented && _currentState != State.Start)
                    {
                        Indent(false);
                    }
                    break;

                case Token.StartElement:
                case Token.Comment:
                case Token.PI:
                case Token.CData:
                    if (_currentState == State.Attribute)
                    {
                        WriteEndAttributeQuote();
                        WriteEndStartTag(false);
                    }
                    else if (_currentState == State.Element)
                    {
                        WriteEndStartTag(false);
                    }
                    if (token == Token.CData)
                    {
                        _stack[_top].mixed = true;
                    }
                    else if (_indented && _currentState != State.Start)
                    {
                        Indent(false);
                    }
                    break;

                case Token.EndElement:
                case Token.LongEndElement:
                    if (_flush)
                    {
                        FlushEncoders();
                    }
                    if (_currentState == State.Attribute)
                    {
                        WriteEndAttributeQuote();
                    }
                    if (_currentState == State.Content)
                    {
                        token = Token.LongEndElement;
                    }
                    else
                    {
                        WriteEndStartTag(token == Token.EndElement);
                    }
                    if (s_stateTableDocument == _stateTable && _top == 1)
                    {
                        newState = State.Epilog;
                    }
                    break;

                case Token.StartAttribute:
                    if (_flush)
                    {
                        FlushEncoders();
                    }
                    if (_currentState == State.Attribute)
                    {
                        WriteEndAttributeQuote();
                        _textWriter.Write(' ');
                    }
                    else if (_currentState == State.Element)
                    {
                        if (this._MyStrBuilder != null)
                        {
                            this._MyStrBuilder.Append(' ');
                        }
                        else
                        {
                            _textWriter.Write(' ');
                        }
                    }
                    break;

                case Token.EndAttribute:
                    if (_flush)
                    {
                        FlushEncoders();
                    }
                    WriteEndAttributeQuote();
                    break;

                case Token.Whitespace:
                case Token.Content:
                case Token.RawData:
                case Token.Base64:

                    if (token != Token.Base64 && _flush)
                    {
                        FlushEncoders();
                    }
                    if (_currentState == State.Element && _lastToken != Token.Content)
                    {
                        WriteEndStartTag(false);
                    }
                    if (newState == State.Content)
                    {
                        _stack[_top].mixed = true;
                    }
                    break;

                default:
                    throw new InvalidOperationException();
            }
            _currentState = newState;
            _lastToken = token;
        }

        private void AutoCompleteAll()
        {
            if (_flush)
            {
                FlushEncoders();
            }
            while (_top > 0)
            {
                WriteEndElement();
            }
        }

        private static readonly char[] s_selfClosingTagOpen = new char[] { '<', '/' };

        private void InternalWriteEndElement(bool longFormat)
        {
            try
            {
                if (_top <= 0)
                {
                    throw new InvalidOperationException();
                }
                // if we are in the element, we need to close it.
                AutoComplete(longFormat ? Token.LongEndElement : Token.EndElement);
                if (_lastToken == Token.LongEndElement)
                {
                    if (_indented)
                    {
                        Indent(true);
                    }
                    _textWriter.Write(s_selfClosingTagOpen);
                    //if (_namespaces && _stack[_top].prefix != null)
                    //{
                    //    _textWriter.Write(_stack[_top].prefix);
                    //    _textWriter.Write(':');
                    //}
                    _textWriter.Write(_stack[_top].name);
                    _textWriter.Write('>');
                }

                // pop namespaces
                int prevNsTop = _stack[_top].prevNsTop;
                //if (_useNsHashtable && prevNsTop < _nsTop)
                //{
                //    PopNamespaces(prevNsTop + 1, _nsTop);
                //}
                _nsTop = prevNsTop;
                _top--;
            }
            catch
            {
                _currentState = State.Error;
                throw;
            }
        }

        private static readonly char[] s_closeTagEnd = new char[] { ' ', '/', '>' };

        private void WriteEndStartTag(bool empty)
        {
            _xmlEncoder.StartAttribute();
            _xmlEncoder.EndAttribute();
            if (empty)
            {
                _textWriter.Write(s_closeTagEnd);
            }
            else
            {
                _textWriter.Write('>');
            }
        }

        private void WriteEndAttributeQuote()
        {
            _xmlEncoder.EndAttribute();
            if (this._MyStrBuilder != null)
            {
                this._MyStrBuilder.Append(_curQuoteChar);
            }
            else
            {
                _textWriter.Write(_curQuoteChar);
            }
        }

        private void Indent(bool beforeEndElement)
        {
            // pretty printing.
            if (_top == 0)
            {
                _textWriter.WriteLine();
            }
            else if (!_stack[_top].mixed)
            {
                _textWriter.WriteLine();
                int i = (beforeEndElement ? _top - 1 : _top) * _indentation;
                if (i <= _indentChars.Length)
                {
                    _textWriter.Write(_indentChars, 0, i);
                }
                else
                {
                    while (i > 0)
                    {
                        _textWriter.Write(_indentChars, 0, Math.Min(i, _indentChars.Length));
                        i -= _indentChars.Length;
                    }
                }
            }
        }

        private void PushStack()
        {
            if (_top == _stack.Length - 1)
            {
                TagInfo[] na = new TagInfo[_stack.Length + 10];
                if (_top > 0) Array.Copy(_stack, na, _top + 1);
                _stack = na;
            }

            _top++; // Move up stack
            _stack[_top].Init(_nsTop);
        }

        private void FlushEncoders()
        {
            //if (null != _base64Encoder)
            //{
            //    // The Flush will call WriteRaw to write out the rest of the encoded characters
            //    _base64Encoder.Flush();
            //}
            _flush = false;
        }
    }
}
