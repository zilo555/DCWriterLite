using System;
using System.Collections.Generic;
using System.Text;
using DCSoft.Writer.Dom ;
using DCSystemXml ;
using System.Drawing;

namespace DCSoft.Writer.Serialization.Xml
{
    public class MyXmlSerializeHelper
    {
        static MyXmlSerializeHelper()
        {
            // 初始化标准的文档XML序列化器
            _ElementTypes.Add(typeof(DomDocument));
            _ElementTypes.Add(typeof(DCSoft.Writer.Dom.DomParagraphFlagElement));
            _ElementTypes.Add(typeof(DCSoft.Writer.Dom.DomImageElement));
            _ElementTypes.Add(typeof(DCSoft.Writer.Dom.DomLineBreakElement));
            _ElementTypes.Add(typeof(DCSoft.Writer.Dom.DomDocumentHeaderElement));
            _ElementTypes.Add(typeof(DCSoft.Writer.Dom.DomDocumentBodyElement));
            _ElementTypes.Add(typeof(DCSoft.Writer.Dom.DomDocumentFooterElement));
            _ElementTypes.Add(typeof(DCSoft.Writer.Dom.DomTableElement));
            _ElementTypes.Add(typeof(DCSoft.Writer.Dom.DomTableRowElement));
            _ElementTypes.Add(typeof(DCSoft.Writer.Dom.DomTableColumnElement));
            _ElementTypes.Add(typeof(DCSoft.Writer.Dom.DomTableCellElement));
            _ElementTypes.Add(typeof(DCSoft.Writer.Dom.DomPageBreakElement));
            _ElementTypes.Add(typeof(DCSoft.Writer.Dom.DomInputFieldElement));
            _ElementTypes.Add(typeof(DCSoft.Writer.Dom.DomCheckBoxElement));
            _ElementTypes.Add(typeof(DCSoft.Writer.Dom.DomRadioBoxElement));
            _ElementTypes.Add(typeof(DCSoft.Writer.Dom.DomPageInfoElement));
            _ElementTypes.Add(typeof(DCSoft.Writer.Dom.DomTextElement));
        }

        private static List<Type> _ElementTypes = new List<Type>();
        public static Type[] ElementTypes
        {
            get
            {
                return _ElementTypes.ToArray();
            }
        }

        private static int _ElementTypesVersion = 0;
        private static Dictionary<Type, XmlSerializerTypeInfo> _SerializerTypes 
            = new Dictionary<Type, XmlSerializerTypeInfo>();
        private class XmlSerializerTypeInfo
        {
            public int Version = 0;
            public Type SerType = null;
        }
        public static void AddSerializerType( Type rootType , Type serType )
        {
            if( rootType == null )
            {
                throw new ArgumentNullException("rootType");
            }
            if( serType == null )
            {
                throw new ArgumentNullException("serType");
            }
            XmlSerializerTypeInfo info = new XmlSerializerTypeInfo();
            info.Version = _ElementTypesVersion;
            info.SerType = serType;
            _SerializerTypes[rootType] = info;
        }

        public static void AddElementType(Type elementType )
        {
            _ElementTypesVersion++;
            if (elementType == null)
            {
                throw new ArgumentNullException("elementType");
            }
            if (typeof(DomElement).IsAssignableFrom(elementType))
            {
                if (_ElementTypes.Contains(elementType) == false)
                {
                    _ElementTypes.Add(elementType);
                    //_xmlSerializers.Clear();
                }
            }
            else
            {
                throw new ArgumentOutOfRangeException(elementType.FullName + "<> XTextElement");
            }
        }
         
    }
}
