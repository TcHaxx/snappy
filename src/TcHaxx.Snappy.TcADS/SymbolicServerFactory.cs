using Microsoft.Extensions.Logging;
using TcHaxx.Snappy.Common.RPC;
using TcHaxx.Snappy.TcADS.Symbols;

namespace TcHaxx.Snappy.TcADS;

public class SymbolicServerFactory : ISymbolicServerFactory
{
    private readonly IRpcMethodDescriptor _RpcMethodDescriptor;
    private readonly ILogger? _Logger;

    public SymbolicServerFactory(IRpcMethodDescriptor rpcMethodDescriptor, ILogger? logger)
    {
        _RpcMethodDescriptor = rpcMethodDescriptor;
        _Logger = logger;
    }
    public ISymbolicServer CreateSymbolicServer(ushort port, string portName)
    {
        return new SymbolicServer(port, portName, new SymbolFactory(_RpcMethodDescriptor, _Logger), _Logger);
    }
}
