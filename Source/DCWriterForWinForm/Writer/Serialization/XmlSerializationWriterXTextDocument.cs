using DCSoft.Writer.Dom;
using System.Text;
using DCSystemXml;

namespace DCSoft.Writer.NewSerializationNoStringEncrypt
{
    [System.Reflection.Obfuscation(Exclude = false, ApplyToMembers = true, Feature = "JIEJIE.NET.SWITCH:-hightstrings,-controlflow")]
    public class XmlSerializationWriterXTextDocument : DCSoft.Writer.NewSerialization.DCXmlSerializationWriter
    {
        private void WriteContainerChildElements(DomContainerElement rootElement)
        {
            var elements = rootElement._Elements;
            if (elements != null && elements.Count > 0)
            {
                var thisWriter = this.Writer;
                thisWriter.WriteStartElement(null, "XElements", null);
                var len = elements.Count;
                var arr = elements.InnerGetArrayRaw();
                if (rootElement is DomTableRowElement)
                {
                    // 特别处理表格行对象
                    for (var iCount = 0; iCount < len; iCount++)
                    {
                        if (arr[iCount] is DomTableCellElement cell)
                        {
                            Write112_XTextTableCellElement("Element", string.Empty, cell, true, true);
                        }
                    }
                }
                else if (rootElement is DomTableElement)
                {
                    // 特别处理表格对象
                    for (var iCount = 0; iCount < len; iCount++)
                    {
                        if (arr[iCount] is DomTableRowElement row)
                        {
                            Write107_XTextTableRowElement("Element", string.Empty, row, true, true);
                        }
                    }
                }
                else
                {
                    var strLastText = new StringBuilder();
                    var lastCharStyleIndex = int.MinValue;
                    for (var iCount = 0; iCount < len; iCount++)
                    {
                        var element = arr[iCount];
                        if (element is DomCharElement)
                        {
                            if (element._StyleIndex != lastCharStyleIndex)
                            {
                                if (strLastText.Length > 0)
                                {
                                    Write_XTextTextElement(thisWriter, strLastText.ToString(), lastCharStyleIndex);
                                    strLastText.Clear();
                                }
                                lastCharStyleIndex = element._StyleIndex;
                            }
                            ((DomCharElement)element).AppendContent(strLastText);
                        }
                        else
                        {
                            if (strLastText.Length > 0)
                            {
                                Write_XTextTextElement(thisWriter, strLastText.ToString(), lastCharStyleIndex);
                                strLastText.Clear();
                            }
                            if (iCount == len - 1
                                && element is DomParagraphFlagElement
                                && ((DomParagraphFlagElement)element).AutoCreate
                                && element._StyleIndex < 0)
                            {
                                continue;
                            }
                            Write3_XTextElement("Element", string.Empty, element, true, false);
                        }
                    }//for
                    if (strLastText.Length > 0)
                    {
                        Write_XTextTextElement(thisWriter, strLastText.ToString(), lastCharStyleIndex);
                        strLastText.Clear();
                    }
                }
                thisWriter.WriteEndElement();
            }
        }

        private static void Write_XTextTextElement(XmlWriter writer, string text, int styleIndex)
        {
            writer.WriteStartElement("Element");
            writer.WriteAttributeString("xsi:type", "XString");
            if (styleIndex >= 0)
            {
                writer.WriteAttributeString("StyleIndex", DCSoft.Common.StringCommon.Int32ToString(styleIndex));
            }
            writer.WriteElementString("Text", text);
            writer.WriteEndElement();
        }


        private DCSystemXml.XmlWriter _BaseWriter = null;
        private void MyWriteElementString(string localName, string value)
        {
            if (value != null && value.Length > 0)
            {
                this._BaseWriter.WriteElementString(localName, null, value);
            }
        }
        private void MyWriteElementStringRaw(string localName, float Value)
        {
            this._BaseWriter.WriteStartElement(localName, null);
            this._BaseWriter.WriteRaw( DCSystemXml.XmlConvert.ToString( Value ));
            this._BaseWriter.WriteEndElement();
        }
        private void MyWriteElementStringRaw(string localName, double Value)
        {
            this._BaseWriter.WriteStartElement(localName, null);
            this._BaseWriter.WriteRaw(DCSystemXml.XmlConvert.ToString(Value));
            this._BaseWriter.WriteEndElement();
        }
        private void MyWriteElementStringRaw(string localName, bool Value)
        {
            this._BaseWriter.WriteStartElement(localName, null);
            this._BaseWriter.WriteRaw(Value ? "true" : "false");
            this._BaseWriter.WriteEndElement();
        }
        private void MyWriteElementStringRaw(string localName, string Value)
        {
            if (Value != null && Value.Length > 0)
            {
                this._BaseWriter.WriteStartElement(localName, null);
                this._BaseWriter.WriteRaw(Value);
                this._BaseWriter.WriteEndElement();
            }
        }
        public void Write261_XTextDocument(object o)
        {
            this._BaseWriter = base.Writer;
            //var localWriter = base.Writer;
            WriteStartDocument();
            if (o == null)
            {
                WriteNullTagLiteral(@"XTextDocument", string.Empty);
                return;
            }
            TopLevelElement();
            Write180_XTextDocument(@"XTextDocument", string.Empty, ((DCSoft.Writer.Dom.DomDocument)o), true, false);
        }
        internal protected void Write180_XTextDocument(string n, string ns, DCSoft.Writer.Dom.DomDocument o, bool isNullable, bool needType)
        {
            this._BaseWriter = base.Writer;
            var localWriter = base.Writer;
            if (o == null)
            {
                //if (isNullable) WriteNullTagLiteral(n, ns);
                return;
            }
            WriteStartElement(n, ns, o, false, null);
            if (needType) WriteXsiType(@"XTextDocument", string.Empty);
            if ((o.@StyleIndex) != -1)
            {
                localWriter.WriteAttributeString(@"StyleIndex", null, o.StyleIndex);
            }
           WriteAttribute(@"EditorVersionString", string.Empty, (o.@EditorVersionString));
            MyWriteElementString(@"ID", (o.@ID));
            WriteContainerChildElements(o);
            MyWriteElementString(@"FileName", (o.@FileName));
            MyWriteElementString(@"FileFormat", (o.@FileFormat));
            Write51_DocumentContentStyleContainer(@"ContentStyles", string.Empty, (o.@ContentStyles), false, false);
            Write56_DocumentInfo(@"Info", string.Empty, (o.@Info), false, false);
            MyWriteElementString(@"BodyText", (o.@BodyText));
            Write71_XPageSettings(@"PageSettings", string.Empty, (o.@PageSettings), false, false);
            WriteEndElement(o);
        }
        internal protected void Write71_XPageSettings(string n, string ns, DCSoft.Printing.XPageSettings o, bool isNullable, bool needType)
        {
            this._BaseWriter = base.Writer;
            var localWriter = base.Writer;
            if (o == null)
            {
                return;
            }
            WriteStartElement(n, ns, o, false, null);
            if (needType) WriteXsiType(@"XPageSettings", string.Empty);
            if ((o.@PowerDocumentGridLine) != false)
            {
                MyWriteElementStringRaw(@"PowerDocumentGridLine", o.PowerDocumentGridLine);
            }
            Write63_DCGridLineInfo(@"DocumentGridLine", string.Empty, (o.@DocumentGridLine), false, false);
            if ((o.@PaperKind) != PaperKind.@A4)
            {
                localWriter.WriteElementString(@"PaperKind", null, Write70_PaperKind((o.@PaperKind)));
            }
            if ((o.@HeaderDistance) != 50)
            {
                MyWriteElementStringRaw(@"HeaderDistance", o.HeaderDistance);
            }
            if ((o.@FooterDistance) != 50)
            {
                MyWriteElementStringRaw(@"FooterDistance", o.FooterDistance);
            }
            if ((o.@DesignerPaperWidth) != 0)
            {
                MyWriteElementStringRaw(@"DesignerPaperWidth", o.DesignerPaperWidth);
            }
            if ((o.@PaperWidth) != 827)
            {
                MyWriteElementStringRaw(@"PaperWidth", o.PaperWidth);
            }
            if ((o.@DesignerPaperHeight) != 0)
            {
                MyWriteElementStringRaw(@"DesignerPaperHeight", o.DesignerPaperHeight);
            }
            if ((o.@PaperHeight) != 1169)
            {
                MyWriteElementStringRaw(@"PaperHeight", o.PaperHeight);
            }
            if ((o.@LeftMargin) != 100)
            {
                MyWriteElementStringRaw(@"LeftMargin", o.LeftMargin);
            }
            if ((o.@TopMargin) != 100)
            {
                MyWriteElementStringRaw(@"TopMargin", o.TopMargin);
            }
            if ((o.@RightMargin) != 100)
            {
                MyWriteElementStringRaw(@"RightMargin", o.RightMargin);
            }
            if ((o.@BottomMargin) != 100)
            {
                MyWriteElementStringRaw(@"BottomMargin", o.BottomMargin);
            }
            if ((o.@Landscape) != false)
            {
                MyWriteElementStringRaw(@"Landscape", o.Landscape);
            }
            WriteEndElement(o);
        }
        string Write70_PaperKind(PaperKind v)
        {
            string s = null;
            switch (v)
            {
                case PaperKind.@Custom: s = @"Custom"; break;
                case PaperKind.@Letter: s = @"Letter"; break;
                case PaperKind.@Legal: s = @"Legal"; break;
                case PaperKind.@A4: s = @"A4"; break;
                case PaperKind.@CSheet: s = @"CSheet"; break;
                case PaperKind.@DSheet: s = @"DSheet"; break;
                case PaperKind.@ESheet: s = @"ESheet"; break;
                case PaperKind.@LetterSmall: s = @"LetterSmall"; break;
                case PaperKind.@Tabloid: s = @"Tabloid"; break;
                case PaperKind.@Ledger: s = @"Ledger"; break;
                case PaperKind.@Statement: s = @"Statement"; break;
                case PaperKind.@Executive: s = @"Executive"; break;
                case PaperKind.@A3: s = @"A3"; break;
                case PaperKind.@A4Small: s = @"A4Small"; break;
                case PaperKind.@A5: s = @"A5"; break;
                case PaperKind.@B4: s = @"B4"; break;
                case PaperKind.@B5: s = @"B5"; break;
                case PaperKind.@Folio: s = @"Folio"; break;
                case PaperKind.@Quarto: s = @"Quarto"; break;
                case PaperKind.@Standard10x14: s = @"Standard10x14"; break;
                case PaperKind.@Standard11x17: s = @"Standard11x17"; break;
                case PaperKind.@Note: s = @"Note"; break;
                case PaperKind.@Number9Envelope: s = @"Number9Envelope"; break;
                case PaperKind.@Number10Envelope: s = @"Number10Envelope"; break;
                case PaperKind.@Number11Envelope: s = @"Number11Envelope"; break;
                case PaperKind.@Number12Envelope: s = @"Number12Envelope"; break;
                case PaperKind.@Number14Envelope: s = @"Number14Envelope"; break;
                case PaperKind.@DLEnvelope: s = @"DLEnvelope"; break;
                case PaperKind.@C5Envelope: s = @"C5Envelope"; break;
                case PaperKind.@C3Envelope: s = @"C3Envelope"; break;
                case PaperKind.@C4Envelope: s = @"C4Envelope"; break;
                case PaperKind.@C6Envelope: s = @"C6Envelope"; break;
                case PaperKind.@C65Envelope: s = @"C65Envelope"; break;
                case PaperKind.@B4Envelope: s = @"B4Envelope"; break;
                case PaperKind.@B5Envelope: s = @"B5Envelope"; break;
                case PaperKind.@B6Envelope: s = @"B6Envelope"; break;
                case PaperKind.@ItalyEnvelope: s = @"ItalyEnvelope"; break;
                case PaperKind.@MonarchEnvelope: s = @"MonarchEnvelope"; break;
                case PaperKind.@PersonalEnvelope: s = @"PersonalEnvelope"; break;
                case PaperKind.@USStandardFanfold: s = @"USStandardFanfold"; break;
                case PaperKind.@GermanStandardFanfold: s = @"GermanStandardFanfold"; break;
                case PaperKind.@GermanLegalFanfold: s = @"GermanLegalFanfold"; break;
                case PaperKind.@IsoB4: s = @"IsoB4"; break;
                case PaperKind.@JapanesePostcard: s = @"JapanesePostcard"; break;
                case PaperKind.@Standard9x11: s = @"Standard9x11"; break;
                case PaperKind.@Standard10x11: s = @"Standard10x11"; break;
                case PaperKind.@Standard15x11: s = @"Standard15x11"; break;
                case PaperKind.@InviteEnvelope: s = @"InviteEnvelope"; break;
                case PaperKind.@LetterExtra: s = @"LetterExtra"; break;
                case PaperKind.@LegalExtra: s = @"LegalExtra"; break;
                case PaperKind.@TabloidExtra: s = @"TabloidExtra"; break;
                case PaperKind.@A4Extra: s = @"A4Extra"; break;
                case PaperKind.@LetterTransverse: s = @"LetterTransverse"; break;
                case PaperKind.@A4Transverse: s = @"A4Transverse"; break;
                case PaperKind.@LetterExtraTransverse: s = @"LetterExtraTransverse"; break;
                case PaperKind.@APlus: s = @"APlus"; break;
                case PaperKind.@BPlus: s = @"BPlus"; break;
                case PaperKind.@LetterPlus: s = @"LetterPlus"; break;
                case PaperKind.@A4Plus: s = @"A4Plus"; break;
                case PaperKind.@A5Transverse: s = @"A5Transverse"; break;
                case PaperKind.@B5Transverse: s = @"B5Transverse"; break;
                case PaperKind.@A3Extra: s = @"A3Extra"; break;
                case PaperKind.@A5Extra: s = @"A5Extra"; break;
                case PaperKind.@B5Extra: s = @"B5Extra"; break;
                case PaperKind.@A2: s = @"A2"; break;
                case PaperKind.@A3Transverse: s = @"A3Transverse"; break;
                case PaperKind.@A3ExtraTransverse: s = @"A3ExtraTransverse"; break;
                case PaperKind.@JapaneseDoublePostcard: s = @"JapaneseDoublePostcard"; break;
                case PaperKind.@A6: s = @"A6"; break;
                case PaperKind.@JapaneseEnvelopeKakuNumber2: s = @"JapaneseEnvelopeKakuNumber2"; break;
                case PaperKind.@JapaneseEnvelopeKakuNumber3: s = @"JapaneseEnvelopeKakuNumber3"; break;
                case PaperKind.@JapaneseEnvelopeChouNumber3: s = @"JapaneseEnvelopeChouNumber3"; break;
                case PaperKind.@JapaneseEnvelopeChouNumber4: s = @"JapaneseEnvelopeChouNumber4"; break;
                case PaperKind.@LetterRotated: s = @"LetterRotated"; break;
                case PaperKind.@A3Rotated: s = @"A3Rotated"; break;
                case PaperKind.@A4Rotated: s = @"A4Rotated"; break;
                case PaperKind.@A5Rotated: s = @"A5Rotated"; break;
                case PaperKind.@B4JisRotated: s = @"B4JisRotated"; break;
                case PaperKind.@B5JisRotated: s = @"B5JisRotated"; break;
                case PaperKind.@JapanesePostcardRotated: s = @"JapanesePostcardRotated"; break;
                case PaperKind.@JapaneseDoublePostcardRotated: s = @"JapaneseDoublePostcardRotated"; break;
                case PaperKind.@A6Rotated: s = @"A6Rotated"; break;
                case PaperKind.@JapaneseEnvelopeKakuNumber2Rotated: s = @"JapaneseEnvelopeKakuNumber2Rotated"; break;
                case PaperKind.@JapaneseEnvelopeKakuNumber3Rotated: s = @"JapaneseEnvelopeKakuNumber3Rotated"; break;
                case PaperKind.@JapaneseEnvelopeChouNumber3Rotated: s = @"JapaneseEnvelopeChouNumber3Rotated"; break;
                case PaperKind.@JapaneseEnvelopeChouNumber4Rotated: s = @"JapaneseEnvelopeChouNumber4Rotated"; break;
                case PaperKind.@B6Jis: s = @"B6Jis"; break;
                case PaperKind.@B6JisRotated: s = @"B6JisRotated"; break;
                case PaperKind.@Standard12x11: s = @"Standard12x11"; break;
                case PaperKind.@JapaneseEnvelopeYouNumber4: s = @"JapaneseEnvelopeYouNumber4"; break;
                case PaperKind.@JapaneseEnvelopeYouNumber4Rotated: s = @"JapaneseEnvelopeYouNumber4Rotated"; break;
                case PaperKind.@Prc16K: s = @"Prc16K"; break;
                case PaperKind.@Prc32K: s = @"Prc32K"; break;
                case PaperKind.@Prc32KBig: s = @"Prc32KBig"; break;
                case PaperKind.@PrcEnvelopeNumber1: s = @"PrcEnvelopeNumber1"; break;
                case PaperKind.@PrcEnvelopeNumber2: s = @"PrcEnvelopeNumber2"; break;
                case PaperKind.@PrcEnvelopeNumber3: s = @"PrcEnvelopeNumber3"; break;
                case PaperKind.@PrcEnvelopeNumber4: s = @"PrcEnvelopeNumber4"; break;
                case PaperKind.@PrcEnvelopeNumber5: s = @"PrcEnvelopeNumber5"; break;
                case PaperKind.@PrcEnvelopeNumber6: s = @"PrcEnvelopeNumber6"; break;
                case PaperKind.@PrcEnvelopeNumber7: s = @"PrcEnvelopeNumber7"; break;
                case PaperKind.@PrcEnvelopeNumber8: s = @"PrcEnvelopeNumber8"; break;
                case PaperKind.@PrcEnvelopeNumber9: s = @"PrcEnvelopeNumber9"; break;
                case PaperKind.@PrcEnvelopeNumber10: s = @"PrcEnvelopeNumber10"; break;
                case PaperKind.@Prc16KRotated: s = @"Prc16KRotated"; break;
                case PaperKind.@Prc32KRotated: s = @"Prc32KRotated"; break;
                case PaperKind.@Prc32KBigRotated: s = @"Prc32KBigRotated"; break;
                case PaperKind.@PrcEnvelopeNumber1Rotated: s = @"PrcEnvelopeNumber1Rotated"; break;
                case PaperKind.@PrcEnvelopeNumber2Rotated: s = @"PrcEnvelopeNumber2Rotated"; break;
                case PaperKind.@PrcEnvelopeNumber3Rotated: s = @"PrcEnvelopeNumber3Rotated"; break;
                case PaperKind.@PrcEnvelopeNumber4Rotated: s = @"PrcEnvelopeNumber4Rotated"; break;
                case PaperKind.@PrcEnvelopeNumber5Rotated: s = @"PrcEnvelopeNumber5Rotated"; break;
                case PaperKind.@PrcEnvelopeNumber6Rotated: s = @"PrcEnvelopeNumber6Rotated"; break;
                case PaperKind.@PrcEnvelopeNumber7Rotated: s = @"PrcEnvelopeNumber7Rotated"; break;
                case PaperKind.@PrcEnvelopeNumber8Rotated: s = @"PrcEnvelopeNumber8Rotated"; break;
                case PaperKind.@PrcEnvelopeNumber9Rotated: s = @"PrcEnvelopeNumber9Rotated"; break;
                case PaperKind.@PrcEnvelopeNumber10Rotated: s = @"PrcEnvelopeNumber10Rotated"; break;
                default: throw XSerHelper_XTextDocument.CreateInvalidEnumValueException((long)v, typeof(PaperKind));
            }
            return s;
        }
        internal protected void Write34_XImageValue(string n, string ns, DCSoft.Drawing.XImageValue o, bool isNullable, bool needType)
        {
            this._BaseWriter = base.Writer;
            //var localWriter = base.Writer;
            if (o == null)
            {
                //if (isNullable) WriteNullTagLiteral(n, ns);
                return;
            }
            WriteStartElement(n, ns, o, false, null);
            if (needType) WriteXsiType(@"XImageValue", string.Empty);
            MyWriteElementString(@"ImageDataBase64String", (o.@ImageDataBase64String));
            WriteEndElement(o);
        }
        string Write44_ParagraphListStyle(DCSoft.Drawing.ParagraphListStyle v)
        {
            string s = null;
            switch (v)
            {
                case DCSoft.Drawing.ParagraphListStyle.@None: s = @"None"; break;
                case DCSoft.Drawing.ParagraphListStyle.@ListNumberStyle: s = @"ListNumberStyle"; break;
                case DCSoft.Drawing.ParagraphListStyle.@ListNumberStyleArabic1: s = @"ListNumberStyleArabic1"; break;
                case DCSoft.Drawing.ParagraphListStyle.@ListNumberStyleArabic2: s = @"ListNumberStyleArabic2"; break;
                case DCSoft.Drawing.ParagraphListStyle.@ListNumberStyleArabic3: s = @"ListNumberStyleArabic3"; break;
                case DCSoft.Drawing.ParagraphListStyle.@ListNumberStyleLowercaseLetter: s = @"ListNumberStyleLowercaseLetter"; break;
                case DCSoft.Drawing.ParagraphListStyle.@ListNumberStyleLowercaseRoman: s = @"ListNumberStyleLowercaseRoman"; break;
                case DCSoft.Drawing.ParagraphListStyle.@ListNumberStyleNumberInCircle: s = @"ListNumberStyleNumberInCircle"; break;
                case DCSoft.Drawing.ParagraphListStyle.@ListNumberStyleUppercaseLetter: s = @"ListNumberStyleUppercaseLetter"; break;
                case DCSoft.Drawing.ParagraphListStyle.@ListNumberStyleUppercaseRoman: s = @"ListNumberStyleUppercaseRoman"; break;
                case DCSoft.Drawing.ParagraphListStyle.@ListNumberStyleZodiac1: s = @"ListNumberStyleZodiac1"; break;
                case DCSoft.Drawing.ParagraphListStyle.@ListNumberStyleZodiac2: s = @"ListNumberStyleZodiac2"; break;
                case DCSoft.Drawing.ParagraphListStyle.@BulletedList: s = @"BulletedList"; break;
                case DCSoft.Drawing.ParagraphListStyle.@BulletedListBlock: s = @"BulletedListBlock"; break;
                case DCSoft.Drawing.ParagraphListStyle.@BulletedListDiamond: s = @"BulletedListDiamond"; break;
                case DCSoft.Drawing.ParagraphListStyle.@BulletedListCheck: s = @"BulletedListCheck"; break;
                case DCSoft.Drawing.ParagraphListStyle.@BulletedListRightArrow: s = @"BulletedListRightArrow"; break;
                case DCSoft.Drawing.ParagraphListStyle.@BulletedListHollowStar: s = @"BulletedListHollowStar"; break;
                default: throw XSerHelper_XTextDocument.CreateInvalidEnumValueException((long)v, typeof(DCSoft.Drawing.ParagraphListStyle));
            }
            return s;
        }
        string Write43_DashStyle(DashStyle v)
        {
            string s = null;
            switch (v)
            {
                case DashStyle.@Solid: s = @"Solid"; break;
                case DashStyle.@Dash: s = @"Dash"; break;
                case DashStyle.@Dot: s = @"Dot"; break;
                case DashStyle.@DashDot: s = @"DashDot"; break;
                case DashStyle.@DashDotDot: s = @"DashDotDot"; break;
                case DashStyle.@Custom: s = @"Custom"; break;
                default: throw XSerHelper_XTextDocument.CreateInvalidEnumValueException((long)v, typeof(DashStyle));
            }
            return s;
        }
        string Write40_VerticalAlignStyle(DCSoft.Drawing.VerticalAlignStyle v)
        {
            string s = null;
            switch (v)
            {
                case DCSoft.Drawing.VerticalAlignStyle.@Top: s = @"Top"; break;
                case DCSoft.Drawing.VerticalAlignStyle.@Middle: s = @"Middle"; break;
                case DCSoft.Drawing.VerticalAlignStyle.@Bottom: s = @"Bottom"; break;
                default: throw XSerHelper_XTextDocument.CreateInvalidEnumValueException((long)v, typeof(DCSoft.Drawing.VerticalAlignStyle));
            }
            return s;
        }
        string Write39_DocumentContentAlignment(DCSoft.Drawing.DocumentContentAlignment v)
        {
            string s = null;
            switch (v)
            {
                case DCSoft.Drawing.DocumentContentAlignment.@Left: s = @"Left"; break;
                case DCSoft.Drawing.DocumentContentAlignment.@Center: s = @"Center"; break;
                case DCSoft.Drawing.DocumentContentAlignment.@Right: s = @"Right"; break;
                case DCSoft.Drawing.DocumentContentAlignment.@Justify: s = @"Justify"; break;
                case DCSoft.Drawing.DocumentContentAlignment.@Distribute: s = @"Distribute"; break;
                default: throw XSerHelper_XTextDocument.CreateInvalidEnumValueException((long)v, typeof(DCSoft.Drawing.DocumentContentAlignment));
            }
            return s;
        }
        string Write38_LineSpacingStyle(DCSoft.Drawing.LineSpacingStyle v)
        {
            string s = null;
            switch (v)
            {
                case DCSoft.Drawing.LineSpacingStyle.@SpaceSingle: s = @"SpaceSingle"; break;
                case DCSoft.Drawing.LineSpacingStyle.@Space1pt5: s = @"Space1pt5"; break;
                case DCSoft.Drawing.LineSpacingStyle.@SpaceDouble: s = @"SpaceDouble"; break;
                case DCSoft.Drawing.LineSpacingStyle.@SpaceExactly: s = @"SpaceExactly"; break;
                case DCSoft.Drawing.LineSpacingStyle.@SpaceSpecify: s = @"SpaceSpecify"; break;
                case DCSoft.Drawing.LineSpacingStyle.@SpaceMultiple: s = @"SpaceMultiple"; break;
                default: throw XSerHelper_XTextDocument.CreateInvalidEnumValueException((long)v, typeof(DCSoft.Drawing.LineSpacingStyle));
            }
            return s;
        }
        internal protected void Write63_DCGridLineInfo(string n, string ns, DCSoft.Printing.DCGridLineInfo o, bool isNullable, bool needType)
        {
            this._BaseWriter = base.Writer;
            var localWriter = base.Writer;
            if (o == null)
            {
                //if (isNullable) WriteNullTagLiteral(n, ns);
                return;
            }
            WriteStartElement(n, ns, o, false, null);
            if (needType) WriteXsiType(@"DCGridLineInfo", string.Empty);
            if ((o.@Visible) != false)
            {
                MyWriteElementStringRaw(@"Visible", o.Visible);
            }
            if ((o.@AlignToGridLine) != true)
            {
                MyWriteElementStringRaw(@"AlignToGridLine", o.AlignToGridLine);
            }
            MyWriteElementString(@"ColorValue", (o.@ColorValue));
            if ((o.@LineWidth) != 1f)
            {
                MyWriteElementStringRaw(@"LineWidth", o.LineWidth);
            }
            if ((o.@GridNumInOnePage) != 0)
            {
                MyWriteElementStringRaw(@"GridNumInOnePage", o.GridNumInOnePage);
            }
            if ((o.@GridSpanInCM) != 0f)
            {
                MyWriteElementStringRaw(@"GridSpanInCM", o.GridSpanInCM);
            }
            if ((o.@LineStyle) != DashStyle.@Solid)
            {
                localWriter.WriteElementString(@"LineStyle", null, Write43_DashStyle((o.@LineStyle)));
            }
            if ((o.@Printable) != true)
            {
                MyWriteElementStringRaw(@"Printable", o.Printable);
            }
            WriteEndElement(o);
        }
        internal protected void Write56_DocumentInfo(string n, string ns, DCSoft.Writer.Dom.DocumentInfo o, bool isNullable, bool needType)
        {
            this._BaseWriter = base.Writer;
            var localWriter = base.Writer;
            if (o == null)
            {
                //if (isNullable) WriteNullTagLiteral(n, ns);
                return;
            }
            WriteStartElement(n, ns, o, false, null);
            if (needType) WriteXsiType(@"DocumentInfo", string.Empty);
            if ((o.@Readonly) != false)
            {
                MyWriteElementStringRaw(@"Readonly", o.Readonly);
            }
            if ((o.@ShowHeaderBottomLine) != DCSoft.Writer.DCBooleanValue.@Inherit)
            {
                localWriter.WriteElementString(@"ShowHeaderBottomLine", null, Write18_DCBooleanValue((o.@ShowHeaderBottomLine)));
            }
            if ((o.@FieldBorderElementWidth) != 1f)
            {
                MyWriteElementStringRaw(@"FieldBorderElementWidth", o.FieldBorderElementWidth);
            }
            MyWriteElementString(@"ID", (o.@ID));
            if ((o.@IsTemplate) != false)
            {
                MyWriteElementStringRaw(@"IsTemplate", o.IsTemplate);
            }
            MyWriteElementString(@"Version", (o.@Version));
            MyWriteElementString(@"Title", (o.@Title));
            MyWriteElementString(@"Description", (o.@Description));
            MyWriteElementStringRaw(@"CreationTime", FromDateTime((o.@CreationTime)));
            MyWriteElementStringRaw(@"LastModifiedTime", FromDateTime((o.@LastModifiedTime)));
            if ((o.@EditMinute) != 0)
            {
                MyWriteElementStringRaw(@"EditMinute", o.EditMinute);
            }
            MyWriteElementStringRaw(@"LastPrintTime", FromDateTime((o.@LastPrintTime)));
            MyWriteElementString(@"Author", (o.@Author));
            MyWriteElementString(@"AuthorName", (o.@AuthorName));
            MyWriteElementString(@"DepartmentID", (o.@DepartmentID));
            MyWriteElementString(@"DepartmentName", (o.@DepartmentName));
            MyWriteElementString(@"DocumentFormat", (o.@DocumentFormat));
            MyWriteElementString(@"DocumentType", (o.@DocumentType));
            if ((o.@DocumentProcessState) != 0)
            {
                MyWriteElementStringRaw(@"DocumentProcessState", o.DocumentProcessState);
            }
            if ((o.@DocumentEditState) != 0)
            {
                MyWriteElementStringRaw(@"DocumentEditState", o.DocumentEditState);
            }
            MyWriteElementString(@"Comment", (o.@Comment));
            MyWriteElementString(@"Operator", (o.@Operator));
            if ((o.@NumOfPage) != 0)
            {
                MyWriteElementStringRaw(@"NumOfPage", o.NumOfPage);
            }
            if ((o.@Printable) != true)
            {
                MyWriteElementStringRaw(@"Printable", o.Printable);
            }
            if ((o.@StartPositionInPringJob) != 0)
            {
                MyWriteElementStringRaw(@"StartPositionInPringJob", o.StartPositionInPringJob);
            }
            if ((o.@HeightInPrintJob) != 0)
            {
                MyWriteElementStringRaw(@"HeightInPrintJob", o.HeightInPrintJob);
            }
            WriteEndElement(o);
        }
        string Write18_DCBooleanValue(DCSoft.Writer.DCBooleanValue v)
        {
            string s = null;
            switch (v)
            {
                case DCSoft.Writer.DCBooleanValue.@True: s = @"True"; break;
                case DCSoft.Writer.DCBooleanValue.@False: s = @"False"; break;
                case DCSoft.Writer.DCBooleanValue.@Inherit: s = @"Inherit"; break;
                default: throw XSerHelper_XTextDocument.CreateInvalidEnumValueException((long)v, typeof(DCSoft.Writer.DCBooleanValue));
            }
            return s;
        }
        internal protected void Write51_DocumentContentStyleContainer(string n, string ns, DCSoft.Writer.Dom.DocumentContentStyleContainer o, bool isNullable, bool needType)
        {
            this._BaseWriter = base.Writer;
            var localWriter = base.Writer;
            if (o == null)
            {
                //if (isNullable) WriteNullTagLiteral(n, ns);
                return;
            }
            WriteStartElement(n, ns, o, false, null);
            if (needType) WriteXsiType(@"DocumentContentStyleContainer", string.Empty);
            Write50_DocumentContentStyle(@"Default", string.Empty, (DCSoft.Writer.Dom.DocumentContentStyle)o.@Default, false, false);
            {
                DCSoft.Drawing.ContentStyleList a = (DCSoft.Drawing.ContentStyleList)(o.@Styles);
                if (a != null && a.Count > 0)
                {
                    var aCount = a.Count;//2222222
                    localWriter.WriteStartElement(null, @"Styles", null);
                    for (int ia = 0; ia < aCount; ia++)
                    {
                        Write50_DocumentContentStyle(@"Style", string.Empty, ((DCSoft.Writer.Dom.DocumentContentStyle)a[ia]), true, false);
                    }
                    localWriter.WriteEndElement();
                }
            }
            WriteEndElement(o);
        }
        internal protected void Write50_DocumentContentStyle(string n, string ns, DCSoft.Writer.Dom.DocumentContentStyle o, bool isNullable, bool needType)
        {
            this._BaseWriter = base.Writer;
            var localWriter = base.Writer;
            if (o == null)
            {
                //if (isNullable) WriteNullTagLiteral(n, ns);
                return;
            }
            WriteStartElement(n, ns, o, false, null);
            if (needType) WriteXsiType(@"DocumentContentStyle", string.Empty);
            if ((o.@Index) != -1)
            {
                localWriter.WriteAttributeString(@"Index", null, o.Index);
            }
            MyWriteElementString(@"BackgroundColor", (o.@BackgroundColorString));
            MyWriteElementString(@"Color", (o.@ColorString));
            MyWriteElementString(@"FontName", (o.@FontName));
            if ((o.@FontSize) != 0f)
            {
                MyWriteElementStringRaw(@"FontSize", o.FontSize);
            }
            if ((o.@Bold) != false)
            {
                MyWriteElementStringRaw(@"Bold", o.Bold);
            }
            if ((o.@Italic) != false)
            {
                MyWriteElementStringRaw(@"Italic", o.Italic);
            }
            if ((o.@Underline) != false)
            {
                MyWriteElementStringRaw(@"Underline", o.Underline);
            }
            if ((o.@Strikeout) != false)
            {
                MyWriteElementStringRaw(@"Strikeout", o.Strikeout);
            }
            if ((o.@Superscript) != false)
            {
                MyWriteElementStringRaw(@"Superscript", o.Superscript);
            }
            if ((o.@Subscript) != false)
            {
                MyWriteElementStringRaw(@"Subscript", o.Subscript);
            }
            if ((o.@SpacingAfterParagraph) != 0f)
            {
                MyWriteElementStringRaw(@"SpacingAfterParagraph", o.SpacingAfterParagraph);
            }
            if ((o.@SpacingBeforeParagraph) != 0f)
            {
                MyWriteElementStringRaw(@"SpacingBeforeParagraph", o.SpacingBeforeParagraph);
            }
            if ((o.@LineSpacingStyle) != DCSoft.Drawing.LineSpacingStyle.@SpaceSingle)
            {
                localWriter.WriteElementString(@"LineSpacingStyle", null, Write38_LineSpacingStyle((o.@LineSpacingStyle)));
            }
            if ((o.@LineSpacing) != 0f)
            {
                MyWriteElementStringRaw(@"LineSpacing", o.LineSpacing);
            }
            if ((o.@Align) != DCSoft.Drawing.DocumentContentAlignment.@Left)
            {
                localWriter.WriteElementString(@"Align", null, Write39_DocumentContentAlignment((o.@Align)));
            }
            if ((o.@VerticalAlign) != DCSoft.Drawing.VerticalAlignStyle.@Top)
            {
                localWriter.WriteElementString(@"VerticalAlign", null, Write40_VerticalAlignStyle((o.@VerticalAlign)));
            }
            if ((o.@FirstLineIndent) != 0f)
            {
                MyWriteElementStringRaw(@"FirstLineIndent", o.FirstLineIndent);
            }
            if ((o.@LeftIndent) != 0f)
            {
                MyWriteElementStringRaw(@"LeftIndent", o.LeftIndent);
            }
            MyWriteElementString(@"BorderLeftColor", (o.@BorderLeftColorString));
            MyWriteElementString(@"BorderTopColor", (o.@BorderTopColorString));
            MyWriteElementString(@"BorderRightColor", (o.@BorderRightColorString));
            MyWriteElementString(@"BorderBottomColor", (o.@BorderBottomColorString));
            if ((o.@BorderStyle) != DashStyle.@Solid)
            {
                localWriter.WriteElementString(@"BorderStyle", null, Write43_DashStyle((o.@BorderStyle)));
            }
            if ((o.@BorderWidth) != 0f)
            {
                MyWriteElementStringRaw(@"BorderWidth", o.BorderWidth);
            }
            if ((o.@BorderLeft) != false)
            {
                MyWriteElementStringRaw(@"BorderLeft", o.BorderLeft);
            }
            if ((o.@BorderBottom) != false)
            {
                MyWriteElementStringRaw(@"BorderBottom", o.BorderBottom);
            }
            if ((o.@BorderTop) != false)
            {
                MyWriteElementStringRaw(@"BorderTop", o.BorderTop);
            }
            if ((o.@BorderRight) != false)
            {
                MyWriteElementStringRaw(@"BorderRight", o.BorderRight);
            }
            if ((o.@PaddingLeft) != 0f)
            {
                MyWriteElementStringRaw(@"PaddingLeft", o.PaddingLeft);
            }
            if ((o.@PaddingTop) != 0f)
            {
                MyWriteElementStringRaw(@"PaddingTop", o.PaddingTop);
            }
            if ((o.@PaddingRight) != 0f)
            {
                MyWriteElementStringRaw(@"PaddingRight", o.PaddingRight);
            }
            if ((o.@PaddingBottom) != 0f)
            {
                MyWriteElementStringRaw(@"PaddingBottom", o.PaddingBottom);
            }
            if ((o.@ParagraphMultiLevel) != false)
            {
                MyWriteElementStringRaw(@"ParagraphMultiLevel", o.ParagraphMultiLevel);
            }
            if ((o.@ParagraphOutlineLevel) != -1)
            {
                MyWriteElementStringRaw(@"ParagraphOutlineLevel", o.ParagraphOutlineLevel);
            }
            if ((o.@ParagraphListStyle) != DCSoft.Drawing.ParagraphListStyle.@None)
            {
                localWriter.WriteElementString(@"ParagraphListStyle", null, Write44_ParagraphListStyle((o.@ParagraphListStyle)));
            }
            WriteEndElement(o);
        }
        internal protected void Write3_XTextElement(string n, string ns, DCSoft.Writer.Dom.DomElement o, bool isNullable, bool needType)
        {
            this._BaseWriter = base.Writer;
            if (o == null)
            {
                return;
            }
            if (!needType)
            {
                System.Type t = o.GetType();
                if (t == typeof(DCSoft.Writer.Dom.DomElement))
                {
                }
                else if (t == typeof(DCSoft.Writer.Dom.DomParagraphFlagElement))
                {
                    Write76_XTextParagraphFlagElement(n, ns, (DCSoft.Writer.Dom.DomParagraphFlagElement)o, isNullable, true);
                    return;
                }

                else if (t == typeof(DCSoft.Writer.Dom.DomInputFieldElement))
                {
                    Write138_XTextInputFieldElement(n, ns, (DCSoft.Writer.Dom.DomInputFieldElement)o, isNullable, true);
                    return;
                }
                else if (t == typeof(DCSoft.Writer.Dom.DomTableCellElement))
                {
                    Write112_XTextTableCellElement(n, ns, (DCSoft.Writer.Dom.DomTableCellElement)o, isNullable, true);
                    return;
                }
                else if (t == typeof(DCSoft.Writer.Dom.DomRadioBoxElement))
                {
                    Write144_XTextRadioBoxElement(n, ns, (DCSoft.Writer.Dom.DomRadioBoxElement)o, isNullable, true);
                    return;
                }
                else if (t == typeof(DCSoft.Writer.Dom.DomCheckBoxElement))
                {
                    Write143_XTextCheckBoxElement(n, ns, (DCSoft.Writer.Dom.DomCheckBoxElement)o, isNullable, true);
                    return;
                }
                else if (t == typeof(DCSoft.Writer.Dom.DomImageElement))
                {
                    Write94_XTextImageElement(n, ns, (DCSoft.Writer.Dom.DomImageElement)o, isNullable, true);
                    return;
                }
                
                else if (t == typeof(DCSoft.Writer.Dom.DomPageBreakElement))
                {
                    Write113_XTextPageBreakElement(n, ns, (DCSoft.Writer.Dom.DomPageBreakElement)o, isNullable, true);
                    return;
                }
                else if (t == typeof(DCSoft.Writer.Dom.DomTableColumnElement))
                {
                    Write108_XTextTableColumnElement(n, ns, (DCSoft.Writer.Dom.DomTableColumnElement)o, isNullable, true);
                    return;
                }
                else if (t == typeof(DCSoft.Writer.Dom.DomLineBreakElement))
                {
                    Write95_XTextLineBreakElement(n, ns, (DCSoft.Writer.Dom.DomLineBreakElement)o, isNullable, true);
                    return;
                }
                else if (t == typeof(DCSoft.Writer.Dom.DomPageInfoElement))
                {
                    Write148_XTextPageInfoElement(n, ns, (DCSoft.Writer.Dom.DomPageInfoElement)o, isNullable, true);
                    return;
                }
                
                else if (t == typeof(DCSoft.Writer.Dom.DomTableRowElement))
                {
                    Write107_XTextTableRowElement(n, ns, (DCSoft.Writer.Dom.DomTableRowElement)o, isNullable, true);
                    return;
                }
                else if (t == typeof(DCSoft.Writer.Dom.DomTableElement))
                {
                    Write104_XTextTableElement(n, ns, (DCSoft.Writer.Dom.DomTableElement)o, isNullable, true);
                    return;
                }
                else if (t == typeof(DCSoft.Writer.Dom.DomDocumentFooterElement))
                {
                    Write99_XTextDocumentFooterElement(n, ns, (DCSoft.Writer.Dom.DomDocumentFooterElement)o, isNullable, true);
                    return;
                }
                else if (t == typeof(DCSoft.Writer.Dom.DomDocumentHeaderElement))
                {
                    Write98_XTextDocumentHeaderElement(n, ns, (DCSoft.Writer.Dom.DomDocumentHeaderElement)o, isNullable, true);
                    return;
                }
                else if (t == typeof(DCSoft.Writer.Dom.DomDocumentBodyElement))
                {
                    Write74_XTextDocumentBodyElement(n, ns, (DCSoft.Writer.Dom.DomDocumentBodyElement)o, isNullable, true);
                    return;
                }
                else if (t == typeof(DCSoft.Writer.Dom.DomDocument))
                {
                    Write180_XTextDocument(n, ns, (DCSoft.Writer.Dom.DomDocument)o, isNullable, true);
                    return;
                }
                else
                {
                    throw CreateUnknownTypeException(o);
                }
            }
        }
        internal protected void Write74_XTextDocumentBodyElement(string n, string ns, DCSoft.Writer.Dom.DomDocumentBodyElement o, bool isNullable, bool needType)
        {
            this._BaseWriter = base.Writer;
            var localWriter = base.Writer;
            if (o == null)
            {
                //if (isNullable) WriteNullTagLiteral(n, ns);
                return;
            }
            WriteStartElement(n, ns, o, false, null);
            if (needType) WriteXsiType(@"XTextBody", string.Empty);
            MyWriteElementString(@"ID", (o.@ID));
            MyWriteElementString(@"ToolTip", (o.@ToolTip));
            WriteContainerChildElements(o);
            if ((o.@Visible) != true)
            {
                MyWriteElementStringRaw(@"Visible", o.Visible);
            }
            WriteEndElement(o);
        }

        internal protected void Write14_ValueValidateStyle(string n, string ns, DCSoft.Common.ValueValidateStyle o, bool isNullable, bool needType)
        {
            this._BaseWriter = base.Writer;
            var localWriter = base.Writer;
            if (o == null)
            {
                //if (isNullable) WriteNullTagLiteral(n, ns);
                return;
            }
            WriteStartElement(n, ns, o, false, null);
            if (needType) WriteXsiType(@"ValueValidateStyle", string.Empty);
            if ((o.@Level) != DCSoft.Common.ValueValidateLevel.@Error)
            {
                localWriter.WriteElementString(@"Level", null, Write12_ValueValidateLevel((o.@Level)));
            }
            MyWriteElementString(@"ValueName", (o.@ValueName));
            if ((o.@Required) != false)
            {
                MyWriteElementStringRaw(@"Required", o.Required);
            }
            if ((o.@ValueType) != DCSoft.Common.ValueTypeStyle.@Text)
            {
                localWriter.WriteElementString(@"ValueType", null, Write13_ValueTypeStyle((o.@ValueType)));
            }
            if ((o.@BinaryLength) != false)
            {
                MyWriteElementStringRaw(@"BinaryLength", o.BinaryLength);
            }
            if ((o.@MaxLength) != 0)
            {
                MyWriteElementStringRaw(@"MaxLength", o.MaxLength);
            }
            if ((o.@MinLength) != 0)
            {
                MyWriteElementStringRaw(@"MinLength", o.MinLength);
            }
            if ((o.@CheckMaxValue) != false)
            {
                MyWriteElementStringRaw(@"CheckMaxValue", o.CheckMaxValue);
            }
            if ((o.@CheckMinValue) != false)
            {
                MyWriteElementStringRaw(@"CheckMinValue", o.CheckMinValue);
            }
            if ((o.@MaxValue) != 0)
            {
                MyWriteElementStringRaw(@"MaxValue", o.MaxValue);
            }
            if ((o.@MinValue) != 0)
            {
                MyWriteElementStringRaw(@"MinValue", o.MinValue);
            }
            if ((o.@CheckDecimalDigits) != false)
            {
                MyWriteElementStringRaw(@"CheckDecimalDigits", o.CheckDecimalDigits);
            }
            if ((o.@MaxDecimalDigits) != 0)
            {
                MyWriteElementStringRaw(@"MaxDecimalDigits", o.MaxDecimalDigits);
            }
            if ((o.@DateTimeMaxValue).Ticks != (624511296000000000))
            {
                MyWriteElementStringRaw(@"DateTimeMaxValue", FromDateTime((o.@DateTimeMaxValue)));
            }
            if ((o.@DateTimeMinValue).Ticks != (624511296000000000))
            {
                MyWriteElementStringRaw(@"DateTimeMinValue", FromDateTime((o.@DateTimeMinValue)));
            }
            if ((o.@RequiredInvalidateFlag) != false)
            {
                MyWriteElementStringRaw(@"RequiredInvalidateFlag", o.RequiredInvalidateFlag);
            }
            WriteEndElement(o);
        }
        string Write13_ValueTypeStyle(DCSoft.Common.ValueTypeStyle v)
        {
            string s = null;
            switch (v)
            {
                case DCSoft.Common.ValueTypeStyle.@Text: s = @"Text"; break;
                case DCSoft.Common.ValueTypeStyle.@Integer: s = @"Integer"; break;
                case DCSoft.Common.ValueTypeStyle.@Numeric: s = @"Numeric"; break;
                case DCSoft.Common.ValueTypeStyle.@Date: s = @"Date"; break;
                case DCSoft.Common.ValueTypeStyle.@Time: s = @"Time"; break;
                case DCSoft.Common.ValueTypeStyle.@DateTime: s = @"DateTime"; break;
                default: throw XSerHelper_XTextDocument.CreateInvalidEnumValueException((long)v, typeof(DCSoft.Common.ValueTypeStyle));
            }
            return s;
        }
        string Write12_ValueValidateLevel(DCSoft.Common.ValueValidateLevel v)
        {
            string s = null;
            switch (v)
            {
                case DCSoft.Common.ValueValidateLevel.@Error: s = @"Error"; break;
                case DCSoft.Common.ValueValidateLevel.@Warring: s = @"Warring"; break;
                case DCSoft.Common.ValueValidateLevel.@Info: s = @"Info"; break;
                default: throw XSerHelper_XTextDocument.CreateInvalidEnumValueException((long)v, typeof(DCSoft.Common.ValueValidateLevel));
            }
            return s;
        }
        internal protected void Write98_XTextDocumentHeaderElement(string n, string ns, DCSoft.Writer.Dom.DomDocumentHeaderElement o, bool isNullable, bool needType)
        {
            this._BaseWriter = base.Writer;
            var localWriter = base.Writer;
            if (o == null)
            {
                //if (isNullable) WriteNullTagLiteral(n, ns);
                return;
            }
            WriteStartElement(n, ns, o, false, null);
            if (needType) WriteXsiType(@"XTextHeader", string.Empty);
            MyWriteElementString(@"ID", (o.@ID));
            
            MyWriteElementString(@"ToolTip", (o.@ToolTip));
            if ((o.@AcceptTab) != false)
            {
                MyWriteElementStringRaw(@"AcceptTab", o.AcceptTab);
            }
            WriteContainerChildElements(o);
            if ((o.@Visible) != true)
            {
                MyWriteElementStringRaw(@"Visible", o.Visible);
            }
            WriteEndElement(o);
        }
        internal protected void Write99_XTextDocumentFooterElement(string n, string ns, DCSoft.Writer.Dom.DomDocumentFooterElement o, bool isNullable, bool needType)
        {
            this._BaseWriter = base.Writer;
            var localWriter = base.Writer;
            if (o == null)
            {
                //if (isNullable) WriteNullTagLiteral(n, ns);
                return;
            }
            WriteStartElement(n, ns, o, false, null);
            if (needType) WriteXsiType(@"XTextFooter", string.Empty);
            MyWriteElementString(@"ID", (o.@ID));
            MyWriteElementString(@"ToolTip", (o.@ToolTip));
            WriteContainerChildElements(o);
            if ((o.@Visible) != true)
            {
                MyWriteElementStringRaw(@"Visible", o.Visible);
            }
            WriteEndElement(o);
        }
        internal protected void Write112_XTextTableCellElement(string n, string ns, DCSoft.Writer.Dom.DomTableCellElement o, bool isNullable, bool needType)
        {
            this._BaseWriter = base.Writer;
            var localWriter = base.Writer;
            if (o == null)
            {
                //if (isNullable) WriteNullTagLiteral(n, ns);
                return;
            }
            WriteStartElement(n, ns, o, false, null);
            if (needType) WriteXsiType(@"XTextTableCell", string.Empty);
            if ((o.@StyleIndex) != -1)
            {
                localWriter.WriteAttributeString(@"StyleIndex", null, o.StyleIndex);
            }
            if (o.IsOverrided)
            {
                localWriter.WriteAttributeString("C", "1");
            }
            MyWriteElementString(@"ID", (o.@ID));
            MyWriteElementString(@"ToolTip", (o.@ToolTip));
            Write14_ValueValidateStyle(@"ValidateStyle", string.Empty, (o.@ValidateStyle), false, false);
            WriteContainerChildElements(o);
            if ((o.@TabStop) != true)
            {
                MyWriteElementStringRaw(@"TabStop", o.TabStop);
            }
            if ((o.@RowSpan) != 1)
            {
                MyWriteElementStringRaw(@"RowSpan", o.RowSpan);
            }
            if ((o.@ColSpan) != 1)
            {
                MyWriteElementStringRaw(@"ColSpan", o.ColSpan);
            }
            WriteEndElement(o);
        }
        string Write110_MoveFocusHotKeys(DCSoft.Writer.MoveFocusHotKeys v)
        {
            string s = null;
            switch (v)
            {
                case DCSoft.Writer.MoveFocusHotKeys.@None: s = @"None"; break;
                case DCSoft.Writer.MoveFocusHotKeys.@Default: s = @"Default"; break;
                case DCSoft.Writer.MoveFocusHotKeys.@Tab: s = @"Tab"; break;
                case DCSoft.Writer.MoveFocusHotKeys.@Enter: s = @"Enter"; break;
                default: s = FromEnum(((long)v), XSerHelper_XTextDocument._Array_5, XSerHelper_XTextDocument._Array_6, @"DCSoft.Writer.MoveFocusHotKeys"); break;
            }
            return s;
        }
        internal protected void Write104_XTextTableElement(string n, string ns, DCSoft.Writer.Dom.DomTableElement o, bool isNullable, bool needType)
        {
            this._BaseWriter = base.Writer;
            var localWriter = base.Writer;
            if (o == null)
            {
                //if (isNullable) WriteNullTagLiteral(n, ns);
                return;
            }
            WriteStartElement(n, ns, o, false, null);
            if (needType) WriteXsiType(@"XTextTable", string.Empty);
            if ((o.@StyleIndex) != -1)
            {
                localWriter.WriteAttributeString(@"StyleIndex", null, o.StyleIndex);
            }
            MyWriteElementString(@"ID", (o.@ID));
            MyWriteElementString(@"ToolTip", (o.@ToolTip));
            WriteContainerChildElements(o);
            if ((o.@Visible) != true)
            {
                MyWriteElementStringRaw(@"Visible", o.Visible);
            }
            {
                DCSoft.Writer.Dom.DomElementList a = (DCSoft.Writer.Dom.DomElementList)(o.@Columns);
                if (a != null && a.Count > 0)
                {
                    var aCount = a.Count;//2222222
                    localWriter.WriteStartElement(null, @"Columns", null);
                    for (int ia = 0; ia < aCount; ia++)
                    {
                        Write3_XTextElement(@"Element", string.Empty, (a[ia]), true, false);
                    }
                    localWriter.WriteEndElement();
                }
            }
            WriteEndElement(o);
        }
        internal protected void Write107_XTextTableRowElement(string n, string ns, DCSoft.Writer.Dom.DomTableRowElement o, bool isNullable, bool needType)
        {
            this._BaseWriter = base.Writer;
            var localWriter = base.Writer;
            if (o == null)
            {
                //if (isNullable) WriteNullTagLiteral(n, ns);
                return;
            }
            WriteStartElement(n, ns, o, false, null);
            if (needType) WriteXsiType(@"XTextTableRow", string.Empty);
            if ((o.@StyleIndex) != -1)
            {
                localWriter.WriteAttributeString(@"StyleIndex", null, o.StyleIndex);
            }
            MyWriteElementString(@"ID", (o.@ID));
            MyWriteElementString(@"ToolTip", (o.@ToolTip));
            WriteContainerChildElements(o);
            if ((o.@Visible) != true)
            {
                MyWriteElementStringRaw(@"Visible", o.Visible);
            }
            if ((o.@NewPage) != false)
            {
                MyWriteElementStringRaw(@"NewPage", o.NewPage);
            }
            if ((o.@CanSplitByPageLine) != true)
            {
                MyWriteElementStringRaw(@"CanSplitByPageLine", o.CanSplitByPageLine);
            }
            if ((o.@SpecifyHeight) != 0f)
            {
                MyWriteElementStringRaw(@"SpecifyHeight", o.SpecifyHeight);
            }
            if ((o.@HeaderStyle) != false)
            {
                MyWriteElementStringRaw(@"HeaderStyle", o.HeaderStyle);
            }
            WriteEndElement(o);
        }
        internal protected void Write138_XTextInputFieldElement(string n, string ns, DCSoft.Writer.Dom.DomInputFieldElement o, bool isNullable, bool needType)
        {
            this._BaseWriter = base.Writer;
            var localWriter = base.Writer;
            if (o == null)
            {
                //if (isNullable) WriteNullTagLiteral(n, ns);
                return;
            }
            WriteStartElement(n, ns, o, false, null);
            if (needType) WriteXsiType(@"XInputField", string.Empty);
            if ((o.@StyleIndex) != -1)
            {
                localWriter.WriteAttributeString(@"StyleIndex", null, o.StyleIndex);
            }
            MyWriteElementString(@"ID", (o.@ID));
            MyWriteElementString(@"ToolTip", (o.@ToolTip));
            if ((o.@AcceptTab) != false)
            {
                MyWriteElementStringRaw(@"AcceptTab", o.AcceptTab);
            }
            Write14_ValueValidateStyle(@"ValidateStyle", string.Empty, (o.@ValidateStyle), false, false);
            WriteContainerChildElements(o);
            if ((o.@Visible) != true)
            {
                MyWriteElementStringRaw(@"Visible", o.Visible);
            }
            if ((o.@TabStop) != true)
            {
                MyWriteElementStringRaw(@"TabStop", o.TabStop);
            }
            if ((o.@MoveFocusHotKey) != (DCSoft.Writer.MoveFocusHotKeys.@Default))
            {
                WriteElementString(@"MoveFocusHotKey", string.Empty, Write110_MoveFocusHotKeys((o.@MoveFocusHotKey)));
            }
            if ((o.@UserEditable) != true)
            {
                MyWriteElementStringRaw(@"UserEditable", o.UserEditable);
            }
            MyWriteElementString(@"Name", (o.@Name));
            Write123_ValueFormater(@"DisplayFormat", string.Empty, (o.@DisplayFormat), false, false);
            MyWriteElementString(@"InnerValue", (o.@InnerValue));
            if ((o.@PrintBackgroundText) != DCSoft.Writer.DCBooleanValue.@Inherit)
            {
                localWriter.WriteElementString(@"PrintBackgroundText", null, Write18_DCBooleanValue((o.@PrintBackgroundText)));
            }
            MyWriteElementString(@"BackgroundText", (o.@BackgroundText));
            if ((o.@BorderVisible) != DCSoft.Writer.DCVisibleState.@Default)
            {
                localWriter.WriteElementString(@"BorderVisible", null, Write125_DCVisibleState((o.@BorderVisible)));
            }
            if ((o.@EnableHighlight) != DCSoft.Writer.EnableState.@Enabled)
            {
                localWriter.WriteElementString(@"EnableHighlight", null, Write126_EnableState((o.@EnableHighlight)));
            }
            if ((o.@EnableUserEditInnerValue) != true)
            {
                MyWriteElementStringRaw(@"EnableUserEditInnerValue", o.EnableUserEditInnerValue);
            }
            if ((o.@EditorActiveMode) != (DCSoft.Writer.ValueEditorActiveMode.@Program |
             DCSoft.Writer.ValueEditorActiveMode.@F2 |
             DCSoft.Writer.ValueEditorActiveMode.@MouseDblClick))
            {
                WriteElementString(@"EditorActiveMode", string.Empty, Write131_ValueEditorActiveMode((o.@EditorActiveMode)));
            }
            Write136_InputFieldSettings(@"FieldSettings", string.Empty, (o.@FieldSettings), false, false);
            WriteEndElement(o);
        }
        internal protected void Write136_InputFieldSettings(string n, string ns, DCSoft.Writer.Dom.InputFieldSettings o, bool isNullable, bool needType)
        {
            this._BaseWriter = base.Writer;
            var localWriter = base.Writer;
            if (o == null)
            {
                //if (isNullable) WriteNullTagLiteral(n, ns);
                return;
            }
            WriteStartElement(n, ns, o, false, null);
            if (needType) WriteXsiType(@"InputFieldSettings", string.Empty);
            if ((o.@EditStyle) != DCSoft.Writer.Dom.InputFieldEditStyle.@Text)
            {
                localWriter.WriteElementString(@"EditStyle", null, Write133_InputFieldEditStyle((o.@EditStyle)));
            }
            if ((o.@MultiSelect) != false)
            {
                MyWriteElementStringRaw(@"MultiSelect", o.MultiSelect);
            }
            Write134_ListSourceInfo(@"ListSource", string.Empty, (o.@ListSource), false, false);
            if ((o.@ListValueSeparatorChar) != @",")
            {
                MyWriteElementString(@"ListValueSeparatorChar", (o.@ListValueSeparatorChar));
            }
            WriteEndElement(o);
        }
        internal protected void Write134_ListSourceInfo(string n, string ns, DCSoft.Writer.Data.ListSourceInfo o, bool isNullable, bool needType)
        {
            this._BaseWriter = base.Writer;
            var localWriter = base.Writer;
            if (o == null)
            {
                //if (isNullable) WriteNullTagLiteral(n, ns);
                return;
            }
            WriteStartElement(n, ns, o, false, null);
            if (needType) WriteXsiType(@"ListSourceInfo", string.Empty);
            {
                DCSoft.Writer.Data.ListItemCollection a = (DCSoft.Writer.Data.ListItemCollection)(o.@Items);
                if (a != null && a.Count > 0)
                {
                    var aCount = a.Count;//2222222
                    localWriter.WriteStartElement(null, @"Items", null);
                    for (int ia = 0; ia < aCount; ia++)
                    {
                        Write132_ListItem(@"Item", string.Empty, (a[ia]), true, false);
                    }
                    localWriter.WriteEndElement();
                }
            }
            WriteEndElement(o);
        }
        internal protected void Write132_ListItem(string n, string ns, DCSoft.Writer.Data.ListItem o, bool isNullable, bool needType)
        {
            this._BaseWriter = base.Writer;
            //var localWriter = base.Writer;
            if (o == null)
            {
                //if (isNullable) WriteNullTagLiteral(n, ns);
                return;
            }
            WriteStartElement(n, ns, o, false, null);
            if (needType) WriteXsiType(@"ListItem", string.Empty);
            MyWriteElementString(@"TextInList", (o.@TextInList));
            MyWriteElementString(@"Text", (o.@Text));
            MyWriteElementString(@"Value", (o.@Value));
            WriteEndElement(o);
        }
        string Write133_InputFieldEditStyle(DCSoft.Writer.Dom.InputFieldEditStyle v)
        {
            string s = null;
            switch (v)
            {
                case DCSoft.Writer.Dom.InputFieldEditStyle.@Text: s = @"Text"; break;
                case DCSoft.Writer.Dom.InputFieldEditStyle.@DropdownList: s = @"DropdownList"; break;
                case DCSoft.Writer.Dom.InputFieldEditStyle.@Date: s = @"Date"; break;
                case DCSoft.Writer.Dom.InputFieldEditStyle.@DateTime: s = @"DateTime"; break;
                case DCSoft.Writer.Dom.InputFieldEditStyle.@Time: s = @"Time"; break;
                case DCSoft.Writer.Dom.InputFieldEditStyle.@Numeric: s = @"Numeric"; break;
                default: throw XSerHelper_XTextDocument.CreateInvalidEnumValueException((long)v, typeof(DCSoft.Writer.Dom.InputFieldEditStyle));
            }
            return s;
        }
        string Write131_ValueEditorActiveMode(DCSoft.Writer.ValueEditorActiveMode v)
        {
            string s = null;
            switch (v)
            {
                case DCSoft.Writer.ValueEditorActiveMode.@None: s = @"None"; break;
                case DCSoft.Writer.ValueEditorActiveMode.@Default: s = @"Default"; break;
                case DCSoft.Writer.ValueEditorActiveMode.@Program: s = @"Program"; break;
                case DCSoft.Writer.ValueEditorActiveMode.@F2: s = @"F2"; break;
                case DCSoft.Writer.ValueEditorActiveMode.@GotFocus: s = @"GotFocus"; break;
                case DCSoft.Writer.ValueEditorActiveMode.@MouseDblClick: s = @"MouseDblClick"; break;
                case DCSoft.Writer.ValueEditorActiveMode.@MouseClick: s = @"MouseClick"; break;
                case DCSoft.Writer.ValueEditorActiveMode.@MouseRightClick: s = @"MouseRightClick"; break;
                case DCSoft.Writer.ValueEditorActiveMode.@Enter: s = @"Enter"; break;
                default: s = FromEnum(((long)v), XSerHelper_XTextDocument._Array_7, XSerHelper_XTextDocument._Array_8, @"DCSoft.Writer.ValueEditorActiveMode"); break;
            }
            return s;
        }
        string Write126_EnableState(DCSoft.Writer.EnableState v)
        {
            string s = null;
            switch (v)
            {
                case DCSoft.Writer.EnableState.@Default: s = @"Default"; break;
                case DCSoft.Writer.EnableState.@Enabled: s = @"Enabled"; break;
                case DCSoft.Writer.EnableState.@Disabled: s = @"Disabled"; break;
                default: throw XSerHelper_XTextDocument.CreateInvalidEnumValueException((long)v, typeof(DCSoft.Writer.EnableState));
            }
            return s;
        }
        string Write125_DCVisibleState(DCSoft.Writer.DCVisibleState v)
        {
            string s = null;
            switch (v)
            {
                case DCSoft.Writer.DCVisibleState.@Visible: s = @"Visible"; break;
                case DCSoft.Writer.DCVisibleState.@Hidden: s = @"Hidden"; break;
                case DCSoft.Writer.DCVisibleState.@Default: s = @"Default"; break;
                case DCSoft.Writer.DCVisibleState.@AlwaysVisible: s = @"AlwaysVisible"; break;
                default: throw XSerHelper_XTextDocument.CreateInvalidEnumValueException((long)v, typeof(DCSoft.Writer.DCVisibleState));
            }
            return s;
        }
        internal protected void Write123_ValueFormater(string n, string ns, DCSoft.Data.ValueFormater o, bool isNullable, bool needType)
        {
            this._BaseWriter = base.Writer;
            var localWriter = base.Writer;
            if (o == null)
            {
                //if (isNullable) WriteNullTagLiteral(n, ns);
                return;
            }
            WriteStartElement(n, ns, o, false, null);
            if (needType) WriteXsiType(@"ValueFormater", string.Empty);
            if ((o.@Style) != DCSoft.Data.ValueFormatStyle.@None)
            {
                localWriter.WriteElementString(@"Style", null, Write122_ValueFormatStyle((o.@Style)));
            }
            MyWriteElementString(@"Format", (o.@Format));
            MyWriteElementString(@"NoneText", (o.@NoneText));
            WriteEndElement(o);
        }
        string Write122_ValueFormatStyle(DCSoft.Data.ValueFormatStyle v)
        {
            string s = null;
            switch (v)
            {
                case DCSoft.Data.ValueFormatStyle.@None: s = @"None"; break;
                case DCSoft.Data.ValueFormatStyle.@Numeric: s = @"Numeric"; break;
                case DCSoft.Data.ValueFormatStyle.@Currency: s = @"Currency"; break;
                case DCSoft.Data.ValueFormatStyle.@DateTime: s = @"DateTime"; break;
                case DCSoft.Data.ValueFormatStyle.@String: s = @"String"; break;
                case DCSoft.Data.ValueFormatStyle.@SpecifyLength: s = @"SpecifyLength"; break;
                case DCSoft.Data.ValueFormatStyle.@Boolean: s = @"Boolean"; break;
                case DCSoft.Data.ValueFormatStyle.@Percent: s = @"Percent"; break;
                default: throw XSerHelper_XTextDocument.CreateInvalidEnumValueException((long)v, typeof(DCSoft.Data.ValueFormatStyle));
            }
            return s;
        }
        string Write119_StringAlignment(StringAlignment v)
        {
            string s = null;
            switch (v)
            {
                case StringAlignment.@Near: s = @"Near"; break;
                case StringAlignment.@Center: s = @"Center"; break;
                case StringAlignment.@Far: s = @"Far"; break;
                default: throw XSerHelper_XTextDocument.CreateInvalidEnumValueException((long)v, typeof(StringAlignment));
            }
            return s;
        }
        internal protected void Write76_XTextParagraphFlagElement(string n, string ns, DCSoft.Writer.Dom.DomParagraphFlagElement o, bool isNullable, bool needType)
        {
            this._BaseWriter = base.Writer;
            var localWriter = base.Writer;
            if (o == null)
            {
                //if (isNullable) WriteNullTagLiteral(n, ns);
                return;
            }
            WriteStartElement(n, ns, o, false, null);
            if (needType) WriteXsiType(@"XParagraphFlag", string.Empty);
            if ((o.@StyleIndex) != -1)
            {
                localWriter.WriteAttributeString(@"StyleIndex", null, o.StyleIndex);
            }
            MyWriteElementString(@"ID", (o.@ID));
            if ((o.@TopicID) != -1)
            {
                MyWriteElementStringRaw(@"TopicID", o.TopicID);
            }
            if ((o.@ResetListIndexFlag) != false)
            {
                MyWriteElementStringRaw(@"ResetListIndexFlag", o.ResetListIndexFlag);
            }
            if ((o.@AutoCreate) != false)
            {
                MyWriteElementStringRaw(@"AutoCreate", o.AutoCreate);
            }
            WriteEndElement(o);
        }
        internal protected void Write94_XTextImageElement(string n, string ns, DCSoft.Writer.Dom.DomImageElement o, bool isNullable, bool needType)
        {
            this._BaseWriter = base.Writer;
            var localWriter = base.Writer;
            if (o == null)
            {
                //if (isNullable) WriteNullTagLiteral(n, ns);
                return;
            }
            WriteStartElement(n, ns, o, false, null);
            if (needType) WriteXsiType(@"XImage", string.Empty);
            if ((o.@StyleIndex) != -1)
            {
                localWriter.WriteAttributeString(@"StyleIndex", null, o.StyleIndex);
            }
            MyWriteElementString(@"ID", (o.@ID));
            MyWriteElementString(@"ToolTip", (o.@ToolTip));

            if ((o.@Visible) != true)
            {
                MyWriteElementStringRaw(@"Visible", o.Visible);
            }
            MyWriteElementString(@"Name", (o.@Name));
            if ((o.@VAlign) != DCSoft.Drawing.VerticalAlignStyle.@Bottom)
            {
                localWriter.WriteElementString(@"VAlign", null, Write40_VerticalAlignStyle((o.@VAlign)));
            }
            if ((o.@KeepWidthHeightRate) != true)
            {
                MyWriteElementStringRaw(@"KeepWidthHeightRate", o.KeepWidthHeightRate);
            }
            MyWriteElementStringRaw(@"Width", o.Width);
            MyWriteElementStringRaw(@"Height", o.Height);
            Write34_XImageValue(@"Image", string.Empty, (o.@Image), false, false);
            if ((o.@SmoothZoom) != true)
            {
                MyWriteElementStringRaw(@"SmoothZoom", o.SmoothZoom);
            }
            WriteEndElement(o);
        }
        internal protected void Write143_XTextCheckBoxElement(string n, string ns, DCSoft.Writer.Dom.DomCheckBoxElement o, bool isNullable, bool needType)
        {
            this._BaseWriter = base.Writer;
            var localWriter = base.Writer;
            if (o == null)
            {
                //if (isNullable) WriteNullTagLiteral(n, ns);
                return;
            }
            WriteStartElement(n, ns, o, false, null);
            if (needType) WriteXsiType(@"XTextCheckBox", string.Empty);
            if ((o.@StyleIndex) != -1)
            {
                localWriter.WriteAttributeString(@"StyleIndex", null, o.StyleIndex);
            }
            MyWriteElementString(@"ID", (o.@ID));
            MyWriteElementString(@"ToolTip", (o.@ToolTip));
            if ((o.@Visible) != true)
            {
                MyWriteElementStringRaw(@"Visible", o.Visible);
            }
            MyWriteElementString(@"Name", (o.@Name));
            if ((o.@Requried) != false)
            {
                MyWriteElementStringRaw(@"Requried", o.Requried);
            }
            MyWriteElementString(@"Caption", (o.@Caption));
            if ((o.@CaptionAlign) != StringAlignment.@Center)
            {
                localWriter.WriteElementString(@"CaptionAlign", null, Write119_StringAlignment((o.@CaptionAlign)));
            }
            if ((o.@AutoHeightForMultiline) != false)
            {
                MyWriteElementStringRaw(@"AutoHeightForMultiline", o.AutoHeightForMultiline);
            }
            MyWriteElementStringRaw(@"Width", o.Width);
            MyWriteElementStringRaw(@"Height", o.Height);
            if ((o.@Checked) != false)
            {
                MyWriteElementStringRaw(@"Checked", o.Checked);
            }
            MyWriteElementString(@"CheckedValue", (o.@CheckedValue));
            if ((o.@Readonly) != false)
            {
                MyWriteElementStringRaw(@"Readonly", o.Readonly);
            }
            if ((o.@Multiline) != false)
            {
                MyWriteElementStringRaw(@"Multiline", o.Multiline);
            }
            WriteEndElement(o);
        }
        internal protected void Write144_XTextRadioBoxElement(string n, string ns, DCSoft.Writer.Dom.DomRadioBoxElement o, bool isNullable, bool needType)
        {
            this._BaseWriter = base.Writer;
            var localWriter = base.Writer;
            if (o == null)
            {
                return;
            }
            WriteStartElement(n, ns, o, false, null);
            if (needType) WriteXsiType(@"XTextRadioBox", string.Empty);
            if ((o.@StyleIndex) != -1)
            {
                localWriter.WriteAttributeString(@"StyleIndex", null, o.StyleIndex);
            }
            MyWriteElementString(@"ID", (o.@ID));
            MyWriteElementString(@"ToolTip", (o.@ToolTip));

            if ((o.@Visible) != true)
            {
                MyWriteElementStringRaw(@"Visible", o.Visible);
            }
            MyWriteElementString(@"Name", (o.@Name));
            if ((o.@Requried) != false)
            {
                MyWriteElementStringRaw(@"Requried", o.Requried);
            }
            MyWriteElementString(@"Caption", (o.@Caption));
            if ((o.@CaptionAlign) != StringAlignment.@Center)
            {
                localWriter.WriteElementString(@"CaptionAlign", null, Write119_StringAlignment((o.@CaptionAlign)));
            }
            if ((o.@AutoHeightForMultiline) != false)
            {
                MyWriteElementStringRaw(@"AutoHeightForMultiline", o.AutoHeightForMultiline);
            }
            MyWriteElementStringRaw(@"Width", o.Width);
            MyWriteElementStringRaw(@"Height", o.Height);
            if ((o.@Checked) != false)
            {
                MyWriteElementStringRaw(@"Checked", o.Checked);
            }
            MyWriteElementString(@"CheckedValue", (o.@CheckedValue));
            if ((o.@Readonly) != false)
            {
                MyWriteElementStringRaw(@"Readonly", o.Readonly);
            }
            if ((o.@Multiline) != false)
            {
                MyWriteElementStringRaw(@"Multiline", o.Multiline);
            }
            WriteEndElement(o);
        }
        internal protected void Write148_XTextPageInfoElement(string n, string ns, DCSoft.Writer.Dom.DomPageInfoElement o, bool isNullable, bool needType)
        {
            this._BaseWriter = base.Writer;
            var localWriter = base.Writer;
            if (o == null)
            {
                return;
            }
            WriteStartElement(n, ns, o, false, null);
            if (needType) WriteXsiType(@"XPageInfo", string.Empty);
            if ((o.@StyleIndex) != -1)
            {
                localWriter.WriteAttributeString(@"StyleIndex", null, o.StyleIndex);
            }
            MyWriteElementString(@"ID", (o.@ID));
            MyWriteElementString(@"ToolTip", (o.@ToolTip));
            if ((o.@Visible) != true)
            {
                MyWriteElementStringRaw(@"Visible", o.Visible);
            }
            MyWriteElementString(@"Name", (o.@Name));
            MyWriteElementStringRaw(@"Width", o.Width);
            if ((o.@AutoHeight) != false)
            {
                MyWriteElementStringRaw(@"AutoHeight", o.AutoHeight);
            }
            MyWriteElementStringRaw(@"Height", o.Height);
            if ((o.@ValueType) != DCSoft.Writer.Dom.PageInfoValueType.@PageIndex)
            {
                localWriter.WriteElementString(@"ValueType", null, Write147_PageInfoValueType((o.@ValueType)));
            }
            WriteEndElement(o);
        }
        string Write147_PageInfoValueType(DCSoft.Writer.Dom.PageInfoValueType v)
        {
            string s = null;
            switch (v)
            {
                case DCSoft.Writer.Dom.PageInfoValueType.@PageIndex: s = @"PageIndex"; break;
                case DCSoft.Writer.Dom.PageInfoValueType.@NumOfPages: s = @"NumOfPages"; break;
                case DCSoft.Writer.Dom.PageInfoValueType.@LocalPageIndex: s = @"LocalPageIndex"; break;
                case DCSoft.Writer.Dom.PageInfoValueType.@LocalNumOfPages: s = @"LocalNumOfPages"; break;
                default: throw XSerHelper_XTextDocument.CreateInvalidEnumValueException((long)v, typeof(DCSoft.Writer.Dom.PageInfoValueType));
            }
            return s;
        }
        internal protected void Write95_XTextLineBreakElement(string n, string ns, DCSoft.Writer.Dom.DomLineBreakElement o, bool isNullable, bool needType)
        {
            this._BaseWriter = base.Writer;
            var localWriter = base.Writer;
            if (o == null)
            {
                return;
            }
            WriteStartElement(n, ns, o, false, null);
            if (needType) WriteXsiType(@"XLineBreak", string.Empty);
            if ((o.@StyleIndex) != -1)
            {
                localWriter.WriteAttributeString(@"StyleIndex", null, o.StyleIndex);
            }
            WriteEndElement(o);
        }

        internal protected void Write108_XTextTableColumnElement(string n, string ns, DCSoft.Writer.Dom.DomTableColumnElement o, bool isNullable, bool needType)
        {
            this._BaseWriter = base.Writer;
            var localWriter = base.Writer;
            if (o == null)
            {
                //if (isNullable) WriteNullTagLiteral(n, ns);
                return;
            }
            WriteStartElement(n, ns, o, false, null);
            if (needType) WriteXsiType(@"XTextTableColumn", string.Empty);
            if ((o.@StyleIndex) != -1)
            {
                localWriter.WriteAttributeString(@"StyleIndex", null, o.StyleIndex);
            }
            MyWriteElementString(@"ID", (o.@ID));
            MyWriteElementStringRaw(@"Width", o.Width);
            WriteEndElement(o);
        }
        internal protected void Write113_XTextPageBreakElement(string n, string ns, DCSoft.Writer.Dom.DomPageBreakElement o, bool isNullable, bool needType)
        {
            this._BaseWriter = base.Writer;
            var localWriter = base.Writer;
            if (o == null)
            {
                return;
            }
            WriteStartElement(n, ns, o, false, null);
            if (needType) WriteXsiType(@"XPageBreak", string.Empty);
            if ((o.@StyleIndex) != -1)
            {
                localWriter.WriteAttributeString(@"StyleIndex", null, o.StyleIndex);
            }
            WriteEndElement(o);
        }
    }
}
