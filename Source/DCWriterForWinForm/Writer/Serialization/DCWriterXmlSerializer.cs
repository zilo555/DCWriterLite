using DCSoft.Writer.Dom;
using System.Text;
using DCSystemXml;

namespace DCSoft.Writer.NewSerializationNoStringEncrypt
{
    internal static class XSerHelper_XTextDocument
    {
        internal static System.Exception CreateInvalidEnumValueException(long v, System.Type t)
        {
            return new System.InvalidOperationException(t.FullName + "不含数值" + v);
        }
        internal static long ToEnum(string val, System.Collections.Generic.Dictionary<string, long> vals, string typeName, bool validate = true)
        {
            long num = 0L;
            if (vals.TryGetValue(val, out num))
            {
                // 直接命中，则立刻返回
                return num;
            }
            string[] array = val.Split(null);
            for (int i = 0; i < array.Length; i++)
            {
                long v2 = 0;
                if (vals.TryGetValue(array[i], out v2))
                {
                    num |= v2;
                }
                else if (validate && array[i].Length > 0)
                {
                    throw new System.InvalidOperationException(typeName + "没有成员" + array[i]);
                }
            }
            // 有很大概率再次命中，缓存来改进性能。
            vals[val] = num;
            return num;
        }
        public static double ToDouble(string v)
        {
            if (v == null || v.Length == 0)
            {
                return 0;
            }
            if (v == "NaN")
            {
                return double.NaN;
            }
            double dv = 0;
            if (double.TryParse(v, out dv))
            {
                return dv;
            }
            return 0;
        }
        public static float ToSingle(string v)
        {
            if (v == null || v.Length == 0)
            {
                return 0;
            }
            if (v == "NaN")
            {
                return float.NaN;
            }
            float dv = 0;
            if (float.TryParse(v, out dv))
            {
                return dv;
            }
            return 0;
        }
        public static readonly string[] _Array_5 = new string[] {@"None",
                    @"Default",
                    @"Tab",
                    @"Enter"};
        public static readonly long[] _Array_6 = new long[] {(long) DCSoft.Writer.MoveFocusHotKeys.@None,
                    (long) DCSoft.Writer.MoveFocusHotKeys.@Default,
                    (long) DCSoft.Writer.MoveFocusHotKeys.@Tab,
                    (long) DCSoft.Writer.MoveFocusHotKeys.@Enter};
        public static readonly string[] _Array_7 = new string[] {@"None",
                    @"Default",
                    @"Program",
                    @"F2",
                    @"GotFocus",
                    @"MouseDblClick",
                    @"MouseClick",
                    @"MouseRightClick",
                    @"Enter"};
        public static readonly long[] _Array_8 = new long[] {(long) DCSoft.Writer.ValueEditorActiveMode.@None,
                    (long) DCSoft.Writer.ValueEditorActiveMode.@Default,
                    (long) DCSoft.Writer.ValueEditorActiveMode.@Program,
                    (long) DCSoft.Writer.ValueEditorActiveMode.@F2,
                    (long) DCSoft.Writer.ValueEditorActiveMode.@GotFocus,
                    (long) DCSoft.Writer.ValueEditorActiveMode.@MouseDblClick,
                    (long) DCSoft.Writer.ValueEditorActiveMode.@MouseClick,
                    (long) DCSoft.Writer.ValueEditorActiveMode.@MouseRightClick,
                    (long) DCSoft.Writer.ValueEditorActiveMode.@Enter};
    }

    
    public abstract class XmlSerializer1 : DCSoft.Writer.NewSerialization.DCXmlSerializerBase// DCSystemXmlSerialization.XmlSerializer
    {
        protected override DCSoft.Writer.NewSerialization.DCXmlSerializationReader DCCreateReader()
        {
            return new XmlSerializationReaderXTextDocument();
        }
        protected override DCSoft.Writer.NewSerialization.DCXmlSerializationWriter DCCreateWriter()
        {
            return new XmlSerializationWriterXTextDocument();
        }
    }
    public sealed class XTextDocumentSerializer : XmlSerializer1
    {
        public override System.Boolean CanDeserialize(DCSystemXml.XmlReader xmlReader)
        {
            return xmlReader.IsStartElement(@"XTextDocument", string.Empty);
        }
        protected override void Serialize(object objectToSerialize, DCSoft.Writer.NewSerialization.DCXmlSerializationWriter writer)
        {
            ((XmlSerializationWriterXTextDocument)writer).Write261_XTextDocument(objectToSerialize);
        }
        protected override object Deserialize(DCSoft.Writer.NewSerialization.DCXmlSerializationReader reader)
        {
            return ((XmlSerializationReaderXTextDocument)reader).Read261_XTextDocument();
        }
    }
}