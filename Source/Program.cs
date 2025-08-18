//using DCSoft.WASM;
global using System;
global using DCSystem_Drawing;
global using DCSystem_Drawing.Drawing2D;
global using DCSystem_Drawing.Printing;
global using DCSystemXml;

using System.Threading.Tasks;
using System.Reflection;
using System.Collections.Specialized;
using DCSoft.Writer.Dom;
using System.Text;

[assembly: AssemblyMetadata("IsTrimmable", "false")]

namespace DCSoft.WASM
{

    [System.Runtime.Versioning.SupportedOSPlatform("browser")]
    internal class Prograss
    {
        public static async Task Main(string[] args)
        {
            var strBasePath = InternalJSImportMethods.GetApplicationEnvironmentCore();
            await JavaScriptMethods.ImportJSModuleAsync(strBasePath);
            WASMStarter.StartModules();
            DCSoft.TrueTypeFontSnapshort.SetGlobalParentNames(JavaScriptMethods.GetFontNames());
            JavaScriptMethods.StartGlobal();
        }
    }
}
