using TcHaxx.Snappy.Common.Verify;

namespace TcHaxx.Snappy.Common.RPC;

public class RpcMethodDescriptor : IRpcMethodDescriptor
{
    readonly Queue<IVerifyMethod> _verifyMethods = new();

    public RpcMethodDescriptor()
    {
    }

    public IEnumerable<RpcMethodDescription> GetRpcMethodDescription()
    {
        while (_verifyMethods.Count > 0)
        {
            yield return Transform(_verifyMethods.Dequeue());
        }
    }

    public void Register(IVerifyMethod rpcVerifyMethod)
    {
        _verifyMethods.Enqueue(rpcVerifyMethod);
    }

    private RpcMethodDescription Transform(IVerifyMethod rpcVerifyMethod)
    {
        var method = rpcVerifyMethod.GetType().GetMethod(nameof(IVerifyMethod.Verify)) ??
            throw new RpcMethodTransformException($"Method \"{nameof(IVerifyMethod.Verify)}\" not found.");
        var parameters = method.GetParameters() ??
            throw new RpcMethodTransformException($"Expected method \"{nameof(IVerifyMethod.Verify)}\" to have parameters");

        var retVal = method.ReturnParameter;

        return new RpcMethodDescription(method, parameters, retVal, rpcVerifyMethod);
    }
}
