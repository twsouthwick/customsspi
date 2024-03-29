﻿#if !NET7_0_OR_GREATER

using Microsoft.Data.SqlClient;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CustomSSPILibrary;

internal sealed class ReflectedNegotiateStateSSPIContextProvider : SSPIContextProvider, IDisposable
{
#if NETFRAMEWORK
    private FrameworkReflectedNegotiateState? _negotiate;
#else
    private NetCoreReflectedNegotiateState? _negotiate;
#endif

    public void Dispose()
    {
        _negotiate?.Dispose();
    }

    protected override IMemoryOwner<byte> GenerateSspiClientContext(ReadOnlyMemory<byte> input)
    {
        _negotiate ??= new("NTLM", new NetworkCredential(AuthenticationParameters.UserId, AuthenticationParameters.Password), AuthenticationParameters.ServerName);

        return new ArrayMemoryOwner(_negotiate.GetOutgoingBlob(input.Span.ToArray()));
    }
}

#endif
