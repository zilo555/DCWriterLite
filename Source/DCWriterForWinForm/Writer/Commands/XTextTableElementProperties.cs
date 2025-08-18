using System;
using System.Collections.Generic;
using System.Text;
using DCSoft.Writer.Dom ;
using System.ComponentModel;
using DCSoft.Writer.Controls;
using DCSoft.Drawing;

namespace DCSoft.Writer.Commands
{
    /// <summary>
    /// 创建表格使用的信息对象
    /// </summary>
    /// <remarks>编制 袁永福</remarks>
    //[System.Obsolete("★★★★★不推荐使用了，请使用DCSoft.Writer.ElementPropertiesEditor")]
    internal class XTextTableElementProperties : XTextElementProperties, System.ICloneable
    {
        /// <summary>
        /// 初始化对象
        /// </summary>
        public XTextTableElementProperties()
        {
        }

        /// <summary>
        /// 不支持
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public override bool ReadProperties(DomElement element)
        {
            return false;
        }

        private string _ID = null;

        /// <summary>
        /// 表格ID
        /// </summary>
        public string ID
        {
            get { return _ID; }
            set { _ID = value; }
        }
        private int _RowsCount = 3;
        /// <summary>
        /// 表格行数
        /// </summary>
        [DefaultValue( 3 )]        
        public int RowsCount
        {
            get { return _RowsCount; }
            set { _RowsCount = value; }
        }

        private int _ColumnsCount = 3;
        /// <summary>
        /// 表格列数
        /// </summary>
        [DefaultValue( 3 )]
        public int ColumnsCount
        {
            get { return _ColumnsCount; }
            set { _ColumnsCount = value; }
        }

        private float _ColumnWidth = 0f;
        /// <summary>
        /// 用户指定的表格列宽度,为0则自动设置,单位Document
        /// </summary>
        [DefaultValue( 0f )]
        public float ColumnWidth
        {
            get { return _ColumnWidth; }
            set { _ColumnWidth = value; }
        }

        private float _RowHeight = 0;
        /// <summary>
        /// 用户指定的表格行高度,为0则自动设置,单位Document
        /// </summary>
        [DefaultValue( 0f)]
        public float RowHeight
        {
            get { return _RowHeight; }
            set { _RowHeight = value; }
        }

        private int _BorderWidth = 1;
        /// <summary>
        /// 边框宽度
        /// </summary>
        [DefaultValue( 1 )]
        public int BorderWidth
        {
            get { return _BorderWidth; }
            set { _BorderWidth = value; }
        }

        private DCSystem_Drawing.Color _BorderColor = DCSystem_Drawing.Color.Black;
        /// <summary>
        /// 边框色
        /// </summary>
       // [DefaultColorValue("Black")]
        public DCSystem_Drawing.Color BorderColor
        {
            get { return _BorderColor; }
            set { _BorderColor = value; }
        }

        private DashStyle _BorderStyle
            = DashStyle.Solid;
        /// <summary>
        /// 边框线样式
        /// </summary>
        [DefaultValue( DashStyle.Solid )]
        public DashStyle BorderStyle
        {
            get { return _BorderStyle; }
            set { _BorderStyle = value; }
        }

        private float _CellPaddingLeft = 15f;
        /// <summary>
        /// 单元格左内边距
        /// </summary>
        [DefaultValue( 15f )]
        public float CellPaddingLeft
        {
            get { return _CellPaddingLeft; }
            set { _CellPaddingLeft = value; }
        }

        private float _CellPaddingTop = 10f;
        /// <summary>
        /// 单元格上内边距
        /// </summary>
        [DefaultValue(10f)]
        public float CellPaddingTop
        {
            get { return _CellPaddingTop; }
            set { _CellPaddingTop = value; }
        }

        private float _CellPaddingRight = 15f;
        /// <summary>
        /// 单元格右内边距
        /// </summary>
        [DefaultValue(15f)]
        public float CellPaddingRight
        {
            get { return _CellPaddingRight; }
            set { _CellPaddingRight = value; }
        }

        private float _CellPaddingBottom = 10f;
        /// <summary>
        /// 单元格下内边距
        /// </summary>
        [DefaultValue(10f)]
        public float CellPaddingBottom
        {
            get { return _CellPaddingBottom; }
            set { _CellPaddingBottom = value; }
        }

        /// <summary>
        /// 根据对象设置创建文档表格对象
        /// </summary>
        /// <param name="document">文档对象</param>
        /// <returns>创建的表格对象</returns>
        public override DomElement CreateElement(DomDocument document)
        {
            if (this.RowsCount <= 0 || this.ColumnsCount <= 0)
            {
                return null;
            }
            DomTableElement table = new DomTableElement();
            ApplyToElement(document, table , false );
            return table;
        }

        /// <summary>
        /// 根据对象参数来设置文档表格对象
        /// </summary>
        /// <param name="document">文档对象</param>
        /// <param name="element">文档元素对象</param>
        /// <param name="logUndo">是否记录撤销操作信息</param>
        /// <returns>创建的表格对象</returns>
        public override bool ApplyToElement(
            DomDocument document, 
            DomElement element ,
            bool logUndo)
        {
            DomTableElement table = (DomTableElement)element;
            table.OwnerDocument = document;
            // 创建单元格对象使用的样式
            DocumentContentStyle style = new DocumentContentStyle();
            style.BorderColor = this.BorderColor;
            style.BorderWidth = this.BorderWidth;
            style.BorderStyle = this.BorderStyle;
            style.BorderLeft = true;
            style.BorderTop = true;
            style.BorderRight = true;
            style.BorderBottom = true;
            style.PaddingBottom = this.CellPaddingBottom;
            style.PaddingLeft = this.CellPaddingLeft;
            style.PaddingRight = this.CellPaddingRight;
            style.PaddingTop = this.CellPaddingTop;
            style.VerticalAlign = Drawing.VerticalAlignStyle.Top ;
            int styleIndex = document.ContentStyles.GetStyleIndex(style);
            float cw = this.ColumnWidth;
            if (cw <= 0)
            {
                cw = ( document.PageSettings.ViewClientWidth - 30)/ this.ColumnsCount;
            }
            float rh = this.RowHeight;
            float rh2 =( ( DocumentContentStyle ) document.ContentStyles.Default).DefaultLineHeight;
            // 创建表格列对象
            for (int colIndex = 0; colIndex < this.ColumnsCount; colIndex++)
            {
                DomTableColumnElement col = table.CreateColumnInstance();
                col.Width = cw;
                table.AppendChildElement(col);
            }
            for (int rowIndex = 0; rowIndex < this.RowsCount; rowIndex++)
            {
                // 创建表格行对象
                DomTableRowElement row = table.CreateRowInstance();
                row.SpecifyHeight = rh;
                row.Height = rh2;
                row.Top = rowIndex * rh2;
                table.AppendChildElement( row );
                for (int colIndex = 0; colIndex < this.ColumnsCount; colIndex++)
                {
                    // 创建单元格对象
                    DomTableCellElement cell = table.CreateCellInstance();
                    cell.StyleIndex = styleIndex;
                    //cell.Width = cw;
                    //cell.Height = rh2;
                    cell.Left = cw * colIndex;
                    cell.Top = 0;
                    row.AppendChildElement( cell );
                    cell.AppendChildElement(new DomParagraphFlagElement());
                }//for
            }//for
            table.ID = this.ID;
            return true;
        }
        /// <summary>
        /// 复制对象
        /// </summary>
        /// <returns>复制品</returns>
        object ICloneable.Clone()
        {
            return (XTextTableElementProperties)this.MemberwiseClone();
        }

    }
}
