namespace TcHaxx.Snappy.Common.RPC;

/// <summary>
/// Describes an descriptor for RPC methods.
/// </summary>
public interface IRpcMethodDescriptor
{
    /// <summary>
    /// Gets the description of the parameters and return value.
    /// </summary>
    /// <returns></returns>
    RpcMethodDescription GetRpcMethodDescription();
}
