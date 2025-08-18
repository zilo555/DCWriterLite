using DCSoft.Printing;
using DCSoft.Writer.Dom;
using DCSoft.Writer;
using System.Collections.Generic;
using System.Text.Json;

namespace DCSoft.WASM
{

    /// <summary>
    /// 打印任务参数
    /// </summary>
    public class MyPrintTaskOptions
    {
        public MyPrintTaskOptions(System.Text.Json.JsonElement inputJson, WriterControlForWASM ctlForWASM)
        {
            if (inputJson.ValueKind != JsonValueKind.Object)
            {
                this._IsEmpty = true;
                return;
            }
            foreach (System.Text.Json.JsonProperty property in inputJson.EnumerateObject())
            {
                var name = property.Name;
                if (name == null)
                {
                    continue;
                }
                name = name.ToLower().Trim();
                switch (name)
                {
                    case "printHeaderfooteratfirstpage":
                        {
                            var pv = property.Value;
                            if (pv.ValueKind == System.Text.Json.JsonValueKind.True)
                            {
                                this.PrintHeaderFooterAtFirstPage = DCBooleanValueHasDefault.True;
                                this._IsEmpty = false;
                            }
                            else if (pv.ValueKind == System.Text.Json.JsonValueKind.False)
                            {
                                this.PrintHeaderFooterAtFirstPage = DCBooleanValueHasDefault.False;
                                this._IsEmpty = false;
                            }
                        }
                        break;
                    case "printrange":
                        {
                            this.PrintRange = property.ConvertToEnum<PrintRange>(PrintRange.AllPages);
                            this._IsEmpty = false;
                        }
                        break;
                    case "printmode":
                        {
                            this.PrintMode = property.ConvertToEnum<DCPrintMode>(DCPrintMode.Normal);
                            this._IsEmpty = false;
                        }
                        break;
                    case "printtablecellborder": this.PrintTableCellBorder = property.ConvertToBoolean(true); break;
                    case "collate": this.Collate = property.ConvertToBoolean(false); this._IsEmpty = false; break;
                    case "copies": this.Copies = property.ConvertToInt32(1); this._IsEmpty = false; break;
                    case "frompage": this.FromPage = property.ConvertToInt32(-1); this._IsEmpty = false; break;
                    case "topage": this.ToPage = property.ConvertToInt32(-1); this._IsEmpty = false; break;
                    case "specifypageindexs": this.SpecifyPageIndexs = property.Value.GetString(); this._IsEmpty = false; break;
                }
            }
        }

        public void Dispose()
        {
            this.SpecifyPageIndexs = null;
        }
        private bool _IsEmpty = true;
        public bool IsEmpty
        {
            get
            {
                return this._IsEmpty;
            }
        }
        private bool _PrintTableCellBorder = true;
        /// <summary>
        /// 是否打印表格单元格边框
        /// </summary>
        public bool PrintTableCellBorder
        {
            get { return this._PrintTableCellBorder; }
            set { this._PrintTableCellBorder = value; }
        }
        

        /// <summary>
        /// 打印范围
        /// </summary>
        public PrintRange PrintRange = PrintRange.AllPages;
        /// <summary>
        /// 打印模式
        /// </summary>
        public DCSoft.Printing.DCPrintMode PrintMode = Printing.DCPrintMode.Normal;
        /// <summary>
        /// 是否为逐份打印
        /// </summary>
        public bool Collate = false;
        /// <summary>
        /// 打印份数
        /// </summary>
        public int Copies = 1;
        /// <summary>
        /// 打印开始页码
        /// </summary>
        public int FromPage = -1;
        /// <summary>
        /// 打印结束页码
        /// </summary>
        public int ToPage = -1;
        /// <summary>
        /// 指定要打印的页码
        /// </summary>
        public string SpecifyPageIndexs = null;

        /// <summary>
        /// 是否在首页打印页眉页脚
        /// </summary>
        public DCBooleanValueHasDefault PrintHeaderFooterAtFirstPage = DCBooleanValueHasDefault.Default;
        public DCSoft.Printing.DCPrintDocumentOptions CreateStdOptions()
        {
            try
            {
                var result = new DCSoft.Printing.DCPrintDocumentOptions();
                result.PrintRange = this.PrintRange;

                return result;
            }
            catch (Exception ex)
            {
                JavaScriptMethods.Tools_ReportException("CreateStdOptions", ex.Message, ex.ToString(), 0);
                return null;
            }
        }
        /// <summary>
        /// 解析指定页码范围的字符串，字符串中的页码源自用户界面，因此是从1开始计算的。
        /// </summary>
        /// <param name="strPageIndexString">页码字符串</param>
        /// <param name="list">页码列表，里面的数字是从0开始计算的。</param>
        /// <param name="totalPageCount">总页数</param>
        /// <returns>添加的页码数量</returns>
        public static int ParseSpecifyPageIndexString(string strPageIndexString, List<int> list, int totalPageCount)
        {
            int addCount = 0;
            if (strPageIndexString != null && strPageIndexString.Length > 0)
            {
                // 指定要打印的页码，这个页码字符串来自用户界面，因此是从1开始计算的
                var items = strPageIndexString.Split(',');
                var pindex = 0;
                foreach (var item in items)
                {
                    if (item.IndexOf('-') > 0)
                    {
                        var items2 = item.Split('-');
                        var n1 = 0;
                        var n2 = 0;
                        if (int.TryParse(items2[0], out n1) && int.TryParse(items2[1], out n2))
                        {
                            for (var pi = Math.Max(1, n1); pi <= Math.Min(n2, totalPageCount); pi++)
                            {
                                list.Add(pi - 1);
                                addCount++;
                            }
                        }
                    }
                    else if (int.TryParse(item, out pindex) && pindex >= 1 && pindex <= totalPageCount)
                    {
                        list.Add(pindex - 1);
                        addCount++;
                    }
                }//foreach
            }
            return addCount;
        }

        /// <summary>
        /// 获得运行时的打印页码数组，页码是从0开始计算的
        /// </summary>
        /// <param name="totalPageCount">总页数</param>
        /// <param name="currentPageIndex">当前页码</param>
        /// <returns>页码数组</returns>
        public int[] GetRuntimePageIndexs(int totalPageCount, int currentPageIndex, DomDocumentList documents)
        {
            try
            {
                var runtimePageIndexs = new List<int>();
                if (this.PrintRange == PrintRange.SomePages)
                {
                    // 打印部分页
                    if (this.SpecifyPageIndexs != null && this.SpecifyPageIndexs.Length > 0)
                    {
                        // 指定要打印的页码，这个页码字符串来自用户界面，因此是从1开始计算的
                        ParseSpecifyPageIndexString(this.SpecifyPageIndexs, runtimePageIndexs, totalPageCount);
                    }
                    else if (this.FromPage >= 0 && this.ToPage >= 0)
                    {
                        // 这个FromPage和ToPage是源自用户界面的，因此是从1开始计算的
                        for (var pi = this.FromPage; pi <= Math.Min(totalPageCount, this.ToPage); pi++)
                        {
                            runtimePageIndexs.Add(pi - 1);
                        }
                    }
                }
                else if (this.PrintRange == PrintRange.AllPages)
                {
                    // 打印所有页
                    for (var pi = 0; pi < totalPageCount; pi++)
                    {
                        runtimePageIndexs.Add(pi);
                    }
                }
                else if (this.PrintRange == PrintRange.CurrentPage)
                {
                    // 打印当前页,这个当前页码来自API，是从0开始计算的
                    runtimePageIndexs.Add(currentPageIndex);
                }
                if (this.PrintMode == DCPrintMode.EvenPage)
                {
                    // 打印偶数页,这里的偶数页是从1开始计算的
                    for (var iCount = runtimePageIndexs.Count - 1; iCount >= 0; iCount--)
                    {
                        if ((runtimePageIndexs[iCount] % 2) == 1)
                        {
                            runtimePageIndexs.RemoveAt(iCount);
                        }
                    }
                }
                else if (this.PrintMode == DCPrintMode.OddPage)
                {
                    // 打印奇数页,这里的页码是从1开始计算的
                    for (var iCount = runtimePageIndexs.Count - 1; iCount >= 0; iCount--)
                    {
                        if ((runtimePageIndexs[iCount] % 2) == 0)
                        {
                            runtimePageIndexs.RemoveAt(iCount);
                        }
                    }
                }
                if (this.Copies > 1)
                {
                    // 多份打印
                    var list2 = new List<int>();
                    if (this.Collate)
                    {
                        // 逐份打印
                        for (var iCount = 0; iCount < this.Copies; iCount++)
                        {
                            list2.AddRange(runtimePageIndexs);
                        }
                    }
                    else
                    {
                        // 逐页打印
                        for (var iCount = 0; iCount < runtimePageIndexs.Count; iCount++)
                        {
                            for (var iCount2 = 0; iCount2 < this.Copies; iCount2++)
                            {
                                list2.Add(runtimePageIndexs[iCount]);
                            }
                        }
                    }
                    return list2.ToArray();
                }
                else
                {
                    return runtimePageIndexs.ToArray();
                }
            }
            catch (Exception ex)
            {
                JavaScriptMethods.Tools_ReportException("GetRuntimePageIndexs", ex.Message, ex.ToString(), 0);
                return null;
            }
        }
    }

}
