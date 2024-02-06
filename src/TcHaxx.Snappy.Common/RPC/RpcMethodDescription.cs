using System.Reflection;
using TcHaxx.Snappy.Common.Verify;

namespace TcHaxx.Snappy.Common.RPC;

public record RpcMethodDescription(MethodInfo Method, IEnumerable<ParameterInfo> Parameters, ParameterInfo ReturnValue, IVerifyMethod RpcInvocableMethod);
