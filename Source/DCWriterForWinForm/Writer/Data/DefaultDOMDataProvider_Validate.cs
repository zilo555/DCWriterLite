using System;
using System.Collections.Generic;
using System.Text;
using DCSoft.Writer.Dom;
using DCSoft.Common;
using DCSoft.WASM;
using System.ComponentModel;

namespace DCSoft.Writer.Data
{
    // 用于文档内容数据校验的代码

    partial class DefaultDOMDataProvider
    {

        /// <summary>
        /// 对整个文档的输入域进行数据校验
        /// </summary>
        /// <returns>校验信息列表，如果返回空列表则校验全部通过</returns>
        public static ValueValidateResultList ValueValidate_Document(DomDocument document, bool getLastResultOnly)
        {
            var opts = document.GetDocumentOptions();
            if (opts.EditOptions.ValueValidateMode == DocumentValueValidateMode.None)
            {
                // 禁止数据校验
                return null;
            }
            ValueValidateResultList result = new ValueValidateResultList();
            // 检查单选框复选框的必填项设置
            DomElementList chks = document.GetElementsBySpecifyType<DomCheckBoxElementBase>();
            if (chks != null && chks.Count > 0)
            {
                List<string> handledNames = new List<string>();
                foreach (DomCheckBoxElementBase chk in chks)
                {
                    if (chk.Requried && string.IsNullOrEmpty(chk.Name) == false
                        && handledNames.Contains(chk.Name) == false && chk.Visible)
                    {
                        handledNames.Add(chk.Name);
                        DomElementList chks2 = chk.GetElementsInSameGroup();
                        bool flag = false;
                        foreach (DomCheckBoxElementBase chk2 in chks2)
                        {
                            if (chk2.Checked)
                            {
                                flag = true;
                                break;
                            }
                        }
                        if (flag == false)
                        {
                            ValueValidateResult re = new ValueValidateResult();
                            re.Element = chk;
                            re.Level = ValueValidateLevel.Error;
                            re.Message = string.Format(DCSR.CheckRequired_Name, chk.Name);
                            re.Type = ValueValidateResultTypes.ValueValidate;
                            result.Add(re);
                        }
                    }
                }//foreach
            }
            DomElementList fields = document.GetElementsBySpecifyType<DomContainerElement>();// .GetElementsByType(typeof(XTextContainerElement));
            if (fields.Count > 0)
            {
                foreach (DomContainerElement field in fields.FastForEach())
                {
                    DomElement p = field;
                    // 判断是否忽略
                    bool ignore = false;
                    while (p != null && (p is DomDocumentContentElement) == false)
                    {
                        if (p.Visible == false)
                        {
                            // 隐藏的元素
                            ignore = true;
                            break;
                        }
                        p = p.Parent;
                    }//while
                    if (ignore)
                    {
                        // 被忽略的元素不进行验证
                        continue;
                    }
                    // 执行容器元素的内容校验
                    ValueValidateResult re = getLastResultOnly ? field.LastValidateResult : Validating_XTextContainerElement(field, false);// field.Validating(false);
                    if (re != null)
                    {
                        result.Add(re);
                    }
                }//foreach
            }//if
            if (result.Count > 1)
            {
                result.SoryByDOMLevel();
            }
            return result;
        }

        /// <summary>
        /// 进行容器元素数据验证
        /// </summary>
        /// <param name="container">容器元素对象</param>
        /// <param name="loadingDocument">是否正在加载文档中</param>
        public static ValueValidateResult Validating_XTextContainerElement(
            DomContainerElement container,
            bool loadingDocument)
        {
            if (container == null)
            {
                return null;
            }
            DomDocument doc = container.OwnerDocument;
            if (doc == null)
            {
                return null;
            }
            var opts = doc.GetDocumentOptions();
            if (opts.EditOptions.ValueValidateMode
                == DocumentValueValidateMode.None)
            {
                // 禁止数据校验 
                return null;
            }
            var hasVResult = container.LastValidateResult != null;
            if (opts.BehaviorOptions.EnableElementEvents)
            {
                ElementValidatingEventArgs args2 = new ElementValidatingEventArgs(container);
                args2.ResultState = ElementValidatingState.Pass;
                doc.OnEventElementValidating(args2);
                if (args2.Handled)
                {
                    // 事件被处理了
                    switch (args2.ResultState)
                    {
                        case ElementValidatingState.Success:
                            // 校验成功
                            container.SetLastValidateResult(null);
                            if (args2.Cancel == false)
                            {
                                container.OnValidated();
                            }
                            return null;
                        case ElementValidatingState.Pass:
                            // 校验通过
                            if (args2.Cancel == false)
                            {
                                container.OnValidated();
                            }
                            break;
                        case ElementValidatingState.Fail:
                            // 校验失败
                            var resultInfo = new ValueValidateResult();
                            resultInfo.Element = container;
                            resultInfo.Type = ValueValidateResultTypes.ValueValidate;
                            resultInfo.Level = args2.ResultLevel;
                            resultInfo.Message = args2.Message;
                            container.SetLastValidateResult(resultInfo);
                            if (args2.Cancel == false)
                            {
                                container.OnValidated();
                            }
                            container.InvalidateHighlightInfo();
                            //UpdateToolTipForValueValidateResult(null);
                            return container.LastValidateResult;
                    }//switch
                }//if
            }
            ValueValidateStyle valStyle = container.ValidateStyle;// .RuntimeSupportValidateStyle() ? container.ValidateStyle : null;
            if (valStyle != null
                && valStyle.IsEmpty == false)
            {
                valStyle.ContentVersion = container.ContentVersion;
                //valStyle.Value = container.Text;

                string resultKeyWords = "";//校验出来的违禁词

                //wyc20240711:针对内含输入域的容器，取文本时排除掉内部输入域的各种背景文字边框内容DUWRITER5_0-3091
                string validatetext = container.Text;
                if (container is DomInputFieldElement &&
                    valStyle.Required == true &&
                    container.GetFirstChild<DomInputFieldElement>() != null)
                {
                    GetTextArgs gta = new GetTextArgs();
                    gta.IncludeBackgroundText = false;
                    gta.IncludeHiddenText = false;
                    validatetext = container.GetText(gta);
                }
                ////////////////////////////////////////////////////////////////////////////////////////////////

                bool result = ValidateValue(valStyle, container.Elements, validatetext, ref resultKeyWords);// valStyle.Validate();
                if (result)
                {
                    container.SetLastValidateResult(null);
                }
                else
                {
                    var info = new ValueValidateResult();
                    info.Element = container;
                    info.Level = valStyle.Level;
                    string name = NameForValidateResult(container);
                    if (string.IsNullOrEmpty(name))
                    {
                        name = container.ID;
                    }
                    if (string.IsNullOrEmpty(name))
                    {
                        info.Message = valStyle.Message;
                    }
                    else
                    {
                        info.Message = name + ":" + valStyle.Message;
                    }
                    info.Type = ValueValidateResultTypes.ValueValidate;
                    container.SetLastValidateResult(info);
                }
                if (loadingDocument == false)
                {
                    container.InvalidateView();
                }
            }
            else
            {
                container.SetLastValidateResult(null);
            }
#if ! RELEASE
            if (doc.GetDocumentBehaviorOptions().DebugMode
                && container.LastValidateResult != null)
            {
                if (string.IsNullOrEmpty(container.LastValidateResult.Message) == false)
                {
                    string msg = string.Format(
                            DCSR.ValueInvalidate_Source_Value_Result,
                            container.ToDebugString(),
                            container.Text,
                            container.LastValidateResult.Message);
                    DCConsole.Default.WriteLine(msg);
                }
            }
#endif
            container.OnValidated();
            if (hasVResult != (container.LastValidateResult != null))
            {
                container.InvalidateHighlightInfo();
            }
            //UpdateToolTipForValueValidateResult(null);
            return container.LastValidateResult;
        }

        /// <summary>
        /// 数据校验结果中使用的名称
        /// </summary>
        private static string NameForValidateResult(DomElement element)
        {
            if (element is DomInputFieldElementBase)
            {
                var field = (DomInputFieldElementBase)element;
                string name = field.ID;
                if (string.IsNullOrEmpty(name))
                {
                    name = field.Name;
                }
                if (string.IsNullOrEmpty(name))
                {
                    name = field.RuntimeBackgroundText;
                }
                return name;
            }
            else
            {
                return element.ID;
            }
        }
        //2020-03-02 李应祥修改
        /// <summary>
        /// 进行数据验证
        /// </summary>
        /// <param name="style">校验对象</param>
        /// <param name="ele">容器子元素</param>
        /// <param name="_Value">文本值</param>
        /// <returns>验证是否通过</returns>
        internal static bool ValidateValue(ValueValidateStyle style, DomElementList eles, object _Value, ref string resultKeyWords)
        {
            resultKeyWords = "";
            if (style == null)
            {
                return true;
            }
            style.RequiredInvalidateFlag = false;
            style.Message = null;
            bool isNullValue = false;

            if (style.Required)
            {
                if (eles.Count > 0)//有子元素
                {
                    isNullValue = ValidateValueEditList(eles, _Value, isNullValue);

                }
                // 执行数据是否为空的判断 isNullValue 为true是没有内容
                if (eles.Count == 0 || isNullValue == true)
                {
                    style.Message = DCSR.ForbidEmpty;
                    style.RequiredInvalidateFlag = true;
                    return false;
                }
                //wyc20250312:DUWRITER5_0-4237
                if (WriterControlForWASM.bValidateBlankIncludingNewLine == true &&
                    eles.Count > 0 && _Value is string && _Value.ToString().Trim('\n') == "")
                {

                    style.Message = DCSR.ForbidEmpty;
                    style.RequiredInvalidateFlag = true;
                    return false;
                }
            }
            switch (style.ValueType)
            {
                case ValueTypeStyle.Text:
                    {
                        string txt = Convert.ToString(_Value);
                        if (style.CheckMaxValue && style.MaxLength > 0)
                        {
                            if (txt != null && GetTextLength(style, txt) > style.MaxLength)
                            {
                                style.Message = string.Format(
                                    DCSR.MoreThanMaxLength_Length,
                                    style.MaxLength);
                                return false;
                            }
                        }
                        if (style.CheckMinValue && style.MinLength > 0)
                        {
                            if (txt != null && GetTextLength(style, txt) < style.MinLength)
                            {
                                style.Message = string.Format(
                                    DCSR.LessThanMinLength_Length,
                                    style.MinLength);
                                return false;
                            }
                        }
                    }
                    break;
                case ValueTypeStyle.Numeric:
                case ValueTypeStyle.Integer:
                    {
                        double v = DoubleNaN.NaN;
                        if (_Value.ToString() == "")
                        {
                            return true;
                        }
                        if (_Value is Int32 || _Value is float || _Value is double)
                        {
                            v = (double)_Value;
                        }
                        else
                        {
                            bool result = false;
                            if (style.ValueType == ValueTypeStyle.Numeric)
                            {
                                result = double.TryParse(Convert.ToString(_Value), out v);
                                //2022-09-13 liyingxiang 修改数字校验前后带有空格校验通过问题 DCWRITER-3934
                                if (result == true)
                                {
                                    if (_Value.ToString().Trim() != _Value.ToString())//表示字符串前后没有空格
                                    {
                                        result = false;
                                    }
                                    //result = false;
                                }
                                if (result == false)
                                {
                                    style.Message = DCSR.MustNumeric;
                                    return false;
                                }
                            }
                            else
                            {
                                int v2 = int.MinValue;
                                result = int.TryParse(Convert.ToString(_Value), out v2);
                                //2022-11-3 liyingxiang 修改数字校验前后带有空格校验通过问题 DCWRITER-3934
                                if (result == true)
                                {
                                    if (_Value.ToString().Trim() != _Value.ToString())//表示字符串前后没有空格
                                    {
                                        result = false;
                                    }
                                    //result = false;
                                }
                                if (result)
                                {
                                    v = v2;
                                }
                                else
                                {
                                    style.Message = DCSR.MustInteger;
                                    return false;
                                }
                            }
                        }
                        if (style.MaxValue != style.MinValue)
                        {
                            if (style.CheckMaxValue && DoubleNaN.IsNaN(style.MaxValue) == false)
                            {
                                if (v > style.MaxValue)
                                {
                                    style.Message = string.Format(
                                        DCSR.MoreThanMaxValue_Value,
                                        style.MaxValue);
                                    return false;
                                }
                            }
                            if (style.CheckMinValue && DoubleNaN.IsNaN(style.MinValue) == false)
                            {
                                if (v < style.MinValue)
                                {
                                    style.Message = string.Format(
                                        DCSR.LessThanMinValue_Value,
                                        style.MinValue);
                                    return false;
                                }
                            }
                        }
                        if (style.CheckDecimalDigits)
                        {
                            string value = Convert.ToString(_Value);
                            if (value.Contains(".")) //是小数进行判断
                            {
                                int digitsLength = value.Length - value.IndexOf('.') - 1;
                                if (digitsLength > style.MaxDecimalDigits)
                                {
                                    style.Message = string.Format(
                                        DCSR.MoreThanMaxDemicalDigits,
                                        style.MaxDecimalDigits);
                                    return false;
                                }
                            }
                        }
                    }
                    break;
                case ValueTypeStyle.Date:
                    {
                        DateTime dtm = DateTime.MinValue;
                        if (Convert.ToString(_Value) == "")
                        {
                            return true;
                        }
                        bool result = DateTime.TryParse(Convert.ToString(_Value), out dtm);
                        if (result == false)
                        {
                            style.Message = DCSR.MustDateType;
                            return false;
                        }
                        dtm = dtm.Date;
                        if (style.CheckMaxValue)
                        {
                            DateTime max = style.DateTimeMaxValue.Date;
                            if (dtm > max)
                            {
                                style.Message = string.Format(
                                    DCSR.MoreThanMaxValue_Value,
                                    FormatUtils.ToYYYY_MM_DD(max));
                                return false;
                            }
                        }
                        if (style.CheckMinValue)
                        {
                            DateTime min = style.DateTimeMinValue.Date;
                            if (dtm < min)
                            {
                                style.Message = string.Format(
                                    DCSR.LessThanMinValue_Value,
                                    FormatUtils.ToYYYY_MM_DD(min));
                                return false;
                            }
                        }
                    }
                    break;
                case ValueTypeStyle.Time:
                    {
                        TimeSpan dtm = TimeSpan.Zero;
                        if (Convert.ToString(_Value) == "")
                        {
                            return true;
                        }
                        bool result = TimeSpan.TryParse(Convert.ToString(_Value), out dtm);
                        if (result == false)
                        {
                            style.Message = DCSR.MustTimeType;
                            return false;
                        }
                        if (style.CheckMaxValue)
                        {
                            TimeSpan max = style.DateTimeMaxValue.TimeOfDay;
                            max = new TimeSpan(max.Hours, max.Minutes, max.Seconds);
                            if (dtm > max)
                            {
                                style.Message = string.Format(
                                    DCSR.MoreThanMaxValue_Value,
                                    max);
                                return false;
                            }
                        }
                        if (style.CheckMinValue)
                        {
                            TimeSpan min = style.DateTimeMinValue.TimeOfDay;
                            min = new TimeSpan(min.Hours, min.Minutes, min.Seconds);
                            if (dtm < min)
                            {
                                style.Message = string.Format(
                                    DCSR.LessThanMinValue_Value,
                                    min);
                                return false;
                            }
                        }
                    }
                    break;
                case ValueTypeStyle.DateTime:
                    {
                        DateTime dtm = DateTime.MinValue;
                        if (Convert.ToString(_Value) == "")
                        {
                            return true;
                        }
                        bool result = DateTime.TryParse(Convert.ToString(_Value), out dtm);
                        if (result == false)
                        {
                            style.Message = DCSR.MustDateTimeType;
                            return false;
                        }
                        if (style.CheckMaxValue)
                        {
                            DateTime max = style.DateTimeMaxValue;
                            if (dtm > max)
                            {
                                style.Message = string.Format(
                                    DCSR.MoreThanMaxValue_Value,
                                    max.ToString(WriterConst.Format_YYYY_MM_DD_HH_MM_SS));
                                return false;
                            }
                        }
                        if (style.CheckMinValue)
                        {
                            DateTime min = style.DateTimeMinValue;
                            if (dtm < min)
                            {
                                style.Message = string.Format(
                                    DCSR.LessThanMinValue_Value,
                                    min.ToString(WriterConst.Format_YYYY_MM_DD_HH_MM_SS));
                                return false;
                            }
                        }
                    }
                    break;
            }//switch
            return true;
        }
        //2020-03-04  李应祥添加
        /// <summary>
        /// 把输入域里面的子元素提取出来一个方法来进行校验
        /// </summary>
        /// <param name="eles"></param>
        /// <param name="_Value"></param>
        /// <param name="isNullValue"></param>
        /// <returns></returns>
        private static bool ValidateValueEditList(DomElementList eles, object _Value, bool isNullValue)
        {
            if (_Value == null || DBNull.Value.Equals(_Value) || _Value.ToString().Length == 0)
            {
                if (eles != null && eles.Count > 0)
                {
                    foreach (var e2 in eles.FastForEach())
                    {
                        if (e2 is DomObjectElement)
                        {
                            isNullValue = false;
                            break;
                        }

                        //wyc20240711:针对内含输入域的容器，取文本时排除掉内部输入域的各种背景文字边框内容DUWRITER5_0-3091
                        if (e2 is DomContainerElement)
                        {
                            DomContainerElement input = e2 as DomContainerElement;
                            string validatetext = input.Text;
                            GetTextArgs gta = new GetTextArgs();
                            gta.IncludeBackgroundText = false;
                            gta.IncludeHiddenText = false;
                            validatetext = input.GetText(gta);

                            isNullValue = ValidateValueEditList(input.Elements, validatetext, isNullValue);
                            if (isNullValue == false)
                            {
                                break;
                            }
                        }
                        //////////////////////////////////////////////////////////////////////////////////////////////
                    }
                }
                if (isNullValue)
                {
                    return true;
                }
            }

            foreach (DomElement item in eles.FastForEach())//遍历子元素
            {

                string txt = Convert.ToString(_Value);
                if (string.IsNullOrEmpty(txt) == false)//Text
                {
                    isNullValue = false;
                    break;
                }
                if (item is DomInputFieldElement)
                {
                    DomInputFieldElement input = item as DomInputFieldElement;
                    if (input.Elements.Count > 0)
                    {
                        //wyc20240711:针对内含输入域的容器，取文本时排除掉内部输入域的各种背景文字边框内容DUWRITER5_0-3091
                        string validatetext = input.Text;
                        GetTextArgs gta = new GetTextArgs();
                        gta.IncludeBackgroundText = false;
                        gta.IncludeHiddenText = false;
                        validatetext = input.GetText(gta);
                        //////////////////////////////////////////////////////////////////////////////////////////////

                        isNullValue = ValidateValueEditList(input.Elements, validatetext, isNullValue);
                        break;
                    }
                }
                if (item is DomImageElement)//图片
                {
                    isNullValue = false;//有子元素
                    break;
                }
            }

            return isNullValue;
        }

        private static Encoding _EncodingForGetBytes = Encoding.Default;
        /// <summary>
        /// 为获得字节数使用的文本编码格式对象
        /// </summary>
        public static Encoding EncodingForGetBytes
        {
            get
            {
                if (_EncodingForGetBytes == null)
                {
                    _EncodingForGetBytes = Encoding.Default;
                }
                return _EncodingForGetBytes;
            }
        }

        private static int GetTextLength(ValueValidateStyle style, string txt)
        {
            if (string.IsNullOrEmpty(txt))
            {
                return 0;
            }
            if (style.BinaryLength)
            {
                return EncodingForGetBytes.GetByteCount(txt);
            }
            else
            {
                return txt.Length;
            }
        }


        /// <summary>
        /// 单独对容器元素进行校验
        /// </summary>
        /// <param name="rootElement"></param>
        /// <returns></returns>
        public static ValueValidateResultList ValueValidateInContainerElement(DomContainerElement rootElement)
        {
            var opts = rootElement.OwnerDocument.GetDocumentOptions();
            ValueValidateResultList result = new ValueValidateResultList();
            List<string> gExcludeKeywords = new List<string>();

            // 检查单选框复选框的必填项设置
            DomElementList chks = rootElement.GetElementsBySpecifyType<DomCheckBoxElementBase>();
            if (chks != null && chks.Count > 0)
            {
                List<string> handledNames = new List<string>();
                foreach (DomCheckBoxElementBase chk in chks)
                {
                    if (chk.Requried && string.IsNullOrEmpty(chk.Name) == false
                        && handledNames.Contains(chk.Name) == false && chk.Visible)
                    {
                        handledNames.Add(chk.Name);
                        DomElementList chks2 = chk.GetElementsInSameGroup();
                        bool flag = false;
                        foreach (DomCheckBoxElementBase chk2 in chks2)
                        {
                            if (chk2.Checked)
                            {
                                flag = true;
                                break;
                            }
                        }
                        if (flag == false)
                        {
                            ValueValidateResult re = new ValueValidateResult();
                            re.Element = chk;
                            re.Level = ValueValidateLevel.Error;
                            re.Message = string.Format(DCSR.CheckRequired_Name, chk.Name);
                            re.Type = ValueValidateResultTypes.ValueValidate;
                            result.Add(re);
                        }
                    }
                }//foreach
            }
            DomElementList fields = rootElement.GetElementsBySpecifyType<DomContainerElement>();// .GetElementsByType(typeof(XTextContainerElement));
            if (fields.Count > 0)
            {
                foreach (DomContainerElement field in fields.FastForEach())
                {
                    DomElement p = field;
                    // 判断是否忽略
                    bool ignore = false;
                    while (p != null && (p is DomDocumentContentElement) == false)
                    {
                        if (p.Visible == false)
                        {
                            // 隐藏的元素
                            ignore = true;
                            break;
                        }
                        p = p.Parent;
                    }//while
                    if (ignore)
                    {
                        // 被忽略的元素不进行验证
                        continue;
                    }
                    // 执行容器元素的内容校验
                    ValueValidateResult re = Validating_XTextContainerElement(field, false);// field.Validating(false);
                    if (re != null)
                    {
                        result.Add(re);
                    }
                }//foreach
            }//if

            //容器本身是输入域
            if (rootElement is DomInputFieldElement)
            {
                DomContainerElement field = rootElement;
                DomElement p = field;
                // 判断是否忽略
                bool ignore = false;
                while (p != null && (p is DomDocumentContentElement) == false)
                {
                    if (p.Visible == false)
                    {
                        // 隐藏的元素
                        ignore = true;
                        break;
                    }
                    p = p.Parent;
                }//while
                if (ignore)
                {

                }
                else
                {
                    // 执行容器元素的内容校验
                    ValueValidateResult re = Validating_XTextContainerElement(field, false);// field.Validating(false);
                    if (re != null)
                    {
                        result.Add(re);
                    }
                }
            }

            if (result.Count > 1)
            {
                result.SoryByDOMLevel();
            }
            return result;
        }

    }
}