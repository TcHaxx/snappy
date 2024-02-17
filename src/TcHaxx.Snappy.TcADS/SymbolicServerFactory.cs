using Microsoft.Extensions.Logging;
using TcHaxx.Snappy.Common.RPC;
using TcHaxx.Snappy.TcADS.Symbols;

namespace TcHaxx.Snappy.TcADS;

public class SymbolicServerFactory(IRpcMethodDescriptor rpcMethodDescriptor, ILogger? logger) : ISymbolicServerFactory
{
    private readonly IRpcMethodDescriptor _rpcMethodDescriptor = rpcMethodDescriptor;
    private readonly ILogger? _logger = logger;

    public ISymbolicServer CreateSymbolicServer(ushort port, string portName)
    {
        return new SymbolicServer(port, portName, new SymbolFactory(_rpcMethodDescriptor, _logger), _logger);
    }
}
