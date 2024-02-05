using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TcHaxx.Snappy.Common.Verify;

namespace TcHaxx.Snappy.Common.RPC;

public class RpcMethodDescriptor : IRpcMethodDescriptor
{
    public RpcMethodDescriptor()
    {

    }
    public RpcMethodDescription GetRpcMethodDescription()
    {
        return Transform();
    }


    private RpcMethodDescription Transform()
    {
        var method = typeof(IVerifyMethod).GetMethod(nameof(IVerifyMethod.Verify)) ??
            throw new Exception($"Method \"{nameof(IVerifyMethod.Verify)}\" not found.");
        var parameters = method.GetParameters() ??
            throw new Exception($"Expected method \"{nameof(IVerifyMethod.Verify)}\" to have parameters");

        var retVal = method.ReturnParameter;

        return new RpcMethodDescription(method.Name, parameters, retVal);
    }
}
