namespace TcHaxx.Snappy.Common.RPC.Attributes;

/// <summary>
/// Defines an alias name for a RPC method.
/// </summary>
[AttributeUsage(AttributeTargets.Method)]
public class AliasAttribute(string aliasName) : Attribute
{
    public string AliasName { get; private set; } = aliasName;
}
