using System.Data;
using System.Reflection;
using TcHaxx.Snappy.Common.RPC.Attributes;

namespace TcHaxx.Snappy.Common.RPC;

public class RpcMethodDescriptor : IRpcMethodDescriptor
{
    private readonly Queue<IRpcMethodMarker> _verifyMethods = new();

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

    public void Register(IRpcMethodMarker rpcMethod)
    {
        _verifyMethods.Enqueue(rpcMethod);
    }
    private static RpcMethodDescription Transform(IRpcMethodMarker rpcMethod)
    {
        var method = GetMethodInfo(rpcMethod);

        var parameters = method.GetParameters() ??
            throw new RpcMethodTransformException($"Expected method \"{method.Name}\" to have parameters");

        var retVal = method.ReturnParameter;

        var alias = GetAliasAttriubte(rpcMethod);
        return new RpcMethodDescription(method, parameters, retVal, rpcMethod, alias);
    }

    private static MethodInfo GetMethodInfo(IRpcMethodMarker rpcMethod)
    {
        var typeName = rpcMethod.GetType().Name;

        var methodInfos = GetMethodInfos(rpcMethod);

        if (methodInfos is null || methodInfos.Length == 0)
        {
            throw new RpcMethodTransformException($"No RPC method found in type \"{typeName}\".");
        }

        if (methodInfos.Length > 1)
        {
            throw new RpcMethodTransformException($"Only one RPC method supported per type ({typeName}).");
        }

        return methodInfos[0];
    }

    private static string? GetAliasAttriubte(IRpcMethodMarker rpcMethod)
    {
        var aliasAttribute = rpcMethod.GetType()
            .GetMethods()
            .Select(x => x.GetCustomAttribute<AliasAttribute>())
            .FirstOrDefault(x => x is not null);
        return aliasAttribute?.AliasName;
    }

    private static MethodInfo[]? GetMethodInfos(IRpcMethodMarker rpcMethod)
    {
        var interfaces = rpcMethod.GetType().GetInterfaces();
        var rpcInterfaces = interfaces
            .FirstOrDefault(i => typeof(IRpcMethodMarker).IsAssignableFrom(i) && i.GetInterfaces().Length == 1 && i.GetInterfaces().Contains(typeof(IRpcMethodMarker)));

        return rpcInterfaces?.GetMethods();
    }
}
