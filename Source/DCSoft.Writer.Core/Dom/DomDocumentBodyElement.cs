using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using DCSoft.Printing;
using DCSoft.Drawing;
using DCSoft.Common;

// 袁永福到此一游

namespace DCSoft.Writer.Dom
{
    /// <summary>
    /// 文档正文对象
    /// </summary>
#if !RELEASE
    [System.Diagnostics.DebuggerDisplay("Body :{ PreviewString }")]
#endif
    public sealed partial class DomDocumentBodyElement : DomDocumentContentElement
    {
        public static readonly string TypeName_XTextDocumentBodyElement = "XTextDocumentBodyElement";
        public override string TypeName => TypeName_XTextDocumentBodyElement;

        /// <summary>
        /// 初始化对象
        /// </summary>
        public DomDocumentBodyElement()
        {
            
        }


        public override PageContentPartyStyle ContentPartyStyle
        {
            get
            {
                return PageContentPartyStyle.Body;
            }
        }
        
        public override DCGridLineInfo InnerRuntimeGridLine()
        {
            if (this.OwnerDocument != null)
            {
                return this.OwnerDocument.PageSettings.DocumentGridLine;
            }
            else
            {
                return null;
            }

        }

        internal void RecalculateLineAbsPosition()
        {
            RecalculateLineAbsPosition_ContentElement(this);
        }

        private static void RecalculateLineAbsPosition_ContentElement(DomContentElement ce)
        {
            if (ce.HasPrivateLines())// .PrivateLines != null && ce.PrivateLines.Count > 0)
            {
                var ap = ce.AbsPosition;
                var apx = ap.X;
                var apy = ap.Y;
                var lineArr = ce.PrivateLines.InnerGetArrayRaw();
                var lineCount = ce.PrivateLines.Count;
                for(var iCount = 0;iCount < lineCount;iCount ++)
                {
                    var line = lineArr[iCount];
                    line.SetAbsPosition(apx, apy);// .RecalculateAbsPosition();
                    if (line.IsTableLine)
                    {
                        var table = line.TableElement;
                        var rowArr = table.Rows.InnerGetArrayRaw();
                        var rowCount = table.Rows.Count;
                        for(var rowIndex = 0; rowIndex < rowCount; rowIndex ++)
                        {
                            var row = (DomTableRowElement)rowArr[rowIndex];
                            if (row._RuntimeVisible)
                            {
                                var cellArr = row.Cells.InnerGetArrayRaw();
                                var cellCount = row.Cells.Count;
                                for(var cIndex = 0; cIndex < cellCount; cIndex ++)
                                {
                                    var cell = (DomTableCellElement)cellArr[cIndex];
                                    if (cell.RuntimeVisible )
                                    {
                                        RecalculateLineAbsPosition_ContentElement(cell);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        public override PointF AbsPosition
        {
            get
            {
                DomDocument document = this.OwnerDocument;
                if (document == null)
                {
                    return new PointF(0, 0);
                }
                else
                {
                    return new PointF(
                        document.Left, 
                        document.Top);
                }
            }
        }

        public override float AbsTop
        {
            get
            {
                DomDocument document = this.OwnerDocument;
                if (document == null)
                {
                    return 0;
                }
                else
                {
                    return document.Top;
                }
            }
        }

        /// <summary>
        /// 高度
        /// </summary>
        public override float Height
        {
            get
            {
                return base.Height;
            }
            set
            {
                if (base.Height != value)
                {
                    base.Height = value;
                }
            }
        }
#if !RELEASE
        /// <summary>
        /// 获得预览文本
        /// </summary>
        public override string PreviewString
        {
            get
            {
                return "Body:" + base.PreviewString;
            }
        }
#endif
        /// <summary>
        /// 判断是否绘制网格线
        /// </summary>
        /// <param name="renderMode">呈现模式</param>
        /// <returns>是否绘制网格线</returns>
        public bool IsDrawGridLine(InnerDocumentRenderMode renderMode)
        {
            DCGridLineInfo info = this.InnerRuntimeGridLine();
            if (info != null && info.Visible && info.RuntimeGridSpan > 0)
            {
                if( info.Printable == false && renderMode == InnerDocumentRenderMode.Print)
                {
                    // 不打印
                    return false;
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// 绘制内容
        /// </summary>
        /// <param name="args">参数</param>
        public override void DrawContent(InnerDocumentPaintEventArgs args)
        {
            if (this.PrivateLines == null
                || this.PrivateLines.Count == 0)
            {
                return;
            }
            args.Content = this.Content;
            InnerDocumentPaintEventArgs contentArgs = args.Clone();
            DrawBodyGridLine(args);
            // 绘制文档内容
            base.DrawContent(contentArgs);
        }

        /// <summary>
        /// 通篇强制绘制网格线
        /// </summary>
        /// <param name="args"></param>
        private void DrawBodyGridLine(InnerDocumentPaintEventArgs args)
        {
            if( args.EnabledDrawGridLine == false )
            {
                // 不允许绘制文档网格线
                return;
            }
            if (IsDrawGridLine(args.RenderMode) == false)
            {
                return;
            }
            DCGridLineInfo info = this.InnerRuntimeGridLine();
            if (info == null
                || info.Visible == false
                || info.RuntimeGridSpan <= 0)
            {
                return;
            }
            if (args.Type == PageContentPartyStyle.HeaderRows)
            {
                return;
            }
            RectangleF clipRect = args.ClipRectangle;
            RectangleF pageClipRect = args.PageClipRectangle;
            if (args.PageClipRectangle.IsEmpty == false)
            {
                clipRect = RectangleF.Union(clipRect, args.PageClipRectangle);
            }
            else
            {
                pageClipRect = clipRect;
            }
            clipRect = RectangleF.Intersect(this.GetAbsBounds(), clipRect);
            if (clipRect.IsEmpty)
            {
                return;
            }
            List<float[]> headerRowsRanges = null;
            if (this.HasHeaderRow)
            {
                headerRowsRanges = new List<float[]>();
                foreach (DomElement element in this.Elements)
                {
                    if (element is DomTableElement && element.RuntimeVisible)
                    {
                        foreach (DomTableRowElement row in element.Elements)
                        {
                            if (row.RuntimeVisible && row.HeaderStyle)
                            {
                                float atop = row.GetAbsTop();
                                if (headerRowsRanges.Count > 0)
                                {
                                    var last = headerRowsRanges[headerRowsRanges.Count - 1];
                                    if (atop <= last[1] + 0.1)
                                    {
                                        last[1] = atop + row.Height + 0.2f;
                                        continue;
                                    }
                                }
                                headerRowsRanges.Add(new float[] { atop - 0.1f, atop + row.Height + 0.2f });
                            }
                        }
                    }
                }
                if(headerRowsRanges.Count == 0 )
                {
                    headerRowsRanges = null;
                }
            }
            LineZoneList zones = null;
            var bolIsGlobal = info != null && info == this.OwnerDocument._GlobalGridInfo;
            if (bolIsGlobal == false)
            {
                zones = this.GetLineZones();
            }
            float step = info.RuntimeGridSpan;
            float pos = 0;
            var back = args.Graphics.Save();
            args.Graphics.ResetClip();
            using (var p = info.CreatePen())
            {
                bool optionsSpecifyDebugMode = this.GetDocumentBehaviorOptions().SpecifyDebugMode;
                var pageClipRect_Bottom = pageClipRect.Bottom;
                var clipRect_Top = clipRect.Top;
                while (pos < pageClipRect_Bottom)
                {
                    if (pos >= clipRect_Top)
                    {
                        if (headerRowsRanges != null )
                        {
                            for (var iCount = headerRowsRanges.Count - 1; iCount >= 0; iCount--)
                            {
                                var range = headerRowsRanges[iCount];
                                if (pos >= range[0] && pos <= range[1])
                                {
                                    goto NextGridLine;
                                }
                            }
                        }
                        {
                            // 不是页面的上边缘才允许绘制网格线。
                            if ( bolIsGlobal || zones.Match(pos) == false)
                            {
                                args.Graphics.DrawLine(
                                    p,
                                    clipRect.Left,
                                    pos,
                                    clipRect.Right,
                                    pos);
                                if (optionsSpecifyDebugMode)// this.OwnerDocument.Options.BehaviorOptions.SpecifyDebugMode)
                                {
                                    args.Graphics.DrawString(
                                        pos.ToString(),
                                        XFontValue.Default,
                                        Color.Blue,
                                        clipRect.Left - 100,
                                        pos);
                                }
                            }
                        }
                    }
                NextGridLine:;
                    pos = pos + step;
                }//while
            }
            headerRowsRanges?.Clear();
            args.Graphics.Restore(back);
        }

       /// <summary>
        /// 销毁对象
        /// </summary>
        public override void Dispose()
        {
            base.Dispose();
            this.ClearLineZones();
        }
        private class LineZone : IComparable<LineZone>
        {
            public bool IsTableLine = false;
            public float Top = 0;
            public float Bottom = 0;

            public int CompareTo(LineZone other)
            {
                if (this.Top > other.Top)
                {
                    return 1;
                }
                else if (this.Top == other.Top)
                {
                    return 0;
                }
                else
                {
                    return -1;
                }
            }

            /// <summary>
            /// 是否命中文档行的中央地带
            /// </summary>
            /// <param name="pos"></param>
            /// <returns></returns>
            public bool MatchMiddle( float pos )
            {
                return pos >= this.Top + 2 && pos <= this.Bottom - 2;
            }
#if !RELEASE
            public override string ToString()
            {
                return this.Top + " -> " + this.Bottom;
            }
#endif
        }
        private class LineZoneList : List<LineZone>
        {
            public LineZoneList(DomDocumentBodyElement body)
            {
                if(body.HasPrivateLines() == false )
                {
                    return;
                }
                bool addTable = body.OwnerDocument._GlobalGridInfo == null 
                    || body.GetDocumentViewOptions().ShowGlobalGridLineInTableAndSection == false ;
                if (addTable == false )
                {
                    this.HasTableLine = false;
                    foreach (var line in body.PrivateLines)
                    {
                        var zone = new LineZone();
                        zone.Top = line.AbsTop;
                        zone.Bottom = zone.Top + line.Height;
                        zone.IsTableLine = false;
                        this.Add(zone);
                    }
                }
                else
                {
                    var lines = body.GetAllLines();
                    float topCount = 0;
                    bool needSort = false;
                    var posDic = new DoubleKeyDictionary<int, int , LineZone>();
                    var bodyAbsTop = body.AbsTop;
                    foreach (var line in lines.FastForEach())
                    {
                        bool isTableLine = line.IsTableLine ;
                        if (isTableLine)
                        {
                            this.HasTableLine = true;
                        }
                        if (line._OwnerContentElement == body)
                        {
                            var zone = new LineZone();
                            zone.Top = bodyAbsTop + line.Top;
                            zone.Bottom = zone.Top + line.Height;
                            zone.IsTableLine = isTableLine;
                            this.Add(zone);
                        }
                        else
                        {
                            var top = line.AbsTop;
                            var bottom = top + line.Height;
                            if (top < topCount)
                            {
                                needSort = true;
                            }
                            topCount = top;
                            if (posDic.ContainsKey((int)top, (int)bottom) == false)
                            {
                                var zone = new LineZone();
                                zone.Top = top;
                                zone.Bottom = bottom;
                                zone.IsTableLine = isTableLine;
                                this.Add(zone);
                                posDic.Add((int)top, (int)bottom, zone);
                            }
                        }
                    }
                    posDic.Clear();
                    if(needSort)
                    {
                        this.Sort();
                    }
                }
            }

            private bool HasTableLine = false;

            private Dictionary<int, bool> _Matched = new Dictionary<int, bool>();
            /// <summary>
            /// 二分法进行查找。
            /// </summary>
            /// <param name="pos"></param>
            /// <returns></returns>
            public bool Match(float pos)
            {
                if (_Matched == null)
                {
                    _Matched = new Dictionary<int, bool>();
                }
                if (_Matched.ContainsKey((int)pos))
                {
                    return _Matched[(int)pos];
                }
                bool result = false;
                if (this.HasTableLine)
                {
                    // 包括了表格行，则进行全局遍历
                    for( var iCount = this.Count -1;iCount >=0;iCount -- )
                    {
                        if(this[iCount].MatchMiddle( pos ))
                        {
                            result = true;
                            break;
                        }
                    }
                }
                else
                {
                    int index1 = 0;
                    int index2 = this.Count - 1;
                    while (index1 < index2)
                    {
                        if (index2 < index1 + 8)
                        {
                            // 间距过小，二分法效率不高，则直接循环判断
                            for (var iCount = index1; iCount <= index2; iCount++)
                            {
                                if (this[iCount].MatchMiddle(pos))
                                {
                                    result = true;
                                    break;
                                }
                            }
                            _Matched[(int)pos] = result;
                            return result;
                        }
                        int index = (index1 + index2) / 2;
                        var item = this[index];
                        if (pos >= item.Top - 2 && pos <= item.Top + 2)
                        {
                            // 直接命中文档行的上边缘，则没命中。
                            result = false;
                            break;
                        }
                        else if (item.MatchMiddle(pos))
                        {
                            // 命中文档行的中央
                            result = true;
                            break;
                        }
                        else if (pos < item.Top + 1)
                        {
                            index2 = index;
                            //index1 = index;
                        }
                        else
                        {
                            index1 = index;
                            //index2 = index2 / 2;
                        }
                        if (index2 - index1 == 1)
                        {
                            break;
                        }
                    }
                }
                _Matched[(int)pos] = result;
                return result;
            }
            public void ClearData()
            {
                this.Clear();
                if (this._Matched != null)
                {
                    this._Matched.Clear();
                    this._Matched = null;
                }
            }
        }
        [NonSerialized]
        private LineZoneList _LineZones = null;
        private LineZoneList GetLineZones()
        {
            if (_LineZones == null)
            {
                _LineZones = new LineZoneList(this);
            }
            return _LineZones;
        }

        internal void ClearLineZones()
        {
            if( _LineZones != null )
            {
                _LineZones.ClearData();
                _LineZones = null;
            }
        }

        /// <summary>
        /// 返回BODY样式
        /// </summary>
        public override PageContentPartyStyle PagePartyStyle
        {
            get
            {
                return PageContentPartyStyle.Body;
            }
        }

#if !RELEASE
        /// <summary>
        /// 返回调试时显示的文本
        /// </summary>
        /// <returns>文本</returns>
        public override string ToDebugString()
        {
            string txt = "Body" ;
            if (this.OwnerDocument != null)
            {
                txt = txt + ":" + this.OwnerDocument.RuntimeTitle;
            }
            return txt;
        }
#endif
    }
}
