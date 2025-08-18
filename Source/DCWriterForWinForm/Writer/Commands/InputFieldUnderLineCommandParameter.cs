using System;
using System.Collections.Generic;
using System.Text;

using DCSoft.Writer.Dom;


namespace DCSoft.Writer.Commands
{
    /// <summary>
    /// 用下边框方式来实现下划线的命令参数
    /// </summary>
    /// <remarks></remarks>
    //[System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = false)]
    //[DCSoft.Common.DCPublishAPI]
    //[System.Runtime.InteropServices.ComVisible(false)]
    public partial class InputFieldUnderLineCommandParameter
    {
        /// <summary>
        /// 获取或设置要进行的操作是加下划线还是去掉下划线
        /// </summary>
        //[System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public bool IsAddLine { get; set; }

        /// <summary>
        /// 获取或设置下划线的颜色
        /// </summary>
        //[System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public DCSystem_Drawing.Color InputFieldUnderLineColor { get; set; }

        /// <summary>
        /// 获取或设置下划线的线宽
        /// </summary>
        //[System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public float InputFieldUnderLineWidth { get; set; }


        /// <summary>
        /// 获取或设置下划线的样式
        /// </summary>
        //[System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public DashStyle InputFieldUnderLineStyle { get; set; }


        ///构造函数
        public InputFieldUnderLineCommandParameter()
        {
            this.InputFieldUnderLineColor = DCSystem_Drawing.Color.Black;
            this.InputFieldUnderLineWidth = 1;
            this.InputFieldUnderLineStyle = DashStyle.Solid;
            this.IsAddLine = false;
        }
    }
}
