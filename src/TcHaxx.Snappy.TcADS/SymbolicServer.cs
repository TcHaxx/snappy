using System.Text;
using Microsoft.Extensions.Logging;
using TcHaxx.Snappy.Common.Verify;
using TwinCAT.Ads;
using TwinCAT.Ads.Server;
using TwinCAT.TypeSystem;
using ISymbolFactory = TcHaxx.Snappy.TcADS.Symbols.ISymbolFactory;

namespace TcHaxx.Snappy.TcADS;

internal class SymbolicServer : AdsSymbolicServer, ISymbolicServer
{
    private readonly ISymbolFactory _SymbolFactory;

    internal SymbolicServer(ushort port, string portName, ISymbolFactory symbolFactory, ILogger? logger)
        : base(port, portName, logger)
    {
        _SymbolFactory = symbolFactory;
    }

    /// <inheritdoc cref="AdsServer.ConnectServerAndWaitAsync(CancellationToken)"/>

    internal new Task<AdsErrorCode> ConnectServerAndWaitAsync(CancellationToken cancel)
    {
        return base.ConnectServerAndWaitAsync(cancel);
    }

    protected override AdsErrorCode OnGetValue(ISymbol symbol, out object? value)
    {
        value = null;
        return AdsErrorCode.DeviceServiceNotSupported;
    }

    protected override AdsErrorCode OnReadRawValue(ISymbol symbol, Span<byte> span)
    {
        return AdsErrorCode.DeviceServiceNotSupported;
    }

    protected override AdsErrorCode OnSetValue(ISymbol symbol, object value, out bool valueChanged)
    {
        valueChanged = false;
        return AdsErrorCode.DeviceServiceNotSupported;
    }

    protected override AdsErrorCode OnWriteRawValue(ISymbol symbol, ReadOnlySpan<byte> span)
    {
        return AdsErrorCode.DeviceServiceNotSupported;
    }

    /// <summary>
    /// Handler function when the SymbolicTestServer gets connected.
    /// </summary>
    protected override void OnConnected()
    {
        _SymbolFactory.AddSymbols(base.symbolFactory);
        base.OnConnected();
    }

    protected override AdsErrorCode OnRpcInvoke(IInterfaceInstance structInstance, IRpcMethod method, object[] values, out object? returnValue)
    {
        var iDataType = structInstance.DataType;
        if (iDataType is null)
        {
            returnValue = null;
            return AdsErrorCode.DeviceInvalidContext;
        }
        return _SymbolFactory.InvokeRpcMethod(iDataType, values, out returnValue);
    }
}
