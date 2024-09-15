using System.Reflection;

namespace TcHaxx.Snappy.Common.RPC;

public record RpcMethodDescription(MethodInfo Method, IEnumerable<ParameterInfo> Parameters, ParameterInfo ReturnValue, IRpcMethodMarker RpcInvocableMethod, string? Alias)
{
    public string InstanceName => RpcInvocableMethod.GetType().FullName!;
}
