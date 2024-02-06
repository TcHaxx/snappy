namespace TcHaxx.Snappy.Common.RPC.Attributes;

/// <summary>
/// Defines an alias name for a RPC method.
/// </summary>
[AttributeUsage(AttributeTargets.Method)]
public class AliasAttribute : Attribute
{
    public string AliasName;

    public AliasAttribute(string aliasName)
    {
        AliasName = aliasName;
    }
}
