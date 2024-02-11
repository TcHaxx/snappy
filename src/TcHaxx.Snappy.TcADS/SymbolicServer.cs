using Microsoft.Extensions.Logging;
using TwinCAT.Ads;
using TwinCAT.Ads.Server;
using TwinCAT.TypeSystem;
using ISymbolFactory = TcHaxx.Snappy.TcADS.Symbols.ISymbolFactory;

namespace TcHaxx.Snappy.TcADS;

internal class SymbolicServer : AdsSymbolicServer, ISymbolicServer
{
    private readonly ISymbolFactory _SymbolFactory;
    private readonly ILogger? _Logger;

    internal SymbolicServer(ushort port, string portName, ISymbolFactory symbolFactory, ILogger? logger)
        : base(port, portName, logger)
    {
        _SymbolFactory = symbolFactory;
        _Logger = logger;
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
        _Logger?.LogInformation("Adding symbols ...");
        _SymbolFactory.AddSymbols(base.symbolFactory);
        _Logger?.LogInformation("done.");
        base.OnConnected();
        _Logger?.LogInformation("Waiting for RPC requests...");
    }

    protected override AdsErrorCode OnRpcInvoke(IInterfaceInstance structInstance, IRpcMethod method, object[] values, out object? returnValue)
    {
        var iDataType = structInstance.DataType;
        if (iDataType is null)
        {
            returnValue = null;
            _Logger?.LogError("{OnRpcInvoke}: {IDataType} is null", nameof(OnRpcInvoke), nameof(IDataType));
            return AdsErrorCode.DeviceInvalidContext;
        }
        _Logger?.LogInformation("{OnRpcInvoke}: Invoking method {IRpcMethod} of {IDataTypeFullName}", nameof(OnRpcInvoke), method, iDataType.FullName);
        return _SymbolFactory.InvokeRpcMethod(iDataType, values, out returnValue);
    }
}
