using TwinCAT.Ads;

namespace TcHaxx.Snappy.TcADS;

public interface ISymbolicServer : IDisposable
{
    Task<AdsErrorCode> ConnectServerAndWaitAsync(CancellationToken cancel);
}
