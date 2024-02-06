using TwinCAT.Ads;
using TwinCAT.Ads.Server.TypeSystem;
using TwinCAT.TypeSystem;

namespace TcHaxx.Snappy.TcADS.Symbols;

internal interface ISymbolFactory
{
    void AddSymbols(ServerSymbolFactory? serverSymbolFactory);
    AdsErrorCode InvokeRpcMethod(IDataType mappedType, object[] values, out object? returnValue);
}
