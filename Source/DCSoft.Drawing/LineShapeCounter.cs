using System;
using System.Collections.Generic;

namespace DCSoft.Drawing
{
    /// <summary>
    /// 线条图形累计器
    /// </summary>
    public class LineShapeCounter : IDisposable
    {
        public void Dispose()
        {
            if (this._HLines != null)
            {
                this._HLines.Clear();
                this._HLines = null;
            }
            if (this._VLines != null)
            {
                this._VLines.Clear();
                this._VLines = null;
            }
        }
        /// <summary>
        /// 添加边框
        /// </summary>
        /// <param name="bounds">矩形区域</param>
        /// <param name="showLeft">显示左边框</param>
        /// <param name="showTop">显示上边框</param>
        /// <param name="showRight">显示右边框</param>
        /// <param name="showBottom">显示下边框</param>
        /// <param name="color">颜色</param>
        /// <param name="lineWidth">线条宽度</param>
        /// <param name="lineStyle">线条样式</param>
        /// <param name="level">等级</param>
        /// <param name="nullLine">是否为空白线段</param>
        public void AddBorder(
             DCSystem_Drawing.RectangleF bounds,
            bool showLeft,
            bool showTop,
            bool showRight,
            bool showBottom,
             DCSystem_Drawing.Color color,
            float lineWidth,
            DashStyle lineStyle,
            int level,
            bool nullLine)
        {
            if (bounds.Width <= 0 || bounds.Height <= 0)
            {
                return;
            }
            if (showLeft)
            {
                AddLine(bounds.Left, bounds.Top, bounds.Left, bounds.Bottom, color, lineWidth, lineStyle, level, nullLine);
            }
            if (showTop)
            {
                AddLine(bounds.Left, bounds.Top, bounds.Right, bounds.Top, color, lineWidth, lineStyle, level, nullLine);
            }
            if (showRight)
            {
                AddLine(bounds.Right, bounds.Top, bounds.Right, bounds.Bottom, color, lineWidth, lineStyle, level, nullLine);
            }
            if (showBottom)
            {
                AddLine(bounds.Left, bounds.Bottom, bounds.Right, bounds.Bottom, color, lineWidth, lineStyle, level, nullLine);
            }
        }
        private int _LastYIndex = -1;

        private static int _NativeTotalCount = 0;
        private static int _DeleteCount = 0;
        private static int _MatchCount = 0;
        private bool _NeedOrderByLevel = false;
        /// <summary>
        /// 垂直线列表
        /// </summary>
        private List<LineShapeItem> _VLines = new List<LineShapeItem>();
        /// <summary>
        /// 水平线列表
        /// </summary>
        private List<LineShapeItem> _HLines = new List<LineShapeItem>();
        /// <summary>
        /// 添加线段
        /// </summary>
        /// <param name="x1">起点X坐标</param>
        /// <param name="y1">起点Y坐标</param>
        /// <param name="x2">终点X坐标</param>
        /// <param name="y2">终点Y坐标</param>
        /// <param name="color">颜色</param>
        /// <param name="width">线条宽度</param>
        /// <param name="style">类型</param>
        /// <param name="level">等级</param>
        /// <param name="nullLine">是否为空白线</param>
        public void AddLine(float x1, float y1, float x2, float y2, Color color, float width, DashStyle style, int level, bool nullLine)
        {
            _NativeTotalCount++;
            if (level != 0)
            {
                this._NeedOrderByLevel = true;
            }
            var styleHashCode = LineShapeItem.ComputeStyleHasCode(nullLine, color, style, width, level);
            if (y1 == y2)
            {
                // 水平线
                if (x1 > x2)
                {
                    var temp = x1;
                    x1 = x2;
                    x2 = temp;
                }
                // 横线快速判断合并，减少后期工作量
                LineShapeItem matchItem = null;
                var hLines = this._HLines;
                if (nullLine || color == Color.White)
                {
                    // 白线无法覆盖之前的非白线
                    float lastY = -1;
                    for (var iCount = hLines.Count - 1; iCount >= 0; iCount--)
                    {
                        var item2 = hLines[iCount];
                        if (item2.Y1 == y1
                            && item2.Level == level
                            && item2.NullLine == false
                            && item2.Color != Color.White)
                        {
                            if (item2.X1 <= x1
                                && item2.X2 >= x2)
                            {
                                _DeleteCount++;
                                return;
                            }
                        }
                        else if (item2.Y1 < y1)
                        {
                            if (lastY > 0)
                            {
                                if (item2.Y1 < lastY)
                                {
                                    break;
                                }
                            }
                            else
                            {
                                lastY = item2.Y1;
                            }
                        }
                        //else if(item2.Y1 < y1 )
                        //{
                        //    break;
                        //}
                    }//for
                }
                //if (matchItem == null)
                {
                    for (var iCount = hLines.Count - 1; iCount >= 0; iCount--)
                    {
                        var item2 = hLines[iCount];
                        if (item2.Y1 == y1 && item2.Level == level)
                        {
                            matchItem = item2;
                            //this._LastXIndex = iCount;
                            break;
                        }
                    }//for
                }
                if (matchItem != null)
                {
                    if (matchItem.StyleHashCode == styleHashCode)
                    {
                        if (matchItem.X2 == x1)
                        {
                            // 延长处理
                            matchItem.X2 = x2;
                            _DeleteCount++;
                            return;
                        }
                        else if (matchItem.X1 <= x1 && x2 <= matchItem.X2)
                        {
                            // 被吞掉了，无需创建
                            _DeleteCount++;
                            return;
                        }
                    }
                    else if (matchItem.X1 <= x1 && x2 <= matchItem.X2)
                    {
                        if (nullLine)
                        {
                            // 被吞并掉了，无需创建
                            _DeleteCount++;
                            return;
                        }
                        else if (color == Color.White)
                        {
                            if (matchItem.Color != Color.White)
                            {
                                _DeleteCount++;
                                return;
                            }
                        }
                    }
                }
                //this._LastXIndex = -1;
                var newItem = new LineShapeItem();
                newItem.X1 = x1;
                newItem.X2 = x2;
                newItem.Y1 = y1;
                newItem.Y2 = y2;
                newItem.Color = color;
                newItem.Width = width;
                newItem.Style = style;
                newItem.Level = level;
                newItem.NullLine = nullLine;
                newItem.StyleHashCode = styleHashCode;// .UpdateStyleHashCode();
                hLines.Add(newItem);
            }
            else
            {
                //竖线
                if (y1 > y2)
                {
                    var temp = y1;
                    y1 = y2;
                    y2 = temp;
                }
                var vLines = this._VLines;
                LineShapeItem matchItem = null;
                if (this._LastYIndex >= 0)
                {
                    var endIndex = (int)Math.Max(0, this._LastYIndex - 2);
                    for (var iCount = (int)Math.Min(vLines.Count - 1, this._LastYIndex + 4); iCount >= endIndex; iCount--)
                    {
                        var item = vLines[iCount];
                        if (item.X1 == x1 && item.Level == level)
                        {
                            _MatchCount++;
                            matchItem = item;
                            break;
                        }
                    }
                }
                if (matchItem == null)
                {
                    for (var iCount = vLines.Count - 1; iCount >= 0; iCount--)
                    {
                        var item2 = vLines[iCount];
                        if (item2.X1 == x1 && item2.Level == level)
                        {
                            matchItem = item2;
                            this._LastYIndex = iCount;
                            break;
                        }
                    }//for
                }
                if (matchItem != null)
                {
                    if (matchItem.StyleHashCode == styleHashCode)
                    {
                        if (matchItem.Y2 == y1)
                        {
                            // 延长处理
                            matchItem.Y2 = y2;
                            _DeleteCount++;
                            return;
                        }
                        if (matchItem.Y1 <= y1 && y2 <= matchItem.Y2)
                        {
                            // 被吞并了，无需处理
                            _DeleteCount++;
                            return;
                        }
                    }
                    if (matchItem.Y1 <= y1 && y2 <= matchItem.Y2)
                    {
                        if (nullLine)
                        {
                            // 被吞并掉了，无需创建
                            _DeleteCount++;
                            return;
                        }
                        else if (color == Color.White && matchItem.NullLine == false)
                        {
                            _DeleteCount++;
                            return;
                        }
                        else if (matchItem.NullLine && matchItem.Y1 == y1 && matchItem.Y2 == y2 && nullLine == false)
                        {
                            // 吞并空白线条
                            matchItem.Color = color;
                            matchItem.Width = width;
                            matchItem.Style = style;
                            matchItem.Level = level;
                            matchItem.NullLine = nullLine;
                            matchItem.StyleHashCode = styleHashCode;
                            _DeleteCount++;
                            return;
                        }
                    }
                    else if (nullLine && matchItem.Y1 <= y1 && y2 <= matchItem.Y2)
                    {
                        // 被吞并掉了，无需创建
                        _DeleteCount++;
                        return;
                    }
                }
                var newItem = new LineShapeItem();
                newItem.Y1 = y1;
                newItem.Y2 = y2;
                newItem.X1 = x1;
                newItem.X2 = x2;
                newItem.Color = color;
                newItem.Width = width;
                newItem.Style = style;
                newItem.Level = level;
                newItem.NullLine = nullLine;
                newItem.StyleHashCode = styleHashCode;// .UpdateStyleHashCode();
                vLines.Add(newItem);
            }

        }

        public LineShapeItem[] GetRuntimeItems(ref int itemsCount, DCSystem_Drawing.RectangleF clipRectangle)
        {
            var hitems = this._HLines.ToArray();
            var hcount = Compress(hitems, clipRectangle, true);
            if (this._NeedOrderByLevel)
            {
                for (var iCount = 0; iCount < hcount; iCount++)
                {
                    hitems[iCount].Index = iCount;
                }
                Array.Sort<LineShapeItem>(hitems, 0, hcount, LevelComparer.Instance);
            }

            var vitems = this._VLines.ToArray();
            var vcount = Compress(vitems, clipRectangle, false);
            if (this._NeedOrderByLevel)
            {
                for (var iCount = 0; iCount < vcount; iCount++)
                {
                    vitems[iCount].Index = iCount;
                }
                Array.Sort<LineShapeItem>(vitems, 0, vcount, LevelComparer.Instance);
            }
            var resultItems = new LineShapeItem[hcount + vcount];
            Array.Copy(hitems, resultItems, hcount);
            Array.Copy(vitems, 0, resultItems, hcount, vcount);
            itemsCount = hcount + vcount;
            return resultItems;
        }

        private static int CompressArray(LineShapeItem[] items, int startIndex, int length)
        {
            if (items == null)
            {
                throw new ArgumentNullException("items");
            }
            if (startIndex < 0)
            {
                throw new ArgumentOutOfRangeException("startIndex=" + startIndex);
            }
            if (length < 0)
            {
                throw new ArgumentOutOfRangeException("length=" + length);
            }
            if (startIndex + length > items.Length)
            {
                throw new ArgumentOutOfRangeException("startIndex+length>=" + items.Length);
            }
            if (length == 0)
            {
                return 0;
            }
            int step = 0;
            int endIndex = startIndex + length - 1;
            for (int iCount = startIndex; iCount <= endIndex; iCount++)
            {
                if (items[iCount] == null)
                {
                    step++;
                }
                else if (step > 0)
                {
                    items[iCount - step] = items[iCount];
                    items[iCount] = null;
                }
            }
            int result = length - step;
            return result;
        }

        private static readonly int _WhiteRgb = DCSystem_Drawing.Color.White.ToArgb();
        private static int CompressNullLine(LineShapeItem[] items, bool isHLine)
        {
            int itemsCount = items.Length;
            int deletedCount = 0;
            // 白色线段或者空白线段进行覆盖压缩
            for (int iCount = itemsCount - 1; iCount >= 0; iCount--)
            {
                LineShapeItem item = items[iCount];
                if (item.NullLine || item.Color.ToArgb() == _WhiteRgb)
                {
                    // 遇到空白线或者白线
                    foreach (var item2 in items)
                    {
                        if (item2 == null || item == item2)
                        {
                            // 不能覆盖自身或者方向不对
                            continue;
                        }
                        //compCounter++;
                        if (item2.NullLine || item2.Width <= 0)
                        {
                            // 等级不够或者为空白线段的不处理
                            continue;
                        }
                        if (isHLine)
                        {
                            // 处理横线
                            if (item.Y1 == item2.Y1 && item2.X1 <= item.X1 && item2.X2 >= item.X2)
                            {
                                // 完全覆盖
                                deletedCount++;
                                items[iCount] = null;
                                break;
                            }
                        }
                        else
                        {
                            // 处理竖线
                            if (item.X1 == item2.X1 && item2.Y1 <= item.Y1 && item2.Y2 >= item.Y2)
                            {
                                // 完全覆盖
                                deletedCount++;
                                items[iCount] = null;
                                break;
                            }
                        }
                    }
                    if (deletedCount > 0)
                    {
                        //itemsCount = CompressArray(items, 0, itemsCount);
                    }
                }
            }//for
            if (deletedCount > 0)
            {
                itemsCount = CompressArray(items, 0, itemsCount);
            }
            return itemsCount;
        }
        /// <summary>
        /// 进行内容压缩,线段合并,减少无效的绘图工作量。
        /// </summary>
        private static int Compress(LineShapeItem[] items, DCSystem_Drawing.RectangleF clipRectangle, bool isHLine)
        {
            int itemsCount = items.Length;
            itemsCount = CompressNullLine(items, isHLine);
            float clipTop = clipRectangle.Top - 8;
            float clipLeft = clipRectangle.Left - 8;
            float clipRight = clipRectangle.Right + 16;
            float clipBottom = clipRectangle.Bottom + 16;
            bool hasDeleted = false;
            for (int iCount = itemsCount - 1; iCount > 0; iCount--)
            {
                var item = items[iCount];
                if (isHLine)
                {
                    // 横线
                    if (item.Y1 < clipTop || item.Y1 > clipBottom)
                    {
                        // 超出剪切矩形范围
                        items[iCount] = null;
                        hasDeleted = true;
                    }
                    else
                    {
                        for (int iCount2 = iCount - 1; iCount2 >= 0; iCount2--)
                        {
                            var item2 = items[iCount2];
                            if (item.Y1 == item2.Y1
                                && item.X1 <= item2.X2 && item.X2 >= item2.X1)// Insterct(item.X1, item.X2, item2.X1, item2.X2))
                            {
                                // 试图合并水平线
                                if (item.StyleHashCode == item2.StyleHashCode)
                                {
                                    // 线条样式完全一样，将item的数据合并到item2
                                    if (item.X1 < item2.X1)
                                    {
                                        item2.X1 = item.X1;
                                    }
                                    if (item.X2 > item2.X2)
                                    {
                                        item2.X2 = item.X2;
                                    }
                                    hasDeleted = true;
                                    items[iCount] = null;
                                }
                                else
                                {
                                    // 线条样式不一致，需要进行截断
                                    if (item2.X1 < item.X1)
                                    {
                                        item2.X2 = item.X1;
                                    }
                                    else if (item2.X2 > item.X2)
                                    {
                                        item2.X1 = item.X2;
                                    }
                                }
                                break;
                            }
                        }//for
                    }
                }//if
                else
                {
                    // 竖线
                    if (item.X1 < clipLeft || item.X1 > clipRight)
                    {
                        // 超出剪切矩形范围
                        hasDeleted = true;
                        items[iCount] = null;
                    }
                    else
                    {
                        for (int iCount2 = iCount - 1; iCount2 >= 0; iCount2--)
                        {
                            var item2 = items[iCount2];
                            if (item.X1 == item2.X1
                                && item.Y1 <= item2.Y2 && item.Y2 >= item2.Y1)// Insterct(item.Y1, item.Y2, item2.Y1, item2.Y2))
                            {
                                // 试图合并竖线
                                if (item.StyleHashCode == item2.StyleHashCode)
                                {
                                    // 线条样式完全一样，可以进行合并
                                    if (item.Y1 < item2.Y1)
                                    {
                                        item2.Y1 = item.Y1;
                                    }
                                    if (item.Y2 > item2.Y2)
                                    {
                                        item2.Y2 = item.Y2;
                                    }
                                    hasDeleted = true;
                                    items[iCount] = null;
                                }
                                else
                                {
                                    // 线条样式不一致，需要进行截断
                                    if (item2.Y1 < item.Y1)
                                    {
                                        item2.Y2 = item.Y1;
                                    }
                                    else if (item2.Y2 > item.Y2)
                                    {
                                        item2.Y1 = item.Y2;
                                    }
                                }
                                break;
                            }
                        }//for
                    }//if
                }//else
            }
            if (hasDeleted)
            {
                itemsCount = CompressArray(items, 0, itemsCount);
            }
            return itemsCount;
        }
        private class LevelComparer : IComparer<LineShapeItem>
        {
            public static readonly LevelComparer Instance = new LevelComparer();
            public int Compare(LineShapeItem x, LineShapeItem y)
            {
                if (x.NullLine != y.NullLine)
                {
                    if (x.NullLine)
                    {
                        return -1;
                    }
                    else
                    {
                        return 1;
                    }
                }
                int result = x.Level - y.Level;

                if (result == 0)
                {
                    result = x.Index - y.Index;
                }
                return result;
            }
        }

        public class LineShapeItem
        {
            public LineShapeItem()
            {

            }
            public string Name = null;
            public int Index = 0;
            public float X1 = float.NaN;
            public float Y1 = float.NaN;
            public float X2 = float.NaN;
            public float Y2 = float.NaN;
            private static readonly DCSystem_Drawing.Color _Black = DCSystem_Drawing.Color.Black;
            public DCSystem_Drawing.Color Color = _Black;
            public float Width = 1f;
            public DashStyle Style = DashStyle.Solid;
            public int Level = 0;
            public bool NullLine = false;
            public long StyleHashCode = 0;
            public static long ComputeStyleHasCode(bool nullLine, Color c, DashStyle style, float width, int level)
            {
                if (nullLine)
                {
                    return int.MinValue;
                }
                else
                {
                    var result = (long)c._value;// .ToArgb();
                    result = (result << 4) + (int)style;
                    result = (result << 8) + (int)(width * 2);
                    result = (result << 1) + level;
                    //result = result << 1;
                    return result;
                }
            }
        }
    }
}