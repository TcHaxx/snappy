using System.Reflection;

namespace TcHaxx.Snappy.Common.RPC
{
    public record RpcMethodDescription(string Name, IEnumerable<ParameterInfo> Parameters, ParameterInfo ReturnValue);
}
