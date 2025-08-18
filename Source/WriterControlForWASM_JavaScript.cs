using DCSoft.Common;
using System.Collections.Generic;
using DCSoft.Writer.Dom;


namespace DCSoft.WASM
{
    // 和JavaScript相关的代码
    partial class WriterControlForWASM
    {
        [JSInvokable]
        public static void SetDefaultFont(int startCharCode, int endCharCode, string strFontName)
        {
            DCSoft.Writer.Dom.CharacterMeasurer.SetDefaultFont((char)startCharCode, (char)endCharCode, strFontName);// "Microsoft Sans Serif");
        }
        
        /// <summary>
        /// 格式化日期值
        /// </summary>
        /// <param name="dtm">日期值</param>
        /// <param name="strFormat">格式化字符串</param>
        /// <returns>时间日期字符串</returns>
        [JSInvokable]
        public static string FormatDateTime( DateTime dtm , string strFormat )
        {
            return dtm.ToString(strFormat);
        }
         
        private static List<WeakReference<WriterControlForWASM>> _ControlInstances = new List<WeakReference<WriterControlForWASM>>();

        [JSInvokable]
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public static DotNetObjectReference<WriterControlForWASM> CreateWriterControlForWASM(string containerElementID)
        {
            var ctl = new WriterControlForWASM();
            ctl._ReferenceAtJS = DotNetObjectReference.Create<WriterControlForWASM>(ctl);
            ctl._ContainerElementID = containerElementID;
            //WASMJsonConvert.AddConverter();
            _ControlInstances.Add(new WeakReference<WriterControlForWASM>(ctl));
            return ctl._ReferenceAtJS;
        }
        [JSInvokable]
        public static DotNetObjectReference<WriterControlForWASM> GetWriterControlForWASM(string containerElementID)
        {
            for(var iCount = _ControlInstances.Count -1;iCount >=0;iCount --)
            {
                WriterControlForWASM ctl = null;
                if(_ControlInstances[iCount].TryGetTarget( out ctl ))
                {
                    if( ctl.IsDisposed())
                    {
                        _ControlInstances[iCount].SetTarget(null);
                        _ControlInstances.RemoveAt(iCount);
                    }
                    else if(ctl._ContainerElementID == containerElementID)
                    {
                        return ctl._ReferenceAtJS;
                    }
                }
            }
            return null;
        }
        internal object JS_InvokeInstance(string methodName, params object[] paramters)
        {
            if (methodName == null || methodName.Length == 0)
            {
                throw new ArgumentNullException("methodName");
            }
            var js2 = WASMJSRuntime.Instance;
            var ps2 = new object[ paramters == null ? 1 : paramters.Length + 1];
            ps2[0] = this._ContainerElementID;
            if (paramters != null)
            {
                Array.Copy(paramters, 0, ps2, 1, paramters.Length);
            }
            var result = js2.Invoke<object>(methodName, ps2);
            return result;
        }

        internal T JS_InvokeInstance<T>(string methodName, params object[] paramters)
        {
            if (methodName == null || methodName.Length == 0)
            {
                throw new ArgumentNullException("methodName");
            }
            var js2 = WASMJSRuntime.Instance;
            var ps2 = new object[paramters.Length + 1];
            ps2[0] = this._ContainerElementID;
            Array.Copy(paramters, 0, ps2, 1, paramters.Length);
            var result = js2.Invoke<T>(methodName, ps2);
            return result;
        }

    }
}
