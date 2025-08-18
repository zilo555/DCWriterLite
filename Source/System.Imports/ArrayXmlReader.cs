

#define ArrayXmlReader_DEBUG222

global using System;
global using DCSystem_Drawing;
global using DCSystem_Drawing.Drawing2D;
global using DCSystem_Drawing.Printing;


using System.Collections.Generic;
using System.Text;
using System.IO;
using DCSystemXml;
using DCSoft.Writer.Data;
using DCSoft.Drawing;
using System.Runtime.Serialization;

namespace DCSoft.Writer.Serialization
{
    /// <summary>
    /// 基于数组的XML读取器
    /// </summary>
    public class ArrayXmlReader : DCSystemXml.XmlReader
    {
        public ArrayXmlReader(string[] strs, byte[] nameIndex, byte[] idNamesIndexs , List<byte[]> binaryDataList)
        {
            if(nameIndex == null )
            {
                throw new ArgumentNullException("nameIndex");
            }
            this._Strings = strs;
            this._IDNameIndexs = GetIndexValues(idNamesIndexs);
            this._NameIndexs = GetIndexValues(nameIndex);
            this._NameIndexsLength = this._NameIndexs.Length;
            this._BinaryDataList = binaryDataList;
        }

        public void ReadForImageDataBase64String(object img)
        {
            var v = this.ReadElementString();
            if (v != null && v.Length > 0)
            {
                if (this._BinaryDataList != null)
                {
                    if (v.StartsWith("$BINARY_", StringComparison.Ordinal))
                    {
                        var index = 0;
                        if (int.TryParse(v.Substring("$BINARY_".Length), out index))
                        {
                            if (index >= 0 && index < this._BinaryDataList.Count)
                            {
                                if (img is XImageValue)
                                {
                                    ((XImageValue)img).ImageData = this._BinaryDataList[index];
                                }
                            }
                        }
                    }
                    else
                    {
                        if (img is XImageValue)
                        {
                            ((XImageValue)img).ImageData = Convert.FromBase64String(v);
                        }
                    }
                }
                else
                {
                    ((XImageValue)img).ImageData = Convert.FromBase64String(v);
                }
            }
        }

        public override void Close()
        {
            base.Close();
            if (this._BinaryDataList != null)
            {
                this._BinaryDataList.Clear();
                this._BinaryDataList = null;
            }
            this._AttributeNames = null;
            this._AttributeValues = null;
            this._Elements = null;
            this._IDNameIndexs = null;
            this._NameIndexs = null;
            this._NameTable = null;
            this._LocalName = null;
            this._Value = null;
            this._Strings = null;
            this._CurrentElement = null;
        }
        private int[] GetIndexValues(byte[] bs )
        {
            if( bs == null || bs.Length == 0 )
            {
                return null;
            }
            var result = new int[bs.Length / 4];
            Buffer.BlockCopy(bs, 0, result, 0, bs.Length );
            return result;
        }
        private List<byte[]> _BinaryDataList = null;
        private string[] _Strings = null;
        private int[] _NameIndexs = null;
        private int _NameIndexsLength = 0;
        private int[] _IDNameIndexs = null;
        private int _Position = 0;
        public override int Depth => throw new NotImplementedException();
        public override bool IsEmptyElement
        {
            get
            {
                return this._CurrentElement == null 
                    || this._CurrentElement.Type == ElementType.EmptyElement
                    || this._CurrentElement.Type == ElementType.ElementNoChild;
            }
        }

        public override string LocalName
        {
            get
            {
                return this._LocalName;
            }
        }

        public override string NamespaceURI => String.Empty;
        private DCSystemXml.NameTable _NameTable = new NameTable();
        public override XmlNameTable NameTable
        {
            get
            {
                return _NameTable;
            }
        }
        public void ApplyNameTable()
        {
            //lock (_LockObject)
            {
                if (this._IDNameIndexs != null
                    && this._IDNameIndexs.Length > 0
                    && this._Strings != null
                    && this._Strings.Length > 0)
                {
                    var nt = this.NameTable;
                    foreach (var item in this._IDNameIndexs)
                    {
                        if (item >= 0 && item < this._Strings.Length)
                        {
                            this._Strings[item] = nt.Add(this._Strings[item]);
                        }
                    }
                    //this._NameTable.Applyed = true;
                    this._IDNameIndexs = null;
                }
            }
        }

        private XmlNodeType _NodeType = XmlNodeType.Element;
        public override XmlNodeType NodeType 
        {
            get
            { 
                return this._NodeType; 
            }
        }

        public override string Prefix => throw new NotImplementedException();

        public override ReadState ReadState => ReadState.Interactive;

        public override string Value
        {
            get
            {
                return this._Value;
            }
        }

        public override string GetAttribute(string name, string namespaceURI)
        {
            for(var iCount = 0; iCount < this._AttributeCount;iCount ++)
            {
                if( this._AttributeNames[iCount] == name)
                {
                    return this._AttributeValues[iCount];
                }
            }
            return null;
        }

        public override XmlNodeType MoveToContent()
        {
            if( this._CurrentElement == null)
            {
                InnerReadElement();
            }
            return this._NodeType;
        }
        public override void Skip()
        {
            if( this._CurrentElement != null && this._CurrentElement.ElementEndPosition > 0 )
            {
                this._Position = this._CurrentElement.ElementEndPosition+1;
                this._NodeType = XmlNodeType.EndElement;
                this.InnerReadForEndElement();
            }
        }
        public override bool MoveToElement()
        {
            return true;
            //if (this._CurrentElement.ElementCount == 0)
            //{
            //    this._NodeType = XmlNodeType.EndElement;
            //    return false;
            //}
            //else
            //{
            //    this._Position = this._CurrentElement.ElementStartPosition;
            //    this.InnerReadElement();
            //    this._NodeType = XmlNodeType.Element;
            //    return true;
            //}
        }
        private void SetAttributeCount( int aCount )
        {
            if (this._AttributeCount != aCount)
            {
                this._AttributeCount = aCount;
                if (this._AttributeNames.Length < aCount)
                {
                    this._AttributeNames = new string[aCount];
                    this._AttributeValues = new string[aCount];
                }
            }
        }
        private string[] _AttributeNames = Array.Empty<string>();
        private string[] _AttributeValues = Array.Empty<string>();
        private int _AttributeCount = 0;
        private int _AttributeIndex = 0;
        public override string ReadString()
        {
            if( this._NodeType == XmlNodeType.Text)
            {
                var result = this._Value;
                this._NodeType = XmlNodeType.EndElement;
                this.InnerReadForEndElement();
                return result;
            }
            else
            {
                throw new NotSupportedException("ReadString");
            }
        }
        public override string ReadElementString()
        {
            var vType = this._CurrentElement.Type;
            if (vType == ElementType.ElementString)
            {
                var strResult = this._CurrentElement.ElementString;
                this._NodeType = XmlNodeType.EndElement;
                this.InnerReadForEndElement();
                return strResult;
            }
            else if (vType == ElementType.EmptyElement)
            {
                InnerReadForEndElement();
                return null;
            }
            else if (vType == ElementType.ElementNoChild)
            {
                InnerReadForEndElement();
                return null;
            }
            else if (this._CurrentElement.ElementCount == 1)
            {
                if (this._NameIndexs[this._Position++] == (int)ElementType.TextNode)
                {
                    var str = this._Strings[this._NameIndexs[this._Position++]];
                    InnerReadForEndElement();
                    return str;
                }
                else
                {
                    var type4 = (ElementType)this._NameIndexs[this._Position - 1];
                    if (type4 == ElementType.ElementFull)
                    {
                        // 具有属性和子节点，首先跳过所有的属性


                    }
                    throw new NotSupportedException(type4.ToString());
                }
                //this.InnerReadElement();
                //var str = this._Value;
                //this._NodeType = XmlNodeType.EndElement;
                //InnerReadForEndElement();
                //return str;
            }
            else
            {
                throw new NotSupportedException(this._CurrentElement.ElementName + "#ReadElementString");
            }
        }
        public override bool MoveToNextAttribute()
        {
            if (this._AttributeIndex < this._AttributeCount)
            {
                this._LocalName = this._AttributeNames[this._AttributeIndex];
                this._Value = this._AttributeValues[this._AttributeIndex];
                this._AttributeIndex++;
                return true;
            }
            else
            {
                this._LocalName = null;
                this._Value = null;
                return false;
            }
        }

        
        private string _LocalName = null;
        private string _Value = null;
        private bool InnerReadForEndElement()
        {
            if (this._ElementsCount > 0)
            {
                this._ElementsCount--;
                //var ep = this._Elements[this._ElementsCount].ElementEndPosition + 1;
                //this._Position = ep;
                //this._Elements[this._ElementsCount] = null;
                if (this._ElementsCount > 0)
                {
                    //var last = this._Elements[this._ElementsCount - 1];
                    if (this._Position < this._Elements[this._ElementsCount - 1].ElementEndPosition)
                    {
                        //var eep = this._Elements[this._ElementsCount].ElementEndPosition + 1;
                        //if (eep != this._Position)
                        //{
                        //    this._Position = eep;
                        //}
                        return InnerReadElement();
                    }
                    else
                    {
                        //this._CurrentElement = last;
                        //if (this._Position != last.ElementEndPosition + 1)
                        //{
                        //    this._Position = last.ElementEndPosition + 1;
                        //}
                        //this._Position = this._Elements[this._ElementsCount].ElementEndPosition + 1;
                        //this._LocalName = null;
                        //this._Value = null;
                        this._NodeType = XmlNodeType.EndElement;
                    }
                }
                return true;
            }
            else
            {
                throw new InvalidOperationException("No element");
                //return false;
            }
        }

        
        public override bool Read()
        {
            if( this._NodeType == XmlNodeType.EndElement )
            {
                return InnerReadForEndElement();
            }
            if( this._CurrentElement != null )
            {
                switch( this._CurrentElement.Type )
                {
                    case ElementType.EmptyElement:
                        this._ElementsCount--;
                        this._NodeType = XmlNodeType.EndElement;
                        break;
                    case ElementType.ElementNoAttribute:
                        return this.InnerReadElement();
                    case ElementType.ElementString:
                        this._LocalName = this._CurrentElement.ElementName;
                        this._Value = this._CurrentElement.ElementString;
                        this._NodeType = XmlNodeType.EndElement;
                        break;
                    case ElementType.ElementNoChild:
                        this._ElementsCount--;
                        this._NodeType = XmlNodeType.EndElement;
                        break;
                    case ElementType.ElementFull:
                        return InnerReadElement();
                }
            }
            return InnerReadElement();
            //return false;
        }

        private const int MAX_ElementsCount = 200;
        private ElementInfo[] _Elements = CreateElements();
        private static ElementInfo[] CreateElements()
        {
            var list = new ElementInfo[MAX_ElementsCount];
            for( var iCount = 0;iCount < list.Length;iCount ++ )
            {
                list[iCount] = new ElementInfo();
            }
            return list;
        }
        private int _ElementsCount = 0;

        private ElementInfo _CurrentElement = null;
        private class ElementInfo
        {
            public ElementType Type;
            public string ElementName;
            public string ElementString;
            //public int AttributeCount;
            //public int AttributeIndex;
            //public int AttributeStartPosition;
            //public int AttributeEndPosition;
            public int ElementCount;
            //public int ElementIndex;
            public int ElementStartPosition;
            public int ElementEndPosition;
        }
        /// <summary>
        /// XML元素的类型
        /// </summary>
        private enum ElementType
        {
            /// <summary>
            /// 纯文本节点
            /// </summary>
            TextNode = 0,
            /// <summary>
            /// 空白元素，没有属性和子节点
            /// </summary>
            EmptyElement = 1,
            /// <summary>
            /// 没有属性，有文本内容
            /// </summary>
            ElementString = 2,
            /// <summary>
            /// 没有属性，但有子节点
            /// </summary>
            ElementNoAttribute = 3,
            /// <summary>
            /// 有属性，但没有子节点
            /// </summary>
            ElementNoChild = 4,
            /// <summary>
            /// 有属性有子节点
            /// </summary>
            ElementFull = 5
        }

        private bool InnerReadElement()
        {
            if (this._Position > this._NameIndexsLength)
            {
                return false;
            }
            else
            {
                //if( this._ElementsCount >= this._Elements.Length)
                //{
                //    var s = "sss";
                //}
                var info = this._Elements[this._ElementsCount++];
                info.Type = (ElementType)this._NameIndexs[this._Position++];// InnerReadInt32();
                info.ElementName = this._LocalName = this._Strings[this._NameIndexs[this._Position++]];// InnerReadStringValue();
                //this._LocalName = info.ElementName;
                switch (info.Type)
                {
                    case ElementType.TextNode:
                        this._CurrentElement = info;
                        this._LocalName = "#text";
                        this._Value = info.ElementName;
                        info.ElementEndPosition = this._Position-1;
                        this._NodeType = XmlNodeType.Text;
                        return true;
                    case ElementType.EmptyElement:
                        this._AttributeCount = 0; //this.SetAttributeCount(0);
                        info.ElementEndPosition = this._Position -1 ;
                        break;
                    case ElementType.ElementString:
                        this._AttributeCount = 0; //this.SetAttributeCount(0);
                        info.ElementString = this._Strings[this._NameIndexs[this._Position++]];//  InnerReadStringValue();
                        info.ElementEndPosition = this._Position -1;
                        break;
                    case ElementType.ElementNoAttribute:
                        this._AttributeCount = 0; //this.SetAttributeCount(0);
                        info.ElementCount = this._NameIndexs[this._Position++];//InnerReadInt32();
                        info.ElementEndPosition = this._NameIndexs[this._Position++];// InnerReadInt32();
                        info.ElementStartPosition = this._Position;
                        break;
                    case ElementType.ElementNoChild:
                        {
                            var aCount = this._NameIndexs[this._Position++];//InnerReadInt32();
                            this.SetAttributeCount(aCount);
                            for (var iCount = 0; iCount < aCount; iCount++)
                            {
                                this._AttributeNames[iCount] = this._Strings[this._NameIndexs[this._Position++]];//InnerReadStringValue();
                                this._AttributeValues[iCount] = this._Strings[this._NameIndexs[this._Position++]];//InnerReadStringValue();
                            }
                            this._AttributeIndex = 0;
                            info.ElementEndPosition = this._Position-1;
                        }
                        break;
                    case ElementType.ElementFull:
                        {
                            var aCount = this._NameIndexs[this._Position++];//InnerReadInt32();
                            this.SetAttributeCount(aCount);
                            for (var iCount = 0; iCount < aCount; iCount++)
                            {
                                this._AttributeNames[iCount] = this._Strings[this._NameIndexs[this._Position++]];// InnerReadStringValue();
                                //if( this._Position >= this._NameIndexs.Length 
                                //    || this._NameIndexs[this._Position] < 0
                                //    || this._NameIndexs[this._Position] >= this._Strings.Length
                                //    || iCount >= this._AttributeValues.Length)
                                //{
                                //    this._Position = this._Position + 1;
                                //}
                                this._AttributeValues[iCount] = this._Strings[this._NameIndexs[this._Position++]];// InnerReadStringValue();
                            }
                            this._AttributeIndex = 0;
                            info.ElementCount = this._NameIndexs[this._Position++];//InnerReadInt32();
                            info.ElementEndPosition = this._NameIndexs[this._Position++];//InnerReadInt32();
                            info.ElementStartPosition = this._Position;
                        }
                        break;
                    default:
                        throw new NotSupportedException(info.Type.ToString());
                }
                this._CurrentElement = info;
                this._NodeType = XmlNodeType.Element;
                return true;
            }
        }
        public override void ResolveEntity()
        {
            throw new NotImplementedException();
        }
#if !RELEASE
        public override string ToString()
        {
            return this.GetType().FullName;
        }
#endif
    }
}
