using System;
using System.Text;
using System.Collections.Generic;
using DCSoft.Writer.Controls;

namespace DCSoft
{

    /// <summary>
    /// 命令行输出界面
    /// </summary>
    public class DCConsole
    {
        public static DCConsole Default = new DCConsole();

        public DCConsole()
        {

        }

        public virtual void WriteLine(string value)
        {
        }

        public virtual void WriteLineError( string value )
        {
        }
        /// <summary>
        /// 输出加载文件完成的调试信息
        /// </summary>
        /// <param name="size">加载的数据字节数</param>
        public void DebugLoadFileComplete(int size)
        {
            this.WriteLine(string.Format(
                    DCSR.LoadComplete_Size,
                    DCSoft.Writer.WriterUtilsInner.FormatByteSize(size)));
        }
    }
}
