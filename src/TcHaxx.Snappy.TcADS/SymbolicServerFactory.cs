using Microsoft.Extensions.Logging;
using TcHaxx.Snappy.TcADS.Symbols;

namespace TcHaxx.Snappy.TcADS;

public class SymbolicServerFactory : ISymbolicServerFactory
{
    private readonly ILogger? _Logger;

    public SymbolicServerFactory(ILogger? logger)
    {
        this._Logger = logger;
    }
    public ISymbolicServer CreateSymbolicServer(ushort port, string portName)
    {
        return new SymbolicServer(port, portName, new SymbolFactory(new RpcMethodProvider(_Logger), _Logger), _Logger);
    }
}
