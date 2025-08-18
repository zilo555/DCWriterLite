using System;
using System.Collections.Generic;
using System.Windows.Forms;
using DCSoft.Drawing;
using DCSoft.Common;

namespace DCSoft.WASM
{
    partial class WriterControlForWASM
    {
        /// <summary>
        /// 判断是否有内容被选择
        /// </summary>
        /// <returns></returns>
        [JSInvokable]
        public bool HasSelection()
        {
            return this._Control.Selection.Length != 0;
        }
        /// <summary>
        /// 执行粘贴操作
        /// </summary>
        /// <param name="copyedDatas">要粘贴的内容</param>
        [JSInvokable]
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public void DoPaste(string[] copyedDatas)
        {
            try
            {
                if (copyedDatas != null && copyedDatas.Length > 0)
                {
                    var vs = WASMUtils.CreateDataDictionary(copyedDatas);
                    var obj = new WASMDataObject(vs);
                    this._Control.GetInnerViewControl().DoPaste(obj, null, true);
                    obj.Clear();
                }
            }
            catch (Exception ex)
            {
                JavaScriptMethods.Tools_ReportException("DoPaste", ex.Message, ex.ToString(), 0);
                return;
            }
        }
        /// <summary>
        /// 执行剪切操作，返回要移动的内容
        /// </summary>
        /// <param name="textOnly">是否只进行纯文本的复制</param>
        /// <param name="showUI">是否显示用户界面</param>
        /// <returns>要移动的内容</returns>
        [JSInvokable]
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public string[] DoCut(bool textOnly, bool showUI)
        {
            try
            {
                if (this._Control.DocumentControler.CanCut())
                {
                    var result = this.DoCopy(textOnly);
                    this._Control.DocumentControler.Delete(showUI);
                    return result;
                }
                return null;
            }
            catch (Exception ex)
            {
                JavaScriptMethods.Tools_ReportException("DoCut", ex.Message, ex.ToString(), 0);
                return null;
            }
        }

        /// <summary>
        /// 执行复制操作，返回要复制的内容
        /// </summary>
        /// <param name="textOnly">是否只进行纯文本的复制</param>
        /// <returns>要复制的内容</returns>
        [JSInvokable]
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public string[] DoCopy(bool textOnly)
        {
            try
            {
                var datas = (WASMDataObject)(this._Control.DocumentControler).WASMCreateSelectionDatas(textOnly);
                var vs = datas?.GetRawDatas();
                if (vs != null)
                {
                    var list = ToHtmlStringList(vs);
                    if (list.Count > 0)
                    {
                        return list.ToArray();
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                JavaScriptMethods.Tools_ReportException("DoCopy", ex.Message, ex.ToString(), 0);
                return null;
            }
        }

        private List<string> ToHtmlStringList(Dictionary<string, object> vs)
        {
            if (vs == null || vs.Count == 0)
            {
                return null;
            }
            var list = new List<string>();
            foreach (var item in vs)
            {
                if (item.Key == System.Windows.Forms.DataFormats.Text
                    || item.Key == System.Windows.Forms.DataFormats.UnicodeText)
                {
                    list.Add("text/plain");
                    list.Add(Convert.ToString(item.Value));
                }
                else if (item.Key == System.Windows.Forms.DataFormats.Bitmap)
                {
                    var bs = ((Bitmap)item.Value).Data;
                    if (FileHeaderHelper.HasBMPHeader(bs))
                    {
                        list.Add("image/bmp");
                    }
                    else if (FileHeaderHelper.HasPNGHeader(bs))
                    {
                        list.Add("image/png");
                    }
                    else if (FileHeaderHelper.HasJpegHeader(bs))
                    {
                        list.Add("image/jpeg");
                    }
                    else if (FileHeaderHelper.HasGIFHeader(bs))
                    {
                        list.Add("image/gif");
                    }
                    list.Add(XImageValue.StaticGetEmitImageSource(bs));
                }
                else
                {
                    list.Add(item.Key);
                    list.Add(Convert.ToString(item.Value));
                }
            }//foreach
            return list;
        }

        private class WASMDataObject : System.Windows.Forms.IDataObject
        {
            private Dictionary<string, object> _Datas = null;
            internal Dictionary<string, object> GetRawDatas()
            {
                return this._Datas;
            }
            public WASMDataObject(Dictionary<string, object> datas)
            {
                if (datas == null)
                {
                    throw new ArgumentNullException("datas");
                }
                this._Datas = datas;
            }
            public void Clear()
            {
                if (this._Datas != null)
                {
                    this._Datas.Clear();
                    this._Datas = null;
                }
            }
            //public string ToStringArray()
            //{
            //    var list = new List<string>();
            //    return list.ToString();
            //}
            internal Dictionary<string, object> ThisData
            {
                get
                {
                    return this._Datas;
                }
            }

            public object GetData(string format)
            {
                try
                {
                    if (this._Datas.ContainsKey(format))
                    {
                        return this._Datas[format];
                    }
                    if (this._Datas.ContainsKey(format.ToLower()))
                    {
                        return this._Datas[format.ToLower()];
                    }
                    else
                    {
                        return null;
                    }
                }
                catch (Exception ex)
                {
                    JavaScriptMethods.Tools_ReportException("GetData", ex.Message, ex.ToString(), 0);
                    return null;
                }
            }

            //public object GetData(string format, bool autoConvert)
            //{
            //    return GetData(format);
            //}

            //public object GetData(Type format)
            //{
            //    return GetData(GetFormatName(format));
            //}

            public bool GetDataPresent(string format)
            {
                try
                {
                    return this._Datas.ContainsKey(format) 
                        || this._Datas.ContainsKey(format.ToLower());
                }
                catch (Exception ex)
                {
                    JavaScriptMethods.Tools_ReportException("GetDataPresent", ex.Message, ex.ToString(), 0);
                    return false;
                }
            }

            //public bool GetDataPresent(string format, bool autoConvert)
            //{
            //    return this.ThisData.ContainsKey(format);
            //}

            //public bool GetDataPresent(Type format)
            //{
            //    return this.ThisData.ContainsKey(GetFormatName(format));
            //}
            //private string GetFormatName(Type format)
            //{
            //    if (format == typeof(string))
            //    {
            //        return DataFormats.Text;
            //    }
            //    return format.Name;
            //}
            public string[] GetFormats()
            {
                try
                {
                    return new List<string>(this._Datas.Keys).ToArray();
                }
                catch (Exception ex)
                {
                    JavaScriptMethods.Tools_ReportException("GetFormats", ex.Message, ex.ToString(), 0);
                    return null;
                }
            }

            //public string[] GetFormats(bool autoConvert)
            //{
            //    return new List<string>(this.ThisData.Keys).ToArray();
            //}

            public void SetData(object data)
            {
                try
                {
                    this._Datas[DataFormats.Text] = data;
                }
                catch (Exception ex)
                {
                    JavaScriptMethods.Tools_ReportException("SetData", ex.Message, ex.ToString(), 0);
                    return;
                }
            }

            //public void SetData(string format, bool autoConvert, object data)
            //{
            //    this.ThisData[format] = data;
            //}

            public void SetData(string format, object data)
            {
                try
                {
                    this._Datas[format] = data;
                }
                catch (Exception ex)
                {
                    JavaScriptMethods.Tools_ReportException("SetData", ex.Message, ex.ToString(), 0);
                    return;
                }
            }

            public void SetTitle( string strTitle)
            {
                this.SetData("DataTitle", strTitle);
            }
            //public void SetData(Type format, object data)
            //{
            //    this.ThisData[GetFormatName(format)] = data;
            //}
        }
    }
}
