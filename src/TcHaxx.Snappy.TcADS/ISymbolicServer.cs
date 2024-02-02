using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwinCAT.Ads;

namespace TcHaxx.Snappy.TcADS;

public interface ISymbolicServer : IDisposable
{
    Task<AdsErrorCode> ConnectServerAndWaitAsync(CancellationToken cancel);
}
