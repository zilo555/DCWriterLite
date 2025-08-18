#if NET9_0

global using System.Threading;
global using System.Threading.Tasks;
global using System.IO;
global using System.Collections;
global using System.Collections.Generic;

using System;
using System.Text.Json;
using System.Runtime.InteropServices.JavaScript;
using Microsoft.JSInterop;
//using System.ComponentModel;
using Microsoft.JSInterop.Infrastructure;
using Microsoft.JSInterop.WebAssembly;
//using System.Buffers;
//using System.Globalization;
//using System.Runtime.Versioning;

namespace DCSoft
{
    [System.Reflection.Obfuscation(Exclude = true , ApplyToMembers = true )]
    internal static partial class InternalJSImportMethods
    {
        [JSImport("Blazor._internal.getApplicationEnvironment", "blazor-internal")]
        public static partial string GetApplicationEnvironmentCore();
    }

    [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
    public sealed partial class WASMJSRuntime : WebAssemblyJSRuntime
    {
        [JSInvokable]
        public static void NotifyLocationChanged(string uri, string? state, bool isInterceptedLink)
        {
            return;
        }

        [JSInvokable]
        public static async System.Threading.Tasks.ValueTask<bool> NotifyLocationChangingAsync(string uri, string? state, bool isInterceptedLink)
        {
            return false;
        }

        [JSInvokable]
        public static void ApplyHotReloadDelta(string moduleIdString, byte[] metadataDelta, byte[] ilDelta, byte[] pdbBytes)
        {
        }
        [JSInvokable]
        public static string GetApplyUpdateCapabilities()
        {
            return null;
        }

        internal static readonly WASMJSRuntime Instance = new();
        private WASMJSRuntime()
        {
        }

        public JsonSerializerOptions ReadJsonSerializerOptions() => JsonSerializerOptions;

        [JSExport]
        public static string? InvokeDotNet(
            string? assemblyName,
            string methodIdentifier,
            [JSMarshalAs<JSType.Number>] long dotNetObjectId,
            string argsJson)
        {
            var callInfo = new DotNetInvocationInfo(assemblyName, methodIdentifier, dotNetObjectId, callId: null);
            return DotNetDispatcher.Invoke(Instance, callInfo, argsJson);
        }

        [JSExport]
        public static void EndInvokeJS(string argsJson)
        {
            WebAssemblyCallQueue.Schedule(argsJson, static argsJson =>
            {
                DotNetDispatcher.EndInvokeJS(Instance, argsJson);
            });
        }

        [JSExport]
        public static void BeginInvokeDotNet(string? callId, string assemblyNameOrDotNetObjectId, string methodIdentifier, string argsJson)
        {
            string? assemblyName;
            long dotNetObjectId;
            if (char.IsDigit(assemblyNameOrDotNetObjectId[0]))
            {
                dotNetObjectId = long.Parse(assemblyNameOrDotNetObjectId);
                assemblyName = null;
            }
            else
            {
                dotNetObjectId = default;
                assemblyName = assemblyNameOrDotNetObjectId;
            }

            var callInfo = new DotNetInvocationInfo(assemblyName, methodIdentifier, dotNetObjectId, callId);
            WebAssemblyCallQueue.Schedule((callInfo, argsJson), static state =>
            {
                // This is not expected to throw, as it takes care of converting any unhandled user code
                // exceptions into a failure on the JS Promise object.
                DotNetDispatcher.BeginInvokeDotNet(Instance, state.callInfo, state.argsJson);
            });
        }
         

        [JSExport]
        //[SupportedOSPlatform("browser")]
        private static void ReceiveByteArrayFromJS(int id, byte[] data)
        {
            DotNetDispatcher.ReceiveByteArray(Instance, id, data);
        }
    }
    internal static class WebAssemblyCallQueue
    {
        private static bool _isCallInProgress;
        private static readonly System.Collections.Generic.Queue<Action> _pendingWork = new();
        public static void Schedule<T>(T state, Action<T> callback)
        {
            if (_isCallInProgress)
            {
                _pendingWork.Enqueue(() => callback(state));
            }
            else
            {
                _isCallInProgress = true;
                callback(state);

                // Now run any queued work items
                while (_pendingWork.TryDequeue(out var nextWorkItem))
                {
                    nextWorkItem();
                }

                _isCallInProgress = false;
            }
        }
    }
}

#endif