using DCSoft.Writer.Dom;
using System.Text;
using DCSystemXml;
using DCSoft.WASM;
using System.Collections.Generic;
using DCSoft.Common;
using DCSoft.Writer.Data;

namespace DCSoft.Writer.NewSerializationNoStringEncrypt
{
    [System.Reflection.Obfuscation(Exclude = false, ApplyToMembers = true, Feature = "JIEJIE.NET.SWITCH:-hightstrings,-controlflow")]
    public class XmlSerializationReaderXTextDocument : DCSoft.Writer.NewSerialization.DCXmlSerializationReader
    {
        private static int _ContentElementCount = 0;
        private void ReadContainerChildElements(XmlReader ThisReader, DomContainerElement rootElement)
        {
            if (ThisReader.IsEmptyElement)
            {
                ThisReader.Skip();
                return;
            }
            var list = LoaderListBuffer<DomElementList>.Instance.Alloc();
            ThisReader.ReadStartElement();
            ThisReader.MoveToContent();
            int whileIterations76 = 0;
            int readerCount76 = ReaderCount;
            var _ReaderNodeType2 = ThisReader.NodeType;
            int colSpanCount = 0;
            var isTableOrRow = rootElement is DomTableElement || rootElement is DomTableRowElement;
            while (_ReaderNodeType2 != DCSystemXml.XmlNodeType.EndElement && _ReaderNodeType2 != DCSystemXml.XmlNodeType.None)
            {
                if (_ReaderNodeType2 == DCSystemXml.XmlNodeType.Element)
                {
                    if (((object)ThisReader.LocalName == (object)id51_Element))
                    {
                        if (isTableOrRow)
                        {
                            // 特别处理表格和表格行
                            var contentCountBack = _ContentElementCount;
                            var item2 = Read3_XTextElement(true, true);
                            if (item2 != null)
                            {
                                if (isTableOrRow)
                                {
                                    if (rootElement is DomTableRowElement)
                                    {
                                        if (item2 is DomTableCellElement cell2)
                                        {
                                            if (colSpanCount > 0)
                                            {
                                                _ContentElementCount = contentCountBack;
                                                colSpanCount--;
                                            }
                                            else if (cell2.ColSpan > 1)
                                            {
                                                colSpanCount = cell2.ColSpan - 1;
                                            }
                                            list.FastAdd2(cell2);
                                        }
                                    }
                                    else
                                    {
                                        if (item2 is DomTableRowElement)
                                        {
                                            list.FastAdd2(item2);
                                        }
                                    }
                                }
                                else
                                {
                                    list.FastAdd2(item2);
                                }
                            }
                        }
                        else
                        {
                            var xsiType = ThisReader.GetAttribute(_typeID, _instanceNsID);
                            if (xsiType == "XString")
                            {
                                // 读取纯文本
                                ReadXTextTextElement(ThisReader, list);
                            }
                            else
                            {
                                var item2 = Read3_XTextElement(true, true );
                                if (item2 != null)
                                {
                                    list.FastAdd2(item2);
                                }
                            }
                        }
                    }
                    else
                    {
                        UnknownNode(null, ThisReader.LocalName);
                    }
                }
                else
                {
                    UnknownNode(null, ThisReader.LocalName);
                }
                ThisReader.MoveToContent();
                CheckReaderCount(ref whileIterations76, ref readerCount76);
                _ReaderNodeType2 = ThisReader.NodeType;
            }//while
            ReadEndElement();
            if (rootElement is DomContentElement)
            {
                if (list.LastElementIsXTextParagraphFlagElement() == false)
                {
                    var pf = new DomParagraphFlagElement();
                    pf.AutoCreate = true;
                    list.FastAdd2(pf);
                }
            }
            if (rootElement._Elements == null)
            {
                rootElement._Elements = (DomElementList)list.CloneList();
            }
            else
            {
                rootElement._Elements.Clear();
                rootElement._Elements.AddRangeByDCList(list);
            }
            // 归还列表
            list.FastClear();
            LoaderListBuffer<DomElementList>.Instance.Return(list);
        }

        private void ReadXTextTextElement(XmlReader thisReader, DomElementList list)
        {
            int styleIndex = -1;
            int whiteSpaceCount = 0;
            int whiteSpaceLength = 0;
            while (thisReader.MoveToNextAttribute())
            {
                string _ReaderLocalName = thisReader.LocalName;
                if (((object)_ReaderLocalName == (object)id3_StyleIndex))
                {
                    styleIndex = DCSystemXml.XmlConvert.ToInt32(thisReader.Value);
                }
                else if (((object)_ReaderLocalName == (object)id1062_WhitespaceCount))
                {
                    whiteSpaceCount = DCSystemXml.XmlConvert.ToInt32(thisReader.Value);
                }
                else if (((object)_ReaderLocalName == (object)id1063_WhiteSpaceLength))
                {
                    whiteSpaceLength = DCSystemXml.XmlConvert.ToInt32(thisReader.Value);
                }
            }
            thisReader.MoveToElement();
            if (whiteSpaceCount > 0)
            {
                thisReader.Skip();
                for (var iCount = 0; iCount < whiteSpaceCount; iCount++)
                {
                    list.AddRaw(new DomCharElement(' ', styleIndex));
                }
                _ContentElementCount += whiteSpaceCount;
                return  ;
            }
            if (thisReader.IsEmptyElement)
            {
                thisReader.Skip();
                return  ;
            }
            thisReader.ReadStartElement();
            thisReader.MoveToContent();
            int whileIterations355 = 0;
            int readerCount355 = ReaderCount;
            var _ReaderNodeType1 = thisReader.NodeType;
            string strTextValue = null;
            while (_ReaderNodeType1 != DCSystemXml.XmlNodeType.EndElement && _ReaderNodeType1 != DCSystemXml.XmlNodeType.None)
            {
                if (_ReaderNodeType1 == DCSystemXml.XmlNodeType.Element)
                {
                    if (thisReader.LocalName == (object)id146_Text)
                    {
                        strTextValue = thisReader.ReadElementString();
                        break;
                    }
                    else
                    {
                        thisReader.Skip();
                    }
                }
                thisReader.MoveToContent();
                CheckReaderCount(ref whileIterations355, ref readerCount355);
                _ReaderNodeType1 = thisReader.NodeType;
            }
            ReadEndElement();
            if (strTextValue != null && strTextValue.Length > 0)
            {
                var len = strTextValue.Length;
                _ContentElementCount += len;
                list.EnsureCapacity(list.Count + len);
                for (var iCount = 0; iCount < len; iCount++)
                {
                    var c = strTextValue[iCount];
                    if (c >= 32)
                    {
                        c = WASMStarter.FixCharValueForLoad(c);
                        list.FastAdd2(new DomCharElement(c, styleIndex));
                    }
                    else if (c == '\r')
                    {
                        var p = new DomParagraphFlagElement();
                        p._StyleIndex = styleIndex;
                        list.FastAdd2(p);
                    }
                    else if (c <= 8 || c == 31)
                    {
                        list.FastAdd2(new DomCharElement(' ', styleIndex));
                    }
                    else
                    {
                        list.FastAdd2(new DomCharElement(c, styleIndex));
                    }
                }
            }
        }

        private System.Collections.Generic.Dictionary<string, string> _StringValues
            = new System.Collections.Generic.Dictionary<string, string>();
        private string CacheString(string v)
        {
            if (v == null || v.Length == 0)
            {
                return v;
            }
            string result = v;
            if (_StringValues.TryGetValue(v, out result) == false)
            {
                _StringValues[v] = v;
                result = v;
            }
            return result;
        }

        public object Read261_XTextDocument()
        {
            var ThisReader = this.Reader;
            object o = null;
            ThisReader.MoveToContent();
            if (ThisReader.NodeType == DCSystemXml.XmlNodeType.Element)
            {
                string _ReaderLocalName = ThisReader.LocalName;
                if (((object)_ReaderLocalName == (object)id1_XTextDocument))
                {
                    o = Read180_XTextDocument(true, true);
                }
                else
                {
                    throw CreateUnknownNodeException();
                }
            }
            else
            {
                UnknownNode(null, ThisReader.LocalName);
            }
            
            this._StringValues.Clear();
            this._StringValues = new Dictionary<string, string>();
            LoaderListBuffer<DomElementList>.Instance.Clear(delegate (DomElementList item2) { item2.ClearAndEmptyAll(); });
            _Cached_ListItems.ClearAndEmptyAll();
            return o;
        }
        internal protected DCSoft.Writer.Dom.DomDocument Read180_XTextDocument(bool isNullable, bool checkType)
        {
            var ThisReader = this.Reader;
            DCSoft.Writer.Dom.DomDocument o = new DomDocument(1);
    
            while (ThisReader.MoveToNextAttribute())
            {
                string _ReaderLocalName = ThisReader.LocalName;
                if (((object)_ReaderLocalName == (object)id3_StyleIndex))
                {
                    o.@StyleIndex = DCSystemXml.XmlConvert.ToInt32(ThisReader.Value);
                }
                else if (((object)_ReaderLocalName == (object)id7_EditorVersionString))
                {
                    o.@EditorVersionString = ThisReader.Value;
                }
            }
            ThisReader.MoveToElement();
            if (ThisReader.IsEmptyElement)
            {
                ThisReader.Skip();
                return o;
            }
            ThisReader.ReadStartElement();
            ThisReader.MoveToContent();
            int whileIterations0 = 0;
            int readerCount0 = ReaderCount;
            var _ReaderNodeType1 = ThisReader.NodeType;
            while (_ReaderNodeType1 != DCSystemXml.XmlNodeType.EndElement && _ReaderNodeType1 != DCSystemXml.XmlNodeType.None)
            {
                if (_ReaderNodeType1 == DCSystemXml.XmlNodeType.Element)
                {
                    string _ReaderLocalName = ThisReader.LocalName;
                    if (((object)_ReaderLocalName == (object)id11_ID))
                    {
                        {
                            o.@ID = CacheString(ThisReader.ReadElementString());
                        }
                    }
                    else if (((object)_ReaderLocalName == (object)id25_ToolTip))
                    {
                        {
                            o.@ToolTip = CacheString(ThisReader.ReadElementString());
                        }
                    }
                    else if (((object)_ReaderLocalName == (object)id26_AcceptTab))
                    {
                        if (ThisReader.IsEmptyElement)
                        {
                            ThisReader.Skip();
                        }
                        else
                        {
                            o.@AcceptTab = DCSystemXml.XmlConvert.ToBoolean(ThisReader.ReadElementString());
                        }
                    }
                    else if (((object)_ReaderLocalName == (object)id32_ValidateStyle))
                    {
                        o.@ValidateStyle = Read14_ValueValidateStyle(false, true);
                    }
                    else if (((object)_ReaderLocalName == (object)id50_XElements))
                    {
                        ReadContainerChildElements(ThisReader, o);
                    }
                    else if (((object)_ReaderLocalName == (object)id54_Visible))
                    {
                        if (ThisReader.IsEmptyElement)
                        {
                            ThisReader.Skip();
                        }
                        else
                        {
                            o.@Visible = DCSystemXml.XmlConvert.ToBoolean(ThisReader.ReadElementString());
                        }
                    }
                    else if (((object)_ReaderLocalName == (object)id68_FileName))
                    {
                        {
                            o.@FileName = CacheString(ThisReader.ReadElementString());
                        }
                    }
                    else if (((object)_ReaderLocalName == (object)id69_FileFormat))
                    {
                        {
                            o.@FileFormat = CacheString(ThisReader.ReadElementString());
                        }
                    }
                    else if (((object)_ReaderLocalName == (object)id73_ContentStyles))
                    {
                        o.@ContentStyles = Read51_DocumentContentStyleContainer(false, true);
                    }
                    else if (((object)_ReaderLocalName == (object)id83_Info))
                    {
                        o.@Info = Read56_DocumentInfo(false, true);
                    }
                    else if (((object)_ReaderLocalName == (object)id84_BodyText))
                    {
                        {
                            o.@BodyText = CacheString(ThisReader.ReadElementString());
                        }
                    }
                    else if (((object)_ReaderLocalName == (object)id90_PageSettings))
                    {
                        o.@PageSettings = Read71_XPageSettings(false, true);
                    }
                    else
                    {
                        UnknownNode(o, _ReaderLocalName);
                    }
                }
                else
                {
                    UnknownNode(o, ThisReader.LocalName);
                }
                ThisReader.MoveToContent();
                CheckReaderCount(ref whileIterations0, ref readerCount0);
                _ReaderNodeType1 = ThisReader.NodeType;
            }
            ReadEndElement();
            return o;
        }
        internal protected DCSoft.Printing.XPageSettings Read71_XPageSettings(bool isNullable, bool checkType)
        {
            var ThisReader = this.Reader;
            DCSoft.Printing.XPageSettings o;
            o = new DCSoft.Printing.XPageSettings();
            ThisReader.MoveToElement();
            if (ThisReader.IsEmptyElement)
            {
                ThisReader.Skip();
                return o;
            }
            ThisReader.ReadStartElement();
            ThisReader.MoveToContent();
            int whileIterations14 = 0;
            int readerCount14 = ReaderCount;
            var _ReaderNodeType1 = ThisReader.NodeType;
            while (_ReaderNodeType1 != DCSystemXml.XmlNodeType.EndElement && _ReaderNodeType1 != DCSystemXml.XmlNodeType.None)
            {
                if (_ReaderNodeType1 == DCSystemXml.XmlNodeType.Element)
                {
                    string _ReaderLocalName = ThisReader.LocalName;
                    if (((object)_ReaderLocalName == (object)id102_PowerDocumentGridLine))
                    {
                        if (ThisReader.IsEmptyElement)
                        {
                            ThisReader.Skip();
                        }
                        else
                        {
                            o.@PowerDocumentGridLine = DCSystemXml.XmlConvert.ToBoolean(ThisReader.ReadElementString());
                        }
                    }
                    else if (((object)_ReaderLocalName == (object)id103_DocumentGridLine))
                    {
                        o.@DocumentGridLine = Read63_DCGridLineInfo(false, true);
                    }
                    else if (((object)_ReaderLocalName == (object)id117_PaperKind))
                    {
                        if (ThisReader.IsEmptyElement)
                        {
                            ThisReader.Skip();
                        }
                        else
                        {
                            o.@PaperKind = Read70_PaperKind(ThisReader.ReadElementString());
                        }
                    }
                    else if (((object)_ReaderLocalName == (object)id121_HeaderDistance))
                    {
                        if (ThisReader.IsEmptyElement)
                        {
                            ThisReader.Skip();
                        }
                        else
                        {
                            o.@HeaderDistance = DCSystemXml.XmlConvert.ToInt32(ThisReader.ReadElementString());
                        }
                    }
                    else if (((object)_ReaderLocalName == (object)id122_FooterDistance))
                    {
                        if (ThisReader.IsEmptyElement)
                        {
                            ThisReader.Skip();
                        }
                        else
                        {
                            o.@FooterDistance = DCSystemXml.XmlConvert.ToInt32(ThisReader.ReadElementString());
                        }
                    }
                    else if (((object)_ReaderLocalName == (object)id123_DesignerPaperWidth))
                    {
                        if (ThisReader.IsEmptyElement)
                        {
                            ThisReader.Skip();
                        }
                        else
                        {
                            o.@DesignerPaperWidth = DCSystemXml.XmlConvert.ToInt32(ThisReader.ReadElementString());
                        }
                    }
                    else if (((object)_ReaderLocalName == (object)id124_PaperWidth))
                    {
                        if (ThisReader.IsEmptyElement)
                        {
                            ThisReader.Skip();
                        }
                        else
                        {
                            o.@PaperWidth = DCSystemXml.XmlConvert.ToInt32(ThisReader.ReadElementString());
                        }
                    }
                    else if (((object)_ReaderLocalName == (object)id125_DesignerPaperHeight))
                    {
                        if (ThisReader.IsEmptyElement)
                        {
                            ThisReader.Skip();
                        }
                        else
                        {
                            o.@DesignerPaperHeight = DCSystemXml.XmlConvert.ToInt32(ThisReader.ReadElementString());
                        }
                    }
                    else if (((object)_ReaderLocalName == (object)id126_PaperHeight))
                    {
                        if (ThisReader.IsEmptyElement)
                        {
                            ThisReader.Skip();
                        }
                        else
                        {
                            o.@PaperHeight = DCSystemXml.XmlConvert.ToInt32(ThisReader.ReadElementString());
                        }
                    }
                    else if (((object)_ReaderLocalName == (object)id127_LeftMargin))
                    {
                        if (ThisReader.IsEmptyElement)
                        {
                            ThisReader.Skip();
                        }
                        else
                        {
                            o.@LeftMargin = DCSystemXml.XmlConvert.ToInt32(ThisReader.ReadElementString());
                        }
                    }
                    else if (((object)_ReaderLocalName == (object)id128_TopMargin))
                    {
                        if (ThisReader.IsEmptyElement)
                        {
                            ThisReader.Skip();
                        }
                        else
                        {
                            o.@TopMargin = DCSystemXml.XmlConvert.ToInt32(ThisReader.ReadElementString());
                        }
                    }
                    else if (((object)_ReaderLocalName == (object)id129_RightMargin))
                    {
                        if (ThisReader.IsEmptyElement)
                        {
                            ThisReader.Skip();
                        }
                        else
                        {
                            o.@RightMargin = DCSystemXml.XmlConvert.ToInt32(ThisReader.ReadElementString());
                        }
                    }
                    else if (((object)_ReaderLocalName == (object)id130_BottomMargin))
                    {
                        if (ThisReader.IsEmptyElement)
                        {
                            ThisReader.Skip();
                        }
                        else
                        {
                            o.@BottomMargin = DCSystemXml.XmlConvert.ToInt32(ThisReader.ReadElementString());
                        }
                    }
                    else if (((object)_ReaderLocalName == (object)id131_Landscape))
                    {
                        if (ThisReader.IsEmptyElement)
                        {
                            ThisReader.Skip();
                        }
                        else
                        {
                            o.@Landscape = DCSystemXml.XmlConvert.ToBoolean(ThisReader.ReadElementString());
                        }
                    }
                    else
                    {
                        UnknownNode(o, _ReaderLocalName);
                    }
                }
                else
                {
                    UnknownNode(o, ThisReader.LocalName);
                }
                ThisReader.MoveToContent();
                CheckReaderCount(ref whileIterations14, ref readerCount14);
                _ReaderNodeType1 = ThisReader.NodeType;
            }
            ReadEndElement();
            return o;
        }
        internal protected PaperKind Read70_PaperKind(string s)
        {

            if (__System_Drawing_Printing_PaperKind == null)
            {
                {
                    var dic20200818 = new System.Collections.Generic.Dictionary<string, PaperKind>();
                    dic20200818.Add("Custom", PaperKind.@Custom);
                    dic20200818.Add("Letter", PaperKind.@Letter);
                    dic20200818.Add("Legal", PaperKind.@Legal);
                    dic20200818.Add("A4", PaperKind.@A4);
                    dic20200818.Add("CSheet", PaperKind.@CSheet);
                    dic20200818.Add("DSheet", PaperKind.@DSheet);
                    dic20200818.Add("ESheet", PaperKind.@ESheet);
                    dic20200818.Add("LetterSmall", PaperKind.@LetterSmall);
                    dic20200818.Add("Tabloid", PaperKind.@Tabloid);
                    dic20200818.Add("Ledger", PaperKind.@Ledger);
                    dic20200818.Add("Statement", PaperKind.@Statement);
                    dic20200818.Add("Executive", PaperKind.@Executive);
                    dic20200818.Add("A3", PaperKind.@A3);
                    dic20200818.Add("A4Small", PaperKind.@A4Small);
                    dic20200818.Add("A5", PaperKind.@A5);
                    dic20200818.Add("B4", PaperKind.@B4);
                    dic20200818.Add("B5", PaperKind.@B5);
                    dic20200818.Add("Folio", PaperKind.@Folio);
                    dic20200818.Add("Quarto", PaperKind.@Quarto);
                    dic20200818.Add("Standard10x14", PaperKind.@Standard10x14);
                    dic20200818.Add("Standard11x17", PaperKind.@Standard11x17);
                    dic20200818.Add("Note", PaperKind.@Note);
                    dic20200818.Add("Number9Envelope", PaperKind.@Number9Envelope);
                    dic20200818.Add("Number10Envelope", PaperKind.@Number10Envelope);
                    dic20200818.Add("Number11Envelope", PaperKind.@Number11Envelope);
                    dic20200818.Add("Number12Envelope", PaperKind.@Number12Envelope);
                    dic20200818.Add("Number14Envelope", PaperKind.@Number14Envelope);
                    dic20200818.Add("DLEnvelope", PaperKind.@DLEnvelope);
                    dic20200818.Add("C5Envelope", PaperKind.@C5Envelope);
                    dic20200818.Add("C3Envelope", PaperKind.@C3Envelope);
                    dic20200818.Add("C4Envelope", PaperKind.@C4Envelope);
                    dic20200818.Add("C6Envelope", PaperKind.@C6Envelope);
                    dic20200818.Add("C65Envelope", PaperKind.@C65Envelope);
                    dic20200818.Add("B4Envelope", PaperKind.@B4Envelope);
                    dic20200818.Add("B5Envelope", PaperKind.@B5Envelope);
                    dic20200818.Add("B6Envelope", PaperKind.@B6Envelope);
                    dic20200818.Add("ItalyEnvelope", PaperKind.@ItalyEnvelope);
                    dic20200818.Add("MonarchEnvelope", PaperKind.@MonarchEnvelope);
                    dic20200818.Add("PersonalEnvelope", PaperKind.@PersonalEnvelope);
                    dic20200818.Add("USStandardFanfold", PaperKind.@USStandardFanfold);
                    dic20200818.Add("GermanStandardFanfold", PaperKind.@GermanStandardFanfold);
                    dic20200818.Add("GermanLegalFanfold", PaperKind.@GermanLegalFanfold);
                    dic20200818.Add("IsoB4", PaperKind.@IsoB4);
                    dic20200818.Add("JapanesePostcard", PaperKind.@JapanesePostcard);
                    dic20200818.Add("Standard9x11", PaperKind.@Standard9x11);
                    dic20200818.Add("Standard10x11", PaperKind.@Standard10x11);
                    dic20200818.Add("Standard15x11", PaperKind.@Standard15x11);
                    dic20200818.Add("InviteEnvelope", PaperKind.@InviteEnvelope);
                    dic20200818.Add("LetterExtra", PaperKind.@LetterExtra);
                    dic20200818.Add("LegalExtra", PaperKind.@LegalExtra);
                    dic20200818.Add("TabloidExtra", PaperKind.@TabloidExtra);
                    dic20200818.Add("A4Extra", PaperKind.@A4Extra);
                    dic20200818.Add("LetterTransverse", PaperKind.@LetterTransverse);
                    dic20200818.Add("A4Transverse", PaperKind.@A4Transverse);
                    dic20200818.Add("LetterExtraTransverse", PaperKind.@LetterExtraTransverse);
                    dic20200818.Add("APlus", PaperKind.@APlus);
                    dic20200818.Add("BPlus", PaperKind.@BPlus);
                    dic20200818.Add("LetterPlus", PaperKind.@LetterPlus);
                    dic20200818.Add("A4Plus", PaperKind.@A4Plus);
                    dic20200818.Add("A5Transverse", PaperKind.@A5Transverse);
                    dic20200818.Add("B5Transverse", PaperKind.@B5Transverse);
                    dic20200818.Add("A3Extra", PaperKind.@A3Extra);
                    dic20200818.Add("A5Extra", PaperKind.@A5Extra);
                    dic20200818.Add("B5Extra", PaperKind.@B5Extra);
                    dic20200818.Add("A2", PaperKind.@A2);
                    dic20200818.Add("A3Transverse", PaperKind.@A3Transverse);
                    dic20200818.Add("A3ExtraTransverse", PaperKind.@A3ExtraTransverse);
                    dic20200818.Add("JapaneseDoublePostcard", PaperKind.@JapaneseDoublePostcard);
                    dic20200818.Add("A6", PaperKind.@A6);
                    dic20200818.Add("JapaneseEnvelopeKakuNumber2", PaperKind.@JapaneseEnvelopeKakuNumber2);
                    dic20200818.Add("JapaneseEnvelopeKakuNumber3", PaperKind.@JapaneseEnvelopeKakuNumber3);
                    dic20200818.Add("JapaneseEnvelopeChouNumber3", PaperKind.@JapaneseEnvelopeChouNumber3);
                    dic20200818.Add("JapaneseEnvelopeChouNumber4", PaperKind.@JapaneseEnvelopeChouNumber4);
                    dic20200818.Add("LetterRotated", PaperKind.@LetterRotated);
                    dic20200818.Add("A3Rotated", PaperKind.@A3Rotated);
                    dic20200818.Add("A4Rotated", PaperKind.@A4Rotated);
                    dic20200818.Add("A5Rotated", PaperKind.@A5Rotated);
                    dic20200818.Add("B4JisRotated", PaperKind.@B4JisRotated);
                    dic20200818.Add("B5JisRotated", PaperKind.@B5JisRotated);
                    dic20200818.Add("JapanesePostcardRotated", PaperKind.@JapanesePostcardRotated);
                    dic20200818.Add("JapaneseDoublePostcardRotated", PaperKind.@JapaneseDoublePostcardRotated);
                    dic20200818.Add("A6Rotated", PaperKind.@A6Rotated);
                    dic20200818.Add("JapaneseEnvelopeKakuNumber2Rotated", PaperKind.@JapaneseEnvelopeKakuNumber2Rotated);
                    dic20200818.Add("JapaneseEnvelopeKakuNumber3Rotated", PaperKind.@JapaneseEnvelopeKakuNumber3Rotated);
                    dic20200818.Add("JapaneseEnvelopeChouNumber3Rotated", PaperKind.@JapaneseEnvelopeChouNumber3Rotated);
                    dic20200818.Add("JapaneseEnvelopeChouNumber4Rotated", PaperKind.@JapaneseEnvelopeChouNumber4Rotated);
                    dic20200818.Add("B6Jis", PaperKind.@B6Jis);
                    dic20200818.Add("B6JisRotated", PaperKind.@B6JisRotated);
                    dic20200818.Add("Standard12x11", PaperKind.@Standard12x11);
                    dic20200818.Add("JapaneseEnvelopeYouNumber4", PaperKind.@JapaneseEnvelopeYouNumber4);
                    dic20200818.Add("JapaneseEnvelopeYouNumber4Rotated", PaperKind.@JapaneseEnvelopeYouNumber4Rotated);
                    dic20200818.Add("Prc16K", PaperKind.@Prc16K);
                    dic20200818.Add("Prc32K", PaperKind.@Prc32K);
                    dic20200818.Add("Prc32KBig", PaperKind.@Prc32KBig);
                    dic20200818.Add("PrcEnvelopeNumber1", PaperKind.@PrcEnvelopeNumber1);
                    dic20200818.Add("PrcEnvelopeNumber2", PaperKind.@PrcEnvelopeNumber2);
                    dic20200818.Add("PrcEnvelopeNumber3", PaperKind.@PrcEnvelopeNumber3);
                    dic20200818.Add("PrcEnvelopeNumber4", PaperKind.@PrcEnvelopeNumber4);
                    dic20200818.Add("PrcEnvelopeNumber5", PaperKind.@PrcEnvelopeNumber5);
                    dic20200818.Add("PrcEnvelopeNumber6", PaperKind.@PrcEnvelopeNumber6);
                    dic20200818.Add("PrcEnvelopeNumber7", PaperKind.@PrcEnvelopeNumber7);
                    dic20200818.Add("PrcEnvelopeNumber8", PaperKind.@PrcEnvelopeNumber8);
                    dic20200818.Add("PrcEnvelopeNumber9", PaperKind.@PrcEnvelopeNumber9);
                    dic20200818.Add("PrcEnvelopeNumber10", PaperKind.@PrcEnvelopeNumber10);
                    dic20200818.Add("Prc16KRotated", PaperKind.@Prc16KRotated);
                    dic20200818.Add("Prc32KRotated", PaperKind.@Prc32KRotated);
                    dic20200818.Add("Prc32KBigRotated", PaperKind.@Prc32KBigRotated);
                    dic20200818.Add("PrcEnvelopeNumber1Rotated", PaperKind.@PrcEnvelopeNumber1Rotated);
                    dic20200818.Add("PrcEnvelopeNumber2Rotated", PaperKind.@PrcEnvelopeNumber2Rotated);
                    dic20200818.Add("PrcEnvelopeNumber3Rotated", PaperKind.@PrcEnvelopeNumber3Rotated);
                    dic20200818.Add("PrcEnvelopeNumber4Rotated", PaperKind.@PrcEnvelopeNumber4Rotated);
                    dic20200818.Add("PrcEnvelopeNumber5Rotated", PaperKind.@PrcEnvelopeNumber5Rotated);
                    dic20200818.Add("PrcEnvelopeNumber6Rotated", PaperKind.@PrcEnvelopeNumber6Rotated);
                    dic20200818.Add("PrcEnvelopeNumber7Rotated", PaperKind.@PrcEnvelopeNumber7Rotated);
                    dic20200818.Add("PrcEnvelopeNumber8Rotated", PaperKind.@PrcEnvelopeNumber8Rotated);
                    dic20200818.Add("PrcEnvelopeNumber9Rotated", PaperKind.@PrcEnvelopeNumber9Rotated);
                    dic20200818.Add("PrcEnvelopeNumber10Rotated", PaperKind.@PrcEnvelopeNumber10Rotated);
                    __System_Drawing_Printing_PaperKind = dic20200818;
                }
            }
            var result = default(PaperKind);
            if (__System_Drawing_Printing_PaperKind.TryGetValue(s, out result))
            {
                return result;
            }
            else
            {
                return default(PaperKind);
            }
        }
        private static System.Collections.Generic.Dictionary<string, PaperKind> __System_Drawing_Printing_PaperKind = null;
        internal protected DCSoft.Drawing.XImageValue Read34_XImageValue(bool isNullable, bool checkType)
        {
            var ThisReader = this.Reader;
            ThisReader.MoveToElement();
            if (ThisReader.IsEmptyElement)
            {
                ThisReader.Skip();
                return null;
            }
            DCSoft.Drawing.XImageValue o;
            o = new DCSoft.Drawing.XImageValue();
            ThisReader.ReadStartElement();
            ThisReader.MoveToContent();
            int whileIterations15 = 0;
            int readerCount15 = ReaderCount;
            var _ReaderNodeType1 = ThisReader.NodeType;
            while (_ReaderNodeType1 != DCSystemXml.XmlNodeType.EndElement && _ReaderNodeType1 != DCSystemXml.XmlNodeType.None)
            {
                if (_ReaderNodeType1 == DCSystemXml.XmlNodeType.Element)
                {
                    string _ReaderLocalName = ThisReader.LocalName;
                    if (((object)_ReaderLocalName == (object)id137_ImageDataBase64String))
                    {
                        {
                            DCSoft.Writer.WriterUtils.ReadImageDataBase64String(ThisReader, o);
                        }
                    }
                    else
                    {
                        UnknownNode(o, _ReaderLocalName);
                    }
                }
                else
                {
                    UnknownNode(o, ThisReader.LocalName);
                }
                ThisReader.MoveToContent();
                CheckReaderCount(ref whileIterations15, ref readerCount15);
                _ReaderNodeType1 = ThisReader.NodeType;
            }
            ReadEndElement();
            return o;
        }
        internal protected DCSoft.Drawing.ParagraphListStyle Read44_ParagraphListStyle(string s)
        {

            if (__DCSoft_Drawing_ParagraphListStyle == null)
            {
                {
                    var dic20200818 = new System.Collections.Generic.Dictionary<string, DCSoft.Drawing.ParagraphListStyle>();
                    dic20200818.Add("None", DCSoft.Drawing.ParagraphListStyle.@None);
                    dic20200818.Add("ListNumberStyle", DCSoft.Drawing.ParagraphListStyle.@ListNumberStyle);
                    dic20200818.Add("ListNumberStyleArabic1", DCSoft.Drawing.ParagraphListStyle.@ListNumberStyleArabic1);
                    dic20200818.Add("ListNumberStyleArabic2", DCSoft.Drawing.ParagraphListStyle.@ListNumberStyleArabic2);
                    dic20200818.Add("ListNumberStyleArabic3", DCSoft.Drawing.ParagraphListStyle.@ListNumberStyleArabic3);
                    dic20200818.Add("ListNumberStyleLowercaseLetter", DCSoft.Drawing.ParagraphListStyle.@ListNumberStyleLowercaseLetter);
                    dic20200818.Add("ListNumberStyleLowercaseRoman", DCSoft.Drawing.ParagraphListStyle.@ListNumberStyleLowercaseRoman);
                    dic20200818.Add("ListNumberStyleNumberInCircle", DCSoft.Drawing.ParagraphListStyle.@ListNumberStyleNumberInCircle);
                    dic20200818.Add("ListNumberStyleUppercaseLetter", DCSoft.Drawing.ParagraphListStyle.@ListNumberStyleUppercaseLetter);
                    dic20200818.Add("ListNumberStyleUppercaseRoman", DCSoft.Drawing.ParagraphListStyle.@ListNumberStyleUppercaseRoman);
                    dic20200818.Add("ListNumberStyleZodiac1", DCSoft.Drawing.ParagraphListStyle.@ListNumberStyleZodiac1);
                    dic20200818.Add("ListNumberStyleZodiac2", DCSoft.Drawing.ParagraphListStyle.@ListNumberStyleZodiac2);
                    dic20200818.Add("BulletedList", DCSoft.Drawing.ParagraphListStyle.@BulletedList);
                    dic20200818.Add("BulletedListBlock", DCSoft.Drawing.ParagraphListStyle.@BulletedListBlock);
                    dic20200818.Add("BulletedListDiamond", DCSoft.Drawing.ParagraphListStyle.@BulletedListDiamond);
                    dic20200818.Add("BulletedListCheck", DCSoft.Drawing.ParagraphListStyle.@BulletedListCheck);
                    dic20200818.Add("BulletedListRightArrow", DCSoft.Drawing.ParagraphListStyle.@BulletedListRightArrow);
                    dic20200818.Add("BulletedListHollowStar", DCSoft.Drawing.ParagraphListStyle.@BulletedListHollowStar);
                    __DCSoft_Drawing_ParagraphListStyle = dic20200818;
                }
            }
            var result = default(DCSoft.Drawing.ParagraphListStyle);
            if (__DCSoft_Drawing_ParagraphListStyle.TryGetValue(s, out result))
            {
                return result;
            }
            else
            {
                return default(DCSoft.Drawing.ParagraphListStyle);
            }
        }
        private static System.Collections.Generic.Dictionary<string, DCSoft.Drawing.ParagraphListStyle> __DCSoft_Drawing_ParagraphListStyle = null;

        internal protected DashStyle Read43_DashStyle(string s)
        {

            if (__System_Drawing_Drawing2D_DashStyle == null)
            {
                {
                    var dic20200818 = new System.Collections.Generic.Dictionary<string, DashStyle>();
                    dic20200818.Add("Solid", DashStyle.@Solid);
                    dic20200818.Add("Dash", DashStyle.@Dash);
                    dic20200818.Add("Dot", DashStyle.@Dot);
                    dic20200818.Add("DashDot", DashStyle.@DashDot);
                    dic20200818.Add("DashDotDot", DashStyle.@DashDotDot);
                    dic20200818.Add("Custom", DashStyle.@Custom);
                    __System_Drawing_Drawing2D_DashStyle = dic20200818;
                }
            }
            var result = default(DashStyle);
            if (__System_Drawing_Drawing2D_DashStyle.TryGetValue(s, out result))
            {
                return result;
            }
            else
            {
                return default(DashStyle);
            }
        }
        private static System.Collections.Generic.Dictionary<string, DashStyle> __System_Drawing_Drawing2D_DashStyle = null;
        internal protected DCSoft.Drawing.VerticalAlignStyle Read40_VerticalAlignStyle(string s)
        {
            switch (s)
            {
                case @"Top": return DCSoft.Drawing.VerticalAlignStyle.@Top;
                case @"Middle": return DCSoft.Drawing.VerticalAlignStyle.@Middle;
                case @"Bottom": return DCSoft.Drawing.VerticalAlignStyle.@Bottom;
                default: return (default(DCSoft.Drawing.VerticalAlignStyle));
            }
        }
        internal protected DCSoft.Drawing.DocumentContentAlignment Read39_DocumentContentAlignment(string s)
        {
            switch (s)
            {
                case @"Left": return DCSoft.Drawing.DocumentContentAlignment.@Left;
                case @"Center": return DCSoft.Drawing.DocumentContentAlignment.@Center;
                case @"Right": return DCSoft.Drawing.DocumentContentAlignment.@Right;
                case @"Justify": return DCSoft.Drawing.DocumentContentAlignment.@Justify;
                case @"Distribute": return DCSoft.Drawing.DocumentContentAlignment.@Distribute;
                default: return (default(DCSoft.Drawing.DocumentContentAlignment));
            }
        }
        internal protected DCSoft.Drawing.LineSpacingStyle Read38_LineSpacingStyle(string s)
        {

            if (__DCSoft_Drawing_LineSpacingStyle == null)
            {
                {
                    var dic20200818 = new System.Collections.Generic.Dictionary<string, DCSoft.Drawing.LineSpacingStyle>();
                    dic20200818.Add("SpaceSingle", DCSoft.Drawing.LineSpacingStyle.@SpaceSingle);
                    dic20200818.Add("Space1pt5", DCSoft.Drawing.LineSpacingStyle.@Space1pt5);
                    dic20200818.Add("SpaceDouble", DCSoft.Drawing.LineSpacingStyle.@SpaceDouble);
                    dic20200818.Add("SpaceExactly", DCSoft.Drawing.LineSpacingStyle.@SpaceExactly);
                    dic20200818.Add("SpaceSpecify", DCSoft.Drawing.LineSpacingStyle.@SpaceSpecify);
                    dic20200818.Add("SpaceMultiple", DCSoft.Drawing.LineSpacingStyle.@SpaceMultiple);
                    __DCSoft_Drawing_LineSpacingStyle = dic20200818;
                }
            }
            var result = default(DCSoft.Drawing.LineSpacingStyle);
            if (__DCSoft_Drawing_LineSpacingStyle.TryGetValue(s, out result))
            {
                return result;
            }
            else
            {
                return default(DCSoft.Drawing.LineSpacingStyle);
            }
        }
        private static System.Collections.Generic.Dictionary<string, DCSoft.Drawing.LineSpacingStyle> __DCSoft_Drawing_LineSpacingStyle = null;

        internal protected DCSoft.Printing.DCGridLineInfo Read63_DCGridLineInfo(bool isNullable, bool checkType)
        {
            var ThisReader = this.Reader;
            ThisReader.MoveToElement();
            if (ThisReader.IsEmptyElement)
            {
                ThisReader.Skip();
                return null;
            }
            DCSoft.Printing.DCGridLineInfo o;
            o = new DCSoft.Printing.DCGridLineInfo();
            ThisReader.ReadStartElement();
            ThisReader.MoveToContent();
            int whileIterations21 = 0;
            int readerCount21 = ReaderCount;
            var _ReaderNodeType1 = ThisReader.NodeType;
            while (_ReaderNodeType1 != DCSystemXml.XmlNodeType.EndElement && _ReaderNodeType1 != DCSystemXml.XmlNodeType.None)
            {
                if (_ReaderNodeType1 == DCSystemXml.XmlNodeType.Element)
                {
                    string _ReaderLocalName = ThisReader.LocalName;
                    if (((object)_ReaderLocalName == (object)id54_Visible))
                    {
                        if (ThisReader.IsEmptyElement)
                        {
                            ThisReader.Skip();
                        }
                        else
                        {
                            o.@Visible = DCSystemXml.XmlConvert.ToBoolean(ThisReader.ReadElementString());
                        }
                    }
                    else if (((object)_ReaderLocalName == (object)id231_AlignToGridLine))
                    {
                        if (ThisReader.IsEmptyElement)
                        {
                            ThisReader.Skip();
                        }
                        else
                        {
                            o.@AlignToGridLine = DCSystemXml.XmlConvert.ToBoolean(ThisReader.ReadElementString());
                        }
                    }
                    else if (((object)_ReaderLocalName == (object)id143_ColorValue))
                    {
                        {
                            o.@ColorValue = CacheString(ThisReader.ReadElementString());
                        }
                    }
                    else if (((object)_ReaderLocalName == (object)id232_LineWidth))
                    {
                        if (ThisReader.IsEmptyElement)
                        {
                            ThisReader.Skip();
                        }
                        else
                        {
                            o.@LineWidth = XSerHelper_XTextDocument.ToSingle(ThisReader.ReadElementString());
                        }
                    }
                    else if (((object)_ReaderLocalName == (object)id233_GridNumInOnePage))
                    {
                        if (ThisReader.IsEmptyElement)
                        {
                            ThisReader.Skip();
                        }
                        else
                        {
                            o.@GridNumInOnePage = DCSystemXml.XmlConvert.ToInt32(ThisReader.ReadElementString());
                        }
                    }
                    else if (((object)_ReaderLocalName == (object)id234_GridSpanInCM))
                    {
                        if (ThisReader.IsEmptyElement)
                        {
                            ThisReader.Skip();
                        }
                        else
                        {
                            o.@GridSpanInCM = XSerHelper_XTextDocument.ToSingle(ThisReader.ReadElementString());
                        }
                    }
                    else if (((object)_ReaderLocalName == (object)id235_LineStyle))
                    {
                        if (ThisReader.IsEmptyElement)
                        {
                            ThisReader.Skip();
                        }
                        else
                        {
                            o.@LineStyle = Read43_DashStyle(ThisReader.ReadElementString());
                        }
                    }
                    else if (((object)_ReaderLocalName == (object)id236_Printable))
                    {
                        if (ThisReader.IsEmptyElement)
                        {
                            ThisReader.Skip();
                        }
                        else
                        {
                            o.@Printable = DCSystemXml.XmlConvert.ToBoolean(ThisReader.ReadElementString());
                        }
                    }
                    else
                    {
                        UnknownNode(o, _ReaderLocalName);
                    }
                }
                else
                {
                    UnknownNode(o, ThisReader.LocalName);
                }
                ThisReader.MoveToContent();
                CheckReaderCount(ref whileIterations21, ref readerCount21);
                _ReaderNodeType1 = ThisReader.NodeType;
            }
            ReadEndElement();
            return o;
        }
        internal protected DCSoft.Writer.Dom.DocumentInfo Read56_DocumentInfo(bool isNullable, bool checkType)
        {
            var ThisReader = this.Reader;
            ThisReader.MoveToElement();
            if (ThisReader.IsEmptyElement)
            {
                ThisReader.Skip();
                return null;
            }
            DCSoft.Writer.Dom.DocumentInfo o;
            o = new DCSoft.Writer.Dom.DocumentInfo();
            ThisReader.ReadStartElement();
            ThisReader.MoveToContent();
            int whileIterations26 = 0;
            int readerCount26 = ReaderCount;
            var _ReaderNodeType1 = ThisReader.NodeType;
            while (_ReaderNodeType1 != DCSystemXml.XmlNodeType.EndElement && _ReaderNodeType1 != DCSystemXml.XmlNodeType.None)
            {
                if (_ReaderNodeType1 == DCSystemXml.XmlNodeType.Element)
                {
                    string _ReaderLocalName = ThisReader.LocalName;
                    if (((object)_ReaderLocalName == (object)id251_Readonly))
                    {
                        if (ThisReader.IsEmptyElement)
                        {
                            ThisReader.Skip();
                        }
                        else
                        {
                            o.@Readonly = DCSystemXml.XmlConvert.ToBoolean(ThisReader.ReadElementString());
                        }
                    }
                    else if (((object)_ReaderLocalName == (object)id252_ShowHeaderBottomLine))
                    {
                        if (ThisReader.IsEmptyElement)
                        {
                            ThisReader.Skip();
                        }
                        else
                        {
                            o.@ShowHeaderBottomLine = Read18_DCBooleanValue(ThisReader.ReadElementString());
                        }
                    }
                    else if (((object)_ReaderLocalName == (object)id253_FieldBorderElementWidth))
                    {
                        if (ThisReader.IsEmptyElement)
                        {
                            ThisReader.Skip();
                        }
                        else
                        {
                            o.@FieldBorderElementWidth = XSerHelper_XTextDocument.ToSingle(ThisReader.ReadElementString());
                        }
                    }
                    else if (((object)_ReaderLocalName == (object)id11_ID))
                    {
                        {
                            o.@ID = CacheString(ThisReader.ReadElementString());
                        }
                    }
                    else if (((object)_ReaderLocalName == (object)id255_IsTemplate))
                    {
                        if (ThisReader.IsEmptyElement)
                        {
                            ThisReader.Skip();
                        }
                        else
                        {
                            o.@IsTemplate = DCSystemXml.XmlConvert.ToBoolean(ThisReader.ReadElementString());
                        }
                    }
                    else if (((object)_ReaderLocalName == (object)id258_Version))
                    {
                        {
                            o.@Version = CacheString(ThisReader.ReadElementString());
                        }
                    }
                    else if (((object)_ReaderLocalName == (object)id238_Title))
                    {
                        {
                            o.@Title = CacheString(ThisReader.ReadElementString());
                        }
                    }
                    else if (((object)_ReaderLocalName == (object)id259_Description))
                    {
                        {
                            o.@Description = CacheString(ThisReader.ReadElementString());
                        }
                    }
                    else if (((object)_ReaderLocalName == (object)id242_CreationTime))
                    {
                        {
                            o.@CreationTime = ToDateTime(ThisReader.ReadElementString());
                        }
                    }
                    else if (((object)_ReaderLocalName == (object)id261_LastModifiedTime))
                    {
                        {
                            o.@LastModifiedTime = ToDateTime(ThisReader.ReadElementString());
                        }
                    }
                    else if (((object)_ReaderLocalName == (object)id262_EditMinute))
                    {
                        if (ThisReader.IsEmptyElement)
                        {
                            ThisReader.Skip();
                        }
                        else
                        {
                            o.@EditMinute = DCSystemXml.XmlConvert.ToInt32(ThisReader.ReadElementString());
                        }
                    }
                    else if (((object)_ReaderLocalName == (object)id263_LastPrintTime))
                    {
                        {
                            o.@LastPrintTime = ToDateTime(ThisReader.ReadElementString());
                        }
                    }
                    else if (((object)_ReaderLocalName == (object)id241_Author))
                    {
                        {
                            o.@Author = CacheString(ThisReader.ReadElementString());
                        }
                    }
                    else if (((object)_ReaderLocalName == (object)id264_AuthorName))
                    {
                        {
                            o.@AuthorName = CacheString(ThisReader.ReadElementString());
                        }
                    }
                    else if (((object)_ReaderLocalName == (object)id266_DepartmentID))
                    {
                        {
                            o.@DepartmentID = CacheString(ThisReader.ReadElementString());
                        }
                    }
                    else if (((object)_ReaderLocalName == (object)id267_DepartmentName))
                    {
                        {
                            o.@DepartmentName = CacheString(ThisReader.ReadElementString());
                        }
                    }
                    else if (((object)_ReaderLocalName == (object)id268_DocumentFormat))
                    {
                        {
                            o.@DocumentFormat = CacheString(ThisReader.ReadElementString());
                        }
                    }
                    else if (((object)_ReaderLocalName == (object)id269_DocumentType))
                    {
                        {
                            o.@DocumentType = CacheString(ThisReader.ReadElementString());
                        }
                    }
                    else if (((object)_ReaderLocalName == (object)id270_DocumentProcessState))
                    {
                        if (ThisReader.IsEmptyElement)
                        {
                            ThisReader.Skip();
                        }
                        else
                        {
                            o.@DocumentProcessState = DCSystemXml.XmlConvert.ToInt32(ThisReader.ReadElementString());
                        }
                    }
                    else if (((object)_ReaderLocalName == (object)id271_DocumentEditState))
                    {
                        if (ThisReader.IsEmptyElement)
                        {
                            ThisReader.Skip();
                        }
                        else
                        {
                            o.@DocumentEditState = DCSystemXml.XmlConvert.ToInt32(ThisReader.ReadElementString());
                        }
                    }
                    else if (((object)_ReaderLocalName == (object)id86_Comment))
                    {
                        {
                            o.@Comment = CacheString(ThisReader.ReadElementString());
                        }
                    }
                    else if (((object)_ReaderLocalName == (object)id272_Operator))
                    {
                        {
                            o.@Operator = CacheString(ThisReader.ReadElementString());
                        }
                    }
                    else if (((object)_ReaderLocalName == (object)id273_NumOfPage))
                    {
                        if (ThisReader.IsEmptyElement)
                        {
                            ThisReader.Skip();
                        }
                        else
                        {
                            o.@NumOfPage = DCSystemXml.XmlConvert.ToInt32(ThisReader.ReadElementString());
                        }
                    }
                    else if (((object)_ReaderLocalName == (object)id236_Printable))
                    {
                        if (ThisReader.IsEmptyElement)
                        {
                            ThisReader.Skip();
                        }
                        else
                        {
                            o.@Printable = DCSystemXml.XmlConvert.ToBoolean(ThisReader.ReadElementString());
                        }
                    }
                    else if (((object)_ReaderLocalName == (object)id275_StartPositionInPringJob))
                    {
                        if (ThisReader.IsEmptyElement)
                        {
                            ThisReader.Skip();
                        }
                        else
                        {
                            o.@StartPositionInPringJob = DCSystemXml.XmlConvert.ToInt32(ThisReader.ReadElementString());
                        }
                    }
                    else if (((object)_ReaderLocalName == (object)id276_HeightInPrintJob))
                    {
                        if (ThisReader.IsEmptyElement)
                        {
                            ThisReader.Skip();
                        }
                        else
                        {
                            o.@HeightInPrintJob = DCSystemXml.XmlConvert.ToInt32(ThisReader.ReadElementString());
                        }
                    }
                    else
                    {
                        UnknownNode(o, _ReaderLocalName);
                    }
                }
                else
                {
                    UnknownNode(o, ThisReader.LocalName);
                }
                ThisReader.MoveToContent();
                CheckReaderCount(ref whileIterations26, ref readerCount26);
                _ReaderNodeType1 = ThisReader.NodeType;
            }
            ReadEndElement();
            return o;
        }
        internal protected DCSoft.Writer.DCBooleanValue Read18_DCBooleanValue(string s)
        {
            switch (s)
            {
                case @"True": return DCSoft.Writer.DCBooleanValue.@True;
                case @"False": return DCSoft.Writer.DCBooleanValue.@False;
                case @"Inherit": return DCSoft.Writer.DCBooleanValue.@Inherit;
                default: return (default(DCSoft.Writer.DCBooleanValue));
            }
        }
        internal protected DCSoft.Writer.Dom.DocumentContentStyleContainer Read51_DocumentContentStyleContainer(bool isNullable, bool checkType)
        {
            var ThisReader = this.Reader;
            DCSoft.Writer.Dom.DocumentContentStyleContainer o;
            o = new DCSoft.Writer.Dom.DocumentContentStyleContainer();
            ThisReader.MoveToElement();
            if (ThisReader.IsEmptyElement)
            {
                ThisReader.Skip();
                return o;
            }
            ThisReader.ReadStartElement();
            ThisReader.MoveToContent();
            int whileIterations30 = 0;
            int readerCount30 = ReaderCount;
            var _ReaderNodeType1 = ThisReader.NodeType;
            while (_ReaderNodeType1 != DCSystemXml.XmlNodeType.EndElement && _ReaderNodeType1 != DCSystemXml.XmlNodeType.None)
            {
                if (_ReaderNodeType1 == DCSystemXml.XmlNodeType.Element)
                {
                    string _ReaderLocalName = ThisReader.LocalName;
                    if (((object)_ReaderLocalName == (object)id291_Default))
                    {
                        o.@Default = Read50_DocumentContentStyle(false, true);
                    }
                    else if (((object)_ReaderLocalName == (object)id292_Styles))
                    {
                        {
                            if ((ThisReader.IsEmptyElement))
                            {
                                ThisReader.Skip();
                            }
                            else
                            {
                                if ((o.@Styles) == null) o.@Styles = new DCSoft.Drawing.ContentStyleList();
                                DCSoft.Drawing.ContentStyleList a_1_0 = o.@Styles;
                                ThisReader.ReadStartElement();
                                ThisReader.MoveToContent();
                                int whileIterations31 = 0;
                                int readerCount31 = ReaderCount;
                                var _ReaderNodeType2 = ThisReader.NodeType;
                                while (_ReaderNodeType2 != DCSystemXml.XmlNodeType.EndElement && _ReaderNodeType2 != DCSystemXml.XmlNodeType.None)
                                {
                                    if (_ReaderNodeType2 == DCSystemXml.XmlNodeType.Element)
                                    {
                                        if (((object)ThisReader.LocalName == (object)id293_Style))
                                        {
                                            if ((a_1_0) == null) ThisReader.Skip(); else a_1_0.Add(Read50_DocumentContentStyle(true, true));
                                        }
                                        else
                                        {
                                            UnknownNode(null, ThisReader.LocalName);
                                        }
                                    }
                                    else
                                    {
                                        UnknownNode(null, ThisReader.LocalName);
                                    }
                                    ThisReader.MoveToContent();
                                    CheckReaderCount(ref whileIterations31, ref readerCount31);
                                    _ReaderNodeType2 = ThisReader.NodeType;
                                }
                                ReadEndElement();
                            }
                        }
                    }
                    else
                    {
                        UnknownNode(o, _ReaderLocalName);
                    }
                }
                else
                {
                    UnknownNode(o, ThisReader.LocalName);
                }
                ThisReader.MoveToContent();
                CheckReaderCount(ref whileIterations30, ref readerCount30);
                _ReaderNodeType1 = ThisReader.NodeType;
            }
            ReadEndElement();
            return o;
        }
        internal protected DCSoft.Writer.Dom.DocumentContentStyle Read50_DocumentContentStyle(bool isNullable, bool checkType)
        {
            var ThisReader = this.Reader;
            DCSoft.Writer.Dom.DocumentContentStyle o;
            o = new DCSoft.Writer.Dom.DocumentContentStyle();
            while (ThisReader.MoveToNextAttribute())
            {
                string _ReaderLocalName = ThisReader.LocalName;
                if (((object)_ReaderLocalName == (object)id158_Index))
                {
                    o.@Index = DCSystemXml.XmlConvert.ToInt32(ThisReader.Value);
                }
            }
            ThisReader.MoveToElement();
            if (ThisReader.IsEmptyElement)
            {
                ThisReader.Skip();
                return o;
            }
            ThisReader.ReadStartElement();
            ThisReader.MoveToContent();
            int whileIterations32 = 0;
            int readerCount32 = ReaderCount;
            var _ReaderNodeType1 = ThisReader.NodeType;
            while (_ReaderNodeType1 != DCSystemXml.XmlNodeType.EndElement && _ReaderNodeType1 != DCSystemXml.XmlNodeType.None)
            {
                if (_ReaderNodeType1 == DCSystemXml.XmlNodeType.Element)
                {
                    string _ReaderLocalName = ThisReader.LocalName;
                    if (((object)_ReaderLocalName == (object)id159_BackgroundColor))
                    {
                        {
                            o.@BackgroundColorString = CacheString(ThisReader.ReadElementString());
                        }
                    }
                    else if (((object)_ReaderLocalName == (object)id168_Color))
                    {
                        {
                            o.@ColorString = CacheString(ThisReader.ReadElementString());
                        }
                    }
                    else if (((object)_ReaderLocalName == (object)id169_FontName))
                    {
                        {
                            o.@FontName = CacheString(ThisReader.ReadElementString());
                        }
                    }
                    else if (((object)_ReaderLocalName == (object)id170_FontSize))
                    {
                        if (ThisReader.IsEmptyElement)
                        {
                            ThisReader.Skip();
                        }
                        else
                        {
                            o.@FontSize = XSerHelper_XTextDocument.ToSingle(ThisReader.ReadElementString());
                        }
                    }
                     
                    else if (((object)_ReaderLocalName == (object)id153_Bold))
                    {
                        if (ThisReader.IsEmptyElement)
                        {
                            ThisReader.Skip();
                        }
                        else
                        {
                            o.@Bold = DCSystemXml.XmlConvert.ToBoolean(ThisReader.ReadElementString());
                        }
                    }
                    else if (((object)_ReaderLocalName == (object)id154_Italic))
                    {
                        if (ThisReader.IsEmptyElement)
                        {
                            ThisReader.Skip();
                        }
                        else
                        {
                            o.@Italic = DCSystemXml.XmlConvert.ToBoolean(ThisReader.ReadElementString());
                        }
                    }
                    else if (((object)_ReaderLocalName == (object)id155_Underline))
                    {
                        if (ThisReader.IsEmptyElement)
                        {
                            ThisReader.Skip();
                        }
                        else
                        {
                            o.@Underline = DCSystemXml.XmlConvert.ToBoolean(ThisReader.ReadElementString());
                        }
                    }
                    else if (((object)_ReaderLocalName == (object)id156_Strikeout))
                    {
                        if (ThisReader.IsEmptyElement)
                        {
                            ThisReader.Skip();
                        }
                        else
                        {
                            o.@Strikeout = DCSystemXml.XmlConvert.ToBoolean(ThisReader.ReadElementString());
                        }
                    }
                    else if (((object)_ReaderLocalName == (object)id174_Superscript))
                    {
                        if (ThisReader.IsEmptyElement)
                        {
                            ThisReader.Skip();
                        }
                        else
                        {
                            o.@Superscript = DCSystemXml.XmlConvert.ToBoolean(ThisReader.ReadElementString());
                        }
                    }
                    else if (((object)_ReaderLocalName == (object)id175_Subscript))
                    {
                        if (ThisReader.IsEmptyElement)
                        {
                            ThisReader.Skip();
                        }
                        else
                        {
                            o.@Subscript = DCSystemXml.XmlConvert.ToBoolean(ThisReader.ReadElementString());
                        }
                    }
                    else if (((object)_ReaderLocalName == (object)id177_SpacingAfterParagraph))
                    {
                        if (ThisReader.IsEmptyElement)
                        {
                            ThisReader.Skip();
                        }
                        else
                        {
                            o.@SpacingAfterParagraph = XSerHelper_XTextDocument.ToSingle(ThisReader.ReadElementString());
                        }
                    }
                    else if (((object)_ReaderLocalName == (object)id178_SpacingBeforeParagraph))
                    {
                        if (ThisReader.IsEmptyElement)
                        {
                            ThisReader.Skip();
                        }
                        else
                        {
                            o.@SpacingBeforeParagraph = XSerHelper_XTextDocument.ToSingle(ThisReader.ReadElementString());
                        }
                    }
                     
                    else if (((object)_ReaderLocalName == (object)id180_LineSpacingStyle))
                    {
                        if (ThisReader.IsEmptyElement)
                        {
                            ThisReader.Skip();
                        }
                        else
                        {
                            o.@LineSpacingStyle = Read38_LineSpacingStyle(ThisReader.ReadElementString());
                        }
                    }
                    else if (((object)_ReaderLocalName == (object)id182_LineSpacing))
                    {
                        if (ThisReader.IsEmptyElement)
                        {
                            ThisReader.Skip();
                        }
                        else
                        {
                            o.@LineSpacing = XSerHelper_XTextDocument.ToSingle(ThisReader.ReadElementString());
                        }
                    }
                    
                    else if (((object)_ReaderLocalName == (object)id184_Align))
                    {
                        if (ThisReader.IsEmptyElement)
                        {
                            ThisReader.Skip();
                        }
                        else
                        {
                            o.@Align = Read39_DocumentContentAlignment(ThisReader.ReadElementString());
                        }
                    }
                    else if (((object)_ReaderLocalName == (object)id185_VerticalAlign))
                    {
                        if (ThisReader.IsEmptyElement)
                        {
                            ThisReader.Skip();
                        }
                        else
                        {
                            o.@VerticalAlign = Read40_VerticalAlignStyle(ThisReader.ReadElementString());
                        }
                    }
                    else if (((object)_ReaderLocalName == (object)id186_FirstLineIndent))
                    {
                        if (ThisReader.IsEmptyElement)
                        {
                            ThisReader.Skip();
                        }
                        else
                        {
                            o.@FirstLineIndent = XSerHelper_XTextDocument.ToSingle(ThisReader.ReadElementString());
                        }
                    }
                    else if (((object)_ReaderLocalName == (object)id187_LeftIndent))
                    {
                        if (ThisReader.IsEmptyElement)
                        {
                            ThisReader.Skip();
                        }
                        else
                        {
                            o.@LeftIndent = XSerHelper_XTextDocument.ToSingle(ThisReader.ReadElementString());
                        }
                    }
                    else if (((object)_ReaderLocalName == (object)id195_BorderLeftColor))
                    {
                        {
                            o.@BorderLeftColorString = CacheString(ThisReader.ReadElementString());
                        }
                    }
                    else if (((object)_ReaderLocalName == (object)id196_BorderTopColor))
                    {
                        {
                            o.@BorderTopColorString = CacheString(ThisReader.ReadElementString());
                        }
                    }
                    else if (((object)_ReaderLocalName == (object)id197_BorderRightColor))
                    {
                        {
                            o.@BorderRightColorString = CacheString(ThisReader.ReadElementString());
                        }
                    }
                    else if (((object)_ReaderLocalName == (object)id198_BorderBottomColor))
                    {
                        {
                            o.@BorderBottomColorString = CacheString(ThisReader.ReadElementString());
                        }
                    }
                    else if (((object)_ReaderLocalName == (object)id199_BorderStyle))
                    {
                        if (ThisReader.IsEmptyElement)
                        {
                            ThisReader.Skip();
                        }
                        else
                        {
                            o.@BorderStyle = Read43_DashStyle(ThisReader.ReadElementString());
                        }
                    }
                    else if (((object)_ReaderLocalName == (object)id200_BorderWidth))
                    {
                        if (ThisReader.IsEmptyElement)
                        {
                            ThisReader.Skip();
                        }
                        else
                        {
                            o.@BorderWidth = XSerHelper_XTextDocument.ToSingle(ThisReader.ReadElementString());
                        }
                    }
                    else if (((object)_ReaderLocalName == (object)id201_BorderLeft))
                    {
                        if (ThisReader.IsEmptyElement)
                        {
                            ThisReader.Skip();
                        }
                        else
                        {
                            o.@BorderLeft = DCSystemXml.XmlConvert.ToBoolean(ThisReader.ReadElementString());
                        }
                    }
                    else if (((object)_ReaderLocalName == (object)id202_BorderBottom))
                    {
                        if (ThisReader.IsEmptyElement)
                        {
                            ThisReader.Skip();
                        }
                        else
                        {
                            o.@BorderBottom = DCSystemXml.XmlConvert.ToBoolean(ThisReader.ReadElementString());
                        }
                    }
                    else if (((object)_ReaderLocalName == (object)id203_BorderTop))
                    {
                        if (ThisReader.IsEmptyElement)
                        {
                            ThisReader.Skip();
                        }
                        else
                        {
                            o.@BorderTop = DCSystemXml.XmlConvert.ToBoolean(ThisReader.ReadElementString());
                        }
                    }
                    else if (((object)_ReaderLocalName == (object)id204_BorderRight))
                    {
                        if (ThisReader.IsEmptyElement)
                        {
                            ThisReader.Skip();
                        }
                        else
                        {
                            o.@BorderRight = DCSystemXml.XmlConvert.ToBoolean(ThisReader.ReadElementString());
                        }
                    }
                    else if (((object)_ReaderLocalName == (object)id210_PaddingLeft))
                    {
                        if (ThisReader.IsEmptyElement)
                        {
                            ThisReader.Skip();
                        }
                        else
                        {
                            o.@PaddingLeft = XSerHelper_XTextDocument.ToSingle(ThisReader.ReadElementString());
                        }
                    }
                    else if (((object)_ReaderLocalName == (object)id211_PaddingTop))
                    {
                        if (ThisReader.IsEmptyElement)
                        {
                            ThisReader.Skip();
                        }
                        else
                        {
                            o.@PaddingTop = XSerHelper_XTextDocument.ToSingle(ThisReader.ReadElementString());
                        }
                    }
                    else if (((object)_ReaderLocalName == (object)id212_PaddingRight))
                    {
                        if (ThisReader.IsEmptyElement)
                        {
                            ThisReader.Skip();
                        }
                        else
                        {
                            o.@PaddingRight = XSerHelper_XTextDocument.ToSingle(ThisReader.ReadElementString());
                        }
                    }
                    else if (((object)_ReaderLocalName == (object)id213_PaddingBottom))
                    {
                        if (ThisReader.IsEmptyElement)
                        {
                            ThisReader.Skip();
                        }
                        else
                        {
                            o.@PaddingBottom = XSerHelper_XTextDocument.ToSingle(ThisReader.ReadElementString());
                        }
                    }
                    else if (((object)_ReaderLocalName == (object)id221_ParagraphMultiLevel))
                    {
                        if (ThisReader.IsEmptyElement)
                        {
                            ThisReader.Skip();
                        }
                        else
                        {
                            o.@ParagraphMultiLevel = DCSystemXml.XmlConvert.ToBoolean(ThisReader.ReadElementString());
                        }
                    }
                    else if (((object)_ReaderLocalName == (object)id222_ParagraphOutlineLevel))
                    {
                        if (ThisReader.IsEmptyElement)
                        {
                            ThisReader.Skip();
                        }
                        else
                        {
                            o.@ParagraphOutlineLevel = DCSystemXml.XmlConvert.ToInt32(ThisReader.ReadElementString());
                        }
                    }
                    else if (((object)_ReaderLocalName == (object)id224_ParagraphListStyle))
                    {
                        if (ThisReader.IsEmptyElement)
                        {
                            ThisReader.Skip();
                        }
                        else
                        {
                            o.@ParagraphListStyle = Read44_ParagraphListStyle(ThisReader.ReadElementString());
                        }
                    }
                    else
                    {
                        UnknownNode(o, _ReaderLocalName);
                    }
                }
                else
                {
                    UnknownNode(o, ThisReader.LocalName);
                }
                ThisReader.MoveToContent();
                CheckReaderCount(ref whileIterations32, ref readerCount32);
                _ReaderNodeType1 = ThisReader.NodeType;
            }
            ReadEndElement();
            return o;
        }
        internal protected DCSoft.Writer.Dom.DomElement Read3_XTextElement(bool isNullable, bool checkType)
        {
            var ThisReader = this.Reader;
            var xsiType = checkType ? GetXsiType() : null;
            bool isNull = false;
            if (checkType)
            {
                var xsiTypeName = xsiType;
                if (xsiTypeName == null || ((object)xsiTypeName == (object)id51_Element))
                {
                }
                else if (((object)xsiTypeName == (object)id353_XParagraphFlag))
                {
                    return Read76_XTextParagraphFlagElement(isNullable, false);
                }
                else if (((object)xsiTypeName == (object)id349_XTextRadioBox))
                {
                    return Read144_XTextRadioBoxElement(isNullable, false);
                }
                else if (((object)xsiTypeName == (object)id350_XTextCheckBox))
                {
                    return Read143_XTextCheckBoxElement(isNullable, false);
                }
                else if (((object)xsiTypeName == (object)id351_XImage))
                {
                    return Read94_XTextImageElement(isNullable, false);
                }
                else if (((object)xsiTypeName == (object)id364_XInputField))
                {
                    return Read138_XTextInputFieldElement(isNullable, false);
                }
                else if (((object)xsiTypeName == (object)id370_XTextTableCell))
                {
                    return Read112_XTextTableCellElement(isNullable, false);
                }
                else if (((object)xsiTypeName == (object)id365_XTextTableRow))
                {
                    return Read107_XTextTableRowElement(isNullable, false);
                }
                else if (((object)xsiTypeName == (object)id366_XTextTable))
                {
                    return Read104_XTextTableElement(isNullable, false);
                }
                else if (((object)xsiTypeName == (object)id324_XPageBreak))
                {
                    return Read113_XTextPageBreakElement(isNullable, false);
                }
                else if (((object)xsiTypeName == (object)id325_XTextTableColumn))
                {
                    return Read108_XTextTableColumnElement(isNullable, false);
                }
                else if (((object)xsiTypeName == (object)id328_XLineBreak))
                {
                    return Read95_XTextLineBreakElement(isNullable, false);
                }
                else if (((object)xsiTypeName == (object)id347_XPageInfo))
                {
                    return Read148_XTextPageInfoElement(isNullable, false);
                }
                else if (((object)xsiTypeName == (object)id374_XTextFooter))
                {
                    return Read99_XTextDocumentFooterElement(isNullable, false);
                }
                else if (((object)xsiTypeName == (object)id375_XTextHeader))
                {
                    return Read98_XTextDocumentHeaderElement(isNullable, false);
                }
                else if (((object)xsiTypeName == (object)id376_XTextBody))
                {
                    return Read74_XTextDocumentBodyElement(isNullable, false);
                }
                else if (((object)xsiTypeName == (object)id1_XTextDocument))
                {
                    return Read180_XTextDocument(isNullable, false);
                }
                else
                {
                    DCConsole.Default.WriteLineError(DCSR.NotSupportedXmlNode + xsiType);
                    ThisReader.Skip();
                    return null;
                }
                    
            }
            if (isNull) return null;
            throw new NotSupportedException("Element");
        }
        internal protected DCSoft.Writer.Dom.DomDocumentBodyElement Read74_XTextDocumentBodyElement(bool isNullable, bool checkType)
        {
            var ThisReader = this.Reader;
            DCSoft.Writer.Dom.DomDocumentBodyElement o;
            o = new DCSoft.Writer.Dom.DomDocumentBodyElement();
            while (ThisReader.MoveToNextAttribute())
            {
                string _ReaderLocalName = ThisReader.LocalName;
                if (((object)_ReaderLocalName == (object)id3_StyleIndex))
                {
                    o.@StyleIndex = DCSystemXml.XmlConvert.ToInt32(ThisReader.Value);
                }
            }
            ThisReader.MoveToElement();
            if (ThisReader.IsEmptyElement)
            {
                ThisReader.Skip();
                return o;
            }
            ThisReader.ReadStartElement();
            ThisReader.MoveToContent();
            int whileIterations36 = 0;
            int readerCount36 = ReaderCount;
            var _ReaderNodeType1 = ThisReader.NodeType;
            while (_ReaderNodeType1 != DCSystemXml.XmlNodeType.EndElement && _ReaderNodeType1 != DCSystemXml.XmlNodeType.None)
            {
                if (_ReaderNodeType1 == DCSystemXml.XmlNodeType.Element)
                {
                    string _ReaderLocalName = ThisReader.LocalName;
                    if (((object)_ReaderLocalName == (object)id11_ID))
                    {
                        {
                            o.@ID = CacheString(ThisReader.ReadElementString());
                        }
                    }

                    else if (((object)_ReaderLocalName == (object)id25_ToolTip))
                    {
                        {
                            o.@ToolTip = CacheString(ThisReader.ReadElementString());
                        }
                    }
                    else if (((object)_ReaderLocalName == (object)id26_AcceptTab))
                    {
                        if (ThisReader.IsEmptyElement)
                        {
                            ThisReader.Skip();
                        }
                        else
                        {
                            o.@AcceptTab = DCSystemXml.XmlConvert.ToBoolean(ThisReader.ReadElementString());
                        }
                    }
                    else if (((object)_ReaderLocalName == (object)id32_ValidateStyle))
                    {
                        o.@ValidateStyle = Read14_ValueValidateStyle(false, true);
                    }
                    else if (((object)_ReaderLocalName == (object)id50_XElements))
                    {
                        _ContentElementCount = 0;
                        ReadContainerChildElements(ThisReader, o);
                        o._ContentCountForLoadDocument = _ContentElementCount;
                    }
                    else if (((object)_ReaderLocalName == (object)id54_Visible))
                    {
                        if (ThisReader.IsEmptyElement)
                        {
                            ThisReader.Skip();
                        }
                        else
                        {
                            o.@Visible = DCSystemXml.XmlConvert.ToBoolean(ThisReader.ReadElementString());
                        }
                    }
                    else
                    {
                        UnknownNode(o, _ReaderLocalName);
                    }
                }
                else
                {
                    UnknownNode(o, ThisReader.LocalName);
                }
                ThisReader.MoveToContent();
                CheckReaderCount(ref whileIterations36, ref readerCount36);
                _ReaderNodeType1 = ThisReader.NodeType;
            }
            ReadEndElement();
            return o;
        }
        internal protected DCSoft.Common.ValueValidateStyle Read14_ValueValidateStyle(bool isNullable, bool checkType)
        {
            var ThisReader = this.Reader;
            ThisReader.MoveToElement();
            if (ThisReader.IsEmptyElement)
            {
                ThisReader.Skip();
                return null;
            }
            var o = new DCSoft.Common.ValueValidateStyle();
            ThisReader.ReadStartElement();
            ThisReader.MoveToContent();
            int whileIterations47 = 0;
            int readerCount47 = ReaderCount;
            var _ReaderNodeType1 = ThisReader.NodeType;
            while (_ReaderNodeType1 != DCSystemXml.XmlNodeType.EndElement && _ReaderNodeType1 != DCSystemXml.XmlNodeType.None)
            {
                if (_ReaderNodeType1 == DCSystemXml.XmlNodeType.Element)
                {
                    string _ReaderLocalName = ThisReader.LocalName;
                    if (((object)_ReaderLocalName == (object)id400_Level))
                    {
                        if (ThisReader.IsEmptyElement)
                        {
                            ThisReader.Skip();
                        }
                        else
                        {
                            o.@Level = Read12_ValueValidateLevel(ThisReader.ReadElementString());
                        }
                    }
                    else if (((object)_ReaderLocalName == (object)id401_ValueName))
                    {
                        {
                            o.@ValueName = CacheString(ThisReader.ReadElementString());
                        }
                    }
                    else if (((object)_ReaderLocalName == (object)id402_Required))
                    {
                        if (ThisReader.IsEmptyElement)
                        {
                            ThisReader.Skip();
                        }
                        else
                        {
                            o.@Required = DCSystemXml.XmlConvert.ToBoolean(ThisReader.ReadElementString());
                        }
                    }
                    else if (((object)_ReaderLocalName == (object)id318_ValueType))
                    {
                        if (ThisReader.IsEmptyElement)
                        {
                            ThisReader.Skip();
                        }
                        else
                        {
                            o.@ValueType = Read13_ValueTypeStyle(ThisReader.ReadElementString());
                        }
                    }
                    else if (((object)_ReaderLocalName == (object)id403_BinaryLength))
                    {
                        if (ThisReader.IsEmptyElement)
                        {
                            ThisReader.Skip();
                        }
                        else
                        {
                            o.@BinaryLength = DCSystemXml.XmlConvert.ToBoolean(ThisReader.ReadElementString());
                        }
                    }
                    else if (((object)_ReaderLocalName == (object)id404_MaxLength))
                    {
                        if (ThisReader.IsEmptyElement)
                        {
                            ThisReader.Skip();
                        }
                        else
                        {
                            o.@MaxLength = DCSystemXml.XmlConvert.ToInt32(ThisReader.ReadElementString());
                        }
                    }
                    else if (((object)_ReaderLocalName == (object)id405_MinLength))
                    {
                        if (ThisReader.IsEmptyElement)
                        {
                            ThisReader.Skip();
                        }
                        else
                        {
                            o.@MinLength = DCSystemXml.XmlConvert.ToInt32(ThisReader.ReadElementString());
                        }
                    }
                    else if (((object)_ReaderLocalName == (object)id406_CheckMaxValue))
                    {
                        if (ThisReader.IsEmptyElement)
                        {
                            ThisReader.Skip();
                        }
                        else
                        {
                            o.@CheckMaxValue = DCSystemXml.XmlConvert.ToBoolean(ThisReader.ReadElementString());
                        }
                    }
                    else if (((object)_ReaderLocalName == (object)id407_CheckMinValue))
                    {
                        if (ThisReader.IsEmptyElement)
                        {
                            ThisReader.Skip();
                        }
                        else
                        {
                            o.@CheckMinValue = DCSystemXml.XmlConvert.ToBoolean(ThisReader.ReadElementString());
                        }
                    }
                    else if (((object)_ReaderLocalName == (object)id408_MaxValue))
                    {
                        if (ThisReader.IsEmptyElement)
                        {
                            ThisReader.Skip();
                        }
                        else
                        {
                            o.@MaxValue = XSerHelper_XTextDocument.ToDouble(ThisReader.ReadElementString());
                        }
                    }
                    else if (((object)_ReaderLocalName == (object)id409_MinValue))
                    {
                        if (ThisReader.IsEmptyElement)
                        {
                            ThisReader.Skip();
                        }
                        else
                        {
                            o.@MinValue = XSerHelper_XTextDocument.ToDouble(ThisReader.ReadElementString());
                        }
                    }
                    else if (((object)_ReaderLocalName == (object)id410_CheckDecimalDigits))
                    {
                        if (ThisReader.IsEmptyElement)
                        {
                            ThisReader.Skip();
                        }
                        else
                        {
                            o.@CheckDecimalDigits = DCSystemXml.XmlConvert.ToBoolean(ThisReader.ReadElementString());
                        }
                    }
                    else if (((object)_ReaderLocalName == (object)id411_MaxDecimalDigits))
                    {
                        if (ThisReader.IsEmptyElement)
                        {
                            ThisReader.Skip();
                        }
                        else
                        {
                            o.@MaxDecimalDigits = DCSystemXml.XmlConvert.ToInt32(ThisReader.ReadElementString());
                        }
                    }
                    else if (((object)_ReaderLocalName == (object)id412_DateTimeMaxValue))
                    {
                        if (ThisReader.IsEmptyElement)
                        {
                            ThisReader.Skip();
                        }
                        else
                        {
                            o.@DateTimeMaxValue = ToDateTime(ThisReader.ReadElementString());
                        }
                    }
                    else if (((object)_ReaderLocalName == (object)id413_DateTimeMinValue))
                    {
                        if (ThisReader.IsEmptyElement)
                        {
                            ThisReader.Skip();
                        }
                        else
                        {
                            o.@DateTimeMinValue = ToDateTime(ThisReader.ReadElementString());
                        }
                    }
                    else if (((object)_ReaderLocalName == (object)id419_RequiredInvalidateFlag))
                    {
                        if (ThisReader.IsEmptyElement)
                        {
                            ThisReader.Skip();
                        }
                        else
                        {
                            o.@RequiredInvalidateFlag = DCSystemXml.XmlConvert.ToBoolean(ThisReader.ReadElementString());
                        }
                    }
                    else
                    {
                        UnknownNode(o, _ReaderLocalName);
                    }
                }
                else
                {
                    UnknownNode(o, ThisReader.LocalName);
                }
                ThisReader.MoveToContent();
                CheckReaderCount(ref whileIterations47, ref readerCount47);
                _ReaderNodeType1 = ThisReader.NodeType;
            }
            ReadEndElement();
            return o;
        }
        internal protected DCSoft.Common.ValueTypeStyle Read13_ValueTypeStyle(string s)
        {

            if (__DCSoft_Common_ValueTypeStyle == null)
            {
                 
                {
                    var dic20200818 = new System.Collections.Generic.Dictionary<string, DCSoft.Common.ValueTypeStyle>();
                    dic20200818.Add("Text", DCSoft.Common.ValueTypeStyle.@Text);
                    dic20200818.Add("Integer", DCSoft.Common.ValueTypeStyle.@Integer);
                    dic20200818.Add("Numeric", DCSoft.Common.ValueTypeStyle.@Numeric);
                    dic20200818.Add("Date", DCSoft.Common.ValueTypeStyle.@Date);
                    dic20200818.Add("Time", DCSoft.Common.ValueTypeStyle.@Time);
                    dic20200818.Add("DateTime", DCSoft.Common.ValueTypeStyle.@DateTime);
                    __DCSoft_Common_ValueTypeStyle = dic20200818;
                }
            }
            var result = default(DCSoft.Common.ValueTypeStyle);
            if (__DCSoft_Common_ValueTypeStyle.TryGetValue(s, out result))
            {
                return result;
            }
            else
            {
                return default(DCSoft.Common.ValueTypeStyle);
            }
        }
        private static System.Collections.Generic.Dictionary<string, DCSoft.Common.ValueTypeStyle> __DCSoft_Common_ValueTypeStyle = null;

        internal protected DCSoft.Common.ValueValidateLevel Read12_ValueValidateLevel(string s)
        {
            switch (s)
            {
                case @"Error": return DCSoft.Common.ValueValidateLevel.@Error;
                case @"Warring": return DCSoft.Common.ValueValidateLevel.@Warring;
                case @"Info": return DCSoft.Common.ValueValidateLevel.@Info;
                default: return (default(DCSoft.Common.ValueValidateLevel));
            }
        }
        internal protected DCSoft.Writer.Dom.DomDocumentHeaderElement Read98_XTextDocumentHeaderElement(bool isNullable, bool checkType)
        {
            var ThisReader = this.Reader;
            DCSoft.Writer.Dom.DomDocumentHeaderElement o;
            o = new DCSoft.Writer.Dom.DomDocumentHeaderElement();
            while (ThisReader.MoveToNextAttribute())
            {
                string _ReaderLocalName = ThisReader.LocalName;
                if (((object)_ReaderLocalName == (object)id3_StyleIndex))
                {
                    o.@StyleIndex = DCSystemXml.XmlConvert.ToInt32(ThisReader.Value);
                }
            }
            ThisReader.MoveToElement();
            if (ThisReader.IsEmptyElement)
            {
                ThisReader.Skip();
                return o;
            }
            ThisReader.ReadStartElement();
            ThisReader.MoveToContent();
            int whileIterations53 = 0;
            int readerCount53 = ReaderCount;
            var _ReaderNodeType1 = ThisReader.NodeType;
            while (_ReaderNodeType1 != DCSystemXml.XmlNodeType.EndElement && _ReaderNodeType1 != DCSystemXml.XmlNodeType.None)
            {
                if (_ReaderNodeType1 == DCSystemXml.XmlNodeType.Element)
                {
                    string _ReaderLocalName = ThisReader.LocalName;
                    if (((object)_ReaderLocalName == (object)id11_ID))
                    {
                        {
                            o.@ID = CacheString(ThisReader.ReadElementString());
                        }
                    }
                    
                    else if (((object)_ReaderLocalName == (object)id25_ToolTip))
                    {
                        {
                            o.@ToolTip = CacheString(ThisReader.ReadElementString());
                        }
                    }
                    else if (((object)_ReaderLocalName == (object)id26_AcceptTab))
                    {
                        if (ThisReader.IsEmptyElement)
                        {
                            ThisReader.Skip();
                        }
                        else
                        {
                            o.@AcceptTab = DCSystemXml.XmlConvert.ToBoolean(ThisReader.ReadElementString());
                        }
                    }
                    else if (((object)_ReaderLocalName == (object)id32_ValidateStyle))
                    {
                        o.@ValidateStyle = Read14_ValueValidateStyle(false, true);
                    }
                    else if (((object)_ReaderLocalName == (object)id50_XElements))
                    {
                        ReadContainerChildElements(ThisReader, o);
                    }
                    else if (((object)_ReaderLocalName == (object)id54_Visible))
                    {
                        if (ThisReader.IsEmptyElement)
                        {
                            ThisReader.Skip();
                        }
                        else
                        {
                            o.@Visible = DCSystemXml.XmlConvert.ToBoolean(ThisReader.ReadElementString());
                        }
                    }
                    else
                    {
                        UnknownNode(o, _ReaderLocalName);
                    }
                }
                else
                {
                    UnknownNode(o, ThisReader.LocalName);
                }
                ThisReader.MoveToContent();
                CheckReaderCount(ref whileIterations53, ref readerCount53);
                _ReaderNodeType1 = ThisReader.NodeType;
            }
            ReadEndElement();
            return o;
        }
        internal protected DCSoft.Writer.Dom.DomDocumentFooterElement Read99_XTextDocumentFooterElement(bool isNullable, bool checkType)
        {
            var ThisReader = this.Reader;
            DCSoft.Writer.Dom.DomDocumentFooterElement o;
            o = new DCSoft.Writer.Dom.DomDocumentFooterElement();
            while (ThisReader.MoveToNextAttribute())
            {
                string _ReaderLocalName = ThisReader.LocalName;
                if (((object)_ReaderLocalName == (object)id3_StyleIndex))
                {
                    o.@StyleIndex = DCSystemXml.XmlConvert.ToInt32(ThisReader.Value);
                }
            }
            ThisReader.MoveToElement();
            if (ThisReader.IsEmptyElement)
            {
                ThisReader.Skip();
                return o;
            }
            ThisReader.ReadStartElement();
            ThisReader.MoveToContent();
            int whileIterations59 = 0;
            int readerCount59 = ReaderCount;
            var _ReaderNodeType1 = ThisReader.NodeType;
            while (_ReaderNodeType1 != DCSystemXml.XmlNodeType.EndElement && _ReaderNodeType1 != DCSystemXml.XmlNodeType.None)
            {
                if (_ReaderNodeType1 == DCSystemXml.XmlNodeType.Element)
                {
                    string _ReaderLocalName = ThisReader.LocalName;
                    if (((object)_ReaderLocalName == (object)id11_ID))
                    {
                        {
                            o.@ID = CacheString(ThisReader.ReadElementString());
                        }
                    }
                    
                    else if (((object)_ReaderLocalName == (object)id25_ToolTip))
                    {
                        {
                            o.@ToolTip = CacheString(ThisReader.ReadElementString());
                        }
                    }
                    else if (((object)_ReaderLocalName == (object)id26_AcceptTab))
                    {
                        if (ThisReader.IsEmptyElement)
                        {
                            ThisReader.Skip();
                        }
                        else
                        {
                            o.@AcceptTab = DCSystemXml.XmlConvert.ToBoolean(ThisReader.ReadElementString());
                        }
                    }
                    else if (((object)_ReaderLocalName == (object)id32_ValidateStyle))
                    {
                        o.@ValidateStyle = Read14_ValueValidateStyle(false, true);
                    }
                    else if (((object)_ReaderLocalName == (object)id50_XElements))
                    {
                        ReadContainerChildElements(ThisReader, o);
                    }
                    else if (((object)_ReaderLocalName == (object)id54_Visible))
                    {
                        if (ThisReader.IsEmptyElement)
                        {
                            ThisReader.Skip();
                        }
                        else
                        {
                            o.@Visible = DCSystemXml.XmlConvert.ToBoolean(ThisReader.ReadElementString());
                        }
                    }
                    else
                    {
                        UnknownNode(o, _ReaderLocalName);
                    }
                }
                else
                {
                    UnknownNode(o, ThisReader.LocalName);
                }
                ThisReader.MoveToContent();
                CheckReaderCount(ref whileIterations59, ref readerCount59);
                _ReaderNodeType1 = ThisReader.NodeType;
            }
            ReadEndElement();
            return o;
        }
        internal protected DCSoft.Writer.Dom.DomTableCellElement Read112_XTextTableCellElement(bool isNullable, bool checkType)
        {
            var ThisReader = this.Reader;
            DCSoft.Writer.Dom.DomTableCellElement o;
            o = new DCSoft.Writer.Dom.DomTableCellElement();
            var _contentCountBack = -1;
            while (ThisReader.MoveToNextAttribute())
            {
                string _ReaderLocalName = ThisReader.LocalName;
                if (((object)_ReaderLocalName == (object)id3_StyleIndex))
                {
                    o.@StyleIndex = DCSystemXml.XmlConvert.ToInt32(ThisReader.Value);
                }
                else if (_ReaderLocalName == "C")
                {
                    // 元素被合并了
                    _contentCountBack = _ContentElementCount;
                }
            }
            ThisReader.MoveToElement();
            if (ThisReader.IsEmptyElement)
            {
                ThisReader.Skip();
                return o;
            }
            ThisReader.ReadStartElement();
            ThisReader.MoveToContent();
            int whileIterations77 = 0;
            int readerCount77 = ReaderCount;
            var _ReaderNodeType1 = ThisReader.NodeType;
            while (_ReaderNodeType1 != DCSystemXml.XmlNodeType.EndElement && _ReaderNodeType1 != DCSystemXml.XmlNodeType.None)
            {
                if (_ReaderNodeType1 == DCSystemXml.XmlNodeType.Element)
                {
                    string _ReaderLocalName = ThisReader.LocalName;
                    if (((object)_ReaderLocalName == (object)id11_ID))
                    {
                        {
                            o.@ID = CacheString(ThisReader.ReadElementString());
                        }
                    }
                    
                    else if (((object)_ReaderLocalName == (object)id25_ToolTip))
                    {
                        {
                            o.@ToolTip = CacheString(ThisReader.ReadElementString());
                        }
                    }
                    else if (((object)_ReaderLocalName == (object)id32_ValidateStyle))
                    {
                        o.@ValidateStyle = Read14_ValueValidateStyle(false, true);
                    }
                    else if (((object)_ReaderLocalName == (object)id50_XElements))
                    {
                        ReadContainerChildElements(ThisReader, o);
                        if( o._Elements != null && o._Elements.LastElementIsXTextParagraphFlagElement() == false )
                        {
                            _ContentElementCount++;
                        }
                    }
                    else if (((object)_ReaderLocalName == (object)id54_Visible))
                    {
                        if (ThisReader.IsEmptyElement)
                        {
                            ThisReader.Skip();
                        }
                        else
                        {
                            o.@Visible = DCSystemXml.XmlConvert.ToBoolean(ThisReader.ReadElementString());
                        }
                    }
                    else if (((object)_ReaderLocalName == (object)id438_TabStop))
                    {
                        if (ThisReader.IsEmptyElement)
                        {
                            ThisReader.Skip();
                        }
                        else
                        {
                            o.@TabStop = DCSystemXml.XmlConvert.ToBoolean(ThisReader.ReadElementString());
                        }
                    }
                    else if (((object)_ReaderLocalName == (object)id445_RowSpan))
                    {
                        if (ThisReader.IsEmptyElement)
                        {
                            ThisReader.Skip();
                        }
                        else
                        {
                            o.@RowSpan = DCSystemXml.XmlConvert.ToInt32(ThisReader.ReadElementString());
                        }
                    }
                    else if (((object)_ReaderLocalName == (object)id446_ColSpan))
                    {
                        if (ThisReader.IsEmptyElement)
                        {
                            ThisReader.Skip();
                        }
                        else
                        {
                            o.@ColSpan = DCSystemXml.XmlConvert.ToInt32(ThisReader.ReadElementString());
                        }
                    }
                    else
                    {
                        UnknownNode(o, _ReaderLocalName);
                    }
                }
                else
                {
                    UnknownNode(o, ThisReader.LocalName);
                }
                ThisReader.MoveToContent();
                CheckReaderCount(ref whileIterations77, ref readerCount77);
                _ReaderNodeType1 = ThisReader.NodeType;
            }
            ReadEndElement();
            if(_contentCountBack >= 0 )
            {
                _ContentElementCount = _contentCountBack;
            }
            return o;
        }
        static System.Collections.Generic.Dictionary<string, long> _MoveFocusHotKeysValues;
        internal System.Collections.Generic.Dictionary<string, long> MoveFocusHotKeysValues
        {
            get
            {
                if (_MoveFocusHotKeysValues == null)
                {
                    var h = new System.Collections.Generic.Dictionary<string, long>();
                    h.Add(@"None", (long)DCSoft.Writer.MoveFocusHotKeys.@None);
                    h.Add(@"Default", (long)DCSoft.Writer.MoveFocusHotKeys.@Default);
                    h.Add(@"Tab", (long)DCSoft.Writer.MoveFocusHotKeys.@Tab);
                    h.Add(@"Enter", (long)DCSoft.Writer.MoveFocusHotKeys.@Enter);
                    _MoveFocusHotKeysValues = h;
                }
                return _MoveFocusHotKeysValues;
            }
        }
        internal protected DCSoft.Writer.MoveFocusHotKeys Read110_MoveFocusHotKeys(string s)
        {
            return (DCSoft.Writer.MoveFocusHotKeys)XSerHelper_XTextDocument.ToEnum(s, MoveFocusHotKeysValues, @" DCSoft.Writer.MoveFocusHotKeys");
        }
        internal protected DCSoft.Writer.Dom.DomTableElement Read104_XTextTableElement(bool isNullable, bool checkType)
        {
            var ThisReader = this.Reader;
            DCSoft.Writer.Dom.DomTableElement o;
            o = new DCSoft.Writer.Dom.DomTableElement();
            while (ThisReader.MoveToNextAttribute())
            {
                string _ReaderLocalName = ThisReader.LocalName;
                if (((object)_ReaderLocalName == (object)id3_StyleIndex))
                {
                    o.@StyleIndex = DCSystemXml.XmlConvert.ToInt32(ThisReader.Value);
                    break;
                }
            }
            ThisReader.MoveToElement();
            if (ThisReader.IsEmptyElement)
            {
                ThisReader.Skip();
                return o;
            }
            ThisReader.ReadStartElement();
            ThisReader.MoveToContent();
            int whileIterations95 = 0;
            int readerCount95 = ReaderCount;
            var _ReaderNodeType1 = ThisReader.NodeType;
            while (_ReaderNodeType1 != DCSystemXml.XmlNodeType.EndElement && _ReaderNodeType1 != DCSystemXml.XmlNodeType.None)
            {
                if (_ReaderNodeType1 == DCSystemXml.XmlNodeType.Element)
                {
                    string _ReaderLocalName = ThisReader.LocalName;
                    if (((object)_ReaderLocalName == (object)id11_ID))
                    {
                        {
                            o.@ID = CacheString(ThisReader.ReadElementString());
                        }
                    }
                    else if (((object)_ReaderLocalName == (object)id25_ToolTip))
                    {
                        {
                            o.@ToolTip = CacheString(ThisReader.ReadElementString());
                        }
                    }
                    else if (((object)_ReaderLocalName == (object)id50_XElements))
                    {
                        ReadContainerChildElements(ThisReader, o );
                    }
                    else if (((object)_ReaderLocalName == (object)id54_Visible))
                    {
                        if (ThisReader.IsEmptyElement)
                        {
                            ThisReader.Skip();
                        }
                        else
                        {
                            o.@Visible = DCSystemXml.XmlConvert.ToBoolean(ThisReader.ReadElementString());
                        }
                    }
                    else if (((object)_ReaderLocalName == (object)id478_Columns))
                    {
                        {
                            if ((ThisReader.IsEmptyElement))
                            {
                                ThisReader.Skip();
                            }
                            else
                            {
                                if ((o.@Columns) == null) o.@Columns = new DCSoft.Writer.Dom.DomElementList();
                                DCSoft.Writer.Dom.DomElementList a_61_0 = o.@Columns;
                                ThisReader.ReadStartElement();
                                ThisReader.MoveToContent();
                                int whileIterations101 = 0;
                                int readerCount101 = ReaderCount;
                                var _ReaderNodeType2 = ThisReader.NodeType;
                                while (_ReaderNodeType2 != DCSystemXml.XmlNodeType.EndElement && _ReaderNodeType2 != DCSystemXml.XmlNodeType.None)
                                {
                                    if (_ReaderNodeType2 == DCSystemXml.XmlNodeType.Element)
                                    {
                                        if (((object)ThisReader.LocalName == (object)id51_Element))
                                        {
                                            if ((a_61_0) == null)
                                                ThisReader.Skip();
                                            else
                                            {
                                                var item9 = Read3_XTextElement(true, true);
                                                if (item9 != null)
                                                {
                                                    a_61_0.Add(item9);
                                                }
                                            }
                                        }
                                        else
                                        {
                                            UnknownNode(null, ThisReader.LocalName);
                                        }
                                    }
                                    else
                                    {
                                        UnknownNode(null, ThisReader.LocalName);
                                    }
                                    ThisReader.MoveToContent();
                                    CheckReaderCount(ref whileIterations101, ref readerCount101);
                                    _ReaderNodeType2 = ThisReader.NodeType;
                                }
                                ReadEndElement();
                            }
                        }
                    }
                    else
                    {
                        UnknownNode(o, _ReaderLocalName);
                    }
                }
                else
                {
                    UnknownNode(o, ThisReader.LocalName);
                }
                ThisReader.MoveToContent();
                CheckReaderCount(ref whileIterations95, ref readerCount95);
                _ReaderNodeType1 = ThisReader.NodeType;
            }
            ReadEndElement();
            return o;
        }
        internal protected DCSoft.Writer.Dom.DomTableRowElement Read107_XTextTableRowElement(bool isNullable, bool checkType)
        {
            var ThisReader = this.Reader;
            DCSoft.Writer.Dom.DomTableRowElement o;
            o = new DCSoft.Writer.Dom.DomTableRowElement();
            while (ThisReader.MoveToNextAttribute())
            {
                string _ReaderLocalName = ThisReader.LocalName;
                if (((object)_ReaderLocalName == (object)id3_StyleIndex))
                {
                    o.@StyleIndex = DCSystemXml.XmlConvert.ToInt32(ThisReader.Value);
                }
            }
            ThisReader.MoveToElement();
            if (ThisReader.IsEmptyElement)
            {
                ThisReader.Skip();
                return o;
            }
            ThisReader.ReadStartElement();
            ThisReader.MoveToContent();
            int whileIterations102 = 0;
            int readerCount102 = ReaderCount;
            var _ReaderNodeType1 = ThisReader.NodeType;
            while (_ReaderNodeType1 != DCSystemXml.XmlNodeType.EndElement && _ReaderNodeType1 != DCSystemXml.XmlNodeType.None)
            {
                if (_ReaderNodeType1 == DCSystemXml.XmlNodeType.Element)
                {
                    string _ReaderLocalName = ThisReader.LocalName;
                    if (((object)_ReaderLocalName == (object)id11_ID))
                    {
                        {
                            o.@ID = CacheString(ThisReader.ReadElementString());
                        }
                    }
                    
                    else if (((object)_ReaderLocalName == (object)id25_ToolTip))
                    {
                        {
                            o.@ToolTip = CacheString(ThisReader.ReadElementString());
                        }
                    }
                    else if (((object)_ReaderLocalName == (object)id50_XElements))
                    {
                        ReadContainerChildElements(ThisReader, o);
                    }
                    else if (((object)_ReaderLocalName == (object)id54_Visible))
                    {
                        if (ThisReader.IsEmptyElement)
                        {
                            ThisReader.Skip();
                        }
                        else
                        {
                            o.@Visible = DCSystemXml.XmlConvert.ToBoolean(ThisReader.ReadElementString());
                        }
                    }
                    else if (((object)_ReaderLocalName == (object)id280_NewPage))
                    {
                        if (ThisReader.IsEmptyElement)
                        {
                            ThisReader.Skip();
                        }
                        else
                        {
                            o.@NewPage = DCSystemXml.XmlConvert.ToBoolean(ThisReader.ReadElementString());
                        }
                    }
                    else if (((object)_ReaderLocalName == (object)id490_CanSplitByPageLine))
                    {
                        if (ThisReader.IsEmptyElement)
                        {
                            ThisReader.Skip();
                        }
                        else
                        {
                            o.@CanSplitByPageLine = DCSystemXml.XmlConvert.ToBoolean(ThisReader.ReadElementString());
                        }
                    }
                    else if (((object)_ReaderLocalName == (object)id457_SpecifyHeight))
                    {
                        if (ThisReader.IsEmptyElement)
                        {
                            ThisReader.Skip();
                        }
                        else
                        {
                            o.@SpecifyHeight = XSerHelper_XTextDocument.ToSingle(ThisReader.ReadElementString());
                        }
                    }
                    else if (((object)_ReaderLocalName == (object)id491_HeaderStyle))
                    {
                        if (ThisReader.IsEmptyElement)
                        {
                            ThisReader.Skip();
                        }
                        else
                        {
                            o.@HeaderStyle = DCSystemXml.XmlConvert.ToBoolean(ThisReader.ReadElementString());
                        }
                    }
                    else
                    {
                        UnknownNode(o, _ReaderLocalName);
                    }
                }
                else
                {
                    UnknownNode(o, ThisReader.LocalName);
                }
                ThisReader.MoveToContent();
                CheckReaderCount(ref whileIterations102, ref readerCount102);
                _ReaderNodeType1 = ThisReader.NodeType;
            }
            ReadEndElement();
            return o;
        }
        internal protected DCSoft.Writer.Dom.DomInputFieldElement Read138_XTextInputFieldElement(bool isNullable, bool checkType)
        {
            var ThisReader = this.Reader;
            DCSoft.Writer.Dom.DomInputFieldElement o;
            o = new DCSoft.Writer.Dom.DomInputFieldElement();
            while (ThisReader.MoveToNextAttribute())
            {
                string _ReaderLocalName = ThisReader.LocalName;
                if (((object)_ReaderLocalName == (object)id3_StyleIndex))
                {
                    o.@StyleIndex = DCSystemXml.XmlConvert.ToInt32(ThisReader.Value);
                }
            }
            ThisReader.MoveToElement();
            if (ThisReader.IsEmptyElement)
            {
                ThisReader.Skip();
                return o;
            }
            ThisReader.ReadStartElement();
            ThisReader.MoveToContent();
            int whileIterations108 = 0;
            int readerCount108 = ReaderCount;
            var _ReaderNodeType1 = ThisReader.NodeType;
            while (_ReaderNodeType1 != DCSystemXml.XmlNodeType.EndElement && _ReaderNodeType1 != DCSystemXml.XmlNodeType.None)
            {
                if (_ReaderNodeType1 == DCSystemXml.XmlNodeType.Element)
                {
                    string _ReaderLocalName = ThisReader.LocalName;
                    if (((object)_ReaderLocalName == (object)id11_ID))
                    {
                        {
                            o.@ID = CacheString(ThisReader.ReadElementString());
                        }
                    }
                    else if (((object)_ReaderLocalName == (object)id25_ToolTip))
                    {
                        {
                            o.@ToolTip = CacheString(ThisReader.ReadElementString());
                        }
                    }
                    else if (((object)_ReaderLocalName == (object)id26_AcceptTab))
                    {
                        if (ThisReader.IsEmptyElement)
                        {
                            ThisReader.Skip();
                        }
                        else
                        {
                            o.@AcceptTab = DCSystemXml.XmlConvert.ToBoolean(ThisReader.ReadElementString());
                        }
                    }
                    else if (((object)_ReaderLocalName == (object)id32_ValidateStyle))
                    {
                        o.@ValidateStyle = Read14_ValueValidateStyle(false, true);
                    }
                    else if (((object)_ReaderLocalName == (object)id50_XElements))
                    {
                        ReadContainerChildElements(ThisReader, o);
                    }
                    else if (((object)_ReaderLocalName == (object)id54_Visible))
                    {
                        if (ThisReader.IsEmptyElement)
                        {
                            ThisReader.Skip();
                        }
                        else
                        {
                            o.@Visible = DCSystemXml.XmlConvert.ToBoolean(ThisReader.ReadElementString());
                        }
                    }
                    else if (((object)_ReaderLocalName == (object)id438_TabStop))
                    {
                        if (ThisReader.IsEmptyElement)
                        {
                            ThisReader.Skip();
                        }
                        else
                        {
                            o.@TabStop = DCSystemXml.XmlConvert.ToBoolean(ThisReader.ReadElementString());
                        }
                    }
                    else if (((object)_ReaderLocalName == (object)id443_MoveFocusHotKey))
                    {
                        if (ThisReader.IsEmptyElement)
                        {
                            ThisReader.Skip();
                        }
                        else
                        {
                            o.@MoveFocusHotKey = Read110_MoveFocusHotKeys(ThisReader.ReadElementString());
                        }
                    }
                    else if (((object)_ReaderLocalName == (object)id509_UserEditable))
                    {
                        if (ThisReader.IsEmptyElement)
                        {
                            ThisReader.Skip();
                        }
                        else
                        {
                            o.@UserEditable = DCSystemXml.XmlConvert.ToBoolean(ThisReader.ReadElementString());
                        }
                    }
                    else if (((object)_ReaderLocalName == (object)id62_Name))
                    {
                        {
                            o.@Name = CacheString(ThisReader.ReadElementString());
                        }
                    }
                    else if (((object)_ReaderLocalName == (object)id435_DisplayFormat))
                    {
                        o.@DisplayFormat = Read123_ValueFormater(false, true);
                    }
                    else if (((object)_ReaderLocalName == (object)id511_InnerValue))
                    {
                        {
                            o.@InnerValue = CacheString(ThisReader.ReadElementString());
                        }
                    }
                    else if (((object)_ReaderLocalName == (object)id512_PrintBackgroundText))
                    {
                        if (ThisReader.IsEmptyElement)
                        {
                            ThisReader.Skip();
                        }
                        else
                        {
                            o.@PrintBackgroundText = Read18_DCBooleanValue(ThisReader.ReadElementString());
                        }
                    }
                    else if (((object)_ReaderLocalName == (object)id513_BackgroundText))
                    {
                        {
                            o.@BackgroundText = CacheString(ThisReader.ReadElementString());
                        }
                    }
                    else if (((object)_ReaderLocalName == (object)id515_BorderVisible))
                    {
                        if (ThisReader.IsEmptyElement)
                        {
                            ThisReader.Skip();
                        }
                        else
                        {
                            o.@BorderVisible = Read125_DCVisibleState(ThisReader.ReadElementString());
                        }
                    }
                    else if (((object)_ReaderLocalName == (object)id516_EnableHighlight))
                    {
                        if (ThisReader.IsEmptyElement)
                        {
                            ThisReader.Skip();
                        }
                        else
                        {
                            o.@EnableHighlight = Read126_EnableState(ThisReader.ReadElementString());
                        }
                    }
                    else if (((object)_ReaderLocalName == (object)id517_EnableUserEditInnerValue))
                    {
                        if (ThisReader.IsEmptyElement)
                        {
                            ThisReader.Skip();
                        }
                        else
                        {
                            o.@EnableUserEditInnerValue = DCSystemXml.XmlConvert.ToBoolean(ThisReader.ReadElementString());
                        }
                    }
                    else if (((object)_ReaderLocalName == (object)id526_EditorActiveMode))
                    {
                        if (ThisReader.IsEmptyElement)
                        {
                            ThisReader.Skip();
                        }
                        else
                        {
                            o.@EditorActiveMode = Read131_ValueEditorActiveMode(ThisReader.ReadElementString());
                        }
                    }
                    else if (((object)_ReaderLocalName == (object)id531_FieldSettings))
                    {
                        o.@FieldSettings = Read136_InputFieldSettings(false, true);
                    }
                    
                    else
                    {
                        UnknownNode(o, _ReaderLocalName);
                    }
                }
                else
                {
                    UnknownNode(o, ThisReader.LocalName);
                }
                ThisReader.MoveToContent();
                CheckReaderCount(ref whileIterations108, ref readerCount108);
                _ReaderNodeType1 = ThisReader.NodeType;
            }
            ReadEndElement();
            if(o.HasElements() == false && o.BackgroundText != null )
            {
                _ContentElementCount += o.BackgroundText.Length;
            }
            _ContentElementCount += 2;
            return o;
        }

        internal protected DCSoft.Writer.Dom.InputFieldSettings Read136_InputFieldSettings(bool isNullable, bool checkType)
        {
            var ThisReader = this.Reader;
            ThisReader.MoveToElement();
            if (ThisReader.IsEmptyElement)
            {
                ThisReader.Skip();
                return null;
            }
            DCSoft.Writer.Dom.InputFieldSettings o;
            o = new DCSoft.Writer.Dom.InputFieldSettings();
            ThisReader.ReadStartElement();
            ThisReader.MoveToContent();
            int whileIterations116 = 0;
            int readerCount116 = ReaderCount;
            var _ReaderNodeType1 = ThisReader.NodeType;
            while (_ReaderNodeType1 != DCSystemXml.XmlNodeType.EndElement && _ReaderNodeType1 != DCSystemXml.XmlNodeType.None)
            {
                if (_ReaderNodeType1 == DCSystemXml.XmlNodeType.Element)
                {
                    string _ReaderLocalName = ThisReader.LocalName;
                    if (((object)_ReaderLocalName == (object)id538_EditStyle))
                    {
                        if (ThisReader.IsEmptyElement)
                        {
                            ThisReader.Skip();
                        }
                        else
                        {
                            o.@EditStyle = Read133_InputFieldEditStyle(ThisReader.ReadElementString());
                        }
                    }
                    else if (((object)_ReaderLocalName == (object)id541_MultiSelect))
                    {
                        if (ThisReader.IsEmptyElement)
                        {
                            ThisReader.Skip();
                        }
                        else
                        {
                            o.@MultiSelect = DCSystemXml.XmlConvert.ToBoolean(ThisReader.ReadElementString());
                        }
                    }
                    else if (((object)_ReaderLocalName == (object)id543_ListSource))
                    {
                        o.@ListSource = Read134_ListSourceInfo(false, true);
                    }
                    else if (((object)_ReaderLocalName == (object)id545_ListValueSeparatorChar))
                    {
                        {
                            o.@ListValueSeparatorChar = CacheString(ThisReader.ReadElementString());
                        }
                    }
                    else
                    {
                        UnknownNode(o, _ReaderLocalName);
                    }
                }
                else
                {
                    UnknownNode(o, ThisReader.LocalName);
                }
                ThisReader.MoveToContent();
                CheckReaderCount(ref whileIterations116, ref readerCount116);
                _ReaderNodeType1 = ThisReader.NodeType;
            }
            ReadEndElement();
            return o;
        }
        private DCSoft.Writer.Data.ListItemCollection _Cached_ListItems = new DCSoft.Writer.Data.ListItemCollection();

        internal protected DCSoft.Writer.Data.ListSourceInfo Read134_ListSourceInfo(bool isNullable, bool checkType)
        {
            var ThisReader = this.Reader;
            ThisReader.MoveToElement();
            if (ThisReader.IsEmptyElement)
            {
                ThisReader.Skip();
                return null;
            }
            DCSoft.Writer.Data.ListSourceInfo o;
            o = new DCSoft.Writer.Data.ListSourceInfo();
            ThisReader.ReadStartElement();
            ThisReader.MoveToContent();
            int whileIterations119 = 0;
            int readerCount119 = ReaderCount;
            var _ReaderNodeType1 = ThisReader.NodeType;
            while (_ReaderNodeType1 != DCSystemXml.XmlNodeType.EndElement && _ReaderNodeType1 != DCSystemXml.XmlNodeType.None)
            {
                if (_ReaderNodeType1 == DCSystemXml.XmlNodeType.Element)
                {
                    string _ReaderLocalName = ThisReader.LocalName;
                    if (((object)_ReaderLocalName == (object)id552_Items))
                    {
                        {
                            if ((ThisReader.IsEmptyElement))
                            {
                                ThisReader.Skip();
                            }
                            else
                            {
                                DCSoft.Writer.Data.ListItemCollection a_3_0 = _Cached_ListItems;
                                a_3_0.FastClear();

                                ThisReader.ReadStartElement();
                                ThisReader.MoveToContent();
                                int whileIterations120 = 0;
                                int readerCount120 = ReaderCount;
                                var _ReaderNodeType2 = ThisReader.NodeType;
                                while (_ReaderNodeType2 != DCSystemXml.XmlNodeType.EndElement && _ReaderNodeType2 != DCSystemXml.XmlNodeType.None)
                                {
                                    if (_ReaderNodeType2 == DCSystemXml.XmlNodeType.Element)
                                    {
                                        if (((object)ThisReader.LocalName == (object)id30_Item))
                                        {
                                            if ((a_3_0) == null) ThisReader.Skip(); else a_3_0.Add(Read132_ListItem(true, true));
                                        }
                                        else
                                        {
                                            UnknownNode(null, ThisReader.LocalName);  
                                        }
                                    }
                                    else
                                    {
                                        UnknownNode(null, ThisReader.LocalName);  
                                    }
                                    ThisReader.MoveToContent();
                                    CheckReaderCount(ref whileIterations120, ref readerCount120);
                                    _ReaderNodeType2 = ThisReader.NodeType;
                                }
                                ReadEndElement();
                                var items9 = new DCSoft.Writer.Data.ListItemCollection();
                                items9.AddRangeByDCList(a_3_0);
                                a_3_0.FastClear();
                                o.Items = items9;
                            }
                        }
                    }
                    else
                    {
                        UnknownNode(o, _ReaderLocalName);
                    }
                }
                else
                {
                    UnknownNode(o, ThisReader.LocalName);
                }
                ThisReader.MoveToContent();
                CheckReaderCount(ref whileIterations119, ref readerCount119);
                _ReaderNodeType1 = ThisReader.NodeType;
            }
            ReadEndElement();
            return o;
        }
        internal protected DCSoft.Writer.Data.ListItem Read132_ListItem(bool isNullable, bool checkType)
        {
            var ThisReader = this.Reader;
            DCSoft.Writer.Data.ListItem o;
            o = new DCSoft.Writer.Data.ListItem();
            ThisReader.MoveToElement();
            if (ThisReader.IsEmptyElement)
            {
                ThisReader.Skip();
                return o;
            }
            ThisReader.ReadStartElement();
            ThisReader.MoveToContent();
            int whileIterations121 = 0;
            int readerCount121 = ReaderCount;
            var _ReaderNodeType1 = ThisReader.NodeType;
            while (_ReaderNodeType1 != DCSystemXml.XmlNodeType.EndElement && _ReaderNodeType1 != DCSystemXml.XmlNodeType.None)
            {
                if (_ReaderNodeType1 == DCSystemXml.XmlNodeType.Element)
                {
                    string _ReaderLocalName = ThisReader.LocalName;
                    if (((object)_ReaderLocalName == (object)id557_TextInList))
                    {
                        {
                            o.@TextInList = CacheString(ThisReader.ReadElementString());
                        }
                    }
                    else if (((object)_ReaderLocalName == (object)id146_Text))
                    {
                        {
                            o.@Text = CacheString(ThisReader.ReadElementString());
                        }
                    }
                    else if (((object)_ReaderLocalName == (object)id248_Value))
                    {
                        {
                            o.@Value = CacheString(ThisReader.ReadElementString());
                        }
                    }
                    else
                    {
                        UnknownNode(o, _ReaderLocalName);
                    }
                }
                else
                {
                    UnknownNode(o, ThisReader.LocalName);
                }
                ThisReader.MoveToContent();
                CheckReaderCount(ref whileIterations121, ref readerCount121);
                _ReaderNodeType1 = ThisReader.NodeType;
            }
            ReadEndElement();
            return o;
        }
        internal protected DCSoft.Writer.Dom.InputFieldEditStyle Read133_InputFieldEditStyle(string s)
        {

            if (__DCSoft_Writer_Dom_InputFieldEditStyle == null)
            {
                 
                {
                    var dic20200818 = new System.Collections.Generic.Dictionary<string, DCSoft.Writer.Dom.InputFieldEditStyle>();
                    dic20200818.Add("Text", DCSoft.Writer.Dom.InputFieldEditStyle.@Text);
                    dic20200818.Add("DropdownList", DCSoft.Writer.Dom.InputFieldEditStyle.@DropdownList);
                    dic20200818.Add("Date", DCSoft.Writer.Dom.InputFieldEditStyle.@Date);
                    dic20200818.Add("DateTime", DCSoft.Writer.Dom.InputFieldEditStyle.@DateTime);
                    dic20200818.Add("Time", DCSoft.Writer.Dom.InputFieldEditStyle.@Time);
                    dic20200818.Add("Numeric", DCSoft.Writer.Dom.InputFieldEditStyle.@Numeric);
                    __DCSoft_Writer_Dom_InputFieldEditStyle = dic20200818;
                }
            }
            var result = default(DCSoft.Writer.Dom.InputFieldEditStyle);
            if (__DCSoft_Writer_Dom_InputFieldEditStyle.TryGetValue(s, out result))
            {
                return result;
            }
            else
            {
                return default(DCSoft.Writer.Dom.InputFieldEditStyle);
            }
        }
        private static System.Collections.Generic.Dictionary<string, DCSoft.Writer.Dom.InputFieldEditStyle> __DCSoft_Writer_Dom_InputFieldEditStyle = null;

        static System.Collections.Generic.Dictionary<string, long> _ValueEditorActiveModeValues;
        internal System.Collections.Generic.Dictionary<string, long> ValueEditorActiveModeValues
        {
            get
            {
                if (_ValueEditorActiveModeValues == null)
                {
                    var h = new System.Collections.Generic.Dictionary<string, long>();
                    h.Add(@"None", (long)DCSoft.Writer.ValueEditorActiveMode.@None);
                    h.Add(@"Default", (long)DCSoft.Writer.ValueEditorActiveMode.@Default);
                    h.Add(@"Program", (long)DCSoft.Writer.ValueEditorActiveMode.@Program);
                    h.Add(@"F2", (long)DCSoft.Writer.ValueEditorActiveMode.@F2);
                    h.Add(@"GotFocus", (long)DCSoft.Writer.ValueEditorActiveMode.@GotFocus);
                    h.Add(@"MouseDblClick", (long)DCSoft.Writer.ValueEditorActiveMode.@MouseDblClick);
                    h.Add(@"MouseClick", (long)DCSoft.Writer.ValueEditorActiveMode.@MouseClick);
                    h.Add(@"MouseRightClick", (long)DCSoft.Writer.ValueEditorActiveMode.@MouseRightClick);
                    h.Add(@"Enter", (long)DCSoft.Writer.ValueEditorActiveMode.@Enter);
                    _ValueEditorActiveModeValues = h;
                }
                return _ValueEditorActiveModeValues;
            }
        }
        internal protected DCSoft.Writer.ValueEditorActiveMode Read131_ValueEditorActiveMode(string s)
        {
            return (DCSoft.Writer.ValueEditorActiveMode)XSerHelper_XTextDocument.ToEnum(s, ValueEditorActiveModeValues, @" DCSoft.Writer.ValueEditorActiveMode");
        }
        internal protected DCSoft.Writer.EnableState Read126_EnableState(string s)
        {
            switch (s)
            {
                case @"Default": return DCSoft.Writer.EnableState.@Default;
                case @"Enabled": return DCSoft.Writer.EnableState.@Enabled;
                case @"Disabled": return DCSoft.Writer.EnableState.@Disabled;
                default: return (default(DCSoft.Writer.EnableState));
            }
        }
        internal protected DCSoft.Writer.DCVisibleState Read125_DCVisibleState(string s)
        {
            switch (s)
            {
                case @"Visible": return DCSoft.Writer.DCVisibleState.@Visible;
                case @"Hidden": return DCSoft.Writer.DCVisibleState.@Hidden;
                case @"Default": return DCSoft.Writer.DCVisibleState.@Default;
                case @"AlwaysVisible": return DCSoft.Writer.DCVisibleState.@AlwaysVisible;
                default: return (default(DCSoft.Writer.DCVisibleState));
            }
        }
        internal protected DCSoft.Data.ValueFormater Read123_ValueFormater(bool isNullable, bool checkType)
        {
            var ThisReader = this.Reader;
            ThisReader.MoveToElement();
            if (ThisReader.IsEmptyElement)
            {
                ThisReader.Skip();
                return null;
            }
            DCSoft.Data.ValueFormater o;
            o = new DCSoft.Data.ValueFormater();
            ThisReader.ReadStartElement();
            ThisReader.MoveToContent();
            int whileIterations123 = 0;
            int readerCount123 = ReaderCount;
            var _ReaderNodeType1 = ThisReader.NodeType;
            while (_ReaderNodeType1 != DCSystemXml.XmlNodeType.EndElement && _ReaderNodeType1 != DCSystemXml.XmlNodeType.None)
            {
                if (_ReaderNodeType1 == DCSystemXml.XmlNodeType.Element)
                {
                    string _ReaderLocalName = ThisReader.LocalName;
                    if (((object)_ReaderLocalName == (object)id293_Style))
                    {
                        if (ThisReader.IsEmptyElement)
                        {
                            ThisReader.Skip();
                        }
                        else
                        {
                            o.@Style = Read122_ValueFormatStyle(ThisReader.ReadElementString());
                        }
                    }
                    else if (((object)_ReaderLocalName == (object)id286_Format))
                    {
                        {
                            o.@Format = CacheString(ThisReader.ReadElementString());
                        }
                    }
                    else if (((object)_ReaderLocalName == (object)id574_NoneText))
                    {
                        {
                            o.@NoneText = CacheString(ThisReader.ReadElementString());
                        }
                    }
                    else
                    {
                        UnknownNode(o, _ReaderLocalName);
                    }
                }
                else
                {
                    UnknownNode(o, ThisReader.LocalName);
                }
                ThisReader.MoveToContent();
                CheckReaderCount(ref whileIterations123, ref readerCount123);
                _ReaderNodeType1 = ThisReader.NodeType;
            }
            ReadEndElement();
            return o;
        }
        internal protected DCSoft.Data.ValueFormatStyle Read122_ValueFormatStyle(string s)
        {

            if (__DCSoft_Data_ValueFormatStyle == null)
            {
                 
                {
                    var dic20200818 = new System.Collections.Generic.Dictionary<string, DCSoft.Data.ValueFormatStyle>();
                    dic20200818.Add("None", DCSoft.Data.ValueFormatStyle.@None);
                    dic20200818.Add("Numeric", DCSoft.Data.ValueFormatStyle.@Numeric);
                    dic20200818.Add("Currency", DCSoft.Data.ValueFormatStyle.@Currency);
                    dic20200818.Add("DateTime", DCSoft.Data.ValueFormatStyle.@DateTime);
                    dic20200818.Add("String", DCSoft.Data.ValueFormatStyle.@String);
                    dic20200818.Add("SpecifyLength", DCSoft.Data.ValueFormatStyle.@SpecifyLength);
                    dic20200818.Add("Boolean", DCSoft.Data.ValueFormatStyle.@Boolean);
                    dic20200818.Add("Percent", DCSoft.Data.ValueFormatStyle.@Percent);
                    __DCSoft_Data_ValueFormatStyle = dic20200818;
                }
            }
            var result = default(DCSoft.Data.ValueFormatStyle);
            if (__DCSoft_Data_ValueFormatStyle.TryGetValue(s, out result))
            {
                return result;
            }
            else
            {
                return default(DCSoft.Data.ValueFormatStyle);
            }
        }
        private static System.Collections.Generic.Dictionary<string, DCSoft.Data.ValueFormatStyle> __DCSoft_Data_ValueFormatStyle = null;
        internal protected StringAlignment Read119_StringAlignment(string s)
        {
            switch (s)
            {
                case @"Near": return StringAlignment.@Near;
                case @"Center": return StringAlignment.@Center;
                case @"Far": return StringAlignment.@Far;
                default: return (default(StringAlignment));
            }
        }
        internal protected DCSoft.Writer.Dom.DomParagraphFlagElement Read76_XTextParagraphFlagElement(bool isNullable, bool checkType)
        {
            _ContentElementCount++;
            var ThisReader = this.Reader;
            var o = new DCSoft.Writer.Dom.DomParagraphFlagElement();
            while (ThisReader.MoveToNextAttribute())
            {
                string _ReaderLocalName = ThisReader.LocalName;
                if (((object)_ReaderLocalName == (object)id3_StyleIndex))
                {
                    o.@StyleIndex = DCSystemXml.XmlConvert.ToInt32(ThisReader.Value);
                }
            }
            ThisReader.MoveToElement();
            if (ThisReader.IsEmptyElement)
            {
                ThisReader.Skip();
                return o;
            }
            ThisReader.ReadStartElement();
            ThisReader.MoveToContent();
            int whileIterations174 = 0;
            int readerCount174 = ReaderCount;
            var _ReaderNodeType1 = ThisReader.NodeType;
            while (_ReaderNodeType1 != DCSystemXml.XmlNodeType.EndElement && _ReaderNodeType1 != DCSystemXml.XmlNodeType.None)
            {
                if (_ReaderNodeType1 == DCSystemXml.XmlNodeType.Element)
                {
                    string _ReaderLocalName = ThisReader.LocalName;
                    if (((object)_ReaderLocalName == (object)id11_ID))
                    {
                        {
                            o.@ID = CacheString(ThisReader.ReadElementString());
                        }
                    }
                    else if (((object)_ReaderLocalName == (object)id599_TopicID))
                    {
                        if (ThisReader.IsEmptyElement)
                        {
                            ThisReader.Skip();
                        }
                        else
                        {
                            o.@TopicID = DCSystemXml.XmlConvert.ToInt32(ThisReader.ReadElementString());
                        }
                    }
                    else if (((object)_ReaderLocalName == (object)id600_ResetListIndexFlag))
                    {
                        if (ThisReader.IsEmptyElement)
                        {
                            ThisReader.Skip();
                        }
                        else
                        {
                            o.@ResetListIndexFlag = DCSystemXml.XmlConvert.ToBoolean(ThisReader.ReadElementString());
                        }
                    }
                    else if (((object)_ReaderLocalName == (object)id601_AutoCreate))
                    {
                        if (ThisReader.IsEmptyElement)
                        {
                            ThisReader.Skip();
                        }
                        else
                        {
                            o.@AutoCreate = DCSystemXml.XmlConvert.ToBoolean(ThisReader.ReadElementString());
                        }
                    }
                    else
                    {
                        UnknownNode(o, _ReaderLocalName);
                    }
                }
                else
                {
                    UnknownNode(o, ThisReader.LocalName);
                }
                ThisReader.MoveToContent();
                CheckReaderCount(ref whileIterations174, ref readerCount174);
                _ReaderNodeType1 = ThisReader.NodeType;
            }
            ReadEndElement();
            return o;
        }
        internal protected DCSoft.Writer.Dom.DomImageElement Read94_XTextImageElement(bool isNullable, bool checkType)
        {
            _ContentElementCount++;
            var ThisReader = this.Reader;
            DCSoft.Writer.Dom.DomImageElement o;
            o = new DCSoft.Writer.Dom.DomImageElement();
            while (ThisReader.MoveToNextAttribute())
            {
                string _ReaderLocalName = ThisReader.LocalName;
                if (((object)_ReaderLocalName == (object)id3_StyleIndex))
                {
                    o.@StyleIndex = DCSystemXml.XmlConvert.ToInt32(ThisReader.Value);
                }
            }
            ThisReader.MoveToElement();
            if (ThisReader.IsEmptyElement)
            {
                ThisReader.Skip();
                return o;
            }
            ThisReader.ReadStartElement();
            ThisReader.MoveToContent();
            int whileIterations176 = 0;
            int readerCount176 = ReaderCount;
            var _ReaderNodeType1 = ThisReader.NodeType;
            while (_ReaderNodeType1 != DCSystemXml.XmlNodeType.EndElement && _ReaderNodeType1 != DCSystemXml.XmlNodeType.None)
            {
                if (_ReaderNodeType1 == DCSystemXml.XmlNodeType.Element)
                {
                    string _ReaderLocalName = ThisReader.LocalName;
                    if (((object)_ReaderLocalName == (object)id11_ID))
                    {
                        {
                            o.@ID = CacheString(ThisReader.ReadElementString());
                        }
                    }
                    else if (((object)_ReaderLocalName == (object)id25_ToolTip))
                    {
                        {
                            o.@ToolTip = CacheString(ThisReader.ReadElementString());
                        }
                    }
                    else if (((object)_ReaderLocalName == (object)id54_Visible))
                    {
                        if (ThisReader.IsEmptyElement)
                        {
                            ThisReader.Skip();
                        }
                        else
                        {
                            o.@Visible = DCSystemXml.XmlConvert.ToBoolean(ThisReader.ReadElementString());
                        }
                    }
                    else if (((object)_ReaderLocalName == (object)id62_Name))
                    {
                        {
                            o.@Name = CacheString(ThisReader.ReadElementString());
                        }
                    }
                    else if (((object)_ReaderLocalName == (object)id591_VAlign))
                    {
                        if (ThisReader.IsEmptyElement)
                        {
                            ThisReader.Skip();
                        }
                        else
                        {
                            o.@VAlign = Read40_VerticalAlignStyle(ThisReader.ReadElementString());
                        }
                    }
                    else if (((object)_ReaderLocalName == (object)id611_KeepWidthHeightRate))
                    {
                        if (ThisReader.IsEmptyElement)
                        {
                            ThisReader.Skip();
                        }
                        else
                        {
                            o.@KeepWidthHeightRate = DCSystemXml.XmlConvert.ToBoolean(ThisReader.ReadElementString());
                        }
                    }
                    else if (((object)_ReaderLocalName == (object)id217_Width))
                    {
                        {
                            o.@Width = XSerHelper_XTextDocument.ToSingle(ThisReader.ReadElementString());
                        }
                    }
                    else if (((object)_ReaderLocalName == (object)id218_Height))
                    {
                        {
                            o.@Height = XSerHelper_XTextDocument.ToSingle(ThisReader.ReadElementString());
                        }
                    }
                    else if (((object)_ReaderLocalName == (object)id75_Image))
                    {
                        o.@Image = Read34_XImageValue(false, true);
                    }
                    else if (((object)_ReaderLocalName == (object)id618_SmoothZoom))
                    {
                        if (ThisReader.IsEmptyElement)
                        {
                            ThisReader.Skip();
                        }
                        else
                        {
                            o.@SmoothZoom = DCSystemXml.XmlConvert.ToBoolean(ThisReader.ReadElementString());
                        }
                    }
                    else{
                        UnknownNode(o, _ReaderLocalName);
                    }
                }
                else
                {
                    UnknownNode(o, ThisReader.LocalName);
                }
                ThisReader.MoveToContent();
                CheckReaderCount(ref whileIterations176, ref readerCount176);
                _ReaderNodeType1 = ThisReader.NodeType;
            }
            ReadEndElement();
            return o;
        }
        internal protected DCSoft.Writer.Dom.DomCheckBoxElement Read143_XTextCheckBoxElement(bool isNullable, bool checkType)
        {
            _ContentElementCount++;
            var ThisReader = this.Reader;
            DCSoft.Writer.Dom.DomCheckBoxElement o;
            o = new DCSoft.Writer.Dom.DomCheckBoxElement();
            while (ThisReader.MoveToNextAttribute())
            {
                string _ReaderLocalName = ThisReader.LocalName;
                if (((object)_ReaderLocalName == (object)id3_StyleIndex))
                {
                    o.@StyleIndex = DCSystemXml.XmlConvert.ToInt32(ThisReader.Value);
                }
            }
            ThisReader.MoveToElement();
            if (ThisReader.IsEmptyElement)
            {
                ThisReader.Skip();
                return o;
            }
            ThisReader.ReadStartElement();
            ThisReader.MoveToContent();
            int whileIterations201 = 0;
            int readerCount201 = ReaderCount;
            var _ReaderNodeType1 = ThisReader.NodeType;
            while (_ReaderNodeType1 != DCSystemXml.XmlNodeType.EndElement && _ReaderNodeType1 != DCSystemXml.XmlNodeType.None)
            {
                if (_ReaderNodeType1 == DCSystemXml.XmlNodeType.Element)
                {
                    string _ReaderLocalName = ThisReader.LocalName;
                    if (((object)_ReaderLocalName == (object)id11_ID))
                    {
                        {
                            o.@ID = CacheString(ThisReader.ReadElementString());
                        }
                    }
                    else if (((object)_ReaderLocalName == (object)id25_ToolTip))
                    {
                        {
                            o.@ToolTip = CacheString(ThisReader.ReadElementString());
                        }
                    }
                    else if (((object)_ReaderLocalName == (object)id54_Visible))
                    {
                        if (ThisReader.IsEmptyElement)
                        {
                            ThisReader.Skip();
                        }
                        else
                        {
                            o.@Visible = DCSystemXml.XmlConvert.ToBoolean(ThisReader.ReadElementString());
                        }
                    }
                    else if (((object)_ReaderLocalName == (object)id62_Name))
                    {
                        {
                            o.@Name = CacheString(ThisReader.ReadElementString());
                        }
                    }
                    else if (((object)_ReaderLocalName == (object)id654_Requried))
                    {
                        if (ThisReader.IsEmptyElement)
                        {
                            ThisReader.Skip();
                        }
                        else
                        {
                            o.@Requried = DCSystemXml.XmlConvert.ToBoolean(ThisReader.ReadElementString());
                        }
                    }
                    else if (((object)_ReaderLocalName == (object)id660_Caption))
                    {
                        {
                            o.@Caption = CacheString(ThisReader.ReadElementString());
                        }
                    }
                    else if (((object)_ReaderLocalName == (object)id662_CaptionAlign))
                    {
                        if (ThisReader.IsEmptyElement)
                        {
                            ThisReader.Skip();
                        }
                        else
                        {
                            o.@CaptionAlign = Read119_StringAlignment(ThisReader.ReadElementString());
                        }
                    }
                    else if (((object)_ReaderLocalName == (object)id663_AutoHeightForMultiline))
                    {
                        if (ThisReader.IsEmptyElement)
                        {
                            ThisReader.Skip();
                        }
                        else
                        {
                            o.@AutoHeightForMultiline = DCSystemXml.XmlConvert.ToBoolean(ThisReader.ReadElementString());
                        }
                    }
                    else if (((object)_ReaderLocalName == (object)id217_Width))
                    {
                        {
                            o.@Width = XSerHelper_XTextDocument.ToSingle(ThisReader.ReadElementString());
                        }
                    }
                    else if (((object)_ReaderLocalName == (object)id218_Height))
                    {
                        {
                            o.@Height = XSerHelper_XTextDocument.ToSingle(ThisReader.ReadElementString());
                        }
                    }
                    else if (((object)_ReaderLocalName == (object)id665_Checked))
                    {
                        if (ThisReader.IsEmptyElement)
                        {
                            ThisReader.Skip();
                        }
                        else
                        {
                            o.@Checked = DCSystemXml.XmlConvert.ToBoolean(ThisReader.ReadElementString());
                        }
                    }
                    else if (((object)_ReaderLocalName == (object)id315_CheckedValue))
                    {
                        {
                            o.@CheckedValue = CacheString(ThisReader.ReadElementString());
                        }
                    }
                    else if (((object)_ReaderLocalName == (object)id251_Readonly))
                    {
                        if (ThisReader.IsEmptyElement)
                        {
                            ThisReader.Skip();
                        }
                        else
                        {
                            o.@Readonly = DCSystemXml.XmlConvert.ToBoolean(ThisReader.ReadElementString());
                        }
                    }
                    else if (((object)_ReaderLocalName == (object)id190_Multiline))
                    {
                        if (ThisReader.IsEmptyElement)
                        {
                            ThisReader.Skip();
                        }
                        else
                        {
                            o.@Multiline = DCSystemXml.XmlConvert.ToBoolean(ThisReader.ReadElementString());
                        }
                    }
                    else
                    {
                        UnknownNode(o, _ReaderLocalName);
                    }
                }
                else
                {
                    UnknownNode(o, ThisReader.LocalName);
                }
                ThisReader.MoveToContent();
                CheckReaderCount(ref whileIterations201, ref readerCount201);
                _ReaderNodeType1 = ThisReader.NodeType;
            }
            ReadEndElement();
            return o;
        }
        internal protected DCSoft.Writer.Dom.DomRadioBoxElement Read144_XTextRadioBoxElement(bool isNullable, bool checkType)
        {
            _ContentElementCount++;
            var ThisReader = this.Reader;
            DCSoft.Writer.Dom.DomRadioBoxElement o;
            o = new DCSoft.Writer.Dom.DomRadioBoxElement();
            while (ThisReader.MoveToNextAttribute())
            {
                string _ReaderLocalName = ThisReader.LocalName;
                if (((object)_ReaderLocalName == (object)id3_StyleIndex))
                {
                    o.@StyleIndex = DCSystemXml.XmlConvert.ToInt32(ThisReader.Value);
                }
            }
            ThisReader.MoveToElement();
            if (ThisReader.IsEmptyElement)
            {
                ThisReader.Skip();
                return o;
            }
            ThisReader.ReadStartElement();
            ThisReader.MoveToContent();
            int whileIterations207 = 0;
            int readerCount207 = ReaderCount;
            var _ReaderNodeType1 = ThisReader.NodeType;
            while (_ReaderNodeType1 != DCSystemXml.XmlNodeType.EndElement && _ReaderNodeType1 != DCSystemXml.XmlNodeType.None)
            {
                if (_ReaderNodeType1 == DCSystemXml.XmlNodeType.Element)
                {
                    string _ReaderLocalName = ThisReader.LocalName;
                    if (((object)_ReaderLocalName == (object)id11_ID))
                    {
                        {
                            o.@ID = CacheString(ThisReader.ReadElementString());
                        }
                    }
                    else if (((object)_ReaderLocalName == (object)id25_ToolTip))
                    {
                        {
                            o.@ToolTip = CacheString(ThisReader.ReadElementString());
                        }
                    }
                    else if (((object)_ReaderLocalName == (object)id54_Visible))
                    {
                        if (ThisReader.IsEmptyElement)
                        {
                            ThisReader.Skip();
                        }
                        else
                        {
                            o.@Visible = DCSystemXml.XmlConvert.ToBoolean(ThisReader.ReadElementString());
                        }
                    }
                    else if (((object)_ReaderLocalName == (object)id62_Name))
                    {
                        {
                            o.@Name = CacheString(ThisReader.ReadElementString());
                        }
                    }
                    else if (((object)_ReaderLocalName == (object)id654_Requried))
                    {
                        if (ThisReader.IsEmptyElement)
                        {
                            ThisReader.Skip();
                        }
                        else
                        {
                            o.@Requried = DCSystemXml.XmlConvert.ToBoolean(ThisReader.ReadElementString());
                        }
                    }
                    else if (((object)_ReaderLocalName == (object)id660_Caption))
                    {
                        {
                            o.@Caption = CacheString(ThisReader.ReadElementString());
                        }
                    }
                    else if (((object)_ReaderLocalName == (object)id662_CaptionAlign))
                    {
                        if (ThisReader.IsEmptyElement)
                        {
                            ThisReader.Skip();
                        }
                        else
                        {
                            o.@CaptionAlign = Read119_StringAlignment(ThisReader.ReadElementString());
                        }
                    }
                    else if (((object)_ReaderLocalName == (object)id663_AutoHeightForMultiline))
                    {
                        if (ThisReader.IsEmptyElement)
                        {
                            ThisReader.Skip();
                        }
                        else
                        {
                            o.@AutoHeightForMultiline = DCSystemXml.XmlConvert.ToBoolean(ThisReader.ReadElementString());
                        }
                    }
                    else if (((object)_ReaderLocalName == (object)id217_Width))
                    {
                        {
                            o.@Width = XSerHelper_XTextDocument.ToSingle(ThisReader.ReadElementString());
                        }
                    }
                    else if (((object)_ReaderLocalName == (object)id218_Height))
                    {
                        {
                            o.@Height = XSerHelper_XTextDocument.ToSingle(ThisReader.ReadElementString());
                        }
                    }
                    else if (((object)_ReaderLocalName == (object)id665_Checked))
                    {
                        if (ThisReader.IsEmptyElement)
                        {
                            ThisReader.Skip();
                        }
                        else
                        {
                            o.@Checked = DCSystemXml.XmlConvert.ToBoolean(ThisReader.ReadElementString());
                        }
                    }
                    else if (((object)_ReaderLocalName == (object)id315_CheckedValue))
                    {
                        {
                            o.@CheckedValue = CacheString(ThisReader.ReadElementString());
                        }
                    }
                    else if (((object)_ReaderLocalName == (object)id251_Readonly))
                    {
                        if (ThisReader.IsEmptyElement)
                        {
                            ThisReader.Skip();
                        }
                        else
                        {
                            o.@Readonly = DCSystemXml.XmlConvert.ToBoolean(ThisReader.ReadElementString());
                        }
                    }
                    else if (((object)_ReaderLocalName == (object)id190_Multiline))
                    {
                        if (ThisReader.IsEmptyElement)
                        {
                            ThisReader.Skip();
                        }
                        else
                        {
                            o.@Multiline = DCSystemXml.XmlConvert.ToBoolean(ThisReader.ReadElementString());
                        }
                    }
                    else
                    {
                        UnknownNode(o, _ReaderLocalName);
                    }
                }
                else
                {
                    UnknownNode(o, ThisReader.LocalName);
                }
                ThisReader.MoveToContent();
                CheckReaderCount(ref whileIterations207, ref readerCount207);
                _ReaderNodeType1 = ThisReader.NodeType;
            }
            ReadEndElement();
            return o;
        }
        internal protected DCSoft.Writer.Dom.DomPageInfoElement Read148_XTextPageInfoElement(bool isNullable, bool checkType)
        {
            _ContentElementCount++;
            var ThisReader = this.Reader;
            DCSoft.Writer.Dom.DomPageInfoElement o;
            o = new DCSoft.Writer.Dom.DomPageInfoElement();
            while (ThisReader.MoveToNextAttribute())
            {
                string _ReaderLocalName = ThisReader.LocalName;
                if (((object)_ReaderLocalName == (object)id3_StyleIndex))
                {
                    o.@StyleIndex = DCSystemXml.XmlConvert.ToInt32(ThisReader.Value);
                }
            }
            ThisReader.MoveToElement();
            if (ThisReader.IsEmptyElement)
            {
                ThisReader.Skip();
                return o;
            }
            ThisReader.ReadStartElement();
            ThisReader.MoveToContent();
            int whileIterations213 = 0;
            int readerCount213 = ReaderCount;
            var _ReaderNodeType1 = ThisReader.NodeType;
            while (_ReaderNodeType1 != DCSystemXml.XmlNodeType.EndElement && _ReaderNodeType1 != DCSystemXml.XmlNodeType.None)
            {
                if (_ReaderNodeType1 == DCSystemXml.XmlNodeType.Element)
                {
                    string _ReaderLocalName = ThisReader.LocalName;
                    if (((object)_ReaderLocalName == (object)id11_ID))
                    {
                        {
                            o.@ID = CacheString(ThisReader.ReadElementString());
                        }
                    }
                    else if (((object)_ReaderLocalName == (object)id25_ToolTip))
                    {
                        {
                            o.@ToolTip = CacheString(ThisReader.ReadElementString());
                        }
                    }
                    else if (((object)_ReaderLocalName == (object)id54_Visible))
                    {
                        if (ThisReader.IsEmptyElement)
                        {
                            ThisReader.Skip();
                        }
                        else
                        {
                            o.@Visible = DCSystemXml.XmlConvert.ToBoolean(ThisReader.ReadElementString());
                        }
                    }
                    else if (((object)_ReaderLocalName == (object)id62_Name))
                    {
                        {
                            o.@Name = CacheString(ThisReader.ReadElementString());
                        }
                    }
                    else if (((object)_ReaderLocalName == (object)id217_Width))
                    {
                        {
                            o.@Width = XSerHelper_XTextDocument.ToSingle(ThisReader.ReadElementString());
                        }
                    }
                    else if (((object)_ReaderLocalName == (object)id670_AutoHeight))
                    {
                        if (ThisReader.IsEmptyElement)
                        {
                            ThisReader.Skip();
                        }
                        else
                        {
                            o.@AutoHeight = DCSystemXml.XmlConvert.ToBoolean(ThisReader.ReadElementString());
                        }
                    }
                    else if (((object)_ReaderLocalName == (object)id218_Height))
                    {
                        {
                            o.@Height = XSerHelper_XTextDocument.ToSingle(ThisReader.ReadElementString());
                        }
                    }
                    else if (((object)_ReaderLocalName == (object)id318_ValueType))
                    {
                        if (ThisReader.IsEmptyElement)
                        {
                            ThisReader.Skip();
                        }
                        else
                        {
                            o.@ValueType = Read147_PageInfoValueType(ThisReader.ReadElementString());
                        }
                    }
                    else
                    {
                        UnknownNode(o, _ReaderLocalName);
                    }
                }
                else
                {
                    UnknownNode(o, ThisReader.LocalName);
                }
                ThisReader.MoveToContent();
                CheckReaderCount(ref whileIterations213, ref readerCount213);
                _ReaderNodeType1 = ThisReader.NodeType;
            }
            ReadEndElement();
            return o;
        }
        internal protected DCSoft.Writer.Dom.PageInfoValueType Read147_PageInfoValueType(string s)
        {
            switch (s)
            {
                case @"PageIndex": return DCSoft.Writer.Dom.PageInfoValueType.@PageIndex;
                case @"NumOfPages": return DCSoft.Writer.Dom.PageInfoValueType.@NumOfPages;
                case @"LocalPageIndex": return DCSoft.Writer.Dom.PageInfoValueType.@LocalPageIndex;
                case @"LocalNumOfPages": return DCSoft.Writer.Dom.PageInfoValueType.@LocalNumOfPages;
                default: return (default(DCSoft.Writer.Dom.PageInfoValueType));
            }
        }
        internal protected DCSoft.Writer.Dom.DomLineBreakElement Read95_XTextLineBreakElement(bool isNullable, bool checkType)
        {
            _ContentElementCount++;
            var ThisReader = this.Reader;
            DCSoft.Writer.Dom.DomLineBreakElement o;
            o = new DCSoft.Writer.Dom.DomLineBreakElement();
            while (ThisReader.MoveToNextAttribute())
            {
                string _ReaderLocalName = ThisReader.LocalName;
                if (((object)_ReaderLocalName == (object)id3_StyleIndex))
                {
                    o.@StyleIndex = DCSystemXml.XmlConvert.ToInt32(ThisReader.Value);
                }
            }
            ThisReader.MoveToElement();
            if (ThisReader.IsEmptyElement)
            {
                ThisReader.Skip();
                return o;
            }
            ThisReader.ReadStartElement();
            ThisReader.MoveToContent();
            int whileIterations351 = 0;
            int readerCount351 = ReaderCount;
            var _ReaderNodeType1 = ThisReader.NodeType;
            while (_ReaderNodeType1 != DCSystemXml.XmlNodeType.EndElement && _ReaderNodeType1 != DCSystemXml.XmlNodeType.None)
            {
                if (_ReaderNodeType1 == DCSystemXml.XmlNodeType.Element)
                {
                    string _ReaderLocalName = ThisReader.LocalName;
                    if (((object)_ReaderLocalName == (object)id11_ID))
                    {
                        {
                            o.@ID = CacheString(ThisReader.ReadElementString());
                        }
                    }
                    else
                    {
                        UnknownNode(o, _ReaderLocalName);
                    }
                }
                else
                {
                    UnknownNode(o, ThisReader.LocalName);
                }
                ThisReader.MoveToContent();
                CheckReaderCount(ref whileIterations351, ref readerCount351);
                _ReaderNodeType1 = ThisReader.NodeType;
            }
            ReadEndElement();
            return o;
        }
        internal protected DCSoft.Writer.Dom.DomTableColumnElement Read108_XTextTableColumnElement(bool isNullable, bool checkType)
        {
            var ThisReader = this.Reader;
            DCSoft.Writer.Dom.DomTableColumnElement o;
            o = new DCSoft.Writer.Dom.DomTableColumnElement();
            while (ThisReader.MoveToNextAttribute())
            {
                string _ReaderLocalName = ThisReader.LocalName;
                if (((object)_ReaderLocalName == (object)id3_StyleIndex))
                {
                    o.@StyleIndex = DCSystemXml.XmlConvert.ToInt32(ThisReader.Value);
                }
            }
            ThisReader.MoveToElement();
            if (ThisReader.IsEmptyElement)
            {
                ThisReader.Skip();
                return o;
            }
            ThisReader.ReadStartElement();
            ThisReader.MoveToContent();
            int whileIterations357 = 0;
            int readerCount357 = ReaderCount;
            var _ReaderNodeType1 = ThisReader.NodeType;
            while (_ReaderNodeType1 != DCSystemXml.XmlNodeType.EndElement && _ReaderNodeType1 != DCSystemXml.XmlNodeType.None)
            {
                if (_ReaderNodeType1 == DCSystemXml.XmlNodeType.Element)
                {
                    string _ReaderLocalName = ThisReader.LocalName;
                    if (((object)_ReaderLocalName == (object)id11_ID))
                    {
                        {
                            o.@ID = CacheString(ThisReader.ReadElementString());
                        }
                    }
                    else if (((object)_ReaderLocalName == (object)id217_Width))
                    {
                        {
                            o.@Width = XSerHelper_XTextDocument.ToSingle(ThisReader.ReadElementString());
                        }
                    }
                    else
                    {
                        UnknownNode(o, _ReaderLocalName);
                    }
                }
                else
                {
                    UnknownNode(o, ThisReader.LocalName);
                }
                ThisReader.MoveToContent();
                CheckReaderCount(ref whileIterations357, ref readerCount357);
                _ReaderNodeType1 = ThisReader.NodeType;
            }
            ReadEndElement();
            return o;
        }
        internal protected DCSoft.Writer.Dom.DomPageBreakElement Read113_XTextPageBreakElement(bool isNullable, bool checkType)
        {
            _ContentElementCount++;
            var ThisReader = this.Reader;
            DCSoft.Writer.Dom.DomPageBreakElement o;
            o = new DCSoft.Writer.Dom.DomPageBreakElement();
            while (ThisReader.MoveToNextAttribute())
            {
                string _ReaderLocalName = ThisReader.LocalName;
                if (((object)_ReaderLocalName == (object)id3_StyleIndex))
                {
                    o.@StyleIndex = DCSystemXml.XmlConvert.ToInt32(ThisReader.Value);
                }
            }
            ThisReader.MoveToElement();
            if (ThisReader.IsEmptyElement)
            {
                ThisReader.Skip();
                return o;
            }
            ThisReader.ReadStartElement();
            ThisReader.MoveToContent();
            int whileIterations359 = 0;
            int readerCount359 = ReaderCount;
            var _ReaderNodeType1 = ThisReader.NodeType;
            while (_ReaderNodeType1 != DCSystemXml.XmlNodeType.EndElement && _ReaderNodeType1 != DCSystemXml.XmlNodeType.None)
            {
                if (_ReaderNodeType1 == DCSystemXml.XmlNodeType.Element)
                {
                    string _ReaderLocalName = ThisReader.LocalName;
                    if (((object)_ReaderLocalName == (object)id11_ID))
                    {
                        {
                            o.@ID = CacheString(ThisReader.ReadElementString());
                        }
                    }
                    else
                    {
                        UnknownNode(o, _ReaderLocalName);
                    }
                }
                else
                {
                    UnknownNode(o, ThisReader.LocalName);
                }
                ThisReader.MoveToContent();
                CheckReaderCount(ref whileIterations359, ref readerCount359);
                _ReaderNodeType1 = ThisReader.NodeType;
            }
            ReadEndElement();
            return o;
        }
        protected override void InitCallbacks()
        {
        }
        string id210_PaddingLeft;
        string id491_HeaderStyle;
        string id231_AlignToGridLine;
        string id30_Item;
        string id185_VerticalAlign;
        string id241_Author;
        string id158_Index;
        string id69_FileFormat;
        string id353_XParagraphFlag;
        string id86_Comment;
        string id660_Caption;
        string id516_EnableHighlight;
        string id266_DepartmentID;
        string id374_XTextFooter;
        string id557_TextInList;
        string id153_Bold;
        string id211_PaddingTop;
        string id203_BorderTop;
        string id259_Description;
        string id490_CanSplitByPageLine;
        string id117_PaperKind;
        string id513_BackgroundText;
        string id515_BorderVisible;
        string id599_TopicID;
        string id347_XPageInfo;
        string id125_DesignerPaperHeight;
        string id190_Multiline;
        string id3_StyleIndex;
        string id127_LeftMargin;
        string id419_RequiredInvalidateFlag;
        string id1_XTextDocument;
        string id270_DocumentProcessState;
        string id291_Default;
        string id146_Text;
        string id232_LineWidth;
        string id253_FieldBorderElementWidth;
        string id350_XTextCheckBox;
        string id445_RowSpan;
        string id662_CaptionAlign;
        string id54_Visible;
        string id410_CheckDecimalDigits;
        string id217_Width;
        string id233_GridNumInOnePage;
        string id591_VAlign;
        string id1063_WhiteSpaceLength;
        string id174_Superscript;
        string id315_CheckedValue;
        string id7_EditorVersionString;
        string id413_DateTimeMinValue;
        string id246_BorderColor;
        string id611_KeepWidthHeightRate;
        string id50_XElements;
        string id242_CreationTime;
        string id552_Items;
        string id123_DesignerPaperWidth;
        string id130_BottomMargin;
        string id269_DocumentType;
        string id204_BorderRight;
        string id25_ToolTip;
        string id11_ID;
        string id642_AutoSize;
        string id517_EnableUserEditInnerValue;
        string id251_Readonly;
        string id401_ValueName;
        string id273_NumOfPage;
        string id121_HeaderDistance;
        string id365_XTextTableRow;
        string id180_LineSpacingStyle;
        string id187_LeftIndent;
        string id126_PaperHeight;
        string id201_BorderLeft;
        string id51_Element;
        string id221_ParagraphMultiLevel;
        string id318_ValueType;
        string id271_DocumentEditState;
        string id236_Printable;
        string id276_HeightInPrintJob;
        string id235_LineStyle;
        string id670_AutoHeight;
        string id154_Italic;
        string id324_XPageBreak;
        string id351_XImage;
        string id75_Image;
        string id255_IsTemplate;
        string id267_DepartmentName;
        string id478_Columns;
        string id328_XLineBreak;
        string id370_XTextTableCell;
        string id435_DisplayFormat;
        string id601_AutoCreate;
        string id212_PaddingRight;
        string id103_DocumentGridLine;
        string id84_BodyText;
        string id62_Name;
        string id175_Subscript;
        string id665_Checked;
        string id376_XTextBody;
        string id234_GridSpanInCM;
        string id178_SpacingBeforeParagraph;
        string id129_RightMargin;
        string id509_UserEditable;
        string id406_CheckMaxValue;
        string id32_ValidateStyle;
        string id258_Version;
        string id465_NumOfRows;
        string id308_ContentStyle;
        string id443_MoveFocusHotKey;
        string id73_ContentStyles;
        string id403_BinaryLength;
        string id200_BorderWidth;
        string id143_ColorValue;
        string id541_MultiSelect;
        string id412_DateTimeMaxValue;
        string id469_Alignment;
        string id404_MaxLength;
        string id83_Info;
        string id275_StartPositionInPringJob;
        string id408_MaxValue;
        string id538_EditStyle;
        string id446_ColSpan;
        string id268_DocumentFormat;
        string id263_LastPrintTime;
        string id280_NewPage;
        string id349_XTextRadioBox;
        string id202_BorderBottom;
        string id124_PaperWidth;
        string id411_MaxDecimalDigits;
        string id248_Value;
        string id531_FieldSettings;
        string id222_ParagraphOutlineLevel;
        string id213_PaddingBottom;
        string id261_LastModifiedTime;
        string id128_TopMargin;
        string id169_FontName;
        string id262_EditMinute;
        string id654_Requried;
        string id526_EditorActiveMode;
        string id198_BorderBottomColor;
        string id218_Height;
        string id402_Required;
        string id155_Underline;
        string id238_Title;
        string id364_XInputField;
        string id196_BorderTopColor;
        string id131_Landscape;
        string id182_LineSpacing;
        string id199_BorderStyle;
        string id293_Style;
        string id292_Styles;
        string id122_FooterDistance;
        string id409_MinValue;
        string id663_AutoHeightForMultiline;
        string id438_TabStop;
        string id170_FontSize;
        string id405_MinLength;
        string id177_SpacingAfterParagraph;
        string id545_ListValueSeparatorChar;
        string id325_XTextTableColumn;
        string id195_BorderLeftColor;
        string id90_PageSettings;
        string id511_InnerValue;
        string id512_PrintBackgroundText;
        string id466_NumOfColumns;
        string id1062_WhitespaceCount;
        string id574_NoneText;
        string id137_ImageDataBase64String;
        string id264_AuthorName;
        string id407_CheckMinValue;
        string id393_FormatString;
        string id252_ShowHeaderBottomLine;
        string id224_ParagraphListStyle;
        string id618_SmoothZoom;
        string id168_Color;
        string id245_ForeColor;
        string id366_XTextTable;
        string id244_BackColor;
        string id400_Level;
        string id184_Align;
        string id156_Strikeout;
        string id457_SpecifyHeight;
        string id294_DocumentContentStyle;
        string id102_PowerDocumentGridLine;
        string id26_AcceptTab;
        string id375_XTextHeader;
        string id543_ListSource;
        string id159_BackgroundColor;
        string id286_Format;
        string id68_FileName;
        string id186_FirstLineIndent;
        string id600_ResetListIndexFlag;
        string id272_Operator;
        string id326_XString;
        string id197_BorderRightColor;

        protected override void InitIDs()
        {
            var myNameTable = this.Reader.NameTable;
            id210_PaddingLeft = myNameTable.Add(@"PaddingLeft");
            id491_HeaderStyle = myNameTable.Add(@"HeaderStyle");
            id231_AlignToGridLine = myNameTable.Add(@"AlignToGridLine");
            id30_Item = myNameTable.Add(@"Item");
            id185_VerticalAlign = myNameTable.Add(@"VerticalAlign");
            id241_Author = myNameTable.Add(@"Author");
            id158_Index = myNameTable.Add(@"Index");
            id69_FileFormat = myNameTable.Add(@"FileFormat");
            id353_XParagraphFlag = myNameTable.Add(@"XParagraphFlag");
            id86_Comment = myNameTable.Add(@"Comment");
            id660_Caption = myNameTable.Add(@"Caption");
            id516_EnableHighlight = myNameTable.Add(@"EnableHighlight");
            id266_DepartmentID = myNameTable.Add(@"DepartmentID");
            id374_XTextFooter = myNameTable.Add(@"XTextFooter");
            id557_TextInList = myNameTable.Add(@"TextInList");
            id153_Bold = myNameTable.Add(@"Bold");
            id211_PaddingTop = myNameTable.Add(@"PaddingTop");
            id203_BorderTop = myNameTable.Add(@"BorderTop");
            id259_Description = myNameTable.Add(@"Description");
            id490_CanSplitByPageLine = myNameTable.Add(@"CanSplitByPageLine");
            id117_PaperKind = myNameTable.Add(@"PaperKind");
            id513_BackgroundText = myNameTable.Add(@"BackgroundText");
            id515_BorderVisible = myNameTable.Add(@"BorderVisible");
            id599_TopicID = myNameTable.Add(@"TopicID");
            id347_XPageInfo = myNameTable.Add(@"XPageInfo");
            id125_DesignerPaperHeight = myNameTable.Add(@"DesignerPaperHeight");
            id190_Multiline = myNameTable.Add(@"Multiline");
            id3_StyleIndex = myNameTable.Add(@"StyleIndex");
            id127_LeftMargin = myNameTable.Add(@"LeftMargin");
            id419_RequiredInvalidateFlag = myNameTable.Add(@"RequiredInvalidateFlag");
            id1_XTextDocument = myNameTable.Add(@"XTextDocument");
            id270_DocumentProcessState = myNameTable.Add(@"DocumentProcessState");
            id291_Default = myNameTable.Add(@"Default");
            id146_Text = myNameTable.Add(@"Text");
            id232_LineWidth = myNameTable.Add(@"LineWidth");
            id253_FieldBorderElementWidth = myNameTable.Add(@"FieldBorderElementWidth");
            id350_XTextCheckBox = myNameTable.Add(@"XTextCheckBox");
            id445_RowSpan = myNameTable.Add(@"RowSpan");
            id662_CaptionAlign = myNameTable.Add(@"CaptionAlign");
            id54_Visible = myNameTable.Add(@"Visible");
            id410_CheckDecimalDigits = myNameTable.Add(@"CheckDecimalDigits");
            id217_Width = myNameTable.Add(@"Width");
            id233_GridNumInOnePage = myNameTable.Add(@"GridNumInOnePage");
            id591_VAlign = myNameTable.Add(@"VAlign");
            id1063_WhiteSpaceLength = myNameTable.Add(@"WhiteSpaceLength");
            id174_Superscript = myNameTable.Add(@"Superscript");
            id315_CheckedValue = myNameTable.Add(@"CheckedValue");
            id7_EditorVersionString = myNameTable.Add(@"EditorVersionString");
            id413_DateTimeMinValue = myNameTable.Add(@"DateTimeMinValue");
            id246_BorderColor = myNameTable.Add(@"BorderColor");
            id611_KeepWidthHeightRate = myNameTable.Add(@"KeepWidthHeightRate");
            id50_XElements = myNameTable.Add(@"XElements");
            id242_CreationTime = myNameTable.Add(@"CreationTime");
            id552_Items = myNameTable.Add(@"Items");
            id123_DesignerPaperWidth = myNameTable.Add(@"DesignerPaperWidth");
            id130_BottomMargin = myNameTable.Add(@"BottomMargin");
            id269_DocumentType = myNameTable.Add(@"DocumentType");
            id204_BorderRight = myNameTable.Add(@"BorderRight");
            id25_ToolTip = myNameTable.Add(@"ToolTip");
            id11_ID = myNameTable.Add(@"ID");
            id642_AutoSize = myNameTable.Add(@"AutoSize");
            id517_EnableUserEditInnerValue = myNameTable.Add(@"EnableUserEditInnerValue");
            id251_Readonly = myNameTable.Add(@"Readonly");
            id401_ValueName = myNameTable.Add(@"ValueName");
            id273_NumOfPage = myNameTable.Add(@"NumOfPage");
            id121_HeaderDistance = myNameTable.Add(@"HeaderDistance");
            id365_XTextTableRow = myNameTable.Add(@"XTextTableRow");
            id180_LineSpacingStyle = myNameTable.Add(@"LineSpacingStyle");
            id187_LeftIndent = myNameTable.Add(@"LeftIndent");
            id126_PaperHeight = myNameTable.Add(@"PaperHeight");
            id201_BorderLeft = myNameTable.Add(@"BorderLeft");
            id51_Element = myNameTable.Add(@"Element");
            id221_ParagraphMultiLevel = myNameTable.Add(@"ParagraphMultiLevel");
            id318_ValueType = myNameTable.Add(@"ValueType");
            id271_DocumentEditState = myNameTable.Add(@"DocumentEditState");
            id236_Printable = myNameTable.Add(@"Printable");
            id276_HeightInPrintJob = myNameTable.Add(@"HeightInPrintJob");
            id235_LineStyle = myNameTable.Add(@"LineStyle");
            id670_AutoHeight = myNameTable.Add(@"AutoHeight");
            id154_Italic = myNameTable.Add(@"Italic");
            id324_XPageBreak = myNameTable.Add(@"XPageBreak");
            id351_XImage = myNameTable.Add(@"XImage");
            id75_Image = myNameTable.Add(@"Image");
            id255_IsTemplate = myNameTable.Add(@"IsTemplate");
            id267_DepartmentName = myNameTable.Add(@"DepartmentName");
            id478_Columns = myNameTable.Add(@"Columns");
            id328_XLineBreak = myNameTable.Add(@"XLineBreak");
            id370_XTextTableCell = myNameTable.Add(@"XTextTableCell");
            id435_DisplayFormat = myNameTable.Add(@"DisplayFormat");
            id601_AutoCreate = myNameTable.Add(@"AutoCreate");
            id212_PaddingRight = myNameTable.Add(@"PaddingRight");
            id103_DocumentGridLine = myNameTable.Add(@"DocumentGridLine");
            id84_BodyText = myNameTable.Add(@"BodyText");
            id62_Name = myNameTable.Add(@"Name");
            id175_Subscript = myNameTable.Add(@"Subscript");
            id665_Checked = myNameTable.Add(@"Checked");
            id376_XTextBody = myNameTable.Add(@"XTextBody");
            id234_GridSpanInCM = myNameTable.Add(@"GridSpanInCM");
            id178_SpacingBeforeParagraph = myNameTable.Add(@"SpacingBeforeParagraph");
            id129_RightMargin = myNameTable.Add(@"RightMargin");
            id509_UserEditable = myNameTable.Add(@"UserEditable");
            id406_CheckMaxValue = myNameTable.Add(@"CheckMaxValue");
            id32_ValidateStyle = myNameTable.Add(@"ValidateStyle");
            id258_Version = myNameTable.Add(@"Version");
            id465_NumOfRows = myNameTable.Add(@"NumOfRows");
            id308_ContentStyle = myNameTable.Add(@"ContentStyle");
            id443_MoveFocusHotKey = myNameTable.Add(@"MoveFocusHotKey");
            id73_ContentStyles = myNameTable.Add(@"ContentStyles");
            id403_BinaryLength = myNameTable.Add(@"BinaryLength");
            id200_BorderWidth = myNameTable.Add(@"BorderWidth");
            id143_ColorValue = myNameTable.Add(@"ColorValue");
            id541_MultiSelect = myNameTable.Add(@"MultiSelect");
            id412_DateTimeMaxValue = myNameTable.Add(@"DateTimeMaxValue");
            id469_Alignment = myNameTable.Add(@"Alignment");
            id404_MaxLength = myNameTable.Add(@"MaxLength");
            id83_Info = myNameTable.Add(@"Info");
            id275_StartPositionInPringJob = myNameTable.Add(@"StartPositionInPringJob");
            id408_MaxValue = myNameTable.Add(@"MaxValue");
            id538_EditStyle = myNameTable.Add(@"EditStyle");
            id446_ColSpan = myNameTable.Add(@"ColSpan");
            id268_DocumentFormat = myNameTable.Add(@"DocumentFormat");
            id263_LastPrintTime = myNameTable.Add(@"LastPrintTime");
            id280_NewPage = myNameTable.Add(@"NewPage");
            id349_XTextRadioBox = myNameTable.Add(@"XTextRadioBox");
            id202_BorderBottom = myNameTable.Add(@"BorderBottom");
            id124_PaperWidth = myNameTable.Add(@"PaperWidth");
            id411_MaxDecimalDigits = myNameTable.Add(@"MaxDecimalDigits");
            id248_Value = myNameTable.Add(@"Value");
            id531_FieldSettings = myNameTable.Add(@"FieldSettings");
            id222_ParagraphOutlineLevel = myNameTable.Add(@"ParagraphOutlineLevel");
            id213_PaddingBottom = myNameTable.Add(@"PaddingBottom");
            id261_LastModifiedTime = myNameTable.Add(@"LastModifiedTime");
            id128_TopMargin = myNameTable.Add(@"TopMargin");
            id169_FontName = myNameTable.Add(@"FontName");
            id262_EditMinute = myNameTable.Add(@"EditMinute");
            id654_Requried = myNameTable.Add(@"Requried");
            id526_EditorActiveMode = myNameTable.Add(@"EditorActiveMode");
            id198_BorderBottomColor = myNameTable.Add(@"BorderBottomColor");
            id218_Height = myNameTable.Add(@"Height");
            id402_Required = myNameTable.Add(@"Required");
            id155_Underline = myNameTable.Add(@"Underline");
            id238_Title = myNameTable.Add(@"Title");
            id364_XInputField = myNameTable.Add(@"XInputField");
            id196_BorderTopColor = myNameTable.Add(@"BorderTopColor");
            id131_Landscape = myNameTable.Add(@"Landscape");
            id182_LineSpacing = myNameTable.Add(@"LineSpacing");
            id199_BorderStyle = myNameTable.Add(@"BorderStyle");
            id293_Style = myNameTable.Add(@"Style");
            id292_Styles = myNameTable.Add(@"Styles");
            id122_FooterDistance = myNameTable.Add(@"FooterDistance");
            id409_MinValue = myNameTable.Add(@"MinValue");
            id663_AutoHeightForMultiline = myNameTable.Add(@"AutoHeightForMultiline");
            id438_TabStop = myNameTable.Add(@"TabStop");
            id170_FontSize = myNameTable.Add(@"FontSize");
            id405_MinLength = myNameTable.Add(@"MinLength");
            id177_SpacingAfterParagraph = myNameTable.Add(@"SpacingAfterParagraph");
            id545_ListValueSeparatorChar = myNameTable.Add(@"ListValueSeparatorChar");
            id325_XTextTableColumn = myNameTable.Add(@"XTextTableColumn");
            id195_BorderLeftColor = myNameTable.Add(@"BorderLeftColor");
            id90_PageSettings = myNameTable.Add(@"PageSettings");
            id511_InnerValue = myNameTable.Add(@"InnerValue");
            id512_PrintBackgroundText = myNameTable.Add(@"PrintBackgroundText");
            id466_NumOfColumns = myNameTable.Add(@"NumOfColumns");
            id1062_WhitespaceCount = myNameTable.Add(@"WhitespaceCount");
            id574_NoneText = myNameTable.Add(@"NoneText");
            id137_ImageDataBase64String = myNameTable.Add(@"ImageDataBase64String");
            id264_AuthorName = myNameTable.Add(@"AuthorName");
            id407_CheckMinValue = myNameTable.Add(@"CheckMinValue");
            id393_FormatString = myNameTable.Add(@"FormatString");
            id252_ShowHeaderBottomLine = myNameTable.Add(@"ShowHeaderBottomLine");
            id224_ParagraphListStyle = myNameTable.Add(@"ParagraphListStyle");
            id618_SmoothZoom = myNameTable.Add(@"SmoothZoom");
            id168_Color = myNameTable.Add(@"Color");
            id245_ForeColor = myNameTable.Add(@"ForeColor");
            id366_XTextTable = myNameTable.Add(@"XTextTable");
            id244_BackColor = myNameTable.Add(@"BackColor");
            id400_Level = myNameTable.Add(@"Level");
            id184_Align = myNameTable.Add(@"Align");
            id156_Strikeout = myNameTable.Add(@"Strikeout");
            id457_SpecifyHeight = myNameTable.Add(@"SpecifyHeight");
            id294_DocumentContentStyle = myNameTable.Add(@"DocumentContentStyle");
            id102_PowerDocumentGridLine = myNameTable.Add(@"PowerDocumentGridLine");
            id26_AcceptTab = myNameTable.Add(@"AcceptTab");
            id375_XTextHeader = myNameTable.Add(@"XTextHeader");
            id543_ListSource = myNameTable.Add(@"ListSource");
            id159_BackgroundColor = myNameTable.Add(@"BackgroundColor");
            id286_Format = myNameTable.Add(@"Format");
            id68_FileName = myNameTable.Add(@"FileName");
            id186_FirstLineIndent = myNameTable.Add(@"FirstLineIndent");
            id600_ResetListIndexFlag = myNameTable.Add(@"ResetListIndexFlag");
            id272_Operator = myNameTable.Add(@"Operator");
            id326_XString = myNameTable.Add(@"XString");
            id197_BorderRightColor = myNameTable.Add(@"BorderRightColor");

            if (base.Reader is DCSoft.Writer.Serialization.ArrayXmlReader)
            {
                ((DCSoft.Writer.Serialization.ArrayXmlReader)base.Reader).ApplyNameTable();
            }
        }
    }
}
