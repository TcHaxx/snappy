using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwinCAT.TypeSystem;

namespace TcHaxx.Snappy.TcADS.Symbols
{
    internal interface IRpcMethodProvider
    {
        RpcMethodCollection GetRpcMethodCollection();
    }
}
