using System;
using System.Collections.Generic;
using System.Text;
using DCSoft.Drawing;

namespace DCSoft.Writer.Commands
{
    /// <summary>
    /// 执行HeaderFormat命令使用的参数
    /// </summary>
    //[System.Reflection.Obfuscation ( Exclude = true , ApplyToMembers = false )]
    //[DCSoft.Common.DCPublishAPI]
    public partial class HeaderFormatCommandParameter
    {
        /// <summary>
        /// 为命令Header1创建参数对象
        /// </summary>
        /// <returns>创建的参数对象</returns>
        //[System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public static HeaderFormatCommandParameter CreateHeader1()
        {
            HeaderFormatCommandParameter p = new HeaderFormatCommandParameter();
            p.ParagraphOutlineLevel = 0;
            p.ParagraphMultiLevel = false;
            p.ParagraphListStyle = Drawing.ParagraphListStyle.None;
            p.LeftIndent = 0;
            p.FirstLineIndent = 0;
            p.FontSize = 24;
            p.LineSpacingStyle = LineSpacingStyle.SpaceMultiple;
            p.LineSpacing = 2.41f;
            p.FontStyle = FontStyle.Bold;
            return p;
        }

        /// <summary>
        /// 为命令Header2创建参数对象
        /// </summary>
        /// <returns>创建的参数对象</returns>
        //[System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public static HeaderFormatCommandParameter CreateHeader2()
        {
            HeaderFormatCommandParameter p = new HeaderFormatCommandParameter();
            p.ParagraphOutlineLevel = 1;
            p.ParagraphMultiLevel = false;
            p.ParagraphListStyle = Drawing.ParagraphListStyle.None;
            p.LeftIndent = 0;
            p.FirstLineIndent = 0;
            p.FontSize = 18;
            p.LineSpacingStyle = LineSpacingStyle.SpaceMultiple;
            p.LineSpacing = 1.73f;
            p.FontStyle = FontStyle.Bold;
            return p;
        }

        /// <summary>
        /// 为命令Header3创建参数对象
        /// </summary>
        /// <returns>创建的参数对象</returns>
        //[System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public static HeaderFormatCommandParameter CreateHeader3()
        {
            HeaderFormatCommandParameter p = new HeaderFormatCommandParameter();
            p.ParagraphOutlineLevel = 2;
            p.ParagraphMultiLevel = false;
            p.ParagraphListStyle = Drawing.ParagraphListStyle.None;
            p.LeftIndent = 0;
            p.FirstLineIndent = 0;
            p.FontSize = 13;
            p.LineSpacingStyle = LineSpacingStyle.SpaceMultiple;
            p.LineSpacing = 1.73f;
            p.FontStyle = FontStyle.Bold;
            return p;
        }

        /// <summary>
        /// 为命令Header4创建参数对象
        /// </summary>
        /// <returns>创建的参数对象</returns>
        //[System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public static HeaderFormatCommandParameter CreateHeader4()
        {
            HeaderFormatCommandParameter p = new HeaderFormatCommandParameter();
            p.ParagraphOutlineLevel = 3;
            p.ParagraphMultiLevel = false;
            p.ParagraphListStyle = Drawing.ParagraphListStyle.None;
            p.LeftIndent = 0;
            p.FirstLineIndent = 0;
            p.FontSize = 12;
            p.LineSpacingStyle = LineSpacingStyle.SpaceMultiple;
            p.LineSpacing = 1.57f;
            p.FontStyle = FontStyle.Bold;
            return p;
        }

        /// <summary>
        /// 为命令Header5创建参数对象
        /// </summary>
        /// <returns>创建的参数对象</returns>
        //[System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public static HeaderFormatCommandParameter CreateHeader5()
        {
            HeaderFormatCommandParameter p = new HeaderFormatCommandParameter();
            p.ParagraphOutlineLevel = 4;
            p.ParagraphMultiLevel = false;
            p.ParagraphListStyle = Drawing.ParagraphListStyle.None;
            p.LeftIndent = 0;
            p.FirstLineIndent = 0;
            p.FontSize = 10;
            p.LineSpacingStyle = LineSpacingStyle.SpaceMultiple;
            p.LineSpacing = 1.57f;
            p.FontStyle = FontStyle.Bold;
            return p;
        }

        /// <summary>
        /// 为命令Header6创建参数对象
        /// </summary>
        /// <returns>创建的参数对象</returns>
        //[System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public static HeaderFormatCommandParameter CreateHeader6()
        {
            HeaderFormatCommandParameter p = new HeaderFormatCommandParameter();
            p.ParagraphOutlineLevel = 5;
            p.ParagraphMultiLevel = false;
            p.ParagraphListStyle = Drawing.ParagraphListStyle.None;
            p.LeftIndent = 0;
            p.FirstLineIndent = 0;
            p.FontSize = 8;
            p.LineSpacingStyle = LineSpacingStyle.SpaceMultiple;
            p.LineSpacing = 1.33f;
            p.FontStyle = FontStyle.Bold;
            return p;
        }

        /// <summary>
        /// 为命令Header1WithMultiNumberlist创建参数对象
        /// </summary>
        /// <returns>创建的参数对象</returns>
        //[System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public static HeaderFormatCommandParameter CreateHeader1WithMultiNumberlist()
        {
            HeaderFormatCommandParameter p = new HeaderFormatCommandParameter();
            p.ParagraphOutlineLevel = 0;
            p.ParagraphMultiLevel = true;
            p.ParagraphListStyle = Drawing.ParagraphListStyle.ListNumberStyle;
            p.LeftIndent = 100;
            p.FirstLineIndent = -100;
            p.FontSize = 24;
            p.LineSpacingStyle = LineSpacingStyle.SpaceMultiple;
            p.LineSpacing = 2.41f;
            p.FontStyle = FontStyle.Bold;
            return p;
        }

        /// <summary>
        /// 为命令Header2WithMultiNumberlist创建参数对象
        /// </summary>
        /// <returns>创建的参数对象</returns>
        //[System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public static HeaderFormatCommandParameter CreateHeader2WithMultiNumberlist()
        {
            HeaderFormatCommandParameter p = new HeaderFormatCommandParameter();
            p.ParagraphOutlineLevel = 1;
            p.ParagraphMultiLevel = true;
            p.ParagraphListStyle = Drawing.ParagraphListStyle.ListNumberStyle;
            p.LeftIndent = 200;
            p.FirstLineIndent = -100;
            p.FontSize = 18;
            p.LineSpacingStyle = LineSpacingStyle.SpaceMultiple;
            p.LineSpacing = 1.73f;
            p.FontStyle = FontStyle.Bold;
            return p;
        }

        /// <summary>
        /// 为命令Header3WithMultiNumberlist创建参数对象
        /// </summary>
        /// <returns>创建的参数对象</returns>
        //[System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public static HeaderFormatCommandParameter CreateHeader3WithMultiNumberlist()
        {
            HeaderFormatCommandParameter p = new HeaderFormatCommandParameter();
            p.ParagraphOutlineLevel = 2;
            p.ParagraphMultiLevel = true;
            p.ParagraphListStyle = Drawing.ParagraphListStyle.ListNumberStyle;
            p.LeftIndent = 300;
            p.FirstLineIndent = -100;
            p.FontSize = 13;
            p.LineSpacingStyle = LineSpacingStyle.SpaceMultiple;
            p.LineSpacing = 1.73f;
            p.FontStyle = FontStyle.Bold;
            return p;
        }

        /// <summary>
        /// 为命令Header4WithMultiNumberlist创建参数对象
        /// </summary>
        /// <returns>创建的参数对象</returns>
        //[System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public static HeaderFormatCommandParameter CreateHeader4WithMultiNumberlist()
        {
            HeaderFormatCommandParameter p = new HeaderFormatCommandParameter();
            p.ParagraphOutlineLevel = 3;
            p.ParagraphMultiLevel = true;
            p.ParagraphListStyle = Drawing.ParagraphListStyle.ListNumberStyle;
            p.LeftIndent = 300;
            p.FirstLineIndent = -100;
            p.FontSize = 12;
            p.LineSpacingStyle = LineSpacingStyle.SpaceMultiple;
            p.LineSpacing = 1.57f;
            p.FontStyle = FontStyle.Bold;
            return p;
        }

        /// <summary>
        /// 为命令Header5WithMultiNumberlist创建参数对象
        /// </summary>
        /// <returns>创建的参数对象</returns>
        //[System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public static HeaderFormatCommandParameter CreateHeader5WithMultiNumberlist()
        {
            HeaderFormatCommandParameter p = new HeaderFormatCommandParameter();
            p.ParagraphOutlineLevel = 4;
            p.ParagraphMultiLevel = true;
            p.ParagraphListStyle = Drawing.ParagraphListStyle.ListNumberStyle;
            p.LeftIndent = 400;
            p.FirstLineIndent = -100;
            p.FontSize = 10;
            p.LineSpacingStyle = LineSpacingStyle.SpaceMultiple;
            p.LineSpacing = 1.57f;
            p.FontStyle = FontStyle.Bold;
            return p;
        }

        /// <summary>
        /// 为命令Header6WithMultiNumberlist创建参数对象
        /// </summary>
        /// <returns>创建的参数对象</returns>
        //[System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public static HeaderFormatCommandParameter CreateHeader6WithMultiNumberlist()
        {
            HeaderFormatCommandParameter p = new HeaderFormatCommandParameter();
            p.ParagraphOutlineLevel = 5;
            p.ParagraphMultiLevel = true;
            p.ParagraphListStyle = Drawing.ParagraphListStyle.ListNumberStyle;
            p.LeftIndent = 500;
            p.FirstLineIndent = -100;
            p.FontSize = 8;
            p.LineSpacingStyle = LineSpacingStyle.SpaceMultiple;
            p.LineSpacing = 1.33f;
            p.FontStyle = FontStyle.Bold;
            return p;
        }

        /// <summary>
        /// 初始化对象
        /// </summary>
        public HeaderFormatCommandParameter()
        {
        }

        private bool _VisibleInDirectory = true;
        /// <summary>
        /// 在目录中是否可见
        /// </summary>
        //[System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public bool VisibleInDirectory
        {
            get
            {
                return _VisibleInDirectory; 
            }
            set
            {
                _VisibleInDirectory = value; 
            }
        }

        private int _ParagraphOutlineLevel = 1;
        /// <summary>
        /// 段落大纲层数
        /// </summary>
        //[System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public int ParagraphOutlineLevel
        {
            get { return _ParagraphOutlineLevel; }
            set { _ParagraphOutlineLevel = value; }
        }

        private bool _ParagraphMultiLevel = false;
        /// <summary>
        /// 是否多层
        /// </summary>
        //[System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public bool ParagraphMultiLevel
        {
            get { return _ParagraphMultiLevel; }
            set { _ParagraphMultiLevel = value; }
        }

        private ParagraphListStyle _ParagraphListStyle = ParagraphListStyle.ListNumberStyle;
        /// <summary>
        /// 段落列表样式
        /// </summary>
        //[System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public ParagraphListStyle ParagraphListStyle
        {
            get { return _ParagraphListStyle; }
            set { _ParagraphListStyle = value; }
        }

        private float _LeftIndent = 0;
        /// <summary>
        /// 段落左缩进量
        /// </summary>
        //[System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public float LeftIndent
        {
            get { return _LeftIndent; }
            set { _LeftIndent = value; }
        }

        private float _FirstLineIndent = 0;
        /// <summary>
        /// 首行缩进
        /// </summary>
        //[System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public float FirstLineIndent
        {
            get { return _FirstLineIndent; }
            set { _FirstLineIndent = value; }
        }

        private string _FontName = null;
        /// <summary>
        /// 字体名称
        /// </summary>
        //[System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public string FontName
        {
            get { return _FontName; }
            set { _FontName = value; }
        }

        private float _FontSize = 20;
        /// <summary>
        /// 字体大小
        /// </summary>
        //[System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public float FontSize
        {
            get { return _FontSize; }
            set { _FontSize = value; }
        }

        private FontStyle _FontStyle = FontStyle.Bold;
        /// <summary>
        /// 字体样式
        /// </summary>
        //[System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public FontStyle FontStyle
        {
            get { return _FontStyle; }
            set { _FontStyle = value; }
        }

        private LineSpacingStyle _LineSpacingStyle = LineSpacingStyle.SpaceSingle;
        /// <summary>
        /// 行间距样式
        /// </summary>
        //[System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public LineSpacingStyle LineSpacingStyle
        {
            get { return _LineSpacingStyle; }
            set { _LineSpacingStyle = value; }
        }

        private float _LineSpacing = 0;
        /// <summary>
        /// 行间距
        /// </summary>
        //[System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public float LineSpacing
        {
            get { return _LineSpacing; }
            set { _LineSpacing = value; }
        }
    }
}
