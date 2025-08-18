#if NET9_0

// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Microsoft.JSInterop.Infrastructure;

[System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
public readonly struct DotNetInvocationResult
{
    internal DotNetInvocationResult(Exception exception, string? errorKind)
    {
        ResultJson = default;
        Exception = exception ?? throw new ArgumentNullException(nameof(exception));
        ErrorKind = errorKind;
        Success = false;
    }
    internal DotNetInvocationResult(string? resultJson)
    {
        ResultJson = resultJson;
        Exception = default;
        ErrorKind = default;
        Success = true;
    }
    public Exception? Exception { get; }
    public string? ErrorKind { get; }
    public string? ResultJson { get; }
    public bool Success { get; }
}
#endif