//  
using DCSoft.Writer.Dom;
using DCSoft.Writer.Controls;
using DCSoft.Common;
using DCSoft.Printing;
using DCSoft.Writer;

namespace DCSoft.WASM
{
    // 绘图相关代码
    partial class WriterControlForWASM
    {
        /// <summary>
        /// 获得标准图标图片数据
        /// </summary>
        /// <returns>数据字符串数值</returns>
        [JSInvokable]
        public static string[] GetStandardImageData()
        {
            var result = new string[(int)DCStdImageKey.Max];
            for (var iCount = (int)DCStdImageKey.None; iCount < result.Length; iCount++)
            {
                var bmp = DCStdImageList.GetBitmap((DCStdImageKey)iCount);
                if (bmp != null)
                {
                    result[iCount] = bmp.ToDataUrl();// XImageValue.StaticGetEmitImageSource(bmp.Data);
                }
            }
            // Graphics.UseStandarImageList = false;
            return result;
        }
        /// <summary>
        /// 基础性的缩放比率
        /// </summary>
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public float WASMBaseZoomRate
        {
            [JSInvokable]
            get
            {
                return this._Control.WASMBaseZoomRate;
            }
            [JSInvokable]
            set
            {
                this._Control.WASMBaseZoomRate = value;
            }
        }



        /// <summary>
        /// 调用JS方法 WriterControl_Paint.NeedUpdateView()
        /// </summary>
        public void NeedUpdateView()
        {
            try
            {
                JavaScriptMethods.Paint_NeedUpdateView(this._ContainerElementID, true);
            }
            catch (Exception ex)
            {
                JavaScriptMethods.Tools_ReportException("NeedUpdateView", ex.Message, ex.ToString(), 0);
                return;
            }
        }
        [JSInvokable]
        public string GetPageClientSelectionBounds(int pageIndex )
        {
            return this._Control.GetInnerViewControl().GetPageClientSelectionBounds(pageIndex);
        }
        /// <summary>
        /// 获得要绘制的图形数据
        /// </summary>
        /// <param name="strPageIndexs">可选的页码列表</param>
        /// <returns>图形数据</returns>
        [JSInvokable]
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public byte[] GetInvalidateViewData(string strPageIndexs)
        {
            try
            {
                _Current = this;
                return this._Control.GetInnerViewControl().GetInvalidateViewData(strPageIndexs);
            }
            catch (Exception ex)
            {
                JavaScriptMethods.Tools_ReportException("GetInvalidateViewData", ex.Message, ex.ToString(), 0);
                return null;
            }
            finally
            {
                _Current = null;
            }
        }
        /// <summary>
        /// 判断是否存在无效区域，需要重新绘制。
        /// </summary>
        /// <returns>是否存在无效区域</returns>
        [JSInvokable]
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public bool HasInvalidateView()
        {
            return this._Control.GetInnerViewControl().HasInvalidateView();
        }

        /// <summary>
        /// 视图缩放比率
        /// </summary>
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public float ZoomRate
        {
            [JSInvokable]
            get
            {
                return this._Control.WASMZoomRate;
            }
            [JSInvokable]
            set
            {
                if (value < 0.1f)
                {
                    this._Control.WASMZoomRate = 0.1f;
                }
                else if (value > 5)
                {
                    this._Control.WASMZoomRate = 5;
                }
                else
                {
                    this._Control.WASMZoomRate = value;
                }
            }
        }
        /// <summary>
        /// 设置视图缩放比率
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        [JSInvokable]
        public float SetViewZoomRate(float v)
        {
            try
            {
                if (v < 0.1f)
                {
                    this._Control.WASMZoomRate = 0.1f;
                }
                else if (v > 5)
                {
                    this._Control.WASMZoomRate = 5;
                }
                else
                {
                    this._Control.WASMZoomRate = v;
                }
                return this._Control.WASMZoomRate;
            }
            catch (Exception ex)
            {
                JavaScriptMethods.Tools_ReportException("SetViewZoomRate", ex.Message, ex.ToString(), 0);
                return this._Control.WASMZoomRate;
            }
        }

        [JSInvokable]
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public byte[] RenderPageContent(int intPageIndex)
        {
            try
            {
                byte[] result = null;
                _Current = this;
                    result = this._Control.WASMPaintPage(intPageIndex);
                return result;
            }
            catch (Exception ex)
            {
                JavaScriptMethods.Tools_ReportException("RenderPageContent", ex.Message, ex.ToString(), 0);
                return null;
            }
            finally
            {
                _Current = null;
            }
        }

        #region  标尺相关代码
        [JSInvokable]
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public byte[] PaintRuleControl(string ruleName, float positionOffset, int viewSize)
        {
            try
            {
                return this._Control.PaintRuleControl(ruleName, (int)positionOffset, viewSize);
            }
            catch (Exception ex)
            {
                JavaScriptMethods.Tools_ReportException("PaintRuleControl", ex.Message, ex.ToString(), 0);
                return null;
            }
        }

        /// <summary>
        /// 设置标尺的可见性
        /// </summary>
        /// <param name="bolVisible">可见性</param>
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public void SetRuleVisible(bool bolVisible)
        {
            try
            {
                JavaScriptMethods.Rule_SetRuleVisible(this._ContainerElementID, bolVisible);
                //this.JS_InvokeVoidInstance(DCWriterModule.JSNAME_WriterControl_Rule_SetRuleVisible, bolVisible);
            }
            catch (Exception ex)
            {
                JavaScriptMethods.Tools_ReportException("SetRuleVisible", ex.Message, ex.ToString(), 0);
                return;
            }
        }
        /// <summary>
        /// 声明标尺控件视图无效，需要重新绘制
        /// </summary>
        /// <param name="ctlName"></param>
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public void RuleInvalidateView(string ctlName)
        {
            try
            {
                JavaScriptMethods.Rule_InvalidateView(this._ContainerElementID, ctlName);
                //this.JS_InvokeVoidInstance(DCWriterModule.JSNAME_WriterControl_Rule_InvalidateView, ctlName);
            }
            catch (Exception ex)
            {
                JavaScriptMethods.Tools_ReportException("RuleInvalidateView", ex.Message, ex.ToString(), 0);
                return;
            }
        }
#endregion
    }
}
