using System;
using System.Collections.Generic;
using System.Text;
//using System.Drawing ;
using System.IO ;
using System.ComponentModel ;
using DCSoft.Drawing;
using DCSoft.Writer.Controls;

namespace DCSoft.Writer
{
    /// <summary>
    /// 标准图标图片对象列表
    /// </summary>
    /// <remarks>编制 袁永福</remarks>
    public static class DCStdImageList
    {
        private const int _MaxImageCount = (int)DCStdImageKey.Max;
        static DCStdImageList()
        {
            _StdImages = new Bitmap[_MaxImageCount];
            _StdImages[(int)DCStdImageKey.CheckBoxChecked] = WriterResourcesCore.CheckBoxChecked;
            _StdImages[(int)DCStdImageKey.CheckBoxUnchecked] = WriterResourcesCore.CheckBoxUnChecked;
            _StdImages[(int)DCStdImageKey.RadioBoxChecked] = WriterResourcesCore.RadioChecked;
            _StdImages[(int)DCStdImageKey.RadioBoxUnchecked] = WriterResourcesCore.RadioUnChecked;
            _StdImages[(int)DCStdImageKey.ParagraphFlagLeftToRight] = WriterResourcesCore.ParagraphFlagLeftToRight;
            _StdImages[(int)DCStdImageKey.Linebreak] = WriterResourcesCore.linebreak;
            _StdImages[(int)DCStdImageKey.DragHandle] = WriterResourcesCore.DragHandle;
            DCDocumentRuleControl.CheckResourceImage();
            for (var iCount = 0; iCount < _StdImages.Length; iCount++)
            {
                if (_StdImages[iCount] != null)
                {
                    _StdImages[iCount].StandardImageIndex = iCount;
                }
            }
        }

        private readonly static Bitmap[] _StdImages = null;

        public static Bitmap GetBitmap( DCStdImageKey index )
        {
            return _StdImages[(int)index];
        }


        /// <summary>
        /// 勾选状态的复选框,必须为16*16的BMP图片格式
        /// </summary>
        public static Bitmap BmpCheckBoxChecked
        {
            get
            {
                return _StdImages[(int)DCStdImageKey.CheckBoxChecked];
            }
        }

        public static Bitmap DragHandle
        {
            get
            {
                return _StdImages[(int)(DCStdImageKey.DragHandle)];
            }
        }

        /// <summary>
        /// 不是勾选状态的复选框,必须为16*16的BMP图片格式,透明色为红色。
        /// </summary>
        public static Bitmap BmpCheckBoxUnchecked
        {
            get
            {
                return _StdImages[(int)DCStdImageKey.CheckBoxUnchecked];
            }
        }

        /// <summary>
        /// 勾选状态的单选框,必须为16*16的BMP图片格式,透明色为红色。
        /// </summary>
        public static Bitmap BmpRadioBoxChecked
        {
            get
            {
                return _StdImages[(int)DCStdImageKey.RadioBoxChecked];
            }
        }

        /// <summary>
        /// 不是勾选状态的单选框,必须为16*16的BMP图片格式,透明色为红色。
        /// </summary>
        public static Bitmap BmpRadioBoxUnchecked
        {
            get
            {
                return _StdImages[(int)DCStdImageKey.RadioBoxUnchecked];
            }
        }

        /// <summary>
        /// 从左到右时的段落符号，必须为9*12的BMP图片格式,透明色为红色。
        /// </summary>
        public static Bitmap BmpParagraphFlagLeftToRight
        {
            get
            {
                return _StdImages[(int)DCStdImageKey.ParagraphFlagLeftToRight];
            }
        }


        /// <summary>
        /// 换行符号,必须为9*12的BMP图片格式,透明色为红色。
        /// </summary>
        public static Bitmap BmpLinebreak
        {
            get
            {
                return _StdImages[(int)DCStdImageKey.Linebreak];
            }
        }
    }

    /// <summary>
    /// 标准图标列表编号
    /// </summary>
    [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
    [System.Text.Json.Serialization.JsonConverter(typeof(System.Text.Json.Serialization.JsonStringEnumConverter))]
    public enum DCStdImageKey
    {
        /// <summary>
        /// 无效编号
        /// </summary>
        None,
        /// <summary>
        /// 勾选状态的复选框,必须为16*16的BMP图片格式,透明色为红色。
        /// </summary>
        CheckBoxChecked,
        /// <summary>
        /// 不是勾选状态的复选框,必须为16*16的BMP图片格式,透明色为红色。
        /// </summary>
        CheckBoxUnchecked,
        /// <summary>
        /// 勾选状态的单选框,必须为16*16的BMP图片格式,透明色为红色。
        /// </summary>
        RadioBoxChecked,
        /// <summary>
        /// 不是勾选状态的单选框,必须为16*16的BMP图片格式,透明色为红色。
        /// </summary>
        RadioBoxUnchecked,
        /// <summary>
        /// 从左到右时的段落符号，必须为9*12的BMP图片格式,透明色为红色。
        /// </summary>
        ParagraphFlagLeftToRight,
        /// <summary>
        /// 换行符号,必须为9*12的BMP图片格式,透明色为红色。
        /// </summary>
        Linebreak,
        /// <summary>
        /// 拖拽内容使用的把柄,必须为13*13的BMP图片格式,透明色为红色。
        /// </summary>
        DragHandle,
        Max
    }
}