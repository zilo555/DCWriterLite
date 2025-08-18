#if NET9_0

// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using Microsoft.JSInterop.Infrastructure;
using static Microsoft.AspNetCore.Internal.LinkerFlags;

namespace Microsoft.JSInterop;

/// <summary>
/// Extensions for <see cref="IJSRuntime"/>.
/// </summary>
public static class JSRuntimeExtensions
{
    public static async ValueTask InvokeVoidAsync(this IJSRuntime jsRuntime, string identifier, params object?[]? args)
    {
        ArgumentNullException.ThrowIfNull(jsRuntime);

        await jsRuntime.InvokeAsync<IJSVoidResult>(identifier, args);
    }
}
#endif