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
    public IEnumerable<RpcMethodDescription> GetRpcMethodDescription();

    /// <summary>
    /// Registers a <see cref="IRpcMethodMarker"/> implementation with <see cref="IRpcMethodDescriptor"/>.
    /// </summary>
    /// <param name="rpcMethod"></param>
    public void Register(IRpcMethodMarker rpcMethod);
}
