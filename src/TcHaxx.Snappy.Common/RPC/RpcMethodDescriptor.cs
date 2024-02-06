using TcHaxx.Snappy.Common.Verify;

namespace TcHaxx.Snappy.Common.RPC;

public class RpcMethodDescriptor : IRpcMethodDescriptor
{
    Queue<IVerifyMethod> _VerifyMethods = new();

    public RpcMethodDescriptor()
    {
    }

    public IEnumerable<RpcMethodDescription> GetRpcMethodDescription()
    {
        while (_VerifyMethods.Count > 0)
        {
            yield return Transform(_VerifyMethods.Dequeue());
        }
        yield break;
    }

    public void Register(IVerifyMethod rpcVerifyMethod)
    {
        _VerifyMethods.Enqueue(rpcVerifyMethod);
    }

    private RpcMethodDescription Transform(IVerifyMethod rpcVerifyMethod)
    {
        var method = rpcVerifyMethod.GetType().GetMethod(nameof(IVerifyMethod.Verify)) ??
            throw new Exception($"Method \"{nameof(IVerifyMethod.Verify)}\" not found.");
        var parameters = method.GetParameters() ??
            throw new Exception($"Expected method \"{nameof(IVerifyMethod.Verify)}\" to have parameters");

        var retVal = method.ReturnParameter;

        return new RpcMethodDescription(method, parameters, retVal, rpcVerifyMethod);
    }
}
