
using DCSoft.Writer.Dom;
using System;
using System.Collections.Generic;
using DCSoft.Common;

namespace DCSoft.Writer.Controls
{
    public static class WASMEnvironment
    {
        private static DCSoft.WASM.WriterControlForWASM _JSProvider = null;
        /// <summary>
        /// 前端JS功能提供者
        /// </summary>
        public static DCSoft.WASM.WriterControlForWASM JSProvider
        {
            get
            {
                return _JSProvider;
            }
        }
        public static void SetJSProivder(DCSoft.WASM.WriterControlForWASM p)
        {
            if (p == null)
            {
                //_JSProvider = new NullJSProvider();
            }
            else
            {
                _JSProvider = p;
            }
        }
         
    }
}

