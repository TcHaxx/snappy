using Microsoft.Extensions.Logging;

namespace TcHaxx.Snappy.TcADS;

public interface ISymbolicServerFactory
{
    ISymbolicServer CreateSymbolicServer(ushort port, string portName);
}
