// DCSoft.WASMTest.JsonConvertCodeGen.BuildSource() 2023-12-26 14:53:58
using System;
using System.Text.Json;
namespace DCSoft.WASM
{
    internal static class DCJsonSerialization
    {
        public static void AddConverter(System.Text.Json.JsonSerializerOptions options)
        {
            options.Converters.Add(new JsonConverter_DashStyle());
            options.Converters.Add(new JsonConverter_DCValidateIDRepeatMode());
            options.Converters.Add(new JsonConverter_DocumentBehaviorOptions());
            options.Converters.Add(new JsonConverter_DocumentEditOptions());
            options.Converters.Add(new JsonConverter_DocumentOptions());
            options.Converters.Add(new JsonConverter_DocumentValueValidateMode());
            options.Converters.Add(new JsonConverter_DocumentViewOptions());
            options.Converters.Add(new JsonConverter_EnableState());
            options.Converters.Add(new JsonConverter_GraphicsUnit());
            options.Converters.Add(new JsonConverter_MoveFocusHotKeys());
            options.Converters.Add(new JsonConverter_PromptProtectedContentMode());
            options.Converters.Add(new JsonConverter_ValueEditorActiveMode());
            options.Converters.Add(new JsonConverter_ValueFormater());
            options.Converters.Add(new JsonConverter_ValueFormatStyle());
            options.Converters.Add(new JsonConverter_ValueTypeStyle());
            options.Converters.Add(new JsonConverter_ValueValidateLevel());
            options.Converters.Add(new JsonConverter_ValueValidateStyle());
            options.Converters.Add(new JsonConverter_WriterDataFormats());
            options.Converters.Add(new JsonConverter_XFontValue());
            options.Converters.Add(new JsonConverter_XImageValue());
        }
        private static readonly string __AcceptDataFormats = "AcceptDataFormats";
        private static readonly string __ActiveCheckInstallWindowsMediaPlayer = "ActiveCheckInstallWindowsMediaPlayer";
        private static readonly string __AdornTextBackColor = "AdornTextBackColor";
        private static readonly string __AdornTextVisibility = "AdornTextVisibility";
        private static readonly string __AllowDeleteJumpOutOfField = "AllowDeleteJumpOutOfField";
        private static readonly string __AllowDragContent = "AllowDragContent";
        private static readonly string __Alpha = "Alpha";
        private static readonly string __Angle = "Angle";
        private static readonly string __AutoClearInvalidateCharacter = "AutoClearInvalidateCharacter";
        private static readonly string __AutoClearTextFormatWhenPasteOrInsertContent = "AutoClearTextFormatWhenPasteOrInsertContent";
        private static readonly string __AutoFixElementIDWhenInsertElements = "AutoFixElementIDWhenInsertElements";
        private static readonly string __AutoFocusWhenOpenDocument = "AutoFocusWhenOpenDocument";
        private static readonly string __AutoSaveIntervalInSecond = "AutoSaveIntervalInSecond";
        private static readonly string __AutoSaveScriptAssemblyToTempFile = "AutoSaveScriptAssemblyToTempFile";
        private static readonly string __AutoShowScriptErrorMessage = "AutoShowScriptErrorMessage";
        private static readonly string __AutoUpdateButtonVisible = "AutoUpdateButtonVisible";
        private static readonly string __BackColorValue = "BackColorValue";
        private static readonly string __BackgroundColor = "BackgroundColor";
        private static readonly string __BackgroundTextColor = "BackgroundTextColor";
        private static readonly string __BehaviorOptions = "BehaviorOptions";
        private static readonly string __BinaryLength = "BinaryLength";
        private static readonly string __Bold = "Bold";
        private static readonly string __CheckDecimalDigits = "CheckDecimalDigits";
        private static readonly string __CheckedValueBindingHandledForTableRow = "CheckedValueBindingHandledForTableRow";
        private static readonly string __CheckMaxValue = "CheckMaxValue";
        private static readonly string __CheckMinValue = "CheckMinValue";
        private static readonly string __ClearFieldValueWhenCopy = "ClearFieldValueWhenCopy";
        private static readonly string __CloneSerialize = "CloneSerialize";
        private static readonly string __CloneWithoutElementBorderStyle = "CloneWithoutElementBorderStyle";
        private static readonly string __ColorValue = "ColorValue";
        private static readonly string __CompressLayoutForFieldBorder = "CompressLayoutForFieldBorder";
        private static readonly string __CompressXMLContent = "CompressXMLContent";
        private static readonly string __ContinueHeaderParagrahStyle = "ContinueHeaderParagrahStyle";
        private static readonly string __CopyInTextFormatOnly = "CopyInTextFormatOnly";
        private static readonly string __CopyWithoutInputFieldStructure = "CopyWithoutInputFieldStructure";
        private static readonly string __CreationDataFormats = "CreationDataFormats";
        private static readonly string __DateTimeMaxValue = "DateTimeMaxValue";
        private static readonly string __DateTimeMinValue = "DateTimeMinValue";
        private static readonly string __DebugMode = "DebugMode";
        private static readonly string __DefaultAdornTextType = "DefaultAdornTextType";
        private static readonly string __DefaultEditorActiveMode = "DefaultEditorActiveMode";
        private static readonly string __DefaultInputFieldHighlight = "DefaultInputFieldHighlight";
        private static readonly string __DefaultLineColorForImageEditor = "DefaultLineColorForImageEditor";
        private static readonly string __DensityForRepeat = "DensityForRepeat";
        private static readonly string __DisplayFormatToInnerValue = "DisplayFormatToInnerValue";
        private static readonly string __DoubleClickToSelectWord = "DoubleClickToSelectWord";
        private static readonly string __EditOptions = "EditOptions";
        private static readonly string __EnableAIForSuspiciousContent = "EnableAIForSuspiciousContent";
        private static readonly string __EnableCalculateControl = "EnableCalculateControl";
        private static readonly string __EnableCheckControlLoaded = "EnableCheckControlLoaded";
        private static readonly string __EnableChineseFontSizeName = "EnableChineseFontSizeName";
        private static readonly string __EnableContentChangedEventWhenLoadDocument = "EnableContentChangedEventWhenLoadDocument";
        private static readonly string __EnableContentLock = "EnableContentLock";
        private static readonly string __EnableControlHostAtDesignTime = "EnableControlHostAtDesignTime";
        private static readonly string __EnableCopySource = "EnableCopySource";
        private static readonly string __Enabled = "Enabled";
        private static readonly string __EnableDataBinding = "EnableDataBinding";
        private static readonly string __EnabledElementEvent = "EnabledElementEvent";
        private static readonly string __EnableDeleteReadonlyEmptyContainer = "EnableDeleteReadonlyEmptyContainer";
        private static readonly string __EnabledShowWinTaskBarProgress = "EnabledShowWinTaskBarProgress";
        private static readonly string __EnableEditElementValue = "EnableEditElementValue";
        private static readonly string __EnableElementEvents = "EnableElementEvents";
        private static readonly string __EnableExpression = "EnableExpression";
        private static readonly string __EnableKBEntryRangeMask = "EnableKBEntryRangeMask";
        private static readonly string __EnableLogUndo = "EnableLogUndo";
        private static readonly string __EnableScript = "EnableScript";
        private static readonly string __ExtendingPrintDialog = "ExtendingPrintDialog";
        private static readonly string __FieldBackColor = "FieldBackColor";
        private static readonly string __FieldBorderColor = "FieldBorderColor";
        private static readonly string __FieldBorderElementPixelWidth = "FieldBorderElementPixelWidth";
        private static readonly string __FieldFocusedBackColor = "FieldFocusedBackColor";
        private static readonly string __FieldHoverBackColor = "FieldHoverBackColor";
        private static readonly string __FieldInvalidateValueBackColor = "FieldInvalidateValueBackColor";
        private static readonly string __FieldInvalidateValueForeColor = "FieldInvalidateValueForeColor";
        private static readonly string __FieldTextPrintColor = "FieldTextPrintColor";
        private static readonly string __FixSizeWhenInsertImage = "FixSizeWhenInsertImage";
        private static readonly string __FixWidthWhenInsertTable = "FixWidthWhenInsertTable";
        private static readonly string __Font = "Font";
        private static readonly string __ForceCollateWhenPrint = "ForceCollateWhenPrint";
        private static readonly string __ForcePopupFormTopMost = "ForcePopupFormTopMost";
        private static readonly string __ForceRaiseEventAfterFieldContentEdit = "ForceRaiseEventAfterFieldContentEdit";
        private static readonly string __Format = "Format";
        private static readonly string __GlobalSpecifyDebugModeValue = "GlobalSpecifyDebugModeValue";
        private static readonly string __HandleCommandException = "HandleCommandException";
        private static readonly string __HeaderBottomLineWidth = "HeaderBottomLineWidth";
        private static readonly string __HiddenFieldBorderWhenLostFocus = "HiddenFieldBorderWhenLostFocus";
        private static readonly string __IgnoreDataBindingWhenMissValue = "IgnoreDataBindingWhenMissValue";
        private static readonly string __IgnoreTopBottomPaddingWhenGridLineLayout = "IgnoreTopBottomPaddingWhenGridLineLayout";
        private static readonly string __Image = "Image";
        private static readonly string __ImageCompressSaveMode = "ImageCompressSaveMode";
        private static readonly string __ImageDataBase64String = "ImageDataBase64String";
        private static readonly string __ImageShapeEditorBackgroundMenuItemConfig = "ImageShapeEditorBackgroundMenuItemConfig";
        private static readonly string __Italic = "Italic";
        private static readonly string __KeepTableWidthWhenInsertDeleteColumn = "KeepTableWidthWhenInsertDeleteColumn";
        private static readonly string __LayoutDirection = "LayoutDirection";
        private static readonly string __Level = "Level";
        private static readonly string __MaxDecimalDigits = "MaxDecimalDigits";
        private static readonly string __MaximizedPrintPreviewDialog = "MaximizedPrintPreviewDialog";
        private static readonly string __MaxLength = "MaxLength";
        private static readonly string __MaxValue = "MaxValue";
        private static readonly string __MaxZoomRate = "MaxZoomRate";
        private static readonly string __MinCountForDropdownListSpellCodeArea = "MinCountForDropdownListSpellCodeArea";
        private static readonly string __MinImageFileSizeForConfirmCompressSaveMode = "MinImageFileSizeForConfirmCompressSaveMode";
        private static readonly string __MinLength = "MinLength";
        private static readonly string __MinTableColumnWidth = "MinTableColumnWidth";
        private static readonly string __MinValue = "MinValue";
        private static readonly string __MinZoomRate = "MinZoomRate";
        private static readonly string __MoveCaretWhenDeleteFail = "MoveCaretWhenDeleteFail";
        private static readonly string __MoveFieldWhenDragWholeContent = "MoveFieldWhenDragWholeContent";
        private static readonly string __MoveFocusHotKey = "MoveFocusHotKey";
        private static readonly string __Name = "Name";
        private static readonly string __NewExpressionExecuteMode = "NewExpressionExecuteMode";
        private static readonly string __NoneBorderColor = "NoneBorderColor";
        private static readonly string __NoneText = "NoneText";
        private static readonly string __NormalFieldBorderColor = "NormalFieldBorderColor";
        private static readonly string __NotificationWorkingTimeout = "NotificationWorkingTimeout";
        private static readonly string __OutputFormatedXMLSource = "OutputFormatedXMLSource";
        private static readonly string __PageLineUnderPageBreak = "PageLineUnderPageBreak";
        private static readonly string __PageMarginLineLength = "PageMarginLineLength";
        private static readonly string __ParagraphFlagFollowTableOrSection = "ParagraphFlagFollowTableOrSection";
        private static readonly string __ParseCrLfAsLineBreakElement = "ParseCrLfAsLineBreakElement";
        private static readonly string __PreserveBackgroundTextWhenPrint = "PreserveBackgroundTextWhenPrint";
        private static readonly string __Printable = "Printable";
        private static readonly string __PrintBackgroundText = "PrintBackgroundText";
        private static readonly string __PrintWatermark = "PrintWatermark";
        private static readonly string __PromptForExcludeSystemClipboardRange = "PromptForExcludeSystemClipboardRange";
        private static readonly string __PromptForRejectFormat = "PromptForRejectFormat";
        private static readonly string __PromptJumpBackForSearch = "PromptJumpBackForSearch";
        private static readonly string __PromptProtectedContent = "PromptProtectedContent";
        private static readonly string __Range = "Range";
        private static readonly string __ReadonlyFieldBorderColor = "ReadonlyFieldBorderColor";
        private static readonly string __RemoveLastParagraphFlagWhenInsertDocument = "RemoveLastParagraphFlagWhenInsertDocument";
        private static readonly string __Repeat = "Repeat";
        private static readonly string __Required = "Required";
        private static readonly string __ResetTextFormatWhenCreateNewLine = "ResetTextFormatWhenCreateNewLine";
        private static readonly string __SaveBodyTextToXml = "SaveBodyTextToXml";
        private static readonly string __SelectionHightlightMaskColor = "SelectionHightlightMaskColor";
        private static readonly string __SelectionTextIncludeBackgroundText = "SelectionTextIncludeBackgroundText";
        private static readonly string __SetParagraphFlagHeightUsePreElement = "SetParagraphFlagHeightUsePreElement";
        private static readonly string __ShowCellNoneBorder = "ShowCellNoneBorder";
        private static readonly string __ShowDebugMessage = "ShowDebugMessage";
        private static readonly string __ShowGlobalGridLineInTableAndSection = "ShowGlobalGridLineInTableAndSection";
        private static readonly string __ShowHeaderBottomLine = "ShowHeaderBottomLine";
        private static readonly string __ShowPageBreak = "ShowPageBreak";
        private static readonly string __ShowPageLine = "ShowPageLine";
        private static readonly string __ShowParagraphFlag = "ShowParagraphFlag";
        private static readonly string __ShowRedErrorMessageForPaint = "ShowRedErrorMessageForPaint";
        private static readonly string __ShowTooltip = "ShowTooltip";
        private static readonly string __ShowWatermark = "ShowWatermark";
        private static readonly string __SimpleElementProperties = "SimpleElementProperties";
        private static readonly string __Size = "Size";
        private static readonly string __SpecifyDebugMode = "SpecifyDebugMode";
        private static readonly string __SpeedupByMultiThread = "SpeedupByMultiThread";
        private static readonly string __StdLablesForImageEditor = "StdLablesForImageEditor";
        private static readonly string __Strikeout = "Strikeout";
        private static readonly string __Style = "Style";
        private static readonly string __SupportUG = "SupportUG";
        private static readonly string __TabKeyToFirstLineIndent = "TabKeyToFirstLineIndent";
        private static readonly string __TabKeyToInsertTableRow = "TabKeyToInsertTableRow";
        private static readonly string __TableCellOperationDetectDistance = "TableCellOperationDetectDistance";
        private static readonly string __Text = "Text";
        private static readonly string __ThreeClickToSelectParagraph = "ThreeClickToSelectParagraph";
        private static readonly string __Type = "Type";
        private static readonly string __Underline = "Underline";
        private static readonly string __UnderLineColor = "UnderLineColor";
        private static readonly string __UnderLineColorNum = "UnderLineColorNum";
        private static readonly string __UnEditableFieldBorderColor = "UnEditableFieldBorderColor";
        private static readonly string __Unit = "Unit";
        private static readonly string __UseNewValueExpressionEngine = "UseNewValueExpressionEngine";
        private static readonly string __ValidateIDRepeatMode = "ValidateIDRepeatMode";
        private static readonly string __ValueName = "ValueName";
        private static readonly string __ValueType = "ValueType";
        private static readonly string __ValueValidateMode = "ValueValidateMode";
        private static readonly string __ViewOptions = "ViewOptions";
        private static readonly string __WeakMode = "WeakMode";
        private static readonly string __WidelyRaiseFocusEvent = "WidelyRaiseFocusEvent";
        private readonly static System.Collections.Generic.Dictionary<string, string> myValueReferences = BuildValueReferences();
        private static System.Collections.Generic.Dictionary<string, string> BuildValueReferences()
        {
            var table = new System.Collections.Generic.Dictionary<string, string>();
            table.Add(__AcceptDataFormats, __AcceptDataFormats);
            table.Add(__ActiveCheckInstallWindowsMediaPlayer, __ActiveCheckInstallWindowsMediaPlayer);
            table.Add(__AdornTextBackColor, __AdornTextBackColor);
            table.Add(__AdornTextVisibility, __AdornTextVisibility);
            table.Add(__AllowDeleteJumpOutOfField, __AllowDeleteJumpOutOfField);
            table.Add(__AllowDragContent, __AllowDragContent);
            table.Add(__Alpha, __Alpha);
            table.Add(__Angle, __Angle);
            table.Add(__AutoClearInvalidateCharacter, __AutoClearInvalidateCharacter);
            table.Add(__AutoClearTextFormatWhenPasteOrInsertContent, __AutoClearTextFormatWhenPasteOrInsertContent);
            table.Add(__AutoFixElementIDWhenInsertElements, __AutoFixElementIDWhenInsertElements);
            table.Add(__AutoFocusWhenOpenDocument, __AutoFocusWhenOpenDocument);
            table.Add(__AutoSaveIntervalInSecond, __AutoSaveIntervalInSecond);
            table.Add(__AutoSaveScriptAssemblyToTempFile, __AutoSaveScriptAssemblyToTempFile);
            table.Add(__AutoShowScriptErrorMessage, __AutoShowScriptErrorMessage);
            table.Add(__AutoUpdateButtonVisible, __AutoUpdateButtonVisible);
            table.Add(__BackColorValue, __BackColorValue);
            table.Add(__BackgroundColor, __BackgroundColor);
            table.Add(__BackgroundTextColor, __BackgroundTextColor);
            table.Add(__BehaviorOptions, __BehaviorOptions);
            table.Add(__BinaryLength, __BinaryLength);
            table.Add(__Bold, __Bold);
            table.Add(__CheckDecimalDigits, __CheckDecimalDigits);
            table.Add(__CheckedValueBindingHandledForTableRow, __CheckedValueBindingHandledForTableRow);
            table.Add(__CheckMaxValue, __CheckMaxValue);
            table.Add(__CheckMinValue, __CheckMinValue);
            table.Add(__ClearFieldValueWhenCopy, __ClearFieldValueWhenCopy);
            table.Add(__CloneSerialize, __CloneSerialize);
            table.Add(__CloneWithoutElementBorderStyle, __CloneWithoutElementBorderStyle);
            table.Add(__ColorValue, __ColorValue);
            table.Add(__CompressLayoutForFieldBorder, __CompressLayoutForFieldBorder);
            table.Add(__CompressXMLContent, __CompressXMLContent);
            table.Add(__ContinueHeaderParagrahStyle, __ContinueHeaderParagrahStyle);
            table.Add(__CopyInTextFormatOnly, __CopyInTextFormatOnly);
            table.Add(__CopyWithoutInputFieldStructure, __CopyWithoutInputFieldStructure);
            table.Add(__CreationDataFormats, __CreationDataFormats);
            table.Add(__DateTimeMaxValue, __DateTimeMaxValue);
            table.Add(__DateTimeMinValue, __DateTimeMinValue);
            table.Add(__DebugMode, __DebugMode);
            table.Add(__DefaultAdornTextType, __DefaultAdornTextType);
            table.Add(__DefaultEditorActiveMode, __DefaultEditorActiveMode);
            table.Add(__DefaultInputFieldHighlight, __DefaultInputFieldHighlight);
            table.Add(__DefaultLineColorForImageEditor, __DefaultLineColorForImageEditor);
             table.Add(__DensityForRepeat, __DensityForRepeat);
            table.Add(__DisplayFormatToInnerValue, __DisplayFormatToInnerValue);
            table.Add(__DoubleClickToSelectWord, __DoubleClickToSelectWord);
            table.Add(__EditOptions, __EditOptions);
            table.Add(__EnableAIForSuspiciousContent, __EnableAIForSuspiciousContent);
            table.Add(__EnableCalculateControl, __EnableCalculateControl);
            table.Add(__EnableCheckControlLoaded, __EnableCheckControlLoaded);
            table.Add(__EnableChineseFontSizeName, __EnableChineseFontSizeName);
            table.Add(__EnableContentChangedEventWhenLoadDocument, __EnableContentChangedEventWhenLoadDocument);
            table.Add(__EnableContentLock, __EnableContentLock);
            table.Add(__EnableControlHostAtDesignTime, __EnableControlHostAtDesignTime);
            table.Add(__EnableCopySource, __EnableCopySource);
            table.Add(__Enabled, __Enabled);
            table.Add(__EnableDataBinding, __EnableDataBinding);
            table.Add(__EnabledElementEvent, __EnabledElementEvent);
            table.Add(__EnableDeleteReadonlyEmptyContainer, __EnableDeleteReadonlyEmptyContainer);
            table.Add(__EnabledShowWinTaskBarProgress, __EnabledShowWinTaskBarProgress);
            table.Add(__EnableEditElementValue, __EnableEditElementValue);
            table.Add(__EnableElementEvents, __EnableElementEvents);
            table.Add(__EnableExpression, __EnableExpression);
            table.Add(__EnableKBEntryRangeMask, __EnableKBEntryRangeMask);
            table.Add(__EnableLogUndo, __EnableLogUndo);
            table.Add(__EnableScript, __EnableScript);
            table.Add(__ExtendingPrintDialog, __ExtendingPrintDialog);
            table.Add(__FieldBackColor, __FieldBackColor);
            table.Add(__FieldBorderColor, __FieldBorderColor);
            table.Add(__FieldBorderElementPixelWidth, __FieldBorderElementPixelWidth);
            table.Add(__FieldFocusedBackColor, __FieldFocusedBackColor);
            table.Add(__FieldHoverBackColor, __FieldHoverBackColor);
            table.Add(__FieldInvalidateValueBackColor, __FieldInvalidateValueBackColor);
            table.Add(__FieldInvalidateValueForeColor, __FieldInvalidateValueForeColor);
            table.Add(__FieldTextPrintColor, __FieldTextPrintColor);
            table.Add(__FixSizeWhenInsertImage, __FixSizeWhenInsertImage);
            table.Add(__FixWidthWhenInsertTable, __FixWidthWhenInsertTable);
            table.Add(__Font, __Font);
            table.Add(__ForceCollateWhenPrint, __ForceCollateWhenPrint);
            table.Add(__ForcePopupFormTopMost, __ForcePopupFormTopMost);
            table.Add(__ForceRaiseEventAfterFieldContentEdit, __ForceRaiseEventAfterFieldContentEdit);
            table.Add(__Format, __Format);
            table.Add(__GlobalSpecifyDebugModeValue, __GlobalSpecifyDebugModeValue);
            table.Add(__HandleCommandException, __HandleCommandException);
            table.Add(__HeaderBottomLineWidth, __HeaderBottomLineWidth);
            table.Add(__HiddenFieldBorderWhenLostFocus, __HiddenFieldBorderWhenLostFocus);
            table.Add(__IgnoreDataBindingWhenMissValue, __IgnoreDataBindingWhenMissValue);
            table.Add(__IgnoreTopBottomPaddingWhenGridLineLayout, __IgnoreTopBottomPaddingWhenGridLineLayout);
            table.Add(__Image, __Image);
            table.Add(__ImageCompressSaveMode, __ImageCompressSaveMode);
            table.Add(__ImageDataBase64String, __ImageDataBase64String);
            table.Add(__ImageShapeEditorBackgroundMenuItemConfig, __ImageShapeEditorBackgroundMenuItemConfig);
            table.Add(__Italic, __Italic);
            table.Add(__KeepTableWidthWhenInsertDeleteColumn, __KeepTableWidthWhenInsertDeleteColumn);
            table.Add(__LayoutDirection, __LayoutDirection);
            table.Add(__Level, __Level);
            table.Add(__MaxDecimalDigits, __MaxDecimalDigits);
            table.Add(__MaximizedPrintPreviewDialog, __MaximizedPrintPreviewDialog);
            table.Add(__MaxLength, __MaxLength);
            table.Add(__MaxValue, __MaxValue);
            table.Add(__MaxZoomRate, __MaxZoomRate);
            table.Add(__MinCountForDropdownListSpellCodeArea, __MinCountForDropdownListSpellCodeArea);
            table.Add(__MinImageFileSizeForConfirmCompressSaveMode, __MinImageFileSizeForConfirmCompressSaveMode);
            table.Add(__MinLength, __MinLength);
            table.Add(__MinTableColumnWidth, __MinTableColumnWidth);
            table.Add(__MinValue, __MinValue);
            table.Add(__MinZoomRate, __MinZoomRate);
            table.Add(__MoveCaretWhenDeleteFail, __MoveCaretWhenDeleteFail);
            table.Add(__MoveFieldWhenDragWholeContent, __MoveFieldWhenDragWholeContent);
            table.Add(__MoveFocusHotKey, __MoveFocusHotKey);
            table.Add(__Name, __Name);
            table.Add(__NewExpressionExecuteMode, __NewExpressionExecuteMode);
            table.Add(__NoneBorderColor, __NoneBorderColor);
            table.Add(__NoneText, __NoneText);
            table.Add(__NormalFieldBorderColor, __NormalFieldBorderColor);
            table.Add(__NotificationWorkingTimeout, __NotificationWorkingTimeout);
            table.Add(__OutputFormatedXMLSource, __OutputFormatedXMLSource);
            table.Add(__PageLineUnderPageBreak, __PageLineUnderPageBreak);
            table.Add(__PageMarginLineLength, __PageMarginLineLength);
            table.Add(__ParagraphFlagFollowTableOrSection, __ParagraphFlagFollowTableOrSection);
            table.Add(__ParseCrLfAsLineBreakElement, __ParseCrLfAsLineBreakElement);
            table.Add(__PreserveBackgroundTextWhenPrint, __PreserveBackgroundTextWhenPrint);
            table.Add(__Printable, __Printable);
            table.Add(__PrintBackgroundText, __PrintBackgroundText);
            table.Add(__PrintWatermark, __PrintWatermark);
            table.Add(__PromptForExcludeSystemClipboardRange, __PromptForExcludeSystemClipboardRange);
            table.Add(__PromptForRejectFormat, __PromptForRejectFormat);
            table.Add(__PromptJumpBackForSearch, __PromptJumpBackForSearch);
            table.Add(__PromptProtectedContent, __PromptProtectedContent);
            table.Add(__Range, __Range);
            table.Add(__ReadonlyFieldBorderColor, __ReadonlyFieldBorderColor);
            table.Add(__RemoveLastParagraphFlagWhenInsertDocument, __RemoveLastParagraphFlagWhenInsertDocument);
            table.Add(__Repeat, __Repeat);
            table.Add(__Required, __Required);
            table.Add(__ResetTextFormatWhenCreateNewLine, __ResetTextFormatWhenCreateNewLine);
            table.Add(__SaveBodyTextToXml, __SaveBodyTextToXml);
            table.Add(__SelectionHightlightMaskColor, __SelectionHightlightMaskColor);
            table.Add(__SelectionTextIncludeBackgroundText, __SelectionTextIncludeBackgroundText);
            table.Add(__SetParagraphFlagHeightUsePreElement, __SetParagraphFlagHeightUsePreElement);
            table.Add(__ShowCellNoneBorder, __ShowCellNoneBorder);
            table.Add(__ShowDebugMessage, __ShowDebugMessage);
            table.Add(__ShowGlobalGridLineInTableAndSection, __ShowGlobalGridLineInTableAndSection);
            table.Add(__ShowHeaderBottomLine, __ShowHeaderBottomLine);
            table.Add(__ShowPageBreak, __ShowPageBreak);
            table.Add(__ShowPageLine, __ShowPageLine);
            table.Add(__ShowParagraphFlag, __ShowParagraphFlag);
            table.Add(__ShowRedErrorMessageForPaint, __ShowRedErrorMessageForPaint);
            table.Add(__ShowTooltip, __ShowTooltip);
            table.Add(__ShowWatermark, __ShowWatermark);
            table.Add(__SimpleElementProperties, __SimpleElementProperties);
            table.Add(__Size, __Size);
            table.Add(__SpecifyDebugMode, __SpecifyDebugMode);
            table.Add(__SpeedupByMultiThread, __SpeedupByMultiThread);
            table.Add(__StdLablesForImageEditor, __StdLablesForImageEditor);
            table.Add(__Strikeout, __Strikeout);
            table.Add(__Style, __Style);
            table.Add(__SupportUG, __SupportUG);
            table.Add(__TabKeyToFirstLineIndent, __TabKeyToFirstLineIndent);
            table.Add(__TabKeyToInsertTableRow, __TabKeyToInsertTableRow);
            table.Add(__TableCellOperationDetectDistance, __TableCellOperationDetectDistance);
            table.Add(__Text, __Text);
            table.Add(__ThreeClickToSelectParagraph, __ThreeClickToSelectParagraph);
            table.Add(__Type, __Type);
            table.Add(__Underline, __Underline);
            table.Add(__UnderLineColor, __UnderLineColor);
            table.Add(__UnderLineColorNum, __UnderLineColorNum);
            table.Add(__UnEditableFieldBorderColor, __UnEditableFieldBorderColor);
            table.Add(__Unit, __Unit);
            table.Add(__UseNewValueExpressionEngine, __UseNewValueExpressionEngine);
            table.Add(__ValidateIDRepeatMode, __ValidateIDRepeatMode);
            table.Add(__ValueName, __ValueName);
            table.Add(__ValueType, __ValueType);
            table.Add(__ValueValidateMode, __ValueValidateMode);
            table.Add(__ViewOptions, __ViewOptions);
            table.Add(__WeakMode, __WeakMode);
            table.Add(__WidelyRaiseFocusEvent, __WidelyRaiseFocusEvent);
            return table;
        }

        private static object GetValueReference(string name)
        {
            if (name != null && name.Length > 0)
            {
                string result = null;
                if (myValueReferences.TryGetValue(name, out result))
                {
                    return result;
                }
            }
            return null;
        }
        private static void NotSupportProperty(string typeName, string propertyName)
        {
            //Console.WriteLine("不支持的JSON属性:" + typeName + "." + propertyName);
        }

        public sealed class JsonConverter_DashStyle : System.Text.Json.Serialization.JsonConverter<DCSystem_Drawing.Drawing2D.DashStyle>
        {
            public override bool CanConvert(Type typeToConvert) { return typeToConvert == typeof(DCSystem_Drawing.Drawing2D.DashStyle); }
            public override DCSystem_Drawing.Drawing2D.DashStyle Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                return ReadEnumDashStyle(ref reader, default(DCSystem_Drawing.Drawing2D.DashStyle));
            }
            public override void Write(Utf8JsonWriter writer, DCSystem_Drawing.Drawing2D.DashStyle Value, JsonSerializerOptions options)
            {
                WriteEnumDashStyle(writer, Value);
            }
        }
        private static readonly string[] _Names_DashStyle = new string[]{
           "Solid", // 0
           "Dash", // 1
           "Dot", // 2
           "DashDot", // 3
           "DashDotDot", // 4
           "Custom"}; // 5
        public static DCSystem_Drawing.Drawing2D.DashStyle ReadEnumDashStyle(ref Utf8JsonReader reader, DCSystem_Drawing.Drawing2D.DashStyle defaultValue)
        {
            if (reader.TokenType == JsonTokenType.String)
            {
                var strValue = reader.GetString();
                for (var iCount = _Names_DashStyle.Length - 1; iCount >= 0; iCount--)
                {
                    if (strValue.Equals(_Names_DashStyle[iCount], StringComparison.OrdinalIgnoreCase)) return (DCSystem_Drawing.Drawing2D.DashStyle)iCount;
                }
            }
            else if (reader.TokenType == JsonTokenType.Number) { return (DCSystem_Drawing.Drawing2D.DashStyle)reader.GetInt32(); }
            return defaultValue;
        }
        public static void WriteEnumDashStyle(Utf8JsonWriter writer, DCSystem_Drawing.Drawing2D.DashStyle vValue)
        {
            var index = (int)vValue;
            if (index >= 0 && index < _Names_DashStyle.Length) writer.WriteStringValue(_Names_DashStyle[index]);
            else writer.WriteNumberValue((int)vValue);
        }

        public sealed class JsonConverter_DCValidateIDRepeatMode : System.Text.Json.Serialization.JsonConverter<DCSoft.Writer.DCValidateIDRepeatMode>
        {
            public override bool CanConvert(Type typeToConvert) { return typeToConvert == typeof(DCSoft.Writer.DCValidateIDRepeatMode); }
            public override DCSoft.Writer.DCValidateIDRepeatMode Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                return ReadEnumDCValidateIDRepeatMode(ref reader, default(DCSoft.Writer.DCValidateIDRepeatMode));
            }
            public override void Write(Utf8JsonWriter writer, DCSoft.Writer.DCValidateIDRepeatMode Value, JsonSerializerOptions options)
            {
                WriteEnumDCValidateIDRepeatMode(writer, Value);
            }
        }
        private static readonly string[] _Names_DCValidateIDRepeatMode = new string[]{
           "None", // 0
           "DetectOnly", // 1
           "AutoFix", // 2
           "ThrowException"}; // 3
        public static DCSoft.Writer.DCValidateIDRepeatMode ReadEnumDCValidateIDRepeatMode(ref Utf8JsonReader reader, DCSoft.Writer.DCValidateIDRepeatMode defaultValue)
        {
            if (reader.TokenType == JsonTokenType.String)
            {
                var strValue = reader.GetString();
                for (var iCount = _Names_DCValidateIDRepeatMode.Length - 1; iCount >= 0; iCount--)
                {
                    if (strValue.Equals(_Names_DCValidateIDRepeatMode[iCount], StringComparison.OrdinalIgnoreCase)) return (DCSoft.Writer.DCValidateIDRepeatMode)iCount;
                }
            }
            else if (reader.TokenType == JsonTokenType.Number) { return (DCSoft.Writer.DCValidateIDRepeatMode)reader.GetInt32(); }
            return defaultValue;
        }
        public static void WriteEnumDCValidateIDRepeatMode(Utf8JsonWriter writer, DCSoft.Writer.DCValidateIDRepeatMode vValue)
        {
            var index = (int)vValue;
            if (index >= 0 && index < _Names_DCValidateIDRepeatMode.Length) writer.WriteStringValue(_Names_DCValidateIDRepeatMode[index]);
            else writer.WriteNumberValue((int)vValue);
        }

        public sealed class JsonConverter_DocumentBehaviorOptions : System.Text.Json.Serialization.JsonConverter<DCSoft.Writer.DocumentBehaviorOptions>
        {
            public override bool CanConvert(Type typeToConvert) { return typeToConvert == typeof(DCSoft.Writer.DocumentBehaviorOptions); }
            public override DCSoft.Writer.DocumentBehaviorOptions Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                return ReadDocumentBehaviorOptions(ref reader, new DCSoft.Writer.DocumentBehaviorOptions());
            }
            public override void Write(Utf8JsonWriter writer, DCSoft.Writer.DocumentBehaviorOptions value, JsonSerializerOptions options)
            {
                WriteDocumentBehaviorOptions(writer, value);
            }
        }
        public static DCSoft.Writer.DocumentBehaviorOptions ReadDocumentBehaviorOptions(ref Utf8JsonReader jreader, DCSoft.Writer.DocumentBehaviorOptions instance)
        {
            if (instance == null || jreader.TokenType != JsonTokenType.StartObject) return instance;
            while (jreader.Read() && jreader.TokenType != JsonTokenType.EndObject)
            {
                if (jreader.TokenType != JsonTokenType.PropertyName) continue;
                var strNativeName = jreader.GetString();
                object pName = GetValueReference(strNativeName);
                if (jreader.Read() == false) break;
                if (pName == (object)__AcceptDataFormats) instance.AcceptDataFormats = ReadEnumWriterDataFormats(ref jreader, DCSoft.Writer.WriterDataFormats.All);
                else if (pName == (object)__AllowDeleteJumpOutOfField) instance.AllowDeleteJumpOutOfField = jreader.ConvertToBoolean(true);
                else if (pName == (object)__AllowDragContent) instance.AllowDragContent = jreader.ConvertToBoolean(false);
                else if (pName == (object)__AutoClearInvalidateCharacter) instance.AutoClearInvalidateCharacter = jreader.ConvertToBoolean(true);
                else if (pName == (object)__AutoClearTextFormatWhenPasteOrInsertContent) instance.AutoClearTextFormatWhenPasteOrInsertContent = jreader.ConvertToBoolean(false);
                else if (pName == (object)__AutoFixElementIDWhenInsertElements) instance.AutoFixElementIDWhenInsertElements = jreader.ConvertToBoolean(true);
                else if (pName == (object)__CloneSerialize) instance.CloneSerialize = jreader.ConvertToBoolean(true);
                else if (pName == (object)__CompressLayoutForFieldBorder) instance.CompressLayoutForFieldBorder = jreader.ConvertToBoolean(true);
                else if (pName == (object)__CompressXMLContent) instance.CompressXMLContent = jreader.ConvertToBoolean(false);
                else if (pName == (object)__ContinueHeaderParagrahStyle) instance.ContinueHeaderParagrahStyle = jreader.ConvertToBoolean(false);
                else if (pName == (object)__CreationDataFormats) instance.CreationDataFormats = ReadEnumWriterDataFormats(ref jreader, DCSoft.Writer.WriterDataFormats.All);
                else if (pName == (object)__DebugMode) instance.DebugMode = jreader.ConvertToBoolean(false);
                else if (pName == (object)__DefaultEditorActiveMode) instance.DefaultEditorActiveMode = ReadEnumValueEditorActiveMode(ref jreader, DCSoft.Writer.ValueEditorActiveMode.None);
                else if (pName == (object)__DisplayFormatToInnerValue) instance.DisplayFormatToInnerValue = jreader.ConvertToBoolean(true);
                else if (pName == (object)__DoubleClickToSelectWord) instance.DoubleClickToSelectWord = jreader.ConvertToBoolean(true);
                else if (pName == (object)__EnableCalculateControl) instance.EnableCalculateControl = jreader.ConvertToBoolean(true);
                else if (pName == (object)__EnableChineseFontSizeName) instance.EnableChineseFontSizeName = jreader.ConvertToBoolean(true);
                else if (pName == (object)__EnableContentChangedEventWhenLoadDocument) instance.EnableContentChangedEventWhenLoadDocument = jreader.ConvertToBoolean(false);
                else if (pName == (object)__EnabledElementEvent) instance.EnabledElementEvent = jreader.ConvertToBoolean(true);
                else if (pName == (object)__EnableEditElementValue) instance.EnableEditElementValue = jreader.ConvertToBoolean(true);
                else if (pName == (object)__EnableElementEvents) instance.EnableElementEvents = jreader.ConvertToBoolean(true);
                else if (pName == (object)__EnableLogUndo) instance.EnableLogUndo = jreader.ConvertToBoolean(true);
                else if (pName == (object)__ForceRaiseEventAfterFieldContentEdit) instance.ForceRaiseEventAfterFieldContentEdit = jreader.ConvertToBoolean(false);
                else if (pName == (object)__GlobalSpecifyDebugModeValue) instance.GlobalSpecifyDebugModeValue = jreader.ConvertToBoolean(false);
                else if (pName == (object)__HandleCommandException) instance.HandleCommandException = jreader.ConvertToBoolean(true);
                else if (pName == (object)__IgnoreTopBottomPaddingWhenGridLineLayout) instance.IgnoreTopBottomPaddingWhenGridLineLayout = jreader.ConvertToBoolean(false);
                else if (pName == (object)__MoveFieldWhenDragWholeContent) instance.MoveFieldWhenDragWholeContent = jreader.ConvertToBoolean(true);
                else if (pName == (object)__MoveFocusHotKey) instance.MoveFocusHotKey = ReadEnumMoveFocusHotKeys(ref jreader, DCSoft.Writer.MoveFocusHotKeys.None);
                else if (pName == (object)__OutputFormatedXMLSource) instance.OutputFormatedXMLSource = jreader.ConvertToBoolean(true);
                else if (pName == (object)__PageLineUnderPageBreak) instance.PageLineUnderPageBreak = jreader.ConvertToBoolean(false);
                else if (pName == (object)__ParagraphFlagFollowTableOrSection) instance.ParagraphFlagFollowTableOrSection = jreader.ConvertToBoolean(false);
                else if (pName == (object)__ParseCrLfAsLineBreakElement) instance.ParseCrLfAsLineBreakElement = jreader.ConvertToBoolean(false);
                else if (pName == (object)__PromptForRejectFormat) instance.PromptForRejectFormat = jreader.ConvertToBoolean(true);
                else if (pName == (object)__PromptJumpBackForSearch) instance.PromptJumpBackForSearch = jreader.ConvertToBoolean(true);
                else if (pName == (object)__PromptProtectedContent) instance.PromptProtectedContent = ReadEnumPromptProtectedContentMode(ref jreader, DCSoft.Writer.PromptProtectedContentMode.Details);
                else if (pName == (object)__RemoveLastParagraphFlagWhenInsertDocument) instance.RemoveLastParagraphFlagWhenInsertDocument = jreader.ConvertToBoolean(false);
                else if (pName == (object)__ResetTextFormatWhenCreateNewLine) instance.ResetTextFormatWhenCreateNewLine = jreader.ConvertToBoolean(false);
                else if (pName == (object)__SaveBodyTextToXml) instance.SaveBodyTextToXml = jreader.ConvertToBoolean(true);
                else if (pName == (object)__SelectionTextIncludeBackgroundText) instance.SelectionTextIncludeBackgroundText = jreader.ConvertToBoolean(true);
                else if (pName == (object)__SetParagraphFlagHeightUsePreElement) instance.SetParagraphFlagHeightUsePreElement = jreader.ConvertToBoolean(true);
                else if (pName == (object)__ShowDebugMessage) instance.ShowDebugMessage = jreader.ConvertToBoolean(false);
                else if (pName == (object)__ShowTooltip) instance.ShowTooltip = jreader.ConvertToBoolean(true);
                else if (pName == (object)__SpecifyDebugMode) instance.SpecifyDebugMode = jreader.ConvertToBoolean(false);
                else if (pName == (object)__TableCellOperationDetectDistance) instance.TableCellOperationDetectDistance = jreader.ConvertToInt32(3);
                else if (pName == (object)__ThreeClickToSelectParagraph) instance.ThreeClickToSelectParagraph = jreader.ConvertToBoolean(true);
                else if (pName == (object)__ValidateIDRepeatMode) instance.ValidateIDRepeatMode = ReadEnumDCValidateIDRepeatMode(ref jreader, DCSoft.Writer.DCValidateIDRepeatMode.None);
                else if (pName == (object)__WeakMode) instance.WeakMode = jreader.ConvertToBoolean(false);
                else if (pName == (object)__WidelyRaiseFocusEvent) instance.WidelyRaiseFocusEvent = jreader.ConvertToBoolean(false);
                else NotSupportProperty("DocumentBehaviorOptions", strNativeName);
            }//while
            return instance;
        }
        public static void WriteDocumentBehaviorOptions(Utf8JsonWriter jwriter, DCSoft.Writer.DocumentBehaviorOptions instance)
        {
            if (instance == null) throw new System.ArgumentNullException("instance");
            if (jwriter == null) throw new System.ArgumentNullException("jwriter");
            jwriter.WriteStartObject();
            { jwriter.WritePropertyName(__AcceptDataFormats); WriteEnumWriterDataFormats(jwriter, instance.AcceptDataFormats); }
            jwriter.WriteBoolean(__AllowDeleteJumpOutOfField, instance.AllowDeleteJumpOutOfField);
            jwriter.WriteBoolean(__AllowDragContent, instance.AllowDragContent);
            jwriter.WriteBoolean(__AutoClearInvalidateCharacter, instance.AutoClearInvalidateCharacter);
            jwriter.WriteBoolean(__AutoClearTextFormatWhenPasteOrInsertContent, instance.AutoClearTextFormatWhenPasteOrInsertContent);
            jwriter.WriteBoolean(__AutoFixElementIDWhenInsertElements, instance.AutoFixElementIDWhenInsertElements);
            jwriter.WriteBoolean(__CloneSerialize, instance.CloneSerialize);
            jwriter.WriteBoolean(__CompressLayoutForFieldBorder, instance.CompressLayoutForFieldBorder);
            jwriter.WriteBoolean(__CompressXMLContent, instance.CompressXMLContent);
            jwriter.WriteBoolean(__ContinueHeaderParagrahStyle, instance.ContinueHeaderParagrahStyle);
            { jwriter.WritePropertyName(__CreationDataFormats); WriteEnumWriterDataFormats(jwriter, instance.CreationDataFormats); }
            jwriter.WriteBoolean(__DebugMode, instance.DebugMode);
            { jwriter.WritePropertyName(__DefaultEditorActiveMode); WriteEnumValueEditorActiveMode(jwriter, instance.DefaultEditorActiveMode); }
            jwriter.WriteBoolean(__DisplayFormatToInnerValue, instance.DisplayFormatToInnerValue);
            jwriter.WriteBoolean(__DoubleClickToSelectWord, instance.DoubleClickToSelectWord);
            jwriter.WriteBoolean(__EnableCalculateControl, instance.EnableCalculateControl);
            jwriter.WriteBoolean(__EnableChineseFontSizeName, instance.EnableChineseFontSizeName);
            jwriter.WriteBoolean(__EnableContentChangedEventWhenLoadDocument, instance.EnableContentChangedEventWhenLoadDocument);
            jwriter.WriteBoolean(__EnabledElementEvent, instance.EnabledElementEvent);
            jwriter.WriteBoolean(__EnableEditElementValue, instance.EnableEditElementValue);
            jwriter.WriteBoolean(__EnableElementEvents, instance.EnableElementEvents);
            jwriter.WriteBoolean(__EnableLogUndo, instance.EnableLogUndo);
            jwriter.WriteBoolean(__ForceRaiseEventAfterFieldContentEdit, instance.ForceRaiseEventAfterFieldContentEdit);
            jwriter.WriteBoolean(__GlobalSpecifyDebugModeValue, instance.GlobalSpecifyDebugModeValue);
            jwriter.WriteBoolean(__HandleCommandException, instance.HandleCommandException);
            jwriter.WriteBoolean(__IgnoreTopBottomPaddingWhenGridLineLayout, instance.IgnoreTopBottomPaddingWhenGridLineLayout);
            jwriter.WriteBoolean(__MoveFieldWhenDragWholeContent, instance.MoveFieldWhenDragWholeContent);
            { jwriter.WritePropertyName(__MoveFocusHotKey); WriteEnumMoveFocusHotKeys(jwriter, instance.MoveFocusHotKey); }
            jwriter.WriteBoolean(__OutputFormatedXMLSource, instance.OutputFormatedXMLSource);
            jwriter.WriteBoolean(__PageLineUnderPageBreak, instance.PageLineUnderPageBreak);
            jwriter.WriteBoolean(__ParagraphFlagFollowTableOrSection, instance.ParagraphFlagFollowTableOrSection);
            jwriter.WriteBoolean(__ParseCrLfAsLineBreakElement, instance.ParseCrLfAsLineBreakElement);
            jwriter.WriteBoolean(__PromptForRejectFormat, instance.PromptForRejectFormat);
            jwriter.WriteBoolean(__PromptJumpBackForSearch, instance.PromptJumpBackForSearch);
            { jwriter.WritePropertyName(__PromptProtectedContent); WriteEnumPromptProtectedContentMode(jwriter, instance.PromptProtectedContent); }
            jwriter.WriteBoolean(__RemoveLastParagraphFlagWhenInsertDocument, instance.RemoveLastParagraphFlagWhenInsertDocument);
            jwriter.WriteBoolean(__ResetTextFormatWhenCreateNewLine, instance.ResetTextFormatWhenCreateNewLine);
            jwriter.WriteBoolean(__SaveBodyTextToXml, instance.SaveBodyTextToXml);
            jwriter.WriteBoolean(__SelectionTextIncludeBackgroundText, instance.SelectionTextIncludeBackgroundText);
            jwriter.WriteBoolean(__SetParagraphFlagHeightUsePreElement, instance.SetParagraphFlagHeightUsePreElement);
            jwriter.WriteBoolean(__ShowDebugMessage, instance.ShowDebugMessage);
            jwriter.WriteBoolean(__ShowTooltip, instance.ShowTooltip);
            jwriter.WriteBoolean(__SpecifyDebugMode, instance.SpecifyDebugMode);
            jwriter.WriteNumber(__TableCellOperationDetectDistance, instance.TableCellOperationDetectDistance);
            jwriter.WriteBoolean(__ThreeClickToSelectParagraph, instance.ThreeClickToSelectParagraph);
            { jwriter.WritePropertyName(__ValidateIDRepeatMode); WriteEnumDCValidateIDRepeatMode(jwriter, instance.ValidateIDRepeatMode); }
            jwriter.WriteBoolean(__WeakMode, instance.WeakMode);
            jwriter.WriteBoolean(__WidelyRaiseFocusEvent, instance.WidelyRaiseFocusEvent);
            jwriter.WriteEndObject();
        }


        public sealed class JsonConverter_DocumentEditOptions : System.Text.Json.Serialization.JsonConverter<DCSoft.Writer.DocumentEditOptions>
        {
            public override bool CanConvert(Type typeToConvert) { return typeToConvert == typeof(DCSoft.Writer.DocumentEditOptions); }
            public override DCSoft.Writer.DocumentEditOptions Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                return ReadDocumentEditOptions(ref reader, new DCSoft.Writer.DocumentEditOptions());
            }
            public override void Write(Utf8JsonWriter writer, DCSoft.Writer.DocumentEditOptions value, JsonSerializerOptions options)
            {
                WriteDocumentEditOptions(writer, value);
            }
        }
        public static DCSoft.Writer.DocumentEditOptions ReadDocumentEditOptions(ref Utf8JsonReader jreader, DCSoft.Writer.DocumentEditOptions instance)
        {
            if (instance == null || jreader.TokenType != JsonTokenType.StartObject) return instance;
            while (jreader.Read() && jreader.TokenType != JsonTokenType.EndObject)
            {
                if (jreader.TokenType != JsonTokenType.PropertyName) continue;
                var strNativeName = jreader.GetString();
                object pName = GetValueReference(strNativeName);
                if (jreader.Read() == false) break;
                if (pName == (object)__ClearFieldValueWhenCopy) instance.ClearFieldValueWhenCopy = jreader.ConvertToBoolean(false);
                else if (pName == (object)__CloneWithoutElementBorderStyle) instance.CloneWithoutElementBorderStyle = jreader.ConvertToBoolean(true);
                else if (pName == (object)__CopyInTextFormatOnly) instance.CopyInTextFormatOnly = jreader.ConvertToBoolean(false);
                else if (pName == (object)__CopyWithoutInputFieldStructure) instance.CopyWithoutInputFieldStructure = jreader.ConvertToBoolean(false);
                else if (pName == (object)__FixSizeWhenInsertImage) instance.FixSizeWhenInsertImage = jreader.ConvertToBoolean(true);
                else if (pName == (object)__FixWidthWhenInsertTable) instance.FixWidthWhenInsertTable = jreader.ConvertToBoolean(true);
                else if (pName == (object)__KeepTableWidthWhenInsertDeleteColumn) instance.KeepTableWidthWhenInsertDeleteColumn = jreader.ConvertToBoolean(true);
                else if (pName == (object)__TabKeyToFirstLineIndent) instance.TabKeyToFirstLineIndent = jreader.ConvertToBoolean(true);
                else if (pName == (object)__ValueValidateMode) instance.ValueValidateMode = ReadEnumDocumentValueValidateMode(ref jreader, DCSoft.Writer.DocumentValueValidateMode.LostFocus);
                else NotSupportProperty("DocumentEditOptions", strNativeName);
            }//while
            return instance;
        }
        public static void WriteDocumentEditOptions(Utf8JsonWriter jwriter, DCSoft.Writer.DocumentEditOptions instance)
        {
            if (instance == null) throw new System.ArgumentNullException("instance");
            if (jwriter == null) throw new System.ArgumentNullException("jwriter");
            jwriter.WriteStartObject();
            jwriter.WriteBoolean(__ClearFieldValueWhenCopy, instance.ClearFieldValueWhenCopy);
            jwriter.WriteBoolean(__CloneWithoutElementBorderStyle, instance.CloneWithoutElementBorderStyle);
            jwriter.WriteBoolean(__CopyInTextFormatOnly, instance.CopyInTextFormatOnly);
            jwriter.WriteBoolean(__CopyWithoutInputFieldStructure, instance.CopyWithoutInputFieldStructure);
            jwriter.WriteBoolean(__FixSizeWhenInsertImage, instance.FixSizeWhenInsertImage);
            jwriter.WriteBoolean(__FixWidthWhenInsertTable, instance.FixWidthWhenInsertTable);
            jwriter.WriteBoolean(__KeepTableWidthWhenInsertDeleteColumn, instance.KeepTableWidthWhenInsertDeleteColumn);
            jwriter.WriteBoolean(__TabKeyToFirstLineIndent, instance.TabKeyToFirstLineIndent);
            { jwriter.WritePropertyName(__ValueValidateMode); WriteEnumDocumentValueValidateMode(jwriter, instance.ValueValidateMode); }
            jwriter.WriteEndObject();
        }


        public sealed class JsonConverter_DocumentOptions : System.Text.Json.Serialization.JsonConverter<DCSoft.Writer.DocumentOptions>
        {
            public override bool CanConvert(Type typeToConvert) { return typeToConvert == typeof(DCSoft.Writer.DocumentOptions); }
            public override DCSoft.Writer.DocumentOptions Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                return ReadDocumentOptions(ref reader, new DCSoft.Writer.DocumentOptions());
            }
            public override void Write(Utf8JsonWriter writer, DCSoft.Writer.DocumentOptions value, JsonSerializerOptions options)
            {
                WriteDocumentOptions(writer, value);
            }
        }
        public static DCSoft.Writer.DocumentOptions ReadDocumentOptions(ref Utf8JsonReader jreader, DCSoft.Writer.DocumentOptions instance)
        {
            if (instance == null || jreader.TokenType != JsonTokenType.StartObject) return instance;
            while (jreader.Read() && jreader.TokenType != JsonTokenType.EndObject)
            {
                if (jreader.TokenType != JsonTokenType.PropertyName) continue;
                var strNativeName = jreader.GetString();
                object pName = GetValueReference(strNativeName);
                if (jreader.Read() == false) break;
                if (pName == (object)__BehaviorOptions) instance.BehaviorOptions = ReadDocumentBehaviorOptions(ref jreader, new DCSoft.Writer.DocumentBehaviorOptions());
                else if (pName == (object)__EditOptions) instance.EditOptions = ReadDocumentEditOptions(ref jreader, new DCSoft.Writer.DocumentEditOptions());
                else if (pName == (object)__ViewOptions) instance.ViewOptions = ReadDocumentViewOptions(ref jreader, new DCSoft.Writer.DocumentViewOptions());
                else NotSupportProperty("DocumentOptions", strNativeName);
            }//while
            return instance;
        }
        public static void WriteDocumentOptions(Utf8JsonWriter jwriter, DCSoft.Writer.DocumentOptions instance)
        {
            if (instance == null) throw new System.ArgumentNullException("instance");
            if (jwriter == null) throw new System.ArgumentNullException("jwriter");
            jwriter.WriteStartObject();
            { jwriter.WritePropertyName(__BehaviorOptions); WriteDocumentBehaviorOptions(jwriter, instance.BehaviorOptions); }
            { jwriter.WritePropertyName(__EditOptions); WriteDocumentEditOptions(jwriter, instance.EditOptions); }
            { jwriter.WritePropertyName(__ViewOptions); WriteDocumentViewOptions(jwriter, instance.ViewOptions); }
            jwriter.WriteEndObject();
        }




        public sealed class JsonConverter_DocumentValueValidateMode : System.Text.Json.Serialization.JsonConverter<DCSoft.Writer.DocumentValueValidateMode>
        {
            public override bool CanConvert(Type typeToConvert) { return typeToConvert == typeof(DCSoft.Writer.DocumentValueValidateMode); }
            public override DCSoft.Writer.DocumentValueValidateMode Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                return ReadEnumDocumentValueValidateMode(ref reader, default(DCSoft.Writer.DocumentValueValidateMode));
            }
            public override void Write(Utf8JsonWriter writer, DCSoft.Writer.DocumentValueValidateMode Value, JsonSerializerOptions options)
            {
                WriteEnumDocumentValueValidateMode(writer, Value);
            }
        }
        private static readonly string[] _Names_DocumentValueValidateMode = new string[]{
           "None", // 0
           "Dynamic", // 1
           "LostFocus", // 2
           "Program"}; // 3
        public static DCSoft.Writer.DocumentValueValidateMode ReadEnumDocumentValueValidateMode(ref Utf8JsonReader reader, DCSoft.Writer.DocumentValueValidateMode defaultValue)
        {
            if (reader.TokenType == JsonTokenType.String)
            {
                var strValue = reader.GetString();
                for (var iCount = _Names_DocumentValueValidateMode.Length - 1; iCount >= 0; iCount--)
                {
                    if (strValue.Equals(_Names_DocumentValueValidateMode[iCount], StringComparison.OrdinalIgnoreCase)) return (DCSoft.Writer.DocumentValueValidateMode)iCount;
                }
            }
            else if (reader.TokenType == JsonTokenType.Number) { return (DCSoft.Writer.DocumentValueValidateMode)reader.GetInt32(); }
            return defaultValue;
        }
        public static void WriteEnumDocumentValueValidateMode(Utf8JsonWriter writer, DCSoft.Writer.DocumentValueValidateMode vValue)
        {
            var index = (int)vValue;
            if (index >= 0 && index < _Names_DocumentValueValidateMode.Length) writer.WriteStringValue(_Names_DocumentValueValidateMode[index]);
            else writer.WriteNumberValue((int)vValue);
        }


        public sealed class JsonConverter_DocumentViewOptions : System.Text.Json.Serialization.JsonConverter<DCSoft.Writer.DocumentViewOptions>
        {
            public override bool CanConvert(Type typeToConvert) { return typeToConvert == typeof(DCSoft.Writer.DocumentViewOptions); }
            public override DCSoft.Writer.DocumentViewOptions Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                return ReadDocumentViewOptions(ref reader, new DCSoft.Writer.DocumentViewOptions());
            }
            public override void Write(Utf8JsonWriter writer, DCSoft.Writer.DocumentViewOptions value, JsonSerializerOptions options)
            {
                WriteDocumentViewOptions(writer, value);
            }
        }
        public static DCSoft.Writer.DocumentViewOptions ReadDocumentViewOptions(ref Utf8JsonReader jreader, DCSoft.Writer.DocumentViewOptions instance)
        {
            if (instance == null || jreader.TokenType != JsonTokenType.StartObject) return instance;
            while (jreader.Read() && jreader.TokenType != JsonTokenType.EndObject)
            {
                if (jreader.TokenType != JsonTokenType.PropertyName) continue;
                var strNativeName = jreader.GetString();
                object pName = GetValueReference(strNativeName);
                if (jreader.Read() == false) break;
                else if (pName == (object)__BackgroundTextColor) instance.BackgroundTextColor = jreader.ConvertToColor(Color.Gray);
                else if (pName == (object)__DefaultInputFieldHighlight) instance.DefaultInputFieldHighlight = ReadEnumEnableState(ref jreader, DCSoft.Writer.EnableState.Enabled);
                else if (pName == (object)__FieldBackColor) instance.FieldBackColor = jreader.ConvertToColor(Color.AliceBlue);
                else if (pName == (object)__FieldBorderColor) instance.FieldBorderColor = jreader.ConvertToColor(Color.Empty);
                else if (pName == (object)__FieldBorderElementPixelWidth) instance.FieldBorderElementPixelWidth = jreader.ConvertToSingle(1f);
                else if (pName == (object)__FieldFocusedBackColor) instance.FieldFocusedBackColor = jreader.ConvertToColor(Color.LightBlue);
                else if (pName == (object)__FieldHoverBackColor) instance.FieldHoverBackColor = jreader.ConvertToColor(Color.LightBlue);
                else if (pName == (object)__FieldInvalidateValueBackColor) instance.FieldInvalidateValueBackColor = jreader.ConvertToColor(Color.Transparent);
                else if (pName == (object)__FieldInvalidateValueForeColor) instance.FieldInvalidateValueForeColor = jreader.ConvertToColor(Color.LightCoral);
                else if (pName == (object)__HeaderBottomLineWidth) instance.HeaderBottomLineWidth = jreader.ConvertToSingle(1f);
                else if (pName == (object)__HiddenFieldBorderWhenLostFocus) instance.HiddenFieldBorderWhenLostFocus = jreader.ConvertToBoolean(true);
                else if (pName == (object)__MinTableColumnWidth) instance.MinTableColumnWidth = jreader.ConvertToSingle(50f);
                else if (pName == (object)__NoneBorderColor) instance.NoneBorderColor = jreader.ConvertToColor(Color.LightGray);
                else if (pName == (object)__NormalFieldBorderColor) instance.NormalFieldBorderColor = jreader.ConvertToColor(Color.Blue);
                else if (pName == (object)__PageMarginLineLength) instance.PageMarginLineLength = jreader.ConvertToInt32(30);
                else if (pName == (object)__PreserveBackgroundTextWhenPrint) instance.PreserveBackgroundTextWhenPrint = jreader.ConvertToBoolean(false);
                else if (pName == (object)__PrintBackgroundText) instance.PrintBackgroundText = jreader.ConvertToBoolean(false);
                else if (pName == (object)__SelectionHightlightMaskColor) instance.SelectionHightlightMaskColor = jreader.ConvertToColor(Color.Black);
                else if (pName == (object)__ShowCellNoneBorder) instance.ShowCellNoneBorder = jreader.ConvertToBoolean(true);
                else if (pName == (object)__ShowGlobalGridLineInTableAndSection) instance.ShowGlobalGridLineInTableAndSection = jreader.ConvertToBoolean(true);
                else if (pName == (object)__ShowHeaderBottomLine) instance.ShowHeaderBottomLine = jreader.ConvertToBoolean(true);
                else if (pName == (object)__ShowPageBreak) instance.ShowPageBreak = jreader.ConvertToBoolean(false);
                else if (pName == (object)__ShowPageLine) instance.ShowPageLine = jreader.ConvertToBoolean(false);
                else if (pName == (object)__ShowParagraphFlag) instance.ShowParagraphFlag = jreader.ConvertToBoolean(true);
                else if (pName == (object)__ShowRedErrorMessageForPaint) instance.ShowRedErrorMessageForPaint = jreader.ConvertToBoolean(true);
                else if (pName == (object)__UnEditableFieldBorderColor) instance.UnEditableFieldBorderColor = jreader.ConvertToColor(Color.Red);
                else NotSupportProperty("DocumentViewOptions", strNativeName);
            }//while
            return instance;
        }
        public static void WriteDocumentViewOptions(Utf8JsonWriter jwriter, DCSoft.Writer.DocumentViewOptions instance)
        {
            if (instance == null) throw new System.ArgumentNullException("instance");
            if (jwriter == null) throw new System.ArgumentNullException("jwriter");
            jwriter.WriteStartObject();
            jwriter.WriteColorString(__BackgroundTextColor, instance.BackgroundTextColor);
            { jwriter.WritePropertyName(__DefaultInputFieldHighlight); WriteEnumEnableState(jwriter, instance.DefaultInputFieldHighlight); }
            jwriter.WriteColorString(__FieldBackColor, instance.FieldBackColor);
            jwriter.WriteColorString(__FieldBorderColor, instance.FieldBorderColor);
            jwriter.WriteNumber(__FieldBorderElementPixelWidth, instance.FieldBorderElementPixelWidth);
            jwriter.WriteColorString(__FieldFocusedBackColor, instance.FieldFocusedBackColor);
            jwriter.WriteColorString(__FieldHoverBackColor, instance.FieldHoverBackColor);
            jwriter.WriteColorString(__FieldInvalidateValueBackColor, instance.FieldInvalidateValueBackColor);
            jwriter.WriteColorString(__FieldInvalidateValueForeColor, instance.FieldInvalidateValueForeColor);
            jwriter.WriteNumber(__HeaderBottomLineWidth, instance.HeaderBottomLineWidth);
            jwriter.WriteBoolean(__HiddenFieldBorderWhenLostFocus, instance.HiddenFieldBorderWhenLostFocus);
            jwriter.WriteNumber(__MinTableColumnWidth, instance.MinTableColumnWidth);
            jwriter.WriteColorString(__NoneBorderColor, instance.NoneBorderColor);
            jwriter.WriteColorString(__NormalFieldBorderColor, instance.NormalFieldBorderColor);
            jwriter.WriteNumber(__PageMarginLineLength, instance.PageMarginLineLength);
            jwriter.WriteBoolean(__PreserveBackgroundTextWhenPrint, instance.PreserveBackgroundTextWhenPrint);
            jwriter.WriteBoolean(__PrintBackgroundText, instance.PrintBackgroundText);
            jwriter.WriteColorString(__SelectionHightlightMaskColor, instance.SelectionHightlightMaskColor);
            jwriter.WriteBoolean(__ShowCellNoneBorder, instance.ShowCellNoneBorder);
            jwriter.WriteBoolean(__ShowGlobalGridLineInTableAndSection, instance.ShowGlobalGridLineInTableAndSection);
            jwriter.WriteBoolean(__ShowHeaderBottomLine, instance.ShowHeaderBottomLine);
            jwriter.WriteBoolean(__ShowPageBreak, instance.ShowPageBreak);
            jwriter.WriteBoolean(__ShowPageLine, instance.ShowPageLine);
            jwriter.WriteBoolean(__ShowParagraphFlag, instance.ShowParagraphFlag);
            jwriter.WriteBoolean(__ShowRedErrorMessageForPaint, instance.ShowRedErrorMessageForPaint);
            jwriter.WriteColorString(__UnEditableFieldBorderColor, instance.UnEditableFieldBorderColor);
            jwriter.WriteEndObject();
        }


        public sealed class JsonConverter_EnableState : System.Text.Json.Serialization.JsonConverter<DCSoft.Writer.EnableState>
        {
            public override bool CanConvert(Type typeToConvert) { return typeToConvert == typeof(DCSoft.Writer.EnableState); }
            public override DCSoft.Writer.EnableState Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                return ReadEnumEnableState(ref reader, default(DCSoft.Writer.EnableState));
            }
            public override void Write(Utf8JsonWriter writer, DCSoft.Writer.EnableState Value, JsonSerializerOptions options)
            {
                WriteEnumEnableState(writer, Value);
            }
        }
        private static readonly string[] _Names_EnableState = new string[]{
           "Default", // 0
           "Enabled", // 1
           "Disabled"}; // 2
        public static DCSoft.Writer.EnableState ReadEnumEnableState(ref Utf8JsonReader reader, DCSoft.Writer.EnableState defaultValue)
        {
            if (reader.TokenType == JsonTokenType.String)
            {
                var strValue = reader.GetString();
                for (var iCount = _Names_EnableState.Length - 1; iCount >= 0; iCount--)
                {
                    if (strValue.Equals(_Names_EnableState[iCount], StringComparison.OrdinalIgnoreCase)) return (DCSoft.Writer.EnableState)iCount;
                }
            }
            else if (reader.TokenType == JsonTokenType.Number) { return (DCSoft.Writer.EnableState)reader.GetInt32(); }
            return defaultValue;
        }
        public static void WriteEnumEnableState(Utf8JsonWriter writer, DCSoft.Writer.EnableState vValue)
        {
            var index = (int)vValue;
            if (index >= 0 && index < _Names_EnableState.Length) writer.WriteStringValue(_Names_EnableState[index]);
            else writer.WriteNumberValue((int)vValue);
        }
        public sealed class JsonConverter_GraphicsUnit : System.Text.Json.Serialization.JsonConverter<DCSystem_Drawing.GraphicsUnit>
        {
            public override bool CanConvert(Type typeToConvert) { return typeToConvert == typeof(DCSystem_Drawing.GraphicsUnit); }
            public override DCSystem_Drawing.GraphicsUnit Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                return ReadEnumGraphicsUnit(ref reader, default(DCSystem_Drawing.GraphicsUnit));
            }
            public override void Write(Utf8JsonWriter writer, DCSystem_Drawing.GraphicsUnit Value, JsonSerializerOptions options)
            {
                WriteEnumGraphicsUnit(writer, Value);
            }
        }
        private static readonly string[] _Names_GraphicsUnit = new string[]{
           "World", // 0
           "Display", // 1
           "Pixel", // 2
           "Point", // 3
           "Inch", // 4
           "Document", // 5
           "Millimeter"}; // 6
        public static DCSystem_Drawing.GraphicsUnit ReadEnumGraphicsUnit(ref Utf8JsonReader reader, DCSystem_Drawing.GraphicsUnit defaultValue)
        {
            if (reader.TokenType == JsonTokenType.String)
            {
                var strValue = reader.GetString();
                for (var iCount = _Names_GraphicsUnit.Length - 1; iCount >= 0; iCount--)
                {
                    if (strValue.Equals(_Names_GraphicsUnit[iCount], StringComparison.OrdinalIgnoreCase)) return (DCSystem_Drawing.GraphicsUnit)iCount;
                }
            }
            else if (reader.TokenType == JsonTokenType.Number) { return (DCSystem_Drawing.GraphicsUnit)reader.GetInt32(); }
            return defaultValue;
        }
        public static void WriteEnumGraphicsUnit(Utf8JsonWriter writer, DCSystem_Drawing.GraphicsUnit vValue)
        {
            var index = (int)vValue;
            if (index >= 0 && index < _Names_GraphicsUnit.Length) writer.WriteStringValue(_Names_GraphicsUnit[index]);
            else writer.WriteNumberValue((int)vValue);
        }



        public sealed class JsonConverter_MoveFocusHotKeys : System.Text.Json.Serialization.JsonConverter<DCSoft.Writer.MoveFocusHotKeys>
        {
            public override bool CanConvert(Type typeToConvert) { return typeToConvert == typeof(DCSoft.Writer.MoveFocusHotKeys); }
            public override DCSoft.Writer.MoveFocusHotKeys Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                return ReadEnumMoveFocusHotKeys(ref reader, default(DCSoft.Writer.MoveFocusHotKeys));
            }
            public override void Write(Utf8JsonWriter writer, DCSoft.Writer.MoveFocusHotKeys Value, JsonSerializerOptions options)
            {
                WriteEnumMoveFocusHotKeys(writer, Value);
            }
        }
        public static DCSoft.Writer.MoveFocusHotKeys ReadEnumMoveFocusHotKeys(ref Utf8JsonReader reader, DCSoft.Writer.MoveFocusHotKeys defaultValue)
        {
            if (reader.TokenType == JsonTokenType.String)
            {
                var strValue = reader.GetString();
                var result = defaultValue;
                if (Enum.TryParse<DCSoft.Writer.MoveFocusHotKeys>(strValue, out result)) return result;
                else return defaultValue;
            }
            else if (reader.TokenType == JsonTokenType.Number) { return (DCSoft.Writer.MoveFocusHotKeys)reader.GetInt32(); }
            return defaultValue;
        }
        public static void WriteEnumMoveFocusHotKeys(Utf8JsonWriter writer, DCSoft.Writer.MoveFocusHotKeys vValue)
        {
            writer.WriteStringValue(vValue.ToString());
        }


        public sealed class JsonConverter_PromptProtectedContentMode : System.Text.Json.Serialization.JsonConverter<DCSoft.Writer.PromptProtectedContentMode>
        {
            public override bool CanConvert(Type typeToConvert) { return typeToConvert == typeof(DCSoft.Writer.PromptProtectedContentMode); }
            public override DCSoft.Writer.PromptProtectedContentMode Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                return ReadEnumPromptProtectedContentMode(ref reader, default(DCSoft.Writer.PromptProtectedContentMode));
            }
            public override void Write(Utf8JsonWriter writer, DCSoft.Writer.PromptProtectedContentMode Value, JsonSerializerOptions options)
            {
                WriteEnumPromptProtectedContentMode(writer, Value);
            }
        }
        private static readonly string[] _Names_PromptProtectedContentMode = new string[]{
           "None", // 0
           "Simple", // 1
           "Details"}; // 2
        public static DCSoft.Writer.PromptProtectedContentMode ReadEnumPromptProtectedContentMode(ref Utf8JsonReader reader, DCSoft.Writer.PromptProtectedContentMode defaultValue)
        {
            if (reader.TokenType == JsonTokenType.String)
            {
                var strValue = reader.GetString();
                for (var iCount = _Names_PromptProtectedContentMode.Length - 1; iCount >= 0; iCount--)
                {
                    if (strValue.Equals(_Names_PromptProtectedContentMode[iCount], StringComparison.OrdinalIgnoreCase)) return (DCSoft.Writer.PromptProtectedContentMode)iCount;
                }
            }
            else if (reader.TokenType == JsonTokenType.Number) { return (DCSoft.Writer.PromptProtectedContentMode)reader.GetInt32(); }
            return defaultValue;
        }
        public static void WriteEnumPromptProtectedContentMode(Utf8JsonWriter writer, DCSoft.Writer.PromptProtectedContentMode vValue)
        {
            var index = (int)vValue;
            if (index >= 0 && index < _Names_PromptProtectedContentMode.Length) writer.WriteStringValue(_Names_PromptProtectedContentMode[index]);
            else writer.WriteNumberValue((int)vValue);
        }
        public sealed class JsonConverter_ValueEditorActiveMode : System.Text.Json.Serialization.JsonConverter<DCSoft.Writer.ValueEditorActiveMode>
        {
            public override bool CanConvert(Type typeToConvert) { return typeToConvert == typeof(DCSoft.Writer.ValueEditorActiveMode); }
            public override DCSoft.Writer.ValueEditorActiveMode Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                return ReadEnumValueEditorActiveMode(ref reader, default(DCSoft.Writer.ValueEditorActiveMode));
            }
            public override void Write(Utf8JsonWriter writer, DCSoft.Writer.ValueEditorActiveMode Value, JsonSerializerOptions options)
            {
                WriteEnumValueEditorActiveMode(writer, Value);
            }
        }
        public static DCSoft.Writer.ValueEditorActiveMode ReadEnumValueEditorActiveMode(ref Utf8JsonReader reader, DCSoft.Writer.ValueEditorActiveMode defaultValue)
        {
            if (reader.TokenType == JsonTokenType.String)
            {
                var strValue = reader.GetString();
                var result = defaultValue;
                if (Enum.TryParse<DCSoft.Writer.ValueEditorActiveMode>(strValue, out result)) return result;
                else return defaultValue;
            }
            else if (reader.TokenType == JsonTokenType.Number) { return (DCSoft.Writer.ValueEditorActiveMode)reader.GetInt32(); }
            return defaultValue;
        }
        public static void WriteEnumValueEditorActiveMode(Utf8JsonWriter writer, DCSoft.Writer.ValueEditorActiveMode vValue)
        {
            writer.WriteStringValue(vValue.ToString());
        }


        public sealed class JsonConverter_ValueFormater : System.Text.Json.Serialization.JsonConverter<DCSoft.Data.ValueFormater>
        {
            public override bool CanConvert(Type typeToConvert) { return typeToConvert == typeof(DCSoft.Data.ValueFormater); }
            public override DCSoft.Data.ValueFormater Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                return ReadValueFormater(ref reader, new DCSoft.Data.ValueFormater());
            }
            public override void Write(Utf8JsonWriter writer, DCSoft.Data.ValueFormater value, JsonSerializerOptions options)
            {
                WriteValueFormater(writer, value);
            }
        }
        public static DCSoft.Data.ValueFormater ReadValueFormater(ref Utf8JsonReader jreader, DCSoft.Data.ValueFormater instance)
        {
            if (instance == null || jreader.TokenType != JsonTokenType.StartObject) return instance;
            while (jreader.Read() && jreader.TokenType != JsonTokenType.EndObject)
            {
                if (jreader.TokenType != JsonTokenType.PropertyName) continue;
                var strNativeName = jreader.GetString();
                object pName = GetValueReference(strNativeName);
                if (jreader.Read() == false) break;
                if (pName == (object)__Format) instance.Format = jreader.GetString();
                else if (pName == (object)__NoneText) instance.NoneText = jreader.GetString();
                else if (pName == (object)__Style) instance.Style = ReadEnumValueFormatStyle(ref jreader, DCSoft.Data.ValueFormatStyle.None);
                else NotSupportProperty("ValueFormater", strNativeName);
            }//while
            return instance;
        }
        public static void WriteValueFormater(Utf8JsonWriter jwriter, DCSoft.Data.ValueFormater instance)
        {
            if (instance == null) throw new System.ArgumentNullException("instance");
            if (jwriter == null) throw new System.ArgumentNullException("jwriter");
            jwriter.WriteStartObject();
            jwriter.WriteString(__Format, instance.Format);
            jwriter.WriteString(__NoneText, instance.NoneText);
            { jwriter.WritePropertyName(__Style); WriteEnumValueFormatStyle(jwriter, instance.Style); }
            jwriter.WriteEndObject();
        }


        public sealed class JsonConverter_ValueFormatStyle : System.Text.Json.Serialization.JsonConverter<DCSoft.Data.ValueFormatStyle>
        {
            public override bool CanConvert(Type typeToConvert) { return typeToConvert == typeof(DCSoft.Data.ValueFormatStyle); }
            public override DCSoft.Data.ValueFormatStyle Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                return ReadEnumValueFormatStyle(ref reader, default(DCSoft.Data.ValueFormatStyle));
            }
            public override void Write(Utf8JsonWriter writer, DCSoft.Data.ValueFormatStyle Value, JsonSerializerOptions options)
            {
                WriteEnumValueFormatStyle(writer, Value);
            }
        }
        private static readonly string[] _Names_ValueFormatStyle = new string[]{
           "None", // 0
           "Numeric", // 1
           "Currency", // 2
           "DateTime", // 3
           "String", // 4
           "SpecifyLength", // 5
           "Boolean", // 6
           "Percent"}; // 7
        public static DCSoft.Data.ValueFormatStyle ReadEnumValueFormatStyle(ref Utf8JsonReader reader, DCSoft.Data.ValueFormatStyle defaultValue)
        {
            if (reader.TokenType == JsonTokenType.String)
            {
                var strValue = reader.GetString();
                for (var iCount = _Names_ValueFormatStyle.Length - 1; iCount >= 0; iCount--)
                {
                    if (strValue.Equals(_Names_ValueFormatStyle[iCount], StringComparison.OrdinalIgnoreCase)) return (DCSoft.Data.ValueFormatStyle)iCount;
                }
            }
            else if (reader.TokenType == JsonTokenType.Number) { return (DCSoft.Data.ValueFormatStyle)reader.GetInt32(); }
            return defaultValue;
        }
        public static void WriteEnumValueFormatStyle(Utf8JsonWriter writer, DCSoft.Data.ValueFormatStyle vValue)
        {
            var index = (int)vValue;
            if (index >= 0 && index < _Names_ValueFormatStyle.Length) writer.WriteStringValue(_Names_ValueFormatStyle[index]);
            else writer.WriteNumberValue((int)vValue);
        }


        public sealed class JsonConverter_ValueTypeStyle : System.Text.Json.Serialization.JsonConverter<DCSoft.Common.ValueTypeStyle>
        {
            public override bool CanConvert(Type typeToConvert) { return typeToConvert == typeof(DCSoft.Common.ValueTypeStyle); }
            public override DCSoft.Common.ValueTypeStyle Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                return ReadEnumValueTypeStyle(ref reader, default(DCSoft.Common.ValueTypeStyle));
            }
            public override void Write(Utf8JsonWriter writer, DCSoft.Common.ValueTypeStyle Value, JsonSerializerOptions options)
            {
                WriteEnumValueTypeStyle(writer, Value);
            }
        }
        private static readonly string[] _Names_ValueTypeStyle = new string[]{
           "Text", // 0
           "Integer", // 1
           "Numeric", // 2
           "Date", // 3
           "Time", // 4
           "DateTime", // 5
           "RegExpress"}; // 6
        public static DCSoft.Common.ValueTypeStyle ReadEnumValueTypeStyle(ref Utf8JsonReader reader, DCSoft.Common.ValueTypeStyle defaultValue)
        {
            if (reader.TokenType == JsonTokenType.String)
            {
                var strValue = reader.GetString();
                for (var iCount = _Names_ValueTypeStyle.Length - 1; iCount >= 0; iCount--)
                {
                    if (strValue.Equals(_Names_ValueTypeStyle[iCount], StringComparison.OrdinalIgnoreCase)) return (DCSoft.Common.ValueTypeStyle)iCount;
                }
            }
            else if (reader.TokenType == JsonTokenType.Number) { return (DCSoft.Common.ValueTypeStyle)reader.GetInt32(); }
            return defaultValue;
        }
        public static void WriteEnumValueTypeStyle(Utf8JsonWriter writer, DCSoft.Common.ValueTypeStyle vValue)
        {
            var index = (int)vValue;
            if (index >= 0 && index < _Names_ValueTypeStyle.Length) writer.WriteStringValue(_Names_ValueTypeStyle[index]);
            else writer.WriteNumberValue((int)vValue);
        }


        public sealed class JsonConverter_ValueValidateLevel : System.Text.Json.Serialization.JsonConverter<DCSoft.Common.ValueValidateLevel>
        {
            public override bool CanConvert(Type typeToConvert) { return typeToConvert == typeof(DCSoft.Common.ValueValidateLevel); }
            public override DCSoft.Common.ValueValidateLevel Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                return ReadEnumValueValidateLevel(ref reader, default(DCSoft.Common.ValueValidateLevel));
            }
            public override void Write(Utf8JsonWriter writer, DCSoft.Common.ValueValidateLevel Value, JsonSerializerOptions options)
            {
                WriteEnumValueValidateLevel(writer, Value);
            }
        }
        private static readonly string[] _Names_ValueValidateLevel = new string[]{
           "Error", // 0
           "Warring", // 1
           "Info"}; // 2
        public static DCSoft.Common.ValueValidateLevel ReadEnumValueValidateLevel(ref Utf8JsonReader reader, DCSoft.Common.ValueValidateLevel defaultValue)
        {
            if (reader.TokenType == JsonTokenType.String)
            {
                var strValue = reader.GetString();
                for (var iCount = _Names_ValueValidateLevel.Length - 1; iCount >= 0; iCount--)
                {
                    if (strValue.Equals(_Names_ValueValidateLevel[iCount], StringComparison.OrdinalIgnoreCase)) return (DCSoft.Common.ValueValidateLevel)iCount;
                }
            }
            else if (reader.TokenType == JsonTokenType.Number) { return (DCSoft.Common.ValueValidateLevel)reader.GetInt32(); }
            return defaultValue;
        }
        public static void WriteEnumValueValidateLevel(Utf8JsonWriter writer, DCSoft.Common.ValueValidateLevel vValue)
        {
            var index = (int)vValue;
            if (index >= 0 && index < _Names_ValueValidateLevel.Length) writer.WriteStringValue(_Names_ValueValidateLevel[index]);
            else writer.WriteNumberValue((int)vValue);
        }


        public sealed class JsonConverter_ValueValidateStyle : System.Text.Json.Serialization.JsonConverter<DCSoft.Common.ValueValidateStyle>
        {
            public override bool CanConvert(Type typeToConvert) { return typeToConvert == typeof(DCSoft.Common.ValueValidateStyle); }
            public override DCSoft.Common.ValueValidateStyle Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                return ReadValueValidateStyle(ref reader, new DCSoft.Common.ValueValidateStyle());
            }
            public override void Write(Utf8JsonWriter writer, DCSoft.Common.ValueValidateStyle value, JsonSerializerOptions options)
            {
                WriteValueValidateStyle(writer, value);
            }
        }
        public static DCSoft.Common.ValueValidateStyle ReadValueValidateStyle(ref Utf8JsonReader jreader, DCSoft.Common.ValueValidateStyle instance)
        {
            if (instance == null || jreader.TokenType != JsonTokenType.StartObject) return instance;
            while (jreader.Read() && jreader.TokenType != JsonTokenType.EndObject)
            {
                if (jreader.TokenType != JsonTokenType.PropertyName) continue;
                var strNativeName = jreader.GetString();
                object pName = GetValueReference(strNativeName);
                if (jreader.Read() == false) break;
                if (pName == (object)__BinaryLength) instance.BinaryLength = jreader.ConvertToBoolean(false);
                else if (pName == (object)__CheckDecimalDigits) instance.CheckDecimalDigits = jreader.ConvertToBoolean(false);
                else if (pName == (object)__CheckMaxValue) instance.CheckMaxValue = jreader.ConvertToBoolean(false);
                else if (pName == (object)__CheckMinValue) instance.CheckMinValue = jreader.ConvertToBoolean(false);
                else if (pName == (object)__DateTimeMaxValue) instance.DateTimeMaxValue = jreader.ConvertToDateTime(624511296000000000);
                else if (pName == (object)__DateTimeMinValue) instance.DateTimeMinValue = jreader.ConvertToDateTime(624511296000000000);
                else if (pName == (object)__Level) instance.Level = ReadEnumValueValidateLevel(ref jreader, DCSoft.Common.ValueValidateLevel.Error);
                else if (pName == (object)__MaxDecimalDigits) instance.MaxDecimalDigits = jreader.ConvertToInt32(0);
                else if (pName == (object)__MaxLength) instance.MaxLength = jreader.ConvertToInt32(0);
                else if (pName == (object)__MaxValue) instance.MaxValue = jreader.ConvertToDouble(0);
                else if (pName == (object)__MinLength) instance.MinLength = jreader.ConvertToInt32(0);
                else if (pName == (object)__MinValue) instance.MinValue = jreader.ConvertToDouble(0);
                else if (pName == (object)__Required) instance.Required = jreader.ConvertToBoolean(false);
                else if (pName == (object)__ValueName) instance.ValueName = jreader.GetString();
                else if (pName == (object)__ValueType) instance.ValueType = ReadEnumValueTypeStyle(ref jreader, DCSoft.Common.ValueTypeStyle.Text);
                else NotSupportProperty("ValueValidateStyle", strNativeName);
            }//while
            return instance;
        }
        public static void WriteValueValidateStyle(Utf8JsonWriter jwriter, DCSoft.Common.ValueValidateStyle instance)
        {
            if (instance == null) throw new System.ArgumentNullException("instance");
            if (jwriter == null) throw new System.ArgumentNullException("jwriter");
            jwriter.WriteStartObject();
            jwriter.WriteBoolean(__BinaryLength, instance.BinaryLength);
            jwriter.WriteBoolean(__CheckDecimalDigits, instance.CheckDecimalDigits);
            jwriter.WriteBoolean(__CheckMaxValue, instance.CheckMaxValue);
            jwriter.WriteBoolean(__CheckMinValue, instance.CheckMinValue);
            jwriter.WriteString(__DateTimeMaxValue, DCSoft.Common.DateTimeCommon.FastToYYYY_MM_DD_HH_MM_SS(instance.DateTimeMaxValue));
            jwriter.WriteString(__DateTimeMinValue, DCSoft.Common.DateTimeCommon.FastToYYYY_MM_DD_HH_MM_SS(instance.DateTimeMinValue));
            { jwriter.WritePropertyName(__Level); WriteEnumValueValidateLevel(jwriter, instance.Level); }
            jwriter.WriteNumber(__MaxDecimalDigits, instance.MaxDecimalDigits);
            jwriter.WriteNumber(__MaxLength, instance.MaxLength);
            jwriter.WriteNumber(__MaxValue, instance.MaxValue);
            jwriter.WriteNumber(__MinLength, instance.MinLength);
            jwriter.WriteNumber(__MinValue, instance.MinValue);
            jwriter.WriteBoolean(__Required, instance.Required);
            jwriter.WriteString(__ValueName, instance.ValueName);
            { jwriter.WritePropertyName(__ValueType); WriteEnumValueTypeStyle(jwriter, instance.ValueType); }
            jwriter.WriteEndObject();
        }

        public sealed class JsonConverter_WriterDataFormats : System.Text.Json.Serialization.JsonConverter<DCSoft.Writer.WriterDataFormats>
        {
            public override bool CanConvert(Type typeToConvert) { return typeToConvert == typeof(DCSoft.Writer.WriterDataFormats); }
            public override DCSoft.Writer.WriterDataFormats Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                return ReadEnumWriterDataFormats(ref reader, default(DCSoft.Writer.WriterDataFormats));
            }
            public override void Write(Utf8JsonWriter writer, DCSoft.Writer.WriterDataFormats Value, JsonSerializerOptions options)
            {
                WriteEnumWriterDataFormats(writer, Value);
            }
        }
        public static DCSoft.Writer.WriterDataFormats ReadEnumWriterDataFormats(ref Utf8JsonReader reader, DCSoft.Writer.WriterDataFormats defaultValue)
        {
            if (reader.TokenType == JsonTokenType.String)
            {
                var strValue = reader.GetString();
                var result = defaultValue;
                if (Enum.TryParse<DCSoft.Writer.WriterDataFormats>(strValue, out result)) return result;
                else return defaultValue;
            }
            else if (reader.TokenType == JsonTokenType.Number) { return (DCSoft.Writer.WriterDataFormats)reader.GetInt32(); }
            return defaultValue;
        }
        public static void WriteEnumWriterDataFormats(Utf8JsonWriter writer, DCSoft.Writer.WriterDataFormats vValue)
        {
            writer.WriteStringValue(vValue.ToString());
        }


        public sealed class JsonConverter_XFontValue : System.Text.Json.Serialization.JsonConverter<DCSoft.Drawing.XFontValue>
        {
            public override bool CanConvert(Type typeToConvert) { return typeToConvert == typeof(DCSoft.Drawing.XFontValue); }
            public override DCSoft.Drawing.XFontValue Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                return ReadXFontValue(ref reader, new DCSoft.Drawing.XFontValue());
            }
            public override void Write(Utf8JsonWriter writer, DCSoft.Drawing.XFontValue value, JsonSerializerOptions options)
            {
                WriteXFontValue(writer, value);
            }
        }
        public static DCSoft.Drawing.XFontValue ReadXFontValue(ref Utf8JsonReader jreader, DCSoft.Drawing.XFontValue instance)
        {
            if (instance == null || jreader.TokenType != JsonTokenType.StartObject) return instance;
            while (jreader.Read() && jreader.TokenType != JsonTokenType.EndObject)
            {
                if (jreader.TokenType != JsonTokenType.PropertyName) continue;
                var strNativeName = jreader.GetString();
                object pName = GetValueReference(strNativeName);
                if (jreader.Read() == false) break;
                if (pName == (object)__Bold) instance.Bold = jreader.ConvertToBoolean(false);
                else if (pName == (object)__Italic) instance.Italic = jreader.ConvertToBoolean(false);
                else if (pName == (object)__Name) instance.Name = jreader.GetString();
                else if (pName == (object)__Size) instance.Size = jreader.ConvertToSingle(9f);
                else if (pName == (object)__Strikeout) instance.Strikeout = jreader.ConvertToBoolean(false);
                else if (pName == (object)__Underline) instance.Underline = jreader.ConvertToBoolean(false);
                else if (pName == (object)__Unit) instance.Unit = ReadEnumGraphicsUnit(ref jreader, DCSystem_Drawing.GraphicsUnit.Point);
                else NotSupportProperty("XFontValue", strNativeName);
            }//while
            return instance;
        }
        public static void WriteXFontValue(Utf8JsonWriter jwriter, DCSoft.Drawing.XFontValue instance)
        {
            if (instance == null) throw new System.ArgumentNullException("instance");
            if (jwriter == null) throw new System.ArgumentNullException("jwriter");
            jwriter.WriteStartObject();
            jwriter.WriteBoolean(__Bold, instance.Bold);
            jwriter.WriteBoolean(__Italic, instance.Italic);
            jwriter.WriteString(__Name, instance.Name);
            jwriter.WriteNumber(__Size, instance.Size);
            jwriter.WriteBoolean(__Strikeout, instance.Strikeout);
            jwriter.WriteBoolean(__Underline, instance.Underline);
            { jwriter.WritePropertyName(__Unit); WriteEnumGraphicsUnit(jwriter, instance.Unit); }
            jwriter.WriteEndObject();
        }


        public sealed class JsonConverter_XImageValue : System.Text.Json.Serialization.JsonConverter<DCSoft.Drawing.XImageValue>
        {
            public override bool CanConvert(Type typeToConvert) { return typeToConvert == typeof(DCSoft.Drawing.XImageValue); }
            public override DCSoft.Drawing.XImageValue Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                return ReadXImageValue(ref reader, new DCSoft.Drawing.XImageValue());
            }
            public override void Write(Utf8JsonWriter writer, DCSoft.Drawing.XImageValue value, JsonSerializerOptions options)
            {
                WriteXImageValue(writer, value);
            }
        }
        public static DCSoft.Drawing.XImageValue ReadXImageValue(ref Utf8JsonReader jreader, DCSoft.Drawing.XImageValue instance)
        {
            if (instance == null || jreader.TokenType != JsonTokenType.StartObject) return instance;
            while (jreader.Read() && jreader.TokenType != JsonTokenType.EndObject)
            {
                if (jreader.TokenType != JsonTokenType.PropertyName) continue;
                var strNativeName = jreader.GetString();
                object pName = GetValueReference(strNativeName);
                if (jreader.Read() == false) break;
                if (pName == (object)__ImageDataBase64String) instance.ImageDataBase64String = jreader.GetString();
                else NotSupportProperty("XImageValue", strNativeName);
            }//while
            return instance;
        }
        public static void WriteXImageValue(Utf8JsonWriter jwriter, DCSoft.Drawing.XImageValue instance)
        {
            if (instance == null) throw new System.ArgumentNullException("instance");
            if (jwriter == null) throw new System.ArgumentNullException("jwriter");
            jwriter.WriteStartObject();
            jwriter.WriteString(__ImageDataBase64String, instance.ImageDataBase64String);
            jwriter.WriteEndObject();
        }


    }
}