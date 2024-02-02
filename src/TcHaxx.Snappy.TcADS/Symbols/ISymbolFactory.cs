using TwinCAT.Ads.Server.TypeSystem;

namespace TcHaxx.Snappy.TcADS.Symbols;

internal interface ISymbolFactory
{
    void AddSymbols(ServerSymbolFactory? serverSymbolFactory);
}
