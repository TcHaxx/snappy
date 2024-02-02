using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwinCAT.TypeSystem;

namespace TcHaxx.Snappy.TcADS.Symbols
{
    internal class RpcMethodProvider : IRpcMethodProvider
    {
        private readonly ILogger? _Logger;

        public RpcMethodProvider(ILogger? logger)
        {
            _Logger = logger;
        }

        public RpcMethodCollection GetRpcMethodCollection()
        {
            return new RpcMethodCollection();
        }
    }
}
